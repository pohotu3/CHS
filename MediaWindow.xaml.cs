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

namespace HomeSystem_CSharp
{

    public partial class MediaWindow : Window
    {

        private bool playing = false;

        public MediaWindow(string dir)
        {
            InitializeComponent();

            video.Source = new Uri(dir);
        }

        public void play()
        {
            if (playing)
                return;

            video.Play();

            if (!this.IsVisible)
                this.Show();

            playing = true;
        }

        public void pause()
        {
            if (!playing)
                return;

            video.Pause();
            playing = false;
        }

        public void resume()
        {

        }

        public void stop()
        {
            if (!playing)
                return;

            video.Stop();
            playing = false;
            this.Close();
        }

        public void loadNewSource(string dir)
        {
            if (playing)
                stop();

            video.Source = new Uri(dir);
        }

        public void setVolume(int i)
        {
            video.Volume = i;
        }
    }
}
