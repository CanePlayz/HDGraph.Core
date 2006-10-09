using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class OptionsForm : Form
    {
        public OptionsForm()
        {
            InitializeComponent();
        }

        //Dictionary<string, TabPage> tabPagesCache;

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            //tabPagesCache = new Dictionary<string, TabPage>();
            //foreach (TabPage tab in tabControl1.TabPages)
            //{
            //    tabPagesCache.Add(tab.Name, tab);
            //}

            //splitContainer1.Panel2.Controls.Remove(tabControl1);

            //treeView1.SelectedNode = treeView1.Nodes[0];
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //if (treeView1.SelectedNode == null)
            //    return;

            //tabControl1.TabPages.Clear();
            //splitContainer1.Panel2.Controls.Clear();
            //TabPage tab = tabPagesCache[treeView1.SelectedNode.Name + "Tab"];
            //tab.Text = treeView1.SelectedNode.Text;
            ////splitContainer1.Panel2.Controls.Add);

            //tabControl1.TabPages.Add(tab);

        }

    }
}