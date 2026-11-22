#nullable disable warnings
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
public sealed partial class TurnButton(Component? owner) : Control(owner)
{

    private ImageLibrary? _imageLibrary;
    private int _animationPhase;
    private int _frameOffset = 11;
    [Published]
    public event NotifyEvent<TurnButton>? AnimationEnded;
    [Published]
    public new event NotifyEvent<TurnButton>? Changed;

    public bool ButtonChecked { get; set; }
    public int AnimationFrame { get; set; }

    // TurnButton.pas:60
    public override void Validate()
    {
        if (_imageLibrary is not null)
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
                        IsDrawn = false;
                    }

                    break;
                case 1:
                    AnimationFrame += 1;
                    IsDrawn = false;
                    if ((LastPlayer * 7) + 5 < AnimationFrame)
                    {
                        AnimationFrame = 0;
                        _animationPhase = 2;
                        _frameOffset = 0;
                    }

                    break;
                case 2:
                    AnimationFrame += 1;
                    IsDrawn = false;
                    if (AnimationFrame > 10)
                    {
                        _animationPhase = 3;
                        AnimationFrame = (CurrentPlayer * 7) + 5;
                        _frameOffset = 11;
                    }

                    break;
                case 3:
                    AnimationFrame -= 1;
                    IsDrawn = false;
                    if (CurrentPlayer * 7 > AnimationFrame)
                    {
                        AnimationFrame = CurrentPlayer * 7;
                        _animationPhase = 0;
                        _frameOffset = 11;
                        LastPlayer = CurrentPlayer;
                        Animating = false;
                        AnimationEnded?.Invoke(this);
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
                RaisePreDraw(this, surfDesc: lockedSurface);

                if (!Animating && ButtonChecked)
                {
                    _imageLibrary[_frameOffset + (CurrentPlayer * 7) + 6].ShowClipped(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, lockedSurface);
                }
                else
                {
                    _imageLibrary[_frameOffset + AnimationFrame].ShowClipped(Window.AbsoluteLeft + WinLeft, Window.AbsoluteTop + WinTop, lockedSurface);
                }

                RaiseDraw(this, surfDesc: lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, text: "Exception during Draw", caption: $"Error - {Name}", flags: 0);
        }
    }

    // TurnButton.pas:151
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
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
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = true;
            RaiseMouseUp(this, button, shift, X, Y);
        }
    }

    // TurnButton.pas:180
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
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
    public void SetImageLibrary(ImageLibrary? value) => _imageLibrary = value ?? null;

    [Published]
    public ImageLibrary? ImageLibrary { get => _imageLibrary; set => SetImageLibrary(value); }

    [Published]
    public int CurrentPlayer { get; set; }

    [Published]
    public int LastPlayer { get; set; }

    [Published]
    public bool Animating { get; set; }

    [Published]
    public override bool Checked { get => ButtonChecked; set => ButtonChecked = value; }

    public new event MouseEvent<TurnButton> MousePressed
    {
        add => ((UIComponent)this).MousePressed += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(MousePressed), value: value);
        remove => ((UIComponent)this).MousePressed -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(MousePressed), value: value);
    }

    public new event MouseEvent<TurnButton> MouseReleased
    {
        add => ((UIComponent)this).MouseReleased += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(MouseReleased), value: value);
        remove => ((UIComponent)this).MouseReleased -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(MouseReleased), value: value);
    }

    public new event MouseMoveEvent<TurnButton> MouseMoved
    {
        add => ((UIComponent)this).MouseMoved += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(MouseMoved), value: value);
        remove => ((UIComponent)this).MouseMoved -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(MouseMoved), value: value);
    }

    public new event KeyPressEvent<TurnButton> CharacterTyped
    {
        add => ((UIComponent)this).CharacterTyped += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(CharacterTyped), value: value);
        remove => ((UIComponent)this).CharacterTyped -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(CharacterTyped), value: value);
    }

    public new event KeyEvent<TurnButton> KeyPressed
    {
        add => ((UIComponent)this).KeyPressed += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(KeyPressed), value: value);
        remove => ((UIComponent)this).KeyPressed -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(KeyPressed), value: value);
    }

    public new event AoWOnDrawEvent<TurnButton> Drawing
    {
        add => ((UIComponent)this).Drawing += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(Drawing), value: value);
        remove => ((UIComponent)this).Drawing -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(Drawing), value: value);
    }

    public new event AoWOnDrawEvent<TurnButton> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<TurnButton> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, TurnButton>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, TurnButton>(this, slot: nameof(Updated), value: value);
    }
}
