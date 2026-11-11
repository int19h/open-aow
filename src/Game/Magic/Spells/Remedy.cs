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
        FSpellName = AbilityRules.TranslateRStr(RemedyRStr);
        FSpellID = 35;
        FBaseManaCost = 5;
        FSpellLevel = 1;
        FMagicSphere = MagicSphere.Cosmos;
        FTargetRange = 12;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Global, SpellControlTypeElement.Combat);
    }

    // UnitSpells.pas:458
    public override bool ValidTargetUnit(Troop Source, Troop Target)
    {
        bool Result = base.ValidTargetUnit(Source, Target);
        return Result && (Target.GetHitPoints() != Target.GetHits() || Target.GetAbilityEnabled(96) || Target.GetAbilityEnabled(92) || Target.GetAbilityEnabled(108) || Target.GetAbilityEnabled(53) || Target.GetAbilityEnabled(93) || Target.GetAbilityEnabled(127));
    }

    // UnitSpells.pas:436
    public override bool ExecuteSpell(Troop Source, Troop Target, int Mana, ref Int32Array2 Data)
    {
        bool Result;
        int HitPoints;
        if (ValidTargetUnit(Source, Target))
        {
            HitPoints = Target.GetHitPoints() + 5;
            if (HitPoints > Target.GetHits())
            {
                HitPoints = Target.GetHits();
            }

            Target.SetHitPoints((sbyte)HitPoints);
            Target.RemoveAbility(93);
            Target.RemoveAbility(96);
            Target.RemoveAbility(108);
            Target.RemoveAbility(53);
            Target.RemoveAbility(92);
            Target.RemoveAbility(127);
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    private static readonly string RemedyRStr = "Remedy";
}
