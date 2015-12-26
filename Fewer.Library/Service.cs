using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Fewer.Library
{
    public static class Service
    {
        public static List<string> GetDisks()
        {
            List<string> disks = new List<string>();
            DriveInfo[] drives = DriveInfo.GetDrives();

            foreach (DriveInfo drive in drives)
            {
                if (drive.DriveType == DriveType.Fixed)
                {
                    disks.Add(drive.Name);
                }
            }

            return disks;
        }

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
            catch (UnauthorizedAccessException ex)
            {

            }
        }

        public static List<File> GetFiles()
        {
            var filesInfos = new List<FileInfo>();

            foreach (string disk in Settings.Disks)
            {
                List<string> filesPaths = new List<string>();
                AddFiles(disk + "totalcmd\\", filesPaths);

                foreach(string filePath in filesPaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    try
                    {
                        if (fileInfo.Length >= Settings.MinSize && fileInfo.LastAccessTime >= Settings.MinDate && fileInfo.IsReadOnly == false)
                        {
                            filesInfos.Add(fileInfo);
                        }
                    }
                    catch (Exception e)
                    {

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

                File file = new File(filesInfos[i].FullName, filesInfos[i].Name, filesInfos[i].LastAccessTime, filesInfos[i].Length);
                files.Add(file);
            }

            foreach(File file in files)
            {
                SetScore(file, minSize, maxSize, minDate, maxDate);
            }

            return files;
        }

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

        private static void SetScore(File file, long minSize, long maxSize, DateTime minDate, DateTime maxDate)
        {
            float score;
            
            float sizeScore = (float)file.Size / ((float)maxSize - (float)minSize);
            float dateScore = (float)file.LastChange.Ticks / (float)maxDate.Ticks;
            score = (sizeScore * 10) + (dateScore * 0);

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
    }
}
