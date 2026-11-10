// Translated from AOWToolTip.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

// { The AOWToolTip unit of aowInt.dpl: TAOWToolTip, the small window that TAOWControl shows with a
//   control's tool tip text after the mouse rested on it for ToolTipDelay updates. }
// Fields: FItems, FOnDrawItem, FToolTipStatic, FToolTipDelay, FShowDuration, FItemHeight,
// FShowing, FControlRect (the control on the screen. the tip shows at its bottom right),
// FItemIndex, FItemRect (the line being drawn, for OnDrawItem), FLines (Items broken into lines
// that fit)
public partial class TAOWToolTip : TAOWGenericWindow
{
    public TStringList FItems;
    public TNotifyEvent<TAOWToolTip> FOnDrawItem;
    public bool FToolTipStatic;
    public int FToolTipDelay;
    public int FShowDuration;
    public int FItemHeight;
    public bool FShowing;
    public TRect FControlRect;
    public int FItemIndex;
    public TRect FItemRect;
    public TStringList FLines;

    // AOWToolTip.pas:62
    public TAOWToolTip(TComponent owner) : base(owner)
    {
        FLines = new TStringList();
        FItems = new TStringList();
        FItems.OnChange = OnChangeText;
        FShowDuration = 100;
        FItemHeight = 10;
    }

    // AOWToolTip.pas:72
    public override void Dispose()
    {
        FLines.Dispose();
        FItems.Dispose();
        base.Dispose();
    }

    // AOWToolTip.pas:79
    public override void Validate()
    {
        if (FILFrame != null)
        {
            FILFrame = FILFrame.CheckIL(FILIndexFrame, 8, this);
        }

        if (FILTexture != null)
        {
            FILTexture = FILTexture.CheckIL(FILIndexTexture, 1, this);
        }

        if (FAOWOwner != null && FILFrame != null && FILTexture != null && FAOWFont != null)
        {
            FValid = true;
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
        bool Moved;
        bool Done;
        if (FOwnerItem != 0)
        {
            FWinWidth = FOwnerItem;
        }
        else
        {
            FOwnerItem = FWinWidth;
        }

        var ilFrameHeight = FILFrame[FILIndexFrame];
        var ilFrame = FILFrame[FILIndexFrame + 5];
        if (ilFrameHeight.Height + ilFrame.Height > FWinHeight)
        {
            var ilFrame2 = FILFrame[FILIndexFrame];
            var ilFrame3 = FILFrame[FILIndexFrame + 5];
            FWinHeight = ilFrame2.Height + ilFrame3.Height;
        }

        var ilFrameWidth = FILFrame[FILIndexFrame];
        var ilFrame4 = FILFrame[FILIndexFrame + 2];
        if (ilFrameWidth.Width + ilFrame4.Width > FWinWidth)
        {
            var ilFrame2 = FILFrame[FILIndexFrame];
            var ilFrame3 = FILFrame[FILIndexFrame];
            FWinWidth = ilFrame2.Width + ilFrame3.Width;
        }

        FLines.Clear();
        I = 0;
        int last = FItems.Count - 1;
        for (; I <= last; I += 1)
        {
            var item = FItems[I];
            FLines.Add(item);
        }

        if (FLines.Count > 0)
        {
            I = 0;
            while (I < FLines.Count)
            {
                Moved = false;
                Done = true;
                while (true)
                {
                    bool notDone = !Done;
                    if (!notDone)
                    {
                        var line = FLines[I];
                        var textWidth = FAOWFont.GetTextWidth(line);
                        var ilFrame2 = FILFrame[3];
                        var ilFrame3 = FILFrame[4];
                        notDone = textWidth > FWinWidth - ilFrame2.Width - ilFrame3.Width - FBorderWidth * 2;
                    }

                    if (!(notDone && FLines[I].Length > 1))
                        break;
                    Done = false;
                    if (I < FLines.Count - 1)
                    {
                        if (Moved == true)
                        {
                            var lineLength = FLines[I];
                            var line = FLines[I];
                            var line2 = FLines[I + 1];
                            FLines[I + 1] = Copy(line, lineLength.Length, 1) + line2;
                        }
                        else
                        {
                            var lineLength = FLines[I];
                            var line = FLines[I];
                            FLines.Insert(I + 1, Copy(line, lineLength.Length, 1));
                        }
                    }
                    else
                    {
                        var lineLength = FLines[I];
                        var line = FLines[I];
                        FLines.Add(Copy(line, lineLength.Length, 1));
                    }

                    var line3 = FLines[I];
                    var line4 = FLines[I];
                    FLines[I] = Copy(line4, 1, line3.Length - 1);
                    Moved = true;
                    var line5 = FLines[I];
                    if (Copy(FLines[I], line5.Length, 1) == " ")
                    {
                        Done = true;
                    }
                }

                I += 1;
            }

            var ilFrame5 = FILFrame[FILIndexFrame + 1];
            var ilFrame6 = FILFrame[FILIndexFrame + 6];
            var count = FLines.Count;
            FWinHeight = ilFrame5.Height + ilFrame6.Height + FBorderHeight * 2 + count * FItemHeight;
            var line6 = FLines[0];
            W = FAOWFont.GetTextWidth(line6);
            var ilFrame7 = FILFrame[FILIndexFrame];
            var ilFrame8 = FILFrame[FILIndexFrame + 2];
            if (W < FWinWidth - ilFrame7.Width - ilFrame8.Width - FBorderWidth * 2)
            {
                var ilFrame2 = FILFrame[FILIndexFrame];
                var ilFrame3 = FILFrame[FILIndexFrame + 2];
                FWinWidth = W + ilFrame2.Width + ilFrame3.Width + FBorderWidth * 2;
            }
        }

        if (FMasked)
        {
            FFrameLeft = FILFrame[FILIndexFrame + 3].Width;
            var ilFrame2 = FILFrame[FILIndexFrame + 4];
            FFrameRight = FWinWidth - ilFrame2.Width;
            FFrameTop = FILFrame[FILIndexFrame + 1].Height;
            var ilFrame3 = FILFrame[FILIndexFrame + 6];
            FFrameBottom = FWinHeight - ilFrame3.Height;
        }

        FWinLeft = FControlRect.Right;
        FWinTop = FControlRect.Bottom;
        if (FWinManager.FWinWidth < FWinLeft + FWinWidth)
        {
            FWinLeft = FWinManager.FWinWidth - FWinWidth;
        }

        if (FWinTop + FWinHeight > FWinManager.FWinHeight)
        {
            FWinTop = FControlRect.Top - FWinHeight;
        }

        if (FWinTop < 0)
        {
            FWinTop = 0;
        }

        base.SetSize();
    }

    // AOWToolTip.pas:162
    public override void Update()
    {
        bool B;
        B = FDoubleBuffered;
        FDoubleBuffered = false;
        base.Update();
        FDoubleBuffered = B;
    }

    // AOWToolTip.pas:186
    public override void SetVisible(bool Value)
    {
        if (Value != FVisible)
        {
            FVisible = Value;
            if (!FVisible)
            {
                FHidden = true;
                if (FOnHide != null)
                {
                    FOnHide(this);
                }
            }
            else
            {
                if (FDoubleBuffered)
                {
                    FSurfaceValid = false;
                }
                else
                {
                    FDrawn = false;
                }

                if (!FHidden)
                {
                    FBackRect.Right = FWinWidth;
                    FBackRect.Bottom = FWinHeight;
                    FBackRect.Top = FWinTop;
                    FBackRect.Left = FWinLeft;
                }

                if (FOnShow != null)
                {
                    FOnShow(this);
                }
            }

            if (FVisible == true)
            {
                FHidden = false;
                SetPriority(FPriority);
            }
        }

        FBackRect.Right = 0;
        FBackRect.Bottom = 0;
    }

    // AOWToolTip.pas:223
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect OldClip;
        try
        {
            if (FValid && FVisible)
            {
                var rect = Rect(FWinLeft, FWinTop, FWinWidth, FWinHeight);
                FILFrame.DrawILF(FILIndexFrame, rect, ref SurfDesc);
                var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                var ilFrame = FILFrame[FILIndexFrame + 6];
                var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                var ilFrame2 = FILFrame[FILIndexFrame + 4];
                var ilFrame3 = FILFrame[FILIndexFrame + 1];
                var ilFrame4 = FILFrame[FILIndexFrame + 3];
                var rect2 = Rect(FWinLeft + ilFrame4.Width, FWinTop + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                FILTexture.DrawILT(FILIndexTexture, rect2, ref SurfDesc);
                FAOWFont.Color = FColor;
                OldClip = SurfDesc.ClipRect;
                if (FLines.Count > 0)
                {
                    for (int I = 0, last = FLines.Count - 1; I <= last; I++)
                    {
                        var ilFrame5 = FILFrame[FILIndexFrame + 4];
                        if (FWinLeft + FWinWidth - ilFrame5.Width < OldClip.Right)
                        {
                            var ilFrame6 = FILFrame[FILIndexFrame + 4];
                            SurfDesc.ClipRect.Right = FWinLeft + FWinWidth - ilFrame6.Width;
                        }

                        if (I < FLines.Count && FLines.Count > 0)
                        {
                            var line = FLines[I];
                            var ilFrame6 = FILFrame[FILIndexFrame];
                            var ilFrame7 = FILFrame[FILIndexFrame + 3];
                            FAOWFont.SetText(FWinLeft + FBorderWidth + ilFrame7.Width, FWinTop + FBorderHeight + ilFrame6.Height + FItemHeight * I, line, SurfDesc);
                            FItemIndex = I;
                            var ilFrame8 = FILFrame[FILIndexFrame + 1];
                            var ilFrame9 = FILFrame[FILIndexFrame + 1];
                            var libraryImage = FILFrame[FILIndexFrame + 3];
                            FItemRect = Rect(FWinLeft + libraryImage.Width, FWinTop + ilFrame9.Height + FItemHeight * I, FWinLeft + FWinWidth, FWinTop + ilFrame8.Height + (I + 1) * FItemHeight);
                            if (FOnDrawItem != null)
                            {
                                FOnDrawItem(this);
                            }
                        }

                        SurfDesc.ClipRect = OldClip;
                    }
                }

                if (FOnDraw != null)
                {
                    FOnDraw(this, SurfDesc);
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Draw", "Error - " + Name, 0);
        }
    }

    // AOWToolTip.pas:266
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FShowing = false;
        SetVisible(false);
    }

    // AOWToolTip.pas:172
    public void SetFStrings(TStringList Value)
    {
        FLines.Clear();
        FItems.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FLines.Add(valueValue);
            var valueValue2 = Value[I];
            FItems.Add(valueValue2);
        }

        FSized = false;
    }

    [Published]
    public TImageLibraryFont AOWFont { get => FAOWFont; set => FAOWFont = value; }

    [Published]
    public TStringList Items { get => FItems; set => SetFStrings(value); }

    [Published]
    public TNotifyEvent<TAOWToolTip> OnDrawItem { get => FOnDrawItem; set => FOnDrawItem = value; }

    [Published]
    public bool ToolTipStatic { get => FToolTipStatic; set => FToolTipStatic = value; }

    [Published]
    public int ToolTipDelay { get => FToolTipDelay; set => FToolTipDelay = value; }

    [Published]
    public int ShowDuration { get => FShowDuration; set => FShowDuration = value; }

    [Published]
    public int ItemHeight { get => FItemHeight; set => FItemHeight = value; }

    public new TNotifyEvent<TAOWToolTip> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TAOWToolTip> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TAOWToolTip> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TAOWToolTip> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TAOWToolTip> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TAOWToolTip> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TAOWToolTip> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TAOWToolTip> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new TAoWOnDrawBackEvent<TAOWToolTip> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new TAoWOnDrawSurface<TAOWToolTip> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new TAoWOnDrawSurface<TAOWToolTip> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new TNotifyEvent<TAOWToolTip> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }
}

public static partial class AOWToolTipUnit
{
}
