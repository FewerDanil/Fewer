using System;
using System.Collections.Generic;
using System.IO;

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

<<<<<<< HEAD
        public static List<File> GetFiles(Settings settings)
        {
            var files = new List<File>();

            foreach (string disk in settings.Disks)
            {
                string[] filesPaths = Directory.GetFiles(disk, "*.*", SearchOption.TopDirectoryOnly);

                foreach(string filePath in filesPaths)
                {
                    FileInfo fileInfo = new FileInfo(filePath);

                    if(fileInfo.Length >= settings.MinSize && fileInfo.LastAccessTime >= settings.MinDate)
                    {
                        files.Add(new File(filePath, fileInfo.Name, fileInfo.LastAccessTime, fileInfo.Length));
                    }
                }
            }

            return files;
        }
        /*
        public static List<File> GetFiles(Settings settings)
        {
            List<File> files = new List<File>();
=======
        public static List<File> GetFiles()
        {
            if(!Settings.IsSet) Settings.SetSettings();

            List<File> listFile = new List<File>();  // list for return
>>>>>>> origin/Dima
            long maxSize = 0;
            long minSize = 0;
            DateTime minDate = DateTime.Now;
            DateTime maxDate = DateTime.Now;

<<<<<<< HEAD
            foreach (var disk in settings.Disks) //scan every disk
=======
            foreach (var disk in Settings.Disks) //scan every disk
>>>>>>> origin/Dima
            {
                String[] allFiles = Directory.GetFiles(disk, "*.*", System.IO.SearchOption.AllDirectories); // get all file names in disk(directory)
                List<FileInfo> files = new List<FileInfo>();
                foreach (var file in allFiles)  // init FileInfo list
                {
                    FileInfo fi = new FileInfo(file);
                    files.Add(fi);
                }

                maxSize = files[0].Length;
                minSize = files[0].Length;
                maxDate = files[0].CreationTime;
                minDate = files[0].CreationTime;

                foreach (var item in files)
                {
                    if (item.Length > maxSize) maxSize = item.Length;
                    if (item.Length < minSize) minSize = item.Length;
                    if (item.CreationTime < minDate) minDate = item.CreationTime;
                    if (item.CreationTime > maxDate) maxDate = item.CreationTime;

                    if (item.Length > Settings.MinSize && item.CreationTime < Settings.MinDate)
                    {
                        File file = new File(item.FullName);
                        file.FileTime = item.CreationTime;
                        file.FileSize = item.Length;
                        listFile.Add(file);
                    }
                }
            }
            SortFiles(listFile, SortingCriteria.FileSize);
            SortFiles(listFile, SortingCriteria.FileUseDate);
            return listFile;
        }

        public static List<File> SortFiles(List<File> files, SortingCriteria sortingCriteria)
        {           
            switch (sortingCriteria)
            {                
                case SortingCriteria.FileSize:

                    files.Sort((a, b) => a.FileSize.CompareTo(b.FileSize));
                    SetFilePriority(files, SortingCriteria.FileSize);
                    return files;

                case SortingCriteria.FileUseDate:

                     files.Sort((a, b) => a.FileTime.CompareTo(b.FileTime));
                     SetFilePriority(files, SortingCriteria.FileUseDate);
                     return files;
                    
                default:                    
                        return files;
                    
            }
        }


        static void SetFilePriority(List<File> fileList, SortingCriteria sort)
        {
           
            double koef = GetKoef(fileList.Count);

            if (sort == SortingCriteria.FileUseDate)
            {
                int count = fileList.Count;
                foreach (File item in fileList)
                {
                    item.FilePriorityTime = (int)(count * koef);
                    count--;
                }
            }
            else
            {
                int count = 1;
                foreach (File item in fileList)
                {
                    item.FilePrioritySize = (int)(count * koef);
                    count++;
                }
            }
        }


        static double GetKoef(int length, int range = 100)
        {
            return range / length;
        }

        public static void DeleteFiles(List<File> files)
        {
            foreach (var item in files)
            {
                item.Delete();               
            }
        }                
<<<<<<< HEAD
        */
        //static int FileDateRating(DateTime currentFileTime, DateTime minDate, DateTime maxDate)
        //{
        //    int dif = maxDate.Minute - minDate.Minute;
        //    int ratingTime = currentFileTime.Minute * dif / 100;
        //    return ratingTime;
        //}


=======
        
>>>>>>> origin/Dima
    }
}
