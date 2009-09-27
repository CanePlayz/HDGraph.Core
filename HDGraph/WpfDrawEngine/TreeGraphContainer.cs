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
        public IActionExecutor ActionExecutor
        {
            get { return GetTreeGraph().ActionExecutor; }
            set { GetTreeGraph().ActionExecutor = value; }
        }

        private bool alreadyLoaded;

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
                alreadyLoaded = true;
            }
        }

        private TreeGraph GetTreeGraph()
        {
            return (TreeGraph)elementHost1.Child;
        }

        private IDirectoryNode node;
        private DrawOptions options;

        public void SetRoot(IDirectoryNode node, DrawOptions options)
        {
            this.options = options;
            SetRoot(node);
        }

        public void SetRoot(IDirectoryNode node)
        {
            this.node = node;
            if (alreadyLoaded)
                UpdateVisual();
        }

        public IDirectoryNode GetRoot()
        {
            return node;
        }


        private void UpdateVisual()
        {
            GetTreeGraph().SetRoot(node, options);
        }

        public void SaveAsImageToFile(string filePath)
        {
            GetTreeGraph().SaveAsImageToFile(filePath);
        }
    }
}
