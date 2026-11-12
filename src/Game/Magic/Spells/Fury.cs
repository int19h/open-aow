// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Fury : UnitSpell
{
    // UnitSpells.pas:336
    public Fury(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(GameText.FuryText);
        SpellId = 33;
        AIValue = 175;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Fire;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        UnitDispelResistance = 3;
    }

    // UnitSpells.pas:369
    public override void PlayDefaultCastEffects(sbyte player, sbyte X, sbyte Y, sbyte layer)
    {
        base.PlayDefaultCastEffects(player, X, Y, layer);
        if (player == AoWHSMap.Current.SeatedPlayer || AoWHSMap.Current.Watching(X, Y, layer, 0))
        {
            AoWHSSet.Current.SpellControl.GetSpell(SpellId).CastSounds.Play(1);
        }
    }

    // UnitSpells.pas:376
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        result = result && !target.GetAbilityEnabled(156);
        if (result)
        {
            if (AoWHSMap.Current.Players.GetPlayers(target.Player).PlayerType != PlayerType.Human && target is Leader)
            {
                result = false;
            }
        }

        return result;
    }

    // UnitSpells.pas:350
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        UnitEnchantment enchantment;
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(156))
            {
                enchantment = ((UnitEnchantmentAbilityData)target.GetAbilityData(156)).Enchantment;
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
