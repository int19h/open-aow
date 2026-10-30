// Translated from PassiveAb.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;

namespace Aow.Units.PassiveAb
{
    // inherited name, ability ID, selection types, ability type. maximum level, level costs
    public partial class TVisionAbility : Aow.Units.AoWE.TMultiLevelAbility
    {
        // PassiveAb.pas:461
        public TVisionAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
            FMaxLevel = 4;
            FAbilityID = 64;
            FLevelCosts[1] = 5;
            FLevelCosts[2] = 5;
            FLevelCosts[3] = 5;
            FLevelCosts[4] = 5;
        }

        // PassiveAb.pas:473
        public override Aow.Rtl.AnsiString GetLevelName(int A1)
        {
            Aow.Rtl.AnsiString Result = default;
            switch (A1)
            {
                case 0:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VisionRStr);
                    break;
                case 1:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VisionRStr) + ((AnsiString)" I");
                    break;
                case 2:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VisionRStr) + ((AnsiString)" II");
                    break;
                case 3:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VisionRStr) + ((AnsiString)" III");
                    break;
                case 4:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VisionRStr) + ((AnsiString)" IV");
                    break;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THolyChampionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        // PassiveAb.pas:484
        public THolyChampionAbility(TECustomNode owner) : base(owner)
        {
            var call__1 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyChampionRStr);
            FAbilityName = call__1;
            FAbilityID = 146;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:492
        public override bool CanExpand(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.CanExpand(A1);
            if (Result && (A1 is Aow.Units.AoWE.TAbstractUnit))
            {
                Result = Aow.Units.AoWE.AoWE.alsPureGood.Contains(((Aow.Units.AoWE.TAbstractUnit)A1).GetAlignment());
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TUnholyChampionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        // PassiveAb.pas:499
        public TUnholyChampionAbility(TECustomNode owner) : base(owner)
        {
            var call__2 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyChampionRStr);
            FAbilityName = call__2;
            FAbilityID = 147;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:507
        public override bool CanExpand(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.CanExpand(A1);
            if (Result && (A1 is Aow.Units.AoWE.TAbstractUnit))
            {
                Result = Aow.Units.AoWE.AoWE.alsPureEvil.Contains(((Aow.Units.AoWE.TAbstractUnit)A1).GetAlignment());
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TNaturesBlessingAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:514
        public TNaturesBlessingAbility(TECustomNode owner) : base(owner)
        {
            var call__3 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NaturesBlessingRStr);
            FAbilityName = call__3;
            FAbilityID = 99;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnchantment;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
        }

        // PassiveAb.pas:524
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:529
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:534
        public override sbyte GetResistance(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:539
        public override bool Expand(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Expand(A1);
            if (A1 is Aow.Units.AoWE.TAbstractUnit)
            {
                var unit = (Aow.Units.AoWE.TAbstractUnit)A1;
                var call__5 = unit.GetHits();
                unit.SetHitPoints(unchecked((sbyte)call__5));
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type, duration mode.)
    public partial class THighPrayerBlessingAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:1267
        public THighPrayerBlessingAbility(TECustomNode owner) : base(owner)
        {
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HighPrayerBlessingRStr);
            FAbilityName = call__6;
            FAbilityID = 100;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnhancement;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:1277
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1282
        public override sbyte GetResistance(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode. duration data
    // countdown, player index
    public partial class TCrusaderAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:703
        public TCrusaderAbility(TECustomNode owner) : base(owner)
        {
            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CrusaderRStr);
            FAbilityName = call__7;
            FAbilityID = 101;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:724
        public override void NewTurn(Aow.Units.AoWE.TAbilityOwner A1, sbyte A2)
        {
            Aow.Units.AoWE.TDurationAbilityData Data = default;
            if (FDurationMode == Aow.Units.AoWE.TDurationMode.dmDay)
            {
                var call__8 = A1.GetAbilityData(FAbilityID);
                Data = (Aow.Units.AoWE.TDurationAbilityData)call__8;
                if (((Data != null) && (Data.FDuration != 0)) && (Data.FPlayerIndex == A2))
                {
                    Data.FDuration = (byte)(((int)Data.FDuration) - 1);
                }
            }
        }

        // PassiveAb.pas:713
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Remove(A1);
            ((Aow.Units.AoWE.TAbstractUnit)A1).Killed();
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
    public partial class TBloodlustAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:546
        public TBloodlustAbility(TECustomNode owner) : base(owner)
        {
            var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BloodlustRStr);
            FAbilityName = call__9;
            FAbilityID = 168;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnchantment;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
        }

        // PassiveAb.pas:556
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:561
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // PassiveAb.pas:566
        public override sbyte GetDamage(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type. duration mode
    public partial class TPoisonedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:571
        public TPoisonedAbility(TECustomNode owner) : base(owner)
        {
            var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonedRStr);
            FAbilityName = call__10;
            FAbilityID = 96;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:581
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // PassiveAb.pas:586
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // PassiveAb.pas:591
        public override sbyte GetResistance(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // PassiveAb.pas:596
        public override sbyte GetDamage(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TDecayAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:844
        public TDecayAbility(TECustomNode owner) : base(owner)
        {
            var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DecayRStr);
            FAbilityName = call__11;
            FAbilityID = 131;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:855
        public override void NewCombatTurn(Aow.Units.AoWE.TCombatObject A1, sbyte A2)
        {
            Aow.Units.AoWE.TDurationAbilityData Data = default;
            var call__13 = A1.GetAbilityOwner();
            var call__14 = call__13.GetAbilityData(FAbilityID);
            Data = (Aow.Units.AoWE.TDurationAbilityData)call__14;
            if ((Data != null) && (Data.FPlayerIndex == A2))
            {
                if (A1.GetAbilityOwner() is Aow.Units.AoWE.TAbstractUnit)
                {
                    A1.ExecuteDamage(1);
                }
            }
        }

        // PassiveAb.pas:868
        public override void CombatDone(Aow.Units.AoWE.TCombatObject A1)
        {
            A1.DestroyObject();
        }
    }

    // inherited name, ability ID, selection types, ability type, duration mode
    public partial class TWebbedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:736
        public TWebbedAbility(TECustomNode owner) : base(owner)
        {
            var call__15 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WebbedRStr);
            FAbilityName = call__15;
            FAbilityID = 97;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:751
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
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
    public partial class TStunnedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:793
        public TStunnedAbility(TECustomNode owner) : base(owner)
        {
            var call__16 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StunnedRStr);
            FAbilityName = call__16;
            FAbilityID = 92;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:810
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
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
    public partial class TBurningAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:756
        public TBurningAbility(TECustomNode owner) : base(owner)
        {
            var call__17 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BurningRStr);
            FAbilityName = call__17;
            FAbilityID = 127;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:773
        public override void NewCombatTurn(Aow.Units.AoWE.TCombatObject A1, sbyte A2)
        {
            Aow.Units.AoWE.TDurationAbilityData Data = default;
            var call__18 = A1.GetAbilityOwner();
            var call__19 = call__18.GetAbilityData(FAbilityID);
            Data = (Aow.Units.AoWE.TDurationAbilityData)call__19;
            if (Data != null)
            {
                if ((Data.FDuration != 0) && (Data.FPlayerIndex == A2))
                {
                    Data.FDuration = (byte)(((int)Data.FDuration) - 1);
                    if (Data.FDuration == 0)
                    {
                        var call__20 = A1.GetAbilityOwner();
                        call__20.RemoveAbility(FAbilityID);
                    }

                    if (A1.GetAbilityOwner() is Aow.Units.AoWE.TAbstractUnit)
                    {
                        var unit = (Aow.Units.AoWE.TAbstractUnit)A1.GetAbilityOwner();
                        var call__22 = unit.GetResistance();
                        var call__23 = Aow.Units.AoWE.AoWE.HitRole(4 - call__22);
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
    public partial class TVertigoAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:952
        public TVertigoAbility(TECustomNode owner) : base(owner)
        {
            var call__25 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.VertigoRStr);
            FAbilityName = call__25;
            FAbilityID = 98;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:969
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // PassiveAb.pas:975
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
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
    public partial class TCursedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:981
        public TCursedAbility(TECustomNode owner) : base(owner)
        {
            var call__26 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CursedRStr);
            FAbilityName = call__26;
            FAbilityID = 93;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
        }

        // PassiveAb.pas:1009
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // PassiveAb.pas:1015
        public override sbyte GetResistance(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // PassiveAb.pas:991
        public override Aow.Rtl.AnsiString GetSourceName(Aow.Units.AoWE.TAbilityOwner A1)
        {
            Aow.Rtl.AnsiString Result = default;
            Aow.Units.AoWE.TDurationAbilityData Data = default;
            var call__27 = A1.GetAbilityData(FAbilityID);
            Data = (Aow.Units.AoWE.TDurationAbilityData)call__27;
            if ((Data != null) && (Data.FSource == 1))
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathMasteryRStr);
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
    public partial class THolyFear : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:816
        public THolyFear(TECustomNode owner) : base(owner)
        {
            var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyFearRStr);
            FAbilityName = call__28;
            FAbilityID = 53;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmDay;
        }

        // PassiveAb.pas:833
        public override Aow.Rtl.AnsiString GetSourceName(Aow.Units.AoWE.TAbilityOwner A1)
        {
            Aow.Rtl.AnsiString Result = default;
            // { Original assumes the ability data exists. }
            var durationAbilityData = (Aow.Units.AoWE.TDurationAbilityData)A1.GetAbilityData(FAbilityID);
            if (durationAbilityData.FSource == 1)
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LifeMasteryRStr);
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
    public partial class TPanickedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:874
        public TPanickedAbility(TECustomNode owner) : base(owner)
        {
            var call__30 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PanickedRStr);
            FAbilityName = call__30;
            FAbilityID = 108;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FDurationMode = Aow.Units.AoWE.TDurationMode.dmEntireCombat;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:885
        public override bool Expand(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Expand(A1);
            if (A1 is Aow.Units.AoWE.TAbstractUnit)
            {
                ((Aow.Units.AoWE.TAbstractUnit)A1).Changed();
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TWallClimbingAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:922
        public TWallClimbingAbility(TECustomNode owner) : base(owner)
        {
            var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WallClimbingRStr);
            FAbilityName = call__31;
            FAbilityID = 36;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
        }

        // PassiveAb.pas:931
        public override TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> Result = default;
            Result = TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing, Aow.Units.AoWE.TWallCombatFeaturesElement.wcParticipate);
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TPassWallAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:937
        public TPassWallAbility(TECustomNode owner) : base(owner)
        {
            var call__32 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PassWallRStr);
            FAbilityName = call__32;
            FAbilityID = 66;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem);
        }

        // PassiveAb.pas:946
        public override TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> Result = default;
            Result = TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing, Aow.Units.AoWE.TWallCombatFeaturesElement.wcParticipate);
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TResurrectedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:892
        public TResurrectedAbility(TECustomNode owner) : base(owner)
        {
            var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResurrectedRStr);
            FAbilityName = call__33;
            FAbilityID = 128;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TAnimatedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:902
        public TAnimatedAbility(TECustomNode owner) : base(owner)
        {
            var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimatedRStr);
            FAbilityName = call__34;
            FAbilityID = 129;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TCombatResurrectedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:912
        public TCombatResurrectedAbility(TECustomNode owner) : base(owner)
        {
            var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResurrectedRStr);
            FAbilityName = call__35;
            FAbilityID = 130;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }
    }

    // (maximum level, level costs.)
    public partial class TMarksmanshipAbility : Aow.Units.AoWE.TMultiLevelAbility
    {
        // PassiveAb.pas:1384
        public TMarksmanshipAbility(TECustomNode owner) : base(owner)
        {
            var call__36 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr);
            FAbilityName = call__36;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnhancement;
            FAbilityID = 32;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
            FMaxLevel = 4;
            FLevelCosts[1] = 5;
            FLevelCosts[2] = 5;
            FLevelCosts[3] = 5;
            FLevelCosts[4] = 5;
        }

        // PassiveAb.pas:1398
        public override Aow.Rtl.AnsiString GetLevelName(int A1)
        {
            Aow.Rtl.AnsiString Result = default;
            switch (A1)
            {
                case 0:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr);
                    break;
                case 1:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr) + ((AnsiString)" I");
                    break;
                case 2:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr) + ((AnsiString)" II");
                    break;
                case 3:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr) + ((AnsiString)" III");
                    break;
                case 4:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MarksmanshipRStr) + ((AnsiString)" IV");
                    break;
            }

            return Result;
        }
    }

    // (FMoveTypes.)
    public partial class TMovementAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        public TSet<Aow.Units.AoWE.TMoveType> FMoveTypes;
        // PassiveAb.pas:1409
        public TMovementAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astRingItem);
            FAbilityType = Aow.Units.AoWE.TAbilityType.atMovement;
            FSkillPointCost = 0;
        }

        // PassiveAb.pas:1417
        public override TSet<Aow.Units.AoWE.TMoveType> GetMoveTypes(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TMoveType> Result = default;
            Result = FMoveTypes;
            return Result;
        }

        // PassiveAb.pas:1422
        public override TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> GetWallCombatFeatures(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TWallCombatFeaturesElement> Result = default;
            if (FAbilityID == 1)
            {
                Result = TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of(Aow.Units.AoWE.TWallCombatFeaturesElement.wcWallPassing, Aow.Units.AoWE.TWallCombatFeaturesElement.wcParticipate);
            }
            else
            {
                Result = TSet<Aow.Units.AoWE.TWallCombatFeaturesElement>.Of();
            }

            return Result;
        }
    }

    // (FImmunityTypes.)
    public partial class TImmunityAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        public TSet<Aow.Units.AoWE.TDamageType> FImmunityTypes;
        // PassiveAb.pas:1428
        public TImmunityAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem);
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnhancement;
        }

        // PassiveAb.pas:1440
        public override TSet<Aow.Units.AoWE.TMoveType> GetMoveTypes(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TMoveType> Result = default;
            if (FImmunityTypes.Contains(Aow.Units.AoWE.TDamageType.dtFire))
            {
                Result = TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtLavaCrossing);
            }
            else
            {
                Result = TSet<Aow.Units.AoWE.TMoveType>.Of();
            }

            return Result;
        }

        // PassiveAb.pas:1435
        public override TSet<Aow.Units.AoWE.TDamageType> GetImmunityTypes(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TDamageType> Result = default;
            Result = FImmunityTypes;
            return Result;
        }
    }

    // (FProtectionTypes.)
    public partial class TProtectionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        public TSet<Aow.Units.AoWE.TDamageType> FProtectionTypes;
        // PassiveAb.pas:1446
        public TProtectionAbility(TECustomNode owner) : base(owner)
        {
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor);
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnhancement;
        }

        // PassiveAb.pas:1453
        public override TSet<Aow.Units.AoWE.TDamageType> GetProtectionTypes(Aow.Units.AoWE.TAbilityOwner A1)
        {
            TSet<Aow.Units.AoWE.TDamageType> Result = default;
            Result = FProtectionTypes;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TEntangledAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:620
        public TEntangledAbility(TECustomNode owner) : base(owner)
        {
            var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EntangledRStr);
            FAbilityName = call__37;
            FAbilityID = 94;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:638
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // PassiveAb.pas:628
        public override Aow.Units.AoWE.TDurationMode GetDurationMode()
        {
            Aow.Units.AoWE.TDurationMode Result = default;
            Result = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
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
        public override Aow.Units.AoWE.TUnitEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFrozenAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:649
        public TFrozenAbility(TECustomNode owner) : base(owner)
        {
            var call__38 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FrozenRStr);
            FAbilityName = call__38;
            FAbilityID = 95;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atEnchantment;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:674
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-2);
            return Result;
        }

        // PassiveAb.pas:658
        public override Aow.Units.AoWE.TDurationMode GetDurationMode()
        {
            Aow.Units.AoWE.TDurationMode Result = default;
            Result = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
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
        public override Aow.Units.AoWE.TUnitEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TTurnedUndeadAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:679
        public TTurnedUndeadAbility(TECustomNode owner) : base(owner)
        {
            var call__39 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TurnedUndeadRStr);
            FAbilityName = call__39;
            FAbilityID = 34;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:693
        public override Aow.Units.AoWE.TDurationMode GetDurationMode()
        {
            Aow.Units.AoWE.TDurationMode Result = default;
            Result = Aow.Units.AoWE.TDurationMode.dmCombatTurn;
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
        public override Aow.Units.AoWE.TUnitEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
            Result.FDispelResistance = 4;
            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THasteAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1021
        public THasteAbility(TECustomNode owner) : base(owner)
        {
            var call__40 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HasteRStr);
            FAbilityName = call__40;
            FAbilityID = 152;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TStoneSkinAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1029
        public TStoneSkinAbility(TECustomNode owner) : base(owner)
        {
            var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StoneSkinRStr);
            FAbilityName = call__41;
            FAbilityID = 153;
        }

        // PassiveAb.pas:1048
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }

        // PassiveAb.pas:1037
        public override Aow.Rtl.AnsiString GetSourceName(Aow.Units.AoWE.TAbilityOwner A1)
        {
            Aow.Rtl.AnsiString Result = default;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (Aow.Units.AoWE.TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EarthMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TEnchantedWeaponAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1073
        public TEnchantedWeaponAbility(TECustomNode owner) : base(owner)
        {
            var call__43 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantedWeaponRStr);
            FAbilityName = call__43;
            FAbilityID = 154;
        }

        // PassiveAb.pas:1081
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1087
        public override sbyte GetDamage(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1093
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            Result = ((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel() + 225;
            if (A1 is Aow.Units.AoWE.TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TSummonedAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1100
        public TSummonedAbility(TECustomNode owner) : base(owner)
        {
            var call__44 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SummonedRStr);
            FAbilityName = call__44;
            FAbilityID = 155;
        }

        // PassiveAb.pas:1122
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            Result = base.Remove(A1);
            if (Result)
            {
                var unit = (Aow.Units.AoWE.TAbstractUnit)A1;
                unit.Killed();
                unit.SetOwner(null);
            }

            return Result;
        }

        // PassiveAb.pas:1134
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            switch (((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel())
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
        public int GetDefaultManaUpkeep(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            int Result = default;
            if (AUnit is Aow.Units.AoWE.TUnit)
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
        public int GetDefaultDispelResistance(Aow.Units.AoWE.TAbstractUnit AUnit)
        {
            int Result = default;
            if (AUnit is Aow.Units.AoWE.TUnit)
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
            var call__46 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(A1);
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
    public partial class TFuryAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1160
        public TFuryAbility(TECustomNode owner) : base(owner)
        {
            var call__47 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FuryRStr);
            FAbilityName = call__47;
            FAbilityID = 156;
        }

        // PassiveAb.pas:1168
        public override sbyte GetAttack(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 2;
            return Result;
        }

        // PassiveAb.pas:1174
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)-1);
            return Result;
        }

        // PassiveAb.pas:1180
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            Result = ((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is Aow.Units.AoWE.TLeader)
            {
                Result -= 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFreeMovementAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1187
        public TFreeMovementAbility(TECustomNode owner) : base(owner)
        {
            var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FreeMovementRStr);
            FAbilityName = call__48;
            FAbilityID = 157;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class THolyChampionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1195
        public THolyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyChampionRStr);
            FAbilityName = call__49;
            FAbilityID = 160;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TUnholyChampionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1203
        public TUnholyChampionEnchantment(TECustomNode owner) : base(owner)
        {
            var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyChampionRStr);
            FAbilityName = call__50;
            FAbilityID = 161;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TConcealmentEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1287
        public TConcealmentEnchantment(TECustomNode owner) : base(owner)
        {
            var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealmentRStr);
            FAbilityName = call__51;
            FAbilityID = 162;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TFireAuraEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1294
        public TFireAuraEnchantment(TECustomNode owner) : base(owner)
        {
            var call__52 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireAuraRStr);
            FAbilityName = call__52;
            FAbilityID = 163;
        }

        // PassiveAb.pas:1301
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
            if (Result && (A1.Owner is Aow.Units.AoWE.TArmy))
            {
                ((Aow.Units.AoWE.TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((Aow.Units.AoWE.TArmy)A1.Owner).ValidateTerrain(unchecked((sbyte)mapField.TerrainType), unchecked((sbyte)mapField.OverlayType));
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TWaterWalkingEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1315
        public TWaterWalkingEnchantment(TECustomNode owner) : base(owner)
        {
            var call__54 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WaterWalkingRStr);
            FAbilityName = call__54;
            FAbilityID = 164;
        }

        // PassiveAb.pas:1322
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
            if (Result && (A1.Owner is Aow.Units.AoWE.TArmy))
            {
                ((Aow.Units.AoWE.TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((Aow.Units.AoWE.TArmy)A1.Owner).ValidateTerrain(unchecked((sbyte)mapField.TerrainType), unchecked((sbyte)mapField.OverlayType));
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TWindWalkingEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1336
        public TWindWalkingEnchantment(TECustomNode owner) : base(owner)
        {
            var call__56 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WindWalkingRStr);
            FAbilityName = call__56;
            FAbilityID = 165;
        }

        // PassiveAb.pas:1343
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
            if (Result && (A1.Owner is Aow.Units.AoWE.TArmy))
            {
                ((Aow.Units.AoWE.TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((Aow.Units.AoWE.TArmy)A1.Owner).ValidateTerrain(unchecked((sbyte)mapField.TerrainType), unchecked((sbyte)mapField.OverlayType));
                }
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TLiquidFormEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1357
        public TLiquidFormEnchantment(TECustomNode owner) : base(owner)
        {
            var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LiquidFormRStr);
            FAbilityName = call__58;
            FAbilityID = 166;
        }

        // PassiveAb.pas:1364
        public override bool Remove(Aow.Units.AoWE.TAbilityOwner A1)
        {
            bool Result = default;
            sbyte X = default;
            sbyte Y = default;
            sbyte L = default;
            Result = base.Remove(A1);
            if (Result && (A1.Owner is Aow.Units.AoWE.TArmy))
            {
                ((Aow.Units.AoWE.TAbstractUnit)A1).GetLocation(ref X, ref Y, ref L);
                if (X != -1)
                {
                    var mapField = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(X, Y, L);
                    ((Aow.Units.AoWE.TArmy)A1.Owner).ValidateTerrain(unchecked((sbyte)mapField.TerrainType), unchecked((sbyte)mapField.OverlayType));
                }
            }

            return Result;
        }

        // PassiveAb.pas:1378
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            Result = ((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel() + 300;
            if (A1 is Aow.Units.AoWE.TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TBlessedEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1211
        public TBlessedEnchantment(TECustomNode owner) : base(owner)
        {
            var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BlessedRStr);
            FAbilityName = call__60;
            FAbilityID = 158;
        }

        // PassiveAb.pas:1219
        public override sbyte GetDefense(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1225
        public override sbyte GetResistance(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1231
        public override Aow.Rtl.AnsiString GetSourceName(Aow.Units.AoWE.TAbilityOwner A1)
        {
            Aow.Rtl.AnsiString Result = default;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (Aow.Units.AoWE.TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LifeMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }

        // PassiveAb.pas:1242
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            Result = ((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is Aow.Units.AoWE.TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // (name, ID, selection types, ability type.)
    public partial class TDarkGiftEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1249
        public TDarkGiftEnchantment(TECustomNode owner) : base(owner)
        {
            var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DarkGiftRStr);
            FAbilityName = call__62;
            FAbilityID = 169;
        }

        // PassiveAb.pas:1256
        public override sbyte GetDamage(Aow.Units.AoWE.TAbilityOwner A1)
        {
            sbyte Result = default;
            Result = 1;
            return Result;
        }

        // PassiveAb.pas:1261
        public override int GetAIUpkeepPriority(Aow.Units.AoWE.TAbilityOwner A1)
        {
            int Result = default;
            Result = ((Aow.Units.AoWE.TAbstractUnit)A1).GetUnitLevel() + 200;
            if (A1 is Aow.Units.AoWE.TLeader)
            {
                Result += 50;
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TFireProtectionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1054
        public TFireProtectionEnchantment(TECustomNode owner) : base(owner)
        {
            var call__63 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireProtectionRStr);
            FAbilityName = call__63;
            FAbilityID = 167;
        }

        // PassiveAb.pas:1062
        public override Aow.Rtl.AnsiString GetSourceName(Aow.Units.AoWE.TAbilityOwner A1)
        {
            Aow.Rtl.AnsiString Result = default;
            // { Original assumes the ability data and enchantment exist. }
            var unitEnchantmentAbilityData = (Aow.Units.AoWE.TUnitEnchantmentAbilityData)A1.GetAbilityData(FAbilityID);
            if (unitEnchantmentAbilityData.FEnchantment.FSource == -2)
            {
                Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireMasteryRStr);
            }
            else
            {
                Result = base.GetSourceName(A1);
            }

            return Result;
        }
    }

    // inherited name, ability ID, selection types, ability type
    public partial class TSlowEnchantmentAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:601
        public TSlowEnchantmentAbility(TECustomNode owner) : base(owner)
        {
            var call__65 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SlowRStr);
            FAbilityName = call__65;
            FAbilityID = 132;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:615
        public override void CombatDone(Aow.Units.AoWE.TCombatObject A1)
        {
            var call__66 = A1.GetAbilityOwner();
            call__66.RemoveAbility(FAbilityID);
        }

        // PassiveAb.pas:609
        public override Aow.Units.AoWE.TUnitEnchantment CreateEnchantment()
        {
            Aow.Units.AoWE.TUnitEnchantment Result = default;
            Result = base.CreateEnchantment();
            Result.FDispelResistance = 3;
            return Result;
        }
    }

    public static partial class PassiveAb
    {
        // PassiveAb.pas:1504
        public static void RegisterPassiveAbilities(Aow.Units.AoWE.TAbilityControl Control)
        {
            var call__67 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireImmunityRStr);
            var call__68 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(7, call__67, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire));
            Control.RegisterAbility(call__68);
            var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ColdImmunityRStr);
            var call__70 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(8, call__69, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtCold));
            Control.RegisterAbility(call__70);
            var call__71 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningImmunityRStr);
            var call__72 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(9, call__71, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtLightning));
            Control.RegisterAbility(call__72);
            var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonImmunityRStr);
            var call__74 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(10, call__73, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPoison));
            Control.RegisterAbility(call__74);
            var call__75 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathImmunityRStr);
            var call__76 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(11, call__75, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath));
            Control.RegisterAbility(call__76);
            var call__77 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyImmunityRStr);
            var call__78 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(12, call__77, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly));
            Control.RegisterAbility(call__78);
            var call__79 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PhysicalImmunityRStr);
            var call__80 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(13, call__79, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical));
            Control.RegisterAbility(call__80);
            var call__81 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MagicImmunityRStr);
            var call__82 = Aow.Units.PassiveAb.PassiveAb.CreateImmunityAbility(6, call__81, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtMagic));
            Control.RegisterAbility(call__82);
            var call__83 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FireProtectionRStr);
            var call__84 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(71, call__83, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtFire), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__84);
            var call__85 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ColdProtectionRStr);
            var call__86 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(76, call__85, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtCold), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__86);
            var call__87 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LightningProtectionRStr);
            var call__88 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(74, call__87, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtLightning), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__88);
            var call__89 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PoisonProtectionRStr);
            var call__90 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(73, call__89, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPoison), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__90);
            var call__91 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeathProtectionRStr);
            var call__92 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(70, call__91, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__92);
            var call__93 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyProtectionRStr);
            var call__94 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(72, call__93, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtHoly), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__94);
            var call__95 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PhysicalProtectionRStr);
            var call__96 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(77, call__95, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__96);
            var call__97 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MagicProtectionRStr);
            var call__98 = Aow.Units.PassiveAb.PassiveAb.CreateProtectionAbility(75, call__97, TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtMagic), TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__98);
            var call__99 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WalkingRStr);
            var call__100 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(0, call__99, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtWalking));
            Control.RegisterAbility(call__100);
            var call__101 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FlyingRStr);
            var call__102 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(1, call__101, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtFlying));
            Control.RegisterAbility(call__102);
            var call__103 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SwimmingRStr);
            var call__104 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(2, call__103, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming));
            Control.RegisterAbility(call__104);
            var call__105 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ForestryRStr);
            var call__106 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(3, call__105, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtForestry));
            Control.RegisterAbility(call__106);
            var call__107 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CaveCrawlingRStr);
            var call__108 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(4, call__107, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtCaveCrawling));
            Control.RegisterAbility(call__108);
            var call__109 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MountaineeringRStr);
            var call__110 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(5, call__109, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtMountaineering));
            Control.RegisterAbility(call__110);
            var call__111 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SailingRStr);
            var call__112 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(23, call__111, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtSwimming));
            Control.RegisterAbility(call__112);
            var call__113 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TunnelingRStr);
            var call__114 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(42, call__113, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtTunneling));
            Control.RegisterAbility(call__114);
            var call__115 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FloatingRStr);
            var call__116 = Aow.Units.PassiveAb.PassiveAb.CreateMovementAbility(59, call__115, TSet<Aow.Units.AoWE.TMoveType>.Of(Aow.Units.AoWE.TMoveType.mtFlying));
            Control.RegisterAbility(call__116);
            var call__117 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TrailOfDarknessRStr);
            var call__118 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(26, call__117, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__118);
            var call__119 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NightVisionRStr);
            var call__120 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(39, call__119, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__120);
            var call__121 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IgnitionRStr);
            var call__122 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(43, call__121, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__122);
            var call__123 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RegenerationRStr);
            var call__124 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(49, call__123, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__124);
            var call__125 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CauseFearRStr);
            var call__126 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(51, call__125, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__126);
            var call__127 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InvisibilityRStr);
            var call__128 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(54, call__127, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__128);
            var call__129 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DragonRStr);
            var call__130 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(63, call__129, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit));
            Control.RegisterAbility(call__130);
            var call__131 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.BardsSkillsRStr);
            var call__132 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(37, call__131, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__132);
            var call__133 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TrueVisionRStr);
            var call__134 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(41, call__133, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__134);
            var call__135 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FearlessRStr);
            var call__136 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(67, call__135, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__136);
            var call__137 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PathOfDecayRStr);
            var call__138 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(68, call__137, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__138);
            var call__139 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PathOfLifeRStr);
            var call__140 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(69, call__139, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__140);
            var call__141 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.PathOfFrostRStr);
            var call__142 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(109, call__141, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__142);
            var call__143 = new Aow.Units.PassiveAb.TVisionAbility(null);
            Control.RegisterAbility(call__143);
            var call__144 = new Aow.Units.PassiveAb.THolyChampionAbility(null);
            Control.RegisterAbility(call__144);
            var call__145 = new Aow.Units.PassiveAb.TUnholyChampionAbility(null);
            Control.RegisterAbility(call__145);
            var call__146 = new Aow.Units.PassiveAb.TWallClimbingAbility(null);
            Control.RegisterAbility(call__146);
            var call__147 = new Aow.Units.PassiveAb.TPassWallAbility(null);
            Control.RegisterAbility(call__147);
            var call__148 = new Aow.Units.PassiveAb.TCursedAbility(null);
            Control.RegisterAbility(call__148);
            var call__149 = new Aow.Units.PassiveAb.TResurrectedAbility(null);
            Control.RegisterAbility(call__149);
            var call__150 = new Aow.Units.PassiveAb.TAnimatedAbility(null);
            Control.RegisterAbility(call__150);
            var call__151 = new Aow.Units.PassiveAb.TCombatResurrectedAbility(null);
            Control.RegisterAbility(call__151);
            var call__152 = new Aow.Units.PassiveAb.TMarksmanshipAbility(null);
            Control.RegisterAbility(call__152);
            var call__153 = new Aow.Units.PassiveAb.TSlowEnchantmentAbility(null);
            Control.RegisterAbility(call__153);
            var call__154 = new Aow.Units.PassiveAb.TWebbedAbility(null);
            Control.RegisterAbility(call__154);
            var call__155 = new Aow.Units.PassiveAb.TVertigoAbility(null);
            Control.RegisterAbility(call__155);
            var call__156 = new Aow.Units.PassiveAb.TPoisonedAbility(null);
            Control.RegisterAbility(call__156);
            var call__157 = new Aow.Units.PassiveAb.TEntangledAbility(null);
            Control.RegisterAbility(call__157);
            var call__158 = new Aow.Units.PassiveAb.TFrozenAbility(null);
            Control.RegisterAbility(call__158);
            var call__159 = new Aow.Units.PassiveAb.TStunnedAbility(null);
            Control.RegisterAbility(call__159);
            var call__160 = new Aow.Units.PassiveAb.TNaturesBlessingAbility(null);
            Control.RegisterAbility(call__160);
            var call__161 = new Aow.Units.PassiveAb.TBloodlustAbility(null);
            Control.RegisterAbility(call__161);
            var call__162 = new Aow.Units.PassiveAb.TTurnedUndeadAbility(null);
            Control.RegisterAbility(call__162);
            var call__163 = new Aow.Units.PassiveAb.THolyFear(null);
            Control.RegisterAbility(call__163);
            var call__164 = new Aow.Units.PassiveAb.THasteAbility(null);
            Control.RegisterAbility(call__164);
            var call__165 = new Aow.Units.PassiveAb.TStoneSkinAbility(null);
            Control.RegisterAbility(call__165);
            var call__166 = new Aow.Units.PassiveAb.TEnchantedWeaponAbility(null);
            Control.RegisterAbility(call__166);
            var call__167 = new Aow.Units.PassiveAb.TSummonedAbility(null);
            Control.RegisterAbility(call__167);
            var call__168 = new Aow.Units.PassiveAb.TFuryAbility(null);
            Control.RegisterAbility(call__168);
            var call__169 = new Aow.Units.PassiveAb.TFreeMovementAbility(null);
            Control.RegisterAbility(call__169);
            var call__170 = new Aow.Units.PassiveAb.THolyChampionEnchantment(null);
            Control.RegisterAbility(call__170);
            var call__171 = new Aow.Units.PassiveAb.TUnholyChampionEnchantment(null);
            Control.RegisterAbility(call__171);
            var call__172 = new Aow.Units.PassiveAb.TConcealmentEnchantment(null);
            Control.RegisterAbility(call__172);
            var call__173 = new Aow.Units.PassiveAb.TFireAuraEnchantment(null);
            Control.RegisterAbility(call__173);
            var call__174 = new Aow.Units.PassiveAb.TWaterWalkingEnchantment(null);
            Control.RegisterAbility(call__174);
            var call__175 = new Aow.Units.PassiveAb.TWindWalkingEnchantment(null);
            Control.RegisterAbility(call__175);
            var call__176 = new Aow.Units.PassiveAb.TLiquidFormEnchantment(null);
            Control.RegisterAbility(call__176);
            var call__177 = new Aow.Units.PassiveAb.TBlessedEnchantment(null);
            Control.RegisterAbility(call__177);
            var call__178 = new Aow.Units.AoWE.TCosmeticSurgeryAbility(null);
            Control.RegisterAbility(call__178);
            var call__179 = new Aow.Units.PassiveAb.TBurningAbility(null);
            Control.RegisterAbility(call__179);
            var call__180 = new Aow.Units.PassiveAb.TFireProtectionEnchantment(null);
            Control.RegisterAbility(call__180);
            var call__181 = new Aow.Units.PassiveAb.TPanickedAbility(null);
            Control.RegisterAbility(call__181);
            var call__182 = new Aow.Units.PassiveAb.TDecayAbility(null);
            Control.RegisterAbility(call__182);
            var call__183 = new Aow.Units.PassiveAb.THighPrayerBlessingAbility(null);
            Control.RegisterAbility(call__183);
            var call__184 = new Aow.Units.PassiveAb.TDarkGiftEnchantment(null);
            Control.RegisterAbility(call__184);
            var call__185 = new Aow.Units.PassiveAb.TCrusaderAbility(null);
            Control.RegisterAbility(call__185);
            var call__186 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SteppeConcealmentRStr);
            var call__187 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(138, call__186, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__187);
            var call__188 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UndergroundConcealmentRStr);
            var call__189 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(139, call__188, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__189);
            var call__190 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GrassConcealmentRStr);
            var call__191 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(140, call__190, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__191);
            var call__192 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.SnowConcealmentRStr);
            var call__193 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(141, call__192, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__193);
            var call__194 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DesertConcealmentRStr);
            var call__195 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(142, call__194, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__195);
            var call__196 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WastelandConcealmentRStr);
            var call__197 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(143, call__196, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__197);
            var call__198 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WaterConcealmentRStr);
            var call__199 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(144, call__198, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem));
            Control.RegisterAbility(call__199);
            var call__200 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealmentRStr);
            var call__201 = Aow.Units.AoWE.AoWE.CreateEnhancementAbility(145, call__200, TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astHeadItem, Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem, Aow.Units.AoWE.TAbilitySelectionType.astDefenseItem, Aow.Units.AoWE.TAbilitySelectionType.astRingItem, Aow.Units.AoWE.TAbilitySelectionType.astEditor));
            Control.RegisterAbility(call__201);
        }

        // PassiveAb.pas:1597
        public static void UnregisterPassiveAbilities(Aow.Units.AoWE.TAbilityControl Control)
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
        public static Aow.Units.PassiveAb.TMovementAbility CreateMovementAbility(int ID, Aow.Rtl.AnsiString Name, TSet<Aow.Units.AoWE.TMoveType> MoveTypes)
        {
            Aow.Units.PassiveAb.TMovementAbility Result = default;
            Aow.Units.PassiveAb.TMovementAbility Ability = default;
            Ability = new Aow.Units.PassiveAb.TMovementAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FMoveTypes = MoveTypes;
            if ((MoveTypes.Contains(Aow.Units.AoWE.TMoveType.mtMountaineering) || MoveTypes.Contains(Aow.Units.AoWE.TMoveType.mtForestry)) || MoveTypes.Contains(Aow.Units.AoWE.TMoveType.mtCaveCrawling))
            {
                Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astEditor);
            }

            Result = Ability;
            return Result;
        }

        // PassiveAb.pas:1472
        public static Aow.Units.PassiveAb.TImmunityAbility CreateImmunityAbility(int ID, Aow.Rtl.AnsiString Name, TSet<Aow.Units.AoWE.TDamageType> DamageTypes)
        {
            Aow.Units.PassiveAb.TImmunityAbility Result = default;
            Aow.Units.PassiveAb.TImmunityAbility Ability = default;
            Ability = new Aow.Units.PassiveAb.TImmunityAbility(null);
            Ability.FAbilityID = ID;
            Ability.FAbilityName = Name;
            Ability.FImmunityTypes = DamageTypes;
            if ((TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical, Aow.Units.AoWE.TDamageType.dtDeath, Aow.Units.AoWE.TDamageType.dtHoly, Aow.Units.AoWE.TDamageType.dtMagic) & DamageTypes) == TSet<Aow.Units.AoWE.TDamageType>.Of())
            {
                Ability.FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astRingItem);
            }
            else
            {
                if ((TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtDeath, Aow.Units.AoWE.TDamageType.dtHoly, Aow.Units.AoWE.TDamageType.dtMagic) & DamageTypes) == TSet<Aow.Units.AoWE.TDamageType>.Of())
                {
                    Ability.FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit, Aow.Units.AoWE.TAbilitySelectionType.astRingItem);
                }
                else
                {
                    Ability.FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astUnit);
                }
            }

            if ((TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtPhysical, Aow.Units.AoWE.TDamageType.dtMagic) & DamageTypes) == TSet<Aow.Units.AoWE.TDamageType>.Of())
            {
                Ability.FSelectionTypes = Ability.FSelectionTypes + TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of(Aow.Units.AoWE.TAbilitySelectionType.astTorsoItem);
            }

            Result = Ability;
            return Result;
        }

        // PassiveAb.pas:1491
        public static Aow.Units.PassiveAb.TProtectionAbility CreateProtectionAbility(int ID, Aow.Rtl.AnsiString Name, TSet<Aow.Units.AoWE.TDamageType> DamageTypes, TSet<Aow.Units.AoWE.TAbilitySelectionType> SelectionTypes)
        {
            Aow.Units.PassiveAb.TProtectionAbility Result = default;
            Aow.Units.PassiveAb.TProtectionAbility Ability = default;
            Ability = new Aow.Units.PassiveAb.TProtectionAbility(null);
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
