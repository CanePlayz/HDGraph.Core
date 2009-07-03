using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;
using System.Globalization;

namespace HDGraph.WpfDrawEngine
{
    /// <summary>
    /// Interaction logic for TreeGraph.xaml
    /// </summary>
    public partial class TreeGraph : UserControl
    {
        public TreeGraph()
        {
            InitializeComponent();
            labelStatus.Content = "Acceleration : " + WpfUtils.GetAccelerationType().ToString();
        }

        /// <summary>
        /// Epaisseur d'un niveau sur le graph.
        /// </summary>
        private double singleLevelHeight;

        private DrawOptions currentWorkingOptions;
        private IDirectoryNode rootNode;

        public void SetRoot(IDirectoryNode root, DrawOptions options)
        {
            if (root == null || options == null)
                return;
            this.rootNode = root;


            // Création du bitmap buffer
            currentWorkingOptions = options;

            // init des données du calcul
            //singleLevelHeight = Convert.ToDouble(
            //                Math.Min(this.Width / currentWorkingOptions.ShownLevelsCount / 2,
            //                         this.Height / currentWorkingOptions.ShownLevelsCount / 2));

            // init des données du calcul
            singleLevelHeight = Convert.ToDouble(
                            Math.Min(500 / currentWorkingOptions.ShownLevelsCount / 2,
                                     500 / currentWorkingOptions.ShownLevelsCount / 2));

            labelInfo.Visibility = Visibility.Hidden;
            if (rootNode == null || rootNode.TotalSize == 0)
            {
                PaintSpecialCase();
                return;
            }
            BuildTree(rootNode, 0, 0, 360);
        }


        /// <summary>
        /// Affiche un message spécifique au lieu du graph.
        /// </summary>
        private void PaintSpecialCase()
        {
            labelInfo.Visibility = Visibility.Visible;
            // TODO.

            //string text;
            //if (moteur != null && moteur.WorkCanceled)
            //    text = Resources.ApplicationMessages.UserCanceledAnalysis;
            //else if (rootNode != null && rootNode.TotalSize == 0)
            //    text = Resources.ApplicationMessages.FolderIsEmpty;
            //else
            //    text = Resources.ApplicationMessages.GraphGuideLine;

            //DrawHelper.PrintTextInTheMiddle(frontGraph, currentWorkingOptions.BitmapSize, text, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), false);
        }

        private const float MINIMUM_ANGLE_TO_DRAW = 1;

        /// <summary>
        /// Procédure récursive pour graphiquer les arcs de cercle. Graphique de l'extérieur vers l'intérieur.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void BuildTree(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            if (node.TotalSize == 0)
                return;
            float nodeAngle = endAngle - startAngle;

            if (node.ExistsUncalcSubDir)
            {
                PaintUnknownPart(node, currentLevel + 1, startAngle, endAngle);
            }
            else
            {
                long cumulSize = 0;
                float currentStartAngle = 0;
                bool multiFolderView = false;
                foreach (IDirectoryNode childNode in node.Children)
                {
                    if (!multiFolderView)
                        currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;

                    if (childNode.DirectoryType != SpecialDirTypes.FreeSpaceAndHide)
                    {
                        float childAngle = childNode.TotalSize * nodeAngle / node.TotalSize;
                        if (childAngle < MINIMUM_ANGLE_TO_DRAW)
                        {
                            multiFolderView = true;
                        }
                        else
                        {
                            float tempEndAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                            if (multiFolderView)
                                PaintMultipleNodesPart(node, currentLevel + 1, currentStartAngle, tempEndAngle);
                            currentStartAngle = tempEndAngle;
                            BuildTree(childNode, currentLevel + 1, currentStartAngle, currentStartAngle + childAngle);
                            multiFolderView = false;
                        }
                        cumulSize += childNode.TotalSize;
                    }
                }
                if (multiFolderView)
                {
                    float tempEndAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                    PaintMultipleNodesPart(node, currentLevel + 1, currentStartAngle, tempEndAngle);
                }
                currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                if (node.Children.Count > 0 && node.FilesSize > 0)
                    BuildFilesPart(currentLevel, currentStartAngle, endAngle);
                //if (node.ProfondeurMax <= 1 && endAngle - currentStartAngle > 10)
                //    Console.WriteLine("Processing folder '" + node.Path + "' (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            }
            BuildDirPart(node, currentLevel, startAngle, endAngle);
        }

        /// <summary>
        /// Dessine sur l'objet "graph" l'arc de cercle représentant une partie "inconnue" (confettis)
        /// d'un répertoire.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintUnknownPart(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Arc arc = new Arc();
            arc.BeginEdit();
            arc.StartAngle = startAngle;
            arc.StopAngle = endAngle - startAngle;
            arc.SmallRadius = Convert.ToSingle(currentLevel * singleLevelHeight);
            arc.LargeRadius = Convert.ToSingle(currentLevel * singleLevelHeight + singleLevelHeight / 6);
            arc.Node = node;
            arc.path1.Style = (Style)FindResource("UncalculatedPart");
            arc.path1.StrokeThickness = 0;
            arc.EndEdit();
            canvas1.Children.Add(arc);
            // TODO : arc.brush1 ==> LargeConfetti

            //frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
            //                            System.Drawing.Drawing2D.HatchStyle.LargeConfetti,
            //                            Color.Gray,
            //                            Color.White),
            //                    Rectangle.Round(rec), startAngle, nodeAngle);

        }

        private void PaintMultipleNodesPart(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Arc arc = new Arc();
            arc.BeginEdit();
            arc.StartAngle = startAngle;
            arc.StopAngle = endAngle - startAngle;
            arc.SmallRadius = Convert.ToSingle(currentLevel * singleLevelHeight);
            arc.LargeRadius = Convert.ToSingle((currentLevel + 1) * singleLevelHeight);
            arc.Node = node;
            arc.path1.Style = (Style)FindResource("MultipleNodeStyle");
            arc.path1.StrokeThickness = 0;
            arc.EndEdit();
            canvas1.Children.Add(arc);
        }


        /// <summary>
        /// Dessine sur l'objet "graph" l'arc de cercle représentant un répertoire, 
        /// ou dessine le nom de ce répertoire (l'un ou l'autre, pas les 2, en fonction de la valeur de printDirNames).
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        private void BuildDirPart(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            // on gère les arcs "pleins" (360°) de manière particulière pour avoir un disque "plein", sans trait à l'angle 0
            if ((endAngle - startAngle) == 360)
            {
                Ellipse e = new Ellipse()
                {
                    Width = currentLevel * singleLevelHeight,
                    Height = Width
                };
                canvas1.Children.Add(e);
                // TODO : print text.
            }
            else
            {
                BuildPartialPie(node, currentLevel, startAngle, endAngle);
            }
        }

        ///// <summary>
        ///// Dessine le nom d'un répertoire sur le graph, lorsque ce répertoire a un angle de 360°.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="rec"></param>
        ///// <returns></returns>
        //private void WriteDirectoryNameForFullPie(IDirectoryNode node, RectangleF rec)
        //{
        //    float x = 0, y;
        //    if (rec.Height == singleLevelHeight * 2)
        //    {
        //        y = 0;
        //    }
        //    else
        //    {
        //        y = rec.Height / 2f - singleLevelHeight * 3f / 4f;
        //    }
        //    x += currentWorkingOptions.BitmapSize.Width / 2f;
        //    y += currentWorkingOptions.BitmapSize.Height / 2f;
        //    string nodeText = node.Name;
        //    if (currentWorkingOptions.ShowSize)
        //        nodeText += Environment.NewLine + HDGTools.FormatSize(node.TotalSize);

        //    SizeF size = frontGraph.MeasureString(nodeText, currentWorkingOptions.TextFont);
        //    x -= size.Width / 2f;
        //    y -= size.Height / 2f;
        //    // Adoucir le fond du texte :
        //    Color colTransp = Color.FromArgb(100, Color.White);
        //    frontGraph.FillRectangle(new SolidBrush(colTransp),
        //                        x, y, size.Width, size.Height);
        //    frontGraph.DrawRectangle(new Pen(Color.Black), x, y, size.Width, size.Height);
        //    frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), x, y);
        //}

        ///// <summary>
        ///// Dessine le nom d'un répertoire sur le graph.
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="rec"></param>
        ///// <param name="startAngle"></param>
        ///// <param name="nodeAngle"></param>
        ///// <returns></returns>
        //private void WriteDirectoryName(IDirectoryNode node, RectangleF rec, float startAngle, float nodeAngle)
        //{
        //    //float textWidthLimit = singleLevelHeight * 1.5f;
        //    float textWidthLimit = singleLevelHeight * 2f;
        //    float x, y, angleCentre, hyp;
        //    hyp = (rec.Width - singleLevelHeight) / 2f;
        //    angleCentre = startAngle + nodeAngle / 2f;
        //    x = (float)Math.Cos(MathHelper.GetRadianFromDegree(angleCentre)) * hyp;
        //    y = (float)Math.Sin(MathHelper.GetRadianFromDegree(angleCentre)) * hyp;
        //    x += currentWorkingOptions.BitmapSize.Width / 2f;
        //    y += currentWorkingOptions.BitmapSize.Height / 2f;
        //    StringFormat format = new StringFormat();
        //    format.Alignment = StringAlignment.Center;
        //    string nodeText = node.Name;
        //    SizeF sizeTextName = frontGraph.MeasureString(nodeText, currentWorkingOptions.TextFont);
        //    if (sizeTextName.Width <= textWidthLimit)
        //    {
        //        if (currentWorkingOptions.ShowSize)
        //        {
        //            float xName = x - sizeTextName.Width / 2f;
        //            float yName = y - sizeTextName.Height;
        //            frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), xName, yName); //, format);
        //            string nodeSize = HDGTools.FormatSize(node.TotalSize);
        //            SizeF sizeTextSize = frontGraph.MeasureString(nodeSize, currentWorkingOptions.TextFont);
        //            float xSize = x - sizeTextSize.Width / 2f;
        //            float ySize = y;
        //            // Adoucir le fond du texte :
        //            //Color colTransp = Color.FromArgb(50, Color.White);
        //            //graph.FillRectangle(new SolidBrush(colTransp),
        //            //                    xSize, ySize, sizeTextSize.Width, sizeTextSize.Height);
        //            frontGraph.DrawString(nodeSize, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), xSize, ySize); //, format);
        //        }
        //        else
        //        {
        //            x -= sizeTextName.Width / 2f;
        //            y -= sizeTextName.Height / 2f;
        //            frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), x, y); //, format);
        //        }
        //    }
        //}

        /// <summary>
        /// Dessine un semi anneau sur le graph.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        private void BuildPartialPie(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Arc arc = BuildArc(node, currentLevel, startAngle, endAngle);
            canvas1.Children.Add(arc);
            arc.MouseEnter += new MouseEventHandler(arc_MouseEnter);
            arc.MouseLeave += new MouseEventHandler(arc_MouseLeave);
            ArcToolTip arcTooltip = new ArcToolTip()
            {
                DataContext = node
            };
            arc.ToolTip = arcTooltip;

            // TODO : now, apply the correct brush.
            if (node.DirectoryType == SpecialDirTypes.NotSpecial)
            {
                // TODO
                //// standard zone
                //frontGraph.FillPie(
                //    GetBrushForAngles(rec, startAngle, nodeAngle),
                //    Rectangle.Round(rec),
                //    startAngle,
                //    nodeAngle);
                //frontGraph.DrawPie(new Pen(Color.Black, 0.05f), rec, startAngle, nodeAngle);
                //// For tests
                ////float middleAngle = startAngle + (nodeAngle / 2f);
                ////frontGraph.DrawRectangle(new Pen(colorManager.GetNextColor(middleAngle), 0.05f),
                ////                        Rectangle.Round(rec));
                Canvas.SetZIndex(arc, DEFAULT_Z_INDEX_STANDARD_ARC);
                ApplyArcLabel(node, currentLevel, startAngle, endAngle);

            }
            else if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
            {
                // TODO
                //// free space
                //frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
                //                            System.Drawing.Drawing2D.HatchStyle.Wave,
                //                            Color.LightGray,
                //                            Color.White),
                //                Rectangle.Round(rec),
                //                startAngle,
                //                nodeAngle);
            }
            else if (node.DirectoryType == SpecialDirTypes.UnknownPart)
            {
                // TODO.
                //// non-calculable files
                //frontGraph.FillPie(new System.Drawing.Drawing2D.HatchBrush(
                //                            System.Drawing.Drawing2D.HatchStyle.Trellis,
                //                            Color.Red,
                //                            Color.White),
                //                Rectangle.Round(rec),
                //                startAngle,
                //                nodeAngle);
            }
        }

        private DivideBy2NumericConverter divideBy2Converter = new DivideBy2NumericConverter(true);

        private void ApplyArcLabel(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Label label = new Label()
            {
                Content = node.Name,// + Environment.NewLine + node.HumanReadableTotalSize,
                //Height = singleLevelHeight,
                //Width = 50, // TODO : optimize ?
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalContentAlignment = VerticalAlignment.Center
            };
            canvas1.Children.Add(label);
            Canvas.SetZIndex(label, DEFAULT_Z_INDEX_ARC_CAPTION);

            // Define coordinates of the label.
            double radius = ((currentLevel + 0.5) * singleLevelHeight);
            double angleInRadian = WpfUtils.GetRadianFromDegree(startAngle + (endAngle - startAngle) / 2);
            double angleCos = Math.Cos(angleInRadian);
            double angleSin = Math.Sin(angleInRadian);
            double xLarge = angleCos * radius;
            double yLarge = angleSin * radius;
            Canvas.SetTop(label, yLarge);
            Canvas.SetLeft(label, xLarge);

            TransformGroup transformGroup = new TransformGroup();
            Binding b;

            // Apply HORIZONTAL Translation to Label (it must be centered)
            b = new Binding();
            TranslateTransform translateTransform = new TranslateTransform();
            b.Source = label;
            b.Path = new PropertyPath(Label.ActualHeightProperty);
            b.Converter = divideBy2Converter;
            BindingOperations.SetBinding(translateTransform, TranslateTransform.YProperty, b);
            transformGroup.Children.Add(translateTransform);
            // Apply VERTICAL Translation to Label (it must be centered)
            b = new Binding();
            translateTransform = new TranslateTransform();
            b.Source = label;
            b.Path = new PropertyPath(Label.ActualWidthProperty);
            b.Converter = divideBy2Converter;
            BindingOperations.SetBinding(translateTransform, TranslateTransform.XProperty, b);
            transformGroup.Children.Add(translateTransform);
            // Apply rotation to Label
            RotateTransform rotateTransform = new RotateTransform();
            b = new Binding();
            b.Source = this.sliderRotation;
            b.Path = new PropertyPath(Slider.ValueProperty);
            b.Converter = new ReverseNumericConverter(); // apply the reversed rotation to the one made on the graph.
            BindingOperations.SetBinding(rotateTransform, RotateTransform.AngleProperty, b);
            transformGroup.Children.Add(rotateTransform);
            
            label.RenderTransform = transformGroup;
        }

        private const int DEFAULT_Z_INDEX_STANDARD_ARC = 1;
        private const int DEFAULT_Z_INDEX_STANDARD_ARC_OVER = 2;
        private const int DEFAULT_Z_INDEX_ARC_CAPTION = 0;

        void arc_MouseLeave(object sender, MouseEventArgs e)
        {
            Arc arc = (Arc)sender;
            if (arc != null)
            {
                arc.path1.StrokeThickness = 1;
                Canvas.SetZIndex(arc, DEFAULT_Z_INDEX_STANDARD_ARC);
            }
        }

        void arc_MouseEnter(object sender, MouseEventArgs e)
        {
            Arc arc = (Arc)sender;
            if (arc != null)
            {
                arc.path1.StrokeThickness = 3;
                Canvas.SetZIndex(arc, DEFAULT_Z_INDEX_STANDARD_ARC_OVER);
            }
        }


        private Arc BuildArc(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Arc arc = new Arc();
            arc.BeginEdit();
            arc.StartAngle = startAngle;
            arc.StopAngle = endAngle - startAngle;
            arc.SmallRadius = Convert.ToSingle(currentLevel * singleLevelHeight);
            arc.LargeRadius = Convert.ToSingle((currentLevel + 1) * singleLevelHeight);
            arc.Node = node;
            arc.EndEdit();
            return arc;
        }

        //private Color myTransparentColor = Color.Black;

        //private Brush GetBrushForAngles(RectangleF rec, float startAngle, float nodeAngle)
        //{
        //    switch (currentWorkingOptions.ColorStyleChoice)
        //    {
        //        case ModeAffichageCouleurs.RandomNeutral:
        //        case ModeAffichageCouleurs.RandomBright:
        //            return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                            rec,
        //                            colorManager.GetNextColor(startAngle),
        //                            Color.SteelBlue,
        //                            LinearGradientMode.ForwardDiagonal
        //                        );
        //        case ModeAffichageCouleurs.Linear2:
        //            return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                            rec,
        //                            colorManager.GetNextColor(startAngle + (nodeAngle / 2f)),
        //                            Color.SteelBlue,
        //                            LinearGradientMode.ForwardDiagonal
        //                        );
        //        case ModeAffichageCouleurs.Linear:
        //            float middleAngle = startAngle + (nodeAngle / 2f);
        //            //return new System.Drawing.Drawing2D.LinearGradientBrush(
        //            //                rec,
        //            //                GetNextColor(middleAngle),
        //            //                Color.SteelBlue,
        //            //                System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
        //            //            );
        //            if (middleAngle < 90)
        //                return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                                rec,
        //                                Color.SteelBlue,
        //                                colorManager.GetNextColor(middleAngle),
        //                                LinearGradientMode.ForwardDiagonal
        //                            );
        //            else if (middleAngle < 180)
        //                return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                            rec,
        //                            Color.SteelBlue,
        //                            colorManager.GetNextColor(middleAngle),
        //                            LinearGradientMode.BackwardDiagonal
        //                        );
        //            else if (middleAngle < 270)
        //                return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                            rec,
        //                            colorManager.GetNextColor(middleAngle),
        //                            Color.SteelBlue,
        //                            LinearGradientMode.ForwardDiagonal
        //                        );
        //            else
        //                return new System.Drawing.Drawing2D.LinearGradientBrush(
        //                            rec,
        //                            colorManager.GetNextColor(middleAngle),
        //                            Color.SteelBlue,
        //                            LinearGradientMode.BackwardDiagonal
        //                        );
        //        case ModeAffichageCouleurs.ImprovedLinear:
        //        default:
        //            return new SolidBrush(myTransparentColor);
        //        //if (nodeAngle < 1)
        //        //    return new System.Drawing.Drawing2D.LinearGradientBrush(rec,
        //        //                        GetNextColor(startAngle + (nodeAngle / 2f)),
        //        //                        Color.SteelBlue,
        //        //                        System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal);
        //        //PointF p1 = new PointF();
        //        //p1.X = rec.Left + rec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle))) * rec.Height / 2f;
        //        //p1.Y = rec.Top + rec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle))) * rec.Height / 2f;
        //        //PointF p2 = new PointF();
        //        //p2.X = rec.Left + rec.Width / 2f + Convert.ToSingle(Math.Cos(GetRadianFromDegree(startAngle + nodeAngle))) * rec.Height / 2f;
        //        //p2.Y = rec.Top + rec.Height / 2f + Convert.ToSingle(Math.Sin(GetRadianFromDegree(startAngle + nodeAngle))) * rec.Height / 2f;
        //        //if (nodeAngle == 360)
        //        //    p2.X = -p2.X;
        //        //try
        //        //{
        //        //    return new System.Drawing.Drawing2D.LinearGradientBrush(
        //        //                    p1, p2,
        //        //                    GetNextColor(startAngle),
        //        //                    GetNextColor(startAngle + nodeAngle)
        //        //                );
        //        //}
        //        //catch (Exception ex)
        //        //{
        //        //    throw;
        //        //}
        //    }

        //}


        /// <summary>
        /// A l'image de PaintDirPart, génère l'arc de cercle correspondant aux fichiers d'un répertoire.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void BuildFilesPart(int currentLevel, float startAngle, float endAngle)
        {
            //if (!printDirNames && treeGraph.OptionAlsoPaintFiles)
            //{
            //    float nodeAngle = endAngle - startAngle;
            //    rec.Inflate(singleLevelHeight, singleLevelHeight);
            //    //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            //    frontGraph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO

            //}
        }

        ///// <summary>
        ///// Trouve quel est le répertoire survolé d'après la position du curseur.
        ///// (Recherche par coordonnées cartésiennes).
        ///// </summary>
        ///// <param name="curseurPos">Position du curseur. Doit être relative au contrôle, pas à l'écran ou à la form !</param>
        ///// <returns></returns>
        //public override IDirectoryNode FindNodeByCursorPosition(Point curseurPos)
        //{
        //    // On a les coordonnées du curseur dans le controle.
        //    // Il faut faire un changement de référentiel pour avoir les coordonnées vis à vis de l'origine (le centre des cercles).
        //    curseurPos.X -= latestUsedOptions.BitmapSize.Width / 2;
        //    curseurPos.Y -= latestUsedOptions.BitmapSize.Height / 2;
        //    // On a maintenant les coordonnées vis-à-vis du centre des cercles.
        //    //System.Windows.Forms.MessageBox.Show(curseurPos.ToString());

        //    // Cherchons l'angle formé par le curseur et la taille du rayon jusqu'à celui-ci.
        //    double angle = MathHelper.GetDegreeFromRadian(Math.Atan(-curseurPos.Y / (double)curseurPos.X));
        //    // l'angle obtenu à corriger en fonction du quartier où se situe le curseur
        //    if (curseurPos.X < 0)
        //        angle = 180 - angle;
        //    else
        //        angle = (curseurPos.Y < 0) ? 360 - angle : -angle;

        //    angle -= latestUsedOptions.ImageRotation;
        //    if (angle < 0)
        //        angle += 360;
        //    double rayon = Math.Sqrt(Math.Pow(curseurPos.X, 2) + Math.Pow(curseurPos.Y, 2));
        //    //System.Windows.Forms.MessageBox.Show("angle: " + angle + "; rayon: " + rayon);
        //    if (this.rootNode == null || this.rootNode.TotalSize == 0)
        //        return this.rootNode;
        //    IDirectoryNode foundNode = FindNodeInTree(
        //                this.rootNode,
        //                0,
        //                0,
        //                360,
        //                angle,
        //                rayon);
        //    return foundNode;
        //}

        ///// <summary>
        ///// Recherche quel est le répertoire dans lequel se trouve le point définit par l'angle cursorAngle et la distance cursorLen.
        ///// (Recherche par coordonnées polaires).
        ///// </summary>
        ///// <param name="node"></param>
        ///// <param name="levelHeight"></param>
        ///// <param name="startAngle"></param>
        ///// <param name="endAngle"></param>
        ///// <param name="cursorAngle"></param>
        ///// <param name="cursorLen"></param>
        ///// <returns></returns>
        //private IDirectoryNode FindNodeInTree(IDirectoryNode node, float levelHeight, float startAngle, float endAngle, double cursorAngle, double cursorLen)
        //{
        //    if (node.TotalSize == 0)
        //        return node;
        //    float nodeAngle = endAngle - startAngle;
        //    levelHeight += singleLevelHeight;
        //    if (levelHeight > cursorLen && cursorAngle >= startAngle && cursorAngle <= endAngle)
        //    {
        //        // le noeud courant est celui recherché
        //        if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndHide)
        //            return null;
        //        return node;
        //    }
        //    long cumulSize = 0;
        //    float currentStartAngle;
        //    foreach (IDirectoryNode childNode in node.Children)
        //    {
        //        currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
        //        float childAngle = childNode.TotalSize * nodeAngle / node.TotalSize;
        //        if (cursorLen > levelHeight && cursorAngle >= currentStartAngle && cursorAngle <= (currentStartAngle + childAngle))
        //            return FindNodeInTree(childNode, levelHeight, currentStartAngle, currentStartAngle + childAngle, cursorAngle, cursorLen);
        //        cumulSize += childNode.TotalSize;
        //    }
        //    currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
        //    return null;
        //}
    }
}
