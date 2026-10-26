// Generated from AoWBaseWin.pas by OpenAoW transpiler 1.4c (2026-10-26). Never edit by hand.
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
using Aow.Units.AOWWinManager;
using static Aow.Units.AOWWinManager.AOWWinManager;
using Aow.Units.AoWComp;
using static Aow.Units.AoWComp.AoWComp;
using Aow.Units.AOWInterfaceImgLib;
using static Aow.Units.AOWInterfaceImgLib.AOWInterfaceImgLib;
using Aow.Units.AoWWinControl;
using static Aow.Units.AoWWinControl.AoWWinControl;

namespace Aow.Units.AoWBaseWin
{
    // { The AoWBaseWin unit of aowInt.dpl: TAOWBaseWindow, a window of the window manager. It draws
//   itself and its controls into its back surface (DrawSurface) or straight into the display surface
//   (DrawSurfDesc), redrawing only the dirty rectangles, and passes invalidated rectangles on to the
//   windows above it and to its owner. }
    // { AoWBaseWin.TAOWBaseWindow (instance size 492). Fields: +$1D0 FBuffer (freed by
//     TAOWGenericWindow.Destroy), +$1D4 FFreeSurfaceOnHide, +$1D8 FWinManager, +$1DC FOnShow, +$1E4
//     FOnHide.
//     VMT (new): SetILFrame, SetILTexture }
    public partial class TAOWBaseWindow : Aow.Units.AoWWinControl.TAOWWindowControl
    {
        public Aow.Rtl.TObject FBuffer;
        public bool FFreeSurfaceOnHide;
        public Aow.Units.AOWWinManager.TAOWWinManager FWinManager;
        public Aow.Rtl.TNotifyEvent FOnShow;
        public Aow.Rtl.TNotifyEvent FOnHide;
        // AoWBaseWin.pas:56
        public override void InvalidateRect(int Index, ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            // { Tried Self identity, receiver scope and local/field staging; register/stack allocation still differs. }
            if (((Index > -1) && FValid) && FVisible)
            {
                {
                    I = Index;
                    int end__1 = FChildren.Count - 1;
                    for (; I <= end__1; I += 1)
                    {
                        var call__2 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__2;
                        if ((((((C.FValid && C.FVisible) && (ARect.Right >= C.FWinLeft)) && ((C.FWinLeft + C.FWinWidth) >= ARect.Left)) && (ARect.Bottom >= C.FWinTop)) && ((C.FWinTop + C.FWinHeight) >= ARect.Top)) && (C != Sender))
                        {
                            if (C is Aow.Units.AoWBaseWin.TAOWBaseWindow)
                            {
                                C.InvalidateRect(0, ref ARect, this);
                            }
                            else
                            {
                                C.FDrawn = false;
                            }
                        }
                    }
                }

                if ((FAOWOwner != Sender) && (FAOWOwner is Aow.Units.AoWBaseWin.TAOWBaseWindow))
                {
                    var call__3 = FAOWOwner.FChildren.IndexOf(this);
                    FAOWOwner.InvalidateRect(call__3 + 1, ref ARect, this);
                }
            }
        }

        // AoWBaseWin.pas:50
        // { TAOWBaseWindow }
        public override void AddBltRect(ref Aow.Rtl.TRect ARect, Aow.Units.AoWComp.TAoWComponent Sender)
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
            // { Tried Self identity and typed/untyped receiver scopes; Self register still differs. }
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

                    if (FFreeSurfaceOnHide && (FBackSurface != null))
                    {
                        FBackSurface.Free();
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

                    if ((!FValid || !FAligned) || !FSized)
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
        public override void DrawSurface(Aow.Units.GFXE.TSurface Surface)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWBaseWin.pas:263
        // { DrawSurfDesc draws the window and its controls into SurfDesc. A double-buffered window draws
//   into its back surface instead (created, or created again when the pixel format changed) and
//   copies it into SurfDesc, masked with FMaskColor for a masked window. }
        public override void DrawSurfDesc(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            /* TODO(T-shim) Platform or editor boundary Windows.MessageBox */
            throw new System.NotImplementedException();
        }

        // AoWBaseWin.pas:341
        public virtual void SetILFrame(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
        {
            if (Value != FILFrame)
            {
                FValid = false;
                FILFrame = Value;
            }
        }

        // AoWBaseWin.pas:350
        public virtual void SetILTexture(Aow.Units.AOWInterfaceImgLib.TInterfaceIL Value)
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
        public void SetManager(Aow.Units.AOWWinManager.TAOWWinManager Value)
        {
            SetOwner(Value);
        }

        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILFrame { get => FILFrame; set => SetILFrame(value); }
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL ILTexture { get => FILTexture; set => SetILTexture(value); }
        public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }
        public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

        [Published]
        public Aow.Rtl.TNotifyEvent OnHide { get => FOnHide; set => FOnHide = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnShow { get => FOnShow; set => FOnShow = value; }
    }

    public static partial class AoWBaseWin
    {
    }
}
