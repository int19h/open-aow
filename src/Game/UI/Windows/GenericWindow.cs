
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
        _windowManager = (WindowManager)OwnerComponent;
    }

    // AoWWinGeneric.pas:51
    public void SetCursor(int value)
    {
        if (value != _cursor)
        {
            _cursor = value;
            WindowCursor = value;
            RaiseChangeCursor(this);
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
    public new event NotifyEvent<GenericWindow> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnChangeCursor), value: value);
    }

    [Published]
    public int MaskColor { get => _maskColor; set => _maskColor = value; }

    [Published]
    public new event AoWOnDrawSurface<GenericWindow> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnDrawSurface), value: value);
    }

    [Published]
    public new event AoWOnDrawSurface<GenericWindow> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    [Published]
    public bool FreeSurfaceOnHide { get => _freeSurfaceOnHide; set => _freeSurfaceOnHide = value; }

    public new event NotifyEvent<GenericWindow> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<GenericWindow> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<GenericWindow> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<GenericWindow> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<GenericWindow> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<GenericWindow> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<GenericWindow> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<GenericWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<GenericWindow> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, GenericWindow>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, GenericWindow>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<GenericWindow> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<GenericWindow> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<GenericWindow> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, GenericWindow>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, GenericWindow>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<GenericWindow> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<GenericWindow> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, GenericWindow>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, GenericWindow>(this, slot: nameof(OnShow), value: value);
    }
}
