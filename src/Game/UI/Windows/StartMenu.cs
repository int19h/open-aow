#nullable disable
// Translated from StartMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// whole-menu image library
public partial class StartMenu(Component owner) : Menu(owner)
{

    // StartMenu.pas:30
    public override void SetSize()
    {
        WindowManager m;
        base.SetSize();
        SetWinHeightWithoutInvalidate(377);
        m = WindowManager;
        if (m is not null && OwnerMenuBar is not null)
        {
            SetWinTopWithoutInvalidate(m.WinHeight - 393);
        }

        SetWinLeftWithoutInvalidate(0);
    }

    // StartMenu.pas:41
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (Visible)
            {
                Highlighted = -1;
                SetWinHeightWithoutInvalidate(377);
                WindowManager m = WindowManager;
                if (m is not null && OwnerMenuBar is not null)
                {
                    SetWinTopWithoutInvalidate(m.WinHeight - 393);
                }

                SetWinLeftWithoutInvalidate(0);
                Drawn = false;
                if (!Hidden)
                {
                    BackRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                }

                RaiseShow(this);
            }
            else
            {
                MenuBar b = OwnerMenuBar;
                if (b is not null)
                {
                    b.Selected = -1;
                    b.Drawn = false;
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
        try
        {
            if (Valid && Visible)
            {
                if (!Drawn)
                {
                    Rect oldClip = lockedSurface.ClipRectangle;
                    Font.Color = Color;
                    ImageLibraryWholeMenu[Highlighted + 1].ShowClipped(0, 0, lockedSurface);
                    for (int i = 0, last = Items.Count - 1; i <= last; i++)
                    {
                        if (Items.Count > 0)
                        {
                            LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                            if (WinLeft + WinWidth - imageLibraryFrameWidth.Width < oldClip.Right)
                            {
                                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                                Rect clipRectangle = lockedSurface.ClipRectangle;
                                clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame.Width;
                                lockedSurface.ClipRectangle = clipRectangle;
                            }

                            if (i == Highlighted)
                            {
                                Font.Color = SelectedColor;
                            }

                            string item = Items[i];
                            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Font.SetText(WinLeft + BorderWidth + imageLibraryFrame2.Width, WinTop + BorderHeight + imageLibraryFrameHeight.Height + (ItemHeight * i), item, lockedSurface);
                            if (i == Highlighted)
                            {
                                Font.Color = Color;
                            }

                            DrawItem = i;
                            LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            DrawItemRectangle = new Rect(WinLeft + imageLibraryFrame5.Width, WinTop + imageLibraryFrame4.Height + (ItemHeight * i), WinLeft + WinWidth, WinTop + imageLibraryFrame3.Height + ((i + 1) * ItemHeight));
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
            _ = MessageBox(0, "Exception during Draw", $"Error - {Name}", 0);
        }
    }

    // StartMenu.pas:123
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        if (Valid)
        {
            LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight - imageLibraryFrameHeight.Height);
            base.CheckMouseDown(button, shift, X, Y);
            LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight + imageLibraryFrame.Height);
        }
    }

    [Published]
    public ImageLibrary ImageLibraryWholeMenu { get; set; }

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
