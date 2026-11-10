// Translated from AOWTitleWindow.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Events;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// splash timer, splash stage
public partial class TTitleWindow : TDataModule
{
    public TTitleWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow TitleWin;
    public TAOWPanel TitlePnl;
    public TAOWPanel SplashPnl;
    public TInterfaceIL DemoIL;
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
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.SetSceneSize(640, 480);
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmTitle);
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyMode();
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
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Clear();
        Aow.Game.Audio.AudioHost.SongPlayer.Songs.Add("SONGS\\TITLE.IT");
        Aow.Game.Audio.AudioHost.SongPlayer.Play(0);
        SplashPnl.ILIndexTexture = 0;
        FSplashTimer = 450;
    }

    // AOWTitleWindow.pas:42
    public void TitleWinDrawSurface(TSurface Surface, int X, int Y)
    {
        TSurfaceDesc SD = default;
        if (Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.Visible && Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFade >= 0)
        {
            Surface.Lock(ref SD);
            if (Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFadeDirection == 0)
            {
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.FactionWin[Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFadeImage].ShowBlended(Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinLeft, Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinTop, SD, Aow.Game.Draw.GfxState.AlphaBlendTable.LookupTables[100 - Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFade]);
            }
            else
            {
                Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.FactionWin[Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFadeImage].ShowBlended(Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinLeft, Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinTop, SD, Aow.Game.Draw.GfxState.AlphaBlendTable.LookupTables[Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FFade]);
            }

            Surface.Unlock();
        }
    }

    // AOWTitleWindow.pas:61
    public void TitlePnlDrawBack(TSurfaceDesc SD)
    {
        Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.FactionWin[0].Show(Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinLeft, Aow.Game.Ui.Dialogs.DialogHosts.SelFactionWin.FactionPnl.WinTop, SD);
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
    public void SplashPnlKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            HideSplash();
            Aow.Game.Ui.Dialogs.WindowActions.ShowTitleScene(Aow.Game.Ui.Dialogs.DialogHosts.TitleScene, false);
        }
        else
        {
            if (!Shift.Contains(TShiftState.ssAlt))
            {
                NextSplash();
            }
        }
    }

    // AOWTitleWindow.pas:131
    public void SplashPnlMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        NextSplash();
    }

    internal static void ShowExitSplash(TTitleWindow Dialog)
    {
        Aow.Game.Ui.Dialogs.TTitleWindow.ShowSplash(Dialog);
        Dialog.SplashPnl.ILIndexTexture = 2;
        Dialog.FSplashStage = 10;
        Dialog.FSplashTimer = 300;
    }

    internal static void ShowSplash(TTitleWindow Dialog)
    {
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoSize(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, 640, 480);
        Aow.Game.Ui.Dialogs.TVideoDialog.SetVideoMode(Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog, TScreenMode.scmTitle);
        Aow.Game.Ui.Dialogs.DialogHosts.VideoDialog.ApplyBtnClick();
        Dialog.TitleWin.Visible = true;
        Dialog.TitlePnl.Visible = false;
        Dialog.SplashPnl.ILTexture = Dialog.DemoIL;
        Dialog.SplashPnl.ILIndexTexture = 0;
        Dialog.SplashPnl.Visible = true;
        Dialog.FSplashTimer = 450;
        Dialog.FSplashStage = 0;
    }

}
