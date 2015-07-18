﻿/*
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
using Gtk;
using ConnectionData;

namespace Heart
{
	class HeartCore
	{

		private const string systemType = "Heart", version = "0.0.1";
		public static string systemName = "", musicDir = "", movieDir = "", commandKey = "", configDir = "/CrystalHomeSys/Heart/heart_config.cfg", logBaseDir = "/CrystalHomeSys/Heart/Logs/";
		private const int serverPort = 6976;

		// unique identifier for the server
		private Guid guid;

		private Config cfg = null;
		private Log log = null;
		private Server server = null;

		private static HeartCore core;
		private static MainWindow mw;

		public HeartCore ()
		{
			core = this;

			// allows the log file to be created in the home directory
			logBaseDir = System.Environment.GetEnvironmentVariable ("HOME") + logBaseDir;

			// allows the config file to be created in the home directory. ie: /home/austin/
			configDir = System.Environment.GetEnvironmentVariable("HOME") + configDir;

			// initialize the main window
			mw = new MainWindow ();
			mw.Show ();

			Init ();
		}

		private void Init()
		{
			// set up logging here
			log = new Log(logBaseDir);
			Write ("#############################SYSTEM STARTUP###################################");
			Write ("System logging initialized...");
			Write ("Log located at " + log.fileName);

			// initialize the configuration files
			cfg = new Config (configDir);
			if (!cfg.exists ()) {
				// if the system hasnt been run before, generate a name for it here with the web api. For now, hardcode it
				systemName = "Crystal";
				InitConfig ();
			}

			// load all the information from the cfg after it's set up
			systemName = cfg.get ("systemName");
			musicDir = cfg.get("musicDir");
			movieDir = cfg.get ("movieDir");
			commandKey = cfg.get ("commandKey");
			guid = Guid.Parse(cfg.get("guid"));

			/* 
			 * set up all the network information and objects, do NOT start
			 * listening yet however, wait until the UI is open and ready for commands
			 */
			Write ("Creating Server on port " + serverPort);
			server = new Server (serverPort, guid); // port number isn't 100% firm, but no reason to change it
			Write ("Created Server connection on port " + serverPort);

			// start listening for connections
			server.Start ();
			Write ("Started listening on IP: " + server.ip.Address + " Port: " + serverPort);

			// start listening for command inputs to control the server.
			Write ("Type quit to exit. Type commands for a list of available commands.");
		}

		private void InitConfig()
		{
			// start the first-time-setup information here, then save it to the cfg
			string musicDir = "musicDir", movieDir = "movieDir", commandKey = "Ok " + systemName;

			// this current code is temp, just for testing
			// sets the different cfg values for future use, such as the media dir's
			cfg.set ("systemName", systemName);
			cfg.set("musicDir", musicDir);
			cfg.set ("movieDir", movieDir);
			cfg.set("commandKey", commandKey);
			cfg.set ("guid", Guid.NewGuid ().ToString ()); // generates a GUID for the server
			cfg.Save ();

			Write ("New configuration file created at " + configDir);
		}

		public static HeartCore GetCore ()
		{
			return core;
		}

		public Server GetServer ()
		{
			return server;
		}

		public MainWindow GetWindow ()
		{
			return mw;
		}

		public void Write (string s)
		{
			mw.Write (s);
			log.write (s);
		}

		public void AnalyzeCommand(string s)
		{
			string[] commands = new string[] {"quit", "commands"};

			switch (s.ToLower ()) {
			case "quit":
				Close ();
				break;
			case "commands":
				string temp = "Available Commands: ";
				for (int i = 0; i < commands.Length; i++) {
					temp += commands [i] + " ";
				}
				Write (temp);
				break;
			default:
				break;
			}
		}

		public void Close ()
		{
			Write ("Closing down Heart...");
			server.Close ();
			Write ("Goodbye.");
			Application.Quit ();
		}

		public static void Main (string[] args)
		{
			Application.Init ();
			new HeartCore ();
			Application.Run ();
		}
	}
}
