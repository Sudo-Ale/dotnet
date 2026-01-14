using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel.Design.Serialization;

namespace SystemInfo
{
    class Execute
    {
        const double STANDARD_FACTOR_BYTE_TO_GB = 1073741824; // valore fisso per convertire da byte a giga

        private readonly HardwareInfo hardwareInfo;
        private readonly List<string> errorList;

        private readonly List<FolderSizeInfo> _topFolders;

        public Execute()
        {
            hardwareInfo = new();
            errorList = new();
            _topFolders = new();
        }

        public void Start()
        {
            // refresho le info hardware
            Console.WriteLine("Sto recuperando le info hardware, potrebbe richiedere qualche minuto...\n");
            hardwareInfo.RefreshAll();

            Console.WriteLine("----- SysInfo version 1.1.3 -----\n");

            // generic info hardware e os
            GenericSysInfo();

            // info sui drive e cartelle
            RetrieveInformationsOnDrives();

            // top n cartelle più pesanti
            foreach (var folder in _topFolders.OrderByDescending(f => f.Size))
            {
                Console.WriteLine($"{folder.Path} {ConvertToGb((ulong)folder.Size)} GB");
            }
        }

        // Hardware.Info
        // recupero info generiche sul sistema
        private void GenericSysInfo()
        {
            Console.WriteLine($"Sistema operativo: \n{hardwareInfo.OperatingSystem}");
            Console.WriteLine($"RAM Totale: {ConvertToGb(hardwareInfo.MemoryStatus.TotalPhysical)} GB.");
            Console.WriteLine($"RAM Disponibile: {ConvertToGb(hardwareInfo.MemoryStatus.AvailablePhysical)} GB.\n");

            foreach (var hardware in hardwareInfo.ComputerSystemList)
            {
                Console.WriteLine($"Nome Dispositivo: {hardware.Name}.");
                Console.WriteLine($"Vendor: {hardware.Vendor}.");
            }

            foreach (var hardware in hardwareInfo.CpuList)
            {
                Console.WriteLine($"Nome Processore: {hardware.Name}.");
                Console.WriteLine($"Numero dei Core: {hardware.NumberOfCores}.");
                Console.WriteLine($"Numero dei processori logici: {hardware.NumberOfLogicalProcessors}.\n");
            }

            int indexAudio = 0;
            foreach (var hardware in hardwareInfo.SoundDeviceList)
            {
                Console.WriteLine($"--- Scheda audio #{indexAudio} ---");
                Console.WriteLine(hardware);
                indexAudio++;
            }

            int indexVideo = 0;
            foreach (var hardware in hardwareInfo.VideoControllerList)
            {
                Console.WriteLine($"--- Scheda video #{indexVideo} ---");
                Console.WriteLine($"Name: {hardware.Name}.");
                Console.WriteLine($"Desrizione: {hardware.Description}.");
                Console.WriteLine($"Refresh rate attuale: {hardware.CurrentRefreshRate}.");
                Console.WriteLine($"Refresh rate min: {hardware.MinRefreshRate}.");
                Console.WriteLine($"Refresh rate max: {hardware.MaxRefreshRate}.");
                Console.WriteLine($"Refresh rate orizzontale x verticale: {hardware.CurrentHorizontalResolution} x {hardware.CurrentVerticalResolution}.\n");

                indexVideo++;
            }
        }
        private void RetrieveInformationsOnDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var drive in allDrives)
            {
                Console.WriteLine("Drive {0}", drive.Name);
                Console.WriteLine("  Drive type: {0}", drive.DriveType);

                if (!drive.IsReady)
                {
                    Console.WriteLine("Driver non pronto");
                    continue;
                }

                Console.WriteLine("  Volume label: {0}", drive.VolumeLabel);
                Console.WriteLine("  File system: {0}", drive.DriveFormat);
                Console.WriteLine(
                    "  Spazio libero per l'utente:    {0, 15} GB",
                    ConvertToGb((ulong)drive.AvailableFreeSpace));
                Console.WriteLine(
                    "  Spazio totale libero:          {0, 15} GB",
                    ConvertToGb((ulong)drive.TotalFreeSpace));
                Console.WriteLine(
                    "  Spazio totale:                 {0, 15} GB ",
                    ConvertToGb((ulong)drive.TotalSize));

                double usedPercent = 100.0 * (drive.TotalSize - drive.TotalFreeSpace) / drive.TotalSize;
                Console.WriteLine(
                    "  Percentuale utilizzata:                    {0, 2:0.00} %",
                    usedPercent);

                var rootDir = new DirectoryInfo(drive.Name);
                DirectoryInfo[] rootSubDirs = rootDir.GetDirectories();

                Console.WriteLine($"\n--- Analisi cartelle in '{drive.Name}' ---\n");
                foreach (var dir in rootSubDirs)
                {
                    long dirSize = GetDirectoriesSize(dir);
                    Console.WriteLine($"--- Cartella: {dir.FullName} \n\tDimensioni Totali: {ConvertToGb((ulong)dirSize)} GB.");
                }

                Console.WriteLine("\n--- Vuoi calcolare le 10 cartelle più pesanti? [Y/N] ---");

                string? answer = Console.ReadLine()?.ToUpper();

                if (answer == string.Empty || answer != "Y")
                {
                    return;
                }

                Console.WriteLine("\n--- Calcolo le cartelle più pesanti, potrebbe richiedere qualche minuto. ---");
                Console.WriteLine($"\n--- Top 10 cartelle più pesanti in '{drive.Name}' ---\n");
                TryProcessDirectory(drive.Name, 10);
            }
        }

        // fa un resoconto dalla root quanto pesa una cartella in byte
        // poi viene convertita in giga
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

        private static double ConvertToGb(ulong value)
        {
            return Math.Round(value / STANDARD_FACTOR_BYTE_TO_GB );
        }


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
