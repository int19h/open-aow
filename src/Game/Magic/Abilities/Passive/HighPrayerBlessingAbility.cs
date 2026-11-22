#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// (name, ID, selection types, ability type, duration mode.)
public class HighPrayerBlessingAbility : DurationAbility
{
    // PassiveAb.pas:1267
    public HighPrayerBlessingAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: s_highPrayerBlessingText);
        AbilityId = 100;
        AbilityType = AbilityType.Enhancement;
        DurationMode = DurationMode.EntireCombat;
        SelectionTypes = AbilitySelectionTypes.None;
    }

    // PassiveAb.pas:1277
    public override int GetDefense(AbilityOwner owner) => 1;

    // PassiveAb.pas:1282
    public override int GetResistance(AbilityOwner owner) => 1;

    private static readonly string s_highPrayerBlessingText = "Blessed";
}
