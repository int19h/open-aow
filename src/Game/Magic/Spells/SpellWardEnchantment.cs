// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SpellWardEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1943
    public new const int ClassID = 135971;

    // GlobalEnchantmentSpells.pas:1935
    public SpellWardEnchantment(ECustomNode owner) : base(owner)
    {
        FName = AbilityRules.TranslateRStr(GameText.SpellWardRStr);
        FEnchantmentType = GlobalEnchantmentType.SpellWard;
        FDispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:1946
    public override int GetAIUpkeepPriority() => 500;

}
