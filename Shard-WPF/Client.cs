using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard_WPF
{
    class Client
    {

        private int port;
        private string id;

        public Client(int port, Guid g)
        {
            this.port = port;
            id = g.ToString();
        }
    }
}
