// Translated from AoWWorldMap.pas.
using Aow.Rtl;
using Aow.Units.AoWBookWindow;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWE;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.aowIntGfxMod;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWTargetSelection;
using Aow.Units.AoWVideoDlg;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWinMan;
using Aow.Units.E2Globals;
using Aow.Units.Engine;
using Aow.Units.GFXE;
using static Aow.Rtl.RtlUnit;
using static Aow.Units.AoWE.AoWEUnit;
using static Aow.Units.AoWExeForms.AoWExeFormsUnit;
using static Aow.Units.AoWLeaderSetup.AoWLeaderSetupUnit;
using static Aow.Units.AoWVideoDlg.AoWVideoDlgUnit;
using static Aow.Units.AoWWinMan.AoWWinManUnit;
using static Aow.Units.E2CampaignSupport.E2CampaignSupportUnit;
using static Aow.Units.E2GameFlow.E2GameFlowUnit;
using static Aow.Units.E2Globals.E2GlobalsUnit;
using static Aow.Units.GFXE.GFXEUnit;
using static Aow.Units.Sound.SoundUnit;

namespace Aow.Units.AoWWorldMap;

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
    public TWMCampaign GetCampaign() => (TWMCampaign)AoWEngine.FCampaign;

    // AoWWorldMap.pas:157
    public void UpdateCampaignState()
    {
        int Index = default;
        TAoWHSMap Preview = default;
        if (GetCampaign() != null)
        {
            switch (GetCampaign().FState)
            {
                case 0:
                    var campaign = GetCampaign();
                    bool condition = !IsEmpty(AoWWorldMapUnit.GetCampaignPage(GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FOutroPages, 0).FImageLibrary);
                    if (!condition)
                    {
                        var campaign2 = GetCampaign();
                        condition = AoWWorldMapUnit.GetCampaignPage(GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID).FOutroPages, 0).FText.Count > 0;
                    }

                    if (condition)
                    {
                        SetCurrentDir(AoWEngine.StartupDirectory + "\\Int\\Scenes");
                        var campaign2 = GetCampaign();
                        var campaignScenario = GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID);
                        var campaign3 = GetCampaign();
                        var campaignPage = AoWWorldMapUnit.GetCampaignPage(campaignScenario.FOutroPages, campaign3.FPage);
                        LabelL.Text = campaignPage.FHeading;
                        var campaign4 = GetCampaign();
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), campaign4.FCurrentScenarioID);
                        var campaign5 = GetCampaign();
                        var aoWWorldMapCampaignPage = AoWWorldMapUnit.GetCampaignPage(campaignScenario2.FOutroPages, campaign5.FPage + 1);
                        LabelR.Text = aoWWorldMapCampaignPage.FHeading;
                        var campaign6 = GetCampaign();
                        var campaignScenario3 = GetCampaignScenario(GetCampaign(), campaign6.FCurrentScenarioID);
                        var campaign7 = GetCampaign();
                        var campaignPage2 = AoWWorldMapUnit.GetCampaignPage(campaignScenario3.FOutroPages, campaign7.FPage);
                        MemoL.Items.Assign(campaignPage2.FText);
                        var campaign8 = GetCampaign();
                        var campaignScenario4 = GetCampaignScenario(GetCampaign(), campaign8.FCurrentScenarioID);
                        var campaign9 = GetCampaign();
                        var campaignPage3 = AoWWorldMapUnit.GetCampaignPage(campaignScenario4.FOutroPages, campaign9.FPage + 1);
                        MemoR.Items.Assign(campaignPage3.FText);
                        FStarIntensity = 50;
                        FStarStep = 1;
                        var campaign10 = GetCampaign();
                        switch (GetCampaignScenario(GetCampaign(), campaign10.FCurrentScenarioID).FMapKind)
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
                        var campaignScenario5 = GetCampaignScenario(GetCampaign(), campaign11.FCurrentScenarioID);
                        var campaign12 = GetCampaign();
                        if (IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario5.FOutroPages, campaign12.FPage).FImageLibrary))
                        {
                            PicL.ImageLib = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage);
                            aowIntGfxModUnit.IntGfxMod.BookPL.LoadFromFile(campaignPage4.FImageLibrary);
                            PicL.ImageLib = aowIntGfxModUnit.IntGfxMod.BookPL;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage);
                            PicL.CurrentImage = campaignPage5.FImageIndex;
                            PicL.WinWidth = 0;
                            PicL.WinHeight = 0;
                            PicL.Visible = true;
                        }

                        var campaign17 = GetCampaign();
                        var campaignScenario8 = GetCampaignScenario(GetCampaign(), campaign17.FCurrentScenarioID);
                        var campaign18 = GetCampaign();
                        if (IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario8.FOutroPages, campaign18.FPage + 1).FImageLibrary))
                        {
                            PicR.ImageLib = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage + 1);
                            aowIntGfxModUnit.IntGfxMod.BookPR.LoadFromFile(campaignPage4.FImageLibrary);
                            PicR.ImageLib = aowIntGfxModUnit.IntGfxMod.BookPR;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage + 1);
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
                            var campaignScenario6 = GetCampaignScenario(GetCampaign(), campaign13.FCurrentScenarioID);
                            var campaign14 = GetCampaign();
                            bool test = !IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FOutroPages, campaign14.FPage + 2).FImageLibrary);
                            if (!test)
                            {
                                var campaign15 = GetCampaign();
                                var campaignScenario7 = GetCampaignScenario(GetCampaign(), campaign15.FCurrentScenarioID);
                                var campaign16 = GetCampaign();
                                test = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FOutroPages, campaign16.FPage + 2).FText.Count > 0;
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

                        if (TitleWindow.TitleWin.Visible)
                        {
                            General.Manager.OverlayPriority = 249;
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
                    var campaignScenario9 = GetCampaignScenario(GetCampaign(), campaign20.FCurrentScenarioID);
                    PPawn.WinLeft = campaignScenario9.FMapX - 25;
                    var campaign21 = GetCampaign();
                    var campaignScenario10 = GetCampaignScenario(GetCampaign(), campaign21.FCurrentScenarioID);
                    PPawn.WinTop = campaignScenario10.FMapY - 65;
                    var campaign22 = GetCampaign();
                    var path = GetCampaignScenario(GetCampaign(), campaign22.FCurrentScenarioID).FPaths[3];
                    Index = GetCampaign().FScenarios.IndexOfID(path);
                    if (Index != -1)
                    {
                        var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
                        EPawn.WinLeft = campaignScenario.FMapX - 25;
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
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
                    if (GetCampaignScenario(GetCampaign(), campaign23.FCurrentScenarioID).FPaths[0] > -1)
                    {
                        Path1.Visible = true;
                        var campaign2 = GetCampaign();
                        Path1.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[0];
                        Index = GetCampaign().FScenarios.IndexOfID(path2);
                        var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
                        Path1.WinLeft = campaignScenario.FMapX;
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                        Path1.WinTop = campaignScenario2.FMapY;
                    }
                    else
                    {
                        Path1.Visible = false;
                    }

                    var campaign24 = GetCampaign();
                    if (GetCampaignScenario(GetCampaign(), campaign24.FCurrentScenarioID).FPaths[1] > -1)
                    {
                        Path2.Visible = true;
                        var campaign2 = GetCampaign();
                        Path2.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[1];
                        Index = GetCampaign().FScenarios.IndexOfID(path2);
                        var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
                        Path2.WinLeft = campaignScenario.FMapX;
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                        Path2.WinTop = campaignScenario2.FMapY;
                    }
                    else
                    {
                        Path2.Visible = false;
                    }

                    var campaign25 = GetCampaign();
                    if (GetCampaignScenario(GetCampaign(), campaign25.FCurrentScenarioID).FPaths[2] > -1)
                    {
                        Path3.Visible = true;
                        var campaign2 = GetCampaign();
                        Path3.CurrentImage = campaign2.FFaction * 3 + (FPathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID).FPaths[2];
                        Index = GetCampaign().FScenarios.IndexOfID(path2);
                        var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
                        Path3.WinLeft = campaignScenario.FMapX;
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                        Path3.WinTop = campaignScenario2.FMapY;
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
                    var campaign26 = GetCampaign();
                    bool passed = !IsEmpty(AoWWorldMapUnit.GetCampaignPage(GetCampaignScenario(GetCampaign(), campaign26.FNextScenarioID).FIntroPages, 0).FImageLibrary);
                    if (!passed)
                    {
                        var campaign2 = GetCampaign();
                        passed = AoWWorldMapUnit.GetCampaignPage(GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID).FIntroPages, 0).FText.Count > 0;
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
                    CampaignTransferDlg.Execute();
                    SetBtn.Visible = false;
                    ShowBtn.Visible = false;
                    QuitBtn.Visible = false;
                    break;
                case 4:
                    var campaign27 = GetCampaign();
                    var campaignScenario11 = GetCampaignScenario(GetCampaign(), campaign27.FNextScenarioID);
                    Preview = (TAoWHSMap)AoWEngine.CreatePreviewHSM(FileBoundary.ReadName(AoWEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario11.FFileName), Aow.Formats.GameProfile.WorldMap);
                    if (Preview != null)
                    {
                        Index = Preview.FPlayers.IndexOfPlayerType(TPlayerType.ptHuman);
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

                        Preview.Free();
                    }

                    break;
                case 7:
                    var campaign28 = GetCampaign();
                    if (!IsEmpty(GetCampaignScenario(GetCampaign(), campaign28.FNextScenarioID).FFileName))
                    {
                        var campaign2 = GetCampaign();
                        campaign2.FState += 1;
                        var campaign3 = GetCampaign();
                        GetCampaign().StartScenario(campaign3.FNextScenarioID, null);
                        SetCurrentDir(AoWEngine.StartupDirectory + "Scenario\\Campaign\\");
                        var campaign4 = GetCampaign();
                        var campaignScenario = GetCampaignScenario(GetCampaign(), campaign4.FCurrentScenarioID);
                        AoWWinManUnit.LoadGame(General, FileBoundary.ReadName(campaignScenario.FFileName));
                        TitleWindow.TitlePnl.Visible = true;
                        WorldPnl.Visible = false;
                        GetCampaign().SetupScenario();
                        AoWWinManUnit.StartGame(General);
                        AoWWorldMapUnit.WorldMap.DescLbl.Text = "";
                    }
                    else
                    {
                        AoWWorldMapUnit.WorldMap.DescLbl.Text = "";
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
        SetCurrentDir(AoWEngine.StartupDirectory + "Int\\Scenes");
        var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
        var campaign = GetCampaign();
        var campaignPage = AoWWorldMapUnit.GetCampaignPage(campaignScenario.FIntroPages, campaign.FPage);
        LabelL.Text = campaignPage.FHeading;
        var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
        var campaign2 = GetCampaign();
        var aoWWorldMapCampaignPage = AoWWorldMapUnit.GetCampaignPage(campaignScenario2.FIntroPages, campaign2.FPage + 1);
        LabelR.Text = aoWWorldMapCampaignPage.FHeading;
        var campaignScenario3 = GetCampaignScenario(GetCampaign(), Index);
        var campaign3 = GetCampaign();
        var campaignPage2 = AoWWorldMapUnit.GetCampaignPage(campaignScenario3.FIntroPages, campaign3.FPage);
        MemoL.Items.Assign(campaignPage2.FText);
        var campaignScenario4 = GetCampaignScenario(GetCampaign(), Index);
        var campaign4 = GetCampaign();
        var campaignPage3 = AoWWorldMapUnit.GetCampaignPage(campaignScenario4.FIntroPages, campaign4.FPage + 1);
        MemoR.Items.Assign(campaignPage3.FText);
        leftSB.Position = 0;
        rightSB.Position = 0;
        var campaignScenario5 = GetCampaignScenario(GetCampaign(), Index);
        var campaign5 = GetCampaign();
        if (IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario5.FIntroPages, campaign5.FPage).FImageLibrary))
        {
            PicL.ImageLib = null;
        }
        else
        {
            var campaignScenario6 = GetCampaignScenario(GetCampaign(), Index);
            var campaign6 = GetCampaign();
            var campaignPage4 = AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage);
            aowIntGfxModUnit.IntGfxMod.BookPL.LoadFromFile(campaignPage4.FImageLibrary);
            PicL.ImageLib = aowIntGfxModUnit.IntGfxMod.BookPL;
            var campaignScenario7 = GetCampaignScenario(GetCampaign(), Index);
            var campaign7 = GetCampaign();
            var campaignPage5 = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage);
            PicL.CurrentImage = campaignPage5.FImageIndex;
            PicL.WinWidth = 0;
            PicL.WinHeight = 0;
            PicL.Visible = true;
        }

        var campaignScenario8 = GetCampaignScenario(GetCampaign(), Index);
        var campaign8 = GetCampaign();
        if (IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario8.FIntroPages, campaign8.FPage + 1).FImageLibrary))
        {
            PicR.ImageLib = null;
        }
        else
        {
            var campaignScenario6 = GetCampaignScenario(GetCampaign(), Index);
            var campaign6 = GetCampaign();
            var campaignPage4 = AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage + 1);
            aowIntGfxModUnit.IntGfxMod.BookPR.LoadFromFile(campaignPage4.FImageLibrary);
            PicR.ImageLib = aowIntGfxModUnit.IntGfxMod.BookPR;
            var campaignScenario7 = GetCampaignScenario(GetCampaign(), Index);
            var campaign7 = GetCampaign();
            var campaignPage5 = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage + 1);
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
            var campaignScenario6 = GetCampaignScenario(GetCampaign(), Index);
            var campaign6 = GetCampaign();
            bool condition = !IsEmpty(AoWWorldMapUnit.GetCampaignPage(campaignScenario6.FIntroPages, campaign6.FPage + 2).FImageLibrary);
            if (!condition)
            {
                var campaignScenario7 = GetCampaignScenario(GetCampaign(), Index);
                var campaign7 = GetCampaign();
                condition = AoWWorldMapUnit.GetCampaignPage(campaignScenario7.FIntroPages, campaign7.FPage + 2).FText.Count > 0;
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
        Picks = GetCampaign().FTransferSettings.FLeader.FSpherePicksMax;
        Control = new TLeaderSetupControl(null);
        var campaign = GetCampaign();
        Control.FLeader.Assign(campaign.FTransferSettings.FLeader, AoWEngine);
        Control.FLeader.SetSpherePicksMax((byte)(Picks + 2));
        Control.FOriginalLeader.Assign(Control.FLeader, AoWEngine);
        Control.FMode = TSet<TLeaderSetupModeElement>.Of(TLeaderSetupModeElement.lsmSpheres);
        LeaderSetupWin.Execute(Control);
        LeaderSetupWin.ClearBtn.Visible = false;
        LeaderSetupWin.DefaultBtn.Visible = false;
        LeaderSetupWin.PrevBtn.Visible = false;
        LeaderSetupWin.SetFixedSpheres(Picks);
        var format = AoWEUnit.Format(TranslateRStr(AssignXNewSpheresRStr), [IntToStr(2)]);
        LeaderSetupWin.Phase3Header.Text = format;
        Control.FOnDone = SpheresDone;
        Control.Release();
        LeaderSetupWin.LeaderPnl.ILTexture = aowIntGfxModUnit.IntGfxMod.WorldMap;
        LeaderSetupWin.LeaderPnl.ILIndexTexture = 0;
    }

    // AoWWorldMap.pas:386
    public void SpheresDone(TObject Sender)
    {
        TLeader Leader;
        _ = (TLeaderSetupControl)Sender;
        var leaderSetupControl = (TLeaderSetupControl)Sender;
        if (leaderSetupControl.FStatus == TLeaderSetupStatus.lssDone)
        {
            Leader = (TLeader)leaderSetupControl.FOriginalLeader.Copy(AoWEngine);
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
        TNotifyEvent Done;
        TEObject Context;
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
        GetCampaign().FState = 0;
        GetCampaign().FPage = 0;
        InfoPnl.Visible = false;
        var showInfo = TranslateRStr(ShowInfoRStr);
        ShowBtn.Text = showInfo;
    }

    // AoWWorldMap.pas:151
    public void ShowCampaignHistory()
    {
        TWMCampaignScenario Scenario;
        GetCampaign().FState = 10;
        GetCampaign().FPage = 0;
        var campaign = GetCampaign();
        Scenario = GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID);
        while (true)
        {
            bool condition = Scenario.FIntroPages.Count > GetCampaign().FPage + 2;
            if (condition)
            {
                var campaign2 = GetCampaign();
                bool test = !IsEmpty(AoWWorldMapUnit.GetCampaignPage(Scenario.FIntroPages, campaign2.FPage + 2).FImageLibrary);
                if (!test)
                {
                    var campaign3 = GetCampaign();
                    test = AoWWorldMapUnit.GetCampaignPage(Scenario.FIntroPages, campaign3.FPage + 2).FText.Count > 0;
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

        PlaySample("Sfx\\Int\\Pagef1.wav");
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
        PlaySample("Sfx\\Int\\Pagef1.wav");
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
                    var campaignScenario = GetCampaignScenario(GetCampaign(), campaign.FNextScenarioID);
                    var campaign2 = GetCampaign();
                    var campaignScenario2 = GetCampaignScenario(GetCampaign(), campaign2.FCurrentScenarioID);
                    var campaign3 = GetCampaign();
                    var campaignScenario3 = GetCampaignScenario(GetCampaign(), campaign3.FCurrentScenarioID);
                    PPawn.WinLeft = (campaignScenario.FMapX - campaignScenario2.FMapX) * (20 - FPawnTicks) / 19 + campaignScenario3.FMapX - 25;
                    var campaign4 = GetCampaign();
                    var campaignScenario4 = GetCampaignScenario(GetCampaign(), campaign4.FNextScenarioID);
                    var campaign5 = GetCampaign();
                    var campaignScenario5 = GetCampaignScenario(GetCampaign(), campaign5.FCurrentScenarioID);
                    var campaign6 = GetCampaign();
                    var campaignScenario6 = GetCampaignScenario(GetCampaign(), campaign6.FCurrentScenarioID);
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
                    var campaignScenario = GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID);
                    if (DescLbl.Text != campaignScenario.FDescription)
                    {
                        var campaign3 = GetCampaign();
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), campaign3.FNextScenarioID);
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
                    var path = GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[3];
                    StartIndex = GetCampaign().FScenarios.IndexOfID(path);
                    var campaign2 = GetCampaign();
                    var path2 = GetCampaignScenario(GetCampaign(), campaign2.FNextScenarioID).FPaths[3];
                    EndIndex = GetCampaign().FScenarios.IndexOfID(path2);
                    if (StartIndex != -1 && EndIndex != -1)
                    {
                        var campaignScenario = GetCampaignScenario(GetCampaign(), EndIndex);
                        var campaignScenario2 = GetCampaignScenario(GetCampaign(), StartIndex);
                        var campaignScenario3 = GetCampaignScenario(GetCampaign(), StartIndex);
                        EPawn.WinLeft = (campaignScenario.FMapX - campaignScenario2.FMapX) * (20 - FPawnTicks) / 19 + campaignScenario3.FMapX - 25;
                        var campaignScenario4 = GetCampaignScenario(GetCampaign(), EndIndex);
                        var campaignScenario5 = GetCampaignScenario(GetCampaign(), StartIndex);
                        var campaignScenario6 = GetCampaignScenario(GetCampaign(), StartIndex);
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
    public void Path1MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().FState == 1)
            {
                DescLbl.Text = "";
                SelectCampaignPath(GetCampaign(), 0);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:578
    public void Path2MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().FState == 1)
            {
                DescLbl.Text = "";
                SelectCampaignPath(GetCampaign(), 1);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:590
    public void Path3MouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().FState == 1)
            {
                DescLbl.Text = "";
                SelectCampaignPath(GetCampaign(), 2);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:602
    public void CancelClick()
    {
        GetCampaign().ResetNextScenario();
        GetCampaign().FState = 1;
        PlaySample("Sfx\\Int\\BookClse.wav");
        UpdateCampaignState();
    }

    // AoWWorldMap.pas:611
    public void PPawnMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        TObject Control;
        Control = Sender;
        if (X > 20 && Control == PPawn)
        {
            if (GetCampaign().FState == 1)
            {
                GetCampaign().FState = 0;
                GetCampaign().FPage = 0;
                UpdateCampaignState();
            }
        }

        // { Original hit test uses X. }
        ((TAoWComponent)Control).FHandled = false;
    }

    // AoWWorldMap.pas:624
    public void PPawnMouseMove(TAoWComponent Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        string Description = "";
        TAoWComponent Pawn;
        Pawn = Sender;
        // { Original hit test uses X. }
        if (X > 20)
        {
            if (GetCampaign().FState == 1)
            {
                if (Pawn == PPawn)
                {
                    Description = TranslateRStr(ViewSummaryRStr);
                }
                else
                {
                    if (GetCampaign().FFaction == 0)
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

        if (DescLbl.Text != Description && !IsEmpty(Description))
        {
            DescLbl.Text = Description;
            this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
        }

        FDescriptionHandled = true;
        Pawn.FHandled = false;
    }

    // AoWWorldMap.pas:643
    public void Path1MouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        int Index;
        if (GetCampaign().FState == 1)
        {
            var campaign = GetCampaign();
            var path = GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[0];
            Index = GetCampaign().FScenarios.IndexOfID(path);
            var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
            if (DescLbl.Text != campaignScenario.FPathDescription)
            {
                var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                DescLbl.Text = campaignScenario2.FPathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        FDescriptionHandled = true;
    }

    // AoWWorldMap.pas:661
    public void Path2MouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        int Index;
        if (GetCampaign().FState == 1)
        {
            var campaign = GetCampaign();
            var path = GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[1];
            Index = GetCampaign().FScenarios.IndexOfID(path);
            var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
            if (DescLbl.Text != campaignScenario.FPathDescription)
            {
                var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                DescLbl.Text = campaignScenario2.FPathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        FDescriptionHandled = true;
    }

    // AoWWorldMap.pas:679
    public void Path3MouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
    {
        int Index;
        if (GetCampaign().FState == 1)
        {
            var campaign = GetCampaign();
            var path = GetCampaignScenario(GetCampaign(), campaign.FCurrentScenarioID).FPaths[2];
            Index = GetCampaign().FScenarios.IndexOfID(path);
            var campaignScenario = GetCampaignScenario(GetCampaign(), Index);
            if (DescLbl.Text != campaignScenario.FPathDescription)
            {
                var campaignScenario2 = GetCampaignScenario(GetCampaign(), Index);
                DescLbl.Text = campaignScenario2.FPathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        FDescriptionHandled = true;
    }

    // AoWWorldMap.pas:697
    public void WorldPnlMouseMove(TObject Sender, TSet<TShiftState> Shift, int X, int Y)
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
        PlaySample("Sfx\\Int\\BookClse.wav");
        if (!WorldPnl.Visible)
        {
            General.Manager.OverlayPriority = 0;
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
        TRect R;
        switch (FBookAction)
        {
            case 3:
                BookWin.StartAnim((PageTurnCtrl.TPageTurnMode)1);
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

        if (BookStart.Visible != AoWWorldMapUnit.BookStartVisible)
        {
            R = Rect(BookWin.WinLeft + BookStart.WinLeft, BookWin.WinTop + BookStart.WinTop, BookWin.WinLeft + BookStart.WinLeft + BookStart.WinWidth, BookWin.WinTop + BookStart.WinTop + BookStart.WinHeight);
            General.Manager.AddDrawRect(ref R, BookWin);
            AoWWorldMapUnit.BookStartVisible = BookStart.Visible;
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
            var showInfo = TranslateRStr(ShowInfoRStr);
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
            var hideInfo = TranslateRStr(HideInfoRStr);
            ShowBtn.Text = hideInfo;
        }
    }

    // AoWWorldMap.pas:786
    public void QuitBtnClick()
    {
        var quitCampaign = TranslateRStr(QuitCampaignRStr);
        var confirm = TranslateRStr(E2GlobalsUnit.ConfirmRStr);
        DialogWin.Confirm(confirm, quitCampaign, 5);
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
        aowIntGfxModUnit.IntGfxMod.WorldMap[FStarImage].ShowBlended(WorldPnl.WinLeft + StarImg.WinLeft, WorldPnl.WinTop + StarImg.WinTop, Surface, IntensityBlendTable.LookupTables[FStarIntensity]);
    }

    // AoWWorldMap.pas:806
    public void Ally1PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        R = Rect(InfoPnl.WinLeft + Ally1Pnl.WinLeft, InfoPnl.WinTop + Ally1Pnl.WinTop, InfoPnl.WinLeft + Ally1Pnl.WinLeft + Ally1Pnl.WinWidth, InfoPnl.WinTop + Ally1Pnl.WinTop + Ally1Pnl.WinHeight);
        if (GetCampaign().FRaceHistory.Count > 0)
        {
            var raceHistory = GetCampaign().FRaceHistory[0];
            AoWHSSet.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
        }
    }

    // AoWWorldMap.pas:818
    public void Ally2PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        R = Rect(InfoPnl.WinLeft + Ally2Pnl.WinLeft, InfoPnl.WinTop + Ally2Pnl.WinTop, InfoPnl.WinLeft + Ally2Pnl.WinLeft + Ally2Pnl.WinWidth, InfoPnl.WinTop + Ally2Pnl.WinTop + Ally2Pnl.WinHeight);
        if (GetCampaign().FRaceHistory.Count > 1)
        {
            var raceHistory = GetCampaign().FRaceHistory[1];
            AoWHSSet.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
        }
    }

    // AoWWorldMap.pas:830
    public void Ally3PnlPreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        R = Rect(InfoPnl.WinLeft + Ally3Pnl.WinLeft, InfoPnl.WinTop + Ally3Pnl.WinTop, InfoPnl.WinLeft + Ally3Pnl.WinLeft + Ally3Pnl.WinWidth, InfoPnl.WinTop + Ally3Pnl.WinTop + Ally3Pnl.WinHeight);
        if (GetCampaign().FRaceHistory.Count > 2)
        {
            var raceHistory = GetCampaign().FRaceHistory[2];
            AoWHSSet.FRaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, Surface);
        }
    }

    // AoWWorldMap.pas:842
    public void HeroPnlPreDraw(TObject Sender, TSurfaceDesc Surface)
    {
        TRect R;
        R = Rect(InfoPnl.WinLeft + HeroPnl.WinLeft, InfoPnl.WinTop + HeroPnl.WinTop, InfoPnl.WinLeft + HeroPnl.WinLeft + HeroPnl.WinWidth, InfoPnl.WinTop + HeroPnl.WinTop + HeroPnl.WinHeight);
        if (GetCampaign().FTransferSettings.FLeader != null)
        {
            GetCampaign().FTransferSettings.FLeader.ShowFace(R, Surface);
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

public static partial class AoWWorldMapUnit
{
    // AoWWorldMap.pas:147
    public static TWMCampaignScenarioPage GetCampaignPage(TWMCampaignScenarioPageList Pages, int Index) => (TWMCampaignScenarioPage)Pages.FChildren.List[Index];

    public static TTargetSelectionDlg TargetSelectionDlg;
    public static TWorldMap WorldMap;
    public static bool BookStartVisible;
}
