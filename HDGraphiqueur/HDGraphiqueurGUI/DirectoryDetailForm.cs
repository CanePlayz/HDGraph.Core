using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class DirectoryDetailForm : Form
    {
        public DirectoryDetailForm()
        {
            InitializeComponent();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private DirectoryNode directory;

        public DirectoryNode Directory
        {
            get { return directory; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException();
                directory = value;
                directoryDetailUC1.directoryNodeEntityBindingSource.DataSource = value;
                directoryDetailUC1.directoryNodeListBindingSource.DataSource = new DirNodeSortableBindingList(value.Children);
                this.Text = String.Format(
                        HDGTools.resManager.GetString("DetailsForFolderFormTitle"),
                        directory.Name);
            }
        }

    }
}