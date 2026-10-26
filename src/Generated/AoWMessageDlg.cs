// Generated from AoWMessageDlg.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWExeForms;
using static Aow.Units.AoWExeForms.AoWExeForms;

namespace Aow.Units.AoWMessageDlg
{
    // { TMessageDlg (size $BC, VMT 0040E4E8): $B0 completion event, $B8 retained message event. }
    public partial class TMessageDlg : Aow.Rtl.TDataModule
    {
        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        // { +0048 }
        public Aow.Units.AoWPanel.TAOWPanel YesNoPnl;
        // { +004C }
        public Aow.Units.AoWButton.TAOWButton YesBtn;
        // { +0050 }
        public Aow.Units.AoWButton.TAOWButton NoBtn;
        // { +0054 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +0058 }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +005C }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0060 }
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        // { +0064 }
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        // { +0068 }
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +006C }
        public Aow.Units.AoWPanel.TAOWPanel OKPnl;
        // { +0070 }
        public Aow.Units.AoWPanel.TAOWPanel OKCancelPnl;
        // { +0074 }
        public Aow.Units.AoWButton.TAOWButton OKBtn2;
        // { +0078 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +007C }
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        // { +0080 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar MessageMemoSB;
        // { +0084 }
        public Aow.Units.AoWPanel.TAOWPanel YesNoCancelPnl;
        // { +0088 }
        public Aow.Units.AoWButton.TAOWButton YesBtn2;
        // { +008C }
        public Aow.Units.AoWButton.TAOWButton NoBtn2;
        // { +0090 }
        public Aow.Units.AoWButton.TAOWButton CancelBtn2;
        // { +0094 }
        public Aow.Units.AoWPanel.TAOWPanel MemoPnl;
        // { +0098 }
        public Aow.Units.AoWPanel.TAOWPanel HeaderMemoPnl;
        // { +009C }
        public Aow.Units.AoWMemo.TAOWMemo HeaderMemo;
        // { +00A0 }
        public Aow.Units.AoWVScroll.TAOWVScrollBar HeaderMemoSB;
        // { +00A4 }
        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        // { +00A8 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +00AC }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $B0 }
        public Aow.Units.EventLog.TMessageEventLog FEvent;
        // AoWMessageDlg.pas:68
        // { $B8 }
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TMessageEventLog Log = default;
            Dlg.Visible = false;
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

        // AoWMessageDlg.pas:83
        // { 0040E780 }
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWMessageDlg.pas:88
        // { draft }
        public void YesBtnClick()
        {
            var call__1 = FEvent.Yes();
            if (call__1)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:93
        // { draft }
        public void NoBtnClick()
        {
            var call__2 = FEvent.No();
            if (call__2)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:98
        // { draft }
        public void OKBtnClick()
        {
            if (FEvent != null)
            {
                var call__3 = FEvent.OK();
                if (!call__3)
                {
                    return;
                }
            }

            Dlg.Visible = false;
        }

        // AoWMessageDlg.pas:105
        // { draft }
        public void OKBtn2Click()
        {
            var call__4 = FEvent.OK();
            if (call__4)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:110
        // { draft }
        public void CancelBtnClick()
        {
            var call__5 = FEvent.Cancel();
            if (call__5)
            {
                Dlg.Visible = false;
            }
        }
    }

    public static partial class AoWMessageDlg
    {
        // AoWMessageDlg.pas:116
        // { draft }
        public static void ShowDemoMessage()
        {
            Aow.Units.AoWExeForms.AoWExeForms.ResetMessageDialog(Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg);
            Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add((AnsiString)"");
            var call__6 = Aow.Units.AoWE.AoWE.TranslateRStr(Aow.Units.AoWE.AoWE.DemoVersionMessageRStr);
            Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.MessageMemo.Items.Add(call__6);
            Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.OKPnl.Visible = true;
            Aow.Units.AoWMessageDlg.AoWMessageDlg.MessageDlg.Dlg.Visible = true;
        }

        public static Aow.Units.AoWMessageDlg.TMessageDlg MessageDlg;
    }
}
