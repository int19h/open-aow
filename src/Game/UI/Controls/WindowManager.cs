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
    public InterfaceImageLibrary _foregroundTexture;
    public InterfaceImageLibrary _backgroundTexture;
    public int _overlayPriority;
    public int NewOverlayPriority;
    public int _borderLeft;
    public int _borderTop;
    public int _borderRight;
    public int _borderBottom;
    public int OldCursor;
    public int DefaultCursor;
    public NotifyEvent<WindowManager> _onChangeCursor;
    public int _imageLibraryIndexTBack;
    public int _imageLibraryIndexTFore;
    public bool _drawOverlay;

    // AOWWinManager.pas:81
    public WindowManager(Component owner) : base(owner)
    {
        _winHeight = 480;
        _winWidth = 640;
        OldCursor = -2;
        DefaultCursor = -2;
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
            if (Valid && _visible)
            {
                if (_onUpdate != null)
                {
                    _onUpdate(this);
                }

                for (int I = 0, last = Children.Count - 1; I <= last; I++)
                {
                    C = (UIComponent)Children[I];
                    if (C._visible && C._priority >= _overlayPriority)
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
        if (Valid && _visible)
        {
            if (sender == null || sender != null && sender is WindowControl && ((UIComponent)Children[index])._priority >= _overlayPriority)
            {
                for (int I = index, last = Children.Count - 1; I <= last; I++)
                {
                    C = (UIComponent)Children[I];
                    if (C.Valid && C._visible && _winLeft + C._winLeft <= rectangle.Right && _winLeft + C._winLeft + C._winWidth >= rectangle.Left && _winTop + C._winTop <= rectangle.Bottom && _winTop + C._winTop + C._winHeight >= rectangle.Top && rectangle.Left <= rectangle.Right && rectangle.Top <= rectangle.Bottom)
                    {
                        if (C._doubleBuffered)
                        {
                            R = new Rect(rectangle.Left - C._winLeft, rectangle.Top - C._winTop, rectangle.Right - C._winLeft, rectangle.Bottom - C._winTop);
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
                                R = new Rect(C._winLeft, C._winTop, C._winLeft + C._winWidth, C._winTop + C._winHeight);
                                AddDrawRectangle(ref R, C);
                            }
                            else
                            {
                                if (C._visible && Drawn && C.BackRectangle.Right > 0 && C.BackRectangle.Bottom > 0)
                                {
                                    // { the window moved: add the parts of its old place it no longer covers. For a
                                    //                 masked window the strips are off (original bugs): the third strip of the left
                                    //                 and right cases is everything below the top frame, and the right case starts
                                    //                 at FWinLeft + FFrameRight }
                                    if (C.BackRectangle.Left < C._winLeft)
                                    {
                                        if (C._masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C._winLeft + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.FrameBottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top + C.FrameBottom, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C._winLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Top < C._winTop)
                                    {
                                        if (C._masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C._winTop + C.FrameTop);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C._winTop);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Left + C.BackRectangle.Right > C._winLeft + C._winWidth)
                                    {
                                        if (C._masked)
                                        {
                                            R = new Rect(C._winLeft + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.FrameBottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top + C.FrameBottom, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C._winLeft + C._winWidth, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }

                                    if (C.BackRectangle.Top + C.BackRectangle.Bottom > C._winTop + C._winHeight)
                                    {
                                        if (C._masked)
                                        {
                                            R = new Rect(C.BackRectangle.Left, C._winTop + C.FrameTop, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left, C.BackRectangle.Top, C.BackRectangle.Left + C.FrameLeft, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                            R = new Rect(C.BackRectangle.Left + C.FrameRight, C.BackRectangle.Top, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                        else
                                        {
                                            R = new Rect(C.BackRectangle.Left, C._winTop + C._winHeight, C.BackRectangle.Left + C.BackRectangle.Right, C.BackRectangle.Top + C.BackRectangle.Bottom);
                                            AddDrawRectangle(ref R, C);
                                        }
                                    }
                                }

                                C.BackRectangle.Top = C._winTop;
                                C.BackRectangle.Left = C._winLeft;
                                C.BackRectangle.Right = C._winWidth;
                                C.BackRectangle.Bottom = C._winHeight;
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
                        var rectangle = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
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
                        if (_winLeft > D.Rectangle.Left)
                        {
                            D.Rectangle.Left = _winLeft;
                        }

                        if (_winTop > D.Rectangle.Top)
                        {
                            D.Rectangle.Top = _winTop;
                        }

                        D.Rectangle.Right += 1;
                        D.Rectangle.Bottom += 1;
                        if (_winLeft + _winWidth < D.Rectangle.Right)
                        {
                            D.Rectangle.Right = _winLeft + _winWidth;
                        }

                        if (_winTop + _winHeight < D.Rectangle.Bottom)
                        {
                            D.Rectangle.Bottom = _winTop + _winHeight;
                        }

                        var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right, D.Rectangle.Bottom);
                        lockedSurface.ClipRectangle = rectangle;
                        Draw(lockedSurface);
                        if (_drawOverlay && _overlayPriority > 0)
                        {
                            var rectangle2 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
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
                if (C.Valid && C._visible)
                {
                    wasValid = C.SurfaceValid;
                    if (!C.Drawn)
                    {
                        wasValid = false;
                    }

                    C.DrawSurface(surface);
                    if (!wasValid || !C.SurfaceValid)
                    {
                        R = new Rect(C._winLeft, C._winTop, C._winLeft + C._winWidth, C._winTop + C._winHeight);
                        AddBlitRectangle(ref R, C);
                    }

                    C.Drawn = true;
                    C.SurfaceValid = true;
                    for (int J = 0, lastJ = BlitCount - 1; J <= lastJ; J++)
                    {
                        D = BlitList[J];
                        InvalidateRectangle(I + 1, ref D.Rectangle, null);
                    }

                    if (_drawOverlay && C._priority < _overlayPriority)
                    {
                        if (surface.Lock(ref lockedSurface) >= 0)
                        {
                            for (int J = 0, lastJ = BlitCount - 1; J <= lastJ; J++)
                            {
                                D = BlitList[J];
                                if (C._winLeft > D.Rectangle.Left)
                                {
                                    D.Rectangle.Left = C._winLeft;
                                }

                                if (C._winTop > D.Rectangle.Top)
                                {
                                    D.Rectangle.Top = C._winTop;
                                }

                                D.Rectangle.Right += 1;
                                D.Rectangle.Bottom += 1;
                                if (C._winLeft + C._winWidth < D.Rectangle.Right)
                                {
                                    D.Rectangle.Right = C._winLeft + C._winWidth;
                                }

                                if (C._winTop + C._winHeight < D.Rectangle.Bottom)
                                {
                                    D.Rectangle.Bottom = C._winTop + C._winHeight;
                                }

                                var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right, D.Rectangle.Bottom);
                                lockedSurface.ClipRectangle = rectangle;
                                var rectangle2 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
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
        if (_overlayPriority > NewOverlayPriority)
        {
            for (int I = 0, last = Children.Count - 1; I <= last; I++)
            {
                C = (UIComponent)Children[I];
                if (C._priority >= NewOverlayPriority && C._priority < _overlayPriority && C._visible)
                {
                    C.Drawn = false;
                    C.SurfaceValid = false;
                }
            }
        }

        _overlayPriority = NewOverlayPriority;
        var rectangle = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
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
            C = (UIComponent)Children[I];
            if (C._priority >= _overlayPriority)
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
            C = (UIComponent)Children[I];
            if (C._priority >= _overlayPriority)
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
        OldCursor = _cursor;
        _cursor = DefaultCursor;
        if (X < 0)
        {
            X = 0;
        }

        if (Y < 0)
        {
            Y = 0;
        }

        if (_winWidth <= X)
        {
            X = _winWidth - 1;
        }

        if (_winHeight <= Y)
        {
            Y = _winHeight - 1;
        }

        for (int I = Children.Count - 1; I >= 0; I--)
        {
            C = (WindowControl)Children[I];
            if (C._priority >= _overlayPriority)
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

        if (_cursor != OldCursor)
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
            C = (UIComponent)Children[I];
            if (C._priority >= _overlayPriority)
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
            C = (UIComponent)Children[I];
            if (!Handled && C._priority >= _overlayPriority && C._visible)
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
        if (value != _overlayPriority || value != NewOverlayPriority)
        {
            NewOverlayPriority = value;
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
