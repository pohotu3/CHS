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
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heart
{
	public class Server
	{

		private int port;
		private bool listening = false;
		private static byte[] buffer = new byte[1024];

		private static List<Socket> clientList = new List<Socket>();

		private static Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

		// create all the connection objects, and get ready to listen for connections
		public Server (int listeningPort)
		{
			port = listeningPort;

			// sets what IP and Port the server is listening on/for
			serverSocket.Bind (new IPEndPoint (IPAddress.Any, port));

			// maximum number of connections the server can accept at any given time
			serverSocket.Listen (5);
		}

		public void start()
		{
			listening = true;

			while (listening) {

			}
		}
	}
}
