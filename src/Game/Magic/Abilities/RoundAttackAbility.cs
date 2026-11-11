// Translated from AoWE.pas.
using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

public partial class RoundAttackAbility : TouchAbility
{
    // AoWAb.PAS:2262
    public RoundAttackAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 114;
        FAbilityType = AbilityType.Combat;
        FAbilityName = AbilityRules.TranslateRStr(RoundAttackRStr);
    }

    // AoWAb.PAS:2404
    public override bool fcExecuteCombatCommand(CombatObject Source, FastBattleCommand Command)
    {
        Set<DamageType> DamageTypes;
        int Count;
        StrikeCombatAction Action;
        Combat.Battle Combat;
        DelphiList Targets;
        CombatObject Target;
        CombatObject Other;
        MoveWallCombatAction MoveAction;
        Combat = Source.FCombat;
        Targets = new DelphiList();
        DamageTypes = CombatObject.MeleeDamageTypes(Source);
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        if (Source is FastBattleUnit)
        {
            ((FastBattleUnit)Source).FRoundDistance = 0;
            if (Source.fcBehindWall() && Source.fcWallInBetween(Target.FPosition) && !Target.WallCombatFeatures().Contains(WallCombatFeaturesElement.WallPassing))
            {
                MoveAction = new MoveWallCombatAction(null);
                MoveAction.F_MoveWallCA_0C = Source.FID;
                MoveAction.F_MoveWallCA_0D = 0;
                Source.FCombat.ExecuteCombatAction(MoveAction);
                MoveAction.Release();
            }
        }

        Action = CombatObject.CreateStrikeCA(Source, Target, false);
        if (Action != null)
        {
            Combat.ExecuteCombatAction(Action);
            Action.Release();
        }

        for (int I = Combat.FSides[(int)Target.GetSide()].GetCount() - 1; I >= 0; I--)
        {
            var side = Target.GetSide();
            Other = Combat.FSides[(int)side].GetObjects(I);
            if (Other != Target && Other.GetEnabled() && (int)(((uint)Other.FPosition) >> 4) == (int)(((uint)Target.FPosition) >> 4) && (!Other.GetAbilityEnabled(1) || Source.GetAbilityEnabled(1)) && !Source.fcWallInBetween(Other.FPosition) && Other.fcRoundDistance() <= Combat.GetRound() - 1 && DamageTypes - Other.GetImmunityTypes() != Set<DamageType>.Of())
            {
                Targets.Add(Other);
            }
        }

        Count = AoWHSMap.Current.Random(2) + 1;
        while (Count != 0 && Targets.Count != 0)
        {
            Other = (CombatObject)Targets[AoWHSMap.Current.Random(Targets.Count)];
            Action = CombatObject.CreateStrikeCA(Source, Other, false);
            if (Action != null)
            {
                Combat.ExecuteCombatAction(Action);
                Action.Release();
            }

            Targets.Remove(Other);
            Count -= 1;
        }

        Targets?.Dispose();
        return true;
    }

    // AoWAb.PAS:2286
    public override int GetDamageValue(Troop Attacker, Troop Defender)
    {
        int Result;
        if (CanTouchUnit(Attacker, Defender))
        {
            Result = Troop.P4UnitStrikeDV(Attacker, Defender) * 2;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:2318
    public override void GetDamageValueEx(Troop Attacker, Troop Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && CanTouchUnit(Attacker, Defender))
        {
            var p4UnitStrikeDV = Troop.P4UnitStrikeDV(Attacker, Defender);
            DamageValues.SourceDamage = p4UnitStrikeDV * 2;
            DamageValues.SourceCost = 0;
            var aoWEP4UnitStrikeDV = Troop.P4UnitStrikeDV(Attacker, Defender);
            DamageValues.TargetDamage = aoWEP4UnitStrikeDV;
            DamageValues.TargetCost = 0;
            var damageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Attacker);
            var immunityTypes = Defender.GetImmunityTypes();
            DamageValues.DamageTypes = damageTypes - immunityTypes;
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

    // AoWAb.PAS:2270
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        int Attack;
        int Damage;
        if (Owner is Troop unit)
        {
            Attack = unit.GetAttack();
            Damage = unit.GetDamage();

            Strength = AbilityRules.P3LimitedDamage(Attack, Damage, 1) * 3;
            Types = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Owner);
        }
    }

    // AoWAb.PAS:2338
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        int Count;
        Set<DamageType> DamageTypes;
        Combat.Battle Combat;
        CombatObject Target;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            DamageTypes = CombatObject.MeleeDamageTypes(Attacker);
            var strikeDV = CombatObject.StrikeDV(Attacker, Defender);
            DamageValues.SourceDamage = strikeDV;
            var damage = Attacker.GetDamage();
            DamageValues.SourceCost = damage;
            var aoWEStrikeDV = CombatObject.StrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = aoWEStrikeDV;
            var defenderDamage = Defender.GetDamage();
            DamageValues.TargetCost = defenderDamage;
            var immunityTypes = Defender.GetImmunityTypes();
            DamageValues.DamageTypes = DamageTypes - immunityTypes;
            Combat = Attacker.FCombat;
            Count = 0;
            for (int I = Combat.FSides[(int)Defender.GetSide()].GetCount() - 1; I >= 0; I--)
            {
                // { Preserve the original saved registers. }
                var side = Defender.GetSide();
                Target = Combat.FSides[(int)side].GetObjects(I);
                // { Original tests Defender's flying ability here, rather than Target's. }
                if (Target != Defender && Target.GetEnabled() && (int)(((uint)Target.FPosition) >> 4) == (int)(((uint)Defender.FPosition) >> 4) && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && !Attacker.fcWallInBetween(Target.FPosition) && Target.fcRoundDistance() <= Combat.GetRoundDistance() && DamageTypes - Target.GetImmunityTypes() != Set<DamageType>.Of())
                {
                    Count += 1;
                }
            }

            if (Count != 0)
            {
                if (Count > 3)
                {
                    Count = 3;
                }

                DamageValues.SourceDamage += Round((double)(DamageValues.SourceDamage * Count) / 3);
            }
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

    // AoWAb.PAS:2383
    public override void tcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, ref AbilityDamageValues DamageValues)
    {
        if (CanTouch(Attacker, Defender))
        {
            var strikeDV = CombatObject.StrikeDV(Attacker, Defender);
            DamageValues.SourceDamage = strikeDV * 2;
            var damage = Attacker.GetDamage();
            DamageValues.SourceCost = damage * 2;
            // { Original repeats the forward strike instead of reversing the arguments. }
            var aoWEStrikeDV = CombatObject.StrikeDV(Attacker, Defender);
            DamageValues.TargetDamage = aoWEStrikeDV;
            var defenderDamage = Defender.GetDamage();
            DamageValues.TargetCost = defenderDamage;
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(Attacker);
            var immunityTypes = Defender.GetImmunityTypes();
            DamageValues.DamageTypes = meleeDamageTypes - immunityTypes;
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

    // AoWAb.PAS:2294
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        Troop Source;
        if (Owner is Troop)
        {
            Source = (Troop)Owner;
            var attack = Source.GetAttack();
            Info.Attack = attack;
            var damage = Source.GetDamage();
            Info.Damage = damage;
            var damageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Source);
            Info.DamageTypes = damageTypes;
            Info.RepeatCount = 2;
            Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
            return;
        }
        else
        {
            Info.Attack = -1;
            Info.Damage = -1;
            Info.DamageTypes = Set<DamageType>.Of(DamageType.Physical);
            Info.RepeatCount = 2;
            Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
        }
    }

    private static readonly string RoundAttackRStr = "Round Attack";
}
