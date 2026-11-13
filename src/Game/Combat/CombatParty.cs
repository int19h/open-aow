// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;
using Aow.Formats.Tree;

using Aow.Game.Units;

using Aow.Rtl;

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

    public int X;
    public int Y;
    public int MapLevel;
    public byte Wall;
    public byte Position;
    public byte PlayerIndex;

    // Combat2.pas:40
    public override void Dispose()
    {
        Army.Deactivate();
        Army?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:50
    public override void ReadWrite(StorageStream stream)
    {
        base.ReadWrite(stream);
        stream.Int32(ref X, 11);
        stream.Int32(ref Y, 12);
        stream.Int32(ref MapLevel, 13);
        stream.Byte(ref PlayerIndex, 14);
        stream.Bytes(Bytes.Of(ref Wall, 1), 1, 15);
        stream.Byte(ref Position, 16);
    }

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
