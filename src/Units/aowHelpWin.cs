// Translated from aowHelpWin.pas.
using Aow.Rtl;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.EventLog;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;

namespace Aow.Units.aowHelpWin
{
    // done callback, help event
    public partial class THelpWindow : TDataModule
    {
        public THelpWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow HelpWin;
        public TAOWLabel Title;
        public TAOWButton CloseBtn;
        public TAOWImage TitleIcon;
        public TAOWImage BBarEnd;
        public TAOWImage BBarBegin;
        public TAOWButton BCloseBut;
        public TAOWImage CeltLineR;
        public TAOWImage BBar;
        public TAOWImage CeltLineL;
        public TAOWVScrollBar HelpSB;
        public TAOWMemo HeaderMemo;
        public TAOWMemo HelpMemo;
        public TIvTranslator Translator;
        public TExecuteDoneEvent FDone;
        public THelpMessageEventLog FHelpEvent;
        // aowHelpWin.pas:53
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            THelpMessageEventLog Event = default;
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
        public void ShowHelpEvent(THelpMessageEventLog Event, bool Unused, TExecuteDoneEvent Done)
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
            bool bool__5 = call__4 > 0;
            if (bool__5)
            {
                var call__6 = HelpMemo.Items[0];
                bool__5 = call__6 == (AnsiString)"";
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
            this.HelpWin.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
            this.HelpWin.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
        }
    }

    public static partial class aowHelpWin
    {
        public static THelpWindow HelpWindow;
        // aowHelpWin.pas:82
        public static void ShowHelp(int HelpID)
        {
            TStringList Lines = default;
            if (HelpWindow.HelpWin.Visible)
            {
                HelpWindow.FinishDialog();
            }

            Lines = new TStringList();
            AoWHSSet.FTextControl.RetrieveText(HelpID, Lines);
            HelpWindow.HelpSB.Position = 0;
            HelpWindow.HeaderMemo.Items.Clear();
            var call__8 = Lines.Count;
            if (call__8 > 0)
            {
                var call__9 = Lines[0];
                HelpWindow.HeaderMemo.Items.Add(call__9);
            }

            HelpWindow.HelpMemo.Items.Clear();
            HelpWindow.HelpMemo.Items.AddStrings(Lines);
            var call__10 = HelpWindow.HelpMemo.Items.Count;
            if (call__10 > 0)
            {
                HelpWindow.HelpMemo.Items.Delete(0);
            }

            var call__11 = HelpWindow.HelpMemo.Items.Count;
            bool bool__12 = call__11 > 0;
            if (bool__12)
            {
                var call__13 = HelpWindow.HelpMemo.Items[0];
                bool__12 = call__13 == (AnsiString)"";
            }

            if (bool__12)
            {
                HelpWindow.HelpMemo.Items.Delete(0);
            }

            HelpWindow.HelpWin.Visible = true;
            Lines.Free();
        }
    }
}
