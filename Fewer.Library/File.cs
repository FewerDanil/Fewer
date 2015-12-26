using System;
using System.IO;

namespace Fewer.Library
{
    public class File
    {
        public string Name { get { return _name; } }
        public string FullName { get { return _path; } }
        public DateTime LastChange { get { return _lastChange; } }
        public long Size { get { return _size; } }
        public string SizeString { get { return string.Format("{0:0.#} mb", (float)Size / 1048576.0f); } }
        public float Score { get { return _score; } }
        public string ScoreString { get { return string.Format("{0:0.#}", _score); } }

        private string _name;
        private string _path;
        private DateTime _lastChange;
        private long _size;
        private float _score;

        internal File (string path, string name, DateTime lastChange, long size)
        {
            _path = path;
            _name = name;
            _lastChange = lastChange;
            _size = size;
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
