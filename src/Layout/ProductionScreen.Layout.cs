// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWProdScreen
{
    public partial class TProductionScreen
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ProductionScreen";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_ProductionScreenCreate_0;
            LayoutOnDestroy = Layout_ProductionScreenDestroy_1;
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 55,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
                OnHide = Layout_DlgHide_2,
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
                MaskColor = 7346272,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 8,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
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
            UnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitPnl",
                Priority = 2000,
                WinLeft = 12,
                WinTop = 3,
                WinWidth = 594,
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
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                WinHeight = 129,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitPnl);
            UAbilitiesLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "UAbilitiesLB",
                OnMouseDown = UAbilitiesLBMouseDown,
                Priority = 200,
                WinLeft = 2,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Lists unit's abilities"
                },
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
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 2
                },
                Masked = false,
                WinWidth = 157,
                WinHeight = 87,
                BorderWidth = 10,
                BorderHeight = 4,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 17,
                ILIndexTSelected = 8,
                ItemHeight = 16,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(UAbilitiesLB);
            UAttackIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UAttackIcon",
                Priority = 1000,
                WinLeft = 12,
                WinTop = 3,
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 3,
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
                Priority = 1000,
                WinLeft = 12,
                WinTop = 20,
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 20,
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
                Priority = 1000,
                WinLeft = 8,
                WinTop = 37,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 37,
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
                Priority = 1000,
                WinLeft = 88,
                WinTop = 3,
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
                    TopOffset = 3,
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
                Priority = 1000,
                WinLeft = 88,
                WinTop = 20,
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
                    TopOffset = 20,
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
                Priority = 1000,
                WinLeft = 88,
                WinTop = 37,
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
                    TopOffset = 37,
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
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "5",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UAttack);
            UDefense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDefense",
                Priority = 0,
                WinLeft = 10,
                WinTop = 22,
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
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "5+1",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UDefense);
            UMoves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UMoves",
                Priority = 0,
                WinLeft = 4,
                WinTop = 39,
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
                    TopOffset = 39,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "50/50",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UMoves);
            UDamage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDamage",
                Priority = 0,
                WinLeft = 15,
                WinTop = 5,
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
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "5",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UDamage);
            UResistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UResistance",
                Priority = 0,
                WinLeft = 15,
                WinTop = 22,
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
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "5",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UResistance);
            UHits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UHits",
                Priority = 0,
                WinLeft = 1,
                WinTop = 39,
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
                    TopOffset = 39,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "50/50",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UHits);
            UStatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatPnl",
                Priority = 20,
                WinLeft = 132,
                WinTop = 29,
                WinWidth = 160,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 140,
                    RightOffset = 0,
                    TopOffset = 29,
                    BottomOffset = 10
                },
                WinHeight = 90,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatPnl);
            UStatP1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatP1",
                Priority = 1000,
                WinLeft = 29,
                WinTop = 3,
                WinWidth = 42,
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
                    LeftOffset = 29,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                WinHeight = -109,
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
                Priority = 1000,
                WinLeft = 114,
                WinTop = 3,
                WinWidth = 36,
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
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                WinHeight = -109,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatP2);
            UAlignment = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UAlignment",
                Priority = 1000,
                WinLeft = 63,
                WinTop = 5,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UAlignment);
            ULevel = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ULevel",
                Priority = 1000,
                WinLeft = 63,
                WinTop = 17,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 17,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(ULevel);
            UPicturePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UPicturePnl",
                Priority = 20,
                WinLeft = 30,
                WinTop = 10,
                WinWidth = 90,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 10
                },
                WinHeight = 109,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UPicturePnl);
            UCostPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UCostPnl",
                Priority = 20,
                WinLeft = 474,
                WinTop = 10,
                WinWidth = 90,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 30,
                    TopOffset = 10,
                    BottomOffset = 10
                },
                WinHeight = 109,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UCostPnl);
            StructName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "StructName",
                Priority = 1500,
                WinLeft = 300,
                WinTop = 16,
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
                    TopOffset = 16,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "####",
                TextColor = 0
            };
            LayoutSession.EndRead(StructName);
            CostLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "CostLbl",
                Priority = 0,
                WinLeft = 33,
                WinTop = 10,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Cost",
                TextColor = 9415072
            };
            LayoutSession.EndRead(CostLbl);
            UCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UCost",
                Priority = 0,
                WinLeft = 28,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UCost);
            UnitLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UnitLbl",
                Priority = 0,
                WinLeft = 267,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 0
            };
            LayoutSession.EndRead(UnitLbl);
            TurnsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnsLbl",
                Priority = 0,
                WinLeft = 31,
                WinTop = 10,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns",
                TextColor = 9415072
            };
            LayoutSession.EndRead(TurnsLbl);
            UTurns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UTurns",
                Priority = 0,
                WinLeft = 23,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "####",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UTurns);
            B4CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B4CB",
                Tag = 3,
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 1700,
                WinLeft = 153,
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
                    LeftOffset = 153,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B4CB);
            B5CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B5CB",
                Tag = 4,
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 1600,
                WinLeft = 204,
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
                    LeftOffset = 204,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B5CB);
            B6CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B6CB",
                Tag = 5,
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 1500,
                WinLeft = 255,
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
                    LeftOffset = 255,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B6CB);
            B1CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B1CB",
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 2000,
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B1CB);
            B2CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B2CB",
                Tag = 1,
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 1900,
                WinLeft = 51,
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
                    LeftOffset = 51,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B2CB);
            B3CB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "B3CB",
                Tag = 2,
                OnPreDraw = ActivityPreDraw,
                OnUpdate = B4CBUpdate,
                Priority = 1800,
                WinLeft = 102,
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
                    LeftOffset = 102,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
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
                OnChange = ActivityChange,
                RadioIndex = 1,
                OnDoubleClick = ActivityDoubleClick
            };
            LayoutSession.EndRead(B3CB);
            T2SBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "T2SBar",
                Visible = false,
                Priority = 201,
                WinLeft = 142,
                WinTop = 1,
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
                WinHeight = 87,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 88,
                ILIndexIncDown = 89,
                ILIndexDecUp = 86,
                ILIndexDecDown = 87,
                ILIndexBack = 93,
                ILIndexTab = 90
            };
            LayoutSession.EndRead(T2SBar);
            AbilPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AbilPnl",
                Priority = 10000,
                WinLeft = 302,
                WinTop = 29,
                WinWidth = 160,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 51,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 140,
                    TopOffset = 29,
                    BottomOffset = 10
                },
                WinHeight = 90,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AbilPnl);
            UpgradePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgradePnl",
                Visible = false,
                Priority = 1000,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 616,
                Masked = true,
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
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 229,
                    BottomOffset = 3
                },
                WinHeight = 128,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgradePnl);
            UpgradeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpgradeLbl",
                Priority = 0,
                WinLeft = 278,
                WinTop = 6,
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpgradeLbl);
            UpgradeMemoFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgradeMemoFr",
                Priority = 20,
                WinLeft = 160,
                WinTop = 29,
                WinWidth = 296,
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
                    LeftOffset = 160,
                    RightOffset = 160,
                    TopOffset = 29,
                    BottomOffset = 10
                },
                WinHeight = 89,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgradeMemoFr);
            UpgradeMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "UpgradeMemo",
                Priority = 0,
                WinLeft = 2,
                WinTop = 1,
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
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 293,
                WinHeight = 87,
                BorderWidth = 8,
                BorderHeight = 3,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 14,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(UpgradeMemo);
            RightPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightPnl",
                Priority = 30000,
                WinLeft = 475,
                WinTop = 358,
                WinWidth = 153,
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
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 12
                },
                WinHeight = 62,
                ILIndexFrame = 0,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightPnl);
            QueuePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "QueuePnl",
                Priority = 5000,
                WinLeft = 11,
                WinTop = 358,
                WinWidth = 157,
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
                    LeftOffset = 11,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                WinHeight = 63,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(QueuePnl);
            Oval = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Oval",
                Priority = 1000,
                WinLeft = 11,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 0,
                WinWidth = 618,
                WinHeight = 163,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Oval);
            ShadowL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ShadowL",
                Priority = 500,
                WinLeft = 11,
                WinTop = 12,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 0,
                WinWidth = 231,
                WinHeight = 151,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ShadowL);
            ShadowR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ShadowR",
                Priority = 500,
                WinLeft = 398,
                WinTop = 12,
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
                    LeftOffset = 12,
                    RightOffset = 11,
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 0,
                WinWidth = 231,
                WinHeight = 151,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ShadowR);
            MainBtn3 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MainBtn3",
                Priority = 100,
                WinLeft = 0,
                WinTop = 42,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "####",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 153,
                OnClick = Layout_MainBtn3Click_3
            };
            LayoutSession.EndRead(MainBtn3);
            MainBtn1 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MainBtn1",
                Priority = 100,
                WinLeft = 0,
                WinTop = 0,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 52
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "####",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 153,
                OnClick = MainBtn1Click
            };
            LayoutSession.EndRead(MainBtn1);
            QueueLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "QueueLB",
                Priority = 1000,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Lists unit's abilities"
                },
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 157,
                WinHeight = 63,
                BorderWidth = 3,
                BorderHeight = 1,
                OnChange = QueueLBChange,
                Selected = -1,
                TextColor = 6783862,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 18,
                ILIndexTSelected = 8,
                ItemHeight = 11,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(QueueLB);
            ActivityPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ActivityPnl",
                Priority = 50000,
                WinLeft = 11,
                WinTop = 227,
                WinWidth = 618,
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
                    LeftOffset = 11,
                    RightOffset = 11,
                    TopOffset = 227,
                    BottomOffset = 73
                },
                WinHeight = 132,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ActivityPnl);
            InfoPnl2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoPnl2",
                Priority = 5000,
                WinLeft = 167,
                WinTop = 354,
                WinWidth = 307,
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
                    LeftOffset = 167,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                WinHeight = 68,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoPnl2);
            ActiveAction = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ActiveAction",
                Priority = 10,
                WinLeft = 78,
                WinTop = 11,
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 11,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 6783862
            };
            LayoutSession.EndRead(ActiveAction);
            ActiveActionTurns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ActiveActionTurns",
                Priority = 10,
                WinLeft = 81,
                WinTop = 26,
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 26,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#####",
                TextColor = 6783862
            };
            LayoutSession.EndRead(ActiveActionTurns);
            SepBar = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SepBar",
                Priority = 1500,
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
                CurrentImage = 9,
                AnimEnd = 0,
                WinWidth = 618,
                WinHeight = 4,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SepBar);
            ActivityPnl2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ActivityPnl2",
                Visible = false,
                Priority = 99999,
                WinLeft = 167,
                WinTop = 358,
                WinWidth = 307,
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
                    LeftOffset = 167,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                WinHeight = 64,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ActivityPnl2);
            ActiveActionTurnsBar = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "ActiveActionTurnsBar",
                Priority = 0,
                WinLeft = 10,
                WinTop = 41,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 172,
                WinHeight = 15,
                Min = 2,
                Max = 100,
                Position = 50,
                ILIndexFrame = 113,
                ILIndexTNormal = 18,
                ILIndexTSelected = 8
            };
            LayoutSession.EndRead(ActiveActionTurnsBar);
            MainBtn2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MainBtn2",
                Visible = false,
                Priority = 100,
                WinLeft = 0,
                WinTop = 21,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 34,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "####",
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 153,
                OnClick = Layout_MainBtn2Click_4
            };
            LayoutSession.EndRead(MainBtn2);
            InfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoPnl",
                Visible = false,
                Priority = 3099,
                WinLeft = 11,
                WinTop = 227,
                WinWidth = 618,
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
                    LeftOffset = 11,
                    RightOffset = 11,
                    TopOffset = 227,
                    BottomOffset = 74
                },
                WinHeight = 131,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoPnl);
            IPPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IPPnl",
                Priority = 3000,
                WinLeft = 55,
                WinTop = 11,
                WinWidth = 248,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 55,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                WinHeight = 110,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IPPnl);
            IPPicFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "IPPicFr",
                Priority = 0,
                WinLeft = 1,
                WinTop = 1,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 90,
                WinHeight = 108,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 67
            };
            LayoutSession.EndRead(IPPicFr);
            PlaceInfoMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "PlaceInfoMemo",
                Priority = 0,
                WinLeft = 90,
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
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 158,
                WinHeight = 109,
                BorderWidth = 5,
                BorderHeight = 2,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 13,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PlaceInfoMemo);
            IIPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IIPnl",
                Priority = 3000,
                WinLeft = 321,
                WinTop = 11,
                WinWidth = 242,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 55,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                WinHeight = 110,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IIPnl);
            IncomePicture = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "IncomePicture",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 90,
                WinHeight = 109,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 60
            };
            LayoutSession.EndRead(IncomePicture);
            IncomeMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeMemo",
                Priority = 0,
                WinLeft = 90,
                WinTop = 32,
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
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 32,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 152,
                WinHeight = 77,
                BorderWidth = 5,
                BorderHeight = 1,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 13,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(IncomeMemo);
            IncomeHdr = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeHdr",
                Priority = 0,
                WinLeft = 90,
                WinTop = 1,
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
                    LeftOffset = 90,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 152,
                WinHeight = 32,
                BorderWidth = 5,
                BorderHeight = 2,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(IncomeHdr);
            CurrentInfo = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CurrentInfo",
                Priority = 3000,
                WinLeft = 65,
                WinTop = 4,
                WinWidth = 192,
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
                    LeftOffset = 65,
                    RightOffset = 50,
                    TopOffset = 4,
                    BottomOffset = 1
                },
                WinHeight = 63,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CurrentInfo);
            RPillar = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "RPillar",
                Priority = 5000,
                WinLeft = 559,
                WinTop = 80,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 11,
                    TopOffset = 80,
                    BottomOffset = 75
                },
                Masked = false,
                WinWidth = 70,
                WinHeight = 277,
                FrameType = Aow.Units.AoWFrame.TFrameType.frVertLine,
                ILIndexFrame = 0,
                ILIndexLine = 64,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(RPillar);
            LPillar = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "LPillar",
                Priority = 5000,
                WinLeft = 11,
                WinTop = 80,
                CaptureInput = false,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 80,
                    BottomOffset = 75
                },
                Masked = false,
                WinWidth = 70,
                WinHeight = 277,
                FrameType = Aow.Units.AoWFrame.TFrameType.frVertLine,
                ILIndexFrame = 0,
                ILIndexLine = 61,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(LPillar);
            QueueInfo = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "QueueInfo",
                Priority = 3000,
                WinLeft = 0,
                WinTop = 4,
                WinWidth = 66,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 1
                },
                WinHeight = 63,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(QueueInfo);
            QueueLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "QueueLbl",
                Priority = 10,
                WinLeft = 20,
                WinTop = 8,
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Queue",
                TextColor = 6783862
            };
            LayoutSession.EndRead(QueueLbl);
            LoopQueueBox = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LoopQueueBox",
                Priority = 0,
                WinLeft = 27,
                WinTop = 25,
                CaptureInput = true,
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
                    LeftOffset = 20,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 48,
                ILIndexLit = 49,
                ILIndexUp = 47,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = Layout_LoopQueueBoxChange_5,
                RadioIndex = 0
            };
            LayoutSession.EndRead(LoopQueueBox);
            RepeatLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RepeatLbl",
                Priority = 10,
                WinLeft = 18,
                WinTop = 47,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 55,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 45,
                    BottomOffset = 5
                },
                Masked = true,
                Text = "Repeat",
                TextColor = 6783862
            };
            LayoutSession.EndRead(RepeatLbl);
            ActiveActionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ActiveActionPnl",
                Priority = 3000,
                WinLeft = 256,
                WinTop = 4,
                WinWidth = 51,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 1
                },
                WinHeight = 63,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ActiveActionPnl);
            CurrentActionImage = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CurrentActionImage",
                OnDraw = CurrentActionImageDraw,
                Priority = 0,
                WinLeft = 3,
                WinTop = 3,
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
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 3,
                    BottomOffset = 3
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 42,
                AnimEnd = 0,
                WinWidth = 45,
                WinHeight = 57,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CurrentActionImage);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 0,
                WinTop = 20,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 20,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 153,
                WinHeight = 22,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 113,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            BlackBg2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "BlackBg2",
                Priority = 10,
                WinLeft = 0,
                WinTop = 1,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 4,
                AnimEnd = 0,
                WinWidth = 307,
                WinHeight = 63,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(BlackBg2);
            UPictureFramePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UPictureFramePnl",
                Priority = 20,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 87,
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
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 2
                },
                WinHeight = 106,
                ILIndexFrame = 113,
                ILIndexTexture = 19,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UPictureFramePnl);
            UStatFr1 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatFr1",
                Priority = 20,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 78,
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
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 33
                },
                WinHeight = 56,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatFr1);
            UStatFr2 = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UStatFr2",
                Priority = 20,
                WinLeft = 80,
                WinTop = 1,
                WinWidth = 79,
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
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 33
                },
                WinHeight = 56,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UStatFr2);
            UInfoFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UInfoFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 58,
                WinWidth = 157,
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
                    RightOffset = 1,
                    TopOffset = 58,
                    BottomOffset = 2
                },
                WinHeight = 30,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UInfoFr);
            CostFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "CostFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 87,
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
                    BottomOffPercent = 50,
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                WinHeight = 54,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(CostFr);
            TurnsFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TurnsFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 54,
                WinWidth = 87,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 50,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                WinHeight = 53,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TurnsFr);
            UpgPicPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgPicPnl",
                Priority = 20,
                WinLeft = 41,
                WinTop = 12,
                WinWidth = 90,
                Masked = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 41,
                    RightOffset = 0,
                    TopOffset = 12,
                    BottomOffset = 7
                },
                WinHeight = 109,
                ILIndexFrame = 105,
                ILIndexTexture = 19,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgPicPnl);
            UpgradePic = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UpgradePic",
                Priority = 10,
                WinLeft = 44,
                WinTop = 14,
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
                    LeftOffset = 44,
                    RightOffset = 28,
                    TopOffset = 14,
                    BottomOffset = 11
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 21,
                AnimEnd = 0,
                WinWidth = 85,
                WinHeight = 103,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UpgradePic);
            UpgPicFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgPicFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 87,
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
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 2
                },
                WinHeight = 106,
                ILIndexFrame = 113,
                ILIndexTexture = 19,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgPicFr);
            UpgCostPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgCostPnl",
                Priority = 20,
                WinLeft = 485,
                WinTop = 12,
                WinWidth = 90,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 41,
                    TopOffset = 12,
                    BottomOffset = 7
                },
                WinHeight = 109,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgCostPnl);
            UpgCostLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpgCostLbl",
                Priority = 0,
                WinLeft = 33,
                WinTop = 10,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Cost",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpgCostLbl);
            UpgradeCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpgradeCost",
                Priority = 0,
                WinLeft = 28,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpgradeCost);
            UpgTurnsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpgTurnsLbl",
                Priority = 0,
                WinLeft = 31,
                WinTop = 10,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turns",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpgTurnsLbl);
            UpgradeTurns = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpgradeTurns",
                Priority = 0,
                WinLeft = 28,
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
                    LeftOffset = 15,
                    RightOffset = 0,
                    TopOffset = 23,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpgradeTurns);
            UpgCostFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgCostFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 87,
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
                    BottomOffPercent = 50,
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                WinHeight = 54,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgCostFr);
            UpgTurnsFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpgTurnsFr",
                Priority = 20,
                WinLeft = 2,
                WinTop = 54,
                WinWidth = 87,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 50,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 2
                },
                WinHeight = 53,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpgTurnsFr);
            RelIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RelIcn",
                Visible = false,
                Priority = 10,
                WinLeft = 8,
                WinTop = 2,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Unit's cost in gems"
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
                    LeftOffset = 8,
                    RightOffset = 0,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 12,
                AnimEnd = 30,
                WinWidth = 11,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RelIcn);
            QueueVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "QueueVSB",
                Visible = false,
                Priority = 1001,
                WinLeft = 140,
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
                LargeChange = 1,
                Position = 0,
                WinHeight = 63,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 88,
                ILIndexIncDown = 89,
                ILIndexDecUp = 86,
                ILIndexDecDown = 87,
                ILIndexBack = 93,
                ILIndexTab = 90
            };
            LayoutSession.EndRead(QueueVSB);
            IPVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "IPVSB",
                Visible = false,
                Priority = 0,
                WinLeft = 231,
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
                Max = 1,
                SmallChange = 1,
                LargeChange = 6,
                Position = 0,
                WinHeight = 109,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 88,
                ILIndexIncDown = 89,
                ILIndexDecUp = 86,
                ILIndexDecDown = 87,
                ILIndexBack = 93,
                ILIndexTab = 90
            };
            LayoutSession.EndRead(IPVSB);
            IIVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "IIVSB",
                Visible = false,
                Priority = 0,
                WinLeft = 225,
                WinTop = 32,
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
                Max = 3,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 77,
                WinWidth = 17,
                NumberVisible = 3,
                VanishWhenFull = true,
                ILIndexIncUp = 88,
                ILIndexIncDown = 89,
                ILIndexDecUp = 86,
                ILIndexDecDown = 87,
                ILIndexBack = 93,
                ILIndexTab = 90
            };
            LayoutSession.EndRead(IIVSB);
            IncomeValuesMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeValuesMemo",
                Priority = 0,
                WinLeft = 202,
                WinTop = 32,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 10,
                    TopOffset = 32,
                    BottomOffset = 1
                },
                Masked = true,
                WinWidth = 30,
                WinHeight = 77,
                BorderWidth = 5,
                BorderHeight = 4,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 20,
                ItemHeight = 13,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(IncomeValuesMemo);
            UFace = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "UFace",
                OnDraw = UFaceDraw,
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
                WinWidth = 89,
                WinHeight = 107
            };
            LayoutSession.EndRead(UFace);
            InfoSep = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "InfoSep",
                Priority = 1500,
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
                CurrentImage = 9,
                AnimEnd = 0,
                WinWidth = 618,
                WinHeight = 4,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(InfoSep);
            Sphere = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sphere",
                Priority = 10,
                WinLeft = 20,
                WinTop = 30,
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
                    RightOffset = 10,
                    TopOffset = 30,
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
            LayoutSession.EndRead(Sphere);
            MapView = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "MapView",
                OnDraw = MapViewDraw,
                OnUpdate = Layout_MapViewUpdate_6,
                Priority = 50,
                WinLeft = 11,
                WinTop = 12,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 618,
                WinHeight = 215
            };
            LayoutSession.EndRead(MapView);
            PNext = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PNext",
                Priority = 100,
                WinLeft = 76,
                WinTop = 21,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Next"
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
                    TopOffPercent = 34,
                    BottomOffPercent = 0,
                    LeftOffset = 76,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = ">",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 77,
                OnClick = Layout_PNextClick_7
            };
            LayoutSession.EndRead(PNext);
            PPrev = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PPrev",
                Priority = 100,
                WinLeft = 0,
                WinTop = 21,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Previous"
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
                    TopOffPercent = 34,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 76,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 79,
                ILIndexLit = 82,
                ILIndexUp = 76,
                Text = "<",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 77,
                OnClick = Layout_PPrevClick_8
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
            UnitPnl.AOWWindow = ActivityPnl;
            UAbilitiesLB.AOWWindow = AbilPnl;
            UAbilitiesLB.VScrollBar = T2SBar;
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
            UStatPnl.AOWWindow = UnitPnl;
            UStatP1.AOWWindow = UStatPnl;
            UStatP2.AOWWindow = UStatPnl;
            UAlignment.AOWWindow = UInfoFr;
            ULevel.AOWWindow = UInfoFr;
            UPicturePnl.AOWWindow = UnitPnl;
            UCostPnl.AOWWindow = UnitPnl;
            StructName.AOWWindow = Dlg;
            CostLbl.AOWWindow = CostFr;
            UCost.AOWWindow = CostFr;
            UnitLbl.AOWWindow = UnitPnl;
            TurnsLbl.AOWWindow = TurnsFr;
            UTurns.AOWWindow = TurnsFr;
            B4CB.AOWWindow = ActivityPnl2;
            B5CB.AOWWindow = ActivityPnl2;
            B6CB.AOWWindow = ActivityPnl2;
            B1CB.AOWWindow = ActivityPnl2;
            B2CB.AOWWindow = ActivityPnl2;
            B3CB.AOWWindow = ActivityPnl2;
            T2SBar.AOWWindow = AbilPnl;
            AbilPnl.AOWWindow = UnitPnl;
            UpgradePnl.AOWWindow = ActivityPnl;
            UpgradeLbl.AOWWindow = UpgradePnl;
            UpgradeMemoFr.AOWWindow = UpgradePnl;
            UpgradeMemo.AOWWindow = UpgradeMemoFr;
            RightPnl.AOWWindow = Dlg;
            QueuePnl.AOWWindow = Dlg;
            Oval.AOWWindow = Dlg;
            ShadowL.AOWWindow = Dlg;
            ShadowR.AOWWindow = Dlg;
            MainBtn3.AOWWindow = RightPnl;
            MainBtn1.AOWWindow = RightPnl;
            QueueLB.AOWWindow = QueuePnl;
            QueueLB.VScrollBar = QueueVSB;
            ActivityPnl.AOWWindow = Dlg;
            InfoPnl2.AOWWindow = Dlg;
            ActiveAction.AOWWindow = CurrentInfo;
            ActiveActionTurns.AOWWindow = CurrentInfo;
            SepBar.AOWWindow = ActivityPnl;
            ActivityPnl2.AOWWindow = Dlg;
            ActiveActionTurnsBar.AOWWindow = CurrentInfo;
            MainBtn2.AOWWindow = RightPnl;
            InfoPnl.AOWWindow = Dlg;
            IPPnl.AOWWindow = InfoPnl;
            IPPicFr.AOWWindow = IPPnl;
            PlaceInfoMemo.AOWWindow = IPPnl;
            PlaceInfoMemo.VScrollBar = IPVSB;
            IIPnl.AOWWindow = InfoPnl;
            IncomePicture.AOWWindow = IIPnl;
            IncomeMemo.AOWWindow = IIPnl;
            IncomeMemo.VScrollBar = IIVSB;
            IncomeHdr.AOWWindow = IIPnl;
            CurrentInfo.AOWWindow = InfoPnl2;
            RPillar.AOWWindow = Dlg;
            LPillar.AOWWindow = Dlg;
            QueueInfo.AOWWindow = InfoPnl2;
            QueueLbl.AOWWindow = QueueInfo;
            LoopQueueBox.AOWWindow = QueueInfo;
            RepeatLbl.AOWWindow = QueueInfo;
            ActiveActionPnl.AOWWindow = InfoPnl2;
            CurrentActionImage.AOWWindow = ActiveActionPnl;
            AoWFrame1.AOWWindow = RightPnl;
            BlackBg2.AOWWindow = ActivityPnl2;
            UPictureFramePnl.AOWWindow = UPicturePnl;
            UStatFr1.AOWWindow = UStatPnl;
            UStatFr2.AOWWindow = UStatPnl;
            UInfoFr.AOWWindow = UStatPnl;
            CostFr.AOWWindow = UCostPnl;
            TurnsFr.AOWWindow = UCostPnl;
            UpgPicPnl.AOWWindow = UpgradePnl;
            UpgradePic.AOWWindow = UpgradePnl;
            UpgPicFr.AOWWindow = UpgPicPnl;
            UpgCostPnl.AOWWindow = UpgradePnl;
            UpgCostLbl.AOWWindow = UpgCostFr;
            UpgradeCost.AOWWindow = UpgCostFr;
            UpgTurnsLbl.AOWWindow = UpgTurnsFr;
            UpgradeTurns.AOWWindow = UpgTurnsFr;
            UpgCostFr.AOWWindow = UpgCostPnl;
            UpgTurnsFr.AOWWindow = UpgCostPnl;
            QueueVSB.AOWWindow = QueuePnl;
            IPVSB.AOWWindow = IPPnl;
            IIVSB.AOWWindow = IIPnl;
            IncomeValuesMemo.AOWWindow = IIPnl;
            UFace.AOWWindow = UPictureFramePnl;
            InfoSep.AOWWindow = InfoPnl;
            Sphere.AOWWindow = IPPnl;
            MapView.AOWWindow = Dlg;
            PNext.AOWWindow = RightPnl;
            PPrev.AOWWindow = RightPnl;
            Loaded();
            Dlg.Loaded();
            UnitPnl.Loaded();
            UAbilitiesLB.Loaded();
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
            UAlignment.Loaded();
            ULevel.Loaded();
            UPicturePnl.Loaded();
            UCostPnl.Loaded();
            StructName.Loaded();
            CostLbl.Loaded();
            UCost.Loaded();
            UnitLbl.Loaded();
            TurnsLbl.Loaded();
            UTurns.Loaded();
            B4CB.Loaded();
            B5CB.Loaded();
            B6CB.Loaded();
            B1CB.Loaded();
            B2CB.Loaded();
            B3CB.Loaded();
            T2SBar.Loaded();
            AbilPnl.Loaded();
            UpgradePnl.Loaded();
            UpgradeLbl.Loaded();
            UpgradeMemoFr.Loaded();
            UpgradeMemo.Loaded();
            RightPnl.Loaded();
            QueuePnl.Loaded();
            Oval.Loaded();
            ShadowL.Loaded();
            ShadowR.Loaded();
            MainBtn3.Loaded();
            MainBtn1.Loaded();
            QueueLB.Loaded();
            ActivityPnl.Loaded();
            InfoPnl2.Loaded();
            ActiveAction.Loaded();
            ActiveActionTurns.Loaded();
            SepBar.Loaded();
            ActivityPnl2.Loaded();
            ActiveActionTurnsBar.Loaded();
            MainBtn2.Loaded();
            InfoPnl.Loaded();
            IPPnl.Loaded();
            IPPicFr.Loaded();
            PlaceInfoMemo.Loaded();
            IIPnl.Loaded();
            IncomePicture.Loaded();
            IncomeMemo.Loaded();
            IncomeHdr.Loaded();
            CurrentInfo.Loaded();
            RPillar.Loaded();
            LPillar.Loaded();
            QueueInfo.Loaded();
            QueueLbl.Loaded();
            LoopQueueBox.Loaded();
            RepeatLbl.Loaded();
            ActiveActionPnl.Loaded();
            CurrentActionImage.Loaded();
            AoWFrame1.Loaded();
            BlackBg2.Loaded();
            UPictureFramePnl.Loaded();
            UStatFr1.Loaded();
            UStatFr2.Loaded();
            UInfoFr.Loaded();
            CostFr.Loaded();
            TurnsFr.Loaded();
            UpgPicPnl.Loaded();
            UpgradePic.Loaded();
            UpgPicFr.Loaded();
            UpgCostPnl.Loaded();
            UpgCostLbl.Loaded();
            UpgradeCost.Loaded();
            UpgTurnsLbl.Loaded();
            UpgradeTurns.Loaded();
            UpgCostFr.Loaded();
            UpgTurnsFr.Loaded();
            RelIcn.Loaded();
            QueueVSB.Loaded();
            IPVSB.Loaded();
            IIVSB.Loaded();
            IncomeValuesMemo.Loaded();
            UFace.Loaded();
            InfoSep.Loaded();
            Sphere.Loaded();
            MapView.Loaded();
            PNext.Loaded();
            PPrev.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UAbilitiesLB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UAbilitiesLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UAbilitiesLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UAbilitiesLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAbilitiesLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => UAbilitiesLB.PopupMenu = module.BannerPopup);
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
            UAttack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDefense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UMoves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDamage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UResistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UHits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UStatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UStatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAlignment.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ULevel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UPicturePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UCostPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UCostPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            StructName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            CostLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UnitLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            TurnsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UTurns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            B4CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            B5CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            B6CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            B1CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            B2CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            B3CB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            T2SBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T2SBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbilPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgradeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UpgradeMemoFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgradeMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UpgradeMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgradeMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RightPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Oval.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            ShadowL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            ShadowR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            MainBtn3.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainBtn3.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            MainBtn1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainBtn1.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            QueueLB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            QueueLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            QueueLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            QueueLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            QueueLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            ActivityPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ActiveAction.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ActiveActionTurns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SepBar.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            ActivityPnl2.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ActiveActionTurnsBar.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ActiveActionTurnsBar.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ActiveActionTurnsBar.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            MainBtn2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainBtn2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            InfoPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IPPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IPPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IPPicFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IPPicFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            PlaceInfoMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PlaceInfoMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlaceInfoMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IIPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IIPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomePicture.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomePicture.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            IncomeMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            IncomeMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomeMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomeHdr.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            IncomeHdr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomeHdr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CurrentInfo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CurrentInfo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RPillar.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            LPillar.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            QueueInfo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            QueueInfo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            QueueLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            LoopQueueBox.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            RepeatLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ActiveActionPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ActiveActionPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CurrentActionImage.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            AoWFrame1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            BlackBg2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UPictureFramePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UStatFr1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UStatFr1.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UStatFr2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UStatFr2.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UInfoFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UInfoFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CostFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            CostFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TurnsFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TurnsFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpgPicPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgradePic.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            UpgPicFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgCostPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgCostPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpgCostLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpgradeCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UpgTurnsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpgradeTurns.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UpgCostFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgCostFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpgTurnsFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpgTurnsFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RelIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RelIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.LBIcons;
            QueueVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            QueueVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            QueueVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            QueueVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IPVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IPVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IPVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IPVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IIVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IIVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IIVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IIVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IncomeValuesMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            InfoSep.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            Sphere.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            PNext.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PNext.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PPrev.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PPrev.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_ProductionScreenCreate_0(TObject p0)
        {
            ProductionScreenCreate();
        }

        private void Layout_ProductionScreenDestroy_1(TObject p0)
        {
            ProductionScreenDestroy();
        }

        private void Layout_DlgHide_2(TObject p0)
        {
            DlgHide();
        }

        private void Layout_MainBtn3Click_3(TObject p0)
        {
            MainBtn3Click();
        }

        private void Layout_MainBtn2Click_4(TObject p0)
        {
            MainBtn2Click();
        }

        private void Layout_LoopQueueBoxChange_5(TObject p0)
        {
            LoopQueueBoxChange();
        }

        private void Layout_MapViewUpdate_6(TObject p0)
        {
            MapViewUpdate();
        }

        private void Layout_PNextClick_7(TObject p0)
        {
            PNextClick();
        }

        private void Layout_PPrevClick_8(TObject p0)
        {
            PPrevClick();
        }
    }
}
