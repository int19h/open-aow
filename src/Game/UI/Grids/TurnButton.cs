// Translated from TurnButton.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

// { ot=1 }
// library, current player, last player, animating, animation phase, frame offset, done event,
// change event, checked, animation frame
public partial class TurnButton : Controls.Control
{
    public ImageLibrary _imageLibrary;
    public int _currentPlayer;
    public int _lastPlayer;
    public bool _animating;
    public int AnimationPhase;
    public int FrameOffset;
    public NotifyEvent<TurnButton> _onAnimationDone;
    public NotifyEvent<TurnButton> OnPlayerChange;
    public bool ButtonChecked;
    public int AnimationFrame;

    // TurnButton.pas:54
    public TurnButton(Component owner) : base(owner)
    {
        FrameOffset = 11;
    }

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
        _winWidth = _imageLibrary[0].Width;
        _winHeight = _imageLibrary[0].Height;
        base.SetSize();
    }

    // TurnButton.pas:79
    public override void Update()
    {
        base.Update();
        if (_visible)
        {
            switch (AnimationPhase)
            {
                case 0:
                    if (_lastPlayer != _currentPlayer)
                    {
                        AnimationFrame = _lastPlayer * 7;
                        FrameOffset = 11;
                        AnimationPhase = 1;
                        _animating = true;
                        Drawn = false;
                    }

                    break;
                case 1:
                    AnimationFrame += 1;
                    Drawn = false;
                    if (_lastPlayer * 7 + 5 < AnimationFrame)
                    {
                        AnimationFrame = 0;
                        AnimationPhase = 2;
                        FrameOffset = 0;
                    }

                    break;
                case 2:
                    AnimationFrame += 1;
                    Drawn = false;
                    if (AnimationFrame > 10)
                    {
                        AnimationPhase = 3;
                        AnimationFrame = _currentPlayer * 7 + 5;
                        FrameOffset = 11;
                    }

                    break;
                case 3:
                    AnimationFrame -= 1;
                    Drawn = false;
                    if (_currentPlayer * 7 > AnimationFrame)
                    {
                        AnimationFrame = _currentPlayer * 7;
                        AnimationPhase = 0;
                        FrameOffset = 11;
                        _lastPlayer = _currentPlayer;
                        _animating = false;
                        if (_onAnimationDone != null)
                        {
                            _onAnimationDone(this);
                        }
                    }

                    break;
            }
        }
    }

    // TurnButton.pas:132
    public override void Draw(LockedSurface lockedSurface)
    {
        try
        {
            if (_visible == true && Valid == true)
            {
                if (_onPreDraw != null)
                {
                    _onPreDraw(this, lockedSurface);
                }

                if (!_animating && ButtonChecked == true)
                {
                    _imageLibrary[FrameOffset + _currentPlayer * 7 + 6].ShowClipped(_window._winLeft + _winLeft, _window._winTop + _winTop, lockedSurface);
                }
                else
                {
                    _imageLibrary[FrameOffset + AnimationFrame].ShowClipped(_window._winLeft + _winLeft, _window._winTop + _winTop, lockedSurface);
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

    // TurnButton.pas:151
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (button == MouseButton.Left && Press())
            {
                if (_onMouseDown != null)
                {
                    _onMouseDown(this, button, shift, X, Y);
                }
            }
        }
    }

    // TurnButton.pas:169
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        if (X > _window._winLeft + _winLeft - 1 && X < _window._winLeft + _winLeft + _winWidth && Y > _window._winTop + _winTop - 1 && Y < _window._winTop + _winTop + _winHeight)
        {
            Handled = true;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // TurnButton.pas:180
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X >= _window._winLeft + _winLeft && X <= _window._winLeft + _winLeft + _winWidth - 1 && Y >= _window._winTop + _winTop && Y <= _window._winTop + _winTop + _winHeight - 1)
        {
            Handled = true;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X, Y);
            }
        }
    }

    // TurnButton.pas:73
    public virtual void SetImageLibrary(ImageLibrary value)
    {
        if (value != null)
        {
            _imageLibrary = value;
        }
        else
        {
            _imageLibrary = null;
        }
    }

    [Published]
    public ImageLibrary ImageLibrary { get => _imageLibrary; set => SetImageLibrary(value); }

    [Published]
    public int CurrentPlayer { get => _currentPlayer; set => _currentPlayer = value; }

    [Published]
    public int LastPlayer { get => _lastPlayer; set => _lastPlayer = value; }

    [Published]
    public bool Animating { get => _animating; set => _animating = value; }

    [Published]
    public NotifyEvent<TurnButton> OnAnimationDone { get => _onAnimationDone; set => _onAnimationDone = value; }

    [Published]
    public new NotifyEvent<TurnButton> OnChange { get => OnPlayerChange; set => OnPlayerChange = value; }

    [Published]
    public bool Checked { get => ButtonChecked; set => ButtonChecked = value; }

    public new MouseEvent<TurnButton> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<TurnButton> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<TurnButton> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<TurnButton> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<TurnButton> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<TurnButton> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<TurnButton> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<TurnButton> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
