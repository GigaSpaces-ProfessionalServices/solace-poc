using GigaSpaces.Core;
using CustomExternalDataSource.ExternalDataSource;
using GigaSpaces.Core.Persistency;

using SolaceSystems.Solclient.Messaging;
using System;
using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json.Linq;
using GigaSpaces.Core.Metadata;
using System.Text;
using System.Linq;
using Piper.Common;

namespace Piper.Processor
{
    public class SolaceExternalDataSource : NHibernateSpaceDataSource
    {

        /// <summary>
        /// MaxAttempts is the number of times that the external data source will attempt to persist data.
        /// </summary>
        int MaxAttempts { get; set; }

        string SpaceName { get; set; }

        // used for publishing to Solace
        string Host { get; set; }
        string UserName { get; set; }

        string Password { get; set; }
        string VpnName { get; set; }

        int ConnectRetries { get; set; }
        bool WithReflectionSerialization { get; set; }

        SolaceSystems.Solclient.Messaging.ISession session;

        IQueue queue;

        // end used for publishing to Solace

        ISpaceProxy? spaceProxy;

        string AssemblyFilePath { get; set; }

        Assembly assembly;

        const bool debuglevel = false;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();


        class MsgInfo
        {
            public bool Acked { get; set; }
            public bool Accepted { get; set; }
            public readonly IMessage Message;

            public MsgInfo(IMessage message)
            {
                Acked = false;
                Accepted = false;
                Message = message;
            }
        }
        public SolaceExternalDataSource()
        {
        }

        public override void Init(Dictionary<string, string> properties)
        {
            base.Init(properties);

            SpaceName = GetProperty("SpaceName");

            MaxAttempts = GetIntProperty("MaxAttempts", 3);

            Host = GetProperty("Solace.Host");

            UserName = GetProperty("Solace.UserName");

            Password = GetProperty("Solace.Password");

            VpnName = GetProperty("Solace.VpnName");

            if (VpnName == null) { VpnName = ""; }

            ConnectRetries = GetIntProperty("Solace.ConnectRetries", -1);
            WithReflectionSerialization = GetBoolProperty("Solace.Serialize.WithReflection", false);

            AssemblyFilePath = GetFileProperty("AssemblyFileName");

            assembly = Assembly.LoadFrom(AssemblyFilePath);

            Logger.Info("SpaceName is: " + SpaceName);
            Logger.Info("MaxAttempts is: " + MaxAttempts);
            Logger.Info("Host is: " + Host);
            Logger.Info("UserName is: " + UserName);
            Logger.Info("VpnName is: " + VpnName);
            Logger.Info("ConnectRetries is: " + ConnectRetries);
            Logger.Info("AssemblyFilePath is: " + AssemblyFilePath);
            Logger.Info("Assemlby.FullName is: " + assembly.FullName);


            solaceInit();
        }
        
        private void solaceInit()
        {
            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);

            try
            {
                IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null);
                Run(context, Host);
            }
            catch (Exception ex)
            {
                Logger.Info("Exception thrown: {0}", ex.Message);
            }

        }

        void Run(IContext context, string host)
        {
            if (context == null)
            {
                throw new ArgumentException("Solace Systems API context Router must be not null.", "context");
            }
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentException("Solace Messaging Router host name must be non-empty.", "host");
            }
            if (string.IsNullOrWhiteSpace(VpnName))
            {
                throw new InvalidOperationException("VPN name must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new InvalidOperationException("Client username must be non-empty.");
            }

            // Create session properties
            // When solace reconnect retries is -1, forever try to connect
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = host,
                VPNName = this.VpnName,
                UserName = this.UserName,
                Password = this.Password,
                ConnectRetries = this.ConnectRetries,
                ReconnectRetries = this.ConnectRetries
            };

            // Connect to the Solace messaging router
            Logger.Info("Connecting as {0}@{1} on {2}...", UserName, VpnName, host);
            // NOTICE HandleSessionEvent as session event handler
            session = context.CreateSession(sessionProps, null, HandleSessionEvent);
            ReturnCode returnCode = session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                Logger.Info("Session successfully connected.");
            }
            else
            {
                Logger.Info("Error connecting, return code: {0}", returnCode);
            }
        }
        void HandleSessionEvent(object sender, SessionEventArgs args)
        {
            // Received a session event
            if (debuglevel)
            {
                Logger.Info("Received session event {0}.", args.ToString());
            }
            switch (args.Event)
            {
                // this is the confirmation
                case SessionEvent.Acknowledgement:
                    if (debuglevel)
                    {
                        Logger.Info("SessionEvent.Acknowledgement {0}.", args.ToString());
                    }
                    break;
                case SessionEvent.RejectedMessageError:
                    if (debuglevel)
                    {
                        Logger.Info("SessionEvent.RejectedMessageError : message record rejected " + args.ResponseCode);
                    }
                    break;
                default:
                    break;
            }
        }

        public override void ExecuteBulk(IList<BulkItem> bulk)
        {
            ExecuteBulk(bulk, 0);
        }
        protected virtual void ExecuteBulk(IList<BulkItem> bulk, int retries)
        {
            JArray jsonArray = new JArray();
            StringBuilder jsonArrayStr = new StringBuilder();
            jsonArrayStr.Append("[");
            try
            {
                //foreach (BulkItem bulkItem in bulk)
                for (int i = 0; i < bulk.Count; i++)
                {
                    // ExecuteBulkItem(bulkItem, retries);
                    if (WithReflectionSerialization)
                    {
                        jsonArray.Add(createJsonResponse(bulk.ElementAt(i)));
                    } else
                    {
                        jsonArrayStr.Append(createJsonResponse3(bulk.ElementAt(i)));
                    }
                    if ((bulk.Count - 1) != i)
                    {
                        jsonArrayStr.Append("},");
                    }
                }
                jsonArrayStr.Append("}]");
            }
            catch (Exception e)
            {
                //  For other execptions retry till max
                if (retries >= MaxAttempts)
                    throw new Exception("Can't execute bulk store.", e);

                if (retries == 1)
                {
                    Logger.Error("Retrying ... " + retries, e);
                }
                ExecuteBulk(bulk, retries + 1);
            }
            //            SendMessage(jsonArray);
            SendMessage(jsonArrayStr.ToString(),jsonArray);
        }

        protected virtual void ExecuteBulkItem(BulkItem bulkItem, int retries)
        {
            JArray jsonArray = new JArray();
            StringBuilder jsonArrayStr = new StringBuilder();
            
            try
            {
                if (WithReflectionSerialization)
                {
                    jsonArray.Add(createJsonResponse(bulkItem));
                }
                else
                {
                    jsonArrayStr.Append("[");
                    jsonArrayStr.Append(createJsonResponse3(bulkItem));
                    jsonArrayStr.Append("]");
                }
                //jsonArrayStr.Append(",");

            }
            catch (Exception e)
            {
                if (retries >= MaxAttempts)
                    throw new Exception("Can't execute bulk store.", e);
                ExecuteBulkItem(bulkItem, retries + 1);
            }
            //            SendMessage(jsonArray);
            SendMessage(jsonArrayStr.ToString(), jsonArray);
        }

        string createJsonResponse3(BulkItem bulkItem)
        {
            setSpaceProxyIfNull();
            StringBuilder mainObjectStr = new StringBuilder();
            IType itemValue = (IType)bulkItem.Item;
            string typeName = bulkItem.Item.GetType().Name;

            mainObjectStr.Append("{");
            mainObjectStr.Append("\"op\":\"");
            mainObjectStr.Append(bulkItem.Operation.ToString());
            mainObjectStr.Append("\",");


            mainObjectStr.Append(itemValue.serializeToJsonString());
            mainObjectStr.Append("]");

            return mainObjectStr.ToString();
        }

        JObject createJsonResponse(BulkItem bulkItem)
        {
            setSpaceProxyIfNull();

            //call Once move to above

            object itemValue = bulkItem.Item;
            if (debuglevel)
            {
                Logger.Info("AssemblyFilePath is: " + AssemblyFilePath);
                Logger.Info("assembly.FullName is: ", assembly.FullName);
                Logger.Info("bulkItem.Item.GetType() is: " + bulkItem.Item.GetType());
                Logger.Info("bulkItem.Item.GetType().FullName is: " + bulkItem.Item.GetType().FullName);
            }
            Type type = assembly.GetType(bulkItem.Item.GetType().FullName);
            PropertyInfo[] propertyInfo = type.GetProperties();

            string[] propertyNames = new string[propertyInfo.Length];
            string[] propertyType = new string[propertyInfo.Length];
            object[] propertyValue = new object[propertyInfo.Length];
            JObject mainObject = new JObject();
            mainObject["op"] = bulkItem.Operation.ToString();
            mainObject["type"] = bulkItem.Item.GetType().Name;
            ISpaceTypeDescriptor spaceTypeDescriptor = spaceProxy.TypeManager.GetTypeDescriptor(bulkItem.Item.GetType());
            mainObject["spaceId"] = spaceTypeDescriptor.IdPropertyName;

            JArray payload = new JArray();

            for (int i = 0; i < propertyInfo.Length; i++)
            {
                JObject fieldDetails = new JObject();
                fieldDetails["columnName"] = propertyInfo[i].Name;
                object itemValTmp = itemValue.GetType().GetProperty(propertyInfo[i].Name).GetValue(itemValue);
                fieldDetails["value"] = new JValue(itemValTmp);
                Type typeTmp = propertyInfo[i].PropertyType;
                if (typeTmp.IsGenericType && typeTmp.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    fieldDetails["columnType"] = Nullable.GetUnderlyingType(typeTmp).Name;
                }
                else
                {
                    fieldDetails["columnType"] = typeTmp.Name;
                }

                propertyNames[i] = propertyInfo[i].Name;
                propertyValue[i] = itemValue.GetType().GetProperty(propertyInfo[i].Name).GetValue(itemValue, null);
                propertyType[i] = propertyInfo[i].PropertyType.Name;
                payload.Add(fieldDetails);
            }
            mainObject.Add("payload", payload);

            return mainObject;
        }

        void setSpaceProxyIfNull()
        {
            if (spaceProxy == null)
            {
                SpaceProxyFactory factory = new SpaceProxyFactory(SpaceName);
                try
                {
                    spaceProxy = factory.Create();
                    //spaceProxy.Count(new object());
                    setSolaceSession();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                    Logger.Info("Space verification failed. Please check & try again...");
                }
            }
        }

        void setSolaceSession()
        {
            // Provision the queue
            string queueName = SpaceName;
            Logger.Info("Attempting to provision the queue '{0}'...", queueName);
            queue = ContextFactory.Instance.CreateQueue(queueName);
            // Set queue permissions to "consume" and access-type to "exclusive"
            EndpointProperties endpointProps = new EndpointProperties()
            {
                Permission = EndpointProperties.EndpointPermission.Consume,
                AccessType = EndpointProperties.EndpointAccessType.Exclusive
            };
            // Provision it, and do not fail if it already exists
            session.Provision(queue, endpointProps,
                ProvisionFlag.IgnoreErrorIfEndpointAlreadyExists | ProvisionFlag.WaitForConfirm, null);
            Logger.Info("Queue '{0}' has been created and provisioned.", queueName);
        }

//        private void SendMessage(JArray data)
        private void SendMessage(string jsonStr, JArray jArray)
        {
            // Create the message
            using (IMessage message = ContextFactory.Instance.CreateMessage())
            {
                // Message's destination is the queue and the message is persistent
                message.Destination = queue;
                message.DeliveryMode = MessageDeliveryMode.Persistent;

                // Create the message content as a binary attachment
                if (WithReflectionSerialization)
                {
                    message.BinaryAttachment = Encoding.ASCII.GetBytes(
                                            Newtonsoft.Json.JsonConvert.SerializeObject(jArray));
                }
                else
                {
                    message.BinaryAttachment = Encoding.ASCII.GetBytes(
                        jsonStr);
                }

                // Send the message to the queue on the Solace messaging router
                Logger.Info("Sending message to queue {0}...", SpaceName);
                ReturnCode returnCode = session.Send(message);
                if (returnCode != ReturnCode.SOLCLIENT_OK)
                {
                    Logger.Info("Message sending failed, return code: {0}", returnCode);

                    // Throw exception for failed message
                    throw new Exception("Msg sending failed");
                }
                else
                {
                    if (debuglevel)
                    {
                        Logger.Info("Message sent successfully, return code: {0}", returnCode);
                    }
                }
            }
         }
    }

}
