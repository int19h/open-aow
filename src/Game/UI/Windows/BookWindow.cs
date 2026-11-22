#nullable disable warnings
// Translated from AoWBookWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Grids;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// completion event, first frame, previous surface validity, page turn, animating, saved page
// surface, page-turn mode
public sealed partial class BookWindow(Component? owner) : Window(owner)
{

    [Published]
    public event NotifyEvent<BookWindow>? AnimationEnded;
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
                AnimationEnded?.Invoke(this);
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
                AnimationEnded?.Invoke(this);
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

    public new event NotifyEvent<BookWindow> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<BookWindow> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<BookWindow> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<BookWindow> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<BookWindow> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<BookWindow> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<BookWindow> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<BookWindow> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<BookWindow> BackgroundDrawn
    {
        add => ((WindowControl)this).BackgroundDrawn += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(BackgroundDrawn), value: value);
        remove => ((WindowControl)this).BackgroundDrawn -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(BackgroundDrawn), value: value);
    }

    public new event NotifyEvent<BookWindow> MouseEntered
    {
        add => ((WindowControl)this).MouseEntered += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(MouseEntered), value: value);
        remove => ((WindowControl)this).MouseEntered -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(MouseEntered), value: value);
    }

    public new event NotifyEvent<BookWindow> MouseLeft
    {
        add => ((WindowControl)this).MouseLeft += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(MouseLeft), value: value);
        remove => ((WindowControl)this).MouseLeft -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(MouseLeft), value: value);
    }

    public new event NotifyEvent<BookWindow> CursorChanged
    {
        add => ((WindowControl)this).CursorChanged += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(CursorChanged), value: value);
        remove => ((WindowControl)this).CursorChanged -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(CursorChanged), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Blitted), value: value);
    }

    public new event AoWOnDrawSurface<BookWindow> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<BookWindow> Dragged
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, BookWindow>(this, slot: nameof(Dragged), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, BookWindow>(this, slot: nameof(Dragged), value: value);
    }

    public new event NotifyEvent<BookWindow> Resized
    {
        add => ((WindowControl)this).Resized += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(Resized), value: value);
        remove => ((WindowControl)this).Resized -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(Resized), value: value);
    }

    public new event NotifyEvent<BookWindow> SizeChanged
    {
        add => ((WindowControl)this).SizeChanged += EventSlot.Add<WindowControl, BookWindow>(this, slot: nameof(SizeChanged), value: value);
        remove => ((WindowControl)this).SizeChanged -= EventSlot.Remove<WindowControl, BookWindow>(this, slot: nameof(SizeChanged), value: value);
    }

    public new event NotifyEvent<BookWindow> Hidden
    {
        add => ((BaseWindow)this).Hidden += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(Hidden), value: value);
        remove => ((BaseWindow)this).Hidden -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(Hidden), value: value);
    }

    public new event NotifyEvent<BookWindow> Shown
    {
        add => ((BaseWindow)this).Shown += EventSlot.Add<BaseWindow, BookWindow>(this, slot: nameof(Shown), value: value);
        remove => ((BaseWindow)this).Shown -= EventSlot.Remove<BaseWindow, BookWindow>(this, slot: nameof(Shown), value: value);
    }
}
