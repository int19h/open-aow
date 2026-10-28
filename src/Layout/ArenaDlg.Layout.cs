// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWArenaDlg
{
    public partial class TArenaDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ArenaDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            SelectBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelectBtn",
                Priority = 100,
                WinLeft = 118,
                WinTop = 290,
                CaptureInput = true,
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
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Train",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 128,
                OnClick = Layout_SelectBtnClick_0
            };
            LayoutSession.EndRead(SelectBtn);
            TitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TitleIcon",
                Priority = 0,
                WinLeft = 20,
                WinTop = 5,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 0,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 456,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes the window."
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
                ILIndexDown = 1,
                ILIndexLit = 2,
                ILIndexUp = 0,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CloseClick_1
            };
            LayoutSession.EndRead(Close);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 0,
                WinLeft = 42,
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
                Text = "Arena",
                TextColor = 2502709
            };
            LayoutSession.EndRead(Title);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 100,
                WinLeft = 246,
                WinTop = 290,
                CaptureInput = true,
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
                    BottomOffset = 11
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
                WinWidth = 128,
                OnClick = Layout_CloseClick_1
            };
            LayoutSession.EndRead(CancelBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 550,
                WinLeft = 74,
                WinTop = 52,
                WinWidth = 492,
                OnDrawBack = Layout_DlgDrawBack_2,
                Masked = true,
                OnHide = Layout_DlgHide_3,
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
                MaskColor = 6268490,
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
                WinHeight = 328,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            UnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitPnl",
                Priority = 1,
                WinLeft = 26,
                WinTop = 195,
                WinWidth = 437,
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
                WinHeight = 100,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitPnl);
            MapViewPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MapViewPnl",
                Priority = 1,
                WinLeft = 20,
                WinTop = 21,
                WinWidth = 452,
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
                WinHeight = 140,
                ILIndexFrame = 113,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MapViewPnl);
            U1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U1",
                OnPreDraw = U1PreDraw,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U1);
            U2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U2",
                Tag = 1,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 55,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U2);
            U3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U3",
                Tag = 2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 110,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U3);
            U4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U4",
                Tag = 3,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 165,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U4);
            U5 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U5",
                Tag = 4,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 220,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U5);
            U6 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U6",
                Tag = 5,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 275,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U6);
            U7 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U7",
                Tag = 6,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 330,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U7);
            U8 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U8",
                Tag = 7,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 386,
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 50,
                Checked = false,
                OnChange = U1Change,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U8);
            DecoL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DecoL",
                Priority = 1000,
                WinLeft = 35,
                WinTop = 290,
                CaptureInput = true,
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
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DecoL);
            DecoR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DecoR",
                Priority = 1000,
                WinLeft = 375,
                WinTop = 290,
                CaptureInput = true,
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
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DecoR);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 26,
                WinTop = 290,
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
                    LeftOffset = 26,
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                WinWidth = 440,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            SelectionInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SelectionInfoPnl",
                Priority = 1,
                WinLeft = 27,
                WinTop = 264,
                WinWidth = 437,
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
                WinHeight = 20,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SelectionInfoPnl);
            SelectionInfoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectionInfoLbl",
                Priority = 100,
                WinLeft = 198,
                WinTop = 10,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
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
                Text = "#####",
                TextColor = 0
            };
            LayoutSession.EndRead(SelectionInfoLbl);
            SelectionLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectionLbl",
                Priority = 100,
                WinLeft = 161,
                WinTop = 172,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
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
                Text = "Select Unit(s) to Train",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SelectionLbl);
            MapView = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "MapView",
                OnDraw = MapViewDraw,
                OnUpdate = Layout_MapViewUpdate_4,
                Priority = 50,
                WinLeft = 23,
                WinTop = 24,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 446,
                WinHeight = 134
            };
            LayoutSession.EndRead(MapView);
            InfoSep = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "InfoSep",
                Priority = 1500,
                WinLeft = 19,
                WinTop = 162,
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
                CurrentImage = 9,
                AnimEnd = 0,
                WinWidth = 453,
                WinHeight = 4,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(InfoSep);
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
            SelectBtn.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            UnitPnl.AOWWindow = Dlg;
            MapViewPnl.AOWWindow = Dlg;
            U1.AOWWindow = UnitPnl;
            U2.AOWWindow = UnitPnl;
            U3.AOWWindow = UnitPnl;
            U4.AOWWindow = UnitPnl;
            U5.AOWWindow = UnitPnl;
            U6.AOWWindow = UnitPnl;
            U7.AOWWindow = UnitPnl;
            U8.AOWWindow = UnitPnl;
            DecoL.AOWWindow = Dlg;
            DecoR.AOWWindow = Dlg;
            AoWFrame1.AOWWindow = Dlg;
            SelectionInfoPnl.AOWWindow = Dlg;
            SelectionInfoLbl.AOWWindow = SelectionInfoPnl;
            SelectionLbl.AOWWindow = Dlg;
            MapView.AOWWindow = Dlg;
            InfoSep.AOWWindow = Dlg;
            Loaded();
            SelectBtn.Loaded();
            TitleIcon.Loaded();
            Close.Loaded();
            Title.Loaded();
            CancelBtn.Loaded();
            Dlg.Loaded();
            UnitPnl.Loaded();
            MapViewPnl.Loaded();
            U1.Loaded();
            U2.Loaded();
            U3.Loaded();
            U4.Loaded();
            U5.Loaded();
            U6.Loaded();
            U7.Loaded();
            U8.Loaded();
            DecoL.Loaded();
            DecoR.Loaded();
            AoWFrame1.Loaded();
            SelectionInfoPnl.Loaded();
            SelectionInfoLbl.Loaded();
            SelectionLbl.Loaded();
            MapView.Loaded();
            InfoSep.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            SelectBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SelectBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            CancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MapViewPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U6.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U7.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            U8.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            DecoL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            DecoR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SelectionInfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SelectionLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            InfoSep.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
        }

        private void Layout_SelectBtnClick_0(TObject p0)
        {
            SelectBtnClick();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }

        private void Layout_DlgDrawBack_2(Aow.Units.AoWComp.TAoWComponent p0, Aow.Units.GFXE.TSurfaceDesc p1)
        {
            DlgDrawBack();
        }

        private void Layout_DlgHide_3(TObject p0)
        {
            DlgHide();
        }

        private void Layout_MapViewUpdate_4(TObject p0)
        {
            MapViewUpdate();
        }
    }
}
