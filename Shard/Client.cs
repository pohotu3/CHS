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

		public static Socket master;

		// requires an IP and socket constructor
		public Client (string ipAddress, int socket)
		{
			this.socket = socket;
			this.ipAddress = iipAddressp;

			master = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			IPEndPoint ip = new IPEndPoint (IPAddress.Parse (ipAddress), socket);

			try {
				// tries to connect to the socket located at the IP and socket given
				master.Connect (ip);
			} catch {
				Console.WriteLine ("could not connect to " + ip);
				Thread.Sleep (1000);
			}
		}

		private static void Data_IN ()
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

		private static void DataManager (Packet p)
		{

		}
	}
}

