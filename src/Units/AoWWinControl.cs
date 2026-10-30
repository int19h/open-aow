// Translated from AoWWinControl.pas.
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

namespace Aow.Units.AoWWinControl
{
    public enum TWindowType : byte
    {
        winNormal = 0,
        winHollow = 1,
        winBorderless = 2,
        winBlank = 3
    }

    // { AoWWinControl.TAOWWindowControl (instance size 464). Fields: +$104 FModal, +$108
//     FOnDrawSurface, +$110 FOnPreDrawSurface, +$118 FDragging, +$11C FDragX, +$120 FDragY, +$124
//     FDraggable, +$128 FResizable, +$12C FResizingX, +$12D FResizingY, +$130 FOnResize, +$138
//     FOnInternalResize, +$140 FDragHeight, +$144 FOverlapTopCorners, +$146 FOldCursor, +$148
//     FWindowCursor, +$14C FOnChangeCursor, +$154 FMinWidth, +$158 FMinHeight, +$15C FMaxWidth, +$160
//     FMaxHeight, +$164 FInResizeArea, +$165 FWindowType, +$168 FILFrame, +$16C FILTexture, +$170
//     FUseManagerBorders, +$174 FFrameLeft, +$178 FFrameTop, +$17C FFrameRight, +$180 FFrameBottom,
//     +$184 FMaskColor, +$188 FUseVMem, +$18C FBackSurface, +$190 (unused), +$194 FBackRect, +$1A4
//     (unused), +$1A8 FOnDrawBack, +$1B0 FOnEnter, +$1B8 FOnExit, +$1C0 FMouseInside, +$1C4
//     FILIndexFrame, +$1C8 FILIndexTexture, +$1CC FDockable.
//     VMT (new): DetachMenuBar, Blt }
    public partial class TAOWWindowControl : Aow.Units.AoWComp.TAoWComponent
    {
        public bool FModal;
        public Aow.Units.AoWWinControl.TAoWOnDrawSurface FOnDrawSurface;
        public Aow.Units.AoWWinControl.TAoWOnDrawSurface FOnPreDrawSurface;
        public byte FDragging;
        public int FDragX;
        public int FDragY;
        public bool FDraggable;
        public bool FResizable;
        public byte FResizingX;
        public byte FResizingY;
        public Aow.Rtl.TNotifyEvent FOnResize;
        public Aow.Rtl.TNotifyEvent FOnInternalResize;
        public int FDragHeight;
        public bool FOverlapTopCorners;
        public int FOldCursor;
        public int FWindowCursor;
        public Aow.Rtl.TNotifyEvent FOnChangeCursor;
        public int FMinWidth;
        public int FMinHeight;
        public int FMaxWidth;
        public int FMaxHeight;
        public bool FInResizeArea;
        public Aow.Units.AoWWinControl.TWindowType FWindowType;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILFrame;
        public Aow.Units.AOWInterfaceImgLib.TInterfaceIL FILTexture;
        public bool FUseManagerBorders;
        public int FFrameLeft;
        public int FFrameTop;
        public int FFrameRight;
        public int FFrameBottom;
        public int FMaskColor;
        public bool FUseVMem;
        public Aow.Units.GFXE.TSurface FBackSurface;
        public int F190;
        public Aow.Rtl.TRect FBackRect;
        public int F1A4;
        public Aow.Units.AoWComp.TAoWOnDrawBackEvent FOnDrawBack;
        public Aow.Rtl.TNotifyEvent FOnEnter;
        public Aow.Rtl.TNotifyEvent FOnExit;
        public bool FMouseInside;
        public int FILIndexFrame;
        public int FILIndexTexture;
        public bool FDockable;
        // AoWWinControl.pas:103
        // { TAOWWindowControl }
        public TAOWWindowControl(TComponent owner) : base(owner)
        {
            FOldCursor = -2;
            FWindowCursor = -2;
        }

        // AoWWinControl.pas:110
        public override void Destroy()
        {
            FBackSurface.Free();
            base.Destroy();
        }

        // AoWWinControl.pas:120
        public override void Validate()
        {
            if (FAOWOwner != null)
            {
                if (FILFrame != null)
                {
                    var call__1 = FILFrame.CheckIL(FILIndexFrame, 8, this);
                    FILFrame = call__1;
                }

                if (FILTexture != null)
                {
                    var call__2 = FILTexture.CheckIL(FILIndexTexture, 1, this);
                    FILTexture = call__2;
                }

                if (FILFrame != null)
                {
                    if (FILTexture != null)
                    {
                        FWindowType = Aow.Units.AoWWinControl.TWindowType.winNormal;
                    }
                    else
                    {
                        FWindowType = Aow.Units.AoWWinControl.TWindowType.winHollow;
                    }
                }
                else
                {
                    if (FILTexture != null)
                    {
                        FWindowType = Aow.Units.AoWWinControl.TWindowType.winBorderless;
                    }
                    else
                    {
                        FWindowType = Aow.Units.AoWWinControl.TWindowType.winBlank;
                    }
                }

                FValid = true;
            }

            base.Validate();
        }

        // AoWWinControl.pas:224
        public override void CheckKeyPress(ref byte Key)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            if (FVisible)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__3 = 0;
                    for (; I >= end__3; I -= 1)
                    {
                        var call__4 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__4;
                        C.CheckKeyPress(ref Key);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if ((FHandled || FModal) && (FOnKeyPress != null))
                {
                    FOnKeyPress(this, ref Key);
                }

                if (FModal)
                {
                    FHandled = true;
                }
            }
        }

        // AoWWinControl.pas:246
        public override void CheckKeyDown(ref ushort Key, TSet<Aow.Rtl.TShiftState> Shift)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            FHandled = false;
            if (FVisible)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__5 = 0;
                    for (; I >= end__5; I -= 1)
                    {
                        var call__6 = FChildren[I];
                        C = (Aow.Units.AoWComp.TAoWComponent)call__6;
                        C.CheckKeyDown(ref Key, Shift);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if (FOnKeyDown != null)
                {
                    FOnKeyDown(this, ref Key, Shift);
                }

                if (FModal)
                {
                    FHandled = true;
                }
            }
        }

        // AoWWinControl.pas:116
        public virtual void DetachMenuBar()
        {
        }

        // AoWWinControl.pas:208
        public virtual void Blt(Aow.Units.GFXE.TSurface Surface)
        {
            Aow.Rtl.TRect R = default;
            if (FOnPreDrawSurface != null)
            {
                FOnPreDrawSurface(this, Surface, FWinLeft, FWinTop);
            }

            var call__7 = Aow.Rtl.Rtl.Rect(0, 0, FWinWidth, FWinHeight);
            R = call__7;
            if (!FMasked)
            {
                FBackSurface.BltAreaClipped(Surface, R, FWinLeft, FWinTop);
            }
            else
            {
                var call__8 = FBackSurface.GetPixelFormatID();
                var call__9 = Aow.Units.GFXE.GFXE.Color2PixelInfo(FMaskColor, call__8, null);
                FBackSurface.BltAreaColorKeyClipped(Surface, R, FWinLeft, FWinTop, call__9);
            }

            if (FOnDrawSurface != null)
            {
                FOnDrawSurface(this, Surface, FWinLeft, FWinTop);
            }
        }

        // AoWWinControl.pas:142
        public void SetOverlap(bool Value)
        {
            FOverlapTopCorners = Value;
            FDrawn = false;
        }

        // AoWWinControl.pas:149
        // { RadioChange unchecks the other children of the radio group of Sender }
        public void RadioChange(Aow.Units.AoWComp.TAoWComponent Sender)
        {
            int I = default;
            Aow.Units.AoWComp.TAoWComponent C = default;
            {
                I = 0;
                int end__10 = FChildren.Count - 1;
                for (; I <= end__10; I += 1)
                {
                    var call__11 = FChildren[I];
                    C = (Aow.Units.AoWComp.TAoWComponent)call__11;
                    if (((Sender.FRadioIndex == C.FRadioIndex) && (C != Sender)) && (C.FChecked == true))
                    {
                        C.FChecked = false;
                        C.FDrawn = false;
                        if (C.FOnChange != null)
                        {
                            C.FOnChange(C);
                        }
                    }
                }
            }
        }

        // AoWWinControl.pas:170
        // { VerifyBackSurface makes the back surface match Surface (a DirectDraw surface in video memory
//   when Surface is one and UseVMem is set, else a surface in system memory or a data surface);
//   True when the back surface was created or restored and has to be drawn again }
        public bool VerifyBackSurface(Aow.Units.GFXE.TSurface Surface)
        {
            bool Result = default;
            TSet<Aow.Units.GFXE.TDDSCreationFlagsElement> Flags = default;
            Aow.Units.GFXE.TDDrawSurface DDS = default;
            Result = false;
            if (FDoubleBuffered)
            {
                if (FBackSurface != null)
                {
                    var call__12 = FBackSurface.GetPixelFormatID();
                    var call__13 = Surface.GetPixelFormatID();
                    if ((call__12 != call__13) || ((FBackSurface is Aow.Units.GFXE.TDDrawSurface) ^ (Surface is Aow.Units.GFXE.TDDrawSurface)))
                    {
                        FBackSurface.Free();
                        FBackSurface = null;
                    }
                }

                if (FBackSurface == null)
                {
                    if (Surface is Aow.Units.GFXE.TDDrawSurface)
                    {
                        if (((Aow.Units.GFXE.TDDrawSurface)Surface).Status.Contains(Aow.Units.GFXE.TDDrawSurfaceStatusElement.ssVideoMemory) && FUseVMem)
                        {
                            Flags = TSet<Aow.Units.GFXE.TDDSCreationFlagsElement>.Of(Aow.Units.GFXE.TDDSCreationFlagsElement.cfVideoMemory, Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory);
                        }
                        else
                        {
                            Flags = TSet<Aow.Units.GFXE.TDDSCreationFlagsElement>.Of(Aow.Units.GFXE.TDDSCreationFlagsElement.cfSystemMemory);
                        }

                        DDS = new Aow.Units.GFXE.TDDrawSurface();
                        FBackSurface = DDS;
                        var call__14 = Surface.GetPixelFormatID();
                        DDS.CreateSurface(FWinWidth, FWinHeight, call__14, Flags);
                    }
                    else
                    {
                        var call__15 = Surface.GetPixelFormatID();
                        var call__16 = new Aow.Units.GFXE.TDataSurface(FWinWidth, FWinHeight, call__15, null);
                        FBackSurface = call__16;
                    }

                    if (FMasked)
                    {
                        FBackSurface.Cls(FMaskColor);
                    }

                    Result = true;
                }
                else
                {
                    bool bool__17 = (FBackSurface is Aow.Units.GFXE.TDDrawSurface);
                    if (bool__17)
                    {
                        var call__18 = ((Aow.Units.GFXE.TDDrawSurface)FBackSurface).Lost();
                        bool__17 = call__18;
                    }

                    if (bool__17)
                    {
                        var call__19 = ((Aow.Units.GFXE.TDDrawSurface)FBackSurface).Restore();
                        Result = call__19 == Aow.Units.GFXE.GFXE.DD_OK;
                    }
                }
            }

            return Result;
        }

        [Published]
        public Aow.Units.AoWComp.TAoWOnDrawBackEvent OnDrawBack { get => FOnDrawBack; set => FOnDrawBack = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnEnter { get => FOnEnter; set => FOnEnter = value; }

        [Published]
        public Aow.Rtl.TNotifyEvent OnExit { get => FOnExit; set => FOnExit = value; }
    }

    public delegate void TAoWOnDrawSurface(Aow.Units.AoWWinControl.TAOWWindowControl Sender, Aow.Units.GFXE.TSurface Surface, int Left, int Top);
    public static partial class AoWWinControl
    {
    }
}
