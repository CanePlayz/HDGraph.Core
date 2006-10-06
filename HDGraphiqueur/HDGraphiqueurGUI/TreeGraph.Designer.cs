namespace HDGraphiqueurGUI
{
    partial class TreeGraph
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.directoryPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnParentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.directoryNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directoryNameToolStripMenuItem,
            this.toolStripSeparator1,
            this.directoryPropertiesToolStripMenuItem,
            this.centerGraphOnThisDirectoryToolStripMenuItem,
            this.centerGraphOnParentDirectoryToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(279, 120);
            this.contextMenuStrip1.Text = "Directory \"[TODO]\"";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // directoryPropertiesToolStripMenuItem
            // 
            this.directoryPropertiesToolStripMenuItem.Name = "directoryPropertiesToolStripMenuItem";
            this.directoryPropertiesToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.directoryPropertiesToolStripMenuItem.Text = "Directory properties";
            // 
            // centerGraphOnThisDirectoryToolStripMenuItem
            // 
            this.centerGraphOnThisDirectoryToolStripMenuItem.Name = "centerGraphOnThisDirectoryToolStripMenuItem";
            this.centerGraphOnThisDirectoryToolStripMenuItem.Size = new System.Drawing.Size(261, 22);
            this.centerGraphOnThisDirectoryToolStripMenuItem.Text = "Center Graph on this directory";
            this.centerGraphOnThisDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnThisDirectoryToolStripMenuItem_Click);
            // 
            // centerGraphOnParentDirectoryToolStripMenuItem
            // 
            this.centerGraphOnParentDirectoryToolStripMenuItem.Name = "centerGraphOnParentDirectoryToolStripMenuItem";
            this.centerGraphOnParentDirectoryToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.centerGraphOnParentDirectoryToolStripMenuItem.Text = "Center Graph on parent directory";
            this.centerGraphOnParentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnParentDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(275, 6);
            // 
            // directoryNameToolStripMenuItem
            // 
            this.directoryNameToolStripMenuItem.Enabled = false;
            this.directoryNameToolStripMenuItem.Name = "directoryNameToolStripMenuItem";
            this.directoryNameToolStripMenuItem.Size = new System.Drawing.Size(278, 22);
            this.directoryNameToolStripMenuItem.Text = "DirectoryName";
            // 
            // TreeGraph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "TreeGraph";
            this.Size = new System.Drawing.Size(354, 277);
            this.Load += new System.EventHandler(this.TreeGraph_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseMove);
            this.Resize += new System.EventHandler(this.TreeGraph_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem directoryPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerGraphOnThisDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerGraphOnParentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}
