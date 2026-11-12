// Translated from AoWListBox.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: FItems, FVScrollBar, F11C (the rows shown), FSelected, F124 (set to -1 by SetSize,
// otherwise unused), FOnPreDrawItem, FOnDrawItem, F138 (the width the frame or the scroll bar
// takes at the right), F13C (the scroll bar is visible), FILFrame, FILTextureNormal,
// FILTextureSelected, FItemHeight, FOnDoubleClick, FTextAlign, F15C (the first item shown),
// F160 (0: drawn with the frame and the textures, 1: text only), FILIndexFrame,
// FILIndexTNormal, FILIndexTSelected, FPopupMenu, FDrawItem, FDrawItemRect (the item and its
// rectangle while OnPreDrawItem and OnDrawItem run), F188 (the item under the mouse, -1 for
// none)
public partial class ListBox : Control
{
    public StringList _items;
    public VerticalScrollBar _verticalScrollBar;
    public int VisibleRows;
    public int _selected;
    public int UnusedPosition;
    public AoWOnDrawItemEvent<ListBox> _onPreDrawItem;
    public AoWOnDrawItemEvent<ListBox> _onDrawItem;
    public int ScrollWidth;
    public bool ScrollBarVisible;
    public InterfaceImageLibrary _imageLibraryFrame;
    public InterfaceImageLibrary _imageLibraryTextureNormal;
    public InterfaceImageLibrary _imageLibraryTextureSelected;
    public int _itemHeight;
    public NotifyEvent<ListBox> _onDoubleClick;
    public TextAlignment _textAlign;
    public int FirstShownItem;
    public byte TextOnly;
    public int _imageLibraryIndexFrame;
    public int _imageLibraryIndexTNormal;
    public int _imageLibraryIndexTSelected;
    public Menu _popupMenu;
    public int DrawItem;
    public Rect DrawItemRectangle;
    public int HoverItem;

    // AoWListBox.pas:107
    public ListBox(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        VisibleRows = 3;
        _itemHeight = 10;
    }

    // AoWListBox.pas:116
    public override void Dispose()
    {
        _items.Dispose();
        base.Dispose();
    }

    // AoWListBox.pas:124
    // { Validate: a list box needs only its font; without all three image libraries it is drawn as
    //   text only }
    public override void Validate()
    {
        if (_font != null)
        {
            if (_imageLibraryFrame != null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
            }

            if (_imageLibraryTextureNormal != null)
            {
                _imageLibraryTextureNormal = _imageLibraryTextureNormal.CheckImageLibrary(_imageLibraryIndexTNormal, 1, this);
            }

            if (_imageLibraryTextureSelected != null)
            {
                _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, 1, this);
            }

            if (_imageLibraryFrame != null && _imageLibraryTextureNormal != null && _imageLibraryTextureSelected != null)
            {
                TextOnly = 0;
            }
            else
            {
                TextOnly = 1;
            }

            Valid = true;
        }

        base.Validate();
    }

    // AoWListBox.pas:143
    public override void SetSize()
    {
        if (_itemHeight < 1)
        {
            _itemHeight = 10;
        }

        switch (TextOnly)
        {
            case 0:
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > _winHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                    _winHeight = imageLibraryFrame2.Height + imageLibraryFrame3.Height;
                }

                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > _winWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                    _winWidth = imageLibraryFrame2.Width + imageLibraryFrame3.Width;
                }

                var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                VisibleRows = (_winHeight - imageLibraryFrame5.Height - imageLibraryFrame6.Height) / _itemHeight;
                ScrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
                _masked = false;
                break;
            case 1:
                VisibleRows = _winHeight / _itemHeight;
                ScrollWidth = 0;
                _masked = true;
                break;
        }

        if (_items.Count < VisibleRows)
        {
            VisibleRows = _items.Count;
        }

        if (VisibleRows < 1)
        {
            VisibleRows = 1;
        }

        if (_items.Count <= _selected)
        {
            _selected = _items.Count - 1;
        }

        if (_items.Count < FirstShownItem + VisibleRows)
        {
            FirstShownItem = _items.Count - VisibleRows;
        }

        if (FirstShownItem < 0)
        {
            FirstShownItem = 0;
        }

        ScrollBarVisible = false;
        if (_verticalScrollBar != null)
        {
            UnusedPosition = -1;
            if (_verticalScrollBar._window != _window)
            {
                _verticalScrollBar.SetWindow(_window);
            }

            _verticalScrollBar.Priority = _priority + 1;
            _verticalScrollBar.SetFMin(0);
            var count = _items.Count;
            _verticalScrollBar.SetFMax(count - 1);
            _verticalScrollBar.SetFPosition(FirstShownItem);
            _verticalScrollBar.SetFNumber(VisibleRows);
            _verticalScrollBar.WinHeight = _winHeight;
            _verticalScrollBar.SetLarge(VisibleRows - 1);
            _verticalScrollBar.Visible = _visible;
            if (_verticalScrollBar._vanishWhenFull && _items.Count - 1 < VisibleRows)
            {
                _verticalScrollBar.Visible = false;
            }
            else
            {
                ScrollWidth = _verticalScrollBar._winWidth;
            }

            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = _winLeft + _winWidth - _verticalScrollBar._winWidth;
            _verticalScrollBar.WinTop = _winTop;
            ScrollBarVisible = _verticalScrollBar._visible;
        }

        base.SetSize();
    }

    // AoWListBox.pas:386
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        if (_visible && _verticalScrollBar != null && _items.Count > 0)
        {
            if (_verticalScrollBar._position != FirstShownItem)
            {
                FirstShownItem = _verticalScrollBar._position;
                var count = _items.Count;
                if (FirstShownItem + VisibleRows > count)
                {
                    FirstShownItem = _items.Count - VisibleRows;
                }

                if (FirstShownItem < 0)
                {
                    FirstShownItem = 0;
                }

                Drawn = false;
            }
        }

        base.Update();
    }

    // AoWListBox.pas:241
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
        if (_verticalScrollBar != null)
        {
            _verticalScrollBar.Visible = _visible;
        }
    }

    // AoWListBox.pas:304
    // { Draw draws the frame and the texture, then the visible items (one more than fit: the loop runs
    //   to F11C, original) clipped to the inside, the selected one on its highlight texture }
    public override void Draw(LockedSurface lockedSurface)
    {
        int L = default;
        int W = default;
        int T = default;
        int H = default;
        Rect oldClip = default;
        try
        {
            if (Valid && _visible)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                switch (TextOnly)
                {
                    case 0:
                        var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(_window._winLeft + _winLeft + imageLibraryFrame3.Width, _window._winTop + _winTop + imageLibraryFrame2.Height, _winWidth - imageLibraryFrameWidth.Width - ScrollWidth, _winHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle2, ref lockedSurface);
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        L = _window._winLeft + _winLeft + imageLibraryFrame4.Width;
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        T = _window._winTop + _winTop + imageLibraryFrame5.Height;
                        var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        H = _winHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        W = _winWidth - imageLibraryFrame8.Width - ScrollWidth;
                        break;
                    default:
                        L = _window._winLeft + _winLeft;
                        T = _window._winTop + _winTop;
                        H = _winHeight;
                        W = _winWidth - ScrollWidth;
                        break;
                }

                if (L + W < oldClip.Right)
                {
                    lockedSurface.ClipRectangle.Right = L + W;
                }

                if (T + H < oldClip.Bottom)
                {
                    lockedSurface.ClipRectangle.Bottom = T + H;
                }

                for (int I = 0, last = VisibleRows; I <= last; I++)
                {
                    var count = _items.Count;
                    if (FirstShownItem + I < count)
                    {
                        DrawItem = FirstShownItem + I;
                        DrawItemRectangle = new Rect(L, _itemHeight * I + T, L + W, (I + 1) * _itemHeight + T);
                        if (DrawItemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            DrawItemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        if (_onPreDrawItem != null)
                        {
                            _onPreDrawItem(this, DrawItemRectangle, DrawItem, ref lockedSurface);
                        }

                        if (FirstShownItem + I == _selected)
                        {
                            _font.Color = SelectedColor;
                            if (TextOnly == 0)
                            {
                                var rectangle = new Rect(L, _itemHeight * I + T, W, _itemHeight);
                                _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, rectangle, ref lockedSurface);
                            }
                        }
                        else
                        {
                            _font.Color = Color;
                        }

                        switch (_textAlign)
                        {
                            case TextAlignment.Left:
                                var item = _items[FirstShownItem + I];
                                _font.SetText(_borderWidth + L, _borderHeight + T + _itemHeight * I, item, lockedSurface);
                                break;
                            case TextAlignment.Right:
                                var item2 = _items[FirstShownItem + I];
                                var textWidth = _font.GetTextWidth(item2);
                                var item3 = _items[FirstShownItem + I];
                                _font.SetText(L + W - _borderWidth - textWidth, _borderHeight + T + _itemHeight * I, item3, lockedSurface);
                                break;
                            default:
                                var item4 = _items[FirstShownItem + I];
                                var fontTextWidth = _font.GetTextWidth(item4);
                                var item5 = _items[FirstShownItem + I];
                                _font.SetText(L + W / 2 - fontTextWidth / 2, _borderHeight + T + _itemHeight * I, item5, lockedSurface);
                                break;
                        }

                        if (_onDrawItem != null)
                        {
                            _onDrawItem(this, DrawItemRectangle, DrawItem, ref lockedSurface);
                        }
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
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

    // AoWListBox.pas:404
    // { CheckMouseDown selects the item under the mouse unless the owner already handled the click;
    //   a double click calls OnDoubleClick, the right button opens the popup menu at the mouse, moved
    //   back inside the window manager when it would stick out }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int I;
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (!_ownerComponent.Handled && X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth - ScrollWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (TextOnly == 0)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                I = (Y - _window._winTop - _winTop - imageLibraryFrame.Height) / _itemHeight + FirstShownItem;
            }
            else
            {
                I = (Y - _window._winTop - _winTop) / _itemHeight + FirstShownItem;
            }

            if (_items.Count > I)
            {
                if (I != _selected)
                {
                    _selected = I;
                    Drawn = false;
                    if (_onChange != null)
                    {
                        _onChange(this);
                    }
                }

                if (button == MouseButton.Left && (Set<ShiftState>.Of(ShiftState.Double) & shift) == Set<ShiftState>.Of(ShiftState.Double) && _onDoubleClick != null)
                {
                    _onDoubleClick(this);
                }

                if (_selected > -1 && button == MouseButton.Right && _popupMenu != null)
                {
                    _popupMenu.SetPopupOwner(this);
                    _popupMenu.WinLeft = X;
                    _popupMenu.WinTop = Y;
                    if (_popupMenu.WindowManager != null)
                    {
                        if (_popupMenu.Hidden)
                        {
                            if (_popupMenu.MenuLeft + _popupMenu.WinWidth > _popupMenu.WindowManager.WinWidth)
                            {
                                _popupMenu.WinLeft = X - _popupMenu.WinWidth;
                            }

                            if (_popupMenu.MenuTop + _popupMenu.WinHeight > _popupMenu.WindowManager.WinHeight)
                            {
                                _popupMenu.WinTop = Y - _popupMenu.WinHeight;
                            }
                        }
                        else
                        {
                            if (_popupMenu.WinLeft + _popupMenu.WinWidth > _popupMenu.WindowManager.WinWidth)
                            {
                                _popupMenu.WinLeft = X - _popupMenu.WinWidth;
                            }

                            if (_popupMenu.WinTop + _popupMenu.WinHeight > _popupMenu.WindowManager.WinHeight)
                            {
                                _popupMenu.WinTop = Y - _popupMenu.WinHeight;
                            }
                        }
                    }

                    _popupMenu.Visible = true;
                }
            }

            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:460
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth - ScrollWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:475
    // { CheckMouseMove tracks the item under the mouse }
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X < _window._winLeft + _winLeft || X > _window._winLeft + _winLeft + _winWidth - ScrollWidth - 1 || Y < _window._winTop + _winTop || Y > _window._winTop + _winTop + _winHeight - 1)
        {
            HoverItem = -1;
        }
        else
        {
            Handled = true;
            if (TextOnly == 0)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                HoverItem = (Y - _window._winTop - _winTop - imageLibraryFrame.Height) / _itemHeight + FirstShownItem;
            }
            else
            {
                HoverItem = (Y - _window._winTop - _winTop) / _itemHeight + FirstShownItem;
            }

            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // AoWListBox.pas:497
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWListBox.pas:275
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWListBox.pas:284
    public virtual void SetImageLibraryTileNorm(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTextureNormal)
        {
            Valid = false;
            _imageLibraryTextureNormal = value;
        }
    }

    // AoWListBox.pas:293
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTextureSelected)
        {
            Valid = false;
            _imageLibraryTextureSelected = value;
        }
    }

    // AoWListBox.pas:202
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        _items.AddStrings(value);
    }

    // AoWListBox.pas:208
    public void SetIndex(int value)
    {
        if (value >= _items.Count)
        {
            value = _items.Count - 1;
        }

        if (value != _selected)
        {
            _selected = value;
            Drawn = false;
        }
    }

    // AoWListBox.pas:219
    public void SetListOff(int value)
    {
        if (value >= _items.Count)
        {
            value = _items.Count - 1;
        }

        if (value < 0)
        {
            value = 0;
        }

        if (value != FirstShownItem)
        {
            FirstShownItem = value;
            Drawn = false;
        }
    }

    // AoWListBox.pas:232
    public void SetVScroll(VerticalScrollBar value)
    {
        _verticalScrollBar = value;
        if (value != null)
        {
            value.Priority = _priority;
        }

        Priority = _priority;
        Sized = false;
    }

    // AoWListBox.pas:248
    public void SetILFIndex(int value)
    {
        if (value != _imageLibraryIndexFrame)
        {
            _imageLibraryIndexFrame = value;
            Valid = false;
        }
    }

    // AoWListBox.pas:257
    public void SetImageLibraryTnIndex(int value)
    {
        if (value != _imageLibraryIndexTNormal)
        {
            _imageLibraryIndexTNormal = value;
            Valid = false;
        }
    }

    // AoWListBox.pas:266
    public void SetImageLibraryTsIndex(int value)
    {
        if (value != _imageLibraryIndexTSelected)
        {
            _imageLibraryIndexTSelected = value;
            Valid = false;
        }
    }

    public int ListOffset { get => FirstShownItem; set => SetListOff(value); }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    [Published]
    public NotifyEvent<ListBox> OnDoubleClick { get => _onDoubleClick; set => _onDoubleClick = value; }

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

    [Published]
    public AoWOnDrawItemEvent<ListBox> OnDrawItem { get => _onDrawItem; set => _onDrawItem = value; }

    [Published]
    public AoWOnDrawItemEvent<ListBox> OnPreDrawItem { get => _onPreDrawItem; set => _onPreDrawItem = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTNormal; set => SetImageLibraryTnIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    [Published]
    public Menu PopupMenu { get => _popupMenu; set => _popupMenu = value; }

    [Published]
    public TextAlignment TextAlign { get => _textAlign; set => _textAlign = value; }

    public new NotifyEvent<ListBox> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<ListBox> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<ListBox> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<ListBox> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<ListBox> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<ListBox> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<ListBox> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<ListBox> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<ListBox> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
