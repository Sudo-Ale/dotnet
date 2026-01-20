using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.UI
{
    class DriveInfoPresenter
    {
        private readonly IOutput _output;
        private readonly ISizeConverter _converter;

        public DriveInfoPresenter(IOutput output, ISizeConverter converter) 
        {
            _output = output;
            _converter = converter;
        }

        public void PrintDriveInfo(DataDriveInfo drive)
        {
            _output.WriteLine("Drive {0}", drive.Name);
            _output.WriteLine("Drive type: {0}", drive.DriveType);
            _output.WriteLine("Volume label: {0}", drive.VolumeLabel);
            _output.WriteLine("File system: {0}", drive.DriveFormat);
            _output.WriteLine("Available free space: {0}", drive.AvailableFreeSpace);
            _output.WriteLine("Total free space: {0}", drive.TotalFreeSpace);
            _output.WriteLine("Total size: {0}", drive.TotalSize);
            _output.WriteLine("Percentage used: {0}", drive.PercentageUsed);
        }
    }
}
