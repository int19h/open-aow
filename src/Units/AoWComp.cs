// Translated from AoWComp.pas.
using Aow.Rtl;
using static Aow.Rtl.Rtl;
using Aow.Units.GFXE;
using Aow.Units.ImageLib;
using System.ComponentModel;

namespace Aow.Units.AoWComp;

public enum TButtonType : byte
{
    btText = 0,
    btIcon = 1
}

public enum TaowTextAlign : byte
{
    taLeft = 0,
    taCenter = 1,
    taRight = 2
}

public enum TAOWAlignHeight : byte
{
    ahNone = 0,
    ahTop = 1,
    ahBottom = 2,
    ahBoth = 3,
    ahCenter = 4
}

public enum TAOWAlignWidth : byte
{
    awNone = 0,
    awLeft = 1,
    awRight = 2,
    awBoth = 3,
    awCenter = 4
}

public enum TAOWDrawMethod : byte
{
    dmBlank = 0,
    dmBorderless = 1,
    dmHollowBox = 2,
    dmFilledBox = 3
}

// Fields: FColor, FSelColor, FValid (Validate found everything it needs), FAligned, FSized,
// FCursor, FOnDraw, FOnPreDraw, FOnMouseDown, FOnMouseUp, FOnMouseMove, FOnKeyPress,
// FOnKeyDown, FOnChange, FMouseDown, FVisible, FHandled (a Check* method consumed the input),
// FWinWidth, FWinHeight, FWinLeft, FWinTop, FAOWFont, FText, FRedraw, FBorderWidth,
// FBorderHeight, FPriority, FDrawn (the component is drawn as it is now), FFirstDraw, FMouseIn,
// FToolTipCount, FRadioIndex, FChecked, FOwnerItem, FOnUpdate, FCaptureInput, FOnDebugMessage,
// FAOWOwner, FChildren (sorted by FPriority), FAlignment, FMasked, FBltList, FDrawList
// (TaowRect), FBltCount, FDrawCount, FToolTipLeft, FOldRect, FHidden, FDoubleBuffered,
// FSurfaceValid
public partial class TAoWComponent : TComponent
{
    public int FColor;
    public int FSelColor;
    public bool FValid;
    public bool FAligned;
    public bool FSized;
    public int FCursor;
    public TAoWOnDrawEvent FOnDraw;
    public TAoWOnDrawEvent FOnPreDraw;
    public TMouseEvent FOnMouseDown;
    public TMouseEvent FOnMouseUp;
    public TMouseMoveEvent FOnMouseMove;
    public TKeyPressEvent FOnKeyPress;
    public TKeyEvent FOnKeyDown;
    public TNotifyEvent FOnChange;
    public bool FMouseDown;
    public bool FVisible;
    public bool FHandled;
    public int FWinWidth;
    public int FWinHeight;
    public int FWinLeft;
    public int FWinTop;
    public TImageLibraryFont FAOWFont;
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
    public TNotifyEvent FOnUpdate;
    public bool FCaptureInput;
    public TAoWOnDebugEvent FOnDebugMessage;
    public TAoWComponent FAOWOwner;
    public TList FChildren;
    public TAOWAlignment FAlignment;
    public bool FMasked;
    public TList FBltList;
    public TList FDrawList;
    public int FBltCount;
    public int FDrawCount;
    public int FToolTipLeft;
    public TRect FOldRect;
    public bool FHidden;
    public bool FDoubleBuffered;
    public bool FSurfaceValid;
    // AoWComp.pas:222
    public TAoWComponent(TComponent owner) : base(owner)
    {
        FVisible = true;
        FFirstDraw = true;
        FAlignment = new TAOWAlignment();
        FChildren = new TList();
        FBltList = new TList();
        FDrawList = new TList();
    }

    // AoWComp.pas:233
    public override void Destroy()
    {
        SetOwner(null);
        while (FChildren.Count > 0)
        {
            ((TAoWComponent)FChildren[0]).SetOwner(null);
        }

        FChildren.Free();
        FAlignment.Free();
        for (int I = 0, last = FDrawList.Count - 1; I <= last; I++)
        {
            ((TaowRect)FDrawList[I]).Free();
        }

        for (int I = 0, last = FBltList.Count - 1; I <= last; I++)
        {
            ((TaowRect)FBltList[I]).Free();
        }

        FBltList.Free();
        FDrawList.Free();
        base.Destroy();
    }

    // AoWComp.pas:251
    public virtual void OnChangeText(TObject Sender)
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
            ((TAoWComponent)FChildren[I]).FValid = false;
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
            ((TAoWComponent)FChildren[I]).FAligned = false;
        }
    }

    // AoWComp.pas:690
    public virtual void ReAlign()
    {
        if (FAOWOwner != null && (FAlignment.FAlignWidth != TAOWAlignWidth.awNone || FAlignment.FAlignHeight != TAOWAlignHeight.ahNone))
        {
            switch (this.FAlignment.FAlignWidth)
            {
                case TAOWAlignWidth.awLeft:
                    FWinLeft = FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent / 100 + this.FAlignment.FLeftOffset;
                    if (FAlignment.FWidthPercent > 0)
                    {
                        FWinWidth = FAOWOwner.FWinWidth * FAlignment.FWidthPercent / 100;
                    }

                    break;
                case TAOWAlignWidth.awRight:
                    if (this.FAlignment.FWidthPercent > 0)
                    {
                        FWinWidth = FAOWOwner.FWinWidth * this.FAlignment.FWidthPercent / 100;
                    }

                    FWinLeft = FAOWOwner.FWinWidth - FAlignment.FRightOffset - FWinWidth - FAOWOwner.FWinWidth * FAlignment.FRightOffPercent / 100;
                    break;
                case TAOWAlignWidth.awBoth:
                    FWinLeft = FAOWOwner.FWinWidth * this.FAlignment.FLeftOffPercent / 100 + this.FAlignment.FLeftOffset;
                    FWinWidth = FAOWOwner.FWinWidth - FWinLeft - FAlignment.FRightOffset - FAOWOwner.FWinWidth * FAlignment.FRightOffPercent / 100;
                    break;
                case TAOWAlignWidth.awCenter:
                    FWinLeft = FAOWOwner.FWinWidth / 2 - FWinWidth / 2;
                    break;
            }

            switch (this.FAlignment.FAlignHeight)
            {
                case TAOWAlignHeight.ahTop:
                    FWinTop = FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent / 100 + this.FAlignment.FTopOffset;
                    if (FAlignment.FHeightPercent > 0)
                    {
                        FWinHeight = FAOWOwner.FWinHeight * FAlignment.FHeightPercent / 100;
                    }

                    break;
                case TAOWAlignHeight.ahBottom:
                    if (this.FAlignment.FHeightPercent > 0)
                    {
                        FWinHeight = FAOWOwner.FWinHeight * this.FAlignment.FHeightPercent / 100;
                    }

                    FWinTop = FAOWOwner.FWinHeight - FAlignment.FBottomOffset - FWinHeight - FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent / 100;
                    break;
                case TAOWAlignHeight.ahBoth:
                    FWinTop = FAOWOwner.FWinHeight * this.FAlignment.FTopOffPercent / 100 + this.FAlignment.FTopOffset;
                    FWinHeight = FAOWOwner.FWinHeight - FWinTop - FAlignment.FBottomOffset - FAOWOwner.FWinHeight * FAlignment.FBottomOffPercent / 100;
                    break;
                case TAOWAlignHeight.ahCenter:
                    FWinTop = FAOWOwner.FWinHeight / 2 - FWinHeight / 2;
                    break;
            }
        }

        FAligned = true;
        FSized = false;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            ((TAoWComponent)FChildren[I]).FAligned = false;
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
                        if (((TAoWComponent)FChildren[I]).FVisible)
                        {
                            ((TAoWComponent)FChildren[I]).Setup();
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
                    ((TAoWComponent)FChildren[I]).Update();
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
            if (((TAoWComponent)FChildren[I]).FVisible)
            {
                ((TAoWComponent)FChildren[I]).ClearDrawList();
            }
        }
    }

    // AoWComp.pas:212
    public virtual void ClearBltList()
    {
        FBltCount = 0;
        for (int I = 0, last = FChildren.Count - 1; I <= last; I++)
        {
            if (((TAoWComponent)FChildren[I]).FVisible)
            {
                ((TAoWComponent)FChildren[I]).ClearBltList();
            }
        }
    }

    // AoWComp.pas:576
    public virtual void InvalidateRect(int Index, ref TRect ARect, TAoWComponent Sender)
    {
    }

    // AoWComp.pas:314
    // { AddBltRect adds ARect to the rectangles to blit, clipped against the rectangles already in the
//   list so that no two overlap: the parts of ARect that another rectangle covers are cut off (a
//   cut that leaves two pieces adds one of them as a new rectangle), and an old rectangle that ARect
//   covers on one side is trimmed or emptied. }
    public virtual void AddBltRect(ref TRect ARect, TAoWComponent Sender)
    {
        TaowRect N;
        int I;
        int Count;
        bool Add;
        TaowRect R;
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
                    R = (TaowRect)FBltList[I];
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FBltList.Add(taowRect);
                                        }

                                        N = (TaowRect)FBltList[FBltCount];
                                        N.R = Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
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
                                        R.R = Rect(0, 0, 0, 0);
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
                    var taowRect = new TaowRect();
                    FBltList.Add(taowRect);
                }

                R = (TaowRect)FBltList[FBltCount];
                R.R = ARect;
                FBltCount += 1;
            }
        }
    }

    // AoWComp.pas:445
    // { AddDrawRect: the same for the rectangles to redraw; the rectangle added last records Sender }
    public virtual void AddDrawRect(ref TRect ARect, TAoWComponent Sender)
    {
        TaowRect N;
        int I;
        int Count;
        bool Add;
        TaowRect R;
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
                    R = (TaowRect)FDrawList[I];
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(ARect.Left, ARect.Top, ARect.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(ARect.Left, R.R.Bottom + 1, R.R.Right, ARect.Bottom);
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(ARect.Left, ARect.Top, R.R.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(R.R.Left, R.R.Bottom + 1, ARect.Right, ARect.Bottom);
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(R.R.Left, ARect.Top, ARect.Right, R.R.Top - 1);
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
                                            var taowRect = new TaowRect();
                                            FDrawList.Add(taowRect);
                                        }

                                        N = (TaowRect)FDrawList[FDrawCount];
                                        N.R = Rect(ARect.Left, ARect.Top, R.R.Left - 1, ARect.Bottom);
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
                                        R.R = Rect(0, 0, 0, 0);
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
                    var taowRect = new TaowRect();
                    FDrawList.Add(taowRect);
                }

                R = (TaowRect)FDrawList[FDrawCount];
                R.R = ARect;
                R.Owner = Sender;
                FDrawCount += 1;
            }
        }
    }

    // AoWComp.pas:262
    public virtual void SetOwner(TAoWComponent Value)
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
                if (((TAoWComponent)FAOWOwner.FChildren[I]).FPriority > FPriority)
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
    public virtual void DrawSurface(TSurface Surface)
    {
    }

    // AoWComp.pas:645
    public virtual void DrawSurfDesc(TSurfaceDesc SurfDesc)
    {
    }

    // AoWComp.pas:649
    public virtual void Draw(TSurfaceDesc SurfDesc)
    {
    }

    // AoWComp.pas:653
    public virtual void CheckMouseDown(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:658
    public virtual void CheckMouseUp(TMouseButton Button, TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:663
    public virtual void CheckMouseMove(TSet<TShiftState> Shift, int X, int Y)
    {
        FHandled = false;
    }

    // AoWComp.pas:668
    public virtual void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWComp.pas:673
    public virtual void CheckKeyDown(ref ushort Key, TSet<TShiftState> Shift)
    {
        FHandled = false;
    }

    // AoWComp.pas:256
    public void SetAlign(TAOWAlignment Value)
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
    public TAoWComponent AOWOwner { get => FAOWOwner; set => SetOwner(value); }
    public TAOWAlignment Alignment { get => FAlignment; set => SetAlign(value); }
    public bool Masked { get => FMasked; set => FMasked = value; }
    public bool CaptureInput { get => FCaptureInput; set => FCaptureInput = value; }
    public TNotifyEvent OnChange { get => FOnChange; set => FOnChange = value; }

    [Published]
    public TMouseEvent OnMouseDown { get => FOnMouseDown; set => FOnMouseDown = value; }

    [Published]
    public TMouseEvent OnMouseUp { get => FOnMouseUp; set => FOnMouseUp = value; }

    [Published]
    public TMouseMoveEvent OnMouseMove { get => FOnMouseMove; set => FOnMouseMove = value; }

    [Published]
    public TKeyPressEvent OnKeyPress { get => FOnKeyPress; set => FOnKeyPress = value; }

    [Published]
    public TKeyEvent OnKeyDown { get => FOnKeyDown; set => FOnKeyDown = value; }

    [Published]
    public TAoWOnDrawEvent OnPreDraw { get => FOnPreDraw; set => FOnPreDraw = value; }

    [Published]
    public TAoWOnDrawEvent OnDraw { get => FOnDraw; set => FOnDraw = value; }

    [Published]
    public TNotifyEvent OnUpdate { get => FOnUpdate; set => FOnUpdate = value; }

    [Published]
    [DefaultValue(true)]
    public bool Visible { get => FVisible; set => SetVisible(value); }

    [Published]
    public TAoWOnDebugEvent OnDebugMessage { get => FOnDebugMessage; set => FOnDebugMessage = value; }
}

public delegate void TAoWOnDrawEvent(TAoWComponent Sender, TSurfaceDesc SurfDesc);
public delegate void TAoWOnDrawBackEvent(TAoWComponent Sender, TSurfaceDesc SurfDesc);
public delegate void TAoWOnDrawItemEvent(TAoWComponent Sender, in TRect DrawRect, int DrawItem, ref TSurfaceDesc SurfDesc);
public delegate void TAoWOnDebugEvent(string Dtext);
// how a component sizes and places itself inside its owner (ReAlign). FWidthPercent,
// FHeightPercent, FLeftOffPercent, FRightOffPercent, FTopOffPercent, FBottomOffPercent,
// FLeftOffset, FRightOffset, FTopOffset, FBottomOffset, FAlignWidth, FAlignHeight. It does not
// override Assign, so TAoWComponent.SetAlign raises EConvertError (original bug)
public partial class TAOWAlignment : TPersistent
{
    internal byte FWidthPercent;
    internal byte FHeightPercent;
    internal int FLeftOffPercent;
    internal int FRightOffPercent;
    internal int FTopOffPercent;
    internal int FBottomOffPercent;
    internal int FLeftOffset;
    internal int FRightOffset;
    internal int FTopOffset;
    internal int FBottomOffset;
    internal TAOWAlignWidth FAlignWidth;
    internal TAOWAlignHeight FAlignHeight;
    [Published]
    public TAOWAlignWidth AlignWidth { get => FAlignWidth; set => FAlignWidth = value; }

    [Published]
    public TAOWAlignHeight AlignHeight { get => FAlignHeight; set => FAlignHeight = value; }

    [Published]
    public byte HeightPercent { get => FHeightPercent; set => FHeightPercent = value; }

    [Published]
    public byte WidthPercent { get => FWidthPercent; set => FWidthPercent = value; }

    [Published]
    public int LeftOffPercent { get => FLeftOffPercent; set => FLeftOffPercent = value; }

    [Published]
    public int RightOffPercent { get => FRightOffPercent; set => FRightOffPercent = value; }

    [Published]
    public int TopOffPercent { get => FTopOffPercent; set => FTopOffPercent = value; }

    [Published]
    public int BottomOffPercent { get => FBottomOffPercent; set => FBottomOffPercent = value; }

    [Published]
    public int LeftOffset { get => FLeftOffset; set => FLeftOffset = value; }

    [Published]
    public int RightOffset { get => FRightOffset; set => FRightOffset = value; }

    [Published]
    public int TopOffset { get => FTopOffset; set => FTopOffset = value; }

    [Published]
    public int BottomOffset { get => FBottomOffset; set => FBottomOffset = value; }

}

// one entry of the dirty rectangle lists. R, Owner (the component that asked for the redraw.
// AddDrawRect only)
public partial class TaowRect : TObject
{
    public TRect R;
    public TAoWComponent Owner;
}

public static partial class AoWComp
{
}
