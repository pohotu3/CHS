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
using System.Threading.Tasks;

namespace CrystalHomeSystems
{
    class commandModule
    {

        public static string getCommand(string c)
        {
            Console.Write(c);
            string line;
            line = Console.ReadLine();
            return line;
        }

        /*
         * list of available action commands:
         * play, start, resume, pause, quit, exit, stop, close
         * 
         * list of available type commands:
         * music, song, artist, album, video, movie, tv, show
         */
        public static bool analyzeCommand(string c)
        {
            c = c.ToLower();

            string actionCommand = c.Split(' ').First();

            switch (actionCommand)
            {
                case "play":
                case "start":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (MainWindow.mediaWindow == null)
                            MainWindow.mediaWindow = new MediaWindow(c);                        
                        else
                            MainWindow.mediaWindow.play();
                    }
                    else
                        typeError();
                    break;
                case "pause":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (MainWindow.mediaWindow == null)
                            MainWindow.getSpeech().speak("There is no media to pause");
                        else
                            MainWindow.mediaWindow.pause();
                    }
                    else
                        typeError();
                    break;
                case "resume":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (MainWindow.mediaWindow == null)
                            MainWindow.getSpeech().speak("There is no media to resume");
                        else
                            MainWindow.mediaWindow.play();
                    }
                    else
                        typeError();
                    break;
                case "quit":
                case "exit":
                case "stop":
                case "close":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (MainWindow.mediaWindow == null)
                            MainWindow.getSpeech().speak("There is no media to close");
                        else
                            MainWindow.closeMedia();
                    }
                    else
                    {
                        MainWindow.close();
                        return false;
                    }
                    break;
                case "mute":
                    if (MainWindow.mediaWindow == null)
                        MainWindow.getSpeech().speak("There is no media to mute");
                    else
                        MainWindow.mediaWindow.mute();
                    break;
                case "unmute":
                    if (MainWindow.mediaWindow == null)
                        MainWindow.getSpeech().speak("There is no media to unmute");
                    else
                        MainWindow.mediaWindow.unmute();
                    break;
                case "increase":
                case "raise":
                    if (MainWindow.mediaWindow == null)
                        MainWindow.getSpeech().speak("There is no media to increase the volume of");
                    else
                        MainWindow.mediaWindow.raiseVolume();
                    break;
                case "decrease":
                case "lower":
                    if (MainWindow.mediaWindow == null)
                        MainWindow.getSpeech().speak("There is no media to decrease the volume of");
                    else
                        MainWindow.mediaWindow.lowerVolume();
                    break;
                case "help":
                    MainWindow.getSpeech().speak("Welcome to crystal home systems. To play media, simply say play the song kryptonite or something similar. Just make sure to include the media type, ie movie or song.");
                    MainWindow.getSpeech().speak("To pause something, simply say pause the movie or pause the music. To increase or decrease volume, simply say increase or decrease. To completely mute, simply say mute.");
                    MainWindow.getSpeech().speak("To un mute something, say un mute. To quit out of the application, say quit.");
                    break;
                case "cancel":
                    MainWindow.getSpeech().speak("ok");
                    MainWindow.getSpeech().resetRecog();
                    break;
                default:
                    MainWindow.getSpeech().speak("There was no valid action command, please try again");
                    break;
            }
            return true;
        }

        private static void typeError()
        {
            MainWindow.getSpeech().speak("Invalid media type.");
        }

        private static bool containsMusic(string c)
        {
            if (c.Contains("music") || c.Contains("song") || c.Contains("album") || c.Contains("artist"))
                return true;
            else
                return false;
        }

        private static bool containsVideo(string c)
        {
            if (c.Contains("movie") || c.Contains("show") || c.Contains("tv") || c.Contains("video"))
                return true;
            else
                return false;
        }

    }
}
