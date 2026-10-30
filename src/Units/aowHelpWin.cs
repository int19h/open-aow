// Translated from aowHelpWin.pas.
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
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;

namespace Aow.Units.aowHelpWin
{
    // done callback, help event
    public partial class THelpWindow : Aow.Rtl.TDataModule
    {
        public THelpWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow HelpWin;
        public Aow.Units.AoWLabel.TAOWLabel Title;
        public Aow.Units.AoWButton.TAOWButton CloseBtn;
        public Aow.Units.AoWImage.TAOWImage TitleIcon;
        public Aow.Units.AoWImage.TAOWImage BBarEnd;
        public Aow.Units.AoWImage.TAOWImage BBarBegin;
        public Aow.Units.AoWButton.TAOWButton BCloseBut;
        public Aow.Units.AoWImage.TAOWImage CeltLineR;
        public Aow.Units.AoWImage.TAOWImage BBar;
        public Aow.Units.AoWImage.TAOWImage CeltLineL;
        public Aow.Units.AoWVScroll.TAOWVScrollBar HelpSB;
        public Aow.Units.AoWMemo.TAOWMemo HeaderMemo;
        public Aow.Units.AoWMemo.TAOWMemo HelpMemo;
        public Aow.Rtl.TIvTranslator Translator;
        public Aow.Units.EventLog.TExecuteDoneEvent FDone;
        public Aow.Units.EventLog.THelpMessageEventLog FHelpEvent;
        // aowHelpWin.pas:53
        public void FinishDialog()
        {
            Aow.Units.EventLog.TExecuteDoneEvent Done = default;
            Aow.Units.EventLog.THelpMessageEventLog Event = default;
            Done = FDone;
            Event = FHelpEvent;
            FDone = null;
            FHelpEvent = null;
            if (Done != null)
            {
                Done(Event);
            }

            if (Event != null)
            {
                Event.Release();
            }
        }

        // aowHelpWin.pas:66
        public void ShowHelpEvent(Aow.Units.EventLog.THelpMessageEventLog Event, bool Unused, Aow.Units.EventLog.TExecuteDoneEvent Done)
        {
            FinishDialog();
            FHelpEvent = Event;
            FHelpEvent.AddRef();
            HeaderMemo.Items.Clear();
            var call__1 = Event.FMessage.Count;
            if (call__1 > 0)
            {
                var call__2 = Event.FMessage[0];
                HeaderMemo.Items.Add(call__2);
            }

            HelpMemo.Items.Clear();
            HelpMemo.Items.AddStrings(Event.FMessage);
            var call__3 = HelpMemo.Items.Count;
            if (call__3 > 0)
            {
                HelpMemo.Items.Delete(0);
            }

            var call__4 = HelpMemo.Items.Count;
            bool bool__5 = (call__4 > 0);
            if (bool__5)
            {
                var call__6 = HelpMemo.Items[0];
                bool__5 = (call__6 == ((AnsiString)""));
            }

            if (bool__5)
            {
                HelpMemo.Items.Delete(0);
            }

            if (Event.FMode != 0)
            {
                FDone = Done;
            }
            else
            {
                if (Done != null)
                {
                    Done(FHelpEvent);
                }
            }

            HelpWin.Visible = true;
        }

        // aowHelpWin.pas:100
        public void CloseBtnClick()
        {
            HelpWin.Visible = false;
        }

        // aowHelpWin.pas:105
        public void HelpWinHide()
        {
            FinishDialog();
        }

        // aowHelpWin.pas:110
        public void HelpWinResize()
        {
            this.HelpWin.Alignment.LeftOffset = this.HelpWin.WinLeft;
            this.HelpWin.Alignment.TopOffset = this.HelpWin.WinTop;
            this.HelpWin.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop;
            this.HelpWin.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;
        }
    }

    public static partial class aowHelpWin
    {
        public static Aow.Units.aowHelpWin.THelpWindow HelpWindow;
        // aowHelpWin.pas:82
        public static void ShowHelp(int HelpID)
        {
            Aow.Rtl.TStringList Lines = default;
            if (Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpWin.Visible)
            {
                Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.FinishDialog();
            }

            Lines = new Aow.Rtl.TStringList();
            Aow.Units.AoWE.AoWE.AoWHSSet.FTextControl.RetrieveText(HelpID, Lines);
            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpSB.Position = 0;
            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HeaderMemo.Items.Clear();
            var call__8 = Lines.Count;
            if (call__8 > 0)
            {
                var call__9 = Lines[0];
                Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HeaderMemo.Items.Add(call__9);
            }

            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.Clear();
            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.AddStrings(Lines);
            var call__10 = Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.Count;
            if (call__10 > 0)
            {
                Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.Delete(0);
            }

            var call__11 = Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.Count;
            bool bool__12 = (call__11 > 0);
            if (bool__12)
            {
                var call__13 = Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items[0];
                bool__12 = (call__13 == ((AnsiString)""));
            }

            if (bool__12)
            {
                Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpMemo.Items.Delete(0);
            }

            Aow.Units.aowHelpWin.aowHelpWin.HelpWindow.HelpWin.Visible = true;
            Lines.Free();
        }
    }
}
