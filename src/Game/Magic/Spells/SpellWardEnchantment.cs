// Translated from GlobalEnchantmentSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

namespace Aow.Game.Magic.Spells;

public partial class SpellWardEnchantment : GlobalEnchantment
{
    // GlobalEnchantmentSpells.pas:1943
    public new const int ClassId = 135971;

    // GlobalEnchantmentSpells.pas:1935
    public SpellWardEnchantment(ECustomNode owner) : base(owner)
    {
        Name = AbilityRules.TranslateResourceString(GameText.SpellWardText);
        EnchantmentType = GlobalEnchantmentType.SpellWard;
        DispelMana = 50;
    }

    // GlobalEnchantmentSpells.pas:1946
    public override int GetAIUpkeepPriority() => 500;

}
