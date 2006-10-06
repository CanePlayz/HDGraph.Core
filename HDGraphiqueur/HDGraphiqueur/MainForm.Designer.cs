namespace HDGraphiqueurGUI
{
    partial class MainForm
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

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.buttonScan = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.comboBoxPath = new System.Windows.Forms.ComboBox();
            this.numUpDownNbNivx = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numUpDownNbNivxAffich = new System.Windows.Forms.NumericUpDown();
            this.treeGraph1 = new HDGraphiqueurGUI.TreeGraph();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNbNivx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNbNivxAffich)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonScan
            // 
            this.buttonScan.Location = new System.Drawing.Point(45, 26);
            this.buttonScan.Name = "buttonScan";
            this.buttonScan.Size = new System.Drawing.Size(75, 23);
            this.buttonScan.TabIndex = 1;
            this.buttonScan.Text = "Scanner";
            this.buttonScan.UseVisualStyleBackColor = true;
            this.buttonScan.Click += new System.EventHandler(this.buttonScan_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 559);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(507, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(31, 17);
            this.toolStripStatusLabel1.Text = "Prêt.";
            // 
            // comboBoxPath
            // 
            this.comboBoxPath.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.comboBoxPath.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystemDirectories;
            this.comboBoxPath.FormattingEnabled = true;
            this.comboBoxPath.Location = new System.Drawing.Point(126, 28);
            this.comboBoxPath.Name = "comboBoxPath";
            this.comboBoxPath.Size = new System.Drawing.Size(302, 21);
            this.comboBoxPath.TabIndex = 4;
            this.comboBoxPath.Text = "C:\\php-5.1.2-Win32";
            // 
            // numUpDownNbNivx
            // 
            this.numUpDownNbNivx.Location = new System.Drawing.Point(226, 55);
            this.numUpDownNbNivx.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownNbNivx.Name = "numUpDownNbNivx";
            this.numUpDownNbNivx.Size = new System.Drawing.Size(68, 20);
            this.numUpDownNbNivx.TabIndex = 5;
            this.numUpDownNbNivx.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(123, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Nb Niveaux calcul:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(318, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Nb Niveaux affichage:";
            // 
            // numUpDownNbNivxAffich
            // 
            this.numUpDownNbNivxAffich.Location = new System.Drawing.Point(437, 55);
            this.numUpDownNbNivxAffich.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numUpDownNbNivxAffich.Name = "numUpDownNbNivxAffich";
            this.numUpDownNbNivxAffich.Size = new System.Drawing.Size(58, 20);
            this.numUpDownNbNivxAffich.TabIndex = 8;
            this.numUpDownNbNivxAffich.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.numUpDownNbNivxAffich.ValueChanged += new System.EventHandler(this.numUpDownNbNivxAffich_ValueChanged);
            // 
            // treeGraph1
            // 
            this.treeGraph1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.treeGraph1.Location = new System.Drawing.Point(45, 81);
            this.treeGraph1.Moteur = null;
            this.treeGraph1.Name = "treeGraph1";
            this.treeGraph1.NbNiveaux = 0;
            this.treeGraph1.OptionShowSize = false;
            this.treeGraph1.Size = new System.Drawing.Size(450, 444);
            this.treeGraph1.TabIndex = 0;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 581);
            this.Controls.Add(this.numUpDownNbNivxAffich);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numUpDownNbNivx);
            this.Controls.Add(this.comboBoxPath);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.buttonScan);
            this.Controls.Add(this.treeGraph1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNbNivx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUpDownNbNivxAffich)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TreeGraph treeGraph1;
        private System.Windows.Forms.Button buttonScan;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ComboBox comboBoxPath;
        private System.Windows.Forms.NumericUpDown numUpDownNbNivx;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numUpDownNbNivxAffich;
    }
}