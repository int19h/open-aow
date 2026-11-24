#nullable disable warnings
// Translated from AoWTitleScene.pas.
using Aow.Formats;

using Aow.Game.Audio;

using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Game.Host;

using Aow.Game.Magic.Abilities;

using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Graphics;

using Aow.Rtl;
using static Aow.Rtl.DelphiPaths;
using static Aow.Rtl.DelphiTextCompare;

namespace Aow.Game.UI.Dialogs;

// frame counters, mouse button, mouse position, selected/pressed/hover button, scene rectangle,
// redraw/credits, credits, credit page/frame, intro flag/frame, font
public sealed partial class TitleScene(UIContext ui) : UIModule(ui)
{
    public ImageLibrary TitleSceneImages { get; private set; }
    public UserWindow Window { get; private set; }
    public Memo LegalNoticeMemo { get; private set; }
    public Label DemoNoticeLabel { get; private set; }
    public Label NameLabel { get; private set; }
    public Label VersionLabel { get; private set; }

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
    private List<DelphiStringList> _credits;
    private int _creditPage;
    private int _creditFrame;
    private bool _showIntro;

    private int _introFrame;
    private ImageLibraryFont _font;
    private ImageLibraryFont _creditsFont;

    // AoWTitleScene.pas:103
    public void DrawCredits(ref readonly LockedSurface surface)
    {
        ShowInfo info = default;
        _creditFrame += 1;
        info.ShowMode = ShowMode.Blended;
        info.BlendMode = BlendMode.Alpha;
        DelphiStringCollection lines = _credits[_creditPage];
        int x = surface.Width / 2;
        int count = lines.Count;
        int y = (surface.Height / 2) - ((((count * 25) + _creditsFont.Height) / 2) + 35);
        int duration = (lines.Count * 30) + 20 + 50;
        if (lines.Count > 0)
        {
            int alpha = _creditFrame;
            alpha = alpha > duration ? 100 - ((alpha - duration) * 5) : alpha * 10;

            info.Blend = alpha;
            if (alpha > 0)
            {
                if (alpha < 100)
                {
                    string line = lines[0];
                    _creditsFont.SetText(x, y, text: line, lockedSurface: surface, mode: SetTextModes.Center, showInfo: info);
                }
                else
                {
                    string line = lines[0];
                    _creditsFont.SetTextCentered(x, y, text: line, lockedSurface: surface);
                }
            }

            for (int i = 1, last = lines.Count - 1; i <= last; i++)
            {
                alpha = _creditFrame > duration ? 100 - ((_creditFrame - duration) * 5) : (_creditFrame - ((i * 30) + 20)) * 10;

                if (alpha > 0)
                {
                    info.Blend = alpha;
                    if (alpha < 100)
                    {
                        string line = lines[i];
                        _creditsFont.SetText(x, y + (i * 25) + 5, text: line, lockedSurface: surface, mode: SetTextModes.Center, showInfo: info);
                    }
                    else
                    {
                        string line = lines[i];
                        _creditsFont.SetTextCentered(x, y + (i * 25) + 5, text: line, lockedSurface: surface);
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
    public void DrawIntro(ref readonly LockedSurface surface)
    {
        ShowInfo info = default;
        _introFrame += 1;
        int x = surface.Width / 2;
        int y = surface.Height / 2;
        int page = _introFrame / 130;
        int frame = _introFrame % 130;
        int opacity = frame switch
        {
            >= 0 and <= 10 => frame * 10,
            >= 11 and <= 30 => 100,
            >= 31 and <= 40 => 100 - ((frame - 30) * 10),
            _ => 0,
        };
        if (opacity != 0)
        {
            string text = "";
            switch (page)
            {
                case 0:
                    text = UI.Engine.TranslateResourceString(s_coDevelopedByText);
                    break;
                case 1:
                    text = UI.Engine.TranslateResourceString(s_coPublishedByText);
                    break;
                case 2:
                    text = "";
                    break;
                default:
                    break;
            }

            if (opacity < 100)
            {
                info.ShowMode = ShowMode.Blended;
                info.BlendMode = BlendMode.Alpha;
                info.Blend = opacity;
                _creditsFont.SetText(x, y - (_creditsFont.Height / 2), text, lockedSurface: surface, mode: SetTextModes.Center, showInfo: info);
            }
            else
            {
                _creditsFont.SetTextCentered(x, y - (_creditsFont.Height / 2), text, lockedSurface: surface);
            }
        }

        if (frame is > 45 and < 130)
        {
            switch (page)
            {
                case 0:
                    if (TitleSceneImages[4] is not null)
                    {
                        LibraryImage? titleSceneImageLibraryHeight = TitleSceneImages[4];
                        LibraryImage? titleSceneImageLibraryWidth = TitleSceneImages[4];
                        TitleSceneImages[4].Show(x - 75 - (titleSceneImageLibraryWidth.Width / 2), y - (titleSceneImageLibraryHeight.Height / 2), lockedSurface: surface);
                    }

                    if (TitleSceneImages[5] is not null)
                    {
                        LibraryImage? titleSceneImageLibraryHeight = TitleSceneImages[5];
                        LibraryImage? titleSceneImageLibraryWidth = TitleSceneImages[5];
                        TitleSceneImages[5].Show(x + 75 - (titleSceneImageLibraryWidth.Width / 2), y - (titleSceneImageLibraryHeight.Height / 2), lockedSurface: surface);
                    }

                    break;
                case 1:
                    string northAmerica = UI.Engine.TranslateResourceString(s_northAmericaText);
                    LibraryImage? titleSceneImageLibrary = TitleSceneImages[6];
                    _creditsFont.SetTextCentered(x - 75, y - (titleSceneImageLibrary.Height / 2) - 20, text: northAmerica, lockedSurface: surface);
                    string europe = UI.Engine.TranslateResourceString(s_europeText);
                    LibraryImage? titleSceneImageLibrary2 = TitleSceneImages[6];
                    _creditsFont.SetTextCentered(x + 75 + 15, y - (titleSceneImageLibrary2.Height / 2) - 20, text: europe, lockedSurface: surface);
                    if (TitleSceneImages[6] is not null)
                    {
                        LibraryImage? titleSceneImageLibraryHeight = TitleSceneImages[6];
                        LibraryImage? titleSceneImageLibraryWidth = TitleSceneImages[6];
                        TitleSceneImages[6].Show(x - 75 - (titleSceneImageLibraryWidth.Width / 2), y - (titleSceneImageLibraryHeight.Height / 2) + 20, lockedSurface: surface);
                    }

                    if (TitleSceneImages[2] is not null)
                    {
                        LibraryImage? titleSceneImageLibraryHeight = TitleSceneImages[2];
                        LibraryImage? titleSceneImageLibraryWidth = TitleSceneImages[2];
                        TitleSceneImages[2].Show(x + 75 + 15 - (titleSceneImageLibraryWidth.Width / 2), y - (titleSceneImageLibraryHeight.Height / 2) + 20, lockedSurface: surface);
                    }

                    break;
                case 2:
                    opacity = frame switch
                    {
                        >= 45 and <= 55 => (frame - 45) * 10,
                        >= 121 and <= 130 => 100 - ((frame - 120) * 10),
                        _ => 101,
                    };
                    if (opacity <= 100)
                    {
                        info.ShowMode = ShowMode.Blended;
                        info.BlendMode = BlendMode.Alpha;
                        info.Blend = opacity;
                        _creditsFont.SetText(x, y - (_creditsFont.Height / 2), text: "Copyright (C) 1999,2000 Triumph Studios", lockedSurface: surface, mode: SetTextModes.Center, showInfo: info);
                    }
                    else
                    {
                        _creditsFont.SetTextCentered(x, y - (_creditsFont.Height / 2), text: "Copyright (C) 1999,2000 Triumph Studios", lockedSurface: surface);
                    }

                    break;
                default:
                    break;
            }
        }

        if (page == 3)
        {
            _showIntro = false;
        }
    }

    // AoWTitleScene.pas:212
    public void DrawScene(ref readonly LockedSurface surface)
    {
        Rect r;
        _sceneRectangle.Left = 0;
        _sceneRectangle.Right = surface.Width;
        _sceneRectangle.Top = (surface.Height / 2) - 240;
        _sceneRectangle.Bottom = _sceneRectangle.Top + 480;
        if (_redrawBackground)
        {
            r = _sceneRectangle;
            LibraryImage? titleSceneImageLibrary = TitleSceneImages[0];
            r.Top += titleSceneImageLibrary.Height;
            _ = PixelFormats.ColorFillSurfaceDescription(r, color: Color.Black, lockedSurface: surface);
        }

        int y = _sceneRectangle.Top;
        int frame = _frame - 25;
        if (frame < 0)
        {
            frame = 0;
        }

        frame = -((frame + 490) % TitleSceneImages[0].Width);
        TitleSceneImages[0].Show(frame, y, lockedSurface: surface);
        LibraryImage? titleSceneImageLibraryWidth = TitleSceneImages[0];
        TitleSceneImages[0].Show(frame + titleSceneImageLibraryWidth.Width, y, lockedSurface: surface);
        int x;
        if (!_showCredits && _frame > 25)
        {
            frame = _frame < 75 ? (_frame - 25) * 2 : _totalFrame <= 730 ? 100 : (750 - _totalFrame) * 5;

            LibraryImage? titleSceneImageLibrary = TitleSceneImages[1];
            x = (_sceneRectangle.Right - _sceneRectangle.Left - titleSceneImageLibrary.Width) / 2;
            if (frame < 100)
            {
                BlendTable lookupTable = GraphicsState.AlphaBlendTable.GetLookupTables(frame);
                TitleSceneImages[1].ShowBlended(x, y + 10, lockedSurface: surface, table: lookupTable);
            }
            else
            {
                TitleSceneImages[3].Show(x, y + 10, lockedSurface: surface);
            }
        }

        int spacing = 120;
        x = ((TitleSceneImages[10].Width + _sceneRectangle.Left + _sceneRectangle.Right) / 2) - 320;
        for (int i = 0; i <= 4; i++)
        {
            int image = i == _pressedButton && i == HitTest() ? 2 : _pressedButton == -1 && i == _hoverButton ? 1 : 0;

            TitleSceneImages[(i * 3) + 10 + image].Show(x + (spacing * i), y + 355, lockedSurface: surface);
            if (i == HitTest())
            {
                _font.Color = Color.White;
                string translatedText = UI.Engine.TranslateResourceString(TitleCaptions.TitleButtons[i]);
                LibraryImage? titleSceneImageLibrary = TitleSceneImages[10];
                _font.SetTextCentered(x + (spacing * i) + (titleSceneImageLibrary.Width / 2), y + 445, text: translatedText, lockedSurface: surface);
            }

            if (UI.Engine.AnimationDetail == 2)
            {
                TitleSceneImages[25].Show(x + (spacing * i), y + 355, lockedSurface: surface);
            }
        }

        if (_showCredits)
        {
            DrawCredits(in surface);
        }

        if (_showIntro)
        {
            DrawIntro(in surface);
        }
    }

    // AoWTitleScene.pas:261
    public void HideScene() => Window.Visible = false;

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
        if (_sceneRectangle.Bottom - 125 < _mouseY && _mouseY < _sceneRectangle.Bottom)
        {
            int spacing = 120;
            int x = ((TitleSceneImages[10].Width + _sceneRectangle.Left + _sceneRectangle.Right) / 2) - 320;
            result = 5;
            bool found = false;
            while (result != 0 && !found)
            {
                result -= 1;
                found = (spacing * result) + x < _mouseX && TitleSceneImages[10].Width + x + (spacing * result) > _mouseX;
            }

            if (!found)
            {
                return -1;
            }
        }
        else
        {
            return -1;
        }

        return result;
    }

    // AoWTitleScene.pas:315
    public void WindowDrawSurfaceEx(Surface? surface, out bool drawn)
    {
        try
        {
            LockedSurface lockedSurface = default;
            if (surface.Lock(ref lockedSurface).IsSuccess())
            {
                DrawScene(in lockedSurface);
                if (UI.Engine.RegisteredVersion() && !_showCredits)
                {
                    LegalNoticeMemo.Draw(lockedSurface);
                    DemoNoticeLabel.Draw(lockedSurface);
                    NameLabel.Draw(lockedSurface);
                }

                VersionLabel.Draw(lockedSurface);
                _ = surface.Unlock();
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
        ResetScene();
        string registeredTo = UI.Engine.RegisteredTo();
        NameLabel.Text = registeredTo;
        string versionString = UI.Engine.GetVersionString();
        string format = AbilityRules.Format(UI.Engine.TranslateResourceString(s_versionFormat), arguments: [versionString]);
        VersionLabel.Text = format;
        _credits = [];
        DelphiStringList lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_designerAndDirectorText));
        _ = lines.Add("Lennart Sas");
        _ = lines.Add("");
        _ = lines.Add(UI.Engine.TranslateResourceString(s_additionalDesignText));
        _ = lines.Add("Arno van Wingerden");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_leadProgrammerText));
        _ = lines.Add("Arno van Wingerden");
        _ = lines.Add("");
        _ = lines.Add(UI.Engine.TranslateResourceString(s_programmerText));
        _ = lines.Add("Josh Farley");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_artistsText));
        _ = lines.Add("Lennart Sas");
        _ = lines.Add("Thomas Cardin");
        _ = lines.Add("Roy Postma");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_leadScenarioDesignerText));
        _ = lines.Add("Menno Sas");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_scenarioDesignersText));
        _ = lines.Add("Doug Gibson");
        _ = lines.Add("Arnout Sas");
        _ = lines.Add("Hugo van Wingerden");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_writerText));
        _ = lines.Add("Raymond Bingham");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_musicText));
        _ = lines.Add("Michiel van den Bos");
        _ = lines.Add("");
        _ = lines.Add(UI.Engine.TranslateResourceString(s_soundEffectText));
        _ = lines.Add("Nando Eweg");
        _credits.Add(lines);
        _ = lines.Add("");
        _ = lines.Add(UI.Engine.TranslateResourceString(s_introVoiceText));
        _ = lines.Add("John McGinn (English)");
        _ = lines.Add("Norbert Gastell (German)");
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_galaxySoundSystemText));
        _ = lines.Add("Carlo Vogelsang");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_internetMailSuiteText));
        _ = lines.Add("Argo Software Design");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_qaCoreText));
        _ = lines.Add("Cliff Benjamin");
        _ = lines.Add("Ian Calderon");
        _ = lines.Add("Ted Stephenson");
        _ = lines.Add("Hugo van Wingerden");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString("Take 2 Europe"));
        _ = lines.Add("");
        _ = lines.Add("Producer");
        _ = lines.Add("Ken Jordan");
        _ = lines.Add("");
        _ = lines.Add("Manuals + Translations");
        _ = lines.Add("Frazer Nash");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add("Tarantula Studios testing suite");
        _ = lines.Add("");
        _ = lines.Add("Test Manager");
        _ = lines.Add("Mark Lloyd");
        _ = lines.Add("");
        _ = lines.Add("Lead Tester");
        _ = lines.Add("Paul Byers");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add("Testers");
        _ = lines.Add("");
        _ = lines.Add("Jim Thompson");
        _ = lines.Add("Kit Brown");
        _ = lines.Add("Julian Turner");
        _ = lines.Add("Jim Cree");
        _ = lines.Add("Lee Johnson");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_bizzText));
        _ = lines.Add("Jay Wilbur");
        _ = lines.Add("Mark Rein");
        _credits.Add(lines);
        lines = [];
        _ = lines.Add(UI.Engine.TranslateResourceString(s_specialThanksText));
        _ = lines.Add("Eelco van der Snoek");
        _ = lines.Add("Doug Gibson");
        _ = lines.Add("Dietmer Tan");
        _ = lines.Add("Arjan Brussee");
        _ = lines.Add("Daniel Cook");
        _credits.Add(lines);
    }

    // AoWTitleScene.pas:448
    public void TitleSceneDestroy()
    {
        for (int i = _credits.Count - 1; i >= 0; i--)
        {
            (_credits[i] as IDisposable)?.Dispose();
        }

        _credits?.Clear();
    }

    // AoWTitleScene.pas:456
    public void WindowUpdate()
    {
        Window.SurfaceValid = false;
        if (!UI.FileDialogWindow.Window.Visible && !UI.ProgressDialog.Window.Visible)
        {
            AdvanceFrame();
        }
    }

    // AoWTitleScene.pas:462
    public void WindowMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        _mouseX = X;
        _mouseY = Y;
        _pressedButton = HitTest();
    }

    // AoWTitleScene.pas:470
    public void WindowMouseMove(ShiftStates shift, int X, int Y)
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
        dialog.UI.TitleWindow.MainWindow.Visible = true;
        dialog.UI.FactionSelectionWindow.FactionPanel.Visible = true;
        dialog.TopLabel.Visible = true;
        string chooseFaction = dialog.UI.Engine.TranslateResourceString(LobbyText.ChooseFactionText);
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
            controller.InfoVisible = controller.UI.PlayerInfoWindow.MainWindow.Visible;
            controller.RealmVisible = controller.UI.RealmWindow.Window.Visible;
            controller.MagicVisible = controller.UI.MagicWindow.Window.Visible;
            controller.UnitVisible = controller.UI.ControlWindow.MainWindow.Visible;
            controller.TargetVisible = controller.UI.ChatWindow.MainWindow.Visible;
            controller.ScannerVisible = controller.UI.ScannerWindow.MainWindow.Visible;
            controller.ControlVisible = controller.UI.UnitWindow.MainWindow.Visible;
            controller.ChatVisible = controller.UI.InfoWindow.MainWindow.Visible;
            controller.MapVisible = controller.UI.MapWindow.MainWindow.Visible;
            for (int i = 0, last = controller.UI.General.Manager.Children.Count - 1; i <= last; i++)
            {
                if (controller.UI.General.Manager.Children[i] != controller.UI.TaskBar.TaskWindow)
                {
                    controller.UI.General.Manager.Children[i].Visible = false;
                }
            }
        }
    }

    private static void LoadGame(General controller, string fileName)
    {
        if (StrategicMap.Current is not null)
        {
            controller.UI.Engine.CloseHsm(null);
        }

        string loading = controller.UI.Engine.TranslateResourceString(s_loadingText);
        string fileNameLocal = ExtractFileName(fileName);
        controller.UI.ProgressDialog.TitleLabel.Text = $"{loading}: {fileNameLocal}...";
        controller.UI.ProgressDialog.StatusLabel.Text = "";
        controller.UI.ProgressDialog.ProgressBar.Position = 0;
        controller.UI.ProgressDialog.ProgressBar.Max = 100;
        controller.UI.ProgressDialog.Window.Visible = true;
        if (!controller.UI.TitleScene.Window.Visible)
        {
            controller.Manager.OverlayPriority = 5_000;
        }

        _ = AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        _ = (AudioHost.SoundSystem?.FadeMusicVolume(0, speed: 1));
        controller.UI.Engine.LoadHsm(fileName, edit: false, progress: (_, message, progression) => controller.MapProgress(message, progression), profile: GameProfile.WorldMap);
        if (controller.UI.Engine.Demo && StrategicMap.Current is not null && !StrategicMap.Current.ShowAllPlayers)
        {
            CloseGame(controller);
        }

        controller.Manager.OverlayPriority = 0;
        controller.UI.ProgressDialog.Window.Visible = false;
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
                for (int i = 0, last = StrategicMap.Current.StartupSongs.Songs.Count - 1; i <= last; i++)
                {
                    if (CompareText(b: controller.UI.Engine.TranslateResourceString(s_fileNotFoundText), a: StrategicMap.Current.StartupSongs.SongNames[i]) != 0)
                    {
                        string song = StrategicMap.Current.StartupSongs.Songs[i];
                        _ = AudioHost.SongPlayer.Songs.Add(song);
                    }
                }
            }
            else
            {
                _ = AudioHost.SongPlayer.Songs.Add("Songs\\Heartwoo.it");
            }
        }

        _ = AudioHost.SongPlayer.Play(0);
        AudioPlayback.SetMusicVolume(controller.UI.SoundDialog.MusicVolumeSlider.Position);
        controller.UI.TaskBar.WorldStartMenuBar.Items.Clear();
        string background = controller.UI.Engine.TranslateResourceString(s_backgroundText);
        _ = controller.UI.TaskBar.WorldStartMenuBar.Items.Add(background);
        if (controller.UI.Engine.Campaign is not null)
        {
            string campaign = controller.UI.Engine.TranslateResourceString(s_campaignText);
            _ = controller.UI.TaskBar.WorldStartMenuBar.Items.Add(campaign);
        }

        controller.UI.TitleScene.Window.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TaskBar.TaskWindow.Visible = true;
        controller.UI.MapWindow.MainWindow.Visible = true;
        controller.UI.ScannerWindow.MainWindow.Visible = true;
        controller.UI.MapWindow.MapViewer.SetHexSpriteMap(StrategicMap.Current);
        controller.UI.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        MapLevel mapLevel = StrategicMap.Current.MapContainer.MapLevels[0];
        controller.UI.ScannerWindow.Scanner.SetLevel(mapLevel);
        controller.UI.TaskBar.WorldTurnButton.ButtonChecked = false;
        controller.UI.TaskBar.ChatButton.Visible = controller.UI.SetupEvents.DirectPlay.Connected;
        controller.Manager.Aligned = false;
        controller.UI.ScannerWindow.Scanner.PreviousWidth = 0;
        controller.UI.ScannerWindow.Scanner.Incremental = false;
        controller.UI.ScannerWindow.Scanner.Sized = false;
        controller.UI.ScannerWindow.ScannerTabs.Items.Clear();
        string surface = controller.UI.Engine.TranslateResourceString(s_surfaceText);
        _ = controller.UI.ScannerWindow.ScannerTabs.Items.Add(surface);
        if (StrategicMap.Current.MapContainer.LevelCount > 1)
        {
            string caverns = controller.UI.Engine.TranslateResourceString(s_cavernsText);
            _ = controller.UI.ScannerWindow.ScannerTabs.Items.Add(caverns);
        }

        if (StrategicMap.Current.MapContainer.LevelCount > 2)
        {
            string depths = controller.UI.Engine.TranslateResourceString(s_depthsText);
            _ = controller.UI.ScannerWindow.ScannerTabs.Items.Add(depths);
        }

        controller.UI.ScannerWindow.ScannerTabs.Selected = 0;
        controller.UI.ScannerWindow.ScannerTabs.Sized = false;
        controller.UI.ScannerWindow.Scanner.SetTWidth(0);
        controller.UI.ScannerWindow.Scanner.SetTHeight(0);
        controller.UI.ScannerWindow.Scanner.SetTLeft(-1);
        controller.UI.ScannerWindow.Scanner.SetTTop(-1);
        controller.UI.ScannerWindow.Scanner.UpdateStep = -1;
        controller.UI.ScannerWindow.Scanner.SetMap(StrategicMap.Current);
        MapLevel mapLevel2 = StrategicMap.Current.MapContainer.MapLevels[0];
        controller.UI.ScannerWindow.Scanner.SetLevel(mapLevel2);
        controller.UI.MapWindow.MapViewer.SetHexSpriteMap(StrategicMap.Current);
        controller.UI.MapWindow.MapViewer.SceneWidth = controller.UI.MapWindow.MainWindow.WinWidth - 24;
        controller.UI.MapWindow.MapViewer.SceneHeight = controller.UI.MapWindow.MainWindow.WinHeight - 24;
        StrategicMap.Current.Activate();
        controller.UI.ControlWindow.MainWindow.Visible = false;
        controller.UI.MapEvents.TheMapArmyDeselected();
        controller.UI.MapEvents.TheMapStructureDeselected();
        controller.UI.TaskBar.TBWorldUpdate();
        controller.UI.PlayerInfoWindow.UpdatePlayer();
        if (StrategicMap.Current.Status.Contains(HexSpriteMapStatuses.Activated))
        {
            TurnButton turnButton = controller.UI.TaskBar.WorldTurnButton;
            if (StrategicMap.Current.SeatedPlayer is null)
            {
                turnButton.CurrentPlayer = 12;
                turnButton.LastPlayer = 12;
                turnButton.AnimationFrame = turnButton.CurrentPlayer * 7;
                controller.UI.TaskBar.AutoCombatCheckBox.Visible = false;
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
                controller.UI.TaskBar.AutoCombatCheckBox.Visible = StrategicMap.Current.PlayMode == PlayMode.Simultaneous;
            }
        }

        controller.UI.TaskBar.WorldClockDelay = 0;
        controller.UI.TaskBar.ChatButton.TextColor = new Color(14, 0, 0);
        controller.UI.TaskBar.ChatFlash = 0;
        controller.UI.VideoDialog.ChangeResolution(controller.UI.VideoDialog.DisplayWidth, controller.UI.VideoDialog.DisplayHeight);
    }

    private static void CloseGame(General controller)
    {
        controller.UI.Engine.CloseHsm(null);
        controller.UI.Engine.CloseCampaign();
        controller.UI.ScannerWindow.Scanner.SetMap(null);
        controller.UI.CombatScanner.Scanner.SetMap(null);
        _ = AudioHost.SongPlayer.Stop(false);
        AudioHost.SongPlayer.Songs.Clear();
        _ = RtlHost.Files.SetCurrentDirectory(controller.UI.Engine.StartupDirectory);
        HideGameWindows(controller.UI.General);
        controller.UI.TaskBar.TaskWindow.Visible = false;
        controller.UI.TitleWindow.MainWindow.Visible = false;
        controller.UI.TitleScene.ResetScene();
        controller.UI.TitleScene.ShowScene(true);
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
                int count = AudioHost.SongPlayer.Songs.Count;
                condition = AudioHost.SongPlayer.CurrentIndex >= count;
            }

            if (condition || AudioHost.SongPlayer.Songs[AudioHost.SongPlayer.CurrentIndex] != "SONGS\\TITLE.IT")
            {
                AudioHost.SongPlayer.Songs.Clear();
                _ = AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
                _ = AudioHost.SongPlayer.Play(0);
            }
        }

        scene.UI.General.Manager.OverlayPriority = 0;
        scene.TitleSceneImages.LoadDiscardableImageData();
        scene.UI.VideoDialog.SetSceneSize(640, H: 480);
        VideoDialog.SetVideoMode(scene.UI.VideoDialog, fullScreen: VideoDialog.ScreenMode.Title);
        scene.UI.VideoDialog.ApplyMode();
        GameHost.MainForm.Display.FrameRate = 30;
        GameHost.MainForm.DisplayController.UseVerticalBlank = true;
        scene._redrawBackground = true;
        scene._pressedButton = -1;
        scene._hoverButton = -1;
        scene._font = scene.UI.Fonts.Age15WhiteOnGrey;
        scene._creditsFont = scene.UI.Fonts.Age15WhiteOnGrey;
        scene.Window.WindowManager.SetImageLibraryIndex(4);
        scene.Window.Visible = true;
    }
}
