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
        }

        private IDirectoryNode node;
        public IDirectoryNode Node
        {
            get { return node; }
            set
            {
                node = value;
                this.ToolTip = node.Path;
            }
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



        public string Caption
        {
            get { return (string)GetValue(CaptionProperty); }
            set { SetValue(CaptionProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Caption.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CaptionProperty =
            DependencyProperty.Register("Caption", typeof(string), typeof(Arc), new UIPropertyMetadata("Test"));



        #endregion


        public static void OnDesignPropertyChanged(DependencyObject obj, DependencyPropertyChangedEventArgs arg)
        {
            Arc arc = obj as Arc;
            if (arc == null)
                return;

            float newRadius = arc.LargeRadius;
            arc.line1.Point = new Point(newRadius, 0);
            arc.arc1.Size = new Size(newRadius, newRadius);
            double xLarge = Math.Cos(GetRadianFromDegree(arc.StopAngle)) * arc.LargeRadius;
            double yLarge = Math.Sin(GetRadianFromDegree(arc.StopAngle)) * arc.LargeRadius;
            double xSmall = Math.Cos(GetRadianFromDegree(arc.StopAngle)) * arc.SmallRadius;
            double ySmall = Math.Sin(GetRadianFromDegree(arc.StopAngle)) * arc.SmallRadius;
            arc.arc1.Point = new Point(xLarge, yLarge);
            arc.line2.Point = new Point(xSmall, ySmall);
            arc.arc1.IsLargeArc = (arc.StopAngle > 180);
            arc.pathFigure1.StartPoint = new Point(arc.SmallRadius, 0);
            arc.arc2.Point = arc.pathFigure1.StartPoint;
            arc.arc2.Size = new Size(arc.SmallRadius, arc.SmallRadius);
            arc.arc2.IsLargeArc = (arc.StopAngle > 180);
            arc.rotateTransform1.Angle = arc.StartAngle;
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
            brush1.Opacity = 0.5;
            path1.StrokeThickness = 5;
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            brush1.Opacity = 1;
            path1.StrokeThickness = 1;
        }

    }
}
