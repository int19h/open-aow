// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class LiquidForm : UnitSpell
{
    // UnitSpells.pas:741
    public LiquidForm(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_liquidFormText);
        SpellId = 49;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
        AIValue = 300;
        TargetRange = 12;
        UnitDispelResistance = 5;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:783
    public override void PlayDefaultCastEffects(sbyte player, sbyte X, sbyte Y, sbyte layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        if (player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(X, Y, layer, 0))
        {
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:774
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        result = result && !target.GetAbilityEnabled(166);
        return result && (!target.GetAbilityEnabled(13) && !target.GetAbilityEnabled(77) || !target.GetAbilityEnabled(2) && !target.GetAbilityEnabled(1) && !target.GetAbilityEnabled(59));
    }

    // UnitSpells.pas:755
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(166))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(166)).Enchantment;
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
