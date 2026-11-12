// Translated from AoWWorldMap.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// star image, brightness, path animation, pawn timing, hover, context, completion event, book
// action
public partial class WorldMap : DataModule
{
    public WorldMap(Component owner) : base(owner)
    { }

    public Memo MemoL;
    public Image PicR;
    public Panel WorldPnl;
    public Image Path1;
    public Image Path2;
    public Image Path3;
    public Image PPawn;
    public Image EPawn;
    public Label DescLbl;
    public Button Back;
    public Button Next;
    public Button Cancel;
    public Button BookStart;
    public Label LabelL;
    public Memo MemoR;
    public Label LabelR;
    public Image PicL;
    public BookWindow BookWin;
    public Panel InfoPnl;
    public Button QuitBtn;
    public Button ShowBtn;
    public Label SelectLbl;
    public Image StarImg;
    public Frame HeroPnl;
    public Frame Ally1Pnl;
    public Frame Ally2Pnl;
    public Frame Ally3Pnl;
    public Button SetBtn;
    public VerticalScrollBar leftSB;
    public VerticalScrollBar rightSB;
    private int _starImageValue;
    private int _starStep;
    private int _starIntensity;
    private int _pathFrame;
    private int _pathDelay;
    private int _pawnTicks;
    private int _enemyDelay;
    private bool _descriptionHandled;

    public EngineObject BookContext { get; set; }
    public NotifyEvent<object> OnBookClosed { get; set; }
    public byte BookAction { get; set; }

    // AoWWorldMap.pas:152
    public WorldMapCampaign GetCampaign() => (WorldMapCampaign)GameTables.GameEngine.Campaign;

    // AoWWorldMap.pas:157
    public void UpdateCampaignState()
    {
        int index = default;
        AoWHSMap preview = default;
        if (GetCampaign() != null)
        {
            switch (GetCampaign().State)
            {
                case 0:
                    var campaign = GetCampaign();
                    bool condition = !IsEmpty(GetCampaignPage(WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId).OutroPages, 0).ImageLibrary);
                    if (!condition)
                    {
                        var campaign2 = GetCampaign();
                        condition = GetCampaignPage(WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.CurrentScenarioId).OutroPages, 0).Text.Count > 0;
                    }

                    if (condition)
                    {
                        SetCurrentDir(GameTables.GameEngine.StartupDirectory + "\\Int\\Scenes");
                        var campaign2 = GetCampaign();
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.CurrentScenarioId);
                        var campaign3 = GetCampaign();
                        var campaignPage = GetCampaignPage(campaignScenario.OutroPages, campaign3.Page);
                        LabelL.Text = campaignPage.Heading;
                        var campaign4 = GetCampaign();
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.CurrentScenarioId);
                        var campaign5 = GetCampaign();
                        var worldMapCampaignPage = GetCampaignPage(campaignScenario2.OutroPages, campaign5.Page + 1);
                        LabelR.Text = worldMapCampaignPage.Heading;
                        var campaign6 = GetCampaign();
                        var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign6.CurrentScenarioId);
                        var campaign7 = GetCampaign();
                        var campaignPage2 = GetCampaignPage(campaignScenario3.OutroPages, campaign7.Page);
                        MemoL.Items.Assign(campaignPage2.Text);
                        var campaign8 = GetCampaign();
                        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign8.CurrentScenarioId);
                        var campaign9 = GetCampaign();
                        var campaignPage3 = GetCampaignPage(campaignScenario4.OutroPages, campaign9.Page + 1);
                        MemoR.Items.Assign(campaignPage3.Text);
                        _starIntensity = 50;
                        _starStep = 1;
                        var campaign10 = GetCampaign();
                        switch (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign10.CurrentScenarioId).MapKind)
                        {
                            case 0:
                                WorldPnl.ImageLibraryIndexTexture = 0;
                                _starImageValue = 13;
                                break;
                            case 1:
                                WorldPnl.ImageLibraryIndexTexture = 10;
                                _starImageValue = 14;
                                break;
                            case 2:
                                WorldPnl.ImageLibraryIndexTexture = 11;
                                _starImageValue = 15;
                                break;
                        }

                        var campaign11 = GetCampaign();
                        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign11.CurrentScenarioId);
                        var campaign12 = GetCampaign();
                        if (IsEmpty(GetCampaignPage(campaignScenario5.OutroPages, campaign12.Page).ImageLibrary))
                        {
                            PicL.ImageLibrary = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign13.CurrentScenarioId);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page);
                            DialogHosts.UIGraphics.BookPL.LoadFromFile(campaignPage4.ImageLibrary);
                            PicL.ImageLibrary = DialogHosts.UIGraphics.BookPL;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign15.CurrentScenarioId);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page);
                            PicL.CurrentImage = campaignPage5.ImageIndex;
                            PicL.WinWidth = 0;
                            PicL.WinHeight = 0;
                            PicL.Visible = true;
                        }

                        var campaign17 = GetCampaign();
                        var campaignScenario8 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign17.CurrentScenarioId);
                        var campaign18 = GetCampaign();
                        if (IsEmpty(GetCampaignPage(campaignScenario8.OutroPages, campaign18.Page + 1).ImageLibrary))
                        {
                            PicR.ImageLibrary = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign13.CurrentScenarioId);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page + 1);
                            DialogHosts.UIGraphics.BookPR.LoadFromFile(campaignPage4.ImageLibrary);
                            PicR.ImageLibrary = DialogHosts.UIGraphics.BookPR;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign15.CurrentScenarioId);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page + 1);
                            PicR.CurrentImage = campaignPage5.ImageIndex;
                            PicR.WinWidth = 0;
                            PicR.WinHeight = 0;
                            PicR.Visible = true;
                        }

                        var campaign19 = GetCampaign();
                        Back.Visible = campaign19.Page > 0;
                        if (GetCampaign().Page == 8)
                        {
                            Next.Visible = false;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign13.CurrentScenarioId);
                            var campaign14 = GetCampaign();
                            bool test = !IsEmpty(GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page + 2).ImageLibrary);
                            if (!test)
                            {
                                var campaign15 = GetCampaign();
                                var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign15.CurrentScenarioId);
                                var campaign16 = GetCampaign();
                                test = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page + 2).Text.Count > 0;
                            }

                            Next.Visible = test;
                        }

                        BookStart.Visible = !Next.Visible;
                        Cancel.Visible = false;
                        var bookWindow = BookWin;
                        bookWindow.Hidden = true;
                        bookWindow.Visible = true;

                        SetBtn.Visible = false;
                        ShowBtn.Visible = false;
                        QuitBtn.Visible = false;
                        if (BookWin.BackSurface != null)
                        {
                            BookWin.BackSurface.Clear(BookWin._maskColor);
                        }

                        if (DialogHosts.TitleWindow.TitleWin.Visible)
                        {
                            DialogHosts.General.Manager.OverlayPriority = 249;
                        }
                    }
                    else
                    {
                        var campaign2 = GetCampaign();
                        campaign2.State += 1;
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
                    var campaignScenario9 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign20.CurrentScenarioId);
                    PPawn.WinLeft = campaignScenario9.MapX - 25;
                    var campaign21 = GetCampaign();
                    var campaignScenario10 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign21.CurrentScenarioId);
                    PPawn.WinTop = campaignScenario10.MapY - 65;
                    var campaign22 = GetCampaign();
                    var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign22.CurrentScenarioId).Paths[3];
                    index = GetCampaign().Scenarios.IndexOfId(path);
                    if (index != -1)
                    {
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        EPawn.WinLeft = campaignScenario.MapX - 25;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        EPawn.WinTop = campaignScenario2.MapY - 65;
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
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign23.CurrentScenarioId).Paths[0] > -1)
                    {
                        Path1.Visible = true;
                        var campaign2 = GetCampaign();
                        Path1.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[0];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path1.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path1.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path1.Visible = false;
                    }

                    var campaign24 = GetCampaign();
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign24.CurrentScenarioId).Paths[1] > -1)
                    {
                        Path2.Visible = true;
                        var campaign2 = GetCampaign();
                        Path2.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[1];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path2.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path2.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path2.Visible = false;
                    }

                    var campaign25 = GetCampaign();
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign25.CurrentScenarioId).Paths[2] > -1)
                    {
                        Path3.Visible = true;
                        var campaign2 = GetCampaign();
                        Path3.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[2];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path3.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path3.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path3.Visible = false;
                    }

                    if (!Path1.Visible && !Path2.Visible && !Path3.Visible)
                    {
                        DialogHosts.TitleWindow.TitlePnl.Visible = false;
                        DialogHosts.TitleWindow.TitlePnl.Redraw = false;
                        DialogHosts.TitleWindow.TitleWin.Visible = false;
                        DialogHosts.TaskBar.TaskWindow.Visible = false;
                        WorldPnl.Visible = false;
                        BookWin.Visible = false;
                        GameTables.GameEngine.CloseCampaign();
                        DialogHosts.TitleScene.ResetScene();
                        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
                    }

                    break;
                case 2:
                    var campaign26 = GetCampaign();
                    bool passed = !IsEmpty(GetCampaignPage(WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign26.NextScenarioId).IntroPages, 0).ImageLibrary);
                    if (!passed)
                    {
                        var campaign2 = GetCampaign();
                        passed = GetCampaignPage(WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.NextScenarioId).IntroPages, 0).Text.Count > 0;
                    }

                    if (passed)
                    {
                        SetBtn.Visible = false;
                        ShowBtn.Visible = false;
                        QuitBtn.Visible = false;
                        ShowIntroPages(GetCampaign().NextScenarioId);
                    }
                    else
                    {
                        _pawnTicks = 20;
                        BookWin.Visible = false;
                        SetBtn.Visible = false;
                        ShowBtn.Visible = false;
                        QuitBtn.Visible = false;
                        var campaign2 = GetCampaign();
                        campaign2.State += 1;
                        UpdateCampaignState();
                    }

                    break;
                case 3:
                    DialogHosts.CampaignTransferDialog.Execute();
                    SetBtn.Visible = false;
                    ShowBtn.Visible = false;
                    QuitBtn.Visible = false;
                    break;
                case 4:
                    var campaign27 = GetCampaign();
                    var campaignScenario11 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign27.NextScenarioId);
                    preview = (AoWHSMap)GameTables.GameEngine.CreatePreviewHsm(FileBoundary.ReadName(GameTables.GameEngine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario11.FileName), GameProfile.WorldMap);
                    if (preview != null)
                    {
                        index = preview.Players.IndexOfPlayerType(PlayerType.Human);
                        bool test = index != -1 && GetCampaign().RaceHistory.Count > 0;
                        if (test)
                        {
                            var player = preview.Players.GetPlayers(index);
                            test = GetCampaign().RaceHistory.IndexOf((byte)player.PlayerRace) == -1;
                        }

                        if (test && GetCampaign().TransferSettings.Leader.GetSphereCount() < GetCampaign().RaceHistory.Count * 2 + 3)
                        {
                            ShowSpheres();
                        }
                        else
                        {
                            var campaign2 = GetCampaign();
                            campaign2.State += 1;
                        }

                        preview?.Dispose();
                    }

                    break;
                case 7:
                    var campaign28 = GetCampaign();
                    if (!IsEmpty(WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign28.NextScenarioId).FileName))
                    {
                        var campaign2 = GetCampaign();
                        campaign2.State += 1;
                        var campaign3 = GetCampaign();
                        GetCampaign().StartScenario(campaign3.NextScenarioId, null);
                        SetCurrentDir(GameTables.GameEngine.StartupDirectory + "Scenario\\Campaign\\");
                        var campaign4 = GetCampaign();
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.CurrentScenarioId);
                        General.LoadGame(DialogHosts.General, FileBoundary.ReadName(campaignScenario.FileName));
                        DialogHosts.TitleWindow.TitlePnl.Visible = true;
                        WorldPnl.Visible = false;
                        GetCampaign().SetupScenario();
                        General.StartGame(DialogHosts.General);
                        DialogHosts.WorldMap.DescLbl.Text = "";
                    }
                    else
                    {
                        DialogHosts.WorldMap.DescLbl.Text = "";
                        GetCampaign().State = 0;
                        UpdateCampaignState();
                    }

                    break;
                case 10:
                    ShowIntroPages(GetCampaign().CurrentScenarioId);
                    break;
            }
        }
    }

    // AoWWorldMap.pas:347
    public void ShowIntroPages(int index)
    {
        SetCurrentDir(GameTables.GameEngine.StartupDirectory + "Int\\Scenes");
        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign = GetCampaign();
        var campaignPage = GetCampaignPage(campaignScenario.IntroPages, campaign.Page);
        LabelL.Text = campaignPage.Heading;
        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign2 = GetCampaign();
        var worldMapCampaignPage = GetCampaignPage(campaignScenario2.IntroPages, campaign2.Page + 1);
        LabelR.Text = worldMapCampaignPage.Heading;
        var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign3 = GetCampaign();
        var campaignPage2 = GetCampaignPage(campaignScenario3.IntroPages, campaign3.Page);
        MemoL.Items.Assign(campaignPage2.Text);
        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign4 = GetCampaign();
        var campaignPage3 = GetCampaignPage(campaignScenario4.IntroPages, campaign4.Page + 1);
        MemoR.Items.Assign(campaignPage3.Text);
        leftSB.Position = 0;
        rightSB.Position = 0;
        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign5 = GetCampaign();
        if (IsEmpty(GetCampaignPage(campaignScenario5.IntroPages, campaign5.Page).ImageLibrary))
        {
            PicL.ImageLibrary = null;
        }
        else
        {
            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign6 = GetCampaign();
            var campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page);
            DialogHosts.UIGraphics.BookPL.LoadFromFile(campaignPage4.ImageLibrary);
            PicL.ImageLibrary = DialogHosts.UIGraphics.BookPL;
            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign7 = GetCampaign();
            var campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page);
            PicL.CurrentImage = campaignPage5.ImageIndex;
            PicL.WinWidth = 0;
            PicL.WinHeight = 0;
            PicL.Visible = true;
        }

        var campaignScenario8 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign8 = GetCampaign();
        if (IsEmpty(GetCampaignPage(campaignScenario8.IntroPages, campaign8.Page + 1).ImageLibrary))
        {
            PicR.ImageLibrary = null;
        }
        else
        {
            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign6 = GetCampaign();
            var campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page + 1);
            DialogHosts.UIGraphics.BookPR.LoadFromFile(campaignPage4.ImageLibrary);
            PicR.ImageLibrary = DialogHosts.UIGraphics.BookPR;
            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign7 = GetCampaign();
            var campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page + 1);
            PicR.CurrentImage = campaignPage5.ImageIndex;
            PicR.WinWidth = 0;
            PicR.WinHeight = 0;
            PicR.Visible = true;
        }

        var campaign9 = GetCampaign();
        Back.Visible = campaign9.Page > 0;
        if (GetCampaign().Page == 8)
        {
            Next.Visible = false;
        }
        else
        {
            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign6 = GetCampaign();
            bool condition = !IsEmpty(GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page + 2).ImageLibrary);
            if (!condition)
            {
                var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                var campaign7 = GetCampaign();
                condition = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page + 2).Text.Count > 0;
            }

            Next.Visible = condition;
        }

        bool test = !Next.Visible || GetCampaign().State == 10;
        BookStart.Visible = test;
        Cancel.Visible = WorldPnl.Visible;
        var bookWindow = BookWin;
        bookWindow.Hidden = true;
        bookWindow.Visible = true;

        if (BookWin.BackSurface != null)
        {
            BookWin.BackSurface.Clear(BookWin._maskColor);
        }

        if (DialogHosts.TitleWindow.TitleWin.Visible)
        {
            DialogHosts.General.Manager.OverlayPriority = 249;
        }
    }

    // AoWWorldMap.pas:404
    public void ShowSpheres()
    {
        int picks = default;
        LeaderSetupControl control = default;
        WorldPnl.Visible = false;
        picks = GetCampaign().TransferSettings.Leader.SpherePicksMax;
        control = new LeaderSetupControl(null);
        var campaign = GetCampaign();
        control.Leader.Assign(campaign.TransferSettings.Leader, GameTables.GameEngine);
        control.Leader.SetSpherePicksMax((byte)(picks + 2));
        control.OriginalLeader.Assign(control.Leader, GameTables.GameEngine);
        control.Mode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres);
        DialogHosts.LeaderSetupWindow.Execute(control);
        DialogHosts.LeaderSetupWindow.ClearBtn.Visible = false;
        DialogHosts.LeaderSetupWindow.DefaultBtn.Visible = false;
        DialogHosts.LeaderSetupWindow.PrevBtn.Visible = false;
        DialogHosts.LeaderSetupWindow.SetFixedSpheres(picks);
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_assignNewSpheresFormat), [IntToStr(2)]);
        DialogHosts.LeaderSetupWindow.Phase3Header.Text = format;
        control.OnDone = SpheresDone;
        control.Release();
        DialogHosts.LeaderSetupWindow.LeaderPnl.ImageLibraryTexture = DialogHosts.UIGraphics.WorldMap;
        DialogHosts.LeaderSetupWindow.LeaderPnl.ImageLibraryIndexTexture = 0;
    }

    // AoWWorldMap.pas:386
    public void SpheresDone(LeaderSetupControl sender)
    {
        Leader leader;
        var leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            leader = (Leader)leaderSetupControl.OriginalLeader.Copy(GameTables.GameEngine);
            for (int I = GetCampaign().TransferSettings.Leader.GetSphereCount(), last = leader.GetSphereCount() - 1; I <= last; I++)
            {
                var sphere = leader.GetSpheres(I);
                GetCampaign().TransferSettings.SelectedSpheres.Add((byte)(int)sphere);
            }

            WorldPnl.Visible = true;
            var campaign = GetCampaign();
            campaign.State += 1;
            UpdateCampaignState();
        }
    }

    // AoWWorldMap.pas:428
    public void BookClosed()
    {
        NotifyEvent<object> done;
        EngineObject context;
        done = OnBookClosed;
        context = BookContext;
        OnBookClosed = null;
        BookContext = null;
        if (done != null)
        {
            done(context);
        }

        if (context != null)
        {
            context.Release();
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
    }

    // AoWWorldMap.pas:146
    public void ResetCampaign()
    {
        GetCampaign().State = 0;
        GetCampaign().Page = 0;
        InfoPnl.Visible = false;
        var showInfo = AbilityRules.TranslateResourceString(s_showInfoText);
        ShowBtn.Text = showInfo;
    }

    // AoWWorldMap.pas:151
    public void ShowCampaignHistory()
    {
        WorldMapCampaignScenario scenario;
        GetCampaign().State = 10;
        GetCampaign().Page = 0;
        var campaign = GetCampaign();
        scenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId);
        while (true)
        {
            bool condition = scenario.IntroPages.Count > GetCampaign().Page + 2;
            if (condition)
            {
                var campaign2 = GetCampaign();
                bool test = !IsEmpty(GetCampaignPage(scenario.IntroPages, campaign2.Page + 2).ImageLibrary);
                if (!test)
                {
                    var campaign3 = GetCampaign();
                    test = GetCampaignPage(scenario.IntroPages, campaign3.Page + 2).Text.Count > 0;
                }

                condition = test;
            }

            if (!condition)
                break;
            var campaign4 = GetCampaign();
            campaign4.Page += 2;
        }

        ShowIntroPages(GetCampaign().CurrentScenarioId);
    }

    // AoWWorldMap.pas:442
    public void BackClick()
    {
        Back.CaptureInput = false;
        Next.CaptureInput = false;
        var button = BookStart;
        button.CaptureInput = false;
        button.Visible = false;

        this.BookWin.Drawn = false;
        if (this.BookWin.BackSurface != null)
        {
            this.BookWin.BackSurface.Clear(this.BookWin._maskColor);
        }

        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 1;
    }

    // AoWWorldMap.pas:863
    public void WorldMapCreate()
    {
        RegisterWorldMapCampaign(GameTables.GameEngine);
    }

    // AoWWorldMap.pas:461
    public void NextStartClick()
    {
        Back.CaptureInput = false;
        Next.CaptureInput = false;
        BookStart.CaptureInput = false;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 2;
    }

    // AoWWorldMap.pas:471
    public void WorldPnlUpdate()
    {
        int frame;
        int startIndex;
        int endIndex;
        if (WorldPnl.Visible && !BookWin.Visible)
        {
            Path1.Drawn = false;
            Path2.Drawn = false;
            Path3.Drawn = false;
            PPawn.Drawn = false;
            EPawn.Drawn = false;
            _pathDelay -= 1;
            if (_pathDelay < 1)
            {
                _pathDelay = 5;
                frame = 1 - _pathFrame;
                _pathFrame = frame;
                var campaign = GetCampaign();
                Path1.CurrentImage = campaign.Faction * 3 + (frame + 2);
                var campaign2 = GetCampaign();
                Path2.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                var campaign3 = GetCampaign();
                Path3.CurrentImage = campaign3.Faction * 3 + (_pathFrame + 2);
            }

            if (GetCampaign().State == 5)
            {
                if (_pawnTicks > 0)
                {
                    var campaign = GetCampaign();
                    var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.NextScenarioId);
                    var campaign2 = GetCampaign();
                    var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.CurrentScenarioId);
                    var campaign3 = GetCampaign();
                    var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId);
                    PPawn.WinLeft = (campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19 + campaignScenario3.MapX - 25;
                    var campaign4 = GetCampaign();
                    var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.NextScenarioId);
                    var campaign5 = GetCampaign();
                    var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign5.CurrentScenarioId);
                    var campaign6 = GetCampaign();
                    var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign6.CurrentScenarioId);
                    PPawn.WinTop = (campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19 + campaignScenario6.MapY - 65;
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
                    _pawnTicks = 20;
                    _enemyDelay = 30;
                    var campaign = GetCampaign();
                    campaign.State += 1;
                    var campaign2 = GetCampaign();
                    var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.NextScenarioId);
                    if (DescLbl.Text != campaignScenario.Description)
                    {
                        var campaign3 = GetCampaign();
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.NextScenarioId);
                        DescLbl.Text = campaignScenario2.Description;
                        this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;

                        _descriptionHandled = true;
                    }
                }

                _pawnTicks -= 1;
            }

            if (GetCampaign().State == 6)
            {
                if (_pawnTicks > 0)
                {
                    _descriptionHandled = true;
                    var campaign = GetCampaign();
                    var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId).Paths[3];
                    startIndex = GetCampaign().Scenarios.IndexOfId(path);
                    var campaign2 = GetCampaign();
                    var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.NextScenarioId).Paths[3];
                    endIndex = GetCampaign().Scenarios.IndexOfId(path2);
                    if (startIndex != -1 && endIndex != -1)
                    {
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), endIndex);
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        EPawn.WinLeft = (campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19 + campaignScenario3.MapX - 25;
                        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), endIndex);
                        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        EPawn.WinTop = (campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19 + campaignScenario6.MapY - 65;
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
                    if (_enemyDelay > 0)
                    {
                        _enemyDelay -= 1;
                    }
                    else
                    {
                        var campaign = GetCampaign();
                        campaign.State += 1;
                        UpdateCampaignState();
                    }
                }

                _pawnTicks -= 1;
            }
        }
    }

    // AoWWorldMap.pas:566
    public void Path1MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().State == 1)
            {
                DescLbl.Text = "";
                SelectCampaignPath(GetCampaign(), 0);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:578
    public void Path2MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().State == 1)
            {
                DescLbl.Text = "";
                SelectCampaignPath(GetCampaign(), 1);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:590
    public void Path3MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!BookWin.Visible)
        {
            if (GetCampaign().State == 1)
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
        GetCampaign().State = 1;
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        UpdateCampaignState();
    }

    // AoWWorldMap.pas:611. Bound to the player pawn.
    public void PPawnMouseDown(Image sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (X > 20)
        {
            if (GetCampaign().State == 1)
            {
                GetCampaign().State = 0;
                GetCampaign().Page = 0;
                UpdateCampaignState();
            }
        }

        // { Original hit test uses X. }
        sender.Handled = false;
    }

    // AoWWorldMap.pas:611. Bound to the enemy pawn.
    public void EPawnMouseDown(Image sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        sender.Handled = false;
    }

    // AoWWorldMap.pas:624. Bound to the player pawn.
    public void PPawnMouseMove(Image sender, Set<ShiftState> shift, int X, int Y)
    {
        string description = "";
        // { Original hit test uses X. }
        if (X > 20)
        {
            if (GetCampaign().State == 1)
            {
                description = AbilityRules.TranslateResourceString(s_viewSummaryText);
            }
        }

        ShowPawnDescription(sender, description);
    }

    // AoWWorldMap.pas:624. Bound to the enemy pawn.
    public void EPawnMouseMove(Image sender, Set<ShiftState> shift, int X, int Y)
    {
        string description = "";
        // { Original hit test uses X. }
        if (X > 20)
        {
            if (GetCampaign().State == 1)
            {
                if (GetCampaign().Faction == 0)
                {
                    description = AbilityRules.TranslateResourceString(LobbyText.CultOfStormsText);
                }
                else
                {
                    description = AbilityRules.TranslateResourceString(LobbyText.KeepersText);
                }
            }
        }

        ShowPawnDescription(sender, description);
    }

    // AoWWorldMap.pas:624. The label update both pawn moves share.
    private void ShowPawnDescription(Image pawn, string description)
    {
        if (DescLbl.Text != description && !IsEmpty(description))
        {
            DescLbl.Text = description;
            this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
        }

        _descriptionHandled = true;
        pawn.Handled = false;
    }

    // AoWWorldMap.pas:643
    public void Path1MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        int index;
        if (GetCampaign().State == 1)
        {
            var campaign = GetCampaign();
            var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId).Paths[0];
            index = GetCampaign().Scenarios.IndexOfId(path);
            var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            if (DescLbl.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescLbl.Text = campaignScenario2.PathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:661
    public void Path2MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        int index;
        if (GetCampaign().State == 1)
        {
            var campaign = GetCampaign();
            var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId).Paths[1];
            index = GetCampaign().Scenarios.IndexOfId(path);
            var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            if (DescLbl.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescLbl.Text = campaignScenario2.PathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:679
    public void Path3MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        int index;
        if (GetCampaign().State == 1)
        {
            var campaign = GetCampaign();
            var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign.CurrentScenarioId).Paths[2];
            index = GetCampaign().Scenarios.IndexOfId(path);
            var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            if (DescLbl.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescLbl.Text = campaignScenario2.PathDescription;
                this.DescLbl.WinTop = 460 - this.DescLbl.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:697
    public void WorldPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!_descriptionHandled)
        {
            if (GetCampaign().State == 1)
            {
                if (!IsEmpty(this.DescLbl.Text))
                {
                    this.DescLbl.Text = "";
                }
            }
        }

        _descriptionHandled = false;
    }

    // AoWWorldMap.pas:706
    public void BookStartClick()
    {
        BookWin.Visible = false;
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        if (!WorldPnl.Visible)
        {
            DialogHosts.General.Manager.OverlayPriority = 0;
        }

        if (WorldPnl.Visible)
        {
            _pawnTicks = 20;
            var campaign = GetCampaign();
            campaign.State += 1;
            UpdateCampaignState();
        }
    }

    // AoWWorldMap.pas:720
    public void BookWinAnimDone()
    {
        Back.CaptureInput = true;
        Next.CaptureInput = true;
        BookStart.CaptureInput = true;
        BookWin.Hidden = true;
    }

    // AoWWorldMap.pas:729
    public void BookWinShow()
    {
        BookAction = 0;
    }

    // AoWWorldMap.pas:733
    public void BookWinUpdate()
    {
        Rect R;
        switch (BookAction)
        {
            case 3:
                BookWin.StartAnimation((PageTurnMode)1);
                var campaign = GetCampaign();
                campaign.Page -= 2;
                UpdateCampaignState();
                BookAction = 0;
                break;
            case 4:
                BookWin.StartAnimation(0);
                var campaign2 = GetCampaign();
                campaign2.Page += 2;
                UpdateCampaignState();
                BookAction = 0;
                break;
            case 1:
                BookAction = 3;
                break;
            case 2:
                BookAction = 4;
                break;
        }

        if (BookStart.Visible != s_bookStartVisible)
        {
            R = new Rect(BookWin.WinLeft + BookStart.WinLeft, BookWin.WinTop + BookStart.WinTop, BookWin.WinLeft + BookStart.WinLeft + BookStart.WinWidth, BookWin.WinTop + BookStart.WinTop + BookStart.WinHeight);
            DialogHosts.General.Manager.AddDrawRectangle(ref R, BookWin);
            s_bookStartVisible = BookStart.Visible;
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
            var showInfo = AbilityRules.TranslateResourceString(s_showInfoText);
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
            var hideInfo = AbilityRules.TranslateResourceString(s_hideInfoText);
            ShowBtn.Text = hideInfo;
        }
    }

    // AoWWorldMap.pas:786
    public void QuitBtnClick()
    {
        var quitCampaign = AbilityRules.TranslateResourceString(s_quitCampaignText);
        var confirm = AbilityRules.TranslateResourceString(LobbyText.ConfirmText);
        DialogHosts.ConfirmationDialog.Confirm(confirm, quitCampaign, 5);
    }

    // AoWWorldMap.pas:791
    public void StarImgUpdate()
    {
        _starIntensity += _starStep;
        if (_starIntensity + _starStep > 100)
        {
            _starStep = -1;
        }

        if (_starIntensity + _starStep < 50)
        {
            _starStep = 1;
        }

        StarImg.Drawn = false;
    }

    // AoWWorldMap.pas:800
    public void StarImgDraw(LockedSurface surface)
    {
        DialogHosts.UIGraphics.WorldMap[_starImageValue].ShowBlended(WorldPnl.WinLeft + StarImg.WinLeft, WorldPnl.WinTop + StarImg.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_starIntensity]);
    }

    // AoWWorldMap.pas:806
    public void Ally1PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPnl.WinLeft + Ally1Pnl.WinLeft, InfoPnl.WinTop + Ally1Pnl.WinTop, InfoPnl.WinLeft + Ally1Pnl.WinLeft + Ally1Pnl.WinWidth, InfoPnl.WinTop + Ally1Pnl.WinTop + Ally1Pnl.WinHeight);
        if (GetCampaign().RaceHistory.Count > 0)
        {
            var raceHistory = GetCampaign().RaceHistory[0];
            AoWHSSet.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:818
    public void Ally2PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPnl.WinLeft + Ally2Pnl.WinLeft, InfoPnl.WinTop + Ally2Pnl.WinTop, InfoPnl.WinLeft + Ally2Pnl.WinLeft + Ally2Pnl.WinWidth, InfoPnl.WinTop + Ally2Pnl.WinTop + Ally2Pnl.WinHeight);
        if (GetCampaign().RaceHistory.Count > 1)
        {
            var raceHistory = GetCampaign().RaceHistory[1];
            AoWHSSet.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:830
    public void Ally3PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPnl.WinLeft + Ally3Pnl.WinLeft, InfoPnl.WinTop + Ally3Pnl.WinTop, InfoPnl.WinLeft + Ally3Pnl.WinLeft + Ally3Pnl.WinWidth, InfoPnl.WinTop + Ally3Pnl.WinTop + Ally3Pnl.WinHeight);
        if (GetCampaign().RaceHistory.Count > 2)
        {
            var raceHistory = GetCampaign().RaceHistory[2];
            AoWHSSet.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:842
    public void HeroPnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPnl.WinLeft + HeroPnl.WinLeft, InfoPnl.WinTop + HeroPnl.WinTop, InfoPnl.WinLeft + HeroPnl.WinLeft + HeroPnl.WinWidth, InfoPnl.WinTop + HeroPnl.WinTop + HeroPnl.WinHeight);
        if (GetCampaign().TransferSettings.Leader != null)
        {
            GetCampaign().TransferSettings.Leader.ShowFace(R, surface);
        }
    }

    // AoWWorldMap.pas:854
    public void SetBtnClick()
    {
        DialogHosts.CampaignSettingsDialog.Dlg.Visible = true;
    }

    // AoWWorldMap.pas:859
    public void BookWinHide()
    {
        BookClosed();
    }

    private static readonly string s_assignNewSpheresFormat = "Assign %s new spheres!";

    private static void RegisterWorldMapCampaign(GameEngine engine)
    {
        engine.RegisterEngineClasses([EngineClass.Of<CampaignScenarioPage>(), EngineClass.Of<WorldMapCampaignScenario>(), EngineClass.Of<WorldMapCampaign>()]);
    }

    private static CampaignScenarioPage GetCampaignPage(WMCampaignScenarioPageList pages, int index) => pages[index];

    private static bool s_bookStartVisible;

    private static void SelectCampaignPath(WorldMapCampaign campaign, int index)
    {
        if (index >= 0 && index < 3)
        {
            campaign.State = 2;
            campaign.Page = 0;
            var path = WorldMapCampaign.GetCampaignScenario(campaign, campaign.CurrentScenarioId).Paths[index];
            campaign.SelectNextScenario(campaign.Scenarios.IndexOfId(path));
            if (WorldMapCampaign.GetCampaignScenario(campaign, campaign.NextScenarioId).Paths[3] == -1)
            {
                var path2 = WorldMapCampaign.GetCampaignScenario(campaign, campaign.CurrentScenarioId).Paths[3];
                WorldMapCampaign.GetCampaignScenario(campaign, campaign.NextScenarioId).Paths[3] = path2;
            }
        }
    }

    private static readonly string s_hideInfoText = "Hide Info";

    private static readonly string s_showInfoText = "Show Info";

    private static readonly string s_viewSummaryText = "View Summary";

    private static readonly string s_quitCampaignText = "Quit the Campaign?";

    internal static void ShowCampaignMap(WorldMap dialog)
    {
        string song = "";
        dialog.ResetCampaign();
        GameTables.GameEngine.CloseHsm(null);
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        DialogHosts.TitleWindow.TitlePnl.Visible = false;
        dialog.WorldPnl.Visible = true;
        dialog._pathFrame = 0;
        dialog._pathDelay = 5;
        dialog.DescLbl.Text = "";
        DialogHosts.General.Manager.OverlayPriority = 249;
        AudioHost.SongPlayer.Stop(false);
        SetCurrentDir(GameTables.GameEngine.StartupDirectory);
        var campaign = dialog.GetCampaign();
        if (IsEmpty(WorldMapCampaign.GetCampaignScenario(dialog.GetCampaign(), campaign.CurrentScenarioId).ImageLibrary))
        {
            song = "Songs\\Title.IT";
        }
        else
        {
            var dialogCampaign = dialog.GetCampaign();
            song = "Songs\\" + WorldMapCampaign.GetCampaignScenario(dialog.GetCampaign(), dialogCampaign.CurrentScenarioId).ImageLibrary;
        }

        AudioPlayback.PlaySong(song, 1, 0, true);
        var campaign2 = dialog.GetCampaign();
        dialog.PPawn.CurrentImage = campaign2.Faction + 7;
        var campaign3 = dialog.GetCampaign();
        dialog.EPawn.CurrentImage = 8 - campaign3.Faction;
        dialog.UpdateCampaignState();
    }

}
