// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class FireAura : UnitSpell
{
    // UnitSpells.pas:616
    public FireAura(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.s_fireAuraText);
        SpellId = 40;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Fire;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:629
    public override void PlayDefaultCastEffects(sbyte player, sbyte X, sbyte Y, sbyte layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        if (player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(X, Y, layer, 0))
        {
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:655
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && (!target.GetAbilityEnabled(7) || !target.GetAbilityEnabled(43)) && !target.GetAbilityEnabled(163);
    }

    // UnitSpells.pas:636
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(163))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(163)).Enchantment;
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
