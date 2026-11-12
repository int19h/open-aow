// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HolyChampion : UnitSpell
{
    // UnitSpells.pas:497
    public HolyChampion(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.HolyChampionText);
        SpellId = 36;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Life;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:529
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(146) && !target.GetAbilityEnabled(160) && GamePack.Current.AbilityControl.GetAbility(146).CanExpand(target);
    }

    // UnitSpells.pas:510
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(160))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(160)).Enchantment;
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
