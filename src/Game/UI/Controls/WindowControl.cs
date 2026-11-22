#nullable disable warnings
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
public class WindowControl(Component? owner) : UIComponent(owner)
{

    [Published]
    public bool Modal { get; set; }

    public event AoWOnDrawSurface<WindowControl>? Blitted;

    protected void RaiseBlitted(WindowControl sender, Surface? surface, int left, int top) => Blitted?.Invoke(sender, surface, left, top);

    public event AoWOnDrawSurface<WindowControl>? Blitting;

    protected void RaiseBlitting(WindowControl sender, Surface? surface, int left, int top) => Blitting?.Invoke(sender, surface, left, top);

    public byte Dragging { get; protected set; }
    public int DragX { get; protected set; }
    public int DragY { get; protected set; }

    [Published]
    public bool Draggable { get; set; }

    [Published]
    public bool Resizable { get; set; }

    public byte ResizingX { get; protected set; }
    public byte ResizingY { get; protected set; }

    public event NotifyEvent<WindowControl>? Resized;

    protected void RaiseResize(WindowControl sender) => Resized?.Invoke(sender);

    public event NotifyEvent<WindowControl>? SizeChanged;

    protected void RaiseInternalResize(WindowControl sender) => SizeChanged?.Invoke(sender);

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
            IsDrawn = false;
        }
    }

    public int OldCursor { get; protected set; } = -2;
    public int WindowCursor { get; protected set; } = -2;

    public event NotifyEvent<WindowControl>? CursorChanged;

    protected void RaiseChangeCursor(WindowControl sender) => CursorChanged?.Invoke(sender);

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
    public virtual InterfaceImageLibrary? ImageLibraryFrame { get; set; }

    [Published]
    public virtual InterfaceImageLibrary? ImageLibraryTexture { get; set; }

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

    public Surface? BackSurface { get; protected set; }

    public SizedRectangle BackRectangle { get; internal set; }

    [Published]
    public event AoWOnDrawBackEvent<WindowControl>? BackgroundDrawn;

    protected void RaiseDrawBack(WindowControl sender, LockedSurface surfDesc) => BackgroundDrawn?.Invoke(sender, surfDesc);

    [Published]
    public event NotifyEvent<WindowControl>? MouseEntered;

    internal void RaiseEnter(WindowControl sender) => MouseEntered?.Invoke(sender);

    [Published]
    public event NotifyEvent<WindowControl>? MouseLeft;

    internal void RaiseExit(WindowControl sender) => MouseLeft?.Invoke(sender);

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
                UIComponent? c = Children[i];
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
                UIComponent? c = Children[i];
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
    public virtual void Blit(Surface? surface)
    {
        Blitting?.Invoke(this, surface, WinLeft, WinTop);

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

        Blitted?.Invoke(this, surface, WinLeft, WinTop);
    }

    // AoWWinControl.pas:142
    public void SetOverlap(bool value) => OverlapTopCorners = value;

    // AoWWinControl.pas:149
    // { RadioChange unchecks the other children of the radio group of Sender }
    public void RadioChange(UIComponent sender)
    {
        for (int i = 0, last = Children.Count - 1; i <= last; i++)
        {
            UIComponent? c = Children[i];
            if (sender.RadioIndex == c.RadioIndex && c != sender && c.Checked)
            {
                c.Checked = false;
                c.IsDrawn = false;
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
    public bool VerifyBackSurface(Surface? surface)
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

    public new event NotifyEvent<WindowControl> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<WindowControl> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<WindowControl> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<WindowControl> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<WindowControl> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<WindowControl> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<WindowControl> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, WindowControl>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, WindowControl>(this, slot: nameof(Drawn), value: value);
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
