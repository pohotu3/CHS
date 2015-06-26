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
        public static MainWindow mw = null;

        public const string systemName = "Crystal Home System";
        public const string systemType = "Heart";
        public const string systemBuild = "PC_Only_Build";
        public const string systemVersion = "0_0_1";
        private static string musicDir = "";
        private static string movieDir = "";
        private const string configDir = "C:\\crystal_config.cfg";
        public static string initialWordsSpoken = "";

        private static Speech speech;

        public static Config systemConfig; // saved to c drive for now, will change when we migrate to linux

        public static MediaWindow mediaWindow = null;

        public MainWindow()
        {
            System.Windows.Forms.Application.ApplicationExit += new EventHandler(OnApplicationExit);
            InitializeComponent();
            mw = this;

            // if software hasn't run before
            if (!System.IO.File.Exists(configDir))
            {
                systemConfig = new Config(configDir);

                // create new 'firstLaunch' object, and pass the systemConfig object
                FirstTimeLaunch ftl = new FirstTimeLaunch(systemConfig);
                ftl.Show();
            }
            else
            {
                systemConfig = new Config(configDir);
                contueStartup();
            }

            Patch patch = new Patch();
            if (patch.needsPatch())
                startPatch();
        }

        public void contueStartup()
        {
            // want to do this first, to get all the dir information loaded to prevent errors
            initConfig();
            TitleLabel.Content = "" + systemName + " " + systemType;
            initialWordsSpoken = "Say '" + systemConfig.get("voicePrompt").ToUpper() + "' to Begin";
            WordsSpoken.Content = initialWordsSpoken;

            // start core processes
            initSpeech();

            speech.speak("Welcome to Crystal Home Systems");
        }

        private void initSpeech()
        {
            speech = new Speech();
            speech.startRecog();
        }

        private void startPatch()
        {
            speech.freezeThenSpeak("There is a patch available! I will pull up the website containing the new download! Goodbye!");
            //System.Diagnostics.Process.Start("http://google.com");
            close();
        }

        public static Speech getSpeech()
        {
            return speech;
        }

        public void OnApplicationExit(object sender, EventArgs e)
        {
            close();
        }

        public static void close()
        {
            speech.dispose();
            closeMedia();
            mw.Close();
        }

        private static void initConfig()
        {
            musicDir = systemConfig.get("musicDir");
            movieDir = systemConfig.get("movieDir");
        }

        public static string getMusicDir()
        {
            return musicDir;
        }

        public static string getMovieDir()
        {
            return movieDir;
        }

        public static void closeMedia()
        {
            if (mediaWindow != null)
            {
                mediaWindow.stop();
                mediaWindow = null;
            }
        }
    }
}
