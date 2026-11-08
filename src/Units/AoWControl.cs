// Translated from AoWControl.pas.
using Aow.Rtl;
using Aow.Units.AoWBaseWin;
using Aow.Units.AoWComp;
using Aow.Units.AOWToolTip;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.AoWControl;

// { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
// Fields: FToolTipText, FToolTipShown (updates the tool tip has been visible), FAOWWindow,
// FToolTip
public partial class TAOWControl : TAoWComponent
{
    public TStringList FToolTipText;
    public int FToolTipShown;
    public TAOWBaseWindow FAOWWindow;
    public TAOWToolTip FToolTip;

    // AoWControl.pas:46
    public TAOWControl(TComponent owner) : base(owner)
    {
        FToolTipText = new TStringList();
        FCaptureInput = true;
    }

    // AoWControl.pas:53
    public override void Destroy()
    {
        FToolTipText.Destroy();
        base.Destroy();
    }

    // AoWControl.pas:84
    // { Update counts the tool tip delay and shows or hides the tool tip, and asks the owner to redraw
    //   the control's old and new places when it moved, changed size or became hidden. FOldRect holds
    //   the place of the last update as (Left, Top, Width, Height); the rectangles to redraw are in the
    //   coordinates of the window manager (the owner's position is added unless the owner is the
    //   manager's direct child... of a window placed in the manager) }
    public override void Update()
    {
        TRect R = default;
        TAOWBaseWindow W;
        if (FValid)
        {
            if (FVisible)
            {
                base.Update();
                if (FToolTip != null)
                {
                    if (FToolTipCount > 0)
                    {
                        if (FToolTipShown > 0 && FMouseIn == true && FToolTip.FShowDuration > 0)
                        {
                            if (FToolTipShown < FToolTip.FShowDuration)
                            {
                                FToolTipShown += 1;
                            }
                            else
                            {
                                if (!FToolTip.FToolTipStatic && FToolTip != null)
                                {
                                    FToolTip.SetVisible(false);
                                }

                                FToolTipShown = 0;
                            }
                        }

                        if (FToolTip.FToolTipDelay > FToolTipCount)
                        {
                            FToolTipCount += 1;
                        }
                        else
                        {
                            if (FToolTipText.Count > 0 && !FMouseIn)
                            {
                                FToolTip.SetFStrings(FToolTipText);
                                if (!FToolTip.FToolTipStatic)
                                {
                                    FToolTip.SetVisible(true);
                                    FAOWWindow.FRedraw = true;
                                    FToolTip.FSized = false;
                                }

                                W = FAOWWindow;
                                while (W.FAOWOwner is TAOWBaseWindow)
                                {
                                    W = (TAOWBaseWindow)W.FAOWOwner;
                                }

                                if (FAOWWindow != W)
                                {
                                    var rect = Rect(FAOWWindow.FWinLeft + W.FWinLeft + FWinLeft, FAOWWindow.FWinTop + W.FWinTop + FWinTop, FAOWWindow.FWinLeft + W.FWinLeft + FToolTipLeft, FAOWWindow.FWinTop + W.FWinTop + FWinTop + FWinHeight);
                                    FToolTip.FControlRect = rect;
                                }
                                else
                                {
                                    var rect = Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FAOWWindow.FWinLeft + FToolTipLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight);
                                    FToolTip.FControlRect = rect;
                                }

                                FMouseIn = true;
                                FToolTipShown = 1;
                            }
                        }
                    }
                    else
                    {
                        if (FMouseIn == true)
                        {
                            if (!FToolTip.FToolTipStatic && FToolTip != null)
                            {
                                FToolTip.SetVisible(false);
                            }

                            FMouseIn = false;
                        }
                    }
                }

                if (FOldRect.Right > 0 && FOldRect.Bottom > 0 && (FOldRect.Left != FWinLeft || FOldRect.Top != FWinTop || FOldRect.Right != FWinWidth || FOldRect.Bottom != FWinHeight))
                {
                    if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                    {
                        R = Rect(FOldRect.Left, FOldRect.Top, FOldRect.Left + FOldRect.Right - 1, FOldRect.Top + FOldRect.Bottom - 1);
                    }
                    else
                    {
                        R = Rect(FAOWOwner.FWinLeft + FOldRect.Left, FAOWOwner.FWinTop + FOldRect.Top, FAOWOwner.FWinLeft + FOldRect.Left + FOldRect.Right - 1, FAOWOwner.FWinTop + FOldRect.Top + FOldRect.Bottom - 1);
                    }

                    FAOWOwner.AddDrawRect(ref R, this);
                    FDrawn = false;
                }
                else
                {
                    if (FMasked && !FDrawn)
                    {
                        if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                        {
                            R = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                        }
                        else
                        {
                            R = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                        }

                        FAOWOwner.AddDrawRect(ref R, this);
                    }
                }

                if (!FDrawn)
                {
                    if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                    {
                        R = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                    }
                    else
                    {
                        R = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                    }

                    var indexOf = FAOWOwner.FChildren.IndexOf(this);
                    FAOWOwner.InvalidateRect(indexOf, ref R, this);
                }

                FOldRect.Left = FWinLeft;
                FOldRect.Top = FWinTop;
                FOldRect.Right = FWinWidth;
                FOldRect.Bottom = FWinHeight;
            }
            else
            {
                if (FHidden)
                {
                    if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                    {
                        R = Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                    }
                    else
                    {
                        R = Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                    }

                    FAOWOwner.AddDrawRect(ref R, this);
                    FHidden = false;
                }
            }
        }
    }

    // AoWControl.pas:64
    public override void SetOwner(TAoWComponent Value)
    {
        base.SetOwner(Value);
        FAOWWindow = (TAOWBaseWindow)FAOWOwner;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (FToolTip != null)
        {
            FToolTip.SetVisible(false);
            FToolTipShown = FToolTip.FShowDuration;
            FMouseIn = true;
            FToolTipCount = FToolTip.FToolTipDelay;
        }
    }

    // AoWControl.pas:201
    public override void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        if (X < FAOWWindow.FWinLeft + FWinLeft || X > FAOWWindow.FWinLeft + FWinLeft + FWinWidth - 1 || Y < FAOWWindow.FWinTop + FWinTop || Y > FAOWWindow.FWinTop + FWinTop + FWinHeight - 1)
        {
            FToolTipCount = 0;
        }
        else
        {
            if (FToolTip != null && FToolTip.FToolTipDelay > FToolTipCount)
            {
                FToolTipLeft = X - FAOWWindow.FWinLeft;
                FToolTipCount = 1;
            }
        }
    }

    // AoWControl.pas:59
    public void SetAOWWindow(TAOWBaseWindow Value)
    {
        SetOwner(Value);
    }

    // AoWControl.pas:70
    public void SetToolStrings(TStringList Value)
    {
        FToolTipText.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FToolTipText.Add(valueValue);
        }
    }

    [Published]
    public TAOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

    [Published]
    public TAOWToolTip ToolTip { get => FToolTip; set => FToolTip = value; }

    [Published]
    public TStringList ToolTipText { get => FToolTipText; set => SetToolStrings(value); }

    public new TNotifyEvent<TAOWControl> OnChange
    {
        get => EventSlot.Get(this, "TAOWControl.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TAOWControl> OnMouseDown
    {
        get => EventSlot.Get(this, "TAOWControl.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TAOWControl> OnMouseUp
    {
        get => EventSlot.Get(this, "TAOWControl.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TAOWControl> OnMouseMove
    {
        get => EventSlot.Get(this, "TAOWControl.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TAOWControl> OnKeyPress
    {
        get => EventSlot.Get(this, "TAOWControl.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TAOWControl> OnKeyDown
    {
        get => EventSlot.Get(this, "TAOWControl.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TAOWControl> OnPreDraw
    {
        get => EventSlot.Get(this, "TAOWControl.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TAOWControl> OnDraw
    {
        get => EventSlot.Get(this, "TAOWControl.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TAOWControl> OnUpdate
    {
        get => EventSlot.Get(this, "TAOWControl.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TAOWControl.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }
}

public static partial class AoWControlUnit
{
}
