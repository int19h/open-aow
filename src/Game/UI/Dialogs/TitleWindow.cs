// Translated from AOWTitleWindow.pas.
using Aow.Game.Audio;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// splash timer, splash stage
public partial class TitleWindow : DataModule
{
    public TitleWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public Panel TitlePanel;
    public Panel SplashPanel;
    public InterfaceImageLibrary DemoImages;
    private int _splashTimer;

    public int SplashStage { get; private set; }

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
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        MainWindow.Visible = true;
        TitlePanel.Visible = false;
        SplashPanel.ImageLibraryTexture = DemoImages;
        SplashPanel.ImageLibraryIndexTexture = 0;
        SplashPanel.Visible = true;
        _splashTimer = 450;
        SplashStage = 0;
    }

    // AOWTitleWindow.pas:97
    public void ShowExitSplash()
    {
        InitializeSplash();
        SplashPanel.ImageLibraryIndexTexture = 2;
        SplashStage = 10;
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
        if (DialogHosts.FactionSelectionWindow.FactionPanel.Visible && DialogHosts.FactionSelectionWindow.Fade >= 0)
        {
            surface.Lock(ref lockedSurface);
            if (DialogHosts.FactionSelectionWindow.FadeDirection == 0)
            {
                DialogHosts.UIGraphics.FactionWindow[DialogHosts.FactionSelectionWindow.FadeImage].ShowBlended(DialogHosts.FactionSelectionWindow.FactionPanel.WinLeft, DialogHosts.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[100 - DialogHosts.FactionSelectionWindow.Fade]);
            }
            else
            {
                DialogHosts.UIGraphics.FactionWindow[DialogHosts.FactionSelectionWindow.FadeImage].ShowBlended(DialogHosts.FactionSelectionWindow.FactionPanel.WinLeft, DialogHosts.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[DialogHosts.FactionSelectionWindow.Fade]);
            }

            surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(LockedSurface lockedSurface)
    {
        DialogHosts.UIGraphics.FactionWindow[0].Show(DialogHosts.FactionSelectionWindow.FactionPanel.WinLeft, DialogHosts.FactionSelectionWindow.FactionPanel.WinTop, lockedSurface);
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
            WindowActions.ShowTitleScene(DialogHosts.TitleScene, false);
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
        dialog.SplashStage = 10;
        dialog._splashTimer = 300;
    }

    internal static void ShowSplash(TitleWindow dialog)
    {
        VideoDialog.SetVideoSize(DialogHosts.VideoDialog, 640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyBtnClick();
        dialog.MainWindow.Visible = true;
        dialog.TitlePanel.Visible = false;
        dialog.SplashPanel.ImageLibraryTexture = dialog.DemoImages;
        dialog.SplashPanel.ImageLibraryIndexTexture = 0;
        dialog.SplashPanel.Visible = true;
        dialog._splashTimer = 450;
        dialog.SplashStage = 0;
    }

}
