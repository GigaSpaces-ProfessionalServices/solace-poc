using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GigaSpaces.Core;
using GigaSpaces.Core.Metadata;

namespace Piper.Common
{


    public class OrderProcessorThread
    {
        public int WorkerID;
        public ConcurrentQueue<OrderMsg> orderQueue;
        public long orderTime;
        public long orderCnt;
        public long writeOrderCnt;
        public long orderQty;
        public long lastGSOrderID;

        public long processStartTime;
        //PVOID p;
        public long processEndTime;
        public ISpaceProxy spaceProxy;

        public OrderProcessorThread(int workerID, ISpaceProxy SpaceProxy, ConcurrentQueue<OrderMsg> queue, long OrderCnt,
            long OrderQty, long LastGSOrderID )
        {
            WorkerID = workerID;
            orderQueue = queue;
            orderTime = 0;
            writeOrderCnt = 0;
            orderCnt = OrderCnt;
            orderQty = OrderQty;
            spaceProxy = SpaceProxy;
            lastGSOrderID = LastGSOrderID;
        }

    }
}
