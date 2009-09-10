using System;
using System.Collections.Generic;
using System.Text;
using HDGraph.Interfaces.ScanEngines;
using System.Windows.Forms;

namespace HDGraph.Interfaces.DrawEngines
{
    
    public interface IDrawEngine
    {
        Control GenerateControlFromNode(IDirectoryNode node, DrawOptions options, IActionExecutor actionExecutor);

        IDirectoryNode GetRootNodeOfControl(Control control);
        void SetRootNodeOfControl(Control control, IDirectoryNode newRoot);
    }
}
