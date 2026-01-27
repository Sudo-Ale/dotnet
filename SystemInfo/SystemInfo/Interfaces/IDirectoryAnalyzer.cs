using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;

namespace SystemInfo.Interfaces
{
    interface IDirectoryAnalyzer
    {
        IEnumerable<DataDirectoryInfo> AnalyzeRootDrive();
        IEnumerable<DataDirectoryInfo> AnalyzeSubdirectories(string rootPath);
        IEnumerable<FolderSizeInfo> GetTopFolders(string rootPath, int topN);
        IEnumerable<(string DriveName, IReadOnlyList<FolderSizeInfo> TopFolders)> AnalyzeTopFoldersPerDrive(int topN);
    }
}
