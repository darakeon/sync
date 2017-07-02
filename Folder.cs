using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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
            txtSubfolder.Text = ConfigurationManager.AppSettings["Subfolder"];

            lblMainPath.Text = Resources.Interface_Field_MainPath;
            lblComparePath.Text = Resources.Interface_Field_ComparePath;
            lblSubfolder.Text = Resources.Interface_Field_Subfolder;
        }

        protected override void OnLoad(EventArgs e)
        {
            Interface.Realign(this);

            base.OnLoad(e);
        }

        private String mainPath;
        private String comparePath;

        private Analyzer analyzer;
        private readonly Interface @interface;
        #endregion



        private String charge()
        {
            @interface.Clear();

            mainPath = getPath(txtMainPath);
            comparePath = getPath(txtComparePath);

            if (mainPath == null || comparePath == null)
                return Resources.Folder_FillTheFields;

            analyzer = new Analyzer(mainPath, comparePath, @interface.AddRow);


            try
            {
                analyzer.CompareDirectories();
            }
            catch (Exception e)
            {
                return e.Message;
            }


            
            if (@interface.RowsCount == 0)
            {
                Interface.Realign(this);
                return Resources.Folder_NoFilesFound;
            }
            
            
            grdDados.DataSource = @interface.Table;

            setSolveButton();

            Interface.Realign(this);

            return String.Format(Resources.Folder_SyncFilesCount, grdDados.RowCount);
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

            MessageBox.Show(String.Format(@"""{0}"" não existe!", path));
            textBox.Focus();
            return null;
        }

        private void setSolveButton()
        {
            addButton(Resources.Interface_Button_Update);
            addButton(Resources.Interface_Button_Rollback);

            foreach (DataGridViewRow row in grdDados.Rows)
            {
                var action = row.Cells[Resources.Interface_Column_Trouble].Value.ToString();
                var acceptButton = row.Cells[Resources.Interface_Button_Update];
                var refuseButton = row.Cells[Resources.Interface_Button_Rollback];

                if (action == Resources.Interface_Row_NotExistsProblem)
                {
                    acceptButton.Value = Resources.Interface_Action_UpdateNotExists;
                    refuseButton.Value = Resources.Interface_Action_RollbackNotExists;
                }
                
                if (action == Resources.Interface_Row_ObseleteProblem)
                {
                    acceptButton.Value = Resources.Interface_Action_UpdateObselete;
                    refuseButton.Value = Resources.Interface_Action_RollbackObselete;
                }
            }
        }

        private void addButton(String name)
        {
            if (grdDados.Columns.Contains(name))
                return;

            var button = 
                new DataGridViewButtonColumn
                    {
                        Name = name,
                        Text = name
                    };

            grdDados.Columns.Add(button);
        }

        

        private void executeButtonAction(DataGridViewCellEventArgs e)
        {
            var isHeaderLine = e.RowIndex < 0;

            if (isHeaderLine)
                return;

            var acceptButton = getButton(Resources.Interface_Button_Update);
            var invertButton = getButton(Resources.Interface_Button_Rollback);

            var isAcceptButton = e.ColumnIndex == acceptButton.Index;
            var isInvertButton = e.ColumnIndex == invertButton.Index;
            var isButton = isAcceptButton || isInvertButton;

            if (!isButton)
                return;

            var row = grdDados.Rows[e.RowIndex];
            var cell = row.Cells[e.ColumnIndex];
            var done = false;

            var cellValue = cell.Value.ToString();

            var fixer = new Fixer(row);

            try
            {
                if (cellValue == Resources.Interface_Action_UpdateNotExists)
                    done = fixer.Copy();

                if (cellValue == Resources.Interface_Action_UpdateObselete)
                    done = fixer.Update();

                if (cellValue == Resources.Interface_Action_RollbackNotExists)
                    done = fixer.Delete();

                if (cellValue == Resources.Interface_Action_RollbackObselete)
                    done = fixer.Rollback();
            }
            catch (ApplicationException error)
            {
                MessageBox.Show(error.Message);
            }

            if (done)
            {
                grdDados.Rows.Remove(row);
                Interface.Realign(this);

                if (grdDados.Rows.Count == 0)
                    MessageBox.Show(Resources.Folder_NoFilesFound);
            }
        }

        private DataGridViewColumn getButton(String buttonName)
        {
            var button = grdDados.Columns[buttonName];

            if (button == null)
                throw new ApplicationException(Resources.Folder_MissingButton);

            return button;
        }



        private void btnAgainClick(object sender, EventArgs e)
        {
            var cancelToken = callWait();

            var message = String.Empty;

            try
            {
                message = charge();
            }
            finally
            {
                cancelToken.Cancel();

                if (!String.IsNullOrWhiteSpace(message))
                    MessageBox.Show(message);

                Show();
            }
        }



        private void grdDadosCellClick(object sender, DataGridViewCellEventArgs e)
        {
            var cancelToken = callWait();

            try
            {
                executeButtonAction(e);
            }
            finally
            {
                cancelToken.Cancel();
                Show();
            }
        }


        
        private CancellationTokenSource callWait()
        {
            Hide();

            var cancelToken = new CancellationTokenSource();

            var task = new Task(() => new Wait().Progress(cancelToken.Token), cancelToken.Token);
            task.Start();

            return cancelToken;
        }


    }

}


