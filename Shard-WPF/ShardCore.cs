using System;
using ConnectionData;
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
        public static string commandKey;

        public ShardCore(MainWindow mw)
        {
            this.mw = mw;
            core = this;
            Write("##################Crystal Shard Dev has been Started##################");

            Write("Setting up client...");
            client = new Client("localhost", 6977, Guid.NewGuid());
        }

        public void Write(string s)
        {
            mw.consoleBlock.Text += "\n";
            mw.consoleBlock.Text += s;
            mw.scrollPanel.ScrollToBottom();
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
    }
}
