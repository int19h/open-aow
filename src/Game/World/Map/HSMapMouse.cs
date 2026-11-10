// Translated from HSEngine.pas.
using Aow.Engine;

using Aow.Rtl;

namespace Aow.Game.World.Map;

// the mouse of a map. FHSMap, FX, FY (the position in the view port in pixels, Low(Integer)
// when outside), FButton, FOnUp, FOnDown, FOnMove, FOnDblClick, FOnCaptureChanged, FCapture
// (the object that gets the mouse events), FStatus, FCursor, FCursorMode, FIgnoreDown, FShift,
// FCursorVisible (and are used by the game only)
public partial class HSMapMouse : IDisposable
{

    public HSMapMouse()
    {
        FOnUpStore = new();
        FOnDownStore = new();
        FOnMoveStore = new();
        FOnDblClickStore = new();
        FOnCaptureChangedStore = new();
        Reset();
    }

    internal HSMap FHSMap;
    internal int FX;
    internal int FY;
    internal MouseButton FButton;

    internal EventStore<HSMapMouse> FOnUpStore;

    internal event NotifyEvent<HSMapMouse> FOnUp
    {
        add => FOnUpStore.Add(value);
        remove => FOnUpStore.Remove(value);
    }

    internal EventStore<HSMapMouse> FOnDownStore;

    internal event NotifyEvent<HSMapMouse> FOnDown
    {
        add => FOnDownStore.Add(value);
        remove => FOnDownStore.Remove(value);
    }

    internal EventStore<HSMapMouse> FOnMoveStore;

    internal event NotifyEvent<HSMapMouse> FOnMove
    {
        add => FOnMoveStore.Add(value);
        remove => FOnMoveStore.Remove(value);
    }

    internal EventStore<HSMapMouse> FOnDblClickStore;

    internal event NotifyEvent<HSMapMouse> FOnDblClick
    {
        add => FOnDblClickStore.Add(value);
        remove => FOnDblClickStore.Remove(value);
    }

    internal EventStore<HSMapMouse> FOnCaptureChangedStore;

    internal event NotifyEvent<HSMapMouse> FOnCaptureChanged
    {
        add => FOnCaptureChangedStore.Add(value);
        remove => FOnCaptureChangedStore.Remove(value);
    }

    internal object FCapture;
    internal Set<MouseStatusElement> FStatus;
    internal int FCursor;
    internal int FCursorMode;
    internal bool FIgnoreDown;
    internal Set<ShiftState> FShift;
    internal bool FCursorVisible;

    // HSEngine.pas:3475

    // HSEngine.pas:3486
    public virtual void Dispose()
    {
        FOnUpStore.Clear();
        FOnDownStore.Clear();
        FOnMoveStore.Clear();
        FOnDblClickStore.Clear();
        FOnCaptureChangedStore.Clear();
    }

    // HSEngine.pas:3505
    public virtual int GetCursor() => FCursor;

    // HSEngine.pas:3510
    public virtual void SetCursor(int Value)
    {
        FCursor = Value;
    }

    // HSEngine.pas:3583
    public virtual bool MouseDown()
    {
        bool Result = true;
        if (FCapture == null && GetXhx() != -1)
        {
            if (FButton == MouseButton.Left)
            {
                var yhp = GetYhp();
                var xhp = GetXhp();
                FHSMap.FSelection.SelectLocation(xhp, yhp, FHSMap.FViewL);
                Result = false;
            }
            else
            {
                if (FButton == MouseButton.Right && !FHSMap.FStatus.Contains(HSMapStatusElement.Editting))
                {
                    FHSMap.FSelection.UnselectLocation();
                    Result = false;
                }
            }
        }

        return Result;
    }

    // HSEngine.pas:3623
    public virtual bool MouseUp() => true;

    // HSEngine.pas:3646
    public virtual bool MouseMove() => true;

    // HSEngine.pas:3667
    public virtual bool MouseDblClick() => true;

    // HSEngine.pas:3600
    // { Down and Up set FX twice when the position is outside (an original slip for FY). }
    public virtual void Down(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (!FIgnoreDown)
        {
            if (X >= 0 && Y >= 0)
            {
                FX = X;
                FY = Y;
            }
            else
            {
                FX = -2147483648;
                FX = -2147483648;
            }

            FButton = Button;
            FShift = Shift;
            if (MouseDown())
            {
                FOnDownStore.Raise(this);
            }
        }
        else
        {
            FIgnoreDown = false;
        }
    }

    // HSEngine.pas:3628
    public virtual void Up(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        if (X >= 0 && Y >= 0)
        {
            FX = X;
            FY = Y;
        }
        else
        {
            FX = -2147483648;
            FX = -2147483648;
        }

        FButton = Button;
        FShift = Shift;
        if (MouseUp())
        {
            FOnUpStore.Raise(this);
        }
    }

    // HSEngine.pas:3651
    public virtual void Move(Set<ShiftState> Shift, int X, int Y)
    {
        // Port extension: a view wider or taller than the map takes scene points past MaxXhp or MaxYhp.
        // GetXhp and GetYhp still test the map coordinates.
        if (X >= 0 && Y >= 0 && X <= Math.Max(FHSMap.GetMaxXhp(), FHSMap.FViewWidth) && Y <= Math.Max(FHSMap.GetMaxYhp(), FHSMap.FViewHeight))
        {
            FX = X;
            FY = Y;
        }
        else
        {
            FX = -2147483648;
            FY = -2147483648;
        }

        if (MouseMove())
        {
            FOnMoveStore.Raise(this);
        }
    }

    // HSEngine.pas:3672
    public virtual void DblClick()
    {
        if (GetXhp() != -2147483648)
        {
            if (MouseDblClick())
            {
                FOnDblClickStore.Raise(this);
            }
        }
    }

    // HSEngine.pas:3496
    public void Reset()
    {
        FX = -2147483648;
        FY = -2147483648;
        FShift = Set<ShiftState>.Of();
        FCursor = 0;
        SetCapture(null);
    }

    // HSEngine.pas:3515
    public int GetXhp()
    {
        int Result;
        if (FX != -2147483648)
        {
            Result = FHSMap.FViewXhp + FX;
            if (Result > FHSMap.GetMaxXhp() || Result < 0)
            {
                Result = -2147483648;
            }
        }
        else
        {
            Result = -2147483648;
        }

        return Result;
    }

    // HSEngine.pas:3527
    public int GetYhp()
    {
        int Result;
        if (FY != -2147483648)
        {
            Result = FHSMap.FViewYhp + FY;
            if (Result > FHSMap.GetMaxYhp() || Result < 0)
            {
                Result = -2147483648;
            }
        }
        else
        {
            Result = -2147483648;
        }

        return Result;
    }

    // HSEngine.pas:3540
    // { Both GetXhx and GetYhx test the result against the width and the height of the map. }
    public int GetXhx()
    {
        int Result = default;
        int Y = default;
        if (FX != -2147483648)
        {
            HexGeometry.HPtoHX(FHSMap.FViewXhp + FX, 0, ref Result, ref Y);
            if (Result < 0 || FHSMap.FMapContainer.FWidth <= Result || FHSMap.FMapContainer.FHeight <= Result)
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // HSEngine.pas:3555
    public int GetYhx()
    {
        int Result = default;
        int X = default;
        if (FY != -2147483648)
        {
            HexGeometry.HPtoHX(FHSMap.FViewXhp + FX, FHSMap.FViewYhp + FY, ref X, ref Result);
            if (Result < 0 || FHSMap.FMapContainer.FWidth <= Result || FHSMap.FMapContainer.FHeight <= Result)
            {
                Result = -1;
            }
        }
        else
        {
            Result = -1;
        }

        return Result;
    }

    // HSEngine.pas:3570
    public void SetCapture(object Value)
    {
        if (!FStatus.Contains(MouseStatusElement.ChangingCapture) && Value != FCapture)
        {
            FStatus = FStatus + Set<MouseStatusElement>.Of(MouseStatusElement.ChangingCapture);
            FCapture = Value;
            if (Value == null)
            {
                FCursor = 0;
            }

            FOnCaptureChangedStore.Raise(this);
            FStatus = FStatus - Set<MouseStatusElement>.Of(MouseStatusElement.ChangingCapture);
        }
    }

    public HSMap HSMap { get => FHSMap; }
    public int X { get => FX; }
    public int Y { get => FY; }
    public int Xhp { get => GetXhp(); }
    public int Yhp { get => GetYhp(); }
    public int Xhx { get => GetXhx(); }
    public int Yhx { get => GetYhx(); }
    public MouseButton Button { get => FButton; }
    public Set<ShiftState> Shift { get => FShift; }

    public event NotifyEvent<HSMapMouse> OnUp
    {
        add => FOnUpStore.Add(value);
        remove => FOnUpStore.Remove(value);
    }

    public event NotifyEvent<HSMapMouse> OnDown
    {
        add => FOnDownStore.Add(value);
        remove => FOnDownStore.Remove(value);
    }

    public event NotifyEvent<HSMapMouse> OnMove
    {
        add => FOnMoveStore.Add(value);
        remove => FOnMoveStore.Remove(value);
    }

    public event NotifyEvent<HSMapMouse> OnDblClick
    {
        add => FOnDblClickStore.Add(value);
        remove => FOnDblClickStore.Remove(value);
    }

    public event NotifyEvent<HSMapMouse> OnCaptureChanged
    {
        add => FOnCaptureChangedStore.Add(value);
        remove => FOnCaptureChangedStore.Remove(value);
    }

    public object Capture { get => FCapture; set => SetCapture(value); }
    public int Cursor { get => GetCursor(); set => SetCursor(value); }
    public int CursorMode { get => FCursorMode; set => FCursorMode = value; }
    public bool IgnoreDown { get => FIgnoreDown; set => FIgnoreDown = value; }
    public bool CursorVisible { get => FCursorVisible; set => FCursorVisible = value; }

    public enum MouseStatusElement : byte
    {
        ChangingCapture = 0
    }

}
