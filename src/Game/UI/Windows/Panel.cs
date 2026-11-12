// Translated from AoWPanel.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// { The AoWPanel unit of aowInt.dpl: TAOWPanel, a window inside a window (Window), drawn into
//   its owner's surface. Its controls are positioned relative to it: while it updates, draws or
//   passes input on, it moves itself by the window's position and restores its own position
//   afterwards. A panel can belong to a tab of a TAOWTabPanel (TabPanel and OwnerItem): it is then
//   visible only while its tab is selected. }
// Fields: FOrgLeft, FOrgTop (the panel's own position, saved while it is moved by the window's
// position), _tabPanel, _window
public partial class Panel : BaseWindow
{
    public int SavedLeft;
    public int SavedTop;
    public TabPanel _tabPanel;
    public BaseWindow _window;

    // AoWPanel.pas:63
    public Panel(Component owner) : base(owner)
    {
        _captureInput = true;
    }

    // AoWPanel.pas:69
    public override void Dispose()
    {
        if (_tabPanel != null)
        {
            var indexOf = _tabPanel.Panels.IndexOf(this);
            _tabPanel.Panels.RemoveAt(indexOf);
        }

        base.Dispose();
    }

    // AoWPanel.pas:235
    // { SetSize: at least as large as the frame corners, kept inside the window, at least MinWidth by
    //   MinHeight; the panel uses the window's window manager }
    public override void SetSize()
    {
        if (!_visible && _tabPanel != null && _tabPanel._selected == _ownerItem)
        {
            _visible = true;
        }

        switch (WindowStyle)
        {
            case Style.Normal:
            case Style.Hollow:
                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > _winWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    _winWidth = imageLibraryFrame2.Width + imageLibraryFrame3.Width;
                }

                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > _winHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
                }

                if (_window._winWidth < _winLeft + _winWidth)
                {
                    _winLeft = _window._winWidth - _winWidth;
                }

                if (_winLeft < 0)
                {
                    _winLeft = 0;
                    if (_window._winWidth < _winLeft + _winWidth)
                    {
                        _winWidth = _window._winWidth - _winLeft;
                    }
                }

                if (_window._winHeight < _winTop + _winHeight)
                {
                    _winTop = _window._winHeight - _winHeight;
                }

                if (_winTop < 0)
                {
                    _winTop = 0;
                    if (_window._winHeight < _winTop + _winHeight)
                    {
                        _winHeight = _window._winHeight - _winTop;
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (_window._winWidth < _winLeft + _winWidth)
                {
                    _winLeft = _window._winWidth - _winWidth;
                }

                if (_winLeft < 0)
                {
                    _winLeft = 0;
                    if (_window._winWidth < _winLeft + _winWidth)
                    {
                        _winWidth = _window._winWidth - _winLeft;
                    }
                }

                if (_window._winHeight < _winTop + _winHeight)
                {
                    _winTop = _window._winHeight - _winHeight;
                }

                if (_winTop < 0)
                {
                    _winTop = 0;
                    if (_window._winHeight < _winTop + _winHeight)
                    {
                        _winHeight = _window._winHeight - _winTop;
                    }
                }

                break;
        }

        if (_minWidth > 0 && _minWidth > _winWidth)
        {
            _winWidth = _minWidth;
        }

        if (_minHeight > 0 && _minHeight > _winHeight)
        {
            _winHeight = _minHeight;
        }

        if (_window._windowManager != null)
        {
            _windowManager = _window._windowManager;
        }

        SurfaceValid = false;
        if (WindowStyle == Style.Blank)
        {
            _masked = true;
        }

        base.SetSize();
    }

    // AoWPanel.pas:297
    // { Setup: like TAoWComponent.Setup, with the controls set up while the panel is moved by the
    //   position of a panel it sits in }
    public override void Setup()
    {
        try
        {
            if (!Valid)
            {
                Validate();
            }

            if (Valid)
            {
                if (!Sized)
                {
                    SetSize();
                }

                if (!Aligned)
                {
                    ReAlign();
                }

                if (!Sized)
                {
                    SetSize();
                }

                SavedLeft = _winLeft;
                SavedTop = _winTop;
                if (_window is Panel)
                {
                    _winLeft += _window._winLeft;
                    _winTop += _window._winTop;
                }

                for (int I = 0, last = Children.Count - 1; I <= last; I++)
                {
                    ((UIComponent)Children[I]).Setup();
                }

                _winLeft = SavedLeft;
                _winTop = SavedTop;
                BlitCount = 0;
                DrawCount = 0;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Setup", "Error - " + Name, 0);
        }
    }

    // AoWPanel.pas:333
    // { Update: the controls are updated with the panel moved by its window's position; when the
    //   panel moved or changed size, its old rectangle is redrawn in the owner }
    public override void Update()
    {
        Rect R = default;
        if (_visible)
        {
            SavedLeft = _winLeft;
            SavedTop = _winTop;
            if (_window is Panel)
            {
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
            }

            if (_onUpdate != null)
            {
                _onUpdate(this);
            }

            for (int I = 0, last = Children.Count - 1; I <= last; I++)
            {
                ((UIComponent)Children[I]).Update();
            }

            _winLeft = SavedLeft;
            _winTop = SavedTop;
            if (OldRectangle.Right > 0 && OldRectangle.Bottom > 0 && (OldRectangle.Left != _winLeft || OldRectangle.Top != _winTop || OldRectangle.Right != _winWidth || OldRectangle.Bottom != _winHeight))
            {
                if (_ownerComponent._ownerComponent._ownerComponent == null)
                {
                    R = new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Right - 1, OldRectangle.Top + OldRectangle.Bottom - 1);
                }
                else
                {
                    R = new Rect(_ownerComponent._winLeft + OldRectangle.Left, _ownerComponent._winTop + OldRectangle.Top, _ownerComponent._winLeft + OldRectangle.Left + OldRectangle.Right - 1, _ownerComponent._winTop + OldRectangle.Top + OldRectangle.Bottom - 1);
                }

                if (_masked)
                {
                    _ownerComponent.AddDrawRectangle(ref R, this);
                }
                else
                {
                    _ownerComponent.AddBlitRectangle(ref R, this);
                }
            }

            OldRectangle.Left = _winLeft;
            OldRectangle.Top = _winTop;
            OldRectangle.Right = _winWidth;
            OldRectangle.Bottom = _winHeight;
        }
        else
        {
            if (Hidden)
            {
                if (_ownerComponent._ownerComponent._ownerComponent == null)
                {
                    R = new Rect(_winLeft, _winTop, _winLeft + _winWidth - 1, _winTop + _winHeight - 1);
                }
                else
                {
                    R = new Rect(_ownerComponent._winLeft + _winLeft, _ownerComponent._winTop + _winTop, _ownerComponent._winLeft + _winLeft + _winWidth - 1, _ownerComponent._winTop + _winTop + _winHeight - 1);
                }

                _ownerComponent.AddDrawRectangle(ref R, this);
                Hidden = false;
            }
        }

        SurfaceValid = Drawn;
        Drawn = false;
        Redraw = true;
    }

    // AoWPanel.pas:97
    // { InvalidateRect: the controls from Index on that ARect touches are redrawn (windows are asked to
    //   invalidate the rectangle themselves), then the windows above the panel in its owner }
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        int oldLeft;
        int oldTop;
        UIComponent C;
        Rect R = default;
        if (Valid && _visible && WindowStyle != Style.Blank)
        {
            AddDrawRectangle(ref rectangle, sender);
        }

        oldLeft = _winLeft;
        oldTop = _winTop;
        _winLeft += _ownerComponent._winLeft;
        _winTop += _ownerComponent._winTop;
        for (int I = index, last = Children.Count - 1; I <= last; I++)
        {
            C = (UIComponent)Children[I];
            if (C._visible)
            {
                if (((_winLeft + C._winLeft >= rectangle.Left && _winLeft + C._winLeft <= rectangle.Right) || (_winLeft + C._winLeft <= rectangle.Left && rectangle.Left <= _winLeft + C._winLeft + C._winWidth)) && ((_winTop + C._winTop >= rectangle.Top && _winTop + C._winTop <= rectangle.Bottom) || (_winTop + C._winTop <= rectangle.Top && rectangle.Top <= _winTop + C._winTop + C._winHeight)) && C != sender)
                {
                    if (C is BaseWindow)
                    {
                        C.InvalidateRectangle(0, ref rectangle, this);
                    }
                    else
                    {
                        if (C.Drawn)
                        {
                            C.Drawn = false;
                            if (C._masked)
                            {
                                R = new Rect(_winLeft + C._winLeft, _winTop + C._winTop, _winLeft + C._winLeft + C._winWidth, _winTop + C._winTop + C._winHeight);
                                AddDrawRectangle(ref R, this);
                            }
                        }
                    }
                }
            }
        }

        if (_ownerComponent != sender)
        {
            var indexOf = _ownerComponent.Children.IndexOf(this);
            _ownerComponent.InvalidateRectangle(indexOf + 1, ref rectangle, this);
        }

        _winLeft = oldLeft;
        _winTop = oldTop;
    }

    // AoWPanel.pas:82
    public override void AddBlitRectangle(ref Rect rectangle, UIComponent sender)
    {
        _ownerComponent.AddBlitRectangle(ref rectangle, this);
    }

    // AoWPanel.pas:87
    public override void AddDrawRectangle(ref Rect rectangle, UIComponent sender)
    {
        if (_masked)
        {
            _ownerComponent.AddDrawRectangle(ref rectangle, this);
        }

        if (WindowStyle != Style.Blank)
        {
            base.AddDrawRectangle(ref rectangle, sender);
        }
    }

    // AoWPanel.pas:76
    public override void SetOwner(UIComponent value)
    {
        base.SetOwner(value);
        _window = (BaseWindow)_ownerComponent;
    }

    // AoWPanel.pas:191
    // { SetVisible: showing sets the back rectangle and marks the panel hidden (original bug: FHidden
    //   set instead of cleared) }
    public override void SetVisible(bool value)
    {
        if (value != _visible)
        {
            _visible = value;
            if (!_visible)
            {
                Hidden = true;
                if (_onHide != null)
                {
                    _onHide(this);
                }
            }
            else
            {
                Drawn = false;
                if (!Hidden)
                {
                    BackRectangle.Right = _winWidth;
                    BackRectangle.Bottom = _winHeight;
                    BackRectangle.Top = _winTop;
                    BackRectangle.Left = _winLeft;
                    Hidden = true;
                }

                if (_onShow != null)
                {
                    _onShow(this);
                }
            }
        }

        if (_visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            Drawn = false;
            SetPriority(_priority);
        }
    }

    // AoWPanel.pas:391
    // { Draw: the background (redrawn only in the dirty rectangles once the surface is valid), then
    //   the controls that need it; the rectangles of redrawn controls are passed on to the controls
    //   above them and blitted }
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect blitRectangle = default;
        Rect oldClip = default;
        UIComponent C = default;
        DirtyRectangle D = default;
        void DrawBack()
        {
            if (WindowStyle != Style.Blank)
            {
                switch (WindowStyle)
                {
                    case Style.Normal:
                        var rectangle = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(_winLeft + imageLibraryFrame4.Width, _winTop + imageLibraryFrame3.Height, _winWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        var rectangle3 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        var rectangle4 = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle4, ref lockedSurface);
                        break;
                }

                blitRectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface.ClipRectangle.Right, lockedSurface.ClipRectangle.Bottom);
                _ownerComponent.AddBlitRectangle(ref blitRectangle, this);
            }
        }

        try
        {
            if (Valid && _visible)
            {
                SavedLeft = _winLeft;
                SavedTop = _winTop;
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
                if (_winLeft < lockedSurface.ClipRectangle.Right && _winLeft > lockedSurface.ClipRectangle.Left)
                {
                    lockedSurface.ClipRectangle.Left = _winLeft;
                }

                if (_winTop < lockedSurface.ClipRectangle.Bottom && _winTop > lockedSurface.ClipRectangle.Top)
                {
                    lockedSurface.ClipRectangle.Top = _winTop;
                }

                if (_winLeft + _winWidth > lockedSurface.ClipRectangle.Left && _winLeft + _winWidth < lockedSurface.ClipRectangle.Right)
                {
                    lockedSurface.ClipRectangle.Right = _winLeft + _winWidth;
                }

                if (_winTop + _winHeight > lockedSurface.ClipRectangle.Top && _winTop + _winHeight < lockedSurface.ClipRectangle.Bottom)
                {
                    lockedSurface.ClipRectangle.Bottom = _winTop + _winHeight;
                }

                if (SurfaceValid)
                {
                    for (int J = 0, last = DrawCount - 1; J <= last; J++)
                    {
                        D = DrawList[J];
                        oldClip = lockedSurface.ClipRectangle;
                        var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right + 1, D.Rectangle.Bottom + 1);
                        lockedSurface.ClipRectangle = rectangle;
                        DrawBack();
                        lockedSurface.ClipRectangle = oldClip;
                        _winLeft = SavedLeft;
                        _winTop = SavedTop;
                        InvalidateRectangle(0, ref D.Rectangle, D.Owner);
                        _winLeft += _window._winLeft;
                        _winTop += _window._winTop;
                    }
                }
                else
                {
                    DrawBack();
                    if (_onDrawBack != null)
                    {
                        _onDrawBack(this, lockedSurface);
                    }
                }

                DrawCount = 0;
                Drawn = SurfaceValid;
                for (int I = 0, lastI = Children.Count - 1; I <= lastI; I++)
                {
                    C = (UIComponent)Children[I];
                    if (C.Valid && C._visible && (!Drawn || !C.Drawn))
                    {
                        C.Drawn = false;
                        if (!Drawn && C is BaseWindow)
                        {
                            C.SurfaceValid = false;
                        }

                        C.Draw(lockedSurface);
                        C.FirstDraw = C.Drawn;
                        C.Drawn = true;
                        for (int J = 0, last = DrawCount - 1; J <= last; J++)
                        {
                            D = DrawList[J];
                            InvalidateRectangle(I + 1, ref D.Rectangle, null);
                        }

                        DrawCount = 0;
                        if (!Drawn || !C.FirstDraw)
                        {
                            blitRectangle = new Rect(_winLeft + C._winLeft, _winTop + C._winTop, _winLeft + C._winLeft + C._winWidth, _winTop + C._winTop + C._winHeight);
                            _ownerComponent.AddBlitRectangle(ref blitRectangle, this);
                            _winLeft = SavedLeft;
                            _winTop = SavedTop;
                            blitRectangle = new Rect(_window._winLeft + _winLeft + C._winLeft, _window._winTop + _winTop + C._winTop, _window._winLeft + _winLeft + C._winLeft + C._winWidth - 1, _window._winTop + _winTop + C._winTop + C._winHeight - 1);
                            InvalidateRectangle(I + 1, ref blitRectangle, C);
                            _winLeft += _window._winLeft;
                            _winTop += _window._winTop;
                        }
                    }
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }

                Drawn = SurfaceValid;
                lockedSurface.ClipRectangle = oldClip;
                _winLeft = SavedLeft;
                _winTop = SavedTop;
                SurfaceValid = true;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AoWPanel.pas:513
    // { CheckMouseDown: the controls first (from the top), with the panel moved by its window's
    //   position }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (_visible == true)
        {
            SavedLeft = _winLeft;
            SavedTop = _winTop;
            if (_window != null)
            {
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw) && C._visible == true && C._captureInput)
                {
                    C.CheckMouseDown(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }

                if (C.Handled == true)
                {
                    C.Redraw = true;
                }
            }

            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                Handled = true;
                Redraw = true;
                _winLeft = SavedLeft;
                _winTop = SavedTop;
                if (_onMouseDown != null)
                {
                    _onMouseDown(this, button, shift, X, Y);
                }
            }
            else
            {
                _winLeft = SavedLeft;
                _winTop = SavedTop;
            }
        }
    }

    // AoWPanel.pas:558
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (_visible == true)
        {
            SavedLeft = _winLeft;
            SavedTop = _winTop;
            if (_window != null)
            {
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true) && C._visible == true && C._captureInput)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                Handled = true;
                _winLeft = SavedLeft;
                _winTop = SavedTop;
                if (_onMouseUp != null)
                {
                    _onMouseUp(this, button, shift, X, Y);
                }
            }
            else
            {
                _winLeft = SavedLeft;
                _winTop = SavedTop;
            }
        }
    }

    // AoWPanel.pas:600
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (_visible == true)
        {
            SavedLeft = _winLeft;
            SavedTop = _winTop;
            if (_window != null)
            {
                _winLeft += _window._winLeft;
                _winTop += _window._winTop;
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true || C.ToolTipCount > 0) && C._visible == true && C._captureInput)
                {
                    C.CheckMouseMove(shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > _winLeft - 1 && X < _winLeft + _winWidth && Y > _winTop - 1 && Y < _winTop + _winHeight)
            {
                Handled = true;
                _winLeft = SavedLeft;
                _winTop = SavedTop;
                if (_onMouseMove != null)
                {
                    _onMouseMove(this, shift, X, Y);
                }
            }
            else
            {
                _winLeft = SavedLeft;
                _winTop = SavedTop;
            }
        }
    }

    // AoWPanel.pas:138
    public override void DetachMenuBar()
    {
        _tabPanel = null;
    }

    // AoWPanel.pas:145
    // { SetTabPanel moves the panel from the panel list of its old tab panel to the list of the new
    //   one, in priority order }
    public virtual void SetTabPanel(TabPanel value)
    {
        int I;
        int newIndex;
        Panel P;
        if (value != _tabPanel)
        {
            if (_tabPanel != null)
            {
                I = 0;
                while (I < _tabPanel.Panels.Count)
                {
                    P = _tabPanel.Panels[I];
                    if (P == this)
                    {
                        _tabPanel.Panels.RemoveAt(I);
                        I = _tabPanel.Panels.Count;
                    }

                    I += 1;
                }
            }

            _tabPanel = value;
            if (_tabPanel != null)
            {
                I = 0;
                newIndex = -1;
                while (I < _tabPanel.Panels.Count)
                {
                    P = _tabPanel.Panels[I];
                    if (P._priority > _priority)
                    {
                        newIndex = I;
                        I = _tabPanel.Panels.Count;
                    }

                    I += 1;
                }

                if (newIndex == -1)
                {
                    _tabPanel.Panels.Add(this);
                }
                else
                {
                    _tabPanel.Panels.Insert(newIndex, this);
                }
            }
        }
    }

    // AoWPanel.pas:228
    public void SetWindow(BaseWindow value)
    {
        SetOwner(value);
    }

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public TabPanel TabPanel { get => _tabPanel; set => SetTabPanel(value); }

    [Published]
    public int OwnerItem { get => _ownerItem; set => _ownerItem = value; }

    [Published]
    public int MinWidth { get => _minWidth; set => _minWidth = value; }

    [Published]
    public int MinHeight { get => _minHeight; set => _minHeight = value; }

    public new NotifyEvent<Panel> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Panel> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Panel> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Panel> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Panel> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Panel> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Panel> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Panel> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Panel> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<Panel> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<Panel> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<Panel> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<Panel> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<Panel> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
