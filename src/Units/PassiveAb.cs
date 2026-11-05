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
            var call__1 = TranslateRStr(HolyChampionRStr);
            FAbilityName = call__1;
            FAbilityID = 146;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:492
        public override bool CanExpand(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.CanExpand(A1);
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
            var call__2 = TranslateRStr(UnholyChampionRStr);
            FAbilityName = call__2;
            FAbilityID = 147;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:507
        public override bool CanExpand(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.CanExpand(A1);
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
            var call__3 = TranslateRStr(NaturesBlessingRStr);
            FAbilityName = call__3;
            FAbilityID = 99;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:524
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:529
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:534
        public override sbyte GetResistance(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:539
        public override bool Expand(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Expand(A1);
            if (A1 is TAbstractUnit)
            {
                var unit = (TAbstractUnit)A1;
                var call__5 = unit.GetHits();
                unit.SetHitPoints(call__5);
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
            var call__6 = TranslateRStr(HighPrayerBlessingRStr);
            FAbilityName = call__6;
            FAbilityID = 100;
            FAbilityType = TAbilityType.atEnhancement;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:1277
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1282
        public override sbyte GetResistance(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode. duration data
    // countdown, player index
    public partial class TCrusaderAbility : TDurationAbility
    {
        // PassiveAb.pas:703
        public TCrusaderAbility(TECustomNode owner) : base(owner)
        {
            var call__7 = TranslateRStr(CrusaderRStr);
            FAbilityName = call__7;
            FAbilityID = 101;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:724
        public override void NewTurn(TAbilityOwner A1, sbyte A2)
        {
            TDurationAbilityData Data = default;
            if (FDurationMode == TDurationMode.dmDay)
            {
                var call__8 = A1.GetAbilityData(FAbilityID);
                Data = (TDurationAbilityData)call__8;
                if (Data != null && Data.FDuration != 0 && Data.FPlayerIndex == A2)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                }
            }
        }

        // PassiveAb.pas:713
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Remove(A1);
            ((TAbstractUnit)A1).Killed();
            return Result;
        }

        // PassiveAb.pas:719
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TBloodlustAbility : TDurationAbility
    {
        // PassiveAb.pas:546
        public TBloodlustAbility(TECustomNode owner) : base(owner)
        {
            var call__9 = TranslateRStr(BloodlustRStr);
            FAbilityName = call__9;
            FAbilityID = 168;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:556
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:561
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // PassiveAb.pas:566
        public override sbyte GetDamage(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TPoisonedAbility : TDurationAbility
    {
        // PassiveAb.pas:571
        public TPoisonedAbility(TECustomNode owner) : base(owner)
        {
            var call__10 = TranslateRStr(PoisonedRStr);
            FAbilityName = call__10;
            FAbilityID = 96;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:581
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // PassiveAb.pas:586
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // PassiveAb.pas:591
        public override sbyte GetResistance(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // PassiveAb.pas:596
        public override sbyte GetDamage(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TDecayAbility : TDurationAbility
    {
        // PassiveAb.pas:844
        public TDecayAbility(TECustomNode owner) : base(owner)
        {
            var call__11 = TranslateRStr(DecayRStr);
            FAbilityName = call__11;
            FAbilityID = 131;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:855
        public override void NewCombatTurn(TCombatObject A1, sbyte A2)
        {
            TDurationAbilityData Data = default;
            var call__13 = A1.GetAbilityOwner();
            var call__14 = call__13.GetAbilityData(FAbilityID);
            Data = (TDurationAbilityData)call__14;
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
            var call__15 = TranslateRStr(WebbedRStr);
            FAbilityName = call__15;
            FAbilityID = 97;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:751
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:746
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TStunnedAbility : TDurationAbility
    {
        // PassiveAb.pas:793
        public TStunnedAbility(TECustomNode owner) : base(owner)
        {
            var call__16 = TranslateRStr(StunnedRStr);
            FAbilityName = call__16;
            FAbilityID = 92;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:810
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:804
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 1;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TBurningAbility : TDurationAbility
    {
        // PassiveAb.pas:756
        public TBurningAbility(TECustomNode owner) : base(owner)
        {
            var call__17 = TranslateRStr(BurningRStr);
            FAbilityName = call__17;
            FAbilityID = 127;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:773
        public override void NewCombatTurn(TCombatObject A1, sbyte A2)
        {
            TDurationAbilityData Data = default;
            var call__18 = A1.GetAbilityOwner();
            var call__19 = call__18.GetAbilityData(FAbilityID);
            Data = (TDurationAbilityData)call__19;
            if (Data != null)
            {
                if (Data.FDuration != 0 && Data.FPlayerIndex == A2)
                {
                    Data.FDuration = (byte)(Data.FDuration - 1);
                    if (Data.FDuration == 0)
                    {
                        var call__20 = A1.GetAbilityOwner();
                        call__20.RemoveAbility(FAbilityID);
                    }

                    if (A1.GetAbilityOwner() is TAbstractUnit)
                    {
                        var unit = (TAbstractUnit)A1.GetAbilityOwner();
                        var call__22 = unit.GetResistance();
                        var call__23 = HitRole(4 - call__22);
                        if (call__23)
                        {
                            A1.ExecuteDamage(1);
                        }
                    }
                }
            }
            else
            {
                var call__24 = A1.GetAbilityOwner();
                call__24.RemoveAbility(FAbilityID);
            }
        }

        // PassiveAb.pas:767
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TVertigoAbility : TDurationAbility
    {
        // PassiveAb.pas:952
        public TVertigoAbility(TECustomNode owner) : base(owner)
        {
            var call__25 = TranslateRStr(VertigoRStr);
            FAbilityName = call__25;
            FAbilityID = 98;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmDay;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:969
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:975
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:963
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TCursedAbility : TDurationAbility
    {
        // PassiveAb.pas:981
        public TCursedAbility(TECustomNode owner) : base(owner)
        {
            var call__26 = TranslateRStr(CursedRStr);
            FAbilityName = call__26;
            FAbilityID = 93;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:1009
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:1015
        public override sbyte GetResistance(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:991
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result = default;
            TDurationAbilityData Data = default;
            var call__27 = A1.GetAbilityData(FAbilityID);
            Data = (TDurationAbilityData)call__27;
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
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class THolyFear : TDurationAbility
    {
        // PassiveAb.pas:816
        public THolyFear(TECustomNode owner) : base(owner)
        {
            var call__28 = TranslateRStr(HolyFearRStr);
            FAbilityName = call__28;
            FAbilityID = 53;
            FAbilityType = TAbilityType.atHandicap;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
            FDurationMode = TDurationMode.dmDay;
        }

        // PassiveAb.pas:833
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result = default;
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
        public override int GetTotalDuration()
        {
            int Result = default;
            Result = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TPanickedAbility : TDurationAbility
    {
        // PassiveAb.pas:874
        public TPanickedAbility(TECustomNode owner) : base(owner)
        {
            var call__30 = TranslateRStr(PanickedRStr);
            FAbilityName = call__30;
            FAbilityID = 108;
            FAbilityType = TAbilityType.atHandicap;
            FDurationMode = TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:885
        public override bool Expand(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Expand(A1);
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
            var call__31 = TranslateRStr(WallClimbingRStr);
            FAbilityName = call__31;
            FAbilityID = 36;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:931
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1)
        {
            TSet<TWallCombatFeaturesElement> Result = default;
            Result = TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TPassWallAbility : TAdjustableAbility
    {
        // PassiveAb.pas:937
        public TPassWallAbility(TECustomNode owner) : base(owner)
        {
            var call__32 = TranslateRStr(PassWallRStr);
            FAbilityName = call__32;
            FAbilityID = 66;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem);
        }

        // PassiveAb.pas:946
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1)
        {
            TSet<TWallCombatFeaturesElement> Result = default;
            Result = TSet<TWallCombatFeaturesElement>.Of(TWallCombatFeaturesElement.wcWallPassing, TWallCombatFeaturesElement.wcParticipate);
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TResurrectedAbility : TAdjustableAbility
    {
        // PassiveAb.pas:892
        public TResurrectedAbility(TECustomNode owner) : base(owner)
        {
            var call__33 = TranslateRStr(ResurrectedRStr);
            FAbilityName = call__33;
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
            var call__34 = TranslateRStr(AnimatedRStr);
            FAbilityName = call__34;
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
            var call__35 = TranslateRStr(ResurrectedRStr);
            FAbilityName = call__35;
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
            var call__36 = TranslateRStr(MarksmanshipRStr);
            FAbilityName = call__36;
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
        public override TSet<TMoveType> GetMoveTypes(TAbilityOwner A1)
        {
            TSet<TMoveType> Result = default;
            Result = FMoveTypes;
            return Result;
        }

        // PassiveAb.pas:1422
        public override TSet<TWallCombatFeaturesElement> GetWallCombatFeatures(TAbilityOwner A1)
        {
            TSet<TWallCombatFeaturesElement> Result = default;
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
            TSet<TMoveType> Result = default;
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
        public override TSet<TDamageType> GetImmunityTypes(TAbilityOwner A1)
        {
            TSet<TDamageType> Result = default;
            Result = FImmunityTypes;
            return Result;
        }
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
        public override TSet<TDamageType> GetProtectionTypes(TAbilityOwner A1)
        {
            TSet<TDamageType> Result = default;
            Result = FProtectionTypes;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TEntangledAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:620
        public TEntangledAbility(TECustomNode owner) : base(owner)
        {
            var call__37 = TranslateRStr(EntangledRStr);
            FAbilityName = call__37;
            FAbilityID = 94;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:638
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:628
        public override TDurationMode GetDurationMode()
        {
            TDurationMode Result = default;
            Result = TDurationMode.dmCombatTurn;
            return Result;
        }

        // PassiveAb.pas:633
        public override sbyte GetTotalDuration()
        {
            sbyte Result = default;
            Result = 3;
            return Result;
        }

        // PassiveAb.pas:643
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
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
            var call__38 = TranslateRStr(FrozenRStr);
            FAbilityName = call__38;
            FAbilityID = 95;
            FAbilityType = TAbilityType.atEnchantment;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:674
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -2;
            return Result;
        }

        // PassiveAb.pas:658
        public override TDurationMode GetDurationMode()
        {
            TDurationMode Result = default;
            Result = TDurationMode.dmCombatTurn;
            return Result;
        }

        // PassiveAb.pas:663
        public override sbyte GetTotalDuration()
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }

        // PassiveAb.pas:668
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
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
            var call__39 = TranslateRStr(TurnedUndeadRStr);
            FAbilityName = call__39;
            FAbilityID = 34;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:693
        public override TDurationMode GetDurationMode()
        {
            TDurationMode Result = default;
            Result = TDurationMode.dmCombatTurn;
            return Result;
        }

        // PassiveAb.pas:698
        public override sbyte GetTotalDuration()
        {
            sbyte Result = default;
            Result = 3;
            return Result;
        }

        // PassiveAb.pas:687
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
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
            var call__40 = TranslateRStr(HasteRStr);
            FAbilityName = call__40;
            FAbilityID = 152;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TStoneSkinAbility : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1029
        public TStoneSkinAbility(TECustomNode owner) : base(owner)
        {
            var call__41 = TranslateRStr(StoneSkinRStr);
            FAbilityName = call__41;
            FAbilityID = 153;
        }

        // PassiveAb.pas:1048
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }

        // PassiveAb.pas:1037
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result = default;
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
            var call__43 = TranslateRStr(EnchantedWeaponRStr);
            FAbilityName = call__43;
            FAbilityID = 154;
        }

        // PassiveAb.pas:1081
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1087
        public override sbyte GetDamage(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1093
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = default;
            Result = ((TAbstractUnit)A1).GetUnitLevel() + 225;
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
            var call__44 = TranslateRStr(SummonedRStr);
            FAbilityName = call__44;
            FAbilityID = 155;
        }

        // PassiveAb.pas:1122
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Remove(A1);
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
            int Result = default;
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
            int Result = default;
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
            int Result = default;
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
            int Result = default;
            var call__46 = AoWHSSet.FUnitResources.GetUnitResource(A1);
            switch (call__46.FLevel)
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
            var call__47 = TranslateRStr(FuryRStr);
            FAbilityName = call__47;
            FAbilityID = 156;
        }

        // PassiveAb.pas:1168
        public override sbyte GetAttack(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }

        // PassiveAb.pas:1174
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = -1;
            return Result;
        }

        // PassiveAb.pas:1180
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = default;
            Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
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
            var call__48 = TranslateRStr(FreeMovementRStr);
            FAbilityName = call__48;
            FAbilityID = 157;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THolyChampionEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1195
        public THolyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            var call__49 = TranslateRStr(HolyChampionRStr);
            FAbilityName = call__49;
            FAbilityID = 160;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TUnholyChampionEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1203
        public TUnholyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            var call__50 = TranslateRStr(UnholyChampionRStr);
            FAbilityName = call__50;
            FAbilityID = 161;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TConcealmentEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1287
        public TConcealmentEnchantment(TECustomNode owner) : base(owner)
        {
            var call__51 = TranslateRStr(ConcealmentRStr);
            FAbilityName = call__51;
            FAbilityID = 162;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TFireAuraEnchantment : TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1294
        public TFireAuraEnchantment(TECustomNode owner) : base(owner)
        {
            var call__52 = TranslateRStr(FireAuraRStr);
            FAbilityName = call__52;
            FAbilityID = 163;
        }

        // PassiveAb.pas:1301
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
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
            var call__54 = TranslateRStr(WaterWalkingRStr);
            FAbilityName = call__54;
            FAbilityID = 164;
        }

        // PassiveAb.pas:1322
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
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
            var call__56 = TranslateRStr(WindWalkingRStr);
            FAbilityName = call__56;
            FAbilityID = 165;
        }

        // PassiveAb.pas:1343
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
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
            var call__58 = TranslateRStr(LiquidFormRStr);
            FAbilityName = call__58;
            FAbilityID = 166;
        }

        // PassiveAb.pas:1364
        public override bool Remove(TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
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
            int Result = default;
            Result = ((TAbstractUnit)A1).GetUnitLevel() + 300;
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
            var call__60 = TranslateRStr(BlessedRStr);
            FAbilityName = call__60;
            FAbilityID = 158;
        }

        // PassiveAb.pas:1219
        public override sbyte GetDefense(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1225
        public override sbyte GetResistance(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1231
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result = default;
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
            int Result = default;
            Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
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
            var call__62 = TranslateRStr(DarkGiftRStr);
            FAbilityName = call__62;
            FAbilityID = 169;
        }

        // PassiveAb.pas:1256
        public override sbyte GetDamage(TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1261
        public override int GetAIUpkeepPriority(TAbilityOwner A1)
        {
            int Result = default;
            Result = ((TAbstractUnit)A1).GetUnitLevel() + 200;
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
            var call__63 = TranslateRStr(FireProtectionRStr);
            FAbilityName = call__63;
            FAbilityID = 167;
        }

        // PassiveAb.pas:1062
        public override AnsiString GetSourceName(TAbilityOwner A1)
        {
            AnsiString Result = default;
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
            var call__65 = TranslateRStr(SlowRStr);
            FAbilityName = call__65;
            FAbilityID = 132;
            FSelectionTypes = TSet<TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:615
        public override void CombatDone(TCombatObject A1)
        {
            var call__66 = A1.GetAbilityOwner();
            call__66.RemoveAbility(FAbilityID);
        }

        // PassiveAb.pas:609
        public override TUnitEnchantment CreateEnchantment()
        {
            TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    public static partial class PassiveAb
    {
        // PassiveAb.pas:1504
        public static void RegisterPassiveAbilities(TAbilityControl Control)
        {
            var call__67 = TranslateRStr(FireImmunityRStr);
            var call__68 = CreateImmunityAbility(7, call__67, TSet<TDamageType>.Of(TDamageType.dtFire));
            Control.RegisterAbility(call__68);
            var call__69 = TranslateRStr(ColdImmunityRStr);
            var call__70 = CreateImmunityAbility(8, call__69, TSet<TDamageType>.Of(TDamageType.dtCold));
            Control.RegisterAbility(call__70);
            var call__71 = TranslateRStr(LightningImmunityRStr);
            var call__72 = CreateImmunityAbility(9, call__71, TSet<TDamageType>.Of(TDamageType.dtLightning));
            Control.RegisterAbility(call__72);
            var call__73 = TranslateRStr(PoisonImmunityRStr);
            var call__74 = CreateImmunityAbility(10, call__73, TSet<TDamageType>.Of(TDamageType.dtPoison));
            Control.RegisterAbility(call__74);
            var call__75 = TranslateRStr(DeathImmunityRStr);
            var call__76 = CreateImmunityAbility(11, call__75, TSet<TDamageType>.Of(TDamageType.dtDeath));
            Control.RegisterAbility(call__76);
            var call__77 = TranslateRStr(HolyImmunityRStr);
            var call__78 = CreateImmunityAbility(12, call__77, TSet<TDamageType>.Of(TDamageType.dtHoly));
            Control.RegisterAbility(call__78);
            var call__79 = TranslateRStr(PhysicalImmunityRStr);
            var call__80 = CreateImmunityAbility(13, call__79, TSet<TDamageType>.Of(TDamageType.dtPhysical));
            Control.RegisterAbility(call__80);
            var call__81 = TranslateRStr(MagicImmunityRStr);
            var call__82 = CreateImmunityAbility(6, call__81, TSet<TDamageType>.Of(TDamageType.dtMagic));
            Control.RegisterAbility(call__82);
            var call__83 = TranslateRStr(FireProtectionRStr);
            var call__84 = CreateProtectionAbility(71, call__83, TSet<TDamageType>.Of(TDamageType.dtFire), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__84);
            var call__85 = TranslateRStr(ColdProtectionRStr);
            var call__86 = CreateProtectionAbility(76, call__85, TSet<TDamageType>.Of(TDamageType.dtCold), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__86);
            var call__87 = TranslateRStr(LightningProtectionRStr);
            var call__88 = CreateProtectionAbility(74, call__87, TSet<TDamageType>.Of(TDamageType.dtLightning), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__88);
            var call__89 = TranslateRStr(PoisonProtectionRStr);
            var call__90 = CreateProtectionAbility(73, call__89, TSet<TDamageType>.Of(TDamageType.dtPoison), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__90);
            var call__91 = TranslateRStr(DeathProtectionRStr);
            var call__92 = CreateProtectionAbility(70, call__91, TSet<TDamageType>.Of(TDamageType.dtDeath), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__92);
            var call__93 = TranslateRStr(HolyProtectionRStr);
            var call__94 = CreateProtectionAbility(72, call__93, TSet<TDamageType>.Of(TDamageType.dtHoly), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__94);
            var call__95 = TranslateRStr(PhysicalProtectionRStr);
            var call__96 = CreateProtectionAbility(77, call__95, TSet<TDamageType>.Of(TDamageType.dtPhysical), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__96);
            var call__97 = TranslateRStr(MagicProtectionRStr);
            var call__98 = CreateProtectionAbility(75, call__97, TSet<TDamageType>.Of(TDamageType.dtMagic), TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__98);
            var call__99 = TranslateRStr(WalkingRStr);
            var call__100 = CreateMovementAbility(0, call__99, TSet<TMoveType>.Of(TMoveType.mtWalking));
            Control.RegisterAbility(call__100);
            var call__101 = TranslateRStr(FlyingRStr);
            var call__102 = CreateMovementAbility(1, call__101, TSet<TMoveType>.Of(TMoveType.mtFlying));
            Control.RegisterAbility(call__102);
            var call__103 = TranslateRStr(SwimmingRStr);
            var call__104 = CreateMovementAbility(2, call__103, TSet<TMoveType>.Of(TMoveType.mtSwimming));
            Control.RegisterAbility(call__104);
            var call__105 = TranslateRStr(ForestryRStr);
            var call__106 = CreateMovementAbility(3, call__105, TSet<TMoveType>.Of(TMoveType.mtForestry));
            Control.RegisterAbility(call__106);
            var call__107 = TranslateRStr(CaveCrawlingRStr);
            var call__108 = CreateMovementAbility(4, call__107, TSet<TMoveType>.Of(TMoveType.mtCaveCrawling));
            Control.RegisterAbility(call__108);
            var call__109 = TranslateRStr(MountaineeringRStr);
            var call__110 = CreateMovementAbility(5, call__109, TSet<TMoveType>.Of(TMoveType.mtMountaineering));
            Control.RegisterAbility(call__110);
            var call__111 = TranslateRStr(SailingRStr);
            var call__112 = CreateMovementAbility(23, call__111, TSet<TMoveType>.Of(TMoveType.mtSwimming));
            Control.RegisterAbility(call__112);
            var call__113 = TranslateRStr(TunnelingRStr);
            var call__114 = CreateMovementAbility(42, call__113, TSet<TMoveType>.Of(TMoveType.mtTunneling));
            Control.RegisterAbility(call__114);
            var call__115 = TranslateRStr(FloatingRStr);
            var call__116 = CreateMovementAbility(59, call__115, TSet<TMoveType>.Of(TMoveType.mtFlying));
            Control.RegisterAbility(call__116);
            var call__117 = TranslateRStr(TrailOfDarknessRStr);
            var call__118 = CreateEnhancementAbility(26, call__117, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__118);
            var call__119 = TranslateRStr(NightVisionRStr);
            var call__120 = CreateEnhancementAbility(39, call__119, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__120);
            var call__121 = TranslateRStr(IgnitionRStr);
            var call__122 = CreateEnhancementAbility(43, call__121, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__122);
            var call__123 = TranslateRStr(RegenerationRStr);
            var call__124 = CreateEnhancementAbility(49, call__123, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__124);
            var call__125 = TranslateRStr(CauseFearRStr);
            var call__126 = CreateEnhancementAbility(51, call__125, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__126);
            var call__127 = TranslateRStr(InvisibilityRStr);
            var call__128 = CreateEnhancementAbility(54, call__127, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__128);
            var call__129 = TranslateRStr(DragonRStr);
            var call__130 = CreateEnhancementAbility(63, call__129, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit));
            Control.RegisterAbility(call__130);
            var call__131 = TranslateRStr(BardsSkillsRStr);
            var call__132 = CreateEnhancementAbility(37, call__131, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__132);
            var call__133 = TranslateRStr(TrueVisionRStr);
            var call__134 = CreateEnhancementAbility(41, call__133, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__134);
            var call__135 = TranslateRStr(FearlessRStr);
            var call__136 = CreateEnhancementAbility(67, call__135, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__136);
            var call__137 = TranslateRStr(PathOfDecayRStr);
            var call__138 = CreateEnhancementAbility(68, call__137, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__138);
            var call__139 = TranslateRStr(PathOfLifeRStr);
            var call__140 = CreateEnhancementAbility(69, call__139, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__140);
            var call__141 = TranslateRStr(PathOfFrostRStr);
            var call__142 = CreateEnhancementAbility(109, call__141, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__142);
            var call__143 = new TVisionAbility(null);
            Control.RegisterAbility(call__143);
            var call__144 = new THolyChampionAbility(null);
            Control.RegisterAbility(call__144);
            var call__145 = new TUnholyChampionAbility(null);
            Control.RegisterAbility(call__145);
            var call__146 = new TWallClimbingAbility(null);
            Control.RegisterAbility(call__146);
            var call__147 = new TPassWallAbility(null);
            Control.RegisterAbility(call__147);
            var call__148 = new TCursedAbility(null);
            Control.RegisterAbility(call__148);
            var call__149 = new TResurrectedAbility(null);
            Control.RegisterAbility(call__149);
            var call__150 = new TAnimatedAbility(null);
            Control.RegisterAbility(call__150);
            var call__151 = new TCombatResurrectedAbility(null);
            Control.RegisterAbility(call__151);
            var call__152 = new TMarksmanshipAbility(null);
            Control.RegisterAbility(call__152);
            var call__153 = new TSlowEnchantmentAbility(null);
            Control.RegisterAbility(call__153);
            var call__154 = new TWebbedAbility(null);
            Control.RegisterAbility(call__154);
            var call__155 = new TVertigoAbility(null);
            Control.RegisterAbility(call__155);
            var call__156 = new TPoisonedAbility(null);
            Control.RegisterAbility(call__156);
            var call__157 = new TEntangledAbility(null);
            Control.RegisterAbility(call__157);
            var call__158 = new TFrozenAbility(null);
            Control.RegisterAbility(call__158);
            var call__159 = new TStunnedAbility(null);
            Control.RegisterAbility(call__159);
            var call__160 = new TNaturesBlessingAbility(null);
            Control.RegisterAbility(call__160);
            var call__161 = new TBloodlustAbility(null);
            Control.RegisterAbility(call__161);
            var call__162 = new TTurnedUndeadAbility(null);
            Control.RegisterAbility(call__162);
            var call__163 = new THolyFear(null);
            Control.RegisterAbility(call__163);
            var call__164 = new THasteAbility(null);
            Control.RegisterAbility(call__164);
            var call__165 = new TStoneSkinAbility(null);
            Control.RegisterAbility(call__165);
            var call__166 = new TEnchantedWeaponAbility(null);
            Control.RegisterAbility(call__166);
            var call__167 = new TSummonedAbility(null);
            Control.RegisterAbility(call__167);
            var call__168 = new TFuryAbility(null);
            Control.RegisterAbility(call__168);
            var call__169 = new TFreeMovementAbility(null);
            Control.RegisterAbility(call__169);
            var call__170 = new THolyChampionEnchantment(null);
            Control.RegisterAbility(call__170);
            var call__171 = new TUnholyChampionEnchantment(null);
            Control.RegisterAbility(call__171);
            var call__172 = new TConcealmentEnchantment(null);
            Control.RegisterAbility(call__172);
            var call__173 = new TFireAuraEnchantment(null);
            Control.RegisterAbility(call__173);
            var call__174 = new TWaterWalkingEnchantment(null);
            Control.RegisterAbility(call__174);
            var call__175 = new TWindWalkingEnchantment(null);
            Control.RegisterAbility(call__175);
            var call__176 = new TLiquidFormEnchantment(null);
            Control.RegisterAbility(call__176);
            var call__177 = new TBlessedEnchantment(null);
            Control.RegisterAbility(call__177);
            var call__178 = new TCosmeticSurgeryAbility(null);
            Control.RegisterAbility(call__178);
            var call__179 = new TBurningAbility(null);
            Control.RegisterAbility(call__179);
            var call__180 = new TFireProtectionEnchantment(null);
            Control.RegisterAbility(call__180);
            var call__181 = new TPanickedAbility(null);
            Control.RegisterAbility(call__181);
            var call__182 = new TDecayAbility(null);
            Control.RegisterAbility(call__182);
            var call__183 = new THighPrayerBlessingAbility(null);
            Control.RegisterAbility(call__183);
            var call__184 = new TDarkGiftEnchantment(null);
            Control.RegisterAbility(call__184);
            var call__185 = new TCrusaderAbility(null);
            Control.RegisterAbility(call__185);
            var call__186 = TranslateRStr(SteppeConcealmentRStr);
            var call__187 = CreateEnhancementAbility(138, call__186, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__187);
            var call__188 = TranslateRStr(UndergroundConcealmentRStr);
            var call__189 = CreateEnhancementAbility(139, call__188, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__189);
            var call__190 = TranslateRStr(GrassConcealmentRStr);
            var call__191 = CreateEnhancementAbility(140, call__190, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__191);
            var call__192 = TranslateRStr(SnowConcealmentRStr);
            var call__193 = CreateEnhancementAbility(141, call__192, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__193);
            var call__194 = TranslateRStr(DesertConcealmentRStr);
            var call__195 = CreateEnhancementAbility(142, call__194, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__195);
            var call__196 = TranslateRStr(WastelandConcealmentRStr);
            var call__197 = CreateEnhancementAbility(143, call__196, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__197);
            var call__198 = TranslateRStr(WaterConcealmentRStr);
            var call__199 = CreateEnhancementAbility(144, call__198, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__199);
            var call__200 = TranslateRStr(ConcealmentRStr);
            var call__201 = CreateEnhancementAbility(145, call__200, TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astUnit, TAbilitySelectionType.astHeadItem, TAbilitySelectionType.astTorsoItem, TAbilitySelectionType.astDefenseItem, TAbilitySelectionType.astRingItem, TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__201);
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
            TMovementAbility Result = default;
            TMovementAbility Ability = default;
            Ability = new TMovementAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FMoveTypes = MoveTypes;
            if (MoveTypes.Contains(TMoveType.mtMountaineering) || MoveTypes.Contains(TMoveType.mtForestry) || MoveTypes.Contains(TMoveType.mtCaveCrawling))
            {
                Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<TAbilitySelectionType>.Of(TAbilitySelectionType.astEditor);
            }

            Result = Ability;
            return Result;
        }

        // PassiveAb.pas:1472
        public static TImmunityAbility CreateImmunityAbility(int ID, AnsiString Name, TSet<TDamageType> DamageTypes)
        {
            TImmunityAbility Result = default;
            TImmunityAbility Ability = default;
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

            Result = Ability;
            return Result;
        }

        // PassiveAb.pas:1491
        public static TProtectionAbility CreateProtectionAbility(int ID, AnsiString Name, TSet<TDamageType> DamageTypes, TSet<TAbilitySelectionType> SelectionTypes)
        {
            TProtectionAbility Result = default;
            TProtectionAbility Ability = default;
            Ability = new TProtectionAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FProtectionTypes = DamageTypes;
            Ability.FSelectionTypes = SelectionTypes;
            Result = Ability;
            return Result;
        }

        public static void Initialize()
        {
        }

        public static void Finalize()
        {
        }
    }
}
