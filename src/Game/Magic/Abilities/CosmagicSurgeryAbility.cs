#nullable disable warnings
// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Units;

namespace Aow.Game.Magic.Abilities;

public class CosmagicSurgeryAbility : UnitEnchantmentAbility
{
    // AoWAb.PAS:910
    public CosmagicSurgeryAbility(CustomNode? owner, EngineObject holder) : base(owner, holder: holder)
    {
        AbilityName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.CosmagicSurgeryText);
        AbilityId = 126;
    }

    // AoWAb.PAS:917
    public override UnitEnchantment CreateEnchantment() => new CosmagicSurgeryEnchantment(null) { Holder = CreationHolder() };
}
