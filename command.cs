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


        public static bool analyzeCommand(string c ,MediaPlayer player)
        {
            c.ToLower();
            if(c.Contains("exit")||c.Contains("quit")||c.Contains("close") )
            {
                if (c.Contains("song") || c.Contains("music"))
                {
                    player.Stop();
                    return true;
                }
                else
                {
                    player.Stop();
                    return false;
                }
            }
            if(c.Contains("stop"))
            {
                if(c.Contains("song")||c.Contains("music"))
                {
                    player.Stop();
                    return true;
                }
                if(c.Contains("movie")||c.Contains("show"))
                {
                    return true;
                }
            }
            if(c.Contains("play")||c.Contains("start"))
            {
                if (c.Contains("music") || c.Contains("song") || c.Contains("album") || c.Contains("artist"))
                {
                        player.Play();
                        return true;
                }
                else
                {
                    Console.WriteLine("Unable to process your command");
                    return true;
                }
            }
            if(c.Contains("pause"))
            {
                if(c.Contains("music")||c.Contains("song"))
                {
                    player.Pause();
                    return true;
                }
                if(c.Contains("movie")||c.Contains("show")||c.Contains("tv"))
                {
                    return true;
                }
            }
            if(c.Contains("resume"))
            {
                if(c.Contains("music")||c.Contains("song"))
                {
                    player.Play();
                        return true;
                }
            }
            Console.WriteLine("Could not process command, please try again");
            return true;
        }
        
    }
}
