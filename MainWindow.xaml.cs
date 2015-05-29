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

namespace PlayVideo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MediaPlayer player)
        {
            InitializeComponent();

            VideoDrawing drawing = new VideoDrawing { Rect = new Rect(0, 0, 800, 600), Player = player };
            DrawingBrush brush = new DrawingBrush(drawing);
            Background = brush;
        }
    }
}
