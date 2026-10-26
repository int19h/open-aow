// Generated from AoWWorldMap.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWBookWindow;
using static Aow.Units.AoWBookWindow.AoWBookWindow;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWTargetSelection;
using static Aow.Units.AoWTargetSelection.AoWTargetSelection;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.E2CampaignSupport;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using Aow.Units.PageTurnCtrl;
using static Aow.Units.PageTurnCtrl.PageTurnCtrl;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWCampaignTransferDlg;
using static Aow.Units.AoWCampaignTransferDlg.AoWCampaignTransferDlg;
using Aow.Units.AoWLeaderSetup;
using static Aow.Units.AoWLeaderSetup.AoWLeaderSetup;

namespace Aow.Units.AoWWorldMap
{
    // { TWorldMap: size $16C; $13C star image, $140/$144 brightness, $148/$14C path animation, $150/$154 pawn timing, $158 hover, $15C context, $160 completion event, $168 book action. VMT 0041B5B8. }
    public partial class TWorldMap : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWMemo.TAOWMemo MemoL;
        // { +0044 }
        public Aow.Units.AoWImage.TAOWImage PicR;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel WorldPnl;
        // { +004C }
        public Aow.Units.AoWImage.TAOWImage Path1;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage Path2;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage Path3;
        // { +0058 }
        public Aow.Units.AoWImage.TAOWImage PPawn;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage EPawn;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel DescLbl;
        // { +0064 }
        public Aow.Units.AoWButton.TAOWButton Back;
        // { +0068 }
        public Aow.Units.AoWButton.TAOWButton Next;
        // { +006C }
        public Aow.Units.AoWButton.TAOWButton Cancel;
        // { +0070 }
        public Aow.Units.AoWButton.TAOWButton BookStart;
        // { +0074 }
        public Aow.Units.AoWLabel.TAOWLabel LabelL;
        // { +0078 }
        public Aow.Units.AoWMemo.TAOWMemo MemoR;
        // { +007C }
        public Aow.Units.AoWLabel.TAOWLabel LabelR;
        // { +0080 }
        public Aow.Units.AoWImage.TAOWImage PicL;
        // { +0084 }
        public Aow.Units.AoWBookWindow.TAoWBookWindow BookWin;
        // { +0088 }
        public Aow.Units.AoWPanel.TAOWPanel InfoPnl;
        // { +008C }
        public Aow.Units.AoWLabel.TAOWLabel Elves;
        // { +0090 }
        public Aow.Units.AoWLabel.TAOWLabel Halflings;
        // { +0094 }
        public Aow.Units.AoWLabel.TAOWLabel Goblins;
        // { +0098 }
        public Aow.Units.AoWLabel.TAOWLabel Orcs;
        // { +009C }
        public Aow.Units.AoWLabel.TAOWLabel Dwarves;
        // { +00A0 }
        public Aow.Units.AoWLabel.TAOWLabel Undead;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel Highmen;
        // { +00A8 }
        public Aow.Units.AoWLabel.TAOWLabel Lizardmen;
        // { +00AC }
        public Aow.Units.AoWLabel.TAOWLabel DarkElves;
        // { +00B0 }
        public Aow.Units.AoWLabel.TAOWLabel VoW;
        // { +00B4 }
        public Aow.Units.AoWLabel.TAOWLabel ElvenC;
        // { +00B8 }
        public Aow.Units.AoWLabel.TAOWLabel IoLG;
        // { +00BC }
        public Aow.Units.AoWLabel.TAOWLabel Deepmir;
        // { +00C0 }
        public Aow.Units.AoWLabel.TAOWLabel Aldor;
        // { +00C4 }
        public Aow.Units.AoWLabel.TAOWLabel Serpent;
        // { +00C8 }
        public Aow.Units.AoWLabel.TAOWLabel Blackwater;
        // { +00CC }
        public Aow.Units.AoWLabel.TAOWLabel Skull;
        // { +00D0 }
        public Aow.Units.AoWLabel.TAOWLabel Sylvanus;
        // { +00D4 }
        public Aow.Units.AoWLabel.TAOWLabel Pass;
        // { +00D8 }
        public Aow.Units.AoWLabel.TAOWLabel Arcalot;
        // { +00DC }
        public Aow.Units.AoWLabel.TAOWLabel Flowershire;
        // { +00E0 }
        public Aow.Units.AoWLabel.TAOWLabel Sobek;
        // { +00E4 }
        public Aow.Units.AoWLabel.TAOWLabel Steppe;
        // { +00E8 }
        public Aow.Units.AoWLabel.TAOWLabel Bronnam;
        // { +00EC }
        public Aow.Units.AoWLabel.TAOWLabel UC;
        // { +00F0 }
        public Aow.Units.AoWLabel.TAOWLabel Woods;
        // { +00F4 }
        public Aow.Units.AoWLabel.TAOWLabel Burbatak;
        // { +00F8 }
        public Aow.Units.AoWButton.TAOWButton QuitBtn;
        // { +00FC }
        public Aow.Units.AoWButton.TAOWButton ShowBtn;
        // { +0100 }
        public Aow.Units.AoWLabel.TAOWLabel SelectLbl;
        // { +0104 }
        public Aow.Units.AoWLabel.TAOWLabel AlliesLnl;
        // { +0108 }
        public Aow.Units.AoWPanel.TAOWPanel Frame;
        // { +010C }
        public Aow.Units.AoWLabel.TAOWLabel Azrac;
        // { +0110 }
        public Aow.Units.AoWImage.TAOWImage StarImg;
        // { +0114 }
        public Aow.Units.AoWLabel.TAOWLabel Terstar;
        // { +0118 }
        public Aow.Units.AoWFrame.TAoWFrame HeroPnl;
        // { +011C }
        public Aow.Units.AoWFrame.TAoWFrame Ally1Pnl;
        // { +0120 }
        public Aow.Units.AoWFrame.TAoWFrame Ally2Pnl;
        // { +0124 }
        public Aow.Units.AoWFrame.TAoWFrame Ally3Pnl;
        // { +0128 }
        public Aow.Units.AoWButton.TAOWButton SetBtn;
        // { +012C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0130 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar leftSB;
        // { +0134 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar rightSB;
        // { +0138 }
        public int FStarImage;
        // { $13C }
        public int FStarStep;
        // { $140 }
        public int FStarIntensity;
        // { $144 }
        public int FPathFrame;
        // { $148 }
        public int FPathDelay;
        // { $14C }
        public int FPawnTicks;
        // { $150 }
        public int FEnemyDelay;
        // { $154 }
        public bool FDescriptionHandled;
        // { $158 }
        public Aow.Units.FixedArray_byte_3 F_WorldMap_159;
        public Aow.Units.Engine.TEObject FBookContext;
        // { $15C }
        public Aow.Rtl.TNotifyEvent FOnBookClosed;
        // { $160 }
        public byte FBookAction;
        // { $168 }
        public Aow.Units.FixedArray_byte_3 F_WorldMap_169;
        // AoWWorldMap.pas:152
        public Aow.Units.AoWVideoDlg.TWMCampaign GetCampaign()
        {
            Aow.Units.AoWVideoDlg.TWMCampaign Result = default;
            Result = (Aow.Units.AoWVideoDlg.TWMCampaign)Aow.Units.AoWE.AoWE.AoWEngine.FCampaign;
            return Result;
        }

        // AoWWorldMap.pas:157
        public void UpdateCampaignState()
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // AoWWorldMap.pas:347
        // { 0041CA5C }
        public void ShowIntroPages(int Index)
        {
            /* TODO(T-shim) Platform or editor boundary SysUtils.SetCurrentDir */
            throw new System.NotImplementedException();
        }

        // AoWWorldMap.pas:404
        public void ShowSpheres()
        {
            int Picks = default;
            Aow.Units.AoWE.TLeaderSetupControl Control = default;
            WorldPnl.Visible = false;
            var call__1 = GetCampaign();
            Picks = call__1.FTransferSettings.FLeader.FSpherePicksMax;
            Control = Aow.Units.AoWE.TLeaderSetupControl.Create(null);
            var call__2 = GetCampaign();
            Control.FLeader.Assign(call__2.FTransferSettings.FLeader, Aow.Units.AoWE.AoWE.AoWEngine);
            Control.FLeader.SetSpherePicksMax(unchecked((byte)(Picks + 2)));
            Control.FOriginalLeader.Assign(Control.FLeader, Aow.Units.AoWE.AoWE.AoWEngine);
            Control.FMode = TSet<Aow.Units.AoWE.TLeaderSetupModeElement>.Of(Aow.Units.AoWE.TLeaderSetupModeElement.lsmSpheres);
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Execute(Control);
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.ClearBtn.Visible = false;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.DefaultBtn.Visible = false;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.PrevBtn.Visible = false;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.SetFixedSpheres(Picks);
            var call__3 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.AssignXNewSpheresRStr);
            var call__4 = Aow.Units.AoWE.AoWE.Format(call__3, [Aow.Rtl.Rtl.IntToStr(2)]);
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.Phase3Header.Text = call__4;
            Control.FOnDone = SpheresDone;
            Control.Release();
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Aow.Units.AoWLeaderSetup.AoWLeaderSetup.LeaderSetupWin.LeaderPnl.ILIndexTexture = 0;
        }

        // AoWWorldMap.pas:386
        public void SpheresDone(Aow.Rtl.TObject Sender)
        {
            Aow.Units.AoWE.TLeaderSetupControl Control = default;
            Aow.Units.AoWE.TLeader Leader = default;
            int I = default;
            Control = (Aow.Units.AoWE.TLeaderSetupControl)Sender;
            {
                var w__5 = (Aow.Units.AoWE.TLeaderSetupControl)Sender;
                if (w__5.FStatus == Aow.Units.AoWE.TLeaderSetupStatus.lssDone)
                {
                    var call__6 = w__5.FOriginalLeader.Copy(Aow.Units.AoWE.AoWE.AoWEngine);
                    Leader = (Aow.Units.AoWE.TLeader)call__6;
                    {
                        I = GetCampaign().FTransferSettings.FLeader.GetSphereCount();
                        int end__7 = Leader.GetSphereCount() - 1;
                        for (; I <= end__7; I += 1)
                        {
                            var call__8 = Leader.GetSpheres(I);
                            var call__9 = GetCampaign();
                            call__9.FTransferSettings.FSelectedSpheres.Add(unchecked((byte)((int)call__8)));
                        }
                    }

                    WorldPnl.Visible = true;
                    var call__10 = GetCampaign();
                    call__10.FState += 1;
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:428
        public void BookClosed()
        {
            Aow.Rtl.TNotifyEvent Done = default;
            Aow.Units.Engine.TEObject Context = default;
            Done = FOnBookClosed;
            Context = FBookContext;
            FOnBookClosed = null;
            FBookContext = null;
            if (Done != null)
            {
                Done(Context);
            }

            if (Context != null)
            {
                Context.Release();
            }

            Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
        }

        // AoWWorldMap.pas:146
        public void ResetCampaign()
        {
            var call__11 = GetCampaign();
            call__11.FState = 0;
            var call__12 = GetCampaign();
            call__12.FPage = 0;
            InfoPnl.Visible = false;
            var call__13 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ShowInfoRStr);
            ShowBtn.Text = call__13;
        }

        // AoWWorldMap.pas:151
        public void ShowCampaignHistory()
        {
            Aow.Units.AoWVideoDlg.TWMCampaignScenario Scenario = default;
            var call__14 = GetCampaign();
            call__14.FState = 10;
            var call__15 = GetCampaign();
            call__15.FPage = 0;
            var call__16 = GetCampaign();
            var call__17 = GetCampaign();
            var call__18 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__17, call__16.FCurrentScenarioID);
            Scenario = call__18;
            while (true)
            {
                var call__19 = Scenario.FIntroPages.Count;
                var call__20 = GetCampaign();
                bool bool__21 = (call__19 > (call__20.FPage + 2));
                if (bool__21)
                {
                    var call__22 = GetCampaign();
                    var call__23 = Aow.Units.AoWWorldMap.AoWWorldMap.GetCampaignPage(Scenario.FIntroPages, call__22.FPage + 2);
                    bool bool__24 = (call__23.FImageLibrary != ((AnsiString)""));
                    if (!bool__24)
                    {
                        var call__25 = GetCampaign();
                        var call__26 = Aow.Units.AoWWorldMap.AoWWorldMap.GetCampaignPage(Scenario.FIntroPages, call__25.FPage + 2);
                        var call__27 = call__26.FText.Count;
                        bool__24 = (call__27 > 0);
                    }

                    bool__21 = bool__24;
                }

                if (!bool__21)
                    break;
                {
                    var call__28 = GetCampaign();
                    call__28.FPage += 2;
                }
            }

            var call__29 = GetCampaign();
            ShowIntroPages(call__29.FCurrentScenarioID);
        }

        // AoWWorldMap.pas:442
        public void BackClick()
        {
            Back.CaptureInput = false;
            Next.CaptureInput = false;
            {
                var w__30 = BookStart;
                w__30.CaptureInput = false;
                w__30.Visible = false;
            }

            {
                var w__31 = BookWin;
                w__31.FDrawn = false;
                if (w__31.FBackSurface != null)
                {
                    w__31.FBackSurface.Cls(w__31.FMaskColor);
                }
            }

            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
            FBookAction = 1;
        }

        // AoWWorldMap.pas:863
        public void WorldMapCreate()
        {
            Aow.Units.AoWVideoDlg.AoWVideoDlg.RegisterWorldMapCampaign(Aow.Units.AoWE.AoWE.AoWEngine);
        }

        // AoWWorldMap.pas:461
        public void NextStartClick()
        {
            Back.CaptureInput = false;
            Next.CaptureInput = false;
            BookStart.CaptureInput = false;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
            FBookAction = 2;
        }

        // AoWWorldMap.pas:471
        public void WorldPnlUpdate()
        {
            int Frame = default;
            int StartIndex = default;
            int EndIndex = default;
            if (WorldPnl.Visible && !BookWin.Visible)
            {
                Path1.FDrawn = false;
                Path2.FDrawn = false;
                Path3.FDrawn = false;
                PPawn.FDrawn = false;
                EPawn.FDrawn = false;
                FPathDelay -= 1;
                if (FPathDelay < 1)
                {
                    FPathDelay = 5;
                    Frame = 1 - FPathFrame;
                    FPathFrame = Frame;
                    var call__32 = GetCampaign();
                    Path1.CurrentImage = (call__32.FFaction * 3) + (Frame + 2);
                    var call__33 = GetCampaign();
                    Path2.CurrentImage = (call__33.FFaction * 3) + (FPathFrame + 2);
                    var call__34 = GetCampaign();
                    Path3.CurrentImage = (call__34.FFaction * 3) + (FPathFrame + 2);
                }

                var call__35 = GetCampaign();
                if (call__35.FState == 5)
                {
                    if (FPawnTicks > 0)
                    {
                        var call__36 = GetCampaign();
                        var call__37 = GetCampaign();
                        var call__38 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__37, call__36.FNextScenarioID);
                        var call__39 = GetCampaign();
                        var call__40 = GetCampaign();
                        var call__41 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__40, call__39.FCurrentScenarioID);
                        var call__42 = GetCampaign();
                        var call__43 = GetCampaign();
                        var call__44 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__43, call__42.FCurrentScenarioID);
                        PPawn.WinLeft = ((((call__38.FMapX - call__41.FMapX) * (20 - FPawnTicks)) / 19) + call__44.FMapX) - 25;
                        var call__45 = GetCampaign();
                        var call__46 = GetCampaign();
                        var call__47 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__46, call__45.FNextScenarioID);
                        var call__48 = GetCampaign();
                        var call__49 = GetCampaign();
                        var call__50 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__49, call__48.FCurrentScenarioID);
                        var call__51 = GetCampaign();
                        var call__52 = GetCampaign();
                        var call__53 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__52, call__51.FCurrentScenarioID);
                        PPawn.WinTop = ((((call__47.FMapY - call__50.FMapY) * (20 - FPawnTicks)) / 19) + call__53.FMapY) - 65;
                        {
                            var w__54 = PPawn;
                            if (w__54.WinTop > EPawn.WinTop)
                            {
                                w__54.Priority = 10;
                                EPawn.Priority = 5;
                            }
                            else
                            {
                                w__54.Priority = 5;
                                EPawn.Priority = 10;
                            }
                        }
                    }
                    else
                    {
                        FPawnTicks = 20;
                        FEnemyDelay = 30;
                        var call__55 = GetCampaign();
                        call__55.FState += 1;
                        var call__56 = GetCampaign();
                        var call__57 = GetCampaign();
                        var call__58 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__57, call__56.FNextScenarioID);
                        if (DescLbl.Text != call__58.FDescription)
                        {
                            var call__59 = GetCampaign();
                            var call__60 = GetCampaign();
                            var call__61 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__60, call__59.FNextScenarioID);
                            DescLbl.Text = call__61.FDescription;
                            {
                                var w__62 = DescLbl;
                                w__62.WinTop = 460 - w__62.WinHeight;
                            }

                            FDescriptionHandled = true;
                        }
                    }

                    FPawnTicks -= 1;
                }

                var call__63 = GetCampaign();
                if (call__63.FState == 6)
                {
                    if (FPawnTicks > 0)
                    {
                        FDescriptionHandled = true;
                        var call__64 = GetCampaign();
                        var call__65 = GetCampaign();
                        var call__66 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__65, call__64.FCurrentScenarioID);
                        var call__67 = call__66.FPaths[3];
                        var call__68 = GetCampaign();
                        var call__69 = call__68.FScenarios.IndexOfID(call__67);
                        StartIndex = call__69;
                        var call__70 = GetCampaign();
                        var call__71 = GetCampaign();
                        var call__72 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__71, call__70.FNextScenarioID);
                        var call__73 = call__72.FPaths[3];
                        var call__74 = GetCampaign();
                        var call__75 = call__74.FScenarios.IndexOfID(call__73);
                        EndIndex = call__75;
                        if ((StartIndex != -1) && (EndIndex != -1))
                        {
                            var call__76 = GetCampaign();
                            var call__77 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__76, EndIndex);
                            var call__78 = GetCampaign();
                            var call__79 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__78, StartIndex);
                            var call__80 = GetCampaign();
                            var call__81 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__80, StartIndex);
                            EPawn.WinLeft = ((((call__77.FMapX - call__79.FMapX) * (20 - FPawnTicks)) / 19) + call__81.FMapX) - 25;
                            var call__82 = GetCampaign();
                            var call__83 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__82, EndIndex);
                            var call__84 = GetCampaign();
                            var call__85 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__84, StartIndex);
                            var call__86 = GetCampaign();
                            var call__87 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__86, StartIndex);
                            EPawn.WinTop = ((((call__83.FMapY - call__85.FMapY) * (20 - FPawnTicks)) / 19) + call__87.FMapY) - 65;
                            {
                                var w__88 = PPawn;
                                if (w__88.WinTop > EPawn.WinTop)
                                {
                                    w__88.Priority = 10;
                                    EPawn.Priority = 5;
                                }
                                else
                                {
                                    w__88.Priority = 5;
                                    EPawn.Priority = 10;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (FEnemyDelay > 0)
                        {
                            FEnemyDelay -= 1;
                        }
                        else
                        {
                            var call__89 = GetCampaign();
                            call__89.FState += 1;
                            UpdateCampaignState();
                        }
                    }

                    FPawnTicks -= 1;
                }
            }
        }

        // AoWWorldMap.pas:566
        public void Path1MouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__90 = GetCampaign();
                if (call__90.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__91 = GetCampaign();
                    Aow.Units.E2CampaignSupport.E2CampaignSupport.SelectCampaignPath(call__91, 0);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:578
        public void Path2MouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__92 = GetCampaign();
                if (call__92.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__93 = GetCampaign();
                    Aow.Units.E2CampaignSupport.E2CampaignSupport.SelectCampaignPath(call__93, 1);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:590
        public void Path3MouseDown(Aow.Units.AoWComp.TAoWComponent Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__94 = GetCampaign();
                if (call__94.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__95 = GetCampaign();
                    Aow.Units.E2CampaignSupport.E2CampaignSupport.SelectCampaignPath(call__95, 2);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:602
        public void CancelClick()
        {
            var call__96 = GetCampaign();
            call__96.ResetNextScenario();
            var call__97 = GetCampaign();
            call__97.FState = 1;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            UpdateCampaignState();
        }

        // AoWWorldMap.pas:611
        public void PPawnMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Rtl.TObject Control = default;
            Control = Sender;
            if ((X > 20) && (Control == PPawn))
            {
                var call__98 = GetCampaign();
                if (call__98.FState == 1)
                {
                    var call__99 = GetCampaign();
                    call__99.FState = 0;
                    var call__100 = GetCampaign();
                    call__100.FPage = 0;
                    UpdateCampaignState();
                }
            }

            // { Original hit test uses X. }
            ((Aow.Units.AoWComp.TAoWComponent)Control).FHandled = false;
        }

        // AoWWorldMap.pas:624
        public void PPawnMouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            Aow.Rtl.AnsiString Description = default;
            Aow.Units.AoWComp.TAoWComponent Pawn = default;
            Pawn = Sender;
            // { Original hit test uses X. }
            if (X > 20)
            {
                var call__101 = GetCampaign();
                if (call__101.FState == 1)
                {
                    if (Pawn == PPawn)
                    {
                        Description = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ViewSummaryRStr);
                    }
                    else
                    {
                        var call__102 = GetCampaign();
                        if (call__102.FFaction == 0)
                        {
                            Description = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.CultOfStormsRStr);
                        }
                        else
                        {
                            Description = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.KeepersRStr);
                        }
                    }
                }
            }

            if ((DescLbl.Text != Description) && (Description != ((AnsiString)"")))
            {
                DescLbl.Text = Description;
                {
                    var w__103 = DescLbl;
                    w__103.WinTop = 460 - w__103.WinHeight;
                }
            }

            FDescriptionHandled = true;
            Pawn.FHandled = false;
        }

        // AoWWorldMap.pas:643
        public void Path1MouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__104 = GetCampaign();
            if (call__104.FState == 1)
            {
                var call__105 = GetCampaign();
                var call__106 = GetCampaign();
                var call__107 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__106, call__105.FCurrentScenarioID);
                var call__108 = call__107.FPaths[0];
                var call__109 = GetCampaign();
                var call__110 = call__109.FScenarios.IndexOfID(call__108);
                Index = call__110;
                var call__111 = GetCampaign();
                var call__112 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__111, Index);
                if (DescLbl.Text != call__112.FPathDescription)
                {
                    var call__113 = GetCampaign();
                    var call__114 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__113, Index);
                    DescLbl.Text = call__114.FPathDescription;
                    {
                        var w__115 = DescLbl;
                        w__115.WinTop = 460 - w__115.WinHeight;
                    }
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:661
        public void Path2MouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__116 = GetCampaign();
            if (call__116.FState == 1)
            {
                var call__117 = GetCampaign();
                var call__118 = GetCampaign();
                var call__119 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__118, call__117.FCurrentScenarioID);
                var call__120 = call__119.FPaths[1];
                var call__121 = GetCampaign();
                var call__122 = call__121.FScenarios.IndexOfID(call__120);
                Index = call__122;
                var call__123 = GetCampaign();
                var call__124 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__123, Index);
                if (DescLbl.Text != call__124.FPathDescription)
                {
                    var call__125 = GetCampaign();
                    var call__126 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__125, Index);
                    DescLbl.Text = call__126.FPathDescription;
                    {
                        var w__127 = DescLbl;
                        w__127.WinTop = 460 - w__127.WinHeight;
                    }
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:679
        public void Path3MouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__128 = GetCampaign();
            if (call__128.FState == 1)
            {
                var call__129 = GetCampaign();
                var call__130 = GetCampaign();
                var call__131 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__130, call__129.FCurrentScenarioID);
                var call__132 = call__131.FPaths[2];
                var call__133 = GetCampaign();
                var call__134 = call__133.FScenarios.IndexOfID(call__132);
                Index = call__134;
                var call__135 = GetCampaign();
                var call__136 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__135, Index);
                if (DescLbl.Text != call__136.FPathDescription)
                {
                    var call__137 = GetCampaign();
                    var call__138 = Aow.Units.E2CampaignSupport.E2CampaignSupport.GetCampaignScenario(call__137, Index);
                    DescLbl.Text = call__138.FPathDescription;
                    {
                        var w__139 = DescLbl;
                        w__139.WinTop = 460 - w__139.WinHeight;
                    }
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:697
        public void WorldPnlMouseMove(Aow.Units.AoWComp.TAoWComponent Sender, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            if (!FDescriptionHandled)
            {
                var call__140 = GetCampaign();
                if (call__140.FState == 1)
                {
                    {
                        var w__141 = DescLbl;
                        if (w__141.Text != ((AnsiString)""))
                        {
                            w__141.Text = (AnsiString)"";
                        }
                    }
                }
            }

            FDescriptionHandled = false;
        }

        // AoWWorldMap.pas:706
        public void BookStartClick()
        {
            BookWin.Visible = false;
            Aow.Units.Sound.Sound.PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            if (!WorldPnl.Visible)
            {
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.OverlayPriority = 0;
            }

            if (WorldPnl.Visible)
            {
                FPawnTicks = 20;
                var call__142 = GetCampaign();
                call__142.FState += 1;
                UpdateCampaignState();
            }
        }

        // AoWWorldMap.pas:720
        public void BookWinAnimDone()
        {
            Back.CaptureInput = true;
            Next.CaptureInput = true;
            BookStart.CaptureInput = true;
            BookWin.FHidden = true;
        }

        // AoWWorldMap.pas:729
        public void BookWinShow()
        {
            FBookAction = 0;
        }

        // AoWWorldMap.pas:733
        public void BookWinUpdate()
        {
            Aow.Rtl.TRect R = default;
            switch (FBookAction)
            {
                case 3:
                    BookWin.StartAnim((Aow.Units.PageTurnCtrl.TPageTurnMode)1);
                    var call__143 = GetCampaign();
                    call__143.FPage -= 2;
                    UpdateCampaignState();
                    FBookAction = 0;
                    break;
                case 4:
                    BookWin.StartAnim((Aow.Units.PageTurnCtrl.TPageTurnMode)0);
                    var call__144 = GetCampaign();
                    call__144.FPage += 2;
                    UpdateCampaignState();
                    FBookAction = 0;
                    break;
                case 1:
                    FBookAction = 3;
                    break;
                case 2:
                    FBookAction = 4;
                    break;
            }

            if (BookStart.Visible != Aow.Units.AoWWorldMap.AoWWorldMap.BookStartVisible)
            {
                var call__145 = Aow.Rtl.Rtl.Rect(BookWin.WinLeft + BookStart.WinLeft, BookWin.WinTop + BookStart.WinTop, (BookWin.WinLeft + BookStart.WinLeft) + BookStart.WinWidth, (BookWin.WinTop + BookStart.WinTop) + BookStart.WinHeight);
                R = call__145;
                Aow.Units.AoWWinMan.AoWWinMan.General.Manager.AddDrawRect(ref R, BookWin);
                Aow.Units.AoWWorldMap.AoWWorldMap.BookStartVisible = BookStart.Visible;
            }
        }

        // AoWWorldMap.pas:764
        public void ShowBtnClick()
        {
            if (InfoPnl.Visible)
            {
                UpdateCampaignState();
                SelectLbl.Visible = true;
                InfoPnl.Visible = false;
                var call__146 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ShowInfoRStr);
                ShowBtn.Text = call__146;
            }
            else
            {
                SelectLbl.Visible = false;
                PPawn.Visible = false;
                EPawn.Visible = false;
                Path1.Visible = false;
                Path2.Visible = false;
                Path3.Visible = false;
                InfoPnl.Visible = true;
                var call__147 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.HideInfoRStr);
                ShowBtn.Text = call__147;
            }
        }

        // AoWWorldMap.pas:786
        public void QuitBtnClick()
        {
            var call__148 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.QuitCampaignRStr);
            var call__149 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.E2Globals.E2Globals.ConfirmRStr);
            Aow.Units.E2Globals.E2Globals.DialogWin.Confirm(call__149, call__148, 5);
        }

        // AoWWorldMap.pas:791
        public void StarImgUpdate()
        {
            FStarIntensity += FStarStep;
            if ((FStarIntensity + FStarStep) > 100)
            {
                FStarStep = -1;
            }

            if ((FStarIntensity + FStarStep) < 50)
            {
                FStarStep = 1;
            }

            StarImg.FDrawn = false;
        }

        // AoWWorldMap.pas:800
        public void StarImgDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            var call__150 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap[FStarImage];
            call__150.ShowBlended(WorldPnl.WinLeft + StarImg.WinLeft, WorldPnl.WinTop + StarImg.WinTop, Surface, Aow.Units.GFXE.GFXE.IntensityBlendTable.LookupTables[FStarIntensity]);
        }

        // AoWWorldMap.pas:806
        public void Ally1PnlPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            var call__151 = Aow.Rtl.Rtl.Rect(InfoPnl.WinLeft + Ally1Pnl.WinLeft, InfoPnl.WinTop + Ally1Pnl.WinTop, (InfoPnl.WinLeft + Ally1Pnl.WinLeft) + Ally1Pnl.WinWidth, (InfoPnl.WinTop + Ally1Pnl.WinTop) + Ally1Pnl.WinHeight);
            R = call__151;
            var call__152 = GetCampaign();
            if (call__152.FRaceHistory.Count > 0)
            {
                var call__153 = GetCampaign();
                var call__154 = call__153.FRaceHistory[0];
                var call__155 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(call__154);
                var call__156 = call__155.GetFace();
                call__156.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:818
        public void Ally2PnlPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            var call__157 = Aow.Rtl.Rtl.Rect(InfoPnl.WinLeft + Ally2Pnl.WinLeft, InfoPnl.WinTop + Ally2Pnl.WinTop, (InfoPnl.WinLeft + Ally2Pnl.WinLeft) + Ally2Pnl.WinWidth, (InfoPnl.WinTop + Ally2Pnl.WinTop) + Ally2Pnl.WinHeight);
            R = call__157;
            var call__158 = GetCampaign();
            if (call__158.FRaceHistory.Count > 1)
            {
                var call__159 = GetCampaign();
                var call__160 = call__159.FRaceHistory[1];
                var call__161 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(call__160);
                var call__162 = call__161.GetFace();
                call__162.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:830
        public void Ally3PnlPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            var call__163 = Aow.Rtl.Rtl.Rect(InfoPnl.WinLeft + Ally3Pnl.WinLeft, InfoPnl.WinTop + Ally3Pnl.WinTop, (InfoPnl.WinLeft + Ally3Pnl.WinLeft) + Ally3Pnl.WinWidth, (InfoPnl.WinTop + Ally3Pnl.WinTop) + Ally3Pnl.WinHeight);
            R = call__163;
            var call__164 = GetCampaign();
            if (call__164.FRaceHistory.Count > 2)
            {
                var call__165 = GetCampaign();
                var call__166 = call__165.FRaceHistory[2];
                var call__167 = Aow.Units.AoWE.AoWE.AoWHSSet.FRaceResources.GetRaceResource(call__166);
                var call__168 = call__167.GetFace();
                call__168.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:842
        public void HeroPnlPreDraw(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc Surface)
        {
            Aow.Rtl.TRect R = default;
            var call__169 = Aow.Rtl.Rtl.Rect(InfoPnl.WinLeft + HeroPnl.WinLeft, InfoPnl.WinTop + HeroPnl.WinTop, (InfoPnl.WinLeft + HeroPnl.WinLeft) + HeroPnl.WinWidth, (InfoPnl.WinTop + HeroPnl.WinTop) + HeroPnl.WinHeight);
            R = call__169;
            var call__170 = GetCampaign();
            if (call__170.FTransferSettings.FLeader != null)
            {
                var call__171 = GetCampaign();
                call__171.FTransferSettings.FLeader.ShowFace(R, Surface);
            }
        }

        // AoWWorldMap.pas:854
        public void SetBtnClick()
        {
            Aow.Units.E2Globals.E2Globals.CampaignSettingsDlg.Dlg.Visible = true;
        }

        // AoWWorldMap.pas:859
        public void BookWinHide()
        {
            BookClosed();
        }
    }

    // { TFileTime (instance size 12, VMT 0041EB14) }
    public partial class TFileTime : Aow.Rtl.TObject
    {
        public double FTime;
        // System.pas:111
        public static Aow.Units.AoWWorldMap.TFileTime Create()
        {
            return new Aow.Units.AoWWorldMap.TFileTime();
        }
    }

    public static partial class AoWWorldMap
    {
        // AoWWorldMap.pas:147
        // { $04; 0041F71B }
        public static Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage GetCampaignPage(Aow.Units.AoWVideoDlg.TWMCampaignScenarioPageList Pages, int Index)
        {
            Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage Result = default;
            Result = (Aow.Units.AoWVideoDlg.TWMCampaignScenarioPage)Pages.FChildren.List[Index];
            return Result;
        }

        public static Aow.Units.AoWTargetSelection.TTargetSelectionDlg TargetSelectionDlg;
        public static Aow.Units.AoWWorldMap.TWorldMap WorldMap;
        // { $45B0F8 }
        public static bool BookStartVisible;
    }
}
