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
                IList<DirectoryNode> childrenListExceptHidenFreeSpace = ExcludeHidenFreeSpace(value.Children);
                directoryDetailUC1.directoryNodeListBindingSource.DataSource = new DirNodeSortableBindingList(childrenListExceptHidenFreeSpace);
                this.Text = String.Format(
                        HDGTools.resManager.GetString("DetailsForFolderFormTitle"),
                        directory.Name);
            }
        }

        private IList<DirectoryNode> ExcludeHidenFreeSpace(List<DirectoryNode> list)
        {
            List<DirectoryNode> resultList = new List<DirectoryNode>(list.Count);
            foreach (DirectoryNode node in list)
            {
                if (node.DirectoryType != SpecialDirTypes.FreeSpaceAndHide)
                    resultList.Add(node);
            }
            return resultList;
        }

    }
}