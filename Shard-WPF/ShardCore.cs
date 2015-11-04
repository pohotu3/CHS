using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard_WPF
{
    class ShardCore
    {
        public ShardCore(MainWindow mw)
        {
            for (int i = 0; i < 100; i++)
                mw.Write("test");
        }
    }
}
