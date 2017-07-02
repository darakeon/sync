using System;
using P = System.IO.Path;
using System.IO;

namespace Sync
{
    class FileToWrite
    {
        public FileToWrite(String majorPath, String relativePath, String file)
        {
            Path = P.Combine(majorPath, relativePath);
            
            var filePath = P.Combine(Path, file);
            
            Date = Directory.GetLastWriteTime(filePath);
        }

        public String Path { get; private set; }
        public DateTime Date { get; private set; }

    }
}
