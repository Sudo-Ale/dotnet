using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;
using SystemInfo.Services;

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

        public void ShowDrive(DataDriveInfo drive)
        {
            _output.WriteLine($"Drive {drive.AudioCaption}");
        }
    }
}
