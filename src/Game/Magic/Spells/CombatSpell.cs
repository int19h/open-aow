// Translated from CombatSpells.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Magic.Abilities;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Spells;

// combat statistics (attack, damage, types, repeats, range, radius, targets). damage mode
public partial class CombatSpell : Spell
{
    public SpellCombatInfo CombatInfo;

    public byte DamageMode { get; protected set; }

    // CombatSpells.pas:632
    public CombatSpell(ECustomNode owner) : base(owner)
    {
        SpellType = SpellType.Combat;
        SpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Combat);
        CombatInfo.Radius = 1;
        CombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
        CastDuration = 5;
        DamageMode = 0;
    }

    // CombatSpells.pas:655
    public override CombatAction CreateCombatAction(CombatObject source, CombatObject target, bool chargeMana)
    {
        CombatSpellCombatAction action;
        action = new CombatSpellCombatAction(null);
        action.SpellId = SpellId;
        action.Generate(source, target, CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, DamageMode);
        if (chargeMana)
        {
            action.ManaCost = ((Hero)((CombatUnit)source).Troop).CastingMana(this);
        }

        return action;
    }

    // CombatSpells.pas:643
    public override void GetCombatInfo(Troop source, ref SpellCombatInfo info)
    {
        info = CombatInfo;
    }

    // CombatSpells.pas:648
    public override int GetCombatDamageValue(CombatObject source, CombatObject target)
    {
        var hitPoints = target.GetHitPoints();
        var defense = target.GetDefense();
        var resistance = target.GetResistance();
        var immunityTypes = target.GetImmunityTypes();
        var protectionTypes = target.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes) * CombatInfo.RepeatCount;
    }

    // CombatSpells.pas:668
    public override void GetCombatDamageValues(CombatObject source, CombatObject target, ref AbilityDamageValues values)
    {
        Set<DamageType> protection;
        Set<DamageType> immunity;
        protection = target.GetProtectionTypes();
        immunity = target.GetImmunityTypes();
        values.DamageTypes = CombatInfo.DamageTypes;
        var hitPoints = target.GetHitPoints();
        var defense = target.GetDefense();
        var resistance = target.GetResistance();
        var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, hitPoints, defense, resistance, immunity, protection);
        values.SourceDamage = statisticsToLimitedDamageValue * CombatInfo.RepeatCount;
        // { Original recomputes the value, using resistance in place of defense for mode 1. }
        if (DamageMode == 1)
        {
            var targetHitPoints = target.GetHitPoints();
            var targetResistance = target.GetResistance();
            var resistance2 = target.GetResistance();
            var statisticsToLimitedDamageValue2 = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, targetHitPoints, targetResistance, resistance2, immunity, protection);
            values.SourceDamage = statisticsToLimitedDamageValue2 * CombatInfo.RepeatCount;
        }
        else
        {
            var a2HitPoints = target.GetHitPoints();
            var targetDefense = target.GetDefense();
            var a2Resistance = target.GetResistance();
            var statisticsToLimitedDamageValue2 = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, CombatInfo.Damage, CombatInfo.DamageTypes, a2HitPoints, targetDefense, a2Resistance, immunity, protection);
            values.SourceDamage = statisticsToLimitedDamageValue2 * CombatInfo.RepeatCount;
        }

        var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(CombatInfo.Damage, CombatInfo.DamageTypes, protection, immunity);
        values.SourceCost = statisticsToMaxDamage * CombatInfo.RepeatCount;
        values.TargetDamage = 0;
        values.TargetCost = 0;
    }

    // CombatSpells.pas:754
    public override void FastBattlePrefetchCombatCommands(FastBattleCommandList commands)
    {
        Troop hero;
        int distance;
        string reason = "";
        CombatObject target;
        FastBattleSpellCommand command;
        AbilityDamageValues values = default;
        hero = ((CombatUnit)commands.Source).Troop;
        if (hero is Hero && ((Hero)hero).CanCastSpellInstantly(SpellId, BaseManaCost, ref reason))
        {
            distance = commands.Combat.GetRoundDistance();
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

            for (int I = 0, last = commands.TargetSide.GetCount() - 1; I <= last; I++)
            {
                target = commands.TargetSide.GetObjects(I);
                if (target.GetEnabled() && target.FastBattleRoundDistance() <= distance)
                {
                    var wallSituation = commands.Source.Combat.GetWallSituation(commands.Source, target);
                    FastBattleGetDamageValues(commands.Source, target, wallSituation, ref values);
                    if (values.SourceDamage != 0)
                    {
                        command = new FastBattleSpellCommand(52);
                        command.Target = commands.TargetSide.GetObjects(I);
                        command.MoveCost = 1;
                        command.SpellId = SpellId;
                        command.ExpectedDamageValue = AbilityRules.DamageValueToDamageEvaluation(values.SourceDamage, values.TargetDamage, commands.Source, target);
                        commands.Add(command);
                    }
                }
            }
        }
    }

    // CombatSpells.pas:791
    public override bool FastBattleExecuteCombatCommand(CombatObject source, FastBattleCommand command)
    {
        int count;
        CombatAction action;
        CombatObject other;
        int distance;
        CombatObject target;
        DelphiList targets;
        Combat.Battle combat;
        target = command.Target;
        for (int I = 0, last = CombatInfo.RepeatCount - 1; I <= last; I++)
        {
            if (!target.Status.Contains(CombatObjectStatusElement.Destroyed))
            {
                action = CreateCombatAction(source, target, I == 0);
                source.Combat.ExecuteCombatAction(action);
                action.Release();
            }
        }

        if (CombatInfo.Radius > 1)
        {
            combat = source.Combat;
            targets = new DelphiList();
            distance = combat.GetRoundDistance();
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

            for (int I = combat.Sides[(int)target.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = target.GetSide();
                other = combat.Sides[(int)side].GetObjects(I);
                bool condition = other != target && other.GetEnabled() && (int)(((uint)other.Position) >> 4) == (int)(((uint)target.Position) >> 4) && distance >= other.FastBattleRoundDistance();
                if (condition)
                {
                    var immunityTypes = other.GetImmunityTypes();
                    condition = CombatInfo.DamageTypes - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    targets.Add(other);
                }
            }

            // { Original shifts Random(4) by two, so this adds no random extra targets. }
            count = (int)(((uint)AoWHSMap.Current.Random(4)) >> 2) + CombatInfo.Radius;
            while (count != 0 && targets.Count != 0)
            {
                other = (CombatObject)targets[AoWHSMap.Current.Random(targets.Count)];
                for (int J = 0, last = CombatInfo.RepeatCount - 1; J <= last; J++)
                {
                    if (!other.Status.Contains(CombatObjectStatusElement.Destroyed))
                    {
                        action = CreateCombatAction(source, other, false);
                        source.Combat.ExecuteCombatAction(action);
                        action.Release();
                    }
                }

                targets.Remove(other);
                count -= 1;
            }

            targets?.Dispose();
        }

        return true;
    }

    // CombatSpells.pas:693
    public override void FastBattleGetDamageValues(CombatObject source, CombatObject target, WallSituation wallSituation, ref AbilityDamageValues values)
    {
        Set<DamageType> damageTypes;
        Set<DamageType> protection;
        Set<DamageType> immunity;
        int count;
        int distance;
        CombatObject other;
        Combat.Battle combat;
        int damage;
        int defense;
        if (wallSituation == WallSituation.None || CombatInfo.Range == -1 || CombatInfo.Range >= 8)
        {
            protection = target.GetProtectionTypes();
            immunity = target.GetImmunityTypes();
            damageTypes = CombatInfo.DamageTypes;
            damage = CombatInfo.Damage;
            defense = target.GetDefense();
            if (wallSituation == WallSituation.Target && (Set<DamageType>.Of(DamageType.Magic, DamageType.Death, DamageType.Holy) & CombatInfo.DamageTypes) == Set<DamageType>.Of())
            {
                defense += 2;
            }

            values.DamageTypes = damageTypes;
            var hitPoints = target.GetHitPoints();
            var resistance = target.GetResistance();
            var statisticsToLimitedDamageValue = AbilityRules.StatisticsToLimitedDamageValue(CombatInfo.Attack, damage, damageTypes, hitPoints, defense, resistance, immunity, protection);
            values.SourceDamage = statisticsToLimitedDamageValue * CombatInfo.RepeatCount;
            if (CombatInfo.Radius > 1)
            {
                values.SourceDamage = (int)(((uint)(values.SourceDamage * CombatInfo.Radius)) >> 1);
            }

            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(damage, damageTypes, protection, immunity);
            values.SourceCost = statisticsToMaxDamage * CombatInfo.RepeatCount;
            values.TargetDamage = 0;
            values.TargetCost = 0;
            if (values.SourceDamage != 0 && CombatInfo.Radius > 1)
            {
                combat = source.Combat;
                count = 0;
                distance = combat.GetRoundDistance();
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

                for (int I = combat.Sides[(int)target.GetSide()].GetCount() - 1; I >= 0; I--)
                {
                    var side = target.GetSide();
                    other = combat.Sides[(int)side].GetObjects(I);
                    if (target != other && other.GetEnabled() && (int)(((uint)other.Position) >> 4) == (int)(((uint)target.Position) >> 4) && other.FastBattleRoundDistance() <= distance && damageTypes - other.GetImmunityTypes() != Set<DamageType>.Of())
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

                    values.SourceDamage += Round((double)(values.SourceDamage * count) * 0.5);
                }
            }
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
