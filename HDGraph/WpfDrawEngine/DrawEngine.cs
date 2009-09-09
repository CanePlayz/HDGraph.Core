using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.WpfDrawEngine
{
    public class DrawEngine : IControlTypeEngine
    {

        #region IControlTypeEngine Members

        public System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options)
        {
            TreeGraphContainer container = new TreeGraphContainer();
            container.ContextMenuRequired += new EventHandler<NodeContextEventArgs>(container_ContextMenuRequired);
            container.SetRoot(node, options);
            return container;
        }

        void container_ContextMenuRequired(object sender, NodeContextEventArgs e)
        {
            if (ContextMenuRequired != null)
                ContextMenuRequired(sender, e);
        }

        #endregion

        #region IControlTypeEngine Members


        public event EventHandler<NodeContextEventArgs> ContextMenuRequired;

        #endregion
    }
}
