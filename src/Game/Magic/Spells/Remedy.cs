// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class Remedy : UnitSpell
{
    // UnitSpells.pas:424
    public Remedy(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_remedyText);
        SpellId = 35;
        BaseManaCost = 5;
        SpellLevel = 1;
        MagicSphere = MagicSphere.Cosmos;
        TargetRange = 12;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:458
    public override bool ValidTargetUnit(Troop source, Troop target)
    {
        bool result = base.ValidTargetUnit(source, target);
        return result && (target.GetHitPoints() != target.GetHits() || target.GetAbilityEnabled(96) || target.GetAbilityEnabled(92) || target.GetAbilityEnabled(108) || target.GetAbilityEnabled(53) || target.GetAbilityEnabled(93) || target.GetAbilityEnabled(127));
    }

    // UnitSpells.pas:436
    public override bool ExecuteSpell(Troop source, Troop target, int mana, ref Int32Array2 data)
    {
        bool result;
        int hitPoints;
        if (ValidTargetUnit(source, target))
        {
            hitPoints = target.GetHitPoints() + 5;
            if (hitPoints > target.GetHits())
            {
                hitPoints = target.GetHits();
            }

            target.SetHitPoints((sbyte)hitPoints);
            target.RemoveAbility(93);
            target.RemoveAbility(96);
            target.RemoveAbility(108);
            target.RemoveAbility(53);
            target.RemoveAbility(92);
            target.RemoveAbility(127);
            result = true;
        }
        else
        {
            result = false;
        }

        return result;
    }

    private static readonly string s_remedyText = "Remedy";
}
