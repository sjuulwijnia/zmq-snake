using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace zmq_step2_server
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new ServerHost(1))
            {
                server.Start();
            }
        }
    }
}
