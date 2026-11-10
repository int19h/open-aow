// Translated from AoWControl.pas.
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Controls;

// { The AoWControl unit of aowInt.dpl: TAOWControl, the ancestor of the controls that sit in a
//   window (buttons, edits, lists, scroll bars, ...). It knows its window and shows its tool tip. }
// Fields: FToolTipText, FToolTipShown (updates the tool tip has been visible), FAOWWindow,
// FToolTip
public partial class AOWControl : AoWComponent
{
    public StringList FToolTipText;
    public int FToolTipShown;
    public AOWBaseWindow FAOWWindow;
    public AOWToolTip FToolTip;

    // AoWControl.pas:46
    public AOWControl(global::Aow.Rtl.Component owner) : base(owner)
    {
        FToolTipText = new StringList();
        FCaptureInput = true;
    }

    // AoWControl.pas:53
    public override void Dispose()
    {
        FToolTipText.Dispose();
        base.Dispose();
    }

    // AoWControl.pas:84
    // { Update counts the tool tip delay and shows or hides the tool tip, and asks the owner to redraw
    //   the control's old and new places when it moved, changed size or became hidden. FOldRect holds
    //   the place of the last update as (Left, Top, Width, Height); the rectangles to redraw are in the
    //   coordinates of the window manager (the owner's position is added unless the owner is the
    //   manager's direct child... of a window placed in the manager) }
    public override void Update()
    {
        global::Aow.Rtl.Rect R = default;
        AOWBaseWindow W;
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
                                while (W.FAOWOwner is AOWBaseWindow)
                                {
                                    W = (AOWBaseWindow)W.FAOWOwner;
                                }

                                if (FAOWWindow != W)
                                {
                                    var rect = global::Aow.Rtl.RtlUnit.Rect(FAOWWindow.FWinLeft + W.FWinLeft + FWinLeft, FAOWWindow.FWinTop + W.FWinTop + FWinTop, FAOWWindow.FWinLeft + W.FWinLeft + FToolTipLeft, FAOWWindow.FWinTop + W.FWinTop + FWinTop + FWinHeight);
                                    FToolTip.FControlRect = rect;
                                }
                                else
                                {
                                    var rect = global::Aow.Rtl.RtlUnit.Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FAOWWindow.FWinLeft + FToolTipLeft, FAOWWindow.FWinTop + FWinTop + FWinHeight);
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
                        R = global::Aow.Rtl.RtlUnit.Rect(FOldRect.Left, FOldRect.Top, FOldRect.Left + FOldRect.Right - 1, FOldRect.Top + FOldRect.Bottom - 1);
                    }
                    else
                    {
                        R = global::Aow.Rtl.RtlUnit.Rect(FAOWOwner.FWinLeft + FOldRect.Left, FAOWOwner.FWinTop + FOldRect.Top, FAOWOwner.FWinLeft + FOldRect.Left + FOldRect.Right - 1, FAOWOwner.FWinTop + FOldRect.Top + FOldRect.Bottom - 1);
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
                            R = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                        }
                        else
                        {
                            R = global::Aow.Rtl.RtlUnit.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                        }

                        FAOWOwner.AddDrawRect(ref R, this);
                    }
                }

                if (!FDrawn)
                {
                    if (FAOWOwner.FAOWOwner.FAOWOwner == null)
                    {
                        R = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                    }
                    else
                    {
                        R = global::Aow.Rtl.RtlUnit.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
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
                        R = global::Aow.Rtl.RtlUnit.Rect(FWinLeft, FWinTop, FWinLeft + FWinWidth - 1, FWinTop + FWinHeight - 1);
                    }
                    else
                    {
                        R = global::Aow.Rtl.RtlUnit.Rect(FAOWOwner.FWinLeft + FWinLeft, FAOWOwner.FWinTop + FWinTop, FAOWOwner.FWinLeft + FWinLeft + FWinWidth - 1, FAOWOwner.FWinTop + FWinTop + FWinHeight - 1);
                    }

                    FAOWOwner.AddDrawRect(ref R, this);
                    FHidden = false;
                }
            }
        }
    }

    // AoWControl.pas:64
    public override void SetOwner(AoWComponent Value)
    {
        base.SetOwner(Value);
        FAOWWindow = (AOWBaseWindow)FAOWOwner;
    }

    // AoWControl.pas:190
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
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
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
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
    public void SetAOWWindow(AOWBaseWindow Value)
    {
        SetOwner(Value);
    }

    // AoWControl.pas:70
    public void SetToolStrings(StringList Value)
    {
        FToolTipText.Clear();
        for (int I = 0, last = Value.Count - 1; I <= last; I++)
        {
            var valueValue = Value[I];
            FToolTipText.Add(valueValue);
        }
    }

    [Published]
    public AOWBaseWindow AOWWindow { get => FAOWWindow; set => SetAOWWindow(value); }

    [Published]
    public AOWToolTip ToolTip { get => FToolTip; set => FToolTip = value; }

    [Published]
    public StringList ToolTipText { get => FToolTipText; set => SetToolStrings(value); }

    public new NotifyEvent<AOWControl> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<AOWControl> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<AOWControl> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<AOWControl> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<AOWControl> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<AOWControl> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<AOWControl> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<AOWControl> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<AOWControl> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
