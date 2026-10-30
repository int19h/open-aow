// Translated from AOWChatWin.pas.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.Engine;
using static Aow.Units.Engine.Engine;
using Aow.Units.PlugInM;
using static Aow.Units.PlugInM.PlugInM;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.DisplayC;
using static Aow.Units.DisplayC.DisplayC;
using Aow.Units.Sound;
using static Aow.Units.Sound.Sound;
using Aow.Units.NetworkE;
using static Aow.Units.NetworkE.NetworkE;
using Aow.Units.aowFX;
using static Aow.Units.aowFX.aowFX;
using Aow.Units.HSEngine;
using static Aow.Units.HSEngine.HSEngine;
using Aow.Units.IsoHex;
using static Aow.Units.IsoHex.IsoHex;
using Aow.Units.ILTer;
using static Aow.Units.ILTer.ILTer;
using Aow.Units.Terrain;
using static Aow.Units.Terrain.Terrain;
using Aow.Units.Hexagon;
using static Aow.Units.Hexagon.Hexagon;
using Aow.Units.Road;
using static Aow.Units.Road.Road;
using Aow.Units.BorderHx;
using static Aow.Units.BorderHx.BorderHx;
using Aow.Units.AoWE;
using static Aow.Units.AoWE.AoWE;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AoWButton;
using static Aow.Units.AoWButton.AoWButton;
using Aow.Units.AoWEdit;
using static Aow.Units.AoWEdit.AoWEdit;
using Aow.Units.AoWImage;
using static Aow.Units.AoWImage.AoWImage;
using Aow.Units.AoWLabel;
using static Aow.Units.AoWLabel.AoWLabel;
using Aow.Units.AoWListBox;
using static Aow.Units.AoWListBox.AoWListBox;
using Aow.Units.AoWMemo;
using static Aow.Units.AoWMemo.AoWMemo;
using Aow.Units.AoWPanel;
using static Aow.Units.AoWPanel.AoWPanel;
using Aow.Units.AoWVScroll;
using static Aow.Units.AoWVScroll.AoWVScroll;
using Aow.Units.AoWWindow;
using static Aow.Units.AoWWindow.AoWWindow;
using Aow.Units.AoWWinMan;
using static Aow.Units.AoWWinMan.AoWWinMan;
using Aow.Units.aowSetupEvents;
using static Aow.Units.aowSetupEvents.aowSetupEvents;

namespace Aow.Units.AOWChatWin
{
    // pending scroll to the newest message
    public partial class TChatWindow : Aow.Rtl.TDataModule
    {
        public TChatWindow(TComponent owner) : base(owner)
        { }

        public Aow.Units.AoWWindow.TAOWWindow ChatWin;
        public Aow.Units.AoWMemo.TAOWMemo ChatMemo;
        public Aow.Units.AoWEdit.TAOWEdit ChatEdit;
        public Aow.Units.AoWListBox.TAOWListBox PlayerList;
        public Aow.Units.AoWImage.TAOWImage SIcon;
        public Aow.Units.AoWLabel.TAOWLabel IWTitle;
        public Aow.Units.AoWButton.TAOWButton Min;
        public Aow.Units.AoWPanel.TAOWPanel ChatPnl;
        public Aow.Units.AoWVScroll.TAOWVScrollBar NSBar;
        public Aow.Units.AoWVScroll.TAOWVScrollBar ChatSB;
        public Aow.Rtl.TIvTranslator Translator;
        public bool FScrollPending;
        // AOWChatWin.pas:47
        public void MinClick()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.PlayButtonSound(0);
            ChatWin.Visible = false;
        }

        // AOWChatWin.pas:53
        public void ChatEditReturn()
        {
            /* TODO: Implement aowDPlayComp.PlayerName in the platform or editor service. */
            throw new System.NotImplementedException();
        }

        // AOWChatWin.pas:67
        public void ChatWinResize()
        {
            var alignment = this.ChatWin.Alignment;
            alignment.LeftOffset = this.ChatWin.WinLeft;
            alignment.BottomOffset = (this.ChatWin.FWinManager.WinHeight - this.ChatWin.WinTop) - this.ChatWin.WinHeight;
            alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft;

            FScrollPending = true;
        }

        // AOWChatWin.pas:79
        public void ChatWinShow()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.RegisterDialog(ChatWin);
            FScrollPending = true;
        }

        // AOWChatWin.pas:85
        public void ChatWinHide()
        {
            Aow.Units.AoWWinMan.AoWWinMan.General.UnregisterDialog(ChatWin);
        }

        // AOWChatWin.pas:88
        public void ChatMemoUpdate()
        {
            if (FScrollPending)
            {
                this.ChatSB.Position = (this.ChatSB.Max + 1) - this.ChatSB.NumberVisible;

                FScrollPending = false;
            }
        }

        // AOWChatWin.pas:97
        public void PlayerListPreDrawItem(Aow.Rtl.TObject Sender, in Aow.Rtl.TRect DrawRect, int DrawItem, in Aow.Units.GFXE.TSurfaceDesc SD)
        {
            sbyte Player = default;
            if (DrawItem != 0)
            {
                var call__4 = PlayerList.Items.Objects[DrawItem];
                var call__5 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayerCommunicator.PCC.IndexOfSystemID(Aow.Rtl.Rtl.ObjectToInt(call__4));
                Player = unchecked((sbyte)call__5);
                // { Original leaves player zero without a race color. }
                bool bool__6 = (Player > 0);
                if (bool__6)
                {
                    var call__7 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.Count;
                    bool__6 = (Player < call__7);
                }

                if (bool__6)
                {
                    var call__8 = Aow.Units.AoWE.AoWE.AoWHSMap.FPlayers.GetPlayers(Player);
                    PlayerList.TextColor = Aow.Units.AoWE.AoWE.RaceColors[call__8.FPlayerRace];
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
