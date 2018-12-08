using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

namespace zmq_step2_server
{
    internal class ServerWorker
    {
        private readonly ZContext _context;
        private readonly int _workerIndex;

        private ZSocket _serverToWorkerSocket;

        public ServerWorker(ZContext context, int workerIndex)
        {
            _context = context;
            _workerIndex = workerIndex;
        }

        public void Start()
        {
            _serverToWorkerSocket = new ZSocket(_context, ZSocketType.DEALER);
            _serverToWorkerSocket.Connect("inproc://server-to-workers");

            _serverToWorkerSocket = new ZSocket(_context, ZSocketType.ROUTER);
            _serverToWorkerSocket.Connect("inproc://workers-to-server");

            WriteLine("Started...");

            ZMessage request;

            while (true)
            {
                if (null == (request = _serverToWorkerSocket.ReceiveMessage(out ZError error)))
                {
                    Stop(error);
                }
                using (request)
                {
                    var identity = request[0].ReadString();
                    var msg = request[1].ReadString();

                    WriteLine($"Received from {identity}: {msg}. Distributing...");

                    using (var message = new ZMessage())
                    {
                        message.Add(new ZFrame(identity));
                        message.Add(new ZFrame(msg));

                        _serverToWorkerSocket.Send(message);
                    }
                }
            }
        }

        public void Stop(ZError error = null)
        {
            _serverToWorkerSocket?.Dispose();

            if (error == null || error == ZError.ETERM)
            {
                // ignore it
                return;
            }

            throw new ZException(error);
        }

        private void WriteLine(string text)
        {
            Console.WriteLine($"[{_workerIndex}] {text}");
        }
    }
}
