#nullable disable warnings
// Translated from AOWWinManager.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

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
public sealed partial class WindowManager : UIComponent
{
    private InterfaceImageLibrary? _foregroundTexture;
    private InterfaceImageLibrary? _backgroundTexture;
    private int _overlayPriority;
    private int _newOverlayPriority;
    private int _oldCursor;
    private readonly int _defaultCursor;
    [Published]
    public event NotifyEvent<WindowManager>? CursorChanged;
    private int _imageLibraryIndexTBack;
    private int _imageLibraryIndexTFore;

    // AOWWinManager.pas:81
    public WindowManager(Component? owner) : base(owner)
    {
        SetWinHeightWithoutInvalidate(480);
        SetWinWidthWithoutInvalidate(640);
        _oldCursor = -2;
        _defaultCursor = -2;
        DrawOverlay = true;
    }

    // AOWWinManager.pas:91
    public override void Validate()
    {
        if (_backgroundTexture is not null)
        {
            _backgroundTexture = _backgroundTexture.CheckImageLibrary(_imageLibraryIndexTBack, count: 1, sender: this);
        }

        if (_foregroundTexture is not null)
        {
            _foregroundTexture = _foregroundTexture.CheckImageLibrary(_imageLibraryIndexTFore, count: 1, sender: this);
        }

        if (_backgroundTexture is not null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWWinManager.pas:176
    public override void Update()
    {
        try
        {
            if (Valid && Visible)
            {
                RaiseUpdate(this);

                for (int i = 0, last = Children.Count - 1; i <= last; i++)
                {
                    UIComponent? c = Children[i];
                    if (c.Visible && c.Priority >= _overlayPriority)
                    {
                        c.Update();
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
            _ = MessageBox(0, text: "Exception during Update", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AOWWinManager.pas:204
    // { InvalidateRect passes ARect (screen coordinates) on to the windows from Index up that it touches
    //   (a rectangle from a window only when that window is not below the overlay priority): a
    //   double-buffered window adds it, in its own coordinates, to its blit list, any other window adds
    //   it to its draw list. The window itself is passed as the Sender. }
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent? sender)
    {
        if (Valid && Visible)
        {
            if (sender is null || (sender is not null && sender is WindowControl && Children[index].Priority >= _overlayPriority))
            {
                for (int i = index, last = Children.Count - 1; i <= last; i++)
                {
                    UIComponent? c = Children[i];
                    if (c.Valid && c.Visible && WinLeft + c.WinLeft <= rectangle.Right && WinLeft + c.WinLeft + c.WinWidth >= rectangle.Left && WinTop + c.WinTop <= rectangle.Bottom && WinTop + c.WinTop + c.WinHeight >= rectangle.Top && rectangle.Left <= rectangle.Right && rectangle.Top <= rectangle.Bottom)
                    {
                        if (c.DoubleBuffered)
                        {
                            Rect r = new(rectangle.Left - c.WinLeft, rectangle.Top - c.WinTop, rectangle.Right - c.WinLeft, rectangle.Bottom - c.WinTop);
                            c.AddBlitRectangle(ref r, sender: c);
                        }
                        else
                        {
                            c.AddDrawRectangle(ref rectangle, sender: c);
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
    public override void DrawSurface(Surface? surface)
    {
        LockedSurface lockedSurface = default;
        try
        {
            Setup();
            Update();
            WindowControl? c;
            Rect r;
            if (Valid)
            {
                {
                    foreach (UIComponent? component in Children)
                    {
                        // The child list is UIComponent. The manager shows windows, and the cast stays.
                        c = (WindowControl?)component;
                        if (c.Valid)
                        {
                            if (!IsDrawn)
                            {
                                if (c.DoubleBuffered)
                                {
                                    c.SurfaceValid = false;
                                }
                                else
                                {
                                    c.IsDrawn = false;
                                }
                            }

                            if (c.IsHidden)
                            {
                                c.IsHidden = false;
                                r = new Rect(c.WinLeft, c.WinTop, c.WinLeft + c.WinWidth, c.WinTop + c.WinHeight);
                                AddDrawRectangle(ref r, sender: c);
                            }
                            else
                            {
                                if (c.Visible && IsDrawn && c.BackRectangle.Width > 0 && c.BackRectangle.Height > 0)
                                {
                                    // { the window moved: add the parts of its old place it no longer covers. For a
                                    //                 masked window the strips are off (original bugs): the third strip of the left
                                    //                 and right cases is everything below the top frame, and the right case starts
                                    //                 at FWinLeft + FFrameRight }
                                    if (c.BackRectangle.Left < c.WinLeft)
                                    {
                                        if (c.Masked)
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.WinLeft + c.FrameLeft, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.FrameBottom);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top + c.FrameBottom, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                        else
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.WinLeft, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                    }

                                    if (c.BackRectangle.Top < c.WinTop)
                                    {
                                        if (c.Masked)
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.WinTop + c.FrameTop);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.FrameLeft, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left + c.FrameRight, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                        else
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.WinTop);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                    }

                                    if (c.BackRectangle.Left + c.BackRectangle.Width > c.WinLeft + c.WinWidth)
                                    {
                                        if (c.Masked)
                                        {
                                            r = new Rect(c.WinLeft + c.FrameRight, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.FrameBottom);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top + c.FrameBottom, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                        else
                                        {
                                            r = new Rect(c.WinLeft + c.WinWidth, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                    }

                                    if (c.BackRectangle.Top + c.BackRectangle.Height > c.WinTop + c.WinHeight)
                                    {
                                        if (c.Masked)
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.WinTop + c.FrameTop, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left, c.BackRectangle.Top, c.BackRectangle.Left + c.FrameLeft, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                            r = new Rect(c.BackRectangle.Left + c.FrameRight, c.BackRectangle.Top, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                        else
                                        {
                                            r = new Rect(c.BackRectangle.Left, c.WinTop + c.WinHeight, c.BackRectangle.Left + c.BackRectangle.Width, c.BackRectangle.Top + c.BackRectangle.Height);
                                            AddDrawRectangle(ref r, sender: c);
                                        }
                                    }
                                }

                                c.BackRectangle = new SizedRectangle(c.WinLeft, c.WinTop, c.WinWidth, c.WinHeight);
                            }
                        }
                    }
                }
            }

            DirtyRectangle d;
            if (!IsDrawn)
            {
                if (surface.Lock(ref lockedSurface) >= 0)
                {
                    Draw(lockedSurface);
                    if (DrawOverlay && _overlayPriority > 0)
                    {
                        SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                        _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, area: rectangle, ref lockedSurface);
                    }

                    _ = surface.Unlock();
                }
            }
            else
            {
                if (surface.Lock(ref lockedSurface) >= 0)
                {
                    for (int j = 0, last = DrawCount - 1; j <= last; j++)
                    {
                        d = DrawList[j];
                        if (WinLeft > d.Rectangle.Left)
                        {
                            Rect rectangle3 = d.Rectangle;
                            rectangle3.Left = WinLeft;
                            d.Rectangle = rectangle3;
                        }

                        if (WinTop > d.Rectangle.Top)
                        {
                            Rect rectangle4 = d.Rectangle;
                            rectangle4.Top = WinTop;
                            d.Rectangle = rectangle4;
                        }

                        Rect rectangle5 = d.Rectangle;
                        rectangle5.Right += 1;
                        d.Rectangle = rectangle5;
                        Rect rectangle6 = d.Rectangle;
                        rectangle6.Bottom += 1;
                        d.Rectangle = rectangle6;
                        if (WinLeft + WinWidth < d.Rectangle.Right)
                        {
                            Rect rectangle7 = d.Rectangle;
                            rectangle7.Right = WinLeft + WinWidth;
                            d.Rectangle = rectangle7;
                        }

                        if (WinTop + WinHeight < d.Rectangle.Bottom)
                        {
                            Rect rectangle8 = d.Rectangle;
                            rectangle8.Bottom = WinTop + WinHeight;
                            d.Rectangle = rectangle8;
                        }

                        Rect rectangle = new(d.Rectangle.Left, d.Rectangle.Top, d.Rectangle.Right, d.Rectangle.Bottom);
                        lockedSurface.ClipRectangle = rectangle;
                        Draw(lockedSurface);
                        if (DrawOverlay && _overlayPriority > 0)
                        {
                            SizedRectangle rectangle2 = new(WinLeft, WinTop, WinWidth, WinHeight);
                            _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, area: rectangle2, ref lockedSurface);
                        }

                        Rect rectangle9 = d.Rectangle;
                        InvalidateRectangle(0, rectangle: ref rectangle9, sender: null);
                        d.Rectangle = rectangle9;
                    }

                    DrawCount = 0;
                    _ = surface.Unlock();
                }
            }

            for (int i = 0, last = Children.Count - 1; i <= last; i++)
            {
                // The child list is UIComponent. The manager shows windows, and the cast stays.
                c = (WindowControl?)Children[i];
                c.IsHidden = false;
                if (c.Valid && c.Visible)
                {
                    bool wasValid = c.SurfaceValid;
                    if (!c.IsDrawn)
                    {
                        wasValid = false;
                    }

                    c.DrawSurface(surface);
                    if (!wasValid || !c.SurfaceValid)
                    {
                        r = new Rect(c.WinLeft, c.WinTop, c.WinLeft + c.WinWidth, c.WinTop + c.WinHeight);
                        AddBlitRectangle(ref r, sender: c);
                    }

                    c.IsDrawn = true;
                    c.SurfaceValid = true;
                    for (int j = 0, lastJ = BlitCount - 1; j <= lastJ; j++)
                    {
                        d = BlitList[j];
                        Rect rectangle10 = d.Rectangle;
                        InvalidateRectangle(i + 1, rectangle: ref rectangle10, sender: null);
                        d.Rectangle = rectangle10;
                    }

                    if (DrawOverlay && c.Priority < _overlayPriority)
                    {
                        if (surface.Lock(ref lockedSurface) >= 0)
                        {
                            for (int j = 0, lastJ = BlitCount - 1; j <= lastJ; j++)
                            {
                                d = BlitList[j];
                                if (c.WinLeft > d.Rectangle.Left)
                                {
                                    Rect rectangle11 = d.Rectangle;
                                    rectangle11.Left = c.WinLeft;
                                    d.Rectangle = rectangle11;
                                }

                                if (c.WinTop > d.Rectangle.Top)
                                {
                                    Rect rectangle12 = d.Rectangle;
                                    rectangle12.Top = c.WinTop;
                                    d.Rectangle = rectangle12;
                                }

                                Rect rectangle13 = d.Rectangle;
                                rectangle13.Right += 1;
                                d.Rectangle = rectangle13;
                                Rect rectangle14 = d.Rectangle;
                                rectangle14.Bottom += 1;
                                d.Rectangle = rectangle14;
                                if (c.WinLeft + c.WinWidth < d.Rectangle.Right)
                                {
                                    Rect rectangle15 = d.Rectangle;
                                    rectangle15.Right = c.WinLeft + c.WinWidth;
                                    d.Rectangle = rectangle15;
                                }

                                if (c.WinTop + c.WinHeight < d.Rectangle.Bottom)
                                {
                                    Rect rectangle16 = d.Rectangle;
                                    rectangle16.Bottom = c.WinTop + c.WinHeight;
                                    d.Rectangle = rectangle16;
                                }

                                Rect rectangle = new(d.Rectangle.Left, d.Rectangle.Top, d.Rectangle.Right, d.Rectangle.Bottom);
                                lockedSurface.ClipRectangle = rectangle;
                                SizedRectangle rectangle2 = new(WinLeft, WinTop, WinWidth, WinHeight);
                                _foregroundTexture.DrawTiled(_imageLibraryIndexTFore, area: rectangle2, ref lockedSurface);
                            }

                            _ = surface.Unlock();
                        }
                    }

                    BlitCount = 0;
                }
            }

            IsDrawn = true;
            ClearDrawList();
            ClearBlitList();
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during DrawSurface", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AOWWinManager.pas:157
    public override void Draw(LockedSurface lockedSurface)
    {
        if (_overlayPriority > _newOverlayPriority)
        {
            foreach (UIComponent? c in Children)
            {
                if (c.Priority >= _newOverlayPriority && c.Priority < _overlayPriority && c.Visible)
                {
                    c.IsDrawn = false;
                    c.SurfaceValid = false;
                }
            }
        }

        _overlayPriority = _newOverlayPriority;
        SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
        _backgroundTexture.DrawTiled(_imageLibraryIndexTBack, area: rectangle, ref lockedSurface);
    }

    // AOWWinManager.pas:449
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int x, int y)
    {
        Handled = false;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            UIComponent? c = Children[i];
            if (c.Priority >= _overlayPriority)
            {
                bool redraw = c.Redraw;
                c.Handled = false;
                if (!Handled || redraw)
                {
                    c.CheckMouseDown(button, shift, x, y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }
        }

        RaiseMouseDown(this, button, shift, x, y);
    }

    // AOWWinManager.pas:473
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            UIComponent? c = Children[i];
            if (c.Priority >= _overlayPriority)
            {
                if (!Handled || c.Redraw)
                {
                    c.CheckMouseUp(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }
        }

        RaiseMouseUp(this, button, shift, X, Y);
    }

    // AOWWinManager.pas:494
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        Handled = false;
        _oldCursor = base.Cursor;
        base.Cursor = _defaultCursor;
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

        for (int i = Children.Count - 1; i >= 0; i--)
        {
            // The child list is UIComponent. The manager shows windows, and the cast stays.
            WindowControl? c = (WindowControl?)Children[i];
            if (c.Priority >= _overlayPriority)
            {
                if (!Handled || c.Redraw)
                {
                    c.CheckMouseMove(shift, X, Y);
                    if (c.Handled)
                    {
                        if (!Handled)
                        {
                            base.Cursor = c.Cursor;
                            if (!c.MouseInside)
                            {
                                c.MouseInside = true;
                                c.RaiseEnter(c);
                            }
                        }
                        else
                        {
                            if (c.MouseInside)
                            {
                                c.MouseInside = false;
                                c.RaiseExit(c);
                            }
                        }

                        Handled = true;
                    }
                    else
                    {
                        if (c.MouseInside)
                        {
                            c.MouseInside = false;
                            c.RaiseExit(c);
                        }
                    }
                }
                else
                {
                    if (c.MouseInside)
                    {
                        c.MouseInside = false;
                        c.RaiseExit(c);
                    }
                }
            }
        }

        if (base.Cursor != _oldCursor)
        {
            CursorChanged?.Invoke(this);
        }

        RaiseMouseMove(this, shift, X, Y);
    }

    // AOWWinManager.pas:558
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            UIComponent? c = Children[i];
            if (c.Priority >= _overlayPriority)
            {
                c.CheckKeyPress(ref key);
                if (c.Handled)
                {
                    Handled = true;
                }
            }
        }

        RaiseKeyPress(this, ref key);
    }

    // AOWWinManager.pas:578
    public override void CheckKeyDown(ref ushort key, ShiftStates shift)
    {
        Handled = false;
        for (int i = Children.Count - 1; i >= 0; i--)
        {
            UIComponent? c = Children[i];
            if (!Handled && c.Priority >= _overlayPriority && c.Visible)
            {
                c.CheckKeyDown(ref key, shift);
                if (c.Handled)
                {
                    Handled = true;
                }
            }
        }

        if (!Handled)
        {
            RaiseKeyDown(this, ref key, shift);
        }
    }

    // AOWWinManager.pas:102
    public void SetILTB(InterfaceImageLibrary? value)
    {
        if (value != _backgroundTexture)
        {
            Valid = false;
            _backgroundTexture = value;
        }
    }

    // AOWWinManager.pas:111
    public void SetILTF(InterfaceImageLibrary? value)
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
            IsDrawn = false;
        }
    }

    // AOWWinManager.pas:147
    public void SetCursor(int value)
    {
        if (value != base.Cursor)
        {
            base.Cursor = value;
            CursorChanged?.Invoke(this);
        }
    }

    [Published]
    public override int Cursor { get => base.Cursor; set => SetCursor(value); }

    [Published]
    public InterfaceImageLibrary? BackgroundTexture { get => _backgroundTexture; set => SetILTB(value); }

    [Published]
    public InterfaceImageLibrary? ForegroundTexture { get => _foregroundTexture; set => SetILTF(value); }

    [Published]
    public int ImageLibraryIndexTBack { get => _imageLibraryIndexTBack; set => SetImageLibraryIndex(value); }

    [Published]
    public int ImageLibraryIndexTFore { get => _imageLibraryIndexTFore; set => SetILFIndex(value); }

    [Published]
    public int OverlayPriority { get => _overlayPriority; set => SetOverPriority(value); }

    internal void SetOverlayPriorityImmediately(int value) => _overlayPriority = value;

    [Published]
    public int BorderLeft { get; set; }

    [Published]
    public int BorderRight { get; set; }

    [Published]
    public int BorderTop { get; set; }

    [Published]
    public int BorderBottom { get; set; }

    [Published]
    public bool DrawOverlay { get; set; }

    public new event NotifyEvent<WindowManager> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<WindowManager> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<WindowManager> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<WindowManager> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<WindowManager> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<WindowManager> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<WindowManager> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<WindowManager> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<WindowManager> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, WindowManager>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, WindowManager>(this, slot: nameof(Updated), value: value);
    }
}
