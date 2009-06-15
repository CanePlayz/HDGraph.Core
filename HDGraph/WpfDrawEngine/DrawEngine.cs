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

        public System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node)
        {
            TreeGraphContainer container = new TreeGraphContainer();
            return container;
        }

        #endregion
    }
}
