// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWMagic
{
    public partial class TMagicWin
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MagicWin";
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
                OnResize = Layout_DlgResize_2,
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
                Text = "Magic Management",
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
                    "MAIN",
                    "SPELLS",
                    "GLOBAL",
                    "POWER"
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
            OWRPillar = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "OWRPillar",
                Visible = false,
                Priority = 500,
                WinLeft = 367,
                WinTop = 0,
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
                WinWidth = 70,
                WinHeight = 222,
                FrameType = Aow.Units.AoWFrame.TFrameType.frVertLine,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(OWRPillar);
            OWLPillar = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "OWLPillar",
                Visible = false,
                Priority = 500,
                WinLeft = 3,
                WinTop = 0,
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
                WinWidth = 70,
                WinHeight = 222,
                FrameType = Aow.Units.AoWFrame.TFrameType.frVertLine,
                ILIndexFrame = 0,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(OWLPillar);
            T1Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T1Pnl",
                Priority = 1,
                WinLeft = 5,
                WinTop = 39,
                WinWidth = 430,
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
                    TopOffset = 39,
                    BottomOffset = 5
                },
                WinHeight = 116,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T1Pnl);
            ResearchPoints = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchPoints",
                Priority = 0,
                WinLeft = 180,
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
                    RightOffset = 8,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "30",
                TextColor = 12632256
            };
            LayoutSession.EndRead(ResearchPoints);
            ResLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResLbl",
                Priority = 0,
                WinLeft = 10,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Research Points",
                TextColor = 9415072
            };
            LayoutSession.EndRead(ResLbl);
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
            EnchantmentList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "EnchantmentList",
                OnMouseDown = EnchantmentListMouseDown,
                Priority = 30,
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
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 160,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_EnchantmentListChange_4,
                OnDoubleClick = Layout_EnchantmentListDoubleClick_5,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(EnchantmentList);
            EnchantmentSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "EnchantmentSortBtn",
                Priority = 100,
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
                    RightOffPercent = 62,
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
                Text = "Enchantment",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 160,
                OnClick = Layout_EnchantmentSortBtnClick_6
            };
            LayoutSession.EndRead(EnchantmentSortBtn);
            UpkeepList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "UpkeepList",
                OnMouseDown = EnchantmentListMouseDown,
                Priority = 10,
                WinLeft = 344,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 82,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 78,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_UpkeepListChange_7,
                OnDoubleClick = Layout_EnchantmentListDoubleClick_5,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(UpkeepList);
            UpkeepSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "UpkeepSortBtn",
                Priority = 100,
                WinLeft = 345,
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
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Mana Upkeep",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 77,
                OnClick = Layout_UpkeepSortBtnClick_8
            };
            LayoutSession.EndRead(UpkeepSortBtn);
            BindingList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "BindingList",
                OnMouseDown = EnchantmentListMouseDown,
                Priority = 20,
                WinLeft = 160,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 77,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 185,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_BindingListChange_9,
                OnDoubleClick = Layout_EnchantmentListDoubleClick_5,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(BindingList);
            BindingSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BindingSortBtn",
                Priority = 100,
                WinLeft = 160,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 76,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Binding",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 186,
                OnClick = Layout_BindingSortBtnClick_10
            };
            LayoutSession.EndRead(BindingSortBtn);
            ResPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResPnl",
                Priority = 1,
                WinLeft = 1,
                WinTop = 21,
                WinWidth = 202,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 22
                },
                WinHeight = 22,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResPnl);
            NetMana = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NetMana",
                Priority = 0,
                WinLeft = 145,
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
                    RightOffset = 8,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "20-18=2",
                TextColor = 12632256
            };
            LayoutSession.EndRead(NetMana);
            NetMamaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NetMamaLbl",
                Priority = 0,
                WinLeft = 10,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Net Mana Income",
                TextColor = 9415072
            };
            LayoutSession.EndRead(NetMamaLbl);
            NetManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NetManaPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 42,
                WinWidth = 202,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 22,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NetManaPnl);
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
            GlobalEnchantmentList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "GlobalEnchantmentList",
                OnMouseDown = GlobalEnchantmentMouseDown,
                Priority = 30,
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
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 160,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_GlobalEnchantmentListChange_11,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(GlobalEnchantmentList);
            GlobalEnchantmentSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GlobalEnchantmentSortBtn",
                Priority = 100,
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
                    RightOffPercent = 62,
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
                Text = "Global Enchantment",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 160,
                OnClick = Layout_GlobalEnchantmentSortBtnClick_12
            };
            LayoutSession.EndRead(GlobalEnchantmentSortBtn);
            GlobalUpkeepList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "GlobalUpkeepList",
                OnMouseDown = GlobalEnchantmentMouseDown,
                Priority = 10,
                WinLeft = 344,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 82,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 78,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_GlobalUpkeepListChange_13,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(GlobalUpkeepList);
            GlobalUpkeepSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GlobalUpkeepSortBtn",
                Priority = 100,
                WinLeft = 345,
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
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Mana Upkeep",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 77,
                OnClick = Layout_GlobalUpkeepSortBtnClick_14
            };
            LayoutSession.EndRead(GlobalUpkeepSortBtn);
            GlobalNameList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "GlobalNameList",
                OnMouseDown = GlobalEnchantmentMouseDown,
                Priority = 20,
                WinLeft = 160,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 77,
                    TopOffset = 14,
                    BottomOffset = 20
                },
                Masked = false,
                WinWidth = 185,
                WinHeight = 73,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_GlobalNameListChange_15,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(GlobalNameList);
            GlobalNameSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GlobalNameSortBtn",
                Priority = 100,
                WinLeft = 160,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahTop,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 76,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Owner",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 186,
                OnClick = Layout_GlobalNameSortBtnClick_16
            };
            LayoutSession.EndRead(GlobalNameSortBtn);
            RightPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "RightPnl",
                Priority = 0,
                WinLeft = 219,
                WinTop = 2,
                WinWidth = 203,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 51,
                    RightOffPercent = 2,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 113,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(RightPnl);
            SpellName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellName",
                Priority = 0,
                WinLeft = 60,
                WinTop = 8,
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
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 12632256
            };
            LayoutSession.EndRead(SpellName);
            GlobalDispelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "GlobalDispelBtn",
                Visible = false,
                Priority = 50,
                WinLeft = 1,
                WinTop = 86,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 97,
                ILIndexLit = 100,
                ILIndexUp = 94,
                Text = "Remove",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 160,
                OnClick = Layout_GlobalDispelBtnClick_17
            };
            LayoutSession.EndRead(GlobalDispelBtn);
            DispelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "DispelBtn",
                Visible = false,
                Priority = 50,
                WinLeft = 1,
                WinTop = 86,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                BorderHeight = 5,
                BorderHeightSelected = 5,
                ILIndexDown = 97,
                ILIndexLit = 100,
                ILIndexUp = 94,
                Text = "Remove",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 160,
                OnClick = Layout_DispelBtnClick_18
            };
            LayoutSession.EndRead(DispelBtn);
            ResearchBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ResearchBtn",
                Priority = 100,
                WinLeft = 10,
                WinTop = 86,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Opens the spell research book."
                },
                CaptureInput = true,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 5
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 6,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Research",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 183,
                OnClick = Layout_ResearchBtnClick_19
            };
            LayoutSession.EndRead(ResearchBtn);
            ResearchTurnsLeft = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResearchTurnsLeft",
                Priority = 0,
                WinLeft = 69,
                WinTop = 50,
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
                    BottomOffset = 3
                },
                Masked = true,
                Text = "########",
                TextColor = 12632256
            };
            LayoutSession.EndRead(ResearchTurnsLeft);
            SpellClass = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellClass",
                Priority = 0,
                WinLeft = 60,
                WinTop = 22,
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
                    TopOffset = 22,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SpellClass);
            SpellPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SpellPnl",
                Priority = 10,
                WinLeft = 0,
                WinTop = 13,
                WinWidth = 203,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                WinHeight = 65,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SpellPnl);
            SpellCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellCost",
                Priority = 0,
                WinLeft = 60,
                WinTop = 34,
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
                    TopOffset = 34,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 9415072
            };
            LayoutSession.EndRead(SpellCost);
            SpellIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SpellIcn",
                OnDraw = SpellIcnDraw,
                Priority = 0,
                WinLeft = 8,
                WinTop = 8,
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
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SpellIcn);
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
                OnClick = Layout_MinimizeClick_20
            };
            LayoutSession.EndRead(Minimize);
            ChangePowerBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ChangePowerBut",
                Priority = 100,
                WinLeft = 10,
                WinTop = 86,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Sets the division of power between mana generation and spell research"
                },
                CaptureInput = true,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 5
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 6,
                ILIndexDown = 46,
                ILIndexLit = 49,
                ILIndexUp = 43,
                Text = "Power Distribution",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 183,
                OnClick = Layout_ChangePowerButClick_21
            };
            LayoutSession.EndRead(ChangePowerBut);
            Power = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Power",
                Priority = 0,
                WinLeft = 180,
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
                    RightOffset = 8,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "50",
                TextColor = 12632256
            };
            LayoutSession.EndRead(Power);
            PowerLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PowerLbl",
                Priority = 0,
                WinLeft = 10,
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
                    LeftOffset = 10,
                    RightOffset = 0,
                    TopOffset = 6,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Power Base",
                TextColor = 9415072
            };
            LayoutSession.EndRead(PowerLbl);
            PowePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PowePnl",
                Priority = 1,
                WinLeft = 1,
                WinTop = 0,
                WinWidth = 202,
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
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 78
                },
                WinHeight = 22,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PowePnl);
            LeftPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeftPnl",
                Priority = 0,
                WinLeft = 8,
                WinTop = 2,
                WinWidth = 203,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahCenter,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 2,
                    RightOffPercent = 51,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 113,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeftPnl);
            NonePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NonePnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 13,
                WinWidth = 203,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                WinHeight = 65,
                ILIndexFrame = 121,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NonePnl);
            NoneMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "NoneMsg",
                Priority = 0,
                WinLeft = 23,
                WinTop = 26,
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
                    LeftOffset = 60,
                    RightOffset = 0,
                    TopOffset = 8,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "No spell is being researched",
                TextColor = 9415072
            };
            LayoutSession.EndRead(NoneMsg);
            T4Pnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T4Pnl",
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
                OwnerItem = 3,
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
                WinHeight = 107,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T4LBPnl);
            PowerValueList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PowerValueList",
                OnMouseDown = PowerValueListMouseDown,
                Priority = 10,
                WinLeft = 344,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 82,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 78,
                WinHeight = 92,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_PowerValueListChange_22,
                OnDoubleClick = Layout_PowerSourceListDoubleClick_23,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(PowerValueList);
            PowerValueSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PowerValueSortBtn",
                Priority = 100,
                WinLeft = 344,
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
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Amount",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 78,
                OnClick = Layout_PowerValueSortBtnClick_24
            };
            LayoutSession.EndRead(PowerValueSortBtn);
            PowerSourceList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PowerSourceList",
                OnMouseDown = PowerValueListMouseDown,
                Priority = 20,
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
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 77,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 344,
                WinHeight = 92,
                BorderWidth = 6,
                BorderHeight = 3,
                OnChange = Layout_PowerSourceListChange_25,
                OnDoubleClick = Layout_PowerSourceListDoubleClick_23,
                Selected = -1,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PowerSourceList);
            PowerSourceSortBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "PowerSourceSortBtn",
                Priority = 100,
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
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 77,
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
                WinWidth = 344,
                OnClick = Layout_PowerSourceSortBtnClick_26
            };
            LayoutSession.EndRead(PowerSourceSortBtn);
            T2BottonPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2BottonPnl",
                Priority = 0,
                WinLeft = 160,
                WinTop = 86,
                WinWidth = 261,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2BottonPnl);
            EnchantmentSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "EnchantmentSB",
                Visible = false,
                Priority = 10,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 73,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_EnchantmentSBChange_27,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(EnchantmentSB);
            GlobalEnchantmentSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "GlobalEnchantmentSB",
                Visible = false,
                Priority = 10,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 73,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_GlobalEnchantmentSBChange_28,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(GlobalEnchantmentSB);
            PowerSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "PowerSB",
                Visible = false,
                Priority = 10,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 92,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_PowerSBChange_29,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(PowerSB);
            T3BottomPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3BottomPnl",
                Priority = 0,
                WinLeft = 160,
                WinTop = 86,
                WinWidth = 261,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 38,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 1,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3BottomPnl);
            LeftFr = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "LeftFr",
                Priority = 10,
                WinLeft = 0,
                WinTop = 13,
                WinWidth = 203,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 35
                },
                WinHeight = 65,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(LeftFr);
            T2ButtonPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T2ButtonPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 86,
                WinWidth = 160,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T2ButtonPnl);
            T3ButtonPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3ButtonPnl",
                Priority = 0,
                WinLeft = 1,
                WinTop = 86,
                WinWidth = 160,
                Masked = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 62,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                WinHeight = 20,
                ILIndexFrame = 113,
                ILIndexTexture = 18,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3ButtonPnl);
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
                OnClick = Layout_HelpClick_30
            };
            LayoutSession.EndRead(Help);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            OWTitle.AOWWindow = Dlg;
            OWTitleIc.AOWWindow = Dlg;
            HeaderTab.AOWWindow = Dlg;
            OWRPillar.AOWWindow = Dlg;
            OWLPillar.AOWWindow = Dlg;
            T1Pnl.AOWWindow = Dlg;
            T1Pnl.TabPanel = HeaderTab;
            ResearchPoints.AOWWindow = ResPnl;
            ResLbl.AOWWindow = ResPnl;
            T2Pnl.AOWWindow = Dlg;
            T2Pnl.TabPanel = HeaderTab;
            T2LBPnl.AOWWindow = T2Pnl;
            EnchantmentList.AOWWindow = T2LBPnl;
            EnchantmentSortBtn.AOWWindow = T2LBPnl;
            UpkeepList.AOWWindow = T2LBPnl;
            UpkeepList.VScrollBar = EnchantmentSB;
            UpkeepSortBtn.AOWWindow = T2LBPnl;
            BindingList.AOWWindow = T2LBPnl;
            BindingSortBtn.AOWWindow = T2LBPnl;
            ResPnl.AOWWindow = LeftFr;
            NetMana.AOWWindow = NetManaPnl;
            NetMamaLbl.AOWWindow = NetManaPnl;
            NetManaPnl.AOWWindow = LeftFr;
            T3Pnl.AOWWindow = Dlg;
            T3Pnl.TabPanel = HeaderTab;
            T3LBPnl.AOWWindow = T3Pnl;
            GlobalEnchantmentList.AOWWindow = T3LBPnl;
            GlobalEnchantmentSortBtn.AOWWindow = T3LBPnl;
            GlobalUpkeepList.AOWWindow = T3LBPnl;
            GlobalUpkeepList.VScrollBar = GlobalEnchantmentSB;
            GlobalUpkeepSortBtn.AOWWindow = T3LBPnl;
            GlobalNameList.AOWWindow = T3LBPnl;
            GlobalNameSortBtn.AOWWindow = T3LBPnl;
            RightPnl.AOWWindow = T1Pnl;
            SpellName.AOWWindow = SpellPnl;
            GlobalDispelBtn.AOWWindow = T3LBPnl;
            DispelBtn.AOWWindow = T2LBPnl;
            ResearchBtn.AOWWindow = RightPnl;
            ResearchTurnsLeft.AOWWindow = SpellPnl;
            SpellClass.AOWWindow = SpellPnl;
            SpellPnl.AOWWindow = RightPnl;
            SpellCost.AOWWindow = SpellPnl;
            SpellIcn.AOWWindow = SpellPnl;
            Minimize.AOWWindow = Dlg;
            ChangePowerBut.AOWWindow = LeftPnl;
            Power.AOWWindow = PowePnl;
            PowerLbl.AOWWindow = PowePnl;
            PowePnl.AOWWindow = LeftFr;
            LeftPnl.AOWWindow = T1Pnl;
            NonePnl.AOWWindow = RightPnl;
            NoneMsg.AOWWindow = NonePnl;
            T4Pnl.AOWWindow = Dlg;
            T4Pnl.TabPanel = HeaderTab;
            T4LBPnl.AOWWindow = T4Pnl;
            PowerValueList.AOWWindow = T4LBPnl;
            PowerValueList.VScrollBar = PowerSB;
            PowerValueSortBtn.AOWWindow = T4LBPnl;
            PowerSourceList.AOWWindow = T4LBPnl;
            PowerSourceSortBtn.AOWWindow = T4LBPnl;
            T2BottonPnl.AOWWindow = T2LBPnl;
            EnchantmentSB.AOWWindow = T2LBPnl;
            GlobalEnchantmentSB.AOWWindow = T3LBPnl;
            PowerSB.AOWWindow = T4LBPnl;
            T3BottomPnl.AOWWindow = T3LBPnl;
            LeftFr.AOWWindow = LeftPnl;
            T2ButtonPnl.AOWWindow = T2LBPnl;
            T3ButtonPnl.AOWWindow = T3LBPnl;
            Help.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            OWTitle.Loaded();
            OWTitleIc.Loaded();
            HeaderTab.Loaded();
            OWRPillar.Loaded();
            OWLPillar.Loaded();
            T1Pnl.Loaded();
            ResearchPoints.Loaded();
            ResLbl.Loaded();
            T2Pnl.Loaded();
            T2LBPnl.Loaded();
            EnchantmentList.Loaded();
            EnchantmentSortBtn.Loaded();
            UpkeepList.Loaded();
            UpkeepSortBtn.Loaded();
            BindingList.Loaded();
            BindingSortBtn.Loaded();
            ResPnl.Loaded();
            NetMana.Loaded();
            NetMamaLbl.Loaded();
            NetManaPnl.Loaded();
            T3Pnl.Loaded();
            T3LBPnl.Loaded();
            GlobalEnchantmentList.Loaded();
            GlobalEnchantmentSortBtn.Loaded();
            GlobalUpkeepList.Loaded();
            GlobalUpkeepSortBtn.Loaded();
            GlobalNameList.Loaded();
            GlobalNameSortBtn.Loaded();
            RightPnl.Loaded();
            SpellName.Loaded();
            GlobalDispelBtn.Loaded();
            DispelBtn.Loaded();
            ResearchBtn.Loaded();
            ResearchTurnsLeft.Loaded();
            SpellClass.Loaded();
            SpellPnl.Loaded();
            SpellCost.Loaded();
            SpellIcn.Loaded();
            Minimize.Loaded();
            ChangePowerBut.Loaded();
            Power.Loaded();
            PowerLbl.Loaded();
            PowePnl.Loaded();
            LeftPnl.Loaded();
            NonePnl.Loaded();
            NoneMsg.Loaded();
            T4Pnl.Loaded();
            T4LBPnl.Loaded();
            PowerValueList.Loaded();
            PowerValueSortBtn.Loaded();
            PowerSourceList.Loaded();
            PowerSourceSortBtn.Loaded();
            T2BottonPnl.Loaded();
            EnchantmentSB.Loaded();
            GlobalEnchantmentSB.Loaded();
            PowerSB.Loaded();
            T3BottomPnl.Loaded();
            LeftFr.Loaded();
            T2ButtonPnl.Loaded();
            T3ButtonPnl.Loaded();
            Translator.Loaded();
            Help.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OWTitleIc.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            HeaderTab.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HeaderTab.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            T1Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ResearchPoints.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T2LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2LBPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EnchantmentList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EnchantmentList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EnchantmentList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EnchantmentList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            EnchantmentSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            EnchantmentSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            UpkeepList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            UpkeepList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            UpkeepList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            UpkeepList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            UpkeepSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            UpkeepSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BindingList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BindingList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            BindingList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            BindingList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            BindingSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            BindingSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ResPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ResPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NetMana.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NetMamaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            NetManaPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NetManaPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GlobalEnchantmentList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            GlobalEnchantmentList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GlobalEnchantmentList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            GlobalEnchantmentList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            GlobalEnchantmentSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            GlobalEnchantmentSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            GlobalUpkeepList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            GlobalUpkeepList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GlobalUpkeepList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            GlobalUpkeepList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            GlobalUpkeepSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            GlobalUpkeepSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            GlobalNameList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            GlobalNameList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            GlobalNameList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            GlobalNameList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            GlobalNameSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            GlobalNameSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SpellName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            GlobalDispelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            GlobalDispelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            DispelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DispelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ResearchBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResearchBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ResearchBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ResearchTurnsLeft.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SpellClass.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SpellPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            SpellPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SpellCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SpellIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.ProdWin;
            Minimize.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Minimize.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ChangePowerBut.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ChangePowerBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ChangePowerBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Power.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PowerLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PowePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NonePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NonePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            NoneMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            T4Pnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T4LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowerValueList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PowerValueList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowerValueList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PowerValueList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            PowerValueSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PowerValueSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            PowerSourceList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PowerSourceList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PowerSourceList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PowerSourceList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            PowerSourceSortBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            PowerSourceSortBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T2BottonPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2BottonPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EnchantmentSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GlobalEnchantmentSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GlobalEnchantmentSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GlobalEnchantmentSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            GlobalEnchantmentSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            PowerSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T3BottomPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T3BottomPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            LeftFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            LeftFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T2ButtonPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T2ButtonPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            T3ButtonPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            T3ButtonPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
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

        private void Layout_DlgResize_2(TObject p0)
        {
            DlgResize();
        }

        private void Layout_HeaderTabChange_3(TObject p0)
        {
            HeaderTabChange();
        }

        private void Layout_EnchantmentListChange_4(TObject p0)
        {
            EnchantmentListChange();
        }

        private void Layout_EnchantmentListDoubleClick_5(TObject p0)
        {
            EnchantmentListDoubleClick();
        }

        private void Layout_EnchantmentSortBtnClick_6(TObject p0)
        {
            EnchantmentSortBtnClick();
        }

        private void Layout_UpkeepListChange_7(TObject p0)
        {
            UpkeepListChange();
        }

        private void Layout_UpkeepSortBtnClick_8(TObject p0)
        {
            UpkeepSortBtnClick();
        }

        private void Layout_BindingListChange_9(TObject p0)
        {
            BindingListChange();
        }

        private void Layout_BindingSortBtnClick_10(TObject p0)
        {
            BindingSortBtnClick();
        }

        private void Layout_GlobalEnchantmentListChange_11(TObject p0)
        {
            GlobalEnchantmentListChange();
        }

        private void Layout_GlobalEnchantmentSortBtnClick_12(TObject p0)
        {
            GlobalEnchantmentSortBtnClick();
        }

        private void Layout_GlobalUpkeepListChange_13(TObject p0)
        {
            GlobalUpkeepListChange();
        }

        private void Layout_GlobalUpkeepSortBtnClick_14(TObject p0)
        {
            GlobalUpkeepSortBtnClick();
        }

        private void Layout_GlobalNameListChange_15(TObject p0)
        {
            GlobalNameListChange();
        }

        private void Layout_GlobalNameSortBtnClick_16(TObject p0)
        {
            GlobalNameSortBtnClick();
        }

        private void Layout_GlobalDispelBtnClick_17(TObject p0)
        {
            GlobalDispelBtnClick();
        }

        private void Layout_DispelBtnClick_18(TObject p0)
        {
            DispelBtnClick();
        }

        private void Layout_ResearchBtnClick_19(TObject p0)
        {
            ResearchBtnClick();
        }

        private void Layout_MinimizeClick_20(TObject p0)
        {
            MinimizeClick();
        }

        private void Layout_ChangePowerButClick_21(TObject p0)
        {
            ChangePowerButClick();
        }

        private void Layout_PowerValueListChange_22(TObject p0)
        {
            PowerValueListChange();
        }

        private void Layout_PowerSourceListDoubleClick_23(TObject p0)
        {
            PowerSourceListDoubleClick();
        }

        private void Layout_PowerValueSortBtnClick_24(TObject p0)
        {
            PowerValueSortBtnClick();
        }

        private void Layout_PowerSourceListChange_25(TObject p0)
        {
            PowerSourceListChange();
        }

        private void Layout_PowerSourceSortBtnClick_26(TObject p0)
        {
            PowerSourceSortBtnClick();
        }

        private void Layout_EnchantmentSBChange_27(TObject p0)
        {
            EnchantmentSBChange();
        }

        private void Layout_GlobalEnchantmentSBChange_28(TObject p0)
        {
            GlobalEnchantmentSBChange();
        }

        private void Layout_PowerSBChange_29(TObject p0)
        {
            PowerSBChange();
        }

        private void Layout_HelpClick_30(TObject p0)
        {
            HelpClick();
        }
    }
}
