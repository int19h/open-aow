#nullable disable
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.Units;

using Aow.Rtl;

namespace Aow.Game.Magic.Spells;

// inherited combat statistics, damage mode
public class TurnUndead : CombatSpell
{
    // CombatSpells.pas:888
    public TurnUndead(CustomNode owner, EngineObject holder) : base(owner)
    {
        Holder = holder;
        SpellName = AbilityRules.TranslateResourceString(GetEngine(), GameText.TurnUndeadText);
        SpellId = 101;
        DamageMode = 1;
        SpellCombatInfo spellCombatInfo = CombatInfo;
        spellCombatInfo.Attack = 6;
        spellCombatInfo.Damage = 5;
        spellCombatInfo.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        spellCombatInfo.RepeatCount = 1;
        spellCombatInfo.Range = 12;

        CombatInfo = spellCombatInfo;
    }

    // CombatSpells.pas:905
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        TurnUndeadSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SpellId = SpellId,
            SourceId = source.Id,
            TargetId = target.Id
        };
        if (chargeMana)
        {
            action.ManaCost = BaseManaCost;
        }

        if (target is CombatUnit targetUnit && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(targetUnit.Troop.GetUnitType()) && (targetUnit.Troop.GetRace() == 11 || targetUnit.Troop.GetAbilityEnabled(129)))
        {
            action.Generate(source, target, CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, DamageMode);
        }

        return action;
    }

    // CombatSpells.pas:922
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        if (target is CombatUnit targetUnit && Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(targetUnit.Troop.GetUnitType()) && (targetUnit.Troop.GetRace() == 11 || targetUnit.Troop.GetAbilityEnabled(129)))
        {
            base.FastBattleGetDamageValues(source, target, wallSituation, ref values);
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
}
