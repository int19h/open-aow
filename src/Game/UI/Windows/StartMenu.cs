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
                IsDrawn = false;
                if (!IsHidden)
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
                    b.IsDrawn = false;
                }

                IsHidden = true;
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
            IsDrawn = false;
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
                if (!IsDrawn)
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

    public new event NotifyEvent<StartMenu> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<StartMenu> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<StartMenu> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<StartMenu> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<StartMenu> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<StartMenu> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<StartMenu> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<StartMenu> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, StartMenu>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, StartMenu>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<StartMenu> BackgroundDrawn
    {
        add => ((WindowControl)this).BackgroundDrawn += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(BackgroundDrawn), value: value);
        remove => ((WindowControl)this).BackgroundDrawn -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(BackgroundDrawn), value: value);
    }

    public new event NotifyEvent<StartMenu> MouseEntered
    {
        add => ((WindowControl)this).MouseEntered += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(MouseEntered), value: value);
        remove => ((WindowControl)this).MouseEntered -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(MouseEntered), value: value);
    }

    public new event NotifyEvent<StartMenu> MouseLeft
    {
        add => ((WindowControl)this).MouseLeft += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(MouseLeft), value: value);
        remove => ((WindowControl)this).MouseLeft -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(MouseLeft), value: value);
    }

    public new event NotifyEvent<StartMenu> CursorChanged
    {
        add => ((WindowControl)this).CursorChanged += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(CursorChanged), value: value);
        remove => ((WindowControl)this).CursorChanged -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(CursorChanged), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(Blitted), value: value);
    }

    public new event AoWOnDrawSurface<StartMenu> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, StartMenu>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, StartMenu>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<StartMenu> Hidden
    {
        add => ((BaseWindow)this).Hidden += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(Hidden), value: value);
        remove => ((BaseWindow)this).Hidden -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(Hidden), value: value);
    }

    public new event NotifyEvent<StartMenu> Shown
    {
        add => ((BaseWindow)this).Shown += EventSlot.Add<BaseWindow, StartMenu>(this, slot: nameof(Shown), value: value);
        remove => ((BaseWindow)this).Shown -= EventSlot.Remove<BaseWindow, StartMenu>(this, slot: nameof(Shown), value: value);
    }

    public new event NotifyEvent<StartMenu> ItemSelected
    {
        add => ((Menu)this).ItemSelected += EventSlot.Add<Menu, StartMenu>(this, slot: nameof(ItemSelected), value: value);
        remove => ((Menu)this).ItemSelected -= EventSlot.Remove<Menu, StartMenu>(this, slot: nameof(ItemSelected), value: value);
    }
}
