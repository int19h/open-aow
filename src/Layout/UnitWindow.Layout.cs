// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWUnitWindow
{
    public partial class TUnitWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "UnitWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = UnitWindowCreate;
            UnitWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "UnitWin",
                OnMouseDown = UnitWinMouseDown,
                OnKeyDown = UnitWinKeyDown,
                OnUpdate = UnitWinUpdate,
                Visible = false,
                Priority = 300,
                WinLeft = 110,
                WinTop = 8,
                WinWidth = 420,
                OnEnter = UnitWinEnter,
                OnExit = UnitWinExit,
                Masked = true,
                OnHide = UnitWinShow,
                OnShow = UnitWinShow,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 10,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = true,
                BorderWidth = 4,
                BorderHeight = 4,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 5808200,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 32,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                OnDrag = UnitWinResize,
                OnResize = UnitWinResize,
                MinWidth = 420,
                MinHeight = 417,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 417,
                Dockable = false
            };
            LayoutSession.EndRead(UnitWin);
            UnitFrameLeft = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UnitFrameLeft",
                Priority = 200,
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
                CurrentImage = 44,
                AnimEnd = 6,
                WinWidth = 118,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UnitFrameLeft);
            UnitTab = new Aow.Units.AoWTabPanel.TAOWTabPanel(this)
            {
                Name = "UnitTab",
                Priority = 3000,
                WinLeft = 21,
                WinTop = 163,
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
                    RightOffset = 20,
                    TopOffset = 163,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 6,
                OnChange = UnitTabChange,
                Selected = -1,
                WinWidth = 379,
                TextColor = 0,
                TextSelColor = 9415072,
                ILIndexDown = 67,
                ILIndexUp = 70,
                BorderHeightSelected = 7
            };
            LayoutSession.EndRead(UnitTab);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 385,
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
                OnClick = CloseClick
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
                Text = "Unit ",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Visible = false,
                Priority = 0,
                WinLeft = 369,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays help on this window"
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
                    RightOffset = 36,
                    TopOffset = 4,
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
                OnClick = HelpClick
            };
            LayoutSession.EndRead(Help);
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
            BCloseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BCloseBtn",
                Priority = 250,
                WinLeft = 212,
                WinTop = 380,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 32,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
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
                Text = "Close",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 119,
                OnClick = CloseClick
            };
            LayoutSession.EndRead(BCloseBtn);
            CeltLineR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltLineR",
                Priority = 100,
                WinLeft = 339,
                WinTop = 380,
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
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltLineR);
            T2Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "T2Memo",
                Priority = 200,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 365,
                WinHeight = 86,
                BorderWidth = 5,
                BorderHeight = 3,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                ItemHeight = 15,
                ColorChangeChar = (byte)126,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2Memo);
            ManaIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ManaIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 66,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Channeling is the amount of mana the Hero can pull from their Leader's pool (Current/Maximum)."
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
                    TopOffset = 66,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 5,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ManaIcon);
            Mana = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Mana",
                Priority = 0,
                WinLeft = 25,
                WinTop = 68,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Channeling is the amount of mana the Hero can pull from their Leader's pool (Current/Maximum)."
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 68,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(Mana);
            T1AbilityLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T1AbilityLB",
                OnMouseDown = T1AbilityLBMouseDown,
                Priority = 100,
                WinLeft = 1,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 164,
                WinHeight = 141,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = T1AbilityLBChange,
                OnDoubleClick = Layout_UseAbility_0,
                Selected = -1,
                OnPreDrawItem = PreDrawAbilityItem,
                TextColor = 14679807,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T1AbilityLB);
            T1UseBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T1UseBut",
                Priority = 100,
                WinLeft = 63,
                WinTop = 174,
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
                    RightOffset = 9,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "####",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 250,
                OnClick = Layout_UseAbility_0
            };
            LayoutSession.EndRead(T1UseBut);
            T1SortName = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T1SortName",
                Priority = 1000,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 54,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Ability Name",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 168,
                OnClick = Layout_T1SortNameClick_1
            };
            LayoutSession.EndRead(T1SortName);
            T1SortType = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T1SortType",
                Priority = 1000,
                WinLeft = 164,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 45,
                    RightOffPercent = 32,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Type",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 85,
                OnClick = Layout_T1SortTypeClick_2
            };
            LayoutSession.EndRead(T1SortType);
            T1SortSource = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T1SortSource",
                Priority = 1000,
                WinLeft = 248,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 68,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Source",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 118,
                OnClick = Layout_T1SortSourceClick_3
            };
            LayoutSession.EndRead(T1SortSource);
            T1UseMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "T1UseMsg",
                Visible = false,
                Priority = 0,
                WinLeft = 109,
                WinTop = 175,
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
                    TopOffset = 158,
                    BottomOffset = 14
                },
                Masked = true,
                Text = "Can only be used in Combat!",
                TextColor = 13165253
            };
            LayoutSession.EndRead(T1UseMsg);
            T1Panel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T1Panel",
                Priority = 900,
                WinLeft = 22,
                WinTop = 178,
                WinWidth = 376,
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
                    LeftOffset = 22,
                    RightOffset = 22,
                    TopOffset = 178,
                    BottomOffset = 38
                },
                WinHeight = 201,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T1Panel);
            T2Panel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2Panel",
                Visible = false,
                Priority = 900,
                WinLeft = 22,
                WinTop = 178,
                WinWidth = 376,
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
                    LeftOffset = 22,
                    RightOffset = 22,
                    TopOffset = 178,
                    BottomOffset = 38
                },
                WinHeight = 201,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2Panel);
            T1TypeLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T1TypeLB",
                OnMouseDown = T1AbilityLBMouseDown,
                Priority = 80,
                WinLeft = 164,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 45,
                    RightOffPercent = 32,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 85,
                WinHeight = 141,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = T1AbilityLBChange,
                OnDoubleClick = Layout_UseAbility_0,
                Selected = -1,
                OnPreDrawItem = PreDrawAbilityItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T1TypeLB);
            T1SrcLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T1SrcLB",
                OnMouseDown = T1AbilityLBMouseDown,
                Priority = 40,
                WinLeft = 248,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 68,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 118,
                WinHeight = 141,
                BorderWidth = 4,
                BorderHeight = 4,
                OnChange = T1AbilityLBChange,
                OnDoubleClick = Layout_UseAbility_0,
                Selected = -1,
                OnPreDrawItem = PreDrawAbilityItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T1SrcLB);
            T1ListPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T1ListPnl",
                Priority = 100,
                WinLeft = 5,
                WinTop = 15,
                WinWidth = 366,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 15,
                    BottomOffset = 30
                },
                WinHeight = 156,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T1ListPnl);
            T1SBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "T1SBar",
                Visible = false,
                Priority = 40,
                WinLeft = 349,
                WinTop = 14,
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
                WinHeight = 141,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = T1SbarChange,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(T1SBar);
            T2SBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "T2SBar",
                Visible = false,
                Priority = 200,
                WinLeft = 349,
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
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 86,
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
            LayoutSession.EndRead(T2SBar);
            T2Text = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2Text",
                Priority = 100,
                WinLeft = 5,
                WinTop = 15,
                WinWidth = 366,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 15,
                    BottomOffset = 85
                },
                WinHeight = 87,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2Text);
            T2Leader = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2Leader",
                Priority = 1000,
                WinLeft = 5,
                WinTop = 15,
                WinWidth = 366,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 15,
                    BottomOffset = 85
                },
                WinHeight = 87,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2Leader);
            Sel4 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel4",
                Priority = 10,
                WinLeft = 157,
                WinTop = 0,
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
                    LeftOffset = 109,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel4);
            SelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SelPnl",
                Priority = 10,
                WinLeft = 0,
                WinTop = 8,
                WinWidth = 366,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
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
                WinHeight = 70,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SelPnl);
            Sel3 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel3",
                Priority = 10,
                WinLeft = 110,
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
                    LeftOffset = 110,
                    RightOffset = 110,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel3);
            Sel2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel2",
                Priority = 10,
                WinLeft = 62,
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
                    LeftOffset = 62,
                    RightOffset = 62,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel2);
            Sel1 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel1",
                Priority = 10,
                WinLeft = 15,
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
                    LeftOffset = 15,
                    RightOffset = 15,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel1);
            Sel5 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel5",
                Priority = 10,
                WinLeft = 204,
                WinTop = 0,
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
                    LeftOffset = 110,
                    RightOffset = 110,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel5);
            Sel6 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel6",
                Priority = 10,
                WinLeft = 252,
                WinTop = 0,
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
                    LeftOffset = 62,
                    RightOffset = 62,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel6);
            Sel7 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel7",
                Priority = 10,
                WinLeft = 299,
                WinTop = 0,
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
                    LeftOffset = 15,
                    RightOffset = 15,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 28,
                ILIndexLit = 29,
                ILIndexUp = 27,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52
            };
            LayoutSession.EndRead(Sel7);
            AAlignment = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AAlignment",
                Priority = 0,
                WinLeft = 6,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 31,
                AnimEnd = 37,
                WinWidth = 113,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AAlignment);
            AAlignmentLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AAlignmentLbl",
                Priority = 0,
                WinLeft = 62,
                WinTop = 80,
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
                    TopOffset = 45,
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AAlignmentLbl);
            T2Alignnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2Alignnl",
                Priority = 10,
                WinLeft = 126,
                WinTop = 109,
                WinWidth = 125,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 60,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 10,
                    BottomOffset = 5
                },
                WinHeight = 87,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2Alignnl);
            AlFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AlFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 124,
                WinHeight = 86,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AlFr);
            TypeIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TypeIcn",
                Priority = 0,
                WinLeft = 39,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 37,
                WinWidth = 39,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TypeIcn);
            TypeV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TypeV",
                Priority = 0,
                WinLeft = 58,
                WinTop = 80,
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
                    TopOffset = 45,
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(TypeV);
            T2TypePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2TypePnl",
                Priority = 10,
                WinLeft = 5,
                WinTop = 109,
                WinWidth = 116,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 68,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 10,
                    BottomOffset = 5
                },
                WinHeight = 87,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2TypePnl);
            TypeFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TypeFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 115,
                WinHeight = 86,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TypeFr);
            LdrFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LdrFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 365,
                WinHeight = 86,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LdrFr);
            SphereLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SphereLbl",
                Priority = 0,
                WinLeft = 131,
                WinTop = 66,
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
                    TopOffset = 45,
                    BottomOffset = 10
                },
                Masked = true,
                Text = "Leader's magic spheres",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SphereLbl);
            T3Panel = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3Panel",
                Visible = false,
                Priority = 900,
                WinLeft = 22,
                WinTop = 178,
                WinWidth = 376,
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
                    LeftOffset = 22,
                    RightOffset = 22,
                    TopOffset = 178,
                    BottomOffset = 38
                },
                WinHeight = 201,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 2,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3Panel);
            BP1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP1",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 10,
                WinTop = 29,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 29,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP1);
            BPLbl1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "BPLbl1",
                Visible = false,
                Priority = 1000,
                WinLeft = 15,
                WinTop = 15,
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
                    RightOffset = 13,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Backpack",
                TextColor = 2370853
            };
            LayoutSession.EndRead(BPLbl1);
            BP2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP2",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 10,
                WinTop = 71,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 71,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP2);
            BP3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP3",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 10,
                WinTop = 113,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 113,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP3);
            BP4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP4",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 10,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP4);
            BP5 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP5",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 325,
                WinTop = 29,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 4,
                    RightOffset = 10,
                    TopOffset = 29,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP5);
            BP6 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP6",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 325,
                WinTop = 71,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 4,
                    RightOffset = 10,
                    TopOffset = 71,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP6);
            BP7 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP7",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 325,
                WinTop = 113,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 4,
                    RightOffset = 10,
                    TopOffset = 113,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP7);
            BP8 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "BP8",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 325,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Regular inventory, any item can be place here."
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
                    LeftOffset = 4,
                    RightOffset = 10,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 30,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(BP8);
            BPLbl2 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "BPLbl2",
                Visible = false,
                Priority = 1000,
                WinLeft = 322,
                WinTop = 15,
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
                    LeftOffset = 15,
                    RightOffset = 13,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Backpack",
                TextColor = 2370853
            };
            LayoutSession.EndRead(BPLbl2);
            BodyLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "BodyLbl",
                Priority = 10,
                WinLeft = 164,
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
                    LeftOffset = 7,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Body Slots",
                TextColor = 2370853
            };
            LayoutSession.EndRead(BodyLbl);
            Head = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Head",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = HeadDraw,
                Priority = 500,
                WinLeft = 168,
                WinTop = 25,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Head slot",
                    ""
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Head);
            Torso = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Torso",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = TorsoDraw,
                Priority = 500,
                WinLeft = 168,
                WinTop = 84,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Torso slot"
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 84,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Torso);
            Attack = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Attack",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = AttackDraw,
                Priority = 500,
                WinLeft = 78,
                WinTop = 46,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Melee weapon slot"
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
                    LeftOffset = 78,
                    RightOffset = 0,
                    TopOffset = 46,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Attack);
            Defend = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Defend",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = DefendDraw,
                Priority = 500,
                WinLeft = 256,
                WinTop = 46,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Defensive weapon slot"
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
                    LeftOffset = 70,
                    RightOffset = 79,
                    TopOffset = 46,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Defend);
            RingL = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RingL",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = RingLDraw,
                Priority = 500,
                WinLeft = 230,
                WinTop = 95,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ring slot"
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
                    LeftOffset = 105,
                    RightOffset = 105,
                    TopOffset = 95,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 37,
                ILIndexLit = 36,
                ILIndexUp = 36,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(RingL);
            RingR = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RingR",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = RingLDraw,
                Priority = 500,
                WinLeft = 105,
                WinTop = 95,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ring slot",
                    ""
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
                    LeftOffset = 105,
                    RightOffset = 105,
                    TopOffset = 95,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 37,
                ILIndexLit = 36,
                ILIndexUp = 36,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(RingR);
            Gnd3 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd3",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 168,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ground item slot.  Moving items from here to the hero slots picks them up.  Placing items here will drop them"
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd3);
            Gnd2 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd2",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 124,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ground item slot.  Moving items from here to the hero slots picks them up.  Placing items here will drop them"
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
                    LeftOffset = 124,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd2);
            Gnd1 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd1",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 80,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ground item slot.  Moving items from here to the hero slots picks them up.  Placing items here will drop them"
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd1);
            Gnd4 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd4",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 212,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ground item slot.  Moving items from here to the hero slots picks them up.  Placing items here will drop them"
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
                    RightOffset = 123,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd4);
            Gnd5 = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "Gnd5",
                OnMouseDown = Gnd3MouseDown,
                OnDraw = Gnd1Draw,
                Priority = 1000,
                WinLeft = 256,
                WinTop = 155,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Ground item slot.  Moving items from here to the hero slots picks them up.  Placing items here will drop them"
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
                    LeftOffset = 77,
                    RightOffset = 79,
                    TopOffset = 155,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 41,
                Checked = false,
                OnChange = Gnd1Change,
                RadioIndex = 1
            };
            LayoutSession.EndRead(Gnd5);
            GndLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "GndLbl",
                Priority = 1000,
                WinLeft = 141,
                WinTop = 143,
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
                    LeftOffset = 7,
                    RightOffset = 7,
                    TopOffset = 143,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Items on the Ground",
                TextColor = 2370853
            };
            LayoutSession.EndRead(GndLbl);
            SlotBack = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SlotBack",
                Priority = 0,
                WinLeft = 9,
                WinTop = 2,
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
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 40,
                AnimEnd = 0,
                WinWidth = 358,
                WinHeight = 193,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SlotBack);
            DisbandBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DisbandBtn",
                Priority = 250,
                WinLeft = 89,
                WinTop = 380,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 32,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 9,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Disband",
                TextColor = 89,
                TextSelColor = 16777215,
                WinWidth = 119,
                OnClick = Layout_DisbandBtnClick_4
            };
            LayoutSession.EndRead(DisbandBtn);
            ItemUsePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ItemUsePnl",
                Visible = false,
                Priority = 500,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 376,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 201,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 3,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ItemUsePnl);
            ItemUseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ItemUseBtn",
                Priority = 100,
                WinLeft = 83,
                WinTop = 103,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Uses selected item"
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
                    RightOffset = 9,
                    TopOffset = 0,
                    BottomOffset = 76
                },
                Masked = true,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 210,
                OnClick = UseItemClick
            };
            LayoutSession.EndRead(ItemUseBtn);
            ItemPic = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ItemPic",
                Priority = 0,
                WinLeft = 9,
                WinTop = 2,
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
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 40,
                AnimEnd = 0,
                WinWidth = 358,
                WinHeight = 193,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ItemPic);
            UnitMainPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitMainPnl",
                Priority = 0,
                WinLeft = 21,
                WinTop = 22,
                WinWidth = 378,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 143,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitMainPnl);
            UnitName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UnitName",
                Priority = 1500,
                WinLeft = 189,
                WinTop = 8,
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
                    LeftOffset = 130,
                    RightOffset = 0,
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 13299411
            };
            LayoutSession.EndRead(UnitName);
            UnitFrRight = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UnitFrRight",
                Priority = 200,
                WinLeft = 260,
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
                AnimStart = 0,
                CurrentImage = 45,
                AnimEnd = 6,
                WinWidth = 118,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UnitFrRight);
            UAttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UAttackIcon",
                Priority = 0,
                WinLeft = 5,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Attack is the ability of a unit to hit a target with a melee strike."
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UAttackIcon);
            UDefenseIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UDefenseIcon",
                Priority = 0,
                WinLeft = 5,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Defense is the ability of a unit to avoid being hit by physical attacks."
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UDefenseIcon);
            UMovesIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UMovesIcon",
                Priority = 0,
                WinLeft = 0,
                WinTop = 45,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Moves are the range a unit can move in a turn (Current/Maximum)."
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
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UMovesIcon);
            UDamageIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UDamageIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Damage is the maximum strength of a unit's melee attack."
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UDamageIcon);
            UResistanceIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UResistanceIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 22,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Resistance is the ability of a unit to repel non-physical attacks."
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
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UResistanceIcon);
            UHitsIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UHitsIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 45,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Hitpoints are the amount of damage a unit can withstand before dying (Current/Maximum)."
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
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 6,
                WinWidth = 15,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UHitsIcon);
            UAttack = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UAttack",
                Priority = 0,
                WinLeft = 22,
                WinTop = 3,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Attack is the ability of a unit to hit a target with a melee strike."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 57,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UAttack);
            UDefense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDefense",
                Priority = 0,
                WinLeft = 22,
                WinTop = 25,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Defense is the ability of a unit to avoid being hit by physical attacks."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 57,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UDefense);
            UMoves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UMoves",
                Priority = 0,
                WinLeft = 22,
                WinTop = 47,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Moves are the range a unit can move in a turn (Current/Maximum)."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 57,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 47,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UMoves);
            UDamage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDamage",
                Priority = 0,
                WinLeft = 25,
                WinTop = 3,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Damage is the maximum strength of a unit's melee attack."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 6,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UDamage);
            UResistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UResistance",
                Priority = 0,
                WinLeft = 25,
                WinTop = 25,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Resistance is the ability of a unit to repel non-physical attacks."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 6,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UResistance);
            UHits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UHits",
                Priority = 0,
                WinLeft = 25,
                WinTop = 47,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Hitpoints are the amount of damage a unit can withstand before dying (Current/Maximum)."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 6,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 47,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UHits);
            UStatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatPnl",
                Priority = 0,
                WinLeft = 117,
                WinTop = 28,
                WinWidth = 144,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                WinHeight = 86,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatPnl);
            UStatP1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatP1",
                Priority = 0,
                WinLeft = 28,
                WinTop = 0,
                WinWidth = 44,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 86,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatP1);
            UStatP2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatP2",
                Priority = 0,
                WinLeft = 93,
                WinTop = 0,
                WinWidth = 51,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 65,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 86,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatP2);
            UnitFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 7,
                WinWidth = 50,
                Masked = false,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 7,
                    BottomOffset = 0
                },
                WinHeight = 47,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitFr);
            Level = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Level",
                Priority = 1000000,
                WinLeft = 59,
                WinTop = 18,
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
                    LeftOffset = 7,
                    RightOffset = 7,
                    TopOffset = 18,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 1514524
            };
            LayoutSession.EndRead(Level);
            UnitLine1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UnitLine1",
                Priority = 1000,
                WinLeft = 118,
                WinTop = 0,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 12,
                WinWidth = 142,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UnitLine1);
            UnitLine2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UnitLine2",
                Priority = 1000,
                WinLeft = 118,
                WinTop = 116,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 12,
                WinWidth = 142,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UnitLine2);
            FrLSh = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FrLSh",
                OnPreDraw = FrLShPreDraw,
                Priority = 100,
                WinLeft = 1,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 46,
                AnimEnd = 6,
                WinWidth = 118,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FrLSh);
            FrRSh = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FrRSh",
                OnPreDraw = FrRShPreDraw,
                Priority = 100,
                WinLeft = 260,
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
                Masked = true,
                AnimStart = 0,
                CurrentImage = 46,
                AnimEnd = 6,
                WinWidth = 118,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FrRSh);
            RightFramePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightFramePnl",
                Priority = 0,
                WinLeft = 260,
                WinTop = 0,
                WinWidth = 118,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                WinHeight = 143,
                ILIndexFrame = 113,
                ILIndexTexture = 7,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightFramePnl);
            MoraleLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MoraleLbl",
                Priority = 1500,
                WinLeft = 189,
                WinTop = 136,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Morale modifiers: High [-], Good[-], Okay [-], Poor [Def-1/Res-1], Terrible [Def-2/Res-2]"
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
                    LeftOffset = 130,
                    RightOffset = 0,
                    TopOffset = 8,
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 13299411
            };
            LayoutSession.EndRead(MoraleLbl);
            UpkeepIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UpkeepIcon",
                Priority = 0,
                WinLeft = 5,
                WinTop = 64,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Upkeep is the cost to maintain the unit, in Gold or Mana, per turn."
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 64,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 51,
                AnimEnd = 6,
                WinWidth = 19,
                WinHeight = 21,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UpkeepIcon);
            UpKeep = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpKeep",
                Priority = 0,
                WinLeft = 22,
                WinTop = 68,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Upkeep is the cost to maintain the unit, in Gold or Mana, per turn."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 57,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 68,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 16777215
            };
            LayoutSession.EndRead(UpKeep);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 380,
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
                WinWidth = 378,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            CeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltLineL",
                Priority = 100,
                WinLeft = 27,
                WinTop = 380,
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
                    RightOffset = 27,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltLineL);
            GroundL = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GroundL",
                Visible = false,
                Priority = 0,
                WinLeft = 65,
                WinTop = 170,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays help on this window"
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
                    RightOffset = 36,
                    TopOffset = 4,
                    BottomOffset = 17
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 33,
                ILIndexLit = 32,
                ILIndexUp = 32,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = GroundLClick
            };
            LayoutSession.EndRead(GroundL);
            GroundR = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GroundR",
                Visible = false,
                Priority = 0,
                WinLeft = 300,
                WinTop = 170,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays help on this window"
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
                    LeftOffset = 654,
                    RightOffset = 63,
                    TopOffset = 4,
                    BottomOffset = 17
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 35,
                ILIndexLit = 34,
                ILIndexUp = 34,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 13,
                OnClick = GroundRClick
            };
            LayoutSession.EndRead(GroundR);
            RelationIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RelationIcn",
                Priority = 0,
                WinLeft = 32,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 11,
                AnimEnd = 37,
                WinWidth = 51,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RelationIcn);
            RelationV = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RelationV",
                Priority = 0,
                WinLeft = 57,
                WinTop = 80,
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
                    TopOffset = 45,
                    BottomOffset = 7
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(RelationV);
            T2RelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2RelPnl",
                Priority = 10,
                WinLeft = 256,
                WinTop = 109,
                WinWidth = 115,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 10,
                    BottomOffset = 5
                },
                WinHeight = 87,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2RelPnl);
            RelFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RelFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 114,
                WinHeight = 86,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RelFr);
            RelRing = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RelRing",
                Priority = 0,
                WinLeft = 37,
                WinTop = 10,
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
                    LeftOffset = 37,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 20,
                AnimEnd = 37,
                WinWidth = 39,
                WinHeight = 36,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RelRing);
            RelationLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RelationLbl",
                Priority = 0,
                WinLeft = 27,
                WinTop = 56,
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
                    TopOffset = 30,
                    BottomOffset = 20
                },
                Masked = true,
                Text = "Race Relation",
                TextColor = 9415072
            };
            LayoutSession.EndRead(RelationLbl);
            AlignLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AlignLbl",
                Priority = 0,
                WinLeft = 42,
                WinTop = 56,
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
                    TopOffset = 45,
                    BottomOffset = 20
                },
                Masked = true,
                Text = "Alignment",
                TextColor = 9415072
            };
            LayoutSession.EndRead(AlignLbl);
            TypeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TypeLbl",
                Priority = 0,
                WinLeft = 48,
                WinTop = 56,
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
                    TopOffset = 45,
                    BottomOffset = 20
                },
                Masked = true,
                Text = "Type",
                TextColor = 9415072
            };
            LayoutSession.EndRead(TypeLbl);
            XP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "XP",
                Priority = 1000000,
                WinLeft = 59,
                WinTop = 30,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Indicates combat experience needed for next level."
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
                    LeftOffset = 7,
                    RightOffset = 7,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 1514524
            };
            LayoutSession.EndRead(XP);
            PNext = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PNext",
                Priority = 50000,
                WinLeft = 331,
                WinTop = 109,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects the next unit in the party."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 85,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 35,
                    TopOffset = 0,
                    BottomOffset = 25
                },
                Masked = true,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 26,
                ILIndexLit = 25,
                ILIndexUp = 25,
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 12,
                OnClick = Layout_PNextClick_5
            };
            LayoutSession.EndRead(PNext);
            PPrev = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PPrev",
                Priority = 50000,
                WinLeft = 296,
                WinTop = 109,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects the previous unit in the party."
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
                    LeftOffset = 1,
                    RightOffset = 70,
                    TopOffset = 0,
                    BottomOffset = 25
                },
                Masked = true,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 24,
                ILIndexLit = 23,
                ILIndexUp = 23,
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 12,
                OnClick = Layout_PPrevClick_6
            };
            LayoutSession.EndRead(PPrev);
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
            UnitFrameLeft.AOWWindow = UnitMainPnl;
            UnitTab.AOWWindow = UnitWin;
            Close.AOWWindow = UnitWin;
            Title.AOWWindow = UnitWin;
            Help.AOWWindow = UnitWin;
            TitleIcon.AOWWindow = UnitWin;
            BCloseBtn.AOWWindow = UnitWin;
            CeltLineR.AOWWindow = UnitWin;
            T2Memo.AOWWindow = T2Text;
            T2Memo.VScrollBar = T2SBar;
            ManaIcon.AOWWindow = UStatPnl;
            Mana.AOWWindow = UStatP2;
            T1AbilityLB.AOWWindow = T1ListPnl;
            T1UseBut.AOWWindow = T1Panel;
            T1SortName.AOWWindow = T1ListPnl;
            T1SortType.AOWWindow = T1ListPnl;
            T1SortSource.AOWWindow = T1ListPnl;
            T1UseMsg.AOWWindow = T1Panel;
            T1Panel.AOWWindow = UnitWin;
            T1Panel.TabPanel = UnitTab;
            T2Panel.AOWWindow = UnitWin;
            T2Panel.TabPanel = UnitTab;
            T1TypeLB.AOWWindow = T1ListPnl;
            T1SrcLB.AOWWindow = T1ListPnl;
            T1SrcLB.VScrollBar = T1SBar;
            T1ListPnl.AOWWindow = T1Panel;
            T1SBar.AOWWindow = T1ListPnl;
            T2SBar.AOWWindow = T2Text;
            T2Text.AOWWindow = T2Panel;
            T2Leader.AOWWindow = T2Panel;
            Sel4.AOWWindow = SelPnl;
            SelPnl.AOWWindow = T2Leader;
            Sel3.AOWWindow = SelPnl;
            Sel2.AOWWindow = SelPnl;
            Sel1.AOWWindow = SelPnl;
            Sel5.AOWWindow = SelPnl;
            Sel6.AOWWindow = SelPnl;
            Sel7.AOWWindow = SelPnl;
            AAlignment.AOWWindow = T2Alignnl;
            AAlignmentLbl.AOWWindow = T2Alignnl;
            T2Alignnl.AOWWindow = T2Panel;
            AlFr.AOWWindow = T2Alignnl;
            TypeIcn.AOWWindow = T2TypePnl;
            TypeV.AOWWindow = T2TypePnl;
            T2TypePnl.AOWWindow = T2Panel;
            TypeFr.AOWWindow = T2TypePnl;
            LdrFr.AOWWindow = T2Leader;
            SphereLbl.AOWWindow = T2Leader;
            T3Panel.AOWWindow = UnitWin;
            T3Panel.TabPanel = UnitTab;
            BP1.AOWWindow = T3Panel;
            BPLbl1.AOWWindow = T3Panel;
            BP2.AOWWindow = T3Panel;
            BP3.AOWWindow = T3Panel;
            BP4.AOWWindow = T3Panel;
            BP5.AOWWindow = T3Panel;
            BP6.AOWWindow = T3Panel;
            BP7.AOWWindow = T3Panel;
            BP8.AOWWindow = T3Panel;
            BPLbl2.AOWWindow = T3Panel;
            BodyLbl.AOWWindow = T3Panel;
            Head.AOWWindow = T3Panel;
            Torso.AOWWindow = T3Panel;
            Attack.AOWWindow = T3Panel;
            Defend.AOWWindow = T3Panel;
            RingL.AOWWindow = T3Panel;
            RingR.AOWWindow = T3Panel;
            Gnd3.AOWWindow = T3Panel;
            Gnd2.AOWWindow = T3Panel;
            Gnd1.AOWWindow = T3Panel;
            Gnd4.AOWWindow = T3Panel;
            Gnd5.AOWWindow = T3Panel;
            GndLbl.AOWWindow = T3Panel;
            SlotBack.AOWWindow = T3Panel;
            DisbandBtn.AOWWindow = UnitWin;
            ItemUsePnl.AOWWindow = T3Panel;
            ItemUseBtn.AOWWindow = ItemUsePnl;
            ItemPic.AOWWindow = ItemUsePnl;
            UnitMainPnl.AOWWindow = UnitWin;
            UnitName.AOWWindow = UnitMainPnl;
            UnitFrRight.AOWWindow = UnitMainPnl;
            UAttackIcon.AOWWindow = UStatPnl;
            UDefenseIcon.AOWWindow = UStatPnl;
            UMovesIcon.AOWWindow = UStatPnl;
            UDamageIcon.AOWWindow = UStatPnl;
            UResistanceIcon.AOWWindow = UStatPnl;
            UHitsIcon.AOWWindow = UStatPnl;
            UAttack.AOWWindow = UStatP1;
            UDefense.AOWWindow = UStatP1;
            UMoves.AOWWindow = UStatP1;
            UDamage.AOWWindow = UStatP2;
            UResistance.AOWWindow = UStatP2;
            UHits.AOWWindow = UStatP2;
            UStatPnl.AOWWindow = UnitMainPnl;
            UStatP1.AOWWindow = UStatPnl;
            UStatP2.AOWWindow = UStatPnl;
            Level.AOWWindow = RightFramePnl;
            UnitLine1.AOWWindow = UnitMainPnl;
            UnitLine2.AOWWindow = UnitMainPnl;
            FrLSh.AOWWindow = UnitMainPnl;
            FrRSh.AOWWindow = UnitMainPnl;
            RightFramePnl.AOWWindow = UnitMainPnl;
            MoraleLbl.AOWWindow = UnitMainPnl;
            UpkeepIcon.AOWWindow = UStatPnl;
            UpKeep.AOWWindow = UStatP1;
            BCeltL.AOWWindow = UnitWin;
            CeltLineL.AOWWindow = UnitWin;
            GroundL.AOWWindow = T3Panel;
            GroundR.AOWWindow = T3Panel;
            RelationIcn.AOWWindow = T2RelPnl;
            RelationV.AOWWindow = T2RelPnl;
            T2RelPnl.AOWWindow = T2Panel;
            RelFr.AOWWindow = T2RelPnl;
            RelRing.AOWWindow = T2RelPnl;
            RelationLbl.AOWWindow = T2RelPnl;
            AlignLbl.AOWWindow = T2Alignnl;
            TypeLbl.AOWWindow = T2TypePnl;
            XP.AOWWindow = RightFramePnl;
            PNext.AOWWindow = UnitMainPnl;
            PPrev.AOWWindow = UnitMainPnl;
            Loaded();
            UnitWin.Loaded();
            UnitFrameLeft.Loaded();
            UnitTab.Loaded();
            Close.Loaded();
            Title.Loaded();
            Help.Loaded();
            TitleIcon.Loaded();
            BCloseBtn.Loaded();
            CeltLineR.Loaded();
            T2Memo.Loaded();
            ManaIcon.Loaded();
            Mana.Loaded();
            T1AbilityLB.Loaded();
            T1UseBut.Loaded();
            T1SortName.Loaded();
            T1SortType.Loaded();
            T1SortSource.Loaded();
            T1UseMsg.Loaded();
            T1Panel.Loaded();
            T2Panel.Loaded();
            T1TypeLB.Loaded();
            T1SrcLB.Loaded();
            T1ListPnl.Loaded();
            T1SBar.Loaded();
            T2SBar.Loaded();
            T2Text.Loaded();
            T2Leader.Loaded();
            Sel4.Loaded();
            SelPnl.Loaded();
            Sel3.Loaded();
            Sel2.Loaded();
            Sel1.Loaded();
            Sel5.Loaded();
            Sel6.Loaded();
            Sel7.Loaded();
            AAlignment.Loaded();
            AAlignmentLbl.Loaded();
            T2Alignnl.Loaded();
            AlFr.Loaded();
            TypeIcn.Loaded();
            TypeV.Loaded();
            T2TypePnl.Loaded();
            TypeFr.Loaded();
            LdrFr.Loaded();
            SphereLbl.Loaded();
            T3Panel.Loaded();
            BP1.Loaded();
            BPLbl1.Loaded();
            BP2.Loaded();
            BP3.Loaded();
            BP4.Loaded();
            BP5.Loaded();
            BP6.Loaded();
            BP7.Loaded();
            BP8.Loaded();
            BPLbl2.Loaded();
            BodyLbl.Loaded();
            Head.Loaded();
            Torso.Loaded();
            Attack.Loaded();
            Defend.Loaded();
            RingL.Loaded();
            RingR.Loaded();
            Gnd3.Loaded();
            Gnd2.Loaded();
            Gnd1.Loaded();
            Gnd4.Loaded();
            Gnd5.Loaded();
            GndLbl.Loaded();
            SlotBack.Loaded();
            DisbandBtn.Loaded();
            ItemUsePnl.Loaded();
            ItemUseBtn.Loaded();
            ItemPic.Loaded();
            UnitMainPnl.Loaded();
            UnitName.Loaded();
            UnitFrRight.Loaded();
            UAttackIcon.Loaded();
            UDefenseIcon.Loaded();
            UMovesIcon.Loaded();
            UDamageIcon.Loaded();
            UResistanceIcon.Loaded();
            UHitsIcon.Loaded();
            UAttack.Loaded();
            UDefense.Loaded();
            UMoves.Loaded();
            UDamage.Loaded();
            UResistance.Loaded();
            UHits.Loaded();
            UStatPnl.Loaded();
            UStatP1.Loaded();
            UStatP2.Loaded();
            UnitFr.Loaded();
            Level.Loaded();
            UnitLine1.Loaded();
            UnitLine2.Loaded();
            FrLSh.Loaded();
            FrRSh.Loaded();
            RightFramePnl.Loaded();
            MoraleLbl.Loaded();
            UpkeepIcon.Loaded();
            UpKeep.Loaded();
            BCeltL.Loaded();
            CeltLineL.Loaded();
            GroundL.Loaded();
            GroundR.Loaded();
            RelationIcn.Loaded();
            RelationV.Loaded();
            T2RelPnl.Loaded();
            RelFr.Loaded();
            RelRing.Loaded();
            RelationLbl.Loaded();
            AlignLbl.Loaded();
            TypeLbl.Loaded();
            XP.Loaded();
            PNext.Loaded();
            PPrev.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            UnitWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            UnitWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UnitWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitFrameLeft.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            UnitTab.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            UnitTab.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            BCloseBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCloseBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CeltLineR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            T2Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            T2Memo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2Memo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ManaIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ManaIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            Mana.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Mana.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            T1AbilityLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1AbilityLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T1AbilityLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T1AbilityLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => T1AbilityLB.PopupMenu = module.BannerPopup);
            T1UseBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T1UseBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T1SortName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1SortName.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T1SortType.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1SortType.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T1SortSource.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1SortSource.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T1UseMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            T1Panel.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2Panel.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T1TypeLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1TypeLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T1TypeLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T1TypeLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => T1TypeLB.PopupMenu = module.BannerPopup);
            T1SrcLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T1SrcLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T1SrcLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T1SrcLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => T1SrcLB.PopupMenu = module.BannerPopup);
            T1ListPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T1ListPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T1SBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T1SBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T1SBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T1SBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2Text.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2Text.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2Leader.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2Leader.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Sel4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel6.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel7.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AAlignment.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            AAlignmentLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2Alignnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2Alignnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AlFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TypeIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            TypeV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2TypePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2TypePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TypeFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LdrFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SphereLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T3Panel.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            BP1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BPLbl1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            BP2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP5.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP6.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP6.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP7.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP7.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BP8.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BP8.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            BPLbl2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            BodyLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Head.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Head.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Torso.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Torso.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Attack.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Attack.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Defend.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Defend.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            RingL.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RingL.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            RingR.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RingR.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            Gnd5.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Gnd5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            GndLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SlotBack.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            DisbandBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisbandBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ItemUsePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ItemUseBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ItemUseBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ItemUseBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ItemPic.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            UnitMainPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UnitFrRight.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            UAttackIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UAttackIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UDefenseIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDefenseIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UMovesIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UMovesIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            UDamageIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDamageIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UResistanceIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UResistanceIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UHitsIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UHitsIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UAttack.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UAttack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UDefense.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDefense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UMoves.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UMoves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UDamage.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDamage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UResistance.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UResistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UHits.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UHits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UnitFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Level.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UnitLine1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            UnitLine2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            FrLSh.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            FrRSh.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            RightFramePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MoraleLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            MoraleLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UpkeepIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UpkeepIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TBWin;
            UpKeep.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UpKeep.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            GroundL.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GroundL.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GroundR.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            GroundR.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            RelationIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            RelationV.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2RelPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2RelPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RelFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RelRing.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitWin;
            RelationLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AlignLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TypeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            XP.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            XP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PNext.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PNext.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PNext.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            PPrev.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            PPrev.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PPrev.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_UseAbility_0(TObject p0)
        {
            UseAbility();
        }

        private void Layout_T1SortNameClick_1(TObject p0)
        {
            T1SortNameClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T1SortTypeClick_2(TObject p0)
        {
            T1SortTypeClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T1SortSourceClick_3(TObject p0)
        {
            T1SortSourceClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_DisbandBtnClick_4(TObject p0)
        {
            DisbandBtnClick();
        }

        private void Layout_PNextClick_5(TObject p0)
        {
            PNextClick();
        }

        private void Layout_PPrevClick_6(TObject p0)
        {
            PPrevClick();
        }
    }
}
