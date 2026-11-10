// Translated from AoWMsgReceiveDlg.pas.
using Aow.Game.Events;

using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;

using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.Ui.Dialogs;

// done callback, retained event
public partial class MsgReceiveDlg : DataModule
{
    public MsgReceiveDlg(Component owner) : base(owner)
    { }

    public AOWWindow MsgReceiveWin;
    public AOWMemo MessageMemo;
    public AOWButton Close;
    public AOWVScrollBar MessageSB;
    public AOWImage LShield;
    public AOWImage RShield;
    public AOWMemo HeaderMemo;
    public IvTranslator Translator;
    public ExecuteDoneEvent FDone;
    public EventLog FEvent;

    // AoWMsgReceiveDlg.pas:54
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

    // AoWMsgReceiveDlg.pas:40
    public void CloseClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        MsgReceiveWin.Visible = false;
    }

    // AoWMsgReceiveDlg.pas:47
    public void MsgReceiveWinHide()
    {
        FinishDialog();
    }

    internal static void ShowReceivedMessage(MsgReceiveDlg Dialog, EventLog Event, ExecuteDoneEvent Done)
    {
        Dialog.FEvent = Event;
        Dialog.FEvent.AddRef();
        Dialog.FDone = Done;
        var text = Event.GetText();
        Dialog.HeaderMemo.Items.Text = text;
        var fMessageText = ((GameOverEventLog)Event).FMessage.Text;
        Dialog.MessageMemo.Items.Text = fMessageText;
        var player = AoWHSMap.Current.FPlayers.GetPlayers(Event.FPlayer);
        Dialog.LShield.CurrentImage = player.FPlayerRace + 13;
        Dialog.RShield.CurrentImage = Dialog.LShield.CurrentImage;
        Dialog.MsgReceiveWin.Visible = true;
    }

}
