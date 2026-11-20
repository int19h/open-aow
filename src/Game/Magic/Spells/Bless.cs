// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public class Bless : UnitSpell
{
    // UnitSpells.pas:790
    public Bless(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_blessText);
        SpellId = 56;
        BaseManaCost = 5;
        SpellLevel = 3;
        MagicSphere = MagicSphere.Water;
        TargetRange = 12;
        UnitDispelResistance = 3;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        AIValue = 225;
        CastDuration = 5;
    }

    // UnitSpells.pas:824
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(158);
    }

    // UnitSpells.pas:805
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(158))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(158).Enchantment;
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

    private static readonly string s_blessText = "Bless";
}
