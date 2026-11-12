// Translated from AoWVideoDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// full screen, change flag, display size
public partial class VideoDialog : DataModule
{
    public VideoDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public ListBox DisplayModesList;
    public CheckBox ResolutionCheckBox;
    private DisplayModeList _displayModes;
    private ScreenMode _fullScreen;

    public bool ModeChanged { get; set; }

    private int _sceneWidth;
    private int _sceneHeight;

    public int DisplayWidth { get; private set; }
    public int DisplayHeight { get; private set; }

    // AoWVideoDlg.pas:366
    public void SetSceneSize(int W, int H)
    {
        _sceneWidth = W;
        _sceneHeight = H;
        if (_fullScreen == ScreenMode.Title)
        {
            ModeChanged = true;
        }
    }

    // AoWVideoDlg.pas:416
    public void ShowModes()
    {
        Window.Visible = true;
        UpdateModes();
    }

    // AoWVideoDlg.pas:172
    public void UpdateModes()
    {
        DisplayModeItem mode;
        string S;
        DisplayModesList.Items.Clear();
        var desktopWindow = AbilityRules.TranslateResourceString(s_desktopWindowText);
        DisplayModesList.Items.AddObject(desktopWindow, IntToObject(-1));
        for (int I = 0, last = _displayModes.Count - 1; I <= last; I++)
        {
            mode = _displayModes.Items[I];
            if (mode.Width >= 640 && mode.Height >= 480)
            {
                var fullScreen = AbilityRules.TranslateResourceString(s_fullScreenText);
                DisplayModesList.Items.AddObject(fullScreen + ":" + IntToStr(mode.Width) + "x" + IntToStr(mode.Height), IntToObject(I));
            }
        }

        var currentMode = CurrentMode();
        DisplayModesList.Selected = currentMode;
        if (DisplayModesList.Selected != -1)
        {
            S = DisplayModesList.Items[DisplayModesList.Selected];
            S = S + " (" + LowerCase(AbilityRules.TranslateResourceString(s_currentText)) + ")";
            DisplayModesList.Items[DisplayModesList.Selected] = S;
        }
    }

    // AoWVideoDlg.pas:195
    public void VideoDialogCreate()
    {
        _displayModes = new DisplayModeList();
        ListDisplayModes(_displayModes, 0, 0, 16);
    }

    // AoWVideoDlg.pas:201
    public void VideoDialogDestroy()
    {
        _displayModes?.Dispose();
    }

    // AoWVideoDlg.pas:206
    public void CloseButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWVideoDlg.pas:212
    public void ApplyBtnClick()
    {
        int I;
        DialogHosts.General.PlayButtonSound(0);
        if (DisplayModesList.Selected != -1)
        {
            I = ObjectToInt(DisplayModesList.Items.Objects[DisplayModesList.Selected]);
            if (I != -1)
            {
                DisplayWidth = _displayModes.Items[I].Width;
                DisplayHeight = _displayModes.Items[I].Height;
                ChangeResolution(DisplayWidth, DisplayHeight);
                _fullScreen = ScreenMode.Game;
            }
            else
            {
                _fullScreen = ScreenMode.Windowed;
                ChangeResolution(0, 0);
            }
        }
    }

    // AoWVideoDlg.pas:233
    public void ResCBChange(CheckBox sender)
    {
        if (sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }

        GameTables.GameEngine.Registry.SetAllowResolutionSwitches(ResolutionCheckBox.Checked);
    }

    // AoWVideoDlg.pas:239
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        DialogHosts.General.Manager.Aligned = false;
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    // AoWVideoDlg.pas:245
    public void DlgShow()
    {
        var allowResolutionSwitches = GameTables.GameEngine.Registry.GetAllowResolutionSwitches();
        DialogHosts.VideoDialog.ResolutionCheckBox.Checked = allowResolutionSwitches;
    }

    private static readonly string s_currentText = "Current";

    private static readonly string s_desktopWindowText = "Desktop Window";

    private static readonly string s_fullScreenText = "Full Screen";

    internal static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1280, 1024);

    internal static Func<IReadOnlyList<(int Width, int Height)>> FullscreenSizes { get; set; } = () => null;

    private static readonly (int Width, int Height)[] s_fixedSizes = [(640, 480), (800, 600), (1024, 768), (1280, 1024)];

    private static void ListDisplayModes(DisplayModeList list, int width, int height, int pixelFormatId)
    {
        var desktop = DesktopSize();
        var sizes = FullscreenSizes() ?? [.. s_fixedSizes.Where(size => size.Width <= desktop.Width && size.Height <= desktop.Height)];
        foreach (var size in sizes)
            if ((width == 0 || width == size.Width) && (height == 0 || height == size.Height) &&
                (pixelFormatId == 0 || pixelFormatId == 16 || pixelFormatId == PixelFormats.pfRGB555))
                list.Add(size.Width, size.Height, PixelFormats.pfRGB555);
    }

    internal static void SetSceneSize(VideoDialog dialog, int width, int height)
    {
        dialog._sceneWidth = width;
        dialog._sceneHeight = height;
        if (dialog._fullScreen == ScreenMode.Title)
        {
            dialog.ModeChanged = true;
        }
    }

    internal static void ApplyVideoSettings(VideoDialog dialog) => dialog.ApplyMode();

    internal static void SetVideoSize(VideoDialog dialog, int width, int height)
    {
        dialog.DisplayWidth = width;
        dialog.DisplayHeight = height;
        if (dialog._fullScreen == ScreenMode.Game)
        {
            dialog.ModeChanged = true;
        }
    }

    internal static void ShowVideoSettings(VideoDialog dialog)
    {
        dialog.Window.Visible = true;
        dialog.UpdateModes();
    }

    internal static void SetVideoMode(VideoDialog dialog, ScreenMode fullScreen)
    {
        if (fullScreen != dialog._fullScreen)
        {
            dialog._fullScreen = fullScreen;
            dialog.ModeChanged = true;
        }
    }

    public enum ScreenMode : byte
    {
        Windowed = 0, Game = 1, Title = 2
    }

}
