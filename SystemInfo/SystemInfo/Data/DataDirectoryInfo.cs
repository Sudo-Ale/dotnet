using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Data
{
    class DataDirectoryInfo
    {
        public string PathName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string VolumeLabel { get; set; } = string.Empty;
        public string DriveFormat { get; set; } = string.Empty;
        public long AvailableFreeSpace { get; set; }
        public long TotalFreeSpace { get; set; }
        public long TotalSize { get; set; }
        public double UsedPercentage => TotalSize == 0 ? 0 : Math.Round((double)(TotalSize - TotalFreeSpace) / TotalSize * 100, 2);
        public string RootName { get; set; } = string.Empty;
    }
}
