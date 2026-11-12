// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class HolyChampionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1195
    public HolyChampionEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.HolyChampionText);
        AbilityId = 160;
    }
}
