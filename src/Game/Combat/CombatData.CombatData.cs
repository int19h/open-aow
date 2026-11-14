using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

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
