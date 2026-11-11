// Translated from AoWComp.pas.
using System.ComponentModel;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: FColor, FSelColor, FValid (Validate found everything it needs), FAligned, FSized,
// FCursor, FOnDraw, FOnPreDraw, FOnMouseDown, FOnMouseUp, FOnMouseMove, FOnKeyPress,
// FOnKeyDown, FOnChange, FMouseDown, FVisible, FHandled (a Check* method consumed the input),
// FWinWidth, FWinHeight, FWinLeft, FWinTop, FAOWFont, FText, FRedraw, FBorderWidth,
// FBorderHeight, FPriority, FDrawn (the component is drawn as it is now), FFirstDraw, FMouseIn,
// FToolTipCount, FRadioIndex, FChecked, FOwnerItem, FOnUpdate, FCaptureInput, FOnDebugMessage,
// FAOWOwner, FChildren (sorted by FPriority), FAlignment, FMasked, FBltList, FDrawList
// (DirtyRectangle), FBltCount, FDrawCount, FToolTipLeft, FOldRect, FHidden, FDoubleBuffered,
// FSurfaceValid
public partial class UIComponent : Rtl.Component
{
    public int FColor;
    public int FSelColor;
    public bool FValid;
    public bool FAligned;
    public bool FSized;
    public int FCursor;
    public AoWOnDrawEvent<UIComponent> FOnDraw;
    public AoWOnDrawEvent<UIComponent> FOnPreDraw;
    public MouseEvent<UIComponent> FOnMouseDown;
    public MouseEvent<UIComponent> FOnMouseUp;
    public MouseMoveEvent<UIComponent> FOnMouseMove;
    public KeyPressEvent<UIComponent> FOnKeyPress;
    public KeyEvent<UIComponent> FOnKeyDown;
    public NotifyEvent<UIComponent> FOnChange;
    public bool FMouseDown;
    public bool FVisible;
    public bool FHandled;
    public int FWinWidth;
    public int FWinHeight;
    public int FWinLeft;
    public int FWinTop;
    public ImageLibraryFont FAOWFont;
    public string FText = "";
    public bool FRedraw;
    public int FBorderWidth;
    public int FBorderHeight;
    public int FPriority;
    public bool FDrawn;
    public bool FFirstDraw;
    public bool FMouseIn;
    public int FToolTipCount;
    public int FRadioIndex;
    public bool FChecked;
    public int FOwnerItem;
    public NotifyEvent<UIComponent> FOnUpdate;
    public bool FCaptureInput;
    public AoWOnDebugEvent FOnDebugMessage;
    public UIComponent FAOWOwner;
    public DelphiList FChildren;
    public Alignment FAlignment;
    public bool FMasked;
    public List<DirtyRectangle> FBltList;
    public List<DirtyRectangle> FDrawList;
    public int FBltCount;
    public int FDrawCount;
    public int FToolTipLeft;
    public Rect FOldRect;
    public bool FHidden;
    public bool FDoubleBuffered;
    public bool FSurfaceValid;

    // AoWComp.pas:222
    public UIComponent(Rtl.Component owner) : base(owner)
    {
        FVisible = true;
        FFirstDraw = true;
        FAlignment = new Alignment();
        FChildren = new DelphiList();
        FBltList = new List<DirtyRectangle>();
        FDrawList = new List<DirtyRectangle>();
    }

    // AoWComp.pas:233
    public override void Dispose()
    {
        SetOwner(null);
        while (FChildren.Count > 0)
        {
            ((UIComponent)FChildren[0]).SetOwner(null);
        }

        FChildren?.Dispose();
        FAlignment?.Dispose();
        for (int I = 0, last = FDrawList.Count - 1; I <= last; I++)
        {
        }

        for (int I = 0, last = FBltList.Count - 1; I <= last; I++)
        {
        }

        FBltList?.Clear();
        FDrawList?.Clear();
        base.Dispose();
    }

    // AoWComp.pas:251
    public virtual void OnChangeText(object Sender)
    {
        FSized = false;
    }

    // AoWComp.pas:300
    public virtual void Validate()
    {
        FSized = false;
        FAligned = false;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((UIComponent)FChildren[I]).FValid = false;
        }
    }

    // AoWComp.pas:678
    public virtual void SetSize()
    {
        FSized = true;
        FDrawn = false;
        FBltCount = 0;
        FDrawCount = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((UIComponent)FChildren[I]).FAligned = false;
        }
    }

    // AoWComp.pas:690
    public virtual void ReAlign()
    {
        if (FAOWOwner != null && (FAlignment.FAlignWidth != AlignWidth.None || FAlignment.FAlignHeight != AlignHeight.None))
        {
            switch (this.FAlignment.FAlignWidth)
            {
                case AlignWidth.Left:
                    FWinLeft = FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent / 100 + this.FAlignment.FLeftOffset;
                    if (FAlignment.FWidthPercent > 0)
                    {
                        FWinWidth = FAOWOwner.FWinWidth * FAlignment.FWidthPercent / 100;
                    }

                    break;
                case AlignWidth.Right:
                    if (this.FAlignment.FWidthPercent > 0)
                    {
                        FWinWidth = FAOWOwner.FWinWidth * this.FAlignment.FWidthPercent / 100;
                    }

                    FWinLeft = FAOWOwner.FWinWidth - FAlignment.FRightOffset - FWinWidth - FAOWOwner.FWinWidth * FAlignment.FRightOffPercent / 100;
                    break;
                case AlignWidth.Both:
                    FWinLeft = FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent / 100 + this.FAlignment.FLeftOffset;
                    FWinWidth = FAOWOwner.FWinWidth - FWinLeft - FAlignment.FRightOffset - FAOWOwner.FWinWidth * FAlignment.FRightOffPercent / 100;
                    break;
                case AlignWidth.Center:
                    FWinLeft = FAOWOwner.FWinWidth / 2 - FWinWidth / 2;
                    break;
            }

            switch (this.FAlignment.FAlignHeight)
            {
                case AlignHeight.Top:
                    FWinTop = FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent / 100 + this.FAlignment.FTopOffset;
                    if (FAlignment.FHeightPercent > 0)
                    {
                        FWinHeight = FAOWOwner.FWinHeight * FAlignment.FHeightPercent / 100;
                    }

                    break;
                case AlignHeight.Bottom:
                    if (this.FAlignment.FHeightPercent > 0)
                    {
                        FWinHeight = FAOWOwner.FWinHeight * this.FAlignment.FHeightPercent / 100;
                    }

                    FWinTop = FAOWOwner.FWinHeight - FAlignment.FBottomOffset - FWinHeight - FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent / 100;
                    break;
                case AlignHeight.Both:
                    FWinTop = FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent / 100 + this.FAlignment.FTopOffset;
                    FWinHeight = FAOWOwner.FWinHeight - FWinTop - FAlignment.FBottomOffset - FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent / 100;
                    break;
                case AlignHeight.Center:
                    FWinTop = FAOWOwner.FWinHeight / 2 - FWinHeight / 2;
                    break;
            }
        }

        FAligned = true;
        FSized = false;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((UIComponent)FChildren[I]).FAligned = false;
        }
    }

    // AoWComp.pas:753
    // { Setup brings the component up to date: Validate, SetSize and ReAlign when their flag is clear,
    //   then Setup of every visible child; an exception in any step is reported in a message box }
    public virtual void Setup()
    {
        if (FVisible)
        {
            try
            {
                if (!FValid)
                {
                    Validate();
                }
            }
            catch
            {
                MessageBox(0, "Exception during Validate", "Error - " + Name, 0);
            }

            if (FValid)
            {
                try
                {
                    if (!FSized)
                    {
                        SetSize();
                    }
                }
                catch
                {
                    MessageBox(0, "Exception during SetSize", "Error - " + Name, 0);
                }

                try
                {
                    if (!FAligned)
                    {
                        ReAlign();
                    }
                }
                catch
                {
                    MessageBox(0, "Exception during ReAlign", "Error - " + Name, 0);
                }

                try
                {
                    if (!FSized)
                    {
                        SetSize();
                    }
                }
                catch
                {
                    MessageBox(0, "Exception during SetSize", "Error - " + Name, 0);
                }

                try
                {
                    for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
                    {
                        if (((UIComponent)FChildren[I]).FVisible)
                        {
                            ((UIComponent)FChildren[I]).Setup();
                        }
                    }
                }
                catch
                {
                    MessageBox(0, "Exception during Setup", "Error - " + Name, 0);
                }
            }
        }
    }

    // AoWComp.pas:796
    public virtual void Update()
    {
        try
        {
            if (FValid && FVisible)
            {
                if (FOnUpdate != null)
                {
                    FOnUpdate(this);
                }

                for (int I = FChildren.Count - 1; I >= 0; I--)
                {
                    ((UIComponent)FChildren[I]).Update();
                }

                if (!FValid || !FAligned || !FSized)
                {
                    Setup();
                }
            }
        }
        catch
        {
            MessageBox(0, "Exception during Update", "Error - " + Name, 0);
        }
    }

    // AoWComp.pas:202
    public virtual void ClearDrawList()
    {
        FDrawCount = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (((UIComponent)FChildren[I]).FVisible)
            {
                ((UIComponent)FChildren[I]).ClearDrawList();
            }
        }
    }

    // AoWComp.pas:212
    public virtual void ClearBltList()
    {
        FBltCount = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (((UIComponent)FChildren[I]).FVisible)
            {
                ((UIComponent)FChildren[I]).ClearBltList();
            }
        }
    }

    // AoWComp.pas:576
    public virtual void InvalidateRect(int Index, ref Rect ARect, UIComponent Sender)
    {
    }

    // AoWComp.pas:314
    // { AddBltRect adds ARect to the rectangles to blit, clipped against the rectangles already in the
    //   list so that no two overlap: the parts of ARect that another rectangle covers are cut off (a
    //   cut that leaves two pieces adds one of them as a new rectangle), and an old rectangle that ARect
    //   covers on one side is trimmed or emptied. }
    public virtual void AddBltRect(ref Rect ARect, UIComponent Sender)
    {
        DirtyRectangle N;
        int I;
        int Count;
        bool Add;
        DirtyRectangle R;
        if (FValid && FVisible)
        {
            I = 0;
            Count = FBltCount;
            Add = true;
            if (ARect.Left < 0)
            {
                ARect.Left = 0;
            }

            if (ARect.Top < 0)
            {
                ARect.Top = 0;
            }

            if (ARect.Right < ARect.Left || ARect.Bottom < ARect.Top)
            {
                Add = false;
            }
            else
            {
                while (I < Count && Add)
                {
                    R = FBltList[I];
                    if (ARect.Right >= R.R.Left && ARect.Left <= R.R.Right && ARect.Bottom >= R.R.Top && ARect.Top <= R.R.Bottom)
                    {
                        if (R.R.Left <= ARect.Left)
                        {
                            if (ARect.Right <= R.R.Right)
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        Add = false;
                                    }
                                    else
                                    {
                                        ARect.Top = R.R.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Bottom = R.R.Top - 1;
                                    }
                                    else
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                        FBltCount += 1;
                                        ARect.Top = R.R.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
                                        FBltCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
                                        FBltCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        R.R.Right = ARect.Left - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ARect.Right <= R.R.Right)
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Right = R.R.Left - 1;
                                    }
                                    else
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
                                        FBltCount += 1;
                                        ARect.Right = R.R.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                        FBltCount += 1;
                                        ARect.Right = R.R.Left - 1;
                                    }
                                    else
                                    {
                                        R.R.Left = ARect.Right + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FBltCount == FBltList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FBltList.Add(taowRect);
                                        }

                                        N = FBltList[FBltCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
                                        FBltCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        R.R.Bottom = ARect.Top - 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        R.R.Top = ARect.Bottom + 1;
                                    }
                                    else
                                    {
                                        R.R = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    I += 1;
                }
            }

            if (Add)
            {
                if (FBltCount == FBltList.Count)
                {
                    var taowRect = new DirtyRectangle();
                    FBltList.Add(taowRect);
                }

                R = FBltList[FBltCount];
                R.R = ARect;
                FBltCount += 1;
            }
        }
    }

    // AoWComp.pas:445
    // { AddDrawRect: the same for the rectangles to redraw; the rectangle added last records Sender }
    public virtual void AddDrawRect(ref Rect ARect, UIComponent Sender)
    {
        DirtyRectangle N;
        int I;
        int Count;
        bool Add;
        DirtyRectangle R;
        if (FValid && FVisible)
        {
            I = 0;
            Count = FDrawCount;
            Add = true;
            if (ARect.Left < 0)
            {
                ARect.Left = 0;
            }

            if (ARect.Top < 0)
            {
                ARect.Top = 0;
            }

            if (ARect.Right < ARect.Left || ARect.Bottom < ARect.Top)
            {
                Add = false;
            }
            else
            {
                while (I < Count && Add)
                {
                    R = FDrawList[I];
                    if (ARect.Right >= R.R.Left && ARect.Left <= R.R.Right && ARect.Bottom >= R.R.Top && ARect.Top <= R.R.Bottom)
                    {
                        if (R.R.Left <= ARect.Left)
                        {
                            if (ARect.Right <= R.R.Right)
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        Add = false;
                                    }
                                    else
                                    {
                                        ARect.Top = R.R.Bottom + 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Bottom = R.R.Top - 1;
                                    }
                                    else
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                        FDrawCount += 1;
                                        ARect.Top = R.R.Bottom + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
                                        FDrawCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
                                        FDrawCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        R.R.Right = ARect.Left - 1;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (ARect.Right <= R.R.Right)
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        ARect.Right = R.R.Left - 1;
                                    }
                                    else
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
                                        FDrawCount += 1;
                                        ARect.Right = R.R.Left - 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
                                        FDrawCount += 1;
                                        ARect.Right = R.R.Left - 1;
                                    }
                                    else
                                    {
                                        R.R.Left = ARect.Right + 1;
                                    }
                                }
                            }
                            else
                            {
                                if (ARect.Top >= R.R.Top)
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        if (FDrawCount == FDrawList.Count)
                                        {
                                            var taowRect = new DirtyRectangle();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = FDrawList[FDrawCount];
                                        N.R = new Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
                                        FDrawCount += 1;
                                        ARect.Left = R.R.Right + 1;
                                    }
                                    else
                                    {
                                        R.R.Bottom = ARect.Top - 1;
                                    }
                                }
                                else
                                {
                                    if (ARect.Bottom <= R.R.Bottom)
                                    {
                                        R.R.Top = ARect.Bottom + 1;
                                    }
                                    else
                                    {
                                        R.R = new Rect(0, 0, 0, 0);
                                    }
                                }
                            }
                        }
                    }

                    I += 1;
                }
            }

            if (Add)
            {
                if (FDrawCount == FDrawList.Count)
                {
                    var taowRect = new DirtyRectangle();
                    FDrawList.Add(taowRect);
                }

                R = FDrawList[FDrawCount];
                R.R = ARect;
                R.Owner = Sender;
                FDrawCount += 1;
            }
        }
    }

    // AoWComp.pas:262
    public virtual void SetOwner(UIComponent Value)
    {
        int I;
        int NewIndex;
        int OldIndex;
        if (FAOWOwner != null)
        {
            OldIndex = FAOWOwner.FChildren.IndexOf(this);
            FAOWOwner.FChildren.Delete(OldIndex);
        }
        else
        {
            OldIndex = -2;
        }

        if (Value != FAOWOwner)
        {
            FValid = false;
        }

        FAOWOwner = Value;
        NewIndex = -1;
        if (FAOWOwner != null)
        {
            I = 0;
            while (I < FAOWOwner.FChildren.Count)
            {
                if (((UIComponent)FAOWOwner.FChildren[I]).FPriority > FPriority)
                {
                    NewIndex = I;
                    I = FAOWOwner.FChildren.Count;
                }
                else
                {
                    I += 1;
                }
            }

            if (NewIndex == -1)
            {
                FAOWOwner.FChildren.Add(this);
            }
            else
            {
                FAOWOwner.FChildren.Insert(NewIndex, this);
            }
        }

        if (NewIndex != OldIndex)
        {
            if (FDoubleBuffered)
            {
                FSurfaceValid = false;
            }
            else
            {
                FDrawn = false;
            }
        }
    }

    // AoWComp.pas:580
    public virtual void SetWinLeft(int Value)
    {
        FWinLeft = Value;
        FDrawn = false;
    }

    // AoWComp.pas:586
    public virtual void SetWinTop(int Value)
    {
        FWinTop = Value;
        FDrawn = false;
    }

    // AoWComp.pas:592
    public virtual void SetWidth(int Value)
    {
        FWinWidth = Value;
        FSized = false;
    }

    // AoWComp.pas:598
    public virtual void SetHeight(int Value)
    {
        FWinHeight = Value;
        FSized = false;
    }

    // AoWComp.pas:604
    public virtual void SetPriority(int Value)
    {
        FPriority = Value;
        SetOwner(FAOWOwner);
    }

    // AoWComp.pas:610
    public virtual void SetVisible(bool Value)
    {
        if (Value != FVisible)
        {
            FVisible = Value;
            FDrawn = false;
            if (FVisible)
            {
                FHidden = false;
                ClearBltList();
                ClearDrawList();
                if (!FValid || !FAligned || !FSized)
                {
                    Setup();
                }
            }
            else
            {
                FHidden = true;
            }
        }
    }

    // AoWComp.pas:629
    public virtual void SetColor(int Value)
    {
        FColor = Value;
        FDrawn = false;
    }

    // AoWComp.pas:635
    public virtual void SetSelColor(int Value)
    {
        FSelColor = Value;
        FDrawn = false;
    }

    // AoWComp.pas:641
    public virtual void DrawSurface(Surface Surface)
    {
    }

    // AoWComp.pas:645
    public virtual void DrawSurfDesc(LockedSurface SurfDesc)
    {
    }

    // AoWComp.pas:649
    public virtual void Draw(LockedSurface SurfDesc)
    {
    }

    // AoWComp.pas:653
    public virtual void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:658
    public virtual void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:663
    public virtual void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:668
    public virtual void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWComp.pas:673
    public virtual void CheckKeyDown(ref ushort Key, Set<ShiftState> Shift)
    {
        FHandled = false;
    }

    // AoWComp.pas:256
    public void SetAlign(Alignment Value)
    {
        FAlignment.Assign(Value);
        FAligned = false;
    }

    public int Priority { get => FPriority; set => SetPriority(value); }
    public int WinLeft { get => FWinLeft; set => SetWinLeft(value); }
    public int WinTop { get => FWinTop; set => SetWinTop(value); }
    public int WinWidth { get => FWinWidth; set => SetWidth(value); }
    public int WinHeight { get => FWinHeight; set => SetHeight(value); }
    public int TextColor { get => FColor; set => SetColor(value); }
    public int TextSelColor { get => FSelColor; set => SetSelColor(value); }
    public UIComponent AOWOwner { get => FAOWOwner; set => SetOwner(value); }
    public Alignment Alignment { get => FAlignment; set => SetAlign(value); }
    public bool Masked { get => FMasked; set => FMasked = value; }
    public bool CaptureInput { get => FCaptureInput; set => FCaptureInput = value; }
    public NotifyEvent<UIComponent> OnChange { get => FOnChange; set => FOnChange = value; }

    [Published]
    public MouseEvent<UIComponent> OnMouseDown { get => FOnMouseDown; set => FOnMouseDown = value; }

    [Published]
    public MouseEvent<UIComponent> OnMouseUp { get => FOnMouseUp; set => FOnMouseUp = value; }

    [Published]
    public MouseMoveEvent<UIComponent> OnMouseMove { get => FOnMouseMove; set => FOnMouseMove = value; }

    [Published]
    public KeyPressEvent<UIComponent> OnKeyPress { get => FOnKeyPress; set => FOnKeyPress = value; }

    [Published]
    public KeyEvent<UIComponent> OnKeyDown { get => FOnKeyDown; set => FOnKeyDown = value; }

    [Published]
    public AoWOnDrawEvent<UIComponent> OnPreDraw { get => FOnPreDraw; set => FOnPreDraw = value; }

    [Published]
    public AoWOnDrawEvent<UIComponent> OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public NotifyEvent<UIComponent> OnUpdate { get => FOnUpdate; set => FOnUpdate = value; }

    [Published]
    [DefaultValue(true)]
    public bool Visible { get => FVisible; set => SetVisible(value); }

    [Published]
    public AoWOnDebugEvent OnDebugMessage { get => FOnDebugMessage; set => FOnDebugMessage = value; }

    public enum ButtonType : byte
    {
        Text = 0, Icon = 1
    }

    public delegate void AoWOnDebugEvent(string Dtext);
}
