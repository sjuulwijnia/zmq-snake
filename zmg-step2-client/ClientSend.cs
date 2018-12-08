using Google.Protobuf.WellKnownTypes;
using System;
using ZeroMQ;

namespace zmq_step2.client
{
    internal class ClientSend
    {
        private readonly string _clientName = null;
        private readonly ZContext _context = null;

        public ClientSend(ZContext context)
        {
            Console.Write("Name: ");

            _clientName = Console.ReadLine();
            _context = context;
        }

        public void Start()
        {
            using (var sendClient = new ZSocket(_context, ZSocketType.DEALER))
            {
                sendClient.IdentityString = _clientName;
                sendClient.Connect("tcp://127.0.0.1:5555");

                while (true)
                {
                    Console.Write("Send: ");

                    var text = Console.ReadLine();
                    var message = new Message
                    {
                        Name = _clientName,
                        Text = text,
                        Timestamp = Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    using (var sendMessage = message.ToZMessage())
                    {
                        if (!sendClient.Send(sendMessage, out ZError error))
                        {
                            if (error == ZError.ETERM)
                                return;    // Interrupted
                            throw new ZException(error);
                        }
                    }
                }
            }
        }
    }
}
