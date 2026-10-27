// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWPowerDlg
{
    public partial class TPowerDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PowerDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
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
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 285,
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
                OnClick = Layout_CancelBtnClick_0
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
                Text = "Power Distribution",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 22,
                WinTop = 282,
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
                    LeftOffset = 22,
                    RightOffset = 21,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                WinWidth = 279,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 100,
                WinLeft = 170,
                WinTop = 282,
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
                OnClick = Layout_CancelBtnClick_0
            };
            LayoutSession.EndRead(CancelBtn);
            SetBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SetBtn",
                Priority = 100,
                WinLeft = 32,
                WinTop = 282,
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
                Text = "Set",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_SetBtnClick_1
            };
            LayoutSession.EndRead(SetBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 159,
                WinTop = 56,
                WinWidth = 322,
                Masked = true,
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
                WinHeight = 320,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            NetMana = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NetMana",
                Priority = 0,
                WinLeft = 40,
                WinTop = 34,
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
                    RightOffset = 8,
                    TopOffset = 34,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "20-18=2",
                TextColor = 16777215
            };
            LayoutSession.EndRead(NetMana);
            NetMamaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NetMamaLbl",
                Priority = 0,
                WinLeft = 22,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Net Mana Income",
                TextColor = 9402970
            };
            LayoutSession.EndRead(NetMamaLbl);
            UpkeePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpkeePnl",
                Priority = 0,
                WinLeft = 22,
                WinTop = 222,
                WinWidth = 138,
                Masked = false,
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
                    LeftOffset = 22,
                    RightOffset = 1,
                    TopOffset = 222,
                    BottomOffset = 39
                },
                WinHeight = 59,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpkeePnl);
            ResearchLeft = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchLeft",
                Priority = 0,
                WinLeft = 33,
                WinTop = 34,
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
                    RightOffset = 8,
                    TopOffset = 34,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ResearchLeft);
            SpellName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellName",
                Priority = 0,
                WinLeft = 27,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Lightning Storm",
                TextColor = 9402970
            };
            LayoutSession.EndRead(SpellName);
            ResearchPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResearchPnl",
                Priority = 0,
                WinLeft = 161,
                WinTop = 222,
                WinWidth = 139,
                Masked = false,
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
                    LeftOffset = 0,
                    RightOffset = 22,
                    TopOffset = 222,
                    BottomOffset = 39
                },
                WinHeight = 59,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResearchPnl);
            SliderPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SliderPnl",
                Priority = 0,
                WinLeft = 22,
                WinTop = 23,
                WinWidth = 278,
                Masked = false,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 50
                },
                WinHeight = 200,
                ILIndexFrame = 121,
                ILIndexTexture = 6,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SliderPnl);
            PowerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PowerLbl",
                Priority = 0,
                WinLeft = 122,
                WinTop = 30,
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
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Power",
                TextColor = 9402970
            };
            LayoutSession.EndRead(PowerLbl);
            Power = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Power",
                Priority = 0,
                WinLeft = 130,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "40",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Power);
            Mana = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Mana",
                Priority = 0,
                WinLeft = 45,
                WinTop = 40,
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
                Text = "20",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Mana);
            Research = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Research",
                Priority = 0,
                WinLeft = 45,
                WinTop = 40,
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
                Text = "20",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Research);
            ManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ManaPnl",
                Priority = 1000,
                WinLeft = 30,
                WinTop = 85,
                WinWidth = 109,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 85,
                    BottomOffset = 0
                },
                WinHeight = 70,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ManaPnl);
            ResPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResPnl",
                Priority = 1000,
                WinLeft = 139,
                WinTop = 85,
                WinWidth = 109,
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
                    LeftOffset = 0,
                    RightOffset = 30,
                    TopOffset = 85,
                    BottomOffset = 0
                },
                WinHeight = 70,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResPnl);
            ManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManaLbl",
                Priority = 0,
                WinLeft = 40,
                WinTop = 55,
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
                Text = "Mana",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ManaLbl);
            ResearchLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 55,
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
                Text = "Research",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResearchLbl);
            PowerSlider = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "PowerSlider",
                Priority = 4000,
                WinLeft = 102,
                WinTop = 97,
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
                    LeftOffset = 102,
                    RightOffset = 0,
                    TopOffset = 97,
                    BottomOffset = 32
                },
                Masked = false,
                Min = 0,
                Max = 100,
                SmallChange = 10,
                LargeChange = 1,
                Position = 50,
                WinHeight = 11,
                WinWidth = 78,
                NumberVisible = 1,
                OnChange = Layout_PowerSliderChange_2,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 25,
                ILIndexTab = 28
            };
            LayoutSession.EndRead(PowerSlider);
            PAMana = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PAMana",
                OnDraw = PAManaDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 89,
                CaptureInput = false,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 11,
                WinWidth = 137,
                WinHeight = 111,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PAMana);
            PARes = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "PARes",
                OnDraw = PAResDraw,
                Priority = 0,
                WinLeft = 141,
                WinTop = 89,
                CaptureInput = false,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 11,
                WinWidth = 137,
                WinHeight = 111,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(PARes);
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
            TitleIcon.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            SetBtn.AOWWindow = Dlg;
            NetMana.AOWWindow = UpkeePnl;
            NetMamaLbl.AOWWindow = UpkeePnl;
            UpkeePnl.AOWWindow = Dlg;
            ResearchLeft.AOWWindow = ResearchPnl;
            SpellName.AOWWindow = ResearchPnl;
            ResearchPnl.AOWWindow = Dlg;
            SliderPnl.AOWWindow = Dlg;
            PowerLbl.AOWWindow = SliderPnl;
            Power.AOWWindow = SliderPnl;
            Mana.AOWWindow = ManaPnl;
            Research.AOWWindow = ResPnl;
            ManaPnl.AOWWindow = SliderPnl;
            ResPnl.AOWWindow = SliderPnl;
            ManaLbl.AOWWindow = ManaPnl;
            ResearchLbl.AOWWindow = ResPnl;
            PowerSlider.AOWWindow = SliderPnl;
            PAMana.AOWWindow = SliderPnl;
            PARes.AOWWindow = SliderPnl;
            Loaded();
            TitleIcon.Loaded();
            Close.Loaded();
            Title.Loaded();
            BCeltL.Loaded();
            CancelBtn.Loaded();
            SetBtn.Loaded();
            Dlg.Loaded();
            NetMana.Loaded();
            NetMamaLbl.Loaded();
            UpkeePnl.Loaded();
            ResearchLeft.Loaded();
            SpellName.Loaded();
            ResearchPnl.Loaded();
            SliderPnl.Loaded();
            PowerLbl.Loaded();
            Power.Loaded();
            Mana.Loaded();
            Research.Loaded();
            ManaPnl.Loaded();
            ResPnl.Loaded();
            ManaLbl.Loaded();
            ResearchLbl.Loaded();
            PowerSlider.Loaded();
            PAMana.Loaded();
            PARes.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SetBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SetBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SetBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NetMana.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            NetMamaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            UpkeePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpkeePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ResearchLeft.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            SpellName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResearchPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ResearchPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SliderPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SliderPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt;
            PowerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Power.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            Mana.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            Research.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResearchLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PowerSlider.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSlider.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSlider.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PAMana.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            PARes.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt;
        }

        private void Layout_CancelBtnClick_0(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_SetBtnClick_1(TObject p0)
        {
            SetBtnClick();
        }

        private void Layout_PowerSliderChange_2(TObject p0)
        {
            PowerSliderChange();
        }
    }
}
