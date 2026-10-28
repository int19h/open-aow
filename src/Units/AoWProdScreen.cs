// Translated from AoWProdScreen.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.PrdPlace;
using static Aow.Units.PrdPlace.PrdPlace;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWTC;
using static Aow.Units.AoWTC.AoWTC;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWGauge;
using static Aow.Units.AOWGauge.AOWGauge;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWCheckBox;
using static Aow.Units.AoWCheckBox.AoWCheckBox;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.aowUserDrawBox;
using static Aow.Units.aowUserDrawBox.aowUserDrawBox;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.OverviewUnit;
using static Aow.Units.OverviewUnit.OverviewUnit;
using Aow.Units.Shipyard;
using static Aow.Units.Shipyard.Shipyard;

namespace Aow.Units.AoWProdScreen
{
    // { TProductionScreen: size $200; $1E0/$1E4 view, $1E8 place, $1EC production,
//     $1F0 queue, $1F4 mode, $1F8 preview, $1FC images. VMT 0043E654. }
    public partial class TProductionScreen : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel UnitPnl;
        // { +0048 }
        public Aow.Units.AoWListBox.TAOWListBox UAbilitiesLB;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage UAttackIcon;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage UDefenseIcon;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage UMovesIcon;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage UDamageIcon;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage UResistanceIcon;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage UHitsIcon;
        // { +0064 }
        public Aow.Units.AoWLabel.TAOWLabel UAttack;
        // { +0068 }
        public Aow.Units.AoWLabel.TAOWLabel UDefense;
        // { +006C }
        public Aow.Units.AoWLabel.TAOWLabel UMoves;
        // { +0070 }
        public Aow.Units.AoWLabel.TAOWLabel UDamage;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel UResistance;
        // { +0078 }
        public Aow.Units.AoWLabel.TAOWLabel UHits;
        // { +007C }
        public Aow.Units.AoWPanel.TAOWPanel UStatPnl;
        // { +0080 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP1;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel UStatP2;
        // { +0088 }
        public Aow.Units.AoWLabel.TAOWLabel UAlignment;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel ULevel;
        // { +0090 }
        public Aow.Units.AoWPanel.TAOWPanel UPicturePnl;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel UCostPnl;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel StructName;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel CostLbl;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel UCost;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel UnitLbl;
        // { +00A8 }
        public Aow.Units.AoWLabel.TAOWLabel TurnsLbl;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel UTurns;
        // { +00B0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B4CB;
        // { +00B4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B5CB;
        // { +00B8 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B6CB;
        // { +00BC }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B1CB;
        // { +00C0 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B2CB;
        // { +00C4 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox B3CB;
        // { +00C8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar T2SBar;
        // { +00CC }
        public Aow.Units.AoWPanel.TAOWPanel AbilPnl;
        // { +00D0 }
        public Aow.Units.AoWPanel.TAOWPanel UpgradePnl;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeLbl;
        // { +00D8 }
        public Aow.Units.AoWPanel.TAOWPanel UpgradeMemoFr;
        // { +00DC }
        public Aow.Units.AoWMemo.TAOWMemo UpgradeMemo;
        // { +00E0 }
        public Aow.Units.AoWPanel.TAOWPanel RightPnl;
        // { +00E4 }
        public Aow.Units.AoWPanel.TAOWPanel QueuePnl;
        // { +00E8 }
        public Aow.Units.AoWImage.TAOWImage Oval;
        // { +00EC }
        public Aow.Units.AoWImage.TAOWImage ShadowL;
        // { +00F0 }
        public Aow.Units.AoWImage.TAOWImage ShadowR;
        // { +00F4 }
        public Aow.Units.AoWButton.TAOWButton MainBtn3;
        // { +00F8 }
        public Aow.Units.AoWButton.TAOWButton MainBtn1;
        // { +00FC }
        public Aow.Units.AoWListBox.TAOWListBox QueueLB;
        // { +0100 }
        public Aow.Units.AoWPanel.TAOWPanel ActivityPnl;
        // { +0104 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl2;
        // { +0108 }
        public Aow.Units.AoWLabel.TAOWLabel ActiveAction;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel ActiveActionTurns;
        // { +0110 }
        public Aow.Units.AoWImage.TAOWImage SepBar;
        // { +0114 }
        public Aow.Units.AoWPanel.TAOWPanel ActivityPnl2;
        // { +0118 }
        public Aow.Units.AOWGauge.TAoWGauge ActiveActionTurnsBar;
        // { +011C }
        public Aow.Units.AoWButton.TAOWButton MainBtn2;
        // { +0120 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl;
        // { +0124 }
        public Aow.Units.AoWPanel.TAOWPanel IPPnl;
        // { +0128 }
        public Aow.Units.AoWFrame.TAoWFrame IPPicFr;
        // { +012C }
        public Aow.Units.AoWMemo.TAOWMemo PlaceInfoMemo;
        // { +0130 }
        public Aow.Units.AoWPanel.TAOWPanel IIPnl;
        // { +0134 }
        public Aow.Units.AoWFrame.TAoWFrame IncomePicture;
        // { +0138 }
        public Aow.Units.AoWMemo.TAOWMemo IncomeMemo;
        // { +013C }
        public Aow.Units.AoWMemo.TAOWMemo IncomeHdr;
        // { +0140 }
        public Aow.Units.AoWPanel.TAOWPanel CurrentInfo;
        // { +0144 }
        public Aow.Units.AoWFrame.TAoWFrame RPillar;
        // { +0148 }
        public Aow.Units.AoWFrame.TAoWFrame LPillar;
        // { +014C }
        public Aow.Units.AoWPanel.TAOWPanel QueueInfo;
        // { +0150 }
        public Aow.Units.AoWLabel.TAOWLabel QueueLbl;
        // { +0154 }
        public Aow.Units.AoWCheckBox.TAOWCheckBox LoopQueueBox;
        // { +0158 }
        public Aow.Units.AoWLabel.TAOWLabel RepeatLbl;
        // { +015C }
        public Aow.Units.AoWPanel.TAOWPanel ActiveActionPnl;
        // { +0160 }
        public Aow.Units.AoWImage.TAOWImage CurrentActionImage;
        // { +0164 }
        public Aow.Units.AoWFrame.TAoWFrame AoWFrame1;
        // { +0168 }
        public Aow.Units.AoWImage.TAOWImage BlackBg2;
        // { +016C }
        public Aow.Units.AoWPanel.TAOWPanel UPictureFramePnl;
        // { +0170 }
        public Aow.Units.AoWPanel.TAOWPanel UStatFr1;
        // { +0174 }
        public Aow.Units.AoWPanel.TAOWPanel UStatFr2;
        // { +0178 }
        public Aow.Units.AoWPanel.TAOWPanel UInfoFr;
        // { +017C }
        public Aow.Units.AoWPanel.TAOWPanel CostFr;
        // { +0180 }
        public Aow.Units.AoWPanel.TAOWPanel TurnsFr;
        // { +0184 }
        public Aow.Units.AoWPanel.TAOWPanel UpgPicPnl;
        // { +0188 }
        public Aow.Units.AoWImage.TAOWImage UpgradePic;
        // { +018C }
        public Aow.Units.AoWPanel.TAOWPanel UpgPicFr;
        // { +0190 }
        public Aow.Units.AoWPanel.TAOWPanel UpgCostPnl;
        // { +0194 }
        public Aow.Units.AoWLabel.TAOWLabel UpgCostLbl;
        // { +0198 }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeCost;
        // { +019C }
        public Aow.Units.AoWLabel.TAOWLabel UpgTurnsLbl;
        // { +01A0 }
        public Aow.Units.AoWLabel.TAOWLabel UpgradeTurns;
        // { +01A4 }
        public Aow.Units.AoWPanel.TAOWPanel UpgCostFr;
        // { +01A8 }
        public Aow.Units.AoWPanel.TAOWPanel UpgTurnsFr;
        // { +01AC }
        public Aow.Units.AoWImage.TAOWImage RelIcn;
        // { +01B0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar QueueVSB;
        // { +01B4 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IPVSB;
        // { +01B8 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar IIVSB;
        // { +01BC }
        public Aow.Units.AoWMemo.TAOWMemo IncomeValuesMemo;
        // { +01C0 }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox UFace;
        // { +01C4 }
        public Aow.Units.AoWImage.TAOWImage InfoSep;
        // { +01C8 }
        public Aow.Units.AoWButton.TAOWButton Sphere;
        // { +01CC }
        public Aow.Units.aowUserDrawBox.TaowUserDrawBox MapView;
        // { +01D0 }
        public Aow.Units.AoWButton.TAOWButton PNext;
        // { +01D4 }
        public Aow.Units.AoWButton.TAOWButton PPrev;
        // { +01D8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +01DC }
        public int FViewX;
        // { $1E0 }
        public int FViewY;
        // { $1E4 }
        public Aow.Units.PrdPlace.TProductionPlace FPlace;
        // { $1E8 }
        public int FProductionIndex;
        // { $1EC }
        public int FQueueIndex;
        // { $1F0 }
        public byte FMode;
        // { $1F4 }
        public Aow.Units.FixedArray_byte_3 F_ProductionScreen_1F5;
        public Aow.Units.AoWTC.TAoWCombatMap FPreview;
        // { $1F8 }
        public Aow.Units.Engine.TIntegerList FImages;
        // AoWProdScreen.pas:590
        // { $1FC }
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
                    var call__3 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(call__2);
                    call__3.Release();
                }
            }

            FImages.Clear();
        }

        // AoWProdScreen.pas:608
        public void LoadUnitImages(int Index)
        {
            int Gfx = default;
            var call__4 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitResources.GetUnitResource(Index);
            Gfx = call__4.FUnitGFXResourceIndex;
            var call__5 = Aow.Units.AoWE.AoWE.AoWHSSet.FUnitGFXResources.GetUnitGFXResource(Gfx);
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
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.OptionsRStr);
            MainBtn1.Text = call__6;
            MainBtn2.Visible = false;
            PPrev.Visible = !MainBtn2.Visible;
            PNext.Visible = !MainBtn2.Visible;
            ShadowL.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            ShadowR.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            Oval.Visible = Aow.Units.AoWE.AoWE.AoWEngine.FAnimationDetail != 0;
            FQueueIndex = -1;
            QueueLB.Selected = -1;
            var call__7 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
            QueueLbl.Text = call__7;
            var call__8 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RepeatRStr);
            RepeatLbl.Text = call__8;
            FreePreview();
        }

        // AoWProdScreen.pas:652
        public void ClosePlace()
        {
            Reset();
            FPlace = null;
            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
        }

        // AoWProdScreen.pas:659
        public void SetMode(byte Mode)
        {
            FMode = unchecked((byte)Mode);
            switch (FMode)
            {
                case 0:
                    InfoPnl.Visible = true;
                    InfoPnl2.Visible = true;
                    ActivityPnl.Visible = false;
                    ActivityPnl2.Visible = false;
                    var call__9 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ChangeRStr);
                    MainBtn1.Text = call__9;
                    MainBtn2.Visible = false;
                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    var call__10 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExitRStr);
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
                    var call__11 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InfoRStr);
                    MainBtn1.Text = call__11;
                    var call__12 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DoneRStr);
                    MainBtn3.Text = call__12;
                    break;
                case 2:
                    ResetChecks();
                    QueueLB.Selected = -1;
                    InfoPnl.Visible = false;
                    InfoPnl2.Visible = false;
                    ActivityPnl.Visible = true;
                    ActivityPnl2.Visible = true;
                    var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InfoRStr);
                    MainBtn1.Text = call__13;
                    var call__14 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DoneRStr);
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
        public void ProductionEvent(Aow.Units.PrdPlace.TProductionPlaceEventLog Event)
        {
            var call__15 = Event.GetProductionPlace();
            ShowPlace(call__15);
            SetMode(1);
            SelectDefaultAction();
        }

        // AoWProdScreen.pas:712
        public void PlaceChanged(Aow.Rtl.TObject Sender)
        {
            // { Tried with dialog and original hide-first branch; named dialog and hide-first branch; hide-first branch and explicit window view; no exact match. }
            if (Dlg.Visible && (Sender == FPlace))
            {
                if (FPlace.FPlayer == Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer)
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
        public int ActivityImage(Aow.Units.AoWE.TProductionItem Item)
        {
            int Result = default;
            switch (Item.FProductionID)
            {
                case 1:
                    Result = 4;
                    break;
                case 10:
                    var call__16 = FPlace.GetMagicSphere();
                    if (call__16 != Aow.Units.AoWE.TMagicSphere.msNone)
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
            Aow.Units.AoWE.TUnit U = default;
            Aow.Units.AoWE.TProductionItem Item = default;
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
                        UAttack.Text = Aow.Rtl.Rtl.IntToStr(call__18);
                        var call__19 = U.GetDefense();
                        UDefense.Text = Aow.Rtl.Rtl.IntToStr(call__19);
                        var call__20 = U.GetResistance();
                        UResistance.Text = Aow.Rtl.Rtl.IntToStr(call__20);
                        var call__21 = U.GetDamage();
                        UDamage.Text = Aow.Rtl.Rtl.IntToStr(call__21);
                        var call__22 = U.GetHits();
                        UHits.Text = Aow.Rtl.Rtl.IntToStr(call__22);
                        var call__23 = U.GetMoves();
                        UMoves.Text = Aow.Rtl.Rtl.IntToStr(call__23);
                        var call__24 = U.GetMoveTypes();
                        var call__25 = Aow.Units.AoWWinMan.AoWWinMan.General.MTtoInt(call__24);
                        UMovesIcon.CurrentImage = call__25;
                        var call__26 = U.GetAlignment();
                        var call__27 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.TAlignmentRStr[(int)call__26]);
                        var call__28 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AlignmentXRStr);
                        var call__29 = Aow.Units.AoWE.AoWE.Format(call__28, [call__27]);
                        UAlignment.Text = call__29;
                        var call__30 = U.GetUnitLevel();
                        var call__31 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.LevelXRStr);
                        var call__32 = Aow.Units.AoWE.AoWE.Format(call__31, [Aow.Rtl.Rtl.IntToStr(call__30)]);
                        var call__33 = U.GetUpkeep();
                        var call__34 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.UpkeepXRStr);
                        var call__35 = Aow.Units.AoWE.AoWE.Format(call__34, [(int)call__33]);
                        ULevel.Text = (call__32 + ((AnsiString)" / ")) + call__35;
                        UAbilitiesLB.Items.Clear();
                        U.ListAbilities(UAbilitiesLB.Items);
                        var call__36 = FPlace.FProductionControl.FUnitProductions.GetItem(FProductionIndex);
                        if (call__36.FResearched)
                        {
                            MainBtn1.Visible = true;
                            var call__37 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceRStr);
                            MainBtn1.Text = call__37;
                            var call__38 = FPlace.FProductionControl.GetQueueLocked();
                            MainBtn2.Visible = !call__38;
                            var call__39 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                            MainBtn2.Text = call__39;
                            var call__40 = U.GetName();
                            var call__41 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceXRStr);
                            var call__42 = Aow.Units.AoWE.AoWE.Format(call__41, [call__40]);
                            UnitLbl.Text = call__42;
                            var call__43 = FPlace.FProductionControl.UnitProductionGold(FProductionIndex);
                            UCost.Text = Aow.Rtl.Rtl.IntToStr(call__43);
                            var call__44 = FPlace.FProductionControl.UnitProductionTime(FProductionIndex);
                            UTurns.Text = Aow.Rtl.Rtl.IntToStr(call__44);
                        }
                        else
                        {
                            MainBtn1.Visible = true;
                            var call__45 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallRStr);
                            MainBtn1.Text = call__45;
                            var call__46 = FPlace.FProductionControl.GetQueueLocked();
                            MainBtn2.Visible = !call__46;
                            var call__47 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                            MainBtn2.Text = call__47;
                            var call__48 = U.GetName();
                            var call__49 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.InstallXRStr);
                            var call__50 = Aow.Units.AoWE.AoWE.Format(call__49, [call__48]);
                            UnitLbl.Text = call__50;
                            var call__51 = FPlace.FProductionControl.UnitResearchGold(FProductionIndex);
                            UCost.Text = Aow.Rtl.Rtl.IntToStr(call__51);
                            var call__52 = FPlace.FProductionControl.UnitResearchTime(FProductionIndex);
                            UTurns.Text = Aow.Rtl.Rtl.IntToStr(call__52);
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
                    Item = Aow.Units.AoWE.TProductionItem.Create(null);
                    Item.FProductionID = 10;
                    var call__53 = FPlace.GetProductionName(Item);
                    MainBtn1.Text = call__53;
                    Item.Free();
                    var call__54 = FPlace.FProductionControl.GetQueueLocked();
                    MainBtn2.Visible = !call__54;
                    var call__55 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.QueueRStr);
                    MainBtn2.Text = call__55;
                    PPrev.Visible = !MainBtn2.Visible;
                    PNext.Visible = !MainBtn2.Visible;
                    var call__56 = FPlace.GetMagicSphere();
                    if (call__56 == Aow.Units.AoWE.TMagicSphere.msNone)
                    {
                        var call__57 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProduceMerchandiseTextRStr);
                        UpgradeMemo.Items.Text = call__57;
                    }
                    else
                    {
                        var call__58 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.GenerateExtraPowerTextRStr);
                        UpgradeMemo.Items.Text = call__58;
                    }

                    UpgradePnl.Visible = true;
                    var call__59 = FPlace.GetMagicSphere();
                    if (call__59 != Aow.Units.AoWE.TMagicSphere.msNone)
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
                    var call__60 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RazeRStr);
                    MainBtn1.Text = call__60 + ((AnsiString)"...");
                    PPrev.Visible = false;
                    PNext.Visible = false;
                    var call__61 = FPlace.GetName();
                    var call__62 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ProductionPlaceXRazeRStr);
                    var call__63 = Aow.Units.AoWE.AoWE.Format(call__62, [call__61]);
                    UpgradeMemo.Items.Text = call__63;
                    UpgradePnl.Visible = true;
                    UpgradePic.CurrentImage = 26;
                    var call__64 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.RazeRStr);
                    UpgradeLbl.Text = call__64;
                    break;
            }
        }

        // AoWProdScreen.pas:832
        public void UpdateInfo()
        {
            Aow.Rtl.AnsiString Reason = default;
            int I = default;
            int J = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            IncomeHdr.Items.Clear();
            var call__65 = FPlace.GetName();
            StructName.Text = call__65;
            var call__66 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.IncomeRStr);
            IncomeHdr.Items.Add(call__66);
            var call__67 = FPlace.GetMagicSphere();
            if (call__67 != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                var call__68 = FPlace.GetPower();
                var call__69 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XPowerRStr);
                var call__70 = Aow.Units.AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__69, [Aow.Rtl.Rtl.IntToStr(call__68)]);
                IncomeHdr.Items.Add(call__70);
            }

            var call__71 = FPlace.GetIncome();
            if (call__71 != 0)
            {
                var call__72 = FPlace.GetIncome();
                var call__73 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.XGoldRStr);
                var call__74 = Aow.Units.AoWE.AoWE.Format(AnsiString.FromBytes(32) + call__73, [Aow.Rtl.Rtl.IntToStr(call__72)]);
                IncomeHdr.Items.Add(call__74);
            }

            var call__75 = FPlace.GetMagicSphere();
            if (call__75 != Aow.Units.AoWE.TMagicSphere.msNone)
            {
                IncomePicture.ILIndexTexture = 64;
                IPPicFr.ILTexture = null;
                var call__76 = FPlace.GetMagicSphere();
                var call__77 = Aow.Units.AoWProdScreen.AoWProdScreen.SphereImage(call__76);
                Sphere.SetILUIndex(call__77);
                Sphere.Visible = true;
            }
            else
            {
                if (FPlace is Aow.Units.Shipyard.TShipyard)
                {
                    IncomeHdr.Items.Clear();
                    IncomePicture.ILIndexTexture = 68;
                }
                else
                {
                    IncomePicture.ILIndexTexture = 65;
                }

                IPPicFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
                if (FPlace is Aow.Units.Shipyard.TShipyard)
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
                    QueueLB.Items.Add((Aow.Rtl.Rtl.IntToStr(I + 1) + ((AnsiString)". ")) + call__80);
                }
            }

            {
                I = FPlace.FProductionControl.FQueue.GetCount();
                byte end__81 = 4;
                for (; I <= end__81; I += 1)
                {
                    var call__82 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.EmptyQueueRStr);
                    QueueLB.Items.Add((Aow.Rtl.Rtl.IntToStr(I + 1) + ((AnsiString)". ")) + call__82);
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
            if (FPlace.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsProducing)
            {
                var call__85 = FPlace.FProductionControl.FQueue.GetItem(0);
                var call__86 = call__85.GetNameProgressive();
                ActiveAction.Text = call__86;
                if (FPlace.FProductionControl.FSettings.FTurnsLeft != -1)
                {
                    var call__87 = Aow.Units.AoWE.AoWE.FmtTurnsLeftStr(FPlace.FProductionControl.FSettings.FTurnsLeft);
                    ActiveActionTurns.Text = call__87;
                    ActiveActionTurns.Visible = true;
                    ActiveActionTurns.TextColor = 6783862;
                    {
                        var w__88 = ActiveActionTurnsBar;
                        w__88.Min = 0;
                        w__88.Max = FPlace.FProductionControl.FSettings.FTurnsTotal;
                        w__88.Position = FPlace.FProductionControl.FSettings.FTurnsTotal - FPlace.FProductionControl.FSettings.FTurnsLeft;
                    }

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
                if (FPlace.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsIdle)
                {
                    var call__91 = FPlace.GetCurrentActivityText();
                    ActiveAction.Text = call__91;
                    var call__92 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DefaultRStr);
                    ActiveActionTurns.Text = (AnsiString.FromBytes(40) + Aow.Rtl.Rtl.LowerCase(call__92)) + AnsiString.FromBytes(41);
                    ActiveActionTurns.TextColor = 6783862;
                    var call__93 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.NoneRStr);
                    if (Aow.Rtl.Rtl.CompareText(ActiveAction.Text, call__93) != 0)
                    {
                        ActiveActionTurns.Visible = true;
                        Item = Aow.Units.AoWE.TProductionItem.Create(null);
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
                    if (FPlace.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsWaiting)
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
        public void ShowPlace(Aow.Units.PrdPlace.TProductionPlace Place)
        {
            int I = default;
            Aow.Units.AoWE.TArmyHS Army = default;
            Aow.Units.AoWTC.TRndTacHsm Generator = default;
            Aow.Units.HSEngine.THexagonSprite Center = default;
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
            Generator = Aow.Units.AoWTC.TRndTacHsm.Create();
            var call__104 = FPlace.GetLhx();
            var call__105 = FPlace.GetYhx();
            var call__106 = FPlace.GetXhx();
            var call__107 = Generator.RandomMap(null, call__106, call__105, call__104);
            FPreview = call__107;
            Generator.Free();
            var call__108 = ((Aow.Units.AoWE.TAoWMapField)FPlace.Owner).GetArmyHS();
            Army = call__108;
            if (Army != null)
            {
                {
                    I = 0;
                    int end__109 = Army.FArmy.GetCount() - 1;
                    for (; I <= end__109; I += 1)
                    {
                        var call__110 = Army.FArmy.GetUnit(I);
                        Aow.Units.OverviewUnit.OverviewUnit.PlaceOverviewUnit(FPreview, call__110, 0, I);
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
                    Aow.Units.HSEngine.HSEngine.HXtoHP(call__113, call__112, ref FViewX, ref FViewY);
                    FViewX = (FViewX + 16) - (MapView.WinWidth / 2);
                    FViewY = (FViewY + 16) - (MapView.WinHeight / 2);
                }
                else
                {
                    FViewX = 800;
                    FViewY = 800;
                }
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 49;
            Dlg.Visible = true;
        }

        // AoWProdScreen.pas:221
        public void ProductionScreenCreate()
        {
            var call__114 = Aow.Units.Engine.TIntegerList.Create();
            FImages = call__114;
        }

        // AoWProdScreen.pas:226
        public void ProductionScreenDestroy()
        {
            FImages.Free();
        }

        // AoWProdScreen.pas:231
        public void ActivityChange(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(1);
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked)
            {
                ResetChecks();
                SelectProduction(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
                ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
            }
        }

        // AoWProdScreen.pas:242
        public void ActivityDoubleClick(Aow.Rtl.TObject Sender)
        {
            ResetChecks();
            SelectProduction(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
            ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Checked = true;
            MainBtn1Click(null);
        }

        // AoWProdScreen.pas:250
        public void ActivityPreDraw(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TUnit U = default;
            Aow.Units.AoWE.TProductionItem Item = default;
            var call__115 = Aow.Rtl.Rtl.Rect(ActivityPnl2.WinLeft + Sender.WinLeft, ActivityPnl2.WinTop + Sender.WinTop, (ActivityPnl2.WinLeft + Sender.WinLeft) + Sender.WinWidth, (ActivityPnl2.WinTop + Sender.WinTop) + Sender.WinHeight);
            R = call__115;
            switch (Sender.Tag)
            {
                case >= 0 and <= 3:
                    var call__116 = FPlace.FProductionControl.CreateUnitProduction(Sender.Tag);
                    U = call__116;
                    if (U != null)
                    {
                        Info.Direction = 3;
                        Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of();
                        Info.Animation = (Aow.Units.AoWE.TUnitAnimation)0;
                        Info.Frame = 0;
                        Info.Looped = false;
                        var call__117 = FPlace.FProductionControl.FUnitProductions.GetItem(Sender.Tag);
                        if (!call__117.FResearched)
                        {
                            var call__118 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                            call__118.Show(R.Left + 2, R.Top + 2, SD);
                            Info.Direction = 5;
                            if (U != null)
                            {
                                U.ShowEx(R.Left - 4, R.Top, SD, Info);
                            }

                            var call__119 = Aow.Units.GFXE.GFXE.AlphaBlendTable.GetLookupTables(45);
                            var call__120 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[5];
                            call__120.ShowBlended(R.Left + 2, R.Top + 2, SD, call__119);
                        }
                        else
                        {
                            var call__121 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[4];
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
                        var call__122 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                        call__122.Show(R.Left + 2, R.Top + 2, SD);
                    }

                    break;
                case 4:
                    var call__123 = FPlace.GetBaseIncome();
                    if (call__123 != 0)
                    {
                        Item = Aow.Units.AoWE.TProductionItem.Create(null);
                        Item.FProductionID = 10;
                        var call__124 = ActivityImage(Item);
                        var call__125 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[call__124];
                        call__125.Show(R.Left + 2, R.Top + 2, SD);
                        Item.Free();
                    }
                    else
                    {
                        var call__126 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[62];
                        call__126.Show(R.Left + 2, R.Top + 2, SD);
                    }

                    break;
                case 5:
                    var call__127 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin[46];
                    call__127.Show(R.Left + 2, R.Top + 2, SD);
                    break;
            }
        }

        // AoWProdScreen.pas:299
        public void MainBtn1Click(Aow.Rtl.TObject Sender)
        {
            Aow.Rtl.AnsiString Reason = default;
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
                                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
                                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                            }

                            break;
                        case 5:
                            var call__132 = FPlace.CanRaze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer), ref Reason);
                            if (call__132)
                            {
                                FPlace.Raze(unchecked((sbyte)Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer));
                                Dlg.Visible = false;
                            }
                            else
                            {
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
            Aow.Rtl.AnsiString Reason = default;
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
                                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
                                    Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
                                Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
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
                            Aow.Units.AoWE.AoWE.AoWHSMap.ShowWarning(Reason);
                        }
                    }

                    break;
            }
        }

        // AoWProdScreen.pas:383
        public void MainBtn3Click()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
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
        public void UFaceDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TUnit U = default;
            Aow.Rtl.TRect R = default;
            var call__138 = FPlace.FProductionControl.CreateUnitProduction(FProductionIndex);
            U = call__138;
            if (U != null)
            {
                var call__139 = Aow.Rtl.Rtl.Rect((UPictureFramePnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + 2, (UPictureFramePnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + 2, ((UPictureFramePnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth) - 4, ((UPictureFramePnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight) - 2);
                R = call__139;
                U.ShowFace(R, SD);
                U.Release();
            }
        }

        // AoWProdScreen.pas:406
        public void QueueLBChange(Aow.Rtl.TObject Sender)
        {
            var call__140 = FPlace.FProductionControl.FQueue.GetCount();
            if ((QueueLB.Selected < call__140) && (FMode != 0))
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
                    var call__143 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DeleteRStr);
                    MainBtn1.Text = call__143;
                    if ((FQueueIndex == 0) && (FPlace.FProductionControl.FStatus == Aow.Units.AoWE.TProductionControlStatus.pcsWaiting))
                    {
                        MainBtn2.Visible = true;
                        var call__144 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.ExecuteRStr);
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
        public void B4CBUpdate(Aow.Rtl.TObject Sender)
        {
            if (((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag <= 3)
            {
                var call__145 = FPlace.FProductionControl.FUnitProductions.GetItem(((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).Tag);
                ((Aow.Units.AoWCheckBox.TAOWCheckBox)Sender).CaptureInput = call__145 != null;
            }
        }

        // AoWProdScreen.pas:445
        public void LoopQueueBoxChange()
        {
            FPlace.FProductionControl.SetLoopQueueTo(LoopQueueBox.Checked);
        }

        // AoWProdScreen.pas:450
        public void CurrentActionImageDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.AoWE.TP2UnitShowInfo Info = default;
            Aow.Rtl.TRect R = default;
            Aow.Units.AoWE.TUnit U = default;
            var call__146 = FPlace.FProductionControl.FQueue.GetCount();
            if (call__146 != 0)
            {
                var call__147 = FPlace.FProductionControl.FQueue.GetItem(0);
                bool bool__148 = (call__147.FProductionID == 1);
                if (!bool__148)
                {
                    var call__149 = FPlace.FProductionControl.FQueue.GetItem(0);
                    bool__148 = (call__149.FProductionID == 2);
                }

                if (bool__148)
                {
                    var call__150 = Aow.Rtl.Rtl.Rect(ActiveActionPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, ActiveActionPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (ActiveActionPnl.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (ActiveActionPnl.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
                    R = call__150;
                    {
                        var w__151 = FPlace.FProductionControl;
                        var call__152 = w__151.FUnitProductions.GetItem(w__151.FSettings.FUnitProductionIndex);
                        if (call__152 != null)
                        {
                            var call__153 = FPlace.FProductionControl.CreateUnitProduction(FPlace.FProductionControl.FSettings.FUnitProductionIndex);
                            U = call__153;
                            if (U != null)
                            {
                                Info.Direction = 3;
                                Info.Overlays = TSet<Aow.Units.AoWE.TUnitOverlayModesElement>.Of();
                                Info.Animation = (Aow.Units.AoWE.TUnitAnimation)0;
                                Info.Frame = 0;
                                Info.Looped = false;
                                U.ShowEx(R.Left - 12, R.Top - 5, SD, Info);
                                U.Release();
                            }
                        }
                    }
                }
            }
        }

        // AoWProdScreen.pas:483
        public void MapViewDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            Aow.Units.GFXE.TSurfaceDesc ViewSD = default;
            Aow.Rtl.TRect R = default;
            int W = default;
            int H = default;
            int Width = default;
            if (FPreview != null)
            {
                var call__154 = Aow.Rtl.Rtl.Rect(Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft, Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop, (Dlg.WinLeft + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinLeft) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinWidth, (Dlg.WinTop + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinTop) + ((Aow.Units.AoWComp.TAoWComponent)Sender).WinHeight);
                R = call__154;
                Width = R.Right - R.Left;
                W = Width;
                H = R.Bottom - R.Top;
                var call__155 = FPreview.GetMaxXhp();
                if (((FViewX + W) + 16) > call__155)
                {
                    var call__156 = FPreview.GetMaxXhp();
                    FViewX = (call__156 - W) - 16;
                }

                var call__157 = FPreview.GetMaxYhp();
                if (((FViewY + H) + 16) > call__157)
                {
                    var call__158 = FPreview.GetMaxYhp();
                    FViewY = (call__158 - H) - 16;
                }

                if (FViewX < 16)
                {
                    FViewX = 16;
                }

                if (FViewY < 16)
                {
                    FViewY = 16;
                }

                Aow.Units.GFXE.GFXE.CreateSDfromSD(SD, R, ref ViewSD);
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
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
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
            Aow.Units.AoWTC.TAoWCombatMap Preview = default;
            Preview = FPreview;
            if (Preview != null)
            {
                Preview.NewFrame();
            }

            MapView.FDrawn = MapView.FDrawn && ((Aow.Units.AoWE.AoWE.AoWHSMap.Frame & 1) != 0);
        }

        // AoWProdScreen.pas:530
        public void DlgHide()
        {
            ClosePlace();
        }

        // AoWProdScreen.pas:535
        public void UAbilitiesLBMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Units.AoWE.TAbility A = default;
            if ((Button == Aow.Rtl.TMouseButton.mbRight) && (UAbilitiesLB.Selected > -1))
            {
                var call__159 = UAbilitiesLB.Items.Objects[UAbilitiesLB.Selected];
                var call__160 = Aow.Units.AoWE.AoWE.AoWHSSet.FAbilityControl.GetAbility(Aow.Rtl.Rtl.ObjectToInt(call__159));
                A = call__160;
                if (A != null)
                {
                    var call__161 = A.GetName(null);
                    Aow.Units.AoWExeForms.AoWExeForms.SetInfoBanner(Aow.Units.AoWInfoBanner.AoWInfoBanner.InfoBanner, call__161, A.FDescription);
                }
            }
        }

        // AoWProdScreen.pas:546
        public void PPrevClick()
        {
            int Start = default;
            int Index = default;
            Aow.Units.AoWE.TPlayerStructure S = default;
            var call__162 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__163 = call__162.FStructures.FindChildIndex(FPlace);
            Start = call__163;
            Index = Start;
            Index -= 1;
            if (Index < 0)
            {
                var call__164 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__165 = call__164.FStructures.GetCount();
                Index = call__165 - 1;
            }

            while (Index != Start)
            {
                var call__166 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__167 = call__166.FStructures.GetStructure(Index);
                S = call__167;
                if ((S is Aow.Units.PrdPlace.TProductionPlace) && (((Aow.Units.PrdPlace.TProductionPlace)S).GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone))
                {
                    Index = Start + 1;
                    ShowPlace((Aow.Units.PrdPlace.TProductionPlace)S);
                }

                Index += -1;
                if (Index < 0)
                {
                    var call__168 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
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
            Aow.Units.AoWE.TPlayerStructure S = default;
            var call__170 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__171 = call__170.FStructures.FindChildIndex(FPlace);
            Start = call__171;
            Index = Start + 1;
            var call__172 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
            var call__173 = call__172.FStructures.GetCount();
            if (Index >= call__173)
            {
                Index = 0;
            }

            while (Index != Start)
            {
                var call__174 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
                var call__175 = call__174.FStructures.GetStructure(Index);
                S = call__175;
                if ((S is Aow.Units.PrdPlace.TProductionPlace) && (((Aow.Units.PrdPlace.TProductionPlace)S).GetMagicSphere() == Aow.Units.AoWE.TMagicSphere.msNone))
                {
                    Index = Start - 1;
                    ShowPlace((Aow.Units.PrdPlace.TProductionPlace)S);
                }

                Index += 1;
                var call__176 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Aow.Units.AoWE.AoWE.AoWHSMap.FSeatedPlayer);
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
        public static Aow.Units.AoWProdScreen.TProductionScreen ProductionScreen;
        // AoWProdScreen.pas:819
        public static int SphereImage(Aow.Units.AoWE.TMagicSphere Sphere)
        {
            int Result = default;
            switch (Sphere)
            {
                case (Aow.Units.AoWE.TMagicSphere)1:
                    Result = 27;
                    break;
                case (Aow.Units.AoWE.TMagicSphere)5:
                    Result = 42;
                    break;
                case (Aow.Units.AoWE.TMagicSphere)3:
                    Result = 33;
                    break;
                case (Aow.Units.AoWE.TMagicSphere)4:
                    Result = 36;
                    break;
                case (Aow.Units.AoWE.TMagicSphere)6:
                    Result = 39;
                    break;
                case (Aow.Units.AoWE.TMagicSphere)2:
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
