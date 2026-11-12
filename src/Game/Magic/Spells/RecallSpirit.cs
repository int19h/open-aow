// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited spell ID, combat statistics, damage mode
public partial class RecallSpirit : CombatSpell
{
    // CombatSpells.pas:1975
    public RecallSpirit(ECustomNode owner) : base(owner)
    {
        SpellName = AbilityRules.TranslateResourceString(s_recallSpiritText);
        SpellId = 129;
        ref var spellCombatInfo = ref CombatInfo;
        spellCombatInfo.Attack = 0;
        spellCombatInfo.Damage = 0;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of();
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = -1;
    }

    // CombatSpells.pas:2008
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        RecallSpiritCombatAction action;
        action = new RecallSpiritCombatAction(null);
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

    // CombatSpells.pas:1990
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

    private static readonly string s_recallSpiritText = "Recall Spirits";
}
