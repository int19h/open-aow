using Aow.Units.Engine;

namespace Aow.Units.HSEngine;

public partial class TMapField
{
    public override void SendMsg(ref TEMsg message, TEObject sender)
    {
        if ((message.MsgID & 0x10000000) != 0)
        {
            for (int i = FCount - 1; i >= 0; i--)
            {
                var sprite = FHSList[i];
                if (sprite.GetLastMsg() == message.Seq) continue;
                if ((message.MsgID & 0x20000000) != 0 && sprite.Owner != this) continue;
                sprite.SetLastMsg(message.Seq);
                sprite.MsgProc(ref message, this);
            }
        }
        else if ((message.MsgID & 0x20000000) == 0)
        {
            for (int i = Count - 1; i >= 0; i--) FHSList[i].MsgProc(ref message, this);
        }
        else
        {
            int count = FCount;
            for (int i = 0; i < count; i++) HSEngine.SendMsgList[i] = FHSList[i];
            for (int i = count - 1; i >= 0; i--)
                if (HSEngine.SendMsgList[i].Owner == this) HSEngine.SendMsgList[i].MsgProc(ref message, this);
        }
    }
}
