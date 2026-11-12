// Translated from AOWChatWin.pas.
using Aow.Game.Draw;
using Aow.Game.Magic.Abilities;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;
using Aow.Rtl;
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Dialogs;

// pending scroll to the newest message
public partial class ChatWindow : DataModule
{
    public ChatWindow(Component owner) : base(owner)
    { }

    public Window ChatWin;
    public Memo ChatMemo;
    public Edit ChatEdit;
    public ListBox PlayerList;
    public VerticalScrollBar ChatSB;

    public bool ScrollPending { get; set; }

    // AOWChatWin.pas:47
    public void MinClick()
    {
        DialogHosts.General.PlayButtonSound(0);
        ChatWin.Visible = false;
    }

    // AOWChatWin.pas:53
    public void ChatEditReturn()
    {
        /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
        throw new NotImplementedException();
    }

    // AOWChatWin.pas:67
    public void ChatWinResize()
    {
        var alignment = this.ChatWin.Alignment;
        alignment.LeftOffset = this.ChatWin.WinLeft;
        alignment.BottomOffset = this.ChatWin._windowManager.WinHeight - this.ChatWin.WinTop - this.ChatWin.WinHeight;
        alignment.AlignWidth = AlignWidth.Left;

        ScrollPending = true;
    }

    // AOWChatWin.pas:79
    public void ChatWinShow()
    {
        DialogHosts.General.RegisterDialog(ChatWin);
        ScrollPending = true;
    }

    // AOWChatWin.pas:85
    public void ChatWinHide()
    {
        DialogHosts.General.UnregisterDialog(ChatWin);
    }

    // AOWChatWin.pas:88
    public void ChatMemoUpdate()
    {
        if (ScrollPending)
        {
            this.ChatSB.Position = this.ChatSB.Max + 1 - this.ChatSB.NumberVisible;

            ScrollPending = false;
        }
    }

    // AOWChatWin.pas:97
    public void PlayerListPreDrawItem(in Rect drawRectangle, int drawItem, in LockedSurface lockedSurface)
    {
        sbyte Player;
        if (drawItem != 0)
        {
            var objectValue = PlayerList.Items.Objects[drawItem];
            Player = (sbyte)AoWHSMap.Current.PlayerCommunicator.PlayerCommunicationControl.IndexOfSystemId(ObjectToInt(objectValue));
            // { Original leaves player zero without a race color. }
            if (Player > 0 && Player < AoWHSMap.Current.Players.Count)
            {
                var player = AoWHSMap.Current.Players.GetPlayers(Player);
                PlayerList.TextColor = AbilityRules.RaceColors[player.PlayerRace];
            }
        }
        else
        {
            PlayerList.TextColor = 9415072;
        }
    }
}
