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
    public NotifyEvent<BookWindow> OnAnimationComplete;
    public bool FirstAnimationFrame;
    public bool PreviousSurfaceValid;
    public PageTurnControl PageTurn;
    public bool Animating;
    public Surface PageSurface;
    public PageTurnMode PageTurnMode;

    // AoWBookWindow.pas:50
    public override void Dispose()
    {
        Surface S;
        S = PageSurface;
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
        if (_visible && Animating)
        {
            SurfaceValid = false;
            if (!PageTurn.UpdateScene())
            {
                Animating = false;
                if (OnAnimationComplete != null)
                {
                    OnAnimationComplete(this);
                }
            }
        }

        base.Update();
    }

    // AoWBookWindow.pas:59
    public override void SetVisible(bool value)
    {
        Surface S;
        if (value != _visible)
        {
            Animating = false;
            if (_freeSurfaceOnHide)
            {
                S = PageSurface;
                if (S != null)
                {
                    S?.Dispose();
                    PageSurface = null;
                }
            }
        }

        base.SetVisible(value);
    }

    // AoWBookWindow.pas:113
    public override void DrawSurface(Surface surface)
    {
        PreviousSurfaceValid = SurfaceValid;
        base.DrawSurface(surface);
        if (PreviousSurfaceValid && Animating)
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
            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                SurfaceValid = false;
                Animating = false;
                if (OnAnimationComplete != null)
                {
                    OnAnimationComplete(this);
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
        if (BackSurface != null && PageSurface != null)
        {
            R = new Rect(0, 0, _winWidth, _winHeight);
            BackSurface.Lock(ref source);
            PageSurface.Lock(ref destination);
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(source, R, destination, 0, 0);
            PageSurface.Unlock();
            BackSurface.Unlock();
            PageTurn.SetPageDimensions(271, 368);
            PageTurn.Speed = 15;
            PageTurn.Start(mode);
            PageTurnMode = mode;
            Drawn = false;
            Animating = true;
            FirstAnimationFrame = true;
        }
    }

    [Published]
    public NotifyEvent<BookWindow> OnAnimationDone { get => OnAnimationComplete; set => OnAnimationComplete = value; }

    public new NotifyEvent<BookWindow> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<BookWindow> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<BookWindow> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<BookWindow> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<BookWindow> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<BookWindow> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<BookWindow> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<BookWindow> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<BookWindow> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<BookWindow> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<BookWindow> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<BookWindow> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<BookWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<BookWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<BookWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<BookWindow> OnDrag
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new NotifyEvent<BookWindow> OnResize
    {
        get => EventSlot.Get(this, _onResize);
        set => EventSlot.Set(this, ref _onResize, value);
    }

    public new NotifyEvent<BookWindow> OnInternalResize
    {
        get => EventSlot.Get(this, _onInternalResize);
        set => EventSlot.Set(this, ref _onInternalResize, value);
    }

    public new NotifyEvent<BookWindow> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<BookWindow> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
