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

    public Window TitleWin;
    public Panel TitlePnl;
    public Panel SplashPnl;
    public InterfaceImageLibrary DemoIL;
    public int SplashTimer;
    public int SplashStage;

    // AOWTitleWindow.pas:66
    public void HideSplash()
    {
        DemoIL.ReleaseDiscardableImageData();
        SplashPnl.Visible = false;
        TitleWin.Visible = false;
    }

    // AOWTitleWindow.pas:73
    public void InitializeSplash()
    {
        DialogHosts.VideoDialog.SetSceneSize(640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyMode();
        TitleWin.Visible = true;
        TitlePnl.Visible = false;
        SplashPnl.ImageLibraryTexture = DemoIL;
        SplashPnl.ImageLibraryIndexTexture = 0;
        SplashPnl.Visible = true;
        SplashTimer = 450;
        SplashStage = 0;
    }

    // AOWTitleWindow.pas:97
    public void ShowExitSplash()
    {
        InitializeSplash();
        SplashPnl.ImageLibraryIndexTexture = 2;
        SplashStage = 10;
        SplashTimer = 300;
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
        SplashPnl.ImageLibraryIndexTexture = 0;
        SplashTimer = 450;
    }

    // AOWTitleWindow.pas:42
    public void TitleWinDrawSurface(Surface surface, int X, int Y)
    {
        LockedSurface lockedSurface = default;
        if (DialogHosts.FactionSelectionWindow.FactionPnl.Visible && DialogHosts.FactionSelectionWindow.Fade >= 0)
        {
            surface.Lock(ref lockedSurface);
            if (DialogHosts.FactionSelectionWindow.FadeDirection == 0)
            {
                DialogHosts.UIGraphics.FactionWin[DialogHosts.FactionSelectionWindow.FadeImage].ShowBlended(DialogHosts.FactionSelectionWindow.FactionPnl.WinLeft, DialogHosts.FactionSelectionWindow.FactionPnl.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[100 - DialogHosts.FactionSelectionWindow.Fade]);
            }
            else
            {
                DialogHosts.UIGraphics.FactionWin[DialogHosts.FactionSelectionWindow.FadeImage].ShowBlended(DialogHosts.FactionSelectionWindow.FactionPnl.WinLeft, DialogHosts.FactionSelectionWindow.FactionPnl.WinTop, lockedSurface, GraphicsState.AlphaBlendTable.LookupTables[DialogHosts.FactionSelectionWindow.Fade]);
            }

            surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(LockedSurface lockedSurface)
    {
        DialogHosts.UIGraphics.FactionWin[0].Show(DialogHosts.FactionSelectionWindow.FactionPnl.WinLeft, DialogHosts.FactionSelectionWindow.FactionPnl.WinTop, lockedSurface);
    }

    // AOWTitleWindow.pas:115
    public void SplashPnlUpdate()
    {
        SplashTimer -= 1;
        if (SplashTimer == 0)
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
        dialog.SplashPnl.ImageLibraryIndexTexture = 2;
        dialog.SplashStage = 10;
        dialog.SplashTimer = 300;
    }

    internal static void ShowSplash(TitleWindow dialog)
    {
        VideoDialog.SetVideoSize(DialogHosts.VideoDialog, 640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyBtnClick();
        dialog.TitleWin.Visible = true;
        dialog.TitlePnl.Visible = false;
        dialog.SplashPnl.ImageLibraryTexture = dialog.DemoIL;
        dialog.SplashPnl.ImageLibraryIndexTexture = 0;
        dialog.SplashPnl.Visible = true;
        dialog.SplashTimer = 450;
        dialog.SplashStage = 0;
    }

}
