#nullable disable
// Translated from AoWBaseWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// { The AoWBaseWin unit of aowInt.dpl: TAOWBaseWindow, a window of the window manager. It draws
//   itself and its controls into its back surface (DrawSurface) or straight into the display surface
//   (DrawSurfDesc), redrawing only the dirty rectangles, and passes invalidated rectangles on to the
//   windows above it and to its owner. }
// Fields: FBuffer (freed by TAOWGenericWindow.Destroy), FFreeSurfaceOnHide, FWinManager,
// OnShow, OnHide
public partial class BaseWindow : WindowControl
{
    public BaseWindow(Component owner) : base(owner)
    { }

    public object Buffer { get; }

    [Published]
    public bool FreeSurfaceOnHide { get; set; }

    [Published]
    public virtual WindowManager WindowManager { get; set; }

    [Published]
    public event NotifyEvent<BaseWindow> OnShow;

    protected void RaiseShow(BaseWindow sender) => OnShow?.Invoke(sender);

    [Published]
    public event NotifyEvent<BaseWindow> OnHide;

    protected void RaiseHide(BaseWindow sender) => OnHide?.Invoke(sender);

    // AoWBaseWin.pas:56
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        UIComponent c;
        if (index > -1 && Valid && Visible)
        {
            for (int i = index, last = Children.Count - 1; i <= last; i++)
            {
                c = Children[i];
                if (c.Valid && c.Visible && rectangle.Right >= c.WinLeft && c.WinLeft + c.WinWidth >= rectangle.Left && rectangle.Bottom >= c.WinTop && c.WinTop + c.WinHeight >= rectangle.Top && c != sender)
                {
                    if (c is BaseWindow)
                    {
                        c.InvalidateRectangle(0, ref rectangle, this);
                    }
                    else
                    {
                        c.Drawn = false;
                    }
                }
            }

            if (OwnerComponent != sender && OwnerComponent is BaseWindow)
            {
                int indexOf = OwnerComponent.Children.IndexOf(this);
                OwnerComponent.InvalidateRectangle(indexOf + 1, ref rectangle, this);
            }
        }
    }

    // AoWBaseWin.pas:50
    public override void AddBlitRectangle(ref Rect rectangle, UIComponent sender)
    {
        if (DoubleBuffered && SurfaceValid)
        {
            base.AddBlitRectangle(ref rectangle, sender);
        }
    }

    // AoWBaseWin.pas:367
    // { SetVisible: a hidden window marks itself hidden (the window manager redraws the place it
    //   covered) and may free its back surface; a shown window is redrawn from scratch, remembers its
    //   place in FBackRect, and is put back at its place in the window order. }
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (!Visible)
            {
                Hidden = true;
                OnHide?.Invoke(this);

                if (FreeSurfaceOnHide && BackSurface != null)
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }
            else
            {
                if (DoubleBuffered)
                {
                    SurfaceValid = false;
                }
                else
                {
                    Drawn = false;
                }

                if (!Hidden)
                {
                    Rect backRectangle = BackRectangle;
                    backRectangle.Right = WinWidth;
                    BackRectangle = backRectangle;
                    Rect backRectangle2 = BackRectangle;
                    backRectangle2.Bottom = WinHeight;
                    BackRectangle = backRectangle2;
                    Rect backRectangle3 = BackRectangle;
                    backRectangle3.Top = WinTop;
                    BackRectangle = backRectangle3;
                    Rect backRectangle4 = BackRectangle;
                    backRectangle4.Left = WinLeft;
                    BackRectangle = backRectangle4;
                }

                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }

                ClearBlitList();
                ClearDrawList();
                OnShow?.Invoke(this);
            }
        }

        if (Visible)
        {
            SetPriority(Priority);
        }
    }

    // AoWBaseWin.pas:102
    // { DrawSurface draws the window and its controls. A double-buffered window draws into its back
    //   surface (at 0, 0: the position is restored afterwards), redrawing everything when it is not
    //   drawn or only the rectangles of its draw list, then lets the controls draw themselves and
    //   blits the dirty rectangles of its blit list to Surface, telling the window manager about each
    //   one. A window without a back surface draws itself and its controls straight into Surface. }
    public override void DrawSurface(Surface surface)
    {
        int oldLeft;
        int oldTop;
        bool locked;
        LockedSurface lockedSurface = default;
        Rect r = default;
        try
        {
            if (Valid && Visible)
            {
                oldLeft = WinLeft;
                oldTop = WinTop;
                try
                {
                    if (DoubleBuffered)
                    {
                        Drawn = !VerifyBackSurface(surface) && Drawn;
                        SetWinLeftWithoutInvalidate(0);
                        SetWinTopWithoutInvalidate(0);
                        locked = BackSurface.Lock(ref lockedSurface) == 0;
                    }
                    else
                    {
                        locked = surface.Lock(ref lockedSurface) == 0;
                    }
                }
                catch
                {
                    locked = false;
                    _ = MessageBox(0, "Verify Surface Error", "Error - " + Name, 0);
                }

                if (locked)
                {
                    DirtyRectangle d;
                    try
                    {
                        if (!Drawn)
                        {
                            SurfaceValid = false;
                            ClearDrawList();
                        }

                        if (!SurfaceValid)
                        {
                            ClearBlitList();
                        }

                        if (!Drawn)
                        {
                            Draw(lockedSurface);
                        }
                        else
                        {
                            if (Priority >= WindowManager.OverlayPriority)
                            {
                                for (int j = 0, last = DrawCount - 1; j <= last; j++)
                                {
                                    d = DrawList[j];
                                    r = lockedSurface.ClipRectangle;
                                    Rect rectangle = new(d.Rectangle.Left, d.Rectangle.Top, d.Rectangle.Right + 1, d.Rectangle.Bottom + 1);
                                    lockedSurface.ClipRectangle = rectangle;
                                    Draw(lockedSurface);
                                    lockedSurface.ClipRectangle = r;
                                    Rect rectangle2 = d.Rectangle;
                                    InvalidateRectangle(0, ref rectangle2, null);
                                    d.Rectangle = rectangle2;
                                }
                            }
                        }

                        DrawCount = 0;
                    }
                    catch
                    {
                        _ = MessageBox(0, "Draw Error", "Error - " + Name, 0);
                    }

                    UIComponent c;
                    if (DoubleBuffered)
                    {
                        if (Priority >= WindowManager.OverlayPriority || !Drawn)
                        {
                            for (int i = 0, lastI = Children.Count - 1; i <= lastI; i++)
                            {
                                c = Children[i];
                                if (c.Valid && c.Visible && (!Drawn || !c.Drawn))
                                {
                                    if (!Drawn && c is BaseWindow)
                                    {
                                        c.SurfaceValid = false;
                                    }

                                    c.Drawn = false;
                                    c.Draw(lockedSurface);
                                    for (int j = 0, last = DrawCount - 1; j <= last; j++)
                                    {
                                        d = DrawList[j];
                                        Rect rectangle3 = d.Rectangle;
                                        InvalidateRectangle(i + 1, ref rectangle3, null);
                                        d.Rectangle = rectangle3;
                                    }

                                    DrawCount = 0;
                                    if (!c.Drawn || !Drawn)
                                    {
                                        if (SurfaceValid)
                                        {
                                            r = new Rect(c.WinLeft, c.WinTop, c.WinLeft + c.WinWidth, c.WinTop + c.WinHeight);
                                            AddBlitRectangle(ref r, c);
                                        }

                                        c.FirstDraw = c.Drawn;
                                        r = new Rect(WinLeft + c.WinLeft, WinTop + c.WinTop, WinLeft + c.WinLeft + c.WinWidth - 1, WinTop + c.WinTop + c.WinHeight - 1);
                                        InvalidateRectangle(i + 1, ref r, c);
                                    }

                                    c.Drawn = true;
                                }
                            }

                            RaiseDraw(this, lockedSurface);
                        }

                        if (locked)
                        {
                            _ = BackSurface.Unlock();
                        }

                        SetWinLeftWithoutInvalidate(oldLeft);
                        SetWinTopWithoutInvalidate(oldTop);
                        try
                        {
                            if (SurfaceValid)
                            {
                                if (BlitCount > 0)
                                {
                                    RaisePreDrawSurface(this, BackSurface, 0, 0);

                                    for (int j = 0, last = BlitCount - 1; j <= last; j++)
                                    {
                                        d = BlitList[j];
                                        r = new Rect(d.Rectangle.Left, d.Rectangle.Top, d.Rectangle.Right + 1, d.Rectangle.Bottom + 1);
                                        if (!Masked)
                                        {
                                            _ = BackSurface.BlitAreaClipped(surface, r, d.Rectangle.Left + oldLeft, d.Rectangle.Top + oldTop);
                                        }
                                        else
                                        {
                                            int color2PixelInfo = PixelFormats.Color2PixelInfo(pixelFormatId: BackSurface.GetPixelFormatId(), color: MaskColor, palette: null);
                                            _ = BackSurface.BlitAreaColorKeyClipped(surface, r, d.Rectangle.Left + oldLeft, d.Rectangle.Top + oldTop, color2PixelInfo);
                                        }

                                        r = new Rect(d.Rectangle.Left + oldLeft, d.Rectangle.Top + oldTop, d.Rectangle.Right + oldLeft, d.Rectangle.Bottom + oldTop);
                                        WindowManager.AddBlitRectangle(ref r, this);
                                    }

                                    RaiseDrawSurface(this, surface, 0, 0);
                                }
                            }
                            else
                            {
                                Blit(surface);
                            }
                        }
                        catch
                        {
                            _ = MessageBox(0, "Blt Error", "Error - " + Name, 0);
                        }
                    }
                    else
                    {
                        for (int i = 0, last = Children.Count - 1; i <= last; i++)
                        {
                            c = Children[i];
                            if (!Drawn || !c.Drawn || c is BaseWindow)
                            {
                                c.Draw(lockedSurface);
                                c.Drawn = true;
                            }
                        }

                        RaiseDraw(this, lockedSurface);

                        if (locked)
                        {
                            _ = surface.Unlock();
                        }

                        try
                        {
                            RaisePreDrawSurface(this, surface, WinLeft, WinTop);

                            RaiseDrawSurface(this, surface, WinLeft, WinTop);
                        }
                        catch
                        {
                            _ = MessageBox(0, "SB Draw Events Error", "Error - " + Name, 0);
                        }
                    }

                    Drawn = true;
                }
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during DrawSurface", "Error - " + Name, 0);
        }
    }

    // AoWBaseWin.pas:263
    // { DrawSurfDesc draws the window and its controls into SurfDesc. A double-buffered window draws
    //   into its back surface instead (created, or created again when the pixel format changed) and
    //   copies it into SurfDesc, masked with FMaskColor for a masked window. }
    public override void DrawTo(LockedSurface lockedSurface)
    {
        int oldLeft;
        int oldTop;
        bool ok;
        LockedSurface destinationSurfaceDescription = default;
        try
        {
            oldLeft = WinLeft;
            oldTop = WinTop;
            ok = true;
            if (DoubleBuffered)
            {
                destinationSurfaceDescription = lockedSurface;
                if (BackSurface == null)
                {
                    BackSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, null);
                    if (Masked)
                    {
                        _ = BackSurface.Clear(MaskColor);
                    }

                    ok = BackSurface.Lock(ref lockedSurface) >= 0;
                }
                else
                {
                    ok = BackSurface.Lock(ref lockedSurface) >= 0;
                    if (ok)
                    {
                        if (lockedSurface.PixelFormatId != destinationSurfaceDescription.PixelFormatId)
                        {
                            _ = BackSurface.Unlock();
                            BackSurface?.Dispose();
                            BackSurface = new DataSurface(WinWidth, WinHeight, destinationSurfaceDescription.PixelFormatId, null);
                            if (Masked)
                            {
                                _ = BackSurface.Clear(MaskColor);
                            }

                            ok = BackSurface.Lock(ref lockedSurface) >= 0;
                        }
                    }
                }

                if (BackSurface != null)
                {
                    SetWinLeftWithoutInvalidate(0);
                    SetWinTopWithoutInvalidate(0);
                }
            }

            if (ok)
            {
                Draw(lockedSurface);
                for (int i = 0, last = Children.Count - 1; i <= last; i++)
                {
                    UIComponent c = Children[i];
                    if (c.Valid && c.Visible && (!Drawn || !c.Drawn))
                    {
                        c.Draw(lockedSurface);
                        c.Drawn = true;
                    }
                }

                RaiseDraw(this, lockedSurface);

                if (DoubleBuffered)
                {
                    SetWinLeftWithoutInvalidate(oldLeft);
                    SetWinTopWithoutInvalidate(oldTop);
                    Rect sourceRectangle = new(0, 0, WinWidth, WinHeight);
                    // { not used }
                    Rect destinationRectangle = new(WinLeft, WinTop, WinLeft + WinWidth, WinTop + WinHeight);
                    try
                    {
                        _ = Masked
                            ? LockedSurface.BlitSpriteClipped16(lockedSurface, sourceRectangle, destinationSurfaceDescription, WinLeft, WinTop, MaskColor)
                            : LockedSurface.BlitSurfaceDescriptionRectangleClipped(lockedSurface, sourceRectangle, destinationSurfaceDescription, WinLeft, WinTop);
                    }
                    catch
                    {
                    }

                    _ = BackSurface.Unlock();
                }
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during DrawSurfaceDesc", "Error - " + Name, 0);
        }
    }

    // AoWBaseWin.pas:341
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != base.ImageLibraryFrame)
        {
            Valid = false;
            base.ImageLibraryFrame = value;
        }
    }

    // AoWBaseWin.pas:350
    public virtual void SetImageLibraryTexture(InterfaceImageLibrary value)
    {
        if (value != base.ImageLibraryTexture)
        {
            Valid = false;
            base.ImageLibraryTexture = value;
        }
    }

    // AoWBaseWin.pas:79
    public void SetILFIndex(int value)
    {
        if (value != base.ImageLibraryIndexFrame)
        {
            base.ImageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AoWBaseWin.pas:88
    public void SetImageLibraryTileIndex(int value)
    {
        if (value != base.ImageLibraryIndexTexture)
        {
            base.ImageLibraryIndexTexture = value;
            Valid = false;
        }
    }

    // AoWBaseWin.pas:359
    public void SetManager(WindowManager value) => SetOwner(value);

    [Published]
    public override InterfaceImageLibrary ImageLibraryFrame { get => base.ImageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public override InterfaceImageLibrary ImageLibraryTexture { get => base.ImageLibraryTexture; set => SetImageLibraryTexture(value); }

    [Published]
    public override int ImageLibraryIndexFrame { get => base.ImageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public override int ImageLibraryIndexTexture { get => base.ImageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

    public new event NotifyEvent<BaseWindow> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<BaseWindow> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<BaseWindow> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<BaseWindow> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<BaseWindow> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<BaseWindow> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<BaseWindow> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<BaseWindow> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<BaseWindow> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, BaseWindow>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, BaseWindow>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<BaseWindow> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, BaseWindow>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, BaseWindow>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<BaseWindow> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, BaseWindow>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, BaseWindow>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<BaseWindow> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, BaseWindow>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, BaseWindow>(this, slot: nameof(OnExit), value: value);
    }
}
