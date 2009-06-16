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
                ((TreeGraph)elementHost1.Child).SetRoot(node, options);
            }
        }

        IDirectoryNode node;
        DrawOptions options;

        public void SetRoot(IDirectoryNode node, DrawOptions options)
        {
            this.node = node;
            this.options = options;
        }
    }
}
