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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.directoryNodeDataGridView = new System.Windows.Forms.DataGridView();
            this.filesSizeTextBox = new System.Windows.Forms.TextBox();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.totalSizeTextBox = new System.Windows.Forms.TextBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSizesInBytes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonHumandReadableSizes = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.directoryNodeEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumnName = new HDGraph.ExternalTools.TextAndImageColumn();
            this.dataGridViewTextBoxColumnTotalSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnFilesSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.directoryNodeListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textAndImageColumn1 = new HDGraph.ExternalTools.TextAndImageColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            filesSizeLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            totalSizeLabel = new System.Windows.Forms.Label();
            pathLabel = new System.Windows.Forms.Label();
            labelFolderContent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // filesSizeLabel
            // 
            filesSizeLabel.AutoSize = true;
            filesSizeLabel.Location = new System.Drawing.Point(14, 88);
            filesSizeLabel.Name = "filesSizeLabel";
            filesSizeLabel.Size = new System.Drawing.Size(57, 13);
            filesSizeLabel.TabIndex = 5;
            filesSizeLabel.Text = "Files Size :";
            // 
            // nameLabel
            // 
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(14, 10);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(41, 13);
            nameLabel.TabIndex = 7;
            nameLabel.Text = "Name :";
            // 
            // totalSizeLabel
            // 
            totalSizeLabel.AutoSize = true;
            totalSizeLabel.Location = new System.Drawing.Point(14, 62);
            totalSizeLabel.Name = "totalSizeLabel";
            totalSizeLabel.Size = new System.Drawing.Size(60, 13);
            totalSizeLabel.TabIndex = 9;
            totalSizeLabel.Text = "Total Size :";
            // 
            // pathLabel
            // 
            pathLabel.AutoSize = true;
            pathLabel.Location = new System.Drawing.Point(14, 36);
            pathLabel.Name = "pathLabel";
            pathLabel.Size = new System.Drawing.Size(35, 13);
            pathLabel.TabIndex = 10;
            pathLabel.Text = "Path :";
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
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.directoryNodeDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.directoryNodeDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryNodeDataGridView.AutoGenerateColumns = false;
            this.directoryNodeDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.directoryNodeDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.directoryNodeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumnName,
            this.dataGridViewTextBoxColumnTotalSize,
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable,
            this.dataGridViewTextBoxColumnFilesSize,
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn5});
            this.directoryNodeDataGridView.DataSource = this.directoryNodeListBindingSource;
            this.directoryNodeDataGridView.Location = new System.Drawing.Point(14, 132);
            this.directoryNodeDataGridView.Name = "directoryNodeDataGridView";
            this.directoryNodeDataGridView.ReadOnly = true;
            this.directoryNodeDataGridView.RowHeadersVisible = false;
            this.directoryNodeDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.directoryNodeDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.directoryNodeDataGridView.Size = new System.Drawing.Size(555, 270);
            this.directoryNodeDataGridView.TabIndex = 1;
            this.directoryNodeDataGridView.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.directoryNodeDataGridView_CellContextMenuStripNeeded);
            // 
            // filesSizeTextBox
            // 
            this.filesSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.filesSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.filesSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filesSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "FilesSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.filesSizeTextBox.Location = new System.Drawing.Point(80, 88);
            this.filesSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.filesSizeTextBox.Name = "filesSizeTextBox";
            this.filesSizeTextBox.ReadOnly = true;
            this.filesSizeTextBox.Size = new System.Drawing.Size(245, 13);
            this.filesSizeTextBox.TabIndex = 6;
            // 
            // nameTextBox
            // 
            this.nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.nameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Name", true));
            this.nameTextBox.Location = new System.Drawing.Point(80, 10);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            this.nameTextBox.Size = new System.Drawing.Size(489, 13);
            this.nameTextBox.TabIndex = 8;
            // 
            // totalSizeTextBox
            // 
            this.totalSizeTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.totalSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.totalSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totalSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "TotalSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.totalSizeTextBox.Location = new System.Drawing.Point(80, 62);
            this.totalSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.totalSizeTextBox.Name = "totalSizeTextBox";
            this.totalSizeTextBox.ReadOnly = true;
            this.totalSizeTextBox.Size = new System.Drawing.Size(245, 13);
            this.totalSizeTextBox.TabIndex = 10;
            // 
            // pathTextBox
            // 
            this.pathTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pathTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Path", true));
            this.pathTextBox.Location = new System.Drawing.Point(80, 36);
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            this.pathTextBox.Size = new System.Drawing.Size(489, 13);
            this.pathTextBox.TabIndex = 11;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.radioButtonSizesInBytes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonHumandReadableSizes);
            this.groupBox1.Location = new System.Drawing.Point(331, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(238, 64);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Size options";
            // 
            // radioButtonSizesInBytes
            // 
            this.radioButtonSizesInBytes.AutoSize = true;
            this.radioButtonSizesInBytes.Location = new System.Drawing.Point(92, 16);
            this.radioButtonSizesInBytes.Name = "radioButtonSizesInBytes";
            this.radioButtonSizesInBytes.Size = new System.Drawing.Size(51, 17);
            this.radioButtonSizesInBytes.TabIndex = 2;
            this.radioButtonSizesInBytes.Text = "Bytes";
            this.radioButtonSizesInBytes.UseVisualStyleBackColor = true;
            this.radioButtonSizesInBytes.CheckedChanged += new System.EventHandler(this.radioButtonSizesInBytes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Show sizes in : ";
            // 
            // radioButtonHumandReadableSizes
            // 
            this.radioButtonHumandReadableSizes.AutoSize = true;
            this.radioButtonHumandReadableSizes.Checked = true;
            this.radioButtonHumandReadableSizes.Location = new System.Drawing.Point(92, 39);
            this.radioButtonHumandReadableSizes.Name = "radioButtonHumandReadableSizes";
            this.radioButtonHumandReadableSizes.Size = new System.Drawing.Size(126, 17);
            this.radioButtonHumandReadableSizes.TabIndex = 0;
            this.radioButtonHumandReadableSizes.TabStop = true;
            this.radioButtonHumandReadableSizes.Text = "Human readable form";
            this.radioButtonHumandReadableSizes.UseVisualStyleBackColor = true;
            this.radioButtonHumandReadableSizes.CheckedChanged += new System.EventHandler(this.radioButtonHumandReadableSizes_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(278, 26);
            // 
            // openThisDirectoryInWindowsExplorerToolStripMenuItem
            // 
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Image = global::HDGraph.Properties.Resources.CascadeWindowsHS;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Name = "openThisDirectoryInWindowsExplorerToolStripMenuItem";
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Size = new System.Drawing.Size(277, 22);
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Text = "Open this directory in Windows Explorer";
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openThisDirectoryInWindowsExplorerToolStripMenuItem_Click);
            // 
            // directoryNodeEntityBindingSource
            // 
            this.directoryNodeEntityBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // dataGridViewTextBoxColumnName
            // 
            this.dataGridViewTextBoxColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnName.DataPropertyName = "Name";
            this.dataGridViewTextBoxColumnName.FillWeight = 300F;
            this.dataGridViewTextBoxColumnName.HeaderText = "Name";
            this.dataGridViewTextBoxColumnName.Image = null;
            this.dataGridViewTextBoxColumnName.Name = "dataGridViewTextBoxColumnName";
            this.dataGridViewTextBoxColumnName.ReadOnly = true;
            this.dataGridViewTextBoxColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumnTotalSize
            // 
            this.dataGridViewTextBoxColumnTotalSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnTotalSize.DataPropertyName = "TotalSize";
            dataGridViewCellStyle2.Format = "N0";
            dataGridViewCellStyle2.NullValue = null;
            this.dataGridViewTextBoxColumnTotalSize.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewTextBoxColumnTotalSize.FillWeight = 200F;
            this.dataGridViewTextBoxColumnTotalSize.HeaderText = "Total size (Bytes)";
            this.dataGridViewTextBoxColumnTotalSize.Name = "dataGridViewTextBoxColumnTotalSize";
            this.dataGridViewTextBoxColumnTotalSize.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnTotalSizeHumanReadable
            // 
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.DataPropertyName = "HumanReadableTotalSize";
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.FillWeight = 200F;
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.HeaderText = "Total size";
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.Name = "dataGridViewTextBoxColumnTotalSizeHumanReadable";
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.ReadOnly = true;
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.Visible = false;
            // 
            // dataGridViewTextBoxColumnFilesSize
            // 
            this.dataGridViewTextBoxColumnFilesSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnFilesSize.DataPropertyName = "FilesSize";
            dataGridViewCellStyle3.Format = "N0";
            dataGridViewCellStyle3.NullValue = null;
            this.dataGridViewTextBoxColumnFilesSize.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewTextBoxColumnFilesSize.FillWeight = 200F;
            this.dataGridViewTextBoxColumnFilesSize.HeaderText = "Files size (Bytes)";
            this.dataGridViewTextBoxColumnFilesSize.Name = "dataGridViewTextBoxColumnFilesSize";
            this.dataGridViewTextBoxColumnFilesSize.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnFilesSizeHumanReadable
            // 
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.DataPropertyName = "HumanReadableFilesSize";
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.FillWeight = 200F;
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.HeaderText = "Files size";
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.Name = "dataGridViewTextBoxColumnFilesSizeHumanReadable";
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.ReadOnly = true;
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.Visible = false;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "ExistsUncalcSubDir";
            this.dataGridViewCheckBoxColumn1.HeaderText = "ExistsUncalcSubDir";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "DirectoryType";
            this.dataGridViewTextBoxColumn5.HeaderText = "Directory Type";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // directoryNodeListBindingSource
            // 
            this.directoryNodeListBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "HumanReadableTotalSize";
            this.dataGridViewTextBoxColumn1.FillWeight = 200F;
            this.dataGridViewTextBoxColumn1.HeaderText = "Total Size (human readable)";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "FilesSizeHumanReadable";
            this.dataGridViewTextBoxColumn2.FillWeight = 200F;
            this.dataGridViewTextBoxColumn2.HeaderText = "Files size";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // textAndImageColumn1
            // 
            this.textAndImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textAndImageColumn1.DataPropertyName = "Name";
            this.textAndImageColumn1.FillWeight = 300F;
            this.textAndImageColumn1.HeaderText = "Name";
            this.textAndImageColumn1.Image = null;
            this.textAndImageColumn1.Name = "textAndImageColumn1";
            this.textAndImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "DirectoryType";
            this.dataGridViewTextBoxColumn3.HeaderText = "Directory Type";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.Visible = false;
            // 
            // DirectoryDetailUC
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBox1);
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
            this.Load += new System.EventHandler(this.DirectoryDetailUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).EndInit();
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
        private HDGraph.ExternalTools.TextAndImageColumn textAndImageColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButtonSizesInBytes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButtonHumandReadableSizes;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private HDGraph.ExternalTools.TextAndImageColumn dataGridViewTextBoxColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnTotalSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnTotalSizeHumanReadable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnFilesSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnFilesSizeHumanReadable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openThisDirectoryInWindowsExplorerToolStripMenuItem;
    }
}
