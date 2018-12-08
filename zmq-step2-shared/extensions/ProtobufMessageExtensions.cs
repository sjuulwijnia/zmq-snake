using Google.Protobuf;
using ZeroMQ;

namespace zmq_step2
{
    public static class ProtobufMessageExtensions
    {
        public static Message ToMessage(this ZFrame frame)
        {
            return Message.Parser.ParseFrom(frame);
        }

        public static Message ToMessage(this ZMessage message, int frameIndex = 1)
        {
            return message[frameIndex].ToMessage();
        }

        public static ZFrame ToZFrame(this Message message)
        {
            return new ZFrame(message.ToByteArray());
        }

        public static ZMessage ToZMessage(this Message message)
        {
            return new ZMessage
            {
                message.ToZFrame()
            };
        }
    }
}
