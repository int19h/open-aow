
using Aow.Graphics;
using Aow.Rtl;

namespace Aow.Game.Draw.Images;

public partial class Picture16
{
    public override void ShowOpaqueStretched(in Rect R, in LockedSurface lockedSurface)
    {
        Span<int> rowTable;
        Span<int> columnTable;
        int w = R.Right - R.Left;
        int h = R.Bottom - R.Top;
        if (w > 0 && h > 0)
        {
            float xScale = (float)((double)w / DataWidth);
            float yScale = (float)((double)h / Height);
            int l = R.Left;
            int sx;
            if (l < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - l;
                l += sx;
            }
            else
            {
                sx = 0;
            }

            int t = R.Top;
            int sy;
            if (t < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - t;
                t += sy;
            }
            else
            {
                sy = 0;
            }

            int rt = R.Right;
            int d;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                d = rt - lockedSurface.ClipRectangle.Right;
                rt -= d;
            }

            int b = R.Bottom;
            if (b > lockedSurface.ClipRectangle.Bottom)
            {
                d = b - lockedSurface.ClipRectangle.Bottom;
                b -= d;
            }

            w = rt - l;
            h = b - t;
            if (w > 0 && h > 0)
            {
                SurfaceView destination = lockedSurface.View((l * 2) + (lockedSurface.Pitch * t));
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[w + h];
                rowTable = tables.AsSpan(0, h);
                columnTable = tables.AsSpan(h, w);
                int xStep = DelphiRounding.Round((double)65535 / xScale);
                int xPosition = sx != 0 ? DelphiRounding.Round((double)((double)sx / xScale) * 65535) : 0;

                int c;
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

                int yStep = DelphiRounding.Round((double)65535 / yScale);
                int yPosition = sy != 0 ? DelphiRounding.Round((double)((double)sy / yScale) * 65535) : 0;

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
        int w = R.Right - R.Left;
        int h = R.Bottom - R.Top;
        if (w > 0 && h > 0)
        {
            float xScale = (float)((double)w / DataWidth);
            float yScale = (float)((double)h / Height);
            int l = R.Left;
            int sx;
            if (l < lockedSurface.ClipRectangle.Left)
            {
                sx = lockedSurface.ClipRectangle.Left - l;
                l += sx;
            }
            else
            {
                sx = 0;
            }

            int t = R.Top;
            int sy;
            if (t < lockedSurface.ClipRectangle.Top)
            {
                sy = lockedSurface.ClipRectangle.Top - t;
                t += sy;
            }
            else
            {
                sy = 0;
            }

            int rt = R.Right;
            int d;
            if (rt > lockedSurface.ClipRectangle.Right)
            {
                d = rt - lockedSurface.ClipRectangle.Right;
                rt -= d;
            }

            int b = R.Bottom;
            if (b > lockedSurface.ClipRectangle.Bottom)
            {
                d = b - lockedSurface.ClipRectangle.Bottom;
                b -= d;
            }

            w = rt - l;
            h = b - t;
            if (w > 0 && h > 0)
            {
                SurfaceView destination = lockedSurface.View((l * 2) + (lockedSurface.Pitch * t));
                // One table of H row offsets followed by W columns, as the original's GetMem.
                int[] tables = new int[w + h];
                rowTable = tables.AsSpan(0, h);
                columnTable = tables.AsSpan(h, w);
                int xStep = DelphiRounding.Round((double)65535 / xScale);
                int xPosition = sx != 0 ? DelphiRounding.Round((double)((double)sx / xScale) * 65535) : 0;

                int c;
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

                int yStep = DelphiRounding.Round((double)65535 / yScale);
                int yPosition = sy != 0 ? DelphiRounding.Round((double)((double)sy / yScale) * 65535) : 0;

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
