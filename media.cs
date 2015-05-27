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

namespace HomeSystem_CSharp
{
    class media
    {
        public static void playMusic(string dir)
        { 
            Program.musicPlayer.Open(new Uri(dir, UriKind.RelativeOrAbsolute));

            VideoDrawing aVideoDrawing = new VideoDrawing();

            aVideoDrawing.Rect = new Rect(0, 0, 100, 100);

            aVideoDrawing.Player = Program.musicPlayer;

            // Play the video once.
            Program.musicPlayer.Play();
        }


        public static void playVideo()
        {
            //Nothing to see here at the moment. Maybe come back later.
        }
    }
}
