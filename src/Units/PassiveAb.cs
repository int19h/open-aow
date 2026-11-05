// Translated from PassiveAb.pas.
using Aow.Rtl;
using Aow.Units.Engine;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PassiveAb
{
    // inherited name, ability ID, selection types, ability type. maximum level, level costs
    public partial class TVisionAbility : TMultiLevelAbility
    {
        // PassiveAb.pas:461
        public TVisionAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
            FMaxLevel = 4;
            FAbilityID = 64;
            FLevelCosts[1] = 5;
            FLevelCosts[2] = 5;
            FLevelCosts[3] = 5;
            FLevelCosts[4] = 5;
        }

        // PassiveAb.pas:473
        public override AnsiString GetLevelName(int A1)
        {
            AnsiString Result = default;
            switch (A1)
            {
                case 0:
                    Result = TranslateRStr(VisionRStr);
                    break;
                case 1:
                    Result = TranslateRStr(VisionRStr) + (AnsiString)" I";
                    break;
                case 2:
                    Result = TranslateRStr(VisionRStr) + (AnsiString)" II";
                    break;
                case 3:
                    Result = TranslateRStr(VisionRStr) + (AnsiString)" III";
                    break;
                case 4:
                    Result = TranslateRStr(VisionRStr) + (AnsiString)" IV";
                    break;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THolyChampionAbility : TEnhancementAbility
    {
        // PassiveAb.pas:484
        public THolyChampionAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(HolyChampionRStr);
            FAbilityID = 146;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:492
        public override bool CanExpand(TAbilityOwner A1)
        {
            bool Result = base.CanExpand(A1);
            if (Result && A1 is TAbstractUnit)
            {
                Result = alsPureGood.Contains(((TAbstractUnit)A1).GetAlignment());
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TUnholyChampionAbility : TEnhancementAbility
    {
        // PassiveAb.pas:499
        public TUnholyChampionAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(UnholyChampionRStr);
            FAbilityID = 147;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:507
        public override bool CanExpand(TAbilityOwner A1)
        {
            bool Result = base.CanExpand(A1);
            if (Result && A1 is TAbstractUnit)
            {
                Result = alsPureEvil.Contains(((TAbstractUnit)A1).GetAlignment());
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TNaturesBlessingAbility : TDurationAbility
    {
        // PassiveAb.pas:514
        public TNaturesBlessingAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(NaturesBlessingRStr);
            FAbilityID = 99;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:524
        public override sbyte GetAttack(TAbilityOwner A1) => 1;

        // PassiveAb.pas:529
        public override sbyte GetDefense(TAbilityOwner A1) => 1;

        // PassiveAb.pas:534
        public override sbyte GetResistance(TAbilityOwner A1) => 1;

        // PassiveAb.pas:539
        public override bool Expand(TAbilityOwner A1)
        {
            bool Result = base.Expand(A1);
            if (A1 is TAbstractUnit)
            {
                var unit = (TAbstractUnit)A1;
                unit.SetHitPoints(unit.GetHits());
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type, duration mode.)
    public partial class THighPrayerBlessingAbility : TDurationAbility
    {
        // PassiveAb.pas:1267
        public THighPrayerBlessingAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(HighPrayerBlessingRStr);
            FAbilityID = 100;
            FAbilityType = TAbilityType.atEnhancement;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:1277
        public override sbyte GetDefense(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1282
        public override sbyte GetResistance(TAbilityOwner A1) => 1;
    }

    // inherited name, ability ID, selection types, ability type, duration mode. duration data
    // countdown, player index
    public partial class TCrusaderAbility : TDurationAbility
    {
        // PassiveAb.pas:703
        public TCrusaderAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(CrusaderRStr);
            FAbilityID = 101;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:724
        public override void NewTurn(TAbilityOwner A1, sbyte A2)
        {
            TDurationAbilityData Data;
            if (FDurationMode == TDurationMode.dmDay)
            {
                Data = (TDurationAbilityData)A1.GetAbilityData(FAbilityID);
                if (Data != null && Data.FDuration != 0 && Data.FPlayerIndex == A2)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                }
            }
        }

        // PassiveAb.pas:713
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = base.Remove(A1);
            ((TAbstractUnit)A1).Killed();
            return Result;
        }

        // PassiveAb.pas:719
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TBloodlustAbility : TDurationAbility
    {
        // PassiveAb.pas:546
        public TBloodlustAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(BloodlustRStr);
            FAbilityID = 168;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:556
        public override sbyte GetAttack(TAbilityOwner A1) => 1;

        // PassiveAb.pas:561
        public override sbyte GetDefense(TAbilityOwner A1) => -1;

        // PassiveAb.pas:566
        public override sbyte GetDamage(TAbilityOwner A1) => 2;
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TPoisonedAbility : TDurationAbility
    {
        // PassiveAb.pas:571
        public TPoisonedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(PoisonedRStr);
            FAbilityID = 96;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:581
        public override sbyte GetAttack(TAbilityOwner A1) => -1;

        // PassiveAb.pas:586
        public override sbyte GetDefense(TAbilityOwner A1) => -1;

        // PassiveAb.pas:591
        public override sbyte GetResistance(TAbilityOwner A1) => -1;

        // PassiveAb.pas:596
        public override sbyte GetDamage(TAbilityOwner A1) => -1;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TDecayAbility : TDurationAbility
    {
        // PassiveAb.pas:844
        public TDecayAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(DecayRStr);
            FAbilityID = 131;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:855
        public override void NewCombatTurn(TCombatObject A1, sbyte A2)
        {
            TDurationAbilityData Data;
            Data = (TDurationAbilityData)A1.GetAbilityOwner().GetAbilityData(FAbilityID);
            if (Data != null && Data.FPlayerIndex == A2)
            {
                if (A1.GetAbilityOwner() is TAbstractUnit)
                {
                    A1.ExecuteDamage(1);
                }
            }
        }

        // PassiveAb.pas:868
        public override void CombatDone(TCombatObject A1)
        {
            A1.DestroyObject();
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TWebbedAbility : TDurationAbility
    {
        // PassiveAb.pas:736
        public TWebbedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(WebbedRStr);
            FAbilityID = 97;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:751
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:746
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TStunnedAbility : TDurationAbility
    {
        // PassiveAb.pas:793
        public TStunnedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(StunnedRStr);
            FAbilityID = 92;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:810
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:804
        public override int GetTotalDuration() => 1;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TBurningAbility : TDurationAbility
    {
        // PassiveAb.pas:756
        public TBurningAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(BurningRStr);
            FAbilityID = 127;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:773
        public override void NewCombatTurn(TCombatObject A1, sbyte A2)
        {
            TDurationAbilityData Data;
            Data = (TDurationAbilityData)A1.GetAbilityOwner().GetAbilityData(FAbilityID);
            if (Data != null)
            {
                if (Data.FDuration != 0 && Data.FPlayerIndex == A2)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                    if (Data.FDuration == 0)
                    {
                        A1.GetAbilityOwner().RemoveAbility(FAbilityID);
                    }

                    if (A1.GetAbilityOwner() is TAbstractUnit)
                    {
                        var unit = (TAbstractUnit)A1.GetAbilityOwner();
                        if (HitRole(4 - unit.GetResistance()))
                        {
                            A1.ExecuteDamage(1);
                        }
                    }
                }
            }
            else
            {
                A1.GetAbilityOwner().RemoveAbility(FAbilityID);
            }
        }

        // PassiveAb.pas:767
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TVertigoAbility : TDurationAbility
    {
        // PassiveAb.pas:952
        public TVertigoAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(VertigoRStr);
            FAbilityID = 98;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:969
        public override sbyte GetAttack(TAbilityOwner A1) => -2;

        // PassiveAb.pas:975
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:963
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TCursedAbility : TDurationAbility
    {
        // PassiveAb.pas:981
        public TCursedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(CursedRStr);
            FAbilityID = 93;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:1009
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:1015
        public override sbyte GetResistance(TAbilityOwner A1) => -2;

        // PassiveAb.pas:991
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result;
            TDurationAbilityData Data;
            Data = (TDurationAbilityData)A1.GetAbilityData(FAbilityID);
            if (Data != null && Data.FSource == 1)
            {
                Result = TranslateRStr(DeathMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }

        // PassiveAb.pas:1003
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class THolyFear : TDurationAbility
    {
        // PassiveAb.pas:816
        public THolyFear(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(HolyFearRStr);
            FAbilityID = 53;
            FAbilityType = TAbilityType.atHandicap;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:833
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result;
            // { Original assumes the ability data exists. }
            var durationAbilityData = (TDurationAbilityData)A1.GetAbilityData(FAbilityID);
            if (durationAbilityData.FSource == 1)
            {
                Result = TranslateRStr(LifeMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }

        // PassiveAb.pas:827
        public override int GetTotalDuration() => 3;
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TPanickedAbility : TDurationAbility
    {
        // PassiveAb.pas:874
        public TPanickedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(PanickedRStr);
            FAbilityID = 108;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:885
        public override bool Expand(TAbilityOwner A1)
        {
            bool Result = base.Expand(A1);
            if (A1 is TAbstractUnit)
            {
                ((TAbstractUnit)A1).Changed();
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TWallClimbingAbility : TAdjustableAbility
    {
        // PassiveAb.pas:922
        public TWallClimbingAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(WallClimbingRStr);
            FAbilityID = 36;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:931
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1) => TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TPassWallAbility : TAdjustableAbility
    {
        // PassiveAb.pas:937
        public TPassWallAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(PassWallRStr);
            FAbilityID = 66;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem);
        }

        // PassiveAb.pas:946
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1) => TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TResurrectedAbility : TAdjustableAbility
    {
        // PassiveAb.pas:892
        public TResurrectedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(ResurrectedRStr);
            FAbilityID = 128;
            FAbilityType = TAbilityType.atHandicap;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TAnimatedAbility : TAdjustableAbility
    {
        // PassiveAb.pas:902
        public TAnimatedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(AnimatedRStr);
            FAbilityID = 129;
            FAbilityType = TAbilityType.atHandicap;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TCombatResurrectedAbility : TAdjustableAbility
    {
        // PassiveAb.pas:912
        public TCombatResurrectedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(ResurrectedRStr);
            FAbilityID = 130;
            FAbilityType = TAbilityType.atHandicap;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }
    }

    // (maximum level, level costs.)
    public partial class TMarksmanshipAbility : TMultiLevelAbility
    {
        // PassiveAb.pas:1384
        public TMarksmanshipAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(MarksmanshipRStr);
            FAbilityType = TAbilityType.atEnhancement;
            FAbilityID = 32;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
            FMaxLevel = 4;
            FLevelCosts[1] = 5;
            FLevelCosts[2] = 5;
            FLevelCosts[3] = 5;
            FLevelCosts[4] = 5;
        }

        // PassiveAb.pas:1398
        public override AnsiString GetLevelName(int A1)
        {
            AnsiString Result = default;
            switch (A1)
            {
                case 0:
                    Result = TranslateRStr(MarksmanshipRStr);
                    break;
                case 1:
                    Result = TranslateRStr(MarksmanshipRStr) + (AnsiString)" I";
                    break;
                case 2:
                    Result = TranslateRStr(MarksmanshipRStr) + (AnsiString)" II";
                    break;
                case 3:
                    Result = TranslateRStr(MarksmanshipRStr) + (AnsiString)" III";
                    break;
                case 4:
                    Result = TranslateRStr(MarksmanshipRStr) + (AnsiString)" IV";
                    break;
            }

            return Result;
        }
    }

    // (FMoveTypes.)
    public partial class TMovementAbility : TAdjustableAbility
    {
        public TSet<TMoveType> FMoveTypes;
        // PassiveAb.pas:1409
        public TMovementAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astRingItem);
            FAbilityType = TAbilityType.atMovement;
            FSkillPointCost = 0;
        }

        // PassiveAb.pas:1417
        public override TSet<TMoveType> GetMoveTypes(TAbilityOwner A1) => FMoveTypes;

        // PassiveAb.pas:1422
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1)
        {
            TSet<TWallCombatFeaturesElement> Result;
            if (FAbilityID == 1)
            {
                Result = TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
            }
            else
            {
                Result = TSet<TWallCombatFeaturesElement>.Of();
            }

            return Result;
        }
    }

    // (FImmunityTypes.)
    public partial class TImmunityAbility : TEnhancementAbility
    {
        public TSet<TDamageType> FImmunityTypes;
        // PassiveAb.pas:1428
        public TImmunityAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem);
            FAbilityType = TAbilityType.atEnhancement;
        }

        // PassiveAb.pas:1440
        public override TSet<TMoveType> GetMoveTypes(TAbilityOwner A1)
        {
            TSet<TMoveType> Result;
            if (FImmunityTypes.Contains(TDamageType.dtFire))
            {
                Result = TSet<TMoveType>.Of(TMoveType.mtLavaCrossing);
            }
            else
            {
                Result = TSet<TMoveType>.Of();
            }

            return Result;
        }

        // PassiveAb.pas:1435
        public override TSet<TDamageType> GetImmunityTypes(TAbilityOwner A1) => FImmunityTypes;
    }

    // (FProtectionTypes.)
    public partial class TProtectionAbility : TEnhancementAbility
    {
        public TSet<TDamageType> FProtectionTypes;
        // PassiveAb.pas:1446
        public TProtectionAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
            FAbilityType = TAbilityType.atEnhancement;
        }

        // PassiveAb.pas:1453
        public override TSet<TDamageType> GetProtectionTypes(TAbilityOwner A1) => FProtectionTypes;
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TEntangledAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:620
        public TEntangledAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(EntangledRStr);
            FAbilityID = 94;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:638
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:628
        public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

        // PassiveAb.pas:633
        public override sbyte GetTotalDuration() => 3;

        // PassiveAb.pas:643
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFrozenAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:649
        public TFrozenAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(FrozenRStr);
            FAbilityID = 95;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:674
        public override sbyte GetDefense(TAbilityOwner A1) => -2;

        // PassiveAb.pas:658
        public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

        // PassiveAb.pas:663
        public override sbyte GetTotalDuration() => 2;

        // PassiveAb.pas:668
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TTurnedUndeadAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:679
        public TTurnedUndeadAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(TurnedUndeadRStr);
            FAbilityID = 34;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:693
        public override TDurationMode GetDurationMode() => TDurationMode.dmCombatTurn;

        // PassiveAb.pas:698
        public override sbyte GetTotalDuration() => 3;

        // PassiveAb.pas:687
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = base.CreateEnchantment();
            Result.FDispelResistance = 4;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THasteAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1021
        public THasteAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(HasteRStr);
            FAbilityID = 152;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TStoneSkinAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1029
        public TStoneSkinAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(StoneSkinRStr);
            FAbilityID = 153;
        }

        // PassiveAb.pas:1048
        public override sbyte GetDefense(TAbilityOwner A1) => 2;

        // PassiveAb.pas:1037
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = TranslateRStr(EarthMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TEnchantedWeaponAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1073
        public TEnchantedWeaponAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(EnchantedWeaponRStr);
            FAbilityID = 154;
        }

        // PassiveAb.pas:1081
        public override sbyte GetAttack(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1087
        public override sbyte GetDamage(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1093
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = ((TAbstractUnit)A1).GetUnitLevel() + 225;
            if (A1 is TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TSummonedAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1100
        public TSummonedAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(SummonedRStr);
            FAbilityID = 155;
        }

        // PassiveAb.pas:1122
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = base.Remove(A1);
            if (Result)
            {
                var unit = (TAbstractUnit)A1;
                unit.Killed();
                unit.SetOwner(null);
            }

            return Result;
        }

        // PassiveAb.pas:1134
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result;
            switch (((TAbstractUnit)A1).GetUnitLevel())
            {
                case 1:
                    Result = 250;
                    break;
                case 2:
                    Result = 250;
                    break;
                case 3:
                    Result = 500;
                    break;
                case 4:
                    Result = 1000;
                    break;
                default:
                    Result = 1000;
                    break;
            }

            return Result;
        }

        // PassiveAb.pas:1108
        public int GetDefaultManaUpkeep(TAbstractUnit AUnit)
        {
            int Result;
            if (AUnit is TUnit)
            {
                Result = (AUnit.GetUnitLevel() + 1) * 2;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // PassiveAb.pas:1115
        public int GetDefaultDispelResistance(TAbstractUnit AUnit)
        {
            int Result;
            if (AUnit is TUnit)
            {
                Result = AUnit.GetUnitLevel() + 3;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // PassiveAb.pas:1147
        public int UnitIndexToAIUpkeepPriority(int A1)
        {
            int Result;
            switch (AoWHSSet.FUnitResources.GetUnitResource(A1).FLevel)
            {
                case 1:
                    Result = 250;
                    break;
                case 2:
                    Result = 250;
                    break;
                case 3:
                    Result = 500;
                    break;
                case 4:
                    Result = 1000;
                    break;
                default:
                    Result = 1000;
                    break;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFuryAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1160
        public TFuryAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(FuryRStr);
            FAbilityID = 156;
        }

        // PassiveAb.pas:1168
        public override sbyte GetAttack(TAbilityOwner A1) => 2;

        // PassiveAb.pas:1174
        public override sbyte GetDefense(TAbilityOwner A1) => -1;

        // PassiveAb.pas:1180
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is TLeader)
            {
                Result -= 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFreeMovementAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1187
        public TFreeMovementAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(FreeMovementRStr);
            FAbilityID = 157;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THolyChampionEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1195
        public THolyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(HolyChampionRStr);
            FAbilityID = 160;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TUnholyChampionEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1203
        public TUnholyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(UnholyChampionRStr);
            FAbilityID = 161;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TConcealmentEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1287
        public TConcealmentEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(ConcealmentRStr);
            FAbilityID = 162;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TFireAuraEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1294
        public TFireAuraEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(FireAuraRStr);
            FAbilityID = 163;
        }

        // PassiveAb.pas:1301
        public override bool Remove(TAbilityOwner A1)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            bool Result = base.Remove(A1);
            if (Result && A1.Owner is TArmy)
            {
                ((TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((TArmy)A1.Owner).ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TWaterWalkingEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1315
        public TWaterWalkingEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(WaterWalkingRStr);
            FAbilityID = 164;
        }

        // PassiveAb.pas:1322
        public override bool Remove(TAbilityOwner A1)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            bool Result = base.Remove(A1);
            if (Result && A1.Owner is TArmy)
            {
                ((TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((TArmy)A1.Owner).ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TWindWalkingEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1336
        public TWindWalkingEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(WindWalkingRStr);
            FAbilityID = 165;
        }

        // PassiveAb.pas:1343
        public override bool Remove(TAbilityOwner A1)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            bool Result = base.Remove(A1);
            if (Result && A1.Owner is TArmy)
            {
                ((TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((TArmy)A1.Owner).ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TLiquidFormEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1357
        public TLiquidFormEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(LiquidFormRStr);
            FAbilityID = 166;
        }

        // PassiveAb.pas:1364
        public override bool Remove(TAbilityOwner A1)
        {
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            bool Result = base.Remove(A1);
            if (Result && A1.Owner is TArmy)
            {
                ((TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((TArmy)A1.Owner).ValidateTerrain(mapField.TerrainType, mapField.OverlayType);
                }
            }

            return Result;
        }

        // PassiveAb.pas:1378
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = ((TAbstractUnit)A1).GetUnitLevel() + 300;
            if (A1 is TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TBlessedEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1211
        public TBlessedEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(BlessedRStr);
            FAbilityID = 158;
        }

        // PassiveAb.pas:1219
        public override sbyte GetDefense(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1225
        public override sbyte GetResistance(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1231
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = TranslateRStr(LifeMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }

        // PassiveAb.pas:1242
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TDarkGiftEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1249
        public TDarkGiftEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(DarkGiftRStr);
            FAbilityID = 169;
        }

        // PassiveAb.pas:1256
        public override sbyte GetDamage(TAbilityOwner A1) => 1;

        // PassiveAb.pas:1261
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFireProtectionEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1054
        public TFireProtectionEnchantment(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(FireProtectionRStr);
            FAbilityID = 167;
        }

        // PassiveAb.pas:1062
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = TranslateRStr(FireMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TSlowEnchantmentAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:601
        public TSlowEnchantmentAbility(TECustomNode owner) : base(owner)
        {
            FAbilityName = TranslateRStr(SlowRStr);
            FAbilityID = 132;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:615
        public override void CombatDone(TCombatObject A1)
        {
            A1.GetAbilityOwner().RemoveAbility(FAbilityID);
        }

        // PassiveAb.pas:609
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    public static partial class PassiveAb
    {
        // PassiveAb.pas:1504
        public static void RegisterPassiveAbilities(TAbilityControl Control)
        {
            Control.RegisterAbility(CreateImmunityAbility(7, TranslateRStr(FireImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtFire)));
            Control.RegisterAbility(CreateImmunityAbility(8, TranslateRStr(ColdImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtCold)));
            Control.RegisterAbility(CreateImmunityAbility(9, TranslateRStr(LightningImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtLightning)));
            Control.RegisterAbility(CreateImmunityAbility(10, TranslateRStr(PoisonImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtPoison)));
            Control.RegisterAbility(CreateImmunityAbility(11, TranslateRStr(DeathImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtDeath)));
            Control.RegisterAbility(CreateImmunityAbility(12, TranslateRStr(HolyImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtHoly)));
            Control.RegisterAbility(CreateImmunityAbility(13, TranslateRStr(PhysicalImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtPhysical)));
            Control.RegisterAbility(CreateImmunityAbility(6, TranslateRStr(MagicImmunityRStr), TSet<TDamageType>.Of(TDamageType.dtMagic)));
            Control.RegisterAbility(CreateProtectionAbility(71, TranslateRStr(FireProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtFire), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(76, TranslateRStr(ColdProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtCold), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(74, TranslateRStr(LightningProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtLightning), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(73, TranslateRStr(PoisonProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtPoison), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(70, TranslateRStr(DeathProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtDeath), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(72, TranslateRStr(HolyProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtHoly), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(77, TranslateRStr(PhysicalProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtPhysical), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateProtectionAbility(75, TranslateRStr(MagicProtectionRStr), TSet<TDamageType>.Of(TDamageType.dtMagic), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateMovementAbility(0, TranslateRStr(WalkingRStr), TSet<TMoveType>.Of(TMoveType.mtWalking)));
            Control.RegisterAbility(CreateMovementAbility(1, TranslateRStr(FlyingRStr), TSet<TMoveType>.Of(TMoveType.mtFlying)));
            Control.RegisterAbility(CreateMovementAbility(2, TranslateRStr(SwimmingRStr), TSet<TMoveType>.Of(TMoveType.mtSwimming)));
            Control.RegisterAbility(CreateMovementAbility(3, TranslateRStr(ForestryRStr), TSet<TMoveType>.Of(TMoveType.mtForestry)));
            Control.RegisterAbility(CreateMovementAbility(4, TranslateRStr(CaveCrawlingRStr), TSet<TMoveType>.Of(TMoveType.mtCaveCrawling)));
            Control.RegisterAbility(CreateMovementAbility(5, TranslateRStr(MountaineeringRStr), TSet<TMoveType>.Of(TMoveType.mtMountaineering)));
            Control.RegisterAbility(CreateMovementAbility(23, TranslateRStr(SailingRStr), TSet<TMoveType>.Of(TMoveType.mtSwimming)));
            Control.RegisterAbility(CreateMovementAbility(42, TranslateRStr(TunnelingRStr), TSet<TMoveType>.Of(TMoveType.mtTunneling)));
            Control.RegisterAbility(CreateMovementAbility(59, TranslateRStr(FloatingRStr), TSet<TMoveType>.Of(TMoveType.mtFlying)));
            Control.RegisterAbility(CreateEnhancementAbility(26, TranslateRStr(TrailOfDarknessRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(39, TranslateRStr(NightVisionRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(43, TranslateRStr(IgnitionRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(49, TranslateRStr(RegenerationRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(51, TranslateRStr(CauseFearRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(54, TranslateRStr(InvisibilityRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(63, TranslateRStr(DragonRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit)));
            Control.RegisterAbility(CreateEnhancementAbility(37, TranslateRStr(BardsSkillsRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astEditor)));
            Control.RegisterAbility(CreateEnhancementAbility(41, TranslateRStr(TrueVisionRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor)));
            Control.RegisterAbility(CreateEnhancementAbility(67, TranslateRStr(FearlessRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(68, TranslateRStr(PathOfDecayRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(69, TranslateRStr(PathOfLifeRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(109, TranslateRStr(PathOfFrostRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(new TVisionAbility(null));
            Control.RegisterAbility(new THolyChampionAbility(null));
            Control.RegisterAbility(new TUnholyChampionAbility(null));
            Control.RegisterAbility(new TWallClimbingAbility(null));
            Control.RegisterAbility(new TPassWallAbility(null));
            Control.RegisterAbility(new TCursedAbility(null));
            Control.RegisterAbility(new TResurrectedAbility(null));
            Control.RegisterAbility(new TAnimatedAbility(null));
            Control.RegisterAbility(new TCombatResurrectedAbility(null));
            Control.RegisterAbility(new TMarksmanshipAbility(null));
            Control.RegisterAbility(new TSlowEnchantmentAbility(null));
            Control.RegisterAbility(new TWebbedAbility(null));
            Control.RegisterAbility(new TVertigoAbility(null));
            Control.RegisterAbility(new TPoisonedAbility(null));
            Control.RegisterAbility(new TEntangledAbility(null));
            Control.RegisterAbility(new TFrozenAbility(null));
            Control.RegisterAbility(new TStunnedAbility(null));
            Control.RegisterAbility(new TNaturesBlessingAbility(null));
            Control.RegisterAbility(new TBloodlustAbility(null));
            Control.RegisterAbility(new TTurnedUndeadAbility(null));
            Control.RegisterAbility(new THolyFear(null));
            Control.RegisterAbility(new THasteAbility(null));
            Control.RegisterAbility(new TStoneSkinAbility(null));
            Control.RegisterAbility(new TEnchantedWeaponAbility(null));
            Control.RegisterAbility(new TSummonedAbility(null));
            Control.RegisterAbility(new TFuryAbility(null));
            Control.RegisterAbility(new TFreeMovementAbility(null));
            Control.RegisterAbility(new THolyChampionEnchantment(null));
            Control.RegisterAbility(new TUnholyChampionEnchantment(null));
            Control.RegisterAbility(new TConcealmentEnchantment(null));
            Control.RegisterAbility(new TFireAuraEnchantment(null));
            Control.RegisterAbility(new TWaterWalkingEnchantment(null));
            Control.RegisterAbility(new TWindWalkingEnchantment(null));
            Control.RegisterAbility(new TLiquidFormEnchantment(null));
            Control.RegisterAbility(new TBlessedEnchantment(null));
            Control.RegisterAbility(new TCosmeticSurgeryAbility(null));
            Control.RegisterAbility(new TBurningAbility(null));
            Control.RegisterAbility(new TFireProtectionEnchantment(null));
            Control.RegisterAbility(new TPanickedAbility(null));
            Control.RegisterAbility(new TDecayAbility(null));
            Control.RegisterAbility(new THighPrayerBlessingAbility(null));
            Control.RegisterAbility(new TDarkGiftEnchantment(null));
            Control.RegisterAbility(new TCrusaderAbility(null));
            Control.RegisterAbility(CreateEnhancementAbility(138, TranslateRStr(SteppeConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(139, TranslateRStr(UndergroundConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(140, TranslateRStr(GrassConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(141, TranslateRStr(SnowConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(142, TranslateRStr(DesertConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(143, TranslateRStr(WastelandConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(144, TranslateRStr(WaterConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem)));
            Control.RegisterAbility(CreateEnhancementAbility(145, TranslateRStr(ConcealmentRStr), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor)));
        }

        // PassiveAb.pas:1597
        public static void UnregisterPassiveAbilities(TAbilityControl Control)
        {
            Control.UnregisterAbilityID(0);
            Control.UnregisterAbilityID(1);
            Control.UnregisterAbilityID(2);
            Control.UnregisterAbilityID(3);
            Control.UnregisterAbilityID(4);
            Control.UnregisterAbilityID(5);
            Control.UnregisterAbilityID(23);
            Control.UnregisterAbilityID(42);
            Control.UnregisterAbilityID(6);
            Control.UnregisterAbilityID(7);
            Control.UnregisterAbilityID(8);
            Control.UnregisterAbilityID(9);
            Control.UnregisterAbilityID(10);
            Control.UnregisterAbilityID(11);
            Control.UnregisterAbilityID(12);
            Control.UnregisterAbilityID(13);
            Control.UnregisterAbilityID(70);
            Control.UnregisterAbilityID(71);
            Control.UnregisterAbilityID(76);
            Control.UnregisterAbilityID(72);
            Control.UnregisterAbilityID(73);
            Control.UnregisterAbilityID(74);
            Control.UnregisterAbilityID(75);
            Control.UnregisterAbilityID(77);
            Control.UnregisterAbilityID(64);
            Control.UnregisterAbilityID(146);
            Control.UnregisterAbilityID(147);
            Control.UnregisterAbilityID(26);
            Control.UnregisterAbilityID(36);
            Control.UnregisterAbilityID(66);
            Control.UnregisterAbilityID(39);
            Control.UnregisterAbilityID(43);
            Control.UnregisterAbilityID(49);
            Control.UnregisterAbilityID(51);
            Control.UnregisterAbilityID(54);
            Control.UnregisterAbilityID(63);
            Control.UnregisterAbilityID(64);
            Control.UnregisterAbilityID(37);
            Control.UnregisterAbilityID(41);
            Control.UnregisterAbilityID(67);
            Control.UnregisterAbilityID(68);
            Control.UnregisterAbilityID(69);
            Control.UnregisterAbilityID(109);
            Control.UnregisterAbilityID(32);
            Control.UnregisterAbilityID(129);
            Control.UnregisterAbilityID(128);
            Control.UnregisterAbilityID(130);
            Control.UnregisterAbilityID(93);
            Control.UnregisterAbilityID(94);
            Control.UnregisterAbilityID(95);
            Control.UnregisterAbilityID(96);
            Control.UnregisterAbilityID(97);
            Control.UnregisterAbilityID(98);
            Control.UnregisterAbilityID(92);
            Control.UnregisterAbilityID(99);
            Control.UnregisterAbilityID(168);
            Control.UnregisterAbilityID(34);
            Control.UnregisterAbilityID(53);
            Control.UnregisterAbilityID(152);
            Control.UnregisterAbilityID(153);
            Control.UnregisterAbilityID(154);
            Control.UnregisterAbilityID(155);
            Control.UnregisterAbilityID(156);
            Control.UnregisterAbilityID(157);
            Control.UnregisterAbilityID(160);
            Control.UnregisterAbilityID(161);
            Control.UnregisterAbilityID(162);
            Control.UnregisterAbilityID(163);
            Control.UnregisterAbilityID(164);
            Control.UnregisterAbilityID(165);
            Control.UnregisterAbilityID(166);
            Control.UnregisterAbilityID(158);
            Control.UnregisterAbilityID(126);
            Control.UnregisterAbilityID(127);
            Control.UnregisterAbilityID(167);
            Control.UnregisterAbilityID(108);
            Control.UnregisterAbilityID(131);
            Control.UnregisterAbilityID(100);
            Control.UnregisterAbilityID(169);
            Control.UnregisterAbilityID(132);
            Control.UnregisterAbilityID(101);
            Control.UnregisterAbilityID(138);
            Control.UnregisterAbilityID(139);
            Control.UnregisterAbilityID(140);
            Control.UnregisterAbilityID(141);
            Control.UnregisterAbilityID(142);
            Control.UnregisterAbilityID(143);
            Control.UnregisterAbilityID(144);
            Control.UnregisterAbilityID(145);
        }

        // PassiveAb.pas:1458
        public static TMovementAbility CreateMovementAbility(int ID, AnsiString Name, TSet<TMoveType> MoveTypes)
        {
            TMovementAbility Ability;
            Ability = new TMovementAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FMoveTypes = MoveTypes;
            if (MoveTypes.Contains(TMoveType.mtMountaineering) || MoveTypes.Contains(TMoveType.mtForestry) || MoveTypes.Contains(TMoveType.mtCaveCrawling))
            {
                Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astEditor);
            }

            return Ability;
        }

        // PassiveAb.pas:1472
        public static TImmunityAbility CreateImmunityAbility(int ID, AnsiString Name, TSet<TDamageType> DamageTypes)
        {
            TImmunityAbility Ability;
            Ability = new TImmunityAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FImmunityTypes = DamageTypes;
            if ((TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtDeath, TDamageType.dtHoly, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
            {
                Ability.FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astRingItem);
            }
            else
            {
                if ((TSet<TDamageType>.Of(TDamageType.dtDeath, TDamageType.dtHoly, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
                {
                    Ability.FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astRingItem);
                }
                else
                {
                    Ability.FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit);
                }
            }

            if ((TSet<TDamageType>.Of(TDamageType.dtPhysical, TDamageType.dtMagic) & DamageTypes) == TSet<TDamageType>.Of())
            {
                Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astTorsoItem);
            }

            return Ability;
        }

        // PassiveAb.pas:1491
        public static TProtectionAbility CreateProtectionAbility(int ID, AnsiString Name, TSet<TDamageType> DamageTypes, TSet<TAbilitySelectionType> SelectionTypes)
        {
            TProtectionAbility Ability;
            Ability = new TProtectionAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FProtectionTypes = DamageTypes;
            Ability.FSelectionTypes = SelectionTypes;
            return Ability;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
