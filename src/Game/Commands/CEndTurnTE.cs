// Translated from CombatTE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;

using Aow.Game.Net;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Commands;

// { ot=1 }
public partial class CEndTurnTE : TokenEvent
{
    public CEndTurnTE(ECustomNode owner) : base(owner)
    { }

    // CombatTE.pas:175
    public new const int ClassID = 2236420;

    // CombatTE.pas:178
    public override ExecuteResult Execute(TokenExecuter TE)
    {
        int TurnIndex;
        bool Found;
        base.Execute(TE);
        RandSeed = AoWHSMap.Current.Random(16777215);
        TurnIndex = AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder.IndexOf((byte)AoWCombatMap.Current.FCurrentCombatPlayer);
        Found = false;
        while (!Found)
        {
            TurnIndex = TurnIndex + 1;
            if (AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder.Count <= TurnIndex)
            {
                TurnIndex = 0;
                AoWCombatMap.Current.FCombat.NewRound();
            }

            for (int I = 0, last = AoWCombatMap.Current.FCombat.FCombatData.GetCount() - 1; I <= last; I++)
            {
                bool condition = AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) != null && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I) is CombatUnit;
                if (condition)
                {
                    var combatObject = AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I);
                    var turnOrder = AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder[TurnIndex];
                    condition = combatObject.FCombatPlayer == turnOrder;
                }

                if (condition && AoWCombatMap.Current.FCombat.FCombatData.GetObjects(I).GetEnabled())
                {
                    Found = true;
                }
            }
        }

        var turnOrder2 = AoWCombatMap.Current.FCombat.FCombatData.FTurnOrder[TurnIndex];
        AoWCombatMap.Current.NewTurn((sbyte)turnOrder2);
        return ExecuteResult.Done;
    }

    // NetworkE.pas:230

}
