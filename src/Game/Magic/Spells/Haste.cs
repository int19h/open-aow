// Translated from UnitSpells.pas.
using Aow.Engine.Nodes;
using Aow.Game.Magic.Abilities;
using Aow.Game.Units;
using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Haste : UnitSpell
{
    // UnitSpells.pas:215
    public Haste(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.HasteText);
        SpellId = 15;
        BaseManaCost = 5;
        ManaUpkeep = 2;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Air;
        TargetRange = 12;
        UnitDispelResistance = 3;
    }

    // UnitSpells.pas:250
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && !target.GetAbilityEnabled(152);
    }

    // UnitSpells.pas:229
    public override bool ExecuteSpell(Troop source, Troop target, int mana, Span<int> data)
    {
        if (ValidTargetUnit(source, target))
        {
            if (target.ExpandAbility(152))
            {
                UnitEnchantment enchantment = target.GetAbilityData<UnitEnchantmentAbilityData>(152).Enchantment;
                enchantment.SetUpkeep(ManaUpkeep);
                enchantment.SetSource(source.Player);
                enchantment.DispelResistance = (sbyte)UnitDispelResistance;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
}
