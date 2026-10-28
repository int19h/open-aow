// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCityJoinDlg
{
    public partial class TCityJoinDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CityJoinDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 5000,
                WinLeft = 93,
                WinTop = 91,
                WinWidth = 455,
                OnDrawBack = DlgDrawBack,
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
                WinHeight = 250,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            ApproachLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ApproachLbl",
                Priority = 1000,
                WinLeft = 150,
                WinTop = 31,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 31,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "##########",
                TextColor = 7921903
            };
            LayoutSession.EndRead(ApproachLbl);
            TCeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineL",
                Visible = false,
                Priority = 100,
                WinLeft = 28,
                WinTop = 213,
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
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 399,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLineL);
            CeltFrame = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "CeltFrame",
                Priority = 500,
                WinLeft = 22,
                WinTop = 24,
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
                    LeftOffset = 22,
                    RightOffset = 22,
                    TopOffset = 24,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 411,
                WinHeight = 180,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 39,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(CeltFrame);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Visible = false,
                Priority = 0,
                WinLeft = 21,
                WinTop = 213,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 413,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            Memo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "Memo",
                Priority = 0,
                WinLeft = 148,
                WinTop = 55,
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
                    LeftOffset = 148,
                    RightOffset = 101,
                    TopOffset = 55,
                    BottomOffset = 0
                },
                Masked = true,
                WinWidth = 206,
                WinHeight = 118,
                BorderWidth = 4,
                BorderHeight = 4,
                TextColor = 16777215,
                CursorChar = (byte)124,
                BlinkDelay = (byte)(0),
                Editable = false,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 22,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(Memo);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 500,
                WinLeft = 304,
                WinTop = 210,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 66,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 4,
                    RightOffset = 32,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 119,
                OnClick = Layout_CancelBtnClick_1
            };
            LayoutSession.EndRead(CancelBtn);
            AttackBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AttackBtn",
                Priority = 500,
                WinLeft = 165,
                WinTop = 210,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 36,
                    RightOffPercent = 36,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Attack",
                TextColor = 328543,
                TextSelColor = 16777215,
                WinWidth = 125,
                OnClick = Layout_AttackBtnClick_2
            };
            LayoutSession.EndRead(AttackBtn);
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
                Text = "City Requester",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            AcceptBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "AcceptBtn",
                Priority = 450,
                WinLeft = 32,
                WinTop = 210,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 66,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 32,
                    RightOffset = 4,
                    TopOffset = 0,
                    BottomOffset = 18
                },
                Masked = false,
                BorderHeight = 6,
                BorderHeightSelected = 7,
                ILIndexDown = 37,
                ILIndexLit = 40,
                ILIndexUp = 34,
                Text = "Accept",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 119,
                OnClick = Layout_AcceptBtnClick_3
            };
            LayoutSession.EndRead(AcceptBtn);
            WhatLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "WhatLbl",
                Priority = 1000,
                WinLeft = 150,
                WinTop = 184,
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
                    LeftOffset = 150,
                    RightOffset = 0,
                    TopOffset = 184,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "What do you do?",
                TextColor = 7921903
            };
            LayoutSession.EndRead(WhatLbl);
            CityPicLine = new Aow.Units.AoWArnoLine.TAoWArnoLine(this)
            {
                Name = "CityPicLine",
                Priority = 0,
                WinLeft = 426,
                WinTop = 50,
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
                    LeftOffset = 7,
                    RightOffset = 29,
                    TopOffset = 50,
                    BottomOffset = 75
                },
                Masked = false,
                WinHeight = 125
            };
            LayoutSession.EndRead(CityPicLine);
            CloseBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBtn",
                Priority = 0,
                WinLeft = 418,
                WinTop = 4,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Closes the window."
                },
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
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 1,
                ILIndexLit = 2,
                ILIndexUp = 0,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 15,
                OnClick = Layout_CloseBtnClick_4
            };
            LayoutSession.EndRead(CloseBtn);
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
            ApproachLbl.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            CeltFrame.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            Memo.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            AttackBtn.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            AcceptBtn.AOWWindow = Dlg;
            WhatLbl.AOWWindow = Dlg;
            CityPicLine.AOWWindow = Dlg;
            CloseBtn.AOWWindow = Dlg;
            Loaded();
            Dlg.Loaded();
            ApproachLbl.Loaded();
            TCeltLineL.Loaded();
            CeltFrame.Loaded();
            BCeltL.Loaded();
            Memo.Loaded();
            CancelBtn.Loaded();
            AttackBtn.Loaded();
            TitleIcon.Loaded();
            Title.Loaded();
            AcceptBtn.Loaded();
            WhatLbl.Loaded();
            CityPicLine.Loaded();
            CloseBtn.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            ApproachLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CeltFrame.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Memo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            Memo.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            AttackBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AttackBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AcceptBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            AcceptBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            AcceptBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            WhatLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            CloseBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            CloseBtn.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
        }

        private void Layout_DlgHide_0(TObject p0)
        {
            DlgHide();
        }

        private void Layout_CancelBtnClick_1(TObject p0)
        {
            CancelBtnClick();
        }

        private void Layout_AttackBtnClick_2(TObject p0)
        {
            AttackBtnClick();
        }

        private void Layout_AcceptBtnClick_3(TObject p0)
        {
            AcceptBtnClick();
        }

        private void Layout_CloseBtnClick_4(TObject p0)
        {
            CloseBtnClick();
        }
    }
}
