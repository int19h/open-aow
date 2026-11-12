// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public struct EngineMessage
{
    public int MessageId;
    public int Sequence;
    public EngineObject Sender;
    public EngineMessageParameters Param;
    public MsgArg Payload;

    public Int32Array4 Data;
    public int Result;

    internal static void InitializeMessage(ref EngineMessage message, int messageId, EngineObject sender)
    {
        message = default;
        message.MessageId = messageId;
        message.Sender = sender;
        message.Result = -1;
    }

}
