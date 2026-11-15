#nullable disable
// Translated from AoWWinControl.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: _modal, OnDrawSurface, OnPreDrawSurface, Dragging, DragX, DragY, _draggable,
// _resizable, ResizingX, ResizingY, OnResize, OnInternalResize, _dragHeight,
// _overlapTopCorners, OldCursor, WindowCursor, OnChangeCursor, _minWidth, _minHeight,
// _maxWidth, _maxHeight, InResizeArea, WindowStyle, _imageLibraryFrame, _imageLibraryTexture, _useManagerBorders,
// FrameLeft, FrameTop, FrameRight, FrameBottom, _maskColor, _useVideoMemory, BackSurface,
// (unused), BackRectangle, (unused), OnDrawBack, OnEnter, OnExit, MouseInside, _imageLibraryIndexFrame,
// _imageLibraryIndexTexture, _dockable
public partial class WindowControl : UIComponent
{
    [Published]
    public bool Modal { get; set; }

    public event AoWOnDrawSurface<WindowControl> OnDrawSurface;

    protected void RaiseDrawSurface(WindowControl sender, Surface surface, int left, int top)
    {
        if (OnDrawSurface != null)
        {
            OnDrawSurface(sender, surface, left, top);
        }
    }

    public event AoWOnDrawSurface<WindowControl> OnPreDrawSurface;

    protected void RaisePreDrawSurface(WindowControl sender, Surface surface, int left, int top)
    {
        if (OnPreDrawSurface != null)
        {
            OnPreDrawSurface(sender, surface, left, top);
        }
    }

    public byte Dragging { get; protected set; }
    public int DragX { get; protected set; }
    public int DragY { get; protected set; }

    [Published]
    public bool Draggable { get; set; }

    [Published]
    public bool Resizable { get; set; }

    public byte ResizingX { get; protected set; }
    public byte ResizingY { get; protected set; }

    public event NotifyEvent<WindowControl> OnResize;

    protected void RaiseResize(WindowControl sender)
    {
        if (OnResize != null)
        {
            OnResize(sender);
        }
    }

    public event NotifyEvent<WindowControl> OnInternalResize;

    protected void RaiseInternalResize(WindowControl sender)
    {
        if (OnInternalResize != null)
        {
            OnInternalResize(sender);
        }
    }

    [Published]
    public int DragHeight { get; set; }

    // AoWWinGeneric publishes this through SetOverlap. The write marks the window dirty.
    [Published]
    public bool OverlapTopCorners
    {
        get;
        set
        {
            field = value;
            Drawn = false;
        }
    }

    public int OldCursor { get; protected set; }
    public int WindowCursor { get; protected set; }

    public event NotifyEvent<WindowControl> OnChangeCursor;

    protected void RaiseChangeCursor(WindowControl sender)
    {
        if (OnChangeCursor != null)
        {
            OnChangeCursor(sender);
        }
    }

    [Published]
    public int MinWidth { get; set; }

    [Published]
    public int MinHeight { get; set; }

    [Published]
    public int MaxWidth { get; set; }

    [Published]
    public int MaxHeight { get; set; }

    public bool InResizeArea { get; protected set; }
    public Style WindowStyle { get; private set; }

    [Published]
    public virtual InterfaceImageLibrary ImageLibraryFrame { get; set; }

    [Published]
    public virtual InterfaceImageLibrary ImageLibraryTexture { get; set; }

    [Published]
    public bool UseManagerBorders { get; set; }

    public int FrameLeft { get; protected set; }
    public int FrameTop { get; protected set; }
    public int FrameRight { get; protected set; }
    public int FrameBottom { get; protected set; }

    [Published]
    public int MaskColor { get; set; }

    [Published]
    public bool UseVideoMemory { get; set; }

    public Surface BackSurface { get; protected set; }

    public Rect BackRectangle { get; internal set; }

    [Published]
    public event AoWOnDrawBackEvent<WindowControl> OnDrawBack;

    protected void RaiseDrawBack(WindowControl sender, LockedSurface surfDesc)
    {
        if (OnDrawBack != null)
        {
            OnDrawBack(sender, surfDesc);
        }
    }

    [Published]
    public event NotifyEvent<WindowControl> OnEnter;

    internal void RaiseEnter(WindowControl sender)
    {
        if (OnEnter != null)
        {
            OnEnter(sender);
        }
    }

    [Published]
    public event NotifyEvent<WindowControl> OnExit;

    internal void RaiseExit(WindowControl sender)
    {
        if (OnExit != null)
        {
            OnExit(sender);
        }
    }

    public bool MouseInside { get; set; }

    [Published]
    public virtual int ImageLibraryIndexFrame { get; set; }

    [Published]
    public virtual int ImageLibraryIndexTexture { get; set; }

    [Published]
    public bool Dockable { get; set; }

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
            if (ImageLibraryFrame != null)
            {
                ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, 8, this);
            }

            if (ImageLibraryTexture != null)
            {
                ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, 1, this);
            }

            if (ImageLibraryFrame != null)
            {
                if (ImageLibraryTexture != null)
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
                if (ImageLibraryTexture != null)
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
        UIComponent c = default;
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
                c.CheckKeyPress(ref key);
                if (c.Handled)
                {
                    Handled = true;
                }
            }

            if (Handled || Modal)
            {
                RaiseKeyPress(this, ref key);
            }

            if (Modal)
            {
                Handled = true;
            }
        }
    }

    // AoWWinControl.pas:246
    public override void CheckKeyDown(ref ushort key, Set<ShiftState> shift)
    {
        UIComponent c = default;
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                c = Children[i];
                c.CheckKeyDown(ref key, shift);
                if (c.Handled)
                {
                    Handled = true;
                }
            }

            RaiseKeyDown(this, ref key, shift);

            if (Modal)
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
        Rect r = default;
        if (OnPreDrawSurface != null)
        {
            OnPreDrawSurface(this, surface, WinLeft, WinTop);
        }

        r = new Rect(0, 0, WinWidth, WinHeight);
        if (!Masked)
        {
            BackSurface.BlitAreaClipped(surface, r, WinLeft, WinTop);
        }
        else
        {
            var color2PixelInfo = PixelFormats.Color2PixelInfo(pixelFormatId: BackSurface.GetPixelFormatId(), color: MaskColor, palette: null);
            BackSurface.BlitAreaColorKeyClipped(surface, r, WinLeft, WinTop, color2PixelInfo);
        }

        if (OnDrawSurface != null)
        {
            OnDrawSurface(this, surface, WinLeft, WinTop);
        }
    }

    // AoWWinControl.pas:142
    public void SetOverlap(bool value) => OverlapTopCorners = value;

    // AoWWinControl.pas:149
    // { RadioChange unchecks the other children of the radio group of Sender }
    public void RadioChange(UIComponent sender)
    {
        UIComponent c;
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            c = Children[i];
            if (sender.RadioIndex == c.RadioIndex && c != sender && c.Checked)
            {
                c.Checked = false;
                c.Drawn = false;
                c.RaiseChange(c);
            }
        }
    }

    // AoWWinControl.pas:170
    // { VerifyBackSurface makes the back surface match Surface (a DirectDraw surface in video memory
    //   when Surface is one and UseVMem is set, else a surface in system memory or a data surface);
    //   True when the back surface was created or restored and has to be drawn again }
    // The port draws on memory surfaces only, so the back surface is a data surface and is never
    // lost.
    public bool VerifyBackSurface(Surface surface)
    {
        bool result = false;
        if (DoubleBuffered)
        {
            if (BackSurface != null)
            {
                if (BackSurface.GetPixelFormatId() != surface.GetPixelFormatId())
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }

            if (BackSurface == null)
            {
                var pixelFormatId = surface.GetPixelFormatId();
                BackSurface = new DataSurface(WinWidth, WinHeight, pixelFormatId, null);
                if (Masked)
                {
                    BackSurface.Clear(MaskColor);
                }

                result = true;
            }
        }

        return result;
    }

    public new event NotifyEvent<WindowControl> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<WindowControl> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<WindowControl> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<WindowControl> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<WindowControl> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<WindowControl> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<WindowControl> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnUpdate), value: value);
    }

    public enum Style : byte
    {
        Normal = 0, Hollow = 1, Borderless = 2, Blank = 3
    }
}
