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
        FSpellName = AbilityRules.TranslateRStr(GameText.GoldRushRStr);
        FSpellID = 53;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        FBaseManaCost = 1;
        FSpellLevel = 4;
        FMagicSphere = MagicSphere.Water;
        FEnchantmentID = CityEnchantmentId.GoldRush;
        FCastDuration = 20;
        FAIPriority = 500;
        FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.Hex4City);
        FAIBudget = 600;
        FEnchantmentSpellID = 25;
    }
}
