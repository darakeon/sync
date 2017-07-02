using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Sync
{
    class Versioning
    {
        public Versioning(String path1, String path2)
        {
            var mainSourceControl = getParent(path1);
            var compareSourceControl = getParent(path2);

            var sourceControlFail = mainSourceControl != path2
                                 && compareSourceControl != path1
                                 && mainSourceControl != compareSourceControl;

            if (sourceControlFail)
                CurrentStatus = Status.Fail;
            else if (mainSourceControl == null && compareSourceControl == null)
                CurrentStatus = Status.None;
        }

        internal enum Status
        {
            Normal,
            Fail,
            None,
        }

        public readonly Status CurrentStatus = Status.Normal;


        private static String getParent(String directory)
        {
            var svn = getSVNParent(directory);
            var hg = getHGParent(directory);

            if (hg == null)
                return svn;

            return hg;
        }

        private static String getSVNParent(String directory)
        {
            var entriesData = getController(directory, "svn", "entries");

            if (entriesData == null)
                return null;

            entriesData.ReadLine();
            entriesData.ReadLine();
            entriesData.ReadLine();
            entriesData.ReadLine();

            var result = entriesData.ReadLine();

            entriesData.Close();

            return result;
        }

        private static String getHGParent(String directory)
        {
            var hgrcData = getController(directory, "hg", "hgrc");

            if (hgrcData == null)
                return null;

            while (!hgrcData.EndOfStream)
            {
                var line = hgrcData.ReadLine();

                if (line == null || !line.StartsWith("[paths]")) continue;


                line = hgrcData.ReadLine();

                hgrcData.Close();

                return line != null
                    ? line.Replace("default = ", "")
                    : null;
            }

            hgrcData.Close();

            return null;
        }


        private static StreamReader getController(string directory, string controller, string fileController)
        {
            var hgControl = Directory.GetDirectories(directory, "." + controller);

            if (hgControl.Length == 0)
                return null;

            var hgrc = Path.Combine(hgControl[0], fileController);

            if (!File.Exists(hgrc))
                return null;

            return new StreamReader(hgrc, Encoding.GetEncoding(1252));
        }


    
    }
}
