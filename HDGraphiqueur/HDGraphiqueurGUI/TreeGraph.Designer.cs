namespace HDGraph
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
            this.refreshThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.centerGraphOnThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnParentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePermanentlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.directoryNameToolStripMenuItem,
            this.toolStripSeparator1,
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem,
            this.toolStripSeparator3,
            this.refreshThisDirectoryToolStripMenuItem,
            this.centerGraphOnThisDirectoryToolStripMenuItem,
            this.centerGraphOnParentDirectoryToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            resources.ApplyResources(this.contextMenuStrip1, "contextMenuStrip1");
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // directoryNameToolStripMenuItem
            // 
            this.directoryNameToolStripMenuItem.BackColor = System.Drawing.SystemColors.MenuHighlight;
            resources.ApplyResources(this.directoryNameToolStripMenuItem, "directoryNameToolStripMenuItem");
            this.directoryNameToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.directoryNameToolStripMenuItem.Image = global::HDGraph.Properties.Resources.OpenSelectedItemHS;
            this.directoryNameToolStripMenuItem.Name = "directoryNameToolStripMenuItem";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // refreshThisDirectoryToolStripMenuItem
            // 
            this.refreshThisDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.RefreshDocViewHS;
            this.refreshThisDirectoryToolStripMenuItem.Name = "refreshThisDirectoryToolStripMenuItem";
            resources.ApplyResources(this.refreshThisDirectoryToolStripMenuItem, "refreshThisDirectoryToolStripMenuItem");
            this.refreshThisDirectoryToolStripMenuItem.Click += new System.EventHandler(this.refreshThisDirectoryToolStripMenuItem_Click);
            // 
            // openThisDirectoryInWindowsExplorerToolStripMenuItem
            // 
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Image = global::HDGraph.Properties.Resources.CascadeWindowsHS;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Name = "openThisDirectoryInWindowsExplorerToolStripMenuItem";
            resources.ApplyResources(this.openThisDirectoryInWindowsExplorerToolStripMenuItem, "openThisDirectoryInWindowsExplorerToolStripMenuItem");
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openThisDirectoryInWindowsExplorerToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // centerGraphOnThisDirectoryToolStripMenuItem
            // 
            this.centerGraphOnThisDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.ZoomHS;
            this.centerGraphOnThisDirectoryToolStripMenuItem.Name = "centerGraphOnThisDirectoryToolStripMenuItem";
            resources.ApplyResources(this.centerGraphOnThisDirectoryToolStripMenuItem, "centerGraphOnThisDirectoryToolStripMenuItem");
            this.centerGraphOnThisDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnThisDirectoryToolStripMenuItem_Click);
            // 
            // centerGraphOnParentDirectoryToolStripMenuItem
            // 
            this.centerGraphOnParentDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.GoToParentFolderHS;
            this.centerGraphOnParentDirectoryToolStripMenuItem.Name = "centerGraphOnParentDirectoryToolStripMenuItem";
            resources.ApplyResources(this.centerGraphOnParentDirectoryToolStripMenuItem, "centerGraphOnParentDirectoryToolStripMenuItem");
            this.centerGraphOnParentDirectoryToolStripMenuItem.Click += new System.EventHandler(this.centerGraphOnParentDirectoryToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePermanentlyToolStripMenuItem});
            this.deleteToolStripMenuItem.Image = global::HDGraph.Properties.Resources.DeleteFolderHS;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            // 
            // deletePermanentlyToolStripMenuItem
            // 
            this.deletePermanentlyToolStripMenuItem.Name = "deletePermanentlyToolStripMenuItem";
            resources.ApplyResources(this.deletePermanentlyToolStripMenuItem, "deletePermanentlyToolStripMenuItem");
            this.deletePermanentlyToolStripMenuItem.Click += new System.EventHandler(this.deletePermanentlyToolStripMenuItem_Click);
            // 
            // TreeGraph
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "TreeGraph";
            this.DoubleClick += new System.EventHandler(this.TreeGraph_DoubleClick);
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
        private System.Windows.Forms.ToolStripMenuItem openThisDirectoryInWindowsExplorerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem directoryNameToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshThisDirectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem deletePermanentlyToolStripMenuItem;
    }
}
