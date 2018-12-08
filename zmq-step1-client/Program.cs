using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

using Protobuf;

using Google.Protobuf;
using Google.Protobuf.Reflection;
using Google.Protobuf.WellKnownTypes;

namespace zmq_step1_client
{
    class Program
    {
        static void Main(string[] args)
        {
            HWClient(args);
        }

        public static void HWClient(string[] args)
        {

            //
            // Hello World client
            //
            // Author: metadings
            //

            if (args == null || args.Length < 1)
            {
                Console.WriteLine();
                Console.WriteLine("Usage: ./{0} HWClient [Endpoint]", AppDomain.CurrentDomain.FriendlyName);
                Console.WriteLine();
                Console.WriteLine("    Endpoint  Where HWClient should connect to.");
                Console.WriteLine("              Default is tcp://127.0.0.1:5555");
                Console.WriteLine();
                args = new string[] { "tcp://127.0.0.1:5555" };
            }

            string endpoint = args[0];

            // Create
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {

                Console.WriteLine("What is your name?");
                string name = Console.ReadLine();

                // Connect
                requester.Connect(endpoint);

                for (int n = 0; n < 1000000; ++n)
                {
                    Console.Write(name + ": ");
                    string data = Console.ReadLine();

                    Message question = new Message()
                    {
                        Data = data,
                        Name = name,
                        Send = Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    byte[] message = question.ToByteArray();
                    requester.Send(new ZFrame(message));

                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        Message answer = Message.Parser.ParseFrom(reply);
                        Console.WriteLine(MessageToString(answer));
                    }
                }
            }
        }

        private static String MessageToString(Message message)
        { return message.Name + " (" + message.Send.ToDateTime().ToShortTimeString() + ")" + ": " + message.Data; }

    }
}
