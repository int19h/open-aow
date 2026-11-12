// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Haste : UnitSpell
{
    // UnitSpells.pas:215
    public Haste(ECustomNode owner) : base(owner)
    {
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        SpellName = AbilityRules.TranslateResourceString(GameText.HasteText);
        SpellId = 15;
        BaseManaCost = 5;
        ManaUpkeep = 2;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Air;
        TargetRange = 12;
        UnitDispelResistance = 3;
    }

    // UnitSpells.pas:250
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(152);
    }

    // UnitSpells.pas:229
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(152))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(152)).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
                result = true;
            }
            else
            {
                result = false;
            }
        }
        else
        {
            result = false;
        }

        return result;
    }
}
