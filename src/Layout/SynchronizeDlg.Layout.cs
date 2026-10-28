// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWSyncDlg
{
    public partial class TSynchronizeDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "SynchronizeDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Dlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "Dlg",
                Visible = false,
                Priority = 1000,
                WinLeft = 80,
                WinTop = 45,
                WinWidth = 480,
                Masked = true,
                OnShow = Layout_DlgShow_0,
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
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = true,
                WinHeight = 343,
                Dockable = false
            };
            LayoutSession.EndRead(Dlg);
            TCeltB = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "TCeltB",
                Priority = 0,
                WinLeft = 66,
                WinTop = 272,
                CaptureInput = false,
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
                    RightOffset = 62,
                    TopOffset = 22,
                    BottomOffset = 44
                },
                Masked = false,
                WinWidth = 352,
                WinHeight = 27,
                FrameType = Aow.Units.AoWFrame.TFrameType.frHorizLine,
                ILIndexFrame = 0,
                ILIndexLine = 55,
                ILIndexTexture = 0
            };
            LayoutSession.EndRead(TCeltB);
            BottomMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "BottomMsg",
                Priority = 0,
                WinLeft = 240,
                WinTop = 292,
                CaptureInput = false,
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
                    BottomOffset = 51
                },
                Masked = true,
                TextColor = 9236989
            };
            LayoutSession.EndRead(BottomMsg);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Priority = 1000,
                WinLeft = 70,
                WinTop = 190,
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
                    LeftOffset = 70,
                    RightOffset = 0,
                    TopOffset = 190,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 33,
                AnimEnd = 38,
                WinWidth = 44,
                WinHeight = 60,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(LShield);
            PlayerName = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerName",
                Priority = 100,
                WinLeft = 210,
                WinTop = 198,
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
                    TopOffset = 198,
                    BottomOffset = 50
                },
                Masked = true,
                Text = "######",
                TextColor = 9757931
            };
            LayoutSession.EndRead(PlayerName);
            InfoMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "InfoMsg",
                Priority = 100,
                WinLeft = 139,
                WinTop = 228,
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
                    TopOffset = 228,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "Waiting for other players",
                TextColor = 16777215
            };
            LayoutSession.EndRead(InfoMsg);
            PlayerRace = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "PlayerRace",
                Priority = 0,
                WinLeft = 76,
                WinTop = 148,
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
                    LeftOffset = 76,
                    RightOffset = 0,
                    TopOffset = 148,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 9236989
            };
            LayoutSession.EndRead(PlayerRace);
            Turn = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Turn",
                Priority = 0,
                WinLeft = 356,
                WinTop = 148,
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
                    LeftOffset = 76,
                    RightOffset = 76,
                    TopOffset = 148,
                    BottomOffset = 0
                },
                Masked = true,
                Text = "######",
                TextColor = 9236989
            };
            LayoutSession.EndRead(Turn);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Priority = 1000,
                WinLeft = 366,
                WinTop = 190,
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
                    RightOffset = 70,
                    TopOffset = 190,
                    BottomOffset = 0
                },
                Masked = false,
                AnimStart = 0,
                CurrentImage = 33,
                AnimEnd = 38,
                WinWidth = 44,
                WinHeight = 60,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(RShield);
            BlueFr = new Aow.Units.AoWFrame.TAoWFrame(this)
            {
                Name = "BlueFr",
                Priority = 0,
                WinLeft = 120,
                WinTop = 185,
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
                    LeftOffset = 120,
                    RightOffset = 120,
                    TopOffset = 185,
                    BottomOffset = 85
                },
                Masked = false,
                WinWidth = 240,
                WinHeight = 73,
                FrameType = Aow.Units.AoWFrame.TFrameType.frFilledBox,
                ILIndexFrame = 30,
                ILIndexLine = 0,
                ILIndexTexture = 14
            };
            LayoutSession.EndRead(BlueFr);
            TCeltLineL = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineL",
                Priority = 100,
                WinLeft = 72,
                WinTop = 272,
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
                    LeftOffset = 72,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 44
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
            LayoutSession.EndRead(TCeltLineL);
            TCeltLineR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineR",
                Priority = 100,
                WinLeft = 329,
                WinTop = 272,
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
                    BottomOffset = 44
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
            LayoutSession.EndRead(TCeltLineR);
            NewTPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "NewTPnl",
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 480,
                Masked = false,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                WinHeight = 343,
                ILIndexFrame = 16,
                ILIndexTexture = 2,
                OwnerItem = 0,
                MinWidth = 0,
                MinHeight = 0
            };
            LayoutSession.EndRead(NewTPnl);
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
            TCeltB.AOWWindow = NewTPnl;
            BottomMsg.AOWWindow = NewTPnl;
            LShield.AOWWindow = NewTPnl;
            PlayerName.AOWWindow = NewTPnl;
            InfoMsg.AOWWindow = NewTPnl;
            PlayerRace.AOWWindow = NewTPnl;
            Turn.AOWWindow = NewTPnl;
            RShield.AOWWindow = NewTPnl;
            BlueFr.AOWWindow = NewTPnl;
            TCeltLineL.AOWWindow = NewTPnl;
            TCeltLineR.AOWWindow = NewTPnl;
            NewTPnl.AOWWindow = Dlg;
            FaceFrame.AOWWindow = NewTPnl;
            Loaded();
            Dlg.Loaded();
            TCeltB.Loaded();
            BottomMsg.Loaded();
            LShield.Loaded();
            PlayerName.Loaded();
            InfoMsg.Loaded();
            PlayerRace.Loaded();
            Turn.Loaded();
            RShield.Loaded();
            BlueFr.Loaded();
            TCeltLineL.Loaded();
            TCeltLineR.Loaded();
            NewTPnl.Loaded();
            FaceFrame.Loaded();
            Translator.Loaded();
            session.Resolve(this);
            Dlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TCeltB.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BottomMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            PlayerName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            InfoMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            PlayerRace.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Turn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            BlueFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BlueFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            NewTPnl.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewTPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_DlgShow_0(TObject p0)
        {
            DlgShow();
        }
    }
}
