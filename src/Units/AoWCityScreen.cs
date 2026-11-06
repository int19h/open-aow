// Translated from AoWCityScreen.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;
using Aow.Units.Engine;
using static Aow.Units.Engine.EngineUnit;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;
using Aow.Units.City;
using static Aow.Units.City.CityUnit;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWEUnit;
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
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.OverviewUnit.OverviewUnitUnit;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWInfoBanner;

namespace Aow.Units.AoWCityScreen;

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
        for (int I = FImages.Count - 1; I >= 0; I--)
        {
            var image = FImages[I];
            AoWHSSet.FUnitGFXResources.GetUnitGFXResource(image).Release();
        }

        FImages.Clear();
    }

    // AoWCityScreen.pas:766
    public void LoadUnitImages(int Index)
    {
        int Gfx;
        Gfx = AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitGFXResourceIndex;
        AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx).AddRef();
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
        var options = TranslateRStr(OptionsRStr);
        CityBtn1.Text = options;
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
        var queue = TranslateRStr(QueueRStr);
        QueueLbl.Text = queue;
        var repeat = TranslateRStr(RepeatRStr);
        RepeatLbl.Text = repeat;
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
                var change = TranslateRStr(ChangeRStr);
                CityBtn1.Text = change;
                CityBtn1.Visible = true;
                CityBtn2.Visible = false;
                PPrev.Visible = !CityBtn2.Visible;
                PNext.Visible = !CityBtn2.Visible;
                var exit = TranslateRStr(ExitRStr);
                CityBtn3.Text = exit;
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
                var info = TranslateRStr(InfoRStr);
                CityBtn1.Text = info;
                var done = TranslateRStr(DoneRStr);
                CityBtn3.Text = done;
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
                var infoText = TranslateRStr(InfoRStr);
                CityBtn1.Text = infoText;
                var doneText = TranslateRStr(DoneRStr);
                CityBtn3.Text = doneText;
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
        int Result;
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
        TUnitProduction P;
        P = FCity.FProduction.FUnitProductions.GetItem(Index);
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
        TMigrationItem Item;
        if (Index >= 0 && Index < MigrationRaceLB.Items.Count)
        {
            Item = (TMigrationItem)MigrationRaceLB.Items.Objects[Index];
            MigrationRaceLB.Selected = Index;
            MigrationTurnsLB.Selected = Index;
            MigrationRelationLB.Selected = Index;
            MigrateTurns.Text = IntToStr(Item.FTime);
            var raceNames = TranslateRStr(RaceNamesRStr[FCity.FRace]);
            var format = AoWEUnit.Format(TranslateRStr(XRelationMinusYRStr), [raceNames, -15]);
            MigrateInfo.Text = format;
            MigrateInfo.Text = MigrateInfo.Text + "   ";
            var raceNamesText = TranslateRStr(RaceNamesRStr[Item.FRace]);
            var aoWEFormat = AoWEUnit.Format(TranslateRStr(XRelationPlusYRStr), [raceNamesText, 10]);
            MigrateInfo.Text = MigrateInfo.Text + aoWEFormat;
            CityBtn1.Visible = true;
            var migrate = TranslateRStr(MigrateRStr);
            CityBtn1.Text = migrate;
        }
        else
        {
            MigrationRaceLB.Selected = -1;
            MigrationTurnsLB.Selected = -1;
            MigrationRelationLB.Selected = -1;
            MigrateInfo.Text = "";
            MigrateTurns.Text = "-";
            UpgradeInfo.Text = "";
            CityBtn1.Visible = false;
        }
    }

    // AoWCityScreen.pas:972
    public void SelectAction(TCityAction Action)
    {
        TUnit U;
        if (FMode != 1)
        {
            SetMode(1);
        }

        FAction = Action;
        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        MigratePnl.Visible = false;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        UpgradeInfo.Text = "";
        CityBtn1.Visible = false;
        CityBtn2.Visible = false;
        switch (FAction)
        {
            case TCityAction.caTrade:
                CityBtn1.Visible = true;
                var produceMerchandise = TranslateRStr(ProduceMerchandiseRStr);
                CityBtn1.Text = produceMerchandise;
                var queueLocked = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !queueLocked;
                var queue = TranslateRStr(QueueRStr);
                CityBtn2.Text = queue;
                var cityTrade = TranslateRStr(CityTradeRStr);
                UpgradeMemo.Items.Text = cityTrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 20;
                var produceMerchandiseText = TranslateRStr(ProduceMerchandiseRStr);
                UpgradeLbl.Text = produceMerchandiseText;
                break;
            case TCityAction.caUpgrade:
                CityBtn1.Visible = true;
                var upgrade = TranslateRStr(UpgradeRStr);
                CityBtn1.Text = upgrade;
                var fProductionQueueLocked = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !fProductionQueueLocked;
                var queueText = TranslateRStr(QueueRStr);
                CityBtn2.Text = queueText;
                var cityUpgrade = TranslateRStr(CityUpgradeRStr);
                UpgradeMemo.Items.Text = cityUpgrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 23;
                var upgradeCost = FCity.UpgradeCost();
                UpgradeCost.Text = IntToStr(upgradeCost);
                var upgradeTime = FCity.UpgradeTime();
                UpgradeTurns.Text = IntToStr(upgradeTime);
                var upgradeText = TranslateRStr(UpgradeRStr);
                UpgradeLbl.Text = upgradeText;
                var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
                var format = AoWEUnit.Format(TranslateRStr(XRelationPlusYRStr), [raceNamesPlural, 5]);
                UpgradeInfo.Text = format;
                break;
            case TCityAction.caFortify:
                CityBtn1.Visible = true;
                var fortify = TranslateRStr(FortifyRStr);
                CityBtn1.Text = fortify;
                var queueLocked2 = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !queueLocked2;
                var queue3 = TranslateRStr(QueueRStr);
                CityBtn2.Text = queue3;
                var cityFortify = TranslateRStr(CityFortifyRStr);
                UpgradeMemo.Items.Text = cityFortify;
                UpgradePnl.Visible = true;
                if (FCity.FWallType == 0)
                {
                    UpgradePic.CurrentImage = 21;
                }
                else
                {
                    UpgradePic.CurrentImage = 22;
                }

                if (FCity.FortifyCost() > 0)
                {
                    var fortifyCost = FCity.FortifyCost();
                    UpgradeCost.Text = IntToStr(fortifyCost);
                    var fortifyTime = FCity.FortifyTime();
                    UpgradeTurns.Text = IntToStr(fortifyTime);
                }
                else
                {
                    UpgradeCost.Text = "-";
                    UpgradeTurns.Text = "-";
                }

                var fortifyText = TranslateRStr(FortifyRStr);
                UpgradeLbl.Text = fortifyText;
                var raceNames = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                var aoWEFormat = AoWEUnit.Format(TranslateRStr(XRelationPlusYRStr), [raceNames, 5]);
                UpgradeInfo.Text = aoWEFormat;
                break;
            case TCityAction.caMigrate:
                MigratePnl.Visible = true;
                SelectMigration(0);
                break;
            case TCityAction.caLoot:
                CityBtn1.Visible = true;
                var loot = TranslateRStr(LootRStr);
                CityBtn1.Text = loot;
                var cityLoot = TranslateRStr(CityLootRStr);
                UpgradeMemo.Items.Text = cityLoot;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 25;
                var lootText = TranslateRStr(LootRStr);
                UpgradeLbl.Text = lootText;
                var raceNamesText = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                var format2 = AoWEUnit.Format(TranslateRStr(XRelationMinusYRStr), [raceNamesText, -30]);
                UpgradeInfo.Text = format2;
                break;
            case TCityAction.caRaze:
                CityBtn1.Visible = true;
                var raze = TranslateRStr(RazeRStr);
                CityBtn1.Text = raze + "...";
                var cityRaze = TranslateRStr(CityRazeRStr);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = TranslateRStr(RazeRStr);
                UpgradeLbl.Text = razeText;
                var raceNames3 = TranslateRStr(RaceNamesRStr[FCity.FRace]);
                var format3 = AoWEUnit.Format(TranslateRStr(XRelationMinusYRStr), [raceNames3, -30]);
                UpgradeInfo.Text = format3;
                break;
            case TCityAction.caResearch:
            case TCityAction.caProduce:
                U = FCity.FProduction.CreateUnitProduction(FProductionIndex);
                UnitPnl.Visible = true;
                var attack = U.GetAttack();
                UAttack.Text = IntToStr(attack);
                var defense = U.GetDefense();
                UDefense.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                UResistance.Text = IntToStr(resistance);
                var damage = U.GetDamage();
                UDamage.Text = IntToStr(damage);
                var hits = U.GetHits();
                UHits.Text = IntToStr(hits);
                var moves = U.GetMoves();
                UMoves.Text = IntToStr(moves);
                var moveTypes = U.GetMoveTypes();
                var mTtoInt = General.MTtoInt(moveTypes);
                UMovesIcon.CurrentImage = mTtoInt;
                var alignment = U.GetAlignment();
                var alignmentText = TranslateRStr(TAlignmentRStr[(int)alignment]);
                var format4 = AoWEUnit.Format(TranslateRStr(AlignmentXRStr), [alignmentText]);
                UAlignment.Text = format4;
                var unitLevel = U.GetUnitLevel();
                var format5 = AoWEUnit.Format(TranslateRStr(LevelXRStr), [IntToStr(unitLevel)]);
                var upkeep = U.GetUpkeep();
                var format6 = AoWEUnit.Format(TranslateRStr(UpkeepXRStr), [(int)upkeep]);
                ULevel.Text = format5 + " / " + format6;
                if (AoWEngine.FDebug)
                {
                    var unitIndexAIPriority = GetUnitIndexAIPriority(U.FUnitResource.UnitResourceIndex(), FCity.FPlayer, 0);
                    AoWHSMap.ShowMessage("AI Prio:" + IntToStr(unitIndexAIPriority));
                }

                UAbilitiesLB.Items.Clear();
                U.ListAbilities(UAbilitiesLB.Items);
                if (FAction == TCityAction.caProduce)
                {
                    CityBtn1.Visible = true;
                    var produce = TranslateRStr(ProduceRStr);
                    CityBtn1.Text = produce;
                    var queueLocked3 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = TranslateRStr(QueueRStr);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AoWEUnit.Format(TranslateRStr(ProduceXRStr), [name]);
                    UnitLbl.Text = format7;
                    var unitProductionGold = FCity.FProduction.UnitProductionGold(FProductionIndex);
                    UCost.Text = IntToStr(unitProductionGold);
                    var unitProductionTime = FCity.FProduction.UnitProductionTime(FProductionIndex);
                    UTurns.Text = IntToStr(unitProductionTime);
                }
                else
                {
                    CityBtn1.Visible = true;
                    var install = TranslateRStr(InstallRStr);
                    CityBtn1.Text = install;
                    var queueLocked3 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = TranslateRStr(QueueRStr);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AoWEUnit.Format(TranslateRStr(InstallXRStr), [name]);
                    UnitLbl.Text = format7;
                    var unitResearchGold = FCity.FProduction.UnitResearchGold(FProductionIndex);
                    UCost.Text = IntToStr(unitResearchGold);
                    var unitResearchTime = FCity.FProduction.UnitResearchTime(FProductionIndex);
                    UTurns.Text = IntToStr(unitResearchTime);
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
        string Reason;
        var name = FCity.GetName();
        StructName.Text = name;
        PopulationHdr.Items.Clear();
        var population = TranslateRStr(PopulationRStr);
        PopulationHdr.Items.Add(population + ":");
        var raceNamesPlural = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
        PopulationHdr.Items.Add(" " + raceNamesPlural);
        var status = TranslateRStr(StatusRStr);
        PopulationHdr.Items.Add(status + ":");
        var civilStatus = FCity.GetCivilStatus();
        var civilStatusText = TranslateRStr(TCivilStatusRStr[(int)civilStatus]);
        PopulationHdr.Items.Add(" " + civilStatusText);
        PopulationMemo.Items.Clear();
        FCity.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHdr.Items.Clear();
        var income = TranslateRStr(IncomeRStr);
        IncomeHdr.Items.Add(income);
        var xGold = TranslateRStr(XGoldRStr);
        var format = AoWEUnit.Format(" " + xGold, [IntToStr(FCity.FIncome)]);
        IncomeHdr.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        FCity.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
        EnhancementsHdr.Items.Clear();
        var enhancements = TranslateRStr(EnhancementsRStr);
        EnhancementsHdr.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        FCity.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (FMigrations != null)
        {
            FMigrations.Free();
            FMigrations = null;
        }

        FMigrations = FCity.CreateMigrationSelection();
        var raceNamesPluralText = TranslateRStr(RaceNamesPluralRStr[FCity.FRace]);
        CurrentRace.Text = raceNamesPluralText;
        var playerRelation = AoWHSMap.FRaces.GetRace(FCity.FRace).GetPlayerRelation(FCity.FPlayer);
        var statusText = TranslateRStr(TStatusRStr[(int)playerRelation]);
        CurrentRelation.Text = statusText;
        MigrationRaceLB.Items.Clear();
        MigrationTurnsLB.Items.Clear();
        MigrationRelationLB.Items.Clear();
        if (FMigrations != null)
        {
            for (int I = 0, last = FMigrations.GetCount() - 1; I <= last; I++)
            {
                var itemRace = FMigrations.GetItems(I);
                var raceNamesPlural3 = TranslateRStr(RaceNamesPluralRStr[itemRace.FRace]);
                var migrationItem = FMigrations.GetItems(I);
                MigrationRaceLB.Items.AddObject(raceNamesPlural3, migrationItem);
                var itemTime = FMigrations.GetItems(I);
                MigrationTurnsLB.Items.Add(IntToStr(itemTime.FTime));
                var itemRelation = FMigrations.GetItems(I);
                var status3 = TranslateRStr(TStatusRStr[(int)itemRelation.FRelation]);
                MigrationRelationLB.Items.Add(status3);
            }
        }

        SortMigration();
        QueueLB.Items.Clear();
        for (int I = 0, last = FCity.FProduction.FQueue.GetCount() - 1; I <= last; I++)
        {
            var name2 = FCity.FProduction.FQueue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = FCity.FProduction.FQueue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = TranslateRStr(EmptyQueueRStr);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = FCity.FProduction.FQueue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = FCity.FProduction.FQueue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            FQueueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = FCity.FProduction.FLoopQueue;
        if (FCity.GetActiveAction() != TCityAction.caNone)
        {
            var nameProgressive = FCity.FProduction.FQueue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (FCity.FProduction.FSettings.FTurnsLeft != -1)
            {
                var fmtTurnsLeftStr = FmtTurnsLeftStr(FCity.FProduction.FSettings.FTurnsLeft);
                ActiveActionTurns.Text = fmtTurnsLeftStr;
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

            var activityImage = FCity.GetActiveOrder().ActivityImage(FCity);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (FCity.FProduction.FQueue.GetCount() == 0)
            {
                var producingMerchandise = TranslateRStr(ProducingMerchandiseRStr);
                ActiveAction.Text = producingMerchandise;
                var defaultText = TranslateRStr(DefaultRStr);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = ActivityImage(TCityAction.caNone);
                CurrentActionImage.CurrentImage = activityImage;
            }
            else
            {
                Reason = "";
                var nameProgressive = FCity.FProduction.FQueue.GetItem(0).GetNameProgressive();
                ActiveAction.Text = nameProgressive;
                var queue = FCity.FProduction.FQueue.GetItem(0);
                FCity.FProduction.CanExecuteProduction(queue, ref Reason);
                ActiveActionTurns.Text = Reason;
                ActiveActionTurns.TextColor = 255;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = FCity.FProduction.FQueue.GetItem(0).Order.ActivityImage(FCity);
                CurrentActionImage.CurrentImage = activityImage;
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
        ShowCity(Event.GetCity());
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
        int Direction;
        TArmyHS Army;
        TArmyHSList Armies;
        TRndTacHsm Generator;
        Reset();
        FCity = City;
        UpdateInfo();
        SetMode(0);
        FreePreview();
        for (int I = 0, last = FCity.FProduction.FUnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(FCity.FProduction.FUnitProductions.GetItem(I).FUnitIndex);
        }

        Generator = new TRndTacHsm();
        var lhx = FCity.GetLhx();
        var yhx = FCity.GetYhx();
        FPreview = Generator.RandomMap(null, FCity.GetXhx(), yhx, lhx);
        Generator.Free();
        Armies = new TArmyHSList(null);
        FCity.ListArmies(Armies);
        for (int I = 0, last = Armies.GetCount() - 1; I <= last; I++)
        {
            Army = Armies.GetArmyHS(I);
            var armyYhx = Army.GetYhx();
            var xhx = Army.GetXhx();
            var fCityYhx = FCity.GetYhx();
            Direction = dHXtoHN(FCity.GetXhx(), fCityYhx, xhx, armyYhx);
            for (int J = 0, lastJ = Army.FArmy.GetCount() - 1; J <= lastJ; J++)
            {
                var unit = Army.FArmy.GetUnit(J);
                PlaceOverviewUnit(FPreview, unit, Direction, J);
            }
        }

        Armies.Free();
        var mapObject = FPreview.FCenterViewLocation;
        if (FPreview.FCenterViewLocation != null)
        {
            var mapObjectYhx = mapObject.GetYhx();
            HXtoHP(mapObject.GetXhx(), mapObjectYhx, ref FViewX, ref FViewY);
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
        FImages = new TIntegerList();
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
        var unitProduction = FCity.FProduction.FUnitProductions.GetItem(((TAOWCheckBox)Sender).Tag);
        ((TAOWCheckBox)Sender).CaptureInput = unitProduction != null;
    }

    // AoWCityScreen.pas:348
    public void ActivityPreDraw(TAoWComponent Sender, TSurfaceDesc SD)
    {
        int Image;
        Image = ActivityImage((TCityAction)((byte)Sender.Tag + 1));
        aowIntGfxModUnit.IntGfxMod.CityWin[Image].Show(ActivityPnl2.WinLeft + Sender.WinLeft + 2, ActivityPnl2.WinTop + Sender.WinTop + 2, SD);
    }

    // AoWCityScreen.pas:354
    public void CityBtn1Click(TObject Sender)
    {
        string Reason = "";
        sbyte Race;
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
                        if (FCity.CanTrade(false, ref Reason))
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
                        if (FCity.CanLoot(ref Reason))
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
                        if (FCity.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason))
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
                        if (FCity.CanUpgrade(false, ref Reason))
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
                        if (FCity.CanFortify(false, ref Reason))
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
                        if (FCity.CanProduce(FProductionIndex, false, ref Reason))
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
                        if (FCity.CanResearch(FProductionIndex, false, ref Reason))
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
                        Race = ((TMigrationItem)MigrationRaceLB.Items.Objects[MigrationRaceLB.Selected]).FRace;
                        if (FCity.CanMigrate(Race, ref Reason))
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
        string Reason = "";
        switch (FMode)
        {
            case 1:
                switch (FAction)
                {
                    case TCityAction.caTrade:
                        if (FCity.CanTrade(true, ref Reason))
                        {
                            FCity.Trade(true);
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case TCityAction.caUpgrade:
                        if (FCity.CanUpgrade(true, ref Reason))
                        {
                            FCity.Upgrade(true);
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case TCityAction.caFortify:
                        if (FCity.CanFortify(true, ref Reason))
                        {
                            FCity.Fortify(true);
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case TCityAction.caProduce:
                        if (FCity.CanProduce(FProductionIndex, true, ref Reason))
                        {
                            FCity.Produce(FProductionIndex, true);
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case TCityAction.caResearch:
                        if (FCity.CanResearch(FProductionIndex, true, ref Reason))
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
                    if (FCity.FProduction.RetryProductionExecution(ref Reason))
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
        TRect R;
        TP2UnitShowInfo Info = default;
        TUnit U;
        TUnitProduction P;
        R = Rect(ActivityPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, ActivityPnl.WinTop + ((TAoWComponent)Sender).WinTop, ActivityPnl2.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, ActivityPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
        // { Original uses two different panels. }
        P = FCity.FProduction.FUnitProductions.GetItem(((TAoWComponent)Sender).Tag);
        if (P != null)
        {
            U = FCity.FProduction.CreateUnitProduction(((TAoWComponent)Sender).Tag);
            Info.Direction = 3;
            Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
            Info.Animation = 0;
            Info.Frame = 0;
            Info.Looped = false;
            if (!P.FResearched)
            {
                aowIntGfxModUnit.IntGfxMod.CityWin[5].Show(R.Left + 2, R.Top + 2, SD);
                Info.Direction = 5;
                if (U != null)
                {
                    U.ShowEx(R.Left - 4, R.Top, SD, Info);
                }

                var lookupTable = AlphaBlendTable.GetLookupTables(45);
                aowIntGfxModUnit.IntGfxMod.CityWin[5].ShowBlended(R.Left + 2, R.Top + 2, SD, lookupTable);
            }
            else
            {
                aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left + 2, R.Top + 2, SD);
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
            aowIntGfxModUnit.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
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
        TUnit U;
        TRect R;
        U = FCity.FProduction.CreateUnitProduction(FProductionIndex);
        if (U != null)
        {
            R = Rect(UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + 2, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + 2, UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth - 4, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight - 2);
            U.ShowFace(R, SD);
            U.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(TAOWListBox Sender)
    {
        TCityAction Action;
        var count = FCity.FProduction.FQueue.GetCount();
        if (QueueLB.Selected < count && FMode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                FQueueIndex = QueueLB.Selected;
                var order = FCity.FProduction.FQueue.GetItem(FQueueIndex).Order;
                Action = order.CityAction;
                if (order is ProduceUnit produce)
                {
                    FProductionIndex = produce.Index;
                    bool condition = FProductionIndex < 0;
                    if (!condition)
                    {
                        var fUnitProductionsCount = FCity.FProduction.FUnitProductions.GetCount();
                        condition = FProductionIndex >= fUnitProductionsCount;
                    }

                    if (condition)
                    {
                        FProductionIndex = 0;
                    }
                }

                SelectAction(Action);
                SetMode(2);
                QueueLB.Selected = FQueueIndex;
                CityBtn1.Visible = true;
                var delete = TranslateRStr(DeleteRStr);
                CityBtn1.Text = delete;
                if (FQueueIndex == 0 && FCity.FProduction.FStatus == TProductionControlStatus.pcsWaiting)
                {
                    CityBtn2.Visible = true;
                    var execute = TranslateRStr(ExecuteRStr);
                    CityBtn2.Text = execute;
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
        TRect R;
        TUnit U;
        if (FCity.FProduction.FQueue.GetCount() != 0)
        {
            if (FCity.FProduction.FQueue.GetItem(0).Order.UnitProductionIndex is int Index)
            {
                R = Rect(ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop, ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
                U = FCity.FProduction.CreateUnitProduction(Index);
                if (U != null)
                {
                    Info.Direction = 3;
                    Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                    Info.Animation = 0;
                    Info.Frame = 0;
                    Info.Looped = false;
                    if (!FCity.FProduction.FUnitProductions.GetItem(Index).FResearched)
                    {
                        Info.Direction = 5;
                    }

                    U.ShowEx(R.Left - 8, R.Top - 5, SD, Info);
                    U.Release();
                }
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
        TRect R;
        R = Rect(IPPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, IPPnl.WinTop + ((TAoWComponent)Sender).WinTop, IPPnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, IPPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
        AoWHSSet.FRaceResources.GetRaceResource(FCity.FRace).GetFace().ShowStretched(R, SD);
    }

    // AoWCityScreen.pas:641
    public void MapViewDraw(TObject Sender, TSurfaceDesc SD)
    {
        TSurfaceDesc ViewSD = default;
        TRect R;
        int W;
        int H;
        int Width;
        if (FPreview != null)
        {
            R = Rect(Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((TAoWComponent)Sender).WinTop, Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, Dlg.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
            Width = R.Right - R.Left;
            W = Width;
            H = R.Bottom - R.Top;
            var maxXhp = FPreview.GetMaxXhp();
            if (FViewX + W + 16 > maxXhp)
            {
                FViewX = FPreview.GetMaxXhp() - W - 16;
            }

            var maxYhp = FPreview.GetMaxYhp();
            if (FViewY + H + 16 > maxYhp)
            {
                FViewY = FPreview.GetMaxYhp() - H - 16;
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
        TAoWCombatMap Preview;
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
        TAbility A;
        if (Button == TMouseButton.mbRight && UAbilitiesLB.Selected > -1)
        {
            var objectValue = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
            A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                SetInfoBanner(AoWInfoBannerUnit.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWCityScreen.pas:704
    public void PPrevClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FCity);
        Index = Start;
        Index -= 1;
        if (Index < 0)
        {
            Index = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
        }

        while (Index != Start)
        {
            S = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TCity)
            {
                Index = Start + 1;
                ShowCity((TCity)S);
            }

            Index += -1;
            if (Index < 0)
            {
                Index = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
            }
        }
    }

    // AoWCityScreen.pas:726
    public void PNextClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FCity);
        Index = Start + 1;
        if (Index >= AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount())
        {
            Index = 0;
        }

        while (Index != Start)
        {
            S = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TCity)
            {
                Index = Start - 1;
                ShowCity((TCity)S);
            }

            Index += 1;
            if (Index >= AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount())
            {
                Index = 0;
            }
        }
    }
}

public static partial class AoWCityScreenUnit
{
    public static TCityScreen CityScreen;
}
