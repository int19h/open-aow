// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Warmonger : CityEnchantmentSpell
{
    // CitySpells.pas:751
    public Warmonger(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_warmongerText);
        SpellId = 58;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        EnchantmentId = CityEnchantmentId.Warmonger;
        CastDuration = 30;
        AIPriority = 600;
        AICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.Hex4City, CityEnchantmentCastSpellSituationElement.Hex3City);
        EnchantmentSpellId = 50;
    }
}
