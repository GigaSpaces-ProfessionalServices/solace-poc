using GigaSpaces.Core;

using Piper.Common;

using System;

using System.Collections.Generic;

using System.Linq;

using System.Text;

using System.Threading;

using System.Threading.Tasks;



namespace Piper.Processor

{

    class OrderProcessorThreadCB

    {

        private static Random random = new Random();



        public static string getRandomString(int length)

        {

            const string chars = "ORD_123";

            return new string(Enumerable.Repeat(chars, length)

                .Select(s => s[random.Next(s.Length)]).ToArray());

        }



        //long partionId;

        public int threadRun(OrderProcessorThread orderProcessor, ISpaceProxy spaceProxy, long partionId, ClusterInfo clusterInfo,

                                long intervalSize, StatRecord statRecord)

        {

            long[] intervals = new long[200];

            int intervalIdx = 0;



            Logger.Write(string.Format("*** Started OrderProcessorThread PartionId:{0} WorkerThread:{1} intervalSize:{2}", partionId, orderProcessor.WorkerID, intervalSize));

            Logger.Write("*** OrderProcessorThread - Getting txManager ");

            ITransactionManager txManager = GigaSpacesFactory.CreateDistributedTransactionManager();



            Logger.Write("*** OrderProcessorThread - Created txManager ");



            //Timer orderTimer;

            GS_Order order = new GS_Order();

            //            order.OPID = partionId;

            order.Symbol = "IBM";

            order.Quantity = orderProcessor.orderQty;

            order.Price = 10;

            order.CumQty = 0;

            order.CalCumQty = 0;

            order.CalExecValue = 0;

            int i = 1;

            order.FldInt_1 = random.Next(10, 20 + i);

            order.FldInt_2 = random.Next(10, 20 + i);

            order.FldInt_3 = random.Next(10, 20 + i);

            order.FldInt_4 = random.Next(10, 20 + i);

            order.FldInt_5 = random.Next(10, 20 + i);

            order.FldInt_6 = random.Next(10, 20 + i);

            order.FldInt_7 = random.Next(10, 20 + i);

            order.FldInt_8 = random.Next(10, 20 + i);

            order.FldInt_9 = random.Next(10, 20 + i);

            order.FldInt_10 = random.Next(10, 20 + i);

            order.FldInt_11 = random.Next(10, 20 + i);

            order.FldInt_12 = random.Next(10, 20 + i);

            order.FldInt_13 = random.Next(10, 20 + i);

            order.FldInt_14 = random.Next(10, 20 + i);

            order.FldInt_15 = random.Next(10, 20 + i);

            order.FldInt_16 = random.Next(10, 20 + i);

            order.FldInt_17 = random.Next(10, 20 + i);

            order.FldInt_18 = random.Next(10, 20 + i);

            order.FldInt_19 = random.Next(10, 20 + i);

            order.FldInt_20 = random.Next(10, 20 + i);

            order.FldInt_21 = random.Next(10, 20 + i);

            order.FldInt_22 = random.Next(10, 20 + i);

            order.FldInt_23 = random.Next(10, 20 + i);

            order.FldInt_24 = random.Next(10, 20 + i);

            order.FldInt_25 = random.Next(10, 20 + i);

            order.FldInt_26 = random.Next(10, 20 + i);

            order.FldInt_27 = random.Next(10, 20 + i);

            order.FldInt_28 = random.Next(10, 20 + i);

            order.FldInt_29 = random.Next(10, 20 + i);

            order.FldInt_30 = random.Next(10, 20 + i);

            order.FldInt_31 = random.Next(10, 20 + i);

            order.FldInt_32 = random.Next(10, 20 + i);

            order.FldInt_33 = random.Next(10, 20 + i);

            order.FldInt_34 = random.Next(10, 20 + i);

            order.FldInt_35 = random.Next(10, 20 + i);

            order.FldInt_36 = random.Next(10, 20 + i);

            order.FldInt_37 = random.Next(10, 20 + i);

            order.FldInt_38 = random.Next(10, 20 + i);

            order.FldInt_39 = random.Next(10, 20 + i);

            order.FldInt_40 = random.Next(10, 20 + i);

            order.FldTime_1 = DateTime.Now;

            order.FldTime_2 = DateTime.Now;

            order.FldTime_3 = DateTime.Now;

            order.FldTime_4 = DateTime.Now;

            order.FldTime_5 = DateTime.Now;

            order.FldTime_6 = DateTime.Now;

            order.FldTime_7 = DateTime.Now;

            order.FldTime_8 = DateTime.Now;

            order.FldStr_1 = getRandomString(random.Next(1, 10));

            order.FldStr_2 = getRandomString(random.Next(1, 10));

            order.FldStr_3 = getRandomString(random.Next(1, 10));

            order.FldStr_4 = getRandomString(random.Next(1, 10));

            order.FldStr_5 = getRandomString(random.Next(1, 10));

            order.FldStr_6 = getRandomString(random.Next(1, 10));

            order.FldStr_7 = getRandomString(random.Next(1, 10));

            order.FldStr_8 = getRandomString(random.Next(1, 10));

            order.FldStr_9 = getRandomString(random.Next(1, 10));

            order.FldStr_10 = getRandomString(random.Next(1, 10));

            order.FldStr_11 = getRandomString(random.Next(1, 10));

            order.FldStr_12 = getRandomString(random.Next(1, 10));

            order.FldStr_13 = getRandomString(random.Next(1, 10));

            order.FldStr_14 = getRandomString(random.Next(1, 10));

            order.FldStr_15 = getRandomString(random.Next(1, 10));

            order.FldStr_16 = getRandomString(random.Next(1, 10));

            order.FldStr_17 = getRandomString(random.Next(1, 10));

            order.FldStr_18 = getRandomString(random.Next(1, 10));

            order.FldStr_19 = getRandomString(random.Next(1, 10));

            order.FldStr_20 = getRandomString(random.Next(1, 10));

            order.FldStr_21 = getRandomString(random.Next(1, 10));

            order.FldStr_22 = getRandomString(random.Next(1, 10));

            order.FldStr_23 = getRandomString(random.Next(1, 10));

            order.FldStr_24 = getRandomString(random.Next(1, 10));

            order.FldStr_25 = getRandomString(random.Next(1, 10));

            /*

            order.FldStr_26 = getRandomString(random.Next(1, 10));

            order.FldStr_27 = getRandomString(random.Next(1, 10));

            order.FldStr_28 = getRandomString(random.Next(1, 10));

            order.FldStr_29 = getRandomString(random.Next(1, 10));

            order.FldStr_30 = getRandomString(random.Next(1, 10));

            order.FldStr_31 = getRandomString(random.Next(1, 10));

            order.FldStr_32 = getRandomString(random.Next(1, 10));

            order.FldStr_33 = getRandomString(random.Next(1, 10));

            order.FldStr_34 = getRandomString(random.Next(1, 10));

            order.FldStr_35 = getRandomString(random.Next(1, 10));

            order.FldStr_36 = getRandomString(random.Next(1, 10));

            order.FldStr_37 = getRandomString(random.Next(1, 10));

            order.FldStr_38 = getRandomString(random.Next(1, 10));

            order.FldStr_39 = getRandomString(random.Next(1, 10));

            order.FldStr_40 = getRandomString(random.Next(1, 10));

            */

            order.FldDbl_1 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_2 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_3 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_4 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_5 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_6 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_7 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_8 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_9 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_10 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_11 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_12 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_13 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_14 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_15 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_16 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_17 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_18 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_19 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_20 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_21 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_22 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_23 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_24 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_25 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_26 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_27 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_28 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_29 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_30 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_31 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_32 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_33 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_34 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_35 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_36 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_37 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_38 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_39 = random.Next(10, 20 + i) * 100 / 32;

            order.FldDbl_40 = random.Next(10, 20 + i) * 100 / 32;





            Logger.Write(string.Format("*** OrderProcessorThread PartionId:{0} OrderProcessorThread:{1} - Adding {2} orders, OrderQty {3}",

                                partionId, orderProcessor.WorkerID, orderProcessor.orderCnt, orderProcessor.orderQty));

            //            long firstOrderID = partionId * orderProcessor.orderCnt + 1;

            //            long lastOrderID = firstOrderID + orderProcessor.orderCnt - 1;



            //    long firstOrderID = 1;

            //    long lastOrderID = orderProcessor.orderCnt;



            long firstOrderID = orderProcessor.lastGSOrderID + 1 + ((orderProcessor.WorkerID - 1) * orderProcessor.orderCnt);

            long lastOrderID = firstOrderID + orderProcessor.orderCnt - 1;





            // long firstOrderID = 1 + ((orderProcessor.WorkerID - 1) * orderProcessor.orderCnt);

            // long lastOrderID = firstOrderID + orderProcessor.orderCnt - 1;

            Console.BackgroundColor = ConsoleColor.Blue;

            Console.ForegroundColor = ConsoleColor.White;

            string startTimeStr = DateTime.Now.ToString("HH:mm:ss.fff");

            Logger.Write(string.Format("*** OrderProcessorThread PartionId {0}: OrderProcessorThread:{1} - firstOrderID {2}, lastOrderID {3}, ticks {4} , current time {5}",

                                         partionId, orderProcessor.WorkerID, firstOrderID, lastOrderID, DateTime.Now.Ticks, startTimeStr));

            Console.ResetColor();



            orderProcessor.processStartTime = DateTime.Now.Ticks;

            intervals[intervalIdx++] = orderProcessor.processStartTime;

            int noOfInstances = (int)clusterInfo.NumberOfInstances;



            order.FldTime_1 = DateTime.Now;



            // for (long i = firstOrderID; i <= lastOrderID; i = i + noOfInstances)

            long counter = 0;



            for (long j = firstOrderID; counter < orderProcessor.orderCnt / noOfInstances; j++)

            {





                if (getPartitionId(j, clusterInfo) != partionId)

                {

                    continue;

                }



                ITransaction tx1 = txManager.Create();

                order.OrderID = j;







                spaceProxy.Write(order, tx1, long.MaxValue, 1000 * 60);

                //spaceProxy.Write(order);

                tx1.Commit();







                OrderMsg orderMsg = new OrderMsg(order.OrderID, order.Quantity, order.Price.Value);

                /*if (orderMsg == null) {

                    continue;

                }*/

                orderProcessor.orderQueue.Enqueue(orderMsg);

                counter++;





                if ((counter % intervalSize) == 0)

                {





                    if (intervalIdx < intervals.Length)

                    {

                        intervals[intervalIdx++] = DateTime.Now.Ticks;

                        order.FldTime_1 = DateTime.Now;

                    }

                }



            }

            orderProcessor.writeOrderCnt = counter;

            orderProcessor.processEndTime = DateTime.Now.Ticks;

            orderProcessor.orderTime = orderProcessor.processEndTime - orderProcessor.processStartTime;



            long orderTimems = (orderProcessor.orderTime / TimeSpan.TicksPerMillisecond);

            double avgOrder = 1.0 * orderTimems / counter;



            long orderMsgPerSec = (long)(orderTimems == 0 ? -1.0 : 1.0 * counter / orderTimems * 1000);



            Console.BackgroundColor = ConsoleColor.Blue;

            Console.ForegroundColor = ConsoleColor.White;

            Logger.Write(string.Format("*** End OrderProcessorThread PartionId:{0} OrderProcessorThread:{1} - Wrote Orders: {2} in {3} ms, average {4} ms, {5} orders in sec, start time {6} , end time {7} ",

                partionId, orderProcessor.WorkerID, counter, orderTimems, avgOrder, orderMsgPerSec, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff")));

            Console.ResetColor();



            for (int k = 1; k < intervalIdx; k++)

            {

                double totalTime = (intervals[k] - intervals[k - 1]) / TimeSpan.TicksPerMillisecond;

                double latency = (double)(1.0 * totalTime / (double)intervalSize);

                long recId = statRecord.Update(intervals[k] - intervals[k - 1]);



                Logger.Write(string.Format("Order Stat partionId:{0} WorkerThread:{1} RecID:{2} - Total time: {3} ms, for {4} messages, Latency: {5} ms",

                               partionId, orderProcessor.WorkerID, recId, (long)totalTime, intervalSize, latency));



                Logger.Write(string.Format("Order Stat: {0},{1},{2},{3},{4},{5},{6}",

                              partionId, orderProcessor.WorkerID, recId, latency, (long)totalTime, intervalSize,

                              (long)(intervals[k - 1] - intervals[0]) / TimeSpan.TicksPerMillisecond));



            }



            Logger.Write(string.Format("*** Exiting OrderProcessorThread PartionId:{0} OrderProcessorThread:{1}", partionId, orderProcessor.WorkerID));

            return 0;

        }



        public static int getPartitionId(long routingValue, ClusterInfo clusterInfo)

        {

            return (int)(safeAbs((int)routingValue) % clusterInfo.NumberOfInstances);

        }

        public static int safeAbs(int value)

        {

            return value == int.MinValue ? int.MaxValue : Math.Abs(value);

        }

    }

}

