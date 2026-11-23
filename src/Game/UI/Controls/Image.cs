#nullable disable warnings
// Translated from AoWImage.pas.
using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// { The AoWImage unit of aowInt.dpl: TAOWImage, a control that shows one image of an image
//   library as a texture filling its rectangle, or plays the images AnimStart..AnimEnd as an
//   animation, one image every AnimationFrameDelay updates. Its mouse events get coordinates relative
//   to the control. }
// Fields: FAnimate, FAnimFrameDelay, FFrameCount (updates left until the next image),
// _animationStart, _currentImage, _animationEnd, _imageLibrary
public sealed partial class Image(Component? owner) : Control(owner)
{
    private bool _animate;
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
        if (Visible && _animate)
        {
            if (_frameCount > 0)
            {
                _frameCount -= 1;
            }
            else
            {
                _frameCount = AnimationFrameDelay;
                int i = _currentImage + 1;
                if (i > _animationEnd)
                {
                    i = _animationStart;
                }

                SetCurrent(i);
            }
        }

        base.Update();
    }

    // AoWImage.pas:163
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y)
    {
        base.CheckMouseDown(button, shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseDown(button, shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // AoWImage.pas:177
    public override void CheckMouseUp(MouseButton button, ShiftStates shift, int X, int Y)
    {
        Handled = false;
        Redraw = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseUp(button, shift, X, Y);
        }
    }

    // AoWImage.pas:190
    public override void CheckMouseMove(ShiftStates shift, int X, int Y)
    {
        base.CheckMouseMove(shift, X, Y);
        Handled = false;
        if (X > Window.AbsoluteLeft + WinLeft - 1 && X < Window.AbsoluteLeft + WinLeft + WinWidth && Y > Window.AbsoluteTop + WinTop - 1 && Y < Window.AbsoluteTop + WinTop + WinHeight)
        {
            Handled = CaptureInput;
            RaiseMouseMove(shift, X - Window.AbsoluteLeft - WinLeft, Y - Window.AbsoluteTop - WinTop);
        }
    }

    // AoWImage.pas:203
    public override void CheckKeyPress(ref byte key) => Handled = false;

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
            IsDrawn = false;
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
    public int AnimationFrameDelay { get; set; }
}
