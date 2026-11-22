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

    public event EventHandler<DrawSurfaceEventArgs>? Blitted;

    protected void RaiseBlitted(Surface? surface, int left, int top) => Blitted?.Invoke(this, e: new DrawSurfaceEventArgs { Surface = surface, Left = left, Top = top });

    public event EventHandler<DrawSurfaceEventArgs>? Blitting;

    protected void RaiseBlitting(Surface? surface, int left, int top) => Blitting?.Invoke(this, e: new DrawSurfaceEventArgs { Surface = surface, Left = left, Top = top });

    public int Dragging { get; protected set; }
    public int DragX { get; protected set; }
    public int DragY { get; protected set; }

    [Published]
    public bool Draggable { get; set; }

    [Published]
    public bool Resizable { get; set; }

    public int ResizingX { get; protected set; }
    public int ResizingY { get; protected set; }

    public event EventHandler? Resized;

    protected void RaiseResize() => Resized?.Invoke(this, e: EventArgs.Empty);

    public event EventHandler? SizeChanged;

    protected void RaiseInternalResize() => SizeChanged?.Invoke(this, e: EventArgs.Empty);

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

    public event EventHandler? CursorChanged;

    protected void RaiseChangeCursor() => CursorChanged?.Invoke(this, e: EventArgs.Empty);

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
    public event EventHandler<SurfaceEventArgs>? BackgroundDrawn;

    protected void RaiseDrawBack(LockedSurface surfDesc) => BackgroundDrawn?.Invoke(this, e: new SurfaceEventArgs { Surface = surfDesc });

    [Published]
    public event EventHandler? MouseEntered;

    internal void RaiseEnter() => MouseEntered?.Invoke(this, e: EventArgs.Empty);

    [Published]
    public event EventHandler? MouseLeft;

    internal void RaiseExit() => MouseLeft?.Invoke(this, e: EventArgs.Empty);

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
                RaiseKeyPress(ref key);
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

            RaiseKeyDown(ref key, shift);

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
        Blitting?.Invoke(this, e: new DrawSurfaceEventArgs { Surface = surface, Left = WinLeft, Top = WinTop });

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

        Blitted?.Invoke(this, e: new DrawSurfaceEventArgs { Surface = surface, Left = WinLeft, Top = WinTop });
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
                c.RaiseChange();
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

    public enum Style : byte
    {
        Normal = 0, Hollow = 1, Borderless = 2, Blank = 3
    }
}
