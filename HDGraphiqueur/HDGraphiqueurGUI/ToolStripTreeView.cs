using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using WilsonProgramming;

namespace HDGraph
{
    //[ToolStripItemDesignerAvailability
    //           (ToolStripItemDesignerAvailability.ToolStrip |
    //           ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripTreeView : ToolStripControlHost
    {
        public ToolStripTreeView()
            : base(new ComboBox())
        {
        }

        //public ExplorerTreeView TreeViewControl
        //{
        //    get
        //    {
        //        return Control as ExplorerTreeView;
        //    }
        //}
    }
}
