namespace HDGraph
{
    partial class OptionsUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsUserControl));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageRightClic = new System.Windows.Forms.TabPage();
            this.checkBoxDeletionAsk4Confirmation = new System.Windows.Forms.CheckBox();
            this.checkBoxAllowDeleteOption = new System.Windows.Forms.CheckBox();
            this.tabPageScanEngine = new System.Windows.Forms.TabPage();
            this.groupBoxEngine = new System.Windows.Forms.GroupBox();
            this.radioButtonNativeEngine = new System.Windows.Forms.RadioButton();
            this.radioButtonSimpleEngine = new System.Windows.Forms.RadioButton();
            this.tabControl1.SuspendLayout();
            this.tabPageRightClic.SuspendLayout();
            this.tabPageScanEngine.SuspendLayout();
            this.groupBoxEngine.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageRightClic);
            this.tabControl1.Controls.Add(this.tabPageScanEngine);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPageRightClic
            // 
            this.tabPageRightClic.Controls.Add(this.checkBoxDeletionAsk4Confirmation);
            this.tabPageRightClic.Controls.Add(this.checkBoxAllowDeleteOption);
            resources.ApplyResources(this.tabPageRightClic, "tabPageRightClic");
            this.tabPageRightClic.Name = "tabPageRightClic";
            this.tabPageRightClic.UseVisualStyleBackColor = true;
            // 
            // checkBoxDeletionAsk4Confirmation
            // 
            resources.ApplyResources(this.checkBoxDeletionAsk4Confirmation, "checkBoxDeletionAsk4Confirmation");
            this.checkBoxDeletionAsk4Confirmation.Checked = global::HDGraph.Properties.Settings.Default.OptionDeletionAsk4Confirmation;
            this.checkBoxDeletionAsk4Confirmation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeletionAsk4Confirmation.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HDGraph.Properties.Settings.Default, "OptionDeletionAsk4Confirmation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxDeletionAsk4Confirmation.Name = "checkBoxDeletionAsk4Confirmation";
            this.checkBoxDeletionAsk4Confirmation.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowDeleteOption
            // 
            resources.ApplyResources(this.checkBoxAllowDeleteOption, "checkBoxAllowDeleteOption");
            this.checkBoxAllowDeleteOption.Checked = global::HDGraph.Properties.Settings.Default.OptionAllowFolderDeletion;
            this.checkBoxAllowDeleteOption.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HDGraph.Properties.Settings.Default, "OptionAllowFolderDeletion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxAllowDeleteOption.Name = "checkBoxAllowDeleteOption";
            this.checkBoxAllowDeleteOption.UseVisualStyleBackColor = true;
            this.checkBoxAllowDeleteOption.CheckedChanged += new System.EventHandler(this.checkBoxAllowDeleteOption_CheckedChanged);
            // 
            // tabPageScanEngine
            // 
            this.tabPageScanEngine.Controls.Add(this.groupBoxEngine);
            resources.ApplyResources(this.tabPageScanEngine, "tabPageScanEngine");
            this.tabPageScanEngine.Name = "tabPageScanEngine";
            this.tabPageScanEngine.UseVisualStyleBackColor = true;
            // 
            // groupBoxEngine
            // 
            resources.ApplyResources(this.groupBoxEngine, "groupBoxEngine");
            this.groupBoxEngine.Controls.Add(this.radioButtonNativeEngine);
            this.groupBoxEngine.Controls.Add(this.radioButtonSimpleEngine);
            this.groupBoxEngine.Name = "groupBoxEngine";
            this.groupBoxEngine.TabStop = false;
            // 
            // radioButtonNativeEngine
            // 
            resources.ApplyResources(this.radioButtonNativeEngine, "radioButtonNativeEngine");
            this.radioButtonNativeEngine.Name = "radioButtonNativeEngine";
            this.radioButtonNativeEngine.TabStop = true;
            this.radioButtonNativeEngine.UseVisualStyleBackColor = true;
            // 
            // radioButtonSimpleEngine
            // 
            resources.ApplyResources(this.radioButtonSimpleEngine, "radioButtonSimpleEngine");
            this.radioButtonSimpleEngine.Name = "radioButtonSimpleEngine";
            this.radioButtonSimpleEngine.TabStop = true;
            this.radioButtonSimpleEngine.UseVisualStyleBackColor = true;
            // 
            // OptionsUserControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "OptionsUserControl";
            this.tabControl1.ResumeLayout(false);
            this.tabPageRightClic.ResumeLayout(false);
            this.tabPageRightClic.PerformLayout();
            this.tabPageScanEngine.ResumeLayout(false);
            this.groupBoxEngine.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageRightClic;
        private System.Windows.Forms.CheckBox checkBoxAllowDeleteOption;
        private System.Windows.Forms.CheckBox checkBoxDeletionAsk4Confirmation;
        private System.Windows.Forms.TabPage tabPageScanEngine;
        private System.Windows.Forms.GroupBox groupBoxEngine;
        private System.Windows.Forms.RadioButton radioButtonNativeEngine;
        private System.Windows.Forms.RadioButton radioButtonSimpleEngine;


    }
}
