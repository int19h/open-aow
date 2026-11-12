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

    private ImageLibrary _imageLibraryWholeMenu;

    // StartMenu.pas:30
    public override void SetSize()
    {
        WindowManager M;
        base.SetSize();
        SetWinHeightWithoutInvalidate(377);
        M = _windowManager;
        if (M != null && OwnerMenuBar != null)
        {
            SetWinTopWithoutInvalidate(M.WinHeight - 393);
        }

        SetWinLeftWithoutInvalidate(0);
    }

    // StartMenu.pas:41
    public override void SetVisible(bool value)
    {
        WindowManager M = default;
        MenuBar B = default;
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (Visible == true)
            {
                Highlighted = -1;
                SetWinHeightWithoutInvalidate(377);
                M = _windowManager;
                if (M != null && OwnerMenuBar != null)
                {
                    SetWinTopWithoutInvalidate(M.WinHeight - 393);
                }

                SetWinLeftWithoutInvalidate(0);
                Drawn = false;
                if (!Hidden)
                {
                    BackRectangle.Right = WinWidth;
                    BackRectangle.Bottom = WinHeight;
                    BackRectangle.Top = WinTop;
                    BackRectangle.Left = WinLeft;
                }

                if (_onShow != null)
                {
                    _onShow(this);
                }
            }
            else
            {
                B = OwnerMenuBar;
                if (B != null)
                {
                    B.Selected = -1;
                    B.Drawn = false;
                }

                Hidden = true;
                if (_onHide != null)
                {
                    _onHide(this);
                }
            }
        }

        if (Visible)
        {
            if (!Valid || !Aligned || !Sized)
            {
                Setup();
            }

            ClearBlitList();
            ClearDrawList();
            Drawn = false;
            SetPriority(Priority);
        }
    }

    // StartMenu.pas:87
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip = default;
        try
        {
            if (Valid && Visible)
            {
                if (!Drawn)
                {
                    oldClip = lockedSurface.ClipRectangle;
                    _font.Color = Color;
                    _imageLibraryWholeMenu[Highlighted + 1].ShowClipped(0, 0, lockedSurface);
                    for (int I = 0, last = Items.Count - 1; I <= last; I++)
                    {
                        if (Items.Count > 0)
                        {
                            var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                            if (WinLeft + WinWidth - imageLibraryFrameWidth.Width < oldClip.Right)
                            {
                                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                                lockedSurface.ClipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame.Width;
                            }

                            if (I == Highlighted)
                            {
                                _font.Color = SelectedColor;
                            }

                            var item = Items[I];
                            var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
                            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            _font.SetText(WinLeft + _borderWidth + imageLibraryFrame2.Width, WinTop + _borderHeight + imageLibraryFrameHeight.Height + ItemHeight * I, item, lockedSurface);
                            if (I == Highlighted)
                            {
                                _font.Color = Color;
                            }

                            DrawItem = I;
                            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            DrawItemRectangle = new Rect(WinLeft + imageLibraryFrame5.Width, WinTop + imageLibraryFrame4.Height + ItemHeight * I, WinLeft + WinWidth, WinTop + imageLibraryFrame3.Height + (I + 1) * ItemHeight);
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
            SetWinHeightWithoutInvalidate(WinHeight - (imageLibraryFrameHeight.Height));
            base.CheckMouseDown(button, shift, X, Y);
            var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight + (imageLibraryFrame.Height));
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
