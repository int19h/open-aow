// Translated from AoWWinControl.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: _modal, _onDrawSurface, _onPreDrawSurface, Dragging, DragX, DragY, _draggable,
// _resizable, ResizingX, ResizingY, _onResize, _onInternalResize, _dragHeight,
// _overlapTopCorners, OldCursor, WindowCursor, _onChangeCursor, _minWidth, _minHeight,
// _maxWidth, _maxHeight, InResizeArea, WindowStyle, _imageLibraryFrame, _imageLibraryTexture, _useManagerBorders,
// FrameLeft, FrameTop, FrameRight, FrameBottom, _maskColor, _useVideoMemory, BackSurface,
// (unused), BackRectangle, (unused), _onDrawBack, _onEnter, _onExit, MouseInside, _imageLibraryIndexFrame,
// _imageLibraryIndexTexture, _dockable
public partial class WindowControl : UIComponent
{
    public bool _modal;
    public AoWOnDrawSurface<WindowControl> _onDrawSurface;
    public AoWOnDrawSurface<WindowControl> _onPreDrawSurface;

    public byte Dragging { get; protected set; }
    public int DragX { get; protected set; }
    public int DragY { get; protected set; }

    public bool _draggable;
    public bool _resizable;

    public byte ResizingX { get; protected set; }
    public byte ResizingY { get; protected set; }

    public NotifyEvent<WindowControl> _onResize;
    public NotifyEvent<WindowControl> _onInternalResize;
    public int _dragHeight;
    public bool _overlapTopCorners;

    public int OldCursor { get; protected set; }
    public int WindowCursor { get; protected set; }

    public NotifyEvent<WindowControl> _onChangeCursor;
    public int _minWidth;
    public int _minHeight;
    public int _maxWidth;
    public int _maxHeight;

    public bool InResizeArea { get; protected set; }
    public Style WindowStyle { get; private set; }

    public InterfaceImageLibrary _imageLibraryFrame;
    public InterfaceImageLibrary _imageLibraryTexture;
    public bool _useManagerBorders;

    public int FrameLeft { get; protected set; }
    public int FrameTop { get; protected set; }
    public int FrameRight { get; protected set; }
    public int FrameBottom { get; protected set; }

    public int _maskColor;
    public bool _useVideoMemory;

    public Surface BackSurface { get; protected set; }
    public int UnusedBackValue { get; }

    public Rect BackRectangle;

    public int UnusedBackValue2 { get; }

    public AoWOnDrawBackEvent<WindowControl> _onDrawBack;
    public NotifyEvent<WindowControl> _onEnter;
    public NotifyEvent<WindowControl> _onExit;

    public bool MouseInside { get; set; }

    public int _imageLibraryIndexFrame;
    public int _imageLibraryIndexTexture;
    public bool _dockable;

    // AoWWinControl.pas:103
    public WindowControl(Component owner) : base(owner)
    {
        OldCursor = -2;
        WindowCursor = -2;
    }

    // AoWWinControl.pas:110
    public override void Dispose()
    {
        BackSurface?.Dispose();
        base.Dispose();
    }

    // AoWWinControl.pas:120
    public override void Validate()
    {
        if (OwnerComponent != null)
        {
            if (_imageLibraryFrame != null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
            }

            if (_imageLibraryTexture != null)
            {
                _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
            }

            if (_imageLibraryFrame != null)
            {
                if (_imageLibraryTexture != null)
                {
                    WindowStyle = Style.Normal;
                }
                else
                {
                    WindowStyle = Style.Hollow;
                }
            }
            else
            {
                if (_imageLibraryTexture != null)
                {
                    WindowStyle = Style.Borderless;
                }
                else
                {
                    WindowStyle = Style.Blank;
                }
            }

            Valid = true;
        }

        base.Validate();
    }

    // AoWWinControl.pas:224
    public override void CheckKeyPress(ref byte key)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible)
        {
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.CheckKeyPress(ref key);
                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if ((Handled || _modal) && _onKeyPress != null)
            {
                _onKeyPress(this, ref key);
            }

            if (_modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWinControl.pas:246
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        UIComponent C = default;
        Handled = false;
        if (Visible)
        {
            for (int I = Children.Count - 1; I >= 0; I--)
            {
                C = (UIComponent)Children[I];
                C.CheckKeyDown(ref key, shift);
                if (C.Handled == true)
                {
                    Handled = true;
                }
            }

            if (_onKeyDown != null)
            {
                _onKeyDown(this, ref key, shift);
            }

            if (_modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWinControl.pas:116
    public virtual void DetachMenuBar()
    {
    }

    // AoWWinControl.pas:208
    public virtual void Blit(Surface surface)
    {
        Rect R = default;
        if (_onPreDrawSurface != null)
        {
            _onPreDrawSurface(this, surface, WinLeft, WinTop);
        }

        R = new Rect(0, 0, WinWidth, WinHeight);
        if (!Masked)
        {
            BackSurface.BlitAreaClipped(surface, R, WinLeft, WinTop);
        }
        else
        {
            var pixelFormatId = BackSurface.GetPixelFormatId();
            var color2PixelInfo = PixelFormats.Color2PixelInfo(_maskColor, pixelFormatId, null);
            BackSurface.BlitAreaColorKeyClipped(surface, R, WinLeft, WinTop, color2PixelInfo);
        }

        if (_onDrawSurface != null)
        {
            _onDrawSurface(this, surface, WinLeft, WinTop);
        }
    }

    // AoWWinControl.pas:142
    public void SetOverlap(bool value)
    {
        _overlapTopCorners = value;
        Drawn = false;
    }

    // AoWWinControl.pas:149
    // { RadioChange unchecks the other children of the radio group of Sender }
    public void RadioChange(UIComponent sender)
    {
        UIComponent C;
        for (int I = 0, last = Children.Count - 1; I <= last; I++)
        {
            C = (UIComponent)Children[I];
            if (sender._radioIndex == C._radioIndex && C != sender && C._checked == true)
            {
                C._checked = false;
                C.Drawn = false;
                if (C._onChange != null)
                {
                    C._onChange(C);
                }
            }
        }
    }

    // AoWWinControl.pas:170
    // { VerifyBackSurface makes the back surface match Surface (a DirectDraw surface in video memory
    //   when Surface is one and UseVMem is set, else a surface in system memory or a data surface);
    //   True when the back surface was created or restored and has to be drawn again }
    public bool VerifyBackSurface(Surface surface)
    {
        Set<DirectDrawSurfaceCreationFlagsElement> flags;
        DirectDrawSurface dDS;
        bool result = false;
        if (_doubleBuffered)
        {
            if (BackSurface != null)
            {
                if (BackSurface.GetPixelFormatId() != surface.GetPixelFormatId() || (BackSurface is DirectDrawSurface) ^ (surface is DirectDrawSurface))
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }

            if (BackSurface == null)
            {
                if (surface is DirectDrawSurface)
                {
                    if (((DirectDrawSurface)surface).Status.Contains(DirectDrawSurfaceStatusElement.VideoMemory) && _useVideoMemory)
                    {
                        flags = Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.VideoMemory, DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                    }
                    else
                    {
                        flags = Set<DirectDrawSurfaceCreationFlagsElement>.Of(DirectDrawSurfaceCreationFlagsElement.SystemMemory);
                    }

                    dDS = new DirectDrawSurface();
                    BackSurface = dDS;
                    var pixelFormatId = surface.GetPixelFormatId();
                    dDS.CreateSurface(WinWidth, WinHeight, pixelFormatId, flags);
                }
                else
                {
                    var pixelFormatId = surface.GetPixelFormatId();
                    BackSurface = new DataSurface(WinWidth, WinHeight, pixelFormatId, null);
                }

                if (Masked)
                {
                    BackSurface.Clear(_maskColor);
                }

                result = true;
            }
            else
            {
                if (BackSurface is DirectDrawSurface && ((DirectDrawSurface)BackSurface).Lost())
                {
                    result = ((DirectDrawSurface)BackSurface).Restore() == PixelFormats.DD_OK;
                }
            }
        }

        return result;
    }

    [Published]
    public AoWOnDrawBackEvent<WindowControl> OnDrawBack { get => _onDrawBack; set => _onDrawBack = value; }

    [Published]
    public NotifyEvent<WindowControl> OnEnter { get => _onEnter; set => _onEnter = value; }

    [Published]
    public NotifyEvent<WindowControl> OnExit { get => _onExit; set => _onExit = value; }

    public new NotifyEvent<WindowControl> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<WindowControl> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<WindowControl> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<WindowControl> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<WindowControl> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<WindowControl> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<WindowControl> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<WindowControl> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<WindowControl> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public enum Style : byte
    {
        Normal = 0, Hollow = 1, Borderless = 2, Blank = 3
    }
}
