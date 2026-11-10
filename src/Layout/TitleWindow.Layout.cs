// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Draw.Images;
using Aow.Game.Layout;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Dialogs;
using Aow.Game.Ui.Windows;

namespace Aow.Game.Ui.Dialogs;

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
        TitleWin = new AOWWindow(this);
        session.BeginRead(TitleWin);
        TitleWin.Name = "TitleWin";
        TitleWin.Visible = false;
        TitleWin.Priority = 250;
        TitleWin.WinLeft = 0;
        TitleWin.WinTop = 0;
        TitleWin.WinWidth = 640;
        TitleWin.Masked = false;
        TitleWin.Alignment.AlignWidth = AOWAlignWidth.Both;
        TitleWin.Alignment.AlignHeight = AOWAlignHeight.Both;
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
        TitleWin.OnDrawSurface = (_, Surface, Left, Top) => TitleWinDrawSurface(Surface, Left, Top);
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
        TitlePnl = new AOWPanel(this);
        session.BeginRead(TitlePnl);
        TitlePnl.Name = "TitlePnl";
        TitlePnl.Visible = false;
        TitlePnl.Priority = 5;
        TitlePnl.WinLeft = 482;
        TitlePnl.WinTop = 0;
        TitlePnl.WinWidth = 640;
        TitlePnl.OnDrawBack = (_, SurfDesc) => TitlePnlDrawBack(SurfDesc);
        TitlePnl.Masked = false;
        TitlePnl.Alignment.AlignWidth = AOWAlignWidth.Center;
        TitlePnl.Alignment.AlignHeight = AOWAlignHeight.Center;
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
        SplashPnl = new AOWPanel(this);
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
        SplashPnl.Alignment.AlignWidth = AOWAlignWidth.Center;
        SplashPnl.Alignment.AlignHeight = AOWAlignHeight.Center;
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
        DemoIL = new InterfaceIL(this);
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
        TitleWin.WinManager = Aow.Game.Ui.Dialogs.DialogHosts.General.Manager;
        TitleWin.ILTexture = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.GenericT;
        TitlePnl.ILTexture = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.TitleScreen;
        SplashPnl.ILTexture = Aow.Game.Ui.Dialogs.DialogHosts.IntGfxMod.TitleScreen;
    }

    private void Layout_SplashPnlUpdate_0()
    {
        SplashPnlUpdate();
    }
}
