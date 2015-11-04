using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Shard_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            new ShardCore(this);
        }

        public void Write(string s)
        {
            consoleBlock.Text += "\n";
            consoleBlock.Text += s;
            scrollPanel.ScrollToBottom();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
