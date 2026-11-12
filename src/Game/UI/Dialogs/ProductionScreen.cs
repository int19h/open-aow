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
    public Image UAttackIcon;
    public Image UDefenseIcon;
    public Image UMovesIcon;
    public Image UDamageIcon;
    public Image UResistanceIcon;
    public Image UHitsIcon;
    public Label UAttack;
    public Label UDefense;
    public Label UMoves;
    public Label UDamage;
    public Label UResistance;
    public Label UHits;
    public Panel UStatPnl;
    public Panel UStatP1;
    public Panel UStatP2;
    public Label UAlignment;
    public Label ULevel;
    public Panel UPicturePnl;
    public Panel UCostPnl;
    public Label StructName;
    public Label CostLbl;
    public Label UCost;
    public Label UnitLbl;
    public Label TurnsLbl;
    public Label UTurns;
    public CheckBox B4CB;
    public CheckBox B5CB;
    public CheckBox B6CB;
    public CheckBox B1CB;
    public CheckBox B2CB;
    public CheckBox B3CB;
    public VerticalScrollBar T2SBar;
    public Panel AbilPnl;
    public Panel UpgradePnl;
    public Label UpgradeLbl;
    public Panel UpgradeMemoFr;
    public Memo UpgradeMemo;
    public Panel RightPnl;
    public Panel QueuePnl;
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
    public Image SepBar;
    public Panel ActivityPnl2;
    public Gauge ActiveActionTurnsBar;
    public Button MainBtn2;
    public Panel InfoPnl;
    public Panel IPPnl;
    public Frame IPPicFr;
    public Memo PlaceInfoMemo;
    public Panel IIPnl;
    public Frame IncomePicture;
    public Memo IncomeMemo;
    public Memo IncomeHdr;
    public Panel CurrentInfo;
    public Frame RPillar;
    public Frame LPillar;
    public Panel QueueInfo;
    public Label QueueLbl;
    public CheckBox LoopQueueBox;
    public Label RepeatLbl;
    public Panel ActiveActionPnl;
    public Image CurrentActionImage;
    public Frame Frame1;
    public Image BlackBg2;
    public Panel UPictureFramePnl;
    public Panel UStatFr1;
    public Panel UStatFr2;
    public Panel UInfoFr;
    public Panel CostFr;
    public Panel TurnsFr;
    public Panel UpgPicPnl;
    public Image UpgradePic;
    public Panel UpgPicFr;
    public Panel UpgCostPnl;
    public Label UpgCostLbl;
    public Label UpgradeCost;
    public Label UpgTurnsLbl;
    public Label UpgradeTurns;
    public Panel UpgCostFr;
    public Panel UpgTurnsFr;
    public Image RelIcn;
    public VerticalScrollBar QueueVSB;
    public VerticalScrollBar IPVSB;
    public VerticalScrollBar IIVSB;
    public Memo IncomeValuesMemo;
    public UserDrawBox UFace;
    public Image InfoSep;
    public Button Sphere;
    public UserDrawBox MapView;
    public Button PNext;
    public Button PPrev;
    public IvTranslator Translator;
    public int ViewX;
    public int ViewY;
    public ProductionPlace Place;
    public int ProductionIndex;
    public int QueueIndex;
    public byte Mode;
    public CombatMap Preview;
    public Int32List Images;

    // AoWProdScreen.pas:590
    public void FreePreview()
    {
        if (Preview != null)
        {
            Preview?.Dispose();
            Preview = null;
        }
    }

    // AoWProdScreen.pas:599
    public void ReleaseImages()
    {
        for (int I = Images.Count - 1; I >= 0; I--)
        {
            var image = Images[I];
            AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(image).Release();
        }

        Images.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int index)
    {
        int graphics;
        graphics = AoWHSSet.Current.UnitResources.GetUnitResource(index).UnitGraphicsResourceIndex;
        AoWHSSet.Current.UnitGraphicsResources.GetUnitGraphicsResource(graphics).AddRef();
        Images.Add(graphics);
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
        Mode = 0;
        InfoPnl.Visible = true;
        InfoPnl2.Visible = true;
        ActivityPnl.Visible = false;
        ActivityPnl2.Visible = false;
        UnitPnl.Visible = false;
        UpgradePnl.Visible = false;
        MainBtn1.Visible = true;
        var options = AbilityRules.TranslateResourceString(GameText.s_optionsText);
        MainBtn1.Text = options;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        ShadowL.Visible = GameTables.GameEngine.AnimationDetail != 0;
        ShadowR.Visible = GameTables.GameEngine.AnimationDetail != 0;
        Oval.Visible = GameTables.GameEngine.AnimationDetail != 0;
        QueueIndex = -1;
        QueueLB.Selected = -1;
        var queue = AbilityRules.TranslateResourceString(GameText.s_queueText);
        QueueLbl.Text = queue;
        var repeat = AbilityRules.TranslateResourceString(GameText.s_repeatText);
        RepeatLbl.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        Place = null;
        DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWProdScreen.pas:659
    public void SetMode(byte mode)
    {
        Mode = mode;
        switch (Mode)
        {
            case 0:
                InfoPnl.Visible = true;
                InfoPnl2.Visible = true;
                ActivityPnl.Visible = false;
                ActivityPnl2.Visible = false;
                var change = AbilityRules.TranslateResourceString(GameText.s_changeText);
                MainBtn1.Text = change;
                MainBtn2.Visible = false;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                var exit = AbilityRules.TranslateResourceString(GameText.s_exitText);
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
                var info = AbilityRules.TranslateResourceString(GameText.s_infoText);
                MainBtn1.Text = info;
                var done = AbilityRules.TranslateResourceString(GameText.s_doneText);
                MainBtn3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var infoText = AbilityRules.TranslateResourceString(GameText.s_infoText);
                MainBtn1.Text = infoText;
                var doneText = AbilityRules.TranslateResourceString(GameText.s_doneText);
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
        if (Dlg.Visible && sender == Place)
        {
            if (Place.Player == AoWHSMap.Current.SeatedPlayer)
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
        if (Mode != 1)
        {
            SetMode(1);
        }

        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        ProductionIndex = index;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        MainBtn1.Visible = false;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        switch (ProductionIndex)
        {
            case >= 0 and <= 3:
                U = Place.ProductionControl.CreateUnitProduction(ProductionIndex);
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
                    var alignmentText = AbilityRules.TranslateResourceString(GameText.s_alignmentTexts[(int)alignment]);
                    var format = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_alignmentFormat), [alignmentText]);
                    UAlignment.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var formattedText = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_levelFormat), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_upkeepFormat), [(int)upkeep]);
                    ULevel.Text = formattedText + " / " + format2;
                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (Place.ProductionControl.UnitProductions.GetItem(ProductionIndex).Researched)
                    {
                        MainBtn1.Visible = true;
                        var produce = AbilityRules.TranslateResourceString(GameText.s_produceText);
                        MainBtn1.Text = produce;
                        var queueLocked = Place.ProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateResourceString(GameText.s_queueText);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_produceFormat), [name]);
                        UnitLbl.Text = format3;
                        var unitProductionGold = Place.ProductionControl.UnitProductionGold(ProductionIndex);
                        UCost.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = Place.ProductionControl.UnitProductionTime(ProductionIndex);
                        UTurns.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainBtn1.Visible = true;
                        var install = AbilityRules.TranslateResourceString(GameText.s_installText);
                        MainBtn1.Text = install;
                        var queueLocked = Place.ProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateResourceString(GameText.s_queueText);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateResourceString(GameText.s_installFormat), [name]);
                        UnitLbl.Text = format3;
                        var unitResearchGold = Place.ProductionControl.UnitResearchGold(ProductionIndex);
                        UCost.Text = IntToStr(unitResearchGold);
                        var unitResearchTime = Place.ProductionControl.UnitResearchTime(ProductionIndex);
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
                var productionName = new Trade(0).Name(Place.ProductionControl);
                MainBtn1.Text = productionName;
                var fProductionControlQueueLocked = Place.ProductionControl.GetQueueLocked();
                MainBtn2.Visible = !fProductionControlQueueLocked;
                var queueText = AbilityRules.TranslateResourceString(GameText.s_queueText);
                MainBtn2.Text = queueText;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                if (Place.GetMagicSphere() == MagicSphere.None)
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
                if (Place.GetMagicSphere() != MagicSphere.None)
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
                var raze = AbilityRules.TranslateResourceString(GameText.s_razeText);
                MainBtn1.Text = raze + "...";
                PPrev.Visible = false;
                PNext.Visible = false;
                var fPlaceName = Place.GetName();
                var format4 = AbilityRules.Format(AbilityRules.TranslateResourceString(s_productionPlaceRazeFormat), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = AbilityRules.TranslateResourceString(GameText.s_razeText);
                UpgradeLbl.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string reason;
        IncomeHdr.Items.Clear();
        var name = Place.GetName();
        StructName.Text = name;
        var income = AbilityRules.TranslateResourceString(GameText.s_incomeText);
        IncomeHdr.Items.Add(income);
        if (Place.GetMagicSphere() != MagicSphere.None)
        {
            var power = Place.GetPower();
            var xPower = AbilityRules.TranslateResourceString(s_powerFormat);
            var format = AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHdr.Items.Add(format);
        }

        if (Place.GetIncome() != 0)
        {
            var fPlaceIncome = Place.GetIncome();
            var xGold = AbilityRules.TranslateResourceString(GameText.s_goldFormat);
            var format = AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHdr.Items.Add(format);
        }

        if (Place.GetMagicSphere() != MagicSphere.None)
        {
            IncomePicture.ImageLibraryIndexTexture = 64;
            IPPicFr.ImageLibraryTexture = null;
            var sphereImage = SphereImage(Place.GetMagicSphere());
            Sphere.SetImageLibraryUIndex(sphereImage);
            Sphere.Visible = true;
        }
        else
        {
            if (Place is Shipyard)
            {
                IncomeHdr.Items.Clear();
                IncomePicture.ImageLibraryIndexTexture = 68;
            }
            else
            {
                IncomePicture.ImageLibraryIndexTexture = 65;
            }

            IPPicFr.ImageLibraryTexture = DialogHosts.UIGraphics.CityWin;
            if (Place is Shipyard)
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
        Place.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueLB.Items.Clear();
        for (int I = 0, last = Place.ProductionControl.Queue.GetCount() - 1; I <= last; I++)
        {
            var name2 = Place.ProductionControl.Queue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = Place.ProductionControl.Queue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = AbilityRules.TranslateResourceString(GameText.s_emptyQueueText);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = Place.ProductionControl.Queue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = Place.ProductionControl.Queue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            QueueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = Place.ProductionControl.LoopQueue;
        if (Place.ProductionControl.Status == ProductionControlStatus.Producing)
        {
            var nameProgressive = Place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (Place.ProductionControl.Settings.TurnsLeft != -1)
            {
                var formatTurnsLeftString = DurationText.FormatTurnsLeftString(Place.ProductionControl.Settings.TurnsLeft);
                ActiveActionTurns.Text = formatTurnsLeftString;
                ActiveActionTurns.Visible = true;
                ActiveActionTurns.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = Place.ProductionControl.Settings.TurnsTotal;
                this.ActiveActionTurnsBar.Position = Place.ProductionControl.Settings.TurnsTotal - Place.ProductionControl.Settings.TurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurns.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            var activityImage = Place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(Place);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (Place.ProductionControl.Status == ProductionControlStatus.Idle)
            {
                var currentActivityText = Place.GetCurrentActivityText();
                ActiveAction.Text = currentActivityText;
                var defaultText = AbilityRules.TranslateResourceString(GameText.s_defaultText);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                var none = AbilityRules.TranslateResourceString(GameTables.s_noneText);
                if (CompareText(ActiveAction.Text, none) != 0)
                {
                    ActiveActionTurns.Visible = true;
                    var activityImage = new Trade(0).ActivityImage(Place);
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
                if (Place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    reason = "";
                    var nameProgressive = Place.ProductionControl.Queue.GetItem(0).GetNameProgressive();
                    ActiveAction.Text = nameProgressive;
                    var queue = Place.ProductionControl.Queue.GetItem(0);
                    Place.ProductionControl.CanExecuteProduction(queue, ref reason);
                    ActiveActionTurns.Text = reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurnsBar.Visible = false;
                    var activityImage = Place.ProductionControl.Queue.GetItem(0).Order.ActivityImage(Place);
                    CurrentActionImage.CurrentImage = activityImage;
                }
            }
        }

        switch (Mode)
        {
            case 1:
                if (ProductionIndex != -1)
                {
                    SelectProduction(ProductionIndex);
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
        Place = place;
        UpdateInfo();
        SetMode(0);
        var text = place.GetDescription().Text;
        PlaceInfoMemo.Items.Text = text;
        for (int I = 0, last = Place.ProductionControl.UnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(Place.ProductionControl.UnitProductions.GetItem(I).UnitIndex);
        }

        FreePreview();
        generator = new RandomBattleMapGenerator();
        var hexLevel = Place.GetHexLevel();
        var hexY = Place.GetHexY();
        Preview = generator.RandomMap(null, Place.GetHexX(), hexY, hexLevel);
        generator?.Dispose();
        army = ((AoWMapField)Place.Owner).GetArmyHexSprite();
        if (army != null)
        {
            for (int I = 0, last = army.Army.GetCount() - 1; I <= last; I++)
            {
                var troop = army.Army.GetUnit(I);
                CombatMap.PlaceOverviewUnit(Preview, troop, 0, I);
            }
        }

        var baseIncome = Place.GetBaseIncome();
        B5CB.CaptureInput = baseIncome != 0;
        center = Preview.CenterViewLocation;
        if (center != null)
        {
            var fCenterViewLocationHexY = Preview.CenterViewLocation.GetHexY();
            HexGeometry.HexToPixel(center.GetHexX(), fCenterViewLocationHexY, ref ViewX, ref ViewY);
            ViewX = ViewX + 16 - MapView.WinWidth / 2;
            ViewY = ViewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            ViewX = 800;
            ViewY = 800;
        }

        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWProdScreen.pas:221
    public void ProductionScreenCreate()
    {
        Images = new Int32List();
    }

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy()
    {
        Images?.Dispose();
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
                U = Place.ProductionControl.CreateUnitProduction(slot);
                if (U != null)
                {
                    info.Direction = 3;
                    info.Overlays = Set<UnitOverlayModesElement>.Of();
                    info.Animation = 0;
                    info.Frame = 0;
                    info.Looped = false;
                    if (!Place.ProductionControl.UnitProductions.GetItem(slot).Researched)
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
                if (Place.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(Place);
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
        switch (Mode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                break;
            case 1:
                switch (ProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (Place.ProductionControl.UnitProductions.GetItem(ProductionIndex).Researched)
                        {
                            if (Place.CanProduce(ProductionIndex, false, ref reason))
                            {
                                Place.Produce(ProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (Place.CanResearch(ProductionIndex, false, ref reason))
                            {
                                Place.Research(ProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(reason);
                            }
                        }

                        break;
                    case 4:
                        if (Place.CanTrade(false, ref reason))
                        {
                            Place.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                    case 5:
                        if (Place.CanRaze(AoWHSMap.Current.SeatedPlayer, ref reason))
                        {
                            Place.Raze(AoWHSMap.Current.SeatedPlayer);
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
                    Place.ProductionControl.DeleteProduction(QueueLB.Selected);
                }

                break;
        }
    }

    // AoWProdScreen.pas:347
    public void MainBtn2Click()
    {
        string reason = "";
        switch (Mode)
        {
            case 1:
                switch (ProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (Place.ProductionControl.UnitProductions.GetItem(ProductionIndex).Researched)
                        {
                            if (Place.CanProduce(ProductionIndex, true, ref reason))
                            {
                                Place.Produce(ProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(reason);
                            }
                        }
                        else
                        {
                            if (Place.CanResearch(ProductionIndex, true, ref reason))
                            {
                                Place.Research(ProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(reason);
                            }
                        }

                        break;
                    case 4:
                        if (Place.CanTrade(true, ref reason))
                        {
                            Place.Trade(true);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(reason);
                        }

                        break;
                }

                break;
            case 2:
                if (QueueIndex == 0)
                {
                    if (Place.ProductionControl.RetryProductionExecution(ref reason))
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
        if (Mode == 0)
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
        U = Place.ProductionControl.CreateUnitProduction(ProductionIndex);
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
        var count = Place.ProductionControl.Queue.GetCount();
        if (QueueLB.Selected < count && Mode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                QueueIndex = QueueLB.Selected;
                if (Place.ProductionControl.Queue.GetItem(QueueIndex).Order is ProduceUnit produce)
                {
                    SelectProduction(produce.Index);
                }
                else
                {
                    SelectProduction(4);
                }

                SetMode(2);
                QueueLB.Selected = QueueIndex;
                MainBtn1.Visible = true;
                var delete = AbilityRules.TranslateResourceString(GameText.s_deleteText);
                MainBtn1.Text = delete;
                if (QueueIndex == 0 && Place.ProductionControl.Status == ProductionControlStatus.Waiting)
                {
                    MainBtn2.Visible = true;
                    var execute = AbilityRules.TranslateResourceString(GameText.s_executeText);
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
            if (Mode == 2)
            {
                QueueLB.Selected = QueueIndex;
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
            var unitProduction = Place.ProductionControl.UnitProductions.GetItem(slot);
            sender.CaptureInput = unitProduction != null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange()
    {
        Place.ProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
    }

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(Image sender, LockedSurface lockedSurface)
    {
        UnitShowInfo info = default;
        Rect R;
        Unit U;
        if (Place.ProductionControl.Queue.GetCount() != 0)
        {
            if (Place.ProductionControl.Queue.GetItem(0).Order.UnitProductionIndex != null)
            {
                R = new Rect(ActiveActionPnl.WinLeft + sender.WinLeft, ActiveActionPnl.WinTop + sender.WinTop, ActiveActionPnl.WinLeft + sender.WinLeft + sender.WinWidth, ActiveActionPnl.WinTop + sender.WinTop + sender.WinHeight);
                var productionPlaceControl = Place.ProductionControl;
                if (productionPlaceControl.UnitProductions.GetItem(productionPlaceControl.Settings.UnitProductionIndex) != null)
                {
                    U = Place.ProductionControl.CreateUnitProduction(Place.ProductionControl.Settings.UnitProductionIndex);
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
        if (Preview != null)
        {
            R = new Rect(Dlg.WinLeft + sender.WinLeft, Dlg.WinTop + sender.WinTop, Dlg.WinLeft + sender.WinLeft + sender.WinWidth, Dlg.WinTop + sender.WinTop + sender.WinHeight);
            width = R.Right - R.Left;
            W = width;
            H = R.Bottom - R.Top;
            var maxPixelX = Preview.GetMaxPixelX();
            if (ViewX + W + 16 > maxPixelX)
            {
                ViewX = Preview.GetMaxPixelX() - W - 16;
            }

            var maxPixelY = Preview.GetMaxPixelY();
            if (ViewY + H + 16 > maxPixelY)
            {
                ViewY = Preview.GetMaxPixelY() - H - 16;
            }

            if (ViewX < 16)
            {
                ViewX = 16;
            }

            if (ViewY < 16)
            {
                ViewY = 16;
            }

            LockedSurface.CreateFrom(lockedSurface, R, ref viewSurfaceDescription);
            Preview.SetExplorationEnabled(false);
            Preview.SetFogEnabled(false);
            Preview.SetViewPort(ViewX, ViewY, 0, width, R.Bottom - R.Top);
            Preview.UpdateStaticScene();
            Preview.UpdateDynamicScene();
            Preview.DrawStaticScene(ref viewSurfaceDescription);
            Preview.DrawDynamicScene(ref viewSurfaceDescription);
        }
    }

    // AoWProdScreen.pas:510
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        switch (key)
        {
            case 37:
                ViewX -= 16;
                MapView.Drawn = false;
                break;
            case 39:
                ViewX += 16;
                MapView.Drawn = false;
                break;
            case 38:
                ViewY -= 16;
                MapView.Drawn = false;
                break;
            case 40:
                ViewY += 16;
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
        preview = Preview;
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
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(Place);
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
        start = AoWHSMap.Current.Players.GetPlayers(AoWHSMap.Current.SeatedPlayer).Structures.FindChildIndex(Place);
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
