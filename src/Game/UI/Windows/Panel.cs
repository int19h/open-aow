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
    private int _savedLeft;
    private int _savedTop;
    private TabPanel _tabPanel;
    private BaseWindow _window;

    // AoWPanel.pas:63
    public Panel(Component owner) : base(owner)
    {
        CaptureInput = true;
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
        if (!Visible && _tabPanel != null && _tabPanel.Selected == _ownerItem)
        {
            SetVisibleWithoutUpdate(true);
        }

        switch (WindowStyle)
        {
            case Style.Normal:
            case Style.Hollow:
                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > WinWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > WinHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                if (_window.WinWidth < WinLeft + WinWidth)
                {
                    SetWinLeftWithoutInvalidate(_window.WinWidth - WinWidth);
                }

                if (WinLeft < 0)
                {
                    SetWinLeftWithoutInvalidate(0);
                    if (_window.WinWidth < WinLeft + WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_window.WinWidth - WinLeft);
                    }
                }

                if (_window.WinHeight < WinTop + WinHeight)
                {
                    SetWinTopWithoutInvalidate(_window.WinHeight - WinHeight);
                }

                if (WinTop < 0)
                {
                    SetWinTopWithoutInvalidate(0);
                    if (_window.WinHeight < WinTop + WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_window.WinHeight - WinTop);
                    }
                }

                break;
            case Style.Borderless:
            case Style.Blank:
                if (_window.WinWidth < WinLeft + WinWidth)
                {
                    SetWinLeftWithoutInvalidate(_window.WinWidth - WinWidth);
                }

                if (WinLeft < 0)
                {
                    SetWinLeftWithoutInvalidate(0);
                    if (_window.WinWidth < WinLeft + WinWidth)
                    {
                        SetWinWidthWithoutInvalidate(_window.WinWidth - WinLeft);
                    }
                }

                if (_window.WinHeight < WinTop + WinHeight)
                {
                    SetWinTopWithoutInvalidate(_window.WinHeight - WinHeight);
                }

                if (WinTop < 0)
                {
                    SetWinTopWithoutInvalidate(0);
                    if (_window.WinHeight < WinTop + WinHeight)
                    {
                        SetWinHeightWithoutInvalidate(_window.WinHeight - WinTop);
                    }
                }

                break;
        }

        if (_minWidth > 0 && _minWidth > WinWidth)
        {
            SetWinWidthWithoutInvalidate(_minWidth);
        }

        if (_minHeight > 0 && _minHeight > WinHeight)
        {
            SetWinHeightWithoutInvalidate(_minHeight);
        }

        if (_window._windowManager != null)
        {
            _windowManager = _window._windowManager;
        }

        SurfaceValid = false;
        if (WindowStyle == Style.Blank)
        {
            Masked = true;
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

                _savedLeft = WinLeft;
                _savedTop = WinTop;
                if (_window is Panel)
                {
                    SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                    SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                }

                for (int I = 0, last = Children.Count - 1; I <= last; I++)
                {
                    Children[I].Setup();
                }

                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
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
        if (Visible)
        {
            _savedLeft = WinLeft;
            _savedTop = WinTop;
            if (_window is Panel)
            {
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
            }

            if (_onUpdate != null)
            {
                _onUpdate(this);
            }

            for (int I = 0, last = Children.Count - 1; I <= last; I++)
            {
                Children[I].Update();
            }

            SetWinLeftWithoutInvalidate(_savedLeft);
            SetWinTopWithoutInvalidate(_savedTop);
            if (OldRectangle.Right > 0 && OldRectangle.Bottom > 0 && (OldRectangle.Left != WinLeft || OldRectangle.Top != WinTop || OldRectangle.Right != WinWidth || OldRectangle.Bottom != WinHeight))
            {
                if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                {
                    R = new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Right - 1, OldRectangle.Top + OldRectangle.Bottom - 1);
                }
                else
                {
                    R = new Rect(OwnerComponent.WinLeft + OldRectangle.Left, OwnerComponent.WinTop + OldRectangle.Top, OwnerComponent.WinLeft + OldRectangle.Left + OldRectangle.Right - 1, OwnerComponent.WinTop + OldRectangle.Top + OldRectangle.Bottom - 1);
                }

                if (Masked)
                {
                    OwnerComponent.AddDrawRectangle(ref R, this);
                }
                else
                {
                    OwnerComponent.AddBlitRectangle(ref R, this);
                }
            }

            OldRectangle.Left = WinLeft;
            OldRectangle.Top = WinTop;
            OldRectangle.Right = WinWidth;
            OldRectangle.Bottom = WinHeight;
        }
        else
        {
            if (Hidden)
            {
                if (OwnerComponent.OwnerComponent.OwnerComponent == null)
                {
                    R = new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1);
                }
                else
                {
                    R = new Rect(OwnerComponent.WinLeft + WinLeft, OwnerComponent.WinTop + WinTop, OwnerComponent.WinLeft + WinLeft + WinWidth - 1, OwnerComponent.WinTop + WinTop + WinHeight - 1);
                }

                OwnerComponent.AddDrawRectangle(ref R, this);
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
        if (Valid && Visible && WindowStyle != Style.Blank)
        {
            AddDrawRectangle(ref rectangle, sender);
        }

        oldLeft = WinLeft;
        oldTop = WinTop;
        SetWinLeftWithoutInvalidate(WinLeft + (OwnerComponent.WinLeft));
        SetWinTopWithoutInvalidate(WinTop + (OwnerComponent.WinTop));
        for (int I = index, last = Children.Count - 1; I <= last; I++)
        {
            C = Children[I];
            if (C.Visible)
            {
                if (((WinLeft + C.WinLeft >= rectangle.Left && WinLeft + C.WinLeft <= rectangle.Right) || (WinLeft + C.WinLeft <= rectangle.Left && rectangle.Left <= WinLeft + C.WinLeft + C.WinWidth)) && ((WinTop + C.WinTop >= rectangle.Top && WinTop + C.WinTop <= rectangle.Bottom) || (WinTop + C.WinTop <= rectangle.Top && rectangle.Top <= WinTop + C.WinTop + C.WinHeight)) && C != sender)
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
                            if (C.Masked)
                            {
                                R = new Rect(WinLeft + C.WinLeft, WinTop + C.WinTop, WinLeft + C.WinLeft + C.WinWidth, WinTop + C.WinTop + C.WinHeight);
                                AddDrawRectangle(ref R, this);
                            }
                        }
                    }
                }
            }
        }

        if (OwnerComponent != sender)
        {
            var indexOf = OwnerComponent.Children.IndexOf(this);
            OwnerComponent.InvalidateRectangle(indexOf + 1, ref rectangle, this);
        }

        SetWinLeftWithoutInvalidate(oldLeft);
        SetWinTopWithoutInvalidate(oldTop);
    }

    // AoWPanel.pas:82
    public override void AddBlitRectangle(ref Rect rectangle, UIComponent sender)
    {
        OwnerComponent.AddBlitRectangle(ref rectangle, this);
    }

    // AoWPanel.pas:87
    public override void AddDrawRectangle(ref Rect rectangle, UIComponent sender)
    {
        if (Masked)
        {
            OwnerComponent.AddDrawRectangle(ref rectangle, this);
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
        _window = (BaseWindow)OwnerComponent;
    }

    // AoWPanel.pas:191
    // { SetVisible: showing sets the back rectangle and marks the panel hidden (original bug: FHidden
    //   set instead of cleared) }
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (!Visible)
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
                    BackRectangle.Right = WinWidth;
                    BackRectangle.Bottom = WinHeight;
                    BackRectangle.Top = WinTop;
                    BackRectangle.Left = WinLeft;
                    Hidden = true;
                }

                if (_onShow != null)
                {
                    _onShow(this);
                }
            }
        }

        if (Visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            Drawn = false;
            SetPriority(Priority);
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
                        var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        var rectangle3 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        var rectangle4 = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                        _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle4, ref lockedSurface);
                        break;
                }

                blitRectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface.ClipRectangle.Right, lockedSurface.ClipRectangle.Bottom);
                OwnerComponent.AddBlitRectangle(ref blitRectangle, this);
            }
        }

        try
        {
            if (Valid && Visible)
            {
                _savedLeft = WinLeft;
                _savedTop = WinTop;
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                if (WinLeft < lockedSurface.ClipRectangle.Right && WinLeft > lockedSurface.ClipRectangle.Left)
                {
                    lockedSurface.ClipRectangle.Left = WinLeft;
                }

                if (WinTop < lockedSurface.ClipRectangle.Bottom && WinTop > lockedSurface.ClipRectangle.Top)
                {
                    lockedSurface.ClipRectangle.Top = WinTop;
                }

                if (WinLeft + WinWidth > lockedSurface.ClipRectangle.Left && WinLeft + WinWidth < lockedSurface.ClipRectangle.Right)
                {
                    lockedSurface.ClipRectangle.Right = WinLeft + WinWidth;
                }

                if (WinTop + WinHeight > lockedSurface.ClipRectangle.Top && WinTop + WinHeight < lockedSurface.ClipRectangle.Bottom)
                {
                    lockedSurface.ClipRectangle.Bottom = WinTop + WinHeight;
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
                        SetWinLeftWithoutInvalidate(_savedLeft);
                        SetWinTopWithoutInvalidate(_savedTop);
                        InvalidateRectangle(0, ref D.Rectangle, D.Owner);
                        SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                        SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
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
                    C = Children[I];
                    if (C.Valid && C.Visible && (!Drawn || !C.Drawn))
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
                            blitRectangle = new Rect(WinLeft + C.WinLeft, WinTop + C.WinTop, WinLeft + C.WinLeft + C.WinWidth, WinTop + C.WinTop + C.WinHeight);
                            OwnerComponent.AddBlitRectangle(ref blitRectangle, this);
                            SetWinLeftWithoutInvalidate(_savedLeft);
                            SetWinTopWithoutInvalidate(_savedTop);
                            blitRectangle = new Rect(_window.WinLeft + WinLeft + C.WinLeft, _window.WinTop + WinTop + C.WinTop, _window.WinLeft + WinLeft + C.WinLeft + C.WinWidth - 1, _window.WinTop + WinTop + C.WinTop + C.WinHeight - 1);
                            InvalidateRectangle(I + 1, ref blitRectangle, C);
                            SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                            SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                        }
                    }
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }

                Drawn = SurfaceValid;
                lockedSurface.ClipRectangle = oldClip;
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
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
        if (Visible == true)
        {
            _savedLeft = WinLeft;
            _savedTop = WinTop;
            if (_window != null)
            {
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw) && C.Visible == true && C.CaptureInput)
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

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                Redraw = true;
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
                if (_onMouseDown != null)
                {
                    _onMouseDown(this, button, shift, X, Y);
                }
            }
            else
            {
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
            }
        }
    }

    // AoWPanel.pas:558
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible == true)
        {
            _savedLeft = WinLeft;
            _savedTop = WinTop;
            if (_window != null)
            {
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true) && C.Visible == true && C.CaptureInput)
                {
                    C.CheckMouseUp(button, shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
                if (_onMouseUp != null)
                {
                    _onMouseUp(this, button, shift, X, Y);
                }
            }
            else
            {
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
            }
        }
    }

    // AoWPanel.pas:600
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible == true)
        {
            _savedLeft = WinLeft;
            _savedTop = WinTop;
            if (_window != null)
            {
                SetWinLeftWithoutInvalidate(WinLeft + (_window.WinLeft));
                SetWinTopWithoutInvalidate(WinTop + (_window.WinTop));
                Handled = _window.Handled;
            }

            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = Children[I];
                C.Handled = false;
                if ((!Handled || C.Redraw == true || C.ToolTipCount > 0) && C.Visible == true && C.CaptureInput)
                {
                    C.CheckMouseMove(shift, X, Y);
                }

                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (X > WinLeft - 1 && X < WinLeft + WinWidth && Y > WinTop - 1 && Y < WinTop + WinHeight)
            {
                Handled = true;
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
                if (_onMouseMove != null)
                {
                    _onMouseMove(this, shift, X, Y);
                }
            }
            else
            {
                SetWinLeftWithoutInvalidate(_savedLeft);
                SetWinTopWithoutInvalidate(_savedTop);
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
                    if (P.Priority > Priority)
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
