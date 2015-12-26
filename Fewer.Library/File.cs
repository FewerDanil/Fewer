using System;
using System.IO;

namespace Fewer.Library
{
    public class File
    {
        public string Name { get; }
        public string FullName { get { return _path; } }
        public DateTime LastChange { get; }
        public long Size { get; }
        public string SizeString { get { return string.Format("{0:0.#} mb", (float)Size / 1048576.0f); } }
        public float Score { get { return _score; } }
        public string ScoreString { get { return string.Format("{0:0.#}", _score); } }

        private float _score;
        private string _path;

        internal File (string path, string name, DateTime lastChange, long size)
        {
            _path = path;
            Name = name;
            LastChange = lastChange;
            Size = size;
        }
        
        internal bool Delete()
        {
            try
            {
                FileInfo fileInfo = new FileInfo(_path);
                fileInfo.Delete();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        internal void SetScore(float score)
        {
            _score = score;
        }
    }
}
