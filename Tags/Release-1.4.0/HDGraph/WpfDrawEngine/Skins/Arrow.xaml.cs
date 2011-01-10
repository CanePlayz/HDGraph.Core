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
using System.Windows.Media.Animation;

namespace HDGraph.WpfDrawEngine.Skins
{
    /// <summary>
    /// Interaction logic for Arrow.xaml
    /// </summary>
    public partial class Arrow : UserControl
    {
        public Arrow()
        {
            InitializeComponent();
        }

        private void showMeWhereToStart_Click(object sender, RoutedEventArgs e)
        {
            Storyboard storyboard = this.FindResource("Storyboard1") as Storyboard;
            storyboard.Begin();
        }
    }
}
