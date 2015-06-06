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

        // these are austin's dirs////////////////////////
        public const string musicDir = "G:\\Media\\Music\\";
        public const string movieDir = "G:\\Media\\Movies\\MP4\\";
        //////////////////////////////////////////////////

        private bool muted = false;
        private double previousVolume = 0.5;

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

        public void mute()
        {
            setVolume(0);
        }

        public void unmute()
        {
            setVolume(previousVolume);
        }

        public void setVolume(double i)
        {
            previousVolume = video.Volume;
            video.Volume = i;
        }

        private string findFile(string command)
        {
            string[] movieFiles = Directory.GetFiles(movieDir, "*.*", SearchOption.AllDirectories);
            string[] musicFiles = Directory.GetFiles(musicDir, "*.*", SearchOption.AllDirectories);
            string[] splitCommand = command.Split(' ');
            ArrayList possibleMatches = new ArrayList();

            //set all of the array's to lowercase, because why the hell do we need them uppercase
            for (int i = 0; i < movieFiles.Length; i++)
                movieFiles[i] = movieFiles[i].ToLower();
            for (int i = 0; i < musicFiles.Length; i++)
                musicFiles[i] = musicFiles[i].ToLower();
            for (int i = 0; i < splitCommand.Length; i++)
                splitCommand[i] = splitCommand[i].ToLower();

            for (int a = 0; a < movieFiles.Length; a++) // for movies
            {
                string title = movieFiles[a].Split('\\').Last();
                title = title.Split('.').First();
                string[] titleSplit = title.Split(' '); // this is used ONLY for getting the number of words in a title
                int titleLength = titleSplit.Length;

                int wordsMatched = 0; // in this forloop because i want the value to reset for the next title
                for (int b = 2; b < splitCommand.Length; b++) // might as well start at 2, the movie name wont be there anyway cause of command words
                {
                    for (int c = 0; c < titleSplit.Length; c++)
                        if (titleSplit[c] == splitCommand[b])
                            wordsMatched++;
                    
                    if (wordsMatched == titleLength) // if the number of words matched == the number of words in teh title, return that as there's no way that's not the right one
                        return movieFiles[a];
                }
            }
            for (int a = 0; a < musicFiles.Length; a++) // for music
            {
                string title = musicFiles[a].Split('\\').Last();
                title = title.Split('.').First();
                string[] titleSplit = title.Split(' '); // this is used ONLY for getting the number of words in a title
                int titleLength = titleSplit.Length;

                int wordsMatched = 0; // in this forloop because i want the value to reset for the next title
                for (int b = 2; b < splitCommand.Length; b++) // might as well start at 2, the movie name wont be there anyway cause of command words
                {
                    for (int c = 0; c < titleSplit.Length; c++)
                        if (titleSplit[c] == splitCommand[b])
                            wordsMatched++;
                    
                    if (wordsMatched == titleLength) // if the number of words matched == the number of words in teh title, return that as there's no way that's not the right one
                        return musicFiles[a];
                }
            }

            return null;
        }
    }
}
