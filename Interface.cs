using System;
using System.Data;
using System.Windows.Forms;
using Sync.Properties;

namespace Sync
{
    class Interface
    {
        public DataTable Table { get; private set; }

        public Interface()
        {
            Table = new DataTable();

            Table.Columns.Add(Resources.Interface_Column_Trouble);
            Table.Columns.Add(Resources.Interface_Column_Obsolete);
            Table.Columns.Add(Resources.Interface_Column_Updated);
            Table.Columns.Add(Resources.Interface_Column_Path);
            Table.Columns.Add(Resources.Interface_Column_FileName);
            
            Table.PrimaryKey = new []
            {
                Table.Columns[0],
                Table.Columns[1],
                Table.Columns[2],
                Table.Columns[3],
                Table.Columns[4]
            };
        }



        private const Int32 maxLines = 20;

        private const int defaultWidth = 276;
        private const int defaultHeight = 110;

        private const int rowLabelWidth = 43;
        private const int colLabelHeight = 38;

        private const int scrollWidth = 18;

        private const int txtMargin = 5;
        private const int lblMargin = 5;

        private const int frmMarginRight = 20;
        private const int frmMarginBottom = 4;


        public static void Realign(Folder form)
        {
            var rowCount = form.grdDados == null
                ? 0 : form.grdDados.RowCount;


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

            var buttonWidth = (formWidth - 3 * txtMargin) / 3;

            form.txtMainPath.Width = buttonWidth;
            form.txtComparePath.Width = buttonWidth;
            form.txtSubfolder.Width = buttonWidth;

            var mainSpace = form.txtMainPath.Width + form.txtMainPath.Left;
            form.txtComparePath.Left = mainSpace + txtMargin;
            form.lblComparePath.Left = mainSpace + lblMargin;

            var compareSpace = form.txtComparePath.Width + form.txtComparePath.Left;
            form.txtSubfolder.Left = compareSpace + txtMargin;
            form.lblSubfolder.Left = compareSpace + lblMargin;
        }

        public void AddRow(String trouble, String obsolete, String updated, String path, String fileName)
        {
            path = String.IsNullOrWhiteSpace(path) 
                ? Resources.Interface_Row_Root 
                : path.Substring(1);

            var row = Table.NewRow();

            row[Resources.Interface_Column_Trouble] = trouble;

            row[Resources.Interface_Column_Obsolete] = obsolete;
            row[Resources.Interface_Column_Updated] = updated;

            row[Resources.Interface_Column_Path] = path;
            row[Resources.Interface_Column_FileName] = fileName;

            Table.Rows.Add(row);
        }

        public void Clear()
        {
            Table.Clear();
        }

        public int RowsCount { get { return Table.Rows.Count; } }
    }
}
