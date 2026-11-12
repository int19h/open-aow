using System.Reflection;
using Aow.Formats.Tree;
using Aow.Rtl;

namespace Aow.Formats;

/// <summary>Resolves a schema enum name where no game object exists. The converter and the backends use it.</summary>
public interface IEnumCatalog
{
    bool TryGet(string typeName, out EnumTypeInfo type);
}

/// <summary>A catalog built from explicit types. Tests register the enums a fixture names.</summary>
internal sealed class DictionaryEnumCatalog : IEnumCatalog
{
    private readonly Dictionary<string, EnumTypeInfo> _types = new();

    public DictionaryEnumCatalog(params Type[] enums)
    {
        foreach (var type in enums)
        {
            var info = EnumTypeInfo.Describe(type);
            if (!_types.TryAdd(info.Name, info))
                throw new InvalidOperationException("The enum " + info.Name + " is registered twice.");
        }
    }

    public bool TryGet(string typeName, out EnumTypeInfo type) => _types.TryGetValue(typeName, out type!);
}

/// <summary>
/// The catalogs registered for the process. Formats does not reference the units assembly.
/// The units register one reflection catalog. A second registration of the same assembly is ignored.
/// Two catalogs that both name one enum fail at the lookup.
/// </summary>
public static class EnumCatalog
{
    private static readonly object s_gate = new();
    private static readonly List<IEnumCatalog> s_catalogs = [];
    private static readonly HashSet<Assembly> s_assemblies = [];

    public static void Register(IEnumCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        lock (s_gate) s_catalogs.Add(catalog);
    }

    public static void RegisterReflection(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        lock (s_gate)
        {
            if (!s_assemblies.Add(assembly)) return;
            s_catalogs.Add(new ReflectionEnumCatalog(assembly));
        }
    }

    public static EnumTypeInfo? Find(string typeName)
    {
        lock (s_gate)
        {
            var found = Lookup(typeName);
            if (found is not null) return found;
            var resolved = NameTable.ResolveStored(typeName);
            if (resolved is null || resolved == typeName) return null;
            return Lookup(resolved);
        }
    }

    private static EnumTypeInfo? Lookup(string typeName)
    {
        EnumTypeInfo? found = null;
        foreach (var catalog in s_catalogs)
        {
            if (!catalog.TryGet(typeName, out var type)) continue;
            if (found is not null)
                throw new InvalidOperationException("The enum " + typeName + " is registered twice.");
            found = type;
        }
        return found;
    }

    public static EnumTypeInfo Require(string typeName, string? file, string path) =>
        Find(typeName) ?? throw new ConversionException(new FormatDiagnostic(Codes.Type, "The enum " + typeName + " is not in the catalog.")
        { File = file, Path = path });
}

sealed class ReflectionEnumCatalog : IEnumCatalog
{
    private readonly Dictionary<string, List<EnumTypeInfo>> _types = new();

    public ReflectionEnumCatalog(Assembly assembly)
    {
        Type[] found;
        try
        {
            found = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException error)
        {
            found = error.Types.Where(type => type is not null).Cast<Type>().ToArray();
        }
        foreach (var type in found)
        {
            if (type is null || !type.IsEnum || type.IsGenericType) continue;
            string stored = NameTable.Stored(type);
            if (!_types.TryGetValue(stored, out var list))
            {
                list = [];
                _types.Add(stored, list);
            }
            list.Add(EnumTypeInfo.Describe(type));
        }
    }

    public bool TryGet(string typeName, out EnumTypeInfo type)
    {
        if (!_types.TryGetValue(typeName, out var list))
        {
            type = null!;
            return false;
        }
        if (list.Count != 1)
            throw new InvalidOperationException("The enum " + typeName + " is declared " + list.Count + " times.");
        type = list[0];
        return true;
    }
}
