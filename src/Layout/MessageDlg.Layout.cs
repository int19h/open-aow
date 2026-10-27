// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWMessageDlg
{
    public partial class TMessageDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MessageDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 600,
                WinLeft = 128,
                WinTop = 119,
                WinWidth = 385,
                Masked = true,
                OnHide = Layout_DlgHide_0,
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
                FreeSurfaceOnHide = false,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 195,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            TitleLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TitleLbl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 7,
                CaptureInput = false,
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
                Text = "########",
                TextColor = 0
            };
            LayoutSession.EndRead(TitleLbl);
            YesNoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "YesNoPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 0,
                WinTop = 50,
                WinWidth = 357,
                Masked = true,
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 40,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(YesNoPnl);
            YesBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "YesBtn",
                Priority = 1000,
                WinLeft = 57,
                WinTop = 108,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Yes",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_YesBtnClick_1
            };
            LayoutSession.EndRead(YesBtn);
            NoBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NoBtn",
                Priority = 1000,
                WinLeft = 195,
                WinTop = 108,
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
                    RightOffset = 57,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "No",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_NoBtnClick_2
            };
            LayoutSession.EndRead(NoBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 158,
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
                WinWidth = 343,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            TCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 22,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 343,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TCeltL);
            TCeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 22,
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
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 329,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLineL);
            AOWImage1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage1",
                Priority = 100,
                WinLeft = 28,
                WinTop = 158,
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
                    RightOffset = 28,
                    TopOffset = 22,
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
            LayoutSession.EndRead(AOWImage1);
            AOWImage2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage2",
                Priority = 100,
                WinLeft = 330,
                WinTop = 158,
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
                    RightOffset = 28,
                    TopOffset = 22,
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
            LayoutSession.EndRead(AOWImage2);
            OKBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn",
                Priority = 1000,
                WinLeft = 127,
                WinTop = 3,
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
                    RightOffset = 57,
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
                WinWidth = 130,
                OnClick = Layout_OKBtnClick_3
            };
            LayoutSession.EndRead(OKBtn);
            OKPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "OKPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 0,
                WinTop = 155,
                WinWidth = 385,
                Masked = true,
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 40,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(OKPnl);
            OKCancelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "OKCancelPnl",
                Visible = false,
                Priority = 100,
                WinLeft = 0,
                WinTop = 50,
                WinWidth = 357,
                Masked = true,
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 40,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(OKCancelPnl);
            OKBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn2",
                Priority = 1000,
                WinLeft = 57,
                WinTop = 108,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Yes",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_OKBtn2Click_4
            };
            LayoutSession.EndRead(OKBtn2);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 1000,
                WinLeft = 195,
                WinTop = 108,
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
                    RightOffset = 57,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "No",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_CancelBtnClick_5
            };
            LayoutSession.EndRead(CancelBtn);
            MessageMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MessageMemo",
                Priority = 0,
                WinLeft = 35,
                WinTop = 0,
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
                    LeftOffset = 35,
                    RightOffset = 35,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 315,
                WinHeight = 100,
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 0,
                CursorChar = (byte)0,
                BlinkDelay = (byte)(0),
                Editable = true,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(MessageMemo);
            MessageMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "MessageMemoSB",
                Visible = false,
                Priority = 0,
                WinLeft = 333,
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
                    TopOffset = 50,
                    BottomOffset = 27
                },
                Masked = false,
                Min = 0,
                Max = 2,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 100,
                WinWidth = 17,
                NumberVisible = 2,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(MessageMemoSB);
            YesNoCancelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "YesNoCancelPnl",
                Priority = 100,
                WinLeft = 0,
                WinTop = 155,
                WinWidth = 385,
                Masked = true,
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 40,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(YesNoCancelPnl);
            YesBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "YesBtn2",
                Priority = 1000,
                WinLeft = 30,
                WinTop = 3,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Yes",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_YesBtnClick_1
            };
            LayoutSession.EndRead(YesBtn2);
            NoBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NoBtn2",
                Priority = 1000,
                WinLeft = 140,
                WinTop = 3,
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
                    LeftOffset = 140,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "No",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 105,
                OnClick = Layout_NoBtnClick_2
            };
            LayoutSession.EndRead(NoBtn2);
            CancelBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn2",
                Priority = 1000,
                WinLeft = 250,
                WinTop = 3,
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
                    RightOffset = 30,
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
                WinWidth = 105,
                OnClick = Layout_CancelBtnClick_5
            };
            LayoutSession.EndRead(CancelBtn2);
            MemoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MemoPnl",
                Visible = false,
                Priority = 90,
                WinLeft = 0,
                WinTop = 55,
                WinWidth = 385,
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
                    TopOffset = 55,
                    BottomOffset = 40
                },
                WinHeight = 100,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MemoPnl);
            HeaderMemoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "HeaderMemoPnl",
                Priority = 90,
                WinLeft = 0,
                WinTop = 55,
                WinWidth = 385,
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
                    TopOffset = 55,
                    BottomOffset = 40
                },
                WinHeight = 100,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(HeaderMemoPnl);
            HeaderMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "HeaderMemo",
                Priority = 0,
                WinLeft = 35,
                WinTop = 35,
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
                    LeftOffset = 35,
                    RightOffset = 35,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 315,
                WinHeight = 60,
                BorderWidth = 0,
                BorderHeight = 0,
                TextColor = 16777215,
                CursorChar = (byte)0,
                BlinkDelay = (byte)(0),
                Editable = true,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(HeaderMemo);
            HeaderMemoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "HeaderMemoSB",
                Visible = false,
                Priority = 0,
                WinLeft = 333,
                WinTop = 35,
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
                    TopOffset = 50,
                    BottomOffset = 27
                },
                Masked = false,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 2,
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
            LayoutSession.EndRead(HeaderMemoSB);
            HeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeaderLbl",
                Priority = 0,
                WinLeft = 192,
                WinTop = 5,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(HeaderLbl);
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
            TitleLbl.AOWWindow = Dlg;
            YesNoPnl.AOWWindow = Dlg;
            YesBtn.AOWWindow = YesNoPnl;
            NoBtn.AOWWindow = YesNoPnl;
            BCeltL.AOWWindow = Dlg;
            TCeltL.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            AOWImage1.AOWWindow = Dlg;
            AOWImage2.AOWWindow = Dlg;
            OKBtn.AOWWindow = OKPnl;
            OKPnl.AOWWindow = Dlg;
            OKCancelPnl.AOWWindow = Dlg;
            OKBtn2.AOWWindow = OKCancelPnl;
            CancelBtn.AOWWindow = OKCancelPnl;
            MessageMemo.AOWWindow = MemoPnl;
            MessageMemo.VScrollBar = MessageMemoSB;
            MessageMemoSB.AOWWindow = MemoPnl;
            YesNoCancelPnl.AOWWindow = Dlg;
            YesBtn2.AOWWindow = YesNoCancelPnl;
            NoBtn2.AOWWindow = YesNoCancelPnl;
            CancelBtn2.AOWWindow = YesNoCancelPnl;
            MemoPnl.AOWWindow = Dlg;
            HeaderMemoPnl.AOWWindow = Dlg;
            HeaderMemo.AOWWindow = HeaderMemoPnl;
            HeaderMemo.VScrollBar = HeaderMemoSB;
            HeaderMemoSB.AOWWindow = HeaderMemoPnl;
            HeaderLbl.AOWWindow = HeaderMemoPnl;
            Loaded();
            Dlg.Loaded();
            TitleLbl.Loaded();
            YesNoPnl.Loaded();
            YesBtn.Loaded();
            NoBtn.Loaded();
            BCeltL.Loaded();
            TCeltL.Loaded();
            TCeltLineL.Loaded();
            AOWImage1.Loaded();
            AOWImage2.Loaded();
            OKBtn.Loaded();
            OKPnl.Loaded();
            OKCancelPnl.Loaded();
            OKBtn2.Loaded();
            CancelBtn.Loaded();
            MessageMemo.Loaded();
            MessageMemoSB.Loaded();
            YesNoCancelPnl.Loaded();
            YesBtn2.Loaded();
            NoBtn2.Loaded();
            CancelBtn2.Loaded();
            MemoPnl.Loaded();
            HeaderMemoPnl.Loaded();
            HeaderMemo.Loaded();
            HeaderMemoSB.Loaded();
            HeaderLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TitleLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            YesBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            YesBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            NoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NoBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWImage2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            OKBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            OKBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MessageMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            MessageMemoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageMemoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            YesBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            YesBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            NoBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NoBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CancelBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            HeaderMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            HeaderMemoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeaderMemoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeaderMemoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeaderMemoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_YesBtnClick_1(TObject p0)
        {
            YesBtnClick();
        }

        private void Layout_NoBtnClick_2(TObject p0)
        {
            NoBtnClick();
        }

        private void Layout_OKBtnClick_3(TObject p0)
        {
            OKBtnClick();
        }

        private void Layout_OKBtn2Click_4(TObject p0)
        {
            OKBtn2Click();
        }

        private void Layout_CancelBtnClick_5(TObject p0)
        {
            CancelBtnClick();
        }
    }
}
