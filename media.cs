using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeSystem_CSharp
{
    class media
    {
        public static void playMusic(string dir, MediaPlayer player)
        {
            // 
            // Create a VideoDrawing. 
            //      
            player.Open(new Uri(dir, UriKind.RelativeOrAbsolute));

            VideoDrawing aVideoDrawing = new VideoDrawing();

            aVideoDrawing.Rect = new Rect(0, 0, 100, 100);

            aVideoDrawing.Player = player;

            // Play the video once.
            player.Play();
        }
    }
}
