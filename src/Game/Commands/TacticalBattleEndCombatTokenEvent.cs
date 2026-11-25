#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

namespace Aow.Game.Commands;

public class TacticalBattleEndCombatTokenEvent(CustomNode owner) : TokenEvent(owner)
{

    // CombatTE.pas:158
    public new const int ClassId = 2_236_421;

    // CombatTE.pas:161
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        _ = base.Execute(tokenEvent);
        for (int i = 0, last = GetEngine().CombatMap.Combat.CombatData.GetCount() - 1; i <= last; i++)
        {
            CombatMap combatMap = GetEngine().CombatMap;
            if (combatMap.Combat.CombatData.GetObjects(i) is not null && combatMap.Combat.CombatData.GetObjects(i) is CombatUnit && combatMap.Combat.CombatData.GetObjects(i).GetEnabled() && combatMap.Combat.CombatData.GetObjects(i).CombatPlayer == combatMap.CurrentCombatPlayer)
            {
                combatMap.Combat.CombatData.GetObjects(i).Surrender();
            }
        }

        return ExecuteResult.Done;
    }
}
