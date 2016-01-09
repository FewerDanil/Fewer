using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Fewer.Library
{
    /// <summary>
    /// File interaction service.
    /// </summary>
    public static class Service
    {
        /// <summary>
        /// Returns disk's, that are ready for interactions.
        /// </summary>
        /// <returns>List of path's to disk's</returns>
        public static List<string> GetDisks()
        {
            List<string> disks = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.DriveType == DriveType.Fixed && drive.IsReady)
                {
                    disks.Add(drive.Name);
                }
            }

            return disks;
        }

        /// <summary>
        /// Recursively scans directories for files and adds them into the given list.
        /// </summary>
        /// <param name="path">Path to search in.</param>
        /// <param name="files">List of files to add in.</param>
        private static void AddFiles(string path, List<string> files)
        {
            try
            {
                Directory.GetFiles(path)
                    .ToList()
                    .ForEach(s => files.Add(s));

                Directory.GetDirectories(path)
                    .ToList()
                    .ForEach(s => AddFiles(s, files));
            }
            catch (Exception exception)
            {
                //Not enough rights.
            }
        }

        /// <summary>
        /// Searches for files using given settings.
        /// </summary>
        /// <returns>List of found files.</returns>
        public static List<File> GetFiles()
        {
            var filesInfos = new List<FileInfo>();

            foreach (string disk in Settings.Disks)
            {
                List<string> filesPaths = new List<string>();
                AddFiles(disk, filesPaths);

                foreach(string filePath in filesPaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    try
                    {
                        if (fileInfo.Length >= Settings.MinSize && fileInfo.LastAccessTime <= Settings.MaxDate.AddDays(1) && fileInfo.IsReadOnly == false)
                        {
                            filesInfos.Add(fileInfo);
                        }
                    }
                    catch (Exception exception)
                    {
                        //File not found.
                    }
                }
            }

            var files = new List<File>();
            long minSize = 0, maxSize = 0;
            DateTime minDate = DateTime.Now, maxDate = DateTime.Now;

            for (int i = 0; i < filesInfos.Count; i++)
            {
                if(i == 0)
                {
                    minSize = filesInfos[i].Length;
                    maxSize = filesInfos[i].Length;
                    minDate = filesInfos[i].LastAccessTime;
                    maxDate = filesInfos[i].LastAccessTime;
                }
                else
                {
                    if(filesInfos[i].Length < minSize)
                    {
                        minSize = filesInfos[i].Length;
                    }
                    else if(filesInfos[i].Length > maxSize)
                    {
                        maxSize = filesInfos[i].Length;
                    }

                    if (filesInfos[i].LastAccessTime < minDate)
                    {
                        minDate = filesInfos[i].LastAccessTime;
                    }
                    else if (filesInfos[i].LastAccessTime > maxDate)
                    {
                        minDate = filesInfos[i].LastAccessTime;
                    }
                }

                File file = new File(filesInfos[i].FullName);
                files.Add(file);
            }

            foreach(File file in files)
            {
                SetScore(file, minSize, maxSize, minDate, maxDate);
            }

            return files;
        }

        /// <summary>
        /// Deletes given list of files.
        /// </summary>
        /// <param name="files">List of files to delete.</param>
        /// <returns>List of bools. Each value represents failure or success of file delete.</returns>
        public static List<bool> DeleteFiles(List<File> files)
        {
            List<bool> results = new List<bool>();

            foreach(File file in files)
            {
                var result = file.Delete();

                results.Add(result);
            }

            return results;
        }

        /// <summary>
        /// Sets score for given file using given parameters.
        /// </summary>
        /// <param name="file">File to set score in.</param>
        /// <param name="minSize">Minimal size of file in list.</param>
        /// <param name="maxSize">Maximal size of file in list.</param>
        /// <param name="minDate">Minimal file access date in list.</param>
        /// <param name="maxDate">Maximal file access date in list.</param>
        private static void SetScore(File file, long minSize, long maxSize, DateTime minDate, DateTime maxDate)
        {
            float score;
            
            float sizeScore = (float)file.Size / ((float)maxSize - (float)minSize);
            float dateScore = (float)file.LastChange.Ticks / (float)maxDate.Ticks;
            score = (sizeScore * 10);

            if (score > 10.0f)
            {
                score = 10.0f;
            }
            else if(float.IsNaN(score))
            {
                score = 0;
            }

            file.SetScore(score);
        }

        public static List<File> SortFiles(List<File> files, SortingCriteria sortingCriteria)
        {
            switch (sortingCriteria)
            {
                case SortingCriteria.FileName:
                    files.Sort((a, b) => a.Name.CompareTo(b.Name));
                    return files;

                case SortingCriteria.FilePath:
                    files.Sort((a, b) => a.FullName.CompareTo(b.FullName));
                    return files;
                
                case SortingCriteria.FileSize:

                    files.Sort((a, b) => a.Size.CompareTo(b.Size));
                    return files;

                case SortingCriteria.FileUseDate:
                    files.Sort((a, b) => a.LastChange.CompareTo(b.LastChange));
                    return files;

                case SortingCriteria.FileScore:
                    files.Sort((a, b) => a.Score.CompareTo(b.Score));
                    return files;

                default: return files;
            }
        }
    }
}
