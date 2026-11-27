// Translated from CitySpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Structures;

namespace Aow.Game.Magic.Spells;

public class GoldRush : CityEnchantmentSpell
{
    // CitySpells.pas:710
    public GoldRush(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.GoldRushText);
        SpellId = 53;
        CityTargetMode = CityTargetModes.Own;
        BaseManaCost = 1;
        SpellLevel = 4;
        MagicSphere = MagicSphere.Water;
        EnchantmentId = CityEnchantmentId.GoldRush;
        CastDuration = 20;
        AIPriority = 500;
        AICastSituation = CityEnchantmentCastSpellSituations.Hex4City;
        AIBudget = 600;
        EnchantmentSpellId = 25;
    }
}
