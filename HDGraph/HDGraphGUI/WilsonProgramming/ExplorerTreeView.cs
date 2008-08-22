using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Windows.Forms.Design;

namespace WilsonProgramming
{
    [ToolStripItemDesignerAvailability
           (ToolStripItemDesignerAvailability.ToolStrip |
           ToolStripItemDesignerAvailability.StatusStrip)]
    public class ExplorerTreeView : ToolStripControlHost
    {
        public ExplorerTreeView():base(new WilsonProgramming.ExplorerTreeViewWnd())
        {
            // 
            // treeWnd
            // 
            TreeViewWnd.Size = new System.Drawing.Size(289, 454);
            // Set the TreeView image list to the system image list.
            SystemImageList.SetTVImageList(TreeViewWnd.Handle);
            LoadRootNodes();
        }

        ///
        /// TreeView window in this user control.
        ///
        public ExplorerTreeViewWnd TreeViewWnd
        {
            get { return this.Control as ExplorerTreeViewWnd; }
        }


        /// <summary>
        /// Loads the root TreeView nodes.
        /// </summary>
        private void LoadRootNodes()
        {
            // Create the root shell item.
            ShellItem m_shDesktop = ShellItem.GetRootShellItem();

            // Create the root node.
            TreeNode tvwRoot = new TreeNode();
            tvwRoot.Text = m_shDesktop.DisplayName;
            tvwRoot.ImageIndex = m_shDesktop.IconIndex;
            tvwRoot.SelectedImageIndex = m_shDesktop.IconIndex;
            tvwRoot.Tag = m_shDesktop;

            // Now we need to add any children to the root node.
            ArrayList arrChildren = m_shDesktop.GetSubFolders();
            foreach (ShellItem shChild in arrChildren)
            {
                TreeNode tvwChild = new TreeNode();
                tvwChild.Text = shChild.DisplayName;
                tvwChild.ImageIndex = shChild.IconIndex;
                tvwChild.SelectedImageIndex = shChild.IconIndex;
                tvwChild.Tag = shChild;

                // If this is a folder item and has children then add a place holder node.
                if (shChild.IsFolder && shChild.HasSubFolder)
                    tvwChild.Nodes.Add("PH");
                tvwRoot.Nodes.Add(tvwChild);
            }

            // Add the root node to the tree.
            TreeViewWnd.Nodes.Clear();
            TreeViewWnd.Nodes.Add(tvwRoot);
            tvwRoot.Expand();
        }
    }
}
