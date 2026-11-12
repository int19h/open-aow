// Layout translated from recovered Delphi forms.
#nullable disable
using Aow.Game.Layout;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.UI.Dialogs;

public partial class PyreScreen : ILayoutModule
{
    public void InitializeComponent()
    {
        InitializeComponent(LayoutSession.Default);
    }

    public void InitializeComponent(LayoutSession session)
    {
        session.Begin(this);
        Name = "PyreScreen";
        DeadPanel = new Panel(this);
        session.BeginRead(DeadPanel);
        DeadPanel.Name = "DeadPnl";
        DeadPanel.OnMouseDown += (_, button, shift, x, y) => DeadPnlMouseDown(button, shift, x, y);
        DeadPanel.OnKeyDown += (_, ref key, shift) => DeadPnlKeyDown(ref key, shift);
        DeadPanel.OnUpdate += _ => DeadPnlUpdate();
        DeadPanel.Visible = false;
        DeadPanel.Priority = 5;
        DeadPanel.WinLeft = 0;
        DeadPanel.WinTop = 0;
        DeadPanel.WinWidth = 640;
        DeadPanel.Masked = true;
        DeadPanel.Alignment.AlignWidth = AlignWidth.Center;
        DeadPanel.Alignment.AlignHeight = AlignHeight.Center;
        DeadPanel.Alignment.HeightPercent = 0;
        DeadPanel.Alignment.WidthPercent = 0;
        DeadPanel.Alignment.LeftOffsetPercent = 0;
        DeadPanel.Alignment.RightOffsetPercent = 0;
        DeadPanel.Alignment.TopOffsetPercent = 43;
        DeadPanel.Alignment.BottomOffsetPercent = 0;
        DeadPanel.Alignment.LeftOffset = 0;
        DeadPanel.Alignment.RightOffset = 0;
        DeadPanel.Alignment.TopOffset = 0;
        DeadPanel.Alignment.BottomOffset = 0;
        DeadPanel.WinHeight = 480;
        DeadPanel.ImageLibraryIndexFrame = 2;
        DeadPanel.ImageLibraryIndexTexture = 0;
        DeadPanel.OwnerItem = 0;
        DeadPanel.MinWidth = 640;
        DeadPanel.MinHeight = 480;
        session.EndRead(DeadPanel);
        Image pyreTop = new Image(this);
        session.BeginRead(pyreTop);
        pyreTop.Name = "PyreTop";
        pyreTop.Priority = 0;
        pyreTop.WinLeft = 0;
        pyreTop.WinTop = 0;
        pyreTop.CaptureInput = false;
        pyreTop.Alignment.AlignWidth = AlignWidth.Left;
        pyreTop.Alignment.AlignHeight = AlignHeight.Top;
        pyreTop.Alignment.HeightPercent = 0;
        pyreTop.Alignment.WidthPercent = 0;
        pyreTop.Alignment.LeftOffsetPercent = 0;
        pyreTop.Alignment.RightOffsetPercent = 0;
        pyreTop.Alignment.TopOffsetPercent = 0;
        pyreTop.Alignment.BottomOffsetPercent = 0;
        pyreTop.Alignment.LeftOffset = 0;
        pyreTop.Alignment.RightOffset = 0;
        pyreTop.Alignment.TopOffset = 0;
        pyreTop.Alignment.BottomOffset = 0;
        pyreTop.Masked = false;
        pyreTop.AnimationStart = 0;
        pyreTop.CurrentImage = 9;
        pyreTop.AnimationEnd = 0;
        pyreTop.WinWidth = 640;
        pyreTop.WinHeight = 43;
        pyreTop.Animate = false;
        pyreTop.AnimationFrameDelay = 0;
        session.EndRead(pyreTop);
        LeaderFaceFrame = new Image(this);
        session.BeginRead(LeaderFaceFrame);
        LeaderFaceFrame.Name = "LeaderFaceFr";
        LeaderFaceFrame.OnPreDraw += (_, surface) => LeaderFaceFrPreDraw(surface);
        LeaderFaceFrame.Priority = 1000;
        LeaderFaceFrame.WinLeft = 225;
        LeaderFaceFrame.WinTop = 5;
        LeaderFaceFrame.CaptureInput = true;
        LeaderFaceFrame.Alignment.AlignWidth = AlignWidth.Center;
        LeaderFaceFrame.Alignment.AlignHeight = AlignHeight.Top;
        LeaderFaceFrame.Alignment.HeightPercent = 0;
        LeaderFaceFrame.Alignment.WidthPercent = 0;
        LeaderFaceFrame.Alignment.LeftOffsetPercent = 0;
        LeaderFaceFrame.Alignment.RightOffsetPercent = 0;
        LeaderFaceFrame.Alignment.TopOffsetPercent = 0;
        LeaderFaceFrame.Alignment.BottomOffsetPercent = 0;
        LeaderFaceFrame.Alignment.LeftOffset = 0;
        LeaderFaceFrame.Alignment.RightOffset = 0;
        LeaderFaceFrame.Alignment.TopOffset = 5;
        LeaderFaceFrame.Alignment.BottomOffset = 0;
        LeaderFaceFrame.Masked = true;
        LeaderFaceFrame.AnimationStart = 0;
        LeaderFaceFrame.CurrentImage = 10;
        LeaderFaceFrame.AnimationEnd = 2;
        LeaderFaceFrame.WinWidth = 191;
        LeaderFaceFrame.WinHeight = 198;
        LeaderFaceFrame.Animate = false;
        LeaderFaceFrame.AnimationFrameDelay = 0;
        session.EndRead(LeaderFaceFrame);
        PyreAnimationView = new UserDrawBox(this);
        session.BeginRead(PyreAnimationView);
        PyreAnimationView.Name = "PyreAnimation";
        PyreAnimationView.OnDraw += (_, surface) => PyreAnimationDraw(surface);
        PyreAnimationView.OnUpdate += _ => PyreAnimationUpdate();
        PyreAnimationView.Priority = 10;
        PyreAnimationView.WinLeft = 0;
        PyreAnimationView.WinTop = 0;
        PyreAnimationView.CaptureInput = true;
        PyreAnimationView.Alignment.AlignWidth = AlignWidth.Left;
        PyreAnimationView.Alignment.AlignHeight = AlignHeight.Top;
        PyreAnimationView.Alignment.HeightPercent = 0;
        PyreAnimationView.Alignment.WidthPercent = 0;
        PyreAnimationView.Alignment.LeftOffsetPercent = 0;
        PyreAnimationView.Alignment.RightOffsetPercent = 0;
        PyreAnimationView.Alignment.TopOffsetPercent = 0;
        PyreAnimationView.Alignment.BottomOffsetPercent = 0;
        PyreAnimationView.Alignment.LeftOffset = 0;
        PyreAnimationView.Alignment.RightOffset = 0;
        PyreAnimationView.Alignment.TopOffset = 43;
        PyreAnimationView.Alignment.BottomOffset = 0;
        PyreAnimationView.Masked = false;
        PyreAnimationView.WinWidth = 640;
        PyreAnimationView.WinHeight = 437;
        session.EndRead(PyreAnimationView);
        session.EndRead(this);
        pyreTop.Window = DeadPanel;
        LeaderFaceFrame.Window = DeadPanel;
        PyreAnimationView.Window = DeadPanel;
        Loaded();
        DeadPanel.Loaded();
        pyreTop.Loaded();
        LeaderFaceFrame.Loaded();
        PyreAnimationView.Loaded();
        session.Resolve(this);
        DeadPanel.Window = UI.TitleWindow.MainWindow;
        pyreTop.ImageLibrary = UI.Graphics.PyreFrames;
        LeaderFaceFrame.ImageLibrary = UI.Graphics.PyreFrames;
    }
}
