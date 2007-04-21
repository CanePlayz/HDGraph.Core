using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace HDGraph
{
    public partial class PickColorForm : Form
    {

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
        /// Graph associé au bitmap buffer
        /// </summary>
        private Graphics graph;

        /// <summary>
        /// Impose au composant de se redessiner, même si sa taille n'a pas changé.
        /// </summary>
        private bool forceRefreshOnNextRepaint = false;

        public bool ForceRefreshOnNextRepaint
        {
            get { return forceRefreshOnNextRepaint; }
            set { forceRefreshOnNextRepaint = value; }
        }


        public PickColorForm()
        {
            InitializeComponent();
        }

        private void PickColorForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Méthode classique OnPaint surchargée pour afficher le graph, et le calculer si nécessaire.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            if (buffer == null || buffer.Width != this.ClientSize.Width || buffer.Height != this.ClientSize.Height || forceRefreshOnNextRepaint)
            {
                // Notif de mise en attente (ANNULE)
                //Form parentForm = FindForm();
                //Cursor oldCursor = parentForm.Cursor;
                //parentForm.Cursor = Cursors.WaitCursor;
                // Création du bitmap buffer
                buffer = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
                graph = Graphics.FromImage(buffer);
                graph.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graph.Clear(Color.White);
                // init des données du calcul
                ChargerArcEnCiel();
                graph.Dispose();
                forceRefreshOnNextRepaint = false;
                // Fin de mise en attente
                //parentForm.Cursor = oldCursor;
            }
            // affichage du buffer
            e.Graphics.DrawImageUnscaled(buffer, 0, 0);
        }

        private void ChargerArcEnCiel()
        {
            for (int i = 0; i < 1000; i++)
            {
                graph.DrawLine(new Pen(new SolidBrush(ColorByLeft(i))),
                               new Point(i, 0),
                               new Point(i, 100));
            }

            graph.FillClosedCurve(new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new Point(100, 100), new Point(200, 200),
                                    Color.Red,
                                    Color.SteelBlue),
                                  new Point[] { new Point(100, 200), new Point(180, 180), new Point(200, 100) },
                                  System.Drawing.Drawing2D.FillMode.Winding);
            //graph.FillPie(new System.Drawing.Drawing2D.LinearGradientBrush(
            //            rec,
            //            GetNextColor(startAngle + nodeAngle / 2f),
            //            Color.SteelBlue,
            //            System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal
            //        ),
            //        Rectangle.Round(rec),
            //        startAngle,
            //        nodeAngle);
            //graph.DrawPie(new Pen(Color.Black), rec, startAngle, nodeAngle);
            graph.FillPie(new System.Drawing.Drawing2D.LinearGradientBrush(
                                    new Point(100, 100), new Point(300, 100),
                                    Color.Red,
                                    Color.SteelBlue), 
                200,200,100,100, 0, 180);

            graph.FillPie(new System.Drawing.Drawing2D.LinearGradientBrush(
                        new Point(200, 400), new Point(301, 400),
                        Color.Blue,
                        Color.Yellow),
                200, 400, 100, 100, 0, 180);
            graph.FillPie(new SolidBrush(Color.White),
                225, 425, 50, 50, 0, 180);
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
        /// 
        /// </summary>
        /// <param name="valSurMille"></param>
        /// <returns></returns>
        public Color ColorByLeft(int valSurMille)
        {
            int valMax = 1000;
            int section = valSurMille * 6 / (valMax);
            valSurMille = Convert.ToInt32(
                        ((float)valSurMille % (valMax / 6f)) * 255 * 6f / valMax);

            switch (section)
            {
                //						       r     G     b
                case 0: return Color.FromArgb(255, 0, valSurMille);
                case 1: return Color.FromArgb(255 - valSurMille, 0, 255);
                case 2: return Color.FromArgb(0, valSurMille, 255);
                case 3: return Color.FromArgb(0, 255, 255 - valSurMille);
                case 4: return Color.FromArgb(valSurMille, 255, 0);
                case 5: return Color.FromArgb(255, 255 - valSurMille, 0);
                default: return Color.Black;
            }
        }
    }
}