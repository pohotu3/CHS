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
            c.ToLower();

            string actionCommand = c.Split(' ').First();

            switch (actionCommand)
            {
                case "play":
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
                            Console.WriteLine("There is no media to resume!");
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
                            Console.WriteLine("There is no media to close!");
                    }
                    else
                    {
                        Console.WriteLine("Are you sure you want to exit the program? Y for yes and any other key for no.");
                        if (Console.ReadKey().Key == ConsoleKey.Y)
                        {
                            if (Program.getMediaThread().IsAlive)
                                invoke("stop");
                            return false;
                        }
                        else
                            Console.WriteLine("");
                    }
                    break;
                case "mute":
                    if (containsMusic(c) || containsVideo(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("mute");
                        else
                            Console.WriteLine("There is no active media to mute!");

                    }
                    break;
                case "unmute":
                    if (containsVideo(c) || containsMusic(c))
                    {
                        if (Program.getMediaThread().IsAlive)
                            invoke("unmute");
                        else
                            Console.WriteLine("There is no active media to unmute!");
                    }
                    break;
                default:
                    Console.WriteLine("There was no valid action command, please try again");
                    break;
            }
            return true;
        }

        private static void invoke(string s)
        {
            if (Program.getPlayer() == null)
            {
                Console.WriteLine("Cannot invoke command " + s + ", mediaPlayer returned NULL");
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
                default:
                    break;
            }
        }

        private static void typeError()
        {
            Console.WriteLine("Invalid type command");
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
