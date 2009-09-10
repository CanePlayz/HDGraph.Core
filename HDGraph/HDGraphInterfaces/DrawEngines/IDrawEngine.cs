using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.Interfaces.DrawEngines
{
    public interface IDrawEngine
    {
        event EventHandler<NodeContextEventArgs> ContextMenuRequired;
        
        System.Windows.Forms.Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options);
    }
}
