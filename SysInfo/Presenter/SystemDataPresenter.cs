using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SysInfo.Services;

namespace SysInfo.Presenter
{
    class SystemDataPresenter
    {
        private readonly Label _label;
        private readonly ISizeConverter _sizeConverter;
        private readonly ISystemDataServices _systemAnalyzer;

        public SystemDataPresenter(Label label, ISizeConverter sizeConverter, ISystemDataServices systemAnalyzer)
        {
            _label = label;
            _sizeConverter = sizeConverter;
            _systemAnalyzer = systemAnalyzer;
        }

        public void ShowInfo()
        {
            var system = _systemAnalyzer.Analyze();

            _label.Text = $"Sistema operativo: {system.NameOperatingSystem} {system.Version}\n" +
                          $"RAM Totale: {_sizeConverter.Convert((long)system.TotalRam).Value:0.##} {_sizeConverter.Convert((long)system.TotalRam).Unit}\n" +
                          $"RAM Disponibile: {_sizeConverter.Convert((long)system.AvailableRam).Value:0.##} {_sizeConverter.Convert((long)system.AvailableRam).Unit}\n" +
                          $"Nome Dispositivo: {system.NameDevice}\n" +
                          $"Vendor: {system.VendorDevice}\n" +
                          $"Nome processore: {system.NameCPU}\n" +
                          $"Numero dei core: {system.CoresNumber}\n" +
                          $"Numero dei processori logici: {system.LogicalNumberProcessor}";
            //// os
            //_output.WriteLine($"Sistema operativo: {system.NameOperatingSystem}");
            //_output.WriteLine($"Versione: {system.Version}");

            //// system ram
            //var total = _sizeConverter.Convert((long)system.TotalRam);
            //_output.WriteLine($"RAM Totale: {total.Value:0.##} {total.Unit}");

            //var available = _sizeConverter.Convert((long)system.AvailableRam);
            //_output.WriteLine($"RAM Disponibile: {available.Value:0.##} {available.Unit}");

            //// system device
            //_output.WriteLine($"Nome Dispositivo: {system.NameDevice}");
            //_output.WriteLine($"Vendor: {system.VendorDevice}");
            //_output.WriteLine($"Nome processore: {system.NameCPU}");
            //_output.WriteLine($"Numero dei core: {system.CoresNumber}");
            //_output.WriteLine($"Numero dei processori logici: {system.LogicalNumberProcessor}");
            //_output.WriteLine("---------------------------------------");
        }
    }
}
