// Translated from AOWTitleWindow.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// splash timer, splash stage
public partial class TitleWindow : UIModule
{
    public TitleWindow(UIContext ui) : base(ui)
    { }

    public Window MainWindow;
    public Panel TitlePanel;
    public Panel SplashPanel;
    public InterfaceImageLibrary DemoImages;
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
        UI.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(UI.VideoDialog, VideoDialog.ScreenMode.Title);
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
    {
        /* TODO: Implement Forms.Close in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AOWTitleWindow.pas:87
    public void ShowSplash()
    {
        InitializeSplash();
        AudioHost.SongPlayer.Songs.Clear();
        AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
        AudioHost.SongPlayer.Play(0);
        SplashPanel.ImageLibraryIndexTexture = 0;
        _splashTimer = 450;
    }

    // AOWTitleWindow.pas:42
    public void TitleWinDrawSurface(Surface surface, int X, int Y)
    {
        LockedSurface lockedSurface = default;
        if (UI.FactionSelectionWindow.FactionPanel.Visible && UI.FactionSelectionWindow.Fade >= 0)
        {
            surface.Lock(ref lockedSurface);
            if (UI.FactionSelectionWindow.FadeDirection == 0)
            {
                UI.Graphics.FactionWindow[UI.FactionSelectionWindow.FadeImage].ShowBlended(UI.FactionSelectionWindow.FactionPanel.WinLeft, UI.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[100 - UI.FactionSelectionWindow.Fade]);
            }
            else
            {
                UI.Graphics.FactionWindow[UI.FactionSelectionWindow.FadeImage].ShowBlended(UI.FactionSelectionWindow.FactionPanel.WinLeft, UI.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[UI.FactionSelectionWindow.Fade]);
            }

            surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(LockedSurface lockedSurface)
    {
        UI.Graphics.FactionWindow[0].Show(UI.FactionSelectionWindow.FactionPanel.WinLeft, UI.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface);
    }

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
    public void SplashPnlKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            HideSplash();
            WindowActions.ShowTitleScene(UI.TitleScene, false);
        }
        else
        {
            if (!shift.Contains(ShiftState.Alt))
            {
                NextSplash();
            }
        }
    }

    // AOWTitleWindow.pas:131
    public void SplashPnlMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        NextSplash();
    }

    internal static void ShowExitSplash(TitleWindow dialog)
    {
        ShowSplash(dialog);
        dialog.SplashPanel.ImageLibraryIndexTexture = 2;
        dialog._splashTimer = 300;
    }

    internal static void ShowSplash(TitleWindow dialog)
    {
        VideoDialog.SetVideoSize(dialog.UI.VideoDialog, 640, 480);
        VideoDialog.SetVideoMode(dialog.UI.VideoDialog, VideoDialog.ScreenMode.Title);
        dialog.UI.VideoDialog.ApplyBtnClick();
        dialog.MainWindow.Visible = true;
        dialog.TitlePanel.Visible = false;
        dialog.SplashPanel.ImageLibraryTexture = dialog.DemoImages;
        dialog.SplashPanel.ImageLibraryIndexTexture = 0;
        dialog.SplashPanel.Visible = true;
        dialog._splashTimer = 450;
    }

}
