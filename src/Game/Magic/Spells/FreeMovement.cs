// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class FreeMovement : UnitSpell
{
    // UnitSpells.pas:386
    public FreeMovement(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_freeMovementText);
        SpellId = 32;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Earth;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        UnitDispelResistance = 4;
    }

    // UnitSpells.pas:418
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(157) && (!target.GetAbilityEnabled(3) || !target.GetAbilityEnabled(5) || !target.GetAbilityEnabled(4));
    }

    // UnitSpells.pas:399
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(157))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(157)).Enchantment;
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
