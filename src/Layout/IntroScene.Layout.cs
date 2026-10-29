// Layout translated from recovered Delphi forms.
#nullable disable
using System;
using Aow.Rtl;
using Aow.Layout;

namespace Aow.Units.AoWIntroScene
{
    public partial class TIntroScene : ILayoutModule
    {
        public void InitializeComponent()
        {
            InitializeComponent(LayoutSession.Default);
        }

        public void InitializeComponent(LayoutSession session)
        {
            session.Begin(this);
            Name = "IntroScene";
            Window = new Aow.Units.AowUserWindow.TAoWUserWindow(this);
            session.BeginRead(Window);
            Window.Name = "Window";
            Window.OnMouseDown = WindowMouseDown;
            Window.OnKeyDown = WindowKeyDown;
            Window.OnUpdate = Layout_WindowUpdate_0;
            Window.Visible = false;
            Window.Priority = 0;
            Window.WinLeft = 0;
            Window.WinTop = 0;
            Window.WinWidth = 640;
            Window.Masked = false;
            Window.OnHide = Layout_WindowHide_1;
            Window.Alignment.AlignWidth = Aow.Units.AoWComp.TAOWAlignWidth.awBoth;
            Window.Alignment.AlignHeight = Aow.Units.AoWComp.TAOWAlignHeight.ahBoth;
            Window.Alignment.HeightPercent = (byte)(0);
            Window.Alignment.WidthPercent = (byte)(0);
            Window.Alignment.LeftOffPercent = 0;
            Window.Alignment.RightOffPercent = 0;
            Window.Alignment.TopOffPercent = 0;
            Window.Alignment.BottomOffPercent = 0;
            Window.Alignment.LeftOffset = 0;
            Window.Alignment.RightOffset = 0;
            Window.Alignment.TopOffset = 0;
            Window.Alignment.BottomOffset = 0;
            Window.UseVMem = false;
            Window.BorderWidth = 0;
            Window.BorderHeight = 0;
            Window.OverlapTopCorners = false;
            Window.Cursor = -1;
            Window.DoubleBuffered = false;
            Window.MaskColor = 0;
            Window.FreeSurfaceOnHide = false;
            Window.ILIndexFrame = 0;
            Window.ILIndexTexture = 0;
            Window.DragHeight = 0;
            Window.Draggable = false;
            Window.Resizable = false;
            Window.MinWidth = 0;
            Window.MinHeight = 0;
            Window.MaxWidth = 0;
            Window.MaxHeight = 0;
            Window.UseManagerBorders = false;
            Window.Modal = false;
            Window.WinHeight = 480;
            Window.Dockable = false;
            Window.OnDrawSurfaceEx = WindowDrawSurfaceEx;
            session.EndRead(Window);
            var created__VideoPlayer = new Aow.Layout.LayoutTVideoPlayer(this);
            VideoPlayer = created__VideoPlayer;
            session.BeginRead(created__VideoPlayer);
            created__VideoPlayer.Name = "VideoPlayer";
            created__VideoPlayer.OnStreamSound = VideoPlayerStreamSound;
            created__VideoPlayer.Loopmode = Aow.Layout.TVideoPlayerLoopMode.vplmNone;
            created__VideoPlayer.Options = Aow.Rtl.TSet<Aow.Layout.TVideoPlayerOptions_Element>.Of(Aow.Layout.TVideoPlayerOptions_Element.vpoSoundEnabled, Aow.Layout.TVideoPlayerOptions_Element.vpoVideoEnabled);
            session.EndRead(created__VideoPlayer);
            session.EndRead(this);
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
