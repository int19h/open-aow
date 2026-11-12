// Translated from AOWToolTip.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Windows;

// { The ToolTip unit of aowInt.dpl: TAOWToolTip, the small window that TAOWControl shows with a
//   control's tool tip text after the mouse rested on it for ToolTipDelay updates. }
// Fields: _items, OnDrawItem, _toolTipStatic, _toolTipDelay, _showDuration, _itemHeight,
// FShowing, FControlRect (the control on the screen. the tip shows at its bottom right),
// FItemIndex, FItemRect (the line being drawn, for OnDrawItem), FLines (Items broken into lines
// that fit)
public partial class ToolTip : GenericWindow
{
    private StringList _items;
    [Published]
    public event NotifyEvent<ToolTip> OnDrawItem;
    private bool _toolTipStatic;
    private int _toolTipDelay;
    private int _showDuration;
    private int _itemHeight;

    public Rect ControlRectangle { get; set; }

    private StringList _lines;

    // AOWToolTip.pas:62
    public ToolTip(Component owner) : base(owner)
    {
        _lines = new StringList();
        _items = new StringList();
        _items.OnChange = OnChangeText;
        _showDuration = 100;
        _itemHeight = 10;
    }

    // AOWToolTip.pas:72
    public override void Dispose()
    {
        _lines.Dispose();
        _items.Dispose();
        base.Dispose();
    }

    // AOWToolTip.pas:79
    public override void Validate()
    {
        if (_imageLibraryFrame != null)
        {
            _imageLibraryFrame = _imageLibraryFrame.CheckImageLibrary(_imageLibraryIndexFrame, 8, this);
        }

        if (_imageLibraryTexture != null)
        {
            _imageLibraryTexture = _imageLibraryTexture.CheckImageLibrary(_imageLibraryIndexTexture, 1, this);
        }

        if (OwnerComponent != null && _imageLibraryFrame != null && _imageLibraryTexture != null && _font != null)
        {
            Valid = true;
        }

        base.Validate();
    }

    // AOWToolTip.pas:95
    // { SetSize breaks the lines of Items into FLines so that each fits the width (moving the last
    //   characters of a line to the next one up to a space), sizes the tool tip to the text and keeps
    //   it on the screen. The width limit of the line breaking uses the images 3 and 4 of the frame
    //   library instead of FILIndexFrame + 3 and + 4, and the minimum width adds the width of the
    //   top left corner twice (original bugs). }
    public override void SetSize()
    {
        int I;
        int W;
        bool moved;
        bool done;
        if (_ownerItem != 0)
        {
            SetWinWidthWithoutInvalidate(_ownerItem);
        }
        else
        {
            _ownerItem = WinWidth;
        }

        var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame];
        var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
        if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
        {
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
        }

        var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame];
        var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
        {
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame];
            SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
        }

        _lines.Clear();
        I = 0;
        int last = _items.Count - 1;
        for (; I <= last; I += 1)
        {
            var item = _items[I];
            _lines.Add(item);
        }

        if (_lines.Count > 0)
        {
            I = 0;
            while (I < _lines.Count)
            {
                moved = false;
                done = true;
                while (true)
                {
                    bool notDone = !done;
                    if (!notDone)
                    {
                        var line = _lines[I];
                        var textWidth = _font.GetTextWidth(line);
                        var imageLibraryFrame2 = _imageLibraryFrame[3];
                        var imageLibraryFrame3 = _imageLibraryFrame[4];
                        notDone = textWidth > WinWidth - imageLibraryFrame2.Width - imageLibraryFrame3.Width - _borderWidth * 2;
                    }

                    if (!(notDone && _lines[I].Length > 1))
                        break;
                    done = false;
                    if (I < _lines.Count - 1)
                    {
                        if (moved == true)
                        {
                            var lineLength = _lines[I];
                            var line = _lines[I];
                            var line2 = _lines[I + 1];
                            _lines[I + 1] = Copy(line, lineLength.Length, 1) + line2;
                        }
                        else
                        {
                            var lineLength = _lines[I];
                            var line = _lines[I];
                            _lines.Insert(I + 1, Copy(line, lineLength.Length, 1));
                        }
                    }
                    else
                    {
                        var lineLength = _lines[I];
                        var line = _lines[I];
                        _lines.Add(Copy(line, lineLength.Length, 1));
                    }

                    var line3 = _lines[I];
                    var line4 = _lines[I];
                    _lines[I] = Copy(line4, 1, line3.Length - 1);
                    moved = true;
                    var line5 = _lines[I];
                    if (Copy(_lines[I], line5.Length, 1) == " ")
                    {
                        done = true;
                    }
                }

                I += 1;
            }

            var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            var count = _lines.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame5.Height + imageLibraryFrame6.Height + _borderHeight * 2 + count * _itemHeight);
            var line6 = _lines[0];
            W = _font.GetTextWidth(line6);
            var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame];
            var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
            if (W < WinWidth - imageLibraryFrame7.Width - imageLibraryFrame8.Width - _borderWidth * 2)
            {
                var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame];
                var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 2];
                SetWinWidthWithoutInvalidate(W + imageLibraryFrame2.Width + imageLibraryFrame3.Width + _borderWidth * 2);
            }
        }

        if (Masked)
        {
            FrameLeft = _imageLibraryFrame[_imageLibraryIndexFrame + 3].Width;
            var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame2.Width;
            FrameTop = _imageLibraryFrame[_imageLibraryIndexFrame + 1].Height;
            var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
            FrameBottom = WinHeight - imageLibraryFrame3.Height;
        }

        SetWinLeftWithoutInvalidate(ControlRectangle.Right);
        SetWinTopWithoutInvalidate(ControlRectangle.Bottom);
        if (_windowManager.WinWidth < WinLeft + WinWidth)
        {
            SetWinLeftWithoutInvalidate(_windowManager.WinWidth - WinWidth);
        }

        if (WinTop + WinHeight > _windowManager.WinHeight)
        {
            SetWinTopWithoutInvalidate(ControlRectangle.Top - WinHeight);
        }

        if (WinTop < 0)
        {
            SetWinTopWithoutInvalidate(0);
        }

        base.SetSize();
    }

    // AOWToolTip.pas:162
    public override void Update()
    {
        bool B;
        B = _doubleBuffered;
        _doubleBuffered = false;
        base.Update();
        _doubleBuffered = B;
    }

    // AOWToolTip.pas:186
    public override void SetVisible(bool value)
    {
        if (value != Visible)
        {
            SetVisibleWithoutUpdate(value);
            if (!Visible)
            {
                Hidden = true;
                RaiseHide(this);
            }
            else
            {
                if (_doubleBuffered)
                {
                    SurfaceValid = false;
                }
                else
                {
                    Drawn = false;
                }

                if (!Hidden)
                {
                    BackRectangle.Right = WinWidth;
                    BackRectangle.Bottom = WinHeight;
                    BackRectangle.Top = WinTop;
                    BackRectangle.Left = WinLeft;
                }

                RaiseShow(this);
            }

            if (Visible == true)
            {
                Hidden = false;
                SetPriority(Priority);
            }
        }

        BackRectangle.Right = 0;
        BackRectangle.Bottom = 0;
    }

    // AOWToolTip.pas:223
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip;
        try
        {
            if (Valid && Visible)
            {
                var rectangle = new Rect(WinLeft, WinTop, WinWidth, WinHeight);
                _imageLibraryFrame.DrawFrame(_imageLibraryIndexFrame, rectangle, ref lockedSurface);
                var imageLibraryFrameHeight = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame = _imageLibraryFrame[_imageLibraryIndexFrame + 6];
                var imageLibraryFrameWidth = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var imageLibraryFrame2 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                var imageLibraryFrame3 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                var imageLibraryFrame4 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                var rectangle2 = new Rect(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                _imageLibraryTexture.DrawTiled(_imageLibraryIndexTexture, rectangle2, ref lockedSurface);
                _font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                if (_lines.Count > 0)
                {
                    for (int I = 0, last = _lines.Count - 1; I <= last; I++)
                    {
                        var imageLibraryFrame5 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame + 4];
                            lockedSurface.ClipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                        }

                        if (I < _lines.Count && _lines.Count > 0)
                        {
                            var line = _lines[I];
                            var imageLibraryFrame6 = _imageLibraryFrame[_imageLibraryIndexFrame];
                            var imageLibraryFrame7 = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            _font.SetText(WinLeft + _borderWidth + imageLibraryFrame7.Width, WinTop + _borderHeight + imageLibraryFrame6.Height + _itemHeight * I, line, lockedSurface);
                            var imageLibraryFrame8 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var imageLibraryFrame9 = _imageLibraryFrame[_imageLibraryIndexFrame + 1];
                            var libraryImage = _imageLibraryFrame[_imageLibraryIndexFrame + 3];
                            if (OnDrawItem != null)
                            {
                                OnDrawItem(this);
                            }
                        }

                        lockedSurface.ClipRectangle = oldClip;
                    }
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWToolTip.pas:266
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y)
    {
        SetVisible(false);
    }

    // AOWToolTip.pas:172
    public void SetFStrings(StringList value)
    {
        _lines.Clear();
        _items.Clear();
        for (int I = 0, last = value.Count - 1; I <= last; I++)
        {
            var valueValue = value[I];
            _lines.Add(valueValue);
            var valueValue2 = value[I];
            _items.Add(valueValue2);
        }

        Sized = false;
    }

    [Published]
    public ImageLibraryFont Font { get => _font; set => _font = value; }

    [Published]
    public StringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public bool ToolTipStatic { get => _toolTipStatic; set => _toolTipStatic = value; }

    [Published]
    public int ToolTipDelay { get => _toolTipDelay; set => _toolTipDelay = value; }

    [Published]
    public int ShowDuration { get => _showDuration; set => _showDuration = value; }

    [Published]
    public int ItemHeight { get => _itemHeight; set => _itemHeight = value; }

    public new event NotifyEvent<ToolTip> OnChange
    {
        add => ((UIComponent)this).OnChange += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnChange), value: value);
        remove => ((UIComponent)this).OnChange -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnChange), value: value);
    }

    public new event MouseEvent<ToolTip> OnMouseDown
    {
        add => ((UIComponent)this).OnMouseDown += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnMouseDown), value: value);
        remove => ((UIComponent)this).OnMouseDown -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnMouseDown), value: value);
    }

    public new event MouseEvent<ToolTip> OnMouseUp
    {
        add => ((UIComponent)this).OnMouseUp += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnMouseUp), value: value);
        remove => ((UIComponent)this).OnMouseUp -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnMouseUp), value: value);
    }

    public new event MouseMoveEvent<ToolTip> OnMouseMove
    {
        add => ((UIComponent)this).OnMouseMove += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnMouseMove), value: value);
        remove => ((UIComponent)this).OnMouseMove -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnMouseMove), value: value);
    }

    public new event KeyPressEvent<ToolTip> OnKeyPress
    {
        add => ((UIComponent)this).OnKeyPress += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnKeyPress), value: value);
        remove => ((UIComponent)this).OnKeyPress -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnKeyPress), value: value);
    }

    public new event KeyEvent<ToolTip> OnKeyDown
    {
        add => ((UIComponent)this).OnKeyDown += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnKeyDown), value: value);
        remove => ((UIComponent)this).OnKeyDown -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnKeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ToolTip> OnPreDraw
    {
        add => ((UIComponent)this).OnPreDraw += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnPreDraw), value: value);
        remove => ((UIComponent)this).OnPreDraw -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnPreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ToolTip> OnDraw
    {
        add => ((UIComponent)this).OnDraw += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnDraw), value: value);
        remove => ((UIComponent)this).OnDraw -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnDraw), value: value);
    }

    public new event NotifyEvent<ToolTip> OnUpdate
    {
        add => ((UIComponent)this).OnUpdate += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(OnUpdate), value: value);
        remove => ((UIComponent)this).OnUpdate -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(OnUpdate), value: value);
    }

    public new event AoWOnDrawBackEvent<ToolTip> OnDrawBack
    {
        add => ((WindowControl)this).OnDrawBack += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnDrawBack), value: value);
        remove => ((WindowControl)this).OnDrawBack -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnDrawBack), value: value);
    }

    public new event NotifyEvent<ToolTip> OnEnter
    {
        add => ((WindowControl)this).OnEnter += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnEnter), value: value);
        remove => ((WindowControl)this).OnEnter -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnEnter), value: value);
    }

    public new event NotifyEvent<ToolTip> OnExit
    {
        add => ((WindowControl)this).OnExit += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnExit), value: value);
        remove => ((WindowControl)this).OnExit -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnExit), value: value);
    }

    public new event NotifyEvent<ToolTip> OnChangeCursor
    {
        add => ((WindowControl)this).OnChangeCursor += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnChangeCursor), value: value);
        remove => ((WindowControl)this).OnChangeCursor -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<ToolTip> OnDrawSurface
    {
        add => ((WindowControl)this).OnDrawSurface += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnDrawSurface), value: value);
        remove => ((WindowControl)this).OnDrawSurface -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnDrawSurface), value: value);
    }

    public new event AoWOnDrawSurface<ToolTip> OnPreDrawSurface
    {
        add => ((WindowControl)this).OnPreDrawSurface += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(OnPreDrawSurface), value: value);
        remove => ((WindowControl)this).OnPreDrawSurface -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(OnPreDrawSurface), value: value);
    }

    public new event NotifyEvent<ToolTip> OnHide
    {
        add => ((BaseWindow)this).OnHide += EventSlot.Add<BaseWindow, ToolTip>(this, slot: nameof(OnHide), value: value);
        remove => ((BaseWindow)this).OnHide -= EventSlot.Remove<BaseWindow, ToolTip>(this, slot: nameof(OnHide), value: value);
    }

    public new event NotifyEvent<ToolTip> OnShow
    {
        add => ((BaseWindow)this).OnShow += EventSlot.Add<BaseWindow, ToolTip>(this, slot: nameof(OnShow), value: value);
        remove => ((BaseWindow)this).OnShow -= EventSlot.Remove<BaseWindow, ToolTip>(this, slot: nameof(OnShow), value: value);
    }
}
