using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;

namespace HDGraph
{
    [Designer(typeof(OptionsUserControlDesigner))]
    public partial class OptionsUserControl : UserControl
    {
        public OptionsUserControl()
        {
            InitializeComponent();
        }

        public System.Windows.Forms.TreeNodeCollection Nodes
        {
            get { return treeView1.Nodes; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel PanelTab
        {
            get { return panel1; }
        }


        private Dictionary<string, Panel> panelsBuffer = new Dictionary<string, Panel>();

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }

    class OptionsUserControlDesigner : ControlDesigner
    {

        public override void Initialize(IComponent comp)
        {
            base.Initialize(comp);
            OptionsUserControl uc = (OptionsUserControl)comp;
            EnableDesignMode(uc.PanelTab, "PanelTab");
        }
    }
}
