// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public partial class BookWindow : Window
{
    [Published]
    public event NotifyEvent<BookWindow> OnAnimationDone;
    private bool _firstAnimationFrame;
    private bool _previousSurfaceValid;

    public PageTurnControl PageTurn { get; }
    public bool Animating { get; set; }

    private Surface _pageSurface;
    private PageTurnMode _pageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        Surface S;
        S = _pageSurface;
        if (S != null)
        {
            S?.Dispose();
        }

        base.Dispose();
    }

    // AoWBookWindow.pas:44
    public BookWindow(Component owner) : base(owner)
    {
        PageTurn = new PageTurnControl();
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
                if (OnAnimationDone != null)
                {
                    OnAnimationDone(this);
                }
            }
        }

        base.Update();
    }

    // AoWBookWindow.pas:59
    public override void SetVisible(bool value)
    {
        Surface S;
        if (value != Visible)
        {
            Animating = false;
            if (_freeSurfaceOnHide)
            {
                S = _pageSurface;
                if (S != null)
                {
                    S?.Dispose();
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
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
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
                if (OnAnimationDone != null)
                {
                    OnAnimationDone(this);
                }
            }
        }
    }

    // AoWBookWindow.pas:199
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = Animating;
        if (!Animating)
        {
            base.CheckMouseUp(button, shift, X, Y);
        }
    }

    // AoWBookWindow.pas:205
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
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
        LockedSurface source = default;
        LockedSurface destination = default;
        Rect R;
        if (BackSurface != null && _pageSurface != null)
        {
            R = new Rect(0, 0, WinWidth, WinHeight);
            BackSurface.Lock(ref source);
            _pageSurface.Lock(ref destination);
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(source, R, destination, 0, 0);
            _pageSurface.Unlock();
            BackSurface.Unlock();
            PageTurn.SetPageDimensions(271, 368);
            PageTurn.Speed = 15;
            PageTurn.Start(mode);
            _pageTurnMode = mode;
            Drawn = false;
            Animating = true;
            _firstAnimationFrame = true;
        }
    }

    public new event NotifyEvent<BookWindow> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<BookWindow> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<BookWindow> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<BookWindow> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<BookWindow> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<BookWindow> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<BookWindow> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<BookWindow> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<BookWindow> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<BookWindow> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<BookWindow> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<BookWindow> OnDrag
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(OnDrag), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(OnDrag), value: value);
    }

    public new event NotifyEvent<BookWindow> OnResize
    {
        add => ((WindowControl)this).OnResize += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnResize), value: value);
        remove => ((WindowControl)this).OnResize -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnResize), value: value);
    }

    public new event NotifyEvent<BookWindow> OnInternalResize
    {
        add => ((WindowControl)this).OnInternalResize += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(OnInternalResize), value: value);
        remove => ((WindowControl)this).OnInternalResize -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(OnInternalResize), value: value);
    }

    public new event NotifyEvent<BookWindow> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<BookWindow> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(OnShow), value: value);
    }
}
