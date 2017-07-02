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

        private void Charge()
        {
            @interface.Clear();

            mainPath = GetPath(txtMainPath);
            comparePath = GetPath(txtComparePath);

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
                MessageBox.Show(Resources.Folder_Charge);
            }
            else
            {
                grdDados.DataSource = @interface.Table;

                Interface.Realign(this);
                MessageBox.Show(
                    string.Format("{0} arquivo(s) a sincronizar.", grdDados.RowCount)
                );
            }

        }

        private string GetPath(Control textBox)
        {
            var path = string.Format("{0}{1}", textBox.Text, txtSubfolder.Text);

            if (Directory.Exists(path))
            {
                return path;
            }


            MessageBox.Show( string.Format(@"""{0}"" não existe!", path));
            textBox.Focus();
            return null;
        }

        private void BtnAgainClick(object sender, EventArgs e)
        {
            btnAgain.Enabled = false;
            Cursor = Cursors.WaitCursor;

            Charge();

            btnAgain.Enabled = true;
            Cursor = Cursors.Default;

            //this.Folder_Resize(sender, e);
        }

        private void GrdDadosCellClick(object sender, DataGridViewCellEventArgs e)
        {
            var isLastColumn = e.ColumnIndex == grdDados.ColumnCount - 1;
            var isHeaderLine = e.RowIndex < 0;

            if (!isLastColumn || isHeaderLine)
                return;


            analyzer.VerifyToDelete(grdDados.Rows[e.RowIndex], @interface.Table);

            Interface.Realign(this);
        }

    }

}


