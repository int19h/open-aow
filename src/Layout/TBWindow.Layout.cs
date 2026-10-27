// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTBWindow
{
    public partial class TTBWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TBWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TaskWindow = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "TaskWindow",
                Visible = false,
                Priority = 50,
                WinLeft = 0,
                WinTop = 432,
                WinWidth = 640,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(100),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = true,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 640,
                MinHeight = 48,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = false,
                WinHeight = 48,
                Dockable = false
            };
            LayoutSession.EndRead(TaskWindow);
            TBNextButton = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TBNextButton",
                Priority = 5,
                WinLeft = 568,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects the next available Unit"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 46,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 4,
                ILIndexLit = 3,
                ILIndexUp = 3,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 26,
                OnClick = Layout_TBNextButtonClick_0
            };
            LayoutSession.EndRead(TBNextButton);
            StartMenuBar = new Aow.Units.AOWMenuBar.TAOWMenuBar(this)
            {
                Name = "StartMenuBar",
                Priority = 50,
                WinLeft = 4,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Opens the options menu"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                LayoutItems = new LayoutStrings()
                {
                    " "
                },
                BorderWidth = 0,
                BorderHeight = 0,
                OnChange = Layout_StartMenuBarChange_1,
                Selected = -1,
                WinWidth = 40,
                TextColor = 0,
                TextSelColor = 0,
                ILIndexDown = 8,
                ILIndexUp = 11
            };
            LayoutSession.EndRead(StartMenuBar);
            TBMessEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "TBMessEdit",
                Priority = 5,
                WinLeft = 48,
                WinTop = 28,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The newest message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 48,
                    RightOffset = 275,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 317,
                BorderWidth = 5,
                BorderHeight = 3,
                MaxChars = 0,
                CursorChar = (byte)124,
                BlinkDelay = 0,
                TextColor = 0,
                Editable = false,
                ILIndex = 5,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(TBMessEdit);
            WGILbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WGILbl",
                Priority = 10,
                WinLeft = 3,
                WinTop = 5,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Gold income per Turn"
                },
                CaptureInput = false,
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
                    BottomOffset = 1
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(WGILbl);
            WGTLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WGTLbl",
                Priority = 10,
                WinLeft = 22,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Total amount of Gold"
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 23,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(WGTLbl);
            TurnButton = new Aow.Units.TurnButton.TAOWTurnButton(this)
            {
                Name = "TurnButton",
                Priority = 5,
                WinLeft = 596,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ends the Turn"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 3,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                CurPlayer = 0,
                LastPlayer = 0,
                Animating = false,
                OnAnimDone = Layout_TurnButtonAnimDone_2,
                OnChange = Layout_TurnButtonChange_3,
                Checked = false
            };
            LayoutSession.EndRead(TurnButton);
            WMButton = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "WMButton",
                Priority = 5,
                WinLeft = 72,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the World Map."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
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
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "World Map",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_WMButtonClick_4
            };
            LayoutSession.EndRead(WMButton);
            OVButton = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OVButton",
                Priority = 5,
                WinLeft = 145,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Realm Overview window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 14,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Realm",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_OVButtonClick_5
            };
            LayoutSession.EndRead(OVButton);
            ConButton = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ConButton",
                Visible = false,
                Priority = 0,
                WinLeft = 291,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Party window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 70,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Party",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_ConButtonClick_6
            };
            LayoutSession.EndRead(ConButton);
            StructButton = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "StructButton",
                Visible = false,
                Priority = 0,
                WinLeft = 291,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Structure window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 70,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Structure",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_StructButtonClick_7
            };
            LayoutSession.EndRead(StructButton);
            OV2Btn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OV2Btn",
                Priority = 0,
                WinLeft = 218,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Magic Management window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 28,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Magic",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_OV2BtnClick_8
            };
            LayoutSession.EndRead(OV2Btn);
            InfoBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "InfoBtn",
                Priority = 0,
                WinLeft = 291,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Events window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 56,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Events",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = InfoBtnClick
            };
            LayoutSession.EndRead(InfoBtn);
            ChatBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ChatBtn",
                OnUpdate = ChatBtnUpdate,
                Visible = false,
                Priority = 0,
                WinLeft = 437,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Chat window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 84,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Chat",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_ChatBtnClick_9
            };
            LayoutSession.EndRead(ChatBtn);
            TaskPanel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TaskPanel",
                Priority = 5,
                WinLeft = 48,
                WinTop = 7,
                WinWidth = 506,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 48,
                    RightOffset = 86,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TaskPanel);
            WGIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "WGIPnl",
                Priority = 10,
                WinLeft = 52,
                WinTop = 1,
                WinWidth = 16,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                WinHeight = 17,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(WGIPnl);
            TBbk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TBbk",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
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
                WinWidth = 640,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TBbk);
            TimerImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TimerImg",
                Priority = 50,
                WinLeft = 385,
                WinTop = 28,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The clock in non-timed games, or the countdown to the forced end-of-turn in timed games."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 222,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 28,
                AnimEnd = 0,
                WinWidth = 33,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TimerImg);
            TimeIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TimeIcn",
                Priority = 5,
                WinLeft = 371,
                WinTop = 29,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 258,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 30,
                WinWidth = 11,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TimeIcn);
            TimeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TimeLbl",
                Priority = 60,
                WinLeft = 388,
                WinTop = 32,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 225,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(TimeLbl);
            PInd = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "PInd",
                OnDraw = PIndDraw,
                Priority = 10,
                WinLeft = 290,
                WinTop = 28,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 275,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 75,
                WinHeight = 17,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 29,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(PInd);
            CurP = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CurP",
                OnUpdate = Layout_CurPUpdate_10,
                Priority = 15,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates current player"
                },
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
                AnimStart = 0,
                CurrentImage = 45,
                AnimEnd = 18,
                WinWidth = 5,
                WinHeight = 5,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CurP);
            TBWorld = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TBWorld",
                OnUpdate = TBWorldUpdate,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
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
                WinHeight = 48,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TBWorld);
            TCNextBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TCNextBtn",
                Priority = 5,
                WinLeft = 568,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects the next available unit"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 46,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 4,
                ILIndexLit = 3,
                ILIndexUp = 3,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 26,
                OnClick = Layout_TCNextBtnClick_11
            };
            LayoutSession.EndRead(TCNextBtn);
            TCStartBar = new Aow.Units.AOWMenuBar.TAOWMenuBar(this)
            {
                Name = "TCStartBar",
                Priority = 50,
                WinLeft = 5,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Opens the options menu"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                LayoutItems = new LayoutStrings()
                {
                    " "
                },
                BorderWidth = 0,
                BorderHeight = 0,
                OnChange = Layout_StartMenuBarChange_1,
                Selected = -1,
                WinWidth = 40,
                TextColor = 0,
                TextSelColor = 0,
                ILIndexDown = 8,
                ILIndexUp = 11
            };
            LayoutSession.EndRead(TCStartBar);
            TCMsg = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "TCMsg",
                Priority = 5,
                WinLeft = 48,
                WinTop = 28,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The newest message",
                    ""
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 48,
                    RightOffset = 275,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 317,
                BorderWidth = 5,
                BorderHeight = 3,
                MaxChars = 0,
                CursorChar = (byte)124,
                BlinkDelay = 0,
                TextColor = 16777215,
                Editable = false,
                ILIndex = 5,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(TCMsg);
            TCTurnButton = new Aow.Units.TurnButton.TAOWTurnButton(this)
            {
                Name = "TCTurnButton",
                Priority = 5,
                WinLeft = 596,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ends the Combat Round."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 3,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                CurPlayer = 0,
                LastPlayer = 0,
                Animating = false,
                OnAnimDone = Layout_TCTurnButtonAnimDone_12,
                OnChange = Layout_TCTurnButtonChange_13,
                Checked = false
            };
            LayoutSession.EndRead(TCTurnButton);
            TCScannerBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TCScannerBtn",
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Battle View."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
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
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Battle Map",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_TCScannerBtnClick_14
            };
            LayoutSession.EndRead(TCScannerBtn);
            TCUnitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TCUnitBtn",
                Visible = false,
                Priority = 0,
                WinLeft = 364,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Unit window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 70,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Unit",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_TCUnitBtnClick_15
            };
            LayoutSession.EndRead(TCUnitBtn);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
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
                WinWidth = 640,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            TCTimerImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCTimerImg",
                Priority = 5,
                WinLeft = 385,
                WinTop = 28,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The clock in non-timed games, or the countdown to the forced end-of-turn in timed games."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 222,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 28,
                AnimEnd = 0,
                WinWidth = 33,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCTimerImg);
            TCTimeImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCTimeImg",
                Priority = 5,
                WinLeft = 371,
                WinTop = 29,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 258,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 30,
                WinWidth = 11,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCTimeImg);
            TCTimeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TCTimeLbl",
                Priority = 10,
                WinLeft = 388,
                WinTop = 32,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 225,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(TCTimeLbl);
            TCPInd = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TCPInd",
                OnDraw = TCPIndDraw,
                Priority = 10,
                WinLeft = 290,
                WinTop = 28,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 275,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 75,
                WinHeight = 17,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 29,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TCPInd);
            TCCurP = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCCurP",
                OnUpdate = Layout_TCCurPUpdate_16,
                Priority = 15,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates current player"
                },
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
                AnimStart = 0,
                CurrentImage = 45,
                AnimEnd = 18,
                WinWidth = 5,
                WinHeight = 5,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCCurP);
            TBCombat = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TBCombat",
                OnUpdate = TBCombatUpdate,
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
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
                WinHeight = 48,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TBCombat);
            TCPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TCPnl",
                Priority = 5,
                WinLeft = 48,
                WinTop = 7,
                WinWidth = 506,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 48,
                    RightOffset = 86,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TCPnl);
            TCChatBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "TCChatBtn",
                OnUpdate = ChatBtnUpdate,
                Visible = false,
                Priority = 0,
                WinLeft = 437,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Chat window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 84,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Chat",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = Layout_ChatBtnClick_9
            };
            LayoutSession.EndRead(TCChatBtn);
            WGoldPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "WGoldPnl",
                Priority = 100,
                WinLeft = 423,
                WinTop = 26,
                WinWidth = 69,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 148,
                    TopOffset = 28,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 0,
                ILIndexTexture = 51,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(WGoldPnl);
            WGSign = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WGSign",
                Priority = 10,
                WinLeft = 46,
                WinTop = 6,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 16,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(WGSign);
            WMILbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WMILbl",
                Priority = 10,
                WinLeft = 3,
                WinTop = 5,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Mana income per Turn"
                },
                CaptureInput = false,
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
                    BottomOffset = 1
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(WMILbl);
            WMTLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WMTLbl",
                Priority = 10,
                WinLeft = 22,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Total amount of Mana Crystals"
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 23,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(WMTLbl);
            WMIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "WMIPnl",
                Priority = 10,
                WinLeft = 52,
                WinTop = 1,
                WinWidth = 16,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                WinHeight = 17,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(WMIPnl);
            WManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "WManaPnl",
                Priority = 100,
                WinLeft = 495,
                WinTop = 26,
                WinWidth = 69,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 76,
                    TopOffset = 28,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 0,
                ILIndexTexture = 52,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(WManaPnl);
            WMTSign = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WMTSign",
                Priority = 10,
                WinLeft = 46,
                WinTop = 6,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 16,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(WMTSign);
            TGILbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TGILbl",
                Priority = 10,
                WinLeft = 3,
                WinTop = 5,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Gold income per Turn"
                },
                CaptureInput = false,
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
                    BottomOffset = 1
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(TGILbl);
            TGTLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TGTLbl",
                Priority = 10,
                WinLeft = 22,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Total amount of Gold"
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 23,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(TGTLbl);
            TGIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TGIcn",
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates total amount of Gold and Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 55,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 51,
                AnimEnd = 0,
                WinWidth = 69,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TGIcn);
            TGIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TGIPnl",
                Priority = 10,
                WinLeft = 52,
                WinTop = 1,
                WinWidth = 16,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                WinHeight = 17,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TGIPnl);
            TGoldPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TGoldPnl",
                Priority = 100,
                WinLeft = 423,
                WinTop = 26,
                WinWidth = 69,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 148,
                    TopOffset = 28,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TGoldPnl);
            TGSign = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TGSign",
                Priority = 10,
                WinLeft = 46,
                WinTop = 6,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 16,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(TGSign);
            TMILbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TMILbl",
                Priority = 10,
                WinLeft = 3,
                WinTop = 5,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Mana income per Turn"
                },
                CaptureInput = false,
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
                    BottomOffset = 1
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(TMILbl);
            TMTLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TMTLbl",
                Priority = 10,
                WinLeft = 22,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Total amount of Mana Crystals"
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 23,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(TMTLbl);
            TWMIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TWMIcn",
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates total amount of Gold and Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 55,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 52,
                AnimEnd = 0,
                WinWidth = 69,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TWMIcn);
            TMIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TMIPnl",
                Priority = 10,
                WinLeft = 52,
                WinTop = 1,
                WinWidth = 16,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                WinHeight = 17,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TMIPnl);
            TManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TManaPnl",
                Priority = 100,
                WinLeft = 495,
                WinTop = 26,
                WinWidth = 69,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 76,
                    TopOffset = 28,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TManaPnl);
            TMTSign = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TMTSign",
                Priority = 10,
                WinLeft = 46,
                WinTop = 6,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 16,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = true,
                TextColor = 7864183
            };
            LayoutSession.EndRead(TMTSign);
            RelationsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RelationsBtn",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays the Relations Overview Window."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 42,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Relations",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                OnClick = RelationsBtnClick
            };
            LayoutSession.EndRead(RelationsBtn);
            AutoBtn = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "AutoBtn",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether your army fights under AI control."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(14),
                    LeftOffPercent = 42,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 2,
                BorderHeightSelected = 4,
                ILIndexDown = 17,
                ILIndexLit = 20,
                ILIndexUp = 14,
                Text = "Auto",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 70,
                Checked = false,
                OnChange = Layout_AutoBtn2Click_17,
                RadioIndex = 0
            };
            LayoutSession.EndRead(AutoBtn);
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
            TBNextButton.AOWWindow = TBWorld;
            StartMenuBar.AOWWindow = TBWorld;
            TBMessEdit.AOWWindow = TBWorld;
            WGILbl.AOWWindow = WGIPnl;
            WGTLbl.AOWWindow = WGoldPnl;
            TurnButton.AOWWindow = TBWorld;
            WMButton.AOWWindow = TaskPanel;
            OVButton.AOWWindow = TaskPanel;
            ConButton.AOWWindow = TaskPanel;
            StructButton.AOWWindow = TaskPanel;
            OV2Btn.AOWWindow = TaskPanel;
            InfoBtn.AOWWindow = TaskPanel;
            ChatBtn.AOWWindow = TaskPanel;
            TaskPanel.AOWWindow = TBWorld;
            WGIPnl.AOWWindow = WGoldPnl;
            TBbk.AOWWindow = TBWorld;
            TimerImg.AOWWindow = TBWorld;
            TimeIcn.AOWWindow = TBWorld;
            TimeLbl.AOWWindow = TBWorld;
            PInd.AOWWindow = TBWorld;
            CurP.AOWWindow = TBWorld;
            TBWorld.AOWWindow = TaskWindow;
            TCNextBtn.AOWWindow = TBCombat;
            TCStartBar.AOWWindow = TBCombat;
            TCMsg.AOWWindow = TBCombat;
            TCTurnButton.AOWWindow = TBCombat;
            TCScannerBtn.AOWWindow = TCPnl;
            TCUnitBtn.AOWWindow = TCPnl;
            AoWFrame1.AOWWindow = TBCombat;
            TCTimerImg.AOWWindow = TBCombat;
            TCTimeImg.AOWWindow = TBCombat;
            TCTimeLbl.AOWWindow = TBCombat;
            TCPInd.AOWWindow = TBCombat;
            TCCurP.AOWWindow = TBCombat;
            TBCombat.AOWWindow = TaskWindow;
            TCPnl.AOWWindow = TBCombat;
            TCChatBtn.AOWWindow = TCPnl;
            WGoldPnl.AOWWindow = TBWorld;
            WGSign.AOWWindow = WGoldPnl;
            WMILbl.AOWWindow = WMIPnl;
            WMTLbl.AOWWindow = WManaPnl;
            WMIPnl.AOWWindow = WManaPnl;
            WManaPnl.AOWWindow = TBWorld;
            WMTSign.AOWWindow = WManaPnl;
            TGILbl.AOWWindow = TGIPnl;
            TGTLbl.AOWWindow = TGoldPnl;
            TGIcn.AOWWindow = TGoldPnl;
            TGIPnl.AOWWindow = TGoldPnl;
            TGoldPnl.AOWWindow = TBCombat;
            TGSign.AOWWindow = TGoldPnl;
            TMILbl.AOWWindow = TMIPnl;
            TMTLbl.AOWWindow = TManaPnl;
            TWMIcn.AOWWindow = TManaPnl;
            TMIPnl.AOWWindow = TManaPnl;
            TManaPnl.AOWWindow = TBCombat;
            TMTSign.AOWWindow = TManaPnl;
            RelationsBtn.AOWWindow = TaskPanel;
            AutoBtn.AOWWindow = TCPnl;
            Loaded();
            TaskWindow.Loaded();
            TBNextButton.Loaded();
            StartMenuBar.Loaded();
            TBMessEdit.Loaded();
            WGILbl.Loaded();
            WGTLbl.Loaded();
            TurnButton.Loaded();
            WMButton.Loaded();
            OVButton.Loaded();
            ConButton.Loaded();
            StructButton.Loaded();
            OV2Btn.Loaded();
            InfoBtn.Loaded();
            ChatBtn.Loaded();
            TaskPanel.Loaded();
            WGIPnl.Loaded();
            TBbk.Loaded();
            TimerImg.Loaded();
            TimeIcn.Loaded();
            TimeLbl.Loaded();
            PInd.Loaded();
            CurP.Loaded();
            TBWorld.Loaded();
            TCNextBtn.Loaded();
            TCStartBar.Loaded();
            TCMsg.Loaded();
            TCTurnButton.Loaded();
            TCScannerBtn.Loaded();
            TCUnitBtn.Loaded();
            AoWFrame1.Loaded();
            TCTimerImg.Loaded();
            TCTimeImg.Loaded();
            TCTimeLbl.Loaded();
            TCPInd.Loaded();
            TCCurP.Loaded();
            TBCombat.Loaded();
            TCPnl.Loaded();
            TCChatBtn.Loaded();
            WGoldPnl.Loaded();
            WGSign.Loaded();
            WMILbl.Loaded();
            WMTLbl.Loaded();
            WMIPnl.Loaded();
            WManaPnl.Loaded();
            WMTSign.Loaded();
            TGILbl.Loaded();
            TGTLbl.Loaded();
            TGIcn.Loaded();
            TGIPnl.Loaded();
            TGoldPnl.Loaded();
            TGSign.Loaded();
            TMILbl.Loaded();
            TMTLbl.Loaded();
            TWMIcn.Loaded();
            TMIPnl.Loaded();
            TManaPnl.Loaded();
            TMTSign.Loaded();
            RelationsBtn.Loaded();
            AutoBtn.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            TaskWindow.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TBNextButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TBNextButton.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            StartMenuBar.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StartMenuBar.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            StartMenuBar.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TBMessEdit.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TBMessEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TBMessEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            WGILbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WGILbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            WGTLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WGTLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            TurnButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnButton.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TurnButton;
            WMButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WMButton.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            WMButton.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            OVButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            OVButton.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OVButton.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            ConButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ConButton.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ConButton.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            StructButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StructButton.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            StructButton.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            OV2Btn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            OV2Btn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OV2Btn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            InfoBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            InfoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            InfoBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            ChatBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ChatBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ChatBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TBbk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TimerImg.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TimerImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TimeIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            TimeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            PInd.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            CurP.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCNextBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCNextBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCStartBar.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCStartBar.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCStartBar.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCMsg.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCMsg.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCTurnButton.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCTurnButton.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TurnButton;
            TCScannerBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCScannerBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCScannerBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCUnitBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCUnitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCUnitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCTimerImg.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCTimerImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCTimeImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            TCTimeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            TCPInd.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCCurP.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TCChatBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TCChatBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCChatBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            WGoldPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            WGSign.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WGSign.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            WMILbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WMILbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            WMTLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WMTLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            WManaPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            WMTSign.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WMTSign.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TGILbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TGILbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TGTLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TGTLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            TGIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TGIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TGSign.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TGSign.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TMILbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TMILbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TMTLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TMTLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.TBarNumber;
            TWMIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TWMIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            TMTSign.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TMTSign.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            RelationsBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RelationsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RelationsBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            AutoBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AutoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AutoBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
        }

        private void Layout_TBNextButtonClick_0(TObject p0)
        {
            TBNextButtonClick();
        }

        private void Layout_StartMenuBarChange_1(TObject p0)
        {
            StartMenuBarChange();
        }

        private void Layout_TurnButtonAnimDone_2(TObject p0)
        {
            TurnButtonAnimDone();
        }

        private void Layout_TurnButtonChange_3(TObject p0)
        {
            TurnButtonChange();
        }

        private void Layout_WMButtonClick_4(TObject p0)
        {
            WMButtonClick();
        }

        private void Layout_OVButtonClick_5(TObject p0)
        {
            OVButtonClick((int)Aow.Rtl.Rtl.ObjectToInt(p0));
        }

        private void Layout_ConButtonClick_6(TObject p0)
        {
            ConButtonClick();
        }

        private void Layout_StructButtonClick_7(TObject p0)
        {
            StructButtonClick();
        }

        private void Layout_OV2BtnClick_8(TObject p0)
        {
            OV2BtnClick();
        }

        private void Layout_ChatBtnClick_9(TObject p0)
        {
            ChatBtnClick();
        }

        private void Layout_CurPUpdate_10(TObject p0)
        {
            CurPUpdate();
        }

        private void Layout_TCNextBtnClick_11(TObject p0)
        {
            TCNextBtnClick();
        }

        private void Layout_TCTurnButtonAnimDone_12(TObject p0)
        {
            TCTurnButtonAnimDone();
        }

        private void Layout_TCTurnButtonChange_13(TObject p0)
        {
            TCTurnButtonChange();
        }

        private void Layout_TCScannerBtnClick_14(TObject p0)
        {
            TCScannerBtnClick();
        }

        private void Layout_TCUnitBtnClick_15(TObject p0)
        {
            TCUnitBtnClick();
        }

        private void Layout_TCCurPUpdate_16(TObject p0)
        {
            TCCurPUpdate();
        }

        private void Layout_AutoBtn2Click_17(TObject p0)
        {
            AutoBtn2Click();
        }
    }
}
