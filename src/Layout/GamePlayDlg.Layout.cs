// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWGamePlayDlg
{
    public partial class TGamePlayDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "GamePlayDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            GamePlayDlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "GamePlayDlg",
                OnKeyDown = GamePlayDlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 150,
                WinTop = 71,
                WinWidth = 340,
                Masked = true,
                OnShow = GamePlayDlgShow,
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
                UseVMem = false,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 5808200,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 290,
                Dockable = false
            };
            LayoutSession.EndRead(GamePlayDlg);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 250,
                WinLeft = 202,
                WinTop = 253,
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
                    LeftOffset = 266,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = CancelClick
            };
            LayoutSession.EndRead(CancelBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 253,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 298,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 0,
                WinLeft = 42,
                WinTop = 7,
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
                    LeftOffset = 42,
                    RightOffset = 0,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "GamePlay Settings",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 305,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes the window."
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
                    RightOffset = 20,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 1,
                ILIndexLit = 2,
                ILIndexUp = 0,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = CancelClick
            };
            LayoutSession.EndRead(Close);
            TCeltLR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLR",
                Priority = 100,
                WinLeft = 258,
                WinTop = 253,
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
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLR);
            TCeltLL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 253,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLL);
            TitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TitleIcon",
                Priority = 0,
                WinLeft = 20,
                WinTop = 5,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            AutoSaveCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "AutoSaveCB",
                Priority = 0,
                WinLeft = 20,
                WinTop = 16,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether to automatically save the game after each turn."
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 16,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(AutoSaveCB);
            asLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "asLbl",
                Priority = 0,
                WinLeft = 40,
                WinTop = 17,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 17,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Auto-Save",
                TextColor = 9415072
            };
            LayoutSession.EndRead(asLbl);
            SkipIntroCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "SkipIntroCB",
                Priority = 0,
                WinLeft = 140,
                WinTop = 16,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether to skip the introductory movie when the game starts."
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
                    LeftOffset = 140,
                    RightOffset = 120,
                    TopOffset = 16,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(SkipIntroCB);
            siLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "siLbl",
                Priority = 0,
                WinLeft = 160,
                WinTop = 17,
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
                    LeftOffset = 160,
                    RightOffset = 50,
                    TopOffset = 17,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Skip Intro",
                TextColor = 9415072
            };
            LayoutSession.EndRead(siLbl);
            Tab1 = new Aow.Units.AoWTabPanel.TAOWTabPanel(this)
            {
                Name = "Tab1",
                Priority = 3000,
                WinLeft = 20,
                WinTop = 23,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                LayoutItems = new LayoutStrings()
                {
                    "General",
                    "Events"
                },
                BorderHeight = 6,
                Selected = 0,
                WinWidth = 300,
                TextColor = 0,
                TextSelColor = 9415072,
                ILIndexDown = 67,
                ILIndexUp = 70,
                BorderHeightSelected = 7
            };
            LayoutSession.EndRead(Tab1);
            Pnl1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Pnl1",
                Priority = 0,
                WinLeft = 10,
                WinTop = 40,
                WinWidth = 320,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 37
                },
                WinHeight = 213,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Pnl1);
            Pnl2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Pnl2",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 40,
                WinWidth = 320,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 37
                },
                WinHeight = 213,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Pnl2);
            GamePasswordLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GamePasswordLbl",
                Priority = 0,
                WinLeft = 114,
                WinTop = 85,
                CaptureInput = false,
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 85,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Game password:",
                TextColor = 0
            };
            LayoutSession.EndRead(GamePasswordLbl);
            PasswordEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "PasswordEdit",
                Priority = 10,
                WinLeft = 87,
                WinTop = 100,
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
                    LeftOffset = 33,
                    RightOffset = 140,
                    TopOffset = 100,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 147,
                BorderWidth = 8,
                BorderHeight = 6,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = false,
                ILIndex = 15,
                PassWordChar = (byte)42
            };
            LayoutSession.EndRead(PasswordEdit);
            CombatResolveModeCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "CombatResolveModeCB",
                Tag = 8,
                Priority = 170,
                WinLeft = 125,
                WinTop = 20,
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
                    LeftOffset = 125,
                    RightOffset = 25,
                    TopOffset = 20,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 170,
                BorderWidth = 7,
                BorderHeight = 5,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 5,
                Selected = -1,
                PriorityOpened = 5000,
                TextColor = 11002593,
                TextSelColor = 0,
                ILEIndex = 27,
                ILFIndex = 113,
                ILTnIndex = 17,
                ILTsIndex = 8,
                ItemHeight = 17
            };
            LayoutSession.EndRead(CombatResolveModeCB);
            CombatResolveModeSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "CombatResolveModeSB",
                Visible = false,
                Priority = 170,
                WinLeft = 0,
                WinTop = 0,
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
                Max = 3,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 70,
                WinWidth = 17,
                NumberVisible = 3,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(CombatResolveModeSB);
            Classic1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic1CB",
                Priority = 0,
                WinLeft = 40,
                WinTop = 35,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 35,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic1CB);
            PopupLbl1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl1",
                Priority = 0,
                WinLeft = 150,
                WinTop = 35,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Combat",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl1);
            Classic2CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic2CB",
                Tag = 1,
                Priority = 0,
                WinLeft = 40,
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
                    LeftOffset = 40,
                    RightOffset = 120,
                    TopOffset = 55,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic2CB);
            PopupLbl2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl2",
                Priority = 0,
                WinLeft = 150,
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
                    LeftOffset = 150,
                    RightOffset = 50,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Diplomacy",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl2);
            Classic3CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic3CB",
                Tag = 2,
                Priority = 0,
                WinLeft = 40,
                WinTop = 75,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 75,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic3CB);
            PopupLbl3 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl3",
                Priority = 0,
                WinLeft = 150,
                WinTop = 75,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 75,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Spell researched",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl3);
            Classic4CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic4CB",
                Tag = 3,
                Priority = 0,
                WinLeft = 40,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 95,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic4CB);
            Classic5CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic5CB",
                Tag = 4,
                Priority = 0,
                WinLeft = 40,
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
                    LeftOffset = 40,
                    RightOffset = 115,
                    TopOffset = 115,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic5CB);
            Classic6CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic6CB",
                Tag = 5,
                Priority = 0,
                WinLeft = 40,
                WinTop = 135,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 135,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic6CB);
            WindowPopupLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WindowPopupLbl",
                Priority = 0,
                WinLeft = 150,
                WinTop = 15,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Window Popup",
                TextColor = 16777215
            };
            LayoutSession.EndRead(WindowPopupLbl);
            ClassicLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ClassicLbl",
                Priority = 0,
                WinLeft = 20,
                WinTop = 15,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Classic",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ClassicLbl);
            SimultaneousLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SimultaneousLbl",
                Priority = 0,
                WinLeft = 80,
                WinTop = 15,
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Simult.",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SimultaneousLbl);
            Sim1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim1CB",
                Priority = 0,
                WinLeft = 95,
                WinTop = 35,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 35,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim1CB);
            Sim2CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim2CB",
                Tag = 1,
                Priority = 0,
                WinLeft = 95,
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
                    LeftOffset = 95,
                    RightOffset = 120,
                    TopOffset = 55,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim2CB);
            Sim3CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim3CB",
                Tag = 2,
                Priority = 0,
                WinLeft = 95,
                WinTop = 75,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 75,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim3CB);
            Sim4CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim4CB",
                Tag = 3,
                Priority = 0,
                WinLeft = 95,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 95,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim4CB);
            Sim5CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim5CB",
                Tag = 4,
                Priority = 0,
                WinLeft = 95,
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
                    LeftOffset = 95,
                    RightOffset = 115,
                    TopOffset = 115,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim5CB);
            Sim6CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim6CB",
                Tag = 5,
                Priority = 0,
                WinLeft = 95,
                WinTop = 135,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 135,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim6CB);
            PopupLbl4 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl4",
                Priority = 0,
                WinLeft = 150,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 95,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Spell ready",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl4);
            PopupLbl5 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl5",
                Priority = 0,
                WinLeft = 150,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 115,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Rebellion/desertion",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl5);
            PopupLbl6 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl6",
                Priority = 0,
                WinLeft = 150,
                WinTop = 135,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 135,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Party damage",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl6);
            Classic7CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic7CB",
                Tag = 6,
                Priority = 0,
                WinLeft = 40,
                WinTop = 155,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 155,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic7CB);
            Sim7CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim7CB",
                Tag = 6,
                Priority = 0,
                WinLeft = 95,
                WinTop = 155,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 155,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim7CB);
            PopupLbl7 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl7",
                Priority = 0,
                WinLeft = 150,
                WinTop = 155,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Hero upgrade",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl7);
            ChangePasswordBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ChangePasswordBtn",
                Priority = 99,
                WinLeft = 115,
                WinTop = 125,
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
                    LeftOffset = 265,
                    RightOffset = 35,
                    TopOffset = 125,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Change",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 90,
                OnClick = Layout_ChangePasswordBtnClick_0
            };
            LayoutSession.EndRead(ChangePasswordBtn);
            MovementSpeedCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "MovementSpeedCB",
                Tag = 8,
                Priority = 170,
                WinLeft = 125,
                WinTop = 50,
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
                    LeftOffset = 125,
                    RightOffset = 25,
                    TopOffset = 50,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 170,
                LayoutItems = new LayoutStrings()
                {
                    "Normal",
                    "Fast",
                    "Very fast"
                },
                BorderWidth = 7,
                BorderHeight = 5,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 5,
                Selected = -1,
                PriorityOpened = 170,
                TextColor = 11002593,
                TextSelColor = 0,
                ILEIndex = 27,
                ILFIndex = 113,
                ILTnIndex = 17,
                ILTsIndex = 8,
                ItemHeight = 17
            };
            LayoutSession.EndRead(MovementSpeedCB);
            OKBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn",
                Priority = 250,
                WinLeft = 29,
                WinTop = 253,
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
                    LeftOffset = 29,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "OK",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 110,
                OnClick = OKBtnClick
            };
            LayoutSession.EndRead(OKBtn);
            Classic8CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Classic8CB",
                Tag = 6,
                Priority = 0,
                WinLeft = 40,
                WinTop = 175,
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
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 175,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Classic8CB);
            Sim8CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Sim8CB",
                Tag = 6,
                Priority = 0,
                WinLeft = 95,
                WinTop = 175,
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
                    LeftOffset = 95,
                    RightOffset = 0,
                    TopOffset = 175,
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
                RadioIndex = 0
            };
            LayoutSession.EndRead(Sim8CB);
            PopupLbl8 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopupLbl8",
                Priority = 0,
                WinLeft = 150,
                WinTop = 175,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 175,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Hero join",
                TextColor = 0
            };
            LayoutSession.EndRead(PopupLbl8);
            CombLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CombLbl",
                Priority = 0,
                WinLeft = 7,
                WinTop = 5,
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
                    LeftOffset = 7,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Combat",
                TextColor = 9415072
            };
            LayoutSession.EndRead(CombLbl);
            CombFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CombFr",
                Priority = 10,
                WinLeft = 26,
                WinTop = 20,
                WinWidth = 100,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CombFr);
            CombPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CombPnl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 20,
                WinWidth = 270,
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
                    LeftOffset = 25,
                    RightOffset = 25,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CombPnl);
            Movelbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Movelbl",
                Priority = 0,
                WinLeft = 7,
                WinTop = 5,
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
                    LeftOffset = 7,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Movement",
                TextColor = 9415072
            };
            LayoutSession.EndRead(Movelbl);
            MovFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MovFr",
                Priority = 10,
                WinLeft = 26,
                WinTop = 50,
                WinWidth = 100,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MovFr);
            BtmPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "BtmPnl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 160,
                WinWidth = 270,
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
                    LeftOffset = 25,
                    RightOffset = 25,
                    TopOffset = 160,
                    BottomOffset = 0
                },
                WinHeight = 42,
                ILIndexFrame = 121,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(BtmPnl);
            MovPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MovPnl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 50,
                WinWidth = 270,
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
                    LeftOffset = 25,
                    RightOffset = 25,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MovPnl);
            AOWPanel1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AOWPanel1",
                Priority = 0,
                WinLeft = 86,
                WinTop = 100,
                WinWidth = 148,
                Masked = true,
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
                    LeftOffset = 33,
                    RightOffset = 140,
                    TopOffset = 100,
                    BottomOffset = 1
                },
                WinHeight = 22,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AOWPanel1);
            CeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltLineL",
                Priority = 1000,
                WinLeft = 143,
                WinTop = 253,
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
                    LeftOffset = 27,
                    RightOffset = 27,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltLineL);
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
            CancelBtn.AOWWindow = GamePlayDlg;
            BCeltL.AOWWindow = GamePlayDlg;
            Title.AOWWindow = GamePlayDlg;
            Close.AOWWindow = GamePlayDlg;
            TCeltLR.AOWWindow = GamePlayDlg;
            TCeltLL.AOWWindow = GamePlayDlg;
            TitleIcon.AOWWindow = GamePlayDlg;
            AutoSaveCB.AOWWindow = BtmPnl;
            asLbl.AOWWindow = BtmPnl;
            SkipIntroCB.AOWWindow = BtmPnl;
            siLbl.AOWWindow = BtmPnl;
            Tab1.AOWWindow = GamePlayDlg;
            Pnl1.AOWWindow = GamePlayDlg;
            Pnl1.TabPanel = Tab1;
            Pnl2.AOWWindow = GamePlayDlg;
            Pnl2.TabPanel = Tab1;
            GamePasswordLbl.AOWWindow = Pnl1;
            PasswordEdit.AOWWindow = Pnl1;
            CombatResolveModeCB.AOWWindow = Pnl1;
            CombatResolveModeSB.AOWWindow = Pnl1;
            Classic1CB.AOWWindow = Pnl2;
            PopupLbl1.AOWWindow = Pnl2;
            Classic2CB.AOWWindow = Pnl2;
            PopupLbl2.AOWWindow = Pnl2;
            Classic3CB.AOWWindow = Pnl2;
            PopupLbl3.AOWWindow = Pnl2;
            Classic4CB.AOWWindow = Pnl2;
            Classic5CB.AOWWindow = Pnl2;
            Classic6CB.AOWWindow = Pnl2;
            WindowPopupLbl.AOWWindow = Pnl2;
            ClassicLbl.AOWWindow = Pnl2;
            SimultaneousLbl.AOWWindow = Pnl2;
            Sim1CB.AOWWindow = Pnl2;
            Sim2CB.AOWWindow = Pnl2;
            Sim3CB.AOWWindow = Pnl2;
            Sim4CB.AOWWindow = Pnl2;
            Sim5CB.AOWWindow = Pnl2;
            Sim6CB.AOWWindow = Pnl2;
            PopupLbl4.AOWWindow = Pnl2;
            PopupLbl5.AOWWindow = Pnl2;
            PopupLbl6.AOWWindow = Pnl2;
            Classic7CB.AOWWindow = Pnl2;
            Sim7CB.AOWWindow = Pnl2;
            PopupLbl7.AOWWindow = Pnl2;
            ChangePasswordBtn.AOWWindow = Pnl1;
            MovementSpeedCB.AOWWindow = Pnl1;
            OKBtn.AOWWindow = GamePlayDlg;
            Classic8CB.AOWWindow = Pnl2;
            Sim8CB.AOWWindow = Pnl2;
            PopupLbl8.AOWWindow = Pnl2;
            CombLbl.AOWWindow = CombFr;
            CombFr.AOWWindow = Pnl1;
            CombPnl.AOWWindow = Pnl1;
            Movelbl.AOWWindow = MovFr;
            MovFr.AOWWindow = Pnl1;
            BtmPnl.AOWWindow = Pnl1;
            MovPnl.AOWWindow = Pnl1;
            AOWPanel1.AOWWindow = Pnl1;
            CeltLineL.AOWWindow = GamePlayDlg;
            Loaded();
            GamePlayDlg.Loaded();
            CancelBtn.Loaded();
            BCeltL.Loaded();
            Title.Loaded();
            Close.Loaded();
            TCeltLR.Loaded();
            TCeltLL.Loaded();
            TitleIcon.Loaded();
            AutoSaveCB.Loaded();
            asLbl.Loaded();
            SkipIntroCB.Loaded();
            siLbl.Loaded();
            Tab1.Loaded();
            Pnl1.Loaded();
            Pnl2.Loaded();
            GamePasswordLbl.Loaded();
            PasswordEdit.Loaded();
            CombatResolveModeCB.Loaded();
            CombatResolveModeSB.Loaded();
            Classic1CB.Loaded();
            PopupLbl1.Loaded();
            Classic2CB.Loaded();
            PopupLbl2.Loaded();
            Classic3CB.Loaded();
            PopupLbl3.Loaded();
            Classic4CB.Loaded();
            Classic5CB.Loaded();
            Classic6CB.Loaded();
            WindowPopupLbl.Loaded();
            ClassicLbl.Loaded();
            SimultaneousLbl.Loaded();
            Sim1CB.Loaded();
            Sim2CB.Loaded();
            Sim3CB.Loaded();
            Sim4CB.Loaded();
            Sim5CB.Loaded();
            Sim6CB.Loaded();
            PopupLbl4.Loaded();
            PopupLbl5.Loaded();
            PopupLbl6.Loaded();
            Classic7CB.Loaded();
            Sim7CB.Loaded();
            PopupLbl7.Loaded();
            ChangePasswordBtn.Loaded();
            MovementSpeedCB.Loaded();
            OKBtn.Loaded();
            Classic8CB.Loaded();
            Sim8CB.Loaded();
            PopupLbl8.Loaded();
            CombLbl.Loaded();
            CombFr.Loaded();
            CombPnl.Loaded();
            Movelbl.Loaded();
            MovFr.Loaded();
            BtmPnl.Loaded();
            MovPnl.Loaded();
            AOWPanel1.Loaded();
            CeltLineL.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            GamePlayDlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            GamePlayDlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GamePlayDlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TCeltLR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            AutoSaveCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AutoSaveCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            asLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SkipIntroCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SkipIntroCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            siLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Tab1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Tab1.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            GamePasswordLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PasswordEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PasswordEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            CombatResolveModeCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CombatResolveModeCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            CombatResolveModeCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CombatResolveModeCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CombatResolveModeCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            CombatResolveModeSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            Classic1CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Classic2CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Classic3CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Classic4CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Classic5CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Classic6CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            WindowPopupLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ClassicLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SimultaneousLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Sim1CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim2CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim3CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim4CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim5CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim6CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl4.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PopupLbl5.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PopupLbl6.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Classic7CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim7CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl7.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ChangePasswordBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ChangePasswordBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MovementSpeedCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MovementSpeedCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            MovementSpeedCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MovementSpeedCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MovementSpeedCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            OKBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Classic8CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sim8CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopupLbl8.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CombLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CombLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CombFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CombFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CombPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CombPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Movelbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Movelbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MovFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MovFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            BtmPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            BtmPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MovPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MovPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AOWPanel1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AOWPanel1.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
        }

        private void Layout_ChangePasswordBtnClick_0(TObject p0)
        {
            ChangePasswordBtnClick();
        }
    }
}
