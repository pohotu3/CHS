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
using Gtk;

namespace Heart
{
	public partial class MainWindow: Gtk.Window
	{

		private TextBuffer consoleBuffer;
		public bool initNewShard = false;

		public MainWindow () : base (Gtk.WindowType.Toplevel)
		{
			Build ();

			// allows the command_enter bar to be default focused
			CommandEnter.GrabFocus ();

			// set up the textview stream
			consoleBuffer = ConsoleView.Buffer;
		}

		protected void OnDeleteEvent (object sender, DeleteEventArgs a)
		{
			Application.Quit ();
			a.RetVal = true;
		}

		public void Write (string s)
		{
			consoleBuffer.Text += s + "\n";
		}

		public string[] InitNewShard ()
		{
			string[] info = new string[3];

			// get shard information
			HeartCore.GetCore ().Write ("New Shard Connected. Please enter the name you wish to assign it and then press enter.");
			HeartCore.GetCore ().Write ("If you do not want to allow this Shard to connect, type REFUSE: ");
			//info[0] = Console.ReadLine ();
			info [0] = "Test Shard";
			if (info[0].ToUpper () == "REFUSE") {
				return null;
			}

			HeartCore.GetCore ().Write ("Now please enter the type of Shard (options: media): ");
			//info[1] = Console.ReadLine ();
			info [1] = "media";

			HeartCore.GetCore ().Write ("Now please enter the location of the Shard (examples: bedroom, kitchen, living room): ");
			//info[2] = Console.ReadLine ();
			info [2] = "bedroom";

			return info;
		}

		// what is called when the command_entry bar has 'enter' pressed on it
		protected void Command_Entry_Enter (object sender, EventArgs e)
		{
			HeartCore.GetCore ().AnalyzeCommand (CommandEnter.Text);
		}

		// automatically scrolls the console_view to the bottom of the page
		protected void ConsoleViewScrollAuto (object o, SizeAllocatedArgs args)
		{
			ConsoleView.ScrollToIter (ConsoleView.Buffer.EndIter, 0, false, 0, 0);
		}
	}
}