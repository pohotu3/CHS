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

