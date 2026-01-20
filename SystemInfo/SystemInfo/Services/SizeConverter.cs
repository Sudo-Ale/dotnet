using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class SizeConverter : ISizeConverter
    {
        public (double Value, string Unit) Convert(long bytes)
        {
            const double BYTE_TO_GB = 1024d * 1024d * 1024d;
            const double BYTE_TO_MB = 1024d * 1024d;
            const double BYTE_TO_KB = 1024d;

            var gb = Math.Round(bytes / BYTE_TO_GB, 1);

            if (gb >= 1)
                return (gb, "GB");

            var mb = Math.Round(bytes / BYTE_TO_MB, 2);
            if (mb >= 1)
                return (mb, "MB");

            var kb = Math.Round(bytes / BYTE_TO_KB, 2);
            return (kb, "KB");
        }
    }
}
