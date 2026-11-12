// Translated from AoWE.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Combat;

public partial class CombatPlayerList : ENode
{
    // Combat2.pas:126
    public CombatPlayerList(ECustomNode owner) : base(owner)
    {
    }

    // Combat2.pas:132
    public CombatPlayer GetPlayers(sbyte index) => (CombatPlayer)_children.List[index];

    // Combat2.pas:135
    public int IndexOfPlayer(byte player)
    {
        int result = _children.Count;
        while (result != 0)
        {
            result -= 1;
            if (player == (byte)((CombatPlayer)_children.List[result]).Player)
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
            P = (CombatPlayer)_children.List[result];
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
