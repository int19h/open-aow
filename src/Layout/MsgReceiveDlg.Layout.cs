// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWMsgReceiveDlg
{
    public partial class TMsgReceiveDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MsgReceiveDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            MsgReceiveWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "MsgReceiveWin",
                Visible = false,
                Priority = 300,
                WinLeft = 128,
                WinTop = 85,
                WinWidth = 384,
                Masked = true,
                OnHide = Layout_MsgReceiveWinHide_0,
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
                FreeSurfaceOnHide = false,
                ILIndexFrame = 80,
                ILIndexTexture = 7,
                DragHeight = 0,
                Draggable = false,
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
            LayoutSession.EndRead(MsgReceiveWin);
            MessageMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "MessageMemo",
                Priority = 200,
                WinLeft = 47,
                WinTop = 100,
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
                    LeftOffset = 47,
                    RightOffset = 47,
                    TopOffset = 100,
                    BottomOffset = 80
                },
                Masked = true,
                WinWidth = 290,
                WinHeight = 82,
                LayoutItems = new LayoutStrings()
                {
                    "##############",
                    "##############"
                },
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
            LayoutSession.EndRead(MessageMemo);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 122,
                WinTop = 211,
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
                Text = "Close",
                TextColor = 855324,
                TextSelColor = 16777215,
                WinWidth = 140,
                OnClick = Layout_CloseClick_1
            };
            LayoutSession.EndRead(Close);
            MessageSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "MessageSB",
                Visible = false,
                Priority = 200,
                WinLeft = 301,
                WinTop = 45,
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
                LargeChange = 5,
                Position = 0,
                WinHeight = 137,
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
            LayoutSession.EndRead(MessageSB);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Priority = 1000,
                WinLeft = 45,
                WinTop = 40,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Player's Shield"
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
                    LeftOffset = 45,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 22,
                AnimEnd = 38,
                WinWidth = 40,
                WinHeight = 53,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LShield);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Priority = 1000,
                WinLeft = 299,
                WinTop = 45,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Player's Shield"
                },
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
                    LeftOffset = 80,
                    RightOffset = 45,
                    TopOffset = 45,
                    BottomOffset = 0
                },
                Masked = true,
                AnimStart = 0,
                CurrentImage = 16,
                AnimEnd = 38,
                WinWidth = 40,
                WinHeight = 53,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RShield);
            HeaderMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "HeaderMemo",
                Priority = 200,
                WinLeft = 70,
                WinTop = 45,
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
                    LeftOffset = 70,
                    RightOffset = 70,
                    TopOffset = 45,
                    BottomOffset = 80
                },
                Masked = true,
                WinWidth = 244,
                WinHeight = 137,
                LayoutItems = new LayoutStrings()
                {
                    "##############"
                },
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
            MessageMemo.AOWWindow = MsgReceiveWin;
            MessageMemo.VScrollBar = MessageSB;
            Close.AOWWindow = MsgReceiveWin;
            MessageSB.AOWWindow = MsgReceiveWin;
            LShield.AOWWindow = MsgReceiveWin;
            RShield.AOWWindow = MsgReceiveWin;
            HeaderMemo.AOWWindow = MsgReceiveWin;
            HeaderMemo.VScrollBar = MessageSB;
            Loaded();
            MsgReceiveWin.Loaded();
            MessageMemo.Loaded();
            Close.Loaded();
            MessageSB.Loaded();
            LShield.Loaded();
            RShield.Loaded();
            HeaderMemo.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            MsgReceiveWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            MsgReceiveWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MsgReceiveWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            MessageMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            MessageMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Close.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10b;
            Close.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CampaignTrans;
            MessageSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            MessageSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            LShield.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            RShield.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            HeaderMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15BlackBrown;
            HeaderMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_MsgReceiveWinHide_0(TObject p0)
        {
            MsgReceiveWinHide();
        }

        private void Layout_CloseClick_1(TObject p0)
        {
            CloseClick();
        }
    }
}
