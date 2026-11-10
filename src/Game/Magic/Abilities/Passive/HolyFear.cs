// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type, duration mode
public partial class HolyFear : DurationAbility
{
    // PassiveAb.pas:816
    public HolyFear(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(HolyFearRStr);
        FAbilityID = 53;
        FAbilityType = AbilityType.Handicap;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
        FDurationMode = DurationMode.Day;
    }

    // PassiveAb.pas:833
    public override string GetSourceName(AbilityOwner Owner)
    {
        string Result;
        // { Original assumes the ability data exists. }
        var durationAbilityData = (DurationAbilityData)Owner.GetAbilityData(FAbilityID);
        if (durationAbilityData.FSource == 1)
        {
            Result = AbilityRules.TranslateRStr(GameText.LifeMasteryRStr);
        }
        else
        {
            Result = base.GetSourceName(Owner);
        }

        return Result;
    }

    // PassiveAb.pas:827
    public override int GetTotalDuration() => 3;

    private static readonly string HolyFearRStr = "Holy Fear";
}
