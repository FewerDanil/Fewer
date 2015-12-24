using System;
using System.Collections.Generic;

namespace Fewer.Library
{
    public class Settings
    {
        long _minSize;
        public long MinSize { get { return _minSize; } set{ _minSize = value; } }
        DateTime _minDate;
        public DateTime MinDate { get { return _minDate; } set { _minDate = value; } }
        List<string> _disk;
        public List<string> Disks { get { return _disk; } set { _disk = value; } }

        public void SetSettings(long minSize, DateTime minDate, List<string> disks)
        {
            _minSize = minSize;
            _minDate = minDate;
            _disk = disks;
        }
      
    }
}
