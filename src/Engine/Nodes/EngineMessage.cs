// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public struct EngineMessage
{
    public int MessageId { get; set; }
    public int Sequence { get; set; }
    public EngineObject Sender { get; set; }

    public EngineMessageParameters Param { get; set; }

    public MsgArg Payload { get; set; }

    public Array4<int> Data { get; set; }

    public int Result { get; set; }

    internal static void InitializeMessage(out EngineMessage message, int messageId, EngineObject sender)
    {
        message = default;
        message.MessageId = messageId;
        message.Sender = sender;
        message.Result = -1;
    }
}
