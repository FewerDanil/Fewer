using System;
using System.Collections.Generic;
using System.IO;

namespace Fewer.Library
{
    public static class Service
    {
        public static List<string> GetDisks()
        {
            List<string> listDiscs = new List<string>();
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (var item in allDrives)
            {
                listDiscs.Add(item.Name);
            }
            return listDiscs;
        }

        public static List<File> GetFiles(Settings settings = null)
        {
            List<File> listFile = new List<File>();  // list for return
            long maxSize = 0;
            long minSize = 0;
            DateTime minDate = DateTime.Now;
            DateTime maxDate = DateTime.Now;

            if (settings == null)  // init settings
            {
                settings = new Settings();
                settings.MinSize = 1;
                settings.MinDate = DateTime.Now/*.AddDays(-10)*/;
                //settings.Disks = GetDisks();
                settings.Disks = new List<string>() { @"test" }; // for test only!                
            }                   

            foreach (var disk in settings.Disks) //scan every disk
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

                    if (item.Length > settings.MinSize && item.CreationTime < settings.MinDate)
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
        
        //static int FileDateRating(DateTime currentFileTime, DateTime minDate, DateTime maxDate)
        //{
        //    int dif = maxDate.Minute - minDate.Minute;
        //    int ratingTime = currentFileTime.Minute * dif / 100;
        //    return ratingTime;
        //}


    }
}
