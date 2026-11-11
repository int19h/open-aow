// Translated from AoWTitleScene.pas.
using Aow.Engine.Buffers;
using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// frame counters, mouse button, mouse position, selected/pressed/hover button, scene rectangle,
// redraw/credits, credits, credit page/frame, intro flag/frame, font
public partial class TitleScene : DataModule
{
    public TitleScene(Component owner) : base(owner)
    { }

    public ImageLibrary TitleSceneIL;
    public UserWindow Window;
    public Memo LegalNotice;
    public Label StrLbl;
    public Label NameLbl;
    public Label Copyright;
    public Label Version;
    public int FFrame;
    public int FTotalFrame;
    public MouseButton FMouseButton;
    public int FMouseX;
    public int FMouseY;
    public int FSelectedButton;
    public int FPressedButton;
    public int FHoverButton;
    public Rect FSceneRect;
    public bool FRedrawBackground;
    public bool FShowCredits;
    public List<StringList> FCredits;
    public int FCreditPage;
    public int FCreditFrame;
    public bool FShowIntro;
    public ByteArray3 FPadA5;
    public int FIntroFrame;
    public ImageLibraryFont FFont;
    public ImageLibraryFont FCreditsFont;

    // AoWTitleScene.pas:103
    public void DrawCredits(ref LockedSurface Surface)
    {
        int X;
        int Y;
        int Duration;
        int Alpha;
        ShowInfo Info = default;
        StringCollection Lines;
        FCreditFrame += 1;
        Info.ShowMode = ShowMode.Blended;
        Info.BlendMode = BlendMode.Alpha;
        Lines = FCredits[FCreditPage];
        X = Surface.Width / 2;
        var count = Lines.Count;
        Y = Surface.Height / 2 - ((count * 25 + FCreditsFont.Height) / 2 + 35);
        Duration = Lines.Count * 30 + 20 + 50;
        if (Lines.Count > 0)
        {
            Alpha = FCreditFrame;
            if (Alpha > Duration)
            {
                Alpha = 100 - (Alpha - Duration) * 5;
            }
            else
            {
                Alpha = Alpha * 10;
            }

            Info.Blend = Alpha;
            if (Alpha > 0)
            {
                if (Alpha < 100)
                {
                    var line = Lines[0];
                    FCreditsFont.SetTextEx(X, Y, line, Surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), Info);
                }
                else
                {
                    var line = Lines[0];
                    FCreditsFont.SetTextCentered(X, Y, line, Surface);
                }
            }

            for (int I = 1, last = Lines.Count - 1; I <= last; I++)
            {
                if (FCreditFrame > Duration)
                {
                    Alpha = 100 - (FCreditFrame - Duration) * 5;
                }
                else
                {
                    Alpha = (FCreditFrame - (I * 30 + 20)) * 10;
                }

                if (Alpha > 0)
                {
                    Info.Blend = Alpha;
                    if (Alpha < 100)
                    {
                        var line = Lines[I];
                        FCreditsFont.SetTextEx(X, Y + I * 25 + 5, line, Surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), Info);
                    }
                    else
                    {
                        var line = Lines[I];
                        FCreditsFont.SetTextCentered(X, Y + I * 25 + 5, line, Surface);
                    }
                }
            }
        }

        if (FCreditFrame > Duration + 20)
        {
            FCreditPage += 1;
            if (FCreditPage < FCredits.Count)
            {
                FCreditFrame = 0;
            }
            else
            {
                FTotalFrame = 0;
                FShowCredits = false;
            }
        }
    }

    // AoWTitleScene.pas:148
    public void DrawIntro(ref LockedSurface Surface)
    {
        int Page;
        int X;
        int Y;
        int Frame;
        int Opacity;
        ShowInfo Info = default;
        string Text = "";
        FIntroFrame += 1;
        X = Surface.Width / 2;
        Y = Surface.Height / 2;
        Page = FIntroFrame / 130;
        Frame = FIntroFrame % 130;
        switch (Frame)
        {
            case >= 0 and <= 10:
                Opacity = Frame * 10;
                break;
            case >= 11 and <= 30:
                Opacity = 100;
                break;
            case >= 31 and <= 40:
                Opacity = 100 - (Frame - 30) * 10;
                break;
            default:
                Opacity = 0;
                break;
        }

        if (Opacity != 0)
        {
            switch (Page)
            {
                case 0:
                    Text = AbilityRules.TranslateRStr(CoDevelopedByRStr);
                    break;
                case 1:
                    Text = AbilityRules.TranslateRStr(CoPublishedByRStr);
                    break;
                case 2:
                    Text = "";
                    break;
            }

            if (Opacity < 100)
            {
                Info.ShowMode = ShowMode.Blended;
                Info.BlendMode = BlendMode.Alpha;
                Info.Blend = Opacity;
                FCreditsFont.SetTextEx(X, Y - FCreditsFont.Height / 2, Text, Surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), Info);
            }
            else
            {
                FCreditsFont.SetTextCentered(X, Y - FCreditsFont.Height / 2, Text, Surface);
            }
        }

        if (Frame > 45 && Frame < 130)
        {
            switch (Page)
            {
                case 0:
                    if (TitleSceneIL[4] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[4];
                        var titleSceneILWidth = TitleSceneIL[4];
                        TitleSceneIL[4].Show(X - 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2, Surface);
                    }

                    if (TitleSceneIL[5] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[5];
                        var titleSceneILWidth = TitleSceneIL[5];
                        TitleSceneIL[5].Show(X + 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2, Surface);
                    }

                    break;
                case 1:
                    var northAmerica = AbilityRules.TranslateRStr(NorthAmericaRStr);
                    var titleSceneIL = TitleSceneIL[6];
                    FCreditsFont.SetTextCentered(X - 75, Y - titleSceneIL.Height / 2 - 20, northAmerica, Surface);
                    var europe = AbilityRules.TranslateRStr(EuropeRStr);
                    var titleSceneIL2 = TitleSceneIL[6];
                    FCreditsFont.SetTextCentered(X + 75 + 15, Y - titleSceneIL2.Height / 2 - 20, europe, Surface);
                    if (TitleSceneIL[6] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[6];
                        var titleSceneILWidth = TitleSceneIL[6];
                        TitleSceneIL[6].Show(X - 75 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2 + 20, Surface);
                    }

                    if (TitleSceneIL[2] != null)
                    {
                        var titleSceneILHeight = TitleSceneIL[2];
                        var titleSceneILWidth = TitleSceneIL[2];
                        TitleSceneIL[2].Show(X + 75 + 15 - titleSceneILWidth.Width / 2, Y - titleSceneILHeight.Height / 2 + 20, Surface);
                    }

                    break;
                case 2:
                    switch (Frame)
                    {
                        case >= 45 and <= 55:
                            Opacity = (Frame - 45) * 10;
                            break;
                        case >= 121 and <= 130:
                            Opacity = 100 - (Frame - 120) * 10;
                            break;
                        default:
                            Opacity = 101;
                            break;
                    }

                    if (Opacity <= 100)
                    {
                        Info.ShowMode = ShowMode.Blended;
                        Info.BlendMode = BlendMode.Alpha;
                        Info.Blend = Opacity;
                        FCreditsFont.SetTextEx(X, Y - FCreditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", Surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), Info);
                    }
                    else
                    {
                        FCreditsFont.SetTextCentered(X, Y - FCreditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", Surface);
                    }

                    break;
            }
        }

        if (Page == 3)
        {
            FShowIntro = false;
        }
    }

    // AoWTitleScene.pas:212
    public void DrawScene(ref LockedSurface Surface)
    {
        int X;
        int Y;
        int Frame;
        int Spacing;
        int Image;
        Rect R;
        FSceneRect.Left = 0;
        FSceneRect.Right = Surface.Width;
        FSceneRect.Top = Surface.Height / 2 - 240;
        FSceneRect.Bottom = FSceneRect.Top + 480;
        if (FRedrawBackground)
        {
            R = FSceneRect;
            var titleSceneIL = TitleSceneIL[0];
            R.Top += titleSceneIL.Height;
            PixelFormats.ColorFillSD(R, 0, Surface);
        }

        Y = FSceneRect.Top;
        Frame = FFrame - 25;
        if (Frame < 0)
        {
            Frame = 0;
        }

        Frame = -((Frame + 490) % TitleSceneIL[0].Width);
        TitleSceneIL[0].Show(Frame, Y, Surface);
        var titleSceneILWidth = TitleSceneIL[0];
        TitleSceneIL[0].Show(Frame + titleSceneILWidth.Width, Y, Surface);
        if (!FShowCredits && FFrame > 25)
        {
            if (FFrame < 75)
            {
                Frame = (FFrame - 25) * 2;
            }
            else
            {
                if (FTotalFrame <= 730)
                {
                    Frame = 100;
                }
                else
                {
                    Frame = (750 - FTotalFrame) * 5;
                }
            }

            var titleSceneIL = TitleSceneIL[1];
            X = (FSceneRect.Right - FSceneRect.Left - titleSceneIL.Width) / 2;
            if (Frame < 100)
            {
                var lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(Frame);
                TitleSceneIL[1].ShowBlended(X, Y + 10, Surface, lookupTable);
            }
            else
            {
                TitleSceneIL[3].Show(X, Y + 10, Surface);
            }
        }

        Spacing = 120;
        X = (TitleSceneIL[10].Width + FSceneRect.Left + FSceneRect.Right) / 2 - 320;
        for (int I = 0; I <= 4; I++)
        {
            if (I == FPressedButton && I == HitTest())
            {
                Image = 2;
            }
            else
            {
                if (FPressedButton == -1 && I == FHoverButton)
                {
                    Image = 1;
                }
                else
                {
                    Image = 0;
                }
            }

            TitleSceneIL[I * 3 + 10 + Image].Show(X + Spacing * I, Y + 355, Surface);
            if (I == HitTest())
            {
                FFont.Color = 16777215;
                var translateRStr = AbilityRules.TranslateRStr(TitleCaptions.TitleButtons[I]);
                var titleSceneIL = TitleSceneIL[10];
                FFont.SetTextCentered(X + Spacing * I + titleSceneIL.Width / 2, Y + 445, translateRStr, Surface);
            }

            if (GameTables.AoWEngine.FAnimationDetail == 2)
            {
                TitleSceneIL[25].Show(X + Spacing * I, Y + 355, Surface);
            }
        }

        if (FShowCredits)
        {
            DrawCredits(ref Surface);
        }

        if (FShowIntro)
        {
            DrawIntro(ref Surface);
        }
    }

    // AoWTitleScene.pas:261
    public void HideScene()
    {
        Window.Visible = false;
    }

    // AoWTitleScene.pas:266
    public void ResetScene()
    {
        FHoverButton = -1;
        FFrame = 0;
        FTotalFrame = 0;
        FShowCredits = false;
        // { Original repeats the total-frame reset. }
        FTotalFrame = 0;
        FShowIntro = false;
    }

    // AoWTitleScene.pas:276
    public void AdvanceFrame()
    {
        FFrame += 1;
        FTotalFrame += 1;
        if (!FShowCredits && FTotalFrame == 750)
        {
            FCreditPage = 0;
            FCreditFrame = 0;
            FShowCredits = true;
        }

        if (!FShowIntro && FTotalFrame == 300)
        {
            FIntroFrame = 0;
            FShowIntro = true;
        }
    }

    // AoWTitleScene.pas:293
    public int HitTest()
    {
        int Result;
        bool Found;
        int X;
        int Spacing;
        if (FSceneRect.Bottom - 125 < FMouseY && FMouseY < FSceneRect.Bottom)
        {
            Spacing = 120;
            X = (TitleSceneIL[10].Width + (FSceneRect.Left + FSceneRect.Right)) / 2 - 320;
            Result = 5;
            Found = false;
            while (Result != 0 && !Found)
            {
                Result -= 1;
                Found = Spacing * Result + X < FMouseX && TitleSceneIL[10].Width + X + Spacing * Result > FMouseX;
            }

            if (!Found)
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // AoWTitleScene.pas:315
    public void WindowDrawSurfaceEx(Surface Surface, ref bool Drawn)
    {
        LockedSurface SD = default;
        try
        {
            if (Succeeded(Surface.Lock(ref SD)))
            {
                DrawScene(ref SD);
                if (GameTables.AoWEngine.RegisteredVersion() && !FShowCredits)
                {
                    LegalNotice.Draw(SD);
                    StrLbl.Draw(SD);
                    NameLbl.Draw(SD);
                }

                Version.Draw(SD);
                Surface.Unlock();
            }

            Drawn = true;
        }
        catch
        {
            throw new InvalidOperationException("Failure during TitleScene DrawSurface");
        }
    }

    // AoWTitleScene.pas:338
    public void TitleSceneCreate()
    {
        StringList Lines;
        ResetScene();
        var registeredTo = GameTables.AoWEngine.RegisteredTo();
        NameLbl.Text = registeredTo;
        var versionStr = GameTables.AoWEngine.GetVersionStr();
        var format = AbilityRules.Format(AbilityRules.TranslateRStr(VersionXRStr), [versionStr]);
        Version.Text = format;
        FCredits = new List<StringList>();
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(DesignerAndDirectorRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("");
        Lines.Add(AbilityRules.TranslateRStr(AdditionalDesignRStr));
        Lines.Add("Arno van Wingerden");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(LeadProgrammerRStr));
        Lines.Add("Arno van Wingerden");
        Lines.Add("");
        Lines.Add(AbilityRules.TranslateRStr(ProgrammerRStr));
        Lines.Add("Josh Farley");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(ArtistsRStr));
        Lines.Add("Lennart Sas");
        Lines.Add("Thomas Cardin");
        Lines.Add("Roy Postma");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(LeadScenarioDesignerRStr));
        Lines.Add("Menno Sas");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(ScenarioDesignersRStr));
        Lines.Add("Doug Gibson");
        Lines.Add("Arnout Sas");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(WriterRStr));
        Lines.Add("Raymond Bingham");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(MusicRStr));
        Lines.Add("Michiel van den Bos");
        Lines.Add("");
        Lines.Add(AbilityRules.TranslateRStr(SoundFXRStr));
        Lines.Add("Nando Eweg");
        FCredits.Add(Lines);
        Lines.Add("");
        Lines.Add(AbilityRules.TranslateRStr(IntroVoiceRStr));
        Lines.Add("John McGinn (English)");
        Lines.Add("Norbert Gastell (German)");
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(GalaxySoundSystemRStr));
        Lines.Add("Carlo Vogelsang");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(InternetMailSuiteRStr));
        Lines.Add("Argo Software Design");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(QACoreRStr));
        Lines.Add("Cliff Benjamin");
        Lines.Add("Ian Calderon");
        Lines.Add("Ted Stephenson");
        Lines.Add("Hugo van Wingerden");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr("Take 2 Europe"));
        Lines.Add("");
        Lines.Add("Producer");
        Lines.Add("Ken Jordan");
        Lines.Add("");
        Lines.Add("Manuals + Translations");
        Lines.Add("Frazer Nash");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add("Tarantula Studios testing suite");
        Lines.Add("");
        Lines.Add("Test Manager");
        Lines.Add("Mark Lloyd");
        Lines.Add("");
        Lines.Add("Lead Tester");
        Lines.Add("Paul Byers");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add("Testers");
        Lines.Add("");
        Lines.Add("Jim Thompson");
        Lines.Add("Kit Brown");
        Lines.Add("Julian Turner");
        Lines.Add("Jim Cree");
        Lines.Add("Lee Johnson");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(BizzRStr));
        Lines.Add("Jay Wilbur");
        Lines.Add("Mark Rein");
        FCredits.Add(Lines);
        Lines = new StringList();
        Lines.Add(AbilityRules.TranslateRStr(SpecialThanksRStr));
        Lines.Add("Eelco van der Snoek");
        Lines.Add("Doug Gibson");
        Lines.Add("Dietmer Tan");
        Lines.Add("Arjan Brussee");
        Lines.Add("Daniel Cook");
        FCredits.Add(Lines);
    }

    // AoWTitleScene.pas:448
    public void TitleSceneDestroy()
    {
        for (int I = FCredits.Count - 1; I >= 0; I--)
        {
            (FCredits[I] as IDisposable)?.Dispose();
        }

        FCredits?.Clear();
    }

    // AoWTitleScene.pas:456
    public void WindowUpdate()
    {
        Window.FSurfaceValid = false;
        if (!DialogHosts.FileDlgWindow.FileDialog.Visible && !DialogHosts.ProgressDialog.ProgDlg.Visible)
        {
            AdvanceFrame();
        }
    }

    // AoWTitleScene.pas:462
    public void WindowMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FMouseX = X;
        FMouseY = Y;
        FMouseButton = Button;
        FPressedButton = HitTest();
    }

    // AoWTitleScene.pas:470
    public void WindowMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        FMouseX = X;
        FMouseY = Y;
        if (FHoverButton != -1)
        {
            FRedrawBackground = true;
        }

        FHoverButton = HitTest();
    }

    private static readonly string AdditionalDesignRStr = "Additional Design";

    private static readonly string ArtistsRStr = "Artists";

    private static readonly string BizzRStr = "Bizz";

    private static readonly string CampaignRStr = "Campaign";

    private static readonly string CoDevelopedByRStr = "Co-developed by:";

    private static readonly string CoPublishedByRStr = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string DesignerAndDirectorRStr = "Designer & Director";

    private static readonly string EuropeRStr = "\x00\x00\x00\x00\x00\x00";

    private static readonly string GalaxySoundSystemRStr = "Galaxy Sound System";

    private static readonly string InternetMailSuiteRStr = "Internet Mail Suite";

    private static readonly string IntroVoiceRStr = "Intro Voice";

    private static readonly string LeadProgrammerRStr = "Lead Programmer";

    private static readonly string LeadScenarioDesignerRStr = "Lead Scenario Designer";

    private static readonly string MusicRStr = "Music";

    private static readonly string NorthAmericaRStr = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string ProgrammerRStr = "Programmer";

    private static readonly string QACoreRStr = "QA Core";

    private static readonly string ScenarioDesignersRStr = "Scenario Designers";

    private static readonly string SoundFXRStr = "Sound FX";

    private static readonly string SpecialThanksRStr = "Special Thanks";

    private static readonly string VersionXRStr = "Version: %s";

    private static readonly string WriterRStr = "Writer";

    private static void ShowFactionSelection(FactionSelectionWindow Dialog)
    {
        DialogHosts.TitleWindow.TitleWin.Visible = true;
        DialogHosts.SelFactionWin.FactionPnl.Visible = true;
        Dialog.TopLbl.Visible = true;
        var chooseFaction = AbilityRules.TranslateRStr(LobbyText.ChooseFactionRStr);
        Dialog.TopLbl.Text = chooseFaction;
        Dialog.LName.Visible = false;
        Dialog.LText.Visible = false;
        Dialog.LBack.Visible = false;
        Dialog.LStart.Visible = false;
        Dialog.LSphere.Visible = false;
        Dialog.DName.Visible = false;
        Dialog.DText.Visible = false;
        Dialog.DBack.Visible = false;
        Dialog.DStart.Visible = false;
        Dialog.DSphere.Visible = false;
        Dialog.FFade = -1;
        Dialog.FactionPnl.ILIndexTexture = 0;
        Dialog.F_SelFactionWin_8C[0] = 0;
    }

    private static void HideGameWindows(General Controller)
    {
        if (!Controller.FWindowsHidden)
        {
            Controller.FWindowsHidden = true;
            Controller.FInfoVisible = DialogHosts.PInfoW.PInfoWin.Visible;
            Controller.FRealmVisible = DialogHosts.RealmWin.Dlg.Visible;
            Controller.FMagicVisible = DialogHosts.MagicWin.Dlg.Visible;
            Controller.FUnitVisible = DialogHosts.ControlWin.MainWindow.Visible;
            Controller.FTargetVisible = DialogHosts.ChatWindow.ChatWin.Visible;
            Controller.FScannerVisible = DialogHosts.SWindow.MainWindow.Visible;
            Controller.FControlVisible = DialogHosts.UnitWindow.UnitWin.Visible;
            Controller.FChatVisible = DialogHosts.IWindow.InfoWin.Visible;
            Controller.FMapVisible = DialogHosts.MWindow.MainWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.FChildren.Count - 1; I <= last; I++)
            {
                if (DialogHosts.General.Manager.FChildren[I] != DialogHosts.TBWindow.TaskWindow)
                {
                    ((UIComponent)DialogHosts.General.Manager.FChildren[I]).Visible = false;
                }
            }
        }
    }

    private static void LoadGame(General Controller, string FileName)
    {
        if (AoWHSMap.Current != null)
        {
            GameTables.AoWEngine.CloseHSM(null);
        }

        var loading = AbilityRules.TranslateRStr(LoadingRStr);
        var fileName = ExtractFileName(FileName);
        DialogHosts.ProgressDialog.Title.Text = loading + ": " + fileName + "...";
        DialogHosts.ProgressDialog.StatusLbl.Text = "";
        DialogHosts.ProgressDialog.Bar.Position = 0;
        DialogHosts.ProgressDialog.Bar.Max = 100;
        DialogHosts.ProgressDialog.ProgDlg.Visible = true;
        if (!DialogHosts.TitleScene.Window.Visible)
        {
            Controller.Manager.OverlayPriority = 5000;
        }

        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        AudioPlayback.FadeMusicVolume(0, 1);
        GameTables.AoWEngine.LoadHSM(FileName, false, (_, Message, Progression) => Controller.MapProgress(Message, Progression), GameProfile.WorldMap);
        if (GameTables.AoWEngine.FDemo && AoWHSMap.Current != null && !AoWHSMap.Current.FShowAllPlayers)
        {
            CloseGame(Controller);
        }

        Controller.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.ProgDlg.Visible = false;
    }

    private static void StartGame(General Controller)
    {
        Controller.FWindowsHidden = false;
        AudioHost.SongPlayer.Songs.Clear();
        if (AoWHSMap.Current.FGameSongs.Songs.Count != 0)
        {
            AudioHost.SongPlayer.Songs.AddStrings(AoWHSMap.Current.FGameSongs.Songs);
        }
        else
        {
            if (AoWHSMap.Current.FStartupSongs.Songs.Count != 0)
            {
                for (int I = 0, last = AoWHSMap.Current.FStartupSongs.Songs.Count - 1; I <= last; I++)
                {
                    var fileNotFound = AbilityRules.TranslateRStr(FileNotFoundRStr);
                    if (CompareText(AoWHSMap.Current.FStartupSongs.SongNames[I], fileNotFound) != 0)
                    {
                        var song = AoWHSMap.Current.FStartupSongs.Songs[I];
                        AudioHost.SongPlayer.Songs.Add(song);
                    }
                }
            }
            else
            {
                AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
            }
        }

        AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(DialogHosts.SoundDlg.MusicVolume.Position);
        DialogHosts.TBWindow.StartMenuBar.Items.Clear();
        var background = AbilityRules.TranslateRStr(BackgroundRStr);
        DialogHosts.TBWindow.StartMenuBar.Items.Add(background);
        if (GameTables.AoWEngine.FCampaign != null)
        {
            var campaign = AbilityRules.TranslateRStr(CampaignRStr);
            DialogHosts.TBWindow.StartMenuBar.Items.Add(campaign);
        }

        DialogHosts.TitleScene.Window.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        DialogHosts.TBWindow.TaskWindow.Visible = true;
        DialogHosts.MWindow.MainWindow.Visible = true;
        DialogHosts.SWindow.MainWindow.Visible = true;
        DialogHosts.MWindow.MapViewer.SetHSMap(AoWHSMap.Current);
        DialogHosts.SWindow.Scanner.SetMap(AoWHSMap.Current);
        var mapLevel = AoWHSMap.Current.MapContainer.MapLevels[0];
        DialogHosts.SWindow.Scanner.SetLevel(mapLevel);
        DialogHosts.TBWindow.TurnButton.FButtonChecked = false;
        DialogHosts.TBWindow.ChatBtn.Visible = DialogHosts.SetupEvents.DPlay.Connected;
        Controller.Manager.FAligned = false;
        DialogHosts.SWindow.Scanner.FPreviousWidth = 0;
        DialogHosts.SWindow.Scanner.FIncremental = false;
        DialogHosts.SWindow.Scanner.FSized = false;
        DialogHosts.SWindow.ScannerTab.Items.Clear();
        var surface = AbilityRules.TranslateRStr(SurfaceRStr);
        DialogHosts.SWindow.ScannerTab.Items.Add(surface);
        if (AoWHSMap.Current.MapContainer.LevelCount > 1)
        {
            var caverns = AbilityRules.TranslateRStr(CavernsRStr);
            DialogHosts.SWindow.ScannerTab.Items.Add(caverns);
        }

        if (AoWHSMap.Current.MapContainer.LevelCount > 2)
        {
            var depths = AbilityRules.TranslateRStr(DepthsRStr);
            DialogHosts.SWindow.ScannerTab.Items.Add(depths);
        }

        DialogHosts.SWindow.ScannerTab.Selected = 0;
        DialogHosts.SWindow.ScannerTab.FSized = false;
        DialogHosts.SWindow.Scanner.SetTWidth(0);
        DialogHosts.SWindow.Scanner.SetTHeight(0);
        DialogHosts.SWindow.Scanner.SetTLeft(-1);
        DialogHosts.SWindow.Scanner.SetTTop(-1);
        DialogHosts.SWindow.Scanner.FUpdateStep = -1;
        DialogHosts.SWindow.Scanner.SetMap(AoWHSMap.Current);
        var mapLevel2 = AoWHSMap.Current.MapContainer.MapLevels[0];
        DialogHosts.SWindow.Scanner.SetLevel(mapLevel2);
        DialogHosts.MWindow.MapViewer.SetHSMap(AoWHSMap.Current);
        DialogHosts.MWindow.MapViewer.SceneWidth = DialogHosts.MWindow.MainWindow.WinWidth - 24;
        DialogHosts.MWindow.MapViewer.SceneHeight = DialogHosts.MWindow.MainWindow.WinHeight - 24;
        AoWHSMap.Current.Activate();
        DialogHosts.ControlWin.MainWindow.Visible = false;
        DialogHosts.MapEvents.TheMapArmyDeselected();
        DialogHosts.MapEvents.TheMapStructureDeselected();
        DialogHosts.TBWindow.TBWorldUpdate();
        DialogHosts.PInfoW.UpdatePlayer();
        if (AoWHSMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            var aowTurnButton = DialogHosts.TBWindow.TurnButton;
            if (AoWHSMap.Current.FSeatedPlayer == -1)
            {
                aowTurnButton.FCurPlayer = 12;
                aowTurnButton.FLastPlayer = 12;
                aowTurnButton.FAnimationFrame = aowTurnButton.FCurPlayer * 7;
                DialogHosts.TBWindow.AutoBtn.Visible = false;
            }
            else
            {
                aowTurnButton.FLastPlayer = AoWHSMap.Current.FPlayers.GetPlayers(AoWHSMap.Current.FSeatedPlayer).FPlayerRace;
                if (aowTurnButton.FLastPlayer == -1)
                {
                    aowTurnButton.FLastPlayer = 12;
                }

                aowTurnButton.FCurPlayer = aowTurnButton.FLastPlayer;
                aowTurnButton.FAnimationFrame = aowTurnButton.FLastPlayer * 7;
                DialogHosts.TBWindow.AutoBtn.Visible = AoWHSMap.Current.FPlayMode == PlayMode.Simultaneous;
            }
        }

        DialogHosts.TBWindow.FWorldClockDelay = 0;
        DialogHosts.TBWindow.ChatBtn.TextColor = 14;
        DialogHosts.TBWindow.FChatFlash = 0;
        DialogHosts.VideoDialog.ChangeResolution(DialogHosts.VideoDialog.FDisplayWidth, DialogHosts.VideoDialog.FDisplayHeight);
    }

    private static void CloseGame(General Controller)
    {
        GameTables.AoWEngine.CloseHSM(null);
        GameTables.AoWEngine.CloseCampaign();
        DialogHosts.SWindow.Scanner.SetMap(null);
        DialogHosts.TCScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.AoWEngine.StartupDirectory);
        HideGameWindows(DialogHosts.General);
        DialogHosts.TBWindow.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.TitleWin.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    private static readonly string LoadingRStr = "Loading";

    private static readonly string FileNotFoundRStr = "File not found";

    private static readonly string BackgroundRStr = "Background";

    private static readonly string SurfaceRStr = "Surface";

    private static readonly string CavernsRStr = "Caverns";

    private static readonly string DepthsRStr = "Depths";

    internal static void ShowTitleScene(TitleScene Scene, bool Music)
    {
        if (Music)
        {
            bool condition = AudioHost.SongPlayer.CurrentIndex == -1;
            if (!condition)
            {
                var count = AudioHost.SongPlayer.Songs.Count;
                condition = AudioHost.SongPlayer.CurrentIndex >= count;
            }

            if (condition || AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
            {
                AudioHost.SongPlayer.Songs.Clear();
                AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
                AudioHost.SongPlayer.Play(0);
            }
        }

        DialogHosts.General.Manager.OverlayPriority = 0;
        Scene.TitleSceneIL.LoadDiscardableImageData();
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.FrameRate = 30;
        GameHost.MainForm.DXDC.UseVerticalBlank = true;
        Scene.FRedrawBackground = true;
        Scene.FPressedButton = -1;
        Scene.FHoverButton = -1;
        Scene.FFont = DialogHosts.FontModule.AoW15WhiteGrey;
        Scene.FCreditsFont = DialogHosts.FontModule.AoW15WhiteGrey;
        Scene.Window.WinManager.SetILBIndex(4);
        Scene.Window.Visible = true;
    }

}
