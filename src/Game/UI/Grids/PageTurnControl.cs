// Translated from PageTurnCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Game.Draw;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

public partial class PageTurnControl
{
    public float Scale;
    public float Shade;
    public float HalfWidth;
    public int Speed;
    public PageTurnMode Mode;
    public int Width;
    public int Height;
    public LockedSurface Source1;
    public int SourceX1;
    public int SourceY1;
    public LockedSurface Source2;
    public int SourceX2;
    public int SourceY2;
    public SingleArray512 InverseCos;
    public float Radius;
    public float Shadow;
    public int Position;

    // PageTurnCtrl.pas:51
    public PageTurnControl()
    {
        Scale = (float)0.5;
        Speed = 2;
        Shade = (float)2;
        GenerateInverseCosTable();
    }

    // PageTurnCtrl.pas:60
    public void SetPageDimensions(int width, int height)
    {
        Width = width;
        Height = height;
        HalfWidth = (int)(((uint)Width) >> 1);
    }

    // PageTurnCtrl.pas:67
    public float GetInverseCos(float value)
    {
        float result;
        Assert((double)value >= -1 && (double)value <= 1, "", "PageTurnCtrl.pas", 85);
        if ((double)value >= 0)
        {
            result = InverseCos[Round((double)value * 511)];
        }
        else
        {
            var pi = Math.PI;
            result = (float)((double)pi - InverseCos[Round((double)-value * 511)]);
        }

        return result;
    }

    // PageTurnCtrl.pas:90
    public void GenerateInverseCosTable()
    {
        float cosValue;
        float angle;
        float target;
        int I;
        angle = 0;
        I = 511;
        while ((double)angle < (double)Math.PI / 2)
        {
            cosValue = (float)Math.Cos(angle);
            target = (float)((double)I / 511);
            if ((double)((double)cosValue - target) > 0)
            {
                InverseCos[I] = angle;
            }
            else
            {
                I -= 1;
            }

            angle = (float)((double)angle + 1E-05);
        }
    }

    // PageTurnCtrl.pas:119
    public void BlitVerticalScanLine(int sourceX, int destinationX, int destinationY, in LockedSurface destination, int top, int bottom)
    {
        int sourceY;
        int height;
        int Y;
        Span<byte> source;
        int sourceP;
        int sourcePitch;
        if (destination.ClipRectangle.Left <= destinationX && destinationX < destination.ClipRectangle.Right)
        {
            sourceY = top;
            Y = destinationY + top;
            height = bottom - top;
            if (Y < destination.ClipRectangle.Top)
            {
                height -= destination.ClipRectangle.Top - Y;
                sourceY += destination.ClipRectangle.Top - Y;
                Y = destination.ClipRectangle.Top;
            }

            if (height + Y > destination.ClipRectangle.Bottom)
            {
                height -= height + Y - destination.ClipRectangle.Bottom;
            }

            if (height > 0)
            {
                // The pointers of the original are byte indexes into the surfaces' memory.
                if (sourceX < Width * 2)
                {
                    source = Source1.Data;
                    sourceP = Source1.DataIndex + (SourceX1 + sourceX) * 2 + (SourceY1 + sourceY) * Source1.Pitch;
                    sourcePitch = Source1.Pitch;
                }
                else
                {
                    source = Source2.Data;
                    sourceP = Source2.DataIndex + (sourceX - Width * 2 + SourceX2) * 2 + (SourceY2 + sourceY) * Source2.Pitch;
                    sourcePitch = Source2.Pitch;
                }

                var destinationLocal = destination.Data;
                int destinationP = destination.DataIndex + destinationX * 2 + destination.Pitch * Y;
                BlitColumn(source, sourceP, sourcePitch, destinationLocal, destinationP, destination.Pitch, height);
            }
        }
    }

    // PageTurnCtrl.pas:151
    public void Start(PageTurnMode mode)
    {
        Mode = mode;
        switch (Mode)
        {
            case PageTurnMode.Left:
                Radius = HalfWidth;
                Shadow = (float)((double)Radius * 0.5);
                Position = Width;
                break;
            case PageTurnMode.Right:
                Radius = 1;
                Shadow = (float)((double)Radius * 0.5);
                Position = 0;
                break;
            case PageTurnMode.Open:
                Position = 0;
                break;
            case PageTurnMode.Close:
                Position = 180;
                break;
        }
    }

    // PageTurnCtrl.pas:164
    public bool UpdateScene()
    {
        bool result;
        switch (Mode)
        {
            case PageTurnMode.Left:
                Shade = (float)3.5;
                if (Position > 0)
                {
                    Position -= Speed;
                    if (Position < 0)
                    {
                        Position = 0;
                    }
                }
                else
                {
                    if ((double)Radius > 0)
                    {
                        var pi = Math.PI;
                        Radius = (float)((double)Radius - (double)((double)((double)Speed * 1.25) * 2) / pi);
                        if ((double)Radius < 1)
                        {
                            Radius = 0;
                        }

                        Shadow = (float)((double)Radius * 0.5);
                    }
                }

                result = (double)Radius >= 1;
                break;
            case PageTurnMode.Right:
                Shade = (float)2;
                if ((double)Radius < HalfWidth)
                {
                    var pi = Math.PI;
                    Radius = (float)((double)Radius + (double)((double)((double)Speed * 1.5) * 2) / pi);
                    if ((double)Radius > HalfWidth)
                    {
                        Radius = HalfWidth;
                    }

                    Shadow = (float)((double)Radius * 0.5);
                }
                else
                {
                    Position += Speed;
                    if (Position > Width)
                    {
                        Position = Width;
                    }
                }

                result = Position < Width;
                break;
            case PageTurnMode.Open:
                Position += Speed;
                if (Position > 180)
                {
                    Position = 180;
                }

                result = Position < 180;
                break;
            case PageTurnMode.Close:
                Position -= Speed;
                if (Position < 0)
                {
                    Position = 0;
                }

                result = Position > 0;
                break;
            default:
                result = false;
                break;
        }

        return result;
    }

    // PageTurnCtrl.pas:215
    public void RenderLeftRightScene(int X, int Y, in LockedSurface destination)
    {
        int originalLength;
        int offset;
        int baseX;
        int span;
        float step;
        float angle;
        float cosValue;
        float height;
        float width;
        float radiusFactor;
        int arcLength;
        int sourceBase;
        int limit;
        Rect R = default;
        int sourceOffset;
        int I;
        if ((double)Radius >= 1)
        {
            height = (float)((double)Shadow * Scale);
            width = (float)((double)Radius * Scale);
            R.Left = SourceX1;
            R.Top = SourceY1;
            R.Right = SourceX1 + Width + Position;
            R.Bottom = SourceY1 + Height;
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(Source1, R, destination, X, Y);
            R.Left = SourceX2 + Width + Position;
            R.Top = SourceY2;
            R.Right = Width * 2 + SourceX2;
            R.Bottom = SourceY2 + Height;
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(Source2, R, destination, X + Width + Position, Y);
            var pi = Math.PI;
            originalLength = Round((double)((double)pi * Radius) * 0.25);
            var mathPI = Math.PI;
            arcLength = Round((double)((double)mathPI * Radius) * 0.25);
            sourceBase = Width + Position;
            radiusFactor = (float)((double)((double)2 / Math.PI) * arcLength);
            baseX = X + Width + Position;
            span = Round(Shadow);
            limit = Width - Position;
            step = (float)((double)1 / Shadow);
            cosValue = 0;
            sourceOffset = 0;
            I = 0;
            while (sourceOffset < limit && I < span)
            {
                angle = GetInverseCos(cosValue);
                sourceOffset = arcLength - Round((double)radiusFactor * angle);
                offset = Round((double)height - (double)Math.Sin(angle) * height);
                if (sourceOffset < limit)
                {
                    if (arcLength - sourceOffset + arcLength < limit)
                    {
                        BlitVerticalScanLine(sourceBase + sourceOffset, X + Width + Position + I, Y - offset, destination, Height - Round(width), Height);
                    }
                    else
                    {
                        BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, Height);
                    }
                }

                cosValue = (float)((double)cosValue + step);
                I += 1;
            }

            var pi2 = Math.PI;
            arcLength = Round((double)((double)pi2 * Radius) * 0.25);
            sourceBase = Width * 3 - Position - arcLength;
            radiusFactor = (float)((double)((double)2 / Math.PI) * arcLength);
            baseX = X + Width + Position;
            span = Round(Shadow);
            I = span;
            cosValue = 1;
            limit = Width * 2 - sourceBase;
            while (sourceOffset >= limit && I >= 0)
            {
                angle = GetInverseCos(cosValue);
                sourceOffset = -Round((double)radiusFactor * angle);
                offset = Round((double)((double)Math.Sin(angle) * height) + height);
                if (sourceOffset >= limit)
                {
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, Height);
                }

                cosValue = (float)((double)cosValue - step);
                I -= 1;
            }

            var pi3 = Math.PI;
            arcLength = Round((double)((double)((double)pi3 * Radius) * 0.25) * Shade);
            sourceBase = Width * 3 - Position - originalLength * 2 - arcLength;
            span = Round((double)Shadow * Shade);
            baseX = X + Width + Position - span;
            height = (float)((double)((double)Shadow * Scale) * 3.4142);
            width = (float)((double)((double)Radius * Scale) * 0.707);
            limit = Width * 2 - sourceBase;
            sourceOffset = Round(arcLength);
            I = span;
            cosValue = 0;
            step = (float)((double)0.707 / ((double)Shadow * Shade));
            if ((double)step > 1)
            {
                step = (float)1;
            }

            while (sourceOffset >= limit && I >= 0)
            {
                angle = GetInverseCos(cosValue);
                var pi4 = Math.PI;
                var pi5 = Math.PI;
                sourceOffset = Round((double)((double)arcLength * ((double)angle - (double)pi4 / 4)) * ((double)4 / pi5));
                offset = Round((double)((double)Math.Sin(angle) * height) - width);
                if (sourceOffset >= limit)
                {
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, Height);
                }

                cosValue = (float)((double)cosValue + step);
                I -= 1;
            }

            baseX = X + Width + Position - span * 2;
            sourceOffset = 0;
            I = span;
            while (sourceOffset >= limit && I >= 0 && (double)cosValue > 0)
            {
                cosValue = (float)((double)cosValue - step);
                angle = GetInverseCos(cosValue);
                var pi4 = Math.PI;
                var pi5 = Math.PI;
                sourceOffset = -Round((double)((double)arcLength * ((double)4 / pi4)) * ((double)angle - (double)pi5 / 4));
                offset = Round((double)((double)1 - Math.Sin(angle)) * height);
                if (sourceOffset >= limit)
                {
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, Height);
                }

                I -= 1;
            }

            R.Left = SourceX2;
            R.Top = SourceY2;
            R.Right = SourceX2 + Width - Position - Round((double)originalLength * ((double)((double)2 * Shade) + 2));
            R.Bottom = SourceY2 + Height;
            if (R.Right > R.Left)
            {
                LockedSurface.BlitSurfaceDescriptionRectangleClipped(Source2, R, destination, baseX - (R.Right - R.Left), Y);
            }
        }
    }

    // PageTurnCtrl.pas:324
    public void RenderOpenCloseScene(int X, int Y, in LockedSurface destination)
    {
        int baseX;
        int height;
        int width;
        float step;
        float sourceX;
        int offset;
        Rect R = default;
        int pageWidth;
        R.Left = SourceX2 + Width;
        R.Top = SourceY2;
        R.Right = SourceX2 + Width * 2;
        R.Bottom = SourceY2 + Height;
        LockedSurface.BlitSurfaceDescriptionRectangleClipped(Source2, R, destination, X + Width, Y);
        var pi = Math.PI;
        height = Round((double)Math.Sin((double)((double)Position * pi) / 180) * Width);
        var mathPI = Math.PI;
        width = Round((double)Math.Cos((double)((double)Position * mathPI) / 180) * Width);
        if (width > 0)
        {
            step = (float)((double)Width / width);
            baseX = X + Width;
            pageWidth = Width;
            sourceX = pageWidth;
            for (int I = 0; I <= width - 1; I++)
            {
                offset = Round((double)((double)((double)I / width) * height) * Scale);
                BlitVerticalScanLine(Round(sourceX), baseX + I, Y - offset, destination, 0, Height);
                sourceX = (float)((double)sourceX + step);
            }
        }
        else
        {
            if (width < 0)
            {
                width = -width;
                step = (float)((double)Width / width);
                baseX = X + Width;
                sourceX = Width * 3;
                for (int I = 0; I <= width - 1; I++)
                {
                    offset = Round((double)((double)((double)I / width) * height) * Scale);
                    BlitVerticalScanLine(Round(sourceX), baseX - I, Y - offset, destination, 0, Height);
                    sourceX = (float)((double)sourceX - step);
                }
            }
        }
    }

    // PageTurnCtrl.pas:361
    public void RenderScene(int X, int Y, in LockedSurface destination, int x1, int y1, in LockedSurface source1, int x2, int y2, in LockedSurface source2, ref Rect R)
    {
        switch (Mode)
        {
            case PageTurnMode.Left:
                Source1 = source1;
                SourceX1 = x1;
                SourceY1 = y1;
                Source2 = source2;
                SourceX2 = x2;
                SourceY2 = y2;
                RenderLeftRightScene(X, Y, destination);
                R.Left = X;
                R.Right = X + Width * 2;
                R.Top = Y - (Round((double)Scale * Radius) + 1);
                R.Bottom = Y + Height;
                break;
            case PageTurnMode.Right:
                Source1 = source2;
                SourceX1 = x2;
                SourceY1 = y2;
                Source2 = source1;
                SourceX2 = x1;
                SourceY2 = y1;
                RenderLeftRightScene(X, Y, destination);
                R.Left = X;
                R.Right = X + Width * 2;
                R.Top = Y - (Round((double)Scale * Radius) + 1);
                R.Bottom = Y + Height;
                break;
            case PageTurnMode.Open:
                Source1 = source1;
                SourceX1 = x1;
                SourceY1 = y1;
                Source2 = source2;
                SourceX2 = x2;
                SourceY2 = y2;
                RenderOpenCloseScene(X, Y, destination);
                R.Left = X;
                R.Right = X + Width * 2;
                var pi = Math.PI;
                var sin = Math.Sin((double)((double)Position * pi) / 180);
                R.Top = Y - Round((double)((double)((double)sin * Scale) * Width) + 1);
                R.Bottom = Y + Height;
                break;
            case PageTurnMode.Close:
                Source1 = source2;
                SourceX1 = x2;
                SourceY1 = y2;
                Source2 = source1;
                SourceX2 = x1;
                SourceY2 = y1;
                RenderOpenCloseScene(X, Y, destination);
                R.Left = X;
                R.Right = X + Width * 2;
                var mathPI = Math.PI;
                var mathSin = Math.Sin((double)((double)Position * mathPI) / 180);
                R.Top = Y - Round((double)((double)((double)mathSin * Scale) * Width) + 1);
                R.Bottom = Y + Height;
                break;
        }
    }

    private static void BlitColumn(ReadOnlySpan<byte> sourceMemory, int source, int sourcePitch, Span<byte> destinationMemory, int destination, int destinationPitch, int height)
    {
        while (height > 0)
        {
            ushort number = MemoryMarshal.Read<ushort>(sourceMemory.Slice(source, 2));
            MemoryMarshal.Write<ushort>(destinationMemory.Slice(destination, 2), in number);
            source += sourcePitch;
            destination += destinationPitch;
            height--;
        }
    }

}
