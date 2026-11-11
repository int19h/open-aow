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
    public int FSplashTimer;
    public int FSplashStage;

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
        SplashPnl.ILTexture = DemoIL;
        SplashPnl.ILIndexTexture = 0;
        SplashPnl.Visible = true;
        FSplashTimer = 450;
        FSplashStage = 0;
    }

    // AOWTitleWindow.pas:97
    public void ShowExitSplash()
    {
        InitializeSplash();
        SplashPnl.ILIndexTexture = 2;
        FSplashStage = 10;
        FSplashTimer = 300;
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
        SplashPnl.ILIndexTexture = 0;
        FSplashTimer = 450;
    }

    // AOWTitleWindow.pas:42
    public void TitleWinDrawSurface(Surface Surface, int X, int Y)
    {
        LockedSurface SD = default;
        if (DialogHosts.SelFactionWin.FactionPnl.Visible && DialogHosts.SelFactionWin.FFade >= 0)
        {
            Surface.Lock(ref SD);
            if (DialogHosts.SelFactionWin.FFadeDirection == 0)
            {
                DialogHosts.IntGfxMod.FactionWin[DialogHosts.SelFactionWin.FFadeImage].ShowBlended(DialogHosts.SelFactionWin.FactionPnl.WinLeft, DialogHosts.SelFactionWin.FactionPnl.WinTop, SD, GraphicsState.AlphaBlendTable.LookupTables[100 - DialogHosts.SelFactionWin.FFade]);
            }
            else
            {
                DialogHosts.IntGfxMod.FactionWin[DialogHosts.SelFactionWin.FFadeImage].ShowBlended(DialogHosts.SelFactionWin.FactionPnl.WinLeft, DialogHosts.SelFactionWin.FactionPnl.WinTop, SD, GraphicsState.AlphaBlendTable.LookupTables[DialogHosts.SelFactionWin.FFade]);
            }

            Surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(LockedSurface SD)
    {
        DialogHosts.IntGfxMod.FactionWin[0].Show(DialogHosts.SelFactionWin.FactionPnl.WinLeft, DialogHosts.SelFactionWin.FactionPnl.WinTop, SD);
    }

    // AOWTitleWindow.pas:115
    public void SplashPnlUpdate()
    {
        FSplashTimer -= 1;
        if (FSplashTimer == 0)
        {
            NextSplash();
        }
    }

    // AOWTitleWindow.pas:121
    public void SplashPnlKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            HideSplash();
            WindowActions.ShowTitleScene(DialogHosts.TitleScene, false);
        }
        else
        {
            if (!Shift.Contains(ShiftState.Alt))
            {
                NextSplash();
            }
        }
    }

    // AOWTitleWindow.pas:131
    public void SplashPnlMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        NextSplash();
    }

    internal static void ShowExitSplash(TitleWindow Dialog)
    {
        ShowSplash(Dialog);
        Dialog.SplashPnl.ILIndexTexture = 2;
        Dialog.FSplashStage = 10;
        Dialog.FSplashTimer = 300;
    }

    internal static void ShowSplash(TitleWindow Dialog)
    {
        VideoDialog.SetVideoSize(DialogHosts.VideoDialog, 640, 480);
        VideoDialog.SetVideoMode(DialogHosts.VideoDialog, VideoDialog.ScreenMode.Title);
        DialogHosts.VideoDialog.ApplyBtnClick();
        Dialog.TitleWin.Visible = true;
        Dialog.TitlePnl.Visible = false;
        Dialog.SplashPnl.ILTexture = Dialog.DemoIL;
        Dialog.SplashPnl.ILIndexTexture = 0;
        Dialog.SplashPnl.Visible = true;
        Dialog.FSplashTimer = 450;
        Dialog.FSplashStage = 0;
    }

}
