// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

// the common part of the display controllers. FLastFrameTime, FFrameCount (Word), FStatus,
// FDisplay, FPrimarySurfaceRgn (the output rectangle), FInvMatrix, FRealizePalet, FLastVBTime
public partial class DisplayController : CustomDisplayController
{

    private int _lastFrameTime;
    private ushort _frameCount;

    internal Set<DisplayControllerStatusElement> Status { get; set; }

    private CustomDisplay _display;

    internal Rect PrimarySurfaceRegion { get; private set; }

    private Matrix _inverseMatrix;

    internal bool RealizePaletteValue { get; private set; }
    internal int LastVerticalBlankTime { get; private set; }

    public DisplayController(Component owner) : base(owner)
    {
        _inverseMatrix = new Matrix(4, 4);
    }

    // DisplayC.pas:1031
    public override void Dispose()
    {
        if (Status.Contains(DisplayControllerStatusElement.Activated))
        {
            Deactivate();
        }

        if (_inverseMatrix != null)
        {
            _inverseMatrix.Dispose();
            _inverseMatrix = null;
        }

        base.Dispose();
    }

    // DisplayC.pas:634
    public override Matrix GetInverseMatrix() => _inverseMatrix;

    // DisplayC.pas:639
    public override int GetSurfaceCount()
    {
        int result;
        if (GetDisplay() == null)
        {
            result = 0;
        }
        else
        {
            result = (int)GetDisplay().BufferingScheme;
        }

        return result;
    }

    // DisplayC.pas:647
    public override int BlitSurface(int index)
    {
        var primarySurfaceRegion = GetPrimarySurfaceRegion();
        var surface = GetSurface(0);
        return GetSurface(index).BlitStretch(surface, default, primarySurfaceRegion);
    }

    // DisplayC.pas:652
    public override int FlipSurface(int index)
    {
        var primarySurfaceRegion = GetPrimarySurfaceRegion();
        var surface = GetSurface(0);
        int result = GetSurface(index).BlitStretch(surface, default, primarySurfaceRegion);
        if (Succeeded(result))
        {
            result = DisplayCodes.DCERR_NOFLIP;
        }

        return result;
    }

    // DisplayC.pas:660
    // { Bug kept from the original: the result of Resize is ignored, so Result stays 0. }
    public override int ResizeBackSurfaces(int width, int height)
    {
        /* TODO: Implement Controls.Color in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:675
    public override void SetPrimarySurfaceRegion(in Rect R)
    {
        PrimarySurfaceRegion = R;
    }

    // DisplayC.pas:680
    // The original returns @FPrimarySurfaceRgn, which is never nil.
    public override Rect? GetPrimarySurfaceRegion() => PrimarySurfaceRegion;

    // DisplayC.pas:685
    public override Set<DisplayControllerStatusElement> GetStatus() => Status;

    // DisplayC.pas:690
    public override int GetHPalette() => 0;

    // DisplayC.pas:695
    public override bool WindowProc(ref Message message)
    {
        bool result = true;
        if (message.Msg == 133)
        {
            result = false;
            message.Result = 0;
        }

        if (message.Msg == 20)
        {
            result = false;
            message.Result = 1;
        }

        return result;
    }

    // DisplayC.pas:710
    public override CustomDisplay GetDisplay() => _display;

    // DisplayC.pas:720
    public override void WindowRecreated()
    {
    }

    // DisplayC.pas:734
    public override int Activate(CustomDisplay display)
    {
        int result;
        if (!Status.Contains(DisplayControllerStatusElement.Activated))
        {
            _display = display;
            Status = Status + Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Activated);
            _display.CurrentFrameRate = _display.FrameRate;
            InvalidateAll();
            result = 0;
        }
        else
        {
            result = DCERR_ACTIVATED;
        }

        return result;
    }

    // DisplayC.pas:748
    public override int Deactivate()
    {
        int result;
        if (Status.Contains(DisplayControllerStatusElement.Activated))
        {
            Status = Status - Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Activated);
            if (_display != null)
            {
                _display.DeactivateDisplayController();
            }

            result = 0;
            _display = null;
        }
        else
        {
            result = DisplayCodes.DCERR_NOTACTIVATED;
        }

        return result;
    }

    // DisplayC.pas:762
    public override int SupportDisplayMode(CustomDisplay display) => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:767
    public override int SetPalette(Palette palette) => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:772
    public void RealizePalette()
    {
        if (_display.Status.Contains(DisplayStatusElement.FrameLoopActive))
        {
            RealizePaletteValue = true;
        }
        else
        {
            RealizePaletteNow();
        }
    }

    // DisplayC.pas:780
    public override int RealizePaletteNow() => DisplayCodes.DCERR_NOTSUPPORTED;

    // DisplayC.pas:787
    // { The mouse position in the display, or -1, -1 when the mouse is outside or another window has
    //   captured it. }
    public override void GetMousePosition(ref int X, ref int Y)
    {
        /* TODO: Implement Windows.GetCapture in the platform or editor service. */
        throw new NotImplementedException();
    }

    // DisplayC.pas:830
    public override int Hide()
    {
        Status = Status - Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Visible);
        return 0;
    }

    // DisplayC.pas:836
    public override int Show()
    {
        Status = Status + Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Visible);
        return 0;
    }

    // DisplayC.pas:842
    public override Palette GetPalette() => null;

    // DisplayC.pas:873
    public override void InitializeInverseMatrix(byte width, byte height)
    {
        if (_inverseMatrix != null)
        {
            _inverseMatrix.Dispose();
        }

        _inverseMatrix = new Matrix(width, height);
    }

    // DisplayC.pas:880
    public override void InvalidateAll()
    {
        if (_inverseMatrix != null)
        {
            for (int I = 0, last = this._inverseMatrix.Width - 1; I <= last; I++)
            {
                for (int J = 0, lastJ = this._inverseMatrix.Height - 1; J <= lastJ; J++)
                {
                    this._inverseMatrix.SetValue((byte)I, (byte)J, 1);
                }
            }
        }
    }

    // DisplayC.pas:894
    // { The size of the largest rectangle of marked blocks with its top left corner at X, Y, grown
    //   first downwards (rows of shrinking width) and then to the right (columns of shrinking
    //   height); BW and BH get its size in blocks, the result is its area. }
    public override int GetInverseBlockSize(int X, int Y, Matrix matrix, ref int bw, ref int bh)
    {
        int result;
        int size1;
        int size2;
        int w1;
        int h2;
        int w2;
        int h1;
        // P and Q are indexes into Matrix.FData.
        int P;
        int Q;
        int N;
        int L;
        int off;
        bool ok;
        P = X + matrix.Width * Y;
        if (matrix.Data[P] != 0)
        {
            h1 = 0;
            Q = P;
            w1 = matrix.Width - X;
            size1 = 0;
            do
            {
                N = w1;
                L = 0;
                while (N != 0 && matrix.Data[Q + L] != 0)
                {
                    L += 1;
                    N -= 1;
                }

                ok = L * h1 >= size1;
                if (ok)
                {
                    h1 += 1;
                    Q = Q + matrix.Width;
                    w1 = L;
                    size1 = h1 * L;
                }
            }
            while (!(!ok || h1 + Y == matrix.Height));
            bh = h1;
            bw = w1;
            w2 = 0;
            Q = P;
            h2 = matrix.Height - Y;
            size2 = 0;
            do
            {
                N = h2;
                L = 0;
                off = 0;
                while (N != 0 && matrix.Data[Q + off] != 0)
                {
                    L += 1;
                    N -= 1;
                    off += matrix.Width;
                }

                ok = L * w2 >= size2;
                if (ok)
                {
                    Q += 1;
                    w2 += 1;
                    h2 = L;
                    size2 = w2 * h2;
                }
            }
            while (!(!ok || w2 + X == matrix.Width));
            if (size2 > size1)
            {
                bw = w2;
                bh = h2;
                result = size2;
            }
            else
            {
                result = size1;
            }
        }
        else
        {
            result = 0;
        }

        return result;
    }

    // DisplayC.pas:964
    public override void GetPixelPositionOfBlock(int bx, int by, Rect R, ref int pixelX, ref int pixelY, ref int bw, ref int bh)
    {
        int W;
        int H;
        W = (R.Right - R.Left + _inverseMatrix.Width - 1) / _inverseMatrix.Width;
        H = (R.Bottom - R.Top + _inverseMatrix.Height - 1) / _inverseMatrix.Height;
        pixelX = W * bx;
        pixelY = H * by;
        bw = W;
        bh = H;
    }

    // DisplayC.pas:976
    public override void GetBlockPositionOfPixel(int pixelX, int pixelY, Rect R, ref int bx, ref int by)
    {
        int W;
        int H;
        W = (R.Right - R.Left + 1 + _inverseMatrix.Width - 1) / _inverseMatrix.Width;
        H = (R.Bottom - R.Top + 1 + _inverseMatrix.Height - 1) / _inverseMatrix.Height;
        bx = (pixelX - R.Left) / W;
        by = (pixelY - R.Top) / H;
    }

    // DisplayC.pas:988
    // { Marks the blocks of the pixel rectangle X, Y, W, H with Value. Bug kept from the original:
    //   the range test checks BY2 >= 0 where BX2 >= 0 was meant (and BY1 twice). }
    public void InverseWndwithValue(int X, int Y, int W, int H, byte value)
    {
        int bx1 = default;
        int bx2 = default;
        int by1 = default;
        int by2 = default;
        Matrix M;
        Rect R = default;
        if (_inverseMatrix != null)
        {
            M = _inverseMatrix;
            R.Left = 0;
            R.Right = GetSurface(1).Width;
            R.Top = 0;
            R.Bottom = GetSurface(1).Height;
            GetBlockPositionOfPixel(X, Y, R, ref bx1, ref by1);
            GetBlockPositionOfPixel(X + W, Y + H, R, ref bx2, ref by2);
            if (M.Width > bx1 && M.Height > by1 && by2 >= 0 && by1 >= 0)
            {
                if (bx1 < 0)
                {
                    bx1 = 0;
                }

                if (by1 < 0)
                {
                    by1 = 0;
                }

                if (M.Width <= bx2)
                {
                    bx2 = M.Width - 1;
                }

                if (M.Height <= by2)
                {
                    by2 = M.Height - 1;
                }

                X = bx1;
                int last = bx2;
                for (; X <= last; X += 1)
                {
                    Y = by1;
                    int limit = by2;
                    for (; Y <= limit; Y += 1)
                    {
                        M.SetValue((byte)X, (byte)Y, value);
                    }
                }
            }
        }
    }

    // DisplayC.pas:1020
    public override void Invalidate(int X, int Y, int W, int H)
    {
        InverseWndwithValue(X, Y, W, H, 1);
    }

    // DisplayC.pas:1043
    public int GetCurrentFrameRate() => _display.CurrentFrameRate;

    // DisplayC.pas:1049
    // { Counts frames; every 5 frames the frame rate is measured. }
    public override void NewFrame()
    {
        int T;
        if (_frameCount >= 5)
        {
            T = PixelFormats.GetElapsedMilliseconds();
            _display.CurrentFrameRate = Round((double)5000 / (T - _lastFrameTime + 1));
            _frameCount = 0;
            _lastFrameTime = T;
        }

        _frameCount = (ushort)(_frameCount + 1);
    }

    // DisplayC.pas:1063
    public override void ShowFrameRate()
    {
    }

    private const int DCERR_ACTIVATED = -2147414011;

}
