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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConnectionData;

namespace Heart
{
	public class Server
	{

		private int port;

		private static Socket listenerSocket;
		static List<ClientData> _clients;

		// create all the connection objects
		public Server (int listeningPort)
		{
			port = listeningPort;

			listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clients = new List<ClientData>();
		}

		// listener - listens for clients trying to connect


		// clientdata thread - receives data from each client individually


		// data manager


		public void start()
		{

		}
	}

	class ClientData
	{
		public Socket clientSocket;
		public Thread clientThread;
		public string id;

		public ClientData ()
		{
			// this generates a unique id so we can identify each shard
			id = Guid.NewGuid ().ToString ();
		}

		public ClientData (Socket clientSocket)
		{
			this.clientSocket = clientSocket;

			// this generates a unique id so we can identify each shard
			id = Guid.NewGuid ().ToString ();
		}
	}
}
