// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class Warmonger : CityEnchantmentSpell
{
    // CitySpells.pas:751
    public Warmonger(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.WarmongerText);
        SpellId = 58;
        CityTargetMode = CityTargetModes.Own;
        EnchantmentId = CityEnchantmentId.Warmonger;
        CastDuration = 30;
        AIPriority = 600;
        AICastSituation = CityEnchantmentCastSpellSituations.Hex4City | CityEnchantmentCastSpellSituations.Hex3City;
        EnchantmentSpellId = 50;
    }
}
