using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Sync.Properties;

namespace Sync
{
    class Analyzer
    {
        private readonly String mainPath = "";
        private readonly String comparePath = "";
        private readonly AddRow addRow;

        public delegate void AddRow(String trouble, String obsolete, String updated, String path, String fileName);

        public Analyzer(String mainPath, String comparePath, AddRow addRow)
        {
            this.mainPath = mainPath;
            this.comparePath = comparePath;

            this.addRow = addRow;
        }


        public void CompareDirectories()
        {
            compareDirectories(mainPath, comparePath);
        }

        private void compareDirectories(String currentMainPath, String currentComparePath)
        {
            var sourceControl = new Versioning(currentMainPath, currentComparePath);

            switch (sourceControl.CurrentStatus)
            {
                case Versioning.Status.None:
                    
                    if (currentMainPath.EndsWith(":"))
                        currentMainPath += @"\";
                    
                    if (currentComparePath.EndsWith(":"))
                        currentComparePath += @"\";
                    
                    analyzeFiles(currentMainPath, currentComparePath);
                    analyzeDirectories(currentMainPath, currentComparePath);

                    break;

                case Versioning.Status.Fail:

                    throw new Exception(
                        String.Format(
                            "One of this is under source control, the other doesn't:\n{0}\n{1}"
                            , currentMainPath, currentComparePath
                            )
                        );

            }

            Application.DoEvents();
        }



        private void analyzeFiles(String currentMainPath, String currentComparePath)
        {
            var mainDirs = Directory.GetFiles(currentMainPath);
            var compareDirs = Directory.GetFiles(currentComparePath);

            analyze(EType.File, mainDirs, compareDirs);
        }

        private void analyzeDirectories(String currentMainPath, String currentComparePath)
        {
            var mainDirs = Directory.GetDirectories(currentMainPath);
            var compareDirs = Directory.GetDirectories(currentComparePath);

            analyze(EType.Directory, mainDirs, compareDirs);
        }

        private void analyze(EType type, IEnumerable<String> mainItems, IEnumerable<String> currentItems)
        {
            var main = new CollectionToTest(mainItems);
            var compare = new CollectionToTest(currentItems);

            while (main.NotEnded() && compare.NotEnded())
            {
                var mainItem = new FileToTest(type, main.GetCurrent(), mainPath);
                var compareItem = new FileToTest(type, compare.GetCurrent(), comparePath);

                if (!mainItem.ShouldVerify)
                    main.Next();

                if (!compareItem.ShouldVerify)
                    compare.Next();


                var shouldVerifyBoth = mainItem.ShouldVerify && compareItem.ShouldVerify;

                if (!shouldVerifyBoth)
                    continue;


                var diff = (ECompare) mainItem.Name.CompareTo(compareItem.Name);

                switch (diff)
                {
                    case ECompare.Greater:
                        addRow(Resources.Interface_Row_NotExistsProblem, mainPath, comparePath, compareItem.Path, compareItem.Name);
                        compare.Next();
                        break;

                    case ECompare.Less:
                        addRow(Resources.Interface_Row_NotExistsProblem, comparePath, mainPath, mainItem.Path, mainItem.Name);
                        main.Next();
                        break;

                    case ECompare.Equal:

                        switch (type)
                        {
                            case EType.File:
                                if (mainItem.IsNewerThan(compareItem))
                                    addRow(Resources.Interface_Row_ObseleteProblem, comparePath, mainPath, mainItem.Path, mainItem.Name);
                                else if (compareItem.IsNewerThan(mainItem))
                                    addRow(Resources.Interface_Row_ObseleteProblem, mainPath, comparePath, compareItem.Path, compareItem.Name);

                                break;

                            case EType.Directory:
                                compareDirectories(main.GetCurrent(), compare.GetCurrent());

                                break;
                        }

                        compare.Next();
                        main.Next();

                        break;
                }
            }

            while(main.NotEnded())
            {
                var item = new FileToTest(type, main.GetCurrent(), mainPath);

                if (item.ShouldVerify)
                    addRow(Resources.Interface_Row_NotExistsProblem, comparePath, mainPath, item.Path, item.Name);

                main.Next();
            }

            while(compare.NotEnded())
            {
                var item = new FileToTest(type, compare.GetCurrent(), comparePath);

                if (item.ShouldVerify)
                    addRow(Resources.Interface_Row_NotExistsProblem, mainPath, comparePath, item.Path, item.Name);

                compare.Next();
            }

        }




    }
}
