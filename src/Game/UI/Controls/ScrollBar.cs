#nullable disable
// Translated from AoWVScroll.pas.
using System.ComponentModel;
using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: FVanishWhenFull, FILIInc (the increment button: down or right), FILIDec (the
// decrement button), FILTBack (the background of a scroll bar), FILLBack (the background line
// of a slider), FILITab (an icon tab), FILLTab (a line tab), F130 (the length of the track the
// tab moves in), F134 (the length of the tab), FMin, FMax, FSmallChange, FLargeChange,
// FPosition, F14C (the number of positions, FMax - FMin + 1), F150 (the updates the mouse
// button has been held. 0 when it is up), F151 (the part that was clicked: 1 the decrement
// button, 2 the track before the tab, 3 the track after it, 4 the increment button, 5 the tab),
// FDelay (the updates before a held button repeats), F154 (the position a click in the track
// pages to. the mouse offset into the tab while the tab is dragged), FNumberVisible, F15C
// (unused), ScrollType, ThumbStyle, _imageLibraryIndexIncrementUp, _imageLibraryIndexIncrementDown, _imageLibraryIndexDecrementUp,
// _imageLibraryIndexDecrementDown, _imageLibraryIndexBack, _thumbImageIndex
public partial class ScrollBar(Rtl.Component owner) : Control(owner)
{

    private InterfaceImageLibrary _imageLibraryImageIncrement;
    private InterfaceImageLibrary _imageLibraryIDecrement;
    private InterfaceImageLibrary _imageLibraryTileBack;
    private InterfaceImageLibrary _imageLibraryLineBack;
    private InterfaceImageLibrary _thumbImage;
    private InterfaceImageLibrary _thumbStrip;

    public int TrackLength { get; protected set; }
    public int ThumbLength { get; protected set; }

    private int _min;
    private int _max = 1;
    private int _smallChange = 1;
    private int _largeChange = 1;
    private int _position;

    public int PositionCount { get; set; }
    public byte HeldUpdateCount { get; protected set; }
    public byte HeldPart { get; protected set; }

    public int TrackClickTarget { get; protected set; }

    private int _numberVisible = 1;

    public Style ScrollType { get; private set; }
    public ScrollBarThumbStyle ThumbStyle { get; private set; }

    private int _imageLibraryIndexIncrementUp;
    private int _imageLibraryIndexIncrementDown;
    private int _imageLibraryIndexDecrementUp;
    private int _imageLibraryIndexDecrementDown;
    private int _imageLibraryIndexBack;
    private int _thumbImageIndex;

    // AoWVScroll.pas:251
    // { Validate checks the image libraries and picks the kind of scroll bar: both buttons with a
    //   background and a tab make a scroll bar, both buttons alone make an increment/decrement pair,
    //   a background line with a tab makes a slider; anything else leaves the scroll bar invalid }
    public override void Validate()
    {
        if (_imageLibraryImageIncrement is not null)
        {
            _imageLibraryImageIncrement = _imageLibraryImageIncrement.CheckImageLibrary(_imageLibraryIndexIncrementUp, count: 1, sender: this);
        }

        if (_imageLibraryImageIncrement is not null)
        {
            _imageLibraryImageIncrement = _imageLibraryImageIncrement.CheckImageLibrary(_imageLibraryIndexIncrementDown, count: 1, sender: this);
        }

        if (_imageLibraryIDecrement is not null)
        {
            _imageLibraryIDecrement = _imageLibraryIDecrement.CheckImageLibrary(_imageLibraryIndexDecrementUp, count: 1, sender: this);
        }

        if (_imageLibraryIDecrement is not null)
        {
            _imageLibraryIDecrement = _imageLibraryIDecrement.CheckImageLibrary(_imageLibraryIndexDecrementDown, count: 1, sender: this);
        }

        if (_imageLibraryTileBack is not null)
        {
            _imageLibraryTileBack = _imageLibraryTileBack.CheckImageLibrary(_imageLibraryIndexBack, count: 1, sender: this);
        }

        if (_imageLibraryLineBack is not null)
        {
            _imageLibraryLineBack = _imageLibraryLineBack.CheckImageLibrary(_imageLibraryIndexBack, count: 3, sender: this);
        }

        if (_thumbImage is not null)
        {
            _thumbImage = _thumbImage.CheckImageLibrary(_thumbImageIndex, count: 3, sender: this);
        }

        if (_thumbStrip is not null)
        {
            _thumbStrip = _thumbStrip.CheckImageLibrary(_thumbImageIndex, count: 3, sender: this);
        }

        ThumbStyle = _thumbImage is not null ? ScrollBarThumbStyle.Icon : ScrollBarThumbStyle.Bar;

        if (_imageLibraryImageIncrement is not null && _imageLibraryIDecrement is not null)
        {
            Valid = true;
            ScrollType = _imageLibraryTileBack is not null && (_thumbImage is not null || _thumbStrip is not null) ? Style.Scroll : Style.IncrementDecrement;
        }
        else
        {
            if (_imageLibraryLineBack is not null && (_thumbImage is not null || _thumbStrip is not null))
            {
                Valid = true;
                ScrollType = Style.Slider;
            }
        }

        base.Validate();
    }

    // AoWVScroll.pas:351
    // { Update repeats the action of a held mouse button once the button has been held for Delay
    //   updates: a button steps by SmallChange, the track pages by LargeChange up to the position that
    //   was clicked }
    public override void Update()
    {
        if (Visible && HeldUpdateCount > 0)
        {
            if (HeldUpdateCount < Delay)
            {
                HeldUpdateCount = (byte)(HeldUpdateCount + 1);
            }
            else
            {
                switch (HeldPart)
                {
                    case 1:
                        ApplySmallStep(true);
                        break;
                    case 4:
                        ApplySmallStep(false);
                        break;
                    case 2:
                        _position -= _largeChange;
                        if (TrackClickTarget > _position)
                        {
                            _position = TrackClickTarget;
                            HeldUpdateCount = 0;
                        }

                        Drawn = false;
                        RaiseChange(this);

                        break;
                    case 3:
                        _position += _largeChange;
                        if (TrackClickTarget + 1 < _position)
                        {
                            _position = TrackClickTarget + 1;
                            HeldUpdateCount = 0;
                        }

                        Drawn = false;
                        RaiseChange(this);

                        break;
                    default:
                        break;
                }
            }
        }

        base.Update();
    }

    // AoWVScroll.pas:194
    public virtual void SetImageLibraryImageUIndex(int value)
    {
        if (value != _imageLibraryIndexIncrementUp)
        {
            _imageLibraryIndexIncrementUp = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:203
    public virtual void SetImageLibraryIdIndex(int value)
    {
        if (value != _imageLibraryIndexIncrementDown)
        {
            _imageLibraryIndexIncrementDown = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:212
    public virtual void SetILDUIndex(int value)
    {
        if (value != _imageLibraryIndexDecrementUp)
        {
            _imageLibraryIndexDecrementUp = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:221
    public virtual void SetILDDIndex(int value)
    {
        if (value != _imageLibraryIndexDecrementDown)
        {
            _imageLibraryIndexDecrementDown = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:230
    public virtual void SetImageLibraryIndex(int value)
    {
        if (value != _imageLibraryIndexBack)
        {
            _imageLibraryIndexBack = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:239
    public virtual void SetImageLibraryTileIndex(int value)
    {
        if (value != _thumbImageIndex)
        {
            _thumbImageIndex = value;
            Valid = false;
        }
    }

    // AoWVScroll.pas:140
    public virtual void SetImageLibraryIncrement(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryImageIncrement)
        {
            Valid = false;
            _imageLibraryImageIncrement = value;
        }
    }

    // AoWVScroll.pas:149
    public virtual void SetImageLibraryDecrement(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryIDecrement)
        {
            Valid = false;
            _imageLibraryIDecrement = value;
        }
    }

    // AoWVScroll.pas:158
    public virtual void SetImageLibraryBack(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTileBack)
        {
            Valid = false;
            _imageLibraryTileBack = value;
        }
    }

    // AoWVScroll.pas:167
    public virtual void SetImageLibraryLineBack(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryLineBack)
        {
            Valid = false;
            _imageLibraryLineBack = value;
        }
    }

    // AoWVScroll.pas:176
    public virtual void SetThumbImage(InterfaceImageLibrary value)
    {
        if (value != _thumbImage)
        {
            Valid = false;
            _thumbImage = value;
        }
    }

    // AoWVScroll.pas:185
    public virtual void SetThumbStrip(InterfaceImageLibrary value)
    {
        if (value != _thumbStrip)
        {
            Valid = false;
            _thumbStrip = value;
        }
    }

    // AoWVScroll.pas:289
    public void SetSmall(int value)
    {
        if (value < 1)
        {
            value = 1;
        }

        _smallChange = value;
    }

    // AoWVScroll.pas:296
    public void SetLarge(int value)
    {
        if (value < 1)
        {
            value = 1;
        }

        _largeChange = value;
    }

    // AoWVScroll.pas:303
    public void SetFMin(int value)
    {
        if (value > _max)
        {
            value = _max;
        }

        if (value != _min)
        {
            _min = value;
            Sized = false;
        }
    }

    // AoWVScroll.pas:314
    public void SetFMax(int value)
    {
        if (value < _min)
        {
            value = _min;
        }

        if (value != _max)
        {
            _max = value;
            Sized = false;
        }
    }

    // AoWVScroll.pas:325
    public void SetFNumber(int value)
    {
        if (value > _max - _min + 1)
        {
            value = _max - _min + 1;
        }

        if (value < 1)
        {
            value = 1;
        }

        if (value != _numberVisible)
        {
            _numberVisible = value;
            Sized = false;
        }
    }

    // AoWVScroll.pas:338
    public void SetFPosition(int value)
    {
        _position = value;
        if (_position + _numberVisible > _max - _min + 1)
        {
            _position = _max - _min - _numberVisible + 1;
        }

        if (_position < 0)
        {
            _position = 0;
        }

        Drawn = false;
    }

    [Published]
    [DefaultValue(20)]
    public byte Delay { get; set; } = 20;

    [Published]
    public int Min { get => _min; set => SetFMin(value); }

    internal void SetMinUnclamped(int value) => _min = value;

    [Published]
    public int Max { get => _max; set => SetFMax(value); }

    internal void SetMaxUnclamped(int value) => _max = value;

    [Published]
    public int SmallChange { get => _smallChange; set => SetSmall(value); }

    internal void SetSmallChangeUnclamped(int value) => _smallChange = value;

    [Published]
    public int LargeChange { get => _largeChange; set => SetLarge(value); }

    [Published]
    public int Position { get => _position; set => SetFPosition(value); }

    protected internal void SetPositionUnclamped(int value) => _position = value;

    [Published]
    public int NumberVisible { get => _numberVisible; set => SetFNumber(value); }

    protected internal void SetNumberVisibleUnclamped(int value) => _numberVisible = value;

    [Published]
    public bool VanishWhenFull { get; set; }

    [Published]
    public InterfaceImageLibrary ImageLibraryImageIncrement { get => _imageLibraryImageIncrement; set => SetImageLibraryIncrement(value); }

    internal void SetImageLibraryImageIncrementWithoutInvalidate(InterfaceImageLibrary value) => _imageLibraryImageIncrement = value;

    [Published]
    public InterfaceImageLibrary ImageLibraryIDecrement { get => _imageLibraryIDecrement; set => SetImageLibraryDecrement(value); }

    internal void SetImageLibraryIDecrementWithoutInvalidate(InterfaceImageLibrary value) => _imageLibraryIDecrement = value;

    [Published]
    public InterfaceImageLibrary ImageLibraryTileBack { get => _imageLibraryTileBack; set => SetImageLibraryBack(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryLineBack { get => _imageLibraryLineBack; set => SetImageLibraryLineBack(value); }

    [Published]
    public InterfaceImageLibrary ThumbImage { get => _thumbImage; set => SetThumbImage(value); }

    [Published]
    public InterfaceImageLibrary ThumbStrip { get => _thumbStrip; set => SetThumbStrip(value); }

    [Published]
    public int ImageLibraryIndexIncrementUp { get => _imageLibraryIndexIncrementUp; set => SetImageLibraryImageUIndex(value); }

    [Published]
    public int ImageLibraryIndexIncrementDown { get => _imageLibraryIndexIncrementDown; set => SetImageLibraryIdIndex(value); }

    [Published]
    public int ImageLibraryIndexDecrementUp { get => _imageLibraryIndexDecrementUp; set => SetILDUIndex(value); }

    [Published]
    public int ImageLibraryIndexDecrementDown { get => _imageLibraryIndexDecrementDown; set => SetILDDIndex(value); }

    [Published]
    public int ImageLibraryIndexBack { get => _imageLibraryIndexBack; set => SetImageLibraryIndex(value); }

    [Published]
    public int ThumbImageIndex { get => _thumbImageIndex; set => SetImageLibraryTileIndex(value); }

    public new event NotifyEvent<ScrollBar> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<ScrollBar> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<ScrollBar> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<ScrollBar> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<ScrollBar> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<ScrollBar> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ScrollBar> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ScrollBar> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ScrollBar> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, ScrollBar>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, ScrollBar>(this, slot: nameof(Updated), value: value);
    }

    public enum Style : byte
    {
        Scroll = 0, Slider = 1, IncrementDecrement = 2
    }
}

public enum ScrollBarThumbStyle : byte
{
    Icon = 0, Bar = 1
}
