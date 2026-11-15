#nullable disable
// Translated from AoWE.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats.Tree;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Combat;

public partial class CombatParty : EngineObject
{
    // Combat2.pas:47
    public new const int ClassId = 131746;

    // Combat2.pas:37
    public CombatParty(CustomNode owner) : base(owner) => Army = new Army(null) { Holder = CreationHolder() };

    public Army Army { get; }

    public MapCoordinate Location { get; set; }
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
        Location = PersistSupport.Coordinate(persister, name: "Location", value: Location, width: CoordinateWidth.Int32);
        PlayerIndex = persister.Byte("PlayerIndex", value: PlayerIndex);
        Wall = persister.RawByte("Wall", value: Wall);
        Position = persister.Byte("Position", value: Position);
    }

    // Combat2.pas:61
    public void Activate(Battle combat)
    {
        CombatPlayer players = combat.CombatData.Players.GetPlayers((sbyte)PlayerIndex);
        Army.SetPlayer(players.Player);
    }

    // Combat2.pas:67
    public void Deactivate(Battle combat)
    {
    }

    // intf_P3_02.inc:180

}
