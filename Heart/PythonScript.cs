using System;
using System.Diagnostics;
using System.IO;
using Gtk;

namespace Heart
{
	public class PythonScript
	{

		ProcessStartInfo process;

		public PythonScript (string filePathDir, string args, MainWindow mw)
		{
			process = new ProcessStartInfo ();
			process.Arguments = args;
			process.FileName = filePathDir;
			process.UseShellExecute = false;
			process.RedirectStandardOutput = true;

			using(Process p = Process.Start(process))
			using (StreamReader reader = p.StandardOutput)
			{
				string foo = reader.ReadToEnd();
				mw.Write (foo);
			}
		}
	}
}

