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
using Shard;
using ConnectionData;

public partial class MainWindow: Gtk.Window
{
	public MainWindow () : base (Gtk.WindowType.Toplevel)
	{
		Build ();
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		ShardCore.getCore ().GetClient ().Close ();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void Send_Clicked (object sender, EventArgs e)
	{
		// if there's nothing entered, dont send anything
		if (EnterCommand.Text == "")
			return;

		Packet packet = new Packet (Packet.PacketType.Command, ShardCore.getCore ().guid.ToString());
		packet.packetString = EnterCommand.Text;
		ShardCore.getCore ().GetClient ().Data_OUT (packet);
		ShardCore.getCore ().Write ("Sent command packet to Heart.");

		EnterCommand.Text = "";
	}

	public void ServerResponse(string s)
	{
		HeartOutput.Text = "Server Response: " + s;
	}
}
