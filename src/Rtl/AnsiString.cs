namespace Aow.Rtl;

public readonly struct AnsiString : IEquatable<AnsiString>, IComparable<AnsiString>
{
    private readonly byte[]? bytes;
    private AnsiString(byte[] bytes) => this.bytes = bytes;
    internal ReadOnlySpan<byte> Span => bytes;
    public int Length => bytes?.Length ?? 0;
    public byte this[int index] => index >= 1 && index <= Length ? bytes![index - 1]
        : throw new ERangeError("String index out of bounds");

    public AnsiString WithChar(int index, byte value)
    {
        _ = this[index];
        var copy = Span.ToArray();
        copy[index - 1] = value;
        return new(copy);
    }

    public AnsiString Resized(int length)
    {
        if (length < 0) throw new ERangeError("Negative string length");
        var copy = new byte[length];
        Span[..Math.Min(length, Length)].CopyTo(copy);
        return new(copy);
    }

    public static AnsiString FromBytes(params byte[] bytes) => new((byte[])bytes.Clone());
    public static AnsiString FromPChar(Ptr<byte> pointer) => pointer.IsNull ? default
        : FromBytes(pointer.AsSpan(Rtl.StrLen(pointer)).ToArray());
    public Ptr<byte> AsPtr()
    {
        var copy = new byte[Length + 1];
        Span.CopyTo(copy);
        return Ptr<byte>.To(copy, 0);
    }

    public static implicit operator AnsiString(string? value) => new(Cp1252.Encode(value));

    public override string ToString() => Cp1252.Decode(Span);

    public int CompareTo(AnsiString other) => Span.SequenceCompareTo(other.Span);
    public bool Equals(AnsiString other) => Span.SequenceEqual(other.Span);
    public override bool Equals(object? obj) => obj is AnsiString other && Equals(other);
    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.AddBytes(Span);
        return hash.ToHashCode();
    }

    public static AnsiString operator +(AnsiString a, AnsiString b)
    {
        var result = new byte[checked(a.Length + b.Length)];
        a.Span.CopyTo(result);
        b.Span.CopyTo(result.AsSpan(a.Length));
        return new(result);
    }
    public static bool operator ==(AnsiString a, AnsiString b) => a.Equals(b);
    public static bool operator !=(AnsiString a, AnsiString b) => !a.Equals(b);
    public static bool operator <(AnsiString a, AnsiString b) => a.CompareTo(b) < 0;
    public static bool operator <=(AnsiString a, AnsiString b) => a.CompareTo(b) <= 0;
    public static bool operator >(AnsiString a, AnsiString b) => a.CompareTo(b) > 0;
    public static bool operator >=(AnsiString a, AnsiString b) => a.CompareTo(b) >= 0;
}
