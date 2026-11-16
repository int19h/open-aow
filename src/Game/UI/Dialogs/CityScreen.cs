#nullable disable
// Translated from AoWCityScreen.pas.
using System.Collections.Immutable;

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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// city, preview, view, action, production, queue, mode, migrations, sort, images
public partial class CityScreen : UIModule
{
    public CityScreen(UIContext ui) : base(ui)
    { }

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
        for (int i = _images.Count - 1; i >= 0; i--)
        {
            int image = _images[i];
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
        UI.General.Manager.OverlayPriority = 0;
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
                result = _city.WallType == 0 ? 41 : 42;

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
        UnitProduction p;
        p = _city.Production.UnitProductions.GetItem(index);
        if (p != null)
        {
            _productionIndex = index;
            if (p.Researched)
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
            string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
            string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), [raceNames, -15]);
            MigrateInfoLabel.Text = format;
            MigrateInfoLabel.Text = MigrateInfoLabel.Text + "   ";
            string raceNamesText = UI.Engine.TranslateResourceString(GameTables.RaceNames[item.Race]);
            string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), [raceNamesText, 10]);
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
        Unit u;
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
                UpgradeCostLabel.Text = IntToStr(upgradeCost);
                int upgradeTime = _city.UpgradeTime();
                UpgradeTurnsLabel.Text = IntToStr(upgradeTime);
                string upgradeText = UI.Engine.TranslateResourceString(GameText.UpgradeText);
                UpgradeLabel.Text = upgradeText;
                string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
                string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), [raceNamesPlural, 5]);
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
                    UpgradeCostLabel.Text = IntToStr(fortifyCost);
                    int fortifyTime = _city.FortifyTime();
                    UpgradeTurnsLabel.Text = IntToStr(fortifyTime);
                }
                else
                {
                    UpgradeCostLabel.Text = "-";
                    UpgradeTurnsLabel.Text = "-";
                }

                string fortifyText = UI.Engine.TranslateResourceString(GameText.FortifyText);
                UpgradeLabel.Text = fortifyText;
                string raceNames = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationPlusFormat), [raceNames, 5]);
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
                string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), [raceNamesText, -30]);
                UpgradeInfoLabel.Text = format2;
                break;
            case CityAction.Raze:
                CityButton1.Visible = true;
                string raze = UI.Engine.TranslateResourceString(GameText.RazeText);
                CityButton1.Text = raze + "...";
                string cityRaze = UI.Engine.TranslateResourceString(s_cityRazeText);
                UpgradeMemo.Items.Text = cityRaze;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                string razeText = UI.Engine.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                string raceNames3 = UI.Engine.TranslateResourceString(GameTables.RaceNames[_city.Race]);
                string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_relationMinusFormat), [raceNames3, -30]);
                UpgradeInfoLabel.Text = format3;
                break;
            case CityAction.Research:
            case CityAction.Produce:
                u = _city.Production.CreateUnitProduction(_productionIndex);
                UnitPanel.Visible = true;
                sbyte attack = u.GetAttack();
                UnitAttackLabel.Text = IntToStr(attack);
                sbyte defense = u.GetDefense();
                UnitDefenseLabel.Text = IntToStr(defense);
                sbyte resistance = u.GetResistance();
                UnitResistanceLabel.Text = IntToStr(resistance);
                sbyte damage = u.GetDamage();
                UnitDamageLabel.Text = IntToStr(damage);
                sbyte hits = u.GetHits();
                UnitHitsLabel.Text = IntToStr(hits);
                sbyte moves = u.GetMoves();
                UnitMovesLabel.Text = IntToStr(moves);
                Set<MoveType> moveTypes = u.GetMoveTypes();
                int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                UnitMovesIcon.CurrentImage = moveTypesToInt32;
                Units.Alignment alignment = u.GetAlignment();
                string alignmentText = UI.Engine.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                string format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                UnitAlignmentLabel.Text = format4;
                sbyte unitLevel = u.GetUnitLevel();
                string format5 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                byte upkeep = u.GetUpkeep();
                string format6 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                UnitLevelLabel.Text = format5 + " / " + format6;
                if (UI.Engine.Debug)
                {
                    int unitIndexAIPriority = RosterTables.GetUnitIndexAIPriority(u.UnitResource.UnitResourceIndex(), _city.Player, 0, engine: UI.Engine);
                    StrategicMap.Current.ShowMessage("AI Prio:" + IntToStr(unitIndexAIPriority));
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
                    string format7 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.ProduceFormat), [name]);
                    UnitLabel.Text = format7;
                    int unitProductionGold = _city.Production.UnitProductionGold(_productionIndex);
                    UnitCostLabel.Text = IntToStr(unitProductionGold);
                    int unitProductionTime = _city.Production.UnitProductionTime(_productionIndex);
                    UnitTurnsLabel.Text = IntToStr(unitProductionTime);
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
                    string format7 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.InstallFormat), [name]);
                    UnitLabel.Text = format7;
                    int unitResearchGold = _city.Production.UnitResearchGold(_productionIndex);
                    UnitCostLabel.Text = IntToStr(unitResearchGold);
                    int unitResearchTime = _city.Production.UnitResearchTime(_productionIndex);
                    UnitTurnsLabel.Text = IntToStr(unitResearchTime);
                }

                u.Release();
                break;
        }

        PreviousCityButton.Visible = false;
        NextCityButton.Visible = false;
    }

    // AoWCityScreen.pas:1099
    public void UpdateInfo()
    {
        string reason;
        string name = _city.GetName();
        StructureNameLabel.Text = name;
        PopulationHeader.Items.Clear();
        string population = UI.Engine.TranslateResourceString(s_populationText);
        PopulationHeader.Items.Add(population + ":");
        string raceNamesPlural = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[_city.Race]);
        PopulationHeader.Items.Add(" " + raceNamesPlural);
        string status = UI.Engine.TranslateResourceString(StatusText);
        PopulationHeader.Items.Add(status + ":");
        City.CivilStatus civilStatus = _city.GetCivilStatus();
        string civilStatusText = UI.Engine.TranslateResourceString(CivilStatusTexts[(int)civilStatus]);
        PopulationHeader.Items.Add(" " + civilStatusText);
        PopulationMemo.Items.Clear();
        _city.ListCivilStatusInfo(PopulationMemo.Items);
        IncomeHeader.Items.Clear();
        string income = UI.Engine.TranslateResourceString(GameText.IncomeText);
        IncomeHeader.Items.Add(income);
        string xGold = UI.Engine.TranslateResourceString(GameText.GoldFormat);
        string format = AbilityRules.Format(" " + xGold, [IntToStr(_city.Income)]);
        IncomeHeader.Items.Add(format);
        IncomeMemo.Items.Clear();
        IncomeGoldMemo.Items.Clear();
        _city.ListIncomeInfo(IncomeMemo.Items, IncomeGoldMemo.Items);
        EnhancementsHeader.Items.Clear();
        string enhancements = UI.Engine.TranslateResourceString(s_enhancementsText);
        EnhancementsHeader.Items.Add(enhancements);
        EnhancementsMemo.Items.Clear();
        _city.ListEnhancementsInfo(EnhancementsMemo.Items);
        if (_migrations != null)
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
        if (_migrations != null)
        {
            for (int i = 0, last = _migrations.GetCount() - 1; i <= last; i++)
            {
                MigrationItem itemRace = _migrations.GetItems(i);
                string raceNamesPlural3 = UI.Engine.TranslateResourceString(GameTables.RaceNamesPluralTexts[itemRace.Race]);
                MigrationItem migrationItem = _migrations.GetItems(i);
                MigrationRaceList.Items.AddObject(raceNamesPlural3, migrationItem);
                MigrationItem itemTime = _migrations.GetItems(i);
                MigrationTurnsList.Items.Add(IntToStr(itemTime.Time));
                MigrationItem itemRelation = _migrations.GetItems(i);
                string status3 = UI.Engine.TranslateResourceString(GameTables.StatusTexts[(int)itemRelation.Relation]);
                MigrationRelationList.Items.Add(status3);
            }
        }

        SortMigration();
        QueueList.Items.Clear();
        for (int i = 0, last = _city.Production.Queue.GetCount() - 1; i <= last; i++)
        {
            string name2 = _city.Production.Queue.GetItem(i).GetName();
            QueueList.Items.Add(IntToStr(i + 1) + ". " + name2);
        }

        for (int i = _city.Production.Queue.GetCount(); i <= 4; i++)
        {
            string emptyQueue = UI.Engine.TranslateResourceString(GameText.EmptyQueueText);
            QueueList.Items.Add(IntToStr(i + 1) + ". " + emptyQueue);
        }

        int count = _city.Production.Queue.GetCount();
        if (QueueList.Selected >= count)
        {
            int fQueueCount = _city.Production.Queue.GetCount();
            QueueList.Selected = fQueueCount - 1;
            _queueIndex = QueueList.Selected;
        }

        LoopQueueCheckBox.Checked = _city.Production.LoopQueue;
        if (_city.GetActiveAction() != CityAction.None)
        {
            string nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
            ActiveActionLabel.Text = nameProgressive;
            if (_city.Production.Settings.TurnsLeft != -1)
            {
                string formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: _city.Production.Settings.TurnsLeft);
                ActiveActionTurnsLabel.Text = formatTurnsLeftString;
                ActiveActionTurnsLabel.Visible = true;
                ActiveActionTurnsLabel.TextColor = 6783862;
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

            int activityImage = _city.GetActiveOrder().ActivityImage(_city);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (_city.Production.Queue.GetCount() == 0)
            {
                string producingMerchandise = UI.Engine.TranslateResourceString(GameText.ProducingMerchandiseText);
                ActiveActionLabel.Text = producingMerchandise;
                string defaultText = UI.Engine.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurnsLabel.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurnsLabel.TextColor = 6783862;
                ActiveActionTurnsBar.Visible = false;
                int activityImage = ActivityImage(CityAction.None);
                CurrentActionImage.CurrentImage = activityImage;
            }
            else
            {
                reason = "";
                string nameProgressive = _city.Production.Queue.GetItem(0).GetNameProgressive();
                ActiveActionLabel.Text = nameProgressive;
                ProductionItem queue = _city.Production.Queue.GetItem(0);
                _city.Production.CanExecuteProduction(queue, ref reason);
                ActiveActionTurnsLabel.Text = reason;
                ActiveActionTurnsLabel.TextColor = 255;
                ActiveActionTurnsBar.Visible = false;
                int activityImage = _city.Production.Queue.GetItem(0).Order.ActivityImage(_city);
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
        for (int i = 0, last = _city.Production.UnitProductions.GetCount() - 1; i <= last; i++)
        {
            LoadUnitImages(_city.Production.UnitProductions.GetItem(i).UnitIndex);
        }

        generator = new RandomBattleMapGenerator(UI.Engine);
        sbyte hexLevel = _city.GetHexLevel();
        sbyte hexY = _city.GetHexY();
        _preview = generator.RandomMap(null, _city.GetHexX(), hexY, hexLevel);
        generator?.Dispose();
        armies = new ArmyHSList(null) { Holder = UI.Engine };
        _city.ListArmies(armies);
        for (int i = 0, last = armies.GetCount() - 1; i <= last; i++)
        {
            army = armies.GetArmyHexSprite(i);
            sbyte armyHexY = army.GetHexY();
            sbyte hexX = army.GetHexX();
            sbyte fCityHexY = _city.GetHexY();
            direction = HexGeometry.DeltaHexToHexNumber(_city.GetHexX(), fCityHexY, hexX, armyHexY);
            for (int j = 0, lastJ = army.Army.Units.Count - 1; j <= lastJ; j++)
            {
                CombatMap.PlaceOverviewUnit(unit: army.Army.Units.GetUnit(j), map: _preview, position: direction, index: j);
            }
        }

        armies?.Dispose();
        MapObject mapObject = _preview.CenterViewLocation;
        if (_preview.CenterViewLocation != null)
        {
            sbyte mapObjectHexY = mapObject.GetHexY();
            HexGeometry.HexToPixel(mapObject.GetHexX(), mapObjectHexY, out _viewX, out _viewY);
            _viewX = _viewX + 16 - (MapView.WinWidth / 2);
            _viewY = _viewY + 16 - (MapView.WinHeight / 2);
        }
        else
        {
            _viewX = 800;
            _viewY = 800;
        }

        StrategicMap.Current.TutorialControl.ShowMessage(56, true, true);
        UI.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
    }

    // AoWCityScreen.pas:292
    public void CityScreenCreate() => _images = new Int32List();

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
        UnitProduction unitProduction = _city.Production.UnitProductions.GetItem(slot);
        sender.CaptureInput = unitProduction != null;
    }

    // AoWCityScreen.pas:348
    public void ActivityPreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        int image;
        image = ActivityImage((CityAction)((byte)slot + 1));
        UI.Graphics.CityWindow[image].Show(ActivityPanel2.WinLeft + sender.WinLeft + 2, ActivityPanel2.WinTop + sender.WinTop + 2, lockedSurface);
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
        UI.General.PlayButtonSound(0);
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
        Rect r;
        UnitShowInfo info = default;
        Unit u;
        UnitProduction p;
        r = new Rect(ActivityPanel.WinLeft + sender.WinLeft, ActivityPanel.WinTop + sender.WinTop, ActivityPanel2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPanel.WinTop + sender.WinTop + sender.WinHeight);
        // { Original uses two different panels. }
        p = _city.Production.UnitProductions.GetItem(slot);
        if (p != null)
        {
            u = _city.Production.CreateUnitProduction(slot);
            info.Direction = 3;
            info.Overlays = Set<UnitOverlayModesElement>.Of();
            info.Animation = 0;
            info.Frame = 0;
            info.Looped = false;
            if (!p.Researched)
            {
                UI.Graphics.CityWindow[5].Show(r.Left + 2, r.Top + 2, lockedSurface);
                info.Direction = 5;
                u?.Show(r.Left - 4, r.Top, lockedSurface, info);

                BlendTable lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                UI.Graphics.CityWindow[5].ShowBlended(r.Left + 2, r.Top + 2, lockedSurface, lookupTable);
            }
            else
            {
                UI.Graphics.CityWindow[4].Show(r.Left + 2, r.Top + 2, lockedSurface);
                u?.Show(r.Left - 4, r.Top, lockedSurface, info);
            }

            u?.Release();
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
        Unit u;
        Rect r;
        u = _city.Production.CreateUnitProduction(_productionIndex);
        if (u != null)
        {
            r = new Rect(UnitPictureFramePanel.WinLeft + sender.WinLeft + 2, UnitPictureFramePanel.WinTop + sender.WinTop + 2, UnitPictureFramePanel.WinLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.WinTop + sender.WinTop + sender.WinHeight - 2);
            u.ShowFace(r, lockedSurface);
            u.Release();
        }
    }

    // AoWCityScreen.pas:558
    public void QueueLBChange(ListBox sender)
    {
        CityAction action;
        int count = _city.Production.Queue.GetCount();
        if (QueueList.Selected < count && _mode != 0)
        {
            if (QueueList.Selected != -1)
            {
                _queueIndex = QueueList.Selected;
                ProductionOrder order = _city.Production.Queue.GetItem(_queueIndex).Order;
                action = order.CityAction;
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
        Rect r;
        Unit u;
        if (_city.Production.Queue.GetCount() != 0)
        {
            if (_city.Production.Queue.GetItem(0).Order.UnitProductionIndex is int index)
            {
                r = new Rect(ActiveActionPanel.WinLeft + sender.WinLeft, ActiveActionPanel.WinTop + sender.WinTop, ActiveActionPanel.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.WinTop + sender.WinTop + sender.WinHeight);
                u = _city.Production.CreateUnitProduction(index);
                if (u != null)
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

                    u.Show(r.Left - 8, r.Top - 5, lockedSurface, info);
                    u.Release();
                }
            }
        }
    }

    // AoWCityScreen.pas:626
    public void LoopQueueBoxChange() => _city.Production.SetLoopQueueTo(LoopQueueCheckBox.Checked);

    // AoWCityScreen.pas:631
    public void RacePortraitDraw(Image sender, LockedSurface lockedSurface)
    {
        Rect r;
        r = new Rect(PopulationPanel.WinLeft + sender.WinLeft, PopulationPanel.WinTop + sender.WinTop, PopulationPanel.WinLeft + sender.WinLeft + sender.WinWidth, PopulationPanel.WinTop + sender.WinTop + sender.WinHeight);
        GamePack.Current.RaceResources.GetRaceResource(_city.Race).GetFace().ShowStretched(r, lockedSurface);
    }

    // AoWCityScreen.pas:641
    public void MapViewDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        LockedSurface viewSurfaceDescription = default;
        Rect r;
        int w;
        int h;
        int width;
        if (_preview != null)
        {
            r = new Rect(Window.WinLeft + sender.WinLeft, Window.WinTop + sender.WinTop, Window.WinLeft + sender.WinLeft + sender.WinWidth, Window.WinTop + sender.WinTop + sender.WinHeight);
            width = r.Right - r.Left;
            w = width;
            h = r.Bottom - r.Top;
            int maxPixelX = _preview.GetMaxPixelX();
            if (_viewX + w + 16 > maxPixelX)
            {
                _viewX = _preview.GetMaxPixelX() - w - 16;
            }

            int maxPixelY = _preview.GetMaxPixelY();
            if (_viewY + h + 16 > maxPixelY)
            {
                _viewY = _preview.GetMaxPixelY() - h - 16;
            }

            if (_viewX < 16)
            {
                _viewX = 16;
            }

            if (_viewY < 16)
            {
                _viewY = 16;
            }

            LockedSurface.CreateFrom(lockedSurface, r, out viewSurfaceDescription);
            _preview.SetExplorationEnabled(false);
            _preview.SetFogEnabled(false);
            _preview.SetViewPort(_viewX, _viewY, 0, width, r.Bottom - r.Top);
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
        preview?.NewFrame();

        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // AoWCityScreen.pas:688
    public void DlgHide() => CloseCity();

    // AoWCityScreen.pas:693
    public void UAbilitiesLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Ability a;
        if (button == MouseButton.Right && UnitAbilitiesList.Selected > -1)
        {
            object objectValue = UnitAbilitiesList.Items.Objects[UnitAbilitiesList.Selected];
            a = GamePack.Current.AbilityControl.GetAbility(ObjectToInt(objectValue));
            if (a != null)
            {
                InfoBanner.SetInfoBanner(caption: a.GetName(null), banner: UI.InfoBanner, description: a.Description);
            }
        }
    }

    // AoWCityScreen.pas:704
    public void PPrevClick()
    {
        int start;
        int index;
        PlayerStructure s;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
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
        int start;
        int index;
        PlayerStructure s;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_city);
        index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
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
