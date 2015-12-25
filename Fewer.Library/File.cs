using System;
using System.IO;

namespace Fewer.Library
{
    public class File
    {
        public string Name { get; }
        public DateTime LastChange { get; }
        public long Size { get; }
        public string SizeString { get { return (Size / 1048576) + " mb"; } }
        public float Score { get { return GetScore(); } }

        private string _path;

        internal File (string path, string name, DateTime lastChange, long size)
        {
            _path = path;
            Name = name;
            LastChange = lastChange;
            Size = size;
        }
        
        internal void Delete()
        {
            FileInfo fileInfo = new FileInfo(_path);
            fileInfo.Delete();
        }

        internal float GetScore()
        {
            return 1.0f;
        }
    }
}
