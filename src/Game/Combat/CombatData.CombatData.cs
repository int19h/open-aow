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
        TurnOrder.Clear();
        Players.Clear();
        CombatObjects.Clear();
        Parties.Clear();
    }

    public override void MessageProc(ref EngineMessage message, EngineObject sender)
    {
        base.MessageProc(ref message, sender);
        CombatObjects.MessageProc(ref message, sender);
        Parties.MessageProc(ref message, sender);
    }

    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref X, 0x0a);
        stream.rwInteger(ref Y, 0x0b);
        stream.rwInteger(ref MapLevel, 0x0c);
        RtlUnit.RwSet(ref ImmunityTypes, 2, 0x0d, stream.rwData, 0);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref AttackerPlayer), 0x0e);
        stream.rwByte(ref Unsafe.As<sbyte, byte>(ref DefenderPlayer), 0x0f);
        stream.rwData(Bytes.Of(ref CombatResult, 1), 1, 0x10);
        stream.rwInteger(ref Round, 0x11);
        stream.rwEObject(Players, 0x12);
        stream.rwEObject(CombatObjects, 0x13);
        stream.rwEObject(Parties, 0x14);
        stream.rwString(ref Header, 0x15);
        stream.rwInteger(ref AttackerX, 0x16);
        stream.rwInteger(ref AttackerY, 0x17);
        stream.rwInteger(ref AttackerL, 0x18);
        stream.rwStreamProc(0x19, DefeatedLeaders.LoadFromStream, DefeatedLeaders.SaveToStream);
        stream.rwStreamProc(0x1a, TurnOrder.LoadFromStream, TurnOrder.SaveToStream);
        stream.rwData(Bytes.Of(ref WallType, 1), 1, 0x1b);
        stream.rwBool(ref WallBreached, 0x1c);
        stream.rwStreamProc(0x1d, InvolvedLeaders.LoadFromStream, InvolvedLeaders.SaveToStream);
    }
}
