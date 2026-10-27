// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWHeroJoinDlg
{
    public partial class THeroJoinDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "HeroJoinDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_HeroJoinDlgCreate_0;
            LayoutOnDestroy = Layout_HeroJoinDlgDestroy_1;
            UnitPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "UnitPnl",
                Priority = 1,
                WinLeft = 70,
                WinTop = 235,
                WinWidth = 345,
                Masked = true,
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
                    LeftOffset = 70,
                    RightOffset = 65,
                    TopOffset = 254,
                    BottomOffset = 81
                },
                WinHeight = 60,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(UnitPnl);
            U1 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U1",
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 0,
                    RightOffPercent = 88,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U1);
            U2 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U2",
                Tag = 1,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 43,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 12,
                    RightOffPercent = 75,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U2);
            U4 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U4",
                Tag = 3,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 129,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 37,
                    RightOffPercent = 50,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U4);
            U5 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U5",
                Tag = 4,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 172,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 50,
                    RightOffPercent = 38,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U5);
            U3 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U3",
                Tag = 2,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 86,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 25,
                    RightOffPercent = 63,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U3);
            U6 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U6",
                Tag = 5,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 215,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 62,
                    RightOffPercent = 25,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 42,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U6);
            U7 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U7",
                Tag = 6,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 258,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 75,
                    RightOffPercent = 13,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 43,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U7);
            U8 = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "U8",
                Tag = 7,
                OnMouseDown = Layout_U1MouseDown_2,
                OnPreDraw = U1PreDraw,
                Priority = 0,
                WinLeft = 302,
                WinTop = 0,
                CaptureInput = true,
                Alignment =
                {
                    AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth,
                    AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth,
                    HeightPercent = (byte)(0),
                    WidthPercent = (byte)(0),
                    LeftOffPercent = 87,
                    RightOffPercent = 0,
                    TopOffPercent = 0,
                    BottomOffPercent = 0,
                    LeftOffset = 2,
                    RightOffset = 2,
                    TopOffset = 0,
                    BottomOffset = 7
                },
                Masked = false,
                WinWidth = 41,
                WinHeight = 53,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHollowBox,
                ILIndexFrame = 40,
                ILIndexLine = 0,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(U8);
            MiddleBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "MiddleBtn",
                Priority = 100,
                WinLeft = 184,
                WinTop = 303,
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
                    TopOffset = 0,
                    BottomOffset = 46
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Attack",
                TextColor = 328543,
                TextSelColor = 16777215,
                WinWidth = 112,
                OnClick = Layout_MiddleBtnClick_3
            };
            LayoutSession.EndRead(MiddleBtn);
            BCeltL = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BCeltL",
                Priority = 0,
                WinLeft = 66,
                WinTop = 303,
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
                    LeftOffset = 66,
                    RightOffset = 65,
                    TopOffset = 0,
                    BottomOffset = 46
                },
                Masked = false,
                WinWidth = 349,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(BCeltL);
            RightBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "RightBtn",
                Priority = 100,
                WinLeft = 298,
                WinTop = 303,
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
                    RightOffset = 70,
                    TopOffset = 0,
                    BottomOffset = 46
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Cancel",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 112,
                OnClick = Layout_RightBtnClick_4
            };
            LayoutSession.EndRead(RightBtn);
            LeftBtn = new Aow.Units.AoWButton.TAOWButton(this)
            {
                Name = "LeftBtn",
                Priority = 100,
                WinLeft = 71,
                WinTop = 303,
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
                    LeftOffset = 71,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 46
                },
                Masked = false,
                BorderHeight = 8,
                BorderHeightSelected = 8,
                ILIndexDown = 22,
                ILIndexLit = 25,
                ILIndexUp = 19,
                Text = "Accept",
                TextColor = 0,
                TextSelColor = 16777215,
                WinWidth = 112,
                OnClick = Layout_LeftBtnClick_5
            };
            LayoutSession.EndRead(LeftBtn);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                OnKeyDown = DlgKeyDown,
                Visible = false,
                Priority = 300,
                WinLeft = 80,
                WinTop = 28,
                WinWidth = 480,
                Masked = true,
                OnHide = Layout_DlgHide_6,
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
                ILIndexFrame = 16,
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
                WinHeight = 376,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            HeroName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeroName",
                Priority = 0,
                WinLeft = 80,
                WinTop = 148,
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
                    LeftOffset = 80,
                    RightOffset = 0,
                    TopOffset = 148,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#######",
                TextColor = 13827061
            };
            LayoutSession.EndRead(HeroName);
            HeroLevel = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "HeroLevel",
                Priority = 0,
                WinLeft = 328,
                WinTop = 148,
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
                    LeftOffset = 76,
                    RightOffset = 80,
                    TopOffset = 148,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "#########",
                TextColor = 13827061
            };
            LayoutSession.EndRead(HeroLevel);
            FaceFrame = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "FaceFrame",
                OnPreDraw = FaceFramePreDraw,
                Priority = 2000,
                WinLeft = 186,
                WinTop = 35,
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
                    LeftOffset = 186,
                    RightOffset = 0,
                    TopOffset = 35,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 104,
                AnimEnd = 38,
                WinWidth = 107,
                WinHeight = 143,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(FaceFrame);
            DetailsLbl = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "DetailsLbl",
                Priority = 9999,
                WinLeft = 170,
                WinTop = 291,
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
                    LeftOffset = 76,
                    RightOffset = 60,
                    TopOffset = 0,
                    BottomOffset = 74
                },
                Masked = true,
                Text = "Click on the unit(s) for details",
                TextColor = 2831663
            };
            LayoutSession.EndRead(DetailsLbl);
            JoinMemo = new Aow.Units.AoWMemo.TAOWMemo(this)
            {
                Name = "JoinMemo",
                Priority = 0,
                WinLeft = 75,
                WinTop = 175,
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
                    LeftOffset = 130,
                    RightOffset = 25,
                    TopOffset = 175,
                    BottomOffset = 130
                },
                Masked = true,
                WinWidth = 330,
                WinHeight = 210,
                LayoutItems = new LayoutStrings()
                {
                    "#########################"
                },
                BorderWidth = 0,
                BorderHeight = 8,
                TextColor = 13827061,
                CursorChar = (byte)95,
                BlinkDelay = (byte)(2),
                Editable = true,
                ILIndexFrame = 40,
                ILIndexTexture = 2,
                ItemHeight = 22,
                ColorChangeChar = (byte)0,
                TextAlign = Aow.Units.AoWComp.TaowTextAlign.taCenter
            };
            LayoutSession.EndRead(JoinMemo);
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
            UnitPnl.AOWWindow = Dlg;
            U1.AOWWindow = UnitPnl;
            U2.AOWWindow = UnitPnl;
            U4.AOWWindow = UnitPnl;
            U5.AOWWindow = UnitPnl;
            U3.AOWWindow = UnitPnl;
            U6.AOWWindow = UnitPnl;
            U7.AOWWindow = UnitPnl;
            U8.AOWWindow = UnitPnl;
            MiddleBtn.AOWWindow = Dlg;
            BCeltL.AOWWindow = Dlg;
            RightBtn.AOWWindow = Dlg;
            LeftBtn.AOWWindow = Dlg;
            HeroName.AOWWindow = Dlg;
            HeroLevel.AOWWindow = Dlg;
            FaceFrame.AOWWindow = Dlg;
            DetailsLbl.AOWWindow = Dlg;
            JoinMemo.AOWWindow = Dlg;
            Loaded();
            UnitPnl.Loaded();
            U1.Loaded();
            U2.Loaded();
            U4.Loaded();
            U5.Loaded();
            U3.Loaded();
            U6.Loaded();
            U7.Loaded();
            U8.Loaded();
            MiddleBtn.Loaded();
            BCeltL.Loaded();
            RightBtn.Loaded();
            LeftBtn.Loaded();
            Dlg.Loaded();
            HeroName.Loaded();
            HeroLevel.Loaded();
            FaceFrame.Loaded();
            DetailsLbl.Loaded();
            JoinMemo.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            U1.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U2.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U4.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U5.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U3.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U6.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U7.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            U8.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            MiddleBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            MiddleBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            BCeltL.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            RightBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            RightBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            RightBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            LeftBtn.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            LeftBtn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10;
            LeftBtn.ILButton = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericB;
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            Dlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            Dlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            HeroName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            HeroLevel.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            DetailsLbl.ToolTip = Aow.Units.AoWWinMan.AoWWinMan.General.ToolTip;
            DetailsLbl.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age8;
            JoinMemo.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            if (session.FireCreateEvents)
                LayoutOnCreate?.Invoke(this);
        }

        private void Layout_HeroJoinDlgCreate_0(TObject p0)
        {
            HeroJoinDlgCreate();
        }

        private void Layout_HeroJoinDlgDestroy_1(TObject p0)
        {
            HeroJoinDlgDestroy();
        }

        private void Layout_U1MouseDown_2(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            U1MouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_MiddleBtnClick_3(TObject p0)
        {
            MiddleBtnClick();
        }

        private void Layout_RightBtnClick_4(TObject p0)
        {
            RightBtnClick();
        }

        private void Layout_LeftBtnClick_5(TObject p0)
        {
            LeftBtnClick();
        }

        private void Layout_DlgHide_6(TObject p0)
        {
            DlgHide();
        }
    }
}
