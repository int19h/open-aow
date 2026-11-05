// Translated from AoWWinControl.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using static Aow.Units.GFXE.GFXE;
using Aow.Units.AoWComp;
using Aow.Units.AOWInterfaceImgLib;

namespace Aow.Units.AoWWinControl;

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
                FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
            }

            if (FILTexture != null)
            {
                FILTexture = FILTexture.CheckIL(FILIndexTexture, 1, this);
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
        TAoWComponent C = default;
        FHandled = false;
        if (FVisible)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TAoWComponent)FChildren[I];
                C.CheckKeyPress(ref Key);
                if (C.FHandled == true)
                {
                    FHandled = true;
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
        TAoWComponent C = default;
        FHandled = false;
        if (FVisible)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (TAoWComponent)FChildren[I];
                C.CheckKeyDown(ref Key, Shift);
                if (C.FHandled == true)
                {
                    FHandled = true;
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

        R = Rect(0, 0, FWinWidth, FWinHeight);
        if (!FMasked)
        {
            FBackSurface.BltAreaClipped(Surface, R, FWinLeft, FWinTop);
        }
        else
        {
            var pixelFormatID = FBackSurface.GetPixelFormatID();
            var color2PixelInfo = Color2PixelInfo(FMaskColor, pixelFormatID, null);
            FBackSurface.BltAreaColorKeyClipped(Surface, R, FWinLeft, FWinTop, color2PixelInfo);
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
        TAoWComponent C;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            C = (TAoWComponent)FChildren[I];
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

    // AoWWinControl.pas:170
    // { VerifyBackSurface makes the back surface match Surface (a DirectDraw surface in video memory
//   when Surface is one and UseVMem is set, else a surface in system memory or a data surface);
//   True when the back surface was created or restored and has to be drawn again }
    public bool VerifyBackSurface(TSurface Surface)
    {
        TSet<TDDSCreationFlagsElement> Flags;
        TDDrawSurface DDS;
        bool Result = false;
        if (FDoubleBuffered)
        {
            if (FBackSurface != null)
            {
                if (FBackSurface.GetPixelFormatID() != Surface.GetPixelFormatID() || (FBackSurface is TDDrawSurface) ^ (Surface is TDDrawSurface))
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
                    var pixelFormatID = Surface.GetPixelFormatID();
                    DDS.CreateSurface(FWinWidth, FWinHeight, pixelFormatID, Flags);
                }
                else
                {
                    var pixelFormatID = Surface.GetPixelFormatID();
                    FBackSurface = new TDataSurface(FWinWidth, FWinHeight, pixelFormatID, null);
                }

                if (FMasked)
                {
                    FBackSurface.Cls(FMaskColor);
                }

                Result = true;
            }
            else
            {
                if (FBackSurface is TDDrawSurface && ((TDDrawSurface)FBackSurface).Lost())
                {
                    Result = ((TDDrawSurface)FBackSurface).Restore() == DD_OK;
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
