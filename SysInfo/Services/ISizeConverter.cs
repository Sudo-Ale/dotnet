using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfo.Services
{
    interface ISizeConverter
    {
        (double Value, string Unit) Convert(long bytes);
    }
}
