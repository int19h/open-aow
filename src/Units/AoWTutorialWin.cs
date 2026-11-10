// Translated from AoWTutorialWin.pas.
using Aow.Game.Events;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AoWTutorialWin.pas.
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

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
