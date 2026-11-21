#nullable disable warnings
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
public sealed class ToolTip : GenericWindow
{
    [Published]
    public event EventHandler? DrawItem;

    public Rect ControlRectangle { get; set; }

    private readonly DelphiStringList _lines;

    // AOWToolTip.pas:62
    public ToolTip(Component owner) : base(owner)
    {
        _lines = [];
        Items = new DelphiStringList
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
        Items.Dispose();
        base.Dispose();
    }

    // AOWToolTip.pas:79
    public override void Validate()
    {
        if (ImageLibraryFrame is not null)
        {
            ImageLibraryFrame = ImageLibraryFrame.CheckImageLibrary(ImageLibraryIndexFrame, count: 8, sender: this);
        }

        if (ImageLibraryTexture is not null)
        {
            ImageLibraryTexture = ImageLibraryTexture.CheckImageLibrary(ImageLibraryIndexTexture, count: 1, sender: this);
        }

        if (OwnerComponent is not null && ImageLibraryFrame is not null && ImageLibraryTexture is not null && Font is not null)
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
        if (OwnerItem != 0)
        {
            SetWinWidthWithoutInvalidate(OwnerItem);
        }
        else
        {
            OwnerItem = WinWidth;
        }

        LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
        if (imageLibraryFrameHeight.Height + imageLibraryFrame.Height > WinHeight)
        {
            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 5];
            SetWinHeightWithoutInvalidate(imageLibraryFrame2.Height + imageLibraryFrame3.Height);
        }

        LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame];
        LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
        if (imageLibraryFrameWidth.Width + imageLibraryFrame4.Width > WinWidth)
        {
            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame];
            SetWinWidthWithoutInvalidate(imageLibraryFrame2.Width + imageLibraryFrame3.Width);
        }

        _lines.Clear();
        int i = 0;
        int last = Items.Count - 1;
        for (; i <= last; i += 1)
        {
            string item = Items[i];
            _ = _lines.Add(item);
        }

        if (_lines.Count > 0)
        {
            i = 0;
            while (i < _lines.Count)
            {
                bool moved = false;
                bool done = true;
                while (true)
                {
                    bool notDone = !done;
                    if (!notDone)
                    {
                        string line = _lines[i];
                        int textWidth = Font.GetTextWidth(line);
                        LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[3];
                        LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[4];
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
                            _lines[i + 1] = Copy(line, index: lineLength.Length, count: 1) + line2;
                        }
                        else
                        {
                            string lineLength = _lines[i];
                            string line = _lines[i];
                            _lines.Insert(i + 1, value: Copy(line, index: lineLength.Length, count: 1));
                        }
                    }
                    else
                    {
                        string lineLength = _lines[i];
                        string line = _lines[i];
                        _ = _lines.Add(Copy(line, index: lineLength.Length, count: 1));
                    }

                    string line3 = _lines[i];
                    string line4 = _lines[i];
                    _lines[i] = Copy(line4, index: 1, count: line3.Length - 1);
                    moved = true;
                    string line5 = _lines[i];
                    if (Copy(_lines[i], index: line5.Length, count: 1) == " ")
                    {
                        done = true;
                    }
                }

                i += 1;
            }

            LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
            LibraryImage? imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
            int count = _lines.Count;
            SetWinHeightWithoutInvalidate(imageLibraryFrame5.Height + imageLibraryFrame6.Height + (BorderHeight * 2) + (count * ItemHeight));
            string line6 = _lines[0];
            int w = Font.GetTextWidth(line6);
            LibraryImage? imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame];
            LibraryImage? imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
            if (w < WinWidth - imageLibraryFrame7.Width - imageLibraryFrame8.Width - (BorderWidth * 2))
            {
                LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame];
                LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 2];
                SetWinWidthWithoutInvalidate(w + imageLibraryFrame2.Width + imageLibraryFrame3.Width + (BorderWidth * 2));
            }
        }

        if (Masked)
        {
            FrameLeft = ImageLibraryFrame[ImageLibraryIndexFrame + 3].Width;
            LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
            FrameRight = WinWidth - imageLibraryFrame2.Width;
            FrameTop = ImageLibraryFrame[ImageLibraryIndexFrame + 1].Height;
            LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
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
        bool b = DoubleBuffered;
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
                    IsDrawn = false;
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
        try
        {
            if (Valid && Visible)
            {
                SizedRectangle rectangle = new(WinLeft, WinTop, WinWidth, WinHeight);
                ImageLibraryFrame.DrawFrame(ImageLibraryIndexFrame, area: rectangle, ref lockedSurface);
                LibraryImage? imageLibraryFrameHeight = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame = ImageLibraryFrame[ImageLibraryIndexFrame + 6];
                LibraryImage? imageLibraryFrameWidth = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                LibraryImage? imageLibraryFrame2 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                LibraryImage? imageLibraryFrame3 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                LibraryImage? imageLibraryFrame4 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                SizedRectangle rectangle2 = new(WinLeft + imageLibraryFrame4.Width, WinTop + imageLibraryFrame3.Height, WinWidth - imageLibraryFrameWidth.Width - imageLibraryFrame2.Width, WinHeight - imageLibraryFrameHeight.Height - imageLibraryFrame.Height);
                ImageLibraryTexture.DrawTiled(ImageLibraryIndexTexture, area: rectangle2, ref lockedSurface);
                Font.Color = Color;
                Rect oldClip = lockedSurface.ClipRectangle;
                if (_lines.Count > 0)
                {
                    for (int i = 0, last = _lines.Count - 1; i <= last; i++)
                    {
                        LibraryImage? imageLibraryFrame5 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                        if (WinLeft + WinWidth - imageLibraryFrame5.Width < oldClip.Right)
                        {
                            LibraryImage? imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame + 4];
                            Rect clipRectangle = lockedSurface.ClipRectangle;
                            clipRectangle.Right = WinLeft + WinWidth - imageLibraryFrame6.Width;
                            lockedSurface.ClipRectangle = clipRectangle;
                        }

                        if (i < _lines.Count && _lines.Count > 0)
                        {
                            string line = _lines[i];
                            LibraryImage? imageLibraryFrame6 = ImageLibraryFrame[ImageLibraryIndexFrame];
                            LibraryImage? imageLibraryFrame7 = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            Font.SetText(WinLeft + BorderWidth + imageLibraryFrame7.Width, WinTop + BorderHeight + imageLibraryFrame6.Height + (ItemHeight * i), text: line, lockedSurface);
                            LibraryImage? imageLibraryFrame8 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? imageLibraryFrame9 = ImageLibraryFrame[ImageLibraryIndexFrame + 1];
                            LibraryImage? libraryImage = ImageLibraryFrame[ImageLibraryIndexFrame + 3];
                            DrawItem?.Invoke(this, e: EventArgs.Empty);
                        }

                        lockedSurface.ClipRectangle = oldClip;
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

    // AOWToolTip.pas:266
    public override void CheckMouseDown(MouseButton button, ShiftStates shift, int X, int Y) => SetVisible(false);

    // AOWToolTip.pas:172
    public void SetFStrings(DelphiStringList value)
    {
        _lines.Clear();
        Items.Clear();
        for (int i = 0, last = value.Count - 1; i <= last; i++)
        {
            string valueValue = value[i];
            _ = _lines.Add(valueValue);
            string valueValue2 = value[i];
            _ = Items.Add(valueValue2);
        }

        Sized = false;
    }

    [Published]
    public DelphiStringList Items { get; }

    [Published]
    public bool ToolTipStatic { get; set; }

    [Published]
    public int ToolTipDelay { get; set; }

    [Published]
    public int ShowDuration { get; set; }

    [Published]
    public int ItemHeight { get; set; }

    public new event NotifyEvent<ToolTip> Change
    {
        add => ((UIComponent)this).Change += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(Change), value: value);
        remove => ((UIComponent)this).Change -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(Change), value: value);
    }

    public new event MouseEvent<ToolTip> MouseDown
    {
        add => ((UIComponent)this).MouseDown += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(MouseDown), value: value);
        remove => ((UIComponent)this).MouseDown -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(MouseDown), value: value);
    }

    public new event MouseEvent<ToolTip> MouseUp
    {
        add => ((UIComponent)this).MouseUp += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(MouseUp), value: value);
        remove => ((UIComponent)this).MouseUp -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(MouseUp), value: value);
    }

    public new event MouseMoveEvent<ToolTip> MouseMove
    {
        add => ((UIComponent)this).MouseMove += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(MouseMove), value: value);
        remove => ((UIComponent)this).MouseMove -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(MouseMove), value: value);
    }

    public new event KeyPressEvent<ToolTip> KeyPress
    {
        add => ((UIComponent)this).KeyPress += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(KeyPress), value: value);
        remove => ((UIComponent)this).KeyPress -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(KeyPress), value: value);
    }

    public new event KeyEvent<ToolTip> KeyDown
    {
        add => ((UIComponent)this).KeyDown += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(KeyDown), value: value);
        remove => ((UIComponent)this).KeyDown -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(KeyDown), value: value);
    }

    public new event AoWOnDrawEvent<ToolTip> PreDraw
    {
        add => ((UIComponent)this).PreDraw += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(PreDraw), value: value);
        remove => ((UIComponent)this).PreDraw -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(PreDraw), value: value);
    }

    public new event AoWOnDrawEvent<ToolTip> Drawn
    {
        add => ((UIComponent)this).Drawn += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(Drawn), value: value);
        remove => ((UIComponent)this).Drawn -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(Drawn), value: value);
    }

    public new event NotifyEvent<ToolTip> Updated
    {
        add => ((UIComponent)this).Updated += EventSlot.Add<UIComponent, ToolTip>(this, slot: nameof(Updated), value: value);
        remove => ((UIComponent)this).Updated -= EventSlot.Remove<UIComponent, ToolTip>(this, slot: nameof(Updated), value: value);
    }

    public new event AoWOnDrawBackEvent<ToolTip> DrawBack
    {
        add => ((WindowControl)this).DrawBack += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(DrawBack), value: value);
        remove => ((WindowControl)this).DrawBack -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(DrawBack), value: value);
    }

    public new event NotifyEvent<ToolTip> Enter
    {
        add => ((WindowControl)this).Enter += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(Enter), value: value);
        remove => ((WindowControl)this).Enter -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(Enter), value: value);
    }

    public new event NotifyEvent<ToolTip> Exit
    {
        add => ((WindowControl)this).Exit += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(Exit), value: value);
        remove => ((WindowControl)this).Exit -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(Exit), value: value);
    }

    public new event NotifyEvent<ToolTip> ChangeCursor
    {
        add => ((WindowControl)this).ChangeCursor += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(ChangeCursor), value: value);
        remove => ((WindowControl)this).ChangeCursor -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(ChangeCursor), value: value);
    }

    public new event AoWOnDrawSurface<ToolTip> Blitted
    {
        add => ((WindowControl)this).Blitted += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(Blitted), value: value);
        remove => ((WindowControl)this).Blitted -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(Blitted), value: value);
    }

    public new event AoWOnDrawSurface<ToolTip> Blitting
    {
        add => ((WindowControl)this).Blitting += EventSlot.Add<WindowControl, ToolTip>(this, slot: nameof(Blitting), value: value);
        remove => ((WindowControl)this).Blitting -= EventSlot.Remove<WindowControl, ToolTip>(this, slot: nameof(Blitting), value: value);
    }

    public new event NotifyEvent<ToolTip> Hide
    {
        add => ((BaseWindow)this).Hide += EventSlot.Add<BaseWindow, ToolTip>(this, slot: nameof(Hide), value: value);
        remove => ((BaseWindow)this).Hide -= EventSlot.Remove<BaseWindow, ToolTip>(this, slot: nameof(Hide), value: value);
    }

    public new event NotifyEvent<ToolTip> Show
    {
        add => ((BaseWindow)this).Show += EventSlot.Add<BaseWindow, ToolTip>(this, slot: nameof(Show), value: value);
        remove => ((BaseWindow)this).Show -= EventSlot.Remove<BaseWindow, ToolTip>(this, slot: nameof(Show), value: value);
    }
}
