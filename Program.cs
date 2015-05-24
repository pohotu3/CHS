using System;
using System.Windows;
using System.Windows.Media;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace HomeSystem_CSharp
{

    class Program
    {
        public const string systemName = "Crystal Home System";
        public const string systemType = "core";
        public const string systemVersion = "0.0.1";
        
        static void Main(string[] args)
        {
           
            bool running = true;
            string command;
            MediaPlayer player = new MediaPlayer();

            // System Setup and Title
           systemStartupMessage();

            // Play startup sound
           media.playMusic("test.ogg",player);

            // Core system Loop
            while(running)
            {
                command = commandModule.getCommand("Enter Command: ");
                running = commandModule.analyzeCommand(command,player);
            }

            Console.ReadKey(true);

        }


        private static void systemStartupMessage()
        {
            Console.Title = "Crystal Home Systems";
            Console.WriteLine(systemName, " ", systemType, " Version ", systemVersion, "\n");
            Console.WriteLine("\nCreated by Ezra and Austin");
        }


        public void fileOpenError()
        {
            Console.WriteLine("Unable to open file!\n");
        }

    }

}


