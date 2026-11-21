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
public class WindowControl(Component owner) : UIComponent(owner)
{

    [Published]
    public bool Modal { get; set; }

    public event AoWOnDrawSurface<WindowControl> SurfaceDrawn;

    protected void RaiseDrawSurface(WindowControl sender, Surface surface, int left, int top) => SurfaceDrawn?.Invoke(sender, surface, left, top);

    public event AoWOnDrawSurface<WindowControl> PreDrawSurface;

    protected void RaisePreDrawSurface(WindowControl sender, Surface surface, int left, int top) => PreDrawSurface?.Invoke(sender, surface, left, top);

    public byte Dragging { get; protected set; }
    public int DragX { get; protected set; }
    public int DragY { get; protected set; }

    [Published]
    public bool Draggable { get; set; }

    [Published]
    public bool Resizable { get; set; }

    public byte ResizingX { get; protected set; }
    public byte ResizingY { get; protected set; }

    public event NotifyEvent<WindowControl> Resize;

    protected void RaiseResize(WindowControl sender) => Resize?.Invoke(sender);

    public event NotifyEvent<WindowControl> InternalResize;

    protected void RaiseInternalResize(WindowControl sender) => InternalResize?.Invoke(sender);

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

    public int OldCursor { get; protected set; } = -2;
    public int WindowCursor { get; protected set; } = -2;

    public event NotifyEvent<WindowControl> ChangeCursor;

    protected void RaiseChangeCursor(WindowControl sender) => ChangeCursor?.Invoke(sender);

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

    public SizedRectangle BackRectangle { get; internal set; }

    [Published]
    public event AoWOnDrawBackEvent<WindowControl> DrawBack;

    protected void RaiseDrawBack(WindowControl sender, LockedSurface surfDesc) => DrawBack?.Invoke(sender, surfDesc);

    [Published]
    public event NotifyEvent<WindowControl> Enter;

    internal void RaiseEnter(WindowControl sender) => Enter?.Invoke(sender);

    [Published]
    public event NotifyEvent<WindowControl> Exit;

    internal void RaiseExit(WindowControl sender) => Exit?.Invoke(sender);

    public bool MouseInside { get; set; }

    [Published]
    public virtual int ImageLibraryIndexFrame { get; set; }

    [Published]
    public virtual int ImageLibraryIndexTexture { get; set; }

    [Published]
    public bool Dockable { get; set; }

    // AoWWinControl.pas:110
    public override void Dispose()
    {
        BackSurface?.Dispose();
        base.Dispose();
    }

    // AoWWinControl.pas:120
    public override void Validate()
    {
        if (OwnerComponent is not null)
        {
            if (ImageLibraryFrame is not null)
            {
                ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, count: 8, sender: this);
            }

            if (ImageLibraryTexture is not null)
            {
                ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, count: 1, sender: this);
            }

            WindowStyle = ImageLibraryFrame is not null
                ? ImageLibraryTexture is not null ? Style.Normal : Style.Hollow
                : ImageLibraryTexture is not null ? Style.Borderless : Style.Blank;

            Valid = true;
        }

        base.Validate();
    }

    // AoWWinControl.pas:224
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent c = Children[i];
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
    public override void CheckKeyDown(ref ushort key, ShiftStates shift)
    {
        Handled = false;
        if (Visible)
        {
            for (int i = Children.Count - 1; i >= 0; i--)
            {
                UIComponent c = Children[i];
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
        PreDrawSurface?.Invoke(this, surface, WinLeft, WinTop);

        Rect r = new(0, 0, WinWidth, WinHeight);
        if (!Masked)
        {
            _ = BackSurface.BlitAreaClipped(surface, area: r, WinLeft, WinTop);
        }
        else
        {
            int color2PixelInfo = PixelFormats.Color2PixelInfo(pixelFormatId: BackSurface.GetPixelFormatId(), color: MaskColor, palette: null);
            _ = BackSurface.BlitAreaColorKeyClipped(surface, area: r, WinLeft, WinTop, colorKey: color2PixelInfo);
        }

        SurfaceDrawn?.Invoke(this, surface, WinLeft, WinTop);
    }

    // AoWWinControl.pas:142
    public void SetOverlap(bool value) => OverlapTopCorners = value;

    // AoWWinControl.pas:149
    // { RadioChange unchecks the other children of the radio group of Sender }
    public void RadioChange(UIComponent sender)
    {
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            UIComponent c = Children[i];
            if (sender.RadioIndex == c.RadioIndex && c != sender && c.Checked)
            {
                c.Checked = false;
                c.Drawn = false;
                c.RaiseChange(c);
            }
        }
    }

    // AoWWinControl.pas:170
    // { VerifyBackSurface makes the back surface match Surface. It returns true when the back
    //   surface was created or recreated because the pixel format changed and has to be drawn
    //   again. }
    // The port draws on memory surfaces only, so the back surface is a data surface and is never
    // lost.
    public bool VerifyBackSurface(Surface surface)
    {
        if (DoubleBuffered)
        {
            if (BackSurface is not null)
            {
                if (BackSurface.GetPixelFormatId() != surface.GetPixelFormatId())
                {
                    BackSurface?.Dispose();
                    BackSurface = null;
                }
            }

            if (BackSurface is null)
            {
                PixelFormat pixelFormatId = surface.GetPixelFormatId();
                BackSurface = new DataSurface(WinWidth, WinHeight, pixelFormatId, palette: null);
                if (Masked)
                {
                    _ = BackSurface.Clear(MaskColor);
                }

                return true;
            }
        }

        return false;
    }

    public new event NotifyEvent<WindowControl> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<WindowControl> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<WindowControl> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<WindowControl> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<WindowControl> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<WindowControl> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<WindowControl> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(Updated), value: value);
    }

    public enum Style : byte
    {
        Normal = 0, Hollow = 1, Borderless = 2, Blank = 3
    }
}
