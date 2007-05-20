using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Drawing2D;

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

        private ModeAffichageCouleurs modeCouleur;

        public ModeAffichageCouleurs ModeCouleur
        {
            get { return modeCouleur; }
            set { modeCouleur = value; }
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
        private Bitmap backBuffer;
        /// <summary>
        /// Bitmap buffer dans lequel le graph est dessiné.
        /// </summary>
        private Bitmap multicolorTree;

        /// <summary>
        /// Obtient le gtaph sous forme d'image.
        /// </summary>
        internal Bitmap ImageBuffer
        {
            get { return backBuffer; }
        }

        /// <summary>
        /// Epaisseur d'un niveau sur le graph.
        /// </summary>
        private float pasNiveau;

        /// <summary>
        /// Graph associé au bitmap buffer
        /// </summary>
        private Graphics frontGraph;
        /// <summary>
        /// Booléen indiquant le type de parcours lors de la création du graph: 
        /// si false, on est dans la phase de dessin des "camemberts". Si true, on est dans la phase 
        /// qui consiste à imprimer les noms des répertoires sur le dessin.
        /// </summary>
        private bool printDirNames = false;


        private Color myTransparentColor = Color.Black;

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
            bool sizeChanged = (backBuffer == null || backBuffer.Width != this.ClientSize.Width || backBuffer.Height != this.ClientSize.Height);
            if (sizeChanged || forceRefreshOnNextRepaint)
            {
                // Création du bitmap buffer
                backBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                Graphics backGraph = Graphics.FromImage(backBuffer);
                Bitmap frontBuffer = null;

                if (modeCouleur == ModeAffichageCouleurs.ImprovedLinear)
                {
                    frontBuffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                    frontGraph = Graphics.FromImage(frontBuffer);
                }
                else
                {
                    frontGraph = backGraph;
                    frontGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                }
                frontGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                frontGraph.Clear(Color.White);
                // init des données du calcul
                pasNiveau = Math.Min(this.ClientSize.Width / (float)nbNiveaux / 2, this.ClientSize.Height / (float)nbNiveaux / 2);
                RectangleF pieRec = new RectangleF(this.ClientSize.Width / 2f,
                                        this.ClientSize.Height / 2f,
                                        0,
                                        0);
                // Calcul
                PaintTree(pieRec);
                if (modeCouleur == ModeAffichageCouleurs.ImprovedLinear)
                {
                    if (multicolorTree == null
                        || multicolorTree.Width != this.ClientSize.Width
                        || multicolorTree.Height != this.ClientSize.Height)
                        // reconstruire multicolorTree si la taille a changé !
                        multicolorTree = ConstruireMulticolorTree(
                            new Bitmap(this.ClientSize.Width, this.ClientSize.Height));
                    backGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    backGraph.DrawImageUnscaled(multicolorTree, new Point(0, 0));
                    frontBuffer.MakeTransparent(myTransparentColor);
                    backGraph.DrawImageUnscaled(frontBuffer, new Point(0, 0));
                    frontGraph.Dispose();
                }
                backGraph.Dispose();
                forceRefreshOnNextRepaint = false;
            }
            // affichage du buffer
            e.Graphics.DrawImageUnscaled(backBuffer, 0, 0);
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
                p1.X = pieRec.Left + pieRec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle))) * pieRec.Height / 2f;
                p1.Y = pieRec.Top + pieRec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle))) * pieRec.Height / 2f;
                PointF p2 = new PointF();
                p2.X = pieRec.Left + pieRec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle + nodeAngle))) * pieRec.Height / 2f;
                p2.Y = pieRec.Top + pieRec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle + nodeAngle))) * pieRec.Height / 2f;


                graph.FillPie(
                      new System.Drawing.Drawing2D.LinearGradientBrush(
                            p1, p2,
                            ColorByLeft(Convert.ToInt32(startAngle / 360f * 1000f), 1000),
                            ColorByLeft(Convert.ToInt32((startAngle + nodeAngle) / 360f * 1000f), 1000)
                      ),
                      pieRec, startAngle, nodeAngle + 1);
            }
            return newMutlicolorBitmap;
        }

        /// <summary>
        /// Effectue le premier lancement de la méthode PaintTree récursive.
        /// </summary>
        private void PaintTree(RectangleF pieRec)
        {
            if (root == null || root.TotalSize == 0)
            {
                PaintSpecialCase();
                return;
            }
            printDirNames = false;
            PaintTree(root, pieRec, 0, 360);
            printDirNames = true;
            PaintTree(root, pieRec, 0, 360);
        }

        /// <summary>
        /// Affiche un message spécifique au lieu du graph.
        /// </summary>
        private void PaintSpecialCase()
        {
            float x = this.ClientSize.Width / 2f;
            float y = this.ClientSize.Height / 2f;
            string text;
            if (moteur != null && moteur.WorkCanceled)
                text = Resources.ApplicationMessages.UserCanceledAnalysis;
            else if (root != null && root.TotalSize == 0)
                text = Resources.ApplicationMessages.FolderIsEmpty;
            else
                text = Resources.ApplicationMessages.GraphGuideLine;

            SizeF sizeTextName = frontGraph.MeasureString(text, Font);
            x -= sizeTextName.Width / 2f;
            y -= sizeTextName.Height / 2f;
            frontGraph.DrawString(text, Font, new SolidBrush(Color.Black), x, y); //, format);



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
            if (node.ExistsUncalcSubDir)
            {
                PaintUnknownPart(node, rec, startAngle, endAngle);
            }
            else
            {
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
            }
            PaintDirPart(node, rec, startAngle, nodeAngle);
        }

        /// <summary>
        /// Dessine sur l'objet "graph" l'arc de cercle représentant une partie "inconnue" (confettis)
        /// d'un répertoire.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintUnknownPart(DirectoryNode node, RectangleF rec, float startAngle, float endAngle)
        {
            if (!printDirNames)
            {
                float nodeAngle = endAngle - startAngle;
                rec.Inflate(pasNiveau / 6f, pasNiveau / 6f);
                //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
                frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
                                            System.Drawing.Drawing2D.HatchStyle.LargeConfetti,
                                            Color.Gray,
                                            Color.White),
                                    Rectangle.Round(rec), startAngle, nodeAngle);
            }
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
            // on gère les arcs "pleins" (360°) de manière particulière pour avoir un disque "plein", sans trait à l'angle 0
            if (nodeAngle == 360)
            {
                if (!printDirNames)
                {
                    // on dessine le disque uniquement                  
                    frontGraph.FillEllipse(
                        GetBrushForAngles(rec, startAngle, nodeAngle),
                        Rectangle.Round(rec));
                    frontGraph.DrawEllipse(new Pen(Color.Black), rec);
                }
                else
                {
                    // on dessine les noms de répertoire uniquement
                    float x = 0, y;
                    if (rec.Height == pasNiveau * 2)
                    {
                        y = 0;
                    }
                    else
                    {
                        y = rec.Height / 2f - pasNiveau * 3f / 4f;
                    }
                    x += this.ClientSize.Width / 2f;
                    y += this.ClientSize.Height / 2f;
                    string nodeText = node.Name;
                    if (optionShowSize)
                        nodeText += Environment.NewLine + FormatSize(node.TotalSize);

                    SizeF size = frontGraph.MeasureString(nodeText, Font);
                    x -= size.Width / 2f;
                    y -= size.Height / 2f;
                    // Adoucir le fond du texte :
                    Color colTransp = Color.FromArgb(100, Color.White);
                    frontGraph.FillRectangle(new SolidBrush(colTransp),
                                        x, y, size.Width, size.Height);
                    frontGraph.DrawRectangle(new Pen(Color.Black), x, y, size.Width, size.Height);
                    frontGraph.DrawString(nodeText, Font, new SolidBrush(Color.Black), x, y);
                }
            }
            else
            {
                if (!printDirNames)
                {
                    // on dessine le disque uniquement
                    if (node.IsFreeSpace)
                    {
                        // free space
                        frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
                                                    System.Drawing.Drawing2D.HatchStyle.Wave,
                                                    Color.LightGray,
                                                    Color.White),
                                        Rectangle.Round(rec),
                                        startAngle,
                                        nodeAngle);
                    }
                    else if (node.IsUnknownPart)
                    {
                        // non-calculable files
                        frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
                                                    System.Drawing.Drawing2D.HatchStyle.Trellis,
                                                    Color.Red,
                                                    Color.White),
                                        Rectangle.Round(rec),
                                        startAngle,
                                        nodeAngle);
                    }
                    else
                    {
                        // standard zone
                        frontGraph.FillPie(
                            GetBrushForAngles(rec, startAngle, nodeAngle),
                            Rectangle.Round(rec),
                            startAngle,
                            nodeAngle);
                        frontGraph.DrawPie(new Pen(Color.Black), rec, startAngle, nodeAngle);
                    }
                }
                else if (nodeAngle > 10)
                {
                    // on dessine les noms de répertoire uniquement (si l'angle est supérieur à 10°)
                    //float textWidthLimit = pasNiveau * 1.5f;
                    float textWidthLimit = pasNiveau * 2f;
                    float x, y, angleCentre, hyp;
                    hyp = (rec.Width - pasNiveau) / 2f;
                    angleCentre = startAngle + nodeAngle / 2f;
                    x = (float)Math.Cos(GetRadianFromDegree(angleCentre)) * hyp;
                    y = (float)Math.Sin(GetRadianFromDegree(angleCentre)) * hyp;
                    x += this.ClientSize.Width / 2f;
                    y += this.ClientSize.Height / 2f;
                    StringFormat format = new StringFormat();
                    format.Alignment = StringAlignment.Center;
                    string nodeText = node.Name;
                    SizeF sizeTextName = frontGraph.MeasureString(nodeText, Font);
                    if (sizeTextName.Width <= textWidthLimit)
                    {
                        if (optionShowSize)
                        {
                            float xName = x - sizeTextName.Width / 2f;
                            float yName = y - sizeTextName.Height;
                            frontGraph.DrawString(nodeText, Font, new SolidBrush(Color.Black), xName, yName); //, format);
                            string nodeSize = FormatSize(node.TotalSize);
                            SizeF sizeTextSize = frontGraph.MeasureString(nodeSize, Font);
                            float xSize = x - sizeTextSize.Width / 2f;
                            float ySize = y;
                            // Adoucir le fond du texte :
                            //Color colTransp = Color.FromArgb(50, Color.White);
                            //graph.FillRectangle(new SolidBrush(colTransp),
                            //                    xSize, ySize, sizeTextSize.Width, sizeTextSize.Height);
                            frontGraph.DrawString(nodeSize, Font, new SolidBrush(Color.Black), xSize, ySize); //, format);
                        }
                        else
                        {
                            x -= sizeTextName.Width / 2f;
                            y -= sizeTextName.Height / 2f;
                            frontGraph.DrawString(nodeText, Font, new SolidBrush(Color.Black), x, y); //, format);
                        }
                    }
                }

            }
        }

        private Brush GetBrushForAngles(RectangleF rec, float startAngle, float nodeAngle)
        {
            switch (modeCouleur)
            {
                case ModeAffichageCouleurs.RandomNeutral:
                case ModeAffichageCouleurs.RandomBright:
                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    GetNextColor(startAngle),
                                    Color.SteelBlue,
                                    System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
                                );
                case ModeAffichageCouleurs.Linear2:
                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    GetNextColor(startAngle + (nodeAngle / 2f)),
                                    Color.SteelBlue,
                                    LinearGradientMode.ForwardDiagonal
                                );
                case ModeAffichageCouleurs.Linear:
                    float middleAngle = startAngle + (nodeAngle / 2f);
                    //return new System.Drawing.Drawing2D.LinearGradientBrush(
                    //                rec,
                    //                GetNextColor(middleAngle),
                    //                Color.SteelBlue,
                    //                System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
                    //            );
                    if (middleAngle < 90)
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                        rec,
                                        Color.SteelBlue,
                                        GetNextColor(middleAngle),
                                        LinearGradientMode.ForwardDiagonal
                                    );
                    else if (middleAngle < 180)
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    Color.SteelBlue,
                                    GetNextColor(middleAngle),
                                    LinearGradientMode.BackwardDiagonal
                                );
                    else if (middleAngle < 270)
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    GetNextColor(middleAngle),
                                    Color.SteelBlue,
                                    LinearGradientMode.ForwardDiagonal
                                );
                    else
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    GetNextColor(middleAngle),
                                    Color.SteelBlue,
                                    LinearGradientMode.BackwardDiagonal
                                );
                case ModeAffichageCouleurs.ImprovedLinear:
                default:
                    return new SolidBrush(myTransparentColor);
                //if (nodeAngle < 1)
                //    return new System.Drawing.Drawing2D.LinearGradientBrush(rec,
                //                        GetNextColor(startAngle + (nodeAngle / 2f)),
                //                        Color.SteelBlue,
                //                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
                //PointF p1 = new PointF();
                //p1.X = rec.Left + rec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle))) * rec.Height / 2f;
                //p1.Y = rec.Top + rec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle))) * rec.Height / 2f;
                //PointF p2 = new PointF();
                //p2.X = rec.Left + rec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle + nodeAngle))) * rec.Height / 2f;
                //p2.Y = rec.Top + rec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle + nodeAngle))) * rec.Height / 2f;
                //if (nodeAngle == 360)
                //    p2.X = -p2.X;
                //try
                //{
                //    return new System.Drawing.Drawing2D.LinearGradientBrush(
                //                    p1, p2,
                //                    GetNextColor(startAngle),
                //                    GetNextColor(startAngle + nodeAngle)
                //                );
                //}
                //catch (Exception ex)
                //{
                //    throw;
                //}
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
            if (sizeInOctet < unit * 1000)
                return sizeInOctet.ToString() + " " + abrevOctet;
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
                return String.Format("{0:F} " + abrevKo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
                return String.Format("{0:F} " + abrevMo, sizeInOctet / (double)unit);
            unit *= 1024;
            if (sizeInOctet < unit * 1000)
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
            if (!printDirNames && optionAlsoPaintFiles)
            {
                float nodeAngle = endAngle - startAngle;
                rec.Inflate(pasNiveau, pasNiveau);
                //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
                frontGraph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO

            }
        }


        private void TreeGraph_Resize(object sender, EventArgs e)
        {
            Refresh();
        }

        /// <summary>
        /// Lance la méthode pointée par le delegate UpdateHoverNode, 
        /// pour signifier au client qu'un répertoire est en ce moment survolé.
        /// Met également à jour le curseur courant.
        /// </summary>
        private void SendPointedNode()
        {
            DirectoryNode foundNode = FindNodeByCursorPosition(PointToClient(Cursor.Position));
            if (foundNode == null)
                this.Cursor = System.Windows.Forms.Cursors.Default;
            else
                this.Cursor = System.Windows.Forms.Cursors.Hand;

            if (updateHoverNode != null)
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
                // le noeud courant est celui recherché
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
            bool nodeIsRegularNode = nodeIsNotNull
                                    && !node.IsFreeSpace
                                    && !node.IsUnknownPart;
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
                directoryNameToolStripMenuItem.Text = node.Name + " (" + FormatSize(node.TotalSize) + ")";
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
                moteur.CompleterArborescence(root, this.nbNiveaux); // TODO: changer en nbCalcul
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
        private Color GetNextColor(float angle)
        {
            switch (modeCouleur)
            {
                case ModeAffichageCouleurs.RandomNeutral:
                    int[] col = new int[] { rand.Next(100, 255), rand.Next(100, 255), rand.Next(100, 255) };
                    col[rand.Next(3)] -= 100;
                    return Color.FromArgb(col[0], col[1], col[2]);
                case ModeAffichageCouleurs.RandomBright:
                    return ColorByLeft(rand.Next(360));
                case ModeAffichageCouleurs.Linear:
                default:
                    return ColorByLeft(Convert.ToInt32(angle));
            }
        }

        /// <summary>
        /// Renvoie une couleur de l'arc en ciel.
        /// </summary>
        /// <param name="valeurSur360">une valeur comprise entre 0 et 360.</param>
        /// <returns></returns>
        public Color ColorByLeft(int valeurSur360)
        {
            if (valeurSur360 > 360 || valeurSur360 < 0)
                throw new ArgumentOutOfRangeException("valeurSur360", "Value must be between 0 and 360.");
            return ColorByLeft(valeurSur360, 360);
        }
        public Color ColorByLeft(int valeur, int valeurMax)
        {
            int valMax = valeurMax;
            int section = valeur * 6 / (valMax);
            valeur = Convert.ToInt32(
                        ((float)valeur % (valMax / 6f)) * 255 * 6f / valMax);

            switch (section)
            {
                //						       r     G     b
                case 0: return Color.FromArgb(255, 0, valeur);
                case 1: return Color.FromArgb(255 - valeur, 0, 255);
                case 2: return Color.FromArgb(0, valeur, 255);
                case 3: return Color.FromArgb(0, 255, 255 - valeur);
                case 4: return Color.FromArgb(valeur, 255, 0);
                case 5: return Color.FromArgb(255, 255 - valeur, 0);
                default: return Color.Red;
            }
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
                    System.IO.Directory.Delete(lastClicNode.Path, true);
                    RafraichirArboDuDernierClic();
                    MessageBox.Show(HDGTools.resManager.GetString("DeletionCompleteMsg"),
                                    HDGTools.resManager.GetString("OperationSuccessfullTitle"),
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    string msgErreur = String.Format(
                        HDGTools.resManager.GetString("ErrorDeletingFolder"),
                        ex.Message);
                    MessageBox.Show(msgErreur,
                        HDGTools.resManager.GetString("Error"),
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RafraichirArboDuDernierClic();
                    Trace.TraceError(HDGTools.PrintError(ex));
                }
            }

        }

        private void directoryNameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ContextMenuStrip.Show();
        }

    }
}
