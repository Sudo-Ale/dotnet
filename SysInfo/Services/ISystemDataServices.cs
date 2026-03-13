using SysInfo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysInfo.Services
{
    internal interface ISystemDataServices
    {
        public SystemData Analyze();
    }
}
