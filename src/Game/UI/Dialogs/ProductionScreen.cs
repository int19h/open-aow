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
using Aow.Game.World.Structures;
using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// view, place, production, queue, mode, preview, images
public partial class ProductionScreen : UIModule
{
    public ProductionScreen(UIContext ui) : base(ui)
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
    public Button MainButton3;
    public Button MainButton1;
    public ListBox QueueList;
    public Panel ActivityPanel;
    public Panel InfoPanel2;
    public Label ActiveActionLabel;
    public Label ActiveActionTurnsLabel;
    public Panel ActivityPanel2;
    public Gauge ActiveActionTurnsBar;
    public Button MainButton2;
    public Panel InfoPanel;
    public Frame PlacePictureFrame;
    public Memo PlaceInfoMemo;
    public Frame IncomePictureFrame;
    public Memo IncomeMemo;
    public Memo IncomeHeader;
    public Label QueueLabel;
    public CheckBox LoopQueueCheckBox;
    public Label RepeatLabel;
    public Panel ActiveActionPanel;
    public Image CurrentActionImage;
    public Panel UnitPictureFramePanel;
    public Image UpgradePicture;
    public Label UpgradeCostLabel;
    public Label UpgradeTurnsLabel;
    public Memo IncomeValuesMemo;
    public Button SphereButton;
    public UserDrawBox MapView;
    public Button NextButton;
    public Button PreviousButton;
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
        for (int I = _images.Count - 1; I >= 0; I--)
        {
            var image = _images[I];
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
        var options = UI.Engine.TranslateResourceString(GameText.OptionsText);
        MainButton1.Text = options;
        MainButton2.Visible = false;
        PreviousButton.Visible = !MainButton2.Visible;
        NextButton.Visible = !MainButton2.Visible;
        LeftShadow.Visible = UI.Engine.AnimationDetail != 0;
        RightShadow.Visible = UI.Engine.AnimationDetail != 0;
        Oval.Visible = UI.Engine.AnimationDetail != 0;
        _queueIndex = -1;
        QueueList.Selected = -1;
        var queue = UI.Engine.TranslateResourceString(GameText.QueueText);
        QueueLabel.Text = queue;
        var repeat = UI.Engine.TranslateResourceString(GameText.RepeatText);
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
                var change = UI.Engine.TranslateResourceString(GameText.ChangeText);
                MainButton1.Text = change;
                MainButton2.Visible = false;
                PreviousButton.Visible = !MainButton2.Visible;
                NextButton.Visible = !MainButton2.Visible;
                var exit = UI.Engine.TranslateResourceString(GameText.ExitText);
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
                var info = UI.Engine.TranslateResourceString(GameText.InfoText);
                MainButton1.Text = info;
                var done = UI.Engine.TranslateResourceString(GameText.DoneText);
                MainButton3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueList.Selected = -1;
                InfoPanel.Visible = false;
                InfoPanel2.Visible = false;
                ActivityPanel.Visible = true;
                ActivityPanel2.Visible = true;
                var infoText = UI.Engine.TranslateResourceString(GameText.InfoText);
                MainButton1.Text = infoText;
                var doneText = UI.Engine.TranslateResourceString(GameText.DoneText);
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
        Unit U;
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
                U = _place.ProductionControl.CreateUnitProduction(_productionIndex);
                if (U != null)
                {
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
                    var moveTypesToInt32 = UI.General.MoveTypesToInt32(moveTypes);
                    UnitMovesIcon.CurrentImage = moveTypesToInt32;
                    var alignment = U.GetAlignment();
                    var alignmentText = UI.Engine.TranslateResourceString(GameText.AlignmentTexts[(int)alignment]);
                    var format = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                    UnitAlignmentLabel.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var formattedText = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                    UnitLevelLabel.Text = formattedText + " / " + format2;
                    UnitAbilitiesList.Items.Clear();
                    U.ListAbilities(UnitAbilitiesList.Items);
                    if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                    {
                        MainButton1.Visible = true;
                        var produce = UI.Engine.TranslateResourceString(GameText.ProduceText);
                        MainButton1.Text = produce;
                        var queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainButton2.Visible = !queueLocked;
                        var queue = UI.Engine.TranslateResourceString(GameText.QueueText);
                        MainButton2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.ProduceFormat), [name]);
                        UnitLabel.Text = format3;
                        var unitProductionGold = _place.ProductionControl.UnitProductionGold(_productionIndex);
                        UnitCostLabel.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = _place.ProductionControl.UnitProductionTime(_productionIndex);
                        UnitTurnsLabel.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainButton1.Visible = true;
                        var install = UI.Engine.TranslateResourceString(GameText.InstallText);
                        MainButton1.Text = install;
                        var queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainButton2.Visible = !queueLocked;
                        var queue = UI.Engine.TranslateResourceString(GameText.QueueText);
                        MainButton2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(UI.Engine.TranslateResourceString(GameText.InstallFormat), [name]);
                        UnitLabel.Text = format3;
                        var unitResearchGold = _place.ProductionControl.UnitResearchGold(_productionIndex);
                        UnitCostLabel.Text = IntToStr(unitResearchGold);
                        var unitResearchTime = _place.ProductionControl.UnitResearchTime(_productionIndex);
                        UnitTurnsLabel.Text = IntToStr(unitResearchTime);
                    }

                    PreviousButton.Visible = !MainButton2.Visible;
                    NextButton.Visible = !MainButton2.Visible;
                    U.Release();
                }
                else
                {
                    UnitPanel.Visible = false;
                }

                break;
            case 4:
                MainButton1.Visible = true;
                var productionName = new Trade(0).Name(_place.ProductionControl);
                MainButton1.Text = productionName;
                var fProductionControlQueueLocked = _place.ProductionControl.GetQueueLocked();
                MainButton2.Visible = !fProductionControlQueueLocked;
                var queueText = UI.Engine.TranslateResourceString(GameText.QueueText);
                MainButton2.Text = queueText;
                PreviousButton.Visible = !MainButton2.Visible;
                NextButton.Visible = !MainButton2.Visible;
                if (_place.GetMagicSphere() == MagicSphere.None)
                {
                    var produceMerchandiseText = UI.Engine.TranslateResourceString(s_produceMerchandiseText);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    var generateExtraPowerText = UI.Engine.TranslateResourceString(s_generateExtraPowerText);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePanel.Visible = true;
                if (_place.GetMagicSphere() != MagicSphere.None)
                {
                    UpgradePicture.CurrentImage = 28;
                }
                else
                {
                    UpgradePicture.CurrentImage = 27;
                }

                UpgradeLabel.Text = MainButton1.Text;
                break;
            case 5:
                MainButton1.Visible = true;
                var raze = UI.Engine.TranslateResourceString(GameText.RazeText);
                MainButton1.Text = raze + "...";
                PreviousButton.Visible = false;
                NextButton.Visible = false;
                var fPlaceName = _place.GetName();
                var format4 = AbilityRules.Format(UI.Engine.TranslateResourceString(s_productionPlaceRazeFormat), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePanel.Visible = true;
                UpgradePicture.CurrentImage = 26;
                var razeText = UI.Engine.TranslateResourceString(GameText.RazeText);
                UpgradeLabel.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string reason;
        IncomeHeader.Items.Clear();
        var name = _place.GetName();
        StructureNameLabel.Text = name;
        var income = UI.Engine.TranslateResourceString(GameText.IncomeText);
        IncomeHeader.Items.Add(income);
        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            var power = _place.GetPower();
            var xPower = UI.Engine.TranslateResourceString(s_powerFormat);
            var format = AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHeader.Items.Add(format);
        }

        if (_place.GetIncome() != 0)
        {
            var fPlaceIncome = _place.GetIncome();
            var xGold = UI.Engine.TranslateResourceString(GameText.GoldFormat);
            var format = AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHeader.Items.Add(format);
        }

        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            IncomePictureFrame.ImageLibraryIndexTexture = 64;
            PlacePictureFrame.ImageLibraryTexture = null;
            var sphereImage = SphereImage(_place.GetMagicSphere());
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
            if (_place is Shipyard)
            {
                PlacePictureFrame.ImageLibraryIndexTexture = 67;
            }
            else
            {
                PlacePictureFrame.ImageLibraryIndexTexture = 66;
            }

            SphereButton.Visible = false;
        }

        IncomeMemo.Items.Clear();
        IncomeValuesMemo.Items.Clear();
        _place.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueList.Items.Clear();
        for (int I = 0, last = _place.ProductionControl.Queue.GetCount() - 1; I <= last; I++)
        {
            var name2 = _place.ProductionControl.Queue.GetItem(I).GetName();
            QueueList.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = _place.ProductionControl.Queue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = UI.Engine.TranslateResourceString(GameText.EmptyQueueText);
            QueueList.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = _place.ProductionControl.Queue.GetCount();
        if (QueueList.Selected >= count)
        {
            var fQueueCount = _place.ProductionControl.Queue.GetCount();
            QueueList.Selected = fQueueCount - 1;
            _queueIndex = QueueList.Selected;
        }

        LoopQueueCheckBox.Checked = _place.ProductionControl.LoopQueue;
        if (_place.ProductionControl.Status == ProductionControlStatus.Producing)
        {
            var nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
            ActiveActionLabel.Text = nameProgressive;
            if (_place.ProductionControl.Settings.TurnsLeft != -1)
            {
                var formatTurnsLeftString = DurationText.FormatTurnsLeftString(UI.Engine, turns: _place.ProductionControl.Settings.TurnsLeft);
                ActiveActionTurnsLabel.Text = formatTurnsLeftString;
                ActiveActionTurnsLabel.Visible = true;
                ActiveActionTurnsLabel.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = _place.ProductionControl.Settings.TurnsTotal;
                this.ActiveActionTurnsBar.Position = _place.ProductionControl.Settings.TurnsTotal - _place.ProductionControl.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurnsLabel.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            var activityImage = _place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(_place);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (_place.ProductionControl.Status == ProductionControlStatus.Idle)
            {
                var currentActivityText = _place.GetCurrentActivityText();
                ActiveActionLabel.Text = currentActivityText;
                var defaultText = UI.Engine.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurnsLabel.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurnsLabel.TextColor = 6783862;
                var none = UI.Engine.TranslateResourceString(GameTables.NoneText);
                if (CompareText(ActiveActionLabel.Text, none) != 0)
                {
                    ActiveActionTurnsLabel.Visible = true;
                    var activityImage = new Trade(0).ActivityImage(_place);
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
                    var nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
                    ActiveActionLabel.Text = nameProgressive;
                    var queue = _place.ProductionControl.Queue.GetItem(0);
                    _place.ProductionControl.CanExecuteProduction(queue, ref reason);
                    ActiveActionTurnsLabel.Text = reason;
                    ActiveActionTurnsLabel.TextColor = 255;
                    ActiveActionTurnsLabel.Visible = true;
                    ActiveActionTurnsBar.Visible = false;
                    var activityImage = _place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(_place);
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
        var text = place.GetDescription().Text;
        PlaceInfoMemo.Items.Text = text;
        for (int I = 0, last = _place.ProductionControl.UnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(_place.ProductionControl.UnitProductions.GetItem(I).UnitIndex);
        }

        FreePreview();
        generator = new RandomBattleMapGenerator();
        var hexLevel = _place.GetHexLevel();
        var hexY = _place.GetHexY();
        _preview = generator.RandomMap(null, _place.GetHexX(), hexY, hexLevel);
        generator?.Dispose();
        army = _place.Owner.GetArmyHexSprite();
        if (army != null)
        {
            for (int I = 0, last = army.Army.GetCount() - 1; I <= last; I++)
            {
                var troop = army.Army.GetUnit(I);
                CombatMap.PlaceOverviewUnit(_preview, troop, 0, I);
            }
        }

        var baseIncome = _place.GetBaseIncome();
        Action5CheckBox.CaptureInput = baseIncome != 0;
        center = _preview.CenterViewLocation;
        if (center != null)
        {
            var fCenterViewLocationHexY = _preview.CenterViewLocation.GetHexY();
            HexGeometry.HexToPixel(center.GetHexX(), fCenterViewLocationHexY, out _viewX, out _viewY);
            _viewX = _viewX + 16 - MapView.WinWidth / 2;
            _viewY = _viewY + 16 - MapView.WinHeight / 2;
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
    public void ProductionScreenCreate()
    {
        _images = new Int32List();
    }

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy()
    {
        _images?.Dispose();
    }

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
        Rect R;
        Unit U;
        R = new Rect(ActivityPanel2.WinLeft + sender.WinLeft, ActivityPanel2.WinTop + sender.WinTop, ActivityPanel2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPanel2.WinTop + sender.WinTop + sender.WinHeight);
        switch (slot)
        {
            case >= 0 and <= 3:
                U = _place.ProductionControl.CreateUnitProduction(slot);
                if (U != null)
                {
                    info.Direction = 3;
                    info.Overlays = Set<UnitOverlayModesElement>.Of();
                    info.Animation = 0;
                    info.Frame = 0;
                    info.Looped = false;
                    if (!_place.ProductionControl.UnitProductions.GetItem(slot).Researched)
                    {
                        UI.Graphics.CityWindow[5].Show(R.Left + 2, R.Top + 2, lockedSurface);
                        info.Direction = 5;
                        if (U != null)
                        {
                            U.Show(R.Left - 4, R.Top, lockedSurface, info);
                        }

                        var lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(45);
                        UI.Graphics.CityWindow[5].ShowBlended(R.Left + 2, R.Top + 2, lockedSurface, lookupTable);
                    }
                    else
                    {
                        UI.Graphics.CityWindow[4].Show(R.Left + 2, R.Top + 2, lockedSurface);
                        if (U != null)
                        {
                            U.Show(R.Left - 4, R.Top, lockedSurface, info);
                        }
                    }

                    U.Release();
                }
                else
                {
                    UI.Graphics.CityWindow[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }

                break;
            case 4:
                if (_place.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(_place);
                    UI.Graphics.CityWindow[activityImage].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }
                else
                {
                    UI.Graphics.CityWindow[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }

                break;
            case 5:
                UI.Graphics.CityWindow[46].Show(R.Left + 2, R.Top + 2, lockedSurface);
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
        Unit U;
        Rect R;
        U = _place.ProductionControl.CreateUnitProduction(_productionIndex);
        if (U != null)
        {
            R = new Rect(UnitPictureFramePanel.WinLeft + sender.WinLeft + 2, UnitPictureFramePanel.WinTop + sender.WinTop + 2, UnitPictureFramePanel.WinLeft + sender.WinLeft + sender.WinWidth - 4, UnitPictureFramePanel.WinTop + sender.WinTop + sender.WinHeight - 2);
            U.ShowFace(R, lockedSurface);
            U.Release();
        }
    }

    // AoWProdScreen.pas:406
    public void QueueLBChange()
    {
        var count = _place.ProductionControl.Queue.GetCount();
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
                var delete = UI.Engine.TranslateResourceString(GameText.DeleteText);
                MainButton1.Text = delete;
                if (_queueIndex == 0 && _place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    MainButton2.Visible = true;
                    var execute = UI.Engine.TranslateResourceString(GameText.ExecuteText);
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
            if (_mode == 2)
            {
                QueueList.Selected = _queueIndex;
            }
            else
            {
                QueueList.Selected = -1;
            }
        }
    }

    // AoWProdScreen.pas:439
    public void B4CBUpdate(CheckBox sender, int slot)
    {
        if (slot <= 3)
        {
            var unitProduction = _place.ProductionControl.UnitProductions.GetItem(slot);
            sender.CaptureInput = unitProduction != null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange()
    {
        _place.ProductionControl.SetLoopQueueTo(LoopQueueCheckBox.Checked);
    }

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        Rect R;
        Unit U;
        if (_place.ProductionControl.Queue.GetCount() != 0)
        {
            if (_place.ProductionControl.Queue.GetItem(0).Order.UnitProductionIndex != null)
            {
                R = new Rect(ActiveActionPanel.WinLeft + sender.WinLeft, ActiveActionPanel.WinTop + sender.WinTop, ActiveActionPanel.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPanel.WinTop + sender.WinTop + sender.WinHeight);
                var productionPlaceControl = _place.ProductionControl;
                if (productionPlaceControl.UnitProductions.GetItem(productionPlaceControl.Settings.UnitProductionIndex) != null)
                {
                    U = _place.ProductionControl.CreateUnitProduction(_place.ProductionControl.Settings.UnitProductionIndex);
                    if (U != null)
                    {
                        info.Direction = 3;
                        info.Overlays = Set<UnitOverlayModesElement>.Of();
                        info.Animation = 0;
                        info.Frame = 0;
                        info.Looped = false;
                        U.Show(R.Left - 12, R.Top - 5, lockedSurface, info);
                        U.Release();
                    }
                }
            }
        }
    }

    // AoWProdScreen.pas:483
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

            LockedSurface.CreateFrom(lockedSurface, R, out viewSurfaceDescription);
            _preview.SetExplorationEnabled(false);
            _preview.SetFogEnabled(false);
            _preview.SetViewPort(_viewX, _viewY, 0, width, R.Bottom - R.Top);
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
        if (preview != null)
        {
            preview.NewFrame();
        }

        MapView.Drawn = MapView.Drawn && (StrategicMap.Current.Frame & 1) != 0;
    }

    // AoWProdScreen.pas:530
    public void DlgHide()
    {
        ClosePlace();
    }

    // AoWProdScreen.pas:535
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
                InfoBanner.SetInfoBanner(UI.InfoBanner, name, A.Description);
            }
        }
    }

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            S = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                index = start + 1;
                ShowPlace((ProductionPlace)S);
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
        PlayerStructure S;
        start = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start + 1;
        if (index >= StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            S = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                index = start - 1;
                ShowPlace((ProductionPlace)S);
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
