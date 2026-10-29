using System.Runtime.CompilerServices;
using Aow.Rtl;

namespace Aow.Units.AoWMemo;

public partial class TAOWMemo
{
    // AoWMemo.pas:511. The editable-byte comparison tests exactly one after the bounds checks.
    public override void CheckMouseUp(TMouseButton button, TSet<TShiftState> shift, int x, int y)
    {
        FHandled = false;
        FMouseDown = false;
        F114 = 0;
        bool old = FRedraw;
        FRedraw = false;
        if (FEditable && FItems.Count == 0)
        {
            FItems.Add("");
            FLines.Add("");
            FColorList.Add(new TColorChange());
        }
        if (x > FAOWWindow.FWinLeft + FWinLeft - 1 && x < FAOWWindow.FWinLeft + FWinLeft + FWinWidth &&
            y > FAOWWindow.FWinTop + FWinTop - 1 && y < FAOWWindow.FWinTop + FWinTop + FWinHeight)
        {
            int left, top, bottom;
            if (!FNoFrame)
            {
                left = FAOWWindow.FWinLeft + FWinLeft + FILFrame[FILIndexFrame + 3].Width - 1;
                top = FAOWWindow.FWinTop + FWinTop + FBorderHeight + FILFrame[FILIndexFrame + 1].Height - 1;
                bottom = FAOWWindow.FWinTop + FWinTop + FWinHeight - FILFrame[FILIndexFrame + 6].Height;
            }
            else
            {
                left = FAOWWindow.FWinLeft + FWinLeft - 1;
                top = FAOWWindow.FWinTop + FWinTop + FBorderHeight - 1;
                bottom = FAOWWindow.FWinTop + FWinTop + FWinHeight;
            }
            if (FEditable && x > left && x < FAOWWindow.FWinLeft + FWinLeft + FWinWidth - FScrollWidth && y > top && y < bottom && button == TMouseButton.mbLeft)
            {
                if (Unsafe.As<bool, byte>(ref FEditable) == 1)
                {
                    FRedraw = true;
                    FBlink = true;
                    FBlinkCount = 0;
                    FHandled = true;
                    FDrawn = false;
                    if (FNoFrame && FAOWWindow != null) FAOWWindow.FDrawn = false;
                }
                if ((FLines.Count - FTopItem) * FItemHeight + top < y)
                {
                    FCurLine = FLines.Count - 1;
                    FCurCol = FLines[FLines.Count - 1].Length;
                    FRealLine = FItems.Count - 1;
                    FRealCol = FItems[FItems.Count - 1].Length;
                }
                else
                {
                    int length = FItems[0].Length;
                    FRealCol = FRealLine = FCurCol = FCurLine = 0;
                    while (FCurLine < FTopItem)
                    {
                        FRealCol += FLines[FCurLine].Length;
                        if (FRealCol >= length)
                        {
                            FRealCol = 0;
                            FRealLine++;
                            length = FItems[FRealLine].Length;
                        }
                        FCurLine++;
                    }
                    while (FLines.Count > FCurLine && (FCurLine - FTopItem + 1) * FItemHeight + top < y)
                    {
                        FRealCol += FLines[FCurLine].Length;
                        if (FRealCol >= length)
                        {
                            FRealCol = 0;
                            FRealLine++;
                            length = FItems[FRealLine].Length;
                        }
                        FCurLine++;
                    }
                    if (FLines.Count == FCurLine) FCurLine--;
                    while (FLines[FCurLine].Length > FCurCol && FAOWFont.GetTextWidth(Aow.Rtl.Rtl.Copy(FLines[FCurLine], 1, FCurCol + 1)) + left < x)
                    {
                        FCurCol++;
                        FRealCol++;
                    }
                }
            }
            FOnMouseUp?.Invoke(this, button, shift, x, y);
        }
        if (FRedraw != old) FDrawn = false;
    }
}
