// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWMultiWin
{
    public partial class TMultiWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MultiWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            JoinPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "JoinPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(JoinPnl);
            GamesLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "GamesLB",
                Priority = 200,
                WinLeft = 9,
                WinTop = 40,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 130,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 100
                },
                Masked = false,
                WinWidth = 140,
                WinHeight = 146,
                BorderWidth = 8,
                BorderHeight = 5,
                OnChange = Layout_GamesLBChange_0,
                OnDoubleClick = Layout_JoinBtnClick_1,
                Selected = -1,
                TextColor = 12632256,
                TextSelColor = 16777215,
                ILIndexFrame = 40,
                ILIndexTNormal = 4,
                ILIndexTSelected = 16,
                ItemHeight = 20,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(GamesLB);
            GLBvsc = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "GLBvsc",
                Visible = false,
                Priority = 200,
                WinLeft = 132,
                WinTop = 40,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 5,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 146,
                WinWidth = 17,
                NumberVisible = 5,
                VanishWhenFull = true,
                ILIndexIncUp = 10,
                ILIndexIncDown = 11,
                ILIndexDecUp = 8,
                ILIndexDecDown = 9,
                ILIndexBack = 15,
                ILIndexTab = 12
            };
            LayoutSession.EndRead(GLBvsc);
            JoinBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "JoinBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 206,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Joins the selected game."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 206,
                    BottomOffset = 60
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Join Game",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_JoinBtnClick_1
            };
            LayoutSession.EndRead(JoinBtn);
            MultiPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MultiPnl",
                OnUpdate = Layout_MultiWinUpdate_2,
                Visible = false,
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(MultiPnl);
            BackBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BackBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 246,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 246,
                    BottomOffset = 230
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Cancel",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_BackBtnClick_3
            };
            LayoutSession.EndRead(BackBtn);
            GamePanel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "GamePanel",
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(GamePanel);
            TCPBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TCPBtn",
                Priority = 19,
                WinLeft = 0,
                WinTop = 86,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play a game using the TCP (Internet) protocol."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 86,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "TCP Internet",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_TCPBtnClick_4
            };
            LayoutSession.EndRead(TCPBtn);
            IPXBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "IPXBtn",
                Priority = 19,
                WinLeft = 0,
                WinTop = 126,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play a game using the IPX (LAN) protocol."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "IPX LAN",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_IPXBtnClick_5
            };
            LayoutSession.EndRead(IPXBtn);
            SingleBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SingleBtn",
                Priority = 19,
                WinLeft = 0,
                WinTop = 46,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play a game on only this computer."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 46,
                    BottomOffset = 410
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Single Computer",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_SingleBtnClick_6
            };
            LayoutSession.EndRead(SingleBtn);
            EmailBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "EmailBtn",
                Priority = 19,
                WinLeft = 0,
                WinTop = 166,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Play a game through email."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 166,
                    BottomOffset = 210
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Play by Email",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_EmailBtnClick_7
            };
            LayoutSession.EndRead(EmailBtn);
            TCPPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TCPPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TCPPnl);
            TCPNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TCPNameLbl",
                Priority = 100,
                WinLeft = 52,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Player Name",
                TextColor = 8472387
            };
            LayoutSession.EndRead(TCPNameLbl);
            Single = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Single",
                Priority = 100,
                WinLeft = 35,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "TCP Internet Game",
                TextColor = 8472387
            };
            LayoutSession.EndRead(Single);
            Address = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Address",
                Priority = 100,
                WinLeft = 53,
                WinTop = 150,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The IP Address of the host computer."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 150,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "IP Address",
                TextColor = 8472387
            };
            LayoutSession.EndRead(Address);
            CancelTCP = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelTCP",
                Priority = 100,
                WinLeft = 0,
                WinTop = 246,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 246,
                    BottomOffset = 230
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Cancel",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CancelTCPClick_8
            };
            LayoutSession.EndRead(CancelTCP);
            Host = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Host",
                Priority = 100,
                WinLeft = 0,
                WinTop = 106,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Starts a new TCP/IP game."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 106,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Host New",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = HostClick
            };
            LayoutSession.EndRead(Host);
            Find = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Find",
                Priority = 100,
                WinLeft = 0,
                WinTop = 206,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Searches for games at the given IP Address."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 206,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Find Games",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_FindClick_9
            };
            LayoutSession.EndRead(Find);
            GameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GameLbl",
                Priority = 19,
                WinLeft = 55,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Game Type",
                TextColor = 8472387
            };
            LayoutSession.EndRead(GameLbl);
            TCPName = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "TCPName",
                Priority = 0,
                WinLeft = 0,
                WinTop = 66,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 66,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 158,
                Text = "Name",
                BorderWidth = 15,
                BorderHeight = 7,
                OnChange = Layout_TCPNameChange_10,
                MaxChars = 15,
                CursorChar = (byte)95,
                BlinkDelay = 0,
                TextColor = 16777215,
                Editable = true,
                ILIndex = 29,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(TCPName);
            IPEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "IPEdit",
                Priority = 0,
                WinLeft = 0,
                WinTop = 166,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The IP Address of the host computer."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 166,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 158,
                BorderWidth = 15,
                BorderHeight = 7,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 0,
                TextColor = 16777215,
                Editable = true,
                ILIndex = 29,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(IPEdit);
            LanPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LanPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LanPnl);
            LANFind = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LANFind",
                Priority = 100,
                WinLeft = 0,
                WinTop = 146,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Searches for IPX games on the LAN."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 146,
                    BottomOffset = 290
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Find Games",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_LANFindClick_11
            };
            LayoutSession.EndRead(LANFind);
            JoinLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "JoinLbl",
                Priority = 100,
                WinLeft = 43,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Available Games",
                TextColor = 8472387
            };
            LayoutSession.EndRead(JoinLbl);
            BottomPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "BottomPnl",
                Priority = 1000,
                WinLeft = 241,
                WinTop = 380,
                WinWidth = 158,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 0,
                ILIndexTexture = 3,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(BottomPnl);
            BackButn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BackButn",
                Priority = 19,
                WinLeft = 0,
                WinTop = 22,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Back to Title",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CloseBtnClick_12
            };
            LayoutSession.EndRead(BackButn);
            MsgPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MsgPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MsgPnl);
            CancelOKBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelOKBut",
                Priority = 100,
                WinLeft = 0,
                WinTop = 246,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 246,
                    BottomOffset = 230
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Cancel",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CancelOKButClick_13
            };
            LayoutSession.EndRead(CancelOKBut);
            MessageMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MessageMemo",
                Priority = 0,
                WinLeft = 6,
                WinTop = 130,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 130,
                    BottomOffset = 50
                },
                Masked = false,
                WinWidth = 146,
                WinHeight = 185,
                LayoutItems = new LayoutStrings()
                {
                    "Connecting...."
                },
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 30,
                ILIndexTexture = 14,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(MessageMemo);
            LANCancel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LANCancel",
                Priority = 100,
                WinLeft = 0,
                WinTop = 246,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 246,
                    BottomOffset = 230
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Cancel",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CancelTCPClick_8
            };
            LayoutSession.EndRead(LANCancel);
            AOWLabel1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AOWLabel1",
                Priority = 100,
                WinLeft = 52,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Player Name",
                TextColor = 8472387
            };
            LayoutSession.EndRead(AOWLabel1);
            LANLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LANLbl",
                Priority = 100,
                WinLeft = 45,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "IPX LAN Game",
                TextColor = 8472387
            };
            LayoutSession.EndRead(LANLbl);
            IPXName = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "IPXName",
                Priority = 0,
                WinLeft = 0,
                WinTop = 66,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 66,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 158,
                Text = "Name",
                BorderWidth = 15,
                BorderHeight = 7,
                OnChange = Layout_IPXNameChange_14,
                MaxChars = 15,
                CursorChar = (byte)95,
                BlinkDelay = 0,
                TextColor = 16777215,
                Editable = true,
                ILIndex = 29,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(IPXName);
            LANHost = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LANHost",
                Priority = 100,
                WinLeft = 0,
                WinTop = 106,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Starts a new LAN/IPX game."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 106,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Host New",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = LANHostClick
            };
            LayoutSession.EndRead(LANHost);
            PBEMPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PBEMPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 241,
                WinTop = 0,
                WinWidth = 158,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 286,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PBEMPnl);
            CancelPBEMBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelPBEMBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 246,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 246,
                    BottomOffset = 230
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Cancel",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CancelPBEMBtnClick_15
            };
            LayoutSession.EndRead(CancelPBEMBtn);
            CheckEmailBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CheckEmailBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 106,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 106,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Check Email",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_CheckEmailBtnClick_16
            };
            LayoutSession.EndRead(CheckEmailBtn);
            NewPBEMGameBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NewPBEMGameBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 186,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 186,
                    BottomOffset = 290
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "New Game",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_NewPBEMGameBtnClick_17
            };
            LayoutSession.EndRead(NewPBEMGameBtn);
            LoadPBEMGameBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LoadPBEMGameBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 146,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 146,
                    BottomOffset = 290
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Load Game",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_LoadPBEMGameBtnClick_18
            };
            LayoutSession.EndRead(LoadPBEMGameBtn);
            EmailSettingsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "EmailSettingsBtn",
                Priority = 100,
                WinLeft = 0,
                WinTop = 66,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 66,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 21,
                ILIndexLit = 22,
                ILIndexUp = 20,
                Text = "Settings",
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 158,
                OnClick = Layout_EmailSettingsBtnClick_19
            };
            LayoutSession.EndRead(EmailSettingsBtn);
            HeatNetBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HeatNetBtn",
                Priority = 19,
                WinLeft = 46,
                WinTop = 205,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Start a HeatNet game."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 205,
                    BottomOffset = 210
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 72,
                ILIndexLit = 71,
                ILIndexUp = 70,
                TextColor = 5046289,
                TextSelColor = 9505286,
                WinWidth = 66,
                OnClick = Layout_HeatNetBtnClick_20
            };
            LayoutSession.EndRead(HeatNetBtn);
            Translator = new Aow.Layout.LayoutTIvTranslator(this)
            {
                Name = "Translator",
                DictionaryName = "AoW",
                TargetsData =
                {
                    new TranslationTarget("", "Items", 0),
                    new TranslationTarget("", "Text", 0),
                    new TranslationTarget("", "ToolTipText", 0)
                }
            };
            LayoutSession.EndRead(Translator);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            JoinPnl.AOWWindow = MultiPnl;
            GamesLB.AOWWindow = JoinPnl;
            GamesLB.VScrollBar = GLBvsc;
            GLBvsc.AOWWindow = JoinPnl;
            JoinBtn.AOWWindow = JoinPnl;
            BackBtn.AOWWindow = JoinPnl;
            GamePanel.AOWWindow = MultiPnl;
            TCPBtn.AOWWindow = GamePanel;
            IPXBtn.AOWWindow = GamePanel;
            SingleBtn.AOWWindow = GamePanel;
            EmailBtn.AOWWindow = GamePanel;
            TCPPnl.AOWWindow = MultiPnl;
            TCPNameLbl.AOWWindow = TCPPnl;
            Single.AOWWindow = TCPPnl;
            Address.AOWWindow = TCPPnl;
            CancelTCP.AOWWindow = TCPPnl;
            Host.AOWWindow = TCPPnl;
            Find.AOWWindow = TCPPnl;
            GameLbl.AOWWindow = GamePanel;
            TCPName.AOWWindow = TCPPnl;
            IPEdit.AOWWindow = TCPPnl;
            LanPnl.AOWWindow = MultiPnl;
            LANFind.AOWWindow = LanPnl;
            JoinLbl.AOWWindow = JoinPnl;
            BottomPnl.AOWWindow = MultiPnl;
            BackButn.AOWWindow = BottomPnl;
            MsgPnl.AOWWindow = MultiPnl;
            CancelOKBut.AOWWindow = MsgPnl;
            MessageMemo.AOWWindow = MsgPnl;
            LANCancel.AOWWindow = LanPnl;
            AOWLabel1.AOWWindow = LanPnl;
            LANLbl.AOWWindow = LanPnl;
            IPXName.AOWWindow = LanPnl;
            LANHost.AOWWindow = LanPnl;
            PBEMPnl.AOWWindow = MultiPnl;
            CancelPBEMBtn.AOWWindow = PBEMPnl;
            CheckEmailBtn.AOWWindow = PBEMPnl;
            NewPBEMGameBtn.AOWWindow = PBEMPnl;
            LoadPBEMGameBtn.AOWWindow = PBEMPnl;
            EmailSettingsBtn.AOWWindow = PBEMPnl;
            HeatNetBtn.AOWWindow = GamePanel;
            Loaded();
            JoinPnl.Loaded();
            GamesLB.Loaded();
            GLBvsc.Loaded();
            JoinBtn.Loaded();
            MultiPnl.Loaded();
            BackBtn.Loaded();
            GamePanel.Loaded();
            TCPBtn.Loaded();
            IPXBtn.Loaded();
            SingleBtn.Loaded();
            EmailBtn.Loaded();
            TCPPnl.Loaded();
            TCPNameLbl.Loaded();
            Single.Loaded();
            Address.Loaded();
            CancelTCP.Loaded();
            Host.Loaded();
            Find.Loaded();
            GameLbl.Loaded();
            TCPName.Loaded();
            IPEdit.Loaded();
            LanPnl.Loaded();
            LANFind.Loaded();
            JoinLbl.Loaded();
            BottomPnl.Loaded();
            BackButn.Loaded();
            MsgPnl.Loaded();
            CancelOKBut.Loaded();
            MessageMemo.Loaded();
            LANCancel.Loaded();
            AOWLabel1.Loaded();
            LANLbl.Loaded();
            IPXName.Loaded();
            LANHost.Loaded();
            PBEMPnl.Loaded();
            CancelPBEMBtn.Loaded();
            CheckEmailBtn.Loaded();
            NewPBEMGameBtn.Loaded();
            LoadPBEMGameBtn.Loaded();
            EmailSettingsBtn.Loaded();
            HeatNetBtn.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            JoinPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            GamesLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            GamesLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GamesLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            GamesLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            GLBvsc.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GLBvsc.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GLBvsc.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GLBvsc.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            JoinBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            JoinBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            JoinBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MultiPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            MultiPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            BackBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            BackBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            GamePanel.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            TCPBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCPBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            TCPBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            IPXBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IPXBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            IPXBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SingleBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SingleBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SingleBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            EmailBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EmailBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            EmailBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            TCPPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            TCPNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Single.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Address.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Address.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            CancelTCP.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelTCP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CancelTCP.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            Host.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Host.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Host.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            Find.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Find.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Find.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            GameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            TCPName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TCPName.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            IPEdit.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IPEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IPEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LanPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LANFind.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LANFind.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LANFind.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            JoinLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            BottomPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            BackButn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            BackButn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MsgPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            CancelOKBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CancelOKBut.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MessageMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            MessageMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MessageMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LANCancel.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LANCancel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LANCancel.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AOWLabel1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            LANLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            IPXName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IPXName.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LANHost.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LANHost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LANHost.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PBEMPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            CancelPBEMBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CancelPBEMBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            CheckEmailBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CheckEmailBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CheckEmailBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            NewPBEMGameBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NewPBEMGameBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            NewPBEMGameBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LoadPBEMGameBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LoadPBEMGameBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LoadPBEMGameBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            EmailSettingsBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EmailSettingsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            EmailSettingsBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            HeatNetBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            HeatNetBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            HeatNetBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
        }

        private void Layout_GamesLBChange_0(TObject p0)
        {
            GamesLBChange();
        }

        private void Layout_JoinBtnClick_1(TObject p0)
        {
            JoinBtnClick();
        }

        private void Layout_MultiWinUpdate_2(TObject p0)
        {
            MultiWinUpdate();
        }

        private void Layout_BackBtnClick_3(TObject p0)
        {
            BackBtnClick();
        }

        private void Layout_TCPBtnClick_4(TObject p0)
        {
            TCPBtnClick();
        }

        private void Layout_IPXBtnClick_5(TObject p0)
        {
            IPXBtnClick();
        }

        private void Layout_SingleBtnClick_6(TObject p0)
        {
            SingleBtnClick();
        }

        private void Layout_EmailBtnClick_7(TObject p0)
        {
            EmailBtnClick();
        }

        private void Layout_CancelTCPClick_8(TObject p0)
        {
            CancelTCPClick();
        }

        private void Layout_FindClick_9(TObject p0)
        {
            FindClick();
        }

        private void Layout_TCPNameChange_10(TObject p0)
        {
            TCPNameChange();
        }

        private void Layout_LANFindClick_11(TObject p0)
        {
            LANFindClick();
        }

        private void Layout_CloseBtnClick_12(TObject p0)
        {
            CloseBtnClick();
        }

        private void Layout_CancelOKButClick_13(TObject p0)
        {
            CancelOKButClick();
        }

        private void Layout_IPXNameChange_14(TObject p0)
        {
            IPXNameChange();
        }

        private void Layout_CancelPBEMBtnClick_15(TObject p0)
        {
            CancelPBEMBtnClick();
        }

        private void Layout_CheckEmailBtnClick_16(TObject p0)
        {
            CheckEmailBtnClick();
        }

        private void Layout_NewPBEMGameBtnClick_17(TObject p0)
        {
            NewPBEMGameBtnClick();
        }

        private void Layout_LoadPBEMGameBtnClick_18(TObject p0)
        {
            LoadPBEMGameBtnClick();
        }

        private void Layout_EmailSettingsBtnClick_19(TObject p0)
        {
            EmailSettingsBtnClick();
        }

        private void Layout_HeatNetBtnClick_20(TObject p0)
        {
            HeatNetBtnClick();
        }
    }
}
