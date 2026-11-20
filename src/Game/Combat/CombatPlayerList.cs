// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

public class CombatPlayerList(CustomNode owner) : OwningNode(owner), IChildList<CombatPlayer>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatPlayer, so the cast never fails.
    public CombatPlayer this[int index] => GetChild<CombatPlayer>(index);

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatPlayer;

    int IChildSink<CombatPlayer>.AddChild(CombatPlayer child) => AddChild(child);

    // Combat2.pas:132
    public CombatPlayer GetPlayers(sbyte index) => this[index];

    // The combat setup of the caller holds only the named player kind, so the cast never fails there.
    public T GetPlayers<T>(sbyte index) where T : CombatPlayer => (T)this[index];

    // Combat2.pas:135
    public int IndexOfPlayer(byte player)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            if (player == (byte)this[result].Player)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat2.pas:146
    public int IndexOf(byte player, byte relation)
    {
        int result = Count;
        while (result != 0)
        {
            result -= 1;
            CombatPlayer p = this[result];
            if ((sbyte)player == p.Player && p.IndependentRelation == relation)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat2.pas:159
    public void Delete(int index) => GetPlayers((sbyte)index)?.Dispose();
}
