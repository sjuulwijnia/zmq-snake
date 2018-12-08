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
            HWServer(args);
        }

        public static void HWServer(string[] args)
        {
            // Create
            using (var context = new ZContext())
            using (var responder = new ZSocket(context, ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5555");

                Console.WriteLine("What is your name?");
                string name = Console.ReadLine();

                while (true)
                {
                    // Receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        Console.WriteLine("Bob, you here?");
                        // receive a message.
                        Message answer = Message.Parser.ParseFrom(request);
                        Console.WriteLine(MessageToString(answer));

                        // send a message
                        Console.Write(name + ": ");
                        string data = Console.ReadLine();

                        Message question = new Message()
                        {
                            Data = data,
                            Name = name,
                            Send = Timestamp.FromDateTime(DateTime.UtcNow)
                        };

                        byte[] message = question.ToByteArray();
                        responder.Send(new ZFrame(message));
                    }
                }
            }

        }

        private static String MessageToString(Message message)
        { return message.Name + " (" + message.Send.ToDateTime().ToShortTimeString() + ")" + ": " + message.Data; }

    }
}