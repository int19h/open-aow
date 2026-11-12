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
    private StringList _items;
    private VerticalScrollBar _verticalScrollBar;

    public int VisibleRows { get; set; }

    private int _selected;

    [Published]
    public event AoWOnDrawItemEvent<ListBox> OnPreDrawItem;
    [Published]
    public event AoWOnDrawItemEvent<ListBox> OnDrawItem;
    private int _scrollWidth;

    private InterfaceImageLibrary _imageLibraryFrame;
    private InterfaceImageLibrary _imageLibraryTextureNormal;
    private InterfaceImageLibrary _imageLibraryTextureSelected;
    private int _itemHeight;
    [Published]
    public event NotifyEvent<ListBox> OnDoubleClick;
    private TextAlignment _textAlign;

    public int FirstShownItem { get; private set; }

    private byte _textOnly;
    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexTNormal;
    private int _imageLibraryIndexTSelected;
    private Menu _popupMenu;
    private int _drawItem;
    private Rect _drawItemRectangle;

    public int HoverItem { get; set; }

    // AoWListBox.pas:107
    public ListBox(Component owner) : this(owner, items: new StringList())
    {
    }

    // A list box whose items carry typed objects passes its own string list.
    protected ListBox(Component owner, StringList items) : base(owner)
    {
        _items = items;
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
                _textOnly = 0;
            }
            else
            {
                _textOnly = 1;
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

        switch (_textOnly)
        {
            case 0:
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                    var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                VisibleRows = (WinHeight - imageLibraryFrame5.Height - imageLibraryFrame6.Height) / _itemHeight;
                _scrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
                Masked = false;
                break;
            case 1:
                VisibleRows = WinHeight / _itemHeight;
                _scrollWidth = 0;
                Masked = true;
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
        if (_verticalScrollBar != null)
        {
            if (_verticalScrollBar.Window != Window)
            {
                _verticalScrollBar.SetWindow(Window);
            }

            _verticalScrollBar.Priority = Priority + 1;
            _verticalScrollBar.SetFMin(0);
            var count = _items.Count;
            _verticalScrollBar.SetFMax(count - 1);
            _verticalScrollBar.SetFPosition(FirstShownItem);
            _verticalScrollBar.SetFNumber(VisibleRows);
            _verticalScrollBar.WinHeight = WinHeight;
            _verticalScrollBar.SetLarge(VisibleRows - 1);
            _verticalScrollBar.Visible = Visible;
            if (_verticalScrollBar.VanishWhenFull && _items.Count - 1 < VisibleRows)
            {
                _verticalScrollBar.Visible = false;
            }
            else
            {
                _scrollWidth = _verticalScrollBar.WinWidth;
            }

            _verticalScrollBar.Sized = false;
            _verticalScrollBar.WinLeft = WinLeft + WinWidth - _verticalScrollBar.WinWidth;
            _verticalScrollBar.WinTop = WinTop;
        }

        base.SetSize();
    }

    // AoWListBox.pas:386
    // { Update keeps the first item shown in step with the scroll bar }
    public override void Update()
    {
        if (Visible && _verticalScrollBar != null && _items.Count > 0)
        {
            if (_verticalScrollBar.Position != FirstShownItem)
            {
                FirstShownItem = _verticalScrollBar.Position;
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
            _verticalScrollBar.Visible = Visible;
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
            if (Valid && Visible)
            {
                RaisePreDraw(this, lockedSurface);

                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                switch (_textOnly)
                {
                    case 0:
                        var rectangle = new Rect(Window.WinLeft + WinLeft, Window.WinTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        var rectangle2 = new Rect(Window.WinLeft + WinLeft + imageLibraryFrame3.Width, Window.WinTop + WinTop + imageLibraryFrame2.Height, WinWidth - imageLibraryFrameWidth.Width - _scrollWidth, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, rectangle2, ref lockedSurface);
                        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        L = Window.WinLeft + WinLeft + imageLibraryFrame4.Width;
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        T = Window.WinTop + WinTop + imageLibraryFrame5.Height;
                        var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        H = WinHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        W = WinWidth - imageLibraryFrame8.Width - _scrollWidth;
                        break;
                    default:
                        L = Window.WinLeft + WinLeft;
                        T = Window.WinTop + WinTop;
                        H = WinHeight;
                        W = WinWidth - _scrollWidth;
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
                        _drawItem = FirstShownItem + I;
                        _drawItemRectangle = new Rect(L, _itemHeight * I + T, L + W, (I + 1) * _itemHeight + T);
                        if (_drawItemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            _drawItemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        if (OnPreDrawItem != null)
                        {
                            OnPreDrawItem(this, _drawItemRectangle, _drawItem, ref lockedSurface);
                        }

                        if (FirstShownItem + I == _selected)
                        {
                            _font.Color = SelectedColor;
                            if (_textOnly == 0)
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

                        if (OnDrawItem != null)
                        {
                            OnDrawItem(this, _drawItemRectangle, _drawItem, ref lockedSurface);
                        }
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
                RaiseDraw(this, lockedSurface);
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
        if (!OwnerComponent.Handled && X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth - _scrollWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            if (_textOnly == 0)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                I = (Y - Window.WinTop - WinTop - imageLibraryFrame.Height) / _itemHeight + FirstShownItem;
            }
            else
            {
                I = (Y - Window.WinTop - WinTop) / _itemHeight + FirstShownItem;
            }

            if (_items.Count > I)
            {
                if (I != _selected)
                {
                    _selected = I;
                    Drawn = false;
                    RaiseChange(this);
                }

                if (button == MouseButton.Left && (Set<ShiftState>.Of(ShiftState.Double) & shift) == Set<ShiftState>.Of(ShiftState.Double) && OnDoubleClick != null)
                {
                    OnDoubleClick(this);
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

            RaiseMouseDown(this, button, shift, X, Y);
        }
    }

    // AoWListBox.pas:460
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth - _scrollWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // AoWListBox.pas:475
    // { CheckMouseMove tracks the item under the mouse }
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X < Window.WinLeft + WinLeft || X > Window.WinLeft + WinLeft + WinWidth - _scrollWidth - 1 || Y < Window.WinTop + WinTop || Y > Window.WinTop + WinTop + WinHeight - 1)
        {
            HoverItem = -1;
        }
        else
        {
            Handled = true;
            if (_textOnly == 0)
            {
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                HoverItem = (Y - Window.WinTop - WinTop - imageLibraryFrame.Height) / _itemHeight + FirstShownItem;
            }
            else
            {
                HoverItem = (Y - Window.WinTop - WinTop) / _itemHeight + FirstShownItem;
            }

            RaiseMouseMove(this, shift, X, Y);
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
            value.Priority = Priority;
        }

        Priority = Priority;
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

    internal void SetVerticalScrollBarWithoutLayout(VerticalScrollBar value) => _verticalScrollBar = value;

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

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

    public new event NotifyEvent<ListBox> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<ListBox> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<ListBox> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<ListBox> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<ListBox> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<ListBox> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ListBox> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ListBox> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ListBox> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, ListBox>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, ListBox>(this, slot: nameof(OnUpdate), value: value);
    }
}
