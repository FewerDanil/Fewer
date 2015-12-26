using System;
using System.Collections.Generic;

namespace Fewer.Library
{
    public static class Settings
    {
<<<<<<< HEAD
        public long MinSize { get; set; }
        public DateTime MinDate { get; set; }
        public List<string> Disks { get; set; }

        public Settings(long minSize, DateTime minDate, List<string> disks)
        {
            MinSize = minSize;
            MinDate = minDate;
            Disks = disks;
=======
        static long _minSize;
        static public long MinSize { get { return _minSize; } set { _minSize = value; } }
        static DateTime? _minDate;
        static public DateTime? MinDate { get { return _minDate; } set { _minDate = value; } }
        static List<string> _disk;
        static public List<string> Disks { get { return _disk; } set { _disk = value; } }
        static SortingCriteria _sortTipe;
        static public SortingCriteria SortTipe { get { return _sortTipe; } set { _sortTipe = value; } }
        static bool _isSet = false;
        static public bool IsSet { get { return _isSet; } set { _isSet = value; } }

        public static void SetSettings(List<string> disks = null, long minSize = 1000, DateTime? minDate = null, SortingCriteria sortTipe = SortingCriteria.FileSize)
        {
            if (disks == null)
            {
                disks = new List<string>() { @"test" };
            }
            if (minDate == null)
            {
                minDate = DateTime.Now;
            }
            _minSize = minSize;
            _minDate = minDate;
            _disk = disks;
            _isSet = true;
>>>>>>> origin/Dima
        }
    }
}
