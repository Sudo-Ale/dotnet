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


            // generic info hardware e os
            //GenericSysInfo();

            //// info sui drive e cartelle
            //RetrieveInformationsOnDrives();

            //// top n cartelle più pesanti
            //foreach (var folder in _topFolders.OrderByDescending(f => f.Size))
            //{
            //    var sizeConverted = ConvertSize((long)folder.Size);
            //    _output.WriteLine("{0, 15}",
            //        $"{folder.Path} {sizeConverted.Value.ToString("0.##", CultureInfo.InvariantCulture)} {sizeConverted.Unit}");
            //}
        }

        // Hardware.Info
        // recupero info generiche sul sistema
        // fatto come oop per future estensioni
        private void GenericSysInfo()
        {
            _output.WriteLine($"Sistema operativo: \n{_hardwareInfo.OperatingSystem}");

            var totalRAM = ConvertSize((long)_hardwareInfo.MemoryStatus.TotalPhysical);
            _output.WriteLine($"RAM Totale: {totalRAM.Value.ToString("0.##", CultureInfo.InvariantCulture)} {totalRAM.Unit}");

            var availableRAM = ConvertSize((long)_hardwareInfo.MemoryStatus.AvailablePhysical);
            _output.WriteLine($"RAM Disponibile: {availableRAM.Value.ToString("0.##", CultureInfo.InvariantCulture)} {availableRAM.Unit}\n");

            foreach (var hardware in _hardwareInfo.ComputerSystemList)
            {
                _output.WriteLine($"Nome Dispositivo: {hardware.Name}.");
                _output.WriteLine($"Vendor: {hardware.Vendor}.");
            }

            foreach (var hardware in _hardwareInfo.CpuList)
            {
                _output.WriteLine($"Nome Processore: {hardware.Name}.");
                _output.WriteLine($"Numero dei Core: {hardware.NumberOfCores}.");
                _output.WriteLine($"Numero dei processori logici: {hardware.NumberOfLogicalProcessors}.\n");
            }

            int indexAudio = 0;
            foreach (var hardware in _hardwareInfo.SoundDeviceList)
            {
                _output.WriteLine($"--- Scheda audio #{indexAudio} ---");
                _output.WriteLine(hardware.Caption);
                _output.WriteLine(hardware.Description);
                _output.WriteLine(hardware.Manufacturer);
                _output.WriteLine(hardware.Name);
                _output.WriteLine(hardware.ProductName);

                indexAudio++;
            }

            int indexVideo = 0;
            foreach (var hardware in _hardwareInfo.VideoControllerList)
            {
                _output.WriteLine($"--- Scheda video #{indexVideo} ---");
                _output.WriteLine($"Name: {hardware.Name}.");
                _output.WriteLine($"Desrizione: {hardware.Description}.");
                _output.WriteLine($"Refresh rate attuale: {hardware.CurrentRefreshRate}.");
                _output.WriteLine($"Refresh rate min: {hardware.MinRefreshRate}.");
                _output.WriteLine($"Refresh rate max: {hardware.MaxRefreshRate}.");
                _output.WriteLine($"Refresh rate orizzontale x verticale: {hardware.CurrentHorizontalResolution} x {hardware.CurrentVerticalResolution}.\n");

                indexVideo++;
            }
        }

        // Info sul drive e le sue cartelle
        // fatto come oop per future estensioni
        private void RetrieveInformationsOnDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var drive in allDrives)
            {
                _output.WriteLine("Drive {0}", drive.Name);
                _output.WriteLine("  Drive type: {0}", drive.DriveType);

                if (!drive.IsReady)
                {
                    _output.WriteLine("Driver non pronto");
                    continue;
                }

                _output.WriteLine("  Volume label: {0}", drive.VolumeLabel);
                _output.WriteLine("  File system: {0}", drive.DriveFormat);

                var availableFreeSpace = ConvertSize((long)drive.AvailableFreeSpace);
                _output.WriteLine(
                    "  Spazio libero per l'utente:    {0, 15}",
                    $"{availableFreeSpace.Value.ToString("0.##", CultureInfo.InvariantCulture)} {availableFreeSpace.Unit}");


                var totalFreeSpace = ConvertSize((long)drive.TotalFreeSpace);
                _output.WriteLine(
                    "  Spazio totale libero:          {0, 15}",
                    $"{totalFreeSpace.Value.ToString("0.##", CultureInfo.InvariantCulture)} {totalFreeSpace.Unit}");

                var totalSize = ConvertSize((long)drive.TotalSize);
                _output.WriteLine(
                    "  Spazio totale:                 {0, 15}",
                    $"{totalSize.Value.ToString("0.##", CultureInfo.InvariantCulture)} {totalSize.Unit}");


                double usedPercent = 100.0 * (drive.TotalSize - drive.TotalFreeSpace) / drive.TotalSize;
                _output.WriteLine(
                    "  Percentuale utilizzata:        {0, 13:0.00} %",
                    usedPercent);

                var rootDir = new DirectoryInfo(drive.Name);
                DirectoryInfo[] rootSubDirs = rootDir.GetDirectories();

                _output.WriteLine($"\n--- Analisi cartelle in '{drive.Name}' ---\n");
                foreach (var dir in rootSubDirs)
                {
                    long dirSize = GetDirectoriesSize(dir);
                    var res = ConvertSize((long)dirSize);

                    _output.WriteLine($"--- Cartella: {dir.FullName} \n\tDimensioni Totali: {res.Value.ToString("0.##", CultureInfo.InvariantCulture)} {res.Unit}");
                }

                _output.WriteLine("\n--- Vuoi calcolare le 10 cartelle più pesanti? [Y/N] ---");

                string? answer = _output.ReadLine()?.ToUpper();
                if (string.IsNullOrEmpty(answer) || answer != "Y")
                {
                    return;
                }

                _output.WriteLine("\n--- Calcolo le cartelle più pesanti, potrebbe richiedere qualche minuto. ---");
                _output.WriteLine($"\n--- Top 10 cartelle più pesanti in '{drive.Name}' ---\n");
                
                TryProcessDirectory(drive.Name, 10);
            }
        }

        // fa un resoconto dalla root quanto pesa una cartella in byte
        // poi viene convertita in giga
        // da fare come oop per future estensioni
        long GetDirectoriesSize(DirectoryInfo root)
        {
            long total = 0;

            var queue = new Queue<DirectoryInfo>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var dir = queue.Dequeue();

                var files = Array.Empty<FileInfo>();
                var subDirs = Array.Empty<DirectoryInfo>();

                try
                {
                    files = dir.GetFiles();
                    subDirs = dir.GetDirectories();
                }
                catch (UnauthorizedAccessException ex)
                {
                    errorList.Add($"[NO ACCESS] on {dir.FullName} - {ex.Message}");
                    continue;
                }
                catch (IOException ex)
                {
                    errorList.Add($"[GENERIC ERROR] on {dir.FullName} - {ex.Message}");
                    continue;
                }

                foreach (var f in files)
                {
                    total += f.Length;
                }

                foreach (var sub in subDirs)
                {
                    queue.Enqueue(sub);
                }
            }

            return total;
        }

        // tupla di conversione byte -> (valore, unità di misura)
        // fatto come oop per future estensioni
        public static (double Value, string Unit) ConvertSize(long bytes)
        {
            const double BYTE_TO_GB = 1024d * 1024d * 1024d;
            const double BYTE_TO_MB = 1024d * 1024d;

            var gb = Math.Round(bytes / BYTE_TO_GB, 1);

            if (gb >= 1)
                return (gb, "GB");

            var mb = Math.Round(bytes / BYTE_TO_MB, 2);
            
            return (mb, "MB");
        }


        // processa una cartella
        // da fare come oop per future estensioni
        private void TryProcessDirectory(string path, int topN)
        {
            long size = 0;

            try
            {
                // file diretti
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch { /* ignora file non accessibili */ }
                }

                // sottocartelle
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    size += GetDirectorySizeRecursive(dir, topN);
                }

                UpdateTopFolders(path, size, topN);
            }
            catch
            {
                // ignora cartelle non accessibili
            }
        }

        // calcola ricorsivamente la dimensione di una cartella
        // da fare come oop per future estensioni
        private long GetDirectorySizeRecursive(string path, int topN)
        {
            long size = 0;

            try
            {
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch { }
                }

                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    size += GetDirectorySizeRecursive(dir, topN);
                }

                UpdateTopFolders(path, size, topN);
            }
            catch
            {
                // ignora
            }

            return size;
        }

        // recupera le top n cartelle più pesanti
        // da fare come oop per future estensioni
        private void UpdateTopFolders(string path, long size, int topN)
        {
            if (size <= 0) return;

            if (_topFolders.Count < topN)
            {
                _topFolders.Add(new FolderSizeInfo { Path = path, Size = size });
            }
            else
            {
                var min = _topFolders.MinBy(f => f.Size);
                if (min != null && size > min.Size)
                {
                    _topFolders.Remove(min);
                    _topFolders.Add(new FolderSizeInfo { Path = path, Size = size });
                }
            }
        }
    }
}
