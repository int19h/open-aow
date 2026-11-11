// Translated from AoWBaseWin.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

// { The AoWBaseWin unit of aowInt.dpl: TAOWBaseWindow, a window of the window manager. It draws
//   itself and its controls into its back surface (DrawSurface) or straight into the display surface
//   (DrawSurfDesc), redrawing only the dirty rectangles, and passes invalidated rectangles on to the
//   windows above it and to its owner. }
// Fields: FBuffer (freed by TAOWGenericWindow.Destroy), FFreeSurfaceOnHide, FWinManager,
// FOnShow, FOnHide
public partial class AOWBaseWindow : AOWWindowControl
{
    public AOWBaseWindow(Component owner) : base(owner)
    { }

    public object FBuffer;
    public bool FFreeSurfaceOnHide;
    public AOWWinManager FWinManager;
    public NotifyEvent<AOWBaseWindow> FOnShow;
    public NotifyEvent<AOWBaseWindow> FOnHide;

    // AoWBaseWin.pas:56
    public override void InvalidateRect(int Index, ref Rect ARect, AoWComponent Sender)
    {
        AoWComponent C;
        if (Index > -1 && FValid && FVisible)
        {
            for (int I = Index, last = FChildren.Count - 1; I <= last; I++)
            {
                C = (AoWComponent)FChildren[I];
                if (C.FValid && C.FVisible && ARect.Right >= C.FWinLeft && C.FWinLeft + C.FWinWidth >= ARect.Left && ARect.Bottom >= C.FWinTop && C.FWinTop + C.FWinHeight >= ARect.Top && C != Sender)
                {
                    if (C is AOWBaseWindow)
                    {
                        C.InvalidateRect(0, ref ARect, this);
                    }
                    else
                    {
                        C.FDrawn = false;
                    }
                }
            }

            if (FAOWOwner != Sender && FAOWOwner is AOWBaseWindow)
            {
                var indexOf = FAOWOwner.FChildren.IndexOf(this);
                FAOWOwner.InvalidateRect(indexOf + 1, ref ARect, this);
            }
        }
    }

    // AoWBaseWin.pas:50
    public override void AddBltRect(ref Rect ARect, AoWComponent Sender)
    {
        if (FDoubleBuffered && FSurfaceValid)
        {
            base.AddBltRect(ref ARect, Sender);
        }
    }

    // AoWBaseWin.pas:367
    // { SetVisible: a hidden window marks itself hidden (the window manager redraws the place it
    //   covered) and may free its back surface; a shown window is redrawn from scratch, remembers its
    //   place in FBackRect, and is put back at its place in the window order. }
    public override void SetVisible(bool Value)
    {
        if (Value != FVisible)
        {
            FVisible = Value;
            if (!FVisible)
            {
                FHidden = true;
                if (FOnHide != null)
                {
                    FOnHide(this);
                }

                if (FFreeSurfaceOnHide && FBackSurface != null)
                {
                    FBackSurface?.Dispose();
                    FBackSurface = null;
                }
            }
            else
            {
                if (FDoubleBuffered)
                {
                    FSurfaceValid = false;
                }
                else
                {
                    FDrawn = false;
                }

                if (!FHidden)
                {
                    FBackRect.Right = FWinWidth;
                    FBackRect.Bottom = FWinHeight;
                    FBackRect.Top = FWinTop;
                    FBackRect.Left = FWinLeft;
                }

                if (!FValid || !FAligned || !FSized)
                {
                    Setup();
                }

                ClearBltList();
                ClearDrawList();
                if (FOnShow != null)
                {
                    FOnShow(this);
                }
            }
        }

        if (FVisible)
        {
            SetPriority(FPriority);
        }
    }

    // AoWBaseWin.pas:102
    // { DrawSurface draws the window and its controls. A double-buffered window draws into its back
    //   surface (at 0, 0: the position is restored afterwards), redrawing everything when it is not
    //   drawn or only the rectangles of its draw list, then lets the controls draw themselves and
    //   blits the dirty rectangles of its blit list to Surface, telling the window manager about each
    //   one. A window without a back surface draws itself and its controls straight into Surface. }
    public override void DrawSurface(Surface Surface)
    {
        AoWComponent C = default;
        TaowRect D = default;
        int OldLeft;
        int OldTop;
        bool Locked;
        SurfaceDesc SD = default;
        Rect R = default;
        try
        {
            if (FValid && FVisible)
            {
                OldLeft = FWinLeft;
                OldTop = FWinTop;
                try
                {
                    if (FDoubleBuffered)
                    {
                        FDrawn = !VerifyBackSurface(Surface) && FDrawn;
                        FWinLeft = 0;
                        FWinTop = 0;
                        Locked = FBackSurface.Lock(ref SD) == 0;
                    }
                    else
                    {
                        Locked = Surface.Lock(ref SD) == 0;
                    }
                }
                catch
                {
                    Locked = false;
                    MessageBox(0, "Verify Surface Error", "Error - " + Name, 0);
                }

                if (Locked)
                {
                    try
                    {
                        if (!FDrawn)
                        {
                            FSurfaceValid = false;
                            ClearDrawList();
                        }

                        if (!FSurfaceValid)
                        {
                            ClearBltList();
                        }

                        if (!FDrawn)
                        {
                            Draw(SD);
                        }
                        else
                        {
                            if (FPriority >= FWinManager.FOverlayPriority)
                            {
                                for (int J = 0, last = FDrawCount - 1; J <= last; J++)
                                {
                                    D = FDrawList[J];
                                    R = SD.ClipRect;
                                    var rect = new Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                                    SD.ClipRect = rect;
                                    Draw(SD);
                                    SD.ClipRect = R;
                                    InvalidateRect(0, ref D.R, null);
                                }
                            }
                        }

                        FDrawCount = 0;
                    }
                    catch
                    {
                        MessageBox(0, "Draw Error", "Error - " + Name, 0);
                    }

                    if (FDoubleBuffered)
                    {
                        if (FPriority >= FWinManager.FOverlayPriority || !FDrawn)
                        {
                            for (int I = 0, lastI = FChildren.Count - 1; I <= lastI; I++)
                            {
                                C = (AoWComponent)FChildren[I];
                                if (C.FValid && C.FVisible && (!FDrawn || !C.FDrawn))
                                {
                                    if (!FDrawn && C is AOWBaseWindow)
                                    {
                                        C.FSurfaceValid = false;
                                    }

                                    C.FDrawn = false;
                                    C.Draw(SD);
                                    for (int J = 0, last = FDrawCount - 1; J <= last; J++)
                                    {
                                        D = FDrawList[J];
                                        InvalidateRect(I + 1, ref D.R, null);
                                    }

                                    FDrawCount = 0;
                                    if (!C.FDrawn || !FDrawn)
                                    {
                                        if (FSurfaceValid)
                                        {
                                            R = new Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                                            AddBltRect(ref R, C);
                                        }

                                        C.FFirstDraw = C.FDrawn;
                                        R = new Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, FWinLeft + C.FWinLeft + C.FWinWidth - 1, FWinTop + C.FWinTop + C.FWinHeight - 1);
                                        InvalidateRect(I + 1, ref R, C);
                                    }

                                    C.FDrawn = true;
                                }
                            }

                            if (FOnDraw != null)
                            {
                                FOnDraw(this, SD);
                            }
                        }

                        if (Locked)
                        {
                            FBackSurface.Unlock();
                        }

                        FWinLeft = OldLeft;
                        FWinTop = OldTop;
                        try
                        {
                            if (FSurfaceValid)
                            {
                                if (FBltCount > 0)
                                {
                                    if (FOnPreDrawSurface != null)
                                    {
                                        FOnPreDrawSurface(this, FBackSurface, 0, 0);
                                    }

                                    for (int J = 0, last = FBltCount - 1; J <= last; J++)
                                    {
                                        D = FBltList[J];
                                        R = new Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                                        if (!FMasked)
                                        {
                                            FBackSurface.BltAreaClipped(Surface, R, D.R.Left + OldLeft, D.R.Top + OldTop);
                                        }
                                        else
                                        {
                                            var pixelFormatID = FBackSurface.GetPixelFormatID();
                                            var color2PixelInfo = PixelFormats.Color2PixelInfo(FMaskColor, pixelFormatID, null);
                                            FBackSurface.BltAreaColorKeyClipped(Surface, R, D.R.Left + OldLeft, D.R.Top + OldTop, color2PixelInfo);
                                        }

                                        R = new Rect(D.R.Left + OldLeft, D.R.Top + OldTop, D.R.Right + OldLeft, D.R.Bottom + OldTop);
                                        FWinManager.AddBltRect(ref R, this);
                                    }

                                    if (FOnDrawSurface != null)
                                    {
                                        FOnDrawSurface(this, Surface, 0, 0);
                                    }
                                }
                            }
                            else
                            {
                                Blt(Surface);
                            }
                        }
                        catch
                        {
                            MessageBox(0, "Blt Error", "Error - " + Name, 0);
                        }
                    }
                    else
                    {
                        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                        {
                            C = (AoWComponent)FChildren[I];
                            if (!FDrawn || !C.FDrawn || C is AOWBaseWindow)
                            {
                                C.Draw(SD);
                                C.FDrawn = true;
                            }
                        }

                        if (FOnDraw != null)
                        {
                            FOnDraw(this, SD);
                        }

                        if (Locked)
                        {
                            Surface.Unlock();
                        }

                        try
                        {
                            if (FOnPreDrawSurface != null)
                            {
                                FOnPreDrawSurface(this, Surface, FWinLeft, FWinTop);
                            }

                            if (FOnDrawSurface != null)
                            {
                                FOnDrawSurface(this, Surface, FWinLeft, FWinTop);
                            }
                        }
                        catch
                        {
                            MessageBox(0, "SB Draw Events Error", "Error - " + Name, 0);
                        }
                    }

                    FDrawn = true;
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
    public override void DrawSurfDesc(SurfaceDesc SurfDesc)
    {
        AoWComponent C = default;
        int OldLeft;
        int OldTop;
        bool Ok;
        SurfaceDesc DestSD = default;
        Rect SrcRect = default;
        Rect DestRect = default;
        try
        {
            OldLeft = FWinLeft;
            OldTop = FWinTop;
            Ok = true;
            if (FDoubleBuffered)
            {
                DestSD = SurfDesc;
                if (FBackSurface == null)
                {
                    FBackSurface = new DataSurface(FWinWidth, FWinHeight, SurfDesc.PixelFormatID, null);
                    if (FMasked)
                    {
                        FBackSurface.Cls(FMaskColor);
                    }

                    Ok = FBackSurface.Lock(ref SurfDesc) >= 0;
                }
                else
                {
                    Ok = FBackSurface.Lock(ref SurfDesc) >= 0;
                    if (Ok)
                    {
                        if (SurfDesc.PixelFormatID != DestSD.PixelFormatID)
                        {
                            FBackSurface.Unlock();
                            FBackSurface?.Dispose();
                            FBackSurface = new DataSurface(FWinWidth, FWinHeight, DestSD.PixelFormatID, null);
                            if (FMasked)
                            {
                                FBackSurface.Cls(FMaskColor);
                            }

                            Ok = FBackSurface.Lock(ref SurfDesc) >= 0;
                        }
                    }
                }

                if (FBackSurface != null)
                {
                    FWinLeft = 0;
                    FWinTop = 0;
                }
            }

            if (Ok)
            {
                Draw(SurfDesc);
                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    C = (AoWComponent)FChildren[I];
                    if (C.FValid && C.FVisible && (!FDrawn || !C.FDrawn))
                    {
                        C.Draw(SurfDesc);
                        C.FDrawn = true;
                    }
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }

                if (FDoubleBuffered)
                {
                    FWinLeft = OldLeft;
                    FWinTop = OldTop;
                    SrcRect = new Rect(0, 0, FWinWidth, FWinHeight);
                    // { not used }
                    DestRect = new Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth, FWinTop + FWinHeight);
                    try
                    {
                        if (FMasked == true)
                        {
                            SurfaceDesc.BltSpriteClipped16(SurfDesc, SrcRect, DestSD, FWinLeft, FWinTop, FMaskColor);
                        }
                        else
                        {
                            SurfaceDesc.BltSDRectClipped(SurfDesc, SrcRect, DestSD, FWinLeft, FWinTop);
                        }
                    }
                    catch
                    {
                    }

                    FBackSurface.Unlock();
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during DrawSurfaceDesc", "Error - " + Name, 0);
        }
    }

    // AoWBaseWin.pas:341
    public virtual void SetILFrame(InterfaceIL Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AoWBaseWin.pas:350
    public virtual void SetILTexture(InterfaceIL Value)
    {
        if (Value != FILTexture)
        {
            FValid = false;
            FILTexture = Value;
        }
    }

    // AoWBaseWin.pas:79
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // AoWBaseWin.pas:88
    public void SetILTIndex(int Value)
    {
        if (Value != FILIndexTexture)
        {
            FILIndexTexture = Value;
            FValid = false;
        }
    }

    // AoWBaseWin.pas:359
    public void SetManager(AOWWinManager Value)
    {
        SetOwner(Value);
    }

    public InterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }
    public InterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }
    public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }
    public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

    [Published]
    public NotifyEvent<AOWBaseWindow> OnHide { get => FOnHide; set => FOnHide = value; }

    [Published]
    public NotifyEvent<AOWBaseWindow> OnShow { get => FOnShow; set => FOnShow = value; }

    public new NotifyEvent<AOWBaseWindow> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWBaseWindow> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWBaseWindow> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWBaseWindow> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWBaseWindow> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWBaseWindow> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWBaseWindow> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWBaseWindow> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWBaseWindow> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new AoWOnDrawBackEvent<AOWBaseWindow> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new NotifyEvent<AOWBaseWindow> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new NotifyEvent<AOWBaseWindow> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }
}
