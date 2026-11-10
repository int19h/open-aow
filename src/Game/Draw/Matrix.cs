// Translated from DisplayC.pas.

namespace Aow.Game.Draw;

// a byte matrix (the invalidated blocks of a surface). FData, FWidth, FHeight. The port's FData
// is the array of FWidth * FHeight bytes, row by row, and null for an empty matrix as GetMem of
// size 0 gives nil.
public partial class Matrix : IDisposable
{
    internal byte[] FData;
    internal byte FWidth;
    internal byte FHeight;

    // DisplayC.pas:3145
    // { TMatrix (DCTypes in the original) }
    public Matrix(byte AWidth, byte AHeight)
    {
        FWidth = AWidth;
        FHeight = AHeight;
        FData = FWidth * FHeight == 0 ? null : new byte[FWidth * FHeight];
    }

    // DisplayC.pas:3155
    public virtual void Dispose()
    {
        FData = null;
    }

    // DisplayC.pas:3166
    // { The value at X, Y, clipped to 2. }
    public int GetVal(byte X, byte Y)
    {
        int Result;
        if (X < FWidth && Y < FHeight)
        {
            Result = FData[X + Y * FWidth];
            if (Result > 2)
            {
                Result = 2;
            }
        }
        else
        {
            throw new ArgumentOutOfRangeException(paramName: null, "TMatrix out of bounds (DCTypes)");
        }

        return Result;
    }

    // DisplayC.pas:3178
    public void SetVal(byte X, byte Y, byte Value)
    {
        if (X < FWidth && Y < FHeight)
        {
            FData[X + Y * FWidth] = Value;
        }
    }

    public byte Width { get => FWidth; }
    public byte Height { get => FHeight; }

}
