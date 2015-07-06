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

namespace Heart
{
	class Core
	{
		private Core core = null;

		private const string systemType = "Heart", version = "0.0.1", logBaseDir = "/home/austin/CrystalHomeSys/Logs/";
		private string systemName = "", musicDir = "", movieDir = "", commandKey = "", configDir = "/CrystalHomeSys/crystal_config.cfg"; // command key will include 'OK' in the cfg

		private Config cfg = null;
		private Log log = null;

		public Core()
		{
			core = this;

			configDir = System.Environment.GetEnvironmentVariable("HOME") + configDir;

			init ();
		}

		public static void Main (string[] args)
		{
			new Core();
		}

		private void init()
		{
			// set up logging here
			log = new Log(logBaseDir);
			write ("System logging initialized...");

			// initialize the configuration files
			cfg = new Config (configDir);
			if (!cfg.exists ()) {
				// if the system hasnt been run before, generate a name for it here with the web api. For now, hardcode it
				systemName = "Crystal";
				initConfig ();
			}

			// load all the information from the cfg after it's set up
			musicDir = cfg.get("musicDir");
			movieDir = cfg.get ("movieDir");
			commandKey = cfg.get ("commandKey");
			

			// because this is the Heart, we do not need to initialize the speech
			// instead, set up all the network information and objects, do NOT start
			// listening yet however, wait until the UI is open and ready for commands

			// initialize the console application next (we can make this a GUI if we want)
			// another thing we could do is set up the console to be a browser based setup, using php or
			// something, allowing remote connection instead of having to directly link up
		}

		private void initConfig()
		{
			// start the first-time-setup information here, then save it to the cfg
			string musicDir = "", movieDir = "", commandKey = "ok " + systemName;

			// this current code is temp, just for testing
			cfg.set("musicDir", musicDir);
			cfg.set ("movieDir", movieDir);
			cfg.set("commandKey", commandKey);
			cfg.Save ();
			write ("New configuration file created at " + configDir);
		}

		public Core getCore()
		{
			return core;
		}

		public void write(string s)
		{
			Console.WriteLine (s);
			log.write (s);
		}
	}
}
