using System.Reflection;
using System.Text;

namespace Aow.Rtl;

// Names the port stores and the names original game files store.
// One row names one type or one enum member.
// Delphi is the name in an original file. Stored is the name in our own documents.
// A lookup takes the current name first and then an old name from the table.
// A type with no row uses its C# name for both.
public static class NameTable
{
    private static readonly object gate = new();
    private static Table? product;
    private static Table? active;
    private static List<Type>? scanned;
    private static int generation;
    private static int scannedGeneration = -1;

    public static string Delphi(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        var defined = Definition(type);
        var table = Current();
        return table.TypeByCurrent.TryGetValue(QualifiedName(defined), out var row) ? row.Delphi : SimpleName(defined);
    }

    public static string Stored(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        return SimpleName(Definition(type));
    }

    public static string DelphiMember(Type enumType, string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(member);
        var table = Current();
        return table.MemberDelphi.TryGetValue((QualifiedName(enumType), member), out var delphi) ? delphi : member;
    }

    public static string StoredMember(Type enumType, string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(member);
        return TryMember(enumType, member, out var current) ? current : member;
    }

    public static string? ResolveStored(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var table = Current();
        if (table.TypeBySimple.ContainsKey(name)) return name;
        if (!table.TypeByDelphi.TryGetValue(name, out var rows)) return null;
        string? simple = null;
        foreach (var row in rows)
        {
            string current = Simple(row.Current);
            if (simple is null) simple = current;
            else if (simple != current)
                throw new InvalidOperationException("The stored name " + name + " maps to more than one current name.");
        }
        return simple;
    }

    public static bool TryMember(Type enumType, string text, out string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(text);
        member = "";
        var defined = Definition(enumType);
        if (!defined.IsEnum) return false;
        foreach (var name in Enum.GetNames(defined))
        {
            if (name != text) continue;
            member = name;
            return true;
        }
        if (!Current().MemberCurrent.TryGetValue((QualifiedName(defined), text), out var current)) return false;
        member = current;
        return true;
    }

    public static bool Names(Type type, string name)
    {
        ArgumentNullException.ThrowIfNull(type);
        ArgumentNullException.ThrowIfNull(name);
        return name == Stored(type) || name == Delphi(type);
    }

    public static Type Find(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        var table = Current();
        string? resolved = ResolveStored(name);
        string simple = resolved ?? name;
        List<NameRow>? rows = null;
        if (table.TypeBySimple.TryGetValue(name, out var bySimple)) rows = bySimple;
        else if (resolved is not null && table.TypeByDelphi.TryGetValue(name, out var byDelphi)) rows = byDelphi;
        var matches = new List<Type>();
        foreach (var type in Scan())
        {
            var defined = Definition(type);
            if (SimpleName(defined) != simple) continue;
            if (rows is not null)
            {
                string qualified = QualifiedName(defined);
                bool listed = false;
                foreach (var row in rows)
                    if (row.Current == qualified) listed = true;
                if (!listed) continue;
            }
            bool known = false;
            foreach (var match in matches)
                if (match == defined) known = true;
            if (!known) matches.Add(defined);
        }
        if (matches.Count == 1) return matches[0];
        if (matches.Count == 0) throw new InvalidOperationException("The stored name " + name + " matches no type.");
        throw new InvalidOperationException("The stored name " + name + " matches more than one type.");
    }

    internal static string QualifiedName(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        type = Definition(type);
        var parts = new List<string>();
        for (var current = type; current is not null; current = current.DeclaringType)
            parts.Add(current.Name);
        parts.Reverse();
        string nested = string.Join('.', parts);
        return type.Namespace is { Length: > 0 } ns ? ns + "." + nested : nested;
    }

    internal static IReadOnlyList<NameRow> Rows => Current().Rows;

    internal static IDisposable Install(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        var parsed = Table.Parse(text);
        lock (gate)
        {
            var previous = active ?? Product();
            active = parsed;
            generation++;
            return new Restorer(previous);
        }
    }

    private static Table Current()
    {
        lock (gate) return active ?? Product();
    }

    private static Table Product()
    {
        if (product is not null) return product;
        using var stream = typeof(NameTable).Assembly.GetManifestResourceStream("Aow.Rtl.names.csv")
            ?? throw new InvalidOperationException("The name table is missing.");
        using var reader = new StreamReader(stream, Encoding.UTF8);
        product = Table.Parse(reader.ReadToEnd());
        return product;
    }

    private static List<Type> Scan()
    {
        lock (gate)
        {
            if (scanned is not null && scannedGeneration == generation) return scanned;
            var list = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic) continue;
                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException error)
                {
                    types = error.Types.Where(type => type is not null).Cast<Type>().ToArray();
                }
                foreach (var type in types)
                {
                    if (type is null || type.Name.Contains('<')) continue;
                    list.Add(type);
                }
            }
            scanned = list;
            scannedGeneration = generation;
            return list;
        }
    }

    private static Type Definition(Type type) =>
        type.IsGenericType && !type.IsGenericTypeDefinition ? type.GetGenericTypeDefinition() : type;

    private static string SimpleName(Type type) => type.Name.TrimStart('@');

    private static string Simple(string qualified)
    {
        int dot = qualified.LastIndexOf('.');
        return dot < 0 ? qualified : qualified[(dot + 1)..];
    }

    private sealed class Restorer(Table previous) : IDisposable
    {
        public void Dispose()
        {
            lock (gate)
            {
                active = previous;
                generation++;
            }
        }
    }

    private sealed class Table
    {
        internal NameRow[] Rows { get; private init; } = [];
        internal Dictionary<string, NameRow> TypeByCurrent { get; private init; } = [];
        internal Dictionary<string, List<NameRow>> TypeBySimple { get; private init; } = [];
        internal Dictionary<string, List<NameRow>> TypeByDelphi { get; private init; } = [];
        internal Dictionary<(string Enum, string Member), string> MemberCurrent { get; private init; } = [];
        internal Dictionary<(string Enum, string Member), string> MemberDelphi { get; private init; } = [];

        internal static Table Parse(string text)
        {
            var rows = new List<NameRow>();
            bool header = false;
            foreach (var parts in Records(text))
            {
                if (parts.Length == 1 && (parts[0].Length == 0 || parts[0][0] == '#')) continue;
                if (!header)
                {
                    if (parts.Length != 3 || parts[0] != "kind" || parts[1] != "delphi" || parts[2] != "current")
                        throw new InvalidOperationException("The name table header is missing.");
                    header = true;
                    continue;
                }
                if (parts.Length != 3 || parts[0] is not ("type" or "member") || parts[1].Length == 0 || parts[2].Length == 0)
                    throw new InvalidOperationException("A name table row is not kind, delphi and current.");
                rows.Add(new NameRow(parts[0], parts[1], parts[2]));
            }
            if (!header) throw new InvalidOperationException("The name table header is missing.");
            return Index(rows);
        }

        // A line that starts with # is a comment. A field is quoted only when it holds a comma, a quote or a line break.
        private static List<string[]> Records(string text)
        {
            var records = new List<string[]>();
            var fields = new List<string>();
            var field = new StringBuilder();
            bool quoted = false;
            bool begin = true;
            string normalized = text.Replace("\r\n", "\n").Replace('\r', '\n');
            for (int i = 0; i < normalized.Length; i++)
            {
                char c = normalized[i];
                if (quoted)
                {
                    if (c == '"')
                    {
                        if (i + 1 < normalized.Length && normalized[i + 1] == '"')
                        {
                            field.Append('"');
                            i++;
                        }
                        else quoted = false;
                    }
                    else field.Append(c);
                    begin = false;
                    continue;
                }
                if (begin && c == '#')
                {
                    int end = normalized.IndexOf('\n', i);
                    i = end < 0 ? normalized.Length - 1 : end;
                    continue;
                }
                if (c == '"')
                {
                    if (field.Length != 0)
                        throw new InvalidOperationException("A name table quote sits inside a field.");
                    quoted = true;
                    begin = false;
                    continue;
                }
                if (c == ',')
                {
                    fields.Add(field.ToString());
                    field.Clear();
                    begin = false;
                    continue;
                }
                if (c == '\n')
                {
                    if (begin && field.Length == 0 && fields.Count == 0) continue;
                    fields.Add(field.ToString());
                    field.Clear();
                    records.Add(fields.ToArray());
                    fields.Clear();
                    begin = true;
                    continue;
                }
                field.Append(c);
                begin = false;
            }
            if (quoted) throw new InvalidOperationException("A quoted name table field is not closed.");
            if (field.Length > 0 || fields.Count > 0)
            {
                fields.Add(field.ToString());
                records.Add(fields.ToArray());
            }
            return records;
        }

        private static Table Index(List<NameRow> rows)
        {
            var typeByCurrent = new Dictionary<string, NameRow>();
            var typeBySimple = new Dictionary<string, List<NameRow>>();
            var typeByDelphi = new Dictionary<string, List<NameRow>>();
            var memberCurrent = new Dictionary<(string, string), string>();
            var memberDelphi = new Dictionary<(string, string), string>();
            var seen = new HashSet<string>();
            foreach (var row in rows)
            {
                if (!seen.Add(row.Current))
                    throw new InvalidOperationException("The name table repeats " + row.Current + ".");
                if (row.Kind == "type")
                {
                    typeByCurrent.Add(row.Current, row);
                    Add(typeBySimple, Simple(row.Current), row);
                    Add(typeByDelphi, row.Delphi, row);
                    continue;
                }
                if (row.Kind != "member")
                    throw new InvalidOperationException("A name table row has an unknown kind.");
                int currentDot = row.Current.LastIndexOf('.');
                int delphiDot = row.Delphi.LastIndexOf('.');
                if (currentDot <= 0 || delphiDot <= 0)
                    throw new InvalidOperationException("A member row needs an enum name and a member name.");
                string enumName = row.Current[..currentDot];
                string currentMember = row.Current[(currentDot + 1)..];
                string delphiMember = row.Delphi[(delphiDot + 1)..];
                if (!memberCurrent.TryAdd((enumName, delphiMember), currentMember))
                    throw new InvalidOperationException("The name table repeats member " + row.Delphi + ".");
                if (!memberDelphi.TryAdd((enumName, currentMember), delphiMember))
                    throw new InvalidOperationException("The name table repeats member " + row.Current + ".");
            }
            return new Table
            {
                Rows = rows.ToArray(),
                TypeByCurrent = typeByCurrent,
                TypeBySimple = typeBySimple,
                TypeByDelphi = typeByDelphi,
                MemberCurrent = memberCurrent,
                MemberDelphi = memberDelphi,
            };
        }

        private static void Add(Dictionary<string, List<NameRow>> index, string key, NameRow row)
        {
            if (!index.TryGetValue(key, out var list))
            {
                list = [];
                index.Add(key, list);
            }
            list.Add(row);
        }
    }
}

internal readonly record struct NameRow(string Kind, string Delphi, string Current);
