package com.solace.consumer.utils;

import com.solace.messaging.MessagingService;
import com.solace.messaging.config.SolaceProperties;
import com.solace.messaging.config.profile.ConfigurationProfile;
import com.solace.messaging.publisher.DirectMessagePublisher;
import com.solace.messaging.publisher.OutboundMessage;
import com.solace.messaging.resources.Topic;
import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;
import org.json.JSONArray;

import java.util.Properties;

public class SolacePublisher {
    protected static final Log logger = LogFactory.getLog(SolacePublisher.class);

    protected DirectMessagePublisher publisher;
    protected MessagingService messagingService;
    protected  Topic topic;
    protected String topicName = "error";

    public SolacePublisher(String host,String vpn, String basicUserName){
        final Properties properties = new Properties();
        properties.setProperty(SolaceProperties.TransportLayerProperties.HOST, host);          // host:port
        properties.setProperty(SolaceProperties.ServiceProperties.VPN_NAME,  vpn);     // message-vpn
        properties.setProperty(SolaceProperties.AuthenticationProperties.SCHEME_BASIC_USER_NAME,basicUserName);
        init(properties);
    }
    protected void init(Properties properties){
        messagingService = MessagingService.builder(ConfigurationProfile.V1)
                .fromProperties(properties).build().connect();

        publisher = messagingService.createDirectMessagePublisherBuilder()
                .onBackPressureWait(1).build().start();
        topic = Topic.of(topicName);
    }
    public void sendToSolace(JSONArray jsonArray){
        //ToDo send custom message
        System.out.println(jsonArray.toString());
        OutboundMessage message = messagingService.messageBuilder().build(jsonArray.toString());
        publisher.publish(message, topic);
    }
}
