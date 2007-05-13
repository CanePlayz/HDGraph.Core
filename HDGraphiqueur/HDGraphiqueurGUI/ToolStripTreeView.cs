using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace HDGraph
{
    [ToolStripItemDesignerAvailability
               (ToolStripItemDesignerAvailability.ToolStrip |
               ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripTreeView : ToolStripControlHost
    {
        public ToolStripTreeView():base(new TreeView())
        {
        }

        public TreeView TreeViewControl
        {
            get
            {
                return Control as TreeView;
            }
        }
    }
}
