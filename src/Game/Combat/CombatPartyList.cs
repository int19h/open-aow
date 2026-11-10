// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Combat;

public partial class CombatPartyList : ENode
{
    public CombatPartyList(ECustomNode owner) : base(owner)
    { }

    // Combat2.pas:70
    public override Set<EControlStyleElement> GetControlStyle() => Set<EControlStyleElement>.Of(EControlStyleElement.SaveLoad, EControlStyleElement.ArrayMode);

    // Combat2.pas:73
    public CombatParty CreateParty(Battle ACombat, byte Position, byte CombatPlayer, int X, int Y, int L, Army Army)
    {
        CombatParty P;
        if (GetParty(Position) == null)
        {
            P = new CombatParty(null);
            P.FPlayerIndex = CombatPlayer;
            P.F_CombatParty_19 = Position;
            P.F_CombatParty_0C = X;
            P.F_CombatParty_10 = Y;
            P.F_CombatParty_14 = L;
            P.SetArrayOwner(this, Position);
            P.FArmy.SetXYL((sbyte)(byte)X, (sbyte)(byte)Y, (sbyte)L);
            var players = ACombat.FCombatData.FPlayers.GetPlayers((sbyte)CombatPlayer);
            P.FArmy.SetPlayer(players.FPlayer);
            if (Army != null)
            {
                P.FArmy.AssignSettings(Army);
            }

            P.FArmy.Activate();
        }
        else
        {
            throw new InvalidOperationException("CombatParty already exists!");
        }

        return P;
    }

    // Combat2.pas:96
    public CombatParty GetParty(byte Position)
    {
        CombatParty Result;
        if (Position < FChildren.Count)
        {
            Result = (CombatParty)FChildren.List[Position];
        }
        else
        {
            Result = null;
        }

        return Result;
    }

    // Combat2.pas:99
    public void Activate(Battle ACombat)
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null)
            {
                ((CombatParty)FChildren.List[I]).Activate(ACombat);
            }
        }
    }

    // Combat2.pas:107
    public void Deactivate(Battle ACombat)
    {
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (FChildren.List[I] != null)
            {
                ((CombatParty)FChildren.List[I]).Deactivate(ACombat);
            }
        }
    }

    // Engine.pas:631

}
