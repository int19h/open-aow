// Generated from AOWTitleWindow.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;
using Aow.Units.E2Globals;
using static Aow.Units.E2Globals.E2Globals;
using Aow.Units.E2GameFlow;
using static Aow.Units.E2GameFlow.E2GameFlow;
using Aow.Units.AoWVideoDlg;
using static Aow.Units.AoWVideoDlg.AoWVideoDlg;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWSelFactionWin;
using static Aow.Units.AoWSelFactionWin.AoWSelFactionWin;

namespace Aow.Units.AOWTitleWindow
{
    // { TTitleWindow: size $5C; $54 splash timer, $58 splash stage. VMT 00429864. }
    public partial class TTitleWindow : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow TitleWin;
        // { +0044 }
        public Aow.Units.AoWPanel.TAOWPanel TitlePnl;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel SplashPnl;
        // { +004C }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL DemoIL;
        // { +0050 }
        public int FSplashTimer;
        // { $54 }
        public int FSplashStage;
        // AOWTitleWindow.pas:66
        // { $58; 00429C2C }
        public void HideSplash()
        {
            DemoIL.ReleaseDiscardableImageData();
            SplashPnl.Visible = false;
            TitleWin.Visible = false;
        }

        // AOWTitleWindow.pas:73
        public void InitializeSplash()
        {
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.SetSceneSize(640, 480);
            Aow.Units.AoWExeForms.AoWExeForms.SetVideoMode(Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog, Aow.Units.AoWVideoDlg.TScreenMode.scmTitle);
            Aow.Units.AoWVideoDlg.AoWVideoDlg.VideoDialog.ApplyMode();
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
            /* TODO(T-shim) Platform or editor boundary Forms.Close */
            throw new System.NotImplementedException();
        }

        // AOWTitleWindow.pas:87
        public void ShowSplash()
        {
            InitializeSplash();
            Aow.Units.Sound.Sound.SongPlayer.Songs.Clear();
            Aow.Units.Sound.Sound.SongPlayer.Songs.Add((AnsiString)"SONGS\\TITLE.IT");
            Aow.Units.Sound.Sound.SongPlayer.Play(0);
            SplashPnl.ILIndexTexture = 0;
            FSplashTimer = 450;
        }

        // AOWTitleWindow.pas:42
        public void TitleWinDrawSurface(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurface Surface, int X, int Y)
        {
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            if (Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.Visible && (Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFade >= 0))
            {
                Surface.Lock(ref SD);
                if (Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFadeDirection == 0)
                {
                    var call__1 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFadeImage];
                    call__1.ShowBlended(Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinLeft, Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinTop, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[100 - Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFade]);
                }
                else
                {
                    var call__2 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFadeImage];
                    call__2.ShowBlended(Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinLeft, Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinTop, SD, Aow.Units.GFXE.GFXE.AlphaBlendTable.LookupTables[Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FFade]);
                }

                Surface.Unlock();
            }
        }

        // AOWTitleWindow.pas:61
        public void TitlePnlDrawBack(Aow.Rtl.TObject Sender, Aow.Units.GFXE.TSurfaceDesc SD)
        {
            var call__3 = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FactionWin[0];
            call__3.Show(Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinLeft, Aow.Units.AoWSelFactionWin.AoWSelFactionWin.SelFactionWin.FactionPnl.WinTop, SD);
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
        public void SplashPnlKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                HideSplash();
                Aow.Units.E2GameFlow.E2GameFlow.ShowTitleScene(Aow.Units.E2Globals.E2Globals.TitleScene, false);
            }
            else
            {
                if (!Shift.Contains(Aow.Rtl.TShiftState.ssAlt))
                {
                    NextSplash();
                }
            }
        }

        // AOWTitleWindow.pas:131
        public void SplashPnlMouseDown(Aow.Rtl.TObject Sender, Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            NextSplash();
        }
    }

    public static partial class AOWTitleWindow
    {
    }
}
