using GigaSpaces.Core;
using GigaSpaces.Examples.ProcessingUnit.Common;
using GigaSpaces.XAP.Events;
using GigaSpaces.XAP.Events.Notify;
using System;

namespace Notify
{
    [NotifyEventDriven(Name ="OrderProcessor")]
    public class SimpleListener
    {
        [EventTemplate]
        public SqlQuery<GS_Order> SelectData
        {
            get
            {
                /*
                GS_Fill template = new GS_Fill();
                template.Processed = false;
                return template;
                */
                SqlQuery<GS_Order> template = new SqlQuery<GS_Order>("CumQty = 0");
                return template;
            }
        }

        [DataEventHandler]
        public void ProcessData(GS_Order order)
        {
            //process GS_Fill here and return processed data
            Console.WriteLine("Processing fill, order,OrderID: " + order.OrderID);
            //fill.Processed = true;
           // return order;        
        }

    }
}
