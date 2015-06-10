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

namespace CrystalHomeSystems
{


    public partial class MainWindow : Window
    {
        static MainWindow mw = null;
        public MainWindow()
        {
            InitializeComponent();
            mw = this;
            TitleLabel.Content = "" + Program.systemName + " " + Program.systemType;

            // start core processes
            Program.startMain();
        }

        public static void close()
        {
            Program.getSpeech().dispose();
            mw.Close();
        }
    }
}
