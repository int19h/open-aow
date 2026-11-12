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
    public override Set<ControlStyleElement> GetControlStyle() => Set<ControlStyleElement>.Of(ControlStyleElement.SaveLoad, ControlStyleElement.ArrayMode);

    // Combat2.pas:73
    public CombatParty CreateParty(Battle combat, byte position, byte combatPlayer, int X, int Y, int L, Army army)
    {
        CombatParty P;
        if (GetParty(position) == null)
        {
            P = new CombatParty(null);
            P.PlayerIndex = combatPlayer;
            P.Position = position;
            P.X = X;
            P.Y = Y;
            P.MapLevel = L;
            P.SetArrayOwner(this, position);
            P.Army.SetMapCoordinate((sbyte)(byte)X, (sbyte)(byte)Y, (sbyte)L);
            var players = combat.CombatData.Players.GetPlayers((sbyte)combatPlayer);
            P.Army.SetPlayer(players.Player);
            if (army != null)
            {
                P.Army.AssignSettings(army);
            }

            P.Army.Activate();
        }
        else
        {
            throw new InvalidOperationException("CombatParty already exists!");
        }

        return P;
    }

    // Combat2.pas:96
    public CombatParty GetParty(byte position)
    {
        CombatParty result;
        if (position < _children.Count)
        {
            result = (CombatParty)_children.List[position];
        }
        else
        {
            result = null;
        }

        return result;
    }

    // Combat2.pas:99
    public void Activate(Battle combat)
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null)
            {
                ((CombatParty)_children.List[I]).Activate(combat);
            }
        }
    }

    // Combat2.pas:107
    public void Deactivate(Battle combat)
    {
        for (int I = 0, last = _children.Count - 1; I <= last; I++)
        {
            if (_children.List[I] != null)
            {
                ((CombatParty)_children.List[I]).Deactivate(combat);
            }
        }
    }

    // Engine.pas:631

}
