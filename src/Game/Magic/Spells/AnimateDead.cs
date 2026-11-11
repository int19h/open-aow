// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class AnimateDead : CombatSpell
{
    // CombatSpells.pas:2046
    public AnimateDead(ECustomNode owner) : base(owner)
    {
        FSpellName = AbilityRules.TranslateRStr(AnimateDeadRStr);
        FSpellID = 130;
        ref var spellCombatInfo = ref FCombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
    }

    // CombatSpells.pas:2079
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        AnimateDeadCombatAction Action;
        Action = new AnimateDeadCombatAction(null);
        Action.FSourceID = Source.FID;
        Action.FTargetID = Target.FID;
        Action.FSucceeded = Target.FStatus.Contains(CombatObjectStatusElement.Destroyed);
        Action.FSpellID = FSpellID;
        if (ChargeMana)
        {
            Action.FManaCost = FBaseManaCost;
        }

        return Action;
    }

    // CombatSpells.pas:2061
    public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        if (Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
        {
            // { Original leaves the other damage-value fields unchanged on success. }
            Values.SourceDamage = 200;
            Values.DamageTypes = Set<DamageType>.Of();
        }
        else
        {
            Values.SourceDamage = 0;
            Values.SourceCost = 0;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            Values.DamageTypes = Set<DamageType>.Of();
        }
    }

    private static readonly string AnimateDeadRStr = "Animate Dead";
}
