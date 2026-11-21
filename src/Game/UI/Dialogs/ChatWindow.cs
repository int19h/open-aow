#nullable disable warnings
// Translated from AOWChatWin.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.Session;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;

using static Aow.Rtl.IntegerObjects;

namespace Aow.Game.UI.Dialogs;

// pending scroll to the newest message
public sealed partial class ChatWindow(UIContext ui) : UIModule(ui)
{
    public Window MainWindow { get; private set; }
    public Memo ChatMemo { get; private set; }
    public Edit ChatEdit { get; private set; }
    public ListBox PlayerList { get; private set; }
    public VerticalScrollBar ChatScrollBar { get; private set; }

    public bool ScrollPending { get; set; }

    // AOWChatWin.pas:47
    public void MinClick()
    {
        UI.General.PlayButtonSound(0);
        MainWindow.Visible = false;
    }

    // AOWChatWin.pas:53
    public void ChatEditReturn()
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        => throw new NotImplementedException();

    // AOWChatWin.pas:67
    public void ChatWinResize()
    {
        Alignment alignment = MainWindow.Alignment;
        alignment.LeftOffset = MainWindow.WinLeft;
        alignment.BottomOffset = MainWindow.WindowManager.WinHeight - MainWindow.WinTop - MainWindow.WinHeight;
        alignment.AlignWidth = AlignWidth.Left;

        ScrollPending = true;
    }

    // AOWChatWin.pas:79
    public void ChatWinShow()
    {
        UI.General.RegisterDialog(MainWindow);
        ScrollPending = true;
    }

    // AOWChatWin.pas:85
    public void ChatWinHide() => UI.General.UnregisterDialog(MainWindow);

    // AOWChatWin.pas:88
    public void ChatMemoUpdate()
    {
        if (ScrollPending)
        {
            ChatScrollBar.Position = ChatScrollBar.Max + 1 - ChatScrollBar.NumberVisible;

            ScrollPending = false;
        }
    }

    // AOWChatWin.pas:97
    public void PlayerListPreDrawItem(in Rect drawRectangle, int drawItem, in LockedSurface lockedSurface)
    {
        if (drawItem != 0)
        {
            object? objectValue = PlayerList.Items.Objects[drawItem];
            sbyte player2 = (sbyte)StrategicMap.Current.PlayerCommunicator.PlayerCommunicationControl.IndexOfSystemId(ObjectToInt(objectValue));
            // { Original leaves player zero without a race color. }
            if (player2 > 0 && player2 < StrategicMap.Current.Players.Count)
            {
                Player? player = StrategicMap.Current.Players.GetPlayers(player2);
                PlayerList.TextColor = AbilityRules.RaceColors[player.PlayerRace];
            }
        }
        else
        {
            PlayerList.TextColor = 0x8F_A9_A0;
        }
    }
}
