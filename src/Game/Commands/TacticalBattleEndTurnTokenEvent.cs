// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// { ot=1 }
public partial class TacticalBattleEndTurnTokenEvent : TokenEvent
{
    public TacticalBattleEndTurnTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:175
    public new const int ClassID = 2236420;

    // CombatTE.pas:178
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        int TurnIndex;
        bool Found;
        base.Execute(TE);
        RandSeed = AoWHSMap.Current.Random(16777215);
        TurnIndex = CombatMap.Current.FCombat.FCombatData.FTurnOrder.IndexOf((byte)CombatMap.Current.FCurrentCombatPlayer);
        Found = false;
        while (!Found)
        {
            TurnIndex = TurnIndex + 1;
            if (CombatMap.Current.FCombat.FCombatData.FTurnOrder.Count <= TurnIndex)
            {
                TurnIndex = 0;
                CombatMap.Current.FCombat.NewRound();
            }

            for (int I = 0, last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                bool condition = CombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit;
                if (condition)
                {
                    var combatObject = CombatMap.Current.FCombat.FCombatData.GetObjects(I);
                    var turnOrder = CombatMap.Current.FCombat.FCombatData.FTurnOrder[TurnIndex];
                    condition = combatObject.FCombatPlayer == turnOrder;
                }

                if (condition && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                {
                    Found = true;
                }
            }
        }

        var turnOrder2 = CombatMap.Current.FCombat.FCombatData.FTurnOrder[TurnIndex];
        CombatMap.Current.NewTurn((sbyte)turnOrder2);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
