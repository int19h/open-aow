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
    public new const int ClassID = 2236421;

    // CombatTE.pas:161
    public override ExecuteResult Execute(TokenExecutor TE)
    {
        base.Execute(TE);
        for (int I = 0, last = CombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
        {
            if (CombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && CombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && CombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && CombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == CombatMap.Current.FCurrentCombatPlayer)
            {
                CombatMap.Current.FCombat.FCombatData.GetObjects(I).Surrender();
            }
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
