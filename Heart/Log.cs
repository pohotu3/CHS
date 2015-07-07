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
	public class Log
	{
		public string fileName;
		private DateTime dt;

		public Log (string s)
		{
			dt = DateTime.Now;

			if(!System.IO.Directory.Exists(s))
				System.IO.Directory.CreateDirectory(s);

			fileName = s + dt.Month + "-" + dt.Day + "-" + dt.Year;
		}

		public void write(string s)
		{
			System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true);
			file.WriteLine (dt.Hour + ":" + dt.Minute + ":" + dt.Second + "-  " + s);
			file.Close ();
		}
	}
}

