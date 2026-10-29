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
    // { TVisionAbility (instance size 48, VMT 557B502C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type;
//     $28 maximum level, $2C level costs. }
    public partial class TVisionAbility : Aow.Units.AoWE.TMultiLevelAbility
    {
        // PassiveAb.pas:461
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:65
        public static new Aow.Units.PassiveAb.TVisionAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TVisionAbility o = new Aow.Units.PassiveAb.TVisionAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THolyChampionAbility (instance size 40, VMT 557B51B8):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class THolyChampionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        // PassiveAb.pas:484
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:73
        public static new Aow.Units.PassiveAb.THolyChampionAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.THolyChampionAbility o = new Aow.Units.PassiveAb.THolyChampionAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TUnholyChampionAbility (instance size 40, VMT 557B534C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TUnholyChampionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        // PassiveAb.pas:499
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:81
        public static new Aow.Units.PassiveAb.TUnholyChampionAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TUnholyChampionAbility o = new Aow.Units.PassiveAb.TUnholyChampionAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TNaturesBlessingAbility (instance size 44, VMT 557B54E4):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type; $28 duration mode. }
    public partial class TNaturesBlessingAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:514
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:89
        public static new Aow.Units.PassiveAb.TNaturesBlessingAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TNaturesBlessingAbility o = new Aow.Units.PassiveAb.TNaturesBlessingAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THighPrayerBlessingAbility (instance size 44; $08 name, $0C ID, $20 selection types, $24 ability type, $28 duration mode; VMT 557B5684) }
    public partial class THighPrayerBlessingAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:1267
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:99
        public static new Aow.Units.PassiveAb.THighPrayerBlessingAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.THighPrayerBlessingAbility o = new Aow.Units.PassiveAb.THighPrayerBlessingAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TCrusaderAbility (instance size 44, VMT 557B582C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type,
//     $28 duration mode; duration data $0C countdown, $0D player index. }
    public partial class TCrusaderAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:703
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:109
        public static new Aow.Units.PassiveAb.TCrusaderAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TCrusaderAbility o = new Aow.Units.PassiveAb.TCrusaderAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TBloodlustAbility (instance size 44, VMT 557B59C0):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type; $28 duration mode. }
    public partial class TBloodlustAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:546
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:119
        public static new Aow.Units.PassiveAb.TBloodlustAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TBloodlustAbility o = new Aow.Units.PassiveAb.TBloodlustAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TPoisonedAbility (instance size 44, VMT 557B5B58):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type; $28 duration mode. }
    public partial class TPoisonedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:571
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:129
        public static new Aow.Units.PassiveAb.TPoisonedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TPoisonedAbility o = new Aow.Units.PassiveAb.TPoisonedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TDecayAbility (instance size 44, VMT 557B5CEC):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TDecayAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:844
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:140
        public static new Aow.Units.PassiveAb.TDecayAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TDecayAbility o = new Aow.Units.PassiveAb.TDecayAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TWebbedAbility (instance size 44, VMT 557B5E7C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type,
//     $28 duration mode. }
    public partial class TWebbedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:736
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:150
        public static new Aow.Units.PassiveAb.TWebbedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TWebbedAbility o = new Aow.Units.PassiveAb.TWebbedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TStunnedAbility (instance size 44, VMT 557B600C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TStunnedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:793
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:159
        public static new Aow.Units.PassiveAb.TStunnedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TStunnedAbility o = new Aow.Units.PassiveAb.TStunnedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TBurningAbility (instance size 44, VMT 557B619C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TBurningAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:756
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:168
        public static new Aow.Units.PassiveAb.TBurningAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TBurningAbility o = new Aow.Units.PassiveAb.TBurningAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TVertigoAbility (instance size 44, VMT 557B632C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TVertigoAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:952
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:177
        public static new Aow.Units.PassiveAb.TVertigoAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TVertigoAbility o = new Aow.Units.PassiveAb.TVertigoAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TCursedAbility (instance size 44, VMT 557B64BC):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TCursedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:981
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:187
        public static new Aow.Units.PassiveAb.TCursedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TCursedAbility o = new Aow.Units.PassiveAb.TCursedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THolyFear (instance size 44, VMT 557B664C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class THolyFear : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:816
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:198
        public static new Aow.Units.PassiveAb.THolyFear Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.THolyFear o = new Aow.Units.PassiveAb.THolyFear();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TPanickedAbility (instance size 44, VMT 557B67D4):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type, $28 duration mode. }
    public partial class TPanickedAbility : Aow.Units.AoWE.TDurationAbility
    {
        // PassiveAb.pas:874
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:207
        public static new Aow.Units.PassiveAb.TPanickedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TPanickedAbility o = new Aow.Units.PassiveAb.TPanickedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TWallClimbingAbility (instance size 40, VMT 557B6968):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TWallClimbingAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:922
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:215
        public static new Aow.Units.PassiveAb.TWallClimbingAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TWallClimbingAbility o = new Aow.Units.PassiveAb.TWallClimbingAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TPassWallAbility (instance size 40, VMT 557B6AFC):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TPassWallAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:937
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:223
        public static new Aow.Units.PassiveAb.TPassWallAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TPassWallAbility o = new Aow.Units.PassiveAb.TPassWallAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TResurrectedAbility (instance size 40, VMT 557B6C88):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TResurrectedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:892
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__33 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResurrectedRStr);
            FAbilityName = call__33;
            FAbilityID = 128;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:231
        public static new Aow.Units.PassiveAb.TResurrectedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TResurrectedAbility o = new Aow.Units.PassiveAb.TResurrectedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TAnimatedAbility (instance size 40, VMT 557B6E18):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TAnimatedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:902
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AnimatedRStr);
            FAbilityName = call__34;
            FAbilityID = 129;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:238
        public static new Aow.Units.PassiveAb.TAnimatedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TAnimatedAbility o = new Aow.Units.PassiveAb.TAnimatedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TCombatResurrectedAbility (instance size 40, VMT 557B6FA4):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TCombatResurrectedAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        // PassiveAb.pas:912
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__35 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ResurrectedRStr);
            FAbilityName = call__35;
            FAbilityID = 130;
            FAbilityType = Aow.Units.AoWE.TAbilityType.atHandicap;
            FSelectionTypes = TSet<Aow.Units.AoWE.TAbilitySelectionType>.Of();
        }

        // PassiveAb.pas:245
        public static new Aow.Units.PassiveAb.TCombatResurrectedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TCombatResurrectedAbility o = new Aow.Units.PassiveAb.TCombatResurrectedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TMarksmanshipAbility (instance size 48; $28 maximum level, $2C level costs; VMT 557B7144) }
    public partial class TMarksmanshipAbility : Aow.Units.AoWE.TMultiLevelAbility
    {
        // PassiveAb.pas:1384
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:251
        public static new Aow.Units.PassiveAb.TMarksmanshipAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TMarksmanshipAbility o = new Aow.Units.PassiveAb.TMarksmanshipAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TMovementAbility (instance size 44; $28 FMoveTypes; VMT 557B72DC) }
    public partial class TMovementAbility : Aow.Units.AoWE.TAdjustableAbility
    {
        public TSet<Aow.Units.AoWE.TMoveType> FMoveTypes;
        // { $28 }
        public Aow.Units.FixedArray_byte_3 F_MovementAbility_29;
        // PassiveAb.pas:1409
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:260
        public static new Aow.Units.PassiveAb.TMovementAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TMovementAbility o = new Aow.Units.PassiveAb.TMovementAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TImmunityAbility (instance size 44; $28 FImmunityTypes; VMT 557B7468) }
    public partial class TImmunityAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        public TSet<Aow.Units.AoWE.TDamageType> FImmunityTypes;
        // { $28 }
        public Aow.Units.FixedArray_byte_2 F_ImmunityAbility_2A;
        // PassiveAb.pas:1428
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:270
        public static new Aow.Units.PassiveAb.TImmunityAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TImmunityAbility o = new Aow.Units.PassiveAb.TImmunityAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TProtectionAbility (instance size 44; $28 FProtectionTypes; VMT 557B75F4) }
    public partial class TProtectionAbility : Aow.Units.AoWE.TEnhancementAbility
    {
        public TSet<Aow.Units.AoWE.TDamageType> FProtectionTypes;
        // { $28 }
        public Aow.Units.FixedArray_byte_2 F_ProtectionAbility_2A;
        // PassiveAb.pas:1446
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:280
        public static new Aow.Units.PassiveAb.TProtectionAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TProtectionAbility o = new Aow.Units.PassiveAb.TProtectionAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TEntangledAbility (instance size 40, VMT 557B7784):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TEntangledAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:620
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:288
        public static new Aow.Units.PassiveAb.TEntangledAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TEntangledAbility o = new Aow.Units.PassiveAb.TEntangledAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFrozenAbility (instance size 40, VMT 557B7928):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TFrozenAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:649
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:299
        public static new Aow.Units.PassiveAb.TFrozenAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TFrozenAbility o = new Aow.Units.PassiveAb.TFrozenAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TTurnedUndeadAbility (instance size 40, VMT 557B7AC4):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TTurnedUndeadAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:679
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:310
        public static new Aow.Units.PassiveAb.TTurnedUndeadAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TTurnedUndeadAbility o = new Aow.Units.PassiveAb.TTurnedUndeadAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THasteAbility (instance size 40, VMT 557B7C6C):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class THasteAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1021
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__40 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HasteRStr);
            FAbilityName = call__40;
            FAbilityID = 152;
        }

        // PassiveAb.pas:320
        public static new Aow.Units.PassiveAb.THasteAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.THasteAbility o = new Aow.Units.PassiveAb.THasteAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TStoneSkinAbility (instance size 40, VMT 557B7E08):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TStoneSkinAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1029
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:327
        public static new Aow.Units.PassiveAb.TStoneSkinAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TStoneSkinAbility o = new Aow.Units.PassiveAb.TStoneSkinAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TEnchantedWeaponAbility (instance size 40, VMT 557B7FAC):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TEnchantedWeaponAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1073
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:336
        public static new Aow.Units.PassiveAb.TEnchantedWeaponAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TEnchantedWeaponAbility o = new Aow.Units.PassiveAb.TEnchantedWeaponAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TSummonedAbility (instance size 40, VMT 557B8158):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TSummonedAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1100
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:346
        public static new Aow.Units.PassiveAb.TSummonedAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TSummonedAbility o = new Aow.Units.PassiveAb.TSummonedAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFuryAbility (instance size 40, VMT 557B82F8):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TFuryAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1160
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:358
        public static new Aow.Units.PassiveAb.TFuryAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TFuryAbility o = new Aow.Units.PassiveAb.TFuryAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFreeMovementAbility (instance size 40, VMT 557B8490):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TFreeMovementAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1187
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FreeMovementRStr);
            FAbilityName = call__48;
            FAbilityID = 157;
        }

        // PassiveAb.pas:368
        public static new Aow.Units.PassiveAb.TFreeMovementAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TFreeMovementAbility o = new Aow.Units.PassiveAb.TFreeMovementAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { THolyChampionEnchantment (instance size 40, VMT 557B8638):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class THolyChampionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1195
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.HolyChampionRStr);
            FAbilityName = call__49;
            FAbilityID = 160;
        }

        // PassiveAb.pas:375
        public static new Aow.Units.PassiveAb.THolyChampionEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.THolyChampionEnchantment o = new Aow.Units.PassiveAb.THolyChampionEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TUnholyChampionEnchantment (instance size 40, VMT 557B87E8):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TUnholyChampionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1203
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__50 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UnholyChampionRStr);
            FAbilityName = call__50;
            FAbilityID = 161;
        }

        // PassiveAb.pas:382
        public static new Aow.Units.PassiveAb.TUnholyChampionEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TUnholyChampionEnchantment o = new Aow.Units.PassiveAb.TUnholyChampionEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TConcealmentEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B899C) }
    public partial class TConcealmentEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1287
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__51 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ConcealmentRStr);
            FAbilityName = call__51;
            FAbilityID = 162;
        }

        // PassiveAb.pas:388
        public static new Aow.Units.PassiveAb.TConcealmentEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TConcealmentEnchantment o = new Aow.Units.PassiveAb.TConcealmentEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFireAuraEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B8B48) }
    public partial class TFireAuraEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1294
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:394
        public static new Aow.Units.PassiveAb.TFireAuraEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TFireAuraEnchantment o = new Aow.Units.PassiveAb.TFireAuraEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TWaterWalkingEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B8CF0) }
    public partial class TWaterWalkingEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1315
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:401
        public static new Aow.Units.PassiveAb.TWaterWalkingEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TWaterWalkingEnchantment o = new Aow.Units.PassiveAb.TWaterWalkingEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TWindWalkingEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B8EA0) }
    public partial class TWindWalkingEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1336
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:408
        public static new Aow.Units.PassiveAb.TWindWalkingEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TWindWalkingEnchantment o = new Aow.Units.PassiveAb.TWindWalkingEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TLiquidFormEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B904C) }
    public partial class TLiquidFormEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1357
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:415
        public static new Aow.Units.PassiveAb.TLiquidFormEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TLiquidFormEnchantment o = new Aow.Units.PassiveAb.TLiquidFormEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TBlessedEnchantment (instance size 40, VMT 557B91F8):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TBlessedEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1211
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:424
        public static new Aow.Units.PassiveAb.TBlessedEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TBlessedEnchantment o = new Aow.Units.PassiveAb.TBlessedEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TDarkGiftEnchantment (instance size 40; $08 name, $0C ID, $20 selection types, $24 ability type; VMT 557B939C) }
    public partial class TDarkGiftEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1249
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:434
        public static new Aow.Units.PassiveAb.TDarkGiftEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TDarkGiftEnchantment o = new Aow.Units.PassiveAb.TDarkGiftEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TFireProtectionEnchantment (instance size 40, VMT 557B9544):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TFireProtectionEnchantment : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:1054
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:443
        public static new Aow.Units.PassiveAb.TFireProtectionEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TFireProtectionEnchantment o = new Aow.Units.PassiveAb.TFireProtectionEnchantment();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    // { TSlowEnchantmentAbility (instance size 40, VMT 557B96F8):
//     inherited $08 name, $0C ability ID, $20 selection types, $24 ability type. }
    public partial class TSlowEnchantmentAbility : Aow.Units.AoWE.TUnitEnchantmentAbility
    {
        // PassiveAb.pas:601
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
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

        // PassiveAb.pas:451
        public static new Aow.Units.PassiveAb.TSlowEnchantmentAbility Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.PassiveAb.TSlowEnchantmentAbility o = new Aow.Units.PassiveAb.TSlowEnchantmentAbility();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
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
            var call__143 = Aow.Units.PassiveAb.TVisionAbility.Create(null);
            Control.RegisterAbility(call__143);
            var call__144 = Aow.Units.PassiveAb.THolyChampionAbility.Create(null);
            Control.RegisterAbility(call__144);
            var call__145 = Aow.Units.PassiveAb.TUnholyChampionAbility.Create(null);
            Control.RegisterAbility(call__145);
            var call__146 = Aow.Units.PassiveAb.TWallClimbingAbility.Create(null);
            Control.RegisterAbility(call__146);
            var call__147 = Aow.Units.PassiveAb.TPassWallAbility.Create(null);
            Control.RegisterAbility(call__147);
            var call__148 = Aow.Units.PassiveAb.TCursedAbility.Create(null);
            Control.RegisterAbility(call__148);
            var call__149 = Aow.Units.PassiveAb.TResurrectedAbility.Create(null);
            Control.RegisterAbility(call__149);
            var call__150 = Aow.Units.PassiveAb.TAnimatedAbility.Create(null);
            Control.RegisterAbility(call__150);
            var call__151 = Aow.Units.PassiveAb.TCombatResurrectedAbility.Create(null);
            Control.RegisterAbility(call__151);
            var call__152 = Aow.Units.PassiveAb.TMarksmanshipAbility.Create(null);
            Control.RegisterAbility(call__152);
            var call__153 = Aow.Units.PassiveAb.TSlowEnchantmentAbility.Create(null);
            Control.RegisterAbility(call__153);
            var call__154 = Aow.Units.PassiveAb.TWebbedAbility.Create(null);
            Control.RegisterAbility(call__154);
            var call__155 = Aow.Units.PassiveAb.TVertigoAbility.Create(null);
            Control.RegisterAbility(call__155);
            var call__156 = Aow.Units.PassiveAb.TPoisonedAbility.Create(null);
            Control.RegisterAbility(call__156);
            var call__157 = Aow.Units.PassiveAb.TEntangledAbility.Create(null);
            Control.RegisterAbility(call__157);
            var call__158 = Aow.Units.PassiveAb.TFrozenAbility.Create(null);
            Control.RegisterAbility(call__158);
            var call__159 = Aow.Units.PassiveAb.TStunnedAbility.Create(null);
            Control.RegisterAbility(call__159);
            var call__160 = Aow.Units.PassiveAb.TNaturesBlessingAbility.Create(null);
            Control.RegisterAbility(call__160);
            var call__161 = Aow.Units.PassiveAb.TBloodlustAbility.Create(null);
            Control.RegisterAbility(call__161);
            var call__162 = Aow.Units.PassiveAb.TTurnedUndeadAbility.Create(null);
            Control.RegisterAbility(call__162);
            var call__163 = Aow.Units.PassiveAb.THolyFear.Create(null);
            Control.RegisterAbility(call__163);
            var call__164 = Aow.Units.PassiveAb.THasteAbility.Create(null);
            Control.RegisterAbility(call__164);
            var call__165 = Aow.Units.PassiveAb.TStoneSkinAbility.Create(null);
            Control.RegisterAbility(call__165);
            var call__166 = Aow.Units.PassiveAb.TEnchantedWeaponAbility.Create(null);
            Control.RegisterAbility(call__166);
            var call__167 = Aow.Units.PassiveAb.TSummonedAbility.Create(null);
            Control.RegisterAbility(call__167);
            var call__168 = Aow.Units.PassiveAb.TFuryAbility.Create(null);
            Control.RegisterAbility(call__168);
            var call__169 = Aow.Units.PassiveAb.TFreeMovementAbility.Create(null);
            Control.RegisterAbility(call__169);
            var call__170 = Aow.Units.PassiveAb.THolyChampionEnchantment.Create(null);
            Control.RegisterAbility(call__170);
            var call__171 = Aow.Units.PassiveAb.TUnholyChampionEnchantment.Create(null);
            Control.RegisterAbility(call__171);
            var call__172 = Aow.Units.PassiveAb.TConcealmentEnchantment.Create(null);
            Control.RegisterAbility(call__172);
            var call__173 = Aow.Units.PassiveAb.TFireAuraEnchantment.Create(null);
            Control.RegisterAbility(call__173);
            var call__174 = Aow.Units.PassiveAb.TWaterWalkingEnchantment.Create(null);
            Control.RegisterAbility(call__174);
            var call__175 = Aow.Units.PassiveAb.TWindWalkingEnchantment.Create(null);
            Control.RegisterAbility(call__175);
            var call__176 = Aow.Units.PassiveAb.TLiquidFormEnchantment.Create(null);
            Control.RegisterAbility(call__176);
            var call__177 = Aow.Units.PassiveAb.TBlessedEnchantment.Create(null);
            Control.RegisterAbility(call__177);
            var call__178 = Aow.Units.AoWE.TCosmeticSurgeryAbility.Create(null);
            Control.RegisterAbility(call__178);
            var call__179 = Aow.Units.PassiveAb.TBurningAbility.Create(null);
            Control.RegisterAbility(call__179);
            var call__180 = Aow.Units.PassiveAb.TFireProtectionEnchantment.Create(null);
            Control.RegisterAbility(call__180);
            var call__181 = Aow.Units.PassiveAb.TPanickedAbility.Create(null);
            Control.RegisterAbility(call__181);
            var call__182 = Aow.Units.PassiveAb.TDecayAbility.Create(null);
            Control.RegisterAbility(call__182);
            var call__183 = Aow.Units.PassiveAb.THighPrayerBlessingAbility.Create(null);
            Control.RegisterAbility(call__183);
            var call__184 = Aow.Units.PassiveAb.TDarkGiftEnchantment.Create(null);
            Control.RegisterAbility(call__184);
            var call__185 = Aow.Units.PassiveAb.TCrusaderAbility.Create(null);
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
            Ability = Aow.Units.PassiveAb.TMovementAbility.Create(null);
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
            Ability = Aow.Units.PassiveAb.TImmunityAbility.Create(null);
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
            Ability = Aow.Units.PassiveAb.TProtectionAbility.Create(null);
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
