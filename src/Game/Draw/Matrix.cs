#nullable disable warnings
// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

// a byte matrix (the invalidated blocks of a surface). FData, FWidth, FHeight. The port's FData
// is the array of FWidth * FHeight bytes, row by row, and null for an empty matrix as GetMem of
// size 0 gives nil.
public sealed class Matrix : IDisposable
{
    internal byte[]? Data { get; private set; }

    // DisplayC.pas:3145
    // { TMatrix (DCTypes in the original) }
    public Matrix(int width, int height)
    {
        Width = width;
        Height = height;
        Data = Width * Height == 0 ? null : new byte[Width * Height];
    }

    // DisplayC.pas:3155
    public void Dispose() => Data = null;

    // DisplayC.pas:3166
    // { The value at X, Y, clipped to 2. }
    public int GetValue(int X, int Y)
    {
        int result;
        if (X < Width && Y < Height)
        {
            result = Data[X + (Y * Width)];
            if (result > 2)
            {
                return 2;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(paramName: null, message: "TMatrix out of bounds (DCTypes)");
        }

        return result;
    }

    // DisplayC.pas:3178
    public void SetValue(int X, int Y, int value)
    {
        if (X < Width && Y < Height)
        {
            Data[X + (Y * Width)] = (byte)value;
        }
    }

    public int Width { get; }
    public int Height { get; }
}
