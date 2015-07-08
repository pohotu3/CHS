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

namespace Shard
{
	class Core
	{

		public static string commandKey, logBaseDir = "/CrystalHomeSys/Shard Logs/";

		private static Core core;
		private Log log;

		public Core ()
		{
			// set up the core variables

			// initialize logging
			logBaseDir = System.Environment.GetEnvironmentVariable ("HOME") + logBaseDir;
			log = new Log (logBaseDir);
			write ("#############################SYSTEM STARTUP###################################");
			write ("System logging initialized...");
			write ("Log located at " + log.fileName);

			// set up connections and connect to the server (this will set command key)

			// set up voice and get it primed to go

			// show window and handle that thread
			ShowWindow ();
		}

		private void init ()
		{

		}

		private void ShowWindow ()
		{
			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}

		public static Core getCore()
		{
			return core;
		}

		public void write(string s)
		{
			log.write (s);
		}

		public static void Main (string[] args)
		{
			core = new Core ();
		}
	}
}
