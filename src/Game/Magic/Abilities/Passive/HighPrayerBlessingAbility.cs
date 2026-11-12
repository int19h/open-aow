// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type, duration mode.)
public partial class HighPrayerBlessingAbility : DurationAbility
{
    // PassiveAb.pas:1267
    public HighPrayerBlessingAbility(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), s_highPrayerBlessingText);
        AbilityId = 100;
        AbilityType = AbilityType.Enhancement;
        DurationMode = DurationMode.EntireCombat;
        SelectionTypes = Set<AbilitySelectionType>.Of();
    }

    // PassiveAb.pas:1277
    public override sbyte GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:1282
    public override sbyte GetResistance(AbilityOwner owner) => 1;

    private static readonly string s_highPrayerBlessingText = "Blessed";
}
