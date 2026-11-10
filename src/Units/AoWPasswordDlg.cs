// Translated from AoWPasswordDlg.pas.
using Aow.Game.Events;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWPasswordDlg.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

public partial class TPasswordDlg : TDataModule
{
    public TPasswordDlg(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWLabel TitleLbl;
    public TAoWFrame BCeltL;
    public TAoWFrame TCeltL;
    public TAOWImage TCeltLineL;
    public TAOWPanel OKCancelPnl;
    public TAOWButton OKBtn;
    public TAOWButton CancelBtn;
    public TAOWLabel PasswordLbl;
    public TAOWEdit PasswordEdit;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TPasswordEventLog FPasswordEvent;

    // AoWPasswordDlg.pas:43
    public void Finish()
    {
        TExecuteDoneEvent Done;
        TPasswordEventLog Event;
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
        FPasswordEvent.FPressedButton = TPressedEventLogButton.pebOk;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:65
    public void CancelBtnClick()
    {
        FPasswordEvent.FPassword = "";
        FPasswordEvent.FPressedButton = TPressedEventLogButton.pebCancel;
        Dlg.Visible = false;
    }

    // AoWPasswordDlg.pas:72
    public void DlgKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        if (Key == 27)
        {
            CancelBtnClick();
        }
    }
}

public static partial class AoWPasswordDlgUnit
{
}
