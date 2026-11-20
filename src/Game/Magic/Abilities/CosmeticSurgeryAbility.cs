#nullable disable
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public class CosmeticSurgeryAbility : UnitEnchantmentAbility
{
    // AoWAb.PAS:910
    public CosmeticSurgeryAbility(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CosmeticSurgeryText);
        AbilityId = 126;
    }

    // AoWAb.PAS:917
    public override UnitEnchantment CreateEnchantment() => new CosmeticSurgeryEnchantment(null) { Holder = CreationHolder() };
}
