#nullable disable
// Translated from AoWPanel.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// { The AoWPanel unit of aowInt.dpl: TAOWPanel, a window inside a window (Window), drawn into
//   its owner's surface. Its controls are positioned relative to it and read its AbsoluteLeft and
//   AbsoluteTop (or WindowRelativeLeft and WindowRelativeTop during Update) so the panel's own
//   coordinates never change. A panel can belong to a tab of a TAOWTabPanel (TabPanel and
//   OwnerItem): it is then visible only while its tab is selected. }
// Fields: _tabPanel, _window.
public sealed partial class Panel : BaseWindow
{
    private TabPanel _tabPanel;
    private BaseWindow _window;

    // AoWPanel.pas:63
    public Panel(Component owner) : base(owner) => CaptureInput = true;

    // AoWPanel.pas:69
    public override void Dispose()
    {
        if (_tabPanel is not null)
        {
            int indexOf = _tabPanel.Panels.IndexOf(this);
            _tabPanel.Panels.RemoveAt(indexOf);
        }

        base.Dispose();
    }

    // AoWPanel.pas:235
    // { SetSize: at least as large as the frame corners, kept inside the window, at least MinWidth by
    //   MinHeight; the panel uses the window's window manager }
    public override void SetSize()
    {
        if (!Visible && _tabPanel is not null && _tabPanel.Selected == OwnerItem)
        {
            SetVisibleWithoutUpdate(true);
        }

        switch (WindowStyle)
        {
            case Style.Normal:
            case Style.Hollow:
                LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame.Width > WinWidth)
                {
                    LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame4.Height > WinHeight)
                {
                    LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                    LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
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
            default:
                break;
        }

        if (MinWidth > 0 && MinWidth > WinWidth)
        {
            SetWinWidthWithoutInvalidate(MinWidth);
        }

        if (MinHeight > 0 && MinHeight > WinHeight)
        {
            SetWinHeightWithoutInvalidate(MinHeight);
        }

        if (_window.WindowManager is not null)
        {
            WindowManager = _window.WindowManager;
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

                for (int i = 0, last = Children.Count - 1; i <= last; i++)
                {
                    Children[i].Setup();
                }

                BlitCount = 0;
                DrawCount = 0;
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Setup", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWPanel.pas:333
    // { Update: when the panel moved or changed size, its old rectangle is redrawn in the owner }
    public override void Update()
    {
        Rect r;
        if (Visible)
        {
            RaiseUpdate(this);

            for (int i = 0, last = Children.Count - 1; i <= last; i++)
            {
                Children[i].Update();
            }

            if (OldRectangle.Width > 0 && OldRectangle.Height > 0 && (OldRectangle.Left != WinLeft || OldRectangle.Top != WinTop || OldRectangle.Width != WinWidth || OldRectangle.Height != WinHeight))
            {
                r = OwnerComponent.OwnerComponent.OwnerComponent is null
                    ? new Rect(OldRectangle.Left, OldRectangle.Top, OldRectangle.Left + OldRectangle.Width - 1, OldRectangle.Top + OldRectangle.Height - 1)
                    : new Rect(OwnerComponent.WindowRelativeLeft + OldRectangle.Left, OwnerComponent.WindowRelativeTop + OldRectangle.Top, OwnerComponent.WindowRelativeLeft + OldRectangle.Left + OldRectangle.Width - 1, OwnerComponent.WindowRelativeTop + OldRectangle.Top + OldRectangle.Height - 1);

                if (Masked)
                {
                    OwnerComponent.AddDrawRectangle(ref r, sender: this);
                }
                else
                {
                    OwnerComponent.AddBlitRectangle(ref r, sender: this);
                }
            }

            OldRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
        }
        else
        {
            if (Hidden)
            {
                r = OwnerComponent.OwnerComponent.OwnerComponent is null
                    ? new Rect(WinLeft, WinTop, WinLeft + WinWidth - 1, WinTop + WinHeight - 1)
                    : new Rect(OwnerComponent.WindowRelativeLeft + WinLeft, OwnerComponent.WindowRelativeTop + WinTop, OwnerComponent.WindowRelativeLeft + WinLeft + WinWidth - 1, OwnerComponent.WindowRelativeTop + WinTop + WinHeight - 1);

                OwnerComponent.AddDrawRectangle(ref r, sender: this);
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
        if (Valid && Visible && WindowStyle != Style.Blank)
        {
            AddDrawRectangle(ref rectangle, sender);
        }

        int absLeft = AbsoluteLeft;
        int absTop = AbsoluteTop;
        for (int i = index, last = Children.Count - 1; i <= last; i++)
        {
            UIComponent c = Children[i];
            if (c.Visible)
            {
                if (((absLeft + c.WinLeft >= rectangle.Left && absLeft + c.WinLeft <= rectangle.Right) || (absLeft + c.WinLeft <= rectangle.Left && rectangle.Left <= absLeft + c.WinLeft + c.WinWidth)) && ((absTop + c.WinTop >= rectangle.Top && absTop + c.WinTop <= rectangle.Bottom) || (absTop + c.WinTop <= rectangle.Top && rectangle.Top <= absTop + c.WinTop + c.WinHeight)) && c != sender)
                {
                    if (c is BaseWindow)
                    {
                        c.InvalidateRectangle(0, ref rectangle, sender: this);
                    }
                    else
                    {
                        if (c.Drawn)
                        {
                            c.Drawn = false;
                            if (c.Masked)
                            {
                                Rect r = new(absLeft + c.WinLeft, absTop + c.WinTop, absLeft + c.WinLeft + c.WinWidth, absTop + c.WinTop + c.WinHeight);
                                AddDrawRectangle(ref r, sender: this);
                            }
                        }
                    }
                }
            }
        }

        if (OwnerComponent != sender)
        {
            int indexOf = OwnerComponent.Children.IndexOf(this);
            OwnerComponent.InvalidateRectangle(indexOf + 1, ref rectangle, sender: this);
        }
    }

    // AoWPanel.pas:82
    public override void AddBlitRectangle(ref Rect rectangle, UIComponent sender) => OwnerComponent.AddBlitRectangle(ref rectangle, sender: this);

    // AoWPanel.pas:87
    public override void AddDrawRectangle(ref Rect rectangle, UIComponent sender)
    {
        if (Masked)
        {
            OwnerComponent.AddDrawRectangle(ref rectangle, sender: this);
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
        // The owner is a UIComponent. This panel sits in a window, and the cast stays.
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
                RaiseHide(this);
            }
            else
            {
                Drawn = false;
                if (!Hidden)
                {
                    BackRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                    Hidden = true;
                }

                RaiseShow(this);
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
        Rect blitRectangle;
        int absLeft = AbsoluteLeft;
        int absTop = AbsoluteTop;
        void DrawBack()
        {
            if (WindowStyle != Style.Blank)
            {
                switch (WindowStyle)
                {
                    case Style.Normal:
                        SizedRectangle rectangle = new(absLeft, absTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                        LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                        LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                        LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(absLeft + imageLibraryFrame4.Width, absTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle2, ref lockedSurface);
                        break;
                    case Style.Hollow:
                        SizedRectangle rectangle3 = new(absLeft, absTop, WinWidth, WinHeight);
                        ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle3, ref lockedSurface);
                        break;
                    case Style.Borderless:
                        SizedRectangle rectangle4 = new(absLeft, absTop, WinWidth, WinHeight);
                        ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle4, ref lockedSurface);
                        break;
                    case Style.Blank:
                    default:
                        break;
                }

                blitRectangle = new Rect(lockedSurface.ClipRectangle.Left, lockedSurface.ClipRectangle.Top, lockedSurface.ClipRectangle.Right, lockedSurface.ClipRectangle.Bottom);
                OwnerComponent.AddBlitRectangle(ref blitRectangle, sender: this);
            }
        }

        try
        {
            if (Valid && Visible)
            {
                if (absLeft < lockedSurface.ClipRectangle.Right && absLeft > lockedSurface.ClipRectangle.Left)
                {
                    Rect clipRectangle = lockedSurface.ClipRectangle;
                    clipRectangle.Left = absLeft;
                    lockedSurface.ClipRectangle = clipRectangle;
                }

                if (absTop < lockedSurface.ClipRectangle.Bottom && absTop > lockedSurface.ClipRectangle.Top)
                {
                    Rect clipRectangle2 = lockedSurface.ClipRectangle;
                    clipRectangle2.Top = absTop;
                    lockedSurface.ClipRectangle = clipRectangle2;
                }

                if (absLeft + WinWidth > lockedSurface.ClipRectangle.Left && absLeft + WinWidth < lockedSurface.ClipRectangle.Right)
                {
                    Rect clipRectangle3 = lockedSurface.ClipRectangle;
                    clipRectangle3.Right = absLeft + WinWidth;
                    lockedSurface.ClipRectangle = clipRectangle3;
                }

                if (absTop + WinHeight > lockedSurface.ClipRectangle.Top && absTop + WinHeight < lockedSurface.ClipRectangle.Bottom)
                {
                    Rect clipRectangle4 = lockedSurface.ClipRectangle;
                    clipRectangle4.Bottom = absTop + WinHeight;
                    lockedSurface.ClipRectangle = clipRectangle4;
                }

                DirtyRectangle d;
                Rect oldClip = default;
                if (SurfaceValid)
                {
                    for (int j = 0, last = DrawCount - 1; j <= last; j++)
                    {
                        d = DrawList[j];
                        oldClip = lockedSurface.ClipRectangle;
                        Rect rectangle = new(d.Rectangle.Left, d.Rectangle.Top, d.Rectangle.Right + 1, d.Rectangle.Bottom + 1);
                        lockedSurface.ClipRectangle = rectangle;
                        DrawBack();
                        lockedSurface.ClipRectangle = oldClip;
                        Rect rectangle5 = d.Rectangle;
                        InvalidateRectangle(0, rectangle: ref rectangle5, sender: d.Owner);
                        d.Rectangle = rectangle5;
                    }
                }
                else
                {
                    DrawBack();
                    RaiseDrawBack(this, surfDesc: lockedSurface);
                }

                DrawCount = 0;
                Drawn = SurfaceValid;
                for (int i = 0, lastI = Children.Count - 1; i <= lastI; i++)
                {
                    UIComponent c = Children[i];
                    if (c.Valid && c.Visible && (!Drawn || !c.Drawn))
                    {
                        c.Drawn = false;
                        if (!Drawn && c is BaseWindow)
                        {
                            c.SurfaceValid = false;
                        }

                        c.Draw(lockedSurface);
                        c.FirstDraw = c.Drawn;
                        c.Drawn = true;
                        for (int j = 0, last = DrawCount - 1; j <= last; j++)
                        {
                            d = DrawList[j];
                            Rect rectangle6 = d.Rectangle;
                            InvalidateRectangle(i + 1, rectangle: ref rectangle6, sender: null);
                            d.Rectangle = rectangle6;
                        }

                        DrawCount = 0;
                        if (!Drawn || !c.FirstDraw)
                        {
                            blitRectangle = new Rect(absLeft + c.WinLeft, absTop + c.WinTop, absLeft + c.WinLeft + c.WinWidth, absTop + c.WinTop + c.WinHeight);
                            OwnerComponent.AddBlitRectangle(ref blitRectangle, sender: this);
                            blitRectangle = new Rect(absLeft + c.WinLeft, absTop + c.WinTop, absLeft + c.WinLeft + c.WinWidth - 1, absTop + c.WinTop + c.WinHeight - 1);
                            InvalidateRectangle(i + 1, rectangle: ref blitRectangle, sender: c);
                        }
                    }
                }

                RaiseDraw(this, surfDesc: lockedSurface);

                Drawn = SurfaceValid;
                lockedSurface.ClipRectangle = oldClip;
                SurfaceValid = true;
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // The panel's rectangle on the screen: its own position moved by its window's position.
    private SizedRectangle AbsoluteArea() => new(AbsoluteLeft, AbsoluteTop, WinWidth, WinHeight);

    // AoWPanel.pas:513
    // { CheckMouseDown: the controls first (from the top), with the panel's absolute origin read
    //   through AbsoluteLeft and AbsoluteTop. }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (Visible)
        {
            SizedRectangle absolute = AbsoluteArea();
            if (_window is not null)
            {
                Handled = _window.Handled;
            }

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseDown(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }

                if (c.Handled)
                {
                    c.Redraw = true;
                }
            }

            if (X > absolute.Left - 1 && X < absolute.Left + WinWidth && Y > absolute.Top - 1 && Y < absolute.Top + WinHeight)
            {
                Handled = true;
                Redraw = true;
                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWPanel.pas:558
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (Visible)
        {
            SizedRectangle absolute = AbsoluteArea();
            if (_window is not null)
            {
                Handled = _window.Handled;
            }

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseUp(button, shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (X > absolute.Left - 1 && X < absolute.Left + WinWidth && Y > absolute.Top - 1 && Y < absolute.Top + WinHeight)
            {
                Handled = true;
                RaiseMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWPanel.pas:600
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (Visible)
        {
            SizedRectangle absolute = AbsoluteArea();
            if (_window is not null)
            {
                Handled = _window.Handled;
            }

            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent c = Children[i];
                c.Handled = false;
                if ((!Handled || c.Redraw || c.ToolTipCount > 0) && c.Visible && c.CaptureInput)
                {
                    c.CheckMouseMove(shift, X, Y);
                }

                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (X > absolute.Left - 1 && X < absolute.Left + WinWidth && Y > absolute.Top - 1 && Y < absolute.Top + WinHeight)
            {
                Handled = true;
                RaiseMouseMove(this, shift, X, Y);
            }
        }
    }

    // AoWPanel.pas:138
    public override void DetachMenuBar() => _tabPanel = null;

    // AoWPanel.pas:145
    // { SetTabPanel moves the panel from the panel list of its old tab panel to the list of the new
    //   one, in priority order }
    public void SetTabPanel(TabPanel value)
    {
        if (value != _tabPanel)
        {
            int i;
            Panel p;
            if (_tabPanel is not null)
            {
                i = 0;
                while (i < _tabPanel.Panels.Count)
                {
                    p = _tabPanel.Panels[i];
                    if (p == this)
                    {
                        _tabPanel.Panels.RemoveAt(i);
                        i = _tabPanel.Panels.Count;
                    }

                    i += 1;
                }
            }

            _tabPanel = value;
            if (_tabPanel is not null)
            {
                i = 0;
                int newIndex = -1;
                while (i < _tabPanel.Panels.Count)
                {
                    p = _tabPanel.Panels[i];
                    if (p.Priority > Priority)
                    {
                        newIndex = i;
                        i = _tabPanel.Panels.Count;
                    }

                    i += 1;
                }

                if (newIndex == -1)
                {
                    _tabPanel.Panels.Add(this);
                }
                else
                {
                    _tabPanel.Panels.Insert(newIndex, item: this);
                }
            }
        }
    }

    // AoWPanel.pas:228
    public void SetWindow(BaseWindow value) => SetOwner(value);

    [Published]
    public BaseWindow Window { get => _window; set => SetWindow(value); }

    [Published]
    public TabPanel TabPanel { get => _tabPanel; set => SetTabPanel(value); }

    public new event NotifyEvent<Panel> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Panel> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Panel> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Panel> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Panel> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Panel> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Panel> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Panel> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Panel> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Panel>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Panel>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<Panel> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, Panel>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, Panel>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<Panel> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, Panel>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, Panel>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<Panel> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, Panel>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, Panel>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<Panel> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, Panel>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, Panel>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<Panel> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, Panel>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, Panel>(this, slot: nameof(OnShow), value: value);
    }
}
