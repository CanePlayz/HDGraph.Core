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
using System.Windows.Media.Animation;
using System.IO;

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
            CommandManager.RegisterClassCommandBinding(
                typeof(TreeGraph),
                new CommandBinding(
                    NavigationCommands.BrowseBack,
                    BrowseBackCommand_Executed));
            CommandManager.RegisterClassCommandBinding(
                typeof(TreeGraph),
                new CommandBinding(
                    NavigationCommands.BrowseForward,
                    BrowseForwardCommand_Executed));
        }

        public IActionExecutor ActionExecutor { get; set; }

        public bool IsRotating
        {
            get { return (bool)GetValue(IsRotatingProperty); }
            set { SetValue(IsRotatingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsRotating.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsRotatingProperty =
            DependencyProperty.Register("IsRotating", typeof(bool), typeof(TreeGraph), new UIPropertyMetadata(false));



        /// <summary>
        /// Epaisseur d'un niveau sur le graph.
        /// </summary>
        private double singleLevelHeight;

        public double SingleLevelHeight
        {
            get { return (double)GetValue(SingleLevelHeightProperty); }
            set { SetValue(SingleLevelHeightProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SingleLevelHeight.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SingleLevelHeightProperty =
            DependencyProperty.Register("SingleLevelHeight", typeof(double), typeof(TreeGraph), new UIPropertyMetadata(0d));




        public DrawOptions CurrentDrawOptions
        {
            get { return (DrawOptions)GetValue(CurrentDrawOptionsProperty); }
            set { SetValue(CurrentDrawOptionsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentDrawOptions.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentDrawOptionsProperty =
            DependencyProperty.Register("CurrentDrawOptions", typeof(DrawOptions), typeof(TreeGraph), new UIPropertyMetadata(null));


        private IDirectoryNode rootNode;

        public void FullRefresh()
        {
            SetRoot(rootNode, CurrentDrawOptions);
        }

        public void SetRoot(IDirectoryNode root, DrawOptions options)
        {
            if (root == null || options == null)
                return;

            this.rootNode = root;
            sliderScale.Value = 1;
            canvas1.Children.Clear();

            // Création du bitmap buffer
            CurrentDrawOptions = options;

            // init des données du calcul
            //SingleLevelHeight = Convert.ToDouble(
            //                Math.Min(this.Width / currentWorkingOptions.ShownLevelsCount / 2,
            //                         this.Height / currentWorkingOptions.ShownLevelsCount / 2));

            // init des données du calcul

            double largeur = Math.Min(scrollViewer1.ActualWidth, scrollViewer1.ActualHeight);
            largeur -= 30; // keep space for last level effects, such as "hidden folder" effect. 
            singleLevelHeight = Convert.ToDouble(largeur / (CurrentDrawOptions.ShownLevelsCount) / 2d);
            SingleLevelHeight = singleLevelHeight; // Set the DP. Using a variable instead of the DP Getter increases perfs.

            labelInfo.Visibility = Visibility.Hidden;
            if (rootNode == null || rootNode.TotalSize == 0)
            {
                PaintSpecialCase();
                return;
            }
            BuildTree(rootNode, 0, 0, 360);
            ActionExecutor.Notify4NewRootNode(root);
        }


        private void BrowseBackCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ActionExecutor.NavigateBackward();
        }

        private void BrowseForwardCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ActionExecutor.NavigateForward();
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
            arc.path1.Style = (Style)FindResource("HiddenFoldersArcStyle");
            arc.path1.StrokeThickness = 0;
            arc.ToolTip = String.Format(Properties.Resources.HiddenFolders,
                                        Environment.NewLine + Environment.NewLine,
                                        node.Name);
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
            arc.ToolTip = String.Format(Properties.Resources.MultipleSmallFolders,
                                        Environment.NewLine + Environment.NewLine,
                                        node.Name);
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
            Arc arc = BuildArc(node, currentLevel, startAngle, endAngle);
            arc.DataContext = node;
            arc.Style = (Style)FindResource("StandardArcStyle");
            canvas1.Children.Add(arc);
            arc.MouseEnter += new MouseEventHandler(arc_MouseEnter);
            arc.MouseLeave += new MouseEventHandler(arc_MouseLeave);
            arc.MouseDoubleClick += new MouseButtonEventHandler(arc_MouseDoubleClick);
            ArcToolTip arcTooltip = new ArcToolTip()
            {
                DataContext = node
            };
            arc.ToolTip = arcTooltip;


            Binding b = new Binding()
            {
                Source = rotateTransform,
                Path = new PropertyPath(RotateTransform.AngleProperty),
            };
            BindingOperations.SetBinding(arc, Arc.TextRotationProperty, b);

            b = new Binding()
            {
                Source = sliderTextSize,
                Path = new PropertyPath(Slider.ValueProperty),
                Mode = BindingMode.OneWay,
            };
            BindingOperations.SetBinding(arc, Arc.FontSizeProperty, b);





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
                // TODO : 
                //Canvas.SetZIndex(arc, DEFAULT_Z_INDEX_STANDARD_ARC);
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

        #region Doubleclick on a node

        void arc_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Arc arc = (Arc)sender;
                IDirectoryNode node = arc.Node;
                if (node != null)
                {
                    if (Object.ReferenceEquals(node, this.rootNode))
                    {
                        // Go to parent
                        SetRoot(node.Parent, CurrentDrawOptions);
                    }
                    else
                    {
                        // Center graph on the clicked node.
                        CenterGraphOnArc(sender, e, arc);
                    }
                }
            }
        }

        private void CenterGraphOnArc(object sender, EventArgs e, Arc arc)
        {
            IDirectoryNode node = arc.Node;
            // Apply animation : existing children disappear.
            Storyboard fadeStoryboard = TryFindResource("FadeOutStoryboard") as Storyboard;
            ApplyAnimationToChildren(fadeStoryboard, arc);

            // Complete the graph if data is missing
            ActionExecutor.ExecuteTreeFillUpToLevel(node, CurrentDrawOptions.ShownLevelsCount);
            // Apply animation to center graph to the new root
            Storyboard centerArcStoryboard = TryFindResource("CenterArcStoryboard") as Storyboard;
            SingleAnimation largeRadiusAnimation = centerArcStoryboard.Children[2] as SingleAnimation;
            newRootNode = node;
            if (largeRadiusAnimation == null || Storyboard.GetTargetProperty(largeRadiusAnimation).Path != "LargeRadius")
            {
                // invalid animation : don't launch the animation.
                Storyboard_Completed(sender, e);
            }
            else
            {
                largeRadiusAnimation.To = Convert.ToSingle(singleLevelHeight);
                // The new root will be set at the end of the animation, in method Storyboard_Completed.
                centerArcStoryboard.Begin(arc);
            }
        }

        /// <summary>
        /// Ater a double clic on a node, the node is stored here in order to 
        /// define it as root, as soon as the animation is finished.
        /// </summary>
        private IDirectoryNode newRootNode;

        private void Storyboard_Completed(object sender, EventArgs e)
        {
            if (newRootNode == null)
                return;
            SetRoot(newRootNode, CurrentDrawOptions);
            newRootNode = null;
        }

        private void ApplyAnimationToChildren(Storyboard storyboard, Object exceptElement)
        {
            foreach (UIElement child in canvas1.Children)
            {
                if (object.ReferenceEquals(child, exceptElement))
                    continue;
                FrameworkElement childElem = child as FrameworkElement;
                if (childElem != null)
                    storyboard.Begin(childElem);//, true); ?
            }
        }
        #endregion


        private DivideBy2NumericConverter divideBy2Converter = new DivideBy2NumericConverter(true);

        void arc_MouseLeave(object sender, MouseEventArgs e)
        {
            Arc arc = (Arc)sender;
            if (arc != null)
            {
                ActionExecutor.Notify4NewHoveredNode(null);
                Cursor = standardCursor;
            }
        }

        private Cursor standardCursor;

        void arc_MouseEnter(object sender, MouseEventArgs e)
        {
            Arc arc = (Arc)sender;
            if (arc != null)
            {
                if (Cursor != Cursors.Hand)
                {
                    standardCursor = Cursor;
                    Cursor = Cursors.Hand;
                }
                ActionExecutor.Notify4NewHoveredNode(arc.Node);
            }
        }


        private Arc BuildArc(IDirectoryNode node, int currentLevel, float startAngle, float endAngle)
        {
            Arc arc = new Arc();
            arc.ContextMenuOpening += new ContextMenuEventHandler(arc_ContextMenuOpening);
            arc.DrawOptions = this.CurrentDrawOptions;
            arc.ContextMenu = (ContextMenu)FindResource("StandardContextMenu");
            arc.BeginEdit();
            arc.StartAngle = startAngle;
            arc.StopAngle = endAngle - startAngle;
            arc.SmallRadius = Convert.ToSingle(currentLevel * singleLevelHeight);
            arc.LargeRadius = Convert.ToSingle((currentLevel + 1) * singleLevelHeight);
            arc.Node = node;
            arc.EndEdit();
            return arc;
        }

        void arc_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            // TODO
            //Arc arc = sender as Arc;
            //if (arc == null)
            //    return;
            //IDirectoryNode selectedNode = arc.Node;
            //e.Handled = true;
            //Point p = arc.PointToScreen(new System.Windows.Point(e.CursorLeft, e.CursorTop));
            //ActionExecutor.ShowContextMenu(new NodeContextEventArgs()
            //    {
            //        Position = new System.Drawing.PointF(Convert.ToSingle(p.X), Convert.ToSingle(p.Y)),
            //        Node = selectedNode,
            //    }
            //);
        }


        private void contextMenu1_Opened(object sender, RoutedEventArgs e)
        {

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
            //    rec.Inflate(SingleLevelHeight, SingleLevelHeight);
            //    //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            //    frontGraph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO

            //}
        }


        private void ButtonPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog dialog = new PrintDialog();
            if (dialog.ShowDialog() == true)
            {
                dialog.PrintVisual(canvas1, "HDGraph diagram");
            }

        }

        private Point? initialCursorLocation;
        private double initialRotationAngle;

        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                initialCursorLocation = e.MouseDevice.GetPosition(canvasContainer);
                e.Handled = true;
                initialRotationAngle = rotateTransform.Angle;
                IsRotating = true;
                if (!canvas1.CaptureMouse())
                    initialCursorLocation = null;

            }
        }

        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (initialCursorLocation != null)
            {
                initialCursorLocation = null;
                e.Handled = true;
                canvas1.ReleaseMouseCapture();
                IsRotating = false;
            }
        }

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            if (initialCursorLocation == null || e.LeftButton != MouseButtonState.Pressed)
                return;
            Point newPoint = e.MouseDevice.GetPosition(canvasContainer);
            Vector centerPoint = new Vector(canvasContainer.ActualWidth / 2, canvasContainer.ActualHeight / 2);
            Vector initVector = new Vector(initialCursorLocation.Value.X, initialCursorLocation.Value.Y);
            Vector newVector = new Vector(newPoint.X, newPoint.Y);
            double rotationAngle = Vector.AngleBetween(initVector - centerPoint, newVector - centerPoint);
            rotationAngle = (rotationAngle + initialRotationAngle) % 360;
            if (rotationAngle < 0)
                rotationAngle = rotationAngle + 360;

            CurrentDrawOptions.ImageRotation = Convert.ToSingle(rotationAngle);
            //rotateTransform.Angle = rotationAngle;
            e.Handled = true;
            //labelStatus.Content = "rotationAngle:" + rotationAngle + " initVector:" + initVector + " newVector:" + newVector + " centerPoint:" + centerPoint;
        }

        private void treeGraph1_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
        }

        private void treeGraph1_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }

        private void grid1_MouseWheel(object sender, MouseWheelEventArgs e)
        {

        }


        public void SaveAsImageToFile(string filePath)
        {
            Visual theVisual = canvas1;
            double width = Convert.ToDouble(theVisual.GetValue(FrameworkElement.WidthProperty));
            double height = Convert.ToDouble(theVisual.GetValue(FrameworkElement.HeightProperty));
            if (double.IsNaN(width) || double.IsNaN(height))
            {
                throw new FormatException("Width or Height of the UIElement is not valid.");
            }
            int quality = 2; // quality : from 1 to n...
            RenderTargetBitmap render = new RenderTargetBitmap(
                  Convert.ToInt32(width) * quality,
                  Convert.ToInt32(height) * quality,
                  96 * quality,
                  96 * quality,
                  PixelFormats.Pbgra32);
            // Indicate which control to render in the image
            render.Render(theVisual);
            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                PngBitmapEncoder encoder = new PngBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(render));
                encoder.Save(stream);
            }
        }

        private void showDetailsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Arc arcTarget = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Arc;
            if (ActionExecutor != null && arcTarget != null)
                ActionExecutor.ShowNodeDetails(arcTarget.Node);
        }

        private void openExternalMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Arc arcTarget = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Arc;
            if (ActionExecutor != null && arcTarget != null)
                ActionExecutor.OpenInExplorer(arcTarget.Node);
        }

        private void refreshMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Arc arcTarget = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Arc;
            if (ActionExecutor != null && arcTarget != null)
            {
                ActionExecutor.ExecuteTreeFullRefresh(arcTarget.Node);
                FullRefresh();
            }
        }

        private void centerOnDirMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Arc arcTarget = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Arc;
            if (ActionExecutor != null && arcTarget != null)
                CenterGraphOnArc(sender, e, arcTarget);
        }

        private void deleteMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Arc arcTarget = ((ContextMenu)((MenuItem)sender).Parent).PlacementTarget as Arc;
            if (ActionExecutor != null && arcTarget != null)
                ActionExecutor.DeleteNode(arcTarget.Node);
        }
    }
}
