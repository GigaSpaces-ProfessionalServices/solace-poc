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
    class CommonUtil
    {
        public static void executeOperationWithRetry(int totalRetries, int retryWaitTime,ISpaceProxy spaceProxy, IdQuery<GS_Order> id_gs_order, ChangeSet change , String operation, object fillOrder, ITransaction tx, Boolean transactionCommit)
        {
            int retries = totalRetries;
            /* GS_Fill gS_Fill = null;
                    GS_Order gS_Order = null;

                    if (fillOrder is GS_Fill) { 
                        gS_Fill = (GS_Fill)fillOrder; 
                    }
                    if (fillOrder is GS_Order)
                    {
                        gS_Order = (GS_Order)fillOrder;
                    }*/
            if (operation == "Write")
            {
                spaceProxy.Write(fillOrder, tx, long.MaxValue, 1000);
            }
            else if (operation == "Change")
            {
                spaceProxy.Change<GS_Order>(id_gs_order, change, tx, 1000, ChangeModifiers.MemoryOnlySearch);
            }
            else if (operation == "Read")
            {
                spaceProxy.Read<GS_Order>(id_gs_order, tx, 1000 * 60, ReadModifiers.ExclusiveReadLock);
            }
            while (retries > 0)
            {
                try
                {
                   
                    if (transactionCommit)
                    {
                        tx.Commit(1000 * 60);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Thread.Sleep(retryWaitTime);
                    if (retries == totalRetries)
                    {
                        Logger.Write("retrying 1st time ...");
                    }
                    if (retries == 1)
                    {
                        Logger.Write("SEVERE : Max limit reached , " + ex.StackTrace);
                        tx.Abort();
                        throw;
                    }
                    retries--;
                }
            }
        }
    }
}
