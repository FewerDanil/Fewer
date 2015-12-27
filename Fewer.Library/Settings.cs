using System;
using System.Collections.Generic;

namespace Fewer.Library
{
    /// <summary>
    /// Global searching settings.
    /// </summary>
    public static class Settings
    {
        /// <summary>
        /// Minimal file size.
        /// </summary>
        public static long MinSize { get; set; }

        /// <summary>
        /// Maximal date.
        /// </summary>
        public static DateTime MaxDate { get; set; }

        /// <summary>
        /// List of disks to search in.
        /// </summary>
        public static List<string> Disks { get; set; }
    }
}
