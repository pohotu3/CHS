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

namespace Heart
{
	class Core
	{

		private const string systemType = "Heart", version = "0.0.1", systemName = "Crystal Home Systems", configDir = "/usr/CrystalHomeSys/crystal_config.cfg";

		public static void Main (string[] args)
		{
			// initialize the configuration files first

			// if first time setup hasn't been done, do that here

			// because this is the Heart, we do not need to initialize the speech

			// initialize the console application next (we can make this a GUI if we want)
			// another thing we could do is set up the console to be a browser based setup, using php or
			// something, allowing remote connection instead of having to directly link up

			Console.WriteLine ("Hello World");
		}
	}
}
