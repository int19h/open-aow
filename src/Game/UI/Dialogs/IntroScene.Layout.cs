// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Rtl;

namespace Aow.Game.UI.Dialogs;

public partial class IntroScene : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "IntroScene";
        Window = new UserWindow(this);
        session.BeginRead(Window);
        Window.Name = "Window";
        Window.OnMouseDown = (_, button, shift, x, y) => WindowMouseDown(button, shift, x, y);
        Window.OnKeyDown = (_, ref key, shift) => WindowKeyDown(ref key, shift);
        Window.OnUpdate = _ => Layout_WindowUpdate_0();
        Window.Visible = false;
        Window.Priority = 0;
        Window.WinLeft = 0;
        Window.WinTop = 0;
        Window.WinWidth = 640;
        Window.Masked = false;
        Window.OnHide = _ => Layout_WindowHide_1();
        Window.Alignment.AlignWidth = AlignWidth.Both;
        Window.Alignment.AlignHeight = AlignHeight.Both;
        Window.Alignment.HeightPercent = 0;
        Window.Alignment.WidthPercent = 0;
        Window.Alignment.LeftOffsetPercent = 0;
        Window.Alignment.RightOffsetPercent = 0;
        Window.Alignment.TopOffsetPercent = 0;
        Window.Alignment.BottomOffsetPercent = 0;
        Window.Alignment.LeftOffset = 0;
        Window.Alignment.RightOffset = 0;
        Window.Alignment.TopOffset = 0;
        Window.Alignment.BottomOffset = 0;
        Window.UseVideoMemory = false;
        Window.BorderWidth = 0;
        Window.BorderHeight = 0;
        Window.OverlapTopCorners = false;
        Window.Cursor = -1;
        Window.DoubleBuffered = false;
        Window.MaskColor = 0;
        Window.FreeSurfaceOnHide = false;
        Window.ImageLibraryIndexFrame = 0;
        Window.ImageLibraryIndexTexture = 0;
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
        Window.OnCustomDrawSurface = WindowDrawSurfaceEx;
        session.EndRead(Window);
        LayoutVideoPlayer videoPlayer = new LayoutVideoPlayer(this);
        session.BeginRead(videoPlayer);
        videoPlayer.Name = "VideoPlayer";
        videoPlayer.OnStreamSound = VideoPlayerStreamSound;
        videoPlayer.LoopMode = LayoutVideoPlayer.VideoPlayerLoopMode.None;
        videoPlayer.Options = Set<VideoPlayerOptionsElement>.Of(VideoPlayerOptionsElement.SoundEnabled, VideoPlayerOptionsElement.VideoEnabled);
        session.EndRead(videoPlayer);
        session.EndRead(this);
        Loaded();
        Window.Loaded();
        videoPlayer.Loaded();
        session.Resolve(this);
        Window.WindowManager = DialogHosts.General.Manager;
    }

    private void Layout_WindowUpdate_0()
    {
        WindowUpdate();
    }

    private void Layout_WindowHide_1()
    {
        WindowHide();
    }
}
