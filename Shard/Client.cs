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
		private bool running = false, connected = false;

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
				ShardCore.getCore().Write("Connected to Heart at IP " + ipAddress + " Socket: " + socket);
			} catch {
				ShardCore.getCore().Write ("Could not connect to " + ipAddress + " on Socket: " + socket);
				Thread.Sleep (1000);

				// we dont want the client socket continuing if we were not able to connect
				return;
			}

			// creates the new listening thread
			listeningThread = new Thread (Data_IN);
			running = true;
			listeningThread.Start (); // Data_IN is now constantly being run
		}

		// for receiving data, constantly running in the listeningThread
		private void Data_IN ()
		{
			byte[] buffer;
			int readBytes;

			// infinite loop, same goes for in the Heart/Server.cs
			while (running) {
				// creates the buffer array to be as large as possible to receive
				buffer = new byte[master.SendBufferSize];
				// gets the number of bytes received
				try{
					readBytes = master.Receive (buffer);
				}catch(Exception e){
					return;
				}

				// as long as we actually received bytes, we can process them
				if (readBytes > 0) {
					// handle data
					DataManager (new Packet (buffer));
				}
			}
		}

		// for sending data through packets
		public void Data_OUT(Packet p)
		{
			if (!connected && p.packetType != Packet.PacketType.Registration) {
				ShardCore.getCore ().Write ("We are not connected yet. Please try restarting...");
				return;
			}
			master.Send (p.ToBytes ());
		}

		// for handling received data
		private void DataManager (Packet p)
		{
			switch (p.packetType) {
			case Packet.PacketType.Registration:
				// if the server is sending the registration packet (start of connection)
				ShardCore.getCore ().Write ("Received registration packet from Heart.");
				serverGuid = p.senderID;

				// now save that to a file if this is the first connection, otherwise compare it to the file

				// set the commandKey from the server registration packet
				ShardCore.commandKey = p.packetString;

				// send client registration packet
				Data_OUT (new Packet (Packet.PacketType.Registration, guid));
				ShardCore.getCore ().Write ("Sent registration packet to Heart.");
				break;
			case Packet.PacketType.Handshake:
				ShardCore.getCore ().Write ("Handshake received. Connection Established.");
				connected = true;
				break;
			case Packet.PacketType.CloseConnection:
				string reason = p.packetString;
				if (reason == null)
					reason = "No Reason Given.";
				ShardCore.getCore ().Write ("Server is closing the connection. Reason: " + reason);
				ShardCore.GetWindow ().ServerResponse ("Server is closing the connection. Reason: " + reason);
				Close ();
				break;
			case Packet.PacketType.Command:
				ShardCore.getCore ().Write ("Server sent the response: " + p.packetString);
				ShardCore.GetWindow ().ServerResponse (p.packetString);
				break;
			default:
				break;
			}
		}

		public void Close()
		{
			Data_OUT (new Packet (Packet.PacketType.CloseConnection, guid));
			ShardCore.getCore ().Write ("Closing connection with Heart.");
			running = false;
			master.Close ();
			listeningThread.Join ();
		}
	}
}

