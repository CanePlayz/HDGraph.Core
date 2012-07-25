using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Windows.Forms.Design;
using HDGraph.Resources;
using HDGraph.Interop;

namespace HDGraph
{
    [Designer(typeof(OptionsUserControlDesigner))]
    public partial class OptionsUserControl : UserControl
    {
        public OptionsUserControl()
        {
            InitializeComponent();
            UpdateDeletionCheckedStatus();
            SetUiFromEngineProperties();
        }

        private void SetUiFromEngineProperties()
        {
            radioButtonSimpleEngine.Checked = Properties.Settings.Default.OptionUseSimpleScanEngine || !ToolProviderBase.CurrentOsIsWindows();
            radioButtonNativeEngine.Checked = !radioButtonSimpleEngine.Checked;
            radioButtonNativeEngine.Enabled = ToolProviderBase.CurrentOsIsWindows();
        }

        private Dictionary<string, Panel> panelsBuffer = new Dictionary<string, Panel>();

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
        public DialogResult SaveValues()
        {
            if (Properties.Settings.Default.OptionUseSimpleScanEngine != radioButtonSimpleEngine.Checked)
            {

                DialogResult res = MessageBox.Show(ApplicationMessages.AppRestartRequiredToApplySetting,
                                                    "HDGraph",
                                                    MessageBoxButtons.YesNoCancel,
                                                    MessageBoxIcon.Question);
                if (res == DialogResult.Cancel)
                {
                    SetUiFromEngineProperties();
                    return DialogResult.Cancel;
                }
                else
                {
                    Properties.Settings.Default.OptionUseSimpleScanEngine = radioButtonSimpleEngine.Checked;
                    Properties.Settings.Default.Save();
                    if (res == DialogResult.Yes)
                        Application.Restart();
                    return DialogResult.OK;
                }
            }
            else
            {
                Properties.Settings.Default.Save();
                return DialogResult.OK;
            }
        }

        private void radioButtonNativeEngine_CheckedChanged(object sender, EventArgs e)
        {
            checkBoxIgnoreReparsePoints.Enabled = radioButtonNativeEngine.Checked;
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
