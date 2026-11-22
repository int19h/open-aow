#nullable disable warnings
// Translated from AoWCityScreen.pas.
using System.Collections.Immutable;
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Engine.Nodes;
using Aow.Game.Combat;
using Aow.Game.Draw;
using Aow.Game.Events;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// city, preview, view, action, production, queue, mode, migrations, sort, images
public sealed partial class CityScreen(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Panel UnitPanel { get; private set; }
    public ListBox UnitAbilitiesList { get; private set; }
    public Image UnitMovesIcon { get; private set; }
    public Label UnitAttackLabel { get; private set; }
    public Label UnitDefenseLabel { get; private set; }
    public Label UnitMovesLabel { get; private set; }
    public Label UnitDamageLabel { get; private set; }
    public Label UnitResistanceLabel { get; private set; }
    public Label UnitHitsLabel { get; private set; }
    public Label UnitAlignmentLabel { get; private set; }
    public Label UnitLevelLabel { get; private set; }
    public Label StructureNameLabel { get; private set; }
    public Label UnitCostLabel { get; private set; }
    public Label UnitLabel { get; private set; }
    public Label UnitTurnsLabel { get; private set; }
    public CheckBox Production1CheckBox { get; private set; }
    public CheckBox Production2CheckBox { get; private set; }
    public CheckBox Production3CheckBox { get; private set; }
    public CheckBox Production4CheckBox { get; private set; }
    public CheckBox Production5CheckBox { get; private set; }
    public CheckBox Production6CheckBox { get; private set; }
    public CheckBox Production7CheckBox { get; private set; }
    public CheckBox Production8CheckBox { get; private set; }
    public CheckBox Production9CheckBox { get; private set; }
    public CheckBox Production10CheckBox { get; private set; }
    public CheckBox Production11CheckBox { get; private set; }
    public CheckBox Production12CheckBox { get; private set; }
    public CheckBox Action4CheckBox { get; private set; }
    public CheckBox Action5CheckBox { get; private set; }
    public CheckBox Action6CheckBox { get; private set; }
    public CheckBox Action1CheckBox { get; private set; }
    public CheckBox Action2CheckBox { get; private set; }
    public CheckBox Action3CheckBox { get; private set; }
    public Panel UpgradePanel { get; private set; }
    public Label UpgradeLabel { get; private set; }
    public Memo UpgradeMemo { get; private set; }
    public Image Oval { get; private set; }
    public Image LeftShadow { get; private set; }
    public Image RightShadow { get; private set; }
    public Button CityButton3 { get; private set; }
    public Button CityButton1 { get; private set; }
    public ListBox QueueList { get; private set; }
    public Panel ActivityPanel { get; private set; }
    public Panel InfoPanel2 { get; private set; }
    public Label ActiveActionLabel { get; private set; }
    public Label ActiveActionTurnsLabel { get; private set; }
    public Panel ActivityPanel2 { get; private set; }
    public Gauge ActiveActionTurnsBar { get; private set; }
    public Button CityButton2 { get; private set; }
    public Panel InfoPanel { get; private set; }
    public Panel PopulationPanel { get; private set; }
    public Memo PopulationMemo { get; private set; }
    public Memo PopulationHeader { get; private set; }
    public Memo IncomeMemo { get; private set; }
    public Memo IncomeHeader { get; private set; }
    public Memo EnhancementsMemo { get; private set; }
    public Memo EnhancementsHeader { get; private set; }
    public Frame RightPillar { get; private set; }
    public Frame LeftPillar { get; private set; }
    public Label QueueLabel { get; private set; }
    public CheckBox LoopQueueCheckBox { get; private set; }
    public Label RepeatLabel { get; private set; }
    public Panel ActiveActionPanel { get; private set; }
    public Image CurrentActionImage { get; private set; }
    public Panel UnitPictureFramePanel { get; private set; }
    public Image UpgradePicture { get; private set; }
    public Label UpgradeCostLabel { get; private set; }
    public Label UpgradeTurnsLabel { get; private set; }
    public Panel MigratePanel { get; private set; }
    public Label MigrateTurnsLabel { get; private set; }
    public ListBox<MigrationItem> MigrationRaceList { get; private set; }
    public ListBox MigrationRelationList { get; private set; }
    public ListBox MigrationTurnsList { get; private set; }
    public VerticalScrollBar MigrationScrollBar { get; private set; }
    public Label CurrentRaceLabel { get; private set; }
    public Label CurrentRelationLabel { get; private set; }
    public Label UpgradeInfoLabel { get; private set; }
    public Memo IncomeGoldMemo { get; private set; }
    public Label MigrateInfoLabel { get; private set; }
    public UserDrawBox MapView { get; private set; }
    public Button NextCityButton { get; private set; }
    public Button PreviousCityButton { get; private set; }

    private City? _city;
    private CombatMap? _preview;
    private int _viewX;
    private int _viewY;
    private CityAction _action;
    private int _productionIndex;
    private int _queueIndex;
    private int _mode;
    private MigrationList? _migrations;
    private int _migrationSort;
    private Int32List _images;

    // AoWCityScreen.pas:748
    public void FreePreview()
    {
        if (_preview is not null)
        {
            _preview?.Dispose();
            _preview = null;
        }
    }

    // AoWCityScreen.pas:757
    public void ReleaseImages()
    {
        for (int i = _images.Count - 1; i >= 0; i--)
        {
            int image = _images[i];
            _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWCityScreen.pas:766
    public void LoadUnitImages(int index)
    {
        int graphics = GamePack.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _ = _images.Add(graphics);
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
        string options = UI.Engine.TranslateResourceString(GameText.OptionsText);
        CityButton1.Text = options;
        CityButton2.Visible = false;
        PreviousCityButton.Visible = !CityButton2.Visible;
        NextCityButton.Visible = !CityButton2.Visible;
        LeftShadow.Visible = UI.Engine.AnimationDetail != 0;
        RightShadow.Visible = UI.Engine.AnimationDetail != 0;
        Oval.Visible = UI.Engine.AnimationDetail != 0;
        _migrationSort = 0;
        _productionIndex = 0;
        QueueList.Selected = -1;
        FreePreview();
        string queue = UI.Engine.TranslateResourceString(GameText.QueueText);
        QueueLabel.Text = queue;
        string repeat = UI.Engine.TranslateResourceString(GameText.RepeatText);
        RepeatLabel.Text = repeat;
        if (_migrations is not null)
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
        UI.General.Manager.OverlayPriority = 0;
    }

    // AoWCityScreen.pas:836
    public void SetMode(int mode)
    {
        _mode = mode;
        switch (_mode)
        {
            case 0:
                InfoPanel.Visible = true;
                InfoPanel2.Visible = true;
                ActivityPanel.Visible = false;
                ActivityPanel2.Visible = false;
                string change = UI.Engine.TranslateResourceString(GameText.ChangeText);
                CityButton1.Text = change;
                CityButton1.Visible = true;
                CityButton2.Visible = false;
                PreviousCityButton.Visible = !CityButton2.Visible;
                NextCityButton.Visible = !CityButton2.Visible;
                string exit = UI.Engine.TranslateResourceString(GameText.ExitText);
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
                string info = UI.Engine.TranslateResourceString(GameText.InfoText);
                CityButton1.Text = info;
                string done = UI.Engine.TranslateResourceString(GameText.DoneText);
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
                string infoText = UI.Engine.TranslateResourceString(GameText.InfoText);
                CityButton1.Text = infoText;
                string doneText = UI.Engine.TranslateResourceString(GameText.DoneText);
                CityButton3.Text = doneText;
                MigrationRaceList.CaptureInput = false;
                MigrationTurnsList.CaptureInput = false;
                MigrationRelationList.CaptureInput = false;
                MigrationRaceList.Selected = -1;
                MigrationTurnsList.Selected = -1;
                MigrationRelationList.Selected = -1;
                break;
            default:
                break;
        }
    }

    // AoWCityScreen.pas:892
    public void SortMigration()
    {
        switch (_migrationSort)
        {
            case 0:
                MigrationTurnsList.Items.SortStringsNumericalAscending(linkedStrings: [MigrationRaceList.Items, MigrationRelationList.Items]);
                break;
            case 1:
                MigrationTurnsList.Items.SortStringsNumericalDescending(linkedStrings: [MigrationRaceList.Items, MigrationRelationList.Items]);
                break;
            case 2:
                MigrationRaceList.Items.SortStringsAscending(linkedStrings: [MigrationTurnsList.Items, MigrationRelationList.Items]);
                break;
            case 3:
                MigrationRaceList.Items.SortStringsDescending(linkedStrings: [MigrationTurnsList.Items, MigrationRelationList.Items]);
                break;
            case 4:
                MigrationRelationList.Items.SortStringsAscending(linkedStrings: [MigrationTurnsList.Items, MigrationRaceList.Items]);
                break;
            case 5:
                MigrationRelationList.Items.SortStringsDescending(linkedStrings: [MigrationTurnsList.Items, MigrationRaceList.Items]);
                break;
            default:
                break;
        }
    }

    // AoWCityScreen.pas:904
    public int ActivityImage(CityAction action)
    {
        return action switch
        {
            CityAction.Trade => 40,
            CityAction.Upgrade => 43,
            CityAction.Fortify => _city.WallType == 0 ? 41 : 42,
            CityAction.Migrate => 44,
            CityAction.Loot => 45,
            CityAction.Raze => 46,
            CityAction.Produce => 4,
            CityAction.Research => 5,
            CityAction.None or _ => 40,
        };
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
        UnitProduction? p = _city.Production.UnitProductions.GetItem(index);
        if (p is not null)
        {
            _productionIndex = index;
            if (p.Researched)
            {
                SelectAction(CityAction.Produce);
                StrategicMap.Current.TutorialControl.ShowMessage(58, immediate: true, onceOnly: true);
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
        if (index >= 0 && index < MigrationRaceList.Items.Count)
        {
            MigrationItem? item = MigrationRaceList.Items.Objects[index];
            MigrationRaceList.Selected = index;
            MigrationTurnsList.Selected = index;
            MigrationRelationList.Selected = index;
            MigrateTurnsLabel.Text = item.Time.ToString(CultureInfo.InvariantCulture);
            string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
            string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), arguments: [raceNames, -15]);
            MigrateInfoLabel.Text = format;
            MigrateInfoLabel.Text = $"{MigrateInfoLabel.Text}   ";
            string raceNamesText = UI.Engine.TranslateResourceString(GameTables.RaceNames[item.Race]);
            string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), arguments: [raceNamesText, 10]);
            MigrateInfoLabel.Text = MigrateInfoLabel.Text + formattedText;
            CityButton1.Visible = true;
            string migrate = UI.Engine.TranslateResourceString(s_migrateText);
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
                string produceMerchandise = UI.Engine.TranslateResourceString(GameText.ProduceMerchandiseText);
                CityButton1.Text = produceMerchandise;
                bool queueLocked = _city.Production.GetQueueLocked();
                CityButton2.Visible = !queueLocked;
                string queue = UI.Engine.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queue;
                string cityTrade = UI.Engine.TranslateResourceString(s_cityTradeText);
                UpgradeMemo.Items.Text = cityTrade;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 20;
                string produceMerchandiseText = UI.Engine.TranslateResourceString(GameText.ProduceMerchandiseText);
                UpgradeLabel.Text = produceMerchandiseText;
                break;
            case CityAction.Upgrade:
                CityButton1.Visible = true;
                string upgrade = UI.Engine.TranslateResourceString(GameText.UpgradeText);
                CityButton1.Text = upgrade;
                bool fProductionQueueLocked = _city.Production.GetQueueLocked();
                CityButton2.Visible = !fProductionQueueLocked;
                string queueText = UI.Engine.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queueText;
                string cityUpgrade = UI.Engine.TranslateResourceString(s_cityUpgradeText);
                UpgradeMemo.Items.Text = cityUpgrade;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 23;
                int upgradeCost = _city.UpgradeCost();
                UpgradeCostLabel.Text = upgradeCost.ToString(CultureInfo.InvariantCulture);
                int upgradeTime = _city.UpgradeTime();
                UpgradeTurnsLabel.Text = upgradeTime.ToString(CultureInfo.InvariantCulture);
                string upgradeText = UI.Engine.TranslateResourceString(GameText.UpgradeText);
                UpgradeLabel.Text = upgradeText;
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), arguments: [raceNamesPlural, 5]);
                UpgradeInfoLabel.Text = format;
                break;
            case CityAction.Fortify:
                CityButton1.Visible = true;
                string fortify = UI.Engine.TranslateResourceString(GameText.FortifyText);
                CityButton1.Text = fortify;
                bool queueLocked2 = _city.Production.GetQueueLocked();
                CityButton2.Visible = !queueLocked2;
                string queue3 = UI.Engine.TranslateResourceString(GameText.QueueText);
                CityButton2.Text = queue3;
                string cityFortify = UI.Engine.TranslateResourceString(s_cityFortifyText);
                UpgradeMemo.Items.Text = cityFortify;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = _city.WallType == 0 ? 21 : 22;

                if (_city.FortifyCost() > 0)
                {
                    int fortifyCost = _city.FortifyCost();
                    UpgradeCostLabel.Text = fortifyCost.ToString(CultureInfo.InvariantCulture);
                    int fortifyTime = _city.FortifyTime();
                    UpgradeTurnsLabel.Text = fortifyTime.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    UpgradeCostLabel.Text = "-";
                    UpgradeTurnsLabel.Text = "-";
                }

                string fortifyText = UI.Engine.TranslateResourceString(GameText.FortifyText);
                UpgradeLabel.Text = fortifyText;
                string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), arguments: [raceNames, 5]);
                UpgradeInfoLabel.Text = formattedText;
                break;
            case CityAction.Migrate:
                MigratePanel.Visible = true;
                SelectMigration(0);
                break;
            case CityAction.Loot:
                CityButton1.Visible = true;
                string loot = UI.Engine.TranslateResourceString(GameText.LootText);
                CityButton1.Text = loot;
                string cityLoot = UI.Engine.TranslateResourceString(s_cityLootText);
                UpgradeMemo.Items.Text = cityLoot;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 25;
                string lootText = UI.Engine.TranslateResourceString(GameText.LootText);
                UpgradeLabel.Text = lootText;
                string raceNamesText = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), arguments: [raceNamesText, -30]);
                UpgradeInfoLabel.Text = format2;
                break;
            case CityAction.Raze:
                CityButton1.Visible = true;
                string raze = UI.Engine.TranslateResourceString(GameText.RazeText);
                CityButton1.Text = $"{raze}...";
                string cityRaze = UI.Engine.TranslateResourceString(s_cityRazeText);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                string razeText = UI.Engine.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                string raceNames3 = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), arguments: [raceNames3, -30]);
                UpgradeInfoLabel.Text = format3;
                break;
            case CityAction.Research:
            case CityAction.Produce:
                Unit? u = _city.Production.CreateUnitProduction(_productionIndex);
                UnitPanel.Visible = true;
                int attack = u.GetAttack();
                UnitAttackLabel.Text = attack.ToString(CultureInfo.InvariantCulture);
                int defense = u.GetDefense();
                UnitDefenseLabel.Text = defense.ToString(CultureInfo.InvariantCulture);
                int resistance = u.GetResistance();
                UnitResistanceLabel.Text = resistance.ToString(CultureInfo.InvariantCulture);
                int damage = u.GetDamage();
                UnitDamageLabel.Text = damage.ToString(CultureInfo.InvariantCulture);
                int hits = u.GetHits();
                UnitHitsLabel.Text = hits.ToString(CultureInfo.InvariantCulture);
                int moves = u.Moves;
                UnitMovesLabel.Text = moves.ToString(CultureInfo.InvariantCulture);
                MoveTypes moveTypes = u.GetMoveTypes();
                int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                UnitMovesIcon.CurrentImage = moveTypesToInt32;
                Units.Alignment alignment = u.GetAlignment();
                string alignmentText = UI.Engine.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                string format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), arguments: [alignmentText]);
                UnitAlignmentLabel.Text = format4;
                int unitLevel = u.UnitLevel;
                string format5 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.LevelFormat), arguments: [unitLevel.ToString(CultureInfo.InvariantCulture)]);
                int upkeep = u.GetUpkeep();
                string format6 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), arguments: [upkeep]);
                UnitLevelLabel.Text = $"{format5} / {format6}";
                if (UI.Engine.Debug)
                {
                    int unitIndexAIPriority = RosterTables.GetUnitIndexAIPriority(u.UnitResource.UnitResourceIndex(), _city.Player, relation: 0, engine: UI.Engine);
                    StrategicMap.Current.ShowMessage($"AI Prio:{unitIndexAIPriority.ToString(CultureInfo.InvariantCulture)}");
                }

                UnitAbilitiesList.Items.Clear();
                u.ListAbilities(UnitAbilitiesList.Items);
                if (_action == CityAction.Produce)
                {
                    CityButton1.Visible = true;
                    string produce = UI.Engine.TranslateResourceString(GameText.ProduceText);
                    CityButton1.Text = produce;
                    bool queueLocked3 = _city.Production.GetQueueLocked();
                    CityButton2.Visible = !queueLocked3;
                    string queue4 = UI.Engine.TranslateResourceString(GameText.QueueText);
                    CityButton2.Text = queue4;
                    string name = u.GetName();
                    string format7 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.ProduceFormat), arguments: [name]);
                    UnitLabel.Text = format7;
                    int unitProductionGold = _city.Production.UnitProductionGold(_productionIndex);
                    UnitCostLabel.Text = unitProductionGold.ToString(CultureInfo.InvariantCulture);
                    int unitProductionTime = _city.Production.UnitProductionTime(_productionIndex);
                    UnitTurnsLabel.Text = unitProductionTime.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    CityButton1.Visible = true;
                    string install = UI.Engine.TranslateResourceString(GameText.InstallText);
                    CityButton1.Text = install;
                    bool queueLocked3 = _city.Production.GetQueueLocked();
                    CityButton2.Visible = !queueLocked3;
                    string queue4 = UI.Engine.TranslateResourceString(GameText.QueueText);
                    CityButton2.Text = queue4;
                    string name = u.GetName();
                    string format7 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.InstallFormat), arguments: [name]);
                    UnitLabel.Text = format7;
                    int unitResearchGold = _city.Production.UnitResearchGold(_productionIndex);
                    UnitCostLabel.Text = unitResearchGold.ToString(CultureInfo.InvariantCulture);
                    int unitResearchTime = _city.Production.UnitResearchTime(_productionIndex);
                    UnitTurnsLabel.Text = unitResearchTime.ToString(CultureInfo.InvariantCulture);
                }

                _ = u.Release();
                break;
            case CityAction.None:
            default:
                break;
        }

        PreviousCityButton.Visible = false;
        NextCityButton.Visible = false;
    }

    // AoWCityScreen.pas:1099
    public void UpdateInfo()
    {
        string name = _city.GetName();
        StructureNameLabel.Text = name;
        PopulationHeader.Items.Clear();
        string population = UI.Engine.TranslateResourceString(s_populationText);
        _ = PopulationHeader.Items.Add($"{population}:");
        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        _ = PopulationHeader.Items.Add($" {raceNamesPlural}");
        string status = UI.Engine.TranslateResourceString(StatusText);
        _ = PopulationHeader.Items.Add($"{status}:");
        City.CivilStatus civilStatus = _city.GetCivilStatus();
        string civilStatusText = UI.Engine.TranslateResourceString(CivilStatusTexts[(int)civilStatus]);
        _ = PopulationHeader.Items.Add($" {civilStatusText}");
        PopulationMemo.Items.Clear();
        _city.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHeader.Items.Clear();
        string income = UI.Engine.TranslateResourceString(GameText.IncomeText);
        _ = IncomeHeader.Items.Add(income);
        string xGold = UI.Engine.TranslateResourceString(GameText.GoldFormat);
        string format = AbilityRules.Format(" " + xGold, arguments: [((int)_city.Income).ToString(CultureInfo.InvariantCulture)]);
        _ = IncomeHeader.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        _city.ListIncomeInfo(IncomeMemo.Items, values: IncomeGoldMemo.Items);
        EnhancementsHeader.Items.Clear();
        string enhancements = UI.Engine.TranslateResourceString(s_enhancementsText);
        _ = EnhancementsHeader.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        _city.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (_migrations is not null)
        {
            _migrations?.Dispose();
            _migrations = null;
        }

        _migrations = _city.CreateMigrationSelection();
        string raceNamesPluralText = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        CurrentRaceLabel.Text = raceNamesPluralText;
        Status playerRelation = StrategicMap.Current.Races.GetRace(_city.Race).GetPlayerRelation(_city.Player);
        string statusText = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)playerRelation]);
        CurrentRelationLabel.Text = statusText;
        MigrationRaceList.Items.Clear();
        MigrationTurnsList.Items.Clear();
        MigrationRelationList.Items.Clear();
        if (_migrations is not null)
        {
            for (int i = 0, last = _migrations.GetCount() - 1; i <= last; i++)
            {
                MigrationItem itemRace = _migrations.GetItems(i);
                string raceNamesPlural3 = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[itemRace.Race]);
                MigrationItem migrationItem = _migrations.GetItems(i);
                _ = MigrationRaceList.Items.AddObject(raceNamesPlural3, item: migrationItem);
                MigrationItem itemTime = _migrations.GetItems(i);
                _ = MigrationTurnsList.Items.Add(itemTime.Time.ToString(CultureInfo.InvariantCulture));
                MigrationItem itemRelation = _migrations.GetItems(i);
                string status3 = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)itemRelation.Relation]);
                _ = MigrationRelationList.Items.Add(status3);
            }
        }

        SortMigration();
        QueueList.Items.Clear();
        for (int i = 0, last = _city.Production.Queue.GetCount() - 1; i <= last; i++)
        {
            string name2 = _city.Production.Queue.GetItem(i).GetName();
            _ = QueueList.Items.Add($"{(i + 1).ToString(CultureInfo.InvariantCulture)}. {name2}");
        }

        for (int i = _city.Production.Queue.GetCount(); i <= 4; i++)
        {
            string emptyQueue = UI.Engine.TranslateResourceString(GameText.EmptyQueueText);
            _ = QueueList.Items.Add($"{(i + 1).ToString(CultureInfo.InvariantCulture)}. {emptyQueue}");
        }

        int count = _city.Production.Queue.GetCount();
        if (QueueList.Selected >= count)
        {
            int fQueueCount = _city.Production.Queue.GetCount();
            QueueList.Selected = fQueueCount - 1;
            _queueIndex = QueueList.Selected;
        }

        LoopQueueCheckBox.Checked = _city.Production.LoopQueue;
        if (_city.ActiveAction != CityAction.None)
        {
            string nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
            ActiveActionLabel.Text = nameProgressive;
            if (_city.Production.Settings.TurnsLeft != -1)
            {
                string formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: _city.Production.Settings.TurnsLeft);
                ActiveActionTurnsLabel.Text = formatTurnsLeftString;
                ActiveActionTurnsLabel.Visible = true;
                ActiveActionTurnsLabel.TextColor = 0x67_83_76;
                ActiveActionTurnsBar.Min = 0;
                ActiveActionTurnsBar.Max = _city.Production.Settings.TurnsTotal;
                ActiveActionTurnsBar.Position = _city.Production.Settings.TurnsTotal - _city.Production.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurnsLabel.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            int activityImage = _city.ActiveOrder.ActivityImage(_city);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (_city.Production.Queue.GetCount() == 0)
            {
                string producingMerchandise = UI.Engine.TranslateResourceString(GameText.ProducingMerchandiseText);
                ActiveActionLabel.Text = producingMerchandise;
                string defaultText = UI.Engine.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurnsLabel.Text = $"({LowerCase(defaultText)})";
                ActiveActionTurnsLabel.TextColor = 0x67_83_76;
                ActiveActionTurnsBar.Visible = false;
                int activityImage = ActivityImage(CityAction.None);
                CurrentActionImage.CurrentImage = activityImage;
            }
            else
            {
                string reason = "";
                string nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
                ActiveActionLabel.Text = nameProgressive;
                ProductionItem? queue = _city.Production.Queue.GetItem(0);
                _ = _city.Production.CanExecuteProduction(queue, ref reason);
                ActiveActionTurnsLabel.Text = reason;
                ActiveActionTurnsLabel.TextColor = 0x00_00_FF;
                ActiveActionTurnsBar.Visible = false;
                int activityImage = _city.Production.Queue.GetItem(0).Order.ActivityImage(_city);
                CurrentActionImage.CurrentImage = activityImage;
            }
        }

        if (_mode == 1 && _action != CityAction.None)
        {
            SelectAction(_action);
        }

        Window.IsDrawn = false;
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
    public void ShowCity(City? city)
    {
        Reset();
        _city = city;
        UpdateInfo();
        SetMode(0);
        FreePreview();
        for (int i = 0, last = _city.Production.UnitProductions.GetCount() - 1; i <= last; i++)
        {
            LoadUnitImages(_city.Production.UnitProductions.GetItem(i).UnitIndex);
        }

        RandomBattleMapGenerator generator = new(UI.Engine);
        int hexLevel = _city.HexLevel;
        int hexY = _city.HexY;
        _preview = generator.RandomMap(null, _city.HexX, hexY, L: hexLevel);
        generator?.Dispose();
        ArmyHSList armies = new(null) { Holder = UI.Engine };
        _ = _city.ListArmies(armies);
        for (int i = 0, last = armies.GetCount() - 1; i <= last; i++)
        {
            ArmyHexSprite army = armies[i];
            int armyHexY = army.HexY;
            int hexX = army.HexX;
            int fCityHexY = _city.HexY;
            int direction = HexGeometry.DeltaHexToHexNumber(_city.HexX, fCityHexY, hexX, armyHexY);
            for (int j = 0, lastJ = army.Army.Units.Count - 1; j <= lastJ; j++)
            {
                CombatMap.PlaceOverviewUnit(unit: army.Army.Units.GetUnit(j), map: _preview, position: direction, index: j);
            }
        }

        armies?.Dispose();
        MapObject? mapObject = _preview.CenterViewLocation;
        if (_preview.CenterViewLocation is not null)
        {
            int mapObjectHexY = mapObject.HexY;
            HexGeometry.HexToPixel(mapObject.HexX, mapObjectHexY, out _viewX, out _viewY);
            _viewX = _viewX + 16 - (MapView.WinWidth / 2);
            _viewY = _viewY + 16 - (MapView.WinHeight / 2);
        }
        else
        {
            _viewX = 800;
            _viewY = 800;
        }

        StrategicMap.Current.TutorialControl.ShowMessage(56, immediate: true, onceOnly: true);
        UI.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
    }

    // AoWCityScreen.pas:292
    public void CityScreenCreate() => _images = [];

    // AoWCityScreen.pas:297
    public void CityScreenDestroy() => _images?.Dispose();

    // AoWCityScreen.pas:302
    public void ActivityChange(CheckBox sender, int slot)
    {
        UI.General.PlayButtonSound(1);
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
        UI.General.PlayButtonSound(1);
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
        UnitProduction? unitProduction = _city.Production.UnitProductions.GetItem(slot);
        sender.CaptureInput = unitProduction is not null;
    }

    // AoWCityScreen.pas:348
    public void ActivityPreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        int image = ActivityImage((CityAction)((byte)slot + 1));
        UI.Graphics.CityWindow[image].Show(ActivityPanel2.AbsoluteLeft + sender.WinLeft + 2, ActivityPanel2.AbsoluteTop + sender.WinTop + 2, lockedSurface);
    }

    // AoWCityScreen.pas:354
    public void CityBtn1Click()
    {
        switch (_mode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                StrategicMap.Current.TutorialControl.ShowMessage(57, immediate: true, onceOnly: true);
                break;
            case 1:
                string reason = "";
                switch (_action)
                {
                    case CityAction.Trade:
                        if (_city.CanTrade(false, reason))
                        {
                            _ = _city.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Loot:
                        if (_city.CanLoot(reason))
                        {
                            _ = _city.Loot();
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
                            _ = _city.Raze(StrategicMap.Current.SeatedPlayer);
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
                            _ = _city.Upgrade(false);
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
                            _ = _city.Fortify(false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, queue: false, reason))
                        {
                            StrategicMap.Current.TutorialControl.ShowMessage(59, immediate: true, onceOnly: true);
                            _ = _city.Produce(_productionIndex, queue: false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Research:
                        if (_city.CanResearch(_productionIndex, queue: false, ref reason))
                        {
                            _ = _city.Research(_productionIndex, queue: false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Migrate:
                        int race = MigrationRaceList.Items.Objects[MigrationRaceList.Selected].Race;
                        if (_city.CanMigrate(race, reason))
                        {
                            StrategicMap.Current.TutorialControl.ShowMessage(66, immediate: true, onceOnly: true);
                            _ = _city.Migrate(race);
                            SetMode(0);
                        }

                        break;
                    case CityAction.None:
                    default:
                        break;
                }

                break;
            case 2:
                if (_queueIndex != -1)
                {
                    _ = _city.Production.DeleteProduction(_queueIndex);
                }

                break;
            default:
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
                        if (_city.CanTrade(true, reason))
                        {
                            _ = _city.Trade(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Upgrade:
                        if (_city.CanUpgrade(true, ref reason))
                        {
                            _ = _city.Upgrade(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Fortify:
                        if (_city.CanFortify(true, ref reason))
                        {
                            _ = _city.Fortify(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Produce:
                        if (_city.CanProduce(_productionIndex, queue: true, reason))
                        {
                            _ = _city.Produce(_productionIndex, queue: true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.Research:
                        if (_city.CanResearch(_productionIndex, queue: true, ref reason))
                        {
                            _ = _city.Research(_productionIndex, queue: true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case CityAction.None:
                    case CityAction.Migrate:
                    case CityAction.Loot:
                    case CityAction.Raze:
                    default:
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
            default:
                break;
        }
    }

    // AoWCityScreen.pas:465
    public void CityBtn3Click()
    {
        UI.General.PlayButtonSound(0);
        if (_mode == 0)
        {
            Window.Visible = false;
        }
        else
        {
            SetMode(0);
        }

        StrategicMap.Current.TutorialControl.ShowMessage(60, immediate: true, onceOnly: true);
    }

    // AoWCityScreen.pas:472
    public void PreDrawProductions(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        UnitShowInfo info = default;
        Rect r = new(ActivityPanel.AbsoluteLeft + sender.WinLeft, ActivityPanel.AbsoluteTop + sender.WinTop, ActivityPanel2.AbsoluteLeft + sender.WinLeft + sender.WinWidth, ActivityPanel.AbsoluteTop + sender.WinTop + sender.WinHeight);
        // { Original uses two different panels. }
        UnitProduction? p = _city.Production.UnitProductions.GetItem(slot);
        if (p is not null)
        {
            Unit? u = _city.Production.CreateUnitProduction(slot);
            info.Direction = 3;
            info.Overlays = UnitOverlayModes.None;
            info.Animation = 0;
            info.Frame = 0;
            info.Looped = false;
            if (!p.Researched)
            {
                UI.Graphics.CityWindow[5].Show(r.Left + 2, r.Top + 2, lockedSurface);
                info.Direction = 5;
                _ = (u?.Show(r.Left - 4, r.Top, lockedSurface, info));

                BlendTable lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                UI.Graphics.CityWindow[5].ShowBlended(r.Left + 2, r.Top + 2, lockedSurface, table: lookupTable);
            }
            else
            {
                UI.Graphics.CityWindow[4].Show(r.Left + 2, r.Top + 2, lockedSurface);
                _ = (u?.Show(r.Left - 4, r.Top, lockedSurface, info));
            }

            _ = (u?.Release());
        }
        else
        {
            UI.Graphics.CityWindow[62].Show(r.Left + 2, r.Top + 2, lockedSurface);
        }
    }

    // AoWCityScreen.pas:508
    public void SortMigrationRacesClick()
    {
        _migrationSort = _migrationSort == 2 ? 3 : 2;

        SortMigration();
    }

    // AoWCityScreen.pas:515
    public void SortMigrationTurnsClick()
    {
        _migrationSort = _migrationSort == 0 ? 1 : 0;

        SortMigration();
    }

    // AoWCityScreen.pas:522
    public void SortMigrationRelationClick()
    {
        _migrationSort = _migrationSort == 4 ? 5 : 4;

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
        Unit? u = _city.Production.CreateUnitProduction(_productionIndex);
        if (u is not null)
        {
            Rect r = new(UnitPictureFramePanel.AbsoluteLeft + sender.WinLeft + 2, UnitPictureFramePanel.AbsoluteTop + sender.WinTop + 2, UnitPictureFramePanel.AbsoluteLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.AbsoluteTop + sender.WinTop + sender.WinHeight - 2);
            u.ShowFace(r, lockedSurface);
            _ = u.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(ListBox sender)
    {
        int count = _city.Production.Queue.GetCount();
        if (QueueList.Selected < count && _mode != 0)
        {
            if (QueueList.Selected != -1)
            {
                _queueIndex = QueueList.Selected;
                ProductionOrder order = _city.Production.Queue.GetItem(_queueIndex).Order;
                CityAction action = order.CityAction;
                if (order is ProduceUnit produce)
                {
                    _productionIndex = produce.Index;
                    bool condition = _productionIndex < 0;
                    if (!condition)
                    {
                        int fUnitProductionsCount = _city.Production.UnitProductions.GetCount();
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
                string delete = UI.Engine.TranslateResourceString(GameText.DeleteText);
                CityButton1.Text = delete;
                if (_queueIndex == 0 && _city.Production.Status == ProductionControlStatus.Waiting)
                {
                    CityButton2.Visible = true;
                    string execute = UI.Engine.TranslateResourceString(GameText.ExecuteText);
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
            sender.Selected = _mode == 2 ? _queueIndex : -1;
        }
    }

    // AoWCityScreen.pas:597
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        if (_city.Production.Queue.GetCount() != 0)
        {
            if (_city.Production.Queue.GetItem(0).Order.UnitProductionIndex is int index)
            {
                Rect r = new(ActiveActionPanel.AbsoluteLeft + sender.WinLeft, ActiveActionPanel.AbsoluteTop + sender.WinTop, ActiveActionPanel.AbsoluteLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.AbsoluteTop + sender.WinTop + sender.WinHeight);
                Unit? u = _city.Production.CreateUnitProduction(index);
                if (u is not null)
                {
                    info.Direction = 3;
                    info.Overlays = UnitOverlayModes.None;
                    info.Animation = 0;
                    info.Frame = 0;
                    info.Looped = false;
                    if (!_city.Production.UnitProductions.GetItem(index).Researched)
                    {
                        info.Direction = 5;
                    }

                    _ = u.Show(r.Left - 8, r.Top - 5, lockedSurface, info);
                    _ = u.Release();
                }
            }
        }
    }

    // AoWCityScreen.pas:626
    public void LoopQueueBoxChange() => _city.Production.SetLoopQueueTo(LoopQueueCheckBox.Checked);

    // AoWCityScreen.pas:631
    public void RacePortraitDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect r = new(PopulationPanel.AbsoluteLeft + sender.WinLeft, PopulationPanel.AbsoluteTop + sender.WinTop, PopulationPanel.AbsoluteLeft + sender.WinLeft + sender.WinWidth, PopulationPanel.AbsoluteTop + sender.WinTop + sender.WinHeight);
        GamePack.Current.RaceResources.GetRaceResource(_city.Race).GetFace().ShowStretched(r, lockedSurface);
    }

    // AoWCityScreen.pas:641
    public void MapViewDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        if (_preview is not null)
        {
            Rect r = new(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
            int width = r.Right - r.Left;
            int w = width;
            int h = r.Bottom - r.Top;
            int maxPixelX = _preview.MaxPixelX;
            if (_viewX + w + 16 > maxPixelX)
            {
                _viewX = _preview.MaxPixelX - w - 16;
            }

            int maxPixelY = _preview.MaxPixelY;
            if (_viewY + h + 16 > maxPixelY)
            {
                _viewY = _preview.MaxPixelY - h - 16;
            }

            if (_viewX < 16)
            {
                _viewX = 16;
            }

            if (_viewY < 16)
            {
                _viewY = 16;
            }

            LockedSurface viewSurfaceDescription = LockedSurface.CreateFrom(lockedSurface, r);
            _preview.SetExplorationEnabled(false);
            _preview.SetFogEnabled(false);
            _ = _preview.SetViewPort(_viewX, _viewY, lHitPoints: 0, width, r.Bottom - r.Top);
            _preview.UpdateStaticScene();
            _preview.UpdateDynamicScene();
            _preview.DrawStaticScene(in viewSurfaceDescription);
            _preview.DrawDynamicScene(ref viewSurfaceDescription);
        }
    }

    // AoWCityScreen.pas:668
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        switch (key)
        {
            case 37:
                _viewX -= 16;
                MapView.IsDrawn = false;
                break;
            case 39:
                _viewX += 16;
                MapView.IsDrawn = false;
                break;
            case 38:
                _viewY -= 16;
                MapView.IsDrawn = false;
                break;
            case 40:
                _viewY += 16;
                MapView.IsDrawn = false;
                break;
            case 27:
                Window.Visible = false;
                break;
            default:
                break;
        }
    }

    // AoWCityScreen.pas:679
    public void MapViewUpdate()
    {
        CombatMap? preview = _preview;
        preview?.NewFrame();

        MapView.IsDrawn = MapView.IsDrawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // AoWCityScreen.pas:688
    public void DlgHide() => CloseCity();

    // AoWCityScreen.pas:693
    public void UAbilitiesLBMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (button == MouseButton.Right && UnitAbilitiesList.Selected > -1)
        {
            object? objectValue = UnitAbilitiesList.Items.Objects[UnitAbilitiesList.Selected];
            Ability? a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
            if (a is not null)
            {
                InfoBanner.SetInfoBanner(caption: a.GetName(null), banner: UI.InfoBanner, description: a.Description);
            }
        }
    }

    // AoWCityScreen.pas:704
    public void PPrevClick()
    {
        int start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        int index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            PlayerStructure s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is City city)
            {
                index = start + 1;
                ShowCity(city);
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
        int start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        int index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            PlayerStructure s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is City city)
            {
                index = start - 1;
                ShowCity(city);
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
