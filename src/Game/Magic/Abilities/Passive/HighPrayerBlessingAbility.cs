// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type, duration mode.)
public partial class HighPrayerBlessingAbility : DurationAbility
{
    // PassiveAb.pas:1267
    public HighPrayerBlessingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityName = AbilityRules.TranslateRStr(HighPrayerBlessingRStr);
        FAbilityID = 100;
        FAbilityType = AbilityType.Enhancement;
        FDurationMode = DurationMode.EntireCombat;
        FSelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:1277
    public override sbyte GetDefense(AbilityOwner Owner) => 1;

    // PassiveAb.pas:1282
    public override sbyte GetResistance(AbilityOwner Owner) => 1;

    private static readonly string HighPrayerBlessingRStr = "Blessed";
}
