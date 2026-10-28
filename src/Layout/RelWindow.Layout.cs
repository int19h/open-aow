// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWRelations
{
    public partial class TRelWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "RelWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_RelWindowCreate_0;
            LayoutOnDestroy = Layout_RelWindowDestroy_1;
            RelWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "RelWin",
                OnKeyDown = RelWinKeyDown,
                Visible = false,
                Priority = 55,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                OnHide = Layout_RelWinHide_2,
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
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTexture = 41,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 432,
                Dockable = false
            };
            LayoutSession.EndRead(RelWin);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Priority = 200,
                WinLeft = 363,
                WinTop = 55,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 34,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 27,
                AnimEnd = 38,
                WinWidth = 44,
                WinHeight = 60,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RShield);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Priority = 200,
                WinLeft = 209,
                WinTop = 55,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 34,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 33,
                AnimEnd = 38,
                WinWidth = 44,
                WinHeight = 60,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LShield);
            MExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MExitBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Exits diplomacy"
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
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 180,
                OnClick = Layout_ExitClick_3
            };
            LayoutSession.EndRead(MExitBtn);
            RelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RelPnl",
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
                WinHeight = 432,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RelPnl);
            RIncomingPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RIncomingPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 416,
                WinTop = 0,
                WinWidth = 200,
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
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RIncomingPnl);
            MAcceptBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MAcceptBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 116,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Agrees to the offer."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 32
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Accept",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_MAcceptBtnClick_4
            };
            LayoutSession.EndRead(MAcceptBtn);
            MRejectBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MRejectBtn",
                Priority = 1000,
                WinLeft = 104,
                WinTop = 116,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Rejects the offer."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 105,
                    BottomOffset = 32
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Reject",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_MRejectBtnClick_5
            };
            LayoutSession.EndRead(MRejectBtn);
            MScroll = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "MScroll",
                Visible = false,
                Priority = 9999,
                WinLeft = 177,
                WinTop = 50,
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
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 3,
                Position = 0,
                WinHeight = 63,
                WinWidth = 13,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 42,
                ILIndexIncDown = 43,
                ILIndexDecUp = 40,
                ILIndexDecDown = 41,
                ILIndexBack = 47,
                ILIndexTab = 44
            };
            LayoutSession.EndRead(MScroll);
            P1I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P1I",
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 14,
                WinTop = 74,
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P1I);
            P2I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P2I",
                Tag = 1,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 65,
                WinTop = 74,
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
                    LeftOffset = 65,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P2I);
            P3I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P3I",
                Tag = 2,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 116,
                WinTop = 74,
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
                    LeftOffset = 116,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P3I);
            P4I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P4I",
                Tag = 3,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 167,
                WinTop = 74,
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
                    LeftOffset = 167,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P4I);
            P5I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P5I",
                Tag = 4,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 218,
                WinTop = 74,
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
                    LeftOffset = 218,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P5I);
            P6I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P6I",
                Tag = 5,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 269,
                WinTop = 74,
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
                    LeftOffset = 269,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P6I);
            P7I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P7I",
                Tag = 6,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 320,
                WinTop = 74,
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
                    LeftOffset = 320,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P7I);
            P8I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P8I",
                Tag = 7,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 371,
                WinTop = 74,
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
                    LeftOffset = 371,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P8I);
            P9I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P9I",
                Tag = 8,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 422,
                WinTop = 74,
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
                    LeftOffset = 422,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P9I);
            P10I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P10I",
                Tag = 9,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 473,
                WinTop = 74,
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
                    LeftOffset = 473,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P10I);
            P11I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P11I",
                Tag = 10,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 524,
                WinTop = 74,
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
                    LeftOffset = 524,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P11I);
            P12I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "P12I",
                Tag = 11,
                OnDraw = P1IDraw,
                Priority = 600,
                WinLeft = 575,
                WinTop = 74,
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
                    LeftOffset = 575,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(P12I);
            R1I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R1I",
                Tag = 20,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 14,
                WinTop = 126,
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R1I);
            R2I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R2I",
                Tag = 21,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 65,
                WinTop = 126,
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
                    LeftOffset = 65,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R2I);
            R3I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R3I",
                Tag = 22,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 116,
                WinTop = 126,
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
                    LeftOffset = 116,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R3I);
            R4I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R4I",
                Tag = 23,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 167,
                WinTop = 126,
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
                    LeftOffset = 167,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R4I);
            R5I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R5I",
                Tag = 24,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 218,
                WinTop = 126,
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
                    LeftOffset = 218,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R5I);
            R6I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R6I",
                Tag = 25,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 269,
                WinTop = 126,
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
                    LeftOffset = 269,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R6I);
            R7I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R7I",
                Tag = 26,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 320,
                WinTop = 126,
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
                    LeftOffset = 320,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R7I);
            R8I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R8I",
                Tag = 27,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 371,
                WinTop = 126,
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
                    LeftOffset = 371,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R8I);
            R9I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R9I",
                Tag = 28,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 422,
                WinTop = 126,
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
                    LeftOffset = 422,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R9I);
            R10I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R10I",
                Tag = 29,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 473,
                WinTop = 126,
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
                    LeftOffset = 473,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R10I);
            R11I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R11I",
                Tag = 30,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 524,
                WinTop = 126,
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
                    LeftOffset = 524,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R11I);
            R12I = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "R12I",
                Tag = 31,
                OnDraw = R1IDraw,
                Priority = 500,
                WinLeft = 575,
                WinTop = 126,
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
                    LeftOffset = 575,
                    RightOffset = 0,
                    TopOffset = 126,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(R12I);
            FaceShadow = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceShadow",
                Visible = false,
                Priority = 25,
                WinLeft = 268,
                WinTop = 269,
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
                    LeftOffset = 268,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 29
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 7,
                AnimEnd = 37,
                WinWidth = 105,
                WinHeight = 134,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceShadow);
            MMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MMemo",
                Priority = 9999,
                WinLeft = 10,
                WinTop = 50,
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
                    RightOffset = 10,
                    TopOffset = 50,
                    BottomOffset = 57
                },
                Masked = false,
                WinWidth = 180,
                WinHeight = 63,
                BorderWidth = 4,
                BorderHeight = 2,
                TextColor = 12632256,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 18,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(MMemo);
            MBGoldLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MBGoldLbl",
                Priority = 0,
                WinLeft = 17,
                WinTop = 8,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of gold when you accept offer!"
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
                    TopOffset = 8,
                    BottomOffset = 40
                },
                Masked = true,
                Text = "Gold Bribe",
                TextColor = 0
            };
            LayoutSession.EndRead(MBGoldLbl);
            MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MIcon",
                Priority = 100,
                WinLeft = 74,
                WinTop = 9,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Symbol for this diplomatic action"
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
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MIcon);
            RMsgEditPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RMsgEditPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 405,
                WinTop = 0,
                WinWidth = 211,
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
                    LeftOffset = 405,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RMsgEditPnl);
            RSBribePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RSBribePnl",
                Priority = 10,
                WinLeft = 416,
                WinTop = 0,
                WinWidth = 200,
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
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RSBribePnl);
            SScroll = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SScroll",
                Visible = false,
                Priority = 0,
                WinLeft = 184,
                WinTop = 30,
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
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 3,
                Position = 0,
                WinHeight = 80,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(SScroll);
            SMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "SMemo",
                Priority = 0,
                WinLeft = 10,
                WinTop = 30,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Edit your message here"
                },
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
                    RightOffset = 10,
                    TopOffset = 30,
                    BottomOffset = 37
                },
                Masked = false,
                WinWidth = 191,
                WinHeight = 103,
                BorderWidth = 4,
                BorderHeight = 5,
                OnChange = Layout_ChangeActionMessage_6,
                TextColor = 12632256,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = true,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(SMemo);
            SBGoldSlid = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "SBGoldSlid",
                Priority = 0,
                WinLeft = 2,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects an amount of Gold to offer with action"
                },
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
                    BottomOffset = 0
                },
                Masked = true,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 18,
                WinWidth = 80,
                NumberVisible = 1,
                OnChange = SBGoldSlidChange,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 80,
                ILIndexTab = 83
            };
            LayoutSession.EndRead(SBGoldSlid);
            ActionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ActionPnl",
                Priority = 10,
                WinLeft = 12,
                WinTop = 250,
                WinWidth = 616,
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
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 250,
                    BottomOffset = 12
                },
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ActionPnl);
            LInfoPlayerPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LInfoPlayerPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 211,
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
                    RightOffset = 405,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LInfoPlayerPnl);
            RNewActionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RNewActionPnl",
                Visible = false,
                Priority = 0,
                WinLeft = 405,
                WinTop = 0,
                WinWidth = 211,
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
                    LeftOffset = 405,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RNewActionPnl);
            AExit = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AExit",
                Priority = 2000,
                WinLeft = 110,
                WinTop = 140,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Exits diplomacy"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 10,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 91,
                OnClick = Layout_ExitClick_3
            };
            LayoutSession.EndRead(AExit);
            APlayer = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "APlayer",
                Priority = 0,
                WinLeft = 105,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 2566452
            };
            LayoutSession.EndRead(APlayer);
            LInfoRacePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LInfoRacePnl",
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 211,
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
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LInfoRacePnl);
            ARace2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ARace2",
                Priority = 0,
                WinLeft = 85,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "####",
                TextColor = 2566452
            };
            LayoutSession.EndRead(ARace2);
            AAlignment = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AAlignment",
                Priority = 0,
                WinLeft = 39,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 37,
                WinWidth = 113,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AAlignment);
            AAlignmentLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AAlignmentLbl",
                Priority = 0,
                WinLeft = 83,
                WinTop = 45,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 4
                },
                Masked = true,
                Text = "####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(AAlignmentLbl);
            P1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P1CB",
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 14,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P1CB);
            P2CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P2CB",
                Tag = 1,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 65,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 65,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P2CB);
            P3CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P3CB",
                Tag = 2,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 116,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 116,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P3CB);
            P4CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P4CB",
                Tag = 3,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 167,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 167,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P4CB);
            P5CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P5CB",
                Tag = 4,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 218,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 218,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P5CB);
            P6CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P6CB",
                Tag = 5,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 269,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 269,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P6CB);
            P7CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P7CB",
                Tag = 6,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 320,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 320,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P7CB);
            P8CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P8CB",
                Tag = 7,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 371,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 371,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P8CB);
            P9CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P9CB",
                Tag = 8,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 422,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 422,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P9CB);
            P10CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P10CB",
                Tag = 9,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 473,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 473,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P10CB);
            P11CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P11CB",
                Tag = 10,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 524,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 524,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P11CB);
            P12CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P12CB",
                Tag = 11,
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 575,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Player."
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
                    LeftOffset = 575,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = P1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(P12CB);
            FaceFr = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFr",
                OnPreDraw = FaceFrPreDraw,
                Priority = 20,
                WinLeft = 268,
                WinTop = 269,
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
                    LeftOffset = 268,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 29
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 6,
                AnimEnd = 37,
                WinWidth = 105,
                WinHeight = 134,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFr);
            R1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R1CB",
                Tag = 20,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 14,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R1CB);
            R2CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R2CB",
                Tag = 21,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 65,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 65,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R2CB);
            R3CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R3CB",
                Tag = 22,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 116,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 116,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R3CB);
            R4CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R4CB",
                Tag = 23,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 167,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 167,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R4CB);
            R5CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R5CB",
                Tag = 24,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 218,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 218,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R5CB);
            R6CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R6CB",
                Tag = 25,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 269,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 269,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R6CB);
            R7CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R7CB",
                Tag = 26,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 320,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 320,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R7CB);
            R8CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R8CB",
                Tag = 27,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 371,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 371,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R8CB);
            R9CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R9CB",
                Tag = 28,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 422,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 422,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R9CB);
            R10CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R10CB",
                Tag = 29,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 473,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 473,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R10CB);
            R11CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R11CB",
                Tag = 30,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 524,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 524,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R11CB);
            R12CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "R12CB",
                Tag = 31,
                OnPreDraw = R1CBPreDraw,
                Priority = 800,
                WinLeft = 575,
                WinTop = 185,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects this Race."
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
                    LeftOffset = 575,
                    RightOffset = 0,
                    TopOffset = 185,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = R1CBChange,
                RadioIndex = 1
            };
            LayoutSession.EndRead(R12CB);
            PlyrGlow = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PlyrGlow",
                OnDraw = PlyrGlowDraw,
                OnUpdate = Layout_PlyrGlowUpdate_7,
                Priority = 1000,
                WinLeft = 14,
                WinTop = 10,
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 39,
                AnimEnd = 39,
                WinWidth = 52,
                WinHeight = 64,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PlyrGlow);
            NoContact = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NoContact",
                Visible = false,
                Priority = 50,
                WinLeft = 278,
                WinTop = 329,
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
                    BottomOffset = 85
                },
                Masked = true,
                Text = "No Contact",
                TextColor = 0
            };
            LayoutSession.EndRead(NoContact);
            ASelect = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ASelect",
                Priority = 2000,
                WinLeft = 5,
                WinTop = 140,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects diplomatic action"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 10,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Select",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 91,
                OnClick = Layout_ASelectClick_8
            };
            LayoutSession.EndRead(ASelect);
            ActionsLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "ActionsLB",
                Priority = 0,
                WinLeft = 1,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 192,
                WinHeight = 106,
                BorderWidth = 8,
                BorderHeight = 3,
                OnChange = ActionsLBChange,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ActionsLB);
            ActionSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ActionSB",
                Visible = false,
                Priority = 0,
                WinLeft = 176,
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
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 106,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(ActionSB);
            LBActionsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LBActionsPnl",
                Priority = 10,
                WinLeft = 10,
                WinTop = 25,
                WinWidth = 193,
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
                    RightOffset = 8,
                    TopOffset = 25,
                    BottomOffset = 38
                },
                WinHeight = 107,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LBActionsPnl);
            ActionLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ActionLbl",
                Priority = 0,
                WinLeft = 78,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Actions",
                TextColor = 2566452
            };
            LayoutSession.EndRead(ActionLbl);
            PIRelIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PIRelIcn",
                Visible = false,
                Priority = 100,
                WinLeft = 22,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates mutual relation to this race"
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
                    LeftOffset = 45,
                    RightOffset = 55,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PIRelIcn);
            PIRelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PIRelLbl",
                Visible = false,
                Priority = 0,
                WinLeft = 17,
                WinTop = 48,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 48,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Race Relation:",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PIRelLbl);
            PIRelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PIRelPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 96,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 60
                },
                WinHeight = 37,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PIRelPnl);
            PIRelV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PIRelV",
                Visible = false,
                Priority = 0,
                WinLeft = 38,
                WinTop = 62,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 62,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PIRelV);
            APlyrIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "APlyrIcon",
                Priority = 0,
                WinLeft = 22,
                WinTop = 10,
                LayoutToolTipText = new LayoutStrings()
                {
                    "AI player strength"
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
                    TopOffset = 10,
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
            LayoutSession.EndRead(APlyrIcon);
            APlyrType = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "APlyrType",
                Priority = 0,
                WinLeft = 35,
                WinTop = 60,
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
                    TopOffset = 60,
                    BottomOffset = 4
                },
                Masked = true,
                Text = "####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(APlyrType);
            PlStatIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PlStatIcn",
                Priority = 100,
                WinLeft = 21,
                WinTop = 17,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates mutual diplomatic status to this player"
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
                    TopOffset = 17,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 9,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PlStatIcn);
            PlStatLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlStatLbl",
                Priority = 0,
                WinLeft = 32,
                WinTop = 58,
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
                    TopOffset = 58,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Status:",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlStatLbl);
            PIStatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PIStatPnl",
                Priority = 10,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 95,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 1,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                WinHeight = 96,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PIStatPnl);
            PlStatV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlStatV",
                Priority = 0,
                WinLeft = 35,
                WinTop = 72,
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
                    TopOffset = 72,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PlStatV);
            PTypePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PTypePnl",
                Priority = 10,
                WinLeft = 97,
                WinTop = 1,
                WinWidth = 95,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                WinHeight = 96,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PTypePnl);
            PIFrame = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PIFrame",
                Priority = 10,
                WinLeft = 8,
                WinTop = 65,
                WinWidth = 193,
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
                    LeftOffset = 8,
                    RightOffset = 10,
                    TopOffset = 65,
                    BottomOffset = 7
                },
                WinHeight = 98,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PIFrame);
            RIFrame = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RIFrame",
                Priority = 10,
                WinLeft = 8,
                WinTop = 25,
                WinWidth = 193,
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
                    LeftOffset = 8,
                    RightOffset = 10,
                    TopOffset = 25,
                    BottomOffset = 7
                },
                WinHeight = 138,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RIFrame);
            RIRelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RIRelPnl",
                Priority = 10,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 191,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 1,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 60
                },
                WinHeight = 77,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RIRelPnl);
            RAlignPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RAlignPnl",
                Priority = 10,
                WinLeft = 1,
                WinTop = 77,
                WinWidth = 191,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 77,
                    BottomOffset = 1
                },
                WinHeight = 60,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RAlignPnl);
            RlRelIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RlRelIcn",
                Priority = 100,
                WinLeft = 69,
                WinTop = 7,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates mutual relation to this race"
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
                    LeftOffset = 45,
                    RightOffset = 55,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RlRelIcn);
            RIRelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RIRelLbl",
                Priority = 0,
                WinLeft = 64,
                WinTop = 48,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 48,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Race Relation:",
                TextColor = 9415072
            };
            LayoutSession.EndRead(RIRelLbl);
            RlRelV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RlRelV",
                Priority = 0,
                WinLeft = 86,
                WinTop = 62,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 62,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 9415072
            };
            LayoutSession.EndRead(RlRelV);
            MBGoldBribeV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MBGoldBribeV",
                Priority = 0,
                WinLeft = 30,
                WinTop = 26,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of gold when you accept offer!"
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
                    TopOffset = 26,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "2000",
                TextColor = 0
            };
            LayoutSession.EndRead(MBGoldBribeV);
            MBGoldIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MBGoldIcn",
                Priority = 0,
                WinLeft = 5,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of gold when you accept offer!"
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
                    TopOffset = 22,
                    BottomOffset = 7
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 51,
                AnimEnd = 3,
                WinWidth = 19,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MBGoldIcn);
            MBGoldPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MBGoldPnl",
                Priority = 10,
                WinLeft = 0,
                WinTop = 4,
                WinWidth = 80,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 20,
                    TopOffset = 4,
                    BottomOffset = 20
                },
                WinHeight = 42,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MBGoldPnl);
            MBManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MBManaPnl",
                Priority = 10,
                WinLeft = 118,
                WinTop = 4,
                WinWidth = 80,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 18,
                    RightOffset = 2,
                    TopOffset = 4,
                    BottomOffset = 20
                },
                WinHeight = 42,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MBManaPnl);
            MBManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MBManaLbl",
                Priority = 0,
                WinLeft = 16,
                WinTop = 8,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of mana when you accept the offer!"
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
                    TopOffset = 8,
                    BottomOffset = 40
                },
                Masked = true,
                Text = "Mana Bribe",
                TextColor = 0
            };
            LayoutSession.EndRead(MBManaLbl);
            MBManaBribeV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MBManaBribeV",
                Priority = 0,
                WinLeft = 30,
                WinTop = 26,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of mana when you accept the offer!"
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
                    TopOffset = 26,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "2000",
                TextColor = 0
            };
            LayoutSession.EndRead(MBManaBribeV);
            MBManaIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MBManaIcn",
                Priority = 0,
                WinLeft = 5,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "You will receive this amount of mana when you accept the offer!"
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
                    TopOffset = 22,
                    BottomOffset = 7
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 52,
                AnimEnd = 3,
                WinWidth = 19,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MBManaIcn);
            MsgLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MsgLbl",
                Priority = 0,
                WinLeft = 46,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Edit a Message:",
                TextColor = 2566452
            };
            LayoutSession.EndRead(MsgLbl);
            MsgSendBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MsgSendBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sends the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Send",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 92,
                OnClick = Layout_MsgSendActionBtn_9
            };
            LayoutSession.EndRead(MsgSendBtn);
            MsgCancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MsgCancelBtn",
                Priority = 1000,
                WinLeft = 115,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancels the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_CancelSelectedAction_10
            };
            LayoutSession.EndRead(MsgCancelBtn);
            RSBribe2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RSBribe2",
                Priority = 0,
                WinLeft = 10,
                WinTop = 127,
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
                WinWidth = 180,
                WinHeight = 3,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RSBribe2);
            SBGoldV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SBGoldV",
                Priority = 0,
                WinLeft = 22,
                WinTop = 6,
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
                    LeftOffset = 22,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "2000",
                TextColor = 0
            };
            LayoutSession.EndRead(SBGoldV);
            SBGoldIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SBGoldIcn",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Unit's cost in gems"
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 51,
                AnimEnd = 3,
                WinWidth = 19,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SBGoldIcn);
            SBGold = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SBGold",
                Priority = 10,
                WinLeft = 10,
                WinTop = 70,
                WinWidth = 85,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 5,
                    TopOffset = 70,
                    BottomOffset = 18
                },
                WinHeight = 40,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SBGold);
            SBMana = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SBMana",
                Priority = 10,
                WinLeft = 105,
                WinTop = 70,
                WinWidth = 83,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 12,
                    TopOffset = 70,
                    BottomOffset = 18
                },
                WinHeight = 40,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SBMana);
            SBManaV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SBManaV",
                Priority = 0,
                WinLeft = 22,
                WinTop = 6,
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
                    LeftOffset = 22,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "2000",
                TextColor = 0
            };
            LayoutSession.EndRead(SBManaV);
            SBManaIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SBManaIcn",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Unit's cost in gems"
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 52,
                AnimEnd = 3,
                WinWidth = 19,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SBManaIcn);
            RSBSep1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RSBSep1",
                Priority = 0,
                WinLeft = 10,
                WinTop = 43,
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
                    TopOffset = 43,
                    BottomOffset = 60
                },
                Masked = false,
                WinWidth = 180,
                WinHeight = 3,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RSBSep1);
            SBribeBtn = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SBribeBtn",
                Priority = 0,
                WinLeft = 67,
                WinTop = 50,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Optional bribes",
                TextColor = 0
            };
            LayoutSession.EndRead(SBribeBtn);
            SBManaSlid = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "SBManaSlid",
                Priority = 0,
                WinLeft = 1,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects an amount of Gold to offer with action"
                },
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
                    BottomOffset = 0
                },
                Masked = true,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 18,
                WinWidth = 80,
                NumberVisible = 1,
                OnChange = SBManaSlidChange,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 80,
                ILIndexTab = 83
            };
            LayoutSession.EndRead(SBManaSlid);
            SBIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SBIcon",
                Priority = 100,
                WinLeft = 2,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Symbol for this diplomatic action"
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
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 10,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SBIcon);
            RSBSendBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RSBSendBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sends the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Send",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_MsgSendActionBtn_9
            };
            LayoutSession.EndRead(RSBSendBtn);
            RBSCanlBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RBSCanlBtn",
                Priority = 1000,
                WinLeft = 104,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancels the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_CancelSelectedAction_10
            };
            LayoutSession.EndRead(RBSCanlBtn);
            StatusGlow = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "StatusGlow",
                Tag = 1,
                Visible = false,
                Priority = 600,
                WinLeft = 14,
                WinTop = 74,
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 49,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(StatusGlow);
            BribeHeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "BribeHeaderLbl",
                Priority = 0,
                WinLeft = 50,
                WinTop = 20,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Offer Peace",
                TextColor = 0
            };
            LayoutSession.EndRead(BribeHeaderLbl);
            RMsgRecPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RMsgRecPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 405,
                WinTop = 0,
                WinWidth = 211,
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
                    LeftOffset = 405,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RMsgRecPnl);
            RMMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "RMMemo",
                Priority = 0,
                WinLeft = 10,
                WinTop = 30,
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
                    RightOffset = 10,
                    TopOffset = 30,
                    BottomOffset = 60
                },
                Masked = false,
                WinWidth = 191,
                WinHeight = 80,
                BorderWidth = 4,
                BorderHeight = 5,
                TextColor = 12632256,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(RMMemo);
            RMHeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RMHeaderLbl",
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
                Text = "######",
                TextColor = 0
            };
            LayoutSession.EndRead(RMHeaderLbl);
            RMReplyBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RMReplyBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 116,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sends the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 32
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Reply",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 92,
                OnClick = Layout_ReplyBtnClick_11
            };
            LayoutSession.EndRead(RMReplyBtn);
            RMCancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RMCancelBtn",
                Priority = 1000,
                WinLeft = 115,
                WinTop = 116,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancels the message"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 105,
                    BottomOffset = 32
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_RMCancelBtnClick_12
            };
            LayoutSession.EndRead(RMCancelBtn);
            RMExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RMExitBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Exits diplomacy"
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
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 191,
                OnClick = Layout_ExitClick_3
            };
            LayoutSession.EndRead(RMExitBtn);
            IncommingInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IncommingInfoPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 416,
                WinTop = 0,
                WinWidth = 200,
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
                WinHeight = 170,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IncommingInfoPnl);
            IIOkBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "IIOkBtn",
                Priority = 1000,
                WinLeft = 10,
                WinTop = 142,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Ok",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_IIOkBtnClick_13
            };
            LayoutSession.EndRead(IIOkBtn);
            IIExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "IIExitBtn",
                Priority = 1000,
                WinLeft = 104,
                WinTop = 142,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Exits diplomacy"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 105,
                    BottomOffset = 6
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 86,
                OnClick = Layout_ExitClick_3
            };
            LayoutSession.EndRead(IIExitBtn);
            IRScroll = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "IRScroll",
                Visible = false,
                Priority = 9999,
                WinLeft = 177,
                WinTop = 50,
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
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 83,
                WinWidth = 13,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 42,
                ILIndexIncDown = 43,
                ILIndexDecUp = 40,
                ILIndexDecDown = 41,
                ILIndexBack = 47,
                ILIndexTab = 44
            };
            LayoutSession.EndRead(IRScroll);
            IIMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IIMemo",
                Priority = 9999,
                WinLeft = 10,
                WinTop = 50,
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
                    RightOffset = 10,
                    TopOffset = 50,
                    BottomOffset = 37
                },
                Masked = false,
                WinWidth = 180,
                WinHeight = 83,
                BorderWidth = 4,
                BorderHeight = 2,
                TextColor = 12632256,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 18,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(IIMemo);
            IIIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "IIIcon",
                Priority = 100,
                WinLeft = 74,
                WinTop = 9,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Symbol for this diplomatic action"
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
                    TopOffset = 9,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 37,
                WinWidth = 52,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(IIIcon);
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
            OfTheLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "OfTheLbl",
                Priority = 0,
                WinLeft = 82,
                WinTop = 25,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "of the",
                TextColor = 2566452
            };
            LayoutSession.EndRead(OfTheLbl);
            PRaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PRaceLbl",
                Priority = 0,
                WinLeft = 105,
                WinTop = 45,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 2566452
            };
            LayoutSession.EndRead(PRaceLbl);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            RShield.AOWWindow = ActionPnl;
            LShield.AOWWindow = ActionPnl;
            MExitBtn.AOWWindow = RIncomingPnl;
            RelPnl.AOWWindow = RelWin;
            RIncomingPnl.AOWWindow = ActionPnl;
            MAcceptBtn.AOWWindow = RIncomingPnl;
            MRejectBtn.AOWWindow = RIncomingPnl;
            MScroll.AOWWindow = RIncomingPnl;
            P1I.AOWWindow = RelPnl;
            P2I.AOWWindow = RelPnl;
            P3I.AOWWindow = RelPnl;
            P4I.AOWWindow = RelPnl;
            P5I.AOWWindow = RelPnl;
            P6I.AOWWindow = RelPnl;
            P7I.AOWWindow = RelPnl;
            P8I.AOWWindow = RelPnl;
            P9I.AOWWindow = RelPnl;
            P10I.AOWWindow = RelPnl;
            P11I.AOWWindow = RelPnl;
            P12I.AOWWindow = RelPnl;
            R1I.AOWWindow = RelPnl;
            R2I.AOWWindow = RelPnl;
            R3I.AOWWindow = RelPnl;
            R4I.AOWWindow = RelPnl;
            R5I.AOWWindow = RelPnl;
            R6I.AOWWindow = RelPnl;
            R7I.AOWWindow = RelPnl;
            R8I.AOWWindow = RelPnl;
            R9I.AOWWindow = RelPnl;
            R10I.AOWWindow = RelPnl;
            R11I.AOWWindow = RelPnl;
            R12I.AOWWindow = RelPnl;
            FaceShadow.AOWWindow = RelPnl;
            MMemo.AOWWindow = RIncomingPnl;
            MMemo.VScrollBar = MScroll;
            MBGoldLbl.AOWWindow = MBGoldPnl;
            MIcon.AOWWindow = RIncomingPnl;
            RMsgEditPnl.AOWWindow = ActionPnl;
            RSBribePnl.AOWWindow = ActionPnl;
            SScroll.AOWWindow = RMsgRecPnl;
            SMemo.AOWWindow = RMsgEditPnl;
            SMemo.VScrollBar = SScroll;
            SBGoldSlid.AOWWindow = SBGold;
            ActionPnl.AOWWindow = RelPnl;
            LInfoPlayerPnl.AOWWindow = ActionPnl;
            RNewActionPnl.AOWWindow = ActionPnl;
            AExit.AOWWindow = RNewActionPnl;
            APlayer.AOWWindow = LInfoPlayerPnl;
            LInfoRacePnl.AOWWindow = ActionPnl;
            ARace2.AOWWindow = LInfoRacePnl;
            AAlignment.AOWWindow = RAlignPnl;
            AAlignmentLbl.AOWWindow = RAlignPnl;
            P1CB.AOWWindow = RelPnl;
            P2CB.AOWWindow = RelPnl;
            P3CB.AOWWindow = RelPnl;
            P4CB.AOWWindow = RelPnl;
            P5CB.AOWWindow = RelPnl;
            P6CB.AOWWindow = RelPnl;
            P7CB.AOWWindow = RelPnl;
            P8CB.AOWWindow = RelPnl;
            P9CB.AOWWindow = RelPnl;
            P10CB.AOWWindow = RelPnl;
            P11CB.AOWWindow = RelPnl;
            P12CB.AOWWindow = RelPnl;
            FaceFr.AOWWindow = RelPnl;
            R1CB.AOWWindow = RelPnl;
            R2CB.AOWWindow = RelPnl;
            R3CB.AOWWindow = RelPnl;
            R4CB.AOWWindow = RelPnl;
            R5CB.AOWWindow = RelPnl;
            R6CB.AOWWindow = RelPnl;
            R7CB.AOWWindow = RelPnl;
            R8CB.AOWWindow = RelPnl;
            R9CB.AOWWindow = RelPnl;
            R10CB.AOWWindow = RelPnl;
            R11CB.AOWWindow = RelPnl;
            R12CB.AOWWindow = RelPnl;
            PlyrGlow.AOWWindow = RelPnl;
            NoContact.AOWWindow = RelPnl;
            ASelect.AOWWindow = RNewActionPnl;
            ActionsLB.AOWWindow = LBActionsPnl;
            ActionsLB.VScrollBar = ActionSB;
            ActionSB.AOWWindow = LBActionsPnl;
            LBActionsPnl.AOWWindow = RNewActionPnl;
            ActionLbl.AOWWindow = RNewActionPnl;
            PIRelIcn.AOWWindow = PIRelPnl;
            PIRelLbl.AOWWindow = PIRelPnl;
            PIRelPnl.AOWWindow = PIFrame;
            PIRelV.AOWWindow = PIRelPnl;
            APlyrIcon.AOWWindow = PTypePnl;
            APlyrType.AOWWindow = PTypePnl;
            PlStatIcn.AOWWindow = PIStatPnl;
            PlStatLbl.AOWWindow = PIStatPnl;
            PIStatPnl.AOWWindow = PIFrame;
            PlStatV.AOWWindow = PIStatPnl;
            PTypePnl.AOWWindow = PIFrame;
            PIFrame.AOWWindow = LInfoPlayerPnl;
            RIFrame.AOWWindow = LInfoRacePnl;
            RIRelPnl.AOWWindow = RIFrame;
            RAlignPnl.AOWWindow = RIFrame;
            RlRelIcn.AOWWindow = RIRelPnl;
            RIRelLbl.AOWWindow = RIRelPnl;
            RlRelV.AOWWindow = RIRelPnl;
            MBGoldBribeV.AOWWindow = MBGoldPnl;
            MBGoldIcn.AOWWindow = MBGoldPnl;
            MBGoldPnl.AOWWindow = RIncomingPnl;
            MBManaPnl.AOWWindow = RIncomingPnl;
            MBManaLbl.AOWWindow = MBManaPnl;
            MBManaBribeV.AOWWindow = MBManaPnl;
            MBManaIcn.AOWWindow = MBManaPnl;
            MsgLbl.AOWWindow = RMsgEditPnl;
            MsgSendBtn.AOWWindow = RMsgEditPnl;
            MsgCancelBtn.AOWWindow = RMsgEditPnl;
            RSBribe2.AOWWindow = RSBribePnl;
            SBGoldV.AOWWindow = SBGold;
            SBGoldIcn.AOWWindow = SBGold;
            SBGold.AOWWindow = RSBribePnl;
            SBMana.AOWWindow = RSBribePnl;
            SBManaV.AOWWindow = SBMana;
            SBManaIcn.AOWWindow = SBMana;
            RSBSep1.AOWWindow = RSBribePnl;
            SBribeBtn.AOWWindow = RSBribePnl;
            SBManaSlid.AOWWindow = SBMana;
            SBIcon.AOWWindow = RSBribePnl;
            RSBSendBtn.AOWWindow = RSBribePnl;
            RBSCanlBtn.AOWWindow = RSBribePnl;
            StatusGlow.AOWWindow = RelPnl;
            BribeHeaderLbl.AOWWindow = RSBribePnl;
            RMsgRecPnl.AOWWindow = ActionPnl;
            RMMemo.AOWWindow = RMsgRecPnl;
            RMMemo.VScrollBar = SScroll;
            RMHeaderLbl.AOWWindow = RMsgRecPnl;
            RMReplyBtn.AOWWindow = RMsgRecPnl;
            RMCancelBtn.AOWWindow = RMsgRecPnl;
            RMExitBtn.AOWWindow = RMsgRecPnl;
            IncommingInfoPnl.AOWWindow = ActionPnl;
            IIOkBtn.AOWWindow = IncommingInfoPnl;
            IIExitBtn.AOWWindow = IncommingInfoPnl;
            IRScroll.AOWWindow = IncommingInfoPnl;
            IIMemo.AOWWindow = IncommingInfoPnl;
            IIMemo.VScrollBar = IRScroll;
            IIIcon.AOWWindow = IncommingInfoPnl;
            OfTheLbl.AOWWindow = LInfoPlayerPnl;
            PRaceLbl.AOWWindow = LInfoPlayerPnl;
            Loaded();
            RelWin.Loaded();
            RShield.Loaded();
            LShield.Loaded();
            MExitBtn.Loaded();
            RelPnl.Loaded();
            RIncomingPnl.Loaded();
            MAcceptBtn.Loaded();
            MRejectBtn.Loaded();
            MScroll.Loaded();
            P1I.Loaded();
            P2I.Loaded();
            P3I.Loaded();
            P4I.Loaded();
            P5I.Loaded();
            P6I.Loaded();
            P7I.Loaded();
            P8I.Loaded();
            P9I.Loaded();
            P10I.Loaded();
            P11I.Loaded();
            P12I.Loaded();
            R1I.Loaded();
            R2I.Loaded();
            R3I.Loaded();
            R4I.Loaded();
            R5I.Loaded();
            R6I.Loaded();
            R7I.Loaded();
            R8I.Loaded();
            R9I.Loaded();
            R10I.Loaded();
            R11I.Loaded();
            R12I.Loaded();
            FaceShadow.Loaded();
            MMemo.Loaded();
            MBGoldLbl.Loaded();
            MIcon.Loaded();
            RMsgEditPnl.Loaded();
            RSBribePnl.Loaded();
            SScroll.Loaded();
            SMemo.Loaded();
            SBGoldSlid.Loaded();
            ActionPnl.Loaded();
            LInfoPlayerPnl.Loaded();
            RNewActionPnl.Loaded();
            AExit.Loaded();
            APlayer.Loaded();
            LInfoRacePnl.Loaded();
            ARace2.Loaded();
            AAlignment.Loaded();
            AAlignmentLbl.Loaded();
            P1CB.Loaded();
            P2CB.Loaded();
            P3CB.Loaded();
            P4CB.Loaded();
            P5CB.Loaded();
            P6CB.Loaded();
            P7CB.Loaded();
            P8CB.Loaded();
            P9CB.Loaded();
            P10CB.Loaded();
            P11CB.Loaded();
            P12CB.Loaded();
            FaceFr.Loaded();
            R1CB.Loaded();
            R2CB.Loaded();
            R3CB.Loaded();
            R4CB.Loaded();
            R5CB.Loaded();
            R6CB.Loaded();
            R7CB.Loaded();
            R8CB.Loaded();
            R9CB.Loaded();
            R10CB.Loaded();
            R11CB.Loaded();
            R12CB.Loaded();
            PlyrGlow.Loaded();
            NoContact.Loaded();
            ASelect.Loaded();
            ActionsLB.Loaded();
            ActionSB.Loaded();
            LBActionsPnl.Loaded();
            ActionLbl.Loaded();
            PIRelIcn.Loaded();
            PIRelLbl.Loaded();
            PIRelPnl.Loaded();
            PIRelV.Loaded();
            APlyrIcon.Loaded();
            APlyrType.Loaded();
            PlStatIcn.Loaded();
            PlStatLbl.Loaded();
            PIStatPnl.Loaded();
            PlStatV.Loaded();
            PTypePnl.Loaded();
            PIFrame.Loaded();
            RIFrame.Loaded();
            RIRelPnl.Loaded();
            RAlignPnl.Loaded();
            RlRelIcn.Loaded();
            RIRelLbl.Loaded();
            RlRelV.Loaded();
            MBGoldBribeV.Loaded();
            MBGoldIcn.Loaded();
            MBGoldPnl.Loaded();
            MBManaPnl.Loaded();
            MBManaLbl.Loaded();
            MBManaBribeV.Loaded();
            MBManaIcn.Loaded();
            MsgLbl.Loaded();
            MsgSendBtn.Loaded();
            MsgCancelBtn.Loaded();
            RSBribe2.Loaded();
            SBGoldV.Loaded();
            SBGoldIcn.Loaded();
            SBGold.Loaded();
            SBMana.Loaded();
            SBManaV.Loaded();
            SBManaIcn.Loaded();
            RSBSep1.Loaded();
            SBribeBtn.Loaded();
            SBManaSlid.Loaded();
            SBIcon.Loaded();
            RSBSendBtn.Loaded();
            RBSCanlBtn.Loaded();
            StatusGlow.Loaded();
            BribeHeaderLbl.Loaded();
            RMsgRecPnl.Loaded();
            RMMemo.Loaded();
            RMHeaderLbl.Loaded();
            RMReplyBtn.Loaded();
            RMCancelBtn.Loaded();
            RMExitBtn.Loaded();
            IncommingInfoPnl.Loaded();
            IIOkBtn.Loaded();
            IIExitBtn.Loaded();
            IRScroll.Loaded();
            IIMemo.Loaded();
            IIIcon.Loaded();
            Translator.Loaded();
            OfTheLbl.Loaded();
            PRaceLbl.Loaded();
            session.Resolve(this);
            RelWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            RelWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            MExitBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MAcceptBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MAcceptBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MAcceptBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MRejectBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MRejectBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MRejectBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MScroll.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MScroll.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MScroll.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MScroll.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            P1I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P1I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P2I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P2I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P3I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P3I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P4I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P4I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P5I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P5I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P6I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P6I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P7I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P7I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P8I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P8I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P9I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P9I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P10I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P10I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P11I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P11I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P12I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P12I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R1I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R1I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R2I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R2I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R3I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R3I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R4I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R4I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R5I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R5I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R6I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R6I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R7I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R7I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R8I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R8I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R9I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R9I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R10I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R10I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R11I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R11I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R12I.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R12I.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            FaceShadow.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            MMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MBGoldLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBGoldLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            SScroll.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SScroll.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SScroll.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SScroll.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SMemo.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SBGoldSlid.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SBGoldSlid.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBGoldSlid.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBGoldSlid.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AExit.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AExit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AExit.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            APlayer.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ARace2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            AAlignment.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            AAlignmentLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            P1CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P1CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P2CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P2CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P3CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P3CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P4CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P4CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P5CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P5CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P6CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P6CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P7CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P7CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P8CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P8CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P9CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P9CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P10CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P10CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P11CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P11CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            P12CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            P12CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            FaceFr.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R1CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R1CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R2CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R2CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R3CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R3CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R4CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R4CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R5CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R5CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R6CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R6CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R7CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R7CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R8CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R8CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R9CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R9CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R10CB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            R10CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R11CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            R12CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            PlyrGlow.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            NoContact.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ASelect.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ASelect.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ASelect.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ActionsLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ActionsLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ActionsLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ActionsLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            ActionSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ActionSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ActionSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ActionSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            LBActionsPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LBActionsPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ActionLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            PIRelIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PIRelIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            PIRelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PIRelPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PIRelPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PIRelV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            APlyrIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            APlyrIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.PlyrIcns;
            APlyrType.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PlStatIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PlStatIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            PlStatLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PIStatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PIStatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlStatV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PTypePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PTypePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PIFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PIFrame.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RIFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RIFrame.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RIRelPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RIRelPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RAlignPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RAlignPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RlRelIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RlRelIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            RIRelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            RlRelV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MBGoldBribeV.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBGoldBribeV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MBGoldIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBGoldIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            MBManaLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MBManaBribeV.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBManaBribeV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MBManaIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MBManaIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            MsgLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            MsgSendBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MsgSendBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MsgSendBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MsgCancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MsgCancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MsgCancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RSBribe2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SBGoldV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SBGoldIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SBGoldIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            SBManaV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SBManaIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SBManaIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            RSBSep1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SBribeBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SBManaSlid.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SBManaSlid.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBManaSlid.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBManaSlid.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SBIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            RSBSendBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RSBSendBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RSBSendBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RBSCanlBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RBSCanlBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RBSCanlBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            StatusGlow.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            BribeHeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMMemo.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RMMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RMMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RMHeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMReplyBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RMReplyBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMReplyBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RMCancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RMCancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMCancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RMExitBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RMExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RMExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            IIOkBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IIOkBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            IIOkBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            IIExitBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IIExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            IIExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            IRScroll.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IRScroll.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IRScroll.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IRScroll.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IIMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            IIMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IIMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IIIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IIIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            OfTheLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            PRaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_RelWindowCreate_0(TObject p0)
        {
            RelWindowCreate();
        }

        private void Layout_RelWindowDestroy_1(TObject p0)
        {
            RelWindowDestroy();
        }

        private void Layout_RelWinHide_2(TObject p0)
        {
            RelWinHide();
        }

        private void Layout_ExitClick_3(TObject p0)
        {
            ExitClick();
        }

        private void Layout_MAcceptBtnClick_4(TObject p0)
        {
            MAcceptBtnClick();
        }

        private void Layout_MRejectBtnClick_5(TObject p0)
        {
            MRejectBtnClick();
        }

        private void Layout_ChangeActionMessage_6(TObject p0)
        {
            ChangeActionMessage((Aow.Units.AoWMemo.TAOWMemo)p0);
        }

        private void Layout_PlyrGlowUpdate_7(TObject p0)
        {
            PlyrGlowUpdate();
        }

        private void Layout_ASelectClick_8(TObject p0)
        {
            ASelectClick();
        }

        private void Layout_MsgSendActionBtn_9(TObject p0)
        {
            MsgSendActionBtn();
        }

        private void Layout_CancelSelectedAction_10(TObject p0)
        {
            CancelSelectedAction();
        }

        private void Layout_ReplyBtnClick_11(TObject p0)
        {
            ReplyBtnClick();
        }

        private void Layout_RMCancelBtnClick_12(TObject p0)
        {
            RMCancelBtnClick();
        }

        private void Layout_IIOkBtnClick_13(TObject p0)
        {
            IIOkBtnClick();
        }
    }
}
