// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.Ui.Controls;

using Aow.Game.Ui.Windows;
using Aow.Rtl;

namespace Aow.Game.Ui.Dialogs;

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
        Window = new AoWUserWindow(this);
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
        Window.Alignment.AlignWidth = AOWAlignWidth.Both;
        Window.Alignment.AlignHeight = AOWAlignHeight.Both;
        Window.Alignment.HeightPercent = 0;
        Window.Alignment.WidthPercent = 0;
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
        LayoutVideoPlayer created__VideoPlayer = new LayoutVideoPlayer(this);
        VideoPlayer = created__VideoPlayer;
        session.BeginRead(created__VideoPlayer);
        created__VideoPlayer.Name = "VideoPlayer";
        created__VideoPlayer.OnStreamSound = VideoPlayerStreamSound;
        created__VideoPlayer.Loopmode = LayoutVideoPlayer.VideoPlayerLoopMode.None;
        created__VideoPlayer.Options = Set<VideoPlayerOptions_Element>.Of(VideoPlayerOptions_Element.SoundEnabled, VideoPlayerOptions_Element.VideoEnabled);
        session.EndRead(created__VideoPlayer);
        session.EndRead(this);
        Loaded();
        Window.Loaded();
        VideoPlayer.Loaded();
        session.Resolve(this);
        Window.WinManager = DialogHosts.General.Manager;
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
