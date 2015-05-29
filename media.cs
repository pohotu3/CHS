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
*along with Foobar.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PlayVideo;

namespace HomeSystem_CSharp
{
    public class Media : MediaPlayer
    {
        bool playing = false;
        VideoDrawing vidDrawing; 
        static MainWindow videoPanel;

        public Media() { }

        public void playMusic(string dir)
        {

            if (playing)
            {
                play();
                return;
            }

            this.Open(new Uri(dir, UriKind.RelativeOrAbsolute));
                       
            play();
            playing = true;
        }

        /*
        public void playVideo(string dir)
        {

            if (playing)
            {
                play();
                return;
            }


            this.Open(new Uri(dir, UriKind.RelativeOrAbsolute));

            vidDrawing = new VideoDrawing();
            vidDrawing.Rect = new Rect(0, 0, 100, 100);
            vidDrawing.Player = this;

            DrawingBrush DBrush = new DrawingBrush(vidDrawing);
            //videoPanel = new MainWindow(this); // GUI panel to play the video on
            
            play();
            new System.Windows.Application().Run(videoPanel);
            playing = true;

        }
         */

        public bool mediaFailed()
        {
            return this.mediaFailed();
        }

        public bool mediaEnded()
        {
            return this.mediaEnded();
        }

        public void pause()
        {
            if (!canPause())
                return;

            this.Pause();
            playing = false;
        }

        public void play()
        {
            playing = true;
            this.Play();
        }

        public void stop()
        {
            this.Stop();
            playing = false;
        }

        public bool isMuted()
        {
            return this.IsMuted;
        }

        public bool canPause()
        {
            return this.CanPause;
        }

        public double getVolume()
        {
            return this.Volume; // this SHOULDNT be changeable such as getVolume() = 1.0   if it is, fix it
        }

        public void setVolume(double d)
        {
            this.Volume = d;
        }
    }
}
