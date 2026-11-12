// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

// a byte matrix (the invalidated blocks of a surface). FData, FWidth, FHeight. The port's FData
// is the array of FWidth * FHeight bytes, row by row, and null for an empty matrix as GetMem of
// size 0 gives nil.
public partial class Matrix : IDisposable
{
    internal byte[] _data;
    internal byte _width;
    internal byte _height;

    // DisplayC.pas:3145
    // { TMatrix (DCTypes in the original) }
    public Matrix(byte width, byte height)
    {
        _width = width;
        _height = height;
        _data = _width * _height == 0 ? null : new byte[_width * _height];
    }

    // DisplayC.pas:3155
    public virtual void Dispose()
    {
        _data = null;
    }

    // DisplayC.pas:3166
    // { The value at X, Y, clipped to 2. }
    public int GetValue(byte X, byte Y)
    {
        int result;
        if (X < _width && Y < _height)
        {
            result = _data[X + Y * _width];
            if (result > 2)
            {
                result = 2;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(paramName: null, "TMatrix out of bounds (DCTypes)");
        }

        return result;
    }

    // DisplayC.pas:3178
    public void SetValue(byte X, byte Y, byte value)
    {
        if (X < _width && Y < _height)
        {
            _data[X + Y * _width] = value;
        }
    }

    public byte Width { get => _width; }
    public byte Height { get => _height; }

}
