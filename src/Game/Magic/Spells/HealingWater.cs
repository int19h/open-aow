// Translated from UnitSpells.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Nodes;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

public partial class HealingWater : UnitSpell
{
    // UnitSpells.pas:467
    public HealingWater(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(HealingWaterRStr);
        FSpellID = 55;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Cosmos;
        FTargetRange = 12;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:491
    public override bool ValidTargetUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(127));
    }

    // UnitSpells.pas:479
    public override bool ExecuteSpell(AbstractUnit Source, AbstractUnit Target, int Mana, ref FixedArray_int_2 Data)
    {
        bool Result;
        if (ValidTargetUnit(Source, Target))
        {
            Target.RemoveAbility(127);
            Target.SetHitPoints(Target.GetHits());
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string HealingWaterRStr = "Healing Water";
}
