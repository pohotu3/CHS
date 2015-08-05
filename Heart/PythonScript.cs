using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Gtk;

namespace Heart
{
	public class PythonScript
	{

		ProcessStartInfo process;
		Process p;
		Thread script_thread;

		string filePathDir, args;
		Action<string> write;

		public PythonScript (string filePathDir, string args, Action<string> write)
		{
			this.filePathDir = filePathDir;
			this.args = args;
			this.write = write;

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
				write ("running");
				string foo = p.StandardOutput.ReadLine ();
				write ("running1");
				write (foo);
			}
		}

		public void Stop ()
		{
			p.Kill ();
			script_thread.Abort ();
		}
	}
}

