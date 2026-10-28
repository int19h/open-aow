// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWPInfoWin
{
    public partial class TPInfoW
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "PInfoW";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            PInfoWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "PInfoWin",
                Visible = false,
                Priority = 10,
                WinLeft = 440,
                WinTop = 216,
                WinWidth = 200,
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
                    TopOffset = 0,
                    BottomOffset = 48
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
                OnDrag = Layout_PInfoWinResize_0,
                OnResize = Layout_PInfoWinResize_0,
                MinWidth = 200,
                MinHeight = 216,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 216,
                Dockable = false
            };
            LayoutSession.EndRead(PInfoWin);
            Min = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Min",
                Priority = 0,
                WinLeft = 181,
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
                    RightOffset = 4,
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
                Text = "Events",
                TextColor = 0
            };
            LayoutSession.EndRead(IWTitle);
            SIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SIcon",
                Priority = 0,
                WinLeft = 2,
                WinTop = 2,
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
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SIcon);
            EventList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "EventList",
                OnMouseDown = EventListMouseDown,
                Priority = 100,
                WinLeft = 1,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 187,
                WinHeight = 108,
                BorderWidth = 37,
                BorderHeight = 4,
                OnDoubleClick = Layout_EventListDoubleClick_2,
                Selected = -1,
                OnDrawItem = EventListDrawItem,
                OnPreDrawItem = EventListPreDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(EventList);
            LeadL = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "LeadL",
                Priority = 5,
                WinLeft = 80,
                WinTop = 18,
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
                    TopOffset = 18,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "of the",
                TextColor = 9415072
            };
            LayoutSession.EndRead(LeadL);
            Turn = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Turn",
                Priority = 5,
                WinLeft = 77,
                WinTop = 50,
                LayoutToolTipText = new LayoutStrings()
                {
                    "The current day."
                },
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
                    TopOffset = 20,
                    BottomOffset = 5
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Turn);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Priority = 3000,
                WinLeft = 6,
                WinTop = 6,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 5,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 0,
                    TopOffset = 44,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 8,
                AnimEnd = 38,
                WinWidth = 40,
                WinHeight = 52,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RShield);
            EventFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "EventFr",
                Priority = 0,
                WinLeft = 6,
                WinTop = 100,
                WinWidth = 188,
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
                    LeftOffset = 6,
                    RightOffset = 6,
                    TopOffset = 100,
                    BottomOffset = 6
                },
                WinHeight = 110,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(EventFr);
            EventSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "EventSB",
                Visible = false,
                Priority = 100,
                WinLeft = 171,
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
                Max = 5,
                SmallChange = 1,
                LargeChange = 5,
                Position = 0,
                WinHeight = 108,
                WinWidth = 17,
                NumberVisible = 5,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(EventSB);
            PInfoPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PInfoPnl",
                Priority = 50000,
                WinLeft = 6,
                WinTop = 19,
                WinWidth = 188,
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
                    LeftOffset = 6,
                    RightOffset = 6,
                    TopOffset = 19,
                    BottomOffset = 0
                },
                WinHeight = 66,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PInfoPnl);
            LeaderCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "LeaderCB",
                Tag = 1,
                Visible = false,
                Priority = 6000,
                WinLeft = 1,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Your Leader's portrait. Click to open Unit Window for your leader."
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
                    TopOffPercent = 2,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(LeaderCB);
            FaceFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "FaceFr",
                OnMouseDown = FaceFrMouseDown,
                OnDraw = FaceFrDraw,
                Priority = 0,
                WinLeft = 1,
                WinTop = 1,
                WinWidth = 52,
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                WinHeight = 64,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(FaceFr);
            EventLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "EventLbl",
                Priority = 0,
                WinLeft = 69,
                WinTop = 88,
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
                    TopOffset = 88,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Event History",
                TextColor = 0
            };
            LayoutSession.EndRead(EventLbl);
            ShieldFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ShieldFr",
                Priority = 0,
                WinLeft = 136,
                WinTop = 1,
                WinWidth = 52,
                Masked = false,
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                WinHeight = 64,
                ILIndexFrame = 113,
                ILIndexTexture = 5,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ShieldFr);
            TurnFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TurnFr",
                Priority = 0,
                WinLeft = 52,
                WinTop = 45,
                WinWidth = 84,
                Masked = false,
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
                    LeftOffset = 52,
                    RightOffset = 52,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TurnFr);
            InfoFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "InfoFr",
                Priority = 0,
                WinLeft = 52,
                WinTop = 0,
                WinWidth = 84,
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
                    LeftOffset = 52,
                    RightOffset = 52,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 46,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(InfoFr);
            Player = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Player",
                Priority = 5,
                WinLeft = 70,
                WinTop = 6,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Your leader's name."
                },
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
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Player);
            Race = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Race",
                Priority = 5,
                WinLeft = 69,
                WinTop = 31,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Your side's race."
                },
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
                    TopOffset = 31,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Race);
            RaceCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "RaceCB",
                Tag = 1,
                Visible = false,
                Priority = 6000,
                WinLeft = 1,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Your race's shield. Click to opens the Race Info Window."
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
                    TopOffPercent = 2,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 41
                },
                Masked = true,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 42,
                ILIndexLit = 43,
                ILIndexUp = 43,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 52,
                Checked = false,
                RadioIndex = 1
            };
            LayoutSession.EndRead(RaceCB);
            Center = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Center",
                Priority = 0,
                WinLeft = 167,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Centers the World Map on the Player's Leader."
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
                ILIndexDown = 7,
                ILIndexLit = 8,
                ILIndexUp = 6,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = LeaderBtnClick
            };
            LayoutSession.EndRead(Center);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Priority = 0,
                WinLeft = 153,
                WinTop = 1,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Displays Help for this window."
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
                ILIndexDown = 60,
                ILIndexLit = 61,
                ILIndexUp = 59,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_HelpClick_3
            };
            LayoutSession.EndRead(Help);
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
            Min.AOWWindow = PInfoWin;
            IWTitle.AOWWindow = PInfoWin;
            SIcon.AOWWindow = PInfoWin;
            EventList.AOWWindow = EventFr;
            EventList.VScrollBar = EventSB;
            LeadL.AOWWindow = PInfoPnl;
            Turn.AOWWindow = PInfoPnl;
            RShield.AOWWindow = ShieldFr;
            EventFr.AOWWindow = PInfoWin;
            EventSB.AOWWindow = EventFr;
            PInfoPnl.AOWWindow = PInfoWin;
            LeaderCB.AOWWindow = PInfoPnl;
            FaceFr.AOWWindow = PInfoPnl;
            EventLbl.AOWWindow = PInfoWin;
            ShieldFr.AOWWindow = PInfoPnl;
            TurnFr.AOWWindow = PInfoPnl;
            InfoFr.AOWWindow = PInfoPnl;
            Player.AOWWindow = PInfoPnl;
            Race.AOWWindow = PInfoPnl;
            RaceCB.AOWWindow = PInfoPnl;
            Center.AOWWindow = PInfoWin;
            Help.AOWWindow = PInfoWin;
            Loaded();
            PInfoWin.Loaded();
            Min.Loaded();
            IWTitle.Loaded();
            SIcon.Loaded();
            EventList.Loaded();
            LeadL.Loaded();
            Turn.Loaded();
            RShield.Loaded();
            EventFr.Loaded();
            EventSB.Loaded();
            PInfoPnl.Loaded();
            LeaderCB.Loaded();
            FaceFr.Loaded();
            EventLbl.Loaded();
            ShieldFr.Loaded();
            TurnFr.Loaded();
            InfoFr.Loaded();
            Player.Loaded();
            Race.Loaded();
            RaceCB.Loaded();
            Center.Loaded();
            Help.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            PInfoWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            PInfoWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PInfoWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Min.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Min.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            IWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            EventList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            EventList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EventList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EventList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            LeadL.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Turn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Turn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            EventFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EventSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EventSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EventSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EventSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PInfoPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PInfoPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LeaderCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LeaderCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            FaceFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EventLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            ShieldFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ShieldFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CityWin;
            TurnFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            InfoFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Player.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Player.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Race.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Race.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            RaceCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RaceCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            Center.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Center.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_PInfoWinResize_0(TObject p0)
        {
            PInfoWinResize();
        }

        private void Layout_MinClick_1(TObject p0)
        {
            MinClick();
        }

        private void Layout_EventListDoubleClick_2(TObject p0)
        {
            EventListDoubleClick();
        }

        private void Layout_HelpClick_3(TObject p0)
        {
            HelpClick();
        }
    }
}
