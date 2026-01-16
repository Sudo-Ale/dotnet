using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Data
{
    class DataDriveInfo
    {
        // Sound device
        public string AudioCaption { get; set; } = string.Empty;
        public string AudioDescription { get; set; } = string.Empty;
        public string AudioManufacturer { get; set; } = string.Empty;
        public string AudioName { get; set; } = string.Empty;
        public string AudioProductName { get; set; } = string.Empty;

        // Video controller
        public string VideoName { get; set; } = string.Empty;
        public string VideoDescription { get; set; } = string.Empty;
        public uint VideoCurrentRefreshRate { get; set; }
        public uint VideoMinRefreshRate { get; set; }
        public uint VideoMaxRefreshRate { get; set; }
        public uint VideoCurrentHorizontalResolution { get; set; }
        public uint VideoCurrentVerticalResolution { get; set; }
    }
}
