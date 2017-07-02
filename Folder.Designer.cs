namespace Sync
{
    partial class Folder
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Folder));
            this.lblSubfolder = new System.Windows.Forms.Label();
            this.lblComparePath = new System.Windows.Forms.Label();
            this.lblMainPath = new System.Windows.Forms.Label();
            this.txtSubfolder = new System.Windows.Forms.TextBox();
            this.txtComparePath = new System.Windows.Forms.TextBox();
            this.txtMainPath = new System.Windows.Forms.TextBox();
            this.btnAgain = new System.Windows.Forms.Button();
            this.grdDados = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdDados)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSubfolder
            // 
            this.lblSubfolder.AutoSize = true;
            this.lblSubfolder.Location = new System.Drawing.Point(175, 30);
            this.lblSubfolder.Name = "lblSubfolder";
            this.lblSubfolder.Size = new System.Drawing.Size(52, 13);
            this.lblSubfolder.TabIndex = 23;
            this.lblSubfolder.Text = "Subfolder";
            // 
            // lblComparePath
            // 
            this.lblComparePath.AutoSize = true;
            this.lblComparePath.Location = new System.Drawing.Point(90, 30);
            this.lblComparePath.Name = "lblComparePath";
            this.lblComparePath.Size = new System.Drawing.Size(76, 13);
            this.lblComparePath.TabIndex = 22;
            this.lblComparePath.Text = "Compare path:";
            // 
            // lblMainPath
            // 
            this.lblMainPath.AutoSize = true;
            this.lblMainPath.Location = new System.Drawing.Point(5, 30);
            this.lblMainPath.Name = "lblMainPath";
            this.lblMainPath.Size = new System.Drawing.Size(57, 13);
            this.lblMainPath.TabIndex = 21;
            this.lblMainPath.Text = "Main path:";
            // 
            // txtSubfolder
            // 
            this.txtSubfolder.Location = new System.Drawing.Point(175, 45);
            this.txtSubfolder.Name = "txtSubfolder";
            this.txtSubfolder.Size = new System.Drawing.Size(60, 20);
            this.txtSubfolder.TabIndex = 20;
            // 
            // txtComparePath
            // 
            this.txtComparePath.Location = new System.Drawing.Point(90, 45);
            this.txtComparePath.Name = "txtComparePath";
            this.txtComparePath.Size = new System.Drawing.Size(80, 20);
            this.txtComparePath.TabIndex = 19;
            // 
            // txtMainPath
            // 
            this.txtMainPath.Location = new System.Drawing.Point(5, 45);
            this.txtMainPath.Name = "txtMainPath";
            this.txtMainPath.Size = new System.Drawing.Size(80, 20);
            this.txtMainPath.TabIndex = 18;
            // 
            // btnAgain
            // 
            this.btnAgain.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAgain.Location = new System.Drawing.Point(0, 0);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(240, 23);
            this.btnAgain.TabIndex = 17;
            this.btnAgain.Text = "RUN";
            this.btnAgain.UseVisualStyleBackColor = true;
            this.btnAgain.Click += new System.EventHandler(this.btnAgainClick);
            // 
            // grdDados
            // 
            this.grdDados.AllowUserToAddRows = false;
            this.grdDados.AllowUserToDeleteRows = false;
            this.grdDados.AllowUserToResizeColumns = false;
            this.grdDados.AllowUserToResizeRows = false;
            this.grdDados.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.grdDados.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdDados.Location = new System.Drawing.Point(2, 74);
            this.grdDados.Name = "grdDados";
            this.grdDados.ReadOnly = true;
            this.grdDados.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.grdDados.Size = new System.Drawing.Size(180, 45);
            this.grdDados.TabIndex = 16;
            this.grdDados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdDadosCellClick);
            // 
            // Folder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(240, 72);
            this.Controls.Add(this.lblSubfolder);
            this.Controls.Add(this.lblComparePath);
            this.Controls.Add(this.lblMainPath);
            this.Controls.Add(this.txtSubfolder);
            this.Controls.Add(this.txtComparePath);
            this.Controls.Add(this.txtMainPath);
            this.Controls.Add(this.btnAgain);
            this.Controls.Add(this.grdDados);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Folder";
            this.Text = "Sync";
            ((System.ComponentModel.ISupportInitialize)(this.grdDados)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label lblSubfolder;
        internal System.Windows.Forms.Label lblComparePath;
        internal System.Windows.Forms.Label lblMainPath;
        internal System.Windows.Forms.TextBox txtSubfolder;
        internal System.Windows.Forms.TextBox txtComparePath;
        internal System.Windows.Forms.TextBox txtMainPath;
        internal System.Windows.Forms.Button btnAgain;
        internal System.Windows.Forms.DataGridView grdDados;

    }
}