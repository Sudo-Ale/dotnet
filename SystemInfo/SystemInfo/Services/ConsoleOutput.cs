using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class ConsoleOutput : IOutput
    {
        public void WriteLine(string message) => Console.WriteLine(message);
        public void WriteLine(string format, params object[] args) => Console.WriteLine(format, args);
        public string? ReadLine() => Console.ReadLine();
    }
}
