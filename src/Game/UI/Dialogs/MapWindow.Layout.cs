// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.UI.Dialogs;

public partial class MapWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MWindow";
        LayoutOnCreate += _ => MWindowCreate();
        MainWindow = new Window(this);
        session.BeginRead(MainWindow);
        MainWindow.Name = "MapWindow";
        MainWindow.OnMouseDown += (_, button, shift, x, y) => MapWindowMouseDown(button, shift, x, y);
        MainWindow.OnMouseUp += (_, button, shift, x, y) => MapWindowMouseUp(button, shift, x, y);
        MainWindow.OnMouseMove += (_, shift, x, y) => MapWindowMouseMove(shift, x, y);
        MainWindow.OnKeyDown += MapWindowKeyDown;
        MainWindow.OnUpdate += _ => MapWindowUpdate();
        MainWindow.Visible = false;
        MainWindow.Priority = 0;
        MainWindow.WinLeft = 0;
        MainWindow.WinTop = 0;
        MainWindow.WinWidth = 440;
        MainWindow.OnExit += _ => MapWindowExit();
        MainWindow.Masked = false;
        MainWindow.Alignment.AlignWidth = AlignWidth.Left;
        MainWindow.Alignment.AlignHeight = AlignHeight.Top;
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
        MainWindow.UseVideoMemory = true;
        MainWindow.BorderWidth = 11;
        MainWindow.BorderHeight = 11;
        MainWindow.OverlapTopCorners = false;
        MainWindow.Cursor = 0;
        MainWindow.DoubleBuffered = false;
        MainWindow.MaskColor = 0;
        MainWindow.OnPreDrawSurface += (_, surface, left, top) => MapWindowPreDrawSurface(surface, left, top);
        MainWindow.FreeSurfaceOnHide = false;
        MainWindow.ImageLibraryIndexFrame = 8;
        MainWindow.ImageLibraryIndexTexture = 0;
        MainWindow.DragHeight = 0;
        MainWindow.Draggable = false;
        MainWindow.Resizable = true;
        MainWindow.OnResize += _ => MapWindowResize();
        MainWindow.MinWidth = 200;
        MainWindow.MinHeight = 200;
        MainWindow.MaxWidth = 0;
        MainWindow.MaxHeight = 0;
        MainWindow.UseManagerBorders = true;
        MainWindow.Modal = false;
        MainWindow.WinHeight = 432;
        MainWindow.Dockable = false;
        session.EndRead(MainWindow);
        MapViewer = new HexSpriteMapViewer(this);
        session.BeginRead(MapViewer);
        MapViewer.Name = "MapViewer";
        MapViewer.OnSceneChanged += _ => MapViewerSceneChanged();
        session.EndRead(MapViewer);
        session.EndRead(this);
        Loaded();
        MainWindow.Loaded();
        MapViewer.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }
}
