#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Commands;

// { ot=1 }
public class TacticalBattleEndTurnTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // CombatTE.pas:175
    public new const int ClassId = 2_236_420;

    // CombatTE.pas:178
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        RandSeed = GetEngine().StrategicMap.Random(16777215);
        CombatMap combatMap = GetEngine().CombatMap;
        int turnIndex = combatMap.Combat.CombatData.TurnOrder.IndexOf((byte)combatMap.CurrentCombatPlayer);
        bool found = false;
        while (!found)
        {
            turnIndex++;
            CombatMap engineCombatMap = GetEngine().CombatMap;
            if (engineCombatMap.Combat.CombatData.TurnOrder.Count <= turnIndex)
            {
                turnIndex = 0;
                engineCombatMap.Combat.NewRound();
            }

            CombatMap getCountCombatMap = GetEngine().CombatMap;
            for (int i = 0, last = getCountCombatMap.Combat.CombatData.GetCount() - 1; i <= last; i++)
            {
                bool condition = getCountCombatMap.Combat.CombatData.GetObjects(i) is not null and CombatUnit;
                if (condition)
                {
                    CombatObject combatObject = getCountCombatMap.Combat.CombatData.GetObjects(i);
                    int turnOrder = getCountCombatMap.Combat.CombatData.TurnOrder[turnIndex];
                    condition = combatObject.CombatPlayer == turnOrder;
                }

                if (condition && getCountCombatMap.Combat.CombatData.GetObjects(i).GetEnabled())
                {
                    found = true;
                }
            }
        }

        CombatMap newTurnCombatMap = GetEngine().CombatMap;
        int turnOrder2 = newTurnCombatMap.Combat.CombatData.TurnOrder[turnIndex];
        newTurnCombatMap.NewTurn((sbyte)turnOrder2);
        return ExecuteResult.Done;
    }
}
