package com.mycompany.app;

import com.gigaspaces.sync.*;
import com.j_spaces.sadapter.datasource.BulkDataItem;
import com.solace.messaging.MessagingService;
import com.solace.messaging.PubSubPlusClientException;
import com.solace.messaging.config.SolaceProperties;
import com.solace.messaging.config.profile.ConfigurationProfile;
import com.solace.messaging.publisher.OutboundMessage;
import com.solace.messaging.publisher.PersistentMessagePublisher;
import com.solace.messaging.resources.Topic;
import java.util.Properties;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.json.simple.JSONArray;
import org.json.simple.JSONObject;

public class MyEndPoint extends SpaceSynchronizationEndpoint{
    protected static final Log logger = LogFactory.getLog(MyEndPoint.class);
    protected String spaceName = "demo";
    protected  Topic topic;
    //protected DirectMessagePublisher publisher;
    protected PersistentMessagePublisher publisher;
    protected MessagingService messagingService;

    public MyEndPoint(){
        final Properties properties = new Properties();
        properties.setProperty(SolaceProperties.TransportLayerProperties.HOST, "localhost");          // host:port
        properties.setProperty(SolaceProperties.ServiceProperties.VPN_NAME,  "default");     // message-vpn
        properties.setProperty(SolaceProperties.AuthenticationProperties.SCHEME_BASIC_USER_NAME, "default");
        init(properties);
    }

    protected void init(Properties properties){
        messagingService = MessagingService.builder(ConfigurationProfile.V1)
                .fromProperties(properties).build().connect();

        publisher = messagingService.createPersistentMessagePublisherBuilder()
                .onBackPressureWait(1).build().start();
        topic = Topic.of(spaceName);
    }


    public void onTransactionSynchronization(TransactionData transactionData) {
        executeDataSyncOperations(transactionData.getTransactionParticipantDataItems());
    }

    public void onOperationsBatchSynchronization(OperationsBatchData batchData) {
        executeDataSyncOperations(batchData.getBatchDataItems());
    }

    private void executeDataSyncOperations(DataSyncOperation[] dataSyncOperations) {
       // Session session = this.getSessionFactory().openSession();
       // Transaction tr = session.beginTransaction();
        MyMessage myMessage = new MyMessage();
        Object latest = null;

        try {
            DataSyncOperation[] oprs = dataSyncOperations;
            int size = dataSyncOperations.length;
            JSONArray jsonArray = new JSONArray();
            for(int k = 0; k < size; ++k) {
                DataSyncOperation dataSyncOperation = oprs[k];
                String[] fieldNames = ((BulkDataItem) dataSyncOperation).getFieldsNames();
                String[] fieldTypes = ((BulkDataItem) dataSyncOperation).getFieldsTypes();
                Object[] fieldValues = ((BulkDataItem) dataSyncOperation).getFieldsValues();
                JSONObject mainObject = new JSONObject();
                mainObject.put("op",dataSyncOperation.getDataSyncOperationType());
                mainObject.put("type",((BulkDataItem) dataSyncOperation).getTypeName());
                mainObject.put("spaceId",((BulkDataItem) dataSyncOperation).getIdPropertyName());
                JSONArray payload = new JSONArray();
                for(int l=0;l<fieldNames.length;l++) {
                    System.out.println(dataSyncOperation.getDataSyncOperationType()+" --> "+((BulkDataItem) dataSyncOperation).getIdPropertyName()+" --> "+fieldNames[l]);
                    System.out.println(dataSyncOperation.getDataSyncOperationType().name().equals("REMOVE") +" --- "+((BulkDataItem) dataSyncOperation).getIdPropertyName().equals(fieldNames[l]));
                    if((dataSyncOperation.getDataSyncOperationType().name().equals("REMOVE") && !((BulkDataItem) dataSyncOperation).getIdPropertyName().equals(fieldNames[l]))) {
                        System.out.println(((BulkDataItem) dataSyncOperation).getTypeName()+" <<<<--> "+((BulkDataItem) dataSyncOperation).getIdPropertyName()+" --> "+fieldNames[l]);
                        continue;
                    }
                    JSONObject fieldDetails = new JSONObject();
                    fieldDetails.put("columnName",fieldNames[l]);
                    fieldDetails.put("columnType",fieldTypes[l]);
                    fieldDetails.put("value",fieldValues[l]);
                    payload.add(fieldDetails);
                }
                mainObject.put("payload",payload);
                logger.info("About to append:"+ dataSyncOperation);
                myMessage.appendOperation(dataSyncOperation);
                jsonArray.add(mainObject);
            }
            //sendToSolace(myMessage);
           // sendToSolace(jsonArray);
            publishStringMessage(publisher, jsonArray);
        } catch (Exception e) {
           // this.rollbackTx(tr);
            throw new SpaceSynchronizationEndpointException("Failed to execute bulk operation, latest object [" + latest + "]", e);
        } finally {
            //this.closeSession(session);
        }
    }



    /*
    ---- Run Solac :
    docker run -d -p 8080:8080 -p 55555:55555 -p 8008:8008 -p 1883:1883 -p 8000:8000 -p 5672:5672 -p 9000:9000 -p 2222:2222 --shm-size=2g --env username_admin_globalaccesslevel=admin --env username_admin_password=admin --name=solace solace/solace-pubsub-standard
     */
    //protected void sendToSolace(MyMessage myMessage){
    protected void sendToSolace(JSONArray jsonArray){
       //ToDo send custom message
        System.out.println(jsonArray.toString());
        OutboundMessage message = messagingService.messageBuilder().build(jsonArray.toString());
        publisher.publish(message, topic);
    }

    public void publishStringMessage(
            final PersistentMessagePublisher messagePublisher, JSONArray jsonArray) {
        System.out.println(jsonArray.toString());
        // listener that processes all delivery confirmations/timeouts for all messages all
        // messages being send using given instance of messagePublisher
        final PersistentMessagePublisher.MessagePublishReceiptListener deliveryConfirmationListener = (publishReceipt) -> {
            final PubSubPlusClientException exceptionIfAnyOrNull = publishReceipt.getException();

            if (null != exceptionIfAnyOrNull) {
                System.out.println("Msg Not Ack, something went wrong : "+exceptionIfAnyOrNull);
            } else {
                System.out.println("Ack : "+ publishReceipt);
            }
        };

        // listen to all delivery confirmations for all messages being send
        messagePublisher.setMessagePublishReceiptListener(deliveryConfirmationListener);

        // publishing a message (String payload in this case)
        messagePublisher
                .publish(jsonArray.toString(), topic);

        //Block till acknowledge
        /*try {
            messagePublisher
                    .publishAwaitAcknowledgement(messagingService.messageBuilder().build(jsonArray.toString()), topic, 20000L);
        } catch (InterruptedException e) {
            e.printStackTrace();
        }*/

    }

}
