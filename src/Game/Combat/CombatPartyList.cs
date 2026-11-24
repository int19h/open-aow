#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Combat;

public sealed class CombatPartyList(CustomNode? owner) : OwningNode(owner), IChildList<CombatParty>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatParty, so the cast never fails.
    public CombatParty this[int index] => GetChild<CombatParty>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatParty;

    int IChildSink<CombatParty>.AddChild(CombatParty child) => AddChild(child);

    // Combat2.pas:70
    public override ControlStyles GetControlStyle() => ControlStyles.SaveLoad | ControlStyles.ArrayMode;

    // Combat2.pas:73
    public CombatParty CreateParty(Battle combat, byte position, byte combatPlayer, int X, int Y, int L, Army? army)
    {
        CombatParty p;
        if (GetParty(position) is null)
        {
            p = new CombatParty(null)
            {
                Holder = CreationHolder(),
                PlayerIndex = combatPlayer,
                Position = position,
                Location = new MapCoordinate((sbyte)X, (sbyte)Y, (sbyte)L)
            };
            p.SetArrayOwner(this, index: position);
            p.Army.SetMapCoordinate(X.TruncateToSByte(), Y.TruncateToSByte(), L.TruncateToSByte());
            CombatPlayer players = combat.CombatData.Players.GetPlayers((sbyte)combatPlayer);
            p.Army.SetPlayer(PlayerId.FromOriginal(players.Player));
            if (army is not null)
            {
                p.Army.AssignSettings(army);
            }

            p.Army.Activate();
        }
        else
        {
            throw new InvalidOperationException("CombatParty already exists!");
        }

        return p;
    }

    // Combat2.pas:96
    public CombatParty? GetParty(byte position) => position < Count ? this[position] : null;

    // Combat2.pas:99
    public void Activate(Battle combat)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null)
            {
                this[i].Activate(combat);
            }
        }
    }

    // Combat2.pas:107
    public void Deactivate(Battle combat)
    {
        for (int i = 0, last = Count - 1; i <= last; i++)
        {
            if (GetChild(i) is not null)
            {
                this[i].Deactivate(combat);
            }
        }
    }
}
