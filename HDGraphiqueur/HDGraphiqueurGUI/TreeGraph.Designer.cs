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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TreeGraph));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.directoryNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.centerGraphOnThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnParentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.AccessibleDescription = null;
            this.contextMenuStrip1.AccessibleName = null;
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.BackgroundImage = null;
            this.contextMenuStrip1.Font = null;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directoryNameToolStripMenuItem,
            this.toolStripSeparator1,
            this.centerGraphOnThisDirectoryToolStripMenuItem,
            this.centerGraphOnParentDirectoryToolStripMenuItem,
            this.toolStripSeparator2,
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // directoryNameToolStripMenuItem
            // 
            this.directoryNameToolStripMenuItem.AccessibleDescription = null;
            this.directoryNameToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.directoryNameToolStripMenuItem, "directoryNameToolStripMenuItem");
            this.directoryNameToolStripMenuItem.BackgroundImage = null;
            this.directoryNameToolStripMenuItem.Name = "directoryNameToolStripMenuItem";
            this.directoryNameToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // centerGraphOnThisDirectoryToolStripMenuItem
            // 
            this.centerGraphOnThisDirectoryToolStripMenuItem.AccessibleDescription = null;
            this.centerGraphOnThisDirectoryToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.centerGraphOnThisDirectoryToolStripMenuItem, "centerGraphOnThisDirectoryToolStripMenuItem");
            this.centerGraphOnThisDirectoryToolStripMenuItem.BackgroundImage = null;
            this.centerGraphOnThisDirectoryToolStripMenuItem.Name = "centerGraphOnThisDirectoryToolStripMenuItem";
            this.centerGraphOnThisDirectoryToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.centerGraphOnThisDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnThisDirectoryToolStripMenuItem_Click);
            // 
            // centerGraphOnParentDirectoryToolStripMenuItem
            // 
            this.centerGraphOnParentDirectoryToolStripMenuItem.AccessibleDescription = null;
            this.centerGraphOnParentDirectoryToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.centerGraphOnParentDirectoryToolStripMenuItem, "centerGraphOnParentDirectoryToolStripMenuItem");
            this.centerGraphOnParentDirectoryToolStripMenuItem.BackgroundImage = null;
            this.centerGraphOnParentDirectoryToolStripMenuItem.Name = "centerGraphOnParentDirectoryToolStripMenuItem";
            this.centerGraphOnParentDirectoryToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.centerGraphOnParentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnParentDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.AccessibleDescription = null;
            this.toolStripSeparator2.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // openThisDirectoryInWindowsExplorerToolStripMenuItem
            // 
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleDescription = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openThisDirectoryInWindowsExplorerToolStripMenuItem, "openThisDirectoryInWindowsExplorerToolStripMenuItem");
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.BackgroundImage = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Name = "openThisDirectoryInWindowsExplorerToolStripMenuItem";
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openThisDirectoryInWindowsExplorerToolStripMenuItem_Click);
            // 
            // TreeGraph
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Font = null;
            this.Name = "TreeGraph";
            this.Load += new System.EventHandler(this.TreeGraph_Load);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseMove);
            this.Resize += new System.EventHandler(this.TreeGraph_Resize);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem centerGraphOnThisDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem centerGraphOnParentDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem openThisDirectoryInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryNameToolStripMenuItem;
    }
}
