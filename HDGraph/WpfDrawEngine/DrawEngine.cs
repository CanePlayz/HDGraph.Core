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
            container.SetRoot(node, options);
            return container;
        }

        #endregion
    }
}
