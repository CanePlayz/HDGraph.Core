using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using HDGraph.Interfaces.ScanEngines;
using HDGraph.Interfaces.DrawEngines;

namespace HDGraph.DrawEngine
{
    public class RectangularImageGraphGenerator : ImageGraphGeneratorBase
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

        public RectangularImageGraphGenerator(IDirectoryNode rootNode, HDGraphScanEngineBase moteur)
        {
            this.moteur = moteur;
            this.colorManager = new ColorManager();
            this.rootNode = rootNode;
        }



        public override BiResult<System.Drawing.Bitmap, DrawOptions> Draw(bool drawImage, bool drawText, DrawOptions options)
        {
            // only 1 execution allowed at a time. To do multiple executions, build a new 
            // instance of ImageGraphGenerator.
            lock (this)
            {
                // Création du bitmap buffer
                currentWorkingOptions = options;
                colorManager.SetOptions(currentWorkingOptions);
                Bitmap backBufferTmp = new Bitmap(currentWorkingOptions.TargetSize.Width, currentWorkingOptions.TargetSize.Height);
                frontGraph = Graphics.FromImage(backBufferTmp);

                if (!drawText)
                    frontGraph.Clear(Color.White);
                else
                    frontGraph.Clear(Color.Transparent);
                //frontGraph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //frontGraph.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                // init des données du calcul
                int marginUp = 30;
                pasNiveau = (currentWorkingOptions.TargetSize.Height - marginUp) / (float)currentWorkingOptions.ShownLevelsCount;
                RectangleF pieRec = new RectangleF(0, 0,
                                        currentWorkingOptions.TargetSize.Width,
                                        pasNiveau);
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
                PaintTree(rootNode, pieRec);
            }
            if (drawText)
            {
                printDirNames = true;
                PaintTree(rootNode, pieRec);
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

            DrawHelper.PrintTextInTheMiddle(frontGraph, currentWorkingOptions.TargetSize, text, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), false);
        }

        /// <summary>
        /// Procédure récursive pour graphiquer les arcs de cercle. Graphique de l'extérieur vers l'intérieur.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintTree(IDirectoryNode node, RectangleF rectangle)
        {
            if (node.TotalSize == 0)
                return;
            if (node.ExistsUncalcSubDir)
            {
                rectangle.Height += (pasNiveau / 10f);
                PaintUnknownPart(node, rectangle);
                rectangle.Height -= (pasNiveau / 10f);
            }
            else
            {
                long cumulSize = 0;
                float xCurrent;
                foreach (DirectoryNode childNode in node.Children)
                {
                    if (childNode.DirectoryType != SpecialDirTypes.FreeSpaceAndHide)
                    {
                        xCurrent = rectangle.Left + cumulSize * rectangle.Width / node.TotalSize;
                        float childWidth = childNode.TotalSize * rectangle.Width / node.TotalSize;
                        PaintTree(childNode, new RectangleF(xCurrent, 0, childWidth, rectangle.Height + pasNiveau));
                        cumulSize += childNode.TotalSize;
                    }
                }

                float filesWidth = rectangle.Width - (cumulSize * rectangle.Width / node.TotalSize);
                xCurrent = rectangle.Left + filesWidth;
                if (node.Children.Count > 0 && node.FilesSize > 0)
                    PaintFilesPart(new RectangleF(xCurrent, 0, filesWidth, rectangle.Height + pasNiveau / 6f));
            }
            PaintDirPart(node, rectangle);
        }


        /// <summary>
        /// Dessine sur l'objet "graph" l'arc de cercle représentant une partie "inconnue" (confettis)
        /// d'un répertoire.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintUnknownPart(IDirectoryNode node, RectangleF targetRec)
        {
            if (!printDirNames)
            {
                Brush brush = new System.Drawing.Drawing2D.HatchBrush(
                                            System.Drawing.Drawing2D.HatchStyle.LargeConfetti,
                                            Color.Gray,
                                            Color.White);
                frontGraph.FillRectangle(brush, targetRec);
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
        private void PaintDirPart(IDirectoryNode node, RectangleF targetRec)
        {

            if (!printDirNames)
            {
                // on dessine le disque uniquement
                DrawRectangle(node, targetRec);
            }
            else if (targetRec.Width > currentWorkingOptions.TextDensity)
            {
                // on dessine les noms de répertoire uniquement (si l'angle est supérieur à 10°)
                WriteDirectoryName(node, new RectangleF(targetRec.X, targetRec.Height - pasNiveau, targetRec.Width, pasNiveau));
            }
        }

        /// <summary>
        /// Dessine le nom d'un répertoire sur le graph.
        /// </summary>
        /// <param name="node"></param>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="nodeAngle"></param>
        /// <returns></returns>
        private void WriteDirectoryName(IDirectoryNode node, RectangleF targetRec)
        {
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Center;
            string nodeText = node.Name;
            if (currentWorkingOptions.ShowSize)
            {
                nodeText += Environment.NewLine + HDGTools.FormatSize(node.TotalSize);
            }
            SizeF sizeTextName = frontGraph.MeasureString(nodeText, currentWorkingOptions.TextFont);
            
            if (sizeTextName.Height <= pasNiveau)
            {
                float x = targetRec.X + (targetRec.Width - sizeTextName.Width) / 2;
                float y = targetRec.Y + (targetRec.Height - sizeTextName.Height) / 2;

                Color colTransp = Color.FromArgb(100, Color.White);
                frontGraph.FillRectangle(new SolidBrush(colTransp),
                                    x, y, sizeTextName.Width, sizeTextName.Height);
                frontGraph.DrawRectangle(new Pen(Color.Gray), x, y, sizeTextName.Width, sizeTextName.Height);

                frontGraph.DrawString(nodeText, currentWorkingOptions.TextFont, new SolidBrush(Color.Black), x, y);
            }
        }

        private void DrawRectangle(IDirectoryNode node, RectangleF targetRec)
        {
            if (node.DirectoryType == SpecialDirTypes.NotSpecial)
            {
                // standard zone
                Brush brush = GetBrushForAngles(targetRec);
                frontGraph.FillRectangle(brush, targetRec);
                frontGraph.DrawRectangle(new Pen(Color.Black), targetRec.X, targetRec.Y, targetRec.Width, targetRec.Height);
            }
            else if (node.DirectoryType == SpecialDirTypes.FreeSpaceAndShow)
            {
                // free space
                Brush brush = new System.Drawing.Drawing2D.HatchBrush(
                                            System.Drawing.Drawing2D.HatchStyle.Wave,
                                            Color.LightGray,
                                            Color.White);
                frontGraph.FillRectangle(brush, targetRec);
            }
            else if (node.DirectoryType == SpecialDirTypes.UnknownPart)
            {
                // non-calculable files
                Brush brush = new System.Drawing.Drawing2D.HatchBrush(
                                            System.Drawing.Drawing2D.HatchStyle.Trellis,
                                            Color.Red,
                                            Color.White);
                frontGraph.FillRectangle(brush, targetRec);
            }
        }

        private Brush GetBrushForAngles(RectangleF targetRec)
        {
            float xDepart = targetRec.Left;
            float xFin = targetRec.Right;
            int milieuX = Convert.ToInt32(xDepart + (xFin - xDepart) / 2f);
            switch (currentWorkingOptions.ColorStyleChoice)
            {
                case ModeAffichageCouleurs.RandomNeutral:
                case ModeAffichageCouleurs.RandomBright:

                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new PointF(targetRec.Left, targetRec.Bottom),
                                    new PointF(targetRec.Left, targetRec.Top),
                                    colorManager.GetNextColor(0),
                                    Color.SteelBlue
                                );
                case ModeAffichageCouleurs.Linear:
                case ModeAffichageCouleurs.Linear2:
                    return new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new PointF(targetRec.Left, targetRec.Bottom),
                                    new PointF(targetRec.Left, targetRec.Top),
                                    ColorManager.ColorByLeft(milieuX, this.currentWorkingOptions.TargetSize.Width),
                                    Color.SteelBlue
                                );
                default:
                    return new SolidBrush(Color.Black);
            }
        }


        /// <summary>
        /// A l'image de PaintDirPart, génère l'arc de cercle correspondant aux fichiers d'un répertoire.
        /// </summary>
        /// <param name="rec"></param>
        /// <param name="startAngle"></param>
        /// <param name="endAngle"></param>
        private void PaintFilesPart(RectangleF rec)
        {
            //if (!printDirNames && treeGraph.OptionAlsoPaintFiles)
            //{
            //    float nodeAngle = endAngle - startAngle;
            //    rec.Inflate(pasNiveau, pasNiveau);
            //    //Console.WriteLine("Processing Files (Angle:" + startAngle + ";" + endAngle + "; Rec:" + rec + ")...");
            //    frontGraph.FillPie(new SolidBrush(Color.White), Rectangle.Round(rec), startAngle, nodeAngle); //TODO

            //}
        }



        public override IDirectoryNode FindNodeByCursorPosition(System.Drawing.Point curseurPos)
        {
            return null; // TODO throw new NotImplementedException();
        }
    }
}
