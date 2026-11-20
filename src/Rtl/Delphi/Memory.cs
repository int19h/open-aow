namespace Aow.Rtl;

public static class DelphiMemory
{
    public static void Move<T>(Ptr<T> source, Ptr<T> destination, int count) => Ptr<T>.Copy(source, destination, count);

    public static void Move(ReadOnlySpan<byte> source, Span<byte> destination, int count)
    {
        if (count <= 0)
        {
            return;
        }

        if (count > source.Length || count > destination.Length)
        {
            throw new ArgumentOutOfRangeException(paramName: null, message: "Move exceeds the buffer");
        }

        source[..count].CopyTo(destination);
    }

    public static void FillChar<T>(Ptr<T> destination, int count, T value) => Ptr<T>.Fill(destination, count, value);

    // ReallocMem of a block of records held as an array: a count of 0 frees it (null), any other
    // count gives a new array that keeps the elements the two sizes share, the rest zero.
    public static T[]? ReallocArray<T>(T[]? array, int count)
    {
        if (count == 0)
        {
            return null;
        }

        T[] result = new T[count];
        array?.AsSpan(0, length: Math.Min(array.Length, count)).CopyTo(result);

        return result;
    }

    // Move of count records from one array index to another, in one array or two. Overlapping
    // ranges copy as Move copies them. A count of 0 or less moves nothing.
    public static void MoveElements<T>(T[]? source, int sourceIndex, T[]? destination, int destinationIndex, int count)
    {
        if (count <= 0)
        {
            return;
        }

        source.AsSpan(sourceIndex, length: count).CopyTo(destination.AsSpan(destinationIndex, length: count));
    }

    // FillChar(A[Index], Count * SizeOf(T), 0) of records held in an array. A count of 0 or less
    // clears nothing.
    public static void ClearElements<T>(T[]? array, int index, int count)
    {
        if (count <= 0)
        {
            return;
        }

        array.AsSpan(index, length: count).Clear();
    }

    public static void FillChar(Span<byte> destination, int count, byte value)
    {
        if (count > destination.Length)
        {
            throw new ArgumentOutOfRangeException(paramName: null, message: "FillChar exceeds the buffer");
        }

        if (count > 0)
        {
            destination[..count].Fill(value);
        }
    }

    public static bool CompareMem(ReadOnlySpan<byte> a, ReadOnlySpan<byte> b, int count)
    {
        return count > a.Length || count > b.Length
            ? throw new ArgumentOutOfRangeException(paramName: null, message: "CompareMem exceeds the buffer")
            : count <= 0 || a[..count].SequenceEqual(b[..count]);
    }
}
