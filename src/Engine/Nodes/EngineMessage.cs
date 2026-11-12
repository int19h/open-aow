// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public struct EngineMessage
{
    public int MessageId { get; set; }
    public int Sequence { get; set; }
    public EngineObject Sender { get; set; }

    public EngineMessageParameters Param;

    public MsgArg Payload { get; set; }

    public Int32Array4 Data;

    public int Result { get; set; }

    internal static void InitializeMessage(ref EngineMessage message, int messageId, EngineObject sender)
    {
        message = default;
        message.MessageId = messageId;
        message.Sender = sender;
        message.Result = -1;
    }

}
