// Translated from AoWBaseWin.pas.
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
        public TAOWBaseWindow(TComponent owner) : base(owner)
        { }

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
            int I = default;
            int J = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            Aow.Units.AoWComp.TaowRect D = default;
            int OldLeft = default;
            int OldTop = default;
            bool Locked = default;
            Aow.Units.GFXE.TSurfaceDesc SD = default;
            Aow.Rtl.TRect R = default;
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
                            var call__4 = VerifyBackSurface(Surface);
                            FDrawn = !call__4 && FDrawn;
                            FWinLeft = 0;
                            FWinTop = 0;
                            var call__5 = FBackSurface.Lock(ref SD);
                            Locked = call__5 == 0;
                        }
                        else
                        {
                            Locked = Surface.Lock(ref SD) == 0;
                        }
                    }
                    catch (System.Exception)
                    {
                        Locked = false;
                        Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Verify Surface Error", ((AnsiString)"Error - ") + Name, (int)0);
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
                                    {
                                        J = 0;
                                        int end__6 = FDrawCount - 1;
                                        for (; J <= end__6; J += 1)
                                        {
                                            var call__7 = FDrawList[J];
                                            D = (Aow.Units.AoWComp.TaowRect)call__7;
                                            R = SD.ClipRect;
                                            var call__8 = Aow.Rtl.Rtl.Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                                            SD.ClipRect = call__8;
                                            Draw(SD);
                                            SD.ClipRect = R;
                                            InvalidateRect(0, ref D.R, null);
                                        }
                                    }
                                }
                            }

                            FDrawCount = 0;
                        }
                        catch (System.Exception)
                        {
                            Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Draw Error", ((AnsiString)"Error - ") + Name, (int)0);
                        }

                        if (FDoubleBuffered)
                        {
                            if ((FPriority >= FWinManager.FOverlayPriority) || !FDrawn)
                            {
                                {
                                    I = 0;
                                    int end__9 = FChildren.Count - 1;
                                    for (; I <= end__9; I += 1)
                                    {
                                        var call__10 = FChildren[I];
                                        C = (Aow.Units.AoWComp.TAoWComponent)call__10;
                                        if ((C.FValid && C.FVisible) && (!FDrawn || !C.FDrawn))
                                        {
                                            if (!FDrawn && (C is Aow.Units.AoWBaseWin.TAOWBaseWindow))
                                            {
                                                C.FSurfaceValid = false;
                                            }

                                            C.FDrawn = false;
                                            C.Draw(SD);
                                            {
                                                J = 0;
                                                int end__11 = FDrawCount - 1;
                                                for (; J <= end__11; J += 1)
                                                {
                                                    var call__12 = FDrawList[J];
                                                    D = (Aow.Units.AoWComp.TaowRect)call__12;
                                                    InvalidateRect(I + 1, ref D.R, null);
                                                }
                                            }

                                            FDrawCount = 0;
                                            if (!C.FDrawn || !FDrawn)
                                            {
                                                if (FSurfaceValid)
                                                {
                                                    var call__13 = Aow.Rtl.Rtl.Rect(C.FWinLeft, C.FWinTop, C.FWinLeft + C.FWinWidth, C.FWinTop + C.FWinHeight);
                                                    R = call__13;
                                                    AddBltRect(ref R, C);
                                                }

                                                C.FFirstDraw = C.FDrawn;
                                                var call__14 = Aow.Rtl.Rtl.Rect(FWinLeft + C.FWinLeft, FWinTop + C.FWinTop, ((FWinLeft + C.FWinLeft) + C.FWinWidth) - 1, ((FWinTop + C.FWinTop) + C.FWinHeight) - 1);
                                                R = call__14;
                                                InvalidateRect(I + 1, ref R, C);
                                            }

                                            C.FDrawn = true;
                                        }
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

                                        {
                                            J = 0;
                                            int end__15 = FBltCount - 1;
                                            for (; J <= end__15; J += 1)
                                            {
                                                var call__16 = FBltList[J];
                                                D = (Aow.Units.AoWComp.TaowRect)call__16;
                                                var call__17 = Aow.Rtl.Rtl.Rect(D.R.Left, D.R.Top, D.R.Right + 1, D.R.Bottom + 1);
                                                R = call__17;
                                                if (!FMasked)
                                                {
                                                    FBackSurface.BltAreaClipped(Surface, R, D.R.Left + OldLeft, D.R.Top + OldTop);
                                                }
                                                else
                                                {
                                                    var call__18 = FBackSurface.GetPixelFormatID();
                                                    var call__19 = Aow.Units.GFXE.GFXE.Color2PixelInfo(FMaskColor, call__18, null);
                                                    FBackSurface.BltAreaColorKeyClipped(Surface, R, D.R.Left + OldLeft, D.R.Top + OldTop, call__19);
                                                }

                                                var call__20 = Aow.Rtl.Rtl.Rect(D.R.Left + OldLeft, D.R.Top + OldTop, D.R.Right + OldLeft, D.R.Bottom + OldTop);
                                                R = call__20;
                                                FWinManager.AddBltRect(ref R, this);
                                            }
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
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Blt Error", ((AnsiString)"Error - ") + Name, (int)0);
                            }
                        }
                        else
                        {
                            {
                                I = 0;
                                int end__21 = FChildren.Count - 1;
                                for (; I <= end__21; I += 1)
                                {
                                    var call__22 = FChildren[I];
                                    C = (Aow.Units.AoWComp.TAoWComponent)call__22;
                                    if ((!FDrawn || !C.FDrawn) || (C is Aow.Units.AoWBaseWin.TAOWBaseWindow))
                                    {
                                        C.Draw(SD);
                                        C.FDrawn = true;
                                    }
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
                            catch (System.Exception)
                            {
                                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"SB Draw Events Error", ((AnsiString)"Error - ") + Name, (int)0);
                            }
                        }

                        FDrawn = true;
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during DrawSurface", ((AnsiString)"Error - ") + Name, (int)0);
            }
        }

        // AoWBaseWin.pas:263
        // { DrawSurfDesc draws the window and its controls into SurfDesc. A double-buffered window draws
//   into its back surface instead (created, or created again when the pixel format changed) and
//   copies it into SurfDesc, masked with FMaskColor for a masked window. }
        public override void DrawSurfDesc(Aow.Units.GFXE.TSurfaceDesc SurfDesc)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            int OldLeft = default;
            int OldTop = default;
            bool Ok = default;
            Aow.Units.GFXE.TSurfaceDesc DestSD = default;
            Aow.Rtl.TRect SrcRect = default;
            Aow.Rtl.TRect DestRect = default;
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
                        var call__23 = new Aow.Units.GFXE.TDataSurface(FWinWidth, FWinHeight, SurfDesc.PixelFormatID, null);
                        FBackSurface = call__23;
                        if (FMasked)
                        {
                            FBackSurface.Cls(FMaskColor);
                        }

                        var call__24 = FBackSurface.Lock(ref SurfDesc);
                        Ok = call__24 >= 0;
                    }
                    else
                    {
                        var call__25 = FBackSurface.Lock(ref SurfDesc);
                        Ok = call__25 >= 0;
                        if (Ok)
                        {
                            if (SurfDesc.PixelFormatID != DestSD.PixelFormatID)
                            {
                                FBackSurface.Unlock();
                                FBackSurface.Free();
                                var call__26 = new Aow.Units.GFXE.TDataSurface(FWinWidth, FWinHeight, DestSD.PixelFormatID, null);
                                FBackSurface = call__26;
                                if (FMasked)
                                {
                                    FBackSurface.Cls(FMaskColor);
                                }

                                var call__27 = FBackSurface.Lock(ref SurfDesc);
                                Ok = call__27 >= 0;
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
                    {
                        I = 0;
                        int end__28 = FChildren.Count - 1;
                        for (; I <= end__28; I += 1)
                        {
                            var call__29 = FChildren[I];
                            C = (Aow.Units.AoWComp.TAoWComponent)call__29;
                            if ((C.FValid && C.FVisible) && (!FDrawn || !C.FDrawn))
                            {
                                C.Draw(SurfDesc);
                                C.FDrawn = true;
                            }
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
                        var call__30 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
                        SrcRect = call__30;
                        // { not used }
                        var call__31 = Aow.Rtl.Rtl.Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth, FWinTop + FWinHeight);
                        DestRect = call__31;
                        try
                        {
                            if (FMasked == true)
                            {
                                Aow.Units.GFXE.GFXE.BltSpriteClipped16(SurfDesc, SrcRect, DestSD, FWinLeft, FWinTop, FMaskColor);
                            }
                            else
                            {
                                Aow.Units.GFXE.GFXE.BltSDRectClipped(SurfDesc, SrcRect, DestSD, FWinLeft, FWinTop);
                            }
                        }
                        catch (System.Exception)
                        {
                        }

                        FBackSurface.Unlock();
                    }
                }
            }
            catch (System.Exception)
            {
                Aow.Rtl.Rtl.MessageBox((int)0, (AnsiString)"Exception during DrawSurfaceDesc", ((AnsiString)"Error - ") + Name, (int)0);
            }
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
