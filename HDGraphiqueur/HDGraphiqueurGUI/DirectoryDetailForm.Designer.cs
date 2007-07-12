namespace HDGraph
{
    partial class DirectoryDetailForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DirectoryDetailForm));
            this.directoryDetailUC1 = new HDGraph.DirectoryDetailUC();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // directoryDetailUC1
            // 
            this.directoryDetailUC1.AccessibleDescription = null;
            this.directoryDetailUC1.AccessibleName = null;
            resources.ApplyResources(this.directoryDetailUC1, "directoryDetailUC1");
            this.directoryDetailUC1.BackColor = System.Drawing.SystemColors.Control;
            this.directoryDetailUC1.BackgroundImage = null;
            this.directoryDetailUC1.Font = null;
            this.directoryDetailUC1.Name = "directoryDetailUC1";
            // 
            // buttonClose
            // 
            this.buttonClose.AccessibleDescription = null;
            this.buttonClose.AccessibleName = null;
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.BackgroundImage = null;
            this.buttonClose.Font = null;
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // DirectoryDetailForm
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.directoryDetailUC1);
            this.Font = null;
            this.Icon = null;
            this.Name = "DirectoryDetailForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.ResumeLayout(false);

        }

        #endregion

        private DirectoryDetailUC directoryDetailUC1;
        private System.Windows.Forms.Button buttonClose;
    }
}