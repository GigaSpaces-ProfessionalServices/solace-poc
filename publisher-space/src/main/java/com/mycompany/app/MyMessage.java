package com.mycompany.app;

import com.gigaspaces.sync.DataSyncOperation;

public class MyMessage {
    StringBuffer stringBuffer = new StringBuffer();
    public void appendOperation(DataSyncOperation dataSyncOperation){
        stringBuffer.append(dataSyncOperation);
        stringBuffer.append("/n");
    }

    @Override
    public String toString() {
        return "MyMessage{" +
                "stringBuffer=" + stringBuffer +
                '}';
    }
}
