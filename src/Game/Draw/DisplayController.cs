#nullable disable
// Translated from DisplayC.pas.
using Aow.Rtl;
using static Aow.Rtl.DelphiRounding;

namespace Aow.Game.Draw;

// The display controller. TAbstractDC declared 35 virtual methods, all abstract but GetSurface and
// BlitInvalidatedSurface, and TDisplayCtrl implemented the other 33 as the common part of the
// display controllers. FLastFrameTime, FFrameCount (Word), FStatus, FDisplay, FPrimarySurfaceRgn
// (the output rectangle), FInvMatrix, FRealizePalet, FLastVBTime
public partial class DisplayController(Component owner) : Component(owner)
{

    // DisplayC.pas:554
    public virtual Surface GetSurface(int index) => null;

    // DisplayC.pas:562
    // { Blits the invalidated blocks of back surface Index to the primary surface: every block the
    //   matrix marks is merged with its marked neighbours (GetInverseBlockSize) into one rectangle, and
    //   the marks count down, so a block is blitted once per back surface. }
    public virtual DisplayControllerError BlitInvalidatedSurface(int index)
    {
        DisplayControllerError result;
        Rect clipR = default;
        Rect sourceR = default;
        Rect destinationR = default;
        if (InverseMatrix is not null)
        {
            result = DisplayControllerError.Ok;
            Matrix inverseMatrix = InverseMatrix;
            Matrix m = new(InverseMatrix.Width, inverseMatrix.Height);
            for (int i = 0, last = InverseMatrix.Width - 1; i <= last; i++)
            {
                for (int j = 0, lastJ = InverseMatrix.Height - 1; j <= lastJ; j++)
                {
                    if (InverseMatrix.GetValue((byte)i, (byte)j) != 0)
                    {
                        m.SetValue((byte)i, (byte)j, 1);
                    }
                }
            }

            clipR.Left = 0;
            clipR.Right = GetSurface(1).Width;
            clipR.Top = 0;
            clipR.Bottom = GetSurface(1).Height;
            int bw = default;
            int bh = default;
            for (int j = 0, lastJ = m.Height - 1; j <= lastJ; j++)
            {
                for (int i = 0, last = m.Width - 1; i <= last; i++)
                {
                    if (result >= 0)
                    {
                        int v = InverseMatrix.GetValue((byte)i, (byte)j);
                        if (v != 0)
                        {
                            InverseMatrix.SetValue((byte)i, (byte)j, (byte)(v - 1));
                        }

                        if (m.GetValue((byte)i, (byte)j) != 0)
                        {
                            _ = GetInverseBlockSize(i, j, m, ref bw, ref bh);
                            _ = sourceR.Left;
                            _ = sourceR.Top;
                            GetPixelPositionOfBlock(i, j, clipR, out int left, out int top, out int blockWidth, out int blockHeight);
                            sourceR.Left = left;
                            sourceR.Top = top;
                            int pixelX2;
                            int pixelY2;
                            if (bw > 1 || bh > 1)
                            {
                                GetPixelPositionOfBlock(i + bw - 1, j + bh - 1, clipR, out pixelX2, out pixelY2, out blockWidth, out blockHeight);
                                sourceR.Right = pixelX2 + blockWidth;
                                sourceR.Bottom = pixelY2 + blockHeight;
                            }
                            else
                            {
                                sourceR.Right = sourceR.Left + blockWidth;
                                sourceR.Bottom = sourceR.Top + blockHeight;
                            }

                            Rect? primarySurfaceRegion = GetPrimarySurfaceRegion();
                            _ = destinationR.Left;
                            _ = destinationR.Top;
                            GetPixelPositionOfBlock(i, j, primarySurfaceRegion.Value, out int left2, out int top2, out blockWidth, out blockHeight);
                            destinationR.Left = left2;
                            destinationR.Top = top2;
                            Rect? primarySurfaceRegion2 = GetPrimarySurfaceRegion();
                            destinationR.Left += primarySurfaceRegion2.Value.Left;
                            Rect? primarySurfaceRegion3 = GetPrimarySurfaceRegion();
                            destinationR.Top += primarySurfaceRegion3.Value.Top;
                            if (bw > 1 || bh > 1)
                            {
                                Rect? primarySurfaceRegion4 = GetPrimarySurfaceRegion();
                                GetPixelPositionOfBlock(i + bw - 1, j + bh - 1, primarySurfaceRegion4.Value, out pixelX2, out pixelY2, out blockWidth, out blockHeight);
                                Rect? primarySurfaceRegion5 = GetPrimarySurfaceRegion();
                                destinationR.Right = pixelX2 + blockWidth + primarySurfaceRegion5.Value.Left;
                                Rect? primarySurfaceRegion6 = GetPrimarySurfaceRegion();
                                destinationR.Bottom = pixelY2 + blockHeight + primarySurfaceRegion6.Value.Top;
                            }
                            else
                            {
                                destinationR.Right = destinationR.Left + blockWidth;
                                destinationR.Bottom = destinationR.Top + blockHeight;
                            }

                            Surface surface = GetSurface(0);
                            result = (DisplayControllerError)GetSurface(index).BlitStretch(surface, sourceR, destinationR);
                            for (int x = 0, lastX = bw - 1; x <= lastX; x++)
                            {
                                for (int y = 0, lastY = bh - 1; y <= lastY; y++)
                                {
                                    m.SetValue((byte)(i + x), (byte)(j + y), 0);
                                }
                            }
                        }
                    }
                }
            }

            m.Dispose();
        }
        else
        {
            return DisplayControllerError.Failed;
        }

        return result;
    }

    private int _lastFrameTime;
    private ushort _frameCount;

    internal Set<DisplayControllerStatusElement> Status { get; set; }

    private CustomDisplay _display;

    internal Rect PrimarySurfaceRegion { get; private set; }

    private Matrix _inverseMatrix = new(4, 4);

    // DisplayC.pas:1031
    public override void Dispose()
    {
        if (Status.Contains(DisplayControllerStatusElement.Activated))
        {
            _ = Deactivate();
        }

        _inverseMatrix?.Dispose();
        _inverseMatrix = null;

        base.Dispose();
    }

    // DisplayC.pas:634
    public virtual Matrix InverseMatrix => _inverseMatrix;

    // DisplayC.pas:639
    public virtual int SurfaceCount => Display is null ? 0 : (int)Display.BufferingScheme;

    // DisplayC.pas:647
    public virtual DisplayControllerError BlitSurface(int index)
    {
        Rect? primarySurfaceRegion = GetPrimarySurfaceRegion();
        Surface surface = GetSurface(0);
        return (DisplayControllerError)GetSurface(index).BlitStretch(surface, default, primarySurfaceRegion);
    }

    // DisplayC.pas:652
    public virtual DisplayControllerError FlipSurface(int index)
    {
        Rect? primarySurfaceRegion = GetPrimarySurfaceRegion();
        Surface surface = GetSurface(0);
        DisplayControllerError result = (DisplayControllerError)GetSurface(index).BlitStretch(surface, default, primarySurfaceRegion);
        return result.IsSuccess() ? DisplayControllerError.NoFlip : result;
    }

    // DisplayC.pas:660
    // { Bug kept from the original: the result of Resize is ignored, so Result stays 0. }
    public virtual DisplayControllerError ResizeBackSurfaces(int width, int height)
        /* TODO: Implement Controls.Color in the platform or editor service. */
        => throw new NotImplementedException();

    // DisplayC.pas:675
    public virtual void SetPrimarySurfaceRegion(in Rect R) => PrimarySurfaceRegion = R;

    // DisplayC.pas:680
    // The original returns @FPrimarySurfaceRgn, which is never nil.
    public virtual Rect? GetPrimarySurfaceRegion() => PrimarySurfaceRegion;

    // DisplayC.pas:685
    public virtual Set<DisplayControllerStatusElement> GetStatus() => Status;

    // DisplayC.pas:690
    public virtual int HPalette => 0;

    // DisplayC.pas:710
    public virtual CustomDisplay Display => _display;

    // DisplayC.pas:720
    public virtual void WindowRecreated()
    {
    }

    // DisplayC.pas:734
    public virtual DisplayControllerError Activate(CustomDisplay display)
    {
        if (!Status.Contains(DisplayControllerStatusElement.Activated))
        {
            _display = display;
            Status += Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Activated);
            _display.CurrentFrameRate = _display.FrameRate;
            InvalidateAll();
            return DisplayControllerError.Ok;
        }
        else
        {
            return DisplayControllerError.Activated;
        }
    }

    // DisplayC.pas:748
    public virtual DisplayControllerError Deactivate()
    {
        if (Status.Contains(DisplayControllerStatusElement.Activated))
        {
            Status -= Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Activated);
            _ = (_display?.DeactivateDisplayController());
            _display = null;
        }
        else
        {
            return DisplayControllerError.NotActivated;
        }

        return DisplayControllerError.Ok;
    }

    // DisplayC.pas:762
    public virtual DisplayControllerError SupportDisplayMode(CustomDisplay display) => DisplayControllerError.NotSupported;

    // DisplayC.pas:767
    public virtual DisplayControllerError SetPalette(Palette palette) => DisplayControllerError.NotSupported;

    // DisplayC.pas:772
    public void RealizePalette()
    {
        if (_display.Status.Contains(DisplayStatusElement.FrameLoopActive))
        {
        }
        else
        {
            _ = RealizePaletteNow();
        }
    }

    // DisplayC.pas:780
    public virtual DisplayControllerError RealizePaletteNow() => DisplayControllerError.NotSupported;

    // DisplayC.pas:787
    // { The mouse position in the display, or -1, -1 when the mouse is outside or another window has
    //   captured it. }
    public virtual void GetMousePosition(ref int X, ref int Y)
        /* TODO: Implement Windows.GetCapture in the platform or editor service. */
        => throw new NotImplementedException();

    // DisplayC.pas:830
    public virtual DisplayControllerError Hide()
    {
        Status -= Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Visible);
        return DisplayControllerError.Ok;
    }

    // DisplayC.pas:836
    public virtual DisplayControllerError Show()
    {
        Status += Set<DisplayControllerStatusElement>.Of(DisplayControllerStatusElement.Visible);
        return DisplayControllerError.Ok;
    }

    // DisplayC.pas:842
    public virtual Palette GetPalette() => null;

    // DisplayC.pas:873
    public virtual void InitializeInverseMatrix(byte width, byte height)
    {
        _inverseMatrix?.Dispose();

        _inverseMatrix = new Matrix(width, height);
    }

    // DisplayC.pas:880
    public virtual void InvalidateAll()
    {
        if (_inverseMatrix is not null)
        {
            for (int i = 0, last = _inverseMatrix.Width - 1; i <= last; i++)
            {
                for (int j = 0, lastJ = _inverseMatrix.Height - 1; j <= lastJ; j++)
                {
                    _inverseMatrix.SetValue((byte)i, (byte)j, 1);
                }
            }
        }
    }

    // DisplayC.pas:894
    // { The size of the largest rectangle of marked blocks with its top left corner at X, Y, grown
    //   first downwards (rows of shrinking width) and then to the right (columns of shrinking
    //   height); BW and BH get its size in blocks, the result is its area. }
    public virtual int GetInverseBlockSize(int X, int Y, Matrix matrix, ref int bw, ref int bh)
    {
        // P and Q are indexes into Matrix.FData.
        int p = X + (matrix.Width * Y);
        if (matrix.Data[p] != 0)
        {
            int h1 = 0;
            int q = p;
            int w1 = matrix.Width - X;
            int size1 = 0;
            int n;
            int l;
            bool ok;
            do
            {
                n = w1;
                l = 0;
                while (n != 0 && matrix.Data[q + l] != 0)
                {
                    l += 1;
                    n -= 1;
                }

                ok = l * h1 >= size1;
                if (ok)
                {
                    h1 += 1;
                    q += matrix.Width;
                    w1 = l;
                    size1 = h1 * l;
                }
            }
            while (!(!ok || h1 + Y == matrix.Height));
            bh = h1;
            bw = w1;
            int w2 = 0;
            q = p;
            int h2 = matrix.Height - Y;
            int size2 = 0;
            do
            {
                n = h2;
                l = 0;
                int off = 0;
                while (n != 0 && matrix.Data[q + off] != 0)
                {
                    l += 1;
                    n -= 1;
                    off += matrix.Width;
                }

                ok = l * w2 >= size2;
                if (ok)
                {
                    q += 1;
                    w2 += 1;
                    h2 = l;
                    size2 = w2 * h2;
                }
            }
            while (!(!ok || w2 + X == matrix.Width));
            if (size2 > size1)
            {
                bw = w2;
                bh = h2;
                return size2;
            }
            else
            {
                return size1;
            }
        }
        else
        {
            return 0;
        }
    }

    // DisplayC.pas:964
    public virtual void GetPixelPositionOfBlock(int bx, int by, Rect R, out int pixelX, out int pixelY, out int bw, out int bh)
    {
        int w = (R.Right - R.Left + _inverseMatrix.Width - 1) / _inverseMatrix.Width;
        int h = (R.Bottom - R.Top + _inverseMatrix.Height - 1) / _inverseMatrix.Height;
        pixelX = w * bx;
        pixelY = h * by;
        bw = w;
        bh = h;
    }

    // DisplayC.pas:976
    public virtual void GetBlockPositionOfPixel(int pixelX, int pixelY, Rect R, out int bx, out int by)
    {
        int w = (R.Right - R.Left + 1 + _inverseMatrix.Width - 1) / _inverseMatrix.Width;
        int h = (R.Bottom - R.Top + 1 + _inverseMatrix.Height - 1) / _inverseMatrix.Height;
        bx = (pixelX - R.Left) / w;
        by = (pixelY - R.Top) / h;
    }

    // DisplayC.pas:988
    // { Marks the blocks of the pixel rectangle X, Y, W, H with Value. Bug kept from the original:
    //   the range test checks BY2 >= 0 where BX2 >= 0 was meant (and BY1 twice). }
    public void InverseWndwithValue(int X, int Y, int W, int H, byte value)
    {
        Rect r = default;
        if (_inverseMatrix is not null)
        {
            Matrix m = _inverseMatrix;
            r.Left = 0;
            r.Right = GetSurface(1).Width;
            r.Top = 0;
            r.Bottom = GetSurface(1).Height;
            GetBlockPositionOfPixel(X, Y, r, out int bx1, out int by1);
            GetBlockPositionOfPixel(X + W, Y + H, r, out int bx2, out int by2);
            if (m.Width > bx1 && m.Height > by1 && by2 >= 0 && by1 >= 0)
            {
                if (bx1 < 0)
                {
                    bx1 = 0;
                }

                if (by1 < 0)
                {
                    by1 = 0;
                }

                if (m.Width <= bx2)
                {
                    bx2 = m.Width - 1;
                }

                if (m.Height <= by2)
                {
                    by2 = m.Height - 1;
                }

                X = bx1;
                int last = bx2;
                for (; X <= last; X += 1)
                {
                    Y = by1;
                    int limit = by2;
                    for (; Y <= limit; Y += 1)
                    {
                        m.SetValue((byte)X, (byte)Y, value);
                    }
                }
            }
        }
    }

    // DisplayC.pas:1020
    public virtual void Invalidate(int X, int Y, int W, int H) => InverseWndwithValue(X, Y, W, H, 1);

    // DisplayC.pas:1043
    public int CurrentFrameRate => _display.CurrentFrameRate;

    // DisplayC.pas:1049
    // { Counts frames; every 5 frames the frame rate is measured. }
    public virtual void NewFrame()
    {
        if (_frameCount >= 5)
        {
            int t = PixelFormats.ElapsedMilliseconds;
            _display.CurrentFrameRate = Round((double)5000 / (t - _lastFrameTime + 1));
            _frameCount = 0;
            _lastFrameTime = t;
        }

        _frameCount = (ushort)(_frameCount + 1);
    }

    // DisplayC.pas:1063
    public virtual void ShowFrameRate()
    {
    }
}
