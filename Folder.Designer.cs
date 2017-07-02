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
            this.lblSubPasta = new System.Windows.Forms.Label();
            this.lblLucy = new System.Windows.Forms.Label();
            this.lblHandhara = new System.Windows.Forms.Label();
            this.txtSubPasta = new System.Windows.Forms.TextBox();
            this.txtLucy = new System.Windows.Forms.TextBox();
            this.txtHandhara = new System.Windows.Forms.TextBox();
            this.btnAgain = new System.Windows.Forms.Button();
            this.grdDados = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.grdDados)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSubPasta
            // 
            this.lblSubPasta.AutoSize = true;
            this.lblSubPasta.Location = new System.Drawing.Point(123, 28);
            this.lblSubPasta.Name = "lblSubPasta";
            this.lblSubPasta.Size = new System.Drawing.Size(52, 13);
            this.lblSubPasta.TabIndex = 23;
            this.lblSubPasta.Text = "Subfolder";
            // 
            // lblLucy
            // 
            this.lblLucy.AutoSize = true;
            this.lblLucy.Location = new System.Drawing.Point(63, 28);
            this.lblLucy.Name = "lblLucy";
            this.lblLucy.Size = new System.Drawing.Size(30, 13);
            this.lblLucy.TabIndex = 22;
            this.lblLucy.Text = "Lucy";
            // 
            // lblHandhara
            // 
            this.lblHandhara.AutoSize = true;
            this.lblHandhara.Location = new System.Drawing.Point(3, 28);
            this.lblHandhara.Name = "lblHandhara";
            this.lblHandhara.Size = new System.Drawing.Size(54, 13);
            this.lblHandhara.TabIndex = 21;
            this.lblHandhara.Text = "Handhara";
            // 
            // txtSubPasta
            // 
            this.txtSubPasta.Location = new System.Drawing.Point(122, 45);
            this.txtSubPasta.Name = "txtSubPasta";
            this.txtSubPasta.Size = new System.Drawing.Size(60, 20);
            this.txtSubPasta.TabIndex = 20;
            // 
            // txtLucy
            // 
            this.txtLucy.Location = new System.Drawing.Point(62, 45);
            this.txtLucy.Name = "txtLucy";
            this.txtLucy.Size = new System.Drawing.Size(60, 20);
            this.txtLucy.TabIndex = 19;
            // 
            // txtHandhara
            // 
            this.txtHandhara.Location = new System.Drawing.Point(2, 45);
            this.txtHandhara.Name = "txtHandhara";
            this.txtHandhara.Size = new System.Drawing.Size(60, 20);
            this.txtHandhara.TabIndex = 18;
            // 
            // btnAgain
            // 
            this.btnAgain.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAgain.Location = new System.Drawing.Point(0, 0);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(184, 23);
            this.btnAgain.TabIndex = 17;
            this.btnAgain.Text = "RUN";
            this.btnAgain.UseVisualStyleBackColor = true;
            this.btnAgain.Click += new System.EventHandler(this.BtnAgainClick);
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
            this.grdDados.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.GrdDadosCellClick);
            // 
            // Folder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(184, 72);
            this.Controls.Add(this.lblSubPasta);
            this.Controls.Add(this.lblLucy);
            this.Controls.Add(this.lblHandhara);
            this.Controls.Add(this.txtSubPasta);
            this.Controls.Add(this.txtLucy);
            this.Controls.Add(this.txtHandhara);
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

        internal System.Windows.Forms.Label lblSubPasta;
        internal System.Windows.Forms.Label lblLucy;
        internal System.Windows.Forms.Label lblHandhara;
        internal System.Windows.Forms.TextBox txtSubPasta;
        internal System.Windows.Forms.TextBox txtLucy;
        internal System.Windows.Forms.TextBox txtHandhara;
        internal System.Windows.Forms.Button btnAgain;
        internal System.Windows.Forms.DataGridView grdDados;

    }
}