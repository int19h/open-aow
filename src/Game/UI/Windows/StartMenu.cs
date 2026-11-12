// Translated from StartMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// whole-menu image library
public partial class StartMenu : Menu
{
    public StartMenu(Component owner) : base(owner)
    { }

    public ImageLibrary _imageLibraryWholeMenu;

    // StartMenu.pas:30
    public override void SetSize()
    {
        WindowManager M;
        base.SetSize();
        _winHeight = 377;
        M = _windowManager;
        if (M != null && _ownerMenuBar != null)
        {
            _winTop = M._winHeight - 393;
        }

        _winLeft = 0;
    }

    // StartMenu.pas:41
    public override void SetVisible(bool value)
    {
        WindowManager M = default;
        MenuBar B = default;
        if (value != _visible)
        {
            _visible = value;
            if (_visible == true)
            {
                Highlighted = -1;
                _winHeight = 377;
                M = _windowManager;
                if (M != null && _ownerMenuBar != null)
                {
                    _winTop = M._winHeight - 393;
                }

                _winLeft = 0;
                Drawn = false;
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
            else
            {
                B = _ownerMenuBar;
                if (B != null)
                {
                    B._selected = -1;
                    B.Drawn = false;
                }

                Hidden = true;
                if (_onHide != null)
                {
                    _onHide(this);
                }
            }
        }

        if (_visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            Drawn = false;
            SetPriority(_priority);
        }
    }

    // StartMenu.pas:87
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        try
        {
            if (Valid && _visible)
            {
                if (!Drawn)
                {
                    oldClip = lockedSurface.ClipRectangle;
                    _font.Color = Color;
                    _imageLibraryWholeMenu[Highlighted + 1].ShowClipped(0, 0, lockedSurface);
                    for (int I = 0, last = _items.Count - 1; I <= last; I++)
                    {
                        if (_items.Count > 0)
                        {
                            var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                            if (_winLeft + _winWidth - imageLibraryFrameWidth.Width < oldClip.Right)
                            {
                                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                                lockedSurface.ClipRectangle.Right = _winLeft + _winWidth - imageLibraryFrame.Width;
                            }

                            if (I == Highlighted)
                            {
                                _font.Color = SelectedColor;
                            }

                            var item = _items[I];
                            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            _font.SetText(_winLeft + _borderWidth + imageLibraryFrame2.Width, _winTop + _borderHeight + imageLibraryFrameHeight.Height + _itemHeight * I, item, lockedSurface);
                            if (I == Highlighted)
                            {
                                _font.Color = Color;
                            }

                            DrawItem = I;
                            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            DrawItemRectangle = new Rect(_winLeft + imageLibraryFrame5.Width, _winTop + imageLibraryFrame4.Height + _itemHeight * I, _winLeft + _winWidth, _winTop + imageLibraryFrame3.Height + (I + 1) * _itemHeight);
                            if (_onDrawItem != null)
                            {
                                _onDrawItem(this);
                            }

                            lockedSurface.ClipRectangle = oldClip;
                        }
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

    // StartMenu.pas:123
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (Valid)
        {
            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            _winHeight -= imageLibraryFrameHeight.Height;
            base.CheckMouseDown(button, shift, X, Y);
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            _winHeight += imageLibraryFrame.Height;
        }
    }

    [Published]
    public ImageLibrary ImageLibraryWholeMenu { get => _imageLibraryWholeMenu; set => _imageLibraryWholeMenu = value; }

    public new NotifyEvent<StartMenu> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<StartMenu> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<StartMenu> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<StartMenu> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<StartMenu> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<StartMenu> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<StartMenu> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<StartMenu> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<StartMenu> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }

    public new AoWOnDrawBackEvent<StartMenu> OnDrawBack
    {
        get => EventSlot.Get(this, _onDrawBack);
        set => EventSlot.Set(this, ref _onDrawBack, value);
    }

    public new NotifyEvent<StartMenu> OnEnter
    {
        get => EventSlot.Get(this, _onEnter);
        set => EventSlot.Set(this, ref _onEnter, value);
    }

    public new NotifyEvent<StartMenu> OnExit
    {
        get => EventSlot.Get(this, _onExit);
        set => EventSlot.Set(this, ref _onExit, value);
    }

    public new NotifyEvent<StartMenu> OnChangeCursor
    {
        get => EventSlot.Get(this, _onChangeCursor);
        set => EventSlot.Set(this, ref _onChangeCursor, value);
    }

    public new AoWOnDrawSurface<StartMenu> OnDrawSurface
    {
        get => EventSlot.Get(this, _onDrawSurface);
        set => EventSlot.Set(this, ref _onDrawSurface, value);
    }

    public new AoWOnDrawSurface<StartMenu> OnPreDrawSurface
    {
        get => EventSlot.Get(this, _onPreDrawSurface);
        set => EventSlot.Set(this, ref _onPreDrawSurface, value);
    }

    public new NotifyEvent<StartMenu> OnHide
    {
        get => EventSlot.Get(this, _onHide);
        set => EventSlot.Set(this, ref _onHide, value);
    }

    public new NotifyEvent<StartMenu> OnShow
    {
        get => EventSlot.Get(this, _onShow);
        set => EventSlot.Set(this, ref _onShow, value);
    }

    public new NotifyEvent<StartMenu> OnDrawItem
    {
        get => EventSlot.Get(this, _onDrawItem);
        set => EventSlot.Set(this, ref _onDrawItem, value);
    }

    public new NotifyEvent<StartMenu> OnSelect
    {
        get => EventSlot.Get(this, _onSelect);
        set => EventSlot.Set(this, ref _onSelect, value);
    }
}
