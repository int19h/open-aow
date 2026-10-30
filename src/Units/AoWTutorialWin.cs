// Translated from AoWTutorialWin.pas.
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
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWTutorialWin
{
    // done callback, retained event
    public partial class TTutorialWindow : Aow.Rtl.TDataModule
    {
        public TTutorialWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        public Aow.Units.AoWButton.TAOWButton Close;
        public Aow.Units.AoWMemo.TAOWMemo HeaderMemo;
        public Aow.Units.AoWVScroll.TAOWVScrollBar AbVSB;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.EventLog.TEventLog FEvent;
        // AoWTutorialWin.pas:51
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TEventLog Log = default;
            Done = FDone;
            Log = FEvent;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Log);
            }

            if (Log != null)
            {
                Log.Release();
            }
        }

        // AoWTutorialWin.pas:38
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWTutorialWin.pas:44
        public void CloseClick()
        {
            Dlg.Visible = false;
        }
    }

    public static partial class AoWTutorialWin
    {
    }
}
