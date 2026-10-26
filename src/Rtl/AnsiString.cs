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
        : FromBytes(pointer.Array!.AsSpan(pointer.Index, Rtl.StrLen(pointer)).ToArray());
    public Ptr<byte> AsPtr()
    {
        var copy = new byte[Length + 1];
        Span.CopyTo(copy);
        return Ptr<byte>.To(copy, 0);
    }

    // Preserve byte escapes in generated literals and encode Unicode CP1252 characters.
    private const string HighControls = "\u20ac\u0081\u201a\u0192\u201e\u2026\u2020\u2021\u02c6\u2030\u0160\u2039\u0152\u008d\u017d\u008f\u0090\u2018\u2019\u201c\u201d\u2022\u2013\u2014\u02dc\u2122\u0161\u203a\u0153\u009d\u017e\u0178";
    public static implicit operator AnsiString(string? value)
    {
        if (string.IsNullOrEmpty(value)) return default;
        var result = new byte[value.Length];
        for (int i = 0; i < result.Length; i++)
        {
            char c = value[i];
            int special = HighControls.IndexOf(c);
            result[i] = c <= 255 ? (byte)c : special >= 0 ? (byte)(128 + special) : (byte)'?';
        }
        return new(result);
    }

    public override string ToString()
    {
        var chars = new char[Length];
        for (int i = 0; i < chars.Length; i++)
            chars[i] = bytes![i] is >= 128 and < 160 ? HighControls[bytes[i] - 128] : (char)bytes[i];
        return new(chars);
    }

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
