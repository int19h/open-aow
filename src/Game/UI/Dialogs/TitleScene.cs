// Translated from AoWTitleScene.pas.
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

    public ImageLibrary TitleSceneImages;
    public UserWindow Window;
    public Memo LegalNoticeMemo;
    public Label DemoNoticeLabel;
    public Label NameLabel;
    public Label VersionLabel;
    private int _frame;
    private int _totalFrame;

    private int _mouseX;
    private int _mouseY;
    private int _selectedButton;
    private int _pressedButton;
    private int _hoverButton;
    private Rect _sceneRectangle;
    private bool _redrawBackground;
    private bool _showCredits;
    private List<StringList> _credits;
    private int _creditPage;
    private int _creditFrame;
    private bool _showIntro;

    private int _introFrame;
    private ImageLibraryFont _font;
    private ImageLibraryFont _creditsFont;

    // AoWTitleScene.pas:103
    public void DrawCredits(ref LockedSurface surface)
    {
        int X;
        int Y;
        int duration;
        int alpha;
        ShowInfo info = default;
        StringCollection lines;
        _creditFrame += 1;
        info.ShowMode = ShowMode.Blended;
        info.BlendMode = BlendMode.Alpha;
        lines = _credits[_creditPage];
        X = surface.Width / 2;
        var count = lines.Count;
        Y = surface.Height / 2 - ((count * 25 + _creditsFont.Height) / 2 + 35);
        duration = lines.Count * 30 + 20 + 50;
        if (lines.Count > 0)
        {
            alpha = _creditFrame;
            if (alpha > duration)
            {
                alpha = 100 - (alpha - duration) * 5;
            }
            else
            {
                alpha = alpha * 10;
            }

            info.Blend = alpha;
            if (alpha > 0)
            {
                if (alpha < 100)
                {
                    var line = lines[0];
                    _creditsFont.SetText(X, Y, line, surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), info);
                }
                else
                {
                    var line = lines[0];
                    _creditsFont.SetTextCentered(X, Y, line, surface);
                }
            }

            for (int I = 1, last = lines.Count - 1; I <= last; I++)
            {
                if (_creditFrame > duration)
                {
                    alpha = 100 - (_creditFrame - duration) * 5;
                }
                else
                {
                    alpha = (_creditFrame - (I * 30 + 20)) * 10;
                }

                if (alpha > 0)
                {
                    info.Blend = alpha;
                    if (alpha < 100)
                    {
                        var line = lines[I];
                        _creditsFont.SetText(X, Y + I * 25 + 5, line, surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), info);
                    }
                    else
                    {
                        var line = lines[I];
                        _creditsFont.SetTextCentered(X, Y + I * 25 + 5, line, surface);
                    }
                }
            }
        }

        if (_creditFrame > duration + 20)
        {
            _creditPage += 1;
            if (_creditPage < _credits.Count)
            {
                _creditFrame = 0;
            }
            else
            {
                _totalFrame = 0;
                _showCredits = false;
            }
        }
    }

    // AoWTitleScene.pas:148
    public void DrawIntro(ref LockedSurface surface)
    {
        int page;
        int X;
        int Y;
        int frame;
        int opacity;
        ShowInfo info = default;
        string text = "";
        _introFrame += 1;
        X = surface.Width / 2;
        Y = surface.Height / 2;
        page = _introFrame / 130;
        frame = _introFrame % 130;
        switch (frame)
        {
            case >= 0 and <= 10:
                opacity = frame * 10;
                break;
            case >= 11 and <= 30:
                opacity = 100;
                break;
            case >= 31 and <= 40:
                opacity = 100 - (frame - 30) * 10;
                break;
            default:
                opacity = 0;
                break;
        }

        if (opacity != 0)
        {
            switch (page)
            {
                case 0:
                    text = AbilityRules.TranslateResourceString(s_coDevelopedByText);
                    break;
                case 1:
                    text = AbilityRules.TranslateResourceString(s_coPublishedByText);
                    break;
                case 2:
                    text = "";
                    break;
            }

            if (opacity < 100)
            {
                info.ShowMode = ShowMode.Blended;
                info.BlendMode = BlendMode.Alpha;
                info.Blend = opacity;
                _creditsFont.SetText(X, Y - _creditsFont.Height / 2, text, surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), info);
            }
            else
            {
                _creditsFont.SetTextCentered(X, Y - _creditsFont.Height / 2, text, surface);
            }
        }

        if (frame > 45 && frame < 130)
        {
            switch (page)
            {
                case 0:
                    if (TitleSceneImages[4] != null)
                    {
                        var titleSceneImageLibraryHeight = TitleSceneImages[4];
                        var titleSceneImageLibraryWidth = TitleSceneImages[4];
                        TitleSceneImages[4].Show(X - 75 - titleSceneImageLibraryWidth.Width / 2, Y - titleSceneImageLibraryHeight.Height / 2, surface);
                    }

                    if (TitleSceneImages[5] != null)
                    {
                        var titleSceneImageLibraryHeight = TitleSceneImages[5];
                        var titleSceneImageLibraryWidth = TitleSceneImages[5];
                        TitleSceneImages[5].Show(X + 75 - titleSceneImageLibraryWidth.Width / 2, Y - titleSceneImageLibraryHeight.Height / 2, surface);
                    }

                    break;
                case 1:
                    var northAmerica = AbilityRules.TranslateResourceString(s_northAmericaText);
                    var titleSceneImageLibrary = TitleSceneImages[6];
                    _creditsFont.SetTextCentered(X - 75, Y - titleSceneImageLibrary.Height / 2 - 20, northAmerica, surface);
                    var europe = AbilityRules.TranslateResourceString(s_europeText);
                    var titleSceneImageLibrary2 = TitleSceneImages[6];
                    _creditsFont.SetTextCentered(X + 75 + 15, Y - titleSceneImageLibrary2.Height / 2 - 20, europe, surface);
                    if (TitleSceneImages[6] != null)
                    {
                        var titleSceneImageLibraryHeight = TitleSceneImages[6];
                        var titleSceneImageLibraryWidth = TitleSceneImages[6];
                        TitleSceneImages[6].Show(X - 75 - titleSceneImageLibraryWidth.Width / 2, Y - titleSceneImageLibraryHeight.Height / 2 + 20, surface);
                    }

                    if (TitleSceneImages[2] != null)
                    {
                        var titleSceneImageLibraryHeight = TitleSceneImages[2];
                        var titleSceneImageLibraryWidth = TitleSceneImages[2];
                        TitleSceneImages[2].Show(X + 75 + 15 - titleSceneImageLibraryWidth.Width / 2, Y - titleSceneImageLibraryHeight.Height / 2 + 20, surface);
                    }

                    break;
                case 2:
                    switch (frame)
                    {
                        case >= 45 and <= 55:
                            opacity = (frame - 45) * 10;
                            break;
                        case >= 121 and <= 130:
                            opacity = 100 - (frame - 120) * 10;
                            break;
                        default:
                            opacity = 101;
                            break;
                    }

                    if (opacity <= 100)
                    {
                        info.ShowMode = ShowMode.Blended;
                        info.BlendMode = BlendMode.Alpha;
                        info.Blend = opacity;
                        _creditsFont.SetText(X, Y - _creditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", surface, Set<SetTextModeElement>.Of(SetTextModeElement.Center), info);
                    }
                    else
                    {
                        _creditsFont.SetTextCentered(X, Y - _creditsFont.Height / 2, "Copyright (C) 1999,2000 Triumph Studios", surface);
                    }

                    break;
            }
        }

        if (page == 3)
        {
            _showIntro = false;
        }
    }

    // AoWTitleScene.pas:212
    public void DrawScene(ref LockedSurface surface)
    {
        int X;
        int Y;
        int frame;
        int spacing;
        int image;
        Rect R;
        _sceneRectangle.Left = 0;
        _sceneRectangle.Right = surface.Width;
        _sceneRectangle.Top = surface.Height / 2 - 240;
        _sceneRectangle.Bottom = _sceneRectangle.Top + 480;
        if (_redrawBackground)
        {
            R = _sceneRectangle;
            var titleSceneImageLibrary = TitleSceneImages[0];
            R.Top += titleSceneImageLibrary.Height;
            PixelFormats.ColorFillSurfaceDescription(R, 0, surface);
        }

        Y = _sceneRectangle.Top;
        frame = _frame - 25;
        if (frame < 0)
        {
            frame = 0;
        }

        frame = -((frame + 490) % TitleSceneImages[0].Width);
        TitleSceneImages[0].Show(frame, Y, surface);
        var titleSceneImageLibraryWidth = TitleSceneImages[0];
        TitleSceneImages[0].Show(frame + titleSceneImageLibraryWidth.Width, Y, surface);
        if (!_showCredits && _frame > 25)
        {
            if (_frame < 75)
            {
                frame = (_frame - 25) * 2;
            }
            else
            {
                if (_totalFrame <= 730)
                {
                    frame = 100;
                }
                else
                {
                    frame = (750 - _totalFrame) * 5;
                }
            }

            var titleSceneImageLibrary = TitleSceneImages[1];
            X = (_sceneRectangle.Right - _sceneRectangle.Left - titleSceneImageLibrary.Width) / 2;
            if (frame < 100)
            {
                var lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(frame);
                TitleSceneImages[1].ShowBlended(X, Y + 10, surface, lookupTable);
            }
            else
            {
                TitleSceneImages[3].Show(X, Y + 10, surface);
            }
        }

        spacing = 120;
        X = (TitleSceneImages[10].Width + _sceneRectangle.Left + _sceneRectangle.Right) / 2 - 320;
        for (int I = 0; I <= 4; I++)
        {
            if (I == _pressedButton && I == HitTest())
            {
                image = 2;
            }
            else
            {
                if (_pressedButton == -1 && I == _hoverButton)
                {
                    image = 1;
                }
                else
                {
                    image = 0;
                }
            }

            TitleSceneImages[I * 3 + 10 + image].Show(X + spacing * I, Y + 355, surface);
            if (I == HitTest())
            {
                _font.Color = 16777215;
                var translatedText = AbilityRules.TranslateResourceString(TitleCaptions.TitleButtons[I]);
                var titleSceneImageLibrary = TitleSceneImages[10];
                _font.SetTextCentered(X + spacing * I + titleSceneImageLibrary.Width / 2, Y + 445, translatedText, surface);
            }

            if (GameTables.GameEngine.AnimationDetail == 2)
            {
                TitleSceneImages[25].Show(X + spacing * I, Y + 355, surface);
            }
        }

        if (_showCredits)
        {
            DrawCredits(ref surface);
        }

        if (_showIntro)
        {
            DrawIntro(ref surface);
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
        _hoverButton = -1;
        _frame = 0;
        _totalFrame = 0;
        _showCredits = false;
        // { Original repeats the total-frame reset. }
        _totalFrame = 0;
        _showIntro = false;
    }

    // AoWTitleScene.pas:276
    public void AdvanceFrame()
    {
        _frame += 1;
        _totalFrame += 1;
        if (!_showCredits && _totalFrame == 750)
        {
            _creditPage = 0;
            _creditFrame = 0;
            _showCredits = true;
        }

        if (!_showIntro && _totalFrame == 300)
        {
            _introFrame = 0;
            _showIntro = true;
        }
    }

    // AoWTitleScene.pas:293
    public int HitTest()
    {
        int result;
        bool found;
        int X;
        int spacing;
        if (_sceneRectangle.Bottom - 125 < _mouseY && _mouseY < _sceneRectangle.Bottom)
        {
            spacing = 120;
            X = (TitleSceneImages[10].Width + (_sceneRectangle.Left + _sceneRectangle.Right)) / 2 - 320;
            result = 5;
            found = false;
            while (result != 0 && !found)
            {
                result -= 1;
                found = spacing * result + X < _mouseX && TitleSceneImages[10].Width + X + spacing * result > _mouseX;
            }

            if (!found)
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // AoWTitleScene.pas:315
    public void WindowDrawSurfaceEx(Surface surface, out bool drawn)
    {
        LockedSurface lockedSurface = default;
        try
        {
            if (Succeeded(surface.Lock(ref lockedSurface)))
            {
                DrawScene(ref lockedSurface);
                if (GameTables.GameEngine.RegisteredVersion() && !_showCredits)
                {
                    LegalNoticeMemo.Draw(lockedSurface);
                    DemoNoticeLabel.Draw(lockedSurface);
                    NameLabel.Draw(lockedSurface);
                }

                VersionLabel.Draw(lockedSurface);
                surface.Unlock();
            }

            drawn = true;
        }
        catch
        {
            throw new InvalidOperationException("Failure during TitleScene DrawSurface");
        }
    }

    // AoWTitleScene.pas:338
    public void TitleSceneCreate()
    {
        StringList lines;
        ResetScene();
        var registeredTo = GameTables.GameEngine.RegisteredTo();
        NameLabel.Text = registeredTo;
        var versionString = GameTables.GameEngine.GetVersionString();
        var format = AbilityRules.Format(AbilityRules.TranslateResourceString(s_versionFormat), [versionString]);
        VersionLabel.Text = format;
        _credits = new List<StringList>();
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_designerAndDirectorText));
        lines.Add("Lennart Sas");
        lines.Add("");
        lines.Add(AbilityRules.TranslateResourceString(s_additionalDesignText));
        lines.Add("Arno van Wingerden");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_leadProgrammerText));
        lines.Add("Arno van Wingerden");
        lines.Add("");
        lines.Add(AbilityRules.TranslateResourceString(s_programmerText));
        lines.Add("Josh Farley");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_artistsText));
        lines.Add("Lennart Sas");
        lines.Add("Thomas Cardin");
        lines.Add("Roy Postma");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_leadScenarioDesignerText));
        lines.Add("Menno Sas");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_scenarioDesignersText));
        lines.Add("Doug Gibson");
        lines.Add("Arnout Sas");
        lines.Add("Hugo van Wingerden");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_writerText));
        lines.Add("Raymond Bingham");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_musicText));
        lines.Add("Michiel van den Bos");
        lines.Add("");
        lines.Add(AbilityRules.TranslateResourceString(s_soundEffectText));
        lines.Add("Nando Eweg");
        _credits.Add(lines);
        lines.Add("");
        lines.Add(AbilityRules.TranslateResourceString(s_introVoiceText));
        lines.Add("John McGinn (English)");
        lines.Add("Norbert Gastell (German)");
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_galaxySoundSystemText));
        lines.Add("Carlo Vogelsang");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_internetMailSuiteText));
        lines.Add("Argo Software Design");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_qaCoreText));
        lines.Add("Cliff Benjamin");
        lines.Add("Ian Calderon");
        lines.Add("Ted Stephenson");
        lines.Add("Hugo van Wingerden");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString("Take 2 Europe"));
        lines.Add("");
        lines.Add("Producer");
        lines.Add("Ken Jordan");
        lines.Add("");
        lines.Add("Manuals + Translations");
        lines.Add("Frazer Nash");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add("Tarantula Studios testing suite");
        lines.Add("");
        lines.Add("Test Manager");
        lines.Add("Mark Lloyd");
        lines.Add("");
        lines.Add("Lead Tester");
        lines.Add("Paul Byers");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add("Testers");
        lines.Add("");
        lines.Add("Jim Thompson");
        lines.Add("Kit Brown");
        lines.Add("Julian Turner");
        lines.Add("Jim Cree");
        lines.Add("Lee Johnson");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_bizzText));
        lines.Add("Jay Wilbur");
        lines.Add("Mark Rein");
        _credits.Add(lines);
        lines = new StringList();
        lines.Add(AbilityRules.TranslateResourceString(s_specialThanksText));
        lines.Add("Eelco van der Snoek");
        lines.Add("Doug Gibson");
        lines.Add("Dietmer Tan");
        lines.Add("Arjan Brussee");
        lines.Add("Daniel Cook");
        _credits.Add(lines);
    }

    // AoWTitleScene.pas:448
    public void TitleSceneDestroy()
    {
        for (int I = _credits.Count - 1; I >= 0; I--)
        {
            (_credits[I] as IDisposable)?.Dispose();
        }

        _credits?.Clear();
    }

    // AoWTitleScene.pas:456
    public void WindowUpdate()
    {
        Window.SurfaceValid = false;
        if (!DialogHosts.FileDialogWindow.Window.Visible && !DialogHosts.ProgressDialog.Window.Visible)
        {
            AdvanceFrame();
        }
    }

    // AoWTitleScene.pas:462
    public void WindowMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        _mouseX = X;
        _mouseY = Y;
        _pressedButton = HitTest();
    }

    // AoWTitleScene.pas:470
    public void WindowMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        _mouseX = X;
        _mouseY = Y;
        if (_hoverButton != -1)
        {
            _redrawBackground = true;
        }

        _hoverButton = HitTest();
    }

    private static readonly string s_additionalDesignText = "Additional Design";

    private static readonly string s_artistsText = "Artists";

    private static readonly string s_bizzText = "Bizz";

    private static readonly string s_campaignText = "Campaign";

    private static readonly string s_coDevelopedByText = "Co-developed by:";

    private static readonly string s_coPublishedByText = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string s_designerAndDirectorText = "Designer & Director";

    private static readonly string s_europeText = "\x00\x00\x00\x00\x00\x00";

    private static readonly string s_galaxySoundSystemText = "Galaxy Sound System";

    private static readonly string s_internetMailSuiteText = "Internet Mail Suite";

    private static readonly string s_introVoiceText = "Intro Voice";

    private static readonly string s_leadProgrammerText = "Lead Programmer";

    private static readonly string s_leadScenarioDesignerText = "Lead Scenario Designer";

    private static readonly string s_musicText = "Music";

    private static readonly string s_northAmericaText = "\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00\x00";

    private static readonly string s_programmerText = "Programmer";

    private static readonly string s_qaCoreText = "QA Core";

    private static readonly string s_scenarioDesignersText = "Scenario Designers";

    private static readonly string s_soundEffectText = "Sound FX";

    private static readonly string s_specialThanksText = "Special Thanks";

    private static readonly string s_versionFormat = "Version: %s";

    private static readonly string s_writerText = "Writer";

    private static void ShowFactionSelection(FactionSelectionWindow dialog)
    {
        DialogHosts.TitleWindow.MainWindow.Visible = true;
        DialogHosts.FactionSelectionWindow.FactionPanel.Visible = true;
        dialog.TopLabel.Visible = true;
        var chooseFaction = AbilityRules.TranslateResourceString(LobbyText.ChooseFactionText);
        dialog.TopLabel.Text = chooseFaction;
        dialog.LightNameLabel.Visible = false;
        dialog.LightTextMemo.Visible = false;
        dialog.LightBackButton.Visible = false;
        dialog.LightStartButton.Visible = false;
        dialog.LightSphereImage.Visible = false;
        dialog.DarkNameLabel.Visible = false;
        dialog.DarkTextMemo.Visible = false;
        dialog.DarkBackButton.Visible = false;
        dialog.DarkStartButton.Visible = false;
        dialog.DarkSphereImage.Visible = false;
        dialog.Fade = -1;
        dialog.FactionPanel.ImageLibraryIndexTexture = 0;
        dialog.SelectionFlags[0] = 0;
    }

    private static void HideGameWindows(General controller)
    {
        if (!controller.WindowsHidden)
        {
            controller.WindowsHidden = true;
            controller.InfoVisible = DialogHosts.PlayerInfoWindow.MainWindow.Visible;
            controller.RealmVisible = DialogHosts.RealmWindow.Window.Visible;
            controller.MagicVisible = DialogHosts.MagicWindow.Window.Visible;
            controller.UnitVisible = DialogHosts.ControlWindow.MainWindow.Visible;
            controller.TargetVisible = DialogHosts.ChatWindow.MainWindow.Visible;
            controller.ScannerVisible = DialogHosts.ScannerWindow.MainWindow.Visible;
            controller.ControlVisible = DialogHosts.UnitWindow.MainWindow.Visible;
            controller.ChatVisible = DialogHosts.InfoWindow.MainWindow.Visible;
            controller.MapVisible = DialogHosts.MapWindow.MainWindow.Visible;
            for (int I = 0, last = DialogHosts.General.Manager.Children.Count - 1; I <= last; I++)
            {
                if (DialogHosts.General.Manager.Children[I] != DialogHosts.TaskBar.TaskWindow)
                {
                    DialogHosts.General.Manager.Children[I].Visible = false;
                }
            }
        }
    }

    private static void LoadGame(General controller, string fileName)
    {
        if (StrategicMap.Current != null)
        {
            GameTables.GameEngine.CloseHsm(null);
        }

        var loading = AbilityRules.TranslateResourceString(s_loadingText);
        var fileNameLocal = ExtractFileName(fileName);
        DialogHosts.ProgressDialog.TitleLabel.Text = loading + ": " + fileNameLocal + "...";
        DialogHosts.ProgressDialog.StatusLabel.Text = "";
        DialogHosts.ProgressDialog.ProgressBar.Position = 0;
        DialogHosts.ProgressDialog.ProgressBar.Max = 100;
        DialogHosts.ProgressDialog.Window.Visible = true;
        if (!DialogHosts.TitleScene.Window.Visible)
        {
            controller.Manager.OverlayPriority = 5000;
        }

        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        AudioPlayback.FadeMusicVolume(0, 1);
        GameTables.GameEngine.LoadHsm(fileName, false, (_, message, progression) => controller.MapProgress(message, progression), GameProfile.WorldMap);
        if (GameTables.GameEngine.Demo && StrategicMap.Current != null && !StrategicMap.Current.ShowAllPlayers)
        {
            CloseGame(controller);
        }

        controller.Manager.OverlayPriority = 0;
        DialogHosts.ProgressDialog.Window.Visible = false;
    }

    private static void StartGame(General controller)
    {
        controller.WindowsHidden = false;
        AudioHost.SongPlayer.Songs.Clear();
        if (StrategicMap.Current.GameSongs.Songs.Count != 0)
        {
            AudioHost.SongPlayer.Songs.AddStrings(StrategicMap.Current.GameSongs.Songs);
        }
        else
        {
            if (StrategicMap.Current.StartupSongs.Songs.Count != 0)
            {
                for (int I = 0, last = StrategicMap.Current.StartupSongs.Songs.Count - 1; I <= last; I++)
                {
                    var fileNotFound = AbilityRules.TranslateResourceString(s_fileNotFoundText);
                    if (CompareText(StrategicMap.Current.StartupSongs.SongNames[I], fileNotFound) != 0)
                    {
                        var song = StrategicMap.Current.StartupSongs.Songs[I];
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
        AudioPlayback.SetMusicVolume(DialogHosts.SoundDialog.MusicVolumeSlider.Position);
        DialogHosts.TaskBar.WorldStartMenuBar.Items.Clear();
        var background = AbilityRules.TranslateResourceString(s_backgroundText);
        DialogHosts.TaskBar.WorldStartMenuBar.Items.Add(background);
        if (GameTables.GameEngine.Campaign != null)
        {
            var campaign = AbilityRules.TranslateResourceString(s_campaignText);
            DialogHosts.TaskBar.WorldStartMenuBar.Items.Add(campaign);
        }

        DialogHosts.TitleScene.Window.Visible = false;
        DialogHosts.TitleWindow.MainWindow.Visible = false;
        DialogHosts.TaskBar.TaskWindow.Visible = true;
        DialogHosts.MapWindow.MainWindow.Visible = true;
        DialogHosts.ScannerWindow.MainWindow.Visible = true;
        DialogHosts.MapWindow.MapViewer.SetHexSpriteMap(StrategicMap.Current);
        DialogHosts.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        var mapLevel = StrategicMap.Current.MapContainer.MapLevels[0];
        DialogHosts.ScannerWindow.Scanner.SetLevel(mapLevel);
        DialogHosts.TaskBar.WorldTurnButton.ButtonChecked = false;
        DialogHosts.TaskBar.ChatButton.Visible = DialogHosts.SetupEvents.DirectPlay.Connected;
        controller.Manager.Aligned = false;
        DialogHosts.ScannerWindow.Scanner.PreviousWidth = 0;
        DialogHosts.ScannerWindow.Scanner.Incremental = false;
        DialogHosts.ScannerWindow.Scanner.Sized = false;
        DialogHosts.ScannerWindow.ScannerTabs.Items.Clear();
        var surface = AbilityRules.TranslateResourceString(s_surfaceText);
        DialogHosts.ScannerWindow.ScannerTabs.Items.Add(surface);
        if (StrategicMap.Current.MapContainer.LevelCount > 1)
        {
            var caverns = AbilityRules.TranslateResourceString(s_cavernsText);
            DialogHosts.ScannerWindow.ScannerTabs.Items.Add(caverns);
        }

        if (StrategicMap.Current.MapContainer.LevelCount > 2)
        {
            var depths = AbilityRules.TranslateResourceString(s_depthsText);
            DialogHosts.ScannerWindow.ScannerTabs.Items.Add(depths);
        }

        DialogHosts.ScannerWindow.ScannerTabs.Selected = 0;
        DialogHosts.ScannerWindow.ScannerTabs.Sized = false;
        DialogHosts.ScannerWindow.Scanner.SetTWidth(0);
        DialogHosts.ScannerWindow.Scanner.SetTHeight(0);
        DialogHosts.ScannerWindow.Scanner.SetTLeft(-1);
        DialogHosts.ScannerWindow.Scanner.SetTTop(-1);
        DialogHosts.ScannerWindow.Scanner.UpdateStep = -1;
        DialogHosts.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        var mapLevel2 = StrategicMap.Current.MapContainer.MapLevels[0];
        DialogHosts.ScannerWindow.Scanner.SetLevel(mapLevel2);
        DialogHosts.MapWindow.MapViewer.SetHexSpriteMap(StrategicMap.Current);
        DialogHosts.MapWindow.MapViewer.SceneWidth = DialogHosts.MapWindow.MainWindow.WinWidth - 24;
        DialogHosts.MapWindow.MapViewer.SceneHeight = DialogHosts.MapWindow.MainWindow.WinHeight - 24;
        StrategicMap.Current.Activate();
        DialogHosts.ControlWindow.MainWindow.Visible = false;
        DialogHosts.MapEvents.TheMapArmyDeselected();
        DialogHosts.MapEvents.TheMapStructureDeselected();
        DialogHosts.TaskBar.TBWorldUpdate();
        DialogHosts.PlayerInfoWindow.UpdatePlayer();
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatusElement.Activated))
        {
            var turnButton = DialogHosts.TaskBar.WorldTurnButton;
            if (StrategicMap.Current.SeatedPlayer == -1)
            {
                turnButton.CurrentPlayer = 12;
                turnButton.LastPlayer = 12;
                turnButton.AnimationFrame = turnButton.CurrentPlayer * 7;
                DialogHosts.TaskBar.AutoCombatCheckBox.Visible = false;
            }
            else
            {
                turnButton.LastPlayer = StrategicMap.Current.Players.GetPlayers(StrategicMap.Current.SeatedPlayer).PlayerRace;
                if (turnButton.LastPlayer == -1)
                {
                    turnButton.LastPlayer = 12;
                }

                turnButton.CurrentPlayer = turnButton.LastPlayer;
                turnButton.AnimationFrame = turnButton.LastPlayer * 7;
                DialogHosts.TaskBar.AutoCombatCheckBox.Visible = StrategicMap.Current.PlayMode == PlayMode.Simultaneous;
            }
        }

        DialogHosts.TaskBar.WorldClockDelay = 0;
        DialogHosts.TaskBar.ChatButton.TextColor = 14;
        DialogHosts.TaskBar.ChatFlash = 0;
        DialogHosts.VideoDialog.ChangeResolution(DialogHosts.VideoDialog.DisplayWidth, DialogHosts.VideoDialog.DisplayHeight);
    }

    private static void CloseGame(General controller)
    {
        GameTables.GameEngine.CloseHsm(null);
        GameTables.GameEngine.CloseCampaign();
        DialogHosts.ScannerWindow.Scanner.SetMap(null);
        DialogHosts.CombatScanner.Scanner.SetMap(null);
        AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        SetCurrentDir(GameTables.GameEngine.StartupDirectory);
        HideGameWindows(DialogHosts.General);
        DialogHosts.TaskBar.TaskWindow.Visible = false;
        DialogHosts.TitleWindow.MainWindow.Visible = false;
        DialogHosts.TitleScene.ResetScene();
        WindowActions.ShowTitleScene(DialogHosts.TitleScene, true);
    }

    private static readonly string s_loadingText = "Loading";

    private static readonly string s_fileNotFoundText = "File not found";

    private static readonly string s_backgroundText = "Background";

    private static readonly string s_surfaceText = "Surface";

    private static readonly string s_cavernsText = "Caverns";

    private static readonly string s_depthsText = "Depths";

    internal static void ShowTitleScene(TitleScene scene, bool music)
    {
        if (music)
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
        scene.TitleSceneImages.LoadDiscardableImageData();
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.FrameRate = 30;
        GameHost.MainForm.DisplayController.UseVerticalBlank = true;
        scene._redrawBackground = true;
        scene._pressedButton = -1;
        scene._hoverButton = -1;
        scene._font = DialogHosts.UIFonts.Age15WhiteOnGrey;
        scene._creditsFont = DialogHosts.UIFonts.Age15WhiteOnGrey;
        scene.Window.WindowManager.SetImageLibraryIndex(4);
        scene.Window.Visible = true;
    }

}
