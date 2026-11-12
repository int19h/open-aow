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

                RaiseShow(this);
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
                RaiseHide(this);
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
                            RaiseDrawItem(this);

                            lockedSurface.ClipRectangle = oldClip;
                        }
                    }
                }

                RaiseDraw(this, lockedSurface);
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

    public new event NotifyEvent<StartMenu> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<StartMenu> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<StartMenu> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<StartMenu> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<StartMenu> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<StartMenu> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<StartMenu> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<StartMenu> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<StartMenu> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<StartMenu> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<StartMenu> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<StartMenu> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<StartMenu> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(OnShow), value: value);
    }

    public new event NotifyEvent<StartMenu> OnDrawItem
    {
        add => ((Menu)this).OnDrawItem += EventSlot.Add<Menu, StartMenu>(this, slot: nameof(OnDrawItem), value: value);
        remove => ((Menu)this).OnDrawItem -= EventSlot.Remove<Menu, StartMenu>(this, slot: nameof(OnDrawItem), value: value);
    }

    public new event NotifyEvent<StartMenu> OnSelect
    {
        add => ((Menu)this).OnSelect += EventSlot.Add<Menu, StartMenu>(this, slot: nameof(OnSelect), value: value);
        remove => ((Menu)this).OnSelect -= EventSlot.Remove<Menu, StartMenu>(this, slot: nameof(OnSelect), value: value);
    }
}
