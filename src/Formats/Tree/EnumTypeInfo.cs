namespace Aow.Formats.Tree;

/// <summary>
/// One C# enum the schema names by its declared type name.
/// A stored name is a declared member. A const alias that is not a declared member is not stored.
/// When two declared members share a value, the first name <see cref="Enum.GetNames"/> returns is kept.
/// </summary>
public sealed class EnumTypeInfo
{
    public required string Name { get; init; }
    public required int Width { get; init; }
    public required bool Signed { get; init; }
    public required IReadOnlyDictionary<string, long> ByName { get; init; }
    public required IReadOnlyDictionary<long, string> ByValue { get; init; }

    internal static (int Width, bool Signed) Layout(Type type)
    {
        var under = Enum.GetUnderlyingType(type);
        if (under == typeof(byte)) return (1, false);
        if (under == typeof(sbyte)) return (1, true);
        if (under == typeof(ushort)) return (2, false);
        if (under == typeof(short)) return (2, true);
        if (under == typeof(uint)) return (4, false);
        if (under == typeof(int)) return (4, true);
        if (under == typeof(ulong)) return (8, false);
        if (under == typeof(long)) return (8, true);
        throw new InvalidOperationException("Enum " + type.Name + " has an unsupported underlying type.");
    }

    internal static EnumTypeInfo Describe(Type type)
    {
        if (!type.IsEnum) throw new InvalidOperationException(type.Name + " is not an enum.");
        var (width, signed) = Layout(type);
        var byName = new Dictionary<string, long>();
        var byValue = new Dictionary<long, string>();
        foreach (var name in Enum.GetNames(type))
        {
            long value = ToInt64(Enum.Parse(type, name));
            byName.Add(name, value);
            byValue.TryAdd(value, name);
        }
        return new EnumTypeInfo { Name = type.Name, Width = width, Signed = signed, ByName = byName, ByValue = byValue };
    }

    internal static long ToInt64(object value) => Convert.ToInt64(value);
}
