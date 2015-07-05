using System;

namespace Heart
{
	public class Log
	{
		private string fileName;

		public Log (string s)
		{
			fileName = s + "temp.log"; // this will need to change when i can load system time/date
			init ();
		}

		private void init()
		{
			if (!System.IO.File.Exists (fileName))
				System.IO.File.Create (fileName);
		}

		public void write(string s)
		{
			System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true);
			file.WriteLine ("System Time: " + s); // i want this to be appended with system time (when i can)
			file.Close ();
		}
	}
}

