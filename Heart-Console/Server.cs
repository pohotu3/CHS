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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ConnectionData;

namespace HeartConsole
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
			listenerSocket.Close ();
			listenerSocket.Dispose ();
			listenThread.Join ();
			listenThread.Abort ();

			// send the close connection status to all clients
			foreach (ClientData cd in _clients) {
				Packet p = new Packet (Packet.PacketType.CloseConnection, guid);
				p.packetString = "maintenence";
				cd.Data_OUT (p);
				cd.Close ();
			}

			_clients.Clear ();
		}
	}

	class ClientData
	{
		public Socket clientSocket;
		public Thread clientThread;
		public string id;
		private bool verified = false, initialized = false;

		// shard details loaded from verification
		private string shardName = "", shardType = "", shardLocation = "";

		public ClientData (Socket clientSocket)
		{
			this.clientSocket = clientSocket;

			HeartCore.GetCore ().Write ("Incoming connection from Shard. IP: " + clientSocket.AddressFamily.ToString ());

			// after we accept a connection, we start a new thread for listening to the client
			clientThread = new Thread (Data_IN);
			clientThread.Start (clientSocket);

            if (HeartCore.cfg_set)
            {
                HeartCore.GetCore().Write("Registering with client " + clientSocket.AddressFamily.ToString());
                Register();
            }
            else
            {
                HeartCore.GetCore().Write("Refusing connection with client " + clientSocket.AddressFamily.ToString() + ". Please set up the config file.");
                HeartCore.GetCore().Write("After config is set up, try again.");
                Packet temp = new Packet(Packet.PacketType.Error, Server.guid);
                temp.packetString = "Heart configuration is not set up. All connections will be refused until it is.";
                Data_OUT(temp);
                Close();
            }

			// wait until we grab the GUID from the client, and then compare it against already generated files
		}

		public void Data_OUT (Packet p)
		{
			try {
				clientSocket.Send (p.ToBytes ());
			} catch (Exception e) {
                HeartCore.GetCore().Write("Unable to send data to the Shard at IP " + clientSocket.AddressFamily.ToString() + ". Closing connection, please restart the Shard.");
                Close();
			}
		}

		private void Register ()
		{
			// this function will send the client the server GUID BEFORE the client sends theirs
			Packet p = new Packet (Packet.PacketType.Registration, Server.guid);
            p.packetString = HeartCore.commandKey;

			Data_OUT (p);
			HeartCore.GetCore ().Write ("Sent registration packet to " + clientSocket.AddressFamily.ToString ());
		}

		public void Close ()
		{
			clientSocket.Close ();
			clientSocket.Dispose ();
			clientThread.Abort ();
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
				try {
					readBytes = clientSocket.Receive (buffer);
				} catch (ObjectDisposedException e) {
					return;
				}

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
					HeartCore.GetCore ().Write ("Shard using GUID " + id + " and IP " + clientSocket.AddressFamily.ToString() + " tried to connect, verification failed. Connection refused.");
					this.Close ();
				}
			}

            // deny packet info if the shard has not been set up before
            if (!initialized)
            {
                HeartCore.GetCore().Write("Shard " + id + " is trying to send a packet, but it hasn't been set up. Please do so.");
                Packet temp = new Packet(Packet.PacketType.Error, Server.guid);
                temp.packetString = "You are not set up with the Heart. All commands will be rejected until you are.";
                Data_OUT(p);
                return;
            }

			switch (p.packetType) {
			case Packet.PacketType.CloseConnection:
				HeartCore.GetCore ().Write ("Shard " + shardName + " has disconnected.");
				Close ();
				break;
			case Packet.PacketType.Command:
				string[] words = p.packetString.ToLower ().Split ();
				string command = words [0]; // the command is going to be the first word in the sentence (for now).
				// later we can dynamically search for them

				// analyze command and respond appropriately
				switch (command) {
				// when the client wants a list of commands
				case "help":
				case "commands":
				case "command":
					HeartCore.GetCore ().Write (shardName + " sent a request 'help'. Responding.");
					Packet t = new Packet (Packet.PacketType.Command, Server.guid);
					t.packetString = "Hi, I'm " + HeartCore.systemName + ". Commands are designed to be intuitive. Enter something as though you would say it out loud and I'll process it!";
					Data_OUT (t);
					break;

				// when the client wants to disconnect from the server
				case "quit":
				case "exit":
				case "disconnect":
					HeartCore.GetCore ().Write (shardName + " send a request to disconnect. Disconnecting.");
					Close ();
					break;

				// when the client wants to start playing a type of media
				case "play":
				case "start":
					HeartCore.GetCore ().Write (shardName + " sent a request to start streaming media. Finding file...");
					string fileToPlay = ""; // the file we want to play
					// find the media file the client is looking for
					string[] mediaFiles = GenerateMediaList (); // full file paths

					// convert all the media files to just file names for easier searching
					string[] fileNames = new string[mediaFiles.Length];
					for (int i = 0; i < fileNames.Length; i++) {
						fileNames[i] = mediaFiles[i].Split ('/').Last (); // remove the file path, leaving only the file name
						fileNames[i] = fileNames[i].Split ('.').First (); // remove the extension end-piece
					}

					// find out if there's a matching file name for the command. deal with duplicate matches
					for (int i = 0; i < fileNames.Length; i++) {
						string currentFile = fileNames [i];

						// look at whether the words in the sent command match the current file name
						for (int a = 1; i < words.Length; a++) { // start at 1 because we dont want to include the command word

						}
					}
					HeartCore.GetCore ().Write ("File for " + shardName + " was found. Location: " + fileToPlay + ".");

					// start streaming on seperate thread

					break;
				default:
					break;
				}
				break;
			default:
				break;
			}
		}

		// generates a list of all media files in the given directories in the config file
		private string[] GenerateMediaList ()
		{
			string[] movieFiles = Directory.GetFiles (HeartCore.movieDir, "*.*", SearchOption.AllDirectories);
			string[] musicFiles = Directory.GetFiles (HeartCore.musicDir, "*.*", SearchOption.AllDirectories);
			ArrayList al = new ArrayList ();

			// for all movie files
			for (int i = 0; i < movieFiles.Length; i++) {
				movieFiles [i] = movieFiles [i].ToLower ();
				al.Add (movieFiles [i]);
			}

			// for all music files
			for (int i = 0; i < musicFiles.Length; i++) {
				musicFiles [i] = musicFiles [i].ToLower ();
				al.Add (musicFiles [i]);
			}

			// return the arraylist of files in the form of a string[]
			return (string[])al.ToArray (typeof(string));
		}

		// finds shard info file saved, and if doesnt exist runs through init system for it
		private bool RetrieveShard (string guid)
		{
			string baseDir = System.Environment.GetEnvironmentVariable ("HOME") + "/CrystalHomeSys/Heart/Shard_Files/";

			// make sure the baseDir exists first
			if (!Directory.Exists (baseDir))
				Directory.CreateDirectory (baseDir);

			// get a list of all current .shard files including the filepaths to them
			string[] shards = Directory.GetFiles (baseDir, "*.shard", SearchOption.TopDirectoryOnly);

			foreach (string shardFile in shards) {
				// load the shard file into a config object for manipulation
				Config t = new Config (shardFile);

				if (id == t.get ("guid")) {
					// load the information from the file
					shardName = t.get ("shardName");
					shardType = t.get ("shardType");
					shardLocation = t.get ("shardLocation");
					return true;
				}
			}

			// get all the shard info
			//string[] info = HeartCore.GetCore ().GetWindow ().InitNewShard ();
			string[] info = {"testName", "testType", "testLocation"};
			if (info == null) { // if the function returns null, the connection was refused by user
				HeartCore.GetCore ().Write ("Shard refused. Closing connection.");
				Packet t = new Packet (Packet.PacketType.CloseConnection, Server.guid);
				t.packetString = "Connection Refused.";
				Data_OUT (t);
				HeartCore.GetCore ().Close ();
				return false;
			}

			shardName = info [0];
			shardType = info [1];
			shardLocation = info [2];

			// create a new config object to load all this to
			Config shardCfg = new Config (baseDir + shardName + ".shard");

			// now save the Shard file to write it to the harddrive
			shardCfg.set ("shardName", shardName);
			shardCfg.set ("shardType", shardType);
			shardCfg.set ("shardLocation", shardLocation);
			shardCfg.set ("guid", id);
			shardCfg.Save ();
			HeartCore.GetCore ().Write ("Configuration on " + shardLocation + " Shard is now complete.");

			return true;
		}
	}
}
