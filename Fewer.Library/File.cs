using System;
using System.IO;

namespace Fewer.Library
{
    public class File : ICloneable

    {
        string _fileName;
        public string FileName { get { return _fileName; } set { _fileName = value; } }
        int _filePrioritySize;
        public int FilePrioritySize { get { return _filePrioritySize; } set { _filePrioritySize = value; } }
        int _filePriorityTime;
        public int FilePriorityTime { get { return _filePriorityTime; } set { _filePriorityTime = value; } }
        DateTime _fileTime;
        public DateTime FileTime { get { return _fileTime; } set { _fileTime = value; } }
        long _fileSize;
        public long FileSize { get { return _fileSize; } set { _fileSize = value; } }

        internal File(string fileName)
        {
            _fileName = fileName;
        }
        
        internal void Delete()
        {
            try
            {
                FileInfo fi = new FileInfo(_fileName);
                fi.Delete();
            }
            catch (Exception)
            {                
                throw;
            }
           
        }

        public object Clone()
        {
            return (object)this;
        }
    }
}
