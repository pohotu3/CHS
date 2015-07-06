using System;

namespace Heart
{
	public class Log
	{
		public string fileName;

		public Log (string s)
		{
			if(!System.IO.Directory.Exists(s))
				System.IO.Directory.CreateDirectory(s);

			fileName = s + "temp.log"; // I want the log name to be systemDate and systemTime
		}

		public void write(string s)
		{
			System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true);
			file.WriteLine ("System Time: " + s); // i want this to be appended with system time (when i can)
			file.Close ();
		}
	}
}

