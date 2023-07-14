using System;


using GigaSpaces.Core;
using GigaSpaces.Core.Linq;
using GigaSpaces.Examples.ProcessingUnit.Common;

namespace Query
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine + "Welcome to XAP.NET Query Example!" + Environment.NewLine);
            long fillCount = 0;
            long execShares = 0;
            try
            {
                String spaceName = args.Length == 0 ? "dataExampleSpace" : args[0];
                spaceName = "dataExampleSpace";
                ISpaceProxy spaceProxy;
                Console.WriteLine("*** Connecting to remote space named '" + spaceName + "'...");
                SpaceProxyFactory spaceProxyFactory = new SpaceProxyFactory(spaceName);
  
                spaceProxyFactory.LookupLocators = "NY5DVET007:4174";
                TimeSpan ts = new TimeSpan(10000*1000*30);
                spaceProxyFactory.LookupTimeout = ts;
                Console.WriteLine(Environment.NewLine + "Creating connection!" + Environment.NewLine);
                spaceProxy = spaceProxyFactory.Create();

                // Connect to space:
                Console.WriteLine("*** Connected to space.");
                Console.WriteLine();
                /*
				// Write a message to the space:
				Random random = new Random();
				Message outgoingMessage = new Message {Text = "Hello World " + random.Next(1, 1001)};
				Console.WriteLine("Writing Message [" + outgoingMessage.Text + "]");
				spaceProxy.Write(outgoingMessage);
				*/

                // Read a message from the space:
                //Message incomingMessage = spaceProxy.Take(new Message());
                //Console.WriteLine("Took Message [" + incomingMessage.Text + "]");


                // GS_Fill[] fills = spaceProxy.ReadMultiple<GS_Fill>(new SqlQuery<GS_Fill>(""));

                int maxFillID;
                int maxOrderID;
                //spaceProxy.

              /* var queryableOrer = from p in spaceProxy.Query<GS_Fill>("") select p;
                maxFillID = queryableOrer.Max(p => p.FillID);

                var queryableFill = from p in spaceProxy.Query<GS_Order>("") select p;
                maxOrderID = queryableFill.Max(p => p.OrderID);
                
                */
                int batchLog = 1000;
                foreach (GS_Fill fill in spaceProxy.GetSpaceIterator<GS_Fill>(new SqlQuery<GS_Fill>("")))
                {
                    fillCount++;
                    execShares = execShares + fill.LastShares;
                    if ((fillCount % batchLog) == 0)
                        Console.WriteLine("Processing fill read {0} fills, execShares {1}", fillCount, execShares);
                }

                spaceProxy.Dispose();

                Console.WriteLine(Environment.NewLine + "Query Example finished successfully!" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + "Query Example failed: " + ex);
            }

            Console.WriteLine("Total fill read {0} fills, execShares {1}", fillCount, execShares);
            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();

        }
    }
}
