// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Units;

namespace Aow.Game.Combat;

public partial class CombatParty : EngineObject
{
    // Combat2.pas:47
    public new const int ClassId = 131746;

    // Combat2.pas:37
    public CombatParty(ECustomNode owner) : base(owner)
    {
        Army = new Army(null) { Holder = CreationHolder() };
    }

    public Army Army { get; }

    public int X { get; set; }
    public int Y { get; set; }
    public int MapLevel { get; set; }
    public byte Wall { get; set; }
    public byte Position { get; set; }
    public byte PlayerIndex { get; set; }

    // Combat2.pas:40
    public override void Dispose()
    {
        Army.Deactivate();
        Army?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:50
    public override void PersistFields(IPersister persister)
    {
        base.PersistFields(persister);
        X = persister.Int32("X", value: X);
        Y = persister.Int32("Y", value: Y);
        MapLevel = persister.Int32("MapLevel", value: MapLevel);
        PlayerIndex = persister.Byte("PlayerIndex", value: PlayerIndex);
        Wall = persister.RawByte("Wall", value: Wall);
        Position = persister.Byte("Position", value: Position);
    }

    // Combat2.pas:61
    public void Activate(Battle combat)
    {
        var players = combat.CombatData.Players.GetPlayers((sbyte)PlayerIndex);
        Army.SetPlayer(players.Player);
    }

    // Combat2.pas:67
    public void Deactivate(Battle combat)
    {
    }

    // intf_P3_02.inc:180

}
