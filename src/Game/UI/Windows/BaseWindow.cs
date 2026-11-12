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

    public bool _freeSurfaceOnHide;
    public WindowManager _windowManager;
    [Published]
    public event NotifyEvent<BaseWindow> OnShow;

    protected void RaiseShow(BaseWindow sender)
    {
        if (OnShow != null)
        {
            OnShow(sender);
        }
    }

    [Published]
    public event NotifyEvent<BaseWindow> OnHide;

    protected void RaiseHide(BaseWindow sender)
    {
        if (OnHide != null)
        {
            OnHide(sender);
        }
    }

    // AoWBaseWin.pas:56
    public override void InvalidateRectangle(int index, ref Rect rectangle, UIComponent sender)
    {
        UIComponent C;
        if (index > -1 && Valid && Visible)
        {
            for (int I = index, last = Children.Count - 1; I <= last; I++)
            {
                C = Children[I];
                if (C.Valid && C.Visible && rectangle.Right >= C.WinLeft && C.WinLeft + C.WinWidth >= rectangle.Left && rectangle.Bottom >= C.WinTop && C.WinTop + C.WinHeight >= rectangle.Top && C != sender)
                {
                    if (C is BaseWindow)
                    {
                        C.InvalidateRectangle(0, ref rectangle, this);
                    }
                    else
                    {
                        C.Drawn = false;
                    }
                }
            }

            if (OwnerComponent != sender && OwnerComponent is BaseWindow)
            {
                var indexOf = OwnerComponent.Children.IndexOf(this);
                OwnerComponent.InvalidateRectangle(indexOf + 1, ref rectangle, this);
            }
        }
    }

    // AoWBaseWin.pas:50
    public override void AddBlitRectangle(ref Rect rectangle, UIComponent sender)
    {
        if (_doubleBuffered && SurfaceValid)
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
                if (OnHide != null)
                {
                    OnHide(this);
                }

                if (_freeSurfaceOnHide && BackSurface != null)
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }
            else
            {
                if (_doubleBuffered)
                {
                    SurfaceValid = false;
                }
                else
                {
                    Drawn = false;
                }

                if (!Hidden)
                {
                    BackRectangle.Right = WinWidth;
                    BackRectangle.Bottom = WinHeight;
                    BackRectangle.Top = WinTop;
                    BackRectangle.Left = WinLeft;
                }

                if (!Valid || !Aligned || !Sized)
                {
                    Setup();
                }

                ClearBlitList();
                ClearDrawList();
                if (OnShow != null)
                {
                    OnShow(this);
                }
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
        UIComponent C = default;
        DirtyRectangle D = default;
        int oldLeft;
        int oldTop;
        bool locked;
        LockedSurface lockedSurface = default;
        Rect R = default;
        try
        {
            if (Valid && Visible)
            {
                oldLeft = WinLeft;
                oldTop = WinTop;
                try
                {
                    if (_doubleBuffered)
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
                    MessageBox(0, "Verify Surface Error", "Error - " + Name, 0);
                }

                if (locked)
                {
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
                            if (Priority >= _windowManager.OverlayPriority)
                            {
                                for (int J = 0, last = DrawCount - 1; J <= last; J++)
                                {
                                    D = DrawList[J];
                                    R = lockedSurface.ClipRectangle;
                                    var rectangle = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right + 1, D.Rectangle.Bottom + 1);
                                    lockedSurface.ClipRectangle = rectangle;
                                    Draw(lockedSurface);
                                    lockedSurface.ClipRectangle = R;
                                    InvalidateRectangle(0, ref D.Rectangle, null);
                                }
                            }
                        }

                        DrawCount = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Draw Error", "Error - " + Name, 0);
                    }

                    if (_doubleBuffered)
                    {
                        if (Priority >= _windowManager.OverlayPriority || !Drawn)
                        {
                            for (int I = 0, lastI = Children.Count - 1; I <= lastI; I++)
                            {
                                C = Children[I];
                                if (C.Valid && C.Visible && (!Drawn || !C.Drawn))
                                {
                                    if (!Drawn && C is BaseWindow)
                                    {
                                        C.SurfaceValid = false;
                                    }

                                    C.Drawn = false;
                                    C.Draw(lockedSurface);
                                    for (int J = 0, last = DrawCount - 1; J <= last; J++)
                                    {
                                        D = DrawList[J];
                                        InvalidateRectangle(I + 1, ref D.Rectangle, null);
                                    }

                                    DrawCount = 0;
                                    if (!C.Drawn || !Drawn)
                                    {
                                        if (SurfaceValid)
                                        {
                                            R = new Rect(C.WinLeft, C.WinTop, C.WinLeft + C.WinWidth, C.WinTop + C.WinHeight);
                                            AddBlitRectangle(ref R, C);
                                        }

                                        C.FirstDraw = C.Drawn;
                                        R = new Rect(WinLeft + C.WinLeft, WinTop + C.WinTop, WinLeft + C.WinLeft + C.WinWidth - 1, WinTop + C.WinTop + C.WinHeight - 1);
                                        InvalidateRectangle(I + 1, ref R, C);
                                    }

                                    C.Drawn = true;
                                }
                            }

                            RaiseDraw(this, lockedSurface);
                        }

                        if (locked)
                        {
                            BackSurface.Unlock();
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

                                    for (int J = 0, last = BlitCount - 1; J <= last; J++)
                                    {
                                        D = BlitList[J];
                                        R = new Rect(D.Rectangle.Left, D.Rectangle.Top, D.Rectangle.Right + 1, D.Rectangle.Bottom + 1);
                                        if (!Masked)
                                        {
                                            BackSurface.BlitAreaClipped(surface, R, D.Rectangle.Left + oldLeft, D.Rectangle.Top + oldTop);
                                        }
                                        else
                                        {
                                            var pixelFormatId = BackSurface.GetPixelFormatId();
                                            var color2PixelInfo = PixelFormats.Color2PixelInfo(_maskColor, pixelFormatId, null);
                                            BackSurface.BlitAreaColorKeyClipped(surface, R, D.Rectangle.Left + oldLeft, D.Rectangle.Top + oldTop, color2PixelInfo);
                                        }

                                        R = new Rect(D.Rectangle.Left + oldLeft, D.Rectangle.Top + oldTop, D.Rectangle.Right + oldLeft, D.Rectangle.Bottom + oldTop);
                                        _windowManager.AddBlitRectangle(ref R, this);
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
                            MessageBox(0, "Blt Error", "Error - " + Name, 0);
                        }
                    }
                    else
                    {
                        for (int I = 0, last = Children.Count - 1; I <= last; I++)
                        {
                            C = Children[I];
                            if (!Drawn || !C.Drawn || C is BaseWindow)
                            {
                                C.Draw(lockedSurface);
                                C.Drawn = true;
                            }
                        }

                        RaiseDraw(this, lockedSurface);

                        if (locked)
                        {
                            surface.Unlock();
                        }

                        try
                        {
                            RaisePreDrawSurface(this, surface, WinLeft, WinTop);

                            RaiseDrawSurface(this, surface, WinLeft, WinTop);
                        }
                        catch
                        {
                            MessageBox(0, "SB Draw Events Error", "Error - " + Name, 0);
                        }
                    }

                    Drawn = true;
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during DrawSurface", "Error - " + Name, 0);
        }
    }

    // AoWBaseWin.pas:263
    // { DrawSurfDesc draws the window and its controls into SurfDesc. A double-buffered window draws
    //   into its back surface instead (created, or created again when the pixel format changed) and
    //   copies it into SurfDesc, masked with FMaskColor for a masked window. }
    public override void DrawTo(LockedSurface lockedSurface)
    {
        UIComponent C = default;
        int oldLeft;
        int oldTop;
        bool ok;
        LockedSurface destinationSurfaceDescription = default;
        Rect sourceRectangle = default;
        Rect destinationRectangle = default;
        try
        {
            oldLeft = WinLeft;
            oldTop = WinTop;
            ok = true;
            if (_doubleBuffered)
            {
                destinationSurfaceDescription = lockedSurface;
                if (BackSurface == null)
                {
                    BackSurface = new DataSurface(WinWidth, WinHeight, lockedSurface.PixelFormatId, null);
                    if (Masked)
                    {
                        BackSurface.Clear(_maskColor);
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
                            BackSurface.Unlock();
                            BackSurface?.Dispose();
                            BackSurface = new DataSurface(WinWidth, WinHeight, destinationSurfaceDescription.PixelFormatId, null);
                            if (Masked)
                            {
                                BackSurface.Clear(_maskColor);
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
                for (int I = 0, last = Children.Count - 1; I <= last; I++)
                {
                    C = Children[I];
                    if (C.Valid && C.Visible && (!Drawn || !C.Drawn))
                    {
                        C.Draw(lockedSurface);
                        C.Drawn = true;
                    }
                }

                RaiseDraw(this, lockedSurface);

                if (_doubleBuffered)
                {
                    SetWinLeftWithoutInvalidate(oldLeft);
                    SetWinTopWithoutInvalidate(oldTop);
                    sourceRectangle = new Rect(0, 0, WinWidth, WinHeight);
                    // { not used }
                    destinationRectangle = new Rect(WinLeft, WinTop, WinLeft + WinWidth, WinTop + WinHeight);
                    try
                    {
                        if (Masked == true)
                        {
                            LockedSurface.BlitSpriteClipped16(lockedSurface, sourceRectangle, destinationSurfaceDescription, WinLeft, WinTop, _maskColor);
                        }
                        else
                        {
                            LockedSurface.BlitSurfaceDescriptionRectangleClipped(lockedSurface, sourceRectangle, destinationSurfaceDescription, WinLeft, WinTop);
                        }
                    }
                    catch
                    {
                    }

                    BackSurface.Unlock();
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during DrawSurfaceDesc", "Error - " + Name, 0);
        }
    }

    // AoWBaseWin.pas:341
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWBaseWin.pas:350
    public virtual void SetImageLibraryTexture(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTexture)
        {
            Valid = false;
            _imageLibraryTexture = value;
        }
    }

    // AoWBaseWin.pas:79
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AoWBaseWin.pas:88
    public void SetImageLibraryTileIndex(int value)
    {
        if (value != _imageLibraryIndexTexture)
        {
            _imageLibraryIndexTexture = value;
            Valid = false;
        }
    }

    // AoWBaseWin.pas:359
    public void SetManager(WindowManager value)
    {
        SetOwner(value);
    }

    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }
    public InterfaceImageLibrary ImageLibraryTexture { get => _imageLibraryTexture; set => SetImageLibraryTexture(value); }
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }
    public int ImageLibraryIndexTexture { get => _imageLibraryIndexTexture; set => SetImageLibraryTileIndex(value); }

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
