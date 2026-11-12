// Translated from AoWE.pas.
using Aow.Game.World.Map;

namespace Aow.Game.Combat;

// { Interface declaration run owned by P5. }
// ability ID, source DV, target DV, expected DV, move cost, target
public partial class FastBattleCommand
{
    // AIGroup.pas:6705
    public FastBattleCommand(int abilityId)
    {
        AbilityId = abilityId;
    }

    public int AbilityId { get; }
    public int SourceDamageValue { get; set; }
    public int TargetDamageValue { get; set; }
    public int ExpectedDamageValue { get; set; }
    public int MoveCost { get; set; }
    public CombatObject Target { get; set; }

    // AIGroup.pas:6710
    public virtual string GetName()
    {
        string result = AoWHSSet.Current.AbilityControl.GetAbility(AbilityId).GetName(null);
        if (Target != null)
        {
            result = result + " on " + Target.IdString();
        }

        return result;
    }

    // intf_P5_05.inc:10

}
