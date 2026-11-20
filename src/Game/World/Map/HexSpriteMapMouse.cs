#nullable disable
// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// the mouse of a map. FHSMap, FX, FY (the position in the view port in pixels, Low(Integer)
// when outside), _button, ButtonUp, ButtonDown, Moved, DoubleClicked, CaptureChanged, _capture
// (the object that gets the mouse events), FStatus, FCursor, FCursorMode, FIgnoreDown, FShift,
// FCursorVisible (and are used by the game only)
public class HexSpriteMapMouse : IDisposable
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
    public virtual void SetCursor(int value) => _cursor = value;

    // HSEngine.pas:3583
    public virtual bool MouseDown()
    {
        if (_capture is null && HexX != -1)
        {
            if (Button == MouseButton.Left)
            {
                int pixelY = PixelY;
                int pixelX = PixelX;
                HexSpriteMap.Selection.SelectLocation(pixelX, pixelY, lHitPoints: HexSpriteMap.ViewMapLevel);
                return false;
            }
            else
            {
                if (Button == MouseButton.Right && !HexSpriteMap.Status.Contains(HexSpriteMapStatusElement.Editting))
                {
                    HexSpriteMap.Selection.UnselectLocation();
                    return false;
                }
            }
        }

        return true;
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
        if (!IgnoreDown)
        {
            if (X >= 0 && Y >= 0)
            {
                this.X = X;
                this.Y = Y;
            }
            else
            {
                this.X = -2147483648;
                this.X = -2147483648;
            }

            Button = button;
            Shift = shift;
            if (MouseDown())
            {
                OnDownStore.Raise(this);
            }
        }
        else
        {
            IgnoreDown = false;
        }
    }

    // HSEngine.pas:3628
    public virtual void Up(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (X >= 0 && Y >= 0)
        {
            this.X = X;
            this.Y = Y;
        }
        else
        {
            this.X = -2147483648;
            this.X = -2147483648;
        }

        Button = button;
        Shift = shift;
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
        if (X >= 0 && Y >= 0 && X <= Math.Max(HexSpriteMap.MaxPixelX, HexSpriteMap.ViewWidth) && Y <= Math.Max(HexSpriteMap.MaxPixelY, HexSpriteMap.ViewHeight))
        {
            this.X = X;
            this.Y = Y;
        }
        else
        {
            this.X = -2147483648;
            this.Y = -2147483648;
        }

        if (MouseMove())
        {
            OnMoveStore.Raise(this);
        }
    }

    // HSEngine.pas:3672
    public virtual void DoubleClick()
    {
        if (PixelX != -2147483648)
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
        X = -2147483648;
        Y = -2147483648;
        Shift = Set<ShiftState>.Of();
        _cursor = 0;
        SetCapture(null);
    }

    // HSEngine.pas:3515
    public int PixelX
    {
        get
        {
            int result;
            if (X != -2147483648)
            {
                result = HexSpriteMap.ViewPixelX + X;
                if (result > HexSpriteMap.MaxPixelX || result < 0)
                {
                    return -2147483648;
                }
            }
            else
            {
                return -2147483648;
            }

            return result;
        }
    }

    // HSEngine.pas:3527
    public int PixelY
    {
        get
        {
            int result;
            if (Y != -2147483648)
            {
                result = HexSpriteMap.ViewPixelY + Y;
                if (result > HexSpriteMap.MaxPixelY || result < 0)
                {
                    return -2147483648;
                }
            }
            else
            {
                return -2147483648;
            }

            return result;
        }
    }

    // HSEngine.pas:3540
    // { Both GetXhx and GetYhx test the result against the width and the height of the map. }
    public int HexX
    {
        get
        {
            int result;
            if (X != -2147483648)
            {
                HexGeometry.PixelToHex(HexSpriteMap.ViewPixelX + X, vertical: 0, out result, out _);
                if (result < 0 || HexSpriteMap.MapContainer.Width <= result || HexSpriteMap.MapContainer.Height <= result)
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
    }

    // HSEngine.pas:3555
    public int HexY
    {
        get
        {
            int result;
            if (Y != -2147483648)
            {
                HexGeometry.PixelToHex(HexSpriteMap.ViewPixelX + X, vertical: HexSpriteMap.ViewPixelY + Y, out _, out result);
                if (result < 0 || HexSpriteMap.MapContainer.Width <= result || HexSpriteMap.MapContainer.Height <= result)
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
    }

    // HSEngine.pas:3570
    public void SetCapture(object value)
    {
        if (!_status.Contains(StatusElement.ChangingCapture) && value != _capture)
        {
            _status += Set<StatusElement>.Of(StatusElement.ChangingCapture);
            _capture = value;
            if (value is null)
            {
                _cursor = 0;
            }

            OnCaptureChangedStore.Raise(this);
            _status -= Set<StatusElement>.Of(StatusElement.ChangingCapture);
        }
    }

    public GameMap HexSpriteMap { get; internal set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public MouseButton Button { get; private set; }
    public Set<ShiftState> Shift { get; private set; }

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
    public int CursorMode { get; set; }
    public bool IgnoreDown { get; private set; }
    public bool CursorVisible { get; set; }

    public enum StatusElement : byte
    {
        ChangingCapture = 0
    }
}
