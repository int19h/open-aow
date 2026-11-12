// Translated from AOWWinManager.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The WindowManager unit of aowInt.dpl: TAOWWinManager, the root of a screen of AoW windows. It
//   owns the windows (its children, sorted by priority), draws the background and foreground
//   textures and the windows into the display surface, keeps the dirty rectangles of the whole
//   screen and passes the mouse and keyboard input to the windows from the top down. }
// Fields: _foregroundTexture, _backgroundTexture, _overlayPriority (windows below it get no
// input), NewOverlayPriority (applied by Draw), _borderLeft, _borderTop, _borderRight,
// FBorderBottom, FOldCursor (the cursor before CheckMouseMove), FDefaultCursor (the cursor when
// no window claims the mouse), FOnChangeCursor, FILIndexTBack, FILIndexTFore, FDrawOverlay
// (draw the foreground texture over the windows below the overlay priority)
public partial class WindowManager : UIComponent
{
    private InterfaceImageLibrary _foregroundTexture;
    private InterfaceImageLibrary _backgroundTexture;
    private int _overlayPriority;
    private int _newOverlayPriority;
    private int _borderLeft;
    private int _borderTop;
    private int _borderRight;
    private int _borderBottom;
    private int _oldCursor;
    private int _defaultCursor;
    private NotifyEvent<WindowManager> _onChangeCursor;
    private int _imageLibraryIndexTBack;
    private int _imageLibraryIndexTFore;
    private bool _drawOverlay;

    // AOWWinManager.pas:81
    public WindowManager(Component owner) : base(owner)
    {
        SetWinHeightWithoutInvalidate(480);
        SetWinWidthWithoutInvalidate(640);
        _oldCursor = -2;
        _defaultCursor = -2;
        _drawOverlay = true;
    }

    // AOWWinManager.pas:91
    public override void Validate()
    {
        if (_backgroundTexture != null)
        {
            _backgroundTexture = _backgroundTexture.CheckImageLibrary(_imageLibraryIndexTBack, 1, this);
        }

        if (_foregroundTexture != null)
        {
            _foregroundTexture = _foregroundTexture.CheckImageLibrary(_imageLibraryIndexTFore, 1, this);
        }

        if (_backgroundTexture != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWWinManager.pas:176
    public override void Update()
    {
        UIComponent C = default;
        try
        {
            if (Valid && Visible)
            {
                if (_onUpdate != null)
                {
                    _onUpdate(this);
                }

                for (int I = 0, last = Children.Count - 1; I <= last; I++)
                {
                    C = Children[I];
                    if (C.Visible && C.Priority >= _overlayPriority)
                    {
                        C.Update();
                    }
                }

                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Update", "Error - " + Name, 0);
        }
    }

    // AOWWinManager.pas:204
    // { InvalidateRect passes ARect (screen coordinates) on to the windows from Index up that it touches
    //   (a rectangle from a window only when that window is not below the overlay priority): a
    //   double-buffered window adds it, in its own coordinates, to its blit list, any other window adds
    //   it to its draw list. The window itself is passed as the Sender. }
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        UIComponent C;
        Rect R = default;
        if (Valid && Visible)
        {
            if (sender == null || sender != null && sender is WindowControl && Children[index].Priority >= _overlayPriority)
            {
                for (int I = index, last = Children.Count - 1; I <= last; I++)
                {
                    C = Children[I];
                    if (C.Valid && C.Visible && WinLeft + C.WinLeft <= rectangle.Right && WinLeft + C.WinLeft + C.WinWidth >= rectangle.Left && WinTop + C.WinTop <= rectangle.Bottom && WinTop + C.WinTop + C.WinHeight >= rectangle.Top && rectangle.Left <= rectangle.Right && rectangle.Top <= rectangle.Bottom)
                    {
                        if (C._doubleBuffered)
                        {
                            R = new Rect(rectangle.Left - C.WinLeft, rectangle.Top - C.WinTop, rectangle.Right - C.WinLeft, rectangle.Bottom - C.WinTop);
                            C.AddBlitRectangle(ref R, C);
                        }
                        else
                        {
                            C.AddDrawRectangle(ref rectangle, C);
                        }
                    }
                }
            }
        }
    }

    // AOWWinManager.pas:239
    // { DrawSurface draws the screen into Surface: first it brings the windows up to date and adds the
    //   screen area that a hidden or moved window uncovered to the draw list (only the frame strips for a
    //   masked window), then it redraws the background (all of it the first time, else only the dirty
    //   rectangles) and asks every window to draw itself; the rectangles each window blitted are passed
    //   on to the windows above it, and the foreground texture is drawn again over them when the window
    //   is below the overlay priority. FBackRect holds the window's last place as (Left, Top, Width,
    //   Height). }
    public override void DrawSurface(Surface surface)
    {
        WindowControl C;
        DirtyRectangle D;
        bool wasValid;
        LockedSurface lockedSurface = default;
        Rect R = default;
        try
        {
            Setup();
            Update();
            if (Valid)
            {
                {
                    for (int I = 0, last = Children.Count - 1; I <= last; I++)
                    {
                        C = (WindowControl)Children[I];
                        if (C.Valid)
                        {
                            if (!Drawn)
                            {
                                if (C._doubleBuffered)
                                {
                                    C.SurfaceValid = false;
                                }
                                else
                                {
                                    C.Drawn = false;
                                }
                            }

                            if (C.Hidden == true)
                            {
                                C.Hidden = false;
                                R = new Rect(C.WinLeft, C.WinTop, C.WinLeft + C.WinWidth, C.WinTop + C.WinHeight);
                                AddDrawRectangle(ref R, C);
                            }
                            else
                            {
                                if (C.Visible && Drawn && C.BackRectangle.Right > 0 && C.BackRectangle.Bottom > 0)
                                {
                                    // { the window moved: add the parts of its old place it no longer covers. For a
                                    //                 masked window the strips are off (original bugs): the third strip of the left
                                    //                 and right cases is everything below the top frame, and the right case starts
                                    //                 at FWinLeft + FFrameRight }
                                    if (C.BackRectangle.Left < C.WinLeft)
                                    {
                                        if (C.Masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.WinLeft + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.FrameBottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top + C.FrameBottom, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.WinLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Top < C.WinTop)
                                    {
                                        if (C.Masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.WinTop + C.FrameTop);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.WinTop);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Left + C.BackRectangle.Right > C.WinLeft + C.WinWidth)
                                    {
                                        if (C.Masked)
                                        {
                                            R = new Rect(C.WinLeft + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.FrameBottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top + C.FrameBottom, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.WinLeft + C.WinWidth, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Top + C.BackRectangle.Bottom > C.WinTop + C.WinHeight)
                                    {
                                        if (C.Masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.WinTop + C.FrameTop, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.WinTop + C.WinHeight, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }
                                }

                                C.BackRectangle.Top = C.WinTop;
                                C.BackRectangle.Left = C.WinLeft;
                                C.BackRectangle.Right = C.WinWidth;
                                C.BackRectangle.Bottom = C.WinHeight;
                            }
                        }
                    }
                }
            }

            if (!Drawn)
            {
                if (surface.Lock(ref lockedSurface) >= 0)
                {
                    Draw(lockedSurface);
                    if (_drawOverlay && _overlayPriority > 0)
                    {
                        var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, rectangle, ref lockedSurface);
                    }

                    surface.Unlock();
                }
            }
            else
            {
                if (surface.Lock(ref lockedSurface) >= 0)
                {
                    for (int J = 0, last = DrawCount - 1; J <= last; J++)
                    {
                        D = DrawList[J];
                        if (WinLeft > D.Rectangle.Left)
                        {
                            D.Rectangle.Left = WinLeft;
                        }

                        if (WinTop > D.Rectangle.Top)
                        {
                            D.Rectangle.Top = WinTop;
                        }

                        D.Rectangle.Right += 1;
                        D.Rectangle.Bottom += 1;
                        if (WinLeft + WinWidth < D.Rectangle.Right)
                        {
                            D.Rectangle.Right = WinLeft + WinWidth;
                        }

                        if (WinTop + WinHeight < D.Rectangle.Bottom)
                        {
                            D.Rectangle.Bottom = WinTop + WinHeight;
                        }

                        var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right, D.Rectangle.Bottom);
                        lockedSurface.ClipRectangle = rectangle;
                        Draw(lockedSurface);
                        if (_drawOverlay && _overlayPriority > 0)
                        {
                            var rectangle2 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                            _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, rectangle2, ref lockedSurface);
                        }

                        InvalidateRectangle(0, ref D.Rectangle, null);
                    }

                    DrawCount = 0;
                    surface.Unlock();
                }
            }

            for (int I = 0, last = Children.Count - 1; I <= last; I++)
            {
                C = (WindowControl)Children[I];
                C.Hidden = false;
                if (C.Valid && C.Visible)
                {
                    wasValid = C.SurfaceValid;
                    if (!C.Drawn)
                    {
                        wasValid = false;
                    }

                    C.DrawSurface(surface);
                    if (!wasValid || !C.SurfaceValid)
                    {
                        R = new Rect(C.WinLeft, C.WinTop, C.WinLeft + C.WinWidth, C.WinTop + C.WinHeight);
                        AddBlitRectangle(ref R, C);
                    }

                    C.Drawn = true;
                    C.SurfaceValid = true;
                    for (int J = 0, lastJ = BlitCount - 1; J <= lastJ; J++)
                    {
                        D = BlitList[J];
                        InvalidateRectangle(I + 1, ref D.Rectangle, null);
                    }

                    if (_drawOverlay && C.Priority < _overlayPriority)
                    {
                        if (surface.Lock(ref lockedSurface) >= 0)
                        {
                            for (int J = 0, lastJ = BlitCount - 1; J <= lastJ; J++)
                            {
                                D = BlitList[J];
                                if (C.WinLeft > D.Rectangle.Left)
                                {
                                    D.Rectangle.Left = C.WinLeft;
                                }

                                if (C.WinTop > D.Rectangle.Top)
                                {
                                    D.Rectangle.Top = C.WinTop;
                                }

                                D.Rectangle.Right += 1;
                                D.Rectangle.Bottom += 1;
                                if (C.WinLeft + C.WinWidth < D.Rectangle.Right)
                                {
                                    D.Rectangle.Right = C.WinLeft + C.WinWidth;
                                }

                                if (C.WinTop + C.WinHeight < D.Rectangle.Bottom)
                                {
                                    D.Rectangle.Bottom = C.WinTop + C.WinHeight;
                                }

                                var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right, D.Rectangle.Bottom);
                                lockedSurface.ClipRectangle = rectangle;
                                var rectangle2 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                                _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, rectangle2, ref lockedSurface);
                            }

                            surface.Unlock();
                        }
                    }

                    BlitCount = 0;
                }
            }

            Drawn = true;
            ClearDrawList();
            ClearBlitList();
        }
        catch
        {
            MessageBox(0, "Exception during DrawSurface", "Error - " + Name, 0);
        }
    }

    // AOWWinManager.pas:157
    public override void Draw(LockedSurface lockedSurface)
    {
        UIComponent C;
        if (_overlayPriority > _newOverlayPriority)
        {
            for (int I = 0, last = Children.Count - 1; I <= last; I++)
            {
                C = Children[I];
                if (C.Priority >= _newOverlayPriority && C.Priority < _overlayPriority && C.Visible)
                {
                    C.Drawn = false;
                    C.SurfaceValid = false;
                }
            }
        }

        _overlayPriority = _newOverlayPriority;
        var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
        _backgroundTexture.DrawTiled(_imageLibraryIndexTBack, rectangle, ref lockedSurface);
    }

    // AOWWinManager.pas:449
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        bool redraw = default;
        Handled = false;
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C.Priority >= _overlayPriority)
            {
                redraw = C.Redraw;
                C.Handled = false;
                if (!Handled || redraw == true)
                {
                    C.CheckMouseDown(button, shift, X, Y);
                }

                if (C.Handled)
                {
                    Handled = true;
                }
            }
        }

        if (_onMouseDown != null)
        {
            _onMouseDown(this, button, shift, X, Y);
        }
    }

    // AOWWinManager.pas:473
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C.Priority >= _overlayPriority)
            {
                if (!Handled || C.Redraw == true)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }
        }

        if (_onMouseUp != null)
        {
            _onMouseUp(this, button, shift, X, Y);
        }
    }

    // AOWWinManager.pas:494
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        WindowControl C = default;
        Handled = false;
        _oldCursor = _cursor;
        _cursor = _defaultCursor;
        if (X < 0)
        {
            X = 0;
        }

        if (Y < 0)
        {
            Y = 0;
        }

        if (WinWidth <= X)
        {
            X = WinWidth - 1;
        }

        if (WinHeight <= Y)
        {
            Y = WinHeight - 1;
        }

        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = (WindowControl)Children[I];
            if (C.Priority >= _overlayPriority)
            {
                if (!Handled || C.Redraw == true)
                {
                    C.CheckMouseMove(shift, X, Y);
                    if (C.Handled == true)
                    {
                        if (!Handled)
                        {
                            _cursor = C._cursor;
                            if (!C.MouseInside)
                            {
                                C.MouseInside = true;
                                if (C._onEnter != null)
                                {
                                    C._onEnter(C);
                                }
                            }
                        }
                        else
                        {
                            if (C.MouseInside)
                            {
                                C.MouseInside = false;
                                if (C._onExit != null)
                                {
                                    C._onExit(C);
                                }
                            }
                        }

                        Handled = true;
                    }
                    else
                    {
                        if (C.MouseInside)
                        {
                            C.MouseInside = false;
                            if (C._onExit != null)
                            {
                                C._onExit(C);
                            }
                        }
                    }
                }
                else
                {
                    if (C.MouseInside)
                    {
                        C.MouseInside = false;
                        if (C._onExit != null)
                        {
                            C._onExit(C);
                        }
                    }
                }
            }
        }

        if (_cursor != _oldCursor)
        {
            if (_onChangeCursor != null)
            {
                _onChangeCursor(this);
            }
        }

        if (_onMouseMove != null)
        {
            _onMouseMove(this, shift, X, Y);
        }
    }

    // AOWWinManager.pas:558
    public override void CheckKeyPress(ref byte key)
    {
        UIComponent C = default;
        Handled = false;
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (C.Priority >= _overlayPriority)
            {
                C.CheckKeyPress(ref key);
                if (C.Handled == true)
                {
                    Handled = true;
                }
            }
        }

        if (_onKeyPress != null)
        {
            _onKeyPress(this, ref key);
        }
    }

    // AOWWinManager.pas:578
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        UIComponent C = default;
        Handled = false;
        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = Children[I];
            if (!Handled && C.Priority >= _overlayPriority && C.Visible)
            {
                C.CheckKeyDown(ref key, shift);
                if (C.Handled == true)
                {
                    Handled = true;
                }
            }
        }

        if (!Handled && _onKeyDown != null)
        {
            _onKeyDown(this, ref key, shift);
        }
    }

    // AOWWinManager.pas:102
    public virtual void SetILTB(InterfaceImageLibrary value)
    {
        if (value != _backgroundTexture)
        {
            Valid = false;
            _backgroundTexture = value;
        }
    }

    // AOWWinManager.pas:111
    public virtual void SetILTF(InterfaceImageLibrary value)
    {
        if (value != _foregroundTexture)
        {
            Valid = false;
            _foregroundTexture = value;
        }
    }

    // AOWWinManager.pas:120
    public void SetImageLibraryIndex(int value)
    {
        if (value != _imageLibraryIndexTBack)
        {
            _imageLibraryIndexTBack = value;
            Valid = false;
        }
    }

    // AOWWinManager.pas:129
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexTFore)
        {
            _imageLibraryIndexTFore = value;
            Valid = false;
        }
    }

    // AOWWinManager.pas:138
    public void SetOverPriority(int value)
    {
        if (value != _overlayPriority || value != _newOverlayPriority)
        {
            _newOverlayPriority = value;
            Drawn = false;
        }
    }

    // AOWWinManager.pas:147
    public void SetCursor(int value)
    {
        if (value != _cursor)
        {
            _cursor = value;
            if (_onChangeCursor != null)
            {
                _onChangeCursor(this);
            }
        }
    }

    [Published]
    public int Cursor { get => _cursor; set => SetCursor(value); }

    [Published]
    public NotifyEvent<WindowManager> OnChangeCursor { get => _onChangeCursor; set => _onChangeCursor = value; }

    [Published]
    public InterfaceImageLibrary BackgroundTexture { get => _backgroundTexture; set => SetILTB(value); }

    [Published]
    public InterfaceImageLibrary ForegroundTexture { get => _foregroundTexture; set => SetILTF(value); }

    [Published]
    public int ImageLibraryIndexTBack { get => _imageLibraryIndexTBack; set => SetImageLibraryIndex(value); }

    [Published]
    public int ImageLibraryIndexTFore { get => _imageLibraryIndexTFore; set => SetILFIndex(value); }

    [Published]
    public int OverlayPriority { get => _overlayPriority; set => SetOverPriority(value); }

    internal void SetOverlayPriorityImmediately(int value) => _overlayPriority = value;

    [Published]
    public int BorderLeft { get => _borderLeft; set => _borderLeft = value; }

    [Published]
    public int BorderRight { get => _borderRight; set => _borderRight = value; }

    [Published]
    public int BorderTop { get => _borderTop; set => _borderTop = value; }

    [Published]
    public int BorderBottom { get => _borderBottom; set => _borderBottom = value; }

    [Published]
    public bool DrawOverlay { get => _drawOverlay; set => _drawOverlay = value; }

    public new NotifyEvent<WindowManager> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<WindowManager> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<WindowManager> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<WindowManager> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<WindowManager> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<WindowManager> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<WindowManager> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<WindowManager> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<WindowManager> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
