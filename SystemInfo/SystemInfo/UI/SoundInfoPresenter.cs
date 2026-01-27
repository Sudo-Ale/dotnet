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
    class SoundInfoPresenter : IPresenter
    {
        private readonly IOutput _output;
        private readonly ISoundAnalyzer _soundAnalyzer;

        public SoundInfoPresenter(IOutput output, ISoundAnalyzer soundAnalyzer)
        {
            _output = output;
            _soundAnalyzer = soundAnalyzer;
        }

        public void ShowInfo()
        {
            var analyzer = _soundAnalyzer.AnalyzeSound();
            
            var index = 1;

            _output.WriteLine("\n=== Informazioni Dispositivi Audio ===");
            foreach (var sound in analyzer)
            {
                _output.WriteLine($"--- Scheda audio #{index} ---");
                _output.WriteLine($"Nome: {sound.AudioName}");
                _output.WriteLine($"Prodotto: {sound.AudioProductName}");
                _output.WriteLine($"Produttore: {sound.AudioManufacturer}");
                _output.WriteLine($"Descrizione: {sound.AudioDescription}");
                _output.WriteLine($"Caption: {sound.AudioCaption}");
                _output.WriteLine("---------------------------------------");

                index++;
            }
        }
    }
}
