// Translated from AoWProdScreen.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AOWGauge;
using Aow.Units.AoWImage;
using Aow.Units.AoWInfoBanner;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTC;
using Aow.Units.aowUserDrawBox;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using Aow.Units.HSEngine;
using Aow.Units.PrdPlace;
using Aow.Units.Shipyard;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.HSEngine.HSEngineUnit;
using static Aow.Units.OverviewUnit.OverviewUnitUnit;

namespace Aow.Units.AoWProdScreen;

// view, place, production, queue, mode, preview, images
public partial class TProductionScreen : TDataModule
{
    public TProductionScreen(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWPanel UnitPnl;
    public TAOWListBox UAbilitiesLB;
    public TAOWImage UAttackIcon;
    public TAOWImage UDefenseIcon;
    public TAOWImage UMovesIcon;
    public TAOWImage UDamageIcon;
    public TAOWImage UResistanceIcon;
    public TAOWImage UHitsIcon;
    public TAOWLabel UAttack;
    public TAOWLabel UDefense;
    public TAOWLabel UMoves;
    public TAOWLabel UDamage;
    public TAOWLabel UResistance;
    public TAOWLabel UHits;
    public TAOWPanel UStatPnl;
    public TAOWPanel UStatP1;
    public TAOWPanel UStatP2;
    public TAOWLabel UAlignment;
    public TAOWLabel ULevel;
    public TAOWPanel UPicturePnl;
    public TAOWPanel UCostPnl;
    public TAOWLabel StructName;
    public TAOWLabel CostLbl;
    public TAOWLabel UCost;
    public TAOWLabel UnitLbl;
    public TAOWLabel TurnsLbl;
    public TAOWLabel UTurns;
    public TAOWCheckBox B4CB;
    public TAOWCheckBox B5CB;
    public TAOWCheckBox B6CB;
    public TAOWCheckBox B1CB;
    public TAOWCheckBox B2CB;
    public TAOWCheckBox B3CB;
    public TAOWVScrollBar T2SBar;
    public TAOWPanel AbilPnl;
    public TAOWPanel UpgradePnl;
    public TAOWLabel UpgradeLbl;
    public TAOWPanel UpgradeMemoFr;
    public TAOWMemo UpgradeMemo;
    public TAOWPanel RightPnl;
    public TAOWPanel QueuePnl;
    public TAOWImage Oval;
    public TAOWImage ShadowL;
    public TAOWImage ShadowR;
    public TAOWButton MainBtn3;
    public TAOWButton MainBtn1;
    public TAOWListBox QueueLB;
    public TAOWPanel ActivityPnl;
    public TAOWPanel InfoPnl2;
    public TAOWLabel ActiveAction;
    public TAOWLabel ActiveActionTurns;
    public TAOWImage SepBar;
    public TAOWPanel ActivityPnl2;
    public TAoWGauge ActiveActionTurnsBar;
    public TAOWButton MainBtn2;
    public TAOWPanel InfoPnl;
    public TAOWPanel IPPnl;
    public TAoWFrame IPPicFr;
    public TAOWMemo PlaceInfoMemo;
    public TAOWPanel IIPnl;
    public TAoWFrame IncomePicture;
    public TAOWMemo IncomeMemo;
    public TAOWMemo IncomeHdr;
    public TAOWPanel CurrentInfo;
    public TAoWFrame RPillar;
    public TAoWFrame LPillar;
    public TAOWPanel QueueInfo;
    public TAOWLabel QueueLbl;
    public TAOWCheckBox LoopQueueBox;
    public TAOWLabel RepeatLbl;
    public TAOWPanel ActiveActionPnl;
    public TAOWImage CurrentActionImage;
    public TAoWFrame AoWFrame1;
    public TAOWImage BlackBg2;
    public TAOWPanel UPictureFramePnl;
    public TAOWPanel UStatFr1;
    public TAOWPanel UStatFr2;
    public TAOWPanel UInfoFr;
    public TAOWPanel CostFr;
    public TAOWPanel TurnsFr;
    public TAOWPanel UpgPicPnl;
    public TAOWImage UpgradePic;
    public TAOWPanel UpgPicFr;
    public TAOWPanel UpgCostPnl;
    public TAOWLabel UpgCostLbl;
    public TAOWLabel UpgradeCost;
    public TAOWLabel UpgTurnsLbl;
    public TAOWLabel UpgradeTurns;
    public TAOWPanel UpgCostFr;
    public TAOWPanel UpgTurnsFr;
    public TAOWImage RelIcn;
    public TAOWVScrollBar QueueVSB;
    public TAOWVScrollBar IPVSB;
    public TAOWVScrollBar IIVSB;
    public TAOWMemo IncomeValuesMemo;
    public TaowUserDrawBox UFace;
    public TAOWImage InfoSep;
    public TAOWButton Sphere;
    public TaowUserDrawBox MapView;
    public TAOWButton PNext;
    public TAOWButton PPrev;
    public TIvTranslator Translator;
    public int FViewX;
    public int FViewY;
    public TProductionPlace FPlace;
    public int FProductionIndex;
    public int FQueueIndex;
    public byte FMode;
    public TAoWCombatMap FPreview;
    public TIntegerList FImages;

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
            AoWHSSet.FUnitGFXResources.GetUnitGFXResource(image).Release();
        }

        FImages.Clear();
    }

    // AoWProdScreen.pas:608
    public void LoadUnitImages(int Index)
    {
        int Gfx;
        Gfx = AoWHSSet.FUnitResources.GetUnitResource(Index).FUnitGFXResourceIndex;
        AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx).AddRef();
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
        var options = TranslateRStr(OptionsRStr);
        MainBtn1.Text = options;
        MainBtn2.Visible = false;
        PPrev.Visible = !MainBtn2.Visible;
        PNext.Visible = !MainBtn2.Visible;
        ShadowL.Visible = AoWEngine.FAnimationDetail != 0;
        ShadowR.Visible = AoWEngine.FAnimationDetail != 0;
        Oval.Visible = AoWEngine.FAnimationDetail != 0;
        FQueueIndex = -1;
        QueueLB.Selected = -1;
        var queue = TranslateRStr(QueueRStr);
        QueueLbl.Text = queue;
        var repeat = TranslateRStr(RepeatRStr);
        RepeatLbl.Text = repeat;
        FreePreview();
    }

    // AoWProdScreen.pas:652
    public void ClosePlace()
    {
        Reset();
        FPlace = null;
        General.Manager.OverlayPriority = 0;
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
                var change = TranslateRStr(ChangeRStr);
                MainBtn1.Text = change;
                MainBtn2.Visible = false;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                var exit = TranslateRStr(ExitRStr);
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
                var info = TranslateRStr(InfoRStr);
                MainBtn1.Text = info;
                var done = TranslateRStr(DoneRStr);
                MainBtn3.Text = done;
                break;
            case 2:
                ResetChecks();
                QueueLB.Selected = -1;
                InfoPnl.Visible = false;
                InfoPnl2.Visible = false;
                ActivityPnl.Visible = true;
                ActivityPnl2.Visible = true;
                var infoText = TranslateRStr(InfoRStr);
                MainBtn1.Text = infoText;
                var doneText = TranslateRStr(DoneRStr);
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
    public void ProductionEvent(TProductionPlaceEventLog Event)
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
            if (FPlace.FPlayer == AoWHSMap.FSeatedPlayer)
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
        TUnit U;
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
                    var mTtoInt = General.MTtoInt(moveTypes);
                    UMovesIcon.CurrentImage = mTtoInt;
                    var alignment = U.GetAlignment();
                    var alignmentText = TranslateRStr(TAlignmentRStr[(int)alignment]);
                    var format = AoWEUnit.Format(TranslateRStr(AlignmentXRStr), [alignmentText]);
                    UAlignment.Text = format;
                    var unitLevel = U.GetUnitLevel();
                    var aoWEFormat = AoWEUnit.Format(TranslateRStr(LevelXRStr), [IntToStr(unitLevel)]);
                    var upkeep = U.GetUpkeep();
                    var format2 = AoWEUnit.Format(TranslateRStr(UpkeepXRStr), [(int)upkeep]);
                    ULevel.Text = aoWEFormat + " / " + format2;
                    UAbilitiesLB.Items.Clear();
                    U.ListAbilities(UAbilitiesLB.Items);
                    if (FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex).FResearched)
                    {
                        MainBtn1.Visible = true;
                        var produce = TranslateRStr(ProduceRStr);
                        MainBtn1.Text = produce;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = TranslateRStr(QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AoWEUnit.Format(TranslateRStr(ProduceXRStr), [name]);
                        UnitLbl.Text = format3;
                        var unitProductionGold = FPlace.FProductionControl.UnitProductionGold(FProductionIndex);
                        UCost.Text = IntToStr(unitProductionGold);
                        var unitProductionTime = FPlace.FProductionControl.UnitProductionTime(FProductionIndex);
                        UTurns.Text = IntToStr(unitProductionTime);
                    }
                    else
                    {
                        MainBtn1.Visible = true;
                        var install = TranslateRStr(InstallRStr);
                        MainBtn1.Text = install;
                        var queueLocked = FPlace.FProductionControl.GetQueueLocked();
                        MainBtn2.Visible = !queueLocked;
                        var queue = TranslateRStr(QueueRStr);
                        MainBtn2.Text = queue;
                        var name = U.GetName();
                        var format3 = AoWEUnit.Format(TranslateRStr(InstallXRStr), [name]);
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
                var queueText = TranslateRStr(QueueRStr);
                MainBtn2.Text = queueText;
                PPrev.Visible = !MainBtn2.Visible;
                PNext.Visible = !MainBtn2.Visible;
                if (FPlace.GetMagicSphere() == TMagicSphere.msNone)
                {
                    var produceMerchandiseText = TranslateRStr(ProduceMerchandiseTextRStr);
                    UpgradeMemo.Items.Text = produceMerchandiseText;
                }
                else
                {
                    var generateExtraPowerText = TranslateRStr(GenerateExtraPowerTextRStr);
                    UpgradeMemo.Items.Text = generateExtraPowerText;
                }

                UpgradePnl.Visible = true;
                if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
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
                var raze = TranslateRStr(RazeRStr);
                MainBtn1.Text = raze + "...";
                PPrev.Visible = false;
                PNext.Visible = false;
                var fPlaceName = FPlace.GetName();
                var format4 = AoWEUnit.Format(TranslateRStr(ProductionPlaceXRazeRStr), [fPlaceName]);
                UpgradeMemo.Items.Text = format4;
                UpgradePnl.Visible = true;
                UpgradePic.CurrentImage = 26;
                var razeText = TranslateRStr(RazeRStr);
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
        var income = TranslateRStr(IncomeRStr);
        IncomeHdr.Items.Add(income);
        if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
        {
            var power = FPlace.GetPower();
            var xPower = TranslateRStr(XPowerRStr);
            var format = AoWEUnit.Format(" " + xPower, [IntToStr(power)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetIncome() != 0)
        {
            var fPlaceIncome = FPlace.GetIncome();
            var xGold = TranslateRStr(XGoldRStr);
            var format = AoWEUnit.Format(" " + xGold, [IntToStr(fPlaceIncome)]);
            IncomeHdr.Items.Add(format);
        }

        if (FPlace.GetMagicSphere() != TMagicSphere.msNone)
        {
            IncomePicture.ILIndexTexture = 64;
            IPPicFr.ILTexture = null;
            var sphereImage = AoWProdScreenUnit.SphereImage(FPlace.GetMagicSphere());
            Sphere.SetILUIndex(sphereImage);
            Sphere.Visible = true;
        }
        else
        {
            if (FPlace is TShipyard)
            {
                IncomeHdr.Items.Clear();
                IncomePicture.ILIndexTexture = 68;
            }
            else
            {
                IncomePicture.ILIndexTexture = 65;
            }

            IPPicFr.ILTexture = aowIntGfxModUnit.IntGfxMod.CityWin;
            if (FPlace is TShipyard)
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
            var emptyQueue = TranslateRStr(EmptyQueueRStr);
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
        if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
        {
            var nameProgressive = FPlace.FProductionControl.FQueue.GetItem(0).GetNameProgressive();
            ActiveAction.Text = nameProgressive;
            if (FPlace.FProductionControl.FSettings.FTurnsLeft != -1)
            {
                var fmtTurnsLeftStr = FmtTurnsLeftStr(FPlace.FProductionControl.FSettings.FTurnsLeft);
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
            if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsIdle)
            {
                var currentActivityText = FPlace.GetCurrentActivityText();
                ActiveAction.Text = currentActivityText;
                var defaultText = TranslateRStr(DefaultRStr);
                ActiveActionTurns.Text = "(" + LowerCase(defaultText) + ")";
                ActiveActionTurns.TextColor = 6783862;
                var none = TranslateRStr(NoneRStr);
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
                if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
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
    public void ShowPlace(TProductionPlace Place)
    {
        TArmyHS Army;
        TRndTacHsm Generator;
        THexagonSprite Center;
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
        Generator = new TRndTacHsm();
        var lhx = FPlace.GetLhx();
        var yhx = FPlace.GetYhx();
        FPreview = Generator.RandomMap(null, FPlace.GetXhx(), yhx, lhx);
        Generator?.Dispose();
        Army = ((TAoWMapField)FPlace.Owner).GetArmyHS();
        if (Army != null)
        {
            for (int I = 0, last = Army.FArmy.GetCount() - 1; I <= last; I++)
            {
                var unit = Army.FArmy.GetUnit(I);
                PlaceOverviewUnit(FPreview, unit, 0, I);
            }
        }

        var baseIncome = FPlace.GetBaseIncome();
        B5CB.CaptureInput = baseIncome != 0;
        Center = FPreview.FCenterViewLocation;
        if (Center != null)
        {
            var fCenterViewLocationYhx = FPreview.FCenterViewLocation.GetYhx();
            HXtoHP(Center.GetXhx(), fCenterViewLocationYhx, ref FViewX, ref FViewY);
            FViewX = FViewX + 16 - MapView.WinWidth / 2;
            FViewY = FViewY + 16 - MapView.WinHeight / 2;
        }
        else
        {
            FViewX = 800;
            FViewY = 800;
        }

        General.Manager.OverlayPriority = 49;
        Dlg.Visible = true;
    }

    // AoWProdScreen.pas:221
    public void ProductionScreenCreate()
    {
        FImages = new TIntegerList();
    }

    // AoWProdScreen.pas:226
    public void ProductionScreenDestroy()
    {
        FImages?.Dispose();
    }

    // AoWProdScreen.pas:231
    public void ActivityChange(TAOWCheckBox Sender, int Slot)
    {
        General.PlayButtonSound(1);
        if (Sender.Checked)
        {
            ResetChecks();
            SelectProduction(Slot);
            Sender.Checked = true;
        }
    }

    // AoWProdScreen.pas:242
    public void ActivityDoubleClick(TAOWCheckBox Sender, int Slot)
    {
        ResetChecks();
        SelectProduction(Slot);
        Sender.Checked = true;
        MainBtn1Click();
    }

    // AoWProdScreen.pas:250
    public void ActivityPreDraw(TAOWCheckBox Sender, TSurfaceDesc SD, int Slot)
    {
        TP2UnitShowInfo Info = default;
        TRect R;
        TUnit U;
        R = Rect(ActivityPnl2.WinLeft + Sender.WinLeft, ActivityPnl2.WinTop + Sender.WinTop, ActivityPnl2.WinLeft + Sender.WinLeft + Sender.WinWidth, ActivityPnl2.WinTop + Sender.WinTop + Sender.WinHeight);
        switch (Slot)
        {
            case >= 0 and <= 3:
                U = FPlace.FProductionControl.CreateUnitProduction(Slot);
                if (U != null)
                {
                    Info.Direction = 3;
                    Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                    Info.Animation = 0;
                    Info.Frame = 0;
                    Info.Looped = false;
                    if (!FPlace.FProductionControl.FUnitProductions.GetItem(Slot).FResearched)
                    {
                        aowIntGfxModUnit.IntGfxMod.CityWin[5].Show(R.Left + 2, R.Top + 2, SD);
                        Info.Direction = 5;
                        if (U != null)
                        {
                            U.ShowEx(R.Left - 4, R.Top, SD, Info);
                        }

                        var lookupTable = AlphaBlendTable.GetLookupTables(45);
                        aowIntGfxModUnit.IntGfxMod.CityWin[5].ShowBlended(R.Left + 2, R.Top + 2, SD, lookupTable);
                    }
                    else
                    {
                        aowIntGfxModUnit.IntGfxMod.CityWin[4].Show(R.Left + 2, R.Top + 2, SD);
                        if (U != null)
                        {
                            U.ShowEx(R.Left - 4, R.Top, SD, Info);
                        }
                    }

                    U.Release();
                }
                else
                {
                    aowIntGfxModUnit.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 4:
                if (FPlace.GetBaseIncome() != 0)
                {
                    var activityImage = new Trade(0).ActivityImage(FPlace);
                    aowIntGfxModUnit.IntGfxMod.CityWin[activityImage].Show(R.Left + 2, R.Top + 2, SD);
                }
                else
                {
                    aowIntGfxModUnit.IntGfxMod.CityWin[62].Show(R.Left + 2, R.Top + 2, SD);
                }

                break;
            case 5:
                aowIntGfxModUnit.IntGfxMod.CityWin[46].Show(R.Left + 2, R.Top + 2, SD);
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
                                AoWHSMap.ShowWarning(Reason);
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
                                AoWHSMap.ShowWarning(Reason);
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
                            AoWHSMap.ShowWarning(Reason);
                        }

                        break;
                    case 5:
                        if (FPlace.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason))
                        {
                            FPlace.Raze(AoWHSMap.FSeatedPlayer);
                            Dlg.Visible = false;
                        }
                        else
                        {
                            AoWHSMap.ShowWarning(Reason);
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
                                AoWHSMap.ShowWarning(Reason);
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
                                AoWHSMap.ShowWarning(Reason);
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
                            AoWHSMap.ShowWarning(Reason);
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
                        AoWHSMap.ShowWarning(Reason);
                    }
                }

                break;
        }
    }

    // AoWProdScreen.pas:383
    public void MainBtn3Click()
    {
        General.PlayButtonSound(0);
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
    public void UFaceDraw(TaowUserDrawBox Sender, TSurfaceDesc SD)
    {
        TUnit U;
        TRect R;
        U = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
        if (U != null)
        {
            R = Rect(UPictureFramePnl.WinLeft + Sender.WinLeft + 2, UPictureFramePnl.WinTop + Sender.WinTop + 2, UPictureFramePnl.WinLeft + Sender.WinLeft + Sender.WinWidth - 4, UPictureFramePnl.WinTop + Sender.WinTop + Sender.WinHeight - 2);
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
                var delete = TranslateRStr(DeleteRStr);
                MainBtn1.Text = delete;
                if (FQueueIndex == 0 && FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
                {
                    MainBtn2.Visible = true;
                    var execute = TranslateRStr(ExecuteRStr);
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
    public void B4CBUpdate(TAOWCheckBox Sender, int Slot)
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
    public void CurrentActionImageDraw(TAOWImage Sender, TSurfaceDesc SD)
    {
        TP2UnitShowInfo Info = default;
        TRect R;
        TUnit U;
        if (FPlace.FProductionControl.FQueue.GetCount() != 0)
        {
            if (FPlace.FProductionControl.FQueue.GetItem(0).Order.UnitProductionIndex != null)
            {
                R = Rect(ActiveActionPnl.WinLeft + Sender.WinLeft, ActiveActionPnl.WinTop + Sender.WinTop, ActiveActionPnl.WinLeft + Sender.WinLeft + Sender.WinWidth, ActiveActionPnl.WinTop + Sender.WinTop + Sender.WinHeight);
                var productionPlaceControl = FPlace.FProductionControl;
                if (productionPlaceControl.FUnitProductions.GetItem(productionPlaceControl.FSettings.FUnitProductionIndex) != null)
                {
                    U = FPlace.FProductionControl.CreateUnitProduction(FPlace.FProductionControl.FSettings.FUnitProductionIndex);
                    if (U != null)
                    {
                        Info.Direction = 3;
                        Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
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
    public void MapViewDraw(TaowUserDrawBox Sender, TSurfaceDesc SD)
    {
        TSurfaceDesc ViewSD = default;
        TRect R;
        int W;
        int H;
        int Width;
        if (FPreview != null)
        {
            R = Rect(Dlg.WinLeft + Sender.WinLeft, Dlg.WinTop + Sender.WinTop, Dlg.WinLeft + Sender.WinLeft + Sender.WinWidth, Dlg.WinTop + Sender.WinTop + Sender.WinHeight);
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

            CreateSDfromSD(SD, R, ref ViewSD);
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
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
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
        TAoWCombatMap Preview;
        Preview = FPreview;
        if (Preview != null)
        {
            Preview.NewFrame();
        }

        MapView.FDrawn = MapView.FDrawn && (AoWHSMap.Frame & 1) != 0;
    }

    // AoWProdScreen.pas:530
    public void DlgHide()
    {
        ClosePlace();
    }

    // AoWProdScreen.pas:535
    public void UAbilitiesLBMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TAbility A;
        if (Button == TMouseButton.mbRight && UAbilitiesLB.Selected > -1)
        {
            var objectValue = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
            A = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(objectValue));
            if (A != null)
            {
                var name = A.GetName(null);
                SetInfoBanner(AoWInfoBannerUnit.InfoBanner, name, A.FDescription);
            }
        }
    }

    // AoWProdScreen.pas:546
    public void PPrevClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start;
        Index -= 1;
        if (Index < 0)
        {
            Index = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
        }

        while (Index != Start)
        {
            S = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
            {
                Index = Start + 1;
                ShowPlace((TProductionPlace)S);
            }

            Index += -1;
            if (Index < 0)
            {
                Index = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount() - 1;
            }
        }
    }

    // AoWProdScreen.pas:568
    public void PNextClick()
    {
        int Start;
        int Index;
        TPlayerStructure S;
        Start = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.FindChildIndex(FPlace);
        Index = Start + 1;
        if (Index >= AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount())
        {
            Index = 0;
        }

        while (Index != Start)
        {
            S = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetStructure(Index);
            if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
            {
                Index = Start - 1;
                ShowPlace((TProductionPlace)S);
            }

            Index += 1;
            if (Index >= AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer).FStructures.GetCount())
            {
                Index = 0;
            }
        }
    }
}

public static partial class AoWProdScreenUnit
{
    public static TProductionScreen ProductionScreen;

    // AoWProdScreen.pas:819
    public static int SphereImage(TMagicSphere Sphere)
    {
        int Result;
        switch (Sphere)
        {
            case (TMagicSphere)1:
                Result = 27;
                break;
            case (TMagicSphere)5:
                Result = 42;
                break;
            case (TMagicSphere)3:
                Result = 33;
                break;
            case (TMagicSphere)4:
                Result = 36;
                break;
            case (TMagicSphere)6:
                Result = 39;
                break;
            case (TMagicSphere)2:
                Result = 30;
                break;
            default:
                Result = -1;
                break;
        }

        return Result;
    }
}
