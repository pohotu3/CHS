using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard_WPF
{
    class ShardCore
    {

        private Client client;

        private MainWindow mw;

        public ShardCore(MainWindow mw)
        {
            this.mw = mw;
            mw.Write("##################Crystal Shard Dev has been Started##################");

            mw.Write("Setting up client...");
            client = new Client(6977, Guid.NewGuid());
        }
    }
}
