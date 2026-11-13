using System.Runtime.CompilerServices;
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

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

    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref X, 0x0a);
        stream.Int32(ref Y, 0x0b);
        stream.Int32(ref MapLevel, 0x0c);
        RtlUnit.TransferSet(ref ImmunityTypes, 2, 0x0d, stream.Bytes, 0);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref AttackerPlayer), 0x0e);
        stream.Byte(ref Unsafe.As<sbyte, byte>(ref DefenderPlayer), 0x0f);
        stream.Bytes(Bytes.Of(ref CombatResult, 1), 1, 0x10);
        stream.Int32(ref Round, 0x11);
        stream.Object(Players, 0x12);
        stream.Object(CombatObjects, 0x13);
        stream.Object(Parties, 0x14);
        stream.String(ref Header, 0x15);
        stream.Int32(ref AttackerX, 0x16);
        stream.Int32(ref AttackerY, 0x17);
        stream.Int32(ref AttackerMapLevel, 0x18);
        stream.Payload(0x19, DefeatedLeaders.LoadFromStream, DefeatedLeaders.SaveToStream);
        stream.Payload(0x1a, TurnOrder.LoadFromStream, TurnOrder.SaveToStream);
        stream.Bytes(Bytes.Of(ref WallType, 1), 1, 0x1b);
        stream.Boolean(ref WallBreached, 0x1c);
        stream.Payload(0x1d, InvolvedLeaders.LoadFromStream, InvolvedLeaders.SaveToStream);
    }

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        X = persister.Int32("X", value: X);
        Y = persister.Int32("Y", value: Y);
        MapLevel = persister.Int32("MapLevel", value: MapLevel);
        ImmunityTypes = persister.Set("ImmunityTypes", value: ImmunityTypes);
        AttackerPlayer = PersistSupport.UnsignedByte(persister, name: "AttackerPlayer", value: AttackerPlayer);
        DefenderPlayer = PersistSupport.UnsignedByte(persister, name: "DefenderPlayer", value: DefenderPlayer);
        CombatResult = persister.RawByte("CombatResult", value: CombatResult);
        Round = persister.Int32("Round", value: Round);
        persister.Object("Players", objectValue: Players);
        persister.Object("CombatObjects", objectValue: CombatObjects);
        persister.Object("Parties", objectValue: Parties);
        Header = persister.String("Header", value: Header);
        AttackerX = persister.Int32("AttackerX", value: AttackerX);
        AttackerY = persister.Int32("AttackerY", value: AttackerY);
        AttackerMapLevel = persister.Int32("AttackerMapLevel", value: AttackerMapLevel);
        PersistSupport.ByteList(persister, name: "DefeatedLeaders", list: DefeatedLeaders);
        PersistSupport.ByteList(persister, name: "TurnOrder", list: TurnOrder);
        WallType = persister.RawByte("WallType", value: WallType);
        WallBreached = persister.Boolean("WallBreached", value: WallBreached);
        PersistSupport.ByteList(persister, name: "InvolvedLeaders", list: InvolvedLeaders);
    }
}
