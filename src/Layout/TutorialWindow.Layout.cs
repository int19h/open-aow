// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTutorialWin
{
    public partial class TTutorialWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TutorialWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 9999,
                WinLeft = 125,
                WinTop = 85,
                WinWidth = 390,
                Masked = true,
                OnHide = Layout_DlgHide_0,
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
                MaskColor = 5808200,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 80,
                ILIndexTexture = 34,
                DragHeight = 20,
                Draggable = true,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 262,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            MessageMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MessageMemo",
                Priority = 200,
                WinLeft = 52,
                WinTop = 80,
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
                    LeftOffset = 52,
                    RightOffset = 52,
                    TopOffset = 80,
                    BottomOffset = 60
                },
                Masked = true,
                WinWidth = 286,
                WinHeight = 122,
                BorderWidth = 15,
                BorderHeight = 5,
                TextColor = 1052711,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 0,
                ItemHeight = 17,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(MessageMemo);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 125,
                WinTop = 211,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes the Tutorial Window. You can reopen it by pressing F1 on the keyboard."
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
                    TopOffset = 1,
                    BottomOffset = 25
                },
                Masked = true,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 6,
                ILIndexLit = 3,
                ILIndexUp = 0,
                Text = "OK",
                TextColor = 855324,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_CloseClick_1
            };
            LayoutSession.EndRead(Close);
            HeaderMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "HeaderMemo",
                Priority = 200,
                WinLeft = 70,
                WinTop = 35,
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
                    LeftOffset = 70,
                    RightOffset = 70,
                    TopOffset = 35,
                    BottomOffset = 80
                },
                Masked = true,
                WinWidth = 250,
                WinHeight = 44,
                BorderWidth = 10,
                BorderHeight = 5,
                TextColor = 0,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 0,
                ItemHeight = 20,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(HeaderMemo);
            AbVSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AbVSB",
                Visible = false,
                Priority = 200,
                WinLeft = 325,
                WinTop = 80,
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
                Masked = true,
                Min = 0,
                Max = 0,
                SmallChange = 1,
                LargeChange = 6,
                Position = 0,
                WinHeight = 122,
                WinWidth = 13,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 42,
                ILIndexIncDown = 43,
                ILIndexDecUp = 40,
                ILIndexDecDown = 41,
                ILIndexBack = 47,
                ILIndexTab = 44
            };
            LayoutSession.EndRead(AbVSB);
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
            MessageMemo.AOWWindow = Dlg;
            MessageMemo.VScrollBar = AbVSB;
            Close.AOWWindow = Dlg;
            HeaderMemo.AOWWindow = Dlg;
            AbVSB.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            MessageMemo.Loaded();
            Close.Loaded();
            HeaderMemo.Loaded();
            AbVSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt;
            MessageMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            Close.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            HeaderMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            HeaderMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            AbVSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AbVSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }
    }
}
