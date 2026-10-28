using System.Runtime.CompilerServices;
using Aow.Rtl;
using Aow.Units.Engine;

namespace Aow.Units.AoWE;

public partial class TCombatData
{
    private TENode CombatObjects => Rtl.Rtl.ReferenceFromId<TENode>(F_CombatData_34);

    public override void Clear()
    {
        base.Clear();
        FTurnOrder.Clear();
        FPlayers.Clear();
        CombatObjects.Clear();
        FParties.Clear();
    }

    public override void MsgProc(ref TEMsg message, TEObject sender)
    {
        base.MsgProc(ref message, sender);
        CombatObjects.MsgProc(ref message, sender);
        FParties.MsgProc(ref message, sender);
    }

    public override void ReadWrite(TEStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref FX, 0x0a);
        stream.rwInteger(ref FY, 0x0b);
        stream.rwInteger(ref FL, 0x0c);
        Rtl.Rtl.RwSet(ref F_CombatData_22, 2, 0x0d, stream.rwData, 0);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttackerPlayer), 0x0e);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefenderPlayer), 0x0f);
        stream.rwData(Bytes.Of(ref F_CombatData_30, 1), 1, 0x10);
        stream.rwInteger(ref FRound, 0x11);
        stream.rwEObject(FPlayers, 0x12);
        stream.rwEObject(CombatObjects, 0x13);
        stream.rwEObject(FParties, 0x14);
        stream.rwString(ref FHeader, 0x15);
        stream.rwInteger(ref FAttackerX, 0x16);
        stream.rwInteger(ref FAttackerY, 0x17);
        stream.rwInteger(ref FAttackerL, 0x18);
        var order = Rtl.Rtl.ReferenceFromId<TByteList>(F_CombatData_2C);
        stream.rwStreamProc(0x19, order.LoadFromStream, order.SaveToStream);
        stream.rwStreamProc(0x1a, FTurnOrder.LoadFromStream, FTurnOrder.SaveToStream);
        stream.rwData(Bytes.Of(ref FWallType, 1), 1, 0x1b);
        stream.rwBool(ref FWallBreached, 0x1c);
        var leaders = Rtl.Rtl.ReferenceFromId<TByteList>(F_CombatData_28);
        stream.rwStreamProc(0x1d, leaders.LoadFromStream, leaders.SaveToStream);
    }
}
