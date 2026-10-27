// Generated once from recovered text forms. This layout is editable product source.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWIntroScene
{
    public partial class TIntroScene
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "IntroScene";
            ComponentState = ComponentState.With(TComponentState.csLoading).With(TComponentState.csReading);
            Window = new Aow.Units.AowUserWindow.TAoWUserWindow(this)
            {
                Name = "Window",
                OnMouseDown = WindowMouseDown,
                OnKeyDown = WindowKeyDown,
                OnUpdate = Layout_WindowUpdate_0,
                Visible = false,
                Priority = 0,
                WinLeft = 0,
                WinTop = 0,
                WinWidth = 640,
                Masked = false,
                OnHide = Layout_WindowHide_1,
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
                Cursor = -1,
                DoubleBuffered = false,
                MaskColor = 0,
                FreeSurfaceOnHide = false,
                ILIndexFrame = 0,
                ILIndexTexture = 0,
                DragHeight = 0,
                Draggable = false,
                Resizable = false,
                MinWidth = 0,
                MinHeight = 0,
                MaxWidth = 0,
                MaxHeight = 0,
                UseManagerBorders = false,
                Modal = false,
                WinHeight = 480,
                Dockable = false,
                OnDrawSurfaceEx = WindowDrawSurfaceEx
            };
            LayoutSession.EndRead(Window);
            VideoPlayer = new Aow.Layout.LayoutTVideoPlayer(this)
            {
                Name = "VideoPlayer",
                OnStreamSound = VideoPlayerStreamSound,
                Loopmode = Aow.Layout.TVideoPlayerLoopMode.vplmNone,
                Options = Aow.Rtl.TSet<Aow.Layout.TVideoPlayerOptions_Element>.Of(Aow.Layout.TVideoPlayerOptions_Element.vpoSoundEnabled, Aow.Layout.TVideoPlayerOptions_Element.vpoVideoEnabled)
            };
            LayoutSession.EndRead(VideoPlayer);
            ComponentState = ComponentState.Without(TComponentState.csReading);
            Loaded();
            Window.Loaded();
            VideoPlayer.Loaded();
            session.Resolve(this);
            Window.WinManager = Aow.Units.AoWWinMan.AoWWinMan.General.Manager;
        }

        private void Layout_WindowUpdate_0(TObject p0)
        {
            WindowUpdate();
        }

        private void Layout_WindowHide_1(TObject p0)
        {
            WindowHide();
        }
    }
}
