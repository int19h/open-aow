// Translated from AoWTutorialWin.pas.
using Aow.Rtl;
using Aow.Units.AoWButton;
using Aow.Units.AoWMemo;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using Aow.Units.EventLog;

namespace Aow.Units.AoWTutorialWin;

// done callback, retained event
public partial class TTutorialWindow : TDataModule
{
    public TTutorialWindow(TComponent owner) : base(owner)
    { }

    public TAOWWindow Dlg;
    public TAOWMemo MessageMemo;
    public TAOWButton Close;
    public TAOWMemo HeaderMemo;
    public TAOWVScrollBar AbVSB;
    public TIvTranslator Translator;
    public TExecuteDoneEvent FDone;
    public TEventLog FEvent;

    // AoWTutorialWin.pas:51
    public void FinishDialog()
    {
        TExecuteDoneEvent Done;
        TEventLog Log;
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

    // AoWTutorialWin.pas:38
    public void DlgHide()
    {
        FinishDialog();
    }

    // AoWTutorialWin.pas:44
    public void CloseClick()
    {
        Dlg.Visible = false;
    }
}

public static partial class AoWTutorialWinUnit
{
}
