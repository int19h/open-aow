// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.FastCombatWin
{
    public partial class TFastCombatWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "FastCombatWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_FastCombatWindowCreate_0;
            LayoutOnDestroy = Layout_FastCombatWindowDestroy_1;
            FCWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "FCWin",
                OnKeyDown = FCWinKeyDown,
                OnUpdate = FCWinUpdate,
                Visible = false,
                Priority = 55,
                WinLeft = 31,
                WinTop = 10,
                WinWidth = 579,
                Masked = true,
                OnHide = Layout_FCWinHide_2,
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
                UseVMem = true,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 7346272,
                OnDrawSurface = FCWinDrawSurface,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTexture = 8,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = true,
                WinHeight = 460,
                Dockable = false
            };
            LayoutSession.EndRead(FCWin);
            LNamePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LNamePnl",
                Priority = 0,
                WinLeft = 45,
                WinTop = 165,
                WinWidth = 120,
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
                    LeftOffset = 45,
                    RightOffset = 0,
                    TopOffset = 165,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LNamePnl);
            LSideLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LSideLbl",
                Priority = 0,
                WinLeft = 36,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 16777215
            };
            LayoutSession.EndRead(LSideLbl);
            RNamePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RNamePnl",
                Priority = 0,
                WinLeft = 414,
                WinTop = 165,
                WinWidth = 120,
                Masked = true,
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
                    LeftOffset = 45,
                    RightOffset = 45,
                    TopOffset = 165,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RNamePnl);
            RSideLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RSideLbl",
                Priority = 0,
                WinLeft = 40,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 16777215
            };
            LayoutSession.EndRead(RSideLbl);
            LCtrlPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LCtrlPnl",
                Priority = 0,
                WinLeft = 74,
                WinTop = 403,
                WinWidth = 120,
                Masked = true,
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
                    LeftOffset = 74,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 22
                },
                WinHeight = 35,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LCtrlPnl);
            RCtrlPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RCtrlPnl",
                Priority = 0,
                WinLeft = 383,
                WinTop = 403,
                WinWidth = 121,
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
                    RightOffset = 75,
                    TopOffset = 0,
                    BottomOffset = 22
                },
                WinHeight = 35,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RCtrlPnl);
            PlayBtn2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "PlayBtn2",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Plays battle from current position"
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 20,
                ILIndexLit = 0,
                ILIndexUp = 19,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 0,
                Checked = false,
                RadioIndex = 0
            };
            LayoutSession.EndRead(PlayBtn2);
            PauseBtn2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "PauseBtn2",
                Priority = 0,
                WinLeft = 48,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Pauses battle at current position"
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
                    LeftOffset = 48,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 18,
                ILIndexLit = 0,
                ILIndexUp = 17,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24,
                Checked = false,
                RadioIndex = 0
            };
            LayoutSession.EndRead(PauseBtn2);
            StartBtn = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "StartBtn",
                Priority = 0,
                WinLeft = 72,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Goes to start of battle."
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 16,
                ILIndexLit = 0,
                ILIndexUp = 15,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 0,
                Checked = false,
                RadioIndex = 0
            };
            LayoutSession.EndRead(StartBtn);
            EndBtn = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "EndBtn",
                Priority = 0,
                WinLeft = 96,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Goes to end of battle, showing final results"
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 10,
                ILIndexLit = 0,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 0,
                Checked = false,
                RadioIndex = 0
            };
            LayoutSession.EndRead(EndBtn);
            CloseBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBut",
                Priority = 0,
                WinLeft = 48,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes Window"
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 31,
                ILIndexLit = 28,
                ILIndexUp = 28,
                Text = "Close",
                TextColor = 9236989,
                TextSelColor = 16777215,
                WinWidth = 73,
                OnClick = Layout_CloseButClick_3
            };
            LayoutSession.EndRead(CloseBut);
            Party1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party1Pnl",
                Priority = 0,
                WinLeft = 200,
                WinTop = 245,
                WinWidth = 179,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 245,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party1Pnl);
            Party1Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party1Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 21,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party1Bg);
            Part1Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Part1Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Part1Dead);
            Party2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party2Pnl",
                Priority = 0,
                WinLeft = 200,
                WinTop = 132,
                WinWidth = 179,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 132,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party2Pnl);
            Party2Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party2Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party2Bg);
            Party2Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party2Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party2Dead);
            Party3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party3Pnl",
                Priority = 0,
                WinLeft = 391,
                WinTop = 187,
                WinWidth = 179,
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
                    LeftOffset = 391,
                    RightOffset = 0,
                    TopOffset = 187,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party3Pnl);
            Party3Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party3Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party3Bg);
            Party3Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party3Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party3Dead);
            Party4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party4Pnl",
                Priority = 0,
                WinLeft = 391,
                WinTop = 300,
                WinWidth = 179,
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
                    LeftOffset = 391,
                    RightOffset = 0,
                    TopOffset = 300,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party4Pnl);
            Party4Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party4Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party4Bg);
            Party4Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party4Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party4Dead);
            Party5Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party5Pnl",
                Priority = 0,
                WinLeft = 200,
                WinTop = 358,
                WinWidth = 179,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 358,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party5Pnl);
            Party5Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party5Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party5Bg);
            Party5Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party5Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party5Dead);
            Party6Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party6Pnl",
                Priority = 0,
                WinLeft = 8,
                WinTop = 300,
                WinWidth = 179,
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 300,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party6Pnl);
            Party6Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party6Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party6Bg);
            Party6Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party6Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party6Dead);
            Party7Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Party7Pnl",
                Priority = 0,
                WinLeft = 8,
                WinTop = 187,
                WinWidth = 179,
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 187,
                    BottomOffset = 0
                },
                WinHeight = 101,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Party7Pnl);
            Party7Bg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party7Bg",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 33,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party7Bg);
            Party7Dead = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Party7Dead",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                CurrentImage = 35,
                AnimEnd = 15,
                WinWidth = 179,
                WinHeight = 101,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Party7Dead);
            LSidePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LSidePnl",
                Priority = 0,
                WinLeft = 11,
                WinTop = 101,
                WinWidth = 175,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 101,
                    BottomOffset = 0
                },
                WinHeight = 58,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LSidePnl);
            RSidePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RSidePnl",
                Priority = 0,
                WinLeft = 393,
                WinTop = 101,
                WinWidth = 175,
                Masked = true,
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
                    RightOffset = 11,
                    TopOffset = 101,
                    BottomOffset = 0
                },
                WinHeight = 58,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RSidePnl);
            FightPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "FightPnl",
                Priority = 0,
                WinLeft = 234,
                WinTop = 25,
                WinWidth = 110,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                WinHeight = 105,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(FightPnl);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Visible = false,
                Priority = 3000,
                WinLeft = 15,
                WinTop = 105,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 38,
                WinWidth = 40,
                WinHeight = 53,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LShield);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Visible = false,
                Priority = 3000,
                WinLeft = 525,
                WinTop = 105,
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
                    LeftOffset = 14,
                    RightOffset = 14,
                    TopOffset = 105,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 38,
                WinWidth = 40,
                WinHeight = 53,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RShield);
            InitialView = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "InitialView",
                Priority = 0,
                WinLeft = 72,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Goes to start of battle."
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
                    LeftOffset = 72,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 16,
                ILIndexLit = 15,
                ILIndexUp = 15,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24,
                OnClick = Layout_InitialViewClick_4
            };
            LayoutSession.EndRead(InitialView);
            FinalView = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "FinalView",
                Priority = 0,
                WinLeft = 96,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Goes to end of battle, showing final results"
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
                    LeftOffset = 96,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 10,
                ILIndexLit = 9,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24,
                OnClick = Layout_FinalViewClick_5
            };
            LayoutSession.EndRead(FinalView);
            PlayBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PlayBtn",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Plays battle from current position"
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 20,
                ILIndexLit = 19,
                ILIndexUp = 19,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 48,
                OnClick = Layout_PlayBtnClick_6
            };
            LayoutSession.EndRead(PlayBtn);
            PauseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PauseBtn",
                Priority = 0,
                WinLeft = 48,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Plays battle from current position"
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
                    LeftOffset = 48,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 18,
                ILIndexLit = 17,
                ILIndexUp = 17,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24,
                OnClick = Layout_PauseBtnClick_7
            };
            LayoutSession.EndRead(PauseBtn);
            SequencePositionSld = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "SequencePositionSld",
                Priority = 0,
                WinLeft = 40,
                WinTop = 21,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 21,
                    BottomOffset = 32
                },
                Masked = false,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 10,
                Position = 0,
                WinHeight = 11,
                WinWidth = 40,
                NumberVisible = 1,
                OnChange = Layout_SequencePositionSldChange_8,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 25,
                ILIndexTab = 28
            };
            LayoutSession.EndRead(SequencePositionSld);
            SequenceSpeedSld = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "SequenceSpeedSld",
                Priority = 0,
                WinLeft = 40,
                WinTop = 21,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 21,
                    BottomOffset = 32
                },
                Masked = false,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 10,
                Position = 15,
                WinHeight = 11,
                WinWidth = 40,
                NumberVisible = 1,
                OnChange = Layout_SequenceSpeedSldChange_9,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 25,
                ILIndexTab = 28
            };
            LayoutSession.EndRead(SequenceSpeedSld);
            GlowAnimation = new Aow.Units.ImageLib.TImageLibrary(this)
            {
                Name = "GlowAnimation",
                LoadOnDemand = true,
                FileName = "Int\\FCWin.ILB"
            };
            LayoutSession.EndRead(GlowAnimation);
            GlowImage = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "GlowImage",
                Priority = 0,
                WinLeft = 234,
                WinTop = 23,
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
                    LeftOffset = 234,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 40,
                CurrentImage = 46,
                AnimEnd = 49,
                WinWidth = 107,
                WinHeight = 86,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(GlowImage);
            PrevSeqBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PrevSeqBtn",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Plays battle from current position"
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 12,
                ILIndexLit = 0,
                ILIndexUp = 11,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24
            };
            LayoutSession.EndRead(PrevSeqBtn);
            NextSeqBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NextSeqBtn",
                Visible = false,
                Priority = 0,
                WinLeft = 24,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Plays battle from current position"
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
                    LeftOffset = 24,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 14,
                ILIndexLit = 0,
                ILIndexUp = 13,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 24
            };
            LayoutSession.EndRead(NextSeqBtn);
            StartLBl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StartLBl",
                Priority = 0,
                WinLeft = 14,
                WinTop = 22,
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
                    RightOffset = 84,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = true,
                Text = "start",
                TextColor = 9872276
            };
            LayoutSession.EndRead(StartLBl);
            EndLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "EndLbl",
                Priority = 0,
                WinLeft = 84,
                WinTop = 22,
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
                    LeftOffset = 84,
                    RightOffset = 84,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = true,
                Text = "end",
                TextColor = 9872276
            };
            LayoutSession.EndRead(EndLbl);
            SlowLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SlowLbl",
                Priority = 0,
                WinLeft = 18,
                WinTop = 22,
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
                    RightOffset = 84,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = true,
                Text = "slow",
                TextColor = 9872276
            };
            LayoutSession.EndRead(SlowLbl);
            FastLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "FastLbl",
                Priority = 0,
                WinLeft = 84,
                WinTop = 22,
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
                    LeftOffset = 84,
                    RightOffset = 84,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = true,
                Text = "fast",
                TextColor = 9872276
            };
            LayoutSession.EndRead(FastLbl);
            ModeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ModeLbl",
                Priority = 0,
                WinLeft = 269,
                WinTop = 115,
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
                    TopOffset = 115,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ModeLbl);
            LShieldUser = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "LShieldUser",
                OnDraw = LShieldUserDraw,
                Priority = 3000,
                WinLeft = 15,
                WinTop = 105,
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
                Masked = true,
                WinWidth = 40,
                WinHeight = 53
            };
            LayoutSession.EndRead(LShieldUser);
            RShieldUser = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "RShieldUser",
                OnDraw = RShieldUserDraw,
                Priority = 3000,
                WinLeft = 525,
                WinTop = 105,
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
                Masked = true,
                WinWidth = 40,
                WinHeight = 53
            };
            LayoutSession.EndRead(RShieldUser);
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
            LNamePnl.AOWWindow = FCWin;
            LSideLbl.AOWWindow = LNamePnl;
            RNamePnl.AOWWindow = FCWin;
            RSideLbl.AOWWindow = RNamePnl;
            LCtrlPnl.AOWWindow = FCWin;
            RCtrlPnl.AOWWindow = FCWin;
            PauseBtn2.AOWWindow = LCtrlPnl;
            CloseBut.AOWWindow = RCtrlPnl;
            Party1Pnl.AOWWindow = FCWin;
            Party1Bg.AOWWindow = Party1Pnl;
            Part1Dead.AOWWindow = Party1Pnl;
            Party2Pnl.AOWWindow = FCWin;
            Party2Bg.AOWWindow = Party2Pnl;
            Party2Dead.AOWWindow = Party2Pnl;
            Party3Pnl.AOWWindow = FCWin;
            Party3Bg.AOWWindow = Party3Pnl;
            Party3Dead.AOWWindow = Party3Pnl;
            Party4Pnl.AOWWindow = FCWin;
            Party4Bg.AOWWindow = Party4Pnl;
            Party4Dead.AOWWindow = Party4Pnl;
            Party5Pnl.AOWWindow = FCWin;
            Party5Bg.AOWWindow = Party5Pnl;
            Party5Dead.AOWWindow = Party5Pnl;
            Party6Pnl.AOWWindow = FCWin;
            Party6Bg.AOWWindow = Party6Pnl;
            Party6Dead.AOWWindow = Party6Pnl;
            Party7Pnl.AOWWindow = FCWin;
            Party7Bg.AOWWindow = Party7Pnl;
            Party7Dead.AOWWindow = Party7Pnl;
            LSidePnl.AOWWindow = FCWin;
            RSidePnl.AOWWindow = FCWin;
            FightPnl.AOWWindow = FCWin;
            LShield.AOWWindow = FCWin;
            RShield.AOWWindow = FCWin;
            InitialView.AOWWindow = LCtrlPnl;
            FinalView.AOWWindow = LCtrlPnl;
            PlayBtn.AOWWindow = LCtrlPnl;
            PauseBtn.AOWWindow = LCtrlPnl;
            SequencePositionSld.AOWWindow = LCtrlPnl;
            SequenceSpeedSld.AOWWindow = RCtrlPnl;
            GlowImage.AOWWindow = FCWin;
            GlowImage.ImageLib = GlowAnimation;
            PrevSeqBtn.AOWWindow = RCtrlPnl;
            NextSeqBtn.AOWWindow = RCtrlPnl;
            StartLBl.AOWWindow = LCtrlPnl;
            EndLbl.AOWWindow = LCtrlPnl;
            SlowLbl.AOWWindow = RCtrlPnl;
            FastLbl.AOWWindow = RCtrlPnl;
            ModeLbl.AOWWindow = FCWin;
            LShieldUser.AOWWindow = FCWin;
            RShieldUser.AOWWindow = FCWin;
            Loaded();
            FCWin.Loaded();
            LNamePnl.Loaded();
            LSideLbl.Loaded();
            RNamePnl.Loaded();
            RSideLbl.Loaded();
            LCtrlPnl.Loaded();
            RCtrlPnl.Loaded();
            PlayBtn2.Loaded();
            PauseBtn2.Loaded();
            StartBtn.Loaded();
            EndBtn.Loaded();
            CloseBut.Loaded();
            Party1Pnl.Loaded();
            Party1Bg.Loaded();
            Part1Dead.Loaded();
            Party2Pnl.Loaded();
            Party2Bg.Loaded();
            Party2Dead.Loaded();
            Party3Pnl.Loaded();
            Party3Bg.Loaded();
            Party3Dead.Loaded();
            Party4Pnl.Loaded();
            Party4Bg.Loaded();
            Party4Dead.Loaded();
            Party5Pnl.Loaded();
            Party5Bg.Loaded();
            Party5Dead.Loaded();
            Party6Pnl.Loaded();
            Party6Bg.Loaded();
            Party6Dead.Loaded();
            Party7Pnl.Loaded();
            Party7Bg.Loaded();
            Party7Dead.Loaded();
            LSidePnl.Loaded();
            RSidePnl.Loaded();
            FightPnl.Loaded();
            LShield.Loaded();
            RShield.Loaded();
            InitialView.Loaded();
            FinalView.Loaded();
            PlayBtn.Loaded();
            PauseBtn.Loaded();
            SequencePositionSld.Loaded();
            SequenceSpeedSld.Loaded();
            GlowAnimation.Loaded();
            GlowImage.Loaded();
            PrevSeqBtn.Loaded();
            NextSeqBtn.Loaded();
            StartLBl.Loaded();
            EndLbl.Loaded();
            SlowLbl.Loaded();
            FastLbl.Loaded();
            ModeLbl.Loaded();
            LShieldUser.Loaded();
            RShieldUser.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            FCWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            FCWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            FCWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            LSideLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            RSideLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PlayBtn2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PlayBtn2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            PauseBtn2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PauseBtn2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            StartBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StartBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            EndBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EndBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            CloseBut.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CloseBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            CloseBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Party1Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Part1Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party2Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party2Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party3Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party3Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party4Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party4Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party5Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party5Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party6Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party6Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party7Bg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            Party7Dead.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            InitialView.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            InitialView.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            FinalView.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            FinalView.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            PlayBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PlayBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            PauseBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PauseBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            SequencePositionSld.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SequencePositionSld.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SequenceSpeedSld.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SequenceSpeedSld.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PrevSeqBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PrevSeqBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            NextSeqBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NextSeqBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.FCWin;
            StartLBl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            EndLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            SlowLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            FastLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ModeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_FastCombatWindowCreate_0(TObject p0)
        {
            FastCombatWindowCreate();
        }

        private void Layout_FastCombatWindowDestroy_1(TObject p0)
        {
            FastCombatWindowDestroy();
        }

        private void Layout_FCWinHide_2(TObject p0)
        {
            FCWinHide();
        }

        private void Layout_CloseButClick_3(TObject p0)
        {
            CloseButClick();
        }

        private void Layout_InitialViewClick_4(TObject p0)
        {
            InitialViewClick();
        }

        private void Layout_FinalViewClick_5(TObject p0)
        {
            FinalViewClick();
        }

        private void Layout_PlayBtnClick_6(TObject p0)
        {
            PlayBtnClick();
        }

        private void Layout_PauseBtnClick_7(TObject p0)
        {
            PauseBtnClick();
        }

        private void Layout_SequencePositionSldChange_8(TObject p0)
        {
            SequencePositionSldChange();
        }

        private void Layout_SequenceSpeedSldChange_9(TObject p0)
        {
            SequenceSpeedSldChange();
        }
    }
}
