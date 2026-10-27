// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWChatWin
{
    public partial class TChatWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "ChatWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            ChatWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "ChatWin",
                Visible = false,
                Priority = 10,
                WinLeft = 0,
                WinTop = 272,
                WinWidth = 440,
                Masked = false,
                OnHide = Layout_ChatWinHide_0,
                OnShow = Layout_ChatWinShow_1,
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
                    RightOffset = 200,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = false,
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
                OnDrag = Layout_ChatWinResize_2,
                OnResize = Layout_ChatWinResize_2,
                MinWidth = 440,
                MinHeight = 160,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 160,
                Dockable = false
            };
            LayoutSession.EndRead(ChatWin);
            ChatMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ChatMemo",
                OnUpdate = Layout_ChatMemoUpdate_3,
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
                    RightOffset = 117,
                    TopOffset = 0,
                    BottomOffset = 21
                },
                Masked = false,
                WinWidth = 306,
                WinHeight = 108,
                BorderWidth = 8,
                BorderHeight = 5,
                TextColor = 12632256,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 113,
                ILIndexTexture = 17,
                ItemHeight = 15,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(ChatMemo);
            ChatEdit = new Aow.Units.AoWEdit.TAOWEdit(this)
            {
                Name = "ChatEdit",
                Priority = 0,
                WinLeft = 1,
                WinTop = 107,
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
                    LeftOffset = 1,
                    RightOffset = 117,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 306,
                BorderWidth = 8,
                BorderHeight = 6,
                MaxChars = 0,
                CursorChar = (byte)95,
                BlinkDelay = 1,
                TextColor = 11002593,
                Editable = true,
                ILIndex = 15,
                OnReturn = Layout_ChatEditReturn_4,
                PassWordChar = (byte)0
            };
            LayoutSession.EndRead(ChatEdit);
            PlayerList = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "PlayerList",
                Priority = 0,
                WinLeft = 307,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 70,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 11,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 117,
                WinHeight = 128,
                BorderWidth = 8,
                BorderHeight = 2,
                Selected = -1,
                OnPreDrawItem = Layout_PlayerListPreDrawItem_5,
                TextColor = 9415072,
                TextSelColor = 16777215,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(PlayerList);
            SIcon = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SIcon",
                Priority = 0,
                WinLeft = 2,
                WinTop = 1,
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
                    TopOffset = 1,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 6,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SIcon);
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
                Text = "Chat",
                TextColor = 0
            };
            LayoutSession.EndRead(IWTitle);
            Min = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Min",
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
                OnClick = Layout_MinClick_6
            };
            LayoutSession.EndRead(Min);
            ChatPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ChatPnl",
                Priority = 0,
                WinLeft = 8,
                WinTop = 23,
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
                    TopOffset = 23,
                    BottomOffset = 8
                },
                WinHeight = 129,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ChatPnl);
            NSBar = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "NSBar",
                Visible = false,
                Priority = 0,
                WinLeft = 407,
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
                LargeChange = 2,
                Position = 0,
                WinHeight = 128,
                WinWidth = 17,
                NumberVisible = 2,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(NSBar);
            ChatSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "ChatSB",
                Visible = false,
                Priority = 0,
                WinLeft = 290,
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
                LargeChange = 5,
                Position = 0,
                WinHeight = 108,
                WinWidth = 17,
                NumberVisible = 2,
                VanishWhenFull = true,
                ILIndexIncUp = 96,
                ILIndexIncDown = 97,
                ILIndexDecUp = 94,
                ILIndexDecDown = 95,
                ILIndexBack = 101,
                ILIndexTab = 98
            };
            LayoutSession.EndRead(ChatSB);
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
            ChatMemo.AOWWindow = ChatPnl;
            ChatMemo.VScrollBar = ChatSB;
            ChatEdit.AOWWindow = ChatPnl;
            PlayerList.AOWWindow = ChatPnl;
            PlayerList.VScrollBar = NSBar;
            SIcon.AOWWindow = ChatWin;
            IWTitle.AOWWindow = ChatWin;
            Min.AOWWindow = ChatWin;
            ChatPnl.AOWWindow = ChatWin;
            NSBar.AOWWindow = ChatPnl;
            ChatSB.AOWWindow = ChatPnl;
            Loaded();
            ChatWin.Loaded();
            ChatMemo.Loaded();
            ChatEdit.Loaded();
            PlayerList.Loaded();
            SIcon.Loaded();
            IWTitle.Loaded();
            Min.Loaded();
            ChatPnl.Loaded();
            NSBar.Loaded();
            ChatSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            ChatWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            ChatWin.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ChatWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChatMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ChatMemo.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            ChatMemo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ChatEdit.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            ChatEdit.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            PlayerList.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            PlayerList.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PlayerList.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            PlayerList.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            SIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            IWTitle.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Min.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Min.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ChatPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NSBar.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            NSBar.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            ChatSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
        }

        private void Layout_ChatWinHide_0(TObject p0)
        {
            ChatWinHide();
        }

        private void Layout_ChatWinShow_1(TObject p0)
        {
            ChatWinShow();
        }

        private void Layout_ChatWinResize_2(TObject p0)
        {
            ChatWinResize();
        }

        private void Layout_ChatMemoUpdate_3(TObject p0)
        {
            ChatMemoUpdate();
        }

        private void Layout_ChatEditReturn_4(TObject p0)
        {
            ChatEditReturn();
        }

        private void Layout_PlayerListPreDrawItem_5(Aow.Units.AoWComp.TAoWComponent p0, in Aow.Rtl.TRect p1, int p2, ref Aow.Units.GFXE.TSurfaceDesc p3)
        {
            PlayerListPreDrawItem(default(Aow.Rtl.TObject), default(Aow.Rtl.TRect), p2, default(Aow.Units.GFXE.TSurfaceDesc));
        }

        private void Layout_MinClick_6(TObject p0)
        {
            MinClick();
        }
    }
}
