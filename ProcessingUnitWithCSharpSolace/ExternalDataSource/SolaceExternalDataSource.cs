using GigaSpaces.Core;
using GigaSpaces.Core.Persistency;
using System.Text;
using SolaceSystems.Solclient.Messaging;
using GigaSpaces.Examples.ProcessingUnit.Common;
using System.Reflection;
using Newtonsoft.Json.Linq;
using GigaSpaces.Core.Metadata;
using NHibernate;
using NHibernate.Metadata;
using NHibernate.Persister.Entity;
using NHibernateCfg = NHibernate.Cfg;
using NHibernate.Criterion;

namespace CustomExternalDataSource.ExternalDataSource
{
    public class SolaceExternalDataSource : AbstractExternalDataSource
    {
        const int MaxRetries = 3;
        const string SpaceName = "dataExampleSpace";

        const string host = "172.31.9.175";
        const string username = "default";
        const string vpnname = "default";
        const string password = "";

        const string LookupGroup = "xap-16.3.0";
        const string AssemblyFilePath = "C:\\GigaSpaces\\XAP.NET-16.3.0-patch-p-3-x64\\NET v4.0\\Examples\\ProcessingUnitWithCSharpSolace\\Common\\obj\\x64\\Debug\\GigaSpaces.Examples.ProcessingUnit.Common.dll";
        const bool debuglevel = false;
        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        string VPNName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        const int DefaultReconnectRetries = 3;
        SolaceSystems.Solclient.Messaging.ISession session;
        IQueue queue;
        ISpaceProxy? spaceProxy;
        Assembly assembly;
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
           /* string[] args = new string[] { "172.31.9.175", "default@default", "" };
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: TopicPublisher <host> <username>@<vpnname> <password>");
                Environment.Exit(1);
            }

            string[] split = args[1].Split('@');
            if (split.Length != 2)
            {
                Console.WriteLine("Usage: TopicPublisher <host> <username>@<vpnname> <password>");
                Environment.Exit(1);
            }

            string host = args[0]; // Solace messaging router host name or IP address
            string username = split[0];
            string vpnname = split[1];
            string password = args[2];*/

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
                VPNName = vpnname;
                UserName = username;
                Password = password; 
                Run(context, host);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
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
            if (string.IsNullOrWhiteSpace(VPNName))
            {
                throw new InvalidOperationException("VPN name must be non-empty.");
            }
            if (string.IsNullOrWhiteSpace(UserName))
            {
                throw new InvalidOperationException("Client username must be non-empty.");
            }

            // Create session properties
            SessionProperties sessionProps = new SessionProperties()
            {
                Host = host,
                VPNName = VPNName,
                UserName = UserName,
                Password = Password,
                ReconnectRetries = DefaultReconnectRetries
            };

            // Connect to the Solace messaging router
            Console.WriteLine("Connecting as {0}@{1} on {2}...", UserName, VPNName, host);
            // NOTICE HandleSessionEvent as session event handler
              session = context.CreateSession(sessionProps, null, HandleSessionEvent);
                ReturnCode returnCode = session.Connect();
                if (returnCode == ReturnCode.SOLCLIENT_OK)
                {
                    Console.WriteLine("Session successfully connected.");
                }
                else
                {
                    Console.WriteLine("Error connecting, return code: {0}", returnCode);
                }
        }
        void HandleSessionEvent(object sender, SessionEventArgs args)
        {
            // Received a session event
            if (debuglevel)
            {
                Console.WriteLine("Received session event {0}.", args.ToString());
            }
            switch (args.Event)
            {
                // this is the confirmation
                case SessionEvent.Acknowledgement:
                    if (debuglevel)
                    {
                        Console.WriteLine("SessionEvent.Acknowledgement {0}.", args.ToString());
                    }
                    MsgInfo messageRecord = args.CorrelationKey as MsgInfo;
                    if (messageRecord != null)
                    {
                        messageRecord.Acked = true;
                        messageRecord.Accepted = args.Event == SessionEvent.Acknowledgement;
                    }
                    break;
                case SessionEvent.RejectedMessageError:
                    if (debuglevel)
                    {
                        Console.WriteLine("SessionEvent.RejectedMessageError : message record rejected " + args.ResponseCode);
                    }
                    MsgInfo _messageRecord = args.CorrelationKey as MsgInfo;
                    if (_messageRecord != null)
                    {
                        _messageRecord.Acked = false;
                        _messageRecord.Accepted = false;
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
            try
            {
                foreach (BulkItem bulkItem in bulk)
                {
                    // ExecuteBulkItem(bulkItem, retries);
                    jsonArray.Add(createJsonResponse(bulkItem));
                }
            }
            catch (Exception e)
            {
                if (retries >= MaxRetries)
                    throw new Exception("Can't execute bulk store.", e);

                if (retries == 1) { 
                    Logger.Error("Retrying ... "+retries, e);
                }
                ExecuteBulk(bulk, retries + 1);
            }
            SendMessage(jsonArray);
        }

        protected virtual void ExecuteBulkItem(BulkItem bulkItem, int retries)
        {
            JArray jsonArray = new JArray();
            try
            {
                jsonArray.Add(createJsonResponse(bulkItem));
            }
            catch (Exception e)
            {
                if (retries >= MaxRetries)
                    throw new Exception("Can't execute bulk store.", e);
                ExecuteBulkItem(bulkItem, retries + 1);
            }
            SendMessage(jsonArray);
        }

        JObject createJsonResponse(BulkItem bulkItem)
        {
            setSpaceProxyIfNull();

            //call Once move to above
            
            object itemValue = bulkItem.Item;

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
                factory.LookupGroups = LookupGroup;
                try
                {
                    spaceProxy = factory.Create();
                    spaceProxy.Count(new object());
                    assembly = Assembly.LoadFrom(AssemblyFilePath);
                    setSolaceSession();
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message, e);
                    Console.WriteLine("Space verification failed. Please check & try again...");
                }
            }
        }

        void setSolaceSession()
        {
            // Provision the queue
            string queueName = SpaceName;
            Console.WriteLine("Attempting to provision the queue '{0}'...", queueName);
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
            Console.WriteLine("Queue '{0}' has been created and provisioned.", queueName);
        }

        private void SendMessage(JArray data)
        {
            
            // Create the queue
            //TODO should be once queue creation
           // using (IQueue queue = ContextFactory.Instance.CreateQueue(queueName))
           // {
                // Create the message
                using (IMessage message = ContextFactory.Instance.CreateMessage())
                {
                    // Message's destination is the queue and the message is persistent
                    message.Destination = queue;
                    message.DeliveryMode = MessageDeliveryMode.Persistent;

                    // Create the message content as a binary attachment
                    message.BinaryAttachment = Encoding.ASCII.GetBytes(
                        Newtonsoft.Json.JsonConvert.SerializeObject(data));

                    // Create a message correlation object
                    MsgInfo msgInfo = new MsgInfo(message);
                    message.CorrelationKey = msgInfo;

                    // Send the message to the queue on the Solace messaging router
                    Console.WriteLine("Sending message to queue {0}...", SpaceName);
                    ReturnCode returnCode = session.Send(message);
                    if (returnCode != ReturnCode.SOLCLIENT_OK)
                    {
                        Console.WriteLine("Sending failed, return code: {0}", returnCode);
                    }
            }

        }

        public override IDataEnumerator GetEnumerator(Query query)
        {
            return new MyIDataEnumerator(query, SessionFactory, EnumeratorLoadFetchSize, PerformOrderById);
        }


        public override IDataEnumerator InitialLoad()
        {
            List<IDataEnumerator> enumerators = new List<IDataEnumerator>();
            foreach (string initialLoadEntity in InitialLoadEntries)
            {
                if (InitialLoadChunkSize == -1)
                    enumerators.Add(GetEnumerator(initialLoadEntity, 0, int.MaxValue));
                else
                    enumerators.AddRange(DivideToChunks(initialLoadEntity));
            }

            return new ConcurrentMultiDataEnumerator(enumerators, EnumeratorLoadFetchSize, InitialLoadThreadPoolSize);
        }



        //TODO ADD initial load 
        public const string NHibernateConfigProperty = "nhibernate-config-file";
        public const string NHibernateHbmDirectory = "nhibernate-hbm-dir";
        public const string NHibernateConnectionStringProperty = "nhibernate-connection-string";

        private HashSet<String> _managedEntriesSet;

        public SolaceExternalDataSource(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
        }

        public ISessionFactory SessionFactory { get; private set; }
        public int InitialLoadThreadPoolSize { get; set; }
        public bool PerformOrderById { get; set; }
        public string[] InitialLoadEntries { get; set; }
        public string[] ManagedEntries { get; set; }
        public int InitialLoadChunkSize { get; set; }
        public int EnumeratorLoadFetchSize { get; set; }
        public bool UseMerge { get; set; }

        public override void Init(Dictionary<string, string> properties)
        {
            base.Init(properties);

            EnumeratorLoadFetchSize = GetIntProperty("EnumeratorLoadFetchSize", 10000);
            if (EnumeratorLoadFetchSize < 1)
                throw new ArgumentException("EnumeratorLoadFetchSize must be a positive number");

            InitialLoadChunkSize = GetIntProperty("InitialLoadChunkSize", 100000);
            if (InitialLoadChunkSize < 1 && InitialLoadChunkSize != -1)
                throw new ArgumentException("InitialLoadChunkSize must be a positive number or -1");

            InitialLoadThreadPoolSize = GetIntProperty("InitialLoadThreadPoolSize", 10);
            if (InitialLoadThreadPoolSize < 1)
                throw new ArgumentException("InitialLoadThreadPoolSize must be a positive number");

            PerformOrderById = GetBoolProperty("PerformOrderById", PerformOrderById);
            UseMerge = GetBoolProperty("UseMerge", UseMerge);

            // only configure a session factory if it is not injected			
            if (SessionFactory == null)
                SessionFactory = CreateSessionFactory();

            // only extract managed entries if it wasn't injected
            if (ManagedEntries == null)
            {
                List<string> managedEntriesList = new List<string>();
                IDictionary<string, IClassMetadata> allClassMetadata = SessionFactory.GetAllClassMetadata();
                foreach (string type in allClassMetadata.Keys)
                    managedEntriesList.Add(type);
                ManagedEntries = managedEntriesList.ToArray();
            }
            //Initialized Managed Entries set to be used later for faster search
            _managedEntriesSet = new HashSet<string>(ManagedEntries);

            // only extract initial load entries if it wasn't injected
            if (InitialLoadEntries == null)
            {
                List<string> initialLoadEntriesList = new List<string>();
                IDictionary<string, IClassMetadata> allClassMetadata = SessionFactory.GetAllClassMetadata();
                foreach (KeyValuePair<string, IClassMetadata> entry in allClassMetadata)
                {
                    AbstractEntityPersister entityPersister = (AbstractEntityPersister)entry.Value;
                    string mappedSuperClass = entityPersister.MappedSuperclass;
                    if (mappedSuperClass != null)
                    {
                        IClassMetadata superClassMetadata = allClassMetadata[mappedSuperClass];
                        if (superClassMetadata.GetMappedClass(EntityMode.Map) != null)
                        {
                            //Filter out those who have their super classes mapped
                            continue;
                        }
                    }
                    initialLoadEntriesList.Add(entry.Key);
                }
                InitialLoadEntries = initialLoadEntriesList.ToArray();
            }
        }

        protected virtual IDataEnumerator GetEnumerator(String entityType, int from, int maxResults)
        {
            return new MyIDataEnumerator(entityType, SessionFactory, EnumeratorLoadFetchSize, PerformOrderById, from, maxResults);
        }

        protected virtual IEnumerable<IDataEnumerator> DivideToChunks(string entityType)
        {
            if (entityType == null)
                throw new ArgumentException("DivideToChunks must receive an Entity Type");

            NHibernate.ISession session = SessionFactory.OpenSession();
            NHibernate.ITransaction tx = session.BeginTransaction();
            try
            {
                //Get number of rows of the current entity in the data base
                ICriteria criteria = session.CreateCriteria(entityType);
                criteria.SetProjection(Projections.RowCount());
                int count = (int)criteria.UniqueResult();
                //Create enumerators for each chunk
                List<IDataEnumerator> enumerators = new List<IDataEnumerator>();
                for (int from = 0; from < count; from += InitialLoadChunkSize)
                    enumerators.Add(GetEnumerator(entityType, from, InitialLoadChunkSize));
                return enumerators;
            }
            catch (Exception e1)
            {
                Console.WriteLine("DivideToChunks355 exception1" + e1.ToString());
                return null;
            }
            finally
            {
                if (tx != null && tx.IsActive)
                    tx.Commit();
                CloseSession(session);
            }
        }
        private static void CloseSession(NHibernate.ISession session)
        {
            if (session.IsOpen)
            {
                session.Flush();
                session.Close();
            }
        }

        private ISessionFactory CreateSessionFactory()
        {
            try
            {
                var config = new NHibernateCfg.Configuration();
                var configFile = GetFileProperty(NHibernateConfigProperty);
                if (configFile == null)
                    config = config.Configure();
                else
                {
                    configFile = Path.GetFullPath(Environment.ExpandEnvironmentVariables(configFile));
                    config = config.Configure(configFile);
                }

                var hbmFolder = GetFileProperty(NHibernateHbmDirectory);
                if (!String.IsNullOrEmpty(hbmFolder))
                {
                    hbmFolder = Path.GetFullPath(Environment.ExpandEnvironmentVariables(hbmFolder));
                    config.AddDirectory(new DirectoryInfo(hbmFolder));
                }

                var connectionString = GetProperty(NHibernateConnectionStringProperty);
                if (!String.IsNullOrEmpty(connectionString))
                    config.SetProperty(NHibernateCfg.Environment.ConnectionString, connectionString);

                return config.BuildSessionFactory();
            }
            catch (Exception e)
            {
                Console.WriteLine("CreateSessionFactory404 exception1" + e.ToString());
                throw new Exception("Error creating NHibernate Session Factory", e);
            }
        }
    }
}
