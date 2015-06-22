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
using System.Windows.Shapes;

namespace CrystalHomeSystems
{
    /// <summary>
    /// Interaction logic for FirstTimeLaunch.xaml
    /// </summary>
    public partial class FirstTimeLaunch : Window
    {
        int panel = 0;

        public FirstTimeLaunch(Config config)
        {
            InitializeComponent();

            NextButton.Click += Next_Button_Click;

            Style s = new Style();
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            Tabs.ItemContainerStyle = s;
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            panel++;
            Tabs.SelectedIndex = panel;
        }
    }
}
