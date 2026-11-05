// Translated from City.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using Aow.Units.NetworkE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.Animation;
using Aow.Units.AoWReg;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.Crops;

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
    // FCity
    public partial class TCityProductionControl : TProductionControl
    {
        public TCityProductionControl(TECustomNode owner) : base(owner)
        { }

        public TCity FCity;
        // City.pas:573
        public override void ExecuteCancelProduction()
        {
            if (FStatus == TProductionControlStatus.pcsProducing)
            {
                var call__1 = FQueue.GetItem(0);
                var call__2 = City.ProductionIDToCityAction(call__1.FProductionID);
                switch (call__2)
                {
                    case TCityAction.caLoot:
                        if (FSettings.FTurnsLeft == FSettings.FTurnsTotal)
                        {
                            var call__3 = GetPlayer();
                            var call__4 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__5 = call__4.GetPlayerRelationValue(call__3);
                            var call__6 = GetPlayer();
                            var call__7 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__7.SetPlayerRelationValue(call__6, call__5 - -30);
                        }
                        else
                        {
                            var call__8 = GetPlayer();
                            var call__9 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__10 = call__9.GetPlayerRelationValue(call__8);
                            var call__11 = GetPlayer();
                            var call__12 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__12.SetPlayerRelationValue(call__11, call__10 + 20);
                        }

                        break;
                    case TCityAction.caMigrate:
                        if (FSettings.FTurnsLeft == FSettings.FTurnsTotal)
                        {
                            var call__13 = GetPlayer();
                            var call__14 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__15 = call__14.GetPlayerRelationValue(call__13);
                            var call__16 = GetPlayer();
                            var call__17 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__17.SetPlayerRelationValue(call__16, call__15 - -15);
                        }
                        else
                        {
                            var call__18 = GetPlayer();
                            var call__19 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            var call__20 = call__19.GetPlayerRelationValue(call__18);
                            var call__21 = GetPlayer();
                            var call__22 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                            call__22.SetPlayerRelationValue(call__21, call__20 + 10);
                        }

                        break;
                }
            }

            base.ExecuteCancelProduction();
        }

        // City.pas:434
        public override bool ExecuteProduction(TProductionItem Item)
        {
            bool Result = default;
            TLocationTextEventLog Log = default;
            Result = base.ExecuteProduction(Item);
            if (Result)
            {
                var call__23 = City.ProductionIDToCityAction(Item.FProductionID);
                switch (call__23)
                {
                    case TCityAction.caTrade:
                        FSettings.FTurnsTotal = (sbyte)Item.FParameter;
                        FSettings.FTurnsLeft = 1;
                        var call__24 = GetPlayer();
                        FSettings.FPlayer = call__24;
                        FStatus = TProductionControlStatus.pcsProducing;
                        break;
                    case TCityAction.caFortify:
                        var call__25 = FCity.FortifyTime();
                        FSettings.FTurnsTotal = (sbyte)call__25;
                        FSettings.FTurnsLeft = FSettings.FTurnsTotal;
                        var call__26 = GetPlayer();
                        FSettings.FPlayer = call__26;
                        var call__27 = FCity.FortifyCost();
                        FSettings.FGold = call__27;
                        var call__28 = GetPlayer();
                        var call__29 = AoWHSMap.FPlayers.GetPlayers(call__28);
                        var call__30 = GetPlayer();
                        var call__31 = AoWHSMap.FPlayers.GetPlayers(call__30);
                        call__31.SetGems(call__29.FGems - FSettings.FGold);
                        FStatus = TProductionControlStatus.pcsProducing;
                        break;
                    case TCityAction.caUpgrade:
                        var call__32 = FCity.UpgradeTime();
                        FSettings.FTurnsTotal = (sbyte)call__32;
                        FSettings.FTurnsLeft = FSettings.FTurnsTotal;
                        var call__33 = GetPlayer();
                        FSettings.FPlayer = call__33;
                        var call__34 = FCity.UpgradeCost();
                        FSettings.FGold = call__34;
                        var call__35 = GetPlayer();
                        var call__36 = AoWHSMap.FPlayers.GetPlayers(call__35);
                        var call__37 = GetPlayer();
                        var call__38 = AoWHSMap.FPlayers.GetPlayers(call__37);
                        call__38.SetGems(call__36.FGems - FSettings.FGold);
                        FStatus = TProductionControlStatus.pcsProducing;
                        break;
                    case TCityAction.caLoot:
                        var call__39 = FCity.LootTime();
                        FSettings.FTurnsTotal = (sbyte)call__39;
                        FSettings.FTurnsLeft = FSettings.FTurnsTotal;
                        var call__40 = GetPlayer();
                        FSettings.FPlayer = call__40;
                        FSettings.FGold = 0;
                        var call__41 = GetPlayer();
                        var call__42 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__43 = call__42.GetPlayerRelationValue(call__41);
                        var call__44 = GetPlayer();
                        var call__45 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__45.SetPlayerRelationValue(call__44, call__43 + -30);
                        Log = new TLocationTextEventLog(null);
                        var call__46 = GetPlayer();
                        Log.FPlayer = call__46;
                        var call__47 = FCity.GetName();
                        var call__48 = TranslateRStr(CityLootingRStr);
                        var call__49 = Rtl.Rtl.Format(call__48, [call__47]);
                        Log.FText = call__49;
                        var call__50 = FCity.GetLhx();
                        var call__51 = FCity.GetYhx();
                        var call__52 = FCity.GetXhx();
                        Log.SetLocation(call__52, call__51, call__50);
                        var call__53 = FCity.GetYhx();
                        var call__54 = FCity.GetLhx();
                        var call__55 = FCity.GetXhx();
                        var call__56 = GetPlayer();
                        DistributeLocationEventLog(Log, call__56, call__55, call__53, call__54, false);
                        Log.Release();
                        FCity.FOverlay.SetMode(TCityOverlayMode.omLooting);
                        FStatus = TProductionControlStatus.pcsProducing;
                        break;
                    case TCityAction.caMigrate:
                        var call__57 = FCity.MigrationTime((sbyte)Item.FParameter);
                        FSettings.FTurnsTotal = (sbyte)call__57;
                        FSettings.FTurnsLeft = FSettings.FTurnsTotal;
                        var call__58 = GetPlayer();
                        FSettings.FPlayer = call__58;
                        FStatus = TProductionControlStatus.pcsProducing;
                        var call__59 = GetPlayer();
                        var call__60 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__61 = call__60.GetPlayerRelationValue(call__59);
                        var call__62 = GetPlayer();
                        var call__63 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__63.SetPlayerRelationValue(call__62, call__61 + -15);
                        break;
                }
            }

            return Result;
        }

        // City.pas:645
        public override void GetLocation(ref sbyte X, ref sbyte Y, ref sbyte L)
        {
            var call__64 = FCity.GetXhx();
            X = call__64;
            var call__65 = FCity.GetYhx();
            Y = call__65;
            var call__66 = FCity.GetLhx();
            L = call__66;
        }

        // City.pas:707
        public override sbyte GetPlayer()
        {
            sbyte Result = default;
            Result = FCity.FPlayer;
            return Result;
        }

        // City.pas:604
        public override bool LoopableProduction(TProductionItem A1)
        {
            bool Result = default;
            Result = base.LoopableProduction(A1);
            var call__67 = City.ProductionIDToCityAction(A1.FProductionID);
            switch (call__67)
            {
                case TCityAction.caMigrate:
                    Result = false;
                    break;
                case TCityAction.caLoot:
                    Result = false;
                    break;
                case TCityAction.caFortify:
                    Result = false;
                    break;
                case TCityAction.caUpgrade:
                    Result = false;
                    break;
            }

            return Result;
        }

        // City.pas:595
        public override bool LockQueueProduction(TProductionItem A1)
        {
            bool Result = default;
            Result = base.LockQueueProduction(A1);
            var call__68 = City.ProductionIDToCityAction(A1.FProductionID);
            switch (call__68)
            {
                case TCityAction.caMigrate:
                    Result = true;
                    break;
                case TCityAction.caLoot:
                    Result = true;
                    break;
            }

            return Result;
        }

        // City.pas:675
        public override bool UnitProduced()
        {
            bool Result = default;
            TCity City = default;
            bool Valid = default;
            int Count = default;
            TMapLevel Level = default;
            THNWalkData Walk = default;
            TCityProductionEventLog Log = default;
            City = FCity;
            Valid = false;
            Count = City.GetTerrainCount();
            Level = (TMapLevel)City.Owner.Owner;
            var call__69 = City.GetYhx();
            var call__70 = City.GetXhx();
            InitHNWalk(ref Walk, call__70, call__69, 0);
            while (Walk.HN < Count && !Valid)
            {
                var call__71 = City.FResource.TerrainList.Terrain[Walk.HN];
                if (call__71 >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < Level.Width && Walk.Y < Level.Height)
                {
                    var call__72 = City.GetLhx();
                    var call__73 = FSettings.FUnit.Place(Walk.X, Walk.Y, call__72, City.FPlayer, TSet<TUnitPlaceModeElement>.Of());
                    Valid = call__73 != null;
                }

                WalkToNextHN(ref Walk);
            }

            if (!Valid)
            {
                var call__74 = City.GetLhx();
                var call__75 = City.GetYhx();
                var call__76 = City.GetXhx();
                var call__77 = FSettings.FUnit.Place(call__76, call__75, call__74, City.FPlayer, TSet<TUnitPlaceModeElement>.Of(TUnitPlaceModeElement.pmScanArea));
                Valid = call__77 != null;
            }

            Log = new TCityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var call__78 = FSettings.FUnit.GetName();
            var call__79 = FCity.GetName();
            var call__80 = TranslateRStr(XProducedAtYRStr);
            var call__81 = Rtl.Rtl.Format(call__80, [call__78, call__79]);
            Log.FText = call__81;
            var call__82 = City.GetLhx();
            var call__83 = City.GetYhx();
            var call__84 = City.GetXhx();
            Log.SetLocation(call__84, call__83, call__82);
            var call__85 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__85.FEventLogbook.AddEvent(Log);
            Log.Release();
            Result = Valid;
            return Result;
        }

        // City.pas:658
        public override void UnitResearched()
        {
            TCity City = default;
            TCityProductionEventLog Log = default;
            base.UnitResearched();
            City = FCity;
            Log = new TCityProductionEventLog(null);
            Log.FPlayer = City.FPlayer;
            var call__86 = FUnitProductions.GetItem(FSettings.FUnitProductionIndex);
            var call__87 = AoWHSSet.FUnitResources.GetUnitResource(call__86.FUnitIndex);
            var call__88 = call__87.GetUnitName();
            var call__89 = FCity.GetName();
            var call__90 = TranslateRStr(XInstalledAtYRStr);
            var call__91 = Rtl.Rtl.Format(call__90, [call__88, call__89]);
            Log.FText = call__91;
            var call__92 = City.GetLhx();
            var call__93 = City.GetYhx();
            var call__94 = City.GetXhx();
            Log.SetLocation(call__94, call__93, call__92);
            var call__95 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
            call__95.FEventLogbook.AddEvent(Log);
            Log.Release();
        }

        // City.pas:639
        public override void Update()
        {
            base.Update();
            if (FUpdateCount == 0)
            {
                FCity.Update();
            }
        }

        // City.pas:652
        public override TUnit CreateUnitProduction(int A1)
        {
            TUnit Result = default;
            Result = base.CreateUnitProduction(A1);
            if (Result != null && FCity.FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
            {
                Result.SetRank(TUnitRank.urSilver);
            }

            return Result;
        }

        // City.pas:496
        public override void NewTurn(sbyte Player)
        {
            TCityProductionEventLog Log = default;
            base.NewTurn(Player);
            if (FStatus == TProductionControlStatus.pcsProducing && Player == FSettings.FPlayer && FSettings.FTurnsLeft == 0)
            {
                var call__96 = FQueue.GetItem(0);
                var call__97 = City.ProductionIDToCityAction(call__96.FProductionID);
                switch (call__97)
                {
                    case TCityAction.caTrade:
                        ProductionDone();
                        break;
                    case TCityAction.caUpgrade:
                        BeginUpdate();
                        FCity.SetUpgradeLevel((byte)(FCity.FUpgradeLevel + 1));
                        var call__98 = GetPlayer();
                        var call__99 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__100 = call__99.GetPlayerRelationValue(call__98);
                        var call__101 = GetPlayer();
                        var call__102 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__102.SetPlayerRelationValue(call__101, call__100 + 5);
                        ProductionDone();
                        Log = new TCityProductionEventLog(null);
                        var call__103 = GetPlayer();
                        Log.FPlayer = call__103;
                        var call__104 = FCity.GetName();
                        var call__105 = TranslateRStr(XUpgradedRStr);
                        var call__106 = Rtl.Rtl.Format(call__105, [call__104]);
                        Log.FText = call__106;
                        var call__107 = FCity.GetLhx();
                        var call__108 = FCity.GetYhx();
                        var call__109 = FCity.GetXhx();
                        Log.SetLocation(call__109, call__108, call__107);
                        var call__110 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__110.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        EndUpdate();
                        break;
                    case TCityAction.caFortify:
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

                        Log = new TCityProductionEventLog(null);
                        var call__111 = GetPlayer();
                        Log.FPlayer = call__111;
                        var call__112 = FCity.GetName();
                        var call__113 = TranslateRStr(CityFortifiedRStr);
                        var call__114 = Rtl.Rtl.Format(call__113, [call__112]);
                        Log.FText = call__114;
                        var call__115 = FCity.GetLhx();
                        var call__116 = FCity.GetYhx();
                        var call__117 = FCity.GetXhx();
                        Log.SetLocation(call__117, call__116, call__115);
                        var call__118 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__118.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        FCity.UpdateImages();
                        var call__119 = GetPlayer();
                        var call__120 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__121 = call__120.GetPlayerRelationValue(call__119);
                        var call__122 = GetPlayer();
                        var call__123 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__123.SetPlayerRelationValue(call__122, call__121 + 5);
                        ProductionDone();
                        EndUpdate();
                        break;
                    case TCityAction.caLoot:
                        BeginUpdate();
                        Log = new TCityProductionEventLog(null);
                        var call__124 = GetPlayer();
                        Log.FPlayer = call__124;
                        var call__125 = FCity.GetName();
                        var call__126 = TranslateRStr(CityLootedRStr);
                        var call__127 = Rtl.Rtl.Format(call__126, [call__125, IntToStr(FCity.FIncome * 3)]);
                        Log.FText = call__127;
                        var call__128 = FCity.GetLhx();
                        var call__129 = FCity.GetYhx();
                        var call__130 = FCity.GetXhx();
                        Log.SetLocation(call__130, call__129, call__128);
                        var call__131 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__131.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        var call__132 = GetPlayer();
                        var call__133 = AoWHSMap.FPlayers.GetPlayers(call__132);
                        var call__134 = GetPlayer();
                        var call__135 = AoWHSMap.FPlayers.GetPlayers(call__134);
                        call__135.SetGems(call__133.FGems + FCity.FIncome * 3);
                        FCity.FOverlay.SetMode(TCityOverlayMode.omIdle);
                        FCity.SetRazed(true);
                        EndUpdate();
                        break;
                    case TCityAction.caMigrate:
                        BeginUpdate();
                        Log = new TCityProductionEventLog(null);
                        var call__136 = GetPlayer();
                        Log.FPlayer = call__136;
                        var call__137 = FCity.GetName();
                        var call__138 = FQueue.GetItem(0);
                        var call__139 = TranslateRStr(RaceNamesPluralRStr[call__138.FParameter]);
                        var call__140 = TranslateRStr(CityMigratedToRStr);
                        var call__141 = Rtl.Rtl.Format(call__140, [call__137, call__139]);
                        Log.FText = call__141;
                        var call__142 = FCity.GetLhx();
                        var call__143 = FCity.GetYhx();
                        var call__144 = FCity.GetXhx();
                        Log.SetLocation(call__144, call__143, call__142);
                        var call__145 = AoWHSMap.FPlayers.GetPlayers(Log.FPlayer);
                        call__145.FEventLogbook.AddEvent(Log);
                        Log.Release();
                        var call__146 = FQueue.GetItem(0);
                        FCity.SetRace((sbyte)call__146.FParameter);
                        var call__147 = GetPlayer();
                        var call__148 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        var call__149 = call__148.GetPlayerRelationValue(call__147);
                        var call__150 = GetPlayer();
                        var call__151 = AoWHSMap.FRaces.GetRace(FCity.FRace);
                        call__151.SetPlayerRelationValue(call__150, call__149 + 10);
                        FUnitProductions.Clear();
                        FCity.SetUpgradeLevel(FCity.FUpgradeLevel);
                        ProductionDone();
                        EndUpdate();
                        break;
                }
            }
        }

        // City.pas:408
        public override bool CanExecuteProduction(TProductionItem Item, ref AnsiString Reason)
        {
            bool Result = default;
            var call__152 = City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__152)
            {
                case TCityAction.caTrade:
                    Result = true;
                    break;
                case TCityAction.caFortify:
                    var call__153 = GetPlayer();
                    var call__154 = AoWHSMap.FPlayers.GetPlayers(call__153);
                    var call__155 = FCity.FortifyCost();
                    if (call__154.FGems >= call__155)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__156 = NotEnoughGoldRStr;
                        var call__157 = TranslateRStr(call__156);
                        Reason = call__157;
                    }

                    break;
                case TCityAction.caMigrate:
                    var call__158 = FCity.CanMigrate((sbyte)Item.FParameter, ref Reason);
                    Result = call__158;
                    break;
                case TCityAction.caLoot:
                    var call__159 = FCity.CanLoot(ref Reason);
                    Result = call__159;
                    break;
                case TCityAction.caUpgrade:
                    var call__160 = GetPlayer();
                    var call__161 = AoWHSMap.FPlayers.GetPlayers(call__160);
                    var call__162 = FCity.UpgradeCost();
                    if (call__161.FGems >= call__162)
                    {
                        Result = true;
                    }
                    else
                    {
                        Result = false;
                        var call__163 = NotEnoughGoldRStr;
                        var call__164 = TranslateRStr(call__163);
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
        public override AnsiString GetProductionName(TProductionItem Item)
        {
            AnsiString Result = default;
            Result = base.GetProductionName(Item);
            var call__165 = City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__165)
            {
                case TCityAction.caTrade:
                    Result = TranslateRStr(ProduceMerchandiseRStr);
                    break;
                case TCityAction.caUpgrade:
                    Result = TranslateRStr(UpgradeRStr);
                    break;
                case TCityAction.caMigrate:
                    var call__166 = TranslateRStr(RaceNamesPluralRStr[Item.FParameter]);
                    var call__167 = TranslateRStr(MigrateToXRStr);
                    var call__168 = Rtl.Rtl.Format(call__167, [call__166]);
                    Result = call__168;
                    break;
                case TCityAction.caLoot:
                    Result = TranslateRStr(LootRStr);
                    break;
                case TCityAction.caFortify:
                    Result = TranslateRStr(FortifyRStr);
                    break;
            }

            return Result;
        }

        // City.pas:627
        public override AnsiString GetProductionNameProgressive(TProductionItem Item)
        {
            AnsiString Result = default;
            Result = base.GetProductionNameProgressive(Item);
            var call__169 = City.ProductionIDToCityAction(Item.FProductionID);
            switch (call__169)
            {
                case TCityAction.caTrade:
                    Result = TranslateRStr(ProducingMerchandiseRStr);
                    break;
                case TCityAction.caUpgrade:
                    Result = TranslateRStr(UpgradingRStr);
                    break;
                case TCityAction.caMigrate:
                    var call__170 = TranslateRStr(RaceNamesPluralRStr[Item.FParameter]);
                    var call__171 = TranslateRStr(MigratingToXRStr);
                    var call__172 = Rtl.Rtl.Format(call__171, [call__170]);
                    Result = call__172;
                    break;
                case TCityAction.caLoot:
                    Result = TranslateRStr(LootingRStr);
                    break;
                case TCityAction.caFortify:
                    Result = TranslateRStr(FortifyingRStr);
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
            var call__174 = AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
            var call__175 = FCity.GetSize();
            Result = call__174.FLevel - call__175 / 2 + 1;
            if (Result < 1)
            {
                Result = 1;
            }

            return Result;
        }

        // intf_P1_02.inc:120

    }

    // FCity, FModeStartFrame, FMode, FShellAnimation, FShellEndFrame
    public partial class TCityOverlayHS : TSingleHS
    {
        public TCityOverlayHS(TECustomNode owner) : base(owner)
        {}

        public TCity FCity;
        public int FModeStartFrame;
        public TCityOverlayMode FMode;
        public AnonymousEnum_saSetup FShellAnimation;
        public int FShellEndFrame;
        // City.pas:718
        public override int GetShowPriority()
        {
            int Result = default;
            var call__176 = FCity.GetShowPriority();
            Result = call__176 + 134217728;
            return Result;
        }

        // City.pas:733
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int Xhp = default;
            int Yhp = default;
            int Intensity = default;
            int Index = default;
            int Frame = default;
            TImageSequence Sequence = default;
            HXtoHP(FCity.FXhx, FCity.FYhx, ref Xhp, ref Yhp);
            XhpSurf = Xhp - XhpSurf;
            YhpSurf = Yhp - YhpSurf;
            if (FCity.FEnchantmentsVisible)
            {
                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
                {
                    Intensity = Math.Abs((int)(((uint)AoWHSMap.Frame) >> 1) % 20 - 10) + 50;
                    var call__177 = FCity.FResource.ImageSequenceList.ImageSequences[17];
                    Sequence = call__177;
                    while (Sequence != null)
                    {
                        Index = Sequence.GetFrameIndex(0, 0);
                        var call__178 = FCity.FResource.ImageSequenceList.ImageSequences[17];
                        if (Sequence == call__178)
                        {
                            var call__179 = Sequence.ImageLibrary[Index];
                            call__179.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, IntensityBlendTable.LookupTables[Intensity]);
                        }
                        else
                        {
                            var call__180 = Sequence.ImageLibrary[Index];
                            call__180.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, AlphaBlendTable.LookupTables[Intensity]);
                        }

                        Sequence = Sequence.Next;
                    }
                }

                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
                {
                    Intensity = Math.Abs((int)(((uint)AoWHSMap.Frame) >> 1) % 20 - 10) + 70;
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
                                call__184.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, IntensityBlendTable.LookupTables[Intensity]);
                            }
                        }
                        else
                        {
                            var call__185 = Sequence.GetFrameIndex(0, AoWHSMap.Frame);
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

                if (FCity.FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
                {
                    var call__188 = FCity.FResource.ImageSequenceList.ImageSequences[19];
                    Sequence = call__188;
                    if (Sequence != null)
                    {
                        Intensity = 50;
                        Index = Sequence.GetFrameIndex(0, 0);
                        if (FShellEndFrame != -1 && AoWHSMap.Frame < FShellEndFrame)
                        {
                            switch (FShellAnimation)
                            {
                                case AnonymousEnum_saSetup.saSetup:
                                    Frame = FShellEndFrame - AoWHSMap.Frame;
                                    if (Frame <= 50)
                                    {
                                        Intensity = 50 - Frame;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                                case AnonymousEnum_saSetup.saFade:
                                    Frame = FShellEndFrame - AoWHSMap.Frame;
                                    if (Frame <= 20)
                                    {
                                        Intensity = Intensity + (10 - Math.Abs(Frame - 10)) * 3;
                                    }
                                    else
                                    {
                                        FShellEndFrame = -1;
                                    }

                                    break;
                            }
                        }

                        var call__189 = Sequence.ImageLibrary[Index];
                        call__189.ShowBlended(XhpSurf + Sequence.dXshift, YhpSurf + Sequence.dYshift, SurfaceDesc, IntensityBlendTable.LookupTables[Intensity]);
                    }
                }

                if (FMode == TCityOverlayMode.omLooting)
                {
                    FCity.FResource.ShowComplexImage(XhpSurf, YhpSurf, SurfaceDesc, 22, (uint)(((TMapField)FCity.Owner).Xhx + ((TMapField)FCity.Owner).Yhx * 16));
                }
            }
        }

        // City.pas:721
        public void PlaySetupShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.saSetup;
            FShellEndFrame = AoWHSMap.Frame + 50;
        }

        // City.pas:727
        public void PlayFadeShellAnimation()
        {
            FShellAnimation = AnonymousEnum_saSetup.saFade;
            FShellEndFrame = AoWHSMap.Frame + 20;
        }

        // City.pas:815
        public void SetMode(TCityOverlayMode Mode)
        {
            if (Mode != FMode)
            {
                FModeStartFrame = AoWHSMap.Frame;
                FMode = Mode;
            }
        }

        // Engine.pas:524

    }

    // FRace, FTime, FRelation
    public partial class TMigrationItem : TEObject
    {
        public TMigrationItem(TECustomNode owner) : base(owner)
        { }

        public sbyte FRace;
        public int FTime;
        public TStatus FRelation;
    }

    public partial class TMigrationList : TENode
    {
        public TMigrationList(TECustomNode owner) : base(owner)
        { }

        // City.pas:2711
        public TMigrationItem GetItems(int Index)
        {
            TMigrationItem Result = default;
            Result = (TMigrationItem)FChildren.List[Index];
            return Result;
        }

    }

    // FEnchantmentID, FEnchantmentLocation
    public partial class TCityEnchantment : TGlobalEnchantment
    {
        // City.pas:2722
        public new const int ClassID = 131820;

        public new TCityEnchantmentID FEnchantmentID;
        public TXYL FEnchantmentLocation;
        // City.pas:2727
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwData(Bytes.Of(ref FEnchantmentID, 1), 1, 20);
            Stream.rwInteger(ref FEnchantmentLocation.XYL, 21);
        }

        // City.pas:2716
        public TCityEnchantment(TECustomNode owner) : base(owner)
        {
            FEnchantmentMode = FEnchantmentMode - TSet<TGlobalEnchantmentModeElement>.Of(TGlobalEnchantmentModeElement.gemTrueGlobal, TGlobalEnchantmentModeElement.gemOwned);
        }

        // City.pas:2762
        public override AnsiString GetName()
        {
            AnsiString Result = default;
            var call__190 = TranslateRStr(City.TCityEnchantmentIDRStr[(int)FEnchantmentID]);
            Result = call__190;
            return Result;
        }

        // City.pas:2739
        public override TXYL GetLocation()
        {
            TXYL Result = default;
            Result.X = FEnchantmentLocation.X;
            Result.Y = FEnchantmentLocation.Y;
            Result.L = FEnchantmentLocation.L;
            return Result;
        }

        // City.pas:2767
        public override int GetAIUpkeepPriority()
        {
            int Result = default;
            TCity City = default;
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
            if (FEnchantmentStatus.Contains(TGlobalEnchantmentStatusElement.getActivated))
            {
                Result = base.Deactivate(Player);
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
            var call__194 = call__193.VisibleForPlayer(Player);
            Result = call__194;
            return Result;
        }

        // City.pas:2756
        public TCity GetCity()
        {
            TCity Result = default;
            var call__195 = AoWHSMap.MapContainer.GetField(FEnchantmentLocation.X, FEnchantmentLocation.Y, FEnchantmentLocation.L);
            var call__196 = call__195.FindNoneTransparentHS(typeof(TCity));
            Result = (TCity)call__196;
            return Result;
        }

    }

    // FMode, FRace, FName, FWallType, FUpgradeLevel, FProduction, FFlagID, FEnchantmentList,
    // FEnchantments, FOverlay, FEnchantmentsVisible, FWallBackImage, FWallFrontImage, FCityImage,
    // FDefendersMode
    public partial class TCity : TPlayerCropStructure
    {
        public TCity(TECustomNode owner) : base(owner)
        {
            var call__197 = new TCityProductionControl(null);
            FProduction = call__197;
            FProduction.FCity = this;
            FDefendersMode = TDefendersGenerateMode.dgAverage;
            FRace = -1;
            var call__198 = TranslateRStr(NonameRStr);
            FName = call__198;
            FUpgradeLevel = 1;
            var call__199 = new TCityOverlayHS(null);
            FOverlay = call__199;
            FOverlay.FCity = this;
            var call__200 = new TGlobalEnchantmentList(null);
            FEnchantmentList = call__200;
            FEnchantmentsVisible = false;
        }

        // City.pas:849
        public new const int ClassID = 131336;

        public TSet<TCityModeElement> FMode;
        public sbyte FRace;
        public AnsiString FName;
        public byte FWallType;
        public byte FUpgradeLevel;
        public TCityProductionControl FProduction;
        public new byte FFlagID;
        public TGlobalEnchantmentList FEnchantmentList;
        public TSet<TCityEnchantmentID> FEnchantments;
        public TCityOverlayHS FOverlay;
        public bool FEnchantmentsVisible;
        public ImageLib.TLibraryImage FWallBackImage;
        public ImageLib.TLibraryImage FWallFrontImage;
        public ImageLib.TLibraryImage FCityImage;
        public TDefendersGenerateMode FDefendersMode;
        // City.pas:839
        public override void Destroy()
        {
            if (GetVisible())
            {
                AoWHSMap.DynamicVisibleHS.RemoveChild(FOverlay);
            }

            FOverlay.Free();
            base.Destroy();
            FProduction.Free();
            FEnchantmentList.Free();
            FEnchantmentList = null;
        }

        // City.pas:2157
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwString(ref FName, 12);
            Stream.rwByte(ref Unsafe.As<sbyte, byte>(ref FRace), 13);
            Stream.rwData(Bytes.Of(ref FWallType, 1), 1, 15);
            Stream.rwByte(ref FUpgradeLevel, 16);
            Stream.rwEObject(FProduction, 18);
            RwSet(ref FMode, 1, 22, Stream.rwData, 0);
            Stream.rwData(Bytes.Of(ref FDefendersMode, 1), 1, 27);
            Stream.rwEObject(FEnchantmentList, 29);
        }

        // City.pas:824


        // City.pas:2191
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
        {
            base.MsgProc(ref Msg, Sender);
            if (Msg.MsgID == 537001989)
            {
                if (FRace >= 0)
                {
                    var call__201 = AoWHSMap.FRaces.GetRace(FRace);
                    bool bool__202 = call__201 == null;
                    if (!bool__202)
                    {
                        var call__203 = AoWHSMap.FRaces.GetRace(FRace);
                        bool__202 = !call__203.FEnabled;
                    }

                    if (bool__202)
                    {
                        FRace = -1;
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
        public override void TerrainChanged(in THSTerrainInfo Info)
        {
            base.TerrainChanged(Info);
            UpdateImages();
        }

        // City.pas:2132
        public override void Show(int XhpSurf, int YhpSurf, in TSurfaceDesc SurfaceDesc)
        {
            int X = default;
            int Y = default;
            HXtoHP(FXhx, FYhx, ref X, ref Y);
            XhpSurf = X - XhpSurf;
            YhpSurf = Y - YhpSurf;
            bool bool__204 = !FMode.Contains(TCityModeElement.cmRazed);
            if (!bool__204)
            {
                var call__205 = GetBuilding();
                bool__204 = call__205;
            }

            if (bool__204)
            {
                if ((AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) || !FFogged || !AoWHSMap.FFogEnabled) && FFlagID != 63 && (((TCityResource)FResource).FClaimed || (FFlagID & 32) == 0))
                {
                    var cityResource = (TCityResource)FResource;
                    if (cityResource.FFlagType <= 1)
                    {
                        AoWHSSet.FFlagControl.ShowFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                    else
                    {
                        AoWHSSet.FFlagControl.ShowStructureFlag(cityResource.FFlagX + XhpSurf, cityResource.FFlagY + YhpSurf, cityResource.Engine.HSMap.Frame + X, this.FFlagID, SurfaceDesc);
                    }
                }
            }

            if (FMode.Contains(TCityModeElement.cmTownQuake))
            {
                XhpSurf = XhpSurf + Random(5) - 2;
                YhpSurf = YhpSurf + Random(5) - 2;
            }

            if (!FMode.Contains(TCityModeElement.cmRazed))
            {
                ((TCityResource)FResource).ShowCity(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
            else
            {
                ((TCityResource)FResource).Show(XhpSurf, YhpSurf, SurfaceDesc, this);
            }
        }

        // City.pas:2170
        public override bool PlaceOnMap()
        {
            bool Result = default;
            Result = base.PlaceOnMap();
            if (Result)
            {
                UpdateMapFields((TMapLevel)Owner.Owner, FXhx, FYhx);
            }

            return Result;
        }

        // City.pas:2176
        public override void RemoveFromMap()
        {
            TMapLevel MapLevel = default;
            MapLevel = (TMapLevel)Owner.Owner;
            base.RemoveFromMap();
            UpdateMapFields(MapLevel, FXhx, FYhx);
        }

        // City.pas:2378
        public override void MapFieldMsgProc(ref TMapFieldMsg Msg)
        {
            bool bool__207 = Msg.MsgID == 4364;
            if (bool__207)
            {
                var call__208 = FResource.TerrainList.Terrain[Msg.HN];
                bool__207 = call__208 >= 0;
            }

            if (!bool__207)
            {
                base.MapFieldMsgProc(ref Msg);
            }

            var call__209 = FResource.TerrainList.Terrain[Msg.HN];
            if (call__209 >= 0)
            {
                if (Msg.MsgID == 4364 && Msg.HN == 0)
                {
                    if ((!AoWHSMap.FFogEnabled || !FFogged) && FPlayer >= 0)
                    {
                        if (FPlayer == 0)
                        {
                            Msg.Param[0] = Msg.Param[0].Value + 851968;
                        }
                        else
                        {
                            var call__210 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                            Msg.Param[0] = Msg.Param[0].Value + (call__210.FPlayerRace + 1) * 65536;
                        }
                    }
                    else
                    {
                        Msg.Param[0] = Msg.Param[0].Value + 917504;
                    }

                    Msg.Param[0] = Msg.Param[0].Value + (((TCityResource)FResource).FSize - 1) * 4194304;
                }
                else
                {
                    if (Msg.MsgID == 4609)
                    {
                        Msg.Result = FProduction;
                    }
                    else
                    {
                        if (Msg.MsgID == 4360)
                        {
                            ReturnBattleFieldInfo(ref ((BattleFieldQuery)Msg.Param[0].Ref).Info);
                        }
                        else
                        {
                            if (Msg.MsgID == 4359)
                            {
                                InitCombat(Msg.Param[0].As<TCombat>(), Msg.Param[1].As<TArmyHS>());
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
                                        bool bool__211 = FWallType != 0 && !FMode.Contains(TCityModeElement.cmWarmode);
                                        if (bool__211)
                                        {
                                            var call__212 = GetRazed();
                                            bool__211 = !call__212;
                                        }

                                        if (bool__211)
                                        {
                                            var call__213 = Msg.Param[0].As<TMapField>().FindNoneTransparentHS(typeof(TCity));
                                            if (this == call__213)
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
                                                var call__214 = GetIndependentRelation();
                                                Msg.Result = call__214;
                                            }
                                        }
                                        else
                                        {
                                            if (Msg.MsgID == 4366)
                                            {
                                                var call__215 = IncommingNegotiateRequest(Msg.Param[0].As<TNegotiateRequestEventLog>());
                                                if (call__215)
                                                {
                                                    Msg.Param[1] = 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Msg.MsgID == 4367)
                                                {
                                                    var call__216 = IncommingNegotiateOffer(Msg.Param[0].As<TNegotiateTE>());
                                                    if (call__216)
                                                    {
                                                        Msg.Result = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    if (Msg.MsgID == 4393 && FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
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
                var call__217 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                call__217.FProductionControls.Remove(FProduction);
            }

            FProduction.Deactivate();
            base.Deactivate();
        }

        // City.pas:2184
        public override void UpdateMapField(in THSMapFieldInfo Info)
        {
            base.UpdateMapField(Info);
            var call__218 = FResource.TerrainList.GetTerrain(Info.HN);
            if (call__218 >= 0)
            {
                ((TAoWMapField)Info.MapField).FFieldFlags = ((TAoWMapField)Info.MapField).FFieldFlags | 67108864;
            }
        }

        // City.pas:1246
        public override void MakeVisible(TVisibilityList VisibilityList)
        {
            if (!FStatus.Contains(TMapObjectStatusElement.moVisible))
            {
                base.MakeVisible(VisibilityList);
                if ((TSet<TCityEnchantmentID>.Of(TCityEnchantmentID.ceiGoldRush, TCityEnchantmentID.ceiAntiMagicShell, TCityEnchantmentID.ceiWarmonger) & FEnchantments) != TSet<TCityEnchantmentID>.Of())
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
        public override sbyte GetValidTerrainType(TMapLevel MapLevel, sbyte Xhx, sbyte Yhx)
        {
            sbyte Result = default;
            Result = GetMostFrequentMapFieldTerrainType(MapLevel, Xhx, Yhx, -1);
            return Result;
        }

        // City.pas:2422
        public override ImageLib.TLibraryImage GetTopBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            TImageSequenceList Sequences = default;
            var call__219 = GetActiveAction();
            var call__220 = GetTopCityActionImage(call__219);
            Result = call__220;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__221 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__222 = call__221.FImageLibrary.GetSafe(40);
                        Result = call__222;
                    }
                    else
                    {
                        var call__223 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__224 = call__223.FImageLibrary.GetSafe(43);
                        Result = call__224;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
        public override ImageLib.TLibraryImage GetMiddleBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            TImageSequenceList Sequences = default;
            var call__227 = GetActiveAction();
            var call__228 = GetMiddleCityActionImage(call__227);
            Result = call__228;
            if (Result == null)
            {
                if (FRace != -1)
                {
                    if (GetLhx() == 0)
                    {
                        var call__229 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__230 = call__229.FImageLibrary.GetSafe(41);
                        Result = call__230;
                    }
                    else
                    {
                        var call__231 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__232 = call__231.FImageLibrary.GetSafe(44);
                        Result = call__232;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
        public override ImageLib.TLibraryImage GetBottomBorderImage()
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            TImageSequenceList Sequences = default;
            var call__235 = GetActiveAction();
            var call__236 = GetBottomCityActionImage(call__235);
            Result = call__236;
            if (Result == null)
            {
                if (FRace >= 0)
                {
                    if (GetLhx() == 0)
                    {
                        var call__237 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__238 = call__237.FImageLibrary.GetSafe(42);
                        Result = call__238;
                    }
                    else
                    {
                        var call__239 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__240 = call__239.FImageLibrary.GetSafe(45);
                        Result = call__240;
                    }
                }
                else
                {
                    Result = null;
                }
            }

            if (FMode.Contains(TCityModeElement.cmRazed))
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
        public override AnsiString GetName()
        {
            AnsiString Result = default;
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
                    SetPlayer(-1);
                }

                if (Value)
                {
                    FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmRazed);
                }
                else
                {
                    FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmRazed);
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
            Result = FMode.Contains(TCityModeElement.cmRazed);
            return Result;
        }

        // City.pas:1111
        public override void SetupRazeDefenderAG(TUnitList Armies, TUnitList Units)
        {
            TAIGroup Group = default;
            var call__244 = AoWHSMap.Random(8);
            if (call__244 == 0)
            {
                var call__245 = UnitsToIndependentRelation(Units, 5);
                var call__246 = AoWHSMap.FAIGroupManager.CreateGroup(0, call__245, TGroupBehavior.gbRefuge);
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
        public override TExecuteResult ExecuteTE(TStructureTE A1, TTokenExecuter Executer)
        {
            TExecuteResult Result = default;
            Result = base.ExecuteTE(A1, Executer);
            return Result;
        }

        // City.pas:1134
        public override bool GetDefenseRequirements(sbyte Player, ref TDefenseRequirements Requirements)
        {
            bool Result = default;
            TBattleFieldInfo Info = default;
            int Maximum = default;
            int Minimum = default;
            TStatus Relation = default;
            if ((FMode & TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed)) == TSet<TCityModeElement>.Of())
            {
                SetupBattleFieldInfo(ref Info, Player, 0, TSet<TBattleFieldInfoRequestsElement>.Of(), dtAll - TSet<TDamageType>.Of(TDamageType.dtWall));
                Info.Requests = Info.Requests + TSet<TBattleFieldInfoRequestsElement>.Of(TBattleFieldInfoRequestsElement.bfrIgnorePassiveIndependents);
                var call__247 = GetLhx();
                var call__248 = GetYhx();
                var call__249 = GetXhx();
                AoWHSMap.RetrieveBattleFieldInfo(call__249, call__248, call__247, 10, ref Info);
                Requirements.Strength = Info.Strength;
                Maximum = Round(GetSize() * 5000);
                var call__250 = GetSize();
                var call__251 = AoWHSMap.FPlayers.GetPlayers(Player);
                Minimum = Round((double)(call__250 * 5000) * call__251.FAIControl.FDefenseStrengthMultiplier);
                if (Player != 0)
                {
                    var call__252 = AoWHSMap.FRaces.GetRace(FRace);
                    var call__253 = call__252.GetPlayerRelation(Player);
                    Relation = call__253;
                }
                else
                {
                    Relation = TStatus.sFriendly;
                }

                // { Original condition makes this neutral-player adjustment unreachable. }
                if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(Relation) && Player == FPlayer && FPlayer == 0)
                {
                    Maximum = Round(Maximum) + 7500;
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
        public override TDefensePriority GetDefensePriority()
        {
            TDefensePriority Result = default;
            if ((FMode & TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed)) == TSet<TCityModeElement>.Of())
            {
                if (GetSize() == 1)
                {
                    Result = TDefensePriority.dpLow;
                }
                else
                {
                    Result = TDefensePriority.dpAverage;
                }

                if (FProduction.FStatus == TProductionControlStatus.pcsProducing && Result == TDefensePriority.dpLow)
                {
                    Result = TDefensePriority.dpAverage;
                }

                bool bool__254 = FPlayer != 0;
                if (bool__254)
                {
                    var call__255 = AoWHSMap.FRaces.GetRace(FRace);
                    var call__256 = call__255.GetPlayerRelation(FPlayer);
                    bool__254 = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(call__256);
                }

                if (bool__254)
                {
                    Result = TDefensePriority.dpHigh;
                }
            }
            else
            {
                Result = TDefensePriority.dpNone;
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
                if (FRace < -1 && AoWHSMap.FGameType == TGameType.gtCampaign)
                {
                    I = Math.Abs(FRace) - 3;
                    if (I >= 0 && I < AoWEngine.FCampaign.FRaceHistory.Count)
                    {
                        var call__257 = AoWEngine.FCampaign.FRaceHistory[I];
                        SetRace((sbyte)call__257);
                    }
                    else
                    {
                        var call__258 = AoWHSMap.FRaces.GetRandomRace();
                        SetRace(call__258);
                    }
                }
                else
                {
                    var call__259 = AoWHSMap.FRaces.GetRandomRace();
                    SetRace(call__259);
                }
            }

            if (AoWHSMap.FDay == 1)
            {
                FProduction.FUnitProductions.Clear();
                SetUpgradeLevel(FUpgradeLevel);
                GenerateDefenders();
                if (FWallType == 3)
                {
                    var call__260 = AoWHSMap.Random(3);
                    FWallType = (byte)call__260;
                    UpdateImages();
                }
            }

            CheckRebellion();
            if (FOverlay.FMode == TCityOverlayMode.omShowRazeAnimation)
            {
                FOverlay.SetMode(TCityOverlayMode.omIdle);
            }
        }

        // City.pas:1705
        public override void NewTurn(sbyte Player)
        {
            base.NewTurn(Player);
            if (!FMode.Contains(TCityModeElement.cmWarmode))
            {
                FProduction.NewTurn(Player);
            }

            if (Player == FPlayer)
            {
                var call__261 = AoWHSMap.FPlayers.GetPlayers(Player);
                if (call__261.FPlayerType != TPlayerType.ptHuman)
                {
                    UpdateAIMigration();
                }
            }
        }

        // City.pas:1335
        public override void ArmyPlaced(TArmyHS A1)
        {
            base.ArmyPlaced(A1);
            if (FEnchantments.Contains(TCityEnchantmentID.ceiFountainOfLife))
            {
                ExecuteFountainOfLife();
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
            {
                FOverlay.PlayFadeShellAnimation();
            }
        }

        // City.pas:1342
        public override void ArmyRemoved(TArmyHS Army)
        {
            base.ArmyRemoved(Army);
            UpdatePlayer();
        }

        // City.pas:886
        public override void BuildingDone()
        {
            FUpgradeLevel = 1;
            FWallType = 0;
            var call__262 = AoWHSMap.FPlayers.GetPlayers(FBuildingPlayer);
            SetRace(call__262.FPlayerRace);
            base.BuildingDone();
        }

        // City.pas:1604
        public override bool GenerateRazeDefenders(sbyte Player, TUnitList Units)
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
            Result = base.ExecuteRaze(Player);
            if (Result)
            {
                var call__263 = AoWHSMap.FRaces.GetRace(FRace);
                var call__264 = call__263.GetPlayerRelationValue(Player);
                var call__265 = AoWHSMap.FRaces.GetRace(FRace);
                call__265.SetPlayerRelationValue(Player, call__264 + -30);
            }

            return Result;
        }

        // City.pas:2611
        public override AnsiString GetCurrentActivityText()
        {
            AnsiString Result = default;
            var call__266 = FProduction.FQueue.Count;
            if (call__266 == 0)
            {
                var call__267 = ProducingMerchandiseRStr;
                var call__268 = TranslateRStr(call__267);
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
        public override bool ValidateMap(TStrings A1)
        {
            bool Result = default;
            Result = base.ValidateMap(A1);
            var call__271 = TranslateRStr(NonameRStr);
            if (CompareText(FName, call__271) == 0 || FName == (AnsiString)"")
            {
                var call__272 = City.CityWarningRStr;
                var call__273 = TranslateRStr(call__272);
                var call__274 = TranslateRStr(NoNameSpecifiedRStr);
                A1.Add(call__273 + (AnsiString)": " + call__274);
                Result = false;
            }

            return Result;
        }

        // City.pas:1162
        public override void UpdateAITarget(in TAITargetInfo Info, ref TAITargetData Data)
        {
            int Amount = default;
            int Priority = default;
            if (!FMode.Contains(TCityModeElement.cmRazed))
            {
                switch (Info.Mode)
                {
                    case TAITargetMode.tmAttack:
                    case TAITargetMode.tmRaid:
                        if (Info.Player != FPlayer)
                        {
                            Priority = Data.Priority;
                            if (FPlayer > 0)
                            {
                                var call__275 = ValidAttackRelation(Info.Player, Info.IndependentRelation, FPlayer, FRace);
                                if (call__275)
                                {
                                    Priority = 100 + (GetSize() - 1) * 10;
                                }
                            }
                            else
                            {
                                if (FPlayer == 0)
                                {
                                    var call__276 = AoWHSMap.FRaces.GetRace(FRace);
                                    var call__277 = call__276.GetPlayerRelation(Info.Player);
                                    if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(call__277))
                                    {
                                        var call__278 = JoinAmount(Info.Player);
                                        Amount = call__278;
                                        if (Amount != -1)
                                        {
                                            Data.Cost = Amount;
                                            Priority = 100 + (GetSize() - 1) * 10;
                                            Data.TargetType = TAITargetType.attNegotiate;
                                        }
                                        else
                                        {
                                            Priority = 100 + (GetSize() - 1) * 10;
                                        }
                                    }
                                    else
                                    {
                                        Priority = 100 + (GetSize() - 1) * 10;
                                    }
                                }
                                else
                                {
                                    if (FMode.Contains(TCityModeElement.cmWarmode) && Priority != 0)
                                    {
                                        Priority = 100 + (GetSize() - 1) * 10;
                                    }
                                }
                            }

                            if (Priority > Data.Priority)
                            {
                                Data.Priority = Priority;
                            }
                        }

                        break;
                    case TAITargetMode.tmProduce:
                        if (Info.Player == FPlayer)
                        {
                            Data.Priority = Data.Priority | 2;
                        }

                        break;
                    case TAITargetMode.tmFortify:
                        bool bool__279 = Info.Player == FPlayer;
                        if (bool__279)
                        {
                            var call__280 = AoWHSMap.FRaces.GetRace(FRace);
                            var call__281 = call__280.GetPlayerRelation(Info.Player);
                            bool__279 = TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(call__281);
                        }

                        bool bool__282 = bool__279;
                        if (bool__282)
                        {
                            var call__283 = GetActiveAction();
                            bool__282 = call__283 != TCityAction.caMigrate;
                        }

                        if (bool__282)
                        {
                            var call__284 = GetSize();
                            Data.Priority = 500 + call__284 * 100 + FWallType * 45;
                            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
                            {
                                Data.Priority += 150;
                            }

                            if (GetLhx() != 0)
                            {
                                Data.Priority += 50;
                            }
                        }

                        break;
                    case TAITargetMode.tmDefend:
                        if (Info.Player == FPlayer)
                        {
                            var call__285 = GetSize();
                            Data.Priority = 50 + call__285 * 20;
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
            bool bool__286 = AoWHSMap.FSeatedPlayer == FPlayer;
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
                    var call__290 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__290.FProductionControls.Remove(FProduction);
                }

                base.SetPlayer(Player);
                if (GetActiveAction() == TCityAction.caMigrate)
                {
                    FProduction.ExecuteCancelProduction();
                    var call__291 = FProduction.FQueue.GetItem(0);
                    call__291.Free();
                }

                ValidateAICityAction();
                UpdateAIMigration();
                if (FPlayer != -1)
                {
                    var call__292 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    call__292.FProductionControls.Add(FProduction);
                }
            }
        }

        // City.pas:2071
        public override void UpdateFlagID()
        {
            bool bool__293 = !FStatus.Contains(TMapObjectStatusElement.moEditMode);
            if (bool__293)
            {
                bool bool__294 = !FMode.Contains(TCityModeElement.cmRazed);
                if (!bool__294)
                {
                    var call__295 = GetBuilding();
                    bool__294 = call__295;
                }

                bool__293 = bool__294;
            }

            if (bool__293 && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                if (FPlayer > 0)
                {
                    var call__296 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    FFlagID = (byte)(call__296.FPlayerRace + (FUpgradeLevel - 1) * 64);
                }
                else
                {
                    FFlagID = (byte)(32 + (FUpgradeLevel - 1) * 64);
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
            bool bool__298 = call__297 != 0;
            if (!bool__298)
            {
                bool bool__299 = FPlayer > 0;
                if (bool__299)
                {
                    var call__300 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    bool__299 = (((byte)call__300.FMagicControl.FEnemyEnchantments.Image) & 8) != 0;
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
        public override void FinalizeCombat(TCombat Combat)
        {
            if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
            {
                var combatData = Combat.FCombatData;
                var call__302 = combatData.FPlayers.GetPlayers(combatData.FAttackerPlayer);
                SetPlayer(call__302.FPlayer);

                UpdatePlayer();
            }
        }

        // City.pas:2214
        public override AnsiString GetCombatEventText(TCombat Combat)
        {
            AnsiString Result = default;
            if (!GetRazed())
            {
                if (Combat.FCombatResult == TCombatResult.crVictory || Combat.FCombatResult == TCombatResult.crNoDefenders)
                {
                    var call__303 = CityBattleWonRStr;
                    var call__304 = TranslateRStr(call__303);
                    var call__305 = GetName();
                    var call__306 = Rtl.Rtl.Format(call__304, [call__305]);
                    Result = call__306;
                }
                else
                {
                    if (Combat.FCombatResult == TCombatResult.crDefeat)
                    {
                        var call__307 = CityBattleLostRStr;
                        var call__308 = TranslateRStr(call__307);
                        var call__309 = GetName();
                        var call__310 = Rtl.Rtl.Format(call__308, [call__309]);
                        Result = call__310;
                    }
                    else
                    {
                        if (Combat.FCombatResult != TCombatResult.crCannotBreachWall)
                        {
                            var call__311 = CityBattleDrawnRStr;
                            var call__312 = TranslateRStr(call__311);
                            var call__313 = GetName();
                            var call__314 = Rtl.Rtl.Format(call__312, [call__313]);
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
            TCityProductionEventLog Log = default;
            var call__315 = GetRazed();
            bool bool__316 = !call__315;
            if (bool__316)
            {
                var call__317 = GetBuilding();
                bool__316 = !call__317;
            }

            if (bool__316 && OldPlayer == 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting) && FStatus.Contains(TMapObjectStatusElement.moActivated) && !((TCityResource)FResource).FClaimed)
            {
                Log = new TCityProductionEventLog(null);
                Log.FPlayer = NewPlayer;
                var call__318 = EditName();
                var call__319 = TranslateRStr(StructureCapturedRStr);
                var call__320 = Rtl.Rtl.Format(call__319, [call__318]);
                Log.FText = call__320;
                var call__321 = GetXhx();
                Log.FXhx = call__321;
                var call__322 = GetYhx();
                Log.FYhx = call__322;
                var call__323 = GetLhx();
                Log.FLhx = call__323;
                var call__324 = GetYhx();
                var call__325 = GetLhx();
                var call__326 = GetXhx();
                DistributeLocationEventLog(Log, FPlayer, call__326, call__324, call__325, false);
                Log.Release();
            }
        }

        // City.pas:2271
        public override TMoveAction GetMoveAction(sbyte Player, TMoveAction Action)
        {
            TMoveAction Result = default;
            Result = Action;
            if (FPlayer == 0)
            {
                bool bool__327 = TSet<TMoveAction>.Of(TMoveAction.maNormal, TMoveAction.maAttack).Contains(Action);
                if (bool__327)
                {
                    var call__328 = GetRazed();
                    bool__327 = !call__328;
                }

                if (bool__327 && !FMode.Contains(TCityModeElement.cmWarmode))
                {
                    if (JoinAmount(Player) != -1)
                    {
                        Result = TMoveAction.maNegotiate;
                    }
                    else
                    {
                        Result = TMoveAction.maAttack;
                    }
                }
            }
            else
            {
                if (FPlayer != Player && FPlayer != -1)
                {
                    Result = TMoveAction.maAttack;
                }
            }

            return Result;
        }

        // City.pas:1285
        public override bool UpdatePlayer()
        {
            bool Result = default;
            bool MultiplePlayers = default;
            TMapLevel MapLevel = default;
            int Count = default;
            THNWalkData Walk = default;
            TArmyHS Army = default;
            sbyte Player = default;
            bool bool__329 = FMode.Contains(TCityModeElement.cmRazed);
            if (bool__329)
            {
                var call__330 = GetBuilding();
                bool__329 = !call__330;
            }

            if (!bool__329)
            {
                MultiplePlayers = false;
                Player = -1;
                Count = GetTerrainCount();
                MapLevel = (TMapLevel)Owner.Owner;
                InitHNWalk(ref Walk, FXhx, FYhx, 0);
                while (Walk.HN < Count && !MultiplePlayers)
                {
                    var call__331 = FResource.TerrainList.GetTerrain(Walk.HN);
                    if (call__331 >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                    {
                        var call__332 = MapLevel.GetField(Walk.X, Walk.Y);
                        var call__333 = call__332.FindOwnedChild(131607);
                        Army = (TArmyHS)call__333;
                        if (Army != null)
                        {
                            if (Player == -1)
                            {
                                Player = Army.FArmy.FPlayer;
                            }
                            else
                            {
                                MultiplePlayers = Army.FArmy.FPlayer != Player;
                            }
                        }
                    }

                    WalkToNextHN(ref Walk);
                }

                if (MultiplePlayers)
                {
                    if (!FMode.Contains(TCityModeElement.cmWarmode))
                    {
                        FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode);
                        SetPlayer(-1);
                        UpdateFlagID();
                        Update();
                    }
                }
                else
                {
                    if (FMode.Contains(TCityModeElement.cmWarmode))
                    {
                        FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode);
                        if (Player != -1)
                        {
                            SetPlayer(Player);
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
                            SetPlayer(Player);
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
                bool__335 = call__336 != TCityAction.caMigrate;
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
                SetIncome((byte)(call__339 * 2 + call__340 * 5));
                if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(GetActiveAction()))
                {
                    SetIncome((byte)(FIncome + Round((double)FIncome * 0.25)));
                }

                if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
                {
                    SetIncome((byte)(FIncome * 2));
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
                    if (AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
                    {
                        Count = ((TCityResource)FResource).FCropLocations.Count;
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
            Result = FRace;
            return Result;
        }

        // City.pas:1211
        public override void CropTrampled(sbyte Player)
        {
            int Race = default;
            if (Player != FPlayer && Player != 0)
            {
                if (FPlayer > 0)
                {
                    var call__341 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    Race = call__341.FPlayerRace;
                }
                else
                {
                    Race = FRace;
                }

                var call__342 = AoWHSMap.FRaces.GetRace(Race);
                var call__343 = call__342.GetPlayerRelationValue(Player);
                var call__344 = AoWHSMap.FRaces.GetRace(Race);
                call__344.SetPlayerRelationValue(Player, call__343 + -5);
            }
        }

        // City.pas:2097
        public virtual void SetRace(sbyte Race)
        {
            if (Race != FRace)
            {
                FRace = Race;
                if (FRace >= 0)
                {
                    SetDefaultProductions();
                }

                UpdateCrops();
                UpdateImages();
            }
        }

        // City.pas:857
        public TCityAction GetActiveAction()
        {
            TCityAction Result = default;
            if (FProduction.FStatus == TProductionControlStatus.pcsProducing)
            {
                var call__345 = FProduction.FQueue.GetItem(0);
                var call__346 = City.ProductionIDToCityAction(call__345.FProductionID);
                Result = call__346;
            }
            else
            {
                Result = TCityAction.caNone;
            }

            return Result;
        }

        // City.pas:894
        public void UpdateActiveEnchantments()
        {
            int I = default;
            FEnchantments = TSet<TCityEnchantmentID>.Of();
            {
                I = FEnchantmentList.Count - 1;
                byte end__347 = 0;
                for (; I >= end__347; I -= 1)
                {
                    var call__348 = FEnchantmentList.GetEnchantments(I);
                    FEnchantments = FEnchantments + TSet<TCityEnchantmentID>.Of(((TCityEnchantment)call__348).FEnchantmentID);
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
                    call__351.Activate(call__350.FPlayerIndex);
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
                    call__354.Deactivate((byte)call__353.FPlayerIndex);
                }
            }
        }

        // City.pas:917
        public void ResetEnchantments()
        {
            UnregisterEnchantments();
            FEnchantmentList.Clear();
            FEnchantments = TSet<TCityEnchantmentID>.Of();
        }

        // City.pas:924
        public int AICityEnchantmentUpkeepPriority(TCityEnchantmentID ID)
        {
            int Result = default;
            switch (ID)
            {
                case TCityEnchantmentID.ceiFountainOfLife:
                    Result = GetSize() * 25 + 600;
                    break;
                case TCityEnchantmentID.ceiGoldRush:
                    Result = GetSize() * 25 + 600;
                    break;
                case TCityEnchantmentID.ceiAntiMagicShell:
                    Result = GetSize() * 25 + 700;
                    break;
                case TCityEnchantmentID.ceiWarmonger:
                    Result = GetSize() * 25 + 600;
                    break;
                default:
                    Result = 0;
                    break;
            }

            return Result;
        }

        // City.pas:935
        public void ActivateEnchantment(TCityEnchantment Enchantment)
        {
            Enchantment.Owner = FEnchantmentList;
            Enchantment.Activate(Enchantment.FPlayerIndex);
            FEnchantments = FEnchantments + TSet<TCityEnchantmentID>.Of(Enchantment.FEnchantmentID);
            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiFountainOfLife)
            {
                ExecuteFountainOfLife();
            }

            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiAntiMagicShell)
            {
                FOverlay.PlaySetupShellAnimation();
            }
        }

        // City.pas:945
        public void DeactivateEnchantment(TCityEnchantment Enchantment)
        {
            Enchantment.Owner = null;
            Enchantment.Deactivate((byte)Enchantment.FPlayerIndex);
            UpdateActiveEnchantments();
            if (Enchantment.FEnchantmentID == TCityEnchantmentID.ceiGoldRush)
            {
                UpdateIncome();
            }

            Enchantment.Release();
            Update();
        }

        // City.pas:955
        public void ListEnhancementsInfo(TStrings List)
        {
            var call__355 = TranslateRStr(UpgradeLevelXRStr);
            var call__356 = Rtl.Rtl.Format(call__355, [IntToStr(FUpgradeLevel)]);
            List.Add(call__356);
            if (FWallType == 1)
            {
                var call__357 = City.CityWoodenWallRStr;
                var call__358 = TranslateRStr(call__357);
                List.Add(call__358);
            }

            if (FWallType == 2)
            {
                var call__359 = City.CityStoneWallRStr;
                var call__360 = TranslateRStr(call__359);
                List.Add(call__360);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiFountainOfLife))
            {
                var call__361 = TranslateRStr(FountainOfLifeRStr);
                var call__362 = TranslateRStr(EnchantmentRStr);
                List.Add(call__361 + (AnsiString)": " + call__362);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
            {
                var call__363 = TranslateRStr(GoldRushRStr);
                var call__364 = TranslateRStr(EnchantmentRStr);
                List.Add(call__363 + (AnsiString)": " + call__364);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiAntiMagicShell))
            {
                var call__365 = TranslateRStr(AntiMagicShellRStr);
                var call__366 = TranslateRStr(EnchantmentRStr);
                List.Add(call__365 + (AnsiString)": " + call__366);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiWarmonger))
            {
                var call__367 = TranslateRStr(WarmongerRStr);
                var call__368 = TranslateRStr(EnchantmentRStr);
                List.Add(call__367 + (AnsiString)": " + call__368);
            }
        }

        // City.pas:970
        public void StartTownQuake()
        {
            FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmTownQuake);
        }

        // City.pas:973
        public void StopTownQuake()
        {
            FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmTownQuake);
        }

        // City.pas:976
        public void ExecuteAnarchy()
        {
            FMode = FMode + TSet<TCityModeElement>.Of(TCityModeElement.cmAnarchy);
            CheckRebellion();
            FMode = FMode - TSet<TCityModeElement>.Of(TCityModeElement.cmAnarchy);
        }

        // City.pas:983
        public bool ExecuteFountainOfLife()
        {
            bool Result = default;
            TArmyHSList Armies = default;
            int I = default;
            TImageSequenceAnimation Animation = default;
            bool PlaySound = default;
            Armies = new TArmyHSList(null);
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
                        var call__378 = AoWHSMap.VisibleForPlayer(call__377, call__375, call__373, AoWHSMap.FSeatedPlayer);
                        if (call__378)
                        {
                            Animation = new TImageSequenceAnimation(null);
                            var call__379 = AoWHSSet.FAbilityControl.GetAbility(47);
                            var call__380 = call__379.FImageSequences.ImageSequences[0];
                            Animation.SetImageSequence(call__380);
                            Animation.FAnimationLevel = 27;
                            var call__381 = Armies.GetArmyHS(I);
                            var call__382 = call__381.GetLhx();
                            var call__383 = Armies.GetArmyHS(I);
                            var call__384 = call__383.GetYhx();
                            var call__385 = Armies.GetArmyHS(I);
                            var call__386 = call__385.GetXhx();
                            AoWHSMap.PlaceHX(Animation, call__386, call__384, call__382);
                            bool bool__387 = PlaySound;
                            if (!bool__387)
                            {
                                var call__388 = Armies.GetArmyHS(I);
                                bool__387 = call__388.FArmy.FPlayer == AoWHSMap.FSeatedPlayer;
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
                                var call__396 = AoWHSMap.Watching(call__395, call__393, call__391, 1);
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
                var call__397 = AoWHSSet.FAbilityControl.GetAbility(47);
                call__397.FSFXLibrary.Play(0);
            }

            Armies.Free();
            return Result;
        }

        // City.pas:1015
        public void UpdateAIMigration()
        {
            TMigrationList Selection = default;
            int I = default;
            int Value = default;
            int BestIndex = default;
            int BestValue = default;
            TProductionItem Item = default;
            if (AoWHSMap.FDay != 0 && !AoWHSMap.Status.Contains(THSMapStatusElement.msEditting))
            {
                if (FPlayer > 0)
                {
                    var call__398 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                    bool bool__399 = call__398.FPlayerType != TPlayerType.ptHuman && (TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed) & FMode) == TSet<TCityModeElement>.Of();
                    if (bool__399)
                    {
                        var call__400 = GetActiveAction();
                        bool__399 = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(call__400);
                    }

                    bool bool__401 = bool__399;
                    if (bool__401)
                    {
                        var call__402 = AoWHSMap.FRaces.GetRace(FRace);
                        var call__403 = call__402.GetPlayerRelation(FPlayer);
                        bool__401 = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(call__403);
                    }

                    if (bool__401)
                    {
                        Selection = CreateMigrationSelection();
                        BestValue = 0;
                        BestIndex = -1;
                        {
                            I = 0;
                            int end__404 = Selection.Count - 1;
                            for (; I <= end__404; I += 1)
                            {
                                var call__405 = Selection.GetItems(I);
                                if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(call__405.FRelation))
                                {
                                    var call__406 = Selection.GetItems(I);
                                    var call__407 = AoWHSMap.FPlayers.RaceToPlayer(call__406.FRace);
                                    bool bool__408 = call__407 == -1;
                                    if (!bool__408)
                                    {
                                        var call__409 = Selection.GetItems(I);
                                        var call__410 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                                        bool__408 = call__409.FRace == call__410.FPlayerRace;
                                    }

                                    if (bool__408)
                                    {
                                        Value = 10;
                                    }
                                    else
                                    {
                                        Value = 5;
                                    }
                                }
                                else
                                {
                                    Value = 0;
                                }

                                var call__411 = Selection.GetItems(I);
                                Value += call__411.FTime;
                                bool bool__412 = Value > BestValue;
                                if (!bool__412)
                                {
                                    bool bool__413 = Value == BestValue;
                                    if (bool__413)
                                    {
                                        var call__414 = AoWHSMap.Random(2);
                                        bool__413 = call__414 == 1;
                                    }

                                    bool__412 = bool__413;
                                }

                                if (bool__412)
                                {
                                    BestValue = Value;
                                    BestIndex = I;
                                }
                            }
                        }

                        if (BestIndex != -1)
                        {
                            FProduction.ExecuteDeleteProductions();
                            Item = new TProductionItem(null);
                            var call__415 = City.CityActionToProductionID(TCityAction.caMigrate);
                            Item.FProductionID = call__415;
                            var call__416 = Selection.GetItems(BestIndex);
                            Item.FParameter = call__416.FRace;
                            FProduction.ExecuteAddProduction(Item);
                            Item.Free();
                        }

                        Selection.Free();
                    }
                }
                else
                {
                    bool bool__417 = FPlayer == 0;
                    if (bool__417)
                    {
                        var call__418 = GetActiveAction();
                        bool__417 = TSet<TCityAction>.Of(TCityAction.caMigrate, TCityAction.caLoot).Contains(call__418);
                    }

                    if (bool__417)
                    {
                        FProduction.ExecuteDeleteProductions();
                    }
                }
            }
        }

        // City.pas:1081
        public void ValidateAICityAction()
        {
            bool Cancel = default;
            bool bool__419 = FPlayer > 0;
            if (bool__419)
            {
                var call__420 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                bool__419 = call__420.FPlayerType != TPlayerType.ptHuman;
            }

            bool bool__421 = bool__419 && (TSet<TCityModeElement>.Of(TCityModeElement.cmWarmode, TCityModeElement.cmRazed) & FMode) == TSet<TCityModeElement>.Of();
            if (bool__421)
            {
                var call__422 = GetActiveAction();
                bool__421 = !TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(call__422);
            }

            if (bool__421)
            {
                var call__423 = AoWHSMap.FRaces.GetRace(FRace);
                var call__424 = call__423.GetPlayerRelation(FPlayer);
                bool bool__425 = TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary, TStatus.sNeutral).Contains(call__424);
                if (bool__425)
                {
                    var call__426 = GetActiveAction();
                    bool bool__427 = call__426 == TCityAction.caMigrate;
                    if (bool__427)
                    {
                        var call__428 = FProduction.FQueue.GetItem(0);
                        var call__429 = AoWHSMap.FRaces.GetRace(call__428.FParameter);
                        var call__430 = call__429.GetPlayerRelation(FPlayer);
                        bool__427 = TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(call__430);
                    }

                    bool__425 = !bool__427;
                }

                Cancel = bool__425;
                if (Cancel)
                {
                    FProduction.ExecuteDeleteProductions();
                }
            }
        }

        // City.pas:1222
        public bool MoveArmiesOutOfCity(TSet<TMoveOutOfCitySelectionElement> Selection)
        {
            bool Result = default;
            TMapLevel MapLevel = default;
            TArmyHS Army = default;
            int Count = default;
            THNWalkData Walk = default;
            Result = true;
            Count = GetTerrainCount();
            MapLevel = (TMapLevel)Owner.Owner;
            // { This eliminated copy preserves the original saved registers. }
            ;
            InitHNWalk(ref Walk, FXhx, FYhx, 0);
            while (Walk.HN < Count && Result)
            {
                var call__431 = FResource.TerrainList.GetTerrain(Walk.HN);
                if (call__431 >= 0 && Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                {
                    var call__432 = MapLevel.GetField(Walk.X, Walk.Y);
                    var call__433 = call__432.FindOwnedChild(131607);
                    Army = (TArmyHS)call__433;
                    if (Army != null && (!Selection.Contains(TMoveOutOfCitySelectionElement.msSkipOwnPlayer) || Army.FArmy.FPlayer != FPlayer))
                    {
                        var call__434 = GetLhx();
                        var call__435 = Army.PlaceEx((sbyte)Walk.X, (sbyte)Walk.Y, call__434, TSet<TArmyPlaceModeElement>.Of(TArmyPlaceModeElement.apSafe, TArmyPlaceModeElement.apScanArea));
                        Result = call__435;
                    }
                }

                WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1256
        public int GetSize()
        {
            int Result = default;
            Result = ((TCityResource)FResource).FSize;
            return Result;
        }

        // City.pas:1259
        public int GetObtainUnitsValue()
        {
            int Result = default;
            int Count = default;
            TMapLevel MapLevel = default;
            THNWalkData Walk = default;
            TArmyHS Army = default;
            ref var walk = ref Walk;
            Result = 0;
            Count = GetTerrainCount();
            MapLevel = (TMapLevel)Owner.Owner;
            var call__437 = GetYhx();
            var call__438 = GetXhx();
            InitHNWalk(ref Walk, call__438, call__437, 0);
            while (walk.HN < Count)
            {
                var call__439 = FResource.TerrainList.GetTerrain(walk.HN);
                if (call__439 >= 0 && walk.X >= 0 && walk.Y >= 0 && walk.X < MapLevel.Width && walk.Y < MapLevel.Height)
                {
                    var call__440 = MapLevel.GetField(walk.X, walk.Y);
                    var call__441 = call__440.FindOwnedChild(131607);
                    Army = (TArmyHS)call__441;
                    if (Army != null)
                    {
                        var call__442 = Army.FArmy.ObtainValue(255);
                        Result += call__442;
                    }
                }

                WalkToNextHN(ref Walk);
            }

            return Result;
        }

        // City.pas:1348
        public int UpgradeCost()
        {
            int Result = default;
            var call__443 = GetSize();
            Result = City.UpgradePrices[call__443 - 1];
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
        public bool CanUpgrade(bool Queue, ref AnsiString Reason)
        {
            bool Result = default;
            int Level = default;
            int I = default;
            Level = FUpgradeLevel;
            if (Queue)
            {
                {
                    I = 0;
                    int end__444 = FProduction.FQueue.Count - 1;
                    for (; I <= end__444; I += 1)
                    {
                        var call__445 = FProduction.FQueue.GetItem(I);
                        var call__446 = City.CityActionToProductionID(TCityAction.caUpgrade);
                        if (call__445.FProductionID == call__446)
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
                var call__447 = MaxUpgradeLevelReachedRStr;
                var call__448 = TranslateRStr(call__447);
                Reason = call__448;
            }

            return Result;
        }

        // City.pas:1371
        public bool Upgrade(bool Queue)
        {
            bool Result = default;
            TProductionItem Item = default;
            AnsiString Reason = default;
            var call__449 = CanUpgrade(Queue, ref Reason);
            bool bool__450 = call__449;
            if (bool__450)
            {
                var call__451 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__452 = call__451.GetBusy();
                bool__450 = !call__452;
            }

            if (bool__450)
            {
                Item = new TProductionItem(null);
                var call__453 = City.CityActionToProductionID(TCityAction.caUpgrade);
                Item.FProductionID = call__453;
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
                    var call__454 = GetSize();
                    Result = City.WoodWallPrices[call__454 - 1];
                    break;
                case 1:
                    var call__455 = GetSize();
                    Result = City.StoneWallPrices[call__455 - 1];
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
        public bool CanFortify(bool Queue, ref AnsiString Reason)
        {
            bool Result = default;
            int Level = default;
            int I = default;
            Level = FWallType;
            if (Queue)
            {
                {
                    I = 0;
                    int end__456 = FProduction.FQueue.Count - 1;
                    for (; I <= end__456; I += 1)
                    {
                        var call__457 = FProduction.FQueue.GetItem(I);
                        var call__458 = City.CityActionToProductionID(TCityAction.caFortify);
                        if (call__457.FProductionID == call__458)
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
                var call__459 = MaxFortifyLevelReachedRStr;
                var call__460 = TranslateRStr(call__459);
                Reason = call__460;
            }

            return Result;
        }

        // City.pas:1415
        public bool Fortify(bool Queue)
        {
            bool Result = default;
            TProductionItem Item = default;
            AnsiString Reason = default;
            var call__461 = CanFortify(Queue, ref Reason);
            bool bool__462 = call__461;
            if (bool__462)
            {
                var call__463 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__464 = call__463.GetBusy();
                bool__462 = !call__464;
            }

            if (bool__462)
            {
                Item = new TProductionItem(null);
                var call__465 = City.CityActionToProductionID(TCityAction.caFortify);
                Item.FProductionID = call__465;
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
            var call__466 = FProduction.UnitProductionGold(Index);
            Result = call__466;
            return Result;
        }

        // City.pas:1435
        public int ProductionTime(int Index)
        {
            int Result = default;
            var call__467 = FProduction.UnitProductionTime(Index);
            Result = call__467;
            return Result;
        }

        // City.pas:1440
        public bool CanProduce(int Index, bool Queue, ref AnsiString Reason)
        {
            bool Result = default;
            var call__468 = FProduction.CanProduceUnit(Index, Queue, ref Reason);
            Result = call__468;
            return Result;
        }

        // City.pas:1445
        public bool Produce(int Index, bool Queue)
        {
            bool Result = default;
            var call__469 = FProduction.ProduceUnit(Index, Queue);
            Result = call__469;
            return Result;
        }

        // City.pas:1450
        public int ResearchCost(int Index)
        {
            int Result = default;
            var call__470 = FProduction.UnitResearchGold(Index);
            Result = call__470;
            return Result;
        }

        // City.pas:1455
        public int ResearchTime(int Index)
        {
            int Result = default;
            var call__471 = FProduction.UnitResearchTime(Index);
            Result = call__471;
            return Result;
        }

        // City.pas:1460
        public bool CanResearch(int Index, bool Queue, ref AnsiString Reason)
        {
            bool Result = default;
            var call__472 = FProduction.CanResearchUnit(Index, Queue, ref Reason);
            Result = call__472;
            return Result;
        }

        // City.pas:1465
        public bool Research(int Index, bool Queue)
        {
            bool Result = default;
            var call__473 = FProduction.ResearchUnit(Index, Queue);
            Result = call__473;
            return Result;
        }

        // City.pas:1470
        public bool CanTrade(bool Queue, ref AnsiString Reason)
        {
            bool Result = default;
            Result = true;
            return Result;
        }

        // City.pas:1475
        public bool Trade(bool Queue)
        {
            bool Result = default;
            TProductionItem Item = default;
            AnsiString Reason = default;
            var call__474 = CanTrade(Queue, ref Reason);
            bool bool__475 = call__474;
            if (bool__475)
            {
                var call__476 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__477 = call__476.GetBusy();
                bool__475 = !call__477;
            }

            if (bool__475)
            {
                Item = new TProductionItem(null);
                var call__478 = City.CityActionToProductionID(TCityAction.caTrade);
                Item.FProductionID = call__478;
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
        public bool CanLoot(ref AnsiString Reason)
        {
            bool Result = default;
            Result = !FMode.Contains(TCityModeElement.cmRazed);
            return Result;
        }

        // City.pas:1499
        public bool Loot()
        {
            bool Result = default;
            TProductionItem Item = default;
            AnsiString Reason = default;
            var call__479 = CanLoot(ref Reason);
            bool bool__480 = call__479;
            if (bool__480)
            {
                var call__481 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__482 = call__481.GetBusy();
                bool__480 = !call__482;
            }

            if (bool__480)
            {
                Item = new TProductionItem(null);
                var call__483 = City.CityActionToProductionID(TCityAction.caLoot);
                Item.FProductionID = call__483;
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
            AnsiString Reason = default;
            THNWalkData Walk = default;
            TMapLevel MapLevel = default;
            int Radius = default;
            TCity City = default;
            if (CanMigrate(Race, ref Reason))
            {
                if (Race != FRace)
                {
                    Radius = -1;
                    MapLevel = (TMapLevel)Owner.Owner;
                    InitHNWalk(ref Walk, FXhx, FYhx, 1);
                    while (Walk.Radius <= 20 && Radius == -1)
                    {
                        if (Walk.X >= 0 && Walk.Y >= 0 && Walk.X < MapLevel.Width && Walk.Y < MapLevel.Height)
                        {
                            var call__484 = MapLevel.GetField(Walk.X, Walk.Y);
                            var call__485 = call__484.FindNoneTransparentHS(typeof(TCity));
                            City = (TCity)call__485;
                            if (City != null && City.FRace == Race)
                            {
                                Radius = Walk.Radius;
                            }
                        }

                        WalkToNextHN(ref Walk);
                    }

                    if (Radius != -1)
                    {
                        Result = Radius / 10 + 3;
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
        public bool CanMigrate(sbyte Race, ref AnsiString Reason)
        {
            bool Result = default;
            int I = default;
            sbyte Player = default;
            if (FRace != Race)
            {
                var call__486 = City.RaceInCity(Race, FPlayer);
                Result = call__486;
                var call__487 = AoWHSMap.FPlayers.Count;
                I = call__487;
                while (I != 1 && !Result)
                {
                    I -= 1;
                    Player = (sbyte)I;
                    var call__488 = AoWHSMap.FPlayers.GetDiplomaticRelation((sbyte)(Player << 0), FPlayer);
                    bool bool__489 = call__488 == TDiplomaticRelation.drAlliance;
                    if (bool__489)
                    {
                        var call__490 = City.RaceInCity(Race, Player);
                        bool__489 = call__490;
                    }

                    Result = bool__489;
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
            TProductionItem Item = default;
            AnsiString Reason = default;
            var call__491 = CanMigrate(Race, ref Reason);
            bool bool__492 = call__491;
            if (bool__492)
            {
                var call__493 = AoWHSMap.FPlayers.GetPlayers(FPlayer);
                var call__494 = call__493.GetBusy();
                bool__492 = !call__494;
            }

            if (bool__492)
            {
                Item = new TProductionItem(null);
                var call__495 = City.CityActionToProductionID(TCityAction.caMigrate);
                Item.FProductionID = call__495;
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
        public TMigrationItem CreateMigrationItem(sbyte Race)
        {
            TMigrationItem Result = default;
            AnsiString Reason = default;
            if (CanMigrate(Race, ref Reason))
            {
                Result = new TMigrationItem(null);
                Result.FRace = Race;
                var call__496 = MigrationTime(Race);
                Result.FTime = call__496;
                var call__497 = AoWHSMap.FRaces.GetRace(Race);
                var call__498 = call__497.GetPlayerRelation(FPlayer);
                Result.FRelation = call__498;
            }
            else
            {
                Result = null;
            }

            return Result;
        }

        // City.pas:1589
        public TMigrationList CreateMigrationSelection()
        {
            TMigrationList Result = default;
            int I = default;
            TMigrationItem Item = default;
            Result = new TMigrationList(null);
            {
                I = 0;
                int end__499 = AoWHSMap.FRaces.Count - 1;
                for (; I <= end__499; I += 1)
                {
                    var call__500 = AoWHSMap.FRaces.GetRace(I);
                    if (call__500.FEnabled)
                    {
                        Item = CreateMigrationItem((sbyte)I);
                        if (Item != null && TSet<TStatus>.Of(TStatus.sNeutral, TStatus.sPolite, TStatus.sFriendly).Contains(Item.FRelation))
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
            TCivilStatus Status = default;
            int Chance = default;
            int I = default;
            TCityRebellionEventLog Log = default;
            TUnitList Units = default;
            if (FPlayer != 0)
            {
                Status = GetCivilStatus();
                if (TSet<TCivilStatus>.Of(TCivilStatus.csUnruly, TCivilStatus.csUnrest).Contains(Status) && !GetRazed())
                {
                    if (Status == TCivilStatus.csUnruly)
                    {
                        Chance = 50;
                    }
                    else
                    {
                        Chance = 10;
                    }

                    var call__501 = AoWHSMap.Random(100);
                    if (Chance >= call__501)
                    {
                        MoveArmiesOutOfCity(TSet<TMoveOutOfCitySelectionElement>.Of());
                        Log = new TCityRebellionEventLog(null);
                        Log.FPlayer = FPlayer;
                        var call__502 = GetName();
                        Log.FCityName = call__502;
                        var call__503 = GetXhx();
                        Log.FXhx = call__503;
                        var call__504 = GetYhx();
                        Log.FYhx = call__504;
                        var call__505 = GetLhx();
                        Log.FLhx = call__505;
                        var call__506 = GetYhx();
                        var call__507 = GetLhx();
                        var call__508 = GetXhx();
                        DistributeLocationEventLog(Log, FPlayer, call__508, call__506, call__507, false);
                        Log.Release();
                        SetPlayer(0);
                        Units = new TUnitList(null);
                        GenerateRebelUnits(Units);
                        {
                            I = Units.Count - 1;
                            byte end__509 = 0;
                            for (; I >= end__509; I -= 1)
                            {
                                var call__510 = GetLhx();
                                var call__511 = GetYhx();
                                var call__512 = GetXhx();
                                var call__513 = Units.GetUnit(I);
                                call__513.Place(call__512, call__511, call__510, 0, TSet<TUnitPlaceModeElement>.Of());
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
            TArmyHSList Armies = default;
            var call__514 = GetRazed();
            if (!call__514 && FDefendersMode != TDefendersGenerateMode.dgNone && !FMode.Contains(TCityModeElement.cmWarmode) && FPlayer == 0)
            {
                Armies = new TArmyHSList(null);
                ListArmies(Armies);
                if (FDefendersMode == TDefendersGenerateMode.dgRandom)
                {
                    var call__515 = Random(3);
                    FDefendersMode = (TDefendersGenerateMode)(call__515 + 2);
                }

                Value = ((int)FDefendersMode - 2) * 25 + 25;
                Value += (GetSize() - 1) * Value / 2;
                {
                    I = 0;
                    int end__516 = Armies.Count - 1;
                    for (; I <= end__516; I += 1)
                    {
                        var call__517 = Armies.GetArmyHS(I);
                        var call__518 = call__517.FArmy.ObtainValue(255);
                        Value -= call__518;
                    }
                }

                if (Value > 0)
                {
                    var call__519 = AoWHSMap.Random(1048575);
                    RandSeed = call__519;
                    var call__520 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                    var call__521 = GetLhx();
                    var call__522 = GetYhx();
                    var call__523 = GetXhx();
                    PlaceRandomUnits(call__523, call__522, call__521, 0, call__520.FUnitIndexes, FUpgradeLevel, Value, TSet<TUnitType>.Of(TUnitType.utHumanoid, TUnitType.utCreature));
                }

                Armies.Free();
            }
        }

        // City.pas:1755
        public void ListIncomeInfo(TStrings Names, TStrings Values)
        {
            int Income = default;
            Income = GetSize() * 5;
            var call__524 = BaseIncomeRStr;
            var call__525 = TranslateRStr(call__524);
            Names.Add(call__525);
            Values.Add(IntToStr(Income));
            Income += GetCropCount() * 2;
            var call__526 = GetCropCount();
            var call__527 = XCropFieldsRStr;
            var call__528 = TranslateRStr(call__527);
            var call__529 = Rtl.Rtl.Format(call__528, [IntToStr(call__526)]);
            Names.Add(call__529);
            var call__530 = GetCropCount();
            Values.Add(IntToStr(call__530 * 2));
            if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(GetActiveAction()))
            {
                var call__531 = ProducingMerchandiseRStr;
                var call__532 = TranslateRStr(call__531);
                Names.Add(call__532);
                Values.Add(IntToStr(Round((double)Income * 0.25)));
                Income += Round((double)Income * 0.25);
            }

            if (FEnchantments.Contains(TCityEnchantmentID.ceiGoldRush))
            {
                var call__533 = GoldRushRStr;
                var call__534 = TranslateRStr(call__533);
                Names.Add(call__534);
                Values.Add(IntToStr(Income));
            }
        }

        // City.pas:1800
        public void SetDefaultProductions()
        {
            FProduction.FUnitProductions.Clear();
            if (FRace >= 0)
            {
                SetUpgradeLevel(FUpgradeLevel);
            }
        }

        // City.pas:1816
        public void ListCivilStatusInfo(TStrings List)
        {
            TCombatPredictor Predictor = default;
            TArmyHSList Armies = default;
            TUnitList Units = default;
            int I = default;
            int J = default;
            TAbstractUnit U = default;
            if (FPlayer != 0 && FRace >= 0)
            {
                var call__535 = AoWHSMap.FRaces.GetRace(FRace);
                var call__536 = call__535.GetPlayerRelation(FPlayer);
                var call__537 = TranslateRStr(TStatusRStr[(int)call__536]);
                var call__538 = RaceRelationXRStr;
                var call__539 = TranslateRStr(call__538);
                var call__540 = Rtl.Rtl.Format(call__539, [call__537]);
                List.Add(call__540);
                if (RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                {
                    var call__541 = TerrainPenaltyXRStr;
                    var call__542 = TranslateRStr(call__541);
                    var call__543 = AoWHSSet.TerrainTypes[TerrainType];
                    var call__544 = Rtl.Rtl.Format(call__542, [call__543]);
                    List.Add(call__544);
                }

                if (RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                {
                    var call__545 = TerrainBonusXRStr;
                    var call__546 = TranslateRStr(call__545);
                    var call__547 = AoWHSSet.TerrainTypes[TerrainType];
                    var call__548 = Rtl.Rtl.Format(call__546, [call__547]);
                    List.Add(call__548);
                }

                Predictor = new TCombatPredictor();
                Armies = new TArmyHSList(null);
                ListArmies(Armies);
                {
                    I = 0;
                    int end__549 = Armies.Count - 1;
                    for (; I <= end__549; I += 1)
                    {
                        {
                            J = 0;
                            int end__550 = Armies.GetArmyHS(I).FArmy.Count - 1;
                            for (; J <= end__550; J += 1)
                            {
                                var call__551 = Armies.GetArmyHS(I);
                                var call__552 = call__551.FArmy.GetUnit(J);
                                U = call__552;
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

                Units = new TUnitList(null);
                GenerateRebelUnits(Units);
                {
                    J = 0;
                    int end__553 = Units.Count - 1;
                    for (; J <= end__553; J += 1)
                    {
                        var call__554 = Units.GetUnit(J);
                        Predictor.AddOpponent(call__554, 0);
                    }
                }

                if (Predictor.Execute() == TCombatResult.crVictory)
                {
                    if (Predictor.GetSuperiority() / 4 >= 75)
                    {
                        var call__555 = StrongRStr;
                        var call__556 = TranslateRStr(call__555);
                        var call__557 = OccupationForcesXRStr;
                        var call__558 = TranslateRStr(call__557);
                        var call__559 = Rtl.Rtl.Format(call__558, [call__556]);
                        List.Add(call__559);
                    }
                    else
                    {
                        var call__560 = AverageRStr;
                        var call__561 = TranslateRStr(call__560);
                        var call__562 = OccupationForcesXRStr;
                        var call__563 = TranslateRStr(call__562);
                        var call__564 = Rtl.Rtl.Format(call__563, [call__561]);
                        List.Add(call__564);
                    }
                }
                else
                {
                    var call__565 = WeakRStr;
                    var call__566 = TranslateRStr(call__565);
                    var call__567 = OccupationForcesXRStr;
                    var call__568 = TranslateRStr(call__567);
                    var call__569 = Rtl.Rtl.Format(call__568, [call__566]);
                    List.Add(call__569);
                }

                Predictor.Free();
                Units.Free();
                Armies.Free();
            }
        }

        // City.pas:1861
        public TCivilStatus GetCivilStatus()
        {
            TCivilStatus Result = default;
            TStatus Relation = default;
            int Value = default;
            int ArmyValue = default;
            int Bonus = default;
            int I = default;
            int J = default;
            TCombatPredictor Predictor = default;
            TArmyHSList Armies = default;
            TUnitList Units = default;
            TAbstractUnit U = default;
            if (FPlayer != 0 && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                if (FMode.Contains(TCityModeElement.cmAnarchy))
                {
                    Relation = TStatus.sHostile;
                }
                else
                {
                    var call__570 = AoWHSMap.FRaces.GetRace(FRace);
                    var call__571 = call__570.GetPlayerRelation(FPlayer);
                    Relation = call__571;
                }

                switch (Relation)
                {
                    case TStatus.sHostile:
                        Value = -35;
                        break;
                    case TStatus.sWary:
                        Value = -25;
                        break;
                    case TStatus.sNeutral:
                        Value = 0;
                        break;
                    case TStatus.sPolite:
                        Value = 15;
                        break;
                    case TStatus.sFriendly:
                        Value = 25;
                        break;
                    default:
                        Value = 0;
                        break;
                }

                Value = Value + FWallType * 5 + FUpgradeLevel * 5;
                if (FRace >= 0)
                {
                    if (RaceTerrain[FRace].Penalty.Contains((byte)TerrainType))
                    {
                        Value += -10;
                    }

                    if (RaceTerrain[FRace].Bonus.Contains((byte)TerrainType))
                    {
                        Value += 10;
                    }

                    Predictor = new TCombatPredictor();
                    ArmyValue = 0;
                    Armies = new TArmyHSList(null);
                    ListArmies(Armies);
                    {
                        I = 0;
                        int end__572 = Armies.Count - 1;
                        for (; I <= end__572; I += 1)
                        {
                            {
                                J = 0;
                                int end__573 = Armies.GetArmyHS(I).FArmy.Count - 1;
                                for (; J <= end__573; J += 1)
                                {
                                    var call__574 = Armies.GetArmyHS(I);
                                    var call__575 = call__574.FArmy.GetUnit(J);
                                    U = call__575;
                                    if (U.FMoraleValue <= 40)
                                    {
                                        Predictor.AddOpponent(U, 0);
                                        // { Original anarchy counts disloyal defenders twice. }
                                        if (FMode.Contains(TCityModeElement.cmAnarchy))
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
                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                    {
                        Units = new TUnitList(null);
                        GenerateRebelUnits(Units);
                        {
                            I = 0;
                            int end__576 = Units.Count - 1;
                            for (; I <= end__576; I += 1)
                            {
                                var call__577 = Units.GetUnit(I);
                                Predictor.AddOpponent(call__577, 0);
                                if (FMode.Contains(TCityModeElement.cmAnarchy))
                                {
                                    var call__578 = Units.GetUnit(I);
                                    Predictor.AddOpponent(call__578, 0);
                                }
                            }
                        }

                        if (Predictor.Execute() == TCombatResult.crVictory)
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
                        if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(Relation))
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
                        Result = TCivilStatus.csUnruly;
                    }
                    else
                    {
                        if (Value < -10)
                        {
                            Result = TCivilStatus.csUnrest;
                        }
                        else
                        {
                            if (Value < 10)
                            {
                                Result = TCivilStatus.csStable;
                            }
                            else
                            {
                                if (Value < 30)
                                {
                                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                                    {
                                        Result = TCivilStatus.csOppressed;
                                    }
                                    else
                                    {
                                        Result = TCivilStatus.csContent;
                                    }
                                }
                                else
                                {
                                    if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(Relation))
                                    {
                                        Result = TCivilStatus.csSlavery;
                                    }
                                    else
                                    {
                                        Result = TCivilStatus.csCheerful;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Result = TCivilStatus.csStable;
                }
            }
            else
            {
                Result = TCivilStatus.csStable;
            }

            return Result;
        }

        // City.pas:2034
        public void SetUpgradeLevel(byte Level)
        {
            TRaceResource RaceResource = default;
            int I = default;
            int UnitLevel = default;
            bool Researched = default;
            if (Level > GetSize())
            {
                Level = (byte)GetSize();
            }

            if (Level < 1)
            {
                Level = 1;
            }

            FUpgradeLevel = Level;
            if (FRace >= 0)
            {
                var call__579 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                RaceResource = call__579;
                {
                    I = RaceResource.FUnitIndexes.Count - 1;
                    byte end__580 = 0;
                    for (; I >= end__580; I -= 1)
                    {
                        var call__581 = RaceResource.FUnitIndexes[I];
                        if (call__581 != -1)
                        {
                            var call__582 = RaceResource.FUnitIndexes[I];
                            var call__583 = FProduction.FUnitProductions.IndexOf(call__582);
                            if (call__583 == -1)
                            {
                                var call__584 = RaceResource.FUnitIndexes[I];
                                var call__585 = AoWHSSet.FUnitResources.GetUnitResource(call__584);
                                var call__586 = GetSize();
                                var call__587 = City.UnitLevelToCityLevel(call__585.FLevel, call__586);
                                UnitLevel = call__587;
                                if (UnitLevel != -1 && UnitLevel <= FUpgradeLevel)
                                {
                                    bool bool__588 = UnitLevel == 1;
                                    if (bool__588)
                                    {
                                        bool bool__589 = RaceResource.FSpecialUnitIndexes.Count >= 4;
                                        if (bool__589)
                                        {
                                            var call__590 = RaceResource.FUnitIndexes[I];
                                            var call__591 = RaceResource.FSpecialUnitIndexes[3];
                                            bool__589 = call__590 == call__591;
                                        }

                                        bool__588 = !bool__589;
                                    }

                                    Researched = bool__588;
                                    var call__592 = RaceResource.FUnitIndexes[I];
                                    FProduction.FUnitProductions.Add(call__592, Researched);
                                }
                            }
                        }
                    }
                }

                City.SortingCity = this;
                FProduction.FUnitProductions.FChildren.Sort(City.CompareCityProductions);
            }
        }

        // City.pas:2085
        public void SetName(AnsiString Name)
        {
            FName = Name;
            Update();
        }

        // City.pas:2091
        public void SetWallType(byte WallType)
        {
            FWallType = WallType;
            UpdateImages();
        }

        // City.pas:2206
        public void InitCombat(TCombat Combat, TArmyHS Army)
        {
            if (FPlayer == 0)
            {
                var call__593 = GetEnemiesIndependentRelation(Army.FArmy.FPlayer);
                var call__594 = Combat.FCombatData.FPlayers.GetPlayers(Combat.FCombatData.FDefenderPlayer);
                call__594.FIndependentRelation = (byte)call__593;
            }

            var call__595 = Army.GetLhx();
            var call__596 = Army.GetYhx();
            var call__597 = Army.GetXhx();
            Combat.InitDefaultArmies(Army.FArmy, call__597, call__596, call__595);
        }

        // City.pas:2234
        public int GetObtainValue()
        {
            int Result = default;
            var call__598 = GetSize();
            Result = City.UpgradePrices[call__598 - 1] * FUpgradeLevel;
            if (FWallType == 1)
            {
                var call__599 = GetSize();
                Result += City.WoodWallPrices[call__599 - 1];
            }

            if (FWallType == 2)
            {
                var call__600 = GetSize();
                var call__601 = GetSize();
                Result = Result + City.WoodWallPrices[call__600 - 1] + City.StoneWallPrices[call__601 - 1];
            }

            return Result;
        }

        // City.pas:2241
        public int JoinAmount(sbyte Player)
        {
            int Result = default;
            TArmyHSList Armies = default;
            int I = default;
            TArmy Army = default;
            bool CanJoin = default;
            int Value = default;
            byte Relation = default;
            byte Selection = default;
            Result = 0;
            Armies = new TArmyHSList(null);
            ListArmies(Armies);
            var call__602 = Armies.Count;
            I = call__602;
            CanJoin = true;
            if (I != 0)
            {
                while (I != 0 && CanJoin)
                {
                    I -= 1;
                    var call__603 = Armies.GetArmyHS(I);
                    Army = call__603.FArmy;
                    Selection = Army.GetCanJoinSelection(Player);
                    CanJoin = Selection == Army.ValidateSelection(255);
                    if (CanJoin)
                    {
                        Result += Army.JoinAmount(Player, 255);
                    }
                }

                if (CanJoin)
                {
                    Value = GetObtainValue() / 4;
                    var call__604 = AoWHSMap.FRaces.GetRace(FRace);
                    var call__605 = call__604.GetPlayerRelation(Player);
                    Relation = (byte)call__605;
                    Result += City.TCity_JoinAmount__JoinPercent[(int)(TStatus)Relation] * Value / 100;
                }
                else
                {
                    Result = -1;
                }
            }
            else
            {
                var call__606 = AoWHSMap.FRaces.GetRace(FRace);
                var call__607 = call__606.GetPlayerRelation(Player);
                if (TSet<TStatus>.Of(TStatus.sPolite, TStatus.sFriendly).Contains(call__607))
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
        public bool IncommingNegotiateRequest(TNegotiateRequestEventLog Request)
        {
            bool Result = default;
            int Amount = default;
            if (FPlayer == 0 && !FMode.Contains(TCityModeElement.cmWarmode))
            {
                var call__608 = JoinAmount(Request.FPlayer);
                Amount = call__608;
                if (Amount != -1)
                {
                    Request.FObject = TNegotiateObject.noCity;
                    var call__609 = GetName();
                    var call__610 = TranslateRStr(CityJoinHeaderRStr);
                    var call__611 = Rtl.Rtl.Format(call__610, [call__609]);
                    Request.FText = call__611;
                    Request.FGold = Amount;
                    if (Request.FGold == 0)
                    {
                        var call__612 = TranslateRStr(CityFreeJoinRStr);
                        Request.FJoinMessage.Text = call__612;
                    }
                    else
                    {
                        var call__613 = TranslateRStr(CityTributeJoinRStr);
                        var call__614 = Rtl.Rtl.Format(call__613, [IntToStr(Request.FGold)]);
                        Request.FJoinMessage.Text = call__614;
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
        public bool IncommingNegotiateOffer(TNegotiateTE Offer)
        {
            bool Result = default;
            int Amount = default;
            TArmyHSList Armies = default;
            int I = default;
            TLocationTextEventLog Log = default;
            if (Offer.FObject == TNegotiateObject.noCity)
            {
                var call__615 = JoinAmount(Offer.FPlayer);
                Amount = call__615;
                if (Offer.FOffer == TNegotiateOffer.noOfferBribe && Amount <= Offer.FGold && Amount != -1)
                {
                    var call__616 = AoWHSMap.FPlayers.GetPlayers(Offer.FPlayer);
                    if (call__616.FGems >= Offer.FGold)
                    {
                        SetPlayer(Offer.FPlayer);
                        Armies = new TArmyHSList(null);
                        ListArmies(Armies);
                        {
                            I = 0;
                            int end__617 = Armies.Count - 1;
                            for (; I <= end__617; I += 1)
                            {
                                var call__618 = Armies.GetArmyHS(I);
                                if (call__618.FArmy.FPlayer == 0)
                                {
                                    var call__619 = Armies.GetArmyHS(I);
                                    call__619.FArmy.SetPlayer(Offer.FPlayer);
                                }
                            }
                        }

                        Armies.Free();
                        Log = new TLocationTextEventLog(null);
                        Log.FPlayer = FPlayer;
                        var call__620 = GetName();
                        var call__621 = TranslateRStr(XOverthrownRStr);
                        var call__622 = Rtl.Rtl.Format(call__621, [call__620]);
                        Log.FText = call__622;
                        var call__623 = GetLhx();
                        var call__624 = GetYhx();
                        var call__625 = GetXhx();
                        Log.SetLocation(call__625, call__624, call__623);
                        var call__626 = GetYhx();
                        var call__627 = GetLhx();
                        var call__628 = GetXhx();
                        DistributeLocationEventLog(Log, FPlayer, call__628, call__626, call__627, false);
                        Log.Release();
                        Offer.Accept();
                    }
                    else
                    {
                        var call__629 = NotEnoughGoldRStr;
                        var call__630 = TranslateRStr(call__629);
                        AoWHSMap.ShowMessage(call__630);
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
            TUnitUserList Units = default;
            int I = default;
            Units = new TUnitUserList(null);
            ListUnits(Units);
            {
                I = Units.Count - 1;
                byte end__631 = 0;
                for (; I >= end__631; I -= 1)
                {
                    var call__632 = Units.GetUnit(I);
                    var call__633 = call__632.CanJoin(Player);
                    if (call__633)
                    {
                        var call__634 = Units.GetUnit(I);
                        Units.DeleteUnit(call__634);
                    }
                }
            }

            // { Original uses the race index as the default relation. }
            var call__635 = UnitsToIndependentRelation(Units, FRace);
            Result = call__635;
            Units.Free();
            return Result;
        }

        // City.pas:2345
        public sbyte GetIndependentRelation()
        {
            sbyte Result = default;
            TUnitUserList Units = default;
            Units = new TUnitUserList(null);
            ListUnits(Units);
            // { Original uses the race index as the default relation. }
            var call__636 = UnitsToIndependentRelation(Units, FRace);
            Result = call__636;
            Units.Free();
            return Result;
        }

        // City.pas:2354
        public void ReturnBattleFieldInfo(ref TBattleFieldInfo Info)
        {
            int Value = default;
            TDiplomaticRelation Relation = default;
            if (Info.Requests.Contains(TBattleFieldInfoRequestsElement.bfrEffectiveStrength) && FPlayer > 0)
            {
                Value = 5000;
                var call__637 = AoWHSMap.FRaces.GetRace(FRace);
                var call__638 = call__637.GetPlayerRelation(FPlayer);
                if (TSet<TStatus>.Of(TStatus.sHostile, TStatus.sWary).Contains(call__638))
                {
                    Value = Value * 2;
                }

                var call__639 = DiplomaticRelation(Info.Player, Info.IndependentRelation, FPlayer, 0);
                Relation = call__639;
                if (Info.FriendlyRelations.Contains((byte)(int)Relation))
                {
                    Info.FriendlyStrength -= Value;
                }

                if (Info.EnemyRelations.Contains((byte)(int)Relation))
                {
                    Info.Strength -= Value;
                }
            }
        }

        // City.pas:2367
        public void UpdateVisibleForSeatedPlayer()
        {
            bool Visible = default;
            var call__640 = VisibleForPlayer(AoWHSMap.FSeatedPlayer);
            Visible = call__640;
            if (FEnchantmentsVisible ^ Visible)
            {
                FEnchantmentsVisible = Visible;
                if (FEnchantments != TSet<TCityEnchantmentID>.Of())
                {
                    AoWHSMap.FGlobalMagicControl.TriggerChangedEvent();
                }
            }
        }

        // City.pas:2503
        public ImageLib.TLibraryImage GetTopCityActionImage(TCityAction Action)
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade, TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case TCityAction.caFortify:
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
                    case TCityAction.caUpgrade:
                        Index = 33;
                        break;
                    case TCityAction.caMigrate:
                        Index = 39;
                        break;
                    case TCityAction.caRaze:
                        Index = 51;
                        break;
                    case TCityAction.caLoot:
                        Index = 45;
                        break;
                }

                if (FResource.ImageSequenceList != null && Index != -1)
                {
                    var call__641 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__641 != null)
                    {
                        var call__642 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__643 = call__642.GetPreviewImage();
                        Result = call__643;
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
                        var call__644 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__645 = call__644.FImageLibrary.GetSafe(40);
                        Result = call__645;
                    }
                    else
                    {
                        var call__646 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__647 = call__646.FImageLibrary.GetSafe(43);
                        Result = call__647;
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
        public ImageLib.TLibraryImage GetMiddleCityActionImage(TCityAction Action)
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade, TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case TCityAction.caFortify:
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
                    case TCityAction.caUpgrade:
                        Index = 34;
                        break;
                    case TCityAction.caMigrate:
                        Index = 40;
                        break;
                    case TCityAction.caRaze:
                        Index = 52;
                        break;
                    case TCityAction.caLoot:
                        Index = 46;
                        break;
                }

                if (FResource.ImageSequenceList != null && Index != -1)
                {
                    var call__648 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__648 != null)
                    {
                        var call__649 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__650 = call__649.GetPreviewImage();
                        Result = call__650;
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
                        var call__651 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__652 = call__651.FImageLibrary.GetSafe(41);
                        Result = call__652;
                    }
                    else
                    {
                        var call__653 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__654 = call__653.FImageLibrary.GetSafe(44);
                        Result = call__654;
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
        public ImageLib.TLibraryImage GetBottomCityActionImage(TCityAction Action)
        {
            ImageLib.TLibraryImage Result = default;
            int Index = default;
            if (!TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade, TCityAction.caProduce).Contains(Action))
            {
                Index = -1;
                switch (Action)
                {
                    case TCityAction.caFortify:
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
                    case TCityAction.caUpgrade:
                        Index = 35;
                        break;
                    case TCityAction.caMigrate:
                        Index = 41;
                        break;
                    case TCityAction.caRaze:
                        Index = 53;
                        break;
                    case TCityAction.caLoot:
                        Index = 47;
                        break;
                }

                if (FResource.ImageSequenceList != null && Index != -1)
                {
                    var call__655 = FResource.ImageSequenceList.GetImageSequence(Index);
                    if (call__655 != null)
                    {
                        var call__656 = FResource.ImageSequenceList.GetImageSequence(Index);
                        var call__657 = call__656.GetPreviewImage();
                        Result = call__657;
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
                        var call__658 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__659 = call__658.FImageLibrary.GetSafe(42);
                        Result = call__659;
                    }
                    else
                    {
                        var call__660 = AoWHSSet.FRaceResources.GetRaceResource(FRace);
                        var call__661 = call__660.FImageLibrary.GetSafe(45);
                        Result = call__661;
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


    }

    // FSize
    public partial class TCityResource : TPlayerCropStructureResource
    {
        public TCityResource(TECustomNode owner) : base(owner)
        {
            FRazeable = true;
        }

        // City.pas:2628
        public new const int ClassID = 131337;

        public byte FSize;
        // City.pas:2622


        // City.pas:2669
        public override void MsgProc(ref TEMsg Msg, TEObject Sender)
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
        public void ShowCity(int X, int Y, in TSurfaceDesc SurfaceDesc, TCity City)
        {
            int dX = default;
            int dY = default;
            ImageLib.TLibraryImage Image = default;
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
                var call__662 = City.GetSelected();
                if (call__662 && (AoWHSMap.Frame & 6) == 6)
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
                int end__663 = FTerrainList.Count - 1;
                for (; I <= end__663; I += 1)
                {
                    var call__664 = FTerrainList.Terrain[I];
                    if (call__664 >= 0)
                    {
                        FSize = (byte)(FSize + 1);
                    }
                }
            }
        }

        // City.pas:262


        // City.pas:2633
        public override AnsiString ResourceEditName()
        {
            AnsiString Result = default;
            var call__665 = CityRStr;
            var call__666 = TranslateRStr(call__665);
            Result = call__666;
            return Result;
        }

    }

    // FXhx, FYhx, FLhx, FCityName
    public partial class TCityRebellionEventLog : TEventLog
    {
        public TCityRebellionEventLog(TECustomNode owner) : base(owner)
        { }

        // City.pas:2675
        public new const int ClassID = 131346;

        public int FXhx;
        public int FYhx;
        public int FLhx;
        public AnsiString FCityName;
        // City.pas:2680
        public override void ReadWrite(TEStorageStream Stream)
        {
            base.ReadWrite(Stream);
            Stream.rwInteger(ref FXhx, 20);
            Stream.rwInteger(ref FYhx, 21);
            Stream.rwInteger(ref FLhx, 22);
            Stream.rwString(ref FCityName, 23);
        }

        // City.pas:2689
        public override AnsiString GetText()
        {
            AnsiString Result = default;
            var call__667 = CityRebellionRStr;
            var call__668 = TranslateRStr(call__667);
            var call__669 = Rtl.Rtl.Format(call__668, [FCityName]);
            Result = call__669;
            return Result;
        }

        // City.pas:2699
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            bool ShowEvent = default;
            ShowEvent = Trigger == TExecuteTrigger.etUser;
            bool bool__670 = ShowEvent;
            if (!bool__670)
            {
                bool bool__671 = AoWHSMap.FPlayMode == TPlayMode.pmTurn;
                if (bool__671)
                {
                    var call__672 = AoWEngine.FRegistry.GetClassicWindowPopupEvents();
                    bool__671 = call__672.Contains(TWindowPopupEvent.wpRebellion);
                }

                bool__670 = bool__671;
            }

            ShowEvent = bool__670;
            bool bool__673 = ShowEvent;
            if (!bool__673)
            {
                bool bool__674 = AoWHSMap.FPlayMode == TPlayMode.pmSimultaneous;
                if (bool__674)
                {
                    var call__675 = AoWEngine.FRegistry.GetSimultaneousWindowPopupEvents();
                    bool__674 = call__675.Contains(TWindowPopupEvent.wpRebellion);
                }

                bool__673 = bool__674;
            }

            ShowEvent = bool__673;
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
            CenterViewTo((sbyte)FXhx, (sbyte)FYhx, (sbyte)FLhx);
        }

    }

    public partial class TAICityUpgradeBudget : TAIBudget
    {
        // City.pas:2775
        public new const int ClassID = 131349;

        public TAICityUpgradeBudget(TECustomNode owner) : base(owner)
        { }

    }

    public partial class TAICityResearchBudget : TAIBudget
    {
        // City.pas:2780
        public new const int ClassID = 131350;

        public TAICityResearchBudget(TECustomNode owner) : base(owner)
        { }

    }

    public partial class TAICityFortifyBudget : TAIBudget
    {
        // City.pas:2785
        public new const int ClassID = 131351;

        public TAICityFortifyBudget(TECustomNode owner) : base(owner)
        { }

    }

    // FCityCount, FProductionLevelCounts, FMaxProductionLevel, FProductionRatio
    public partial class TCityAIPA : TAIPlayerAction, IAIPlayerActionClass
    {
        public TCityAIPA(TECustomNode owner) : base(owner)
        { }

        public int FCityCount;
        public FixedArray_int_4 FProductionLevelCounts;
        public int FMaxProductionLevel;
        public float FProductionRatio;
        // City.pas:3130
        public override bool Process()
        {
            bool Result = default;
            TPlayer Player = default;
            TAIBudgetManager Manager = default;
            TAIBudget UpgradeBudget = default;
            TAIBudget ResearchBudget = default;
            TAIBudget FortifyBudget = default;
            UpdateStatistics();
            Player = FControl.FPlayer;
            if (Player.FPlayerIndex != 0)
            {
                Manager = Player.FAIBudget;
                var call__676 = Manager.FBudgets.FindChild(131349);
                UpgradeBudget = (TAIBudget)call__676;
                if (UpgradeBudget == null)
                {
                    UpgradeBudget = new TAICityUpgradeBudget(null);
                    UpgradeBudget.Owner = Manager.FBudgets;
                    UpgradeBudget.FRequiredGold = 250;
                    UpgradeBudget.FDays = 7;
                }

                var call__677 = Manager.FBudgets.FindChild(131350);
                ResearchBudget = (TAIBudget)call__677;
                if (ResearchBudget != null)
                {
                    if (ResearchBudget.FGold + UpgradeBudget.FGold > 125)
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
                    ResearchBudget = new TAICityResearchBudget(null);
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
                var call__678 = Manager.FBudgets.FindChild(131351);
                FortifyBudget = (TAIBudget)call__678;
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
                    if (AoWHSMap.FDay > 5)
                    {
                        FortifyBudget = new TAICityFortifyBudget(null);
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
        public bool ValidResearchCity(TCity City)
        {
            bool Result = default;
            var call__679 = City.FProduction.GetProduceUnitLevelMax();
            Result = call__679 < City.FUpgradeLevel;
            bool bool__680 = Result;
            if (bool__680)
            {
                var call__681 = City.GetActiveAction();
                bool bool__682 = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(call__681);
                if (!bool__682)
                {
                    var call__683 = City.GetActiveAction();
                    bool__682 = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(call__683) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                bool__680 = bool__682;
            }

            Result = bool__680;
            return Result;
        }

        // City.pas:2802
        public bool ValidResearchBudgetCity(TCity City)
        {
            bool Result = default;
            var call__684 = City.FProduction.GetProduceUnitLevelMax();
            bool bool__685 = call__684 != City.FUpgradeLevel;
            if (!bool__685)
            {
                var call__686 = City.GetActiveAction();
                bool__685 = call__686 == TCityAction.caUpgrade;
            }

            Result = bool__685;
            return Result;
        }

        // City.pas:2807
        public void ValidateResearchBudget(TAIBudget Budget)
        {
            TPlayer Player = default;
            int I = default;
            int Count = default;
            int Total = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            Total = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__687 = 0;
                for (; I >= end__687; I -= 1)
                {
                    var call__688 = Player.FStructures.GetStructure(I);
                    Structure = call__688;
                    if (Structure is TCity)
                    {
                        Total += 1;
                        if (ValidResearchBudgetCity((TCity)Structure))
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
                Priority = 7 - Count * Total / 3;
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
        public bool ResearchProduction(TAIBudget ResearchBudget, TAIBudget UpgradeBudget)
        {
            bool Result = default;
            TDoubleItemList Cities = default;
            bool Valid = default;
            TPlayer Player = default;
            int I = default;
            int J = default;
            int UnitIndex = default;
            int Index = default;
            int Value = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            TCity City = default;
            TProductionItem Production = default;
            Result = false;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList();
            {
                I = Player.FStructures.Count - 1;
                byte end__689 = 0;
                for (; I >= end__689; I -= 1)
                {
                    var call__690 = Player.FStructures.GetStructure(I);
                    Structure = call__690;
                    if (Structure is TCity && ValidResearchCity((TCity)Structure))
                    {
                        City = (TCity)Structure;
                        Valid = false;
                        {
                            J = City.FProduction.FUnitProductions.Count - 1;
                            byte end__691 = 0;
                            for (; J >= end__691; J -= 1)
                            {
                                var call__692 = City.FProduction.FUnitProductions.GetItem(J);
                                UnitIndex = call__692.FUnitIndex;
                                if (UnitIndex != -1)
                                {
                                    bool bool__693 = Valid;
                                    if (!bool__693)
                                    {
                                        var call__694 = AoWHSSet.FUnitResources.GetUnitResource(UnitIndex);
                                        bool bool__695 = call__694.FLevel == City.FUpgradeLevel;
                                        if (bool__695)
                                        {
                                            var call__696 = City.FProduction.FUnitProductions.GetItem(J);
                                            bool__695 = !call__696.FResearched;
                                        }

                                        bool bool__697 = bool__695;
                                        if (bool__697)
                                        {
                                            var call__698 = City.ResearchCost(J);
                                            bool__697 = call__698 <= ResearchBudget.FGold + UpgradeBudget.FGold;
                                        }

                                        bool__693 = bool__697;
                                    }

                                    Valid = bool__693;
                                }
                            }
                        }

                        if (Valid)
                        {
                            var call__699 = City.GetSize();
                            Priority = call__699 * 2 + (City.FProductionPriority + City.FWallType * 3);
                            Cities.AddAB(ObjectToInt(City), Priority);
                        }
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                Index = 0;
                var call__700 = Cities[0];
                Value = Round((double)call__700.B * 0.75);
                while (true)
                {
                    bool bool__701 = Index + 1 < Cities.Count;
                    if (bool__701)
                    {
                        var call__702 = Cities[Index + 1];
                        bool__701 = Value <= call__702.B;
                    }

                    if (!bool__701)
                        break;
                    {
                        Index += 1;
                    }
                }

                var call__703 = AoWHSMap.Random(Index + 1);
                Index = call__703;
                var call__704 = Cities[Index];
                City = ReferenceFromId<TCity>(call__704.A);
                var call__705 = AoWHSMap.Random(16777215);
                RandSeed = call__705;
                var call__706 = Cities[Index];
                var call__707 = ReferenceFromId<TCity>(call__706.A).FProduction.FindOptimalResearchProduction(null, ResearchBudget.FGold + UpgradeBudget.FGold);
                Index = call__707;
                if (Index != -1)
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = new TProductionItem(null);
                    Production.FProductionID = 2;
                    Production.FParameter = Index;
                    City.FProduction.ExecuteAddProduction(Production);
                    Production.Free();
                    Index = City.ResearchCost(Index);
                    if (Index > ResearchBudget.FGold)
                    {
                        Index -= ResearchBudget.FGold;
                        Player.FAIBudget.BudgetExpenseMade(ResearchBudget.FGold, 0, ResearchBudget, TBudgetExpenseMode.bemBudget);
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, UpgradeBudget, TBudgetExpenseMode.bemBudget);
                    }
                    else
                    {
                        Player.FAIBudget.BudgetExpenseMade(Index, 0, ResearchBudget, TBudgetExpenseMode.bemBudget);
                    }

                    Result = true;
                }
            }

            Cities.Free();
            return Result;
        }

        // City.pas:2909
        public bool ValidUpgradeCity(TCity City)
        {
            bool Result = default;
            AnsiString Reason = default;
            int Level = default;
            var call__708 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__708;
            Result = City.CanUpgrade(false, ref Reason);
            bool bool__709 = Result;
            if (bool__709)
            {
                var call__710 = City.GetSize();
                bool__709 = call__710 > City.FUpgradeLevel;
            }

            Result = bool__709 && !(Level == FMaxProductionLevel && FProductionLevelCounts[Level - 1] == 1) && Level == City.FUpgradeLevel;
            bool bool__711 = Result;
            if (bool__711)
            {
                var call__712 = City.GetActiveAction();
                bool bool__713 = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(call__712);
                if (!bool__713)
                {
                    var call__714 = City.GetActiveAction();
                    bool__713 = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(call__714) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                bool__711 = bool__713;
            }

            Result = bool__711;
            return Result;
        }

        // City.pas:2921
        public bool ValidUpgradeBudgetCity(TCity City)
        {
            bool Result = default;
            int Level = default;
            var call__715 = City.FProduction.GetProduceUnitLevelMax();
            Level = call__715;
            var call__716 = City.GetSize();
            if (call__716 <= City.FUpgradeLevel || Level != City.FUpgradeLevel)
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
        public bool UpgradeCity(TAIBudget Budget)
        {
            bool Result = default;
            TPlayer Player = default;
            TDoubleItemList Cities = default;
            int I = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            TCity City = default;
            TProductionItem Production = default;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList();
            {
                I = Player.FStructures.Count - 1;
                byte end__717 = 0;
                for (; I >= end__717; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__718 = Player.FStructures.GetStructure(I);
                    Structure = call__718;
                    if (Structure is TCity && ValidUpgradeCity((TCity)Structure))
                    {
                        City = (TCity)Structure;
                        var call__719 = City.GetSize();
                        Priority = call__719 * 2 + (City.FProductionPriority + City.FWallType * 3);
                        Cities.AddAB(ObjectToInt(City), Priority);
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                var call__720 = Cities[0];
                City = ReferenceFromId<TCity>(call__720.A);
                {
                    City.FProduction.ExecuteDeleteProductions();
                    Production = new TProductionItem(null);
                    var call__721 = Units.City.City.CityActionToProductionID(TCityAction.caUpgrade);
                    Production.FProductionID = call__721;
                    City.FProduction.ExecuteAddProduction(Production);
                    Production.Free();
                    Player.FAIBudget.BudgetExpenseMade(250, 0, Budget, TBudgetExpenseMode.bemBudget);
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
        public void ValidateUpgradeBudget(TAIBudget Budget)
        {
            TPlayer Player = default;
            int I = default;
            int Count = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__722 = 0;
                for (; I >= end__722; I -= 1)
                {
                    var call__723 = Player.FStructures.GetStructure(I);
                    Structure = call__723;
                    if (Structure is TCity)
                    {
                        if (ValidUpgradeBudgetCity((TCity)Structure))
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
                Priority = 8 - Count / 2;
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
        public bool ValidFortifyCity(TCity City)
        {
            bool Result = default;
            AnsiString Reason = default;
            Result = City.CanFortify(false, ref Reason);
            bool bool__724 = Result && City.FWallType == 0;
            if (!bool__724)
            {
                bool bool__725 = City.FWallType == 1;
                if (bool__725)
                {
                    var call__726 = City.GetSize();
                    bool__725 = call__726 > 2;
                }

                bool__724 = bool__725 && City.FUpgradeLevel > 1 && AoWHSMap.FDay >= 30;
            }

            Result = bool__724;
            bool bool__727 = Result;
            if (bool__727)
            {
                var call__728 = City.GetActiveAction();
                bool bool__729 = TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade).Contains(call__728);
                if (!bool__729)
                {
                    var call__730 = City.GetActiveAction();
                    bool__729 = TSet<TCityAction>.Of(TCityAction.caProduce).Contains(call__730) && City.FProduction.FSettings.FTurnsLeft == City.FProduction.FSettings.FTurnsTotal;
                }

                bool__727 = bool__729;
            }

            Result = bool__727;
            return Result;
        }

        // City.pas:3009
        public bool ValidFortifyBudgetCity(TCity City)
        {
            bool Result = default;
            bool bool__731 = City.FWallType == 0;
            if (!bool__731)
            {
                bool bool__732 = City.FWallType == 1;
                if (bool__732)
                {
                    var call__733 = City.GetSize();
                    bool__732 = call__733 > 2;
                }

                bool__731 = bool__732 && City.FUpgradeLevel > 1 && AoWHSMap.FDay >= 30;
            }

            Result = bool__731;
            return Result;
        }

        // City.pas:3015
        public bool FortifyCity(TAIBudget Budget)
        {
            bool Result = default;
            TPlayer Player = default;
            TDoubleItemList Cities = default;
            int I = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            TCity City = default;
            TProductionItem Production = default;
            Player = FControl.FPlayer;
            Cities = new TDoubleItemList();
            {
                I = Player.FStructures.Count - 1;
                byte end__734 = 0;
                for (; I >= end__734; I -= 1)
                {
                    // { Preserve the original runtime class check. }
                    var call__735 = Player.FStructures.GetStructure(I);
                    Structure = call__735;
                    if (Structure is TCity && ValidFortifyCity((TCity)Structure))
                    {
                        City = (TCity)Structure;
                        var call__736 = City.GetSize();
                        Priority = call__736 * 2 + City.FProductionPriority;
                        if (City.FWallType == 0)
                        {
                            Priority += 3;
                        }

                        Cities.AddAB(ObjectToInt(City), Priority);
                    }
                }
            }

            Cities.SortDescendingItemB();
            if (Cities.Count != 0)
            {
                var call__737 = Cities[0];
                City = ReferenceFromId<TCity>(call__737.A);
                {
                    City.FProduction.ExecuteDeleteProductions();
                    var call__738 = City.FortifyCost();
                    Player.FAIBudget.BudgetExpenseMade(call__738, 0, Budget, TBudgetExpenseMode.bemBudget);
                    Production = new TProductionItem(null);
                    var call__739 = Units.City.City.CityActionToProductionID(TCityAction.caFortify);
                    Production.FProductionID = call__739;
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
        public void ValidateFortifyBudget(TAIBudget Budget)
        {
            TPlayer Player = default;
            int I = default;
            int Count = default;
            int Priority = default;
            TPlayerStructure Structure = default;
            Player = FControl.FPlayer;
            Count = 0;
            {
                I = Player.FStructures.Count - 1;
                byte end__740 = 0;
                for (; I >= end__740; I -= 1)
                {
                    var call__741 = Player.FStructures.GetStructure(I);
                    Structure = call__741;
                    if (Structure is TCity)
                    {
                        if (ValidFortifyBudgetCity((TCity)Structure))
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
                Priority = 8 - Count / 2;
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
            TPlayerStructure Structure = default;
            TCity City = default;
            Count = 0;
            {
                I = FControl.FPlayer.FStructures.Count - 1;
                byte end__742 = 0;
                for (; I >= end__742; I -= 1)
                {
                    var call__743 = FControl.FPlayer.FStructures.GetStructure(I);
                    Structure = call__743;
                    if (Structure is TCity)
                    {
                        City = (TCity)Structure;
                        var city = (TCity)Structure;
                        var call__745 = city.GetSize();
                        if (call__745 > city.FUpgradeLevel)
                        {
                            Count += 1;
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
            TCity City = default;
            FCityCount = 0;
            FMaxProductionLevel = 0;
            Available = 0;
            Busy = 0;
            FProductionLevelCounts = default;
            {
                I = FControl.FPlayer.FStructures.Count - 1;
                byte end__746 = 0;
                for (; I >= end__746; I -= 1)
                {
                    var call__747 = FControl.FPlayer.FStructures.GetStructure(I);
                    if (call__747 is TCity)
                    {
                        FCityCount += 1;
                        var call__748 = FControl.FPlayer.FStructures.GetStructure(I);
                        City = (TCity)call__748;
                        if (TSet<TCityAction>.Of(TCityAction.caNone, TCityAction.caTrade, TCityAction.caProduce).Contains(City.GetActiveAction()))
                        {
                            Available += (City.GetSize() + 1) / 2;
                            var call__749 = City.FProduction.GetProduceUnitLevelMax();
                            Level = call__749;
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
                FProductionRatio = Round((double)Available / (Available + Busy));
            }
            else
            {
                FProductionRatio = 0;
            }
        }

        // City.pas:2790
        public static new int Priority => 13;

    }

    public partial class TCityProductionEventLog : TProductionEventLog
    {
        public TCityProductionEventLog(TECustomNode owner) : base(owner)
        { }

        // City.pas:3195
        public new const int ClassID = 131834;

        // City.pas:3205
        public override bool Execute(bool Mode, TExecuteTrigger Trigger, TExecuteDoneEvent Done)
        {
            bool Result = default;
            TCity City = default;
            if (Trigger == TExecuteTrigger.etUser)
            {
                City = GetCity();
                if (City != null && AoWHSMap.FSeatedPlayer == City.FPlayer)
                {
                    AoWHSMap.CenterViewTo(FXhx, FYhx, FLhx);
                    AoWHSMap.TriggerExecuteEventLog(this, true, Done);
                }
            }

            Result = false;
            return Result;
        }

        // City.pas:3200
        public TCity GetCity()
        {
            TCity Result = default;
            var call__750 = AoWHSMap.MapContainer.GetField(FXhx, FYhx, FLhx);
            var call__751 = call__750.FindNoneTransparentHS(typeof(TCity));
            Result = (TCity)call__751;
            return Result;
        }

    }

    public partial class TRegistrator : TObject
    {
        // City.pas:3306
        public static void MapLoaded(TObject Sender)
        {
            if (Sender == AoWHSMap)
            {
                AoWHSMap.FAIPlayerActionManager.RegisterClasses([AIPlayerActionClass.Of<TCityAIPA>()]);
            }
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
        public static FixedArray_int_5 TCity_JoinAmount__JoinPercent = FixedArray_int_5.From([200, 175, 150, 125, 100]);
        // City.pas:3220
        public static TCityAction ProductionIDToCityAction(int ID)
        {
            TCityAction Result = default;
            switch (ID)
            {
                case 1:
                    Result = TCityAction.caProduce;
                    break;
                case 2:
                    Result = TCityAction.caResearch;
                    break;
                case 10:
                    Result = TCityAction.caTrade;
                    break;
                case 11:
                    Result = TCityAction.caUpgrade;
                    break;
                case 12:
                    Result = TCityAction.caFortify;
                    break;
                case 13:
                    Result = TCityAction.caMigrate;
                    break;
                case 14:
                    Result = TCityAction.caLoot;
                    break;
                default:
                    Result = TCityAction.caNone;
                    break;
            }

            return Result;
        }

        // City.pas:3234
        public static int CityActionToProductionID(TCityAction Action)
        {
            int Result = default;
            switch (Action)
            {
                case TCityAction.caProduce:
                    Result = 1;
                    break;
                case TCityAction.caResearch:
                    Result = 2;
                    break;
                case TCityAction.caTrade:
                    Result = 10;
                    break;
                case TCityAction.caUpgrade:
                    Result = 11;
                    break;
                case TCityAction.caFortify:
                    Result = 12;
                    break;
                case TCityAction.caMigrate:
                    Result = 13;
                    break;
                case TCityAction.caLoot:
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
            TPlayer P = default;
            int I = default;
            Result = false;
            var call__752 = AoWHSMap.FPlayers.GetPlayers(Player);
            P = call__752;
            I = 0;
            while (true)
            {
                var call__753 = P.FStructures.Count;
                if (!(I != call__753 && !Result))
                    break;
                {
                    var call__754 = P.FStructures.GetStructure(I);
                    if (call__754 is TCity)
                    {
                        var call__755 = P.FStructures.GetStructure(I);
                        var call__756 = ((TCity)call__755).GetBuilding();
                        bool bool__757 = !call__756;
                        if (bool__757)
                        {
                            var call__758 = P.FStructures.GetStructure(I);
                            bool__757 = ((TCity)call__758).FRace == Race;
                        }

                        Result = bool__757;
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
            TPlayer P = default;
            int I = default;
            TPlayerStructure Structure = default;
            if (AoWHSMap != null)
            {
                var call__759 = AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__759;
                Result = 0;
                {
                    I = 0;
                    int end__760 = P.FStructures.Count - 1;
                    for (; I <= end__760; I += 1)
                    {
                        var call__761 = P.FStructures.GetStructure(I);
                        Structure = call__761;
                        var call__762 = Structure.EClass.ClassID;
                        if (call__762 == 131336 && Structure.FPlayer == Player)
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
            TPlayer P = default;
            int I = default;
            TPlayerStructure Structure = default;
            if (AoWHSMap != null)
            {
                var call__763 = AoWHSMap.FPlayers.GetPlayers(Player);
                P = call__763;
                Result = 0;
                {
                    I = 0;
                    int end__764 = P.FStructures.Count - 1;
                    for (; I <= end__764; I += 1)
                    {
                        var call__765 = P.FStructures.GetStructure(I);
                        Structure = call__765;
                        var call__766 = Structure.EClass.ClassID;
                        if (call__766 == 131336 && Structure.FPlayer == Player)
                        {
                            Result += ((TCity)Structure).FIncome;
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

        public static FixedArray_Aow_Rtl_AnsiString_9 TCityActionRStr = FixedArray_Aow_Rtl_AnsiString_9.From([(AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)"", (AnsiString)""]);
        // export
        public static FixedArray_Aow_Rtl_AnsiString_7 TCivilStatusRStr = FixedArray_Aow_Rtl_AnsiString_7.From([(AnsiString)"Unruly", (AnsiString)"Unrest", (AnsiString)"Stable", (AnsiString)"Content", (AnsiString)"Cheerful", (AnsiString)"Oppressed", (AnsiString)"Slavery"]);
        public static FixedArray_Aow_Rtl_AnsiString_4 TCityEnchantmentIDRStr = FixedArray_Aow_Rtl_AnsiString_4.From([(AnsiString)"Fountain Of Life", (AnsiString)"Gold Rush", (AnsiString)"Anti Magic Shell", (AnsiString)"Warmonger"]);
        public static FixedArray_byte_4 UpgradePrices = FixedArray_byte_4.From([250, 250, 250, 250]);
        public static FixedArray_byte_4 WoodWallPrices = FixedArray_byte_4.From([20, 40, 60, 80]);
        public static FixedArray_byte_4 StoneWallPrices = FixedArray_byte_4.From([20, 40, 60, 80]);
        public static readonly AnsiString CityNotEnoughGoldRStr = (AnsiString)"Not enough gold.";
        public static readonly AnsiString CityWarningRStr = (AnsiString)"Warning";
        public static readonly AnsiString CityWoodenWallRStr = (AnsiString)"Wooden Wall";
        public static readonly AnsiString CityStoneWallRStr = (AnsiString)"Stone Wall";
        public static TCity SortingCity;
        // City.pas:2005
        public static int CompareCityProductions(object Item1, object Item2)
        {
            int Result = default;
            int Value1 = default;
            int Value2 = default;
            var unitProduction = (TUnitProduction)Item1;
            var call__768 = AoWHSSet.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
            if (call__768 != null)
            {
                var call__769 = AoWHSSet.FUnitResources.GetUnitResource(unitProduction.FUnitIndex);
                Value1 = call__769.FLevel * 2;
                if (SortingCity.FRace != -1)
                {
                    var call__770 = AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace);
                    if (call__770.FSpecialUnitIndexes.Count > 3)
                    {
                        var call__771 = AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace);
                        var call__772 = call__771.FSpecialUnitIndexes[3];
                        if (call__772 == unitProduction.FUnitIndex)
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

            var unitProduction2 = (TUnitProduction)Item2;
            var call__774 = AoWHSSet.FUnitResources.GetUnitResource(unitProduction2.FUnitIndex);
            if (call__774 != null)
            {
                var call__775 = AoWHSSet.FUnitResources.GetUnitResource(unitProduction2.FUnitIndex);
                Value2 = call__775.FLevel * 2;
                if (SortingCity.FRace != -1)
                {
                    var call__776 = AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace);
                    if (call__776.FSpecialUnitIndexes.Count > 3)
                    {
                        var call__777 = AoWHSSet.FRaceResources.GetRaceResource(SortingCity.FRace);
                        var call__778 = call__777.FSpecialUnitIndexes[3];
                        if (call__778 == unitProduction2.FUnitIndex)
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

            Result = Value1 - Value2;
            if (Result == 0)
            {
                Result = ((TUnitProduction)Item1).FUnitIndex - ((TUnitProduction)Item2).FUnitIndex;
            }

            return Result;
        }

        public static void Initialize()
        {
            if (ValidateAoWEngine())
            {
                AoWEngine.RegisterEClasses([EClass.Of<TCityEnchantment>(), EClass.Of<TCity>(), EClass.Of<TCityResource>(), EClass.Of<TCityRebellionEventLog>(), EClass.Of<TAICityUpgradeBudget>(), EClass.Of<TAICityResearchBudget>(), EClass.Of<TAICityFortifyBudget>(), EClass.Of<TCityProductionEventLog>()]);
                AoWEngine.OnHSMLoaded.Add(TRegistrator.MapLoaded);
            }
        }

        public static void Finalize()
        {
            if (AoWEngine != null)
            {
                AoWEngine.OnHSMLoaded.Remove(TRegistrator.MapLoaded);
            }
        }
    }
}
