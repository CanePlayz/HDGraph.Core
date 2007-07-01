namespace HDGraph
{
    partial class DirectoryDetailUC
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.Label filesSizeLabel;
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label totalSizeLabel;
            System.Windows.Forms.Label pathLabel;
            System.Windows.Forms.Label labelFolderContent;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.directoryNodeDataGridView = new System.Windows.Forms.DataGridView();
            this.directoryNodeListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.filesSizeTextBox = new System.Windows.Forms.TextBox();
            this.directoryNodeEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.totalSizeTextBox = new System.Windows.Forms.TextBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.DirectoryType = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            filesSizeLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            totalSizeLabel = new System.Windows.Forms.Label();
            pathLabel = new System.Windows.Forms.Label();
            labelFolderContent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // filesSizeLabel
            // 
            filesSizeLabel.AutoSize = true;
            filesSizeLabel.Location = new System.Drawing.Point(14, 88);
            filesSizeLabel.Name = "filesSizeLabel";
            filesSizeLabel.Size = new System.Drawing.Size(103, 13);
            filesSizeLabel.TabIndex = 5;
            filesSizeLabel.Text = "Files Size (in Bytes) :";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(14, 10);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 7;
            nameLabel.Text = "Name:";
            // 
            // totalSizeLabel
            // 
            totalSizeLabel.AutoSize = true;
            totalSizeLabel.Location = new System.Drawing.Point(14, 62);
            totalSizeLabel.Name = "totalSizeLabel";
            totalSizeLabel.Size = new System.Drawing.Size(106, 13);
            totalSizeLabel.TabIndex = 9;
            totalSizeLabel.Text = "Total Size (in Bytes) :";
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new System.Drawing.Point(14, 36);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new System.Drawing.Size(32, 13);
            pathLabel.TabIndex = 10;
            pathLabel.Text = "Path:";
            // 
            // labelFolderContent
            // 
            labelFolderContent.AutoSize = true;
            labelFolderContent.Location = new System.Drawing.Point(14, 116);
            labelFolderContent.Name = "labelFolderContent";
            labelFolderContent.Size = new System.Drawing.Size(81, 13);
            labelFolderContent.TabIndex = 12;
            labelFolderContent.Text = "Folder content :";
            // 
            // directoryNodeDataGridView
            // 
            this.directoryNodeDataGridView.AllowUserToAddRows = false;
            this.directoryNodeDataGridView.AllowUserToDeleteRows = false;
            this.directoryNodeDataGridView.AllowUserToOrderColumns = true;
            this.directoryNodeDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryNodeDataGridView.AutoGenerateColumns = false;
            this.directoryNodeDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.directoryNodeDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.directoryNodeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DirectoryType,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewCheckBoxColumn1});
            this.directoryNodeDataGridView.DataSource = this.directoryNodeListBindingSource;
            this.directoryNodeDataGridView.Location = new System.Drawing.Point(14, 132);
            this.directoryNodeDataGridView.Name = "directoryNodeDataGridView";
            this.directoryNodeDataGridView.ReadOnly = true;
            this.directoryNodeDataGridView.RowHeadersVisible = false;
            this.directoryNodeDataGridView.Size = new System.Drawing.Size(555, 270);
            this.directoryNodeDataGridView.TabIndex = 1;
            // 
            // directoryNodeListBindingSource
            // 
            this.directoryNodeListBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // filesSizeTextBox
            // 
            this.filesSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filesSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.filesSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filesSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "FilesSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.filesSizeTextBox.Location = new System.Drawing.Point(132, 88);
            this.filesSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.filesSizeTextBox.Name = "filesSizeTextBox";
            this.filesSizeTextBox.ReadOnly = true;
            this.filesSizeTextBox.Size = new System.Drawing.Size(193, 13);
            this.filesSizeTextBox.TabIndex = 6;
            // 
            // directoryNodeEntityBindingSource
            // 
            this.directoryNodeEntityBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Name", true));
            this.nameTextBox.Location = new System.Drawing.Point(63, 10);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(506, 13);
            this.nameTextBox.TabIndex = 8;
            // 
            // totalSizeTextBox
            // 
            this.totalSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.totalSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.totalSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totalSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "TotalSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.totalSizeTextBox.Location = new System.Drawing.Point(132, 62);
            this.totalSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.totalSizeTextBox.Name = "totalSizeTextBox";
            this.totalSizeTextBox.ReadOnly = true;
            this.totalSizeTextBox.Size = new System.Drawing.Size(193, 13);
            this.totalSizeTextBox.TabIndex = 10;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Path", true));
            this.pathTextBox.Location = new System.Drawing.Point(63, 36);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(506, 13);
            this.pathTextBox.TabIndex = 11;
            // 
            // DirectoryType
            // 
            this.DirectoryType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DirectoryType.Frozen = true;
            this.DirectoryType.HeaderText = "";
            this.DirectoryType.Name = "DirectoryType";
            this.DirectoryType.ReadOnly = true;
            this.DirectoryType.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.DirectoryType.Width = 30;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumn6.FillWeight = 300F;
            this.dataGridViewTextBoxColumn6.HeaderText = "Name";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TotalSize";
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.dataGridViewTextBoxColumn1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewTextBoxColumn1.FillWeight = 200F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Total Size";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "FilesSize";
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.dataGridViewTextBoxColumn2.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumn2.FillWeight = 200F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Files Size";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "DirectoryType";
            this.dataGridViewTextBoxColumn5.HeaderText = "Directory Type";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "ExistsUncalcSubDir";
            this.dataGridViewCheckBoxColumn1.HeaderText = "ExistsUncalcSubDir";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Visible = false;
            // 
            // DirectoryDetailUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(labelFolderContent);
            this.Controls.Add(pathLabel);
            this.Controls.Add(this.pathTextBox);
            this.Controls.Add(filesSizeLabel);
            this.Controls.Add(this.filesSizeTextBox);
            this.Controls.Add(nameLabel);
            this.Controls.Add(this.nameTextBox);
            this.Controls.Add(totalSizeLabel);
            this.Controls.Add(this.totalSizeTextBox);
            this.Controls.Add(this.directoryNodeDataGridView);
            this.Name = "DirectoryDetailUC";
            this.Size = new System.Drawing.Size(583, 415);
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView directoryNodeDataGridView;
        private System.Windows.Forms.TextBox filesSizeTextBox;
        private System.Windows.Forms.TextBox nameTextBox;
        private System.Windows.Forms.TextBox totalSizeTextBox;
        public System.Windows.Forms.BindingSource directoryNodeEntityBindingSource;
        public System.Windows.Forms.BindingSource directoryNodeListBindingSource;
        private System.Windows.Forms.TextBox pathTextBox;
        private System.Windows.Forms.DataGridViewImageColumn DirectoryType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
    }
}
