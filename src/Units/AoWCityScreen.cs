// Translated from AoWCityScreen.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.City;
using static Aow.Units.City.City;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.AoWComp;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.OverviewUnit.OverviewUnit;

namespace Aow.Units.AoWCityScreen
{
    // city, preview, view, action, production, queue, mode, migrations, sort, images
    public partial class TCityScreen : TDataModule
    {
        public TCityScreen(TComponent owner) : base(owner)
        { }

        public TAOWWindow Dlg;
        public TAOWPanel UnitPnl;
        public TAOWListBox UAbilitiesLB;
        public TAOWImage UAttackIcon;
        public TAOWImage UDefenseIcon;
        public TAOWImage UMovesIcon;
        public TAOWImage UDamageIcon;
        public TAOWImage UResistanceIcon;
        public TAOWImage UHitsIcon;
        public TAOWLabel UAttack;
        public TAOWLabel UDefense;
        public TAOWLabel UMoves;
        public TAOWLabel UDamage;
        public TAOWLabel UResistance;
        public TAOWLabel UHits;
        public TAOWPanel UStatPnl;
        public TAOWPanel UStatP1;
        public TAOWPanel UStatP2;
        public TAOWLabel UAlignment;
        public TAOWLabel ULevel;
        public TAOWPanel UPicturePnl;
        public TAOWPanel UCostPnl;
        public TAOWLabel StructName;
        public TAOWLabel CostLbl;
        public TAOWLabel UCost;
        public TAOWLabel UnitLbl;
        public TAOWLabel TurnsLbl;
        public TAOWLabel UTurns;
        public TAOWCheckBox P1CB;
        public TAOWCheckBox P2CB;
        public TAOWCheckBox P3CB;
        public TAOWCheckBox P4CB;
        public TAOWCheckBox P5CB;
        public TAOWCheckBox P6CB;
        public TAOWCheckBox P7CB;
        public TAOWCheckBox P8CB;
        public TAOWCheckBox P9CB;
        public TAOWCheckBox P10CB;
        public TAOWCheckBox P11CB;
        public TAOWCheckBox P12CB;
        public TAOWCheckBox B4CB;
        public TAOWCheckBox B5CB;
        public TAOWCheckBox B6CB;
        public TAOWCheckBox B1CB;
        public TAOWCheckBox B2CB;
        public TAOWCheckBox B3CB;
        public TAOWVScrollBar T2SBar;
        public TAOWPanel AbilPnl;
        public TAOWPanel UpgradePnl;
        public TAOWLabel UpgradeLbl;
        public TAOWPanel UpgradeMemoFr;
        public TAOWMemo UpgradeMemo;
        public TAOWPanel RightPnl;
        public TAOWPanel QueuePnl;
        public TAOWImage Oval;
        public TAOWImage ShadowL;
        public TAOWImage ShadowR;
        public TAOWButton CityBtn3;
        public TAOWButton CityBtn1;
        public TAOWListBox QueueLB;
        public TAOWPanel ActivityPnl;
        public TAOWPanel InfoPnl2;
        public TAOWLabel ActiveAction;
        public TAOWLabel ActiveActionTurns;
        public TAOWImage SepBar;
        public TAOWPanel ActivityPnl2;
        public TAoWGauge ActiveActionTurnsBar;
        public TAOWButton CityBtn2;
        public TAOWPanel InfoPnl;
        public TAOWPanel IPPnl;
        public TAoWFrame IPPicFr;
        public TAOWMemo PopulationMemo;
        public TAOWMemo PopulationHdr;
        public TAOWPanel IIPnl;
        public TAoWFrame IIPicFr;
        public TAOWMemo IncomeMemo;
        public TAOWMemo IncomeHdr;
        public TAOWPanel IEPnl;
        public TAoWFrame IEPicFr;
        public TAOWMemo EnhancementsMemo;
        public TAOWMemo EnhancementsHdr;
        public TAOWPanel CurrentInfo;
        public TAoWFrame RPillar;
        public TAoWFrame LPillar;
        public TAOWPanel QueueInfo;
        public TAOWLabel QueueLbl;
        public TAOWCheckBox LoopQueueBox;
        public TAOWLabel RepeatLbl;
        public TAOWPanel ActiveActionPnl;
        public TAOWImage CurrentActionImage;
        public TAOWImage RacePortrait;
        public TAoWFrame AoWFrame1;
        public TAOWImage BlackBg;
        public TAOWImage BlackBg2;
        public TAOWPanel UPictureFramePnl;
        public TAOWPanel UStatFr1;
        public TAOWPanel UStatFr2;
        public TAOWPanel UInfoFr;
        public TAOWPanel CostFr;
        public TAOWPanel TurnsFr;
        public TAOWPanel UpgPicPnl;
        public TAOWImage UpgradePic;
        public TAOWPanel UpgPicFr;
        public TAOWPanel UpgCostPnl;
        public TAOWLabel UpgCostLbl;
        public TAOWLabel UpgradeCost;
        public TAOWLabel UpgTurnsLbl;
        public TAOWLabel UpgradeTurns;
        public TAOWPanel UpgCostFr;
        public TAOWPanel UpgTurnsFr;
        public TAOWPanel MigratePnl;
        public TAOWLabel NewRaceLbl;
        public TAOWPanel MigrateMainFr;
        public TAOWPanel MigPicPnl;
        public TAOWImage MigratePic;
        public TAOWPanel MigPicFr;
        public TAOWPanel MigCostPnl;
        public TAOWLabel MigCostLbl;
        public TAOWLabel MigrateCost;
        public TAOWLabel MigTurnsLbl;
        public TAOWLabel MigrateTurns;
        public TAOWPanel MigCostFr;
        public TAOWPanel MigTurnsFr;
        public TAOWListBox MigrationRaceLB;
        public TAOWListBox MigrationRelationLB;
        public TAOWListBox MigrationTurnsLB;
        public TAOWButton SortMigrationRaces;
        public TAOWButton SortMigrationTurns;
        public TAOWButton SortMigrationRelation;
        public TAOWPanel T3LBPnl;
        public TAOWVScrollBar MigrationSB;
        public TAOWLabel T3RaceLbl;
        public TAOWLabel CurrentRace;
        public TAOWLabel T3RelLbl;
        public TAOWLabel CurrentRelation;
        public TAOWPanel CurrentRacePnl;
        public TAOWPanel CurrentRaceFr;
        public TAOWLabel CurrentRaceLbl;
        public TAOWPanel UpgBtm;
        public TAOWLabel UpgradeInfo;
        public TAOWImage RelIcn;
        public TAOWVScrollBar QueueVSB;
        public TAOWVScrollBar IPVSB;
        public TAOWVScrollBar IIVSB;
        public TAOWVScrollBar IEVSB;
        public TAOWMemo IncomeGoldMemo;
        public TAOWLabel Migrate;
        public TaowUserDrawBox UFace;
        public TAOWPanel MigrateInfoPnl;
        public TAOWLabel MigrateInfo;
        public TaowUserDrawBox MapView;
        public TAOWImage InfoSep;
        public TAOWButton PNext;
        public TAOWButton PPrev;
        public TIvTranslator Translator;
        public TCity FCity;
        public TAoWCombatMap FPreview;
        public int FViewX;
        public int FViewY;
        public TCityAction FAction;
        public int FProductionIndex;
        public int FQueueIndex;
        public byte FMode;
        public TMigrationList FMigrations;
        public int FMigrationSort;
        public TIntegerList FImages;
        // AoWCityScreen.pas:748
        public void FreePreview()
        {
            if (FPreview != null)
            {
                FPreview.Free();
                FPreview = null;
            }
        }

        // AoWCityScreen.pas:757
        public void ReleaseImages()
        {
            int I = default;
            {
                I = FImages.Count - 1;
                byte end__1 = 0;
                for (; I >= end__1; I -= 1)
                {
                    var call__2 = FImages[I];
                    var call__3 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__2);
                    call__3.Release();
                }
            }

            FImages.Clear();
        }

        // AoWCityScreen.pas:766
        public void LoadUnitImages(int Index)
        {
            int Gfx = default;
            var call__4 = AoWHSSet.FUnitResources.GetUnitResource(Index);
            Gfx = call__4.FUnitGFXResourceIndex;
            var call__5 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx);
            call__5.AddRef();
            FImages.Add(Gfx);
        }

        // AoWCityScreen.pas:775
        public void ResetChecks()
        {
            P1CB.Checked = false;
            P2CB.Checked = false;
            P3CB.Checked = false;
            P4CB.Checked = false;
            P5CB.Checked = false;
            P6CB.Checked = false;
            P7CB.Checked = false;
            P8CB.Checked = false;
            P9CB.Checked = false;
            P10CB.Checked = false;
            P11CB.Checked = false;
            P12CB.Checked = false;
            B1CB.Checked = false;
            B2CB.Checked = false;
            B3CB.Checked = false;
            B4CB.Checked = false;
            B5CB.Checked = false;
            B6CB.Checked = false;
        }

        // AoWCityScreen.pas:797
        public void Reset()
        {
            ReleaseImages();
            FMode = 0;
            InfoPnl.Visible = true;
            InfoPnl2.Visible = true;
            ActivityPnl.Visible = false;
            ActivityPnl2.Visible = false;
            UnitPnl.Visible = false;
            UpgradePnl.Visible = false;
            MigratePnl.Visible = false;
            CityBtn1.Visible = true;
            var call__6 = TranslateRStr(OptionsRStr);
            CityBtn1.Text = call__6;
            CityBtn2.Visible = false;
            PPrev.Visible = !CityBtn2.Visible;
            PNext.Visible = !CityBtn2.Visible;
            ShadowL.Visible = AoWEngine.FAnimationDetail != 0;
            ShadowR.Visible = AoWEngine.FAnimationDetail != 0;
            Oval.Visible = AoWEngine.FAnimationDetail != 0;
            FMigrationSort = 0;
            FProductionIndex = 0;
            QueueLB.Selected = -1;
            FreePreview();
            var call__7 = TranslateRStr(QueueRStr);
            QueueLbl.Text = call__7;
            var call__8 = TranslateRStr(RepeatRStr);
            RepeatLbl.Text = call__8;
            if (FMigrations != null)
            {
                FMigrations.Free();
                FMigrations = null;
            }
        }

        // AoWCityScreen.pas:829
        public void CloseCity()
        {
            Reset();
            FCity = null;
            General.Manager.OverlayPriority = 0;
        }

        // AoWCityScreen.pas:836
        public void SetMode(byte Mode)
        {
            FMode = Mode;
            switch (FMode)
            {
                case 0:
                    InfoPnl.Visible = true;
                    InfoPnl2.Visible = true;
                    ActivityPnl.Visible = false;
                    ActivityPnl2.Visible = false;
                    var call__9 = TranslateRStr(ChangeRStr);
                    CityBtn1.Text = call__9;
                    CityBtn1.Visible = true;
                    CityBtn2.Visible = false;
                    PPrev.Visible = !CityBtn2.Visible;
                    PNext.Visible = !CityBtn2.Visible;
                    var call__10 = TranslateRStr(ExitRStr);
                    CityBtn3.Text = call__10;
                    QueueLB.Selected = -1;
                    LPillar.Visible = true;
                    RPillar.Visible = true;
                    MapView.WinHeight = InfoPnl.WinTop - MapView.WinTop;
                    break;
                case 1:
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    var call__11 = TranslateRStr(InfoRStr);
                    CityBtn1.Text = call__11;
                    var call__12 = TranslateRStr(DoneRStr);
                    CityBtn3.Text = call__12;
                    LPillar.Visible = false;
                    RPillar.Visible = false;
                    MapView.WinHeight = ActivityPnl.WinTop - MapView.WinTop;
                    MigrationRaceLB.CaptureInput = true;
                    MigrationTurnsLB.CaptureInput = true;
                    MigrationRelationLB.CaptureInput = true;
                    break;
                case 2:
                    ResetChecks();
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    LPillar.Visible = false;
                    RPillar.Visible = false;
                    var call__13 = TranslateRStr(InfoRStr);
                    CityBtn1.Text = call__13;
                    var call__14 = TranslateRStr(DoneRStr);
                    CityBtn3.Text = call__14;
                    MigrationRaceLB.CaptureInput = false;
                    MigrationTurnsLB.CaptureInput = false;
                    MigrationRelationLB.CaptureInput = false;
                    MigrationRaceLB.Selected = -1;
                    MigrationTurnsLB.Selected = -1;
                    MigrationRelationLB.Selected = -1;
                    break;
            }
        }

        // AoWCityScreen.pas:892
        public void SortMigration()
        {
            switch (FMigrationSort)
            {
                case 0:
                    SortStringsNumericalAscending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                    break;
                case 1:
                    SortStringsNumericalDescending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                    break;
                case 2:
                    SortStringsAscending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                    break;
                case 3:
                    SortStringsDescending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                    break;
                case 4:
                    SortStringsAscending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                    break;
                case 5:
                    SortStringsDescending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                    break;
            }
        }

        // AoWCityScreen.pas:904
        public int ActivityImage(TCityAction Action)
        {
            int Result = default;
            switch (Action)
            {
                case TCityAction.caTrade:
                    Result = 40;
                    break;
                case TCityAction.caUpgrade:
                    Result = 43;
                    break;
                case TCityAction.caFortify:
                    if (FCity.FWallType == 0)
                    {
                        Result = 41;
                    }
                    else
                    {
                        Result = 42;
                    }

                    break;
                case TCityAction.caMigrate:
                    Result = 44;
                    break;
                case TCityAction.caLoot:
                    Result = 45;
                    break;
                case TCityAction.caRaze:
                    Result = 46;
                    break;
                case TCityAction.caProduce:
                    Result = 4;
                    break;
                case TCityAction.caResearch:
                    Result = 5;
                    break;
                default:
                    Result = 40;
                    break;
            }

            return Result;
        }

        // AoWCityScreen.pas:919
        public void SelectDefaultAction()
        {
            ResetChecks();
            B1CB.Checked = true;
            SelectAction(TCityAction.caTrade);
        }

        // AoWCityScreen.pas:926
        public void SelectProduction(int Index)
        {
            TUnitProduction P = default;
            var call__15 = FCity.FProduction.FUnitProductions.GetItem(Index);
            P = call__15;
            if (P != null)
            {
                FProductionIndex = Index;
                if (P.FResearched)
                {
                    SelectAction(TCityAction.caProduce);
                    AoWHSMap.FTutorialControl.ShowMessage(58, true, true);
                }
                else
                {
                    SelectAction(TCityAction.caResearch);
                }
            }
        }

        // AoWCityScreen.pas:943
        public void SelectMigration(int Index)
        {
            TMigrationItem Item = default;
            bool bool__16 = Index >= 0;
            if (bool__16)
            {
                var call__17 = MigrationRaceLB.Items.Count;
                bool__16 = Index < call__17;
            }

            if (bool__16)
            {
                var call__18 = MigrationRaceLB.Items.Objects[Index];
                Item = (TMigrationItem)call__18;
                MigrationRaceLB.Selected = Index;
                MigrationTurnsLB.Selected = Index;
                MigrationRelationLB.Selected = Index;
                MigrateTurns.Text = IntToStr(Item.FTime);
                var call__19 = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                var call__20 = TranslateRStr(XRelationMinusYRStr);
                var call__21 = AoWE.AoWE.Format(call__20, [call__19, -15]);
                MigrateInfo.Text = call__21;
                MigrateInfo.Text = MigrateInfo.Text + (AnsiString)"   ";
                var call__22 = TranslateRStr(RaceNamesRStr[Item.FRace]);
                var call__23 = TranslateRStr(XRelationPlusYRStr);
                var call__24 = AoWE.AoWE.Format(call__23, [call__22, 10]);
                MigrateInfo.Text = MigrateInfo.Text + call__24;
                CityBtn1.Visible = true;
                var call__25 = TranslateRStr(MigrateRStr);
                CityBtn1.Text = call__25;
            }
            else
            {
                MigrationRaceLB.Selected = -1;
                MigrationTurnsLB.Selected = -1;
                MigrationRelationLB.Selected = -1;
                MigrateInfo.Text = (AnsiString)"";
                MigrateTurns.Text = AnsiString.FromBytes(45);
                UpgradeInfo.Text = (AnsiString)"";
                CityBtn1.Visible = false;
            }
        }

        // AoWCityScreen.pas:972
        public void SelectAction(TCityAction Action)
        {
            TUnit U = default;
            if (FMode != 1)
            {
                SetMode(1);
            }

            FAction = Action;
            UpgradePnl.Visible = false;
            UnitPnl.Visible = false;
            MigratePnl.Visible = false;
            UpgradePic.CurrentImage = 21;
            UpgradeCost.Text = AnsiString.FromBytes(45);
            UpgradeTurns.Text = AnsiString.FromBytes(45);
            UpgradeInfo.Text = (AnsiString)"";
            CityBtn1.Visible = false;
            CityBtn2.Visible = false;
            switch (FAction)
            {
                case TCityAction.caTrade:
                    CityBtn1.Visible = true;
                    var call__26 = TranslateRStr(ProduceMerchandiseRStr);
                    CityBtn1.Text = call__26;
                    var call__27 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__27;
                    var call__28 = TranslateRStr(QueueRStr);
                    CityBtn2.Text = call__28;
                    var call__29 = TranslateRStr(CityTradeRStr);
                    UpgradeMemo.Items.Text = call__29;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 20;
                    var call__30 = TranslateRStr(ProduceMerchandiseRStr);
                    UpgradeLbl.Text = call__30;
                    break;
                case TCityAction.caUpgrade:
                    CityBtn1.Visible = true;
                    var call__31 = TranslateRStr(UpgradeRStr);
                    CityBtn1.Text = call__31;
                    var call__32 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__32;
                    var call__33 = TranslateRStr(QueueRStr);
                    CityBtn2.Text = call__33;
                    var call__34 = TranslateRStr(CityUpgradeRStr);
                    UpgradeMemo.Items.Text = call__34;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 23;
                    var call__35 = FCity.UpgradeCost();
                    UpgradeCost.Text = IntToStr(call__35);
                    var call__36 = FCity.UpgradeTime();
                    UpgradeTurns.Text = IntToStr(call__36);
                    var call__37 = TranslateRStr(UpgradeRStr);
                    UpgradeLbl.Text = call__37;
                    var call__38 = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
                    var call__39 = TranslateRStr(XRelationPlusYRStr);
                    var call__40 = AoWE.AoWE.Format(call__39, [call__38, 5]);
                    UpgradeInfo.Text = call__40;
                    break;
                case TCityAction.caFortify:
                    CityBtn1.Visible = true;
                    var call__41 = TranslateRStr(FortifyRStr);
                    CityBtn1.Text = call__41;
                    var call__42 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !call__42;
                    var call__43 = TranslateRStr(QueueRStr);
                    CityBtn2.Text = call__43;
                    var call__44 = TranslateRStr(CityFortifyRStr);
                    UpgradeMemo.Items.Text = call__44;
                    UpgradePnl.Visible = true;
                    if (FCity.FWallType == 0)
                    {
                        UpgradePic.CurrentImage = 21;
                    }
                    else
                    {
                        UpgradePic.CurrentImage = 22;
                    }

                    var call__45 = FCity.FortifyCost();
                    if (call__45 > 0)
                    {
                        var call__46 = FCity.FortifyCost();
                        UpgradeCost.Text = IntToStr(call__46);
                        var call__47 = FCity.FortifyTime();
                        UpgradeTurns.Text = IntToStr(call__47);
                    }
                    else
                    {
                        UpgradeCost.Text = AnsiString.FromBytes(45);
                        UpgradeTurns.Text = AnsiString.FromBytes(45);
                    }

                    var call__48 = TranslateRStr(FortifyRStr);
                    UpgradeLbl.Text = call__48;
                    var call__49 = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                    var call__50 = TranslateRStr(XRelationPlusYRStr);
                    var call__51 = AoWE.AoWE.Format(call__50, [call__49, 5]);
                    UpgradeInfo.Text = call__51;
                    break;
                case TCityAction.caMigrate:
                    MigratePnl.Visible = true;
                    SelectMigration(0);
                    break;
                case TCityAction.caLoot:
                    CityBtn1.Visible = true;
                    var call__52 = TranslateRStr(LootRStr);
                    CityBtn1.Text = call__52;
                    var call__53 = TranslateRStr(CityLootRStr);
                    UpgradeMemo.Items.Text = call__53;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 25;
                    var call__54 = TranslateRStr(LootRStr);
                    UpgradeLbl.Text = call__54;
                    var call__55 = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                    var call__56 = TranslateRStr(XRelationMinusYRStr);
                    var call__57 = AoWE.AoWE.Format(call__56, [call__55, -30]);
                    UpgradeInfo.Text = call__57;
                    break;
                case TCityAction.caRaze:
                    CityBtn1.Visible = true;
                    var call__58 = TranslateRStr(RazeRStr);
                    CityBtn1.Text = call__58 + (AnsiString)"...";
                    var call__59 = TranslateRStr(CityRazeRStr);
                    UpgradeMemo.Items.Text = call__59;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 26;
                    var call__60 = TranslateRStr(RazeRStr);
                    UpgradeLbl.Text = call__60;
                    var call__61 = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                    var call__62 = TranslateRStr(XRelationMinusYRStr);
                    var call__63 = AoWE.AoWE.Format(call__62, [call__61, -30]);
                    UpgradeInfo.Text = call__63;
                    break;
                case TCityAction.caResearch:
                case TCityAction.caProduce:
                    var call__64 = FCity.FProduction.CreateUnitProduction(FProductionIndex);
                    U = call__64;
                    UnitPnl.Visible = true;
                    var call__65 = U.GetAttack();
                    UAttack.Text = IntToStr(call__65);
                    var call__66 = U.GetDefense();
                    UDefense.Text = IntToStr(call__66);
                    var call__67 = U.GetResistance();
                    UResistance.Text = IntToStr(call__67);
                    var call__68 = U.GetDamage();
                    UDamage.Text = IntToStr(call__68);
                    var call__69 = U.GetHits();
                    UHits.Text = IntToStr(call__69);
                    var call__70 = U.GetMoves();
                    UMoves.Text = IntToStr(call__70);
                    var call__71 = U.GetMoveTypes();
                    var call__72 = General.MTtoInt(call__71);
                    UMovesIcon.CurrentImage = call__72;
                    var call__73 = U.GetAlignment();
                    var call__74 = TranslateRStr(TAlignmentRStr[(int)call__73]);
                    var call__75 = TranslateRStr(AlignmentXRStr);
                    var call__76 = AoWE.AoWE.Format(call__75, [call__74]);
                    UAlignment.Text = call__76;
                    var call__77 = U.GetUnitLevel();
                    var call__78 = TranslateRStr(LevelXRStr);
                    var call__79 = AoWE.AoWE.Format(call__78, [IntToStr(call__77)]);
                    var call__80 = U.GetUpkeep();
                    var call__81 = TranslateRStr(UpkeepXRStr);
                    var call__82 = AoWE.AoWE.Format(call__81, [(int)call__80]);
                    ULevel.Text = call__79 + (AnsiString)" / " + call__82;
                    if (AoWEngine.FDebug)
                    {
                        var call__83 = U.FUnitResource.UnitResourceIndex();
                        var call__84 = GetUnitIndexAIPriority(call__83, FCity.FPlayer, 0);
                        AoWHSMap.ShowMessage((AnsiString)"AI Prio:" + IntToStr(call__84));
                    }

                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (FAction == TCityAction.caProduce)
                    {
                        CityBtn1.Visible = true;
                        var call__85 = TranslateRStr(ProduceRStr);
                        CityBtn1.Text = call__85;
                        var call__86 = FCity.FProduction.GetQueueLocked();
                        CityBtn2.Visible = !call__86;
                        var call__87 = TranslateRStr(QueueRStr);
                        CityBtn2.Text = call__87;
                        var call__88 = U.GetName();
                        var call__89 = TranslateRStr(ProduceXRStr);
                        var call__90 = AoWE.AoWE.Format(call__89, [call__88]);
                        UnitLbl.Text = call__90;
                        var call__91 = FCity.FProduction.UnitProductionGold(FProductionIndex);
                        UCost.Text = IntToStr(call__91);
                        var call__92 = FCity.FProduction.UnitProductionTime(FProductionIndex);
                        UTurns.Text = IntToStr(call__92);
                    }
                    else
                    {
                        CityBtn1.Visible = true;
                        var call__93 = TranslateRStr(InstallRStr);
                        CityBtn1.Text = call__93;
                        var call__94 = FCity.FProduction.GetQueueLocked();
                        CityBtn2.Visible = !call__94;
                        var call__95 = TranslateRStr(QueueRStr);
                        CityBtn2.Text = call__95;
                        var call__96 = U.GetName();
                        var call__97 = TranslateRStr(InstallXRStr);
                        var call__98 = AoWE.AoWE.Format(call__97, [call__96]);
                        UnitLbl.Text = call__98;
                        var call__99 = FCity.FProduction.UnitResearchGold(FProductionIndex);
                        UCost.Text = IntToStr(call__99);
                        var call__100 = FCity.FProduction.UnitResearchTime(FProductionIndex);
                        UTurns.Text = IntToStr(call__100);
                    }

                    U.Release();
                    break;
            }

            PPrev.Visible = false;
            PNext.Visible = false;
        }

        // AoWCityScreen.pas:1099
        public void UpdateInfo()
        {
            AnsiString Reason = default;
            int I = default;
            int J = default;
            var call__101 = FCity.GetName();
            StructName.Text = call__101;
            PopulationHdr.Items.Clear();
            var call__102 = TranslateRStr(PopulationRStr);
            PopulationHdr.Items.Add(call__102 + AnsiString.FromBytes(58));
            var call__103 = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
            PopulationHdr.Items.Add(AnsiString.FromBytes(32) + call__103);
            var call__104 = TranslateRStr(StatusRStr);
            PopulationHdr.Items.Add(call__104 + AnsiString.FromBytes(58));
            var call__105 = FCity.GetCivilStatus();
            var call__106 = TranslateRStr(TCivilStatusRStr[(int)call__105]);
            PopulationHdr.Items.Add(AnsiString.FromBytes(32) + call__106);
            PopulationMemo.Items.Clear();
            FCity.ListCivilStatusInfo(PopulationMemo.Items);
            IncomeHdr.Items.Clear();
            var call__107 = TranslateRStr(IncomeRStr);
            IncomeHdr.Items.Add(call__107);
            var call__108 = TranslateRStr(XGoldRStr);
            var call__109 = AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__108, [IntToStr(FCity.FIncome)]);
            IncomeHdr.Items.Add(call__109);
            IncomeMemo.Items.Clear();
            IncomeGoldMemo.Items.Clear();
            FCity.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
            EnhancementsHdr.Items.Clear();
            var call__110 = TranslateRStr(EnhancementsRStr);
            EnhancementsHdr.Items.Add(call__110);
            EnhancementsMemo.Items.Clear();
            FCity.ListEnhancementsInfo(EnhancementsMemo.Items);
            if (FMigrations != null)
            {
                FMigrations.Free();
                FMigrations = null;
            }

            var call__111 = FCity.CreateMigrationSelection();
            FMigrations = call__111;
            var call__112 = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
            CurrentRace.Text = call__112;
            var call__113 = AoWHSMap.FRaces.GetRace(FCity.FRace);
            var call__114 = call__113.GetPlayerRelation(FCity.FPlayer);
            var call__115 = TranslateRStr(TStatusRStr[(int)call__114]);
            CurrentRelation.Text = call__115;
            MigrationRaceLB.Items.Clear();
            MigrationTurnsLB.Items.Clear();
            MigrationRelationLB.Items.Clear();
            if (FMigrations != null)
            {
                {
                    I = 0;
                    int end__116 = FMigrations.GetCount() - 1;
                    for (; I <= end__116; I += 1)
                    {
                        var call__117 = FMigrations.GetItems(I);
                        var call__118 = TranslateRStr(RaceNamesPluralRStr[call__117.FRace]);
                        var call__119 = FMigrations.GetItems(I);
                        MigrationRaceLB.Items.AddObject(call__118, call__119);
                        var call__120 = FMigrations.GetItems(I);
                        MigrationTurnsLB.Items.Add(IntToStr(call__120.FTime));
                        var call__121 = FMigrations.GetItems(I);
                        var call__122 = TranslateRStr(TStatusRStr[(int)call__121.FRelation]);
                        MigrationRelationLB.Items.Add(call__122);
                    }
                }
            }

            SortMigration();
            QueueLB.Items.Clear();
            {
                I = 0;
                int end__123 = FCity.FProduction.FQueue.GetCount() - 1;
                for (; I <= end__123; I += 1)
                {
                    var call__124 = FCity.FProduction.FQueue.GetItem(I);
                    var call__125 = call__124.GetName();
                    QueueLB.Items.Add(IntToStr(I + 1) + (AnsiString)". " + call__125);
                }
            }

            {
                I = FCity.FProduction.FQueue.GetCount();
                byte end__126 = 4;
                for (; I <= end__126; I += 1)
                {
                    var call__127 = TranslateRStr(EmptyQueueRStr);
                    QueueLB.Items.Add(IntToStr(I + 1) + (AnsiString)". " + call__127);
                }
            }

            var call__128 = FCity.FProduction.FQueue.GetCount();
            if (QueueLB.Selected >= call__128)
            {
                var call__129 = FCity.FProduction.FQueue.GetCount();
                QueueLB.Selected = call__129 - 1;
                FQueueIndex = QueueLB.Selected;
            }

            LoopQueueBox.Checked = FCity.FProduction.FLoopQueue;
            var call__130 = FCity.GetActiveAction();
            if (call__130 != TCityAction.caNone)
            {
                var call__131 = FCity.FProduction.FQueue.GetItem(0);
                var call__132 = call__131.GetNameProgressive();
                ActiveAction.Text = call__132;
                if (FCity.FProduction.FSettings.FTurnsLeft != -1)
                {
                    var call__133 = FmtTurnsLeftStr(FCity.FProduction.FSettings.FTurnsLeft);
                    ActiveActionTurns.Text = call__133;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurns.TextColor = 6783862;
                    this.ActiveActionTurnsBar.Min = 0;
                    this.ActiveActionTurnsBar.Max = FCity.FProduction.FSettings.FTurnsTotal;
                    this.ActiveActionTurnsBar.Position = FCity.FProduction.FSettings.FTurnsTotal - FCity.FProduction.FSettings.FTurnsLeft;

                    ActiveActionTurnsBar.Visible = true;
                }
                else
                {
                    ActiveActionTurns.Visible = false;
                    ActiveActionTurnsBar.Visible = false;
                }

                var call__135 = FCity.GetActiveAction();
                var call__136 = ActivityImage(call__135);
                CurrentActionImage.CurrentImage = call__136;
            }
            else
            {
                var call__137 = FCity.FProduction.FQueue.GetCount();
                if (call__137 == 0)
                {
                    var call__138 = TranslateRStr(ProducingMerchandiseRStr);
                    ActiveAction.Text = call__138;
                    var call__139 = TranslateRStr(DefaultRStr);
                    ActiveActionTurns.Text = AnsiString.FromBytes(40) + LowerCase(call__139) + AnsiString.FromBytes(41);
                    ActiveActionTurns.TextColor = 6783862;
                    ActiveActionTurnsBar.Visible = false;
                    var call__140 = FCity.GetActiveAction();
                    var call__141 = ActivityImage(call__140);
                    CurrentActionImage.CurrentImage = call__141;
                }
                else
                {
                    Reason = (AnsiString)"";
                    var call__142 = FCity.FProduction.FQueue.GetItem(0);
                    var call__143 = call__142.GetNameProgressive();
                    ActiveAction.Text = call__143;
                    var call__144 = FCity.FProduction.FQueue.GetItem(0);
                    FCity.FProduction.CanExecuteProduction(call__144, ref Reason);
                    ActiveActionTurns.Text = Reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurnsBar.Visible = false;
                    var call__145 = FCity.FProduction.FQueue.GetItem(0);
                    var call__146 = ProductionIDToCityAction(call__145.FProductionID);
                    var call__147 = ActivityImage(call__146);
                    CurrentActionImage.CurrentImage = call__147;
                }
            }

            if (FMode == 1 && FAction != TCityAction.caNone)
            {
                SelectAction(FAction);
            }

            Dlg.FDrawn = false;
        }

        // AoWCityScreen.pas:1200
        public void ProductionEvent(TCityProductionEventLog Event)
        {
            var call__148 = Event.GetCity();
            ShowCity(call__148);
            SetMode(1);
            SelectDefaultAction();
        }

        // AoWCityScreen.pas:1207
        public void CityChanged(TObject Sender)
        {
            if (Dlg.Visible && Sender == FCity)
            {
                if (FCity.FPlayer == AoWHSMap.FSeatedPlayer)
                {
                    UpdateInfo();
                }
                else
                {
                    Dlg.Visible = false;
                }
            }
        }

        // AoWCityScreen.pas:245
        public void ShowCity(TCity City)
        {
            int I = default;
            int J = default;
            int Direction = default;
            TArmyHS Army = default;
            TArmyHSList Armies = default;
            TRndTacHsm Generator = default;
            Reset();
            FCity = City;
            UpdateInfo();
            SetMode(0);
            FreePreview();
            {
                I = 0;
                int end__149 = FCity.FProduction.FUnitProductions.GetCount() - 1;
                for (; I <= end__149; I += 1)
                {
                    var call__150 = FCity.FProduction.FUnitProductions.GetItem(I);
                    LoadUnitImages(call__150.FUnitIndex);
                }
            }

            Generator = new TRndTacHsm();
            var call__151 = FCity.GetLhx();
            var call__152 = FCity.GetYhx();
            var call__153 = FCity.GetXhx();
            var call__154 = Generator.RandomMap(null, call__153, call__152, call__151);
            FPreview = call__154;
            Generator.Free();
            Armies = new TArmyHSList(null);
            FCity.ListArmies(Armies);
            {
                I = 0;
                int end__155 = Armies.GetCount() - 1;
                for (; I <= end__155; I += 1)
                {
                    Army = Armies.GetArmyHS(I);
                    var call__156 = Army.GetYhx();
                    var call__157 = Army.GetXhx();
                    var call__158 = FCity.GetYhx();
                    var call__159 = FCity.GetXhx();
                    var call__160 = dHXtoHN(call__159, call__158, call__157, call__156);
                    Direction = call__160;
                    {
                        J = 0;
                        int end__161 = Army.FArmy.GetCount() - 1;
                        for (; J <= end__161; J += 1)
                        {
                            var call__162 = Army.FArmy.GetUnit(J);
                            PlaceOverviewUnit(FPreview, call__162, Direction, J);
                        }
                    }
                }
            }

            Armies.Free();
            var mapObject = FPreview.FCenterViewLocation;
            if (FPreview.FCenterViewLocation != null)
            {
                var call__164 = mapObject.GetYhx();
                var call__165 = mapObject.GetXhx();
                HXtoHP(call__165, call__164, ref FViewX, ref FViewY);
                FViewX = FViewX + 16 - MapView.WinWidth / 2;
                FViewY = FViewY + 16 - MapView.WinHeight / 2;
            }
            else
            {
                FViewX = 800;
                FViewY = 800;
            }

            AoWHSMap.FTutorialControl.ShowMessage(56, true, true);
            General.Manager.OverlayPriority = 49;
            Dlg.Visible = true;
        }

        // AoWCityScreen.pas:292
        public void CityScreenCreate()
        {
            var call__166 = new TIntegerList();
            FImages = call__166;
        }

        // AoWCityScreen.pas:297
        public void CityScreenDestroy()
        {
            FImages.Free();
        }

        // AoWCityScreen.pas:302
        public void ActivityChange(TObject Sender)
        {
            General.PlayButtonSound(1);
            if (((TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectAction((TCityAction)((byte)((TAOWCheckBox)Sender).Tag + 1));
                ((TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWCityScreen.pas:313
        public void ActivityDoubleClick(TObject Sender)
        {
            if ((TCityAction)((byte)((TAOWCheckBox)Sender).Tag + 1) != TCityAction.caMigrate)
            {
                ResetChecks();
                SelectAction((TCityAction)((byte)((TAOWCheckBox)Sender).Tag + 1));
                ((TAOWCheckBox)Sender).Checked = true;
                CityBtn1Click(null);
            }
        }

        // AoWCityScreen.pas:324
        public void ProductionChange(TObject Sender)
        {
            General.PlayButtonSound(1);
            if (((TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectProduction(((TAOWCheckBox)Sender).Tag);
                ((TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWCityScreen.pas:335
        public void ProductionDoubleClick(TObject Sender)
        {
            ResetChecks();
            SelectProduction(((TAOWCheckBox)Sender).Tag);
            ((TAOWCheckBox)Sender).Checked = true;
            CityBtn1Click(null);
        }

        // AoWCityScreen.pas:343
        public void ProductionsUpdate(TObject Sender)
        {
            var call__167 = FCity.FProduction.FUnitProductions.GetItem(((TAOWCheckBox)Sender).Tag);
            ((TAOWCheckBox)Sender).CaptureInput = call__167 != null;
        }

        // AoWCityScreen.pas:348
        public void ActivityPreDraw(TAoWComponent Sender, TSurfaceDesc SD)
        {
            int Image = default;
            var call__168 = ActivityImage((TCityAction)((byte)Sender.Tag + 1));
            Image = call__168;
            var call__169 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[Image];
            call__169.Show(ActivityPnl2.WinLeft + Sender.WinLeft + 2, ActivityPnl2.WinTop + Sender.WinTop + 2, SD);
        }

        // AoWCityScreen.pas:354
        public void CityBtn1Click(TObject Sender)
        {
            AnsiString Reason = default;
            sbyte Race = default;
            switch (FMode)
            {
                case 0:
                    SetMode(1);
                    SelectDefaultAction();
                    AoWHSMap.FTutorialControl.ShowMessage(57, true, true);
                    break;
                case 1:
                    switch (FAction)
                    {
                        case TCityAction.caTrade:
                            var call__170 = FCity.CanTrade(false, ref Reason);
                            if (call__170)
                            {
                                FCity.Trade(false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caLoot:
                            var call__171 = FCity.CanLoot(ref Reason);
                            if (call__171)
                            {
                                FCity.Loot();
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caRaze:
                            var call__172 = FCity.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason);
                            if (call__172)
                            {
                                FCity.Raze(AoWHSMap.FSeatedPlayer);
                                Dlg.Visible = false;
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caUpgrade:
                            var call__173 = FCity.CanUpgrade(false, ref Reason);
                            if (call__173)
                            {
                                FCity.Upgrade(false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caFortify:
                            var call__174 = FCity.CanFortify(false, ref Reason);
                            if (call__174)
                            {
                                FCity.Fortify(false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caProduce:
                            var call__175 = FCity.CanProduce(FProductionIndex, false, ref Reason);
                            if (call__175)
                            {
                                AoWHSMap.FTutorialControl.ShowMessage(59, true, true);
                                FCity.Produce(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caResearch:
                            var call__176 = FCity.CanResearch(FProductionIndex, false, ref Reason);
                            if (call__176)
                            {
                                FCity.Research(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caMigrate:
                            var call__177 = MigrationRaceLB.Items.Objects[MigrationRaceLB.Selected];
                            Race = ((TMigrationItem)call__177).FRace;
                            var call__178 = FCity.CanMigrate(Race, ref Reason);
                            if (call__178)
                            {
                                AoWHSMap.FTutorialControl.ShowMessage(66, true, true);
                                FCity.Migrate(Race);
                                SetMode(0);
                            }

                            break;
                    }

                    break;
                case 2:
                    if (FQueueIndex != -1)
                    {
                        FCity.FProduction.DeleteProduction(FQueueIndex);
                    }

                    break;
            }
        }

        // AoWCityScreen.pas:437
        public void CityBtn2Click()
        {
            AnsiString Reason = default;
            switch (FMode)
            {
                case 1:
                    switch (FAction)
                    {
                        case TCityAction.caTrade:
                            var call__179 = FCity.CanTrade(true, ref Reason);
                            if (call__179)
                            {
                                FCity.Trade(true);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caUpgrade:
                            var call__180 = FCity.CanUpgrade(true, ref Reason);
                            if (call__180)
                            {
                                FCity.Upgrade(true);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caFortify:
                            var call__181 = FCity.CanFortify(true, ref Reason);
                            if (call__181)
                            {
                                FCity.Fortify(true);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caProduce:
                            var call__182 = FCity.CanProduce(FProductionIndex, true, ref Reason);
                            if (call__182)
                            {
                                FCity.Produce(FProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case TCityAction.caResearch:
                            var call__183 = FCity.CanResearch(FProductionIndex, true, ref Reason);
                            if (call__183)
                            {
                                FCity.Research(FProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                    }

                    break;
                case 2:
                    if (FQueueIndex == 0)
                    {
                        var call__184 = FCity.FProduction.RetryProductionExecution(ref Reason);
                        if (call__184)
                        {
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }
                    }

                    break;
            }
        }

        // AoWCityScreen.pas:465
        public void CityBtn3Click()
        {
            General.PlayButtonSound(0);
            if (FMode == 0)
            {
                Dlg.Visible = false;
            }
            else
            {
                SetMode(0);
            }

            AoWHSMap.FTutorialControl.ShowMessage(60, true, true);
        }

        // AoWCityScreen.pas:472
        public void PreDrawProductions(TObject Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            TP2UnitShowInfo Info = default;
            TUnit U = default;
            TUnitProduction P = default;
            var call__185 = Rect(ActivityPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, ActivityPnl.WinTop + ((TAoWComponent)Sender).WinTop, ActivityPnl2.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, ActivityPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
            R = call__185;
            // { Original uses two different panels. }
            var call__186 = FCity.FProduction.FUnitProductions.GetItem(((TAoWComponent)Sender).Tag);
            P = call__186;
            if (P != null)
            {
                var call__187 = FCity.FProduction.CreateUnitProduction(((TAoWComponent)Sender).Tag);
                U = call__187;
                Info.Direction = 3;
                Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                Info.Animation = 0;
                Info.Frame = 0;
                Info.Looped = false;
                if (!P.FResearched)
                {
                    var call__188 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__188.Show(R.Left + 2, R.Top + 2, SD);
                    Info.Direction = 5;
                    if (U != null)
                    {
                        U.ShowEx(R.Left - 4, R.Top, SD, Info);
                    }

                    var call__189 = AlphaBlendTable.GetLookupTables(45);
                    var call__190 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                    call__190.ShowBlended(R.Left + 2, R.Top + 2, SD, call__189);
                }
                else
                {
                    var call__191 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                    call__191.Show(R.Left + 2, R.Top + 2, SD);
                    if (U != null)
                    {
                        U.ShowEx(R.Left - 4, R.Top, SD, Info);
                    }
                }

                if (U != null)
                {
                    U.Release();
                }
            }
            else
            {
                var call__192 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                call__192.Show(R.Left + 2, R.Top + 2, SD);
            }
        }

        // AoWCityScreen.pas:508
        public void SortMigrationRacesClick()
        {
            if (FMigrationSort == 2)
            {
                FMigrationSort = 3;
            }
            else
            {
                FMigrationSort = 2;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:515
        public void SortMigrationTurnsClick()
        {
            if (FMigrationSort == 0)
            {
                FMigrationSort = 1;
            }
            else
            {
                FMigrationSort = 0;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:522
        public void SortMigrationRelationClick()
        {
            if (FMigrationSort == 4)
            {
                FMigrationSort = 5;
            }
            else
            {
                FMigrationSort = 4;
            }

            SortMigration();
        }

        // AoWCityScreen.pas:529
        public void MigrationChange(TAOWListBox Sender)
        {
            if (FMode != 2)
            {
                SelectMigration(Sender.Selected);
            }
        }

        // AoWCityScreen.pas:534
        public void MigrationSBChange(TObject Sender)
        {
            MigrationRaceLB.ListOffset = ((TAOWVScrollBar)Sender).Position;
            MigrationTurnsLB.ListOffset = ((TAOWVScrollBar)Sender).Position;
            MigrationRelationLB.ListOffset = ((TAOWVScrollBar)Sender).Position;
        }

        // AoWCityScreen.pas:541
        public void UFaceDraw(TObject Sender, TSurfaceDesc SD)
        {
            TUnit U = default;
            TRect R = default;
            var call__193 = FCity.FProduction.CreateUnitProduction(FProductionIndex);
            U = call__193;
            if (U != null)
            {
                var call__194 = Rect(UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + 2, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + 2, UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth - 4, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight - 2);
                R = call__194;
                U.ShowFace(R, SD);
                U.Release();
            }
        }

        // AoWCityScreen.pas:558
        public void QueueLBChange(TAOWListBox Sender)
        {
            TCityAction Action = default;
            var call__195 = FCity.FProduction.FQueue.GetCount();
            if (QueueLB.Selected < call__195 && FMode != 0)
            {
                if (QueueLB.Selected != -1)
                {
                    FQueueIndex = QueueLB.Selected;
                    var call__196 = FCity.FProduction.FQueue.GetItem(FQueueIndex);
                    var call__197 = ProductionIDToCityAction(call__196.FProductionID);
                    Action = call__197;
                    if (Action == TCityAction.caProduce)
                    {
                        var call__198 = FCity.FProduction.FQueue.GetItem(FQueueIndex);
                        FProductionIndex = call__198.FParameter;
                        bool bool__199 = FProductionIndex < 0;
                        if (!bool__199)
                        {
                            var call__200 = FCity.FProduction.FUnitProductions.GetCount();
                            bool__199 = FProductionIndex >= call__200;
                        }

                        if (bool__199)
                        {
                            FProductionIndex = 0;
                        }
                    }

                    SelectAction(Action);
                    SetMode(2);
                    QueueLB.Selected = FQueueIndex;
                    CityBtn1.Visible = true;
                    var call__201 = TranslateRStr(DeleteRStr);
                    CityBtn1.Text = call__201;
                    if (FQueueIndex == 0 && FCity.FProduction.FStatus == TProductionControlStatus.pcsWaiting)
                    {
                        CityBtn2.Visible = true;
                        var call__202 = TranslateRStr(ExecuteRStr);
                        CityBtn2.Text = call__202;
                    }
                    else
                    {
                        CityBtn2.Visible = false;
                    }

                    PPrev.Visible = false;
                    PNext.Visible = false;
                }
                else
                {
                    SetMode(1);
                    SelectDefaultAction();
                }
            }
            else
            {
                if (FMode == 2)
                {
                    Sender.Selected = FQueueIndex;
                }
                else
                {
                    Sender.Selected = -1;
                }
            }
        }

        // AoWCityScreen.pas:597
        public void CurrentActionImageDraw(TObject Sender, TSurfaceDesc SD)
        {
            TP2UnitShowInfo Info = default;
            TRect R = default;
            TUnit U = default;
            var call__203 = FCity.FProduction.FQueue.GetCount();
            if (call__203 != 0)
            {
                var call__204 = FCity.FProduction.FQueue.GetItem(0);
                switch (call__204.FProductionID)
                {
                    case 1:
                    case 2:
                        var call__205 = Rect(ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop, ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
                        R = call__205;
                        var call__206 = FCity.FProduction.FQueue.GetItem(0);
                        var call__207 = FCity.FProduction.CreateUnitProduction(call__206.FParameter);
                        U = call__207;
                        if (U != null)
                        {
                            Info.Direction = 3;
                            Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                            Info.Animation = 0;
                            Info.Frame = 0;
                            Info.Looped = false;
                            var call__208 = FCity.FProduction.FQueue.GetItem(0);
                            var call__209 = FCity.FProduction.FUnitProductions.GetItem(call__208.FParameter);
                            if (!call__209.FResearched)
                            {
                                Info.Direction = 5;
                            }

                            U.ShowEx(R.Left - 8, R.Top - 5, SD, Info);
                            U.Release();
                        }

                        break;
                }
            }
        }

        // AoWCityScreen.pas:626
        public void LoopQueueBoxChange()
        {
            FCity.FProduction.SetLoopQueueTo(LoopQueueBox.Checked);
        }

        // AoWCityScreen.pas:631
        public void RacePortraitDraw(TObject Sender, TSurfaceDesc SD)
        {
            TRect R = default;
            var call__210 = Rect(IPPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, IPPnl.WinTop + ((TAoWComponent)Sender).WinTop, IPPnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, IPPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
            R = call__210;
            var call__211 = AoWHSSet.FRaceResources.GetRaceResource(FCity.FRace);
            var call__212 = call__211.GetFace();
            call__212.ShowStretched(R, SD);
        }

        // AoWCityScreen.pas:641
        public void MapViewDraw(TObject Sender, TSurfaceDesc SD)
        {
            TSurfaceDesc ViewSD = default;
            TRect R = default;
            int W = default;
            int H = default;
            int Width = default;
            if (FPreview != null)
            {
                var call__213 = Rect(Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((TAoWComponent)Sender).WinTop, Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, Dlg.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
                R = call__213;
                Width = R.Right - R.Left;
                W = Width;
                H = R.Bottom - R.Top;
                var call__214 = FPreview.GetMaxXhp();
                if (FViewX + W + 16 > call__214)
                {
                    var call__215 = FPreview.GetMaxXhp();
                    FViewX = call__215 - W - 16;
                }

                var call__216 = FPreview.GetMaxYhp();
                if (FViewY + H + 16 > call__216)
                {
                    var call__217 = FPreview.GetMaxYhp();
                    FViewY = call__217 - H - 16;
                }

                if (FViewX < 16)
                {
                    FViewX = 16;
                }

                if (FViewY < 16)
                {
                    FViewY = 16;
                }

                CreateSDfromSD(SD, R, ref ViewSD);
                FPreview.SetExplorationEnabled(false);
                FPreview.SetFogEnabled(false);
                FPreview.SetViewPort(FViewX, FViewY, 0, Width, R.Bottom - R.Top);
                FPreview.UpdateStaticScene();
                FPreview.UpdateDynamicScene();
                FPreview.DrawStaticScene(ref ViewSD);
                FPreview.DrawDynamicScene(ref ViewSD);
            }
        }

        // AoWCityScreen.pas:668
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            switch (Key)
            {
                case 37:
                    FViewX -= 16;
                    MapView.FDrawn = false;
                    break;
                case 39:
                    FViewX += 16;
                    MapView.FDrawn = false;
                    break;
                case 38:
                    FViewY -= 16;
                    MapView.FDrawn = false;
                    break;
                case 40:
                    FViewY += 16;
                    MapView.FDrawn = false;
                    break;
                case 27:
                    Dlg.Visible = false;
                    break;
            }
        }

        // AoWCityScreen.pas:679
        public void MapViewUpdate()
        {
            TAoWCombatMap Preview = default;
            Preview = FPreview;
            if (Preview != null)
            {
                Preview.NewFrame();
            }

            MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Frame & 1) != 0;
        }

        // AoWCityScreen.pas:688
        public void DlgHide()
        {
            CloseCity();
        }

        // AoWCityScreen.pas:693
        public void UAbilitiesLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility A = default;
            if (Button == TMouseButton.mbRight && UAbilitiesLB.Selected > -1)
            {
                var call__218 = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
                var call__219 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__218));
                A = call__219;
                if (A != null)
                {
                    var call__220 = A.GetName(null);
                    SetInfoBanner(AoWInfoBanner.AoWInfoBanner.InfoBanner, call__220, A.FDescription);
                }
            }
        }

        // AoWCityScreen.pas:704
        public void PPrevClick()
        {
            int Start = default;
            int Index = default;
            TPlayerStructure S = default;
            var call__221 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__222 = call__221.FStructures.FindChildIndex(FCity);
            Start = call__222;
            Index = Start;
            Index -= 1;
            if (Index < 0)
            {
                var call__223 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__224 = call__223.FStructures.GetCount();
                Index = call__224 - 1;
            }

            while (Index != Start)
            {
                var call__225 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__226 = call__225.FStructures.GetStructure(Index);
                S = call__226;
                if (S is TCity)
                {
                    Index = Start + 1;
                    ShowCity((TCity)S);
                }

                Index += -1;
                if (Index < 0)
                {
                    var call__227 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__228 = call__227.FStructures.GetCount();
                    Index = call__228 - 1;
                }
            }
        }

        // AoWCityScreen.pas:726
        public void PNextClick()
        {
            int Start = default;
            int Index = default;
            TPlayerStructure S = default;
            var call__229 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__230 = call__229.FStructures.FindChildIndex(FCity);
            Start = call__230;
            Index = Start + 1;
            var call__231 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__232 = call__231.FStructures.GetCount();
            if (Index >= call__232)
            {
                Index = 0;
            }

            while (Index != Start)
            {
                var call__233 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__234 = call__233.FStructures.GetStructure(Index);
                S = call__234;
                if (S is TCity)
                {
                    Index = Start - 1;
                    ShowCity((TCity)S);
                }

                Index += 1;
                var call__235 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__236 = call__235.FStructures.GetCount();
                if (Index >= call__236)
                {
                    Index = 0;
                }
            }
        }
    }

    public static partial class AoWCityScreen
    {
        public static TCityScreen CityScreen;
    }
}
