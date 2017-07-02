using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sync
{
    public partial class Wait : Form
    {
        public Wait()
        {
            InitializeComponent();
        }

        public void Progress(CancellationToken token)
        {
            Show();
            Cursor = Cursors.WaitCursor;

            var counter = 0;


























            progressBar00.Value = 9;
            progressBar01.Value = 15;
            progressBar02.Value = 20;
            progressBar03.Value = 18;
            progressBar04.Value = 6;
            progressBar05.Value = 16;
            progressBar06.Value = 15;
            progressBar07.Value = 11;
            progressBar08.Value = 11;
            progressBar09.Value = 19;
            progressBar10.Value = 7;
            progressBar11.Value = 12;
            progressBar12.Value = 14;
            progressBar13.Value = 1;
            progressBar14.Value = 7;
            progressBar15.Value = 10;
            progressBar16.Value = 9;
            progressBar17.Value = 13;
            progressBar18.Value = 18;
            progressBar19.Value = 8;
            progressBar20.Value = 10;
            progressBar21.Value = 9;
            progressBar22.Value = 9;
            progressBar23.Value = 12;
            progressBar24.Value = 7;
            progressBar25.Value = 3;

            while (!token.IsCancellationRequested)
            {
                progressBar00.Value = progressBar01.Value;
                progressBar01.Value = progressBar02.Value;
                progressBar02.Value = progressBar03.Value;
                progressBar03.Value = progressBar04.Value;
                progressBar04.Value = progressBar05.Value;
                progressBar05.Value = progressBar06.Value;
                progressBar06.Value = progressBar07.Value;
                progressBar07.Value = progressBar08.Value;
                progressBar08.Value = progressBar09.Value;
                progressBar09.Value = progressBar10.Value;
                progressBar10.Value = progressBar11.Value;
                progressBar11.Value = progressBar12.Value;
                progressBar12.Value = progressBar13.Value;
                progressBar13.Value = progressBar14.Value;
                progressBar14.Value = progressBar15.Value;
                progressBar15.Value = progressBar16.Value;
                progressBar16.Value = progressBar17.Value;
                progressBar17.Value = progressBar18.Value;
                progressBar18.Value = progressBar19.Value;
                progressBar19.Value = progressBar20.Value;
                progressBar20.Value = progressBar21.Value;
                progressBar21.Value = progressBar22.Value;
                progressBar22.Value = progressBar23.Value;
                progressBar23.Value = progressBar24.Value;
                progressBar24.Value = progressBar25.Value;
                progressBar25.Value = progressBar26.Value;
                random(progressBar26);

                counter++;

                Thread.Sleep(123);
            }

            Hide();
        }

        readonly Random number = new Random();

        private void random(ProgressBar progressBar)
        {
            progressBar.Value = number.Next(progressBar.Minimum, progressBar.Maximum);
        }




    }
}
