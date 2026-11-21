using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// One C# enum the schema names by its declared type name.
/// A stored name is a declared member. A const alias that is not a declared member is not stored.
/// When two declared members share a value, the first name <see cref="Enum.GetNames"/> returns is kept.
/// </summary>
public sealed class EnumTypeInfo
{
    public required string Name { get; init; }
    internal Type Color { get; init; } = null!;
    internal int Width { get; init; }
    public required bool Signed { get; init; }
    public required IReadOnlyDictionary<string, long> ByName { get; init; }
    public required IReadOnlyDictionary<long, string> ByValue { get; init; }

    internal static (int Width, bool Signed) Layout(Type type)
    {
        Type under = Enum.GetUnderlyingType(type);
        if (under == typeof(byte))
        {
            return (1, false);
        }

        if (under == typeof(sbyte))
        {
            return (1, true);
        }

        if (under == typeof(ushort))
        {
            return (2, false);
        }

        if (under == typeof(short))
        {
            return (2, true);
        }

        if (under == typeof(uint))
        {
            return (4, false);
        }

        if (under == typeof(int))
        {
            return (4, true);
        }

        if (under == typeof(ulong))
        {
            return (8, false);
        }

        return under == typeof(long)
            ? ((int Width, bool Signed))(8, true)
            : throw new InvalidOperationException($"Enum {type.Name} has an unsupported underlying type.");
    }

    internal static EnumTypeInfo Describe(Type type)
    {
        if (!type.IsEnum)
        {
            throw new InvalidOperationException($"{type.Name} is not an enum.");
        }

        (int width, bool signed) = Layout(type);
        Dictionary<string, long> byName = [];
        Dictionary<long, string> byValue = [];
        // A flags enum is a set: a member's number is its bit, as a set codec reads it.
        IEnumerable<(string Name, long Value)> members = FlagsExtensions.IsFlags(type)
            ? FlagsExtensions.MemberBits(type).Select(m => (m.Name, (long)m.Bit))
            : Enum.GetNames(type).Select(n => (n, ToInt64(value: Enum.Parse(type, value: n))));
        foreach ((string name, long value) in members)
        {
            byName.Add(name, value);
            _ = byValue.TryAdd(value, name);
        }
        return new EnumTypeInfo
        {
            Name = NameTable.Stored(type),
            Color = type,
            Width = width,
            Signed = signed,
            ByName = byName,
            ByValue = byValue,
        };
    }

    internal bool TryNumber(string text, out long number) => ByName.TryGetValue(text, out number) || (NameTable.TryMember(Color, text, out string? member) && ByName.TryGetValue(member, out number));

    internal static long ToInt64(object value) => Convert.ToInt64(value);
}
