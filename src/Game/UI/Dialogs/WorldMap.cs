#nullable disable
// Translated from AoWWorldMap.pas.
using Aow.Engine;
using Aow.Engine.Nodes;
using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Magic;
using Aow.Game.Magic.Abilities;

using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.Units;

using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.DelphiConversions;
using static Aow.Rtl.DelphiFiles;
using static Aow.Rtl.DelphiStrings;

namespace Aow.Game.UI.Dialogs;

// star image, brightness, path animation, pawn timing, hover, context, completion event, book
// action
public partial class WorldMap(UIContext ui) : UIModule(ui)
{
    public Memo LeftPageMemo { get; private set; }
    public Image RightPagePicture { get; private set; }
    public Panel WorldPanel { get; private set; }
    public Image Path1Image { get; private set; }
    public Image Path2Image { get; private set; }
    public Image Path3Image { get; private set; }
    public Image PlayerPawnImage { get; private set; }
    public Image EnemyPawnImage { get; private set; }
    public Label DescriptionLabel { get; private set; }
    public Button BackButton { get; private set; }
    public Button NextButton { get; private set; }
    public Button CancelButton { get; private set; }
    public Button BookStartButton { get; private set; }
    public Label LeftPageLabel { get; private set; }
    public Memo RightPageMemo { get; private set; }
    public Label RightPageLabel { get; private set; }
    public Image LeftPagePicture { get; private set; }
    public BookWindow Book { get; private set; }
    public Panel InfoPanel { get; private set; }
    public Button QuitButton { get; private set; }
    public Button ShowButton { get; private set; }
    public Label SelectLabel { get; private set; }
    public Image StarImage { get; private set; }
    public Frame HeroFrame { get; private set; }
    public Frame Ally1Frame { get; private set; }
    public Frame Ally2Frame { get; private set; }
    public Frame Ally3Frame { get; private set; }
    public Button SetButton { get; private set; }
    public VerticalScrollBar LeftPageScrollBar { get; private set; }
    public VerticalScrollBar RightPageScrollBar { get; private set; }

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

    // The world map screen runs while the engine holds a world map campaign.
    // AoWWorldMap.pas:152
    public WorldMapCampaign Campaign => UI.Engine.Campaign;

    // AoWWorldMap.pas:157
    public void UpdateCampaignState()
    {
        if (Campaign is not null)
        {
            int index;
            switch (Campaign.State)
            {
                case 0:
                    WorldMapCampaign campaign = Campaign;
                    bool condition = !IsEmpty(GetCampaignPage(WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId).OutroPages, 0).ImageLibrary);
                    if (!condition)
                    {
                        WorldMapCampaign campaign2 = Campaign;
                        condition = GetCampaignPage(WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.CurrentScenarioId).OutroPages, 0).Text.Count > 0;
                    }

                    if (condition)
                    {
                        _ = SetCurrentDir($"{UI.Engine.StartupDirectory}\\Int\\Scenes");
                        WorldMapCampaign campaign2 = Campaign;
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.CurrentScenarioId);
                        WorldMapCampaign campaign3 = Campaign;
                        CampaignScenarioPage campaignPage = GetCampaignPage(campaignScenario.OutroPages, campaign3.Page);
                        LeftPageLabel.Text = campaignPage.Heading;
                        WorldMapCampaign campaign4 = Campaign;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign4.CurrentScenarioId);
                        WorldMapCampaign campaign5 = Campaign;
                        CampaignScenarioPage worldMapCampaignPage = GetCampaignPage(campaignScenario2.OutroPages, campaign5.Page + 1);
                        RightPageLabel.Text = worldMapCampaignPage.Heading;
                        WorldMapCampaign campaign6 = Campaign;
                        WorldMapCampaignScenario campaignScenario3 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign6.CurrentScenarioId);
                        WorldMapCampaign campaign7 = Campaign;
                        CampaignScenarioPage campaignPage2 = GetCampaignPage(campaignScenario3.OutroPages, campaign7.Page);
                        LeftPageMemo.Items.Assign(campaignPage2.Text);
                        WorldMapCampaign campaign8 = Campaign;
                        WorldMapCampaignScenario campaignScenario4 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign8.CurrentScenarioId);
                        WorldMapCampaign campaign9 = Campaign;
                        CampaignScenarioPage campaignPage3 = GetCampaignPage(campaignScenario4.OutroPages, campaign9.Page + 1);
                        RightPageMemo.Items.Assign(campaignPage3.Text);
                        _starIntensity = 50;
                        _starStep = 1;
                        WorldMapCampaign campaign10 = Campaign;
                        switch (WorldMapCampaign.GetCampaignScenario(Campaign, campaign10.CurrentScenarioId).MapKind)
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
                            default:
                                break;
                        }

                        WorldMapCampaign campaign11 = Campaign;
                        WorldMapCampaignScenario campaignScenario5 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign11.CurrentScenarioId);
                        WorldMapCampaign campaign12 = Campaign;
                        if (IsEmpty(GetCampaignPage(campaignScenario5.OutroPages, campaign12.Page).ImageLibrary))
                        {
                            LeftPagePicture.ImageLibrary = null;
                        }
                        else
                        {
                            WorldMapCampaign campaign13 = Campaign;
                            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign13.CurrentScenarioId);
                            WorldMapCampaign campaign14 = Campaign;
                            CampaignScenarioPage campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page);
                            UI.Graphics.BookLeftPage.LoadFromFile(campaignPage4.ImageLibrary);
                            LeftPagePicture.ImageLibrary = UI.Graphics.BookLeftPage;
                            WorldMapCampaign campaign15 = Campaign;
                            WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign15.CurrentScenarioId);
                            WorldMapCampaign campaign16 = Campaign;
                            CampaignScenarioPage campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page);
                            LeftPagePicture.CurrentImage = campaignPage5.ImageIndex;
                            LeftPagePicture.WinWidth = 0;
                            LeftPagePicture.WinHeight = 0;
                            LeftPagePicture.Visible = true;
                        }

                        WorldMapCampaign campaign17 = Campaign;
                        WorldMapCampaignScenario campaignScenario8 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign17.CurrentScenarioId);
                        WorldMapCampaign campaign18 = Campaign;
                        if (IsEmpty(GetCampaignPage(campaignScenario8.OutroPages, campaign18.Page + 1).ImageLibrary))
                        {
                            RightPagePicture.ImageLibrary = null;
                        }
                        else
                        {
                            WorldMapCampaign campaign13 = Campaign;
                            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign13.CurrentScenarioId);
                            WorldMapCampaign campaign14 = Campaign;
                            CampaignScenarioPage campaignPage4 = GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page + 1);
                            UI.Graphics.BookRightPage.LoadFromFile(campaignPage4.ImageLibrary);
                            RightPagePicture.ImageLibrary = UI.Graphics.BookRightPage;
                            WorldMapCampaign campaign15 = Campaign;
                            WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign15.CurrentScenarioId);
                            WorldMapCampaign campaign16 = Campaign;
                            CampaignScenarioPage campaignPage5 = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page + 1);
                            RightPagePicture.CurrentImage = campaignPage5.ImageIndex;
                            RightPagePicture.WinWidth = 0;
                            RightPagePicture.WinHeight = 0;
                            RightPagePicture.Visible = true;
                        }

                        WorldMapCampaign campaign19 = Campaign;
                        BackButton.Visible = campaign19.Page > 0;
                        if (Campaign.Page == 8)
                        {
                            NextButton.Visible = false;
                        }
                        else
                        {
                            WorldMapCampaign campaign13 = Campaign;
                            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign13.CurrentScenarioId);
                            WorldMapCampaign campaign14 = Campaign;
                            bool test = !IsEmpty(GetCampaignPage(campaignScenario6.OutroPages, campaign14.Page + 2).ImageLibrary);
                            if (!test)
                            {
                                WorldMapCampaign campaign15 = Campaign;
                                WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign15.CurrentScenarioId);
                                WorldMapCampaign campaign16 = Campaign;
                                test = GetCampaignPage(campaignScenario7.OutroPages, campaign16.Page + 2).Text.Count > 0;
                            }

                            NextButton.Visible = test;
                        }

                        BookStartButton.Visible = !NextButton.Visible;
                        CancelButton.Visible = false;
                        BookWindow bookWindow = Book;
                        bookWindow.Hidden = true;
                        bookWindow.Visible = true;

                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        _ = (Book.BackSurface?.Clear(Book.MaskColor));

                        if (UI.TitleWindow.MainWindow.Visible)
                        {
                            UI.General.Manager.OverlayPriority = 249;
                        }
                    }
                    else
                    {
                        WorldMapCampaign campaign2 = Campaign;
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
                    WorldMapCampaign campaign20 = Campaign;
                    WorldMapCampaignScenario campaignScenario9 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign20.CurrentScenarioId);
                    PlayerPawnImage.WinLeft = campaignScenario9.MapX - 25;
                    WorldMapCampaign campaign21 = Campaign;
                    WorldMapCampaignScenario campaignScenario10 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign21.CurrentScenarioId);
                    PlayerPawnImage.WinTop = campaignScenario10.MapY - 65;
                    WorldMapCampaign campaign22 = Campaign;
                    int path = WorldMapCampaign.GetCampaignScenario(Campaign, campaign22.CurrentScenarioId).Paths[3];
                    index = Campaign.Scenarios.IndexOfId(path);
                    if (index != -1)
                    {
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        EnemyPawnImage.WinLeft = campaignScenario.MapX - 25;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        EnemyPawnImage.WinTop = campaignScenario2.MapY - 65;
                    }

                    {
                        if (PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                        {
                            PlayerPawnImage.Priority = 10;
                            EnemyPawnImage.Priority = 5;
                        }
                        else
                        {
                            PlayerPawnImage.Priority = 5;
                            EnemyPawnImage.Priority = 10;
                        }
                    }

                    WorldMapCampaign campaign23 = Campaign;
                    if (WorldMapCampaign.GetCampaignScenario(Campaign, campaign23.CurrentScenarioId).Paths[0] > -1)
                    {
                        Path1Image.Visible = true;
                        WorldMapCampaign campaign2 = Campaign;
                        Path1Image.CurrentImage = (campaign2.Faction * 3) + _pathFrame + 2;
                        WorldMapCampaign campaign3 = Campaign;
                        int path2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign3.CurrentScenarioId).Paths[0];
                        index = Campaign.Scenarios.IndexOfId(path2);
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        Path1Image.WinLeft = campaignScenario.MapX;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        Path1Image.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path1Image.Visible = false;
                    }

                    WorldMapCampaign campaign24 = Campaign;
                    if (WorldMapCampaign.GetCampaignScenario(Campaign, campaign24.CurrentScenarioId).Paths[1] > -1)
                    {
                        Path2Image.Visible = true;
                        WorldMapCampaign campaign2 = Campaign;
                        Path2Image.CurrentImage = (campaign2.Faction * 3) + _pathFrame + 2;
                        WorldMapCampaign campaign3 = Campaign;
                        int path2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign3.CurrentScenarioId).Paths[1];
                        index = Campaign.Scenarios.IndexOfId(path2);
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        Path2Image.WinLeft = campaignScenario.MapX;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        Path2Image.WinTop = campaignScenario2.MapY;
                    }
                    else
                    {
                        Path2Image.Visible = false;
                    }

                    WorldMapCampaign campaign25 = Campaign;
                    if (WorldMapCampaign.GetCampaignScenario(Campaign, campaign25.CurrentScenarioId).Paths[2] > -1)
                    {
                        Path3Image.Visible = true;
                        WorldMapCampaign campaign2 = Campaign;
                        Path3Image.CurrentImage = (campaign2.Faction * 3) + _pathFrame + 2;
                        WorldMapCampaign campaign3 = Campaign;
                        int path2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign3.CurrentScenarioId).Paths[2];
                        index = Campaign.Scenarios.IndexOfId(path2);
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                        Path3Image.WinLeft = campaignScenario.MapX;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
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
                    WorldMapCampaign campaign26 = Campaign;
                    bool passed = !IsEmpty(GetCampaignPage(WorldMapCampaign.GetCampaignScenario(Campaign, campaign26.NextScenarioId).IntroPages, 0).ImageLibrary);
                    if (!passed)
                    {
                        WorldMapCampaign campaign2 = Campaign;
                        passed = GetCampaignPage(WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.NextScenarioId).IntroPages, 0).Text.Count > 0;
                    }

                    if (passed)
                    {
                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        ShowIntroPages(Campaign.NextScenarioId);
                    }
                    else
                    {
                        _pawnTicks = 20;
                        Book.Visible = false;
                        SetButton.Visible = false;
                        ShowButton.Visible = false;
                        QuitButton.Visible = false;
                        WorldMapCampaign campaign2 = Campaign;
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
                    WorldMapCampaign campaign27 = Campaign;
                    WorldMapCampaignScenario campaignScenario11 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign27.NextScenarioId);
                    StrategicMap preview = UI.Engine.CreatePreviewHsm<StrategicMap>(FileBoundary.ReadName($"{UI.Engine.StartupDirectory}Scenario\\Campaign\\{campaignScenario11.FileName}"), profile: GameProfile.WorldMap);
                    if (preview is not null)
                    {
                        index = preview.Players.IndexOfPlayerType(PlayerType.Human);
                        bool test = index != -1 && Campaign.RaceHistory.Count > 0;
                        if (test)
                        {
                            Player player = preview.Players.GetPlayers(index);
                            test = Campaign.RaceHistory.IndexOf((byte)player.PlayerRace) == -1;
                        }

                        if (test && Campaign.TransferSettings.Leader.SphereCount < (Campaign.RaceHistory.Count * 2) + 3)
                        {
                            ShowSpheres();
                        }
                        else
                        {
                            WorldMapCampaign campaign2 = Campaign;
                            campaign2.State += 1;
                        }

                        preview?.Dispose();
                    }

                    break;
                case 7:
                    WorldMapCampaign campaign28 = Campaign;
                    if (!IsEmpty(WorldMapCampaign.GetCampaignScenario(Campaign, campaign28.NextScenarioId).FileName))
                    {
                        WorldMapCampaign campaign2 = Campaign;
                        campaign2.State += 1;
                        WorldMapCampaign campaign3 = Campaign;
                        Campaign.StartScenario(campaign3.NextScenarioId, null);
                        _ = SetCurrentDir($"{UI.Engine.StartupDirectory}Scenario\\Campaign\\");
                        WorldMapCampaign campaign4 = Campaign;
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, campaign4.CurrentScenarioId);
                        General.LoadGame(UI.General, FileBoundary.ReadName(campaignScenario.FileName));
                        UI.TitleWindow.TitlePanel.Visible = true;
                        WorldPanel.Visible = false;
                        Campaign.SetupScenario();
                        General.StartGame(UI.General);
                        UI.WorldMap.DescriptionLabel.Text = "";
                    }
                    else
                    {
                        UI.WorldMap.DescriptionLabel.Text = "";
                        Campaign.State = 0;
                        UpdateCampaignState();
                    }

                    break;
                case 10:
                    ShowIntroPages(Campaign.CurrentScenarioId);
                    break;
                default:
                    break;
            }
        }
    }

    // AoWWorldMap.pas:347
    public void ShowIntroPages(int index)
    {
        _ = SetCurrentDir($"{UI.Engine.StartupDirectory}Int\\Scenes");
        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign = Campaign;
        CampaignScenarioPage campaignPage = GetCampaignPage(campaignScenario.IntroPages, campaign.Page);
        LeftPageLabel.Text = campaignPage.Heading;
        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign2 = Campaign;
        CampaignScenarioPage worldMapCampaignPage = GetCampaignPage(campaignScenario2.IntroPages, campaign2.Page + 1);
        RightPageLabel.Text = worldMapCampaignPage.Heading;
        WorldMapCampaignScenario campaignScenario3 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign3 = Campaign;
        CampaignScenarioPage campaignPage2 = GetCampaignPage(campaignScenario3.IntroPages, campaign3.Page);
        LeftPageMemo.Items.Assign(campaignPage2.Text);
        WorldMapCampaignScenario campaignScenario4 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign4 = Campaign;
        CampaignScenarioPage campaignPage3 = GetCampaignPage(campaignScenario4.IntroPages, campaign4.Page + 1);
        RightPageMemo.Items.Assign(campaignPage3.Text);
        LeftPageScrollBar.Position = 0;
        RightPageScrollBar.Position = 0;
        WorldMapCampaignScenario campaignScenario5 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign5 = Campaign;
        if (IsEmpty(GetCampaignPage(campaignScenario5.IntroPages, campaign5.Page).ImageLibrary))
        {
            LeftPagePicture.ImageLibrary = null;
        }
        else
        {
            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            WorldMapCampaign campaign6 = Campaign;
            CampaignScenarioPage campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page);
            UI.Graphics.BookLeftPage.LoadFromFile(campaignPage4.ImageLibrary);
            LeftPagePicture.ImageLibrary = UI.Graphics.BookLeftPage;
            WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            WorldMapCampaign campaign7 = Campaign;
            CampaignScenarioPage campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page);
            LeftPagePicture.CurrentImage = campaignPage5.ImageIndex;
            LeftPagePicture.WinWidth = 0;
            LeftPagePicture.WinHeight = 0;
            LeftPagePicture.Visible = true;
        }

        WorldMapCampaignScenario campaignScenario8 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
        WorldMapCampaign campaign8 = Campaign;
        if (IsEmpty(GetCampaignPage(campaignScenario8.IntroPages, campaign8.Page + 1).ImageLibrary))
        {
            RightPagePicture.ImageLibrary = null;
        }
        else
        {
            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            WorldMapCampaign campaign6 = Campaign;
            CampaignScenarioPage campaignPage4 = GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page + 1);
            UI.Graphics.BookRightPage.LoadFromFile(campaignPage4.ImageLibrary);
            RightPagePicture.ImageLibrary = UI.Graphics.BookRightPage;
            WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            WorldMapCampaign campaign7 = Campaign;
            CampaignScenarioPage campaignPage5 = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page + 1);
            RightPagePicture.CurrentImage = campaignPage5.ImageIndex;
            RightPagePicture.WinWidth = 0;
            RightPagePicture.WinHeight = 0;
            RightPagePicture.Visible = true;
        }

        WorldMapCampaign campaign9 = Campaign;
        BackButton.Visible = campaign9.Page > 0;
        if (Campaign.Page == 8)
        {
            NextButton.Visible = false;
        }
        else
        {
            WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            WorldMapCampaign campaign6 = Campaign;
            bool condition = !IsEmpty(GetCampaignPage(campaignScenario6.IntroPages, campaign6.Page + 2).ImageLibrary);
            if (!condition)
            {
                WorldMapCampaignScenario campaignScenario7 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                WorldMapCampaign campaign7 = Campaign;
                condition = GetCampaignPage(campaignScenario7.IntroPages, campaign7.Page + 2).Text.Count > 0;
            }

            NextButton.Visible = condition;
        }

        bool test = !NextButton.Visible || Campaign.State == 10;
        BookStartButton.Visible = test;
        CancelButton.Visible = WorldPanel.Visible;
        BookWindow bookWindow = Book;
        bookWindow.Hidden = true;
        bookWindow.Visible = true;

        _ = (Book.BackSurface?.Clear(Book.MaskColor));

        if (UI.TitleWindow.MainWindow.Visible)
        {
            UI.General.Manager.OverlayPriority = 249;
        }
    }

    // AoWWorldMap.pas:404
    public void ShowSpheres()
    {
        WorldPanel.Visible = false;
        int picks = Campaign.TransferSettings.Leader.SpherePicksMax;
        LeaderSetupControl control = new(null) { Holder = UI.Engine };
        WorldMapCampaign campaign = Campaign;
        control.Leader.Assign(campaign.TransferSettings.Leader, UI.Engine);
        control.Leader.SetSpherePicksMax((byte)(picks + 2));
        control.OriginalLeader.Assign(control.Leader, UI.Engine);
        control.Mode = Set<LeaderSetupModeElement>.Of(LeaderSetupModeElement.Spheres);
        UI.LeaderSetupWindow.Execute(control);
        UI.LeaderSetupWindow.ClearButton.Visible = false;
        UI.LeaderSetupWindow.DefaultButton.Visible = false;
        UI.LeaderSetupWindow.PreviousButton.Visible = false;
        UI.LeaderSetupWindow.SetFixedSpheres(picks);
        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_assignNewSpheresFormat), [IntToStr(2)]);
        UI.LeaderSetupWindow.Phase3Header.Text = format;
        control.OnDone += SpheresDone;
        _ = control.Release();
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryTexture = UI.Graphics.WorldMap;
        UI.LeaderSetupWindow.LeaderPanel.ImageLibraryIndexTexture = 0;
    }

    // AoWWorldMap.pas:386
    public void SpheresDone(LeaderSetupControl sender)
    {
        LeaderSetupControl leaderSetupControl = sender;
        if (leaderSetupControl.Status == LeaderSetupControl.LeaderSetupStatus.Done)
        {
            Leader leader = leaderSetupControl.OriginalLeader.Copy<Leader>(UI.Engine);
            for (int i = Campaign.TransferSettings.Leader.SphereCount, last = leader.SphereCount - 1; i <= last; i++)
            {
                MagicSphere sphere = leader.GetSpheres(i);
                _ = Campaign.TransferSettings.SelectedSpheres.Add((byte)(int)sphere);
            }

            WorldPanel.Visible = true;
            WorldMapCampaign campaign = Campaign;
            campaign.State += 1;
            UpdateCampaignState();
        }
    }

    // AoWWorldMap.pas:428
    public void BookClosed()
    {
        NotifyEvent<object> done = OnBookClosed;
        EngineObject context = BookContext;
        OnBookClosed = null;
        BookContext = null;
        done?.Invoke(context);

        _ = (context?.Release());

        UI.General.Manager.OverlayPriority = 0;
    }

    // AoWWorldMap.pas:146
    public void ResetCampaign()
    {
        Campaign.State = 0;
        Campaign.Page = 0;
        InfoPanel.Visible = false;
        string showInfo = UI.Engine.TranslateResourceString(s_showInfoText);
        ShowButton.Text = showInfo;
    }

    // AoWWorldMap.pas:151
    public void ShowCampaignHistory()
    {
        Campaign.State = 10;
        Campaign.Page = 0;
        WorldMapCampaign campaign = Campaign;
        WorldMapCampaignScenario scenario = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId);
        while (true)
        {
            bool condition = scenario.IntroPages.Count > Campaign.Page + 2;
            if (condition)
            {
                WorldMapCampaign campaign2 = Campaign;
                bool test = !IsEmpty(GetCampaignPage(scenario.IntroPages, campaign2.Page + 2).ImageLibrary);
                if (!test)
                {
                    WorldMapCampaign campaign3 = Campaign;
                    test = GetCampaignPage(scenario.IntroPages, campaign3.Page + 2).Text.Count > 0;
                }

                condition = test;
            }

            if (!condition)
            {
                break;
            }

            WorldMapCampaign campaign4 = Campaign;
            campaign4.Page += 2;
        }

        ShowIntroPages(Campaign.CurrentScenarioId);
    }

    // AoWWorldMap.pas:442
    public void BackClick()
    {
        BackButton.CaptureInput = false;
        NextButton.CaptureInput = false;
        Button button = BookStartButton;
        button.CaptureInput = false;
        button.Visible = false;

        Book.Drawn = false;
        _ = (Book.BackSurface?.Clear(Book.MaskColor));

        _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 1;
    }

    // AoWWorldMap.pas:863
    public void WorldMapCreate() => RegisterWorldMapCampaign(UI.Engine);

    // AoWWorldMap.pas:461
    public void NextStartClick()
    {
        BackButton.CaptureInput = false;
        NextButton.CaptureInput = false;
        BookStartButton.CaptureInput = false;
        _ = AudioPlayback.PlaySample("Sfx\\Int\\Pagef1.wav");
        BookAction = 2;
    }

    // AoWWorldMap.pas:471
    public void WorldPnlUpdate()
    {
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
                int frame = 1 - _pathFrame;
                _pathFrame = frame;
                WorldMapCampaign campaign = Campaign;
                Path1Image.CurrentImage = (campaign.Faction * 3) + frame + 2;
                WorldMapCampaign campaign2 = Campaign;
                Path2Image.CurrentImage = (campaign2.Faction * 3) + _pathFrame + 2;
                WorldMapCampaign campaign3 = Campaign;
                Path3Image.CurrentImage = (campaign3.Faction * 3) + _pathFrame + 2;
            }

            if (Campaign.State == 5)
            {
                if (_pawnTicks > 0)
                {
                    WorldMapCampaign campaign = Campaign;
                    WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.NextScenarioId);
                    WorldMapCampaign campaign2 = Campaign;
                    WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.CurrentScenarioId);
                    WorldMapCampaign campaign3 = Campaign;
                    WorldMapCampaignScenario campaignScenario3 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign3.CurrentScenarioId);
                    PlayerPawnImage.WinLeft = ((campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19) + campaignScenario3.MapX - 25;
                    WorldMapCampaign campaign4 = Campaign;
                    WorldMapCampaignScenario campaignScenario4 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign4.NextScenarioId);
                    WorldMapCampaign campaign5 = Campaign;
                    WorldMapCampaignScenario campaignScenario5 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign5.CurrentScenarioId);
                    WorldMapCampaign campaign6 = Campaign;
                    WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign6.CurrentScenarioId);
                    PlayerPawnImage.WinTop = ((campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19) + campaignScenario6.MapY - 65;
                    if (PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                    {
                        PlayerPawnImage.Priority = 10;
                        EnemyPawnImage.Priority = 5;
                    }
                    else
                    {
                        PlayerPawnImage.Priority = 5;
                        EnemyPawnImage.Priority = 10;
                    }
                }
                else
                {
                    _pawnTicks = 20;
                    _enemyDelay = 30;
                    WorldMapCampaign campaign = Campaign;
                    campaign.State += 1;
                    WorldMapCampaign campaign2 = Campaign;
                    WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.NextScenarioId);
                    if (DescriptionLabel.Text != campaignScenario.Description)
                    {
                        WorldMapCampaign campaign3 = Campaign;
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign3.NextScenarioId);
                        DescriptionLabel.Text = campaignScenario2.Description;
                        DescriptionLabel.WinTop = 460 - DescriptionLabel.WinHeight;

                        _descriptionHandled = true;
                    }
                }

                _pawnTicks -= 1;
            }

            if (Campaign.State == 6)
            {
                if (_pawnTicks > 0)
                {
                    _descriptionHandled = true;
                    WorldMapCampaign campaign = Campaign;
                    int path = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId).Paths[3];
                    int startIndex = Campaign.Scenarios.IndexOfId(path);
                    WorldMapCampaign campaign2 = Campaign;
                    int path2 = WorldMapCampaign.GetCampaignScenario(Campaign, campaign2.NextScenarioId).Paths[3];
                    int endIndex = Campaign.Scenarios.IndexOfId(path2);
                    if (startIndex != -1 && endIndex != -1)
                    {
                        WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, endIndex);
                        WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, startIndex);
                        WorldMapCampaignScenario campaignScenario3 = WorldMapCampaign.GetCampaignScenario(Campaign, startIndex);
                        EnemyPawnImage.WinLeft = ((campaignScenario.MapX - campaignScenario2.MapX) * (20 - _pawnTicks) / 19) + campaignScenario3.MapX - 25;
                        WorldMapCampaignScenario campaignScenario4 = WorldMapCampaign.GetCampaignScenario(Campaign, endIndex);
                        WorldMapCampaignScenario campaignScenario5 = WorldMapCampaign.GetCampaignScenario(Campaign, startIndex);
                        WorldMapCampaignScenario campaignScenario6 = WorldMapCampaign.GetCampaignScenario(Campaign, startIndex);
                        EnemyPawnImage.WinTop = ((campaignScenario4.MapY - campaignScenario5.MapY) * (20 - _pawnTicks) / 19) + campaignScenario6.MapY - 65;
                        if (PlayerPawnImage.WinTop > EnemyPawnImage.WinTop)
                        {
                            PlayerPawnImage.Priority = 10;
                            EnemyPawnImage.Priority = 5;
                        }
                        else
                        {
                            PlayerPawnImage.Priority = 5;
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
                        WorldMapCampaign campaign = Campaign;
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
            if (Campaign.State == 1)
            {
                DescriptionLabel.Text = "";
                SelectCampaignPath(Campaign, 0);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:578
    public void Path2MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!Book.Visible)
        {
            if (Campaign.State == 1)
            {
                DescriptionLabel.Text = "";
                SelectCampaignPath(Campaign, 1);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:590
    public void Path3MouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!Book.Visible)
        {
            if (Campaign.State == 1)
            {
                DescriptionLabel.Text = "";
                SelectCampaignPath(Campaign, 2);
                UpdateCampaignState();
            }
        }
    }

    // AoWWorldMap.pas:602
    public void CancelClick()
    {
        Campaign.ResetNextScenario();
        Campaign.State = 1;
        _ = AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        UpdateCampaignState();
    }

    // AoWWorldMap.pas:611. Bound to the player pawn.
    public void PPawnMouseDown(Image sender, MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (X > 20)
        {
            if (Campaign.State == 1)
            {
                Campaign.State = 0;
                Campaign.Page = 0;
                UpdateCampaignState();
            }
        }

        // { Original hit test uses X. }
        sender.Handled = false;
    }

    // AoWWorldMap.pas:611. Bound to the enemy pawn.
    public void EPawnMouseDown(Image sender, MouseButton button, Set<ShiftState> shift, int X, int Y) => sender.Handled = false;

    // AoWWorldMap.pas:624. Bound to the player pawn.
    public void PPawnMouseMove(Image sender, Set<ShiftState> shift, int X, int Y)
    {
        string description = "";
        // { Original hit test uses X. }
        if (X > 20)
        {
            if (Campaign.State == 1)
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
            if (Campaign.State == 1)
            {
                description = Campaign.Faction == 0
                    ? UI.Engine.TranslateResourceString(LobbyText.CultOfStormsText)
                    : UI.Engine.TranslateResourceString(LobbyText.KeepersText);
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
            DescriptionLabel.WinTop = 460 - DescriptionLabel.WinHeight;
        }

        _descriptionHandled = true;
        pawn.Handled = false;
    }

    // AoWWorldMap.pas:643
    public void Path1MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (Campaign.State == 1)
        {
            WorldMapCampaign campaign = Campaign;
            int path = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId).Paths[0];
            int index = Campaign.Scenarios.IndexOfId(path);
            WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                DescriptionLabel.WinTop = 460 - DescriptionLabel.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:661
    public void Path2MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (Campaign.State == 1)
        {
            WorldMapCampaign campaign = Campaign;
            int path = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId).Paths[1];
            int index = Campaign.Scenarios.IndexOfId(path);
            WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                DescriptionLabel.WinTop = 460 - DescriptionLabel.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:679
    public void Path3MouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (Campaign.State == 1)
        {
            WorldMapCampaign campaign = Campaign;
            int path = WorldMapCampaign.GetCampaignScenario(Campaign, campaign.CurrentScenarioId).Paths[2];
            int index = Campaign.Scenarios.IndexOfId(path);
            WorldMapCampaignScenario campaignScenario = WorldMapCampaign.GetCampaignScenario(Campaign, index);
            if (DescriptionLabel.Text != campaignScenario.PathDescription)
            {
                WorldMapCampaignScenario campaignScenario2 = WorldMapCampaign.GetCampaignScenario(Campaign, index);
                DescriptionLabel.Text = campaignScenario2.PathDescription;
                DescriptionLabel.WinTop = 460 - DescriptionLabel.WinHeight;
            }
        }

        _descriptionHandled = true;
    }

    // AoWWorldMap.pas:697
    public void WorldPnlMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        if (!_descriptionHandled)
        {
            if (Campaign.State == 1)
            {
                if (!IsEmpty(DescriptionLabel.Text))
                {
                    DescriptionLabel.Text = "";
                }
            }
        }

        _descriptionHandled = false;
    }

    // AoWWorldMap.pas:706
    public void BookStartClick()
    {
        Book.Visible = false;
        _ = AudioPlayback.PlaySample("Sfx\\Int\\BookClse.wav");
        if (!WorldPanel.Visible)
        {
            UI.General.Manager.OverlayPriority = 0;
        }

        if (WorldPanel.Visible)
        {
            _pawnTicks = 20;
            WorldMapCampaign campaign = Campaign;
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
    public void BookWinShow() => BookAction = 0;

    // AoWWorldMap.pas:733
    public void BookWinUpdate()
    {
        switch (BookAction)
        {
            case 3:
                Book.StartAnimation((PageTurnMode)1);
                WorldMapCampaign campaign = Campaign;
                campaign.Page -= 2;
                UpdateCampaignState();
                BookAction = 0;
                break;
            case 4:
                Book.StartAnimation(0);
                WorldMapCampaign campaign2 = Campaign;
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
            default:
                break;
        }

        if (BookStartButton.Visible != s_bookStartVisible)
        {
            Rect r = new(Book.WinLeft + BookStartButton.WinLeft, Book.WinTop + BookStartButton.WinTop, Book.WinLeft + BookStartButton.WinLeft + BookStartButton.WinWidth, Book.WinTop + BookStartButton.WinTop + BookStartButton.WinHeight);
            UI.General.Manager.AddDrawRectangle(ref r, Book);
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
            string showInfo = UI.Engine.TranslateResourceString(s_showInfoText);
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
            string hideInfo = UI.Engine.TranslateResourceString(s_hideInfoText);
            ShowButton.Text = hideInfo;
        }
    }

    // AoWWorldMap.pas:786
    public void QuitBtnClick()
    {
        string quitCampaign = UI.Engine.TranslateResourceString(s_quitCampaignText);
        string confirm = UI.Engine.TranslateResourceString(LobbyText.ConfirmText);
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
    public void StarImgDraw(LockedSurface surface) => UI.Graphics.WorldMap[_starImageValue].ShowBlended(WorldPanel.AbsoluteLeft + StarImage.WinLeft, WorldPanel.AbsoluteTop + StarImage.WinTop, surface, GraphicsState.IntensityBlendTable.LookupTables[_starIntensity]);

    // AoWWorldMap.pas:806
    public void Ally1PnlPreDraw(LockedSurface surface)
    {
        Rect r = new(InfoPanel.AbsoluteLeft + Ally1Frame.WinLeft, InfoPanel.AbsoluteTop + Ally1Frame.WinTop, InfoPanel.AbsoluteLeft + Ally1Frame.WinLeft + Ally1Frame.WinWidth, InfoPanel.AbsoluteTop + Ally1Frame.WinTop + Ally1Frame.WinHeight);
        if (Campaign.RaceHistory.Count > 0)
        {
            byte raceHistory = Campaign.RaceHistory[0];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(r, surface);
        }
    }

    // AoWWorldMap.pas:818
    public void Ally2PnlPreDraw(LockedSurface surface)
    {
        Rect r = new(InfoPanel.AbsoluteLeft + Ally2Frame.WinLeft, InfoPanel.AbsoluteTop + Ally2Frame.WinTop, InfoPanel.AbsoluteLeft + Ally2Frame.WinLeft + Ally2Frame.WinWidth, InfoPanel.AbsoluteTop + Ally2Frame.WinTop + Ally2Frame.WinHeight);
        if (Campaign.RaceHistory.Count > 1)
        {
            byte raceHistory = Campaign.RaceHistory[1];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(r, surface);
        }
    }

    // AoWWorldMap.pas:830
    public void Ally3PnlPreDraw(LockedSurface surface)
    {
        Rect r = new(InfoPanel.AbsoluteLeft + Ally3Frame.WinLeft, InfoPanel.AbsoluteTop + Ally3Frame.WinTop, InfoPanel.AbsoluteLeft + Ally3Frame.WinLeft + Ally3Frame.WinWidth, InfoPanel.AbsoluteTop + Ally3Frame.WinTop + Ally3Frame.WinHeight);
        if (Campaign.RaceHistory.Count > 2)
        {
            byte raceHistory = Campaign.RaceHistory[2];
            GamePack.Current.RaceResources.GetRaceResource(raceHistory).GetFace().ShowStretched(r, surface);
        }
    }

    // AoWWorldMap.pas:842
    public void HeroPnlPreDraw(LockedSurface surface)
    {
        Rect r = new(InfoPanel.AbsoluteLeft + HeroFrame.WinLeft, InfoPanel.AbsoluteTop + HeroFrame.WinTop, InfoPanel.AbsoluteLeft + HeroFrame.WinLeft + HeroFrame.WinWidth, InfoPanel.AbsoluteTop + HeroFrame.WinTop + HeroFrame.WinHeight);
        Campaign.TransferSettings.Leader?.ShowFace(r, surface);
    }

    // AoWWorldMap.pas:854
    public void SetBtnClick() => UI.CampaignSettingsDialog.Window.Visible = true;

    // AoWWorldMap.pas:859
    public void BookWinHide() => BookClosed();

    private static readonly string s_assignNewSpheresFormat = "Assign %s new spheres!";

    private static void RegisterWorldMapCampaign(GameEngine engine) => engine.RegisterEngineClasses([EngineClass.Of<CampaignScenarioPage>(), EngineClass.Of<WorldMapCampaignScenario>(), EngineClass.Of<WorldMapCampaign>()]);

    private static CampaignScenarioPage GetCampaignPage(WMCampaignScenarioPageList pages, int index) => pages[index];

    private static bool s_bookStartVisible;

    private static void SelectCampaignPath(WorldMapCampaign campaign, int index)
    {
        if (index is >= 0 and < 3)
        {
            campaign.State = 2;
            campaign.Page = 0;
            int path = WorldMapCampaign.GetCampaignScenario(campaign, campaign.CurrentScenarioId).Paths[index];
            campaign.SelectNextScenario(campaign.Scenarios.IndexOfId(path));
            if (WorldMapCampaign.GetCampaignScenario(campaign, campaign.NextScenarioId).Paths[3] == -1)
            {
                int path2 = WorldMapCampaign.GetCampaignScenario(campaign, campaign.CurrentScenarioId).Paths[3];
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
        dialog.ResetCampaign();
        dialog.UI.Engine.CloseHsm(null);
        dialog.UI.TitleWindow.MainWindow.Visible = true;
        dialog.UI.TitleWindow.TitlePanel.Visible = false;
        dialog.WorldPanel.Visible = true;
        dialog._pathFrame = 0;
        dialog._pathDelay = 5;
        dialog.DescriptionLabel.Text = "";
        dialog.UI.General.Manager.OverlayPriority = 249;
        _ = AudioHost.SongPlayer.Stop(false);
        _ = SetCurrentDir(dialog.UI.Engine.StartupDirectory);
        WorldMapCampaign campaign = dialog.Campaign;
        string song;
        if (IsEmpty(WorldMapCampaign.GetCampaignScenario(dialog.Campaign, campaign.CurrentScenarioId).ImageLibrary))
        {
            song = "Songs\\Title.IT";
        }
        else
        {
            WorldMapCampaign dialogCampaign = dialog.Campaign;
            song = $"Songs\\{WorldMapCampaign.GetCampaignScenario(dialog.Campaign, dialogCampaign.CurrentScenarioId).ImageLibrary}";
        }

        _ = AudioPlayback.PlaySong(song, 1, 0, true);
        WorldMapCampaign campaign2 = dialog.Campaign;
        dialog.PlayerPawnImage.CurrentImage = campaign2.Faction + 7;
        WorldMapCampaign campaign3 = dialog.Campaign;
        dialog.EnemyPawnImage.CurrentImage = 8 - campaign3.Faction;
        dialog.UpdateCampaignState();
    }
}
