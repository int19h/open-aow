// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

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
        Army = new Army(null);
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
    public override void ReadWrite(EStorageStream stream)
    {
        base.ReadWrite(stream);
        stream.rwInteger(ref X, 11);
        stream.rwInteger(ref Y, 12);
        stream.rwInteger(ref MapLevel, 13);
        stream.rwByte(ref PlayerIndex, 14);
        stream.rwData(Bytes.Of(ref Wall, 1), 1, 15);
        stream.rwByte(ref Position, 16);
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
