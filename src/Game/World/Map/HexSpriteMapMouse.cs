// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// the mouse of a map. FHSMap, FX, FY (the position in the view port in pixels, Low(Integer)
// when outside), _button, ButtonUp, ButtonDown, Moved, DoubleClicked, CaptureChanged, _capture
// (the object that gets the mouse events), FStatus, FCursor, FCursorMode, FIgnoreDown, FShift,
// FCursorVisible (and are used by the game only)
public partial class HexSpriteMapMouse : IDisposable
{

    public HexSpriteMapMouse()
    {
        OnUpStore = new();
        OnDownStore = new();
        OnMoveStore = new();
        OnDoubleClickStore = new();
        OnCaptureChangedStore = new();
        Reset();
    }

    private GameMap _hexSpriteMap;
    private int _x;
    private int _y;
    private MouseButton _button;

    internal EventStore<HexSpriteMapMouse> OnUpStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> ButtonUp
    {
        add => OnUpStore.Add(value);
        remove => OnUpStore.Remove(value);
    }

    internal EventStore<HexSpriteMapMouse> OnDownStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> ButtonDown
    {
        add => OnDownStore.Add(value);
        remove => OnDownStore.Remove(value);
    }

    internal EventStore<HexSpriteMapMouse> OnMoveStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> Moved
    {
        add => OnMoveStore.Add(value);
        remove => OnMoveStore.Remove(value);
    }

    internal EventStore<HexSpriteMapMouse> OnDoubleClickStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> DoubleClicked
    {
        add => OnDoubleClickStore.Add(value);
        remove => OnDoubleClickStore.Remove(value);
    }

    internal EventStore<HexSpriteMapMouse> OnCaptureChangedStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> CaptureChanged
    {
        add => OnCaptureChangedStore.Add(value);
        remove => OnCaptureChangedStore.Remove(value);
    }

    private object _capture;
    private Set<StatusElement> _status;
    private int _cursor;
    private int _cursorMode;
    private bool _ignoreDown;
    private Set<ShiftState> _shift;
    private bool _cursorVisible;

    // HSEngine.pas:3475

    // HSEngine.pas:3486
    public virtual void Dispose()
    {
        OnUpStore.Clear();
        OnDownStore.Clear();
        OnMoveStore.Clear();
        OnDoubleClickStore.Clear();
        OnCaptureChangedStore.Clear();
    }

    // HSEngine.pas:3505
    public virtual int GetCursor() => _cursor;

    // HSEngine.pas:3510
    public virtual void SetCursor(int value)
    {
        _cursor = value;
    }

    // HSEngine.pas:3583
    public virtual bool MouseDown()
    {
        bool result = true;
        if (_capture == null && GetHexX() != -1)
        {
            if (_button == MouseButton.Left)
            {
                var pixelY = GetPixelY();
                var pixelX = GetPixelX();
                _hexSpriteMap.Selection.SelectLocation(pixelX, pixelY, _hexSpriteMap._viewMapLevel);
                result = false;
            }
            else
            {
                if (_button == MouseButton.Right && !_hexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
                {
                    _hexSpriteMap.Selection.UnselectLocation();
                    result = false;
                }
            }
        }

        return result;
    }

    // HSEngine.pas:3623
    public virtual bool MouseUp() => true;

    // HSEngine.pas:3646
    public virtual bool MouseMove() => true;

    // HSEngine.pas:3667
    public virtual bool MouseDoubleClick() => true;

    // HSEngine.pas:3600
    // { Down and Up set FX twice when the position is outside (an original slip for FY). }
    public virtual void Down(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (!_ignoreDown)
        {
            if (X >= 0 && Y >= 0)
            {
                _x = X;
                _y = Y;
            }
            else
            {
                _x = -2147483648;
                _x = -2147483648;
            }

            _button = button;
            _shift = shift;
            if (MouseDown())
            {
                OnDownStore.Raise(this);
            }
        }
        else
        {
            _ignoreDown = false;
        }
    }

    // HSEngine.pas:3628
    public virtual void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (X >= 0 && Y >= 0)
        {
            _x = X;
            _y = Y;
        }
        else
        {
            _x = -2147483648;
            _x = -2147483648;
        }

        _button = button;
        _shift = shift;
        if (MouseUp())
        {
            OnUpStore.Raise(this);
        }
    }

    // HSEngine.pas:3651
    public virtual void Move(Set<ShiftState> shift, int X, int Y)
    {
        // Port extension: a view wider or taller than the map takes scene points past MaxXhp or MaxYhp.
        // GetXhp and GetYhp still test the map coordinates.
        if (X >= 0 && Y >= 0 && X <= Math.Max(_hexSpriteMap.GetMaxPixelX(), _hexSpriteMap.ViewWidth) && Y <= Math.Max(_hexSpriteMap.GetMaxPixelY(), _hexSpriteMap.ViewHeight))
        {
            _x = X;
            _y = Y;
        }
        else
        {
            _x = -2147483648;
            _y = -2147483648;
        }

        if (MouseMove())
        {
            OnMoveStore.Raise(this);
        }
    }

    // HSEngine.pas:3672
    public virtual void DoubleClick()
    {
        if (GetPixelX() != -2147483648)
        {
            if (MouseDoubleClick())
            {
                OnDoubleClickStore.Raise(this);
            }
        }
    }

    // HSEngine.pas:3496
    public void Reset()
    {
        _x = -2147483648;
        _y = -2147483648;
        _shift = Set<ShiftState>.Of();
        _cursor = 0;
        SetCapture(null);
    }

    // HSEngine.pas:3515
    public int GetPixelX()
    {
        int result;
        if (_x != -2147483648)
        {
            result = _hexSpriteMap._viewPixelX + _x;
            if (result > _hexSpriteMap.GetMaxPixelX() || result < 0)
            {
                result = -2147483648;
            }
        }
        else
        {
            result = -2147483648;
        }

        return result;
    }

    // HSEngine.pas:3527
    public int GetPixelY()
    {
        int result;
        if (_y != -2147483648)
        {
            result = _hexSpriteMap._viewPixelY + _y;
            if (result > _hexSpriteMap.GetMaxPixelY() || result < 0)
            {
                result = -2147483648;
            }
        }
        else
        {
            result = -2147483648;
        }

        return result;
    }

    // HSEngine.pas:3540
    // { Both GetXhx and GetYhx test the result against the width and the height of the map. }
    public int GetHexX()
    {
        int result = default;
        int Y = default;
        if (_x != -2147483648)
        {
            HexGeometry.PixelToHex(_hexSpriteMap._viewPixelX + _x, 0, out result, out Y);
            if (result < 0 || _hexSpriteMap.MapContainer.Width <= result || _hexSpriteMap.MapContainer.Height <= result)
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:3555
    public int GetHexY()
    {
        int result = default;
        int X = default;
        if (_y != -2147483648)
        {
            HexGeometry.PixelToHex(_hexSpriteMap._viewPixelX + _x, _hexSpriteMap._viewPixelY + _y, out X, out result);
            if (result < 0 || _hexSpriteMap.MapContainer.Width <= result || _hexSpriteMap.MapContainer.Height <= result)
            {
                result = -1;
            }
        }
        else
        {
            result = -1;
        }

        return result;
    }

    // HSEngine.pas:3570
    public void SetCapture(object value)
    {
        if (!_status.Contains(StatusElement.ChangingCapture) && value != _capture)
        {
            _status = _status + Set<StatusElement>.Of(StatusElement.ChangingCapture);
            _capture = value;
            if (value == null)
            {
                _cursor = 0;
            }

            OnCaptureChangedStore.Raise(this);
            _status = _status - Set<StatusElement>.Of(StatusElement.ChangingCapture);
        }
    }

    public GameMap HexSpriteMap { get => _hexSpriteMap; internal set => _hexSpriteMap = value; }
    public int X { get => _x; }
    public int Y { get => _y; }
    public int PixelX { get => GetPixelX(); }
    public int PixelY { get => GetPixelY(); }
    public int HexX { get => GetHexX(); }
    public int HexY { get => GetHexY(); }
    public MouseButton Button { get => _button; }
    public Set<ShiftState> Shift { get => _shift; }

    public event NotifyEvent<HexSpriteMapMouse> OnUp
    {
        add => OnUpStore.Add(value);
        remove => OnUpStore.Remove(value);
    }

    public event NotifyEvent<HexSpriteMapMouse> OnDown
    {
        add => OnDownStore.Add(value);
        remove => OnDownStore.Remove(value);
    }

    public event NotifyEvent<HexSpriteMapMouse> OnMove
    {
        add => OnMoveStore.Add(value);
        remove => OnMoveStore.Remove(value);
    }

    public event NotifyEvent<HexSpriteMapMouse> OnDoubleClick
    {
        add => OnDoubleClickStore.Add(value);
        remove => OnDoubleClickStore.Remove(value);
    }

    public event NotifyEvent<HexSpriteMapMouse> OnCaptureChanged
    {
        add => OnCaptureChangedStore.Add(value);
        remove => OnCaptureChangedStore.Remove(value);
    }

    public object Capture { get => _capture; set => SetCapture(value); }
    public int Cursor { get => GetCursor(); set => SetCursor(value); }
    public int CursorMode { get => _cursorMode; set => _cursorMode = value; }
    public bool IgnoreDown { get => _ignoreDown; set => _ignoreDown = value; }
    public bool CursorVisible { get => _cursorVisible; set => _cursorVisible = value; }

    public enum StatusElement : byte
    {
        ChangingCapture = 0
    }

}
