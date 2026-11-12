// Translated from AoWComboBox.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;

using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// { The AoWComboBox unit of aowInt.dpl: TAOWComboBox, a drop-down list. Closed, it shows the
//   selected item in an edit field; a click opens the list below the field (at PriorityOpened, with
//   a vertical scroll bar when one is attached), a click on an item selects it and closes the list. }
// Fields: F114 (the mouse button went down on the field), FOpen (the list is shown), FItems,
// FVScrollBar, F124 (the rows the open list shows), FNumberVisible, F12C (the first item
// shown), FListBorderWidth, FListBorderHeight, FSelected, F13C (the item under the mouse, -1
// for none), FPriorityOpened, F144 (the priority before the list was opened), FOnDrawItem, F150
// (the width the scroll bar takes from the list), F154 (the scroll bar is visible), FILEdit,
// _imageLibraryFrame, _imageLibraryTextureNormal, _imageLibraryTextureSelected, _itemHeight, _onOpen, _onClose, _ileIndex,
// FILFIndex, FILTnIndex, FILTsIndex, FDrawItem, FDrawItemRect (the item and its rectangle while
// OnDrawItem runs)
public partial class ComboBox : Control
{
    public bool MouseDownOnField;
    public bool Open;
    public StringList _items;
    public VerticalScrollBar _verticalScrollBar;
    public int OpenListRows;
    public int _numberVisible;
    public int FirstShownItem;
    public int _listBorderWidth;
    public int _listBorderHeight;
    public int _selected;
    public int HoverItem;
    public int _priorityOpened;
    public int PriorityBeforeOpen;
    public NotifyEvent<ComboBox> _onDrawItem;
    public int ScrollBarWidth;
    public bool ScrollBarVisible;
    public InterfaceImageLibrary _imageLibraryEdit;
    public InterfaceImageLibrary _imageLibraryFrame;
    public InterfaceImageLibrary _imageLibraryTextureNormal;
    public InterfaceImageLibrary _imageLibraryTextureSelected;
    public int _itemHeight;
    public NotifyEvent<ComboBox> _onOpen;
    public NotifyEvent<ComboBox> _onClose;
    public int _ileIndex;
    public int _ilfIndex;
    public int _imageLibraryTnIndex;
    public int _imageLibraryTsIndex;
    public int DrawItem;
    public Rect DrawItemRectangle;

    // AoWComboBox.pas:114
    public ComboBox(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        HoverItem = -1;
    }

    // AoWComboBox.pas:122
    public override void Dispose()
    {
        _items.Dispose();
        base.Dispose();
    }

    // AoWComboBox.pas:128
    public override void Validate()
    {
        if (_imageLibraryEdit != null)
        {
            _imageLibraryEdit = _imageLibraryEdit.CheckImageLibrary(_ileIndex, 3, this);
        }

        if (_imageLibraryFrame != null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_ilfIndex, 8, this);
        }

        if (_imageLibraryTextureNormal != null)
        {
            _imageLibraryTextureNormal = _imageLibraryTextureNormal.CheckImageLibrary(_imageLibraryTnIndex, 1, this);
        }

        if (_imageLibraryTextureSelected != null)
        {
            _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryTsIndex, 1, this);
        }

        if (_font != null && _imageLibraryEdit != null && _imageLibraryFrame != null && _imageLibraryTextureNormal != null && _imageLibraryTextureSelected != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AoWComboBox.pas:146
    // { SetSize closes the list and sets up the scroll bar. The scroll bar's range and page are one
    //   item short of the open list's, and its top is taken from the first edit image (original) }
    public override void SetSize()
    {
        Open = false;
        var imageLibraryEditWidth = _imageLibraryEdit[_ileIndex];
        var imageLibraryEdit = _imageLibraryEdit[_ileIndex + 2];
        if (imageLibraryEditWidth.Width + imageLibraryEdit.Width > _winWidth)
        {
            var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
            var imageLibraryEdit3 = _imageLibraryEdit[_ileIndex + 2];
            _winWidth = imageLibraryEdit2.Width + imageLibraryEdit3.Width;
        }

        OpenListRows = _numberVisible;
        var count = _items.Count;
        if (OpenListRows > count)
        {
            OpenListRows = _items.Count;
        }

        if (_items.Count <= _selected)
        {
            _selected = _items.Count - 1;
        }

        _winHeight = _imageLibraryEdit[_ileIndex].Height;
        if (_verticalScrollBar != null)
        {
            if (_verticalScrollBar._window != _window)
            {
                _verticalScrollBar.SetWindow(_window);
            }

            _verticalScrollBar.Priority = _priority + 1;
            _verticalScrollBar.SetFMin(0);
            var fItemsCount = _items.Count;
            _verticalScrollBar.SetFMax(fItemsCount - 2);
            _verticalScrollBar.SetFPosition(FirstShownItem);
            _verticalScrollBar.SetFNumber(OpenListRows - 1);
            var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 6];
            _verticalScrollBar.WinHeight = imageLibraryFrame.Height + (_itemHeight * OpenListRows + _borderHeight);
            _verticalScrollBar.SetLarge(OpenListRows - 1);
            _verticalScrollBar.Visible = Open;
            if (_verticalScrollBar._vanishWhenFull && _items.Count - 1 < OpenListRows)
            {
                _verticalScrollBar.Visible = false;
            }

            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = _winLeft + _winWidth - _verticalScrollBar._winWidth;
            var imageLibraryEditHeight = _imageLibraryEdit[0];
            _verticalScrollBar.WinTop = imageLibraryEditHeight.Height + _winTop;
            ScrollBarVisible = _verticalScrollBar._visible;
        }

        base.SetSize();
    }

    // AoWComboBox.pas:356
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        base.Update();
        if (_visible)
        {
            Redraw = Open;
            if (_verticalScrollBar != null && _items.Count > 0)
            {
                if (_verticalScrollBar._position != FirstShownItem)
                {
                    FirstShownItem = _verticalScrollBar._position;
                    var count = _items.Count;
                    if (FirstShownItem + OpenListRows > count)
                    {
                        FirstShownItem = _items.Count - OpenListRows;
                    }

                    Drawn = false;
                }
            }
        }
    }

    // AoWComboBox.pas:204
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
        if (Open && _verticalScrollBar != null)
        {
            _verticalScrollBar.Visible = _visible;
        }
    }

    // AoWComboBox.pas:286
    // { Draw draws the edit field with the selected item and, when the list is open, the frame, the
    //   texture of the list, the highlight of the item under the mouse and the items, clipped short of
    //   the scroll bar; OnDrawItem is called for every item with FDrawItem and FDrawItemRect set }
    public override void Draw(LockedSurface lockedSurface)
    {
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
                var rectangle = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop, _winWidth, _winHeight);
                _imageLibraryEdit.DrawHorizontalStrip(_ileIndex, rectangle, ref lockedSurface);
                if (_selected > -1 && _items.Count > _selected)
                {
                    var item = _items[_selected];
                    _font.SetText(_window._winLeft + _winLeft + _borderWidth, _window._winTop + _winTop + _borderHeight, item, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
                if (Open == true)
                {
                    var imageLibraryFrameHeight = _imageLibraryFrame[_ilfIndex + 1];
                    var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 6];
                    var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                    var rectangle2 = new Rect(_window._winLeft + _winLeft, _window._winTop + _winTop + imageLibraryEditHeight.Height, _winWidth, imageLibraryFrameHeight.Height + imageLibraryFrame.Height + _itemHeight * OpenListRows + _listBorderHeight);
                    _imageLibraryFrame.DrawFrame(_ilfIndex, rectangle2, ref lockedSurface);
                    var imageLibraryFrameWidth = _imageLibraryFrame[_ilfIndex + 3];
                    var imageLibraryFrame2 = _imageLibraryFrame[_ilfIndex + 4];
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    var imageLibraryFrame3 = _imageLibraryFrame[_ilfIndex + 1];
                    var imageLibraryFrame4 = _imageLibraryFrame[_ilfIndex + 3];
                    var rectangle3 = new Rect(_window._winLeft + _winLeft + imageLibraryFrame4.Width, _window._winTop + _winTop + imageLibraryEdit.Height + imageLibraryFrame3.Height, _winWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _itemHeight * OpenListRows + _listBorderHeight);
                    _imageLibraryTextureNormal.DrawTiled(_imageLibraryTnIndex, rectangle3, ref lockedSurface);
                    if (HoverItem >= FirstShownItem && HoverItem < OpenListRows + FirstShownItem)
                    {
                        var imageLibraryFrame5 = _imageLibraryFrame[_ilfIndex + 3];
                        var imageLibraryFrame6 = _imageLibraryFrame[_ilfIndex + 4];
                        var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
                        var imageLibraryFrame7 = _imageLibraryFrame[_ilfIndex + 1];
                        var imageLibraryFrame8 = _imageLibraryFrame[_ilfIndex + 3];
                        var rectangle4 = new Rect(_window._winLeft + _winLeft + imageLibraryFrame8.Width, _window._winTop + _winTop + imageLibraryEdit2.Height + imageLibraryFrame7.Height + (HoverItem - FirstShownItem) * _itemHeight, _winWidth - imageLibraryFrame5.Width - imageLibraryFrame6.Width, _itemHeight);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryTsIndex, rectangle4, ref lockedSurface);
                    }

                    if (_window._winLeft + _winLeft + _winWidth - ScrollBarWidth < oldClip.Right)
                    {
                        lockedSurface.ClipRectangle.Right = _window._winLeft + _winLeft + _winWidth - ScrollBarWidth;
                    }

                    for (int I = 0, last = OpenListRows - 1; I <= last; I++)
                    {
                        if (I < _items.Count)
                        {
                            if (FirstShownItem + I == HoverItem)
                            {
                                _font.Color = SelectedColor;
                            }

                            var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
                            var imageLibraryFrame5 = _imageLibraryFrame[_ilfIndex + 1];
                            var item = _items[FirstShownItem + I];
                            _font.SetText(_window._winLeft + _winLeft + _listBorderWidth, _window._winTop + _winTop + imageLibraryEdit2.Height + imageLibraryFrame5.Height + _listBorderHeight + _itemHeight * I, item, lockedSurface);
                            if (FirstShownItem + I == HoverItem)
                            {
                                _font.Color = Color;
                            }

                            DrawItem = FirstShownItem + I;
                            var imageLibraryEdit3 = _imageLibraryEdit[_ileIndex + 1];
                            var imageLibraryFrame6 = _imageLibraryFrame[_ilfIndex + 1];
                            var imageLibraryEdit4 = _imageLibraryEdit[_ileIndex + 1];
                            var imageLibraryFrame7 = _imageLibraryFrame[_ilfIndex + 1];
                            var imageLibraryFrame8 = _imageLibraryFrame[_ilfIndex + 3];
                            var imageLibraryFrame9 = _imageLibraryFrame[_ilfIndex + 3];
                            DrawItemRectangle = new Rect(_window._winLeft + _winLeft + imageLibraryFrame9.Width, _window._winTop + _winTop + imageLibraryEdit4.Height + imageLibraryFrame7.Height + _itemHeight * I, _window._winLeft + _winLeft + imageLibraryFrame8.Width + _winWidth, _window._winTop + _winTop + imageLibraryEdit3.Height + imageLibraryFrame6.Height + (I + 1) * _itemHeight);
                            if (_onDrawItem != null)
                            {
                                _onDrawItem(this);
                            }
                        }
                    }

                    lockedSurface.ClipRectangle = oldClip;
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

    // AoWComboBox.pas:375
    // { CheckMouseDown opens or closes the list with a click on the edit field and closes it with a
    //   click outside the control }
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        bool condition = X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1;
        if (condition)
        {
            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
            condition = Y < _window._winTop + _winTop + imageLibraryEdit.Height;
        }

        if (condition)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                MouseDown = true;
                MouseDownOnField = true;
                if (Valid == true && _visible == true)
                {
                    if (Open)
                    {
                        if (PriorityBeforeOpen != _priority)
                        {
                            Priority = PriorityBeforeOpen;
                        }

                        Open = false;
                        _winHeight = _imageLibraryEdit[_ileIndex].Height;
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        if (_onClose != null)
                        {
                            _onClose(this);
                        }
                    }
                    else
                    {
                        PriorityBeforeOpen = _priority;
                        if (_priorityOpened != _priority)
                        {
                            Priority = _priorityOpened;
                        }

                        Open = true;
                        if (_items.Count < OpenListRows)
                        {
                            OpenListRows = _items.Count;
                        }

                        var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                        var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 1];
                        var imageLibraryFrameHeight = _imageLibraryFrame[_ilfIndex + 6];
                        _winHeight = imageLibraryEditHeight.Height + _listBorderHeight + _itemHeight * OpenListRows + imageLibraryFrame.Height + imageLibraryFrameHeight.Height;
                        if (_items.Count < FirstShownItem + OpenListRows)
                        {
                            FirstShownItem = _items.Count - OpenListRows;
                        }

                        Drawn = false;
                        ScrollBarWidth = _imageLibraryFrame[_ilfIndex + 4].Width;
                        ScrollBarVisible = false;
                        if (_verticalScrollBar != null && OpenListRows > 0)
                        {
                            if (_priority + 1 != _verticalScrollBar._priority)
                            {
                                _verticalScrollBar.Priority = _priority + 1;
                            }

                            _verticalScrollBar.SetFMin(0);
                            var count = _items.Count;
                            _verticalScrollBar.SetFMax(count - 1);
                            _verticalScrollBar.SetFPosition(FirstShownItem);
                            _verticalScrollBar.SetFNumber(OpenListRows);
                            var imageLibraryFrame2 = _imageLibraryFrame[_ilfIndex + 6];
                            _verticalScrollBar.WinHeight = imageLibraryFrame2.Height + (_itemHeight * OpenListRows + _borderHeight);
                            _verticalScrollBar.Visible = true;
                            if (_verticalScrollBar._vanishWhenFull == true && _items.Count - 1 < OpenListRows)
                            {
                                _verticalScrollBar.Visible = false;
                            }
                            else
                            {
                                ScrollBarWidth = _verticalScrollBar._winWidth;
                            }

                            ScrollBarVisible = _verticalScrollBar._visible;
                            _verticalScrollBar.Sized = false;
                            _verticalScrollBar.WinLeft = _winLeft + _winWidth - _verticalScrollBar._winWidth;
                            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                            _verticalScrollBar.WinTop = imageLibraryEdit.Height + _winTop;
                            _verticalScrollBar.Drawn = false;
                        }

                        if (_onOpen != null)
                        {
                            _onOpen(this);
                        }
                    }
                }
                else
                {
                    if (Open == true)
                    {
                        if (PriorityBeforeOpen != _priority)
                        {
                            Priority = PriorityBeforeOpen;
                        }

                        Open = false;
                        _winHeight = _imageLibraryEdit[_ileIndex].Height;
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        if (_onClose != null)
                        {
                            _onClose(this);
                        }
                    }
                }
            }

            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X, Y);
            }
        }
        else
        {
            if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
            {
                Handled = true;
            }
            else
            {
                if (Open)
                {
                    if (PriorityBeforeOpen != _priority)
                    {
                        Priority = PriorityBeforeOpen;
                    }

                    Open = false;
                    _winHeight = _imageLibraryEdit[_ileIndex].Height;
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.Visible = false;
                    }

                    if (_onClose != null)
                    {
                        _onClose(this);
                    }
                }
            }
        }
    }

    // AoWComboBox.pas:476
    // { CheckMouseUp selects the item under the mouse in the open list and closes the list }
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        int I = default;
        Handled = false;
        MouseDown = false;
        MouseDownOnField = false;
        Redraw = false;
        bool condition = X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1;
        if (condition)
        {
            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
            condition = Y < _window._winTop + _winTop + imageLibraryEdit.Height;
        }

        if (condition)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
        else
        {
            if (button == MouseButton.Left && Valid == true && _visible == true && Open == true)
            {
                bool test = X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth;
                if (test)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    test = Y > _window._winTop + _winTop + imageLibraryEdit.Height - 1;
                }

                if (test && Y < _window._winTop + _winTop + _winHeight)
                {
                    if (X < _window._winLeft + _winLeft + _winWidth - ScrollBarWidth)
                    {
                        var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                        I = (Y - _window._winTop - _winTop - _borderHeight - imageLibraryEdit.Height) / _itemHeight + FirstShownItem;
                        if (I < _items.Count)
                        {
                            _selected = I;
                        }

                        Open = false;
                        Redraw = false;
                        _winHeight = _imageLibraryEdit[_ileIndex].Height;
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        Handled = true;
                        if (_onChange != null)
                        {
                            _onChange(this);
                        }

                        if (_onClose != null)
                        {
                            _onClose(this);
                        }
                    }
                }
                else
                {
                    Open = false;
                    Redraw = false;
                    _winHeight = _imageLibraryEdit[_ileIndex].Height;
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.Visible = false;
                    }

                    if (_onClose != null)
                    {
                        _onClose(this);
                    }
                }
            }
        }
    }

    // AoWComboBox.pas:530
    // { CheckMouseMove tracks the item under the mouse in the open list }
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        int oldItem;
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        oldItem = HoverItem;
        HoverItem = -1;
        if (X >= _window._winLeft + _winLeft && X < _window._winLeft + _winLeft + _winWidth - ScrollBarWidth && Y >= _window._winTop + _winTop && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (Open)
            {
                var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                bool condition = Y >= _window._winTop + _winTop + _borderHeight + imageLibraryEditHeight.Height;
                if (condition)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    condition = Y < _window._winTop + _winTop + imageLibraryEdit.Height + _borderHeight + _itemHeight * OpenListRows;
                }

                if (condition)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    HoverItem = (Y - _window._winTop - _winTop - _borderHeight - imageLibraryEdit.Height) / _itemHeight + FirstShownItem;
                }
            }

            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }

        if (Open == true && oldItem != HoverItem)
        {
            Drawn = false;
            if (_verticalScrollBar != null)
            {
                _verticalScrollBar.Drawn = false;
            }
        }
    }

    // AoWComboBox.pas:211
    public virtual void SetImageLibraryEdit(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryEdit)
        {
            Valid = false;
            _imageLibraryEdit = value;
        }
    }

    // AoWComboBox.pas:220
    public virtual void SetILEIndex(int value)
    {
        if (value != _ileIndex)
        {
            _ileIndex = value;
            Valid = false;
        }
    }

    // AoWComboBox.pas:229
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWComboBox.pas:238
    public virtual void SetILFIndex(int value)
    {
        if (value != _ilfIndex)
        {
            _ilfIndex = value;
            Valid = false;
        }
    }

    // AoWComboBox.pas:247
    public virtual void SetImageLibraryTileNorm(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTextureNormal)
        {
            Valid = false;
            _imageLibraryTextureNormal = value;
        }
    }

    // AoWComboBox.pas:256
    public virtual void SetImageLibraryTnIndex(int value)
    {
        if (value != _imageLibraryTnIndex)
        {
            _imageLibraryTnIndex = value;
            Valid = false;
        }
    }

    // AoWComboBox.pas:265
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary value)
    {
        if (value != _imageLibraryTextureSelected)
        {
            Valid = false;
            _imageLibraryTextureSelected = value;
        }
    }

    // AoWComboBox.pas:274
    public virtual void SetImageLibraryTsIndex(int value)
    {
        if (value != _imageLibraryTsIndex)
        {
            _imageLibraryTsIndex = value;
            Valid = false;
        }
    }

    // AoWComboBox.pas:179
    public virtual void SetIndex(int value)
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

    // AoWComboBox.pas:190
    public void SetFStrings(StringList value)
    {
        _items.Clear();
        _items.AddStrings(value);
    }

    // AoWComboBox.pas:196
    public void SetVScroll(VerticalScrollBar value)
    {
        _verticalScrollBar = value;
        if (value != null)
        {
            value.Priority = _priority;
        }

        Sized = false;
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public int BorderWidth { get => _borderWidth; set => _borderWidth = value; }

    [Published]
    public int BorderHeight { get => _borderHeight; set => _borderHeight = value; }

    [Published]
    public int ListBorderWidth { get => _listBorderWidth; set => _listBorderWidth = value; }

    [Published]
    public int ListBorderHeight { get => _listBorderHeight; set => _listBorderHeight = value; }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    [Published]
    public int NumberVisible { get => _numberVisible; set => _numberVisible = value; }

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

    [Published]
    public int PriorityOpened { get => _priorityOpened; set => _priorityOpened = value; }

    [Published]
    public NotifyEvent<ComboBox> OnDrawItem { get => _onDrawItem; set => _onDrawItem = value; }

    [Published]
    public InterfaceImageLibrary ImageLibraryEdit { get => _imageLibraryEdit; set => SetImageLibraryEdit(value); }

    [Published]
    public int ILEIndex { get => _ileIndex; set => SetILEIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public int ILFIndex { get => _ilfIndex; set => SetILFIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public int ImageLibraryTnIndex { get => _imageLibraryTnIndex; set => SetImageLibraryTnIndex(value); }

    [Published]
    public InterfaceImageLibrary ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryTsIndex { get => _imageLibraryTsIndex; set => SetImageLibraryTsIndex(value); }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    [Published]
    public NotifyEvent<ComboBox> OnOpen { get => _onOpen; set => _onOpen = value; }

    [Published]
    public NotifyEvent<ComboBox> OnClose { get => _onClose; set => _onClose = value; }

    public new NotifyEvent<ComboBox> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<ComboBox> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<ComboBox> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<ComboBox> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<ComboBox> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<ComboBox> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<ComboBox> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<ComboBox> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<ComboBox> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
