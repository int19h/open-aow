// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Engine.Storage;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatParty : EObject
{
    // Combat2.pas:47
    public new const int ClassID = 131746;

    // Combat2.pas:37
    public CombatParty(ECustomNode owner) : base(owner)
    {
        FArmy = new Army(null);
    }

    public Army FArmy;
    public int F_CombatParty_0C;
    public int F_CombatParty_10;
    public int F_CombatParty_14;
    public byte FWall;
    public byte F_CombatParty_19;
    public byte FPlayerIndex;

    // Combat2.pas:40
    public override void Dispose()
    {
        FArmy.Deactivate();
        FArmy?.Dispose();
        base.Dispose();
    }

    // Combat2.pas:50
    public override void ReadWrite(EStorageStream Stream)
    {
        base.ReadWrite(Stream);
        Stream.rwInteger(ref F_CombatParty_0C, 11);
        Stream.rwInteger(ref F_CombatParty_10, 12);
        Stream.rwInteger(ref F_CombatParty_14, 13);
        Stream.rwByte(ref FPlayerIndex, 14);
        Stream.rwData(Bytes.Of(ref FWall, 1), 1, 15);
        Stream.rwByte(ref F_CombatParty_19, 16);
    }

    // Combat2.pas:61
    public void Activate(Battle ACombat)
    {
        var players = ACombat.FCombatData.FPlayers.GetPlayers((sbyte)FPlayerIndex);
        FArmy.SetPlayer(players.FPlayer);
    }

    // Combat2.pas:67
    public void Deactivate(Battle ACombat)
    {
    }

    // intf_P3_02.inc:180

}
