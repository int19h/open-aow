#nullable disable
// Translated from AoWArnoLine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Controls;

// top, middle, bottom, copied line image library
public sealed class ArnoLine : Control
{
    private LibraryImage _imageTop;
    private LibraryImage _imageMiddle;
    private LibraryImage _imageBottom;
    private readonly InterfaceImageLibrary _lineLibrary;

    // AoWArnoLine.pas:47
    public override void Dispose()
    {
        _lineLibrary?.Dispose();
        base.Dispose();
    }

    // AoWArnoLine.pas:41
    public ArnoLine(Component owner) : base(owner) => _lineLibrary = new InterfaceImageLibrary(this);

    // AoWArnoLine.pas:53
    public override void Validate()
    {
        if (_imageTop is not null && _imageMiddle is not null && _imageBottom is not null)
        {
            _lineLibrary.Clear();
            LibraryImage copy = _imageTop.Copy();
            _lineLibrary.Add(copy);
            LibraryImage fImageMiddleCopy = _imageMiddle.Copy();
            _lineLibrary.Add(fImageMiddleCopy);
            LibraryImage fImageBottomCopy = _imageBottom.Copy();
            _lineLibrary.Add(fImageBottomCopy);
            Valid = true;
        }

        base.Validate();
    }

    // AoWArnoLine.pas:93
    public override void SetSize()
    {
        SetWinWidthWithoutInvalidate(_imageTop.Width);
        base.SetSize();
    }

    // AoWArnoLine.pas:99
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, surfDesc: lockedSurface);

                Rect oldClip = lockedSurface.ClipRectangle;
                if (WinTop > lockedSurface.ClipRectangle.Top)
                {
                    Rect clipRectangle = lockedSurface.ClipRectangle;
                    clipRectangle.Top = WinTop;
                    lockedSurface.ClipRectangle = clipRectangle;
                }

                SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                _lineLibrary.DrawVerticalStrip(0, area: rectangle, ref lockedSurface);
                RaiseDraw(this, surfDesc: lockedSurface);

                lockedSurface.ClipRectangle = oldClip;
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWArnoLine.pas:66
    public void SetITop(LibraryImage value)
    {
        if (value != _imageTop)
        {
            _imageTop = value;
            Valid = false;
        }
    }

    // AoWArnoLine.pas:75
    public void SetIMiddle(LibraryImage value)
    {
        if (value != _imageMiddle)
        {
            _imageMiddle = value;
            Valid = false;
        }
    }

    // AoWArnoLine.pas:84
    public void SetIBottom(LibraryImage value)
    {
        if (value != _imageBottom)
        {
            _imageBottom = value;
            Valid = false;
        }
    }

    [Published]
    public LibraryImage ImageTop { get => _imageTop; set => SetITop(value); }

    [Published]
    public LibraryImage ImageMiddle { get => _imageMiddle; set => SetIMiddle(value); }

    [Published]
    public LibraryImage ImageBottom { get => _imageBottom; set => SetIBottom(value); }

    public new event NotifyEvent<ArnoLine> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<ArnoLine> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<ArnoLine> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<ArnoLine> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<ArnoLine> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<ArnoLine> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ArnoLine> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ArnoLine> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ArnoLine> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(Updated), value: value);
    }
}
