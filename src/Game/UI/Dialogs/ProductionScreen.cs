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
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// view, place, production, queue, mode, preview, images
public partial class ProductionScreen : DataModule
{
    public ProductionScreen(Component owner) : base(owner)
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
    public Button MainBtn3;
    public Button MainBtn1;
    public ListBox QueueLB;
    public Panel ActivityPnl;
    public Panel InfoPnl2;
    public Label ActiveAction;
    public Label ActiveActionTurns;
    public Panel ActivityPnl2;
    public Gauge ActiveActionTurnsBar;
    public Button MainBtn2;
    public Panel InfoPnl;
    public Frame IPPicFr;
    public Memo PlaceInfoMemo;
    public Frame IncomePicture;
    public Memo IncomeMemo;
    public Memo IncomeHdr;
    public Label QueueLbl;
    public CheckBox LoopQueueBox;
    public Label RepeatLbl;
    public Panel ActiveActionPnl;
    public Image CurrentActionImage;
    public Panel UPictureFramePnl;
    public Image UpgradePic;
    public Label UpgradeCost;
    public Label UpgradeTurns;
    public Memo IncomeValuesMemo;
    public Button Sphere;
    public UserDrawBox MapView;
    public Button PNext;
    public Button PPrev;
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
            AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        _images.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int index)
    {
        int graphics;
        graphics = AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        _images.Add(graphics);
    }

    // AoWProdScreen.pas:617
    public void ResetChecks()
    {
        B1CB.Checked = false;
        B2CB.Checked = false;
        B3CB.Checked = false;
        B4CB.Checked = false;
        B5CB.Checked = false;
        B6CB.Checked = false;
    }

    // AoWProdScreen.pas:627
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
        MainBtn1.Visible = true;
        var options = AbilityRules.TranslateResourceString(GameText.OptionsText);
        MainBtn1.Text = options;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        ShadowL.Visible = GameTables.GameEngine.AnimationDetail != 0;
        ShadowR.Visible = GameTables.GameEngine.AnimationDetail != 0;
        Oval.Visible = GameTables.GameEngine.AnimationDetail != 0;
        _queueIndex = -1;
        QueueLB.Selected = -1;
        var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
        QueueLbl.Text = queue;
        var repeat = AbilityRules.TranslateResourceString(GameText.RepeatText);
        RepeatLbl.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        _place = null;
        DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWProdScreen.pas:659
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
                MainBtn1.Text = change;
                MainBtn2.Visible = false;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                var exit = AbilityRules.TranslateResourceString(GameText.ExitText);
                MainBtn3.Text = exit;
                QueueLB.Selected = -1;
                break;
            case 1:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var info = AbilityRules.TranslateResourceString(GameText.InfoText);
                MainBtn1.Text = info;
                var done = AbilityRules.TranslateResourceString(GameText.DoneText);
                MainBtn3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var infoText = AbilityRules.TranslateResourceString(GameText.InfoText);
                MainBtn1.Text = infoText;
                var doneText = AbilityRules.TranslateResourceString(GameText.DoneText);
                MainBtn3.Text = doneText;
                break;
        }
    }

    // AoWProdScreen.pas:698
    public void SelectDefaultAction()
    {
        ResetChecks();
        B1CB.Checked = true;
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
        if (Dlg.Visible && sender == _place)
        {
            if (_place.Player == AoWHSMap.Current.SeatedPlayer)
            {
                UpdateInfo();
            }
            else
            {
                Dlg.Visible = false;
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

        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        _productionIndex = index;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        MainBtn1.Visible = false;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        switch (_productionIndex)
        {
            case >= 0 and <= 3:
                U = _place.ProductionControl.CreateUnitProduction(_productionIndex);
                if (U != null)
                {
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
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.AlignmentFormat), [alignmentText]);
                    UAlignment.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.LevelFormat), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.UpkeepFormat), [(int)upkeep]);
                    ULevel.Text = formattedText + " / " + format2;
                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (_place.ProductionControl.UnitProductions.GetItem(_productionIndex).Researched)
                    {
                        MainBtn1.Visible = true;
                        var produce = AbilityRules.TranslateResourceString(GameText.ProduceText);
                        MainBtn1.Text = produce;
                        var queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.ProduceFormat), [name]);
                        UnitLbl.Text = format3;
                        var unitProductionGold = _place.ProductionControl.UnitProductionGold(_productionIndex);
                        UCost.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = _place.ProductionControl.UnitProductionTime(_productionIndex);
                        UTurns.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainBtn1.Visible = true;
                        var install = AbilityRules.TranslateResourceString(GameText.InstallText);
                        MainBtn1.Text = install;
                        var queueLocked = _place.ProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateResourceString(GameText.QueueText);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.InstallFormat), [name]);
                        UnitLbl.Text = format3;
                        var unitResearchGold = _place.ProductionControl.UnitResearchGold(_productionIndex);
                        UCost.Text = IntToStr(unitResearchGold);
                        var unitResearchTime = _place.ProductionControl.UnitResearchTime(_productionIndex);
                        UTurns.Text = IntToStr(unitResearchTime);
                    }

                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    U.Release();
                }
                else
                {
                    UnitPnl.Visible = false;
                }

                break;
            case 4:
                MainBtn1.Visible = true;
                var productionName = new Trade(0).Name(_place.ProductionControl);
                MainBtn1.Text = productionName;
                var fProductionControlQueueLocked = _place.ProductionControl.GetQueueLocked();
                MainBtn2.Visible = !fProductionControlQueueLocked;
                var queueText = AbilityRules.TranslateResourceString(GameText.QueueText);
                MainBtn2.Text = queueText;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                if (_place.GetMagicSphere() == MagicSphere.None)
                {
                    var produceMerchandiseText = AbilityRules.TranslateResourceString(s_produceMerchandiseText);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    var generateExtraPowerText = AbilityRules.TranslateResourceString(s_generateExtraPowerText);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePnl.Visible = true;
                if (_place.GetMagicSphere() != MagicSphere.None)
                {
                    UpgradePic.CurrentImage = 28;
                }
                else
                {
                    UpgradePic.CurrentImage = 27;
                }

                UpgradeLbl.Text = MainBtn1.Text;
                break;
            case 5:
                MainBtn1.Visible = true;
                var raze = AbilityRules.TranslateResourceString(GameText.RazeText);
                MainBtn1.Text = raze + "...";
                PPrev.Visible = false;
                PNext.Visible = false;
                var fPlaceName = _place.GetName();
                var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_productionPlaceRazeFormat), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = AbilityRules.TranslateResourceString(GameText.RazeText);
                UpgradeLbl.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string reason;
        IncomeHdr.Items.Clear();
        var name = _place.GetName();
        StructName.Text = name;
        var income = AbilityRules.TranslateResourceString(GameText.IncomeText);
        IncomeHdr.Items.Add(income);
        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            var power = _place.GetPower();
            var xPower = AbilityRules.TranslateResourceString(s_powerFormat);
            var format = AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHdr.Items.Add(format);
        }

        if (_place.GetIncome() != 0)
        {
            var fPlaceIncome = _place.GetIncome();
            var xGold = AbilityRules.TranslateResourceString(GameText.GoldFormat);
            var format = AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHdr.Items.Add(format);
        }

        if (_place.GetMagicSphere() != MagicSphere.None)
        {
            IncomePicture.ImageLibraryIndexTexture = 64;
            IPPicFr.ImageLibraryTexture = null;
            var sphereImage = SphereImage(_place.GetMagicSphere());
            Sphere.SetImageLibraryUIndex(sphereImage);
            Sphere.Visible = true;
        }
        else
        {
            if (_place is Shipyard)
            {
                IncomeHdr.Items.Clear();
                IncomePicture.ImageLibraryIndexTexture = 68;
            }
            else
            {
                IncomePicture.ImageLibraryIndexTexture = 65;
            }

            IPPicFr.ImageLibraryTexture = DialogHosts.UIGraphics.CityWin;
            if (_place is Shipyard)
            {
                IPPicFr.ImageLibraryIndexTexture = 67;
            }
            else
            {
                IPPicFr.ImageLibraryIndexTexture = 66;
            }

            Sphere.Visible = false;
        }

        IncomeMemo.Items.Clear();
        IncomeValuesMemo.Items.Clear();
        _place.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueLB.Items.Clear();
        for (int I = 0, last = _place.ProductionControl.Queue.GetCount() - 1; I <= last; I++)
        {
            var name2 = _place.ProductionControl.Queue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = _place.ProductionControl.Queue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = AbilityRules.TranslateResourceString(GameText.EmptyQueueText);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = _place.ProductionControl.Queue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = _place.ProductionControl.Queue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            _queueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = _place.ProductionControl.LoopQueue;
        if (_place.ProductionControl.Status == ProductionControlStatus.Producing)
        {
            var nameProgressive = _place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (_place.ProductionControl.Settings.TurnsLeft != -1)
            {
                var formatTurnsLeftString = DurationText.FormatTurnsLeftString(_place.ProductionControl.Settings.TurnsLeft);
                ActiveActionTurns.Text = formatTurnsLeftString;
                ActiveActionTurns.Visible = true;
                ActiveActionTurns.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = _place.ProductionControl.Settings.TurnsTotal;
                this.ActiveActionTurnsBar.Position = _place.ProductionControl.Settings.TurnsTotal - _place.ProductionControl.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurns.Visible = false;
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
                ActiveAction.Text = currentActivityText;
                var defaultText = AbilityRules.TranslateResourceString(GameText.DefaultText);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                var none = AbilityRules.TranslateResourceString(GameTables.NoneText);
                if (CompareText(ActiveAction.Text, none) != 0)
                {
                    ActiveActionTurns.Visible = true;
                    var activityImage = new Trade(0).ActivityImage(_place);
                    CurrentActionImage.CurrentImage = activityImage;
                }
                else
                {
                    ActiveActionTurns.Visible = false;
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
                    ActiveAction.Text = nameProgressive;
                    var queue = _place.ProductionControl.Queue.GetItem(0);
                    _place.ProductionControl.CanExecuteProduction(queue, ref reason);
                    ActiveActionTurns.Text = reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurns.Visible = true;
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

        Dlg.Drawn = false;
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
        army = ((AoWMapField)_place.Owner).GetArmyHexSprite();
        if (army != null)
        {
            for (int I = 0, last = army.Army.GetCount() - 1; I <= last; I++)
            {
                var troop = army.Army.GetUnit(I);
                CombatMap.PlaceOverviewUnit(_preview, troop, 0, I);
            }
        }

        var baseIncome = _place.GetBaseIncome();
        B5CB.CaptureInput = baseIncome != 0;
        center = _preview.CenterViewLocation;
        if (center != null)
        {
            var fCenterViewLocationHexY = _preview.CenterViewLocation.GetHexY();
            HexGeometry.HexToPixel(center.GetHexX(), fCenterViewLocationHexY, ref _viewX, ref _viewY);
            _viewX = _viewX + 16 - MapView.WinWidth / 2;
            _viewY = _viewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            _viewX = 800;
            _viewY = 800;
        }

        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
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
        DialogHosts.General.PlayButtonSound(1);
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
        R = new Rect(ActivityPnl2.WinLeft + sender.WinLeft, ActivityPnl2.WinTop + sender.WinTop, ActivityPnl2.WinLeft + sender.WinLeft + sender.WinWidth, ActivityPnl2.WinTop + sender.WinTop + sender.WinHeight);
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

                    U.Release();
                }
                else
                {
                    DialogHosts.UIGraphics.CityWin[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }

                break;
            case 4:
                if (_place.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(_place);
                    DialogHosts.UIGraphics.CityWin[activityImage].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }
                else
                {
                    DialogHosts.UIGraphics.CityWin[62].Show(R.Left + 2, R.Top + 2, lockedSurface);
                }

                break;
            case 5:
                DialogHosts.UIGraphics.CityWin[46].Show(R.Left + 2, R.Top + 2, lockedSurface);
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
                                AoWHSMap.Current.ShowWarning(reason);
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
                                AoWHSMap.Current.ShowWarning(reason);
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
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case 5:
                        if (_place.CanRaze(AoWHSMap.Current.SeatedPlayer, ref reason))
                        {
                            _place.Raze(AoWHSMap.Current.SeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                }

                break;
            case 2:
                if (QueueLB.Selected != -1)
                {
                    _place.ProductionControl.DeleteProduction(QueueLB.Selected);
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
                                AoWHSMap.Current.ShowWarning(reason);
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
                                AoWHSMap.Current.ShowWarning(reason);
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
                            AoWHSMap.Current.ShowWarning(reason);
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
                        AoWHSMap.Current.ShowWarning(reason);
                    }
                }

                break;
        }
    }

    // AoWProdScreen.pas:383
    public void MainBtn3Click()
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
    }

    // AoWProdScreen.pas:389
    public void UFaceDraw(UserDrawBox sender, LockedSurface lockedSurface)
    {
        Unit U;
        Rect R;
        U = _place.ProductionControl.CreateUnitProduction(_productionIndex);
        if (U != null)
        {
            R = new Rect(UPictureFramePnl.WinLeft + sender.WinLeft + 2, UPictureFramePnl.WinTop + sender.WinTop + 2, UPictureFramePnl.WinLeft + sender.WinLeft + sender.WinWidth - 4, UPictureFramePnl.WinTop + sender.WinTop + sender.WinHeight - 2);
            U.ShowFace(R, lockedSurface);
            U.Release();
        }
    }

    // AoWProdScreen.pas:406
    public void QueueLBChange()
    {
        var count = _place.ProductionControl.Queue.GetCount();
        if (QueueLB.Selected < count && _mode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                _queueIndex = QueueLB.Selected;
                if (_place.ProductionControl.Queue.GetItem(_queueIndex).Order is ProduceUnit produce)
                {
                    SelectProduction(produce.Index);
                }
                else
                {
                    SelectProduction(4);
                }

                SetMode(2);
                QueueLB.Selected = _queueIndex;
                MainBtn1.Visible = true;
                var delete = AbilityRules.TranslateResourceString(GameText.DeleteText);
                MainBtn1.Text = delete;
                if (_queueIndex == 0 && _place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    MainBtn2.Visible = true;
                    var execute = AbilityRules.TranslateResourceString(GameText.ExecuteText);
                    MainBtn2.Text = execute;
                }
                else
                {
                    MainBtn2.Visible = false;
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
                QueueLB.Selected = _queueIndex;
            }
            else
            {
                QueueLB.Selected = -1;
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
        _place.ProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
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
                R = new Rect(ActiveActionPnl.WinLeft + sender.WinLeft, ActiveActionPnl.WinTop + sender.WinTop, ActiveActionPnl.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPnl.WinTop + sender.WinTop + sender.WinHeight);
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
                Dlg.Visible = false;
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

        MapView.Drawn = MapView.Drawn && (AoWHSMap.Current.Frame & 1) != 0;
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

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start;
        index -= 1;
        if (index < 0)
        {
            index = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1;
        }

        while (index != start)
        {
            S = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                index = start + 1;
                ShowPlace((ProductionPlace)S);
            }

            index += -1;
            if (index < 0)
            {
                index = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount() - 1;
            }
        }
    }

    // AoWProdScreen.pas:568
    public void PNextClick()
    {
        int start;
        int index;
        PlayerStructure S;
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(_place);
        index = start + 1;
        if (index >= AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount())
        {
            index = 0;
        }

        while (index != start)
        {
            S = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetStructure(index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                index = start - 1;
                ShowPlace((ProductionPlace)S);
            }

            index += 1;
            if (index >= AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.GetCount())
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
