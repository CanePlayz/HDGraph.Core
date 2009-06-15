using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using HDGraph.Interfaces.ScanEngines;

namespace HDGraph.DrawEngine
{
    internal class CircularImageGraphGenerator : ImageGraphGeneratorBase
    {
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

        private HDGraphScanEngineBase moteur;
        private DrawOptions currentWorkingOptions;
        private DrawOptions latestUsedOptions;
        private ColorManager colorManager;
        private IDirectoryNode rootNode;

        public CircularImageGraphGenerator(IDirectoryNode rootNode, HDGraphScanEngineBase moteur)
        {
            this.moteur = moteur;
            this.colorManager = new ColorManager();
            this.rootNode = rootNode;
        }

        public override BiResult<Bitmap, DrawOptions> Draw(bool drawImage, bool drawText, DrawOptions options)
        {
            // only 1 execution allowed at a time. To do multiple executions, build a new 
            // instance of ImageGraphGenerator.
            lock (this)
            {
                // Création du bitmap buffer
                currentWorkingOptions = options;
                colorManager.SetOptions(currentWorkingOptions);
                Bitmap backBufferTmp = new Bitmap(currentWorkingOptions.BitmapSize.Width, currentWorkingOptions.BitmapSize.Height);
                frontGraph = Graphics.FromImage(backBufferTmp);

                if (!drawText)
                    frontGraph.Clear(Color.White);
                else
                    frontGraph.Clear(Color.Transparent);
                frontGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                frontGraph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                // init des données du calcul
                pasNiveau = Math.Min(currentWorkingOptions.BitmapSize.Width / (float)currentWorkingOptions.ShownLevelsCount / 2,
                                     currentWorkingOptions.BitmapSize.Height / (float)currentWorkingOptions.ShownLevelsCount / 2);
                RectangleF pieRec = new RectangleF(currentWorkingOptions.BitmapSize.Width / 2f,
                                        currentWorkingOptions.BitmapSize.Height / 2f,
                                        0,
                                        0);

                PaintTree(pieRec, drawImage, drawText);
                frontGraph.Dispose();
                latestUsedOptions = currentWorkingOptions;
                return new BiResult<Bitmap, DrawOptions>()
                        {
                            Obj1 = backBufferTmp,
                            Obj2 = currentWorkingOptions
                        };
            }
        }

        /// <summary>
        /// Effectue le premier lancement de la méthode PaintTree récursive.
        /// </summary>
        private void PaintTree(RectangleF pieRec, bool drawImage, bool drawText)
        {
            if (rootNode == null || rootNode.TotalSize == 0)
            {
                if (drawText)
                    PaintSpecialCase();
                return;
            }
            if (drawImage)
            {
                printDirNames = false;
                PaintTree(rootNode, pieRec, currentWorkingOptions.ImageRotation, 360 + currentWorkingOptions.ImageRotation);
            }
            if (drawText)
            {
                printDirNames = true;
                PaintTree(rootNode, pieRec, currentWorkingOptions.ImageRotation, 360 + currentWorkingOptions.ImageRotation);
            }
        }

        /// <summary>
        /// Affiche un message spécifique au lieu du graph.
        /// </summary>
        private void PaintSpecialCase()
        {
            string text;
            if (moteur != null && moteur.WorkCanceled)
                text = Resources.ApplicationMessages.UserCanceledAnalysis;
            else if (rootNode != null && rootNode.TotalSize == 0)
                text = Resources.ApplicationMessages.FolderIsEmpty;
            else
                text = Resources.ApplicationMessages.GraphGuideLine;

            DrawHelper.PrintTextInTheMiddle(frontGraph, currentWorkingOptions.BitmapSize, text, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), false);
        }

        
        /// <summary>
        /// Procédure récursive pour graphiquer les arcs de cercle. Graphique de l'extérieur vers l'intérieur.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintTree(IDirectoryNode node, RectangleF rec, float startAngle, float endAngle)
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
                foreach (IDirectoryNode childNode in node.Children)
                {
                    if (childNode.DirectoryType != SpecialDirTypes.FreeSpaceAndHide)
                    {
                        currentStartAngle = startAngle + cumulSize * nodeAngle / node.TotalSize;
                        float childAngle = childNode.TotalSize * nodeAngle / node.TotalSize;
                        PaintTree(childNode, rec, currentStartAngle, currentStartAngle + childAngle);
                        cumulSize += childNode.TotalSize;
                    }
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
        private void PaintUnknownPart(IDirectoryNode node, RectangleF rec, float startAngle, float endAngle)
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
        private void PaintDirPart(IDirectoryNode node, RectangleF rec, float startAngle, float nodeAngle)
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
                    // on écrit les noms de répertoire uniquement
                    WriteDirectoryNameForFullPie(node, rec);
                }
            }
            else
            {
                if (!printDirNames)
                {
                    // on dessine le disque uniquement
                    DrawPartialPie(node, rec, startAngle, nodeAngle);
                }
                else if (nodeAngle > currentWorkingOptions.TextDensity)
                {
                    // on dessine les noms de répertoire uniquement (si l'angle est supérieur à 10°)
                    WriteDirectoryName(node, rec, startAngle, nodeAngle);
                }

            }
        }

        /// <summary>
        /// Dessine le nom d'un répertoire sur le graph, lorsque ce répertoire a un angle de 360°.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <returns></returns>
        private void WriteDirectoryNameForFullPie(IDirectoryNode node, RectangleF rec)
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
            x += currentWorkingOptions.BitmapSize.Width / 2f;
            y += currentWorkingOptions.BitmapSize.Height / 2f;
            string nodeText = node.Name;
            if (currentWorkingOptions.ShowSize)
                nodeText += Environment.NewLine + HDGTools.FormatSize(node.TotalSize);

            SizeF size = frontGraph.MeasureString(nodeText, currentWorkingOptions.TextFont);
            x -= size.Width / 2f;
            y -= size.Height / 2f;
            // Adoucir le fond du texte :
            Color colTransp = Color.FromArgb(100, Color.White);
            frontGraph.FillRectangle(new SolidBrush(colTransp),
                                x, y, size.Width, size.Height);
            frontGraph.DrawRectangle(new Pen(Color.Black), x, y, size.Width, size.Height);
            frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), x, y);
        }

        /// <summary>
        /// Dessine le nom d'un répertoire sur le graph.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        /// <returns></returns>
        private void WriteDirectoryName(IDirectoryNode node, RectangleF rec, float startAngle, float nodeAngle)
        {
            //float textWidthLimit = pasNiveau * 1.5f;
            float textWidthLimit = pasNiveau * 2f;
            float x, y, angleCentre, hyp;
            hyp = (rec.Width - pasNiveau) / 2f;
            angleCentre = startAngle + nodeAngle / 2f;
            x = (float)Math.Cos(MathHelper.GetRadianFromDegree(angleCentre)) * hyp;
            y = (float)Math.Sin(MathHelper.GetRadianFromDegree(angleCentre)) * hyp;
            x += currentWorkingOptions.BitmapSize.Width / 2f;
            y += currentWorkingOptions.BitmapSize.Height / 2f;
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            string nodeText = node.Name;
            SizeF sizeTextName = frontGraph.MeasureString(nodeText, currentWorkingOptions.TextFont);
            if (sizeTextName.Width <= textWidthLimit)
            {
                if (currentWorkingOptions.ShowSize)
                {
                    float xName = x - sizeTextName.Width / 2f;
                    float yName = y - sizeTextName.Height;
                    frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), xName, yName); //, format);
                    string nodeSize = HDGTools.FormatSize(node.TotalSize);
                    SizeF sizeTextSize = frontGraph.MeasureString(nodeSize, currentWorkingOptions.TextFont);
                    float xSize = x - sizeTextSize.Width / 2f;
                    float ySize = y;
                    // Adoucir le fond du texte :
                    //Color colTransp = Color.FromArgb(50, Color.White);
                    //graph.FillRectangle(new SolidBrush(colTransp),
                    //                    xSize, ySize, sizeTextSize.Width, sizeTextSize.Height);
                    frontGraph.DrawString(nodeSize, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), xSize, ySize); //, format);
                }
                else
                {
                    x -= sizeTextName.Width / 2f;
                    y -= sizeTextName.Height / 2f;
                    frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), x, y); //, format);
                }
            }
        }

        /// <summary>
        /// Dessine un semi anneau sur le graph.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        private void DrawPartialPie(IDirectoryNode node, RectangleF rec, float startAngle, float nodeAngle)
        {
            if (node.DirectoryType == SpecialDirTypes.NotSpecial)
            {
                // standard zone
                frontGraph.FillPie(
                    GetBrushForAngles(rec, startAngle, nodeAngle),
                    Rectangle.Round(rec),
                    startAngle,
                    nodeAngle);
                frontGraph.DrawPie(new Pen(Color.Black, 0.05f), rec, startAngle, nodeAngle);
                // For tests
                //float middleAngle = startAngle + (nodeAngle / 2f);
                //frontGraph.DrawRectangle(new Pen(colorManager.GetNextColor(middleAngle), 0.05f),
                //                        Rectangle.Round(rec));
            }
            else if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
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
            else if (node.DirectoryType == SpecialDirTypes.UnknownPart)
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
        }

        private Color myTransparentColor = Color.Black;

        private Brush GetBrushForAngles(RectangleF rec, float startAngle, float nodeAngle)
        {
            switch (currentWorkingOptions.ColorStyleChoice)
            {
                case ModeAffichageCouleurs.RandomNeutral:
                case ModeAffichageCouleurs.RandomBright:
                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    colorManager.GetNextColor(startAngle),
                                    Color.SteelBlue,
                                    LinearGradientMode.ForwardDiagonal
                                );
                case ModeAffichageCouleurs.Linear2:
                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    colorManager.GetNextColor(startAngle + (nodeAngle / 2f)),
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
                                        colorManager.GetNextColor(middleAngle),
                                        LinearGradientMode.ForwardDiagonal
                                    );
                    else if (middleAngle < 180)
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    Color.SteelBlue,
                                    colorManager.GetNextColor(middleAngle),
                                    LinearGradientMode.BackwardDiagonal
                                );
                    else if (middleAngle < 270)
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    colorManager.GetNextColor(middleAngle),
                                    Color.SteelBlue,
                                    LinearGradientMode.ForwardDiagonal
                                );
                    else
                        return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    rec,
                                    colorManager.GetNextColor(middleAngle),
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
        /// A l'image de PaintDirPart, génère l'arc de cercle correspondant aux fichiers d'un répertoire.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintFilesPart(RectangleF rec, float startAngle, float endAngle)
        {
            //if (!printDirNames && treeGraph.OptionAlsoPaintFiles)
            //{
            //    float nodeAngle = endAngle - startAngle;
            //    rec.Inflate(pasNiveau, pasNiveau);
            //    //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            //    frontGraph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO

            //}
        }




        /// <summary>
        /// Trouve quel est le répertoire survolé d'après la position du curseur.
        /// (Recherche par coordonnées cartésiennes).
        /// </summary>
        /// <param name="curseurPos">Position du curseur. Doit être relative au contrôle, pas à l'écran ou à la form !</param>
        /// <returns></returns>
        public override IDirectoryNode FindNodeByCursorPosition(Point curseurPos)
        {
            // On a les coordonnées du curseur dans le controle.
            // Il faut faire un changement de référentiel pour avoir les coordonnées vis à vis de l'origine (le centre des cercles).
            curseurPos.X -= latestUsedOptions.BitmapSize.Width / 2;
            curseurPos.Y -= latestUsedOptions.BitmapSize.Height / 2;
            // On a maintenant les coordonnées vis-à-vis du centre des cercles.
            //System.Windows.Forms.MessageBox.Show(curseurPos.ToString());

            // Cherchons l'angle formé par le curseur et la taille du rayon jusqu'à celui-ci.
            double angle = MathHelper.GetDegreeFromRadian(Math.Atan(-curseurPos.Y / (double)curseurPos.X));
            // l'angle obtenu à corriger en fonction du quartier où se situe le curseur
            if (curseurPos.X < 0)
                angle = 180 - angle;
            else
                angle = (curseurPos.Y < 0) ? 360 - angle : -angle;

            angle -= latestUsedOptions.ImageRotation;
            if (angle < 0)
                angle += 360;
            double rayon = Math.Sqrt(Math.Pow(curseurPos.X, 2) + Math.Pow(curseurPos.Y, 2));
            //System.Windows.Forms.MessageBox.Show("angle: " + angle + "; rayon: " + rayon);
            if (this.rootNode == null || this.rootNode.TotalSize == 0)
                return this.rootNode;
            IDirectoryNode foundNode = FindNodeInTree(
                        this.rootNode,
                        0,
                        0,
                        360,
                        angle,
                        rayon);
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
        private IDirectoryNode FindNodeInTree(IDirectoryNode node, float levelHeight, float startAngle, float endAngle, double cursorAngle, double cursorLen)
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
            foreach (IDirectoryNode childNode in node.Children)
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

    }
}
