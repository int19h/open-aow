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
public partial class ScrollBar : Control
{
    public bool _vanishWhenFull;
    public InterfaceImageLibrary _imageLibraryImageIncrement;
    public InterfaceImageLibrary _imageLibraryIDecrement;
    public InterfaceImageLibrary _imageLibraryTileBack;
    public InterfaceImageLibrary _imageLibraryLineBack;
    public InterfaceImageLibrary _thumbImage;
    public InterfaceImageLibrary _thumbStrip;
    public int TrackLength;
    public int ThumbLength;
    public int _min;
    public int _max;
    public int _smallChange;
    public int _largeChange;
    public int _position;
    public int PositionCount;
    public byte HeldUpdateCount;
    public byte HeldPart;
    public byte _delay;
    public int TrackClickTarget;
    public int _numberVisible;
    public int UnusedValue;
    public Style ScrollType;
    public ScrollBarThumbStyle ThumbStyle;
    public int _imageLibraryIndexIncrementUp;
    public int _imageLibraryIndexIncrementDown;
    public int _imageLibraryIndexDecrementUp;
    public int _imageLibraryIndexDecrementDown;
    public int _imageLibraryIndexBack;
    public int _thumbImageIndex;

    // AoWVScroll.pas:130
    public ScrollBar(Rtl.Component owner) : base(owner)
    {
        _smallChange = 1;
        _largeChange = 1;
        _max = 1;
        _numberVisible = 1;
        _delay = 20;
    }

    // AoWVScroll.pas:251
    // { Validate checks the image libraries and picks the kind of scroll bar: both buttons with a
    //   background and a tab make a scroll bar, both buttons alone make an increment/decrement pair,
    //   a background line with a tab makes a slider; anything else leaves the scroll bar invalid }
    public override void Validate()
    {
        if (_imageLibraryImageIncrement != null)
        {
            _imageLibraryImageIncrement = _imageLibraryImageIncrement.CheckImageLibrary(_imageLibraryIndexIncrementUp, 1, this);
        }

        if (_imageLibraryImageIncrement != null)
        {
            _imageLibraryImageIncrement = _imageLibraryImageIncrement.CheckImageLibrary(_imageLibraryIndexIncrementDown, 1, this);
        }

        if (_imageLibraryIDecrement != null)
        {
            _imageLibraryIDecrement = _imageLibraryIDecrement.CheckImageLibrary(_imageLibraryIndexDecrementUp, 1, this);
        }

        if (_imageLibraryIDecrement != null)
        {
            _imageLibraryIDecrement = _imageLibraryIDecrement.CheckImageLibrary(_imageLibraryIndexDecrementDown, 1, this);
        }

        if (_imageLibraryTileBack != null)
        {
            _imageLibraryTileBack = _imageLibraryTileBack.CheckImageLibrary(_imageLibraryIndexBack, 1, this);
        }

        if (_imageLibraryLineBack != null)
        {
            _imageLibraryLineBack = _imageLibraryLineBack.CheckImageLibrary(_imageLibraryIndexBack, 3, this);
        }

        if (_thumbImage != null)
        {
            _thumbImage = _thumbImage.CheckImageLibrary(_thumbImageIndex, 3, this);
        }

        if (_thumbStrip != null)
        {
            _thumbStrip = _thumbStrip.CheckImageLibrary(_thumbImageIndex, 3, this);
        }

        if (_thumbImage != null)
        {
            ThumbStyle = ScrollBarThumbStyle.Icon;
        }
        else
        {
            ThumbStyle = ScrollBarThumbStyle.Bar;
        }

        if (_imageLibraryImageIncrement != null && _imageLibraryIDecrement != null)
        {
            Valid = true;
            if (_imageLibraryTileBack != null && (_thumbImage != null || _thumbStrip != null))
            {
                ScrollType = Style.Scroll;
            }
            else
            {
                ScrollType = Style.IncrementDecrement;
            }
        }
        else
        {
            if (_imageLibraryLineBack != null && (_thumbImage != null || _thumbStrip != null))
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
        if (_visible && HeldUpdateCount > 0)
        {
            if (HeldUpdateCount < _delay)
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
                        _position = _position - _largeChange;
                        if (TrackClickTarget > _position)
                        {
                            _position = TrackClickTarget;
                            HeldUpdateCount = 0;
                        }

                        Drawn = false;
                        if (_onChange != null)
                        {
                            _onChange(this);
                        }

                        break;
                    case 3:
                        _position = _position + _largeChange;
                        if (TrackClickTarget + 1 < _position)
                        {
                            _position = TrackClickTarget + 1;
                            HeldUpdateCount = 0;
                        }

                        Drawn = false;
                        if (_onChange != null)
                        {
                            _onChange(this);
                        }

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
    public byte Delay { get => _delay; set => _delay = value; }

    [Published]
    public int Min { get => _min; set => SetFMin(value); }

    [Published]
    public int Max { get => _max; set => SetFMax(value); }

    [Published]
    public int SmallChange { get => _smallChange; set => SetSmall(value); }

    [Published]
    public int LargeChange { get => _largeChange; set => SetLarge(value); }

    [Published]
    public int Position { get => _position; set => SetFPosition(value); }

    [Published]
    public int NumberVisible { get => _numberVisible; set => SetFNumber(value); }

    [Published]
    public bool VanishWhenFull { get => _vanishWhenFull; set => _vanishWhenFull = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryImageIncrement { get => _imageLibraryImageIncrement; set => SetImageLibraryIncrement(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryIDecrement { get => _imageLibraryIDecrement; set => SetImageLibraryDecrement(value); }

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

    public new NotifyEvent<ScrollBar> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<ScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<ScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<ScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<ScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<ScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<ScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<ScrollBar> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<ScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
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
