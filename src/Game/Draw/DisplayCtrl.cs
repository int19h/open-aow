// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the common part of the display controllers. FLastFrameTime, FFrameCount (Word), FStatus,
// FDisplay, FPrimarySurfaceRgn (the output rectangle), FInvMatrix, FRealizePalet, FLastVBTime
public partial class DisplayCtrl : AbstractDC
{

    internal int FLastFrameTime;
    internal ushort FFrameCount;
    internal Set<DisplayCtrlStatusElement> FStatus;
    internal CustomDisplay FDisplay;
    internal Rect FPrimarySurfaceRgn;
    internal Matrix FInvMatrix;
    internal bool FRealizePalet;
    internal int FLastVBTime;

    public DisplayCtrl(Component owner) : base(owner)
    {
        FInvMatrix = new Matrix(4, 4);
    }

    // DisplayC.pas:1031
    public override void Dispose()
    {
        if (FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            Deactivate();
        }

        if (FInvMatrix != null)
        {
            FInvMatrix.Dispose();
            FInvMatrix = null;
        }

        base.Dispose();
    }

    // DisplayC.pas:634
    public override Matrix GetInvMatrix() => FInvMatrix;

    // DisplayC.pas:639
    public override int GetSurfaceCount()
    {
        int Result;
        if (GetDisplay() == null)
        {
            Result = 0;
        }
        else
        {
            Result = (int)GetDisplay().FBufferingScheme;
        }

        return Result;
    }

    // DisplayC.pas:647
    public override int BltSurface(int Index)
    {
        var primarySurfaceRgn = GetPrimarySurfaceRgn();
        var surface = GetSurface(0);
        return GetSurface(Index).BltStretch(surface, default, primarySurfaceRgn);
    }

    // DisplayC.pas:652
    public override int FlipSurface(int Index)
    {
        var primarySurfaceRgn = GetPrimarySurfaceRgn();
        var surface = GetSurface(0);
        int Result = GetSurface(Index).BltStretch(surface, default, primarySurfaceRgn);
        if (Succeeded(Result))
        {
            Result = DisplayCodes.DCERR_NOFLIP;
        }

        return Result;
    }

    // DisplayC.pas:660
    // { Bug kept from the original: the result of Resize is ignored, so Result stays 0. }
    public override int ResizeBackSurfaces(int AWidth, int AHeight)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:675
    public override void SetPrimarySurfaceRgn(in Rect R)
    {
        FPrimarySurfaceRgn = R;
    }

    // DisplayC.pas:680
    // The original returns @FPrimarySurfaceRgn, which is never nil.
    public override Rect? GetPrimarySurfaceRgn() => FPrimarySurfaceRgn;

    // DisplayC.pas:685
    public override Set<DisplayCtrlStatusElement> GetStatus() => FStatus;

    // DisplayC.pas:690
    public override int GetHPalette() => 0;

    // DisplayC.pas:695
    public override bool WndProc(ref Message Message)
    {
        bool Result = true;
        if (Message.Msg == 133)
        {
            Result = false;
            Message.Result = 0;
        }

        if (Message.Msg == 20)
        {
            Result = false;
            Message.Result = 1;
        }

        return Result;
    }

    // DisplayC.pas:710
    public override CustomDisplay GetDisplay() => FDisplay;

    // DisplayC.pas:720
    public override void WindowRecreated()
    {
    }

    // DisplayC.pas:734
    public override int Activate(CustomDisplay ADisplay)
    {
        int Result;
        if (!FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            FDisplay = ADisplay;
            FStatus = FStatus + Set<DisplayCtrlStatusElement>.Of(DisplayCtrlStatusElement.Activated);
            FDisplay.FCurrentFrameRate = FDisplay.FFrameRate;
            InvalidateAll();
            Result = 0;
        }
        else
        {
            Result = DCERR_ACTIVATED;
        }

        return Result;
    }

    // DisplayC.pas:748
    public override int Deactivate()
    {
        int Result;
        if (FStatus.Contains(DisplayCtrlStatusElement.Activated))
        {
            FStatus = FStatus - Set<DisplayCtrlStatusElement>.Of(DisplayCtrlStatusElement.Activated);
            if (FDisplay != null)
            {
                FDisplay.DeactivateDC();
            }

            Result = 0;
            FDisplay = null;
        }
        else
        {
            Result = DisplayCodes.DCERR_NOTACTIVATED;
        }

        return Result;
    }

    // DisplayC.pas:762
    public override int SupportDisplayMode(CustomDisplay ADisplay) => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:767
    public override int SetPalet(GFXEPalette Palette) => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:772
    public void RealizePalet()
    {
        if (FDisplay.FStatus.Contains(DisplayStatusElement.FrameLoopActive))
        {
            FRealizePalet = true;
        }
        else
        {
            RealizePaletNow();
        }
    }

    // DisplayC.pas:780
    public override int RealizePaletNow() => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:787
    // { The mouse position in the display, or -1, -1 when the mouse is outside or another window has
    //   captured it. }
    public override void GetMousePos(ref int X, ref int Y)
    {
        /* TODO: Implement Windows.GetCapture in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:830
    public override int Hide()
    {
        FStatus = FStatus - Set<DisplayCtrlStatusElement>.Of(DisplayCtrlStatusElement.Visible);
        return 0;
    }

    // DisplayC.pas:836
    public override int Show()
    {
        FStatus = FStatus + Set<DisplayCtrlStatusElement>.Of(DisplayCtrlStatusElement.Visible);
        return 0;
    }

    // DisplayC.pas:842
    public override GFXEPalette GetPalette() => null;

    // DisplayC.pas:873
    public override void InitInvMatrix(byte AWidth, byte AHeight)
    {
        if (FInvMatrix != null)
        {
            FInvMatrix.Dispose();
        }

        FInvMatrix = new Matrix(AWidth, AHeight);
    }

    // DisplayC.pas:880
    public override void InvalidateAll()
    {
        if (FInvMatrix != null)
        {
            for (int I = 0, last = this.FInvMatrix.Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = this.FInvMatrix.Height - 1; J <= lastJ; J++)
                {
                    this.FInvMatrix.SetVal((byte)I, (byte)J, 1);
                }
            }
        }
    }

    // DisplayC.pas:894
    // { The size of the largest rectangle of marked blocks with its top left corner at X, Y, grown
    //   first downwards (rows of shrinking width) and then to the right (columns of shrinking
    //   height); BW and BH get its size in blocks, the result is its area. }
    public override int GetInvBlockSize(int X, int Y, Matrix Matrix, ref int BW, ref int BH)
    {
        int Result;
        int Size1;
        int Size2;
        int W1;
        int H2;
        int W2;
        int H1;
        // P and Q are indexes into Matrix.FData.
        int P;
        int Q;
        int N;
        int L;
        int Off;
        bool Ok;
        P = X + Matrix.FWidth * Y;
        if (Matrix.FData[P] != 0)
        {
            H1 = 0;
            Q = P;
            W1 = Matrix.FWidth - X;
            Size1 = 0;
            do
            {
                N = W1;
                L = 0;
                while (N != 0 && Matrix.FData[Q + L] != 0)
                {
                    L += 1;
                    N -= 1;
                }

                Ok = L * H1 >= Size1;
                if (Ok)
                {
                    H1 += 1;
                    Q = Q + Matrix.FWidth;
                    W1 = L;
                    Size1 = H1 * L;
                }
            }
            while (!(!Ok || H1 + Y == Matrix.FHeight));
            BH = H1;
            BW = W1;
            W2 = 0;
            Q = P;
            H2 = Matrix.FHeight - Y;
            Size2 = 0;
            do
            {
                N = H2;
                L = 0;
                Off = 0;
                while (N != 0 && Matrix.FData[Q + Off] != 0)
                {
                    L += 1;
                    N -= 1;
                    Off += Matrix.FWidth;
                }

                Ok = L * W2 >= Size2;
                if (Ok)
                {
                    Q += 1;
                    W2 += 1;
                    H2 = L;
                    Size2 = W2 * H2;
                }
            }
            while (!(!Ok || W2 + X == Matrix.FWidth));
            if (Size2 > Size1)
            {
                BW = W2;
                BH = H2;
                Result = Size2;
            }
            else
            {
                Result = Size1;
            }
        }
        else
        {
            Result = 0;
        }

        return Result;
    }

    // DisplayC.pas:964
    public override void GetPixPosofBlock(int BX, int BY, Rect R, ref int PX, ref int PY, ref int BW, ref int BH)
    {
        int W;
        int H;
        W = (R.Right - R.Left + FInvMatrix.FWidth - 1) / FInvMatrix.FWidth;
        H = (R.Bottom - R.Top + FInvMatrix.FHeight - 1) / FInvMatrix.FHeight;
        PX = W * BX;
        PY = H * BY;
        BW = W;
        BH = H;
    }

    // DisplayC.pas:976
    public override void GetBlockPosofPix(int PX, int PY, Rect R, ref int BX, ref int BY)
    {
        int W;
        int H;
        W = (R.Right - R.Left + 1 + FInvMatrix.FWidth - 1) / FInvMatrix.FWidth;
        H = (R.Bottom - R.Top + 1 + FInvMatrix.FHeight - 1) / FInvMatrix.FHeight;
        BX = (PX - R.Left) / W;
        BY = (PY - R.Top) / H;
    }

    // DisplayC.pas:988
    // { Marks the blocks of the pixel rectangle X, Y, W, H with Value. Bug kept from the original:
    //   the range test checks BY2 >= 0 where BX2 >= 0 was meant (and BY1 twice). }
    public void InvWndwithValue(int X, int Y, int W, int H, byte Value)
    {
        int BX1 = default;
        int BX2 = default;
        int BY1 = default;
        int BY2 = default;
        Matrix M;
        Rect R = default;
        if (FInvMatrix != null)
        {
            M = FInvMatrix;
            R.Left = 0;
            R.Right = GetSurface(1).Width;
            R.Top = 0;
            R.Bottom = GetSurface(1).Height;
            GetBlockPosofPix(X, Y, R, ref BX1, ref BY1);
            GetBlockPosofPix(X + W, Y + H, R, ref BX2, ref BY2);
            if (M.FWidth > BX1 && M.FHeight > BY1 && BY2 >= 0 && BY1 >= 0)
            {
                if (BX1 < 0)
                {
                    BX1 = 0;
                }

                if (BY1 < 0)
                {
                    BY1 = 0;
                }

                if (M.FWidth <= BX2)
                {
                    BX2 = M.FWidth - 1;
                }

                if (M.FHeight <= BY2)
                {
                    BY2 = M.FHeight - 1;
                }

                X = BX1;
                int last = BX2;
                for (; X <= last; X += 1)
                {
                    Y = BY1;
                    int limit = BY2;
                    for (; Y <= limit; Y += 1)
                    {
                        M.SetVal((byte)X, (byte)Y, Value);
                    }
                }
            }
        }
    }

    // DisplayC.pas:1020
    public override void Invalidate(int X, int Y, int W, int H)
    {
        InvWndwithValue(X, Y, W, H, 1);
    }

    // DisplayC.pas:1043
    public int GetCurrentFrameRate() => FDisplay.FCurrentFrameRate;

    // DisplayC.pas:1049
    // { Counts frames; every 5 frames the frame rate is measured. }
    public override void NewFrame()
    {
        int T;
        if (FFrameCount >= 5)
        {
            T = PixelFormats.GetElapsedMilliSeconds();
            FDisplay.FCurrentFrameRate = Round((double)5000 / (T - FLastFrameTime + 1));
            FFrameCount = 0;
            FLastFrameTime = T;
        }

        FFrameCount = (ushort)(FFrameCount + 1);
    }

    // DisplayC.pas:1063
    public override void ShowFrameRate()
    {
    }

    private const int DCERR_ACTIVATED = -2147414011;

}
