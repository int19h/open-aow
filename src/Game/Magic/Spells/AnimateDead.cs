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
    public AnimateDead(ECustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), s_animateDeadText);
        SpellId = 130;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
    }

    // CombatSpells.pas:2079
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        AnimateDeadCombatAction action;
        action = new AnimateDeadCombatAction(null) { Holder = CreationHolder() };
        action.SourceId = source.Id;
        action.TargetId = target.Id;
        action.Succeeded = target.Status.Contains(CombatObjectStatusElement.Destroyed);
        action.SpellId = SpellId;
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        return action;
    }

    // CombatSpells.pas:2061
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        if (target.Status.Contains(CombatObjectStatusElement.Destroyed))
        {
            // { Original leaves the other damage-value fields unchanged on success. }
            values.SourceDamage = 200;
            values.DamageTypes = Set<DamageType>.Of();
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = Set<DamageType>.Of();
        }
    }

    private static readonly string s_animateDeadText = "Animate Dead";
}
