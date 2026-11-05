// Translated from AOWChatWin.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using Aow.Units.AoWButton;
using Aow.Units.AoWEdit;
using Aow.Units.AoWImage;
using Aow.Units.AoWLabel;
using Aow.Units.AoWListBox;
using Aow.Units.AoWMemo;
using Aow.Units.AoWPanel;
using Aow.Units.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWinMan.AoWWinMan;

namespace Aow.Units.AOWChatWin
{
    // pending scroll to the newest message
    public partial class TChatWindow : TDataModule
    {
        public TChatWindow(TComponent owner) : base(owner)
        { }

        public TAOWWindow ChatWin;
        public TAOWMemo ChatMemo;
        public TAOWEdit ChatEdit;
        public TAOWListBox PlayerList;
        public TAOWImage SIcon;
        public TAOWLabel IWTitle;
        public TAOWButton Min;
        public TAOWPanel ChatPnl;
        public TAOWVScrollBar NSBar;
        public TAOWVScrollBar ChatSB;
        public TIvTranslator Translator;
        public bool FScrollPending;
        // AOWChatWin.pas:47
        public void MinClick()
        {
            General.PlayButtonSound(0);
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
            alignment.AlignWidth = TAOWAlignWidth.awLeft;

            FScrollPending = true;
        }

        // AOWChatWin.pas:79
        public void ChatWinShow()
        {
            General.RegisterDialog(ChatWin);
            FScrollPending = true;
        }

        // AOWChatWin.pas:85
        public void ChatWinHide()
        {
            General.UnregisterDialog(ChatWin);
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
        public void PlayerListPreDrawItem(TObject Sender, in TRect DrawRect, int DrawItem, in TSurfaceDesc SD)
        {
            sbyte Player;
            if (DrawItem != 0)
            {
                var objectValue = PlayerList.Items.Objects[DrawItem];
                Player = (sbyte)AoWHSMap.FPlayerCommunicator.PCC.IndexOfSystemID(ObjectToInt(objectValue));
                // { Original leaves player zero without a race color. }
                if (Player > 0 && Player < AoWHSMap.FPlayers.Count)
                {
                    var player = AoWHSMap.FPlayers.GetPlayers(Player);
                    PlayerList.TextColor = RaceColors[player.FPlayerRace];
                }
            }
            else
            {
                PlayerList.TextColor = 9415072;
            }
        }
    }

    public static partial class AOWChatWin
    {
    }
}
