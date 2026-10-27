// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWWorldMap
{
    public partial class TWorldMap
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "WorldMap";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_WorldMapCreate_0;
            MemoL = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MemoL",
                Priority = 500,
                WinLeft = 50,
                WinTop = 70,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 53,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 40,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 107
                },
                Masked = true,
                WinWidth = 252,
                WinHeight = 288,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1251110,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 30,
                ILIndexTexture = 0,
                ItemHeight = 18,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(MemoL);
            PicR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PicR",
                Priority = 200,
                WinLeft = 301,
                WinTop = 11,
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
                    LeftOffset = 301,
                    RightOffset = 0,
                    TopOffset = 11,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 0,
                WinWidth = 272,
                WinHeight = 372,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PicR);
            WorldPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "WorldPnl",
                OnMouseMove = Layout_WorldPnlMouseMove_1,
                OnUpdate = Layout_WorldPnlUpdate_2,
                Visible = false,
                Priority = 10,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = false,
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
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(WorldPnl);
            Path1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Path1",
                OnMouseDown = Layout_Path1MouseDown_3,
                OnMouseMove = Layout_Path1MouseMove_4,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 8,
                WinWidth = 29,
                WinHeight = 28,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Path1);
            Path2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Path2",
                OnMouseDown = Layout_Path2MouseDown_5,
                OnMouseMove = Layout_Path2MouseMove_6,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 8,
                WinWidth = 29,
                WinHeight = 28,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Path2);
            Path3 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Path3",
                OnMouseDown = Layout_Path3MouseDown_7,
                OnMouseMove = Layout_Path3MouseMove_8,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 8,
                WinWidth = 29,
                WinHeight = 28,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Path3);
            PPawn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PPawn",
                OnMouseDown = PPawnMouseDown,
                OnMouseMove = Layout_PPawnMouseMove_9,
                Visible = false,
                Priority = 1,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 7,
                AnimEnd = 8,
                WinWidth = 58,
                WinHeight = 85,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PPawn);
            EPawn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "EPawn",
                OnMouseDown = PPawnMouseDown,
                OnMouseMove = Layout_PPawnMouseMove_9,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 8,
                WinWidth = 58,
                WinHeight = 85,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(EPawn);
            DescLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DescLbl",
                Priority = 1000,
                WinLeft = 320,
                WinTop = 460,
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
                TextColor = 0
            };
            LayoutSession.EndRead(DescLbl);
            Back = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Back",
                Priority = 20000,
                WinLeft = 28,
                WinTop = 343,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Previous page"
                },
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
                    BottomOffset = 82
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 2,
                ILIndexLit = 3,
                ILIndexUp = 1,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 55,
                OnClick = Layout_BackClick_10
            };
            LayoutSession.EndRead(Back);
            Next = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Next",
                Priority = 20000,
                WinLeft = 518,
                WinTop = 343,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Next Page"
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
                    LeftOffset = 28,
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 82
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 5,
                ILIndexLit = 6,
                ILIndexUp = 4,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 55,
                OnClick = Layout_NextStartClick_11
            };
            LayoutSession.EndRead(Next);
            Cancel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Cancel",
                Priority = 0,
                WinLeft = 520,
                WinTop = 386,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Close"
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
                    RightOffset = 40,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 8,
                ILIndexLit = 9,
                ILIndexUp = 7,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_CancelClick_12
            };
            LayoutSession.EndRead(Cancel);
            BookStart = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BookStart",
                Priority = 0,
                WinLeft = 475,
                WinTop = 386,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Accept"
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
                    RightOffset = 85,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 12,
                ILIndexUp = 10,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_BookStartClick_13
            };
            LayoutSession.EndRead(BookStart);
            LabelL = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LabelL",
                Priority = 500,
                WinLeft = 50,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(LabelL);
            MemoR = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MemoR",
                Priority = 500,
                WinLeft = 325,
                WinTop = 70,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 53,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 315,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 107
                },
                Masked = true,
                WinWidth = 252,
                WinHeight = 288,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 1251110,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 30,
                ILIndexTexture = 0,
                ItemHeight = 18,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(MemoR);
            LabelR = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LabelR",
                Priority = 500,
                WinLeft = 325,
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
                    LeftOffset = 325,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(LabelR);
            PicL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PicL",
                Priority = 200,
                WinLeft = 28,
                WinTop = 11,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 11,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 0,
                WinWidth = 272,
                WinHeight = 372,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PicL);
            BookWin = new Aow.Units.AoWBookWindow.TAoWBookWindow(this)
            {
                Name = "BookWin",
                OnUpdate = Layout_BookWinUpdate_14,
                Visible = false,
                Priority = 275,
                WinLeft = 21,
                WinTop = 8,
                WinWidth = 599,
                Masked = true,
                OnHide = Layout_BookWinHide_15,
                OnShow = Layout_BookWinShow_16,
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
                BorderWidth = 30,
                BorderHeight = 14,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 5808200,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 28,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 599,
                MinHeight = 465,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = true,
                WinHeight = 465,
                Dockable = false,
                OnAnimDone = Layout_BookWinAnimDone_17
            };
            LayoutSession.EndRead(BookWin);
            InfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoPnl",
                Visible = false,
                Priority = 10000,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 9,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoPnl);
            Elves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Elves",
                Priority = 2000,
                WinLeft = 300,
                WinTop = 150,
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
                    LeftOffset = 300,
                    RightOffset = 0,
                    TopOffset = 150,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Elves",
                TextColor = 0
            };
            LayoutSession.EndRead(Elves);
            Halflings = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Halflings",
                Priority = 2000,
                WinLeft = 100,
                WinTop = 140,
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
                    LeftOffset = 100,
                    RightOffset = 0,
                    TopOffset = 140,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Halflings",
                TextColor = 0
            };
            LayoutSession.EndRead(Halflings);
            Goblins = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Goblins",
                Priority = 2000,
                WinLeft = 120,
                WinTop = 189,
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 189,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Goblins",
                TextColor = 0
            };
            LayoutSession.EndRead(Goblins);
            Orcs = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Orcs",
                Priority = 2000,
                WinLeft = 90,
                WinTop = 290,
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
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 290,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Orcs",
                TextColor = 0
            };
            LayoutSession.EndRead(Orcs);
            Dwarves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Dwarves",
                Priority = 2000,
                WinLeft = 210,
                WinTop = 130,
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
                    LeftOffset = 210,
                    RightOffset = 0,
                    TopOffset = 130,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Dwarves",
                TextColor = 0
            };
            LayoutSession.EndRead(Dwarves);
            Undead = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Undead",
                Priority = 2000,
                WinLeft = 390,
                WinTop = 230,
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
                    LeftOffset = 390,
                    RightOffset = 0,
                    TopOffset = 230,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Undead",
                TextColor = 0
            };
            LayoutSession.EndRead(Undead);
            Highmen = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Highmen",
                Priority = 2000,
                WinLeft = 280,
                WinTop = 190,
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
                    LeftOffset = 280,
                    RightOffset = 0,
                    TopOffset = 190,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "High Men",
                TextColor = 0
            };
            LayoutSession.EndRead(Highmen);
            Lizardmen = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Lizardmen",
                Priority = 2000,
                WinLeft = 200,
                WinTop = 240,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 240,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Lizard Men",
                TextColor = 0
            };
            LayoutSession.EndRead(Lizardmen);
            DarkElves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DarkElves",
                Priority = 2000,
                WinLeft = 330,
                WinTop = 280,
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
                    LeftOffset = 330,
                    RightOffset = 0,
                    TopOffset = 280,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Dark Elves",
                TextColor = 0
            };
            LayoutSession.EndRead(DarkElves);
            VoW = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "VoW",
                Priority = 2000,
                WinLeft = 464,
                WinTop = 330,
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
                    RightOffset = 30,
                    TopOffset = 330,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Valley of Wonders",
                TextColor = 0
            };
            LayoutSession.EndRead(VoW);
            ElvenC = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ElvenC",
                Priority = 2000,
                WinLeft = 540,
                WinTop = 310,
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
                    LeftOffset = 540,
                    RightOffset = 0,
                    TopOffset = 310,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The Elven Court",
                TextColor = 11642509
            };
            LayoutSession.EndRead(ElvenC);
            IoLG = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IoLG",
                Priority = 2000,
                WinLeft = 506,
                WinTop = 100,
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
                    RightOffset = 30,
                    TopOffset = 100,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Isle of Last Goodbyes",
                TextColor = 11642509
            };
            LayoutSession.EndRead(IoLG);
            Deepmir = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Deepmir",
                Priority = 2000,
                WinLeft = 190,
                WinTop = 110,
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
                    LeftOffset = 190,
                    RightOffset = 30,
                    TopOffset = 110,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Deepmir",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Deepmir);
            Aldor = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Aldor",
                Priority = 2000,
                WinLeft = 40,
                WinTop = 110,
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
                    TopOffset = 110,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Aldor Isle",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Aldor);
            Serpent = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Serpent",
                Priority = 2000,
                WinLeft = 200,
                WinTop = 210,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 210,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Serpent River",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Serpent);
            Blackwater = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Blackwater",
                Priority = 2000,
                WinLeft = 270,
                WinTop = 225,
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
                    LeftOffset = 270,
                    RightOffset = 0,
                    TopOffset = 225,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Blackwater Lake",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Blackwater);
            Skull = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Skull",
                Priority = 2000,
                WinLeft = 390,
                WinTop = 200,
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
                    LeftOffset = 390,
                    RightOffset = 0,
                    TopOffset = 200,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The Skull",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Skull);
            Sylvanus = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Sylvanus",
                Priority = 2000,
                WinLeft = 300,
                WinTop = 130,
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
                    LeftOffset = 300,
                    RightOffset = 0,
                    TopOffset = 130,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Sylvanus",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Sylvanus);
            Pass = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Pass",
                Priority = 2000,
                WinLeft = 460,
                WinTop = 260,
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
                    LeftOffset = 460,
                    RightOffset = 0,
                    TopOffset = 260,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Pass of Grief",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Pass);
            Arcalot = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Arcalot",
                Priority = 2000,
                WinLeft = 360,
                WinTop = 250,
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
                    LeftOffset = 360,
                    RightOffset = 0,
                    TopOffset = 250,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Arcalot",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Arcalot);
            Flowershire = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Flowershire",
                Priority = 2000,
                WinLeft = 80,
                WinTop = 160,
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
                    TopOffset = 160,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Flowershire",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Flowershire);
            Sobek = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Sobek",
                Priority = 2000,
                WinLeft = 150,
                WinTop = 260,
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
                    TopOffset = 260,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Peninsula of Sobek",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Sobek);
            Steppe = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Steppe",
                Priority = 2000,
                WinLeft = 210,
                WinTop = 330,
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
                    LeftOffset = 210,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Ashen Steppe",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Steppe);
            Bronnam = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Bronnam",
                Priority = 2000,
                WinLeft = 210,
                WinTop = 80,
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
                    LeftOffset = 210,
                    RightOffset = 0,
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Bronnam Outpost",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Bronnam);
            UC = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UC",
                Priority = 2000,
                WinLeft = 180,
                WinTop = 170,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 170,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "United Cities",
                TextColor = 11642509
            };
            LayoutSession.EndRead(UC);
            Woods = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Woods",
                Priority = 2000,
                WinLeft = 390,
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
                    LeftOffset = 390,
                    RightOffset = 0,
                    TopOffset = 135,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Tollrock Woods",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Woods);
            Burbatak = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Burbatak",
                Priority = 2000,
                WinLeft = 440,
                WinTop = 190,
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
                    LeftOffset = 440,
                    RightOffset = 0,
                    TopOffset = 193,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Isle of Burbatak",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Burbatak);
            QuitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "QuitBtn",
                Priority = 20000,
                WinLeft = 500,
                WinTop = 448,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Quits the campaign"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 6,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Quit",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 130,
                OnClick = Layout_QuitBtnClick_18
            };
            LayoutSession.EndRead(QuitBtn);
            ShowBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ShowBtn",
                Priority = 20000,
                WinLeft = 500,
                WinTop = 418,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows information on map locations and allies"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 40
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 6,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Show Info",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 130,
                OnClick = Layout_ShowBtnClick_19
            };
            LayoutSession.EndRead(ShowBtn);
            SelectLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectLbl",
                Priority = 1000,
                WinLeft = 245,
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
                    LeftOffset = 330,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Select a Destination",
                TextColor = 0
            };
            LayoutSession.EndRead(SelectLbl);
            AlliesLnl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AlliesLnl",
                Priority = 2000,
                WinLeft = 58,
                WinTop = 4,
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
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "-Allies-",
                TextColor = 14211288
            };
            LayoutSession.EndRead(AlliesLnl);
            Frame = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Frame",
                Priority = 0,
                WinLeft = 80,
                WinTop = 390,
                WinWidth = 160,
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 10
                },
                WinHeight = 80,
                ILIndexFrame = 40,
                ILIndexTexture = 3,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Frame);
            Azrac = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Azrac",
                Priority = 2000,
                WinLeft = 200,
                WinTop = 364,
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
                    LeftOffset = 200,
                    RightOffset = 0,
                    TopOffset = 135,
                    BottomOffset = 105
                },
                Masked = true,
                Text = "Azrac Empire",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Azrac);
            StarImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "StarImg",
                OnDraw = StarImgDraw,
                OnUpdate = Layout_StarImgUpdate_20,
                Priority = 200,
                WinLeft = 461,
                WinTop = 193,
                CaptureInput = false,
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
                    RightOffset = 17,
                    TopOffset = 193,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 12,
                AnimEnd = 0,
                WinWidth = 162,
                WinHeight = 163,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(StarImg);
            Terstar = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Terstar",
                Priority = 2000,
                WinLeft = 50,
                WinTop = 270,
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
                    TopOffset = 270,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Terstar",
                TextColor = 11642509
            };
            LayoutSession.EndRead(Terstar);
            HeroPnl = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "HeroPnl",
                OnPreDraw = HeroPnlPreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = true,
                WinWidth = 65,
                WinHeight = 80,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(HeroPnl);
            Ally1Pnl = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Ally1Pnl",
                OnPreDraw = Ally1PnlPreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = true,
                WinWidth = 50,
                WinHeight = 60,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Ally1Pnl);
            Ally2Pnl = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Ally2Pnl",
                OnPreDraw = Ally2PnlPreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                    LeftOffset = 135,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = true,
                WinWidth = 50,
                WinHeight = 60,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Ally2Pnl);
            Ally3Pnl = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "Ally3Pnl",
                OnPreDraw = Ally3PnlPreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
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
                    LeftOffset = 190,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = true,
                WinWidth = 50,
                WinHeight = 60,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(Ally3Pnl);
            SetBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SetBtn",
                Priority = 20000,
                WinLeft = 500,
                WinTop = 418,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Allows you to change the Campaign's settings."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 70
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 6,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Settings",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 130,
                OnClick = Layout_SetBtnClick_21
            };
            LayoutSession.EndRead(SetBtn);
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
            leftSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "leftSB",
                Visible = false,
                Priority = 500,
                WinLeft = 269,
                WinTop = 70,
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
                LargeChange = 14,
                Position = 0,
                WinHeight = 283,
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
            LayoutSession.EndRead(leftSB);
            rightSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "rightSB",
                Visible = false,
                Priority = 500,
                WinLeft = 544,
                WinTop = 70,
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
                LargeChange = 14,
                Position = 0,
                WinHeight = 283,
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
            LayoutSession.EndRead(rightSB);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            MemoL.AOWWindow = BookWin;
            MemoL.VScrollBar = leftSB;
            PicR.AOWWindow = BookWin;
            Path1.AOWWindow = WorldPnl;
            Path2.AOWWindow = WorldPnl;
            Path3.AOWWindow = WorldPnl;
            PPawn.AOWWindow = WorldPnl;
            EPawn.AOWWindow = WorldPnl;
            DescLbl.AOWWindow = WorldPnl;
            Back.AOWWindow = BookWin;
            Next.AOWWindow = BookWin;
            Cancel.AOWWindow = BookWin;
            BookStart.AOWWindow = BookWin;
            LabelL.AOWWindow = BookWin;
            MemoR.AOWWindow = BookWin;
            MemoR.VScrollBar = rightSB;
            LabelR.AOWWindow = BookWin;
            PicL.AOWWindow = BookWin;
            InfoPnl.AOWWindow = WorldPnl;
            Elves.AOWWindow = InfoPnl;
            Halflings.AOWWindow = InfoPnl;
            Goblins.AOWWindow = InfoPnl;
            Orcs.AOWWindow = InfoPnl;
            Dwarves.AOWWindow = InfoPnl;
            Undead.AOWWindow = InfoPnl;
            Highmen.AOWWindow = InfoPnl;
            Lizardmen.AOWWindow = InfoPnl;
            DarkElves.AOWWindow = InfoPnl;
            VoW.AOWWindow = InfoPnl;
            ElvenC.AOWWindow = InfoPnl;
            IoLG.AOWWindow = InfoPnl;
            Deepmir.AOWWindow = InfoPnl;
            Aldor.AOWWindow = InfoPnl;
            Serpent.AOWWindow = InfoPnl;
            Blackwater.AOWWindow = InfoPnl;
            Skull.AOWWindow = InfoPnl;
            Sylvanus.AOWWindow = InfoPnl;
            Pass.AOWWindow = InfoPnl;
            Arcalot.AOWWindow = InfoPnl;
            Flowershire.AOWWindow = InfoPnl;
            Sobek.AOWWindow = InfoPnl;
            Steppe.AOWWindow = InfoPnl;
            Bronnam.AOWWindow = InfoPnl;
            UC.AOWWindow = InfoPnl;
            Woods.AOWWindow = InfoPnl;
            Burbatak.AOWWindow = InfoPnl;
            QuitBtn.AOWWindow = WorldPnl;
            ShowBtn.AOWWindow = WorldPnl;
            SelectLbl.AOWWindow = WorldPnl;
            AlliesLnl.AOWWindow = Frame;
            Frame.AOWWindow = InfoPnl;
            Azrac.AOWWindow = InfoPnl;
            StarImg.AOWWindow = WorldPnl;
            Terstar.AOWWindow = InfoPnl;
            HeroPnl.AOWWindow = InfoPnl;
            Ally1Pnl.AOWWindow = InfoPnl;
            Ally2Pnl.AOWWindow = InfoPnl;
            Ally3Pnl.AOWWindow = InfoPnl;
            SetBtn.AOWWindow = WorldPnl;
            leftSB.AOWWindow = BookWin;
            rightSB.AOWWindow = BookWin;
            Loaded();
            MemoL.Loaded();
            PicR.Loaded();
            WorldPnl.Loaded();
            Path1.Loaded();
            Path2.Loaded();
            Path3.Loaded();
            PPawn.Loaded();
            EPawn.Loaded();
            DescLbl.Loaded();
            Back.Loaded();
            Next.Loaded();
            Cancel.Loaded();
            BookStart.Loaded();
            LabelL.Loaded();
            MemoR.Loaded();
            LabelR.Loaded();
            PicL.Loaded();
            BookWin.Loaded();
            InfoPnl.Loaded();
            Elves.Loaded();
            Halflings.Loaded();
            Goblins.Loaded();
            Orcs.Loaded();
            Dwarves.Loaded();
            Undead.Loaded();
            Highmen.Loaded();
            Lizardmen.Loaded();
            DarkElves.Loaded();
            VoW.Loaded();
            ElvenC.Loaded();
            IoLG.Loaded();
            Deepmir.Loaded();
            Aldor.Loaded();
            Serpent.Loaded();
            Blackwater.Loaded();
            Skull.Loaded();
            Sylvanus.Loaded();
            Pass.Loaded();
            Arcalot.Loaded();
            Flowershire.Loaded();
            Sobek.Loaded();
            Steppe.Loaded();
            Bronnam.Loaded();
            UC.Loaded();
            Woods.Loaded();
            Burbatak.Loaded();
            QuitBtn.Loaded();
            ShowBtn.Loaded();
            SelectLbl.Loaded();
            AlliesLnl.Loaded();
            Frame.Loaded();
            Azrac.Loaded();
            StarImg.Loaded();
            Terstar.Loaded();
            HeroPnl.Loaded();
            Ally1Pnl.Loaded();
            Ally2Pnl.Loaded();
            Ally3Pnl.Loaded();
            SetBtn.Loaded();
            Translator.Loaded();
            leftSB.Loaded();
            rightSB.Loaded();
            session.Resolve(this);
            MemoL.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MemoL.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            WorldPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            WorldPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Path1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Path2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Path3.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            PPawn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            EPawn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            DescLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Back.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Back.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Next.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Next.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Cancel.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Cancel.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            BookStart.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BookStart.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            LabelL.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            MemoR.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MemoR.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LabelR.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            BookWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            BookWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            BookWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            InfoPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Elves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Halflings.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Goblins.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Orcs.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Dwarves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Undead.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Highmen.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Lizardmen.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            DarkElves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            VoW.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ElvenC.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            IoLG.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Deepmir.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Aldor.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Serpent.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Blackwater.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Skull.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Sylvanus.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Pass.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Arcalot.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Flowershire.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Sobek.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Steppe.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Bronnam.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            UC.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Woods.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Burbatak.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            QuitBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            QuitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            QuitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ShowBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ShowBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ShowBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SelectLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            AlliesLnl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Azrac.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            StarImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.WorldMap;
            Terstar.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            HeroPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Ally1Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Ally2Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Ally3Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SetBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SetBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SetBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            leftSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            leftSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            leftSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            leftSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            rightSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            rightSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            rightSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            rightSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_WorldMapCreate_0(TObject p0)
        {
            WorldMapCreate();
        }

        private void Layout_WorldPnlMouseMove_1(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            WorldPnlMouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_WorldPnlUpdate_2(TObject p0)
        {
            WorldPnlUpdate();
        }

        private void Layout_Path1MouseDown_3(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            Path1MouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_Path1MouseMove_4(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            Path1MouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_Path2MouseDown_5(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            Path2MouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_Path2MouseMove_6(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            Path2MouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_Path3MouseDown_7(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            Path3MouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_Path3MouseMove_8(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            Path3MouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_PPawnMouseMove_9(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            PPawnMouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_BackClick_10(TObject p0)
        {
            BackClick();
        }

        private void Layout_NextStartClick_11(TObject p0)
        {
            NextStartClick();
        }

        private void Layout_CancelClick_12(TObject p0)
        {
            CancelClick();
        }

        private void Layout_BookStartClick_13(TObject p0)
        {
            BookStartClick();
        }

        private void Layout_BookWinUpdate_14(TObject p0)
        {
            BookWinUpdate();
        }

        private void Layout_BookWinHide_15(TObject p0)
        {
            BookWinHide();
        }

        private void Layout_BookWinShow_16(TObject p0)
        {
            BookWinShow();
        }

        private void Layout_BookWinAnimDone_17(TObject p0)
        {
            BookWinAnimDone();
        }

        private void Layout_QuitBtnClick_18(TObject p0)
        {
            QuitBtnClick();
        }

        private void Layout_ShowBtnClick_19(TObject p0)
        {
            ShowBtnClick();
        }

        private void Layout_StarImgUpdate_20(TObject p0)
        {
            StarImgUpdate();
        }

        private void Layout_SetBtnClick_21(TObject p0)
        {
            SetBtnClick();
        }
    }
}
