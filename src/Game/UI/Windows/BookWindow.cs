#nullable disable
// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public sealed partial class BookWindow(Component owner) : Window(owner)
{

    [Published]
    public event NotifyEvent<BookWindow> AnimationDone;
    private bool _firstAnimationFrame;
    private bool _previousSurfaceValid;

    public PageTurnControl PageTurn { get; } = new PageTurnControl();
    public bool Animating { get; set; }

    private Surface _pageSurface;
    private PageTurnMode _pageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        Surface s = _pageSurface;
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
                AnimationDone?.Invoke(this);
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
                Surface s = _pageSurface;
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
    public override void DrawSurface(Surface surface)
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
                AnimationDone?.Invoke(this);
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
            Drawn = false;
            Animating = true;
            _firstAnimationFrame = true;
        }
    }

    public new event NotifyEvent<BookWindow> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<BookWindow> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<BookWindow> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<BookWindow> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<BookWindow> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<BookWindow> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<BookWindow> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<BookWindow> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<BookWindow> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<BookWindow> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<BookWindow> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> SurfaceDrawn
    {
        add => ((WindowControl)this).SurfaceDrawn += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(SurfaceDrawn), value: value);
        remove => ((WindowControl)this).SurfaceDrawn -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(SurfaceDrawn), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> PreDrawSurface
    {
        add => ((WindowControl)this).PreDrawSurface += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(PreDrawSurface), value: value);
        remove => ((WindowControl)this).PreDrawSurface -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(PreDrawSurface), value: value);
    }

    public new event NotifyEvent<BookWindow> Drag
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Drag), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Drag), value: value);
    }

    public new event NotifyEvent<BookWindow> Resize
    {
        add => ((WindowControl)this).Resize += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Resize), value: value);
        remove => ((WindowControl)this).Resize -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Resize), value: value);
    }

    public new event NotifyEvent<BookWindow> InternalResize
    {
        add => ((WindowControl)this).InternalResize += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(InternalResize), value: value);
        remove => ((WindowControl)this).InternalResize -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(InternalResize), value: value);
    }

    public new event NotifyEvent<BookWindow> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<BookWindow> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(Show), value: value);
    }
}
