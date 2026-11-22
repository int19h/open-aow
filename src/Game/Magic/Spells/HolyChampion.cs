#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.
// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class HolyChampion : UnitSpell
{
    // UnitSpells.pas:497
    public HolyChampion(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.HolyChampionText);
        SpellId = 36;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Life;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
    }

    // UnitSpells.pas:529
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(146) && !target.GetAbilityEnabled(160) && GamePack.Current.AbilityControl[146].CanExpand(target);
    }

    // UnitSpells.pas:510
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(160))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(160).Enchantment;
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
