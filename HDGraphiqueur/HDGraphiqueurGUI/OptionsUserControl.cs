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
            UpdateDeletionCheckedStatus();
        }

        private Dictionary<string, Panel> panelsBuffer = new Dictionary<string, Panel>();

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void checkBoxAllowDeleteOption_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDeletionCheckedStatus();
        }

        private void UpdateDeletionCheckedStatus()
        {
            checkBoxDeletionAsk4Confirmation.Enabled = checkBoxAllowDeleteOption.Checked;
        }

        /// <summary>
        /// Sauvegarde les valeurs dans le fichier de config.
        /// </summary>
        public void SaveValues()
        {
            Properties.Settings.Default.Save();
        }
    }

    class OptionsUserControlDesigner : ControlDesigner
    {

        public override void Initialize(IComponent comp)
        {
            base.Initialize(comp);

        }
    }
}
