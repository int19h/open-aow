#nullable disable
// Translated from AOWToolTip.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Rtl;
using static Aow.Rtl.DelphiStrings;
using static Aow.Rtl.RtlHost;

namespace Aow.Game.UI.Windows;

// { The ToolTip unit of aowInt.dpl: TAOWToolTip, the small window that TAOWControl shows with a
//   control's tool tip text after the mouse rested on it for ToolTipDelay updates. }
// Fields: _items, OnDrawItem, _toolTipStatic, _toolTipDelay, _showDuration, _itemHeight,
// FShowing, FControlRect (the control on the screen. the tip shows at its bottom right),
// FItemIndex, FItemRect (the line being drawn, for OnDrawItem), FLines (Items broken into lines
// that fit)
public partial class ToolTip : GenericWindow
{
    private readonly DelphiStringList _items;
    [Published]
    public event NotifyEvent<ToolTip> OnDrawItem;

    public Rect ControlRectangle { get; set; }

    private readonly DelphiStringList _lines;

    // AOWToolTip.pas:62
    public ToolTip(Component owner) : base(owner)
    {
        _lines = new DelphiStringList();
        _items = new DelphiStringList
        {
            OnChange = OnChangeText
        };
        ShowDuration = 100;
        ItemHeight = 10;
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
        if (ImageLibraryFrame != null)
        {
            ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, 8, this);
        }

        if (ImageLibraryTexture != null)
        {
            ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, 1, this);
        }

        if (OwnerComponent != null && ImageLibraryFrame != null && ImageLibraryTexture != null && base.Font != null)
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
        int i;
        int w;
        bool moved;
        bool done;
        if (OwnerItem != 0)
        {
            SetWinWidthWithoutInvalidate(OwnerItem);
        }
        else
        {
            OwnerItem = WinWidth;
        }

        LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
        if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
        {
            LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
        }

        LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
        {
            LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame];
            SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
        }

        _lines.Clear();
        i = 0;
        int last = _items.Count - 1;
        for (; i <= last; i += 1)
        {
            string item = _items[i];
            _ = _lines.Add(item);
        }

        if (_lines.Count > 0)
        {
            i = 0;
            while (i < _lines.Count)
            {
                moved = false;
                done = true;
                while (true)
                {
                    bool notDone = !done;
                    if (!notDone)
                    {
                        string line = _lines[i];
                        int textWidth = base.Font.GetTextWidth(line);
                        LibraryImage imageLibraryFrame2 = ImageLibraryFrame[3];
                        LibraryImage imageLibraryFrame3 = ImageLibraryFrame[4];
                        notDone = textWidth > WinWidth - imageLibraryFrame2.Width - imageLibraryFrame3.Width - (BorderWidth * 2);
                    }

                    if (!(notDone && _lines[i].Length > 1))
                    {
                        break;
                    }

                    done = false;
                    if (i < _lines.Count - 1)
                    {
                        if (moved)
                        {
                            string lineLength = _lines[i];
                            string line = _lines[i];
                            string line2 = _lines[i + 1];
                            _lines[i + 1] = Copy(line, lineLength.Length, 1) + line2;
                        }
                        else
                        {
                            string lineLength = _lines[i];
                            string line = _lines[i];
                            _lines.Insert(i + 1, Copy(line, lineLength.Length, 1));
                        }
                    }
                    else
                    {
                        string lineLength = _lines[i];
                        string line = _lines[i];
                        _ = _lines.Add(Copy(line, lineLength.Length, 1));
                    }

                    string line3 = _lines[i];
                    string line4 = _lines[i];
                    _lines[i] = Copy(line4, 1, line3.Length - 1);
                    moved = true;
                    string line5 = _lines[i];
                    if (Copy(_lines[i], line5.Length, 1) == " ")
                    {
                        done = true;
                    }
                }

                i += 1;
            }

            LibraryImage imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
            LibraryImage imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            int count = _lines.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame5.Height + imageLibraryFrame6.Height + (BorderHeight * 2) + (count * ItemHeight));
            string line6 = _lines[0];
            w = base.Font.GetTextWidth(line6);
            LibraryImage imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
            if (w < WinWidth - imageLibraryFrame7.Width - imageLibraryFrame8.Width - (BorderWidth * 2))
            {
                LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                SetWinWidthWithoutInvalidate(w + imageLibraryFrame2.Width + imageLibraryFrame3.Width + (BorderWidth * 2));
            }
        }

        if (Masked)
        {
            FrameLeft = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Width;
            LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame2.Width;
            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
            LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            FrameBottom = WinHeight - imageLibraryFrame3.Height;
        }

        SetWinLeftWithoutInvalidate(ControlRectangle.Right);
        SetWinTopWithoutInvalidate(ControlRectangle.Bottom);
        if (WindowManager.WinWidth < WinLeft + WinWidth)
        {
            SetWinLeftWithoutInvalidate(WindowManager.WinWidth - WinWidth);
        }

        if (WinTop + WinHeight > WindowManager.WinHeight)
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
        bool b;
        b = DoubleBuffered;
        DoubleBuffered = false;
        base.Update();
        DoubleBuffered = b;
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
                if (DoubleBuffered)
                {
                    SurfaceValid = false;
                }
                else
                {
                    Drawn = false;
                }

                if (!Hidden)
                {
                    BackRectangle = new SizedRectangle(WinLeft, WinTop, WinWidth, WinHeight);
                }

                RaiseShow(this);
            }

            if (Visible)
            {
                Hidden = false;
                SetPriority(Priority);
            }
        }

        BackRectangle = BackRectangle with { Width = 0, Height = 0 };
    }

    // AOWToolTip.pas:223
    public override void Draw(LockedSurface lockedSurface)
    {
        Rect oldClip;
        try
        {
            if (Valid && Visible)
            {
                SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, rectangle, ref lockedSurface);
                LibraryImage imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                LibraryImage imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                LibraryImage imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                LibraryImage imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                SizedRectangle rectangle2 = new(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, rectangle2, ref lockedSurface);
                base.Font.Color = Color;
                oldClip = lockedSurface.ClipRectangle;
                if (_lines.Count > 0)
                {
                    for (int i = 0, last = _lines.Count - 1; i <= last; i++)
                    {
                        LibraryImage imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            LibraryImage imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        if (i < _lines.Count && _lines.Count > 0)
                        {
                            string line = _lines[i];
                            LibraryImage imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            base.Font.SetText(WinLeft + BorderWidth + imageLibraryFrame7.Width, WinTop + BorderHeight + imageLibraryFrame6.Height + (ItemHeight * i), line, lockedSurface);
                            LibraryImage imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage imageLibraryFrame9 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage libraryImage = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            OnDrawItem?.Invoke(this);
                        }

                        lockedSurface.ClipRectangle = oldClip;
                    }
                }

                RaiseDraw(this, lockedSurface);
            }
        }
        catch
        {
            _ = MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWToolTip.pas:266
    public override void CheckMouseDown(MouseButton button, Set<ShiftState> shift, int X, int Y) => SetVisible(false);

    // AOWToolTip.pas:172
    public void SetFStrings(DelphiStringList value)
    {
        _lines.Clear();
        _items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = _lines.Add(valueValue);
            string valueValue2 = value[i];
            _ = _items.Add(valueValue2);
        }

        Sized = false;
    }

    [Published]
    public DelphiStringList Items { get => _items; set => SetFStrings(value); }

    [Published]
    public bool ToolTipStatic { get; set; }

    [Published]
    public int ToolTipDelay { get; set; }

    [Published]
    public int ShowDuration { get; set; }

    [Published]
    public int ItemHeight { get; set; }

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
