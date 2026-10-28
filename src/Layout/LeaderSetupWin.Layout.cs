// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWLeaderSetup
{
    public partial class TLeaderSetupWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "LeaderSetupWin";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_LeaderSetupWinCreate_0;
            LayoutOnDestroy = Layout_LeaderSetupWinDestroy_1;
            SelectedAbilities = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SelectedAbilities",
                OnMouseDown = SelectedAbilitiesCostMouseDown,
                Priority = 10000,
                WinLeft = 10,
                WinTop = 252,
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
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 5,
                    TopOffset = 252,
                    BottomOffset = 41
                },
                Masked = false,
                WinWidth = 215,
                WinHeight = 87,
                BorderWidth = 6,
                BorderHeight = 4,
                OnChange = Layout_SelectedAbilitiesChange_2,
                Selected = -1,
                TextColor = 8421504,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(SelectedAbilities);
            Phase1Header = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Phase1Header",
                Priority = 10,
                WinLeft = 270,
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
                Masked = true,
                Text = "##########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Phase1Header);
            Phase2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Phase2Pnl",
                Visible = false,
                Priority = 20,
                WinLeft = 10,
                WinTop = 40,
                WinWidth = 460,
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
                    LeftOffset = 10,
                    RightOffset = 170,
                    TopOffset = 40,
                    BottomOffset = 60
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Phase2Pnl);
            AvailableAbilities = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AvailableAbilities",
                OnMouseDown = AvailableAbilitiesMouseDown,
                Priority = 10000,
                WinLeft = 235,
                WinTop = 64,
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
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 64,
                    BottomOffset = 41
                },
                Masked = false,
                WinWidth = 215,
                WinHeight = 275,
                BorderWidth = 6,
                BorderHeight = 4,
                OnChange = Layout_AvailableAbilitiesChange_3,
                Selected = -1,
                TextColor = 8421504,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(AvailableAbilities);
            NextBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "NextBtn",
                Priority = 9999,
                WinLeft = 475,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 13
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Next",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 135,
                OnClick = Layout_NextBtnClick_4
            };
            LayoutSession.EndRead(NextBtn);
            PrevBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PrevBtn",
                Priority = 9999,
                WinLeft = 325,
                WinTop = 11,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 15,
                    RightOffset = 15,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Back",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 125,
                OnClick = Layout_PrevBtnClick_5
            };
            LayoutSession.EndRead(PrevBtn);
            ClearBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ClearBtn",
                Priority = 9999,
                WinLeft = 10,
                WinTop = 11,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Clears all displayed settings."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 67
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Clear",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 135,
                OnClick = Layout_ClearBtnClick_6
            };
            LayoutSession.EndRead(ClearBtn);
            DefaultBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefaultBtn",
                Priority = 9999,
                WinLeft = 165,
                WinTop = 11,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Reverts all displayed settings to their default values."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Default",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 135,
                OnClick = DefaultBtnClick
            };
            LayoutSession.EndRead(DefaultBtn);
            LMainPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LMainPnl",
                Priority = 1,
                WinLeft = 480,
                WinTop = 40,
                WinWidth = 150,
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
                    LeftOffset = 480,
                    RightOffset = 10,
                    TopOffset = 40,
                    BottomOffset = 60
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LMainPnl);
            EditStatisticsPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "EditStatisticsPnl",
                Priority = 10,
                WinLeft = 10,
                WinTop = 65,
                WinWidth = 215,
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
                    BottomOffPercent = 50,
                    LeftOffset = 10,
                    RightOffset = 5,
                    TopOffset = 65,
                    BottomOffset = 4
                },
                WinHeight = 142,
                ILIndexFrame = 96,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(EditStatisticsPnl);
            AttValue = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttValue",
                Priority = 0,
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(AttValue);
            AttLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Attack",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AttLbl);
            AttPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AttPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 215,
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
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(DamValue);
            DamLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Damage",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DamLbl);
            DamPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DamPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 48,
                WinWidth = 215,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 48,
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
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(DefValue);
            DefLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Defense",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DefLbl);
            DefPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DefPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 24,
                WinWidth = 215,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 24,
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
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(ResValue);
            ResLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Resistance",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ResLbl);
            ResPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 72,
                WinWidth = 215,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 72,
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
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(HitValue);
            HitLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Hits",
                TextColor = 8421504
            };
            LayoutSession.EndRead(HitLbl);
            HitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "HitPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 96,
                WinWidth = 215,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 96,
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
                WinLeft = 182,
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
                TextColor = 12632256
            };
            LayoutSession.EndRead(MovValue);
            MovLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovLbl",
                Priority = 0,
                WinLeft = 30,
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
                    LeftOffset = 30,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Moves",
                TextColor = 8421504
            };
            LayoutSession.EndRead(MovLbl);
            MovPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MovPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 120,
                WinWidth = 215,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 120,
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
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AttI);
            DamI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamI",
                Priority = 0,
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DamI);
            DefI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefI",
                Priority = 0,
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DefI);
            ResI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResI",
                Priority = 0,
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ResI);
            HitI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitI",
                Priority = 0,
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(HitI);
            MovI = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovI",
                Priority = 0,
                WinLeft = 169,
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
                    RightOffset = 40,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "=",
                TextColor = 8421504
            };
            LayoutSession.EndRead(MovI);
            AttA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AttA);
            DamA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DamA);
            DefA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DefA);
            ResA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ResA);
            HitA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(HitA);
            MovA = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovA",
                Priority = 0,
                WinLeft = 141,
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
                    RightOffset = 60,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(MovA);
            AttP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AttP);
            DamP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DamP);
            DefP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DefP);
            ResP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ResP);
            HitP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(HitP);
            MovP = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovP",
                Priority = 0,
                WinLeft = 123,
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
                    RightOffset = 85,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "+",
                TextColor = 8421504
            };
            LayoutSession.EndRead(MovP);
            AttB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AttB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AttB);
            DamB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DamB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DamB);
            DefB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DefB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DefB);
            ResB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(ResB);
            HitB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HitB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(HitB);
            MovB = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "MovB",
                Priority = 0,
                WinLeft = 101,
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
                    RightOffset = 100,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "10",
                TextColor = 8421504
            };
            LayoutSession.EndRead(MovB);
            SkillPointsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SkillPointsLbl",
                Priority = 10,
                WinLeft = 185,
                WinTop = 12,
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
                    TopOffset = 12,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 12632256
            };
            LayoutSession.EndRead(SkillPointsLbl);
            UAttIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UAttIcn",
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
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
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
            LayoutSession.EndRead(UAttIcn);
            UDefIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UDefIcn",
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
                    LeftOffPercent = 5,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
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
            LayoutSession.EndRead(UDefIcn);
            UMovIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UMovIcn",
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
                    LeftOffPercent = 5,
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
                CurrentImage = 0,
                AnimEnd = 19,
                WinWidth = 32,
                WinHeight = 17,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(UMovIcn);
            UDamIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UDamIcn",
                Priority = 0,
                WinLeft = 71,
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
            LayoutSession.EndRead(UDamIcn);
            UResIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UResIcn",
                Priority = 0,
                WinLeft = 71,
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
            LayoutSession.EndRead(UResIcn);
            UHitIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "UHitIcn",
                Priority = 0,
                WinLeft = 71,
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
            LayoutSession.EndRead(UHitIcn);
            UAttack = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UAttack",
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
                Text = "5",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UAttack);
            UDefense = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDefense",
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
                Text = "5+1",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UDefense);
            UMoves = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UMoves",
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
                Text = "50/50",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UMoves);
            UDamage = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UDamage",
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
                Text = "5",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UDamage);
            UResistance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UResistance",
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
                Text = "5",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UResistance);
            UHits = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UHits",
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
                Text = "50/50",
                TextColor = 8421504
            };
            LayoutSession.EndRead(UHits);
            StatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "StatPnl",
                Priority = 0,
                WinLeft = 10,
                WinTop = 180,
                WinWidth = 130,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 180,
                    BottomOffset = 0
                },
                WinHeight = 56,
                ILIndexFrame = 64,
                ILIndexTexture = 19,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(StatPnl);
            UAbilities = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "UAbilities",
                Priority = 0,
                WinLeft = 10,
                WinTop = 250,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 250,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 130,
                WinHeight = 120,
                LayoutItems = new LayoutStrings()
                {
                    ""
                },
                BorderWidth = 6,
                BorderHeight = 3,
                Selected = -1,
                TextColor = 8421504,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(UAbilities);
            AbilityPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "AbilityPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 150,
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
                    BottomOffset = 10
                },
                WinHeight = 370,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(AbilityPnl);
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
                WinLeft = 90,
                WinTop = 0,
                WinWidth = 36,
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
            LayoutSession.EndRead(UStatP2);
            LeaderName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeaderName",
                Priority = 0,
                WinLeft = 51,
                WinTop = 130,
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
                    TopOffset = 130,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 12632256
            };
            LayoutSession.EndRead(LeaderName);
            ShieldL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ShieldL",
                Priority = 0,
                WinLeft = 5,
                WinTop = 60,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 13,
                AnimEnd = 6,
                WinWidth = 40,
                WinHeight = 54,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ShieldL);
            ShieldR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "ShieldR",
                Priority = 0,
                WinLeft = 105,
                WinTop = 60,
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
                    LeftOffset = 0,
                    RightOffset = 5,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 13,
                AnimEnd = 6,
                WinWidth = 40,
                WinHeight = 54,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(ShieldR);
            ButtonPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ButtonPnl",
                Priority = 9999,
                WinLeft = 10,
                WinTop = 430,
                WinWidth = 620,
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
                    LeftOffset = 10,
                    RightOffset = 10,
                    TopOffset = 430,
                    BottomOffset = 10
                },
                WinHeight = 40,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ButtonPnl);
            LeftPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeftPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 230,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeftPnl);
            RightPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightPnl",
                Priority = 0,
                WinLeft = 230,
                WinTop = 0,
                WinWidth = 230,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightPnl);
            LeaderStatsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeaderStatsLbl",
                Priority = 10,
                WinLeft = 70,
                WinTop = 35,
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
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Leader Statistics",
                TextColor = 12632256
            };
            LayoutSession.EndRead(LeaderStatsLbl);
            LeaderAbilLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeaderAbilLbl",
                Priority = 10,
                WinLeft = 74,
                WinTop = 220,
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
                    TopOffset = 220,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Leader Abilities",
                TextColor = 12632256
            };
            LayoutSession.EndRead(LeaderAbilLbl);
            FullAbilLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "FullAbilLbl",
                Priority = 10,
                WinLeft = 79,
                WinTop = 35,
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
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Available List",
                TextColor = 12632256
            };
            LayoutSession.EndRead(FullAbilLbl);
            RemoveAbility = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RemoveAbility",
                Priority = 0,
                WinLeft = 10,
                WinTop = 351,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes the selected ability from the Leader."
                },
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
                    RightOffset = 235,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Remove",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 215,
                OnClick = Layout_RemoveAbilityClick_7
            };
            LayoutSession.EndRead(RemoveAbility);
            AddAbility = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AddAbility",
                Priority = 0,
                WinLeft = 235,
                WinTop = 351,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds the selected ability to the Leader."
                },
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
                    LeftOffset = 235,
                    RightOffset = 10,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                BorderHeight = 4,
                BorderHeightSelected = 5,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "Add",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 215,
                OnClick = Layout_AddAbilityClick_8
            };
            LayoutSession.EndRead(AddAbility);
            MainFace = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "MainFace",
                Priority = 0,
                WinLeft = 50,
                WinTop = 55,
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
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 50,
                WinHeight = 65,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(MainFace);
            Phase1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Phase1Pnl",
                Priority = 0,
                WinLeft = 10,
                WinTop = 40,
                WinWidth = 460,
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
                    LeftOffset = 10,
                    RightOffset = 170,
                    TopOffset = 40,
                    BottomOffset = 60
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Phase1Pnl);
            Face4 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face4",
                Priority = 0,
                WinLeft = 179,
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
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 103,
                WinHeight = 128,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face4);
            Face5 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face5",
                Priority = 0,
                WinLeft = 290,
                WinTop = 50,
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
                    LeftOffset = 290,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 72,
                WinHeight = 92,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face5);
            Face6 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face6",
                Priority = 0,
                WinLeft = 370,
                WinTop = 50,
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
                    LeftOffset = 370,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 35,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face6);
            Face7 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face7",
                Priority = 0,
                WinLeft = 412,
                WinTop = 50,
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
                    LeftOffset = 412,
                    RightOffset = 0,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 20,
                WinHeight = 26,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face7);
            Face3 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face3",
                Priority = 0,
                WinLeft = 98,
                WinTop = 50,
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
                    LeftOffset = 290,
                    RightOffset = 290,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 72,
                WinHeight = 92,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face3);
            Face2 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face2",
                Priority = 0,
                WinLeft = 55,
                WinTop = 50,
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
                    LeftOffset = 370,
                    RightOffset = 370,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 35,
                WinHeight = 45,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face2);
            Face1 = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "Face1",
                Priority = 0,
                WinLeft = 28,
                WinTop = 50,
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
                    LeftOffset = 412,
                    RightOffset = 412,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 0,
                WinWidth = 20,
                WinHeight = 26,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(Face1);
            NameEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "NameEdit",
                Priority = 5000,
                WinLeft = 130,
                WinTop = 243,
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
                    LeftOffset = 2,
                    RightOffset = 30,
                    TopOffset = 243,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 200,
                Text = "########",
                BorderWidth = 8,
                BorderHeight = 6,
                OnChange = Layout_NameEditChange_9,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 2,
                TextColor = 16777215,
                Editable = true,
                ILIndex = 23,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(NameEdit);
            NameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NameLbl",
                Priority = 0,
                WinLeft = 210,
                WinTop = 220,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 220,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Name",
                TextColor = 9402970
            };
            LayoutSession.EndRead(NameLbl);
            FaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "FaceLbl",
                Priority = 0,
                WinLeft = 199,
                WinTop = 25,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 25,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Portrait",
                TextColor = 9402970
            };
            LayoutSession.EndRead(FaceLbl);
            RaceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "RaceLbl",
                Priority = 0,
                WinLeft = 212,
                WinTop = 271,
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
                    LeftOffset = 132,
                    RightOffset = 0,
                    TopOffset = 271,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Race",
                TextColor = 9402970
            };
            LayoutSession.EndRead(RaceLbl);
            UAbilSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "UAbilSB",
                Visible = false,
                Priority = 0,
                WinLeft = 123,
                WinTop = 250,
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
                WinHeight = 120,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(UAbilSB);
            AvailableAbilitiesLB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AvailableAbilitiesLB",
                Visible = false,
                Priority = 100001,
                WinLeft = 433,
                WinTop = 64,
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
                WinHeight = 275,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_AvailableAbilitiesLBChange_10,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(AvailableAbilitiesLB);
            SelectedAbilitiesLB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "SelectedAbilitiesLB",
                Visible = false,
                Priority = 10001,
                WinLeft = 208,
                WinTop = 252,
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
                OnChange = Layout_SelectedAbilitiesLBChange_11,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(SelectedAbilitiesLB);
            Phase3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "Phase3Pnl",
                Visible = false,
                Priority = 0,
                WinLeft = 10,
                WinTop = 40,
                WinWidth = 460,
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
                    LeftOffset = 10,
                    RightOffset = 170,
                    TopOffset = 40,
                    BottomOffset = 60
                },
                WinHeight = 380,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(Phase3Pnl);
            SpheresLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpheresLbl",
                Priority = 0,
                WinLeft = 144,
                WinTop = 15,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 15,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "The Spheres of Magic",
                TextColor = 9402970
            };
            LayoutSession.EndRead(SpheresLbl);
            SelDeath = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelDeath",
                Priority = 0,
                WinLeft = 124,
                WinTop = 156,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    TopOffset = 0,
                    BottomOffset = 25
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 31,
                ILIndexLit = 32,
                ILIndexUp = 30,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_SelDeathClick_12
            };
            LayoutSession.EndRead(SelDeath);
            SpherePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SpherePnl",
                Priority = 0,
                WinLeft = 80,
                WinTop = 40,
                WinWidth = 300,
                Masked = true,
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
                    RightOffset = 170,
                    TopOffset = 40,
                    BottomOffset = 60
                },
                WinHeight = 230,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SpherePnl);
            SelLife = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelLife",
                Priority = 0,
                WinLeft = 124,
                WinTop = 25,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = Layout_SelLifeClick_13
            };
            LayoutSession.EndRead(SelLife);
            SelFire = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelFire",
                Priority = 0,
                WinLeft = 60,
                WinTop = 55,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 43,
                ILIndexLit = 44,
                ILIndexUp = 42,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_SelFireClick_14
            };
            LayoutSession.EndRead(SelFire);
            SelEarth = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelEarth",
                Priority = 0,
                WinLeft = 60,
                WinTop = 126,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 55
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 34,
                ILIndexLit = 35,
                ILIndexUp = 33,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_SelEarthClick_15
            };
            LayoutSession.EndRead(SelEarth);
            SelAir = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelAir",
                Priority = 0,
                WinLeft = 188,
                WinTop = 55,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    LeftOffset = 50,
                    RightOffset = 60,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 37,
                ILIndexLit = 38,
                ILIndexUp = 36,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_SelAirClick_16
            };
            LayoutSession.EndRead(SelAir);
            SelWater = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelWater",
                Priority = 0,
                WinLeft = 188,
                WinTop = 126,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Adds this sphere to the Leader."
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
                    LeftOffset = 50,
                    RightOffset = 60,
                    TopOffset = 55,
                    BottomOffset = 55
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 40,
                ILIndexLit = 41,
                ILIndexUp = 39,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                OnClick = Layout_SelWaterClick_17
            };
            LayoutSession.EndRead(SelWater);
            LifeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LifeLbl",
                Priority = 0,
                WinLeft = 140,
                WinTop = 5,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 5,
                    BottomOffset = 5
                },
                Masked = true,
                Text = "Life",
                TextColor = 8421504
            };
            LayoutSession.EndRead(LifeLbl);
            Sel4 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel4",
                Tag = 3,
                Priority = 0,
                WinLeft = 159,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 159,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel4);
            SphereSelectionPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SphereSelectionPnl",
                Priority = 0,
                WinLeft = 45,
                WinTop = 310,
                WinWidth = 370,
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
                    LeftOffset = 0,
                    RightOffset = 160,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                WinHeight = 50,
                ILIndexFrame = 72,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SphereSelectionPnl);
            Sel3 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel3",
                Tag = 2,
                Priority = 0,
                WinLeft = 106,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 106,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel3);
            Sel2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel2",
                Tag = 1,
                Priority = 0,
                WinLeft = 53,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 53,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel2);
            Sel1 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel1",
                Priority = 0,
                WinLeft = 0,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel1);
            Sel5 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel5",
                Tag = 4,
                Priority = 0,
                WinLeft = 212,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 212,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel5);
            Sel6 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel6",
                Tag = 5,
                Priority = 0,
                WinLeft = 265,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 265,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel6);
            Sel7 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sel7",
                Tag = 6,
                Priority = 0,
                WinLeft = 318,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Removes this sphere from the Leader."
                },
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
                    LeftOffset = 318,
                    RightOffset = 0,
                    TopOffset = 25,
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
                WinWidth = 52,
                OnClick = DeleteSphereClick
            };
            LayoutSession.EndRead(Sel7);
            DeathLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DeathLbl",
                Priority = 0,
                WinLeft = 135,
                WinTop = 213,
                CaptureInput = false,
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
                    TopOffset = 30,
                    BottomOffset = 5
                },
                Masked = true,
                Text = "Death",
                TextColor = 8421504
            };
            LayoutSession.EndRead(DeathLbl);
            EarthLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "EarthLbl",
                Priority = 0,
                WinLeft = 31,
                WinTop = 173,
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
                    RightOffset = 240,
                    TopOffset = 30,
                    BottomOffset = 45
                },
                Masked = true,
                Text = "Earth",
                TextColor = 8421504
            };
            LayoutSession.EndRead(EarthLbl);
            WaterLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WaterLbl",
                Priority = 0,
                WinLeft = 240,
                WinTop = 173,
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
                    LeftOffset = 240,
                    RightOffset = 240,
                    TopOffset = 30,
                    BottomOffset = 45
                },
                Masked = true,
                Text = "Water",
                TextColor = 8421504
            };
            LayoutSession.EndRead(WaterLbl);
            FireLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "FireLbl",
                Priority = 0,
                WinLeft = 39,
                WinTop = 45,
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
                    LeftOffset = 0,
                    RightOffset = 240,
                    TopOffset = 45,
                    BottomOffset = 45
                },
                Masked = true,
                Text = "Fire",
                TextColor = 8421504
            };
            LayoutSession.EndRead(FireLbl);
            AirLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AirLbl",
                Priority = 0,
                WinLeft = 240,
                WinTop = 45,
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
                    LeftOffset = 240,
                    RightOffset = 240,
                    TopOffset = 45,
                    BottomOffset = 45
                },
                Masked = true,
                Text = "Air",
                TextColor = 8421504
            };
            LayoutSession.EndRead(AirLbl);
            SelectedSpheresLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SelectedSpheresLbl",
                Priority = 0,
                WinLeft = 165,
                WinTop = 289,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 289,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Selected Spheres",
                TextColor = 9402970
            };
            LayoutSession.EndRead(SelectedSpheresLbl);
            FaceNextBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "FaceNextBtn",
                Priority = 0,
                WinLeft = 235,
                WinTop = 190,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Views the next portrait."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 190,
                    BottomOffset = 13
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = ">",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 60,
                OnClick = Layout_FaceNextBtnClick_18
            };
            LayoutSession.EndRead(FaceNextBtn);
            FacePrevBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "FacePrevBtn",
                Priority = 0,
                WinLeft = 165,
                WinTop = 190,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Views the previous portrait."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 190,
                    BottomOffset = 13
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 35,
                ILIndexLit = 38,
                ILIndexUp = 32,
                Text = "<",
                TextColor = 15134446,
                TextSelColor = 0,
                WinWidth = 60,
                OnClick = Layout_FacePrevBtnClick_19
            };
            LayoutSession.EndRead(FacePrevBtn);
            LeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeaderLbl",
                Priority = 0,
                WinLeft = 38,
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
                Masked = true,
                Text = "Leader of the",
                TextColor = 12632256
            };
            LayoutSession.EndRead(LeaderLbl);
            SideNameLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SideNameLbl",
                Priority = 0,
                WinLeft = 47,
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
                Text = "#######",
                TextColor = 12632256
            };
            LayoutSession.EndRead(SideNameLbl);
            SelectedAbilitiesCost = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "SelectedAbilitiesCost",
                OnMouseDown = SelectedAbilitiesCostMouseDown,
                Priority = 10001,
                WinLeft = 180,
                WinTop = 252,
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
                    LeftOffPercent = 37,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 5,
                    TopOffset = 252,
                    BottomOffset = 41
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 87,
                BorderWidth = 6,
                BorderHeight = 4,
                OnChange = Layout_SelectedAbilitiesCostChange_20,
                Selected = -1,
                TextColor = 8421504,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(SelectedAbilitiesCost);
            AvailableAbilitiesCost = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "AvailableAbilitiesCost",
                OnMouseDown = AvailableAbilitiesMouseDown,
                Priority = 100001,
                WinLeft = 405,
                WinTop = 64,
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
                    LeftOffPercent = 87,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 64,
                    BottomOffset = 41
                },
                Masked = false,
                WinWidth = 45,
                WinHeight = 275,
                BorderWidth = 6,
                BorderHeight = 4,
                OnChange = Layout_AvailableAbilitiesCostChange_21,
                Selected = -1,
                TextColor = 8421504,
                TextSelColor = 0,
                ILIndexFrame = 64,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(AvailableAbilitiesCost);
            AttUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AttUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_AttUpClick_22
            };
            LayoutSession.EndRead(AttUp);
            AttDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AttDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_AttDnClick_23
            };
            LayoutSession.EndRead(AttDn);
            DamUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DamUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_DamUpClick_24
            };
            LayoutSession.EndRead(DamUp);
            DamDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DamDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_DamDnClick_25
            };
            LayoutSession.EndRead(DamDn);
            DefUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_DefUpClick_26
            };
            LayoutSession.EndRead(DefUp);
            DefDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DefDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_DefDnClick_27
            };
            LayoutSession.EndRead(DefDn);
            ResUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ResUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_ResUpClick_28
            };
            LayoutSession.EndRead(ResUp);
            ResDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ResDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_ResDnClick_29
            };
            LayoutSession.EndRead(ResDn);
            HitUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HitUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_HitUpClick_30
            };
            LayoutSession.EndRead(HitUp);
            HitDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HitDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_HitDnClick_31
            };
            LayoutSession.EndRead(HitDn);
            MovUp = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MovUp",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_MovUpClick_32
            };
            LayoutSession.EndRead(MovUp);
            MovDn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MovDn",
                Priority = 0,
                WinLeft = 206,
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
                OnClick = Layout_MovDnClick_33
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
            SBase = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SBase",
                Priority = 300,
                WinLeft = 10,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 1,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Base Value",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 115,
                OnClick = Layout_SBaseClick_34
            };
            LayoutSession.EndRead(SBase);
            SExtra = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SExtra",
                Priority = 300,
                WinLeft = 125,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 125,
                    RightOffset = 1,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Extra",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 50,
                OnClick = Layout_SExtraClick_35
            };
            LayoutSession.EndRead(SExtra);
            SFinal = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SFinal",
                Priority = 300,
                WinLeft = 175,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 175,
                    RightOffset = 1,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Final",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 50,
                OnClick = Layout_SFinalClick_36
            };
            LayoutSession.EndRead(SFinal);
            SelectedAbilitiesSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelectedAbilitiesSort",
                Priority = 300,
                WinLeft = 10,
                WinTop = 239,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 1,
                    TopOffset = 239,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Ability",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 170,
                OnClick = Layout_SelectedAbilitiesSortClick_37
            };
            LayoutSession.EndRead(SelectedAbilitiesSort);
            SelectedAbilitiesCostSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SelectedAbilitiesCostSort",
                Priority = 300,
                WinLeft = 180,
                WinTop = 239,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 180,
                    RightOffset = 1,
                    TopOffset = 239,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Cost",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 45,
                OnClick = Layout_SelectedAbilitiesCostSortClick_38
            };
            LayoutSession.EndRead(SelectedAbilitiesCostSort);
            AvailableAbilitiesSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AvailableAbilitiesSort",
                Priority = 300,
                WinLeft = 235,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 235,
                    RightOffset = 1,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Ability",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 170,
                OnClick = Layout_AvailableAbilitiesSortClick_39
            };
            LayoutSession.EndRead(AvailableAbilitiesSort);
            AvailableAbilitiesCostSort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AvailableAbilitiesCostSort",
                Priority = 300,
                WinLeft = 405,
                WinTop = 50,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 405,
                    RightOffset = 1,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 52,
                ILIndexLit = 49,
                ILIndexUp = 49,
                Text = "Cost",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 45,
                OnClick = Layout_AvailableAbilitiesCostSortClick_40
            };
            LayoutSession.EndRead(AvailableAbilitiesCostSort);
            MainFaceFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "MainFaceFr",
                OnPreDraw = MainFaceFrPreDraw,
                Priority = 10,
                WinLeft = 50,
                WinTop = 55,
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
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 50,
                WinHeight = 65,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(MainFaceFr);
            FFr1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr1",
                Tag = -3,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 28,
                WinTop = 50,
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
                    LeftOffset = 412,
                    RightOffset = 412,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 20,
                WinHeight = 26,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr1);
            FFr7 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr7",
                Tag = 3,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 412,
                WinTop = 50,
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
                    LeftOffset = 412,
                    RightOffset = 412,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 20,
                WinHeight = 26,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr7);
            FFr2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr2",
                Tag = -2,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 55,
                WinTop = 50,
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
                    LeftOffset = 370,
                    RightOffset = 370,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 35,
                WinHeight = 45,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr2);
            FFr6 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr6",
                Tag = 2,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 370,
                WinTop = 50,
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
                    LeftOffset = 370,
                    RightOffset = 370,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 35,
                WinHeight = 45,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr6);
            FFr3 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr3",
                Tag = -1,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 98,
                WinTop = 50,
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
                    LeftOffset = 290,
                    RightOffset = 290,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 72,
                WinHeight = 92,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr3);
            FFr5 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr5",
                Tag = 1,
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 290,
                WinTop = 50,
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
                    LeftOffset = 290,
                    RightOffset = 290,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 72,
                WinHeight = 92,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr5);
            FFr4 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "FFr4",
                OnDraw = DrawSelectionFace,
                Priority = 10,
                WinLeft = 179,
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
                    LeftOffset = 290,
                    RightOffset = 290,
                    TopOffset = 50,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 103,
                WinHeight = 128,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 64,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(FFr4);
            Phase2Header = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Phase2Header",
                Visible = false,
                Priority = 10,
                WinLeft = 219,
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
                Masked = true,
                Text = "##########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Phase2Header);
            Phase3Header = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Phase3Header",
                Visible = false,
                Priority = 10,
                WinLeft = 233,
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
                Masked = true,
                Text = "##########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(Phase3Header);
            LeaderPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeaderPnl",
                Visible = false,
                Priority = 10,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 4,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(LeaderPnl);
            RaceCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "RaceCB",
                Tag = 5,
                Priority = 150,
                WinLeft = 159,
                WinTop = 295,
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
                    LeftOffset = 185,
                    RightOffset = 7,
                    TopOffset = 295,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 143,
                BorderWidth = 6,
                BorderHeight = 4,
                ListBorderWidth = 6,
                ListBorderHeight = 2,
                NumberVisible = 5,
                OnChange = Layout_RaceCBChange_41,
                Selected = -1,
                PriorityOpened = 150,
                TextColor = 15134446,
                TextSelColor = 16777215,
                ILEIndex = 26,
                ILFIndex = 64,
                ILTnIndex = 4,
                ILTsIndex = 16,
                ItemHeight = 13
            };
            LayoutSession.EndRead(RaceCB);
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
            SelectedAbilities.AOWWindow = Phase2Pnl;
            Phase1Header.AOWWindow = LeaderPnl;
            Phase2Pnl.AOWWindow = LeaderPnl;
            AvailableAbilities.AOWWindow = Phase2Pnl;
            NextBtn.AOWWindow = ButtonPnl;
            PrevBtn.AOWWindow = ButtonPnl;
            ClearBtn.AOWWindow = ButtonPnl;
            DefaultBtn.AOWWindow = ButtonPnl;
            LMainPnl.AOWWindow = LeaderPnl;
            EditStatisticsPnl.AOWWindow = Phase2Pnl;
            AttValue.AOWWindow = AttPnl;
            AttLbl.AOWWindow = AttPnl;
            AttPnl.AOWWindow = EditStatisticsPnl;
            DamValue.AOWWindow = DamPnl;
            DamLbl.AOWWindow = DamPnl;
            DamPnl.AOWWindow = EditStatisticsPnl;
            DefValue.AOWWindow = DefPnl;
            DefLbl.AOWWindow = DefPnl;
            DefPnl.AOWWindow = EditStatisticsPnl;
            ResValue.AOWWindow = ResPnl;
            ResLbl.AOWWindow = ResPnl;
            ResPnl.AOWWindow = EditStatisticsPnl;
            HitValue.AOWWindow = HitPnl;
            HitLbl.AOWWindow = HitPnl;
            HitPnl.AOWWindow = EditStatisticsPnl;
            MovValue.AOWWindow = MovPnl;
            MovLbl.AOWWindow = MovPnl;
            MovPnl.AOWWindow = EditStatisticsPnl;
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
            SkillPointsLbl.AOWWindow = Phase2Pnl;
            UAttIcn.AOWWindow = StatPnl;
            UDefIcn.AOWWindow = StatPnl;
            UMovIcn.AOWWindow = StatPnl;
            UDamIcn.AOWWindow = StatPnl;
            UResIcn.AOWWindow = StatPnl;
            UHitIcn.AOWWindow = StatPnl;
            UAttack.AOWWindow = UStatP1;
            UDefense.AOWWindow = UStatP1;
            UMoves.AOWWindow = UStatP1;
            UDamage.AOWWindow = UStatP2;
            UResistance.AOWWindow = UStatP2;
            UHits.AOWWindow = UStatP2;
            StatPnl.AOWWindow = LMainPnl;
            UAbilities.AOWWindow = AbilityPnl;
            UAbilities.VScrollBar = UAbilSB;
            AbilityPnl.AOWWindow = LMainPnl;
            UStatP1.AOWWindow = StatPnl;
            UStatP2.AOWWindow = StatPnl;
            LeaderName.AOWWindow = LMainPnl;
            ShieldL.AOWWindow = LMainPnl;
            ShieldR.AOWWindow = LMainPnl;
            ButtonPnl.AOWWindow = LeaderPnl;
            LeftPnl.AOWWindow = Phase2Pnl;
            RightPnl.AOWWindow = Phase2Pnl;
            LeaderStatsLbl.AOWWindow = LeftPnl;
            LeaderAbilLbl.AOWWindow = LeftPnl;
            FullAbilLbl.AOWWindow = RightPnl;
            RemoveAbility.AOWWindow = Phase2Pnl;
            AddAbility.AOWWindow = Phase2Pnl;
            MainFace.AOWWindow = LMainPnl;
            Phase1Pnl.AOWWindow = LeaderPnl;
            Face4.AOWWindow = Phase1Pnl;
            Face5.AOWWindow = Phase1Pnl;
            Face6.AOWWindow = Phase1Pnl;
            Face7.AOWWindow = Phase1Pnl;
            Face3.AOWWindow = Phase1Pnl;
            Face2.AOWWindow = Phase1Pnl;
            Face1.AOWWindow = Phase1Pnl;
            NameEdit.AOWWindow = Phase1Pnl;
            NameLbl.AOWWindow = Phase1Pnl;
            FaceLbl.AOWWindow = Phase1Pnl;
            RaceLbl.AOWWindow = Phase1Pnl;
            UAbilSB.AOWWindow = AbilityPnl;
            AvailableAbilitiesLB.AOWWindow = Phase2Pnl;
            SelectedAbilitiesLB.AOWWindow = Phase2Pnl;
            Phase3Pnl.AOWWindow = LeaderPnl;
            SpheresLbl.AOWWindow = Phase3Pnl;
            SelDeath.AOWWindow = SpherePnl;
            SpherePnl.AOWWindow = Phase3Pnl;
            SelLife.AOWWindow = SpherePnl;
            SelFire.AOWWindow = SpherePnl;
            SelEarth.AOWWindow = SpherePnl;
            SelAir.AOWWindow = SpherePnl;
            SelWater.AOWWindow = SpherePnl;
            LifeLbl.AOWWindow = SpherePnl;
            Sel4.AOWWindow = SphereSelectionPnl;
            SphereSelectionPnl.AOWWindow = Phase3Pnl;
            Sel3.AOWWindow = SphereSelectionPnl;
            Sel2.AOWWindow = SphereSelectionPnl;
            Sel1.AOWWindow = SphereSelectionPnl;
            Sel5.AOWWindow = SphereSelectionPnl;
            Sel6.AOWWindow = SphereSelectionPnl;
            Sel7.AOWWindow = SphereSelectionPnl;
            DeathLbl.AOWWindow = SpherePnl;
            EarthLbl.AOWWindow = SpherePnl;
            WaterLbl.AOWWindow = SpherePnl;
            FireLbl.AOWWindow = SpherePnl;
            AirLbl.AOWWindow = SpherePnl;
            SelectedSpheresLbl.AOWWindow = Phase3Pnl;
            FaceNextBtn.AOWWindow = Phase1Pnl;
            FacePrevBtn.AOWWindow = Phase1Pnl;
            LeaderLbl.AOWWindow = LMainPnl;
            SideNameLbl.AOWWindow = LMainPnl;
            SelectedAbilitiesCost.AOWWindow = Phase2Pnl;
            SelectedAbilitiesCost.VScrollBar = SelectedAbilitiesLB;
            AvailableAbilitiesCost.AOWWindow = Phase2Pnl;
            AvailableAbilitiesCost.VScrollBar = AvailableAbilitiesLB;
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
            SBase.AOWWindow = Phase2Pnl;
            SExtra.AOWWindow = Phase2Pnl;
            SFinal.AOWWindow = Phase2Pnl;
            SelectedAbilitiesSort.AOWWindow = Phase2Pnl;
            SelectedAbilitiesCostSort.AOWWindow = Phase2Pnl;
            AvailableAbilitiesSort.AOWWindow = Phase2Pnl;
            AvailableAbilitiesCostSort.AOWWindow = Phase2Pnl;
            MainFaceFr.AOWWindow = LMainPnl;
            FFr1.AOWWindow = Phase1Pnl;
            FFr7.AOWWindow = Phase1Pnl;
            FFr2.AOWWindow = Phase1Pnl;
            FFr6.AOWWindow = Phase1Pnl;
            FFr3.AOWWindow = Phase1Pnl;
            FFr5.AOWWindow = Phase1Pnl;
            FFr4.AOWWindow = Phase1Pnl;
            Phase2Header.AOWWindow = LeaderPnl;
            Phase3Header.AOWWindow = LeaderPnl;
            RaceCB.AOWWindow = Phase1Pnl;
            Loaded();
            SelectedAbilities.Loaded();
            Phase1Header.Loaded();
            Phase2Pnl.Loaded();
            AvailableAbilities.Loaded();
            NextBtn.Loaded();
            PrevBtn.Loaded();
            ClearBtn.Loaded();
            DefaultBtn.Loaded();
            LMainPnl.Loaded();
            EditStatisticsPnl.Loaded();
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
            SkillPointsLbl.Loaded();
            UAttIcn.Loaded();
            UDefIcn.Loaded();
            UMovIcn.Loaded();
            UDamIcn.Loaded();
            UResIcn.Loaded();
            UHitIcn.Loaded();
            UAttack.Loaded();
            UDefense.Loaded();
            UMoves.Loaded();
            UDamage.Loaded();
            UResistance.Loaded();
            UHits.Loaded();
            StatPnl.Loaded();
            UAbilities.Loaded();
            AbilityPnl.Loaded();
            UStatP1.Loaded();
            UStatP2.Loaded();
            LeaderName.Loaded();
            ShieldL.Loaded();
            ShieldR.Loaded();
            ButtonPnl.Loaded();
            LeftPnl.Loaded();
            RightPnl.Loaded();
            LeaderStatsLbl.Loaded();
            LeaderAbilLbl.Loaded();
            FullAbilLbl.Loaded();
            RemoveAbility.Loaded();
            AddAbility.Loaded();
            MainFace.Loaded();
            Phase1Pnl.Loaded();
            Face4.Loaded();
            Face5.Loaded();
            Face6.Loaded();
            Face7.Loaded();
            Face3.Loaded();
            Face2.Loaded();
            Face1.Loaded();
            NameEdit.Loaded();
            NameLbl.Loaded();
            FaceLbl.Loaded();
            RaceLbl.Loaded();
            UAbilSB.Loaded();
            AvailableAbilitiesLB.Loaded();
            SelectedAbilitiesLB.Loaded();
            Phase3Pnl.Loaded();
            SpheresLbl.Loaded();
            SelDeath.Loaded();
            SpherePnl.Loaded();
            SelLife.Loaded();
            SelFire.Loaded();
            SelEarth.Loaded();
            SelAir.Loaded();
            SelWater.Loaded();
            LifeLbl.Loaded();
            Sel4.Loaded();
            SphereSelectionPnl.Loaded();
            Sel3.Loaded();
            Sel2.Loaded();
            Sel1.Loaded();
            Sel5.Loaded();
            Sel6.Loaded();
            Sel7.Loaded();
            DeathLbl.Loaded();
            EarthLbl.Loaded();
            WaterLbl.Loaded();
            FireLbl.Loaded();
            AirLbl.Loaded();
            SelectedSpheresLbl.Loaded();
            FaceNextBtn.Loaded();
            FacePrevBtn.Loaded();
            LeaderLbl.Loaded();
            SideNameLbl.Loaded();
            SelectedAbilitiesCost.Loaded();
            AvailableAbilitiesCost.Loaded();
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
            SBase.Loaded();
            SExtra.Loaded();
            SFinal.Loaded();
            SelectedAbilitiesSort.Loaded();
            SelectedAbilitiesCostSort.Loaded();
            AvailableAbilitiesSort.Loaded();
            AvailableAbilitiesCostSort.Loaded();
            MainFaceFr.Loaded();
            FFr1.Loaded();
            FFr7.Loaded();
            FFr2.Loaded();
            FFr6.Loaded();
            FFr3.Loaded();
            FFr5.Loaded();
            FFr4.Loaded();
            Phase2Header.Loaded();
            Phase3Header.Loaded();
            LeaderPnl.Loaded();
            RaceCB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            SelectedAbilities.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelectedAbilities.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilities.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SelectedAbilities.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SelectedAbilities.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => SelectedAbilities.PopupMenu = module.BannerPopup);
            Phase1Header.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Phase2Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Phase2Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AvailableAbilities.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AvailableAbilities.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilities.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AvailableAbilities.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AvailableAbilities.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => AvailableAbilities.PopupMenu = module.BannerPopup);
            NextBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NextBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            PrevBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PrevBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            ClearBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ClearBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ClearBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            DefaultBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DefaultBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DefaultBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LMainPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LMainPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AttValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            AttLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AttPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AttPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DamValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            DamLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DamPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DamPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DefValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            DefLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DefPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DefPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ResValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            ResLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ResPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HitValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            HitLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HitPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HitPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MovValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            MovLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MovPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MovPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AttI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovI.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DamA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DefA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HitA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MovA.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AttP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DamP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            DefP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ResP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            HitP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            MovP.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AttB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DamB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DefB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HitB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MovB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SkillPointsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UAttIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UAttIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UDefIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDefIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UMovIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UMovIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MoveIcons;
            UDamIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UDamIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UResIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UResIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UHitIcn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            UHitIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.UnitIcons;
            UAttack.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDefense.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UMoves.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UDamage.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UResistance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            UHits.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            StatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            StatPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAbilities.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UAbilities.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UAbilities.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UAbilities.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LeaderName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ShieldL.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ShieldL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            ShieldR.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ShieldR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            ButtonPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ButtonPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LeaderStatsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LeaderAbilLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FullAbilLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RemoveAbility.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RemoveAbility.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RemoveAbility.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AddAbility.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AddAbility.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AddAbility.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MainFace.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Phase1Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Phase1Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face4.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face5.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face6.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face7.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face3.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face2.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Face1.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NameEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            NameEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            NameLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            NameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            FaceLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            FaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            RaceLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RaceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            UAbilSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UAbilSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesLB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesLB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesLB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AvailableAbilitiesLB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesLB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesLB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesLB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            SelectedAbilitiesLB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            Phase3Pnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Phase3Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SpheresLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SpheresLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SelDeath.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelDeath.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelLife.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelLife.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelFire.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelFire.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelEarth.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelEarth.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelAir.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelAir.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SelWater.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelWater.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            LifeLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LifeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Sel4.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel4.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            SphereSelectionPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Sel3.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel3.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel2.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel2.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel1.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel1.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel5.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel5.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel6.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel6.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Sel7.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Sel7.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            DeathLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DeathLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EarthLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            EarthLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            WaterLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            WaterLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FireLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            FireLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AirLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AirLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SelectedSpheresLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelectedSpheresLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            FaceNextBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            FaceNextBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FaceNextBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            FacePrevBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            FacePrevBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            FacePrevBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            LeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SideNameLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SelectedAbilitiesCost.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            SelectedAbilitiesCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesCost.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SelectedAbilitiesCost.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SelectedAbilitiesCost.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => SelectedAbilitiesCost.PopupMenu = module.BannerPopup);
            AvailableAbilitiesCost.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AvailableAbilitiesCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilitiesCost.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AvailableAbilitiesCost.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            AvailableAbilitiesCost.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            session.Defer("InfoBanner", (Aow.Units.AoWInfoBanner.TInfoBanner module) => AvailableAbilitiesCost.PopupMenu = module.BannerPopup);
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
            SBase.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SBase.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SExtra.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SExtra.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SFinal.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SFinal.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SelectedAbilitiesSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SelectedAbilitiesCostSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SelectedAbilitiesCostSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AvailableAbilitiesSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilitiesSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            AvailableAbilitiesCostSort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AvailableAbilitiesCostSort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            MainFaceFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr7.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr6.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr3.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr5.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            FFr4.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Phase2Header.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Phase3Header.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            LeaderPnl.AOWWindow = Aow.Units.E2Globals.E2Globals.TitleWindow.TitleWin;
            LeaderPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            RaceCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            RaceCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            RaceCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            RaceCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            RaceCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_LeaderSetupWinCreate_0(TObject p0)
        {
            LeaderSetupWinCreate();
        }

        private void Layout_LeaderSetupWinDestroy_1(TObject p0)
        {
            LeaderSetupWinDestroy();
        }

        private void Layout_SelectedAbilitiesChange_2(TObject p0)
        {
            SelectedAbilitiesChange();
        }

        private void Layout_AvailableAbilitiesChange_3(TObject p0)
        {
            AvailableAbilitiesChange();
        }

        private void Layout_NextBtnClick_4(TObject p0)
        {
            NextBtnClick();
        }

        private void Layout_PrevBtnClick_5(TObject p0)
        {
            PrevBtnClick();
        }

        private void Layout_ClearBtnClick_6(TObject p0)
        {
            ClearBtnClick();
        }

        private void Layout_RemoveAbilityClick_7(TObject p0)
        {
            RemoveAbilityClick();
        }

        private void Layout_AddAbilityClick_8(TObject p0)
        {
            AddAbilityClick();
        }

        private void Layout_NameEditChange_9(TObject p0)
        {
            NameEditChange();
        }

        private void Layout_AvailableAbilitiesLBChange_10(TObject p0)
        {
            AvailableAbilitiesLBChange();
        }

        private void Layout_SelectedAbilitiesLBChange_11(TObject p0)
        {
            SelectedAbilitiesLBChange();
        }

        private void Layout_SelDeathClick_12(TObject p0)
        {
            SelDeathClick();
        }

        private void Layout_SelLifeClick_13(TObject p0)
        {
            SelLifeClick();
        }

        private void Layout_SelFireClick_14(TObject p0)
        {
            SelFireClick();
        }

        private void Layout_SelEarthClick_15(TObject p0)
        {
            SelEarthClick();
        }

        private void Layout_SelAirClick_16(TObject p0)
        {
            SelAirClick();
        }

        private void Layout_SelWaterClick_17(TObject p0)
        {
            SelWaterClick();
        }

        private void Layout_FaceNextBtnClick_18(TObject p0)
        {
            FaceNextBtnClick();
        }

        private void Layout_FacePrevBtnClick_19(TObject p0)
        {
            FacePrevBtnClick();
        }

        private void Layout_SelectedAbilitiesCostChange_20(TObject p0)
        {
            SelectedAbilitiesCostChange();
        }

        private void Layout_AvailableAbilitiesCostChange_21(TObject p0)
        {
            AvailableAbilitiesCostChange();
        }

        private void Layout_AttUpClick_22(TObject p0)
        {
            AttUpClick();
        }

        private void Layout_AttDnClick_23(TObject p0)
        {
            AttDnClick();
        }

        private void Layout_DamUpClick_24(TObject p0)
        {
            DamUpClick();
        }

        private void Layout_DamDnClick_25(TObject p0)
        {
            DamDnClick();
        }

        private void Layout_DefUpClick_26(TObject p0)
        {
            DefUpClick();
        }

        private void Layout_DefDnClick_27(TObject p0)
        {
            DefDnClick();
        }

        private void Layout_ResUpClick_28(TObject p0)
        {
            ResUpClick();
        }

        private void Layout_ResDnClick_29(TObject p0)
        {
            ResDnClick();
        }

        private void Layout_HitUpClick_30(TObject p0)
        {
            HitUpClick();
        }

        private void Layout_HitDnClick_31(TObject p0)
        {
            HitDnClick();
        }

        private void Layout_MovUpClick_32(TObject p0)
        {
            MovUpClick();
        }

        private void Layout_MovDnClick_33(TObject p0)
        {
            MovDnClick();
        }

        private void Layout_SBaseClick_34(TObject p0)
        {
            SBaseClick();
        }

        private void Layout_SExtraClick_35(TObject p0)
        {
            SExtraClick();
        }

        private void Layout_SFinalClick_36(TObject p0)
        {
            SFinalClick();
        }

        private void Layout_SelectedAbilitiesSortClick_37(TObject p0)
        {
            SelectedAbilitiesSortClick();
        }

        private void Layout_SelectedAbilitiesCostSortClick_38(TObject p0)
        {
            SelectedAbilitiesCostSortClick();
        }

        private void Layout_AvailableAbilitiesSortClick_39(TObject p0)
        {
            AvailableAbilitiesSortClick();
        }

        private void Layout_AvailableAbilitiesCostSortClick_40(TObject p0)
        {
            AvailableAbilitiesCostSortClick();
        }

        private void Layout_RaceCBChange_41(TObject p0)
        {
            RaceCBChange();
        }
    }
}
