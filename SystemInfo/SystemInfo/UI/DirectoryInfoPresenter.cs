using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Interfaces;
using SystemInfo.Services;

namespace SystemInfo.UI
{
    class DirectoryInfoPresenter : IPresenter
    {
        private readonly IOutput _output;
        private readonly IDirectoryAnalyzer _directoryAnalyzer;
        private readonly ISizeConverter _converter;
        private int _topN = 10;
        public DirectoryInfoPresenter(IOutput output, ISizeConverter converter,IDirectoryAnalyzer directoryAnalyzer)
        {
            _output = output;
            _directoryAnalyzer = directoryAnalyzer;
            _converter = converter;
        }

        public void ShowInfo()
        {
            _output.WriteLine("\n---- Inizio Analisi Drive ----");

            var rootAnalyzer = _directoryAnalyzer.AnalyzeRootDrive();

            foreach (var dir in rootAnalyzer)
            {
                var res = _converter.Convert(dir.TotalSize);
                _output.WriteLine($"Cartella: {dir.Name} - {res.Value} {res.Unit}");
                _output.WriteLine("---------------------------------------");
            }
            _output.WriteLine("---- Fine Analisi Drive ----\n");

            _output.WriteLine("---------------------------------------\n");

            _output.WriteLine("---- Inizio Analisi Sottocartelle Drive ----");
            var topByDrive = _directoryAnalyzer.AnalyzeTopFoldersPerDrive(_topN);
            foreach (var driveResult in topByDrive)
            {
                _output.WriteLine($"\nDrive: {driveResult.DriveName}");

                foreach (var folder in driveResult.TopFolders)
                {
                    var res = _converter.Convert(folder.Size);
                    _output.WriteLine($"  {folder.Path} - {res.Value} {res.Unit}");
                }
            }
        }
    }
}
