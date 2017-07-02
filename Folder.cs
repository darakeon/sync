using System;
using System.Configuration;
using System.IO;
using System.Windows.Forms;
using Sync.Properties;

namespace Sync
{
    public partial class Folder : Form
    {
        #region INIT
        public Folder()
        {
            InitializeComponent();
            @interface = new Interface();

            txtMainPath.Text = ConfigurationManager.AppSettings["MainPath"];
            txtComparePath.Text = ConfigurationManager.AppSettings["ComparePath"];
        }

        private String mainPath;
        private String comparePath;

        private Analyzer analyzer;
        private readonly Interface @interface;
        #endregion

        private void charge()
        {
            @interface.Clear();

            mainPath = getPath(txtMainPath);
            comparePath = getPath(txtComparePath);

            if (mainPath == null || comparePath == null)
                return;

            analyzer = new Analyzer(mainPath, comparePath, @interface.AddRow);



            try
            {
                analyzer.CompareDirectories();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }


            
            if (@interface.RowsCount == 0)
            {
                Interface.Realign(this);
                MessageBox.Show(Resources.Folder_NotFilesFound);
            }
            else
            {
                grdDados.DataSource = @interface.Table;

                setSolveButton();

                Interface.Realign(this);
                MessageBox.Show(
                    string.Format(Resources.Folder_SyncFilesCount, grdDados.RowCount)
                );
            }

        }

        private void setSolveButton()
        {
            var solveButton = new DataGridViewButtonColumn
                                  {
                                      Name = Interface.ColSolve,
                                      Text = Interface.ColSolve
                                  };

            grdDados.Columns.Add(solveButton);

            foreach (DataGridViewRow row in grdDados.Rows)
            {
                var action = row.Cells[Interface.ColTrouble].Value.ToString();
                var button = row.Cells[Interface.ColSolve];

                switch (action)
                {
                    case Interface.NotExistsProblem:
                        button.Value = Interface.SolveNotExists;
                        break;
                    case Interface.ObseleteProblem:
                        button.Value = Interface.SolveObselete;
                        break;
                }
                
            }
        }

        private String getPath(Control textBox)
        {
            var path = Path.Combine(textBox.Text, txtSubfolder.Text);

            if (Directory.Exists(path))
            {
                if (path.EndsWith("/") || path.EndsWith(@"\"))
                    path = path.Substring(0, path.Length - 1);

                return path;
            }

            MessageBox.Show( String.Format(@"""{0}"" não existe!", path));
            textBox.Focus();
            return null;
        }

        private void btnAgainClick(object sender, EventArgs e)
        {
            btnAgain.Enabled = false;
            Cursor = Cursors.WaitCursor;

            charge();

            btnAgain.Enabled = true;
            Cursor = Cursors.Default;

            //this.Folder_Resize(sender, e);
        }

        private void grdDadosCellClick(object sender, DataGridViewCellEventArgs e)
        {
            var buttonColumn = grdDados.Columns[Interface.ColSolve];

            var isLastColumn = e.ColumnIndex == buttonColumn.Index;
            var isHeaderLine = e.RowIndex < 0;

            if (!isLastColumn || isHeaderLine)
                return;

            var cell = grdDados.Rows[e.RowIndex].Cells[e.ColumnIndex];
            var done = false;

            switch (cell.Value.ToString())
            {
                case Interface.SolveNotExists:
                    done = copy(e.RowIndex);
                    break;
                case Interface.SolveObselete:
                    done = overwrite(e.RowIndex);
                    break;
            }

            if (done)
                Interface.Realign(this);
        }


        private Boolean copy(Int32 rowNumber)
        {
            var row = grdDados.Rows[rowNumber];

            var updated = row.Cells[Interface.ColUpdated].Value.ToString();
            var obsolete = row.Cells[Interface.ColObsolete].Value.ToString();

            var relativePath = row.Cells[Interface.ColPath].Value.ToString();
            var file = row.Cells[Interface.ColFileName].Value.ToString();

            var origin = new FileToWrite(updated, relativePath, file);
            var destiny = new FileToWrite(obsolete, relativePath, file);

            var confirm = confirmBox(Resources.Folder_SureCopy, origin.Path, file, destiny.Date, origin.Date);

            if (!confirm)
                return false;
            
            throw new NotImplementedException();
        }

        
        private Boolean overwrite(Int32 rowNumber)
        {
            var confirm = confirmBox(Resources.Folder_SureOverwrite);

            if (!confirm)
                return false;
            throw new NotImplementedException();
        }
        

        private Boolean confirmBox(String format, params object[] args)
        {
            var message = String.Format(format, args);

            var response = MessageBox.Show(message, Resources.Sync, MessageBoxButtons.YesNo);

            return response == DialogResult.Yes;
        }


    }

}


