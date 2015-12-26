using System;
using System.IO;

namespace Fewer.Library
{
    /// <summary>
    /// File class.
    /// </summary>
    public class File
    {
        /// <summary>
        /// File name.
        /// </summary>
        public string Name { get { return _fileInfo.Name; } }

        /// <summary>
        /// Full file path.
        /// </summary>
        public string FullName { get { return _fileInfo.FullName ; } }

        /// <summary>
        /// Last file access date.
        /// </summary>
        public DateTime LastChange { get { return _fileInfo.LastAccessTime; } }

        /// <summary>
        /// File size.
        /// </summary>
        public long Size { get { return _fileInfo.Length; } }

        /// <summary>
        /// File size formatted.
        /// </summary>
        public string SizeString { get { return string.Format("{0:0.#} mb", (float)_fileInfo.Length / 1048576.0f); } }

        /// <summary>
        /// File score.
        /// </summary>
        public float Score { get { return _score; } }

        /// <summary>
        /// File score formatted.
        /// </summary>
        public string ScoreString { get { return string.Format("{0:0.#}", _score); } }

        /// <summary>
        /// Instance of FileInfo.
        /// </summary>
        private FileInfo _fileInfo;

        /// <summary>
        /// File score.
        /// </summary>
        private float _score;

        /// <summary>
        /// File constructor.
        /// </summary>
        /// <param name="path">Full path to file.</param>
        internal File (string path)
        {
            _fileInfo = new FileInfo(path);
        }

        /// <summary>
        /// Set's file score.
        /// </summary>
        /// <param name="score">Score to set.</param>
        internal void SetScore(float score)
        {
            _score = score;
        }
        
        /// <summary>
        /// Delete's file.
        /// </summary>
        /// <returns>True if file is successfully deleted. False if exception is thrown.</returns>
        internal bool Delete()
        {
            try
            {
                _fileInfo.Delete();
            }
            catch (Exception exception)
            {
                return false;
            }

            return true;
        }
    }
}
