// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWScenSetup
{
    public partial class TScenSetup
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ScenSetup";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            ChatMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ChatMemo",
                Priority = 2,
                WinLeft = 10,
                WinTop = 12,
                CaptureInput = true,
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
                    LeftOffset = 10,
                    RightOffset = 175,
                    TopOffset = 12,
                    BottomOffset = 40
                },
                Masked = false,
                WinWidth = 299,
                WinHeight = 93,
                BorderWidth = 6,
                BorderHeight = 5,
                TextColor = 12632256,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(1),
                Editable = false,
                ILIndexFrame = 64,
                ILIndexTexture = 4,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ChatMemo);
            ChatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ChatPnl",
                Priority = 1,
                WinLeft = 0,
                WinTop = 286,
                WinWidth = 484,
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
                    RightOffset = 16,
                    TopOffset = 286,
                    BottomOffset = 10
                },
                WinHeight = 145,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ChatPnl);
            ChatSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ChatSB",
                OnUpdate = Layout_ChatSBUpdate_0,
                Visible = false,
                Priority = 2,
                WinLeft = 292,
                WinTop = 12,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 93,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(ChatSB);
            ChatEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "ChatEdit",
                Priority = 0,
                WinLeft = 10,
                WinTop = 114,
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
                    LeftOffset = 10,
                    RightOffset = 175,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 299,
                BorderWidth = 8,
                BorderHeight = 6,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 15134446,
                Editable = true,
                ILIndex = 23,
                OnReturn = Layout_ChatEditReturn_1,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(ChatEdit);
            SidesPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SidesPnl",
                Priority = 10,
                WinLeft = 10,
                WinTop = 39,
                WinWidth = 500,
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
                WinHeight = 441,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SidesPnl);
            SettingsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SettingsPnl",
                Priority = 1,
                WinLeft = 500,
                WinTop = 40,
                WinWidth = 134,
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
                    LeftOffset = 500,
                    RightOffset = 6,
                    TopOffset = 40,
                    BottomOffset = 10
                },
                WinHeight = 430,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SettingsPnl);
            PlayerList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PlayerList",
                Priority = 0,
                WinLeft = 0,
                WinTop = 15,
                LayoutToolTipText = new LayoutStrings()
                {
                    ""
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 40
                },
                Masked = false,
                WinWidth = 70,
                WinHeight = 78,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = PlayerListChange,
                Selected = -1,
                OnDrawItem = PlayerListDrawItem,
                TextColor = 15134446,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 4,
                ILIndexTSelected = 16,
                ItemHeight = 15,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PlayerList);
            EjectBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "EjectBtn",
                Priority = 0,
                WinLeft = 85,
                WinTop = 103,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Eject the selected player."
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
                    LeftOffset = 400,
                    RightOffset = 11,
                    TopOffset = 0,
                    BottomOffset = 12
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "EJECT",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 70,
                OnClick = Layout_EjectBtnClick_2
            };
            LayoutSession.EndRead(EjectBtn);
            R1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R1Pnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R1Pnl);
            R5Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R5Pnl",
                Tag = 4,
                Priority = 0,
                WinLeft = 164,
                WinTop = 0,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R5Pnl);
            R9Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R9Pnl",
                Tag = 8,
                Priority = 0,
                WinLeft = 328,
                WinTop = 0,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 76,
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R9Pnl);
            R2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R2Pnl",
                Tag = 1,
                Priority = 0,
                WinLeft = 0,
                WinTop = 70,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R2Pnl);
            R6Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R6Pnl",
                Tag = 5,
                Priority = 0,
                WinLeft = 164,
                WinTop = 70,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 34,
                    TopOffPercent = 0,
                    BottomOffPercent = 51,
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R6Pnl);
            R10Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R10Pnl",
                Tag = 9,
                Priority = 0,
                WinLeft = 328,
                WinTop = 70,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 51,
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R10Pnl);
            R3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R3Pnl",
                Tag = 2,
                Priority = 0,
                WinLeft = 0,
                WinTop = 140,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R3Pnl);
            R7Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R7Pnl",
                Tag = 6,
                Priority = 0,
                WinLeft = 164,
                WinTop = 140,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 34,
                    TopOffPercent = 0,
                    BottomOffPercent = 26,
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R7Pnl);
            R11Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R11Pnl",
                Tag = 10,
                Priority = 0,
                WinLeft = 328,
                WinTop = 140,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 26,
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R11Pnl);
            R4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R4Pnl",
                Tag = 3,
                Priority = 0,
                WinLeft = 0,
                WinTop = 210,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R4Pnl);
            R8Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R8Pnl",
                Tag = 7,
                Priority = 0,
                WinLeft = 164,
                WinTop = 210,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 34,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R8Pnl);
            R12Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R12Pnl",
                Tag = 11,
                Priority = 0,
                WinLeft = 328,
                WinTop = 210,
                WinWidth = 156,
                OnDrawBack = RacePnlDrawBack,
                Masked = true,
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
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R12Pnl);
            ExitGame = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ExitGame",
                Priority = 99980,
                WinLeft = 10,
                WinTop = 372,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Aborts the current game."
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 40
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Exit",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 114,
                OnClick = ExitGameClick
            };
            LayoutSession.EndRead(ExitGame);
            PickScenarioBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PickScenarioBtn",
                Priority = 99980,
                WinLeft = 10,
                WinTop = 345,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Opens the scenario-selection dialog, to pick a new map."
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 67
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Scenario",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 114,
                OnClick = Layout_PickScenarioBtnClick_3
            };
            LayoutSession.EndRead(PickScenarioBtn);
            StartBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "StartBtn",
                Priority = 99980,
                WinLeft = 10,
                WinTop = 399,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Starts the game, using the current settings."
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 13
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Start!",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 114,
                OnClick = Layout_StartBtnClick_4
            };
            LayoutSession.EndRead(StartBtn);
            MiscLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MiscLbl",
                Priority = 0,
                WinLeft = 10,
                WinTop = 142,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 142,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Miscellaneous",
                TextColor = 12632256
            };
            LayoutSession.EndRead(MiscLbl);
            ScenSetupPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ScenSetupPnl",
                OnUpdate = Layout_ScenSetupPnlUpdate_5,
                Visible = false,
                Priority = 10,
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
                ILIndexTexture = 4,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(ScenSetupPnl);
            R1PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R1PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R1PlayerIcon);
            R1Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R1Lbl",
                Priority = 0,
                WinLeft = 28,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Humans",
                TextColor = 14316400
            };
            LayoutSession.EndRead(R1Lbl);
            R1PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R1PlayerCB",
                Priority = 170,
                WinLeft = 50,
                WinTop = 25,
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
                    LeftOffset = 51,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 170,
                TextColor = 14316400,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 15
            };
            LayoutSession.EndRead(R1PlayerCB);
            R1CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R1CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R1CenterPnl);
            TurnLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnLbl",
                Priority = 0,
                WinLeft = 10,
                WinTop = 10,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns ",
                TextColor = 12632256
            };
            LayoutSession.EndRead(TurnLbl);
            DurationLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DurationLbl",
                Priority = 0,
                WinLeft = 10,
                WinTop = 55,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Duration",
                TextColor = 12632256
            };
            LayoutSession.EndRead(DurationLbl);
            TurnsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnsLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 105,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the game's time limit to be in minutes."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns",
                TextColor = 8421504
            };
            LayoutSession.EndRead(TurnsLbl);
            TurnTimeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnTimeLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 75,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the game's time limit to be in turns."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 75,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turn Time",
                TextColor = 8421504
            };
            LayoutSession.EndRead(TurnTimeLbl);
            TurnTimeSl = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "TurnTimeSl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 87,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the time limit on each player's turn."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 87,
                    BottomOffset = 32
                },
                Masked = false,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 11,
                WinWidth = 50,
                NumberVisible = 1,
                OnChange = Layout_TurnTimeSlChange_6,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 49,
                ILIndexTab = 52
            };
            LayoutSession.EndRead(TurnTimeSl);
            TurnTimeVal = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnTimeVal",
                Priority = 100,
                WinLeft = 70,
                WinTop = 87,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the time limit on each player's turn."
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 87,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#######",
                TextColor = 8421504
            };
            LayoutSession.EndRead(TurnTimeVal);
            DurFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "DurFr",
                Priority = 0,
                WinLeft = 10,
                WinTop = 70,
                CaptureInput = true,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 114,
                WinHeight = 65,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(DurFr);
            TurnsSl = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "TurnsSl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 116,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the limit on the length of the game."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 116,
                    BottomOffset = 32
                },
                Masked = false,
                Min = 0,
                Max = 40,
                SmallChange = 1,
                LargeChange = 20,
                Position = 0,
                WinHeight = 11,
                WinWidth = 50,
                NumberVisible = 1,
                OnChange = TurnsSlChange,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 49,
                ILIndexTab = 52
            };
            LayoutSession.EndRead(TurnsSl);
            TurnsVal = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnsVal",
                Priority = 100,
                WinLeft = 70,
                WinTop = 117,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the limit on the length of the game."
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 117,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#######",
                TextColor = 8421504
            };
            LayoutSession.EndRead(TurnsVal);
            R1BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R1BehaviorCB",
                Priority = 160,
                WinLeft = 50,
                WinTop = 45,
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
                    LeftOffset = 51,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 160,
                TextColor = 14316400,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R1BehaviorCB);
            R1BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R1BehaviorSB",
                Visible = false,
                Priority = 160,
                WinLeft = 132,
                WinTop = 63,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R1BehaviorSB);
            R1PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R1PlayerSB",
                Visible = false,
                Priority = 170,
                WinLeft = 132,
                WinTop = 43,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R1PlayerSB);
            PlayModeCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "PlayModeCB",
                Priority = 500,
                WinLeft = 10,
                WinTop = 25,
                CaptureInput = true,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 25,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 114,
                BorderWidth = 10,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 2,
                OnChange = Layout_PlayModeCBChange_8,
                Selected = -1,
                PriorityOpened = 500,
                TextColor = 15134446,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(PlayModeCB);
            TurnVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "TurnVSB",
                Visible = false,
                Priority = 500,
                WinLeft = 107,
                WinTop = 43,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(TurnVSB);
            R1InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R1InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 14316400
            };
            LayoutSession.EndRead(R1InfoLbl);
            R3CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R3CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R3CenterPnl);
            R3Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R3Lbl",
                Priority = 0,
                WinLeft = 19,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Lizard Men",
                TextColor = 9347661
            };
            LayoutSession.EndRead(R3Lbl);
            R3BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R3BehaviorCB",
                Tag = 2,
                Priority = 120,
                WinLeft = 50,
                WinTop = 185,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 185,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 120,
                TextColor = 9347661,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R3BehaviorCB);
            R3BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R3BehaviorSB",
                Visible = false,
                Priority = 120,
                WinLeft = 132,
                WinTop = 203,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R3BehaviorSB);
            R3PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R3PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R3PlayerIcon);
            R3PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R3PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 9347661
            };
            LayoutSession.EndRead(R3PlayerLbl);
            R3PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R3PlayerCB",
                Tag = 2,
                Priority = 130,
                WinLeft = 50,
                WinTop = 165,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 165,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 130,
                TextColor = 9347661,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R3PlayerCB);
            R3PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R3PlayerSB",
                Visible = false,
                Priority = 130,
                WinLeft = 132,
                WinTop = 183,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R3PlayerSB);
            R3InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R3InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 9347661
            };
            LayoutSession.EndRead(R3InfoLbl);
            R2CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R2CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R2CenterPnl);
            R2Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R2Lbl",
                Priority = 0,
                WinLeft = 31,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Azracs",
                TextColor = 7845352
            };
            LayoutSession.EndRead(R2Lbl);
            R2BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R2BehaviorCB",
                Tag = 1,
                Priority = 140,
                WinLeft = 50,
                WinTop = 115,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 115,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 140,
                TextColor = 7845352,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R2BehaviorCB);
            R2BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R2BehaviorSB",
                Visible = false,
                Priority = 140,
                WinLeft = 132,
                WinTop = 133,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R2BehaviorSB);
            R2PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R2PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R2PlayerIcon);
            R2InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R2InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 7845352
            };
            LayoutSession.EndRead(R2InfoLbl);
            R2PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R2PlayerCB",
                Tag = 1,
                Priority = 150,
                WinLeft = 50,
                WinTop = 95,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 95,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 150,
                TextColor = 7845352,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R2PlayerCB);
            R2PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R2PlayerSB",
                Visible = false,
                Priority = 150,
                WinLeft = 132,
                WinTop = 113,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R2PlayerSB);
            R2PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R2PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 7845352
            };
            LayoutSession.EndRead(R2PlayerLbl);
            R4InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R4InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 15188376
            };
            LayoutSession.EndRead(R4InfoLbl);
            R4PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R4PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 15188376
            };
            LayoutSession.EndRead(R4PlayerLbl);
            R4CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R4CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R4CenterPnl);
            R4Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R4Lbl",
                Priority = 0,
                WinLeft = 23,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Frostlings",
                TextColor = 15188376
            };
            LayoutSession.EndRead(R4Lbl);
            R4BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R4BehaviorSB",
                Visible = false,
                Priority = 100,
                WinLeft = 132,
                WinTop = 273,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R4BehaviorSB);
            R4PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R4PlayerCB",
                Tag = 3,
                Priority = 110,
                WinLeft = 50,
                WinTop = 235,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 235,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 110,
                TextColor = 15188376,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R4PlayerCB);
            R4PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R4PlayerSB",
                Visible = false,
                Priority = 110,
                WinLeft = 132,
                WinTop = 253,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R4PlayerSB);
            R4PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R4PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R4PlayerIcon);
            R5PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R5PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R5PlayerIcon);
            R5Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R5Lbl",
                Priority = 0,
                WinLeft = 34,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Elves",
                TextColor = 4830826
            };
            LayoutSession.EndRead(R5Lbl);
            R5PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R5PlayerCB",
                Tag = 4,
                Priority = 170,
                WinLeft = 212,
                WinTop = 25,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 25,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 170,
                TextColor = 4830826,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R5PlayerCB);
            R5CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R5CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R5CenterPnl);
            R5BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R5BehaviorCB",
                Tag = 4,
                Priority = 160,
                WinLeft = 212,
                WinTop = 45,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 45,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 160,
                TextColor = 4830826,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R5BehaviorCB);
            R5BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R5BehaviorSB",
                Visible = false,
                Priority = 160,
                WinLeft = 294,
                WinTop = 63,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R5BehaviorSB);
            R5PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R5PlayerSB",
                Visible = false,
                Priority = 170,
                WinLeft = 294,
                WinTop = 43,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R5PlayerSB);
            R5InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R5InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 4830826
            };
            LayoutSession.EndRead(R5InfoLbl);
            R5PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R5PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 4830826
            };
            LayoutSession.EndRead(R5PlayerLbl);
            R6CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R6CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R6CenterPnl);
            R6Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R6Lbl",
                Priority = 0,
                WinLeft = 26,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Halflings",
                TextColor = 7464174
            };
            LayoutSession.EndRead(R6Lbl);
            R6BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R6BehaviorCB",
                Tag = 5,
                Priority = 140,
                WinLeft = 212,
                WinTop = 115,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 115,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 140,
                TextColor = 7464174,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R6BehaviorCB);
            R6BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R6BehaviorSB",
                Visible = false,
                Priority = 140,
                WinLeft = 294,
                WinTop = 133,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R6BehaviorSB);
            R6PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R6PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R6PlayerIcon);
            R6PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R6PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 7464174
            };
            LayoutSession.EndRead(R6PlayerLbl);
            R6PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R6PlayerCB",
                Tag = 5,
                Priority = 150,
                WinLeft = 212,
                WinTop = 95,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 95,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 150,
                TextColor = 7464174,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R6PlayerCB);
            R6PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R6PlayerSB",
                Visible = false,
                Priority = 150,
                WinLeft = 294,
                WinTop = 113,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R6PlayerSB);
            R6InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R6InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 7464174
            };
            LayoutSession.EndRead(R6InfoLbl);
            R7CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R7CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R7CenterPnl);
            R7Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R7Lbl",
                Priority = 0,
                WinLeft = 26,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Dwarves",
                TextColor = 9351120
            };
            LayoutSession.EndRead(R7Lbl);
            R7BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R7BehaviorCB",
                Tag = 6,
                Priority = 120,
                WinLeft = 212,
                WinTop = 185,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 185,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 120,
                TextColor = 9351120,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R7BehaviorCB);
            R7BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R7BehaviorSB",
                Visible = false,
                Priority = 120,
                WinLeft = 294,
                WinTop = 203,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R7BehaviorSB);
            R7PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R7PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R7PlayerIcon);
            R7PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R7PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 9351120
            };
            LayoutSession.EndRead(R7PlayerLbl);
            R7PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R7PlayerCB",
                Tag = 6,
                Priority = 130,
                WinLeft = 212,
                WinTop = 165,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 165,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 130,
                TextColor = 9351120,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R7PlayerCB);
            R7PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R7PlayerSB",
                Visible = false,
                Priority = 130,
                WinLeft = 294,
                WinTop = 183,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R7PlayerSB);
            R7InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R7InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 9351120
            };
            LayoutSession.EndRead(R7InfoLbl);
            R8InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R8InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 14346733
            };
            LayoutSession.EndRead(R8InfoLbl);
            R8PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R8PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 14346733
            };
            LayoutSession.EndRead(R8PlayerLbl);
            R8CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R8CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R8CenterPnl);
            R8Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R8Lbl",
                Priority = 0,
                WinLeft = 24,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "High Men",
                TextColor = 14346733
            };
            LayoutSession.EndRead(R8Lbl);
            R8BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R8BehaviorCB",
                Tag = 7,
                Priority = 100,
                WinLeft = 212,
                WinTop = 255,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 255,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 100,
                TextColor = 14346733,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R8BehaviorCB);
            R8BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R8BehaviorSB",
                Visible = false,
                Priority = 100,
                WinLeft = 294,
                WinTop = 273,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R8BehaviorSB);
            R8PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R8PlayerCB",
                Tag = 7,
                Priority = 110,
                WinLeft = 212,
                WinTop = 235,
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
                    LeftOffset = 215,
                    RightOffset = 7,
                    TopOffset = 235,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 110,
                TextColor = 14346733,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R8PlayerCB);
            R8PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R8PlayerSB",
                Visible = false,
                Priority = 110,
                WinLeft = 294,
                WinTop = 253,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R8PlayerSB);
            R8PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R8PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R8PlayerIcon);
            R9PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R9PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R9PlayerIcon);
            R9Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R9Lbl",
                Priority = 0,
                WinLeft = 19,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Dark Elves",
                TextColor = 12215222
            };
            LayoutSession.EndRead(R9Lbl);
            R9PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R9PlayerCB",
                Tag = 8,
                Priority = 170,
                WinLeft = 376,
                WinTop = 25,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 25,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 170,
                TextColor = 12215222,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R9PlayerCB);
            R9CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R9CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R9CenterPnl);
            R9BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R9BehaviorCB",
                Tag = 8,
                Priority = 160,
                WinLeft = 376,
                WinTop = 45,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 45,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 160,
                TextColor = 12215222,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R9BehaviorCB);
            R9BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R9BehaviorSB",
                Visible = false,
                Priority = 160,
                WinLeft = 458,
                WinTop = 63,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R9BehaviorSB);
            R9PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R9PlayerSB",
                Visible = false,
                Priority = 170,
                WinLeft = 458,
                WinTop = 43,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R9PlayerSB);
            R9InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R9InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 12215222
            };
            LayoutSession.EndRead(R9InfoLbl);
            R9PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R9PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 12215222
            };
            LayoutSession.EndRead(R9PlayerLbl);
            R10CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R10CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R10CenterPnl);
            R10Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R10Lbl",
                Priority = 0,
                WinLeft = 36,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Orcs",
                TextColor = 4408305
            };
            LayoutSession.EndRead(R10Lbl);
            R10BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R10BehaviorCB",
                Tag = 9,
                Priority = 140,
                WinLeft = 376,
                WinTop = 115,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 115,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 140,
                TextColor = 4408305,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R10BehaviorCB);
            R10BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R10BehaviorSB",
                Visible = false,
                Priority = 140,
                WinLeft = 458,
                WinTop = 133,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R10BehaviorSB);
            R10PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R10PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R10PlayerIcon);
            R10PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R10PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 4408305
            };
            LayoutSession.EndRead(R10PlayerLbl);
            R10PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R10PlayerCB",
                Tag = 9,
                Priority = 150,
                WinLeft = 376,
                WinTop = 95,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 95,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 150,
                TextColor = 4408305,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R10PlayerCB);
            s10sb2 = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "s10sb2",
                Visible = false,
                Priority = 150,
                WinLeft = 458,
                WinTop = 113,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(s10sb2);
            R10InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R10InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 4408305
            };
            LayoutSession.EndRead(R10InfoLbl);
            R11CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R11CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R11CenterPnl);
            R11Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R11Lbl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Goblins",
                TextColor = 5731259
            };
            LayoutSession.EndRead(R11Lbl);
            R11BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R11BehaviorCB",
                Tag = 10,
                Priority = 120,
                WinLeft = 376,
                WinTop = 185,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 185,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 120,
                TextColor = 5731259,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R11BehaviorCB);
            R11BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R11BehaviorSB",
                Visible = false,
                Priority = 120,
                WinLeft = 458,
                WinTop = 203,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R11BehaviorSB);
            R11PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R11PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R11PlayerIcon);
            R11PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R11PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 5731259
            };
            LayoutSession.EndRead(R11PlayerLbl);
            R11PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R11PlayerCB",
                Tag = 10,
                Priority = 130,
                WinLeft = 376,
                WinTop = 165,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 165,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 130,
                TextColor = 5731259,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R11PlayerCB);
            R11PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R11PlayerSB",
                Visible = false,
                Priority = 130,
                WinLeft = 458,
                WinTop = 183,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R11PlayerSB);
            R11InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R11InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 5731259
            };
            LayoutSession.EndRead(R11InfoLbl);
            R12InfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R12InfoLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 59,
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
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 7765394
            };
            LayoutSession.EndRead(R12InfoLbl);
            R12PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R12PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 7765394
            };
            LayoutSession.EndRead(R12PlayerLbl);
            R12CenterPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R12CenterPnl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 0,
                WinWidth = 99,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R12CenterPnl);
            R12Lbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R12Lbl",
                Priority = 0,
                WinLeft = 17,
                WinTop = 9,
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
                    LeftOffset = 49,
                    RightOffset = 0,
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The Undead",
                TextColor = 7765394
            };
            LayoutSession.EndRead(R12Lbl);
            R12BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R12BehaviorCB",
                Tag = 11,
                Priority = 100,
                WinLeft = 376,
                WinTop = 255,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 255,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 100,
                TextColor = 7765394,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R12BehaviorCB);
            R12BehaviorSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R12BehaviorSB",
                Visible = false,
                Priority = 100,
                WinLeft = 458,
                WinTop = 273,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R12BehaviorSB);
            R12PlayerCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R12PlayerCB",
                Tag = 11,
                Priority = 110,
                WinLeft = 376,
                WinTop = 235,
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
                    LeftOffset = 379,
                    RightOffset = 7,
                    TopOffset = 235,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 7,
                OnChange = RacePlayerCBChange,
                Selected = -1,
                PriorityOpened = 110,
                TextColor = 7765394,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R12PlayerCB);
            R12PlayerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "R12PlayerSB",
                Visible = false,
                Priority = 110,
                WinLeft = 458,
                WinTop = 253,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(R12PlayerSB);
            R12PlayerIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R12PlayerIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
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
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 13,
                WinWidth = 50,
                WinHeight = 46,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R12PlayerIcon);
            PreferredRaceCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "PreferredRaceCB",
                Priority = 99990,
                WinLeft = 10,
                WinTop = 308,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 104
                },
                Masked = false,
                WinWidth = 114,
                LayoutItems = new LayoutStrings()
                {
                    "Humans",
                    "Lizards"
                },
                BorderWidth = 10,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 2,
                OnChange = Layout_PreferredRaceCBChange_9,
                Selected = 0,
                PriorityOpened = 99990,
                TextColor = 15134446,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(PreferredRaceCB);
            PreferredRaceSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PreferredRaceSB",
                Visible = false,
                Priority = 99990,
                WinLeft = 107,
                WinTop = 326,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 31,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(PreferredRaceSB);
            PreferredRaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PreferredRaceLbl",
                Priority = 99999,
                WinLeft = 10,
                WinTop = 295,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 295,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Preferred Side",
                TextColor = 8421504
            };
            LayoutSession.EndRead(PreferredRaceLbl);
            LFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LFr",
                Priority = 0,
                WinLeft = 10,
                WinTop = 155,
                CaptureInput = true,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 114,
                WinHeight = 70,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LFr);
            CustomizeLeaders = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "CustomizeLeaders",
                Priority = 100,
                WinLeft = 15,
                WinTop = 177,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not players can customize their Leader before play begins."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 177,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = CustomizeLeadersChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(CustomizeLeaders);
            CustomizeLeadersLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CustomizeLeadersLbl",
                Priority = 100,
                WinLeft = 30,
                WinTop = 179,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not players can customize their Leader before play begins."
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 179,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Customize leaders",
                TextColor = 8421504
            };
            LayoutSession.EndRead(CustomizeLeadersLbl);
            R4BehaviorCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "R4BehaviorCB",
                Tag = 3,
                Priority = 100,
                WinLeft = 50,
                WinTop = 255,
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
                    LeftOffset = 51,
                    RightOffset = 7,
                    TopOffset = 255,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 98,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 6,
                OnChange = Layout_RaceBehaviorCBChange_7,
                Selected = -1,
                PriorityOpened = 100,
                TextColor = 15188376,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(R4BehaviorCB);
            R1FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R1FogPnl",
                Priority = 175,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 166,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R1FogPnl);
            R1PlayerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "R1PlayerLbl",
                Priority = 0,
                WinLeft = 49,
                WinTop = 41,
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
                    BottomOffset = 25
                },
                Masked = true,
                TextColor = 14316400
            };
            LayoutSession.EndRead(R1PlayerLbl);
            R2FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R2FogPnl",
                Priority = 155,
                WinLeft = 0,
                WinTop = 70,
                WinWidth = 156,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R2FogPnl);
            R3FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R3FogPnl",
                Priority = 135,
                WinLeft = 0,
                WinTop = 140,
                WinWidth = 156,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R3FogPnl);
            R4FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R4FogPnl",
                Priority = 115,
                WinLeft = 0,
                WinTop = 210,
                WinWidth = 156,
                Masked = true,
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
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R4FogPnl);
            R8FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R8FogPnl",
                Priority = 115,
                WinLeft = 164,
                WinTop = 210,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R8FogPnl);
            R7FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R7FogPnl",
                Priority = 135,
                WinLeft = 164,
                WinTop = 140,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R7FogPnl);
            R6FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R6FogPnl",
                Priority = 155,
                WinLeft = 164,
                WinTop = 70,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R6FogPnl);
            R5FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R5FogPnl",
                Priority = 175,
                WinLeft = 164,
                WinTop = 0,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 164,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R5FogPnl);
            R12FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R12FogPnl",
                Priority = 115,
                WinLeft = 328,
                WinTop = 210,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R12FogPnl);
            R11FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R11FogPnl",
                Priority = 135,
                WinLeft = 328,
                WinTop = 140,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R11FogPnl);
            R10FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R10FogPnl",
                Priority = 155,
                WinLeft = 328,
                WinTop = 70,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R10FogPnl);
            R9FogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "R9FogPnl",
                Priority = 175,
                WinLeft = 328,
                WinTop = 0,
                WinWidth = 156,
                Masked = true,
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
                    LeftOffset = 328,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(R9FogPnl);
            PreferredRaceSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PreferredRaceSort",
                Priority = 300,
                WinLeft = 95,
                WinTop = 0,
                CaptureInput = false,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 52,
                Text = "Pref. Race",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 61
            };
            LayoutSession.EndRead(PreferredRaceSort);
            PlayerListSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PlayerListSort",
                Priority = 500,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = false,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 52,
                Text = "Players",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 71
            };
            LayoutSession.EndRead(PlayerListSort);
            ConnectionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ConnectionPnl",
                Priority = 1,
                WinLeft = 318,
                WinTop = 12,
                WinWidth = 166,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 12,
                    BottomOffset = 0
                },
                WinHeight = 133,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ConnectionPnl);
            PreferredRaceList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PreferredRaceList",
                Priority = 2,
                WinLeft = 95,
                WinTop = 15,
                LayoutToolTipText = new LayoutStrings()
                {
                    ""
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 40
                },
                Masked = false,
                WinWidth = 61,
                WinHeight = 78,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = PlayerListChange,
                Selected = -1,
                TextColor = 15134446,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 4,
                ILIndexTSelected = 16,
                ItemHeight = 15,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PreferredRaceList);
            AOWButton1 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AOWButton1",
                Priority = 301,
                WinLeft = 70,
                WinTop = 0,
                CaptureInput = false,
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 52,
                Text = "Ping",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 26
            };
            LayoutSession.EndRead(AOWButton1);
            PingList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PingList",
                Priority = 999,
                WinLeft = 70,
                WinTop = 15,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 40
                },
                Masked = false,
                WinWidth = 25,
                WinHeight = 78,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = PlayerListChange,
                Selected = -1,
                OnDrawItem = PingListDrawItem,
                TextColor = 15134446,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 4,
                ILIndexTSelected = 16,
                ItemHeight = 15,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(PingList);
            PlayerListSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PlayerListSB",
                Visible = false,
                Priority = 2,
                WinLeft = 139,
                WinTop = 15,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 78,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_PlayerListSBChange_10,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(PlayerListSB);
            SettingsFogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SettingsFogPnl",
                Priority = 10010,
                WinLeft = 0,
                WinTop = 2,
                WinWidth = 132,
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
                WinHeight = 420,
                ILIndexFrame = 0,
                ILIndexTexture = 22,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SettingsFogPnl);
            ExplorationLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ExplorationLbl",
                Priority = 100,
                WinLeft = 30,
                WinTop = 209,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether the map is hidden when play begins, or all areas are immediately visible."
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 209,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Exploration",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ExplorationLbl);
            Exploration = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Exploration",
                Priority = 100,
                WinLeft = 15,
                WinTop = 207,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether the map is hidden when play begins, or all areas are immediately visible."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 207,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = ExplorationChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Exploration);
            ScenarioLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ScenarioLbl",
                Priority = 10,
                WinLeft = 210,
                WinTop = 8,
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
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######################",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ScenarioLbl);
            HostNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HostNameLbl",
                Priority = 10,
                WinLeft = 8,
                WinTop = 27,
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 27,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##############",
                TextColor = 10329501
            };
            LayoutSession.EndRead(HostNameLbl);
            PlayerNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerNameLbl",
                Priority = 10,
                WinLeft = 542,
                WinTop = 27,
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
                    RightOffset = 8,
                    TopOffset = 27,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###############",
                TextColor = 10329501
            };
            LayoutSession.EndRead(PlayerNameLbl);
            FileNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "FileNameLbl",
                Priority = 10,
                WinLeft = 263,
                WinTop = 27,
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
                    TopOffset = 27,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###################",
                TextColor = 10329501
            };
            LayoutSession.EndRead(FileNameLbl);
            MakeHostBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MakeHostBtn",
                Priority = 0,
                WinLeft = 0,
                WinTop = 103,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 18,
                    TopOffset = 0,
                    BottomOffset = 12
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "MAKE HOST",
                TextColor = 15134446,
                TextSelColor = 16777215,
                WinWidth = 70,
                OnClick = Layout_MakeHostBtnClick_11
            };
            LayoutSession.EndRead(MakeHostBtn);
            RestartSettingsFogPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RestartSettingsFogPnl",
                Visible = false,
                Priority = 9999,
                WinLeft = 0,
                WinTop = 130,
                WinWidth = 132,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
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
                    BottomOffset = 0
                },
                WinHeight = 300,
                ILIndexFrame = 0,
                ILIndexTexture = 22,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RestartSettingsFogPnl);
            ChatFogPnl = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ChatFogPnl",
                Priority = 100,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = true,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 0,
                WinWidth = 484,
                WinHeight = 145,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ChatFogPnl);
            LeadersOnMapCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LeadersOnMapCB",
                Priority = 100,
                WinLeft = 15,
                WinTop = 162,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not Leaders are represented by units on the game map."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 162,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = LeadersOnMapCBChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(LeadersOnMapCB);
            AOWLabel1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AOWLabel1",
                Priority = 100,
                WinLeft = 30,
                WinTop = 164,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not Leaders are represented by units on the game map."
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 164,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Leaders on map",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AOWLabel1);
            AlliedVictoryCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "AlliedVictoryCB",
                Priority = 100,
                WinLeft = 15,
                WinTop = 192,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether allies can have a joint victory, or only one player can win the game."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 192,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = AlliedVictoryCBChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(AlliedVictoryCB);
            AlliedVictoryLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AlliedVictoryLbl",
                Priority = 100,
                WinLeft = 30,
                WinTop = 194,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether allies can have a joint victory, or only one player can win the game."
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 194,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Allied victory",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AlliedVictoryLbl);
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
            AOWVScrollBar1 = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AOWVScrollBar1",
                Visible = false,
                Priority = 10000,
                WinLeft = 107,
                WinTop = 268,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 5,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(AOWVScrollBar1);
            TacticalCombatCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "TacticalCombatCB",
                Priority = 10000,
                WinLeft = 10,
                WinTop = 250,
                CaptureInput = true,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 250,
                    BottomOffset = 23
                },
                Masked = false,
                WinWidth = 114,
                BorderWidth = 10,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 5,
                OnChange = Layout_TacticalCombatCBChange_12,
                Selected = -1,
                PriorityOpened = 199999,
                TextColor = 15134446,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(TacticalCombatCB);
            TacticalCombatLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TacticalCombatLbl",
                Priority = 10000,
                WinLeft = 10,
                WinTop = 235,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 235,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Allow Tactical Combat",
                TextColor = 12632256
            };
            LayoutSession.EndRead(TacticalCombatLbl);
            AlwaysViewTCCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "AlwaysViewTCCB",
                Priority = 10001,
                WinLeft = 15,
                WinTop = 270,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not Leaders are represented by units on the game map."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 270,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_AlwaysViewTCCBChange_13,
                RadioIndex = 0
            };
            LayoutSession.EndRead(AlwaysViewTCCB);
            AlwaysViewTCLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AlwaysViewTCLbl",
                Priority = 10001,
                WinLeft = 30,
                WinTop = 272,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether or not Leaders are represented by units on the game map."
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 272,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Always View TC",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AlwaysViewTCLbl);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            ChatMemo.AOWWindow = ChatPnl;
            ChatMemo.VScrollBar = ChatSB;
            ChatPnl.AOWWindow = SidesPnl;
            ChatSB.AOWWindow = ChatPnl;
            ChatEdit.AOWWindow = ChatPnl;
            SidesPnl.AOWWindow = ScenSetupPnl;
            SettingsPnl.AOWWindow = ScenSetupPnl;
            PlayerList.AOWWindow = ConnectionPnl;
            EjectBtn.AOWWindow = ConnectionPnl;
            R1Pnl.AOWWindow = SidesPnl;
            R5Pnl.AOWWindow = SidesPnl;
            R9Pnl.AOWWindow = SidesPnl;
            R2Pnl.AOWWindow = SidesPnl;
            R6Pnl.AOWWindow = SidesPnl;
            R10Pnl.AOWWindow = SidesPnl;
            R3Pnl.AOWWindow = SidesPnl;
            R7Pnl.AOWWindow = SidesPnl;
            R11Pnl.AOWWindow = SidesPnl;
            R4Pnl.AOWWindow = SidesPnl;
            R8Pnl.AOWWindow = SidesPnl;
            R12Pnl.AOWWindow = SidesPnl;
            ExitGame.AOWWindow = SettingsPnl;
            PickScenarioBtn.AOWWindow = SettingsPnl;
            StartBtn.AOWWindow = SettingsPnl;
            MiscLbl.AOWWindow = SettingsPnl;
            R1PlayerIcon.AOWWindow = R1Pnl;
            R1Lbl.AOWWindow = R1CenterPnl;
            R1PlayerCB.AOWWindow = SidesPnl;
            R1PlayerCB.VScrollBar = R1PlayerSB;
            R1CenterPnl.AOWWindow = R1Pnl;
            TurnLbl.AOWWindow = SettingsPnl;
            DurationLbl.AOWWindow = SettingsPnl;
            TurnsLbl.AOWWindow = SettingsPnl;
            TurnTimeLbl.AOWWindow = SettingsPnl;
            TurnTimeSl.AOWWindow = SettingsPnl;
            TurnTimeVal.AOWWindow = SettingsPnl;
            DurFr.AOWWindow = SettingsPnl;
            TurnsSl.AOWWindow = SettingsPnl;
            TurnsVal.AOWWindow = SettingsPnl;
            R1BehaviorCB.AOWWindow = SidesPnl;
            R1BehaviorCB.VScrollBar = R1BehaviorSB;
            R1BehaviorSB.AOWWindow = SidesPnl;
            R1PlayerSB.AOWWindow = SidesPnl;
            PlayModeCB.AOWWindow = SettingsPnl;
            PlayModeCB.VScrollBar = TurnVSB;
            TurnVSB.AOWWindow = SettingsPnl;
            R1InfoLbl.AOWWindow = R1CenterPnl;
            R3CenterPnl.AOWWindow = R3Pnl;
            R3Lbl.AOWWindow = R3CenterPnl;
            R3BehaviorCB.AOWWindow = SidesPnl;
            R3BehaviorCB.VScrollBar = R3BehaviorSB;
            R3BehaviorSB.AOWWindow = SidesPnl;
            R3PlayerIcon.AOWWindow = R3Pnl;
            R3PlayerLbl.AOWWindow = R3CenterPnl;
            R3PlayerCB.AOWWindow = SidesPnl;
            R3PlayerCB.VScrollBar = R3PlayerSB;
            R3PlayerSB.AOWWindow = SidesPnl;
            R3InfoLbl.AOWWindow = R3CenterPnl;
            R2CenterPnl.AOWWindow = R2Pnl;
            R2Lbl.AOWWindow = R2CenterPnl;
            R2BehaviorCB.AOWWindow = SidesPnl;
            R2BehaviorCB.VScrollBar = R2BehaviorSB;
            R2BehaviorSB.AOWWindow = SidesPnl;
            R2PlayerIcon.AOWWindow = R2Pnl;
            R2InfoLbl.AOWWindow = R2CenterPnl;
            R2PlayerCB.AOWWindow = SidesPnl;
            R2PlayerCB.VScrollBar = R2PlayerSB;
            R2PlayerSB.AOWWindow = SidesPnl;
            R2PlayerLbl.AOWWindow = R2CenterPnl;
            R4InfoLbl.AOWWindow = R4CenterPnl;
            R4PlayerLbl.AOWWindow = R4CenterPnl;
            R4CenterPnl.AOWWindow = R4Pnl;
            R4Lbl.AOWWindow = R4CenterPnl;
            R4BehaviorSB.AOWWindow = SidesPnl;
            R4PlayerCB.AOWWindow = SidesPnl;
            R4PlayerCB.VScrollBar = R4PlayerSB;
            R4PlayerSB.AOWWindow = SidesPnl;
            R4PlayerIcon.AOWWindow = R4Pnl;
            R5PlayerIcon.AOWWindow = R5Pnl;
            R5Lbl.AOWWindow = R5CenterPnl;
            R5PlayerCB.AOWWindow = SidesPnl;
            R5PlayerCB.VScrollBar = R5PlayerSB;
            R5CenterPnl.AOWWindow = R5Pnl;
            R5BehaviorCB.AOWWindow = SidesPnl;
            R5BehaviorCB.VScrollBar = R5BehaviorSB;
            R5BehaviorSB.AOWWindow = SidesPnl;
            R5PlayerSB.AOWWindow = SidesPnl;
            R5InfoLbl.AOWWindow = R5CenterPnl;
            R5PlayerLbl.AOWWindow = R5CenterPnl;
            R6CenterPnl.AOWWindow = R6Pnl;
            R6Lbl.AOWWindow = R6CenterPnl;
            R6BehaviorCB.AOWWindow = SidesPnl;
            R6BehaviorCB.VScrollBar = R6BehaviorSB;
            R6BehaviorSB.AOWWindow = SidesPnl;
            R6PlayerIcon.AOWWindow = R6Pnl;
            R6PlayerLbl.AOWWindow = R6CenterPnl;
            R6PlayerCB.AOWWindow = SidesPnl;
            R6PlayerCB.VScrollBar = R6PlayerSB;
            R6PlayerSB.AOWWindow = SidesPnl;
            R6InfoLbl.AOWWindow = R6CenterPnl;
            R7CenterPnl.AOWWindow = R7Pnl;
            R7Lbl.AOWWindow = R7CenterPnl;
            R7BehaviorCB.AOWWindow = SidesPnl;
            R7BehaviorCB.VScrollBar = R7BehaviorSB;
            R7BehaviorSB.AOWWindow = SidesPnl;
            R7PlayerIcon.AOWWindow = R7Pnl;
            R7PlayerLbl.AOWWindow = R7CenterPnl;
            R7PlayerCB.AOWWindow = SidesPnl;
            R7PlayerCB.VScrollBar = R7PlayerSB;
            R7PlayerSB.AOWWindow = SidesPnl;
            R7InfoLbl.AOWWindow = R7CenterPnl;
            R8InfoLbl.AOWWindow = R8CenterPnl;
            R8PlayerLbl.AOWWindow = R8CenterPnl;
            R8CenterPnl.AOWWindow = R8Pnl;
            R8Lbl.AOWWindow = R8CenterPnl;
            R8BehaviorCB.AOWWindow = SidesPnl;
            R8BehaviorCB.VScrollBar = R8BehaviorSB;
            R8BehaviorSB.AOWWindow = SidesPnl;
            R8PlayerCB.AOWWindow = SidesPnl;
            R8PlayerCB.VScrollBar = R8PlayerSB;
            R8PlayerSB.AOWWindow = SidesPnl;
            R8PlayerIcon.AOWWindow = R8Pnl;
            R9PlayerIcon.AOWWindow = R9Pnl;
            R9Lbl.AOWWindow = R9CenterPnl;
            R9PlayerCB.AOWWindow = SidesPnl;
            R9PlayerCB.VScrollBar = R9PlayerSB;
            R9CenterPnl.AOWWindow = R9Pnl;
            R9BehaviorCB.AOWWindow = SidesPnl;
            R9BehaviorCB.VScrollBar = R9BehaviorSB;
            R9BehaviorSB.AOWWindow = SidesPnl;
            R9PlayerSB.AOWWindow = SidesPnl;
            R9InfoLbl.AOWWindow = R9CenterPnl;
            R9PlayerLbl.AOWWindow = R9CenterPnl;
            R10CenterPnl.AOWWindow = R10Pnl;
            R10Lbl.AOWWindow = R10CenterPnl;
            R10BehaviorCB.AOWWindow = SidesPnl;
            R10BehaviorCB.VScrollBar = R10BehaviorSB;
            R10BehaviorSB.AOWWindow = SidesPnl;
            R10PlayerIcon.AOWWindow = R10Pnl;
            R10PlayerLbl.AOWWindow = R10CenterPnl;
            R10PlayerCB.AOWWindow = SidesPnl;
            R10PlayerCB.VScrollBar = s10sb2;
            s10sb2.AOWWindow = SidesPnl;
            R10InfoLbl.AOWWindow = R10CenterPnl;
            R11CenterPnl.AOWWindow = R11Pnl;
            R11Lbl.AOWWindow = R11CenterPnl;
            R11BehaviorCB.AOWWindow = SidesPnl;
            R11BehaviorCB.VScrollBar = R11BehaviorSB;
            R11BehaviorSB.AOWWindow = SidesPnl;
            R11PlayerIcon.AOWWindow = R11Pnl;
            R11PlayerLbl.AOWWindow = R11CenterPnl;
            R11PlayerCB.AOWWindow = SidesPnl;
            R11PlayerCB.VScrollBar = R11PlayerSB;
            R11PlayerSB.AOWWindow = SidesPnl;
            R11InfoLbl.AOWWindow = R11CenterPnl;
            R12InfoLbl.AOWWindow = R12CenterPnl;
            R12PlayerLbl.AOWWindow = R12CenterPnl;
            R12CenterPnl.AOWWindow = R12Pnl;
            R12Lbl.AOWWindow = R12CenterPnl;
            R12BehaviorCB.AOWWindow = SidesPnl;
            R12BehaviorCB.VScrollBar = R12BehaviorSB;
            R12BehaviorSB.AOWWindow = SidesPnl;
            R12PlayerCB.AOWWindow = SidesPnl;
            R12PlayerCB.VScrollBar = R12PlayerSB;
            R12PlayerSB.AOWWindow = SidesPnl;
            R12PlayerIcon.AOWWindow = R12Pnl;
            PreferredRaceCB.AOWWindow = SettingsPnl;
            PreferredRaceCB.VScrollBar = PreferredRaceSB;
            PreferredRaceSB.AOWWindow = SettingsPnl;
            PreferredRaceLbl.AOWWindow = SettingsPnl;
            LFr.AOWWindow = SettingsPnl;
            CustomizeLeaders.AOWWindow = SettingsPnl;
            CustomizeLeadersLbl.AOWWindow = SettingsPnl;
            R4BehaviorCB.AOWWindow = SidesPnl;
            R4BehaviorCB.VScrollBar = R4BehaviorSB;
            R1FogPnl.AOWWindow = SidesPnl;
            R1PlayerLbl.AOWWindow = R1CenterPnl;
            R2FogPnl.AOWWindow = SidesPnl;
            R3FogPnl.AOWWindow = SidesPnl;
            R4FogPnl.AOWWindow = SidesPnl;
            R8FogPnl.AOWWindow = SidesPnl;
            R7FogPnl.AOWWindow = SidesPnl;
            R6FogPnl.AOWWindow = SidesPnl;
            R5FogPnl.AOWWindow = SidesPnl;
            R12FogPnl.AOWWindow = SidesPnl;
            R11FogPnl.AOWWindow = SidesPnl;
            R10FogPnl.AOWWindow = SidesPnl;
            R9FogPnl.AOWWindow = SidesPnl;
            PreferredRaceSort.AOWWindow = ConnectionPnl;
            PlayerListSort.AOWWindow = ConnectionPnl;
            ConnectionPnl.AOWWindow = ChatPnl;
            PreferredRaceList.AOWWindow = ConnectionPnl;
            PreferredRaceList.VScrollBar = PlayerListSB;
            AOWButton1.AOWWindow = ConnectionPnl;
            PingList.AOWWindow = ConnectionPnl;
            PlayerListSB.AOWWindow = ConnectionPnl;
            SettingsFogPnl.AOWWindow = SettingsPnl;
            ExplorationLbl.AOWWindow = SettingsPnl;
            Exploration.AOWWindow = SettingsPnl;
            ScenarioLbl.AOWWindow = ScenSetupPnl;
            HostNameLbl.AOWWindow = ScenSetupPnl;
            PlayerNameLbl.AOWWindow = ScenSetupPnl;
            FileNameLbl.AOWWindow = ScenSetupPnl;
            MakeHostBtn.AOWWindow = ConnectionPnl;
            RestartSettingsFogPnl.AOWWindow = SettingsPnl;
            ChatFogPnl.AOWWindow = ChatPnl;
            LeadersOnMapCB.AOWWindow = SettingsPnl;
            AOWLabel1.AOWWindow = SettingsPnl;
            AlliedVictoryCB.AOWWindow = SettingsPnl;
            AlliedVictoryLbl.AOWWindow = SettingsPnl;
            AOWVScrollBar1.AOWWindow = SettingsPnl;
            TacticalCombatCB.AOWWindow = SettingsPnl;
            TacticalCombatCB.VScrollBar = AOWVScrollBar1;
            TacticalCombatLbl.AOWWindow = SettingsPnl;
            AlwaysViewTCCB.AOWWindow = SettingsPnl;
            AlwaysViewTCLbl.AOWWindow = SettingsPnl;
            Loaded();
            ChatMemo.Loaded();
            ChatPnl.Loaded();
            ChatSB.Loaded();
            ChatEdit.Loaded();
            SidesPnl.Loaded();
            SettingsPnl.Loaded();
            PlayerList.Loaded();
            EjectBtn.Loaded();
            R1Pnl.Loaded();
            R5Pnl.Loaded();
            R9Pnl.Loaded();
            R2Pnl.Loaded();
            R6Pnl.Loaded();
            R10Pnl.Loaded();
            R3Pnl.Loaded();
            R7Pnl.Loaded();
            R11Pnl.Loaded();
            R4Pnl.Loaded();
            R8Pnl.Loaded();
            R12Pnl.Loaded();
            ExitGame.Loaded();
            PickScenarioBtn.Loaded();
            StartBtn.Loaded();
            MiscLbl.Loaded();
            ScenSetupPnl.Loaded();
            R1PlayerIcon.Loaded();
            R1Lbl.Loaded();
            R1PlayerCB.Loaded();
            R1CenterPnl.Loaded();
            TurnLbl.Loaded();
            DurationLbl.Loaded();
            TurnsLbl.Loaded();
            TurnTimeLbl.Loaded();
            TurnTimeSl.Loaded();
            TurnTimeVal.Loaded();
            DurFr.Loaded();
            TurnsSl.Loaded();
            TurnsVal.Loaded();
            R1BehaviorCB.Loaded();
            R1BehaviorSB.Loaded();
            R1PlayerSB.Loaded();
            PlayModeCB.Loaded();
            TurnVSB.Loaded();
            R1InfoLbl.Loaded();
            R3CenterPnl.Loaded();
            R3Lbl.Loaded();
            R3BehaviorCB.Loaded();
            R3BehaviorSB.Loaded();
            R3PlayerIcon.Loaded();
            R3PlayerLbl.Loaded();
            R3PlayerCB.Loaded();
            R3PlayerSB.Loaded();
            R3InfoLbl.Loaded();
            R2CenterPnl.Loaded();
            R2Lbl.Loaded();
            R2BehaviorCB.Loaded();
            R2BehaviorSB.Loaded();
            R2PlayerIcon.Loaded();
            R2InfoLbl.Loaded();
            R2PlayerCB.Loaded();
            R2PlayerSB.Loaded();
            R2PlayerLbl.Loaded();
            R4InfoLbl.Loaded();
            R4PlayerLbl.Loaded();
            R4CenterPnl.Loaded();
            R4Lbl.Loaded();
            R4BehaviorSB.Loaded();
            R4PlayerCB.Loaded();
            R4PlayerSB.Loaded();
            R4PlayerIcon.Loaded();
            R5PlayerIcon.Loaded();
            R5Lbl.Loaded();
            R5PlayerCB.Loaded();
            R5CenterPnl.Loaded();
            R5BehaviorCB.Loaded();
            R5BehaviorSB.Loaded();
            R5PlayerSB.Loaded();
            R5InfoLbl.Loaded();
            R5PlayerLbl.Loaded();
            R6CenterPnl.Loaded();
            R6Lbl.Loaded();
            R6BehaviorCB.Loaded();
            R6BehaviorSB.Loaded();
            R6PlayerIcon.Loaded();
            R6PlayerLbl.Loaded();
            R6PlayerCB.Loaded();
            R6PlayerSB.Loaded();
            R6InfoLbl.Loaded();
            R7CenterPnl.Loaded();
            R7Lbl.Loaded();
            R7BehaviorCB.Loaded();
            R7BehaviorSB.Loaded();
            R7PlayerIcon.Loaded();
            R7PlayerLbl.Loaded();
            R7PlayerCB.Loaded();
            R7PlayerSB.Loaded();
            R7InfoLbl.Loaded();
            R8InfoLbl.Loaded();
            R8PlayerLbl.Loaded();
            R8CenterPnl.Loaded();
            R8Lbl.Loaded();
            R8BehaviorCB.Loaded();
            R8BehaviorSB.Loaded();
            R8PlayerCB.Loaded();
            R8PlayerSB.Loaded();
            R8PlayerIcon.Loaded();
            R9PlayerIcon.Loaded();
            R9Lbl.Loaded();
            R9PlayerCB.Loaded();
            R9CenterPnl.Loaded();
            R9BehaviorCB.Loaded();
            R9BehaviorSB.Loaded();
            R9PlayerSB.Loaded();
            R9InfoLbl.Loaded();
            R9PlayerLbl.Loaded();
            R10CenterPnl.Loaded();
            R10Lbl.Loaded();
            R10BehaviorCB.Loaded();
            R10BehaviorSB.Loaded();
            R10PlayerIcon.Loaded();
            R10PlayerLbl.Loaded();
            R10PlayerCB.Loaded();
            s10sb2.Loaded();
            R10InfoLbl.Loaded();
            R11CenterPnl.Loaded();
            R11Lbl.Loaded();
            R11BehaviorCB.Loaded();
            R11BehaviorSB.Loaded();
            R11PlayerIcon.Loaded();
            R11PlayerLbl.Loaded();
            R11PlayerCB.Loaded();
            R11PlayerSB.Loaded();
            R11InfoLbl.Loaded();
            R12InfoLbl.Loaded();
            R12PlayerLbl.Loaded();
            R12CenterPnl.Loaded();
            R12Lbl.Loaded();
            R12BehaviorCB.Loaded();
            R12BehaviorSB.Loaded();
            R12PlayerCB.Loaded();
            R12PlayerSB.Loaded();
            R12PlayerIcon.Loaded();
            PreferredRaceCB.Loaded();
            PreferredRaceSB.Loaded();
            PreferredRaceLbl.Loaded();
            LFr.Loaded();
            CustomizeLeaders.Loaded();
            CustomizeLeadersLbl.Loaded();
            R4BehaviorCB.Loaded();
            R1FogPnl.Loaded();
            R1PlayerLbl.Loaded();
            R2FogPnl.Loaded();
            R3FogPnl.Loaded();
            R4FogPnl.Loaded();
            R8FogPnl.Loaded();
            R7FogPnl.Loaded();
            R6FogPnl.Loaded();
            R5FogPnl.Loaded();
            R12FogPnl.Loaded();
            R11FogPnl.Loaded();
            R10FogPnl.Loaded();
            R9FogPnl.Loaded();
            PreferredRaceSort.Loaded();
            PlayerListSort.Loaded();
            ConnectionPnl.Loaded();
            PreferredRaceList.Loaded();
            AOWButton1.Loaded();
            PingList.Loaded();
            PlayerListSB.Loaded();
            SettingsFogPnl.Loaded();
            ExplorationLbl.Loaded();
            Exploration.Loaded();
            ScenarioLbl.Loaded();
            HostNameLbl.Loaded();
            PlayerNameLbl.Loaded();
            FileNameLbl.Loaded();
            MakeHostBtn.Loaded();
            RestartSettingsFogPnl.Loaded();
            ChatFogPnl.Loaded();
            LeadersOnMapCB.Loaded();
            AOWLabel1.Loaded();
            AlliedVictoryCB.Loaded();
            AlliedVictoryLbl.Loaded();
            Translator.Loaded();
            AOWVScrollBar1.Loaded();
            TacticalCombatCB.Loaded();
            TacticalCombatLbl.Loaded();
            AlwaysViewTCCB.Loaded();
            AlwaysViewTCLbl.Loaded();
            session.Resolve(this);
            ChatMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ChatMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ChatMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ChatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChatSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ChatEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SettingsPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SettingsPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayerList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PlayerList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayerList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayerList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EjectBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EjectBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            EjectBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R1Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R1Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R5Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R9Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R2Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R2Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R6Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R10Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R10Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R3Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R7Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R11Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R4Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R4Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R8Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R12Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ExitGame.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ExitGame.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ExitGame.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PickScenarioBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PickScenarioBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PickScenarioBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            StartBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StartBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            StartBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MiscLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ScenSetupPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            ScenSetupPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R1PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R1Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R1PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R1PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R1PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R1PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R1PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TurnLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            DurationLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            TurnsLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            TurnTimeLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnTimeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            TurnTimeSl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnTimeSl.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnTimeSl.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnTimeSl.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnTimeVal.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnTimeVal.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            DurFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TurnsSl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnsSl.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnsSl.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnsSl.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnsVal.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TurnsVal.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R1BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R1BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R1BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R1BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R1BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R1BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayModeCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PlayModeCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PlayModeCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayModeCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayModeCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TurnVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R1InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R3Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R3BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R3BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R3BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R3BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R3PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R3PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R3PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R3PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R3PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R3InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R2Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R2BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R2BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R2BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R2BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R2BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R2BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R2InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R2PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R2PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R2PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R2PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R2PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R2PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R2PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R4BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R4PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R4PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R4PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R4PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R4PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R5PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R5Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R5PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R5PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R5PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R5PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R5BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R5BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R5BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R5InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R5PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R6Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R6BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R6BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R6BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R6BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R6PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R6PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R6PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R6PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R6PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R6InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R7Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R7BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R7BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R7BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R7BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R7PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R7PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R7PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R7PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R7PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R7InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R8InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R8PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R8Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R8BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R8BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R8BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R8BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R8PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R8PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R8PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R8PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R9PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R9Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R9PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R9PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R9PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R9PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R9BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R9BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R9BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R9InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R9PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R10Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R10BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R10BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R10BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R10BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R10BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R10BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R10BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R10BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R10BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R10PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R10PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R10PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R10PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R10PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R10PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R10PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            s10sb2.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            s10sb2.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            s10sb2.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            s10sb2.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R10InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R11Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R11BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R11BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R11BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R11BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            R11PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R11PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R11PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R11PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R11PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R11InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R12InfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R12PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R12Lbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            R12BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R12BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R12BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R12BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12BehaviorSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12BehaviorSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12BehaviorSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12BehaviorSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12PlayerCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R12PlayerCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R12PlayerCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R12PlayerCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12PlayerCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12PlayerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12PlayerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12PlayerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12PlayerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            R12PlayerIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            PreferredRaceCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            PreferredRaceCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PreferredRaceCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PreferredRaceCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PreferredRaceCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PreferredRaceSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PreferredRaceSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PreferredRaceSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PreferredRaceSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PreferredRaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            LFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CustomizeLeaders.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CustomizeLeaders.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            CustomizeLeadersLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CustomizeLeadersLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4BehaviorCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R4BehaviorCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            R4BehaviorCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            R4BehaviorCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R4BehaviorCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R1FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R1PlayerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            R2FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R3FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R4FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R8FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R7FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R6FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R5FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R12FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R11FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R10FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            R9FogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PreferredRaceSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PreferredRaceSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PlayerListSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PlayerListSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PreferredRaceList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PreferredRaceList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PreferredRaceList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PreferredRaceList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AOWButton1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AOWButton1.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PingList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PingList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PingList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PingList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayerListSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerListSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerListSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlayerListSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SettingsFogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ExplorationLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ExplorationLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Exploration.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Exploration.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ScenarioLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            HostNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            PlayerNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            FileNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            MakeHostBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            MakeHostBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            RestartSettingsFogPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChatFogPnl.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LeadersOnMapCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LeadersOnMapCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AOWLabel1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AOWLabel1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            AlliedVictoryCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AlliedVictoryCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AlliedVictoryLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AlliedVictoryLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            AOWVScrollBar1.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TacticalCombatCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TacticalCombatCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            TacticalCombatCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TacticalCombatCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TacticalCombatCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TacticalCombatLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            AlwaysViewTCCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AlwaysViewTCCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AlwaysViewTCLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AlwaysViewTCLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
        }

        private void Layout_ChatSBUpdate_0(TObject p0)
        {
            ChatSBUpdate();
        }

        private void Layout_ChatEditReturn_1(TObject p0)
        {
            ChatEditReturn();
        }

        private void Layout_EjectBtnClick_2(TObject p0)
        {
            EjectBtnClick();
        }

        private void Layout_PickScenarioBtnClick_3(TObject p0)
        {
            PickScenarioBtnClick();
        }

        private void Layout_StartBtnClick_4(TObject p0)
        {
            StartBtnClick();
        }

        private void Layout_ScenSetupPnlUpdate_5(TObject p0)
        {
            ScenSetupPnlUpdate();
        }

        private void Layout_TurnTimeSlChange_6(TObject p0)
        {
            TurnTimeSlChange();
        }

        private void Layout_RaceBehaviorCBChange_7(TObject p0)
        {
            RaceBehaviorCBChange((Aow.Units.AoWComboBox.TAOWComboBox)p0);
        }

        private void Layout_PlayModeCBChange_8(TObject p0)
        {
            PlayModeCBChange();
        }

        private void Layout_PreferredRaceCBChange_9(TObject p0)
        {
            PreferredRaceCBChange();
        }

        private void Layout_PlayerListSBChange_10(TObject p0)
        {
            PlayerListSBChange();
        }

        private void Layout_MakeHostBtnClick_11(TObject p0)
        {
            MakeHostBtnClick();
        }

        private void Layout_TacticalCombatCBChange_12(TObject p0)
        {
            TacticalCombatCBChange();
        }

        private void Layout_AlwaysViewTCCBChange_13(TObject p0)
        {
            AlwaysViewTCCBChange();
        }
    }
}
