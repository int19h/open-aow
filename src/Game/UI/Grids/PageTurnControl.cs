// Translated from PageTurnCtrl.pas.
using System.Runtime.InteropServices;
using Aow.Engine.Buffers;
using Aow.Game.Draw;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.UI.Grids;

public partial class PageTurnControl
{
    private readonly float _scale;
    private float _shade;
    private float _halfWidth;

    public int Speed { get; set; }

    private PageTurnMode _mode;
    private int _width;
    private int _height;
    private LockedSurface _source1;
    private int _sourceX1;
    private int _sourceY1;
    private LockedSurface _source2;
    private int _sourceX2;
    private int _sourceY2;
    private readonly float[] _inverseCos = new float[512];
    private float _radius;
    private float _shadow;
    private int _position;

    // PageTurnCtrl.pas:51
    public PageTurnControl()
    {
        _scale = (float)0.5;
        Speed = 2;
        _shade = (float)2;
        GenerateInverseCosTable();
    }

    // PageTurnCtrl.pas:60
    public void SetPageDimensions(int width, int height)
    {
        _width = width;
        _height = height;
        _halfWidth = (int)(((uint)_width) >> 1);
    }

    // PageTurnCtrl.pas:67
    public float GetInverseCos(float value)
    {
        float result;
        Assert((double)value >= -1 && (double)value <= 1, "", "PageTurnCtrl.pas", 85);
        if ((double)value >= 0)
        {
            result = _inverseCos[Round((double)value * 511)];
        }
        else
        {
            var pi = Math.PI;
            result = (float)((double)pi - _inverseCos[Round((double)-value * 511)]);
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
                _inverseCos[I] = angle;
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
                if (sourceX < _width * 2)
                {
                    source = _source1.Data;
                    sourceP = _source1.DataIndex + (_sourceX1 + sourceX) * 2 + (_sourceY1 + sourceY) * _source1.Pitch;
                    sourcePitch = _source1.Pitch;
                }
                else
                {
                    source = _source2.Data;
                    sourceP = _source2.DataIndex + (sourceX - _width * 2 + _sourceX2) * 2 + (_sourceY2 + sourceY) * _source2.Pitch;
                    sourcePitch = _source2.Pitch;
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
        _mode = mode;
        switch (_mode)
        {
            case PageTurnMode.Left:
                _radius = _halfWidth;
                _shadow = (float)((double)_radius * 0.5);
                _position = _width;
                break;
            case PageTurnMode.Right:
                _radius = 1;
                _shadow = (float)((double)_radius * 0.5);
                _position = 0;
                break;
            case PageTurnMode.Open:
                _position = 0;
                break;
            case PageTurnMode.Close:
                _position = 180;
                break;
        }
    }

    // PageTurnCtrl.pas:164
    public bool UpdateScene()
    {
        bool result;
        switch (_mode)
        {
            case PageTurnMode.Left:
                _shade = (float)3.5;
                if (_position > 0)
                {
                    _position -= Speed;
                    if (_position < 0)
                    {
                        _position = 0;
                    }
                }
                else
                {
                    if ((double)_radius > 0)
                    {
                        var pi = Math.PI;
                        _radius = (float)((double)_radius - (double)((double)((double)Speed * 1.25) * 2) / pi);
                        if ((double)_radius < 1)
                        {
                            _radius = 0;
                        }

                        _shadow = (float)((double)_radius * 0.5);
                    }
                }

                result = (double)_radius >= 1;
                break;
            case PageTurnMode.Right:
                _shade = (float)2;
                if ((double)_radius < _halfWidth)
                {
                    var pi = Math.PI;
                    _radius = (float)((double)_radius + (double)((double)((double)Speed * 1.5) * 2) / pi);
                    if ((double)_radius > _halfWidth)
                    {
                        _radius = _halfWidth;
                    }

                    _shadow = (float)((double)_radius * 0.5);
                }
                else
                {
                    _position += Speed;
                    if (_position > _width)
                    {
                        _position = _width;
                    }
                }

                result = _position < _width;
                break;
            case PageTurnMode.Open:
                _position += Speed;
                if (_position > 180)
                {
                    _position = 180;
                }

                result = _position < 180;
                break;
            case PageTurnMode.Close:
                _position -= Speed;
                if (_position < 0)
                {
                    _position = 0;
                }

                result = _position > 0;
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
        if ((double)_radius >= 1)
        {
            height = (float)((double)_shadow * _scale);
            width = (float)((double)_radius * _scale);
            R.Left = _sourceX1;
            R.Top = _sourceY1;
            R.Right = _sourceX1 + _width + _position;
            R.Bottom = _sourceY1 + _height;
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(_source1, R, destination, X, Y);
            R.Left = _sourceX2 + _width + _position;
            R.Top = _sourceY2;
            R.Right = _width * 2 + _sourceX2;
            R.Bottom = _sourceY2 + _height;
            LockedSurface.BlitSurfaceDescriptionRectangleClipped(_source2, R, destination, X + _width + _position, Y);
            var pi = Math.PI;
            originalLength = Round((double)((double)pi * _radius) * 0.25);
            var mathPI = Math.PI;
            arcLength = Round((double)((double)mathPI * _radius) * 0.25);
            sourceBase = _width + _position;
            radiusFactor = (float)((double)((double)2 / Math.PI) * arcLength);
            baseX = X + _width + _position;
            span = Round(_shadow);
            limit = _width - _position;
            step = (float)((double)1 / _shadow);
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
                        BlitVerticalScanLine(sourceBase + sourceOffset, X + _width + _position + I, Y - offset, destination, _height - Round(width), _height);
                    }
                    else
                    {
                        BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, _height);
                    }
                }

                cosValue = (float)((double)cosValue + step);
                I += 1;
            }

            var pi2 = Math.PI;
            arcLength = Round((double)((double)pi2 * _radius) * 0.25);
            sourceBase = _width * 3 - _position - arcLength;
            radiusFactor = (float)((double)((double)2 / Math.PI) * arcLength);
            baseX = X + _width + _position;
            span = Round(_shadow);
            I = span;
            cosValue = 1;
            limit = _width * 2 - sourceBase;
            while (sourceOffset >= limit && I >= 0)
            {
                angle = GetInverseCos(cosValue);
                sourceOffset = -Round((double)radiusFactor * angle);
                offset = Round((double)((double)Math.Sin(angle) * height) + height);
                if (sourceOffset >= limit)
                {
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, _height);
                }

                cosValue = (float)((double)cosValue - step);
                I -= 1;
            }

            var pi3 = Math.PI;
            arcLength = Round((double)((double)((double)pi3 * _radius) * 0.25) * _shade);
            sourceBase = _width * 3 - _position - originalLength * 2 - arcLength;
            span = Round((double)_shadow * _shade);
            baseX = X + _width + _position - span;
            height = (float)((double)((double)_shadow * _scale) * 3.4142);
            width = (float)((double)((double)_radius * _scale) * 0.707);
            limit = _width * 2 - sourceBase;
            sourceOffset = Round(arcLength);
            I = span;
            cosValue = 0;
            step = (float)((double)0.707 / ((double)_shadow * _shade));
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
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, _height);
                }

                cosValue = (float)((double)cosValue + step);
                I -= 1;
            }

            baseX = X + _width + _position - span * 2;
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
                    BlitVerticalScanLine(sourceBase + sourceOffset, baseX + I, Y - offset, destination, 0, _height);
                }

                I -= 1;
            }

            R.Left = _sourceX2;
            R.Top = _sourceY2;
            R.Right = _sourceX2 + _width - _position - Round((double)originalLength * ((double)((double)2 * _shade) + 2));
            R.Bottom = _sourceY2 + _height;
            if (R.Right > R.Left)
            {
                LockedSurface.BlitSurfaceDescriptionRectangleClipped(_source2, R, destination, baseX - (R.Right - R.Left), Y);
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
        R.Left = _sourceX2 + _width;
        R.Top = _sourceY2;
        R.Right = _sourceX2 + _width * 2;
        R.Bottom = _sourceY2 + _height;
        LockedSurface.BlitSurfaceDescriptionRectangleClipped(_source2, R, destination, X + _width, Y);
        var pi = Math.PI;
        height = Round((double)Math.Sin((double)((double)_position * pi) / 180) * _width);
        var mathPI = Math.PI;
        width = Round((double)Math.Cos((double)((double)_position * mathPI) / 180) * _width);
        if (width > 0)
        {
            step = (float)((double)_width / width);
            baseX = X + _width;
            pageWidth = _width;
            sourceX = pageWidth;
            for (int I = 0; I <= width - 1; I++)
            {
                offset = Round((double)((double)((double)I / width) * height) * _scale);
                BlitVerticalScanLine(Round(sourceX), baseX + I, Y - offset, destination, 0, _height);
                sourceX = (float)((double)sourceX + step);
            }
        }
        else
        {
            if (width < 0)
            {
                width = -width;
                step = (float)((double)_width / width);
                baseX = X + _width;
                sourceX = _width * 3;
                for (int I = 0; I <= width - 1; I++)
                {
                    offset = Round((double)((double)((double)I / width) * height) * _scale);
                    BlitVerticalScanLine(Round(sourceX), baseX - I, Y - offset, destination, 0, _height);
                    sourceX = (float)((double)sourceX - step);
                }
            }
        }
    }

    // PageTurnCtrl.pas:361
    public void RenderScene(int X, int Y, in LockedSurface destination, int x1, int y1, in LockedSurface source1, int x2, int y2, in LockedSurface source2, ref Rect R)
    {
        switch (_mode)
        {
            case PageTurnMode.Left:
                _source1 = source1;
                _sourceX1 = x1;
                _sourceY1 = y1;
                _source2 = source2;
                _sourceX2 = x2;
                _sourceY2 = y2;
                RenderLeftRightScene(X, Y, destination);
                R.Left = X;
                R.Right = X + _width * 2;
                R.Top = Y - (Round((double)_scale * _radius) + 1);
                R.Bottom = Y + _height;
                break;
            case PageTurnMode.Right:
                _source1 = source2;
                _sourceX1 = x2;
                _sourceY1 = y2;
                _source2 = source1;
                _sourceX2 = x1;
                _sourceY2 = y1;
                RenderLeftRightScene(X, Y, destination);
                R.Left = X;
                R.Right = X + _width * 2;
                R.Top = Y - (Round((double)_scale * _radius) + 1);
                R.Bottom = Y + _height;
                break;
            case PageTurnMode.Open:
                _source1 = source1;
                _sourceX1 = x1;
                _sourceY1 = y1;
                _source2 = source2;
                _sourceX2 = x2;
                _sourceY2 = y2;
                RenderOpenCloseScene(X, Y, destination);
                R.Left = X;
                R.Right = X + _width * 2;
                var pi = Math.PI;
                var sin = Math.Sin((double)((double)_position * pi) / 180);
                R.Top = Y - Round((double)((double)((double)sin * _scale) * _width) + 1);
                R.Bottom = Y + _height;
                break;
            case PageTurnMode.Close:
                _source1 = source2;
                _sourceX1 = x2;
                _sourceY1 = y2;
                _source2 = source1;
                _sourceX2 = x1;
                _sourceY2 = y1;
                RenderOpenCloseScene(X, Y, destination);
                R.Left = X;
                R.Right = X + _width * 2;
                var mathPI = Math.PI;
                var mathSin = Math.Sin((double)((double)_position * mathPI) / 180);
                R.Top = Y - Round((double)((double)((double)mathSin * _scale) * _width) + 1);
                R.Bottom = Y + _height;
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
