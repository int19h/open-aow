// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWDisjunctionDlg
{
    public partial class TDisjunctionDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "DisjunctionDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
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
                CurrentImage = 0,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
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
                Text = "Disjunction",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 22,
                WinTop = 382,
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
                    LeftOffset = 22,
                    RightOffset = 21,
                    TopOffset = 0,
                    BottomOffset = 11
                },
                Masked = false,
                WinWidth = 357,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 100,
                WinLeft = 248,
                WinTop = 382,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cancel casting"
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
                    LeftOffset = 0,
                    RightOffset = 32,
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
                WinWidth = 120,
                OnClick = Layout_CancelBtnClick_0
            };
            LayoutSession.EndRead(CancelBtn);
            CastBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CastBtn",
                Priority = 100,
                WinLeft = 32,
                WinTop = 382,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Cast Disjunction"
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
                    LeftOffset = 32,
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
                WinWidth = 120,
                OnClick = Layout_CastBtnClick_1
            };
            LayoutSession.EndRead(CastBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 120,
                WinTop = 6,
                WinWidth = 400,
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
                WinHeight = 430,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            DisjunctionChance = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DisjunctionChance",
                Priority = 0,
                WinLeft = 307,
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
                Text = "40%",
                TextColor = 12632256
            };
            LayoutSession.EndRead(DisjunctionChance);
            DisjunctionChanceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DisjunctionChanceLbl",
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
                Text = "Disjunction Chance",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DisjunctionChanceLbl);
            DisjunctionChancePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DisjunctionChancePnl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 327,
                WinWidth = 340,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 0,
                    BottomOffset = 71
                },
                WinHeight = 22,
                ILIndexFrame = 40,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DisjunctionChancePnl);
            DisjunctionMana = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DisjunctionMana",
                Priority = 0,
                WinLeft = 318,
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
                Text = "25",
                TextColor = 12632256
            };
            LayoutSession.EndRead(DisjunctionMana);
            DisjunctionManaLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DisjunctionManaLbl",
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
                Text = "Disjunction Mana",
                TextColor = 9402970
            };
            LayoutSession.EndRead(DisjunctionManaLbl);
            DisjunctionManaPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DisjunctionManaPnl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 305,
                WinWidth = 340,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 0,
                    BottomOffset = 93
                },
                WinHeight = 22,
                ILIndexFrame = 40,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DisjunctionManaPnl);
            T4LBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T4LBPnl",
                Priority = 0,
                WinLeft = 29,
                WinTop = 160,
                WinWidth = 342,
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
                    LeftOffset = 29,
                    RightOffset = 29,
                    TopOffset = 170,
                    BottomOffset = 123
                },
                WinHeight = 137,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T4LBPnl);
            OwnerList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "OwnerList",
                Priority = 10,
                WinLeft = 171,
                WinTop = 16,
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
                    LeftOffset = 0,
                    RightOffset = 2,
                    TopOffset = 16,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 169,
                WinHeight = 121,
                BorderWidth = 8,
                BorderHeight = 4,
                OnChange = Layout_OwnerListChange_3,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(OwnerList);
            EnchantmentList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "EnchantmentList",
                Priority = 20,
                WinLeft = 1,
                WinTop = 16,
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
                    RightOffset = 0,
                    TopOffset = 16,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 170,
                WinHeight = 121,
                BorderWidth = 8,
                BorderHeight = 4,
                OnChange = Layout_EnchantmentListChange_4,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 40,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(EnchantmentList);
            Sort = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Sort",
                Priority = 100,
                WinLeft = 171,
                WinTop = 2,
                CaptureInput = true,
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
                    RightOffset = 1,
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Source",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 170,
                OnClick = Layout_SortClick_5
            };
            LayoutSession.EndRead(Sort);
            SortEnchantment = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SortEnchantment",
                Priority = 100,
                WinLeft = 1,
                WinTop = 2,
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
                    TopOffset = 2,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 3,
                ILIndexDown = 9,
                ILIndexLit = 6,
                ILIndexUp = 6,
                Text = "Enchantment",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 169,
                OnClick = Layout_SortEnchantmentClick_6
            };
            LayoutSession.EndRead(SortEnchantment);
            EnchantmentsScrollBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "EnchantmentsScrollBar",
                Visible = false,
                Priority = 10,
                WinLeft = 323,
                WinTop = 16,
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
                WinHeight = 121,
                WinWidth = 17,
                NumberVisible = 1,
                OnChange = Layout_EnchantmentsScrollBarChange_7,
                VanishWhenFull = true,
                ILIndexIncUp = 74,
                ILIndexIncDown = 75,
                ILIndexDecUp = 72,
                ILIndexDecDown = 73,
                ILIndexBack = 79,
                ILIndexTab = 76
            };
            LayoutSession.EndRead(EnchantmentsScrollBar);
            Spell = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Spell",
                Visible = false,
                Priority = 0,
                WinLeft = 150,
                WinTop = 30,
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
                    LeftOffset = 42,
                    RightOffset = 0,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Select a spell",
                TextColor = 0
            };
            LayoutSession.EndRead(Spell);
            ChanceSlider = new Aow.Units.AoWHScroll.TAOWHScrollBar(this)
            {
                Name = "ChanceSlider",
                Priority = 0,
                WinLeft = 154,
                WinTop = 357,
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
                    LeftOffset = 125,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 45
                },
                Masked = true,
                Min = 1,
                Max = 999,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 18,
                WinWidth = 92,
                NumberVisible = 1,
                OnChange = Layout_ChanceSliderChange_8,
                VanishWhenFull = false,
                ILIndexIncUp = 0,
                ILIndexIncDown = 0,
                ILIndexDecUp = 0,
                ILIndexDecDown = 0,
                ILIndexBack = 80,
                ILIndexTab = 83
            };
            LayoutSession.EndRead(ChanceSlider);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 363,
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
                    RightOffset = 22,
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
                OnClick = Layout_CancelBtnClick_0
            };
            LayoutSession.EndRead(Close);
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
            MapViewPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "MapViewPnl",
                Priority = 1,
                WinLeft = 31,
                WinTop = 30,
                WinWidth = 338,
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
                    LeftOffset = 31,
                    RightOffset = 31,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                WinHeight = 142,
                ILIndexFrame = 105,
                ILIndexTexture = 1,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(MapViewPnl);
            MapView = new Aow.Units.aowUserDrawBox.TaowUserDrawBox(this)
            {
                Name = "MapView",
                OnDraw = MapViewDraw,
                OnUpdate = Layout_MapViewUpdate_9,
                Priority = 50,
                WinLeft = 31,
                WinTop = 30,
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
                    LeftOffset = 31,
                    RightOffset = 31,
                    TopOffset = 30,
                    BottomOffset = 0
                },
                Masked = false,
                WinWidth = 338,
                WinHeight = 142
            };
            LayoutSession.EndRead(MapView);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            TitleIcon.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            CastBtn.AOWWindow = Dlg;
            DisjunctionChance.AOWWindow = DisjunctionChancePnl;
            DisjunctionChanceLbl.AOWWindow = DisjunctionChancePnl;
            DisjunctionChancePnl.AOWWindow = Dlg;
            DisjunctionMana.AOWWindow = DisjunctionManaPnl;
            DisjunctionManaLbl.AOWWindow = DisjunctionManaPnl;
            DisjunctionManaPnl.AOWWindow = Dlg;
            T4LBPnl.AOWWindow = Dlg;
            OwnerList.AOWWindow = T4LBPnl;
            OwnerList.VScrollBar = EnchantmentsScrollBar;
            EnchantmentList.AOWWindow = T4LBPnl;
            Sort.AOWWindow = T4LBPnl;
            SortEnchantment.AOWWindow = T4LBPnl;
            EnchantmentsScrollBar.AOWWindow = T4LBPnl;
            Spell.AOWWindow = Dlg;
            ChanceSlider.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            MapViewPnl.AOWWindow = Dlg;
            MapView.AOWWindow = Dlg;
            Loaded();
            TitleIcon.Loaded();
            Title.Loaded();
            BCeltL.Loaded();
            CancelBtn.Loaded();
            CastBtn.Loaded();
            Dlg.Loaded();
            DisjunctionChance.Loaded();
            DisjunctionChanceLbl.Loaded();
            DisjunctionChancePnl.Loaded();
            DisjunctionMana.Loaded();
            DisjunctionManaLbl.Loaded();
            DisjunctionManaPnl.Loaded();
            T4LBPnl.Loaded();
            OwnerList.Loaded();
            EnchantmentList.Loaded();
            Sort.Loaded();
            SortEnchantment.Loaded();
            EnchantmentsScrollBar.Loaded();
            Spell.Loaded();
            ChanceSlider.Loaded();
            Close.Loaded();
            Translator.Loaded();
            MapViewPnl.Loaded();
            MapView.Loaded();
            session.Resolve(this);
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CastBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CastBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CastBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DisjunctionChance.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisjunctionChanceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisjunctionChancePnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DisjunctionChancePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DisjunctionMana.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisjunctionManaLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisjunctionManaPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DisjunctionManaPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OwnerList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OwnerList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            OwnerList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OwnerList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EnchantmentList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            EnchantmentList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            EnchantmentList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            EnchantmentList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Sort.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            Sort.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SortEnchantment.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SortEnchantment.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            EnchantmentsScrollBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentsScrollBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentsScrollBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            EnchantmentsScrollBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            Spell.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ChanceSlider.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChanceSlider.ILLBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChanceSlider.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            MapViewPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_CancelBtnClick_0(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_CastBtnClick_1(TObject p0)
        {
            CastBtnClick();
        }

        private void Layout_DlgHide_2(TObject p0)
        {
            DlgHide();
        }

        private void Layout_OwnerListChange_3(TObject p0)
        {
            OwnerListChange();
        }

        private void Layout_EnchantmentListChange_4(TObject p0)
        {
            EnchantmentListChange();
        }

        private void Layout_SortClick_5(TObject p0)
        {
            SortClick();
        }

        private void Layout_SortEnchantmentClick_6(TObject p0)
        {
            SortEnchantmentClick();
        }

        private void Layout_EnchantmentsScrollBarChange_7(TObject p0)
        {
            EnchantmentsScrollBarChange();
        }

        private void Layout_ChanceSliderChange_8(TObject p0)
        {
            ChanceSliderChange();
        }

        private void Layout_MapViewUpdate_9(TObject p0)
        {
            MapViewUpdate();
        }
    }
}
