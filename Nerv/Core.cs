using System;
using Gtk;

namespace Nerv
{
	class Core
	{

		private static Core core;
		private static MainWindow mw;
		private static Client client;
		private static int port = 6976;
		private static Guid guid = Guid.NewGuid();

		public static string commandKey = "";

		public Core()
		{
			core = this;

			// set up client info
			client = new Client("127.0.0.1", port, guid);
		}

		public static Core GetCore()
		{
			return core;
		}

		public void Notify(string s)
		{

		}

		public void Shutdown()
		{

		}

		public static void Main (string[] args)
		{
			new Core ();
			Application.Init ();
			mw = new MainWindow ();
			mw.Show ();
			Application.Run ();
		}
	}
}
