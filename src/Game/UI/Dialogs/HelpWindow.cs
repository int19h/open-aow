// Translated from aowHelpWin.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// done callback, help event
public partial class HelpWindow : DataModule
{
    public HelpWindow(Component owner) : base(owner)
    { }

    public Window HelpWin;
    public Label Title;
    public Button CloseBtn;
    public Image TitleIcon;
    public Image BBarEnd;
    public Image BBarBegin;
    public Button BCloseBut;
    public Image CeltLineR;
    public Image BBar;
    public Image CeltLineL;
    public VerticalScrollBar HelpSB;
    public Memo HeaderMemo;
    public Memo HelpMemo;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public HelpMessageEventLog FHelpEvent;

    // aowHelpWin.pas:53
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        HelpMessageEventLog Event;
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
    public void ShowHelpEvent(HelpMessageEventLog Event, bool Unused, ExecuteDoneEvent Done)
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
        this.HelpWin.Alignment.AlignHeight = AlignHeight.Top;
        this.HelpWin.Alignment.AlignWidth = AlignWidth.Left;
    }

    internal static void ShowHelp(int HelpID)
    {
        StringList Lines;
        if (DialogHosts.HelpWindow.HelpWin.Visible)
        {
            DialogHosts.HelpWindow.FinishDialog();
        }

        Lines = new StringList();
        AoWHSSet.Current.FTextControl.RetrieveText(HelpID, Lines);
        DialogHosts.HelpWindow.HelpSB.Position = 0;
        DialogHosts.HelpWindow.HeaderMemo.Items.Clear();
        if (Lines.Count > 0)
        {
            var line = Lines[0];
            DialogHosts.HelpWindow.HeaderMemo.Items.Add(line);
        }

        DialogHosts.HelpWindow.HelpMemo.Items.Clear();
        DialogHosts.HelpWindow.HelpMemo.Items.AddStrings(Lines);
        if (DialogHosts.HelpWindow.HelpMemo.Items.Count > 0)
        {
            DialogHosts.HelpWindow.HelpMemo.Items.Delete(0);
        }

        if (DialogHosts.HelpWindow.HelpMemo.Items.Count > 0 && RtlUnit.IsEmpty(DialogHosts.HelpWindow.HelpMemo.Items[0]))
        {
            DialogHosts.HelpWindow.HelpMemo.Items.Delete(0);
        }

        DialogHosts.HelpWindow.HelpWin.Visible = true;
        Lines?.Dispose();
    }

}
