using System;
using System.Collections.Generic;

namespace Fewer.Library
{
    public class Settings
    {
        public long MinSize { get; set; }
        public DateTime MinDate { get; set; }
        public List<string> Disks { get; set; }

        public Settings(long minSize, DateTime minDate, List<string> disks)
        {
            MinSize = minSize;
            MinDate = minDate;
            Disks = disks;
        }
    }
}
