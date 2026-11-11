// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public struct EngineMessage
{
    public int MsgID;
    public int Seq;
    public EngineObject Sender;
    public EngineMessageParameters Param;
    public MsgArg Payload;

    public Int32Array4 Data;
    public int Result;

    internal static void InitMsg(ref EngineMessage Msg, int MsgID, EngineObject Sender)
    {
        Msg = default;
        Msg.MsgID = MsgID;
        Msg.Sender = Sender;
        Msg.Result = -1;
    }

}
