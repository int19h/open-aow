// Translated from aowHelpWin.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWComp;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.EventLog;
using static Aow.Units.AoWE.AoWEUnit;

namespace Aow.Units.aowHelpWin;

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
        TExecuteDoneEvent Done;
        THelpMessageEventLog Event;
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
        if (Event.FMessage.Count > 0)
        {
            var message = Event.FMessage[0];
            HeaderMemo.Items.Add(message);
        }

        HelpMemo.Items.Clear();
        HelpMemo.Items.AddStrings(Event.FMessage);
        if (HelpMemo.Items.Count > 0)
        {
            HelpMemo.Items.Delete(0);
        }

        if (HelpMemo.Items.Count > 0 && RtlUnit.IsEmpty(HelpMemo.Items[0]))
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

public static partial class aowHelpWinUnit
{
    public static THelpWindow HelpWindow;

    // aowHelpWin.pas:82
    public static void ShowHelp(int HelpID)
    {
        TStringList Lines;
        if (HelpWindow.HelpWin.Visible)
        {
            HelpWindow.FinishDialog();
        }

        Lines = new TStringList();
        AoWHSSet.FTextControl.RetrieveText(HelpID, Lines);
        HelpWindow.HelpSB.Position = 0;
        HelpWindow.HeaderMemo.Items.Clear();
        if (Lines.Count > 0)
        {
            var line = Lines[0];
            HelpWindow.HeaderMemo.Items.Add(line);
        }

        HelpWindow.HelpMemo.Items.Clear();
        HelpWindow.HelpMemo.Items.AddStrings(Lines);
        if (HelpWindow.HelpMemo.Items.Count > 0)
        {
            HelpWindow.HelpMemo.Items.Delete(0);
        }

        if (HelpWindow.HelpMemo.Items.Count > 0 && RtlUnit.IsEmpty(HelpWindow.HelpMemo.Items[0]))
        {
            HelpWindow.HelpMemo.Items.Delete(0);
        }

        HelpWindow.HelpWin.Visible = true;
        Lines.Free();
    }
}
