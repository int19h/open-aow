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
    public CombatPlayer GetPlayers(sbyte Index) => (CombatPlayer)FChildren.List[Index];

    // Combat2.pas:135
    public int IndexOfPlayer(byte Player)
    {
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            if (Player == (byte)((CombatPlayer)FChildren.List[Result]).FPlayer)
            {
                return Result;
            }
        }

        return -1;
    }

    // Combat2.pas:146
    public int IndexOf(byte Player, byte Relation)
    {
        CombatPlayer P;
        int Result = FChildren.Count;
        while (Result != 0)
        {
            Result -= 1;
            P = (CombatPlayer)FChildren.List[Result];
            if ((sbyte)Player == P.FPlayer && P.FIndependentRelation == Relation)
            {
                return Result;
            }
        }

        return -1;
    }

    // Combat2.pas:159
    public void Delete(int Index)
    {
        GetPlayers((sbyte)Index)?.Dispose();
    }

    // intf_P3_02.inc:212

}
