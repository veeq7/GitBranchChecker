namespace GitBranchChecker
{
    partial class BranchCheckerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BranchCheckerForm));
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnRegisterAssosiation = new System.Windows.Forms.Button();
            this.startFilterDate = new System.Windows.Forms.DateTimePicker();
            this.endFilterDate = new System.Windows.Forms.DateTimePicker();
            this.doFilterStartDate = new System.Windows.Forms.CheckBox();
            this.doFilterEndDate = new System.Windows.Forms.CheckBox();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.ProgressBarInfo = new System.Windows.Forms.Label();
            this.abortLoading = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(359, 20);
            this.textBox1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.WhiteSmoke;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 64);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowTemplate.Height = 45;
            this.dataGridView1.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.Size = new System.Drawing.Size(776, 333);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(377, 11);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(51, 21);
            this.btnSelectFile.TabIndex = 2;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_click);
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(707, 418);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(81, 20);
            this.btnCompare.TabIndex = 4;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnRegisterAssosiation
            // 
            this.btnRegisterAssosiation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRegisterAssosiation.BackgroundImage = global::GitBranchChecker.Properties.Resources.RegEdit1;
            this.btnRegisterAssosiation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnRegisterAssosiation.Location = new System.Drawing.Point(755, 9);
            this.btnRegisterAssosiation.Name = "btnRegisterAssosiation";
            this.btnRegisterAssosiation.Size = new System.Drawing.Size(33, 30);
            this.btnRegisterAssosiation.TabIndex = 3;
            this.btnRegisterAssosiation.UseVisualStyleBackColor = true;
            this.btnRegisterAssosiation.Click += new System.EventHandler(this.btnRegisterAssosiation_Click);
            // 
            // startFilterDate
            // 
            this.startFilterDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.startFilterDate.Enabled = false;
            this.startFilterDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.startFilterDate.Location = new System.Drawing.Point(555, 12);
            this.startFilterDate.Name = "startFilterDate";
            this.startFilterDate.Size = new System.Drawing.Size(171, 20);
            this.startFilterDate.TabIndex = 5;
            this.startFilterDate.Value = new System.DateTime(2000, 2, 1, 0, 0, 0, 0);
            this.startFilterDate.ValueChanged += new System.EventHandler(this.startFilterDate_ValueChanged);
            // 
            // endFilterDate
            // 
            this.endFilterDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.endFilterDate.Enabled = false;
            this.endFilterDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.endFilterDate.Location = new System.Drawing.Point(555, 38);
            this.endFilterDate.Name = "endFilterDate";
            this.endFilterDate.Size = new System.Drawing.Size(171, 20);
            this.endFilterDate.TabIndex = 6;
            this.endFilterDate.Value = new System.DateTime(2050, 12, 31, 0, 0, 0, 0);
            this.endFilterDate.ValueChanged += new System.EventHandler(this.endFilterDate_ValueChanged);
            // 
            // doFilterStartDate
            // 
            this.doFilterStartDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doFilterStartDate.AutoSize = true;
            this.doFilterStartDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.doFilterStartDate.Location = new System.Drawing.Point(475, 17);
            this.doFilterStartDate.Name = "doFilterStartDate";
            this.doFilterStartDate.Size = new System.Drawing.Size(74, 17);
            this.doFilterStartDate.TabIndex = 7;
            this.doFilterStartDate.Text = "Start Date";
            this.doFilterStartDate.UseVisualStyleBackColor = true;
            this.doFilterStartDate.CheckedChanged += new System.EventHandler(this.doFilterStartDate_CheckedChanged);
            // 
            // doFilterEndDate
            // 
            this.doFilterEndDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.doFilterEndDate.AutoSize = true;
            this.doFilterEndDate.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.doFilterEndDate.Location = new System.Drawing.Point(478, 38);
            this.doFilterEndDate.Name = "doFilterEndDate";
            this.doFilterEndDate.Size = new System.Drawing.Size(71, 17);
            this.doFilterEndDate.TabIndex = 8;
            this.doFilterEndDate.Text = "End Date";
            this.doFilterEndDate.UseVisualStyleBackColor = true;
            this.doFilterEndDate.CheckedChanged += new System.EventHandler(this.doFilterEndDate_CheckedChanged);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Location = new System.Drawing.Point(15, 418);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(459, 20);
            this.ProgressBar.TabIndex = 9;
            // 
            // ProgressBarInfo
            // 
            this.ProgressBarInfo.AutoSize = true;
            this.ProgressBarInfo.Location = new System.Drawing.Point(12, 402);
            this.ProgressBarInfo.Name = "ProgressBarInfo";
            this.ProgressBarInfo.Size = new System.Drawing.Size(62, 13);
            this.ProgressBarInfo.TabIndex = 10;
            this.ProgressBarInfo.Text = "Select a file";
            // 
            // abortLoading
            // 
            this.abortLoading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.abortLoading.Enabled = false;
            this.abortLoading.Location = new System.Drawing.Point(480, 418);
            this.abortLoading.Name = "abortLoading";
            this.abortLoading.Size = new System.Drawing.Size(81, 20);
            this.abortLoading.TabIndex = 11;
            this.abortLoading.Text = "Abort";
            this.abortLoading.UseVisualStyleBackColor = true;
            this.abortLoading.Click += new System.EventHandler(this.abortLoading_Click);
            // 
            // BranchCheckerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.abortLoading);
            this.Controls.Add(this.ProgressBarInfo);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.doFilterEndDate);
            this.Controls.Add(this.doFilterStartDate);
            this.Controls.Add(this.endFilterDate);
            this.Controls.Add(this.startFilterDate);
            this.Controls.Add(this.btnCompare);
            this.Controls.Add(this.btnRegisterAssosiation);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.textBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BranchCheckerForm";
            this.Text = "BranchChecker";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnRegisterAssosiation;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.DateTimePicker startFilterDate;
        private System.Windows.Forms.DateTimePicker endFilterDate;
        private System.Windows.Forms.CheckBox doFilterStartDate;
        private System.Windows.Forms.CheckBox doFilterEndDate;
        public System.Windows.Forms.ProgressBar ProgressBar;
        public System.Windows.Forms.Label ProgressBarInfo;
        private System.Windows.Forms.Button abortLoading;
    }
}