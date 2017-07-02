using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Sync.Properties;

namespace Sync
{
    class Analyzer
    {
        private readonly string handhara;
        private readonly string lucy;
        private readonly AddRow addRow;

        private String currentPathH;
        private String currentPathL;

        private enum EType
        {
            File, Directory
        }

        private enum ECompare
        {
            Less = -1,
            Equal = 0,
            Greater = 1
        }

        public delegate void AddRow(String trouble, String disk, String path, String fileName);

        public Analyzer(String pathH, String pathL, AddRow addRow)
        {
            currentPathH = putBar(pathH);
            currentPathL = putBar(pathL);

            handhara = currentPathH;
            lucy = currentPathL;

            this.addRow = addRow;
        }


        public void CompareDirectories()
        {
            var sourceControlH = getSourceControlParent(currentPathH);
            var sourceControlL = getSourceControlParent(currentPathL);

            var sourceControlFail = sourceControlH != currentPathL 
                                 && sourceControlL != currentPathH
                                 && sourceControlH != sourceControlL;

            if (sourceControlFail)
            {
                throw new Exception(
                    String.Format(
                        "One of this is under source control, the other doesn't:\n{0}\n{1}"
                        , currentPathH, currentPathL
                    )
                );
            }


            if (sourceControlH == null && sourceControlL == null)
            {
                analyzeFiles();
                analyzeDirectories();
            }

            Application.DoEvents();
        }



        private void analyzeFiles()
        {
            var directoryH = Directory.GetFiles(currentPathH);
            var directoryL = Directory.GetFiles(currentPathL);

            analyze(EType.File, directoryH, directoryL);
        }

        private void analyzeDirectories()
        {
            var directoryH = Directory.GetDirectories(currentPathH);
            var directoryL = Directory.GetDirectories(currentPathL);

            analyze(EType.Directory, directoryH, directoryL);
        }

        private void analyze(EType type, String[] directoryH, String[] directoryL)
        {
            directoryH = directoryH.OrderBy(p => p).ToArray();
            directoryL = directoryL.OrderBy(p => p).ToArray();

            var lengthH = directoryH.Length;
            var lengthL = directoryL.Length;

            var positionH = 0;
            var positionL = 0;

            while (positionH < lengthH && positionL < lengthL)
            {
                var nameH = itemName(directoryH[positionH]);
                var nameL = itemName(directoryL[positionL]);

                var pathH = filePath(directoryH[positionH]);
                var pathL = filePath(directoryL[positionL]);

                var discardH = isDisposable(type, nameH);
                var discardL = isDisposable(type, nameL);
                var discard = discardH || discardL;

                if (discardH) positionH++;

                if (discardL) positionL++;

                if (!discard)
                {
                    var compare = nameH.CompareTo(nameL);

                    switch ((ECompare)compare)
                    {
                        case ECompare.Greater:
                            addRow(Interface.CNaoExiste, Interface.CHandhara, pathL, nameL);
                            positionL++;
                            break;
                        case ECompare.Less:
                            addRow(Interface.CNaoExiste, Interface.CLucy, pathH, nameH);
                            positionH++;
                            break;
                        case ECompare.Equal:

                            switch (type)
                            {
                                case EType.File:
                                    var dataH = Directory.GetLastWriteTime(directoryH[positionH]);
                                    var dataL = Directory.GetLastWriteTime(directoryL[positionL]);

                                    if (dataH.IsReallyGreaterThan(dataL))
                                        addRow(Interface.CSobrescrever, Interface.CLucy, pathH, nameH);
                                    else if (dataL.IsReallyGreaterThan(dataH))
                                        addRow(Interface.CSobrescrever, Interface.CHandhara, pathL, nameL);
                                    break;

                                case EType.Directory:
                                    currentPathH = directoryH[positionH];
                                    currentPathL = directoryL[positionL];
                                    CompareDirectories();

                                    break;
                            }

                            positionL++;
                            positionH++;

                            break;
                    }
                }
            }

            for (var i = positionH; i < lengthH; i++)
            {
                var name = itemName(directoryH[i]);
                var path = filePath(directoryH[i]);

                var discard = isDisposable(type, name);

                if (!discard)
                    addRow(Interface.CNaoExiste, Interface.CLucy, path, name);
            }

            for (var i = positionL; i < lengthL; i++)
            {
                var name = itemName(directoryL[i]);
                var path = filePath(directoryL[i]);

                var discard = isDisposable(type, name);

                if (!discard)
                    addRow(Interface.CNaoExiste, Interface.CHandhara, path, name);
            }

        }



        private static String itemName(String path)
        {
            return path.Substring(path.LastIndexOf(@"\") + 1);
        }

        private static String filePath(String path)
        {
            return path.Remove(path.LastIndexOf(@"\"));
        }



        private static bool isDisposable(EType type, String name)
        {
            return type == EType.Directory
                ? name == "bin" || name == "obj" || name == ".svn" || name.StartsWith("_ReSharper")
                : name.EndsWith(".user") || name.EndsWith(".suo") || name.EndsWith(".ReSharper");
        }


        private static String getSourceControlParent(String directory)
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


        public void VerifyToDelete(DataGridViewRow row, DataTable table)
        {
            var screenRow = row.Cells;

            var search = new[]
                {
                    screenRow[0].Value,
                    screenRow[1].Value,
                    screenRow[2].Value,
                    screenRow[3].Value
                };

            var tableRow = table.Rows.Find(search);

            verifyToDelete(tableRow);
        }

        private void verifyToDelete(DataRow row)
        {
            String pathH, pathL, pathDestiny;
            DateTime dataH, dataL;
            
            var issue = row[Interface.ColAction].ToString();
            var pathOrigin = Path.Combine(
                    row[Interface.ColPath] + @"\",
                    row[Interface.ColFileName].ToString());
            var disk = row[Interface.ColDisk].ToString();


            switch (disk)
            {
                case Interface.CHandhara:
                    pathL = pathOrigin;
                    pathH = pathOrigin.Replace(lucy, handhara);
                    pathDestiny = pathH;
                    break;
                case Interface.CLucy:
                    pathH = pathOrigin;
                    pathL = pathOrigin.Replace(handhara, lucy);
                    pathDestiny = pathL;
                    break;
                default:
                    MessageBox.Show(Resources.Analyzer_VerifyToDelete);
                    return;
            }

            switch (issue)
            {
                case Interface.CSobrescrever:

                    dataH = File.GetLastWriteTime(pathH);
                    dataL = File.GetLastWriteTime(pathL);


                    if (dataH.IsReallyGreaterThan(dataL))
                    {
                        if (disk == Interface.CLucy)
                        {
                            Interface.IssueNotSolved(row);
                        }
                        else
                        {
                            row[Interface.ColDisk] = Interface.CLucy;
                            row[Interface.ColPath] = pathH;
                        }
                    }
                    else if (dataL.IsReallyGreaterThan(dataH))
                    {
                        if (disk == Interface.CHandhara)
                        {
                            Interface.IssueNotSolved(row);
                        }
                        else
                        {
                            row[Interface.ColDisk] = Interface.CHandhara;
                            row[Interface.ColPath] = pathL;
                        }
                    }
                    else
                    {
                        row.Delete();
                    }
                    break;

                case Interface.CNaoExiste:

                    var originExists = File.Exists(pathOrigin) || Directory.Exists(pathOrigin);
                    var destinyExists = File.Exists(pathDestiny) || Directory.Exists(pathDestiny);

                    if (destinyExists || !originExists)
                        row.Delete();
                    else
                        Interface.IssueNotSolved(row);

                    break;
            }
        }

        private static string putBar(string path)
        {
            if (path.Last() != '\\')
                path += @"\";

            return path;
        }

    }

    public static class DateTimeExtended
    {
        public static Boolean IsReallyGreaterThan(this DateTime date, DateTime other)
        {
            return (date - other) > new TimeSpan(0, 0, 2);
        }
    }
}
