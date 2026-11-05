// Translated from AoWProdScreen.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using Aow.Units.AoWComp;
using Aow.Units.AOWGauge;
using Aow.Units.AoWButton;
using Aow.Units.AoWCheckBox;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.OverviewUnit.OverviewUnit;
using Aow.Units.Shipyard;

namespace Aow.Units.AoWProdScreen
{
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
                FPreview.Free();
                FPreview = null;
            }
        }

        // AoWProdScreen.pas:599
        public void ReleaseImages()
        {
            int I = default;
            {
                I = FImages.Count - 1;
                byte end__1 = 0;
                for (; I >= end__1; I -= 1)
                {
                    var call__2 = FImages[I];
                    var call__3 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__2);
                    call__3.Release();
                }
            }

            FImages.Clear();
        }

        // AoWProdScreen.pas:608
        public void LoadUnitImages(int Index)
        {
            int Gfx = default;
            var call__4 = AoWHSSet.FUnitResources.GetUnitResource(Index);
            Gfx = call__4.FUnitGFXResourceIndex;
            var call__5 = AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx);
            call__5.AddRef();
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
            var call__6 = TranslateRStr(OptionsRStr);
            MainBtn1.Text = call__6;
            MainBtn2.Visible = false;
            PPrev.Visible = !MainBtn2.Visible;
            PNext.Visible = !MainBtn2.Visible;
            ShadowL.Visible = AoWEngine.FAnimationDetail != 0;
            ShadowR.Visible = AoWEngine.FAnimationDetail != 0;
            Oval.Visible = AoWEngine.FAnimationDetail != 0;
            FQueueIndex = -1;
            QueueLB.Selected = -1;
            var call__7 = TranslateRStr(QueueRStr);
            QueueLbl.Text = call__7;
            var call__8 = TranslateRStr(RepeatRStr);
            RepeatLbl.Text = call__8;
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
                    var call__9 = TranslateRStr(ChangeRStr);
                    MainBtn1.Text = call__9;
                    MainBtn2.Visible = false;
                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    var call__10 = TranslateRStr(ExitRStr);
                    MainBtn3.Text = call__10;
                    QueueLB.Selected = -1;
                    break;
                case 1:
                    ResetChecks();
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    var call__11 = TranslateRStr(InfoRStr);
                    MainBtn1.Text = call__11;
                    var call__12 = TranslateRStr(DoneRStr);
                    MainBtn3.Text = call__12;
                    break;
                case 2:
                    ResetChecks();
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    var call__13 = TranslateRStr(InfoRStr);
                    MainBtn1.Text = call__13;
                    var call__14 = TranslateRStr(DoneRStr);
                    MainBtn3.Text = call__14;
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
            var call__15 = Event.GetProductionPlace();
            ShowPlace(call__15);
            SetMode(1);
            SelectDefaultAction();
        }

        // AoWProdScreen.pas:712
        public void PlaceChanged(TObject Sender)
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

        // AoWProdScreen.pas:719
        public int ActivityImage(TProductionItem Item)
        {
            int Result = default;
            switch (Item.FProductionID)
            {
                case 1:
                    Result = 4;
                    break;
                case 10:
                    var call__16 = FPlace.GetMagicSphere();
                    if (call__16 != TMagicSphere.msNone)
                    {
                        Result = 48;
                    }
                    else
                    {
                        Result = 47;
                    }

                    break;
                default:
                    Result = 4;
                    break;
            }

            return Result;
        }

        // AoWProdScreen.pas:729
        public void SelectProduction(int Index)
        {
            TUnit U = default;
            TProductionItem Item = default;
            if (FMode != 1)
            {
                SetMode(1);
            }

            UpgradePnl.Visible = false;
            UnitPnl.Visible = false;
            FProductionIndex = Index;
            UpgradePic.CurrentImage = 21;
            UpgradeCost.Text = AnsiString.FromBytes(45);
            UpgradeTurns.Text = AnsiString.FromBytes(45);
            MainBtn1.Visible = false;
            MainBtn2.Visible = false;
            PPrev.Visible = !MainBtn2.Visible;
            PNext.Visible = !MainBtn2.Visible;
            switch (FProductionIndex)
            {
                case >= 0 and <= 3:
                    var call__17 = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
                    U = call__17;
                    if (U != null)
                    {
                        UnitPnl.Visible = true;
                        var call__18 = U.GetAttack();
                        UAttack.Text = IntToStr(call__18);
                        var call__19 = U.GetDefense();
                        UDefense.Text = IntToStr(call__19);
                        var call__20 = U.GetResistance();
                        UResistance.Text = IntToStr(call__20);
                        var call__21 = U.GetDamage();
                        UDamage.Text = IntToStr(call__21);
                        var call__22 = U.GetHits();
                        UHits.Text = IntToStr(call__22);
                        var call__23 = U.GetMoves();
                        UMoves.Text = IntToStr(call__23);
                        var call__24 = U.GetMoveTypes();
                        var call__25 = General.MTtoInt(call__24);
                        UMovesIcon.CurrentImage = call__25;
                        var call__26 = U.GetAlignment();
                        var call__27 = TranslateRStr(TAlignmentRStr[(int)call__26]);
                        var call__28 = TranslateRStr(AlignmentXRStr);
                        var call__29 = AoWE.AoWE.Format(call__28, [call__27]);
                        UAlignment.Text = call__29;
                        var call__30 = U.GetUnitLevel();
                        var call__31 = TranslateRStr(LevelXRStr);
                        var call__32 = AoWE.AoWE.Format(call__31, [IntToStr(call__30)]);
                        var call__33 = U.GetUpkeep();
                        var call__34 = TranslateRStr(UpkeepXRStr);
                        var call__35 = AoWE.AoWE.Format(call__34, [(int)call__33]);
                        ULevel.Text = call__32 + (AnsiString)" / " + call__35;
                        UAbilitiesLB.Items.Clear();
                        U.ListAbilities(UAbilitiesLB.Items);
                        var call__36 = FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex);
                        if (call__36.FResearched)
                        {
                            MainBtn1.Visible = true;
                            var call__37 = TranslateRStr(ProduceRStr);
                            MainBtn1.Text = call__37;
                            var call__38 = FPlace.FProductionControl.GetQueueLocked();
                            MainBtn2.Visible = !call__38;
                            var call__39 = TranslateRStr(QueueRStr);
                            MainBtn2.Text = call__39;
                            var call__40 = U.GetName();
                            var call__41 = TranslateRStr(ProduceXRStr);
                            var call__42 = AoWE.AoWE.Format(call__41, [call__40]);
                            UnitLbl.Text = call__42;
                            var call__43 = FPlace.FProductionControl.UnitProductionGold(FProductionIndex);
                            UCost.Text = IntToStr(call__43);
                            var call__44 = FPlace.FProductionControl.UnitProductionTime(FProductionIndex);
                            UTurns.Text = IntToStr(call__44);
                        }
                        else
                        {
                            MainBtn1.Visible = true;
                            var call__45 = TranslateRStr(InstallRStr);
                            MainBtn1.Text = call__45;
                            var call__46 = FPlace.FProductionControl.GetQueueLocked();
                            MainBtn2.Visible = !call__46;
                            var call__47 = TranslateRStr(QueueRStr);
                            MainBtn2.Text = call__47;
                            var call__48 = U.GetName();
                            var call__49 = TranslateRStr(InstallXRStr);
                            var call__50 = AoWE.AoWE.Format(call__49, [call__48]);
                            UnitLbl.Text = call__50;
                            var call__51 = FPlace.FProductionControl.UnitResearchGold(FProductionIndex);
                            UCost.Text = IntToStr(call__51);
                            var call__52 = FPlace.FProductionControl.UnitResearchTime(FProductionIndex);
                            UTurns.Text = IntToStr(call__52);
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
                    Item = new TProductionItem(null);
                    Item.FProductionID = 10;
                    var call__53 = FPlace.GetProductionName(Item);
                    MainBtn1.Text = call__53;
                    Item.Free();
                    var call__54 = FPlace.FProductionControl.GetQueueLocked();
                    MainBtn2.Visible = !call__54;
                    var call__55 = TranslateRStr(QueueRStr);
                    MainBtn2.Text = call__55;
                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    var call__56 = FPlace.GetMagicSphere();
                    if (call__56 == TMagicSphere.msNone)
                    {
                        var call__57 = TranslateRStr(ProduceMerchandiseTextRStr);
                        UpgradeMemo.Items.Text = call__57;
                    }
                    else
                    {
                        var call__58 = TranslateRStr(GenerateExtraPowerTextRStr);
                        UpgradeMemo.Items.Text = call__58;
                    }

                    UpgradePnl.Visible = true;
                    var call__59 = FPlace.GetMagicSphere();
                    if (call__59 != TMagicSphere.msNone)
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
                    var call__60 = TranslateRStr(RazeRStr);
                    MainBtn1.Text = call__60 + (AnsiString)"...";
                    PPrev.Visible = false;
                    PNext.Visible = false;
                    var call__61 = FPlace.GetName();
                    var call__62 = TranslateRStr(ProductionPlaceXRazeRStr);
                    var call__63 = AoWE.AoWE.Format(call__62, [call__61]);
                    UpgradeMemo.Items.Text = call__63;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 26;
                    var call__64 = TranslateRStr(RazeRStr);
                    UpgradeLbl.Text = call__64;
                    break;
            }
        }

        // AoWProdScreen.pas:832
        public void UpdateInfo()
        {
            AnsiString Reason = default;
            int I = default;
            int J = default;
            TProductionItem Item = default;
            IncomeHdr.Items.Clear();
            var call__65 = FPlace.GetName();
            StructName.Text = call__65;
            var call__66 = TranslateRStr(IncomeRStr);
            IncomeHdr.Items.Add(call__66);
            var call__67 = FPlace.GetMagicSphere();
            if (call__67 != TMagicSphere.msNone)
            {
                var call__68 = FPlace.GetPower();
                var call__69 = TranslateRStr(XPowerRStr);
                var call__70 = AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__69, [IntToStr(call__68)]);
                IncomeHdr.Items.Add(call__70);
            }

            var call__71 = FPlace.GetIncome();
            if (call__71 != 0)
            {
                var call__72 = FPlace.GetIncome();
                var call__73 = TranslateRStr(XGoldRStr);
                var call__74 = AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__73, [IntToStr(call__72)]);
                IncomeHdr.Items.Add(call__74);
            }

            var call__75 = FPlace.GetMagicSphere();
            if (call__75 != TMagicSphere.msNone)
            {
                IncomePicture.ILIndexTexture = 64;
                IPPicFr.ILTexture = null;
                var call__76 = FPlace.GetMagicSphere();
                var call__77 = AoWProdScreen.SphereImage(call__76);
                Sphere.SetILUIndex(call__77);
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

                IPPicFr.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
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
            {
                I = 0;
                int end__78 = FPlace.FProductionControl.FQueue.GetCount() - 1;
                for (; I <= end__78; I += 1)
                {
                    var call__79 = FPlace.FProductionControl.FQueue.GetItem(I);
                    var call__80 = call__79.GetName();
                    QueueLB.Items.Add(IntToStr(I + 1) + (AnsiString)". " + call__80);
                }
            }

            {
                I = FPlace.FProductionControl.FQueue.GetCount();
                byte end__81 = 4;
                for (; I <= end__81; I += 1)
                {
                    var call__82 = TranslateRStr(EmptyQueueRStr);
                    QueueLB.Items.Add(IntToStr(I + 1) + (AnsiString)". " + call__82);
                }
            }

            var call__83 = FPlace.FProductionControl.FQueue.GetCount();
            if (QueueLB.Selected >= call__83)
            {
                var call__84 = FPlace.FProductionControl.FQueue.GetCount();
                QueueLB.Selected = call__84 - 1;
                FQueueIndex = QueueLB.Selected;
            }

            LoopQueueBox.Checked = FPlace.FProductionControl.FLoopQueue;
            if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsProducing)
            {
                var call__85 = FPlace.FProductionControl.FQueue.GetItem(0);
                var call__86 = call__85.GetNameProgressive();
                ActiveAction.Text = call__86;
                if (FPlace.FProductionControl.FSettings.FTurnsLeft != -1)
                {
                    var call__87 = FmtTurnsLeftStr(FPlace.FProductionControl.FSettings.FTurnsLeft);
                    ActiveActionTurns.Text = call__87;
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

                var call__89 = FPlace.FProductionControl.FQueue.GetItem(0);
                var call__90 = ActivityImage(call__89);
                CurrentActionImage.CurrentImage = call__90;
            }
            else
            {
                if (FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsIdle)
                {
                    var call__91 = FPlace.GetCurrentActivityText();
                    ActiveAction.Text = call__91;
                    var call__92 = TranslateRStr(DefaultRStr);
                    ActiveActionTurns.Text = AnsiString.FromBytes(40) + LowerCase(call__92) + AnsiString.FromBytes(41);
                    ActiveActionTurns.TextColor = 6783862;
                    var call__93 = TranslateRStr(NoneRStr);
                    if (CompareText(ActiveAction.Text, call__93) != 0)
                    {
                        ActiveActionTurns.Visible = true;
                        Item = new TProductionItem(null);
                        Item.FProductionID = 10;
                        var call__94 = ActivityImage(Item);
                        CurrentActionImage.CurrentImage = call__94;
                        Item.Free();
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
                        Reason = (AnsiString)"";
                        var call__95 = FPlace.FProductionControl.FQueue.GetItem(0);
                        var call__96 = call__95.GetNameProgressive();
                        ActiveAction.Text = call__96;
                        var call__97 = FPlace.FProductionControl.FQueue.GetItem(0);
                        FPlace.FProductionControl.CanExecuteProduction(call__97, ref Reason);
                        ActiveActionTurns.Text = Reason;
                        ActiveActionTurns.TextColor = 255;
                        ActiveActionTurns.Visible = true;
                        ActiveActionTurnsBar.Visible = false;
                        var call__98 = FPlace.FProductionControl.FQueue.GetItem(0);
                        var call__99 = ActivityImage(call__98);
                        CurrentActionImage.CurrentImage = call__99;
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
                    QueueLBChange(this);
                    break;
            }

            Dlg.FDrawn = false;
        }

        // AoWProdScreen.pas:180
        public void ShowPlace(TProductionPlace Place)
        {
            int I = default;
            TArmyHS Army = default;
            TRndTacHsm Generator = default;
            THexagonSprite Center = default;
            Reset();
            FPlace = Place;
            UpdateInfo();
            SetMode(0);
            var call__100 = Place.GetDescription();
            var call__101 = call__100.Text;
            PlaceInfoMemo.Items.Text = call__101;
            {
                I = 0;
                int end__102 = FPlace.FProductionControl.FUnitProductions.GetCount() - 1;
                for (; I <= end__102; I += 1)
                {
                    var call__103 = FPlace.FProductionControl.FUnitProductions.GetItem(I);
                    LoadUnitImages(call__103.FUnitIndex);
                }
            }

            FreePreview();
            Generator = new TRndTacHsm();
            var call__104 = FPlace.GetLhx();
            var call__105 = FPlace.GetYhx();
            var call__106 = FPlace.GetXhx();
            var call__107 = Generator.RandomMap(null, call__106, call__105, call__104);
            FPreview = call__107;
            Generator.Free();
            var call__108 = ((TAoWMapField)FPlace.Owner).GetArmyHS();
            Army = call__108;
            if (Army != null)
            {
                {
                    I = 0;
                    int end__109 = Army.FArmy.GetCount() - 1;
                    for (; I <= end__109; I += 1)
                    {
                        var call__110 = Army.FArmy.GetUnit(I);
                        PlaceOverviewUnit(FPreview, call__110, 0, I);
                    }
                }
            }

            var call__111 = FPlace.GetBaseIncome();
            B5CB.CaptureInput = call__111 != 0;
            Center = FPreview.FCenterViewLocation;
            {
                if (Center != null)
                {
                    var call__112 = FPreview.FCenterViewLocation.GetYhx();
                    var call__113 = Center.GetXhx();
                    HXtoHP(call__113, call__112, ref FViewX, ref FViewY);
                    FViewX = FViewX + 16 - MapView.WinWidth / 2;
                    FViewY = FViewY + 16 - MapView.WinHeight / 2;
                }
                else
                {
                    FViewX = 800;
                    FViewY = 800;
                }
            }

            General.Manager.OverlayPriority = 49;
            Dlg.Visible = true;
        }

        // AoWProdScreen.pas:221
        public void ProductionScreenCreate()
        {
            var call__114 = new TIntegerList();
            FImages = call__114;
        }

        // AoWProdScreen.pas:226
        public void ProductionScreenDestroy()
        {
            FImages.Free();
        }

        // AoWProdScreen.pas:231
        public void ActivityChange(TObject Sender)
        {
            General.PlayButtonSound(1);
            if (((TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectProduction(((TAOWCheckBox)Sender).Tag);
                ((TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWProdScreen.pas:242
        public void ActivityDoubleClick(TObject Sender)
        {
            ResetChecks();
            SelectProduction(((TAOWCheckBox)Sender).Tag);
            ((TAOWCheckBox)Sender).Checked = true;
            MainBtn1Click(null);
        }

        // AoWProdScreen.pas:250
        public void ActivityPreDraw(TAoWComponent Sender, TSurfaceDesc SD)
        {
            TP2UnitShowInfo Info = default;
            TRect R = default;
            TUnit U = default;
            TProductionItem Item = default;
            var call__115 = Rect(ActivityPnl2.WinLeft + Sender.WinLeft, ActivityPnl2.WinTop + Sender.WinTop, ActivityPnl2.WinLeft + Sender.WinLeft + Sender.WinWidth, ActivityPnl2.WinTop + Sender.WinTop + Sender.WinHeight);
            R = call__115;
            switch (Sender.Tag)
            {
                case >= 0 and <= 3:
                    var call__116 = FPlace.FProductionControl.CreateUnitProduction(Sender.Tag);
                    U = call__116;
                    if (U != null)
                    {
                        Info.Direction = 3;
                        Info.Overlays = TSet<TUnitOverlayModesElement>.Of();
                        Info.Animation = 0;
                        Info.Frame = 0;
                        Info.Looped = false;
                        var call__117 = FPlace.FProductionControl.FUnitProductions.GetItem(Sender.Tag);
                        if (!call__117.FResearched)
                        {
                            var call__118 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                            call__118.Show(R.Left + 2, R.Top + 2, SD);
                            Info.Direction = 5;
                            if (U != null)
                            {
                                U.ShowEx(R.Left - 4, R.Top, SD, Info);
                            }

                            var call__119 = AlphaBlendTable.GetLookupTables(45);
                            var call__120 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                            call__120.ShowBlended(R.Left + 2, R.Top + 2, SD, call__119);
                        }
                        else
                        {
                            var call__121 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
                            call__121.Show(R.Left + 2, R.Top + 2, SD);
                            if (U != null)
                            {
                                U.ShowEx(R.Left - 4, R.Top, SD, Info);
                            }
                        }

                        U.Release();
                    }
                    else
                    {
                        var call__122 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                        call__122.Show(R.Left + 2, R.Top + 2, SD);
                    }

                    break;
                case 4:
                    var call__123 = FPlace.GetBaseIncome();
                    if (call__123 != 0)
                    {
                        Item = new TProductionItem(null);
                        Item.FProductionID = 10;
                        var call__124 = ActivityImage(Item);
                        var call__125 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[call__124];
                        call__125.Show(R.Left + 2, R.Top + 2, SD);
                        Item.Free();
                    }
                    else
                    {
                        var call__126 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                        call__126.Show(R.Left + 2, R.Top + 2, SD);
                    }

                    break;
                case 5:
                    var call__127 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[46];
                    call__127.Show(R.Left + 2, R.Top + 2, SD);
                    break;
            }
        }

        // AoWProdScreen.pas:299
        public void MainBtn1Click(TObject Sender)
        {
            AnsiString Reason = default;
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
                            var call__128 = FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex);
                            if (call__128.FResearched)
                            {
                                var call__129 = FPlace.CanProduce(FProductionIndex, false, ref Reason);
                                if (call__129)
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
                                var call__130 = FPlace.CanResearch(FProductionIndex, false, ref Reason);
                                if (call__130)
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
                            var call__131 = FPlace.CanTrade(false, ref Reason);
                            if (call__131)
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
                            var call__132 = FPlace.CanRaze(AoWHSMap.FSeatedPlayer, ref Reason);
                            if (call__132)
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
            AnsiString Reason = default;
            switch (FMode)
            {
                case 1:
                    switch (FProductionIndex)
                    {
                        case >= 0 and <= 3:
                            var call__133 = FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex);
                            if (call__133.FResearched)
                            {
                                var call__134 = FPlace.CanProduce(FProductionIndex, true, ref Reason);
                                if (call__134)
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
                                var call__135 = FPlace.CanResearch(FProductionIndex, true, ref Reason);
                                if (call__135)
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
                            var call__136 = FPlace.CanTrade(true, ref Reason);
                            if (call__136)
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
                        var call__137 = FPlace.FProductionControl.RetryProductionExecution(ref Reason);
                        if (call__137)
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
        public void UFaceDraw(TObject Sender, TSurfaceDesc SD)
        {
            TUnit U = default;
            TRect R = default;
            var call__138 = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
            U = call__138;
            if (U != null)
            {
                var call__139 = Rect(UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + 2, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + 2, UPictureFramePnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth - 4, UPictureFramePnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight - 2);
                R = call__139;
                U.ShowFace(R, SD);
                U.Release();
            }
        }

        // AoWProdScreen.pas:406
        public void QueueLBChange(TObject Sender)
        {
            var call__140 = FPlace.FProductionControl.FQueue.GetCount();
            if (QueueLB.Selected < call__140 && FMode != 0)
            {
                if (QueueLB.Selected != -1)
                {
                    FQueueIndex = QueueLB.Selected;
                    var call__141 = FPlace.FProductionControl.FQueue.GetItem(FQueueIndex);
                    if (call__141.FProductionID == 1)
                    {
                        var call__142 = FPlace.FProductionControl.FQueue.GetItem(FQueueIndex);
                        SelectProduction(call__142.FParameter);
                    }
                    else
                    {
                        SelectProduction(4);
                    }

                    SetMode(2);
                    QueueLB.Selected = FQueueIndex;
                    MainBtn1.Visible = true;
                    var call__143 = TranslateRStr(DeleteRStr);
                    MainBtn1.Text = call__143;
                    if (FQueueIndex == 0 && FPlace.FProductionControl.FStatus == TProductionControlStatus.pcsWaiting)
                    {
                        MainBtn2.Visible = true;
                        var call__144 = TranslateRStr(ExecuteRStr);
                        MainBtn2.Text = call__144;
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
        public void B4CBUpdate(TObject Sender)
        {
            if (((TAOWCheckBox)Sender).Tag <= 3)
            {
                var call__145 = FPlace.FProductionControl.FUnitProductions.GetItem(((TAOWCheckBox)Sender).Tag);
                ((TAOWCheckBox)Sender).CaptureInput = call__145 != null;
            }
        }

        // AoWProdScreen.pas:445
        public void LoopQueueBoxChange()
        {
            FPlace.FProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
        }

        // AoWProdScreen.pas:450
        public void CurrentActionImageDraw(TObject Sender, TSurfaceDesc SD)
        {
            TP2UnitShowInfo Info = default;
            TRect R = default;
            TUnit U = default;
            var call__146 = FPlace.FProductionControl.FQueue.GetCount();
            if (call__146 != 0)
            {
                var call__147 = FPlace.FProductionControl.FQueue.GetItem(0);
                bool bool__148 = call__147.FProductionID == 1;
                if (!bool__148)
                {
                    var call__149 = FPlace.FProductionControl.FQueue.GetItem(0);
                    bool__148 = call__149.FProductionID == 2;
                }

                if (bool__148)
                {
                    var call__150 = Rect(ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop, ActiveActionPnl.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, ActiveActionPnl.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
                    R = call__150;
                    var productionPlaceControl = FPlace.FProductionControl;
                    var call__152 = productionPlaceControl.FUnitProductions.GetItem(productionPlaceControl.FSettings.FUnitProductionIndex);
                    if (call__152 != null)
                    {
                        var call__153 = FPlace.FProductionControl.CreateUnitProduction(FPlace.FProductionControl.FSettings.FUnitProductionIndex);
                        U = call__153;
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
        public void MapViewDraw(TObject Sender, TSurfaceDesc SD)
        {
            TSurfaceDesc ViewSD = default;
            TRect R = default;
            int W = default;
            int H = default;
            int Width = default;
            if (FPreview != null)
            {
                var call__154 = Rect(Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((TAoWComponent)Sender).WinTop, Dlg.WinLeft + ((TAoWComponent)Sender).WinLeft + ((TAoWComponent)Sender).WinWidth, Dlg.WinTop + ((TAoWComponent)Sender).WinTop + ((TAoWComponent)Sender).WinHeight);
                R = call__154;
                Width = R.Right - R.Left;
                W = Width;
                H = R.Bottom - R.Top;
                var call__155 = FPreview.GetMaxXhp();
                if (FViewX + W + 16 > call__155)
                {
                    var call__156 = FPreview.GetMaxXhp();
                    FViewX = call__156 - W - 16;
                }

                var call__157 = FPreview.GetMaxYhp();
                if (FViewY + H + 16 > call__157)
                {
                    var call__158 = FPreview.GetMaxYhp();
                    FViewY = call__158 - H - 16;
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
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
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
            TAoWCombatMap Preview = default;
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
        public void UAbilitiesLBMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TAbility A = default;
            if (Button == TMouseButton.mbRight && UAbilitiesLB.Selected > -1)
            {
                var call__159 = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
                var call__160 = AoWHSSet.FAbilityControl.GetAbility(ObjectToInt(call__159));
                A = call__160;
                if (A != null)
                {
                    var call__161 = A.GetName(null);
                    SetInfoBanner(AoWInfoBanner.AoWInfoBanner.InfoBanner, call__161, A.FDescription);
                }
            }
        }

        // AoWProdScreen.pas:546
        public void PPrevClick()
        {
            int Start = default;
            int Index = default;
            TPlayerStructure S = default;
            var call__162 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__163 = call__162.FStructures.FindChildIndex(FPlace);
            Start = call__163;
            Index = Start;
            Index -= 1;
            if (Index < 0)
            {
                var call__164 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__165 = call__164.FStructures.GetCount();
                Index = call__165 - 1;
            }

            while (Index != Start)
            {
                var call__166 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__167 = call__166.FStructures.GetStructure(Index);
                S = call__167;
                if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
                {
                    Index = Start + 1;
                    ShowPlace((TProductionPlace)S);
                }

                Index += -1;
                if (Index < 0)
                {
                    var call__168 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                    var call__169 = call__168.FStructures.GetCount();
                    Index = call__169 - 1;
                }
            }
        }

        // AoWProdScreen.pas:568
        public void PNextClick()
        {
            int Start = default;
            int Index = default;
            TPlayerStructure S = default;
            var call__170 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__171 = call__170.FStructures.FindChildIndex(FPlace);
            Start = call__171;
            Index = Start + 1;
            var call__172 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
            var call__173 = call__172.FStructures.GetCount();
            if (Index >= call__173)
            {
                Index = 0;
            }

            while (Index != Start)
            {
                var call__174 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__175 = call__174.FStructures.GetStructure(Index);
                S = call__175;
                if (S is TProductionPlace && ((TProductionPlace)S).GetMagicSphere() == TMagicSphere.msNone)
                {
                    Index = Start - 1;
                    ShowPlace((TProductionPlace)S);
                }

                Index += 1;
                var call__176 = AoWHSMap.FPlayers.GetPlayers(AoWHSMap.FSeatedPlayer);
                var call__177 = call__176.FStructures.GetCount();
                if (Index >= call__177)
                {
                    Index = 0;
                }
            }
        }
    }

    public static partial class AoWProdScreen
    {
        public static TProductionScreen ProductionScreen;
        // AoWProdScreen.pas:819
        public static int SphereImage(TMagicSphere Sphere)
        {
            int Result = default;
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
}
