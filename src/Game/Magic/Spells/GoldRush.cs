// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class GoldRush : CityEnchantmentSpell
{
    // CitySpells.pas:710
    public GoldRush(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_goldRushText);
        SpellId = 53;
        CityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        EnchantmentId = CityEnchantmentId.GoldRush;
        CastDuration = 20;
        AIPriority = 500;
        AICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.Hex4City);
        AIBudget = 600;
        EnchantmentSpellId = 25;
    }
}
