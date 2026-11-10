// Translated from AOWWinManager.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AOWWinManager unit of aowInt.dpl: TAOWWinManager, the root of a screen of AoW windows. It
//   owns the windows (its children, sorted by priority), draws the background and foreground
//   textures and the windows into the display surface, keeps the dirty rectangles of the whole
//   screen and passes the mouse and keyboard input to the windows from the top down. }
// Fields: FForegroundTexture, FBackgroundTexture, FOverlayPriority (windows below it get no
// input), FNewOverlayPriority (applied by Draw), FBorderLeft, FBorderTop, FBorderRight,
// FBorderBottom, FOldCursor (the cursor before CheckMouseMove), FDefaultCursor (the cursor when
// no window claims the mouse), FOnChangeCursor, FILIndexTBack, FILIndexTFore, FDrawOverlay
// (draw the foreground texture over the windows below the overlay priority)
public partial class AOWWinManager : AoWComponent
{
    public InterfaceIL FForegroundTexture;
    public InterfaceIL FBackgroundTexture;
    public int FOverlayPriority;
    public int FNewOverlayPriority;
    public int FBorderLeft;
    public int FBorderTop;
    public int FBorderRight;
    public int FBorderBottom;
    public int FOldCursor;
    public int FDefaultCursor;
    public NotifyEvent<AOWWinManager> FOnChangeCursor;
    public int FILIndexTBack;
    public int FILIndexTFore;
    public bool FDrawOverlay;

    // AOWWinManager.pas:81
    public AOWWinManager(global::Aow.Rtl.Component owner) : base(owner)
    {
        FWinHeight = 480;
        FWinWidth = 640;
        FOldCursor = -2;
        FDefaultCursor = -2;
        FDrawOverlay = true;
    }

    // AOWWinManager.pas:91
    public override void Validate()
    {
        if (FBackgroundTexture != null)
        {
            FBackgroundTexture = FBackgroundTexture.CheckIL(FILIndexTBack, 1, this);
        }

        if (FForegroundTexture != null)
        {
            FForegroundTexture = FForegroundTexture.CheckIL(FILIndexTFore, 1, this);
        }

        if (FBackgroundTexture != null)
        {
            FValid = true;
        }

        base.Validate();
    }

    // AOWWinManager.pas:176
    public override void Update()
    {
        AoWComponent C = default;
        try
        {
            if (FValid && FVisible)
            {
                if (FOnUpdate != null)
                {
                    FOnUpdate(this);
                }

                for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                {
                    C = (AoWComponent)FChildren[I];
                    if (C.FVisible && C.FPriority >= FOverlayPriority)
                    {
                        C.Update();
                    }
                }

                if (!FValid || !FAligned || !FSized)
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
    public override void InvalidateRect(int Index, ref global::Aow.Rtl.Rect ARect, AoWComponent Sender)
    {
        AoWComponent C;
        global::Aow.Rtl.Rect R = default;
        if (FValid && FVisible)
        {
            if (Sender == null || Sender != null && Sender is AOWWindowControl && ((AoWComponent)FChildren[Index]).FPriority >= FOverlayPriority)
            {
                for (int I = Index, last = FChildren.Count - 1; I <= last; I++)
                {
                    C = (AoWComponent)FChildren[I];
                    if (C.FValid && C.FVisible && FWinLeft + C.FWinLeft <= ARect.Right && FWinLeft + C.FWinLeft + C.FWinWidth >= ARect.Left && FWinTop + C.FWinTop <= ARect.Bottom && FWinTop + C.FWinTop + C.FWinHeight >= ARect.Top && ARect.Left <= ARect.Right && ARect.Top <= ARect.Bottom)
                    {
                        if (C.FDoubleBuffered)
                        {
                            R = global::Aow.Rtl.RtlUnit.Rect(ARect.Left - C.FWinLeft, ARect.Top - C.FWinTop, ARect.Right - C.FWinLeft, ARect.Bottom - C.FWinTop);
                            C.AddBltRect(ref R, C);
                        }
                        else
                        {
                            C.AddDrawRect(ref ARect, C);
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
    public override void DrawSurface(Surface Surface)
    {
        AOWWindowControl C;
        TaowRect D;
        bool WasValid;
        SurfaceDesc SD = default;
        global::Aow.Rtl.Rect R = default;
        try
        {
            Setup();
            Update();
            if (FValid)
            {
                {
                    for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                    {
                        C = (AOWWindowControl)FChildren[I];
                        if (C.FValid)
                        {
                            if (!FDrawn)
                            {
                                if (C.FDoubleBuffered)
                                {
                                    C.FSurfaceValid = false;
                                }
                                else
                                {
                                    C.FDrawn = false;
                                }
                            }

                            if (C.FHidden == true)
                            {
                                C.FHidden = false;
                                R = global::Aow.Rtl.RtlUnit.Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                                AddDrawRect(ref R, C);
                            }
                            else
                            {
                                if (C.FVisible && FDrawn && C.FBackRect.Right > 0 && C.FBackRect.Bottom > 0)
                                {
                                    // { the window moved: add the parts of its old place it no longer covers. For a
                                    //                 masked window the strips are off (original bugs): the third strip of the left
                                    //                 and right cases is everything below the top frame, and the right case starts
                                    //                 at FWinLeft + FFrameRight }
                                    if (C.FBackRect.Left < C.FWinLeft)
                                    {
                                        if (C.FMasked)
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FWinLeft + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FFrameBottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top + C.FFrameBottom, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                        else
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FWinLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                    }

                                    if (C.FBackRect.Top < C.FWinTop)
                                    {
                                        if (C.FMasked)
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FWinTop + C.FFrameTop);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                        else
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FWinTop);
                                            AddDrawRect(ref R, C);
                                        }
                                    }

                                    if (C.FBackRect.Left + C.FBackRect.Right > C.FWinLeft + C.FWinWidth)
                                    {
                                        if (C.FMasked)
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FWinLeft + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FFrameBottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top + C.FFrameBottom, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                        else
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FWinLeft + C.FWinWidth, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                    }

                                    if (C.FBackRect.Top + C.FBackRect.Bottom > C.FWinTop + C.FWinHeight)
                                    {
                                        if (C.FMasked)
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FWinTop + C.FFrameTop, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                        else
                                        {
                                            R = global::Aow.Rtl.RtlUnit.Rect(C.FBackRect.Left, C.FWinTop + C.FWinHeight, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                            AddDrawRect(ref R, C);
                                        }
                                    }
                                }

                                C.FBackRect.Top = C.FWinTop;
                                C.FBackRect.Left = C.FWinLeft;
                                C.FBackRect.Right = C.FWinWidth;
                                C.FBackRect.Bottom = C.FWinHeight;
                            }
                        }
                    }
                }
            }

            if (!FDrawn)
            {
                if (Surface.Lock(ref SD) >= 0)
                {
                    Draw(SD);
                    if (FDrawOverlay && FOverlayPriority > 0)
                    {
                        var rect = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                        FForegroundTexture.DrawILT(FILIndexTFore, rect, ref SD);
                    }

                    Surface.Unlock();
                }
            }
            else
            {
                if (Surface.Lock(ref SD) >= 0)
                {
                    for (int J = 0, last = FDrawCount - 1; J <= last; J++)
                    {
                        D = (TaowRect)FDrawList[J];
                        if (FWinLeft > D.R.Left)
                        {
                            D.R.Left = FWinLeft;
                        }

                        if (FWinTop > D.R.Top)
                        {
                            D.R.Top = FWinTop;
                        }

                        D.R.Right += 1;
                        D.R.Bottom += 1;
                        if (FWinLeft + FWinWidth < D.R.Right)
                        {
                            D.R.Right = FWinLeft + FWinWidth;
                        }

                        if (FWinTop + FWinHeight < D.R.Bottom)
                        {
                            D.R.Bottom = FWinTop + FWinHeight;
                        }

                        var rect = global::Aow.Rtl.RtlUnit.Rect(D.R.Left, D.R.Top, D.R.Right, D.R.Bottom);
                        SD.ClipRect = rect;
                        Draw(SD);
                        if (FDrawOverlay && FOverlayPriority > 0)
                        {
                            var rect2 = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FForegroundTexture.DrawILT(FILIndexTFore, rect2, ref SD);
                        }

                        InvalidateRect(0, ref D.R, null);
                    }

                    FDrawCount = 0;
                    Surface.Unlock();
                }
            }

            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                C = (AOWWindowControl)FChildren[I];
                C.FHidden = false;
                if (C.FValid && C.FVisible)
                {
                    WasValid = C.FSurfaceValid;
                    if (!C.FDrawn)
                    {
                        WasValid = false;
                    }

                    C.DrawSurface(Surface);
                    if (!WasValid || !C.FSurfaceValid)
                    {
                        R = global::Aow.Rtl.RtlUnit.Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                        AddBltRect(ref R, C);
                    }

                    C.FDrawn = true;
                    C.FSurfaceValid = true;
                    for (int J = 0, lastJ = FBltCount - 1; J <= lastJ; J++)
                    {
                        D = (TaowRect)FBltList[J];
                        InvalidateRect(I + 1, ref D.R, null);
                    }

                    if (FDrawOverlay && C.FPriority < FOverlayPriority)
                    {
                        if (Surface.Lock(ref SD) >= 0)
                        {
                            for (int J = 0, lastJ = FBltCount - 1; J <= lastJ; J++)
                            {
                                D = (TaowRect)FBltList[J];
                                if (C.FWinLeft > D.R.Left)
                                {
                                    D.R.Left = C.FWinLeft;
                                }

                                if (C.FWinTop > D.R.Top)
                                {
                                    D.R.Top = C.FWinTop;
                                }

                                D.R.Right += 1;
                                D.R.Bottom += 1;
                                if (C.FWinLeft + C.FWinWidth < D.R.Right)
                                {
                                    D.R.Right = C.FWinLeft + C.FWinWidth;
                                }

                                if (C.FWinTop + C.FWinHeight < D.R.Bottom)
                                {
                                    D.R.Bottom = C.FWinTop + C.FWinHeight;
                                }

                                var rect = global::Aow.Rtl.RtlUnit.Rect(D.R.Left, D.R.Top, D.R.Right, D.R.Bottom);
                                SD.ClipRect = rect;
                                var rect2 = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                                FForegroundTexture.DrawILT(FILIndexTFore, rect2, ref SD);
                            }

                            Surface.Unlock();
                        }
                    }

                    FBltCount = 0;
                }
            }

            FDrawn = true;
            ClearDrawList();
            ClearBltList();
        }
        catch
        {
            MessageBox(0, "Exception during DrawSurface", "Error - " + Name, 0);
        }
    }

    // AOWWinManager.pas:157
    public override void Draw(SurfaceDesc SurfDesc)
    {
        AoWComponent C;
        if (FOverlayPriority > FNewOverlayPriority)
        {
            for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
            {
                C = (AoWComponent)FChildren[I];
                if (C.FPriority >= FNewOverlayPriority && C.FPriority < FOverlayPriority && C.FVisible)
                {
                    C.FDrawn = false;
                    C.FSurfaceValid = false;
                }
            }
        }

        FOverlayPriority = FNewOverlayPriority;
        var rect = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
        FBackgroundTexture.DrawILT(FILIndexTBack, rect, ref SurfDesc);
    }

    // AOWWinManager.pas:449
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        bool Redraw = default;
        FHandled = false;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (AoWComponent)FChildren[I];
            if (C.FPriority >= FOverlayPriority)
            {
                Redraw = C.FRedraw;
                C.FHandled = false;
                if (!FHandled || Redraw == true)
                {
                    C.CheckMouseDown(Button, Shift, X, Y);
                }

                if (C.FHandled)
                {
                    FHandled = true;
                }
            }
        }

        if (FOnMouseDown != null)
        {
            FOnMouseDown(this, Button, Shift, X, Y);
        }
    }

    // AOWWinManager.pas:473
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        AoWComponent C = default;
        FHandled = false;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (AoWComponent)FChildren[I];
            if (C.FPriority >= FOverlayPriority)
            {
                if (!FHandled || C.FRedraw == true)
                {
                    C.CheckMouseUp(Button, Shift, X, Y);
                }

                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }
        }

        if (FOnMouseUp != null)
        {
            FOnMouseUp(this, Button, Shift, X, Y);
        }
    }

    // AOWWinManager.pas:494
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        AOWWindowControl C = default;
        FHandled = false;
        FOldCursor = FCursor;
        FCursor = FDefaultCursor;
        if (X < 0)
        {
            X = 0;
        }

        if (Y < 0)
        {
            Y = 0;
        }

        if (FWinWidth <= X)
        {
            X = FWinWidth - 1;
        }

        if (FWinHeight <= Y)
        {
            Y = FWinHeight - 1;
        }

        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (AOWWindowControl)FChildren[I];
            if (C.FPriority >= FOverlayPriority)
            {
                if (!FHandled || C.FRedraw == true)
                {
                    C.CheckMouseMove(Shift, X, Y);
                    if (C.FHandled == true)
                    {
                        if (!FHandled)
                        {
                            FCursor = C.FCursor;
                            if (!C.FMouseInside)
                            {
                                C.FMouseInside = true;
                                if (C.FOnEnter != null)
                                {
                                    C.FOnEnter(C);
                                }
                            }
                        }
                        else
                        {
                            if (C.FMouseInside)
                            {
                                C.FMouseInside = false;
                                if (C.FOnExit != null)
                                {
                                    C.FOnExit(C);
                                }
                            }
                        }

                        FHandled = true;
                    }
                    else
                    {
                        if (C.FMouseInside)
                        {
                            C.FMouseInside = false;
                            if (C.FOnExit != null)
                            {
                                C.FOnExit(C);
                            }
                        }
                    }
                }
                else
                {
                    if (C.FMouseInside)
                    {
                        C.FMouseInside = false;
                        if (C.FOnExit != null)
                        {
                            C.FOnExit(C);
                        }
                    }
                }
            }
        }

        if (FCursor != FOldCursor)
        {
            if (FOnChangeCursor != null)
            {
                FOnChangeCursor(this);
            }
        }

        if (FOnMouseMove != null)
        {
            FOnMouseMove(this, Shift, X, Y);
        }
    }

    // AOWWinManager.pas:558
    public override void CheckKeyPress(ref byte Key)
    {
        AoWComponent C = default;
        FHandled = false;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (AoWComponent)FChildren[I];
            if (C.FPriority >= FOverlayPriority)
            {
                C.CheckKeyPress(ref Key);
                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }
        }

        if (FOnKeyPress != null)
        {
            FOnKeyPress(this, ref Key);
        }
    }

    // AOWWinManager.pas:578
    public override void CheckKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        AoWComponent C = default;
        FHandled = false;
        for (int I = FChildren.Count - 1; I >= 0; I--)
        {
            C = (AoWComponent)FChildren[I];
            if (!FHandled && C.FPriority >= FOverlayPriority && C.FVisible)
            {
                C.CheckKeyDown(ref Key, Shift);
                if (C.FHandled == true)
                {
                    FHandled = true;
                }
            }
        }

        if (!FHandled && FOnKeyDown != null)
        {
            FOnKeyDown(this, ref Key, Shift);
        }
    }

    // AOWWinManager.pas:102
    public virtual void SetILTB(InterfaceIL Value)
    {
        if (Value != FBackgroundTexture)
        {
            FValid = false;
            FBackgroundTexture = Value;
        }
    }

    // AOWWinManager.pas:111
    public virtual void SetILTF(InterfaceIL Value)
    {
        if (Value != FForegroundTexture)
        {
            FValid = false;
            FForegroundTexture = Value;
        }
    }

    // AOWWinManager.pas:120
    public void SetILBIndex(int Value)
    {
        if (Value != FILIndexTBack)
        {
            FILIndexTBack = Value;
            FValid = false;
        }
    }

    // AOWWinManager.pas:129
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexTFore)
        {
            FILIndexTFore = Value;
            FValid = false;
        }
    }

    // AOWWinManager.pas:138
    public void SetOverPri(int Value)
    {
        if (Value != FOverlayPriority || Value != FNewOverlayPriority)
        {
            FNewOverlayPriority = Value;
            FDrawn = false;
        }
    }

    // AOWWinManager.pas:147
    public void SetCursor(int Value)
    {
        if (Value != FCursor)
        {
            FCursor = Value;
            if (FOnChangeCursor != null)
            {
                FOnChangeCursor(this);
            }
        }
    }

    [Published]
    public int Cursor { get => FCursor; set => SetCursor(value); }

    [Published]
    public NotifyEvent<AOWWinManager> OnChangeCursor { get => FOnChangeCursor; set => FOnChangeCursor = value; }

    [Published]
    public InterfaceIL BackgroundTexture { get => FBackgroundTexture; set => SetILTB(value); }

    [Published]
    public InterfaceIL ForegroundTexture { get => FForegroundTexture; set => SetILTF(value); }

    [Published]
    public int ILIndexTBack { get => FILIndexTBack; set => SetILBIndex(value); }

    [Published]
    public int ILIndexTFore { get => FILIndexTFore; set => SetILFIndex(value); }

    [Published]
    public int OverlayPriority { get => FOverlayPriority; set => SetOverPri(value); }

    [Published]
    public int BorderLeft { get => FBorderLeft; set => FBorderLeft = value; }

    [Published]
    public int BorderRight { get => FBorderRight; set => FBorderRight = value; }

    [Published]
    public int BorderTop { get => FBorderTop; set => FBorderTop = value; }

    [Published]
    public int BorderBottom { get => FBorderBottom; set => FBorderBottom = value; }

    [Published]
    public bool DrawOverlay { get => FDrawOverlay; set => FDrawOverlay = value; }

    public new NotifyEvent<AOWWinManager> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWWinManager> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWWinManager> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWWinManager> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWWinManager> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWWinManager> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWWinManager> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWWinManager> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWWinManager> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
