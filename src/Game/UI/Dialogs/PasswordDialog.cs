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

    public Window Dlg;
    public Label TitleLbl;
    public Frame BCeltL;
    public Frame TCeltL;
    public Image TCeltLineL;
    public Panel OKCancelPnl;
    public Button OKBtn;
    public Button CancelBtn;
    public Label PasswordLbl;
    public Edit PasswordEdit;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public PasswordEventLog FPasswordEvent;

    // AoWPasswordDlg.pas:43
    public void Finish()
    {
        ExecuteDoneEvent Done;
        PasswordEventLog Event;
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
    public void OKBtnClick()
    {
        FPasswordEvent.FPassword = PasswordEdit.Text;
        FPasswordEvent.FPressedButton = PressedEventLogButton.Ok;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:65
    public void CancelBtnClick()
    {
        FPasswordEvent.FPassword = "";
        FPasswordEvent.FPressedButton = PressedEventLogButton.Cancel;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:72
    public void DlgKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        if (Key == 27)
        {
            CancelBtnClick();
        }
    }

    internal static void ShowPasswordEvent(PasswordDialog Dialog, PasswordEventLog Event, ExecuteDoneEvent Done)
    {
        PasswordEventLog Log;
        Dialog.Reset();
        Dialog.FDone = Done;
        Dialog.FPasswordEvent = Event;
        Dialog.FPasswordEvent.AddRef();
        Log = Dialog.FPasswordEvent;
        Dialog.TitleLbl.Text = Log.FTitle;
        Dialog.PasswordLbl.Text = Log.FPrompt;
        Dialog.PasswordEdit.Text = Log.FPassword;
        Dialog.PasswordEdit.FRedraw = true;
        Dialog.Dlg.Visible = true;
    }

}
