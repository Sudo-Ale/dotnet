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
    class SystemInfoService : ISystemAnalyzer
    {
        private readonly HardwareInfo _hardwareInfo;

        public SystemInfoService(HardwareInfo hardwareInfo)
        {
            _hardwareInfo = hardwareInfo;
        }

        public DataSystemInfo Analyze()
        {
            return new DataSystemInfo
            {
                NameOperatingSystem = _hardwareInfo.OperatingSystem.Name,
                Version = _hardwareInfo.OperatingSystem.Version.ToString(),
                
                TotalRam = _hardwareInfo.MemoryStatus.TotalPhysical,
                AvailableRam = _hardwareInfo.MemoryStatus.AvailablePhysical,

                NameDevice = _hardwareInfo.ComputerSystemList.FirstOrDefault()?.Name ?? "N/A",
                VendorDevice = _hardwareInfo.ComputerSystemList.FirstOrDefault()?.Vendor ?? "N/A",
                NameCPU = _hardwareInfo.CpuList.FirstOrDefault()?.Name ?? "N/A",
                CoresNumber = _hardwareInfo.CpuList.FirstOrDefault()?.NumberOfCores ?? 0,
                LogicalNumberProcessor = _hardwareInfo.CpuList.FirstOrDefault()?.NumberOfLogicalProcessors ?? 0,
            };
        }
    }
}
