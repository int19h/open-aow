// Translated from AOWTitleWindow.pas.
using Aow.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using static Aow.Units.Sound.Sound;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWExeForms.AoWExeForms;
using static Aow.Units.E2Globals.E2Globals;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using static Aow.Units.AoWSelFactionWin.AoWSelFactionWin;

namespace Aow.Units.AOWTitleWindow
{
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
            VideoDialog.SetSceneSize(640, 480);
            SetVideoMode(VideoDialog, TScreenMode.scmTitle);
            VideoDialog.ApplyMode();
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
            SongPlayer.Songs.Clear();
            SongPlayer.Songs.Add("SONGS\\TITLE.IT");
            SongPlayer.Play(0);
            SplashPnl.ILIndexTexture = 0;
            FSplashTimer = 450;
        }

        // AOWTitleWindow.pas:42
        public void TitleWinDrawSurface(TObject Sender, TSurface Surface, int X, int Y)
        {
            TSurfaceDesc SD = default;
            if (SelFactionWin.FactionPnl.Visible && SelFactionWin.FFade >= 0)
            {
                Surface.Lock(ref SD);
                if (SelFactionWin.FFadeDirection == 0)
                {
                    aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[SelFactionWin.FFadeImage].ShowBlended(SelFactionWin.FactionPnl.WinLeft, SelFactionWin.FactionPnl.WinTop, SD, AlphaBlendTable.LookupTables[100 - SelFactionWin.FFade]);
                }
                else
                {
                    aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[SelFactionWin.FFadeImage].ShowBlended(SelFactionWin.FactionPnl.WinLeft, SelFactionWin.FactionPnl.WinTop, SD, AlphaBlendTable.LookupTables[SelFactionWin.FFade]);
                }

                Surface.Unlock();
            }
        }

        // AOWTitleWindow.pas:61
        public void TitlePnlDrawBack(TObject Sender, TSurfaceDesc SD)
        {
            aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[0].Show(SelFactionWin.FactionPnl.WinLeft, SelFactionWin.FactionPnl.WinTop, SD);
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
        public void SplashPnlKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                HideSplash();
                ShowTitleScene(TitleScene, false);
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
        public void SplashPnlMouseDown(TObject Sender, TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            NextSplash();
        }
    }

    public static partial class AOWTitleWindow
    {
    }
}
