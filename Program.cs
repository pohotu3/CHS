/*
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


namespace HomeSystem_CSharp
{

    class Program
    {
        public const string systemName = "Crystal Home System";
        public const string systemType = "core";
        public const string systemVersion = "0.0.1";       
        public static string mediaDir = "";
        private static MediaWindow mediaPlayer = null;
        private static Thread movieThread = new Thread(ShowMediaWindow), musicThread = new Thread(ShowMediaWindow);

        static void Main(string[] args)
        {

            bool running = true;
            string command;

            // System Setup and Title
            systemStartupMessage();

            // Play startup sound/video
            startNewMedia("Bass Head.mp3");

            // Core system Loop
            while (running)
            {
                command = commandModule.getCommand("Enter Command: ");
                running = commandModule.analyzeCommand(command);
            }

            System.Windows.Forms.Application.Exit();

        }


        private static void systemStartupMessage()
        {
            Console.Title = "Crystal Home Systems";
            Console.WriteLine(systemName, " ", systemType, " Version ", systemVersion, "\n");
            Console.WriteLine("\nCreated by Ezra and Austin");
        }
        
        public static MediaWindow getPlayer()
        {
            return mediaPlayer; // want to make this a pointer, however idk how right now
        }

        public static void startNewMedia(string dir)
        {
            mediaDir = dir;

            //now to make it so that the program cannot run both at once, and will pause the other when one wants to play

            //temperary fix for now I suppose, could use some refinement
            if (dir.Contains(".mp4") || dir.Contains(".avi") || dir.Contains("mpg"))
            {
                movieThread = new Thread(ShowMediaWindow);
                movieThread.SetApartmentState(ApartmentState.STA);
                movieThread.Start();
            }
            else if (dir.Contains(".ogg") || dir.Contains(".mp3") || dir.Contains(".wav"))
            {
                musicThread = new Thread(ShowMediaWindow);
                musicThread.SetApartmentState(ApartmentState.STA);
                musicThread.Start();
            }
        }

        private static void ShowMediaWindow()
        {
            (mediaPlayer = new MediaWindow(mediaDir)).ShowDialog();
        }

        public static Thread getMovieThread()
        {
            return movieThread;
        }

        public static Thread getMusicThread()
        {
            return musicThread;
        }
    }

}