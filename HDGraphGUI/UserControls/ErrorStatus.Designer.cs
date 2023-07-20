namespace HDGraph.UserControls
{
    partial class ErrorStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ErrorStatus));
            this.labelErrors = new System.Windows.Forms.Label();
            this.linkLabelDetails = new System.Windows.Forms.LinkLabel();
            this.blinkingImage1 = new HDGraph.UserControls.BlinkingImage();
            ((System.ComponentModel.ISupportInitialize)(this.blinkingImage1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelErrors
            // 
            resources.ApplyResources(this.labelErrors, "labelErrors");
            this.labelErrors.Name = "labelErrors";
            // 
            // linkLabelDetails
            // 
            resources.ApplyResources(this.linkLabelDetails, "linkLabelDetails");
            this.linkLabelDetails.Name = "linkLabelDetails";
            this.linkLabelDetails.TabStop = true;
            this.linkLabelDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDetails_LinkClicked);
            // 
            // blinkingImage1
            // 
            this.blinkingImage1.BlinkMaxDuration = 15000;
            this.blinkingImage1.Image = global::HDGraph.Properties.Resources.Warning;
            resources.ApplyResources(this.blinkingImage1, "blinkingImage1");
            this.blinkingImage1.Name = "blinkingImage1";
            this.blinkingImage1.TabStop = false;
            // 
            // ErrorStatus
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.blinkingImage1);
            this.Controls.Add(this.linkLabelDetails);
            this.Controls.Add(this.labelErrors);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Name = "ErrorStatus";
            this.VisibleChanged += new System.EventHandler(this.ErrorStatus_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.blinkingImage1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelErrors;
        private System.Windows.Forms.LinkLabel linkLabelDetails;
        private BlinkingImage blinkingImage1;
    }
}
