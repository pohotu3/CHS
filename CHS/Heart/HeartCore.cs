using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heart
{
    class HeartCore
    {
        private HeartCore core;

        public HeartCore()
        {
            core = this;
        }

        static void Main(string[] args)
        {
            new HeartCore();
        }
    }
}
