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
            container.SetRoot(node, options);
            return container;
        }

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

        public void SaveAsImageToFile(System.Windows.Forms.Control control, string filePath)
        {
            if (control == null
                || !(control is TreeGraphContainer))
                throw new ArgumentException("The given control is null or is not a TreeGraphContainer.", "control");

            ((TreeGraphContainer)control).SaveAsImageToFile(filePath);
        }

        #endregion
    }
}
