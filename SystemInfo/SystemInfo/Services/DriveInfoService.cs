using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class DriveInfoService
    {
        private readonly HardwareInfo _hardwareInfo;

        public DriveInfoService(HardwareInfo hardwareInfo)
        {
            _hardwareInfo = hardwareInfo;
        }

        public DataDriveInfo GetDriveInfo() 
        {
            _hardwareInfo.RefreshAll();

            return new DataDriveInfo
            {
                AudioCaption = _hardwareInfo.SoundDeviceList.FirstOrDefault()?.Caption ?? "N/A",
                AudioDescription = _hardwareInfo.SoundDeviceList.FirstOrDefault()?.Description ?? "N/A",
                AudioManufacturer = _hardwareInfo.SoundDeviceList.FirstOrDefault()?.Manufacturer ?? "N/A",
                AudioName = _hardwareInfo.SoundDeviceList.FirstOrDefault()?.Name ?? "N/A",
                AudioProductName = _hardwareInfo.SoundDeviceList.FirstOrDefault()?.ProductName ?? "N/A",

                VideoName = _hardwareInfo.VideoControllerList.FirstOrDefault()?.Name ?? "N/A",
                VideoDescription = _hardwareInfo.VideoControllerList.FirstOrDefault()?.Description ?? "N/A",
                VideoCurrentRefreshRate = _hardwareInfo.VideoControllerList.FirstOrDefault()?.CurrentRefreshRate ?? 0,
                VideoMinRefreshRate = _hardwareInfo.VideoControllerList.FirstOrDefault()?.MinRefreshRate ?? 0,
                VideoMaxRefreshRate = _hardwareInfo.VideoControllerList.FirstOrDefault()?.MaxRefreshRate ?? 0,
                VideoCurrentHorizontalResolution = _hardwareInfo.VideoControllerList.FirstOrDefault()?.CurrentHorizontalResolution ?? 0
            };
        }
    }
}
