// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWTargetSelection
{
    public partial class TTargetSelectionDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TargetSelectionDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 510,
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
                MinWidth = 200,
                MinHeight = 216,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 216,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            CeltFrame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "CeltFrame",
                Priority = 13,
                WinLeft = 5,
                WinTop = 19,
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
                    LeftOffset = 5,
                    RightOffset = 5,
                    TopOffset = 19,
                    BottomOffset = 5
                },
                Masked = false,
                WinWidth = 190,
                WinHeight = 192,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 22,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(CeltFrame);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 181,
                WinTop = 2,
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
                    RightOffset = 4,
                    TopOffset = 2,
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
                OnClick = Layout_CloseClick_0
            };
            LayoutSession.EndRead(Close);
            Title = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Title",
                Priority = 1000,
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
                Text = "Target Selector",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
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
                CurrentImage = 0,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(SIcon);
            MainBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MainBtn",
                Priority = 500,
                WinLeft = 10,
                WinTop = 184,
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
                    LeftOffset = 10,
                    RightOffset = 80,
                    TopOffset = 0,
                    BottomOffset = 5
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
                WinWidth = 110,
                OnClick = Layout_MainBtnClick_1
            };
            LayoutSession.EndRead(MainBtn);
            DefaultMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "DefaultMemo",
                Priority = 0,
                WinLeft = 3,
                WinTop = 26,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 0,
                    TopOffPercent = 15,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 114,
                WinHeight = 150,
                LayoutItems = new LayoutStrings()
                {
                    "##########"
                },
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 16,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(DefaultMemo);
            DefaultPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "DefaultPnl",
                Priority = 15,
                WinLeft = 5,
                WinTop = 40,
                WinWidth = 120,
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
                    RightOffset = 75,
                    TopOffset = 40,
                    BottomOffset = 0
                },
                WinHeight = 176,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(DefaultPnl);
            HeaderLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeaderLbl",
                Priority = 100,
                WinLeft = 26,
                WinTop = 10,
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
                    TopOffset = 10,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(HeaderLbl);
            HeaderPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "HeaderPnl",
                Priority = 400,
                WinLeft = 4,
                WinTop = 17,
                WinWidth = 116,
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
                    LeftOffset = 4,
                    RightOffset = 80,
                    TopOffset = 17,
                    BottomOffset = 31
                },
                WinHeight = 27,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(HeaderPnl);
            BorderImage = new Aow.Units.AoWArnoLine.TAoWArnoLine(this)
            {
                Name = "BorderImage",
                Priority = 0,
                WinLeft = 188,
                WinTop = 46,
                CaptureInput = false,
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
                    RightOffset = 12,
                    TopOffset = 46,
                    BottomOffset = 31
                },
                Masked = false,
                WinHeight = 139
            };
            LayoutSession.EndRead(BorderImage);
            SpellPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SpellPnl",
                Visible = false,
                Priority = 15,
                WinLeft = 4,
                WinTop = 44,
                WinWidth = 116,
                Masked = true,
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
                WinHeight = 141,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(SpellPnl);
            SpellMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "SpellMemo",
                Priority = 0,
                WinLeft = 3,
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
                    TopOffPercent = 10,
                    BottomOffPercent = 0,
                    LeftOffset = 3,
                    RightOffset = 3,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 110,
                WinHeight = 127,
                BorderWidth = 3,
                BorderHeight = 0,
                TextColor = 9236989,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 16,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(SpellMemo);
            TargetRangeLabel = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TargetRangeLabel",
                Priority = 100,
                WinLeft = 23,
                WinTop = 90,
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
                    TopOffset = 90,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Target Range",
                TextColor = 9236989
            };
            LayoutSession.EndRead(TargetRangeLabel);
            TargetLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TargetLbl",
                Priority = 100,
                WinLeft = 36,
                WinTop = 108,
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
                    TopOffset = 108,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 16777215
            };
            LayoutSession.EndRead(TargetLbl);
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
            CeltFrame.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            SIcon.AOWWindow = Dlg;
            MainBtn.AOWWindow = Dlg;
            DefaultMemo.AOWWindow = DefaultPnl;
            DefaultPnl.AOWWindow = Dlg;
            HeaderLbl.AOWWindow = HeaderPnl;
            HeaderPnl.AOWWindow = Dlg;
            BorderImage.AOWWindow = Dlg;
            SpellPnl.AOWWindow = Dlg;
            SpellMemo.AOWWindow = SpellPnl;
            TargetRangeLabel.AOWWindow = DefaultPnl;
            TargetLbl.AOWWindow = DefaultPnl;
            Loaded();
            Dlg.Loaded();
            CeltFrame.Loaded();
            Close.Loaded();
            Title.Loaded();
            SIcon.Loaded();
            MainBtn.Loaded();
            DefaultMemo.Loaded();
            DefaultPnl.Loaded();
            HeaderLbl.Loaded();
            HeaderPnl.Loaded();
            BorderImage.Loaded();
            SpellPnl.Loaded();
            SpellMemo.Loaded();
            TargetRangeLabel.Loaded();
            TargetLbl.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CeltFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            SIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            MainBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MainBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            DefaultMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            HeaderLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            SpellMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TargetRangeLabel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TargetLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
        }

        private void Layout_CloseClick_0(TObject p0)
        {
            CloseClick();
        }

        private void Layout_MainBtnClick_1(TObject p0)
        {
            MainBtnClick();
        }
    }
}
