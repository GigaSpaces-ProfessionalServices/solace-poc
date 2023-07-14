using GigaSpaces.Examples.ProcessingUnit.Common;
using System;

using GigaSpaces.Core;
using GigaSpaces.XAP.Events;

namespace Notify
{
    class Notify
    {
        private ISpaceProxy _iSpaceProxy;
        public Notify(ISpaceProxy iSpaceProxy)
        {
            _iSpaceProxy = iSpaceProxy;
        }
        public static void Main(string[] args)
        {
            Console.WriteLine(Environment.NewLine + "Welcome to XAP.NET Notify Example!" + Environment.NewLine);

            try
            {
                String spaceName = args.Length == 0 ? "dataExampleSpace" : args[0];
                ISpaceProxy spaceProxy;
                Console.WriteLine("*** Connecting to remote space named '" + spaceName + "'...");
                spaceProxy = new SpaceProxyFactory(spaceName).Create();

                // Connect to space:
                Console.WriteLine("*** Connected to space.");
                Console.WriteLine();

                Notify program = new Notify(spaceProxy);

                //Thread thread = new Thread(new ThreadStart(program.Run));
                //thread.Start();

                program.Run();




                // when needed dispose of the container
                //notifyEventListenerContainer.Dispose();
                //spaceProxy.Dispose();

                Console.WriteLine(Environment.NewLine + "Notify Example finished successfully!" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(Environment.NewLine + "Notify Example failed: " + ex);
            }

        }
        public void Run()
        {
            IEventListenerContainer<GS_Order> eventListenerContainer = EventListenerContainerFactory.CreateContainer<GS_Order>(_iSpaceProxy, new SimpleListener());

            eventListenerContainer.Start();

            Console.WriteLine("Press ENTER to exit.");
            Console.ReadLine();

        }

    }
}
