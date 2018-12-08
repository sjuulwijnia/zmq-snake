using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace zmq_step2.client
{
    class Program
    {
        static void Main(string[] args)
        {
            new ClientHost().Start();
        }
    }
}
