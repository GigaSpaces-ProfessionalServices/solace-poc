using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GigaSpaces.Examples.ProcessingUnit.Common
{
    class PUConfig
    {
        public bool isDR = false;
        public int totalSpaces = 2;
        public long primaryOrders = 100;
        public long backupOrders = 100;

        public PUConfig(IDictionary<string, string> properties)
        {
            string tempVal;

            properties.TryGetValue("TotalSpaces", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                totalSpaces = int.Parse(tempVal);

            properties.TryGetValue("PrimaryOrders", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                primaryOrders = int.Parse(tempVal);

            properties.TryGetValue("BackupOrders", out tempVal);
            if (!string.IsNullOrEmpty(tempVal))
                backupOrders = int.Parse(tempVal);
        }
    }
}
