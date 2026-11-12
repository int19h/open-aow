// Translated from AoWArnoLine.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// top, middle, bottom, copied line image library
public partial class ArnoLine : Control
{
    private LibraryImage _imageTop;
    private LibraryImage _imageMiddle;
    private LibraryImage _imageBottom;
    private InterfaceImageLibrary _lineLibrary;

    // AoWArnoLine.pas:47
    public override void Dispose()
    {
        _lineLibrary?.Dispose();
        base.Dispose();
    }

    // AoWArnoLine.pas:41
    public ArnoLine(Component owner) : base(owner)
    {
        _lineLibrary = new InterfaceImageLibrary(this);
    }

    // AoWArnoLine.pas:53
    public override void Validate()
    {
        if (_imageTop != null && _imageMiddle != null && _imageBottom != null)
        {
            _lineLibrary.Clear();
            var copy = _imageTop.Copy();
            _lineLibrary.Add(copy);
            var fImageMiddleCopy = _imageMiddle.Copy();
            _lineLibrary.Add(fImageMiddleCopy);
            var fImageBottomCopy = _imageBottom.Copy();
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
        Rect oldClip = default;
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                oldClip = lockedSurface.ClipRectangle;
                if (WinTop > lockedSurface.ClipRectangle.Top)
                {
                    lockedSurface.ClipRectangle.Top = WinTop;
                }

                var rectangle = new SizedRectangle(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                _lineLibrary.DrawVerticalStrip(0, rectangle, ref lockedSurface);
                RaiseDraw(this, lockedSurface);

                lockedSurface.ClipRectangle = oldClip;
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
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

    public new event NotifyEvent<ArnoLine> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<ArnoLine> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<ArnoLine> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<ArnoLine> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<ArnoLine> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<ArnoLine> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ArnoLine> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ArnoLine> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ArnoLine> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, ArnoLine>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, ArnoLine>(this, slot: nameof(OnUpdate), value: value);
    }
}
