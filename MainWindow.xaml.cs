﻿using System;
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
        public const string systemVersion = "0.0.1";
        private static string musicDir = "";
        private static string movieDir = "";
        private const string configDir = "C:\\crystal_config.cfg";

        private static Speech speech;

        public static Config systemConfig; // saved to c drive for now, will change when we migrate to linux

        public static MediaWindow mediaWindow = null;

        public MainWindow()
        {
            // if software hasn't run before
            if (!System.IO.File.Exists(configDir))
            {
                systemConfig = new Config(configDir);

                // create new 'firstLaunch' object, and pass the systemConfig object

                systemConfig.set("musicDir", "G:\\Media\\Music\\");
                systemConfig.set("movieDir", "G:\\Media\\Movies\\MP4\\");
                systemConfig.set("voicePrompt", "ok crystal");
                systemConfig.Save();
            }
            else
            {
                systemConfig = new Config(configDir);
            }

            // want to do this first, to get all the dir information loaded to prevent errors
            initConfig();

            InitializeComponent();
            mw = this;
            TitleLabel.Content = "" + systemName + " " + systemType;
            WordsSpoken.Content = "Say 'Ok Crystal' to Begin";

            // start core processes
            initSpeech();

            speech.speak("Welcome to Crystal Home Systems");
        }

        private void initSpeech()
        {
            speech = new Speech();
            speech.startRecog();
        }

        public static Speech getSpeech()
        {
            return speech;
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
