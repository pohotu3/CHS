using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConnectionData
{
	[Serializable]
	public class Packet
	{

		public List<string> gData;
		public int packetInt;
		public bool packetBool;
		public string senderID;
		public PacketType packetType;


		public Packet (PacketType type, string senderID)
		{
			gData = new List<String> ();
			this.senderID = senderID;
			this.packetType = type;
		}

		public Packet(byte[] packetBytes)
		{
			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream ms = new MemoryStream (packetBytes);

			Packet p = (Packet)bf.Deserialize (ms);
			ms.Close ();

			this.gData = p.gData;
			this.packetInt = p.packetInt;
			this.packetBool = p.packetBool;
			this.senderID = p.senderID;
			this.packetType = p.packetType;
		}

		public byte[] ToBytes()
		{
			BinaryFormatter bf = new BinaryFormatter ();
			MemoryStream ms = new MemoryStream ();

			bf.Serialize (ms, this);
			byte[] bytes = ms.ToArray ();
			ms.Close ();
			return bytes;
		}

		// this function will return the active IP address of the system. if it
		// cant find one, it returns default local address
		public static string GetIP4Address()
		{
			// this lists all addresses shown in IPConfig
			IPAddress[] ips = Dns.GetHostAddresses (Dns.GetHostName ());

			foreach(IPAddress i in ips)
			{
				// if there's an IP4 address in the list, return it
				if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) 
				{
					return i.ToString ();
				}

				// else return local address
				return "127.0.0.1";			
			}
		}

		public enum PacketType
		{
		}
	}
}

