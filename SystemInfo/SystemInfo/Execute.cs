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

        public Execute()
        {
            hardwareInfo = new();
            errorList = new();
        }

        public void Start()
        {
            // refresho le info hardware
            Console.WriteLine("Sto recuperando le info hardware, potrebbe richiedere qualche minuto...\n");
            hardwareInfo.RefreshAll();

            Console.WriteLine("----- SysInfo version 0.0.2 -----\n");

            // generic info hardware e os
            GenericSysInfo();

            // Chiedo all 'utente il disco di cui vuole le info
            Console.WriteLine("----- Scrivi il nome del disco interessato per info (esempio: 'C') -----");
            string? driveName = Console.ReadLine()?.ToUpper();

            CheckOnDrive(driveName, out string rootPath);

            var rootDir = new DirectoryInfo(rootPath);

            DirectoryInfo[] rootSubDirs = rootDir.GetDirectories();

            foreach (var dir in rootSubDirs)
            {
                long dirSize = GetDirectoriesSize(dir);

                Console.WriteLine($"--- Cartella: {dir.FullName} \n\tDimensioni Totali: {ConvertToGb((ulong)dirSize)} GB.");
            }

            //Console.WriteLine("--- Cartelle senza permessi di accesso:\n");
            //foreach (var noAccess in errorList)
            //{
            //    Console.WriteLine(noAccess);
            //}
        }



        // Hardware.Info
        // recupero info generiche sul sistema
        void GenericSysInfo()
        {
            Console.WriteLine($"Sistema operativo: \n{hardwareInfo.OperatingSystem}");
            //Console.WriteLine($"RAM Totale: {Math.Round(_hardwareInfo.MemoryStatus.TotalPhysical / standardFactorByteToGb)} GB.");
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
                    //Console.WriteLine($"[NO ACCESS] {dir.FullName} - {ex.Message}");
                    errorList.Add($"[NO ACCESS] on {dir.FullName} - {ex.Message}");
                    continue;
                }
                catch (IOException ex)
                {
                    //Console.WriteLine($"[GENERIC ERROR] {dir.FullName} - {ex.Message}");
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
        private static void CheckOnDrive(string? driveName, out string rootPath)
        {
            rootPath = string.Empty;

            if (string.IsNullOrEmpty(driveName))
            {
                Console.WriteLine("Nome del disco non può essere vuoto.");
                return;
            }

            // guardo se esiste il disco
            var driveUser = DriveInfo.GetDrives().FirstOrDefault(d => d.IsReady && d.Name.StartsWith(driveName));

            if (driveUser == null)
            {
                Console.WriteLine($"Disco '{driveName}' non trovato o non pronto.");
                return;
            }

            rootPath = driveUser.RootDirectory.FullName;
        }

        private static double ConvertToGb(ulong value)
        {
            return Math.Round(value / STANDARD_FACTOR_BYTE_TO_GB );
        }
    }
}
