// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWInfoWin
{
    public partial class TIWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "IWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            InfoWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "InfoWin",
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
                    BottomOffset = 0
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
                OnDrag = Layout_InfoWinResize_0,
                OnResize = Layout_InfoWinResize_0,
                MinWidth = 200,
                MinHeight = 216,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 216,
                Dockable = false
            };
            LayoutSession.EndRead(InfoWin);
            Min = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Min",
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
                OnClick = Layout_MinClick_1
            };
            LayoutSession.EndRead(Min);
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
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 1000,
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
                    RightOffset = 10,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Structure",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            SIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SIcon",
                Priority = 0,
                WinLeft = 2,
                WinTop = 1,
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SIcon);
            MainBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MainBtn",
                Priority = 500,
                WinLeft = 10,
                WinTop = 184,
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
                    RightOffset = 82,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Enter",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 108,
                OnClick = Layout_MainBtnClick_2
            };
            LayoutSession.EndRead(MainBtn);
            CeltFrame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "CeltFrame",
                Priority = 13,
                WinLeft = 5,
                WinTop = 19,
                CaptureInput = false,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 19,
                    BottomOffset = 5
                },
                Masked = false,
                WinWidth = 190,
                WinHeight = 192,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 22,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(CeltFrame);
            StructLine = new Aow.Units.AoWArnoLine.TAoWArnoLine(this)
            {
                Name = "StructLine",
                Priority = 0,
                WinLeft = 188,
                WinTop = 46,
                CaptureInput = false,
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
                    RightOffset = 12,
                    TopOffset = 46,
                    BottomOffset = 31
                },
                Masked = false,
                WinHeight = 139
            };
            LayoutSession.EndRead(StructLine);
            StructNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StructNameLbl",
                Priority = 100,
                WinLeft = 48,
                WinTop = 10,
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
                Masked = true,
                Text = "#####",
                TextColor = 16777215
            };
            LayoutSession.EndRead(StructNameLbl);
            ProductionPlacePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ProductionPlacePnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ProductionPlacePnl);
            NamePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NamePnl",
                Priority = 400,
                WinLeft = 4,
                WinTop = 17,
                WinWidth = 136,
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
                    LeftOffset = 4,
                    RightOffset = 60,
                    TopOffset = 17,
                    BottomOffset = 31
                },
                WinHeight = 27,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NamePnl);
            ActiveActionMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ActiveActionMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 37,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 27,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 108,
                WinHeight = 101,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ActiveActionMemo);
            SignPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SignPnl",
                Visible = false,
                Priority = 1000,
                WinLeft = 5,
                WinTop = 18,
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
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 18,
                    BottomOffset = 5
                },
                WinHeight = 193,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SignPnl);
            SignText = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "SignText",
                Priority = 500,
                WinLeft = 0,
                WinTop = 27,
                CaptureInput = false,
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
                    TopOffset = 27,
                    BottomOffset = 27
                },
                Masked = false,
                WinWidth = 190,
                WinHeight = 139,
                BorderWidth = 12,
                BorderHeight = 20,
                TextColor = 9688812,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = true,
                ILIndexFrame = 40,
                ILIndexTexture = 10,
                ItemHeight = 24,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(SignText);
            SignLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SignLbl",
                Priority = 2500,
                WinLeft = 53,
                WinTop = 7,
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
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The sign reads:",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SignLbl);
            SignTLine = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "SignTLine",
                Priority = 500,
                WinLeft = 0,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 190,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(SignTLine);
            SignTDecoL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SignTDecoL",
                Priority = 1000,
                WinLeft = 8,
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SignTDecoL);
            SignTDecoR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SignTDecoR",
                Priority = 1000,
                WinLeft = 155,
                WinTop = 0,
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
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SignTDecoR);
            SignBLine = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "SignBLine",
                Priority = 500,
                WinLeft = 0,
                WinTop = 166,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 190,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(SignBLine);
            SignBDecoL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SignBDecoL",
                Priority = 1000,
                WinLeft = 8,
                WinTop = 166,
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SignBDecoL);
            SingBDemoR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SingBDemoR",
                Priority = 1000,
                WinLeft = 155,
                WinTop = 166,
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
                    RightOffset = 8,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 27,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SingBDemoR);
            SignTextSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SignTextSB",
                Priority = 500,
                WinLeft = 172,
                WinTop = 27,
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
                LargeChange = 4,
                Position = 0,
                WinHeight = 139,
                WinWidth = 18,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 2,
                ILIndexIncDown = 3,
                ILIndexDecUp = 0,
                ILIndexDecDown = 1,
                ILIndexBack = 7,
                ILIndexTab = 4
            };
            LayoutSession.EndRead(SignTextSB);
            CityPanel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CityPanel",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CityPanel);
            CityRaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CityRaceLbl",
                Priority = 0,
                WinLeft = 27,
                WinTop = 73,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 53,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 0
            };
            LayoutSession.EndRead(CityRaceLbl);
            CityBorderPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CityBorderPnl",
                Visible = false,
                Priority = 14,
                WinLeft = 120,
                WinTop = 18,
                WinWidth = 80,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
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
                WinHeight = 194,
                ILIndexFrame = 115,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CityBorderPnl);
            ProdOrb = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ProdOrb",
                OnDraw = Layout_ProdOrb2Draw_3,
                Priority = 5,
                WinLeft = 20,
                WinTop = 102,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 55
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 1,
                WinWidth = 39,
                WinHeight = 37,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ProdOrb);
            SingleButPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SingleButPnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SingleButPnl);
            SBmemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "SBmemo",
                Priority = 0,
                WinLeft = 5,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 1,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 109,
                WinHeight = 127,
                BorderWidth = 4,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 16,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(SBmemo);
            AltarPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AltarPnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AltarPnl);
            Bar = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Bar",
                Priority = 0,
                WinLeft = 6,
                WinTop = 26,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 6,
                    TopOffPercent = 8,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 50
                },
                Masked = false,
                WinWidth = 102,
                WinHeight = 20,
                Min = 0,
                Max = 100,
                Position = 50,
                ILIndexFrame = 40,
                ILIndexTNormal = 14,
                ILIndexTSelected = 6
            };
            LayoutSession.EndRead(Bar);
            ChargeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ChargeLbl",
                Priority = 0,
                WinLeft = 37,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 8,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Charge:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(ChargeLbl);
            GemLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GemLbl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 4,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 53,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 16777215
            };
            LayoutSession.EndRead(GemLbl);
            ChargePcntLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ChargePcntLbl",
                Priority = 0,
                WinLeft = 45,
                WinTop = 31,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 8,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 0
            };
            LayoutSession.EndRead(ChargePcntLbl);
            T2GemI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "T2GemI",
                Priority = 10,
                WinLeft = 2,
                WinTop = 2,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 53,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 27,
                    TopOffset = 2,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 3,
                WinWidth = 18,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(T2GemI);
            T2TurnI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "T2TurnI",
                Priority = 10,
                WinLeft = 72,
                WinTop = 3,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 66,
                    RightOffPercent = 18,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 3,
                WinWidth = 12,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(T2TurnI);
            DayLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DayLbl",
                Priority = 0,
                WinLeft = 84,
                WinTop = 4,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 66,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 15,
                    RightOffset = 4,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 16777215
            };
            LayoutSession.EndRead(DayLbl);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Priority = 1000,
                WinLeft = 71,
                WinTop = 80,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 33,
                AnimEnd = 38,
                WinWidth = 43,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LShield);
            NotYoursPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NotYoursPnl",
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NotYoursPnl);
            NotYoursMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "NotYoursMemo",
                Priority = 100,
                WinLeft = 5,
                WinTop = 9,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 7,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                Masked = true,
                WinWidth = 109,
                WinHeight = 94,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(NotYoursMemo);
            Ownerlbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Ownerlbl",
                Priority = 0,
                WinLeft = 11,
                WinTop = 107,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 20
                },
                Masked = true,
                Text = "Owned by:",
                TextColor = 9236989
            };
            LayoutSession.EndRead(Ownerlbl);
            OwnerRaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "OwnerRaceLbl",
                Priority = 0,
                WinLeft = 11,
                WinTop = 122,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = true,
                Text = "#####",
                TextColor = 16777215
            };
            LayoutSession.EndRead(OwnerRaceLbl);
            ProdOrbTurnsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ProdOrbTurnsLbl",
                Priority = 10,
                WinLeft = 41,
                WinTop = 145,
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
                    LeftOffset = 41,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 38
                },
                Masked = true,
                Text = "##",
                TextColor = 2829099
            };
            LayoutSession.EndRead(ProdOrbTurnsLbl);
            orLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "orLbl",
                Priority = 0,
                WinLeft = 55,
                WinTop = 4,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 54,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "or",
                TextColor = 7921903
            };
            LayoutSession.EndRead(orLbl);
            AltarInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AltarInfoPnl",
                Priority = 100,
                WinLeft = 6,
                WinTop = 87,
                WinWidth = 102,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 6,
                    TopOffPercent = 50,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 18,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 40,
                ILIndexTexture = 14,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AltarInfoPnl);
            ProdOrbTime = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ProdOrbTime",
                Priority = 5,
                WinLeft = 25,
                WinTop = 143,
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
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 150,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 1,
                WinWidth = 30,
                WinHeight = 13,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ProdOrbTime);
            Center = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Center",
                Priority = 0,
                WinLeft = 153,
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
                OnClick = Layout_CenterClick_4
            };
            LayoutSession.EndRead(Center);
            ConstructionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ConstructionPnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ConstructionPnl);
            ConstructionMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ConstructionMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 13,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 10,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 108,
                WinHeight = 125,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ConstructionMemo);
            UnderConstructionLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UnderConstructionLbl",
                Priority = 0,
                WinLeft = 15,
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
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Under Construction",
                TextColor = 9236989
            };
            LayoutSession.EndRead(UnderConstructionLbl);
            ConstructionDaysLeftLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ConstructionDaysLeftLbl",
                Priority = 0,
                WinLeft = 32,
                WinTop = 120,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ConstructionDaysLeftLbl);
            RazePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RazePnl",
                Priority = 2000,
                WinLeft = 128,
                WinTop = 174,
                WinWidth = 56,
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
                    RightOffset = 16,
                    TopOffset = 17,
                    BottomOffset = 7
                },
                WinHeight = 35,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RazePnl);
            RazeBg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RazeBg",
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
                CurrentImage = 74,
                AnimEnd = 0,
                WinWidth = 56,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RazeBg);
            RazeBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RazeBtn",
                Priority = 0,
                WinLeft = 8,
                WinTop = 13,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Razes Object"
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 54,
                ILIndexLit = 55,
                ILIndexUp = 53,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 43,
                OnClick = Layout_RazeBtnClick_5
            };
            LayoutSession.EndRead(RazeBtn);
            CivilStatusLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CivilStatusLbl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 126,
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
                    BottomOffPercent = 1,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 0
            };
            LayoutSession.EndRead(CivilStatusLbl);
            RaceRelationIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RaceRelationIcon",
                Tag = 20,
                Priority = 500,
                WinLeft = 32,
                WinTop = 86,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 63,
                    BottomOffPercent = 0,
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RaceRelationIcon);
            CityIncomeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CityIncomeLbl",
                Priority = 0,
                WinLeft = 32,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Town's Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 32,
                    RightOffPercent = 53,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 16777215
            };
            LayoutSession.EndRead(CityIncomeLbl);
            GoldIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "GoldIcn",
                Priority = 10,
                WinLeft = 10,
                WinTop = 2,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Town's Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 3,
                WinWidth = 15,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(GoldIcn);
            UgradeI = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UgradeI",
                Priority = 10,
                WinLeft = 61,
                WinTop = 3,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Upgrade Level",
                    ""
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 60,
                    RightOffPercent = 18,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 3,
                WinWidth = 11,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UgradeI);
            CityLevelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CityLevelLbl",
                Priority = 0,
                WinLeft = 81,
                WinTop = 4,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 80,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 4,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##",
                TextColor = 16777215
            };
            LayoutSession.EndRead(CityLevelLbl);
            InfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoPnl",
                Priority = 100,
                WinLeft = 6,
                WinTop = 48,
                WinWidth = 102,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 6,
                    TopOffPercent = 32,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoPnl);
            ProductionPlaceInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ProductionPlaceInfoPnl",
                Priority = 100,
                WinLeft = 6,
                WinTop = 100,
                WinWidth = 102,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 6,
                    TopOffPercent = 70,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ProductionPlaceInfoPnl);
            IncomeTypeIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "IncomeTypeIcon",
                Priority = 10,
                WinLeft = 34,
                WinTop = 2,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structures Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 3,
                WinWidth = 15,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(IncomeTypeIcon);
            IncomeValueLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IncomeValueLbl",
                Priority = 0,
                WinLeft = 53,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "20",
                TextColor = 16777215
            };
            LayoutSession.EndRead(IncomeValueLbl);
            ActiveCityActionMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ActiveCityActionMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 8,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 108,
                WinHeight = 127,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ActiveCityActionMemo);
            CostToFireLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CostToFireLbl",
                Priority = 0,
                WinLeft = 22,
                WinTop = 69,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 50,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Cost to Fire:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(CostToFireLbl);
            CurrentActivityLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CurrentActivityLbl",
                Priority = 0,
                WinLeft = 13,
                WinTop = 20,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
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
                    TopOffPercent = 15,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Current Activity:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(CurrentActivityLbl);
            IncomeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IncomeLbl",
                Priority = 0,
                WinLeft = 37,
                WinTop = 82,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
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
                    TopOffPercent = 60,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Income:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(IncomeLbl);
            ProductionWarningLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ProductionWarningLbl",
                Priority = 0,
                WinLeft = 21,
                WinTop = 35,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 8,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 24,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "not enough gold",
                TextColor = 128
            };
            LayoutSession.EndRead(ProductionWarningLbl);
            CurrentActivityWarningLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CurrentActivityWarningLbl",
                Visible = false,
                Priority = 0,
                WinLeft = 21,
                WinTop = 61,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 27,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 24,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "not enough gold",
                TextColor = 128
            };
            LayoutSession.EndRead(CurrentActivityWarningLbl);
            IncomePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IncomePnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IncomePnl);
            IncomInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IncomInfoPnl",
                Priority = 100,
                WinLeft = 6,
                WinTop = 110,
                WinWidth = 102,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 6,
                    TopOffPercent = 77,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IncomInfoPnl);
            IncomeTypeImage = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "IncomeTypeImage",
                Priority = 10,
                WinLeft = 34,
                WinTop = 2,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structures Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 38
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 33,
                AnimEnd = 3,
                WinWidth = 15,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(IncomeTypeImage);
            IncomeAmountLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IncomeAmountLbl",
                Priority = 0,
                WinLeft = 53,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "20",
                TextColor = 16777215
            };
            LayoutSession.EndRead(IncomeAmountLbl);
            IncomeTextLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IncomeTextLbl",
                Priority = 0,
                WinLeft = 37,
                WinTop = 96,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
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
                    TopOffPercent = 70,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Income:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(IncomeTextLbl);
            IncomeMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 13,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 10,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 108,
                WinHeight = 125,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(IncomeMemo);
            ItemPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ItemPnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ItemPnl);
            ItemsInBagLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ItemsInBagLbl",
                Priority = 0,
                WinLeft = 22,
                WinTop = 28,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Structure's Income"
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
                    TopOffPercent = 6,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Items in bag:",
                TextColor = 7921903
            };
            LayoutSession.EndRead(ItemsInBagLbl);
            ItembagLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "ItembagLB",
                Priority = 10,
                WinLeft = 18,
                WinTop = 47,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 82,
                    RightOffPercent = 0,
                    TopOffPercent = 20,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 78,
                WinHeight = 60,
                LayoutItems = new LayoutStrings()
                {
                    "99",
                    "99",
                    "99",
                    "99",
                    "99",
                    "99",
                    "99",
                    "99",
                    "99"
                },
                BorderWidth = 6,
                BorderHeight = 3,
                Selected = -1,
                OnDrawItem = Layout_ItembagLBDrawItem_6,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 14,
                ILIndexTSelected = 14,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ItembagLB);
            ItembagSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ItembagSB",
                Priority = 10,
                WinLeft = 79,
                WinTop = 47,
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
                Max = 8,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 60,
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
            LayoutSession.EndRead(ItembagSB);
            ItemBagMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ItemBagMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 13,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 10,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 108,
                WinHeight = 125,
                LayoutItems = new LayoutStrings()
                {
                    "Move onto the item bag to see what is in it."
                },
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ItemBagMemo);
            SBSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SBSB",
                Priority = 0,
                WinLeft = 97,
                WinTop = 11,
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
                Max = 12,
                SmallChange = 1,
                LargeChange = 6,
                Position = 0,
                WinHeight = 127,
                WinWidth = 17,
                NumberVisible = 6,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(SBSB);
            NotYoursMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "NotYoursMemoSB",
                Visible = false,
                Priority = 101,
                WinLeft = 97,
                WinTop = 9,
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
                WinHeight = 94,
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
            LayoutSession.EndRead(NotYoursMemoSB);
            ExplorationSitePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ExplorationSitePnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 45,
                WinWidth = 114,
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
                    LeftOffset = 4,
                    RightOffset = 82,
                    TopOffset = 45,
                    BottomOffset = 33
                },
                WinHeight = 138,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ExplorationSitePnl);
            ESMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ESMemo",
                Priority = 0,
                WinLeft = 5,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 1,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 30
                },
                Masked = true,
                WinWidth = 109,
                WinHeight = 92,
                BorderWidth = 4,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 16,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ESMemo);
            ESSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ESSB",
                Visible = false,
                Priority = 0,
                WinLeft = 97,
                WinTop = 11,
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
                LargeChange = 4,
                Position = 0,
                WinHeight = 92,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_ESSBChange_7,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(ESSB);
            ESLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ESLbl",
                Priority = 0,
                WinLeft = 38,
                WinTop = 109,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 17
                },
                Masked = true,
                Text = "####",
                TextColor = 9236989
            };
            LayoutSession.EndRead(ESLbl);
            ESStatusLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ESStatusLbl",
                Priority = 0,
                WinLeft = 34,
                WinTop = 124,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                Masked = true,
                Text = "#####",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ESStatusLbl);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Priority = 0,
                WinLeft = 138,
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
                    RightOffset = 47,
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
                OnClick = Layout_HelpClick_8
            };
            LayoutSession.EndRead(Help);
            PopulationRaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopulationRaceLbl",
                Priority = 0,
                WinLeft = 36,
                WinTop = 47,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 80
                },
                Masked = true,
                Text = "#######",
                TextColor = 16777215
            };
            LayoutSession.EndRead(PopulationRaceLbl);
            PopulationLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PopulationLbl",
                Priority = 0,
                WinLeft = 34,
                WinTop = 32,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 10,
                    RightOffPercent = 0,
                    TopOffPercent = 58,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 95
                },
                Masked = true,
                Text = "Population:",
                TextColor = 9236989
            };
            LayoutSession.EndRead(PopulationLbl);
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
            Min.AOWWindow = InfoWin;
            Close.AOWWindow = InfoWin;
            Title.AOWWindow = InfoWin;
            SIcon.AOWWindow = InfoWin;
            MainBtn.AOWWindow = InfoWin;
            CeltFrame.AOWWindow = InfoWin;
            StructLine.AOWWindow = InfoWin;
            StructNameLbl.AOWWindow = NamePnl;
            ProductionPlacePnl.AOWWindow = InfoWin;
            NamePnl.AOWWindow = InfoWin;
            ActiveActionMemo.AOWWindow = ProductionPlacePnl;
            SignPnl.AOWWindow = InfoWin;
            SignText.AOWWindow = SignPnl;
            SignText.VScrollBar = SignTextSB;
            SignLbl.AOWWindow = SignPnl;
            SignTLine.AOWWindow = SignPnl;
            SignTDecoL.AOWWindow = SignPnl;
            SignTDecoR.AOWWindow = SignPnl;
            SignBLine.AOWWindow = SignPnl;
            SignBDecoL.AOWWindow = SignPnl;
            SingBDemoR.AOWWindow = SignPnl;
            SignTextSB.AOWWindow = SignPnl;
            CityPanel.AOWWindow = InfoWin;
            CityRaceLbl.AOWWindow = CityPanel;
            CityBorderPnl.AOWWindow = InfoWin;
            ProdOrb.AOWWindow = CityBorderPnl;
            SingleButPnl.AOWWindow = InfoWin;
            SBmemo.AOWWindow = SingleButPnl;
            SBmemo.VScrollBar = SBSB;
            AltarPnl.AOWWindow = InfoWin;
            Bar.AOWWindow = AltarPnl;
            ChargeLbl.AOWWindow = AltarPnl;
            GemLbl.AOWWindow = AltarInfoPnl;
            ChargePcntLbl.AOWWindow = AltarPnl;
            T2GemI.AOWWindow = AltarInfoPnl;
            T2TurnI.AOWWindow = AltarInfoPnl;
            DayLbl.AOWWindow = AltarInfoPnl;
            LShield.AOWWindow = NotYoursPnl;
            NotYoursPnl.AOWWindow = InfoWin;
            NotYoursMemo.AOWWindow = NotYoursPnl;
            NotYoursMemo.VScrollBar = NotYoursMemoSB;
            Ownerlbl.AOWWindow = NotYoursPnl;
            OwnerRaceLbl.AOWWindow = NotYoursPnl;
            ProdOrbTurnsLbl.AOWWindow = CityBorderPnl;
            orLbl.AOWWindow = AltarInfoPnl;
            AltarInfoPnl.AOWWindow = AltarPnl;
            ProdOrbTime.AOWWindow = CityBorderPnl;
            Center.AOWWindow = InfoWin;
            ConstructionPnl.AOWWindow = InfoWin;
            ConstructionMemo.AOWWindow = ConstructionPnl;
            UnderConstructionLbl.AOWWindow = ConstructionPnl;
            ConstructionDaysLeftLbl.AOWWindow = ConstructionPnl;
            RazePnl.AOWWindow = InfoWin;
            RazeBg.AOWWindow = RazePnl;
            RazeBtn.AOWWindow = RazePnl;
            CivilStatusLbl.AOWWindow = CityPanel;
            RaceRelationIcon.AOWWindow = CityPanel;
            CityIncomeLbl.AOWWindow = InfoPnl;
            GoldIcn.AOWWindow = InfoPnl;
            UgradeI.AOWWindow = InfoPnl;
            CityLevelLbl.AOWWindow = InfoPnl;
            InfoPnl.AOWWindow = CityPanel;
            ProductionPlaceInfoPnl.AOWWindow = ProductionPlacePnl;
            IncomeTypeIcon.AOWWindow = ProductionPlaceInfoPnl;
            IncomeValueLbl.AOWWindow = ProductionPlaceInfoPnl;
            ActiveCityActionMemo.AOWWindow = CityPanel;
            CostToFireLbl.AOWWindow = AltarPnl;
            CurrentActivityLbl.AOWWindow = ProductionPlacePnl;
            IncomeLbl.AOWWindow = ProductionPlacePnl;
            ProductionWarningLbl.AOWWindow = CityPanel;
            CurrentActivityWarningLbl.AOWWindow = ProductionPlacePnl;
            IncomePnl.AOWWindow = InfoWin;
            IncomInfoPnl.AOWWindow = IncomePnl;
            IncomeTypeImage.AOWWindow = IncomInfoPnl;
            IncomeAmountLbl.AOWWindow = IncomInfoPnl;
            IncomeTextLbl.AOWWindow = IncomePnl;
            IncomeMemo.AOWWindow = IncomePnl;
            ItemPnl.AOWWindow = InfoWin;
            ItemsInBagLbl.AOWWindow = ItemPnl;
            ItembagLB.AOWWindow = ItemPnl;
            ItembagLB.VScrollBar = ItembagSB;
            ItembagSB.AOWWindow = ItemPnl;
            ItemBagMemo.AOWWindow = ItemPnl;
            SBSB.AOWWindow = SingleButPnl;
            NotYoursMemoSB.AOWWindow = NotYoursPnl;
            ExplorationSitePnl.AOWWindow = InfoWin;
            ESMemo.AOWWindow = ExplorationSitePnl;
            ESMemo.VScrollBar = ESSB;
            ESSB.AOWWindow = ExplorationSitePnl;
            ESLbl.AOWWindow = ExplorationSitePnl;
            ESStatusLbl.AOWWindow = ExplorationSitePnl;
            Help.AOWWindow = InfoWin;
            PopulationRaceLbl.AOWWindow = NotYoursPnl;
            PopulationLbl.AOWWindow = NotYoursPnl;
            Loaded();
            InfoWin.Loaded();
            Min.Loaded();
            Close.Loaded();
            Title.Loaded();
            SIcon.Loaded();
            MainBtn.Loaded();
            CeltFrame.Loaded();
            StructLine.Loaded();
            StructNameLbl.Loaded();
            ProductionPlacePnl.Loaded();
            NamePnl.Loaded();
            ActiveActionMemo.Loaded();
            SignPnl.Loaded();
            SignText.Loaded();
            SignLbl.Loaded();
            SignTLine.Loaded();
            SignTDecoL.Loaded();
            SignTDecoR.Loaded();
            SignBLine.Loaded();
            SignBDecoL.Loaded();
            SingBDemoR.Loaded();
            SignTextSB.Loaded();
            CityPanel.Loaded();
            CityRaceLbl.Loaded();
            CityBorderPnl.Loaded();
            ProdOrb.Loaded();
            SingleButPnl.Loaded();
            SBmemo.Loaded();
            AltarPnl.Loaded();
            Bar.Loaded();
            ChargeLbl.Loaded();
            GemLbl.Loaded();
            ChargePcntLbl.Loaded();
            T2GemI.Loaded();
            T2TurnI.Loaded();
            DayLbl.Loaded();
            LShield.Loaded();
            NotYoursPnl.Loaded();
            NotYoursMemo.Loaded();
            Ownerlbl.Loaded();
            OwnerRaceLbl.Loaded();
            ProdOrbTurnsLbl.Loaded();
            orLbl.Loaded();
            AltarInfoPnl.Loaded();
            ProdOrbTime.Loaded();
            Center.Loaded();
            ConstructionPnl.Loaded();
            ConstructionMemo.Loaded();
            UnderConstructionLbl.Loaded();
            ConstructionDaysLeftLbl.Loaded();
            RazePnl.Loaded();
            RazeBg.Loaded();
            RazeBtn.Loaded();
            CivilStatusLbl.Loaded();
            RaceRelationIcon.Loaded();
            CityIncomeLbl.Loaded();
            GoldIcn.Loaded();
            UgradeI.Loaded();
            CityLevelLbl.Loaded();
            InfoPnl.Loaded();
            ProductionPlaceInfoPnl.Loaded();
            IncomeTypeIcon.Loaded();
            IncomeValueLbl.Loaded();
            ActiveCityActionMemo.Loaded();
            CostToFireLbl.Loaded();
            CurrentActivityLbl.Loaded();
            IncomeLbl.Loaded();
            ProductionWarningLbl.Loaded();
            CurrentActivityWarningLbl.Loaded();
            IncomePnl.Loaded();
            IncomInfoPnl.Loaded();
            IncomeTypeImage.Loaded();
            IncomeAmountLbl.Loaded();
            IncomeTextLbl.Loaded();
            IncomeMemo.Loaded();
            ItemPnl.Loaded();
            ItemsInBagLbl.Loaded();
            ItembagLB.Loaded();
            ItembagSB.Loaded();
            ItemBagMemo.Loaded();
            SBSB.Loaded();
            NotYoursMemoSB.Loaded();
            ExplorationSitePnl.Loaded();
            ESMemo.Loaded();
            ESSB.Loaded();
            ESLbl.Loaded();
            ESStatusLbl.Loaded();
            Help.Loaded();
            PopulationRaceLbl.Loaded();
            PopulationLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            InfoWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            InfoWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            InfoWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Min.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            MainBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CeltFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            StructNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ActiveActionMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ActiveActionMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SignPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SignText.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SignText.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SignText.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SignLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SignTLine.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SignTDecoL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SignTDecoR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SignBLine.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SignBDecoL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SingBDemoR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SignTextSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SignTextSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SignTextSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SignTextSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CityRaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ProdOrb.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ProdOrb.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ProdWin;
            SBmemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Bar.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Bar.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Bar.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChargeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            GemLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ChargePcntLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2GemI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2GemI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ProdIcons;
            T2TurnI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2TurnI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ProdIcons;
            DayLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            NotYoursMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            NotYoursMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Ownerlbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            OwnerRaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ProdOrbTurnsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            orLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AltarInfoPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AltarInfoPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ProdOrbTime.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ProdOrbTime.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ProdWin;
            Center.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ConstructionMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ConstructionMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnderConstructionLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ConstructionDaysLeftLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            RazeBg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            RazeBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RazeBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            CivilStatusLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            RaceRelationIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            CityIncomeLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CityIncomeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            GoldIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GoldIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            UgradeI.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UgradeI.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            CityLevelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IncomeTypeIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeTypeIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            IncomeValueLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeValueLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ActiveCityActionMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ActiveCityActionMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CostToFireLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            CurrentActivityLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CurrentActivityLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IncomeLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ProductionWarningLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            CurrentActivityWarningLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            IncomeTypeImage.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeTypeImage.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            IncomeAmountLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeAmountLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IncomeTextLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            IncomeTextLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IncomeMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            IncomeMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ItemsInBagLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ItemsInBagLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ItembagLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ItembagLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ItembagLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ItembagLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ItembagSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ItembagSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ItembagSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ItembagSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ItemBagMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ItemBagMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SBSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SBSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NotYoursMemoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NotYoursMemoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NotYoursMemoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NotYoursMemoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ESMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ESSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ESSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ESSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ESSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ESLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ESStatusLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PopulationRaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            PopulationLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
        }

        private void Layout_InfoWinResize_0(TObject p0)
        {
            InfoWinResize();
        }

        private void Layout_MinClick_1(TObject p0)
        {
            MinClick();
        }

        private void Layout_MainBtnClick_2(TObject p0)
        {
            MainBtnClick();
        }

        private void Layout_ProdOrb2Draw_3(Aow.Units.AoWComp.TAoWComponent p0, Aow.Units.GFXE.TSurfaceDesc p1)
        {
            ProdOrb2Draw();
        }

        private void Layout_CenterClick_4(TObject p0)
        {
            CenterClick();
        }

        private void Layout_RazeBtnClick_5(TObject p0)
        {
            RazeBtnClick();
        }

        private void Layout_ItembagLBDrawItem_6(Aow.Units.AoWComp.TAoWComponent p0, in Aow.Rtl.TRect p1, int p2, ref Aow.Units.GFXE.TSurfaceDesc p3)
        {
            ItembagLBDrawItem(default(Aow.Rtl.TObject), p1, default(int), p3);
        }

        private void Layout_ESSBChange_7(TObject p0)
        {
            ESSBChange();
        }

        private void Layout_HelpClick_8(TObject p0)
        {
            HelpClick();
        }
    }
}
