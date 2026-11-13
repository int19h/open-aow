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

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// star image, brightness, path animation, pawn timing, hover, context, completion event, book
// action
public partial class WorldMap : UIModule
{
    public WorldMap(UIContext ui) : base(ui)
    { }

    public Memo LeftPageMemo;
    public Image RightPagePicture;
    public Panel WorldPanel;
    public Image Path1Image;
    public Image Path2Image;
    public Image Path3Image;
    public Image PlayerPawnImage;
    public Image EnemyPawnImage;
    public Label DescriptionLabel;
    public Button BackButton;
    public Button NextButton;
    public Button CancelButton;
    public Button BookStartButton;
    public Label LeftPageLabel;
    public Memo RightPageMemo;
    public Label RightPageLabel;
    public Image LeftPagePicture;
    public BookWindow Book;
    public Panel InfoPanel;
    public Button QuitButton;
    public Button ShowButton;
    public Label SelectLabel;
    public Image StarImage;
    public Frame HeroFrame;
    public Frame Ally1Frame;
    public Frame Ally2Frame;
    public Frame Ally3Frame;
    public Button SetButton;
    public VerticalScrollBar LeftPageScrollBar;
    public VerticalScrollBar RightPageScrollBar;
    private int _starImageValue;
    private int _starStep;
    private int _starIntensity;
    private int _pathFrame;
    private int _pathDelay;
    private int _pawnTicks;
    private int _enemyDelay;
    private bool _descriptionHandled;

    public EngineObject BookContext { get; set; }

    public event NotifyEvent<object> OnBookClosed;

    internal void SetBookClosedHandler(NotifyEvent<object> handler) => OnBookClosed = handler;

    public byte BookAction { get; set; }

    // AoWWorldMap.pas:152
    public WorldMapCampaign GetCampaign() => (WorldMapCampaign)UI.Engine.Campaign;

    // AoWWorldMap.pas:157
    public void UpdateCampaignState()
    {
        int index = default;
        StrategicMap preview = default;
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
                        SetCurrentDir(UI.Engine.StartupDirectory + "\\Int\\Scenes");
                        var campaign2 = GetCampaign();
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign2.CurrentScenarioId);
                        var campaign3 = GetCampaign();
                        var campaignPage = GetCampaignPage(campaignScenario.OutroPages, campaign3.Page);
                        LeftPageLabel.Text = campaignPage.Heading;
                        var campaign4 = GetCampaign();
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.CurrentScenarioId);
                        var campaign5 = GetCampaign();
                        var worldMapCampaignPage = GetCampaignPage(campaignScenario2.OutroPages, campaign5.Page + 1);
                        RightPageLabel.Text = worldMapCampaignPage.Heading;
                        var campaign6 = GetCampaign();
                        var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign6.CurrentScenarioId);
                        var campaign7 = GetCampaign();
                        var campaignPage2 = GetCampaignPage(campaignScenario3.OutroPages, campaign7.Page);
                        LeftPageMemo.Items.Assign(campaignPage2.Text);
                        var campaign8 = GetCampaign();
                        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign8.CurrentScenarioId);
                        var campaign9 = GetCampaign();
                        var campaignPage3 = GetCampaignPage(campaignScenario4.OutroPages, campaign9.Page + 1);
                        RightPageMemo.Items.Assign(campaignPage3.Text);
                        _starIntensity = 50;
                        _starStep = 1;
                        var campaign10 = GetCampaign();
                        switch (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign10.CurrentScenarioId).MapKind)
                        {
                            case 0:
                                WorldPanel.ImageLibraryIndexTexture = 0;
                                _starImageValue = 13;
                                break;
                            case 1:
                                WorldPanel.ImageLibraryIndexTexture = 10;
                                _starImageValue = 14;
                                break;
                            case 2:
                                WorldPanel.ImageLibraryIndexTexture = 11;
                                _starImageValue = 15;
                                break;
                        }

                        var campaign11 = GetCampaign();
                        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign11.CurrentScenarioId);
                        var campaign12 = GetCampaign();
                        if (IsEmpty(GetCampaignPage(campaignScenario5.OutroPages, campaign12.Page).ImageLibrary))
                        {
                            LeftPagePicture.ImageLibrary = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign13.CurrentScenarioId);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page);
                            UI.Graphics.BookLeftPage.LoadFromFile(campaignPage4.ImageLibrary);
                            LeftPagePicture.ImageLibrary = UI.Graphics.BookLeftPage;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign15.CurrentScenarioId);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page);
                            LeftPagePicture.CurrentImage = campaignPage5.ImageIndex;
                            LeftPagePicture.WinWidth = 0;
                            LeftPagePicture.WinHeight = 0;
                            LeftPagePicture.Visible = true;
                        }

                        var campaign17 = GetCampaign();
                        var campaignScenario8 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign17.CurrentScenarioId);
                        var campaign18 = GetCampaign();
                        if (IsEmpty(GetCampaignPage(campaignScenario8.OutroPages, campaign18.Page + 1).ImageLibrary))
                        {
                            RightPagePicture.ImageLibrary = null;
                        }
                        else
                        {
                            var campaign13 = GetCampaign();
                            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign13.CurrentScenarioId);
                            var campaign14 = GetCampaign();
                            var campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page + 1);
                            UI.Graphics.BookRightPage.LoadFromFile(campaignPage4.ImageLibrary);
                            RightPagePicture.ImageLibrary = UI.Graphics.BookRightPage;
                            var campaign15 = GetCampaign();
                            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign15.CurrentScenarioId);
                            var campaign16 = GetCampaign();
                            var campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page + 1);
                            RightPagePicture.CurrentImage = campaignPage5.ImageIndex;
                            RightPagePicture.WinWidth = 0;
                            RightPagePicture.WinHeight = 0;
                            RightPagePicture.Visible = true;
                        }

                        var campaign19 = GetCampaign();
                        BackButton.Visible = campaign19.Page > 0;
                        if (GetCampaign().Page == 8)
                        {
                            NextButton.Visible = false;
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

                            NextButton.Visible = test;
                        }

                        BookStartButton.Visible = !NextButton.Visible;
                        CancelButton.Visible = false;
                        var bookWindow = Book;
                        bookWindow.Hidden = true;
                        bookWindow.Visible = true;

                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        if (Book.BackSurface != null)
                        {
                            Book.BackSurface.Clear(Book._maskColor);
                        }

                        if (UI.TitleWindow.MainWindow.Visible)
                        {
                            UI.General.Manager.OverlayPriority = 249;
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
                    SetButton.Visible = true;
                    ShowButton.Visible = true;
                    QuitButton.Visible = true;
                    Book.Visible = false;
                    PlayerPawnImage.Visible = true;
                    EnemyPawnImage.Visible = true;
                    var campaign20 = GetCampaign();
                    var campaignScenario9 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign20.CurrentScenarioId);
                    PlayerPawnImage.WinLeft = campaignScenario9.MapX - 25;
                    var campaign21 = GetCampaign();
                    var campaignScenario10 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign21.CurrentScenarioId);
                    PlayerPawnImage.WinTop = campaignScenario10.MapY - 65;
                    var campaign22 = GetCampaign();
                    var path = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign22.CurrentScenarioId).Paths[3];
                    index = GetCampaign().Scenarios.IndexOfId(path);
                    if (index != -1)
                    {
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        EnemyPawnImage.WinLeft = campaignScenario.MapX - 25;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        EnemyPawnImage.WinTop = campaignScenario2.MapY - 65;
                    }

                    {
                        if (this.PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                        {
                            this.PlayerPawnImage.Priority = 10;
                            EnemyPawnImage.Priority = 5;
                        }
                        else
                        {
                            this.PlayerPawnImage.Priority = 5;
                            EnemyPawnImage.Priority = 10;
                        }
                    }

                    var campaign23 = GetCampaign();
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign23.CurrentScenarioId).Paths[0] > -1)
                    {
                        Path1Image.Visible = true;
                        var campaign2 = GetCampaign();
                        Path1Image.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[0];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path1Image.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path1Image.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path1Image.Visible = false;
                    }

                    var campaign24 = GetCampaign();
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign24.CurrentScenarioId).Paths[1] > -1)
                    {
                        Path2Image.Visible = true;
                        var campaign2 = GetCampaign();
                        Path2Image.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[1];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path2Image.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path2Image.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path2Image.Visible = false;
                    }

                    var campaign25 = GetCampaign();
                    if (WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign25.CurrentScenarioId).Paths[2] > -1)
                    {
                        Path3Image.Visible = true;
                        var campaign2 = GetCampaign();
                        Path3Image.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                        var campaign3 = GetCampaign();
                        var path2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.CurrentScenarioId).Paths[2];
                        index = GetCampaign().Scenarios.IndexOfId(path2);
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path3Image.WinLeft = campaignScenario.MapX;
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                        Path3Image.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path3Image.Visible = false;
                    }

                    if (!Path1Image.Visible && !Path2Image.Visible && !Path3Image.Visible)
                    {
                        UI.TitleWindow.TitlePanel.Visible = false;
                        UI.TitleWindow.TitlePanel.Redraw = false;
                        UI.TitleWindow.MainWindow.Visible = false;
                        UI.TaskBar.TaskWindow.Visible = false;
                        WorldPanel.Visible = false;
                        Book.Visible = false;
                        UI.Engine.CloseCampaign();
                        UI.TitleScene.ResetScene();
                        WindowActions.ShowTitleScene(UI.TitleScene, true);
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
                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        ShowIntroPages(GetCampaign().NextScenarioId);
                    }
                    else
                    {
                        _pawnTicks = 20;
                        Book.Visible = false;
                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        var campaign2 = GetCampaign();
                        campaign2.State += 1;
                        UpdateCampaignState();
                    }

                    break;
                case 3:
                    UI.CampaignTransferDialog.Execute();
                    SetButton.Visible = false;
                    ShowButton.Visible = false;
                    QuitButton.Visible = false;
                    break;
                case 4:
                    var campaign27 = GetCampaign();
                    var campaignScenario11 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign27.NextScenarioId);
                    preview = (StrategicMap)UI.Engine.CreatePreviewHsm(FileBoundary.ReadName(UI.Engine.StartupDirectory + "Scenario\\Campaign\\" + campaignScenario11.FileName), GameProfile.WorldMap);
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
                        SetCurrentDir(UI.Engine.StartupDirectory + "Scenario\\Campaign\\");
                        var campaign4 = GetCampaign();
                        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.CurrentScenarioId);
                        General.LoadGame(UI.General, FileBoundary.ReadName(campaignScenario.FileName));
                        UI.TitleWindow.TitlePanel.Visible = true;
                        WorldPanel.Visible = false;
                        GetCampaign().SetupScenario();
                        General.StartGame(UI.General);
                        UI.WorldMap.DescriptionLabel.Text = "";
                    }
                    else
                    {
                        UI.WorldMap.DescriptionLabel.Text = "";
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
        SetCurrentDir(UI.Engine.StartupDirectory + "Int\\Scenes");
        var campaignScenario = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign = GetCampaign();
        var campaignPage = GetCampaignPage(campaignScenario.IntroPages, campaign.Page);
        LeftPageLabel.Text = campaignPage.Heading;
        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign2 = GetCampaign();
        var worldMapCampaignPage = GetCampaignPage(campaignScenario2.IntroPages, campaign2.Page + 1);
        RightPageLabel.Text = worldMapCampaignPage.Heading;
        var campaignScenario3 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign3 = GetCampaign();
        var campaignPage2 = GetCampaignPage(campaignScenario3.IntroPages, campaign3.Page);
        LeftPageMemo.Items.Assign(campaignPage2.Text);
        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign4 = GetCampaign();
        var campaignPage3 = GetCampaignPage(campaignScenario4.IntroPages, campaign4.Page + 1);
        RightPageMemo.Items.Assign(campaignPage3.Text);
        LeftPageScrollBar.Position = 0;
        RightPageScrollBar.Position = 0;
        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign5 = GetCampaign();
        if (IsEmpty(GetCampaignPage(campaignScenario5.IntroPages, campaign5.Page).ImageLibrary))
        {
            LeftPagePicture.ImageLibrary = null;
        }
        else
        {
            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign6 = GetCampaign();
            var campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page);
            UI.Graphics.BookLeftPage.LoadFromFile(campaignPage4.ImageLibrary);
            LeftPagePicture.ImageLibrary = UI.Graphics.BookLeftPage;
            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign7 = GetCampaign();
            var campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page);
            LeftPagePicture.CurrentImage = campaignPage5.ImageIndex;
            LeftPagePicture.WinWidth = 0;
            LeftPagePicture.WinHeight = 0;
            LeftPagePicture.Visible = true;
        }

        var campaignScenario8 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
        var campaign8 = GetCampaign();
        if (IsEmpty(GetCampaignPage(campaignScenario8.IntroPages, campaign8.Page + 1).ImageLibrary))
        {
            RightPagePicture.ImageLibrary = null;
        }
        else
        {
            var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign6 = GetCampaign();
            var campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page + 1);
            UI.Graphics.BookRightPage.LoadFromFile(campaignPage4.ImageLibrary);
            RightPagePicture.ImageLibrary = UI.Graphics.BookRightPage;
            var campaignScenario7 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
            var campaign7 = GetCampaign();
            var campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page + 1);
            RightPagePicture.CurrentImage = campaignPage5.ImageIndex;
            RightPagePicture.WinWidth = 0;
            RightPagePicture.WinHeight = 0;
            RightPagePicture.Visible = true;
        }

        var campaign9 = GetCampaign();
        BackButton.Visible = campaign9.Page > 0;
        if (GetCampaign().Page == 8)
        {
            NextButton.Visible = false;
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

            NextButton.Visible = condition;
        }

        bool test = !NextButton.Visible || GetCampaign().State == 10;
        BookStartButton.Visible = test;
        CancelButton.Visible = WorldPanel.Visible;
        var bookWindow = Book;
        bookWindow.Hidden = true;
        bookWindow.Visible = true;

        if (Book.BackSurface != null)
        {
            Book.BackSurface.Clear(Book._maskColor);
        }

        if (UI.TitleWindow.MainWindow.Visible)
        {
            UI.General.Manager.OverlayPriority = 249;
        }
    }

    // AoWWorldMap.pas:404
    public void ShowSpheres()
    {
        int picks = default;
        LeaderSetupControl control = default;
        WorldPanel.Visible = false;
        picks = GetCampaign().TransferSettings.Leader.SpherePicksMax;
        control = new LeaderSetupControl(null) { Holder = UI.Engine };
        var campaign = GetCampaign();
        control.Leader.Assign(campaign.TransferSettings.Leader, UI.Engine);
        control.Leader.SetSpherePicksMax((byte)(picks + 2));
        control.OriginalLeader.Assign(control.Leader, UI.Engine);
        control.Mode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres);
        UI.LeaderSetupWindow.Execute(control);
        UI.LeaderSetupWindow.ClearButton.Visible = false;
        UI.LeaderSetupWindow.DefaultButton.Visible = false;
        UI.LeaderSetupWindow.PreviousButton.Visible = false;
        UI.LeaderSetupWindow.SetFixedSpheres(picks);
        var format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_assignNewSpheresFormat), [IntToStr(2)]);
        UI.LeaderSetupWindow.Phase3Header.Text = format;
        control.OnDone += SpheresDone;
        control.Release();
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryTexture = UI.Graphics.WorldMap;
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryIndexTexture = 0;
    }

    // AoWWorldMap.pas:386
    public void SpheresDone(LeaderSetupControl sender)
    {
        Leader leader;
        var leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            leader = (Leader)leaderSetupControl.OriginalLeader.Copy(UI.Engine);
            for (int I = GetCampaign().TransferSettings.Leader.GetSphereCount(), last = leader.GetSphereCount() - 1; I <= last; I++)
            {
                var sphere = leader.GetSpheres(I);
                GetCampaign().TransferSettings.SelectedSpheres.Add((byte)(int)sphere);
            }

            WorldPanel.Visible = true;
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

        UI.General.Manager.OverlayPriority = 0;
    }

    // AoWWorldMap.pas:146
    public void ResetCampaign()
    {
        GetCampaign().State = 0;
        GetCampaign().Page = 0;
        InfoPanel.Visible = false;
        var showInfo = UI.Engine.TranslateResourceString(s_showInfoText);
        ShowButton.Text = showInfo;
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
        BackButton.CaptureInput = false;
        NextButton.CaptureInput = false;
        var button = BookStartButton;
        button.CaptureInput = false;
        button.Visible = false;

        this.Book.Drawn = false;
        if (this.Book.BackSurface != null)
        {
            this.Book.BackSurface.Clear(this.Book._maskColor);
        }

        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 1;
    }

    // AoWWorldMap.pas:863
    public void WorldMapCreate()
    {
        RegisterWorldMapCampaign(UI.Engine);
    }

    // AoWWorldMap.pas:461
    public void NextStartClick()
    {
        BackButton.CaptureInput = false;
        NextButton.CaptureInput = false;
        BookStartButton.CaptureInput = false;
        AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 2;
    }

    // AoWWorldMap.pas:471
    public void WorldPnlUpdate()
    {
        int frame;
        int startIndex;
        int endIndex;
        if (WorldPanel.Visible && !Book.Visible)
        {
            Path1Image.Drawn = false;
            Path2Image.Drawn = false;
            Path3Image.Drawn = false;
            PlayerPawnImage.Drawn = false;
            EnemyPawnImage.Drawn = false;
            _pathDelay -= 1;
            if (_pathDelay < 1)
            {
                _pathDelay = 5;
                frame = 1 - _pathFrame;
                _pathFrame = frame;
                var campaign = GetCampaign();
                Path1Image.CurrentImage = campaign.Faction * 3 + (frame + 2);
                var campaign2 = GetCampaign();
                Path2Image.CurrentImage = campaign2.Faction * 3 + (_pathFrame + 2);
                var campaign3 = GetCampaign();
                Path3Image.CurrentImage = campaign3.Faction * 3 + (_pathFrame + 2);
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
                    PlayerPawnImage.WinLeft = (campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19 + campaignScenario3.MapX - 25;
                    var campaign4 = GetCampaign();
                    var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign4.NextScenarioId);
                    var campaign5 = GetCampaign();
                    var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign5.CurrentScenarioId);
                    var campaign6 = GetCampaign();
                    var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign6.CurrentScenarioId);
                    PlayerPawnImage.WinTop = (campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19 + campaignScenario6.MapY - 65;
                    if (this.PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                    {
                        this.PlayerPawnImage.Priority = 10;
                        EnemyPawnImage.Priority = 5;
                    }
                    else
                    {
                        this.PlayerPawnImage.Priority = 5;
                        EnemyPawnImage.Priority = 10;
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
                    if (DescriptionLabel.Text != campaignScenario.Description)
                    {
                        var campaign3 = GetCampaign();
                        var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), campaign3.NextScenarioId);
                        DescriptionLabel.Text = campaignScenario2.Description;
                        this.DescriptionLabel.WinTop = 460 - this.DescriptionLabel.WinHeight;

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
                        EnemyPawnImage.WinLeft = (campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19 + campaignScenario3.MapX - 25;
                        var campaignScenario4 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), endIndex);
                        var campaignScenario5 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        var campaignScenario6 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), startIndex);
                        EnemyPawnImage.WinTop = (campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19 + campaignScenario6.MapY - 65;
                        if (this.PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                        {
                            this.PlayerPawnImage.Priority = 10;
                            EnemyPawnImage.Priority = 5;
                        }
                        else
                        {
                            this.PlayerPawnImage.Priority = 5;
                            EnemyPawnImage.Priority = 10;
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
        if (!Book.Visible)
        {
            if (GetCampaign().State == 1)
            {
                DescriptionLabel.Text = "";
                SelectCampaignPath(GetCampaign(), 0);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:578
    public void Path2MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!Book.Visible)
        {
            if (GetCampaign().State == 1)
            {
                DescriptionLabel.Text = "";
                SelectCampaignPath(GetCampaign(), 1);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:590
    public void Path3MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!Book.Visible)
        {
            if (GetCampaign().State == 1)
            {
                DescriptionLabel.Text = "";
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
                description = UI.Engine.TranslateResourceString(s_viewSummaryText);
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
                    description = UI.Engine.TranslateResourceString(LobbyText.CultOfStormsText);
                }
                else
                {
                    description = UI.Engine.TranslateResourceString(LobbyText.KeepersText);
                }
            }
        }

        ShowPawnDescription(sender, description);
    }

    // AoWWorldMap.pas:624. The label update both pawn moves share.
    private void ShowPawnDescription(Image pawn, string description)
    {
        if (DescriptionLabel.Text != description && !IsEmpty(description))
        {
            DescriptionLabel.Text = description;
            this.DescriptionLabel.WinTop = 460 - this.DescriptionLabel.WinHeight;
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
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                this.DescriptionLabel.WinTop = 460 - this.DescriptionLabel.WinHeight;
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
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                this.DescriptionLabel.WinTop = 460 - this.DescriptionLabel.WinHeight;
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
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                var campaignScenario2 = WorldMapCampaign.GetCampaignScenario(GetCampaign(), index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                this.DescriptionLabel.WinTop = 460 - this.DescriptionLabel.WinHeight;
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
                if (!IsEmpty(this.DescriptionLabel.Text))
                {
                    this.DescriptionLabel.Text = "";
                }
            }
        }

        _descriptionHandled = false;
    }

    // AoWWorldMap.pas:706
    public void BookStartClick()
    {
        Book.Visible = false;
        AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        if (!WorldPanel.Visible)
        {
            UI.General.Manager.OverlayPriority = 0;
        }

        if (WorldPanel.Visible)
        {
            _pawnTicks = 20;
            var campaign = GetCampaign();
            campaign.State += 1;
            UpdateCampaignState();
        }
    }

    // AoWWorldMap.pas:720
    public void BookAnimationDone()
    {
        BackButton.CaptureInput = true;
        NextButton.CaptureInput = true;
        BookStartButton.CaptureInput = true;
        Book.Hidden = true;
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
                Book.StartAnimation((PageTurnMode)1);
                var campaign = GetCampaign();
                campaign.Page -= 2;
                UpdateCampaignState();
                BookAction = 0;
                break;
            case 4:
                Book.StartAnimation(0);
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

        if (BookStartButton.Visible != s_bookStartVisible)
        {
            R = new Rect(Book.WinLeft + BookStartButton.WinLeft, Book.WinTop + BookStartButton.WinTop, Book.WinLeft + BookStartButton.WinLeft + BookStartButton.WinWidth, Book.WinTop + BookStartButton.WinTop + BookStartButton.WinHeight);
            UI.General.Manager.AddDrawRectangle(ref R, Book);
            s_bookStartVisible = BookStartButton.Visible;
        }
    }

    // AoWWorldMap.pas:764
    public void ShowBtnClick()
    {
        if (InfoPanel.Visible)
        {
            UpdateCampaignState();
            SelectLabel.Visible = true;
            InfoPanel.Visible = false;
            var showInfo = UI.Engine.TranslateResourceString(s_showInfoText);
            ShowButton.Text = showInfo;
        }
        else
        {
            SelectLabel.Visible = false;
            PlayerPawnImage.Visible = false;
            EnemyPawnImage.Visible = false;
            Path1Image.Visible = false;
            Path2Image.Visible = false;
            Path3Image.Visible = false;
            InfoPanel.Visible = true;
            var hideInfo = UI.Engine.TranslateResourceString(s_hideInfoText);
            ShowButton.Text = hideInfo;
        }
    }

    // AoWWorldMap.pas:786
    public void QuitBtnClick()
    {
        var quitCampaign = UI.Engine.TranslateResourceString(s_quitCampaignText);
        var confirm = UI.Engine.TranslateResourceString(LobbyText.ConfirmText);
        UI.ConfirmationDialog.Confirm(confirm, text: quitCampaign, onConfirmed: QuitCampaign);
    }

    // AOWDialogWin.pas:73, action 5.
    private void QuitCampaign()
    {
        UI.WorldMap.WorldPanel.Visible = false;
        UI.General.ReturnToTitle();
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

        StarImage.Drawn = false;
    }

    // AoWWorldMap.pas:800
    public void StarImgDraw(LockedSurface surface)
    {
        UI.Graphics.WorldMap[_starImageValue].ShowBlended(WorldPanel.WinLeft + StarImage.WinLeft, WorldPanel.WinTop + StarImage.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_starIntensity]);
    }

    // AoWWorldMap.pas:806
    public void Ally1PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPanel.WinLeft + Ally1Frame.WinLeft, InfoPanel.WinTop + Ally1Frame.WinTop, InfoPanel.WinLeft + Ally1Frame.WinLeft + Ally1Frame.WinWidth, InfoPanel.WinTop + Ally1Frame.WinTop + Ally1Frame.WinHeight);
        if (GetCampaign().RaceHistory.Count > 0)
        {
            var raceHistory = GetCampaign().RaceHistory[0];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:818
    public void Ally2PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPanel.WinLeft + Ally2Frame.WinLeft, InfoPanel.WinTop + Ally2Frame.WinTop, InfoPanel.WinLeft + Ally2Frame.WinLeft + Ally2Frame.WinWidth, InfoPanel.WinTop + Ally2Frame.WinTop + Ally2Frame.WinHeight);
        if (GetCampaign().RaceHistory.Count > 1)
        {
            var raceHistory = GetCampaign().RaceHistory[1];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:830
    public void Ally3PnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPanel.WinLeft + Ally3Frame.WinLeft, InfoPanel.WinTop + Ally3Frame.WinTop, InfoPanel.WinLeft + Ally3Frame.WinLeft + Ally3Frame.WinWidth, InfoPanel.WinTop + Ally3Frame.WinTop + Ally3Frame.WinHeight);
        if (GetCampaign().RaceHistory.Count > 2)
        {
            var raceHistory = GetCampaign().RaceHistory[2];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(R, surface);
        }
    }

    // AoWWorldMap.pas:842
    public void HeroPnlPreDraw(LockedSurface surface)
    {
        Rect R;
        R = new Rect(InfoPanel.WinLeft + HeroFrame.WinLeft, InfoPanel.WinTop + HeroFrame.WinTop, InfoPanel.WinLeft + HeroFrame.WinLeft + HeroFrame.WinWidth, InfoPanel.WinTop + HeroFrame.WinTop + HeroFrame.WinHeight);
        if (GetCampaign().TransferSettings.Leader != null)
        {
            GetCampaign().TransferSettings.Leader.ShowFace(R, surface);
        }
    }

    // AoWWorldMap.pas:854
    public void SetBtnClick()
    {
        UI.CampaignSettingsDialog.Window.Visible = true;
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
        dialog.UI.Engine.CloseHsm(null);
        dialog.UI.TitleWindow.MainWindow.Visible = true;
        dialog.UI.TitleWindow.TitlePanel.Visible = false;
        dialog.WorldPanel.Visible = true;
        dialog._pathFrame = 0;
        dialog._pathDelay = 5;
        dialog.DescriptionLabel.Text = "";
        dialog.UI.General.Manager.OverlayPriority = 249;
        AudioHost.SongPlayer.Stop(false);
        SetCurrentDir(dialog.UI.Engine.StartupDirectory);
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
        dialog.PlayerPawnImage.CurrentImage = campaign2.Faction + 7;
        var campaign3 = dialog.GetCampaign();
        dialog.EnemyPawnImage.CurrentImage = 8 - campaign3.Faction;
        dialog.UpdateCampaignState();
    }

}
