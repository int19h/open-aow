#nullable disable warnings
// Translated from AowUserWindow.pas.
using Aow.Game.Draw;
using Aow.Game.UI.Controls;
using Aow.Rtl;

namespace Aow.Game.UI.Windows;

// extended drawing event
public sealed class UserWindow(Component owner) : Window(owner)
{
    [Published]
    public event DrawSurfaceExEvent? SurfaceDrawing;

    // AowUserWindow.pas:29
    public override void DrawSurface(Surface? surface)
    {
        bool handled = false;
        SurfaceDrawing?.Invoke(surface, out handled);

        if (!handled)
        {
            base.DrawSurface(surface);
        }
    }

    public new event NotifyEvent<UserWindow> Changed
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Changed), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Changed), value: value);
    }

    public new event MouseEvent<UserWindow> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<UserWindow> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<UserWindow> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<UserWindow> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<UserWindow> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<UserWindow> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<UserWindow> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<UserWindow> BackgroundDrawn
    {
        add => ((WindowControl)this).BackgroundDrawn += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(BackgroundDrawn), value: value);
        remove => ((WindowControl)this).BackgroundDrawn -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(BackgroundDrawn), value: value);
    }

    public new event NotifyEvent<UserWindow> MouseEntered
    {
        add => ((WindowControl)this).MouseEntered += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(MouseEntered), value: value);
        remove => ((WindowControl)this).MouseEntered -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(MouseEntered), value: value);
    }

    public new event NotifyEvent<UserWindow> MouseLeft
    {
        add => ((WindowControl)this).MouseLeft += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(MouseLeft), value: value);
        remove => ((WindowControl)this).MouseLeft -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(MouseLeft), value: value);
    }

    public new event NotifyEvent<UserWindow> CursorChanged
    {
        add => ((WindowControl)this).CursorChanged += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(CursorChanged), value: value);
        remove => ((WindowControl)this).CursorChanged -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(CursorChanged), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Blitted), value: value);
    }

    public new event AoWOnDrawSurface<UserWindow> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<UserWindow> Dragged
    {
        add => ((UIComponent)this).Changed += EventSlot.Add<UIComponent, UserWindow>(this, slot: nameof(Dragged), value: value);
        remove => ((UIComponent)this).Changed -= EventSlot.Remove<UIComponent, UserWindow>(this, slot: nameof(Dragged), value: value);
    }

    public new event NotifyEvent<UserWindow> Resized
    {
        add => ((WindowControl)this).Resized += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(Resized), value: value);
        remove => ((WindowControl)this).Resized -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(Resized), value: value);
    }

    public new event NotifyEvent<UserWindow> SizeChanged
    {
        add => ((WindowControl)this).SizeChanged += EventSlot.Add<WindowControl, UserWindow>(this, slot: nameof(SizeChanged), value: value);
        remove => ((WindowControl)this).SizeChanged -= EventSlot.Remove<WindowControl, UserWindow>(this, slot: nameof(SizeChanged), value: value);
    }

    public new event NotifyEvent<UserWindow> Hidden
    {
        add => ((BaseWindow)this).Hidden += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(Hidden), value: value);
        remove => ((BaseWindow)this).Hidden -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(Hidden), value: value);
    }

    public new event NotifyEvent<UserWindow> Shown
    {
        add => ((BaseWindow)this).Shown += EventSlot.Add<BaseWindow, UserWindow>(this, slot: nameof(Shown), value: value);
        remove => ((BaseWindow)this).Shown -= EventSlot.Remove<BaseWindow, UserWindow>(this, slot: nameof(Shown), value: value);
    }

    public delegate void DrawSurfaceExEvent(Surface? Surface, out bool Handled);
}
