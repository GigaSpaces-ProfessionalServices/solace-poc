using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using GigaSpaces.Core;
using GigaSpaces.Core.Metadata;

namespace GigaSpaces.Examples.ProcessingUnit.Common
{


    public class FillProcessor
    {
        public int WorkerID;

        public ConcurrentQueue<FillMsg> fillQueue;
        public long fillTime;
        public long fillCnt;
        public long lastGSFillID;
        public long processStartTime;
        //PVOID p;
        public long processEndTime;
        public ISpaceProxy spaceProxy;
        public FillProcessor(int workerID, ISpaceProxy SpaceProxy, ConcurrentQueue<FillMsg> queue, long FillCnt,
            long LastGSFillID)
        {
            WorkerID = workerID;
            fillQueue = queue;
            fillTime = 0;
            fillCnt = FillCnt;
            spaceProxy = SpaceProxy;
            lastGSFillID = LastGSFillID;
        }

    }
}