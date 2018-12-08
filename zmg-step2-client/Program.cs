using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroMQ;

using zmq_step2;

namespace zmq_step2.client
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
                address = "tcp://127.0.0.1.5555";
            Jonsole.ConnectedTo = address;

            // retrieve the name, if no name is given fall back to a default.
            Jonsole.WriteInteractive("Your name: ");
            string name = Jonsole.Read();

            if (String.IsNullOrEmpty(name) || name == "default")
                name = "Server";
            Jonsole.User = name;

            new ClientHost().Start();
        }
    }
}
