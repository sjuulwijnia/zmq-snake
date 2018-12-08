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

using zmq_step2;

namespace zmq_step1_client
{
    class Program
    {
        static void Main(string[] args)
        {
            Jonsole.Setup();

            // retrieve the adress, if no adress is given fall back to a default.
            Jonsole.WriteInteractive("Adress to connect to: ");
            string address = Jonsole.Read();

            if (String.IsNullOrEmpty(address) || address == "default")
                address = "tcp://127.0.0.1:5555";
            Jonsole.ConnectedTo = address;

            // retrieve the name, if no name is given fall back to a default.
            Jonsole.WriteInteractive("Your name: ");
            string name = Jonsole.Read();

            if (String.IsNullOrEmpty(name) || name == "default")
                name = "Client";
            Jonsole.User = name;

            HWClient(address, name);
        }

        public static void HWClient(string address, string name)
        {

            // Create
            using (var context = new ZContext())
            using (var requester = new ZSocket(context, ZSocketType.REQ))
            {
                // Connect
                requester.Connect(address);

                for (int n = 0; n < 1000000; ++n)
                {
                    // receive the message.
                    string message = Jonsole.Read();

                    // construct the query.
                    Message question = new Message()
                    {
                        Data = message,
                        Name = name,
                        Send = Timestamp.FromDateTime(DateTime.UtcNow)
                    };

                    // put the message in the backbuffer.
                    Jonsole.WriteCommon(MessageToString(question));
                    
                    // send out the message.
                    byte[] byteMessage = question.ToByteArray();
                    requester.Send(new ZFrame(byteMessage));

                    // receive a message.
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        Message answer = Message.Parser.ParseFrom(reply);
                        Jonsole.WriteCommon(MessageToString(answer));
                    }
                }
            }
        }

        private static String MessageToString(Message message)
        { return message.Name + " (" + message.Send.ToDateTime().ToShortTimeString() + ")" + ": " + message.Data; }

    }
}
