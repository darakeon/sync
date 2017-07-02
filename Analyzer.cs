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
        private readonly String mainPath = "";
        private readonly String comparePath = "";
        private readonly AddRow addRow;

        private String currentMainPath;
        private String currentComparePath;

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

        public Analyzer(String mainPath, String comparePath, AddRow addRow)
        {
            currentMainPath = putBar(mainPath);
            currentComparePath = putBar(comparePath);

            this.mainPath = currentMainPath;
            this.comparePath = currentComparePath;

            this.addRow = addRow;
        }


        public void CompareDirectories()
        {
            var mainSourceControl = getSourceControlParent(currentMainPath);
            var compareSourceControl = getSourceControlParent(currentComparePath);

            var sourceControlFail = mainSourceControl != currentComparePath 
                                 && compareSourceControl != currentMainPath
                                 && mainSourceControl != compareSourceControl;

            if (sourceControlFail)
            {
                throw new Exception(
                    String.Format(
                        "One of this is under source control, the other doesn't:\n{0}\n{1}"
                        , currentMainPath, currentComparePath
                    )
                );
            }


            if (mainSourceControl == null && compareSourceControl == null)
            {
                analyzeFiles();
                analyzeDirectories();
            }

            Application.DoEvents();
        }



        private void analyzeFiles()
        {
            var mainDirs = Directory.GetFiles(currentMainPath);
            var compareDirs = Directory.GetFiles(currentComparePath);

            analyze(EType.File, mainDirs, compareDirs);
        }

        private void analyzeDirectories()
        {
            var mainDirs = Directory.GetDirectories(currentMainPath);
            var compareDirs = Directory.GetDirectories(currentComparePath);

            analyze(EType.Directory, mainDirs, compareDirs);
        }

        private void analyze(EType type, String[] mainDirs, String[] currentDirs)
        {
            mainDirs = mainDirs.OrderBy(p => p).ToArray();
            currentDirs = currentDirs.OrderBy(p => p).ToArray();

            var mainDirsCount = mainDirs.Length;
            var currentDirsCount = currentDirs.Length;

            var currentMainDir = 0;
            var currentCompareDir = 0;
            
            while (currentMainDir < mainDirsCount && currentCompareDir < currentDirsCount)
            {
                var mainDirName = itemName(mainDirs[currentMainDir]);
                var compareDirName = itemName(currentDirs[currentCompareDir]);

                var mainFilePath = filePath(mainDirs[currentMainDir]);
                var compareFilePath = filePath(currentDirs[currentCompareDir]);

                var shouldVerifyMain = shouldVerify(type, mainDirName);
                var shouldVerifyCompare = shouldVerify(type, compareDirName);
                var shouldVerifyBoth = shouldVerifyMain && shouldVerifyCompare;

                if (!shouldVerifyMain) currentMainDir++;

                if (!shouldVerifyCompare) currentCompareDir++;

                if (shouldVerifyBoth)
                {
                    var compare = mainDirName.CompareTo(compareDirName);

                    switch ((ECompare)compare)
                    {
                        case ECompare.Greater:
                            addRow(Interface.NotExistsProblem, mainPath, compareFilePath, compareDirName);
                            currentCompareDir++;
                            break;
                        case ECompare.Less:
                            addRow(Interface.NotExistsProblem, comparePath, mainFilePath, mainDirName);
                            currentMainDir++;
                            break;
                        case ECompare.Equal:

                            switch (type)
                            {
                                case EType.File:
                                    var mainDate = Directory.GetLastWriteTime(mainDirs[currentMainDir]);
                                    var compareDate = Directory.GetLastWriteTime(currentDirs[currentCompareDir]);

                                    if (mainDate.IsReallyGreaterThan(compareDate))
                                        addRow(Interface.CSobrescrever, comparePath, mainFilePath, mainDirName);
                                    else if (compareDate.IsReallyGreaterThan(mainDate))
                                        addRow(Interface.CSobrescrever, mainPath, compareFilePath, compareDirName);
                                    break;

                                case EType.Directory:
                                    currentMainPath = mainDirs[currentMainDir];
                                    currentComparePath = currentDirs[currentCompareDir];
                                    CompareDirectories();

                                    break;
                            }

                            currentCompareDir++;
                            currentMainDir++;

                            break;
                    }
                }
            }

            for (var i = currentMainDir; i < mainDirsCount; i++)
            {
                var name = itemName(mainDirs[i]);
                var path = filePath(mainDirs[i]);

                if (shouldVerify(type, name))
                    addRow(Interface.NotExistsProblem, comparePath, path, name);
            }

            for (var i = currentCompareDir; i < currentDirsCount; i++)
            {
                var name = itemName(currentDirs[i]);
                var path = filePath(currentDirs[i]);

                if (shouldVerify(type, name))
                    addRow(Interface.NotExistsProblem, mainPath, path, name);
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



        private static bool shouldVerify(EType type, String name)
        {
            return type == EType.Directory
                ? name != "bin" && name != "obj" && name != ".svn" && !name.StartsWith("_ReSharper")
                : !name.EndsWith(".user") && !name.EndsWith(".suo") && !name.EndsWith(".ReSharper");
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
            String rowMainPath, rowComparePath, pathDestiny;
            DateTime mainDate, compareDate;
            
            var issue = row[Interface.ColAction].ToString();
            var pathOrigin = Path.Combine(
                    row[Interface.ColPath] + @"\",
                    row[Interface.ColFileName].ToString());
            var disk = row[Interface.ColDisk].ToString();


            if (disk == mainPath)
            {
                rowComparePath = pathOrigin;
                rowMainPath = pathOrigin.Replace(comparePath, mainPath);
                pathDestiny = rowMainPath;
            }
            else if (disk == comparePath)
            {
                rowMainPath = pathOrigin;
                rowComparePath = pathOrigin.Replace(mainPath, comparePath);
                pathDestiny = rowComparePath;
            }
            else
            {
                MessageBox.Show(Resources.Analyzer_VerifyToDelete);
                return;
            }

            switch (issue)
            {
                case Interface.CSobrescrever:

                    mainDate = File.GetLastWriteTime(rowMainPath);
                    compareDate = File.GetLastWriteTime(rowComparePath);
                    

                    if (mainDate.IsReallyGreaterThan(compareDate))
                    {
                        if (disk == comparePath)
                        {
                            Interface.IssueNotSolved(row);
                        }
                        else
                        {
                            row[Interface.ColDisk] = comparePath;
                            row[Interface.ColPath] = rowMainPath;
                        }
                    }
                    else if (compareDate.IsReallyGreaterThan(mainDate))
                    {
                        if (disk == mainPath)
                        {
                            Interface.IssueNotSolved(row);
                        }
                        else
                        {
                            row[Interface.ColDisk] = mainPath;
                            row[Interface.ColPath] = rowComparePath;
                        }
                    }
                    else
                    {
                        row.Delete();
                    }
                    break;

                case Interface.NotExistsProblem:

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
