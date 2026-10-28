// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTCUnit
{
    public partial class TTCUnit
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TCUnit";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TCUnitWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "TCUnitWin",
                Visible = false,
                Priority = 10,
                WinLeft = 490,
                WinTop = 170,
                WinWidth = 150,
                Masked = false,
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
                    TopOffset = 170,
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
                OnDrag = Layout_TCUnitWinResize_0,
                OnResize = Layout_TCUnitWinResize_0,
                MinWidth = 150,
                MinHeight = 262,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 262,
                Dockable = false
            };
            LayoutSession.EndRead(TCUnitWin);
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
                CurrentImage = 3,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SIcon);
            Min = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Min",
                Priority = 0,
                WinLeft = 117,
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
                WinLeft = 131,
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
                OnClick = Layout_CloseClick_2
            };
            LayoutSession.EndRead(Close);
            IWTitle = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IWTitle",
                Priority = 0,
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
                Text = "Unit",
                TextColor = 0
            };
            LayoutSession.EndRead(IWTitle);
            UAttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UAttackIcon",
                Priority = 0,
                WinLeft = 10,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Attack is the ability of a unit to hit a target with a melee strike."
                },
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
                    LeftOffset = 10,
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
                WinLeft = 10,
                WinTop = 18,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Defense is the ability of a unit to avoid being hit by physical attacks."
                },
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 18,
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
                WinLeft = 6,
                WinTop = 35,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Moves are the range a unit can move in a turn (Current/Maximum)."
                },
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 35,
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
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
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
                WinTop = 18,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Resistance is the ability of a unit to repel non-physical attacks."
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 18,
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
                WinTop = 35,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Hitpoints are the amount of damage a unit can withstand before dying (Current/Maximum)."
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 35,
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
                WinLeft = 18,
                WinTop = 6,
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
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UAttack);
            UDefense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDefense",
                Priority = 0,
                WinLeft = 10,
                WinTop = 23,
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
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UDefense);
            UMoves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UMoves",
                Priority = 0,
                WinLeft = 4,
                WinTop = 40,
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
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UMoves);
            UDamage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDamage",
                Priority = 0,
                WinLeft = 15,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UDamage);
            UResistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UResistance",
                Priority = 0,
                WinLeft = 15,
                WinTop = 23,
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
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UResistance);
            UHits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UHits",
                Priority = 0,
                WinLeft = 1,
                WinTop = 40,
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
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(UHits);
            UnitName = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "UnitName",
                Priority = 0,
                WinLeft = 48,
                WinTop = 0,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 89,
                WinHeight = 61,
                BorderWidth = 5,
                BorderHeight = 3,
                TextColor = 9415072,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(0),
                Editable = true,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 13,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(UnitName);
            UStatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 60,
                WinWidth = 136,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                WinHeight = 54,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatPnl);
            UAbilLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "UAbilLB",
                OnMouseDown = UAbilLBMouseDown,
                OnMouseMove = UAbilLBMouseMove,
                OnUpdate = Layout_UAbilLBUpdate_3,
                Priority = 0,
                WinLeft = 1,
                WinTop = 113,
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
                    TopOffset = 113,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 136,
                WinHeight = 122,
                BorderWidth = 20,
                BorderHeight = 3,
                Selected = -1,
                OnPreDrawItem = UAbilLBPreDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(UAbilLB);
            AbilNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AbilNameLbl",
                Priority = 100,
                WinLeft = 32,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AbilNameLbl);
            TCUnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TCUnitPnl",
                Priority = 0,
                WinLeft = 6,
                WinTop = 20,
                WinWidth = 137,
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
                    LeftOffset = 6,
                    RightOffset = 7,
                    TopOffset = 20,
                    BottomOffset = 6
                },
                WinHeight = 236,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TCUnitPnl);
            AOWImage1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AOWImage1",
                Visible = false,
                Priority = 0,
                WinLeft = 5,
                WinTop = 20,
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
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 71,
                WinWidth = 52,
                WinHeight = 63,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(AOWImage1);
            AUnitName = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "AUnitName",
                Visible = false,
                Priority = 0,
                WinLeft = 62,
                WinTop = 18,
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
                    LeftOffset = 62,
                    RightOffset = 1,
                    TopOffset = 18,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 79,
                WinHeight = 60,
                BorderWidth = 5,
                BorderHeight = 2,
                TextColor = 9415072,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 19,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(AUnitName);
            USelAbilLb = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "USelAbilLb",
                Priority = 0,
                WinLeft = 1,
                WinTop = 74,
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
                    TopOffset = 74,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 136,
                WinHeight = 87,
                BorderWidth = 20,
                BorderHeight = 3,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(USelAbilLb);
            TCAbilPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TCAbilPnl",
                Visible = false,
                Priority = 0,
                WinLeft = 4,
                WinTop = 20,
                WinWidth = 142,
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
                    RightOffset = 4,
                    TopOffset = 20,
                    BottomOffset = 3
                },
                WinHeight = 239,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TCAbilPnl);
            AbilAtt = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AbilAtt",
                Priority = 0,
                WinLeft = 5,
                WinTop = 1,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
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
            LayoutSession.EndRead(AbilAtt);
            AbilAttLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AbilAttLbl",
                Priority = 0,
                WinLeft = 25,
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
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AbilAttLbl);
            AbilStatBox = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AbilStatBox",
                Priority = 0,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 42,
                Masked = true,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                WinHeight = 39,
                ILIndexFrame = 113,
                ILIndexTexture = 19,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AbilStatBox);
            AbilStat = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AbilStat",
                Priority = 0,
                WinLeft = 42,
                WinTop = 20,
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
                    LeftOffset = 42,
                    RightOffset = 1,
                    TopOffset = 20,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 94,
                WinHeight = 39,
                BorderWidth = 4,
                BorderHeight = 3,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 16,
                ItemHeight = 11,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(AbilStat);
            AbilDam = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AbilDam",
                Priority = 0,
                WinLeft = 5,
                WinTop = 18,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 18,
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
            LayoutSession.EndRead(AbilDam);
            AbilDamLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AbilDamLbl",
                Priority = 0,
                WinLeft = 25,
                WinTop = 23,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 25,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AbilDamLbl);
            TargetLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TargetLbl",
                Priority = 100,
                WinLeft = 57,
                WinTop = 62,
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
                    TopOffset = 62,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Target",
                TextColor = 0
            };
            LayoutSession.EndRead(TargetLbl);
            TargetULbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TargetULbl",
                Priority = 100,
                WinLeft = 13,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(TargetULbl);
            TargetSelPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TargetSelPnl",
                Visible = false,
                Priority = 1000,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 142,
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
                    TopOffset = 0,
                    BottomOffset = 3
                },
                WinHeight = 236,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TargetSelPnl);
            NoTargetPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NoTargetPnl",
                Priority = 10,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 142,
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
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 239,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NoTargetPnl);
            TargetMessage = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "TargetMessage",
                Priority = 0,
                WinLeft = 1,
                WinTop = 200,
                CaptureInput = false,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 90,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 140,
                WinHeight = 38,
                LayoutItems = new LayoutStrings()
                {
                    "No Target Unit Selected"
                },
                BorderWidth = 5,
                BorderHeight = 2,
                TextColor = 9415072,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(0),
                Editable = true,
                ILIndexFrame = 121,
                ILIndexTexture = 17,
                ItemHeight = 16,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(TargetMessage);
            UnitHits = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "UnitHits",
                Priority = 20,
                WinLeft = 1,
                WinTop = 53,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Hitpoints are the amount of damage a unit can withstand before dying."
                },
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 56,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 11,
                    TopOffset = 53,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 50,
                WinHeight = 8,
                Min = 0,
                Max = 6,
                Position = 1,
                ILIndexFrame = 113,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(UnitHits);
            UnitMana = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "UnitMana",
                Priority = 20,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Channeling is the amount of mana the Hero can pull from their Leader's pool."
                },
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 50,
                WinHeight = 8,
                Min = 0,
                Max = 6,
                Position = 1,
                ILIndexFrame = 113,
                ILIndexTNormal = 4,
                ILIndexTSelected = 6
            };
            LayoutSession.EndRead(UnitMana);
            CelticCross = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CelticCross",
                Priority = 0,
                WinLeft = -1,
                WinTop = 61,
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
                    BottomOffPercent = 17,
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 13,
                AnimEnd = 1,
                WinWidth = 144,
                WinHeight = 138,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CelticCross);
            UStatP1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatP1",
                Priority = 0,
                WinLeft = 27,
                WinTop = 0,
                WinWidth = 42,
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
                WinHeight = 54,
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
                WinLeft = 91,
                WinTop = 0,
                WinWidth = 36,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 17,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 54,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatP2);
            AAttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AAttackIcon",
                Priority = 0,
                WinLeft = 10,
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
                    LeftOffset = 10,
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
            LayoutSession.EndRead(AAttackIcon);
            ADefenseIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ADefenseIcon",
                Priority = 0,
                WinLeft = 10,
                WinTop = 18,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 18,
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
            LayoutSession.EndRead(ADefenseIcon);
            AMovesIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AMovesIcon",
                Priority = 0,
                WinLeft = 6,
                WinTop = 35,
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
                    LeftOffset = 6,
                    RightOffset = 0,
                    TopOffset = 35,
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
            LayoutSession.EndRead(AMovesIcon);
            ADamageIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ADamageIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 1,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
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
            LayoutSession.EndRead(ADamageIcon);
            AResistanceIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AResistanceIcon",
                Priority = 0,
                WinLeft = 74,
                WinTop = 18,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 18,
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
            LayoutSession.EndRead(AResistanceIcon);
            AHitsIcons = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "AHitsIcons",
                Priority = 0,
                WinLeft = 74,
                WinTop = 35,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 35,
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
            LayoutSession.EndRead(AHitsIcons);
            AAttack = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AAttack",
                Priority = 0,
                WinLeft = 18,
                WinTop = 6,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AAttack);
            ADefense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ADefense",
                Priority = 0,
                WinLeft = 10,
                WinTop = 23,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(ADefense);
            AMoves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AMoves",
                Priority = 0,
                WinLeft = 4,
                WinTop = 40,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AMoves);
            ADamage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ADamage",
                Priority = 0,
                WinLeft = 15,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(ADamage);
            AResistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AResistance",
                Priority = 0,
                WinLeft = 15,
                WinTop = 23,
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
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AResistance);
            AHits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AHits",
                Priority = 0,
                WinLeft = 1,
                WinTop = 40,
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
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(AHits);
            AStatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AStatPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 20,
                WinWidth = 136,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                WinHeight = 55,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AStatPnl);
            AStatP1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AStatP1",
                Priority = 0,
                WinLeft = 27,
                WinTop = 0,
                WinWidth = 42,
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
                WinHeight = 55,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AStatP1);
            AStatP2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AStatP2",
                Priority = 0,
                WinLeft = 96,
                WinTop = 0,
                WinWidth = 36,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahNone,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 55,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 17,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 55,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AStatP2);
            Center = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Center",
                Priority = 0,
                WinLeft = 103,
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
            AbilSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AbilSB",
                Visible = false,
                Priority = 0,
                WinLeft = 119,
                WinTop = 20,
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
                WinHeight = 39,
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
            LayoutSession.EndRead(AbilSB);
            UAbilSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "UAbilSB",
                Priority = 0,
                WinLeft = 120,
                WinTop = 113,
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
                LargeChange = 5,
                Position = 0,
                WinHeight = 122,
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
            LayoutSession.EndRead(UAbilSB);
            AOWVScrollBar1 = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AOWVScrollBar1",
                Priority = 0,
                WinLeft = 120,
                WinTop = 74,
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
                Max = 4,
                SmallChange = 1,
                LargeChange = 3,
                Position = 0,
                WinHeight = 87,
                WinWidth = 17,
                NumberVisible = 3,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(AOWVScrollBar1);
            AbilFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AbilFr",
                Priority = 0,
                WinLeft = 2,
                WinTop = 0,
                WinWidth = 137,
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
                    LeftOffset = 2,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 60,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AbilFr);
            ANameFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ANameFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ANameFr);
            TargerFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TargerFr",
                Priority = 0,
                WinLeft = 2,
                WinTop = 74,
                WinWidth = 137,
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
                    LeftOffset = 2,
                    RightOffset = 3,
                    TopOffset = 74,
                    BottomOffset = 0
                },
                WinHeight = 162,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TargerFr);
            TNamePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TNamePnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 136,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TNamePnl);
            UnitFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitFr",
                OnMouseDown = HeroSMapMouseDown,
                Priority = 30,
                WinLeft = 1,
                WinTop = 2,
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
                    TopOffset = 2,
                    BottomOffset = 0
                },
                WinHeight = 59,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitFr);
            UnitBox = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "UnitBox",
                OnMouseDown = HeroSMapMouseDown,
                OnDraw = TCUnitPnlDrawBack,
                Priority = 10,
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
                WinWidth = 45,
                WinHeight = 60
            };
            LayoutSession.EndRead(UnitBox);
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
            SIcon.AOWWindow = TCUnitWin;
            Min.AOWWindow = TCUnitWin;
            Close.AOWWindow = TCUnitWin;
            IWTitle.AOWWindow = TCUnitWin;
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
            UnitName.AOWWindow = TCUnitPnl;
            UStatPnl.AOWWindow = TCUnitPnl;
            UAbilLB.AOWWindow = TCUnitPnl;
            UAbilLB.VScrollBar = UAbilSB;
            AbilNameLbl.AOWWindow = ANameFr;
            TCUnitPnl.AOWWindow = TCUnitWin;
            AOWImage1.AOWWindow = TCAbilPnl;
            AUnitName.AOWWindow = TCAbilPnl;
            USelAbilLb.AOWWindow = TargerFr;
            USelAbilLb.VScrollBar = AOWVScrollBar1;
            TCAbilPnl.AOWWindow = TCUnitWin;
            AbilAtt.AOWWindow = AbilStatBox;
            AbilAttLbl.AOWWindow = AbilStatBox;
            AbilStatBox.AOWWindow = AbilFr;
            AbilStat.AOWWindow = AbilFr;
            AbilStat.VScrollBar = AbilSB;
            AbilDam.AOWWindow = AbilStatBox;
            AbilDamLbl.AOWWindow = AbilStatBox;
            TargetLbl.AOWWindow = TargetSelPnl;
            TargetULbl.AOWWindow = TNamePnl;
            TargetSelPnl.AOWWindow = TCAbilPnl;
            NoTargetPnl.AOWWindow = TCAbilPnl;
            TargetMessage.AOWWindow = NoTargetPnl;
            UnitHits.AOWWindow = TCUnitPnl;
            UnitMana.AOWWindow = TCUnitPnl;
            CelticCross.AOWWindow = NoTargetPnl;
            UStatP1.AOWWindow = UStatPnl;
            UStatP2.AOWWindow = UStatPnl;
            AAttackIcon.AOWWindow = AStatPnl;
            ADefenseIcon.AOWWindow = AStatPnl;
            AMovesIcon.AOWWindow = AStatPnl;
            ADamageIcon.AOWWindow = AStatPnl;
            AResistanceIcon.AOWWindow = AStatPnl;
            AHitsIcons.AOWWindow = AStatPnl;
            AAttack.AOWWindow = AStatP1;
            ADefense.AOWWindow = AStatP1;
            AMoves.AOWWindow = AStatP1;
            ADamage.AOWWindow = AStatP2;
            AResistance.AOWWindow = AStatP2;
            AHits.AOWWindow = AStatP2;
            AStatPnl.AOWWindow = TargerFr;
            AStatP1.AOWWindow = AStatPnl;
            AStatP2.AOWWindow = AStatPnl;
            Center.AOWWindow = TCUnitWin;
            AbilSB.AOWWindow = AbilFr;
            UAbilSB.AOWWindow = TCUnitPnl;
            AOWVScrollBar1.AOWWindow = TargerFr;
            AbilFr.AOWWindow = TCAbilPnl;
            ANameFr.AOWWindow = AbilFr;
            TargerFr.AOWWindow = TargetSelPnl;
            TNamePnl.AOWWindow = TargerFr;
            UnitFr.AOWWindow = TCUnitPnl;
            UnitBox.AOWWindow = TCUnitPnl;
            Loaded();
            TCUnitWin.Loaded();
            SIcon.Loaded();
            Min.Loaded();
            Close.Loaded();
            IWTitle.Loaded();
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
            UnitName.Loaded();
            UStatPnl.Loaded();
            UAbilLB.Loaded();
            AbilNameLbl.Loaded();
            TCUnitPnl.Loaded();
            AOWImage1.Loaded();
            AUnitName.Loaded();
            USelAbilLb.Loaded();
            TCAbilPnl.Loaded();
            AbilAtt.Loaded();
            AbilAttLbl.Loaded();
            AbilStatBox.Loaded();
            AbilStat.Loaded();
            AbilDam.Loaded();
            AbilDamLbl.Loaded();
            TargetLbl.Loaded();
            TargetULbl.Loaded();
            TargetSelPnl.Loaded();
            NoTargetPnl.Loaded();
            TargetMessage.Loaded();
            UnitHits.Loaded();
            UnitMana.Loaded();
            CelticCross.Loaded();
            UStatP1.Loaded();
            UStatP2.Loaded();
            AAttackIcon.Loaded();
            ADefenseIcon.Loaded();
            AMovesIcon.Loaded();
            ADamageIcon.Loaded();
            AResistanceIcon.Loaded();
            AHitsIcons.Loaded();
            AAttack.Loaded();
            ADefense.Loaded();
            AMoves.Loaded();
            ADamage.Loaded();
            AResistance.Loaded();
            AHits.Loaded();
            AStatPnl.Loaded();
            AStatP1.Loaded();
            AStatP2.Loaded();
            Center.Loaded();
            AbilSB.Loaded();
            UAbilSB.Loaded();
            AOWVScrollBar1.Loaded();
            AbilFr.Loaded();
            ANameFr.Loaded();
            TargerFr.Loaded();
            TNamePnl.Loaded();
            UnitFr.Loaded();
            UnitBox.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            TCUnitWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCUnitWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TCUnitWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Min.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            IWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
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
            UAttack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDefense.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDefense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UMoves.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UMoves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDamage.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDamage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UResistance.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UResistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UHits.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UHits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UnitName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            UnitName.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UnitName.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UStatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UStatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAbilLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UAbilLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UAbilLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAbilLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => UAbilLB.PopupMenu = module.BannerPopup);
            AbilNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TCUnitPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TCUnitPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AUnitName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            AUnitName.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AUnitName.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            USelAbilLb.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            USelAbilLb.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            USelAbilLb.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            USelAbilLb.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            AbilAtt.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AbilAtt.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AbilAttLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AbilStatBox.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AbilStat.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AbilStat.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AbilStat.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AbilStat.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            AbilDam.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AbilDam.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AbilDamLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TargetLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TargetULbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TargetMessage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TargetMessage.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TargetMessage.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitHits.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UnitHits.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitHits.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitMana.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UnitMana.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitMana.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CelticCross.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AAttackIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AAttackIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            ADefenseIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ADefenseIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AMovesIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AMovesIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            ADamageIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ADamageIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AResistanceIcon.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AResistanceIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AHitsIcons.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AHitsIcons.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            AAttack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ADefense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AMoves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ADamage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AResistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AHits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            AStatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AStatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Center.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Center.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            AbilSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbilSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbilSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbilSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbilFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AbilFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ANameFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TargerFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TargerFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TNamePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UnitFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_TCUnitWinResize_0(TObject p0)
        {
            TCUnitWinResize();
        }

        private void Layout_MinClick_1(TObject p0)
        {
            MinClick();
        }

        private void Layout_CloseClick_2(TObject p0)
        {
            CloseClick();
        }

        private void Layout_UAbilLBUpdate_3(TObject p0)
        {
            UAbilLBUpdate();
        }

        private void Layout_CenterClick_4(TObject p0)
        {
            CenterClick();
        }
    }
}
