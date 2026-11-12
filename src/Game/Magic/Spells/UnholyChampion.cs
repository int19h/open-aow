// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class UnholyChampion : UnitSpell
{
    // UnitSpells.pas:537
    public UnholyChampion(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_unholyChampionText);
        SpellId = 37;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Death;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:569
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(147) && !target.GetAbilityEnabled(161) && AoWHSSet.Current.AbilityControl.GetAbility(147).CanExpand(target);
    }

    // UnitSpells.pas:550
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(161))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(161)).Enchantment;
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
