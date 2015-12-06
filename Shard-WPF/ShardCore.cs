using System;
using ConnectionData;
using System.Windows.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard_WPF
{
    class ShardCore
    {

        private Client client;
        private static ShardCore core;
        private MainWindow mw;
        public Guid guid;
        public static string commandKey; // the command key that the speach uses to tell if you're talking to it. example: Ok Crystal

        public ShardCore(MainWindow mw)
        {
            guid = Guid.Parse("9b9b181f-3def-4ae1-b3f3-199ed2f08f90");
            this.mw = mw;
            core = this;
            Write("##################Crystal Shard Dev has been Started##################"); // ##################

            Write("Setting up client...");
            client = new Client("127.0.0.1", 6976, guid);
        }

        public void Write(string s)
        {
            mw.Write(s);
        }

        public void SendPacket(Packet p)
        {
            client.Data_OUT(p);
        }

        public void Speak(string s)
        {
            Write(s);
        }

        public static ShardCore GetCore()
        {
            return core;
        }

        public Client GetClient()
        {
            return client;
        }

        public void Shutdown()
        {
            client.Close();
            
        }
    }
}
