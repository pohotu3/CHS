using System;
using Gtk;

namespace Nerv
{
	class Core
	{

		private static Core core;
		private static MainWindow mw;

		public static string commandKey = "";

		public Core(){
			core = this;
		}

		public static Core GetCore(){
			return core;
		}

		public void Log(string s){

		}

		public void Speak(string s){

		}

		public void Write(string s){
		}

		public void Shutdown(){
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
