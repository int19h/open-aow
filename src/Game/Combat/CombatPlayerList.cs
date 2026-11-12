// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

public partial class CombatPlayerList : ENode, IChildList<CombatPlayer>
{
    // The child in slot index, read as the original reads the list, without a range check.
    // AcceptsChild admits only a CombatPlayer, so the cast never fails.
    public CombatPlayer this[int index] => (CombatPlayer)_children.List[index];

    protected sealed override bool AcceptsChild(EngineObject child) => child is CombatPlayer;

    int IChildSink<CombatPlayer>.AddChild(CombatPlayer child) => AddChild(child);

    // Combat2.pas:126
    public CombatPlayerList(ECustomNode owner) : base(owner)
    {
    }

    // Combat2.pas:132
    public CombatPlayer GetPlayers(sbyte index) => this[index];

    // Combat2.pas:135
    public int IndexOfPlayer(byte player)
    {
        int result = _children.Count;
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
        CombatPlayer P;
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            P = this[result];
            if ((sbyte)player == P.Player && P.IndependentRelation == relation)
            {
                return result;
            }
        }

        return -1;
    }

    // Combat2.pas:159
    public void Delete(int index)
    {
        GetPlayers((sbyte)index)?.Dispose();
    }

    // intf_P3_02.inc:212

}
