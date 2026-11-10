// Translated from AoWProdScreen.pas.
using Aow.Engine.Collections;
using Aow.Game.Combat;
using Aow.Game.Draw;

using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World.Map;
using Aow.Game.World.Structures;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// view, place, production, queue, mode, preview, images
public partial class ProductionScreen : DataModule
{
    public ProductionScreen(Component owner) : base(owner)
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
    public AOWButton MainBtn3;
    public AOWButton MainBtn1;
    public AOWListBox QueueLB;
    public AOWPanel ActivityPnl;
    public AOWPanel InfoPnl2;
    public AOWLabel ActiveAction;
    public AOWLabel ActiveActionTurns;
    public AOWImage SepBar;
    public AOWPanel ActivityPnl2;
    public AoWGauge ActiveActionTurnsBar;
    public AOWButton MainBtn2;
    public AOWPanel InfoPnl;
    public AOWPanel IPPnl;
    public AoWFrame IPPicFr;
    public AOWMemo PlaceInfoMemo;
    public AOWPanel IIPnl;
    public AoWFrame IncomePicture;
    public AOWMemo IncomeMemo;
    public AOWMemo IncomeHdr;
    public AOWPanel CurrentInfo;
    public AoWFrame RPillar;
    public AoWFrame LPillar;
    public AOWPanel QueueInfo;
    public AOWLabel QueueLbl;
    public AOWCheckBox LoopQueueBox;
    public AOWLabel RepeatLbl;
    public AOWPanel ActiveActionPnl;
    public AOWImage CurrentActionImage;
    public AoWFrame AoWFrame1;
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
    public AOWImage RelIcn;
    public AOWVScrollBar QueueVSB;
    public AOWVScrollBar IPVSB;
    public AOWVScrollBar IIVSB;
    public AOWMemo IncomeValuesMemo;
    public TaowUserDrawBox UFace;
    public AOWImage InfoSep;
    public AOWButton Sphere;
    public TaowUserDrawBox MapView;
    public AOWButton PNext;
    public AOWButton PPrev;
    public IvTranslator Translator;
    public int FViewX;
    public int FViewY;
    public ProductionPlace FPlace;
    public int FProductionIndex;
    public int FQueueIndex;
    public byte FMode;
    public AoWCombatMap FPreview;
    public IntegerList FImages;

    // AoWProdScreen.pas:590
    public void FreePreview()
    {
        if (FPreview != null)
        {
            FPreview?.Dispose();
            FPreview = null;
        }
    }

    // AoWProdScreen.pas:599
    public void ReleaseImages()
    {
        for (int I = FImages.Count - 1; I >= 0; I--)
        {
            var image = FImages[I];
            AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(image).Release();
        }

        FImages.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int Index)
    {
        int Gfx;
        Gfx = AoWHSSet.Current.FUnitResources.GetUnitResource(Index).FUnitGFXResourceIndex;
        AoWHSSet.Current.FUnitGFXResources.GetUnitGFXResource(Gfx).AddRef();
        FImages.Add(Gfx);
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
        FMode = 0;
        InfoPnl.Visible = true;
        InfoPnl2.Visible = true;
        ActivityPnl.Visible = false;
        ActivityPnl2.Visible = false;
        UnitPnl.Visible = false;
        UpgradePnl.Visible = false;
        MainBtn1.Visible = true;
        var options = AbilityRules.TranslateRStr(GameText.OptionsRStr);
        MainBtn1.Text = options;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        ShadowL.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        ShadowR.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        Oval.Visible = GameTables.AoWEngine.FAnimationDetail != 0;
        FQueueIndex = -1;
        QueueLB.Selected = -1;
        var queue = AbilityRules.TranslateRStr(GameText.QueueRStr);
        QueueLbl.Text = queue;
        var repeat = AbilityRules.TranslateRStr(GameText.RepeatRStr);
        RepeatLbl.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        FPlace = null;
        DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWProdScreen.pas:659
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
                MainBtn1.Text = change;
                MainBtn2.Visible = false;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                var exit = AbilityRules.TranslateRStr(GameText.ExitRStr);
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
                var info = AbilityRules.TranslateRStr(GameText.InfoRStr);
                MainBtn1.Text = info;
                var done = AbilityRules.TranslateRStr(GameText.DoneRStr);
                MainBtn3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var infoText = AbilityRules.TranslateRStr(GameText.InfoRStr);
                MainBtn1.Text = infoText;
                var doneText = AbilityRules.TranslateRStr(GameText.DoneRStr);
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
    public void ProductionEvent(ProductionPlaceEventLog Event)
    {
        ShowPlace(Event.GetProductionPlace());
        SetMode(1);
        SelectDefaultAction();
    }

    // AoWProdScreen.pas:712
    public void PlaceChanged(object Sender)
    {
        if (Dlg.Visible && Sender == FPlace)
        {
            if (FPlace.FPlayer == AoWHSMap.Current.FSeatedPlayer)
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
    public void SelectProduction(int Index)
    {
        Unit U;
        if (FMode != 1)
        {
            SetMode(1);
        }

        UpgradePnl.Visible = false;
        UnitPnl.Visible = false;
        FProductionIndex = Index;
        UpgradePic.CurrentImage = 21;
        UpgradeCost.Text = "-";
        UpgradeTurns.Text = "-";
        MainBtn1.Visible = false;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        switch (FProductionIndex)
        {
            case >= 0 and <= 3:
                U = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
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
                    var mTtoInt = DialogHosts.General.MTtoInt(moveTypes);
                    UMovesIcon.CurrentImage = mTtoInt;
                    var alignment = U.GetAlignment();
                    var alignmentText = AbilityRules.TranslateRStr(GameText.TAlignmentRStr[(int)alignment]);
                    var format = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.AlignmentXRStr), [alignmentText]);
                    UAlignment.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var aoWEFormat = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.LevelXRStr), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.UpkeepXRStr), [(int)upkeep]);
                    ULevel.Text = aoWEFormat + " / " + format2;
                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                    {
                        MainBtn1.Visible = true;
                        var produce = AbilityRules.TranslateRStr(GameText.ProduceRStr);
                        MainBtn1.Text = produce;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateRStr(GameText.QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.ProduceXRStr), [name]);
                        UnitLbl.Text = format3;
                        var unitProductionGold = FPlace.FProductionControl.UnitProductionGold(FProductionIndex);
                        UCost.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = FPlace.FProductionControl.UnitProductionTime(FProductionIndex);
                        UTurns.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainBtn1.Visible = true;
                        var install = AbilityRules.TranslateRStr(GameText.InstallRStr);
                        MainBtn1.Text = install;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = AbilityRules.TranslateRStr(GameText.QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AbilityRules.Format(AbilityRules.TranslateRStr(GameText.InstallXRStr), [name]);
                        UnitLbl.Text = format3;
                        var unitResearchGold = FPlace.FProductionControl.UnitResearchGold(FProductionIndex);
                        UCost.Text = IntToStr(unitResearchGold);
                        var unitResearchTime = FPlace.FProductionControl.UnitResearchTime(FProductionIndex);
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
                var productionName = new Trade(0).Name(FPlace.FProductionControl);
                MainBtn1.Text = productionName;
                var fProductionControlQueueLocked = FPlace.FProductionControl.GetQueueLocked();
                MainBtn2.Visible = !fProductionControlQueueLocked;
                var queueText = AbilityRules.TranslateRStr(GameText.QueueRStr);
                MainBtn2.Text = queueText;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                if (FPlace.GetMagicSphere() == MagicSphere.None)
                {
                    var produceMerchandiseText = AbilityRules.TranslateRStr(ProduceMerchandiseTextRStr);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    var generateExtraPowerText = AbilityRules.TranslateRStr(GenerateExtraPowerTextRStr);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePnl.Visible = true;
                if (FPlace.GetMagicSphere() != MagicSphere.None)
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
                var raze = AbilityRules.TranslateRStr(GameText.RazeRStr);
                MainBtn1.Text = raze + "...";
                PPrev.Visible = false;
                PNext.Visible = false;
                var fPlaceName = FPlace.GetName();
                var format4 = AbilityRules.Format(AbilityRules.TranslateRStr(ProductionPlaceXRazeRStr), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = AbilityRules.TranslateRStr(GameText.RazeRStr);
                UpgradeLbl.Text = razeText;
                break;
        }
    }

    // AoWProdScreen.pas:832
    public void UpdateInfo()
    {
        string Reason;
        IncomeHdr.Items.Clear();
        var name = FPlace.GetName();
        StructName.Text = name;
        var income = AbilityRules.TranslateRStr(GameText.IncomeRStr);
        IncomeHdr.Items.Add(income);
        if (FPlace.GetMagicSphere() != MagicSphere.None)
        {
            var power = FPlace.GetPower();
            var xPower = AbilityRules.TranslateRStr(XPowerRStr);
            var format = AbilityRules.Format(" " + xPower, [IntToStr(power)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetIncome() != 0)
        {
            var fPlaceIncome = FPlace.GetIncome();
            var xGold = AbilityRules.TranslateRStr(GameText.XGoldRStr);
            var format = AbilityRules.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetMagicSphere() != MagicSphere.None)
        {
            IncomePicture.ILIndexTexture = 64;
            IPPicFr.ILTexture = null;
            var sphereImage = SphereImage(FPlace.GetMagicSphere());
            Sphere.SetILUIndex(sphereImage);
            Sphere.Visible = true;
        }
        else
        {
            if (FPlace is Shipyard)
            {
                IncomeHdr.Items.Clear();
                IncomePicture.ILIndexTexture = 68;
            }
            else
            {
                IncomePicture.ILIndexTexture = 65;
            }

            IPPicFr.ILTexture = DialogHosts.IntGfxMod.CityWin;
            if (FPlace is Shipyard)
            {
                IPPicFr.ILIndexTexture = 67;
            }
            else
            {
                IPPicFr.ILIndexTexture = 66;
            }

            Sphere.Visible = false;
        }

        IncomeMemo.Items.Clear();
        IncomeValuesMemo.Items.Clear();
        FPlace.ListIncomeInfo(IncomeMemo.Items, IncomeValuesMemo.Items);
        QueueLB.Items.Clear();
        for (int I = 0, last = FPlace.FProductionControl.FQueue.GetCount() - 1; I <= last; I++)
        {
            var name2 = FPlace.FProductionControl.FQueue.GetItem(I).GetName();
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + name2);
        }

        for (int I = FPlace.FProductionControl.FQueue.GetCount(); I <= 4; I++)
        {
            var emptyQueue = AbilityRules.TranslateRStr(GameText.EmptyQueueRStr);
            QueueLB.Items.Add(IntToStr(I + 1) + ". " + emptyQueue);
        }

        var count = FPlace.FProductionControl.FQueue.GetCount();
        if (QueueLB.Selected >= count)
        {
            var fQueueCount = FPlace.FProductionControl.FQueue.GetCount();
            QueueLB.Selected = fQueueCount - 1;
            FQueueIndex = QueueLB.Selected;
        }

        LoopQueueBox.Checked = FPlace.FProductionControl.FLoopQueue;
        if (FPlace.FProductionControl.FStatus == ProductionControl.Status.Producing)
        {
            var nameProgressive = FPlace.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (FPlace.FProductionControl.FSettings.FTurnsLeft != -1)
            {
                var fmtTurnsLeftStr = DurationText.FmtTurnsLeftStr(FPlace.FProductionControl.FSettings.FTurnsLeft);
                ActiveActionTurns.Text = fmtTurnsLeftStr;
                ActiveActionTurns.Visible = true;
                ActiveActionTurns.TextColor = 6783862;
                this.ActiveActionTurnsBar.Min = 0;
                this.ActiveActionTurnsBar.Max = FPlace.FProductionControl.FSettings.FTurnsTotal;
                this.ActiveActionTurnsBar.Position = FPlace.FProductionControl.FSettings.FTurnsTotal - FPlace.FProductionControl.FSettings.FTurnsLeft;

                ActiveActionTurnsBar.Visible = true;
            }
            else
            {
                ActiveActionTurns.Visible = false;
                ActiveActionTurnsBar.Visible = false;
            }

            var activityImage = FPlace.FProductionControl.FQueue.GetItem(0).Order.ActivityImage(FPlace);
            CurrentActionImage.CurrentImage = activityImage;
        }
        else
        {
            if (FPlace.FProductionControl.FStatus == ProductionControl.Status.Idle)
            {
                var currentActivityText = FPlace.GetCurrentActivityText();
                ActiveAction.Text = currentActivityText;
                var defaultText = AbilityRules.TranslateRStr(GameText.DefaultRStr);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                var none = AbilityRules.TranslateRStr(GameTables.NoneRStr);
                if (CompareText(ActiveAction.Text, none) != 0)
                {
                    ActiveActionTurns.Visible = true;
                    var activityImage = new Trade(0).ActivityImage(FPlace);
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
                if (FPlace.FProductionControl.FStatus == ProductionControl.Status.Waiting)
                {
                    Reason = "";
                    var nameProgressive = FPlace.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
                    ActiveAction.Text = nameProgressive;
                    var queue = FPlace.FProductionControl.FQueue.GetItem(0);
                    FPlace.FProductionControl.CanExecuteProduction(queue, ref Reason);
                    ActiveActionTurns.Text = Reason;
                    ActiveActionTurns.TextColor = 255;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurnsBar.Visible = false;
                    var activityImage = FPlace.FProductionControl.FQueue.GetItem(0).Order.ActivityImage(FPlace);
                    CurrentActionImage.CurrentImage = activityImage;
                }
            }
        }

        switch (FMode)
        {
            case 1:
                if (FProductionIndex != -1)
                {
                    SelectProduction(FProductionIndex);
                }

                break;
            case 2:
                QueueLBChange();
                break;
        }

        Dlg.FDrawn = false;
    }

    // AoWProdScreen.pas:180
    public void ShowPlace(ProductionPlace Place)
    {
        ArmyHS Army;
        RndTacHsm Generator;
        HexagonSprite Center;
        Reset();
        FPlace = Place;
        UpdateInfo();
        SetMode(0);
        var text = Place.GetDescription().Text;
        PlaceInfoMemo.Items.Text = text;
        for (int I = 0, last = FPlace.FProductionControl.FUnitProductions.GetCount() - 1; I <= last; I++)
        {
            LoadUnitImages(FPlace.FProductionControl.FUnitProductions.GetItem(I).FUnitIndex);
        }

        FreePreview();
        Generator = new RndTacHsm();
        var lhx = FPlace.GetLhx();
        var yhx = FPlace.GetYhx();
        FPreview = Generator.RandomMap(null, FPlace.GetXhx(), yhx, lhx);
        Generator?.Dispose();
        Army = ((AoWMapField)FPlace.Owner).GetArmyHS();
        if (Army != null)
        {
            for (int I = 0, last = Army.FArmy.GetCount() - 1; I <= last; I++)
            {
                var unit = Army.FArmy.GetUnit(I);
                AoWCombatMap.PlaceOverviewUnit(FPreview, unit, 0, I);
            }
        }

        var baseIncome = FPlace.GetBaseIncome();
        B5CB.CaptureInput = baseIncome != 0;
        Center = FPreview.FCenterViewLocation;
        if (Center != null)
        {
            var fCenterViewLocationYhx = FPreview.FCenterViewLocation.GetYhx();
            HexGeometry.HXtoHP(Center.GetXhx(), fCenterViewLocationYhx, ref FViewX, ref FViewY);
            FViewX = FViewX + 16 - MapView.WinWidth / 2;
            FViewY = FViewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            FViewX = 800;
            FViewY = 800;
        }

        DialogHosts.General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWProdScreen.pas:221
    public void ProductionScreenCreate()
    {
        FImages = new IntegerList();
    }

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy()
    {
        FImages?.Dispose();
    }

    // AoWProdScreen.pas:231
    public void ActivityChange(AOWCheckBox Sender, int Slot)
    {
        DialogHosts.General.PlayButtonSound(1);
        if (Sender.Checked)
        {
            ResetChecks();
            SelectProduction(Slot);
            Sender.Checked = true;
        }
    }

    // AoWProdScreen.pas:242
    public void ActivityDoubleClick(AOWCheckBox Sender, int Slot)
    {
        ResetChecks();
        SelectProduction(Slot);
        Sender.Checked = true;
        MainBtn1Click();
    }

    // AoWProdScreen.pas:250
    public void ActivityPreDraw(AOWCheckBox Sender, SurfaceDesc SD, int Slot)
    {
        P2UnitShowInfo Info = default;
        Rect R;
        Unit U;
        R = new Rect(ActivityPnl2.WinLeft + Sender.WinLeft, ActivityPnl2.WinTop + Sender.WinTop, ActivityPnl2.WinLeft + Sender.WinLeft + Sender.WinWidth, ActivityPnl2.WinTop + Sender.WinTop + Sender.WinHeight);
        switch (Slot)
        {
            case >= 0 and <= 3:
                U = FPlace.FProductionControl.CreateUnitProduction(Slot);
                if (U != null)
                {
                    Info.Direction = 3;
                    Info.Overlays = Set<UnitOverlayModesElement>.Of();
                    Info.Animation = 0;
                    Info.Frame = 0;
                    Info.Looped = false;
                    if (!FPlace.FProductionControl.FUnitProductions.GetItem(Slot).FResearched)
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

                    U.Release();
                }
                else
                {
                    DialogHosts.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 4:
                if (FPlace.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(FPlace);
                    DialogHosts.IntGfxMod.CityWin[activityImage].Show(R.Left + 2, R.Top + 2, SD);
                }
                else
                {
                    DialogHosts.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 5:
                DialogHosts.IntGfxMod.CityWin[46].Show(R.Left + 2, R.Top + 2, SD);
                break;
        }
    }

    // AoWProdScreen.pas:299
    public void MainBtn1Click()
    {
        string Reason = "";
        switch (FMode)
        {
            case 0:
                SetMode(1);
                SelectDefaultAction();
                break;
            case 1:
                switch (FProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                        {
                            if (FPlace.CanProduce(FProductionIndex, false, ref Reason))
                            {
                                FPlace.Produce(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(Reason);
                            }
                        }
                        else
                        {
                            if (FPlace.CanResearch(FProductionIndex, false, ref Reason))
                            {
                                FPlace.Research(FProductionIndex, false);
                                SetMode(0);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(Reason);
                            }
                        }

                        break;
                    case 4:
                        if (FPlace.CanTrade(false, ref Reason))
                        {
                            FPlace.Trade(false);
                            SetMode(0);
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                    case 5:
                        if (FPlace.CanRaze(AoWHSMap.Current.FSeatedPlayer, ref Reason))
                        {
                            FPlace.Raze(AoWHSMap.Current.FSeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.Current.ShowWarning(Reason);
                        }

                        break;
                }

                break;
            case 2:
                if (QueueLB.Selected != -1)
                {
                    FPlace.FProductionControl.DeleteProduction(QueueLB.Selected);
                }

                break;
        }
    }

    // AoWProdScreen.pas:347
    public void MainBtn2Click()
    {
        string Reason = "";
        switch (FMode)
        {
            case 1:
                switch (FProductionIndex)
                {
                    case >= 0 and <= 3:
                        if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                        {
                            if (FPlace.CanProduce(FProductionIndex, true, ref Reason))
                            {
                                FPlace.Produce(FProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(Reason);
                            }
                        }
                        else
                        {
                            if (FPlace.CanResearch(FProductionIndex, true, ref Reason))
                            {
                                FPlace.Research(FProductionIndex, true);
                            }
                            else
                            {
                                AoWHSMap.Current.ShowWarning(Reason);
                            }
                        }

                        break;
                    case 4:
                        if (FPlace.CanTrade(true, ref Reason))
                        {
                            FPlace.Trade(true);
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
                    if (FPlace.FProductionControl.RetryProductionExecution(ref Reason))
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

    // AoWProdScreen.pas:383
    public void MainBtn3Click()
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
    }

    // AoWProdScreen.pas:389
    public void UFaceDraw(TaowUserDrawBox Sender, SurfaceDesc SD)
    {
        Unit U;
        Rect R;
        U = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
        if (U != null)
        {
            R = new Rect(UPictureFramePnl.WinLeft + Sender.WinLeft + 2, UPictureFramePnl.WinTop + Sender.WinTop + 2, UPictureFramePnl.WinLeft + Sender.WinLeft + Sender.WinWidth - 4, UPictureFramePnl.WinTop + Sender.WinTop + Sender.WinHeight - 2);
            U.ShowFace(R, SD);
            U.Release();
        }
    }

    // AoWProdScreen.pas:406
    public void QueueLBChange()
    {
        var count = FPlace.FProductionControl.FQueue.GetCount();
        if (QueueLB.Selected < count && FMode != 0)
        {
            if (QueueLB.Selected != -1)
            {
                FQueueIndex = QueueLB.Selected;
                if (FPlace.FProductionControl.FQueue.GetItem(FQueueIndex).Order is ProduceUnit produce)
                {
                    SelectProduction(produce.Index);
                }
                else
                {
                    SelectProduction(4);
                }

                SetMode(2);
                QueueLB.Selected = FQueueIndex;
                MainBtn1.Visible = true;
                var delete = AbilityRules.TranslateRStr(GameText.DeleteRStr);
                MainBtn1.Text = delete;
                if (FQueueIndex == 0 && FPlace.FProductionControl.FStatus == ProductionControl.Status.Waiting)
                {
                    MainBtn2.Visible = true;
                    var execute = AbilityRules.TranslateRStr(GameText.ExecuteRStr);
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
            if (FMode == 2)
            {
                QueueLB.Selected = FQueueIndex;
            }
            else
            {
                QueueLB.Selected = -1;
            }
        }
    }

    // AoWProdScreen.pas:439
    public void B4CBUpdate(AOWCheckBox Sender, int Slot)
    {
        if (Slot <= 3)
        {
            var unitProduction = FPlace.FProductionControl.FUnitProductions.GetItem(Slot);
            Sender.CaptureInput = unitProduction != null;
        }
    }

    // AoWProdScreen.pas:445
    public void LoopQueueBoxChange()
    {
        FPlace.FProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
    }

    // AoWProdScreen.pas:450
    public void CurrentActionImageDraw(AOWImage Sender, SurfaceDesc SD)
    {
        P2UnitShowInfo Info = default;
        Rect R;
        Unit U;
        if (FPlace.FProductionControl.FQueue.GetCount() != 0)
        {
            if (FPlace.FProductionControl.FQueue.GetItem(0).Order.UnitProductionIndex != null)
            {
                R = new Rect(ActiveActionPnl.WinLeft + Sender.WinLeft, ActiveActionPnl.WinTop + Sender.WinTop, ActiveActionPnl.WinLeft + Sender.WinLeft + Sender.WinWidth, ActiveActionPnl.WinTop + Sender.WinTop + Sender.WinHeight);
                var productionPlaceControl = FPlace.FProductionControl;
                if (productionPlaceControl.FUnitProductions.GetItem(productionPlaceControl.FSettings.FUnitProductionIndex) != null)
                {
                    U = FPlace.FProductionControl.CreateUnitProduction(FPlace.FProductionControl.FSettings.FUnitProductionIndex);
                    if (U != null)
                    {
                        Info.Direction = 3;
                        Info.Overlays = Set<UnitOverlayModesElement>.Of();
                        Info.Animation = 0;
                        Info.Frame = 0;
                        Info.Looped = false;
                        U.ShowEx(R.Left - 12, R.Top - 5, SD, Info);
                        U.Release();
                    }
                }
            }
        }
    }

    // AoWProdScreen.pas:483
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

    // AoWProdScreen.pas:510
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

    // AoWProdScreen.pas:521
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

    // AoWProdScreen.pas:530
    public void DlgHide()
    {
        ClosePlace();
    }

    // AoWProdScreen.pas:535
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

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int Start;
        int Index;
        PlayerStructure S;
        Start = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start;
        Index -= 1;
        if (Index < 0)
        {
            Index = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1;
        }

        while (Index != Start)
        {
            S = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                Index = Start + 1;
                ShowPlace((ProductionPlace)S);
            }

            Index += -1;
            if (Index < 0)
            {
                Index = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount() - 1;
            }
        }
    }

    // AoWProdScreen.pas:568
    public void PNextClick()
    {
        int Start;
        int Index;
        PlayerStructure S;
        Start = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start + 1;
        if (Index >= AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount())
        {
            Index = 0;
        }

        while (Index != Start)
        {
            S = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is ProductionPlace && ((ProductionPlace)S).GetMagicSphere() == MagicSphere.None)
            {
                Index = Start - 1;
                ShowPlace((ProductionPlace)S);
            }

            Index += 1;
            if (Index >= AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FStructures.GetCount())
            {
                Index = 0;
            }
        }
    }

    private static readonly string GenerateExtraPowerTextRStr = "Increases the power generated by the node by 25%.";
    private static readonly string ProduceMerchandiseTextRStr = "When producing merchandise the income is increased by 25%.";
    private static readonly string ProductionPlaceXRazeRStr = "Destroys the %s instantly by burning it to the ground.";
    private static readonly string XPowerRStr = "%s power";

    private static int SphereImage(MagicSphere Sphere)
    {
        return Sphere switch
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
