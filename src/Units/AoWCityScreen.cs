// Translated from AoWCityScreen.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Engine;
using Aow.Game.Engine.Collections;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
// Translated from AoWCityScreen.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// city, preview, view, action, production, queue, mode, migrations, sort, images
public partial class CityScreen : DataModule
{
    public CityScreen(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWPanel UnitPnl;
    public AOWListBox UAbilitiesLB;
    public AOWImage UAttackIcon;
    public AOWImage UDefenseIcon;
    public AOWImage UMovesIcon;
    public AOWImage UDamageIcon;
    public AOWImage UResistanceIcon;
    public AOWImage UHitsIcon;
    public AOWLabel UAttack;
    public AOWLabel UDefense;
    public AOWLabel UMoves;
    public AOWLabel UDamage;
    public AOWLabel UResistance;
    public AOWLabel UHits;
    public AOWPanel UStatPnl;
    public AOWPanel UStatP1;
    public AOWPanel UStatP2;
    public AOWLabel UAlignment;
    public AOWLabel ULevel;
    public AOWPanel UPicturePnl;
    public AOWPanel UCostPnl;
    public AOWLabel StructName;
    public AOWLabel CostLbl;
    public AOWLabel UCost;
    public AOWLabel UnitLbl;
    public AOWLabel TurnsLbl;
    public AOWLabel UTurns;
    public AOWCheckBox P1CB;
    public AOWCheckBox P2CB;
    public AOWCheckBox P3CB;
    public AOWCheckBox P4CB;
    public AOWCheckBox P5CB;
    public AOWCheckBox P6CB;
    public AOWCheckBox P7CB;
    public AOWCheckBox P8CB;
    public AOWCheckBox P9CB;
    public AOWCheckBox P10CB;
    public AOWCheckBox P11CB;
    public AOWCheckBox P12CB;
    public AOWCheckBox B4CB;
    public AOWCheckBox B5CB;
    public AOWCheckBox B6CB;
    public AOWCheckBox B1CB;
    public AOWCheckBox B2CB;
    public AOWCheckBox B3CB;
    public AOWVScrollBar T2SBar;
    public AOWPanel AbilPnl;
    public AOWPanel UpgradePnl;
    public AOWLabel UpgradeLbl;
    public AOWPanel UpgradeMemoFr;
    public AOWMemo UpgradeMemo;
    public AOWPanel RightPnl;
    public AOWPanel QueuePnl;
    public AOWImage Oval;
    public AOWImage ShadowL;
    public AOWImage ShadowR;
    public AOWButton CityBtn3;
    public AOWButton CityBtn1;
    public AOWListBox QueueLB;
    public AOWPanel ActivityPnl;
    public AOWPanel InfoPnl2;
    public AOWLabel ActiveAction;
    public AOWLabel ActiveActionTurns;
    public AOWImage SepBar;
    public AOWPanel ActivityPnl2;
    public AoWGauge ActiveActionTurnsBar;
    public AOWButton CityBtn2;
    public AOWPanel InfoPnl;
    public AOWPanel IPPnl;
    public AoWFrame IPPicFr;
    public AOWMemo PopulationMemo;
    public AOWMemo PopulationHdr;
    public AOWPanel IIPnl;
    public AoWFrame IIPicFr;
    public AOWMemo IncomeMemo;
    public AOWMemo IncomeHdr;
    public AOWPanel IEPnl;
    public AoWFrame IEPicFr;
    public AOWMemo EnhancementsMemo;
    public AOWMemo EnhancementsHdr;
    public AOWPanel CurrentInfo;
    public AoWFrame RPillar;
    public AoWFrame LPillar;
    public AOWPanel QueueInfo;
    public AOWLabel QueueLbl;
    public AOWCheckBox LoopQueueBox;
    public AOWLabel RepeatLbl;
    public AOWPanel ActiveActionPnl;
    public AOWImage CurrentActionImage;
    public AOWImage RacePortrait;
    public AoWFrame AoWFrame1;
    public AOWImage BlackBg;
    public AOWImage BlackBg2;
    public AOWPanel UPictureFramePnl;
    public AOWPanel UStatFr1;
    public AOWPanel UStatFr2;
    public AOWPanel UInfoFr;
    public AOWPanel CostFr;
    public AOWPanel TurnsFr;
    public AOWPanel UpgPicPnl;
    public AOWImage UpgradePic;
    public AOWPanel UpgPicFr;
    public AOWPanel UpgCostPnl;
    public AOWLabel UpgCostLbl;
    public AOWLabel UpgradeCost;
    public AOWLabel UpgTurnsLbl;
    public AOWLabel UpgradeTurns;
    public AOWPanel UpgCostFr;
    public AOWPanel UpgTurnsFr;
    public AOWPanel MigratePnl;
    public AOWLabel NewRaceLbl;
    public AOWPanel MigrateMainFr;
    public AOWPanel MigPicPnl;
    public AOWImage MigratePic;
    public AOWPanel MigPicFr;
    public AOWPanel MigCostPnl;
    public AOWLabel MigCostLbl;
    public AOWLabel MigrateCost;
    public AOWLabel MigTurnsLbl;
    public AOWLabel MigrateTurns;
    public AOWPanel MigCostFr;
    public AOWPanel MigTurnsFr;
    public AOWListBox MigrationRaceLB;
    public AOWListBox MigrationRelationLB;
    public AOWListBox MigrationTurnsLB;
    public AOWButton SortMigrationRaces;
    public AOWButton SortMigrationTurns;
    public AOWButton SortMigrationRelation;
    public AOWPanel T3LBPnl;
    public AOWVScrollBar MigrationSB;
    public AOWLabel T3RaceLbl;
    public AOWLabel CurrentRace;
    public AOWLabel T3RelLbl;
    public AOWLabel CurrentRelation;
    public AOWPanel CurrentRacePnl;
    public AOWPanel CurrentRaceFr;
    public AOWLabel CurrentRaceLbl;
    public AOWPanel UpgBtm;
    public AOWLabel UpgradeInfo;
    public AOWImage RelIcn;
    public AOWVScrollBar QueueVSB;
    public AOWVScrollBar IPVSB;
    public AOWVScrollBar IIVSB;
    public AOWVScrollBar IEVSB;
    public AOWMemo IncomeGoldMemo;
    public AOWLabel Migrate;
    public TaowUserDrawBox UFace;
    public AOWPanel MigrateInfoPnl;
    public AOWLabel MigrateInfo;
    public TaowUserDrawBox MapView;
    public AOWImage InfoSep;
    public AOWButton PNext;
    public AOWButton PPrev;
    public IvTranslator Translator;
    public City FCity;
    public AoWCombatMap FPreview;
    public int FViewX;
    public int FViewY;
    public CityAction FAction;
    public int FProductionIndex;
    public int FQueueIndex;
    public byte FMode;
    public MigrationList FMigrations;
    public int FMigrationSort;
    public IntegerList FImages;

    // AoWCityScreen.pas:748
    public void FreePreview()
    {
        if (FPreview != null)
        {
            FPreview?.Dispose();
            FPreview = null;
        }
    }

    // AoWCityScreen.pas:757
    public void ReleaseImages()
    {
        for (int I = FImages.Count - 1; I >= 0; I--)
        {
            var image = FImages[I];
            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(image).Release();
        }

        FImages.Clear();
    }

    // AoWCityScreen.pas:766
    public void LoadUnitImages(int Index)
    {
        int Gfx;
        Gfx = AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FUnitGFXResourceIndex;
        AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Gfx).AddRef();
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
        var options = AbilityRules.TranslateRStr(GameText.OptionsRStr);
        CityBtn1.Text = options;
        CityBtn2.Visible = false;
        PPrev.Visible = !CityBtn2.Visible;
        PNext.Visible = !CityBtn2.Visible;
        ShadowL.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        ShadowR.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        Oval.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        FMigrationSort = 0;
        FProductionIndex = 0;
        QueueLB.Selected = -1;
        FreePreview();
        var queue = AbilityRules.TranslateRStr(GameText.QueueRStr);
        QueueLbl.Text = queue;
        var repeat = AbilityRules.TranslateRStr(GameText.RepeatRStr);
        RepeatLbl.Text = repeat;
        if (FMigrations != null)
        {
            FMigrations?.Dispose();
            FMigrations = null;
        }
    }

    // AoWCityScreen.pas:829
    public void CloseCity()
    {
        Reset();
        FCity = null;
        DialogHosts.General.Manager.OverlayPriority = 0;
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
                var change = AbilityRules.TranslateRStr(GameText.ChangeRStr);
                CityBtn1.Text = change;
                CityBtn1.Visible = true;
                CityBtn2.Visible = false;
                PPrev.Visible = !CityBtn2.Visible;
                PNext.Visible = !CityBtn2.Visible;
                var exit = AbilityRules.TranslateRStr(GameText.ExitRStr);
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
                var info = AbilityRules.TranslateRStr(GameText.InfoRStr);
                CityBtn1.Text = info;
                var done = AbilityRules.TranslateRStr(GameText.DoneRStr);
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
                var infoText = AbilityRules.TranslateRStr(GameText.InfoRStr);
                CityBtn1.Text = infoText;
                var doneText = AbilityRules.TranslateRStr(GameText.DoneRStr);
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
                StringSorting.SortStringsNumericalAscending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                break;
            case 1:
                StringSorting.SortStringsNumericalDescending(MigrationTurnsLB.Items, [MigrationRaceLB.Items, MigrationRelationLB.Items]);
                break;
            case 2:
                StringSorting.SortStringsAscending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                break;
            case 3:
                StringSorting.SortStringsDescending(MigrationRaceLB.Items, [MigrationTurnsLB.Items, MigrationRelationLB.Items]);
                break;
            case 4:
                StringSorting.SortStringsAscending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                break;
            case 5:
                StringSorting.SortStringsDescending(MigrationRelationLB.Items, [MigrationTurnsLB.Items, MigrationRaceLB.Items]);
                break;
        }
    }

    // AoWCityScreen.pas:904
    public int ActivityImage(CityAction Action)
    {
        int Result;
        switch (Action)
        {
            case CityAction.Trade:
                Result = 40;
                break;
            case CityAction.Upgrade:
                Result = 43;
                break;
            case CityAction.Fortify:
                if (FCity.FWallType == 0)
                {
                    Result = 41;
                }
                else
                {
                    Result = 42;
                }

                break;
            case CityAction.Migrate:
                Result = 44;
                break;
            case CityAction.Loot:
                Result = 45;
                break;
            case CityAction.Raze:
                Result = 46;
                break;
            case CityAction.Produce:
                Result = 4;
                break;
            case CityAction.Research:
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
        SelectAction(CityAction.Trade);
    }

    // AoWCityScreen.pas:926
    public void SelectProduction(int Index)
    {
        UnitProduction P;
        P = FCity.FProduction.FUnitProductions.GetItem(Index);
        if (P != null)
        {
            FProductionIndex = Index;
            if (P.FResearched)
            {
                SelectAction(CityAction.Produce);
                AoWHSMap.Current.FTutorialControl.ShowMessage(58, true, true);
            }
            else
            {
                SelectAction(CityAction.Research);
            }
        }
    }

    // AoWCityScreen.pas:943
    public void SelectMigration(int Index)
    {
        MigrationItem Item;
        if (Index >= 0 && Index < MigrationRaceLB.Items.Count)
        {
            Item = (MigrationItem)MigrationRaceLB.Items.Objects[Index];
            MigrationRaceLB.Selected = Index;
            MigrationTurnsLB.Selected = Index;
            MigrationRelationLB.Selected = Index;
            MigrateTurns.Text = IntToStr(Item.FTime);
            var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FCity.FRace]);
            var format = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationMinusYRStr), [raceNames, -15]);
            MigrateInfo.Text = format;
            MigrateInfo.Text = MigrateInfo.Text + "   ";
            var raceNamesText = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[Item.FRace]);
            var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationPlusYRStr), [raceNamesText, 10]);
            MigrateInfo.Text = MigrateInfo.Text + aoWEFormat;
            CityBtn1.Visible = true;
            var migrate = AbilityRules.TranslateRStr(MigrateRStr);
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
    public void SelectAction(CityAction Action)
    {
        Unit U;
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
            case CityAction.Trade:
                CityBtn1.Visible = true;
                var produceMerchandise = AbilityRules.TranslateRStr(GameText.ProduceMerchandiseRStr);
                CityBtn1.Text = produceMerchandise;
                var queueLocked = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !queueLocked;
                var queue = AbilityRules.TranslateRStr(GameText.QueueRStr);
                CityBtn2.Text = queue;
                var cityTrade = AbilityRules.TranslateRStr(CityTradeRStr);
                UpgradeMemo.Items.Text = cityTrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 20;
                var produceMerchandiseText = AbilityRules.TranslateRStr(GameText.ProduceMerchandiseRStr);
                UpgradeLbl.Text = produceMerchandiseText;
                break;
            case CityAction.Upgrade:
                CityBtn1.Visible = true;
                var upgrade = AbilityRules.TranslateRStr(GameText.UpgradeRStr);
                CityBtn1.Text = upgrade;
                var fProductionQueueLocked = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !fProductionQueueLocked;
                var queueText = AbilityRules.TranslateRStr(GameText.QueueRStr);
                CityBtn2.Text = queueText;
                var cityUpgrade = AbilityRules.TranslateRStr(CityUpgradeRStr);
                UpgradeMemo.Items.Text = cityUpgrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 23;
                var upgradeCost = FCity.UpgradeCost();
                UpgradeCost.Text = IntToStr(upgradeCost);
                var upgradeTime = FCity.UpgradeTime();
                UpgradeTurns.Text = IntToStr(upgradeTime);
                var upgradeText = AbilityRules.TranslateRStr(GameText.UpgradeRStr);
                UpgradeLbl.Text = upgradeText;
                var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FCity.FRace]);
                var format = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationPlusYRStr), [raceNamesPlural, 5]);
                UpgradeInfo.Text = format;
                break;
            case CityAction.Fortify:
                CityBtn1.Visible = true;
                var fortify = AbilityRules.TranslateRStr(GameText.FortifyRStr);
                CityBtn1.Text = fortify;
                var queueLocked2 = FCity.FProduction.GetQueueLocked();
                CityBtn2.Visible = !queueLocked2;
                var queue3 = AbilityRules.TranslateRStr(GameText.QueueRStr);
                CityBtn2.Text = queue3;
                var cityFortify = AbilityRules.TranslateRStr(CityFortifyRStr);
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

                var fortifyText = AbilityRules.TranslateRStr(GameText.FortifyRStr);
                UpgradeLbl.Text = fortifyText;
                var raceNames = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FCity.FRace]);
                var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationPlusYRStr), [raceNames, 5]);
                UpgradeInfo.Text = aoWEFormat;
                break;
            case CityAction.Migrate:
                MigratePnl.Visible = true;
                SelectMigration(0);
                break;
            case CityAction.Loot:
                CityBtn1.Visible = true;
                var loot = AbilityRules.TranslateRStr(GameText.LootRStr);
                CityBtn1.Text = loot;
                var cityLoot = AbilityRules.TranslateRStr(CityLootRStr);
                UpgradeMemo.Items.Text = cityLoot;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 25;
                var lootText = AbilityRules.TranslateRStr(GameText.LootRStr);
                UpgradeLbl.Text = lootText;
                var raceNamesText = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FCity.FRace]);
                var format2 = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationMinusYRStr), [raceNamesText, -30]);
                UpgradeInfo.Text = format2;
                break;
            case CityAction.Raze:
                CityBtn1.Visible = true;
                var raze = AbilityRules.TranslateRStr(GameText.RazeRStr);
                CityBtn1.Text = raze + "...";
                var cityRaze = AbilityRules.TranslateRStr(CityRazeRStr);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = AbilityRules.TranslateRStr(GameText.RazeRStr);
                UpgradeLbl.Text = razeText;
                var raceNames3 = AbilityRules.TranslateRStr(GameTables.RaceNamesRStr[FCity.FRace]);
                var format3 = AbilityRules.Format(AbilityRules.TranslateRStr(XRelationMinusYRStr), [raceNames3, -30]);
                UpgradeInfo.Text = format3;
                break;
            case CityAction.Research:
            case CityAction.Produce:
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
                var mTtoInt = DialogHosts.General.MTtoInt(moveTypes);
                UMovesIcon.CurrentImage = mTtoInt;
                var alignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateRStr(GameText.TAlignmentRStr[(int)alignment]);
                var format4 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.AlignmentXRStr), [alignmentText]);
                UAlignment.Text = format4;
                var unitLevel = U.GetUnitLevel();
                var format5 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.LevelXRStr), [IntToStr(unitLevel)]);
                var upkeep = U.GetUpkeep();
                var format6 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.UpkeepXRStr), [(int)upkeep]);
                ULevel.Text = format5 + " / " + format6;
                if (GameTables.AoWEngine.FDebug)
                {
                    var unitIndexAIPriority = RosterTables.GetUnitIndexAIPriority(U.FUnitResource.UnitResourceIndex(), FCity.FPlayer, 0);
                    AoWHSMap.Current.ShowMessage("AI Prio:" + IntToStr(unitIndexAIPriority));
                }

                UAbilitiesLB.Items.Clear();
                U.ListAbilities(UAbilitiesLB.Items);
                if (FAction == CityAction.Produce)
                {
                    CityBtn1.Visible = true;
                    var produce = AbilityRules.TranslateRStr(GameText.ProduceRStr);
                    CityBtn1.Text = produce;
                    var queueLocked3 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateRStr(GameText.QueueRStr);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.ProduceXRStr), [name]);
                    UnitLbl.Text = format7;
                    var unitProductionGold = FCity.FProduction.UnitProductionGold(FProductionIndex);
                    UCost.Text = IntToStr(unitProductionGold);
                    var unitProductionTime = FCity.FProduction.UnitProductionTime(FProductionIndex);
                    UTurns.Text = IntToStr(unitProductionTime);
                }
                else
                {
                    CityBtn1.Visible = true;
                    var install = AbilityRules.TranslateRStr(GameText.InstallRStr);
                    CityBtn1.Text = install;
                    var queueLocked3 = FCity.FProduction.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateRStr(GameText.QueueRStr);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.InstallXRStr), [name]);
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
        var population = AbilityRules.TranslateRStr(PopulationRStr);
        PopulationHdr.Items.Add(population + ":");
        var raceNamesPlural = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FCity.FRace]);
        PopulationHdr.Items.Add(" " + raceNamesPlural);
        var status = AbilityRules.TranslateRStr(StatusRStr);
        PopulationHdr.Items.Add(status + ":");
        var civilStatus = FCity.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateRStr(TCivilStatusRStr[(int)civilStatus]);
        PopulationHdr.Items.Add(" " + civilStatusText);
        PopulationMemo.Items.Clear();
        FCity.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHdr.Items.Clear();
        var income = AbilityRules.TranslateRStr(GameText.IncomeRStr);
        IncomeHdr.Items.Add(income);
        var xGold = AbilityRules.TranslateRStr(GameText.XGoldRStr);
        var format = AbilityRules.Format(" " + xGold, [IntToStr(FCity.FIncome)]);
        IncomeHdr.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        FCity.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
        EnhancementsHdr.Items.Clear();
        var enhancements = AbilityRules.TranslateRStr(EnhancementsRStr);
        EnhancementsHdr.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        FCity.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (FMigrations != null)
        {
            FMigrations?.Dispose();
            FMigrations = null;
        }

        FMigrations = FCity.CreateMigrationSelection();
        var raceNamesPluralText = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[FCity.FRace]);
        CurrentRace.Text = raceNamesPluralText;
        var playerRelation = AoWHSMap.Current.FRaces.GetRace(FCity.FRace).GetPlayerRelation(FCity.FPlayer);
        var statusText = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)playerRelation]);
        CurrentRelation.Text = statusText;
        MigrationRaceLB.Items.Clear();
        MigrationTurnsLB.Items.Clear();
        MigrationRelationLB.Items.Clear();
        if (FMigrations != null)
        {
            for (int I = 0, last = FMigrations.GetCount() - 1; I <= last; I++)
            {
                var itemRace = FMigrations.GetItems(I);
                var raceNamesPlural3 = AbilityRules.TranslateRStr(GameTables.RaceNamesPluralRStr[itemRace.FRace]);
                var migrationItem = FMigrations.GetItems(I);
                MigrationRaceLB.Items.AddObject(raceNamesPlural3, migrationItem);
                var itemTime = FMigrations.GetItems(I);
                MigrationTurnsLB.Items.Add(IntToStr(itemTime.FTime));
                var itemRelation = FMigrations.GetItems(I);
                var status3 = AbilityRules.TranslateRStr(GameTables.TStatusRStr[(int)itemRelation.FRelation]);
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
            var emptyQueue = AbilityRules.TranslateRStr(GameText.EmptyQueueRStr);
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
        if (FCity.GetActiveAction() != CityAction.None)
        {
            var nameProgressive = FCity.FProduction.FQueue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (FCity.FProduction.FSettings.FTurnsLeft != -1)
            {
                var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(FCity.FProduction.FSettings.FTurnsLeft);
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
                var producingMerchandise = AbilityRules.TranslateRStr(GameText.ProducingMerchandiseRStr);
                ActiveAction.Text = producingMerchandise;
                var defaultText = AbilityRules.TranslateRStr(GameText.DefaultRStr);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = ActivityImage(CityAction.None);
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

        if (FMode == 1 && FAction != CityAction.None)
        {
            SelectAction(FAction);
        }

        Dlg.FDrawn = false;
    }

    // AoWCityScreen.pas:1200
    public void ProductionEvent(CityProductionEventLog Event)
    {
        ShowCity(Event.GetCity());
        SetMode(1);
        SelectDefaultAction();
    }

    // AoWCityScreen.pas:1207
    public void CityChanged(object Sender)
    {
        if (Dlg.Visible && Sender == FCity)
        {
            if (FCity.FPlayer == AoWHSMap.Current.FSeatedPlayer)
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
    public void ShowCity(City City)
    {
        int Direction;
        ArmyHS Army;
        ArmyHSList Armies;
        RndTacHsm Generator;
        Reset();
        FCity = City;
        UpdateInfo();
        SetMode(0);
        FreePreview();
        for (int I = 0, last = FCity.FProduction.FUnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(FCity.FProduction.FUnitProductions.GetItem(I).FUnitIndex);
        }

        Generator = new RndTacHsm();
        var lhx = FCity.GetLhx();
        var yhx = FCity.GetYhx();
        FPreview = Generator.RandomMap(null, FCity.GetXhx(), yhx, lhx);
        Generator?.Dispose();
        Armies = new ArmyHSList(null);
        FCity.ListArmies(Armies);
        for (int I = 0, last = Armies.GetCount() - 1; I <= last; I++)
        {
            Army = Armies.GetArmyHS(I);
            var armyYhx = Army.GetYhx();
            var xhx = Army.GetXhx();
            var fCityYhx = FCity.GetYhx();
            Direction = HexGeometry.dHXtoHN(FCity.GetXhx(), fCityYhx, xhx, armyYhx);
            for (int J = 0, lastJ = Army.FArmy.GetCount() - 1; J <= lastJ; J++)
            {
                var unit = Army.FArmy.GetUnit(J);
                AoWCombatMap.PlaceOverviewUnit(FPreview, unit, Direction, J);
            }
        }

        Armies?.Dispose();
        var mapObject = FPreview.FCenterViewLocation;
        if (FPreview.FCenterViewLocation != null)
        {
            var mapObjectYhx = mapObject.GetYhx();
            HexGeometry.HXtoHP(mapObject.GetXhx(), mapObjectYhx, ref FViewX, ref FViewY);
            FViewX = FViewX + 16 - MapView.WinWidth / 2;
            FViewY = FViewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            FViewX = 800;
            FViewY = 800;
        }

        AoWHSMap.Current.FTutorialControl.ShowMessage(56, true, true);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWCityScreen.pas:292
    public void CityScreenCreate()
    {
        FImages = new IntegerList();
    }

    // AoWCityScreen.pas:297
    public void CityScreenDestroy()
    {
        FImages?.Dispose();
    }

    // AoWCityScreen.pas:302
    public void ActivityChange(AOWCheckBox Sender, int Slot)
    {
        DialogHosts.General.PlayButtonSound(1);
        if (Sender.Checked)
        {
            ResetChecks();
            SelectAction((CityAction)((byte)Slot + 1));
            Sender.Checked = true;
        }
    }

    // AoWCityScreen.pas:313
    public void ActivityDoubleClick(AOWCheckBox Sender, int Slot)
    {
        if ((CityAction)((byte)Slot + 1) != CityAction.Migrate)
        {
            ResetChecks();
            SelectAction((CityAction)((byte)Slot + 1));
            Sender.Checked = true;
            CityBtn1Click();
        }
    }

    // AoWCityScreen.pas:324
    public void ProductionChange(AOWCheckBox Sender, int Slot)
    {
        DialogHosts.General.PlayButtonSound(1);
        if (Sender.Checked)
        {
            ResetChecks();
            SelectProduction(Slot);
            Sender.Checked = true;
        }
    }

    // AoWCityScreen.pas:335
    public void ProductionDoubleClick(AOWCheckBox Sender, int Slot)
    {
        ResetChecks();
        SelectProduction(Slot);
        Sender.Checked = true;
        CityBtn1Click();
    }

    // AoWCityScreen.pas:343
    public void ProductionsUpdate(AOWCheckBox Sender, int Slot)
    {
        var unitProduction = FCity.FProduction.FUnitProductions.GetItem(Slot);
        Sender.CaptureInput = unitProduction != null;
    }

    // AoWCityScreen.pas:348
    public void ActivityPreDraw(AOWCheckBox Sender, SurfaceDesc SD, int Slot)
    {
        int Image;
        Image = ActivityImage((CityAction)((byte)Slot + 1));
        DialogHosts.IntGfxMod.CityWin[Image].Show(ActivityPnl2.WinLeft + Sender.WinLeft + 2, ActivityPnl2.WinTop + Sender.WinTop + 2, SD);
    }

    // AoWCityScreen.pas:354
    public void CityBtn1Click()
    {
        string Reason = "";
        sbyte Race;
        switch (FMode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                AoWHSMap.Current.FTutorialControl.ShowMessage(57, true, true);
                break;
            case 1:
                switch (FAction)
                {
                    case CityAction.Trade:
                        if (FCity.CanTrade(false, ref Reason))
                        {
                            FCity.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Loot:
                        if (FCity.CanLoot(ref Reason))
                        {
                            FCity.Loot();
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Raze:
                        if (FCity.CanRaze(AoWHSMap.Current.FSeatedPlayer, ref Reason))
                        {
                            FCity.Raze(AoWHSMap.Current.FSeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (FCity.CanUpgrade(false, ref Reason))
                        {
                            FCity.Upgrade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (FCity.CanFortify(false, ref Reason))
                        {
                            FCity.Fortify(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (FCity.CanProduce(FProductionIndex, false, ref Reason))
                        {
                            AoWHSMap.Current.FTutorialControl.ShowMessage(59, true, true);
                            FCity.Produce(FProductionIndex, false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Research:
                        if (FCity.CanResearch(FProductionIndex, false, ref Reason))
                        {
                            FCity.Research(FProductionIndex, false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Migrate:
                        Race = ((MigrationItem)MigrationRaceLB.Items.Objects[MigrationRaceLB.Selected]).FRace;
                        if (FCity.CanMigrate(Race, ref Reason))
                        {
                            AoWHSMap.Current.FTutorialControl.ShowMessage(66, true, true);
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
                    case CityAction.Trade:
                        if (FCity.CanTrade(true, ref Reason))
                        {
                            FCity.Trade(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (FCity.CanUpgrade(true, ref Reason))
                        {
                            FCity.Upgrade(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (FCity.CanFortify(true, ref Reason))
                        {
                            FCity.Fortify(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (FCity.CanProduce(FProductionIndex, true, ref Reason))
                        {
                            FCity.Produce(FProductionIndex, true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case CityAction.Research:
                        if (FCity.CanResearch(FProductionIndex, true, ref Reason))
                        {
                            FCity.Research(FProductionIndex, true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
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
                        AoWHSMap.Current.ShowWarning(Reason);
                    }
                }

                break;
        }
    }

    // AoWCityScreen.pas:465
    public void CityBtn3Click()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (FMode == 0)
        {
            Dlg.Visible = false;
        }
        else
        {
            SetMode(0);
        }

        AoWHSMap.Current.FTutorialControl.ShowMessage(60, true, true);
    }

    // AoWCityScreen.pas:472
    public void PreDrawProductions(AOWCheckBox Sender, SurfaceDesc SD, int Slot)
    {
        Rect R;
        P2UnitShowInfo Info = default;
        Unit U;
        UnitProduction P;
        R = new Rect(ActivityPnl.WinLeft + Sender.WinLeft, ActivityPnl.WinTop + Sender.WinTop, ActivityPnl2.WinLeft + Sender.WinLeft + Sender.WinWidth, ActivityPnl.WinTop + Sender.WinTop + Sender.WinHeight);
        // { Original uses two different panels. }
        P = FCity.FProduction.FUnitProductions.GetItem(Slot);
        if (P != null)
        {
            U = FCity.FProduction.CreateUnitProduction(Slot);
            Info.Direction = 3;
            Info.Overlays = Set<UnitOverlayModesElement>.Of();
            Info.Animation = 0;
            Info.Frame = 0;
            Info.Looped = false;
            if (!P.FResearched)
            {
                DialogHosts.IntGfxMod.CityWin[5].Show(R.Left + 2, R.Top + 2, SD);
                Info.Direction = 5;
                if (U != null)
                {
                    U.ShowEx(R.Left - 4, R.Top, SD, Info);
                }

                var lookupTable = GfxState.AlphaBlendTable.GetLookupTables(45);
                DialogHosts.IntGfxMod.CityWin[5].ShowBlended(R.Left + 2, R.Top + 2, SD, lookupTable);
            }
            else
            {
                DialogHosts.IntGfxMod.CityWin[4].Show(R.Left + 2, R.Top + 2, SD);
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
            DialogHosts.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
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
    public void MigrationChange(AOWListBox Sender)
    {
        if (FMode != 2)
        {
            SelectMigration(Sender.Selected);
        }
    }

    // AoWCityScreen.pas:534
    public void MigrationSBChange(AOWVScrollBar Sender)
    {
        MigrationRaceLB.ListOffset = Sender.Position;
        MigrationTurnsLB.ListOffset = Sender.Position;
        MigrationRelationLB.ListOffset = Sender.Position;
    }

    // AoWCityScreen.pas:541
    public void UFaceDraw(TaowUserDrawBox Sender, SurfaceDesc SD)
    {
        Unit U;
        Rect R;
        U = FCity.FProduction.CreateUnitProduction(FProductionIndex);
        if (U != null)
        {
            R = new Rect(UPictureFramePnl.WinLeft + Sender.WinLeft + 2, UPictureFramePnl.WinTop + Sender.WinTop + 2, UPictureFramePnl.WinLeft + Sender.WinLeft + Sender.WinWidth - 4, UPictureFramePnl.WinTop + Sender.WinTop + Sender.WinHeight - 2);
            U.ShowFace(R, SD);
            U.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(AOWListBox Sender)
    {
        CityAction Action;
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
                var delete = AbilityRules.TranslateRStr(GameText.DeleteRStr);
                CityBtn1.Text = delete;
                if (FQueueIndex == 0 && FCity.FProduction.FStatus == ProductionControl.Status.Waiting)
                {
                    CityBtn2.Visible = true;
                    var execute = AbilityRules.TranslateRStr(GameText.ExecuteRStr);
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
    public void CurrentActionImageDraw(AOWImage Sender, SurfaceDesc SD)
    {
        P2UnitShowInfo Info = default;
        Rect R;
        Unit U;
        if (FCity.FProduction.FQueue.GetCount() != 0)
        {
            if (FCity.FProduction.FQueue.GetItem(0).Order.UnitProductionIndex is int Index)
            {
                R = new Rect(ActiveActionPnl.WinLeft + Sender.WinLeft, ActiveActionPnl.WinTop + Sender.WinTop, ActiveActionPnl.WinLeft + Sender.WinLeft + Sender.WinWidth, ActiveActionPnl.WinTop + Sender.WinTop + Sender.WinHeight);
                U = FCity.FProduction.CreateUnitProduction(Index);
                if (U != null)
                {
                    Info.Direction = 3;
                    Info.Overlays = Set<UnitOverlayModesElement>.Of();
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
    public void RacePortraitDraw(AOWImage Sender, SurfaceDesc SD)
    {
        Rect R;
        R = new Rect(IPPnl.WinLeft + Sender.WinLeft, IPPnl.WinTop + Sender.WinTop, IPPnl.WinLeft + Sender.WinLeft + Sender.WinWidth, IPPnl.WinTop + Sender.WinTop + Sender.WinHeight);
        AoWHSSet.Current.FRaceResources.GetRaceResource(FCity.FRace).GetFace().ShowStretched(R, SD);
    }

    // AoWCityScreen.pas:641
    public void MapViewDraw(TaowUserDrawBox Sender, SurfaceDesc SD)
    {
        SurfaceDesc ViewSD = default;
        Rect R;
        int W;
        int H;
        int Width;
        if (FPreview != null)
        {
            R = new Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
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

            SurfaceDesc.CreateSDfromSD(SD, R, ref ViewSD);
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
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
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
        AoWCombatMap Preview;
        Preview = FPreview;
        if (Preview != null)
        {
            Preview.NewFrame();
        }

        MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Current.Frame & 1) != 0;
    }

    // AoWCityScreen.pas:688
    public void DlgHide()
    {
        CloseCity();
    }

    // AoWCityScreen.pas:693
    public void UAbilitiesLBMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        Ability A;
        if (Button == MouseButton.Right && UAbilitiesLB.Selected > -1)
        {
            var objectValue = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
            A = AoWHSSet.Current.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWCityScreen.pas:704
    public void PPrevClick()
    {
        int Start;
        int Index;
        PlayerStructure S;
        Start = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.FindChildIndex(FCity);
        Index = Start;
        Index -= 1;
        if (Index < 0)
        {
            Index = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1;
        }

        while (Index != Start)
        {
            S = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is City)
            {
                Index = Start + 1;
                ShowCity((City)S);
            }

            Index += -1;
            if (Index < 0)
            {
                Index = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1;
            }
        }
    }

    // AoWCityScreen.pas:726
    public void PNextClick()
    {
        int Start;
        int Index;
        PlayerStructure S;
        Start = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.FindChildIndex(FCity);
        Index = Start + 1;
        if (Index >= AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount())
        {
            Index = 0;
        }

        while (Index != Start)
        {
            S = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is City)
            {
                Index = Start - 1;
                ShowCity((City)S);
            }

            Index += 1;
            if (Index >= AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount())
            {
                Index = 0;
            }
        }
    }

    private static readonly string CityFortifyRStr = "Improves town defenses. Ground based attackers must breach the wall to enter town. First level of fortification is a wooden pallisade, the second level is a stone wall.";
    private static readonly string CityLootRStr = "Pillages the town, taking 1 turn per city hex, after which the town is turned to ruins. The amount of gold depends on the city size. Looting decreases your relation with the population's race!";
    private static readonly string CityRazeRStr = "Destroys the town instantly by burning it to the ground. Razing is faster as the loot option, but doens't deliver any gold. Razing decreases your relation with the population's race!";
    private static readonly string CityTradeRStr = "When producing merchandise, a city's income increases by 25%.";
    private static readonly string CityUpgradeRStr = "Upgrades the infrastructure of the town with the technology to buy more powerful units.";
    private static readonly string EnhancementsRStr = "Enhancements";
    private static readonly string MigrateRStr = "Migrate";
    private static readonly string PopulationRStr = "Population";
    internal static readonly string StatusRStr = "Status";
    private static readonly string XRelationMinusYRStr = "%s Relation %d";
    private static readonly string XRelationPlusYRStr = "%s Relation +%d";
    internal static FixedArray_string_7 TCivilStatusRStr = FixedArray_string_7.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful", "Oppressed", "Slavery"]);
}
