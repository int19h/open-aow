// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWCampaignSettings
{
    public partial class TCampaignSettingsDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "CampaignSettingsDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
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
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 300,
                WinLeft = 158,
                WinTop = 126,
                WinWidth = 325,
                Masked = true,
                OnShow = DlgShow,
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
                WinHeight = 180,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            OKBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "OKBtn",
                Priority = 250,
                WinLeft = 30,
                WinTop = 143,
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
                    LeftOffset = 30,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 7,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "OK",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_OKBtnClick_0
            };
            LayoutSession.EndRead(OKBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 143,
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
                WinWidth = 283,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
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
                Text = "Campaign Settings",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 290,
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
                    RightOffset = 20,
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
                OnClick = Layout_CancelBtnClick_1
            };
            LayoutSession.EndRead(Close);
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
                CurrentImage = 11,
                AnimEnd = 11,
                WinWidth = 19,
                WinHeight = 16,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TitleIcon);
            DifficultyLevelLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DifficultyLevelLbl",
                Priority = 0,
                WinLeft = 35,
                WinTop = 70,
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Difficulty Level:",
                TextColor = 16777215
            };
            LayoutSession.EndRead(DifficultyLevelLbl);
            DifficultyLevelCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "DifficultyLevelCB",
                Tag = 8,
                Priority = 6170,
                WinLeft = 160,
                WinTop = 65,
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
                    LeftOffset = 160,
                    RightOffset = 30,
                    TopOffset = 65,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 135,
                BorderWidth = 6,
                BorderHeight = 5,
                ListBorderWidth = 7,
                ListBorderHeight = 5,
                NumberVisible = 5,
                Selected = -1,
                PriorityOpened = 6170,
                TextColor = 11002593,
                TextSelColor = 16777215,
                ILEIndex = 27,
                ILFIndex = 113,
                ILTnIndex = 21,
                ILTsIndex = 8,
                ItemHeight = 17
            };
            LayoutSession.EndRead(DifficultyLevelCB);
            CombatResolveModeSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "CombatResolveModeSB",
                Visible = false,
                Priority = 170,
                WinLeft = 458,
                WinTop = 43,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                Masked = false,
                Min = 0,
                Max = 3,
                SmallChange = 1,
                LargeChange = 4,
                Position = 0,
                WinHeight = 70,
                WinWidth = 17,
                NumberVisible = 3,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(CombatResolveModeSB);
            TurnModeLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnModeLbl",
                Priority = 0,
                WinLeft = 35,
                WinTop = 105,
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
                    LeftOffset = 35,
                    RightOffset = 0,
                    TopOffset = 105,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Turn Mode:",
                TextColor = 16777215
            };
            LayoutSession.EndRead(TurnModeLbl);
            TurnModeCB = new Aow.Units.AoWComboBox.TAOWComboBox(this)
            {
                Name = "TurnModeCB",
                Tag = 8,
                Priority = 5170,
                WinLeft = 160,
                WinTop = 100,
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
                    LeftOffset = 160,
                    RightOffset = 30,
                    TopOffset = 100,
                    BottomOffset = 4
                },
                Masked = false,
                WinWidth = 135,
                BorderWidth = 6,
                BorderHeight = 5,
                ListBorderWidth = 7,
                ListBorderHeight = 5,
                NumberVisible = 5,
                Selected = -1,
                PriorityOpened = 5170,
                TextColor = 11002593,
                TextSelColor = 16777215,
                ILEIndex = 27,
                ILFIndex = 113,
                ILTnIndex = 21,
                ILTsIndex = 8,
                ItemHeight = 17
            };
            LayoutSession.EndRead(TurnModeCB);
            AOWVScrollBar1 = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "AOWVScrollBar1",
                Visible = false,
                Priority = 5170,
                WinLeft = 278,
                WinTop = 119,
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
                WinHeight = 8,
                WinWidth = 17,
                NumberVisible = 1,
                VanishWhenFull = true,
                ILIndexIncUp = 58,
                ILIndexIncDown = 59,
                ILIndexDecUp = 56,
                ILIndexDecDown = 57,
                ILIndexBack = 63,
                ILIndexTab = 60
            };
            LayoutSession.EndRead(AOWVScrollBar1);
            AoWFrame1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "AoWFrame1",
                Priority = 0,
                WinLeft = 21,
                WinTop = 25,
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
                    LeftOffset = 21,
                    RightOffset = 21,
                    TopOffset = 25,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 283,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(AoWFrame1);
            CancelBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CancelBtn",
                Priority = 250,
                WinLeft = 177,
                WinTop = 143,
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
                    LeftOffset = 30,
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 7,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 120,
                OnClick = Layout_CancelBtnClick_1
            };
            LayoutSession.EndRead(CancelBtn);
            TCeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 25,
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
                    LeftOffset = 28,
                    RightOffset = 28,
                    TopOffset = 25,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 269,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLineL);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            OKBtn.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            DifficultyLevelLbl.AOWWindow = Dlg;
            DifficultyLevelCB.AOWWindow = Dlg;
            CombatResolveModeSB.AOWWindow = Dlg;
            TurnModeLbl.AOWWindow = Dlg;
            TurnModeCB.AOWWindow = Dlg;
            TurnModeCB.VScrollBar = AOWVScrollBar1;
            AOWVScrollBar1.AOWWindow = Dlg;
            AoWFrame1.AOWWindow = Dlg;
            CancelBtn.AOWWindow = Dlg;
            TCeltLineL.AOWWindow = Dlg;
            Loaded();
            Translator.Loaded();
            Dlg.Loaded();
            OKBtn.Loaded();
            BCeltL.Loaded();
            Title.Loaded();
            Close.Loaded();
            TitleIcon.Loaded();
            DifficultyLevelLbl.Loaded();
            DifficultyLevelCB.Loaded();
            CombatResolveModeSB.Loaded();
            TurnModeLbl.Loaded();
            TurnModeCB.Loaded();
            AOWVScrollBar1.Loaded();
            AoWFrame1.Loaded();
            CancelBtn.Loaded();
            TCeltLineL.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            OKBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            OKBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            DifficultyLevelLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            DifficultyLevelCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DifficultyLevelCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            DifficultyLevelCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DifficultyLevelCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DifficultyLevelCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            CombatResolveModeSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            CombatResolveModeSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            TurnModeLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            TurnModeCB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            TurnModeCB.ILEdit = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericE;
            TurnModeCB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            TurnModeCB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TurnModeCB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            AOWVScrollBar1.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AOWVScrollBar1.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            AoWFrame1.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            CancelBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CancelBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
        }

        private void Layout_OKBtnClick_0(TObject p0)
        {
            OKBtnClick();
        }

        private void Layout_CancelBtnClick_1(TObject p0)
        {
            CancelBtnClick();
        }
    }
}
