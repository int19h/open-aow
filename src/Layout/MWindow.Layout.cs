// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AOWMWindow
{
    public partial class TMWindow
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "MWindow";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            LayoutOnCreate = Layout_MWindowCreate_0;
            MapWindow = new Aow.Units.AoWWindow.TAOWWindow(this)
            {
                Name = "MapWindow",
                OnMouseDown = Layout_MapWindowMouseDown_1,
                OnMouseUp = Layout_MapWindowMouseUp_2,
                OnMouseMove = Layout_MapWindowMouseMove_3,
                OnKeyDown = Layout_MapWindowKeyDown_4,
                OnUpdate = Layout_MapWindowUpdate_5,
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 440,
                OnExit = MapWindowExit,
                Masked = false,
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
                    LeftOffset = 0,
                    RightOffset = 0,
                    TopOffset = 0,
                    BottomOffset = 0
                },
                UseVMem = true,
                BorderWidth = 11,
                BorderHeight = 11,
                OverlapTopCorners = false,
                Cursor = 0,
                DoubleBuffered = false,
                MaskColor = 0,
                OnPreDrawSurface = MapWindowPreDrawSurface,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 8,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = true,
                OnResize = Layout_MapWindowResize_6,
                MinWidth = 200,
                MinHeight = 200,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = true,
                Modal = false,
                WinHeight = 432,
                Dockable = false
            };
            LayoutSession.EndRead(MapWindow);
            MapViewer = new Aow.Units.HSEngine.THSMapViewer(this)
            {
                Name = "MapViewer",
                OnSceneChanged = Layout_MapViewerSceneChanged_7
            };
            LayoutSession.EndRead(MapViewer);
            ComponentState = ComponentState.Without(TComponentState.csReading);
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
            MapWindowMouseDown((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_MapWindowMouseUp_2(TObject p0, TMouseButton p1, TSet<TShiftState> p2, int p3, int p4)
        {
            MapWindowMouseUp((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3, p4);
        }

        private void Layout_MapWindowMouseMove_3(TObject p0, TSet<TShiftState> p1, int p2, int p3)
        {
            MapWindowMouseMove((Aow.Units.AoWComp.TAoWComponent)p0, p1, p2, p3);
        }

        private void Layout_MapWindowKeyDown_4(TObject p0, ref ushort p1, TSet<TShiftState> p2)
        {
            MapWindowKeyDown((Aow.Units.AoWComp.TAoWComponent)p0, ref p1, p2);
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
}
