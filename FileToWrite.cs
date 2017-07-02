using System;
using Sync.Properties;
using P = System.IO.Path;
using D = System.IO.Directory;

namespace Sync
{
    class FileToWrite
    {
        public FileToWrite(String majorPath, String relativePath, String name)
        {
            Name = name;

            Folder = relativePath == Resources.Interface_Row_Root
                ? majorPath
                : P.Combine(majorPath, relativePath);

            Path = P.Combine(Folder, Name);
            
            Date = D.GetLastWriteTime(Path);
        }

        public String Folder { get; private set; }
        public String Name { get; private set; }
        public String Path { get; private set; }
        public DateTime Date { get; private set; }

    }
}
