using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace Sync
{
    class Interface
    {
        public DataTable Table { get; private set; }

        public Interface()
        {
            Table = new DataTable();

            Table.Columns.Add(ColAction);
            Table.Columns.Add(ColDisk);
            Table.Columns.Add(ColPath);
            Table.Columns.Add(ColFileName);
            
            Table.Columns.Add(ColVerify).DefaultValue = ColVerify;

            var primaryKeys = new List<DataColumn>
                                  {
                                      Table.Columns[0],
                                      Table.Columns[1],
                                      Table.Columns[2],
                                      Table.Columns[3]
                                  };

            Table.PrimaryKey = primaryKeys.ToArray();
        }


        public const String NotExistsProblem = "Não existe";
        public const String CSobrescrever = "Sobrescrever";
        private const Int32 cMaxLinhas = 20;

        public const String ColAction = "Ação";
        public const String ColDisk = "Em";
        public const String ColPath = "Caminho";
        public const String ColFileName = "Arquivo";
        public const String ColVerify = "Verifica";


        private const int defaultWidth = 180;
        private const int defaultHeight = 110;

        private const int rowLabelWidth = 43;
        private const int colLabelHeight = 22;

        private const int scrollWidth = 18;

        private const int txtMargin = 4;
        private const int lblMargin = 5;

        private const int frmMarginRight = 20;
        private const int frmMarginBottom = 4;
        

        private static readonly String[] No = new[] { "Não", "NÃO", "Não..." };
        private static Int32 witchNo;


        public static void Realign(Folder form)
        {
            var rowCount = form.grdDados.RowCount;

            int formWidth;

            if (rowCount == 0)
            {
                formWidth = defaultWidth;
                form.Height = defaultHeight;
            }
            else
            {
                formWidth = rowLabelWidth;

                for (var i = 0; i < form.grdDados.ColumnCount; i++)
                {
                    formWidth += form.grdDados.Columns[i].Width;
                }

                form.grdDados.Width = formWidth;

                
                int heightIncrease;

                if (rowCount <= cMaxLinhas)
                {
                    heightIncrease = rowCount;
                    form.grdDados.ScrollBars = ScrollBars.None;
                }
                else
                {
                    heightIncrease = cMaxLinhas;
                    form.grdDados.Width += scrollWidth;
                    formWidth += scrollWidth;
                }

                form.grdDados.BackgroundColor = form.BackColor;

                form.grdDados.Height = colLabelHeight + heightIncrease * form.grdDados.Rows[0].Height;
                form.Height = form.grdDados.Height + defaultHeight + frmMarginBottom;
            }

            form.Width = formWidth + frmMarginRight;

            form.btnAgain.Width = formWidth;

            var buttonWidth = formWidth / 3;

            form.txtMainPath.Width = buttonWidth;
            form.txtComparePath.Width = buttonWidth;
            form.txtSubfolder.Width = buttonWidth;

            form.txtComparePath.Left = buttonWidth + txtMargin;
            form.txtSubfolder.Left = 2 * buttonWidth + txtMargin;

            form.lblComparePath.Left = buttonWidth + lblMargin;
            form.lblSubfolder.Left = 2 * buttonWidth + lblMargin;
        }

        public static void IssueNotSolved(DataRow row)
        {
            row[ColVerify] = No[witchNo];
            witchNo++;
            witchNo = witchNo % No.Length;
        }

        public void AddRow(String trouble, String disk, String path, String fileName)
        {
            var row = Table.NewRow();

            row[ColAction] = trouble;
            row[ColDisk] = disk;

            row[ColPath] = path;
            row[ColFileName] = fileName;

            Table.Rows.Add(row);
        }

        public void Clear()
        {
            Table.Clear();
        }

        public int RowsCount { get { return Table.Rows.Count; } }
    }
}
