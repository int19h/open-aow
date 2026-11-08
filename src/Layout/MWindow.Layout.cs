// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Layout;
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AoWWindow;
using Aow.Units.HSEngine;

namespace Aow.Units.AOWMWindow;

public partial class TMWindow : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "MWindow";
        LayoutOnCreate = Layout_MWindowCreate_0;
        MapWindow = new TAOWWindow(this);
        session.BeginRead(MapWindow);
        MapWindow.Name = "MapWindow";
        MapWindow.OnMouseDown = Layout_MapWindowMouseDown_1;
        MapWindow.OnMouseUp = Layout_MapWindowMouseUp_2;
        MapWindow.OnMouseMove = Layout_MapWindowMouseMove_3;
        MapWindow.OnKeyDown = Layout_MapWindowKeyDown_4;
        MapWindow.OnUpdate = Layout_MapWindowUpdate_5;
        MapWindow.Visible = false;
        MapWindow.Priority = 0;
        MapWindow.WinLeft = 0;
        MapWindow.WinTop = 0;
        MapWindow.WinWidth = 440;
        MapWindow.OnExit = MapWindowExit;
        MapWindow.Masked = false;
        MapWindow.Alignment.AlignWidth = TAOWAlignWidth.awLeft;
        MapWindow.Alignment.AlignHeight = TAOWAlignHeight.ahTop;
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
        MapWindow.OnPreDrawSurface = MapWindowPreDrawSurface;
        MapWindow.FreeSurfaceOnHide = false;
        MapWindow.ILIndexFrame = 8;
        MapWindow.ILIndexTexture = 0;
        MapWindow.DragHeight = 0;
        MapWindow.Draggable = false;
        MapWindow.Resizable = true;
        MapWindow.OnResize = Layout_MapWindowResize_6;
        MapWindow.MinWidth = 200;
        MapWindow.MinHeight = 200;
        MapWindow.MaxWidth = 0;
        MapWindow.MaxHeight = 0;
        MapWindow.UseManagerBorders = true;
        MapWindow.Modal = false;
        MapWindow.WinHeight = 432;
        MapWindow.Dockable = false;
        session.EndRead(MapWindow);
        MapViewer = new THSMapViewer(this);
        session.BeginRead(MapViewer);
        MapViewer.Name = "MapViewer";
        MapViewer.OnSceneChanged = Layout_MapViewerSceneChanged_7;
        session.EndRead(MapViewer);
        session.EndRead(this);
        Loaded();
        MapWindow.Loaded();
        MapViewer.Loaded();
        session.Resolve(this);
        if (session.FireCreateEvents)
            LayoutOnCreate?.Invoke(this);
    }

    private void Layout_MWindowCreate_0(TObject p0)
    {
        MWindowCreate();
    }

    private void Layout_MapWindowMouseDown_1(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
    {
        MapWindowMouseDown(p0, p1, p2, p3, p4);
    }

    private void Layout_MapWindowMouseUp_2(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
    {
        MapWindowMouseUp(p0, p1, p2, p3, p4);
    }

    private void Layout_MapWindowMouseMove_3(TObject p0, TSet<TShiftState> p1, int p2, int p3)
    {
        MapWindowMouseMove(p0, p1, p2, p3);
    }

    private void Layout_MapWindowKeyDown_4(TObject p0, ref ushort p1, TSet<TShiftState> p2)
    {
        MapWindowKeyDown((TAoWComponent)p0, ref p1, p2);
    }

    private void Layout_MapWindowUpdate_5(TObject p0)
    {
        MapWindowUpdate();
    }

    private void Layout_MapWindowResize_6(TObject p0)
    {
        MapWindowResize();
    }

    private void Layout_MapViewerSceneChanged_7(TObject p0)
    {
        MapViewerSceneChanged();
    }
}
