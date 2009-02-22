using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using HDGraph.Resources;
using HDGraph.DrawEngine;

namespace HDGraph
{
    public partial class TreeGraph : UserControl
    {
        #region Variables et propriétés

        /// <summary>
        /// Objet DirectoryNode qui doit être considéré comme la racine du graphe.
        /// </summary>
        private DirectoryNode root;

        public DirectoryNode Root
        {
            get { return root; }
            set { root = value; }
        }

        /// <summary>
        /// Moteur qui a la charge de conserver l'intégrité de l'arborescence DirectoryNode.
        /// </summary>
        private HDGraphScanEngine moteur;

        public HDGraphScanEngine Moteur
        {
            get { return moteur; }
            set
            {
                moteur = value;
                if (moteur != null)
                    root = moteur.Root;
            }
        }

        public ModeAffichageCouleurs ModeCouleur
        {
            get { return drawOptions.ColorStyleChoice; }
            set { drawOptions.ColorStyleChoice = value; }
        }



        /// <summary>
        /// Obtient ou définit le nombre de niveaux d'arborescence à afficher.
        /// </summary>
        public int NbNiveaux
        {
            get { return drawOptions.ShownLevelsCount; }
            set { drawOptions.ShownLevelsCount = value; }
        }

        private Pen graphPen = new Pen(Color.Black, 1.0f);

        public Pen GraphPen
        {
            get { return graphPen; }
            set { graphPen = value; }
        }

        /// <summary>
        /// Obtient ou définit le booléen indiquant si le composant doit afficher la taille des répertoires en plus de leur nom.
        /// </summary>
        public bool OptionShowSize
        {
            get { return drawOptions.ShowSize; }
            set { drawOptions.ShowSize = value; }
        }

        public override Font Font
        {
            get
            {
                return base.Font;
            }
            set
            {
                base.Font = value;
                drawOptions.TextFont = value;
            }
        }

        public delegate void NodeNotificationDelegate(DirectoryNode node);

        private NodeNotificationDelegate updateHoverNode;
        /// <summary>
        /// Obtient ou définit la méthode appelée par le composant TreeGraph lorsque le curseur de la souris 
        /// passe au dessus d'un répertoire du graphe.
        /// </summary>
        public NodeNotificationDelegate UpdateHoverNode
        {
            get { return updateHoverNode; }
            set { updateHoverNode = value; }
        }

        private NodeNotificationDelegate notifyNewRootNode;
        /// <summary>
        /// Obtient ou définit la méthode appelée par le composant TreeGraph lorsque le répertoire au centre du graph a changé.
        /// </summary>
        public NodeNotificationDelegate NotifyNewRootNode
        {
            get { return notifyNewRootNode; }
            set { notifyNewRootNode = value; }
        }

        /// <summary>
        /// Impose au composant de se redessiner, même si sa taille n'a pas changé.
        /// </summary>
        private bool forceRefreshOnNextRepaint = false;

        public bool ForceRefreshOnNextRepaint
        {
            get { return forceRefreshOnNextRepaint; }
            set { forceRefreshOnNextRepaint = value; }
        }


        /// <summary>
        /// Coordonnées du curseur de la souris à l'intérieur du contrôle, lors du dernier clic effectué sur le contrôle.
        /// Utilisé par exemple lors du chargement du menu contextuel, pour savoir sur quel répertoire du graph le clic droit a été effectué.
        /// </summary>
        private Point? lastClicPosition = null;
        /// <summary>
        /// Idem que lastClicPosition, mais stocke le directoryNode directement et non les coordonnées du curseur. 
        /// Est utilisé lorsque lastClicPosition a été définit.
        /// </summary>
        private DirectoryNode lastClicNode = null;

        /// <summary>
        /// Bitmap buffer dans lequel le graph est dessiné.
        /// </summary>
        private Bitmap backBuffer;

        /// <summary>
        /// Obtient le gtaph sous forme d'image.
        /// </summary>
        internal Bitmap ImageBuffer
        {
            get { return backBuffer; }
        }


        #endregion

        #region Constructeur

        public TreeGraph()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }

        #endregion

        #region Méthodes
        private void TreeGraph_Load(object sender, EventArgs e)
        {
            drawOptions.TextFont = this.Font;
        }


        private float pasNiveau;

        public enum CalculationState
        {
            None,
            InProgress,
            Finished
        }

        private CalculationState calculationState;

        private bool resizing;

        public bool Resizing
        {
            get { return resizing; }
            set { resizing = value; }
        }

        private DrawOptions drawOptions = new DrawOptions();

        public DrawOptions DrawOptions
        {
            get { return drawOptions; }
        }

        /// <summary>
        /// Méthode classique OnPaint surchargée pour afficher le graph, et le calculer si nécessaire.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            bool sizeChanged = (backBuffer == null
                                || backBuffer.Width != this.ClientSize.Width
                                || backBuffer.Height != this.ClientSize.Height);
            Bitmap backBufferTmp = backBuffer;
            try
            {
                if (backBuffer == null)
                {
                    // tout premier init.
                    drawOptions.BitmapSize = this.ClientSize;
                    ImageGraphGenerator generator = new ImageGraphGenerator(this.Root, moteur, drawOptions);
                    forceRefreshOnNextRepaint = true;
                    this.backgroundWorker1_DoWork(this, new DoWorkEventArgs(generator));
                }
                if (sizeChanged || forceRefreshOnNextRepaint)
                {
                    ImageGraphGenerator generator;
                    if (resizing)
                        backBufferTmp = TransformToWaitImage(this.backBuffer, this.ClientSize, ApplicationMessages.ResizeInProgressByUser);
                    else
                        switch (calculationState)
                        {
                            case CalculationState.None:
                                calculationState = CalculationState.InProgress;
                                backBufferTmp = TransformToWaitImage(this.backBuffer, this.ClientSize, ApplicationMessages.PleaseWaitWhileDrawing);

                                // lancement du calcul
                                // Calcul
                                drawOptions.BitmapSize = this.ClientSize;
                                generator = new ImageGraphGenerator(this.Root, moteur, drawOptions);
                                backgroundWorker1.RunWorkerAsync(generator);
                                break;
                            case CalculationState.InProgress:
                                backBufferTmp = TransformToWaitImage(this.backBuffer, this.ClientSize, ApplicationMessages.PleaseWaitWhileDrawing);
                                break;
                            case CalculationState.Finished:
                                if (backBuffer.Size != this.ClientSize
                                    || !drawOptions.Equals(lastCompletedGraphOption))
                                {
                                    calculationState = CalculationState.InProgress;
                                    backBufferTmp = TransformToWaitImage(this.backBuffer, this.ClientSize, ApplicationMessages.PleaseWaitWhileDrawing);

                                    // lancement du calcul
                                    // Calcul
                                    drawOptions.BitmapSize = this.ClientSize;
                                    generator = new ImageGraphGenerator(this.Root, moteur, drawOptions);
                                    backgroundWorker1.RunWorkerAsync(generator);
                                }
                                else
                                {
                                    backBufferTmp = backBuffer;
                                    forceRefreshOnNextRepaint = false;
                                    calculationState = CalculationState.None;
                                }
                                break;
                            default:
                                throw new NotSupportedException("Value of calculationState (" + calculationState + ") is not supported.");
                        }
                }


            }
            catch (Exception ex)
            {
                // TODO : gérer exception.
                backBufferTmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                Graphics.FromImage(backBufferTmp).DrawString(ex.ToString(), this.ParentForm.Font, new SolidBrush(Color.Black), new RectangleF(0, 0, this.ClientSize.Width, this.ClientSize.Height));
            }
            // affichage du buffer
            e.Graphics.DrawImageUnscaled(backBufferTmp, 0, 0);

        }

        private Bitmap TransformToWaitImage(Bitmap originalBitmap, Size clientSize, string message)
        {
            if (originalBitmap == null)
                return null;
            Bitmap newBitmap = new Bitmap(clientSize.Width, clientSize.Height);
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                float originalRatio = originalBitmap.Height / (float)originalBitmap.Width;
                float newRatio = clientSize.Height / (float)clientSize.Width;

                float ratio = Math.Min(originalRatio, newRatio);

                float hScale = (float)clientSize.Height / originalBitmap.Height;
                float wScale = (float)clientSize.Width / originalBitmap.Width;
                Rectangle targetRectangle = new Rectangle();


                float newWidth = originalBitmap.Width * clientSize.Height / (float)originalBitmap.Height;
                if (newWidth > clientSize.Width)
                {
                    targetRectangle.Width = clientSize.Width;
                    targetRectangle.Height = Convert.ToInt32(originalBitmap.Height * clientSize.Width / (float)originalBitmap.Width);

                    targetRectangle.X = 0;
                    targetRectangle.Y = Math.Abs(targetRectangle.Height - clientSize.Height) / 2;
                }
                else
                {
                    targetRectangle.Width = Convert.ToInt32(newWidth);
                    targetRectangle.Height = clientSize.Height;

                    targetRectangle.X = targetRectangle.Y = Math.Abs(targetRectangle.Width - clientSize.Width) / 2;
                    targetRectangle.Y = 0;
                }
                g.DrawImage(originalBitmap, targetRectangle);

                Brush brush = new SolidBrush(Color.FromArgb(100, 255, 255, 255));
                g.FillRectangle(brush, 0, 0, newBitmap.Width, newBitmap.Height);
                brush = new SolidBrush(Color.FromArgb(50, 0, 0, 0));
                g.FillRectangle(brush, 0, 0, newBitmap.Width, newBitmap.Height);

                Font font = new Font(System.Drawing.FontFamily.GenericSerif, 24, FontStyle.Bold);
                ImageGraphGenerator.AfficherTexteAuCentre(g, clientSize, message, font, new SolidBrush(Color.Black), true);

            }
            return newBitmap;
        }

        private Bitmap ConstruireMulticolorTree(Bitmap newMutlicolorBitmap)
        {
            Rectangle pieRec;
            if (ClientSize.Height > ClientSize.Width)
                pieRec = new Rectangle(-(ClientSize.Height - ClientSize.Width) / 2, 0, ClientSize.Height, ClientSize.Height);
            else
                pieRec = new Rectangle(0, -(ClientSize.Width - ClientSize.Height) / 2, ClientSize.Width, ClientSize.Width);
            Graphics graph = Graphics.FromImage(newMutlicolorBitmap);
            int nbMaxQuartiers = 1000;
            for (int i = 0; i < nbMaxQuartiers; i++)
            {
                float startAngle = (360f / nbMaxQuartiers) * i;
                float nodeAngle = 360f / nbMaxQuartiers;
                PointF p1 = new PointF();
                p1.X = pieRec.Left + pieRec.Width / 2f + Convert.ToSingle(Math.Cos(MathHelper.GetRadianFromDegree(startAngle))) * pieRec.Height / 2f;
                p1.Y = pieRec.Top + pieRec.Height / 2f + Convert.ToSingle(Math.Sin(MathHelper.GetRadianFromDegree(startAngle))) * pieRec.Height / 2f;
                PointF p2 = new PointF();
                p2.X = pieRec.Left + pieRec.Width / 2f + Convert.ToSingle(Math.Cos(MathHelper.GetRadianFromDegree(startAngle + nodeAngle))) * pieRec.Height / 2f;
                p2.Y = pieRec.Top + pieRec.Height / 2f + Convert.ToSingle(Math.Sin(MathHelper.GetRadianFromDegree(startAngle + nodeAngle))) * pieRec.Height / 2f;


                graph.FillPie(
                      new System.Drawing.Drawing2D.LinearGradientBrush(
                            p1, p2,
                            ColorManager.ColorByLeft(Convert.ToInt32(startAngle / 360f * 1000f), 1000),
                            ColorManager.ColorByLeft(Convert.ToInt32((startAngle + nodeAngle) / 360f * 1000f), 1000)
                      ),
                      pieRec, startAngle, nodeAngle + 1);
            }
            return newMutlicolorBitmap;
        }






        private void TreeGraph_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Lance la méthode pointée par le delegate UpdateHoverNode, 
        /// pour signifier au client qu'un répertoire est en ce moment survolé.
        /// Met également à jour le curseur courant et l'infoBulle du répertoire survolé.
        /// </summary>
        private void SendPointedNode()
        {
            DirectoryNode foundNode = FindNodeByCursorPosition(PointToClient(Cursor.Position));
            if (foundNode == null)
            {
                this.Cursor = System.Windows.Forms.Cursors.Default;
                HideToolTip();
            }
            else
            {
                this.Cursor = System.Windows.Forms.Cursors.Hand;
                UpdateOrCreateToolTip(foundNode);
            }

            if (updateHoverNode != null)
                UpdateHoverNode(foundNode);
        }

        private ToolTip toolTip;

        private bool showTooltip = true;
        /// <summary>
        /// Hide or show a tooltip on directories.
        /// </summary>
        public bool ShowTooltip
        {
            get { return showTooltip; }
            set { showTooltip = value; }
        }

        /// <summary>
        /// Ensure the correct tooltip is affected to the current userControl, according to the given node.
        /// </summary>
        /// <param name="foundNode"></param>
        private void UpdateOrCreateToolTip(DirectoryNode foundNode)
        {
            if (!ShowTooltip
                || this.calculationState == CalculationState.InProgress)
                return;
            if (toolTip != null
                && (string)toolTip.Tag != foundNode.Path)
                HideToolTip();
            if (toolTip == null)
            {
                toolTip = new ToolTip();
                toolTip.Tag = foundNode.Path;
                toolTip.IsBalloon = true;
                string toolTipText = foundNode.Name + Environment.NewLine + foundNode.HumanReadableTotalSize;
                toolTip.SetToolTip(this, toolTipText);
            }
        }

        /// <summary>
        /// Hide a previous affected tooltip.
        /// </summary>
        private void HideToolTip()
        {
            if (toolTip != null)
            {
                toolTip.RemoveAll();
                //.Hide(this.ParentForm);
                toolTip = null;
            }

        }

        /// <summary>
        /// Trouve quel est le répertoire survolé d'après la position du curseur.
        /// (Recherche par coordonnées cartésiennes).
        /// </summary>
        /// <param name="curseurPos">Position du curseur. Doit être relative au contrôle, pas à l'écran ou à la form !</param>
        /// <returns></returns>
        private DirectoryNode FindNodeByCursorPosition(Point curseurPos)
        {
            // On a les coordonnées du curseur dans le controle.
            // Il faut faire un changement de référentiel pour avoir les coordonnées vis à vis de l'origine (le centre des cercles).
            curseurPos.X -= Width / 2;
            curseurPos.Y -= Height / 2;
            // On a maintenant les coordonnées vis-à-vis du centre des cercles.
            //System.Windows.Forms.MessageBox.Show(curseurPos.ToString());

            // Cherchons l'angle formé formé par le curseur et la taille du rayon jusqu'à celui-ci.
            double angle = MathHelper.GetDegreeFromRadian(Math.Atan(-curseurPos.Y / (double)curseurPos.X));
            // l'angle obtenu à corriger en fonction du quartier où se situe le curseur
            if (curseurPos.X < 0)
                angle = 180 - angle;
            else
                angle = (curseurPos.Y < 0) ? 360 - angle : -angle;

            double rayon = Math.Sqrt(Math.Pow(curseurPos.X, 2) + Math.Pow(curseurPos.Y, 2));
            //System.Windows.Forms.MessageBox.Show("angle: " + angle + "; rayon: " + rayon);
            if (root == null || root.TotalSize == 0)
                return root;
            DirectoryNode foundNode = FindNodeInTree(root, 0, 0, 360, angle, rayon);
            return foundNode;
        }

        /// <summary>
        /// Recherche quel est le répertoire dans lequel se trouve le point définit par l'angle cursorAngle et la distance cursorLen.
        /// (Recherche par coordonnées polaires).
        /// </summary>
        /// <param name="node"></param>
        /// <param name="levelHeight"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        /// <param name="cursorAngle"></param>
        /// <param name="cursorLen"></param>
        /// <returns></returns>
        private DirectoryNode FindNodeInTree(DirectoryNode node, float levelHeight, float startAngle, float endAngle, double cursorAngle, double cursorLen)
        {
            if (node.TotalSize == 0)
                return node;
            float nodeAngle = endAngle - startAngle;
            levelHeight += pasNiveau;
            if (levelHeight > cursorLen && cursorAngle >= startAngle && cursorAngle <= endAngle)
            {
                // le noeud courant est celui recherché
                if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndHide)
                    return null;
                return node;
            }
            long cumulSize = 0;
            float currentStartAngle;
            foreach (DirectoryNode childNode in node.Children)
            {
                currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                float childAngle = childNode.TotalSize * nodeAngle / node.TotalSize;
                if (cursorLen > levelHeight && cursorAngle >= currentStartAngle && cursorAngle <= (currentStartAngle + childAngle))
                    return FindNodeInTree(childNode, levelHeight, currentStartAngle, currentStartAngle + childAngle, cursorAngle, cursorLen);
                cumulSize += childNode.TotalSize;
            }
            currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
            return null;
        }

        private void TreeGraph_MouseMove(object sender, MouseEventArgs e)
        {
            SendPointedNode();
        }

        private void TreeGraph_MouseDown(object sender, MouseEventArgs e)
        {
            lastClicPosition = PointToClient(Cursor.Position);
        }

        /// <summary>
        /// Chargement du menu contextuel lors du clic droit sur le contrôle.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if (!lastClicPosition.HasValue)
                return;

            DirectoryNode node = FindNodeByCursorPosition(lastClicPosition.Value);
            lastClicNode = node;
            bool nodeIsNotNull = (node != null);
            bool nodeIsRegularNode = nodeIsNotNull
                                    && node.DirectoryType == SpecialDirTypes.NotSpecial;
            if (node == null)
            {
                e.Cancel = true;
                return;
            }
            directoryNameToolStripMenuItem.Enabled = nodeIsNotNull;
            centerGraphOnThisDirectoryToolStripMenuItem.Enabled = (nodeIsRegularNode
                                                                   && node != root);
            centerGraphOnParentDirectoryToolStripMenuItem.Enabled = (nodeIsRegularNode
                                                                && node.Parent != root
                                                                && node.Parent != null);
            openThisDirectoryInWindowsExplorerToolStripMenuItem.Enabled = nodeIsRegularNode;
            refreshThisDirectoryToolStripMenuItem.Enabled = nodeIsRegularNode;

            // Item "Suppression"
            deleteToolStripMenuItem.Enabled = nodeIsRegularNode && Properties.Settings.Default.OptionAllowFolderDeletion;

            // Item "Titre du dossier"
            if (nodeIsNotNull)
                directoryNameToolStripMenuItem.Text = node.Name + " (" + HDGTools.FormatSize(node.TotalSize) + ")";
            else
                directoryNameToolStripMenuItem.Text = "/";
        }

        private void centerGraphOnThisDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterGraphOnThisDirectory();
        }

        /// <summary>
        /// Centre le graph sur le répertoire designé par lastClicNode.
        /// </summary>
        private void CenterGraphOnThisDirectory()
        {
            if (lastClicNode != null)
            {
                this.root = lastClicNode;
                moteur.CompleterArborescence(root, this.NbNiveaux); // TODO: changer en nbCalcul
                if (notifyNewRootNode != null)
                    notifyNewRootNode(this.root);
            }
            ForceRefresh();
        }

        private void centerGraphOnParentDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CenterGraphOnParentDirectory();
        }

        /// <summary>
        /// Centre le graph sur le répertoire parent de lastClicNode.
        /// </summary>
        private void CenterGraphOnParentDirectory()
        {
            if (lastClicNode != null && lastClicNode.Parent != null)
            {
                this.root = lastClicNode.Parent;
                if (notifyNewRootNode != null)
                    notifyNewRootNode(this.root);
            }
            ForceRefresh();
        }

        /// <summary>
        /// Ouvre le répertoire désigné par lastClicNode dans l'explorateur.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openThisDirectoryInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastClicNode != null)
                System.Diagnostics.Process.Start(lastClicNode.Path);
        }

        /// <summary>
        /// Force le rafraichissement du contrôle (même si le graph n'a pas changé).
        /// </summary>
        public void ForceRefresh()
        {
            forceRefreshOnNextRepaint = true;
            this.Refresh();
        }

        /// <summary>
        /// Gère le double clic sur le contrôle (recentrage du graph sur le répertoire cliqué ou sur le parent du répertoire cliqué).
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeGraph_DoubleClick(object sender, EventArgs e)
        {
            lastClicPosition = PointToClient(Cursor.Position);
            lastClicNode = FindNodeByCursorPosition(lastClicPosition.Value);
            if (lastClicNode != null)
            {
                if (lastClicNode == root)
                    CenterGraphOnParentDirectory();
                else
                    CenterGraphOnThisDirectory();
            }

        }

        /// <summary>
        /// Demande le rafraichissement du répertoire de l'arborescence pointé par la souris.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void refreshThisDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RafraichirArboDuDernierClic();
        }

        #endregion

        /// <summary>
        /// Rafraichit l'arborescence visée par le dernier clic.
        /// </summary>
        private void RafraichirArboDuDernierClic()
        {
            if (lastClicNode != null)
            {
                moteur.RafraichirArborescence(lastClicNode);
            }
            ForceRefresh();
        }

        /// <summary>
        /// Supprime un dossier.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void deletePermanentlyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = String.Format(HDGTools.resManager.GetString("GoingToDeleteFolderMsg"),
                                       lastClicNode.Name);
            if ((!Properties.Settings.Default.OptionDeletionAsk4Confirmation)
                || MessageBox.Show(msg,
                        HDGTools.resManager.GetString("GoingToDeleteFolderTitle"),
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Exclamation,
                        MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                try
                {
                    new WaitForm().ShowDialogAndStartAction(HDGTools.resManager.GetString("DeleteInProgress"),
                                                            DeleteSelectedForlder);
                    RafraichirArboDuDernierClic();
                    MessageBox.Show(HDGTools.resManager.GetString("DeletionCompleteMsg"),
                                    HDGTools.resManager.GetString("OperationSuccessfullTitle"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    WaitForm.HideWaitForm();
                    string msgErreur = String.Format(
                        HDGTools.resManager.GetString("ErrorDeletingFolder"),
                        ex.Message);
                    MessageBox.Show(msgErreur,
                        HDGTools.resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Trace.TraceError(HDGTools.PrintError(ex));
                    RafraichirArboDuDernierClic();
                }
            }
        }

        /// <summary>
        /// Supprime définitivement un répertoire et rafraichit l'arborescence en conséquence.
        /// </summary>
        private void DeleteSelectedForlder()
        {
            System.IO.Directory.Delete(lastClicNode.Path, true);
        }



        private void directoryNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuStrip.Show();
        }


        internal void ShowFreeSpace()
        {
            if (moteur != null)
            {
                moteur.ShowDiskFreeSpace = true;
                moteur.ApplyFreeSpaceOption(this.root);
            }
            this.ForceRefresh();
        }

        internal void HideFreeSpace()
        {
            if (moteur != null)
            {
                moteur.ShowDiskFreeSpace = false;
                moteur.ApplyFreeSpaceOption(this.root);
            }
            this.ForceRefresh();
        }

        private void detailsViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNodeDetails(lastClicNode);
        }

        /// <summary>
        /// Open a new form showing the details of a given DirectoryNode.
        /// </summary>
        /// <param name="node"></param>
        public static void ShowNodeDetails(DirectoryNode node)
        {
            if (node == null)
                return;
            if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
            {
                MessageBox.Show(
                    String.Format(
                            ApplicationMessages.FreeSpaceDescription,
                            node.HumanReadableTotalSize, node.TotalSize
                            ).Replace("\\n", Environment.NewLine),
                    "HDGraph", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (node.DirectoryType == SpecialDirTypes.UnknownPart)
            {
                MessageBox.Show(
                   String.Format(
                           ApplicationMessages.UnknownPartDescription,
                           node.HumanReadableTotalSize, node.TotalSize
                           ).Replace("\\n", Environment.NewLine),
                   "HDGraph", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (node.ExistsUncalcSubDir)
            {
                MessageBox.Show(
                            ApplicationMessages.UnableToShowUnknownContent,
                    "HDGraph", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DirectoryDetailForm form = new DirectoryDetailForm();
            form.Directory = node;
            form.Owner = Application.OpenForms[0];
            form.Show();
        }

        DrawOptions lastCompletedGraphOption;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            ImageGraphGenerator generator = e.Argument as ImageGraphGenerator;
            if (generator == null)
                return;
            BiResult<Bitmap, DrawOptions> result = generator.Draw();
            Bitmap backBufferTmp = result.Obj1;
            lastCompletedGraphOption = result.Obj2;
            pasNiveau = generator.PasNiveau;
            backBuffer = backBufferTmp;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            forceRefreshOnNextRepaint = true;
            calculationState = CalculationState.Finished;
            this.Refresh();
        }
    }
}
