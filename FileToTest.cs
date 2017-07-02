using System;
using System.IO;

namespace Sync
{
    class FileToTest
    {
        public FileToTest(EType type, String path, String majorPath)
        {
            Name = itemName(path);
            Path = filePath(path, majorPath);
            ShouldVerify = shouldVerify(type, Name);
            date = Directory.GetLastWriteTime(path);
        }

        public String Name { get; private set; }
        public String Path { get; private set; }
        public Boolean ShouldVerify { get; private set; }
        private DateTime date { get; set; }


        private static String itemName(String path)
        {
            return path.Substring(path.LastIndexOf(@"\") + 1);
        }


        private static String filePath(String path, String majorPath)
        {
            return path
                .Remove(path.LastIndexOf(@"\"))
                .Replace(majorPath, "");
        }


        private static bool shouldVerify(EType type, String name)
        {
            return type == EType.Directory
                ? name != "bin" && name != "obj" && name != ".svn" && !name.StartsWith("_ReSharper")
                : !name.EndsWith(".user") && !name.EndsWith(".suo") && !name.EndsWith(".ReSharper");
        }





        public Boolean IsNewerThan(FileToTest other)
        {
            return (date - other.date) > new TimeSpan(0, 0, 2);
        }


    }
}
