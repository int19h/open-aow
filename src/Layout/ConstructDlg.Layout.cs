// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWConstructDlg
{
    public partial class TConstructDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ConstructDlg";
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
                CurrentImage = 2,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 323,
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
                Text = "Construct",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 22,
                WinTop = 170,
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
                WinWidth = 317,
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
                WinLeft = 198,
                WinTop = 170,
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
                WinWidth = 130,
                OnClick = Layout_CancelBtnClick_0
            };
            LayoutSession.EndRead(CancelBtn);
            ConstructBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ConstructBtn",
                Priority = 100,
                WinLeft = 32,
                WinTop = 170,
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
                Text = "Construct",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 130,
                OnClick = ConstructBtnClick
            };
            LayoutSession.EndRead(ConstructBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 140,
                WinTop = 112,
                WinWidth = 360,
                Masked = true,
                OnHide = Layout_DlgHide_1,
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
                WinHeight = 208,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            ConstructPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ConstructPnl",
                Priority = 0,
                WinLeft = 30,
                WinTop = 55,
                WinWidth = 300,
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
                    LeftOffset = 30,
                    RightOffset = 30,
                    TopOffset = 55,
                    BottomOffset = 50
                },
                WinHeight = 103,
                ILIndexFrame = 105,
                ILIndexTexture = 18,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ConstructPnl);
            ConstructionList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "ConstructionList",
                Priority = 60,
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
                    RightOffPercent = 29,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 2,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 210,
                WinHeight = 88,
                BorderWidth = 22,
                BorderHeight = 4,
                OnChange = Layout_ConstructionListChange_2,
                OnDoubleClick = ConstructBtnClick,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ConstructionList);
            SortConstructionsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SortConstructionsBtn",
                Priority = 100,
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
                    RightOffPercent = 29,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 1,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                BorderHeight = 3,
                BorderHeightSelected = 4,
                ILIndexDown = 88,
                ILIndexLit = 91,
                ILIndexUp = 85,
                Text = "Source",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 210,
                OnClick = Layout_SortConstructionsBtnClick_3
            };
            LayoutSession.EndRead(SortConstructionsBtn);
            SortConstructionCostBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SortConstructionCostBtn",
                Priority = 100,
                WinLeft = 210,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 70,
                    RightOffPercent = 14,
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
                Text = "Cost",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 48,
                OnClick = Layout_SortConstructionCostBtnClick_4
            };
            LayoutSession.EndRead(SortConstructionCostBtn);
            ConstructionCostList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "ConstructionCostList",
                Priority = 40,
                WinLeft = 210,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 70,
                    RightOffPercent = 14,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 48,
                WinHeight = 88,
                BorderWidth = 10,
                BorderHeight = 4,
                OnChange = Layout_ConstructionCostListChange_5,
                OnDoubleClick = ConstructBtnClick,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ConstructionCostList);
            ConstructionTurnsList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "ConstructionTurnsList",
                Priority = 30,
                WinLeft = 257,
                WinTop = 14,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 85,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 0,
                    TopOffset = 14,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 43,
                WinHeight = 88,
                BorderWidth = 10,
                BorderHeight = 4,
                OnChange = Layout_ConstructionTurnsListChange_6,
                OnDoubleClick = ConstructBtnClick,
                Selected = -1,
                TextColor = 9402970,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 19,
                ILIndexTSelected = 16,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ConstructionTurnsList);
            SortConstructionTurnsBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "SortConstructionTurnsBtn",
                Priority = 100,
                WinLeft = 257,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 85,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
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
                Text = "Turns",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 43,
                OnClick = Layout_SortConstructionTurnsBtnClick_7
            };
            LayoutSession.EndRead(SortConstructionTurnsBtn);
            AOWLabel1 = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "AOWLabel1",
                Priority = 0,
                WinLeft = 83,
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
                Text = "Select object to construct",
                TextColor = 0
            };
            LayoutSession.EndRead(AOWLabel1);
            ConSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ConSB",
                Visible = false,
                Priority = 30,
                WinLeft = 283,
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
                WinHeight = 88,
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
            LayoutSession.EndRead(ConSB);
            CeltM = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltM",
                Priority = 100,
                WinLeft = 167,
                WinTop = 170,
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
                    BottomOffset = 11
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
            AOWButton2 = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AOWButton2",
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
                WinWidth = 78
            };
            LayoutSession.EndRead(AOWButton2);
            T4VSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "T4VSB",
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
                Max = 7,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 92,
                WinWidth = 17,
                NumberVisible = 4,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(T4VSB);
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
            TitleIcon.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            ConstructBtn.AOWWindow = Dlg;
            ConstructPnl.AOWWindow = Dlg;
            ConstructionList.AOWWindow = ConstructPnl;
            SortConstructionsBtn.AOWWindow = ConstructPnl;
            SortConstructionCostBtn.AOWWindow = ConstructPnl;
            ConstructionCostList.AOWWindow = ConstructPnl;
            ConstructionTurnsList.AOWWindow = ConstructPnl;
            ConstructionTurnsList.VScrollBar = ConSB;
            SortConstructionTurnsBtn.AOWWindow = ConstructPnl;
            AOWLabel1.AOWWindow = Dlg;
            ConSB.AOWWindow = ConstructPnl;
            CeltM.AOWWindow = Dlg;
            Loaded();
            TitleIcon.Loaded();
            Close.Loaded();
            Title.Loaded();
            BCeltL.Loaded();
            CancelBtn.Loaded();
            ConstructBtn.Loaded();
            Dlg.Loaded();
            ConstructPnl.Loaded();
            ConstructionList.Loaded();
            SortConstructionsBtn.Loaded();
            SortConstructionCostBtn.Loaded();
            ConstructionCostList.Loaded();
            ConstructionTurnsList.Loaded();
            SortConstructionTurnsBtn.Loaded();
            AOWLabel1.Loaded();
            ConSB.Loaded();
            CeltM.Loaded();
            AOWButton2.Loaded();
            T4VSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CancelBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ConstructBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ConstructBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ConstructBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ConstructPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructionList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ConstructionList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ConstructionList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructionList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SortConstructionsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SortConstructionsBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            SortConstructionCostBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SortConstructionCostBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            ConstructionCostList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ConstructionCostList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ConstructionCostList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructionCostList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructionTurnsList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ConstructionTurnsList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ConstructionTurnsList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ConstructionTurnsList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SortConstructionTurnsBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            SortConstructionTurnsBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AOWLabel1.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            ConSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ConSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ConSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ConSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CeltM.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            AOWButton2.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            AOWButton2.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T4VSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T4VSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T4VSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            T4VSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_CancelBtnClick_0(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_DlgHide_1(TObject p0)
        {
            DlgHide();
        }

        private void Layout_ConstructionListChange_2(TObject p0)
        {
            ConstructionListChange();
        }

        private void Layout_SortConstructionsBtnClick_3(TObject p0)
        {
            SortConstructionsBtnClick();
        }

        private void Layout_SortConstructionCostBtnClick_4(TObject p0)
        {
            SortConstructionCostBtnClick();
        }

        private void Layout_ConstructionCostListChange_5(TObject p0)
        {
            ConstructionCostListChange();
        }

        private void Layout_ConstructionTurnsListChange_6(TObject p0)
        {
            ConstructionTurnsListChange();
        }

        private void Layout_SortConstructionTurnsBtnClick_7(TObject p0)
        {
            SortConstructionTurnsBtnClick();
        }
    }
}
