// Translated from AOWChatWin.pas.
using Aow.Game.Draw;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
// Translated from AOWChatWin.pas.
using Aow.Rtl.Localize;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Dialogs;

// pending scroll to the newest message
public partial class ChatWindow : DataModule
{
    public ChatWindow(Component owner) : base(owner)
    { }

    public AOWWindow ChatWin;
    public AOWMemo ChatMemo;
    public AOWEdit ChatEdit;
    public AOWListBox PlayerList;
    public AOWImage SIcon;
    public AOWLabel IWTitle;
    public AOWButton Min;
    public AOWPanel ChatPnl;
    public AOWVScrollBar NSBar;
    public AOWVScrollBar ChatSB;
    public IvTranslator Translator;
    public bool FScrollPending;

    // AOWChatWin.pas:47
    public void MinClick()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.PlayButtonSound(0);
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
        alignment.AlignWidth = AOWAlignWidth.Left;

        FScrollPending = true;
    }

    // AOWChatWin.pas:79
    public void ChatWinShow()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.RegisterDialog(ChatWin);
        FScrollPending = true;
    }

    // AOWChatWin.pas:85
    public void ChatWinHide()
    {
        Aow.Game.Ui.Dialogs.DialogHosts.General.UnregisterDialog(ChatWin);
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
    public void PlayerListPreDrawItem(in Rect DrawRect, int DrawItem, in SurfaceDesc SD)
    {
        sbyte Player;
        if (DrawItem != 0)
        {
            var objectValue = PlayerList.Items.Objects[DrawItem];
            Player = (sbyte)Aow.Game.World.Map.AoWHSMap.Current.FPlayerCommunicator.PCC.IndexOfSystemID(ObjectToInt(objectValue));
            // { Original leaves player zero without a race color. }
            if (Player > 0 && Player < World.Map.AoWHSMap.Current.FPlayers.Count)
            {
                var player = Aow.Game.World.Map.AoWHSMap.Current.FPlayers.GetPlayers(Player);
                PlayerList.TextColor = Magic.Abilities.AbilityRules.RaceColors[player.FPlayerRace];
            }
        }
        else
        {
            PlayerList.TextColor = 9415072;
        }
    }
}
