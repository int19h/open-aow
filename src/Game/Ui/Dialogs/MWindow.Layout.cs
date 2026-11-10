// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Game.World.Map;

namespace Aow.Game.Ui.Dialogs;

public partial class MWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MWindow";
        LayoutOnCreate = _ => Layout_MWindowCreate_0();
        MapWindow = new AOWWindow(this);
        session.BeginRead(MapWindow);
        MapWindow.Name = "MapWindow";
        MapWindow.OnMouseDown = (_, button, shift, x, y) => MapWindowMouseDown(button, shift, x, y);
        MapWindow.OnMouseUp = (_, button, shift, x, y) => MapWindowMouseUp(button, shift, x, y);
        MapWindow.OnMouseMove = (_, shift, x, y) => MapWindowMouseMove(shift, x, y);
        MapWindow.OnKeyDown = MapWindowKeyDown;
        MapWindow.OnUpdate = _ => Layout_MapWindowUpdate_5();
        MapWindow.Visible = false;
        MapWindow.Priority = 0;
        MapWindow.WinLeft = 0;
        MapWindow.WinTop = 0;
        MapWindow.WinWidth = 440;
        MapWindow.OnExit = _ => MapWindowExit();
        MapWindow.Masked = false;
        MapWindow.Alignment.AlignWidth = AOWAlignWidth.Left;
        MapWindow.Alignment.AlignHeight = AOWAlignHeight.Top;
        MapWindow.Alignment.HeightPercent = 0;
        MapWindow.Alignment.WidthPercent = 0;
        MapWindow.Alignment.LeftOffPercent = 0;
        MapWindow.Alignment.RightOffPercent = 0;
        MapWindow.Alignment.TopOffPercent = 0;
        MapWindow.Alignment.BottomOffPercent = 0;
        MapWindow.Alignment.LeftOffset = 0;
        MapWindow.Alignment.RightOffset = 0;
        MapWindow.Alignment.TopOffset = 0;
        MapWindow.Alignment.BottomOffset = 0;
        MapWindow.UseVMem = true;
        MapWindow.BorderWidth = 11;
        MapWindow.BorderHeight = 11;
        MapWindow.OverlapTopCorners = false;
        MapWindow.Cursor = 0;
        MapWindow.DoubleBuffered = false;
        MapWindow.MaskColor = 0;
        MapWindow.OnPreDrawSurface = (_, Surface, Left, Top) => MapWindowPreDrawSurface(Surface, Left, Top);
        MapWindow.FreeSurfaceOnHide = false;
        MapWindow.ILIndexFrame = 8;
        MapWindow.ILIndexTexture = 0;
        MapWindow.DragHeight = 0;
        MapWindow.Draggable = false;
        MapWindow.Resizable = true;
        MapWindow.OnResize = _ => Layout_MapWindowResize_6();
        MapWindow.MinWidth = 200;
        MapWindow.MinHeight = 200;
        MapWindow.MaxWidth = 0;
        MapWindow.MaxHeight = 0;
        MapWindow.UseManagerBorders = true;
        MapWindow.Modal = false;
        MapWindow.WinHeight = 432;
        MapWindow.Dockable = false;
        session.EndRead(MapWindow);
        MapViewer = new HSMapViewer(this);
        session.BeginRead(MapViewer);
        MapViewer.Name = "MapViewer";
        MapViewer.OnSceneChanged = _ => Layout_MapViewerSceneChanged_7();
        session.EndRead(MapViewer);
        session.EndRead(this);
        Loaded();
        MapWindow.Loaded();
        MapViewer.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_MWindowCreate_0()
    {
        MWindowCreate();
    }

    private void Layout_MapWindowUpdate_5()
    {
        MapWindowUpdate();
    }

    private void Layout_MapWindowResize_6()
    {
        MapWindowResize();
    }

    private void Layout_MapViewerSceneChanged_7()
    {
        MapViewerSceneChanged();
    }
}
