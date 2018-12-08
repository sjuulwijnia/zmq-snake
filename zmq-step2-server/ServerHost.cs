using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ZeroMQ;

namespace zmq_step2_server
{
    internal enum ServerHostState
    {
        CLOSED = 1,
        OPEN = 2
    }

    internal class ServerHost : IDisposable
    {
        private readonly ZContext _context;
        private readonly int _workerCount = 5;

        private ServerHostState state = ServerHostState.CLOSED;
        private ZSocket _serverToWorkersSocket = null;
        private ZSocket _serverFrontendPublishSocket = null;
        private ZSocket _serverFrontendReceiveSocket = null;
        private ZSocket _workersToServerSocket = null;

        public ServerHost(int workerCount)
        {
            if (workerCount < 1)
            {
                workerCount = 5;
            }

            _context = new ZContext();
            _workerCount = workerCount;
        }

        public void Dispose()
        {
            if (state == ServerHostState.OPEN)
            {
                // close existing sockets
                Stop();
            }

            _context.Dispose();
        }

        public void Start()
        {
            if (state == ServerHostState.OPEN)
            {
                // don't open again
                return;
            }

            Console.WriteLine("[HOST] Started...");
            state = ServerHostState.OPEN;

            // create our sockets
            _serverFrontendPublishSocket = new ZSocket(_context, ZSocketType.PUB);
            _serverFrontendReceiveSocket = new ZSocket(_context, ZSocketType.ROUTER);

            // bind both sockets
            _serverFrontendReceiveSocket.Bind("tcp://*:5555");
            _serverFrontendPublishSocket.Bind("tcp://*:5556");

            //while (true)
            //{
            //    using (var receivedMessage = _serverFrontendReceiveSocket.ReceiveMessage())
            //    {
            //        var identity = receivedMessage[0].ReadString();
            //        var message = receivedMessage[1].ReadString();

            //        Console.WriteLine($"{identity}: {message}");

            //        using (var sendMessage = new ZMessage())
            //        {
            //            sendMessage.Add(new ZFrame(identity));
            //            sendMessage.Add(new ZFrame(message));

            //            _serverFrontendPublishSocket.Send(sendMessage);
            //        }
            //    }
            //}

            ZContext.Proxy(_serverFrontendReceiveSocket, _serverFrontendPublishSocket);
        }

        public void Stop()
        {
            if (state == ServerHostState.CLOSED)
            {
                return;
            }

            Console.WriteLine("[HOST] Stopped...");
            state = ServerHostState.CLOSED;

            _serverToWorkersSocket?.Dispose();
            _serverFrontendReceiveSocket?.Dispose();

            _serverToWorkersSocket = null;
            _serverFrontendReceiveSocket = null;
        }
    }
}
