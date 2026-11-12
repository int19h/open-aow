using Aow.Rtl;

namespace Aow.Game.Draw.Images;

// AnsiCompareStr equality. The soft hyphen matches NUL and U+02C6 matches ^.
internal sealed class AnsiCompareComparer : IEqualityComparer<string>
{
    internal static AnsiCompareComparer Instance { get; } = new();

    private AnsiCompareComparer()
    {
    }

    public bool Equals(string x, string y) => RtlUnit.AnsiCompareStr(x, y) == 0;

    public int GetHashCode(string value)
    {
        var hash = new HashCode();
        if (value is null) return hash.ToHashCode();
        for (int index = 0; index < value.Length; index++)
            hash.Add(Collation(value[index]));
        return hash.ToHashCode();
    }

    private static int Collation(char value)
    {
        if (!Cp1252.IsMapped(value)) return Cp1252.Key(value);
        byte mapped = Cp1252.Byte(value);
        return mapped == 173 ? 0 : mapped == 136 ? 94 : mapped;
    }
}
