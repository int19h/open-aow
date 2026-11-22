#nullable disable warnings
// Translated from AoWListBox.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

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
    private readonly DelphiStringList _items;
    private VerticalScrollBar _verticalScrollBar;

    public int VisibleRows { get; set; }

    private int _selected;

    [Published]
    public event EventHandler<DrawItemEventArgs>? ItemDrawing;
    [Published]
    public event EventHandler<DrawItemEventArgs>? ItemDrawn;
    private int _scrollWidth;

    private InterfaceImageLibrary? _imageLibraryFrame;
    private InterfaceImageLibrary? _imageLibraryTextureNormal;
    private InterfaceImageLibrary? _imageLibraryTextureSelected;

    [Published]
    public event EventHandler? DoubleClicked;

    public int FirstShownItem { get; private set; }

    private int _textOnly;
    private int _imageLibraryIndexFrame;
    private int _imageLibraryIndexTNormal;
    private int _imageLibraryIndexTSelected;
    private int _drawItem;
    private Rect _drawItemRectangle;

    public int HoverItem { get; set; }

    // AoWListBox.pas:107
    public ListBox(Component? owner) : this(owner, items: [])
    {
    }

    // A list box whose items carry typed objects passes its own string list.
    protected ListBox(Component? owner, DelphiStringList items) : base(owner)
    {
        _items = items;
        _items.OnChange = OnChangeText;
        VisibleRows = 3;
        ItemHeight = 10;
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
        if (base.Font is not null)
        {
            if (_imageLibraryFrame is not null)
            {
                _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, count: 8, sender: this);
            }

            if (_imageLibraryTextureNormal is not null)
            {
                _imageLibraryTextureNormal = _imageLibraryTextureNormal.CheckImageLibrary(_imageLibraryIndexTNormal, count: 1, sender: this);
            }

            if (_imageLibraryTextureSelected is not null)
            {
                _imageLibraryTextureSelected = _imageLibraryTextureSelected.CheckImageLibrary(_imageLibraryIndexTSelected, count: 1, sender: this);
            }

            _textOnly = _imageLibraryFrame is not null && _imageLibraryTextureNormal is not null && _imageLibraryTextureSelected is not null ? 0 : 1;

            Valid = true;
        }

        base.Validate();
    }

    // AoWListBox.pas:143
    public override void SetSize()
    {
        if (ItemHeight < 1)
        {
            ItemHeight = 10;
        }

        switch (_textOnly)
        {
            case 0:
                LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                    SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
                }

                LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
                {
                    LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                    LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                    SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
                }

                LibraryImage? imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                VisibleRows = (WinHeight - imageLibraryFrame5.Height - imageLibraryFrame6.Height) / ItemHeight;
                _scrollWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 4].Width;
                Masked = false;
                break;
            case 1:
                VisibleRows = WinHeight / ItemHeight;
                _scrollWidth = 0;
                Masked = true;
                break;
            default:
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
        if (_verticalScrollBar is not null)
        {
            if (_verticalScrollBar.Window != Window)
            {
                _verticalScrollBar.SetWindow(Window);
            }

            _verticalScrollBar.Priority = Priority + 1;
            _verticalScrollBar.SetFMin(0);
            int count = _items.Count;
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
        if (Visible && _verticalScrollBar is not null && _items.Count > 0)
        {
            if (_verticalScrollBar.Position != FirstShownItem)
            {
                FirstShownItem = _verticalScrollBar.Position;
                int count = _items.Count;
                if (FirstShownItem + VisibleRows > count)
                {
                    FirstShownItem = _items.Count - VisibleRows;
                }

                if (FirstShownItem < 0)
                {
                    FirstShownItem = 0;
                }

                IsDrawn = false;
            }
        }

        base.Update();
    }

    // AoWListBox.pas:241
    public override void SetVisible(bool value)
    {
        base.SetVisible(value);
        _verticalScrollBar?.Visible = Visible;
    }

    // AoWListBox.pas:304
    // { Draw draws the frame and the texture, then the visible items (one more than fit: the loop runs
    //   to F11C, original) clipped to the inside, the selected one on its highlight texture }
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Valid && Visible)
            {
                RaisePreDraw(surfDesc: lockedSurface);

                base.Font.Color = Color;
                Rect oldClip = lockedSurface.ClipRectangle;
                int l;
                int w;
                int t;
                int h;
                switch (_textOnly)
                {
                    case 0:
                        SizedRectangle rectangle = new(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, WinWidth, WinHeight);
                        _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                        LibraryImage? imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        LibraryImage? imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        LibraryImage? imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        SizedRectangle rectangle2 = new(Window.AbsoluteLeft + WinLeft + imageLibraryFrame3.Width, Window.AbsoluteTop + WinTop + imageLibraryFrame2.Height, WinWidth - imageLibraryFrameWidth.Width - _scrollWidth, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                        _imageLibraryTextureNormal.DrawTiled(_imageLibraryIndexTNormal, area: rectangle2, ref lockedSurface);
                        LibraryImage? imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        l = Window.AbsoluteLeft + WinLeft + imageLibraryFrame4.Width;
                        LibraryImage? imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        t = Window.AbsoluteTop + WinTop + imageLibraryFrame5.Height;
                        LibraryImage? imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                        LibraryImage? imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                        h = WinHeight - imageLibraryFrame6.Height - imageLibraryFrame7.Height;
                        LibraryImage? imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                        w = WinWidth - imageLibraryFrame8.Width - _scrollWidth;
                        break;
                    default:
                        l = Window.AbsoluteLeft + WinLeft;
                        t = Window.AbsoluteTop + WinTop;
                        h = WinHeight;
                        w = WinWidth - _scrollWidth;
                        break;
                }

                if (l + w < oldClip.Right)
                {
                    Rect clipRectangle = lockedSurface.ClipRectangle;
                    clipRectangle.Right = l + w;
                    lockedSurface.ClipRectangle = clipRectangle;
                }

                if (t + h < oldClip.Bottom)
                {
                    Rect clipRectangle2 = lockedSurface.ClipRectangle;
                    clipRectangle2.Bottom = t + h;
                    lockedSurface.ClipRectangle = clipRectangle2;
                }

                for (int i = 0, last = VisibleRows; i <= last; i++)
                {
                    int count = _items.Count;
                    if (FirstShownItem + i < count)
                    {
                        _drawItem = FirstShownItem + i;
                        _drawItemRectangle = new Rect(l, (ItemHeight * i) + t, l + w, ((i + 1) * ItemHeight) + t);
                        if (_drawItemRectangle.Bottom > lockedSurface.ClipRectangle.Bottom)
                        {
                            _drawItemRectangle.Bottom = lockedSurface.ClipRectangle.Bottom;
                        }

                        DrawItemEventArgs args = new()
                        {
                            Rectangle = _drawItemRectangle,
                            Item = _drawItem,
                            Surface = lockedSurface
                        };
                        ItemDrawing?.Invoke(this, args);
                        lockedSurface = args.Surface;
                        if (FirstShownItem + i == _selected)
                        {
                            base.Font.Color = SelectedColor;
                            if (_textOnly == 0)
                            {
                                SizedRectangle rectangle = new(l, (ItemHeight * i) + t, w, ItemHeight);
                                _imageLibraryTextureSelected.DrawTiled(_imageLibraryIndexTSelected, area: rectangle, ref lockedSurface);
                            }
                        }
                        else
                        {
                            base.Font.Color = Color;
                        }

                        switch (TextAlign)
                        {
                            case TextAlignment.Left:
                                string item = _items[FirstShownItem + i];
                                base.Font.SetText(BorderWidth + l, BorderHeight + t + (ItemHeight * i), text: item, lockedSurface);
                                break;
                            case TextAlignment.Right:
                                string item2 = _items[FirstShownItem + i];
                                int textWidth = base.Font.GetTextWidth(item2);
                                string item3 = _items[FirstShownItem + i];
                                base.Font.SetText(l + w - BorderWidth - textWidth, BorderHeight + t + (ItemHeight * i), text: item3, lockedSurface);
                                break;
                            case TextAlignment.Center:
                            default:
                                string item4 = _items[FirstShownItem + i];
                                int fontTextWidth = base.Font.GetTextWidth(item4);
                                string item5 = _items[FirstShownItem + i];
                                base.Font.SetText(l + (w / 2) - (fontTextWidth / 2), BorderHeight + t + (ItemHeight * i), text: item5, lockedSurface);
                                break;
                        }

                        DrawItemEventArgs e = new()
                        {
                            Rectangle = _drawItemRectangle,
                            Item = _drawItem,
                            Surface = lockedSurface
                        };
                        ItemDrawn?.Invoke(this, e);
                        lockedSurface = e.Surface;
                    }
                }

                lockedSurface.ClipRectangle = oldClip;
                RaiseDraw(surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // AoWListBox.pas:404
    // { CheckMouseDown selects the item under the mouse unless the owner already handled the click;
    //   a double click calls OnDoubleClick, the right button opens the popup menu at the mouse, moved
    //   back inside the window manager when it would stick out }
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (!OwnerComponent.Handled && X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth - _scrollWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            int i;
            if (_textOnly == 0)
            {
                LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                i = ((Y - Window.AbsoluteTop - WinTop - imageLibraryFrame.Height) / ItemHeight) + FirstShownItem;
            }
            else
            {
                i = ((Y - Window.AbsoluteTop - WinTop) / ItemHeight) + FirstShownItem;
            }

            if (_items.Count > i)
            {
                if (i != _selected)
                {
                    _selected = i;
                    IsDrawn = false;
                    RaiseChange();
                }

                if (button == MouseButton.Left && (ShiftStates.Double & shift) == ShiftStates.Double && DoubleClicked is not null)
                {
                    DoubleClicked(this, e: EventArgs.Empty);
                }

                if (_selected > -1 && button == MouseButton.Right && PopupMenu is not null)
                {
                    PopupMenu.SetPopupOwner(this);
                    PopupMenu.WinLeft = X;
                    PopupMenu.WinTop = Y;
                    if (PopupMenu.WindowManager is not null)
                    {
                        if (PopupMenu.IsHidden)
                        {
                            if (PopupMenu.MenuLeft + PopupMenu.WinWidth > PopupMenu.WindowManager.WinWidth)
                            {
                                PopupMenu.WinLeft = X - PopupMenu.WinWidth;
                            }

                            if (PopupMenu.MenuTop + PopupMenu.WinHeight > PopupMenu.WindowManager.WinHeight)
                            {
                                PopupMenu.WinTop = Y - PopupMenu.WinHeight;
                            }
                        }
                        else
                        {
                            if (PopupMenu.WinLeft + PopupMenu.WinWidth > PopupMenu.WindowManager.WinWidth)
                            {
                                PopupMenu.WinLeft = X - PopupMenu.WinWidth;
                            }

                            if (PopupMenu.WinTop + PopupMenu.WinHeight > PopupMenu.WindowManager.WinHeight)
                            {
                                PopupMenu.WinTop = Y - PopupMenu.WinHeight;
                            }
                        }
                    }

                    PopupMenu.Visible = true;
                }
            }

            RaiseMouseDown(button, shift, X, Y);
        }
    }

    // AoWListBox.pas:460
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth - _scrollWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(button, shift, X, Y);
        }
    }

    // AoWListBox.pas:475
    // { CheckMouseMove tracks the item under the mouse }
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X < Window.AbsoluteLeft + WinLeft || X > Window.AbsoluteLeft + WinLeft + WinWidth - _scrollWidth - 1 || Y < Window.AbsoluteTop + WinTop || Y > Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            HoverItem = -1;
        }
        else
        {
            Handled = true;
            if (_textOnly == 0)
            {
                LibraryImage? imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame];
                HoverItem = ((Y - Window.AbsoluteTop - WinTop - imageLibraryFrame.Height) / ItemHeight) + FirstShownItem;
            }
            else
            {
                HoverItem = ((Y - Window.AbsoluteTop - WinTop) / ItemHeight) + FirstShownItem;
            }

            RaiseMouseMove(shift, X, Y);
        }
    }

    // AoWListBox.pas:497
    public override void CheckKeyPress(ref byte key) => Handled = false;

    // AoWListBox.pas:275
    public virtual void SetImageLibraryFrame(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryFrame)
        {
            Valid = false;
            _imageLibraryFrame = value;
        }
    }

    // AoWListBox.pas:284
    public virtual void SetImageLibraryTileNorm(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTextureNormal)
        {
            Valid = false;
            _imageLibraryTextureNormal = value;
        }
    }

    // AoWListBox.pas:293
    public virtual void SetImageLibraryTileSelected(InterfaceImageLibrary? value)
    {
        if (value != _imageLibraryTextureSelected)
        {
            Valid = false;
            _imageLibraryTextureSelected = value;
        }
    }

    // AoWListBox.pas:202
    public void SetFStrings(DelphiStringList value)
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
            IsDrawn = false;
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
            IsDrawn = false;
        }
    }

    // AoWListBox.pas:232
    public void SetVScroll(VerticalScrollBar value)
    {
        _verticalScrollBar = value;
        value?.Priority = Priority;

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
    public DelphiStringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public VerticalScrollBar VerticalScrollBar { get => _verticalScrollBar; set => SetVScroll(value); }

    internal void SetVerticalScrollBarWithoutLayout(VerticalScrollBar value) => _verticalScrollBar = value;

    [Published]
    public int Selected { get => _selected; set => SetIndex(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryFrame { get => _imageLibraryFrame; set => SetImageLibraryFrame(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureNormal { get => _imageLibraryTextureNormal; set => SetImageLibraryTileNorm(value); }

    [Published]
    public InterfaceImageLibrary? ImageLibraryTextureSelected { get => _imageLibraryTextureSelected; set => SetImageLibraryTileSelected(value); }

    [Published]
    public int ImageLibraryIndexFrame { get => _imageLibraryIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ImageLibraryIndexTNormal { get => _imageLibraryIndexTNormal; set => SetImageLibraryTnIndex(value); }

    [Published]
    public int ImageLibraryIndexTSelected { get => _imageLibraryIndexTSelected; set => SetImageLibraryTsIndex(value); }

    [Published]
    public int ItemHeight { get; set; }

    [Published]
    public Menu? PopupMenu { get; set; }

    [Published]
    public TextAlignment TextAlign { get; set; }
}
