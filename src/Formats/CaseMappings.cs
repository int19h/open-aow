namespace Aow.Formats;

/// <summary>
/// The unit side of a case mapping (own-formats.md 3.1): what the game's kind of a key writes
/// back for stored values, through the kind family's FromFields and ToFields. Aow.Units registers
/// one per mapped class. The converter calls it only on numbers and creates no game object.
/// </summary>
public interface ICaseUnit
{
    /// <summary>The stored values the kind of Key writes, by stored member, absent members read as 0.</summary>
    IReadOnlyDictionary<string, long> Written(long key, IReadOnlyDictionary<string, long> stored);
}

/// <summary>The registered unit sides of the mapped classes, by persisted class name.</summary>
public static class CaseMappings
{
    private static readonly Dictionary<string, ICaseUnit> units = new(StringComparer.Ordinal);

    public static void Register(string persistedClass, ICaseUnit unit)
    {
        lock (units)
            if (!units.TryAdd(persistedClass, unit))
                throw new InvalidOperationException("The persisted class " + persistedClass + " has two case mappings.");
    }

    public static ICaseUnit? Find(string persistedClass)
    {
        lock (units) return units.GetValueOrDefault(persistedClass);
    }
}
