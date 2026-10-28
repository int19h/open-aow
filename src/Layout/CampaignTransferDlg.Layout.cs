// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCampaignTransferDlg
{
    public partial class TCampaignTransferDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CampaignTransferDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LHero1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LHero1",
                OnDraw = LHeroesDraw,
                OnUpdate = LHeroesUpdate,
                Priority = 1500,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LHero1);
            LHero2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LHero2",
                Tag = 1,
                OnDraw = LHeroesDraw,
                OnUpdate = LHeroesUpdate,
                Priority = 1600,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LHero2);
            LHero3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LHero3",
                Tag = 2,
                OnDraw = LHeroesDraw,
                OnUpdate = LHeroesUpdate,
                Priority = 1700,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LHero3);
            LHero4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LHero4",
                Tag = 3,
                OnDraw = LHeroesDraw,
                OnUpdate = LHeroesUpdate,
                Priority = 1800,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LHero4);
            LItem1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LItem1",
                OnDraw = LItemsDraw,
                OnUpdate = LItemsUpdate,
                Priority = 3000,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LItem1);
            LItemPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LItemPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 186,
                WinWidth = 257,
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
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 213,
                    BottomOffset = 115
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LItemPnl);
            LItem2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LItem2",
                Tag = 1,
                OnDraw = LItemsDraw,
                OnUpdate = LItemsUpdate,
                Priority = 3000,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LItem2);
            LItem3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LItem3",
                Tag = 2,
                OnDraw = LItemsDraw,
                OnUpdate = LItemsUpdate,
                Priority = 3000,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LItem3);
            LItem4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LItem4",
                Tag = 3,
                OnDraw = LItemsDraw,
                OnUpdate = LItemsUpdate,
                Priority = 3000,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LItem4);
            LUnit1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LUnit1",
                OnDraw = LUnitsDraw,
                OnUpdate = LUnitsUpdate,
                Priority = 4500,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LUnitSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LUnit1);
            LUnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LUnitPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 256,
                WinWidth = 257,
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
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 276,
                    BottomOffset = 45
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LUnitPnl);
            LUnit2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LUnit2",
                Tag = 1,
                OnDraw = LUnitsDraw,
                OnUpdate = LUnitsUpdate,
                Priority = 4500,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LUnitSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LUnit2);
            LUnit3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LUnit3",
                Tag = 2,
                OnDraw = LUnitsDraw,
                OnUpdate = LUnitsUpdate,
                Priority = 4500,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LUnitSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LUnit3);
            LUnit4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LUnit4",
                Tag = 3,
                OnDraw = LUnitsDraw,
                OnUpdate = LUnitsUpdate,
                Priority = 4500,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = LUnitSelect,
                RadioIndex = 0,
                OnDoubleClick = AddBtnClick
            };
            LayoutSession.EndRead(LUnit4);
            Available = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Available",
                Priority = 10,
                WinLeft = 99,
                WinTop = 42,
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
                    TopOffset = 42,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Available",
                TextColor = 12632256
            };
            LayoutSession.EndRead(Available);
            TransBookWin = new Aow.Units.AoWBookWindow.TAoWBookWindow(this)
            {
                Name = "TransBookWin",
                Visible = false,
                Priority = 275,
                WinLeft = 21,
                WinTop = 8,
                WinWidth = 599,
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
                    TopOffset = 2,
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
                Dockable = false
            };
            LayoutSession.EndRead(TransBookWin);
            BookStart = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BookStart",
                Priority = 0,
                WinLeft = 475,
                WinTop = 386,
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
                TextColor = 1055529,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = BookStartClick
            };
            LayoutSession.EndRead(BookStart);
            Cancel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Cancel",
                Priority = 0,
                WinLeft = 520,
                WinTop = 386,
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
                TextColor = 1055529,
                TextSelColor = 0,
                WinWidth = 39,
                OnClick = Layout_CancelClick_0
            };
            LayoutSession.EndRead(Cancel);
            PnlL = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PnlL",
                Priority = 10,
                WinLeft = 30,
                WinTop = 12,
                WinWidth = 265,
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
                    LeftOffset = 30,
                    RightOffset = 5,
                    TopOffset = 12,
                    BottomOffset = 84
                },
                WinHeight = 369,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PnlL);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 10,
                WinLeft = 53,
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
                Text = "Transfer Resources",
                TextColor = 1055529
            };
            LayoutSession.EndRead(Title);
            PnlR = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PnlR",
                Priority = 10,
                WinLeft = 306,
                WinTop = 12,
                WinWidth = 265,
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
                    LeftOffset = 7,
                    RightOffset = 28,
                    TopOffset = 12,
                    BottomOffset = 84
                },
                WinHeight = 369,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PnlR);
            TransferPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TransferPointsLbl",
                Priority = 10,
                WinLeft = 92,
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
                Text = "1000/5000",
                TextColor = 1055529
            };
            LayoutSession.EndRead(TransferPointsLbl);
            AddBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AddBtn",
                Priority = 100,
                WinLeft = 15,
                WinTop = 333,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds selected resource",
                    "Clicking on a selected item again will also add it"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 6,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 10
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "Add",
                TextColor = 1254460,
                TextSelColor = 16777215,
                WinWidth = 115,
                OnClick = AddBtnClick
            };
            LayoutSession.EndRead(AddBtn);
            LGoldLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LGoldLbl",
                Priority = 10,
                WinLeft = 55,
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
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Gold",
                TextColor = 1055529
            };
            LayoutSession.EndRead(LGoldLbl);
            GoldLeftLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GoldLeftLbl",
                Priority = 10,
                WinLeft = 119,
                WinTop = 74,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Available gold"
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "290",
                TextColor = 1055529
            };
            LayoutSession.EndRead(GoldLeftLbl);
            LManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LManaLbl",
                Priority = 10,
                WinLeft = 55,
                WinTop = 96,
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
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Mana",
                TextColor = 1055529
            };
            LayoutSession.EndRead(LManaLbl);
            ManaLeftLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ManaLeftLbl",
                Priority = 10,
                WinLeft = 114,
                WinTop = 96,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Available mana"
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3400",
                TextColor = 1055529
            };
            LayoutSession.EndRead(ManaLeftLbl);
            Next = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Next",
                Priority = 10,
                WinLeft = 77,
                WinTop = 42,
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
                    TopOffset = 42,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Next Scenario",
                TextColor = 12632256
            };
            LayoutSession.EndRead(Next);
            LSep1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LSep1",
                Priority = 0,
                WinLeft = 20,
                WinTop = 35,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 225,
                WinHeight = 2,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 15,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LSep1);
            LHeroImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LHeroImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LHeroImgL);
            LHeroPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LHeroPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 116,
                WinWidth = 257,
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
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 150,
                    BottomOffset = 185
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LHeroPnl);
            LItemImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LItemImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LItemImgL);
            LUnitImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LUnitImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LUnitImgL);
            LHeroImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LHeroImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LHeroImgR);
            LItemImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LItemImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LItemImgR);
            LUnitImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LUnitImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LUnitImgR);
            RHero1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RHero1",
                OnDraw = RHeroesDraw,
                OnUpdate = RHeroesUpdate,
                Priority = 1500,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RHero1);
            RHero2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RHero2",
                Tag = 1,
                OnDraw = RHeroesDraw,
                OnUpdate = RHeroesUpdate,
                Priority = 1500,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RHero2);
            RHero3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RHero3",
                Tag = 2,
                OnDraw = RHeroesDraw,
                OnUpdate = RHeroesUpdate,
                Priority = 1500,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RHero3);
            RHero4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RHero4",
                Tag = 3,
                OnDraw = RHeroesDraw,
                OnUpdate = RHeroesUpdate,
                Priority = 1500,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RHeroesSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RHero4);
            RItem1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RItem1",
                OnDraw = RItemsDraw,
                OnUpdate = RItemsUpdate,
                Priority = 1500,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RItem1);
            RItemPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RItemPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 186,
                WinWidth = 257,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 213,
                    BottomOffset = 115
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RItemPnl);
            RItem2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RItem2",
                Tag = 1,
                OnDraw = RItemsDraw,
                OnUpdate = RItemsUpdate,
                Priority = 1500,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RItem2);
            RItem3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RItem3",
                Tag = 2,
                OnDraw = RItemsDraw,
                OnUpdate = RItemsUpdate,
                Priority = 1500,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RItem3);
            RItem4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RItem4",
                Tag = 3,
                OnDraw = RItemsDraw,
                OnUpdate = RItemsUpdate,
                Priority = 1500,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RItemsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RItem4);
            RUnit1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RUnit1",
                OnDraw = RUnitsDraw,
                OnUpdate = RUnitsUpdate,
                Priority = 1500,
                WinLeft = 26,
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
                    LeftOffset = 26,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RUnitsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RUnit1);
            RUnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RUnitPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 256,
                WinWidth = 257,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 276,
                    BottomOffset = 45
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RUnitPnl);
            RUnit2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RUnit2",
                Tag = 1,
                OnDraw = RUnitsDraw,
                OnUpdate = RUnitsUpdate,
                Priority = 1500,
                WinLeft = 77,
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
                    LeftOffset = 77,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RUnitsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RUnit2);
            RUnit3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RUnit3",
                Tag = 2,
                OnDraw = RUnitsDraw,
                OnUpdate = RUnitsUpdate,
                Priority = 1500,
                WinLeft = 128,
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
                    LeftOffset = 128,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RUnitsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RUnit3);
            RUnit4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RUnit4",
                Tag = 3,
                OnDraw = RUnitsDraw,
                OnUpdate = RUnitsUpdate,
                Priority = 1500,
                WinLeft = 179,
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
                    RightOffset = 26,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 19,
                ILIndexLit = 18,
                ILIndexUp = 18,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                OnChange = RUnitsSelect,
                RadioIndex = 0,
                OnDoubleClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RUnit4);
            RHeroImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RHeroImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RHeroImgL);
            RHeroPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RHeroPnl",
                Priority = 10,
                WinLeft = 4,
                WinTop = 116,
                WinWidth = 257,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 150,
                    BottomOffset = 185
                },
                WinHeight = 72,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RHeroPnl);
            RItemImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RItemImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RItemImgL);
            RUnitImgL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RUnitImgL",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RUnitImgL);
            RHeroImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RHeroImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RHeroImgR);
            RItemImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RItemImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RItemImgR);
            RUnitImgR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RUnitImgR",
                Visible = false,
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 0,
                WinWidth = 13,
                WinHeight = 39,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RUnitImgR);
            RSep1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RSep1",
                Priority = 0,
                WinLeft = 20,
                WinTop = 35,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 225,
                WinHeight = 2,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 15,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RSep1);
            RemoveBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RemoveBtn",
                Priority = 100,
                WinLeft = 62,
                WinTop = 333,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes selected resource"
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
                    TopOffset = 40,
                    BottomOffset = 10
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "Remove",
                TextColor = 1254460,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = RemoveBtnClick
            };
            LayoutSession.EndRead(RemoveBtn);
            LSep2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LSep2",
                Priority = 0,
                WinLeft = 20,
                WinTop = 65,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 65,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 225,
                WinHeight = 2,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 15,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LSep2);
            RSep2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RSep2",
                Priority = 0,
                WinLeft = 20,
                WinTop = 65,
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
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 65,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 225,
                WinHeight = 2,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 15,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RSep2);
            GoldSlider = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "GoldSlider",
                Priority = 0,
                WinLeft = 170,
                WinTop = 79,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets Gold"
                },
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
                    LeftOffset = 170,
                    RightOffset = 30,
                    TopOffset = 79,
                    BottomOffset = 0
                },
                Masked = true,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 9,
                WinWidth = 65,
                NumberVisible = 1,
                OnChange = GoldSliderChange,
                VanishWhenFull = true,
                ILIndexIncUp = 25,
                ILIndexIncDown = 26,
                ILIndexDecUp = 23,
                ILIndexDecDown = 24,
                ILIndexBack = 27,
                ILIndexTab = 28
            };
            LayoutSession.EndRead(GoldSlider);
            ManaSlider = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "ManaSlider",
                Priority = 0,
                WinLeft = 170,
                WinTop = 99,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets Mana"
                },
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
                    LeftOffset = 170,
                    RightOffset = 30,
                    TopOffset = 99,
                    BottomOffset = 0
                },
                Masked = true,
                Min = 0,
                Max = 20,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 9,
                WinWidth = 65,
                NumberVisible = 1,
                OnChange = ManaSliderChange,
                VanishWhenFull = true,
                ILIndexIncUp = 25,
                ILIndexIncDown = 26,
                ILIndexDecUp = 23,
                ILIndexDecDown = 24,
                ILIndexBack = 27,
                ILIndexTab = 28
            };
            LayoutSession.EndRead(ManaSlider);
            LHeroLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LHeroLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LHeroLBClick
            };
            LayoutSession.EndRead(LHeroLB);
            AutoBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AutoBtn",
                Priority = 100,
                WinLeft = 135,
                WinTop = 333,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Automatically assigns resources to next scenario"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 51,
                    RightOffPercent = 6,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 10
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "Auto",
                TextColor = 1254460,
                TextSelColor = 16777215,
                WinWidth = 115,
                OnClick = AutoBtnClick
            };
            LayoutSession.EndRead(AutoBtn);
            LItemLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LItemLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LItemLBClick
            };
            LayoutSession.EndRead(LItemLB);
            LUnitLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LUnitLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LUnitLBClick
            };
            LayoutSession.EndRead(LUnitLB);
            LHeroRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LHeroRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LHeroRBClick
            };
            LayoutSession.EndRead(LHeroRB);
            LItemRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LItemRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LItemRBClick
            };
            LayoutSession.EndRead(LItemRB);
            LUnitRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LUnitRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = LUnitRBClick
            };
            LayoutSession.EndRead(LUnitRB);
            RHeroLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RHeroLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RHeroLBClick
            };
            LayoutSession.EndRead(RHeroLB);
            RItemLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RItemLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RItemLBClick
            };
            LayoutSession.EndRead(RItemLB);
            RUnitLB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RUnitLB",
                Priority = 0,
                WinLeft = 10,
                WinTop = 17,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 11,
                ILIndexLit = 10,
                ILIndexUp = 9,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RUnitLBClick
            };
            LayoutSession.EndRead(RUnitLB);
            RHeroRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RHeroRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RHeroRBClick
            };
            LayoutSession.EndRead(RHeroRB);
            RItemRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RItemRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RItemRBClick
            };
            LayoutSession.EndRead(RItemRB);
            RUnitRB = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RUnitRB",
                Priority = 0,
                WinLeft = 234,
                WinTop = 17,
                CaptureInput = true,
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
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 13,
                ILIndexLit = 14,
                ILIndexUp = 12,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = RUnitRBClick
            };
            LayoutSession.EndRead(RUnitRB);
            CostImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CostImg",
                Visible = false,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 0,
                WinWidth = 29,
                WinHeight = 11,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CostImg);
            CostPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CostPnl",
                Visible = false,
                Priority = 2000,
                WinLeft = 37,
                WinTop = 61,
                WinWidth = 29,
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
                    LeftOffset = 37,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 11,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CostPnl);
            CostLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CostLbl",
                Visible = false,
                Priority = 2000,
                WinLeft = 7,
                WinTop = 1,
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "100",
                TextColor = 16777215
            };
            LayoutSession.EndRead(CostLbl);
            LGoldImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LGoldImg",
                Priority = 0,
                WinLeft = 30,
                WinTop = 75,
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
                    TopOffset = 75,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 0,
                WinWidth = 16,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LGoldImg);
            LManaImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LManaImg",
                Priority = 0,
                WinLeft = 30,
                WinTop = 96,
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
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 32,
                AnimEnd = 0,
                WinWidth = 16,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LManaImg);
            TGoldLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TGoldLbl",
                Priority = 10,
                WinLeft = 55,
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
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Gold",
                TextColor = 1055529
            };
            LayoutSession.EndRead(TGoldLbl);
            TransferGoldLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TransferGoldLbl",
                Priority = 10,
                WinLeft = 119,
                WinTop = 74,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Available gold"
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "290",
                TextColor = 1055529
            };
            LayoutSession.EndRead(TransferGoldLbl);
            TManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TManaLbl",
                Priority = 10,
                WinLeft = 55,
                WinTop = 96,
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
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Mana",
                TextColor = 1055529
            };
            LayoutSession.EndRead(TManaLbl);
            TransferManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TransferManaLbl",
                Priority = 10,
                WinLeft = 114,
                WinTop = 96,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Available mana"
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
                    LeftOffset = 120,
                    RightOffset = 0,
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3400",
                TextColor = 1055529
            };
            LayoutSession.EndRead(TransferManaLbl);
            RGoldImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RGoldImg",
                Priority = 0,
                WinLeft = 30,
                WinTop = 75,
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
                    TopOffset = 75,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 0,
                WinWidth = 16,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RGoldImg);
            RManaImg = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RManaImg",
                Priority = 0,
                WinLeft = 30,
                WinTop = 96,
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
                    TopOffset = 96,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 32,
                AnimEnd = 0,
                WinWidth = 16,
                WinHeight = 14,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RManaImg);
            ResBackG = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ResBackG",
                Visible = false,
                Priority = 0,
                WinLeft = 28,
                WinTop = 2,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 0,
                WinWidth = 48,
                WinHeight = 60,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ResBackG);
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
            LHero1.AOWWindow = LHeroPnl;
            LHero2.AOWWindow = LHeroPnl;
            LHero3.AOWWindow = LHeroPnl;
            LHero4.AOWWindow = LHeroPnl;
            LItem1.AOWWindow = LItemPnl;
            LItemPnl.AOWWindow = PnlL;
            LItem2.AOWWindow = LItemPnl;
            LItem3.AOWWindow = LItemPnl;
            LItem4.AOWWindow = LItemPnl;
            LUnit1.AOWWindow = LUnitPnl;
            LUnitPnl.AOWWindow = PnlL;
            LUnit2.AOWWindow = LUnitPnl;
            LUnit3.AOWWindow = LUnitPnl;
            LUnit4.AOWWindow = LUnitPnl;
            Available.AOWWindow = PnlL;
            BookStart.AOWWindow = TransBookWin;
            Cancel.AOWWindow = TransBookWin;
            PnlL.AOWWindow = TransBookWin;
            Title.AOWWindow = PnlL;
            PnlR.AOWWindow = TransBookWin;
            TransferPointsLbl.AOWWindow = PnlR;
            AddBtn.AOWWindow = PnlL;
            LGoldLbl.AOWWindow = PnlL;
            GoldLeftLbl.AOWWindow = PnlL;
            LManaLbl.AOWWindow = PnlL;
            ManaLeftLbl.AOWWindow = PnlL;
            Next.AOWWindow = PnlR;
            LSep1.AOWWindow = PnlL;
            LHeroImgL.AOWWindow = LHeroPnl;
            LHeroPnl.AOWWindow = PnlL;
            LItemImgL.AOWWindow = LItemPnl;
            LUnitImgL.AOWWindow = LUnitPnl;
            LHeroImgR.AOWWindow = LHeroPnl;
            LItemImgR.AOWWindow = LItemPnl;
            LUnitImgR.AOWWindow = LUnitPnl;
            RHero1.AOWWindow = RHeroPnl;
            RHero2.AOWWindow = RHeroPnl;
            RHero3.AOWWindow = RHeroPnl;
            RHero4.AOWWindow = RHeroPnl;
            RItem1.AOWWindow = RItemPnl;
            RItemPnl.AOWWindow = PnlR;
            RItem2.AOWWindow = RItemPnl;
            RItem3.AOWWindow = RItemPnl;
            RItem4.AOWWindow = RItemPnl;
            RUnit1.AOWWindow = RUnitPnl;
            RUnitPnl.AOWWindow = PnlR;
            RUnit2.AOWWindow = RUnitPnl;
            RUnit3.AOWWindow = RUnitPnl;
            RUnit4.AOWWindow = RUnitPnl;
            RHeroImgL.AOWWindow = RHeroPnl;
            RHeroPnl.AOWWindow = PnlR;
            RItemImgL.AOWWindow = RItemPnl;
            RUnitImgL.AOWWindow = RUnitPnl;
            RHeroImgR.AOWWindow = RHeroPnl;
            RItemImgR.AOWWindow = RItemPnl;
            RUnitImgR.AOWWindow = RUnitPnl;
            RSep1.AOWWindow = PnlR;
            RemoveBtn.AOWWindow = PnlR;
            LSep2.AOWWindow = PnlL;
            RSep2.AOWWindow = PnlR;
            GoldSlider.AOWWindow = PnlL;
            ManaSlider.AOWWindow = PnlL;
            LHeroLB.AOWWindow = LHeroPnl;
            AutoBtn.AOWWindow = PnlL;
            LItemLB.AOWWindow = LItemPnl;
            LUnitLB.AOWWindow = LUnitPnl;
            LHeroRB.AOWWindow = LHeroPnl;
            LItemRB.AOWWindow = LItemPnl;
            LUnitRB.AOWWindow = LUnitPnl;
            RHeroLB.AOWWindow = RHeroPnl;
            RItemLB.AOWWindow = RItemPnl;
            RUnitLB.AOWWindow = RUnitPnl;
            RHeroRB.AOWWindow = RHeroPnl;
            RItemRB.AOWWindow = RItemPnl;
            RUnitRB.AOWWindow = RUnitPnl;
            CostImg.AOWWindow = CostPnl;
            CostPnl.AOWWindow = LHeroPnl;
            CostLbl.AOWWindow = CostPnl;
            LGoldImg.AOWWindow = PnlL;
            LManaImg.AOWWindow = PnlL;
            TGoldLbl.AOWWindow = PnlR;
            TransferGoldLbl.AOWWindow = PnlR;
            TManaLbl.AOWWindow = PnlR;
            TransferManaLbl.AOWWindow = PnlR;
            RGoldImg.AOWWindow = PnlR;
            RManaImg.AOWWindow = PnlR;
            ResBackG.AOWWindow = LHeroPnl;
            Loaded();
            LHero1.Loaded();
            LHero2.Loaded();
            LHero3.Loaded();
            LHero4.Loaded();
            LItem1.Loaded();
            LItemPnl.Loaded();
            LItem2.Loaded();
            LItem3.Loaded();
            LItem4.Loaded();
            LUnit1.Loaded();
            LUnitPnl.Loaded();
            LUnit2.Loaded();
            LUnit3.Loaded();
            LUnit4.Loaded();
            Available.Loaded();
            TransBookWin.Loaded();
            BookStart.Loaded();
            Cancel.Loaded();
            PnlL.Loaded();
            Title.Loaded();
            PnlR.Loaded();
            TransferPointsLbl.Loaded();
            AddBtn.Loaded();
            LGoldLbl.Loaded();
            GoldLeftLbl.Loaded();
            LManaLbl.Loaded();
            ManaLeftLbl.Loaded();
            Next.Loaded();
            LSep1.Loaded();
            LHeroImgL.Loaded();
            LHeroPnl.Loaded();
            LItemImgL.Loaded();
            LUnitImgL.Loaded();
            LHeroImgR.Loaded();
            LItemImgR.Loaded();
            LUnitImgR.Loaded();
            RHero1.Loaded();
            RHero2.Loaded();
            RHero3.Loaded();
            RHero4.Loaded();
            RItem1.Loaded();
            RItemPnl.Loaded();
            RItem2.Loaded();
            RItem3.Loaded();
            RItem4.Loaded();
            RUnit1.Loaded();
            RUnitPnl.Loaded();
            RUnit2.Loaded();
            RUnit3.Loaded();
            RUnit4.Loaded();
            RHeroImgL.Loaded();
            RHeroPnl.Loaded();
            RItemImgL.Loaded();
            RUnitImgL.Loaded();
            RHeroImgR.Loaded();
            RItemImgR.Loaded();
            RUnitImgR.Loaded();
            RSep1.Loaded();
            RemoveBtn.Loaded();
            LSep2.Loaded();
            RSep2.Loaded();
            GoldSlider.Loaded();
            ManaSlider.Loaded();
            LHeroLB.Loaded();
            AutoBtn.Loaded();
            LItemLB.Loaded();
            LUnitLB.Loaded();
            LHeroRB.Loaded();
            LItemRB.Loaded();
            LUnitRB.Loaded();
            RHeroLB.Loaded();
            RItemLB.Loaded();
            RUnitLB.Loaded();
            RHeroRB.Loaded();
            RItemRB.Loaded();
            RUnitRB.Loaded();
            CostImg.Loaded();
            CostPnl.Loaded();
            CostLbl.Loaded();
            LGoldImg.Loaded();
            LManaImg.Loaded();
            TGoldLbl.Loaded();
            TransferGoldLbl.Loaded();
            TManaLbl.Loaded();
            TransferManaLbl.Loaded();
            RGoldImg.Loaded();
            RManaImg.Loaded();
            ResBackG.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            LHero1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHero2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHero3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHero4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItem1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItem2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItem3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItem4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnit1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnit2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnit3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnit4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            Available.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            TransBookWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TransBookWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            TransBookWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            BookStart.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Cancel.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.BookWin;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            TransferPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            AddBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AddBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            AddBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LGoldLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            GoldLeftLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GoldLeftLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            ManaLeftLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaLeftLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Next.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            LSep1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHeroImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItemImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnitImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHeroImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItemImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnitImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHero1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHero2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHero3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHero4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItem1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItem2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItem3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItem4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnit1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnit2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnit3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnit4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHeroImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItemImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnitImgL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHeroImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItemImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnitImgR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RSep1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RemoveBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RemoveBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            RemoveBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LSep2.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RSep2.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            GoldSlider.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GoldSlider.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            GoldSlider.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            GoldSlider.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            GoldSlider.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            GoldSlider.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            ManaSlider.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            ManaSlider.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            ManaSlider.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            ManaSlider.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            ManaSlider.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHeroLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            AutoBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AutoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            AutoBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItemLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnitLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LHeroRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LItemRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            LUnitRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHeroLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItemLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnitLB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RHeroRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RItemRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            RUnitRB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            CostImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            CostLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            LGoldImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            LManaImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            TGoldLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            TransferGoldLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TransferGoldLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            TManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            TransferManaLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            TransferManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            RGoldImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            RManaImg.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            ResBackG.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
        }

        private void Layout_CancelClick_0(TObject p0)
        {
            CancelClick();
        }
    }
}
