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
            this.detailsViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.refreshThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnThisDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.centerGraphOnParentDirectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deletePermanentlyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
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
            this.detailsViewToolStripMenuItem,
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem,
            this.toolStripSeparator3,
            this.refreshThisDirectoryToolStripMenuItem,
            this.centerGraphOnThisDirectoryToolStripMenuItem,
            this.centerGraphOnParentDirectoryToolStripMenuItem,
            this.toolStripSeparator2,
            this.deleteToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // directoryNameToolStripMenuItem
            // 
            this.directoryNameToolStripMenuItem.AccessibleDescription = null;
            this.directoryNameToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.directoryNameToolStripMenuItem, "directoryNameToolStripMenuItem");
            this.directoryNameToolStripMenuItem.BackgroundImage = null;
            this.directoryNameToolStripMenuItem.ForeColor = System.Drawing.Color.Navy;
            this.directoryNameToolStripMenuItem.Image = global::HDGraph.Properties.Resources.OpenSelectedItemHS;
            this.directoryNameToolStripMenuItem.Margin = new System.Windows.Forms.Padding(30, 5, 0, 5);
            this.directoryNameToolStripMenuItem.Name = "directoryNameToolStripMenuItem";
            this.directoryNameToolStripMenuItem.Padding = new System.Windows.Forms.Padding(0);
            this.directoryNameToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.directoryNameToolStripMenuItem.Click += new System.EventHandler(this.directoryNameToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.AccessibleDescription = null;
            this.toolStripSeparator1.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // detailsViewToolStripMenuItem
            // 
            this.detailsViewToolStripMenuItem.AccessibleDescription = null;
            this.detailsViewToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.detailsViewToolStripMenuItem, "detailsViewToolStripMenuItem");
            this.detailsViewToolStripMenuItem.BackgroundImage = null;
            this.detailsViewToolStripMenuItem.Image = global::HDGraph.Properties.Resources.ActualSizeHS;
            this.detailsViewToolStripMenuItem.Name = "detailsViewToolStripMenuItem";
            this.detailsViewToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.detailsViewToolStripMenuItem.Click += new System.EventHandler(this.detailsViewToolStripMenuItem_Click);
            // 
            // openThisDirectoryInWindowsExplorerToolStripMenuItem
            // 
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleDescription = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.openThisDirectoryInWindowsExplorerToolStripMenuItem, "openThisDirectoryInWindowsExplorerToolStripMenuItem");
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.BackgroundImage = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Image = global::HDGraph.Properties.Resources.CascadeWindowsHS;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Name = "openThisDirectoryInWindowsExplorerToolStripMenuItem";
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.openThisDirectoryInWindowsExplorerToolStripMenuItem.Click += new System.EventHandler(this.openThisDirectoryInWindowsExplorerToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.AccessibleDescription = null;
            this.toolStripSeparator3.AccessibleName = null;
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // refreshThisDirectoryToolStripMenuItem
            // 
            this.refreshThisDirectoryToolStripMenuItem.AccessibleDescription = null;
            this.refreshThisDirectoryToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.refreshThisDirectoryToolStripMenuItem, "refreshThisDirectoryToolStripMenuItem");
            this.refreshThisDirectoryToolStripMenuItem.BackgroundImage = null;
            this.refreshThisDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.RefreshDocViewHS;
            this.refreshThisDirectoryToolStripMenuItem.Name = "refreshThisDirectoryToolStripMenuItem";
            this.refreshThisDirectoryToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.refreshThisDirectoryToolStripMenuItem.Click += new System.EventHandler(this.refreshThisDirectoryToolStripMenuItem_Click);
            // 
            // centerGraphOnThisDirectoryToolStripMenuItem
            // 
            this.centerGraphOnThisDirectoryToolStripMenuItem.AccessibleDescription = null;
            this.centerGraphOnThisDirectoryToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.centerGraphOnThisDirectoryToolStripMenuItem, "centerGraphOnThisDirectoryToolStripMenuItem");
            this.centerGraphOnThisDirectoryToolStripMenuItem.BackgroundImage = null;
            this.centerGraphOnThisDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.ZoomHS;
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
            this.centerGraphOnParentDirectoryToolStripMenuItem.Image = global::HDGraph.Properties.Resources.GoToParentFolderHS;
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
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.AccessibleDescription = null;
            this.deleteToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.BackgroundImage = null;
            this.deleteToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deletePermanentlyToolStripMenuItem});
            this.deleteToolStripMenuItem.Image = global::HDGraph.Properties.Resources.DeleteFolderHS;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.ShortcutKeyDisplayString = null;
            // 
            // deletePermanentlyToolStripMenuItem
            // 
            this.deletePermanentlyToolStripMenuItem.AccessibleDescription = null;
            this.deletePermanentlyToolStripMenuItem.AccessibleName = null;
            resources.ApplyResources(this.deletePermanentlyToolStripMenuItem, "deletePermanentlyToolStripMenuItem");
            this.deletePermanentlyToolStripMenuItem.BackgroundImage = null;
            this.deletePermanentlyToolStripMenuItem.Name = "deletePermanentlyToolStripMenuItem";
            this.deletePermanentlyToolStripMenuItem.ShortcutKeyDisplayString = null;
            this.deletePermanentlyToolStripMenuItem.Click += new System.EventHandler(this.deletePermanentlyToolStripMenuItem_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // TreeGraph
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Name = "TreeGraph";
            this.Load += new System.EventHandler(this.TreeGraph_Load);
            this.DoubleClick += new System.EventHandler(this.TreeGraph_DoubleClick);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseMove);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TreeGraph_MouseDown);
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
        private System.Windows.Forms.ToolStripMenuItem detailsViewToolStripMenuItem;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}
