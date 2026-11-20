#nullable disable
// Translated from AoWProdScreen.pas.
using System.Globalization;
using Aow.Engine.Collections;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// view, place, production, queue, mode, preview, images
public sealed partial class ProductionScreen(UIContext ui) : UIModule(ui)
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
    public Button MainButton3 { get; private set; }
    public Button MainButton1 { get; private set; }
    public ListBox QueueList { get; private set; }
    public Panel ActivityPanel { get; private set; }
    public Panel InfoPanel2 { get; private set; }
    public Label ActiveActionLabel { get; private set; }
    public Label ActiveActionTurnsLabel { get; private set; }
    public Panel ActivityPanel2 { get; private set; }
    public Gauge ActiveActionTurnsBar { get; private set; }
    public Button MainButton2 { get; private set; }
    public Panel InfoPanel { get; private set; }
    public Frame PlacePictureFrame { get; private set; }
    public Memo PlaceInfoMemo { get; private set; }
    public Frame IncomePictureFrame { get; private set; }
    public Memo IncomeMemo { get; private set; }
    public Memo IncomeHeader { get; private set; }
    public Label QueueLabel { get; private set; }
    public CheckBox LoopQueueCheckBox { get; private set; }
    public Label RepeatLabel { get; private set; }
    public Panel ActiveActionPanel { get; private set; }
    public Image CurrentActionImage { get; private set; }
    public Panel UnitPictureFramePanel { get; private set; }
    public Image UpgradePicture { get; private set; }
    public Label UpgradeCostLabel { get; private set; }
    public Label UpgradeTurnsLabel { get; private set; }
    public Memo IncomeValuesMemo { get; private set; }
    public Button SphereButton { get; private set; }
    public UserDrawBox MapView { get; private set; }
    public Button NextButton { get; private set; }
    public Button PreviousButton { get; private set; }

    private int _viewX;
    private int _viewY;
    private ProductionPlace _place;
    private int _productionIndex;
    private int _queueIndex;
    private byte _mode;
    private CombatMap _preview;
    private Int32List _images;

    // AoWProdScreen.pas:590
    public void FreePreview()
    {
        if (_preview is not null)
        {
            _preview?.Dispose();
            _preview = null;
        }
    }

    // AoWProdScreen.pas:599
    public void ReleaseImages()
    {
        for (int i = _images.Count - 1; i >= 0; i--)
        {
            int image = _images[i];
            _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int index)
    {
        int graphics = GamePack.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        _ = GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _ = _images.Add(graphics);
    }

    // AoWProdScreen.pas:617
    public void ResetChecks()
    {
        Action1CheckBox.Checked = false;
        Action2CheckBox.Checked = false;
        Action3CheckBox.Checked = false;
        Action4CheckBox.Checked = false;
        Action5CheckBox.Checked = false;
        Action6CheckBox.Checked = false;
    }

    // AoWProdScreen.pas:627
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
        MainButton1.Visible = true;
        string options = UI.Engine.TranslateResourceString(GameText.OptionsText);
        MainButton1.Text = options;
        MainButton2.Visible = false;
        PreviousButton.Visible = !MainButton2.Visible;
        NextButton.Visible = !MainButton2.Visible;
        LeftShadow.Visible = UI.Engine.AnimationDetail != 0;
        RightShadow.Visible = UI.Engine.AnimationDetail != 0;
        Oval.Visible = UI.Engine.AnimationDetail != 0;
        _queueIndex = -1;
        QueueList.Selected = -1;
        string queue = UI.Engine.TranslateResourceString(GameText.QueueText);
        QueueLabel.Text = queue;
        string repeat = UI.Engine.TranslateResourceString(GameText.RepeatText);
        RepeatLabel.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        _place = null;
        UI.General.Manager.OverlayPriority = 0;
    }

    // AoWProdScreen.pas:659
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
                MainButton1.Text = change;
                MainButton2.Visible = false;
                PreviousButton.Visible = !MainButton2.Visible;
                NextButton.Visible = !MainButton2.Visible;
                string exit = UI.Engine.TranslateResourceString(GameText.ExitText);
                MainButton3.Text = exit;
                QueueList.Selected = -1;
                break;
            case 1:
                ResetChecks();
                QueueList.Selected = -1;
                InfoPanel.Visible = false;
                InfoPanel2.Visible = false;
                ActivityPanel.Visible = true;
                ActivityPanel2.Visible = true;
                string info = UI.Engine.TranslateResourceString(GameText.InfoText);
                MainButton1.Text = info;
                string done = UI.Engine.TranslateResourceString(GameText.DoneText);
                MainButton3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueList.Selected = -1;
                InfoPanel.Visible = false;
                InfoPanel2.Visible = false;
                ActivityPanel.Visible = true;
                ActivityPanel2.Visible = true;
                string infoText = UI.Engine.TranslateResourceString(GameText.InfoText);
                MainButton1.Text = infoText;
                string doneText = UI.Engine.TranslateResourceString(GameText.DoneText);
                MainButton3.Text = doneText;
                break;
            default:
                break;
        }
    }

    // AoWProdScreen.pas:698
    public void SelectDefaultAction()
    {
        ResetChecks();
        Action1CheckBox.Checked = true;
        SelectProduction(0);
    }

    // AoWProdScreen.pas:705
    public void ProductionEvent(ProductionPlaceEventLog eventValue)
    {
        ShowPlace(eventValue.GetProductionPlace());
        SetMode(1);
        SelectDefaultAction();
    }

    // AoWProdScreen.pas:712
    public void PlaceChanged(object sender)
    {
        if (Window.Visible && sender == _place)
        {
            if (_place.Player == StrategicMap.Current.SeatedPlayer)
            {
                UpdateInfo();
            }
            else
            {
                Window.Visible = false;
            }
        }
    }

    // AoWProdScreen.pas:729
    public void SelectProduction(int index)
    {
        if (_mode != 1)
        {
            SetMode(1);
        }

        UpgradePanel.Visible = false;
        UnitPanel.Visible = false;
        _productionIndex = index;
        UpgradePicture.CurrentImage = 21;
        UpgradeCostLabel.Text = "-";
        UpgradeTurnsLabel.Text = "-";
        MainButton1.Visible = false;
        MainButton2.Visible = false;
        PreviousButton.Visible = !MainButton2.Visible;
        NextButton.Visible = !MainButton2.Visible;
        switch (_productionIndex)
        {
            case >= 0 and <= 3:
                Unit u = _place.ProductionControl.CreateUnitProduction(_productionIndex);
                if (u is not null)
                {
                    UnitPanel.Visible = true;
                    sbyte attack = u.GetAttack();
                    UnitAttackLabel.Text = ((int)attack).ToString(CultureInfo.InvariantCulture);
                    sbyte defense = u.GetDefense();
                    UnitDefenseLabel.Text = ((int)defense).ToString(CultureInfo.InvariantCulture);
                    sbyte resistance = u.GetResistance();
                    UnitResistanceLabel.Text = ((int)resistance).ToString(CultureInfo.InvariantCulture);
                    sbyte damage = u.GetDamage();
                    UnitDamageLabel.Text = ((int)damage).ToString(CultureInfo.InvariantCulture);
                    sbyte hits = u.GetHits();
                    UnitHitsLabel.Text = ((int)hits).ToString(CultureInfo.InvariantCulture);
                    sbyte moves = u.Moves;
                    UnitMovesLabel.Text = ((int)moves).ToString(CultureInfo.InvariantCulture);
                    Set<MoveType> moveTypes = u.GetMoveTypes();
                    int moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                    UnitMovesIcon.CurrentImage = moveTypesToInt32;
                    Units.Alignment alignment = u.GetAlignment();
                    string alignmentText = UI.Engine.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                    string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), arguments: [alignmentText]);
                    UnitAlignmentLabel.Text = format;
                    sbyte unitLevel = u.UnitLevel;
                    string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.LevelFormat), arguments: [((int)unitLevel).ToString(CultureInfo.InvariantCulture)]);
                    byte upkeep = u.GetUpkeep();
                    string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), arguments: [(int)upkeep]);
                    UnitLevelLabel.Text = $"{formattedText} / {format2}";
                    UnitAbilitiesList.Items.Clear();
                    u.ListAbilities(UnitAbilitiesList.Items);
                    if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                    {
                        MainButton1.Visible = true;
                        string produce = UI.Engine.TranslateResourceString(GameText.ProduceText);
                        MainButton1.Text = produce;
                        bool queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainButton2.Visible = !queueLocked;
                        string queue = UI.Engine.TranslateResourceString(GameText.QueueText);
                        MainButton2.Text = queue;
                        string name = u.GetName();
                        string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.ProduceFormat), arguments: [name]);
                        UnitLabel.Text = format3;
                        int unitProductionGold = _place.ProductionControl.UnitProductionGold(_productionIndex);
                        UnitCostLabel.Text = unitProductionGold.ToString(CultureInfo.InvariantCulture);
                        int unitProductionTime = _place.ProductionControl.UnitProductionTime(_productionIndex);
                        UnitTurnsLabel.Text = unitProductionTime.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        MainButton1.Visible = true;
                        string install = UI.Engine.TranslateResourceString(GameText.InstallText);
                        MainButton1.Text = install;
                        bool queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainButton2.Visible = !queueLocked;
                        string queue = UI.Engine.TranslateResourceString(GameText.QueueText);
                        MainButton2.Text = queue;
                        string name = u.GetName();
                        string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.InstallFormat), arguments: [name]);
                        UnitLabel.Text = format3;
                        int unitResearchGold = _place.ProductionControl.UnitResearchGold(_productionIndex);
                        UnitCostLabel.Text = unitResearchGold.ToString(CultureInfo.InvariantCulture);
                        int unitResearchTime = _place.ProductionControl.UnitResearchTime(_productionIndex);
                        UnitTurnsLabel.Text = unitResearchTime.ToString(CultureInfo.InvariantCulture);
                    }

                    PreviousButton.Visible = !MainButton2.Visible;
                    NextButton.Visible = !MainButton2.Visible;
                    _ = u.Release();
                }
                else
                {
                    UnitPanel.Visible = false;
                }

                break;
            case 4:
                MainButton1.Visible = true;
                string productionName = new Trade(0).Name(_place.ProductionControl);
                MainButton1.Text = productionName;
                bool fProductionControlQueueLocked = _place.ProductionControl.GetQueueLocked();
                MainButton2.Visible = !fProductionControlQueueLocked;
                string queueText = UI.Engine.TranslateResourceString(GameText.QueueText);
                MainButton2.Text = queueText;
                PreviousButton.Visible = !MainButton2.Visible;
                NextButton.Visible = !MainButton2.Visible;
                if (_place.GetMagicSphere() == MagicSphere.None)
                {
                    string produceMerchandiseText = UI.Engine.TranslateResourceString(s_produceMerchandiseText);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    string generateExtraPowerText = UI.Engine.TranslateResourceString(s_generateExtraPowerText);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = _place.GetMagicSphere() != MagicSphere.None ? 28 : 27;

                UpgradeLabel.Text = MainButton1.Text;
                break;
            case 5:
                MainButton1.Visible = true;
                string raze = UI.Engine.TranslateResourceString(GameText.RazeText);
                MainButton1.Text = $"{raze}...";
                PreviousButton.Visible = false;
                NextButton.Visible = false;
                string fPlaceName = _place.GetName();
                string format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_productionPlaceRazeFormat), arguments: [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                string razeText = UI.Engine.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                break;
            default:
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        IncomeHeader.Items.Clear();
        string name = _place.GetName();
        StructureNameLabel.Text = name;
        string income = UI.Engine.TranslateResourceString(GameText.IncomeText);
        _ = IncomeHeader.Items.Add(income);
        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            int power = _place.GetPower();
            string xPower = UI.Engine.TranslateResourceString(s_powerFormat);
            string format = AbilityRules.Format(" " + xPower, arguments: [power.ToString(CultureInfo.InvariantCulture)]);
            _ = IncomeHeader.Items.Add(format);
        }

        if (_place.GetIncome() != 0)
        {
            int fPlaceIncome = _place.GetIncome();
            string xGold = UI.Engine.TranslateResourceString(GameText.GoldFormat);
            string format = AbilityRules.Format(" " + xGold, arguments: [fPlaceIncome.ToString(CultureInfo.InvariantCulture)]);
            _ = IncomeHeader.Items.Add(format);
        }

        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            IncomePictureFrame.ImageLibraryIndexTexture = 64;
            PlacePictureFrame.ImageLibraryTexture = null;
            int sphereImage = SphereImage(_place.GetMagicSphere());
            SphereButton.SetImageLibraryUIndex(sphereImage);
            SphereButton.Visible = true;
        }
        else
        {
            if (_place is Shipyard)
            {
                IncomeHeader.Items.Clear();
                IncomePictureFrame.ImageLibraryIndexTexture = 68;
            }
            else
            {
                IncomePictureFrame.ImageLibraryIndexTexture = 65;
            }

            PlacePictureFrame.ImageLibraryTexture = UI.Graphics.CityWindow;
            PlacePictureFrame.ImageLibraryIndexTexture = _place is Shipyard ? 67 : 66;

            SphereButton.Visible = false;
        }

        IncomeMemo.Items.Clear();
        IncomeValuesMemo.Items.Clear();
        _place.ListIncomeInfo(IncomeMemo.Items, values: IncomeValuesMemo.Items);
        QueueList.Items.Clear();
        for (int i = 0, last = _place.ProductionControl.Queue.GetCount() - 1; i <= last; i++)
        {
            string name2 = _place.ProductionControl.Queue.GetItem(i).GetName();
            _ = QueueList.Items.Add($"{(i + 1).ToString(CultureInfo.InvariantCulture)}. {name2}");
        }

        for (int i = _place.ProductionControl.Queue.GetCount(); i <= 4; i++)
        {
            string emptyQueue = UI.Engine.TranslateResourceString(GameText.EmptyQueueText);
            _ = QueueList.Items.Add($"{(i + 1).ToString(CultureInfo.InvariantCulture)}. {emptyQueue}");
        }

        int count = _place.ProductionControl.Queue.GetCount();
        if (QueueList.Selected >= count)
        {
            int fQueueCount = _place.ProductionControl.Queue.GetCount();
            QueueList.Selected = fQueueCount - 1;
            _queueIndex = QueueList.Selected;
        }

        LoopQueueCheckBox.Checked = _place.ProductionControl.LoopQueue;
        if (_place.ProductionControl.Status == ProductionControlStatus.Producing)
        {
            string nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
            ActiveActionLabel.Text = nameProgressive;
            if (_place.ProductionControl.Settings.TurnsLeft != -1)
            {
                string formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: _place.ProductionControl.Settings.TurnsLeft);
                ActiveActionTurnsLabel.Text = formatTurnsLeftString;
                ActiveActionTurnsLabel.Visible = true;
                ActiveActionTurnsLabel.TextColor = 0x67_83_76;
                ActiveActionTurnsBar.Min = 0;
                ActiveActionTurnsBar.Max = _place.ProductionControl.Settings.TurnsTotal;
                ActiveActionTurnsBar.Position = _place.ProductionControl.Settings.TurnsTotal - _place.ProductionControl.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurnsLabel.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            int activityImage = _place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(_place);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (_place.ProductionControl.Status == ProductionControlStatus.Idle)
            {
                string currentActivityText = _place.GetCurrentActivityText();
                ActiveActionLabel.Text = currentActivityText;
                string defaultText = UI.Engine.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurnsLabel.Text = $"({LowerCase(defaultText)})";
                ActiveActionTurnsLabel.TextColor = 0x67_83_76;
                if (CompareText(b: UI.Engine.TranslateResourceString(GameTables.NoneText), a: ActiveActionLabel.Text) != 0)
                {
                    ActiveActionTurnsLabel.Visible = true;
                    int activityImage = new Trade(0).ActivityImage(_place);
                    CurrentActionImage.CurrentImage = activityImage;
                }
                else
                {
                    ActiveActionTurnsLabel.Visible = false;
                    CurrentActionImage.CurrentImage = 62;
                }

                ActiveActionTurnsBar.Visible = false;
            }
            else
            {
                if (_place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    string reason = "";
                    string nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
                    ActiveActionLabel.Text = nameProgressive;
                    ProductionItem queue = _place.ProductionControl.Queue.GetItem(0);
                    _ = _place.ProductionControl.CanExecuteProduction(queue, ref reason);
                    ActiveActionTurnsLabel.Text = reason;
                    ActiveActionTurnsLabel.TextColor = 0x00_00_FF;
                    ActiveActionTurnsLabel.Visible = true;
                    ActiveActionTurnsBar.Visible = false;
                    int activityImage = _place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(_place);
                    CurrentActionImage.CurrentImage = activityImage;
                }
            }
        }

        switch (_mode)
        {
            case 1:
                if (_productionIndex != -1)
                {
                    SelectProduction(_productionIndex);
                }

                break;
            case 2:
                QueueLBChange();
                break;
            default:
                break;
        }

        Window.Drawn = false;
    }

    // AoWProdScreen.pas:180
    public void ShowPlace(ProductionPlace place)
    {
        Reset();
        _place = place;
        UpdateInfo();
        SetMode(0);
        string text = place.Description.Text;
        PlaceInfoMemo.Items.Text = text;
        for (int i = 0, last = _place.ProductionControl.UnitProductions.GetCount() - 1; i <= last; i++)
        {
            LoadUnitImages(_place.ProductionControl.UnitProductions.GetItem(i).UnitIndex);
        }

        FreePreview();
        RandomBattleMapGenerator generator = new(UI.Engine);
        sbyte hexLevel = _place.HexLevel;
        sbyte hexY = _place.HexY;
        _preview = generator.RandomMap(null, _place.HexX, hexY, L: hexLevel);
        generator?.Dispose();
        ArmyHexSprite army = _place.Owner.GetArmyHexSprite();
        if (army is not null)
        {
            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
            {
                CombatMap.PlaceOverviewUnit(unit: army.Army.Units.GetUnit(i), map: _preview, position: 0, index: i);
            }
        }

        int baseIncome = _place.BaseIncome;
        Action5CheckBox.CaptureInput = baseIncome != 0;
        HexSprite center = _preview.CenterViewLocation;
        if (center is not null)
        {
            sbyte fCenterViewLocationHexY = _preview.CenterViewLocation.HexY;
            HexGeometry.HexToPixel(center.HexX, fCenterViewLocationHexY, out _viewX, out _viewY);
            _viewX = _viewX + 16 - (MapView.WinWidth / 2);
            _viewY = _viewY + 16 - (MapView.WinHeight / 2);
        }
        else
        {
            _viewX = 800;
            _viewY = 800;
        }

        UI.General.Manager.OverlayPriority = 49;
        Window.Visible = true;
    }

    // AoWProdScreen.pas:221
    public void ProductionScreenCreate() => _images = new Int32List();

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy() => _images?.Dispose();

    // AoWProdScreen.pas:231
    public void ActivityChange(CheckBox sender, int slot)
    {
        UI.General.PlayButtonSound(1);
        if (sender.Checked)
        {
            ResetChecks();
            SelectProduction(slot);
            sender.Checked = true;
        }
    }

    // AoWProdScreen.pas:242
    public void ActivityDoubleClick(CheckBox sender, int slot)
    {
        ResetChecks();
        SelectProduction(slot);
        sender.Checked = true;
        MainBtn1Click();
    }

    // AoWProdScreen.pas:250
    public void ActivityPreDraw(CheckBox sender, LockedSurface lockedSurface, int slot)
    {
        UnitShowInfo info = default;
        Rect r = new(ActivityPanel2.AbsoluteLeft + sender.WinLeft, ActivityPanel2.AbsoluteTop + sender.WinTop, ActivityPanel2.AbsoluteLeft + sender.WinLeft + sender.WinWidth, ActivityPanel2.AbsoluteTop + sender.WinTop + sender.WinHeight);
        switch (slot)
        {
            case >= 0 and <= 3:
                Unit u = _place.ProductionControl.CreateUnitProduction(slot);
                if (u is not null)
                {
                    info.Direction = 3;
                    info.Overlays = Set<UnitOverlayModesElement>.Of();
                    info.Animation = 0;
                    info.Frame = 0;
                    info.Looped = false;
                    if (!_place.ProductionControl.UnitProductions.GetItem(slot).Researched)
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

                    _ = u.Release();
                }
                else
                {
                    UI.Graphics.CityWindow[62].Show(r.Left + 2, r.Top + 2, lockedSurface);
                }

                break;
            case 4:
                if (_place.BaseIncome != 0)
                {
                    int activityImage = new Trade(0).ActivityImage(_place);
                    UI.Graphics.CityWindow[activityImage].Show(r.Left + 2, r.Top + 2, lockedSurface);
                }
                else
                {
                    UI.Graphics.CityWindow[62].Show(r.Left + 2, r.Top + 2, lockedSurface);
                }

                break;
            case 5:
                UI.Graphics.CityWindow[46].Show(r.Left + 2, r.Top + 2, lockedSurface);
                break;
            default:
                break;
        }
    }

    // AoWProdScreen.pas:299
    public void MainBtn1Click()
    {
        switch (_mode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                break;
            case 1:
                string reason = "";
                switch (_productionIndex)
                {
                    case >= 0 and <= 3:
                        if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                        {
                            if (_place.CanProduce(_productionIndex, queue: false, reason))
                            {
                                _ = _place.Produce(_productionIndex, queue: false);
                                SetMode(0);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (_place.CanResearch(_productionIndex, queue: false, ref reason))
                            {
                                _ = _place.Research(_productionIndex, queue: false);
                                SetMode(0);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }

                        break;
                    case 4:
                        if (_place.CanTrade(false, ref reason))
                        {
                            _ = _place.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    case 5:
                        if (_place.CanRaze(StrategicMap.Current.SeatedPlayer, ref reason))
                        {
                            _ = _place.Raze(StrategicMap.Current.SeatedPlayer);
                            Window.Visible = false;
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    default:
                        break;
                }

                break;
            case 2:
                if (QueueList.Selected != -1)
                {
                    _ = _place.ProductionControl.DeleteProduction(QueueList.Selected);
                }

                break;
            default:
                break;
        }
    }

    // AoWProdScreen.pas:347
    public void MainBtn2Click()
    {
        string reason = "";
        switch (_mode)
        {
            case 1:
                switch (_productionIndex)
                {
                    case >= 0 and <= 3:
                        if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                        {
                            if (_place.CanProduce(_productionIndex, queue: true, reason))
                            {
                                _ = _place.Produce(_productionIndex, queue: true);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (_place.CanResearch(_productionIndex, queue: true, ref reason))
                            {
                                _ = _place.Research(_productionIndex, queue: true);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }

                        break;
                    case 4:
                        if (_place.CanTrade(true, ref reason))
                        {
                            _ = _place.Trade(true);
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                    default:
                        break;
                }

                break;
            case 2:
                if (_queueIndex == 0)
                {
                    if (_place.ProductionControl.RetryProductionExecution(ref reason))
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

    // AoWProdScreen.pas:383
    public void MainBtn3Click()
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
    }

    // AoWProdScreen.pas:389
    public void UFaceDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        Unit u = _place.ProductionControl.CreateUnitProduction(_productionIndex);
        if (u is not null)
        {
            Rect r = new(UnitPictureFramePanel.AbsoluteLeft + sender.WinLeft + 2, UnitPictureFramePanel.AbsoluteTop + sender.WinTop + 2, UnitPictureFramePanel.AbsoluteLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.AbsoluteTop + sender.WinTop + sender.WinHeight - 2);
            u.ShowFace(r, lockedSurface);
            _ = u.Release();
        }
    }

    // AoWProdScreen.pas:406
    public void QueueLBChange()
    {
        int count = _place.ProductionControl.Queue.GetCount();
        if (QueueList.Selected < count && _mode != 0)
        {
            if (QueueList.Selected != -1)
            {
                _queueIndex = QueueList.Selected;
                if (_place.ProductionControl.Queue.GetItem(_queueIndex).Order is ProduceUnit produce)
                {
                    SelectProduction(produce.Index);
                }
                else
                {
                    SelectProduction(4);
                }

                SetMode(2);
                QueueList.Selected = _queueIndex;
                MainButton1.Visible = true;
                string delete = UI.Engine.TranslateResourceString(GameText.DeleteText);
                MainButton1.Text = delete;
                if (_queueIndex == 0 && _place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    MainButton2.Visible = true;
                    string execute = UI.Engine.TranslateResourceString(GameText.ExecuteText);
                    MainButton2.Text = execute;
                }
                else
                {
                    MainButton2.Visible = false;
                }

                PreviousButton.Visible = false;
                NextButton.Visible = false;
            }
            else
            {
                SetMode(1);
                SelectDefaultAction();
            }
        }
        else
        {
            QueueList.Selected = _mode == 2 ? _queueIndex : -1;
        }
    }

    // AoWProdScreen.pas:439
    public void B4CBUpdate(CheckBox sender, int slot)
    {
        if (slot <= 3)
        {
            UnitProduction unitProduction = _place.ProductionControl.UnitProductions.GetItem(slot);
            sender.CaptureInput = unitProduction is not null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange() => _place.ProductionControl.SetLoopQueueTo(LoopQueueCheckBox.Checked);

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        if (_place.ProductionControl.Queue.GetCount() != 0)
        {
            if (_place.ProductionControl.Queue.GetItem(0).Order.UnitProductionIndex is not null)
            {
                Rect r = new(ActiveActionPanel.AbsoluteLeft + sender.WinLeft, ActiveActionPanel.AbsoluteTop + sender.WinTop, ActiveActionPanel.AbsoluteLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.AbsoluteTop + sender.WinTop + sender.WinHeight);
                ProductionPlaceControl productionPlaceControl = _place.ProductionControl;
                if (productionPlaceControl.UnitProductions.GetItem(productionPlaceControl.Settings.UnitProductionIndex) is not null)
                {
                    Unit u = _place.ProductionControl.CreateUnitProduction(_place.ProductionControl.Settings.UnitProductionIndex);
                    if (u is not null)
                    {
                        info.Direction = 3;
                        info.Overlays = Set<UnitOverlayModesElement>.Of();
                        info.Animation = 0;
                        info.Frame = 0;
                        info.Looped = false;
                        _ = u.Show(r.Left - 12, r.Top - 5, lockedSurface, info);
                        _ = u.Release();
                    }
                }
            }
        }
    }

    // AoWProdScreen.pas:483
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

            LockedSurface.CreateFrom(lockedSurface, r, destination: out LockedSurface viewSurfaceDescription);
            _preview.SetExplorationEnabled(false);
            _preview.SetFogEnabled(false);
            _ = _preview.SetViewPort(_viewX, _viewY, lHitPoints: 0, width, r.Bottom - r.Top);
            _preview.UpdateStaticScene();
            _preview.UpdateDynamicScene();
            _preview.DrawStaticScene(in viewSurfaceDescription);
            _preview.DrawDynamicScene(ref viewSurfaceDescription);
        }
    }

    // AoWProdScreen.pas:510
    public void DlgKeyDown(in ushort key, Set<ShiftState> shift)
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
            default:
                break;
        }
    }

    // AoWProdScreen.pas:521
    public void MapViewUpdate()
    {
        CombatMap preview = _preview;
        preview?.NewFrame();

        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // AoWProdScreen.pas:530
    public void DlgHide() => ClosePlace();

    // AoWProdScreen.pas:535
    public void UAbilitiesLBMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (button == MouseButton.Right && UnitAbilitiesList.Selected > -1)
        {
            object objectValue = UnitAbilitiesList.Items.Objects[UnitAbilitiesList.Selected];
            Ability a = GamePack.Current.AbilityControl[ObjectToInt(objectValue)];
            if (a is not null)
            {
                InfoBanner.SetInfoBanner(caption: a.GetName(null), banner: UI.InfoBanner, description: a.Description);
            }
        }
    }

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        int index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            PlayerStructure s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is ProductionPlace place && place.GetMagicSphere() == MagicSphere.None)
            {
                index = start + 1;
                ShowPlace(place);
            }

            index += -1;
            if (index < 0)
            {
                index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
            }
        }
    }

    // AoWProdScreen.pas:568
    public void PNextClick()
    {
        int start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        int index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            PlayerStructure s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is ProductionPlace place && place.GetMagicSphere() == MagicSphere.None)
            {
                index = start - 1;
                ShowPlace(place);
            }

            index += 1;
            if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
            {
                index = 0;
            }
        }
    }

    private static readonly string s_generateExtraPowerText = "Increases the power generated by the node by 25%.";
    private static readonly string s_produceMerchandiseText = "When producing merchandise the income is increased by 25%.";
    private static readonly string s_productionPlaceRazeFormat = "Destroys the %s instantly by burning it to the ground.";
    private static readonly string s_powerFormat = "%s power";

    private static int SphereImage(MagicSphere sphere)
    {
        return sphere switch
        {
            MagicSphere.Life => 27,
            MagicSphere.Fire => 42,
            MagicSphere.Earth => 33,
            MagicSphere.Air => 36,
            MagicSphere.Water => 39,
            MagicSphere.Death => 30,
            MagicSphere.Cosmos or MagicSphere.Secret or MagicSphere.None or _ => -1,
        };
    }
}
