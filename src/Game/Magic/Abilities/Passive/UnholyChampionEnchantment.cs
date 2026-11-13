// Translated from PassiveAb.pas.
using Aow.Engine.Nodes;

namespace Aow.Game.Magic.Abilities.Passive;

// inherited name, ability ID, selection types, ability type
public partial class UnholyChampionEnchantment : UnitEnchantmentAbility
{
    // PassiveAb.pas:1203
    public UnholyChampionEnchantment(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), GameText.UnholyChampionText);
        AbilityId = 161;
    }
}
