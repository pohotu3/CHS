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
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using ConnectionData;

namespace Shard
{
	public class Client
	{

		private int socket;
		private string ipAddress, guid, serverGuid;
		private bool running = false, registered = false;

		public Socket master;
		private Thread listeningThread;

		// requires an IP and socket constructor
		public Client (string ipAddress, int socket, Guid guid)
		{
			this.socket = socket;
			this.ipAddress = ipAddress;
			this.guid = guid.ToString ();

			master = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ip = new IPEndPoint (IPAddress.Parse (ipAddress), socket);

			try {
				// tries to connect to the socket located at the IP and socket given
				master.Connect (ip);
			} catch {
				Console.WriteLine ("Could not connect to " + ip);
				Thread.Sleep (1000);

				// we dont want the client socket continuing if we were not able to connect
				return;
			}

			// creates the new listening thread
			listeningThread = new Thread (Data_IN);
		}

		public void start()
		{
			running = true;
			listeningThread.Start (); // Data_IN is now constantly being run
		}

		// for receiving data, constantly running in the listeningThread
		private void Data_IN ()
		{
			byte[] buffer;
			int readBytes;

			// infinite loop, same goes for in the Heart/Server.cs
			for (;;) {
				// creates the buffer array to be as large as possible to receive
				buffer = new byte[master.SendBufferSize];
				// gets the number of bytes received
				readBytes = master.Receive (buffer);

				// as long as we actually received bytes, we can process them
				if (readBytes > 0) {
					// handle data
					Packet packet = new Packet (buffer);
					DataManager (new Packet (packet));
				}
			}
		}

		// for sending data through packets
		public void Data_OUT(Packet p)
		{

		}

		// for handling received data
		private void DataManager (Packet p)
		{
			switch (p.packetType) {
			case Packet.PacketType.Registration:
				// if the server is sending the registration packet
				serverGuid = p.senderID;
				Register ();
				break;
			default:
				break;
			}

		}

		// called if we recieve a registration packet from the server
		private void Register()
		{
			// send the client GUID
			Packet p = new Packet (Packet.PacketType.Registration, guid);
			Data_OUT (p);
		}
	}
}

