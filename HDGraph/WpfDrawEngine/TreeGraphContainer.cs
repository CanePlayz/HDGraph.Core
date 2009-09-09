using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.WpfDrawEngine
{
    public partial class TreeGraphContainer : UserControl
    {
        public TreeGraphContainer()
        {
            InitializeComponent();
            this.Load += new EventHandler(TreeGraphContainer_Load);
        }

        void TreeGraphContainer_Load(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                GetTreeGraph().SetRoot(node, options);
            }
        }

        private TreeGraph GetTreeGraph()
        {
            return (TreeGraph)elementHost1.Child;
        }

        IDirectoryNode node;
        DrawOptions options;

        public void SetRoot(IDirectoryNode node, DrawOptions options)
        {
            this.node = node;
            this.options = options;
        }

        public event EventHandler<NodeContextEventArgs> ContextMenuRequired
        {
            add
            {
                if (!this.DesignMode)
                {
                    TreeGraph treeGraph = GetTreeGraph();
                    if (treeGraph != null)
                        treeGraph.ContextMenuRequired += value;
                }
            }
            remove
            {
                if (!this.DesignMode)
                {
                    TreeGraph treeGraph = GetTreeGraph();
                    if (treeGraph != null)
                        treeGraph.ContextMenuRequired -= value;
                }
            }
        }
    }
}
