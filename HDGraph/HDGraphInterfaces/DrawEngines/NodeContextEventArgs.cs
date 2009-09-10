using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using System.Drawing;

namespace HDGraph.Interfaces.DrawEngines
{
    public class NodeContextEventArgs : EventArgs
    {
        public IDirectoryNode Node { get; set; }

        public PointF Position { get; set; }
    }
}
