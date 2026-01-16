using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Data
{
    class DataSystemInfo
    {
        // os
        public string NameOperatingSystem { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;

        // System ram
        public ulong TotalRam { get; set; }
        public ulong AvailableRam { get; set; }

        // System device
        public string NameDevice { get; set; } = string.Empty;
        public string VendorDevice { get; set; } = string.Empty;
        public string NameCPU { get; set; } = string.Empty;
        public uint CoresNumber { get; set; }
        public uint LogicalNumberProcessor { get; set; }
    }
}
