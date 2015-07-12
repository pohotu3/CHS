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
using Gtk;
using ConnectionData;
using System.IO;

namespace Shard
{
	class ShardCore
	{

		public static string commandKey, logBaseDir = "/CrystalHomeSys/Shard/Shard Logs/", cfgBaseDir = "/CrystalHomeSys/Shard/shard_config.cfg";

		private static ShardCore core;
		private Client client;
		private Log log;
		private Config config;

		public ShardCore ()
		{
			// set up the core variables
			core = this;


			// initialize logging
			logBaseDir = System.Environment.GetEnvironmentVariable ("HOME") + logBaseDir;
			log = new Log (logBaseDir);
			Write ("#############################SYSTEM STARTUP###################################");
			Write ("System logging initialized...");
			Write ("Log located at " + log.fileName);

			// initialize the config file
			cfgBaseDir = System.Environment.GetEnvironmentVariable ("HOME") + cfgBaseDir;
			Write ("Loading configuration file...");
			config = new Config (cfgBaseDir);
			if (!config.exists ()) { // make sure the cfg file exists, and if it doesnt create it
				Write ("Configuration file does not exist! Creating...");
				config.set ("guid", Guid.NewGuid ());
				// probably add a setting for the server's IP address, but i'm not really sure yet...
				config.Save ();
			}
			Write ("Configuration loaded.");

			// set up connections and connect to the server (this will set command key)
			Write ("Creating connection to Heart...");
			client = new Client ("127.0.0.1", 6976, Guid.Parse(config.get("guid")));

			// set up voice and get it primed to go

			// show window and handle that thread
			ShowWindow ();
		}

		private void Init ()
		{

		}

		private void ShowWindow ()
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static ShardCore getCore()
		{
			return core;
		}

		public Client GetClient()
		{
			return client;
		}

		public void Write(string s)
		{
			Console.WriteLine (s);
			log.write (s);
		}

		public static void Main (string[] args)
		{
			core = new ShardCore ();
		}
	}
}
