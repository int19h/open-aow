#nullable disable
// Translated from TurnButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Grids;

// { ot=1 }
// library, current player, last player, animating, animation phase, frame offset, done event,
// change event, checked, animation frame
public partial class TurnButton(Component owner) : Control(owner)
{

    private ImageLibrary _imageLibrary;
    private int _animationPhase;
    private int _frameOffset = 11;
    [Published]
    public event NotifyEvent<TurnButton> OnAnimationDone;
    [Published]
    public new event NotifyEvent<TurnButton> OnChange;

    public bool ButtonChecked { get; set; }
    public int AnimationFrame { get; set; }

    // TurnButton.pas:60
    public override void Validate()
    {
        if (_imageLibrary != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // TurnButton.pas:66
    public override void SetSize()
    {
        SetWinWidthWithoutInvalidate(_imageLibrary[0].Width);
        SetWinHeightWithoutInvalidate(_imageLibrary[0].Height);
        base.SetSize();
    }

    // TurnButton.pas:79
    public override void Update()
    {
        base.Update();
        if (Visible)
        {
            switch (_animationPhase)
            {
                case 0:
                    if (LastPlayer != CurrentPlayer)
                    {
                        AnimationFrame = LastPlayer * 7;
                        _frameOffset = 11;
                        _animationPhase = 1;
                        Animating = true;
                        Drawn = false;
                    }

                    break;
                case 1:
                    AnimationFrame += 1;
                    Drawn = false;
                    if ((LastPlayer * 7) + 5 < AnimationFrame)
                    {
                        AnimationFrame = 0;
                        _animationPhase = 2;
                        _frameOffset = 0;
                    }

                    break;
                case 2:
                    AnimationFrame += 1;
                    Drawn = false;
                    if (AnimationFrame > 10)
                    {
                        _animationPhase = 3;
                        AnimationFrame = (CurrentPlayer * 7) + 5;
                        _frameOffset = 11;
                    }

                    break;
                case 3:
                    AnimationFrame -= 1;
                    Drawn = false;
                    if (CurrentPlayer * 7 > AnimationFrame)
                    {
                        AnimationFrame = CurrentPlayer * 7;
                        _animationPhase = 0;
                        _frameOffset = 11;
                        LastPlayer = CurrentPlayer;
                        Animating = false;
                        OnAnimationDone?.Invoke(this);
                    }

                    break;
                default:
                    break;
            }
        }
    }

    // TurnButton.pas:132
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (Visible && Valid)
            {
                RaisePreDraw(this, lockedSurface);

                if (!Animating && ButtonChecked)
                {
                    _imageLibrary[_frameOffset + (CurrentPlayer * 7) + 6].ShowClipped(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, lockedSurface);
                }
                else
                {
                    _imageLibrary[_frameOffset + AnimationFrame].ShowClipped(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, lockedSurface);
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // TurnButton.pas:151
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            if (button == MouseButton.Left && Press())
            {
                RaiseMouseDown(this, button, shift, X, Y);
            }
        }
    }

    // TurnButton.pas:169
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // TurnButton.pas:180
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= Window.AbsoluteLeft + WinLeft && X <= Window.AbsoluteLeft + WinLeft + WinWidth - 1 && Y >= Window.AbsoluteTop + WinTop && Y <= Window.AbsoluteTop + WinTop + WinHeight - 1)
        {
            Handled = true;
            RaiseMouseMove(this, shift, X, Y);
        }
    }

    // TurnButton.pas:73
    public virtual void SetImageLibrary(ImageLibrary value) => _imageLibrary = value ?? null;

    [Published]
    public ImageLibrary ImageLibrary { get => _imageLibrary; set => SetImageLibrary(value); }

    [Published]
    public int CurrentPlayer { get; set; }

    [Published]
    public int LastPlayer { get; set; }

    [Published]
    public bool Animating { get; set; }

    [Published]
    public override bool Checked { get => ButtonChecked; set => ButtonChecked = value; }

    public new event MouseEvent<TurnButton> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<TurnButton> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<TurnButton> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<TurnButton> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<TurnButton> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<TurnButton> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<TurnButton> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<TurnButton> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(OnUpdate), value: value);
    }
}
