// Translated from AoWFrame.pas.
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.UI.Windows;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Controls;

// Fields: FFrameType, FILFrame, FILTexture, FILLine, FILIndexFrame, FILIndexLine,
// FILIndexTexture, FPopupMenu
public partial class Frame : Control
{
    public Frame(Component owner) : base(owner)
    { }

    public FrameType FFrameType;
    public InterfaceImageLibrary FILFrame;
    public InterfaceImageLibrary FILTexture;
    public InterfaceImageLibrary FILLine;
    public int FILIndexFrame;
    public int FILIndexLine;
    public int FILIndexTexture;
    public Menu FPopupMenu;

    // AoWFrame.pas:63
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

        if (FILLine != null)
        {
            FILLine = FILLine.CheckIL(FILIndexLine, 3, this);
        }

        if (FILFrame != null || FILLine != null)
        {
            FValid = true;
            if (FILFrame != null)
            {
                if (FILTexture != null)
                {
                    FFrameType = FrameType.FilledBox;
                }
                else
                {
                    FFrameType = FrameType.HollowBox;
                }
            }
            else
            {
                if (FILLine != null)
                {
                    if (FFrameType != FrameType.HorizLine)
                    {
                        FFrameType = FrameType.VertLine;
                    }
                }
            }
        }

        base.Validate();
    }

    // AoWFrame.pas:113
    public override void SetSize()
    {
        switch (FFrameType)
        {
            case FrameType.FilledBox:
            case FrameType.HollowBox:
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
                    var ilFrame3 = FILFrame[FILIndexFrame + 2];
                    FWinWidth = ilFrame2.Width + ilFrame3.Width;
                }

                break;
            case FrameType.HorizLine:
                FWinHeight = FILLine[FILIndexLine].Height;
                var ilLineWidth = FILLine[FILIndexLine];
                var ilLine = FILLine[FILIndexLine + 2];
                if (ilLineWidth.Width + ilLine.Width > FWinWidth)
                {
                    var ilLine2 = FILLine[FILIndexLine];
                    var ilLine3 = FILLine[FILIndexLine + 2];
                    FWinWidth = ilLine2.Width + ilLine3.Width;
                }

                break;
            case FrameType.VertLine:
                var ilLineHeight = FILLine[FILIndexLine];
                var ilLine4 = FILLine[FILIndexLine + 2];
                if (ilLineHeight.Height + ilLine4.Height > FWinHeight)
                {
                    var ilLine2 = FILLine[FILIndexLine];
                    var ilLine3 = FILLine[FILIndexLine + 2];
                    FWinHeight = ilLine2.Height + ilLine3.Height;
                }

                FWinWidth = FILLine[FILIndexLine].Width;
                break;
        }

        base.SetSize();
    }

    // AoWFrame.pas:166
    public override void Draw(LockedSurface SurfDesc)
    {
        try
        {
            if (FValid && FVisible)
            {
                if (FOnPreDraw != null)
                {
                    FOnPreDraw(this, SurfDesc);
                }

                switch (FFrameType)
                {
                    case FrameType.VertLine:
                        var rect = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILLine.DrawILLv(FILIndexLine, rect, ref SurfDesc);
                        break;
                    case FrameType.HorizLine:
                        var rect2 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILLine.DrawILLh(FILIndexLine, rect2, ref SurfDesc);
                        break;
                    case FrameType.HollowBox:
                        var rect3 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect3, ref SurfDesc);
                        break;
                    case FrameType.FilledBox:
                        var rect4 = new Rect(FAOWWindow.FWinLeft + FWinLeft, FAOWWindow.FWinTop + FWinTop, FWinWidth, FWinHeight);
                        FILFrame.DrawILF(FILIndexFrame, rect4, ref SurfDesc);
                        var ilFrameHeight = FILFrame[FILIndexFrame + 1];
                        var ilFrame = FILFrame[FILIndexFrame + 6];
                        var ilFrameWidth = FILFrame[FILIndexFrame + 3];
                        var ilFrame2 = FILFrame[FILIndexFrame + 4];
                        var ilFrame3 = FILFrame[FILIndexFrame + 1];
                        var ilFrame4 = FILFrame[FILIndexFrame + 3];
                        var rect5 = new Rect(FAOWWindow.FWinLeft + FWinLeft + ilFrame4.Width, FAOWWindow.FWinTop + FWinTop + ilFrame3.Height, FWinWidth - ilFrameWidth.Width - ilFrame2.Width, FWinHeight - ilFrameHeight.Height - ilFrame.Height);
                        FILTexture.DrawILT(FILIndexTexture, rect5, ref SurfDesc);
                        break;
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

    // AoWFrame.pas:202
    public override void CheckMouseDown(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        base.CheckMouseDown(Button, Shift, X, Y);
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            FHandled = true;
            if (FOnMouseDown != null)
            {
                FOnMouseDown(this, Button, Shift, X, Y);
            }

            if (Button == MouseButton.Right && FPopupMenu != null)
            {
                FPopupMenu.SetPopupOwner(this);
                FPopupMenu.WinLeft = X;
                FPopupMenu.WinTop = Y;
                if (FPopupMenu.FWinManager != null)
                {
                    if (FPopupMenu.FHidden)
                    {
                        if (FPopupMenu.FMenuLeft + FPopupMenu.FWinWidth > FPopupMenu.FWinManager.FWinWidth)
                        {
                            FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                        }

                        if (FPopupMenu.FMenuTop + FPopupMenu.FWinHeight > FPopupMenu.FWinManager.FWinHeight)
                        {
                            FPopupMenu.WinTop = Y - FPopupMenu.FWinHeight;
                        }
                    }
                    else
                    {
                        if (FPopupMenu.FWinLeft + FPopupMenu.FWinWidth > FPopupMenu.FWinManager.FWinWidth)
                        {
                            FPopupMenu.WinLeft = X - FPopupMenu.FWinWidth;
                        }

                        if (FPopupMenu.FWinTop + FPopupMenu.FWinHeight > FPopupMenu.FWinManager.FWinHeight)
                        {
                            FPopupMenu.WinTop = Y - FPopupMenu.FWinHeight;
                        }
                    }
                }

                FPopupMenu.Visible = true;
            }
        }
    }

    // AoWFrame.pas:237
    public override void CheckMouseUp(MouseButton Button, Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            if (FOnMouseUp != null)
            {
                FOnMouseUp(this, Button, Shift, X, Y);
            }
        }
    }

    // AoWFrame.pas:246
    public override void CheckMouseMove(Set<ShiftState> Shift, int X, int Y)
    {
        FHandled = false;
        if (X > FAOWWindow.FWinLeft + FWinLeft - 1 && X < FAOWWindow.FWinLeft + FWinLeft + FWinWidth && Y > FAOWWindow.FWinTop + FWinTop - 1 && Y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            if (FOnMouseMove != null)
            {
                FOnMouseMove(this, Shift, X, Y);
            }
        }
    }

    // AoWFrame.pas:255
    public override void CheckKeyPress(ref byte Key)
    {
        FHandled = false;
    }

    // AoWFrame.pas:139
    public virtual void SetILFrame(InterfaceImageLibrary Value)
    {
        if (Value != FILFrame)
        {
            FValid = false;
            FILFrame = Value;
        }
    }

    // AoWFrame.pas:148
    public virtual void SetILTexture(InterfaceImageLibrary Value)
    {
        if (Value != FILTexture)
        {
            FValid = false;
            FILTexture = Value;
        }
    }

    // AoWFrame.pas:157
    public virtual void SetILLine(InterfaceImageLibrary Value)
    {
        if (Value != FILLine)
        {
            FValid = false;
            FILLine = Value;
        }
    }

    // AoWFrame.pas:86
    public void SetILFIndex(int Value)
    {
        if (Value != FILIndexFrame)
        {
            FILIndexFrame = Value;
            FValid = false;
        }
    }

    // AoWFrame.pas:95
    public void SetILLIndex(int Value)
    {
        if (Value != FILIndexLine)
        {
            FILIndexLine = Value;
            FValid = false;
        }
    }

    // AoWFrame.pas:104
    public void SetILTIndex(int Value)
    {
        if (Value != FILIndexTexture)
        {
            FILIndexTexture = Value;
            FValid = false;
        }
    }

    [Published]
    public FrameType FrameType { get => FFrameType; set => FFrameType = value; }

    [Published]
    public InterfaceImageLibrary ILFrame { get => FILFrame; set => SetILFrame(value); }

    [Published]
    public int ILIndexFrame { get => FILIndexFrame; set => SetILFIndex(value); }

    [Published]
    public int ILIndexLine { get => FILIndexLine; set => SetILLIndex(value); }

    [Published]
    public int ILIndexTexture { get => FILIndexTexture; set => SetILTIndex(value); }

    [Published]
    public InterfaceImageLibrary ILLine { get => FILLine; set => SetILLine(value); }

    [Published]
    public InterfaceImageLibrary ILTexture { get => FILTexture; set => SetILTexture(value); }

    [Published]
    public Menu PopupMenu { get => FPopupMenu; set => FPopupMenu = value; }

    public new NotifyEvent<Frame> OnChange
    {
        get => EventSlot.Get(this, FOnChange);
        set => EventSlot.Set(this, ref FOnChange, value);
    }

    public new MouseEvent<Frame> OnMouseDown
    {
        get => EventSlot.Get(this, FOnMouseDown);
        set => EventSlot.Set(this, ref FOnMouseDown, value);
    }

    public new MouseEvent<Frame> OnMouseUp
    {
        get => EventSlot.Get(this, FOnMouseUp);
        set => EventSlot.Set(this, ref FOnMouseUp, value);
    }

    public new MouseMoveEvent<Frame> OnMouseMove
    {
        get => EventSlot.Get(this, FOnMouseMove);
        set => EventSlot.Set(this, ref FOnMouseMove, value);
    }

    public new KeyPressEvent<Frame> OnKeyPress
    {
        get => EventSlot.Get(this, FOnKeyPress);
        set => EventSlot.Set(this, ref FOnKeyPress, value);
    }

    public new KeyEvent<Frame> OnKeyDown
    {
        get => EventSlot.Get(this, FOnKeyDown);
        set => EventSlot.Set(this, ref FOnKeyDown, value);
    }

    public new AoWOnDrawEvent<Frame> OnPreDraw
    {
        get => EventSlot.Get(this, FOnPreDraw);
        set => EventSlot.Set(this, ref FOnPreDraw, value);
    }

    public new AoWOnDrawEvent<Frame> OnDraw
    {
        get => EventSlot.Get(this, FOnDraw);
        set => EventSlot.Set(this, ref FOnDraw, value);
    }

    public new NotifyEvent<Frame> OnUpdate
    {
        get => EventSlot.Get(this, FOnUpdate);
        set => EventSlot.Set(this, ref FOnUpdate, value);
    }
}
