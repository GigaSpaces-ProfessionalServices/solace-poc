package com.solace.consumer.utils;

import org.apache.commons.logging.Log;
import org.apache.commons.logging.LogFactory;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;

public class JDBCUtil {

    protected static final Log logger = LogFactory.getLog(JDBCUtil.class);

    private static String jdbcDriver = "com.microsoft.sqlserver.jdbc.SQLServerDriver";//""com.mysql.jdbc.Driver";
    private static String jdbcURL = "jdbc:sqlserver://35.183.95.219:1433;DatabaseName=piperdb;encrypt=true;trustServerCertificate=true";
    private static String dbUser = "piper";
    private static String dbPwd = "Piper123*";

    private static int dbConnectRetryCount = -1;
    private static long dbConnectRetryInterval = 500; //in milliseconds


    public static Connection getConnection() {
        Connection databaseConnection =null;
        try {
            Class.forName(jdbcDriver).newInstance();
            logger.info("JDBC driver loaded");
        } catch (Exception err) {
            logger.error("Error loading JDBC driver");
            err.printStackTrace(System.err);
            System.exit(0);
        }
        int count=0;
        while (databaseConnection == null && (dbConnectRetryCount == -1 || count<dbConnectRetryCount)){
            logger.info("Connecting to database");
            try {
                databaseConnection = DriverManager.getConnection(jdbcURL,dbUser,dbPwd);
            }catch (SQLException e){
                count++;
                logger.warn("Connecting to DB failed, retrying... attempt count: "+count);
                try {
                    Thread.sleep(dbConnectRetryInterval);
                } catch (InterruptedException ex) {
                    ex.printStackTrace();
                }
            }
        }
        return databaseConnection;
    }
}
