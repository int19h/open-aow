#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class FreeMovement : UnitSpell
{
    // UnitSpells.pas:386
    public FreeMovement(CustomNode owner, EngineObject holder) : base(owner, holder: holder)
    {
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.FreeMovementText);
        SpellId = 32;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Earth;
        TargetRange = 12;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
        UnitDispelResistance = 4;
    }

    // UnitSpells.pas:418
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(157) && (!target.GetAbilityEnabled(3) || !target.GetAbilityEnabled(5) || !target.GetAbilityEnabled(4));
    }

    // UnitSpells.pas:399
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(157))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(157).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = UnitDispelResistance.TruncateToSByte();
            }

            // { Original succeeds even when expansion fails. }
            return true;
        }
        else
        {
            return false;
        }
    }
}
