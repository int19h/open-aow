// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

namespace Aow.Game.Commands;

public partial class CEndCombatTE : TokenEvent
{
    public CEndCombatTE(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:158
    public new const int ClassID = 2236421;

    // CombatTE.pas:161
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        base.Execute(TE);
        for (int I = 0, last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
        {
            if (AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled() && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).FCombatPlayer == AoWCombatMap.Current.FCurrentCombatPlayer)
            {
                AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).Surrender();
            }
        }

        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
