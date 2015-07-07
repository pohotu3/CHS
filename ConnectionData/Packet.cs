using System;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ConnectionData
{
	public class Packet
	{
		public Packet ()
		{
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
	}
}

