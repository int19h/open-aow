// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.aowHelpWin
{
    public partial class THelpWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "HelpWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            HelpWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "HelpWin",
                Visible = false,
                Priority = 999999,
                WinLeft = 135,
                WinTop = 16,
                WinWidth = 370,
                Masked = false,
                OnHide = Layout_HelpWinHide_0,
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
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 0,
                ILIndexTexture = 2,
                DragHeight = 20,
                Draggable = true,
                Resizable = true,
                OnResize = Layout_HelpWinResize_1,
                MinWidth = 200,
                MinHeight = 200,
                MaxWidth = 400,
                MaxHeight = 400,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 400,
                Dockable = false
            };
            LayoutSession.EndRead(HelpWin);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 0,
                WinLeft = 27,
                WinTop = 4,
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
                Masked = true,
                Text = "Help",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            CloseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBtn",
                Priority = 0,
                WinLeft = 351,
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
                ILIndexDown = 1,
                ILIndexLit = 2,
                ILIndexUp = 0,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CloseBtnClick_2
            };
            LayoutSession.EndRead(CloseBtn);
            TitleIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TitleIcon",
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
                CurrentImage = 7,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            BBarEnd = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "BBarEnd",
                Priority = 0,
                WinLeft = 360,
                WinTop = 369,
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
                    RightOffset = 3,
                    TopOffset = 135,
                    BottomOffset = 4
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 2,
                AnimEnd = 12,
                WinWidth = 7,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(BBarEnd);
            BBarBegin = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "BBarBegin",
                Priority = 0,
                WinLeft = 4,
                WinTop = 369,
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
                    LeftOffset = 4,
                    RightOffset = 0,
                    TopOffset = 160,
                    BottomOffset = 4
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 0,
                AnimEnd = 12,
                WinWidth = 7,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(BBarBegin);
            BCloseBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BCloseBut",
                Priority = 250,
                WinLeft = 118,
                WinTop = 369,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 32,
                    RightOffPercent = 32,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 4
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
                WinWidth = 134,
                OnClick = Layout_CloseBtnClick_2
            };
            LayoutSession.EndRead(BCloseBut);
            CeltLineR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltLineR",
                Priority = 10,
                WinLeft = 280,
                WinTop = 369,
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
                    RightOffset = 9,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltLineR);
            BBar = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "BBar",
                Priority = 0,
                WinLeft = 92,
                WinTop = 369,
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
                    LeftOffset = 92,
                    RightOffset = 60,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 1,
                AnimEnd = 12,
                WinWidth = 218,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(BBar);
            CeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "CeltLineL",
                Priority = 0,
                WinLeft = 11,
                WinTop = 369,
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
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 4
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 81,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(CeltLineL);
            HelpSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "HelpSB",
                Priority = 200,
                WinLeft = 350,
                WinTop = 51,
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
                Max = 18,
                SmallChange = 1,
                LargeChange = 17,
                Position = 0,
                WinHeight = 318,
                WinWidth = 17,
                NumberVisible = 17,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(HelpSB);
            HeaderMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "HeaderMemo",
                Priority = 200,
                WinLeft = 4,
                WinTop = 17,
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
                    LeftOffset = 4,
                    RightOffset = 3,
                    TopOffset = 17,
                    BottomOffset = 80
                },
                Masked = false,
                WinWidth = 363,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 9,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 11,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(HeaderMemo);
            HelpMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "HelpMemo",
                Priority = 200,
                WinLeft = 4,
                WinTop = 51,
                CaptureInput = false,
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
                    RightOffset = 3,
                    TopOffset = 51,
                    BottomOffset = 31
                },
                Masked = false,
                WinWidth = 363,
                WinHeight = 318,
                BorderWidth = 10,
                BorderHeight = 9,
                TextColor = 2565942,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 11,
                ItemHeight = 17,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(HelpMemo);
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
            Title.AOWWindow = HelpWin;
            CloseBtn.AOWWindow = HelpWin;
            TitleIcon.AOWWindow = HelpWin;
            BBarEnd.AOWWindow = HelpWin;
            BBarBegin.AOWWindow = HelpWin;
            BCloseBut.AOWWindow = HelpWin;
            CeltLineR.AOWWindow = HelpWin;
            BBar.AOWWindow = HelpWin;
            CeltLineL.AOWWindow = HelpWin;
            HelpSB.AOWWindow = HelpWin;
            HeaderMemo.AOWWindow = HelpWin;
            HelpMemo.AOWWindow = HelpWin;
            HelpMemo.VScrollBar = HelpSB;
            Loaded();
            HelpWin.Loaded();
            Title.Loaded();
            CloseBtn.Loaded();
            TitleIcon.Loaded();
            BBarEnd.Loaded();
            BBarBegin.Loaded();
            BCloseBut.Loaded();
            CeltLineR.Loaded();
            BBar.Loaded();
            CeltLineL.Loaded();
            HelpSB.Loaded();
            HeaderMemo.Loaded();
            HelpMemo.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            HelpWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            HelpWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HelpWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CloseBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CloseBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            BBarEnd.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BBarBegin.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BCloseBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BCloseBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            CeltLineR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BBar.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            HelpSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HelpSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HelpSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HelpSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            HeaderMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            HeaderMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HeaderMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HelpMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            HelpMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            HelpMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
        }

        private void Layout_HelpWinHide_0(TObject p0)
        {
            HelpWinHide();
        }

        private void Layout_HelpWinResize_1(TObject p0)
        {
            HelpWinResize();
        }

        private void Layout_CloseBtnClick_2(TObject p0)
        {
            CloseBtnClick();
        }
    }
}
