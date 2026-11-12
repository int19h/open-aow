// Translated from AoWMsgReceiveDlg.pas.
using Aow.Game.Events;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

// done callback, retained event
public partial class MessageReceiveDialog : DataModule
{
    public MessageReceiveDialog(Component owner) : base(owner)
    { }

    public Window Window;
    public Memo MessageMemo;
    public Image LeftShield;
    public Image RightShield;
    public Memo HeaderMemo;
    private event ExecuteDoneEvent Done;
    private EventLog _event;

    // AoWMsgReceiveDlg.pas:54
    public void FinishDialog()
    {
        ExecuteDoneEvent done;
        EventLog log;
        done = Done;
        log = _event;
        Done = null;
        _event = null;
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
        Window.Visible = false;
    }

    // AoWMsgReceiveDlg.pas:47
    public void MsgReceiveWinHide()
    {
        FinishDialog();
    }

    internal static void ShowReceivedMessage(MessageReceiveDialog dialog, EventLog eventValue, ExecuteDoneEvent done)
    {
        dialog._event = eventValue;
        dialog._event.AddRef();
        dialog.Done = done;
        var text = eventValue.GetText();
        dialog.HeaderMemo.Items.Text = text;
        var fMessageText = ((GameOverEventLog)eventValue).Message.Text;
        dialog.MessageMemo.Items.Text = fMessageText;
        var player = StrategicMap.Current.Players.GetPlayers(eventValue._player);
        dialog.LeftShield.CurrentImage = player.PlayerRace + 13;
        dialog.RightShield.CurrentImage = dialog.LeftShield.CurrentImage;
        dialog.Window.Visible = true;
    }

}
