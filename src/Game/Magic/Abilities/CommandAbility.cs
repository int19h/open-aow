// Translated from AoWE.pas.
using Aow.Engine;

using Aow.Engine.Nodes;

using Aow.Game.Combat;
using Aow.Game.Combat.Actions;

using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

// (commanded ability ID)
public partial class CommandAbility : TouchAbility
{
    // AoWAb.PAS:5088
    public CommandAbility(ECustomNode owner) : base(owner)
    {
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem);
    }

    public int FCommandedAbilityID;

    // AoWAb.PAS:5190
    public override AbilityType GetAbilityType() => AbilityType.Combat;

    // AoWAb.PAS:5288
    public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
        int Distance;
        CombatObject Target;
        AbilityDamageValues Values = default;
        FastCombatCommand Command;
        Distance = Commands.FCombat.GetRoundDistance();
        if (Commands.FSource.GetMoves() >= 32)
        {
            Distance += 2;
        }

        for (int I = 0, last = Commands.FTargetSide.GetCount() - 1; I <= last; I++)
        {
            Target = Commands.FTargetSide.GetObjects(I);
            if (Target.GetEnabled() && Target.fcRoundDistance() <= Distance && CanTouch(Commands.FSource, Target))
            {
                fcGetDamageValueEx(Commands.FSource, Target, WallSituation.None, ref Values);
                if (Values.SourceDamage != 0)
                {
                    Command = new FastCombatCommand(FAbilityID);
                    Command.FTarget = Commands.FTargetSide.GetObjects(I);
                    Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, Values.TargetDamage, Commands.FSource, Target);
                    Command.FMoveCost = 0;
                    Commands.Add(Command);
                }
            }
        }
    }

    // AoWAb.PAS:5266
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        CommandCA Action;
        StrikeCA Strike;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateCommandCA((CombatUnit)Source, (CombatUnit)Target);
        Source.FCombat.ExecuteCombatAction(Action);
        if (!Action.FSuccessful)
        {
            Strike = CombatObject.CreateStrikeCA(Target, Source, true);
            if (Strike != null)
            {
                Source.FCombat.ExecuteCombatAction(Strike);
                Strike.Release();
            }
        }

        Action.Release();
        return true;
    }

    // AoWAb.PAS:5097
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result)
        {
            owner.RemoveAbilityDataID(FAbilityID);
        }

        return Result;
    }

    // AoWAb.PAS:5194
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        float Probability;
        if (CanTouchUnit(Attacker, Defender))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            Result = Round((double)(Defender.GetHitPoints() * 200) * Probability);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:5213
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (WallSituation == WallSituation.None && (!Defender.GetAbilityEnabled(1) || Attacker.GetAbilityEnabled(1)) && Defender.GetUnitType() != UnitType.Machine)
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)(hitPoints * 200) * Probability);
            var defenderHitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = defenderHitPoints * 2;
            var p4UnitStrikeDV = AbstractUnit.P4UnitStrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)p4UnitStrikeDV * ((double)1 - Probability));
            var damage = Defender.GetDamage();
            DamageValues.TargetCost = damage;
        }
        else
        {
            // { Original leaves DamageTypes unchanged on this path. }
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:5205
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        if (Owner is AbstractUnit unit)
        {
            Strength = Round((double)(GetTouchAttack(GetLevel(unit)) * (unit.GetAttack() * 5)) * 0.1);
            Types = Set<DamageType>.Of(DamageType.None);
        }
    }

    // AoWAb.PAS:5237
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        if (CanTouch(Attacker, Defender) && !(((CombatUnit)Defender).FUnit is Leader))
        {
            Probability = CombatTouchRoleProbability((CombatUnit)Attacker, Defender);
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceDamage = Round((double)(hitPoints * 200) * Probability);
            var defenderHitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = defenderHitPoints * 2;
            var strikeDV = CombatObject.StrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)strikeDV * ((double)1 - Probability));
            var damage = Defender.GetDamage();
            DamageValues.TargetCost = damage;
        }
        else
        {
            // { Original leaves DamageTypes unchanged on this path. }
            DamageValues.SourceDamage = 0;
            DamageValues.SourceCost = 0;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of();
        }
    }

    // AoWAb.PAS:5106
    public override void CombatObjectDestroyed(CombatObject CombatObject)
    {
        ResetCommandedUnits((CombatUnit)CombatObject);
    }

    // AoWAb.PAS:5102
    public override void CombatDone(CombatObject CombatObject)
    {
        CombatObject.GetAbilityOwner().RemoveAbilityDataID(FAbilityID);
    }

    // AoWAb.PAS:5176
    public override bool CanTouch(CombatObject Source, CombatObject Target) => base.CanTouch(Source, Target) && (Source is CombatUnit && Target is CombatUnit && ((CombatUnit)Target).FUnit.GetUnitType() != UnitType.Machine && !Target.GetAbilityEnabled(107));

    // AoWAb.PAS:5184
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result;
        if (base.CanTouchUnit(Source, Target))
        {
            Result = Target.GetUnitType() != UnitType.Machine;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:5110
    public virtual void Command(CombatUnit Source, CombatUnit Target)
    {
        CommandedAbilityData TargetData;
        CommandAbilityData SourceData;
        // { The returned class is consumed by its constructor. }
        Uncommand(Target);
        // { Inline the returned class receiver. }
        TargetData = (CommandedAbilityData)AoWHSSet.Current.FAbilityControl.GetAbility(FCommandedAbilityID).AbilityDataClass().Create(null);
        TargetData.FAbilityID = FCommandedAbilityID;
        TargetData.FCommandAbilityID = FAbilityID;
        TargetData.FSourceID = Source.FID;
        TargetData.FOriginalCombatPlayer = (byte)Target.FCombatPlayer;
        SetAb(Target.FUnit, FCommandedAbilityID, 1);
        Target.FUnit.AddAbilityData(TargetData);
        Target.SetCombatPlayer((byte)Source.FCombatPlayer);
        SourceData = (CommandAbilityData)Source.FUnit.GetAbilityData(FAbilityID);
        if (SourceData == null)
        {
            SourceData = new CommandAbilityData(null);
            SourceData.FAbilityID = FAbilityID;
            Source.FUnit.AddAbilityData(SourceData);
        }

        SourceData.FCommandedUnits.Add(Target.FID);
    }

    // AoWAb.PAS:5259
    public virtual CommandCA CreateCommandCA(CombatUnit Source, CombatUnit Target)
    {
        CommandCA Result = new CommandCA(null);
        bool condition = !(Target.FUnit is Leader) && CombatTouchRole(Source, Target);
        Result.Initialize(Source.FID, Target.FID, condition);
        Result.FAbilityID = FAbilityID;
        return Result;
    }

    // AoWAb.PAS:5157
    public virtual void ResetCommandedUnits(CombatUnit Source)
    {
        AbstractUnit UnitOwner;
        CommandAbilityData Data;
        CombatUnit Target;
        UnitOwner = Source.FUnit;
        if (UnitOwner.GetAbilityEnabled(FAbilityID))
        {
            Data = (CommandAbilityData)Source.FUnit.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                for (int I = Data.FCommandedUnits.Count - 1; I >= 0; I--)
                {
                    Target = (CombatUnit)Source.FCombat.FCombatData.FindID(Data.FCommandedUnits.List[I]);
                    if (Target != null)
                    {
                        Uncommand(Target);
                    }
                }
            }
        }
    }

    // AoWAb.PAS:5135
    public virtual void Uncommand(CombatUnit Target)
    {
        AbstractUnit UnitOwner;
        CommandedAbilityData Data;
        CombatUnit Source;
        CommandAbilityData SourceData;
        UnitOwner = Target.FUnit;
        if (UnitOwner.GetAbilityEnabled(FCommandedAbilityID))
        {
            Data = (CommandedAbilityData)UnitOwner.GetAbilityData(FCommandedAbilityID);
            // { Original reads Data before checking it for nil. }
            Source = (CombatUnit)Target.FCombat.FCombatData.FindID(Data.FSourceID);
            if (Data != null)
            {
                Target.SetCombatPlayer(Data.FOriginalCombatPlayer);
            }

            AoWHSSet.Current.FAbilityControl.GetAbility(FCommandedAbilityID).Remove(Target.FUnit);
            if (Source != null)
            {
                SourceData = (CommandAbilityData)Source.FUnit.GetAbilityData(FAbilityID);
                if (SourceData != null)
                {
                    SourceData.FCommandedUnits.Remove(Target.FID);
                }
            }
        }
    }

    // AoWAb.PAS:5093
    public override EClass AbilityDataClass() => P4CommandDataClass;

    internal static EClass P4CommandDataClass = EClass.Of<CommandAbilityData>();

}
