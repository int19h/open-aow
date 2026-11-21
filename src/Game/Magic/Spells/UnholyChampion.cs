// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;
using Aow.Game.World.Map;

namespace Aow.Game.Magic.Spells;

public class UnholyChampion : UnitSpell
{
    // UnitSpells.pas:537
    public UnholyChampion(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), S: GameText.UnholyChampionText);
        SpellId = 37;
        BaseManaCost = 5;
        SpellLevel = 2;
        MagicSphere = MagicSphere.Death;
        TargetRange = 12;
        UnitDispelResistance = 4;
        SpellControlTypes = SpellControlTypes.Global | SpellControlTypes.Combat;
    }

    // UnitSpells.pas:569
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(147) && !target.GetAbilityEnabled(161) && GamePack.Current.AbilityControl[147].CanExpand(target);
    }

    // UnitSpells.pas:550
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(161))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(161).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
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
