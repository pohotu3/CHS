using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace HeartConsole
{
	public class PythonScript
	{

		ProcessStartInfo process;
		Process p;
		Thread script_thread;

		string filePathDir, args;
		Action<string> Write;

		public PythonScript (string filePathDir, string args, Action<string> Write)
		{
			this.filePathDir = filePathDir;
			this.args = args;
			this.Write = Write;

			script_thread = new Thread (Run);
			script_thread.Start ();
		}

		private void Run ()
		{
			process = new ProcessStartInfo ();
			process.Arguments = args;
			process.FileName = filePathDir;
			process.UseShellExecute = false;
			process.RedirectStandardOutput = true;

			p = Process.Start (process);
			while (true) {
				string foo = p.StandardOutput.ReadLine ();
				Write (foo);
			}
		}

		public void Stop ()
		{
			p.Kill ();
			script_thread.Abort ();
		}
	}
}

