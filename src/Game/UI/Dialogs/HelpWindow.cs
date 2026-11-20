#nullable disable
// Translated from aowHelpWin.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// done callback, help event
public partial class HelpWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public VerticalScrollBar HelpScrollBar { get; private set; }
    public Memo HeaderMemo { get; private set; }
    public Memo HelpMemo { get; private set; }

    private event ExecuteDoneEvent Done;
    private HelpMessageEventLog _helpEvent;

    // aowHelpWin.pas:53
    public void FinishDialog()
    {
        ExecuteDoneEvent done = Done;
        HelpMessageEventLog eventValue = _helpEvent;
        Done = null;
        _helpEvent = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // aowHelpWin.pas:66
    public void ShowHelpEvent(HelpMessageEventLog eventValue, bool unused, ExecuteDoneEvent done)
    {
        FinishDialog();
        _helpEvent = eventValue;
        _ = _helpEvent.AddRef();
        HeaderMemo.Items.Clear();
        if (eventValue.Message.Count > 0)
        {
            string message = eventValue.Message[0];
            _ = HeaderMemo.Items.Add(message);
        }

        HelpMemo.Items.Clear();
        HelpMemo.Items.AddStrings(eventValue.Message);
        if (HelpMemo.Items.Count > 0)
        {
            HelpMemo.Items.Delete(0);
        }

        if (HelpMemo.Items.Count > 0 && DelphiStrings.IsEmpty(HelpMemo.Items[0]))
        {
            HelpMemo.Items.Delete(0);
        }

        if (eventValue.Mode != 0)
        {
            Done = done;
        }
        else
        {
            done?.Invoke(_helpEvent);
        }

        MainWindow.Visible = true;
    }

    // aowHelpWin.pas:100
    public void CloseBtnClick() => MainWindow.Visible = false;

    // aowHelpWin.pas:105
    public void HelpWinHide() => FinishDialog();

    // aowHelpWin.pas:110
    public void HelpWinResize()
    {
        MainWindow.Alignment.LeftOffset = MainWindow.WinLeft;
        MainWindow.Alignment.TopOffset = MainWindow.WinTop;
        MainWindow.Alignment.AlignHeight = AlignHeight.Top;
        MainWindow.Alignment.AlignWidth = AlignWidth.Left;
    }

    internal void ShowHelp(int helpId)
    {
        if (UI.HelpWindow.MainWindow.Visible)
        {
            UI.HelpWindow.FinishDialog();
        }

        DelphiStringList lines = new();
        GamePack.Current.TextControl.RetrieveText(helpId, lines);
        UI.HelpWindow.HelpScrollBar.Position = 0;
        UI.HelpWindow.HeaderMemo.Items.Clear();
        if (lines.Count > 0)
        {
            string line = lines[0];
            _ = UI.HelpWindow.HeaderMemo.Items.Add(line);
        }

        UI.HelpWindow.HelpMemo.Items.Clear();
        UI.HelpWindow.HelpMemo.Items.AddStrings(lines);
        if (UI.HelpWindow.HelpMemo.Items.Count > 0)
        {
            UI.HelpWindow.HelpMemo.Items.Delete(0);
        }

        if (UI.HelpWindow.HelpMemo.Items.Count > 0 && DelphiStrings.IsEmpty(UI.HelpWindow.HelpMemo.Items[0]))
        {
            UI.HelpWindow.HelpMemo.Items.Delete(0);
        }

        UI.HelpWindow.MainWindow.Visible = true;
        lines?.Dispose();
    }

}
