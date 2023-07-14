#region Copyright & License
/*
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
#endregion

using System;
using System.Text;
using SolaceSystems.Solclient.Messaging;
using System.Threading;
using System.Data.SqlClient;
using Newtonsoft.Json.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.Json.Nodes;
using System.Collections.Generic;
using System.Linq;
using static System.Collections.Specialized.BitVector32;
using System.Runtime.InteropServices;

/// <summary>
/// Solace Systems Messaging API tutorial: QueueConsumer
/// </summary>

namespace Tutorial
{
    /// <summary>
    /// Demonstrates how to use Solace Systems Messaging API for sending and receiving a guaranteed delivery message
    /// </summary>
    class QueueConsumer : IDisposable
    {
        string VPNName { get; set; }
        string HOST { get; set; }
        string UserName { get; set; }
        string Password { get; set; }

        const int DefaultReconnectRetries = 3;

        private ISession Session = null;
        private IQueue Queue = null;
        private IFlow Flow = null;
        private EventWaitHandle WaitEventWaitHandle = new AutoResetEvent(false);

        void Run(IContext context, string host)
        {
            // Validate parameters
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
            Session = context.CreateSession(sessionProps, null, null);
            ReturnCode returnCode = Session.Connect();
            if (returnCode == ReturnCode.SOLCLIENT_OK)
            {
                Console.WriteLine("Session successfully connected.");

                // Provision the queue
                string queueName = "dataExampleSpace";
                Console.WriteLine("Attempting to provision the queue '{0}'...", queueName);

                // Set queue permissions to "consume" and access-type to "exclusive"
                EndpointProperties endpointProps = new EndpointProperties()
                {
                    Permission = EndpointProperties.EndpointPermission.Consume,
                    AccessType = EndpointProperties.EndpointAccessType.Exclusive
                };
                // Create the queue
                Queue = ContextFactory.Instance.CreateQueue(queueName);

                //Session.Dispose();
                // Provision it, and do not fail if it already exists
                Session.Provision(Queue, endpointProps,
                    ProvisionFlag.IgnoreErrorIfEndpointAlreadyExists | ProvisionFlag.WaitForConfirm, null);
                Console.WriteLine("Queue '{0}' has been created and provisioned.", queueName);

                // Create and start flow to the newly provisioned queue
                // NOTICE HandleMessageEvent as the message event handler 
                // and HandleFlowEvent as the flow event handler
                Flow = Session.CreateFlow(new FlowProperties()
                {
                    AckMode = MessageAckMode.ClientAck
                },
                Queue, null, HandleMessageEvent, HandleFlowEvent);
                Flow.Start();
                Console.WriteLine("Waiting for a message in the queue '{0}'...", queueName);

                WaitEventWaitHandle.WaitOne();
            }
            else
            {
                Console.WriteLine("Error connecting, return code: {0}", returnCode);
            }
        }

        /// <summary>
        /// This event handler is invoked by Solace Systems Messaging API when a message arrives
        /// </summary>
        /// <param name="source"></param>
        /// <param name="args"></param>
        private void HandleMessageEvent(object source, MessageEventArgs args)
        {
            // Received a message
            Console.WriteLine("Received message.");
//            string host;
            string messageVPN;
            string basicUsername;
            using (IMessage message = args.Message)
            {
                // Expecting the message content as a binary attachment
                string str = Encoding.ASCII.GetString(message.BinaryAttachment);
                Console.WriteLine("Message content: {0}", Encoding.ASCII.GetString(message.BinaryAttachment));
                JArray jArray = JArray.Parse(str);
                int affectedRows = -1;
                JArray failedMessageArray = new JArray();
                foreach (JObject item in jArray) // <-- Note that here we used JObject instead of usual JProperty
                {
                    string op = item.GetValue("op").ToString();
                    string type = item.GetValue("type").ToString();
                    type = type.Substring(type.LastIndexOf('.') + 1) +"s";//.ToLowerInvariant();
                    string idColumnName = item.GetValue("spaceId").ToString();
                    object idColumnValue = null;

                    JArray payloadArray = JArray.Parse(item.GetValue("payload").ToString());
                    string columns = "";
                    string valuePlaceHolders = "";
                    string separator = "";
                    List<object> values = new List<object>();

                    foreach (JObject payloadObj in payloadArray)
                    {
                        // For first column we don't need separator
                        if (columns.Length > 0)
                        {
                            separator = ", ";
                        }
                        if (string.Equals(op, "write", StringComparison.OrdinalIgnoreCase))
                        {
                            //columns += separator + payloadObj.GetValue("columnName").ToString().ToUpperInvariant();
                            //valuePlaceHolders += separator + "?";
                            if (payloadObj.GetValue("value")!=null && !payloadObj.GetValue("value").Equals(""))
                            {
                                columns += separator + payloadObj.GetValue("columnName").ToString().ToUpperInvariant();
                                valuePlaceHolders += separator +"'"+ payloadObj.GetValue("value")+"'";

                            }
                            values.Add(payloadObj.GetValue("value"));
                        }
                        if (string.Equals(op, "update", StringComparison.OrdinalIgnoreCase))
                        {
                            if (string.Equals(idColumnName, payloadObj.GetValue("columnName").ToString()))
                            {
                                idColumnValue = payloadObj.GetValue("value");
                            }
                            else
                            {
                                columns += separator + payloadObj.GetValue("columnName").ToString().ToUpperInvariant() + "='"+ payloadObj.GetValue("value") +"'";
                                //valuePlaceHolders += separator + "?";
                                valuePlaceHolders += separator + payloadObj.GetValue("value");
                                values.Add(payloadObj.GetValue("value"));
                            }
                        }
                        if (string.Equals(op, "remove", StringComparison.OrdinalIgnoreCase)
                        && (string.Equals(idColumnName, payloadObj.GetValue("columnName").ToString())))
                        {
                            idColumnValue = payloadObj.GetValue("value");
                        }
                        // Add idColumn at last for where clause condition
                        if (string.Equals(op, "update", StringComparison.OrdinalIgnoreCase)
                                || string.Equals(op, "remove", StringComparison.OrdinalIgnoreCase))
                        {
                            values.Add(idColumnValue);
                        }
                    }
                        string sql = "";
                        // Insert/update to Database
                        if (string.Equals(op, "write", StringComparison.OrdinalIgnoreCase))
                        {
                            sql = "INSERT INTO " + type + " (" + columns + ") values(" + valuePlaceHolders.Replace("''","null") + ")";
                        }
                        if (string.Equals(op, "update", StringComparison.OrdinalIgnoreCase))
                        {
                            sql = "UPDATE " + type + " SET " + columns + " WHERE " + idColumnName + "="+ idColumnValue;
                        }
                        if (string.Equals(op, "remove", StringComparison.OrdinalIgnoreCase))
                        {
                            sql = "DELETE FROM " + type + " WHERE " + idColumnName + "="+ idColumnValue;
                        }
                        Console.WriteLine("Query: " + sql);
                        object[] valuesArr = values.ToArray();
                        affectedRows = writeToDB(sql, valuesArr, failedMessageArray, item);
                    //}
                    if (failedMessageArray.Count > 0)
                    {
                        Console.WriteLine("Writing failed messages to Solace");
                        writeToSolace(HOST, VPNName, UserName, failedMessageArray);
                    }

                }


                if (affectedRows >= 0)
                {
                    Flow.Ack(message.ADMessageId);
                }
                // ACK the message
                //Flow.Ack(message.ADMessageId);
                // finish the program
              //  WaitEventWaitHandle.WaitOne();// Set();
            }
        }

        private void writeToSolace(string host, string messageVPN, string basicUsername, JArray failedMessageArray)
        {
            throw new NotImplementedException();
        }

        private int writeToDB(string sql, object[] valuesArr, JArray failedMessageArray, JObject object1)
        {
            Console.WriteLine("Write to Database - piperDB");
            // Write to Database
            //SqlConnection conn = new SqlConnection(@"Data Source=.\SQLEXPRESS;AttachDbFilename=|DataDirectory|\Person.mdf;Integrated Security=True;User Instance=True");
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString =
                  "Data Source=EC2AMAZ-STCEB9P;" +
                  "Initial Catalog=piperdb;" +
                  "User id=piper;" +
                  "Password=Piper123*;";
            //conn.Open();
            //string sql = "INSERT INTO product (ID, NAME, PRICE) VALUES(1, N'Train', 13)";

            try
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand(sql, conn);
                
                return cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                string msg = "Insert Error:";
                msg += ex.Message;
                Console.WriteLine("Error msg:", msg);
                conn.Close();
                // Send message to Solace in failure topic
                failedMessageArray.Add(object1);
                return 1; // We will write this message to error topic so it is safe to ack broker
            }
            finally
            {
                Console.WriteLine("Finished.");
            }
        }
        /*public void mapParams(PreparedStatement ps, object[] args)
        {
            int i = 1;
            for (object arg in args) {
                if (arg instanceof Date) {
                    ps.setTimestamp(i++, new Timestamp(((Date) arg).getTime()));
                } else if (arg instanceof Integer) {
                    ps.setInt(i++, (Integer) arg);
                } else if (arg instanceof Long) {
            ps.setLong(i++, (Long)arg);
        } else if (arg instanceof Double) {
            ps.setDouble(i++, (Double)arg);
        } else if (arg instanceof Float) {
            ps.setFloat(i++, (Float)arg);
        } else
        {
            ps.setString(i++, (String)arg);
        }
                }
    }*/

        public void HandleFlowEvent(object sender, FlowEventArgs args)
        {
            // Received a flow event
            Console.WriteLine("Received Flow Event '{0}' Type: '{1}' Text: '{2}'",
                args.Event,
                args.ResponseCode.ToString(),
                args.Info);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Session != null)
                    {
                        Session.Dispose();
                        Session = null;
                    }
                    if (Queue != null)
                    {
                        Queue.Dispose();
                        Queue = null;
                    }
                    if (Flow != null)
                    {
                        Flow.Dispose();
                        Flow = null;
                    }
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region Main
        static void Main(string[] args)
        {
            args = new string[] { "172.31.9.175", "default@default", "" };
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
            string password = args[2];

            // Initialize Solace Systems Messaging API with logging to console at Warning level
            ContextFactoryProperties cfp = new ContextFactoryProperties()
            {
                SolClientLogLevel = SolLogLevel.Warning
            };
            cfp.LogToConsoleError();
            ContextFactory.Instance.Init(cfp);

            

            try
            {
                // Context must be created first
                using (IContext context = ContextFactory.Instance.CreateContext(new ContextProperties(), null))
                {
                    // Create the application
                    using (QueueConsumer queueConsumer = new QueueConsumer()
                    {
                        HOST = host,
                        VPNName = vpnname,
                        UserName = username,
                        Password = password
                    })
                    {
                        // Run the application within the context and against the host
                        queueConsumer.Run(context, host);
                    }

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception thrown: {0}", ex.Message);
            }
            finally
            {
                // Dispose Solace Systems Messaging API
                ContextFactory.Instance.Cleanup();
            }
            Console.WriteLine("Finished.");
        
                            
        }
        #endregion
    }

}
