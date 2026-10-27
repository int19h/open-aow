// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWWizardsTowerDlg
{
    public partial class TWizardsTowerDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "WizardsTowerDlg";
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
                CurrentImage = 4,
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
                WinLeft = 279,
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
                Text = "Wizards Tower",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 22,
                WinTop = 287,
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
                WinWidth = 273,
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
                Priority = 99999,
                WinLeft = 164,
                WinTop = 287,
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
            BuyBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "BuyBtn",
                Priority = 99999,
                WinLeft = 32,
                WinTop = 287,
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
                Text = "Buy",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_BuyBtnClick_1
            };
            LayoutSession.EndRead(BuyBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 162,
                WinTop = 54,
                WinWidth = 316,
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
                WinHeight = 325,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            ResearchPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "ResearchPnl",
                Priority = 100,
                WinLeft = 27,
                WinTop = 155,
                WinWidth = 262,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 155,
                    BottomOffset = 30
                },
                WinHeight = 150,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(ResearchPnl);
            SpellName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellName",
                Priority = 0,
                WinLeft = 76,
                WinTop = 40,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "###########",
                TextColor = 1254460
            };
            LayoutSession.EndRead(SpellName);
            SpellIcn = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "SpellIcn",
                OnDraw = SpellIcnDraw,
                Priority = 100,
                WinLeft = 111,
                WinTop = 2,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awCenter,
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
                CurrentImage = 38,
                AnimEnd = 0,
                WinWidth = 40,
                WinHeight = 35,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SpellIcn);
            SpellClass = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellClass",
                Priority = 0,
                WinLeft = 95,
                WinTop = 60,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 60,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "############",
                TextColor = 12632256
            };
            LayoutSession.EndRead(SpellClass);
            SpellCost = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "SpellCost",
                Priority = 0,
                WinLeft = 95,
                WinTop = 72,
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
                    LeftOffset = 57,
                    RightOffset = 0,
                    TopOffset = 72,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "############",
                TextColor = 12632256
            };
            LayoutSession.EndRead(SpellCost);
            ResearchMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "ResearchMemo",
                Priority = 0,
                WinLeft = 1,
                WinTop = 87,
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
                    TopOffset = 87,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 260,
                WinHeight = 35,
                BorderWidth = 10,
                BorderHeight = 0,
                TextColor = 12632256,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                ItemHeight = 12,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(ResearchMemo);
            PriceLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PriceLbl",
                Priority = 100,
                WinLeft = 118,
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
                Text = "########",
                TextColor = 5908276
            };
            LayoutSession.EndRead(PriceLbl);
            OfferText = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "OfferText",
                Priority = 100,
                WinLeft = 98,
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
                Masked = true,
                Text = "the wizards offer:",
                TextColor = 5908276
            };
            LayoutSession.EndRead(OfferText);
            PicPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "PicPnl",
                Priority = 0,
                WinLeft = 22,
                WinTop = 22,
                WinWidth = 272,
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
                    LeftOffset = 22,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 0
                },
                WinHeight = 265,
                ILIndexFrame = 121,
                ILIndexTexture = 9,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(PicPnl);
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
            BuyBtn.AOWWindow = Dlg;
            ResearchPnl.AOWWindow = Dlg;
            SpellName.AOWWindow = ResearchPnl;
            SpellIcn.AOWWindow = ResearchPnl;
            SpellClass.AOWWindow = ResearchPnl;
            SpellCost.AOWWindow = ResearchPnl;
            ResearchMemo.AOWWindow = ResearchPnl;
            PriceLbl.AOWWindow = Dlg;
            OfferText.AOWWindow = Dlg;
            PicPnl.AOWWindow = Dlg;
            Loaded();
            TitleIcon.Loaded();
            Close.Loaded();
            Title.Loaded();
            BCeltL.Loaded();
            CancelBtn.Loaded();
            BuyBtn.Loaded();
            Dlg.Loaded();
            ResearchPnl.Loaded();
            SpellName.Loaded();
            SpellIcn.Loaded();
            SpellClass.Loaded();
            SpellCost.Loaded();
            ResearchMemo.Loaded();
            PriceLbl.Loaded();
            OfferText.Loaded();
            PicPnl.Loaded();
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
            BuyBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            BuyBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            BuyBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            SpellName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            SpellIcn.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.RelWin;
            SpellClass.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            SpellCost.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            ResearchMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8s;
            PriceLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            OfferText.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            PicPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            PicPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.MiscArt;
        }

        private void Layout_CancelBtnClick_0(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_BuyBtnClick_1(TObject p0)
        {
            BuyBtnClick();
        }

        private void Layout_DlgHide_2(TObject p0)
        {
            DlgHide();
        }
    }
}
