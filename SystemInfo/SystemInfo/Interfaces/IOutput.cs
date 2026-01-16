using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemInfo.Interfaces
{
    interface IOutput
    {
        void WriteLine(string message);
        void WriteLine(string format, params object[] args);
        string? ReadLine();
    }
}
