// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public partial class CosmeticSurgeryAbility : UnitEnchantmentAbility
{
    // AoWAb.PAS:910
    public CosmeticSurgeryAbility(ECustomNode owner) : base(owner)
    {
        AbilityName = AbilityRules.TranslateResourceString(GameText.CosmeticSurgeryText);
        AbilityId = 126;
    }

    // AoWAb.PAS:917
    public override UnitEnchantment CreateEnchantment() => new CosmeticSurgeryEnchantment(null);
}
