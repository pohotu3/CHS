﻿/*
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
		private string ipAddress;
		private bool running = false;

		private static Guid guid;
		public Socket master;
		private Thread listeningThread;

		// requires an IP and socket constructor
		public Client (string ipAddress, int socket, Guid guid)
		{
			this.socket = socket;
			this.ipAddress = ipAddress;
			this.guid = guid;

			master = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ip = new IPEndPoint (IPAddress.Parse (ipAddress), socket);

			try {
				// tries to connect to the socket located at the IP and socket given
				master.Connect (ip);
			} catch {
				Console.WriteLine ("could not connect to " + ip);
				Thread.Sleep (1000);
			}

			// creates the new listening thread
			listeningThread = new Thread (Data_IN);
		}

		public void start()
		{
			running = true;
			listeningThread.Start ();
		}

		// for receiving data
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
				if (readBytes > 0)
					DataManager (new Packet (buffer));
			}
		}

		// for sending data
		public void Data_OUT()
		{

		}

		// for handling received data
		private static void DataManager (Packet p)
		{

		}
	}
}

