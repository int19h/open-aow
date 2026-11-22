#nullable disable warnings
// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Grids;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public sealed partial class BookWindow(Component? owner) : Window(owner)
{

    [Published]
    public event EventHandler? AnimationEnded;
    private bool _firstAnimationFrame;
    private bool _previousSurfaceValid;

    public PageTurnControl PageTurn { get; } = new PageTurnControl();
    public bool Animating { get; set; }

    private Surface? _pageSurface;
    private PageTurnMode _pageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        Surface? s = _pageSurface;
        s?.Dispose();

        base.Dispose();
    }

    // AoWBookWindow.pas:78
    public override void Update()
    {
        if (Visible && Animating)
        {
            SurfaceValid = false;
            if (!PageTurn.UpdateScene())
            {
                Animating = false;
                AnimationEnded?.Invoke(this, e: EventArgs.Empty);
            }
        }

        base.Update();
    }

    // AoWBookWindow.pas:59
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            Animating = false;
            if (FreeSurfaceOnHide)
            {
                Surface? s = _pageSurface;
                if (s is not null)
                {
                    s?.Dispose();
                    _pageSurface = null;
                }
            }
        }

        base.SetVisible(value);
    }

    // AoWBookWindow.pas:113
    public override void DrawSurface(Surface? surface)
    {
        _previousSurfaceValid = SurfaceValid;
        base.DrawSurface(surface);
        if (_previousSurfaceValid && Animating)
        {
            Blit(surface);
        }
    }

    // AoWBookWindow.pas:186
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = Animating;
        if (!Animating)
        {
            base.CheckMouseDown(button, shift, X, Y);
        }
        else
        {
            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                SurfaceValid = false;
                Animating = false;
                AnimationEnded?.Invoke(this, e: EventArgs.Empty);
            }
        }
    }

    // AoWBookWindow.pas:199
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = Animating;
        if (!Animating)
        {
            base.CheckMouseUp(button, shift, X, Y);
        }
    }

    // AoWBookWindow.pas:205
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        Handled = Animating;
        if (!Animating)
        {
            base.CheckMouseMove(shift, X, Y);
        }
    }

    // AoWBookWindow.pas:92
    public void StartAnimation(PageTurnMode mode)
    {
        if (BackSurface is not null && _pageSurface is not null)
        {
            Rect r = new(0, 0, WinWidth, WinHeight);
            LockedSurface source = default;
            _ = BackSurface.Lock(ref source);
            LockedSurface destination = default;
            _ = _pageSurface.Lock(ref destination);
            _ = LockedSurface.BlitSurfaceDescriptionRectangleClipped(source, sourceRectangle: r, destination, 0, 0);
            _ = _pageSurface.Unlock();
            _ = BackSurface.Unlock();
            PageTurn.SetPageDimensions(271, 368);
            PageTurn.Speed = 15;
            PageTurn.Start(mode);
            _pageTurnMode = mode;
            IsDrawn = false;
            Animating = true;
            _firstAnimationFrame = true;
        }
    }
}
