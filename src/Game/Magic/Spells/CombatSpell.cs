#nullable disable warnings
// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Magic.Spells;

// combat statistics (attack, damage, types, repeats, range, radius, targets). damage mode
public class CombatSpell : Spell
{
    public SpellCombatInfo CombatInfo { get; protected init; }

    public int DamageMode { get; protected init; }

    // CombatSpells.pas:632
    public CombatSpell(CustomNode? owner) : base(owner)
    {
        SpellType = SpellType.Combat;
        SpellControlTypes = SpellControlTypes.Combat;
        SpellCombatInfo combatInfo = CombatInfo;
        combatInfo.Radius = 1;
        CombatInfo = combatInfo;
        SpellCombatInfo combatInfo2 = CombatInfo;
        combatInfo2.TargetTypes = CombatTargetTypes.Enemy;
        CombatInfo = combatInfo2;
        CastDuration = 5;
        DamageMode = 0;
    }

    // CombatSpells.pas:655
    public override CombatAction CreateCombatAction(CombatObject? source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action = new(null)
        {
            Holder = CreationHolder(),
            SpellId = SpellId
        };
        action.Generate(source, target, CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, DamageMode);
        if (chargeMana && source is CombatUnit sourceUnit)
        {
            action.ManaCost = sourceUnit.Troop.CastingMana(this);
        }

        return action;
    }

    // CombatSpells.pas:643
    public override void GetCombatInfo(Troop? source, ref SpellCombatInfo info) => info = CombatInfo;

    // CombatSpells.pas:648
    public override int GetCombatDamageValue(CombatObject source, CombatObject target)
    {
        int hitPoints = target.HitPoints;
        int defense = target.GetDefense();
        int resistance = target.GetResistance();
        DamageTypes immunityTypes = target.GetImmunityTypes();
        DamageTypes protectionTypes = target.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, maximum: hitPoints, defense, resistance, immunityTypes, protectionTypes) * CombatInfo.RepeatCount;
    }

    // CombatSpells.pas:668
    public override void GetCombatDamageValues(CombatObject? source, CombatObject? target, ref AbilityDamageValues values)
    {
        DamageTypes protection = target.GetProtectionTypes();
        DamageTypes immunity = target.GetImmunityTypes();
        values.DamageTypes = CombatInfo.DamageTypes;
        int hitPoints = target.HitPoints;
        int defense = target.GetDefense();
        int resistance = target.GetResistance();
        int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, maximum: hitPoints, defense, resistance, immunityTypes: immunity, protectionTypes: protection);
        values.SourceDamage = statisticsToLimitedDamageValue * CombatInfo.RepeatCount;
        // { Original recomputes the value, using resistance in place of defense for mode 1. }
        if (DamageMode == 1)
        {
            int targetHitPoints = target.HitPoints;
            int targetResistance = target.GetResistance();
            int resistance2 = target.GetResistance();
            int statisticsToLimitedDamageValue2 = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, maximum: targetHitPoints, defense: targetResistance, resistance: resistance2, immunityTypes: immunity, protectionTypes: protection);
            values.SourceDamage = statisticsToLimitedDamageValue2 * CombatInfo.RepeatCount;
        }
        else
        {
            int a2HitPoints = target.HitPoints;
            int targetDefense = target.GetDefense();
            int a2Resistance = target.GetResistance();
            int statisticsToLimitedDamageValue2 = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, maximum: a2HitPoints, defense: targetDefense, resistance: a2Resistance, immunityTypes: immunity, protectionTypes: protection);
            values.SourceDamage = statisticsToLimitedDamageValue2 * CombatInfo.RepeatCount;
        }

        int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(CombatInfo.Damage, CombatInfo.DamageTypes, immunityTypes: protection, protectionTypes: immunity);
        values.SourceCost = statisticsToMaxDamage * CombatInfo.RepeatCount;
        values.TargetDamage = 0;
        values.TargetCost = 0;
    }

    // CombatSpells.pas:754
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        string reason = "";
        Troop? hero = commands.Source is CombatUnit sourceUnit ? sourceUnit.Troop : null;
        if (hero is not null && hero.CanCastSpellInstantly(SpellId, mana: BaseManaCost, ref reason))
        {
            int distance = commands.Combat.RoundDistance;
            if (CombatInfo.Range == -1)
            {
                distance += 8;
            }
            else
            {
                if (CombatInfo.Range >= 12)
                {
                    distance += 4;
                }
                else
                {
                    if (CombatInfo.Range >= 8)
                    {
                        distance += 2;
                    }
                }
            }

            AbilityDamageValues values = default;
            for (int i = 0, last = commands.TargetSide.GetCount() - 1; i <= last; i++)
            {
                CombatObject target = commands.TargetSide.GetObjects(i);
                if (target.GetEnabled() && target.FastBattleRoundDistance() <= distance)
                {
                    FastBattleGetDamageValues(wallSituation: commands.Source.Combat.GetWallSituation(commands.Source, target), source: commands.Source, target: target, values: ref values);
                    if (values.SourceDamage != 0)
                    {
                        FastBattleSpellCommand command = new(52, creator: this)
                        {
                            Target = commands.TargetSide.GetObjects(i),
                            MoveCost = 1,
                            SpellId = SpellId,
                            ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, targetDamageValue: values.TargetDamage, commands.Source, target)
                        };
                        commands.Add(command);
                    }
                }
            }
        }
    }

    // CombatSpells.pas:791
    public override bool FastBattleExecuteCombatCommand(FastBattleUnit source, FastBattleCommand command)
    {
        CombatObject? target = command.Target;
        CombatAction action;
        for (int i = 0, last = CombatInfo.RepeatCount - 1; i <= last; i++)
        {
            if (!target.Status.Contains(CombatObjectStatuses.Destroyed))
            {
                action = CreateCombatAction(source, target, chargeMana: i == 0);
                source.Combat.ExecuteCombatAction(action);
                _ = action.Release();
            }
        }

        if (CombatInfo.Radius > 1)
        {
            Battle combat = source.Combat;
            DelphiList<CombatObject> targets = [];
            int distance = combat.RoundDistance;
            if (CombatInfo.Range == -1)
            {
                distance += 8;
            }
            else
            {
                if (CombatInfo.Range >= 12)
                {
                    distance += 4;
                }
                else
                {
                    if (CombatInfo.Range >= 8)
                    {
                        distance += 2;
                    }
                }
            }

            CombatObject? other;
            for (int i = combat.Sides[(int)target.Side].GetCount() - 1; i >= 0; i--)
            {
                CombatSideIndex side = target.Side;
                other = combat.Sides[(int)side].GetObjects(i);
                bool condition = other != target && other.GetEnabled() && other.Position >>> 4 == target.Position >>> 4 && distance >= other.FastBattleRoundDistance();
                if (condition)
                {
                    DamageTypes immunityTypes = other.GetImmunityTypes();
                    condition = (CombatInfo.DamageTypes & ~immunityTypes) != DamageTypes.None;
                }

                if (condition)
                {
                    _ = targets.Add(other);
                }
            }

            // { Original shifts Random(4) by two, so this adds no random extra targets. }
            int count = (StrategicMap.Current.Random(4) >>> 2) + CombatInfo.Radius;
            while (count != 0 && targets.Count != 0)
            {
                other = targets[StrategicMap.Current.Random(targets.Count)];
                for (int j = 0, last = CombatInfo.RepeatCount - 1; j <= last; j++)
                {
                    if (!other.Status.Contains(CombatObjectStatuses.Destroyed))
                    {
                        action = CreateCombatAction(source, target: other, chargeMana: false);
                        source.Combat.ExecuteCombatAction(action);
                        _ = action.Release();
                    }
                }

                _ = targets.Remove(other);
                count -= 1;
            }

            targets?.Dispose();
        }

        return true;
    }

    // CombatSpells.pas:693
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        if (wallSituation == WallSituation.None || CombatInfo.Range is -1 or >= 8)
        {
            DamageTypes protection = target.GetProtectionTypes();
            DamageTypes immunity = target.GetImmunityTypes();
            DamageTypes damageTypes = CombatInfo.DamageTypes;
            int damage = CombatInfo.Damage;
            int defense = target.GetDefense();
            if (wallSituation == WallSituation.Target && ((DamageTypes.Magic | DamageTypes.Death | DamageTypes.Holy) & CombatInfo.DamageTypes) == DamageTypes.None)
            {
                defense += 2;
            }

            values.DamageTypes = damageTypes;
            int hitPoints = target.HitPoints;
            int resistance = target.GetResistance();
            int statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, damage, damageTypes, maximum: hitPoints, defense, resistance, immunityTypes: immunity, protectionTypes: protection);
            values.SourceDamage = statisticsToLimitedDamageValue * CombatInfo.RepeatCount;
            if (CombatInfo.Radius > 1)
            {
                values.SourceDamage = (values.SourceDamage * CombatInfo.Radius) >>> 1;
            }

            int statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(damage, damageTypes, immunityTypes: protection, protectionTypes: immunity);
            values.SourceCost = statisticsToMaxDamage * CombatInfo.RepeatCount;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            if (values.SourceDamage != 0 && CombatInfo.Radius > 1)
            {
                Battle combat = source.Combat;
                int count = 0;
                int distance = combat.RoundDistance;
                if (CombatInfo.Range == -1)
                {
                    distance += 8;
                }
                else
                {
                    if (CombatInfo.Range >= 12)
                    {
                        distance += 4;
                    }
                    else
                    {
                        if (CombatInfo.Range >= 8)
                        {
                            distance += 2;
                        }
                    }
                }

                for (int i = combat.Sides[(int)target.Side].GetCount() - 1; i >= 0; i--)
                {
                    CombatSideIndex side = target.Side;
                    CombatObject other = combat.Sides[(int)side].GetObjects(i);
                    if (target != other && other.GetEnabled() && other.Position >>> 4 == target.Position >>> 4 && other.FastBattleRoundDistance() <= distance && (damageTypes & ~other.GetImmunityTypes()) != DamageTypes.None)
                    {
                        count += 1;
                    }
                }

                if (count != 0)
                {
                    if (CombatInfo.Radius < count)
                    {
                        count = CombatInfo.Radius;
                    }

                    values.SourceDamage += Round(values.SourceDamage * count * 0.5);
                }
            }
        }
        else
        {
            values.SourceDamage = 0;
            values.SourceCost = 0;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            values.DamageTypes = DamageTypes.None;
        }
    }
}
