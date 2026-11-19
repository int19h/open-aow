using System.Reflection;
using System.Text;

namespace Aow.Rtl;

// Names the port stores and the names original game files store.
// One row names one type or one enum member.
// Delphi is the name in an original file. Stored is the name in our own documents.
// A row with a stored name keeps that name when the C# name changes. An empty stored name
// is the declaring-type chain. A nested type stores Owner.Nested, with no namespace.
// A type lookup takes the current stored name first and then a Delphi or earlier name from the table.
// A type with no row stores that declaring-type chain and reports its simple name as the Delphi name.
public static class NameTable
{
    private static readonly Lock s_gate = new();
    private static Table? s_product;
    private static Table? s_active;
    private static List<Type>? s_scanned;
    private static int s_generation;
    private static int s_scannedGeneration = -1;

    public static string Delphi(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type defined = Definition(type);
        Table table = Current();
        return table.TypeByCurrent.TryGetValue(QualifiedName(defined), out NameRow row) ? row.Delphi : SimpleName(defined);
    }

    public static string Stored(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        Type defined = Definition(type);
        Table table = Current();
        return table.StoredByCurrent.TryGetValue(QualifiedName(defined), out string? stored) ? stored : DeclaringChain(defined);
    }

    public static string DelphiMember(Type enumType, string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(member);
        Table table = Current();
        return table.MemberDelphi.TryGetValue((QualifiedName(enumType), member), out string? delphi) ? delphi : member;
    }

    public static string StoredMember(Type enumType, string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(member);
        return TryMember(enumType, member, out string? current) ? current : member;
    }

    // The Delphi name of the type a stored name names: the first row of its current name, the name
    // Delphi(type) gives. Folded classes (issue 200) and old stored names add rows with other
    // Delphi names, and the first row stays the class's own Delphi name.
    public static string? DelphiName(string stored)
    {
        ArgumentNullException.ThrowIfNull(stored);
        Table table = Current();
        return !table.TypeByStored.TryGetValue(stored, out List<NameRow>? rows) || rows.Count == 0
            ? null
            : table.TypeByCurrent.TryGetValue(rows[0].Current, out NameRow first) ? first.Delphi : rows[0].Delphi;
    }

    public static string? ResolveStored(string name)
    {
        ArgumentNullException.ThrowIfNull(name);
        Table table = Current();
        if (table.TypeByStored.ContainsKey(name))
        {
            return name;
        }

        if (!table.TypeByDelphi.TryGetValue(name, out List<NameRow>? rows))
        {
            return null;
        }

        string? stored = null;
        foreach (NameRow row in rows)
        {
            string current = table.StoredByCurrent[row.Current];
            if (stored is null)
            {
                stored = current;
            }
            else if (stored != current)
            {
                throw new InvalidOperationException($"The stored name {name} maps to more than one current name.");
            }
        }
        return stored;
    }

    public static bool TryMember(Type enumType, string text, out string member)
    {
        ArgumentNullException.ThrowIfNull(enumType);
        ArgumentNullException.ThrowIfNull(text);
        member = "";
        Type defined = Definition(enumType);
        if (!defined.IsEnum)
        {
            return false;
        }

        foreach (string name in Enum.GetNames(defined))
        {
            if (name != text)
            {
                continue;
            }

            member = name;
            return true;
        }
        if (!Current().MemberCurrent.TryGetValue((QualifiedName(defined), text), out string? current))
        {
            return false;
        }

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
        Table table = Current();
        string? resolved = ResolveStored(name);
        string stored = resolved ?? name;
        List<NameRow>? rows = null;
        if (table.TypeByStored.TryGetValue(name, out List<NameRow>? byStored))
        {
            rows = byStored;
        }
        else if (resolved is not null && table.TypeByDelphi.TryGetValue(name, out List<NameRow>? byDelphi))
        {
            rows = byDelphi;
        }

        List<Type> matches = [];
        foreach (Type type in Scan())
        {
            Type defined = Definition(type);
            if (Stored(defined) != stored)
            {
                continue;
            }

            if (rows is not null)
            {
                string qualified = QualifiedName(defined);
                bool listed = false;
                foreach (NameRow row in rows)
                {
                    if (row.Current == qualified)
                    {
                        listed = true;
                    }
                }

                if (!listed)
                {
                    continue;
                }
            }
            bool known = false;
            foreach (Type match in matches)
            {
                if (match == defined)
                {
                    known = true;
                }
            }

            if (!known)
            {
                matches.Add(defined);
            }
        }
        if (matches.Count == 1)
        {
            return matches[0];
        }

        if (matches.Count == 0)
        {
            throw new InvalidOperationException($"The stored name {name} matches no type.");
        }

        throw new InvalidOperationException($"The stored name {name} matches more than one type.");
    }

    internal static string QualifiedName(Type type)
    {
        ArgumentNullException.ThrowIfNull(type);
        type = Definition(type);
        List<string> parts = [];
        for (Type? current = type; current is not null; current = current.DeclaringType)
        {
            parts.Add(current.Name);
        }

        parts.Reverse();
        string nested = string.Join('.', parts);
        return type.Namespace is { Length: > 0 } ns ? $"{ns}.{nested}" : nested;
    }

    internal static IReadOnlyList<NameRow> Rows => Current().Rows;

    internal static IDisposable Install(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        Table parsed = Table.Parse(text);
        lock (s_gate)
        {
            Table previous = s_active ?? Product();
            s_active = parsed;
            s_generation++;
            return new Restorer(previous);
        }
    }

    private static Table Current()
    {
        lock (s_gate)
        {
            return s_active ?? Product();
        }
    }

    private static Table Product()
    {
        if (s_product is not null)
        {
            return s_product;
        }

        using Stream stream = typeof(NameTable).Assembly.GetManifestResourceStream("Aow.Rtl.names.csv")
            ?? throw new InvalidOperationException("The name table is missing.");
        using StreamReader reader = new(stream, Encoding.UTF8);
        s_product = Table.Parse(reader.ReadToEnd());
        return s_product;
    }

    private static List<Type> Scan()
    {
        lock (s_gate)
        {
            if (s_scanned is not null && s_scannedGeneration == s_generation)
            {
                return s_scanned;
            }

            List<Type> list = [];
            foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                Type[] types;
                try
                {
                    types = assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException error)
                {
                    types = [.. error.Types.Where(type => type is not null).Cast<Type>()];
                }
                foreach (Type? type in types)
                {
                    if (type is null || type.Name.Contains('<'))
                    {
                        continue;
                    }

                    list.Add(type);
                }
            }
            s_scanned = list;
            s_scannedGeneration = s_generation;
            return list;
        }
    }

    private static Type Definition(Type type) =>
        type.IsGenericType && !type.IsGenericTypeDefinition ? type.GetGenericTypeDefinition() : type;

    private static string SimpleName(Type type) => type.Name.TrimStart('@');

    private static string DeclaringChain(Type type)
    {
        List<string> parts = [];
        for (Type? current = type; current is not null; current = current.DeclaringType)
        {
            parts.Add(SimpleName(current));
        }

        parts.Reverse();
        return string.Join('.', parts);
    }

    private static string Simple(string qualified)
    {
        int dot = qualified.LastIndexOf('.');
        return dot < 0 ? qualified : qualified[(dot + 1)..];
    }

    private sealed class Restorer(Table previous) : IDisposable
    {
        public void Dispose()
        {
            lock (s_gate)
            {
                s_active = previous;
                s_generation++;
            }
        }
    }

    private sealed class Table
    {
        internal NameRow[] Rows { get; private init; } = [];
        internal Dictionary<string, NameRow> TypeByCurrent { get; private init; } = [];
        internal Dictionary<string, string> StoredByCurrent { get; private init; } = [];
        internal Dictionary<string, List<NameRow>> TypeByStored { get; private init; } = [];
        internal Dictionary<string, List<NameRow>> TypeByDelphi { get; private init; } = [];
        internal Dictionary<(string Enum, string Member), string> MemberCurrent { get; private init; } = [];
        internal Dictionary<(string Enum, string Member), string> MemberDelphi { get; private init; } = [];

        internal static Table Parse(string text)
        {
            List<NameRow> rows = [];
            bool header = false;
            foreach (string[] parts in Records(text))
            {
                if (parts.Length == 1 && (parts[0].Length == 0 || parts[0][0] == '#'))
                {
                    continue;
                }

                if (!header)
                {
                    bool known = parts.Length is 3 or 4
                        && parts[0] == "kind" && parts[1] == "delphi" && parts[2] == "current"
                        && (parts.Length == 3 || parts[3] == "stored");
                    if (!known)
                    {
                        throw new InvalidOperationException("The name table header is missing.");
                    }

                    header = true;
                    continue;
                }
                if (parts[0] is not ("type" or "member") || parts[1].Length == 0 || parts[2].Length == 0
                    || parts.Length is not (3 or 4))
                {
                    throw new InvalidOperationException("A name table row is not kind, delphi, current and an optional stored name.");
                }

                string storedName = parts.Length == 4 ? parts[3] : "";
                if (parts[0] == "member" && storedName.Length != 0)
                {
                    throw new InvalidOperationException("A member row has no stored name.");
                }

                rows.Add(new NameRow(parts[0], parts[1], parts[2], storedName));
            }
            return !header ? throw new InvalidOperationException("The name table header is missing.") : Index(rows);
        }

        // A line that starts with # is a comment. A field is quoted only when it holds a comma, a quote or a line break.
        private static List<string[]> Records(string text)
        {
            List<string[]> records = [];
            List<string> fields = [];
            StringBuilder field = new();
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
                            _ = field.Append('"');
                            i++;
                        }
                        else
                        {
                            quoted = false;
                        }
                    }
                    else
                    {
                        _ = field.Append(c);
                    }

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
                    {
                        throw new InvalidOperationException("A name table quote sits inside a field.");
                    }

                    quoted = true;
                    begin = false;
                    continue;
                }
                if (c == ',')
                {
                    fields.Add(field.ToString());
                    _ = field.Clear();
                    begin = false;
                    continue;
                }
                if (c == '\n')
                {
                    if (begin && field.Length == 0 && fields.Count == 0)
                    {
                        continue;
                    }

                    fields.Add(field.ToString());
                    _ = field.Clear();
                    records.Add([.. fields]);
                    fields.Clear();
                    begin = true;
                    continue;
                }
                _ = field.Append(c);
                begin = false;
            }
            if (quoted)
            {
                throw new InvalidOperationException("A quoted name table field is not closed.");
            }

            if (field.Length > 0 || fields.Count > 0)
            {
                fields.Add(field.ToString());
                records.Add([.. fields]);
            }
            return records;
        }

        private static Table Index(List<NameRow> rows)
        {
            Dictionary<string, NameRow> typeByCurrent = [];
            Dictionary<string, List<NameRow>> typeByDelphi = [];
            Dictionary<(string, string), string> memberCurrent = [];
            Dictionary<(string, string), string> memberDelphi = [];
            HashSet<string> seen = [];
            foreach (NameRow row in rows)
            {
                if (!seen.Add(row.Current))
                {
                    // The three engine classes share one current name and keep their Delphi names.
                    if (row.Kind == "type" && typeByCurrent.ContainsKey(row.Current))
                    {
                        Add(typeByDelphi, row.Delphi, row);
                        continue;
                    }
                    throw new InvalidOperationException($"The name table repeats {row.Current}.");
                }
                if (row.Kind == "type")
                {
                    typeByCurrent.Add(row.Current, row);
                    Add(typeByDelphi, row.Delphi, row);
                    continue;
                }
                if (row.Kind != "member")
                {
                    throw new InvalidOperationException("A name table row has an unknown kind.");
                }

                int currentDot = row.Current.LastIndexOf('.');
                int delphiDot = row.Delphi.LastIndexOf('.');
                if (currentDot <= 0 || delphiDot <= 0)
                {
                    throw new InvalidOperationException("A member row needs an enum name and a member name.");
                }

                string enumName = row.Current[..currentDot];
                string currentMember = row.Current[(currentDot + 1)..];
                string delphiMember = row.Delphi[(delphiDot + 1)..];
                if (!memberCurrent.TryAdd((enumName, delphiMember), currentMember))
                {
                    throw new InvalidOperationException($"The name table repeats member {row.Delphi}.");
                }

                if (!memberDelphi.TryAdd((enumName, currentMember), delphiMember))
                {
                    throw new InvalidOperationException($"The name table repeats member {row.Current}.");
                }
            }
            Dictionary<string, string> storedByCurrent = AssignStored(rows);
            Dictionary<string, List<NameRow>> typeByStored = [];
            foreach (NameRow row in rows)
            {
                if (row.Kind != "type")
                {
                    continue;
                }

                Add(typeByStored, storedByCurrent[row.Current], row);
            }
            return new Table
            {
                Rows = [.. rows],
                TypeByCurrent = typeByCurrent,
                StoredByCurrent = storedByCurrent,
                TypeByStored = typeByStored,
                TypeByDelphi = typeByDelphi,
                MemberCurrent = memberCurrent,
                MemberDelphi = memberDelphi,
            };
        }

        // A current name that is a dotted prefix of another current name is that type's owner.
        // The measurement against reflection found no prefix that is only a namespace.
        // Two types that share a stored name and differ in their Delphi names cannot both keep
        // the short name. The one whose Delphi name is already that short name keeps it.
        // The other gains namespace segments from the right until the stored name is free.
        // TPropertyTable is Engine.PropertyTable beside the formats type PropertyTable.
        private static Dictionary<string, string> AssignStored(List<NameRow> rows)
        {
            HashSet<string> names = [];
            Dictionary<string, string> delphiOf = [];
            foreach (NameRow row in rows)
            {
                if (row.Kind != "type")
                {
                    continue;
                }

                if (!delphiOf.TryAdd(row.Current, row.Delphi))
                {
                    continue;
                }

                _ = names.Add(row.Current);
            }
            Dictionary<string, string?> ownerOf = [];
            foreach (string name in names)
            {
                ownerOf[name] = Owner(name, names);
            }

            List<string> ordered = [with(names)];
            ordered.Sort((left, right) =>
            {
                int length = left.Length.CompareTo(right.Length);
                return length != 0 ? length : Cp1252.Compare(left, right);
            });
            Dictionary<string, string> stored = [];
            foreach (string name in ordered)
            {
                string? owner = ownerOf[name];
                stored[name] = owner is null ? Simple(name) : stored[owner] + name[owner.Length..];
            }
            HashSet<string> pinned = [];
            // A written stored name wins over the computed one and stays put while children take it as their prefix.
            foreach (NameRow row in rows)
            {
                if (row.Kind != "type" || row.Stored.Length == 0)
                {
                    continue;
                }

                if (!stored.ContainsKey(row.Current))
                {
                    continue;
                }

                stored[row.Current] = row.Stored;
                _ = pinned.Add(row.Current);
            }
            Dictionary<string, List<string>> groups = [];
            foreach (KeyValuePair<string, string> pair in stored)
            {
                if (!groups.TryGetValue(pair.Value, out List<string>? list))
                {
                    groups[pair.Value] = list = [];
                }

                list.Add(pair.Key);
            }
            foreach (KeyValuePair<string, List<string>> group in groups)
            {
                if (group.Value.Count < 2)
                {
                    continue;
                }

                HashSet<string> delphis = [];
                foreach (string member in group.Value)
                {
                    _ = delphis.Add(delphiOf[member]);
                }

                if (delphis.Count < 2)
                {
                    continue;
                }

                foreach (string member in group.Value)
                {
                    if (pinned.Contains(member) || delphiOf[member] == group.Key)
                    {
                        continue;
                    }

                    stored[member] = Qualify(member, group.Key, stored, names);
                    _ = pinned.Add(member);
                }
            }
            bool changed = true;
            while (changed)
            {
                changed = false;
                foreach (string name in ordered)
                {
                    if (pinned.Contains(name))
                    {
                        continue;
                    }

                    string? owner = ownerOf[name];
                    if (owner is null)
                    {
                        continue;
                    }

                    string next = stored[owner] + name[owner.Length..];
                    if (stored[name] == next)
                    {
                        continue;
                    }

                    stored[name] = next;
                    changed = true;
                }
            }
            Dictionary<string, string> seen = [];
            foreach (KeyValuePair<string, string> pair in stored)
            {
                if (!seen.TryGetValue(pair.Value, out string? first))
                {
                    seen[pair.Value] = delphiOf[pair.Key];
                    continue;
                }
                if (first != delphiOf[pair.Key])
                {
                    throw new InvalidOperationException($"The stored name {pair.Value} is shared by {first} and {delphiOf[pair.Key]}.");
                }
            }
            return stored;
        }

        private static string? Owner(string name, HashSet<string> names)
        {
            for (int dot = name.LastIndexOf('.'); dot > 0; dot = name.LastIndexOf('.', dot - 1))
            {
                if (names.Contains(name[..dot]))
                {
                    return name[..dot];
                }
            }

            return null;
        }

        private static string Qualify(string qualified, string baseStored, Dictionary<string, string> stored, HashSet<string> names)
        {
            string ns = NamespaceOf(qualified, names);
            string[] parts = ns.Length == 0 ? [] : ns.Split('.');
            HashSet<string> used = [with(stored.Values)];
            string candidate = baseStored;
            for (int i = parts.Length - 1; i >= 0; i--)
            {
                candidate = $"{parts[i]}.{candidate}";
                if (!used.Contains(candidate))
                {
                    return candidate;
                }
            }
            throw new InvalidOperationException($"The stored name {baseStored} has no free qualification for {qualified}.");
        }

        private static string NamespaceOf(string qualified, HashSet<string> names)
        {
            string[] parts = qualified.Split('.');
            int start = parts.Length - 1;
            for (int i = 1; i <= parts.Length; i++)
            {
                if (!names.Contains(string.Join('.', parts[..i])))
                {
                    continue;
                }

                start = i - 1;
                break;
            }
            return start <= 0 ? "" : string.Join('.', parts[..start]);
        }

        private static void Add(Dictionary<string, List<NameRow>> index, string key, NameRow row)
        {
            if (!index.TryGetValue(key, out List<NameRow>? list))
            {
                list = [];
                index.Add(key, list);
            }
            list.Add(row);
        }
    }
}

internal readonly record struct NameRow(string Kind, string Delphi, string Current, string Stored = "");
