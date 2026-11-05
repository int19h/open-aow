// Translated from AOWWinManager.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;

namespace Aow.Units.AOWWinManager
{
    // { The AOWWinManager unit of aowInt.dpl: TAOWWinManager, the root of a screen of AoW windows. It
//   owns the windows (its children, sorted by priority), draws the background and foreground
//   textures and the windows into the display surface, keeps the dirty rectangles of the whole
//   screen and passes the mouse and keyboard input to the windows from the top down. }
    // Fields: FForegroundTexture, FBackgroundTexture, FOverlayPriority (windows below it get no
    // input), FNewOverlayPriority (applied by Draw), FBorderLeft, FBorderTop, FBorderRight,
    // FBorderBottom, FOldCursor (the cursor before CheckMouseMove), FDefaultCursor (the cursor when
    // no window claims the mouse), FOnChangeCursor, FILIndexTBack, FILIndexTFore, FDrawOverlay
    // (draw the foreground texture over the windows below the overlay priority)
    public partial class TAOWWinManager : TAoWComponent
    {
        public TInterfaceIL FForegroundTexture;
        public TInterfaceIL FBackgroundTexture;
        public int FOverlayPriority;
        public int FNewOverlayPriority;
        public int FBorderLeft;
        public int FBorderTop;
        public int FBorderRight;
        public int FBorderBottom;
        public int FOldCursor;
        public int FDefaultCursor;
        public TNotifyEvent FOnChangeCursor;
        public int FILIndexTBack;
        public int FILIndexTFore;
        public bool FDrawOverlay;
        // AOWWinManager.pas:81
        public TAOWWinManager(TComponent owner) : base(owner)
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
                var call__1 = FBackgroundTexture.CheckIL(FILIndexTBack, 1, this);
                FBackgroundTexture = call__1;
            }

            if (FForegroundTexture != null)
            {
                var call__2 = FForegroundTexture.CheckIL(FILIndexTFore, 1, this);
                FForegroundTexture = call__2;
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
            int I = default;
            TAoWComponent C = default;
            try
            {
                if (FValid && FVisible)
                {
                    if (FOnUpdate != null)
                    {
                        FOnUpdate(this);
                    }

                    {
                        I = 0;
                        int end__3 = FChildren.Count - 1;
                        for (; I <= end__3; I += 1)
                        {
                            var call__4 = FChildren[I];
                            C = (TAoWComponent)call__4;
                            if (C.FVisible && C.FPriority >= FOverlayPriority)
                            {
                                C.Update();
                            }
                        }
                    }

                    if (!FValid || !FAligned || !FSized)
                    {
                        Setup();
                    }
                }
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Exception during Update", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AOWWinManager.pas:204
        // { InvalidateRect passes ARect (screen coordinates) on to the windows from Index up that it touches
//   (a rectangle from a window only when that window is not below the overlay priority): a
//   double-buffered window adds it, in its own coordinates, to its blit list, any other window adds
//   it to its draw list. The window itself is passed as the Sender. }
        public override void InvalidateRect(int Index, ref TRect ARect, TAoWComponent Sender)
        {
            int I = default;
            TAoWComponent C = default;
            TRect R = default;
            if (FValid && FVisible)
            {
                bool bool__5 = Sender == null;
                if (!bool__5)
                {
                    bool bool__6 = Sender != null && Sender is TAOWWindowControl;
                    if (bool__6)
                    {
                        var call__7 = FChildren[Index];
                        bool__6 = ((TAoWComponent)call__7).FPriority >= FOverlayPriority;
                    }

                    bool__5 = bool__6;
                }

                if (bool__5)
                {
                    {
                        I = Index;
                        int end__8 = FChildren.Count - 1;
                        for (; I <= end__8; I += 1)
                        {
                            var call__9 = FChildren[I];
                            C = (TAoWComponent)call__9;
                            if (C.FValid && C.FVisible && FWinLeft + C.FWinLeft <= ARect.Right && FWinLeft + C.FWinLeft + C.FWinWidth >= ARect.Left && FWinTop + C.FWinTop <= ARect.Bottom && FWinTop + C.FWinTop + C.FWinHeight >= ARect.Top && ARect.Left <= ARect.Right && ARect.Top <= ARect.Bottom)
                            {
                                if (C.FDoubleBuffered)
                                {
                                    var call__10 = Rect(ARect.Left - C.FWinLeft, ARect.Top - C.FWinTop, ARect.Right - C.FWinLeft, ARect.Bottom - C.FWinTop);
                                    R = call__10;
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
        }

        // AOWWinManager.pas:239
        // { DrawSurface draws the screen into Surface: first it brings the windows up to date and adds the
//   screen area that a hidden or moved window uncovered to the draw list (only the frame strips for a
//   masked window), then it redraws the background (all of it the first time, else only the dirty
//   rectangles) and asks every window to draw itself; the rectangles each window blitted are passed
//   on to the windows above it, and the foreground texture is drawn again over them when the window
//   is below the overlay priority. FBackRect holds the window's last place as (Left, Top, Width,
//   Height). }
        public override void DrawSurface(TSurface Surface)
        {
            int I = default;
            int J = default;
            TAOWWindowControl C = default;
            TaowRect D = default;
            bool WasValid = default;
            TSurfaceDesc SD = default;
            TRect R = default;
            try
            {
                Setup();
                Update();
                if (FValid)
                {
                    {
                        I = 0;
                        int end__11 = FChildren.Count - 1;
                        for (; I <= end__11; I += 1)
                        {
                            var call__12 = FChildren[I];
                            C = (TAOWWindowControl)call__12;
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
                                    var call__13 = Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                                    R = call__13;
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
                                                var call__14 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FWinLeft + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__14;
                                                AddDrawRect(ref R, C);
                                                var call__15 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FFrameBottom);
                                                R = call__15;
                                                AddDrawRect(ref R, C);
                                                var call__16 = Rect(C.FBackRect.Left, C.FBackRect.Top + C.FFrameBottom, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__16;
                                                AddDrawRect(ref R, C);
                                            }
                                            else
                                            {
                                                var call__17 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FWinLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__17;
                                                AddDrawRect(ref R, C);
                                            }
                                        }

                                        if (C.FBackRect.Top < C.FWinTop)
                                        {
                                            if (C.FMasked)
                                            {
                                                var call__18 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FWinTop + C.FFrameTop);
                                                R = call__18;
                                                AddDrawRect(ref R, C);
                                                var call__19 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__19;
                                                AddDrawRect(ref R, C);
                                                var call__20 = Rect(C.FBackRect.Left + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__20;
                                                AddDrawRect(ref R, C);
                                            }
                                            else
                                            {
                                                var call__21 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FWinTop);
                                                R = call__21;
                                                AddDrawRect(ref R, C);
                                            }
                                        }

                                        if (C.FBackRect.Left + C.FBackRect.Right > C.FWinLeft + C.FWinWidth)
                                        {
                                            if (C.FMasked)
                                            {
                                                var call__22 = Rect(C.FWinLeft + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__22;
                                                AddDrawRect(ref R, C);
                                                var call__23 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FFrameBottom);
                                                R = call__23;
                                                AddDrawRect(ref R, C);
                                                var call__24 = Rect(C.FBackRect.Left, C.FBackRect.Top + C.FFrameBottom, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__24;
                                                AddDrawRect(ref R, C);
                                            }
                                            else
                                            {
                                                var call__25 = Rect(C.FWinLeft + C.FWinWidth, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__25;
                                                AddDrawRect(ref R, C);
                                            }
                                        }

                                        if (C.FBackRect.Top + C.FBackRect.Bottom > C.FWinTop + C.FWinHeight)
                                        {
                                            if (C.FMasked)
                                            {
                                                var call__26 = Rect(C.FBackRect.Left, C.FWinTop + C.FFrameTop, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__26;
                                                AddDrawRect(ref R, C);
                                                var call__27 = Rect(C.FBackRect.Left, C.FBackRect.Top, C.FBackRect.Left + C.FFrameLeft, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__27;
                                                AddDrawRect(ref R, C);
                                                var call__28 = Rect(C.FBackRect.Left + C.FFrameRight, C.FBackRect.Top, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__28;
                                                AddDrawRect(ref R, C);
                                            }
                                            else
                                            {
                                                var call__29 = Rect(C.FBackRect.Left, C.FWinTop + C.FWinHeight, C.FBackRect.Left + C.FBackRect.Right, C.FBackRect.Top + C.FBackRect.Bottom);
                                                R = call__29;
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
                            var call__30 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                            FForegroundTexture.DrawILT(FILIndexTFore, call__30, ref SD);
                        }

                        Surface.Unlock();
                    }
                }
                else
                {
                    if (Surface.Lock(ref SD) >= 0)
                    {
                        {
                            J = 0;
                            int end__31 = FDrawCount - 1;
                            for (; J <= end__31; J += 1)
                            {
                                var call__32 = FDrawList[J];
                                D = (TaowRect)call__32;
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

                                var call__33 = Rect(D.R.Left, D.R.Top, D.R.Right, D.R.Bottom);
                                SD.ClipRect = call__33;
                                Draw(SD);
                                if (FDrawOverlay && FOverlayPriority > 0)
                                {
                                    var call__34 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                                    FForegroundTexture.DrawILT(FILIndexTFore, call__34, ref SD);
                                }

                                InvalidateRect(0, ref D.R, null);
                            }
                        }

                        FDrawCount = 0;
                        Surface.Unlock();
                    }
                }

                {
                    I = 0;
                    int end__35 = FChildren.Count - 1;
                    for (; I <= end__35; I += 1)
                    {
                        var call__36 = FChildren[I];
                        C = (TAOWWindowControl)call__36;
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
                                var call__37 = Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                                R = call__37;
                                AddBltRect(ref R, C);
                            }

                            C.FDrawn = true;
                            C.FSurfaceValid = true;
                            {
                                J = 0;
                                int end__38 = FBltCount - 1;
                                for (; J <= end__38; J += 1)
                                {
                                    var call__39 = FBltList[J];
                                    D = (TaowRect)call__39;
                                    InvalidateRect(I + 1, ref D.R, null);
                                }
                            }

                            if (FDrawOverlay && C.FPriority < FOverlayPriority)
                            {
                                if (Surface.Lock(ref SD) >= 0)
                                {
                                    {
                                        J = 0;
                                        int end__40 = FBltCount - 1;
                                        for (; J <= end__40; J += 1)
                                        {
                                            var call__41 = FBltList[J];
                                            D = (TaowRect)call__41;
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

                                            var call__42 = Rect(D.R.Left, D.R.Top, D.R.Right, D.R.Bottom);
                                            SD.ClipRect = call__42;
                                            var call__43 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                                            FForegroundTexture.DrawILT(FILIndexTFore, call__43, ref SD);
                                        }
                                    }

                                    Surface.Unlock();
                                }
                            }

                            FBltCount = 0;
                        }
                    }
                }

                FDrawn = true;
                ClearDrawList();
                ClearBltList();
            }
            catch (System.Exception)
            {
                MessageBox(0, (AnsiString)"Exception during DrawSurface", (AnsiString)"Error - " + Name, 0);
            }
        }

        // AOWWinManager.pas:157
        public override void Draw(TSurfaceDesc SurfDesc)
        {
            int I = default;
            TAoWComponent C = default;
            if (FOverlayPriority > FNewOverlayPriority)
            {
                {
                    I = 0;
                    int end__44 = FChildren.Count - 1;
                    for (; I <= end__44; I += 1)
                    {
                        var call__45 = FChildren[I];
                        C = (TAoWComponent)call__45;
                        if (C.FPriority >= FNewOverlayPriority && C.FPriority < FOverlayPriority && C.FVisible)
                        {
                            C.FDrawn = false;
                            C.FSurfaceValid = false;
                        }
                    }
                }
            }

            FOverlayPriority = FNewOverlayPriority;
            var call__46 = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
            FBackgroundTexture.DrawILT(FILIndexTBack, call__46, ref SurfDesc);
        }

        // AOWWinManager.pas:449
        public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            int I = default;
            TAoWComponent C = default;
            bool Redraw = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__47 = 0;
                for (; I >= end__47; I -= 1)
                {
                    var call__48 = FChildren[I];
                    C = (TAoWComponent)call__48;
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
            }

            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }
        }

        // AOWWinManager.pas:473
        public override void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
        {
            int I = default;
            TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__49 = 0;
                for (; I >= end__49; I -= 1)
                {
                    var call__50 = FChildren[I];
                    C = (TAoWComponent)call__50;
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
            }

            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }

        // AOWWinManager.pas:494
        public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
        {
            int I = default;
            TAOWWindowControl C = default;
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

            {
                I = FChildren.Count - 1;
                byte end__51 = 0;
                for (; I >= end__51; I -= 1)
                {
                    var call__52 = FChildren[I];
                    C = (TAOWWindowControl)call__52;
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
            int I = default;
            TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__53 = 0;
                for (; I >= end__53; I -= 1)
                {
                    var call__54 = FChildren[I];
                    C = (TAoWComponent)call__54;
                    if (C.FPriority >= FOverlayPriority)
                    {
                        C.CheckKeyPress(ref Key);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }
            }

            if (FOnKeyPress != null)
            {
                FOnKeyPress(this, ref Key);
            }
        }

        // AOWWinManager.pas:578
        public override void CheckKeyDown(ref ushort Key, TSet<TShiftState> Shift)
        {
            int I = default;
            TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__55 = 0;
                for (; I >= end__55; I -= 1)
                {
                    var call__56 = FChildren[I];
                    C = (TAoWComponent)call__56;
                    if (!FHandled && C.FPriority >= FOverlayPriority && C.FVisible)
                    {
                        C.CheckKeyDown(ref Key, Shift);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }
            }

            if (!FHandled && FOnKeyDown != null)
            {
                FOnKeyDown(this, ref Key, Shift);
            }
        }

        // AOWWinManager.pas:102
        public virtual void SetILTB(TInterfaceIL Value)
        {
            if (Value != FBackgroundTexture)
            {
                FValid = false;
                FBackgroundTexture = Value;
            }
        }

        // AOWWinManager.pas:111
        public virtual void SetILTF(TInterfaceIL Value)
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
        public TNotifyEvent OnChangeCursor { get => FOnChangeCursor; set => FOnChangeCursor = value; }

        [Published]
        public TInterfaceIL BackgroundTexture { get => FBackgroundTexture; set => SetILTB(value); }

        [Published]
        public TInterfaceIL ForegroundTexture { get => FForegroundTexture; set => SetILTF(value); }

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
    }

    public static partial class AOWWinManager
    {
        // AOWWinManager.pas:598
        public static void Register()
        {
            RegisterComponents((AnsiString)"AOW Interface", [typeof(TAOWWinManager)]);
        }
    }
}
