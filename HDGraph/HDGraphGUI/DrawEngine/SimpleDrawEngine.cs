using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.DrawEngine
{
    public class SimpleDrawEngine : IDrawEngine
    {
        TreeGraph treeGraph1;

        #region IDrawEngine Members

        public event EventHandler<NodeContextEventArgs> ContextMenuRequired;

        public System.Windows.Forms.Control GenerateControlFromNode(HDGraph.Interfaces.ScanEngines.IDirectoryNode node, DrawOptions options)
        {
            // 
            // treeGraph1
            // 
            TreeGraph treeGraph1 = new TreeGraph();
            treeGraph1.BackColor = System.Drawing.Color.White;
            treeGraph1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            treeGraph1.DrawOptions = options;
            treeGraph1.DrawType = HDGraph.DrawEngine.DrawType.Circular;
            treeGraph1.MinimumSize = new System.Drawing.Size(50, 50);
            treeGraph1.Name = "treeGraph1";
            treeGraph1.Root = node;
            return treeGraph1;
        }

        #endregion
    }
}
