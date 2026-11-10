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
        FSpellName = AbilityRules.TranslateRStr(GameText.WarmongerRStr);
        FSpellID = 58;
        FCityTargetMode = Set<CityTargetModeElement>.Of(CityTargetModeElement.Own);
        FEnchantmentID = CityEnchantmentID.Warmonger;
        FCastDuration = 30;
        FAIPriority = 600;
        FAICastSituation = Set<CityEnchantmentCastSpellSituationElement>.Of(CityEnchantmentCastSpellSituationElement.Hex4City, CityEnchantmentCastSpellSituationElement.Hex3City);
        FEnchantmentSpellID = 50;
    }
}
