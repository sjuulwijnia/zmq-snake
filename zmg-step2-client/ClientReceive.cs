using System;
using ZeroMQ;

namespace zmq_step2_client
{
    internal class ClientReceive
    {
        private readonly ZContext _context = null;

        public ClientReceive(ZContext context)
        {
            _context = context;
        }

        public void Start()
        {
            using (var subscriptionClient = new ZSocket(_context, ZSocketType.SUB))
            {
                subscriptionClient.Connect("tcp://127.0.0.1:5556");
                subscriptionClient.SubscribeAll();

                while (true)
                {
                    using (var message = subscriptionClient.ReceiveMessage())
                    {
                        Console.WriteLine(message[0].ReadString() + ": " + message[1].ReadString());
                    }
                }
            }
        }
    }
}
