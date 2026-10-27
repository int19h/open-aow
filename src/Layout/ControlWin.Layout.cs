// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWControlWin
{
    public partial class TControlWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ControlWin";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            ControlWindow = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "ControlWindow",
                Visible = false,
                Priority = 10,
                WinLeft = 440,
                WinTop = 216,
                WinWidth = 200,
                Masked = false,
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
                    TopOffset = 216,
                    BottomOffset = 48
                },
                UseVMem = true,
                BorderWidth = 4,
                BorderHeight = 4,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = true,
                OnDrag = Layout_ControlWindowResize_0,
                OnResize = Layout_ControlWindowResize_0,
                MinWidth = 200,
                MinHeight = 216,
                MaxWidth = 400,
                MaxHeight = 432,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 216,
                Dockable = false
            };
            LayoutSession.EndRead(ControlWindow);
            CWTitle = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CWTitle",
                Priority = 0,
                WinLeft = 25,
                WinTop = 4,
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
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Party",
                TextColor = 0
            };
            LayoutSession.EndRead(CWTitle);
            CWMinIcon = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CWMinIcon",
                Priority = 0,
                WinLeft = 167,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Minimizes the window."
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
                    RightOffset = 18,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 4,
                ILIndexLit = 5,
                ILIndexUp = 3,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CWMinIconClick_1
            };
            LayoutSession.EndRead(CWMinIcon);
            Unit1Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit1Frame",
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
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
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit1Frame);
            Unit2Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit2Frame",
                Tag = 1,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit2Frame);
            Unit3Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit3Frame",
                Tag = 2,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit3Frame);
            Unit4Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit4Frame",
                Tag = 3,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit4Frame);
            Unit5Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit5Frame",
                Tag = 4,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
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
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit5Frame);
            Unit6Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit6Frame",
                Tag = 5,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit6Frame);
            Unit7Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit7Frame",
                Tag = 6,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit7Frame);
            Unit8Frame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Unit8Frame",
                Tag = 7,
                OnMouseDown = Layout_Unit1FrameMouseDown_2,
                OnPreDraw = Unit1FramePreDraw,
                Priority = 0,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Unit8Frame);
            CTitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CTitleIcon",
                Priority = 0,
                WinLeft = 2,
                WinTop = 2,
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
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CTitleIcon);
            MovesLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovesLbl",
                Priority = 20,
                WinLeft = 38,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Movepoints available to the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 40,
                    RightOffPercent = 5,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 12,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(MovesLbl);
            PartyStatusLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PartyStatusLbl",
                Priority = 10,
                WinLeft = 48,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Status of the party."
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(PartyStatusLbl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 181,
                WinTop = 1,
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
                    RightOffset = 4,
                    TopOffset = 1,
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
                OnClick = CloseClick
            };
            LayoutSession.EndRead(Close);
            U1MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U1MIcon",
                Priority = 5,
                WinLeft = 3,
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
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U1MIcon);
            U2MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U2MIcon",
                Priority = 5,
                WinLeft = 51,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U2MIcon);
            U3MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U3MIcon",
                Priority = 5,
                WinLeft = 99,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U3MIcon);
            U4MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U4MIcon",
                Priority = 5,
                WinLeft = 147,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U4MIcon);
            U5MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U5MIcon",
                Priority = 5,
                WinLeft = 3,
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
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U5MIcon);
            U6MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U6MIcon",
                Priority = 5,
                WinLeft = 51,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U6MIcon);
            U7MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U7MIcon",
                Priority = 5,
                WinLeft = 99,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U7MIcon);
            U8MIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "U8MIcon",
                Priority = 5,
                WinLeft = 147,
                WinTop = 0,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(U8MIcon);
            MoraleMask = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MoraleMask",
                Visible = false,
                Priority = 0,
                WinLeft = 149,
                WinTop = 5,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 17,
                WinWidth = 31,
                WinHeight = 32,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MoraleMask);
            MoveIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MoveIcon",
                Priority = 20,
                WinLeft = 6,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Party's Movement Type"
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 10,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MoveIcon);
            PartyT1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyT1Pnl",
                Priority = 0,
                WinLeft = 4,
                WinTop = 21,
                WinWidth = 192,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 10,
                    BottomOffPercent = 68,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 49,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyT1Pnl);
            PartyBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyBPnl",
                Priority = 0,
                WinLeft = 4,
                WinTop = 170,
                WinWidth = 96,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 79,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 14,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyBPnl);
            PartyT2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyT2Pnl",
                Priority = 0,
                WinLeft = 4,
                WinTop = 73,
                WinWidth = 192,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 34,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyT2Pnl);
            PartyT3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyT3Pnl",
                Priority = 0,
                WinLeft = 4,
                WinTop = 95,
                WinWidth = 192,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 44,
                    BottomOffPercent = 34,
                    LeftOffset = 4,
                    RightOffset = 4,
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
            LayoutSession.EndRead(PartyT3Pnl);
            PartyT4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyT4Pnl",
                Priority = 0,
                WinLeft = 4,
                WinTop = 146,
                WinWidth = 192,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 68,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyT4Pnl);
            Unit1Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit1Gge",
                Priority = 5,
                WinLeft = 12,
                WinTop = 44,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit1Gge);
            Unit2Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit2Gge",
                Priority = 5,
                WinLeft = 60,
                WinTop = 44,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit2Gge);
            Unit3Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit3Gge",
                Priority = 5,
                WinLeft = 108,
                WinTop = 44,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit3Gge);
            Unit4Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit4Gge",
                Priority = 5,
                WinLeft = 156,
                WinTop = 44,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit4Gge);
            Unit5Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit5Gge",
                Priority = 5,
                WinLeft = 12,
                WinTop = 43,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit5Gge);
            Unit6Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit6Gge",
                Priority = 5,
                WinLeft = 60,
                WinTop = 43,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit6Gge);
            Unit7Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit7Gge",
                Priority = 5,
                WinLeft = 108,
                WinTop = 43,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit7Gge);
            Unit8Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit8Gge",
                Priority = 5,
                WinLeft = 156,
                WinTop = 43,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 24,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit8Gge);
            Unit1Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit1Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit1Chk);
            Unit2Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit2Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 49,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit2Chk);
            Unit3Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit3Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 97,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit3Chk);
            Unit5Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit5Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit5Chk);
            Unit6Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit6Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 49,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit6Chk);
            Unit7Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit7Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 97,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit7Chk);
            Unit4Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit4Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 145,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit4Chk);
            Unit8Chk = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Unit8Chk",
                OnMouseDown = Unit1ChkMouseDown,
                Priority = 0,
                WinLeft = 145,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Toggles whether or not the unit travels with the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 6,
                ILIndexDown = 23,
                ILIndexLit = 20,
                ILIndexUp = 20,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 46,
                Checked = false,
                OnChange = Unit1ChkChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(Unit8Chk);
            Emp1Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp1Fr",
                Priority = 50,
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
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp1Fr);
            Emp2Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp2Fr",
                Priority = 50,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp2Fr);
            Emp3Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp3Fr",
                Priority = 50,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp3Fr);
            Emp4Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp4Fr",
                Priority = 50,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 49,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp4Fr);
            Emp5Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp5Fr",
                Priority = 50,
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
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp5Fr);
            Emp6Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp6Fr",
                Priority = 50,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp6Fr);
            Emp7Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp7Fr",
                Priority = 50,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp7Fr);
            Emp8Fr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp8Fr",
                Priority = 50,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 8
            };
            LayoutSession.EndRead(Emp8Fr);
            Emp1Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp1Chk",
                Priority = 50,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp1Chk);
            Emp2Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp2Chk",
                Priority = 50,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp2Chk);
            Emp3Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp3Chk",
                Priority = 50,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp3Chk);
            Emp4Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp4Chk",
                Priority = 50,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp4Chk);
            Emp5Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp5Chk",
                Priority = 50,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp5Chk);
            Emp6Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp6Chk",
                Priority = 50,
                WinLeft = 49,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp6Chk);
            Emp7Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp7Chk",
                Priority = 50,
                WinLeft = 97,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp7Chk);
            Emp8Chk = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Emp8Chk",
                Priority = 50,
                WinLeft = 145,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Emp8Chk);
            PartyIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyIPnl",
                Priority = 0,
                WinLeft = 5,
                WinTop = 168,
                WinWidth = 190,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 78,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 0,
                    BottomOffset = 6
                },
                WinHeight = 42,
                ILIndexFrame = 0,
                ILIndexTexture = 8,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyIPnl);
            PartyILbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PartyILbl",
                Priority = 10,
                WinLeft = 95,
                WinTop = 21,
                CaptureInput = true,
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
                Masked = true,
                TextColor = 9236989
            };
            LayoutSession.EndRead(PartyILbl);
            Center = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Center",
                Priority = 0,
                WinLeft = 153,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Centers the World Map on this party."
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
                    RightOffset = 32,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 7,
                ILIndexLit = 8,
                ILIndexUp = 6,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CenterClick_3
            };
            LayoutSession.EndRead(Center);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Priority = 0,
                WinLeft = 139,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays Help for this window."
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 60,
                ILIndexLit = 61,
                ILIndexUp = 59,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_HelpClick_4
            };
            LayoutSession.EndRead(Help);
            PPrev = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PPrev",
                Priority = 100,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Previous Active Party"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 85,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "<",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 29,
                OnClick = Layout_PPrevClick_5
            };
            LayoutSession.EndRead(PPrev);
            PMove = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PMove",
                Priority = 100,
                WinLeft = 96,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Moves this Party"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 49,
                    RightOffPercent = 15,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "Move",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 69,
                OnClick = Layout_PMoveClick_6
            };
            LayoutSession.EndRead(PMove);
            PNext = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PNext",
                Priority = 100,
                WinLeft = 164,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Next Active Party"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 85,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = ">",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 29,
                OnClick = Layout_PNextClick_7
            };
            LayoutSession.EndRead(PNext);
            PControlPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PControlPnl",
                Priority = 0,
                WinLeft = 3,
                WinTop = 192,
                WinWidth = 194,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 78,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 20,
                ILIndexFrame = 30,
                ILIndexTexture = 14,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PControlPnl);
            PGuard = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "PGuard",
                Priority = 0,
                WinLeft = 29,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes party from next party loop. "
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 15,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "Guard",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 68,
                Checked = false,
                OnChange = Layout_PGuardChange_8,
                RadioIndex = 0
            };
            LayoutSession.EndRead(PGuard);
            AoWFrame2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame2",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 94,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame2);
            PartyB2Pbl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PartyB2Pbl",
                Priority = 0,
                WinLeft = 100,
                WinTop = 170,
                WinWidth = 96,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 79,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                WinHeight = 18,
                ILIndexFrame = 0,
                ILIndexTexture = 14,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PartyB2Pbl);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 94,
                WinHeight = 18,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 9,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
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
            PartyActionLbl1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PartyActionLbl1",
                Priority = 10,
                WinLeft = 60,
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
                    TopOffPercent = 15,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Party Action:",
                TextColor = 9236989
            };
            LayoutSession.EndRead(PartyActionLbl1);
            PartyActionLbl2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PartyActionLbl2",
                Priority = 10,
                WinLeft = 60,
                WinTop = 23,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 55,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Party Action:",
                TextColor = 16777215
            };
            LayoutSession.EndRead(PartyActionLbl2);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            CWTitle.AOWWindow = ControlWindow;
            CWMinIcon.AOWWindow = ControlWindow;
            Unit1Frame.AOWWindow = PartyT1Pnl;
            Unit2Frame.AOWWindow = PartyT1Pnl;
            Unit3Frame.AOWWindow = PartyT1Pnl;
            Unit4Frame.AOWWindow = PartyT1Pnl;
            Unit5Frame.AOWWindow = PartyT3Pnl;
            Unit6Frame.AOWWindow = PartyT3Pnl;
            Unit7Frame.AOWWindow = PartyT3Pnl;
            Unit8Frame.AOWWindow = PartyT3Pnl;
            CTitleIcon.AOWWindow = ControlWindow;
            MovesLbl.AOWWindow = PartyBPnl;
            PartyStatusLbl.AOWWindow = PartyB2Pbl;
            Close.AOWWindow = ControlWindow;
            U1MIcon.AOWWindow = PartyT2Pnl;
            U2MIcon.AOWWindow = PartyT2Pnl;
            U3MIcon.AOWWindow = PartyT2Pnl;
            U4MIcon.AOWWindow = PartyT2Pnl;
            U5MIcon.AOWWindow = PartyT4Pnl;
            U6MIcon.AOWWindow = PartyT4Pnl;
            U7MIcon.AOWWindow = PartyT4Pnl;
            U8MIcon.AOWWindow = PartyT4Pnl;
            MoraleMask.AOWWindow = PartyBPnl;
            MoveIcon.AOWWindow = PartyBPnl;
            PartyT1Pnl.AOWWindow = ControlWindow;
            PartyBPnl.AOWWindow = ControlWindow;
            PartyT2Pnl.AOWWindow = ControlWindow;
            PartyT3Pnl.AOWWindow = ControlWindow;
            PartyT4Pnl.AOWWindow = ControlWindow;
            Unit1Gge.AOWWindow = PartyT1Pnl;
            Unit2Gge.AOWWindow = PartyT1Pnl;
            Unit3Gge.AOWWindow = PartyT1Pnl;
            Unit4Gge.AOWWindow = PartyT1Pnl;
            Unit5Gge.AOWWindow = PartyT3Pnl;
            Unit6Gge.AOWWindow = PartyT3Pnl;
            Unit7Gge.AOWWindow = PartyT3Pnl;
            Unit8Gge.AOWWindow = PartyT3Pnl;
            Unit1Chk.AOWWindow = PartyT2Pnl;
            Unit2Chk.AOWWindow = PartyT2Pnl;
            Unit3Chk.AOWWindow = PartyT2Pnl;
            Unit5Chk.AOWWindow = PartyT4Pnl;
            Unit6Chk.AOWWindow = PartyT4Pnl;
            Unit7Chk.AOWWindow = PartyT4Pnl;
            Unit4Chk.AOWWindow = PartyT2Pnl;
            Unit8Chk.AOWWindow = PartyT4Pnl;
            Emp1Fr.AOWWindow = PartyT1Pnl;
            Emp2Fr.AOWWindow = PartyT1Pnl;
            Emp3Fr.AOWWindow = PartyT1Pnl;
            Emp4Fr.AOWWindow = PartyT1Pnl;
            Emp5Fr.AOWWindow = PartyT3Pnl;
            Emp6Fr.AOWWindow = PartyT3Pnl;
            Emp7Fr.AOWWindow = PartyT3Pnl;
            Emp8Fr.AOWWindow = PartyT3Pnl;
            Emp1Chk.AOWWindow = PartyT2Pnl;
            Emp2Chk.AOWWindow = PartyT2Pnl;
            Emp3Chk.AOWWindow = PartyT2Pnl;
            Emp4Chk.AOWWindow = PartyT2Pnl;
            Emp5Chk.AOWWindow = PartyT4Pnl;
            Emp6Chk.AOWWindow = PartyT4Pnl;
            Emp7Chk.AOWWindow = PartyT4Pnl;
            Emp8Chk.AOWWindow = PartyT4Pnl;
            PartyIPnl.AOWWindow = ControlWindow;
            PartyILbl.AOWWindow = PartyIPnl;
            Center.AOWWindow = ControlWindow;
            Help.AOWWindow = ControlWindow;
            PPrev.AOWWindow = PControlPnl;
            PMove.AOWWindow = PControlPnl;
            PNext.AOWWindow = PControlPnl;
            PControlPnl.AOWWindow = ControlWindow;
            PGuard.AOWWindow = PControlPnl;
            AoWFrame2.AOWWindow = PartyBPnl;
            PartyB2Pbl.AOWWindow = ControlWindow;
            AoWFrame1.AOWWindow = PartyB2Pbl;
            PartyActionLbl1.AOWWindow = PartyIPnl;
            PartyActionLbl2.AOWWindow = PartyIPnl;
            Loaded();
            ControlWindow.Loaded();
            CWTitle.Loaded();
            CWMinIcon.Loaded();
            Unit1Frame.Loaded();
            Unit2Frame.Loaded();
            Unit3Frame.Loaded();
            Unit4Frame.Loaded();
            Unit5Frame.Loaded();
            Unit6Frame.Loaded();
            Unit7Frame.Loaded();
            Unit8Frame.Loaded();
            CTitleIcon.Loaded();
            MovesLbl.Loaded();
            PartyStatusLbl.Loaded();
            Close.Loaded();
            U1MIcon.Loaded();
            U2MIcon.Loaded();
            U3MIcon.Loaded();
            U4MIcon.Loaded();
            U5MIcon.Loaded();
            U6MIcon.Loaded();
            U7MIcon.Loaded();
            U8MIcon.Loaded();
            MoraleMask.Loaded();
            MoveIcon.Loaded();
            PartyT1Pnl.Loaded();
            PartyBPnl.Loaded();
            PartyT2Pnl.Loaded();
            PartyT3Pnl.Loaded();
            PartyT4Pnl.Loaded();
            Unit1Gge.Loaded();
            Unit2Gge.Loaded();
            Unit3Gge.Loaded();
            Unit4Gge.Loaded();
            Unit5Gge.Loaded();
            Unit6Gge.Loaded();
            Unit7Gge.Loaded();
            Unit8Gge.Loaded();
            Unit1Chk.Loaded();
            Unit2Chk.Loaded();
            Unit3Chk.Loaded();
            Unit5Chk.Loaded();
            Unit6Chk.Loaded();
            Unit7Chk.Loaded();
            Unit4Chk.Loaded();
            Unit8Chk.Loaded();
            Emp1Fr.Loaded();
            Emp2Fr.Loaded();
            Emp3Fr.Loaded();
            Emp4Fr.Loaded();
            Emp5Fr.Loaded();
            Emp6Fr.Loaded();
            Emp7Fr.Loaded();
            Emp8Fr.Loaded();
            Emp1Chk.Loaded();
            Emp2Chk.Loaded();
            Emp3Chk.Loaded();
            Emp4Chk.Loaded();
            Emp5Chk.Loaded();
            Emp6Chk.Loaded();
            Emp7Chk.Loaded();
            Emp8Chk.Loaded();
            PartyIPnl.Loaded();
            PartyILbl.Loaded();
            Center.Loaded();
            Help.Loaded();
            PPrev.Loaded();
            PMove.Loaded();
            PNext.Loaded();
            PControlPnl.Loaded();
            PGuard.Loaded();
            AoWFrame2.Loaded();
            PartyB2Pbl.Loaded();
            AoWFrame1.Loaded();
            Translator.Loaded();
            PartyActionLbl1.Loaded();
            PartyActionLbl2.Loaded();
            session.Resolve(this);
            ControlWindow.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            ControlWindow.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ControlWindow.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CWMinIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CWMinIcon.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Unit1Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit1Frame.PopupMenu = module.BannerPopup);
            Unit2Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit2Frame.PopupMenu = module.BannerPopup);
            Unit3Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit3Frame.PopupMenu = module.BannerPopup);
            Unit4Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit4Frame.PopupMenu = module.BannerPopup);
            Unit5Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit5Frame.PopupMenu = module.BannerPopup);
            Unit6Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit6Frame.PopupMenu = module.BannerPopup);
            Unit7Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit7Frame.PopupMenu = module.BannerPopup);
            Unit8Frame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            session.Defer("UnitBanner", (Aow.Units.AoWUnitBanner.TUnitBanner module) => Unit8Frame.PopupMenu = module.BannerPopup);
            CTitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            MovesLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MovesLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PartyStatusLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PartyStatusLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U1MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U1MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U2MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U2MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U3MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U3MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U4MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U4MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U5MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U5MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U6MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U6MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U7MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U7MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            U8MIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            U8MIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            MoraleMask.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MoveIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MoveIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            Unit1Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit1Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit1Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit1Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit1Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit2Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit2Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit2Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit3Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit3Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit3Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit5Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit5Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit5Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit6Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit6Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit6Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit7Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit7Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit7Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit4Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit4Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit4Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Unit8Chk.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Unit8Chk.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit8Chk.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp1Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp1Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp2Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp2Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp3Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp3Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp4Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp4Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp5Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp5Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp6Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp6Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp7Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp7Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp8Fr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp8Fr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp1Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp2Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp3Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp4Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp5Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp6Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp7Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            Emp8Chk.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            PartyIPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            PartyIPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            PartyILbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Center.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Center.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PPrev.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PPrev.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PMove.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PMove.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PNext.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PNext.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PGuard.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PGuard.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PGuard.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AoWFrame2.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            PartyActionLbl1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            PartyActionLbl2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
        }

        private void Layout_ControlWindowResize_0(TObject p0)
        {
            ControlWindowResize();
        }

        private void Layout_CWMinIconClick_1(TObject p0)
        {
            CWMinIconClick();
        }

        private void Layout_Unit1FrameMouseDown_2(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            Unit1FrameMouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_CenterClick_3(TObject p0)
        {
            CenterClick();
        }

        private void Layout_HelpClick_4(TObject p0)
        {
            HelpClick();
        }

        private void Layout_PPrevClick_5(TObject p0)
        {
            PPrevClick();
        }

        private void Layout_PMoveClick_6(TObject p0)
        {
            PMoveClick();
        }

        private void Layout_PNextClick_7(TObject p0)
        {
            PNextClick();
        }

        private void Layout_PGuardChange_8(TObject p0)
        {
            PGuardChange();
        }
    }
}
