// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Concealment : UnitSpell
{
    // UnitSpells.pas:577
    public Concealment(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_concealmentText);
        SpellId = 39;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Earth;
        TargetRange = 12;
        UnitDispelResistance = 5;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global);
    }

    // UnitSpells.pas:609
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(145) && !target.GetAbilityEnabled(162) && !target.GetAbilityEnabled(54);
    }

    // UnitSpells.pas:590
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(162))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(162)).Enchantment;
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
