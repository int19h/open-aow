// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

namespace Aow.Game.Commands;

public partial class TacticalBattleEndCombatTokenEvent : TokenEvent
{
    public TacticalBattleEndCombatTokenEvent(CustomNode owner) : base(owner)
    { }

    // CombatTE.pas:158
    public new const int ClassId = 2236421;

    // CombatTE.pas:161
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        base.Execute(tokenEvent);
        for (int i = 0, last = CombatMap.Current.Combat.CombatData.GetCount() - 1; i <= last; i++)
        {
            if (CombatMap.Current.Combat.CombatData.GetObjects(i) != null && CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(i).CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
            {
                CombatMap.Current.Combat.CombatData.GetObjects(i).Surrender();
            }
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
