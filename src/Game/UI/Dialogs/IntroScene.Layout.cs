// Layout translated from recovered Delphi forms.
#nullable disable warnings
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;
using Aow.Graphics;

namespace Aow.Game.UI.Dialogs;

public sealed partial class IntroScene : ILayoutModule
{
    public void InitializeComponent() => InitializeComponent(LayoutSession.Default);

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "IntroScene";
        Window = new UserWindow(this);
        session.BeginRead(Window);
        Window.Name = "Window";
        Window.MousePressed += (_, e) => WindowMouseDown(e.Button, e.Shift, e.X, e.Y);
        Window.KeyPressed += (_, e) => WindowKeyDown(e.Key, e.Shift);
        Window.Updated += (_, _) => WindowUpdate();
        Window.Visible = false;
        Window.Priority = 0;
        Window.WinLeft = 0;
        Window.WinTop = 0;
        Window.WinWidth = 640;
        Window.Masked = false;
        Window.Hidden += (_, _) => WindowHide();
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
        Window.MaskColor = Color.Black;
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
        Window.SurfaceDrawing += (_, e) =>
        {
            bool handled = e.Handled;
            try
            {
                WindowDrawSurfaceEx(e.Surface, out handled);
            }
            finally
            {
                e.Handled = handled || e.Handled;
            }
        };
        session.EndRead(Window);
        LayoutVideoPlayer videoPlayer = new(this);
        session.BeginRead(videoPlayer);
        videoPlayer.Name = "VideoPlayer";
        videoPlayer.LoopMode = LayoutVideoPlayer.VideoPlayerLoopMode.None;
        videoPlayer.Options = VideoPlayerOptions.SoundEnabled | VideoPlayerOptions.VideoEnabled;
        session.EndRead(videoPlayer);
        session.EndRead(this);
        Loaded();
        Window.Loaded();
        videoPlayer.Loaded();
        session.Resolve(this);
        Window.WindowManager = UI.General.Manager;
    }
}
