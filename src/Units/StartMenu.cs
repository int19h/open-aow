// Translated from StartMenu.pas.
using Aow.Rtl;
using Aow.Units.AoWComp;
using Aow.Units.AOWMenu;
using Aow.Units.AOWMenuBar;
using Aow.Units.AoWWinControl;
using Aow.Units.AOWWinManager;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using static Aow.Rtl.RtlUnit;

namespace Aow.Units.StartMenu;

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
        get => EventSlot.Get(this, "TStartMenu.FOnChange", FOnChange, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnChange", ref FOnChange, value, handler => sender => handler(this));
    }

    public new TMouseEvent<TStartMenu> OnMouseDown
    {
        get => EventSlot.Get(this, "TStartMenu.FOnMouseDown", FOnMouseDown, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnMouseDown", ref FOnMouseDown, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseEvent<TStartMenu> OnMouseUp
    {
        get => EventSlot.Get(this, "TStartMenu.FOnMouseUp", FOnMouseUp, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnMouseUp", ref FOnMouseUp, value, handler => (sender, button, shift, x, y) => handler(this, button, shift, x, y));
    }

    public new TMouseMoveEvent<TStartMenu> OnMouseMove
    {
        get => EventSlot.Get(this, "TStartMenu.FOnMouseMove", FOnMouseMove, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnMouseMove", ref FOnMouseMove, value, handler => (sender, shift, x, y) => handler(this, shift, x, y));
    }

    public new TKeyPressEvent<TStartMenu> OnKeyPress
    {
        get => EventSlot.Get(this, "TStartMenu.FOnKeyPress", FOnKeyPress, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnKeyPress", ref FOnKeyPress, value, handler => (sender, ref key) => handler(this, ref key));
    }

    public new TKeyEvent<TStartMenu> OnKeyDown
    {
        get => EventSlot.Get(this, "TStartMenu.FOnKeyDown", FOnKeyDown, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnKeyDown", ref FOnKeyDown, value, handler => (sender, ref key, shift) => handler(this, ref key, shift));
    }

    public new TAoWOnDrawEvent<TStartMenu> OnPreDraw
    {
        get => EventSlot.Get(this, "TStartMenu.FOnPreDraw", FOnPreDraw, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnPreDraw", ref FOnPreDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TAoWOnDrawEvent<TStartMenu> OnDraw
    {
        get => EventSlot.Get(this, "TStartMenu.FOnDraw", FOnDraw, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnDraw", ref FOnDraw, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TStartMenu> OnUpdate
    {
        get => EventSlot.Get(this, "TStartMenu.FOnUpdate", FOnUpdate, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnUpdate", ref FOnUpdate, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawBackEvent<TStartMenu> OnDrawBack
    {
        get => EventSlot.Get(this, "TStartMenu.FOnDrawBack", FOnDrawBack, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnDrawBack", ref FOnDrawBack, value, handler => (sender, surface) => handler(this, surface));
    }

    public new TNotifyEvent<TStartMenu> OnEnter
    {
        get => EventSlot.Get(this, "TStartMenu.FOnEnter", FOnEnter, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnEnter", ref FOnEnter, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TStartMenu> OnExit
    {
        get => EventSlot.Get(this, "TStartMenu.FOnExit", FOnExit, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnExit", ref FOnExit, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TStartMenu> OnChangeCursor
    {
        get => EventSlot.Get(this, "TStartMenu.FOnChangeCursor", FOnChangeCursor, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnChangeCursor", ref FOnChangeCursor, value, handler => sender => handler(this));
    }

    public new TAoWOnDrawSurface<TStartMenu> OnDrawSurface
    {
        get => EventSlot.Get(this, "TStartMenu.FOnDrawSurface", FOnDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnDrawSurface", ref FOnDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TAoWOnDrawSurface<TStartMenu> OnPreDrawSurface
    {
        get => EventSlot.Get(this, "TStartMenu.FOnPreDrawSurface", FOnPreDrawSurface, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnPreDrawSurface", ref FOnPreDrawSurface, value, handler => (sender, surface, left, top) => handler(this, surface, left, top));
    }

    public new TNotifyEvent<TStartMenu> OnHide
    {
        get => EventSlot.Get(this, "TStartMenu.FOnHide", FOnHide, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnHide", ref FOnHide, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TStartMenu> OnShow
    {
        get => EventSlot.Get(this, "TStartMenu.FOnShow", FOnShow, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnShow", ref FOnShow, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TStartMenu> OnDrawItem
    {
        get => EventSlot.Get(this, "TStartMenu.FOnDrawItem", FOnDrawItem, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnDrawItem", ref FOnDrawItem, value, handler => sender => handler(this));
    }

    public new TNotifyEvent<TStartMenu> OnSelect
    {
        get => EventSlot.Get(this, "TStartMenu.FOnSelect", FOnSelect, static slot => slot);
        set => EventSlot.Set(this, "TStartMenu.FOnSelect", ref FOnSelect, value, handler => sender => handler(this));
    }
}

public static partial class StartMenuUnit
{
    public static void Initialize()
    {
    }

    public static void Finalize()
    {
    }
}
