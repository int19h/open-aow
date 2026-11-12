using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// The debug message window of the DWin form (AOWDialogWin.pas): the frame rate and the last 100
// debug messages of the window manager. The form lays it out beside the confirmation dialog, and
// the layout hands its components and its UI context to this type.
public sealed class DebugWindow(UIContext ui, Window window, Label frameRateLabel, Memo memo, VerticalScrollBar memoScrollBar)
{
    public void Show() => window.Visible = true;

    public void ShowFrameRate(int frameRate) => frameRateLabel.Text = RtlUnit.IntToStr(frameRate);

    // AOWDialogWin.pas:113
    public void AddMessage(string text)
    {
        memo.Items.Add(text);
        if (memo.Items.Count > 99)
        {
            memo.Items.Delete(0);
        }

        memo.Sized = false;
        memoScrollBar.Position = memoScrollBar.Max;
    }

    // AOWDialogWin.pas:67, the Close button.
    public void Close()
    {
        ui.General.PlayButtonSound(0);
        window.Visible = false;
    }
}
