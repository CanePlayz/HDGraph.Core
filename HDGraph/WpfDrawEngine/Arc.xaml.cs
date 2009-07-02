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

namespace HDGraph.WpfDrawEngine
{
    /// <summary>
    /// Interaction logic for Arc.xaml
    /// </summary>
    public partial class Arc : UserControl
    {

        public Arc()
        {
            InitializeComponent();
            this.ToolTipOpening += new ToolTipEventHandler(Arc_ToolTipOpening);
            this.ToolTip = "TODO : Tooltip here."; // TODO.
        }

        void Arc_ToolTipOpening(object sender, ToolTipEventArgs e)
        {

            //this.ToolTip = ((Node == null) ? String.Empty : Node.Path) +
            //                "; StartAngle:" + StartAngle + "; StopAngle:" + StopAngle;
        }



        #region Dependency Properties

        public float StartAngle
        {
            get { return (float)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(float), typeof(Arc), new UIPropertyMetadata(0f, new PropertyChangedCallback(OnDesignPropertyChanged)));


        public float StopAngle
        {
            get { return (float)GetValue(StopAngleProperty); }
            set { SetValue(StopAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StopAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StopAngleProperty =
            DependencyProperty.Register("StopAngle", typeof(float), typeof(Arc), new UIPropertyMetadata(45f, new PropertyChangedCallback(OnDesignPropertyChanged)));



        public float LargeRadius
        {
            get { return (float)GetValue(LargeRadiusProperty); }
            set { SetValue(LargeRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeRadiusProperty =
            DependencyProperty.Register("LargeRadius", typeof(float), typeof(Arc), new UIPropertyMetadata(200f, new PropertyChangedCallback(OnDesignPropertyChanged)));


        public float SmallRadius
        {
            get { return (float)GetValue(SmallRadiusProperty); }
            set { SetValue(SmallRadiusProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SmallRadius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SmallRadiusProperty =
            DependencyProperty.Register("SmallRadius", typeof(float), typeof(Arc), new UIPropertyMetadata(50f, new PropertyChangedCallback(OnDesignPropertyChanged)));


        public IDirectoryNode Node
        {
            get { return (IDirectoryNode)GetValue(NodeProperty); }
            set { SetValue(NodeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Node.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NodeProperty =
            DependencyProperty.Register("Node", typeof(IDirectoryNode), typeof(Arc), new UIPropertyMetadata(null, new PropertyChangedCallback(OnNodePropertyChanged)));


        #endregion

        public static void OnNodePropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            //Arc arc = obj as Arc;
            //if (arc == null)
            //    return;
            ////if (arc.Node.DirectoryType == SpecialDirTypes.UnknownPart)
            //// TODO
        }

        private bool editing;

        public void BeginEdit()
        {
            editing = true;
        }

        public void EndEdit()
        {
            if (editing)
            {
                editing = false;
                UpdateDesign();
            }
        }

        private void UpdateDesign()
        {
            float newRadius = this.LargeRadius;
            this.line1.Point = new Point(newRadius, 0);
            this.arc1.Size = new Size(newRadius, newRadius);
            double stopAngleInRadian = GetRadianFromDegree(this.StopAngle);
            double stopAngleCos = Math.Cos(stopAngleInRadian);
            double stopAngleSin = Math.Sin(stopAngleInRadian);
            double xLarge = stopAngleCos * this.LargeRadius;
            double yLarge = stopAngleSin * this.LargeRadius;
            double xSmall = stopAngleCos * this.SmallRadius;
            double ySmall = stopAngleSin * this.SmallRadius;
            this.arc1.Point = new Point(xLarge, yLarge);
            this.line2.Point = new Point(xSmall, ySmall);
            this.arc1.IsLargeArc = (this.StopAngle > 180);
            this.pathFigure1.StartPoint = new Point(this.SmallRadius, 0);
            this.arc2.Point = this.pathFigure1.StartPoint;
            this.arc2.Size = new Size(this.SmallRadius, this.SmallRadius);
            this.arc2.IsLargeArc = this.arc1.IsLargeArc;
            this.rotateTransform1.Angle = this.StartAngle;
        }

        public static void OnDesignPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            Arc arc = obj as Arc;
            if (arc == null)
                return;
            if (!arc.editing)
                // Update design now only if not in edit mode
                // (Update will be called at the end of the "edit mode" if it is enabled).
                arc.UpdateDesign();
        }

        // TODO : Move out there.
        /// <summary>
        /// Convertit un angle en degrés en radian.
        /// </summary>
        /// <param name="degree"></param>
        /// <returns></returns>
        public static double GetRadianFromDegree(float degree)
        {
            return degree * Math.PI / 180f;
        }

        // TODO : Move out there.
        /// <summary>
        /// Convertit un angle en radian en degrés.
        /// </summary>
        public static double GetDegreeFromRadian(double radian)
        {
            return radian * 180 / Math.PI;
        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            //brush1.Opacity = 0.5;
            //path1.StrokeThickness = 5;
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            //brush1.Opacity = 1;
            //path1.StrokeThickness = 1;
        }

    }
}
