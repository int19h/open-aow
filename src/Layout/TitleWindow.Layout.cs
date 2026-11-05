// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Rtl;
using Aow.Layout;
using Aow.Units.AoWWindow;
using Aow.Units.AoWComp;
using Aow.Units.AoWPanel;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AOWTitleWindow
{
    public partial class TTitleWindow : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "TitleWindow";
            TitleWin = new TAOWWindow(this);
            session.BeginRead(TitleWin);
            TitleWin.Name = "TitleWin";
            TitleWin.Visible = false;
            TitleWin.Priority = 250;
            TitleWin.WinLeft = 0;
            TitleWin.WinTop = 0;
            TitleWin.WinWidth = 640;
            TitleWin.Masked = false;
            TitleWin.Alignment.AlignWidth = TAOWAlignWidth.awBoth;
            TitleWin.Alignment.AlignHeight = TAOWAlignHeight.ahBoth;
            TitleWin.Alignment.HeightPercent = 0;
            TitleWin.Alignment.WidthPercent = 0;
            TitleWin.Alignment.LeftOffPercent = 0;
            TitleWin.Alignment.RightOffPercent = 0;
            TitleWin.Alignment.TopOffPercent = 0;
            TitleWin.Alignment.BottomOffPercent = 0;
            TitleWin.Alignment.LeftOffset = 0;
            TitleWin.Alignment.RightOffset = 0;
            TitleWin.Alignment.TopOffset = 0;
            TitleWin.Alignment.BottomOffset = 0;
            TitleWin.UseVMem = false;
            TitleWin.BorderWidth = 0;
            TitleWin.BorderHeight = 0;
            TitleWin.OverlapTopCorners = false;
            TitleWin.Cursor = 0;
            TitleWin.DoubleBuffered = true;
            TitleWin.MaskColor = 0;
            TitleWin.OnDrawSurface = TitleWinDrawSurface;
            TitleWin.FreeSurfaceOnHide = true;
            TitleWin.ILIndexFrame = 0;
            TitleWin.ILIndexTexture = 18;
            TitleWin.DragHeight = 0;
            TitleWin.Draggable = false;
            TitleWin.Resizable = false;
            TitleWin.MinWidth = 640;
            TitleWin.MinHeight = 480;
            TitleWin.MaxWidth = 0;
            TitleWin.MaxHeight = 0;
            TitleWin.UseManagerBorders = false;
            TitleWin.Modal = false;
            TitleWin.WinHeight = 480;
            TitleWin.Dockable = false;
            session.EndRead(TitleWin);
            TitlePnl = new TAOWPanel(this);
            session.BeginRead(TitlePnl);
            TitlePnl.Name = "TitlePnl";
            TitlePnl.Visible = false;
            TitlePnl.Priority = 5;
            TitlePnl.WinLeft = 482;
            TitlePnl.WinTop = 0;
            TitlePnl.WinWidth = 640;
            TitlePnl.OnDrawBack = TitlePnlDrawBack;
            TitlePnl.Masked = false;
            TitlePnl.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            TitlePnl.Alignment.AlignHeight = TAOWAlignHeight.ahCenter;
            TitlePnl.Alignment.HeightPercent = 0;
            TitlePnl.Alignment.WidthPercent = 0;
            TitlePnl.Alignment.LeftOffPercent = 0;
            TitlePnl.Alignment.RightOffPercent = 0;
            TitlePnl.Alignment.TopOffPercent = 0;
            TitlePnl.Alignment.BottomOffPercent = 0;
            TitlePnl.Alignment.LeftOffset = 0;
            TitlePnl.Alignment.RightOffset = 0;
            TitlePnl.Alignment.TopOffset = 0;
            TitlePnl.Alignment.BottomOffset = 0;
            TitlePnl.WinHeight = 480;
            TitlePnl.ILIndexFrame = 0;
            TitlePnl.ILIndexTexture = 0;
            TitlePnl.OwnerItem = 0;
            TitlePnl.MinWidth = 640;
            TitlePnl.MinHeight = 480;
            session.EndRead(TitlePnl);
            SplashPnl = new TAOWPanel(this);
            session.BeginRead(SplashPnl);
            SplashPnl.Name = "SplashPnl";
            SplashPnl.OnMouseDown = SplashPnlMouseDown;
            SplashPnl.OnKeyDown = SplashPnlKeyDown;
            SplashPnl.OnUpdate = Layout_SplashPnlUpdate_0;
            SplashPnl.Visible = false;
            SplashPnl.Priority = 10;
            SplashPnl.WinLeft = 0;
            SplashPnl.WinTop = 0;
            SplashPnl.WinWidth = 640;
            SplashPnl.Masked = true;
            SplashPnl.Alignment.AlignWidth = TAOWAlignWidth.awCenter;
            SplashPnl.Alignment.AlignHeight = TAOWAlignHeight.ahCenter;
            SplashPnl.Alignment.HeightPercent = 0;
            SplashPnl.Alignment.WidthPercent = 0;
            SplashPnl.Alignment.LeftOffPercent = 0;
            SplashPnl.Alignment.RightOffPercent = 0;
            SplashPnl.Alignment.TopOffPercent = 0;
            SplashPnl.Alignment.BottomOffPercent = 0;
            SplashPnl.Alignment.LeftOffset = 0;
            SplashPnl.Alignment.RightOffset = 0;
            SplashPnl.Alignment.TopOffset = 0;
            SplashPnl.Alignment.BottomOffset = 0;
            SplashPnl.WinHeight = 480;
            SplashPnl.ILIndexFrame = 0;
            SplashPnl.ILIndexTexture = 4;
            SplashPnl.OwnerItem = 0;
            SplashPnl.MinWidth = 640;
            SplashPnl.MinHeight = 480;
            session.EndRead(SplashPnl);
            DemoIL = new TInterfaceIL(this);
            session.BeginRead(DemoIL);
            DemoIL.Name = "DemoIL";
            DemoIL.LoadOnDemand = true;
            DemoIL.FileName = "IMAGES\\Demo.ILB";
            session.EndRead(DemoIL);
            session.EndRead(this);
            TitlePnl.AOWWindow = TitleWin;
            SplashPnl.AOWWindow = TitleWin;
            Loaded();
            TitleWin.Loaded();
            TitlePnl.Loaded();
            SplashPnl.Loaded();
            DemoIL.Loaded();
            session.Resolve(this);
            TitleWin.WinManager = AoWWinMan.AoWWinMan.General.Manager;
            TitleWin.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.GenericT;
            TitlePnl.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
            SplashPnl.ILTexture = aowIntGfxMod.aowIntGfxMod.IntGfxMod.TitleScreen;
        }

        private void Layout_SplashPnlUpdate_0(TObject p0)
        {
            SplashPnlUpdate();
        }
    }
}
