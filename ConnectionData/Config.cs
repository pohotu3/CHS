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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConnectionData
{
	public class Config
	{

		private Dictionary<String, String> list;
		private String filename;

		// constructor
		// NOTE: cfg file will not be created (if not already created)
		// until the save() function is called!
		public Config(String fullDir)
		{
			reload(fullDir);
		}

		// checks if the current cfg file exists
		public bool exists()
		{
			return System.IO.File.Exists (filename);
		}

		// returns a value from the cfg file. ie: musicDir
		public String get(String field)
		{
			return (list.ContainsKey(field)) ? (list[field]) : (null);
		}

		// sets/creates the value in the cfg file. ie: musicDir
		public void set(String field, Object value)
		{
			if (!list.ContainsKey(field))
				list.Add(field, value.ToString());
			else
				list[field] = value.ToString();
		}

		public void Save()
		{
			Save(this.filename);
		}

		// writes all the values to the cfg file, and creates it if it doesnt already exist
		public void Save(String filename)
		{
			this.filename = filename;

			System.IO.StreamWriter file = new System.IO.StreamWriter(filename, true);

			foreach (String prop in list.Keys.ToArray())
				if (!String.IsNullOrWhiteSpace(list[prop]))
					file.WriteLine(prop + "=" + list[prop]);

			file.Close();
		}

		// refreshes the values
		public void reload()
		{
			reload(this.filename);
		}

		// refreshes the values
		public void reload(String filename)
		{
			this.filename = filename;
			list = new Dictionary<String, String>();

			if (System.IO.File.Exists(filename))
				loadFromFile(filename);
		}

		// loads the values from a cfg file to the array
		private void loadFromFile(String file)
		{
			foreach (String line in System.IO.File.ReadAllLines(file))
			{
				if ((!String.IsNullOrEmpty(line)) &&
					(!line.StartsWith(";")) &&
					(!line.StartsWith("#")) &&
					(!line.StartsWith("'")) &&
					(line.Contains('=')))
				{
					int index = line.IndexOf('=');
					String key = line.Substring(0, index).Trim();
					String value = line.Substring(index + 1).Trim();

					if ((value.StartsWith("\"") && value.EndsWith("\"")) ||
						(value.StartsWith("'") && value.EndsWith("'")))
					{
						value = value.Substring(1, value.Length - 2);
					}

					try
					{
						//ignore dublicates
						list.Add(key, value);
					}
					catch { }
				}
			}
		}

		// returns the number of settings in a cfg file
		public int numberOfSettings()
		{
			return list.Count;
		}

        public string FileName()
        {
            return filename;
        }
	}
}
