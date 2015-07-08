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
		public static string guid;
		public static bool listening = false;

		private static Socket listenerSocket;
		private static List<ClientData> _clients;
		private static Thread listenThread;
		public IPEndPoint ip;

		// create all the connection objects
		public Server (int listeningPort, Guid g)
		{
			port = listeningPort;
			guid = g.ToString ();

			listenerSocket = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			_clients = new List<ClientData> ();

			ip = new IPEndPoint (IPAddress.Parse (Packet.GetIP4Address ()), port);
			listenerSocket.Bind (ip);

			listenThread = new Thread (ListenThread);
		}

		public void Start ()
		{
			listening = true;
			listenThread.Start ();
		}

		// listener - listens for clients trying to connect
		private void ListenThread ()
		{
			while (listening) {
				// perameter 5 is the backlog, allows 5 connection attempts at the same time
				// prevents DDOSING in a way
				listenerSocket.Listen (5);

				try {
					_clients.Add (new ClientData (listenerSocket.Accept ()));
				} catch {
				}
			}
		}

		// closes all connections and releases resources
		public void Close ()
		{
			listening = false;

			// send the close connection status to all clients
			foreach (ClientData cd in _clients) {
				Packet p = new Packet (Packet.PacketType.CloseConnection, guid);
				cd.Data_OUT (p);
			}

			listenerSocket.Close ();
			listenThread.Join ();
			_clients.Clear ();
		}
	}

	class ClientData
	{
		public Socket clientSocket;
		public Thread clientThread;
		public string id;

		public ClientData (Socket clientSocket)
		{
			this.clientSocket = clientSocket;

			// this generates a unique id so we can identify each shard
			id = Guid.NewGuid ().ToString ();

			// after we accept a connection, we start a new thread for listening to the client
			clientThread = new Thread (Data_IN);
			clientThread.Start (clientSocket);

			Register ();
		}

		public void Data_OUT (Packet p)
		{
			clientSocket.Send (p.ToBytes ());
		}

		private void Register ()
		{
			// this function will send the client the server GUID BEFORE the client sends theirs
			Packet p = new Packet (Packet.PacketType.Registration, Server.guid);
			p.packetString = Core.commandKey;
			Data_OUT (p);

			// then it will receive the client GUID, and figure out if the client has been set up
			// before or not
		}

		private void Close ()
		{
			clientSocket.Close ();
			clientThread.Join ();
		}

		// clientdata thread - receives data from each client individually
		public void Data_IN (object cSocket)
		{
			Socket clientSocket = (Socket)cSocket;

			byte[] buffer;
			int readBytes;

			// infinite loop, i'm thinking about doing it a different way later
			while (Server.listening) {
				// sets our buffer array to the max size we're able to receive
				buffer = new byte[clientSocket.SendBufferSize];

				// gets the amount of bytes we've received
				readBytes = clientSocket.Receive (buffer);

				// if we actually recieve something, then sort through it
				if (readBytes > 0) {
					// handle data
					Packet packet = new Packet (buffer);
					DataManager (packet);
				}
			}
		}

		// this will handle everything about the packet
		public void DataManager (Packet p)
		{
			switch (p.packetType) {
			case Packet.PacketType.CloseConnection:
				Close ();
				break;
			case Packet.PacketType.Command:

				break;
			default:
				break;
			}
		}
	}
}
