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
            this.labelErrors.AccessibleDescription = null;
            this.labelErrors.AccessibleName = null;
            resources.ApplyResources(this.labelErrors, "labelErrors");
            this.labelErrors.Font = null;
            this.labelErrors.Name = "labelErrors";
            // 
            // linkLabelDetails
            // 
            this.linkLabelDetails.AccessibleDescription = null;
            this.linkLabelDetails.AccessibleName = null;
            resources.ApplyResources(this.linkLabelDetails, "linkLabelDetails");
            this.linkLabelDetails.Font = null;
            this.linkLabelDetails.Name = "linkLabelDetails";
            this.linkLabelDetails.TabStop = true;
            this.linkLabelDetails.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDetails_LinkClicked);
            // 
            // blinkingImage1
            // 
            this.blinkingImage1.AccessibleDescription = null;
            this.blinkingImage1.AccessibleName = null;
            resources.ApplyResources(this.blinkingImage1, "blinkingImage1");
            this.blinkingImage1.BackgroundImage = null;
            this.blinkingImage1.BlinkEnabled = true;
            this.blinkingImage1.Font = null;
            this.blinkingImage1.Image = global::HDGraph.Properties.Resources.Warning;
            this.blinkingImage1.ImageLocation = null;
            this.blinkingImage1.Name = "blinkingImage1";
            this.blinkingImage1.TabStop = false;
            // 
            // ErrorStatus
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.blinkingImage1);
            this.Controls.Add(this.linkLabelDetails);
            this.Controls.Add(this.labelErrors);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.Font = null;
            this.Name = "ErrorStatus";
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
