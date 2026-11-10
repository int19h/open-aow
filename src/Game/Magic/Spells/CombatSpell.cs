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
    public SpellCombatInfo FCombatInfo;
    public byte FDamageMode;

    // CombatSpells.pas:632
    public CombatSpell(ECustomNode owner) : base(owner)
    {
        FSpellType = SpellType.Combat;
        FSpellControlTypes = Set<SpellControlTypeElement>.Of(SpellControlTypeElement.Combat);
        FCombatInfo.Radius = 1;
        FCombatInfo.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
        FCastDuration = 5;
        FDamageMode = 0;
    }

    // CombatSpells.pas:655
    public override CombatAction CreateCA(CombatObject Source, CombatObject Target, bool ChargeMana)
    {
        CombatSpellCA Action;
        Action = new CombatSpellCA(null);
        Action.FSpellID = FSpellID;
        Action.GenerateEx(Source, Target, FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, FDamageMode);
        if (ChargeMana)
        {
            Action.FManaCost = ((Hero)((CombatUnit)Source).FUnit).CastingMana(this);
        }

        return Action;
    }

    // CombatSpells.pas:643
    public override void GetCombatInfo(AbstractUnit Source, ref SpellCombatInfo Info)
    {
        Info = FCombatInfo;
    }

    // CombatSpells.pas:648
    public override int GetCombatDamageValue(CombatObject Source, CombatObject Target)
    {
        var hitPoints = Target.GetHitPoints();
        var defense = Target.GetDefense();
        var resistance = Target.GetResistance();
        var immunityTypes = Target.GetImmunityTypes();
        var protectionTypes = Target.GetProtectionTypes();
        return AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, immunityTypes, protectionTypes) * FCombatInfo.RepeatCount;
    }

    // CombatSpells.pas:668
    public override void GetCombatDamageValueEx(CombatObject Source, CombatObject Target, ref AbilityDamageValues Values)
    {
        Set<DamageType> Protection;
        Set<DamageType> Immunity;
        Protection = Target.GetProtectionTypes();
        Immunity = Target.GetImmunityTypes();
        Values.DamageTypes = FCombatInfo.DamageTypes;
        var hitPoints = Target.GetHitPoints();
        var defense = Target.GetDefense();
        var resistance = Target.GetResistance();
        var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, hitPoints, defense, resistance, Immunity, Protection);
        Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
        // { Original recomputes the value, using resistance in place of defense for mode 1. }
        if (FDamageMode == 1)
        {
            var targetHitPoints = Target.GetHitPoints();
            var targetResistance = Target.GetResistance();
            var resistance2 = Target.GetResistance();
            var statisticsToLimitedDV2 = AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, targetHitPoints, targetResistance, resistance2, Immunity, Protection);
            Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
        }
        else
        {
            var a2HitPoints = Target.GetHitPoints();
            var targetDefense = Target.GetDefense();
            var a2Resistance = Target.GetResistance();
            var statisticsToLimitedDV2 = AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, FCombatInfo.Damage, FCombatInfo.DamageTypes, a2HitPoints, targetDefense, a2Resistance, Immunity, Protection);
            Values.SourceDamage = statisticsToLimitedDV2 * FCombatInfo.RepeatCount;
        }

        var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(FCombatInfo.Damage, FCombatInfo.DamageTypes, Protection, Immunity);
        Values.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
        Values.TargetDamage = 0;
        Values.TargetCost = 0;
    }

    // CombatSpells.pas:754
    public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
        AbstractUnit Hero;
        int Distance;
        string Reason = "";
        CombatObject Target;
        SpellCC Command;
        AbilityDamageValues Values = default;
        Hero = ((CombatUnit)Commands.FSource).FUnit;
        if (Hero is Hero && ((Hero)Hero).CanCastSpellInstantly(FSpellID, FBaseManaCost, ref Reason))
        {
            Distance = Commands.FCombat.GetRoundDistance();
            if (FCombatInfo.Range == -1)
            {
                Distance += 8;
            }
            else
            {
                if (FCombatInfo.Range >= 12)
                {
                    Distance += 4;
                }
                else
                {
                    if (FCombatInfo.Range >= 8)
                    {
                        Distance += 2;
                    }
                }
            }

            for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
            {
                Target = Commands.FTargetSide.GetObjects(I);
                if (Target.GetEnabled() && Target.fcRoundDistance() <= Distance)
                {
                    var wallSituation = Commands.FSource.FCombat.GetWallSituation(Commands.FSource, Target);
                    fcGetDamageValueEx(Commands.FSource, Target, wallSituation, ref Values);
                    if (Values.SourceDamage != 0)
                    {
                        Command = new SpellCC(52);
                        Command.FTarget = Commands.FTargetSide.GetObjects(I);
                        Command.FMoveCost = 1;
                        Command.FSpellID = FSpellID;
                        Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                        Commands.Add(Command);
                    }
                }
            }
        }
    }

    // CombatSpells.pas:791
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        int Count;
        CombatAction Action;
        CombatObject Other;
        int Distance;
        CombatObject Target;
        DelphiList Targets;
        Combat.Battle Combat;
        Target = Command.FTarget;
        for (int I = 0, last = FCombatInfo.RepeatCount - 1; I <= last; I++)
        {
            if (!Target.FStatus.Contains(CombatObjectStatusElement.Destroyed))
            {
                Action = CreateCA(Source, Target, I == 0);
                Source.FCombat.ExecuteCombatAction(Action);
                Action.Release();
            }
        }

        if (FCombatInfo.Radius > 1)
        {
            Combat = Source.FCombat;
            Targets = new DelphiList();
            Distance = Combat.GetRoundDistance();
            if (FCombatInfo.Range == -1)
            {
                Distance += 8;
            }
            else
            {
                if (FCombatInfo.Range >= 12)
                {
                    Distance += 4;
                }
                else
                {
                    if (FCombatInfo.Range >= 8)
                    {
                        Distance += 2;
                    }
                }
            }

            for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                var side = Target.GetSide();
                Other = Combat.FSides[(int)side].GetObjects(I);
                bool condition = Other != Target && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Distance >= Other.fcRoundDistance();
                if (condition)
                {
                    var immunityTypes = Other.GetImmunityTypes();
                    condition = FCombatInfo.DamageTypes - immunityTypes != Set<DamageType>.Of();
                }

                if (condition)
                {
                    Targets.Add(Other);
                }
            }

            // { Original shifts Random(4) by two, so this adds no random extra targets. }
            Count = (int)(((uint)AoWHSMap.Current.Random(4)) >> 2) + FCombatInfo.Radius;
            while (Count != 0 && Targets.Count != 0)
            {
                Other = (CombatObject)Targets[AoWHSMap.Current.Random(Targets.Count)];
                for (int J = 0, last = FCombatInfo.RepeatCount - 1; J <= last; J++)
                {
                    if (!Other.FStatus.Contains(CombatObjectStatusElement.Destroyed))
                    {
                        Action = CreateCA(Source, Other, false);
                        Source.FCombat.ExecuteCombatAction(Action);
                        Action.Release();
                    }
                }

                Targets.Remove(Other);
                Count -= 1;
            }

            Targets?.Dispose();
        }

        return true;
    }

    // CombatSpells.pas:693
    public override void fcGetDamageValueEx(CombatObject Source, CombatObject Target, WallSituation WallSituation, ref AbilityDamageValues Values)
    {
        Set<DamageType> DamageTypes;
        Set<DamageType> Protection;
        Set<DamageType> Immunity;
        int Count;
        int Distance;
        CombatObject Other;
        Combat.Battle Combat;
        int Damage;
        int Defense;
        if (WallSituation == WallSituation.None || FCombatInfo.Range == -1 || FCombatInfo.Range >= 8)
        {
            Protection = Target.GetProtectionTypes();
            Immunity = Target.GetImmunityTypes();
            DamageTypes = FCombatInfo.DamageTypes;
            Damage = FCombatInfo.Damage;
            Defense = Target.GetDefense();
            if (WallSituation == WallSituation.Target && (Set<DamageType>.Of(DamageType.Magic, DamageType.Death, DamageType.Holy) & FCombatInfo.DamageTypes) == Set<DamageType>.Of())
            {
                Defense += 2;
            }

            Values.DamageTypes = DamageTypes;
            var hitPoints = Target.GetHitPoints();
            var resistance = Target.GetResistance();
            var statisticsToLimitedDV = AbilityRules.StatisticsToLimitedDV(FCombatInfo.Attack, Damage, DamageTypes, hitPoints, Defense, resistance, Immunity, Protection);
            Values.SourceDamage = statisticsToLimitedDV * FCombatInfo.RepeatCount;
            if (FCombatInfo.Radius > 1)
            {
                Values.SourceDamage = (int)(((uint)(Values.SourceDamage * FCombatInfo.Radius)) >> 1);
            }

            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Damage, DamageTypes, Protection, Immunity);
            Values.SourceCost = statisticsToMaxDamage * FCombatInfo.RepeatCount;
            Values.TargetDamage = 0;
            Values.TargetCost = 0;
            if (Values.SourceDamage != 0 && FCombatInfo.Radius > 1)
            {
                Combat = Source.FCombat;
                Count = 0;
                Distance = Combat.GetRoundDistance();
                if (FCombatInfo.Range == -1)
                {
                    Distance += 8;
                }
                else
                {
                    if (FCombatInfo.Range >= 12)
                    {
                        Distance += 4;
                    }
                    else
                    {
                        if (FCombatInfo.Range >= 8)
                        {
                            Distance += 2;
                        }
                    }
                }

                for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
                {
                    var side = Target.GetSide();
                    Other = Combat.FSides[(int)side].GetObjects(I);
                    if (Target != Other && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && Other.fcRoundDistance() <= Distance && DamageTypes - Other.GetImmunityTypes() != Set<DamageType>.Of())
                    {
                        Count += 1;
                    }
                }

                if (Count != 0)
                {
                    if (FCombatInfo.Radius < Count)
                    {
                        Count = FCombatInfo.Radius;
                    }

                    Values.SourceDamage += Round((double)(Values.SourceDamage * Count) * 0.5);
                }
            }
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
}
