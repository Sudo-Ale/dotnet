using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class VideoInfoService : IVideoAnalyzer
    {
        public HardwareInfo _hardwareInfo;
        public VideoInfoService(HardwareInfo hardware) 
        {
            _hardwareInfo = hardware;
        }

        public IEnumerable<DataVideoInfo> AnalyzeVideo()
        {
            var result = new List<DataVideoInfo>();

            foreach (var drive in _hardwareInfo.VideoControllerList)
            {
                result.Add(new DataVideoInfo
                {
                    Name = drive.Name,
                    Description = drive.Description,
                    CurrentRefreshRate = drive.CurrentRefreshRate,
                    MinRefreshRate = drive.MinRefreshRate,
                    MaxRefreshRate = drive.MaxRefreshRate,
                    CurrentHorizontalResolution = drive.CurrentHorizontalResolution,
                    CurrentVerticalResolution = drive.CurrentVerticalResolution
                });
            }

            return result;
        }
    }
}
