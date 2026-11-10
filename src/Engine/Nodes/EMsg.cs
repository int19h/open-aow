// Translated from Engine.pas.
using Aow.Engine.Buffers;

using Aow.Rtl;

namespace Aow.Engine.Nodes;

public struct EMsg
{
    public int MsgID;
    public int Seq;
    public EObject Sender;
    public EMsgParams Param;
    public MsgArg Payload;

    public FixedArray_int_4 Data;
    public int Result;

    internal static void InitMsg(ref EMsg Msg, int MsgID, EObject Sender)
    {
        Msg = default;
        Msg.MsgID = MsgID;
        Msg.Sender = Sender;
        Msg.Result = -1;
    }

}
