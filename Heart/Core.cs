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

		private const string systemType = "Heart", version = "0.0.1", systemName = "Crystal Home Systems", 
		configDir = "/home/austin/crystal_config.cfg";

		private Config cfg = null;

		public Core()
		{
			core = this;

			init ();

			Console.WriteLine ("Hello World");
		}

		public static void Main (string[] args)
		{
			new Core();
		}

		private void init()
		{
			// initialize the configuration files first
			cfg = new Config (configDir);
			if (!cfg.exists ()) 
			{
				initConfig ();
			}

			// because this is the Heart, we do not need to initialize the speech

			// initialize the console application next (we can make this a GUI if we want)
			// another thing we could do is set up the console to be a browser based setup, using php or
			// something, allowing remote connection instead of having to directly link up
		}

		private void initConfig()
		{
			// start the first-time-setup information here, then save it to the cfg

			// this current code is temp, just for testing
			cfg.set("version", version);
			cfg.Save ();
			Console.WriteLine ("set new cfg");
		}

		public Core getCore()
		{
			return core;
		}
	}
}
