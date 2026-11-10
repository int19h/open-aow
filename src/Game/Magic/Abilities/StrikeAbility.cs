// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.Magic.Abilities;

// (melee round)
public partial class StrikeAbility : AdjustableAbility
{

    // AoWAb.PAS:1210
    public StrikeAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.AttackItem);
        FAbilityType = AbilityType.Combat;
        FAbilityID = 21;
        FAbilityName = AbilityRules.TranslateRStr(StrikeRStr);
        FMeleeRound = new MeleeRound();
    }

    public MeleeRound FMeleeRound;

    // AoWAb.PAS:1220
    public override void Dispose()
    {
        FMeleeRound?.Dispose();
        base.Dispose();
    }

    // AoWAb.PAS:1349
    public override CombatMode GetCombatMode(AbilityOwner Owner) => CombatMode.Melee;

    // AoWAb.PAS:1226
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of();

    // AoWAb.PAS:1445
    public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
        CombatObject Target;
        FastCombatCommand Command;
        AbilityDamageValues Values = default;
        for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
        {
            Target = Commands.FTargetSide.GetObjects(I);
            if (Target.GetEnabled() && fcValidRoundDistance(Commands.FSource, Target) && CanExecuteMelee(Commands.FSource, Target))
            {
                fcGetDamageValueEx(Commands.FSource, Target, WallSituation.None, ref Values);
                if (Values.SourceDamage != 0)
                {
                    Command = new FastCombatCommand(FAbilityID);
                    Command.FTarget = Target;
                    Command.FSourceDV = Values.SourceDamage;
                    Command.FTargetDV = Values.TargetDamage;
                    Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                    // { Both original branches assign the same move cost. }
                    if (Values.TargetDamage == 0)
                    {
                        Command.FMoveCost = 2;
                    }
                    else
                    {
                        Command.FMoveCost = 2;
                    }

                    Commands.Add(Command);
                }
            }
        }
    }

    // AoWAb.PAS:1399
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        MeleeRound Round;
        int I;
        bool Stop;
        CombatObject Target;
        CombatAction Action;
        MoveWallCA MoveCA;
        MeleeRound.MeleeStrike Strike = default;
        Target = Command.FTarget;
        Round = new MeleeRound();
        Round.Calculate(Source, Target);
        Stop = false;
        I = 0;
        if (Source is FastCombatUnit)
        {
            ((FastCombatUnit)Source).FState = ((FastCombatUnit)Source).FState + Set<FastCombatUnitStateElement>.Of(FastCombatUnitStateElement.Vulnerable);
            if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                MoveCA = new MoveWallCA(null);
                MoveCA.F_MoveWallCA_0C = Source.FID;

                MoveCA.F_MoveWallCA_0D = 0;

                Source.FCombat.ExecuteCombatAction(MoveCA);
                MoveCA.Release();
            }
        }

        while (I < Round.FCount && !Stop)
        {
            Action = Round.CreateStrikeCA(I);
            if (Action != null)
            {
                Source.FCombat.ExecuteCombatAction(Action);
                Round.GetMeleeStrike(I, ref Strike);
                switch (Strike.Side)
                {
                    case CombatSideIndex.Attacker:
                        Stop = !Target.GetEnabled();
                        break;
                    case CombatSideIndex.Defender:
                        Stop = !Source.GetEnabled();
                        break;
                }

                Action.Release();
            }

            I += 1;
        }

        bool Result = Round.FCount != 0;
        Round?.Dispose();
        return Result;
    }

    // AoWAb.PAS:1297
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        if (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1))
        {
            FMeleeRound.CalculateUnit(Attacker, Defender);
            Result = FMeleeRound.GetAttackerDV();
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:1308
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)))
        {
            FMeleeRound.CalculateUnit(Attacker, Defender);
            var attackerDV = FMeleeRound.GetAttackerDV();
            DamageValues.SourceDamage = attackerDV;
            DamageValues.SourceCost = 0;
            var defenderDV = FMeleeRound.GetDefenderDV();
            DamageValues.TargetDamage = defenderDV;
            DamageValues.TargetCost = 0;
            var immunityTypes = Defender.GetImmunityTypes();
            DamageValues.DamageTypes = FMeleeRound.FAttackerDamageTypes - immunityTypes;
        }
        else
        {
            // { Original leaves the cost fields untouched on this path. }
            DamageValues.SourceDamage = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:1274
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        int Attack;
        int Damage;
        int Repeats;
        if (Owner is AbstractUnit unit)
        {
            Repeats = 2;
            Attack = unit.GetAttack();
            Damage = unit.GetDamage();
            if (unit.GetAbilityEnabled(115))
            {
                Repeats += 1;
            }

            Strength = AbilityRules.P3LimitedDamage(Attack, Damage, Repeats);
            Types = GetDamageTypes(Owner);
        }
        else
        {
            Strength = 0;
            Types = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:1328
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)))
        {
            FMeleeRound.Calculate(Attacker, Defender);
            var attackerDV = FMeleeRound.GetAttackerDV();
            DamageValues.SourceDamage = attackerDV;
            DamageValues.SourceCost = 0;
            var defenderDV = FMeleeRound.GetDefenderDV();
            DamageValues.TargetDamage = defenderDV;
            DamageValues.TargetCost = 0;
            var immunityTypes = Defender.GetImmunityTypes();
            DamageValues.DamageTypes = FMeleeRound.FAttackerDamageTypes - immunityTypes;
        }
        else
        {
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:1378
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        if (Owner is AbstractUnit)
        {
            var unit = (AbstractUnit)Owner;
            var attack = unit.GetAttack();
            Info.Attack = attack;
            var damage = unit.GetDamage();
            Info.Damage = damage;

            var damageTypes = GetDamageTypes(Owner);
            Info.DamageTypes = damageTypes;
            Info.RepeatCount = 2;
        }
        else
        {
            Info.Attack = -1;
            Info.Damage = -1;
            Info.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            Info.RepeatCount = 2;
        }
    }

    // AoWAb.PAS:1248
    public bool CanExecuteMelee(CombatObject Source, CombatObject Target)
    {
        bool Result;
        if (Source.GetSide() == CombatSideIndex.Defender)
        {
            if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition))
            {
                Result = Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing);
            }
            else
            {
                Result = true;
            }
        }
        else
        {
            if (Target.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Source.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }
        }

        return Result && (!Target.GetAbilityEnabled(1) || Source.GetAbilityEnabled(1));
    }

    // AoWAb.PAS:1354
    public Set<DamageType> GetDamageTypes(AbilityOwner Owner)
    {
        Set<DamageType> Result;
        if (Owner is AbstractUnit && ((AbstractUnit)Owner).GetAbilityEnabled(21))
        {
            Result = Set<DamageType>.Of(DamageType.Physical);
            var unit = (AbstractUnit)Owner;
            if (unit.GetAbilityEnabled(14) || unit.GetAbilityEnabled(163))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Fire);
            }

            if (unit.GetAbilityEnabled(15))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Cold);
            }

            if (unit.GetAbilityEnabled(16))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Lightning);
            }

            if (unit.GetAbilityEnabled(17))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Poison);
            }

            var unit2 = (AbstractUnit)Owner;
            if (unit2.GetAbilityEnabled(18) || unit2.GetAbilityEnabled(169))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Death);
            }

            if (unit2.GetAbilityEnabled(19))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Holy);
            }

            var unit3 = (AbstractUnit)Owner;
            if (unit3.GetAbilityEnabled(20) || unit3.GetAbilityEnabled(154))
            {
                Result = Result + Set<DamageType>.Of(DamageType.Magic);
            }
        }
        else
        {
            Result = Set<DamageType>.Of();
        }

        return Result;
    }

    public override bool Remove(AbilityOwner owner)
    {
        if (!owner.GetAbEnabled(21)) return false;
        base.Remove(owner);
        for (int i = owner.GetAbCount() - 1; i >= 0; --i)
            if (owner.GetAbEnabled(i) && AoWHSSet.Current.FAbilityControl.GetAbility(i) is StrikeEnhancementAbility)
                while (AoWHSSet.Current.FAbilityControl.GetAbility(i).Remove(owner)) { }
        return true;
    }

    private static readonly string StrikeRStr = "Strike";
}
