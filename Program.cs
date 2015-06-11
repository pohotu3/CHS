﻿/*
* Crystal Home Systems 
* Created by Austin and Ezra
* Open Source with Related GitHub Repo
* UNDER DEVELOPMENT
*
* Copyright© 2015 Austin VanAlstyne, Bailey Thorson
*/

/*
*This file is part of Cyrstal Home Systems.
*
*Cyrstal Home Systems is free software: you can redistribute it and/or modify
*it under the terms of the GNU General Public License as published by
*the Free Software Foundation, either version 3 of the License, or
*(at your option) any later version.
*
*Cyrstal Home Systems is distributed in the hope that it will be useful,
*but WITHOUT ANY WARRANTY; without even the implied warranty of
*MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*GNU General Public License for more details.
*
*You should have received a copy of the GNU General Public License
*along with Cyrstal Home Systems.  If not, see <http://www.gnu.org/licenses/>.
 */


using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace CrystalHomeSystems
{

    class Program
    {
        public const string systemName = "Crystal Home System";
        public const string systemType = "Heart";
        public const string systemVersion = "0.0.1";
        public static string mediaDir = "";
        private static MediaWindow mediaPlayer = null;
        private static Thread mediaThread = new Thread(ShowMediaWindow);
        private static Speech speech = new Speech();

        public static void startMain()
        {
            // start speech system
            speech.startRecog();
            
            // setup config's and check if this software has run before. if not, initialize first time setup

            // welcome and indicate ready to run
            speech.speak("Welcome to Crystal Home Systems. System type " + systemType + ".");
        }

        private static void systemStartupMessage()
        {
            Console.Title = "Crystal Home Systems";
            Console.WriteLine(systemName, " ", systemType, " Version ", systemVersion, "\n");
            Console.WriteLine("\nCreated by Ezra and Austin");

            speech.speak("Welcome to Crystal Home Systems. System type " + systemType + ".");
        }

        public static MediaWindow getPlayer()
        {
            return mediaPlayer; // want to make this a pointer, however idk how right now
        }

        public static void startNewMedia(string dir)
        {
            mediaDir = dir;

            // now to make it so that the program cannot run both at once, and will pause the other when one wants to play

            // temperary fix for now I suppose, could use some refinement
            mediaThread = new Thread(ShowMediaWindow);
            mediaThread.SetApartmentState(ApartmentState.STA);
            mediaThread.Start();
        }

        private static void ShowMediaWindow()
        {
            try
            {
                (mediaPlayer = new MediaWindow(mediaDir)).ShowDialog();
            }
            catch (System.InvalidOperationException) { }
        }

        public static Thread getMediaThread()
        {
            return mediaThread;
        }

        public static Speech getSpeech()
        {
            return speech;
        }
    }

}