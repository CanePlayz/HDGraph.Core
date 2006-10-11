using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class TreeGraph : UserControl
    {
        #region Variables et propriétés

        /// <summary>
        /// Objet DirectoryNode qui doit être considéré comme la racine du graphe.
        /// </summary>
        private DirectoryNode root;

        /// <summary>
        /// Moteur qui a la charge de conserver l'intégrité de l'arborescence DirectoryNode.
        /// </summary>
        private MoteurGraphiqueur moteur;

        public MoteurGraphiqueur Moteur
        {
            get { return moteur; }
            set
            {
                moteur = value;
                if (moteur != null)
                    root = moteur.Root;
            }
        }


        private int nbNiveaux;
        /// <summary>
        /// Obtient ou définit le nombre de niveaux d'arborescence à afficher.
        /// </summary>
        public int NbNiveaux
        {
            get { return nbNiveaux; }
            set { nbNiveaux = value; }
        }

        private Pen graphPen = new Pen(Color.Black, 1.0f);

        public Pen GraphPen
        {
            get { return graphPen; }
            set { graphPen = value; }
        }

        private bool optionShowSize = true;
        /// <summary>
        /// Obtient ou définit le booléen indiquant si le composant doit afficher la taille des répertoires en plus de leur nom.
        /// </summary>
        public bool OptionShowSize
        {
            get { return optionShowSize; }
            set { optionShowSize = value; }
        }

        private Boolean optionAlsoPaintFiles = false;
        /// <summary>
        /// Obtient ou définit le booléen indiquant si le composant doit afficher les arcs représentant les fichiers ou non.
        /// </summary>
        public Boolean OptionAlsoPaintFiles
        {
            get { return optionAlsoPaintFiles; }
            set { optionAlsoPaintFiles = value; }
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
        private Bitmap buffer;
        /// <summary>
        /// Obtient le gtaph sous forme d'image.
        /// </summary>
        internal Bitmap ImageBuffer
        {
            get { return buffer; }
        }

        /// <summary>
        /// Epaisseur d'un niveau sur le graph.
        /// </summary>
        private float pasNiveau;

        /// <summary>
        /// Graph associé au bitmap buffer
        /// </summary>
        private Graphics graph;
        /// <summary>
        /// Rectangle représentant la surface sur laquelle le graph doit être dessiné.
        /// </summary>
        private RectangleF pieRec;
        /// <summary>
        /// Booléen indiquant le type de parcours lors de la création du graph: 
        /// si false, on est dans la phase de dessin des "camemberts". Si true, on est dans la phase 
        /// qui consiste à imprimer les noms des répertoires sur le dessin.
        /// </summary>
        private bool printDirNames = false;

        #region Variables chaîne (utilisées en tant que cache du resourceManager)

        //private string abrevOctet = HDGTools.resManager.GetString("abreviationOctet");
        //private string abrevKo = HDGTools.resManager.GetString("abreviationKOctet");
        //private string abrevMo = HDGTools.resManager.GetString("abreviationMOctet");
        //private string abrevGo = HDGTools.resManager.GetString("abreviationGOctet");
        //private string abrevTo = HDGTools.resManager.GetString("abreviationTOctet");

        private string abrevOctet = "";
        private string abrevKo = "";
        private string abrevMo = "";
        private string abrevGo = "";
        private string abrevTo = "";

        #endregion

        #endregion

        #region Constructeur

        public TreeGraph()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            if (HDGTools.resManager != null)
            {
                abrevOctet = HDGTools.resManager.GetString("abreviationOctet");
                abrevKo = HDGTools.resManager.GetString("abreviationKOctet");
                abrevMo = HDGTools.resManager.GetString("abreviationMOctet");
                abrevGo = HDGTools.resManager.GetString("abreviationGOctet");
                abrevTo = HDGTools.resManager.GetString("abreviationTOctet");
            }
        }

        #endregion

        #region Méthodes
        private void TreeGraph_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Méthode classique OnPaint surchargée pour afficher le graph, et le calculer si nécessaire.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (buffer == null || buffer.Width != this.Width || buffer.Height != this.Height || forceRefreshOnNextRepaint)
            {
                // Notif de mise en attente
                Form parentForm = FindForm();
                Cursor oldCursor = parentForm.Cursor;
                parentForm.Cursor = Cursors.WaitCursor;
                // Création du bitmap buffer
                buffer = new Bitmap(this.Width, this.Height);
                graph = Graphics.FromImage(buffer);
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.Clear(Color.White);
                // init des données du calcul
                pasNiveau = Math.Min(this.Width / (float)nbNiveaux / 2, this.Height / (float)nbNiveaux / 2);
                pieRec = new RectangleF((float)this.Width / 2,
                                        (float)this.Height / 2,
                                        0,
                                        0);
                // Calcul
                PaintTree();
                graph.Dispose();
                forceRefreshOnNextRepaint = false;
                // Fin de mise en attente
                parentForm.Cursor = oldCursor;
            }
            // affichage du buffer
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        /// <summary>
        /// Effectue le premier lancement de la méthode PaintTree récursive.
        /// </summary>
        private void PaintTree()
        {
            if (root == null || root.TotalSize == 0)
                return;
            printDirNames = false;
            PaintTree(root, pieRec, 0, 360);
            printDirNames = true;
            PaintTree(root, pieRec, 0, 360);
        }

        /// <summary>
        /// Procédure récursive pour graphiquer les arcs de cercle. Graphique de l'extérieur vers l'intérieur.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintTree(DirectoryNode node, RectangleF rec, float startAngle, float endAngle)
        {
            if (node.TotalSize == 0)
                return;
            float nodeAngle = endAngle - startAngle;
            rec.Inflate(pasNiveau, pasNiveau);
            long cumulSize = 0;
            float currentStartAngle;
            foreach (DirectoryNode childNode in node.Children)
            {
                currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                float childAngle = childNode.TotalSize * nodeAngle / node.TotalSize;
                PaintTree(childNode, rec, currentStartAngle, currentStartAngle + childAngle);
                cumulSize += childNode.TotalSize;
            }
            currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
            if (node.Children.Count > 0 && node.FilesSize > 0)
                PaintFilesPart(rec, currentStartAngle, endAngle);
            //if (node.ProfondeurMax <= 1 && endAngle - currentStartAngle > 10)
            //    Console.WriteLine("Processing folder '" + node.Path + "' (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");

            PaintDirPart(node, rec, startAngle, nodeAngle);
        }


        /// <summary>
        /// Dessine sur l'objet "graph" l'arc de cercle représentant un répertoire, 
        /// ou dessine le nom de ce répertoire (l'un ou l'autre, pas les 2, en fonction de la valeur de printDirNames).
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        private void PaintDirPart(DirectoryNode node, RectangleF rec, float startAngle, float nodeAngle)
        {
            // on gère les arcs "pleins" (360 de manière particulière pour avoir un disque "plein", sans trait à l'angle 0)
            if (nodeAngle == 360)
            {
                if (!printDirNames)
                {
                    graph.FillEllipse(
                        new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    GetNextColor(),
                                    Color.SteelBlue,
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
                                ),
                        Rectangle.Round(rec));
                    graph.DrawEllipse(new Pen(Color.Black), rec);
                }
                else
                {
                    float x = 0, y;
                    if (rec.Height == pasNiveau * 2)
                    {
                        y = 0;
                    }
                    else
                    {
                        y = rec.Height / 2f - pasNiveau * 3f / 4f;
                    }
                    x += this.Width / 2f;
                    y += this.Height / 2f;
                    string nodeText = node.Name;
                    if (optionShowSize)
                        nodeText += Environment.NewLine + FormatSize(node.TotalSize);

                    SizeF size = graph.MeasureString(nodeText, Font);
                    x -= size.Width / 2f;
                    y -= size.Height / 2f;
                    // Adoucir le fond du texte :
                    Color colTransp = Color.FromArgb(100, Color.White);
                    graph.FillRectangle(new SolidBrush(colTransp),
                                        x, y, size.Width, size.Height);
                    graph.DrawRectangle(new Pen(Color.Black), x, y, size.Width, size.Height);
                    graph.DrawString(nodeText, Font, new SolidBrush(Color.Black), x, y);
                }
            }
            else
            {
                if (!printDirNames)
                {
                    graph.FillPie(new System.Drawing.Drawing2D.LinearGradientBrush(
                            rec,
                            GetNextColor(),
                            Color.SteelBlue,
                            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
                        ),
                        Rectangle.Round(rec),
                        startAngle,
                        nodeAngle);
                    graph.DrawPie(new Pen(Color.Black), rec, startAngle, nodeAngle);
                }
                else if (nodeAngle > 10)
                {
                    //float textWidthLimit = pasNiveau * 1.5f;
                    float textWidthLimit = pasNiveau * 2f;
                    float x, y, angleCentre, hyp;
                    hyp = (rec.Width - pasNiveau) / 2f;
                    angleCentre = startAngle + nodeAngle / 2f;
                    x = (float)Math.Cos(GetRadianFromDegree(angleCentre)) * hyp;
                    y = (float)Math.Sin(GetRadianFromDegree(angleCentre)) * hyp;
                    x += this.Width / 2f;
                    y += this.Height / 2f;
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    string nodeText = node.Name;
                    SizeF sizeTextName = graph.MeasureString(nodeText, Font);
                    if (sizeTextName.Width <= textWidthLimit)
                    {
                        if (optionShowSize)
                        {
                            float xName = x - sizeTextName.Width / 2f;
                            float yName = y - sizeTextName.Height;
                            graph.DrawString(nodeText, Font, new SolidBrush(Color.Black), xName, yName); //, format);
                            string nodeSize = FormatSize(node.TotalSize);
                            SizeF sizeTextSize = graph.MeasureString(nodeSize, Font);
                            float xSize = x - sizeTextSize.Width / 2f;
                            float ySize = y;
                            // Adoucir le fond du texte :
                            //Color colTransp = Color.FromArgb(50, Color.White);
                            //graph.FillRectangle(new SolidBrush(colTransp),
                            //                    xSize, ySize, sizeTextSize.Width, sizeTextSize.Height);
                            graph.DrawString(nodeSize, Font, new SolidBrush(Color.Black), xSize, ySize); //, format);
                        }
                        else
                        {
                            x -= sizeTextName.Width / 2f;
                            y -= sizeTextName.Height / 2f;
                            graph.DrawString(nodeText, Font, new SolidBrush(Color.Black), x, y); //, format);
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Format une taille en octets en chaine de caractères.
        /// </summary>
        /// <param name="sizeInOctet"></param>
        /// <returns></returns>
        public string FormatSize(long sizeInOctet)
        {
            long unit = 1;
            if (sizeInOctet < unit * 1024)
                return sizeInOctet.ToString() + " " + abrevOctet;
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} " + abrevKo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} " + abrevMo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} " + abrevGo, sizeInOctet / (double)unit);
            unit *= 1024;
            return String.Format("{0:F} " + abrevTo, sizeInOctet / (double)unit);

        }

        /// <summary>
        /// Convertit un angle en degrés en radian.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public double GetRadianFromDegree(float degree)
        {
            return degree * Math.PI / 180f;
        }

        /// <summary>
        /// Convertit un angle en radian en degrés.
        /// </summary>
        public double GetDegreeFromRadian(double radian)
        {
            return radian * 180 / Math.PI;
        }


        /// <summary>
        /// A l'image de PaintDirPart, génère l'arc de cercle correspondant aux fichiers d'un répertoire.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintFilesPart(RectangleF rec, float startAngle, float endAngle)
        {
            if (optionAlsoPaintFiles)
            {
                float nodeAngle = endAngle - startAngle;
                rec.Inflate(pasNiveau, pasNiveau);
                //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
                graph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO
            }
        }


        private void TreeGraph_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Lance la méthode pointée par le delegate UpdateHoverNode, pour signifier au client qu'un répertoire est en ce moment survolé.
        /// </summary>
        private void SendPointedNode()
        {
            if (updateHoverNode == null)
                return;

            DirectoryNode foundNode = FindNodeByCursorPosition(PointToClient(Cursor.Position));
            UpdateHoverNode(foundNode);
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
            double angle = GetDegreeFromRadian(Math.Atan(-curseurPos.Y / (double)curseurPos.X));
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
                return node;
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
            if (node == null)
                contextMenuStrip1.Hide();
            centerGraphOnThisDirectoryToolStripMenuItem.Enabled = (nodeIsNotNull && node != root);
            centerGraphOnParentDirectoryToolStripMenuItem.Enabled = (nodeIsNotNull && node == root);
            openThisDirectoryInWindowsExplorerToolStripMenuItem.Enabled = nodeIsNotNull;
            if (nodeIsNotNull)
                directoryNameToolStripMenuItem.Text = node.Name + " (" + FormatSize(node.TotalSize) + ")";
            else
                directoryNameToolStripMenuItem.Text = "";
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
        /// Est utilisé dans le cas de la génération aléatoire des couleurs.
        /// </summary>
        Random rand = new Random();

        /// <summary>
        /// Renvoie la prochaine couleur à utiliser pour la prochaine partie du graph à dessiner.
        /// </summary>
        /// <returns></returns>
        private Color GetNextColor()
        {
            int[] col = new int[] { rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255) };
            col[rand.Next(3)] -= 100;
            //return Color.FromArgb(rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255));
            return Color.FromArgb(col[0], col[1], col[2]);
        }

        /// <summary>
        /// Force le rafraichissement du contrôle (même si le graph n'a pas changé).
        /// </summary>
        public void ForceRefresh()
        {
            forceRefreshOnNextRepaint = true;
            this.Refresh();
        }

        #endregion

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

    }
}
