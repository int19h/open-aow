using System.Numerics;

namespace Aow.Rtl;

// Explicit bounds describe the Delphi storage image, independently of the CLR layout.
public readonly struct Set<E> : IEquatable<Set<E>> where E : unmanaged
{
    private readonly ulong a, b, c, d;
    private readonly int low, high;
    private readonly bool bounded;
    private static readonly (int Low, int High) DefaultBounds = GetDefaultBounds();

    private int Low => bounded ? low : DefaultBounds.Low;
    private int High => bounded ? high : DefaultBounds.High;
    public ulong Mask => a;

    public ulong Image
    {
        get
        {
            int shift = Low / 8 % 8 * 8;
            int word = Low / 64;
            return shift == 0 ? Word(word) : (Word(word) >> shift) | (Word(word + 1) << (64 - shift));
        }
    }

    public int StorageSize
    {
        get
        {
            int size = High / 8 - Low / 8 + 1;
            return size == 3 ? 4 : size;
        }
    }

    private Set(ulong a, ulong b, ulong c, ulong d, int low, int high)
    {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
        this.low = low;
        this.high = high;
        bounded = true;
    }

    private static (int, int) GetDefaultBounds()
    {
        if (typeof(E) == typeof(byte)) return (0, 63);
        if (!typeof(E).IsEnum) throw new InvalidOperationException("Set elements require an enum or byte");
        var values = Enum.GetValues(typeof(E)).Cast<E>().Select(Ordinal).ToArray();
        return values.Length == 0 ? (0, 63) : (values.Min(), values.Max());
    }

    private static int Ordinal(E value)
    {
        long number = Convert.ToInt64(value);
        if (number is < 0 or > 255) throw new ArgumentOutOfRangeException(paramName: null, "Set ordinal outside 0..255");
        return (int)number;
    }

    private ulong Word(int index) => index switch { 0 => a, 1 => b, 2 => c, 3 => d, _ => 0 };

    public static Set<E> WithBounds(int low, int high)
    {
        if (low < 0 || high > 255 || low > high) throw new ArgumentOutOfRangeException(paramName: null, "Invalid set bounds");
        return new(0, 0, 0, 0, low, high);
    }

    public static Set<E> Of(params E[] values)
    {
        Set<E> result = default;
        foreach (E value in values)
        {
            if (Ordinal(value) > 63) throw new ArgumentOutOfRangeException(paramName: null, "Implicit set ordinal exceeds 63");
            result = result.With(value);
        }
        return result;
    }

    public static Set<E> FromMask(ulong mask) => new(mask, 0, 0, 0, DefaultBounds.Low, DefaultBounds.High);
    public static Set<E> Range(E lo, E hi) => default(Set<E>).WithRange(lo, hi);

    public Set<E> WithRange(E lo, E hi)
    {
        int first = Ordinal(lo), last = Ordinal(hi);
        if (!bounded && (first > 63 || last > 63)) throw new ArgumentOutOfRangeException(paramName: null, "Implicit set ordinal exceeds 63");
        var result = this;
        for (int i = first; i <= last; i++) result = result.SetBit(i, true);
        return result;
    }

    private Set<E> SetBit(int ordinal, bool present)
    {
        if (ordinal < Low || ordinal > High) throw new ArgumentOutOfRangeException(paramName: null, "Ordinal outside declared set bounds");
        ulong bit = 1UL << (ordinal % 64);
        ulong change = present ? Word(ordinal / 64) | bit : Word(ordinal / 64) & ~bit;
        return new(ordinal < 64 ? change : a, ordinal is >= 64 and < 128 ? change : b,
            ordinal is >= 128 and < 192 ? change : c, ordinal >= 192 ? change : d, Low, High);
    }

    public bool Contains(E value)
    {
        int ordinal = Ordinal(value);
        return ordinal >= Low && ordinal <= High && (Word(ordinal / 64) & (1UL << (ordinal % 64))) != 0;
    }

    public Set<E> With(E value)
    {
        int ordinal = Ordinal(value);
        if (!bounded && ordinal > 63) throw new ArgumentOutOfRangeException(paramName: null, "Implicit set ordinal exceeds 63");
        return SetBit(ordinal, true);
    }

    public Set<E> Without(E value) => SetBit(Ordinal(value), false);
    public bool IsSubsetOf(Set<E> other) => ((a & ~other.a) | (b & ~other.b) | (c & ~other.c) | (d & ~other.d)) == 0;

    public bool IsEmpty => (a | b | c | d) == 0;
    public int Count => BitOperations.PopCount(a) + BitOperations.PopCount(b) + BitOperations.PopCount(c) + BitOperations.PopCount(d);

    public void CopyImageTo(Span<byte> destination)
    {
        if (destination.Length < StorageSize) throw new ArgumentOutOfRangeException(paramName: null, "Set image buffer is too small");
        for (int i = 0; i < StorageSize; i++)
        {
            int bit = (Low / 8 + i) * 8;
            destination[i] = (byte)(Word(bit / 64) >> (bit % 64));
        }
    }

    public static Set<E> operator +(Set<E> x, Set<E> y) => new(x.a | y.a, x.b | y.b, x.c | y.c, x.d | y.d, Math.Min(x.Low, y.Low), Math.Max(x.High, y.High));
    public static Set<E> operator -(Set<E> x, Set<E> y) => new(x.a & ~y.a, x.b & ~y.b, x.c & ~y.c, x.d & ~y.d, x.Low, x.High);
    public static Set<E> operator &(Set<E> x, Set<E> y) => new(x.a & y.a, x.b & y.b, x.c & y.c, x.d & y.d, x.Low, x.High);
    public bool Equals(Set<E> other) => a == other.a && b == other.b && c == other.c && d == other.d;
    public override bool Equals(object? obj) => obj is Set<E> other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(a, b, c, d);
    public static bool operator ==(Set<E> x, Set<E> y) => x.Equals(y);
    public static bool operator !=(Set<E> x, Set<E> y) => !x.Equals(y);
}

public static class SetExtensions
{
    public static Set<E> Range<E>(this Set<E> set, E low, E high) where E : unmanaged => set.WithRange(low, high);
}
