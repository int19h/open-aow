// Translated from AoWWinControl.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// Fields: FModal, FOnDrawSurface, FOnPreDrawSurface, FDragging, FDragX, FDragY, FDraggable,
// FResizable, FResizingX, FResizingY, FOnResize, FOnInternalResize, FDragHeight,
// FOverlapTopCorners, FOldCursor, FWindowCursor, FOnChangeCursor, FMinWidth, FMinHeight,
// FMaxWidth, FMaxHeight, FInResizeArea, FWindowType, FILFrame, FILTexture, FUseManagerBorders,
// FFrameLeft, FFrameTop, FFrameRight, FFrameBottom, FMaskColor, FUseVMem, FBackSurface,
// (unused), FBackRect, (unused), FOnDrawBack, FOnEnter, FOnExit, FMouseInside, FILIndexFrame,
// FILIndexTexture, FDockable
public partial class AOWWindowControl : AoWComponent
{
    public bool FModal;
    public AoWOnDrawSurface<AOWWindowControl> FOnDrawSurface;
    public AoWOnDrawSurface<AOWWindowControl> FOnPreDrawSurface;
    public byte FDragging;
    public int FDragX;
    public int FDragY;
    public bool FDraggable;
    public bool FResizable;
    public byte FResizingX;
    public byte FResizingY;
    public NotifyEvent<AOWWindowControl> FOnResize;
    public NotifyEvent<AOWWindowControl> FOnInternalResize;
    public int FDragHeight;
    public bool FOverlapTopCorners;
    public int FOldCursor;
    public int FWindowCursor;
    public NotifyEvent<AOWWindowControl> FOnChangeCursor;
    public int FMinWidth;
    public int FMinHeight;
    public int FMaxWidth;
    public int FMaxHeight;
    public bool FInResizeArea;
    public WindowType FWindowType;
    public InterfaceIL FILFrame;
    public InterfaceIL FILTexture;
    public bool FUseManagerBorders;
    public int FFrameLeft;
    public int FFrameTop;
    public int FFrameRight;
    public int FFrameBottom;
    public int FMaskColor;
    public bool FUseVMem;
    public Surface FBackSurface;
    public int F190;
    public global::Aow.Rtl.Rect FBackRect;
    public int F1A4;
    public AoWOnDrawBackEvent<AOWWindowControl> FOnDrawBack;
    public NotifyEvent<AOWWindowControl> FOnEnter;
    public NotifyEvent<AOWWindowControl> FOnExit;
    public bool FMouseInside;
    public int FILIndexFrame;
    public int FILIndexTexture;
    public bool FDockable;

    // AoWWinControl.pas:103
    public AOWWindowControl(global::Aow.Rtl.Component owner) : base(owner)
    {
        FOldCursor = -2;
        FWindowCursor = -2;
    }

    // AoWWinControl.pas:110
    public override void Dispose()
    {
        FBackSurface?.Dispose();
        base.Dispose();
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
                    FWindowType = WindowType.Normal;
                }
                else
                {
                    FWindowType = WindowType.Hollow;
                }
            }
            else
            {
                if (FILTexture != null)
                {
                    FWindowType = WindowType.Borderless;
                }
                else
                {
                    FWindowType = WindowType.Blank;
                }
            }

            FValid = true;
        }

        base.Validate();
    }

    // AoWWinControl.pas:224
    public override void CheckKeyPress(ref byte Key)
    {
        AoWComponent C = default;
        FHandled = false;
        if (FVisible)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
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
    public override void CheckKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        AoWComponent C = default;
        FHandled = false;
        if (FVisible)
        {
            for (int I = FChildren.Count - 1; I >= 0; I--)
            {
                C = (AoWComponent)FChildren[I];
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
    public virtual void Blt(Surface Surface)
    {
        global::Aow.Rtl.Rect R = default;
        if (FOnPreDrawSurface != null)
        {
            FOnPreDrawSurface(this, Surface, FWinLeft, FWinTop);
        }

        R = global::Aow.Rtl.RtlUnit.Rect(0, 0, FWinWidth, FWinHeight);
        if (!FMasked)
        {
            FBackSurface.BltAreaClipped(Surface, R, FWinLeft, FWinTop);
        }
        else
        {
            var pixelFormatID = FBackSurface.GetPixelFormatID();
            var color2PixelInfo = Aow.Game.Draw.PixelFormats.Color2PixelInfo(FMaskColor, pixelFormatID, null);
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
    public void RadioChange(AoWComponent Sender)
    {
        AoWComponent C;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            C = (AoWComponent)FChildren[I];
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
    public bool VerifyBackSurface(Surface Surface)
    {
        Set<DDSCreationFlagsElement> Flags;
        DDrawSurface DDS;
        bool Result = false;
        if (FDoubleBuffered)
        {
            if (FBackSurface != null)
            {
                if (FBackSurface.GetPixelFormatID() != Surface.GetPixelFormatID() || (FBackSurface is DDrawSurface) ^ (Surface is DDrawSurface))
                {
                    FBackSurface?.Dispose();
                    FBackSurface = null;
                }
            }

            if (FBackSurface == null)
            {
                if (Surface is DDrawSurface)
                {
                    if (((DDrawSurface)Surface).Status.Contains(DDrawSurfaceStatusElement.VideoMemory) && FUseVMem)
                    {
                        Flags = Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.VideoMemory, DDSCreationFlagsElement.SystemMemory);
                    }
                    else
                    {
                        Flags = Set<DDSCreationFlagsElement>.Of(DDSCreationFlagsElement.SystemMemory);
                    }

                    DDS = new DDrawSurface();
                    FBackSurface = DDS;
                    var pixelFormatID = Surface.GetPixelFormatID();
                    DDS.CreateSurface(FWinWidth, FWinHeight, pixelFormatID, Flags);
                }
                else
                {
                    var pixelFormatID = Surface.GetPixelFormatID();
                    FBackSurface = new DataSurface(FWinWidth, FWinHeight, pixelFormatID, null);
                }

                if (FMasked)
                {
                    FBackSurface.Cls(FMaskColor);
                }

                Result = true;
            }
            else
            {
                if (FBackSurface is DDrawSurface && ((DDrawSurface)FBackSurface).Lost())
                {
                    Result = ((DDrawSurface)FBackSurface).Restore() == Aow.Game.Draw.PixelFormats.DD_OK;
                }
            }
        }

        return Result;
    }

    [Published]
    public AoWOnDrawBackEvent<AOWWindowControl> OnDrawBack { get => FOnDrawBack; set => FOnDrawBack = value; }

    [Published]
    public NotifyEvent<AOWWindowControl> OnEnter { get => FOnEnter; set => FOnEnter = value; }

    [Published]
    public NotifyEvent<AOWWindowControl> OnExit { get => FOnExit; set => FOnExit = value; }

    public new NotifyEvent<AOWWindowControl> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWWindowControl> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWWindowControl> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWWindowControl> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWWindowControl> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWWindowControl> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWWindowControl> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWWindowControl> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWWindowControl> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public enum WindowType : byte
    {
        Normal = 0, Hollow = 1, Borderless = 2, Blank = 3
    }
}

public delegate void AoWOnDrawSurface<in TSender>(TSender Sender, Surface Surface, int Left, int Top);
