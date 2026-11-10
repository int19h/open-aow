using System.Runtime.CompilerServices;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatData
{
    public override void Clear()
    {
        base.Clear();
        FTurnOrder.Clear();
        FPlayers.Clear();
        FCombatObjects.Clear();
        FParties.Clear();
    }

    public override void MsgProc(ref EMsg message, EObject sender)
    {
        base.MsgProc(ref message, sender);
        FCombatObjects.MsgProc(ref message, sender);
        FParties.MsgProc(ref message, sender);
    }

    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref FX, 0x0a);
        stream.rwInteger(ref FY, 0x0b);
        stream.rwInteger(ref FL, 0x0c);
        RtlUnit.RwSet(ref F_CombatData_22, 2, 0x0d, stream.rwData, 0);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FAttackerPlayer), 0x0e);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FDefenderPlayer), 0x0f);
        stream.rwData(Bytes.Of(ref F_CombatData_30, 1), 1, 0x10);
        stream.rwInteger(ref FRound, 0x11);
        stream.rwEObject(FPlayers, 0x12);
        stream.rwEObject(FCombatObjects, 0x13);
        stream.rwEObject(FParties, 0x14);
        stream.rwString(ref FHeader, 0x15);
        stream.rwInteger(ref FAttackerX, 0x16);
        stream.rwInteger(ref FAttackerY, 0x17);
        stream.rwInteger(ref FAttackerL, 0x18);
        stream.rwStreamProc(0x19, FDefeatedLeaders.LoadFromStream, FDefeatedLeaders.SaveToStream);
        stream.rwStreamProc(0x1a, FTurnOrder.LoadFromStream, FTurnOrder.SaveToStream);
        stream.rwData(Bytes.Of(ref FWallType, 1), 1, 0x1b);
        stream.rwBool(ref FWallBreached, 0x1c);
        stream.rwStreamProc(0x1d, FInvolvedLeaders.LoadFromStream, FInvolvedLeaders.SaveToStream);
    }
}
