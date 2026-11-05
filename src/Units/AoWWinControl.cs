// Translated from AoWWinControl.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWWinControl
{
    public enum TWindowType : byte
    {
        winNormal = 0,
        winHollow = 1,
        winBorderless = 2,
        winBlank = 3
    }

    // Fields: FModal, FOnDrawSurface, FOnPreDrawSurface, FDragging, FDragX, FDragY, FDraggable,
    // FResizable, FResizingX, FResizingY, FOnResize, FOnInternalResize, FDragHeight,
    // FOverlapTopCorners, FOldCursor, FWindowCursor, FOnChangeCursor, FMinWidth, FMinHeight,
    // FMaxWidth, FMaxHeight, FInResizeArea, FWindowType, FILFrame, FILTexture, FUseManagerBorders,
    // FFrameLeft, FFrameTop, FFrameRight, FFrameBottom, FMaskColor, FUseVMem, FBackSurface,
    // (unused), FBackRect, (unused), FOnDrawBack, FOnEnter, FOnExit, FMouseInside, FILIndexFrame,
    // FILIndexTexture, FDockable
    public partial class TAOWWindowControl : TAoWComponent
    {
        public bool FModal;
        public TAoWOnDrawSurface FOnDrawSurface;
        public TAoWOnDrawSurface FOnPreDrawSurface;
        public byte FDragging;
        public int FDragX;
        public int FDragY;
        public bool FDraggable;
        public bool FResizable;
        public byte FResizingX;
        public byte FResizingY;
        public TNotifyEvent FOnResize;
        public TNotifyEvent FOnInternalResize;
        public int FDragHeight;
        public bool FOverlapTopCorners;
        public int FOldCursor;
        public int FWindowCursor;
        public TNotifyEvent FOnChangeCursor;
        public int FMinWidth;
        public int FMinHeight;
        public int FMaxWidth;
        public int FMaxHeight;
        public bool FInResizeArea;
        public TWindowType FWindowType;
        public TInterfaceIL FILFrame;
        public TInterfaceIL FILTexture;
        public bool FUseManagerBorders;
        public int FFrameLeft;
        public int FFrameTop;
        public int FFrameRight;
        public int FFrameBottom;
        public int FMaskColor;
        public bool FUseVMem;
        public TSurface FBackSurface;
        public int F190;
        public TRect FBackRect;
        public int F1A4;
        public TAoWOnDrawBackEvent FOnDrawBack;
        public TNotifyEvent FOnEnter;
        public TNotifyEvent FOnExit;
        public bool FMouseInside;
        public int FILIndexFrame;
        public int FILIndexTexture;
        public bool FDockable;
        // AoWWinControl.pas:103
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
                        FWindowType = TWindowType.winNormal;
                    }
                    else
                    {
                        FWindowType = TWindowType.winHollow;
                    }
                }
                else
                {
                    if (FILTexture != null)
                    {
                        FWindowType = TWindowType.winBorderless;
                    }
                    else
                    {
                        FWindowType = TWindowType.winBlank;
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
            TAoWComponent C = default;
            FHandled = false;
            if (FVisible)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__3 = 0;
                    for (; I >= end__3; I -= 1)
                    {
                        var call__4 = FChildren[I];
                        C = (TAoWComponent)call__4;
                        C.CheckKeyPress(ref Key);
                        if (C.FHandled == true)
                        {
                            FHandled = true;
                        }
                    }
                }

                if ((FHandled || FModal) && FOnKeyPress != null)
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
        public override void CheckKeyDown(ref ushort Key, TSet<TShiftState> Shift)
        {
            int I = default;
            TAoWComponent C = default;
            FHandled = false;
            if (FVisible)
            {
                {
                    I = FChildren.Count - 1;
                    byte end__5 = 0;
                    for (; I >= end__5; I -= 1)
                    {
                        var call__6 = FChildren[I];
                        C = (TAoWComponent)call__6;
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
        public virtual void Blt(TSurface Surface)
        {
            TRect R = default;
            if (FOnPreDrawSurface != null)
            {
                FOnPreDrawSurface(this, Surface, FWinLeft, FWinTop);
            }

            var call__7 = Rect(0, 0, FWinWidth, FWinHeight);
            R = call__7;
            if (!FMasked)
            {
                FBackSurface.BltAreaClipped(Surface, R, FWinLeft, FWinTop);
            }
            else
            {
                var call__8 = FBackSurface.GetPixelFormatID();
                var call__9 = Color2PixelInfo(FMaskColor, call__8, null);
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
        public void RadioChange(TAoWComponent Sender)
        {
            int I = default;
            TAoWComponent C = default;
            {
                I = 0;
                int end__10 = FChildren.Count - 1;
                for (; I <= end__10; I += 1)
                {
                    var call__11 = FChildren[I];
                    C = (TAoWComponent)call__11;
                    if (Sender.FRadioIndex == C.FRadioIndex && C != Sender && C.FChecked == true)
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
        public bool VerifyBackSurface(TSurface Surface)
        {
            bool Result = default;
            TSet<TDDSCreationFlagsElement> Flags = default;
            TDDrawSurface DDS = default;
            Result = false;
            if (FDoubleBuffered)
            {
                if (FBackSurface != null)
                {
                    var call__12 = FBackSurface.GetPixelFormatID();
                    var call__13 = Surface.GetPixelFormatID();
                    if (call__12 != call__13 || (FBackSurface is TDDrawSurface) ^ (Surface is TDDrawSurface))
                    {
                        FBackSurface.Free();
                        FBackSurface = null;
                    }
                }

                if (FBackSurface == null)
                {
                    if (Surface is TDDrawSurface)
                    {
                        if (((TDDrawSurface)Surface).Status.Contains(TDDrawSurfaceStatusElement.ssVideoMemory) && FUseVMem)
                        {
                            Flags = TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfVideoMemory, TDDSCreationFlagsElement.cfSystemMemory);
                        }
                        else
                        {
                            Flags = TSet<TDDSCreationFlagsElement>.Of(TDDSCreationFlagsElement.cfSystemMemory);
                        }

                        DDS = new TDDrawSurface();
                        FBackSurface = DDS;
                        var call__14 = Surface.GetPixelFormatID();
                        DDS.CreateSurface(FWinWidth, FWinHeight, call__14, Flags);
                    }
                    else
                    {
                        var call__15 = Surface.GetPixelFormatID();
                        var call__16 = new TDataSurface(FWinWidth, FWinHeight, call__15, null);
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
                    bool bool__17 = FBackSurface is TDDrawSurface;
                    if (bool__17)
                    {
                        var call__18 = ((TDDrawSurface)FBackSurface).Lost();
                        bool__17 = call__18;
                    }

                    if (bool__17)
                    {
                        var call__19 = ((TDDrawSurface)FBackSurface).Restore();
                        Result = call__19 == DD_OK;
                    }
                }
            }

            return Result;
        }

        [Published]
        public TAoWOnDrawBackEvent OnDrawBack { get => FOnDrawBack; set => FOnDrawBack = value; }

        [Published]
        public TNotifyEvent OnEnter { get => FOnEnter; set => FOnEnter = value; }

        [Published]
        public TNotifyEvent OnExit { get => FOnExit; set => FOnExit = value; }
    }

    public delegate void TAoWOnDrawSurface(TAOWWindowControl Sender, TSurface Surface, int Left, int Top);
    public static partial class AoWWinControl
    {
    }
}
