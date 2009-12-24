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
using System.Windows.Shapes;
using System.Net;

namespace WpfTests
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            WebClient cli = new WebClient();
            cli.DownloadStringCompleted += new DownloadStringCompletedEventHandler(cli_DownloadStringCompleted);
            cli.DownloadStringAsync(new Uri("http://www.hdgraph.com/info_info.php"));
        }

        void cli_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            
        }
    }
}
