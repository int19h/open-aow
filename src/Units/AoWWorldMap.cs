// Translated from AoWWorldMap.pas.
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
// Translated from AoWWorldMap.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs
{

    // star image, brightness, path animation, pawn timing, hover, context, completion event, book
    // action
    public partial class WorldMap : DataModule
    {
        public WorldMap(Component owner) : base(owner)
        { }

        public AOWMemo MemoL;
        public AOWImage PicR;
        public AOWPanel WorldPnl;
        public AOWImage Path1;
        public AOWImage Path2;
        public AOWImage Path3;
        public AOWImage PPawn;
        public AOWImage EPawn;
        public AOWLabel DescLbl;
        public AOWButton Back;
        public AOWButton Next;
        public AOWButton Cancel;
        public AOWButton BookStart;
        public AOWLabel LabelL;
        public AOWMemo MemoR;
        public AOWLabel LabelR;
        public AOWImage PicL;
        public AoWBookWindow BookWin;
        public AOWPanel InfoPnl;
        public AOWLabel Elves;
        public AOWLabel Halflings;
        public AOWLabel Goblins;
        public AOWLabel Orcs;
        public AOWLabel Dwarves;
        public AOWLabel Undead;
        public AOWLabel Highmen;
        public AOWLabel Lizardmen;
        public AOWLabel DarkElves;
        public AOWLabel VoW;
        public AOWLabel ElvenC;
        public AOWLabel IoLG;
        public AOWLabel Deepmir;
        public AOWLabel Aldor;
        public AOWLabel Serpent;
        public AOWLabel Blackwater;
        public AOWLabel Skull;
        public AOWLabel Sylvanus;
        public AOWLabel Pass;
        public AOWLabel Arcalot;
        public AOWLabel Flowershire;
        public AOWLabel Sobek;
        public AOWLabel Steppe;
        public AOWLabel Bronnam;
        public AOWLabel UC;
        public AOWLabel Woods;
        public AOWLabel Burbatak;
        public AOWButton QuitBtn;
        public AOWButton ShowBtn;
        public AOWLabel SelectLbl;
        public AOWLabel AlliesLnl;
        public AOWPanel Frame;
        public AOWLabel Azrac;
        public AOWImage StarImg;
        public AOWLabel Terstar;
        public AoWFrame HeroPnl;
        public AoWFrame Ally1Pnl;
        public AoWFrame Ally2Pnl;
        public AoWFrame Ally3Pnl;
        public AOWButton SetBtn;
        public IvTranslator Translator;
        public AOWVScrollBar leftSB;
        public AOWVScrollBar rightSB;
        public int FStarImage;
        public int FStarStep;
        public int FStarIntensity;
        public int FPathFrame;
        public int FPathDelay;
        public int FPawnTicks;
        public int FEnemyDelay;
        public bool FDescriptionHandled;
        public EObject FBookContext;
        public NotifyEvent<object> FOnBookClosed;
        public byte FBookAction;

        // AoWWorldMap.pas:152
        public WMCampaign GetCampaign() => (WMCampaign)GameTables.AoWEngine.FCampaign;

        // AoWWorldMap.pas:157
        public void UpdateCampaignState()
        {
            int Index = default;
            AoWHSMap Preview = default;
            if (GetCampaign() != null)
            {
                switch (GetCampaign().FState)
                {
                    case 0:
                        var campaign = GetCampaign();
                        bool condition = !IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FOutroPages, 0).FImageLibrary);
                        if (!condition)
                        {
                            var campaign2 = GetCampaign();
                            condition = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID).FOutroPages, 0).FText.Count > 0;
                        }

                        if (condition)
                        {
                            SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "\\Int\\Scenes");
                            var campaign2 = GetCampaign();
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID);
                            var campaign3 = GetCampaign();
                            var campaignPage = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario.FOutroPages, campaign3.FPage);
                            LabelL.Text = campaignPage.FHeading;
                            var campaign4 = GetCampaign();
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign4.FCurrentScenarioID);
                            var campaign5 = GetCampaign();
                            var aoWWorldMapCampaignPage = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario2.FOutroPages, campaign5.FPage + 1);
                            LabelR.Text = aoWWorldMapCampaignPage.FHeading;
                            var campaign6 = GetCampaign();
                            var campaignScenario3 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign6.FCurrentScenarioID);
                            var campaign7 = GetCampaign();
                            var campaignPage2 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario3.FOutroPages, campaign7.FPage);
                            MemoL.Items.Assign(campaignPage2.FText);
                            var campaign8 = GetCampaign();
                            var campaignScenario4 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign8.FCurrentScenarioID);
                            var campaign9 = GetCampaign();
                            var campaignPage3 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario4.FOutroPages, campaign9.FPage + 1);
                            MemoR.Items.Assign(campaignPage3.FText);
                            FStarIntensity = 50;
                            FStarStep = 1;
                            var campaign10 = GetCampaign();
                            switch (Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign10.FCurrentScenarioID).FMapKind)
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

                            var campaign11 = GetCampaign();
                            var campaignScenario5 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign11.FCurrentScenarioID);
                            var campaign12 = GetCampaign();
                            if (IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario5.FOutroPages, campaign12.FPage).FImageLibrary))
                            {
                                PicL.ImageLib = null;
                            }
                            else
                            {
                                var campaign13 = GetCampaign();
                                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                                var campaign14 = GetCampaign();
                                var campaignPage4 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage);
                                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.BookPL.LoadFromFile(campaignPage4.FImageLibrary);
                                PicL.ImageLib = DialogHosts.IntGfxMod.BookPL;
                                var campaign15 = GetCampaign();
                                var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                                var campaign16 = GetCampaign();
                                var campaignPage5 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage);
                                PicL.CurrentImage = campaignPage5.FImageIndex;
                                PicL.WinWidth = 0;
                                PicL.WinHeight = 0;
                                PicL.Visible = true;
                            }

                            var campaign17 = GetCampaign();
                            var campaignScenario8 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign17.FCurrentScenarioID);
                            var campaign18 = GetCampaign();
                            if (IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario8.FOutroPages, campaign18.FPage + 1).FImageLibrary))
                            {
                                PicR.ImageLib = null;
                            }
                            else
                            {
                                var campaign13 = GetCampaign();
                                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                                var campaign14 = GetCampaign();
                                var campaignPage4 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage + 1);
                                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.BookPR.LoadFromFile(campaignPage4.FImageLibrary);
                                PicR.ImageLib = DialogHosts.IntGfxMod.BookPR;
                                var campaign15 = GetCampaign();
                                var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                                var campaign16 = GetCampaign();
                                var campaignPage5 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage + 1);
                                PicR.CurrentImage = campaignPage5.FImageIndex;
                                PicR.WinWidth = 0;
                                PicR.WinHeight = 0;
                                PicR.Visible = true;
                            }

                            var campaign19 = GetCampaign();
                            Back.Visible = campaign19.FPage > 0;
                            if (GetCampaign().FPage == 8)
                            {
                                Next.Visible = false;
                            }
                            else
                            {
                                var campaign13 = GetCampaign();
                                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                                var campaign14 = GetCampaign();
                                bool test = !IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage + 2).FImageLibrary);
                                if (!test)
                                {
                                    var campaign15 = GetCampaign();
                                    var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                                    var campaign16 = GetCampaign();
                                    test = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage + 2).FText.Count > 0;
                                }

                                Next.Visible = test;
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

                            if (DialogHosts.TitleWindow.TitleWin.Visible)
                            {
                                DialogHosts.General.Manager.OverlayPriority = 249;
                            }
                        }
                        else
                        {
                            var campaign2 = GetCampaign();
                            campaign2.FState += 1;
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
                        var campaign20 = GetCampaign();
                        var campaignScenario9 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign20.FCurrentScenarioID);
                        PPawn.WinLeft = campaignScenario9.FMapX - 25;
                        var campaign21 = GetCampaign();
                        var campaignScenario10 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign21.FCurrentScenarioID);
                        PPawn.WinTop = campaignScenario10.FMapY - 65;
                        var campaign22 = GetCampaign();
                        var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign22.FCurrentScenarioID).FPaths[3];
                        Index = GetCampaign().FScenarios.IndexOfID(path);
                        if (Index != -1)
                        {
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            EPawn.WinLeft = campaignScenario.FMapX - 25;
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            EPawn.WinTop = campaignScenario2.FMapY - 65;
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

                        var campaign23 = GetCampaign();
                        if (Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign23.FCurrentScenarioID).FPaths[0] > -1)
                        {
                            Path1.Visible = true;
                            var campaign2 = GetCampaign();
                            Path1.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                            var campaign3 = GetCampaign();
                            var path2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[0];
                            Index = GetCampaign().FScenarios.IndexOfID(path2);
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path1.WinLeft = campaignScenario.FMapX;
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path1.WinTop = campaignScenario2.FMapY;
                        }
                        else
                        {
                            Path1.Visible = false;
                        }

                        var campaign24 = GetCampaign();
                        if (Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign24.FCurrentScenarioID).FPaths[1] > -1)
                        {
                            Path2.Visible = true;
                            var campaign2 = GetCampaign();
                            Path2.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                            var campaign3 = GetCampaign();
                            var path2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[1];
                            Index = GetCampaign().FScenarios.IndexOfID(path2);
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path2.WinLeft = campaignScenario.FMapX;
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path2.WinTop = campaignScenario2.FMapY;
                        }
                        else
                        {
                            Path2.Visible = false;
                        }

                        var campaign25 = GetCampaign();
                        if (Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign25.FCurrentScenarioID).FPaths[2] > -1)
                        {
                            Path3.Visible = true;
                            var campaign2 = GetCampaign();
                            Path3.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                            var campaign3 = GetCampaign();
                            var path2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[2];
                            Index = GetCampaign().FScenarios.IndexOfID(path2);
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path3.WinLeft = campaignScenario.FMapX;
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                            Path3.WinTop = campaignScenario2.FMapY;
                        }
                        else
                        {
                            Path3.Visible = false;
                        }

                        if (!Path1.Visible && !Path2.Visible && !Path3.Visible)
                        {
                            DialogHosts.TitleWindow.TitlePnl.Visible = false;
                            DialogHosts.TitleWindow.TitlePnl.FRedraw = false;
                            DialogHosts.TitleWindow.TitleWin.Visible = false;
                            DialogHosts.TBWindow.TaskWindow.Visible = false;
                            WorldPnl.Visible = false;
                            BookWin.Visible = false;
                            Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseCampaign();
                            Aow.Game.Ui.Dialogs.DialogHosts.TitleScene.ResetScene();
                            Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
                        }

                        break;
                    case 2:
                        var campaign26 = GetCampaign();
                        bool passed = !IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign26.FNextScenarioID).FIntroPages, 0).FImageLibrary);
                        if (!passed)
                        {
                            var campaign2 = GetCampaign();
                            passed = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID).FIntroPages, 0).FText.Count > 0;
                        }

                        if (passed)
                        {
                            SetBtn.Visible = false;
                            ShowBtn.Visible = false;
                            QuitBtn.Visible = false;
                            ShowIntroPages(GetCampaign().FNextScenarioID);
                        }
                        else
                        {
                            FPawnTicks = 20;
                            BookWin.Visible = false;
                            SetBtn.Visible = false;
                            ShowBtn.Visible = false;
                            QuitBtn.Visible = false;
                            var campaign2 = GetCampaign();
                            campaign2.FState += 1;
                            UpdateCampaignState();
                        }

                        break;
                    case 3:
                        Aow.Game.Ui.Dialogs.DialogHosts.CampaignTransferDlg.Execute();
                        SetBtn.Visible = false;
                        ShowBtn.Visible = false;
                        QuitBtn.Visible = false;
                        break;
                    case 4:
                        var campaign27 = GetCampaign();
                        var campaignScenario11 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign27.FNextScenarioID);
                        Preview = (AoWHSMap)Aow.Game.Magic.Abilities.GameTables.AoWEngine.CreatePreviewHSM(FileBoundary.ReadName(GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario11.FFileName), Aow.Formats.GameProfile.WorldMap);
                        if (Preview != null)
                        {
                            Index = Preview.FPlayers.IndexOfPlayerType(PlayerType.Human);
                            bool test = Index != -1 && GetCampaign().FRaceHistory.Count > 0;
                            if (test)
                            {
                                var player = Preview.FPlayers.GetPlayers(Index);
                                test = GetCampaign().FRaceHistory.IndexOf((byte)player.FPlayerRace) == -1;
                            }

                            if (test && GetCampaign().FTransferSettings.FLeader.GetSphereCount() < GetCampaign().FRaceHistory.Count * 2 + 3)
                            {
                                ShowSpheres();
                            }
                            else
                            {
                                var campaign2 = GetCampaign();
                                campaign2.FState += 1;
                            }

                            Preview?.Dispose();
                        }

                        break;
                    case 7:
                        var campaign28 = GetCampaign();
                        if (!IsEmpty(Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign28.FNextScenarioID).FFileName))
                        {
                            var campaign2 = GetCampaign();
                            campaign2.FState += 1;
                            var campaign3 = GetCampaign();
                            GetCampaign().StartScenario(campaign3.FNextScenarioID, null);
                            SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "Scenario\\Campaign\\");
                            var campaign4 = GetCampaign();
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign4.FCurrentScenarioID);
                            Aow.Game.Ui.Dialogs.General.LoadGame(DialogHosts.General, FileBoundary.ReadName(campaignScenario.FFileName));
                            DialogHosts.TitleWindow.TitlePnl.Visible = true;
                            WorldPnl.Visible = false;
                            GetCampaign().SetupScenario();
                            Aow.Game.Ui.Dialogs.General.StartGame(DialogHosts.General);
                            DescLbl.Text = "";
                        }
                        else
                        {
                            DescLbl.Text = "";
                            GetCampaign().FState = 0;
                            UpdateCampaignState();
                        }

                        break;
                    case 10:
                        ShowIntroPages(GetCampaign().FCurrentScenarioID);
                        break;
                }
            }
        }

        // AoWWorldMap.pas:347
        public void ShowIntroPages(int Index)
        {
            SetCurrentDir(GameTables.AoWEngine.StartupDirectory + "Int\\Scenes");
            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign = GetCampaign();
            var campaignPage = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario.FIntroPages, campaign.FPage);
            LabelL.Text = campaignPage.FHeading;
            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign2 = GetCampaign();
            var aoWWorldMapCampaignPage = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario2.FIntroPages, campaign2.FPage + 1);
            LabelR.Text = aoWWorldMapCampaignPage.FHeading;
            var campaignScenario3 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign3 = GetCampaign();
            var campaignPage2 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario3.FIntroPages, campaign3.FPage);
            MemoL.Items.Assign(campaignPage2.FText);
            var campaignScenario4 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign4 = GetCampaign();
            var campaignPage3 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario4.FIntroPages, campaign4.FPage + 1);
            MemoR.Items.Assign(campaignPage3.FText);
            leftSB.Position = 0;
            rightSB.Position = 0;
            var campaignScenario5 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign5 = GetCampaign();
            if (IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario5.FIntroPages, campaign5.FPage).FImageLibrary))
            {
                PicL.ImageLib = null;
            }
            else
            {
                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                var campaign6 = GetCampaign();
                var campaignPage4 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage);
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.BookPL.LoadFromFile(campaignPage4.FImageLibrary);
                PicL.ImageLib = DialogHosts.IntGfxMod.BookPL;
                var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                var campaign7 = GetCampaign();
                var campaignPage5 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage);
                PicL.CurrentImage = campaignPage5.FImageIndex;
                PicL.WinWidth = 0;
                PicL.WinHeight = 0;
                PicL.Visible = true;
            }

            var campaignScenario8 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
            var campaign8 = GetCampaign();
            if (IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario8.FIntroPages, campaign8.FPage + 1).FImageLibrary))
            {
                PicR.ImageLib = null;
            }
            else
            {
                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                var campaign6 = GetCampaign();
                var campaignPage4 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage + 1);
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.BookPR.LoadFromFile(campaignPage4.FImageLibrary);
                PicR.ImageLib = DialogHosts.IntGfxMod.BookPR;
                var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                var campaign7 = GetCampaign();
                var campaignPage5 = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage + 1);
                PicR.CurrentImage = campaignPage5.FImageIndex;
                PicR.WinWidth = 0;
                PicR.WinHeight = 0;
                PicR.Visible = true;
            }

            var campaign9 = GetCampaign();
            Back.Visible = campaign9.FPage > 0;
            if (GetCampaign().FPage == 8)
            {
                Next.Visible = false;
            }
            else
            {
                var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                var campaign6 = GetCampaign();
                bool condition = !IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage + 2).FImageLibrary);
                if (!condition)
                {
                    var campaignScenario7 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                    var campaign7 = GetCampaign();
                    condition = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage + 2).FText.Count > 0;
                }

                Next.Visible = condition;
            }

            bool test = !Next.Visible || GetCampaign().FState == 10;
            BookStart.Visible = test;
            Cancel.Visible = WorldPnl.Visible;
            var aoWBookWindow = BookWin;
            aoWBookWindow.FHidden = true;
            aoWBookWindow.Visible = true;

            if (BookWin.FBackSurface != null)
            {
                BookWin.FBackSurface.Cls(BookWin.FMaskColor);
            }

            if (DialogHosts.TitleWindow.TitleWin.Visible)
            {
                DialogHosts.General.Manager.OverlayPriority = 249;
            }
        }

        // AoWWorldMap.pas:404
        public void ShowSpheres()
        {
            int Picks = default;
            LeaderSetupControl Control = default;
            WorldPnl.Visible = false;
            Picks = GetCampaign().FTransferSettings.FLeader.FSpherePicksMax;
            Control = new LeaderSetupControl(null);
            var campaign = GetCampaign();
            Control.FLeader.Assign(campaign.FTransferSettings.FLeader, GameTables.AoWEngine);
            Control.FLeader.SetSpherePicksMax((byte)(Picks + 2));
            Control.FOriginalLeader.Assign(Control.FLeader, GameTables.AoWEngine);
            Control.FMode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres);
            Aow.Game.Ui.Dialogs.DialogHosts.LeaderSetupWin.Execute(Control);
            DialogHosts.LeaderSetupWin.ClearBtn.Visible = false;
            DialogHosts.LeaderSetupWin.DefaultBtn.Visible = false;
            DialogHosts.LeaderSetupWin.PrevBtn.Visible = false;
            Aow.Game.Ui.Dialogs.DialogHosts.LeaderSetupWin.SetFixedSpheres(Picks);
            var format = Aow.Game.Magic.Abilities.AbilityRules.Format(Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(AssignXNewSpheresRStr), [IntToStr(2)]);
            DialogHosts.LeaderSetupWin.Phase3Header.Text = format;
            Control.FOnDone = SpheresDone;
            Control.Release();
            DialogHosts.LeaderSetupWin.LeaderPnl.ILTexture = DialogHosts.IntGfxMod.WorldMap;
            DialogHosts.LeaderSetupWin.LeaderPnl.ILIndexTexture = 0;
        }

        // AoWWorldMap.pas:386
        public void SpheresDone(LeaderSetupControl Sender)
        {
            Leader Leader;
            var leaderSetupControl = Sender;
            if (leaderSetupControl.FStatus == LeaderSetupControl.LeaderSetupStatus.Done)
            {
                Leader = (Leader)leaderSetupControl.FOriginalLeader.Copy(GameTables.AoWEngine);
                for (int I = GetCampaign().FTransferSettings.FLeader.GetSphereCount(), last = Leader.GetSphereCount() - 1; I <= last; I++)
                {
                    var sphere = Leader.GetSpheres(I);
                    GetCampaign().FTransferSettings.FSelectedSpheres.Add((byte)(int)sphere);
                }

                WorldPnl.Visible = true;
                var campaign = GetCampaign();
                campaign.FState += 1;
                UpdateCampaignState();
            }
        }

        // AoWWorldMap.pas:428
        public void BookClosed()
        {
            NotifyEvent<object> Done;
            EObject Context;
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

            DialogHosts.General.Manager.OverlayPriority = 0;
        }

        // AoWWorldMap.pas:146
        public void ResetCampaign()
        {
            GetCampaign().FState = 0;
            GetCampaign().FPage = 0;
            InfoPnl.Visible = false;
            var showInfo = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ShowInfoRStr);
            ShowBtn.Text = showInfo;
        }

        // AoWWorldMap.pas:151
        public void ShowCampaignHistory()
        {
            WMCampaignScenario Scenario;
            GetCampaign().FState = 10;
            GetCampaign().FPage = 0;
            var campaign = GetCampaign();
            Scenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID);
            while (true)
            {
                bool condition = Scenario.FIntroPages.Count > GetCampaign().FPage + 2;
                if (condition)
                {
                    var campaign2 = GetCampaign();
                    bool test = !IsEmpty(Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Scenario.FIntroPages, campaign2.FPage + 2).FImageLibrary);
                    if (!test)
                    {
                        var campaign3 = GetCampaign();
                        test = Aow.Game.Ui.Dialogs.WorldMap.GetCampaignPage(Scenario.FIntroPages, campaign3.FPage + 2).FText.Count > 0;
                    }

                    condition = test;
                }

                if (!condition)
                    break;
                var campaign4 = GetCampaign();
                campaign4.FPage += 2;
            }

            ShowIntroPages(GetCampaign().FCurrentScenarioID);
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

            Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            FBookAction = 1;
        }

        // AoWWorldMap.pas:863
        public void WorldMapCreate()
        {
            Aow.Game.Ui.Dialogs.WorldMap.RegisterWorldMapCampaign(GameTables.AoWEngine);
        }

        // AoWWorldMap.pas:461
        public void NextStartClick()
        {
            Back.CaptureInput = false;
            Next.CaptureInput = false;
            BookStart.CaptureInput = false;
            Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
            FBookAction = 2;
        }

        // AoWWorldMap.pas:471
        public void WorldPnlUpdate()
        {
            int Frame;
            int StartIndex;
            int EndIndex;
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
                    var campaign = GetCampaign();
                    Path1.CurrentImage = campaign.FFaction * 3 + (Frame + 2);
                    var campaign2 = GetCampaign();
                    Path2.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                    var campaign3 = GetCampaign();
                    Path3.CurrentImage = campaign3.FFaction * 3 + (FPathFrame + 2);
                }

                if (GetCampaign().FState == 5)
                {
                    if (FPawnTicks > 0)
                    {
                        var campaign = GetCampaign();
                        var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FNextScenarioID);
                        var campaign2 = GetCampaign();
                        var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID);
                        var campaign3 = GetCampaign();
                        var campaignScenario3 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID);
                        PPawn.WinLeft = (campaignScenario.FMapX - campaignScenario2.FMapX) * (20 - FPawnTicks) / 19 + campaignScenario3.FMapX - 25;
                        var campaign4 = GetCampaign();
                        var campaignScenario4 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign4.FNextScenarioID);
                        var campaign5 = GetCampaign();
                        var campaignScenario5 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign5.FCurrentScenarioID);
                        var campaign6 = GetCampaign();
                        var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign6.FCurrentScenarioID);
                        PPawn.WinTop = (campaignScenario4.FMapY - campaignScenario5.FMapY) * (20 - FPawnTicks) / 19 + campaignScenario6.FMapY - 65;
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
                        var campaign = GetCampaign();
                        campaign.FState += 1;
                        var campaign2 = GetCampaign();
                        var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID);
                        if (DescLbl.Text != campaignScenario.FDescription)
                        {
                            var campaign3 = GetCampaign();
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign3.FNextScenarioID);
                            DescLbl.Text = campaignScenario2.FDescription;
                            this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;

                            FDescriptionHandled = true;
                        }
                    }

                    FPawnTicks -= 1;
                }

                if (GetCampaign().FState == 6)
                {
                    if (FPawnTicks > 0)
                    {
                        FDescriptionHandled = true;
                        var campaign = GetCampaign();
                        var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[3];
                        StartIndex = GetCampaign().FScenarios.IndexOfID(path);
                        var campaign2 = GetCampaign();
                        var path2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID).FPaths[3];
                        EndIndex = GetCampaign().FScenarios.IndexOfID(path2);
                        if (StartIndex != -1 && EndIndex != -1)
                        {
                            var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), EndIndex);
                            var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), StartIndex);
                            var campaignScenario3 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), StartIndex);
                            EPawn.WinLeft = (campaignScenario.FMapX - campaignScenario2.FMapX) * (20 - FPawnTicks) / 19 + campaignScenario3.FMapX - 25;
                            var campaignScenario4 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), EndIndex);
                            var campaignScenario5 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), StartIndex);
                            var campaignScenario6 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), StartIndex);
                            EPawn.WinTop = (campaignScenario4.FMapY - campaignScenario5.FMapY) * (20 - FPawnTicks) / 19 + campaignScenario6.FMapY - 65;
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
                            var campaign = GetCampaign();
                            campaign.FState += 1;
                            UpdateCampaignState();
                        }
                    }

                    FPawnTicks -= 1;
                }
            }
        }

        // AoWWorldMap.pas:566
        public void Path1MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                if (GetCampaign().FState == 1)
                {
                    DescLbl.Text = "";
                    Aow.Game.Ui.Dialogs.WorldMap.SelectCampaignPath(GetCampaign(), 0);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:578
        public void Path2MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                if (GetCampaign().FState == 1)
                {
                    DescLbl.Text = "";
                    Aow.Game.Ui.Dialogs.WorldMap.SelectCampaignPath(GetCampaign(), 1);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:590
        public void Path3MouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
        {
            if (!BookWin.Visible)
            {
                if (GetCampaign().FState == 1)
                {
                    DescLbl.Text = "";
                    Aow.Game.Ui.Dialogs.WorldMap.SelectCampaignPath(GetCampaign(), 2);
                    UpdateCampaignState();
                }
            }
        }

        // AoWWorldMap.pas:602
        public void CancelClick()
        {
            GetCampaign().ResetNextScenario();
            GetCampaign().FState = 1;
            Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
            UpdateCampaignState();
        }

        // AoWWorldMap.pas:611. Bound to the player pawn.
        public void PPawnMouseDown(AOWImage Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
        {
            if (X > 20)
            {
                if (GetCampaign().FState == 1)
                {
                    GetCampaign().FState = 0;
                    GetCampaign().FPage = 0;
                    UpdateCampaignState();
                }
            }

            // { Original hit test uses X. }
            Sender.FHandled = false;
        }

        // AoWWorldMap.pas:611. Bound to the enemy pawn.
        public void EPawnMouseDown(AOWImage Sender, MouseButton Button, Set<ShiftState> Shift, int X, int Y)
        {
            Sender.FHandled = false;
        }

        // AoWWorldMap.pas:624. Bound to the player pawn.
        public void PPawnMouseMove(AOWImage Sender, Set<ShiftState> Shift, int X, int Y)
        {
            string Description = "";
            // { Original hit test uses X. }
            if (X > 20)
            {
                if (GetCampaign().FState == 1)
                {
                    Description = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ViewSummaryRStr);
                }
            }

            ShowPawnDescription(Sender, Description);
        }

        // AoWWorldMap.pas:624. Bound to the enemy pawn.
        public void EPawnMouseMove(AOWImage Sender, Set<ShiftState> Shift, int X, int Y)
        {
            string Description = "";
            // { Original hit test uses X. }
            if (X > 20)
            {
                if (GetCampaign().FState == 1)
                {
                    if (GetCampaign().FFaction == 0)
                    {
                        Description = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.CultOfStormsRStr);
                    }
                    else
                    {
                        Description = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.KeepersRStr);
                    }
                }
            }

            ShowPawnDescription(Sender, Description);
        }

        // AoWWorldMap.pas:624. The label update both pawn moves share.
        private void ShowPawnDescription(AOWImage Pawn, string Description)
        {
            if (DescLbl.Text != Description && !IsEmpty(Description))
            {
                DescLbl.Text = Description;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }

            FDescriptionHandled = true;
            Pawn.FHandled = false;
        }

        // AoWWorldMap.pas:643
        public void Path1MouseMove(Set<ShiftState> Shift, int X, int Y)
        {
            int Index;
            if (GetCampaign().FState == 1)
            {
                var campaign = GetCampaign();
                var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[0];
                Index = GetCampaign().FScenarios.IndexOfID(path);
                var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                if (DescLbl.Text != campaignScenario.FPathDescription)
                {
                    var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                    DescLbl.Text = campaignScenario2.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:661
        public void Path2MouseMove(Set<ShiftState> Shift, int X, int Y)
        {
            int Index;
            if (GetCampaign().FState == 1)
            {
                var campaign = GetCampaign();
                var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[1];
                Index = GetCampaign().FScenarios.IndexOfID(path);
                var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                if (DescLbl.Text != campaignScenario.FPathDescription)
                {
                    var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                    DescLbl.Text = campaignScenario2.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:679
        public void Path3MouseMove(Set<ShiftState> Shift, int X, int Y)
        {
            int Index;
            if (GetCampaign().FState == 1)
            {
                var campaign = GetCampaign();
                var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[2];
                Index = GetCampaign().FScenarios.IndexOfID(path);
                var campaignScenario = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                if (DescLbl.Text != campaignScenario.FPathDescription)
                {
                    var campaignScenario2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(GetCampaign(), Index);
                    DescLbl.Text = campaignScenario2.FPathDescription;
                    this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
                }
            }

            FDescriptionHandled = true;
        }

        // AoWWorldMap.pas:697
        public void WorldPnlMouseMove(Set<ShiftState> Shift, int X, int Y)
        {
            if (!FDescriptionHandled)
            {
                if (GetCampaign().FState == 1)
                {
                    if (!IsEmpty(this.DescLbl.Text))
                    {
                        this.DescLbl.Text = "";
                    }
                }
            }

            FDescriptionHandled = false;
        }

        // AoWWorldMap.pas:706
        public void BookStartClick()
        {
            BookWin.Visible = false;
            Aow.Game.Audio.AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
            if (!WorldPnl.Visible)
            {
                DialogHosts.General.Manager.OverlayPriority = 0;
            }

            if (WorldPnl.Visible)
            {
                FPawnTicks = 20;
                var campaign = GetCampaign();
                campaign.FState += 1;
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
            Rect R;
            switch (FBookAction)
            {
                case 3:
                    BookWin.StartAnim((Grids.PageTurnMode)1);
                    var campaign = GetCampaign();
                    campaign.FPage -= 2;
                    UpdateCampaignState();
                    FBookAction = 0;
                    break;
                case 4:
                    BookWin.StartAnim(0);
                    var campaign2 = GetCampaign();
                    campaign2.FPage += 2;
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

            if (BookStart.Visible != BookStartVisible)
            {
                R = new Rect(BookWin.WinLeft + BookStart.WinLeft, BookWin.WinTop + BookStart.WinTop, BookWin.WinLeft + BookStart.WinLeft + BookStart.WinWidth, BookWin.WinTop + BookStart.WinTop + BookStart.WinHeight);
                Aow.Game.Ui.Dialogs.DialogHosts.General.Manager.AddDrawRect(ref R, BookWin);
                BookStartVisible = BookStart.Visible;
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
                var showInfo = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(ShowInfoRStr);
                ShowBtn.Text = showInfo;
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
                var hideInfo = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(HideInfoRStr);
                ShowBtn.Text = hideInfo;
            }
        }

        // AoWWorldMap.pas:786
        public void QuitBtnClick()
        {
            var quitCampaign = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(QuitCampaignRStr);
            var confirm = Aow.Game.Magic.Abilities.AbilityRules.TranslateRStr(LobbyText.ConfirmRStr);
            Aow.Game.Ui.Dialogs.DialogHosts.DialogWin.Confirm(confirm, quitCampaign, 5);
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
        public void StarImgDraw(SurfaceDesc Surface)
        {
            DialogHosts.IntGfxMod.WorldMap[FStarImage].ShowBlended(WorldPnl.WinLeft + StarImg.WinLeft, WorldPnl.WinTop + StarImg.WinTop, Surface, GfxState.IntensityBlendTable.LookupTables[FStarIntensity]);
        }

        // AoWWorldMap.pas:806
        public void Ally1PnlPreDraw(SurfaceDesc Surface)
        {
            Rect R;
            R = new Rect(InfoPnl.WinLeft + Ally1Pnl.WinLeft, InfoPnl.WinTop + Ally1Pnl.WinTop, InfoPnl.WinLeft + Ally1Pnl.WinLeft + Ally1Pnl.WinWidth, InfoPnl.WinTop + Ally1Pnl.WinTop + Ally1Pnl.WinHeight);
            if (GetCampaign().FRaceHistory.Count > 0)
            {
                var raceHistory = GetCampaign().FRaceHistory[0];
                Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:818
        public void Ally2PnlPreDraw(SurfaceDesc Surface)
        {
            Rect R;
            R = new Rect(InfoPnl.WinLeft + Ally2Pnl.WinLeft, InfoPnl.WinTop + Ally2Pnl.WinTop, InfoPnl.WinLeft + Ally2Pnl.WinLeft + Ally2Pnl.WinWidth, InfoPnl.WinTop + Ally2Pnl.WinTop + Ally2Pnl.WinHeight);
            if (GetCampaign().FRaceHistory.Count > 1)
            {
                var raceHistory = GetCampaign().FRaceHistory[1];
                Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:830
        public void Ally3PnlPreDraw(SurfaceDesc Surface)
        {
            Rect R;
            R = new Rect(InfoPnl.WinLeft + Ally3Pnl.WinLeft, InfoPnl.WinTop + Ally3Pnl.WinTop, InfoPnl.WinLeft + Ally3Pnl.WinLeft + Ally3Pnl.WinWidth, InfoPnl.WinTop + Ally3Pnl.WinTop + Ally3Pnl.WinHeight);
            if (GetCampaign().FRaceHistory.Count > 2)
            {
                var raceHistory = GetCampaign().FRaceHistory[2];
                Aow.Game.Engine.AoWHSSet.Current.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
            }
        }

        // AoWWorldMap.pas:842
        public void HeroPnlPreDraw(SurfaceDesc Surface)
        {
            Rect R;
            R = new Rect(InfoPnl.WinLeft + HeroPnl.WinLeft, InfoPnl.WinTop + HeroPnl.WinTop, InfoPnl.WinLeft + HeroPnl.WinLeft + HeroPnl.WinWidth, InfoPnl.WinTop + HeroPnl.WinTop + HeroPnl.WinHeight);
            if (GetCampaign().FTransferSettings.FLeader != null)
            {
                GetCampaign().FTransferSettings.FLeader.ShowFace(R, Surface);
            }
        }

        // AoWWorldMap.pas:854
        public void SetBtnClick()
        {
            DialogHosts.CampaignSettingsDlg.Dlg.Visible = true;
        }

        // AoWWorldMap.pas:859
        public void BookWinHide()
        {
            BookClosed();
        }

        private static readonly string AssignXNewSpheresRStr = "Assign %s new spheres!";

        private static void RegisterWorldMapCampaign(Engine.Engine AEngine)
        {
            AEngine.RegisterEClasses([EClass.Of<WMCampaignScenarioPage>(), EClass.Of<WMCampaignScenario>(), EClass.Of<WMCampaign>()]);
        }

        private static WMCampaignScenarioPage GetCampaignPage(WMCampaignScenarioPageList Pages, int Index) => (WMCampaignScenarioPage)Pages.FChildren.List[Index];

        private static bool BookStartVisible;

        private static void SelectCampaignPath(WMCampaign Campaign, int Index)
        {
            if (Index >= 0 && Index < 3)
            {
                Campaign.FState = 2;
                Campaign.FPage = 0;
                var path = Aow.Game.Session.WMCampaign.GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID).FPaths[Index];
                Campaign.SelectNextScenario(Campaign.FScenarios.IndexOfID(path));
                if (Aow.Game.Session.WMCampaign.GetCampaignScenario(Campaign, Campaign.FNextScenarioID).FPaths[3] == -1)
                {
                    var path2 = Aow.Game.Session.WMCampaign.GetCampaignScenario(Campaign, Campaign.FCurrentScenarioID).FPaths[3];
                    Aow.Game.Session.WMCampaign.GetCampaignScenario(Campaign, Campaign.FNextScenarioID).FPaths[3] = path2;
                }
            }
        }

        private static readonly string HideInfoRStr = "Hide Info";

        private static readonly string ShowInfoRStr = "Show Info";

        private static readonly string ViewSummaryRStr = "View Summary";

        private static readonly string QuitCampaignRStr = "Quit the Campaign?";

        internal static void ShowCampaignMap(WorldMap Dialog)
        {
            string Song = "";
            Dialog.ResetCampaign();
            Aow.Game.Magic.Abilities.GameTables.AoWEngine.CloseHSM(null);
            DialogHosts.TitleWindow.TitleWin.Visible = true;
            DialogHosts.TitleWindow.TitlePnl.Visible = false;
            Dialog.WorldPnl.Visible = true;
            Dialog.FPathFrame = 0;
            Dialog.FPathDelay = 5;
            Dialog.DescLbl.Text = "";
            DialogHosts.General.Manager.OverlayPriority = 249;
            Aow.Game.Audio.AudioHost.SongPlayer.Stop(false);
            SetCurrentDir(GameTables.AoWEngine.StartupDirectory);
            var campaign = Dialog.GetCampaign();
            if (IsEmpty(Aow.Game.Session.WMCampaign.GetCampaignScenario(Dialog.GetCampaign(), campaign.FCurrentScenarioID).FImageLibrary))
            {
                Song = "Songs\\Title.IT";
            }
            else
            {
                var dialogCampaign = Dialog.GetCampaign();
                Song = "Songs\\" + Aow.Game.Session.WMCampaign.GetCampaignScenario(Dialog.GetCampaign(), dialogCampaign.FCurrentScenarioID).FImageLibrary;
            }

            Aow.Game.Audio.AudioPlayback.PlaySongEx(Song, 1, 0, true);
            var campaign2 = Dialog.GetCampaign();
            Dialog.PPawn.CurrentImage = campaign2.FFaction + 7;
            var campaign3 = Dialog.GetCampaign();
            Dialog.EPawn.CurrentImage = 8 - campaign3.FFaction;
            Dialog.UpdateCampaignState();
        }

    }
}

namespace Aow.Game.Ui.Controls
{

    [LegacyObject]
    public partial class FileTime
    {
        public double FTime;

        public FileTime()
        {
            CaptureAddress.Assign(this);
        }
    }
}
