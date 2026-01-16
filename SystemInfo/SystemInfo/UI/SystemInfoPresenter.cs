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

        public SystemInfoPresenter(IOutput output, ISizeConverter sizeConverter)
        {
            _output = output;
            _sizeConverter = sizeConverter;
        }

        public void Show(DataSystemInfo summary)
        {
            // os
            _output.WriteLine($"Sistema operativo: {summary.NameOperatingSystem}");
            _output.WriteLine($"Versione: {summary.Version}");

            // system ram
            var total = _sizeConverter.Convert((long)summary.TotalRam);
            _output.WriteLine($"RAM Totale: {total.Value:0.##} {total.Unit}");

            var available = _sizeConverter.Convert((long)summary.AvailableRam);
            _output.WriteLine($"RAM Disponibile: {available.Value:0.##} {available.Unit}");

            // system device
            _output.WriteLine($"Nome Dispositivo: {summary.NameDevice}");
            _output.WriteLine($"Vendor: {summary.VendorDevice}");
            _output.WriteLine($"Nome processore: {summary.NameCPU}");
            _output.WriteLine($"Numero dei core: {summary.CoresNumber}");
            _output.WriteLine($"Numero dei processori logici: {summary.LogicalNumberProcessor}");
        }
    }
}
