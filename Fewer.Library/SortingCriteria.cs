using System;

namespace Fewer.Library
{
    /// <summary>
    /// File sorting criteria.
    /// </summary>
    public enum SortingCriteria
    {
        /// <summary>
        /// Sort by file size.
        /// </summary>
        FileSize,

        /// <summary>
        /// Sort file by last date of use.
        /// </summary>
        FileUseDate,

        /// <summary>
        /// Sort file by score.
        /// </summary>
        FileScore
    };
}
