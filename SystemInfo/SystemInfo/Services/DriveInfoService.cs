using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;
using SystemInfo.UI;

namespace SystemInfo.Services
{
    class DriveInfoService : IDriveAnalyzer
    {
        public IEnumerable<DataDriveInfo> AnalyzeAllDrives() 
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            var result = new List<DataDriveInfo>();

            foreach (var d in allDrives)
            {
                if (!d.IsReady)
                    continue;

                result.Add(new DataDriveInfo
                {
                    Name = d.Name,
                    DriveType = d.DriveType.ToString(),
                    VolumeLabel = d.VolumeLabel,
                    DriveFormat = d.DriveFormat,
                    AvailableFreeSpace = d.AvailableFreeSpace,
                    TotalFreeSpace = d.TotalFreeSpace,
                    TotalSize = d.TotalSize,
                    PercentageUsed = Math.Round((double)(d.TotalSize - d.TotalFreeSpace) / d.TotalSize * 100, 2)
                    //100.0 * (drive.TotalSize - drive.TotalFreeSpace) / drive.TotalSize;
                });
            }
            
            return result;
        }
    }
}
