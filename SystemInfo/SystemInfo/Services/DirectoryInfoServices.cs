using Hardware.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemInfo.Data;
using SystemInfo.Interfaces;

namespace SystemInfo.Services
{
    class DirectoryInfoServices : IDirectoryAnalyzer
    {
        private readonly List<FolderSizeInfo> _topFolders;
        private readonly List<string> _errorList;
        private readonly ISizeConverter _converter;

        public DirectoryInfoServices(ISizeConverter converter)
        {
            _converter = converter;
            _topFolders = [];
            _errorList = [];
        }

        public IEnumerable<DataDirectoryInfo> AnalyzeRootDrive()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();

            foreach (var drive in allDrives)
            {
                if(!drive.IsReady)
                    continue;
                
                var rootDir = new DirectoryInfo(drive.Name);
                DirectoryInfo[] rootSubDirs = rootDir.GetDirectories();

                foreach (var dir in rootSubDirs)
                {
                    long dirSize = GetDirectoriesSize(dir);

                    yield return new DataDirectoryInfo
                    {
                        PathName = dir.FullName,
                        Name = dir.Name,
                        TotalSize = dirSize
                    };
                }
            }
        }
        public IEnumerable<DataDirectoryInfo> GetRootSubDirectoryInfo(DataDirectoryInfo rootDir)
        {
            var dirInfo = new DirectoryInfo(rootDir.PathName);
            DirectoryInfo[] subDirectoryInfo = dirInfo.GetDirectories();

            foreach (var dir in subDirectoryInfo) 
            {
                long dirSize = GetDirectoriesSize(dir);

                yield return new DataDirectoryInfo
                {
                    PathName = dir.FullName,
                    Name = dir.Name,
                    TotalSize = dirSize
                };
            }
        }
        public IEnumerable<DataDirectoryInfo> AnalyzeSubdirectories(string rootPath)
        {
            return GetRootSubDirectoryInfo(new DataDirectoryInfo { PathName = rootPath });
        }
        long GetDirectoriesSize(DirectoryInfo root)
        {
            long total = 0;

            var queue = new Queue<DirectoryInfo>();
            queue.Enqueue(root);

            while (queue.Count > 0)
            {
                var dir = queue.Dequeue();

                var files = Array.Empty<FileInfo>();
                var subDirs = Array.Empty<DirectoryInfo>();

                try
                {
                    files = dir.GetFiles();
                    subDirs = dir.GetDirectories();
                }
                catch (UnauthorizedAccessException ex)
                {
                    _errorList.Add($"[NO ACCESS] on {dir.FullName} - {ex.Message}");
                    continue;
                }
                catch (IOException ex)
                {
                    _errorList.Add($"[GENERIC ERROR] on {dir.FullName} - {ex.Message}");
                    continue;
                }

                foreach (var f in files)
                {
                    total += f.Length;
                }

                foreach (var sub in subDirs)
                {
                    queue.Enqueue(sub);
                }
            }

            return total;
        }
        private void TryProcessDirectory(string path, int topN)
        {
            long size = 0;

            try
            {
                // file diretti
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch { /* ignora file non accessibili */ }
                }

                // sottocartelle
                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    size += GetDirectorySizeRecursive(dir, topN);
                }

                UpdateTopFolders(path, size, topN);
            }
            catch
            {
                // ignora cartelle non accessibili
            }
        }
        private long GetDirectorySizeRecursive(string path, int topN)
        {
            long size = 0;

            try
            {
                foreach (var file in Directory.EnumerateFiles(path))
                {
                    try
                    {
                        var info = new FileInfo(file);
                        size += info.Length;
                    }
                    catch { }
                }

                foreach (var dir in Directory.EnumerateDirectories(path))
                {
                    size += GetDirectorySizeRecursive(dir, topN);
                }

                UpdateTopFolders(path, size, topN);
            }
            catch
            {
                // ignora
            }

            return size;
        }
        private void UpdateTopFolders(string path, long size, int topN)
        {
            if (size <= 0) return;

            if (_topFolders.Count < topN)
            {
                _topFolders.Add(new FolderSizeInfo { Path = path, Size = size });
            }
            else
            {
                var min = _topFolders.MinBy(f => f.Size);
                if (min != null && size > min.Size)
                {
                    _topFolders.Remove(min);
                    _topFolders.Add(new FolderSizeInfo { Path = path, Size = size });
                }
            }
        }
        public IEnumerable<FolderSizeInfo> GetTopFolders(string rootPath, int topN)
        {
            _topFolders.Clear();
            TryProcessDirectory(rootPath, topN);

            return _topFolders.OrderByDescending(f => f.Size).ToList().AsReadOnly();
        }

        public IEnumerable<(string DriveName, IReadOnlyList<FolderSizeInfo> TopFolders)> AnalyzeTopFoldersPerDrive(int topN)
        {
            var result = new List<(string DriveName, IReadOnlyList<FolderSizeInfo> TopFolders)>();

            DriveInfo[] allDrives = DriveInfo.GetDrives(); // qui, non nel presenter[web:22]

            foreach (var drive in allDrives)
            {
                if (!drive.IsReady)
                    continue;

                var top = GetTopFolders(drive.Name, topN);
                var topReadOnly = top.ToList().AsReadOnly();
                result.Add((drive.Name, topReadOnly));
            }

            return result;
        }
    }
}
