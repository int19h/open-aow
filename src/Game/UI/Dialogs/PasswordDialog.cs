// Translated from AoWPasswordDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

public partial class PasswordDialog : DataModule
{
    public PasswordDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Label TitleLabel;
    public Label PasswordLabel;
    public Edit PasswordEdit;
    private event ExecuteDoneEvent Done;
    private PasswordEventLog _passwordEvent;

    // AoWPasswordDlg.pas:43
    public void Finish()
    {
        ExecuteDoneEvent done;
        PasswordEventLog eventValue;
        Window.Visible = false;
        done = Done;
        eventValue = _passwordEvent;
        Done = null;
        _passwordEvent = null;
        if (done != null)
        {
            done(eventValue);
        }

        if (eventValue != null)
        {
            eventValue.Release();
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
    public void DlgKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        if (key == 27)
        {
            CancelBtnClick();
        }
    }

    internal static void ShowPasswordEvent(PasswordDialog dialog, PasswordEventLog eventValue, ExecuteDoneEvent done)
    {
        PasswordEventLog log;
        dialog.Reset();
        dialog.Done = done;
        dialog._passwordEvent = eventValue;
        dialog._passwordEvent.AddRef();
        log = dialog._passwordEvent;
        dialog.TitleLabel.Text = log.Title;
        dialog.PasswordLabel.Text = log.Prompt;
        dialog.PasswordEdit.Text = log.Password;
        dialog.PasswordEdit.Redraw = true;
        dialog.Window.Visible = true;
    }

}
