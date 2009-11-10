//Copyright Jean-Yves LAUGEL (2009)
//http://www.hdgraph.com

//This software is a computer program whose purpose is to draw multi-level pie charts of disk space.

//This software is governed by the CeCILL license under French law and
//abiding by the rules of distribution of free software.  You can  use, 
//modify and/ or redistribute the software under the terms of the CeCILL
//license as circulated by CEA, CNRS and INRIA at the following URL
//"http://www.cecill.info". 

//As a counterpart to the access to the source code and  rights to copy,
//modify and redistribute granted by the license, users are provided only
//with a limited warranty  and the software's author,  the holder of the
//economic rights,  and the successive licensors  have only  limited
//liability. 

//In this respect, the user's attention is drawn to the risks associated
//with loading,  using,  modifying and/or developing or reproducing the
//software by the user in light of its specific status of free software,
//that may mean  that it is complicated to manipulate,  and  that  also
//therefore means  that it is reserved for developers  and  experienced
//professionals having in-depth computer knowledge. Users are therefore
//encouraged to load and test the software's suitability as regards their
//requirements in conditions enabling the security of their systems and/or 
//data to be ensured and,  more generally, to use and operate it in the 
//same conditions as regards security. 

//The fact that you are presently reading this means that you have had
//knowledge of the CeCILL license and that you accept its terms.


using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Windows.Controls.Primitives;

namespace HDGraph.WpfDrawEngine
{
    /// <summary>
    /// Interaction logic for ScrollPreview.xaml
    /// </summary>
    public partial class ScrollPreview : UserControl
    {

        #region TargetScrollViewer property

        public ScrollViewer TargetScrollViewer
        {
            get { return (ScrollViewer)GetValue(TargetScrollViewerProperty); }
            set { SetValue(TargetScrollViewerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetScrollViewer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetScrollViewerProperty =
            DependencyProperty.Register("TargetScrollViewer", typeof(ScrollViewer), typeof(ScrollPreview), new UIPropertyMetadata(null, new PropertyChangedCallback(OnTargetScrollViewerChanged)));

        private static void OnTargetScrollViewerChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs arg)
        {
            ScrollPreview scrollPreview = depObj as ScrollPreview;
            if (scrollPreview == null)
                return;

            ScrollViewer previous = arg.OldValue as ScrollViewer;
            if (previous != null)
            {
                previous.ScrollChanged -= scrollPreview.scrollViewer1_ScrollChanged;
            }

            ScrollViewer newScrollViewer = arg.NewValue as ScrollViewer;
            if (newScrollViewer != null)
            {
                newScrollViewer.ScrollChanged += scrollPreview.scrollViewer1_ScrollChanged;
            }
            UpdateSizeWithScaleFactor(scrollPreview);

        }

        #endregion

        #region TargetPopupButton

        public ButtonBase TargetPopupButton
        {
            get { return (ButtonBase)GetValue(TargetPopupButtonProperty); }
            set { SetValue(TargetPopupButtonProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TargetPopupButton.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPopupButtonProperty =
            DependencyProperty.Register("TargetPopupButton", typeof(ButtonBase), typeof(ScrollPreview), new UIPropertyMetadata(null, new PropertyChangedCallback(OnTargetPopupButtonChanged)));

        private static void OnTargetPopupButtonChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs arg)
        {
            ScrollPreview scrollPreview = depObj as ScrollPreview;
            if (scrollPreview == null)
                return;

            ButtonBase previous = arg.OldValue as ButtonBase;
            if (previous != null)
                previous.Click -= scrollPreview.buttonZoomCheck_Click;

            ButtonBase newButton = arg.NewValue as ButtonBase;
            if (newButton != null)
                newButton.Click += scrollPreview.buttonZoomCheck_Click;
        }

        #endregion

        #region ScaleFactor Property

        public double ScaleFactor
        {
            get { return (double)GetValue(ScaleFactorProperty); }
            set { SetValue(ScaleFactorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ScaleFactor.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ScaleFactorProperty =
            DependencyProperty.Register("ScaleFactor", typeof(double), typeof(ScrollPreview), new UIPropertyMetadata(0d, new PropertyChangedCallback(OnScaleFactorChanged)));

        private static void OnScaleFactorChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs arg)
        {
            //ScrollPreview scrollPreview = depObj as ScrollPreview;
            //if (scrollPreview == null)
            //    return;

            //UpdateSizeWithScaleFactor(scrollPreview);



            // Nothing to do : property is linked through binding thanks to OnTargetScrollViewerChanged !
        }

        private static void UpdateSizeWithScaleFactor(ScrollPreview scrollPreview)
        {
            if (scrollPreview.ScaleFactor != 0
                && scrollPreview.TargetScrollViewer != null
                && scrollPreview.TargetScrollViewer.Content != null
                && scrollPreview.TargetScrollViewer.Content is FrameworkElement)
            {
                FrameworkElement elem = scrollPreview.TargetScrollViewer.Content as FrameworkElement;

                // Set a binding on scrollPreview.Width in order to keep it in sync with ScaleFactor AND elem.Width.
                MultiBinding binding = new MultiBinding()
                {
                    Converter = new MultiplierNumericConverter(),
                };
                binding.Bindings.Add(new Binding()
                {
                    Source = scrollPreview,
                    Path = new PropertyPath(ScrollPreview.ScaleFactorProperty),
                });
                binding.Bindings.Add(new Binding()
                {
                    Source = elem,
                    Path = new PropertyPath(FrameworkElement.WidthProperty),
                });
                BindingOperations.SetBinding(scrollPreview, ScrollPreview.WidthProperty, binding);

                // Set a binding on scrollPreview.Heigth in order to keep it in sync with ScaleFactor AND elem.Height.
                binding = new MultiBinding()
                {
                    Converter = new MultiplierNumericConverter(),
                };
                binding.Bindings.Add(new Binding()
                {
                    Source = scrollPreview,
                    Path = new PropertyPath(ScrollPreview.ScaleFactorProperty),
                });
                binding.Bindings.Add(new Binding()
                {
                    Source = elem,
                    Path = new PropertyPath(FrameworkElement.HeightProperty),
                });
                BindingOperations.SetBinding(scrollPreview, ScrollPreview.HeightProperty, binding);

                //scrollPreview.Width = scrollPreview.ScaleFactor * elem.Width;
                //scrollPreview.Height = scrollPreview.ScaleFactor * elem.Height;
            }
        }

        #endregion

        public ScrollPreview()
        {
            InitializeComponent();
        }

        private void scrollViewer1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (object.ReferenceEquals(e.OriginalSource, e.Source)) // prevent events comming from scrollviewer childs to interact with ZoomCheck.
            {
                if (TargetPopupButton != null)
                {
                    if (!IsScrollPossible())
                        TargetPopupButton.Visibility = Visibility.Collapsed;
                    else
                        TargetPopupButton.Visibility = Visibility.Visible;

                    TargetPopupButton.Width = TargetScrollViewer.ActualWidth - e.ViewportWidth;
                    TargetPopupButton.Height = TargetScrollViewer.ActualHeight - e.ViewportHeight;

                    // TODO: move oustside ?
                    TargetPopupButton.Margin = new Thickness(TargetScrollViewer.Margin.Left + e.ViewportWidth,
                                                           TargetScrollViewer.Margin.Top + e.ViewportHeight,
                                                           0, 0);
                    TargetPopupButton.VerticalAlignment = VerticalAlignment.Top;
                    TargetPopupButton.HorizontalAlignment = HorizontalAlignment.Left;
                }
                ArrangeComponents();
            }
        }

        private void ArrangeComponents()
        {
            FrameworkElement elem = TargetScrollViewer.Content as FrameworkElement;

            // hypothèses :
            // x2/y2 = x4/y4
            // x5/y5 = x1/y1
            // y2/y1 = y4/y5
            // x1/x2 = x5/x3
            // 
            // avec :

            double x1 = TargetScrollViewer.ViewportWidth;
            double y1 = TargetScrollViewer.ViewportHeight;

            //double x2 = elem.ActualWidth;
            //double y2 = elem.ActualHeight;
            double x2 = TargetScrollViewer.ExtentWidth;
            double y2 = TargetScrollViewer.ExtentHeight;
            if (x2 == 0 || y2 == 0)
                return;


            double x3 = gridPreview.ActualWidth;
            double y3 = gridPreview.ActualHeight;

            double displayedElementRatio = x2 / y2;
            double previewBoxRatio = x3 / y3;

            double x4, y4;

            if (displayedElementRatio > previewBoxRatio)
            {
                // dans le gridPreview, du blanc est rajouté en haut et en bas
                // hypothèse : x4 = x3

                x4 = x3; // largeur élément complet dans preview
                y4 = y2 * x4 / x2; // hauteur élément complet dans preview
            }
            else
            {
                // dans le gridPreview, du blanc est rajouté sur les côtés droit et gauche
                // hypothèse : y4 = y3

                y4 = y3; // hauteur élément complet dans preview
                x4 = x2 * y4 / y2; // largeur élément complet dans preview
            }

            double margeHautSansScroll = (y3 - y4) / 2;
            double margeGaucheSansScroll = (x3 - x4) / 2;

            double y5 = y1 * y4 / y2;
            if (y5 > y3)
                y5 = y3;
            double x5 = x4 * x1 / x2;
            if (x5 > x3)
                x5 = x3;

            if (y5 > y4)
                margeHautSansScroll -= (y5 - y4) / 2;

            if (x5 > x4)
                margeGaucheSansScroll -= (x5 - x4) / 2;

            double oY = TargetScrollViewer.VerticalOffset;
            double oX = TargetScrollViewer.HorizontalOffset;

            double offsetYDansPreview = oY * y4 / y2;
            double offsetXDansPreview = oX * x4 / x2;

            viewportOverhead.Width = x5;
            viewportOverhead.Height = y5;

            viewportOverhead.Margin = new Thickness(margeGaucheSansScroll + offsetXDansPreview,
                                                    margeHautSansScroll + offsetYDansPreview,
                                                    0, 0);
        }

        private bool IsScrollPossible()
        {
            return !(TargetScrollViewer.ViewportHeight > TargetScrollViewer.ExtentHeight
                                && TargetScrollViewer.ViewportWidth > TargetScrollViewer.ExtentWidth);
        }


        private Point? initialCursorPosition;
        private Point? initialScrollOffset;

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!IsScrollPossible())
                return;
            initialCursorPosition = e.GetPosition(gridPreview);

            Thickness thickness = viewportOverhead.Margin;
            initialScrollOffset = new Point(TargetScrollViewer.HorizontalOffset, TargetScrollViewer.VerticalOffset);
            //previousCursor = Cursor;
            //Cursor = Cursors.ScrollAll;
            ((IInputElement)sender).CaptureMouse();
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ((IInputElement)sender).ReleaseMouseCapture();
            initialCursorPosition = null;
            //Cursor = previousCursor;
            previewPopup.IsOpen = false;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {

            if (initialCursorPosition == null)
                return;

            Point newCursorPosition = e.GetPosition(gridPreview);
            double deltaX = newCursorPosition.X - initialCursorPosition.Value.X;
            double deltaY = newCursorPosition.Y - initialCursorPosition.Value.Y;

            if (TargetScrollViewer.ViewportHeight < TargetScrollViewer.ExtentHeight)
            {
                // there is something to scroll vertically
                TargetScrollViewer.ScrollToVerticalOffset(initialScrollOffset.Value.Y + deltaY * TargetScrollViewer.ViewportHeight / viewportOverhead.Height);
            }
            if (TargetScrollViewer.ViewportWidth < TargetScrollViewer.ExtentWidth)
            {
                // there is something to scroll horizontally
                TargetScrollViewer.ScrollToHorizontalOffset(initialScrollOffset.Value.X + deltaX * TargetScrollViewer.ViewportWidth / viewportOverhead.Width);
            }
            // note : it's useless to manually define the new margin on viewportOverhead, because
            // using ScrollToVerticalOffset or ScrollToHorizontalOffset will automatically 
            // call the scrollViewer1_ScrollChanged method, wich will update viewportOverhead margins.
        }

        private void buttonZoomCheck_Click(object sender, RoutedEventArgs e)
        {
            previewPopup.IsOpen = true;
            ArrangeComponents();
        }
    }
}
