// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWVideoDlg
{
    public partial class TVideoDialog
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "VideoDialog";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_VideoDlgWindowCreate_0;
            LayoutOnDestroy = Layout_VideoDlgWindowDestroy_1;
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 150,
                WinTop = 76,
                WinWidth = 340,
                Masked = true,
                OnShow = Layout_DlgShow_2,
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
                WinHeight = 280,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            CloseBut = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "CloseBut",
                Priority = 250,
                WinLeft = 170,
                WinTop = 243,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awLeft,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Close",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 136,
                OnClick = Layout_CloseButClick_3
            };
            LayoutSession.EndRead(CloseBut);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 21,
                WinTop = 243,
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
                WinWidth = 298,
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
                Text = "Video Settings",
                TextColor = 0
            };
            LayoutSession.EndRead(Title);
            Close = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "Close",
                Priority = 0,
                WinLeft = 305,
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
                OnClick = Layout_CloseButClick_3
            };
            LayoutSession.EndRead(Close);
            TCeltLR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLR",
                Priority = 100,
                WinLeft = 258,
                WinTop = 243,
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
                    RightOffset = 28,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLR);
            TopLine = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TopLine",
                Priority = 0,
                WinLeft = 21,
                WinTop = 22,
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
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                WinWidth = 298,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TopLine);
            VideoLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "VideoLbl",
                Priority = 0,
                WinLeft = 50,
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 70,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Available Display Modes",
                TextColor = 16777215
            };
            LayoutSession.EndRead(VideoLbl);
            TCeltPattern = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltPattern",
                Priority = 1000,
                WinLeft = 28,
                WinTop = 22,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 22,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 270,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltPattern);
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
            TCeltLL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLL",
                Priority = 100,
                WinLeft = 28,
                WinTop = 243,
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
                    LeftOffset = 28,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 3,
                AnimEnd = 12,
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLL);
            DisplayModesLB = new Aow.Units.AoWListBox.TAOWListBox(this)
            {
                Name = "DisplayModesLB",
                Priority = 1000,
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
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 1
                },
                Masked = false,
                WinWidth = 239,
                WinHeight = 107,
                LayoutItems = new LayoutStrings()
                {
                    ""
                },
                BorderWidth = 6,
                BorderHeight = 4,
                OnDoubleClick = Layout_ApplyBtnClick_4,
                Selected = 0,
                TextColor = 9415072,
                TextSelColor = 0,
                ILIndexFrame = 113,
                ILIndexTNormal = 21,
                ILIndexTSelected = 8,
                ItemHeight = 17,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taLeft
            };
            LayoutSession.EndRead(DisplayModesLB);
            ResCB = new Aow.Units.AoWCheckBox.TAOWCheckBox(this)
            {
                Name = "ResCB",
                Priority = 0,
                WinLeft = 50,
                WinTop = 212,
                LayoutToolTipText = new LayoutStrings()
                {
                    "Selects whether to change to 640x480 resolution for some scenes, or stay in the current resolution."
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
                    LeftOffset = 50,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 56
                },
                Masked = false,
                BorderHeight = 0,
                BorderHeightSelected = 0,
                ILIndexDown = 46,
                ILIndexLit = 45,
                ILIndexUp = 45,
                TextColor = 0,
                TextSelColor = 0,
                WinWidth = 12,
                Checked = false,
                OnChange = ResCBChange,
                RadioIndex = 0
            };
            LayoutSession.EndRead(ResCB);
            ResSwLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ResSwLbl",
                Priority = 0,
                WinLeft = 70,
                WinTop = 214,
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 54
                },
                Masked = true,
                Text = "Allow Resolution Switches",
                TextColor = 16777215
            };
            LayoutSession.EndRead(ResSwLbl);
            ApplyBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "ApplyBtn",
                Priority = 250,
                WinLeft = 34,
                WinTop = 243,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awRight,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBottom,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 10
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Apply",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 136,
                OnClick = Layout_ApplyBtnClick_4
            };
            LayoutSession.EndRead(ApplyBtn);
            T3LBPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "T3LBPnl",
                Priority = 0,
                WinLeft = 50,
                WinTop = 90,
                WinWidth = 240,
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
                    LeftOffset = 50,
                    RightOffset = 50,
                    TopOffset = 90,
                    BottomOffset = 82
                },
                WinHeight = 108,
                ILIndexFrame = 105,
                ILIndexTexture = 0,
                OwnerItem = 1,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(T3LBPnl);
            VideoSB = new Aow.Units.AoWVScroll.TAOWVScrollBar(this)
            {
                Name = "VideoSB",
                Visible = false,
                Priority = 1001,
                WinLeft = 223,
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
                Max = 0,
                SmallChange = 1,
                LargeChange = 1,
                Position = 0,
                WinHeight = 107,
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
            LayoutSession.EndRead(VideoSB);
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
            CloseBut.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            Title.AOWWindow = Dlg;
            Close.AOWWindow = Dlg;
            TCeltLR.AOWWindow = Dlg;
            TopLine.AOWWindow = Dlg;
            VideoLbl.AOWWindow = Dlg;
            TCeltPattern.AOWWindow = Dlg;
            TitleIcon.AOWWindow = Dlg;
            TCeltLL.AOWWindow = Dlg;
            DisplayModesLB.AOWWindow = T3LBPnl;
            DisplayModesLB.VScrollBar = VideoSB;
            ResCB.AOWWindow = Dlg;
            ResSwLbl.AOWWindow = Dlg;
            ApplyBtn.AOWWindow = Dlg;
            T3LBPnl.AOWWindow = Dlg;
            VideoSB.AOWWindow = T3LBPnl;
            Loaded();
            Dlg.Loaded();
            CloseBut.Loaded();
            BCeltL.Loaded();
            Title.Loaded();
            Close.Loaded();
            TCeltLR.Loaded();
            TopLine.Loaded();
            VideoLbl.Loaded();
            TCeltPattern.Loaded();
            TitleIcon.Loaded();
            TCeltLL.Loaded();
            DisplayModesLB.Loaded();
            ResCB.Loaded();
            ResSwLbl.Loaded();
            ApplyBtn.Loaded();
            T3LBPnl.Loaded();
            VideoSB.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            CloseBut.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            CloseBut.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            Title.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            Close.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            Close.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            TCeltLR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TopLine.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            VideoLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            TCeltPattern.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TitleIcon.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleIcons;
            TCeltLL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            DisplayModesLB.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            DisplayModesLB.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DisplayModesLB.ILTextureNormal = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            DisplayModesLB.ILTextureSelected = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Menus;
            ResCB.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            ResCB.ILIcon = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericI;
            ResSwLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10bs;
            ApplyBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            ApplyBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            T3LBPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            VideoSB.ILIInc = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            VideoSB.ILIDec = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            VideoSB.ILTBack = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            VideoSB.ILLTab = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericS;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_VideoDlgWindowCreate_0(TObject p0)
        {
            VideoDlgWindowCreate();
        }

        private void Layout_VideoDlgWindowDestroy_1(TObject p0)
        {
            VideoDlgWindowDestroy();
        }

        private void Layout_DlgShow_2(TObject p0)
        {
            DlgShow();
        }

        private void Layout_CloseButClick_3(TObject p0)
        {
            CloseButClick();
        }

        private void Layout_ApplyBtnClick_4(TObject p0)
        {
            ApplyBtnClick();
        }
    }
}
