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
            this.tabPageRightClic = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBoxAllowDeleteOption = new System.Windows.Forms.CheckBox();
            this.checkBoxDeletionAsk4Confirmation = new System.Windows.Forms.CheckBox();
            this.tabPageScanEngine = new System.Windows.Forms.TabPage();
            this.labelScanEngineDesc = new System.Windows.Forms.Label();
            this.groupBoxEngine = new System.Windows.Forms.GroupBox();
            this.radioButtonNativeEngine = new System.Windows.Forms.RadioButton();
            this.radioButtonSimpleEngine = new System.Windows.Forms.RadioButton();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageRightClic.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageScanEngine.SuspendLayout();
            this.groupBoxEngine.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPageRightClic
            // 
            this.tabPageRightClic.AccessibleDescription = null;
            this.tabPageRightClic.AccessibleName = null;
            resources.ApplyResources(this.tabPageRightClic, "tabPageRightClic");
            this.tabPageRightClic.BackgroundImage = null;
            this.tabPageRightClic.Controls.Add(this.groupBox2);
            this.tabPageRightClic.Controls.Add(this.groupBox1);
            this.tabPageRightClic.Font = null;
            this.tabPageRightClic.Name = "tabPageRightClic";
            this.tabPageRightClic.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.checkBox1);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // checkBox1
            // 
            this.checkBox1.AccessibleDescription = null;
            this.checkBox1.AccessibleName = null;
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.AutoEllipsis = true;
            this.checkBox1.BackgroundImage = null;
            this.checkBox1.Checked = global::HDGraph.Properties.Settings.Default.OptionCheckForNewVersionAfterStartup;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HDGraph.Properties.Settings.Default, "OptionCheckForNewVersionAfterStartup", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Font = null;
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.checkBoxAllowDeleteOption);
            this.groupBox1.Controls.Add(this.checkBoxDeletionAsk4Confirmation);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // checkBoxAllowDeleteOption
            // 
            this.checkBoxAllowDeleteOption.AccessibleDescription = null;
            this.checkBoxAllowDeleteOption.AccessibleName = null;
            resources.ApplyResources(this.checkBoxAllowDeleteOption, "checkBoxAllowDeleteOption");
            this.checkBoxAllowDeleteOption.BackgroundImage = null;
            this.checkBoxAllowDeleteOption.Checked = global::HDGraph.Properties.Settings.Default.OptionAllowFolderDeletion;
            this.checkBoxAllowDeleteOption.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HDGraph.Properties.Settings.Default, "OptionAllowFolderDeletion", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxAllowDeleteOption.Font = null;
            this.checkBoxAllowDeleteOption.Name = "checkBoxAllowDeleteOption";
            this.checkBoxAllowDeleteOption.UseVisualStyleBackColor = true;
            this.checkBoxAllowDeleteOption.CheckedChanged += new System.EventHandler(this.checkBoxAllowDeleteOption_CheckedChanged);
            // 
            // checkBoxDeletionAsk4Confirmation
            // 
            this.checkBoxDeletionAsk4Confirmation.AccessibleDescription = null;
            this.checkBoxDeletionAsk4Confirmation.AccessibleName = null;
            resources.ApplyResources(this.checkBoxDeletionAsk4Confirmation, "checkBoxDeletionAsk4Confirmation");
            this.checkBoxDeletionAsk4Confirmation.BackgroundImage = null;
            this.checkBoxDeletionAsk4Confirmation.Checked = global::HDGraph.Properties.Settings.Default.OptionDeletionAsk4Confirmation;
            this.checkBoxDeletionAsk4Confirmation.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxDeletionAsk4Confirmation.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::HDGraph.Properties.Settings.Default, "OptionDeletionAsk4Confirmation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBoxDeletionAsk4Confirmation.Font = null;
            this.checkBoxDeletionAsk4Confirmation.Name = "checkBoxDeletionAsk4Confirmation";
            this.checkBoxDeletionAsk4Confirmation.UseVisualStyleBackColor = true;
            // 
            // tabPageScanEngine
            // 
            this.tabPageScanEngine.AccessibleDescription = null;
            this.tabPageScanEngine.AccessibleName = null;
            resources.ApplyResources(this.tabPageScanEngine, "tabPageScanEngine");
            this.tabPageScanEngine.BackgroundImage = null;
            this.tabPageScanEngine.Controls.Add(this.labelScanEngineDesc);
            this.tabPageScanEngine.Controls.Add(this.groupBoxEngine);
            this.tabPageScanEngine.Font = null;
            this.tabPageScanEngine.Name = "tabPageScanEngine";
            this.tabPageScanEngine.UseVisualStyleBackColor = true;
            // 
            // labelScanEngineDesc
            // 
            this.labelScanEngineDesc.AccessibleDescription = null;
            this.labelScanEngineDesc.AccessibleName = null;
            resources.ApplyResources(this.labelScanEngineDesc, "labelScanEngineDesc");
            this.labelScanEngineDesc.Font = null;
            this.labelScanEngineDesc.Name = "labelScanEngineDesc";
            // 
            // groupBoxEngine
            // 
            this.groupBoxEngine.AccessibleDescription = null;
            this.groupBoxEngine.AccessibleName = null;
            resources.ApplyResources(this.groupBoxEngine, "groupBoxEngine");
            this.groupBoxEngine.BackgroundImage = null;
            this.groupBoxEngine.Controls.Add(this.radioButtonNativeEngine);
            this.groupBoxEngine.Controls.Add(this.radioButtonSimpleEngine);
            this.groupBoxEngine.Font = null;
            this.groupBoxEngine.Name = "groupBoxEngine";
            this.groupBoxEngine.TabStop = false;
            // 
            // radioButtonNativeEngine
            // 
            this.radioButtonNativeEngine.AccessibleDescription = null;
            this.radioButtonNativeEngine.AccessibleName = null;
            resources.ApplyResources(this.radioButtonNativeEngine, "radioButtonNativeEngine");
            this.radioButtonNativeEngine.BackgroundImage = null;
            this.radioButtonNativeEngine.Font = null;
            this.radioButtonNativeEngine.Name = "radioButtonNativeEngine";
            this.radioButtonNativeEngine.TabStop = true;
            this.radioButtonNativeEngine.UseVisualStyleBackColor = true;
            // 
            // radioButtonSimpleEngine
            // 
            this.radioButtonSimpleEngine.AccessibleDescription = null;
            this.radioButtonSimpleEngine.AccessibleName = null;
            resources.ApplyResources(this.radioButtonSimpleEngine, "radioButtonSimpleEngine");
            this.radioButtonSimpleEngine.BackgroundImage = null;
            this.radioButtonSimpleEngine.Font = null;
            this.radioButtonSimpleEngine.Name = "radioButtonSimpleEngine";
            this.radioButtonSimpleEngine.TabStop = true;
            this.radioButtonSimpleEngine.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.AccessibleDescription = null;
            this.tabControl1.AccessibleName = null;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.BackgroundImage = null;
            this.tabControl1.Controls.Add(this.tabPageScanEngine);
            this.tabControl1.Controls.Add(this.tabPageRightClic);
            this.tabControl1.Font = null;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // OptionsUserControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.tabControl1);
            this.Font = null;
            this.Name = "OptionsUserControl";
            this.tabPageRightClic.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageScanEngine.ResumeLayout(false);
            this.groupBoxEngine.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPageRightClic;
        private System.Windows.Forms.CheckBox checkBoxDeletionAsk4Confirmation;
        private System.Windows.Forms.CheckBox checkBoxAllowDeleteOption;
        private System.Windows.Forms.TabPage tabPageScanEngine;
        private System.Windows.Forms.Label labelScanEngineDesc;
        private System.Windows.Forms.GroupBox groupBoxEngine;
        private System.Windows.Forms.RadioButton radioButtonNativeEngine;
        private System.Windows.Forms.RadioButton radioButtonSimpleEngine;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;



    }
}
