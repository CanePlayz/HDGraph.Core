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

        private DirectoryNode root;

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
        private float pasNiveau;

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

        public bool OptionShowSize
        {
            get { return optionShowSize; }
            set { optionShowSize = value; }
        }

        public delegate void UpdateHoverNodeDelegate(DirectoryNode node);

        private UpdateHoverNodeDelegate updateHoverNode;

        public UpdateHoverNodeDelegate UpdateHoverNode
        {
            get { return updateHoverNode; }
            set { updateHoverNode = value; }
        }

        private bool forceRefreshOnNextRepaint = false;

        public bool ForceRefreshOnNextRepaint
        {
            get { return forceRefreshOnNextRepaint; }
            set { forceRefreshOnNextRepaint = value; }
        }


        private Point? lastClicPosition = null;
        private DirectoryNode lastClicNode = null;

        internal Bitmap buffer;
        private Graphics graph;
        private RectangleF pieRec;
        private bool printDirNames = false;

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

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (buffer == null || buffer.Width != this.Width || buffer.Height != this.Height || forceRefreshOnNextRepaint)
            {
                buffer = new Bitmap(this.Width, this.Height);
                graph = Graphics.FromImage(buffer);
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.Clear(Color.White);
                //if (mouseDown)
                //    g.DrawRectangle(p, new Rectangle(startPoint, size));
                pasNiveau = Math.Min(this.Width / (float)nbNiveaux / 2, this.Height / (float)nbNiveaux / 2);
                pieRec = new RectangleF((float)this.Width / 2,
                                        (float)this.Height / 2,
                                        0,
                                        0);
                PaintTree();
                graph.Dispose();
                forceRefreshOnNextRepaint = false;
            }
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

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
        /// Procédure récursive pour graphiquer les acrs de cercle. Graphique de l'extérieur vers l'intérieur.
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
                    if (optionShowSize)
                        nodeText += Environment.NewLine + FormatSize(node.TotalSize);

                    SizeF size = graph.MeasureString(nodeText, Font);
                    x -= size.Width / 2f;
                    y -= size.Height / 2f;
                    //graph.DrawRectangle(new Pen(Color.Black), x, y, size.Width, size.Height);
                    graph.DrawString(nodeText, Font, new SolidBrush(Color.Black), x, y); //, format);
                }

            }
        }

        public string FormatSize(long sizeInOctet)
        {
            long unit = 1;
            if (sizeInOctet < unit * 1024)
                return sizeInOctet.ToString() + " o.";
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} Ko", sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} Mo", sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1024)
                return String.Format("{0:F} Go", sizeInOctet / (double)unit);
            unit *= 1024;
            return String.Format("{0:F} To", sizeInOctet / (double)unit);

        }

        public double GetRadianFromDegree(float degree)
        {
            return degree * Math.PI / 180f;
        }

        public double GetDegreeFromRadian(double radian)
        {
            return radian * 180 / Math.PI;
        }

        private void PaintFilesPart(RectangleF rec, float startAngle, float endAngle)
        {
            float nodeAngle = endAngle - startAngle;
            rec.Inflate(pasNiveau, pasNiveau);
            //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            //graph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle);
        }

        private void TreeGraph_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        private void SendPointedNode()
        {
            if (updateHoverNode == null)
                return;

            DirectoryNode foundNode = FindNodeByCursorPosition(PointToClient(Cursor.Position));
            UpdateHoverNode(foundNode);
        }

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
            if (lastClicNode != null)
                this.root = lastClicNode;
            Refresh();
        }

        private void centerGraphOnParentDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastClicNode != null && lastClicNode.Parent != null)
                this.root = lastClicNode.Parent;
            Refresh();
        }

        private void openThisDirectoryInWindowsExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lastClicNode != null)
                System.Diagnostics.Process.Start(lastClicNode.Path);
        }

        //protected override void NotifyInvalidate(Rectangle invalidatedArea)
        //{
        //    base.NotifyInvalidate(this.DisplayRectangle);
        //}

        Random rand = new Random();
        private Color GetNextColor()
        {
            return Color.FromArgb(rand.Next(255), rand.Next(255), rand.Next(255));
        }

        #endregion

    }
}
