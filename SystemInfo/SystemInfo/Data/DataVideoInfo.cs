using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Data
{
    class DataVideoInfo
    {
        // Video controller
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public uint CurrentRefreshRate { get; set; }
        public uint MinRefreshRate { get; set; }
        public uint MaxRefreshRate { get; set; }
        public uint CurrentHorizontalResolution { get; set; }
        public uint CurrentVerticalResolution { get; set; }
    }
}
