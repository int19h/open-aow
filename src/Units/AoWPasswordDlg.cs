// Translated from AoWPasswordDlg.pas.
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
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using static Aow.Units.EventLog.EventLog;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWFrame;
using static Aow.Units.AoWFrame.AoWFrame;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.AoWPasswordDlg
{
    // { TPasswordDlg (instance size 124, VMT 0044DEA4) }
    public partial class TPasswordDlg : Aow.Rtl.TDataModule
    {
        public TPasswordDlg(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow Dlg;
        // { +0044 }
        public Aow.Units.AoWLabel.TAOWLabel TitleLbl;
        // { +0048 }
        public Aow.Units.AoWFrame.TAoWFrame BCeltL;
        // { +004C }
        public Aow.Units.AoWFrame.TAoWFrame TCeltL;
        // { +0050 }
        public Aow.Units.AoWImage.TAOWImage TCeltLineL;
        // { +0054 }
        public Aow.Units.AoWPanel.TAOWPanel OKCancelPnl;
        // { +0058 }
        public Aow.Units.AoWButton.TAOWButton OKBtn;
        // { +005C }
        public Aow.Units.AoWButton.TAOWButton CancelBtn;
        // { +0060 }
        public Aow.Units.AoWLabel.TAOWLabel PasswordLbl;
        // { +0064 }
        public Aow.Units.AoWEdit.TAOWEdit PasswordEdit;
        // { +0068 }
        public Aow.Rtl.TIvTranslator Translator;
        // { +006C }
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        // { $70 }
        public Aow.Units.EventLog.TPasswordEventLog FPasswordEvent;
        // AoWPasswordDlg.pas:43
        // { $78 }
        public void Finish()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.TPasswordEventLog Event = default;
            Dlg.Visible = false;
            Done = FDone;
            Event = FPasswordEvent;
            FDone = null;
            FPasswordEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // AoWPasswordDlg.pas:42
        public void Reset()
        {
        }

        // AoWPasswordDlg.pas:55
        public void DlgHide()
        {
            Finish();
        }

        // AoWPasswordDlg.pas:58
        public void OKBtnClick(Aow.Rtl.TObject Sender)
        {
            FPasswordEvent.FPassword = PasswordEdit.Text;
            FPasswordEvent.FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebOk;
            Dlg.Visible = false;
        }

        // AoWPasswordDlg.pas:65
        public void CancelBtnClick(Aow.Rtl.TObject Sender)
        {
            FPasswordEvent.FPassword = (AnsiString)"";
            FPasswordEvent.FPressedButton = Aow.Units.EventLog.TPressedEventLogButton.pebCancel;
            Dlg.Visible = false;
        }

        // AoWPasswordDlg.pas:72
        public void DlgKeyDown(Aow.Rtl.TObject Sender, ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            if (Key == 27)
            {
                CancelBtnClick(null);
            }
        }
    }

    public static partial class AoWPasswordDlg
    {
    }
}
