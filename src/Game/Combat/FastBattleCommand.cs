// Translated from AoWE.pas.
using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P5. }
// ability ID, source DV, target DV, expected DV, move cost, target
public partial class FastBattleCommand
{
    // AIGroup.pas:6705
    public FastBattleCommand(int AbilityID)
    {
        FAbilityID = AbilityID;
    }

    public int FAbilityID;
    public int FSourceDV;
    public int FTargetDV;
    public int FExpectedDV;
    public int FMoveCost;
    public CombatObject FTarget;

    // AIGroup.pas:6710
    public virtual string GetName()
    {
        string Result = AoWHSSet.Current.FAbilityControl.GetAbility(FAbilityID).GetName(null);
        if (FTarget != null)
        {
            Result = Result + " on " + FTarget.IDStr();
        }

        return Result;
    }

    // intf_P5_05.inc:10

}
