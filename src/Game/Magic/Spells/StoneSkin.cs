// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class StoneSkin : UnitSpell
{
    // UnitSpells.pas:256
    public StoneSkin(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.StoneSkinText);
        SpellId = 17;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Earth;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        UnitDispelResistance = 3;
        AIValue = 200;
        CastDuration = 5;
    }

    // UnitSpells.pas:290
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(153);
    }

    // UnitSpells.pas:271
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(153))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(153)).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
            }

            // { Original succeeds even when expansion fails. }
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }
}
