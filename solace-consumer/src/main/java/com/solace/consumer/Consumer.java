/*
 * Copyright 2021-2023 Solace Corporation. All rights reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"); you may not
 * use this file except in compliance with the License. You may obtain a copy of
 * the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations under
 * the License.
 */

package com.solace.consumer;

import java.io.IOException;
import java.sql.*;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Properties;

import com.solace.consumer.utils.JDBCUtil;
import com.solace.consumer.utils.SolacePublisher;
import com.solace.messaging.MessagingService;
import com.solace.messaging.config.MissingResourcesCreationConfiguration;
import com.solace.messaging.config.SolaceProperties.AuthenticationProperties;
import com.solace.messaging.config.SolaceProperties.ServiceProperties;
import com.solace.messaging.config.SolaceProperties.TransportLayerProperties;
import com.solace.messaging.config.profile.ConfigurationProfile;
import com.solace.messaging.receiver.DirectMessageReceiver;
import com.solace.messaging.receiver.MessageReceiver.MessageHandler;
import com.solace.messaging.receiver.PersistentMessageReceiver;
import com.solace.messaging.resources.Queue;
import com.solace.messaging.resources.TopicSubscription;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.json.JSONArray;
import org.json.JSONObject;

public class Consumer {

    protected static final Log logger = LogFactory.getLog(Consumer.class);

    private static final String SAMPLE_NAME = Consumer.class.getSimpleName();
    private static final String TOPIC_PREFIX = "demo";  // used as the topic "root"
    private static final String QUEUE_NAME = "q_java_sub";
    private static final String API = "Java";
    private static volatile boolean isShutdown = false;           // are we done yet?
    private static volatile int msgRecvCounter = 0;                 // num messages received
    private static volatile boolean hasDetectedRedelivery = false;  // detected any messages being redelivered?

    public static void main(String... args) throws IOException {
        if (args.length < 3) {  // Check command line arguments
            System.out.println("Usage: "+SAMPLE_NAME+" <host:port> <message-vpn> <client-username> [password]%n");
            System.out.println("  e.g. "+SAMPLE_NAME+" localhost default default%n%n");
            System.exit(-1);
        }

        System.out.println(API + " " + SAMPLE_NAME + " initializing...");
        final Properties properties = new Properties();
        String host = args[0];
        String messageVPN = args[1];
        String basicUserName = args[2];
        properties.setProperty(TransportLayerProperties.HOST, host);          // host:port
        properties.setProperty(ServiceProperties.VPN_NAME,  messageVPN);     // message-vpn
        properties.setProperty(AuthenticationProperties.SCHEME_BASIC_USER_NAME, basicUserName);      // client-username
        if (args.length > 3) {
            properties.setProperty(AuthenticationProperties.SCHEME_BASIC_PASSWORD, args[3]);  // client-password
        }
        //properties.setProperty(ServiceProperties.RECEIVER_DIRECT_SUBSCRIPTION_REAPPLY, "true");  // subscribe Direct subs after reconnect
        properties.setProperty(TransportLayerProperties.RECONNECTION_ATTEMPTS, "20");  // recommended settings
        properties.setProperty(TransportLayerProperties.CONNECTION_RETRIES_PER_HOST, "5");


        final MessagingService messagingService = MessagingService.builder(ConfigurationProfile.V1)
                .fromProperties(properties).build().connect();  // blocking connect to the broker

        messagingService.addServiceInterruptionListener(serviceEvent -> {
            logger.warn("### SERVICE INTERRUPTION: "+serviceEvent.getCause());
            //isShutdown = true;
        });
        messagingService.addReconnectionAttemptListener(serviceEvent -> {
            System.out.println("### RECONNECTING ATTEMPT: "+serviceEvent);
        });
        messagingService.addReconnectionListener(serviceEvent -> {
            System.out.println("### RECONNECTED: "+serviceEvent);
        });

        // this receiver assumes the queue already exists and has a topic subscription mapped to it
        // if not, first create queue with PubSub+Manager, or SEMP management API
        /*final PersistentMessageReceiver receiver = messagingService
                .createPersistentMessageReceiverBuilder()
                .build(Queue.durableExclusiveQueue(QUEUE_NAME));*/
        Queue queue = Queue.durableExclusiveQueue(QUEUE_NAME);
        final PersistentMessageReceiver receiver = messagingService.createPersistentMessageReceiverBuilder()
                .withSubscriptions(TopicSubscription.of(TOPIC_PREFIX))
                .withMissingResourcesCreationStrategy(MissingResourcesCreationConfiguration.MissingResourcesCreationStrategy.CREATE_ON_START)   // The strategy to attempt create missing resources when the connection is established.
                .build(queue);

        try {
            receiver.start();
        } catch (RuntimeException e) {
            logger.error(e);
            logger.error("%n*** Could not establish a connection to queue '"+QUEUE_NAME+"': "+ e.getMessage());
            return;

        }

        // asynchronous anonymous receiver message callback
        receiver.receiveAsync(message -> {
            msgRecvCounter++;
            if (message.isRedelivered()) {  // useful check
                // this is the broker telling the consumer that this message has been sent and not ACKed before.
                // this can happen if an exception is thrown, or the broker restarts, or the network disconnects
                // perhaps an error in processing? Should do extra checks to avoid duplicate processing
                hasDetectedRedelivery = true;
            }

            JSONArray array = new JSONArray(message.getPayloadAsString());
            System.out.println("RECEIVED A MESSAGE As String"+message.getPayloadAsString());

            // Process message and if it returns positive row count then only ACK to broker
            int affectedRows =processMessage(array,host,messageVPN,basicUserName);
            System.out.println("affectedRows: "+affectedRows);
            // Messages are removed from the broker queue when the ACK is received.
            // Therefore, DO NOT ACK until all processing/storing of this message is complete.
            // NOTE that messages can be acknowledged from any thread.

            /*
            affectedRows =
            0  = ExecuteUpdate returns 0 when try to update/delete records whose id is not available in DB
            >0 = a) Insert/Update/Delete performs successfully b) if sql exception occurs then write that msg to error topic and send ack
            -1 = When db connection raises any error
             */
            if(affectedRows >= 0) {
                System.out.println("ACKs for the message");
                receiver.ack(message);  // ACKs are asynchronous
            }
        });


        // async queue receive working now, so time to wait until done...
        while (System.in.available() == 0 && !isShutdown) {  // loop now, just use main thread
            try {
                Thread.sleep(5000);  // take a pause
                System.out.println(API+" "+SAMPLE_NAME+" Received msgs: "+msgRecvCounter);  // simple way of calculating message rates
                msgRecvCounter = 0;
                if (hasDetectedRedelivery) {
                    System.out.println("*** Redelivery detected ***");
                    hasDetectedRedelivery = false;  // only show the error once per second
                }
            } catch (RuntimeException e) {
                logger.error("### Exception caught during consume: %s%n", e);
                isShutdown = true;
            } catch (InterruptedException e) {
                // Thread.sleep() interrupted... probably getting shut down
            }
        }
        isShutdown = true;
        receiver.terminate(500);
        messagingService.disconnect();
        System.out.println("Main thread quitting.");
    }

    public static int processMessage(JSONArray array,String host, String messageVPN, String basicUsername){
        JSONArray failedMessageArray = new JSONArray();
        int affectedRows =-1;
        for(int i=0; i < array.length(); i++) {
            JSONObject object = array.getJSONObject(i);

            String op = object.getString("op");
            String type = object.getString("type");
            type = type.substring(type.lastIndexOf('.')+1).toLowerCase();
            String idColumnName = object.has("spaceId")?object.getString("spaceId"):"";
            Object idColumnValue = null;

            JSONArray payloadArray = object.getJSONArray("payload");
            String columns = "";
            String valuePlaceHolders = "";
            List<Object> values = new ArrayList<>();
            String separator="";
            for (int j = 0; j < payloadArray.length(); j++) {
                JSONObject payloadObj = payloadArray.getJSONObject(j);

                // For first column we don't need separator
                if(columns.length()>0){
                    separator=", ";
                }
                if(op.equalsIgnoreCase("write")){
                    columns += separator + payloadObj.getString("columnName").toUpperCase();
                    valuePlaceHolders += separator + "?";
                    values.add(payloadObj.get("value"));
                }
                if(op.equalsIgnoreCase("update")){
                    if(payloadObj.getString("columnName").equals(idColumnName)){
                        idColumnValue = payloadObj.get("value");
                    }else{
                        columns += separator + payloadObj.getString("columnName").toUpperCase() + "=?";
                        valuePlaceHolders += separator + "?";
                        values.add(payloadObj.get("value"));
                    }
                }
                if(op.equalsIgnoreCase("remove")
                        && payloadObj.getString("columnName").equals(idColumnName)){
                    idColumnValue = payloadObj.get("value");
                }
            }

            // Add idColumn at last for where clause condition
            if(op.equalsIgnoreCase("update")
                    || op.equalsIgnoreCase("remove")){
                values.add(idColumnValue);
            }
            String sql = "";
            // Insert/update to Database
            if(op.equalsIgnoreCase("write")){
                sql ="INSERT INTO "+type+" (" + columns + ") values(" + valuePlaceHolders + ")";
            }
            if(op.equalsIgnoreCase("update")){
                sql ="UPDATE "+type+" SET " + columns + " WHERE "+idColumnName+"=?";
            }
            if(op.equalsIgnoreCase("remove")){
                sql ="DELETE FROM "+type+" WHERE "+idColumnName+"=?";
            }
            System.out.println("Query: "+sql);
            Object[] valuesArr = values.toArray();
            affectedRows = writeToDB(sql,valuesArr,failedMessageArray,object);
        }
        if (!failedMessageArray.isEmpty()) {
            System.out.println("Writing failed messages to Solace");
            writeToSolace(host, messageVPN, basicUsername, failedMessageArray);
        }
        return affectedRows;
    }
    public static void mapParams(PreparedStatement ps, Object... args) throws SQLException {
        int i = 1;
        for (Object arg : args) {
            if (arg instanceof Date) {
                ps.setTimestamp(i++, new Timestamp(((Date) arg).getTime()));
            } else if (arg instanceof Integer) {
                ps.setInt(i++, (Integer) arg);
            } else if (arg instanceof Long) {
                ps.setLong(i++, (Long) arg);
            } else if (arg instanceof Double) {
                ps.setDouble(i++, (Double) arg);
            } else if (arg instanceof Float) {
                ps.setFloat(i++, (Float) arg);
            } else {
                ps.setString(i++, (String) arg);
            }
        }
    }
    public static int writeToDB(String sql,Object[] valuesArr, JSONArray failedMessageArray,JSONObject object){
        try{
            Connection conn = JDBCUtil.getConnection();
            if(conn == null){
                return -1; // Not able to connect to DB
            }
            PreparedStatement ps = conn.prepareStatement(sql);
            mapParams(ps,valuesArr);
            return ps.executeUpdate();
        }catch (SQLException e) {
            logger.error("Message failed to persist so writing to error topic. Message: "+object.toString());
            e.printStackTrace();
            // Send message to Solace in failure topic
            failedMessageArray.put(object);
            return 1; // We will write this message to error topic so it is safe to ack broker
        }
    }
    public static void writeToSolace(String host,String vpn, String basicUserName, JSONArray failedMsgs){
        SolacePublisher sp= new SolacePublisher(host, vpn, basicUserName);
        sp.sendToSolace(failedMsgs);
    }
}
