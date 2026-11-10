// Translated from AoWTutorialWin.pas.
using Aow.Game.Events;

using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// done callback, retained event
public partial class TutorialWindow : DataModule
{
    public TutorialWindow(Component owner) : base(owner)
    { }

    public AOWWindow Dlg;
    public AOWMemo MessageMemo;
    public AOWButton Close;
    public AOWMemo HeaderMemo;
    public AOWVScrollBar AbVSB;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public EventLog FEvent;

    // AoWTutorialWin.pas:51
    public void FinishDialog()
    {
        ExecuteDoneEvent Done;
        EventLog Log;
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

    internal static void ShowTutorialMessage(TutorialWindow Dialog)
    {
        if (Dialog.MessageMemo.Items.Count > 0)
        {
            Dialog.Dlg.Visible = true;
        }
    }

    internal static void ShowTutorialEvent(TutorialWindow Dialog, TutorialMessageEventLog Event, bool Unused, ExecuteDoneEvent Done)
    {
        Dialog.AbVSB.Position = 0;
        Dialog.FinishDialog();
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.FDone = Done;
        Dialog.HeaderMemo.Items.Clear();
        if (Event.FMessage.Count > 0)
        {
            var message = Event.FMessage[0];
            Dialog.HeaderMemo.Items.Add(message);
        }

        Dialog.MessageMemo.Items.Clear();
        Dialog.MessageMemo.Items.AddStrings(Event.FMessage);
        if (Dialog.MessageMemo.Items.Count > 0)
        {
            Dialog.MessageMemo.Items.Delete(0);
        }

        if (Dialog.MessageMemo.Items.Count > 0 && IsEmpty(Dialog.MessageMemo.Items[0]))
        {
            Dialog.MessageMemo.Items.Delete(0);
        }

        Dialog.Dlg.Visible = true;
    }

    internal static void ShowFlagMessage(TutorialWindow Dialog, FlagMessageEventLog Event, bool Unused, ExecuteDoneEvent Done)
    {
        Dialog.AbVSB.Position = 0;
        Dialog.FinishDialog();
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.FDone = Done;
        Dialog.HeaderMemo.Items.Clear();
        Dialog.HeaderMemo.Items.Add(Event.FHelpFileName);
        Dialog.MessageMemo.Items.Clear();
        Dialog.MessageMemo.Items.AddStrings(Event.FMessage);
        Dialog.Dlg.Visible = true;
    }

}
