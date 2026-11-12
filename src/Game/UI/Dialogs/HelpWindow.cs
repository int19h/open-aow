// Translated from aowHelpWin.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// done callback, help event
public partial class HelpWindow : DataModule
{
    public HelpWindow(Component owner) : base(owner)
    { }

    public Window MainWindow;
    public VerticalScrollBar HelpScrollBar;
    public Memo HeaderMemo;
    public Memo HelpMemo;
    private ExecuteDoneEvent _done;
    private HelpMessageEventLog _helpEvent;

    // aowHelpWin.pas:53
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        HelpMessageEventLog eventValue;
        done = _done;
        eventValue = _helpEvent;
        _done = null;
        _helpEvent = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
        }
    }

    // aowHelpWin.pas:66
    public void ShowHelpEvent(HelpMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        FinishDialog();
        _helpEvent = eventValue;
        _helpEvent.AddRef();
        HeaderMemo.Items.Clear();
        if (eventValue.Message.Count > 0)
        {
            var message = eventValue.Message[0];
            HeaderMemo.Items.Add(message);
        }

        HelpMemo.Items.Clear();
        HelpMemo.Items.AddStrings(eventValue.Message);
        if (HelpMemo.Items.Count > 0)
        {
            HelpMemo.Items.Delete(0);
        }

        if (HelpMemo.Items.Count > 0 && RtlUnit.IsEmpty(HelpMemo.Items[0]))
        {
            HelpMemo.Items.Delete(0);
        }

        if (eventValue.Mode != 0)
        {
            _done = done;
        }
        else
        {
            if (done != null)
            {
                done(_helpEvent);
            }
        }

        MainWindow.Visible = true;
    }

    // aowHelpWin.pas:100
    public void CloseBtnClick()
    {
        MainWindow.Visible = false;
    }

    // aowHelpWin.pas:105
    public void HelpWinHide()
    {
        FinishDialog();
    }

    // aowHelpWin.pas:110
    public void HelpWinResize()
    {
        this.MainWindow.Alignment.LeftOffset = this.MainWindow.WinLeft;
        this.MainWindow.Alignment.TopOffset = this.MainWindow.WinTop;
        this.MainWindow.Alignment.AlignHeight = AlignHeight.Top;
        this.MainWindow.Alignment.AlignWidth = AlignWidth.Left;
    }

    internal static void ShowHelp(int helpId)
    {
        StringList lines;
        if (DialogHosts.HelpWindow.MainWindow.Visible)
        {
            DialogHosts.HelpWindow.FinishDialog();
        }

        lines = new StringList();
        GamePack.Current.TextControl.RetrieveText(helpId, lines);
        DialogHosts.HelpWindow.HelpScrollBar.Position = 0;
        DialogHosts.HelpWindow.HeaderMemo.Items.Clear();
        if (lines.Count > 0)
        {
            var line = lines[0];
            DialogHosts.HelpWindow.HeaderMemo.Items.Add(line);
        }

        DialogHosts.HelpWindow.HelpMemo.Items.Clear();
        DialogHosts.HelpWindow.HelpMemo.Items.AddStrings(lines);
        if (DialogHosts.HelpWindow.HelpMemo.Items.Count > 0)
        {
            DialogHosts.HelpWindow.HelpMemo.Items.Delete(0);
        }

        if (DialogHosts.HelpWindow.HelpMemo.Items.Count > 0 && RtlUnit.IsEmpty(DialogHosts.HelpWindow.HelpMemo.Items[0]))
        {
            DialogHosts.HelpWindow.HelpMemo.Items.Delete(0);
        }

        DialogHosts.HelpWindow.MainWindow.Visible = true;
        lines?.Dispose();
    }

}
