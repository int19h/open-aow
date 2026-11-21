#nullable disable warnings
// Translated from StartMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// whole-menu image library
public sealed class StartMenu(Component? owner) : Menu(owner)
{

    // StartMenu.pas:30
    public override void SetSize()
    {
        base.SetSize();
        SetWinHeightWithoutInvalidate(377);
        WindowManager? m = WindowManager;
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
                WindowManager? m = WindowManager;
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
                MenuBar? b = OwnerMenuBar;
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
                            LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                            if (WinLeft + WinWidth - imageLibraryFrameWidth.Width < oldClip.Right)
                            {
                                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                                Rect clipRectangle = lockedSurface.ClipRectangle;
                                clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame.Width;
                                lockedSurface.ClipRectangle = clipRectangle;
                            }

                            if (i == Highlighted)
                            {
                                Font.Color = SelectedColor;
                            }

                            string item = Items[i];
                            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Font.SetText(WinLeft + BorderWidth + imageLibraryFrame2.Width, WinTop + BorderHeight + imageLibraryFrameHeight.Height + (ItemHeight * i), text: item, lockedSurface);
                            if (i == Highlighted)
                            {
                                Font.Color = Color;
                            }

                            int drawItem = i;
                            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Rect drawItemRectangle = new(WinLeft + imageLibraryFrame5.Width, WinTop + imageLibraryFrame4.Height + (ItemHeight * i), WinLeft + WinWidth, WinTop + imageLibraryFrame3.Height + ((i + 1) * ItemHeight));
                            RaiseDrawItem(this, drawRectangle: drawItemRectangle, drawItem: drawItem, lockedSurface: ref lockedSurface);

                            lockedSurface.ClipRectangle = oldClip;
                        }
                    }
                }

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // StartMenu.pas:123
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        if (Valid)
        {
            LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight - imageLibraryFrameHeight.Height);
            base.CheckMouseDown(button, shift, X, Y);
            LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            SetWinHeightWithoutInvalidate(WinHeight + imageLibraryFrame.Height);
        }
    }

    [Published]
    public ImageLibrary ImageLibraryWholeMenu { get; set; }

    public new event NotifyEvent<StartMenu> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<StartMenu> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<StartMenu> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<StartMenu> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<StartMenu> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<StartMenu> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<StartMenu> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<StartMenu> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<StartMenu> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<StartMenu> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<StartMenu> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> SurfaceDrawn
    {
        add => ((WindowControl)this).SurfaceDrawn += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(SurfaceDrawn), value: value);
        remove => ((WindowControl)this).SurfaceDrawn -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(SurfaceDrawn), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> PreDrawSurface
    {
        add => ((WindowControl)this).PreDrawSurface += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(PreDrawSurface), value: value);
        remove => ((WindowControl)this).PreDrawSurface -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(PreDrawSurface), value: value);
    }

    public new event NotifyEvent<StartMenu> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<StartMenu> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(Show), value: value);
    }

    public new event NotifyEvent<StartMenu> Select
    {
        add => ((Menu)this).Select += EventSlot.Add<Menu, StartMenu>(this, slot: nameof(Select), value: value);
        remove => ((Menu)this).Select -= EventSlot.Remove<Menu, StartMenu>(this, slot: nameof(Select), value: value);
    }
}
