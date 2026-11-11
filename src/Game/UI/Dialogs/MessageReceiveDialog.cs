// Translated from AoWMsgReceiveDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;
using Aow.Rtl.Localize;

namespace Aow.Game.UI.Dialogs;

// done callback, retained event
public partial class MessageReceiveDialog : DataModule
{
    public MessageReceiveDialog(Component owner) : base(owner)
    { }

    public Window MsgReceiveWin;
    public Memo MessageMemo;
    public Button Close;
    public VerticalScrollBar MessageSB;
    public Image LShield;
    public Image RShield;
    public Memo HeaderMemo;
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

    internal static void ShowReceivedMessage(MessageReceiveDialog Dialog, EventLog Event, ExecuteDoneEvent Done)
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
