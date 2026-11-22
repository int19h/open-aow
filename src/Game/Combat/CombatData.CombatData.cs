#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Game.Combat;

public sealed partial class CombatData
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

    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        Location = persister.Coordinate(name: "Location", value: Location, width: CoordinateWidth.Int32);
        ImmunityTypes = persister.Set("ImmunityTypes", value: ImmunityTypes);
        AttackerPlayer = persister.UnsignedByte(name: "AttackerPlayer", value: AttackerPlayer);
        DefenderPlayer = persister.UnsignedByte(name: "DefenderPlayer", value: DefenderPlayer);
        CombatResult = persister.RawByte("CombatResult", value: CombatResult.TruncateToByte());
        Round = persister.Int32("Round", value: Round);
        persister.Object("Players", objectValue: Players);
        persister.Object("CombatObjects", objectValue: CombatObjects);
        persister.Object("Parties", objectValue: Parties);
        Header = persister.String("Header", value: Header);
        AttackerLocation = persister.Coordinate(name: "AttackerLocation", value: AttackerLocation, width: CoordinateWidth.Int32);
        persister.ByteList(name: "DefeatedLeaders", list: DefeatedLeaders);
        persister.ByteList(name: "TurnOrder", list: TurnOrder);
        WallType = persister.RawByte("WallType", value: WallType.TruncateToByte());
        WallBreached = persister.Boolean("WallBreached", value: WallBreached);
        persister.ByteList(name: "InvolvedLeaders", list: InvolvedLeaders);
    }
}
