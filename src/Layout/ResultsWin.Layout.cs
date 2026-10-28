// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWResultsWin
{
    public partial class TResultsWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ResultsWin";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_ResultsWinCreate_0;
            LayoutOnDestroy = Layout_ResultsWinDestroy_1;
            P1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "P1CB",
                OnPreDraw = P1CBPreDraw,
                Priority = 800,
                WinLeft = 14,
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 14,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 65,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 116,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 167,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 218,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 269,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 320,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 371,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 422,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 473,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 524,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
                WinTop = 375,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows this player's statistics."
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
                    LeftOffset = 575,
                    RightOffset = 0,
                    TopOffset = 330,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
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
            StrLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StrLbl",
                Priority = 100,
                WinLeft = 130,
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
                    LeftOffset = 130,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Side Dominance",
                TextColor = 0
            };
            LayoutSession.EndRead(StrLbl);
            TurnLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnLbl",
                Priority = 100,
                WinLeft = 465,
                WinTop = 175,
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
                    RightOffset = 150,
                    TopOffset = 175,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns",
                TextColor = 0
            };
            LayoutSession.EndRead(TurnLbl);
            ResNames = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ResNames",
                Visible = false,
                Priority = 100,
                WinLeft = 220,
                WinTop = 227,
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
                    LeftOffset = 220,
                    RightOffset = 270,
                    TopOffset = 227,
                    BottomOffset = 120
                },
                Masked = true,
                WinWidth = 150,
                WinHeight = 133,
                BorderWidth = 20,
                BorderHeight = 4,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 19,
                ItemHeight = 17,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ResNames);
            ResValues = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ResValues",
                Visible = false,
                Priority = 100,
                WinLeft = 370,
                WinTop = 227,
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
                    LeftOffset = 370,
                    RightOffset = 220,
                    TopOffset = 227,
                    BottomOffset = 120
                },
                Masked = true,
                WinWidth = 50,
                WinHeight = 133,
                BorderWidth = 3,
                BorderHeight = 4,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 19,
                ItemHeight = 17,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ResValues);
            SideLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SideLbl",
                Priority = 100,
                WinLeft = 260,
                WinTop = 210,
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
                    TopOffset = 210,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 0
            };
            LayoutSession.EndRead(SideLbl);
            Axis = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Axis",
                OnDraw = AxisDraw,
                Priority = 200,
                WinLeft = 149,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 48,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 0,
                WinWidth = 343,
                WinHeight = 124,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Axis);
            ExitBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ExitBtn",
                Visible = false,
                Priority = 100,
                WinLeft = 508,
                WinTop = 449,
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
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 5,
                ILIndexLit = 8,
                ILIndexUp = 2,
                Text = "Exit",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 120,
                OnClick = Layout_ExitBtnClick_2
            };
            LayoutSession.EndRead(ExitBtn);
            FaceGrid = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceGrid",
                Priority = 40,
                WinLeft = 10,
                WinTop = 374,
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
                    TopOffset = 48,
                    BottomOffset = 41
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 0,
                WinWidth = 620,
                WinHeight = 65,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceGrid);
            LoadBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LoadBtn",
                Visible = false,
                Priority = 100,
                WinLeft = 11,
                WinTop = 449,
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
                    LeftOffset = 11,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 5,
                ILIndexLit = 8,
                ILIndexUp = 2,
                Text = "Replay",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 120
            };
            LayoutSession.EndRead(LoadBtn);
            ContinueBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ContinueBtn",
                Priority = 100,
                WinLeft = 261,
                WinTop = 449,
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
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 5,
                ILIndexLit = 8,
                ILIndexUp = 2,
                Text = "Continue",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 119,
                OnClick = Layout_ContinueBtnClick_3
            };
            LayoutSession.EndRead(ContinueBtn);
            ResultsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResultsPnl",
                OnUpdate = Layout_ResultsPnlUpdate_4,
                Visible = false,
                Priority = 5,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                OnShow = Layout_ResultsPnlShow_5,
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
                ILIndexTexture = 12,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(ResultsPnl);
            StrengthCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "StrengthCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 5,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's overall strength."
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = true,
                OnChange = Layout_AgendaChange_6,
                RadioIndex = 1
            };
            LayoutSession.EndRead(StrengthCB);
            AgendaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AgendaPnl",
                Priority = 100,
                WinLeft = 55,
                WinTop = 70,
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
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                WinHeight = 100,
                ILIndexFrame = 0,
                ILIndexTexture = 38,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AgendaPnl);
            StrengthLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StrengthLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 7,
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
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Winning",
                TextColor = 0
            };
            LayoutSession.EndRead(StrengthLbl);
            GoldCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "GoldCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 35,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's gold."
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
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_AgendaChange_6,
                RadioIndex = 1
            };
            LayoutSession.EndRead(GoldCB);
            GoldLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GoldLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 37,
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
                    TopOffset = 37,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Gold",
                TextColor = 0
            };
            LayoutSession.EndRead(GoldLbl);
            ArmiesCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "ArmiesCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 20,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's army strength."
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
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_AgendaChange_6,
                RadioIndex = 1
            };
            LayoutSession.EndRead(ArmiesCB);
            ArmiesLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ArmiesLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 22,
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
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Armies",
                TextColor = 0
            };
            LayoutSession.EndRead(ArmiesLbl);
            HeroesCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "HeroesCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 65,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's number of heroes."
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
                    TopOffset = 65,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_AgendaChange_6,
                RadioIndex = 1
            };
            LayoutSession.EndRead(HeroesCB);
            HeroesLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeroesLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 67,
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
                    TopOffset = 67,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Heroes",
                TextColor = 0
            };
            LayoutSession.EndRead(HeroesLbl);
            StructuresCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "StructuresCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 80,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's number of structures."
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
                    TopOffset = 80,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_AgendaChange_6,
                RadioIndex = 1
            };
            LayoutSession.EndRead(StructuresCB);
            StructuresLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StructuresLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 82,
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
                    TopOffset = 82,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Structures",
                TextColor = 0
            };
            LayoutSession.EndRead(StructuresLbl);
            KeyMax = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "KeyMax",
                Priority = 100,
                WinLeft = 115,
                WinTop = 45,
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
                    RightOffset = 500,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns",
                TextColor = 0
            };
            LayoutSession.EndRead(KeyMax);
            KeyMin = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "KeyMin",
                Priority = 100,
                WinLeft = 135,
                WinTop = 160,
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
                    RightOffset = 500,
                    TopOffset = 160,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "0",
                TextColor = 0
            };
            LayoutSession.EndRead(KeyMin);
            ManaCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "ManaCB",
                Priority = 100,
                WinLeft = 0,
                WinTop = 50,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Shows a graph of player's mana."
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(ManaCB);
            ManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManaLbl",
                Priority = 100,
                WinLeft = 15,
                WinTop = 52,
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
                    TopOffset = 52,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Mana",
                TextColor = 0
            };
            LayoutSession.EndRead(ManaLbl);
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
            P1CB.AOWWindow = ResultsPnl;
            P2CB.AOWWindow = ResultsPnl;
            P3CB.AOWWindow = ResultsPnl;
            P4CB.AOWWindow = ResultsPnl;
            P5CB.AOWWindow = ResultsPnl;
            P6CB.AOWWindow = ResultsPnl;
            P7CB.AOWWindow = ResultsPnl;
            P8CB.AOWWindow = ResultsPnl;
            P9CB.AOWWindow = ResultsPnl;
            P10CB.AOWWindow = ResultsPnl;
            P11CB.AOWWindow = ResultsPnl;
            P12CB.AOWWindow = ResultsPnl;
            StrLbl.AOWWindow = ResultsPnl;
            TurnLbl.AOWWindow = ResultsPnl;
            ResNames.AOWWindow = ResultsPnl;
            ResValues.AOWWindow = ResultsPnl;
            SideLbl.AOWWindow = ResultsPnl;
            Axis.AOWWindow = ResultsPnl;
            ExitBtn.AOWWindow = ResultsPnl;
            FaceGrid.AOWWindow = ResultsPnl;
            LoadBtn.AOWWindow = ResultsPnl;
            ContinueBtn.AOWWindow = ResultsPnl;
            StrengthCB.AOWWindow = AgendaPnl;
            AgendaPnl.AOWWindow = ResultsPnl;
            StrengthLbl.AOWWindow = AgendaPnl;
            GoldCB.AOWWindow = AgendaPnl;
            GoldLbl.AOWWindow = AgendaPnl;
            ArmiesCB.AOWWindow = AgendaPnl;
            ArmiesLbl.AOWWindow = AgendaPnl;
            HeroesCB.AOWWindow = AgendaPnl;
            HeroesLbl.AOWWindow = AgendaPnl;
            StructuresCB.AOWWindow = AgendaPnl;
            StructuresLbl.AOWWindow = AgendaPnl;
            KeyMax.AOWWindow = ResultsPnl;
            KeyMin.AOWWindow = ResultsPnl;
            ManaCB.AOWWindow = AgendaPnl;
            ManaLbl.AOWWindow = AgendaPnl;
            Loaded();
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
            StrLbl.Loaded();
            TurnLbl.Loaded();
            ResNames.Loaded();
            ResValues.Loaded();
            SideLbl.Loaded();
            Axis.Loaded();
            ExitBtn.Loaded();
            FaceGrid.Loaded();
            LoadBtn.Loaded();
            ContinueBtn.Loaded();
            ResultsPnl.Loaded();
            StrengthCB.Loaded();
            AgendaPnl.Loaded();
            StrengthLbl.Loaded();
            GoldCB.Loaded();
            GoldLbl.Loaded();
            ArmiesCB.Loaded();
            ArmiesLbl.Loaded();
            HeroesCB.Loaded();
            HeroesLbl.Loaded();
            StructuresCB.Loaded();
            StructuresLbl.Loaded();
            KeyMax.Loaded();
            KeyMin.Loaded();
            ManaCB.Loaded();
            ManaLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
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
            StrLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TurnLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResNames.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResValues.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SideLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Axis.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            ExitBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ExitBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            FaceGrid.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            LoadBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LoadBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            ContinueBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ContinueBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            ResultsPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            ResultsPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.EndScene;
            StrengthCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StrengthCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            StrengthCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AgendaPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            StrengthLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StrengthLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            GoldCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GoldCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            GoldCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            GoldLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GoldLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ArmiesCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ArmiesCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ArmiesCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ArmiesLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ArmiesLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HeroesCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            HeroesCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            HeroesCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            HeroesLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            HeroesLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            StructuresCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StructuresCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            StructuresCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            StructuresLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            StructuresLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            KeyMax.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            KeyMin.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ManaCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ManaCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ManaLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_ResultsWinCreate_0(TObject p0)
        {
            ResultsWinCreate();
        }

        private void Layout_ResultsWinDestroy_1(TObject p0)
        {
            ResultsWinDestroy();
        }

        private void Layout_ExitBtnClick_2(TObject p0)
        {
            ExitBtnClick();
        }

        private void Layout_ContinueBtnClick_3(TObject p0)
        {
            ContinueBtnClick();
        }

        private void Layout_ResultsPnlUpdate_4(TObject p0)
        {
            ResultsPnlUpdate();
        }

        private void Layout_ResultsPnlShow_5(TObject p0)
        {
            ResultsPnlShow();
        }

        private void Layout_AgendaChange_6(TObject p0)
        {
            AgendaChange();
        }
    }
}
