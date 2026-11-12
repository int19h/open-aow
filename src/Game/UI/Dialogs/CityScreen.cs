// Translated from AoWCityScreen.pas.
using Aow.Engine.Buffers;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// city, preview, view, action, production, queue, mode, migrations, sort, images
public partial class CityScreen : DataModule
{
    public CityScreen(Component owner) : base(owner)
    { }

    public Window Dlg;
    public Panel UnitPnl;
    public ListBox UAbilitiesLB;
    public Image UMovesIcon;
    public Label UAttack;
    public Label UDefense;
    public Label UMoves;
    public Label UDamage;
    public Label UResistance;
    public Label UHits;
    public Label UAlignment;
    public Label ULevel;
    public Label StructName;
    public Label UCost;
    public Label UnitLbl;
    public Label UTurns;
    public CheckBox P1CB;
    public CheckBox P2CB;
    public CheckBox P3CB;
    public CheckBox P4CB;
    public CheckBox P5CB;
    public CheckBox P6CB;
    public CheckBox P7CB;
    public CheckBox P8CB;
    public CheckBox P9CB;
    public CheckBox P10CB;
    public CheckBox P11CB;
    public CheckBox P12CB;
    public CheckBox B4CB;
    public CheckBox B5CB;
    public CheckBox B6CB;
    public CheckBox B1CB;
    public CheckBox B2CB;
    public CheckBox B3CB;
    public Panel UpgradePnl;
    public Label UpgradeLbl;
    public Memo UpgradeMemo;
    public Image Oval;
    public Image ShadowL;
    public Image ShadowR;
    public Button CityBtn3;
    public Button CityBtn1;
    public ListBox QueueLB;
    public Panel ActivityPnl;
    public Panel InfoPnl2;
    public Label ActiveAction;
    public Label ActiveActionTurns;
    public Panel ActivityPnl2;
    public Gauge ActiveActionTurnsBar;
    public Button CityBtn2;
    public Panel InfoPnl;
    public Panel IPPnl;
    public Memo PopulationMemo;
    public Memo PopulationHdr;
    public Memo IncomeMemo;
    public Memo IncomeHdr;
    public Memo EnhancementsMemo;
    public Memo EnhancementsHdr;
    public Frame RPillar;
    public Frame LPillar;
    public Label QueueLbl;
    public CheckBox LoopQueueBox;
    public Label RepeatLbl;
    public Panel ActiveActionPnl;
    public Image CurrentActionImage;
    public Panel UPictureFramePnl;
    public Image UpgradePic;
    public Label UpgradeCost;
    public Label UpgradeTurns;
    public Panel MigratePnl;
    public Label MigrateTurns;
    public ListBox MigrationRaceLB;
    public ListBox MigrationRelationLB;
    public ListBox MigrationTurnsLB;
    public VerticalScrollBar MigrationSB;
    public Label CurrentRace;
    public Label CurrentRelation;
    public Label UpgradeInfo;
    public Memo IncomeGoldMemo;
    public Label MigrateInfo;
    public UserDrawBox MapView;
    public Button PNext;
    public Button PPrev;
    private City _city;
    private CombatMap _preview;
    private int _viewX;
    private int _viewY;
    private CityAction _action;
    private int _productionIndex;
    private int _queueIndex;
    private byte _mode;
    private MigrationList _migrations;
    private int _migrationSort;
    private Int32List _images;

    // AoWCityScreen.pas:748
    public void FreePreview()
    {
        if (_preview != null)
        {
            _preview?.Dispose();
            _preview = null;
        }
    }

    // AoWCityScreen.pas:757
    public void ReleaseImages()
    {
        for (int I = _images.Count - 1; I >= 0; I--)
        {
            var image = _images[I];
            AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWCityScreen.pas:766
    public void LoadUnitImages(int index)
    {
        int graphics;
        graphics = AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _images.Add(graphics);
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
        _mode = 0;
        InfoPnl.Visible = true;
        InfoPnl2.Visible = true;
        ActivityPnl.Visible = false;
        ActivityPnl2.Visible = false;
        UnitPnl.Visible = false;
        UpgradePnl.Visible = false;
        MigratePnl.Visible = false;
        CityBtn1.Visible = true;
        var options = AbilityRules.TranslateResourceString(GameText.OptionsText);
        CityBtn1.Text = options;
        CityBtn2.Visible = false;
        PPrev.Visible = !CityBtn2.Visible;
        PNext.Visible = !CityBtn2.Visible;
        ShadowL.Visible = GameTables.GameEngine.AnimationDetail != 0;
        ShadowR.Visible = GameTables.GameEngine.AnimationDetail != 0;
        Oval.Visible = GameTables.GameEngine.AnimationDetail != 0;
        _migrationSort = 0;
        _productionIndex = 0;
        QueueLB.Selected = -1;
        FreePreview();
        var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
        QueueLbl.Text = queue;
        var repeat = AbilityRules.TranslateResourceString(GameText.RepeatText);
        RepeatLbl.Text = repeat;
        if (_migrations != null)
        {
            _migrations?.Dispose();
            _migrations = null;
        }
    }

    // AoWCityScreen.pas:829
    public void CloseCity()
    {
        Reset();
        _city = null;
        DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWCityScreen.pas:836
    public void SetMode(byte mode)
    {
        _mode = mode;
        switch (_mode)
        {
            case 0:
                InfoPnl.Visible = true;
                InfoPnl2.Visible = true;
                ActivityPnl.Visible = false;
                ActivityPnl2.Visible = false;
                var change = AbilityRules.TranslateResourceString(GameText.ChangeText);
                CityBtn1.Text = change;
                CityBtn1.Visible = true;
                CityBtn2.Visible = false;
                PPrev.Visible = !CityBtn2.Visible;
                PNext.Visible = !CityBtn2.Visible;
                var exit = AbilityRules.TranslateResourceString(GameText.ExitText);
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
                var info = AbilityRules.TranslateResourceString(GameText.InfoText);
                CityBtn1.Text = info;
                var done = AbilityRules.TranslateResourceString(GameText.DoneText);
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
                var infoText = AbilityRules.TranslateResourceString(GameText.InfoText);
                CityBtn1.Text = infoText;
                var doneText = AbilityRules.TranslateResourceString(GameText.DoneText);
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
        switch (_migrationSort)
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
    public int ActivityImage(CityAction action)
    {
        int result;
        switch (action)
        {
            case CityAction.Trade:
                result = 40;
                break;
            case CityAction.Upgrade:
                result = 43;
                break;
            case CityAction.Fortify:
                if (_city.WallType == 0)
                {
                    result = 41;
                }
                else
                {
                    result = 42;
                }

                break;
            case CityAction.Migrate:
                result = 44;
                break;
            case CityAction.Loot:
                result = 45;
                break;
            case CityAction.Raze:
                result = 46;
                break;
            case CityAction.Produce:
                result = 4;
                break;
            case CityAction.Research:
                result = 5;
                break;
            default:
                result = 40;
                break;
        }

        return result;
    }

    // AoWCityScreen.pas:919
    public void SelectDefaultAction()
    {
        ResetChecks();
        B1CB.Checked = true;
        SelectAction(CityAction.Trade);
    }

    // AoWCityScreen.pas:926
    public void SelectProduction(int index)
    {
        UnitProduction P;
        P = _city.Production.UnitProductions.GetItem(index);
        if (P != null)
        {
            _productionIndex = index;
            if (P.Researched)
            {
                SelectAction(CityAction.Produce);
                AoWHSMap.Current.TutorialControl.ShowMessage(58, true, true);
            }
            else
            {
                SelectAction(CityAction.Research);
            }
        }
    }

    // AoWCityScreen.pas:943
    public void SelectMigration(int index)
    {
        MigrationItem item;
        if (index >= 0 && index < MigrationRaceLB.Items.Count)
        {
            item = (MigrationItem)MigrationRaceLB.Items.Objects[index];
            MigrationRaceLB.Selected = index;
            MigrationTurnsLB.Selected = index;
            MigrationRelationLB.Selected = index;
            MigrateTurns.Text = IntToStr(item.Time);
            var raceNames = AbilityRules.TranslateResourceString(GameTables.s_raceNames[_city.Race]);
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNames, -15]);
            MigrateInfo.Text = format;
            MigrateInfo.Text = MigrateInfo.Text + "   ";
            var raceNamesText = AbilityRules.TranslateResourceString(GameTables.s_raceNames[item.Race]);
            var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNamesText, 10]);
            MigrateInfo.Text = MigrateInfo.Text + formattedText;
            CityBtn1.Visible = true;
            var migrate = AbilityRules.TranslateResourceString(s_migrateText);
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
    public void SelectAction(CityAction action)
    {
        Unit U;
        if (_mode != 1)
        {
            SetMode(1);
        }

        _action = action;
        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        MigratePnl.Visible = false;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        UpgradeInfo.Text = "";
        CityBtn1.Visible = false;
        CityBtn2.Visible = false;
        switch (_action)
        {
            case CityAction.Trade:
                CityBtn1.Visible = true;
                var produceMerchandise = AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText);
                CityBtn1.Text = produceMerchandise;
                var queueLocked = _city.Production.GetQueueLocked();
                CityBtn2.Visible = !queueLocked;
                var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityBtn2.Text = queue;
                var cityTrade = AbilityRules.TranslateResourceString(s_cityTradeText);
                UpgradeMemo.Items.Text = cityTrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 20;
                var produceMerchandiseText = AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText);
                UpgradeLbl.Text = produceMerchandiseText;
                break;
            case CityAction.Upgrade:
                CityBtn1.Visible = true;
                var upgrade = AbilityRules.TranslateResourceString(GameText.UpgradeText);
                CityBtn1.Text = upgrade;
                var fProductionQueueLocked = _city.Production.GetQueueLocked();
                CityBtn2.Visible = !fProductionQueueLocked;
                var queueText = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityBtn2.Text = queueText;
                var cityUpgrade = AbilityRules.TranslateResourceString(s_cityUpgradeText);
                UpgradeMemo.Items.Text = cityUpgrade;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 23;
                var upgradeCost = _city.UpgradeCost();
                UpgradeCost.Text = IntToStr(upgradeCost);
                var upgradeTime = _city.UpgradeTime();
                UpgradeTurns.Text = IntToStr(upgradeTime);
                var upgradeText = AbilityRules.TranslateResourceString(GameText.UpgradeText);
                UpgradeLbl.Text = upgradeText;
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNamesPlural, 5]);
                UpgradeInfo.Text = format;
                break;
            case CityAction.Fortify:
                CityBtn1.Visible = true;
                var fortify = AbilityRules.TranslateResourceString(GameText.FortifyText);
                CityBtn1.Text = fortify;
                var queueLocked2 = _city.Production.GetQueueLocked();
                CityBtn2.Visible = !queueLocked2;
                var queue3 = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityBtn2.Text = queue3;
                var cityFortify = AbilityRules.TranslateResourceString(s_cityFortifyText);
                UpgradeMemo.Items.Text = cityFortify;
                UpgradePnl.Visible = true;
                if (_city.WallType == 0)
                {
                    UpgradePic.CurrentImage = 21;
                }
                else
                {
                    UpgradePic.CurrentImage = 22;
                }

                if (_city.FortifyCost() > 0)
                {
                    var fortifyCost = _city.FortifyCost();
                    UpgradeCost.Text = IntToStr(fortifyCost);
                    var fortifyTime = _city.FortifyTime();
                    UpgradeTurns.Text = IntToStr(fortifyTime);
                }
                else
                {
                    UpgradeCost.Text = "-";
                    UpgradeTurns.Text = "-";
                }

                var fortifyText = AbilityRules.TranslateResourceString(GameText.FortifyText);
                UpgradeLbl.Text = fortifyText;
                var raceNames = AbilityRules.TranslateResourceString(GameTables.s_raceNames[_city.Race]);
                var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNames, 5]);
                UpgradeInfo.Text = formattedText;
                break;
            case CityAction.Migrate:
                MigratePnl.Visible = true;
                SelectMigration(0);
                break;
            case CityAction.Loot:
                CityBtn1.Visible = true;
                var loot = AbilityRules.TranslateResourceString(GameText.LootText);
                CityBtn1.Text = loot;
                var cityLoot = AbilityRules.TranslateResourceString(s_cityLootText);
                UpgradeMemo.Items.Text = cityLoot;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 25;
                var lootText = AbilityRules.TranslateResourceString(GameText.LootText);
                UpgradeLbl.Text = lootText;
                var raceNamesText = AbilityRules.TranslateResourceString(GameTables.s_raceNames[_city.Race]);
                var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNamesText, -30]);
                UpgradeInfo.Text = format2;
                break;
            case CityAction.Raze:
                CityBtn1.Visible = true;
                var raze = AbilityRules.TranslateResourceString(GameText.RazeText);
                CityBtn1.Text = raze + "...";
                var cityRaze = AbilityRules.TranslateResourceString(s_cityRazeText);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = AbilityRules.TranslateResourceString(GameText.RazeText);
                UpgradeLbl.Text = razeText;
                var raceNames3 = AbilityRules.TranslateResourceString(GameTables.s_raceNames[_city.Race]);
                var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNames3, -30]);
                UpgradeInfo.Text = format3;
                break;
            case CityAction.Research:
            case CityAction.Produce:
                U = _city.Production.CreateUnitProduction(_productionIndex);
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
                var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                UMovesIcon.CurrentImage = moveTypesToInt32;
                var alignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                UAlignment.Text = format4;
                var unitLevel = U.GetUnitLevel();
                var format5 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                var upkeep = U.GetUpkeep();
                var format6 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                ULevel.Text = format5 + " / " + format6;
                if (GameTables.GameEngine.Debug)
                {
                    var unitIndexAIPriority = RosterTables.GetUnitIndexAIPriority(U.UnitResource.UnitResourceIndex(), _city.Player, 0);
                    AoWHSMap.Current.ShowMessage("AI Prio:" + IntToStr(unitIndexAIPriority));
                }

                UAbilitiesLB.Items.Clear();
                U.ListAbilities(UAbilitiesLB.Items);
                if (_action == CityAction.Produce)
                {
                    CityBtn1.Visible = true;
                    var produce = AbilityRules.TranslateResourceString(GameText.ProduceText);
                    CityBtn1.Text = produce;
                    var queueLocked3 = _city.Production.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateResourceString(GameText.QueueText);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ProduceFormat), [name]);
                    UnitLbl.Text = format7;
                    var unitProductionGold = _city.Production.UnitProductionGold(_productionIndex);
                    UCost.Text = IntToStr(unitProductionGold);
                    var unitProductionTime = _city.Production.UnitProductionTime(_productionIndex);
                    UTurns.Text = IntToStr(unitProductionTime);
                }
                else
                {
                    CityBtn1.Visible = true;
                    var install = AbilityRules.TranslateResourceString(GameText.InstallText);
                    CityBtn1.Text = install;
                    var queueLocked3 = _city.Production.GetQueueLocked();
                    CityBtn2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateResourceString(GameText.QueueText);
                    CityBtn2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.InstallFormat), [name]);
                    UnitLbl.Text = format7;
                    var unitResearchGold = _city.Production.UnitResearchGold(_productionIndex);
                    UCost.Text = IntToStr(unitResearchGold);
                    var unitResearchTime = _city.Production.UnitResearchTime(_productionIndex);
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
        string reason;
        var name = _city.GetName();
        StructName.Text = name;
        PopulationHdr.Items.Clear();
        var population = AbilityRules.TranslateResourceString(s_populationText);
        PopulationHdr.Items.Add(population + ":");
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        PopulationHdr.Items.Add(" " + raceNamesPlural);
        var status = AbilityRules.TranslateResourceString(StatusText);
        PopulationHdr.Items.Add(status + ":");
        var civilStatus = _city.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateResourceString(s_civilStatusTexts[(int)civilStatus]);
        PopulationHdr.Items.Add(" " + civilStatusText);
        PopulationMemo.Items.Clear();
        _city.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHdr.Items.Clear();
        var income = AbilityRules.TranslateResourceString(GameText.IncomeText);
        IncomeHdr.Items.Add(income);
        var xGold = AbilityRules.TranslateResourceString(GameText.GoldFormat);
        var format = AbilityRules.Format(" " + xGold, [IntToStr(_city.Income)]);
        IncomeHdr.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        _city.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
        EnhancementsHdr.Items.Clear();
        var enhancements = AbilityRules.TranslateResourceString(s_enhancementsText);
        EnhancementsHdr.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        _city.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (_migrations != null)
        {
            _migrations?.Dispose();
            _migrations = null;
        }

        _migrations = _city.CreateMigrationSelection();
        var raceNamesPluralText = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        CurrentRace.Text = raceNamesPluralText;
        var playerRelation = AoWHSMap.Current.Races.GetRace(_city.Race).GetPlayerRelation(_city.Player);
        var statusText = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
        CurrentRelation.Text = statusText;
        MigrationRaceLB.Items.Clear();
        MigrationTurnsLB.Items.Clear();
        MigrationRelationLB.Items.Clear();
        if (_migrations != null)
        {
            for (int I = 0, last = _migrations.GetCount() - 1; I <= last; I++)
            {
                var itemRace = _migrations.GetItems(I);
                var raceNamesPlural3 = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[itemRace.Race]);
                var migrationItem = _migrations.GetItems(I);
                MigrationRaceLB.Items.AddObject(raceNamesPlural3, migrationItem);
                var itemTime = _migrations.GetItems(I);
                MigrationTurnsLB.Items.Add(IntToStr(itemTime.Time));
                var itemRelation = _migrations.GetItems(I);
                var status3 = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)itemRelation.Relation]);
                MigrationRelationLB.Items.Add(status3);
            }
        }

        SortMigration();
        QueueLB.Items.Clear();
        for (int I = 0, last = _city.Production.Queue.GetCount() - 1; I <= last; I++)
        {
            var name2 = _city.Production.Queue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = _city.Production.Queue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = AbilityRules.TranslateResourceString(GameText.EmptyQueueText);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = _city.Production.Queue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = _city.Production.Queue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            _queueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = _city.Production.LoopQueue;
        if (_city.GetActiveAction() != CityAction.None)
        {
            var nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (_city.Production.Settings.TurnsLeft != -1)
            {
                var formatTurnsLeftString = DurationText.FormatTurnsLeftString(_city.Production.Settings.TurnsLeft);
                ActiveActionTurns.Text = formatTurnsLeftString;
                ActiveActionTurns.Visible = true;
                ActiveActionTurns.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = _city.Production.Settings.TurnsTotal;
                this.ActiveActionTurnsBar.Position = _city.Production.Settings.TurnsTotal - _city.Production.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurns.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            var activityImage = _city.GetActiveOrder().ActivityImage(_city);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (_city.Production.Queue.GetCount() == 0)
            {
                var producingMerchandise = AbilityRules.TranslateResourceString(GameText.ProducingMerchandiseText);
                ActiveAction.Text = producingMerchandise;
                var defaultText = AbilityRules.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = ActivityImage(CityAction.None);
                CurrentActionImage.CurrentImage = activityImage;
            }
            else
            {
                reason = "";
                var nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
                ActiveAction.Text = nameProgressive;
                var queue = _city.Production.Queue.GetItem(0);
                _city.Production.CanExecuteProduction(queue, ref reason);
                ActiveActionTurns.Text = reason;
                ActiveActionTurns.TextColor = 255;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = _city.Production.Queue.GetItem(0).Order.ActivityImage(_city);
                CurrentActionImage.CurrentImage = activityImage;
            }
        }

        if (_mode == 1 && _action != CityAction.None)
        {
            SelectAction(_action);
        }

        Dlg.Drawn = false;
    }

    // AoWCityScreen.pas:1200
    public void ProductionEvent(CityProductionEventLog eventValue)
    {
        ShowCity(eventValue.GetCity());
        SetMode(1);
        SelectDefaultAction();
    }

    // AoWCityScreen.pas:1207
    public void CityChanged(object sender)
    {
        if (Dlg.Visible && sender == _city)
        {
            if (_city.Player == AoWHSMap.Current.SeatedPlayer)
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
    public void ShowCity(City city)
    {
        int direction;
        ArmyHexSprite army;
        ArmyHSList armies;
        RandomBattleMapGenerator generator;
        Reset();
        _city = city;
        UpdateInfo();
        SetMode(0);
        FreePreview();
        for (int I = 0, last = _city.Production.UnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(_city.Production.UnitProductions.GetItem(I).UnitIndex);
        }

        generator = new RandomBattleMapGenerator();
        var hexLevel = _city.GetHexLevel();
        var hexY = _city.GetHexY();
        _preview = generator.RandomMap(null, _city.GetHexX(), hexY, hexLevel);
        generator?.Dispose();
        armies = new ArmyHSList(null);
        _city.ListArmies(armies);
        for (int I = 0, last = armies.GetCount() - 1; I <= last; I++)
        {
            army = armies.GetArmyHexSprite(I);
            var armyHexY = army.GetHexY();
            var hexX = army.GetHexX();
            var fCityHexY = _city.GetHexY();
            direction = HexGeometry.DeltaHexToHexNumber(_city.GetHexX(), fCityHexY, hexX, armyHexY);
            for (int J = 0, lastJ = army.Army.GetCount() - 1; J <= lastJ; J++)
            {
                var troop = army.Army.GetUnit(J);
                CombatMap.PlaceOverviewUnit(_preview, troop, direction, J);
            }
        }

        armies?.Dispose();
        var mapObject = _preview.CenterViewLocation;
        if (_preview.CenterViewLocation != null)
        {
            var mapObjectHexY = mapObject.GetHexY();
            HexGeometry.HexToPixel(mapObject.GetHexX(), mapObjectHexY, ref _viewX, ref _viewY);
            _viewX = _viewX + 16 - MapView.WinWidth / 2;
            _viewY = _viewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            _viewX = 800;
            _viewY = 800;
        }

        AoWHSMap.Current.TutorialControl.ShowMessage(56, true, true);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWCityScreen.pas:292
    public void CityScreenCreate()
    {
        _images = new Int32List();
    }

    // AoWCityScreen.pas:297
    public void CityScreenDestroy()
    {
        _images?.Dispose();
    }

    // AoWCityScreen.pas:302
    public void ActivityChange(CheckBox sender, int slot)
    {
        DialogHosts.General.PlayButtonSound(1);
        if (sender.Checked)
        {
            ResetChecks();
            SelectAction((CityAction)((byte)slot + 1));
            sender.Checked = true;
        }
    }

    // AoWCityScreen.pas:313
    public void ActivityDoubleClick(CheckBox sender, int slot)
    {
        if ((CityAction)((byte)slot + 1) != CityAction.Migrate)
        {
            ResetChecks();
            SelectAction((CityAction)((byte)slot + 1));
            sender.Checked = true;
            CityBtn1Click();
        }
    }

    // AoWCityScreen.pas:324
    public void ProductionChange(CheckBox sender, int slot)
    {
        DialogHosts.General.PlayButtonSound(1);
        if (sender.Checked)
        {
            ResetChecks();
            SelectProduction(slot);
            sender.Checked = true;
        }
    }

    // AoWCityScreen.pas:335
    public void ProductionDoubleClick(CheckBox sender, int slot)
    {
        ResetChecks();
        SelectProduction(slot);
        sender.Checked = true;
        CityBtn1Click();
    }

    // AoWCityScreen.pas:343
    public void ProductionsUpdate(CheckBox sender, int slot)
    {
        var unitProduction = _city.Production.UnitProductions.GetItem(slot);
        sender.CaptureInput = unitProduction != null;
    }

    // AoWCityScreen.pas:348
    public void ActivityPreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        int image;
        image = ActivityImage((CityAction)((byte)slot + 1));
        DialogHosts.UIGraphics.CityWin[image].Show(ActivityPnl2.WinLeft + sender.WinLeft + 2, ActivityPnl2.WinTop + sender.WinTop + 2, lockedSurface);
    }

    // AoWCityScreen.pas:354
    public void CityBtn1Click()
    {
        string reason = "";
        sbyte race;
        switch (_mode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                AoWHSMap.Current.TutorialControl.ShowMessage(57, true, true);
                break;
            case 1:
                switch (_action)
                {
                    case CityAction.Trade:
                        if (_city.CanTrade(false, ref reason))
                        {
                            _city.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Loot:
                        if (_city.CanLoot(ref reason))
                        {
                            _city.Loot();
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Raze:
                        if (_city.CanRaze(AoWHSMap.Current.SeatedPlayer, ref reason))
                        {
                            _city.Raze(AoWHSMap.Current.SeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (_city.CanUpgrade(false, ref reason))
                        {
                            _city.Upgrade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (_city.CanFortify(false, ref reason))
                        {
                            _city.Fortify(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, false, ref reason))
                        {
                            AoWHSMap.Current.TutorialControl.ShowMessage(59, true, true);
                            _city.Produce(_productionIndex, false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Research:
                        if (_city.CanResearch(_productionIndex, false, ref reason))
                        {
                            _city.Research(_productionIndex, false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Migrate:
                        race = ((MigrationItem)MigrationRaceLB.Items.Objects[MigrationRaceLB.Selected]).Race;
                        if (_city.CanMigrate(race, ref reason))
                        {
                            AoWHSMap.Current.TutorialControl.ShowMessage(66, true, true);
                            _city.Migrate(race);
                            SetMode(0);
                        }

                        break;
                }

                break;
            case 2:
                if (_queueIndex != -1)
                {
                    _city.Production.DeleteProduction(_queueIndex);
                }

                break;
        }
    }

    // AoWCityScreen.pas:437
    public void CityBtn2Click()
    {
        string reason = "";
        switch (_mode)
        {
            case 1:
                switch (_action)
                {
                    case CityAction.Trade:
                        if (_city.CanTrade(true, ref reason))
                        {
                            _city.Trade(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (_city.CanUpgrade(true, ref reason))
                        {
                            _city.Upgrade(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (_city.CanFortify(true, ref reason))
                        {
                            _city.Fortify(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, true, ref reason))
                        {
                            _city.Produce(_productionIndex, true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Research:
                        if (_city.CanResearch(_productionIndex, true, ref reason))
                        {
                            _city.Research(_productionIndex, true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                }

                break;
            case 2:
                if (_queueIndex == 0)
                {
                    if (_city.Production.RetryProductionExecution(ref reason))
                    {
                        SetMode(0);
                    }
                    else
                    {
                        AoWHSMap.Current.ShowWarning(reason);
                    }
                }

                break;
        }
    }

    // AoWCityScreen.pas:465
    public void CityBtn3Click()
    {
        DialogHosts.General.PlayButtonSound(0);
        if (_mode == 0)
        {
            Dlg.Visible = false;
        }
        else
        {
            SetMode(0);
        }

        AoWHSMap.Current.TutorialControl.ShowMessage(60, true, true);
    }

    // AoWCityScreen.pas:472
    public void PreDrawProductions(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Rect R;
        UnitShowInfo info = default;
        Unit U;
        UnitProduction P;
        R = new Rect(ActivityPnl.WinLeft + sender.WinLeft, ActivityPnl.WinTop + sender.WinTop, ActivityPnl2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPnl.WinTop + sender.WinTop + sender.WinHeight);
        // { Original uses two different panels. }
        P = _city.Production.UnitProductions.GetItem(slot);
        if (P != null)
        {
            U = _city.Production.CreateUnitProduction(slot);
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of();
            info.Animation = 0;
            info.Frame = 0;
            info.Looped = false;
            if (!P.Researched)
            {
                DialogHosts.UIGraphics.CityWin[5].Show(R.Left + 2, R.Top + 2, lockedSurface);
                info.Direction = 5;
                if (U != null)
                {
                    U.Show(R.Left - 4, R.Top, lockedSurface, info);
                }

                var lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                DialogHosts.UIGraphics.CityWin[5].ShowBlended(R.Left + 2, R.Top + 2, lockedSurface, lookupTable);
            }
            else
            {
                DialogHosts.UIGraphics.CityWin[4].Show(R.Left + 2, R.Top + 2, lockedSurface);
                if (U != null)
                {
                    U.Show(R.Left - 4, R.Top, lockedSurface, info);
                }
            }

            if (U != null)
            {
                U.Release();
            }
        }
        else
        {
            DialogHosts.UIGraphics.CityWin[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
        }
    }

    // AoWCityScreen.pas:508
    public void SortMigrationRacesClick()
    {
        if (_migrationSort == 2)
        {
            _migrationSort = 3;
        }
        else
        {
            _migrationSort = 2;
        }

        SortMigration();
    }

    // AoWCityScreen.pas:515
    public void SortMigrationTurnsClick()
    {
        if (_migrationSort == 0)
        {
            _migrationSort = 1;
        }
        else
        {
            _migrationSort = 0;
        }

        SortMigration();
    }

    // AoWCityScreen.pas:522
    public void SortMigrationRelationClick()
    {
        if (_migrationSort == 4)
        {
            _migrationSort = 5;
        }
        else
        {
            _migrationSort = 4;
        }

        SortMigration();
    }

    // AoWCityScreen.pas:529
    public void MigrationChange(ListBox sender)
    {
        if (_mode != 2)
        {
            SelectMigration(sender.Selected);
        }
    }

    // AoWCityScreen.pas:534
    public void MigrationSBChange(VerticalScrollBar sender)
    {
        MigrationRaceLB.ListOffset = sender.Position;
        MigrationTurnsLB.ListOffset = sender.Position;
        MigrationRelationLB.ListOffset = sender.Position;
    }

    // AoWCityScreen.pas:541
    public void UFaceDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        Unit U;
        Rect R;
        U = _city.Production.CreateUnitProduction(_productionIndex);
        if (U != null)
        {
            R = new Rect(UPictureFramePnl.WinLeft + sender.WinLeft + 2, UPictureFramePnl.WinTop + sender.WinTop + 2, UPictureFramePnl.WinLeft + sender.WinLeft + sender.WinWidth - 4, UPictureFramePnl.WinTop + sender.WinTop + sender.WinHeight - 2);
            U.ShowFace(R, lockedSurface);
            U.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(ListBox sender)
    {
        CityAction action;
        var count = _city.Production.Queue.GetCount();
        if (QueueLB.Selected < count && _mode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                _queueIndex = QueueLB.Selected;
                var order = _city.Production.Queue.GetItem(_queueIndex).Order;
                action = order.CityAction;
                if (order is ProduceUnit produce)
                {
                    _productionIndex = produce.Index;
                    bool condition = _productionIndex < 0;
                    if (!condition)
                    {
                        var fUnitProductionsCount = _city.Production.UnitProductions.GetCount();
                        condition = _productionIndex >= fUnitProductionsCount;
                    }

                    if (condition)
                    {
                        _productionIndex = 0;
                    }
                }

                SelectAction(action);
                SetMode(2);
                QueueLB.Selected = _queueIndex;
                CityBtn1.Visible = true;
                var delete = AbilityRules.TranslateResourceString(GameText.DeleteText);
                CityBtn1.Text = delete;
                if (_queueIndex == 0 && _city.Production.Status == ProductionControlStatus.Waiting)
                {
                    CityBtn2.Visible = true;
                    var execute = AbilityRules.TranslateResourceString(GameText.ExecuteText);
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
            if (_mode == 2)
            {
                sender.Selected = _queueIndex;
            }
            else
            {
                sender.Selected = -1;
            }
        }
    }

    // AoWCityScreen.pas:597
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        Rect R;
        Unit U;
        if (_city.Production.Queue.GetCount() != 0)
        {
            if (_city.Production.Queue.GetItem(0).Order.UnitProductionIndex is int index)
            {
                R = new Rect(ActiveActionPnl.WinLeft + sender.WinLeft, ActiveActionPnl.WinTop + sender.WinTop, ActiveActionPnl.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPnl.WinTop + sender.WinTop + sender.WinHeight);
                U = _city.Production.CreateUnitProduction(index);
                if (U != null)
                {
                    info.Direction = 3;
                    info.Overlays = Set<UnitOverlayModesElement>.Of();
                    info.Animation = 0;
                    info.Frame = 0;
                    info.Looped = false;
                    if (!_city.Production.UnitProductions.GetItem(index).Researched)
                    {
                        info.Direction = 5;
                    }

                    U.Show(R.Left - 8, R.Top - 5, lockedSurface, info);
                    U.Release();
                }
            }
        }
    }

    // AoWCityScreen.pas:626
    public void LoopQueueBoxChange()
    {
        _city.Production.SetLoopQueueTo(LoopQueueBox.Checked);
    }

    // AoWCityScreen.pas:631
    public void RacePortraitDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect R;
        R = new Rect(IPPnl.WinLeft + sender.WinLeft, IPPnl.WinTop + sender.WinTop, IPPnl.WinLeft + sender.WinLeft + sender.WinWidth, IPPnl.WinTop + sender.WinTop + sender.WinHeight);
        AoWHSSet.Current.RaceResources.GetRaceResource(_city.Race).GetFace().ShowStretched(R, lockedSurface);
    }

    // AoWCityScreen.pas:641
    public void MapViewDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        LockedSurface viewSurfaceDescription = default;
        Rect R;
        int W;
        int H;
        int width;
        if (_preview != null)
        {
            R = new Rect(Dlg.WinLeft + sender.WinLeft, Dlg.WinTop + sender.WinTop, Dlg.WinLeft + sender.WinLeft + sender.WinWidth, Dlg.WinTop + sender.WinTop + sender.WinHeight);
            width = R.Right - R.Left;
            W = width;
            H = R.Bottom - R.Top;
            var maxPixelX = _preview.GetMaxPixelX();
            if (_viewX + W + 16 > maxPixelX)
            {
                _viewX = _preview.GetMaxPixelX() - W - 16;
            }

            var maxPixelY = _preview.GetMaxPixelY();
            if (_viewY + H + 16 > maxPixelY)
            {
                _viewY = _preview.GetMaxPixelY() - H - 16;
            }

            if (_viewX < 16)
            {
                _viewX = 16;
            }

            if (_viewY < 16)
            {
                _viewY = 16;
            }

            LockedSurface.CreateFrom(lockedSurface, R, ref viewSurfaceDescription);
            _preview.SetExplorationEnabled(false);
            _preview.SetFogEnabled(false);
            _preview.SetViewPort(_viewX, _viewY, 0, width, R.Bottom - R.Top);
            _preview.UpdateStaticScene();
            _preview.UpdateDynamicScene();
            _preview.DrawStaticScene(ref viewSurfaceDescription);
            _preview.DrawDynamicScene(ref viewSurfaceDescription);
        }
    }

    // AoWCityScreen.pas:668
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 37:
                _viewX -= 16;
                MapView.Drawn = false;
                break;
            case 39:
                _viewX += 16;
                MapView.Drawn = false;
                break;
            case 38:
                _viewY -= 16;
                MapView.Drawn = false;
                break;
            case 40:
                _viewY += 16;
                MapView.Drawn = false;
                break;
            case 27:
                Dlg.Visible = false;
                break;
        }
    }

    // AoWCityScreen.pas:679
    public void MapViewUpdate()
    {
        CombatMap preview;
        preview = _preview;
        if (preview != null)
        {
            preview.NewFrame();
        }

        MapView.Drawn = MapView.Drawn && (AoWHSMap.Current.Frame & 1) != 0;
    }

    // AoWCityScreen.pas:688
    public void DlgHide()
    {
        CloseCity();
    }

    // AoWCityScreen.pas:693
    public void UAbilitiesLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability A;
        if (button == MouseButton.Right && UAbilitiesLB.Selected > -1)
        {
            var objectValue = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
            A = AoWHSSet.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                InfoBanner.SetInfoBanner(DialogHosts.InfoBanner, name, A.Description);
            }
        }
    }

    // AoWCityScreen.pas:704
    public void PPrevClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            S = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is City)
            {
                index = start + 1;
                ShowCity((City)S);
            }

            index += -1;
            if (index < 0)
            {
                index = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1;
            }
        }
    }

    // AoWCityScreen.pas:726
    public void PNextClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start + 1;
        if (index >= AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            S = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is City)
            {
                index = start - 1;
                ShowCity((City)S);
            }

            index += 1;
            if (index >= AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount())
            {
                index = 0;
            }
        }
    }

    private static readonly string s_cityFortifyText = "Improves town defenses. Ground based attackers must breach the wall to enter town. First level of fortification is a wooden pallisade, the second level is a stone wall.";
    private static readonly string s_cityLootText = "Pillages the town, taking 1 turn per city hex, after which the town is turned to ruins. The amount of gold depends on the city size. Looting decreases your relation with the population's race!";
    private static readonly string s_cityRazeText = "Destroys the town instantly by burning it to the ground. Razing is faster as the loot option, but doens't deliver any gold. Razing decreases your relation with the population's race!";
    private static readonly string s_cityTradeText = "When producing merchandise, a city's income increases by 25%.";
    private static readonly string s_cityUpgradeText = "Upgrades the infrastructure of the town with the technology to buy more powerful units.";
    private static readonly string s_enhancementsText = "Enhancements";
    private static readonly string s_migrateText = "Migrate";
    private static readonly string s_populationText = "Population";

    internal static string StatusText { get; } = "Status";

    private static readonly string s_relationMinusFormat = "%s Relation %d";
    private static readonly string s_relationPlusFormat = "%s Relation +%d";
    internal static StringArray7 s_civilStatusTexts = StringArray7.From(["Unruly", "Unrest", "Stable", "Content", "Cheerful", "Oppressed", "Slavery"]);
}
