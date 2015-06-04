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

        public static bool analyzeCommand(string c)
        {
            c.ToLower();

            if (c.Contains("exit") || c.Contains("quit") || c.Contains("close"))
            {
                if (containsVideo(c) || containsMusic(c))
                {
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().stop());
                    return true;
                }
                else
                {
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().stop());
                    return false;
                }
            }

            if (c.Contains("stop"))
            {
                if (containsMusic(c) || containsVideo(c))
                {
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().stop());
                    return true;
                }
            }

            if (c.Contains("play") || c.Contains("start"))
            {
                if (containsMusic(c) || containsVideo(c)) // note, typing 'keep playing the song' will call this function
                {
                    Program.startNewMedia("C:\\test.mp4");
                    return true;
                }
            }

            if (c.Contains("pause"))
            {
                if (containsMusic(c) || containsVideo(c))
                {
                    Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().pause());
                    return true;
                }
            }

            if (c.Contains("resume"))
            {
                if (containsMusic(c) || containsVideo(c))
                {
                    if (Program.getMovieThread().IsAlive || Program.getMusicThread().IsAlive)
                        Program.getPlayer().Dispatcher.Invoke(() => Program.getPlayer().play());
                    else
                        Console.WriteLine("Cannot resume, there is no media to play!");

                    return true;
                }
            }

            Console.WriteLine("No valid action statement. Please try again.");
            return true;
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
