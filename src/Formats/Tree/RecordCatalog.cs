using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Rtl;

namespace Aow.Formats.Tree;

/// <summary>
/// One explicit-layout struct the schema names. Fields follow <c>FieldOffset</c> order.
/// Every byte is covered by a field or by a <see cref="StoredPaddingAttribute"/> span.
/// Padding bytes are ignored on read, written as 0 and never stored in a document.
/// A one-field wrapper of a set, with no padding, is stored as that set.
/// </summary>
internal sealed partial class RecordTypeInfo
{
    public required string Name { get; init; }

    /// <summary>The struct the record describes: an I/O image bound by <see cref="StoredRecordAttribute"/>, or a test record.</summary>
    public required Type Image { get; init; }
    public required int Size { get; init; }
    public required IReadOnlyList<RecordFieldInfo> Fields { get; init; }
    public required IReadOnlyList<(int Offset, int Length)> Padding { get; init; }
    public bool BareSet { get; init; }
}

/// <summary>Resolves a schema record name where no game object exists.</summary>
internal interface IRecordCatalog
{
    bool TryGet(string typeName, out RecordTypeInfo type);
}

/// <summary>A catalog built from explicit types. Tests register the records a fixture names.</summary>
internal sealed class DictionaryRecordCatalog : IRecordCatalog
{
    private readonly Dictionary<string, RecordTypeInfo> _types = [];

    public DictionaryRecordCatalog(params Type[] records)
    {
        foreach (Type type in records)
        {
            RecordTypeInfo info = RecordTypeInfo.Describe(type);
            if (!_types.TryAdd(info.Name, info))
            {
                throw new InvalidOperationException($"The record {info.Name} is registered twice.");
            }
        }
    }

    public bool TryGet(string typeName, out RecordTypeInfo type) => _types.TryGetValue(typeName, out type!);
}

/// <summary>
/// The record catalogs registered for the process. Formats does not reference the units assembly.
/// The units register one reflection catalog for the units and one for the runtime records.
/// A reflection catalog takes the structs that carry <see cref="StoredRecordAttribute"/>, each under
/// the record name the attribute binds. A test catalog names a struct by its stored type name.
/// A second registration of the same assembly is ignored. Two catalogs that both name one record fail at the lookup.
/// </summary>
public static class RecordCatalog
{
    private static readonly Lock s_gate = new();
    private static readonly List<IRecordCatalog> s_catalogs = [];
    private static readonly HashSet<Assembly> s_assemblies = [];

    internal static void Register(IRecordCatalog catalog)
    {
        ArgumentNullException.ThrowIfNull(catalog);
        lock (s_gate)
        {
            s_catalogs.Add(catalog);
        }
    }

    public static void RegisterReflection(Assembly assembly)
    {
        ArgumentNullException.ThrowIfNull(assembly);
        lock (s_gate)
        {
            if (!s_assemblies.Add(assembly))
            {
                return;
            }

            s_catalogs.Add(new ReflectionRecordCatalog(assembly));
        }
    }

    internal static RecordTypeInfo? Find(string typeName)
    {
        lock (s_gate)
        {
            RecordTypeInfo? found = Lookup(typeName);
            if (found is not null)
            {
                return found;
            }

            string? resolved = NameTable.ResolveStored(typeName);
            return resolved is null || resolved == typeName ? null : Lookup(resolved);
        }
    }

    private static RecordTypeInfo? Lookup(string typeName)
    {
        RecordTypeInfo? found = null;
        foreach (IRecordCatalog catalog in s_catalogs)
        {
            if (!catalog.TryGet(typeName, out RecordTypeInfo? type))
            {
                continue;
            }

            if (found is not null)
            {
                throw new InvalidOperationException($"The record {typeName} is registered twice.");
            }

            found = type;
        }
        return found;
    }

    internal static RecordTypeInfo Require(string typeName, string? file, string path)
        => Find(typeName) ?? throw new ConversionException(new FormatDiagnostic(Codes.Type, Message: $"The record {typeName} is not in the catalog.")
        { File = file, Path = path });
}

internal sealed class ReflectionRecordCatalog : IRecordCatalog
{
    private readonly Dictionary<string, List<Type>> _types = [];
    private readonly Dictionary<string, RecordTypeInfo> _described = [];

    public ReflectionRecordCatalog(Assembly assembly)
    {
        Type[] found;
        try
        {
            found = assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException error)
        {
            found = [.. error.Types.Where(type => type is not null).Cast<Type>()];
        }
        foreach (Type? type in found)
        {
            if (type is null || !type.IsValueType || type.IsEnum || type.IsGenericType)
            {
                continue;
            }

            if (type.GetCustomAttribute<StoredRecordAttribute>() is not { } binding)
            {
                continue;
            }

            string stored = binding.Name;
            if (!_types.TryGetValue(stored, out List<Type>? list))
            {
                list = [];
                _types.Add(stored, list);
            }
            list.Add(type);
        }
    }

    public bool TryGet(string typeName, out RecordTypeInfo type)
    {
        if (_described.TryGetValue(typeName, out type!))
        {
            return true;
        }

        if (!_types.TryGetValue(typeName, out List<Type>? list))
        {
            type = null!;
            return false;
        }
        if (list.Count != 1)
        {
            throw new InvalidOperationException($"The record {typeName} is declared {list.Count} times.");
        }

        type = RecordTypeInfo.Describe(list[0]);
        _described.Add(typeName, type);
        return true;
    }
}

internal sealed partial class RecordTypeInfo
{
    /// <summary>The record name a struct stores: its <see cref="StoredRecordAttribute"/> binding, or else its stored type name.</summary>
    internal static string StoredName(Type type) => type.GetCustomAttribute<StoredRecordAttribute>()?.Name ?? NameTable.Stored(type);

    internal static RecordTypeInfo Describe(Type type)
    {
        StructLayoutAttribute? layout = type.StructLayoutAttribute;
        if (type is not { IsValueType: true, IsEnum: false } || layout is null || layout.Value != LayoutKind.Explicit || layout.Size <= 0)
        {
            throw new InvalidOperationException($"{type.Name} is not an explicit record.");
        }

        int size = layout.Size;
        List<(FieldInfo Field, FieldOffsetAttribute? Offset)> raw = [.. type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            .Select(field => (Field: field, Offset: field.GetCustomAttribute<FieldOffsetAttribute>()))
            .Where(field => field.Offset is not null)];
        HashSet<FieldInfo> consumed = [];
        List<RecordFieldInfo> named = [];
        List<(int Offset, int Length)> padding = [];
        foreach (PropertyInfo property in type.GetProperties(BindingFlags.Instance | BindingFlags.Public))
        {
            if (!property.PropertyType.IsGenericType || property.PropertyType.GetGenericTypeDefinition() != typeof(Set<>))
            {
                continue;
            }

            FieldInfo image = raw.FirstOrDefault(field => field.Field.Name == $"{property.Name}Image").Field
                ?? throw new InvalidOperationException($"{type.Name}.{property.Name} has no image field.");
            _ = consumed.Add(image);
            int offset = image.GetCustomAttribute<FieldOffsetAttribute>()!.Value;
            int fieldSize = FieldSize(image);
            int storage = SetStorage(property.PropertyType);
            if (storage != fieldSize)
            {
                throw new InvalidOperationException($"{type.Name}.{property.Name} stores {storage} bytes in a {fieldSize} byte field.");
            }

            Type element = property.PropertyType.GetGenericArguments()[0];
            named.Add(new SetRecordField
            {
                Name = property.Name,
                Offset = offset,
                Size = fieldSize,
                ElementType = NameTable.Stored(element),
                ElementWidth = fieldSize,
                ElementColor = element,
            });
        }
        foreach ((FieldInfo? field, FieldOffsetAttribute? offset) in raw)
        {
            if (consumed.Contains(field))
            {
                continue;
            }

            int at = offset!.Value;
            int fieldSize = FieldSize(field);
            named.Add(DescribeField(type, field, offset: at, size: fieldSize));
        }
        foreach (StoredPaddingAttribute declared in type.GetCustomAttributes<StoredPaddingAttribute>(inherit: false))
        {
            if (declared.Length <= 0)
            {
                throw new InvalidOperationException($"{type.Name} declares padding with non-positive length {declared.Length}.");
            }

            padding.Add((declared.Offset, declared.Length));
        }
        bool[] cover = new bool[size];
        void Cover(int offset, int length, string name)
        {
            if (offset < 0 || length < 0 || offset + length > size)
            {
                throw new InvalidOperationException($"{type.Name}.{name} does not fit the record.");
            }

            for (int i = 0; i < length; i++)
            {
                if (cover[offset + i])
                {
                    throw new InvalidOperationException($"{type.Name} covers byte {offset + i} twice.");
                }

                cover[offset + i] = true;
            }
        }
        // A record stores each byte once (own-formats.md 2.3): a field that overlaps another is refused.
        named.Sort(static (a, b) => a.Offset.CompareTo(b.Offset));
        foreach (RecordFieldInfo field in named)
        {
            Cover(field.Offset, length: field.Size, field.Name);
        }

        foreach ((int offset, int length) in padding)
        {
            Cover(offset, length, name: "padding");
        }

        for (int i = 0; i < size; i++)
        {
            if (!cover[i])
            {
                throw new InvalidOperationException($"{type.Name} leaves byte {i} uncovered. Declare a field or [StoredPadding].");
            }
        }
        padding.Sort(static (a, b) => a.Offset.CompareTo(b.Offset));
        bool bare = named.Count == 1 && named[0].Kind == RecordFieldKind.Set && padding.Count == 0;
        return new RecordTypeInfo { Name = StoredName(type), Image = type, Size = size, Fields = named, Padding = padding, BareSet = bare };
    }

    private static RecordFieldInfo DescribeField(Type owner, FieldInfo field, int offset, int size)
    {
        InlineArrayAttribute? inline = field.FieldType.GetCustomAttribute<InlineArrayAttribute>();
        if (inline is not null)
        {
            FieldInfo element = field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault()
                ?? throw new InvalidOperationException($"{owner.Name}.{field.Name} has no element.");
            (int width, bool signed) = IntegerLayout(element.FieldType, name: $"{owner.Name}.{field.Name}");
            return new ArrayRecordField
            {
                Name = field.Name,
                Offset = offset,
                Size = size,
                ElementWidth = width,
                ElementSigned = signed,
                ElementCount = inline.Length,
                ElementColor = element.FieldType,
            };
        }
        if (field.FieldType.IsEnum)
        {
            (int width, bool signed) = EnumTypeInfo.Layout(field.FieldType);
            return new EnumRecordField
            {
                Name = field.Name,
                Offset = offset,
                Size = size,
                ElementType = NameTable.Stored(field.FieldType),
                ElementWidth = width,
                ElementSigned = signed,
                ElementColor = field.FieldType,
            };
        }
        if (field.FieldType == typeof(bool))
        {
            return new BooleanRecordField { Name = field.Name, Offset = offset, Size = size };
        }

        StructLayoutAttribute? nested = field.FieldType.StructLayoutAttribute;
        if (nested is { Value: LayoutKind.Explicit, Size: > 0 })
        {
            return new NestedRecordField
            {
                Name = field.Name,
                Offset = offset,
                Size = nested.Size,
                ElementType = StoredName(field.FieldType),
                ElementColor = field.FieldType,
            };
        }

        (int integerWidth, bool integerSigned) = IntegerLayout(field.FieldType, name: $"{owner.Name}.{field.Name}");
        return new IntegerRecordField
        {
            Name = field.Name,
            Offset = offset,
            Size = size,
            ElementWidth = integerWidth,
            ElementSigned = integerSigned,
        };
    }

    private static (int Width, bool Signed) IntegerLayout(Type type, string name)
    {
        if (type == typeof(byte))
        {
            return (1, false);
        }

        if (type == typeof(sbyte))
        {
            return (1, true);
        }

        if (type == typeof(ushort))
        {
            return (2, false);
        }

        if (type == typeof(short))
        {
            return (2, true);
        }

        if (type == typeof(uint))
        {
            return (4, false);
        }

        return type == typeof(int) ? ((int Width, bool Signed))(4, true) : throw new InvalidOperationException($"{name} is not an integer field.");
    }

    private static int FieldSize(FieldInfo field)
    {
        InlineArrayAttribute? inline = field.FieldType.GetCustomAttribute<InlineArrayAttribute>();
        if (inline is not null)
        {
            FieldInfo element = field.FieldType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault()
                ?? throw new InvalidOperationException($"{field.Name} has no element.");
            int width = element.FieldType == typeof(byte) || element.FieldType == typeof(sbyte) || element.FieldType == typeof(bool) ? 1
                : element.FieldType == typeof(short) || element.FieldType == typeof(ushort) ? 2
                : element.FieldType == typeof(int) || element.FieldType == typeof(uint) ? 4
                : throw new InvalidOperationException($"{field.Name} is not an integer array.");
            return inline.Length * width;
        }
        return field.FieldType.IsEnum
            ? EnumTypeInfo.Layout(field.FieldType).Width
            : field.FieldType == typeof(bool) ? 1 : Marshal.SizeOf(field.FieldType);
    }

    private static int SetStorage(Type setType)
    {
        object value = Activator.CreateInstance(setType)!;
        return (int)setType.GetProperty(nameof(Set<>.StorageSize))!.GetValue(value)!;
    }
}
