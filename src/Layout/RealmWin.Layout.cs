// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWRealm
{
    public partial class TRealmWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "RealmWin";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 10,
                WinLeft = 0,
                WinTop = 272,
                WinWidth = 440,
                Masked = false,
                OnHide = Layout_DlgHide_0,
                OnShow = Layout_DlgShow_1,
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
                UseVMem = true,
                BorderWidth = 4,
                BorderHeight = 4,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = true,
                OnDrag = Layout_OverWindowResize_2,
                OnResize = Layout_OverWindowResize_2,
                MinWidth = 440,
                MinHeight = 160,
                MaxWidth = 1280,
                MaxHeight = 1024,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 160,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            OWTitle = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "OWTitle",
                Priority = 0,
                WinLeft = 23,
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
                    LeftOffset = 23,
                    RightOffset = 0,
                    TopOffset = 4,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Realm Overview",
                TextColor = 0
            };
            LayoutSession.EndRead(OWTitle);
            OWTitleIc = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "OWTitleIc",
                Priority = 0,
                WinLeft = 2,
                WinTop = 2,
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
                CurrentImage = 0,
                AnimEnd = 0,
                WinWidth = 18,
                WinHeight = 15,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(OWTitleIc);
            HeaderTab = new Aow.Units.AoWTabPanel.TAOWTabPanel(this)
            {
                Name = "HeaderTab",
                Priority = 1000,
                WinLeft = 4,
                WinTop = 19,
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
                    LeftOffset = 4,
                    RightOffset = 3,
                    TopOffset = 19,
                    BottomOffset = 0
                },
                Masked = true,
                LayoutItems = new LayoutStrings()
                {
                    "GOLD",
                    "CITIES",
                    "PLACES",
                    "HEROES"
                },
                BorderHeight = 6,
                OnChange = Layout_HeaderTabChange_3,
                Selected = 0,
                WinWidth = 433,
                TextColor = 0,
                TextSelColor = 9415072,
                ILIndexDown = 67,
                ILIndexUp = 70,
                BorderHeightSelected = 7
            };
            LayoutSession.EndRead(HeaderTab);
            T1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T1Pnl",
                Priority = 1,
                WinLeft = 8,
                WinTop = 44,
                WinWidth = 424,
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
                    LeftOffset = 8,
                    RightOffset = 8,
                    TopOffset = 44,
                    BottomOffset = 5
                },
                WinHeight = 111,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T1Pnl);
            T2Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2Pnl",
                Visible = false,
                Priority = 1,
                WinLeft = 8,
                WinTop = 44,
                WinWidth = 424,
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
                    LeftOffset = 8,
                    RightOffset = 8,
                    TopOffset = 44,
                    BottomOffset = 5
                },
                WinHeight = 111,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2Pnl);
            T2LBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2LBPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 422,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 107,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2LBPnl);
            T3Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3Pnl",
                Visible = false,
                Priority = 1,
                WinLeft = 8,
                WinTop = 44,
                WinWidth = 424,
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
                    LeftOffset = 8,
                    RightOffset = 8,
                    TopOffset = 44,
                    BottomOffset = 5
                },
                WinHeight = 111,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 2,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3Pnl);
            T3LBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3LBPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 422,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 107,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3LBPnl);
            Minimize = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Minimize",
                Priority = 0,
                WinLeft = 421,
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
                OnClick = Layout_OWMinIconClick_4
            };
            LayoutSession.EndRead(Minimize);
            T4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T4Pnl",
                Visible = false,
                Priority = 1,
                WinLeft = 8,
                WinTop = 45,
                WinWidth = 424,
                Masked = true,
                OnHide = T4PnlHide,
                OnShow = T4PnlShow,
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
                    LeftOffset = 8,
                    RightOffset = 8,
                    TopOffset = 45,
                    BottomOffset = 5
                },
                WinHeight = 110,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T4Pnl);
            T4LBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T4LBPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 422,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 106,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T4LBPnl);
            T2NameLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2NameLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
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
                    RightOffPercent = 73,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 113,
                WinHeight = 92,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2NameLB);
            T2RaceLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2RaceLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
                WinLeft = 113,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 27,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 221,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 88,
                WinHeight = 92,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2RaceLB);
            T2IncomeLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2IncomeLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
                WinLeft = 200,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 47,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 196,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2IncomeLB);
            T2LevelLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2LevelLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
                WinLeft = 225,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 52,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 171,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2LevelLB);
            T2WallLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2WallLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
                WinLeft = 250,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 59,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 146,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                OnDrawItem = T2WallLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T2WallLB);
            T2SizeLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2SizeLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 0,
                WinLeft = 275,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 65,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 121,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2SizeLB);
            T2EncLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2EncLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 400,
                WinLeft = 375,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 89,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 47,
                WinHeight = 92,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 16777215,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T2EncLB);
            T2SName = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SName",
                Priority = 1000,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Name."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 73,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "City Name",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 113,
                OnClick = Layout_T2SNameClick_6
            };
            LayoutSession.EndRead(T2SName);
            T2SRace = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SRace",
                Priority = 1000,
                WinLeft = 113,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by the Race of their dwellers."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 27,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 221,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Population",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 88,
                OnClick = Layout_T2SRaceClick_7
            };
            LayoutSession.EndRead(T2SRace);
            T2SIn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SIn",
                Priority = 1000,
                WinLeft = 200,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Income."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 47,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 196,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Inc",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SInClick_8
            };
            LayoutSession.EndRead(T2SIn);
            T2SWall = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SWall",
                Priority = 1000,
                WinLeft = 250,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Wall Strength."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 59,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 146,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Wal",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SWallClick_9
            };
            LayoutSession.EndRead(T2SWall);
            T2SLvl = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SLvl",
                Priority = 1000,
                WinLeft = 225,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Upgrade Level."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 53,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 171,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Lvl",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SLvlClick_10
            };
            LayoutSession.EndRead(T2SLvl);
            T2SSize = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SSize",
                Priority = 1000,
                WinLeft = 275,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Size."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 65,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 121,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Siz",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SSizeClick_11
            };
            LayoutSession.EndRead(T2SSize);
            T2SEnc = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SEnc",
                Priority = 1000,
                WinLeft = 375,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Enchantment."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 89,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Enc",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 47,
                OnClick = Layout_T2SEncClick_12
            };
            LayoutSession.EndRead(T2SEnc);
            T2ActLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2ActLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 300,
                WinLeft = 350,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 83,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 46,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                OnDrawItem = T2ActLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T2ActLB);
            T2SAct = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SAct",
                Priority = 1000,
                WinLeft = 350,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Activity."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 83,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 46,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Act",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SActClick_13
            };
            LayoutSession.EndRead(T2SAct);
            T2RelLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2RelLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 100,
                WinLeft = 300,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 71,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 96,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                OnDrawItem = T2RelLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T2RelLB);
            T2SRel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SRel",
                Priority = 1000,
                WinLeft = 300,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Race Relation."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 71,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 96,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Rel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SRelClick_14
            };
            LayoutSession.EndRead(T2SRel);
            T2MorLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T2MorLB",
                OnMouseDown = Layout_CityListMouseDown_5,
                Priority = 100,
                WinLeft = 325,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 77,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 71,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 26,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = CityListChange,
                OnDoubleClick = CityDoubleClick,
                Selected = -1,
                OnDrawItem = T2MorLBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T2MorLB);
            T2SMor = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T2SMor",
                Priority = 1000,
                WinLeft = 325,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Cities by Morale."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 77,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 71,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Mor",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 26,
                OnClick = Layout_T2SMorClick_15
            };
            LayoutSession.EndRead(T2SMor);
            CitySB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "CitySB",
                Priority = 400,
                WinLeft = 405,
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
                Max = 10,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 92,
                WinWidth = 17,
                NumberVisible = 4,
                OnChange = Layout_T2SBChange_16,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(CitySB);
            T3Name = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T3Name",
                OnMouseDown = Layout_PlacesListMouseDown_17,
                Priority = 0,
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
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 209,
                WinHeight = 92,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = T3NameChange,
                OnDoubleClick = PlaceListDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T3Name);
            T3SName = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T3SName",
                Priority = 1000,
                WinLeft = 1,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Structures by Name."
                },
                CaptureInput = true,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Structure Type",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 209,
                OnClick = Layout_T3SNameClick_18
            };
            LayoutSession.EndRead(T3SName);
            T3ActLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T3ActLB",
                OnMouseDown = Layout_PlacesListMouseDown_17,
                Priority = 0,
                WinLeft = 209,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 49,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 213,
                WinHeight = 92,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = T3NameChange,
                OnDoubleClick = PlaceListDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T3ActLB);
            T3Activity = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T3Activity",
                Priority = 1000,
                WinLeft = 209,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Structures by Activity."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 49,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Activity",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 213,
                OnClick = Layout_T3ActivityClick_19
            };
            LayoutSession.EndRead(T3Activity);
            PlaceSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PlaceSB",
                Visible = false,
                Priority = 0,
                WinLeft = 405,
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
                Max = 3,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 92,
                WinWidth = 17,
                NumberVisible = 3,
                OnChange = Layout_T3SBChange_20,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(PlaceSB);
            UnitFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "UnitFr",
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
                WinWidth = 43,
                WinHeight = 48,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 12,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(UnitFr);
            UnitGg = new Aow.Units.AOWGauge.TAoWGauge(this)
            {
                Name = "UnitGg",
                Visible = false,
                Priority = 5,
                WinLeft = 12,
                WinTop = 101,
                CaptureInput = false,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 88,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 12,
                    RightOffset = 12,
                    TopOffset = 0,
                    BottomOffset = 3
                },
                Masked = false,
                WinWidth = 27,
                WinHeight = 2,
                Min = 0,
                Max = 1,
                Position = 0,
                ILIndexFrame = 0,
                ILIndexTNormal = 4,
                ILIndexTSelected = 5
            };
            LayoutSession.EndRead(UnitGg);
            Unit1LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit1LB",
                Priority = 0,
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
                    RightOffPercent = 88,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 50,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit1LB);
            Unit2LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit2LB",
                Tag = 1,
                Priority = 0,
                WinLeft = 52,
                WinTop = 1,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 12,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 52,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit2LB);
            Unit3LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit3LB",
                Tag = 2,
                Priority = 0,
                WinLeft = 105,
                WinTop = 1,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 52,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit3LB);
            Unit4LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit4LB",
                Tag = 3,
                Priority = 0,
                WinLeft = 158,
                WinTop = 1,
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
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 51,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit4LB);
            Unit5LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit5LB",
                Tag = 4,
                Priority = 0,
                WinLeft = 211,
                WinTop = 1,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 38,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 51,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit5LB);
            Unit6LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit6LB",
                Tag = 5,
                Priority = 0,
                WinLeft = 263,
                WinTop = 1,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 62,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 52,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit6LB);
            Unit7LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit7LB",
                Tag = 6,
                Priority = 0,
                WinLeft = 316,
                WinTop = 1,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 13,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 52,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit7LB);
            Unit8LB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "Unit8LB",
                Tag = 7,
                Priority = 0,
                WinLeft = 369,
                WinTop = 1,
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
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 1,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 51,
                WinHeight = 104,
                BorderWidth = 0,
                BorderHeight = 3,
                OnChange = Unit1LBChange,
                OnDoubleClick = Unit1LBDoubleClick,
                Selected = -1,
                OnDrawItem = Unit1LBDrawItem,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 17,
                ILIndexTSelected = 2,
                ItemHeight = 60,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(Unit8LB);
            PartySb = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PartySb",
                Visible = false,
                Priority = 0,
                WinLeft = 403,
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
                WinHeight = 104,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_PartyVSBChange_21,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(PartySb);
            HeroPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "HeroPnl",
                Visible = false,
                Priority = 1,
                WinLeft = 8,
                WinTop = 45,
                WinWidth = 424,
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
                    LeftOffset = 8,
                    RightOffset = 8,
                    TopOffset = 45,
                    BottomOffset = 5
                },
                WinHeight = 110,
                ILIndexFrame = 105,
                ILIndexTexture = 2,
                OwnerItem = 3,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(HeroPnl);
            T5MainPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T5MainPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 422,
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
                    LeftOffset = 1,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 106,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T5MainPnl);
            T5Name = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T5Name",
                OnMouseDown = HeroListMouseDown,
                Priority = 0,
                WinLeft = 0,
                WinTop = 14,
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
                    RightOffset = 124,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 230,
                WinHeight = 91,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = HeroListChange,
                OnDoubleClick = HeroListDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(T5Name);
            T5Health = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T5Health",
                OnMouseDown = HeroListMouseDown,
                Priority = 0,
                WinLeft = 229,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 65,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 53,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 72,
                WinHeight = 91,
                BorderWidth = 4,
                BorderHeight = 3,
                OnChange = HeroListChange,
                OnDoubleClick = HeroListDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T5Health);
            T5Level = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "T5Level",
                OnMouseDown = HeroListMouseDown,
                Priority = 0,
                WinLeft = 300,
                WinTop = 14,
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
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 54,
                WinHeight = 91,
                BorderWidth = 3,
                BorderHeight = 3,
                OnChange = HeroListChange,
                OnDoubleClick = HeroListDoubleClick,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(T5Level);
            T5SName = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T5SName",
                Priority = 1000,
                WinLeft = 0,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Heroes by Name."
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
                    LeftOffset = 0,
                    RightOffset = 124,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Hero Name",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 230,
                OnClick = Layout_T5SNameClick_22
            };
            LayoutSession.EndRead(T5SName);
            T5SHealth = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T5SHealth",
                Priority = 1000,
                WinLeft = 229,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Heroes by Health."
                },
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 65,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 53,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Health",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 72,
                OnClick = Layout_T5SHealthClick_23
            };
            LayoutSession.EndRead(T5SHealth);
            T5SLevel = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "T5SLevel",
                Priority = 1000,
                WinLeft = 300,
                WinTop = 0,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sort Heroes by Level."
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Level",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 54,
                OnClick = Layout_T5SLevelClick_24
            };
            LayoutSession.EndRead(T5SLevel);
            T5HeroInf = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T5HeroInf",
                OnDraw = T5HeroInfDraw,
                Priority = 10,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 69,
                Masked = false,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 20
                },
                WinHeight = 86,
                ILIndexFrame = 113,
                ILIndexTexture = 2,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T5HeroInf);
            T5LBFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T5LBFr",
                Priority = 0,
                WinLeft = 70,
                WinTop = 0,
                WinWidth = 354,
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                WinHeight = 106,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T5LBFr);
            HInfoBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "HInfoBtn",
                Priority = 50,
                WinLeft = 1,
                WinTop = 85,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 85,
                    BottomOffset = 1
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 97,
                ILIndexLit = 100,
                ILIndexUp = 94,
                Text = "Info",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 69,
                OnClick = Layout_HeroInfoClick_25
            };
            LayoutSession.EndRead(HInfoBtn);
            HeroSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "HeroSB",
                Priority = 0,
                WinLeft = 337,
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
                Max = 10,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 91,
                WinWidth = 17,
                NumberVisible = 4,
                OnChange = Layout_HeroVSBChange_26,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(HeroSB);
            TotalPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TotalPnl",
                Priority = 3000,
                WinLeft = 10,
                WinTop = 6,
                WinWidth = 125,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 67,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 10,
                    RightOffset = 5,
                    TopOffset = 6,
                    BottomOffset = 8
                },
                WinHeight = 97,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TotalPnl);
            IncomePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IncomePnl",
                Priority = 3000,
                WinLeft = 149,
                WinTop = 6,
                WinWidth = 126,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 34,
                    RightOffPercent = 34,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 6,
                    BottomOffset = 8
                },
                WinHeight = 97,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(IncomePnl);
            IncomeMem = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeMem",
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
                    BottomOffset = 21
                },
                Masked = false,
                WinWidth = 125,
                WinHeight = 76,
                BorderWidth = 5,
                BorderHeight = 1,
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
            LayoutSession.EndRead(IncomeMem);
            IncomeBtm = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "IncomeBtm",
                Priority = 3000,
                WinLeft = 1,
                WinTop = 75,
                WinWidth = 125,
                Masked = true,
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
            LayoutSession.EndRead(IncomeBtm);
            IncomeSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "IncomeSB",
                Visible = false,
                Priority = 0,
                WinLeft = 109,
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
                Max = 2,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 76,
                WinWidth = 17,
                NumberVisible = 2,
                OnChange = Layout_IncomeSBChange_27,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(IncomeSB);
            IncomeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "IncomeLbl",
                Priority = 0,
                WinLeft = 5,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Income",
                TextColor = 9415072
            };
            LayoutSession.EndRead(IncomeLbl);
            Income = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Income",
                Priority = 0,
                WinLeft = 90,
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
                    LeftOffset = 5,
                    RightOffset = 20,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Income);
            UpkeepPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpkeepPnl",
                Priority = 3000,
                WinLeft = 289,
                WinTop = 6,
                WinWidth = 125,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 67,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 5,
                    RightOffset = 10,
                    TopOffset = 6,
                    BottomOffset = 8
                },
                WinHeight = 97,
                ILIndexFrame = 105,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UpkeepPnl);
            UpkeepMem = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "UpkeepMem",
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
                    BottomOffset = 21
                },
                Masked = false,
                WinWidth = 124,
                WinHeight = 76,
                BorderWidth = 5,
                BorderHeight = 1,
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
            LayoutSession.EndRead(UpkeepMem);
            UpkeepBtm = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UpkeepBtm",
                Priority = 3000,
                WinLeft = 1,
                WinTop = 75,
                WinWidth = 124,
                Masked = true,
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
            LayoutSession.EndRead(UpkeepBtm);
            UpkeepSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "UpkeepSB",
                Visible = false,
                Priority = 0,
                WinLeft = 108,
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
                WinHeight = 76,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_UpkeepSBChange_28,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(UpkeepSB);
            UpkeepLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "UpkeepLbl",
                Priority = 0,
                WinLeft = 5,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Upkeep",
                TextColor = 9415072
            };
            LayoutSession.EndRead(UpkeepLbl);
            Upkeep = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Upkeep",
                Priority = 0,
                WinLeft = 89,
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
                    LeftOffset = 5,
                    RightOffset = 20,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Upkeep);
            Totallbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Totallbl",
                Priority = 0,
                WinLeft = 5,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Net Income",
                TextColor = 9415072
            };
            LayoutSession.EndRead(Totallbl);
            NetInc = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NetInc",
                Priority = 0,
                WinLeft = 103,
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
                    LeftOffset = 5,
                    RightOffset = 6,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(NetInc);
            TotalBtm = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TotalBtm",
                Priority = 3000,
                WinLeft = 1,
                WinTop = 75,
                WinWidth = 124,
                Masked = true,
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
            LayoutSession.EndRead(TotalBtm);
            TreasuryLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TreasuryLbl",
                Priority = 0,
                WinLeft = 5,
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
                    LeftOffset = 5,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Treasury",
                TextColor = 9415072
            };
            LayoutSession.EndRead(TreasuryLbl);
            Treasury = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Treasury",
                Priority = 0,
                WinLeft = 93,
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
                    LeftOffset = 5,
                    RightOffset = 6,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                TextColor = 9415072
            };
            LayoutSession.EndRead(Treasury);
            TreasuryPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TreasuryPnl",
                Priority = 3000,
                WinLeft = 1,
                WinTop = 55,
                WinWidth = 124,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 55,
                    BottomOffset = 21
                },
                WinHeight = 21,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(TreasuryPnl);
            IncomeValue = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "IncomeValue",
                Priority = 5,
                WinLeft = 81,
                WinTop = 0,
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
                    RightOffset = 20,
                    TopOffset = 0,
                    BottomOffset = 21
                },
                Masked = true,
                WinWidth = 25,
                WinHeight = 76,
                BorderWidth = 0,
                BorderHeight = 4,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 20,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taRight
            };
            LayoutSession.EndRead(IncomeValue);
            UpkeepValue = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "UpkeepValue",
                Priority = 5,
                WinLeft = 80,
                WinTop = 0,
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
                    RightOffset = 20,
                    TopOffset = 0,
                    BottomOffset = 21
                },
                Masked = true,
                WinWidth = 25,
                WinHeight = 76,
                BorderWidth = 0,
                BorderHeight = 4,
                TextColor = 9415072,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 20,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taRight
            };
            LayoutSession.EndRead(UpkeepValue);
            GoldAnim = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "GoldAnim",
                Priority = 0,
                WinLeft = 3,
                WinTop = 3,
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
                    LeftOffset = 3,
                    RightOffset = 0,
                    TopOffset = 3,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 10,
                CurrentImage = 30,
                AnimEnd = 33,
                WinWidth = 117,
                WinHeight = 49,
                Animate = true,
                AnimFrameDelay = 2
            };
            LayoutSession.EndRead(GoldAnim);
            PicPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PicPnl",
                Priority = 3000,
                WinLeft = 2,
                WinTop = 1,
                WinWidth = 123,
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
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 1,
                    BottomOffset = 0
                },
                WinHeight = 55,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PicPnl);
            Help = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Help",
                Priority = 0,
                WinLeft = 405,
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
                    RightOffset = 20,
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
                OnClick = Layout_HelpClick_29
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
            OWTitle.AOWWindow = Dlg;
            OWTitleIc.AOWWindow = Dlg;
            HeaderTab.AOWWindow = Dlg;
            T1Pnl.AOWWindow = Dlg;
            T1Pnl.TabPanel = HeaderTab;
            T2Pnl.AOWWindow = Dlg;
            T2Pnl.TabPanel = HeaderTab;
            T2LBPnl.AOWWindow = T2Pnl;
            T3Pnl.AOWWindow = Dlg;
            T3Pnl.TabPanel = HeaderTab;
            T3LBPnl.AOWWindow = T3Pnl;
            Minimize.AOWWindow = Dlg;
            T4Pnl.AOWWindow = Dlg;
            T4LBPnl.AOWWindow = T4Pnl;
            T2NameLB.AOWWindow = T2LBPnl;
            T2RaceLB.AOWWindow = T2LBPnl;
            T2IncomeLB.AOWWindow = T2LBPnl;
            T2LevelLB.AOWWindow = T2LBPnl;
            T2WallLB.AOWWindow = T2LBPnl;
            T2SizeLB.AOWWindow = T2LBPnl;
            T2EncLB.AOWWindow = T2LBPnl;
            T2EncLB.VScrollBar = CitySB;
            T2SName.AOWWindow = T2LBPnl;
            T2SRace.AOWWindow = T2LBPnl;
            T2SIn.AOWWindow = T2LBPnl;
            T2SWall.AOWWindow = T2LBPnl;
            T2SLvl.AOWWindow = T2LBPnl;
            T2SSize.AOWWindow = T2LBPnl;
            T2SEnc.AOWWindow = T2LBPnl;
            T2ActLB.AOWWindow = T2LBPnl;
            T2SAct.AOWWindow = T2LBPnl;
            T2RelLB.AOWWindow = T2LBPnl;
            T2SRel.AOWWindow = T2LBPnl;
            T2MorLB.AOWWindow = T2LBPnl;
            T2SMor.AOWWindow = T2LBPnl;
            CitySB.AOWWindow = T2LBPnl;
            T3Name.AOWWindow = T3LBPnl;
            T3SName.AOWWindow = T3LBPnl;
            T3ActLB.AOWWindow = T3LBPnl;
            T3ActLB.VScrollBar = PlaceSB;
            T3Activity.AOWWindow = T3LBPnl;
            PlaceSB.AOWWindow = T3LBPnl;
            UnitFr.AOWWindow = T4LBPnl;
            UnitGg.AOWWindow = T4LBPnl;
            Unit1LB.AOWWindow = T4LBPnl;
            Unit2LB.AOWWindow = T4LBPnl;
            Unit3LB.AOWWindow = T4LBPnl;
            Unit4LB.AOWWindow = T4LBPnl;
            Unit5LB.AOWWindow = T4LBPnl;
            Unit6LB.AOWWindow = T4LBPnl;
            Unit7LB.AOWWindow = T4LBPnl;
            Unit8LB.AOWWindow = T4LBPnl;
            Unit8LB.VScrollBar = PartySb;
            PartySb.AOWWindow = T4LBPnl;
            HeroPnl.AOWWindow = Dlg;
            HeroPnl.TabPanel = HeaderTab;
            T5MainPnl.AOWWindow = HeroPnl;
            T5Name.AOWWindow = T5LBFr;
            T5Health.AOWWindow = T5LBFr;
            T5Level.AOWWindow = T5LBFr;
            T5Level.VScrollBar = HeroSB;
            T5SName.AOWWindow = T5LBFr;
            T5SHealth.AOWWindow = T5LBFr;
            T5SLevel.AOWWindow = T5LBFr;
            T5HeroInf.AOWWindow = T5MainPnl;
            T5LBFr.AOWWindow = HeroPnl;
            HInfoBtn.AOWWindow = T5MainPnl;
            HeroSB.AOWWindow = T5LBFr;
            TotalPnl.AOWWindow = T1Pnl;
            IncomePnl.AOWWindow = T1Pnl;
            IncomeMem.AOWWindow = IncomePnl;
            IncomeMem.VScrollBar = IncomeSB;
            IncomeBtm.AOWWindow = IncomePnl;
            IncomeSB.AOWWindow = IncomePnl;
            IncomeLbl.AOWWindow = IncomeBtm;
            Income.AOWWindow = IncomeBtm;
            UpkeepPnl.AOWWindow = T1Pnl;
            UpkeepMem.AOWWindow = UpkeepPnl;
            UpkeepMem.VScrollBar = UpkeepSB;
            UpkeepBtm.AOWWindow = UpkeepPnl;
            UpkeepSB.AOWWindow = UpkeepPnl;
            UpkeepLbl.AOWWindow = UpkeepBtm;
            Upkeep.AOWWindow = UpkeepBtm;
            Totallbl.AOWWindow = TotalBtm;
            NetInc.AOWWindow = TotalBtm;
            TotalBtm.AOWWindow = TotalPnl;
            TreasuryLbl.AOWWindow = TreasuryPnl;
            Treasury.AOWWindow = TreasuryPnl;
            TreasuryPnl.AOWWindow = TotalPnl;
            IncomeValue.AOWWindow = IncomePnl;
            UpkeepValue.AOWWindow = UpkeepPnl;
            GoldAnim.AOWWindow = PicPnl;
            PicPnl.AOWWindow = TotalPnl;
            Help.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            OWTitle.Loaded();
            OWTitleIc.Loaded();
            HeaderTab.Loaded();
            T1Pnl.Loaded();
            T2Pnl.Loaded();
            T2LBPnl.Loaded();
            T3Pnl.Loaded();
            T3LBPnl.Loaded();
            Minimize.Loaded();
            T4Pnl.Loaded();
            T4LBPnl.Loaded();
            T2NameLB.Loaded();
            T2RaceLB.Loaded();
            T2IncomeLB.Loaded();
            T2LevelLB.Loaded();
            T2WallLB.Loaded();
            T2SizeLB.Loaded();
            T2EncLB.Loaded();
            T2SName.Loaded();
            T2SRace.Loaded();
            T2SIn.Loaded();
            T2SWall.Loaded();
            T2SLvl.Loaded();
            T2SSize.Loaded();
            T2SEnc.Loaded();
            T2ActLB.Loaded();
            T2SAct.Loaded();
            T2RelLB.Loaded();
            T2SRel.Loaded();
            T2MorLB.Loaded();
            T2SMor.Loaded();
            CitySB.Loaded();
            T3Name.Loaded();
            T3SName.Loaded();
            T3ActLB.Loaded();
            T3Activity.Loaded();
            PlaceSB.Loaded();
            UnitFr.Loaded();
            UnitGg.Loaded();
            Unit1LB.Loaded();
            Unit2LB.Loaded();
            Unit3LB.Loaded();
            Unit4LB.Loaded();
            Unit5LB.Loaded();
            Unit6LB.Loaded();
            Unit7LB.Loaded();
            Unit8LB.Loaded();
            PartySb.Loaded();
            HeroPnl.Loaded();
            T5MainPnl.Loaded();
            T5Name.Loaded();
            T5Health.Loaded();
            T5Level.Loaded();
            T5SName.Loaded();
            T5SHealth.Loaded();
            T5SLevel.Loaded();
            T5HeroInf.Loaded();
            T5LBFr.Loaded();
            HInfoBtn.Loaded();
            HeroSB.Loaded();
            TotalPnl.Loaded();
            IncomePnl.Loaded();
            IncomeMem.Loaded();
            IncomeBtm.Loaded();
            IncomeSB.Loaded();
            IncomeLbl.Loaded();
            Income.Loaded();
            UpkeepPnl.Loaded();
            UpkeepMem.Loaded();
            UpkeepBtm.Loaded();
            UpkeepSB.Loaded();
            UpkeepLbl.Loaded();
            Upkeep.Loaded();
            Totallbl.Loaded();
            NetInc.Loaded();
            TotalBtm.Loaded();
            TreasuryLbl.Loaded();
            Treasury.Loaded();
            TreasuryPnl.Loaded();
            IncomeValue.Loaded();
            UpkeepValue.Loaded();
            GoldAnim.Loaded();
            PicPnl.Loaded();
            Help.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OWTitleIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            HeaderTab.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HeaderTab.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            T1Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2LBPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Minimize.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Minimize.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            T4Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T4LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2NameLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2NameLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2NameLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2NameLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2RaceLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2RaceLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2RaceLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2RaceLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2IncomeLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2IncomeLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2IncomeLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2IncomeLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2LevelLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2LevelLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2LevelLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2LevelLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2WallLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10LB;
            T2WallLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2WallLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2WallLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2SizeLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2SizeLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2SizeLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2SizeLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2EncLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10LB;
            T2EncLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2EncLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2EncLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2SName.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SName.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SRace.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SRace.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SRace.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SIn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SIn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SIn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SWall.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SWall.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SWall.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SLvl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SLvl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SLvl.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SSize.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SSize.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SSize.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2SEnc.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SEnc.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SEnc.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2ActLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10LB;
            T2ActLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2ActLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2ActLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2SAct.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SAct.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SAct.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2RelLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10LB;
            T2RelLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2RelLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2RelLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2SRel.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SRel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SRel.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2MorLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10LB;
            T2MorLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2MorLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2MorLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T2SMor.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T2SMor.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T2SMor.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CitySB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CitySB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CitySB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CitySB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T3Name.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T3Name.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T3Name.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3Name.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T3SName.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T3SName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T3SName.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T3ActLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T3ActLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T3ActLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3ActLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T3Activity.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T3Activity.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T3Activity.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PlaceSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaceSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaceSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PlaceSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UnitFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Units;
            UnitFr.PopupMenu = Aow.Units.AoWUnitBanner.AoWUnitBanner.UnitBanner.BannerPopup;
            UnitGg.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UnitGg.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit1LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit1LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit1LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit1LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit2LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit2LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit2LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit3LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit3LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit3LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit4LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit4LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit4LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit5LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit5LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit5LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit6LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit6LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit6LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit7LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit7LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit7LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8LB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Unit8LB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Unit8LB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Unit8LB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PartySb.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PartySb.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PartySb.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PartySb.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeroPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5MainPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T5MainPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5Name.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T5Name.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T5Name.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5Name.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T5Health.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T5Health.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T5Health.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5Health.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T5Level.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T5Level.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T5Level.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5Level.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            T5SName.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T5SName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T5SName.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T5SHealth.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T5SHealth.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T5SHealth.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T5SLevel.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            T5SLevel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            T5SLevel.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T5HeroInf.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T5HeroInf.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T5LBFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HInfoBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HInfoBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            HeroSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeroSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeroSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeroSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TotalPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TotalPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomeMem.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            IncomeMem.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomeMem.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomeBtm.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            IncomeBtm.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomeSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IncomeSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IncomeSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IncomeSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            IncomeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Income.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpkeepPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpkeepPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpkeepMem.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpkeepMem.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpkeepMem.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpkeepBtm.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpkeepBtm.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpkeepSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UpkeepSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UpkeepSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UpkeepSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            UpkeepLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Upkeep.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Totallbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            NetInc.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TotalBtm.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TotalBtm.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TreasuryLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Treasury.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            TreasuryPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TreasuryPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            IncomeValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpkeepValue.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            GoldAnim.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt;
            PicPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PicPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Help.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Help.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_DlgShow_1(TObject p0)
        {
            DlgShow();
        }

        private void Layout_OverWindowResize_2(TObject p0)
        {
            OverWindowResize();
        }

        private void Layout_HeaderTabChange_3(TObject p0)
        {
            HeaderTabChange();
        }

        private void Layout_OWMinIconClick_4(TObject p0)
        {
            OWMinIconClick();
        }

        private void Layout_CityListMouseDown_5(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            CityListMouseDown((Aow.Units.AoWListBox.TAOWListBox)p0, p1, p2, p3, p4);
        }

        private void Layout_T2SNameClick_6(TObject p0)
        {
            T2SNameClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SRaceClick_7(TObject p0)
        {
            T2SRaceClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SInClick_8(TObject p0)
        {
            T2SInClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SWallClick_9(TObject p0)
        {
            T2SWallClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SLvlClick_10(TObject p0)
        {
            T2SLvlClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SSizeClick_11(TObject p0)
        {
            T2SSizeClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SEncClick_12(TObject p0)
        {
            T2SEncClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SActClick_13(TObject p0)
        {
            T2SActClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SRelClick_14(TObject p0)
        {
            T2SRelClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SMorClick_15(TObject p0)
        {
            T2SMorClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T2SBChange_16(TObject p0)
        {
            T2SBChange();
        }

        private void Layout_PlacesListMouseDown_17(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            PlacesListMouseDown((Aow.Units.AoWListBox.TAOWListBox)p0, p1, p2, p3, p4);
        }

        private void Layout_T3SNameClick_18(TObject p0)
        {
            T3SNameClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T3ActivityClick_19(TObject p0)
        {
            T3ActivityClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T3SBChange_20(TObject p0)
        {
            T3SBChange();
        }

        private void Layout_PartyVSBChange_21(TObject p0)
        {
            PartyVSBChange();
        }

        private void Layout_T5SNameClick_22(TObject p0)
        {
            T5SNameClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T5SHealthClick_23(TObject p0)
        {
            T5SHealthClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_T5SLevelClick_24(TObject p0)
        {
            T5SLevelClick(default(Aow.Rtl.TObject), default(int));
        }

        private void Layout_HeroInfoClick_25(TObject p0)
        {
            HeroInfoClick();
        }

        private void Layout_HeroVSBChange_26(TObject p0)
        {
            HeroVSBChange();
        }

        private void Layout_IncomeSBChange_27(TObject p0)
        {
            IncomeSBChange();
        }

        private void Layout_UpkeepSBChange_28(TObject p0)
        {
            UpkeepSBChange();
        }

        private void Layout_HelpClick_29(TObject p0)
        {
            HelpClick();
        }
    }
}
