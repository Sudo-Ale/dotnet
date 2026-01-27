using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Security.Principal;
using Hardware.Info;
using SystemInfo.Interfaces;
using SystemInfo.Services;
using SystemInfo.UI;
using SystemInfo.Data;

namespace SystemInfo
{
    class Execute
    {
        // saranno da rimuovere queste due liste
        private readonly List<string> errorList;
        private readonly List<FolderSizeInfo> _topFolders;


        private readonly HardwareInfo _hardwareInfo;
        private readonly IOutput _output;
        private readonly ISizeConverter _converter;

        public Execute()
        {
            errorList = new();
            _topFolders = new();

            _hardwareInfo = new();
            _output = new ConsoleOutput();
            _converter = new SizeConverter();
        }

        public void Start()
        {
            _output.WriteLine("----- SysInfo version 1.2.4 -----\n");

            // refresho le info hardware
            _output.WriteLine("Sto recuperando le info hardware, potrebbe richiedere qualche minuto...\n");
            _hardwareInfo.RefreshAll();

            // system info
            var systemInfoService = new SystemInfoService(_hardwareInfo);
            var systemPresenter = new SystemInfoPresenter(_output, _converter, systemInfoService);
            systemPresenter.ShowInfo();

            // Audio e Video
            var audioInfoService = new SoundInfoService(_hardwareInfo);
            var soundPresenter = new SoundInfoPresenter(_output, audioInfoService);
            soundPresenter.ShowInfo();

            var videoInfoService = new VideoInfoService(_hardwareInfo);
            var videoPresenter = new VideoInfoPresenter(_output, videoInfoService);
            videoPresenter.ShowInfo();

            // Drive storage
            var driveInfoService = new DriveInfoService();
            var drivePresenter = new DriveInfoPresenter(_output, _converter, driveInfoService);
            drivePresenter.ShowInfo();

            // Cartelle info
            var directoryInfoService = new DirectoryInfoServices(_converter);
            var directoryPresenter = new DirectoryInfoPresenter(_output, _converter, directoryInfoService);
            directoryPresenter.ShowInfo();
        }
    }
}
