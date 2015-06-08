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

namespace HomeSystem_CSharp
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

            Console.WriteLine(c);

            string actionCommand = c.Split(' ').First();

            switch (actionCommand)
            {
                case "play":
                case "played":
                case "start":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("play");
                        else
                        {
                            Program.startNewMedia(c);
                        }
                    }
                    else
                        typeError();
                    break;
                case "pause":
                case "paz":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("pause");
                        else
                            typeError();
                    }
                    break;
                case "resume":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("play");
                        else
                            Program.getSpeech().speak("There is no media to resume!");
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
                        if (Program.getMediaThread().IsAlive)
                            invoke("stop");
                        else
                            Program.getSpeech().speak("There is no media to close!");
                    }
                    else
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("stop");
                        Program.getSpeech().speak("Goodbye!");
                        return false;
                    }
                    break;
                case "mute":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("mute");
                        else
                            Program.getSpeech().speak("There is no active media to mute!");

                    }
                    break;
                case "unmute":
                    if (containsVideo(c) || containsMusic(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("unmute");
                        else
                            Program.getSpeech().speak("There is no active media to unmute!");
                    }
                    break;
                case "increase":
                case "raise":
                    if (Program.getMediaThread().IsAlive)
                        invoke("increase volume");
                    else
                        Program.getSpeech().speak("There is no active media to change the volume on!");
                    break;
                case "decrease":
                case "lower":
                    if (Program.getMediaThread().IsAlive)
                        invoke("decrease volume");
                    else
                        Program.getSpeech().speak("There is no active media to change the volume on!");
                    break;
                default:
                    Program.getSpeech().speak("There was no valid action command, please try again");
                    break;
            }
            return true;
        }

        private static void invoke(string s)
        {
            if (Program.getPlayer() == null)
            {
                Program.getSpeech().speak("Could not complete command, there is an error with the setup. Please restart if problem continues.");
                return;
            }

            switch (s)
            {
                case "stop":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().stop());
                    break;
                case "play":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().play());
                    break;
                case "pause":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().pause());
                    break;
                case "mute":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().mute());
                    break;
                case "unmute":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().unmute());
                    break;
                case "increase volume":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().raiseVolume());
                    break;
                case "decrease volume":
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().lowerVolume());
                    break;
                default:
                    break;
            }
        }

        private static void typeError()
        {
            Program.getSpeech().speak("Invalid media type.");
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
