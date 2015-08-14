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

        public PythonScript(string filePathDir, string args, Action<string> Write)
        {
            this.filePathDir = filePathDir;
            this.args = args;
            this.Write = Write;

            script_thread = new Thread(Run);
            script_thread.Start();
        }

        private void Run()
        {
            process = new ProcessStartInfo();
            process.Arguments = args;
            process.FileName = filePathDir;
            process.UseShellExecute = false;
            process.RedirectStandardOutput = true;

            try
            {
                p = Process.Start(process);
            }
            catch (Exception e)
            {
                HeartCore.GetCore().Write("Unable to start process. Details: " + e.Message);
                //HeartCore.GetCore().Close();
            }

            while (true)
            {
                string foo = p.StandardOutput.ReadLine();
                Write(foo);
            }
        }

        public void Stop()
		{
            try
            {
                p.Kill();
            }
            catch (Exception e) { }

			script_thread.Abort ();
		}
    }
}

