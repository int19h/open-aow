#nullable disable warnings
// Translated from AoWVideoDlg.pas.
using System.Globalization;
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.DelphiTextCompare;
using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// full screen, change flag, display size
public sealed partial class VideoDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public ListBox DisplayModesList { get; private set; }
    public CheckBox ResolutionCheckBox { get; private set; }

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
        DisplayModesList.Items.Clear();
        string desktopWindow = UI.Engine.TranslateResourceString(s_desktopWindowText);
        _ = DisplayModesList.Items.AddObject(desktopWindow, item: IntToObject(-1));
        for (int i = 0, last = _displayModes.Count - 1; i <= last; i++)
        {
            DisplayModeItem mode = _displayModes.Items[i];
            if (mode.Width >= 640 && mode.Height >= 480)
            {
                string fullScreen = UI.Engine.TranslateResourceString(s_fullScreenText);
                _ = DisplayModesList.Items.AddObject($"{fullScreen}:{mode.Width.ToString(CultureInfo.InvariantCulture)}x{mode.Height.ToString(CultureInfo.InvariantCulture)}", item: IntToObject(i));
            }
        }

        int currentMode = CurrentMode();
        DisplayModesList.Selected = currentMode;
        if (DisplayModesList.Selected != -1)
        {
            string s = DisplayModesList.Items[DisplayModesList.Selected];
            s = $"{s} ({LowerCase(UI.Engine.TranslateResourceString(s_currentText))})";
            DisplayModesList.Items[DisplayModesList.Selected] = s;
        }
    }

    // AoWVideoDlg.pas:195
    public void VideoDialogCreate()
    {
        _displayModes = new DisplayModeList();
        ListDisplayModes(_displayModes, 0, 0, pixelFormatId: 16);
    }

    // AoWVideoDlg.pas:201
    public void VideoDialogDestroy() => _displayModes?.Dispose();

    // AoWVideoDlg.pas:206
    public void CloseButClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWVideoDlg.pas:212
    public void ApplyBtnClick()
    {
        UI.General.PlayButtonSound(0);
        if (DisplayModesList.Selected != -1)
        {
            int i = ObjectToInt(DisplayModesList.Items.Objects[DisplayModesList.Selected]);
            if (i != -1)
            {
                DisplayWidth = _displayModes.Items[i].Width;
                DisplayHeight = _displayModes.Items[i].Height;
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
            UI.General.PlayButtonSound(1);
        }
        else
        {
            UI.General.PlayButtonSound(2);
        }

        UI.Engine.Registry.SetAllowResolutionSwitches(ResolutionCheckBox.Checked);
    }

    // AoWVideoDlg.pas:239
    public void DlgKeyDown(in int key, ShiftStates shift)
    {
        UI.General.Manager.Aligned = false;
        if (key == 27)
        {
            Window.Visible = false;
        }
    }

    // AoWVideoDlg.pas:245
    public void DlgShow()
    {
        bool allowResolutionSwitches = UI.Engine.Registry.GetAllowResolutionSwitches();
        UI.VideoDialog.ResolutionCheckBox.Checked = allowResolutionSwitches;
    }

    private static readonly string s_currentText = "Current";

    private static readonly string s_desktopWindowText = "Desktop Window";

    private static readonly string s_fullScreenText = "Full Screen";

    internal static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1_280, 1_024);

    internal static Func<IReadOnlyList<(int Width, int Height)>> FullscreenSizes { get; set; } = () => null;

    private static readonly (int Width, int Height)[] s_fixedSizes = [(640, 480), (800, 600), (1_024, 768), (1_280, 1_024)];

    private static void ListDisplayModes(DisplayModeList list, int width, int height, int pixelFormatId)
    {
        (int desktopWidth, int desktopHeight) = DesktopSize();
        IReadOnlyList<(int Width, int Height)> sizes = FullscreenSizes() ?? [.. s_fixedSizes.Where(size => size.Width <= desktopWidth && size.Height <= desktopHeight)];
        foreach ((int sizeWidth, int sizeHeight) in sizes)
        {
            if ((width == 0 || width == sizeWidth) && (height == 0 || height == sizeHeight) &&
                (pixelFormatId is 0 or 16 or (int)PixelFormat.Rgb555))
            {
                list.Add(sizeWidth, sizeHeight, pixelFormatId: PixelFormat.Rgb555);
            }
        }
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
