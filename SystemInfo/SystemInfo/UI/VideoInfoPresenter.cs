using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Interfaces;

namespace SystemInfo.UI
{
    class VideoInfoPresenter
    {
        private readonly IOutput _output;
        private readonly IVideoAnalyzer _videoAnalyzer;

        public VideoInfoPresenter(IOutput output, IVideoAnalyzer videoAnalyzer)
        {
            _output = output;
            _videoAnalyzer = videoAnalyzer;
        }

        public void ShowInfo()
        {
            var analyzer = _videoAnalyzer.AnalyzeVideo();

            var index = 1;

            _output.WriteLine("\n=== Informazioni Dispositivi Audio ===");
            foreach (var video in analyzer)
            {
                _output.WriteLine($"--- Scheda video #{index} ---");
                _output.WriteLine($"Nome: {video.Name}");
                _output.WriteLine($"Descrizione: {video.Description}");
                _output.WriteLine($"Refresh rate attuale: {video.CurrentRefreshRate}");
                _output.WriteLine($"Refresh rate min: {video.MinRefreshRate}");
                _output.WriteLine($"Refresh rate max: {video.MaxRefreshRate}");
                _output.WriteLine($"Refresh rate orizzontale x verticale: {video.CurrentHorizontalResolution} x {video.CurrentVerticalResolution}.");
                _output.WriteLine("---------------------------------------");

                index++;
            }
        }
    }
}
