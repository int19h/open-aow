// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWPartyJoin
{
    public partial class TPartyJoinDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PartyJoinDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_PartyJoinDlgCreate_0;
            LayoutOnDestroy = Layout_PartyJoinDlgDestroy_1;
            UnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitPnl",
                Priority = 1,
                WinLeft = 35,
                WinTop = 171,
                WinWidth = 370,
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
                    LeftOffset = 35,
                    RightOffset = 35,
                    TopOffset = 171,
                    BottomOffset = 38
                },
                WinHeight = 61,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitPnl);
            U1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U1",
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
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
                    RightOffPercent = 88,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U1);
            U2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U2",
                Tag = 1,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 46,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 12,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U2);
            U4 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U4",
                Tag = 3,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 138,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 37,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U4);
            U5 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U5",
                Tag = 4,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 185,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 38,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U5);
            U3 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U3",
                Tag = 2,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 92,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U3);
            U6 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U6",
                Tag = 5,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 231,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 62,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U6);
            U7 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U7",
                Tag = 6,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 277,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 13,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U7);
            U8 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U8",
                Tag = 7,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 323,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 87,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 54,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U8);
            MiddleBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MiddleBtn",
                Priority = 100,
                WinLeft = 160,
                WinTop = 232,
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
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Attack",
                TextColor = 328543,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_MiddleBtnClick_3
            };
            LayoutSession.EndRead(MiddleBtn);
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
                CurrentImage = 4,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
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
                Text = "Party Join",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 232,
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
                    BottomOffset = 11
                },
                Masked = false,
                WinWidth = 398,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            RightBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RightBtn",
                Priority = 100,
                WinLeft = 288,
                WinTop = 232,
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
                    RightOffset = 32,
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
                WinWidth = 120,
                OnClick = Layout_RightBtnClick_4
            };
            LayoutSession.EndRead(RightBtn);
            LeftBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LeftBtn",
                Priority = 100,
                WinLeft = 32,
                WinTop = 232,
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
                    LeftOffset = 32,
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
                Text = "Accept",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_LeftBtnClick_5
            };
            LayoutSession.EndRead(LeftBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 100,
                WinTop = 81,
                WinWidth = 440,
                Masked = true,
                OnHide = Layout_DlgHide_6,
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
                WinHeight = 270,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            UnitLine1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "UnitLine1",
                Priority = 100,
                WinLeft = 134,
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
                    LeftOffset = 134,
                    RightOffset = 21,
                    TopOffset = 23,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 285,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(UnitLine1);
            UnitLine2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "UnitLine2",
                Priority = 100,
                WinLeft = 134,
                WinTop = 139,
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
                    LeftOffset = 134,
                    RightOffset = 21,
                    TopOffset = 139,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 285,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(UnitLine2);
            JoinMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "JoinMemo",
                Priority = 0,
                WinLeft = 141,
                WinTop = 53,
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
                    LeftOffset = 141,
                    RightOffset = 20,
                    TopOffset = 53,
                    BottomOffset = 130
                },
                Masked = true,
                WinWidth = 279,
                WinHeight = 87,
                BorderWidth = 0,
                BorderHeight = 8,
                TextColor = 13827061,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = true,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 27,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(JoinMemo);
            Header = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Header",
                Priority = 200,
                WinLeft = 140,
                WinTop = 31,
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
                    LeftOffset = 140,
                    RightOffset = 0,
                    TopOffset = 31,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The leader of the party steps forward:",
                TextColor = 13165253
            };
            LayoutSession.EndRead(Header);
            FaceFrame = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFrame",
                OnPreDraw = FaceFramePreDraw,
                Priority = 100,
                WinLeft = 22,
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
                    LeftOffset = 22,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 118,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFrame);
            DetailsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DetailsLbl",
                Priority = 100,
                WinLeft = 140,
                WinTop = 147,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awNone,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 60,
                    TopOffset = 147,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Click on the unit(s) for details",
                TextColor = 13165253
            };
            LayoutSession.EndRead(DetailsLbl);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 403,
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
                    RightOffset = 22,
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
                OnClick = Layout_CloseClick_7
            };
            LayoutSession.EndRead(Close);
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
            UnitPnl.AOWWindow = Dlg;
            U1.AOWWindow = UnitPnl;
            U2.AOWWindow = UnitPnl;
            U4.AOWWindow = UnitPnl;
            U5.AOWWindow = UnitPnl;
            U3.AOWWindow = UnitPnl;
            U6.AOWWindow = UnitPnl;
            U7.AOWWindow = UnitPnl;
            U8.AOWWindow = UnitPnl;
            MiddleBtn.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            RightBtn.AOWWindow = Dlg;
            LeftBtn.AOWWindow = Dlg;
            UnitLine1.AOWWindow = Dlg;
            UnitLine2.AOWWindow = Dlg;
            JoinMemo.AOWWindow = Dlg;
            Header.AOWWindow = Dlg;
            FaceFrame.AOWWindow = Dlg;
            DetailsLbl.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Loaded();
            UnitPnl.Loaded();
            U1.Loaded();
            U2.Loaded();
            U4.Loaded();
            U5.Loaded();
            U3.Loaded();
            U6.Loaded();
            U7.Loaded();
            U8.Loaded();
            MiddleBtn.Loaded();
            TitleIcon.Loaded();
            Title.Loaded();
            BCeltL.Loaded();
            RightBtn.Loaded();
            LeftBtn.Loaded();
            Dlg.Loaded();
            UnitLine1.Loaded();
            UnitLine2.Loaded();
            JoinMemo.Loaded();
            Header.Loaded();
            FaceFrame.Loaded();
            DetailsLbl.Loaded();
            Close.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            U1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U4.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U5.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U3.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U6.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U7.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U8.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MiddleBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MiddleBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            RightBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RightBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RightBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LeftBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LeftBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LeftBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitLine1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            UnitLine2.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            JoinMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Header.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Header.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            DetailsLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DetailsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_PartyJoinDlgCreate_0(TObject p0)
        {
            PartyJoinDlgCreate();
        }

        private void Layout_PartyJoinDlgDestroy_1(TObject p0)
        {
            PartyJoinDlgDestroy();
        }

        private void Layout_U1MouseDown_2(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            U1MouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_MiddleBtnClick_3(TObject p0)
        {
            MiddleBtnClick();
        }

        private void Layout_RightBtnClick_4(TObject p0)
        {
            RightBtnClick();
        }

        private void Layout_LeftBtnClick_5(TObject p0)
        {
            LeftBtnClick();
        }

        private void Layout_DlgHide_6(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CloseClick_7(TObject p0)
        {
            CloseClick();
        }
    }
}
