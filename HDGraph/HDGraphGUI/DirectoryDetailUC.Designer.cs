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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryDetailUC));
            System.Windows.Forms.Label nameLabel;
            System.Windows.Forms.Label totalSizeLabel;
            System.Windows.Forms.Label pathLabel;
            System.Windows.Forms.Label labelFolderContent;
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.directoryNodeDataGridView = new System.Windows.Forms.DataGridView();
            this.directoryNodeListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.filesSizeTextBox = new System.Windows.Forms.TextBox();
            this.directoryNodeEntityBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.totalSizeTextBox = new System.Windows.Forms.TextBox();
            this.pathTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButtonSizesInBytes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButtonHumandReadableSizes = new System.Windows.Forms.RadioButton();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showDetailsInANewWindowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textAndImageColumn1 = new HDGraph.ExternalTools.TextAndImageColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextImageColumnName = new HDGraph.ExternalTools.TextAndImageColumn();
            this.dataGridViewTextBoxColumnTotalSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnFilesSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            filesSizeLabel = new System.Windows.Forms.Label();
            nameLabel = new System.Windows.Forms.Label();
            totalSizeLabel = new System.Windows.Forms.Label();
            pathLabel = new System.Windows.Forms.Label();
            labelFolderContent = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // filesSizeLabel
            // 
            filesSizeLabel.AccessibleDescription = null;
            filesSizeLabel.AccessibleName = null;
            resources.ApplyResources(filesSizeLabel, "filesSizeLabel");
            filesSizeLabel.Font = null;
            filesSizeLabel.Name = "filesSizeLabel";
            // 
            // nameLabel
            // 
            nameLabel.AccessibleDescription = null;
            nameLabel.AccessibleName = null;
            resources.ApplyResources(nameLabel, "nameLabel");
            nameLabel.Font = null;
            nameLabel.Name = "nameLabel";
            // 
            // totalSizeLabel
            // 
            totalSizeLabel.AccessibleDescription = null;
            totalSizeLabel.AccessibleName = null;
            resources.ApplyResources(totalSizeLabel, "totalSizeLabel");
            totalSizeLabel.Font = null;
            totalSizeLabel.Name = "totalSizeLabel";
            // 
            // pathLabel
            // 
            pathLabel.AccessibleDescription = null;
            pathLabel.AccessibleName = null;
            resources.ApplyResources(pathLabel, "pathLabel");
            pathLabel.Font = null;
            pathLabel.Name = "pathLabel";
            // 
            // labelFolderContent
            // 
            labelFolderContent.AccessibleDescription = null;
            labelFolderContent.AccessibleName = null;
            resources.ApplyResources(labelFolderContent, "labelFolderContent");
            labelFolderContent.Font = null;
            labelFolderContent.Name = "labelFolderContent";
            // 
            // directoryNodeDataGridView
            // 
            this.directoryNodeDataGridView.AccessibleDescription = null;
            this.directoryNodeDataGridView.AccessibleName = null;
            this.directoryNodeDataGridView.AllowUserToAddRows = false;
            this.directoryNodeDataGridView.AllowUserToDeleteRows = false;
            this.directoryNodeDataGridView.AllowUserToOrderColumns = true;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.BlanchedAlmond;
            this.directoryNodeDataGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.directoryNodeDataGridView, "directoryNodeDataGridView");
            this.directoryNodeDataGridView.AutoGenerateColumns = false;
            this.directoryNodeDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.directoryNodeDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.directoryNodeDataGridView.BackgroundImage = null;
            this.directoryNodeDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextImageColumnName,
            this.dataGridViewTextBoxColumnTotalSize,
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable,
            this.dataGridViewTextBoxColumnFilesSize,
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn5});
            this.directoryNodeDataGridView.DataSource = this.directoryNodeListBindingSource;
            this.directoryNodeDataGridView.Font = null;
            this.directoryNodeDataGridView.Name = "directoryNodeDataGridView";
            this.directoryNodeDataGridView.ReadOnly = true;
            this.directoryNodeDataGridView.RowHeadersVisible = false;
            this.directoryNodeDataGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.directoryNodeDataGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.directoryNodeDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.directoryNodeDataGridView.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.directoryNodeDataGridView_RowsAdded);
            this.directoryNodeDataGridView.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.directoryNodeDataGridView_CellContextMenuStripNeeded);
            this.directoryNodeDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.directoryNodeDataGridView_CellFormatting);
            // 
            // directoryNodeListBindingSource
            // 
            this.directoryNodeListBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // filesSizeTextBox
            // 
            this.filesSizeTextBox.AccessibleDescription = null;
            this.filesSizeTextBox.AccessibleName = null;
            resources.ApplyResources(this.filesSizeTextBox, "filesSizeTextBox");
            this.filesSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.filesSizeTextBox.BackgroundImage = null;
            this.filesSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.filesSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "FilesSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.filesSizeTextBox.Font = null;
            this.filesSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.filesSizeTextBox.Name = "filesSizeTextBox";
            this.filesSizeTextBox.ReadOnly = true;
            // 
            // directoryNodeEntityBindingSource
            // 
            this.directoryNodeEntityBindingSource.DataSource = typeof(HDGraph.DirectoryNode);
            // 
            // nameTextBox
            // 
            this.nameTextBox.AccessibleDescription = null;
            this.nameTextBox.AccessibleName = null;
            resources.ApplyResources(this.nameTextBox, "nameTextBox");
            this.nameTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.nameTextBox.BackgroundImage = null;
            this.nameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nameTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Name", true));
            this.nameTextBox.Font = null;
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.ReadOnly = true;
            // 
            // totalSizeTextBox
            // 
            this.totalSizeTextBox.AccessibleDescription = null;
            this.totalSizeTextBox.AccessibleName = null;
            resources.ApplyResources(this.totalSizeTextBox, "totalSizeTextBox");
            this.totalSizeTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.totalSizeTextBox.BackgroundImage = null;
            this.totalSizeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.totalSizeTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "TotalSize", true, System.Windows.Forms.DataSourceUpdateMode.OnValidation, null, "N0"));
            this.totalSizeTextBox.Font = null;
            this.totalSizeTextBox.MinimumSize = new System.Drawing.Size(187, 0);
            this.totalSizeTextBox.Name = "totalSizeTextBox";
            this.totalSizeTextBox.ReadOnly = true;
            // 
            // pathTextBox
            // 
            this.pathTextBox.AccessibleDescription = null;
            this.pathTextBox.AccessibleName = null;
            resources.ApplyResources(this.pathTextBox, "pathTextBox");
            this.pathTextBox.BackColor = System.Drawing.SystemColors.Control;
            this.pathTextBox.BackgroundImage = null;
            this.pathTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pathTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.directoryNodeEntityBindingSource, "Path", true));
            this.pathTextBox.Font = null;
            this.pathTextBox.Name = "pathTextBox";
            this.pathTextBox.ReadOnly = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.radioButtonSizesInBytes);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.radioButtonHumandReadableSizes);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // radioButtonSizesInBytes
            // 
            this.radioButtonSizesInBytes.AccessibleDescription = null;
            this.radioButtonSizesInBytes.AccessibleName = null;
            resources.ApplyResources(this.radioButtonSizesInBytes, "radioButtonSizesInBytes");
            this.radioButtonSizesInBytes.BackgroundImage = null;
            this.radioButtonSizesInBytes.Font = null;
            this.radioButtonSizesInBytes.Name = "radioButtonSizesInBytes";
            this.radioButtonSizesInBytes.UseVisualStyleBackColor = true;
            this.radioButtonSizesInBytes.CheckedChanged += new System.EventHandler(this.radioButtonSizesInBytes_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // radioButtonHumandReadableSizes
            // 
            this.radioButtonHumandReadableSizes.AccessibleDescription = null;
            this.radioButtonHumandReadableSizes.AccessibleName = null;
            resources.ApplyResources(this.radioButtonHumandReadableSizes, "radioButtonHumandReadableSizes");
            this.radioButtonHumandReadableSizes.BackgroundImage = null;
            this.radioButtonHumandReadableSizes.Checked = true;
            this.radioButtonHumandReadableSizes.Font = null;
            this.radioButtonHumandReadableSizes.Name = "radioButtonHumandReadableSizes";
            this.radioButtonHumandReadableSizes.TabStop = true;
            this.radioButtonHumandReadableSizes.UseVisualStyleBackColor = true;
            this.radioButtonHumandReadableSizes.CheckedChanged += new System.EventHandler(this.radioButtonHumandReadableSizes_CheckedChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleDescription = null;
            this.contextMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackgroundImage = null;
            this.contextMenuStrip1.Font = null;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showDetailsInANewWindowToolStripMenuItem,
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // showDetailsInANewWindowToolStripMenuItem
            // 
            this.showDetailsInANewWindowToolStripMenuItem.AccessibleDescription = null;
            this.showDetailsInANewWindowToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.showDetailsInANewWindowToolStripMenuItem, "showDetailsInANewWindowToolStripMenuItem");
            this.showDetailsInANewWindowToolStripMenuItem.BackgroundImage = null;
            this.showDetailsInANewWindowToolStripMenuItem.Image = global::HDGraph.Properties.Resources.ActualSizeHS;
            this.showDetailsInANewWindowToolStripMenuItem.Name = "showDetailsInANewWindowToolStripMenuItem";
            this.showDetailsInANewWindowToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.showDetailsInANewWindowToolStripMenuItem.Click += new System.EventHandler(this.showDetailsInANewWindowToolStripMenuItem_Click);
            // 
            // openThisDirectoryInWindowsExplorerToolStripMenuItem
            // 
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleDescription = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openThisDirectoryInWindowsExplorerToolStripMenuItem, "openThisDirectoryInWindowsExplorerToolStripMenuItem");
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.BackgroundImage = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Image = global::HDGraph.Properties.Resources.CascadeWindowsHS;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Name = "openThisDirectoryInWindowsExplorerToolStripMenuItem";
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openThisDirectoryInWindowsExplorerToolStripMenuItem_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "HumanReadableTotalSize";
            this.dataGridViewTextBoxColumn1.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "FilesSizeHumanReadable";
            this.dataGridViewTextBoxColumn2.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // textAndImageColumn1
            // 
            this.textAndImageColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.textAndImageColumn1.DataPropertyName = "Name";
            this.textAndImageColumn1.FillWeight = 300F;
            resources.ApplyResources(this.textAndImageColumn1, "textAndImageColumn1");
            this.textAndImageColumn1.Image = null;
            this.textAndImageColumn1.Name = "textAndImageColumn1";
            this.textAndImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "DirectoryType";
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextImageColumnName
            // 
            this.dataGridViewTextImageColumnName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextImageColumnName.DataPropertyName = "Name";
            this.dataGridViewTextImageColumnName.FillWeight = 300F;
            resources.ApplyResources(this.dataGridViewTextImageColumnName, "dataGridViewTextImageColumnName");
            this.dataGridViewTextImageColumnName.Image = null;
            this.dataGridViewTextImageColumnName.Name = "dataGridViewTextImageColumnName";
            this.dataGridViewTextImageColumnName.ReadOnly = true;
            this.dataGridViewTextImageColumnName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumnTotalSize
            // 
            this.dataGridViewTextBoxColumnTotalSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnTotalSize.DataPropertyName = "TotalSize";
            dataGridViewCellStyle8.Format = "N0";
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridViewTextBoxColumnTotalSize.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumnTotalSize.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumnTotalSize, "dataGridViewTextBoxColumnTotalSize");
            this.dataGridViewTextBoxColumnTotalSize.Name = "dataGridViewTextBoxColumnTotalSize";
            this.dataGridViewTextBoxColumnTotalSize.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnTotalSizeHumanReadable
            // 
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.DataPropertyName = "HumanReadableTotalSize";
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumnTotalSizeHumanReadable, "dataGridViewTextBoxColumnTotalSizeHumanReadable");
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.Name = "dataGridViewTextBoxColumnTotalSizeHumanReadable";
            this.dataGridViewTextBoxColumnTotalSizeHumanReadable.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnFilesSize
            // 
            this.dataGridViewTextBoxColumnFilesSize.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnFilesSize.DataPropertyName = "FilesSize";
            dataGridViewCellStyle9.Format = "N0";
            dataGridViewCellStyle9.NullValue = null;
            this.dataGridViewTextBoxColumnFilesSize.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumnFilesSize.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumnFilesSize, "dataGridViewTextBoxColumnFilesSize");
            this.dataGridViewTextBoxColumnFilesSize.Name = "dataGridViewTextBoxColumnFilesSize";
            this.dataGridViewTextBoxColumnFilesSize.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumnFilesSizeHumanReadable
            // 
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.DataPropertyName = "HumanReadableFilesSize";
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.FillWeight = 200F;
            resources.ApplyResources(this.dataGridViewTextBoxColumnFilesSizeHumanReadable, "dataGridViewTextBoxColumnFilesSizeHumanReadable");
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.Name = "dataGridViewTextBoxColumnFilesSizeHumanReadable";
            this.dataGridViewTextBoxColumnFilesSizeHumanReadable.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "ExistsUncalcSubDir";
            resources.ApplyResources(this.dataGridViewCheckBoxColumn1, "dataGridViewCheckBoxColumn1");
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "DirectoryType";
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // DirectoryDetailUC
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BackgroundImage = null;
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
            this.Font = null;
            this.Name = "DirectoryDetailUC";
            this.Load += new System.EventHandler(this.DirectoryDetailUC_Load);
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.directoryNodeEntityBindingSource)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem openThisDirectoryInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showDetailsInANewWindowToolStripMenuItem;
        private HDGraph.ExternalTools.TextAndImageColumn dataGridViewTextImageColumnName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnTotalSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnTotalSizeHumanReadable;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnFilesSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumnFilesSizeHumanReadable;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
