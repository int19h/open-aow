// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCastUnitSpellDlg
{
    public partial class TCastUnitSpellDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CastUnitSpellDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Unit1Gge = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit1Gge",
                Visible = false,
                Priority = 1000,
                WinLeft = 10,
                WinTop = 62,
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
                WinWidth = 30,
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
                Visible = false,
                Priority = 1000,
                WinLeft = 65,
                WinTop = 62,
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
                WinWidth = 30,
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
                Visible = false,
                Priority = 1000,
                WinLeft = 120,
                WinTop = 62,
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
                WinWidth = 30,
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
                Visible = false,
                Priority = 1000,
                WinLeft = 175,
                WinTop = 62,
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
                WinWidth = 30,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit4Gge);
            Unit5Gg = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit5Gg",
                Visible = false,
                Priority = 1000,
                WinLeft = 230,
                WinTop = 62,
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
                WinWidth = 30,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit5Gg);
            Unit6Gg = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit6Gg",
                Visible = false,
                Priority = 1000,
                WinLeft = 285,
                WinTop = 62,
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
                WinWidth = 30,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit6Gg);
            Unit7Gg = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit7Gg",
                Visible = false,
                Priority = 1000,
                WinLeft = 340,
                WinTop = 62,
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
                WinWidth = 30,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit7Gg);
            Unit8Gg = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "Unit8Gg",
                Visible = false,
                Priority = 1000,
                WinLeft = 395,
                WinTop = 62,
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
                WinWidth = 30,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(Unit8Gg);
            CastBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CastBut",
                Priority = 100,
                WinLeft = 118,
                WinTop = 198,
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
                Text = "Cast",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 128,
                OnClick = Layout_CastButClick_0
            };
            LayoutSession.EndRead(CastBut);
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
                CurrentImage = 3,
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
                Text = "Cast Unit Spell",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            CancelBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBut",
                Priority = 100,
                WinLeft = 246,
                WinTop = 198,
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
            LayoutSession.EndRead(CancelBut);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnUpdate = Layout_DlgUpdate_2,
                Visible = false,
                Priority = 520,
                WinLeft = 74,
                WinTop = 98,
                WinWidth = 492,
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
                WinHeight = 236,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            SelectLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectLbl",
                Priority = 100,
                WinLeft = 196,
                WinTop = 90,
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
                Text = "Select Unit(s)",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SelectLbl);
            UnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitPnl",
                Priority = 1,
                WinLeft = 28,
                WinTop = 115,
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
                WinHeight = 60,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitPnl);
            TopPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TopPnl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 30,
                WinWidth = 432,
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
                WinHeight = 59,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TopPnl);
            SpellUpkeepLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellUpkeepLbl",
                Priority = 100,
                WinLeft = 90,
                WinTop = 70,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 0
            };
            LayoutSession.EndRead(SpellUpkeepLbl);
            SpellManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellManaLbl",
                Priority = 100,
                WinLeft = 90,
                WinTop = 55,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 0
            };
            LayoutSession.EndRead(SpellManaLbl);
            CastingManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingManaLbl",
                Priority = 100,
                WinLeft = 338,
                WinTop = 70,
                CaptureInput = true,
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
                    RightOffset = 90,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 0
            };
            LayoutSession.EndRead(CastingManaLbl);
            SpellNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellNameLbl",
                Priority = 100,
                WinLeft = 90,
                WinTop = 30,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SpellNameLbl);
            CasterNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CasterNameLbl",
                Priority = 100,
                WinLeft = 282,
                WinTop = 30,
                CaptureInput = true,
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
                    RightOffset = 90,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "############",
                TextColor = 16777215
            };
            LayoutSession.EndRead(CasterNameLbl);
            CastingPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CastingPointsLbl",
                Priority = 100,
                WinLeft = 322,
                WinTop = 55,
                CaptureInput = true,
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
                    RightOffset = 90,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 0
            };
            LayoutSession.EndRead(CastingPointsLbl);
            U1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U1",
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U1);
            U2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U2",
                Tag = 1,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U2);
            U3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U3",
                Tag = 2,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U3);
            U4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U4",
                Tag = 3,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U4);
            U5 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U5",
                Tag = 4,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U5);
            U6 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U6",
                Tag = 5,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U6);
            U7 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U7",
                Tag = 6,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U7);
            U8 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "U8",
                Tag = 7,
                OnPreDraw = U1PreDraw,
                OnDraw = Layout_U1Draw_4,
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
                OnChange = Layout_U1Change_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(U8);
            DecoL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DecoL",
                Priority = 1000,
                WinLeft = 35,
                WinTop = 198,
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
                WinTop = 198,
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
            SpellFr = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SpellFr",
                OnPreDraw = SpellFrPreDraw,
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
                CurrentImage = 73,
                AnimEnd = 0,
                WinWidth = 50,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SpellFr);
            FaceFrame = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFrame",
                OnPreDraw = FaceFramePreDraw,
                Priority = 0,
                WinLeft = 382,
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
                CurrentImage = 73,
                AnimEnd = 0,
                WinWidth = 50,
                WinHeight = 59,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFrame);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 26,
                WinTop = 198,
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
                WinLeft = 28,
                WinTop = 180,
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
                WinLeft = 182,
                WinTop = 0,
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
                Text = "#########",
                TextColor = 0
            };
            LayoutSession.EndRead(SelectionInfoLbl);
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
            Unit1Gge.AOWWindow = UnitPnl;
            Unit2Gge.AOWWindow = UnitPnl;
            Unit3Gge.AOWWindow = UnitPnl;
            Unit4Gge.AOWWindow = UnitPnl;
            Unit5Gg.AOWWindow = UnitPnl;
            Unit6Gg.AOWWindow = UnitPnl;
            Unit7Gg.AOWWindow = UnitPnl;
            Unit8Gg.AOWWindow = UnitPnl;
            CastBut.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            CancelBut.AOWWindow = Dlg;
            SelectLbl.AOWWindow = Dlg;
            UnitPnl.AOWWindow = Dlg;
            TopPnl.AOWWindow = Dlg;
            SpellUpkeepLbl.AOWWindow = Dlg;
            SpellManaLbl.AOWWindow = Dlg;
            CastingManaLbl.AOWWindow = Dlg;
            SpellNameLbl.AOWWindow = Dlg;
            CasterNameLbl.AOWWindow = Dlg;
            CastingPointsLbl.AOWWindow = Dlg;
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
            SpellFr.AOWWindow = TopPnl;
            FaceFrame.AOWWindow = TopPnl;
            AoWFrame1.AOWWindow = Dlg;
            SelectionInfoPnl.AOWWindow = Dlg;
            SelectionInfoLbl.AOWWindow = SelectionInfoPnl;
            Loaded();
            Unit1Gge.Loaded();
            Unit2Gge.Loaded();
            Unit3Gge.Loaded();
            Unit4Gge.Loaded();
            Unit5Gg.Loaded();
            Unit6Gg.Loaded();
            Unit7Gg.Loaded();
            Unit8Gg.Loaded();
            CastBut.Loaded();
            TitleIcon.Loaded();
            Close.Loaded();
            Title.Loaded();
            CancelBut.Loaded();
            Dlg.Loaded();
            SelectLbl.Loaded();
            UnitPnl.Loaded();
            TopPnl.Loaded();
            SpellUpkeepLbl.Loaded();
            SpellManaLbl.Loaded();
            CastingManaLbl.Loaded();
            SpellNameLbl.Loaded();
            CasterNameLbl.Loaded();
            CastingPointsLbl.Loaded();
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
            SpellFr.Loaded();
            FaceFrame.Loaded();
            AoWFrame1.Loaded();
            SelectionInfoPnl.Loaded();
            SelectionInfoLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Unit1Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit1Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4Gge.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4Gge.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5Gg.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5Gg.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6Gg.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6Gg.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7Gg.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7Gg.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8Gg.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8Gg.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CastBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBut.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SelectLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SpellUpkeepLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SpellManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastingManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SpellNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CasterNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CastingPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
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
            SpellFr.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            SelectionInfoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
        }

        private void Layout_CastButClick_0(TObject p0)
        {
            CastButClick();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }

        private void Layout_DlgUpdate_2(TObject p0)
        {
            DlgUpdate();
        }

        private void Layout_DlgHide_3(TObject p0)
        {
            DlgHide();
        }

        private void Layout_U1Draw_4(Aow.Units.AoWComp.TAoWComponent p0, Aow.Units.GFXE.TSurfaceDesc p1)
        {
            U1Draw();
        }

        private void Layout_U1Change_5(TObject p0)
        {
            U1Change((Aow.Units.AoWCheckBox.TAOWCheckBox)p0);
        }
    }
}
