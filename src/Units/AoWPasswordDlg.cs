// Translated from AoWPasswordDlg.pas.
using Aow.Rtl;
using Aow.Units.EventLog;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWFrame;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWPanel;
using Aow.Units.AoWWindow;

namespace Aow.Units.AoWPasswordDlg
{
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
        public void OKBtnClick(TObject Sender)
        {
            FPasswordEvent.FPassword = PasswordEdit.Text;
            FPasswordEvent.FPressedButton = TPressedEventLogButton.pebOk;
            Dlg.Visible = false;
        }

        // AoWPasswordDlg.pas:65
        public void CancelBtnClick(TObject Sender)
        {
            FPasswordEvent.FPassword = (AnsiString)"";
            FPasswordEvent.FPressedButton = TPressedEventLogButton.pebCancel;
            Dlg.Visible = false;
        }

        // AoWPasswordDlg.pas:72
        public void DlgKeyDown(TObject Sender, ref ushort Key, TSet<TShiftState> Shift)
        {
            if (Key == 27)
            {
                CancelBtnClick(null);
            }
        }
    }

    public static partial class AoWPasswordDlg
    {
    }
}
