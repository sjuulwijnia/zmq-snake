using System;
using System.Threading;
using ZeroMQ;

namespace zmq_step2.client
{
    internal class ClientHost
    {
        private readonly ZContext _context = null;

        public ClientHost()
        {
            _context = new ZContext();
        }

        public void Start()
        {
            new Thread(() =>
            {
                new ClientSend(_context).Start();
            }).Start();
            new Thread(() =>
            {
                new ClientReceive(_context).Start();
            }).Start();
        }
    }
}
