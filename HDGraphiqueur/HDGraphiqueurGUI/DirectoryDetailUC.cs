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
            dataGridViewTextBoxColumnTotalSize.Visible = true;
            dataGridViewTextBoxColumnTotalSizeHumanReadable.Visible = false;
            dataGridViewTextBoxColumnFilesSize.Visible = true;
            dataGridViewTextBoxColumnFilesSizeHumanReadable.Visible = false;
        }

        private void radioButtonHumandReadableSizes_CheckedChanged(object sender, EventArgs e)
        {
            dataGridViewTextBoxColumnTotalSize.Visible = false;
            dataGridViewTextBoxColumnTotalSizeHumanReadable.Visible = true;
            dataGridViewTextBoxColumnFilesSize.Visible = false;
            dataGridViewTextBoxColumnFilesSizeHumanReadable.Visible = true;
        }
    }
}
