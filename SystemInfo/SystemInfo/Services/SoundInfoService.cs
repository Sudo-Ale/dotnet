using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class SoundInfoService : ISoundAnalyzer
    {
        private readonly HardwareInfo _hardwareInfo;

        public SoundInfoService(HardwareInfo hardwareInfo)
        {
            _hardwareInfo = hardwareInfo;
        }

        public IEnumerable<DataSoundInfo> AnalyzeSound() 
        {
            _hardwareInfo.RefreshAll();
            
            var result = new List<DataSoundInfo>();

            foreach (var audio in _hardwareInfo.SoundDeviceList)
            {
                result.Add(new DataSoundInfo
                {
                    AudioCaption = audio.Caption ?? "N/A",
                    AudioDescription = audio.Description ?? "N/A",
                    AudioManufacturer = audio.Manufacturer ?? "N/A",
                    AudioName = audio.Name ?? "N/A",    
                    AudioProductName = audio.ProductName ?? "N/A",
                });
            }
            return result;
        }
    }
}
