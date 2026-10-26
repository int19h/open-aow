// Generated from AoWMsgReceiveDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
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

namespace Aow.Units.AoWMsgReceiveDlg
{
    // { TMsgReceiveDlg (instance size $70, VMT 00438EC4): $64 done callback, $6C retained event. }
    public partial class TMsgReceiveDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow MsgReceiveWin;
        // { +0044 }
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        // { +0048 }
        public Aow.Units.AoWButton.TAOWButton Close;
        // { +004C }
        public Aow.Units.AoWVScroll.TAOWVScrollBar MessageSB;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage LShield;
        // { +0054 }
        public Aow.Units.AoWImage.TAOWImage RShield;
        // { +0058 }
        public Aow.Units.AoWMemo.TAOWMemo HeaderMemo;
        // { +005C }
        public Aow.Rtl.TIvTranslator Translator;
        // { +0060 }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $64 }
        public Aow.Units.EventLog.TEventLog FEvent;
        // AoWMsgReceiveDlg.pas:54
        // { $6C }
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

        // AoWMsgReceiveDlg.pas:40
        public void CloseClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            MsgReceiveWin.Visible = false;
        }

        // AoWMsgReceiveDlg.pas:47
        public void MsgReceiveWinHide()
        {
            FinishDialog();
        }
    }

    public static partial class AoWMsgReceiveDlg
    {
    }
}
