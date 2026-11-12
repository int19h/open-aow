
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// { The AoWWinGeneric unit of aowInt.dpl: TAOWGenericWindow, the base window class that publishes
//   the window manager's settings (the ancestor of TAOWWindow, TAOWToolTip and TAOWMenu). }
// no fields of its own
public partial class GenericWindow : BaseWindow
{
    public GenericWindow(Component owner) : base(owner)
    { }

    // AoWWinGeneric.pas:39
    public override void Dispose()
    {
        (Buffer as IDisposable)?.Dispose();
        base.Dispose();
    }

    // AoWWinGeneric.pas:45
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _windowManager = (WindowManager)_ownerComponent;
    }

    // AoWWinGeneric.pas:51
    public void SetCursor(int value)
    {
        if (value != _cursor)
        {
            _cursor = value;
            WindowCursor = value;
            if (_onChangeCursor != null)
            {
                _onChangeCursor(this);
            }
        }
    }

    // AoWWinGeneric.pas:62
    public void SetBuffer(bool value)
    {
        if (value != _doubleBuffered)
        {
            Drawn = false;
            _doubleBuffered = value;
            if (!_doubleBuffered)
            {
                if (BackSurface != null)
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }
        }
    }

    [Published]
    public WindowManager WindowManager { get => _windowManager; set => SetManager(value); }

    [Published]
    public bool UseVideoMemory { get => _useVideoMemory; set => _useVideoMemory = value; }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public bool OverlapTopCorners { get => _overlapTopCorners; set => SetOverlap(value); }

    [Published]
    public int Cursor { get => _cursor; set => SetCursor(value); }

    [Published]
    public bool DoubleBuffered { get => _doubleBuffered; set => SetBuffer(value); }

    [Published]
    public NotifyEvent<GenericWindow> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    [Published]
    public int MaskColor { get => _maskColor; set => _maskColor = value; }

    [Published]
    public AoWOnDrawSurface<GenericWindow> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    [Published]
    public AoWOnDrawSurface<GenericWindow> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    [Published]
    public bool FreeSurfaceOnHide { get => _freeSurfaceOnHide; set => _freeSurfaceOnHide = value; }

    public new NotifyEvent<GenericWindow> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<GenericWindow> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<GenericWindow> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<GenericWindow> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<GenericWindow> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<GenericWindow> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<GenericWindow> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<GenericWindow> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<GenericWindow> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<GenericWindow> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<GenericWindow> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<GenericWindow> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<GenericWindow> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<GenericWindow> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
