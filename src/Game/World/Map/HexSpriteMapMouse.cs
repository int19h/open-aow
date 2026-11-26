#nullable disable warnings
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
        OnDownStore = new();
        Reset();
    }

    internal event NotifyEvent<HexSpriteMapMouse>? ButtonReleased;

    // Issue 326 keeps the ordered store: TargetSelectionControl.MouseDownEvent, subscribed last,
    // sets SelectedArmy.IgnoreMouseDown before SelectedArmy.MouseDownEvent reads it.
    internal EventStore<HexSpriteMapMouse> OnDownStore { get; }

    internal event NotifyEvent<HexSpriteMapMouse> ButtonPressed
    {
        add => OnDownStore.Add(value);
        remove => OnDownStore.Remove(value);
    }

    internal event NotifyEvent<HexSpriteMapMouse>? Moved;

    internal event NotifyEvent<HexSpriteMapMouse>? DoubleClicked;

    internal event NotifyEvent<HexSpriteMapMouse>? CaptureChanged;

    private object? _capture;
    private Statuses _status;
    private int _cursor;

    // HSEngine.pas:3486
    public virtual void Dispose()
    {
        ButtonReleased = null;
        OnDownStore.Clear();
        Moved = null;
        DoubleClicked = null;
        CaptureChanged = null;
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
                if (Button == MouseButton.Right && !HexSpriteMap.Status.Contains(HexSpriteMapStatuses.Editting))
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
    public virtual void Down(MouseButton button, ShiftStates shift, int X, int Y)
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
    public virtual void Up(MouseButton button, ShiftStates shift, int X, int Y)
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
            NotifyEvent<HexSpriteMapMouse>? handlers = ButtonReleased;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(ButtonReleased));
            handlers?.Invoke(this);
        }
    }

    // HSEngine.pas:3651
    public virtual void Move(ShiftStates shift, int X, int Y)
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
            Moved?.Invoke(this);
        }
    }

    // HSEngine.pas:3672
    public virtual void DoubleClick()
    {
        if (PixelX != -2_147_483_648)
        {
            if (MouseDoubleClick())
            {
                NotifyEvent<HexSpriteMapMouse>? handlers = DoubleClicked;
                EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(DoubleClicked));
                handlers?.Invoke(this);
            }
        }
    }

    // HSEngine.pas:3496
    public void Reset()
    {
        X = -2147483648;
        Y = -2147483648;
        Shift = ShiftStates.None;
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
                    return -2_147_483_648;
                }
            }
            else
            {
                return -2_147_483_648;
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
                    return -2_147_483_648;
                }
            }
            else
            {
                return -2_147_483_648;
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
    public void SetCapture(object? value)
    {
        if (!_status.Contains(Statuses.ChangingCapture) && value != _capture)
        {
            _status |= Statuses.ChangingCapture;
            _capture = value;
            if (value is null)
            {
                _cursor = 0;
            }

            NotifyEvent<HexSpriteMapMouse>? handlers = CaptureChanged;
            EventChecks.AtMostOneHandler(handlers, owner: this, eventName: nameof(CaptureChanged));
            handlers?.Invoke(this);
            _status &= ~Statuses.ChangingCapture;
        }
    }

    public GameMap HexSpriteMap { get; internal set; }
    public int X { get; private set; }
    public int Y { get; private set; }
    public MouseButton Button { get; private set; }
    public ShiftStates Shift { get; private set; }

    public object? Capture { get => _capture; set => SetCapture(value); }
    public int Cursor { get => GetCursor(); set => SetCursor(value); }
    public int CursorMode { get; set; }
    public bool IgnoreDown { get; private set; }
    public bool CursorVisible { get; set; }

    [Flags]
    public enum Statuses : byte
    {
        None = 0,
        ChangingCapture = 1 << 0,
    }
}
