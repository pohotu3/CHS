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
using System.Threading.Tasks;
using ConnectionData;
using System.IO;

namespace HeartConsole
{
    class HeartCore
    {

        public static string systemName = "", musicDir = "", movieDir = "", commandKey = "", baseDir = Variables.Default.baseDir, configDir = baseDir + Variables.Default.configDir, logBaseDir = baseDir + Variables.Default.logBaseDir;
        private int serverPort = Variables.Default.serverPort;
        public static bool cfg_set = false;

        // unique identifier for the server
        private Guid guid;

        private Config cfg = null;
        private Log log = null;
        private Server server = null;

        private static HeartCore core;
        private PythonScript python_api;

        public HeartCore()
        {
            core = this;

            baseDir = System.Environment.GetEnvironmentVariable("HOME") + baseDir;

            // allows the log file to be created in the home directory
            logBaseDir = System.Environment.GetEnvironmentVariable("HOME") + logBaseDir;

            // allows the config file to be created in the home directory. ie: /home/austin/
            configDir = System.Environment.GetEnvironmentVariable("HOME") + configDir;

            Init();
        }

        private void Init()
        {
            // set up logging here
            log = new Log(logBaseDir);
            Write("#############################SYSTEM STARTUP###################################");
            Write("System logging initialized...");
            Write("Log located at " + log.fileName);

            // initialize the configuration files
            Write("Setting up configuration...");
            cfg = new Config(configDir);

            if (cfg.exists())
            {
                Write("Configuration file found. Loading settings.");
                LoadConfig();
                //if (cfg.get("cfg_set") == Boolean.TrueString)
                //    ConsolidateFiles();
            }
            else
            {
                Write("Configuration file does not exist. Creating file. Please configure your server with your web browser.");
                cfg.set("cfg_set", cfg_set.ToString());
                cfg.set("guid", Guid.NewGuid());
                cfg.Save();
            }

            // set up all the network information and objects, do NOT start
            Write("Creating Server on port " + serverPort);
            server = new Server(serverPort, guid); // port number isn't 100% firm, but no reason to change it
            Write("Created Server connection on port " + serverPort);

            // start listening for connections
            server.Start();
            Write("Heart Server started listening on IP: " + server.ip.Address + " Port: " + serverPort);

            string py_var;
            if (Environment.OSVersion.Platform.ToString() == "Win32NT")
                py_var = Environment.GetEnvironmentVariable("PY_HOME", EnvironmentVariableTarget.Machine) + "/python.exe";
            else
                py_var = "python3";

            python_api = new PythonScript(py_var, "HeartAPI.py" + " " + server.ip.Address + " " + serverPort + " " + baseDir, Write);
        }

        private void ConsolidateFiles()
        {
            string[] fileList = Directory.GetFiles(@"F:\Media\Temp");
            foreach(string s in fileList){
                
            }
            System.IO.File.Move("oldfilename", "newfilename");
        }

        public static HeartCore GetCore()
        {
            return core;
        }

        public Server GetServer()
        {
            return server;
        }

        public void Write(string s)
        {
            Console.WriteLine(s);
            log.write(s);
        }

        // closes down connections and the quits the program
        public void Close()
        {
            Write("Closing down Heart...");
            server.Close();
            Write("Goodbye.");
            python_api.Stop();
            Environment.Exit(0);
        }

        public Config GetConfig()
        {
            return cfg;
        }

        // loads up all the settings from the config to the variables
        public void LoadConfig()
        {
            // load all the information from the cfg after it's set up
            if (!cfg_set)
            {
                Write("Config is not set up. Shards will be refused until that is set.");
            }
            else
            {
                try
                {
                    systemName = cfg.get("systemName");
                    musicDir = cfg.get("musicDir");
                    movieDir = cfg.get("movieDir");
                    commandKey = cfg.get("commandKey");
                }
                catch (Exception e)
                {
                    Write("Unable to load config file. Deleting file. Please set up the configuration over your web browser.");
                    var temp_guid = cfg.get("guid");
                    File.Delete(cfg.FileName());
                    cfg.reload();
                    cfg.set("cfg_set", "False");
                    cfg.set("guid", temp_guid);
                    cfg.Save();
                }
            }

            // this variable is guarenteed to be in the cfg file
            cfg_set = Boolean.Parse(cfg.get("cfg_set"));
            guid = Guid.Parse(cfg.get("guid"));
        }

        public static void Main(string[] args)
        {
            new HeartCore();
        }
    }
}
