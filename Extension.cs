using System;

namespace Sync
{
    static class Extension
    {
        public static String CorrectIfDisk(this String path)
        {
            return path.EndsWith(":")
                ? path + @"\"
                : path;
        }


    }
}
