
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class Picture16
{
    public override void ShowOpaqueStretched(in Rect R, in LockedSurface lockedSurface)
    {
        Span<int> rowTable;
        Span<int> columnTable;
        int w;
        SurfaceView destination;
        int xPosition;
        int yPosition;
        int xStep;
        int yStep;
        int sx;
        int sy;
        float xScale;
        float yScale;
        int h;
        int l;
        int t;
        int rt;
        int b;
        int c;
        int d;
        w = R.Right - R.Left;
        h = R.Bottom - R.Top;
        if (w > 0 && h > 0)
        {
            xScale = (float)((double)w / DataWidth);
            yScale = (float)((double)h / Height);
            l = R.Left;
            if (l < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - l;
                l += sx;
            }
            else
            {
                sx = 0;
            }

            t = R.Top;
            if (t < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - t;
                t += sy;
            }
            else
            {
                sy = 0;
            }

            rt = R.Right;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                d = rt - lockedSurface.ClipRectangle.Right;
                rt -= d;
            }

            b = R.Bottom;
            if (b > lockedSurface.ClipRectangle.Bottom)
            {
                d = b - lockedSurface.ClipRectangle.Bottom;
                b -= d;
            }

            w = rt - l;
            h = b - t;
            if (w > 0 && h > 0)
            {
                destination = lockedSurface.View((l * 2) + (lockedSurface.Pitch * t));
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[w + h];
                rowTable = tables.AsSpan(0, h);
                columnTable = tables.AsSpan(h, w);
                xStep = RtlUnit.Round((double)65535 / xScale);
                xPosition = sx != 0 ? RtlUnit.Round((double)((double)sx / xScale) * 65535) : 0;

                for (int i = 0; i <= w - 1; i++)
                {
                    c = (int)(((uint)xPosition) >> 16);
                    if (c >= DataWidth)
                    {
                        c = DataWidth - 1;
                    }

                    columnTable[i] = c;
                    xPosition += xStep;
                }

                yStep = RtlUnit.Round((double)65535 / yScale);
                yPosition = sy != 0 ? RtlUnit.Round((double)((double)sy / yScale) * 65535) : 0;

                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int i = 0; i <= h - 1; i++)
                    {
                        c = (int)(((uint)yPosition) >> 16);
                        if (c >= DataHeight)
                        {
                            c = DataHeight - 1;
                        }

                        rowTable[i] = c * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitStretched16(View, rowTable, columnTable, destination, w, h);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitStretched16(View, rowTable, columnTable, destination, w, h);
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    for (int i = 0; i <= h - 1; i++)
                    {
                        c = (int)(((uint)yPosition) >> 16);
                        if (c >= DataHeight)
                        {
                            c = DataHeight - 1;
                        }

                        rowTable[i] = c * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        BlitStretched16(View, rowTable, columnTable, destination, w, h);
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        BlitStretched16(View, rowTable, columnTable, destination, w, h);
                    }
                }
            }
        }
    }

    public override void ShowBlendedStretched(in Rect R, in LockedSurface lockedSurface, BlendTable table)
    {
        Span<int> rowTable;
        Span<int> columnTable;
        int w;
        int h;
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
        int l;
        int t;
        int b;
        int c;
        int d;
        w = R.Right - R.Left;
        h = R.Bottom - R.Top;
        if (w > 0 && h > 0)
        {
            xScale = (float)((double)w / DataWidth);
            yScale = (float)((double)h / Height);
            l = R.Left;
            if (l < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - l;
                l += sx;
            }
            else
            {
                sx = 0;
            }

            t = R.Top;
            if (t < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - t;
                t += sy;
            }
            else
            {
                sy = 0;
            }

            rt = R.Right;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                d = rt - lockedSurface.ClipRectangle.Right;
                rt -= d;
            }

            b = R.Bottom;
            if (b > lockedSurface.ClipRectangle.Bottom)
            {
                d = b - lockedSurface.ClipRectangle.Bottom;
                b -= d;
            }

            w = rt - l;
            h = b - t;
            if (w > 0 && h > 0)
            {
                destination = lockedSurface.View((l * 2) + (lockedSurface.Pitch * t));
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[w + h];
                rowTable = tables.AsSpan(0, h);
                columnTable = tables.AsSpan(h, w);
                xStep = RtlUnit.Round((double)65535 / xScale);
                xPosition = sx != 0 ? RtlUnit.Round((double)((double)sx / xScale) * 65535) : 0;

                for (int i = 0; i <= w - 1; i++)
                {
                    c = (int)(((uint)xPosition) >> 16);
                    if (c >= DataWidth)
                    {
                        c = DataWidth - 1;
                    }

                    columnTable[i] = c;
                    xPosition += xStep;
                }

                yStep = RtlUnit.Round((double)65535 / yScale);
                yPosition = sy != 0 ? RtlUnit.Round((double)((double)sy / yScale) * 65535) : 0;

                if (!DataLoaded)
                {
                    LoadDiscardableImageData();
                    for (int i = 0; i <= h - 1; i++)
                    {
                        c = (int)(((uint)yPosition) >> 16);
                        if (c >= DataHeight)
                        {
                            c = DataHeight - 1;
                        }

                        rowTable[i] = c * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendStretched555(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendStretched565(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendStretched555(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendStretched565(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }

                    if (LoadMode == LoadMode.LoadFreeOnDemand)
                    {
                        ReleaseDiscardableImageData();
                    }
                }
                else
                {
                    for (int i = 0; i <= h - 1; i++)
                    {
                        c = (int)(((uint)yPosition) >> 16);
                        if (c >= DataHeight)
                        {
                            c = DataHeight - 1;
                        }

                        rowTable[i] = c * _pitch;
                        yPosition += yStep;
                    }

                    if (lockedSurface.PixelFormatId == PixelFormatId)
                    {
                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendStretched555(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendStretched565(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                    else
                    {
                        if (RemapToPixelFormat(lockedSurface.PixelFormatId) != GraphicsResult.Ok)
                        {
                            throw new NotSupportedException("Surface pixel format not supported!");
                        }

                        switch (lockedSurface.PixelFormatId)
                        {
                            case PixelFormat.Rgb555:
                                BlendStretched555(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.Rgb565:
                                BlendStretched565(View, rowTable, columnTable, destination, w, h, table.Bytes);
                                break;
                            case PixelFormat.None:
                            case PixelFormat.Pal8:
                            case PixelFormat.Argb1555:
                            case PixelFormat.Rgb24:
                            case PixelFormat.Bgr24:
                            case PixelFormat.Rgb32:
                            case PixelFormat.Xrgb32:
                            default:
                                break;
                        }
                    }
                }
            }
        }
    }
}
