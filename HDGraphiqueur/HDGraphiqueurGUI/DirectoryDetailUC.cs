using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class DirectoryDetailUC : UserControl
    {
        public DirectoryDetailUC()
        {
            InitializeComponent();
            //this.dataGridViewImageColumn1.ValuesAreIcons = true;

            this.dataGridViewTextBoxColumnName.Image = ExternalTools.IconReader.GetFolderIcon(
                                                        HDGraph.ExternalTools.IconReader.IconSize.Small,
                                                        HDGraph.ExternalTools.IconReader.FolderType.Open).ToBitmap();
        }

        private void radioButtonSizesInBytes_CheckedChanged(object sender, EventArgs e)
        {
            ShowSizesInBytes();
            // Save choice in the config file
            Properties.Settings.Default.OptionShowSizesInHumanForm = false;
            Properties.Settings.Default.Save();
        }

        private void radioButtonHumandReadableSizes_CheckedChanged(object sender, EventArgs e)
        {
            ShowSizesInHumanReadableStyle();
            // Save choice in the config file
            Properties.Settings.Default.OptionShowSizesInHumanForm = true;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// Update the UserControl style to show sizes in bytes.
        /// </summary>
        private void ShowSizesInBytes()
        {
            // Update DataGridView style
            dataGridViewTextBoxColumnTotalSize.Visible = true;
            dataGridViewTextBoxColumnTotalSizeHumanReadable.Visible = false;
            dataGridViewTextBoxColumnFilesSize.Visible = true;
            dataGridViewTextBoxColumnFilesSizeHumanReadable.Visible = false;
            // Update main folder size TextBox styles
            ChangePropertyNameOfBinding(totalSizeTextBox, "TotalSize", "N0");
            ChangePropertyNameOfBinding(filesSizeTextBox, "FilesSize", "N0");
        }

        /// <summary>
        /// Update the UserControl style to show sizes in human readable style.
        /// </summary>
        private void ShowSizesInHumanReadableStyle()
        {
            // Update DataGridView style
            dataGridViewTextBoxColumnTotalSize.Visible = false;
            dataGridViewTextBoxColumnTotalSizeHumanReadable.Visible = true;
            dataGridViewTextBoxColumnFilesSize.Visible = false;
            dataGridViewTextBoxColumnFilesSizeHumanReadable.Visible = true;
            // Update main folder size TextBox styles
            ChangePropertyNameOfBinding(totalSizeTextBox, "HumanReadableTotalSize", "");
            ChangePropertyNameOfBinding(filesSizeTextBox, "HumanReadableFilesSize", "");
        }

        /// <summary>
        /// Change ne property name of a control binding.
        /// </summary>
        /// <param name="textBox"></param>
        /// <param name="newPropertyName"></param>
        private void ChangePropertyNameOfBinding(Control control, string newPropertyName, string format)
        {
            control.DataBindings.Clear();
            control.DataBindings.Add(
                new System.Windows.Forms.Binding("Text",
                    this.directoryNodeEntityBindingSource,
                    newPropertyName,
                    true,
                    System.Windows.Forms.DataSourceUpdateMode.OnValidation,
                    null,
                    format));
        }

        /// <summary>
        /// UC load.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DirectoryDetailUC_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.OptionShowSizesInHumanForm)
            {
                radioButtonHumandReadableSizes.Checked = true;
                ShowSizesInHumanReadableStyle();
            }
            else
            {
                radioButtonSizesInBytes.Checked = true;
                ShowSizesInBytes();
            }

        }

        private void openThisDirectoryInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedNode != null)
                System.Diagnostics.Process.Start(selectedNode.Path);
        }

        private DirectoryNode selectedNode = null;

        private void directoryNodeDataGridView_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            e.ContextMenuStrip = contextMenuStrip1;
            directoryNodeDataGridView.ClearSelection();
            directoryNodeDataGridView.Rows[e.RowIndex].Selected = true;
            selectedNode = directoryNodeDataGridView.Rows[e.RowIndex].DataBoundItem as DirectoryNode;
        }
    }
}
