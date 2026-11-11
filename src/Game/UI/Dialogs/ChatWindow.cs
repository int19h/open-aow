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
    public Image SIcon;
    public Label IWTitle;
    public Button Min;
    public Panel ChatPnl;
    public VerticalScrollBar NSBar;
    public VerticalScrollBar ChatSB;
    public IvTranslator Translator;
    public bool FScrollPending;

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
        alignment.BottomOffset = this.ChatWin.FWinManager.WinHeight - this.ChatWin.WinTop - this.ChatWin.WinHeight;
        alignment.AlignWidth = AlignWidth.Left;

        FScrollPending = true;
    }

    // AOWChatWin.pas:79
    public void ChatWinShow()
    {
        DialogHosts.General.RegisterDialog(ChatWin);
        FScrollPending = true;
    }

    // AOWChatWin.pas:85
    public void ChatWinHide()
    {
        DialogHosts.General.UnregisterDialog(ChatWin);
    }

    // AOWChatWin.pas:88
    public void ChatMemoUpdate()
    {
        if (FScrollPending)
        {
            this.ChatSB.Position = this.ChatSB.Max + 1 - this.ChatSB.NumberVisible;

            FScrollPending = false;
        }
    }

    // AOWChatWin.pas:97
    public void PlayerListPreDrawItem(in Rect DrawRect, int DrawItem, in LockedSurface SD)
    {
        sbyte Player;
        if (DrawItem != 0)
        {
            var objectValue = PlayerList.Items.Objects[DrawItem];
            Player = (sbyte)AoWHSMap.Current.FPlayerCommunicator.PCC.IndexOfSystemID(ObjectToInt(objectValue));
            // { Original leaves player zero without a race color. }
            if (Player > 0 && Player < AoWHSMap.Current.FPlayers.Count)
            {
                var player = AoWHSMap.Current.FPlayers.GetPlayers(Player);
                PlayerList.TextColor = AbilityRules.RaceColors[player.FPlayerRace];
            }
        }
        else
        {
            PlayerList.TextColor = 9415072;
        }
    }
}
