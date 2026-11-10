// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class CursedAbility : DurationAbility
{
    // PassiveAb.pas:981
    public CursedAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(CursedRStr);
        FAbilityID = 93;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:1009
    public override sbyte GetDefense(AbilityOwner Owner) => -2;

    // PassiveAb.pas:1015
    public override sbyte GetResistance(AbilityOwner Owner) => -2;

    // PassiveAb.pas:991
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        DurationAbilityData Data;
        Data = (DurationAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data != null && Data.FSource == 1)
        {
            Result = AbilityRules.TranslateRStr(GameText.DeathMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:1003
    public override int GetTotalDuration() => 3;

    private static readonly string CursedRStr = "Cursed";
}
