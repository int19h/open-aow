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
    public ExecuteDoneEvent Done;
    public EventLog Event;

    // AoWMsgReceiveDlg.pas:54
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog log;
        done = Done;
        log = Event;
        Done = null;
        Event = null;
        if (done != null)
        {
            done(log);
        }

        if (log != null)
        {
            log.Release();
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

    internal static void ShowReceivedMessage(MessageReceiveDialog dialog, EventLog eventValue, ExecuteDoneEvent done)
    {
        dialog.Event = eventValue;
        dialog.Event.AddRef();
        dialog.Done = done;
        var text = eventValue.GetText();
        dialog.HeaderMemo.Items.Text = text;
        var fMessageText = ((GameOverEventLog)eventValue).Message.Text;
        dialog.MessageMemo.Items.Text = fMessageText;
        var player = AoWHSMap.Current.Players.GetPlayers(eventValue._player);
        dialog.LShield.CurrentImage = player.PlayerRace + 13;
        dialog.RShield.CurrentImage = dialog.LShield.CurrentImage;
        dialog.MsgReceiveWin.Visible = true;
    }

}
