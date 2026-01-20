using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;

namespace SystemInfo.Interfaces
{
    interface IVideoAnalyzer
    {
        public IEnumerable<DataVideoInfo> AnalyzeVideo();
    }
}
