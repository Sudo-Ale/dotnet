using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.UI
{
    class SystemInfoPresenter
    {
        private readonly IOutput _output;
        private readonly ISizeConverter _sizeConverter;
        private readonly ISystemAnalyzer _systemAnalyzer;

        public SystemInfoPresenter(IOutput output, ISizeConverter sizeConverter, ISystemAnalyzer systemAnalyzer)
        {
            _output = output;
            _sizeConverter = sizeConverter;
            _systemAnalyzer = systemAnalyzer;
        }

        public void ShowInfo()
        {
            var system = _systemAnalyzer.Analyze();
            // os
            _output.WriteLine($"Sistema operativo: {system.NameOperatingSystem}");
            _output.WriteLine($"Versione: {system.Version}");

            // system ram
            var total = _sizeConverter.Convert((long)system.TotalRam);
            _output.WriteLine($"RAM Totale: {total.Value:0.##} {total.Unit}");

            var available = _sizeConverter.Convert((long)system.AvailableRam);
            _output.WriteLine($"RAM Disponibile: {available.Value:0.##} {available.Unit}");

            // system device
            _output.WriteLine($"Nome Dispositivo: {system.NameDevice}");
            _output.WriteLine($"Vendor: {system.VendorDevice}");
            _output.WriteLine($"Nome processore: {system.NameCPU}");
            _output.WriteLine($"Numero dei core: {system.CoresNumber}");
            _output.WriteLine($"Numero dei processori logici: {system.LogicalNumberProcessor}");
            _output.WriteLine("---------------------------------------");
        }
    }
}
