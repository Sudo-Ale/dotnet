using Hardware.Info;
using System.Diagnostics;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;

namespace SystemInfo
{
    class Program
    {
        private const string THUMBPRINT = "21acce504112a72915c67fac9d2b9ad2bfe5e609";
        
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Init program..");

                // Controllo se il certificato è già installato
                // Se non lo è, chiedo all utente di installarlo
                if (!IsCertInstalled(THUMBPRINT))
                {
                    Console.WriteLine("Certificato SysInfoCodeSigning per SysInfo non installato.");
                    Console.WriteLine("Serve installarlo per fidarsi di questa app su questo PC.");

                    Console.Write("Vuoi aprire il wizard di installazione adesso? [Y/N]: ");

                    var key = Console.ReadKey();
                    Console.WriteLine();

                    if (key.Key == ConsoleKey.Y)
                    {
                        LaunchPfxInstaller();
                        Console.WriteLine("Si è aperto il wizard del certificato.");
                        Console.WriteLine("Dopo aver completato il wizard, rilancia il programma.");
                        Console.WriteLine("Premi un tasto per uscire...");
                        Console.ReadKey();
                        return; // esco, l'utente poi riapre l'app
                    }
                    else
                    {
                        Console.WriteLine("Procedo senza certificato installato (potrebbero esserci avvisi di sicurezza).");
                    }
                }

                var exe = new Execute();
                exe.Start();
            }

            catch (Exception ex)
            {
                // mostro il messaggio
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Closing program..");
            }

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        static bool IsCertInstalled(string thumbprint)
        {
            using var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);

            var certs = store.Certificates.Find(
                X509FindType.FindByThumbprint,
                thumbprint,
                validOnly: false);

            return certs.Count > 0;
        }

        static void LaunchPfxInstaller()
        {
            // Presuppone che il file PFX sia nella stessa cartella dell'exe
            var exeDir = AppContext.BaseDirectory;
            var pfxPath = Path.Combine(exeDir, "sysinfocodesign.pfx");

            if (!File.Exists(pfxPath))
            {
                Console.WriteLine($"ATTENZIONE: file PFX non trovato: {pfxPath}");
                return;
            }

            // Apre il wizard standard di Windows per importare il PFX
            Process.Start(new ProcessStartInfo
            {
                FileName = pfxPath,
                UseShellExecute = true
            });
        }
    }
}
