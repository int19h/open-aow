// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWHeroUpgradeDlg
{
    public partial class THeroUpgradeDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "HeroUpgradeDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 117,
                WinTop = 0,
                WinWidth = 407,
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
                WinHeight = 432,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 250,
                WinLeft = 219,
                WinTop = 395,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancels upgrading the hero."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 54,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 53,
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
                WinWidth = 135,
                OnClick = Layout_CloseClick_1
            };
            LayoutSession.EndRead(CancelBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 395,
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
                WinWidth = 365,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            Done = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Done",
                Priority = 250,
                WinLeft = 53,
                WinTop = 395,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Upgrades the hero to the selected settings."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 54,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 53,
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
                Text = "Done",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 135,
                OnClick = Layout_DoneClick_2
            };
            LayoutSession.EndRead(Done);
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
                Text = "Upgrade Hero",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 372,
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
                    RightOffset = 20,
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
                CurrentImage = 3,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            CeltM = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltM",
                Priority = 100,
                WinLeft = 190,
                WinTop = 395,
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
                    LeftOffset = 10,
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
            LayoutSession.EndRead(CeltM);
            CeltL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltL",
                Priority = 100,
                WinLeft = 27,
                WinTop = 395,
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
                    LeftOffset = 27,
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
            LayoutSession.EndRead(CeltL);
            CeltR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltR",
                Priority = 100,
                WinLeft = 353,
                WinTop = 395,
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
                    LeftOffset = 27,
                    RightOffset = 27,
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
            LayoutSession.EndRead(CeltR);
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
                WinWidth = 365,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TCeltL);
            TCelt = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCelt",
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
                    RightOffset = 30,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 349,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCelt);
            UpgradePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgradePnl",
                Priority = 2000,
                WinLeft = 24,
                WinTop = 244,
                WinWidth = 359,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 5,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 38
                },
                WinHeight = 150,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgradePnl);
            HeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeaderLbl",
                Priority = 0,
                WinLeft = 90,
                WinTop = 15,
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
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#######",
                TextColor = 0
            };
            LayoutSession.EndRead(HeaderLbl);
            SkillPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SkillPointsLbl",
                Priority = 0,
                WinLeft = 41,
                WinTop = 50,
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
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Skill Points left to spend",
                TextColor = 13165253
            };
            LayoutSession.EndRead(SkillPointsLbl);
            AvailableAbilities = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AvailableAbilities",
                OnMouseDown = AvailableAbilitiesMouseDown,
                Priority = 50,
                WinLeft = 185,
                WinTop = 15,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 185,
                    RightOffset = 50,
                    TopOffset = 15,
                    BottomOffset = 28
                },
                Masked = false,
                WinWidth = 126,
                WinHeight = 107,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = Layout_AvailableAbilitiesChange_3,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(AvailableAbilities);
            AvailableAbilitiesSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AvailableAbilitiesSB",
                Visible = false,
                Priority = 41,
                WinLeft = 337,
                WinTop = 15,
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
                LargeChange = 1,
                Position = 0,
                WinHeight = 107,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_AvailableAbilitiesSBChange_4,
                VanishWhenFull = true,
                ILIndexIncUp = 74,
                ILIndexIncDown = 75,
                ILIndexDecUp = 72,
                ILIndexDecDown = 73,
                ILIndexBack = 79,
                ILIndexTab = 76
            };
            LayoutSession.EndRead(AvailableAbilitiesSB);
            SelectedAbilities = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SelectedAbilities",
                OnMouseDown = SelectedAbilitiesMouseDown,
                Priority = 50,
                WinLeft = 5,
                WinTop = 15,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 28
                },
                Masked = false,
                WinWidth = 124,
                WinHeight = 107,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = Layout_SelectedAbilitiesChange_5,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(SelectedAbilities);
            SelectedAbilitiesSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SelectedAbilitiesSB",
                Visible = false,
                Priority = 42,
                WinLeft = 157,
                WinTop = 15,
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
                LargeChange = 1,
                Position = 0,
                WinHeight = 107,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_SelectedAbilitiesSBChange_6,
                VanishWhenFull = true,
                ILIndexIncUp = 74,
                ILIndexIncDown = 75,
                ILIndexDecUp = 72,
                ILIndexDecDown = 73,
                ILIndexBack = 79,
                ILIndexTab = 76
            };
            LayoutSession.EndRead(SelectedAbilitiesSB);
            SelectedAbilitiesSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelectedAbilitiesSort",
                Priority = 1000,
                WinLeft = 5,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Current Abilities",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 123,
                OnClick = Layout_SelectedAbilitiesSortClick_7
            };
            LayoutSession.EndRead(SelectedAbilitiesSort);
            AvailableAbilityCostSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AvailableAbilityCostSort",
                Priority = 1000,
                WinLeft = 310,
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
                    RightOffset = 5,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Cost",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 44,
                OnClick = Layout_AvailableAbilityCostSortClick_8
            };
            LayoutSession.EndRead(AvailableAbilityCostSort);
            AvailableAbilitiesCost = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AvailableAbilitiesCost",
                OnMouseDown = AvailableAbilitiesMouseDown,
                Priority = 40,
                WinLeft = 308,
                WinTop = 15,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 51,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 5,
                    TopOffset = 15,
                    BottomOffset = 28
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 107,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = Layout_AvailableAbilitiesCostChange_9,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(AvailableAbilitiesCost);
            AvailableAbilitiesSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AvailableAbilitiesSort",
                Priority = 1000,
                WinLeft = 185,
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
                    LeftOffset = 185,
                    RightOffset = 51,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Upgrades",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 125,
                OnClick = Layout_AvailableAbilitiesSortClick_10
            };
            LayoutSession.EndRead(AvailableAbilitiesSort);
            AddAbilityBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AddAbilityBtn",
                Priority = 100,
                WinLeft = 199,
                WinTop = 125,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds the selected ability"
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Add",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_AddAbilityBtnClick_11
            };
            LayoutSession.EndRead(AddAbilityBtn);
            RemoveBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RemoveBtn",
                Priority = 100,
                WinLeft = 20,
                WinTop = 125,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes the selected ability."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 20,
                    RightOffset = 20,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Remove",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_RemoveBtnClick_12
            };
            LayoutSession.EndRead(RemoveBtn);
            FaceFrame = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFrame",
                OnPreDraw = FaceFramePreDraw,
                Priority = 100,
                WinLeft = 0,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 38,
                AnimEnd = 6,
                WinWidth = 118,
                WinHeight = 142,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFrame);
            UnitName = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "UnitName",
                Priority = 0,
                WinLeft = 112,
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
                    LeftOffset = 112,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 257,
                Text = "########",
                BorderWidth = 10,
                BorderHeight = 7,
                MaxChars = 0,
                CursorChar = (byte)124,
                BlinkDelay = 0,
                TextColor = 13165253,
                Editable = false,
                ILIndex = 12,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(UnitName);
            Alignment = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "Alignment",
                Priority = 0,
                WinLeft = 111,
                WinTop = 116,
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
                    LeftOffset = 111,
                    RightOffset = 0,
                    TopOffset = 116,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 259,
                Text = "Assign Skill Points",
                BorderWidth = 10,
                BorderHeight = 8,
                MaxChars = 0,
                CursorChar = (byte)124,
                BlinkDelay = 0,
                TextColor = 13165253,
                Editable = false,
                ILIndex = 12,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(Alignment);
            TopPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TopPnl",
                Priority = 2000,
                WinLeft = 20,
                WinTop = 22,
                WinWidth = 365,
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
                    LeftOffset = 20,
                    RightOffset = 22,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 142,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TopPnl);
            StatisicsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "StatisicsPnl",
                Priority = 9999,
                WinLeft = 30,
                WinTop = 170,
                WinWidth = 347,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 170,
                    BottomOffset = 4
                },
                WinHeight = 70,
                ILIndexFrame = 96,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(StatisicsPnl);
            AttValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(AttValue);
            AttLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Attack",
                TextColor = 9402970
            };
            LayoutSession.EndRead(AttLbl);
            AttPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AttPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 167,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AttPnl);
            DamValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DamValue);
            DamLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Damage",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DamLbl);
            DamPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DamPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 44,
                WinWidth = 167,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 44,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DamPnl);
            DefValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DefValue);
            DefLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Defense",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DefLbl);
            DefPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DefPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 22,
                WinWidth = 167,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 52,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DefPnl);
            ResValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResValue);
            ResLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Resistance",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResLbl);
            ResPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResPnl",
                Priority = 0,
                WinLeft = 180,
                WinTop = 0,
                WinWidth = 167,
                Masked = false,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResPnl);
            HitValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(HitValue);
            HitLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Hits",
                TextColor = 9402970
            };
            LayoutSession.EndRead(HitLbl);
            HitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "HitPnl",
                Priority = 0,
                WinLeft = 180,
                WinTop = 22,
                WinWidth = 167,
                Masked = false,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(HitPnl);
            MovValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovValue",
                Priority = 0,
                WinLeft = 142,
                WinTop = 6,
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
                    RightOffset = 15,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 9402970
            };
            LayoutSession.EndRead(MovValue);
            MovLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovLbl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 6,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Moves",
                TextColor = 9402970
            };
            LayoutSession.EndRead(MovLbl);
            MovPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MovPnl",
                Priority = 0,
                WinLeft = 180,
                WinTop = 44,
                WinWidth = 167,
                Masked = false,
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
                    LeftOffset = 180,
                    RightOffset = 0,
                    TopOffset = 44,
                    BottomOffset = 0
                },
                WinHeight = 22,
                ILIndexFrame = 64,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MovPnl);
            AttI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(AttI);
            DamI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DamI);
            DefI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DefI);
            ResI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResI);
            HitI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(HitI);
            MovI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovI",
                Priority = 0,
                WinLeft = 130,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 9402970
            };
            LayoutSession.EndRead(MovI);
            AttA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(AttA);
            DamA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(DamA);
            DefA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(DefA);
            ResA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(ResA);
            HitA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(HitA);
            MovA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovA",
                Priority = 0,
                WinLeft = 118,
                WinTop = 6,
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
                    LeftOffset = 118,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "3",
                TextColor = 12632256
            };
            LayoutSession.EndRead(MovA);
            AttP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(AttP);
            DamP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DamP);
            DefP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DefP);
            ResP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResP);
            HitP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(HitP);
            MovP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovP",
                Priority = 0,
                WinLeft = 102,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 9402970
            };
            LayoutSession.EndRead(MovP);
            AttB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(AttB);
            DamB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DamB);
            DefB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DefB);
            ResB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(ResB);
            HitB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(HitB);
            MovB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovB",
                Priority = 0,
                WinLeft = 90,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "9",
                TextColor = 9402970
            };
            LayoutSession.EndRead(MovB);
            AttUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AttUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_AttUpClick_13
            };
            LayoutSession.EndRead(AttUp);
            AttDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AttDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_AttDnClick_14
            };
            LayoutSession.EndRead(AttDn);
            DamUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DamUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_DamUpClick_15
            };
            LayoutSession.EndRead(DamUp);
            DamDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DamDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_DamDnClick_16
            };
            LayoutSession.EndRead(DamDn);
            DefUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_DefUpClick_17
            };
            LayoutSession.EndRead(DefUp);
            DefDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_DefDnClick_18
            };
            LayoutSession.EndRead(DefDn);
            ResUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ResUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_ResUpClick_19
            };
            LayoutSession.EndRead(ResUp);
            ResDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ResDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_ResDnClick_20
            };
            LayoutSession.EndRead(ResDn);
            HitUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HitUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_HitUpClick_21
            };
            LayoutSession.EndRead(HitUp);
            HitDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HitDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_HitDnClick_22
            };
            LayoutSession.EndRead(HitDn);
            MovUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MovUp",
                Priority = 0,
                WinLeft = 158,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 44,
                ILIndexLit = 45,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_MovUpClick_23
            };
            LayoutSession.EndRead(MovUp);
            MovDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MovDn",
                Priority = 0,
                WinLeft = 158,
                WinTop = 11,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 47,
                ILIndexLit = 48,
                ILIndexUp = 46,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 9,
                OnClick = Layout_MovDnClick_24
            };
            LayoutSession.EndRead(MovDn);
            AttIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AttIc",
                Priority = 0,
                WinLeft = 6,
                WinTop = 1,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 0,
                WinWidth = 15,
                WinHeight = 20,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AttIc);
            DamIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DamIcn",
                Priority = 0,
                WinLeft = 6,
                WinTop = 1,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 0,
                WinWidth = 15,
                WinHeight = 20,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DamIcn);
            DefIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "DefIc",
                Priority = 0,
                WinLeft = 6,
                WinTop = 1,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 0,
                WinWidth = 15,
                WinHeight = 20,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(DefIc);
            ResIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ResIc",
                Priority = 0,
                WinLeft = 6,
                WinTop = 1,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 15,
                WinHeight = 20,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ResIc);
            HitIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "HitIc",
                Priority = 0,
                WinLeft = 6,
                WinTop = 1,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 0,
                WinWidth = 15,
                WinHeight = 20,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(HitIc);
            MovIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MovIc",
                Priority = 0,
                WinLeft = 0,
                WinTop = 3,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 16,
                AnimEnd = 0,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MovIc);
            SelectedAbilitiesCostSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelectedAbilitiesCostSort",
                Priority = 1000,
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
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Cost",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 47,
                OnClick = Layout_SelectedAbilitiesCostSortClick_25
            };
            LayoutSession.EndRead(SelectedAbilitiesCostSort);
            SelectedAbilitiesCost = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SelectedAbilitiesCost",
                OnMouseDown = SelectedAbilitiesMouseDown,
                Priority = 41,
                WinLeft = 128,
                WinTop = 15,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 128,
                    RightOffset = 5,
                    TopOffset = 15,
                    BottomOffset = 28
                },
                Masked = false,
                WinWidth = 46,
                WinHeight = 107,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = Layout_SelectedAbilitiesCostChange_26,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(SelectedAbilitiesCost);
            InfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoPnl",
                Priority = 2000,
                WinLeft = 115,
                WinTop = 26,
                WinWidth = 250,
                Masked = true,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 90,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoPnl);
            LevelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LevelLbl",
                Priority = 0,
                WinLeft = 318,
                WinTop = 7,
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
                    RightOffset = 7,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 13165253
            };
            LayoutSession.EndRead(LevelLbl);
            SkillPoints = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SkillPoints",
                Priority = 0,
                WinLeft = 104,
                WinTop = 67,
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
                    TopOffset = 67,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10/10",
                TextColor = 16777215
            };
            LayoutSession.EndRead(SkillPoints);
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
            CancelBtn.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            Done.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            CeltM.AOWWindow = Dlg;
            CeltL.AOWWindow = Dlg;
            CeltR.AOWWindow = Dlg;
            TCeltL.AOWWindow = Dlg;
            TCelt.AOWWindow = Dlg;
            UpgradePnl.AOWWindow = Dlg;
            HeaderLbl.AOWWindow = InfoPnl;
            SkillPointsLbl.AOWWindow = InfoPnl;
            AvailableAbilities.AOWWindow = UpgradePnl;
            AvailableAbilitiesSB.AOWWindow = UpgradePnl;
            SelectedAbilities.AOWWindow = UpgradePnl;
            SelectedAbilitiesSB.AOWWindow = UpgradePnl;
            SelectedAbilitiesSort.AOWWindow = UpgradePnl;
            AvailableAbilityCostSort.AOWWindow = UpgradePnl;
            AvailableAbilitiesCost.AOWWindow = UpgradePnl;
            AvailableAbilitiesCost.VScrollBar = AvailableAbilitiesSB;
            AvailableAbilitiesSort.AOWWindow = UpgradePnl;
            AddAbilityBtn.AOWWindow = UpgradePnl;
            RemoveBtn.AOWWindow = UpgradePnl;
            FaceFrame.AOWWindow = TopPnl;
            UnitName.AOWWindow = TopPnl;
            Alignment.AOWWindow = TopPnl;
            TopPnl.AOWWindow = Dlg;
            StatisicsPnl.AOWWindow = Dlg;
            AttValue.AOWWindow = AttPnl;
            AttLbl.AOWWindow = AttPnl;
            AttPnl.AOWWindow = StatisicsPnl;
            DamValue.AOWWindow = DamPnl;
            DamLbl.AOWWindow = DamPnl;
            DamPnl.AOWWindow = StatisicsPnl;
            DefValue.AOWWindow = DefPnl;
            DefLbl.AOWWindow = DefPnl;
            DefPnl.AOWWindow = StatisicsPnl;
            ResValue.AOWWindow = ResPnl;
            ResLbl.AOWWindow = ResPnl;
            ResPnl.AOWWindow = StatisicsPnl;
            HitValue.AOWWindow = HitPnl;
            HitLbl.AOWWindow = HitPnl;
            HitPnl.AOWWindow = StatisicsPnl;
            MovValue.AOWWindow = MovPnl;
            MovLbl.AOWWindow = MovPnl;
            MovPnl.AOWWindow = StatisicsPnl;
            AttI.AOWWindow = AttPnl;
            DamI.AOWWindow = DamPnl;
            DefI.AOWWindow = DefPnl;
            ResI.AOWWindow = ResPnl;
            HitI.AOWWindow = HitPnl;
            MovI.AOWWindow = MovPnl;
            AttA.AOWWindow = AttPnl;
            DamA.AOWWindow = DamPnl;
            DefA.AOWWindow = DefPnl;
            ResA.AOWWindow = ResPnl;
            HitA.AOWWindow = HitPnl;
            MovA.AOWWindow = MovPnl;
            AttP.AOWWindow = AttPnl;
            DamP.AOWWindow = DamPnl;
            DefP.AOWWindow = DefPnl;
            ResP.AOWWindow = ResPnl;
            HitP.AOWWindow = HitPnl;
            MovP.AOWWindow = MovPnl;
            AttB.AOWWindow = AttPnl;
            DamB.AOWWindow = DamPnl;
            DefB.AOWWindow = DefPnl;
            ResB.AOWWindow = ResPnl;
            HitB.AOWWindow = HitPnl;
            MovB.AOWWindow = MovPnl;
            AttUp.AOWWindow = AttPnl;
            AttDn.AOWWindow = AttPnl;
            DamUp.AOWWindow = DamPnl;
            DamDn.AOWWindow = DamPnl;
            DefUp.AOWWindow = DefPnl;
            DefDn.AOWWindow = DefPnl;
            ResUp.AOWWindow = ResPnl;
            ResDn.AOWWindow = ResPnl;
            HitUp.AOWWindow = HitPnl;
            HitDn.AOWWindow = HitPnl;
            MovUp.AOWWindow = MovPnl;
            MovDn.AOWWindow = MovPnl;
            AttIc.AOWWindow = AttPnl;
            DamIcn.AOWWindow = DamPnl;
            DefIc.AOWWindow = DefPnl;
            ResIc.AOWWindow = ResPnl;
            HitIc.AOWWindow = HitPnl;
            MovIc.AOWWindow = MovPnl;
            SelectedAbilitiesCostSort.AOWWindow = UpgradePnl;
            SelectedAbilitiesCost.AOWWindow = UpgradePnl;
            SelectedAbilitiesCost.VScrollBar = SelectedAbilitiesSB;
            InfoPnl.AOWWindow = TopPnl;
            LevelLbl.AOWWindow = TopPnl;
            SkillPoints.AOWWindow = InfoPnl;
            Loaded();
            Dlg.Loaded();
            CancelBtn.Loaded();
            BCeltL.Loaded();
            Done.Loaded();
            Title.Loaded();
            Close.Loaded();
            TitleIcon.Loaded();
            CeltM.Loaded();
            CeltL.Loaded();
            CeltR.Loaded();
            TCeltL.Loaded();
            TCelt.Loaded();
            UpgradePnl.Loaded();
            HeaderLbl.Loaded();
            SkillPointsLbl.Loaded();
            AvailableAbilities.Loaded();
            AvailableAbilitiesSB.Loaded();
            SelectedAbilities.Loaded();
            SelectedAbilitiesSB.Loaded();
            SelectedAbilitiesSort.Loaded();
            AvailableAbilityCostSort.Loaded();
            AvailableAbilitiesCost.Loaded();
            AvailableAbilitiesSort.Loaded();
            AddAbilityBtn.Loaded();
            RemoveBtn.Loaded();
            FaceFrame.Loaded();
            UnitName.Loaded();
            Alignment.Loaded();
            TopPnl.Loaded();
            StatisicsPnl.Loaded();
            AttValue.Loaded();
            AttLbl.Loaded();
            AttPnl.Loaded();
            DamValue.Loaded();
            DamLbl.Loaded();
            DamPnl.Loaded();
            DefValue.Loaded();
            DefLbl.Loaded();
            DefPnl.Loaded();
            ResValue.Loaded();
            ResLbl.Loaded();
            ResPnl.Loaded();
            HitValue.Loaded();
            HitLbl.Loaded();
            HitPnl.Loaded();
            MovValue.Loaded();
            MovLbl.Loaded();
            MovPnl.Loaded();
            AttI.Loaded();
            DamI.Loaded();
            DefI.Loaded();
            ResI.Loaded();
            HitI.Loaded();
            MovI.Loaded();
            AttA.Loaded();
            DamA.Loaded();
            DefA.Loaded();
            ResA.Loaded();
            HitA.Loaded();
            MovA.Loaded();
            AttP.Loaded();
            DamP.Loaded();
            DefP.Loaded();
            ResP.Loaded();
            HitP.Loaded();
            MovP.Loaded();
            AttB.Loaded();
            DamB.Loaded();
            DefB.Loaded();
            ResB.Loaded();
            HitB.Loaded();
            MovB.Loaded();
            AttUp.Loaded();
            AttDn.Loaded();
            DamUp.Loaded();
            DamDn.Loaded();
            DefUp.Loaded();
            DefDn.Loaded();
            ResUp.Loaded();
            ResDn.Loaded();
            HitUp.Loaded();
            HitDn.Loaded();
            MovUp.Loaded();
            MovDn.Loaded();
            AttIc.Loaded();
            DamIcn.Loaded();
            DefIc.Loaded();
            ResIc.Loaded();
            HitIc.Loaded();
            MovIc.Loaded();
            SelectedAbilitiesCostSort.Loaded();
            SelectedAbilitiesCost.Loaded();
            InfoPnl.Loaded();
            LevelLbl.Loaded();
            SkillPoints.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Done.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Done.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Done.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            CeltM.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCelt.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            UpgradePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SkillPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            AvailableAbilities.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilities.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AvailableAbilities.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AvailableAbilities.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => AvailableAbilities.PopupMenu = module.BannerPopup);
            AvailableAbilitiesSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilities.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilities.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SelectedAbilities.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SelectedAbilities.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => SelectedAbilities.PopupMenu = module.BannerPopup);
            SelectedAbilitiesSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AvailableAbilityCostSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilityCostSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AvailableAbilitiesCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilitiesCost.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AvailableAbilitiesCost.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AvailableAbilitiesCost.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => AvailableAbilitiesCost.PopupMenu = module.BannerPopup);
            AvailableAbilitiesSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilitiesSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AddAbilityBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AddAbilityBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AddAbilityBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            RemoveBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RemoveBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RemoveBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            UnitName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UnitName.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            Alignment.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            Alignment.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            TopPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            StatisicsPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AttValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AttPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DamValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DamPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DefValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DefPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ResValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ResPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HitValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HitPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MovValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MovPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AttI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AttDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            DamUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            DamDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            DefUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            DefDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            ResUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            ResDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            HitUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            HitDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MovUp.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MovDn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AttIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            DamIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            DefIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            ResIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            HitIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            MovIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            SelectedAbilitiesCostSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesCostSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SelectedAbilitiesCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesCost.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SelectedAbilitiesCost.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SelectedAbilitiesCost.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => SelectedAbilitiesCost.PopupMenu = module.BannerPopup);
            InfoPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LevelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SkillPoints.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }

        private void Layout_DoneClick_2(TObject p0)
        {
            DoneClick();
        }

        private void Layout_AvailableAbilitiesChange_3(TObject p0)
        {
            AvailableAbilitiesChange();
        }

        private void Layout_AvailableAbilitiesSBChange_4(TObject p0)
        {
            AvailableAbilitiesSBChange();
        }

        private void Layout_SelectedAbilitiesChange_5(TObject p0)
        {
            SelectedAbilitiesChange();
        }

        private void Layout_SelectedAbilitiesSBChange_6(TObject p0)
        {
            SelectedAbilitiesSBChange();
        }

        private void Layout_SelectedAbilitiesSortClick_7(TObject p0)
        {
            SelectedAbilitiesSortClick();
        }

        private void Layout_AvailableAbilityCostSortClick_8(TObject p0)
        {
            AvailableAbilityCostSortClick();
        }

        private void Layout_AvailableAbilitiesCostChange_9(TObject p0)
        {
            AvailableAbilitiesCostChange();
        }

        private void Layout_AvailableAbilitiesSortClick_10(TObject p0)
        {
            AvailableAbilitiesSortClick();
        }

        private void Layout_AddAbilityBtnClick_11(TObject p0)
        {
            AddAbilityBtnClick();
        }

        private void Layout_RemoveBtnClick_12(TObject p0)
        {
            RemoveBtnClick();
        }

        private void Layout_AttUpClick_13(TObject p0)
        {
            AttUpClick();
        }

        private void Layout_AttDnClick_14(TObject p0)
        {
            AttDnClick();
        }

        private void Layout_DamUpClick_15(TObject p0)
        {
            DamUpClick();
        }

        private void Layout_DamDnClick_16(TObject p0)
        {
            DamDnClick();
        }

        private void Layout_DefUpClick_17(TObject p0)
        {
            DefUpClick();
        }

        private void Layout_DefDnClick_18(TObject p0)
        {
            DefDnClick();
        }

        private void Layout_ResUpClick_19(TObject p0)
        {
            ResUpClick();
        }

        private void Layout_ResDnClick_20(TObject p0)
        {
            ResDnClick();
        }

        private void Layout_HitUpClick_21(TObject p0)
        {
            HitUpClick();
        }

        private void Layout_HitDnClick_22(TObject p0)
        {
            HitDnClick();
        }

        private void Layout_MovUpClick_23(TObject p0)
        {
            MovUpClick();
        }

        private void Layout_MovDnClick_24(TObject p0)
        {
            MovDnClick();
        }

        private void Layout_SelectedAbilitiesCostSortClick_25(TObject p0)
        {
            SelectedAbilitiesCostSortClick();
        }

        private void Layout_SelectedAbilitiesCostChange_26(TObject p0)
        {
            SelectedAbilitiesCostChange();
        }
    }
}
