// Translated from AoWMessageDlg.pas.
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
    // completion event, retained message event
    public partial class TMessageDlg : Aow.Rtl.TDataModule
    {
        public TMessageDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        public Aow.Units.AoWPanel.TAOWPanel YesNoPnl;
        public Aow.Units.AoWButton.TAOWButton YesBtn;
        public Aow.Units.AoWButton.TAOWButton NoBtn;
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        public Aow.Units.AoWImage.TAOWImage AOWImage1;
        public Aow.Units.AoWImage.TAOWImage AOWImage2;
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        public Aow.Units.AoWPanel.TAOWPanel OKPnl;
        public Aow.Units.AoWPanel.TAOWPanel OKCancelPnl;
        public Aow.Units.AoWButton.TAOWButton OKBtn2;
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        public Aow.Units.AoWMemo.TAOWMemo MessageMemo;
        public Aow.Units.AoWVScroll.TAOWVScrollBar MessageMemoSB;
        public Aow.Units.AoWPanel.TAOWPanel YesNoCancelPnl;
        public Aow.Units.AoWButton.TAOWButton YesBtn2;
        public Aow.Units.AoWButton.TAOWButton NoBtn2;
        public Aow.Units.AoWButton.TAOWButton CancelBtn2;
        public Aow.Units.AoWPanel.TAOWPanel MemoPnl;
        public Aow.Units.AoWPanel.TAOWPanel HeaderMemoPnl;
        public Aow.Units.AoWMemo.TAOWMemo HeaderMemo;
        public Aow.Units.AoWVScroll.TAOWVScrollBar HeaderMemoSB;
        public Aow.Units.AoWLabel.TAOWLabel HeaderLbl;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.EventLog.TMessageEventLog FEvent;
        // AoWMessageDlg.pas:68
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
        public void DlgHide()
        {
            FinishDialog();
        }

        // AoWMessageDlg.pas:88
        public void YesBtnClick()
        {
            var call__1 = FEvent.Yes();
            if (call__1)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:93
        public void NoBtnClick()
        {
            var call__2 = FEvent.No();
            if (call__2)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:98
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
        public void OKBtn2Click()
        {
            var call__4 = FEvent.OK();
            if (call__4)
            {
                Dlg.Visible = false;
            }
        }

        // AoWMessageDlg.pas:110
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
