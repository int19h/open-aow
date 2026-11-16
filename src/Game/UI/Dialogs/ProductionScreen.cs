#nullable disable
// Translated from AoWProdScreen.pas.
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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// view, place, production, queue, mode, preview, images
public partial class ProductionScreen : UIModule
{
    public ProductionScreen(UIContext ui) : base(ui)
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
        if (_preview != null)
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
            GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int index)
    {
        int graphics;
        graphics = GamePack.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        GamePack.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _images.Add(graphics);
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
        Unit u;
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
                u = _place.ProductionControl.CreateUnitProduction(_productionIndex);
                if (u != null)
                {
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
                    string format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                    UnitAlignmentLabel.Text = format;
                    sbyte unitLevel = u.GetUnitLevel();
                    string formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                    byte upkeep = u.GetUpkeep();
                    string format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                    UnitLevelLabel.Text = formattedText + " / " + format2;
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
                        string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.ProduceFormat), [name]);
                        UnitLabel.Text = format3;
                        int unitProductionGold = _place.ProductionControl.UnitProductionGold(_productionIndex);
                        UnitCostLabel.Text = IntToStr(unitProductionGold);
                        int unitProductionTime = _place.ProductionControl.UnitProductionTime(_productionIndex);
                        UnitTurnsLabel.Text = IntToStr(unitProductionTime);
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
                        string format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.InstallFormat), [name]);
                        UnitLabel.Text = format3;
                        int unitResearchGold = _place.ProductionControl.UnitResearchGold(_productionIndex);
                        UnitCostLabel.Text = IntToStr(unitResearchGold);
                        int unitResearchTime = _place.ProductionControl.UnitResearchTime(_productionIndex);
                        UnitTurnsLabel.Text = IntToStr(unitResearchTime);
                    }

                    PreviousButton.Visible = !MainButton2.Visible;
                    NextButton.Visible = !MainButton2.Visible;
                    u.Release();
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
                MainButton1.Text = raze + "...";
                PreviousButton.Visible = false;
                NextButton.Visible = false;
                string fPlaceName = _place.GetName();
                string format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_productionPlaceRazeFormat), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                string razeText = UI.Engine.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string reason;
        IncomeHeader.Items.Clear();
        string name = _place.GetName();
        StructureNameLabel.Text = name;
        string income = UI.Engine.TranslateResourceString(GameText.IncomeText);
        IncomeHeader.Items.Add(income);
        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            int power = _place.GetPower();
            string xPower = UI.Engine.TranslateResourceString(s_powerFormat);
            string format = AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHeader.Items.Add(format);
        }

        if (_place.GetIncome() != 0)
        {
            int fPlaceIncome = _place.GetIncome();
            string xGold = UI.Engine.TranslateResourceString(GameText.GoldFormat);
            string format = AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHeader.Items.Add(format);
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
        _place.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueList.Items.Clear();
        for (int i = 0, last = _place.ProductionControl.Queue.GetCount() - 1; i <= last; i++)
        {
            string name2 = _place.ProductionControl.Queue.GetItem(i).GetName();
            QueueList.Items.Add(IntToStr(i + 1) + ". " + name2);
        }

        for (int i = _place.ProductionControl.Queue.GetCount(); i <= 4; i++)
        {
            string emptyQueue = UI.Engine.TranslateResourceString(GameText.EmptyQueueText);
            QueueList.Items.Add(IntToStr(i + 1) + ". " + emptyQueue);
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
                ActiveActionTurnsLabel.TextColor = 6783862;
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
                ActiveActionTurnsLabel.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurnsLabel.TextColor = 6783862;
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
                    reason = "";
                    string nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
                    ActiveActionLabel.Text = nameProgressive;
                    ProductionItem queue = _place.ProductionControl.Queue.GetItem(0);
                    _place.ProductionControl.CanExecuteProduction(queue, ref reason);
                    ActiveActionTurnsLabel.Text = reason;
                    ActiveActionTurnsLabel.TextColor = 255;
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
        }

        Window.Drawn = false;
    }

    // AoWProdScreen.pas:180
    public void ShowPlace(ProductionPlace place)
    {
        ArmyHexSprite army;
        RandomBattleMapGenerator generator;
        HexSprite center;
        Reset();
        _place = place;
        UpdateInfo();
        SetMode(0);
        string text = place.GetDescription().Text;
        PlaceInfoMemo.Items.Text = text;
        for (int i = 0, last = _place.ProductionControl.UnitProductions.GetCount() - 1; i <= last; i++)
        {
            LoadUnitImages(_place.ProductionControl.UnitProductions.GetItem(i).UnitIndex);
        }

        FreePreview();
        generator = new RandomBattleMapGenerator(UI.Engine);
        sbyte hexLevel = _place.GetHexLevel();
        sbyte hexY = _place.GetHexY();
        _preview = generator.RandomMap(null, _place.GetHexX(), hexY, hexLevel);
        generator?.Dispose();
        army = _place.Owner.GetArmyHexSprite();
        if (army != null)
        {
            for (int i = 0, last = army.Army.Units.Count - 1; i <= last; i++)
            {
                CombatMap.PlaceOverviewUnit(unit: army.Army.Units.GetUnit(i), map: _preview, position: 0, index: i);
            }
        }

        int baseIncome = _place.GetBaseIncome();
        Action5CheckBox.CaptureInput = baseIncome != 0;
        center = _preview.CenterViewLocation;
        if (center != null)
        {
            sbyte fCenterViewLocationHexY = _preview.CenterViewLocation.GetHexY();
            HexGeometry.HexToPixel(center.GetHexX(), fCenterViewLocationHexY, out _viewX, out _viewY);
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
        Rect r;
        Unit u;
        r = new Rect(ActivityPanel2.WinLeft + sender.WinLeft, ActivityPanel2.WinTop + sender.WinTop, ActivityPanel2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPanel2.WinTop + sender.WinTop + sender.WinHeight);
        switch (slot)
        {
            case >= 0 and <= 3:
                u = _place.ProductionControl.CreateUnitProduction(slot);
                if (u != null)
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
                        u?.Show(r.Left - 4, r.Top, lockedSurface, info);

                        BlendTable lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                        UI.Graphics.CityWindow[5].ShowBlended(r.Left + 2, r.Top + 2, lockedSurface, lookupTable);
                    }
                    else
                    {
                        UI.Graphics.CityWindow[4].Show(r.Left + 2, r.Top + 2, lockedSurface);
                        u?.Show(r.Left - 4, r.Top, lockedSurface, info);
                    }

                    u.Release();
                }
                else
                {
                    UI.Graphics.CityWindow[62].Show(r.Left + 2, r.Top + 2, lockedSurface);
                }

                break;
            case 4:
                if (_place.GetBaseIncome() != 0)
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
        }
    }

    // AoWProdScreen.pas:299
    public void MainBtn1Click()
    {
        string reason = "";
        switch (_mode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                break;
            case 1:
                switch (_productionIndex)
                {
                    case >= 0 and <= 3:
                        if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                        {
                            if (_place.CanProduce(_productionIndex, false, ref reason))
                            {
                                _place.Produce(_productionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (_place.CanResearch(_productionIndex, false, ref reason))
                            {
                                _place.Research(_productionIndex, false);
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
                            _place.Trade(false);
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
                            _place.Raze(StrategicMap.Current.SeatedPlayer);
                            Window.Visible = false;
                        }
                        else
                        {
                            StrategicMap.Current.ShowWarning(reason);
                        }

                        break;
                }

                break;
            case 2:
                if (QueueList.Selected != -1)
                {
                    _place.ProductionControl.DeleteProduction(QueueList.Selected);
                }

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
                            if (_place.CanProduce(_productionIndex, true, ref reason))
                            {
                                _place.Produce(_productionIndex, true);
                            }
                            else
                            {
                                StrategicMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (_place.CanResearch(_productionIndex, true, ref reason))
                            {
                                _place.Research(_productionIndex, true);
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
                            _place.Trade(true);
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
        Unit u;
        Rect r;
        u = _place.ProductionControl.CreateUnitProduction(_productionIndex);
        if (u != null)
        {
            r = new Rect(UnitPictureFramePanel.WinLeft + sender.WinLeft + 2, UnitPictureFramePanel.WinTop + sender.WinTop + 2, UnitPictureFramePanel.WinLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.WinTop + sender.WinTop + sender.WinHeight - 2);
            u.ShowFace(r, lockedSurface);
            u.Release();
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
            sender.CaptureInput = unitProduction != null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange() => _place.ProductionControl.SetLoopQueueTo(LoopQueueCheckBox.Checked);

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        Rect r;
        Unit u;
        if (_place.ProductionControl.Queue.GetCount() != 0)
        {
            if (_place.ProductionControl.Queue.GetItem(0).Order.UnitProductionIndex != null)
            {
                r = new Rect(ActiveActionPanel.WinLeft + sender.WinLeft, ActiveActionPanel.WinTop + sender.WinTop, ActiveActionPanel.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.WinTop + sender.WinTop + sender.WinHeight);
                ProductionPlaceControl productionPlaceControl = _place.ProductionControl;
                if (productionPlaceControl.UnitProductions.GetItem(productionPlaceControl.Settings.UnitProductionIndex) != null)
                {
                    u = _place.ProductionControl.CreateUnitProduction(_place.ProductionControl.Settings.UnitProductionIndex);
                    if (u != null)
                    {
                        info.Direction = 3;
                        info.Overlays = Set<UnitOverlayModesElement>.Of();
                        info.Animation = 0;
                        info.Frame = 0;
                        info.Looped = false;
                        u.Show(r.Left - 12, r.Top - 5, lockedSurface, info);
                        u.Release();
                    }
                }
            }
        }
    }

    // AoWProdScreen.pas:483
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

    // AoWProdScreen.pas:510
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

    // AoWProdScreen.pas:521
    public void MapViewUpdate()
    {
        CombatMap preview;
        preview = _preview;
        preview?.NewFrame();

        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // AoWProdScreen.pas:530
    public void DlgHide() => ClosePlace();

    // AoWProdScreen.pas:535
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

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int start;
        int index;
        PlayerStructure s;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is ProductionPlace && ((ProductionPlace)s).GetMagicSphere() == MagicSphere.None)
            {
                index = start + 1;
                ShowPlace((ProductionPlace)s);
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
        int start;
        int index;
        PlayerStructure s;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            s = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (s is ProductionPlace && ((ProductionPlace)s).GetMagicSphere() == MagicSphere.None)
            {
                index = start - 1;
                ShowPlace((ProductionPlace)s);
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
            _ => -1,
        };
    }
}
