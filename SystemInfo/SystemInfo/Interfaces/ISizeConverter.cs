using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Interfaces
{
    interface ISizeConverter
    {
        (double Value, string Unit) Convert(long bytes);
    }
}
