// Translated from AOWToolTip.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// { The ToolTip unit of aowInt.dpl: TAOWToolTip, the small window that TAOWControl shows with a
//   control's tool tip text after the mouse rested on it for ToolTipDelay updates. }
// Fields: _items, _onDrawItem, _toolTipStatic, _toolTipDelay, _showDuration, _itemHeight,
// FShowing, FControlRect (the control on the screen. the tip shows at its bottom right),
// FItemIndex, FItemRect (the line being drawn, for OnDrawItem), FLines (Items broken into lines
// that fit)
public partial class ToolTip : GenericWindow
{
    public StringList _items;
    public NotifyEvent<ToolTip> _onDrawItem;
    public bool _toolTipStatic;
    public int _toolTipDelay;
    public int _showDuration;
    public int _itemHeight;
    public bool Showing;
    public Rect ControlRectangle;
    public int ItemIndex;
    public Rect ItemRectangle;
    public StringList Lines;

    // AOWToolTip.pas:62
    public ToolTip(Component owner) : base(owner)
    {
        Lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        _showDuration = 100;
        _itemHeight = 10;
    }

    // AOWToolTip.pas:72
    public override void Dispose()
    {
        Lines.Dispose();
        _items.Dispose();
        base.Dispose();
    }

    // AOWToolTip.pas:79
    public override void Validate()
    {
        if (_imageLibraryFrame != null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
        }

        if (_imageLibraryTexture != null)
        {
            _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
        }

        if (_ownerComponent != null && _imageLibraryFrame != null && _imageLibraryTexture != null && _font != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWToolTip.pas:95
    // { SetSize breaks the lines of Items into FLines so that each fits the width (moving the last
    //   characters of a line to the next one up to a space), sizes the tool tip to the text and keeps
    //   it on the screen. The width limit of the line breaking uses the images 3 and 4 of the frame
    //   library instead of FILIndexFrame + 3 and + 4, and the minimum width adds the width of the
    //   top left corner twice (original bugs). }
    public override void SetSize()
    {
        int I;
        int W;
        bool moved;
        bool done;
        if (_ownerItem != 0)
        {
            _winWidth = _ownerItem;
        }
        else
        {
            _ownerItem = _winWidth;
        }

        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
        if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > _winHeight)
        {
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
            _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
        }

        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > _winWidth)
        {
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame];
            _winWidth = imageLibraryFrame2.Width + imageLibraryFrame3.Width;
        }

        Lines.Clear();
        I = 0;
        int last = _items.Count - 1;
        for (; I <= last; I += 1)
        {
            var item = _items[I];
            Lines.Add(item);
        }

        if (Lines.Count > 0)
        {
            I = 0;
            while (I < Lines.Count)
            {
                moved = false;
                done = true;
                while (true)
                {
                    bool notDone = !done;
                    if (!notDone)
                    {
                        var line = Lines[I];
                        var textWidth = _font.GetTextWidth(line);
                        var imageLibraryFrame2 = _imageLibraryFrame[3];
                        var imageLibraryFrame3 = _imageLibraryFrame[4];
                        notDone = textWidth > _winWidth - imageLibraryFrame2.Width - imageLibraryFrame3.Width - _borderWidth * 2;
                    }

                    if (!(notDone && Lines[I].Length > 1))
                        break;
                    done = false;
                    if (I < Lines.Count - 1)
                    {
                        if (moved == true)
                        {
                            var lineLength = Lines[I];
                            var line = Lines[I];
                            var line2 = Lines[I + 1];
                            Lines[I + 1] = Copy(line, lineLength.Length, 1) + line2;
                        }
                        else
                        {
                            var lineLength = Lines[I];
                            var line = Lines[I];
                            Lines.Insert(I + 1, Copy(line, lineLength.Length, 1));
                        }
                    }
                    else
                    {
                        var lineLength = Lines[I];
                        var line = Lines[I];
                        Lines.Add(Copy(line, lineLength.Length, 1));
                    }

                    var line3 = Lines[I];
                    var line4 = Lines[I];
                    Lines[I] = Copy(line4, 1, line3.Length - 1);
                    moved = true;
                    var line5 = Lines[I];
                    if (Copy(Lines[I], line5.Length, 1) == " ")
                    {
                        done = true;
                    }
                }

                I += 1;
            }

            var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            var count = Lines.Count;
            _winHeight = imageLibraryFrame5.Height + imageLibraryFrame6.Height + _borderHeight * 2 + count * _itemHeight;
            var line6 = Lines[0];
            W = _font.GetTextWidth(line6);
            var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
            if (W < _winWidth - imageLibraryFrame7.Width - imageLibraryFrame8.Width - _borderWidth * 2)
            {
                var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                _winWidth = W + imageLibraryFrame2.Width + imageLibraryFrame3.Width + _borderWidth * 2;
            }
        }

        if (_masked)
        {
            FrameLeft = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width;
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
            FrameRight = _winWidth - imageLibraryFrame2.Width;
            FrameTop = _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height;
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            FrameBottom = _winHeight - imageLibraryFrame3.Height;
        }

        _winLeft = ControlRectangle.Right;
        _winTop = ControlRectangle.Bottom;
        if (_windowManager._winWidth < _winLeft + _winWidth)
        {
            _winLeft = _windowManager._winWidth - _winWidth;
        }

        if (_winTop + _winHeight > _windowManager._winHeight)
        {
            _winTop = ControlRectangle.Top - _winHeight;
        }

        if (_winTop < 0)
        {
            _winTop = 0;
        }

        base.SetSize();
    }

    // AOWToolTip.pas:162
    public override void Update()
    {
        bool B;
        B = _doubleBuffered;
        _doubleBuffered = false;
        base.Update();
        _doubleBuffered = B;
    }

    // AOWToolTip.pas:186
    public override void SetVisible(bool value)
    {
        if (value != _visible)
        {
            _visible = value;
            if (!_visible)
            {
                Hidden = true;
                if (_onHide != null)
                {
                    _onHide(this);
                }
            }
            else
            {
                if (_doubleBuffered)
                {
                    SurfaceValid = false;
                }
                else
                {
                    Drawn = false;
                }

                if (!Hidden)
                {
                    BackRectangle.Right = _winWidth;
                    BackRectangle.Bottom = _winHeight;
                    BackRectangle.Top = _winTop;
                    BackRectangle.Left = _winLeft;
                }

                if (_onShow != null)
                {
                    _onShow(this);
                }
            }

            if (_visible == true)
            {
                Hidden = false;
                SetPriority(_priority);
            }
        }

        BackRectangle.Right = 0;
        BackRectangle.Bottom = 0;
    }

    // AOWToolTip.pas:223
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip;
        try
        {
            if (Valid && _visible)
            {
                var rectangle = new Rect(_winLeft, _winTop, _winWidth, _winHeight);
                _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var rectangle2 = new Rect(_winLeft + imageLibraryFrame4.Width, _winTop + imageLibraryFrame3.Height, _winWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                if (Lines.Count > 0)
                {
                    for (int I = 0, last = Lines.Count - 1; I <= last; I++)
                    {
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        if (_winLeft + _winWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                            lockedSurface.ClipRectangle.Right = _winLeft + _winWidth - imageLibraryFrame6.Width;
                        }

                        if (I < Lines.Count && Lines.Count > 0)
                        {
                            var line = Lines[I];
                            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame];
                            var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            _font.SetText(_winLeft + _borderWidth + imageLibraryFrame7.Width, _winTop + _borderHeight + imageLibraryFrame6.Height + _itemHeight * I, line, lockedSurface);
                            ItemIndex = I;
                            var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame9 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var libraryImage = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            ItemRectangle = new Rect(_winLeft + libraryImage.Width, _winTop + imageLibraryFrame9.Height + _itemHeight * I, _winLeft + _winWidth, _winTop + imageLibraryFrame8.Height + (I + 1) * _itemHeight);
                            if (_onDrawItem != null)
                            {
                                _onDrawItem(this);
                            }
                        }

                        lockedSurface.ClipRectangle = oldClip;
                    }
                }

                if (_onDraw != null)
                {
                    _onDraw(this, lockedSurface);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWToolTip.pas:266
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Showing = false;
        SetVisible(false);
    }

    // AOWToolTip.pas:172
    public void SetFStrings(StringList value)
    {
        Lines.Clear();
        _items.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            Lines.Add(valueValue);
            var valueValue2 = value[I];
            _items.Add(valueValue2);
        }

        Sized = false;
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public NotifyEvent<ToolTip> OnDrawItem { get => _onDrawItem; set => _onDrawItem = value; }

    [Published]
    public bool ToolTipStatic { get => _toolTipStatic; set => _toolTipStatic = value; }

    [Published]
    public int ToolTipDelay { get => _toolTipDelay; set => _toolTipDelay = value; }

    [Published]
    public int ShowDuration { get => _showDuration; set => _showDuration = value; }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    public new NotifyEvent<ToolTip> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<ToolTip> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<ToolTip> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<ToolTip> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<ToolTip> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<ToolTip> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<ToolTip> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<ToolTip> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<ToolTip> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<ToolTip> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<ToolTip> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<ToolTip> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<ToolTip> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<ToolTip> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<ToolTip> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<ToolTip> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<ToolTip> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }
}
