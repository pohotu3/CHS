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
using System.Windows.Forms;

namespace CrystalHomeSystems
{

    public partial class FirstTimeLaunch : Window
    {
        private int panel = 0;
        private string musicDir = "";
        private string movieDir = "";
        private string initCommand = "";
        private Config systemConfig;
        public bool active = false;


        public FirstTimeLaunch(Config config)
        {
            active = true;
            systemConfig = config;

            InitializeComponent();

            NextButton.Click += Next_Button_Click;

            Style s = new Style();
            s.Setters.Add(new Setter(UIElement.VisibilityProperty, Visibility.Collapsed));
            Tabs.ItemContainerStyle = s;
        }

        private void Next_Button_Click(object sender, RoutedEventArgs e)
        {
            nextPanel();
        }

        private void nextPanel()
        {
            Tabs.SelectedIndex = ++panel;
        }

        private void MusicDirSet_Click(object sender, RoutedEventArgs e)
        {
            musicDir = getOpenDir();

            if (musicDir != "")
                nextPanel();
            else
                System.Windows.MessageBox.Show("Please enter a valid directory path.");
        }

        private string getOpenDir()
        {
            FolderBrowserDialog open = new FolderBrowserDialog();
            open.ShowDialog();
            return open.SelectedPath;
        }

        private void MovieDirSet_Click(object sender, RoutedEventArgs e)
        {
            movieDir = getOpenDir();
            if (movieDir != "")
                nextPanel();
            else
                System.Windows.MessageBox.Show("Please enter a valid directory path.");
        }

        private void InitCommandSet_Click(object sender, RoutedEventArgs e)
        {
            if (InitCommandText.Text != "")
                initCommand = InitCommandText.Text;
            nextPanel();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            systemConfig.set("musicDir", musicDir);
            systemConfig.set("movieDir", movieDir);
            systemConfig.set("voicePrompt", initCommand);
            systemConfig.Save();

            active = false;
            MainWindow.mw.contueStartup();
            this.Close();
        }
    }
}
