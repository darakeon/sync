using System;
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

            Table.Columns.Add(ColTrouble);
            Table.Columns.Add(ColObsolete);
            Table.Columns.Add(ColUpdated);
            Table.Columns.Add(ColPath);
            Table.Columns.Add(ColFileName);
            
            Table.PrimaryKey = new []
            {
                Table.Columns[0],
                Table.Columns[1],
                Table.Columns[2],
                Table.Columns[3],
                Table.Columns[4]
            };
        }


        public const String NotExistsProblem = "Não existe";
        public const String ObseleteProblem = "Desatualizado";
        public const String SolveNotExists = "Copiar";
        public const String SolveObselete = "Sobrescrever";

        private const Int32 maxLines = 20;

        public const String ColTrouble = "Problema";
        public const String ColObsolete = "Em";
        public const String ColUpdated = "Atualizado";
        public const String ColPath = "Caminho";
        public const String ColFileName = "Arquivo";
        public const String ColSolve = "Resolver";


        private const int defaultWidth = 180;
        private const int defaultHeight = 110;

        private const int rowLabelWidth = 43;
        private const int colLabelHeight = 22;

        private const int scrollWidth = 18;

        private const int txtMargin = 4;
        private const int lblMargin = 5;

        private const int frmMarginRight = 20;
        private const int frmMarginBottom = 4;
        

        private static readonly String[] denies = new[] { "Não", "NÃO", "Não..." };
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

                if (rowCount <= maxLines)
                {
                    heightIncrease = rowCount;
                    form.grdDados.ScrollBars = ScrollBars.None;
                }
                else
                {
                    heightIncrease = maxLines;
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

        public void AddRow(String trouble, String obsolete, String updated, String path, String fileName)
        {
            var row = Table.NewRow();

            row[ColTrouble] = trouble;
            
            row[ColObsolete] = obsolete;
            row[ColUpdated] = updated;

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
