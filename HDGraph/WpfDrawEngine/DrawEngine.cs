using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.WpfDrawEngine
{
    public class DrawEngine : IDrawEngine
    {

        #region IDrawEngine Members

        public System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options, IActionExecutor actionExecutor)
        {
            TreeGraphContainer container = new TreeGraphContainer();
            container.ActionExecutor = actionExecutor;
            container.ContextMenuRequired += new EventHandler<NodeContextEventArgs>(container_ContextMenuRequired);
            container.SetRoot(node, options);
            return container;
        }

        void container_ContextMenuRequired(object sender, NodeContextEventArgs e)
        {
            if (ContextMenuRequired != null)
                ContextMenuRequired(sender, e);
        }

        public event EventHandler<NodeContextEventArgs> ContextMenuRequired;

        public IDirectoryNode GetRootNodeOfControl(System.Windows.Forms.Control control)
        {
            if (control == null
                || !(control is TreeGraphContainer))
                throw new ArgumentException("The given control is null or is not a TreeGraphContainer.", "control");

            return ((TreeGraphContainer)control).GetRoot();
        }

        public void SetRootNodeOfControl(System.Windows.Forms.Control control, IDirectoryNode newRoot)
        {
            if (control == null
                || !(control is TreeGraphContainer))
                throw new ArgumentException("The given control is null or is not a TreeGraphContainer.", "control");

            ((TreeGraphContainer)control).SetRoot(newRoot);
        }

        #endregion
    }
}
