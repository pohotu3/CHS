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
using System.IO;
using System.Collections;

namespace HomeSystem_CSharp
{

    public partial class MediaWindow : Window
    {

        // these are austin's dirs
        public const string musicDir = "G:\\Media\\Music\\";
        public const string movieDir = "G:\\Media\\Movies\\MP4\\";
        ///////////////////////////////////////////////

        public MediaWindow(string c)
        {
            // we want the media indexing to take place here. Every time this is launched, we want it to check for new media.
            // file name will be the only thing passed through to the dir, so we need it to be able to figure out where. for
            // threading purposes, lets just keep it localized to this mediawindow for now

            string dir = findFile(c);
            if (dir == null)
            {
                Console.WriteLine("Unable to find media file, invalid directory.");
                this.Close();
                return;
            }

            Console.WriteLine(dir);
            InitializeComponent(); // place this lower so that it doesnt black screen for so long while it finds the file
            video.Source = new Uri(dir);


            play();
            setVolume(1);
        }

        public void play()
        {
            video.Play();
        }

        public void pause()
        {
            video.Pause();
        }

        public void stop()
        {
            video.Stop();
            this.Close();
        }

        public void setVolume(int i)
        {
            video.Volume = i;
        }

        private string findFile(string command)
        {
            string[] movieFiles = Directory.GetFiles(movieDir, "*.*", SearchOption.AllDirectories);
            string[] musicFiles = Directory.GetFiles(musicDir, "*.*", SearchOption.AllDirectories);
            ArrayList possibleMatches = new ArrayList();

            string[] splitCommand = command.Split(' ');

            //maybe change up the order it does this, i'm thinking this may be a slower way to do it but i'm not sure...
            for (int x = 2; x < splitCommand.Length; x++) // starting at 2 beacuse the first 2 words are guarenteed NOT to be in the title, so just failsafe(ish)
            {
                string commandWord = splitCommand[x];

                for (int y = 0; y < movieFiles.Length; y++) // search whole list of movie files
                {
                    string movieTitle = movieFiles[y].Split('\\').Last();
                    movieTitle = movieTitle.Split('.').First();

                    if (movieTitle.Contains(commandWord)) // if this specific movie file dir contains the command word
                    {
                        possibleMatches.Add(movieFiles[y]);
                    }
                }

                for (int z = 0; z < musicFiles.Length; z++) // search whole list of music files
                {
                    string musicTitle = musicFiles[z].Split('\\').Last();
                    musicTitle = musicTitle.Split('.').First();

                    if (musicTitle.Contains(commandWord)) // if this specific music file dir contains the command word
                    {
                        possibleMatches.Add(musicFiles[z]);
                    }
                }

            }
            if (possibleMatches.Count == 0)
                return null;

            return possibleMatches[0].ToString();

            /*
            for (int x = 0; x < movieFiles.Length; x++)
            {
                if (movieFiles[x].Contains(command))
                {
                    return movieFiles[x];
                }
            }
            for (int x = 0; x < musicFiles.Length; x++)
            {
                if (musicFiles[x].Contains(command))
                {
                    return musicFiles[x];
                }
            }
            */
        }
    }
}
