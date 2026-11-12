
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class Picture16
{
    public override void ShowOpaqueStretched(in Rect R, in LockedSurface lockedSurface)
    {
        Span<int> rowTable;
        Span<int> columnTable;
        int W;
        SurfaceView destination;
        int xPosition;
        int yPosition;
        int xStep;
        int yStep;
        int sx;
        int sy;
        float xScale;
        float yScale;
        int H;
        int L;
        int T;
        int rt;
        int B;
        int C;
        int D;
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        if (W > 0 && H > 0)
        {
            xScale = (float)((double)W / _dataWidth);
            yScale = (float)((double)H / _height);
            L = R.Left;
            if (L < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - L;
                L += sx;
            }
            else
            {
                sx = 0;
            }

            T = R.Top;
            if (T < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - T;
                T += sy;
            }
            else
            {
                sy = 0;
            }

            rt = R.Right;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                D = rt - lockedSurface.ClipRectangle.Right;
                rt -= D;
            }

            B = R.Bottom;
            if (B > lockedSurface.ClipRectangle.Bottom)
            {
                D = B - lockedSurface.ClipRectangle.Bottom;
                B -= D;
            }

            W = rt - L;
            H = B - T;
            if (W > 0 && H > 0)
            {
                destination = lockedSurface.View(L * 2 + lockedSurface.Pitch * T);
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[W + H];
                rowTable = tables.AsSpan(0, H);
                columnTable = tables.AsSpan(H, W);
                xStep = RtlUnit.Round((double)65535 / xScale);
                if (sx != 0)
                {
                    xPosition = RtlUnit.Round((double)((double)sx / xScale) * 65535);
                }
                else
                {
                    xPosition = 0;
                }

                for (int I = 0; I <= W - 1; I++)
                {
                    C = (int)(((uint)xPosition) >> 16);
                    if (C >= _dataWidth)
                    {
                        C = _dataWidth - 1;
                    }

                    columnTable[I] = C;
                    xPosition += xStep;
                }

                yStep = RtlUnit.Round((double)65535 / yScale);
                if (sy != 0)
                {
                    yPosition = RtlUnit.Round((double)((double)sy / yScale) * 65535);
                }
                else
                {
                    yPosition = 0;
                }

                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)yPosition) >> 16);
                        if (C >= _dataHeight)
                        {
                            C = _dataHeight - 1;
                        }

                        rowTable[I] = C * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitStretched16(View, rowTable, columnTable, destination, W, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitStretched16(View, rowTable, columnTable, destination, W, H);
                    }

                    if (_loadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)yPosition) >> 16);
                        if (C >= _dataHeight)
                        {
                            C = _dataHeight - 1;
                        }

                        rowTable[I] = C * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitStretched16(View, rowTable, columnTable, destination, W, H);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitStretched16(View, rowTable, columnTable, destination, W, H);
                    }
                }
            }
        }
    }

    public override void ShowBlendedStretched(in Rect R, in LockedSurface lockedSurface, BlendTable table)
    {
        Span<int> rowTable;
        Span<int> columnTable;
        int W;
        int H;
        int rt;
        SurfaceView destination;
        int xPosition;
        int yPosition;
        int xStep;
        int yStep;
        int sx;
        int sy;
        float xScale;
        float yScale;
        int L;
        int T;
        int B;
        int C;
        int D;
        W = R.Right - R.Left;
        H = R.Bottom - R.Top;
        if (W > 0 && H > 0)
        {
            xScale = (float)((double)W / _dataWidth);
            yScale = (float)((double)H / _height);
            L = R.Left;
            if (L < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - L;
                L += sx;
            }
            else
            {
                sx = 0;
            }

            T = R.Top;
            if (T < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - T;
                T += sy;
            }
            else
            {
                sy = 0;
            }

            rt = R.Right;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                D = rt - lockedSurface.ClipRectangle.Right;
                rt -= D;
            }

            B = R.Bottom;
            if (B > lockedSurface.ClipRectangle.Bottom)
            {
                D = B - lockedSurface.ClipRectangle.Bottom;
                B -= D;
            }

            W = rt - L;
            H = B - T;
            if (W > 0 && H > 0)
            {
                destination = lockedSurface.View(L * 2 + lockedSurface.Pitch * T);
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[W + H];
                rowTable = tables.AsSpan(0, H);
                columnTable = tables.AsSpan(H, W);
                xStep = RtlUnit.Round((double)65535 / xScale);
                if (sx != 0)
                {
                    xPosition = RtlUnit.Round((double)((double)sx / xScale) * 65535);
                }
                else
                {
                    xPosition = 0;
                }

                for (int I = 0; I <= W - 1; I++)
                {
                    C = (int)(((uint)xPosition) >> 16);
                    if (C >= _dataWidth)
                    {
                        C = _dataWidth - 1;
                    }

                    columnTable[I] = C;
                    xPosition += xStep;
                }

                yStep = RtlUnit.Round((double)65535 / yScale);
                if (sy != 0)
                {
                    yPosition = RtlUnit.Round((double)((double)sy / yScale) * 65535);
                }
                else
                {
                    yPosition = 0;
                }

                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)yPosition) >> 16);
                        if (C >= _dataHeight)
                        {
                            C = _dataHeight - 1;
                        }

                        rowTable[I] = C * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendStretched555(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendStretched565(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendStretched555(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendStretched565(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }

                    if (_loadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    for (int I = 0; I <= H - 1; I++)
                    {
                        C = (int)(((uint)yPosition) >> 16);
                        if (C >= _dataHeight)
                        {
                            C = _dataHeight - 1;
                        }

                        rowTable[I] = C * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendStretched555(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendStretched565(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != 0)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormats.pfRGB555:
                                BlendStretched555(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                            case PixelFormats.pfRGB565:
                                BlendStretched565(View, rowTable, columnTable, destination, W, H, ((BlendTable5Bit)table).Bytes);
                                break;
                        }
                    }
                }
            }
        }
    }
}
