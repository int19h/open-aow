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
// _imageLibraryFrame, _imageLibraryTextureNormal, _imageLibraryTextureSelected, _itemHeight, OnOpen, OnClose, _ileIndex,
// FILFIndex, FILTnIndex, FILTsIndex, FDrawItem, FDrawItemRect (the item and its rectangle while
// OnDrawItem runs)
public partial class ComboBox : Control
{

    private bool _open;
    private StringList _items;
    private VerticalScrollBar _verticalScrollBar;
    private int _openListRows;
    private int _numberVisible;
    private int _firstShownItem;
    private int _listBorderWidth;
    private int _listBorderHeight;
    private int _selected;
    private int _hoverItem;
    private int _priorityOpened;
    private int _priorityBeforeOpen;
    [Published]
    public event NotifyEvent<ComboBox> OnDrawItem;
    private int _scrollBarWidth;

    private InterfaceImageLibrary _imageLibraryEdit;
    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTextureNormal;
    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private int _itemHeight;
    [Published]
    public event NotifyEvent<ComboBox> OnOpen;
    [Published]
    public event NotifyEvent<ComboBox> OnClose;
    private int _ileIndex;
    private int _ilfIndex;
    private int _imageLibraryTnIndex;
    private int _imageLibraryTsIndex;

    // AoWComboBox.pas:114
    public ComboBox(Component owner) : base(owner)
    {
        _items = new StringList();
        _items.OnChange = OnChangeText;
        _hoverItem = -1;
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
        _open = false;
        var imageLibraryEditWidth = _imageLibraryEdit[_ileIndex];
        var imageLibraryEdit = _imageLibraryEdit[_ileIndex + 2];
        if (imageLibraryEditWidth.Width + imageLibraryEdit.Width > WinWidth)
        {
            var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
            var imageLibraryEdit3 = _imageLibraryEdit[_ileIndex + 2];
            SetWinWidthWithoutInvalidate(imageLibraryEdit2.Width + imageLibraryEdit3.Width);
        }

        _openListRows = _numberVisible;
        var count = _items.Count;
        if (_openListRows > count)
        {
            _openListRows = _items.Count;
        }

        if (_items.Count <= _selected)
        {
            _selected = _items.Count - 1;
        }

        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
        if (_verticalScrollBar != null)
        {
            if (_verticalScrollBar.Window != Window)
            {
                _verticalScrollBar.SetWindow(Window);
            }

            _verticalScrollBar.Priority = Priority + 1;
            _verticalScrollBar.SetFMin(0);
            var fItemsCount = _items.Count;
            _verticalScrollBar.SetFMax(fItemsCount - 2);
            _verticalScrollBar.SetFPosition(_firstShownItem);
            _verticalScrollBar.SetFNumber(_openListRows - 1);
            var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 6];
            _verticalScrollBar.WinHeight = imageLibraryFrame.Height + (_itemHeight * _openListRows + _borderHeight);
            _verticalScrollBar.SetLarge(_openListRows - 1);
            _verticalScrollBar.Visible = _open;
            if (_verticalScrollBar.VanishWhenFull && _items.Count - 1 < _openListRows)
            {
                _verticalScrollBar.Visible = false;
            }

            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = WinLeft + WinWidth - _verticalScrollBar.WinWidth;
            var imageLibraryEditHeight = _imageLibraryEdit[0];
            _verticalScrollBar.WinTop = imageLibraryEditHeight.Height + WinTop;
        }

        base.SetSize();
    }

    // AoWComboBox.pas:356
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        base.Update();
        if (Visible)
        {
            Redraw = _open;
            if (_verticalScrollBar != null && _items.Count > 0)
            {
                if (_verticalScrollBar.Position != _firstShownItem)
                {
                    _firstShownItem = _verticalScrollBar.Position;
                    var count = _items.Count;
                    if (_firstShownItem + _openListRows > count)
                    {
                        _firstShownItem = _items.Count - _openListRows;
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
        if (_open && _verticalScrollBar != null)
        {
            _verticalScrollBar.Visible = Visible;
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
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                _imageLibraryEdit.DrawHorizontalStrip(_ileIndex, rectangle, ref lockedSurface);
                if (_selected > -1 && _items.Count > _selected)
                {
                    var item = _items[_selected];
                    _font.SetText(Window.WinLeft + WinLeft + _borderWidth, Window.WinTop + WinTop + _borderHeight, item, lockedSurface);
                }

                lockedSurface.ClipRectangle = oldClip;
                if (_open == true)
                {
                    var imageLibraryFrameHeight = _imageLibraryFrame[_ilfIndex + 1];
                    var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 6];
                    var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                    var rectangle2 = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop + imageLibraryEditHeight.Height, WinWidth, imageLibraryFrameHeight.Height + imageLibraryFrame.Height + _itemHeight * _openListRows + _listBorderHeight);
                    _imageLibraryFrame.DrawFrame(_ilfIndex, rectangle2, ref lockedSurface);
                    var imageLibraryFrameWidth = _imageLibraryFrame[_ilfIndex + 3];
                    var imageLibraryFrame2 = _imageLibraryFrame[_ilfIndex + 4];
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    var imageLibraryFrame3 = _imageLibraryFrame[_ilfIndex + 1];
                    var imageLibraryFrame4 = _imageLibraryFrame[_ilfIndex + 3];
                    var rectangle3 = new Rect(Window.WinLeft + WinLeft + imageLibraryFrame4.Width, Window.WinTop + WinTop + imageLibraryEdit.Height + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, _itemHeight * _openListRows + _listBorderHeight);
                    _imageLibraryTextureNormal.DrawTiled(_imageLibraryTnIndex, rectangle3, ref lockedSurface);
                    if (_hoverItem >= _firstShownItem && _hoverItem < _openListRows + _firstShownItem)
                    {
                        var imageLibraryFrame5 = _imageLibraryFrame[_ilfIndex + 3];
                        var imageLibraryFrame6 = _imageLibraryFrame[_ilfIndex + 4];
                        var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
                        var imageLibraryFrame7 = _imageLibraryFrame[_ilfIndex + 1];
                        var imageLibraryFrame8 = _imageLibraryFrame[_ilfIndex + 3];
                        var rectangle4 = new Rect(Window.WinLeft + WinLeft + imageLibraryFrame8.Width, Window.WinTop + WinTop + imageLibraryEdit2.Height + imageLibraryFrame7.Height + (_hoverItem - _firstShownItem) * _itemHeight, WinWidth - imageLibraryFrame5.Width - imageLibraryFrame6.Width, _itemHeight);
                        _imageLibraryTextureSelected.DrawTiled(_imageLibraryTsIndex, rectangle4, ref lockedSurface);
                    }

                    if (Window.WinLeft + WinLeft + WinWidth - _scrollBarWidth < oldClip.Right)
                    {
                        lockedSurface.ClipRectangle.Right = Window.WinLeft + WinLeft + WinWidth - _scrollBarWidth;
                    }

                    for (int I = 0, last = _openListRows - 1; I <= last; I++)
                    {
                        if (I < _items.Count)
                        {
                            if (_firstShownItem + I == _hoverItem)
                            {
                                _font.Color = SelectedColor;
                            }

                            var imageLibraryEdit2 = _imageLibraryEdit[_ileIndex];
                            var imageLibraryFrame5 = _imageLibraryFrame[_ilfIndex + 1];
                            var item = _items[_firstShownItem + I];
                            _font.SetText(Window.WinLeft + WinLeft + _listBorderWidth, Window.WinTop + WinTop + imageLibraryEdit2.Height + imageLibraryFrame5.Height + _listBorderHeight + _itemHeight * I, item, lockedSurface);
                            if (_firstShownItem + I == _hoverItem)
                            {
                                _font.Color = Color;
                            }
                            var imageLibraryEdit3 = _imageLibraryEdit[_ileIndex + 1];
                            var imageLibraryFrame6 = _imageLibraryFrame[_ilfIndex + 1];
                            var imageLibraryEdit4 = _imageLibraryEdit[_ileIndex + 1];
                            var imageLibraryFrame7 = _imageLibraryFrame[_ilfIndex + 1];
                            var imageLibraryFrame8 = _imageLibraryFrame[_ilfIndex + 3];
                            var imageLibraryFrame9 = _imageLibraryFrame[_ilfIndex + 3];
                            if (OnDrawItem != null)
                            {
                                OnDrawItem(this);
                            }
                        }
                    }

                    lockedSurface.ClipRectangle = oldClip;
                }

                RaiseDraw(this, lockedSurface);
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
        bool condition = X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1;
        if (condition)
        {
            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
            condition = Y < Window.WinTop + WinTop + imageLibraryEdit.Height;
        }

        if (condition)
        {
            Handled = true;
            if (button == MouseButton.Left)
            {
                MouseDown = true;
                if (Valid == true && Visible == true)
                {
                    if (_open)
                    {
                        if (_priorityBeforeOpen != Priority)
                        {
                            Priority = _priorityBeforeOpen;
                        }

                        _open = false;
                        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        if (OnClose != null)
                        {
                            OnClose(this);
                        }
                    }
                    else
                    {
                        _priorityBeforeOpen = Priority;
                        if (_priorityOpened != Priority)
                        {
                            Priority = _priorityOpened;
                        }

                        _open = true;
                        if (_items.Count < _openListRows)
                        {
                            _openListRows = _items.Count;
                        }

                        var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                        var imageLibraryFrame = _imageLibraryFrame[_ilfIndex + 1];
                        var imageLibraryFrameHeight = _imageLibraryFrame[_ilfIndex + 6];
                        SetWinHeightWithoutInvalidate(imageLibraryEditHeight.Height + _listBorderHeight + _itemHeight * _openListRows + imageLibraryFrame.Height + imageLibraryFrameHeight.Height);
                        if (_items.Count < _firstShownItem + _openListRows)
                        {
                            _firstShownItem = _items.Count - _openListRows;
                        }

                        Drawn = false;
                        _scrollBarWidth = _imageLibraryFrame[_ilfIndex + 4].Width;
                        if (_verticalScrollBar != null && _openListRows > 0)
                        {
                            if (Priority + 1 != _verticalScrollBar.Priority)
                            {
                                _verticalScrollBar.Priority = Priority + 1;
                            }

                            _verticalScrollBar.SetFMin(0);
                            var count = _items.Count;
                            _verticalScrollBar.SetFMax(count - 1);
                            _verticalScrollBar.SetFPosition(_firstShownItem);
                            _verticalScrollBar.SetFNumber(_openListRows);
                            var imageLibraryFrame2 = _imageLibraryFrame[_ilfIndex + 6];
                            _verticalScrollBar.WinHeight = imageLibraryFrame2.Height + (_itemHeight * _openListRows + _borderHeight);
                            _verticalScrollBar.Visible = true;
                            if (_verticalScrollBar.VanishWhenFull == true && _items.Count - 1 < _openListRows)
                            {
                                _verticalScrollBar.Visible = false;
                            }
                            else
                            {
                                _scrollBarWidth = _verticalScrollBar.WinWidth;
                            }
                            _verticalScrollBar.Sized = false;
                            _verticalScrollBar.WinLeft = WinLeft + WinWidth - _verticalScrollBar.WinWidth;
                            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                            _verticalScrollBar.WinTop = imageLibraryEdit.Height + WinTop;
                            _verticalScrollBar.Drawn = false;
                        }

                        if (OnOpen != null)
                        {
                            OnOpen(this);
                        }
                    }
                }
                else
                {
                    if (_open == true)
                    {
                        if (_priorityBeforeOpen != Priority)
                        {
                            Priority = _priorityBeforeOpen;
                        }

                        _open = false;
                        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        if (OnClose != null)
                        {
                            OnClose(this);
                        }
                    }
                }
            }

            RaiseMouseDown(this, button, shift, X, Y);
        }
        else
        {
            if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
            {
                Handled = true;
            }
            else
            {
                if (_open)
                {
                    if (_priorityBeforeOpen != Priority)
                    {
                        Priority = _priorityBeforeOpen;
                    }

                    _open = false;
                    SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.Visible = false;
                    }

                    if (OnClose != null)
                    {
                        OnClose(this);
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
        Redraw = false;
        bool condition = X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1;
        if (condition)
        {
            var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
            condition = Y < Window.WinTop + WinTop + imageLibraryEdit.Height;
        }

        if (condition)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
        else
        {
            if (button == MouseButton.Left && Valid == true && Visible == true && _open == true)
            {
                bool test = X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth;
                if (test)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    test = Y > Window.WinTop + WinTop + imageLibraryEdit.Height - 1;
                }

                if (test && Y < Window.WinTop + WinTop + WinHeight)
                {
                    if (X < Window.WinLeft + WinLeft + WinWidth - _scrollBarWidth)
                    {
                        var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                        I = (Y - Window.WinTop - WinTop - _borderHeight - imageLibraryEdit.Height) / _itemHeight + _firstShownItem;
                        if (I < _items.Count)
                        {
                            _selected = I;
                        }

                        _open = false;
                        Redraw = false;
                        SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
                        if (_verticalScrollBar != null)
                        {
                            _verticalScrollBar.Visible = false;
                        }

                        Handled = true;
                        RaiseChange(this);

                        if (OnClose != null)
                        {
                            OnClose(this);
                        }
                    }
                }
                else
                {
                    _open = false;
                    Redraw = false;
                    SetWinHeightWithoutInvalidate(_imageLibraryEdit[_ileIndex].Height);
                    if (_verticalScrollBar != null)
                    {
                        _verticalScrollBar.Visible = false;
                    }

                    if (OnClose != null)
                    {
                        OnClose(this);
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
        oldItem = _hoverItem;
        _hoverItem = -1;
        if (X >= Window.WinLeft + WinLeft && X < Window.WinLeft + WinLeft + WinWidth - _scrollBarWidth && Y >= Window.WinTop + WinTop && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_open)
            {
                var imageLibraryEditHeight = _imageLibraryEdit[_ileIndex];
                bool condition = Y >= Window.WinTop + WinTop + _borderHeight + imageLibraryEditHeight.Height;
                if (condition)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    condition = Y < Window.WinTop + WinTop + imageLibraryEdit.Height + _borderHeight + _itemHeight * _openListRows;
                }

                if (condition)
                {
                    var imageLibraryEdit = _imageLibraryEdit[_ileIndex];
                    _hoverItem = (Y - Window.WinTop - WinTop - _borderHeight - imageLibraryEdit.Height) / _itemHeight + _firstShownItem;
                }
            }

            RaiseMouseMove(this, shift, X, Y);
        }

        if (_open == true && oldItem != _hoverItem)
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
            value.Priority = Priority;
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

    public new event NotifyEvent<ComboBox> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<ComboBox> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<ComboBox> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<ComboBox> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<ComboBox> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<ComboBox> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ComboBox> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ComboBox> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ComboBox> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, ComboBox>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, ComboBox>(this, slot: nameof(OnUpdate), value: value);
    }
}
