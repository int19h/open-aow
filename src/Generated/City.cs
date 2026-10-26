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

        // City.pas:2113
        public override void Activate()
        {
            RegisterEnchantments();
            base.Activate();
            UpdateVisibleForSeatedPlayer();
            FProduction.Activate();
            if (FPlayer != -1)
            {
                var call__216 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__216.FProductionControls.Add(FProduction);
            }

            /* TODO(T-N3) Untyped pointer dereference requires a typed storage override */
            throw new System.NotImplementedException();
            UpdateImages();
        }

        // City.pas:2124
        public override void Deactivate()
        {
            UnregisterEnchantments();
            if (FPlayer != -1)
            {
                var call__217 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__217.FProductionControls.Remove(FProduction);
            }

            FProduction.Deactivate();
            base.Deactivate();
        }

        // City.pas:2184
        public override void UpdateMapField(in Aow.Units.HSEngine.THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            var call__218 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__218 >= 0)
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
            var call__219 = GetActiveAction();
            var call__220 = GetTopCityActionImage(call__219);
            Result = call__220;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__221 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__222 = call__221.FImageLibrary.GetSafe(40);
                        Result = call__222;
                    }
                    else
                    {
                        var call__223 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__224 = call__223.FImageLibrary.GetSafe(43);
                        Result = call__224;
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
                        var call__225 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__226 = call__225.GetPreviewImage();
                        Result = call__226;
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
            var call__227 = GetActiveAction();
            var call__228 = GetMiddleCityActionImage(call__227);
            Result = call__228;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__229 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__230 = call__229.FImageLibrary.GetSafe(41);
                        Result = call__230;
                    }
                    else
                    {
                        var call__231 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__232 = call__231.FImageLibrary.GetSafe(44);
                        Result = call__232;
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
                        var call__233 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__234 = call__233.GetPreviewImage();
                        Result = call__234;
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
            var call__235 = GetActiveAction();
            var call__236 = GetBottomCityActionImage(call__235);
            Result = call__236;
            if (Result == null)
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        var call__237 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__238 = call__237.FImageLibrary.GetSafe(42);
                        Result = call__238;
                    }
                    else
                    {
                        var call__239 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__240 = call__239.FImageLibrary.GetSafe(45);
                        Result = call__240;
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
                        var call__241 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__242 = call__241.GetPreviewImage();
                        Result = call__242;
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

                var call__243 = GetTerrainTypeImage(TerrainType);
                ImageIndex = call__243;
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
            var call__244 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(8);
            if (call__244 == 0)
            {
                var call__245 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, 5);
                var call__246 = Aow.Units.AoWE.AoWE.AoWHSMap.FAIGroupManager.CreateGroup(0, unchecked((sbyte)call__245), Aow.Units.AoWE.TGroupBehavior.gbRefuge);
                Group = call__246;
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
                var call__247 = GetLhx();
                var call__248 = GetYhx();
                var call__249 = GetXhx();
                Aow.Units.AoWE.AoWE.AoWHSMap.RetrieveBattleFieldInfo(call__249, call__248, call__247, 10, ref Info);
                Requirements.Strength = Info.Strength;
                Maximum = Aow.Rtl.Rtl.Round(GetSize() * 5000);
                var call__250 = GetSize();
                var call__251 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                Minimum = Aow.Rtl.Rtl.Round((call__250 * 5000) * call__251.FAIControl.FDefenseStrengthMultiplier);
                if (Player != 0)
                {
                    var call__252 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__253 = call__252.GetPlayerRelation(unchecked((sbyte)Player));
                    Relation = call__253;
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

                bool bool__254 = (FPlayer != 0);
                if (bool__254)
                {
                    var call__255 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__256 = call__255.GetPlayerRelation(unchecked((sbyte)FPlayer));
                    bool__254 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(call__256);
                }

                if (bool__254)
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
                        var call__257 = Aow.Units.AoWE.AoWE.AoWEngine.FCampaign.FRaceHistory[I];
                        SetRace(unchecked((sbyte)call__257));
                    }
                    else
                    {
                        var call__258 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRandomRace();
                        SetRace(unchecked((sbyte)call__258));
                    }
                }
                else
                {
                    var call__259 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRandomRace();
                    SetRace(unchecked((sbyte)call__259));
                }
            }

            if (Aow.Units.AoWE.AoWE.AoWHSMap.FDay == 1)
            {
                FProduction.FUnitProductions.Clear();
                SetUpgradeLevel(unchecked((byte)FUpgradeLevel));
                GenerateDefenders();
                if (FWallType == 3)
                {
                    var call__260 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(3);
                    FWallType = unchecked((byte)call__260);
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
                var call__261 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__261.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman)
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
            var call__262 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FBuildingPlayer);
            SetRace(unchecked((sbyte)call__262.FPlayerRace));
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
                var call__263 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__264 = call__263.GetPlayerRelationValue(unchecked((sbyte)Player));
                var call__265 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                call__265.SetPlayerRelationValue(unchecked((sbyte)Player), call__264 + -30);
            }

            return Result;
        }

        // City.pas:2611
        public override Aow.Rtl.AnsiString GetCurrentActivityText()
        {
            Aow.Rtl.AnsiString Result = default;
            var call__266 = FProduction.FQueue.Count;
            if (call__266 == 0)
            {
                var call__267 = Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr;
                var call__268 = Aow.Units.AoWE.AoWE.TranslateRStr(call__267);
                Result = call__268;
            }
            else
            {
                var call__269 = FProduction.FQueue.GetItem(0);
                var call__270 = FProduction.GetProductionNameProgressive(call__269);
                Result = call__270;
            }

            return Result;
        }

        // City.pas:876
        public override bool ValidateMap(Aow.Rtl.TStrings A1)
        {
            bool Result = default;
            Result = base.ValidateMap(A1);
            var call__271 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NonameRStr);
            if ((Aow.Rtl.Rtl.CompareText(FName, call__271) == 0) || (FName == ((AnsiString)"")))
            {
                var call__272 = Aow.Units.City.City.CityWarningRStr;
                var call__273 = Aow.Units.AoWE.AoWE.TranslateRStr(call__272);
                var call__274 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoNameSpecifiedRStr);
                A1.Add((call__273 + ((AnsiString)": ")) + call__274);
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
                                var call__275 = Aow.Units.AoWE.AoWE.ValidAttackRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), unchecked((sbyte)FRace));
                                if (call__275)
                                {
                                    Priority = 100 + ((GetSize() - 1) * 10);
                                }
                            }
                            else
                            {
                                if (FPlayer == 0)
                                {
                                    var call__276 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                                    var call__277 = call__276.GetPlayerRelation(unchecked((sbyte)Info.Player));
                                    if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__277))
                                    {
                                        var call__278 = JoinAmount(unchecked((sbyte)Info.Player));
                                        Amount = call__278;
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
                        bool bool__279 = (Info.Player == FPlayer);
                        if (bool__279)
                        {
                            var call__280 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                            var call__281 = call__280.GetPlayerRelation(unchecked((sbyte)Info.Player));
                            bool__279 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__281);
                        }

                        bool bool__282 = bool__279;
                        if (bool__282)
                        {
                            var call__283 = GetActiveAction();
                            bool__282 = (call__283 != Aow.Units.City.TCityAction.caMigrate);
                        }

                        if (bool__282)
                        {
                            var call__284 = GetSize();
                            Data.Priority = (500 + (call__284 * 100)) + (FWallType * 45);
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
                            var call__285 = GetSize();
                            Data.Priority = 50 + (call__285 * 20);
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
            bool bool__286 = (Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer == FPlayer);
            if (bool__286)
            {
                var call__287 = GetRazed();
                bool__286 = !call__287;
            }

            bool bool__288 = bool__286;
            if (bool__288)
            {
                var call__289 = GetBuilding();
                bool__288 = !call__289;
            }

            Result = bool__288;
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
                    var call__290 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__290.FProductionControls.Remove(FProduction);
                }

                base.SetPlayer(unchecked((sbyte)Player));
                if (GetActiveAction() == Aow.Units.City.TCityAction.caMigrate)
                {
                    FProduction.ExecuteCancelProduction();
                    var call__291 = FProduction.FQueue.GetItem(0);
                    call__291.Free();
                }

                ValidateAICityAction();
                UpdateAIMigration();
                if (FPlayer != -1)
                {
                    var call__292 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__292.FProductionControls.Add(FProduction);
                }
            }
        }

        // City.pas:2071
        public override void UpdateFlagID()
        {
            bool bool__293 = !FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moEditMode);
            if (bool__293)
            {
                bool bool__294 = !FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
                if (!bool__294)
                {
                    var call__295 = GetBuilding();
                    bool__294 = call__295;
                }

                bool__293 = bool__294;
            }

            if (bool__293 && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
            {
                if (FPlayer > 0)
                {
                    var call__296 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    FFlagID = unchecked((byte)(call__296.FPlayerRace + ((FUpgradeLevel - 1) * 64)));
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
            var call__297 = GetLhx();
            bool bool__298 = (call__297 != 0);
            if (!bool__298)
            {
                bool bool__299 = (FPlayer > 0);
                if (bool__299)
                {
                    var call__300 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    bool__299 = ((((byte)call__300.FMagicControl.FEnemyEnchantments.Image) & 8) != 0);
                }

                bool__298 = bool__299;
            }

            if (bool__298)
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
                    var w__301 = Combat.FCombatData;
                    var call__302 = w__301.FPlayers.GetPlayers(unchecked((sbyte)w__301.FAttackerPlayer));
                    SetPlayer(unchecked((sbyte)call__302.FPlayer));
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
                    var call__303 = Aow.Units.AoWE.AoWE.CityBattleWonRStr;
                    var call__304 = Aow.Units.AoWE.AoWE.TranslateRStr(call__303);
                    var call__305 = GetName();
                    var call__306 = Aow.Rtl.Rtl.Format(call__304, [call__305]);
                    Result = call__306;
                }
                else
                {
                    if (Combat.FCombatResult == Aow.Units.AoWE.TCombatResult.crDefeat)
                    {
                        var call__307 = Aow.Units.AoWE.AoWE.CityBattleLostRStr;
                        var call__308 = Aow.Units.AoWE.AoWE.TranslateRStr(call__307);
                        var call__309 = GetName();
                        var call__310 = Aow.Rtl.Rtl.Format(call__308, [call__309]);
                        Result = call__310;
                    }
                    else
                    {
                        if (Combat.FCombatResult != Aow.Units.AoWE.TCombatResult.crCannotBreachWall)
                        {
                            var call__311 = Aow.Units.AoWE.AoWE.CityBattleDrawnRStr;
                            var call__312 = Aow.Units.AoWE.AoWE.TranslateRStr(call__311);
                            var call__313 = GetName();
                            var call__314 = Aow.Rtl.Rtl.Format(call__312, [call__313]);
                            Result = call__314;
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
            var call__315 = GetRazed();
            bool bool__316 = !call__315;
            if (bool__316)
            {
                var call__317 = GetBuilding();
                bool__316 = !call__317;
            }

            if ((((bool__316 && (OldPlayer == 0)) && !Aow.Units.AoWE.AoWE.AoWHSMap.Status.Contains(Aow.Units.HSEngine.THSMapStatusElement.msEditting)) && FStatus.Contains(Aow.Units.HSEngine.TMapObjectStatusElement.moActivated)) && !((Aow.Units.City.TCityResource)FResource).FClaimed)
            {
                Log = Aow.Units.City.TCityProductionEventLog.Create(null);
                Log.FPlayer = unchecked((sbyte)NewPlayer);
                var call__318 = EditName();
                var call__319 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.StructureCapturedRStr);
                var call__320 = Aow.Rtl.Rtl.Format(call__319, [call__318]);
                Log.FText = call__320;
                var call__321 = GetXhx();
                Log.FXhx = unchecked((sbyte)call__321);
                var call__322 = GetYhx();
                Log.FYhx = unchecked((sbyte)call__322);
                var call__323 = GetLhx();
                Log.FLhx = unchecked((sbyte)call__323);
                var call__324 = GetYhx();
                var call__325 = GetLhx();
                var call__326 = GetXhx();
                Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__326), unchecked((sbyte)call__324), unchecked((sbyte)call__325), false);
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
                bool bool__327 = TSet<Aow.Units.AoWE.TMoveAction>.Of(Aow.Units.AoWE.TMoveAction.maNormal, Aow.Units.AoWE.TMoveAction.maAttack).Contains(Action);
                if (bool__327)
                {
                    var call__328 = GetRazed();
                    bool__327 = !call__328;
                }

                if (bool__327 && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode))
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
            bool bool__329 = FMode.Contains(Aow.Units.City.TCityModeElement.cmRazed);
            if (bool__329)
            {
                var call__330 = GetBuilding();
                bool__329 = !call__330;
            }

            if (!bool__329)
            {
                MultiplePlayers = false;
                Player = unchecked((sbyte)-1);
                Count = GetTerrainCount();
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while ((Walk.HN < Count) && !MultiplePlayers)
                {
                    var call__331 = FResource.TerrainList.GetTerrain(Walk.HN);
                    if (((((call__331 >= 0) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                    {
                        var call__332 = MapLevel.GetField(Walk.X, Walk.Y);
                        var call__333 = call__332.FindOwnedChild(131607);
                        Army = (Aow.Units.AoWE.TArmyHS)call__333;
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
            var call__334 = GetRazed();
            bool bool__335 = !call__334;
            if (bool__335)
            {
                var call__336 = GetActiveAction();
                bool__335 = (call__336 != Aow.Units.City.TCityAction.caMigrate);
            }

            bool bool__337 = bool__335;
            if (bool__337)
            {
                var call__338 = GetBuilding();
                bool__337 = !call__338;
            }

            if (bool__337)
            {
                var call__339 = GetCropCount();
                var call__340 = GetSize();
                SetIncome(unchecked((byte)((call__339 * 2) + (call__340 * 5))));
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
                    var call__341 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    Race = call__341.FPlayerRace;
                }
                else
                {
                    Race = FRace;
                }

                var call__342 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                var call__343 = call__342.GetPlayerRelationValue(unchecked((sbyte)Player));
                var call__344 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                call__344.SetPlayerRelationValue(unchecked((sbyte)Player), call__343 + -5);
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
                var call__345 = FProduction.FQueue.GetItem(0);
                var call__346 = Aow.Units.City.City.ProductionIDToCityAction(call__345.FProductionID);
                Result = call__346;
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
                byte end__347 = 0;
                for (; I >= end__347; I -= 1)
                {
                    var call__348 = FEnchantmentList.GetEnchantments(I);
                    FEnchantments = FEnchantments + TSet<Aow.Units.City.TCityEnchantmentID>.Of(((Aow.Units.City.TCityEnchantment)call__348).FEnchantmentID);
                }
            }
        }

        // City.pas:902
        public void RegisterEnchantments()
        {
            int I = default;
            {
                I = FEnchantmentList.Count - 1;
                byte end__349 = 0;
                for (; I >= end__349; I -= 1)
                {
                    var call__350 = FEnchantmentList.GetEnchantments(I);
                    var call__351 = FEnchantmentList.GetEnchantments(I);
                    call__351.Activate(unchecked((sbyte)call__350.FPlayerIndex));
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
                byte end__352 = 0;
                for (; I >= end__352; I -= 1)
                {
                    var call__353 = FEnchantmentList.GetEnchantments(I);
                    var call__354 = FEnchantmentList.GetEnchantments(I);
                    call__354.Deactivate(unchecked((byte)call__353.FPlayerIndex));
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
            var call__355 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpgradeLevelXRStr);
            var call__356 = Aow.Rtl.Rtl.Format(call__355, [Aow.Rtl.Rtl.IntToStr(FUpgradeLevel)]);
            List.Add(call__356);
            if (FWallType == 1)
            {
                var call__357 = Aow.Units.City.City.CityWoodenWallRStr;
                var call__358 = Aow.Units.AoWE.AoWE.TranslateRStr(call__357);
                List.Add(call__358);
            }

            if (FWallType == 2)
            {
                var call__359 = Aow.Units.City.City.CityStoneWallRStr;
                var call__360 = Aow.Units.AoWE.AoWE.TranslateRStr(call__359);
                List.Add(call__360);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiFountainOfLife))
            {
                var call__361 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.FountainOfLifeRStr);
                var call__362 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__361 + ((AnsiString)": ")) + call__362);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
            {
                var call__363 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GoldRushRStr);
                var call__364 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__363 + ((AnsiString)": ")) + call__364);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiAntiMagicShell))
            {
                var call__365 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AntiMagicShellRStr);
                var call__366 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__365 + ((AnsiString)": ")) + call__366);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiWarmonger))
            {
                var call__367 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.WarmongerRStr);
                var call__368 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EnchantmentRStr);
                List.Add((call__367 + ((AnsiString)": ")) + call__368);
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
                int end__369 = Armies.Count - 1;
                for (; I <= end__369; I += 1)
                {
                    var call__370 = Armies.GetArmyHS(I);
                    var call__371 = call__370.FArmy.HealUnits(255);
                    if (call__371)
                    {
                        var call__372 = Armies.GetArmyHS(I);
                        var call__373 = call__372.GetLhx();
                        var call__374 = Armies.GetArmyHS(I);
                        var call__375 = call__374.GetYhx();
                        var call__376 = Armies.GetArmyHS(I);
                        var call__377 = call__376.GetXhx();
                        var call__378 = Aow.Units.AoWE.AoWE.AoWHSMap.VisibleForPlayer(call__377, call__375, call__373, unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                        if (call__378)
                        {
                            Animation = Aow.Units.Animation.TImageSequenceAnimation.Create(null);
                            var call__379 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                            var call__380 = call__379.FImageSequences.ImageSequences[0];
                            Animation.SetImageSequence(call__380);
                            Animation.FAnimationLevel = 27;
                            var call__381 = Armies.GetArmyHS(I);
                            var call__382 = call__381.GetLhx();
                            var call__383 = Armies.GetArmyHS(I);
                            var call__384 = call__383.GetYhx();
                            var call__385 = Armies.GetArmyHS(I);
                            var call__386 = call__385.GetXhx();
                            Aow.Units.AoWE.AoWE.AoWHSMap.PlaceHX(Animation, call__386, call__384, call__382);
                            bool bool__387 = PlaySound;
                            if (!bool__387)
                            {
                                var call__388 = Armies.GetArmyHS(I);
                                bool__387 = (call__388.FArmy.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                            }

                            bool bool__389 = bool__387;
                            if (!bool__389)
                            {
                                var call__390 = Armies.GetArmyHS(I);
                                var call__391 = call__390.GetLhx();
                                var call__392 = Armies.GetArmyHS(I);
                                var call__393 = call__392.GetYhx();
                                var call__394 = Armies.GetArmyHS(I);
                                var call__395 = call__394.GetXhx();
                                var call__396 = Aow.Units.AoWE.AoWE.AoWHSMap.Watching(unchecked((sbyte)call__395), unchecked((sbyte)call__393), unchecked((sbyte)call__391), 1);
                                bool__389 = call__396;
                            }

                            PlaySound = bool__389;
                        }

                        Result = true;
                    }
                }
            }

            if (PlaySound)
            {
                var call__397 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(47);
                call__397.FSFXLibrary.Play(0);
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
            bool bool__398 = (FPlayer > 0);
            if (bool__398)
            {
                var call__399 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                bool__398 = (call__399.FPlayerType != Aow.Units.AoWE.TPlayerType.ptHuman);
            }

            bool bool__400 = bool__398 && ((TSet<Aow.Units.City.TCityModeElement>.Of(Aow.Units.City.TCityModeElement.cmWarmode, Aow.Units.City.TCityModeElement.cmRazed) & FMode) == TSet<Aow.Units.City.TCityModeElement>.Of());
            if (bool__400)
            {
                var call__401 = GetActiveAction();
                bool__400 = !TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__401);
            }

            if (bool__400)
            {
                var call__402 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__403 = call__402.GetPlayerRelation(unchecked((sbyte)FPlayer));
                bool bool__404 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary, Aow.Units.AoWE.TStatus.sNeutral).Contains(call__403);
                if (bool__404)
                {
                    var call__405 = GetActiveAction();
                    bool bool__406 = (call__405 == Aow.Units.City.TCityAction.caMigrate);
                    if (bool__406)
                    {
                        var call__407 = FProduction.FQueue.GetItem(0);
                        var call__408 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(call__407.FParameter);
                        var call__409 = call__408.GetPlayerRelation(unchecked((sbyte)FPlayer));
                        bool__406 = TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__409);
                    }

                    bool__404 = !bool__406;
                }

                Cancel = bool__404;
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
                var call__410 = FResource.TerrainList.GetTerrain(Walk.HN);
                if (((((call__410 >= 0) && (Walk.X >= 0)) && (Walk.Y >= 0)) && (Walk.X < MapLevel.Width)) && (Walk.Y < MapLevel.Height))
                {
                    var call__411 = MapLevel.GetField(Walk.X, Walk.Y);
                    var call__412 = call__411.FindOwnedChild(131607);
                    Army = (Aow.Units.AoWE.TArmyHS)call__412;
                    if ((Army != null) && (!Selection.Contains(Aow.Units.City.TMoveOutOfCitySelectionElement.msSkipOwnPlayer) || (Army.FArmy.FPlayer != FPlayer)))
                    {
                        var call__413 = GetLhx();
                        var call__414 = Army.PlaceEx(unchecked((sbyte)Walk.X), unchecked((sbyte)Walk.Y), unchecked((sbyte)call__413), TSet<Aow.Units.AoWE.TArmyPlaceModeElement>.Of(Aow.Units.AoWE.TArmyPlaceModeElement.apSafe, Aow.Units.AoWE.TArmyPlaceModeElement.apScanArea));
                        Result = call__414;
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
                ref var w__415 = ref Walk;
                Result = 0;
                Count = GetTerrainCount();
                MapLevel = (Aow.Units.HSEngine.TMapLevel)Owner.Owner;
                var call__416 = GetYhx();
                var call__417 = GetXhx();
                Aow.Units.HSEngine.HSEngine.InitHNWalk(ref Walk, call__417, call__416, 0);
                while (w__415.HN < Count)
                {
                    var call__418 = FResource.TerrainList.GetTerrain(w__415.HN);
                    if (((((call__418 >= 0) && (w__415.X >= 0)) && (w__415.Y >= 0)) && (w__415.X < MapLevel.Width)) && (w__415.Y < MapLevel.Height))
                    {
                        var call__419 = MapLevel.GetField(w__415.X, w__415.Y);
                        var call__420 = call__419.FindOwnedChild(131607);
                        Army = (Aow.Units.AoWE.TArmyHS)call__420;
                        if (Army != null)
                        {
                            var call__421 = Army.FArmy.ObtainValue(255);
                            Result += call__421;
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
            var call__422 = GetSize();
            Result = Aow.Units.City.City.UpgradePrices[call__422 - 1];
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
                    int end__423 = FProduction.FQueue.Count - 1;
                    for (; I <= end__423; I += 1)
                    {
                        var call__424 = FProduction.FQueue.GetItem(I);
                        var call__425 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                        if (call__424.FProductionID == call__425)
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
                var call__426 = Aow.Units.AoWE.AoWE.MaxUpgradeLevelReachedRStr;
                var call__427 = Aow.Units.AoWE.AoWE.TranslateRStr(call__426);
                Reason = call__427;
            }

            return Result;
        }

        // City.pas:1371
        public bool Upgrade(bool Queue)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__428 = CanUpgrade(Queue, ref Reason);
            bool bool__429 = call__428;
            if (bool__429)
            {
                var call__430 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__431 = call__430.GetBusy();
                bool__429 = !call__431;
            }

            if (bool__429)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__432 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                Item.FProductionID = call__432;
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
                    var call__433 = GetSize();
                    Result = Aow.Units.City.City.WoodWallPrices[call__433 - 1];
                    break;
                case 1:
                    var call__434 = GetSize();
                    Result = Aow.Units.City.City.StoneWallPrices[call__434 - 1];
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
                    int end__435 = FProduction.FQueue.Count - 1;
                    for (; I <= end__435; I += 1)
                    {
                        var call__436 = FProduction.FQueue.GetItem(I);
                        var call__437 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                        if (call__436.FProductionID == call__437)
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
                var call__438 = Aow.Units.AoWE.AoWE.MaxFortifyLevelReachedRStr;
                var call__439 = Aow.Units.AoWE.AoWE.TranslateRStr(call__438);
                Reason = call__439;
            }

            return Result;
        }

        // City.pas:1415
        public bool Fortify(bool Queue)
        {
            bool Result = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            Aow.Rtl.AnsiString Reason = default;
            var call__440 = CanFortify(Queue, ref Reason);
            bool bool__441 = call__440;
            if (bool__441)
            {
                var call__442 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__443 = call__442.GetBusy();
                bool__441 = !call__443;
            }

            if (bool__441)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__444 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                Item.FProductionID = call__444;
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
            var call__445 = FProduction.UnitProductionGold(Index);
            Result = call__445;
            return Result;
        }

        // City.pas:1435
        public int ProductionTime(int Index)
        {
            int Result = default;
            var call__446 = FProduction.UnitProductionTime(Index);
            Result = call__446;
            return Result;
        }

        // City.pas:1440
        public bool CanProduce(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__447 = FProduction.CanProduceUnit(Index, Queue, ref Reason);
            Result = call__447;
            return Result;
        }

        // City.pas:1445
        public bool Produce(int Index, bool Queue)
        {
            bool Result = default;
            var call__448 = FProduction.ProduceUnit(Index, Queue);
            Result = call__448;
            return Result;
        }

        // City.pas:1450
        public int ResearchCost(int Index)
        {
            int Result = default;
            var call__449 = FProduction.UnitResearchGold(Index);
            Result = call__449;
            return Result;
        }

        // City.pas:1455
        public int ResearchTime(int Index)
        {
            int Result = default;
            var call__450 = FProduction.UnitResearchTime(Index);
            Result = call__450;
            return Result;
        }

        // City.pas:1460
        public bool CanResearch(int Index, bool Queue, ref Aow.Rtl.AnsiString Reason)
        {
            bool Result = default;
            var call__451 = FProduction.CanResearchUnit(Index, Queue, ref Reason);
            Result = call__451;
            return Result;
        }

        // City.pas:1465
        public bool Research(int Index, bool Queue)
        {
            bool Result = default;
            var call__452 = FProduction.ResearchUnit(Index, Queue);
            Result = call__452;
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
            var call__453 = CanTrade(Queue, ref Reason);
            bool bool__454 = call__453;
            if (bool__454)
            {
                var call__455 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__456 = call__455.GetBusy();
                bool__454 = !call__456;
            }

            if (bool__454)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__457 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caTrade);
                Item.FProductionID = call__457;
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
            var call__458 = CanLoot(ref Reason);
            bool bool__459 = call__458;
            if (bool__459)
            {
                var call__460 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__461 = call__460.GetBusy();
                bool__459 = !call__461;
            }

            if (bool__459)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__462 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caLoot);
                Item.FProductionID = call__462;
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
                            var call__463 = MapLevel.GetField(Walk.X, Walk.Y);
                            var call__464 = call__463.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
                            City = (Aow.Units.City.TCity)call__464;
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
                var call__465 = Aow.Units.City.City.RaceInCity(unchecked((sbyte)Race), unchecked((sbyte)FPlayer));
                Result = call__465;
                var call__466 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count;
                I = call__466;
                while ((I != 1) && !Result)
                {
                    I -= 1;
                    Player = unchecked((sbyte)I);
                    var call__467 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetDiplomaticRelation(unchecked((sbyte)(Player << 0)), unchecked((sbyte)FPlayer));
                    bool bool__468 = (call__467 == Aow.Units.AoWE.TDiplomaticRelation.drAlliance);
                    if (bool__468)
                    {
                        var call__469 = Aow.Units.City.City.RaceInCity(unchecked((sbyte)Race), unchecked((sbyte)Player));
                        bool__468 = call__469;
                    }

                    Result = bool__468;
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
            var call__470 = CanMigrate(unchecked((sbyte)Race), ref Reason);
            bool bool__471 = call__470;
            if (bool__471)
            {
                var call__472 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__473 = call__472.GetBusy();
                bool__471 = !call__473;
            }

            if (bool__471)
            {
                Item = Aow.Units.AoWE.TProductionItem.Create(null);
                var call__474 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caMigrate);
                Item.FProductionID = call__474;
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
                var call__475 = MigrationTime(unchecked((sbyte)Race));
                Result.FTime = call__475;
                var call__476 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(Race);
                var call__477 = call__476.GetPlayerRelation(unchecked((sbyte)FPlayer));
                Result.FRelation = call__477;
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
                int end__478 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.Count - 1;
                for (; I <= end__478; I += 1)
                {
                    var call__479 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(I);
                    if (call__479.FEnabled)
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

                    var call__480 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(100);
                    if (Chance >= call__480)
                    {
                        MoveArmiesOutOfCity(TSet<Aow.Units.City.TMoveOutOfCitySelectionElement>.Of());
                        Log = Aow.Units.City.TCityRebellionEventLog.Create(null);
                        Log.FPlayer = unchecked((sbyte)FPlayer);
                        var call__481 = GetName();
                        Log.FCityName = call__481;
                        var call__482 = GetXhx();
                        Log.FXhx = call__482;
                        var call__483 = GetYhx();
                        Log.FYhx = call__483;
                        var call__484 = GetLhx();
                        Log.FLhx = call__484;
                        var call__485 = GetYhx();
                        var call__486 = GetLhx();
                        var call__487 = GetXhx();
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__487), unchecked((sbyte)call__485), unchecked((sbyte)call__486), false);
                        Log.Release();
                        SetPlayer(0);
                        Units = Aow.Units.AoWE.TUnitList.Create(null);
                        GenerateRebelUnits(Units);
                        {
                            I = Units.Count - 1;
                            byte end__488 = 0;
                            for (; I >= end__488; I -= 1)
                            {
                                var call__489 = GetLhx();
                                var call__490 = GetYhx();
                                var call__491 = GetXhx();
                                var call__492 = Units.GetUnit(I);
                                call__492.Place(call__491, call__490, call__489, 0, TSet<Aow.Units.AoWE.TUnitPlaceModeElement>.Of());
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
            var call__493 = GetRazed();
            if (((!call__493 && (FDefendersMode != Aow.Units.City.TDefendersGenerateMode.dgNone)) && !FMode.Contains(Aow.Units.City.TCityModeElement.cmWarmode)) && (FPlayer == 0))
            {
                Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                ListArmies(Armies);
                if (FDefendersMode == Aow.Units.City.TDefendersGenerateMode.dgRandom)
                {
                    var call__494 = Aow.Rtl.Rtl.Random(3);
                    FDefendersMode = (Aow.Units.City.TDefendersGenerateMode)(call__494 + 2);
                }

                Value = ((((int)FDefendersMode) - 2) * 25) + 25;
                Value += ((GetSize() - 1) * Value) / 2;
                {
                    I = 0;
                    int end__495 = Armies.Count - 1;
                    for (; I <= end__495; I += 1)
                    {
                        var call__496 = Armies.GetArmyHS(I);
                        var call__497 = call__496.FArmy.ObtainValue(255);
                        Value -= call__497;
                    }
                }

                if (Value > 0)
                {
                    var call__498 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(1048575);
                    Aow.Rtl.Rtl.RandSeed = call__498;
                    var call__499 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                    var call__500 = GetLhx();
                    var call__501 = GetYhx();
                    var call__502 = GetXhx();
                    Aow.Units.AoWE.AoWE.PlaceRandomUnits(call__502, call__501, call__500, 0, call__499.FUnitIndexes, FUpgradeLevel, Value, TSet<Aow.Units.AoWE.TUnitType>.Of(Aow.Units.AoWE.TUnitType.utHumanoid, Aow.Units.AoWE.TUnitType.utCreature));
                }

                Armies.Free();
            }
        }

        // City.pas:1755
        public void ListIncomeInfo(Aow.Rtl.TStrings Names, Aow.Rtl.TStrings Values)
        {
            int Income = default;
            Income = GetSize() * 5;
            var call__503 = Aow.Units.AoWE.AoWE.BaseIncomeRStr;
            var call__504 = Aow.Units.AoWE.AoWE.TranslateRStr(call__503);
            Names.Add(call__504);
            Values.Add(Aow.Rtl.Rtl.IntToStr(Income));
            Income += GetCropCount() * 2;
            var call__505 = GetCropCount();
            var call__506 = Aow.Units.AoWE.AoWE.XCropFieldsRStr;
            var call__507 = Aow.Units.AoWE.AoWE.TranslateRStr(call__506);
            var call__508 = Aow.Rtl.Rtl.Format(call__507, [Aow.Rtl.Rtl.IntToStr(call__505)]);
            Names.Add(call__508);
            var call__509 = GetCropCount();
            Values.Add(Aow.Rtl.Rtl.IntToStr(call__509 * 2));
            if (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(GetActiveAction()))
            {
                var call__510 = Aow.Units.AoWE.AoWE.ProducingMerchandiseRStr;
                var call__511 = Aow.Units.AoWE.AoWE.TranslateRStr(call__510);
                Names.Add(call__511);
                Values.Add(Aow.Rtl.Rtl.IntToStr(Aow.Rtl.Rtl.Round(Income * 0.25)));
                Income += Aow.Rtl.Rtl.Round(Income * 0.25);
            }

            if (FEnchantments.Contains(Aow.Units.City.TCityEnchantmentID.ceiGoldRush))
            {
                var call__512 = Aow.Units.AoWE.AoWE.GoldRushRStr;
                var call__513 = Aow.Units.AoWE.AoWE.TranslateRStr(call__512);
                Names.Add(call__513);
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
                var call__514 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__515 = call__514.GetPlayerRelation(unchecked((sbyte)FPlayer));
                var call__516 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TStatusRStr[(int)call__515]);
                var call__517 = Aow.Units.AoWE.AoWE.RaceRelationXRStr;
                var call__518 = Aow.Units.AoWE.AoWE.TranslateRStr(call__517);
                var call__519 = Aow.Rtl.Rtl.Format(call__518, [call__516]);
                List.Add(call__519);
                if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                {
                    var call__520 = Aow.Units.AoWE.AoWE.TerrainPenaltyXRStr;
                    var call__521 = Aow.Units.AoWE.AoWE.TranslateRStr(call__520);
                    var call__522 = Aow.Units.AoWE.AoWE.AoWHSSet.TerrainTypes[TerrainType];
                    var call__523 = Aow.Rtl.Rtl.Format(call__521, [call__522]);
                    List.Add(call__523);
                }

                if (Aow.Units.AoWE.AoWE.RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                {
                    var call__524 = Aow.Units.AoWE.AoWE.TerrainBonusXRStr;
                    var call__525 = Aow.Units.AoWE.AoWE.TranslateRStr(call__524);
                    var call__526 = Aow.Units.AoWE.AoWE.AoWHSSet.TerrainTypes[TerrainType];
                    var call__527 = Aow.Rtl.Rtl.Format(call__525, [call__526]);
                    List.Add(call__527);
                }

                Predictor = Aow.Units.AoWE.TCombatPredictor.Create();
                Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                ListArmies(Armies);
                {
                    I = 0;
                    int end__528 = Armies.Count - 1;
                    for (; I <= end__528; I += 1)
                    {
                        {
                            J = 0;
                            int end__529 = Armies.GetArmyHS(I).FArmy.Count - 1;
                            for (; J <= end__529; J += 1)
                            {
                                var call__530 = Armies.GetArmyHS(I);
                                var call__531 = call__530.FArmy.GetUnit(J);
                                U = call__531;
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
                    int end__532 = Units.Count - 1;
                    for (; J <= end__532; J += 1)
                    {
                        var call__533 = Units.GetUnit(J);
                        Predictor.AddOpponent(call__533, 0);
                    }
                }

                if (Predictor.Execute() == Aow.Units.AoWE.TCombatResult.crVictory)
                {
                    if ((Predictor.GetSuperiority() / 4) >= 75)
                    {
                        var call__534 = Aow.Units.AoWE.AoWE.StrongRStr;
                        var call__535 = Aow.Units.AoWE.AoWE.TranslateRStr(call__534);
                        var call__536 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                        var call__537 = Aow.Units.AoWE.AoWE.TranslateRStr(call__536);
                        var call__538 = Aow.Rtl.Rtl.Format(call__537, [call__535]);
                        List.Add(call__538);
                    }
                    else
                    {
                        var call__539 = Aow.Units.AoWE.AoWE.AverageRStr;
                        var call__540 = Aow.Units.AoWE.AoWE.TranslateRStr(call__539);
                        var call__541 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                        var call__542 = Aow.Units.AoWE.AoWE.TranslateRStr(call__541);
                        var call__543 = Aow.Rtl.Rtl.Format(call__542, [call__540]);
                        List.Add(call__543);
                    }
                }
                else
                {
                    var call__544 = Aow.Units.AoWE.AoWE.WeakRStr;
                    var call__545 = Aow.Units.AoWE.AoWE.TranslateRStr(call__544);
                    var call__546 = Aow.Units.AoWE.AoWE.OccupationForcesXRStr;
                    var call__547 = Aow.Units.AoWE.AoWE.TranslateRStr(call__546);
                    var call__548 = Aow.Rtl.Rtl.Format(call__547, [call__545]);
                    List.Add(call__548);
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
                    var call__549 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__550 = call__549.GetPlayerRelation(unchecked((sbyte)FPlayer));
                    Relation = call__550;
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
                        int end__551 = Armies.Count - 1;
                        for (; I <= end__551; I += 1)
                        {
                            {
                                J = 0;
                                int end__552 = Armies.GetArmyHS(I).FArmy.Count - 1;
                                for (; J <= end__552; J += 1)
                                {
                                    var call__553 = Armies.GetArmyHS(I);
                                    var call__554 = call__553.FArmy.GetUnit(J);
                                    U = call__554;
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
                            int end__555 = Units.Count - 1;
                            for (; I <= end__555; I += 1)
                            {
                                var call__556 = Units.GetUnit(I);
                                Predictor.AddOpponent(call__556, 0);
                                if (FMode.Contains(Aow.Units.City.TCityModeElement.cmAnarchy))
                                {
                                    var call__557 = Units.GetUnit(I);
                                    Predictor.AddOpponent(call__557, 0);
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

        // City.pas:1944
        public void UpdateImages()
        {
            Aow.Units.AoWE.TRaceResource RaceResource = default;
            int Terrain = default;
            int Offset = default;
            int Size = default;
            int Index = default;
            Aow.Units.ImageLib.TCustomImageLibrary Images = default;
            Terrain = ((Aow.Units.HSEngine.TMapField)Owner).TerrainType;
            if (Terrain == 3)
            {
                Offset = 20;
            }
            else
            {
                if (TSet<byte>.WithBounds(0, 255).With(5).WithRange(7, 9).WithRange(12, 13).Contains((byte)Terrain))
                {
                    Offset = 40;
                }
                else
                {
                    Offset = 0;
                }
            }

            var call__558 = FResource.ImageSequenceList.ImageSequences[0];
            Images = call__558.ImageLibrary;
            if (!GetRazed())
            {
                if (FRace >= 0)
                {
                    var call__559 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                    RaceResource = call__559;
                    Size = ((Aow.Units.City.TCityResource)FResource).FSize;
                    if (RaceResource != null)
                    {
                        var call__560 = RaceResource.GetCityImage(Size, Terrain, ((Aow.Units.HSEngine.TMapField)Owner).Lhx);
                        FCityImage = call__560;
                    }

                    if (FCityImage == null)
                    {
                        if (Images.ImageExist(Offset))
                        {
                            var call__561 = Images[Offset];
                            FCityImage = call__561;
                        }
                        else
                        {
                            var call__562 = Images[0];
                            FCityImage = call__562;
                        }
                    }
                }
                else
                {
                    Index = Offset + 1;
                    if (Images.ImageExist(Index))
                    {
                        var call__563 = Images[Index];
                        FCityImage = call__563;
                    }
                    else
                    {
                        var call__564 = Images[1];
                        FCityImage = call__564;
                    }
                }

                if ((FWallType != 0) && (Ptr<byte>.Unbox(this)[76] != 3))
                {
                    Index = Ptr<byte>.Unbox(this)[76];
                    Index = Index << 1;
                    Index += Offset;
                    if (Images.ImageExist(Index))
                    {
                        var call__565 = Images[Index];
                        FWallFrontImage = call__565;
                        var call__566 = Images[Index + 1];
                        FWallBackImage = call__566;
                    }
                    else
                    {
                        var call__567 = Images[unchecked((int)FWallType) << 1];
                        FWallFrontImage = call__567;
                        var call__568 = Images[(unchecked((int)FWallType) << 1) + 1];
                        FWallBackImage = call__568;
                    }
                }
                else
                {
                    FWallBackImage = null;
                    FWallFrontImage = null;
                }
            }
            else
            {
                FCityImage = null;
                FWallBackImage = null;
                FWallFrontImage = null;
            }

            UpdateFlagID();
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
                var call__569 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                RaceResource = call__569;
                {
                    I = RaceResource.FUnitIndexes.Count - 1;
                    byte end__570 = 0;
                    for (; I >= end__570; I -= 1)
                    {
                        var call__571 = RaceResource.FUnitIndexes[I];
                        if (call__571 != -1)
                        {
                            var call__572 = RaceResource.FUnitIndexes[I];
                            var call__573 = FProduction.FUnitProductions.IndexOf(call__572);
                            if (call__573 == -1)
                            {
                                var call__574 = RaceResource.FUnitIndexes[I];
                                var call__575 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(call__574);
                                var call__576 = GetSize();
                                var call__577 = Aow.Units.City.City.UnitLevelToCityLevel(call__575.FLevel, call__576);
                                UnitLevel = call__577;
                                if ((UnitLevel != -1) && (UnitLevel <= FUpgradeLevel))
                                {
                                    bool bool__578 = (UnitLevel == 1);
                                    if (bool__578)
                                    {
                                        bool bool__579 = (RaceResource.FSpecialUnitIndexes.Count >= 4);
                                        if (bool__579)
                                        {
                                            var call__580 = RaceResource.FUnitIndexes[I];
                                            var call__581 = RaceResource.FSpecialUnitIndexes[3];
                                            bool__579 = (call__580 == call__581);
                                        }

                                        bool__578 = !bool__579;
                                    }

                                    Researched = bool__578;
                                    var call__582 = RaceResource.FUnitIndexes[I];
                                    FProduction.FUnitProductions.Add(call__582, Researched);
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
                var call__583 = GetEnemiesIndependentRelation(unchecked((sbyte)Army.FArmy.FPlayer));
                var call__584 = Combat.FCombatData.FPlayers.GetPlayers(unchecked((sbyte)Combat.FCombatData.FDefenderPlayer));
                call__584.FIndependentRelation = unchecked((byte)call__583);
            }

            var call__585 = Army.GetLhx();
            var call__586 = Army.GetYhx();
            var call__587 = Army.GetXhx();
            Combat.InitDefaultArmies(Army.FArmy, call__587, call__586, call__585);
        }

        // City.pas:2234
        public int GetObtainValue()
        {
            int Result = default;
            var call__588 = GetSize();
            Result = Aow.Units.City.City.UpgradePrices[call__588 - 1] * FUpgradeLevel;
            if (FWallType == 1)
            {
                var call__589 = GetSize();
                Result += Aow.Units.City.City.WoodWallPrices[call__589 - 1];
            }

            if (FWallType == 2)
            {
                var call__590 = GetSize();
                var call__591 = GetSize();
                Result = (Result + Aow.Units.City.City.WoodWallPrices[call__590 - 1]) + Aow.Units.City.City.StoneWallPrices[call__591 - 1];
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
            var call__592 = Armies.Count;
            I = call__592;
            CanJoin = true;
            if (I != 0)
            {
                while ((I != 0) && CanJoin)
                {
                    I -= 1;
                    var call__593 = Armies.GetArmyHS(I);
                    Army = call__593.FArmy;
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
                    var call__594 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                    var call__595 = call__594.GetPlayerRelation(unchecked((sbyte)Player));
                    Relation = unchecked((byte)unchecked((byte)call__595));
                    Result += (Aow.Units.City.City.TCity_JoinAmount__JoinPercent[(int)((Aow.Units.AoWE.TStatus)Relation)] * Value) / 100;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                var call__596 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__597 = call__596.GetPlayerRelation(unchecked((sbyte)Player));
                if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sPolite, Aow.Units.AoWE.TStatus.sFriendly).Contains(call__597))
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
                var call__598 = JoinAmount(unchecked((sbyte)Request.FPlayer));
                Amount = call__598;
                if (Amount != -1)
                {
                    Request.FObject = Aow.Units.AoWE.TNegotiateObject.noCity;
                    var call__599 = GetName();
                    var call__600 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityJoinHeaderRStr);
                    var call__601 = Aow.Rtl.Rtl.Format(call__600, [call__599]);
                    Request.FText = call__601;
                    Request.FGold = Amount;
                    if (Request.FGold == 0)
                    {
                        var call__602 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityFreeJoinRStr);
                        Request.FJoinMessage.Text = call__602;
                    }
                    else
                    {
                        var call__603 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.CityTributeJoinRStr);
                        var call__604 = Aow.Rtl.Rtl.Format(call__603, [Aow.Rtl.Rtl.IntToStr(Request.FGold)]);
                        Request.FJoinMessage.Text = call__604;
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
                var call__605 = JoinAmount(unchecked((sbyte)Offer.FPlayer));
                Amount = call__605;
                if (((Offer.FOffer == Aow.Units.AoWE.TNegotiateOffer.noOfferBribe) && (Amount <= Offer.FGold)) && (Amount != -1))
                {
                    var call__606 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Offer.FPlayer);
                    if (call__606.FGems >= Offer.FGold)
                    {
                        SetPlayer(unchecked((sbyte)Offer.FPlayer));
                        Armies = Aow.Units.AoWE.TArmyHSList.Create(null);
                        ListArmies(Armies);
                        {
                            I = 0;
                            int end__607 = Armies.Count - 1;
                            for (; I <= end__607; I += 1)
                            {
                                var call__608 = Armies.GetArmyHS(I);
                                if (call__608.FArmy.FPlayer == 0)
                                {
                                    var call__609 = Armies.GetArmyHS(I);
                                    call__609.FArmy.SetPlayer(unchecked((sbyte)Offer.FPlayer));
                                }
                            }
                        }

                        Armies.Free();
                        Log = Aow.Units.EventLog.TLocationTextEventLog.Create(null);
                        Log.FPlayer = unchecked((sbyte)FPlayer);
                        var call__610 = GetName();
                        var call__611 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XOverthrownRStr);
                        var call__612 = Aow.Rtl.Rtl.Format(call__611, [call__610]);
                        Log.FText = call__612;
                        var call__613 = GetLhx();
                        var call__614 = GetYhx();
                        var call__615 = GetXhx();
                        Log.SetLocation(unchecked((sbyte)call__615), unchecked((sbyte)call__614), unchecked((sbyte)call__613));
                        var call__616 = GetYhx();
                        var call__617 = GetLhx();
                        var call__618 = GetXhx();
                        Aow.Units.EventLog.EventLog.DistributeLocationEventLog(Log, unchecked((sbyte)FPlayer), unchecked((sbyte)call__618), unchecked((sbyte)call__616), unchecked((sbyte)call__617), false);
                        Log.Release();
                        Offer.Accept();
                    }
                    else
                    {
                        var call__619 = Aow.Units.AoWE.AoWE.NotEnoughGoldRStr;
                        var call__620 = Aow.Units.AoWE.AoWE.TranslateRStr(call__619);
                        Aow.Units.AoWE.AoWE.AoWHSMap.ShowMessage(call__620);
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
                byte end__621 = 0;
                for (; I >= end__621; I -= 1)
                {
                    var call__622 = Units.GetUnit(I);
                    var call__623 = call__622.CanJoin(unchecked((sbyte)Player));
                    if (call__623)
                    {
                        var call__624 = Units.GetUnit(I);
                        Units.DeleteUnit(call__624);
                    }
                }
            }

            // { Original uses the race index as the default relation. }
            var call__625 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, unchecked((sbyte)FRace));
            Result = unchecked((sbyte)call__625);
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
            var call__626 = Aow.Units.AoWE.AoWE.UnitsToIndependentRelation(Units, unchecked((sbyte)FRace));
            Result = unchecked((sbyte)call__626);
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
                var call__627 = Aow.Units.AoWE.AoWE.AoWHSMap.FRaces.GetRace(FRace);
                var call__628 = call__627.GetPlayerRelation(unchecked((sbyte)FPlayer));
                if (TSet<Aow.Units.AoWE.TStatus>.Of(Aow.Units.AoWE.TStatus.sHostile, Aow.Units.AoWE.TStatus.sWary).Contains(call__628))
                {
                    Value = Value * 2;
                }

                var call__629 = Aow.Units.AoWE.AoWE.DiplomaticRelation(unchecked((sbyte)Info.Player), unchecked((sbyte)Info.IndependentRelation), unchecked((sbyte)FPlayer), 0);
                Relation = call__629;
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
            var call__630 = VisibleForPlayer(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
            Visible = call__630;
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
                    var call__631 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__631 != null)
                    {
                        var call__632 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__633 = call__632.GetPreviewImage();
                        Result = call__633;
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
                        var call__634 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__635 = call__634.FImageLibrary.GetSafe(40);
                        Result = call__635;
                    }
                    else
                    {
                        var call__636 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__637 = call__636.FImageLibrary.GetSafe(43);
                        Result = call__637;
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
                    var call__638 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__638 != null)
                    {
                        var call__639 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__640 = call__639.GetPreviewImage();
                        Result = call__640;
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
                        var call__641 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__642 = call__641.FImageLibrary.GetSafe(41);
                        Result = call__642;
                    }
                    else
                    {
                        var call__643 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__644 = call__643.FImageLibrary.GetSafe(44);
                        Result = call__644;
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
                    var call__645 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__645 != null)
                    {
                        var call__646 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__647 = call__646.GetPreviewImage();
                        Result = call__647;
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
                        var call__648 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__649 = call__648.FImageLibrary.GetSafe(42);
                        Result = call__649;
                    }
                    else
                    {
                        var call__650 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__651 = call__650.FImageLibrary.GetSafe(45);
                        Result = call__651;
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
                var call__652 = City.GetSelected();
                if (call__652 && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 6) == 6))
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
                int end__653 = FTerrainList.Count - 1;
                for (; I <= end__653; I += 1)
                {
                    var call__654 = FTerrainList.Terrain[I];
                    if (call__654 >= 0)
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
                var call__655 = Aow.Units.AoWE.AoWE.CityRStr;
                var call__656 = Aow.Units.AoWE.AoWE.TranslateRStr(call__655);
                Result = call__656;
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
            var call__657 = Aow.Units.AoWE.AoWE.CityRebellionRStr;
            var call__658 = Aow.Units.AoWE.AoWE.TranslateRStr(call__657);
            var call__659 = Aow.Rtl.Rtl.Format(call__658, [FCityName]);
            Result = call__659;
            return Result;
        }

        // City.pas:2699
        public override bool Execute(bool Mode, Aow.Units.EventLog.TExecuteTrigger Trigger, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            bool Result = default;
            bool ShowEvent = default;
            ShowEvent = Trigger == Aow.Units.EventLog.TExecuteTrigger.etUser;
            bool bool__660 = ShowEvent;
            if (!bool__660)
            {
                bool bool__661 = (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmTurn);
                if (bool__661)
                {
                    var call__662 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetClassicWindowPopupEvents();
                    bool__661 = call__662.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
                }

                bool__660 = bool__661;
            }

            ShowEvent = bool__660;
            bool bool__663 = ShowEvent;
            if (!bool__663)
            {
                bool bool__664 = (Aow.Units.AoWE.AoWE.AoWHSMap.FPlayMode == Aow.Units.AoWE.TPlayMode.pmSimultaneous);
                if (bool__664)
                {
                    var call__665 = Aow.Units.AoWE.AoWE.AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
                    bool__664 = call__665.Contains(Aow.Units.AoWReg.TWindowPopupEvent.wpRebellion);
                }

                bool__663 = bool__664;
            }

            ShowEvent = bool__663;
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
                var call__666 = Manager.FBudgets.FindChild(131349);
                UpgradeBudget = (Aow.Units.AoWE.TAIBudget)call__666;
                if (UpgradeBudget == null)
                {
                    UpgradeBudget = Aow.Units.City.TAICityUpgradeBudget.Create(null);
                    UpgradeBudget.Owner = Manager.FBudgets;
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }

                var call__667 = Manager.FBudgets.FindChild(131350);
                ResearchBudget = (Aow.Units.AoWE.TAIBudget)call__667;
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
                var call__668 = Manager.FBudgets.FindChild(131351);
                FortifyBudget = (Aow.Units.AoWE.TAIBudget)call__668;
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
            var call__669 = City.FProduction.GetProduceUnitLevelMax();
            Result = call__669 < City.FUpgradeLevel;
            bool bool__670 = Result;
            if (bool__670)
            {
                var call__671 = City.GetActiveAction();
                bool bool__672 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__671);
                if (!bool__672)
                {
                    var call__673 = City.GetActiveAction();
                    bool__672 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__673) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__670 = bool__672;
            }

            Result = bool__670;
            return Result;
        }

        // City.pas:2802
        public bool ValidResearchBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            var call__674 = City.FProduction.GetProduceUnitLevelMax();
            bool bool__675 = (call__674 != City.FUpgradeLevel);
            if (!bool__675)
            {
                var call__676 = City.GetActiveAction();
                bool__675 = (call__676 == Aow.Units.City.TCityAction.caUpgrade);
            }

            Result = bool__675;
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
                byte end__677 = 0;
                for (; I >= end__677; I -= 1)
                {
                    var call__678 = Player.FStructures.GetStructure(I);
                    Structure = call__678;
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
                byte end__679 = 0;
                for (; I >= end__679; I -= 1)
                {
                    var call__680 = Player.FStructures.GetStructure(I);
                    Structure = call__680;
                    if ((Structure is Aow.Units.City.TCity) && ValidResearchCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        Valid = false;
                        {
                            J = City.FProduction.FUnitProductions.Count - 1;
                            byte end__681 = 0;
                            for (; J >= end__681; J -= 1)
                            {
                                var call__682 = City.FProduction.FUnitProductions.GetItem(J);
                                UnitIndex = call__682.FUnitIndex;
                                if (UnitIndex != -1)
                                {
                                    bool bool__683 = Valid;
                                    if (!bool__683)
                                    {
                                        var call__684 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
                                        bool bool__685 = (call__684.FLevel == City.FUpgradeLevel);
                                        if (bool__685)
                                        {
                                            var call__686 = City.FProduction.FUnitProductions.GetItem(J);
                                            bool__685 = !call__686.FResearched;
                                        }

                                        bool bool__687 = bool__685;
                                        if (bool__687)
                                        {
                                            var call__688 = City.ResearchCost(J);
                                            bool__687 = (call__688 <= (ResearchBudget.FGold + UpgradeBudget.FGold));
                                        }

                                        bool__683 = bool__687;
                                    }

                                    Valid = bool__683;
                                }
                            }
                        }

                        if (Valid)
                        {
                            var call__689 = City.GetSize();
                            Priority = (call__689 * 2) + (City.FProductionPriority + (City.FWallType * 3));
                            Cities.AddAB(Aow.Rtl.Rtl.ObjectToInt(City), Priority);
                        }
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                Index = 0;
                var call__690 = Cities[0];
                Value = Aow.Rtl.Rtl.Round(call__690.B * 0.75);
                while (true)
                {
                    bool bool__691 = ((Index + 1) < Cities.Count);
                    if (bool__691)
                    {
                        var call__692 = Cities[Index + 1];
                        bool__691 = (Value <= call__692.B);
                    }

                    if (!bool__691)
                        break;
                    {
                        Index += 1;
                    }
                }

                var call__693 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(Index + 1);
                Index = call__693;
                /* TODO(T-N7) Integer sentinel in a typed class reference requires an override */
                throw new System.NotImplementedException();
                var call__695 = Aow.Units.AoWE.AoWE.AoWHSMap.Random(16777215);
                Aow.Rtl.Rtl.RandSeed = call__695;
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
            var call__698 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__698;
            Result = City.CanUpgrade(false, ref Reason);
            bool bool__699 = Result;
            if (bool__699)
            {
                var call__700 = City.GetSize();
                bool__699 = (call__700 > City.FUpgradeLevel);
            }

            Result = (bool__699 && !((Level == FMaxProductionLevel) && (FProductionLevelCounts[Level - 1] == 1))) && (Level == City.FUpgradeLevel);
            bool bool__701 = Result;
            if (bool__701)
            {
                var call__702 = City.GetActiveAction();
                bool bool__703 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__702);
                if (!bool__703)
                {
                    var call__704 = City.GetActiveAction();
                    bool__703 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__704) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__701 = bool__703;
            }

            Result = bool__701;
            return Result;
        }

        // City.pas:2921
        public bool ValidUpgradeBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            int Level = default;
            var call__705 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__705;
            var call__706 = City.GetSize();
            if ((call__706 <= City.FUpgradeLevel) || (Level != Ptr<byte>.Unbox(City)[77]))
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
                byte end__707 = 0;
                for (; I >= end__707; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__708 = Player.FStructures.GetStructure(I);
                    Structure = call__708;
                    if ((Structure is Aow.Units.City.TCity) && ValidUpgradeCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        var call__709 = City.GetSize();
                        Priority = (call__709 * 2) + (City.FProductionPriority + (City.FWallType * 3));
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
                    var call__711 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caUpgrade);
                    Production.FProductionID = call__711;
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
                byte end__712 = 0;
                for (; I >= end__712; I -= 1)
                {
                    var call__713 = Player.FStructures.GetStructure(I);
                    Structure = call__713;
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
            bool bool__714 = (Result && (City.FWallType == 0));
            if (!bool__714)
            {
                bool bool__715 = (City.FWallType == 1);
                if (bool__715)
                {
                    var call__716 = City.GetSize();
                    bool__715 = (call__716 > 2);
                }

                bool__714 = ((bool__715 && (City.FUpgradeLevel > 1)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay >= 30));
            }

            Result = bool__714;
            bool bool__717 = Result;
            if (bool__717)
            {
                var call__718 = City.GetActiveAction();
                bool bool__719 = TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade).Contains(call__718);
                if (!bool__719)
                {
                    var call__720 = City.GetActiveAction();
                    bool__719 = (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caProduce).Contains(call__720) && (City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal));
                }

                bool__717 = bool__719;
            }

            Result = bool__717;
            return Result;
        }

        // City.pas:3009
        public bool ValidFortifyBudgetCity(Aow.Units.City.TCity City)
        {
            bool Result = default;
            bool bool__721 = (City.FWallType == 0);
            if (!bool__721)
            {
                bool bool__722 = (City.FWallType == 1);
                if (bool__722)
                {
                    var call__723 = City.GetSize();
                    bool__722 = (call__723 > 2);
                }

                bool__721 = ((bool__722 && (City.FUpgradeLevel > 1)) && (Aow.Units.AoWE.AoWE.AoWHSMap.FDay >= 30));
            }

            Result = bool__721;
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
                byte end__724 = 0;
                for (; I >= end__724; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__725 = Player.FStructures.GetStructure(I);
                    Structure = call__725;
                    if ((Structure is Aow.Units.City.TCity) && ValidFortifyCity((Aow.Units.City.TCity)Structure))
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        var call__726 = City.GetSize();
                        Priority = (call__726 * 2) + City.FProductionPriority;
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
                    var call__728 = City.FortifyCost();
                    Player.FAIBudget.BudgetExpenseMade(call__728, 0, Budget, Aow.Units.AoWE.TBudgetExpenseMode.bemBudget);
                    Production = Aow.Units.AoWE.TProductionItem.Create(null);
                    var call__729 = Aow.Units.City.City.CityActionToProductionID(Aow.Units.City.TCityAction.caFortify);
                    Production.FProductionID = call__729;
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
                byte end__730 = 0;
                for (; I >= end__730; I -= 1)
                {
                    var call__731 = Player.FStructures.GetStructure(I);
                    Structure = call__731;
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
                byte end__732 = 0;
                for (; I >= end__732; I -= 1)
                {
                    var call__733 = FControl.FPlayer.FStructures.GetStructure(I);
                    Structure = call__733;
                    if (Structure is Aow.Units.City.TCity)
                    {
                        City = (Aow.Units.City.TCity)Structure;
                        {
                            var w__734 = (Aow.Units.City.TCity)Structure;
                            var call__735 = w__734.GetSize();
                            if (call__735 > w__734.FUpgradeLevel)
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
                byte end__736 = 0;
                for (; I >= end__736; I -= 1)
                {
                    var call__737 = FControl.FPlayer.FStructures.GetStructure(I);
                    if (call__737 is Aow.Units.City.TCity)
                    {
                        FCityCount += 1;
                        var call__738 = FControl.FPlayer.FStructures.GetStructure(I);
                        City = (Aow.Units.City.TCity)call__738;
                        if (TSet<Aow.Units.City.TCityAction>.Of(Aow.Units.City.TCityAction.caNone, Aow.Units.City.TCityAction.caTrade, Aow.Units.City.TCityAction.caProduce).Contains(City.GetActiveAction()))
                        {
                            Available += (City.GetSize() + 1) / 2;
                            var call__739 = City.FProduction.GetProduceUnitLevelMax();
                            Level = call__739;
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
            var call__740 = Aow.Units.AoWE.AoWE.AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__741 = call__740.FindNoneTransparentHS(typeof(Aow.Units.City.TCity));
            Result = (Aow.Units.City.TCity)call__741;
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
            var call__742 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
            P = call__742;
            I = 0;
            while (true)
            {
                var call__743 = P.FStructures.Count;
                if (!((I != call__743) && !Result))
                    break;
                {
                    var call__744 = P.FStructures.GetStructure(I);
                    if (call__744 is Aow.Units.City.TCity)
                    {
                        var call__745 = P.FStructures.GetStructure(I);
                        var call__746 = ((Aow.Units.City.TCity)call__745).GetBuilding();
                        bool bool__747 = !call__746;
                        if (bool__747)
                        {
                            var call__748 = P.FStructures.GetStructure(I);
                            bool__747 = (((Aow.Units.City.TCity)call__748).FRace == Race);
                        }

                        Result = bool__747;
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
                var call__749 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__749;
                Result = 0;
                {
                    I = 0;
                    int end__750 = P.FStructures.Count - 1;
                    for (; I <= end__750; I += 1)
                    {
                        var call__751 = P.FStructures.GetStructure(I);
                        Structure = call__751;
                        var call__752 = Structure.ClassType.ClassID();
                        if ((call__752 == 131336) && (Structure.FPlayer == Player))
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
                var call__753 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__753;
                Result = 0;
                {
                    I = 0;
                    int end__754 = P.FStructures.Count - 1;
                    for (; I <= end__754; I += 1)
                    {
                        var call__755 = P.FStructures.GetStructure(I);
                        Structure = call__755;
                        var call__756 = Structure.ClassType.ClassID();
                        if ((call__756 == 131336) && (Structure.FPlayer == Player))
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
                var w__757 = (Aow.Units.AoWE.TUnitProduction)Item1;
                var call__758 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__757.FUnitIndex);
                if (call__758 != null)
                {
                    var call__759 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__757.FUnitIndex);
                    Value1 = call__759.FLevel * 2;
                    if (Aow.Units.City.City.SortingCity.FRace != -1)
                    {
                        var call__760 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                        if (call__760.FSpecialUnitIndexes.Count > 3)
                        {
                            var call__761 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                            var call__762 = call__761.FSpecialUnitIndexes[3];
                            if (call__762 == w__757.FUnitIndex)
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
                var w__763 = (Aow.Units.AoWE.TUnitProduction)Item2;
                var call__764 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__763.FUnitIndex);
                if (call__764 != null)
                {
                    var call__765 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(w__763.FUnitIndex);
                    Value2 = call__765.FLevel * 2;
                    if (Aow.Units.City.City.SortingCity.FRace != -1)
                    {
                        var call__766 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                        if (call__766.FSpecialUnitIndexes.Count > 3)
                        {
                            var call__767 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(Aow.Units.City.City.SortingCity.FRace);
                            var call__768 = call__767.FSpecialUnitIndexes[3];
                            if (call__768 == w__763.FUnitIndex)
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
