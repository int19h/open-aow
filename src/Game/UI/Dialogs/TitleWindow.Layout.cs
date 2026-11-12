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
        MainWindow = new Window(this);
        session.BeginRead(MainWindow);
        MainWindow.Name = "TitleWin";
        MainWindow.Visible = false;
        MainWindow.Priority = 250;
        MainWindow.WinLeft = 0;
        MainWindow.WinTop = 0;
        MainWindow.WinWidth = 640;
        MainWindow.Masked = false;
        MainWindow.Alignment.AlignWidth = AlignWidth.Both;
        MainWindow.Alignment.AlignHeight = AlignHeight.Both;
        MainWindow.Alignment.HeightPercent = 0;
        MainWindow.Alignment.WidthPercent = 0;
        MainWindow.Alignment.LeftOffsetPercent = 0;
        MainWindow.Alignment.RightOffsetPercent = 0;
        MainWindow.Alignment.TopOffsetPercent = 0;
        MainWindow.Alignment.BottomOffsetPercent = 0;
        MainWindow.Alignment.LeftOffset = 0;
        MainWindow.Alignment.RightOffset = 0;
        MainWindow.Alignment.TopOffset = 0;
        MainWindow.Alignment.BottomOffset = 0;
        MainWindow.UseVideoMemory = false;
        MainWindow.BorderWidth = 0;
        MainWindow.BorderHeight = 0;
        MainWindow.OverlapTopCorners = false;
        MainWindow.Cursor = 0;
        MainWindow.DoubleBuffered = true;
        MainWindow.MaskColor = 0;
        MainWindow.OnDrawSurface += (_, surface, left, top) => TitleWinDrawSurface(surface, left, top);
        MainWindow.FreeSurfaceOnHide = true;
        MainWindow.ImageLibraryIndexFrame = 0;
        MainWindow.ImageLibraryIndexTexture = 18;
        MainWindow.DragHeight = 0;
        MainWindow.Draggable = false;
        MainWindow.Resizable = false;
        MainWindow.MinWidth = 640;
        MainWindow.MinHeight = 480;
        MainWindow.MaxWidth = 0;
        MainWindow.MaxHeight = 0;
        MainWindow.UseManagerBorders = false;
        MainWindow.Modal = false;
        MainWindow.WinHeight = 480;
        MainWindow.Dockable = false;
        session.EndRead(MainWindow);
        TitlePanel = new Panel(this);
        session.BeginRead(TitlePanel);
        TitlePanel.Name = "TitlePnl";
        TitlePanel.Visible = false;
        TitlePanel.Priority = 5;
        TitlePanel.WinLeft = 482;
        TitlePanel.WinTop = 0;
        TitlePanel.WinWidth = 640;
        TitlePanel.OnDrawBack += (_, surface) => TitlePnlDrawBack(surface);
        TitlePanel.Masked = false;
        TitlePanel.Alignment.AlignWidth = AlignWidth.Center;
        TitlePanel.Alignment.AlignHeight = AlignHeight.Center;
        TitlePanel.Alignment.HeightPercent = 0;
        TitlePanel.Alignment.WidthPercent = 0;
        TitlePanel.Alignment.LeftOffsetPercent = 0;
        TitlePanel.Alignment.RightOffsetPercent = 0;
        TitlePanel.Alignment.TopOffsetPercent = 0;
        TitlePanel.Alignment.BottomOffsetPercent = 0;
        TitlePanel.Alignment.LeftOffset = 0;
        TitlePanel.Alignment.RightOffset = 0;
        TitlePanel.Alignment.TopOffset = 0;
        TitlePanel.Alignment.BottomOffset = 0;
        TitlePanel.WinHeight = 480;
        TitlePanel.ImageLibraryIndexFrame = 0;
        TitlePanel.ImageLibraryIndexTexture = 0;
        TitlePanel.OwnerItem = 0;
        TitlePanel.MinWidth = 640;
        TitlePanel.MinHeight = 480;
        session.EndRead(TitlePanel);
        SplashPanel = new Panel(this);
        session.BeginRead(SplashPanel);
        SplashPanel.Name = "SplashPnl";
        SplashPanel.OnMouseDown += (_, button, shift, x, y) => SplashPnlMouseDown(button, shift, x, y);
        SplashPanel.OnKeyDown += (_, ref key, shift) => SplashPnlKeyDown(ref key, shift);
        SplashPanel.OnUpdate += _ => SplashPnlUpdate();
        SplashPanel.Visible = false;
        SplashPanel.Priority = 10;
        SplashPanel.WinLeft = 0;
        SplashPanel.WinTop = 0;
        SplashPanel.WinWidth = 640;
        SplashPanel.Masked = true;
        SplashPanel.Alignment.AlignWidth = AlignWidth.Center;
        SplashPanel.Alignment.AlignHeight = AlignHeight.Center;
        SplashPanel.Alignment.HeightPercent = 0;
        SplashPanel.Alignment.WidthPercent = 0;
        SplashPanel.Alignment.LeftOffsetPercent = 0;
        SplashPanel.Alignment.RightOffsetPercent = 0;
        SplashPanel.Alignment.TopOffsetPercent = 0;
        SplashPanel.Alignment.BottomOffsetPercent = 0;
        SplashPanel.Alignment.LeftOffset = 0;
        SplashPanel.Alignment.RightOffset = 0;
        SplashPanel.Alignment.TopOffset = 0;
        SplashPanel.Alignment.BottomOffset = 0;
        SplashPanel.WinHeight = 480;
        SplashPanel.ImageLibraryIndexFrame = 0;
        SplashPanel.ImageLibraryIndexTexture = 4;
        SplashPanel.OwnerItem = 0;
        SplashPanel.MinWidth = 640;
        SplashPanel.MinHeight = 480;
        session.EndRead(SplashPanel);
        DemoImages = new InterfaceImageLibrary(this);
        session.BeginRead(DemoImages);
        DemoImages.Name = "DemoIL";
        DemoImages.LoadOnDemand = true;
        DemoImages.FileName = "IMAGES\\Demo.ILB";
        session.EndRead(DemoImages);
        session.EndRead(this);
        TitlePanel.Window = MainWindow;
        SplashPanel.Window = MainWindow;
        Loaded();
        MainWindow.Loaded();
        TitlePanel.Loaded();
        SplashPanel.Loaded();
        DemoImages.Loaded();
        session.Resolve(this);
        MainWindow.WindowManager = UI.General.Manager;
        MainWindow.ImageLibraryTexture = UI.Graphics.GenericTextures;
        TitlePanel.ImageLibraryTexture = UI.Graphics.TitleScreen;
        SplashPanel.ImageLibraryTexture = UI.Graphics.TitleScreen;
    }
}
