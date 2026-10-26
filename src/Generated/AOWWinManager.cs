// Generated from AOWWinManager.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
using System;
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Exception = Aow.Rtl.Exception;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.ImageLib;
using static Aow.Units.ImageLib.ImageLib;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;

namespace Aow.Units.AOWWinManager
{
    // { The AOWWinManager unit of aowInt.dpl: TAOWWinManager, the root of a screen of AoW windows. It
//   owns the windows (its children, sorted by priority), draws the background and foreground
//   textures and the windows into the display surface, keeps the dirty rectangles of the whole
//   screen and passes the mouse and keyboard input to the windows from the top down. }
    // { AOWWinManager.TAOWWinManager (instance size 316). Fields: +$104 FForegroundTexture, +$108
//     FBackgroundTexture, +$10C FOverlayPriority (windows below it get no input), +$110
//     FNewOverlayPriority (applied by Draw), +$114 FBorderLeft, +$118 FBorderTop, +$11C
//     FBorderRight, +$120 FBorderBottom, +$124 FOldCursor (the cursor before CheckMouseMove), +$126
//     FDefaultCursor (the cursor when no window claims the mouse), +$128 FOnChangeCursor, +$130
//     FILIndexTBack, +$134 FILIndexTFore, +$138 FDrawOverlay (draw the foreground texture over
//     the windows below the overlay priority).
//     VMT (new): SetILTB, SetILTF }
    public partial class TAOWWinManager : Aow.Units.AoWComp.TAoWComponent
    {
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FForegroundTexture;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FBackgroundTexture;
        public int FOverlayPriority;
        public int FNewOverlayPriority;
        public int FBorderLeft;
        public int FBorderTop;
        public int FBorderRight;
        public int FBorderBottom;
        public int FOldCursor;
        public int FDefaultCursor;
        public Aow.Rtl.TNotifyEvent FOnChangeCursor;
        public int FILIndexTBack;
        public int FILIndexTFore;
        public bool FDrawOverlay;
        // AOWWinManager.pas:81
        // { TAOWWinManager }
        public override void Create__init(Aow.Rtl.TComponent AOwner)
        {
            base.Create__init(AOwner);
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
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AOWWinManager.pas:204
        // { InvalidateRect passes ARect (screen coordinates) on to the windows from Index up that it touches
//   (a rectangle from a window only when that window is not below the overlay priority): a
//   double-buffered window adds it, in its own coordinates, to its blit list, any other window adds
//   it to its draw list. The window itself is passed as the Sender. }
        public override void InvalidateRect(int Index, ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            Aow.Rtl.TRect R = default;
            // { Tried Self identity, receiver scope and local/field staging; register/stack allocation still differs. }
            if (FValid && FVisible)
            {
                bool bool__3 = (Sender == null);
                if (!bool__3)
                {
                    bool bool__4 = (Sender != null) && (Sender is Aow.Units.AoWWinControl.TAOWWindowControl);
                    if (bool__4)
                    {
                        var call__5 = FChildren[Index];
                        bool__4 = (((Aow.Units.AoWComp.TAoWComponent)call__5).FPriority >= FOverlayPriority);
                    }

                    bool__3 = bool__4;
                }

                if (bool__3)
                {
                    {
                        I = Index;
                        int end__6 = FChildren.Count - 1;
                        for (; I <= end__6; I += 1)
                        {
                            var call__7 = FChildren[I];
                            C = (Aow.Units.AoWComp.TAoWComponent)call__7;
                            if (((((((C.FValid && C.FVisible) && ((FWinLeft + C.FWinLeft) <= ARect.Right)) && (((FWinLeft + C.FWinLeft) + C.FWinWidth) >= ARect.Left)) && ((FWinTop + C.FWinTop) <= ARect.Bottom)) && (((FWinTop + C.FWinTop) + C.FWinHeight) >= ARect.Top)) && (ARect.Left <= ARect.Right)) && (ARect.Top <= ARect.Bottom))
                            {
                                if (C.FDoubleBuffered)
                                {
                                    var call__8 = Aow.Rtl.Rtl.Rect(ARect.Left - C.FWinLeft, ARect.Top - C.FWinTop, ARect.Right - C.FWinLeft, ARect.Bottom - C.FWinTop);
                                    R = call__8;
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
        public override void DrawSurface(Aow.Units.GFXE.TSurface Surface)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AOWWinManager.pas:157
        public override void Draw(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            if (FOverlayPriority > FNewOverlayPriority)
            {
                {
                    I = 0;
                    int end__9 = FChildren.Count - 1;
                    for (; I <= end__9; I += 1)
                    {
                        var call__10 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__10;
                        if (((C.FPriority >= FNewOverlayPriority) && (C.FPriority < FOverlayPriority)) && C.FVisible)
                        {
                            C.FDrawn = false;
                            C.FSurfaceValid = false;
                        }
                    }
                }
            }

            FOverlayPriority = FNewOverlayPriority;
            var call__11 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
            FBackgroundTexture.DrawILT(FILIndexTBack, call__11, ref SurfDesc);
        }

        // AOWWinManager.pas:449
        public override void CheckMouseDown(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            bool Redraw = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__12 = 0;
                for (; I >= end__12; I -= 1)
                {
                    var call__13 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__13;
                    if (C.FPriority >= FOverlayPriority)
                    {
                        Redraw = C.FRedraw;
                        C.FHandled = false;
                        if ((FHandled == false) || (Redraw == true))
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
        public override void CheckMouseUp(Aow.Rtl.TMouseButton Button, TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__14 = 0;
                for (; I >= end__14; I -= 1)
                {
                    var call__15 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__15;
                    if (C.FPriority >= FOverlayPriority)
                    {
                        if ((FHandled == false) || (C.FRedraw == true))
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
        public override void CheckMouseMove(TSet<Aow.Rtl.TShiftState> Shift, int X, int Y)
        {
            int I = default;
            Aow.Units.AoWWinControl.TAOWWindowControl C = default;
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
                byte end__16 = 0;
                for (; I >= end__16; I -= 1)
                {
                    var call__17 = FChildren[I];
                    C = (Aow.Units.AoWWinControl.TAOWWindowControl)call__17;
                    if (C.FPriority >= FOverlayPriority)
                    {
                        if ((FHandled == false) || (C.FRedraw == true))
                        {
                            C.CheckMouseMove(Shift, X, Y);
                            if (C.FHandled == true)
                            {
                                if (FHandled == false)
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
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__18 = 0;
                for (; I >= end__18; I -= 1)
                {
                    var call__19 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__19;
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
        public override void CheckKeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            {
                I = FChildren.Count - 1;
                byte end__20 = 0;
                for (; I >= end__20; I -= 1)
                {
                    var call__21 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__21;
                    if ((!FHandled && (C.FPriority >= FOverlayPriority)) && C.FVisible)
                    {
                        C.CheckKeyDown(ref Key, Shift);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }
            }

            if (!FHandled && (FOnKeyDown != null))
            {
                FOnKeyDown(this, ref Key, Shift);
            }
        }

        // AOWWinManager.pas:102
        public virtual void SetILTB(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FBackgroundTexture)
            {
                FValid = false;
                FBackgroundTexture = Value;
            }
        }

        // AOWWinManager.pas:111
        public virtual void SetILTF(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
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
            if ((Value != FOverlayPriority) || (Value != FNewOverlayPriority))
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
        public Aow.Rtl.TNotifyEvent OnChangeCursor { get => FOnChangeCursor; set => FOnChangeCursor = value; }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL BackgroundTexture { get => FBackgroundTexture; set => SetILTB(value); }

        [Published]
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ForegroundTexture { get => FForegroundTexture; set => SetILTF(value); }

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

        // AOWWinManager.pas:39
        public static new Aow.Units.AOWWinManager.TAOWWinManager Create(Aow.Rtl.TComponent AOwner)
        {
            Aow.Units.AOWWinManager.TAOWWinManager o = new Aow.Units.AOWWinManager.TAOWWinManager();
            try
            {
                o.Create__init(AOwner);
            }
            catch
            {
                o.Destroy();
                throw;
            }

            return o;
        }
    }

    public static partial class AOWWinManager
    {
        // AOWWinManager.pas:598
        public static void Register()
        {
            Aow.Rtl.Rtl.RegisterComponents((AnsiString)"AOW Interface", [typeof(Aow.Units.AOWWinManager.TAOWWinManager)]);
        }
    }
}
