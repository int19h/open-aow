// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWNewTurnDialog
{
    public partial class TNewTurnDlg
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "NewTurnDlg";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            NewTDlg = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "NewTDlg",
                OnMouseDown = NewTDlgMouseDown,
                OnKeyPress = NewTDlgKeyPress,
                Visible = false,
                Priority = 1000,
                WinLeft = 80,
                WinTop = 45,
                WinWidth = 480,
                Masked = true,
                OnHide = Layout_NewTDlgHide_0,
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
                ILIndexFrame = 16,
                ILIndexTexture = 2,
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
            LayoutSession.EndRead(NewTDlg);
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
            ClickMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "ClickMsg",
                Priority = 0,
                WinLeft = 220,
                WinTop = 280,
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
                Text = "#####",
                TextColor = 9236989
            };
            LayoutSession.EndRead(ClickMsg);
            LShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "LShield",
                Priority = 1000,
                WinLeft = 75,
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
                    LeftOffset = 75,
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
            TurnMsg = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "TurnMsg",
                Priority = 100,
                WinLeft = 200,
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
                Text = "########",
                TextColor = 16777215
            };
            LayoutSession.EndRead(TurnMsg);
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
                Text = "#######",
                TextColor = 9236989
            };
            LayoutSession.EndRead(PlayerRace);
            Turn = new Aow.Units.AoWLabel.TAOWLabel(this)
            {
                Name = "Turn",
                Priority = 0,
                WinLeft = 348,
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
                Text = "#######",
                TextColor = 9236989
            };
            LayoutSession.EndRead(Turn);
            RShield = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "RShield",
                Priority = 1000,
                WinLeft = 363,
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
                    RightOffset = 73,
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
                WinLeft = 129,
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
                    LeftOffset = 129,
                    RightOffset = 129,
                    TopOffset = 185,
                    BottomOffset = 85
                },
                Masked = false,
                WinWidth = 222,
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
                WinLeft = 71,
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
                    LeftOffset = 71,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 44
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
            LayoutSession.EndRead(TCeltLineL);
            TCeltLineR = new Aow.Units.AoWImage.TAOWImage(this)
            {
                Name = "TCeltLineR",
                Priority = 100,
                WinLeft = 356,
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
                WinWidth = 54,
                WinHeight = 27,
                Animate = false,
                AnimFrameDelay = 0
            };
            LayoutSession.EndRead(TCeltLineR);
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
            ComponentState = ComponentState.Without(TComponentState.csReading);
            TCeltB.AOWWindow = NewTDlg;
            ClickMsg.AOWWindow = NewTDlg;
            LShield.AOWWindow = NewTDlg;
            PlayerName.AOWWindow = NewTDlg;
            TurnMsg.AOWWindow = NewTDlg;
            PlayerRace.AOWWindow = NewTDlg;
            Turn.AOWWindow = NewTDlg;
            RShield.AOWWindow = NewTDlg;
            BlueFr.AOWWindow = NewTDlg;
            TCeltLineL.AOWWindow = NewTDlg;
            TCeltLineR.AOWWindow = NewTDlg;
            FaceFrame.AOWWindow = NewTDlg;
            Loaded();
            NewTDlg.Loaded();
            TCeltB.Loaded();
            ClickMsg.Loaded();
            LShield.Loaded();
            PlayerName.Loaded();
            TurnMsg.Loaded();
            PlayerRace.Loaded();
            Turn.Loaded();
            RShield.Loaded();
            BlueFr.Loaded();
            TCeltLineL.Loaded();
            TCeltLineR.Loaded();
            FaceFrame.Loaded();
            session.Resolve(this);
            NewTDlg.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            NewTDlg.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
            NewTDlg.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCeltB.ILLine = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            ClickMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            LShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            PlayerName.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            TurnMsg.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.AoW15WhiteGrey;
            PlayerRace.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            Turn.AOWFont = Aow.Units.AoWExeForms.AoWExeForms.FontModule.Age10s;
            RShield.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.Shields;
            BlueFr.ILFrame = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            BlueFr.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TCeltLineL.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            TCeltLineR.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.CeltDeco;
            FaceFrame.ImageLib = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericF;
        }

        private void Layout_NewTDlgHide_0(TObject p0)
        {
            NewTDlgHide();
        }
    }
}
