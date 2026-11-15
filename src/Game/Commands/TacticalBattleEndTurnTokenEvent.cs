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
    public TacticalBattleEndTurnTokenEvent(CustomNode owner) : base(owner)
    { }

    // CombatTE.pas:175
    public new const int ClassId = 2236420;

    // CombatTE.pas:178
    public override ExecuteResult Execute(TokenExecutor tokenEvent)
    {
        int turnIndex;
        bool found;
        base.Execute(tokenEvent);
        RandSeed = StrategicMap.Current.Random(16777215);
        turnIndex = CombatMap.Current.Combat.CombatData.TurnOrder.IndexOf((byte)CombatMap.Current.CurrentCombatPlayer);
        found = false;
        while (!found)
        {
            turnIndex = turnIndex + 1;
            if (CombatMap.Current.Combat.CombatData.TurnOrder.Count <= turnIndex)
            {
                turnIndex = 0;
                CombatMap.Current.Combat.NewRound();
            }

            for (int i = 0, last = CombatMap.Current.Combat.CombatData.GetCount() - 1; i <= last; i++)
            {
                bool condition = CombatMap.Current.Combat.CombatData.GetObjects(i) != null && CombatMap.Current.Combat.CombatData.GetObjects(i) is CombatUnit;
                if (condition)
                {
                    var combatObject = CombatMap.Current.Combat.CombatData.GetObjects(i);
                    var turnOrder = CombatMap.Current.Combat.CombatData.TurnOrder[turnIndex];
                    condition = combatObject.CombatPlayer == turnOrder;
                }

                if (condition && CombatMap.Current.Combat.CombatData.GetObjects(i).GetEnabled())
                {
                    found = true;
                }
            }
        }

        var turnOrder2 = CombatMap.Current.Combat.CombatData.TurnOrder[turnIndex];
        CombatMap.Current.NewTurn((sbyte)turnOrder2);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
