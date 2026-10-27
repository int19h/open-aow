// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWTitleWindow
{
    public partial class TTitleWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TitleWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            TitleWin = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "TitleWin",
                Visible = false,
                Priority = 250,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
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
                UseVMem = false,
                BorderWidth = 0,
                BorderHeight = 0,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = true,
                MaskColor = 0,
                OnDrawSurface = TitleWinDrawSurface,
                FreeSurfaceOnHide = true,
                ILIndexFrame = 0,
                ILIndexTexture = 18,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 640,
                MinHeight = 480,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = false,
                WinHeight = 480,
                Dockable = false
            };
            LayoutSession.EndRead(TitleWin);
            TitlePnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "TitlePnl",
                Visible = false,
                Priority = 5,
                WinLeft = 482,
                WinTop = 0,
                WinWidth = 640,
                OnDrawBack = TitlePnlDrawBack,
                Masked = false,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(TitlePnl);
            SplashPnl = new Aow.Units.AoWPanel.TAOWPanel(this)
            {
                Name = "SplashPnl",
                OnMouseDown = SplashPnlMouseDown,
                OnKeyDown = SplashPnlKeyDown,
                OnUpdate = Layout_SplashPnlUpdate_0,
                Visible = false,
                Priority = 10,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = true,
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
                WinHeight = 480,
                ILIndexFrame = 0,
                ILIndexTexture = 4,
                OwnerItem = 0,
                MinWidth = 640,
                MinHeight = 480
            };
            LayoutSession.EndRead(SplashPnl);
            DemoIL = new Aow.Units.AOWInterfaceImgLib.TInterfaceIL(this)
            {
                Name = "DemoIL",
                LoadOnDemand = true,
                FileName = "IMAGES\\Demo.ILB"
            };
            LayoutSession.EndRead(DemoIL);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            TitlePnl.AOWWindow = TitleWin;
            SplashPnl.AOWWindow = TitleWin;
            Loaded();
            TitleWin.Loaded();
            TitlePnl.Loaded();
            SplashPnl.Loaded();
            DemoIL.Loaded();
            session.Resolve(this);
            TitleWin.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
            TitleWin.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TitlePnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SplashPnl.ILTexture = Aow.Units.aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
        }

        private void Layout_SplashPnlUpdate_0(TObject p0)
        {
            SplashPnlUpdate();
        }
    }
}
