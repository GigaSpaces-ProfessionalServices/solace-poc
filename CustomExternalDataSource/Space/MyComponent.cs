using GigaSpaces.XAP.ProcessingUnit.Containers;
using GigaSpaces.XAP.ProcessingUnit.Containers.BasicContainer;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomExternalDataSource.Space
{
    [BasicProcessingUnitComponent]
    class MyComponent
    {
        private int? _instanceId;
        private int? _pid;

        public int? InstanceId
        {
            get { return _instanceId; }
            private set { _instanceId = value; }
        }
        public int? Pid {
            get { return _pid; }
            private set { _pid = value; }
        }
        [ContainerInitialized]
        public void Initialize(BasicProcessingUnitContainer container)
        {
            _instanceId = ProcessingUnitContainer.Current.ClusterInfo.InstanceId;
            _pid = Process.GetCurrentProcess().Id;
            GlobalDiagnosticsContext.Set("InstanceId", InstanceId);
            GlobalDiagnosticsContext.Set("Pid", Pid);
        }
    }
}
