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

public partial class TurnUndeadAbility : TouchAbility
{
    // AoWAb.PAS:3121
    public TurnUndeadAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 38;
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.HeadItem, AbilitySelectionType.TorsoItem, AbilitySelectionType.DefenseItem, AbilitySelectionType.RingItem, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        FAbilityName = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr);
        FAbilityType = AbilityType.Combat;
    }

    // AoWAb.PAS:3191
    public override string GetName(AbilityOwner Owner) => GetLevelName(GetLevel(Owner));

    // AoWAb.PAS:3146
    public override int GetLevel(AbilityOwner Owner)
    {
        int Result;
        MultiLevelAbilityData Data;
        if (Owner != null)
        {
            Data = (MultiLevelAbilityData)Owner.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                Result = Data.FLevel;
            }
            else
            {
                return 0;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:3213
    public override int GetSkillPoints(AbilityOwner owner)
    {
        int Result = GetLevel(owner);
        return Result * 5;
    }

    // AoWAb.PAS:3374
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        TurnUndeadCA Action;
        StrikeCA Strike;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateTurnUndeadCA(Source, Target);
        Source.FCombat.ExecuteCombatAction(Action);
        if (Target.GetEnabled())
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

    // AoWAb.PAS:3204
    public override bool CanExpand(AbilityOwner owner)
    {
        if ((owner.GetAbilitySelectionTypes() & FSelectionTypes) == Set<AbilitySelectionType>.Of() || GetLevel(owner) >= 4)
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:3199
    public override int ExpandCost(AbilityOwner owner)
    {
        int Result = GetLevel(owner);
        if (Result < 4)
        {
            Result = 5;
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:3195
    public override string ExpandName(AbilityOwner Owner) => GetLevelName(GetLevel(Owner) + 1);

    // AoWAb.PAS:3217
    public override bool Expand(AbilityOwner Owner)
    {
        MultiLevelAbilityData Data;
        bool Result = false;
        Data = (MultiLevelAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new MultiLevelAbilityData(null);
            Data.FLevelAbilityID = (ushort)FAbilityID;
            Data.FLevel = 1;
            SetAb(Owner, FAbilityID, 1);
            Owner.AddAbilityData(Data);
            Result = true;
        }
        else
        {
            if (Data.FLevel != 4)
            {
                Result = true;
                Data.FLevel = (byte)(Data.FLevel + 1);
            }
        }

        return Result;
    }

    // AoWAb.PAS:3238
    public override bool Remove(AbilityOwner owner)
    {
        MultiLevelAbilityData Data;
        bool Result = true;
        Data = (MultiLevelAbilityData)owner.GetAbilityData(FAbilityID);
        if (Data != null)
        {
            if (Data.FLevel == 1)
            {
                SetAb(owner, FAbilityID, 0);
                owner.RemoveAbilityData(Data);
            }
            else
            {
                Data.FLevel = (byte)(Data.FLevel - 1);
            }
        }
        else
        {
            SetAb(owner, FAbilityID, 0);
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:3293
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        float Probability;
        if (CanTouchUnit(Attacker, Defender) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            Result = Round((double)((double)Defender.GetHitPoints() * Probability) * 100);
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // AoWAb.PAS:3305
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        int HitPoints;
        if (WallSituation == WallSituation.None && CanTouchUnit(Attacker, Defender) && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Probability = TouchRoleProbability(Attacker, Defender);
            HitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = HitPoints;
            DamageValues.SourceDamage = Round((double)((double)HitPoints * Probability) * 100);
            var p4UnitStrikeDV = AbstractUnit.P4UnitStrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)p4UnitStrikeDV * ((double)1 - Probability));
            var protectionTypes = Attacker.GetProtectionTypes();
            var damageTypes = ((StrikeAbility)AoWHSSet.Current.FAbilityControl.GetAbility(21)).GetDamageTypes(Defender);
            var immunityTypes = Attacker.GetImmunityTypes();
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Defender.GetDamage(), damageTypes, immunityTypes, protectionTypes);
            DamageValues.TargetCost = statisticsToMaxDamage;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
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

    // AoWAb.PAS:3168
    public override void GetOffensiveStrength(AbilityOwner Owner, ref int Strength, ref Set<DamageType> Types)
    {
        int Damage;
        if (Owner is AbstractUnit unit)
        {
            Damage = GetTurnUndeadDamage(GetLevel(unit));
            if (Damage > 5)
            {
                Damage = 5;
            }

            Strength = Round((double)(unit.GetAttack() * 4 * Damage) * 0.1);
            // { Original reports death damage here, although the attack deals holy damage. }
            Types = Set<DamageType>.Of(DamageType.Death);
        }
    }

    // AoWAb.PAS:3331
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        float Probability;
        int HitPoints;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            Probability = CombatTouchRoleProbability((CombatUnit)Attacker, Defender);
            HitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = HitPoints;
            DamageValues.SourceDamage = Round((double)((double)HitPoints * Probability) * 100);
            var strikeDV = CombatObject.StrikeDV(Defender, Attacker);
            DamageValues.TargetDamage = Round((double)strikeDV * ((double)1 - Probability));
            var protectionTypes = Attacker.GetProtectionTypes();
            var immunityTypes = Attacker.GetImmunityTypes();
            var meleeDamageTypes = CombatObject.MeleeDamageTypes(Defender);
            var statisticsToMaxDamage = AbilityRules.StatisticsToMaxDamage(Defender.GetDamage(), meleeDamageTypes, immunityTypes, protectionTypes);
            DamageValues.TargetCost = statisticsToMaxDamage;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
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

    // AoWAb.PAS:3282
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        int Level;
        Level = GetLevel(Owner);
        var touchAttack = GetTouchAttack(Level);
        Info.Attack = touchAttack;
        var turnUndeadDamage = GetTurnUndeadDamage(Level);
        Info.Damage = turnUndeadDamage;
        Info.DamageTypes = Set<DamageType>.Of(DamageType.Holy);
        Info.RepeatCount = 1;
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:3260
    public override sbyte GetTouchAttack(int Level)
    {
        sbyte Result;
        switch (Level)
        {
            case 1:
                Result = 4;
                break;
            case 2:
                Result = 5;
                break;
            case 3:
                Result = 6;
                break;
            case 4:
                Result = 7;
                break;
            default:
                Result = 4;
                break;
        }

        return Result;
    }

    // AoWAb.PAS:3137
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        if (Result)
        {
            bool condition = Source is CombatUnit && Target is CombatUnit;
            if (condition)
            {
                var unitType = ((CombatUnit)Target).FUnit.GetUnitType();
                condition = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType);
            }

            Result = condition && ((ushort)((CombatUnit)Target).FUnit.GetRace() == 11 || ((CombatUnit)Target).FUnit.GetAbilityEnabled(129));
        }

        return Result;
    }

    // AoWAb.PAS:3130
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.CanTouchUnit(Source, Target);
        if (Result)
        {
            var unitType = Target.GetUnitType();
            Result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && ((ushort)Target.GetRace() == 11 || Target.GetAbilityEnabled(129));
        }

        return Result;
    }

    // AoWAb.PAS:3181
    public string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr);
                break;
            case 1:
                Result = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr) + " I";
                break;
            case 2:
                Result = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr) + " II";
                break;
            case 3:
                Result = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr) + " III";
                break;
            case 4:
                Result = AbilityRules.TranslateRStr(GameText.TurnUndeadRStr) + " IV";
                break;
        }

        return Result;
    }

    // AoWAb.PAS:3271
    public sbyte GetTurnUndeadDamage(int Level)
    {
        sbyte Result;
        switch (Level)
        {
            case 1:
                Result = 4;
                break;
            case 2:
                Result = 5;
                break;
            case 3:
                Result = 6;
                break;
            case 4:
                Result = 7;
                break;
            default:
                Result = 0;
                break;
        }

        return Result;
    }

    // AoWAb.PAS:3356
    public TurnUndeadCA CreateTurnUndeadCA(CombatObject Source, CombatObject Target)
    {
        TurnUndeadCA Action;
        int Level;
        // { These eliminated copies preserve the original saved registers. }
        Action = new TurnUndeadCA(null);
        if (HitChance.HitRole(Source.GetAttack() - Target.GetDefense()))
        {
            Level = Source.GetAbilityLevel(FAbilityID);
            var turnUndeadDamage = GetTurnUndeadDamage(Level);
            Action.GenerateEx(Source, Target, GetTouchAttack(Level), turnUndeadDamage, Set<DamageType>.Of(DamageType.Holy), 1);
        }
        else
        {
            Action.FSourceID = Source.FID;
            Action.FTargetID = Target.FID;
        }

        return Action;
    }

    // AoWAb.PAS:3164
    public override EClass AbilityDataClass() => AbilityRules.P4MultiLevelDataClass;

}
