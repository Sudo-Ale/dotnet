using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.UI
{
    class DriveInfoPresenter : IPresenter
    {
        private readonly IOutput _output;
        private readonly IDriveAnalyzer _driveAnalyzer;
        private readonly ISizeConverter _converter;

        public DriveInfoPresenter(IOutput output, ISizeConverter converter, IDriveAnalyzer driveAnalyzer)
        {
            _output = output;
            _driveAnalyzer = driveAnalyzer;
            _converter = converter;
        }

        public void ShowInfo()
        {
            var drives = _driveAnalyzer.AnalyzeDrives();

            _output.WriteLine("\n=== Informazioni Drive ===");

            foreach (var drive in drives)
            {
                _output.WriteLine("Drive {0}", drive.Name);
                _output.WriteLine("Drive type: {0}", drive.DriveType);
                _output.WriteLine("Volume label: {0}", drive.VolumeLabel);
                _output.WriteLine("File system: {0}", drive.DriveFormat);

                {
                    var (value, unit) = _converter.Convert(drive.AvailableFreeSpace);
                    _output.WriteLine("Available free space: {0} {1}", value, unit);
                }
                {
                    var (value, unit) = _converter.Convert(drive.TotalFreeSpace);
                    _output.WriteLine("Total free space: {0} {1}", value, unit);
                }
                {
                    var (value, unit) = _converter.Convert(drive.TotalSize);
                    _output.WriteLine("Total size: {0} {1}", value, unit);
                }

                _output.WriteLine("Percentage used: {0}%", drive.PercentageUsed);
            }
        }
    }
}
