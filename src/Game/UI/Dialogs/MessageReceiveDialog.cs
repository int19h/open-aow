#nullable disable
// Translated from AoWMsgReceiveDlg.pas.
using Aow.Game.Events;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.UI.Dialogs;

// done callback, retained event
public partial class MessageReceiveDialog(UIContext ui) : UIModule(ui)
{
    public Window Window { get; private set; }
    public Memo MessageMemo { get; private set; }
    public Image LeftShield { get; private set; }
    public Image RightShield { get; private set; }
    public Memo HeaderMemo { get; private set; }

    private event ExecuteDoneEvent Done;
    private EventLog _event;

    // AoWMsgReceiveDlg.pas:54
    public void FinishDialog()
    {
        ExecuteDoneEvent done = Done;
        EventLog log = _event;
        Done = null;
        _event = null;
        done?.Invoke(log);

        _ = (log?.Release());
    }

    // AoWMsgReceiveDlg.pas:40
    public void CloseClick()
    {
        UI.General.PlayButtonSound(0);
        Window.Visible = false;
    }

    // AoWMsgReceiveDlg.pas:47
    public void MsgReceiveWinHide() => FinishDialog();

    internal static void ShowReceivedMessage(MessageReceiveDialog dialog, GameOverEventLog eventValue, ExecuteDoneEvent done)
    {
        dialog._event = eventValue;
        _ = dialog._event.AddRef();
        dialog.Done = done;
        string text = eventValue.GetText();
        dialog.HeaderMemo.Items.Text = text;
        string fMessageText = eventValue.Message.Text;
        dialog.MessageMemo.Items.Text = fMessageText;
        Player player = StrategicMap.Current.Players.GetPlayers(eventValue.Player);
        dialog.LeftShield.CurrentImage = player.PlayerRace + 13;
        dialog.RightShield.CurrentImage = dialog.LeftShield.CurrentImage;
        dialog.Window.Visible = true;
    }

}
