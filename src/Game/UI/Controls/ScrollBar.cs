// Translated from AoWVScroll.pas.
using System.ComponentModel;
using Aow.Game.Draw.Images;

using Aow.Rtl;

namespace Aow.Game.UI.Controls;

// Fields: FVanishWhenFull, FILIInc (the increment button: down or right), FILIDec (the
// decrement button), FILTBack (the background of a scroll bar), FILLBack (the background line
// of a slider), FILITab (an icon tab), FILLTab (a line tab), F130 (the length of the track the
// tab moves in), F134 (the length of the tab), FMin, FMax, FSmallChange, FLargeChange,
// FPosition, F14C (the number of positions, FMax - FMin + 1), F150 (the updates the mouse
// button has been held. 0 when it is up), F151 (the part that was clicked: 1 the decrement
// button, 2 the track before the tab, 3 the track after it, 4 the increment button, 5 the tab),
// FDelay (the updates before a held button repeats), F154 (the position a click in the track
// pages to. the mouse offset into the tab while the tab is dragged), FNumberVisible, F15C
// (unused), FScrollType, FTabType, FILIndexIncUp, FILIndexIncDown, FILIndexDecUp,
// FILIndexDecDown, FILIndexBack, FILIndexTab
public partial class ScrollBar : Control
{
    public bool FVanishWhenFull;
    public InterfaceImageLibrary FILIInc;
    public InterfaceImageLibrary FILIDec;
    public InterfaceImageLibrary FILTBack;
    public InterfaceImageLibrary FILLBack;
    public InterfaceImageLibrary FILITab;
    public InterfaceImageLibrary FILLTab;
    public int F130;
    public int F134;
    public int FMin;
    public int FMax;
    public int FSmallChange;
    public int FLargeChange;
    public int FPosition;
    public int F14C;
    public byte F150;
    public byte F151;
    public byte FDelay;
    public int F154;
    public int FNumberVisible;
    public int F15C;
    public Style FScrollType;
    public ThumbStyle FTabType;
    public int FILIndexIncUp;
    public int FILIndexIncDown;
    public int FILIndexDecUp;
    public int FILIndexDecDown;
    public int FILIndexBack;
    public int FILIndexTab;

    // AoWVScroll.pas:130
    public ScrollBar(Rtl.Component owner) : base(owner)
    {
        FSmallChange = 1;
        FLargeChange = 1;
        FMax = 1;
        FNumberVisible = 1;
        FDelay = 20;
    }

    // AoWVScroll.pas:251
    // { Validate checks the image libraries and picks the kind of scroll bar: both buttons with a
    //   background and a tab make a scroll bar, both buttons alone make an increment/decrement pair,
    //   a background line with a tab makes a slider; anything else leaves the scroll bar invalid }
    public override void Validate()
    {
        if (FILIInc != null)
        {
            FILIInc = FILIInc.CheckIL(FILIndexIncUp, 1, this);
        }

        if (FILIInc != null)
        {
            FILIInc = FILIInc.CheckIL(FILIndexIncDown, 1, this);
        }

        if (FILIDec != null)
        {
            FILIDec = FILIDec.CheckIL(FILIndexDecUp, 1, this);
        }

        if (FILIDec != null)
        {
            FILIDec = FILIDec.CheckIL(FILIndexDecDown, 1, this);
        }

        if (FILTBack != null)
        {
            FILTBack = FILTBack.CheckIL(FILIndexBack, 1, this);
        }

        if (FILLBack != null)
        {
            FILLBack = FILLBack.CheckIL(FILIndexBack, 3, this);
        }

        if (FILITab != null)
        {
            FILITab = FILITab.CheckIL(FILIndexTab, 3, this);
        }

        if (FILLTab != null)
        {
            FILLTab = FILLTab.CheckIL(FILIndexTab, 3, this);
        }

        if (FILITab != null)
        {
            FTabType = ThumbStyle.Icon;
        }
        else
        {
            FTabType = ThumbStyle.Bar;
        }

        if (FILIInc != null && FILIDec != null)
        {
            FValid = true;
            if (FILTBack != null && (FILITab != null || FILLTab != null))
            {
                FScrollType = Style.Scroll;
            }
            else
            {
                FScrollType = Style.IncDec;
            }
        }
        else
        {
            if (FILLBack != null && (FILITab != null || FILLTab != null))
            {
                FValid = true;
                FScrollType = Style.Slider;
            }
        }

        base.Validate();
    }

    // AoWVScroll.pas:351
    // { Update repeats the action of a held mouse button once the button has been held for Delay
    //   updates: a button steps by SmallChange, the track pages by LargeChange up to the position that
    //   was clicked }
    public override void Update()
    {
        if (FVisible && F150 > 0)
        {
            if (F150 < FDelay)
            {
                F150 = (byte)(F150 + 1);
            }
            else
            {
                switch (F151)
                {
                    case 1:
                        ApplySmallStep(true);
                        break;
                    case 4:
                        ApplySmallStep(false);
                        break;
                    case 2:
                        FPosition = FPosition - FLargeChange;
                        if (F154 > FPosition)
                        {
                            FPosition = F154;
                            F150 = 0;
                        }

                        FDrawn = false;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        break;
                    case 3:
                        FPosition = FPosition + FLargeChange;
                        if (F154 + 1 < FPosition)
                        {
                            FPosition = F154 + 1;
                            F150 = 0;
                        }

                        FDrawn = false;
                        if (FOnChange != null)
                        {
                            FOnChange(this);
                        }

                        break;
                }
            }
        }

        base.Update();
    }

    // AoWVScroll.pas:194
    public virtual void SetILIUIndex(int Value)
    {
        if (Value != FILIndexIncUp)
        {
            FILIndexIncUp = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:203
    public virtual void SetILIDIndex(int Value)
    {
        if (Value != FILIndexIncDown)
        {
            FILIndexIncDown = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:212
    public virtual void SetILDUIndex(int Value)
    {
        if (Value != FILIndexDecUp)
        {
            FILIndexDecUp = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:221
    public virtual void SetILDDIndex(int Value)
    {
        if (Value != FILIndexDecDown)
        {
            FILIndexDecDown = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:230
    public virtual void SetILBIndex(int Value)
    {
        if (Value != FILIndexBack)
        {
            FILIndexBack = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:239
    public virtual void SetILTIndex(int Value)
    {
        if (Value != FILIndexTab)
        {
            FILIndexTab = Value;
            FValid = false;
        }
    }

    // AoWVScroll.pas:140
    public virtual void SetILInc(InterfaceImageLibrary Value)
    {
        if (Value != FILIInc)
        {
            FValid = false;
            FILIInc = Value;
        }
    }

    // AoWVScroll.pas:149
    public virtual void SetILDec(InterfaceImageLibrary Value)
    {
        if (Value != FILIDec)
        {
            FValid = false;
            FILIDec = Value;
        }
    }

    // AoWVScroll.pas:158
    public virtual void SetILBack(InterfaceImageLibrary Value)
    {
        if (Value != FILTBack)
        {
            FValid = false;
            FILTBack = Value;
        }
    }

    // AoWVScroll.pas:167
    public virtual void SetILLBack(InterfaceImageLibrary Value)
    {
        if (Value != FILLBack)
        {
            FValid = false;
            FILLBack = Value;
        }
    }

    // AoWVScroll.pas:176
    public virtual void SetILITab(InterfaceImageLibrary Value)
    {
        if (Value != FILITab)
        {
            FValid = false;
            FILITab = Value;
        }
    }

    // AoWVScroll.pas:185
    public virtual void SetILLTab(InterfaceImageLibrary Value)
    {
        if (Value != FILLTab)
        {
            FValid = false;
            FILLTab = Value;
        }
    }

    // AoWVScroll.pas:289
    public void SetSmall(int Value)
    {
        if (Value < 1)
        {
            Value = 1;
        }

        FSmallChange = Value;
    }

    // AoWVScroll.pas:296
    public void SetLarge(int Value)
    {
        if (Value < 1)
        {
            Value = 1;
        }

        FLargeChange = Value;
    }

    // AoWVScroll.pas:303
    public void SetFMin(int Value)
    {
        if (Value > FMax)
        {
            Value = FMax;
        }

        if (Value != FMin)
        {
            FMin = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:314
    public void SetFMax(int Value)
    {
        if (Value < FMin)
        {
            Value = FMin;
        }

        if (Value != FMax)
        {
            FMax = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:325
    public void SetFNum(int Value)
    {
        if (Value > FMax - FMin + 1)
        {
            Value = FMax - FMin + 1;
        }

        if (Value < 1)
        {
            Value = 1;
        }

        if (Value != FNumberVisible)
        {
            FNumberVisible = Value;
            FSized = false;
        }
    }

    // AoWVScroll.pas:338
    public void SetFPos(int Value)
    {
        FPosition = Value;
        if (FPosition + FNumberVisible > FMax - FMin + 1)
        {
            FPosition = FMax - FMin - FNumberVisible + 1;
        }

        if (FPosition < 0)
        {
            FPosition = 0;
        }

        FDrawn = false;
    }

    [Published]
    [DefaultValue(20)]
    public byte Delay { get => FDelay; set => FDelay = value; }

    [Published]
    public int Min { get => FMin; set => SetFMin(value); }

    [Published]
    public int Max { get => FMax; set => SetFMax(value); }

    [Published]
    public int SmallChange { get => FSmallChange; set => SetSmall(value); }

    [Published]
    public int LargeChange { get => FLargeChange; set => SetLarge(value); }

    [Published]
    public int Position { get => FPosition; set => SetFPos(value); }

    [Published]
    public int NumberVisible { get => FNumberVisible; set => SetFNum(value); }

    [Published]
    public bool VanishWhenFull { get => FVanishWhenFull; set => FVanishWhenFull = value; }

    [Published]
    public InterfaceImageLibrary ILIInc { get => FILIInc; set => SetILInc(value); }

    [Published]
    public InterfaceImageLibrary ILIDec { get => FILIDec; set => SetILDec(value); }

    [Published]
    public InterfaceImageLibrary ILTBack { get => FILTBack; set => SetILBack(value); }

    [Published]
    public InterfaceImageLibrary ILLBack { get => FILLBack; set => SetILLBack(value); }

    [Published]
    public InterfaceImageLibrary ILITab { get => FILITab; set => SetILITab(value); }

    [Published]
    public InterfaceImageLibrary ILLTab { get => FILLTab; set => SetILLTab(value); }

    [Published]
    public int ILIndexIncUp { get => FILIndexIncUp; set => SetILIUIndex(value); }

    [Published]
    public int ILIndexIncDown { get => FILIndexIncDown; set => SetILIDIndex(value); }

    [Published]
    public int ILIndexDecUp { get => FILIndexDecUp; set => SetILDUIndex(value); }

    [Published]
    public int ILIndexDecDown { get => FILIndexDecDown; set => SetILDDIndex(value); }

    [Published]
    public int ILIndexBack { get => FILIndexBack; set => SetILBIndex(value); }

    [Published]
    public int ILIndexTab { get => FILIndexTab; set => SetILTIndex(value); }

    public new NotifyEvent<ScrollBar> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<ScrollBar> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<ScrollBar> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<ScrollBar> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<ScrollBar> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<ScrollBar> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<ScrollBar> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<ScrollBar> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<ScrollBar> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }

    public enum Style : byte
    {
        Scroll = 0, Slider = 1, IncDec = 2
    }

    public enum ThumbStyle : byte
    {
        Icon = 0, Bar = 1
    }
}
