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

public partial class HealingAbility : TouchAbility
{
    // AoWAb.PAS:3616
    public HealingAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 47;
        FAbilityName = AbilityRules.TranslateRStr(HealingRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        FAbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:3663
    public override string GetName(AbilityOwner Owner)
    {
        string Result;
        if (Owner == null || GetEnabled(Owner))
        {
            Result = FAbilityName;
        }
        else
        {
            var used = AbilityRules.TranslateRStr(GameText.UsedRStr);
            Result = FAbilityName + " (" + LowerCase(used) + ")";
        }

        return Result;
    }

    // AoWAb.PAS:3640
    public override bool GetEnabled(AbilityOwner owner)
    {
        HealingAbilityData Data;
        bool Result = owner.GetAbSet(FAbilityID);
        if (Result)
        {
            Data = (HealingAbilityData)owner.GetAbilityData(FAbilityID);
            if (Data != null)
            {
                Result = Data.FEnabled;
            }
            else
            {
                Result = true;
            }
        }

        return Result;
    }

    // AoWAb.PAS:3624
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:3801
    public override void fcPrefetchCombatCommands(FastCombatCommandList Commands)
    {
        CombatObject Target;
        FastCombatCommand Command;
        AbilityDamageValues Values = default;
        for (int I = 0, last = Commands.FSourceSide.GetCount() - 1; I <= last; I++)
        {
            Target = Commands.FSourceSide.GetObjects(I);
            if (Target.GetEnabled() && CanTouch(Commands.FSource, Target))
            {
                fcGetDamageValueEx(Commands.FSource, Target, WallSituation.None, ref Values);
                if (Values.SourceDamage != 0)
                {
                    Command = new FastCombatCommand(FAbilityID);
                    Command.FTarget = Target;
                    Command.FSourceDV = Values.SourceDamage;
                    Command.FTargetDV = 0;
                    Command.FExpectedDV = AbilityRules.DVtoDEV(Values.SourceDamage, 0, Commands.FSource, Target);
                    Command.FMoveCost = 0;
                    Commands.Add(Command);
                }
            }
        }
    }

    // AoWAb.PAS:3789
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        HealingCA Action;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = CreateHealingCA(Source, Target);
        Source.FCombat.ExecuteCombatAction(Action);
        Action.Release();
        return true;
    }

    // AoWAb.PAS:3654
    public override void NewTurn(AbilityOwner Owner, sbyte Player)
    {
        if (Owner is AbstractUnit && ((AbstractUnit)Owner).FPlayer == Player)
        {
            SetEnabled(Owner, true);
        }
    }

    // AoWAb.PAS:3827
    public override bool Activate(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        UnitSelectorAAI Info;
        HealingUnitSelector Selector;
        if (CanActivate(Owner, ref Reason))
        {
            var unit = Owner as AbstractUnit;
            Info = new UnitSelectorAAI(null);
            Info.FAbilityID = FAbilityID;
            if (unit != null)
            {
                Info.FUnitID = unit.FUnitID;
            }
            else
            {
                Info.FUnitID = 0;
            }

            Selector = new HealingUnitSelector();
            var healing = AbilityRules.TranslateRStr(HealingRStr);
            Selector.FCaption = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.SelectUnitForXRStr), [healing]);
            if (unit != null)
            {
                if (unit.Owner is HeroList)
                {
                    var empty = new UnitUserList(null);
                    Selector.Setup(unit, empty);
                    empty.Dispose();
                }
                else
                {
                    Selector.Setup(unit, unit.UnitListOwner);
                }
            }

            Info.SetUnitSelector(Selector);
            // { Original omits Selector.Release here. }
            AoWHSMap.Current.FOnActivateAbilityStore.Raise(Info);
            Info.Release();
            Result = true;
        }
        else
        {
            Result = false;
        }

        return Result;
    }

    // AoWAb.PAS:3670
    public override bool Remove(AbilityOwner owner)
    {
        bool Result = base.Remove(owner);
        if (Result)
        {
            owner.RemoveAbilityDataID(FAbilityID);
        }

        return Result;
    }

    // AoWAb.PAS:3707
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        if (!Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            Result = Defender.GetHits() - Defender.GetHitPoints();
            if (Result > 5)
            {
                Result = 5;
            }

            return Result * 100;
        }

        return 0;
    }

    // AoWAb.PAS:3718
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && !Defender.GetImmunityTypes().Contains(DamageType.None))
        {
            var hits = Defender.GetHits();
            var hitPoints = Defender.GetHitPoints();
            DamageValues.SourceCost = hits - hitPoints;
            if (DamageValues.SourceCost > 5)
            {
                DamageValues.SourceCost = 5;
            }

            DamageValues.SourceDamage = DamageValues.SourceCost * 100;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.None);
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

    // AoWAb.PAS:3738
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        int HitPoints;
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            HitPoints = GetHealHitPoints(Defender);
            DamageValues.SourceCost = HitPoints;
            DamageValues.SourceDamage = HitPoints * 100;
            DamageValues.TargetDamage = 0;
            DamageValues.TargetCost = 0;
            DamageValues.DamageTypes = Set<DamageType>.Of(DamageType.None);
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

    // AoWAb.PAS:3702
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        base.GetCombatInfo(Owner, ref Info);
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend);
    }

    // AoWAb.PAS:3693
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

            Result = condition && (Target.GetHits() != Target.GetHitPoints() || Target.GetAbilityEnabled(96) || Target.GetAbilityEnabled(53) || Target.GetAbilityEnabled(108));
        }

        return Result;
    }

    // AoWAb.PAS:3685
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target)
    {
        bool Result = base.CanTouchUnit(Source, Target);
        if (Result)
        {
            var unitType = Target.GetUnitType();
            Result = Set<UnitType>.Of(UnitType.Humanoid, UnitType.Creature).Contains(unitType) && (Target.GetHits() != Target.GetHitPoints() || Target.GetAbilityEnabled(96) || Target.GetAbilityEnabled(53) || Target.GetAbilityEnabled(108));
        }

        return Result;
    }

    // AoWAb.PAS:3628
    public void SetEnabled(AbilityOwner Owner, bool Enabled)
    {
        HealingAbilityData Data;
        Data = (HealingAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new HealingAbilityData(null);
            Owner.AddAbilityData(Data);
        }

        Data.FEnabled = Enabled;
    }

    // AoWAb.PAS:3675
    public int GetHealHitPoints(CombatObject Target)
    {
        int Result = Target.GetHits() - Target.GetHitPoints();
        if (Result > 5)
        {
            Result = 5;
        }

        return Result;
    }

    // AoWAb.PAS:3680
    public HealingCA CreateHealingCA(CombatObject Source, CombatObject Target)
    {
        HealingCA Result = new HealingCA(null);
        Result.Initialize(Source.FID, Target.FID);
        return Result;
    }

    // AoWAb.PAS:3760
    public bool HealUnit(AbstractUnit Unit)
    {
        int HitPoints;
        bool Result = false;
        if (Unit.GetHits() != Unit.GetHitPoints())
        {
            HitPoints = Unit.GetHits() - Unit.GetHitPoints();
            if (HitPoints > 5)
            {
                HitPoints = 5;
            }

            Unit.SetHitPoints((sbyte)(Unit.GetHitPoints() + (sbyte)HitPoints));
            Result = true;
        }

        if (Unit.GetAbilityEnabled(96))
        {
            Unit.RemoveAbility(96);
            Result = true;
        }

        if (Unit.GetAbilityEnabled(53))
        {
            Unit.RemoveAbility(53);
            Result = true;
        }

        if (Unit.GetAbilityEnabled(108))
        {
            Unit.RemoveAbility(108);
            Result = true;
        }

        Unit.Changed();
        return Result;
    }

    // AoWAb.PAS:3659
    public override EClass AbilityDataClass() => P4HealingDataClass;

    private static EClass P4HealingDataClass = EClass.Of<HealingAbilityData>();
    private static readonly string HealingRStr = "Healing";

}
