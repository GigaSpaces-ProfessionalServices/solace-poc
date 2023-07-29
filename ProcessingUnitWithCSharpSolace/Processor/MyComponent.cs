using GigaSpaces.Core;
using GigaSpaces.Core.Admin;
using GigaSpaces.Core.XAP.ProcessingUnit.Containers.BasicContainer;
using GigaSpaces.XAP.ProcessingUnit.Containers;
using GigaSpaces.XAP.ProcessingUnit.Containers.BasicContainer;
using NLog;
using Piper.Common;
using Piper.Processor;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;

namespace GigaSpaces.Examples.ProcessingUnit.Processor
{
    [BasicProcessingUnitComponent]
    class MyComponent
    {
        static Random random = new Random();
        static String Version = "1.1";
        static int totalSpaces = 2;
        static long primaryOrders = 100;
        static long backupOrders = 100;
        static long lastGSOrderID = 0;
        static long lastGSFillID = 0;
        static long baseRcordID = 0;
        static string withFeeder = "true";
        static long orderQty = 10;
        static int numOrderWorkers = 1;
        static int numFillProcessorWorkers = 10;
        bool? isStartedAsPrimary;

        private int? _instanceId;
        private int? _pid;

        private static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public int? InstanceId
        {
            get { return _instanceId; }
            private set { _instanceId = value; }
        }
        public int? Pid
        {
            get { return _pid; }
            private set { _pid = value; }
        }

        public MyComponent()
        {
            Console.WriteLine(">>>>> Starting OP");
        }
        static public long getLastGSOrderID()
        {
            return lastGSOrderID;
        }
        static public long getLastGSFillID()
        {
            return lastGSFillID;
        }
        [ContainerInitialized]
        public void Initialize(BasicProcessingUnitContainer container)
        {
            string tempVal;
            container.Properties.TryGetValue("TotalSpaces", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                totalSpaces = int.Parse(tempVal);
            container.Properties.TryGetValue("PrimaryOrders", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                primaryOrders = int.Parse(tempVal);
            container.Properties.TryGetValue("BackupOrders", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                backupOrders = int.Parse(tempVal);
            container.Properties.TryGetValue("BaseRcordID", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                baseRcordID = int.Parse(tempVal);
            container.Properties.TryGetValue("WithFeeder", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                withFeeder = tempVal;
            container.Properties.TryGetValue("NumOrderWorkers", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                numOrderWorkers = int.Parse(tempVal);
            container.Properties.TryGetValue("NumFillProcessorWorkers", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                numFillProcessorWorkers = int.Parse(tempVal);

            _instanceId = ProcessingUnitContainer.Current.ClusterInfo.InstanceId;
            _pid = Process.GetCurrentProcess().Id;

            // for NLog output
            GlobalDiagnosticsContext.Set("InstanceId", InstanceId);
            GlobalDiagnosticsContext.Set("Pid", Pid);
        }
        [BeforePrimary]
        public void beforePrimaryEvent(ISpaceProxy proxy, SpaceMode spaceMode)
        {
            Console.WriteLine(">>>>> before primary -> beforePrimaryEvent " + proxy.Name + ", spaceMode: " + spaceMode.ToString());
        }
        [BeforeBackup]
        public void beforeBackupEvent(ISpaceProxy proxy, SpaceMode spaceMode)
        {
            Console.WriteLine(">>>>> before backup -> beforeBackupEvent " + proxy.Name + ", spaceMode: " + spaceMode.ToString());
        }
        [PostBackup]
        public void postBackupEvent(ISpaceProxy proxy, SpaceMode spaceMode)
        {
            ClusterInfo clusterInfo = ProcessingUnitContainer.Current.ClusterInfo;
            long partitionId = (long)(clusterInfo.InstanceId - 1);
            if (isStartedAsPrimary == null)
            {
                isStartedAsPrimary = false;
                //Logger.Init(partionId);
            }
            Console.WriteLine(">>>>> post backup -> postBackupEvent " + proxy.Name + ", spaceMode: " + spaceMode.ToString()
                + " isStartedAsPrimary: " + isStartedAsPrimary.ToString());
            Logger.Info(">>>>> post backup -> postBackupEvent " + proxy.Name + ", spaceMode: " + spaceMode.ToString()
                + " isStartedAsPrimary: " + isStartedAsPrimary.ToString());
            Logger.Info("clusterInfo : " + clusterInfo);
            Logger.Info("clusterInfo.NumberOfInstances : " + clusterInfo.NumberOfInstances);
        }
        public long getMaxOrderID(ISpaceProxy spaceProxy, ClusterInfo clusterInfo)
        {
            long maxOrderID = 0;
            int numOfPartitions = (int)clusterInfo.NumberOfInstances;
            foreach (GS_Order order in spaceProxy.GetSpaceIterator<GS_Order>(new SqlQuery<GS_Order>("")))
            {
                if (maxOrderID < order.OrderID)
                    maxOrderID = order.OrderID;
            }
            if (maxOrderID > 0)
            {
                maxOrderID = maxOrderID + numOfPartitions +
                                (numOfPartitions - (maxOrderID % numOfPartitions));
            }
            else
            {
                maxOrderID = baseRcordID;
            }
            return maxOrderID;
        }
        public long getMaxFillID(ISpaceProxy spaceProxy, ClusterInfo clusterInfo)
        {
            long maxFillID = 0;
            int numOfPartitions = (int)clusterInfo.NumberOfInstances;
            foreach (GS_Fill fill in spaceProxy.GetSpaceIterator<GS_Fill>(new SqlQuery<GS_Fill>("")))
            {
                if (maxFillID < fill.FillID)
                    maxFillID = fill.FillID;
            }
            if (maxFillID > 0)
            {
                maxFillID = maxFillID + numOfPartitions +
                                (numOfPartitions - (maxFillID % numOfPartitions));
            }
            else
            {
                maxFillID = baseRcordID * orderQty;
            }
            return maxFillID;
        }
        /* public static void Main(string[] args)
         {
             IDictionary<string, string> prop = new Dictionary<string, string>();
             prop.Add("space-config.engine.cache_policy", "1");
             prop.Add("cluster-config.cache-loader.external-data-source", "true");
             prop.Add("cluster-config.cache-loader.central-data-source", "true");
             var customProps = new Dictionary<string, string> { };
             var factory = new EmbeddedSpaceFactory("demo")
             {
                 ExternalDataSource = new ExternalDataSourceConfig { Instance = new SolaceExternalDataSource(), CustomProperties = customProps },
                 CustomProperties = prop
             };
             ISpaceProxy spaceProxy = factory.Create();
             Data data = new Data();
             data.Id = 101;
             data.Info = "testData101";
             data.Type = 101;
             data.Date = DateTime.Now;
             data.Processed = false;
             // Write the object to the space
             spaceProxy.Write(data);
             data = new Data();
             data.Id = 102;
             data.Info = "testData102";
             data.Type = 102;
             data.Date = DateTime.Now;
             data.Processed = false;
             // Write the object to the space
             spaceProxy.Write(data);
             Console.WriteLine(spaceProxy.Count(new object()));
         }*/
        [PostPrimary]
        public void postPrimaryEvent(ISpaceProxy spaceProxy, SpaceMode spaceMode)
        {
            ClusterInfo clusterInfo = ProcessingUnitContainer.Current.ClusterInfo;
            long partitionId = (long)(clusterInfo.InstanceId - 1);
            if (isStartedAsPrimary == null)
            {
                isStartedAsPrimary = true;
                //Logger.Init(partionId);
            }
            Console.WriteLine(">>>>> post primary -> postPrimaryEvent " + spaceProxy.Name + ", spaceMode: " + spaceMode.ToString()
                + " isStartedAsPrimary: " + isStartedAsPrimary.ToString());
            Logger.Info(">>>>> post primary -> postPrimaryEvent " + spaceProxy.Name + ", spaceMode: " + spaceMode.ToString()
                            + " isStartedAsPrimary: " + isStartedAsPrimary.ToString());
            Logger.Info(">>>>> post primary -> postPrimaryEvent Version: " + Version + " Partions: " + totalSpaces.ToString()
                                + " PrimaryOrders: " + primaryOrders.ToString()
                                + " backupOrders: " + backupOrders.ToString()
                                + " baseRcordID: " + baseRcordID.ToString());
            lastGSOrderID = getMaxOrderID(spaceProxy, clusterInfo);
            lastGSFillID = getMaxFillID(spaceProxy, clusterInfo);
            Logger.Info(string.Format("lastGSOrderID: {0} lastGSFillID : {1}", lastGSOrderID, lastGSFillID));
            //if (isStartedAsPrimary == true )
            //    startPU(spaceProxy, spaceMode, clusterInfo);
            // For recovery comment below 2 lines, then uncomment
            Console.WriteLine("withFeeder >>>> " + withFeeder);
            Console.WriteLine(">>>>>>>" + withFeeder.Equals("true"));
            if (withFeeder.Equals("true"))
            {
                Thread thread = new Thread(() => new OP().startPU(spaceProxy, spaceMode, clusterInfo));
                thread.Start();
            }
            Logger.Info(">>>>> post primary -> postPrimaryEvent " + spaceProxy.Name + ", spaceMode: " + spaceMode.ToString()
                           + " started thread ");
        }
        class OP
        {
            public void startPU(ISpaceProxy spaceProxy, SpaceMode spaceMode, ClusterInfo clusterInfo)
            {
                Logger.Info(" startPU Sleeping ");
                Thread.Sleep(60 * 1000);
                Logger.Info(" startPU Running ");
                Logger.Info("clusterInfo : " + clusterInfo);
                Logger.Info("clusterInfo.NumberOfInstances : " + clusterInfo.NumberOfInstances);
                long partionId = (long)(clusterInfo.InstanceId - 1);
                ConcurrentQueue<OrderMsg> orderQueue = new ConcurrentQueue<OrderMsg>();
                ConcurrentQueue<FillMsg> fillQueue = new ConcurrentQueue<FillMsg>();
                List<FillFeeder> fillFeederWorkers = new List<FillFeeder>();
                List<Thread> fillFeederIDs = new List<Thread>();
                List<FillProcessor> fillProcessorWorkers = new List<FillProcessor>();
                List<Thread> fillProcessorIDs = new List<Thread>();
                List<OrderProcessorThread> orderProcessorWorkers = new List<OrderProcessorThread>();
                List<Thread> orderProcessorIDs = new List<Thread>();
                long totalOrders;
                if (lastGSOrderID == 0)
                    totalOrders = primaryOrders * totalSpaces;
                else
                    totalOrders = backupOrders * totalSpaces;
                long totalPartitions = (long)clusterInfo.NumberOfInstances;
                //            long totalPartitions = 16;
                long ordCnt = totalOrders;
                //           long ordCnt = totalOrders / totalPartitions;
                long totalFills = 0;
                int numFillFeederWorkers = 4;
                long fillSampleSize = totalOrders * orderQty / 100;
                long orderSampleSize = ordCnt / 100;
                StatRecord fillstatRecord = new StatRecord();
                StatRecord orderstatRecord = new StatRecord();
                totalFills = ordCnt * orderQty;
                Logger.Info(string.Format("*** PartitionID:{0} Connected to space.", partionId));
                long orderProcesserTotalStartTime = DateTime.Now.Ticks;
                string startTimeStr = DateTime.Now.ToString("HH:mm:ss.fff");
                for (int i = 1; i <= numFillFeederWorkers; i++)
                {
                    //FillFeeder *fillFeeder = new FillFeeder(i, fillQueueArray[i], orderQueueArray[i]);
                    FillFeeder fillFeeder = new FillFeeder(i, fillQueue, orderQueue);
                    Thread thread = new Thread(() => new FillFeederThread().threadRun(fillFeeder, numFillProcessorWorkers));
                    thread.Start();
                    fillFeederIDs.Add(thread);
                    fillFeederWorkers.Add(fillFeeder);
                }
                if (numFillProcessorWorkers > 1)
                {
                    for (int i = 1; i <= numFillProcessorWorkers; i++)
                    {
                        FillProcessor fillProcessor = new FillProcessor(i, spaceProxy, fillQueue, totalFills / numFillProcessorWorkers, lastGSFillID);
                        Thread thread = new Thread(() => new FillProcessorThread().threadRun(fillProcessor, spaceProxy, clusterInfo,
                                                                                        fillSampleSize, fillstatRecord));
                        thread.Start();
                        fillProcessorIDs.Add(thread);
                        fillProcessorWorkers.Add(fillProcessor);
                    }
                }
                for (int i = 1; i <= numOrderWorkers; i++)
                {
                    OrderProcessorThread orderProcessorThread = new OrderProcessorThread(i, spaceProxy, orderQueue, ordCnt / numOrderWorkers, orderQty, lastGSOrderID);
                    Thread thread = new Thread(() => new OrderProcessorThreadCB().threadRun(orderProcessorThread, spaceProxy, partionId, clusterInfo,
                                                                                                orderSampleSize, orderstatRecord));
                    thread.Start();
                    orderProcessorIDs.Add(thread);
                    orderProcessorWorkers.Add(orderProcessorThread);
                }
                ITransactionManager txManager = GigaSpacesFactory.CreateDistributedTransactionManager();
                for (int i = 0; i < orderProcessorIDs.Count; i++)
                {
                    orderProcessorIDs[i].Join();
                }
                // Add messages to stop thread
                for (int i = 0; i < numFillFeederWorkers; i++)
                {
                    OrderMsg orderMsg = new OrderMsg(-1, -1, -1);
                    //orderQueueArray[i].push(orderMsg);
                    orderQueue.Enqueue(orderMsg);
                }
                for (int i = 0; i < fillFeederIDs.Count; i++)
                {
                    fillFeederIDs[i].Join();
                }
                if (numFillProcessorWorkers == 1)
                {
                    for (int i = 1; i <= numFillProcessorWorkers; i++)
                    {
                        FillProcessor fillProcessor = new FillProcessor(i, spaceProxy, fillQueue, totalFills / numFillProcessorWorkers, lastGSFillID);
                        Thread thread = new Thread(() => new FillProcessorThread().threadRun(fillProcessor, spaceProxy, clusterInfo,
                                                                                                fillSampleSize, fillstatRecord));
                        thread.Start();
                        fillProcessorIDs.Add(thread);
                        fillProcessorWorkers.Add(fillProcessor);
                    }
                }
                // Add messages to stop thread
                for (int i = 0; i < numFillProcessorWorkers; i++)
                {
                    FillMsg fillMsg = new FillMsg(-1, -1, -1);
                    //fillQueueArray[i].push(fillMsg);
                    fillQueue.Enqueue(fillMsg);
                }
                for (int i = 0; i < fillProcessorIDs.Count; i++)
                {
                    fillProcessorIDs[i].Join();
                }
                long orderProcesserTotalEndime = DateTime.Now.Ticks;
                Logger.Info(string.Format("PartitionID:{0} Total Time taken  : {1}", partionId, ((orderProcesserTotalEndime - orderProcesserTotalStartTime) / TimeSpan.TicksPerMillisecond)));
                ordCnt = 0;
                long orderTime = 0;
                long minProcessTime = orderProcessorWorkers[0].processStartTime;
                long maxProcessTime = orderProcessorWorkers[0].processEndTime;
                for (int i = 0; i < orderProcessorWorkers.Count; i++)
                {
                    ordCnt += orderProcessorWorkers[i].writeOrderCnt;
                    orderTime += orderProcessorWorkers[i].orderTime;
                    if (orderProcessorWorkers[i].processStartTime < minProcessTime)
                    {
                        minProcessTime = orderProcessorWorkers[i].processStartTime;
                    }
                    if (orderProcessorWorkers[i].processEndTime > maxProcessTime)
                    {
                        maxProcessTime = orderProcessorWorkers[i].processEndTime;
                    }
                }
                Logger.Info(string.Format("Test Config: PartitionID:{0}, Orders: {1}, Fills: {2}, OrderThreads: {3},  FillThreads: {4}",
                            partionId, ordCnt, totalFills / totalPartitions, numOrderWorkers, numFillProcessorWorkers));
                long orderTimems = (orderTime / TimeSpan.TicksPerMillisecond);
                double avgOrder = 1.0 * orderTimems / ordCnt;
                long orderMsgPerSec = (long)((orderTime == 0) ? -1.0 : 1.0 * ordCnt / orderTimems * 1000);
                Logger.Info(string.Format("Wrote Orders: {0} in {1} ms, average {2} ms, {3} orders in sec, {4} start time, {5} end time, PartitionID:{6}  ",
                                            ordCnt, orderTimems, avgOrder, orderMsgPerSec, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff"), partionId));
                orderTimems = ((maxProcessTime - minProcessTime) / TimeSpan.TicksPerMillisecond);
                avgOrder = 1.0 * orderTimems / ordCnt;
                orderMsgPerSec = (long)((orderTime == 0) ? -1.0 : 1.0 * ordCnt / orderTimems * 1000);
                Logger.Info(string.Format(">>>>>> Updated code : Wrote Orders: {0} in {1} ms, average {2} ms, {3} orders in sec, {4} start time, {5} end time, {6} minTicks, {7} maxTicks, PartitionID:{8} ",
                            ordCnt, orderTimems, avgOrder, orderMsgPerSec, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff"), minProcessTime, maxProcessTime, partionId));
                if (orderstatRecord.N > 1)
                {
                    Logger.Info(string.Format("Order Stat partionId:{0} - {1} samples taken, each one averaging {2} orders records.",
                                  partionId, orderstatRecord.N, orderSampleSize));
                    Logger.Info(string.Format("Order Stat partionId:{0} - Aggregate Latency: {1}.",
                                 partionId, orderstatRecord.getStat(1.0 / TimeSpan.TicksPerMillisecond / orderSampleSize)));
                }
                long fillCnt = 0;
                long fillTime = 0;
                minProcessTime = fillProcessorWorkers[0].processStartTime;
                maxProcessTime = fillProcessorWorkers[0].processEndTime;
                for (int i = 0; i < fillProcessorWorkers.Count; i++)
                {
                    fillCnt += fillProcessorWorkers[i].fillCnt;
                    fillTime += fillProcessorWorkers[i].fillTime;
                    if (fillProcessorWorkers[i].processStartTime < minProcessTime)
                    {
                        minProcessTime = fillProcessorWorkers[i].processStartTime;
                    }
                    if (fillProcessorWorkers[i].processEndTime > maxProcessTime)
                    {
                        maxProcessTime = fillProcessorWorkers[i].processEndTime;
                    }
                }
                long fillTimems = (fillTime / TimeSpan.TicksPerMillisecond);
                double avgFill = 1.0 * fillTimems / fillCnt;
                long fillMsgPerSec = (long)((fillTime == 0) ? -1.0 : 1.0 * fillCnt / fillTimems * 1000);
                Logger.Info(string.Format("Wrote Fills: {0} in {1} ms, average {2} ms, {3} fills in sec, PartitionID:{4}, {5} start time, {6} end time", fillCnt, fillTimems, avgFill, fillMsgPerSec,
                                partionId, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff")));
                fillTimems = ((maxProcessTime - minProcessTime) / TimeSpan.TicksPerMillisecond);
                avgFill = 1.0 * fillTimems / fillCnt;
                fillMsgPerSec = (long)((fillTime == 0) ? -1.0 : 1.0 * fillCnt / fillTimems * 1000);
                Logger.Info(string.Format(">>>>>> Updated code : Wrote Fills: {0} in {1} ms, average {2} ms, {3} fills in sec , {4} minTicks, {5} maxTicks, PartitionID:{6}, {7} start time, {8} end time ",
                                fillCnt, fillTimems, avgFill, fillMsgPerSec, minProcessTime, maxProcessTime, partionId, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff")));
                if (fillstatRecord.N > 1)
                {
                    Logger.Info(string.Format("Fill Stat partionId:{0} - {1} samples taken, each one averaging {2} fills records.",
                                  partionId, fillstatRecord.N, fillSampleSize));
                    Logger.Info(string.Format("Fill Stat partionId:{0} - Aggregate Latency: {1}.",
                                 partionId, fillstatRecord.getStat(1.0 / TimeSpan.TicksPerMillisecond / fillSampleSize)));
                }
                lastGSOrderID += totalOrders;
                lastGSFillID += totalFills;
                Logger.Info("Order Processor finished successfully!");
            }
        }
    }
}
