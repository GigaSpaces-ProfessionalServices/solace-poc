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
    class FillProcessorThread
    {
        private static Random random = new Random();
        public static string getRandomString(int length)
        {
            const string chars = "FIL_123";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public int threadRun(FillProcessor fillProcessor, ISpaceProxy spaceProxy, ClusterInfo clusterInfo,
                            long intervalSize, StatRecord statRecord)
        {
            long partionId = (long)(clusterInfo.InstanceId - 1);
            long[] intervals = new long[200];
            int intervalIdx = 0;

            Logger.Write(string.Format("*** Started FillProcessorThread partionId:{0} WorkerThread:{1}", partionId, fillProcessor.WorkerID));

            Logger.Write("*** FillProcessorThread - Getting txManager ");
            ITransactionManager txManager = GigaSpacesFactory.CreateDistributedTransactionManager();
            Logger.Write("*** FillProcessorThread - Created txManager ");


            int k = 1;
            GS_Fill fill = new GS_Fill();
            fill.FldInt_1 = k;
            fill.FldInt_2 = k;
            fill.FldInt_3 = k;
            fill.FldInt_4 = k;
            fill.FldInt_5 = k;
            fill.FldInt_6 = k;
            fill.FldInt_7 = k;
            fill.FldInt_8 = k;
            fill.FldInt_9 = k;
            fill.FldInt_10 = k;
            fill.FldInt_11 = k;
            fill.FldTime_1 = DateTime.Now;
            fill.FldTime_2 = DateTime.Now;
            fill.FldTime_3 = DateTime.Now;
            fill.FldTime_4 = DateTime.Now;
            fill.FldDbl_1 = random.Next(10, 20 + k) % 10000 / 32;
            fill.FldDbl_2 = random.Next(10, 20 + k) % 10000 / 32;
            fill.FldDbl_3 = random.Next(10, 20 + k) % 10000 / 32;
            fill.FldStr_1 = getRandomString(random.Next(1, 10));
            fill.FldStr_2 = getRandomString(random.Next(1, 10));
            fill.FldStr_3 = getRandomString(random.Next(1, 10));
            fill.FldStr_4 = getRandomString(random.Next(1, 10));
            fill.FldStr_5 = getRandomString(random.Next(1, 10));
            fill.FldStr_6 = getRandomString(random.Next(1, 10));
            fill.FldStr_7 = getRandomString(random.Next(1, 10));
            fill.FldStr_8 = getRandomString(random.Next(1, 10));
            fill.FldStr_9 = getRandomString(random.Next(1, 10));
            fill.FldStr_10 = getRandomString(random.Next(1, 10));
            fill.FldStr_11 = getRandomString(random.Next(1, 10));
            fill.FldStr_12 = getRandomString(random.Next(1, 10));
            fill.FldStr_13 = getRandomString(random.Next(1, 10));
            fill.FldStr_14 = getRandomString(random.Next(1, 10));
            fill.FldStr_15 = getRandomString(random.Next(1, 10));
            fill.FldStr_16 = getRandomString(random.Next(1, 10));
            fill.FldStr_17 = getRandomString(random.Next(1, 10));
            fill.FldStr_18 = getRandomString(random.Next(1, 10));
            fill.FldStr_19 = getRandomString(random.Next(1, 10));
            fill.FldStr_20 = getRandomString(random.Next(1, 10));
            /*
            fill.FldStr_21 = getRandomString(random.Next(1, 10));
            fill.FldStr_22 = getRandomString(random.Next(1, 10));
            fill.FldStr_23 = getRandomString(random.Next(1, 10));
            fill.FldStr_24 = getRandomString(random.Next(1, 10));
            fill.FldStr_25 = getRandomString(random.Next(1, 10));
           
            fill.FldStr_26 = getRandomString(random.Next(1, 10));
            fill.FldStr_27 = getRandomString(random.Next(1, 10));
            fill.FldStr_28 = getRandomString(random.Next(1, 10));
            fill.FldStr_29 = getRandomString(random.Next(1, 10));
            fill.FldStr_30 = getRandomString(random.Next(1, 10));
            fill.FldStr_31 = getRandomString(random.Next(1, 10));
            fill.FldStr_32 = getRandomString(random.Next(1, 10));
            fill.FldStr_33 = getRandomString(random.Next(1, 10));
            fill.FldStr_34 = getRandomString(random.Next(1, 10));
            fill.FldStr_35 = getRandomString(random.Next(1, 10));
            fill.FldStr_36 = getRandomString(random.Next(1, 10));
            fill.FldStr_37 = getRandomString(random.Next(1, 10));
            fill.FldStr_38 = getRandomString(random.Next(1, 10));
            fill.FldStr_39 = getRandomString(random.Next(1, 10));
            fill.FldStr_40 = getRandomString(random.Next(1, 10));
            fill.FldStr_41 = getRandomString(random.Next(1, 10));
            fill.FldStr_42 = getRandomString(random.Next(1, 10));
            fill.FldStr_43 = getRandomString(random.Next(1, 10));
            fill.FldStr_44 = getRandomString(random.Next(1, 10));
            fill.FldStr_45 = getRandomString(random.Next(1, 10));
            fill.FldStr_46 = getRandomString(random.Next(1, 10));
            fill.FldStr_47 = getRandomString(random.Next(1, 10));
            fill.FldStr_48 = getRandomString(random.Next(1, 10));
            fill.FldStr_49 = getRandomString(random.Next(1, 10));
            fill.FldStr_50 = getRandomString(random.Next(1, 10));
            */
            //           fill.OPID = partionId;

            /*long nextFillID =  1;
            long lastFillID = fillProcessor.fillCnt;*/


            long nextFillID = fillProcessor.lastGSFillID + 1 + ((fillProcessor.WorkerID - 1) * fillProcessor.fillCnt);
            long lastFillID = nextFillID + fillProcessor.fillCnt - 1;

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            string startTimeStr = DateTime.Now.ToString("HH:mm:ss.fff");
            Logger.Write(string.Format("*** Start FillProcessorThread partionId:{0} WorkerThread{1} nextFillID {2}, lastFillID {3}, Ticks {4} , current time {5} current time",
                        partionId, fillProcessor.WorkerID, nextFillID, lastFillID, DateTime.Now.Ticks, startTimeStr));
            Console.ResetColor();
            fillProcessor.fillCnt = 0;

            fillProcessor.processStartTime = DateTime.Now.Ticks;
            intervals[intervalIdx++] = fillProcessor.processStartTime;
            fill.FldTime_1 = DateTime.Now;

            long totalSleepTimems = 0;
            long intervalSleepTimems = 0;

            int noOfInstances = (int)clusterInfo.NumberOfInstances;
            nextFillID = nextFillID + partionId;
            while (nextFillID <= lastFillID)
            {
                //   Console.WriteLine(fillProcessor.fillQueue.Count);
                //                if (!fillProcessor.fillQueue.Any())
                if (fillProcessor.fillQueue.Count <= 0)
                {
                    Thread.Sleep(500);
                    totalSleepTimems += 500;
                    intervalSleepTimems += 500;
                    continue;
                }

                FillMsg newFillMsg;
                fillProcessor.fillQueue.TryDequeue(out newFillMsg);


                //  Console.WriteLine(newFillMsg);
                // Done with processing
                if (newFillMsg.OrderID == -1)
                {
                    break;
                }
                if (getPartitionId(newFillMsg.OrderID, clusterInfo) != partionId)
                {
                    continue;
                }
                // fillTimer.StartTimer();
                long fillTImeElapsed = DateTime.Now.Ticks;
                ITransaction tx2 = txManager.Create();

                ICollection<String> projections = new List<String>();
                projections.Add(new String("OrderID".ToCharArray()));
                projections.Add(new String("Symbol".ToCharArray()));
                projections.Add(new String("CalExecValue".ToCharArray()));
                projections.Add(new String("CalCumQty".ToCharArray()));
                IdQuery<GS_Order> idQuery = new IdQuery<GS_Order>(newFillMsg.OrderID);
                idQuery.Projections = projections;

                GS_Order orderRead = spaceProxy.Read<GS_Order>(idQuery, tx2, 1000 * 60, ReadModifiers.ExclusiveReadLock);
                //GS_Order orderRead = spaceProxy.Read<GS_Order>(idQuery);
                //GS_Order orderRead = spaceProxy.Read<GS_Order>(idQuery, tx2);

                fill.FillID = nextFillID;
                //                fill.FillID = nextFillID++;
                fill.OrderID = newFillMsg.OrderID;
                fill.LastShares = newFillMsg.LastShares;
                fill.LastPrice = newFillMsg.LastPrice;
                nextFillID = nextFillID + noOfInstances;

                //Console.WriteLine("Writting Order: {0} {1} ", order.OrderID, order.Symbol);
                spaceProxy.Write(fill, tx2, long.MaxValue, 1000);
                // spaceProxy.Write(fill);
                //Console.WriteLine("FillProcessorThread {0} - added fill", fillProcessor.WorkerID );

                /*    GS_Order orderWrite = new GS_Order();
                    orderWrite.OrderID = orderRead.OrderID;*/
                IdQuery<GS_Order> orderWrite = new IdQuery<GS_Order>(newFillMsg.OrderID);


                ChangeSet orderChange = new ChangeSet();
                orderChange.Increment("CalCumQty", newFillMsg.LastShares);
                orderChange.Increment("CalExecValue", (newFillMsg.LastPrice * newFillMsg.LastShares));
                IChangeResult<GS_Order> orderChangeResults =
                      spaceProxy.Change<GS_Order>(orderWrite, orderChange, tx2, 1000, ChangeModifiers.MemoryOnlySearch);
                //spaceProxy.Change<GS_Order>(orderWrite, orderChange);
                tx2.Commit(1000 * 60);


                fillProcessor.fillCnt++;

                if ((fillProcessor.fillCnt % intervalSize) == 0)
                {
                    if (intervalIdx < intervals.Length)
                    {

                        intervals[intervalIdx++] = (DateTime.Now.Ticks - (intervalSleepTimems * TimeSpan.TicksPerMillisecond));
                        intervalSleepTimems = 0;
                        fill.FldTime_1 = DateTime.Now;
                    }
                }
            }


            fillProcessor.processEndTime = DateTime.Now.Ticks;
            fillProcessor.fillTime = fillProcessor.processEndTime - fillProcessor.processStartTime;

            long fillTimems = (fillProcessor.fillTime / TimeSpan.TicksPerMillisecond) - totalSleepTimems;
            double avgFill = 1.0 * fillTimems / fillProcessor.fillCnt;

            long fillMsgPerSec = (long)((fillTimems == 0) ? -1.0 : 1.0 * fillProcessor.fillCnt / fillTimems * 1000);

            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            Logger.Write(string.Format("*** End FillProcessorThread partionId:{0} WorkerThread:{1} - Wrote Fills: {2} in {3} ms, average {4} ms, {5} fills in sec,  {6} start time, {7} end time, {8} totalSleepTimems",
                 partionId, fillProcessor.WorkerID, fillProcessor.fillCnt, fillTimems, avgFill, fillMsgPerSec, startTimeStr, DateTime.Now.ToString("HH:mm:ss.fff"), totalSleepTimems));
            Console.ResetColor();

            for (int i = 1; i < intervalIdx; i++)
            {
                double totalTime = (intervals[i] - intervals[i - 1]) / TimeSpan.TicksPerMillisecond;
                double latency = (double)(1.0 * totalTime / (double)intervalSize);
                long recId = statRecord.Update(intervals[i] - intervals[i - 1]);

                Logger.Write(string.Format("Fill Stat partionId:{0} WorkerThread:{1} RecID:{2} - Total time: {3} ms, for {4} messages, Latency: {5} ms",
                               partionId, fillProcessor.WorkerID, recId, (long)totalTime, intervalSize, latency));

                Logger.Write(string.Format("Fill Stat: {0},{1},{2},{3},{4},{5},{6}",
                              partionId, fillProcessor.WorkerID, recId, latency, (long)totalTime, intervalSize,
                              (long)(intervals[i - 1] - intervals[0]) / TimeSpan.TicksPerMillisecond));

            }


            Logger.Write(string.Format("*** Exiting FillProcessorThread partionId:{0} WorkerThread{1}",
                partionId, fillProcessor.WorkerID));
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
