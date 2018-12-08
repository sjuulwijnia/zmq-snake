using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace zmq_step2
{
    public static class Jonsole
    {
        private static int interactiveLeft = 0;
        private static int interactiveTop = 3;

        private static int maximumFeed = 19;
        private static int feedOffset = 5;
        private static LinkedList<string> backbuffer = new LinkedList<string>();

        private static string consoleUser = "";
        private static string connectedTo = "";

        public static void Setup()
        {
            string divider = new String('-', Console.BufferWidth);

            Console.SetCursorPosition(0, 0);
            Console.Title = "Little chat application.";

            Console.SetCursorPosition(0, 1);
            Console.Write(divider);

            Console.SetCursorPosition(0, 4);
            Console.WriteLine(divider);
        }

        public static string Read()
        {
            // read out the message on the std_in.
            string message = Console.ReadLine();

            // clear out the interactive area - this is the only place where we can type.
            ClearLine(interactiveTop);

            // set the cursor position to the start of the interactive area.
            Console.SetCursorPosition(interactiveLeft, interactiveTop);

            // return the message.
            return message;
        }

        public static void ClearLine(int top)
        {
            string empty = new String(' ', Console.BufferWidth);
            Jonsole.WriteAndReset(empty, 0, top);
        }

        /// <summary>
        /// Writes to the interactive part of the console.
        /// </summary>
        public static void WriteInteractive(string message)
        { Write(message, 0, interactiveTop); }

        /// <summary>
        /// Writes to the common 'feed'.
        /// </summary>
        /// <param name="message"></param>
        public static void WriteCommon(String message)
        {
            backbuffer.AddFirst(message);

            while (backbuffer.Count > maximumFeed)
                backbuffer.RemoveLast();

            WriteOutBackbuffer();
        }

        private static void WriteAndReset(string message, int left, int top)
        {
            // keep track of the current position.
            int oLeft, oTop;
            oLeft = Console.CursorLeft;
            oTop = Console.CursorTop;

            Write(message, left, top);

            // reset the cursor position.
            Console.SetCursorPosition(oLeft, oTop);
        }

        private static void Write(string message, int left, int top)
        {
            // write out the message.
            Console.SetCursorPosition(left, top);
            Console.Write(message);
        }

        private static void WriteOutBackbuffer()
        {
            // clear out the entire feed.
            for (int j = 0; j < maximumFeed; j++)
                ClearLine(j + feedOffset);

            // write out the feed messages.
            int count = 0;
            foreach (string s in backbuffer)
            {
                WriteAndReset(s, 0, feedOffset + count);
                count++;
            }
        }

        /// <summary>
        /// Writes out to what IP / port configuration we're connected.
        /// </summary>
        public static string ConnectedTo
        {
            get { return connectedTo; }
            set
            {
                connectedTo = value;
                WriteAndReset("Connected to: " + connectedTo, 0, 0);
            }
        }

        /// <summary>
        /// Writes out the current user(name).
        /// </summary>
        public static string User
        {
            get { return consoleUser; }
            set
            {
                consoleUser = value;
                WriteAndReset("Name: " + consoleUser, 0, 2);
            }
        }

    }
}