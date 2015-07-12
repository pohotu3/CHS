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
using System.IO;
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

			//ip = new IPEndPoint (IPAddress.Parse (Packet.GetIP4Address ()), port);
			ip = new IPEndPoint (IPAddress.Parse ("127.0.0.1"), port);
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
				p.packetString = "maintenence";
				cd.Data_OUT (p);
				cd.Close ();
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
		private bool verified = false;

		// shard details loaded from verification
		private string shardName = "", shardType = "", shardLocation = "";

		public ClientData (Socket clientSocket)
		{
			this.clientSocket = clientSocket;

			HeartCore.GetCore ().Write ("Incoming connection from shard. IP: " + clientSocket.AddressFamily.ToString ());

			// after we accept a connection, we start a new thread for listening to the client
			clientThread = new Thread (Data_IN);
			clientThread.Start (clientSocket);

			HeartCore.GetCore ().Write ("Registering with client " + clientSocket.AddressFamily.ToString ());
			Register ();

			// wait until we grab the GUID from the client, and then compare it against already generated files
		}

		public void Data_OUT (Packet p)
		{
			clientSocket.Send (p.ToBytes ());
		}

		private void Register ()
		{
			// this function will send the client the server GUID BEFORE the client sends theirs
			Packet p = new Packet (Packet.PacketType.Registration, Server.guid);
			p.packetString = HeartCore.commandKey;
			Data_OUT (p);
			HeartCore.GetCore ().Write ("Sent registration packet to " + clientSocket.AddressFamily.ToString ());

			// now we wait for the client to send registration packet. We WILL NOT read other packets till their verified
		}

		public void Close ()
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
				readBytes = clientSocket.Receive (buffer); // throws an exception when being closed...

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
			// if the client is not verified, we will not accept any other type of packet
			if (!verified) {
				if (p.packetType != Packet.PacketType.Registration)
					return;
				id = p.senderID;
				// now we verify the ID against the given saves. if it passes, verified = true
				if (RetrieveShard (id)) {
					verified = true;
					// now we load all the shard information from the saved file

					// confirm successful connection
					HeartCore.GetCore ().Write ("Shard " + shardName + " registered and connected successfully. Sending handshake.");
					Data_OUT (new Packet (Packet.PacketType.Handshake, Server.guid));
					return;
				} else {
					HeartCore.GetCore ().Write ("Shard using GUID " + id + " tried to connect, verification failed. Connection refused.");
					this.Close ();
				}
								
			}
				
			switch (p.packetType) {
			case Packet.PacketType.CloseConnection:
				HeartCore.GetCore ().Write ("Shard " + shardName + " has disconnected.");
				Close ();
				break;
			case Packet.PacketType.Command:
				// not sure how i want to handle this part... i'll work on it later
				break;
			default:
				break;
			}
		}

		// finds shard info file saved, and if doesnt exist runs through init system for it
		private bool RetrieveShard (string guid)
		{
			string baseDir = System.Environment.GetEnvironmentVariable ("HOME") + "/CrystalHomeSys/Heart/Shard Files/";

			// make sure the baseDir exists first
			if (!Directory.Exists (baseDir))
				Directory.CreateDirectory (baseDir);

			// get a list of all current .shard files including the filepaths to them
			string[] shards = Directory.GetFiles (baseDir, "*.shard", SearchOption.TopDirectoryOnly);

			foreach (string shardFile in shards) {
				// load the shard file into a config object for manipulation
				Config t = new Config (shardFile);

				if (id == t.get("guid")) {
					// load the information from the file
					shardName = t.get ("shardName");
					shardType = t.get ("shardType");
					shardLocation = t.get ("shardLocation");
					return true;
				}
			}

			// get shard information
			HeartCore.GetCore ().Write ("New Shard Connected. Please enter the name you wish to assign it and then press enter.");
			HeartCore.GetCore ().Write ("If you do not want to allow this Shard to connect, type REFUSE: ");
			shardName = Console.ReadLine ();
			if (shardName.ToUpper () == "REFUSE") {
				HeartCore.GetCore ().Write ("Shard refused. Closing connection.");
				Packet t = new Packet (Packet.PacketType.CloseConnection, Server.guid);
				t.packetString = "Connection Refused.";
				Data_OUT (t);
				Close ();
			}

			HeartCore.GetCore ().Write ("Now please enter the type of Shard (options: media): ");
			shardType = Console.ReadLine ();

			HeartCore.GetCore ().Write ("Now please enter the location of the Shard (examples: bedroom, kitchen, living room): ");
			shardLocation = Console.ReadLine ();

			// create a new config object to load all this to
			Config shardCfg = new Config(baseDir + shardName + ".shard");

			// now save the Shard file to write it to the harddrive
			shardCfg.set("shardName", shardName);
			shardCfg.set ("shardType", shardType);
			shardCfg.set ("shardLocation", shardLocation);
			shardCfg.set ("guid", id);
			shardCfg.Save ();
			HeartCore.GetCore ().Write ("Configuration on " + shardLocation + " Shard is now complete.");

			return true;
		}

		public void AnalyzeCommand (string s)
		{

		}
	}
}
