// Translated from AoWCityScreen.pas.
using System.Collections.Immutable;

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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// city, preview, view, action, production, queue, mode, migrations, sort, images
public partial class CityScreen : DataModule
{
    public CityScreen(Component owner) : base(owner)
    { }

    public Window Window;
    public Panel UnitPanel;
    public ListBox UnitAbilitiesList;
    public Image UnitMovesIcon;
    public Label UnitAttackLabel;
    public Label UnitDefenseLabel;
    public Label UnitMovesLabel;
    public Label UnitDamageLabel;
    public Label UnitResistanceLabel;
    public Label UnitHitsLabel;
    public Label UnitAlignmentLabel;
    public Label UnitLevelLabel;
    public Label StructureNameLabel;
    public Label UnitCostLabel;
    public Label UnitLabel;
    public Label UnitTurnsLabel;
    public CheckBox Production1CheckBox;
    public CheckBox Production2CheckBox;
    public CheckBox Production3CheckBox;
    public CheckBox Production4CheckBox;
    public CheckBox Production5CheckBox;
    public CheckBox Production6CheckBox;
    public CheckBox Production7CheckBox;
    public CheckBox Production8CheckBox;
    public CheckBox Production9CheckBox;
    public CheckBox Production10CheckBox;
    public CheckBox Production11CheckBox;
    public CheckBox Production12CheckBox;
    public CheckBox Action4CheckBox;
    public CheckBox Action5CheckBox;
    public CheckBox Action6CheckBox;
    public CheckBox Action1CheckBox;
    public CheckBox Action2CheckBox;
    public CheckBox Action3CheckBox;
    public Panel UpgradePanel;
    public Label UpgradeLabel;
    public Memo UpgradeMemo;
    public Image Oval;
    public Image LeftShadow;
    public Image RightShadow;
    public Button CityButton3;
    public Button CityButton1;
    public ListBox QueueList;
    public Panel ActivityPanel;
    public Panel InfoPanel2;
    public Label ActiveActionLabel;
    public Label ActiveActionTurnsLabel;
    public Panel ActivityPanel2;
    public Gauge ActiveActionTurnsBar;
    public Button CityButton2;
    public Panel InfoPanel;
    public Panel PopulationPanel;
    public Memo PopulationMemo;
    public Memo PopulationHeader;
    public Memo IncomeMemo;
    public Memo IncomeHeader;
    public Memo EnhancementsMemo;
    public Memo EnhancementsHeader;
    public Frame RightPillar;
    public Frame LeftPillar;
    public Label QueueLabel;
    public CheckBox LoopQueueCheckBox;
    public Label RepeatLabel;
    public Panel ActiveActionPanel;
    public Image CurrentActionImage;
    public Panel UnitPictureFramePanel;
    public Image UpgradePicture;
    public Label UpgradeCostLabel;
    public Label UpgradeTurnsLabel;
    public Panel MigratePanel;
    public Label MigrateTurnsLabel;
    public ListBox<MigrationItem> MigrationRaceList;
    public ListBox MigrationRelationList;
    public ListBox MigrationTurnsList;
    public VerticalScrollBar MigrationScrollBar;
    public Label CurrentRaceLabel;
    public Label CurrentRelationLabel;
    public Label UpgradeInfoLabel;
    public Memo IncomeGoldMemo;
    public Label MigrateInfoLabel;
    public UserDrawBox MapView;
    public Button NextCityButton;
    public Button PreviousCityButton;
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
            GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWCityScreen.pas:766
    public void LoadUnitImages(int index)
    {
        int graphics;
        graphics = GamePack.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _images.Add(graphics);
    }

    // AoWCityScreen.pas:775
    public void ResetChecks()
    {
        Production1CheckBox.Checked = false;
        Production2CheckBox.Checked = false;
        Production3CheckBox.Checked = false;
        Production4CheckBox.Checked = false;
        Production5CheckBox.Checked = false;
        Production6CheckBox.Checked = false;
        Production7CheckBox.Checked = false;
        Production8CheckBox.Checked = false;
        Production9CheckBox.Checked = false;
        Production10CheckBox.Checked = false;
        Production11CheckBox.Checked = false;
        Production12CheckBox.Checked = false;
        Action1CheckBox.Checked = false;
        Action2CheckBox.Checked = false;
        Action3CheckBox.Checked = false;
        Action4CheckBox.Checked = false;
        Action5CheckBox.Checked = false;
        Action6CheckBox.Checked = false;
    }

    // AoWCityScreen.pas:797
    public void Reset()
    {
        ReleaseImages();
        _mode = 0;
        InfoPanel.Visible = true;
        InfoPanel2.Visible = true;
        ActivityPanel.Visible = false;
        ActivityPanel2.Visible = false;
        UnitPanel.Visible = false;
        UpgradePanel.Visible = false;
        MigratePanel.Visible = false;
        CityButton1.Visible = true;
        var options = AbilityRules.TranslateResourceString(GameText.OptionsText);
        CityButton1.Text = options;
        CityButton2.Visible = false;
        PreviousCityButton.Visible = !CityButton2.Visible;
        NextCityButton.Visible = !CityButton2.Visible;
        LeftShadow.Visible = GameTables.GameEngine.AnimationDetail != 0;
        RightShadow.Visible = GameTables.GameEngine.AnimationDetail != 0;
        Oval.Visible = GameTables.GameEngine.AnimationDetail != 0;
        _migrationSort = 0;
        _productionIndex = 0;
        QueueList.Selected = -1;
        FreePreview();
        var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
        QueueLabel.Text = queue;
        var repeat = AbilityRules.TranslateResourceString(GameText.RepeatText);
        RepeatLabel.Text = repeat;
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
                InfoPanel.Visible = true;
                InfoPanel2.Visible = true;
                ActivityPanel.Visible = false;
                ActivityPanel2.Visible = false;
                var change = AbilityRules.TranslateResourceString(GameText.ChangeText);
                CityButton1.Text = change;
                CityButton1.Visible = true;
                CityButton2.Visible = false;
                PreviousCityButton.Visible = !CityButton2.Visible;
                NextCityButton.Visible = !CityButton2.Visible;
                var exit = AbilityRules.TranslateResourceString(GameText.ExitText);
                CityButton3.Text = exit;
                QueueList.Selected = -1;
                LeftPillar.Visible = true;
                RightPillar.Visible = true;
                MapView.WinHeight = InfoPanel.WinTop - MapView.WinTop;
                break;
            case 1:
                QueueList.Selected = -1;
                InfoPanel.Visible = false;
                InfoPanel2.Visible = false;
                ActivityPanel.Visible = true;
                ActivityPanel2.Visible = true;
                var info = AbilityRules.TranslateResourceString(GameText.InfoText);
                CityButton1.Text = info;
                var done = AbilityRules.TranslateResourceString(GameText.DoneText);
                CityButton3.Text = done;
                LeftPillar.Visible = false;
                RightPillar.Visible = false;
                MapView.WinHeight = ActivityPanel.WinTop - MapView.WinTop;
                MigrationRaceList.CaptureInput = true;
                MigrationTurnsList.CaptureInput = true;
                MigrationRelationList.CaptureInput = true;
                break;
            case 2:
                ResetChecks();
                QueueList.Selected = -1;
                InfoPanel.Visible = false;
                InfoPanel2.Visible = false;
                ActivityPanel.Visible = true;
                ActivityPanel2.Visible = true;
                LeftPillar.Visible = false;
                RightPillar.Visible = false;
                var infoText = AbilityRules.TranslateResourceString(GameText.InfoText);
                CityButton1.Text = infoText;
                var doneText = AbilityRules.TranslateResourceString(GameText.DoneText);
                CityButton3.Text = doneText;
                MigrationRaceList.CaptureInput = false;
                MigrationTurnsList.CaptureInput = false;
                MigrationRelationList.CaptureInput = false;
                MigrationRaceList.Selected = -1;
                MigrationTurnsList.Selected = -1;
                MigrationRelationList.Selected = -1;
                break;
        }
    }

    // AoWCityScreen.pas:892
    public void SortMigration()
    {
        switch (_migrationSort)
        {
            case 0:
                StringSorting.SortStringsNumericalAscending(MigrationTurnsList.Items, [MigrationRaceList.Items, MigrationRelationList.Items]);
                break;
            case 1:
                StringSorting.SortStringsNumericalDescending(MigrationTurnsList.Items, [MigrationRaceList.Items, MigrationRelationList.Items]);
                break;
            case 2:
                StringSorting.SortStringsAscending(MigrationRaceList.Items, [MigrationTurnsList.Items, MigrationRelationList.Items]);
                break;
            case 3:
                StringSorting.SortStringsDescending(MigrationRaceList.Items, [MigrationTurnsList.Items, MigrationRelationList.Items]);
                break;
            case 4:
                StringSorting.SortStringsAscending(MigrationRelationList.Items, [MigrationTurnsList.Items, MigrationRaceList.Items]);
                break;
            case 5:
                StringSorting.SortStringsDescending(MigrationRelationList.Items, [MigrationTurnsList.Items, MigrationRaceList.Items]);
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
        Action1CheckBox.Checked = true;
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
                StrategicMap.Current.TutorialControl.ShowMessage(58, true, true);
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
        if (index >= 0 && index < MigrationRaceList.Items.Count)
        {
            item = MigrationRaceList.Items.Objects[index];
            MigrationRaceList.Selected = index;
            MigrationTurnsList.Selected = index;
            MigrationRelationList.Selected = index;
            MigrateTurnsLabel.Text = IntToStr(item.Time);
            var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[_city.Race]);
            var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNames, -15]);
            MigrateInfoLabel.Text = format;
            MigrateInfoLabel.Text = MigrateInfoLabel.Text + "   ";
            var raceNamesText = AbilityRules.TranslateResourceString(GameTables.RaceNames[item.Race]);
            var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNamesText, 10]);
            MigrateInfoLabel.Text = MigrateInfoLabel.Text + formattedText;
            CityButton1.Visible = true;
            var migrate = AbilityRules.TranslateResourceString(s_migrateText);
            CityButton1.Text = migrate;
        }
        else
        {
            MigrationRaceList.Selected = -1;
            MigrationTurnsList.Selected = -1;
            MigrationRelationList.Selected = -1;
            MigrateInfoLabel.Text = "";
            MigrateTurnsLabel.Text = "-";
            UpgradeInfoLabel.Text = "";
            CityButton1.Visible = false;
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
        UpgradePanel.Visible = false;
        UnitPanel.Visible = false;
        MigratePanel.Visible = false;
        UpgradePicture.CurrentImage = 21;
        UpgradeCostLabel.Text = "-";
        UpgradeTurnsLabel.Text = "-";
        UpgradeInfoLabel.Text = "";
        CityButton1.Visible = false;
        CityButton2.Visible = false;
        switch (_action)
        {
            case CityAction.Trade:
                CityButton1.Visible = true;
                var produceMerchandise = AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText);
                CityButton1.Text = produceMerchandise;
                var queueLocked = _city.Production.GetQueueLocked();
                CityButton2.Visible = !queueLocked;
                var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queue;
                var cityTrade = AbilityRules.TranslateResourceString(s_cityTradeText);
                UpgradeMemo.Items.Text = cityTrade;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 20;
                var produceMerchandiseText = AbilityRules.TranslateResourceString(GameText.ProduceMerchandiseText);
                UpgradeLabel.Text = produceMerchandiseText;
                break;
            case CityAction.Upgrade:
                CityButton1.Visible = true;
                var upgrade = AbilityRules.TranslateResourceString(GameText.UpgradeText);
                CityButton1.Text = upgrade;
                var fProductionQueueLocked = _city.Production.GetQueueLocked();
                CityButton2.Visible = !fProductionQueueLocked;
                var queueText = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queueText;
                var cityUpgrade = AbilityRules.TranslateResourceString(s_cityUpgradeText);
                UpgradeMemo.Items.Text = cityUpgrade;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 23;
                var upgradeCost = _city.UpgradeCost();
                UpgradeCostLabel.Text = IntToStr(upgradeCost);
                var upgradeTime = _city.UpgradeTime();
                UpgradeTurnsLabel.Text = IntToStr(upgradeTime);
                var upgradeText = AbilityRules.TranslateResourceString(GameText.UpgradeText);
                UpgradeLabel.Text = upgradeText;
                var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
                var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNamesPlural, 5]);
                UpgradeInfoLabel.Text = format;
                break;
            case CityAction.Fortify:
                CityButton1.Visible = true;
                var fortify = AbilityRules.TranslateResourceString(GameText.FortifyText);
                CityButton1.Text = fortify;
                var queueLocked2 = _city.Production.GetQueueLocked();
                CityButton2.Visible = !queueLocked2;
                var queue3 = AbilityRules.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queue3;
                var cityFortify = AbilityRules.TranslateResourceString(s_cityFortifyText);
                UpgradeMemo.Items.Text = cityFortify;
                UpgradePanel.Visible = true;
                if (_city.WallType == 0)
                {
                    UpgradePicture.CurrentImage = 21;
                }
                else
                {
                    UpgradePicture.CurrentImage = 22;
                }

                if (_city.FortifyCost() > 0)
                {
                    var fortifyCost = _city.FortifyCost();
                    UpgradeCostLabel.Text = IntToStr(fortifyCost);
                    var fortifyTime = _city.FortifyTime();
                    UpgradeTurnsLabel.Text = IntToStr(fortifyTime);
                }
                else
                {
                    UpgradeCostLabel.Text = "-";
                    UpgradeTurnsLabel.Text = "-";
                }

                var fortifyText = AbilityRules.TranslateResourceString(GameText.FortifyText);
                UpgradeLabel.Text = fortifyText;
                var raceNames = AbilityRules.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationPlusFormat), [raceNames, 5]);
                UpgradeInfoLabel.Text = formattedText;
                break;
            case CityAction.Migrate:
                MigratePanel.Visible = true;
                SelectMigration(0);
                break;
            case CityAction.Loot:
                CityButton1.Visible = true;
                var loot = AbilityRules.TranslateResourceString(GameText.LootText);
                CityButton1.Text = loot;
                var cityLoot = AbilityRules.TranslateResourceString(s_cityLootText);
                UpgradeMemo.Items.Text = cityLoot;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 25;
                var lootText = AbilityRules.TranslateResourceString(GameText.LootText);
                UpgradeLabel.Text = lootText;
                var raceNamesText = AbilityRules.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNamesText, -30]);
                UpgradeInfoLabel.Text = format2;
                break;
            case CityAction.Raze:
                CityButton1.Visible = true;
                var raze = AbilityRules.TranslateResourceString(GameText.RazeText);
                CityButton1.Text = raze + "...";
                var cityRaze = AbilityRules.TranslateResourceString(s_cityRazeText);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                var razeText = AbilityRules.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                var raceNames3 = AbilityRules.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_relationMinusFormat), [raceNames3, -30]);
                UpgradeInfoLabel.Text = format3;
                break;
            case CityAction.Research:
            case CityAction.Produce:
                U = _city.Production.CreateUnitProduction(_productionIndex);
                UnitPanel.Visible = true;
                var attack = U.GetAttack();
                UnitAttackLabel.Text = IntToStr(attack);
                var defense = U.GetDefense();
                UnitDefenseLabel.Text = IntToStr(defense);
                var resistance = U.GetResistance();
                UnitResistanceLabel.Text = IntToStr(resistance);
                var damage = U.GetDamage();
                UnitDamageLabel.Text = IntToStr(damage);
                var hits = U.GetHits();
                UnitHitsLabel.Text = IntToStr(hits);
                var moves = U.GetMoves();
                UnitMovesLabel.Text = IntToStr(moves);
                var moveTypes = U.GetMoveTypes();
                var moveTypesToInt32 = DialogHosts.General.MoveTypesToInt32(moveTypes);
                UnitMovesIcon.CurrentImage = moveTypesToInt32;
                var alignment = U.GetAlignment();
                var alignmentText = AbilityRules.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                UnitAlignmentLabel.Text = format4;
                var unitLevel = U.GetUnitLevel();
                var format5 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                var upkeep = U.GetUpkeep();
                var format6 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                UnitLevelLabel.Text = format5 + " / " + format6;
                if (GameTables.GameEngine.Debug)
                {
                    var unitIndexAIPriority = RosterTables.GetUnitIndexAIPriority(U.UnitResource.UnitResourceIndex(), _city.Player, 0);
                    StrategicMap.Current.ShowMessage("AI Prio:" + IntToStr(unitIndexAIPriority));
                }

                UnitAbilitiesList.Items.Clear();
                U.ListAbilities(UnitAbilitiesList.Items);
                if (_action == CityAction.Produce)
                {
                    CityButton1.Visible = true;
                    var produce = AbilityRules.TranslateResourceString(GameText.ProduceText);
                    CityButton1.Text = produce;
                    var queueLocked3 = _city.Production.GetQueueLocked();
                    CityButton2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateResourceString(GameText.QueueText);
                    CityButton2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ProduceFormat), [name]);
                    UnitLabel.Text = format7;
                    var unitProductionGold = _city.Production.UnitProductionGold(_productionIndex);
                    UnitCostLabel.Text = IntToStr(unitProductionGold);
                    var unitProductionTime = _city.Production.UnitProductionTime(_productionIndex);
                    UnitTurnsLabel.Text = IntToStr(unitProductionTime);
                }
                else
                {
                    CityButton1.Visible = true;
                    var install = AbilityRules.TranslateResourceString(GameText.InstallText);
                    CityButton1.Text = install;
                    var queueLocked3 = _city.Production.GetQueueLocked();
                    CityButton2.Visible = !queueLocked3;
                    var queue4 = AbilityRules.TranslateResourceString(GameText.QueueText);
                    CityButton2.Text = queue4;
                    var name = U.GetName();
                    var format7 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.InstallFormat), [name]);
                    UnitLabel.Text = format7;
                    var unitResearchGold = _city.Production.UnitResearchGold(_productionIndex);
                    UnitCostLabel.Text = IntToStr(unitResearchGold);
                    var unitResearchTime = _city.Production.UnitResearchTime(_productionIndex);
                    UnitTurnsLabel.Text = IntToStr(unitResearchTime);
                }

                U.Release();
                break;
        }

        PreviousCityButton.Visible = false;
        NextCityButton.Visible = false;
    }

    // AoWCityScreen.pas:1099
    public void UpdateInfo()
    {
        string reason;
        var name = _city.GetName();
        StructureNameLabel.Text = name;
        PopulationHeader.Items.Clear();
        var population = AbilityRules.TranslateResourceString(s_populationText);
        PopulationHeader.Items.Add(population + ":");
        var raceNamesPlural = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        PopulationHeader.Items.Add(" " + raceNamesPlural);
        var status = AbilityRules.TranslateResourceString(StatusText);
        PopulationHeader.Items.Add(status + ":");
        var civilStatus = _city.GetCivilStatus();
        var civilStatusText = AbilityRules.TranslateResourceString(CivilStatusTexts[(int)civilStatus]);
        PopulationHeader.Items.Add(" " + civilStatusText);
        PopulationMemo.Items.Clear();
        _city.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHeader.Items.Clear();
        var income = AbilityRules.TranslateResourceString(GameText.IncomeText);
        IncomeHeader.Items.Add(income);
        var xGold = AbilityRules.TranslateResourceString(GameText.GoldFormat);
        var format = AbilityRules.Format(" " + xGold, [IntToStr(_city.Income)]);
        IncomeHeader.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        _city.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
        EnhancementsHeader.Items.Clear();
        var enhancements = AbilityRules.TranslateResourceString(s_enhancementsText);
        EnhancementsHeader.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        _city.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (_migrations != null)
        {
            _migrations?.Dispose();
            _migrations = null;
        }

        _migrations = _city.CreateMigrationSelection();
        var raceNamesPluralText = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        CurrentRaceLabel.Text = raceNamesPluralText;
        var playerRelation = StrategicMap.Current.Races.GetRace(_city.Race).GetPlayerRelation(_city.Player);
        var statusText = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
        CurrentRelationLabel.Text = statusText;
        MigrationRaceList.Items.Clear();
        MigrationTurnsList.Items.Clear();
        MigrationRelationList.Items.Clear();
        if (_migrations != null)
        {
            for (int I = 0, last = _migrations.GetCount() - 1; I <= last; I++)
            {
                var itemRace = _migrations.GetItems(I);
                var raceNamesPlural3 = AbilityRules.TranslateResourceString(GameTables.RaceNamesPluralTexts[itemRace.Race]);
                var migrationItem = _migrations.GetItems(I);
                MigrationRaceList.Items.AddObject(raceNamesPlural3, migrationItem);
                var itemTime = _migrations.GetItems(I);
                MigrationTurnsList.Items.Add(IntToStr(itemTime.Time));
                var itemRelation = _migrations.GetItems(I);
                var status3 = AbilityRules.TranslateResourceString(GameTables.StatusTexts[(int)itemRelation.Relation]);
                MigrationRelationList.Items.Add(status3);
            }
        }

        SortMigration();
        QueueList.Items.Clear();
        for (int I = 0, last = _city.Production.Queue.GetCount() - 1; I <= last; I++)
        {
            var name2 = _city.Production.Queue.GetItem(I).GetName();
            QueueList.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = _city.Production.Queue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = AbilityRules.TranslateResourceString(GameText.EmptyQueueText);
            QueueList.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = _city.Production.Queue.GetCount();
        if (QueueList.Selected >= count)
        {
            var fQueueCount = _city.Production.Queue.GetCount();
            QueueList.Selected = fQueueCount - 1;
            _queueIndex = QueueList.Selected;
        }

        LoopQueueCheckBox.Checked = _city.Production.LoopQueue;
        if (_city.GetActiveAction() != CityAction.None)
        {
            var nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
            ActiveActionLabel.Text = nameProgressive;
            if (_city.Production.Settings.TurnsLeft != -1)
            {
                var formatTurnsLeftString = DurationText.FormatTurnsLeftString(_city.Production.Settings.TurnsLeft);
                ActiveActionTurnsLabel.Text = formatTurnsLeftString;
                ActiveActionTurnsLabel.Visible = true;
                ActiveActionTurnsLabel.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = _city.Production.Settings.TurnsTotal;
                this.ActiveActionTurnsBar.Position = _city.Production.Settings.TurnsTotal - _city.Production.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurnsLabel.Visible = false;
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
                ActiveActionLabel.Text = producingMerchandise;
                var defaultText = AbilityRules.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurnsLabel.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurnsLabel.TextColor = 6783862;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = ActivityImage(CityAction.None);
                CurrentActionImage.CurrentImage = activityImage;
            }
            else
            {
                reason = "";
                var nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
                ActiveActionLabel.Text = nameProgressive;
                var queue = _city.Production.Queue.GetItem(0);
                _city.Production.CanExecuteProduction(queue, ref reason);
                ActiveActionTurnsLabel.Text = reason;
                ActiveActionTurnsLabel.TextColor = 255;
                ActiveActionTurnsBar.Visible = false;
                var activityImage = _city.Production.Queue.GetItem(0).Order.ActivityImage(_city);
                CurrentActionImage.CurrentImage = activityImage;
            }
        }

        if (_mode == 1 && _action != CityAction.None)
        {
            SelectAction(_action);
        }

        Window.Drawn = false;
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
        if (Window.Visible && sender == _city)
        {
            if (_city.Player == StrategicMap.Current.SeatedPlayer)
            {
                UpdateInfo();
            }
            else
            {
                Window.Visible = false;
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

        StrategicMap.Current.TutorialControl.ShowMessage(56, true, true);
        DialogHosts.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
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
        DialogHosts.UIGraphics.CityWindow[image].Show(ActivityPanel2.WinLeft + sender.WinLeft + 2, ActivityPanel2.WinTop + sender.WinTop + 2, lockedSurface);
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
                StrategicMap.Current.TutorialControl.ShowMessage(57, true, true);
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
                            StrategicMap.Current.ShowWarning(reason);
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
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Raze:
                        if (_city.CanRaze(StrategicMap.Current.SeatedPlayer, ref reason))
                        {
                            _city.Raze(StrategicMap.Current.SeatedPlayer);
                            Window.Visible = false;
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
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
                            StrategicMap.Current.ShowWarning(reason);
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
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, false, ref reason))
                        {
                            StrategicMap.Current.TutorialControl.ShowMessage(59, true, true);
                            _city.Produce(_productionIndex, false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
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
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Migrate:
                        race = MigrationRaceList.Items.Objects[MigrationRaceList.Selected].Race;
                        if (_city.CanMigrate(race, ref reason))
                        {
                            StrategicMap.Current.TutorialControl.ShowMessage(66, true, true);
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
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (_city.CanUpgrade(true, ref reason))
                        {
                            _city.Upgrade(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (_city.CanFortify(true, ref reason))
                        {
                            _city.Fortify(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, true, ref reason))
                        {
                            _city.Produce(_productionIndex, true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Research:
                        if (_city.CanResearch(_productionIndex, true, ref reason))
                        {
                            _city.Research(_productionIndex, true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
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
                        StrategicMap.Current.ShowWarning(reason);
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
            Window.Visible = false;
        }
        else
        {
            SetMode(0);
        }

        StrategicMap.Current.TutorialControl.ShowMessage(60, true, true);
    }

    // AoWCityScreen.pas:472
    public void PreDrawProductions(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        Rect R;
        UnitShowInfo info = default;
        Unit U;
        UnitProduction P;
        R = new Rect(ActivityPanel.WinLeft + sender.WinLeft, ActivityPanel.WinTop + sender.WinTop, ActivityPanel2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPanel.WinTop + sender.WinTop + sender.WinHeight);
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
                DialogHosts.UIGraphics.CityWindow[5].Show(R.Left + 2, R.Top + 2, lockedSurface);
                info.Direction = 5;
                if (U != null)
                {
                    U.Show(R.Left - 4, R.Top, lockedSurface, info);
                }

                var lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                DialogHosts.UIGraphics.CityWindow[5].ShowBlended(R.Left + 2, R.Top + 2, lockedSurface, lookupTable);
            }
            else
            {
                DialogHosts.UIGraphics.CityWindow[4].Show(R.Left + 2, R.Top + 2, lockedSurface);
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
            DialogHosts.UIGraphics.CityWindow[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
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
        MigrationRaceList.ListOffset = sender.Position;
        MigrationTurnsList.ListOffset = sender.Position;
        MigrationRelationList.ListOffset = sender.Position;
    }

    // AoWCityScreen.pas:541
    public void UFaceDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        Unit U;
        Rect R;
        U = _city.Production.CreateUnitProduction(_productionIndex);
        if (U != null)
        {
            R = new Rect(UnitPictureFramePanel.WinLeft + sender.WinLeft + 2, UnitPictureFramePanel.WinTop + sender.WinTop + 2, UnitPictureFramePanel.WinLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.WinTop + sender.WinTop + sender.WinHeight - 2);
            U.ShowFace(R, lockedSurface);
            U.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(ListBox sender)
    {
        CityAction action;
        var count = _city.Production.Queue.GetCount();
        if (QueueList.Selected < count && _mode != 0)
        {
            if (QueueList.Selected != -1)
            {
                _queueIndex = QueueList.Selected;
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
                QueueList.Selected = _queueIndex;
                CityButton1.Visible = true;
                var delete = AbilityRules.TranslateResourceString(GameText.DeleteText);
                CityButton1.Text = delete;
                if (_queueIndex == 0 && _city.Production.Status == ProductionControlStatus.Waiting)
                {
                    CityButton2.Visible = true;
                    var execute = AbilityRules.TranslateResourceString(GameText.ExecuteText);
                    CityButton2.Text = execute;
                }
                else
                {
                    CityButton2.Visible = false;
                }

                PreviousCityButton.Visible = false;
                NextCityButton.Visible = false;
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
                R = new Rect(ActiveActionPanel.WinLeft + sender.WinLeft, ActiveActionPanel.WinTop + sender.WinTop, ActiveActionPanel.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.WinTop + sender.WinTop + sender.WinHeight);
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
        _city.Production.SetLoopQueueTo(LoopQueueCheckBox.Checked);
    }

    // AoWCityScreen.pas:631
    public void RacePortraitDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect R;
        R = new Rect(PopulationPanel.WinLeft + sender.WinLeft, PopulationPanel.WinTop + sender.WinTop, PopulationPanel.WinLeft + sender.WinLeft + sender.WinWidth, PopulationPanel.WinTop + sender.WinTop + sender.WinHeight);
        GamePack.Current.RaceResources.GetRaceResource(_city.Race).GetFace().ShowStretched(R, lockedSurface);
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
            R = new Rect(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
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
                Window.Visible = false;
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

        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
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
        if (button == MouseButton.Right && UnitAbilitiesList.Selected > -1)
        {
            var objectValue = UnitAbilitiesList.Items.Objects[UnitAbilitiesList.Selected];
            A = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
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
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            S = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is City)
            {
                index = start + 1;
                ShowCity((City)S);
            }

            index += -1;
            if (index < 0)
            {
                index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
            }
        }
    }

    // AoWCityScreen.pas:726
    public void PNextClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            S = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is City)
            {
                index = start - 1;
                ShowCity((City)S);
            }

            index += 1;
            if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
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

    internal static ImmutableArray<string> CivilStatusTexts { get; } = ["Unruly", "Unrest", "Stable", "Content", "Cheerful", "Oppressed", "Slavery"];
}
