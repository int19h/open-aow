// Translated from StartMenu.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Ui.Windows;

// whole-menu image library
public partial class TStartMenu : TAOWMenu
{
    public TStartMenu(TComponent owner) : base(owner)
    { }

    public TImageLibrary FILWholeMenu;

    // StartMenu.pas:30
    public override void SetSize()
    {
        TAOWWinManager M;
        base.SetSize();
        FWinHeight = 377;
        M = FWinManager;
        if (M != null && FOwnerMenuBar != null)
        {
            FWinTop = M.FWinHeight - 393;
        }

        FWinLeft = 0;
    }

    // StartMenu.pas:41
    public override void SetVisible(bool Value)
    {
        TAOWWinManager M = default;
        TAOWMenuBar B = default;
        if (Value != FVisible)
        {
            FVisible = Value;
            if (FVisible == true)
            {
                FHighlighted = -1;
                FWinHeight = 377;
                M = FWinManager;
                if (M != null && FOwnerMenuBar != null)
                {
                    FWinTop = M.FWinHeight - 393;
                }

                FWinLeft = 0;
                FDrawn = false;
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
            else
            {
                B = FOwnerMenuBar;
                if (B != null)
                {
                    B.FSelected = -1;
                    B.FDrawn = false;
                }

                FHidden = true;
                if (FOnHide != null)
                {
                    FOnHide(this);
                }
            }
        }

        if (FVisible)
        {
            if (!FValid || !FAligned || !FSized)
            {
                Setup();
            }

            ClearBltList();
            ClearDrawList();
            FDrawn = false;
            SetPriority(FPriority);
        }
    }

    // StartMenu.pas:87
    public override void Draw(TSurfaceDesc SurfDesc)
    {
        TRect OldClip = default;
        try
        {
            if (FValid && FVisible)
            {
                if (!FDrawn)
                {
                    OldClip = SurfDesc.ClipRect;
                    FAOWFont.Color = FColor;
                    FILWholeMenu[FHighlighted + 1].ShowClipped(0, 0, SurfDesc);
                    for (int I = 0, last = FItems.Count - 1; I <= last; I++)
                    {
                        if (FItems.Count > 0)
                        {
                            var ilFrameWidth = FILFrame[FILIndexFrame + 2];
                            if (FWinLeft + FWinWidth - ilFrameWidth.Width < OldClip.Right)
                            {
                                var ilFrame = FILFrame[FILIndexFrame + 2];
                                SurfDesc.ClipRect.Right = FWinLeft + FWinWidth - ilFrame.Width;
                            }

                            if (I == FHighlighted)
                            {
                                FAOWFont.Color = FSelColor;
                            }

                            var item = FItems[I];
                            var ilFrameHeight = FILFrame[FILIndexFrame];
                            var ilFrame2 = FILFrame[FILIndexFrame + 3];
                            FAOWFont.SetText(FWinLeft + FBorderWidth + ilFrame2.Width, FWinTop + FBorderHeight + ilFrameHeight.Height + FItemHeight * I, item, SurfDesc);
                            if (I == FHighlighted)
                            {
                                FAOWFont.Color = FColor;
                            }

                            FDrawItem = I;
                            var ilFrame3 = FILFrame[FILIndexFrame + 1];
                            var ilFrame4 = FILFrame[FILIndexFrame + 1];
                            var ilFrame5 = FILFrame[FILIndexFrame + 3];
                            FDrawItemRect = Rect(FWinLeft + ilFrame5.Width, FWinTop + ilFrame4.Height + FItemHeight * I, FWinLeft + FWinWidth, FWinTop + ilFrame3.Height + (I + 1) * FItemHeight);
                            if (FOnDrawItem != null)
                            {
                                FOnDrawItem(this);
                            }

                            SurfDesc.ClipRect = OldClip;
                        }
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

    // StartMenu.pas:123
    public override void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        if (FValid)
        {
            var ilFrameHeight = FILFrame[FILIndexFrame + 6];
            FWinHeight -= ilFrameHeight.Height;
            base.CheckMouseDown(Button, Shift, X, Y);
            var ilFrame = FILFrame[FILIndexFrame + 6];
            FWinHeight += ilFrame.Height;
        }
    }

    [Published]
    public TImageLibrary ILWholeMenu { get => FILWholeMenu; set => FILWholeMenu = value; }

    public new TNotifyEvent<TStartMenu> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new TMouseEvent<TStartMenu> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new TMouseEvent<TStartMenu> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new TMouseMoveEvent<TStartMenu> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new TKeyPressEvent<TStartMenu> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new TKeyEvent<TStartMenu> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new TAoWOnDrawEvent<TStartMenu> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new TAoWOnDrawEvent<TStartMenu> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new TNotifyEvent<TStartMenu> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public new TAoWOnDrawBackEvent<TStartMenu> OnDrawBack
    {
        get => EventSlot.Get(this, FOnDrawBack);
        set => EventSlot.Set(this, ref FOnDrawBack, value);
    }

    public new TNotifyEvent<TStartMenu> OnEnter
    {
        get => EventSlot.Get(this, FOnEnter);
        set => EventSlot.Set(this, ref FOnEnter, value);
    }

    public new TNotifyEvent<TStartMenu> OnExit
    {
        get => EventSlot.Get(this, FOnExit);
        set => EventSlot.Set(this, ref FOnExit, value);
    }

    public new TNotifyEvent<TStartMenu> OnChangeCursor
    {
        get => EventSlot.Get(this, FOnChangeCursor);
        set => EventSlot.Set(this, ref FOnChangeCursor, value);
    }

    public new TAoWOnDrawSurface<TStartMenu> OnDrawSurface
    {
        get => EventSlot.Get(this, FOnDrawSurface);
        set => EventSlot.Set(this, ref FOnDrawSurface, value);
    }

    public new TAoWOnDrawSurface<TStartMenu> OnPreDrawSurface
    {
        get => EventSlot.Get(this, FOnPreDrawSurface);
        set => EventSlot.Set(this, ref FOnPreDrawSurface, value);
    }

    public new TNotifyEvent<TStartMenu> OnHide
    {
        get => EventSlot.Get(this, FOnHide);
        set => EventSlot.Set(this, ref FOnHide, value);
    }

    public new TNotifyEvent<TStartMenu> OnShow
    {
        get => EventSlot.Get(this, FOnShow);
        set => EventSlot.Set(this, ref FOnShow, value);
    }

    public new TNotifyEvent<TStartMenu> OnDrawItem
    {
        get => EventSlot.Get(this, FOnDrawItem);
        set => EventSlot.Set(this, ref FOnDrawItem, value);
    }

    public new TNotifyEvent<TStartMenu> OnSelect
    {
        get => EventSlot.Get(this, FOnSelect);
        set => EventSlot.Set(this, ref FOnSelect, value);
    }
}
