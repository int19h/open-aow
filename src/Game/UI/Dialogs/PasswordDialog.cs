#nullable disable
// Translated from AoWPasswordDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public sealed partial class PasswordDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Label TitleLabel { get; private set; }
    public Label PasswordLabel { get; private set; }
    public Edit PasswordEdit { get; private set; }

    private ExecuteDoneEvent _done;
    private PasswordEventLog _passwordEvent;

    // AoWPasswordDlg.pas:43
    public void Finish()
    {
        Window.Visible = false;
        ExecuteDoneEvent done = _done;
        PasswordEventLog eventValue = _passwordEvent;
        _done = null;
        _passwordEvent = null;
        done?.Invoke(eventValue);

        _ = (eventValue?.Release());
    }

    // AoWPasswordDlg.pas:42
    public void Reset()
    {
    }

    // AoWPasswordDlg.pas:55
    public void DlgHide() => Finish();

    // AoWPasswordDlg.pas:58
    public void OKBtnClick()
    {
        _passwordEvent.Password = PasswordEdit.Text;
        _passwordEvent.PressedButton = PressedEventLogButton.Ok;
        Window.Visible = false;
    }

    // AoWPasswordDlg.pas:65
    public void CancelBtnClick()
    {
        _passwordEvent.Password = "";
        _passwordEvent.PressedButton = PressedEventLogButton.Cancel;
        Window.Visible = false;
    }

    // AoWPasswordDlg.pas:72
    public void DlgKeyDown(in ushort key, ShiftStates shift)
    {
        if (key == 27)
        {
            CancelBtnClick();
        }
    }

    internal static void ShowPasswordEvent(PasswordDialog dialog, PasswordEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Reset();
        dialog._done = done;
        dialog._passwordEvent = eventValue;
        _ = dialog._passwordEvent.AddRef();
        PasswordEventLog log = dialog._passwordEvent;
        dialog.TitleLabel.Text = log.Title;
        dialog.PasswordLabel.Text = log.Prompt;
        dialog.PasswordEdit.Text = log.Password;
        dialog.PasswordEdit.Redraw = true;
        dialog.Window.Visible = true;
    }
}
