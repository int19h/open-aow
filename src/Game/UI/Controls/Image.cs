// Translated from AoWImage.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWImage unit of aowInt.dpl: TAOWImage, a control that shows one image of an image
//   library as a texture filling its rectangle, or plays the images AnimStart..AnimEnd as an
//   animation, one image every AnimationFrameDelay updates. Its mouse events get coordinates relative
//   to the control. }
// Fields: FAnimate, FAnimFrameDelay, FFrameCount (updates left until the next image),
// _animationStart, _currentImage, _animationEnd, _imageLibrary
public partial class Image : Control
{
    public Image(Component owner) : base(owner)
    { }

    private bool _animate;
    private int _animationFrameDelay;
    private int _frameCount;
    private int _animationStart;
    private int _currentImage;
    private int _animationEnd;

    // AoWImage.pas:79
    public override void SetSize()
    {
        if (WinWidth == 0)
        {
            SetWinWidthWithoutInvalidate(_imageLibrary[_currentImage].Width);
        }

        if (WinHeight == 0)
        {
            SetWinHeightWithoutInvalidate(_imageLibrary[_currentImage].Height);
        }

        base.SetSize();
    }

    // AoWImage.pas:145
    public override void Update()
    {
        int I;
        if (Visible && _animate == true)
        {
            if (_frameCount > 0)
            {
                _frameCount -= 1;
            }
            else
            {
                _frameCount = _animationFrameDelay;
                I = _currentImage + 1;
                if (I > _animationEnd)
                {
                    I = _animationStart;
                }

                SetCurrent(I);
            }
        }

        base.Update();
    }

    // AoWImage.pas:163
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            if (_onMouseDown != null)
            {
                _onMouseDown(this, button, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
            }
        }
    }

    // AoWImage.pas:177
    public override void CheckMouseUp(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        Handled = false;
        Redraw = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            if (_onMouseUp != null)
            {
                _onMouseUp(this, button, shift, X, Y);
            }
        }
    }

    // AoWImage.pas:190
    public override void CheckMouseMove(Set<ShiftState> shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.WinLeft + WinLeft - 1 && X < Window.WinLeft + WinLeft + WinWidth && Y > Window.WinTop + WinTop - 1 && Y < Window.WinTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            if (_onMouseMove != null)
            {
                _onMouseMove(this, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
            }
        }
    }

    // AoWImage.pas:203
    public override void CheckKeyPress(ref byte key)
    {
        Handled = false;
    }

    // AoWImage.pas:88
    public void SetAnimation(bool value)
    {
        if (value != _animate)
        {
            _animate = value;
            Valid = false;
        }
    }

    // AoWImage.pas:97
    public void SetStart(int value)
    {
        if (value < 0)
        {
            value = 0;
        }

        if (value != _animationStart)
        {
            _animationStart = value;
            Valid = false;
        }
    }

    // AoWImage.pas:108
    public void SetCurrent(int value)
    {
        if (value < _animationStart)
        {
            value = _animationStart;
        }

        if (value != _currentImage)
        {
            _currentImage = value;
            Drawn = false;
        }
    }

    // AoWImage.pas:119
    public void SetEnd(int value)
    {
        if (value != _animationEnd)
        {
            _animationEnd = value;
            Valid = false;
        }
    }

    [Published]
    public int AnimationStart { get => _animationStart; set => SetStart(value); }

    [Published]
    public int CurrentImage { get => _currentImage; set => SetCurrent(value); }

    [Published]
    public int AnimationEnd { get => _animationEnd; set => SetEnd(value); }

    [Published]
    public bool Animate { get => _animate; set => SetAnimation(value); }

    [Published]
    public int AnimationFrameDelay { get => _animationFrameDelay; set => _animationFrameDelay = value; }

    public new NotifyEvent<Image> OnChange
    {
        get => EventSlot.Get(this, _onChange);
        set => EventSlot.Set(this, ref _onChange, value);
    }

    public new MouseEvent<Image> OnMouseDown
    {
        get => EventSlot.Get(this, _onMouseDown);
        set => EventSlot.Set(this, ref _onMouseDown, value);
    }

    public new MouseEvent<Image> OnMouseUp
    {
        get => EventSlot.Get(this, _onMouseUp);
        set => EventSlot.Set(this, ref _onMouseUp, value);
    }

    public new MouseMoveEvent<Image> OnMouseMove
    {
        get => EventSlot.Get(this, _onMouseMove);
        set => EventSlot.Set(this, ref _onMouseMove, value);
    }

    public new KeyPressEvent<Image> OnKeyPress
    {
        get => EventSlot.Get(this, _onKeyPress);
        set => EventSlot.Set(this, ref _onKeyPress, value);
    }

    public new KeyEvent<Image> OnKeyDown
    {
        get => EventSlot.Get(this, _onKeyDown);
        set => EventSlot.Set(this, ref _onKeyDown, value);
    }

    public new AoWOnDrawEvent<Image> OnPreDraw
    {
        get => EventSlot.Get(this, _onPreDraw);
        set => EventSlot.Set(this, ref _onPreDraw, value);
    }

    public new AoWOnDrawEvent<Image> OnDraw
    {
        get => EventSlot.Get(this, _onDraw);
        set => EventSlot.Set(this, ref _onDraw, value);
    }

    public new NotifyEvent<Image> OnUpdate
    {
        get => EventSlot.Get(this, _onUpdate);
        set => EventSlot.Set(this, ref _onUpdate, value);
    }
}
