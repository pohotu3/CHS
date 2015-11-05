using System;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConnectionData;
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
    public partial class MainWindow : Window
    {

        ShardCore core;

        public MainWindow()
        {
            InitializeComponent();
            core = new ShardCore(this);
        }

        public void Write(string s)
        {
            Dispatcher.Invoke(() =>
            {
                consoleBlock.Text += "\n";
                consoleBlock.Text += s;
                scrollPanel.ScrollToBottom();
            });
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            core.GetClient().Close();
            Environment.Exit(0);
        }
    }
}
