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
using System.IO;
using System.Threading.Tasks;
using Gtk;

namespace Heart
{
	public partial class MainWindow: Gtk.Window
	{

		private TextBuffer consoleBuffer;
		private string[] shardInfo;
		private bool waitingForShardSetupComplete = true;

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
			shardInfo = new string[3];

			Notebook.CurrentPage = 1;

			// get shard information
			while (waitingForShardSetupComplete) {
				Task.Delay(100);
			}

			return shardInfo;
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

		protected void Shard_Entry_Pressed (object sender, EventArgs e)
		{
			// shard name
			shardInfo[0] = nameEntry.Text;

			// shard type
			shardInfo[1] = typeEntry.ActiveText;

			// shard location
			shardInfo[2] = locationEntry.Text;

			Notebook.CurrentPage = 0;

			waitingForShardSetupComplete = false;
		}
	}
}