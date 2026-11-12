// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

namespace Aow.Game.Commands;

public partial class TacticalBattleEndCombatTokenEvent : TokenEvent
{
    public TacticalBattleEndCombatTokenEvent(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:158
    public new const int ClassId = 2236421;

    // CombatTE.pas:161
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        base.Execute(tokenEvent);
        for (int I = 0, last = CombatMap.Current.Combat.CombatData.GetCount() - 1; I <= last; I++)
        {
            if (CombatMap.Current.Combat.CombatData.GetObjects(I) != null && CombatMap.Current.Combat.CombatData.GetObjects(I) is CombatUnit && CombatMap.Current.Combat.CombatData.GetObjects(I).GetEnabled() && CombatMap.Current.Combat.CombatData.GetObjects(I).CombatPlayer == CombatMap.Current.CurrentCombatPlayer)
            {
                CombatMap.Current.Combat.CombatData.GetObjects(I).Surrender();
            }
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
