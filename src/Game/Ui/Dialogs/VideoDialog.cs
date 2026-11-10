// Translated from AoWVideoDlg.pas.
using Aow.Game.Draw;

using Aow.Game.Magic.Abilities;

using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// full screen, change flag, display size
public partial class VideoDialog : DataModule
{
    public VideoDialog(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWButton CloseBut;
    public AoWFrame BCeltL;
    public AOWLabel Title;
    public AOWButton Close;
    public AOWImage TCeltLR;
    public AoWFrame TopLine;
    public AOWLabel VideoLbl;
    public AOWImage TCeltPattern;
    public AOWImage TitleIcon;
    public AOWImage TCeltLL;
    public AOWListBox DisplayModesLB;
    public AOWCheckBox ResCB;
    public AOWLabel ResSwLbl;
    public AOWButton ApplyBtn;
    public AOWPanel T3LBPnl;
    public AOWVScrollBar VideoSB;
    public IvTranslator Translator;
    public DisplayModeList FDisplayModes;
    public ScreenMode FFullScreen;
    public bool FModeChanged;
    public int FSceneWidth;
    public int FSceneHeight;
    public int FDisplayWidth;
    public int FDisplayHeight;

    // AoWVideoDlg.pas:366
    public void SetSceneSize(int W, int H)
    {
        FSceneWidth = W;
        FSceneHeight = H;
        if (FFullScreen == ScreenMode.Title)
        {
            FModeChanged = true;
        }
    }

    // AoWVideoDlg.pas:416
    public void ShowModes()
    {
        Dlg.Visible = true;
        UpdateModes();
    }

    // AoWVideoDlg.pas:172
    public void UpdateModes()
    {
        DisplayModeItem Mode;
        string S;
        DisplayModesLB.Items.Clear();
        var desktopWindow = AbilityRules.TranslateRStr(DesktopWindowRStr);
        DisplayModesLB.Items.AddObject(desktopWindow, IntToObject(-1));
        for (int I = 0, last = FDisplayModes.Count - 1; I <= last; I++)
        {
            Mode = FDisplayModes.Items[I];
            if (Mode.Width >= 640 && Mode.Height >= 480)
            {
                var fullScreen = AbilityRules.TranslateRStr(FullScreenRStr);
                DisplayModesLB.Items.AddObject(fullScreen + ":" + IntToStr(Mode.Width) + "x" + IntToStr(Mode.Height), IntToObject(I));
            }
        }

        var currentMode = CurrentMode();
        DisplayModesLB.Selected = currentMode;
        if (DisplayModesLB.Selected != -1)
        {
            S = DisplayModesLB.Items[DisplayModesLB.Selected];
            S = S + " (" + LowerCase(AbilityRules.TranslateRStr(CurrentRStr)) + ")";
            DisplayModesLB.Items[DisplayModesLB.Selected] = S;
        }
    }

    // AoWVideoDlg.pas:195
    public void VideoDlgWindowCreate()
    {
        FDisplayModes = new DisplayModeList();
        ListDisplayModes(FDisplayModes, 0, 0, 16);
    }

    // AoWVideoDlg.pas:201
    public void VideoDlgWindowDestroy()
    {
        FDisplayModes?.Dispose();
    }

    // AoWVideoDlg.pas:206
    public void CloseButClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        Dlg.Visible = false;
    }

    // AoWVideoDlg.pas:212
    public void ApplyBtnClick()
    {
        int I;
        DialogHosts.General.PlayButtonSound(0);
        if (DisplayModesLB.Selected != -1)
        {
            I = ObjectToInt(DisplayModesLB.Items.Objects[DisplayModesLB.Selected]);
            if (I != -1)
            {
                FDisplayWidth = FDisplayModes.Items[I].Width;
                FDisplayHeight = FDisplayModes.Items[I].Height;
                ChangeResolution(FDisplayWidth, FDisplayHeight);
                FFullScreen = ScreenMode.Game;
            }
            else
            {
                FFullScreen = ScreenMode.Windowed;
                ChangeResolution(0, 0);
            }
        }
    }

    // AoWVideoDlg.pas:233
    public void ResCBChange(AOWCheckBox Sender)
    {
        if (Sender.Checked)
        {
            DialogHosts.General.PlayButtonSound(1);
        }
        else
        {
            DialogHosts.General.PlayButtonSound(2);
        }

        GameTables.AoWEngine.FRegistry.SetAllowResolutionSwitches(ResCB.Checked);
    }

    // AoWVideoDlg.pas:239
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        DialogHosts.General.Manager.FAligned = false;
        if (Key == 27)
        {
            Dlg.Visible = false;
        }
    }

    // AoWVideoDlg.pas:245
    public void DlgShow()
    {
        var allowResolutionSwitches = GameTables.AoWEngine.FRegistry.GetAllowResolutionSwitches();
        DialogHosts.VideoDialog.ResCB.Checked = allowResolutionSwitches;
    }

    private static readonly string CurrentRStr = "Current";

    private static readonly string DesktopWindowRStr = "Desktop Window";

    private static readonly string FullScreenRStr = "Full Screen";

    internal static Func<(int Width, int Height)> DesktopSize { get; set; } = () => (1280, 1024);

    internal static Func<IReadOnlyList<(int Width, int Height)>> FullscreenSizes { get; set; } = () => null;

    private static readonly (int Width, int Height)[] FixedSizes = [(640, 480), (800, 600), (1024, 768), (1280, 1024)];

    private static void ListDisplayModes(DisplayModeList list, int width, int height, int pixelFormatID)
    {
        var desktop = DesktopSize();
        var sizes = FullscreenSizes() ?? [.. FixedSizes.Where(size => size.Width <= desktop.Width && size.Height <= desktop.Height)];
        foreach (var size in sizes)
            if ((width == 0 || width == size.Width) && (height == 0 || height == size.Height) &&
                (pixelFormatID == 0 || pixelFormatID == 16 || pixelFormatID == PixelFormats.pfRGB555))
                list.Add(size.Width, size.Height, PixelFormats.pfRGB555);
    }

    internal static void SetSceneSize(VideoDialog Dialog, int Width, int Height)
    {
        Dialog.FSceneWidth = Width;
        Dialog.FSceneHeight = Height;
        if (Dialog.FFullScreen == ScreenMode.Title)
        {
            Dialog.FModeChanged = true;
        }
    }

    internal static void ApplyVideoSettings(VideoDialog dialog) => dialog.ApplyMode();

    internal static void SetVideoSize(VideoDialog Dialog, int Width, int Height)
    {
        Dialog.FDisplayWidth = Width;
        Dialog.FDisplayHeight = Height;
        if (Dialog.FFullScreen == ScreenMode.Game)
        {
            Dialog.FModeChanged = true;
        }
    }

    internal static void ShowVideoSettings(VideoDialog Dialog)
    {
        Dialog.Dlg.Visible = true;
        Dialog.UpdateModes();
    }

    internal static void SetVideoMode(VideoDialog Dialog, ScreenMode FullScreen)
    {
        if (FullScreen != Dialog.FFullScreen)
        {
            Dialog.FFullScreen = FullScreen;
            Dialog.FModeChanged = true;
        }
    }

    public enum ScreenMode : byte
    {
        Windowed = 0, Game = 1, Title = 2
    }

}
