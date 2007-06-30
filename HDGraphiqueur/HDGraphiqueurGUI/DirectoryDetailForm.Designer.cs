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
            this.directoryDetailUC1 = new HDGraph.DirectoryDetailUC();
            this.buttonClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // directoryDetailUC1
            // 
            this.directoryDetailUC1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.directoryDetailUC1.Location = new System.Drawing.Point(-4, 0);
            this.directoryDetailUC1.Name = "directoryDetailUC1";
            this.directoryDetailUC1.Size = new System.Drawing.Size(577, 436);
            this.directoryDetailUC1.TabIndex = 0;
            // 
            // buttonClose
            // 
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.Location = new System.Drawing.Point(485, 432);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 23);
            this.buttonClose.TabIndex = 1;
            this.buttonClose.Text = "Close";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // DirectoryDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 467);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.directoryDetailUC1);
            this.Name = "DirectoryDetailForm";
            this.Text = "DirectoryDetailForm";
            this.ResumeLayout(false);

        }

        #endregion

        private DirectoryDetailUC directoryDetailUC1;
        private System.Windows.Forms.Button buttonClose;
    }
}