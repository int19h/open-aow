// Translated from AoWWorldMap.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWComp;
using Aow.Units.AoWBookWindow;
using Aow.Units.AoWButton;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWTargetSelection;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.AoWWinMan.AoWWinMan;
using static Aow.Units.E2CampaignSupport.E2CampaignSupport;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E2GameFlow.E2GameFlow;
using static Aow.Units.AoWLeaderSetup.AoWLeaderSetup;

namespace Aow.Units.AoWWorldMap
{
    // star image, brightness, path animation, pawn timing, hover, context, completion event, book
    // action
    public partial class TWorldMap : TDataModule
    {
        public TWorldMap(TComponent owner) : base(owner)
        { }

        public TAOWMemo MemoL;
        public TAOWImage PicR;
        public TAOWPanel WorldPnl;
        public TAOWImage Path1;
        public TAOWImage Path2;
        public TAOWImage Path3;
        public TAOWImage PPawn;
        public TAOWImage EPawn;
        public TAOWLabel DescLbl;
        public TAOWButton Back;
        public TAOWButton Next;
        public TAOWButton Cancel;
        public TAOWButton BookStart;
        public TAOWLabel LabelL;
        public TAOWMemo MemoR;
        public TAOWLabel LabelR;
        public TAOWImage PicL;
        public TAoWBookWindow BookWin;
        public TAOWPanel InfoPnl;
        public TAOWLabel Elves;
        public TAOWLabel Halflings;
        public TAOWLabel Goblins;
        public TAOWLabel Orcs;
        public TAOWLabel Dwarves;
        public TAOWLabel Undead;
        public TAOWLabel Highmen;
        public TAOWLabel Lizardmen;
        public TAOWLabel DarkElves;
        public TAOWLabel VoW;
        public TAOWLabel ElvenC;
        public TAOWLabel IoLG;
        public TAOWLabel Deepmir;
        public TAOWLabel Aldor;
        public TAOWLabel Serpent;
        public TAOWLabel Blackwater;
        public TAOWLabel Skull;
        public TAOWLabel Sylvanus;
        public TAOWLabel Pass;
        public TAOWLabel Arcalot;
        public TAOWLabel Flowershire;
        public TAOWLabel Sobek;
        public TAOWLabel Steppe;
        public TAOWLabel Bronnam;
        public TAOWLabel UC;
        public TAOWLabel Woods;
        public TAOWLabel Burbatak;
        public TAOWButton QuitBtn;
        public TAOWButton ShowBtn;
        public TAOWLabel SelectLbl;
        public TAOWLabel AlliesLnl;
        public TAOWPanel Frame;
        public TAOWLabel Azrac;
        public TAOWImage StarImg;
        public TAOWLabel Terstar;
        public TAoWFrame HeroPnl;
        public TAoWFrame Ally1Pnl;
        public TAoWFrame Ally2Pnl;
        public TAoWFrame Ally3Pnl;
        public TAOWButton SetBtn;
        public TIvTranslator Translator;
        public TAOWVScrollBar leftSB;
        public TAOWVScrollBar rightSB;
        public int FStarImage;
        public int FStarStep;
        public int FStarIntensity;
        public int FPathFrame;
        public int FPathDelay;
        public int FPawnTicks;
        public int FEnemyDelay;
        public bool FDescriptionHandled;
        public TEObject FBookContext;
        public TNotifyEvent FOnBookClosed;
        public byte FBookAction;
        // AoWWorldMap.pas:152
        public TWMCampaign GetCampaign()
        {
            TWMCampaign Result = default;
            Result = (TWMCampaign)AoWEngine.FCampaign;
            return Result;
        }

        // AoWWorldMap.pas:157
        public void UpdateCampaignState()
        {
            int Index = default;
            TAoWHSMap Preview = default;
            if (GetCampaign() != null)
            {
                var call__1 = GetCampaign();
                switch (call__1.FState)
                {
                    case 0:
                        var call__2 = GetCampaign();
                        var call__3 = GetCampaign();
                        var call__4 = GetCampaignScenario(call__3, call__2.FCurrentScenarioID);
                        var call__5 = AoWWorldMap.GetCampaignPage(call__4.FOutroPages, 0);
                        bool bool__6 = call__5.FImageLibrary != (AnsiString)"";
                        if (!bool__6)
                        {
                            var call__7 = GetCampaign();
                            var call__8 = GetCampaign();
                            var call__9 = GetCampaignScenario(call__8, call__7.FCurrentScenarioID);
                            var call__10 = AoWWorldMap.GetCampaignPage(call__9.FOutroPages, 0);
                            var call__11 = call__10.FText.Count;
                            bool__6 = call__11 > 0;
                        }

                        if (bool__6)
                        {
                            SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"\\Int\\Scenes");
                            var call__12 = GetCampaign();
                            var call__13 = GetCampaign();
                            var call__14 = GetCampaignScenario(call__13, call__12.FCurrentScenarioID);
                            var call__15 = GetCampaign();
                            var call__16 = AoWWorldMap.GetCampaignPage(call__14.FOutroPages, call__15.FPage);
                            LabelL.Text = call__16.FHeading;
                            var call__17 = GetCampaign();
                            var call__18 = GetCampaign();
                            var call__19 = GetCampaignScenario(call__18, call__17.FCurrentScenarioID);
                            var call__20 = GetCampaign();
                            var call__21 = AoWWorldMap.GetCampaignPage(call__19.FOutroPages, call__20.FPage + 1);
                            LabelR.Text = call__21.FHeading;
                            var call__22 = GetCampaign();
                            var call__23 = GetCampaign();
                            var call__24 = GetCampaignScenario(call__23, call__22.FCurrentScenarioID);
                            var call__25 = GetCampaign();
                            var call__26 = AoWWorldMap.GetCampaignPage(call__24.FOutroPages, call__25.FPage);
                            MemoL.Items.Assign(call__26.FText);
                            var call__27 = GetCampaign();
                            var call__28 = GetCampaign();
                            var call__29 = GetCampaignScenario(call__28, call__27.FCurrentScenarioID);
                            var call__30 = GetCampaign();
                            var call__31 = AoWWorldMap.GetCampaignPage(call__29.FOutroPages, call__30.FPage + 1);
                            MemoR.Items.Assign(call__31.FText);
                            FStarIntensity = 50;
                            FStarStep = 1;
                            var call__32 = GetCampaign();
                            var call__33 = GetCampaign();
                            var call__34 = GetCampaignScenario(call__33, call__32.FCurrentScenarioID);
                            switch (call__34.FMapKind)
                            {
                                case 0:
                                    WorldPnl.ILIndexTexture = 0;
                                    FStarImage = 13;
                                    break;
                                case 1:
                                    WorldPnl.ILIndexTexture = 10;
                                    FStarImage = 14;
                                    break;
                                case 2:
                                    WorldPnl.ILIndexTexture = 11;
                                    FStarImage = 15;
                                    break;
                            }

                            var call__35 = GetCampaign();
                            var call__36 = GetCampaign();
                            var call__37 = GetCampaignScenario(call__36, call__35.FCurrentScenarioID);
                            var call__38 = GetCampaign();
                            var call__39 = AoWWorldMap.GetCampaignPage(call__37.FOutroPages, call__38.FPage);
                            if (call__39.FImageLibrary == (AnsiString)"")
                            {
                                PicL.ImageLib = null;
                            }
                            else
                            {
                                var call__40 = GetCampaign();
                                var call__41 = GetCampaign();
                                var call__42 = GetCampaignScenario(call__41, call__40.FCurrentScenarioID);
                                var call__43 = GetCampaign();
                                var call__44 = AoWWorldMap.GetCampaignPage(call__42.FOutroPages, call__43.FPage);
                                aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPL.LoadFromFile(call__44.FImageLibrary);
                                PicL.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPL;
                                var call__45 = GetCampaign();
                                var call__46 = GetCampaign();
                                var call__47 = GetCampaignScenario(call__46, call__45.FCurrentScenarioID);
                                var call__48 = GetCampaign();
                                var call__49 = AoWWorldMap.GetCampaignPage(call__47.FOutroPages, call__48.FPage);
                                PicL.CurrentImage = call__49.FImageIndex;
                                PicL.WinWidth = 0;
                                PicL.WinHeight = 0;
                                PicL.Visible = true;
                            }

                            var call__50 = GetCampaign();
                            var call__51 = GetCampaign();
                            var call__52 = GetCampaignScenario(call__51, call__50.FCurrentScenarioID);
                            var call__53 = GetCampaign();
                            var call__54 = AoWWorldMap.GetCampaignPage(call__52.FOutroPages, call__53.FPage + 1);
                            if (call__54.FImageLibrary == (AnsiString)"")
                            {
                                PicR.ImageLib = null;
                            }
                            else
                            {
                                var call__55 = GetCampaign();
                                var call__56 = GetCampaign();
                                var call__57 = GetCampaignScenario(call__56, call__55.FCurrentScenarioID);
                                var call__58 = GetCampaign();
                                var call__59 = AoWWorldMap.GetCampaignPage(call__57.FOutroPages, call__58.FPage + 1);
                                aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPR.LoadFromFile(call__59.FImageLibrary);
                                PicR.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPR;
                                var call__60 = GetCampaign();
                                var call__61 = GetCampaign();
                                var call__62 = GetCampaignScenario(call__61, call__60.FCurrentScenarioID);
                                var call__63 = GetCampaign();
                                var call__64 = AoWWorldMap.GetCampaignPage(call__62.FOutroPages, call__63.FPage + 1);
                                PicR.CurrentImage = call__64.FImageIndex;
                                PicR.WinWidth = 0;
                                PicR.WinHeight = 0;
                                PicR.Visible = true;
                            }

                            var call__65 = GetCampaign();
                            Back.Visible = call__65.FPage > 0;
                            var call__66 = GetCampaign();
                            if (call__66.FPage == 8)
                            {
                                Next.Visible = false;
                            }
                            else
                            {
                                var call__67 = GetCampaign();
                                var call__68 = GetCampaign();
                                var call__69 = GetCampaignScenario(call__68, call__67.FCurrentScenarioID);
                                var call__70 = GetCampaign();
                                var call__71 = AoWWorldMap.GetCampaignPage(call__69.FOutroPages, call__70.FPage + 2);
                                bool bool__72 = call__71.FImageLibrary != (AnsiString)"";
                                if (!bool__72)
                                {
                                    var call__73 = GetCampaign();
                                    var call__74 = GetCampaign();
                                    var call__75 = GetCampaignScenario(call__74, call__73.FCurrentScenarioID);
                                    var call__76 = GetCampaign();
                                    var call__77 = AoWWorldMap.GetCampaignPage(call__75.FOutroPages, call__76.FPage + 2);
                                    var call__78 = call__77.FText.Count;
                                    bool__72 = call__78 > 0;
                                }

                                Next.Visible = bool__72;
                            }

                            BookStart.Visible = !Next.Visible;
                            Cancel.Visible = false;
                            var aoWBookWindow = BookWin;
                            aoWBookWindow.FHidden = true;
                            aoWBookWindow.Visible = true;

                            SetBtn.Visible = false;
                            ShowBtn.Visible = false;
                            QuitBtn.Visible = false;
                            if (BookWin.FBackSurface != null)
                            {
                                BookWin.FBackSurface.Cls(BookWin.FMaskColor);
                            }

                            if (TitleWindow.TitleWin.Visible)
                            {
                                General.Manager.OverlayPriority = 249;
                            }
                        }
                        else
                        {
                            var call__80 = GetCampaign();
                            call__80.FState += 1;
                            UpdateCampaignState();
                        }

                        break;
                    case 1:
                        SetBtn.Visible = true;
                        ShowBtn.Visible = true;
                        QuitBtn.Visible = true;
                        BookWin.Visible = false;
                        PPawn.Visible = true;
                        EPawn.Visible = true;
                        var call__81 = GetCampaign();
                        var call__82 = GetCampaign();
                        var call__83 = GetCampaignScenario(call__82, call__81.FCurrentScenarioID);
                        PPawn.WinLeft = call__83.FMapX - 25;
                        var call__84 = GetCampaign();
                        var call__85 = GetCampaign();
                        var call__86 = GetCampaignScenario(call__85, call__84.FCurrentScenarioID);
                        PPawn.WinTop = call__86.FMapY - 65;
                        var call__87 = GetCampaign();
                        var call__88 = GetCampaign();
                        var call__89 = GetCampaignScenario(call__88, call__87.FCurrentScenarioID);
                        var call__90 = call__89.FPaths[3];
                        var call__91 = GetCampaign();
                        var call__92 = call__91.FScenarios.IndexOfID(call__90);
                        Index = call__92;
                        if (Index != -1)
                        {
                            var call__93 = GetCampaign();
                            var call__94 = GetCampaignScenario(call__93, Index);
                            EPawn.WinLeft = call__94.FMapX - 25;
                            var call__95 = GetCampaign();
                            var call__96 = GetCampaignScenario(call__95, Index);
                            EPawn.WinTop = call__96.FMapY - 65;
                        }

                    {
                        if (this.PPawn.WinTop > EPawn.WinTop)
                        {
                            this.PPawn.Priority = 10;
                            EPawn.Priority = 5;
                        }
                        else
                        {
                            this.PPawn.Priority = 5;
                            EPawn.Priority = 10;
                        }
                    }

                        var call__98 = GetCampaign();
                        var call__99 = GetCampaign();
                        var call__100 = GetCampaignScenario(call__99, call__98.FCurrentScenarioID);
                        var call__101 = call__100.FPaths[0];
                        if (call__101 > -1)
                        {
                            Path1.Visible = true;
                            var call__102 = GetCampaign();
                            Path1.CurrentImage = call__102.FFaction * 3 + (FPathFrame + 2);
                            var call__103 = GetCampaign();
                            var call__104 = GetCampaign();
                            var call__105 = GetCampaignScenario(call__104, call__103.FCurrentScenarioID);
                            var call__106 = call__105.FPaths[0];
                            var call__107 = GetCampaign();
                            var call__108 = call__107.FScenarios.IndexOfID(call__106);
                            Index = call__108;
                            var call__109 = GetCampaign();
                            var call__110 = GetCampaignScenario(call__109, Index);
                            Path1.WinLeft = call__110.FMapX;
                            var call__111 = GetCampaign();
                            var call__112 = GetCampaignScenario(call__111, Index);
                            Path1.WinTop = call__112.FMapY;
                        }
                        else
                        {
                            Path1.Visible = false;
                        }

                        var call__113 = GetCampaign();
                        var call__114 = GetCampaign();
                        var call__115 = GetCampaignScenario(call__114, call__113.FCurrentScenarioID);
                        var call__116 = call__115.FPaths[1];
                        if (call__116 > -1)
                        {
                            Path2.Visible = true;
                            var call__117 = GetCampaign();
                            Path2.CurrentImage = call__117.FFaction * 3 + (FPathFrame + 2);
                            var call__118 = GetCampaign();
                            var call__119 = GetCampaign();
                            var call__120 = GetCampaignScenario(call__119, call__118.FCurrentScenarioID);
                            var call__121 = call__120.FPaths[1];
                            var call__122 = GetCampaign();
                            var call__123 = call__122.FScenarios.IndexOfID(call__121);
                            Index = call__123;
                            var call__124 = GetCampaign();
                            var call__125 = GetCampaignScenario(call__124, Index);
                            Path2.WinLeft = call__125.FMapX;
                            var call__126 = GetCampaign();
                            var call__127 = GetCampaignScenario(call__126, Index);
                            Path2.WinTop = call__127.FMapY;
                        }
                        else
                        {
                            Path2.Visible = false;
                        }

                        var call__128 = GetCampaign();
                        var call__129 = GetCampaign();
                        var call__130 = GetCampaignScenario(call__129, call__128.FCurrentScenarioID);
                        var call__131 = call__130.FPaths[2];
                        if (call__131 > -1)
                        {
                            Path3.Visible = true;
                            var call__132 = GetCampaign();
                            Path3.CurrentImage = call__132.FFaction * 3 + (FPathFrame + 2);
                            var call__133 = GetCampaign();
                            var call__134 = GetCampaign();
                            var call__135 = GetCampaignScenario(call__134, call__133.FCurrentScenarioID);
                            var call__136 = call__135.FPaths[2];
                            var call__137 = GetCampaign();
                            var call__138 = call__137.FScenarios.IndexOfID(call__136);
                            Index = call__138;
                            var call__139 = GetCampaign();
                            var call__140 = GetCampaignScenario(call__139, Index);
                            Path3.WinLeft = call__140.FMapX;
                            var call__141 = GetCampaign();
                            var call__142 = GetCampaignScenario(call__141, Index);
                            Path3.WinTop = call__142.FMapY;
                        }
                        else
                        {
                            Path3.Visible = false;
                        }

                        if (!Path1.Visible && !Path2.Visible && !Path3.Visible)
                        {
                            TitleWindow.TitlePnl.Visible = false;
                            TitleWindow.TitlePnl.FRedraw = false;
                            TitleWindow.TitleWin.Visible = false;
                            TBWindow.TaskWindow.Visible = false;
                            WorldPnl.Visible = false;
                            BookWin.Visible = false;
                            AoWEngine.CloseCampaign();
                            TitleScene.ResetScene();
                            ShowTitleScene(TitleScene, true);
                        }

                        break;
                    case 2:
                        var call__143 = GetCampaign();
                        var call__144 = GetCampaign();
                        var call__145 = GetCampaignScenario(call__144, call__143.FNextScenarioID);
                        var call__146 = AoWWorldMap.GetCampaignPage(call__145.FIntroPages, 0);
                        bool bool__147 = call__146.FImageLibrary != (AnsiString)"";
                        if (!bool__147)
                        {
                            var call__148 = GetCampaign();
                            var call__149 = GetCampaign();
                            var call__150 = GetCampaignScenario(call__149, call__148.FNextScenarioID);
                            var call__151 = AoWWorldMap.GetCampaignPage(call__150.FIntroPages, 0);
                            var call__152 = call__151.FText.Count;
                            bool__147 = call__152 > 0;
                        }

                        if (bool__147)
                        {
                            SetBtn.Visible = false;
                            ShowBtn.Visible = false;
                            QuitBtn.Visible = false;
                            var call__153 = GetCampaign();
                            ShowIntroPages(call__153.FNextScenarioID);
                        }
                        else
                        {
                            FPawnTicks = 20;
                            BookWin.Visible = false;
                            SetBtn.Visible = false;
                            ShowBtn.Visible = false;
                            QuitBtn.Visible = false;
                            var call__154 = GetCampaign();
                            call__154.FState += 1;
                            UpdateCampaignState();
                        }

                        break;
                    case 3:
                        CampaignTransferDlg.Execute();
                        SetBtn.Visible = false;
                        ShowBtn.Visible = false;
                        QuitBtn.Visible = false;
                        break;
                    case 4:
                        var call__155 = GetCampaign();
                        var call__156 = GetCampaign();
                        var call__157 = GetCampaignScenario(call__156, call__155.FNextScenarioID);
                        var call__158 = AoWEngine.CreatePreviewHSM(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\" + call__157.FFileName);
                        Preview = (TAoWHSMap)call__158;
                        if (Preview != null)
                        {
                            var call__159 = Preview.FPlayers.IndexOfPlayerType(TPlayerType.ptHuman);
                            Index = call__159;
                            bool bool__160 = Index != -1;
                            if (bool__160)
                            {
                                var call__161 = GetCampaign();
                                bool__160 = call__161.FRaceHistory.Count > 0;
                            }

                            bool bool__162 = bool__160;
                            if (bool__162)
                            {
                                var call__163 = Preview.FPlayers.GetPlayers(Index);
                                var call__164 = GetCampaign();
                                var call__165 = call__164.FRaceHistory.IndexOf((byte)call__163.FPlayerRace);
                                bool__162 = call__165 == -1;
                            }

                            bool bool__166 = bool__162;
                            if (bool__166)
                            {
                                var call__167 = GetCampaign();
                                var call__168 = call__167.FTransferSettings.FLeader.GetSphereCount();
                                var call__169 = GetCampaign();
                                bool__166 = call__168 < call__169.FRaceHistory.Count * 2 + 3;
                            }

                            if (bool__166)
                            {
                                ShowSpheres();
                            }
                            else
                            {
                                var call__170 = GetCampaign();
                                call__170.FState += 1;
                            }

                            Preview.Free();
                        }

                        break;
                    case 7:
                        var call__171 = GetCampaign();
                        var call__172 = GetCampaign();
                        var call__173 = GetCampaignScenario(call__172, call__171.FNextScenarioID);
                        if (call__173.FFileName != (AnsiString)"")
                        {
                            var call__174 = GetCampaign();
                            call__174.FState += 1;
                            var call__175 = GetCampaign();
                            var call__176 = GetCampaign();
                            call__176.StartScenario(call__175.FNextScenarioID, null);
                            SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"Scenario\\Campaign\\");
                            var call__177 = GetCampaign();
                            var call__178 = GetCampaign();
                            var call__179 = GetCampaignScenario(call__178, call__177.FCurrentScenarioID);
                            AoWWinMan.AoWWinMan.LoadGame(General, call__179.FFileName);
                            TitleWindow.TitlePnl.Visible = true;
                            WorldPnl.Visible = false;
                            var call__180 = GetCampaign();
                            call__180.SetupScenario();
                            AoWWinMan.AoWWinMan.StartGame(General);
                            AoWWorldMap.WorldMap.DescLbl.Text = (AnsiString)"";
                        }
                        else
                        {
                            AoWWorldMap.WorldMap.DescLbl.Text = (AnsiString)"";
                            var call__181 = GetCampaign();
                            call__181.FState = 0;
                            UpdateCampaignState();
                        }

                        break;
                    case 10:
                        var call__182 = GetCampaign();
                        ShowIntroPages(call__182.FCurrentScenarioID);
                        break;
                }
            }
        }

        // AoWWorldMap.pas:347
        public void ShowIntroPages(int Index)
        {
            SetCurrentDir(AoWEngine.StartupDirectory + (AnsiString)"Int\\Scenes");
            var call__183 = GetCampaign();
            var call__184 = GetCampaignScenario(call__183, Index);
            var call__185 = GetCampaign();
            var call__186 = AoWWorldMap.GetCampaignPage(call__184.FIntroPages, call__185.FPage);
            LabelL.Text = call__186.FHeading;
            var call__187 = GetCampaign();
            var call__188 = GetCampaignScenario(call__187, Index);
            var call__189 = GetCampaign();
            var call__190 = AoWWorldMap.GetCampaignPage(call__188.FIntroPages, call__189.FPage + 1);
            LabelR.Text = call__190.FHeading;
            var call__191 = GetCampaign();
            var call__192 = GetCampaignScenario(call__191, Index);
            var call__193 = GetCampaign();
            var call__194 = AoWWorldMap.GetCampaignPage(call__192.FIntroPages, call__193.FPage);
            MemoL.Items.Assign(call__194.FText);
            var call__195 = GetCampaign();
            var call__196 = GetCampaignScenario(call__195, Index);
            var call__197 = GetCampaign();
            var call__198 = AoWWorldMap.GetCampaignPage(call__196.FIntroPages, call__197.FPage + 1);
            MemoR.Items.Assign(call__198.FText);
            leftSB.Position = 0;
            rightSB.Position = 0;
            var call__199 = GetCampaign();
            var call__200 = GetCampaignScenario(call__199, Index);
            var call__201 = GetCampaign();
            var call__202 = AoWWorldMap.GetCampaignPage(call__200.FIntroPages, call__201.FPage);
            if (call__202.FImageLibrary == (AnsiString)"")
            {
                PicL.ImageLib = null;
            }
            else
            {
                var call__203 = GetCampaign();
                var call__204 = GetCampaignScenario(call__203, Index);
                var call__205 = GetCampaign();
                var call__206 = AoWWorldMap.GetCampaignPage(call__204.FIntroPages, call__205.FPage);
                aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPL.LoadFromFile(call__206.FImageLibrary);
                PicL.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPL;
                var call__207 = GetCampaign();
                var call__208 = GetCampaignScenario(call__207, Index);
                var call__209 = GetCampaign();
                var call__210 = AoWWorldMap.GetCampaignPage(call__208.FIntroPages, call__209.FPage);
                PicL.CurrentImage = call__210.FImageIndex;
                PicL.WinWidth = 0;
                PicL.WinHeight = 0;
                PicL.Visible = true;
            }

            var call__211 = GetCampaign();
            var call__212 = GetCampaignScenario(call__211, Index);
            var call__213 = GetCampaign();
            var call__214 = AoWWorldMap.GetCampaignPage(call__212.FIntroPages, call__213.FPage + 1);
            if (call__214.FImageLibrary == (AnsiString)"")
            {
                PicR.ImageLib = null;
            }
            else
            {
                var call__215 = GetCampaign();
                var call__216 = GetCampaignScenario(call__215, Index);
                var call__217 = GetCampaign();
                var call__218 = AoWWorldMap.GetCampaignPage(call__216.FIntroPages, call__217.FPage + 1);
                aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPR.LoadFromFile(call__218.FImageLibrary);
                PicR.ImageLib = aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookPR;
                var call__219 = GetCampaign();
                var call__220 = GetCampaignScenario(call__219, Index);
                var call__221 = GetCampaign();
                var call__222 = AoWWorldMap.GetCampaignPage(call__220.FIntroPages, call__221.FPage + 1);
                PicR.CurrentImage = call__222.FImageIndex;
                PicR.WinWidth = 0;
                PicR.WinHeight = 0;
                PicR.Visible = true;
            }

            var call__223 = GetCampaign();
            Back.Visible = call__223.FPage > 0;
            var call__224 = GetCampaign();
            if (call__224.FPage == 8)
            {
                Next.Visible = false;
            }
            else
            {
                var call__225 = GetCampaign();
                var call__226 = GetCampaignScenario(call__225, Index);
                var call__227 = GetCampaign();
                var call__228 = AoWWorldMap.GetCampaignPage(call__226.FIntroPages, call__227.FPage + 2);
                bool bool__229 = call__228.FImageLibrary != (AnsiString)"";
                if (!bool__229)
                {
                    var call__230 = GetCampaign();
                    var call__231 = GetCampaignScenario(call__230, Index);
                    var call__232 = GetCampaign();
                    var call__233 = AoWWorldMap.GetCampaignPage(call__231.FIntroPages, call__232.FPage + 2);
                    var call__234 = call__233.FText.Count;
                    bool__229 = call__234 > 0;
                }

                Next.Visible = bool__229;
            }

            bool bool__235 = !Next.Visible;
            if (!bool__235)
            {
                var call__236 = GetCampaign();
                bool__235 = call__236.FState == 10;
            }

            BookStart.Visible = bool__235;
            Cancel.Visible = WorldPnl.Visible;
            var aoWBookWindow = BookWin;
            aoWBookWindow.FHidden = true;
            aoWBookWindow.Visible = true;

            if (BookWin.FBackSurface != null)
            {
                BookWin.FBackSurface.Cls(BookWin.FMaskColor);
            }

            if (TitleWindow.TitleWin.Visible)
            {
                General.Manager.OverlayPriority = 249;
            }
        }

        // AoWWorldMap.pas:404
        public void ShowSpheres()
        {
            int Picks = default;
            TLeaderSetupControl Control = default;
            WorldPnl.Visible = false;
            var call__238 = GetCampaign();
            Picks = call__238.FTransferSettings.FLeader.FSpherePicksMax;
            Control = new TLeaderSetupControl(null);
            var call__239 = GetCampaign();
            Control.FLeader.Assign(call__239.FTransferSettings.FLeader, AoWEngine);
            Control.FLeader.SetSpherePicksMax((byte)(Picks + 2));
            Control.FOriginalLeader.Assign(Control.FLeader, AoWEngine);
            Control.FMode = TSet<TLeaderSetupModeElement>.Of(TLeaderSetupModeElement.lsmSpheres);
            LeaderSetupWin.Execute(Control);
            LeaderSetupWin.ClearBtn.Visible = false;
            LeaderSetupWin.DefaultBtn.Visible = false;
            LeaderSetupWin.PrevBtn.Visible = false;
            LeaderSetupWin.SetFixedSpheres(Picks);
            var call__240 = TranslateRStr(AssignXNewSpheresRStr);
            var call__241 = AoWE.AoWE.Format(call__240, [IntToStr(2)]);
            LeaderSetupWin.Phase3Header.Text = call__241;
            Control.FOnDone = SpheresDone;
            Control.Release();
            LeaderSetupWin.LeaderPnl.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            LeaderSetupWin.LeaderPnl.ILIndexTexture = 0;
        }

        // AoWWorldMap.pas:386
        public void SpheresDone(TObject Sender)
        {
            TLeaderSetupControl Control = default;
            TLeader Leader = default;
            int I = default;
            Control = (TLeaderSetupControl)Sender;
            var leaderSetupControl = (TLeaderSetupControl)Sender;
            if (leaderSetupControl.FStatus == TLeaderSetupStatus.lssDone)
            {
                var call__243 = leaderSetupControl.FOriginalLeader.Copy(AoWEngine);
                Leader = (TLeader)call__243;
                {
                    I = GetCampaign().FTransferSettings.FLeader.GetSphereCount();
                    int end__244 = Leader.GetSphereCount() - 1;
                    for (; I <= end__244; I += 1)
                    {
                        var call__245 = Leader.GetSpheres(I);
                        var call__246 = GetCampaign();
                        call__246.FTransferSettings.FSelectedSpheres.Add((byte)(int)call__245);
                    }
                }

                WorldPnl.Visible = true;
                var call__247 = GetCampaign();
                call__247.FState += 1;
                UpdateCampaignState();
            }
        }

        // AoWWorldMap.pas:428
        public void BookClosed()
        {
            TNotifyEvent Done = default;
            TEObject Context = default;
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

            General.Manager.OverlayPriority = 0;
        }

        // AoWWorldMap.pas:146
        public void ResetCampaign()
        {
            var call__248 = GetCampaign();
            call__248.FState = 0;
            var call__249 = GetCampaign();
            call__249.FPage = 0;
            InfoPnl.Visible = false;
            var call__250 = TranslateRStr(ShowInfoRStr);
            ShowBtn.Text = call__250;
        }

        // AoWWorldMap.pas:151
        public void ShowCampaignHistory()
        {
            TWMCampaignScenario Scenario = default;
            var call__251 = GetCampaign();
            call__251.FState = 10;
            var call__252 = GetCampaign();
            call__252.FPage = 0;
            var call__253 = GetCampaign();
            var call__254 = GetCampaign();
            var call__255 = GetCampaignScenario(call__254, call__253.FCurrentScenarioID);
            Scenario = call__255;
            while (true)
            {
                var call__256 = Scenario.FIntroPages.Count;
                var call__257 = GetCampaign();
                bool bool__258 = call__256 > call__257.FPage + 2;
                if (bool__258)
                {
                    var call__259 = GetCampaign();
                    var call__260 = AoWWorldMap.GetCampaignPage(Scenario.FIntroPages, call__259.FPage + 2);
                    bool bool__261 = call__260.FImageLibrary != (AnsiString)"";
                    if (!bool__261)
                    {
                        var call__262 = GetCampaign();
                        var call__263 = AoWWorldMap.GetCampaignPage(Scenario.FIntroPages, call__262.FPage + 2);
                        var call__264 = call__263.FText.Count;
                        bool__261 = call__264 > 0;
                    }

                    bool__258 = bool__261;
                }

                if (!bool__258)
                    break;
                {
                    var call__265 = GetCampaign();
                    call__265.FPage += 2;
                }
            }

            var call__266 = GetCampaign();
            ShowIntroPages(call__266.FCurrentScenarioID);
        }

        // AoWWorldMap.pas:442
        public void BackClick()
        {
            Back.CaptureInput = false;
            Next.CaptureInput = false;
            var button = BookStart;
            button.CaptureInput = false;
            button.Visible = false;

            this.BookWin.FDrawn = false;
            if (this.BookWin.FBackSurface != null)
            {
                this.BookWin.FBackSurface.Cls(this.BookWin.FMaskColor);
            }

            PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
            FBookAction = 1;
        }

        // AoWWorldMap.pas:863
        public void WorldMapCreate()
        {
            RegisterWorldMapCampaign(AoWEngine);
        }

        // AoWWorldMap.pas:461
        public void NextStartClick()
        {
            Back.CaptureInput = false;
            Next.CaptureInput = false;
            BookStart.CaptureInput = false;
            PlaySample((AnsiString)"Sfx\\Int\\Pagef1.wav");
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
                    var call__269 = GetCampaign();
                    Path1.CurrentImage = call__269.FFaction * 3 + (Frame + 2);
                    var call__270 = GetCampaign();
                    Path2.CurrentImage = call__270.FFaction * 3 + (FPathFrame + 2);
                    var call__271 = GetCampaign();
                    Path3.CurrentImage = call__271.FFaction * 3 + (FPathFrame + 2);
                }

                var call__272 = GetCampaign();
                if (call__272.FState == 5)
                {
                    if (FPawnTicks > 0)
                    {
                        var call__273 = GetCampaign();
                        var call__274 = GetCampaign();
                        var call__275 = GetCampaignScenario(call__274, call__273.FNextScenarioID);
                        var call__276 = GetCampaign();
                        var call__277 = GetCampaign();
                        var call__278 = GetCampaignScenario(call__277, call__276.FCurrentScenarioID);
                        var call__279 = GetCampaign();
                        var call__280 = GetCampaign();
                        var call__281 = GetCampaignScenario(call__280, call__279.FCurrentScenarioID);
                        PPawn.WinLeft = (call__275.FMapX - call__278.FMapX) * (20 - FPawnTicks) / 19 + call__281.FMapX - 25;
                        var call__282 = GetCampaign();
                        var call__283 = GetCampaign();
                        var call__284 = GetCampaignScenario(call__283, call__282.FNextScenarioID);
                        var call__285 = GetCampaign();
                        var call__286 = GetCampaign();
                        var call__287 = GetCampaignScenario(call__286, call__285.FCurrentScenarioID);
                        var call__288 = GetCampaign();
                        var call__289 = GetCampaign();
                        var call__290 = GetCampaignScenario(call__289, call__288.FCurrentScenarioID);
                        PPawn.WinTop = (call__284.FMapY - call__287.FMapY) * (20 - FPawnTicks) / 19 + call__290.FMapY - 65;
                        if (this.PPawn.WinTop > EPawn.WinTop)
                        {
                            this.PPawn.Priority = 10;
                            EPawn.Priority = 5;
                        }
                        else
                        {
                            this.PPawn.Priority = 5;
                            EPawn.Priority = 10;
                        }
                    }
                    else
                    {
                        FPawnTicks = 20;
                        FEnemyDelay = 30;
                        var call__292 = GetCampaign();
                        call__292.FState += 1;
                        var call__293 = GetCampaign();
                        var call__294 = GetCampaign();
                        var call__295 = GetCampaignScenario(call__294, call__293.FNextScenarioID);
                        if (DescLbl.Text != call__295.FDescription)
                        {
                            var call__296 = GetCampaign();
                            var call__297 = GetCampaign();
                            var call__298 = GetCampaignScenario(call__297, call__296.FNextScenarioID);
                            DescLbl.Text = call__298.FDescription;
                            this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;

                            FDescriptionHandled = true;
                        }
                    }

                    FPawnTicks -= 1;
                }

                var call__300 = GetCampaign();
                if (call__300.FState == 6)
                {
                    if (FPawnTicks > 0)
                    {
                        FDescriptionHandled = true;
                        var call__301 = GetCampaign();
                        var call__302 = GetCampaign();
                        var call__303 = GetCampaignScenario(call__302, call__301.FCurrentScenarioID);
                        var call__304 = call__303.FPaths[3];
                        var call__305 = GetCampaign();
                        var call__306 = call__305.FScenarios.IndexOfID(call__304);
                        StartIndex = call__306;
                        var call__307 = GetCampaign();
                        var call__308 = GetCampaign();
                        var call__309 = GetCampaignScenario(call__308, call__307.FNextScenarioID);
                        var call__310 = call__309.FPaths[3];
                        var call__311 = GetCampaign();
                        var call__312 = call__311.FScenarios.IndexOfID(call__310);
                        EndIndex = call__312;
                        if (StartIndex != -1 && EndIndex != -1)
                        {
                            var call__313 = GetCampaign();
                            var call__314 = GetCampaignScenario(call__313, EndIndex);
                            var call__315 = GetCampaign();
                            var call__316 = GetCampaignScenario(call__315, StartIndex);
                            var call__317 = GetCampaign();
                            var call__318 = GetCampaignScenario(call__317, StartIndex);
                            EPawn.WinLeft = (call__314.FMapX - call__316.FMapX) * (20 - FPawnTicks) / 19 + call__318.FMapX - 25;
                            var call__319 = GetCampaign();
                            var call__320 = GetCampaignScenario(call__319, EndIndex);
                            var call__321 = GetCampaign();
                            var call__322 = GetCampaignScenario(call__321, StartIndex);
                            var call__323 = GetCampaign();
                            var call__324 = GetCampaignScenario(call__323, StartIndex);
                            EPawn.WinTop = (call__320.FMapY - call__322.FMapY) * (20 - FPawnTicks) / 19 + call__324.FMapY - 65;
                            if (this.PPawn.WinTop > EPawn.WinTop)
                            {
                                this.PPawn.Priority = 10;
                                EPawn.Priority = 5;
                            }
                            else
                            {
                                this.PPawn.Priority = 5;
                                EPawn.Priority = 10;
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
                            var call__326 = GetCampaign();
                            call__326.FState += 1;
                            UpdateCampaignState();
                        }
                    }

                    FPawnTicks -= 1;
                }
            }
        }

        // AoWWorldMap.pas:566
        public void Path1MouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__327 = GetCampaign();
                if (call__327.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__328 = GetCampaign();
                    SelectCampaignPath(call__328, 0);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:578
        public void Path2MouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__329 = GetCampaign();
                if (call__329.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__330 = GetCampaign();
                    SelectCampaignPath(call__330, 1);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:590
        public void Path3MouseDown(TAoWComponent Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                var call__331 = GetCampaign();
                if (call__331.FState == 1)
                {
                    DescLbl.Text = (AnsiString)"";
                    var call__332 = GetCampaign();
                    SelectCampaignPath(call__332, 2);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:602
        public void CancelClick()
        {
            var call__333 = GetCampaign();
            call__333.ResetNextScenario();
            var call__334 = GetCampaign();
            call__334.FState = 1;
            PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            UpdateCampaignState();
        }

        // AoWWorldMap.pas:611
        public void PPawnMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            TObject Control = default;
            Control = Sender;
            if (X > 20 && Control == PPawn)
            {
                var call__335 = GetCampaign();
                if (call__335.FState == 1)
                {
                    var call__336 = GetCampaign();
                    call__336.FState = 0;
                    var call__337 = GetCampaign();
                    call__337.FPage = 0;
                    UpdateCampaignState();
                }
            }

            // { Original hit test uses X. }
            ((TAoWComponent)Control).FHandled = false;
        }

        // AoWWorldMap.pas:624
        public void PPawnMouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            AnsiString Description = default;
            TAoWComponent Pawn = default;
            Pawn = Sender;
            // { Original hit test uses X. }
            if (X > 20)
            {
                var call__338 = GetCampaign();
                if (call__338.FState == 1)
                {
                    if (Pawn == PPawn)
                    {
                        Description = TranslateRStr(ViewSummaryRStr);
                    }
                    else
                    {
                        var call__339 = GetCampaign();
                        if (call__339.FFaction == 0)
                        {
                            Description = TranslateRStr(CultOfStormsRStr);
                        }
                        else
                        {
                            Description = TranslateRStr(KeepersRStr);
                        }
                    }
                }
            }

            if (DescLbl.Text != Description && Description != (AnsiString)"")
            {
                DescLbl.Text = Description;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }

            FDescriptionHandled = true;
            Pawn.FHandled = false;
        }

        // AoWWorldMap.pas:643
        public void Path1MouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__341 = GetCampaign();
            if (call__341.FState == 1)
            {
                var call__342 = GetCampaign();
                var call__343 = GetCampaign();
                var call__344 = GetCampaignScenario(call__343, call__342.FCurrentScenarioID);
                var call__345 = call__344.FPaths[0];
                var call__346 = GetCampaign();
                var call__347 = call__346.FScenarios.IndexOfID(call__345);
                Index = call__347;
                var call__348 = GetCampaign();
                var call__349 = GetCampaignScenario(call__348, Index);
                if (DescLbl.Text != call__349.FPathDescription)
                {
                    var call__350 = GetCampaign();
                    var call__351 = GetCampaignScenario(call__350, Index);
                    DescLbl.Text = call__351.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:661
        public void Path2MouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__353 = GetCampaign();
            if (call__353.FState == 1)
            {
                var call__354 = GetCampaign();
                var call__355 = GetCampaign();
                var call__356 = GetCampaignScenario(call__355, call__354.FCurrentScenarioID);
                var call__357 = call__356.FPaths[1];
                var call__358 = GetCampaign();
                var call__359 = call__358.FScenarios.IndexOfID(call__357);
                Index = call__359;
                var call__360 = GetCampaign();
                var call__361 = GetCampaignScenario(call__360, Index);
                if (DescLbl.Text != call__361.FPathDescription)
                {
                    var call__362 = GetCampaign();
                    var call__363 = GetCampaignScenario(call__362, Index);
                    DescLbl.Text = call__363.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:679
        public void Path3MouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            int Index = default;
            var call__365 = GetCampaign();
            if (call__365.FState == 1)
            {
                var call__366 = GetCampaign();
                var call__367 = GetCampaign();
                var call__368 = GetCampaignScenario(call__367, call__366.FCurrentScenarioID);
                var call__369 = call__368.FPaths[2];
                var call__370 = GetCampaign();
                var call__371 = call__370.FScenarios.IndexOfID(call__369);
                Index = call__371;
                var call__372 = GetCampaign();
                var call__373 = GetCampaignScenario(call__372, Index);
                if (DescLbl.Text != call__373.FPathDescription)
                {
                    var call__374 = GetCampaign();
                    var call__375 = GetCampaignScenario(call__374, Index);
                    DescLbl.Text = call__375.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:697
        public void WorldPnlMouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
        {
            if (!FDescriptionHandled)
            {
                var call__377 = GetCampaign();
                if (call__377.FState == 1)
                {
                    if (this.DescLbl.Text != (AnsiString)"")
                    {
                        this.DescLbl.Text = (AnsiString)"";
                    }
                }
            }

            FDescriptionHandled = false;
        }

        // AoWWorldMap.pas:706
        public void BookStartClick()
        {
            BookWin.Visible = false;
            PlaySample((AnsiString)"Sfx\\Int\\BookClse.wav");
            if (!WorldPnl.Visible)
            {
                General.Manager.OverlayPriority = 0;
            }

            if (WorldPnl.Visible)
            {
                FPawnTicks = 20;
                var call__379 = GetCampaign();
                call__379.FState += 1;
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
            TRect R = default;
            switch (FBookAction)
            {
                case 3:
                    BookWin.StartAnim((PageTurnCtrl.TPageTurnMode)1);
                    var call__380 = GetCampaign();
                    call__380.FPage -= 2;
                    UpdateCampaignState();
                    FBookAction = 0;
                    break;
                case 4:
                    BookWin.StartAnim(0);
                    var call__381 = GetCampaign();
                    call__381.FPage += 2;
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

            if (BookStart.Visible != AoWWorldMap.BookStartVisible)
            {
                var call__382 = Rect(BookWin.WinLeft + BookStart.WinLeft, BookWin.WinTop + BookStart.WinTop, BookWin.WinLeft + BookStart.WinLeft + BookStart.WinWidth, BookWin.WinTop + BookStart.WinTop + BookStart.WinHeight);
                R = call__382;
                General.Manager.AddDrawRect(ref R, BookWin);
                AoWWorldMap.BookStartVisible = BookStart.Visible;
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
                var call__383 = TranslateRStr(ShowInfoRStr);
                ShowBtn.Text = call__383;
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
                var call__384 = TranslateRStr(HideInfoRStr);
                ShowBtn.Text = call__384;
            }
        }

        // AoWWorldMap.pas:786
        public void QuitBtnClick()
        {
            var call__385 = TranslateRStr(QuitCampaignRStr);
            var call__386 = TranslateRStr(E2Globals.E2Globals.ConfirmRStr);
            DialogWin.Confirm(call__386, call__385, 5);
        }

        // AoWWorldMap.pas:791
        public void StarImgUpdate()
        {
            FStarIntensity += FStarStep;
            if (FStarIntensity + FStarStep > 100)
            {
                FStarStep = -1;
            }

            if (FStarIntensity + FStarStep < 50)
            {
                FStarStep = 1;
            }

            StarImg.FDrawn = false;
        }

        // AoWWorldMap.pas:800
        public void StarImgDraw(TObject Sender, TSurfaceDesc Surface)
        {
            var call__387 = aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap[FStarImage];
            call__387.ShowBlended(WorldPnl.WinLeft + StarImg.WinLeft, WorldPnl.WinTop + StarImg.WinTop, Surface, IntensityBlendTable.LookupTables[FStarIntensity]);
        }

        // AoWWorldMap.pas:806
        public void Ally1PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            var call__388 = Rect(InfoPnl.WinLeft + Ally1Pnl.WinLeft, InfoPnl.WinTop + Ally1Pnl.WinTop, InfoPnl.WinLeft + Ally1Pnl.WinLeft + Ally1Pnl.WinWidth, InfoPnl.WinTop + Ally1Pnl.WinTop + Ally1Pnl.WinHeight);
            R = call__388;
            var call__389 = GetCampaign();
            if (call__389.FRaceHistory.Count > 0)
            {
                var call__390 = GetCampaign();
                var call__391 = call__390.FRaceHistory[0];
                var call__392 = AoWHSSet.FRaceResources.GetRaceResource(call__391);
                var call__393 = call__392.GetFace();
                call__393.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:818
        public void Ally2PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            var call__394 = Rect(InfoPnl.WinLeft + Ally2Pnl.WinLeft, InfoPnl.WinTop + Ally2Pnl.WinTop, InfoPnl.WinLeft + Ally2Pnl.WinLeft + Ally2Pnl.WinWidth, InfoPnl.WinTop + Ally2Pnl.WinTop + Ally2Pnl.WinHeight);
            R = call__394;
            var call__395 = GetCampaign();
            if (call__395.FRaceHistory.Count > 1)
            {
                var call__396 = GetCampaign();
                var call__397 = call__396.FRaceHistory[1];
                var call__398 = AoWHSSet.FRaceResources.GetRaceResource(call__397);
                var call__399 = call__398.GetFace();
                call__399.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:830
        public void Ally3PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            var call__400 = Rect(InfoPnl.WinLeft + Ally3Pnl.WinLeft, InfoPnl.WinTop + Ally3Pnl.WinTop, InfoPnl.WinLeft + Ally3Pnl.WinLeft + Ally3Pnl.WinWidth, InfoPnl.WinTop + Ally3Pnl.WinTop + Ally3Pnl.WinHeight);
            R = call__400;
            var call__401 = GetCampaign();
            if (call__401.FRaceHistory.Count > 2)
            {
                var call__402 = GetCampaign();
                var call__403 = call__402.FRaceHistory[2];
                var call__404 = AoWHSSet.FRaceResources.GetRaceResource(call__403);
                var call__405 = call__404.GetFace();
                call__405.ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:842
        public void HeroPnlPreDraw(TObject Sender, TSurfaceDesc Surface)
        {
            TRect R = default;
            var call__406 = Rect(InfoPnl.WinLeft + HeroPnl.WinLeft, InfoPnl.WinTop + HeroPnl.WinTop, InfoPnl.WinLeft + HeroPnl.WinLeft + HeroPnl.WinWidth, InfoPnl.WinTop + HeroPnl.WinTop + HeroPnl.WinHeight);
            R = call__406;
            var call__407 = GetCampaign();
            if (call__407.FTransferSettings.FLeader != null)
            {
                var call__408 = GetCampaign();
                call__408.FTransferSettings.FLeader.ShowFace(R, Surface);
            }
        }

        // AoWWorldMap.pas:854
        public void SetBtnClick()
        {
            CampaignSettingsDlg.Dlg.Visible = true;
        }

        // AoWWorldMap.pas:859
        public void BookWinHide()
        {
            BookClosed();
        }
    }

    public partial class TFileTime : TObject
    {
        public double FTime;
    }

    public static partial class AoWWorldMap
    {
        // AoWWorldMap.pas:147
        public static TWMCampaignScenarioPage GetCampaignPage(TWMCampaignScenarioPageList Pages, int Index)
        {
            TWMCampaignScenarioPage Result = default;
            Result = (TWMCampaignScenarioPage)Pages.FChildren.List[Index];
            return Result;
        }

        public static TTargetSelectionDlg TargetSelectionDlg;
        public static TWorldMap WorldMap;
        public static bool BookStartVisible;
    }
}
