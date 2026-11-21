#nullable disable
// Translated from AOWTitleWindow.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// splash timer, splash stage
public sealed partial class TitleWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public Panel TitlePanel { get; private set; }
    public Panel SplashPanel { get; private set; }
    public InterfaceImageLibrary DemoImages { get; private set; }

    private int _splashTimer;

    // AOWTitleWindow.pas:66
    public void HideSplash()
    {
        DemoImages.ReleaseDiscardableImageData();
        SplashPanel.Visible = false;
        MainWindow.Visible = false;
    }

    // AOWTitleWindow.pas:73
    public void InitializeSplash()
    {
        UI.VideoDialog.SetSceneSize(640, H: 480);
        VideoDialog.SetVideoMode(UI.VideoDialog, fullScreen: VideoDialog.ScreenMode.Title);
        UI.VideoDialog.ApplyMode();
        MainWindow.Visible = true;
        TitlePanel.Visible = false;
        SplashPanel.ImageLibraryTexture = DemoImages;
        SplashPanel.ImageLibraryIndexTexture = 0;
        SplashPanel.Visible = true;
        _splashTimer = 450;
    }

    // AOWTitleWindow.pas:97
    public void ShowExitSplash()
    {
        InitializeSplash();
        SplashPanel.ImageLibraryIndexTexture = 2;
        _splashTimer = 300;
    }

    // AOWTitleWindow.pas:105
    public void NextSplash()
        /* TODO: Implement Forms.Close in the platform or editor service. */
        => throw new NotImplementedException();

    // AOWTitleWindow.pas:87
    public void ShowSplash()
    {
        InitializeSplash();
        AudioHost.SongPlayer.Songs.Clear();
        _ = AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
        _ = AudioHost.SongPlayer.Play(0);
        SplashPanel.ImageLibraryIndexTexture = 0;
        _splashTimer = 450;
    }

    // AOWTitleWindow.pas:42
    public void TitleWinDrawSurface(Surface surface, int X, int Y)
    {
        if (UI.FactionSelectionWindow.FactionPanel.Visible && UI.FactionSelectionWindow.Fade >= 0)
        {
            LockedSurface lockedSurface = default;
            _ = surface.Lock(ref lockedSurface);
            if (UI.FactionSelectionWindow.FadeDirection == 0)
            {
                UI.Graphics.FactionWindow[UI.FactionSelectionWindow.FadeImage].ShowBlended(UI.FactionSelectionWindow.FactionPanel.AbsoluteLeft, UI.FactionSelectionWindow.FactionPanel.AbsoluteTop, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[100 - UI.FactionSelectionWindow.Fade]);
            }
            else
            {
                UI.Graphics.FactionWindow[UI.FactionSelectionWindow.FadeImage].ShowBlended(UI.FactionSelectionWindow.FactionPanel.AbsoluteLeft, UI.FactionSelectionWindow.FactionPanel.AbsoluteTop, lockedSurface, table: GraphicsState.AlphaBlendTable.LookupTables[UI.FactionSelectionWindow.Fade]);
            }

            _ = surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(LockedSurface lockedSurface) => UI.Graphics.FactionWindow[0].Show(UI.FactionSelectionWindow.FactionPanel.AbsoluteLeft, UI.FactionSelectionWindow.FactionPanel.AbsoluteTop, lockedSurface);

    // AOWTitleWindow.pas:115
    public void SplashPnlUpdate()
    {
        _splashTimer -= 1;
        if (_splashTimer == 0)
        {
            NextSplash();
        }
    }

    // AOWTitleWindow.pas:121
    public void SplashPnlKeyDown(in ushort key, ShiftStates shift)
    {
        if (key == 27)
        {
            HideSplash();
            UI.TitleScene.ShowScene(false);
        }
        else
        {
            if (!shift.Contains(ShiftStates.Alt))
            {
                NextSplash();
            }
        }
    }

    // AOWTitleWindow.pas:131
    public void SplashPnlMouseDown(MouseButton button, ShiftStates shift, int X, int Y) => NextSplash();

    internal static void ShowExitSplash(TitleWindow dialog)
    {
        ShowSplash(dialog);
        dialog.SplashPanel.ImageLibraryIndexTexture = 2;
        dialog._splashTimer = 300;
    }

    internal static void ShowSplash(TitleWindow dialog)
    {
        VideoDialog.SetVideoSize(dialog.UI.VideoDialog, 640, 480);
        VideoDialog.SetVideoMode(dialog.UI.VideoDialog, fullScreen: VideoDialog.ScreenMode.Title);
        dialog.UI.VideoDialog.ApplyBtnClick();
        dialog.MainWindow.Visible = true;
        dialog.TitlePanel.Visible = false;
        dialog.SplashPanel.ImageLibraryTexture = dialog.DemoImages;
        dialog.SplashPanel.ImageLibraryIndexTexture = 0;
        dialog.SplashPanel.Visible = true;
        dialog._splashTimer = 450;
    }
}
