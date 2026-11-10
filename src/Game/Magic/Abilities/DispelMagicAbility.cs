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

public partial class DispelMagicAbility : TouchAbility
{
    // AoWAb.PAS:4040
    public DispelMagicAbility(ECustomNode owner) : base(owner)
    {
        FAbilityID = 60;
        FAbilityName = AbilityRules.TranslateRStr(GameText.DispelMagicRStr);
        FSelectionTypes = Set<AbilitySelectionType>.Of(AbilitySelectionType.Unit, AbilitySelectionType.UseItem, AbilitySelectionType.Editor);
        FAbilityType = AbilityType.Enhancement;
    }

    // AoWAb.PAS:4129
    public override string GetName(AbilityOwner Owner)
    {
        string Result = GetLevelName(GetLevel(Owner));
        if (Owner != null && !GetEnabled(Owner))
        {
            Result = Result + " (" + LowerCase(AbilityRules.TranslateRStr(GameText.UsedRStr)) + ")";
        }

        return Result;
    }

    // AoWAb.PAS:4096
    public override int GetLevel(AbilityOwner Owner)
    {
        int Result;
        DispelMagicAbilityData Data;
        if (Owner != null)
        {
            if (Owner.GetAbSet(60))
            {
                Data = (DispelMagicAbilityData)Owner.GetAbilityData(FAbilityID);
                if (Data != null)
                {
                    Result = Data.FLevel;
                }
                else
                {
                    Result = 1;
                }
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

    // AoWAb.PAS:4064
    public override bool GetEnabled(AbilityOwner owner)
    {
        DispelMagicAbilityData Data;
        bool Result = owner.GetAbSet(FAbilityID);
        if (Result)
        {
            Data = (DispelMagicAbilityData)owner.GetAbilityData(FAbilityID);
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

    // AoWAb.PAS:4148
    public override int GetSkillPoints(AbilityOwner owner)
    {
        int Result = GetLevel(owner);
        return Result * 5;
    }

    // AoWAb.PAS:4048
    public override Set<AbilityControlTypeElement> GetControlType(AbilityOwner Owner) => Set<AbilityControlTypeElement>.Of(AbilityControlTypeElement.Action, AbilityControlTypeElement.CombatAction);

    // AoWAb.PAS:4275
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

    // AoWAb.PAS:4262
    public override bool fcExecuteCombatCommand(CombatObject Source, FastCombatCommand Command)
    {
        CombatObject Target;
        DispelMagicAbilityCA Action;
        Target = Command.FTarget;
        fcValidateMoveOutsideWall(Source, Target);
        Action = new DispelMagicAbilityCA(null);
        Action.Generate(Source, Target, GetDispelMana(((CombatUnit)Source).FUnit));
        Source.FCombat.ExecuteCombatAction(Action);
        Action.Release();
        return true;
    }

    // AoWAb.PAS:4087
    public override void NewTurn(AbilityOwner Owner, sbyte Player)
    {
        if (Owner is AbstractUnit && ((AbstractUnit)Owner).FPlayer == Player)
        {
            SetEnabled(Owner, true);
        }
    }

    // AoWAb.PAS:4311
    public override bool Activate(AbilityOwner Owner, ref string Reason)
    {
        bool Result;
        UnitSelectorAAI Info;
        DispelMagicAbilityUnitSelector Selector;
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

            Selector = new DispelMagicAbilityUnitSelector();
            var dispelMagic = AbilityRules.TranslateRStr(GameText.DispelMagicRStr);
            Selector.FCaption = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.SelectUnitForXRStr), [dispelMagic]);
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
            Selector.Release();
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

    // AoWAb.PAS:4139
    public override bool CanExpand(AbilityOwner owner)
    {
        if ((owner.GetAbilitySelectionTypes() & FSelectionTypes) == Set<AbilitySelectionType>.Of() || GetLevel(owner) >= 3)
        {
            return false;
        }

        return true;
    }

    // AoWAb.PAS:4135
    public override int ExpandCost(AbilityOwner owner) => 5;

    // AoWAb.PAS:4125
    public override string ExpandName(AbilityOwner Owner) => GetLevelName(GetLevel(Owner) + 1);

    // AoWAb.PAS:4152
    public override bool Expand(AbilityOwner Owner)
    {
        DispelMagicAbilityData Data;
        bool Result = false;
        Data = (DispelMagicAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new DispelMagicAbilityData(null);
            Data.FLevel = 1;
            Data.FEnabled = true;
            SetAb(Owner, FAbilityID, 1);
            Owner.AddAbilityData(Data);
            Result = true;
        }
        else
        {
            if (Data.FLevel != 3)
            {
                Result = true;
                Data.FLevel = (byte)(Data.FLevel + 1);
                // { Original skips level one when upgrading data created by SetEnabled. }
                if (Data.FLevel == 1)
                {
                    Data.FLevel = 2;
                }
            }
        }

        return Result;
    }

    // AoWAb.PAS:4175
    public override bool Remove(AbilityOwner owner)
    {
        DispelMagicAbilityData Data;
        bool Result = true;
        Data = (DispelMagicAbilityData)owner.GetAbilityData(FAbilityID);
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

    // AoWAb.PAS:4209
    public override int GetDamageValue(AbstractUnit Attacker, AbstractUnit Defender)
    {
        int Result;
        // { Original evaluates dispelling with the healing hit-point formula. }
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

    // AoWAb.PAS:4221
    public override void GetDamageValueEx(AbstractUnit Attacker, AbstractUnit Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        // { Original evaluates dispelling with the healing hit-point formula. }
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

    // AoWAb.PAS:4242
    public override void fcGetDamageValueEx(CombatObject Attacker, CombatObject Defender, WallSituation WallSituation, ref AbilityDamageValues DamageValues)
    {
        if (WallSituation == WallSituation.None && CanTouch(Attacker, Defender))
        {
            // { Original assigns zero benefit, so its prefetch loop emits no commands. }
            DamageValues.SourceCost = 0;
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

    // AoWAb.PAS:4078
    public override void GetCombatInfo(AbilityOwner Owner, ref AbilityCombatInfo Info)
    {
        base.GetCombatInfo(Owner, ref Info);
        Info.TargetTypes = Set<CombatTargetTypeElement>.Of(CombatTargetTypeElement.Friend, CombatTargetTypeElement.Enemy);
    }

    // AoWAb.PAS:4202
    public override bool CanTouch(CombatObject Source, CombatObject Target)
    {
        bool Result = base.CanTouch(Source, Target);
        if (Result)
        {
            bool condition = Source is CombatUnit && Target is CombatUnit;
            if (condition)
            {
                var player = Source.GetPlayer();
                condition = ((CombatUnit)Target).FUnit.CanDispelEnchantment(player);
            }

            Result = condition;
        }

        return Result;
    }

    // AoWAb.PAS:4197
    public override bool CanTouchUnit(AbstractUnit Source, AbstractUnit Target) => base.CanTouchUnit(Source, Target) && Target.CanDispelEnchantment(Source.FPlayer);

    // AoWAb.PAS:4052
    public void SetEnabled(AbilityOwner Owner, bool Enabled)
    {
        DispelMagicAbilityData Data;
        Data = (DispelMagicAbilityData)Owner.GetAbilityData(FAbilityID);
        if (Data == null)
        {
            Data = new DispelMagicAbilityData(null);
            Owner.AddAbilityData(Data);
        }

        Data.FEnabled = Enabled;
    }

    // AoWAb.PAS:4083
    public int GetDispelMana(AbilityOwner Owner) => GetLevel(Owner) * 10 + 20;

    // AoWAb.PAS:4116
    public string GetLevelName(int Level)
    {
        string Result = "";
        switch (Level)
        {
            case 0:
                Result = AbilityRules.TranslateRStr(GameText.DispelMagicRStr);
                break;
            case 1:
                Result = AbilityRules.TranslateRStr(GameText.DispelMagicRStr) + " I";
                break;
            case 2:
                Result = AbilityRules.TranslateRStr(GameText.DispelMagicRStr) + " II";
                break;
            case 3:
                Result = AbilityRules.TranslateRStr(GameText.DispelMagicRStr) + " III";
                break;
        }

        return Result;
    }

    // AoWAb.PAS:4301
    public DispelMagicAbilityCA CreateCA(CombatUnit Source, CombatObject Target)
    {
        bool Successful;
        DispelMagicAbilityCA Result = new DispelMagicAbilityCA(null);
        Result.Generate(Source, Target, GetDispelMana(Source.FUnit));
        if (Source.GetSide() == Target.GetSide())
        {
            Successful = true;
        }
        else
        {
            Successful = CombatTouchRole(Source, Target);
        }

        if (!Successful)
        {
            Result.FEnchantmentIDs.Clear();
        }

        return Result;
    }

    // AoWAb.PAS:4092
    public override EClass AbilityDataClass() => P4DispelMagicDataClass;

    private static EClass P4DispelMagicDataClass = EClass.Of<DispelMagicAbilityData>();

}
