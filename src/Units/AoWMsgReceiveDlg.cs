// Translated from AoWMsgReceiveDlg.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWImage;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.EventLog;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AoWMsgReceiveDlg
{
    // done callback, retained event
    public partial class TMsgReceiveDlg : TDataModule
    {
        public TMsgReceiveDlg(TComponent owner) : base(owner)
        { }

        public TAOWWindow MsgReceiveWin;
        public TAOWMemo MessageMemo;
        public TAOWButton Close;
        public TAOWVScrollBar MessageSB;
        public TAOWImage LShield;
        public TAOWImage RShield;
        public TAOWMemo HeaderMemo;
        public TIvTranslator Translator;
        public TExecuteDoneEvent FDone;
        public TEventLog FEvent;
        // AoWMsgReceiveDlg.pas:54
        public void FinishDialog()
        {
            TExecuteDoneEvent Done = default;
            TEventLog Log = default;
            Done = FDone;
            Log = FEvent;
            FDone = null;
            FEvent = null;
            if (Done != null)
            {
                Done(Log);
            }

            if (Log != null)
            {
                Log.Release();
            }
        }

        // AoWMsgReceiveDlg.pas:40
        public void CloseClick()
        {
            General.PlayButtonSound(0);
            MsgReceiveWin.Visible = false;
        }

        // AoWMsgReceiveDlg.pas:47
        public void MsgReceiveWinHide()
        {
            FinishDialog();
        }
    }

    public static partial class AoWMsgReceiveDlg
    {
    }
}
