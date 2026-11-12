// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

public partial class TitleWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "TitleWindow";
        TitleWin = new Window(this);
        session.BeginRead(TitleWin);
        TitleWin.Name = "TitleWin";
        TitleWin.Visible = false;
        TitleWin.Priority = 250;
        TitleWin.WinLeft = 0;
        TitleWin.WinTop = 0;
        TitleWin.WinWidth = 640;
        TitleWin.Masked = false;
        TitleWin.Alignment.AlignWidth = AlignWidth.Both;
        TitleWin.Alignment.AlignHeight = AlignHeight.Both;
        TitleWin.Alignment.HeightPercent = 0;
        TitleWin.Alignment.WidthPercent = 0;
        TitleWin.Alignment.LeftOffsetPercent = 0;
        TitleWin.Alignment.RightOffsetPercent = 0;
        TitleWin.Alignment.TopOffsetPercent = 0;
        TitleWin.Alignment.BottomOffsetPercent = 0;
        TitleWin.Alignment.LeftOffset = 0;
        TitleWin.Alignment.RightOffset = 0;
        TitleWin.Alignment.TopOffset = 0;
        TitleWin.Alignment.BottomOffset = 0;
        TitleWin.UseVideoMemory = false;
        TitleWin.BorderWidth = 0;
        TitleWin.BorderHeight = 0;
        TitleWin.OverlapTopCorners = false;
        TitleWin.Cursor = 0;
        TitleWin.DoubleBuffered = true;
        TitleWin.MaskColor = 0;
        TitleWin.OnDrawSurface = (_, surface, left, top) => TitleWinDrawSurface(surface, left, top);
        TitleWin.FreeSurfaceOnHide = true;
        TitleWin.ImageLibraryIndexFrame = 0;
        TitleWin.ImageLibraryIndexTexture = 18;
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
        TitlePnl = new Panel(this);
        session.BeginRead(TitlePnl);
        TitlePnl.Name = "TitlePnl";
        TitlePnl.Visible = false;
        TitlePnl.Priority = 5;
        TitlePnl.WinLeft = 482;
        TitlePnl.WinTop = 0;
        TitlePnl.WinWidth = 640;
        TitlePnl.OnDrawBack = (_, lockedSurface) => TitlePnlDrawBack(lockedSurface);
        TitlePnl.Masked = false;
        TitlePnl.Alignment.AlignWidth = AlignWidth.Center;
        TitlePnl.Alignment.AlignHeight = AlignHeight.Center;
        TitlePnl.Alignment.HeightPercent = 0;
        TitlePnl.Alignment.WidthPercent = 0;
        TitlePnl.Alignment.LeftOffsetPercent = 0;
        TitlePnl.Alignment.RightOffsetPercent = 0;
        TitlePnl.Alignment.TopOffsetPercent = 0;
        TitlePnl.Alignment.BottomOffsetPercent = 0;
        TitlePnl.Alignment.LeftOffset = 0;
        TitlePnl.Alignment.RightOffset = 0;
        TitlePnl.Alignment.TopOffset = 0;
        TitlePnl.Alignment.BottomOffset = 0;
        TitlePnl.WinHeight = 480;
        TitlePnl.ImageLibraryIndexFrame = 0;
        TitlePnl.ImageLibraryIndexTexture = 0;
        TitlePnl.OwnerItem = 0;
        TitlePnl.MinWidth = 640;
        TitlePnl.MinHeight = 480;
        session.EndRead(TitlePnl);
        SplashPnl = new Panel(this);
        session.BeginRead(SplashPnl);
        SplashPnl.Name = "SplashPnl";
        SplashPnl.OnMouseDown = (_, button, shift, x, y) => SplashPnlMouseDown(button, shift, x, y);
        SplashPnl.OnKeyDown = (_, ref key, shift) => SplashPnlKeyDown(ref key, shift);
        SplashPnl.OnUpdate = _ => Layout_SplashPnlUpdate_0();
        SplashPnl.Visible = false;
        SplashPnl.Priority = 10;
        SplashPnl.WinLeft = 0;
        SplashPnl.WinTop = 0;
        SplashPnl.WinWidth = 640;
        SplashPnl.Masked = true;
        SplashPnl.Alignment.AlignWidth = AlignWidth.Center;
        SplashPnl.Alignment.AlignHeight = AlignHeight.Center;
        SplashPnl.Alignment.HeightPercent = 0;
        SplashPnl.Alignment.WidthPercent = 0;
        SplashPnl.Alignment.LeftOffsetPercent = 0;
        SplashPnl.Alignment.RightOffsetPercent = 0;
        SplashPnl.Alignment.TopOffsetPercent = 0;
        SplashPnl.Alignment.BottomOffsetPercent = 0;
        SplashPnl.Alignment.LeftOffset = 0;
        SplashPnl.Alignment.RightOffset = 0;
        SplashPnl.Alignment.TopOffset = 0;
        SplashPnl.Alignment.BottomOffset = 0;
        SplashPnl.WinHeight = 480;
        SplashPnl.ImageLibraryIndexFrame = 0;
        SplashPnl.ImageLibraryIndexTexture = 4;
        SplashPnl.OwnerItem = 0;
        SplashPnl.MinWidth = 640;
        SplashPnl.MinHeight = 480;
        session.EndRead(SplashPnl);
        DemoIL = new InterfaceImageLibrary(this);
        session.BeginRead(DemoIL);
        DemoIL.Name = "DemoIL";
        DemoIL.LoadOnDemand = true;
        DemoIL.FileName = "IMAGES\\Demo.ILB";
        session.EndRead(DemoIL);
        session.EndRead(this);
        TitlePnl.Window = TitleWin;
        SplashPnl.Window = TitleWin;
        Loaded();
        TitleWin.Loaded();
        TitlePnl.Loaded();
        SplashPnl.Loaded();
        DemoIL.Loaded();
        session.Resolve(this);
        TitleWin.WindowManager = DialogHosts.General.Manager;
        TitleWin.ImageLibraryTexture = DialogHosts.UIGraphics.GenericT;
        TitlePnl.ImageLibraryTexture = DialogHosts.UIGraphics.TitleScreen;
        SplashPnl.ImageLibraryTexture = DialogHosts.UIGraphics.TitleScreen;
    }

    private void Layout_SplashPnlUpdate_0()
    {
        SplashPnlUpdate();
    }
}
