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
            RaiseMouseDown(this, button, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
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
            RaiseMouseUp(this, button, shift, X, Y);
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
            RaiseMouseMove(this, shift, X - Window.WinLeft - WinLeft, Y - Window.WinTop - WinTop);
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

    public new event NotifyEvent<Image> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<Image> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<Image> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<Image> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<Image> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<Image> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<Image> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<Image> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<Image> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, Image>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, Image>(this, slot: nameof(OnUpdate), value: value);
    }
}
