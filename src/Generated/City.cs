// Generated from City.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;
using static Aow.Units.Animation.Animation;
using Aow.Units.AoWReg;
using static Aow.Units.AoWReg.AoWReg;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.Crops;
using static Aow.Units.Crops.Crops;
using Aow.Units.Pad;
using static Aow.Units.Pad.Pad;

namespace Aow.Units.City
{
    public enum TCityModeElement : byte
    {
        cmWarmode = 0,
        cmRazed = 1,
        cmTownQuake = 2,
        cmAnarchy = 3
    }

    public enum TMoveOutOfCitySelectionElement : byte
    {
        msSkipOwnPlayer = 0
    }

    public enum AnonymousEnum_saSetup : byte
    {
        saSetup = 0,
        saFade = 1
    }

    // { ot=1 }
    // { TCityProductionControl (instance size 44, VMT 557A6D34): $28 FCity. }
    public partial class TCityProductionControl : Aow.Units.AoWE.TProductionControl
    {
        public Aow.Units.City.TCity FCity;
        // City.pas:573
        // { $28 }
        public override void ExecuteCancelProduction()
        {
            if (FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing)
            {
                var call__1 = FQueue.GetItem(0);
                var call__2 = Aow.Units.City.City.ProductionIDToCityAction(call__1.FProductionID);
                switch (call__2)
                {
                    case Aow.Units.City.TCityAction.caLoot:
                        if (FSettings.FTurnsLeft == FSettings.FTurnsTotal)
                        {
                            var call__3 = GetPlayer();
                            var call__4 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__5 = call__4.GetPlayerRelationValue(unchecked((sbyte)call__3));
                            var call__6 = GetPlayer();
                            var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__7.SetPlayerRelationValue(unchecked((sbyte)call__6), call__5 - -30);
                        }
                        else
                        {
                            var call__8 = GetPlayer();
                            var call__9 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__10 = call__9.GetPlayerRelationValue(unchecked((sbyte)call__8));
                            var call__11 = GetPlayer();
                            var call__12 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__12.SetPlayerRelationValue(unchecked((sbyte)call__11), call__10 + 20);
                        }

                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        if (FSettings.FTurnsLeft == FSettings.FTurnsTotal)
                        {
                            var call__13 = GetPlayer();
                            var call__14 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__15 = call__14.GetPlayerRelationValue(unchecked((sbyte)call__13));
                            var call__16 = GetPlayer();
                            var call__17 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__17.SetPlayerRelationValue(unchecked((sbyte)call__16), call__15 - -15);
                        }
                        else
                        {
                            var call__18 = GetPlayer();
                            var call__19 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__20 = call__19.GetPlayerRelationValue(unchecked((sbyte)call__18));
                            var call__21 = GetPlayer();
                            var call__22 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__22.SetPlayerRelationValue(unchecked((sbyte)call__21), call__20 + 10);
                        }

                        break;
                }
            }

            base.ExecuteCancelProduction();
        }

        // City.pas:434
        public override bool ExecuteProduction(Aow.Units.AoWE.TProductionItem Item)
        {
            bool Result = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            Result = base.ExecuteProduction(Item);
            if (Result)
            {
                var call__23 = Aow.Units.City.City.ProductionIDToCityAction(Item.FProductionID);
                switch (call__23)
                {
                    case Aow.Units.City.TCityAction.caTrade:
                        FSettings.FTurnsTotal = unchecked((sbyte)Item.FParameter);
                        FSettings.FTurnsLeft = 1;
                        var call__24 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__24);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        break;
                    case Aow.Units.City.TCityAction.caFortify:
                        var call__25 = FCity.FortifyTime();
                        FSettings.FTurnsTotal = unchecked((sbyte)call__25);
                        FSettings.FTurnsLeft = unchecked((sbyte)FSettings.FTurnsTotal);
                        var call__26 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__26);
                        var call__27 = FCity.FortifyCost();
                        FSettings.FGold = call__27;
                        var call__28 = GetPlayer();
                        var call__29 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__28);
                        var call__30 = GetPlayer();
                        var call__31 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__30);
                        call__31.SetGems(call__29.FGems - FSettings.FGold);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        break;
                    case Aow.Units.City.TCityAction.caUpgrade:
                        var call__32 = FCity.UpgradeTime();
                        FSettings.FTurnsTotal = unchecked((sbyte)call__32);
                        FSettings.FTurnsLeft = unchecked((sbyte)FSettings.FTurnsTotal);
                        var call__33 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__33);
                        var call__34 = FCity.UpgradeCost();
                        FSettings.FGold = call__34;
                        var call__35 = GetPlayer();
                        var call__36 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__35);
                        var call__37 = GetPlayer();
                        var call__38 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__37);
                        call__38.SetGems(call__36.FGems - FSettings.FGold);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        break;
                    case Aow.Units.City.TCityAction.caLoot:
                        var call__39 = FCity.LootTime();
                        FSettings.FTurnsTotal = unchecked((sbyte)call__39);
                        FSettings.FTurnsLeft = unchecked((sbyte)FSettings.FTurnsTotal);
                        var call__40 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__40);
                        FSettings.FGold = 0;
                        var call__41 = GetPlayer();
                        var call__42 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__43 = call__42.GetPlayerRelationValue(unchecked((sbyte)call__41));
                        var call__44 = GetPlayer();
                        var call__45 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__45.SetPlayerRelationValue(unchecked((sbyte)call__44), call__43 + -30);
                        Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                        var call__46 = GetPlayer();
                        Log.FPlayer = unchecked((sbyte)call__46);
                        var call__47 = FCity.GetName();
                        var call__48 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityLootingRStr);
                        var call__49 = Aow.Rtl.Rtl.Format(call__48, [call__47]);
                        Log.FText = call__49;
                        var call__50 = FCity.GetLhx();
                        var call__51 = FCity.GetYhx();
                        var call__52 = FCity.GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__52), unchecked((sbyte)call__51), unchecked((sbyte)call__50));
                        var call__53 = FCity.GetYhx();
                        var call__54 = FCity.GetLhx();
                        var call__55 = FCity.GetXhx();
                        var call__56 = GetPlayer();
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)call__56), unchecked((sbyte)call__55), unchecked((sbyte)call__53), unchecked((sbyte)call__54), false);
                        Log.Release();
                        FCity.FOverlay.SetMode(Aow.Units.City.TCityOverlayMode.omLooting);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        var call__57 = FCity.MigrationTime(unchecked((sbyte)Item.FParameter));
                        FSettings.FTurnsTotal = unchecked((sbyte)call__57);
                        FSettings.FTurnsLeft = unchecked((sbyte)FSettings.FTurnsTotal);
                        var call__58 = GetPlayer();
                        FSettings.FPlayer = unchecked((sbyte)call__58);
                        FStatus = Aow.Units.AoWE.TProductionControlStatus.pcsProducing;
                        var call__59 = GetPlayer();
                        var call__60 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__61 = call__60.GetPlayerRelationValue(unchecked((sbyte)call__59));
                        var call__62 = GetPlayer();
                        var call__63 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__63.SetPlayerRelationValue(unchecked((sbyte)call__62), call__61 + -15);
                        break;
                }
            }

            return Result;
        }

        // City.pas:645
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            var call__64 = FCity.GetXhx();
            X = unchecked((sbyte)call__64);
            var call__65 = FCity.GetYhx();
            Y = unchecked((sbyte)call__65);
            var call__66 = FCity.GetLhx();
            L = unchecked((sbyte)call__66);
        }

        // City.pas:707
        public override sbyte GetPlayer()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)FCity.FPlayer);
            return Result;
        }

        // City.pas:604
        public override bool LoopableProduction(Aow.Units.AoWE.TProductionItem A1)
        {
            bool Result = default;
            Result = base.LoopableProduction(A1);
            var call__67 = Aow.Units.City.City.ProductionIDToCityAction(A1.FProductionID);
            switch (call__67)
            {
                case Aow.Units.City.TCityAction.caMigrate:
                    Result = false;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = false;
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    Result = false;
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    Result = false;
                    break;
            }

            return Result;
        }

        // City.pas:595
        public override bool LockQueueProduction(Aow.Units.AoWE.TProductionItem A1)
        {
            bool Result = default;
            Result = base.LockQueueProduction(A1);
            var call__68 = Aow.Units.City.City.ProductionIDToCityAction(A1.FProductionID);
            switch (call__68)
            {
                case Aow.Units.City.TCityAction.caMigrate:
                    Result = true;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = true;
                    break;
            }

            return Result;
        }

        // City.pas:675
        public override bool UnitProduced()
        {
            bool Result = default;
            Aow.Units.City.TCity City = default;
            bool Valid = default;
            int Count = default;
            Aow.Units.HSEngine.TMapLevel Level = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.City.TCityProductionEventLog Log = default;
            City = FCity;
            Valid = false;
            Count = City.GetTerrainCount();
            Level = (Aow.Units.HSEngine.TMapLevel)City.Owner.Owner;
            var call__69 = City.GetYhx();
            var call__70 = City.GetXhx();
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__70, call__69, 0);
            while ((Walk.HN < Count) && !Valid)
            {
                var call__71 = City.FResource.TerrainList.Terrain[Walk.HN];
                if (((((call__71 >= 0) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < Level.Width)) && (Walk.Y < Level.Height))
                {
                    var call__72 = City.GetLhx();
                    var call__73 = FSettings.FUnit.Place(Walk.X, Walk.Y, call__72, unchecked((sbyte)City.FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of());
                    Valid = call__73 != null;
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            if (!Valid)
            {
                var call__74 = City.GetLhx();
                var call__75 = City.GetYhx();
                var call__76 = City.GetXhx();
                var call__77 = FSettings.FUnit.Place(call__76, call__75, call__74, unchecked((sbyte)City.FPlayer), TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of(Aow.Units.AoWE.TUnitPlaceModeElement.pmScanArea));
                Valid = call__77 != null;
            }

            Log = Aow.Units.City.TCityProductionEventLog.Create(null);
            Log.FPlayer = unchecked((sbyte)City.FPlayer);
            var call__78 = FSettings.FUnit.GetName();
            var call__79 = FCity.GetName();
            var call__80 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XProducedAtYRStr);
            var call__81 = Aow.Rtl.Rtl.Format(call__80, [call__78, call__79]);
            Log.FText = call__81;
            var call__82 = City.GetLhx();
            var call__83 = City.GetYhx();
            var call__84 = City.GetXhx();
            Log.SetLocation(unchecked((sbyte)call__84), unchecked((sbyte)call__83), unchecked((sbyte)call__82));
            var call__85 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__85.FEventLogbook.AddEvent(Log);
            Log.Release();
            Result = Valid;
            return Result;
        }

        // City.pas:658
        public override void UnitResearched()
        {
            Aow.Units.City.TCity City = default;
            Aow.Units.City.TCityProductionEventLog Log = default;
            base.UnitResearched();
            City = FCity;
            Log = Aow.Units.City.TCityProductionEventLog.Create(null);
            Log.FPlayer = unchecked((sbyte)City.FPlayer);
            var call__86 = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var call__87 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__86.FUnitIndex);
            var call__88 = call__87.GetUnitName();
            var call__89 = FCity.GetName();
            var call__90 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XInstalledAtYRStr);
            var call__91 = Aow.Rtl.Rtl.Format(call__90, [call__88, call__89]);
            Log.FText = call__91;
            var call__92 = City.GetLhx();
            var call__93 = City.GetYhx();
            var call__94 = City.GetXhx();
            Log.SetLocation(unchecked((sbyte)call__94), unchecked((sbyte)call__93), unchecked((sbyte)call__92));
            var call__95 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__95.FEventLogbook.AddEvent(Log);
            Log.Release();
        }

        // City.pas:639
        public override void Update()
        {
            // { Unmatched: tried identity assignment to Self; Pointer identity assignment to Self; zero-shift receiver before field access (5 variants total). }
            base.Update();
            if (FUpdateCount == 0)
            {
                FCity.Update();
            }
        }

        // City.pas:652
        public override Aow.Units.AoWE.TUnit CreateUnitProduction(int A1)
        {
            Aow.Units.AoWE.TUnit Result = default;
            Result = base.CreateUnitProduction(A1);
            if ((Result != null) && FCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiWarmonger))
            {
                Result.SetRank(Aow.Units.AoWE.TUnitRank.urSilver);
            }

            return Result;
        }

        // City.pas:496
        public override void NewTurn(sbyte Player)
        {
            Aow.Units.City.TCityProductionEventLog Log = default;
            base.NewTurn(unchecked((sbyte)Player));
            if (((FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing) && (Player == FSettings.FPlayer)) && (FSettings.FTurnsLeft == 0))
            {
                var call__96 = FQueue.GetItem(0);
                var call__97 = Aow.Units.City.City.ProductionIDToCityAction(call__96.FProductionID);
                switch (call__97)
                {
                    case Aow.Units.City.TCityAction.caTrade:
                        ProductionDone();
                        break;
                    case Aow.Units.City.TCityAction.caUpgrade:
                        BeginUpdate();
                        FCity.SetUpgradeLevel(unchecked((byte)(FCity.FUpgradeLevel + 1)));
                        var call__98 = GetPlayer();
                        var call__99 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__100 = call__99.GetPlayerRelationValue(unchecked((sbyte)call__98));
                        var call__101 = GetPlayer();
                        var call__102 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__102.SetPlayerRelationValue(unchecked((sbyte)call__101), call__100 + 5);
                        ProductionDone();
                        Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                        var call__103 = GetPlayer();
                        Log.FPlayer = unchecked((sbyte)call__103);
                        var call__104 = FCity.GetName();
                        var call__105 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XUpgradedRStr);
                        var call__106 = Aow.Rtl.Rtl.Format(call__105, [call__104]);
                        Log.FText = call__106;
                        var call__107 = FCity.GetLhx();
                        var call__108 = FCity.GetYhx();
                        var call__109 = FCity.GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__109), unchecked((sbyte)call__108), unchecked((sbyte)call__107));
                        var call__110 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__110.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        EndUpdate();
                        break;
                    case Aow.Units.City.TCityAction.caFortify:
                        BeginUpdate();
                        switch (FCity.FWallType)
                        {
                            case 0:
                                FCity.FWallType = 1;
                                break;
                            case 1:
                                FCity.FWallType = 2;
                                break;
                        }

                        Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                        var call__111 = GetPlayer();
                        Log.FPlayer = unchecked((sbyte)call__111);
                        var call__112 = FCity.GetName();
                        var call__113 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityFortifiedRStr);
                        var call__114 = Aow.Rtl.Rtl.Format(call__113, [call__112]);
                        Log.FText = call__114;
                        var call__115 = FCity.GetLhx();
                        var call__116 = FCity.GetYhx();
                        var call__117 = FCity.GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__117), unchecked((sbyte)call__116), unchecked((sbyte)call__115));
                        var call__118 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__118.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        FCity.UpdateImages();
                        var call__119 = GetPlayer();
                        var call__120 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__121 = call__120.GetPlayerRelationValue(unchecked((sbyte)call__119));
                        var call__122 = GetPlayer();
                        var call__123 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__123.SetPlayerRelationValue(unchecked((sbyte)call__122), call__121 + 5);
                        ProductionDone();
                        EndUpdate();
                        break;
                    case Aow.Units.City.TCityAction.caLoot:
                        BeginUpdate();
                        Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                        var call__124 = GetPlayer();
                        Log.FPlayer = unchecked((sbyte)call__124);
                        var call__125 = FCity.GetName();
                        var call__126 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityLootedRStr);
                        var call__127 = Aow.Rtl.Rtl.Format(call__126, [call__125, Aow.Rtl.Rtl.IntToStr(FCity.FIncome * 3)]);
                        Log.FText = call__127;
                        var call__128 = FCity.GetLhx();
                        var call__129 = FCity.GetYhx();
                        var call__130 = FCity.GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__130), unchecked((sbyte)call__129), unchecked((sbyte)call__128));
                        var call__131 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__131.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        var call__132 = GetPlayer();
                        var call__133 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__132);
                        var call__134 = GetPlayer();
                        var call__135 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__134);
                        call__135.SetGems(call__133.FGems + (FCity.FIncome * 3));
                        FCity.FOverlay.SetMode(Aow.Units.City.TCityOverlayMode.omIdle);
                        FCity.SetRazed(true);
                        EndUpdate();
                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        BeginUpdate();
                        Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                        var call__136 = GetPlayer();
                        Log.FPlayer = unchecked((sbyte)call__136);
                        var call__137 = FCity.GetName();
                        var call__138 = FQueue.GetItem(0);
                        var call__139 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[call__138.FParameter]);
                        var call__140 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityMigratedToRStr);
                        var call__141 = Aow.Rtl.Rtl.Format(call__140, [call__137, call__139]);
                        Log.FText = call__141;
                        var call__142 = FCity.GetLhx();
                        var call__143 = FCity.GetYhx();
                        var call__144 = FCity.GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__144), unchecked((sbyte)call__143), unchecked((sbyte)call__142));
                        var call__145 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__145.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        var call__146 = FQueue.GetItem(0);
                        FCity.SetRace(unchecked((sbyte)call__146.FParameter));
                        var call__147 = GetPlayer();
                        var call__148 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__149 = call__148.GetPlayerRelationValue(unchecked((sbyte)call__147));
                        var call__150 = GetPlayer();
                        var call__151 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__151.SetPlayerRelationValue(unchecked((sbyte)call__150), call__149 + 10);
                        FUnitProductions.Clear();
                        FCity.SetUpgradeLevel(unchecked((byte)FCity.FUpgradeLevel));
                        ProductionDone();
                        EndUpdate();
                        break;
                }
            }
        }

        // City.pas:408
        public override bool CanExecuteProduction(Aow.Units.AoWE.TProductionItem Item, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__152 = Aow.Units.City.City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__152)
            {
                case Aow.Units.City.TCityAction.caTrade:
                    Result = true;
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    var call__153 = GetPlayer();
                    var call__154 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__153);
                    var call__155 = FCity.FortifyCost();
                    if (call__154.FGems >= call__155)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__156 = Aow.Units.AoWE.AoWE.NotEnoughGoldRStr;
                        var call__157 = Aow.Units.AoWE.AoWE.TranslateRStr(call__156);
                        Reason = call__157;
                    }

                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    var call__158 = FCity.CanMigrate(unchecked((sbyte)Item.FParameter), ref Reason);
                    Result = call__158;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    var call__159 = FCity.CanLoot(ref Reason);
                    Result = call__159;
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    var call__160 = GetPlayer();
                    var call__161 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(call__160);
                    var call__162 = FCity.UpgradeCost();
                    if (call__161.FGems >= call__162)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__163 = Aow.Units.AoWE.AoWE.NotEnoughGoldRStr;
                        var call__164 = Aow.Units.AoWE.AoWE.TranslateRStr(call__163);
                        Reason = call__164;
                    }

                    break;
                default:
                    Result = base.CanExecuteProduction(Item, ref Reason);
                    break;
            }

            return Result;
        }

        // City.pas:615
        public override Aow.Rtl.AnsiString GetProductionName(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = base.GetProductionName(Item);
            var call__165 = Aow.Units.City.City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__165)
            {
                case Aow.Units.City.TCityAction.caTrade:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceMerchandiseRStr);
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradeRStr);
                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    var call__166 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Item.FParameter]);
                    var call__167 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MigrateToXRStr);
                    var call__168 = Aow.Rtl.Rtl.Format(call__167, [call__166]);
                    Result = call__168;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LootRStr);
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FortifyRStr);
                    break;
            }

            return Result;
        }

        // City.pas:627
        public override Aow.Rtl.AnsiString GetProductionNameProgressive(Aow.Units.AoWE.TProductionItem Item)
        {
            Aow.Rtl.AnsiString Result = default;
            Result = base.GetProductionNameProgressive(Item);
            var call__169 = Aow.Units.City.City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__169)
            {
                case Aow.Units.City.TCityAction.caTrade:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr);
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradingRStr);
                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    var call__170 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RaceNamesPluralRStr[Item.FParameter]);
                    var call__171 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.MigratingToXRStr);
                    var call__172 = Aow.Rtl.Rtl.Format(call__171, [call__170]);
                    Result = call__172;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LootingRStr);
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    Result = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FortifyingRStr);
                    break;
            }

            return Result;
        }

        // City.pas:710
        public override int UnitProductionTime(int Index)
        {
            int Result = default;
            int UnitIndex = default;
            var call__173 = FUnitProductions.GetItem(Index);
            UnitIndex = call__173.FUnitIndex;
            var call__174 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
            var call__175 = FCity.GetSize();
            Result = (call__174.FLevel - (call__175 / 2)) + 1;
            if (Result < 1)
            {
                Result = 1;
            }

            return Result;
        }

        // intf_P1_02.inc:120
        public static new Aow.Units.City.TCityProductionControl Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityProductionControl o = new Aow.Units.City.TCityProductionControl();
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

    // { TCityOverlayHS (instance size 28, VMT 557A6EFC): $0C FCity,
//     $10 FModeStartFrame, $14 FMode, $15 FShellAnimation, $18 FShellEndFrame. }
    public partial class TCityOverlayHS : Aow.Units.HSEngine.TSingleHS
    {
        public Aow.Units.City.TCity FCity;
        // { $0C }
        public int FModeStartFrame;
        // { $10 }
        public Aow.Units.City.TCityOverlayMode FMode;
        // { $14 }
        public Aow.Units.City.AnonymousEnum_saSetup FShellAnimation;
        // { $15 }
        public Aow.Units.FixedArray_byte_2 F_CityOverlayHS_16;
        public int FShellEndFrame;
        // City.pas:718
        // { $18 }
        public override int GetShowPriority()
        {
            int Result = default;
            var call__176 = FCity.GetShowPriority();
            Result = call__176 + 134217728;
            return Result;
        }

        // City.pas:733
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int Intensity = default;
            int Index = default;
            int Frame = default;
            Aow.Units.ImageLib.TImageSequence Sequence = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FCity.FXhx, FCity.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FCity.FEnchantmentsVisible)
            {
                if (FCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiWarmonger))
                {
                    Intensity = System.Math.Abs((((int)(((uint)Aow.Units.AoWE.AoWE.AoWHSMap.Frame) >> 1)) % 20) - 10) + 50;
                    var call__177 = FCity.FResource.ImageSequenceList.ImageSequences[17];
                    Sequence = call__177;
                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(0, 0);
                        var call__178 = FCity.FResource.ImageSequenceList.ImageSequences[17];
                        if (Sequence == call__178)
                        {
                            var call__179 = Sequence.ImageLibrary[Index];
                            call__179.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
                        }
                        else
                        {
                            var call__180 = Sequence.ImageLibrary[Index];
                            call__180.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Intensity]);
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
                {
                    Intensity = System.Math.Abs((((int)(((uint)Aow.Units.AoWE.AoWE.AoWHSMap.Frame) >> 1)) % 20) - 10) + 70;
                    var call__181 = FCity.FResource.ImageSequenceList.ImageSequences[18];
                    Sequence = call__181;
                    while (Sequence != null)
                    {
                        var call__182 = FCity.FResource.ImageSequenceList.ImageSequences[18];
                        if (Sequence == call__182)
                        {
                            Index = Sequence.GetFrameIndex(0, 0);
                            var call__183 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__183)
                            {
                                var call__184 = Sequence.ImageLibrary[Index];
                                call__184.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
                            }
                        }
                        else
                        {
                            var call__185 = Sequence.GetFrameIndex(0, Aow.Units.AoWE.AoWE.AoWHSMap.Frame);
                            Index = call__185;
                            var call__186 = Sequence.ImageLibrary.ImageExist(Index);
                            if (call__186)
                            {
                                var call__187 = Sequence.ImageLibrary[Index];
                                call__187.Show(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc);
                            }
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
                {
                    var call__188 = FCity.FResource.ImageSequenceList.ImageSequences[19];
                    Sequence = call__188;
                    if (Sequence != null)
                    {
                        Intensity = 50;
                        Index = Sequence.GetFrameIndex(0, 0);
                        if ((FShellEndFrame != -1) && (Aow.Units.AoWE.AoWE.AoWHSMap.Frame < FShellEndFrame))
                        {
                            switch (FShellAnimation)
                            {
                                case Aow.Units.City.AnonymousEnum_saSetup.saSetup:
                                    Frame = FShellEndFrame - Aow.Units.AoWE.AoWE.AoWHSMap.Frame;
                                    if (Frame <= 50)
                                    {
                                        Intensity = 50 - Frame;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                                case Aow.Units.City.AnonymousEnum_saSetup.saFade:
                                    Frame = FShellEndFrame - Aow.Units.AoWE.AoWE.AoWHSMap.Frame;
                                    if (Frame <= 20)
                                    {
                                        Intensity = Intensity + ((10 - System.Math.Abs(Frame - 10)) * 3);
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                            }
                        }

                        var call__189 = Sequence.ImageLibrary[Index];
                        call__189.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[Intensity]);
                    }
                }

                if (FMode == Aow.Units.City.TCityOverlayMode.omLooting)
                {
                    FCity.FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 22, unchecked((uint)(((Aow.Units.HSEngine.TMapField)FCity.Owner).Xhx + (((Aow.Units.HSEngine.TMapField)FCity.Owner).Yhx * 16))));
                }
            }
        }

        // City.pas:721
        public void PlaySetupShellAnimation()
        {
            FShellAnimation = Aow.Units.City.AnonymousEnum_saSetup.saSetup;
            FShellEndFrame = Aow.Units.AoWE.AoWE.AoWHSMap.Frame + 50;
        }

        // City.pas:727
        public void PlayFadeShellAnimation()
        {
            FShellAnimation = Aow.Units.City.AnonymousEnum_saSetup.saFade;
            FShellEndFrame = Aow.Units.AoWE.AoWE.AoWHSMap.Frame + 20;
        }

        // City.pas:815
        public void SetMode(Aow.Units.City.TCityOverlayMode Mode)
        {
            if (Mode != FMode)
            {
                FModeStartFrame = Aow.Units.AoWE.AoWE.AoWHSMap.Frame;
                FMode = Mode;
            }
        }

        // Engine.pas:524
        public static new Aow.Units.City.TCityOverlayHS Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityOverlayHS o = new Aow.Units.City.TCityOverlayHS();
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

    // { TMigrationItem (instance size 20, VMT 557A7084): $08 FRace, $0C FTime, $10 FRelation. }
    public partial class TMigrationItem : Aow.Units.Engine.TEObject
    {
        public sbyte FRace;
        // { $08 }
        public Aow.Units.FixedArray_byte_3 FReserved09;
        public int FTime;
        // { $0C }
        public Aow.Units.AoWE.TStatus FRelation;
        // { $10 }
        public Aow.Units.FixedArray_byte_3 FReserved11;
        // Engine.pas:524
        public static new Aow.Units.City.TMigrationItem Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TMigrationItem o = new Aow.Units.City.TMigrationItem();
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

    // { TMigrationList (instance size 12, VMT 557A7148) }
    public partial class TMigrationList : Aow.Units.Engine.TENode
    {
        // City.pas:2711
        public Aow.Units.City.TMigrationItem GetItems(int Index)
        {
            Aow.Units.City.TMigrationItem Result = default;
            Result = (Aow.Units.City.TMigrationItem)FChildren.List[Index];
            return Result;
        }

        // Engine.pas:631
        public static new Aow.Units.City.TMigrationList Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TMigrationList o = new Aow.Units.City.TMigrationList();
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

    // { TCityEnchantment (instance size 44, VMT 557A7294): $24 FEnchantmentID, $28 FEnchantmentLocation. }
    public partial class TCityEnchantment : Aow.Units.AoWE.TGlobalEnchantment
    {
        public new Aow.Units.City.TCityEnchantmentID FEnchantmentID;
        // { $24 }
        public Aow.Units.FixedArray_byte_3 F_CityEnchantment_25;
        public Aow.Units.HSEngine.TXYL FEnchantmentLocation;
        // City.pas:2727
        // { $28 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FEnchantmentID, 1), 1, 20);
            Stream.rwInteger(ref FEnchantmentLocation.XYL, 21);
        }

        // City.pas:2716
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FEnchantmentMode = FEnchantmentMode - TSet<Aow.Units.AoWE.TGlobalEnchantmentModeElement>.Of(Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemTrueGlobal, Aow.Units.AoWE.TGlobalEnchantmentModeElement.gemOwned);
        }

        // City.pas:2762
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__190 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.City.City.TCityEnchantmentIDRStr[(int)FEnchantmentID]);
            Result = call__190;
            return Result;
        }

        // City.pas:2739
        public override Aow.Units.HSEngine.TXYL GetLocation()
        {
            Aow.Units.HSEngine.TXYL Result = default;
            Result.X = unchecked((sbyte)FEnchantmentLocation.X);
            Result.Y = unchecked((sbyte)FEnchantmentLocation.Y);
            Result.L = unchecked((sbyte)FEnchantmentLocation.L);
            return Result;
        }

        // City.pas:2767
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            Aow.Units.City.TCity City = default;
            City = GetCity();
            if (City != null)
            {
                var call__191 = City.AICityEnchantmentUpkeepPriority(FEnchantmentID);
                Result = call__191;
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // City.pas:2746
        public override bool Deactivate(byte Player)
        {
            bool Result = default;
            if (FEnchantmentStatus.Contains(Aow.Units.AoWE.TGlobalEnchantmentStatusElement.getActivated))
            {
                Result = base.Deactivate(unchecked((byte)Player));
                var call__192 = GetCity();
                call__192.DeactivateEnchantment(this);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2734
        public override bool VisibleForPlayer(sbyte Player)
        {
            bool Result = default;
            var call__193 = GetCity();
            var call__194 = call__193.VisibleForPlayer(unchecked((sbyte)Player));
            Result = call__194;
            return Result;
        }

        // City.pas:2756
        public Aow.Units.City.TCity GetCity()
        {
            Aow.Units.City.TCity Result = default;
            var call__195 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FEnchantmentLocation.X, FEnchantmentLocation.Y, FEnchantmentLocation.L);
            var call__196 = call__195.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            Result = (Aow.Units.City.TCity)call__196;
            return Result;
        }

        // City.pas:98
        public static new Aow.Units.City.TCityEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityEnchantment o = new Aow.Units.City.TCityEnchantment();
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

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // City.pas:2722
            public override int ClassID()
            {
                int Result = default;
                Result = 131820;
                return Result;
            }

            // City.pas:98
            public new Aow.Units.City.TCityEnchantment Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCityEnchantment o = (Aow.Units.City.TCityEnchantment)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCityEnchantment => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCity (instance size 120, VMT 557A73F0): $44 FMode, $45 FRace,
//     $48 FName, $4C FWallType, $4D FUpgradeLevel, $50 FProduction,
//     $54 FFlagID, $58 FEnchantmentList, $5C FEnchantments, $60 FOverlay, $64 FEnchantmentsVisible,
//     $68 FWallBackImage, $6C FWallFrontImage, $70 FCityImage, $74 FDefendersMode. }
    public partial class TCity : Aow.Units.Crops.TPlayerCropStructure
    {
        public TSet<Aow.Units.City.TCityModeElement> FMode;
        // { $44 }
        public sbyte FRace;
        // { $45 }
        public Aow.Units.FixedArray_byte_2 F_City_46;
        public Aow.Rtl.AnsiString FName;
        // { $48 }
        public byte FWallType;
        // { $4C }
        public byte FUpgradeLevel;
        // { $4D }
        public Aow.Units.FixedArray_byte_2 F_City_4E;
        public Aow.Units.City.TCityProductionControl FProduction;
        // { $50 }
        public new byte FFlagID;
        // { $54 }
        public Aow.Units.FixedArray_byte_3 F_City_55;
        public Aow.Units.AoWE.TGlobalEnchantmentList FEnchantmentList;
        // { $58 }
        public TSet<Aow.Units.City.TCityEnchantmentID> FEnchantments;
        // { $5C }
        // { conflict: FEnchantmentIDs: TCityEnchantmentIDs; }
        public Aow.Units.FixedArray_byte_3 F_City_5D;
        public Aow.Units.City.TCityOverlayHS FOverlay;
        // { $60 }
        public bool FEnchantmentsVisible;
        // { $64 }
        public Aow.Units.FixedArray_byte_3 F_City_65;
        public Aow.Units.ImageLib.TLibraryImage FWallBackImage;
        // { $68 }
        public Aow.Units.ImageLib.TLibraryImage FWallFrontImage;
        // { $6C }
        public Aow.Units.ImageLib.TLibraryImage FCityImage;
        // { $70 }
        public Aow.Units.City.TDefendersGenerateMode FDefendersMode;
        // { $74 }
        public Aow.Units.FixedArray_byte_3 F_City_75;
        // City.pas:839
        public override void Destroy()
        {
            if (GetVisible())
            {
                Aow.Units.AoWE.AoWE.AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlay);
            }

            FOverlay.Free();
            base.Destroy();
            FProduction.Free();
            FEnchantmentList.Free();
            FEnchantmentList = null;
        }

        // City.pas:2157
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwString(ref FName, 12);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 13);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FWallType, 1), 1, 15);
            Stream.rwByte(ref FUpgradeLevel, 16);
            Stream.rwEObject(FProduction, 18);
            Aow.Rtl.Rtl.RwSet(ref FMode, 1, 22, Stream.rwData, 0);
            Stream.rwData(Aow.Rtl.Bytes.Of(ref FDefendersMode, 1), 1, 27);
            Stream.rwEObject(FEnchantmentList, 29);
        }

        // City.pas:824
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            var call__197 = Aow.Units.City.TCityProductionControl.Create(null);
            FProduction = call__197;
            FProduction.FCity = this;
            FDefendersMode = Aow.Units.City.TDefendersGenerateMode.dgAverage;
            FRace = unchecked((sbyte)-1);
            var call__198 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NonameRStr);
            FName = call__198;
            FUpgradeLevel = 1;
            var call__199 = Aow.Units.City.TCityOverlayHS.Create(null);
            FOverlay = call__199;
            FOverlay.FCity = this;
            var call__200 = Aow.Units.AoWE.TGlobalEnchantmentList.Create(null);
            FEnchantmentList = call__200;
            FEnchantmentsVisible = false;
        }

        // City.pas:2191
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001989)
            {
                if (FRace >= 0)
                {
                    var call__201 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    bool bool__202 = (call__201 == null);
                    if (!bool__202)
                    {
                        var call__203 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                        bool__202 = !call__203.FEnabled;
                    }

                    if (bool__202)
                    {
                        FRace = unchecked((sbyte)-1);
                        UpdateImages();
                    }
                }
            }
            else
            {
                if (Msg.MsgID == 537001993)
                {
                    UpdateVisibleForSeatedPlayer();
                }
            }
        }

        // City.pas:2065
        public override void TerrainChanged(in Aow.Units.HSEngine.THSTerrainInfo Info)
        {
            base.TerrainChanged(Info);
            UpdateImages();
        }

        // City.pas:2132
        public override void Show(int XhpSurf, int YhpSurf, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            Aow.Units.HSEngine.HSEngine.HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            bool bool__204 = !FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
            if (!bool__204)
            {
                var call__205 = GetBuilding();
                bool__204 = call__205;
            }

            if (bool__204)
            {
                /* TODO(T-N3) Untyped pointer dereference requires a typed storage override */
                throw new System.NotImplementedException();
            }

            if (FMode.Contains(Aow.Units.City.TCityModeElement.cmTownQuake))
            {
                XhpSurf = (XhpSurf + Aow.Rtl.Rtl.Random(5)) - 2;
                YhpSurf = (YhpSurf + Aow.Rtl.Rtl.Random(5)) - 2;
            }

            if (!FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed))
            {
                ((Aow.Units.City.TCityResource)FResource).ShowCity(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
            else
            {
                ((Aow.Units.City.TCityResource)FResource).Show(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
        }

        // City.pas:2170
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((Aow.Units.HSEngine.TMapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // City.pas:2176
        public override void RemoveFromMap()
        {
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // City.pas:2378
        public override void MapFieldMsgProc(ref Aow.Units.HSEngine.TMapFieldMsg Msg)
        {
            // { Unmatched: tried owned-player color branch before independent-player branch. }
            bool bool__206 = (Msg.MsgID == 4364);
            if (bool__206)
            {
                var call__207 = FResource.TerrainList.Terrain[Msg.HN];
                bool__206 = (call__207 >= 0);
            }

            if (!bool__206)
            {
                base.MapFieldMsgProc(ref Msg);
            }

            var call__208 = FResource.TerrainList.Terrain[Msg.HN];
            if (call__208 >= 0)
            {
                if ((Msg.MsgID == 4364) && (Msg.HN == 0))
                {
                    if ((!Aow.Units.AoWE.AoWE.AoWHSMap.FFogEnabled || !FFogged) && (FPlayer >= 0))
                    {
                        if (FPlayer == 0)
                        {
                            Msg.Param[0] = Msg.Param[0].Value + 851968;
                        }
                        else
                        {
                            var call__209 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                            Msg.Param[0] = Msg.Param[0].Value + ((call__209.FPlayerRace + 1) * 65536);
                        }
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Value + 917504;
                    }

                    Msg.Param[0] = Msg.Param[0].Value + ((((Aow.Units.City.TCityResource)FResource).FSize - 1) * 4194304);
                }
                else
                {
                    if (Msg.MsgID == 4609)
                    {
                        Msg.Result = Aow.Rtl.Rtl.ObjectToInt(FProduction);
                    }
                    else
                    {
                        if (Msg.MsgID == 4360)
                        {
                            /* TODO(T-N3) Untyped pointer dereference requires a typed storage override */
                            throw new System.NotImplementedException();
                        }
                        else
                        {
                            if (Msg.MsgID == 4359)
                            {
                                InitCombat(Msg.Param[0].As<Aow.Units.AoWE.TCombat>(), Msg.Param[1].As<Aow.Units.AoWE.TArmyHS>());
                                Msg.Result = 1;
                            }
                            else
                            {
                                if (Msg.MsgID == 4365)
                                {
                                    UpdateVisibleForSeatedPlayer();
                                }
                                else
                                {
                                    if (Msg.MsgID == 4387)
                                    {
                                        bool bool__210 = (FWallType != 0) && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode);
                                        if (bool__210)
                                        {
                                            var call__211 = GetRazed();
                                            bool__210 = !call__211;
                                        }

                                        if (bool__210)
                                        {
                                            var call__212 = Msg.Param[0].As<Aow.Units.HSEngine.TMapField>().FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                                            if (this == call__212)
                                            {
                                                Msg.Result = FWallType;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Msg.MsgID == 4389)
                                        {
                                            if (FPlayer == 0)
                                            {
                                                var call__213 = GetIndependentRelation();
                                                Msg.Result = call__213;
                                            }
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 4366)
                                            {
                                                var call__214 = IncommingNegotiateRequest(Msg.Param[0].As<Aow.Units.AoWE.TNegotiateRequestEventLog>());
                                                if (call__214)
                                                {
                                                    Msg.Param[1] = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4367)
                                                {
                                                    var call__215 = IncommingNegotiateOffer(Msg.Param[0].As<Aow.Units.AoWE.TNegotiateTE>());
                                                    if (call__215)
                                                    {
                                                        Msg.Result = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if ((Msg.MsgID == 4393) && FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
                                                    {
                                                        Msg.Param[0] = 1;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // City.pas:2124
        public override void Deactivate()
        {
            UnregisterEnchantments();
            if (FPlayer != -1)
            {
                var call__216 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__216.FProductionControls.Remove(FProduction);
            }

            FProduction.Deactivate();
            base.Deactivate();
        }

        // City.pas:2184
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            var call__217 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__217 >= 0)
            {
                ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags = ((Aow.Units.AoWE.TAoWMapField)Info.MapField).FFieldFlags | 67108864;
            }
        }

        // City.pas:1246
        public override void MakeVisible(Aow.Units.HSEngine.TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if ((TSet<Aow.Units.City.TCityEnchantmentID>.Of(Aow.Units.City.TCityEnchantmentID.ceiGoldRush, Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell, Aow.Units.City.TCityEnchantmentID.ceiWarmonger) & FEnchantments) != TSet<Aow.Units.City.TCityEnchantmentID>.Of())
                {
                    FOverlay.MakeVisible(VisibilityList);
                }
            }
        }

        // City.pas:2617
        public override void Select()
        {
            base.Select();
        }

        // City.pas:1787
        public override sbyte GetValidTerrainType(Aow.Units.HSEngine.TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = unchecked((sbyte)GetMostFrequentMapFieldTerrainType(MapLevel, unchecked((sbyte)Xhx), unchecked((sbyte)Yhx), unchecked((sbyte)-1)));
            return Result;
        }

        // City.pas:2422
        public override Aow.Units.ImageLib.TLibraryImage GetTopBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            Aow.Units.ImageLib.TImageSequenceList Sequences = default;
            var call__218 = GetActiveAction();
            var call__219 = GetTopCityActionImage(call__218);
            Result = call__219;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__220 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__221 = call__220.FImageLibrary.GetSafe(40);
                        Result = call__221;
                    }
                    else
                    {
                        var call__222 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__223 = call__222.FImageLibrary.GetSafe(43);
                        Result = call__223;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed))
            {
                Index = 42;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        var call__224 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__225 = call__224.GetPreviewImage();
                        Result = call__225;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2449
        public override Aow.Units.ImageLib.TLibraryImage GetMiddleBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            Aow.Units.ImageLib.TImageSequenceList Sequences = default;
            var call__226 = GetActiveAction();
            var call__227 = GetMiddleCityActionImage(call__226);
            Result = call__227;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__228 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__229 = call__228.FImageLibrary.GetSafe(41);
                        Result = call__229;
                    }
                    else
                    {
                        var call__230 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__231 = call__230.FImageLibrary.GetSafe(44);
                        Result = call__231;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed))
            {
                Index = 43;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        var call__232 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__233 = call__232.GetPreviewImage();
                        Result = call__233;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2476
        public override Aow.Units.ImageLib.TLibraryImage GetBottomBorderImage()
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            Aow.Units.ImageLib.TImageSequenceList Sequences = default;
            var call__234 = GetActiveAction();
            var call__235 = GetBottomCityActionImage(call__234);
            Result = call__235;
            if (Result == null)
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        var call__236 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__237 = call__236.FImageLibrary.GetSafe(42);
                        Result = call__237;
                    }
                    else
                    {
                        var call__238 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__239 = call__238.FImageLibrary.GetSafe(45);
                        Result = call__239;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed))
            {
                Index = 44;
                Sequences = FResource.ImageSequenceList;
                if (Sequences != null)
                {
                    if (Sequences.GetImageSequence(Index) != null)
                    {
                        var call__240 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__241 = call__240.GetPreviewImage();
                        Result = call__241;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2080
        public override Aow.Rtl.AnsiString GetName()
        {
            Aow.Rtl.AnsiString Result = default;
            Result = FName;
            return Result;
        }

        // City.pas:1623
        public override void SetRazed(bool Value)
        {
            if (Value != GetRazed())
            {
                FProduction.ExecuteDeleteProductions();
                if (Value)
                {
                    SetPlayer(unchecked((sbyte)-1));
                }

                if (Value)
                {
                    FMode = FMode + TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmRazed);
                }
                else
                {
                    FMode = FMode - TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmRazed);
                }

                var call__242 = GetTerrainTypeImage(TerrainType);
                ImageIndex = call__242;
                ResetEnchantments();
                UpdateImages();
                UpdateCrops();
                Update();
                SetWallType(0);
            }
        }

        // City.pas:1618
        public override bool GetRazed()
        {
            bool Result = default;
            Result = FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
            return Result;
        }

        // City.pas:1111
        public override void SetupRazeDefenderAG(Aow.Units.AoWE.TUnitList Armies, Aow.Units.AoWE.TUnitList Units)
        {
            Aow.Units.AoWE.TAIGroup Group = default;
            var call__243 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(8);
            if (call__243 == 0)
            {
                var call__244 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, 5);
                var call__245 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(0, unchecked((sbyte)call__244), Aow.Units.AoWE.TGroupBehavior.gbRefuge);
                Group = call__245;
                Group.AddUnits(Units);
                Group.Release();
            }
            else
            {
                base.SetupRazeDefenderAG(Armies, Units);
            }
        }

        // City.pas:864
        public override Aow.Units.NetworkE.TExecuteResult ExecuteTE(Aow.Units.AoWE.TStructureTE A1, Aow.Units.NetworkE.TTokenExecuter Executer)
        {
            Aow.Units.NetworkE.TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            return Result;
        }

        // City.pas:1134
        public override bool GetDefenseRequirements(sbyte Player, ref Aow.Units.AoWE.TDefenseRequirements Requirements)
        {
            bool Result = default;
            Aow.Units.AoWE.TBattleFieldInfo Info = default;
            int Maximum = default;
            int Minimum = default;
            Aow.Units.AoWE.TStatus Relation = default;
            if ((FMode & TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode, Aow.Units.City.TCityModeElement.cmRazed)) == TSet<Aow.Units.City.TCityModeElement>.Of())
            {
                Aow.Units.AoWE.AoWE.SetupBattleFieldInfo(ref Info, unchecked((sbyte)Player), 0, TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(), Aow.Units.AoWE.AoWE.dtAll - TSet<Aow.Units.AoWE.TDamageType>.Of(Aow.Units.AoWE.TDamageType.dtWall));
                Info.Requests = Info.Requests + TSet<Aow.Units.AoWE.TBattleFieldInfoRequestsElement>.Of(Aow.Units.AoWE.TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
                var call__246 = GetLhx();
                var call__247 = GetYhx();
                var call__248 = GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(call__248, call__247, call__246, 10, ref Info);
                Requirements.Strength = Info.Strength;
                Maximum = Aow.Rtl.Rtl.Round(GetSize() * 5000);
                var call__249 = GetSize();
                var call__250 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                Minimum = Aow.Rtl.Rtl.Round((call__249 * 5000) * call__250.FAIControl.FDefenseStrengthMultiplier);
                if (Player != 0)
                {
                    var call__251 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__252 = call__251.GetPlayerRelation(unchecked((sbyte)Player));
                    Relation = call__252;
                }
                else
                {
                    Relation = Aow.Units.AoWE.TStatus.sFriendly;
                }

                // { Original condition makes this neutral-player adjustment unreachable. }
                if ((TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary, Aow.Units.AoWE.TStatus.sNeutral).Contains(Relation) && (Player == FPlayer)) && (FPlayer == 0))
                {
                    Maximum = Aow.Rtl.Rtl.Round(Maximum) + 7500;
                    Minimum = Maximum;
                }

                if (Requirements.Strength < Minimum)
                {
                    Requirements.Strength = Minimum;
                }

                if (Maximum < Requirements.Strength)
                {
                    Requirements.Strength = Maximum;
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1123
        public override Aow.Units.AoWE.TDefensePriority GetDefensePriority()
        {
            Aow.Units.AoWE.TDefensePriority Result = default;
            if ((FMode & TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode, Aow.Units.City.TCityModeElement.cmRazed)) == TSet<Aow.Units.City.TCityModeElement>.Of())
            {
                if (GetSize() == 1)
                {
                    Result = Aow.Units.AoWE.TDefensePriority.dpLow;
                }
                else
                {
                    Result = Aow.Units.AoWE.TDefensePriority.dpAverage;
                }

                if ((FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing) && (Result == Aow.Units.AoWE.TDefensePriority.dpLow))
                {
                    Result = Aow.Units.AoWE.TDefensePriority.dpAverage;
                }

                bool bool__253 = (FPlayer != 0);
                if (bool__253)
                {
                    var call__254 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__255 = call__254.GetPlayerRelation(unchecked((sbyte)FPlayer));
                    bool__253 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(call__255);
                }

                if (bool__253)
                {
                    Result = Aow.Units.AoWE.TDefensePriority.dpHigh;
                }
            }
            else
            {
                Result = Aow.Units.AoWE.TDefensePriority.dpNone;
            }

            return Result;
        }

        // City.pas:1713
        public override void NewDay()
        {
            int I = default;
            base.NewDay();
            if (FRace < 0)
            {
                if ((FRace < -1) && (Aow.Units.AoWE.AoWE.AoWHSMap.FGameType == Aow.Units.AoWE.TGameType.gtCampaign))
                {
                    I = System.Math.Abs(FRace) - 3;
                    if ((I >= 0) && (I < Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FRaceHistory.Count))
                    {
                        var call__256 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FRaceHistory[I];
                        SetRace(unchecked((sbyte)call__256));
                    }
                    else
                    {
                        var call__257 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRandomRace();
                        SetRace(unchecked((sbyte)call__257));
                    }
                }
                else
                {
                    var call__258 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRandomRace();
                    SetRace(unchecked((sbyte)call__258));
                }
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1)
            {
                FProduction.FUnitProductions.Clear();
                SetUpgradeLevel(unchecked((byte)FUpgradeLevel));
                GenerateDefenders();
                if (FWallType == 3)
                {
                    var call__259 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                    FWallType = unchecked((byte)call__259);
                    UpdateImages();
                }
            }

            CheckRebellion();
            if (FOverlay.FMode == Aow.Units.City.TCityOverlayMode.omShowRazeAnimation)
            {
                FOverlay.SetMode(Aow.Units.City.TCityOverlayMode.omIdle);
            }
        }

        // City.pas:1705
        public override void NewTurn(sbyte Player)
        {
            base.NewTurn(unchecked((sbyte)Player));
            if (!FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
            {
                FProduction.NewTurn(unchecked((sbyte)Player));
            }

            if (Player == FPlayer)
            {
                var call__260 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__260.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
                {
                    UpdateAIMigration();
                }
            }
        }

        // City.pas:1335
        public override void ArmyPlaced(Aow.Units.AoWE.TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife))
            {
                ExecuteFountainOfLife();
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
            {
                FOverlay.PlayFadeShellAnimation();
            }
        }

        // City.pas:1342
        public override void ArmyRemoved(Aow.Units.AoWE.TArmyHS Army)
        {
            base.ArmyRemoved(Army);
            UpdatePlayer();
        }

        // City.pas:886
        public override void BuildingDone()
        {
            FUpgradeLevel = 1;
            FWallType = 0;
            var call__261 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FBuildingPlayer);
            SetRace(unchecked((sbyte)call__261.FPlayerRace));
            base.BuildingDone();
        }

        // City.pas:1604
        public override bool GenerateRazeDefenders(sbyte Player, Aow.Units.AoWE.TUnitList Units)
        {
            bool Result = default;
            GenerateRebelUnits(Units);
            Result = true;
            return Result;
        }

        // City.pas:1610
        public override bool ExecuteRaze(sbyte Player)
        {
            bool Result = default;
            Result = base.ExecuteRaze(unchecked((sbyte)Player));
            if (Result)
            {
                var call__262 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__263 = call__262.GetPlayerRelationValue(unchecked((sbyte)Player));
                var call__264 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                call__264.SetPlayerRelationValue(unchecked((sbyte)Player), call__263 + -30);
            }

            return Result;
        }

        // City.pas:2611
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__265 = FProduction.FQueue.Count;
            if (call__265 == 0)
            {
                var call__266 = Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr;
                var call__267 = Aow.Units.AoWE.AoWE.TranslateRStr(call__266);
                Result = call__267;
            }
            else
            {
                var call__268 = FProduction.FQueue.GetItem(0);
                var call__269 = FProduction.GetProductionNameProgressive(call__268);
                Result = call__269;
            }

            return Result;
        }

        // City.pas:876
        public override bool ValidateMap(Aow.Rtl.TStrings A1)
        {
            bool Result = default;
            Result = base.ValidateMap(A1);
            var call__270 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NonameRStr);
            if ((Aow.Rtl.Rtl.CompareText(FName, call__270) == 0) || (FName == ((AnsiString)"")))
            {
                var call__271 = Aow.Units.City.City.CityWarningRStr;
                var call__272 = Aow.Units.AoWE.AoWE.TranslateRStr(call__271);
                var call__273 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoNameSpecifiedRStr);
                A1.Add((call__272 + ((AnsiString)": ")) + call__273);
                Result = false;
            }

            return Result;
        }

        // City.pas:1162
        public override void UpdateAITarget(in Aow.Units.AoWE.TAITargetInfo Info, ref Aow.Units.AoWE.TAITargetData Data)
        {
            int Amount = default;
            int Priority = default;
            if (!FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed))
            {
                switch (Info.Mode)
                {
                    case Aow.Units.AoWE.TAITargetMode.tmAttack:
                    case Aow.Units.AoWE.TAITargetMode.tmRaid:
                        if (Info.Player != FPlayer)
                        {
                            Priority = Data.Priority;
                            if (FPlayer > 0)
                            {
                                var call__274 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), unchecked((sbyte)FRace));
                                if (call__274)
                                {
                                    Priority = 100 + ((GetSize() - 1) * 10);
                                }
                            }
                            else
                            {
                                if (FPlayer == 0)
                                {
                                    var call__275 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                                    var call__276 = call__275.GetPlayerRelation(unchecked((sbyte)Info.Player));
                                    if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__276))
                                    {
                                        var call__277 = JoinAmount(unchecked((sbyte)Info.Player));
                                        Amount = call__277;
                                        if (Amount != -1)
                                        {
                                            Data.Cost = Amount;
                                            Priority = 100 + ((GetSize() - 1) * 10);
                                            Data.TargetType = Aow.Units.AoWE.TAITargetType.attNegotiate;
                                        }
                                        else
                                        {
                                            Priority = 100 + ((GetSize() - 1) * 10);
                                        }
                                    }
                                    else
                                    {
                                        Priority = 100 + ((GetSize() - 1) * 10);
                                    }
                                }
                                else
                                {
                                    if (FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode) && (Priority != 0))
                                    {
                                        Priority = 100 + ((GetSize() - 1) * 10);
                                    }
                                }
                            }

                            if (Priority > Data.Priority)
                            {
                                Data.Priority = Priority;
                            }
                        }

                        break;
                    case Aow.Units.AoWE.TAITargetMode.tmProduce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case Aow.Units.AoWE.TAITargetMode.tmFortify:
                        bool bool__278 = (Info.Player == FPlayer);
                        if (bool__278)
                        {
                            var call__279 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                            var call__280 = call__279.GetPlayerRelation(unchecked((sbyte)Info.Player));
                            bool__278 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__280);
                        }

                        bool bool__281 = bool__278;
                        if (bool__281)
                        {
                            var call__282 = GetActiveAction();
                            bool__281 = (call__282 != Aow.Units.City.TCityAction.caMigrate);
                        }

                        if (bool__281)
                        {
                            var call__283 = GetSize();
                            Data.Priority = (500 + (call__283 * 100)) + (FWallType * 45);
                            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
                            {
                                Data.Priority += 150;
                            }

                            if (GetLhx() != 0)
                            {
                                Data.Priority += 50;
                            }
                        }

                        break;
                    case Aow.Units.AoWE.TAITargetMode.tmDefend:
                        if (Info.Player == FPlayer)
                        {
                            var call__284 = GetSize();
                            Data.Priority = 50 + (call__284 * 20);
                        }

                        break;
                }
            }
        }

        // City.pas:867
        public override void Update()
        {
            if (FUpdateCount == 0)
            {
                UpdateIncome();
                base.Update();
            }
        }

        // City.pas:852
        public override bool CanDblClick()
        {
            bool Result = default;
            bool bool__285 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == FPlayer);
            if (bool__285)
            {
                var call__286 = GetRazed();
                bool__285 = !call__286;
            }

            bool bool__287 = bool__285;
            if (bool__287)
            {
                var call__288 = GetBuilding();
                bool__287 = !call__288;
            }

            Result = bool__287;
            return Result;
        }

        // City.pas:1639
        public override void GetRebuildInfo(ref int Cost, ref int Time)
        {
            Cost = GetSize() * 25;
            Time = GetSize() + 1;
        }

        // City.pas:1092
        public override void SetPlayer(sbyte Player)
        {
            if (Player != FPlayer)
            {
                FProduction.PlayerChanged();
                ResetEnchantments();
                if (FPlayer != -1)
                {
                    var call__289 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__289.FProductionControls.Remove(FProduction);
                }

                base.SetPlayer(unchecked((sbyte)Player));
                if (GetActiveAction() == Aow.Units.City.TCityAction.caMigrate)
                {
                    FProduction.ExecuteCancelProduction();
                    var call__290 = FProduction.FQueue.GetItem(0);
                    call__290.Free();
                }

                ValidateAICityAction();
                UpdateAIMigration();
                if (FPlayer != -1)
                {
                    var call__291 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__291.FProductionControls.Add(FProduction);
                }
            }
        }

        // City.pas:2071
        public override void UpdateFlagID()
        {
            bool bool__292 = !FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moEditMode);
            if (bool__292)
            {
                bool bool__293 = !FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
                if (!bool__293)
                {
                    var call__294 = GetBuilding();
                    bool__293 = call__294;
                }

                bool__292 = bool__293;
            }

            if (bool__292 && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
            {
                if (FPlayer > 0)
                {
                    var call__295 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    FFlagID = unchecked((byte)(call__295.FPlayerRace + ((FUpgradeLevel - 1) * 64)));
                }
                else
                {
                    FFlagID = unchecked((byte)(32 + ((FUpgradeLevel - 1) * 64)));
                }
            }
            else
            {
                FFlagID = 63;
            }
        }

        // City.pas:1792
        public override int VisibilityRange()
        {
            int Result = default;
            Result = 6;
            var call__296 = GetLhx();
            bool bool__297 = (call__296 != 0);
            if (!bool__297)
            {
                bool bool__298 = (FPlayer > 0);
                if (bool__298)
                {
                    var call__299 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    bool__298 = ((((byte)call__299.FMagicControl.FEnemyEnchantments.Image) & 8) != 0);
                }

                bool__297 = bool__298;
            }

            if (bool__297)
            {
                Result = (int)(((uint)(Result + 1)) >> 1);
            }

            return Result;
        }

        // City.pas:2225
        public override void FinalizeCombat(Aow.Units.AoWE.TCombat Combat)
        {
            // { Unmatched: tried Pointer identity assignment to Combat; explicit combat local; zero-shift combat identity assignment (5 variants total). }
            if ((Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crVictory) || (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crNoDefenders))
            {
                {
                    var w__300 = Combat.FCombatData;
                    var call__301 = w__300.FPlayers.GetPlayers(unchecked((sbyte)w__300.FAttackerPlayer));
                    SetPlayer(unchecked((sbyte)call__301.FPlayer));
                }

                UpdatePlayer();
            }
        }

        // City.pas:2214
        public override Aow.Rtl.AnsiString GetCombatEventText(Aow.Units.AoWE.TCombat Combat)
        {
            Aow.Rtl.AnsiString Result = default;
            if (!GetRazed())
            {
                if ((Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crVictory) || (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crNoDefenders))
                {
                    var call__302 = Aow.Units.AoWE.AoWE.CityBattleWonRStr;
                    var call__303 = Aow.Units.AoWE.AoWE.TranslateRStr(call__302);
                    var call__304 = GetName();
                    var call__305 = Aow.Rtl.Rtl.Format(call__303, [call__304]);
                    Result = call__305;
                }
                else
                {
                    if (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crDefeat)
                    {
                        var call__306 = Aow.Units.AoWE.AoWE.CityBattleLostRStr;
                        var call__307 = Aow.Units.AoWE.AoWE.TranslateRStr(call__306);
                        var call__308 = GetName();
                        var call__309 = Aow.Rtl.Rtl.Format(call__307, [call__308]);
                        Result = call__309;
                    }
                    else
                    {
                        if (Combat.FCombatResult != Aow.Units.AoWE.TCombatResult.crCannotBreachWall)
                        {
                            var call__310 = Aow.Units.AoWE.AoWE.CityBattleDrawnRStr;
                            var call__311 = Aow.Units.AoWE.AoWE.TranslateRStr(call__310);
                            var call__312 = GetName();
                            var call__313 = Aow.Rtl.Rtl.Format(call__311, [call__312]);
                            Result = call__313;
                        }
                    }
                }
            }

            return Result;
        }

        // City.pas:1062
        public override void StructureCaptured(sbyte OldPlayer, sbyte NewPlayer)
        {
            Aow.Units.City.TCityProductionEventLog Log = default;
            var call__314 = GetRazed();
            bool bool__315 = !call__314;
            if (bool__315)
            {
                var call__316 = GetBuilding();
                bool__315 = !call__316;
            }

            if ((((bool__315 && (OldPlayer == 0)) && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting)) && FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated)) && !((Aow.Units.City.TCityResource)FResource).FClaimed)
            {
                Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                Log.FPlayer = unchecked((sbyte)NewPlayer);
                var call__317 = EditName();
                var call__318 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StructureCapturedRStr);
                var call__319 = Aow.Rtl.Rtl.Format(call__318, [call__317]);
                Log.FText = call__319;
                var call__320 = GetXhx();
                Log.FXhx = unchecked((sbyte)call__320);
                var call__321 = GetYhx();
                Log.FYhx = unchecked((sbyte)call__321);
                var call__322 = GetLhx();
                Log.FLhx = unchecked((sbyte)call__322);
                var call__323 = GetYhx();
                var call__324 = GetLhx();
                var call__325 = GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__325), unchecked((sbyte)call__323), unchecked((sbyte)call__324), false);
                Log.Release();
            }
        }

        // City.pas:2271
        public override Aow.Units.AoWE.TMoveAction GetMoveAction(sbyte Player, Aow.Units.AoWE.TMoveAction Action)
        {
            Aow.Units.AoWE.TMoveAction Result = default;
            Result = Action;
            if (FPlayer == 0)
            {
                bool bool__326 = TSet<Aow.Units.AoWE.TMoveAction>.Of(Aow.Units.AoWE.TMoveAction.maNormal, Aow.Units.AoWE.TMoveAction.maAttack).Contains(Action);
                if (bool__326)
                {
                    var call__327 = GetRazed();
                    bool__326 = !call__327;
                }

                if (bool__326 && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
                {
                    if (JoinAmount(unchecked((sbyte)Player)) != -1)
                    {
                        Result = Aow.Units.AoWE.TMoveAction.maNegotiate;
                    }
                    else
                    {
                        Result = Aow.Units.AoWE.TMoveAction.maAttack;
                    }
                }
            }
            else
            {
                if ((FPlayer != Player) && (FPlayer != -1))
                {
                    Result = Aow.Units.AoWE.TMoveAction.maAttack;
                }
            }

            return Result;
        }

        // City.pas:1285
        public override bool UpdatePlayer()
        {
            bool Result = default;
            bool MultiplePlayers = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int Count = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            sbyte Player = default;
            bool bool__328 = FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
            if (bool__328)
            {
                var call__329 = GetBuilding();
                bool__328 = !call__329;
            }

            if (!bool__328)
            {
                MultiplePlayers = false;
                Player = unchecked((sbyte)-1);
                Count = GetTerrainCount();
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while ((Walk.HN < Count) && !MultiplePlayers)
                {
                    var call__330 = FResource.TerrainList.GetTerrain(Walk.HN);
                    if (((((call__330 >= 0) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                    {
                        var call__331 = MapLevel.GetField(Walk.X, Walk.Y);
                        var call__332 = call__331.FindOwnedChild(131607);
                        Army = (Aow.Units.AoWE.TArmyHS)call__332;
                        if (Army != null)
                        {
                            if (Player == -1)
                            {
                                Player = unchecked((sbyte)Army.FArmy.FPlayer);
                            }
                            else
                            {
                                MultiplePlayers = Army.FArmy.FPlayer != Player;
                            }
                        }
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }

                if (MultiplePlayers)
                {
                    if (!FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
                    {
                        FMode = FMode + TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode);
                        SetPlayer(unchecked((sbyte)-1));
                        UpdateFlagID();
                        Update();
                    }
                }
                else
                {
                    if (FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
                    {
                        FMode = FMode - TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode);
                        if (Player != -1)
                        {
                            SetPlayer(unchecked((sbyte)Player));
                        }
                        else
                        {
                            SetPlayer(0);
                        }

                        UpdateFlagID();
                        Update();
                    }
                    else
                    {
                        if (Player != -1)
                        {
                            SetPlayer(unchecked((sbyte)Player));
                        }
                    }
                }
            }

            Result = true;
            return Result;
        }

        // City.pas:1777
        public override void UpdateIncome()
        {
            var call__333 = GetRazed();
            bool bool__334 = !call__333;
            if (bool__334)
            {
                var call__335 = GetActiveAction();
                bool__334 = (call__335 != Aow.Units.City.TCityAction.caMigrate);
            }

            bool bool__336 = bool__334;
            if (bool__336)
            {
                var call__337 = GetBuilding();
                bool__336 = !call__337;
            }

            if (bool__336)
            {
                var call__338 = GetCropCount();
                var call__339 = GetSize();
                SetIncome(unchecked((byte)((call__338 * 2) + (call__339 * 5))));
                if (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(GetActiveAction()))
                {
                    SetIncome(unchecked((byte)(FIncome + Aow.Rtl.Rtl.Round(FIncome * 0.25))));
                }

                if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
                {
                    SetIncome(unchecked((byte)(FIncome * 2)));
                }
            }
            else
            {
                SetIncome(0);
            }
        }

        // City.pas:1741
        public override void UpdateCrops()
        {
            int Count = default;
            if (!GetEditMode())
            {
                if (GetRazed())
                {
                    Count = 0;
                }
                else
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting))
                    {
                        Count = ((Aow.Units.City.TCityResource)FResource).FCropLocations.Count;
                    }
                    else
                    {
                        Count = -1;
                    }
                }

                if (Count != -1)
                {
                    SetCropCount(Count);
                }

                UpdateCropSettings();
                UpdateIncome();
            }
        }

        // City.pas:2108
        public override sbyte GetRace()
        {
            sbyte Result = default;
            Result = unchecked((sbyte)FRace);
            return Result;
        }

        // City.pas:1211
        public override void CropTrampled(sbyte Player)
        {
            int Race = default;
            if ((Player != FPlayer) && (Player != 0))
            {
                if (FPlayer > 0)
                {
                    var call__340 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    Race = call__340.FPlayerRace;
                }
                else
                {
                    Race = FRace;
                }

                var call__341 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                var call__342 = call__341.GetPlayerRelationValue(unchecked((sbyte)Player));
                var call__343 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                call__343.SetPlayerRelationValue(unchecked((sbyte)Player), call__342 + -5);
            }
        }

        // City.pas:2097
        public virtual void SetRace(sbyte Race)
        {
            if (Race != FRace)
            {
                FRace = unchecked((sbyte)Race);
                if (FRace >= 0)
                {
                    SetDefaultProductions();
                }

                UpdateCrops();
                UpdateImages();
            }
        }

        // City.pas:857
        public Aow.Units.City.TCityAction GetActiveAction()
        {
            Aow.Units.City.TCityAction Result = default;
            if (FProduction.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing)
            {
                var call__344 = FProduction.FQueue.GetItem(0);
                var call__345 = Aow.Units.City.City.ProductionIDToCityAction(call__344.FProductionID);
                Result = call__345;
            }
            else
            {
                Result = Aow.Units.City.TCityAction.caNone;
            }

            return Result;
        }

        // City.pas:894
        public void UpdateActiveEnchantments()
        {
            int I = default;
            FEnchantments = TSet<Aow.Units.City.TCityEnchantmentID>.Of();
            {
                I = FEnchantmentList.Count - 1;
                byte end__346 = 0;
                for (; I >= end__346; I -= 1)
                {
                    var call__347 = FEnchantmentList.GetEnchantments(I);
                    FEnchantments = FEnchantments + TSet<Aow.Units.City.TCityEnchantmentID>.Of(((Aow.Units.City.TCityEnchantment)call__347).FEnchantmentID);
                }
            }
        }

        // City.pas:902
        public void RegisterEnchantments()
        {
            int I = default;
            {
                I = FEnchantmentList.Count - 1;
                byte end__348 = 0;
                for (; I >= end__348; I -= 1)
                {
                    var call__349 = FEnchantmentList.GetEnchantments(I);
                    var call__350 = FEnchantmentList.GetEnchantments(I);
                    call__350.Activate(unchecked((sbyte)call__349.FPlayerIndex));
                }
            }

            UpdateActiveEnchantments();
        }

        // City.pas:910
        public void UnregisterEnchantments()
        {
            int I = default;
            {
                I = FEnchantmentList.Count - 1;
                byte end__351 = 0;
                for (; I >= end__351; I -= 1)
                {
                    var call__352 = FEnchantmentList.GetEnchantments(I);
                    var call__353 = FEnchantmentList.GetEnchantments(I);
                    call__353.Deactivate(unchecked((byte)call__352.FPlayerIndex));
                }
            }
        }

        // City.pas:917
        public void ResetEnchantments()
        {
            UnregisterEnchantments();
            FEnchantmentList.Clear();
            FEnchantments = TSet<Aow.Units.City.TCityEnchantmentID>.Of();
        }

        // City.pas:924
        public int AICityEnchantmentUpkeepPriority(Aow.Units.City.TCityEnchantmentID ID)
        {
            int Result = default;
            switch (ID)
            {
                case Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife:
                    Result = (GetSize() * 25) + 600;
                    break;
                case Aow.Units.City.TCityEnchantmentID.ceiGoldRush:
                    Result = (GetSize() * 25) + 600;
                    break;
                case Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell:
                    Result = (GetSize() * 25) + 700;
                    break;
                case Aow.Units.City.TCityEnchantmentID.ceiWarmonger:
                    Result = (GetSize() * 25) + 600;
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // City.pas:935
        public void ActivateEnchantment(Aow.Units.City.TCityEnchantment Enchantment)
        {
            Enchantment.Owner = FEnchantmentList;
            Enchantment.Activate(unchecked((sbyte)Enchantment.FPlayerIndex));
            FEnchantments = FEnchantments + TSet<Aow.Units.City.TCityEnchantmentID>.Of(Enchantment.FEnchantmentID);
            if (Enchantment.FEnchantmentID == Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife)
            {
                ExecuteFountainOfLife();
            }

            if (Enchantment.FEnchantmentID == Aow.Units.City.TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            if (Enchantment.FEnchantmentID == Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell)
            {
                FOverlay.PlaySetupShellAnimation();
            }
        }

        // City.pas:945
        public void DeactivateEnchantment(Aow.Units.City.TCityEnchantment Enchantment)
        {
            Enchantment.Owner = null;
            Enchantment.Deactivate(unchecked((byte)Enchantment.FPlayerIndex));
            UpdateActiveEnchantments();
            if (Enchantment.FEnchantmentID == Aow.Units.City.TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            Enchantment.Release();
            Update();
        }

        // City.pas:955
        public void ListEnhancementsInfo(Aow.Rtl.TStrings List)
        {
            var call__354 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradeLevelXRStr);
            var call__355 = Aow.Rtl.Rtl.Format(call__354, [Aow.Rtl.Rtl.IntToStr(FUpgradeLevel)]);
            List.Add(call__355);
            if (FWallType == 1)
            {
                var call__356 = Aow.Units.City.City.CityWoodenWallRStr;
                var call__357 = Aow.Units.AoWE.AoWE.TranslateRStr(call__356);
                List.Add(call__357);
            }

            if (FWallType == 2)
            {
                var call__358 = Aow.Units.City.City.CityStoneWallRStr;
                var call__359 = Aow.Units.AoWE.AoWE.TranslateRStr(call__358);
                List.Add(call__359);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife))
            {
                var call__360 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FountainOfLifeRStr);
                var call__361 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__360 + ((AnsiString)": ")) + call__361);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
            {
                var call__362 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoldRushRStr);
                var call__363 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__362 + ((AnsiString)": ")) + call__363);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
            {
                var call__364 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AntiMagicShellRStr);
                var call__365 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__364 + ((AnsiString)": ")) + call__365);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiWarmonger))
            {
                var call__366 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WarmongerRStr);
                var call__367 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__366 + ((AnsiString)": ")) + call__367);
            }
        }

        // City.pas:970
        public void StartTownQuake()
        {
            FMode = FMode + TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmTownQuake);
        }

        // City.pas:973
        public void StopTownQuake()
        {
            FMode = FMode - TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmTownQuake);
        }

        // City.pas:976
        public void ExecuteAnarchy()
        {
            FMode = FMode + TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmAnarchy);
            CheckRebellion();
            FMode = FMode - TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmAnarchy);
        }

        // City.pas:983
        public bool ExecuteFountainOfLife()
        {
            bool Result = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            int I = default;
            Aow.Units.Animation.TImageSequenceAnimation Animation = default;
            bool PlaySound = default;
            Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
            ListArmies(Armies);
            Result = false;
            PlaySound = false;
            {
                I = 0;
                int end__368 = Armies.Count - 1;
                for (; I <= end__368; I += 1)
                {
                    var call__369 = Armies.GetArmyHS(I);
                    var call__370 = call__369.FArmy.HealUnits(255);
                    if (call__370)
                    {
                        var call__371 = Armies.GetArmyHS(I);
                        var call__372 = call__371.GetLhx();
                        var call__373 = Armies.GetArmyHS(I);
                        var call__374 = call__373.GetYhx();
                        var call__375 = Armies.GetArmyHS(I);
                        var call__376 = call__375.GetXhx();
                        var call__377 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__376, call__374, call__372, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        if (call__377)
                        {
                            Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                            var call__378 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                            var call__379 = call__378.FImageSequences.ImageSequences[0];
                            Animation.SetImageSequence(call__379);
                            Animation.FAnimationLevel = 27;
                            var call__380 = Armies.GetArmyHS(I);
                            var call__381 = call__380.GetLhx();
                            var call__382 = Armies.GetArmyHS(I);
                            var call__383 = call__382.GetYhx();
                            var call__384 = Armies.GetArmyHS(I);
                            var call__385 = call__384.GetXhx();
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, call__385, call__383, call__381);
                            bool bool__386 = PlaySound;
                            if (!bool__386)
                            {
                                var call__387 = Armies.GetArmyHS(I);
                                bool__386 = (call__387.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            }

                            bool bool__388 = bool__386;
                            if (!bool__388)
                            {
                                var call__389 = Armies.GetArmyHS(I);
                                var call__390 = call__389.GetLhx();
                                var call__391 = Armies.GetArmyHS(I);
                                var call__392 = call__391.GetYhx();
                                var call__393 = Armies.GetArmyHS(I);
                                var call__394 = call__393.GetXhx();
                                var call__395 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__394), unchecked((sbyte)call__392), unchecked((sbyte)call__390), 1);
                                bool__388 = call__395;
                            }

                            PlaySound = bool__388;
                        }

                        Result = true;
                    }
                }
            }

            if (PlaySound)
            {
                var call__396 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                call__396.FSFXLibrary.Play(0);
            }

            Armies.Free();
            return Result;
        }

        // City.pas:1015
        public void UpdateAIMigration()
        {
            Aow.Units.City.TMigrationList Selection = default;
            int I = default;
            int Value = default;
            int BestIndex = default;
            int BestValue = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            /* TODO(T-N3) Untyped pointer dereference requires a typed storage override */
            throw new System.NotImplementedException();
        }

        // City.pas:1081
        public void ValidateAICityAction()
        {
            bool Cancel = default;
            bool bool__397 = (FPlayer > 0);
            if (bool__397)
            {
                var call__398 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                bool__397 = (call__398.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
            }

            bool bool__399 = bool__397 && ((TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode, Aow.Units.City.TCityModeElement.cmRazed) & FMode) == TSet<Aow.Units.City.TCityModeElement>.Of());
            if (bool__399)
            {
                var call__400 = GetActiveAction();
                bool__399 = !TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__400);
            }

            if (bool__399)
            {
                var call__401 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__402 = call__401.GetPlayerRelation(unchecked((sbyte)FPlayer));
                bool bool__403 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary, Aow.Units.AoWE.TStatus.sNeutral).Contains(call__402);
                if (bool__403)
                {
                    var call__404 = GetActiveAction();
                    bool bool__405 = (call__404 == Aow.Units.City.TCityAction.caMigrate);
                    if (bool__405)
                    {
                        var call__406 = FProduction.FQueue.GetItem(0);
                        var call__407 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__406.FParameter);
                        var call__408 = call__407.GetPlayerRelation(unchecked((sbyte)FPlayer));
                        bool__405 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__408);
                    }

                    bool__403 = !bool__405;
                }

                Cancel = bool__403;
                if (Cancel)
                {
                    FProduction.ExecuteDeleteProductions();
                }
            }
        }

        // City.pas:1222
        public bool MoveArmiesOutOfCity(TSet<Aow.Units.City.TMoveOutOfCitySelectionElement> Selection)
        {
            bool Result = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            int Count = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Result = true;
            Count = GetTerrainCount();
            MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
            // { This eliminated copy preserves the original saved registers. }
            ;
            Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while ((Walk.HN < Count) && Result)
            {
                var call__409 = FResource.TerrainList.GetTerrain(Walk.HN);
                if (((((call__409 >= 0) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                {
                    var call__410 = MapLevel.GetField(Walk.X, Walk.Y);
                    var call__411 = call__410.FindOwnedChild(131607);
                    Army = (Aow.Units.AoWE.TArmyHS)call__411;
                    if ((Army != null) && (!Selection.Contains(Aow.Units.City.TMoveOutOfCitySelectionElement.msSkipOwnPlayer) || (Army.FArmy.FPlayer != FPlayer)))
                    {
                        var call__412 = GetLhx();
                        var call__413 = Army.PlaceEx(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)call__412), TSet<Aow.Units.AoWE.TArmyPlaceModeElement>.Of(Aow.Units.AoWE.TArmyPlaceModeElement.apSafe, Aow.Units.AoWE.TArmyPlaceModeElement.apScanArea));
                        Result = call__413;
                    }
                }

                Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1256
        public int GetSize()
        {
            int Result = default;
            Result = ((Aow.Units.City.TCityResource)FResource).FSize;
            return Result;
        }

        // City.pas:1259
        public int GetObtainUnitsValue()
        {
            int Result = default;
            int Count = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            {
                ref var w__414 = ref Walk;
                Result = 0;
                Count = GetTerrainCount();
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                var call__415 = GetYhx();
                var call__416 = GetXhx();
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__416, call__415, 0);
                while (w__414.HN < Count)
                {
                    var call__417 = FResource.TerrainList.GetTerrain(w__414.HN);
                    if (((((call__417 >= 0) && (w__414.X >= 0)) && (w__414.Y >= 0)) && (w__414.X < MapLevel.Width)) && (w__414.Y < MapLevel.Height))
                    {
                        var call__418 = MapLevel.GetField(w__414.X, w__414.Y);
                        var call__419 = call__418.FindOwnedChild(131607);
                        Army = (Aow.Units.AoWE.TArmyHS)call__419;
                        if (Army != null)
                        {
                            var call__420 = Army.FArmy.ObtainValue(255);
                            Result += call__420;
                        }
                    }

                    Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                }
            }

            return Result;
        }

        // City.pas:1348
        public int UpgradeCost()
        {
            int Result = default;
            var call__421 = GetSize();
            Result = Aow.Units.City.City.UpgradePrices[call__421 - 1];
            return Result;
        }

        // City.pas:1351
        public int UpgradeTime()
        {
            int Result = default;
            Result = 3;
            return Result;
        }

        // City.pas:1356
        public bool CanUpgrade(bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            int Level = default;
            int I = default;
            Level = FUpgradeLevel;
            if (Queue)
            {
                {
                    I = 0;
                    int end__422 = FProduction.FQueue.Count - 1;
                    for (; I <= end__422; I += 1)
                    {
                        var call__423 = FProduction.FQueue.GetItem(I);
                        var call__424 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                        if (call__423.FProductionID == call__424)
                        {
                            Level += 1;
                        }
                    }
                }
            }

            if (Level < GetSize())
            {
                Result = true;
            }
            else
            {
                Result = false;
                var call__425 = Aow.Units.AoWE.AoWE.MaxUpgradeLevelReachedRStr;
                var call__426 = Aow.Units.AoWE.AoWE.TranslateRStr(call__425);
                Reason = call__426;
            }

            return Result;
        }

        // City.pas:1371
        public bool Upgrade(bool Queue)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__427 = CanUpgrade(Queue, ref Reason);
            bool bool__428 = call__427;
            if (bool__428)
            {
                var call__429 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__430 = call__429.GetBusy();
                bool__428 = !call__430;
            }

            if (bool__428)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__431 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                Item.FProductionID = call__431;
                FProduction.Produce(Item, Queue);
                Item.Free();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1386
        public int FortifyCost()
        {
            int Result = default;
            switch (FWallType)
            {
                case 0:
                    var call__432 = GetSize();
                    Result = Aow.Units.City.City.WoodWallPrices[call__432 - 1];
                    break;
                case 1:
                    var call__433 = GetSize();
                    Result = Aow.Units.City.City.StoneWallPrices[call__433 - 1];
                    break;
                default:
                    Result = -1;
                    break;
            }

            return Result;
        }

        // City.pas:1395
        public int FortifyTime()
        {
            int Result = default;
            Result = 2;
            return Result;
        }

        // City.pas:1400
        public bool CanFortify(bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            int Level = default;
            int I = default;
            Level = FWallType;
            if (Queue)
            {
                {
                    I = 0;
                    int end__434 = FProduction.FQueue.Count - 1;
                    for (; I <= end__434; I += 1)
                    {
                        var call__435 = FProduction.FQueue.GetItem(I);
                        var call__436 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                        if (call__435.FProductionID == call__436)
                        {
                            Level += 1;
                        }
                    }
                }
            }

            if (Level != 2)
            {
                Result = true;
            }
            else
            {
                Result = false;
                var call__437 = Aow.Units.AoWE.AoWE.MaxFortifyLevelReachedRStr;
                var call__438 = Aow.Units.AoWE.AoWE.TranslateRStr(call__437);
                Reason = call__438;
            }

            return Result;
        }

        // City.pas:1415
        public bool Fortify(bool Queue)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__439 = CanFortify(Queue, ref Reason);
            bool bool__440 = call__439;
            if (bool__440)
            {
                var call__441 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__442 = call__441.GetBusy();
                bool__440 = !call__442;
            }

            if (bool__440)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__443 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                Item.FProductionID = call__443;
                FProduction.Produce(Item, Queue);
                Item.Free();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1430
        public int ProductionCost(int Index)
        {
            int Result = default;
            var call__444 = FProduction.UnitProductionGold(Index);
            Result = call__444;
            return Result;
        }

        // City.pas:1435
        public int ProductionTime(int Index)
        {
            int Result = default;
            var call__445 = FProduction.UnitProductionTime(Index);
            Result = call__445;
            return Result;
        }

        // City.pas:1440
        public bool CanProduce(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__446 = FProduction.CanProduceUnit(Index, Queue, ref Reason);
            Result = call__446;
            return Result;
        }

        // City.pas:1445
        public bool Produce(int Index, bool Queue)
        {
            bool Result = default;
            var call__447 = FProduction.ProduceUnit(Index, Queue);
            Result = call__447;
            return Result;
        }

        // City.pas:1450
        public int ResearchCost(int Index)
        {
            int Result = default;
            var call__448 = FProduction.UnitResearchGold(Index);
            Result = call__448;
            return Result;
        }

        // City.pas:1455
        public int ResearchTime(int Index)
        {
            int Result = default;
            var call__449 = FProduction.UnitResearchTime(Index);
            Result = call__449;
            return Result;
        }

        // City.pas:1460
        public bool CanResearch(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__450 = FProduction.CanResearchUnit(Index, Queue, ref Reason);
            Result = call__450;
            return Result;
        }

        // City.pas:1465
        public bool Research(int Index, bool Queue)
        {
            bool Result = default;
            var call__451 = FProduction.ResearchUnit(Index, Queue);
            Result = call__451;
            return Result;
        }

        // City.pas:1470
        public bool CanTrade(bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // City.pas:1475
        public bool Trade(bool Queue)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__452 = CanTrade(Queue, ref Reason);
            bool bool__453 = call__452;
            if (bool__453)
            {
                var call__454 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__455 = call__454.GetBusy();
                bool__453 = !call__455;
            }

            if (bool__453)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__456 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caTrade);
                Item.FProductionID = call__456;
                if (Queue)
                {
                    Item.FParameter = 1;
                }
                else
                {
                    Item.FParameter = -1;
                }

                FProduction.Produce(Item, Queue);
                Item.Free();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1491
        public int LootTime()
        {
            int Result = default;
            Result = GetSize();
            return Result;
        }

        // City.pas:1496
        public bool CanLoot(ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            Result = !FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
            return Result;
        }

        // City.pas:1499
        public bool Loot()
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__457 = CanLoot(ref Reason);
            bool bool__458 = call__457;
            if (bool__458)
            {
                var call__459 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__460 = call__459.GetBusy();
                bool__458 = !call__460;
            }

            if (bool__458)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__461 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caLoot);
                Item.FProductionID = call__461;
                FProduction.Produce(Item, false);
                Item.Free();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1514
        public int MigrationTime(sbyte Race)
        {
            int Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Aow.Units.HSEngine.THNWalkData Walk = default;
            Aow.Units.HSEngine.TMapLevel MapLevel = default;
            int Radius = default;
            Aow.Units.City.TCity City = default;
            if (CanMigrate(unchecked((sbyte)Race), ref Reason))
            {
                if (Race != FRace)
                {
                    Radius = -1;
                    MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                    Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 1);
                    while ((Walk.Radius <= 20) && (Radius == -1))
                    {
                        if ((((Walk.X >= 0) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                        {
                            var call__462 = MapLevel.GetField(Walk.X, Walk.Y);
                            var call__463 = call__462.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            City = (Aow.Units.City.TCity)call__463;
                            if ((City != null) && (City.FRace == Race))
                            {
                                Radius = Walk.Radius;
                            }
                        }

                        Aow.Units.HSEngine.HSEngine.WalkToNextHN(ref Walk);
                    }

                    if (Radius != -1)
                    {
                        Result = (Radius / 10) + 3;
                    }
                    else
                    {
                        Result = 5;
                    }
                }
                else
                {
                    Result = 0;
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        // City.pas:1544
        public bool CanMigrate(sbyte Race, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            int I = default;
            sbyte Player = default;
            if (FRace != Race)
            {
                var call__464 = Aow.Units.City.City.RaceInCity(unchecked((sbyte)Race), unchecked((sbyte)FPlayer));
                Result = call__464;
                var call__465 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count;
                I = call__465;
                while ((I != 1) && !Result)
                {
                    I -= 1;
                    Player = unchecked((sbyte)I);
                    var call__466 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)(Player << 0)), unchecked((sbyte)FPlayer));
                    bool bool__467 = (call__466 == Aow.Units.AoWE.TDiplomaticRelation.drAlliance);
                    if (bool__467)
                    {
                        var call__468 = Aow.Units.City.City.RaceInCity(unchecked((sbyte)Race), unchecked((sbyte)Player));
                        bool__467 = call__468;
                    }

                    Result = bool__467;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1561
        public bool Migrate(sbyte Race)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__469 = CanMigrate(unchecked((sbyte)Race), ref Reason);
            bool bool__470 = call__469;
            if (bool__470)
            {
                var call__471 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__472 = call__471.GetBusy();
                bool__470 = !call__472;
            }

            if (bool__470)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__473 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caMigrate);
                Item.FProductionID = call__473;
                Item.FParameter = Race;
                FProduction.Produce(Item, false);
                Item.Free();
                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:1577
        public Aow.Units.City.TMigrationItem CreateMigrationItem(sbyte Race)
        {
            Aow.Units.City.TMigrationItem Result = default;
            Aow.Rtl.AnsiString Reason = default;
            if (CanMigrate(unchecked((sbyte)Race), ref Reason))
            {
                Result = Aow.Units.City.TMigrationItem.Create(null);
                Result.FRace = unchecked((sbyte)Race);
                var call__474 = MigrationTime(unchecked((sbyte)Race));
                Result.FTime = call__474;
                var call__475 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                var call__476 = call__475.GetPlayerRelation(unchecked((sbyte)FPlayer));
                Result.FRelation = call__476;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // City.pas:1589
        public Aow.Units.City.TMigrationList CreateMigrationSelection()
        {
            Aow.Units.City.TMigrationList Result = default;
            int I = default;
            Aow.Units.City.TMigrationItem Item = default;
            Result = Aow.Units.City.TMigrationList.Create(null);
            {
                I = 0;
                int end__477 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.Count - 1;
                for (; I <= end__477; I += 1)
                {
                    var call__478 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(I);
                    if (call__478.FEnabled)
                    {
                        Item = CreateMigrationItem(unchecked((sbyte)I));
                        if ((Item != null) && TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sNeutral, Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(Item.FRelation))
                        {
                            Item.Owner = Result;
                        }
                        else
                        {
                            Item.Free();
                        }
                    }
                }
            }

            return Result;
        }

        // City.pas:1645
        public void CheckRebellion()
        {
            Aow.Units.City.TCivilStatus Status = default;
            int Chance = default;
            int I = default;
            Aow.Units.City.TCityRebellionEventLog Log = default;
            Aow.Units.AoWE.TUnitList Units = default;
            if (FPlayer != 0)
            {
                Status = GetCivilStatus();
                if (TSet<Aow.Units.City.TCivilStatus>.Of(Aow.Units.City.TCivilStatus.csUnruly, Aow.Units.City.TCivilStatus.csUnrest).Contains(Status) && !GetRazed())
                {
                    if (Status == Aow.Units.City.TCivilStatus.csUnruly)
                    {
                        Chance = 50;
                    }
                    else
                    {
                        Chance = 10;
                    }

                    var call__479 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                    if (Chance >= call__479)
                    {
                        MoveArmiesOutOfCity(TSet<Aow.Units.City.TMoveOutOfCitySelectionElement>.Of());
                        Log = Aow.Units.City.TCityRebellionEventLog.Create(null);
                        Log.FPlayer = unchecked((sbyte)FPlayer);
                        var call__480 = GetName();
                        Log.FCityName = call__480;
                        var call__481 = GetXhx();
                        Log.FXhx = call__481;
                        var call__482 = GetYhx();
                        Log.FYhx = call__482;
                        var call__483 = GetLhx();
                        Log.FLhx = call__483;
                        var call__484 = GetYhx();
                        var call__485 = GetLhx();
                        var call__486 = GetXhx();
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__486), unchecked((sbyte)call__484), unchecked((sbyte)call__485), false);
                        Log.Release();
                        SetPlayer(0);
                        Units = Aow.Units.AoWE.TUnitList.Create(null);
                        GenerateRebelUnits(Units);
                        {
                            I = Units.Count - 1;
                            byte end__487 = 0;
                            for (; I >= end__487; I -= 1)
                            {
                                var call__488 = GetLhx();
                                var call__489 = GetYhx();
                                var call__490 = GetXhx();
                                var call__491 = Units.GetUnit(I);
                                call__491.Place(call__490, call__489, call__488, 0, TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of());
                            }
                        }

                        Units.Free();
                        FProduction.ExecuteDeleteProductions();
                    }
                }
            }
        }

        // City.pas:1681
        public void GenerateDefenders()
        {
            int Value = default;
            int I = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            var call__492 = GetRazed();
            if (((!call__492 && (FDefendersMode != Aow.Units.City.TDefendersGenerateMode.dgNone)) && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode)) && (FPlayer == 0))
            {
                Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                ListArmies(Armies);
                if (FDefendersMode == Aow.Units.City.TDefendersGenerateMode.dgRandom)
                {
                    var call__493 = Aow.Rtl.Rtl.Random(3);
                    FDefendersMode = (Aow.Units.City.TDefendersGenerateMode)(call__493 + 2);
                }

                Value = ((((int)FDefendersMode) - 2) * 25) + 25;
                Value += ((GetSize() - 1) * Value) / 2;
                {
                    I = 0;
                    int end__494 = Armies.Count - 1;
                    for (; I <= end__494; I += 1)
                    {
                        var call__495 = Armies.GetArmyHS(I);
                        var call__496 = call__495.FArmy.ObtainValue(255);
                        Value -= call__496;
                    }
                }

                if (Value > 0)
                {
                    var call__497 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(1048575);
                    Aow.Rtl.Rtl.RandSeed = call__497;
                    var call__498 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                    var call__499 = GetLhx();
                    var call__500 = GetYhx();
                    var call__501 = GetXhx();
                    Aow.Units.AoWE.AoWE.PlaceRandomUnits(call__501, call__500, call__499, 0, call__498.FUnitIndexes, FUpgradeLevel, Value, TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature));
                }

                Armies.Free();
            }
        }

        // City.pas:1755
        public void ListIncomeInfo(Aow.Rtl.TStrings Names, Aow.Rtl.TStrings Values)
        {
            int Income = default;
            Income = GetSize() * 5;
            var call__502 = Aow.Units.AoWE.AoWE.BaseIncomeRStr;
            var call__503 = Aow.Units.AoWE.AoWE.TranslateRStr(call__502);
            Names.Add(call__503);
            Values.Add(Aow.Rtl.Rtl.IntToStr(Income));
            Income += GetCropCount() * 2;
            var call__504 = GetCropCount();
            var call__505 = Aow.Units.AoWE.AoWE.XCropFieldsRStr;
            var call__506 = Aow.Units.AoWE.AoWE.TranslateRStr(call__505);
            var call__507 = Aow.Rtl.Rtl.Format(call__506, [Aow.Rtl.Rtl.IntToStr(call__504)]);
            Names.Add(call__507);
            var call__508 = GetCropCount();
            Values.Add(Aow.Rtl.Rtl.IntToStr(call__508 * 2));
            if (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(GetActiveAction()))
            {
                var call__509 = Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr;
                var call__510 = Aow.Units.AoWE.AoWE.TranslateRStr(call__509);
                Names.Add(call__510);
                Values.Add(Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Round(Income * 0.25)));
                Income += Aow.Rtl.Rtl.Round(Income * 0.25);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
            {
                var call__511 = Aow.Units.AoWE.AoWE.GoldRushRStr;
                var call__512 = Aow.Units.AoWE.AoWE.TranslateRStr(call__511);
                Names.Add(call__512);
                Values.Add(Aow.Rtl.Rtl.IntToStr(Income));
            }
        }

        // City.pas:1800
        public void SetDefaultProductions()
        {
            FProduction.FUnitProductions.Clear();
            if (FRace >= 0)
            {
                SetUpgradeLevel(unchecked((byte)FUpgradeLevel));
            }
        }

        // City.pas:1816
        public void ListCivilStatusInfo(Aow.Rtl.TStrings List)
        {
            Aow.Units.AoWE.TCombatPredictor Predictor = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            Aow.Units.AoWE.TUnitList Units = default;
            int I = default;
            int J = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            if ((FPlayer != 0) && (FRace >= 0))
            {
                var call__513 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__514 = call__513.GetPlayerRelation(unchecked((sbyte)FPlayer));
                var call__515 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__514]);
                var call__516 = Aow.Units.AoWE.AoWE.RaceRelationXRStr;
                var call__517 = Aow.Units.AoWE.AoWE.TranslateRStr(call__516);
                var call__518 = Aow.Rtl.Rtl.Format(call__517, [call__515]);
                List.Add(call__518);
                if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                {
                    var call__519 = Aow.Units.AoWE.AoWE.TerrainPenaltyXRStr;
                    var call__520 = Aow.Units.AoWE.AoWE.TranslateRStr(call__519);
                    var call__521 = Aow.Units.AoWE.AoWE.AoWHSSet.TerrainTypes[TerrainType];
                    var call__522 = Aow.Rtl.Rtl.Format(call__520, [call__521]);
                    List.Add(call__522);
                }

                if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                {
                    var call__523 = Aow.Units.AoWE.AoWE.TerrainBonusXRStr;
                    var call__524 = Aow.Units.AoWE.AoWE.TranslateRStr(call__523);
                    var call__525 = Aow.Units.AoWE.AoWE.AoWHSSet.TerrainTypes[TerrainType];
                    var call__526 = Aow.Rtl.Rtl.Format(call__524, [call__525]);
                    List.Add(call__526);
                }

                Predictor = Aow.Units.AoWE.TCombatPredictor.Create();
                Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                ListArmies(Armies);
                {
                    I = 0;
                    int end__527 = Armies.Count - 1;
                    for (; I <= end__527; I += 1)
                    {
                        {
                            J = 0;
                            int end__528 = Armies.GetArmyHS(I).FArmy.Count - 1;
                            for (; J <= end__528; J += 1)
                            {
                                var call__529 = Armies.GetArmyHS(I);
                                var call__530 = call__529.FArmy.GetUnit(J);
                                U = call__530;
                                if (U.FMoraleValue <= 40)
                                {
                                    Predictor.AddOpponent(U, 0);
                                }
                                else
                                {
                                    Predictor.AddUnit(U);
                                }
                            }
                        }
                    }
                }

                Units = Aow.Units.AoWE.TUnitList.Create(null);
                GenerateRebelUnits(Units);
                {
                    J = 0;
                    int end__531 = Units.Count - 1;
                    for (; J <= end__531; J += 1)
                    {
                        var call__532 = Units.GetUnit(J);
                        Predictor.AddOpponent(call__532, 0);
                    }
                }

                if (Predictor.Execute() == Aow.Units.AoWE.TCombatResult.crVictory)
                {
                    if ((Predictor.GetSuperiority() / 4) >= 75)
                    {
                        var call__533 = Aow.Units.AoWE.AoWE.StrongRStr;
                        var call__534 = Aow.Units.AoWE.AoWE.TranslateRStr(call__533);
                        var call__535 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                        var call__536 = Aow.Units.AoWE.AoWE.TranslateRStr(call__535);
                        var call__537 = Aow.Rtl.Rtl.Format(call__536, [call__534]);
                        List.Add(call__537);
                    }
                    else
                    {
                        var call__538 = Aow.Units.AoWE.AoWE.AverageRStr;
                        var call__539 = Aow.Units.AoWE.AoWE.TranslateRStr(call__538);
                        var call__540 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                        var call__541 = Aow.Units.AoWE.AoWE.TranslateRStr(call__540);
                        var call__542 = Aow.Rtl.Rtl.Format(call__541, [call__539]);
                        List.Add(call__542);
                    }
                }
                else
                {
                    var call__543 = Aow.Units.AoWE.AoWE.WeakRStr;
                    var call__544 = Aow.Units.AoWE.AoWE.TranslateRStr(call__543);
                    var call__545 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                    var call__546 = Aow.Units.AoWE.AoWE.TranslateRStr(call__545);
                    var call__547 = Aow.Rtl.Rtl.Format(call__546, [call__544]);
                    List.Add(call__547);
                }

                Predictor.Free();
                Units.Free();
                Armies.Free();
            }
        }

        // City.pas:1861
        public Aow.Units.City.TCivilStatus GetCivilStatus()
        {
            Aow.Units.City.TCivilStatus Result = default;
            Aow.Units.AoWE.TStatus Relation = default;
            int Value = default;
            int ArmyValue = default;
            int Bonus = default;
            int I = default;
            int J = default;
            Aow.Units.AoWE.TCombatPredictor Predictor = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            Aow.Units.AoWE.TUnitList Units = default;
            Aow.Units.AoWE.TAbstractUnit U = default;
            if ((FPlayer != 0) && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
            {
                if (FMode.Contains(Aow.Units.City.TCityModeElement.cmAnarchy))
                {
                    Relation = Aow.Units.AoWE.TStatus.sHostile;
                }
                else
                {
                    var call__548 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__549 = call__548.GetPlayerRelation(unchecked((sbyte)FPlayer));
                    Relation = call__549;
                }

                switch (Relation)
                {
                    case Aow.Units.AoWE.TStatus.sHostile:
                        Value = -35;
                        break;
                    case Aow.Units.AoWE.TStatus.sWary:
                        Value = -25;
                        break;
                    case Aow.Units.AoWE.TStatus.sNeutral:
                        Value = 0;
                        break;
                    case Aow.Units.AoWE.TStatus.sPolite:
                        Value = 15;
                        break;
                    case Aow.Units.AoWE.TStatus.sFriendly:
                        Value = 25;
                        break;
                    default:
                        Value = 0;
                        break;
                }

                Value = (Value + (FWallType * 5)) + (FUpgradeLevel * 5);
                if (FRace >= 0)
                {
                    if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                    {
                        Value += -10;
                    }

                    if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                    {
                        Value += 10;
                    }

                    Predictor = Aow.Units.AoWE.TCombatPredictor.Create();
                    ArmyValue = 0;
                    Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                    ListArmies(Armies);
                    {
                        I = 0;
                        int end__550 = Armies.Count - 1;
                        for (; I <= end__550; I += 1)
                        {
                            {
                                J = 0;
                                int end__551 = Armies.GetArmyHS(I).FArmy.Count - 1;
                                for (; J <= end__551; J += 1)
                                {
                                    var call__552 = Armies.GetArmyHS(I);
                                    var call__553 = call__552.FArmy.GetUnit(J);
                                    U = call__553;
                                    if (U.FMoraleValue <= 40)
                                    {
                                        Predictor.AddOpponent(U, 0);
                                        // { Original anarchy counts disloyal defenders twice. }
                                        if (FMode.Contains(Aow.Units.City.TCityModeElement.cmAnarchy))
                                        {
                                            Predictor.AddOpponent(U, 0);
                                        }
                                    }
                                    else
                                    {
                                        ArmyValue += U.GetObtainValue();
                                        Predictor.AddUnit(U);
                                    }
                                }
                            }
                        }
                    }

                    Armies.Free();
                    if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(Relation))
                    {
                        Units = Aow.Units.AoWE.TUnitList.Create(null);
                        GenerateRebelUnits(Units);
                        {
                            I = 0;
                            int end__554 = Units.Count - 1;
                            for (; I <= end__554; I += 1)
                            {
                                var call__555 = Units.GetUnit(I);
                                Predictor.AddOpponent(call__555, 0);
                                if (FMode.Contains(Aow.Units.City.TCityModeElement.cmAnarchy))
                                {
                                    var call__556 = Units.GetUnit(I);
                                    Predictor.AddOpponent(call__556, 0);
                                }
                            }
                        }

                        if (Predictor.Execute() == Aow.Units.AoWE.TCombatResult.crVictory)
                        {
                            Bonus = Predictor.GetSuperiority() / 4;
                            if (Bonus >= 75)
                            {
                                Bonus = 75;
                            }
                            else
                            {
                                if (Bonus >= 26)
                                {
                                    Bonus = 50;
                                }
                                else
                                {
                                    Bonus = 25;
                                }
                            }
                        }
                        else
                        {
                            Bonus = 0;
                        }

                        Value += Bonus;
                        Units.Free();
                    }
                    else
                    {
                        if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(Relation))
                        {
                            ArmyValue = ArmyValue / (GetSize() * 10);
                            if (ArmyValue > 10)
                            {
                                ArmyValue = 10;
                            }

                            Value += ArmyValue;
                        }
                    }

                    Predictor.Free();
                    if (Value < -30)
                    {
                        Result = Aow.Units.City.TCivilStatus.csUnruly;
                    }
                    else
                    {
                        if (Value < -10)
                        {
                            Result = Aow.Units.City.TCivilStatus.csUnrest;
                        }
                        else
                        {
                            if (Value < 10)
                            {
                                Result = Aow.Units.City.TCivilStatus.csStable;
                            }
                            else
                            {
                                if (Value < 30)
                                {
                                    if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(Relation))
                                    {
                                        Result = Aow.Units.City.TCivilStatus.csOppressed;
                                    }
                                    else
                                    {
                                        Result = Aow.Units.City.TCivilStatus.csContent;
                                    }
                                }
                                else
                                {
                                    if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(Relation))
                                    {
                                        Result = Aow.Units.City.TCivilStatus.csSlavery;
                                    }
                                    else
                                    {
                                        Result = Aow.Units.City.TCivilStatus.csCheerful;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Result = Aow.Units.City.TCivilStatus.csStable;
                }
            }
            else
            {
                Result = Aow.Units.City.TCivilStatus.csStable;
            }

            return Result;
        }

        // City.pas:2034
        public void SetUpgradeLevel(byte Level)
        {
            Aow.Units.AoWE.TRaceResource RaceResource = default;
            int I = default;
            int UnitLevel = default;
            bool Researched = default;
            if (Level > GetSize())
            {
                Level = unchecked((byte)GetSize());
            }

            if (Level < 1)
            {
                Level = 1;
            }

            FUpgradeLevel = unchecked((byte)Level);
            if (FRace >= 0)
            {
                var call__557 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                RaceResource = call__557;
                {
                    I = RaceResource.FUnitIndexes.Count - 1;
                    byte end__558 = 0;
                    for (; I >= end__558; I -= 1)
                    {
                        var call__559 = RaceResource.FUnitIndexes[I];
                        if (call__559 != -1)
                        {
                            var call__560 = RaceResource.FUnitIndexes[I];
                            var call__561 = FProduction.FUnitProductions.IndexOf(call__560);
                            if (call__561 == -1)
                            {
                                var call__562 = RaceResource.FUnitIndexes[I];
                                var call__563 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__562);
                                var call__564 = GetSize();
                                var call__565 = Aow.Units.City.City.UnitLevelToCityLevel(call__563.FLevel, call__564);
                                UnitLevel = call__565;
                                if ((UnitLevel != -1) && (UnitLevel <= FUpgradeLevel))
                                {
                                    bool bool__566 = (UnitLevel == 1);
                                    if (bool__566)
                                    {
                                        bool bool__567 = (RaceResource.FSpecialUnitIndexes.Count >= 4);
                                        if (bool__567)
                                        {
                                            var call__568 = RaceResource.FUnitIndexes[I];
                                            var call__569 = RaceResource.FSpecialUnitIndexes[3];
                                            bool__567 = (call__568 == call__569);
                                        }

                                        bool__566 = !bool__567;
                                    }

                                    Researched = bool__566;
                                    var call__570 = RaceResource.FUnitIndexes[I];
                                    FProduction.FUnitProductions.Add(call__570, Researched);
                                }
                            }
                        }
                    }
                }

                Aow.Units.City.City.SortingCity = this;
                FProduction.FUnitProductions.FChildren.Sort(Aow.Units.City.City.CompareCityProductions);
            }
        }

        // City.pas:2085
        public void SetName(Aow.Rtl.AnsiString Name)
        {
            // { Unmatched: tried identity assignment to Self; Pointer identity assignment to Self; zero-shift receiver before field access (5 variants total). }
            FName = Name;
            Update();
        }

        // City.pas:2091
        public void SetWallType(byte WallType)
        {
            FWallType = unchecked((byte)WallType);
            UpdateImages();
        }

        // City.pas:2206
        public void InitCombat(Aow.Units.AoWE.TCombat Combat, Aow.Units.AoWE.TArmyHS Army)
        {
            if (FPlayer == 0)
            {
                var call__571 = GetEnemiesIndependentRelation(unchecked((sbyte)Army.FArmy.FPlayer));
                var call__572 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FDefenderPlayer));
                call__572.FIndependentRelation = unchecked((byte)call__571);
            }

            var call__573 = Army.GetLhx();
            var call__574 = Army.GetYhx();
            var call__575 = Army.GetXhx();
            Combat.InitDefaultArmies(Army.FArmy, call__575, call__574, call__573);
        }

        // City.pas:2234
        public int GetObtainValue()
        {
            int Result = default;
            var call__576 = GetSize();
            Result = Aow.Units.City.City.UpgradePrices[call__576 - 1] * FUpgradeLevel;
            if (FWallType == 1)
            {
                var call__577 = GetSize();
                Result += Aow.Units.City.City.WoodWallPrices[call__577 - 1];
            }

            if (FWallType == 2)
            {
                var call__578 = GetSize();
                var call__579 = GetSize();
                Result = (Result + Aow.Units.City.City.WoodWallPrices[call__578 - 1]) + Aow.Units.City.City.StoneWallPrices[call__579 - 1];
            }

            return Result;
        }

        // City.pas:2241
        public int JoinAmount(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            int I = default;
            Aow.Units.AoWE.TArmy Army = default;
            bool CanJoin = default;
            int Value = default;
            byte Relation = default;
            byte Selection = default;
            Result = 0;
            Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
            ListArmies(Armies);
            var call__580 = Armies.Count;
            I = call__580;
            CanJoin = true;
            if (I != 0)
            {
                while ((I != 0) && CanJoin)
                {
                    I -= 1;
                    var call__581 = Armies.GetArmyHS(I);
                    Army = call__581.FArmy;
                    Selection = unchecked((byte)Army.GetCanJoinSelection(unchecked((sbyte)Player)));
                    CanJoin = Selection == Army.ValidateSelection(255);
                    if (CanJoin)
                    {
                        Result += Army.JoinAmount(unchecked((sbyte)Player), 255);
                    }
                }

                if (CanJoin)
                {
                    Value = GetObtainValue() / 4;
                    var call__582 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__583 = call__582.GetPlayerRelation(unchecked((sbyte)Player));
                    Relation = unchecked((byte)unchecked((byte)call__583));
                    Result += (Aow.Units.City.City.TCity_JoinAmount__JoinPercent[(int)((Aow.Units.AoWE.TStatus)Relation)] * Value) / 100;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                var call__584 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__585 = call__584.GetPlayerRelation(unchecked((sbyte)Player));
                if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__585))
                {
                    Result = 0;
                }
                else
                {
                    Result = -1;
                }
            }

            Armies.Free();
            return Result;
        }

        // City.pas:2282
        public bool IncommingNegotiateRequest(Aow.Units.AoWE.TNegotiateRequestEventLog Request)
        {
            bool Result = default;
            int Amount = default;
            if ((FPlayer == 0) && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
            {
                var call__586 = JoinAmount(unchecked((sbyte)Request.FPlayer));
                Amount = call__586;
                if (Amount != -1)
                {
                    Request.FObject = Aow.Units.AoWE.TNegotiateObject.noCity;
                    var call__587 = GetName();
                    var call__588 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityJoinHeaderRStr);
                    var call__589 = Aow.Rtl.Rtl.Format(call__588, [call__587]);
                    Request.FText = call__589;
                    Request.FGold = Amount;
                    if (Request.FGold == 0)
                    {
                        var call__590 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityFreeJoinRStr);
                        Request.FJoinMessage.Text = call__590;
                    }
                    else
                    {
                        var call__591 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityTributeJoinRStr);
                        var call__592 = Aow.Rtl.Rtl.Format(call__591, [Aow.Rtl.Rtl.IntToStr(Request.FGold)]);
                        Request.FJoinMessage.Text = call__592;
                    }

                    Result = true;
                }
                else
                {
                    Result = false;
                }
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2302
        public bool IncommingNegotiateOffer(Aow.Units.AoWE.TNegotiateTE Offer)
        {
            bool Result = default;
            int Amount = default;
            Aow.Units.AoWE.TArmyHSList Armies = default;
            int I = default;
            Aow.Units.EventLog.TLocationTextEventLog Log = default;
            if (Offer.FObject == Aow.Units.AoWE.TNegotiateObject.noCity)
            {
                var call__593 = JoinAmount(unchecked((sbyte)Offer.FPlayer));
                Amount = call__593;
                if (((Offer.FOffer == Aow.Units.AoWE.TNegotiateOffer.noOfferBribe) && (Amount <= Offer.FGold)) && (Amount != -1))
                {
                    var call__594 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Offer.FPlayer);
                    if (call__594.FGems >= Offer.FGold)
                    {
                        SetPlayer(unchecked((sbyte)Offer.FPlayer));
                        Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                        ListArmies(Armies);
                        {
                            I = 0;
                            int end__595 = Armies.Count - 1;
                            for (; I <= end__595; I += 1)
                            {
                                var call__596 = Armies.GetArmyHS(I);
                                if (call__596.FArmy.FPlayer == 0)
                                {
                                    var call__597 = Armies.GetArmyHS(I);
                                    call__597.FArmy.SetPlayer(unchecked((sbyte)Offer.FPlayer));
                                }
                            }
                        }

                        Armies.Free();
                        Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                        Log.FPlayer = unchecked((sbyte)FPlayer);
                        var call__598 = GetName();
                        var call__599 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XOverthrownRStr);
                        var call__600 = Aow.Rtl.Rtl.Format(call__599, [call__598]);
                        Log.FText = call__600;
                        var call__601 = GetLhx();
                        var call__602 = GetYhx();
                        var call__603 = GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__603), unchecked((sbyte)call__602), unchecked((sbyte)call__601));
                        var call__604 = GetYhx();
                        var call__605 = GetLhx();
                        var call__606 = GetXhx();
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__606), unchecked((sbyte)call__604), unchecked((sbyte)call__605), false);
                        Log.Release();
                        Offer.Accept();
                    }
                    else
                    {
                        var call__607 = Aow.Units.AoWE.AoWE.NotEnoughGoldRStr;
                        var call__608 = Aow.Units.AoWE.AoWE.TranslateRStr(call__607);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__608);
                    }
                }
                else
                {
                    Offer.Reject();
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2334
        public sbyte GetEnemiesIndependentRelation(sbyte Player)
        {
            sbyte Result = default;
            Aow.Units.AoWE.TUnitUserList Units = default;
            int I = default;
            Units = Aow.Units.AoWE.TUnitUserList.Create(null);
            ListUnits(Units);
            {
                I = Units.Count - 1;
                byte end__609 = 0;
                for (; I >= end__609; I -= 1)
                {
                    var call__610 = Units.GetUnit(I);
                    var call__611 = call__610.CanJoin(unchecked((sbyte)Player));
                    if (call__611)
                    {
                        var call__612 = Units.GetUnit(I);
                        Units.DeleteUnit(call__612);
                    }
                }
            }

            // { Original uses the race index as the default relation. }
            var call__613 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, unchecked((sbyte)FRace));
            Result = unchecked((sbyte)call__613);
            Units.Free();
            return Result;
        }

        // City.pas:2345
        public sbyte GetIndependentRelation()
        {
            sbyte Result = default;
            Aow.Units.AoWE.TUnitUserList Units = default;
            Units = Aow.Units.AoWE.TUnitUserList.Create(null);
            ListUnits(Units);
            // { Original uses the race index as the default relation. }
            var call__614 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, unchecked((sbyte)FRace));
            Result = unchecked((sbyte)call__614);
            Units.Free();
            return Result;
        }

        // City.pas:2354
        public void ReturnBattleFieldInfo(ref Aow.Units.AoWE.TBattleFieldInfo Info)
        {
            int Value = default;
            Aow.Units.AoWE.TDiplomaticRelation Relation = default;
            if (Info.Requests.Contains(Aow.Units.AoWE.TBattleFieldInfoRequestsElement.bfrEffectiveStrength) && (FPlayer > 0))
            {
                Value = 5000;
                var call__615 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__616 = call__615.GetPlayerRelation(unchecked((sbyte)FPlayer));
                if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(call__616))
                {
                    Value = Value * 2;
                }

                var call__617 = Aow.Units.AoWE.AoWE.DiplomaticRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), 0);
                Relation = call__617;
                if (Info.FriendlyRelations.Contains((byte)((int)Relation)))
                {
                    Info.FriendlyStrength -= Value;
                }

                if (Info.EnemyRelations.Contains((byte)((int)Relation)))
                {
                    Info.Strength -= Value;
                }
            }
        }

        // City.pas:2367
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible = default;
            var call__618 = VisibleForPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Visible = call__618;
            if (FEnchantmentsVisible ^ Visible)
            {
                FEnchantmentsVisible = Visible;
                if (FEnchantments != TSet<Aow.Units.City.TCityEnchantmentID>.Of())
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
                }
            }
        }

        // City.pas:2503
        public Aow.Units.ImageLib.TLibraryImage GetTopCityActionImage(Aow.Units.City.TCityAction Action)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade, Aow.Units.City.TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case Aow.Units.City.TCityAction.caFortify:
                        switch (FWallType)
                        {
                            case 0:
                                Index = 30;
                                break;
                            case 1:
                                Index = 48;
                                break;
                        }

                        break;
                    case Aow.Units.City.TCityAction.caUpgrade:
                        Index = 33;
                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        Index = 39;
                        break;
                    case Aow.Units.City.TCityAction.caRaze:
                        Index = 51;
                        break;
                    case Aow.Units.City.TCityAction.caLoot:
                        Index = 45;
                        break;
                }

                if ((FResource.ImageSequenceList != null) && (Index != -1))
                {
                    var call__619 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__619 != null)
                    {
                        var call__620 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__621 = call__620.GetPreviewImage();
                        Result = call__621;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__622 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__623 = call__622.FImageLibrary.GetSafe(40);
                        Result = call__623;
                    }
                    else
                    {
                        var call__624 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__625 = call__624.FImageLibrary.GetSafe(43);
                        Result = call__625;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2539
        public Aow.Units.ImageLib.TLibraryImage GetMiddleCityActionImage(Aow.Units.City.TCityAction Action)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade, Aow.Units.City.TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case Aow.Units.City.TCityAction.caFortify:
                        switch (FWallType)
                        {
                            case 0:
                                Index = 31;
                                break;
                            case 1:
                                Index = 49;
                                break;
                        }

                        break;
                    case Aow.Units.City.TCityAction.caUpgrade:
                        Index = 34;
                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        Index = 40;
                        break;
                    case Aow.Units.City.TCityAction.caRaze:
                        Index = 52;
                        break;
                    case Aow.Units.City.TCityAction.caLoot:
                        Index = 46;
                        break;
                }

                if ((FResource.ImageSequenceList != null) && (Index != -1))
                {
                    var call__626 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__626 != null)
                    {
                        var call__627 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__628 = call__627.GetPreviewImage();
                        Result = call__628;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__629 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__630 = call__629.FImageLibrary.GetSafe(41);
                        Result = call__630;
                    }
                    else
                    {
                        var call__631 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__632 = call__631.FImageLibrary.GetSafe(44);
                        Result = call__632;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:2575
        public Aow.Units.ImageLib.TLibraryImage GetBottomCityActionImage(Aow.Units.City.TCityAction Action)
        {
            Aow.Units.ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade, Aow.Units.City.TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case Aow.Units.City.TCityAction.caFortify:
                        switch (FWallType)
                        {
                            case 0:
                                Index = 32;
                                break;
                            case 1:
                                Index = 50;
                                break;
                        }

                        break;
                    case Aow.Units.City.TCityAction.caUpgrade:
                        Index = 35;
                        break;
                    case Aow.Units.City.TCityAction.caMigrate:
                        Index = 41;
                        break;
                    case Aow.Units.City.TCityAction.caRaze:
                        Index = 53;
                        break;
                    case Aow.Units.City.TCityAction.caLoot:
                        Index = 47;
                        break;
                }

                if ((FResource.ImageSequenceList != null) && (Index != -1))
                {
                    var call__633 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__633 != null)
                    {
                        var call__634 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__635 = call__634.GetPreviewImage();
                        Result = call__635;
                    }
                    else
                    {
                        Result = null;
                    }
                }
                else
                {
                    Result = null;
                }
            }
            else
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        var call__636 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__637 = call__636.FImageLibrary.GetSafe(42);
                        Result = call__637;
                    }
                    else
                    {
                        var call__638 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__639 = call__638.FImageLibrary.GetSafe(45);
                        Result = call__639;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            return Result;
        }

        // City.pas:138
        public static new Aow.Units.City.TCity Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCity o = new Aow.Units.City.TCity();
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

        public new partial class Meta : Aow.Units.Crops.TPlayerCropStructure.Meta
        {
            // City.pas:849
            public override int ClassID()
            {
                int Result = default;
                Result = 131336;
                return Result;
            }

            // City.pas:138
            public new Aow.Units.City.TCity Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCity o = (Aow.Units.City.TCity)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCity => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCityResource (instance size 124, VMT 557A76AC): $78 FSize. }
    public partial class TCityResource : Aow.Units.Crops.TPlayerCropStructureResource
    {
        public byte FSize;
        // { $78 }
        public Aow.Units.FixedArray_byte_3 F_CityResource_79;
        // City.pas:2622
        public override void Create__init(Aow.Units.Engine.TECustomNode AOwner)
        {
            base.Create__init(AOwner);
            FRazeable = true;
        }

        // City.pas:2669
        public override void MsgProc(ref Aow.Units.Engine.TEMsg Msg, Aow.Units.Engine.TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 536870920)
            {
                ActivateMsg();
            }
        }

        // City.pas:2638
        public override int ResourceUserClassID()
        {
            int Result = default;
            Result = 131336;
            return Result;
        }

        // City.pas:2643
        public void ShowCity(int X, int Y, in Aow.Units.GFXE.TSurfaceDesc SurfaceDesc, Aow.Units.City.TCity City)
        {
            int dX = default;
            int dY = default;
            Aow.Units.ImageLib.TLibraryImage Image = default;
            dX = FdXImage + X;
            dY = Y;
            dY += FdYImage;
            Image = City.FWallBackImage;
            if (Image != null)
            {
                Image.Show(dX, dY, SurfaceDesc);
            }

            if (City.FCityImage != null)
            {
                // { Original blinks the selected city from frame bits. }
                var call__640 = City.GetSelected();
                if (call__640 && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 6) == 6))
                {
                    City.FCityImage.ShowTransparent(dX, dY, SurfaceDesc);
                }
                else
                {
                    City.FCityImage.Show(dX, dY, SurfaceDesc);
                }
            }

            Image = City.FWallFrontImage;
            if (Image != null)
            {
                Image.Show(dX, dY, SurfaceDesc);
            }
        }

        // City.pas:2661
        public new void ActivateMsg()
        {
            int I = default;
            FSize = 0;
            {
                I = 0;
                int end__641 = FTerrainList.Count - 1;
                for (; I <= end__641; I += 1)
                {
                    var call__642 = FTerrainList.Terrain[I];
                    if (call__642 >= 0)
                    {
                        FSize = (byte)(((int)FSize) + 1);
                    }
                }
            }
        }

        // City.pas:262
        public static new Aow.Units.City.TCityResource Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityResource o = new Aow.Units.City.TCityResource();
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

        public new partial class Meta : Aow.Units.Crops.TPlayerCropStructureResource.Meta
        {
            // City.pas:2628
            public override int ClassID()
            {
                int Result = default;
                Result = 131337;
                return Result;
            }

            // City.pas:2633
            public override Aow.Rtl.AnsiString ResourceEditName()
            {
                Aow.Rtl.AnsiString Result = default;
                var call__643 = Aow.Units.AoWE.AoWE.CityRStr;
                var call__644 = Aow.Units.AoWE.AoWE.TranslateRStr(call__643);
                Result = call__644;
                return Result;
            }

            // City.pas:262
            public new Aow.Units.City.TCityResource Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCityResource o = (Aow.Units.City.TCityResource)NewInstance();
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

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCityResource => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    // { TCityRebellionEventLog (instance size 40, VMT 557A77A0): $18 FXhx, $1C FYhx,
//     $20 FLhx, $24 FCityName. }
    public partial class TCityRebellionEventLog : Aow.Units.EventLog.TEventLog
    {
        public int FXhx;
        // { $18 }
        public int FYhx;
        // { $1C }
        public int FLhx;
        // { $20 }
        public Aow.Rtl.AnsiString FCityName;
        // City.pas:2680
        // { $24 }
        public override void ReadWrite(Aow.Units.Engine.TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FXhx, 20);
            Stream.rwInteger(ref FYhx, 21);
            Stream.rwInteger(ref FLhx, 22);
            Stream.rwString(ref FCityName, 23);
        }

        // City.pas:2689
        public override Aow.Rtl.AnsiString GetText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__645 = Aow.Units.AoWE.AoWE.CityRebellionRStr;
            var call__646 = Aow.Units.AoWE.AoWE.TranslateRStr(call__645);
            var call__647 = Aow.Rtl.Rtl.Format(call__646, [FCityName]);
            Result = call__647;
            return Result;
        }

        // City.pas:2699
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            bool ShowEvent = default;
            ShowEvent = Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser;
            bool bool__648 = ShowEvent;
            if (!bool__648)
            {
                bool bool__649 = (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn);
                if (bool__649)
                {
                    var call__650 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
                    bool__649 = call__650.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
                }

                bool__648 = bool__649;
            }

            ShowEvent = bool__648;
            bool bool__651 = ShowEvent;
            if (!bool__651)
            {
                bool bool__652 = (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous);
                if (bool__652)
                {
                    var call__653 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
                    bool__652 = call__653.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
                }

                bool__651 = bool__652;
            }

            ShowEvent = bool__651;
            if (ShowEvent)
            {
                Result = base.Execute(Mode, Trigger, Done);
            }
            else
            {
                Result = false;
            }

            return Result;
        }

        // City.pas:2694
        public override void ViewLocation()
        {
            CenterViewTo(unchecked((sbyte)FXhx), unchecked((sbyte)FYhx), unchecked((sbyte)FLhx));
        }

        // EventLog.pas:56
        public static new Aow.Units.City.TCityRebellionEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityRebellionEventLog o = new Aow.Units.City.TCityRebellionEventLog();
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

        public new partial class Meta : Aow.Units.Engine.TEObject.Meta
        {
            // City.pas:2675
            public override int ClassID()
            {
                int Result = default;
                Result = 131346;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCityRebellionEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:56
            public new Aow.Units.City.TCityRebellionEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCityRebellionEventLog o = (Aow.Units.City.TCityRebellionEventLog)NewInstance();
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
    }

    // { TAICityUpgradeBudget (instance size 56, VMT 557A78B4) }
    public partial class TAICityUpgradeBudget : Aow.Units.AoWE.TAIBudget
    {
        // Engine.pas:524
        public static new Aow.Units.City.TAICityUpgradeBudget Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TAICityUpgradeBudget o = new Aow.Units.City.TAICityUpgradeBudget();
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

        public new partial class Meta : Aow.Units.AoWE.TAIBudget.Meta
        {
            // City.pas:2775
            public override int ClassID()
            {
                int Result = default;
                Result = 131349;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TAICityUpgradeBudget => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.City.TAICityUpgradeBudget Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TAICityUpgradeBudget o = (Aow.Units.City.TAICityUpgradeBudget)NewInstance();
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
    }

    // { TAICityResearchBudget (instance size 56, VMT 557A7994) }
    public partial class TAICityResearchBudget : Aow.Units.AoWE.TAIBudget
    {
        // Engine.pas:524
        public static new Aow.Units.City.TAICityResearchBudget Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TAICityResearchBudget o = new Aow.Units.City.TAICityResearchBudget();
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

        public new partial class Meta : Aow.Units.AoWE.TAIBudget.Meta
        {
            // City.pas:2780
            public override int ClassID()
            {
                int Result = default;
                Result = 131350;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TAICityResearchBudget => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.City.TAICityResearchBudget Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TAICityResearchBudget o = (Aow.Units.City.TAICityResearchBudget)NewInstance();
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
    }

    // { TAICityFortifyBudget (instance size 56, VMT 557A7A74) }
    public partial class TAICityFortifyBudget : Aow.Units.AoWE.TAIBudget
    {
        // Engine.pas:524
        public static new Aow.Units.City.TAICityFortifyBudget Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TAICityFortifyBudget o = new Aow.Units.City.TAICityFortifyBudget();
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

        public new partial class Meta : Aow.Units.AoWE.TAIBudget.Meta
        {
            // City.pas:2785
            public override int ClassID()
            {
                int Result = default;
                Result = 131351;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TAICityFortifyBudget => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.City.TAICityFortifyBudget Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TAICityFortifyBudget o = (Aow.Units.City.TAICityFortifyBudget)NewInstance();
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
    }

    // { TCityAIPA (instance size 48, VMT 557A7B54): $14 FCityCount,
//     $18..$24 FProductionLevelCounts, $28 FMaxProductionLevel, $2C FProductionRatio. }
    public partial class TCityAIPA : Aow.Units.AoWE.TAIPlayerAction
    {
        public int FCityCount;
        // { $14 }
        public Aow.Units.FixedArray_int_4 FProductionLevelCounts;
        // { $18..$24 }
        public int FMaxProductionLevel;
        // { $28 }
        public float FProductionRatio;
        // City.pas:3130
        public override bool Process()
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Units.AoWE.TAIBudgetManager Manager = default;
            Aow.Units.AoWE.TAIBudget UpgradeBudget = default;
            Aow.Units.AoWE.TAIBudget ResearchBudget = default;
            Aow.Units.AoWE.TAIBudget FortifyBudget = default;
            UpdateStatistics();
            Player = FControl.FPlayer;
            if (Player.FPlayerIndex != 0)
            {
                Manager = Player.FAIBudget;
                var call__654 = Manager.FBudgets.FindChild(131349);
                UpgradeBudget = (Aow.Units.AoWE.TAIBudget)call__654;
                if (UpgradeBudget == null)
                {
                    UpgradeBudget = Aow.Units.City.TAICityUpgradeBudget.Create(null);
                    UpgradeBudget.Owner = Manager.FBudgets;
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }

                var call__655 = Manager.FBudgets.FindChild(131350);
                ResearchBudget = (Aow.Units.AoWE.TAIBudget)call__655;
                if (ResearchBudget != null)
                {
                    if ((ResearchBudget.FGold + UpgradeBudget.FGold) > 125)
                    {
                        if (ResearchProduction(ResearchBudget, UpgradeBudget))
                        {
                            ResearchBudget.FDays = 7;
                            ResearchBudget.FRequiredGold = 125;
                        }
                    }
                }
                else
                {
                    ResearchBudget = Aow.Units.City.TAICityResearchBudget.Create(null);
                    ResearchBudget.FDays = 7;
                    ResearchBudget.FRequiredGold = 125;
                    ResearchBudget.Owner = Manager.FBudgets;
                }

                ValidateResearchBudget(ResearchBudget);
                if (UpgradeBudget.GetReady())
                {
                    if (UpgradeCity(UpgradeBudget))
                    {
                        UpgradeBudget.FRequiredGold = 250;
                        UpgradeBudget.FDays = 7;
                    }
                }

                ValidateUpgradeBudget(UpgradeBudget);
                var call__656 = Manager.FBudgets.FindChild(131351);
                FortifyBudget = (Aow.Units.AoWE.TAIBudget)call__656;
                if (FortifyBudget != null)
                {
                    if (FortifyBudget.GetReady())
                    {
                        if (FortifyCity(FortifyBudget))
                        {
                            FortifyBudget.FDays = 7;
                            FortifyBudget.FRequiredGold = 80;
                        }
                    }
                }
                else
                {
                    if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay > 5)
                    {
                        FortifyBudget = Aow.Units.City.TAICityFortifyBudget.Create(null);
                        FortifyBudget.FDays = 7;
                        FortifyBudget.FRequiredGold = 80;
                        FortifyBudget.Owner = Manager.FBudgets;
                    }
                }

                if (FortifyBudget != null)
                {
                    ValidateFortifyBudget(FortifyBudget);
                }
            }

            Result = false;
            return Result;
        }

        // City.pas:2795
        public bool ValidResearchCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            var call__657 = City.FProduction.GetProduceUnitLevelMax();
            Result = call__657 < City.FUpgradeLevel;
            bool bool__658 = Result;
            if (bool__658)
            {
                var call__659 = City.GetActiveAction();
                bool bool__660 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__659);
                if (!bool__660)
                {
                    var call__661 = City.GetActiveAction();
                    bool__660 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__661) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__658 = bool__660;
            }

            Result = bool__658;
            return Result;
        }

        // City.pas:2802
        public bool ValidResearchBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            var call__662 = City.FProduction.GetProduceUnitLevelMax();
            bool bool__663 = (call__662 != City.FUpgradeLevel);
            if (!bool__663)
            {
                var call__664 = City.GetActiveAction();
                bool__663 = (call__664 == Aow.Units.City.TCityAction.caUpgrade);
            }

            Result = bool__663;
            return Result;
        }

        // City.pas:2807
        public void ValidateResearchBudget(Aow.Units.AoWE.TAIBudget Budget)
        {
            Aow.Units.AoWE.TPlayer Player = default;
            int I = default;
            int Count = default;
            int Total = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            Total = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__665 = 0;
                for (; I >= end__665; I -= 1)
                {
                    var call__666 = Player.FStructures.GetStructure(I);
                    Structure = call__666;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        Total += 1;
                        if (ValidResearchBudgetCity((Aow.Units.City.TCity)Structure))
                        {
                            Count += 1;
                        }
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 125;
                Priority = 7 - ((Count * Total) / 3);
                if (Priority < 3)
                {
                    Priority = 3;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:2842
        public bool ResearchProduction(Aow.Units.AoWE.TAIBudget ResearchBudget, Aow.Units.AoWE.TAIBudget UpgradeBudget)
        {
            bool Result = default;
            Aow.Units.Engine.TDoubleItemList Cities = default;
            bool Valid = default;
            Aow.Units.AoWE.TPlayer Player = default;
            int I = default;
            int J = default;
            int UnitIndex = default;
            int Index = default;
            int Value = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Aow.Units.City.TCity City = default;
            Aow.Units.AoWE.TProductionItem Production = default;
            Result = false;
            Player = FControl.FPlayer;
            Cities = Aow.Units.Engine.TDoubleItemList.Create();
            {
                I = Player.FStructures.Count - 1;
                byte end__667 = 0;
                for (; I >= end__667; I -= 1)
                {
                    var call__668 = Player.FStructures.GetStructure(I);
                    Structure = call__668;
                    if ((Structure is Aow.Units.City.TCity) && ValidResearchCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        Valid = false;
                        {
                            J = City.FProduction.FUnitProductions.Count - 1;
                            byte end__669 = 0;
                            for (; J >= end__669; J -= 1)
                            {
                                var call__670 = City.FProduction.FUnitProductions.GetItem(J);
                                UnitIndex = call__670.FUnitIndex;
                                if (UnitIndex != -1)
                                {
                                    bool bool__671 = Valid;
                                    if (!bool__671)
                                    {
                                        var call__672 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
                                        bool bool__673 = (call__672.FLevel == City.FUpgradeLevel);
                                        if (bool__673)
                                        {
                                            var call__674 = City.FProduction.FUnitProductions.GetItem(J);
                                            bool__673 = !call__674.FResearched;
                                        }

                                        bool bool__675 = bool__673;
                                        if (bool__675)
                                        {
                                            var call__676 = City.ResearchCost(J);
                                            bool__675 = (call__676 <= (ResearchBudget.FGold + UpgradeBudget.FGold));
                                        }

                                        bool__671 = bool__675;
                                    }

                                    Valid = bool__671;
                                }
                            }
                        }

                        if (Valid)
                        {
                            var call__677 = City.GetSize();
                            Priority = (call__677 * 2) + (City.FProductionPriority + (City.FWallType * 3));
                            Cities.AddAB(Aow.Rtl.Rtl.ObjectToInt(City), Priority);
                        }
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                Index = 0;
                var call__678 = Cities[0];
                Value = Aow.Rtl.Rtl.Round(call__678.B * 0.75);
                while (true)
                {
                    bool bool__679 = ((Index + 1) < Cities.Count);
                    if (bool__679)
                    {
                        var call__680 = Cities[Index + 1];
                        bool__679 = (Value <= call__680.B);
                    }

                    if (!bool__679)
                        break;
                    {
                        Index += 1;
                    }
                }

                var call__681 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Index + 1);
                Index = call__681;
                /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                throw new System.NotImplementedException();
                var call__683 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                Aow.Rtl.Rtl.RandSeed = call__683;
                /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                throw new System.NotImplementedException();
                if (Index != -1)
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = Aow.Units.AoWE.TProductionItem.Create(null);
                    Production.FProductionID = 2;
                    Production.FParameter = Index;
                    City.FProduction.ExecuteAddProduction(Production);
                    Production.Free();
                    Index = City.ResearchCost(Index);
                    if (Index > ResearchBudget.FGold)
                    {
                        Index -= ResearchBudget.FGold;
                        Player.FAIBudget.BudgetExpenseMade(ResearchBudget.FGold, 0, ResearchBudget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, UpgradeBudget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                    }
                    else
                    {
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, ResearchBudget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                    }

                    Result = true;
                }
            }

            Cities.Free();
            return Result;
        }

        // City.pas:2909
        public bool ValidUpgradeCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            int Level = default;
            var call__686 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__686;
            Result = City.CanUpgrade(false, ref Reason);
            bool bool__687 = Result;
            if (bool__687)
            {
                var call__688 = City.GetSize();
                bool__687 = (call__688 > City.FUpgradeLevel);
            }

            Result = (bool__687 && !((Level == FMaxProductionLevel) && (FProductionLevelCounts[Level - 1] == 1))) && (Level == City.FUpgradeLevel);
            bool bool__689 = Result;
            if (bool__689)
            {
                var call__690 = City.GetActiveAction();
                bool bool__691 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__690);
                if (!bool__691)
                {
                    var call__692 = City.GetActiveAction();
                    bool__691 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__692) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__689 = bool__691;
            }

            Result = bool__689;
            return Result;
        }

        // City.pas:2921
        public bool ValidUpgradeBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            int Level = default;
            var call__693 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__693;
            var call__694 = City.GetSize();
            if ((call__694 <= City.FUpgradeLevel) || (Level != Ptr<byte>.Unbox(City)[77]))
            {
                Result = false;
            }
            else
            {
                Result = true;
            }

            return Result;
        }

        // City.pas:2928
        public bool UpgradeCity(Aow.Units.AoWE.TAIBudget Budget)
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Units.Engine.TDoubleItemList Cities = default;
            int I = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Aow.Units.City.TCity City = default;
            Aow.Units.AoWE.TProductionItem Production = default;
            Player = FControl.FPlayer;
            Cities = Aow.Units.Engine.TDoubleItemList.Create();
            {
                I = Player.FStructures.Count - 1;
                byte end__695 = 0;
                for (; I >= end__695; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__696 = Player.FStructures.GetStructure(I);
                    Structure = call__696;
                    if ((Structure is Aow.Units.City.TCity) && ValidUpgradeCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        var call__697 = City.GetSize();
                        Priority = (call__697 * 2) + (City.FProductionPriority + (City.FWallType * 3));
                        Cities.AddAB(Aow.Rtl.Rtl.ObjectToInt(City), Priority);
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                throw new System.NotImplementedException();
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = Aow.Units.AoWE.TProductionItem.Create(null);
                    var call__699 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                    Production.FProductionID = call__699;
                    City.FProduction.ExecuteAddProduction(Production);
                    Production.Free();
                    Player.FAIBudget.BudgetExpenseMade(250, 0, Budget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            Cities.Free();
            return Result;
        }

        // City.pas:2964
        public void ValidateUpgradeBudget(Aow.Units.AoWE.TAIBudget Budget)
        {
            Aow.Units.AoWE.TPlayer Player = default;
            int I = default;
            int Count = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__700 = 0;
                for (; I >= end__700; I -= 1)
                {
                    var call__701 = Player.FStructures.GetStructure(I);
                    Structure = call__701;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        if (ValidUpgradeBudgetCity((Aow.Units.City.TCity)Structure))
                        {
                            Count += 1;
                        }
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 250;
                Priority = 8 - (Count / 2);
                if (Priority < 5)
                {
                    Priority = 5;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:2999
        public bool ValidFortifyCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            Aow.Rtl.AnsiString Reason = default;
            Result = City.CanFortify(false, ref Reason);
            bool bool__702 = (Result && (City.FWallType == 0));
            if (!bool__702)
            {
                bool bool__703 = (City.FWallType == 1);
                if (bool__703)
                {
                    var call__704 = City.GetSize();
                    bool__703 = (call__704 > 2);
                }

                bool__702 = ((bool__703 && (City.FUpgradeLevel > 1)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay >= 30));
            }

            Result = bool__702;
            bool bool__705 = Result;
            if (bool__705)
            {
                var call__706 = City.GetActiveAction();
                bool bool__707 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__706);
                if (!bool__707)
                {
                    var call__708 = City.GetActiveAction();
                    bool__707 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__708) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__705 = bool__707;
            }

            Result = bool__705;
            return Result;
        }

        // City.pas:3009
        public bool ValidFortifyBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            bool bool__709 = (City.FWallType == 0);
            if (!bool__709)
            {
                bool bool__710 = (City.FWallType == 1);
                if (bool__710)
                {
                    var call__711 = City.GetSize();
                    bool__710 = (call__711 > 2);
                }

                bool__709 = ((bool__710 && (City.FUpgradeLevel > 1)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay >= 30));
            }

            Result = bool__709;
            return Result;
        }

        // City.pas:3015
        public bool FortifyCity(Aow.Units.AoWE.TAIBudget Budget)
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer Player = default;
            Aow.Units.Engine.TDoubleItemList Cities = default;
            int I = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Aow.Units.City.TCity City = default;
            Aow.Units.AoWE.TProductionItem Production = default;
            Player = FControl.FPlayer;
            Cities = Aow.Units.Engine.TDoubleItemList.Create();
            {
                I = Player.FStructures.Count - 1;
                byte end__712 = 0;
                for (; I >= end__712; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__713 = Player.FStructures.GetStructure(I);
                    Structure = call__713;
                    if ((Structure is Aow.Units.City.TCity) && ValidFortifyCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        var call__714 = City.GetSize();
                        Priority = (call__714 * 2) + City.FProductionPriority;
                        if (City.FWallType == 0)
                        {
                            Priority += 3;
                        }

                        Cities.AddAB(Aow.Rtl.Rtl.ObjectToInt(City), Priority);
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                throw new System.NotImplementedException();
                {
                    City.FProduction.ExecuteDeleteProductions();
                    var call__716 = City.FortifyCost();
                    Player.FAIBudget.BudgetExpenseMade(call__716, 0, Budget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                    Production = Aow.Units.AoWE.TProductionItem.Create(null);
                    var call__717 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                    Production.FProductionID = call__717;
                    City.FProduction.ExecuteAddProduction(Production);
                    Production.Free();
                }

                Result = true;
            }
            else
            {
                Result = false;
            }

            Cities.Free();
            return Result;
        }

        // City.pas:3052
        public void ValidateFortifyBudget(Aow.Units.AoWE.TAIBudget Budget)
        {
            Aow.Units.AoWE.TPlayer Player = default;
            int I = default;
            int Count = default;
            int Priority = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__718 = 0;
                for (; I >= end__718; I -= 1)
                {
                    var call__719 = Player.FStructures.GetStructure(I);
                    Structure = call__719;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        if (ValidFortifyBudgetCity((Aow.Units.City.TCity)Structure))
                        {
                            Count += 1;
                        }
                    }
                }
            }

            if (Count == 0)
            {
                Player.FAIBudget.ReturnExpense(Budget.FGold, Budget.FMana);
                Budget.FGold = 0;
                Budget.FMana = 0;
                Budget.FRequiredGold = 0;
                // { Original limits days, not priority. }
                Budget.FDays = 7;
            }
            else
            {
                Budget.FRequiredGold = 80;
                Priority = 8 - (Count / 2);
                if (Priority < 4)
                {
                    Priority = 4;
                }

                if (Priority < Budget.FDays)
                {
                    Budget.FDays = Priority;
                }
            }
        }

        // City.pas:3087
        public int UpgradeableCities()
        {
            int Result = default;
            int I = default;
            int Count = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            Aow.Units.City.TCity City = default;
            Count = 0;
            {
                I = FControl.FPlayer.FStructures.Count - 1;
                byte end__720 = 0;
                for (; I >= end__720; I -= 1)
                {
                    var call__721 = FControl.FPlayer.FStructures.GetStructure(I);
                    Structure = call__721;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        {
                            var w__722 = (Aow.Units.City.TCity)Structure;
                            var call__723 = w__722.GetSize();
                            if (call__723 > w__722.FUpgradeLevel)
                            {
                                Count += 1;
                            }
                        }
                    }
                }
            }

            Result = Count;
            return Result;
        }

        // City.pas:3102
        public void UpdateStatistics()
        {
            int I = default;
            int Level = default;
            int Available = default;
            int Busy = default;
            Aow.Units.City.TCity City = default;
            FCityCount = 0;
            FMaxProductionLevel = 0;
            Available = 0;
            Busy = 0;
            FProductionLevelCounts = default;
            {
                I = FControl.FPlayer.FStructures.Count - 1;
                byte end__724 = 0;
                for (; I >= end__724; I -= 1)
                {
                    var call__725 = FControl.FPlayer.FStructures.GetStructure(I);
                    if (call__725 is Aow.Units.City.TCity)
                    {
                        FCityCount += 1;
                        var call__726 = FControl.FPlayer.FStructures.GetStructure(I);
                        City = (Aow.Units.City.TCity)call__726;
                        if (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade, Aow.Units.City.TCityAction.caProduce).Contains(City.GetActiveAction()))
                        {
                            Available += (City.GetSize() + 1) / 2;
                            var call__727 = City.FProduction.GetProduceUnitLevelMax();
                            Level = call__727;
                            if (Level > 0)
                            {
                                FProductionLevelCounts[Level - 1] += 1;
                            }

                            if (Level > FMaxProductionLevel)
                            {
                                FMaxProductionLevel = Level;
                            }
                        }
                        else
                        {
                            Busy += (City.GetSize() + 1) / 2;
                        }
                    }
                }
            }

            if (FCityCount != 0)
            {
                // { Original rounds the fraction before storing it. }
                FProductionRatio = Aow.Rtl.Rtl.Round(((double)Available) / (Available + Busy));
            }
            else
            {
                FProductionRatio = 0;
            }
        }

        public new partial class Meta : Aow.Units.AoWE.TAIPlayerAction.Meta
        {
            // City.pas:2790
            // { $2C }
            public override int Priority()
            {
                int Result = default;
                Result = 13;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCityAIPA => (Meta)MetaRegistry.Of(typeof(T));
            // Engine.pas:524
            public new Aow.Units.City.TCityAIPA Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCityAIPA o = (Aow.Units.City.TCityAIPA)NewInstance();
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
    }

    // { TCityProductionEventLog (instance size 32, VMT 557A7C24) }
    public partial class TCityProductionEventLog : Aow.Units.AoWE.TProductionEventLog
    {
        // City.pas:3205
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            Aow.Units.City.TCity City = default;
            if (Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser)
            {
                City = GetCity();
                if ((City != null) && (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == City.FPlayer))
                {
                    Aow.Units.AoWE.AoWE.AoWHSMap.CenterViewTo(FXhx, FYhx, FLhx);
                    Aow.Units.AoWE.AoWE.AoWHSMap.TriggerExecuteEventLog(this, true, Done);
                }
            }

            Result = false;
            return Result;
        }

        // City.pas:3200
        public Aow.Units.City.TCity GetCity()
        {
            Aow.Units.City.TCity Result = default;
            var call__728 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__729 = call__728.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            Result = (Aow.Units.City.TCity)call__729;
            return Result;
        }

        // EventLog.pas:91
        public static new Aow.Units.City.TCityProductionEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
        {
            Aow.Units.City.TCityProductionEventLog o = new Aow.Units.City.TCityProductionEventLog();
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

        public new partial class Meta : Aow.Units.AoWE.TProductionEventLog.Meta
        {
            // City.pas:3195
            public override int ClassID()
            {
                int Result = default;
                Result = 131834;
                return Result;
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TCityProductionEventLog => (Meta)MetaRegistry.Of(typeof(T));
            // EventLog.pas:91
            public new Aow.Units.City.TCityProductionEventLog Create(Aow.Units.Engine.TECustomNode AOwner)
            {
                Aow.Units.City.TCityProductionEventLog o = (Aow.Units.City.TCityProductionEventLog)NewInstance();
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
    }

    // { TRegistrator (instance size 4, VMT 557AF144): no added fields. }
    public partial class TRegistrator : Aow.Rtl.TObject
    {
        public new partial class Meta : Aow.Rtl.TObject.Meta
        {
            // City.pas:3306
            public void MapLoaded(Aow.Rtl.TObject Sender)
            {
                if (Sender == Aow.Units.AoWE.AoWE.AoWHSMap)
                {
                    Aow.Rtl.Rtl.RegisterClasses([typeof(Aow.Units.City.TCityAIPA)]);
                }
            }

            public static new Meta Of<T>()
                where T : Aow.Units.City.TRegistrator => (Meta)MetaRegistry.Of(typeof(T));
        }
    }

    public enum TCivilStatus : byte
    {
        csUnruly = 0,
        csUnrest = 1,
        csStable = 2,
        csContent = 3,
        csCheerful = 4,
        csOppressed = 5,
        csSlavery = 6
    }

    public enum TCityEnchantmentID : byte
    {
        ceiFountainOfLife = 0,
        ceiGoldRush = 1,
        ceiAntiMagicShell = 2,
        ceiWarmonger = 3
    }

    public enum TCityAction : byte
    {
        caNone = 0,
        caTrade = 1,
        caUpgrade = 2,
        caFortify = 3,
        caMigrate = 4,
        caLoot = 5,
        caRaze = 6,
        caResearch = 7,
        caProduce = 8
    }

    public enum TCityOverlayMode : byte
    {
        omIdle = 0,
        omShowRazeAnimation = 1,
        omLooting = 2
    }

    public enum TDefendersGenerateMode : byte
    {
        dgNone = 0,
        dgRandom = 1,
        dgWeak = 2,
        dgAverage = 3,
        dgStrong = 4
    }

    public static partial class City
    {
        public static Aow.Units.FixedArray_int_5 TCity_JoinAmount__JoinPercent = Aow.Units.FixedArray_int_5.From([200, 175, 150, 125, 100]);
        // City.pas:3220
        public static Aow.Units.City.TCityAction ProductionIDToCityAction(int ID)
        {
            Aow.Units.City.TCityAction Result = default;
            switch (ID)
            {
                case 1:
                    Result = Aow.Units.City.TCityAction.caProduce;
                    break;
                case 2:
                    Result = Aow.Units.City.TCityAction.caResearch;
                    break;
                case 10:
                    Result = Aow.Units.City.TCityAction.caTrade;
                    break;
                case 11:
                    Result = Aow.Units.City.TCityAction.caUpgrade;
                    break;
                case 12:
                    Result = Aow.Units.City.TCityAction.caFortify;
                    break;
                case 13:
                    Result = Aow.Units.City.TCityAction.caMigrate;
                    break;
                case 14:
                    Result = Aow.Units.City.TCityAction.caLoot;
                    break;
                default:
                    Result = Aow.Units.City.TCityAction.caNone;
                    break;
            }

            return Result;
        }

        // City.pas:3234
        public static int CityActionToProductionID(Aow.Units.City.TCityAction Action)
        {
            int Result = default;
            switch (Action)
            {
                case Aow.Units.City.TCityAction.caProduce:
                    Result = 1;
                    break;
                case Aow.Units.City.TCityAction.caResearch:
                    Result = 2;
                    break;
                case Aow.Units.City.TCityAction.caTrade:
                    Result = 10;
                    break;
                case Aow.Units.City.TCityAction.caUpgrade:
                    Result = 11;
                    break;
                case Aow.Units.City.TCityAction.caFortify:
                    Result = 12;
                    break;
                case Aow.Units.City.TCityAction.caMigrate:
                    Result = 13;
                    break;
                case Aow.Units.City.TCityAction.caLoot:
                    Result = 14;
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // City.pas:3248
        public static bool RaceInCity(sbyte Race, sbyte Player)
        {
            bool Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            int I = default;
            Result = false;
            var call__730 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            P = call__730;
            I = 0;
            while (true)
            {
                var call__731 = P.FStructures.Count;
                if (!((I != call__731) && !Result))
                    break;
                {
                    var call__732 = P.FStructures.GetStructure(I);
                    if (call__732 is Aow.Units.City.TCity)
                    {
                        var call__733 = P.FStructures.GetStructure(I);
                        var call__734 = ((Aow.Units.City.TCity)call__733).GetBuilding();
                        bool bool__735 = !call__734;
                        if (bool__735)
                        {
                            var call__736 = P.FStructures.GetStructure(I);
                            bool__735 = (((Aow.Units.City.TCity)call__736).FRace == Race);
                        }

                        Result = bool__735;
                    }

                    I += 1;
                }
            }

            return Result;
        }

        // City.pas:3263
        public static int UnitLevelToCityLevel(int Level, int Size)
        {
            int Result = default;
            Result = Level;
            return Result;
        }

        // City.pas:3268
        public static int PlayerCities(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            int I = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                var call__737 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__737;
                Result = 0;
                {
                    I = 0;
                    int end__738 = P.FStructures.Count - 1;
                    for (; I <= end__738; I += 1)
                    {
                        var call__739 = P.FStructures.GetStructure(I);
                        Structure = call__739;
                        var call__740 = Structure.ClassType.ClassID();
                        if ((call__740 == 131336) && (Structure.FPlayer == Player))
                        {
                            Result += 1;
                        }
                    }
                }
            }
            else
            {
                Result = 0;
            }

            return Result;
        }

        // City.pas:3286
        public static int CityIncome(sbyte Player)
        {
            int Result = default;
            Aow.Units.AoWE.TPlayer P = default;
            int I = default;
            Aow.Units.AoWE.TPlayerStructure Structure = default;
            if (Aow.Units.AoWE.AoWE.AoWHSMap != null)
            {
                var call__741 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__741;
                Result = 0;
                {
                    I = 0;
                    int end__742 = P.FStructures.Count - 1;
                    for (; I <= end__742; I += 1)
                    {
                        var call__743 = P.FStructures.GetStructure(I);
                        Structure = call__743;
                        var call__744 = Structure.ClassType.ClassID();
                        if ((call__744 == 131336) && (Structure.FPlayer == Player))
                        {
                            Result += ((Aow.Units.City.TCity)Structure).FIncome;
                        }
                    }
                }
            }
            else
            {
                Result = -1;
            }

            return Result;
        }

        public static Aow.Units.FixedArray_Aow_Rtl_AnsiString_9 TCityActionRStr = Aow.Units.FixedArray_Aow_Rtl_AnsiString_9.From([(AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)""]);
        // { export $558E8DEC }
        public static Aow.Units.FixedArray_Aow_Rtl_AnsiString_7 TCivilStatusRStr = Aow.Units.FixedArray_Aow_Rtl_AnsiString_7.From([(AnsiString)"Unruly", (AnsiString)"Unrest", (AnsiString)"Stable", (AnsiString)"Content", (AnsiString)"Cheerful", (AnsiString)"Oppressed", (AnsiString)"Slavery"]);
        public static Aow.Units.FixedArray_Aow_Rtl_AnsiString_4 TCityEnchantmentIDRStr = Aow.Units.FixedArray_Aow_Rtl_AnsiString_4.From([(AnsiString)"Fountain Of Life", (AnsiString)"Gold Rush", (AnsiString)"Anti Magic Shell", (AnsiString)"Warmonger"]);
        public static Aow.Units.FixedArray_byte_4 UpgradePrices = Aow.Units.FixedArray_byte_4.From([250, 250, 250, 250]);
        public static Aow.Units.FixedArray_byte_4 WoodWallPrices = Aow.Units.FixedArray_byte_4.From([20, 40, 60, 80]);
        public static Aow.Units.FixedArray_byte_4 StoneWallPrices = Aow.Units.FixedArray_byte_4.From([20, 40, 60, 80]);
        public static readonly Aow.Rtl.AnsiString CityNotEnoughGoldRStr = (AnsiString)"Not enough gold.";
        public static readonly Aow.Rtl.AnsiString CityWarningRStr = (AnsiString)"Warning";
        public static readonly Aow.Rtl.AnsiString CityWoodenWallRStr = (AnsiString)"Wooden Wall";
        public static readonly Aow.Rtl.AnsiString CityStoneWallRStr = (AnsiString)"Stone Wall";
        public static Aow.Units.City.TCity SortingCity;
        // City.pas:2005
        public static int CompareCityProductions(object Item1, object Item2)
        {
            int Result = default;
            int Value1 = default;
            int Value2 = default;
            {
                var w__745 = (Aow.Units.AoWE.TUnitProduction)Item1;
                var call__746 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__745.FUnitIndex);
                if (call__746 != null)
                {
                    var call__747 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__745.FUnitIndex);
                    Value1 = call__747.FLevel * 2;
                    if (Aow.Units.City.City.SortingCity.FRace != -1)
                    {
                        var call__748 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                        if (call__748.FSpecialUnitIndexes.Count > 3)
                        {
                            var call__749 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                            var call__750 = call__749.FSpecialUnitIndexes[3];
                            if (call__750 == w__745.FUnitIndex)
                            {
                                Value1 += 1;
                            }
                        }
                    }
                }
                else
                {
                    Value1 = 0;
                }
            }

            {
                var w__751 = (Aow.Units.AoWE.TUnitProduction)Item2;
                var call__752 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__751.FUnitIndex);
                if (call__752 != null)
                {
                    var call__753 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__751.FUnitIndex);
                    Value2 = call__753.FLevel * 2;
                    if (Aow.Units.City.City.SortingCity.FRace != -1)
                    {
                        var call__754 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                        if (call__754.FSpecialUnitIndexes.Count > 3)
                        {
                            var call__755 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                            var call__756 = call__755.FSpecialUnitIndexes[3];
                            if (call__756 == w__751.FUnitIndex)
                            {
                                Value2 += 1;
                            }
                        }
                    }
                }
                else
                {
                    Value2 = 0;
                }
            }

            Result = Value1 - Value2;
            if (Result == 0)
            {
                Result = ((Aow.Units.AoWE.TUnitProduction)Item1).FUnitIndex - ((Aow.Units.AoWE.TUnitProduction)Item2).FUnitIndex;
            }

            return Result;
        }

        public static void Initialize()
        {
            if (Aow.Units.AoWE.AoWE.ValidateAoWEngine())
            {
                Aow.Units.AoWE.AoWE.AoWEngine.RegisterEClasses([Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TCityEnchantment>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TCity>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TCityResource>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TCityRebellionEventLog>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TAICityUpgradeBudget>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TAICityResearchBudget>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TAICityFortifyBudget>(), Aow.Units.Engine.TEObject.Meta.Of<Aow.Units.City.TCityProductionEventLog>()]);
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Add(Aow.Units.City.TRegistrator.Meta.Of<Aow.Units.City.TRegistrator>().MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (Aow.Units.AoWE.AoWE.AoWEngine != null)
            {
                Aow.Units.AoWE.AoWE.AoWEngine.OnHSMLoaded.Remove(Aow.Units.City.TRegistrator.Meta.Of<Aow.Units.City.TRegistrator>().MapLoaded);
            }
        }
    }
}
